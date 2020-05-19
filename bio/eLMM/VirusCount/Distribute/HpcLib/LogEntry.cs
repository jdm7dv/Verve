//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Hpc.Scheduler;
using Microsoft.Hpc.Scheduler.Properties;
using MBT.Escience;
using MBT.Escience.Parse;
using Bio.Util;

namespace MBT.Escience.HpcLib
{
    [Serializable]
    public class LogEntry : IComparable<LogEntry>
    {
        public event EventHandler OnJobStateChanged;
        public event EventHandler OnTaskStateChanged;

        [Flags]
        public enum UpdateResult
        {
            NoChange = 0, JobStateChanged = 1, TaskStatusChanged = 2
        };

        [XmlIgnore]
        public int LogIndex { get; set; }
        public DateTime Date { get; set; }
        public string LocalDir { get; set; }
        public ClusterSubmitterArgs ClusterArgs { get; set; }
        public bool CopiedToLocalDir { get; set; }
        public string TaskStatus { get; set; }
        public JobState JobState { get; set; }
        public int FailedTaskCount { get; set; }
        public string FailedTasks { get; set; }


        [XmlIgnore]
        public TimeSpan WallTime { get; set; }
        public string WallTimeForSaving
        {
            get { return WallTime.ToString(); }
            set { WallTime = TimeSpan.Parse(value); }
        }

        [XmlIgnore]
        public TimeSpan CpuTime { get; set; }
        public string CpuTimeForSaving
        {
            get { return CpuTime.ToString(); }
            set { CpuTime = TimeSpan.Parse(value); }
        }

        public int JobID { get { return ClusterArgs.JobID; } }
        public string JobName { get { return ClusterArgs.Name; } }
        public string Cluster { get { return ClusterArgs.Cluster; } }
        public string JobTemplate { get { return ClusterArgs.JobTemplate; } }
        public string ClusterDir { get { try { return ClusterArgs.ExternalRemoteDirectoryName; } catch (UnknownClusterException) { return ""; } } }
        public string StdOutDir { get { return ClusterArgs.StdOutDirName; } }
        public string StdErrDir { get { return ClusterArgs.StdErrDirName; } }






        public string FormattedCpuTime
        {
            get { return FormatTime(CpuTime); }
        }

        public string FormattedWallTime
        {
            get { return FormatTime(WallTime); }
        }



        public bool FailedOrCanceled
        {
            get
            {
                return this.JobState == JobState.Failed || JobState == JobState.Canceled || FailedTaskCount > 0 || !string.IsNullOrEmpty(FailedTasks);
            }
        }

        string _project;
        public string Project
        {
            get
            {
                if (_project == null && ClusterDir != "")
                    _project = new DirectoryInfo(ClusterDir).Name;
                return _project;
            }
        }




        private IScheduler _scheduler;
        private ISchedulerJob _job;


        public LogEntry() { }


        public LogEntry(ClusterSubmitterArgs clusterArgs)
        {
            Date = DateTime.Now;
            LocalDir = Environment.CurrentDirectory;
            if (clusterArgs.RelativeDir)
            {
                clusterArgs.Dir = clusterArgs.ExternalRemoteDirectoryName;
                clusterArgs.RelativeDir = false;
            }
            ClusterArgs = clusterArgs;
        }

        public static LogEntry GetInstanceFromJobID(string clustername, int jobID)
        {
            LogEntry entry = new LogEntry();
            entry.ClusterArgs = new ClusterSubmitterArgs();
            entry.ClusterArgs.Cluster = clustername;
            entry.ClusterArgs.JobID = jobID;

            entry.Connect();
            entry._job = entry.GetJob();
            entry.ClusterArgs.Name = entry._job.Name;
            entry.Date = entry._job.SubmitTime;

            ISchedulerTask exampleTask = entry._job.GetTaskList(null, null, true).Cast<ISchedulerTask>().First();
            entry.ClusterArgs.StdErrDirName = exampleTask.StdErrFilePath;
            entry.ClusterArgs.StdOutDirName = exampleTask.StdOutFilePath;
            entry.ClusterArgs.Dir = exampleTask.WorkDirectory;

            string clusterPath = entry.ClusterDir.ToLower();
            string rootClusterPath = Path.Combine(HpcLibSettings.KnownClusters[entry.Cluster].StoragePath, "carlson");
            string relativeDir = clusterPath.Replace(rootClusterPath.ToLower(), "");
            if (!relativeDir.StartsWith("\\"))
                relativeDir = "\\" + relativeDir;
            entry.LocalDir = @"d:\projects" + relativeDir;

            return entry;
        }

        public bool StartTrackingJob()
        {
            _scheduler = null;
            _job = null;
            if (HpcLibSettings.ActiveClusters.Contains(Cluster, StringComparer.CurrentCultureIgnoreCase) && Connect())
            {
                _job = GetJob();
                if (_job != null)
                {
                    _job.OnJobState += new EventHandler<JobStateEventArg>(_job_OnJobState);
                    _job.OnTaskState += new EventHandler<TaskStateEventArg>(_job_OnTaskState);
                    RefreshJobStatus();
                }
            }
            return _scheduler != null && _job != null;
        }

        void _job_OnTaskState(object sender, TaskStateEventArg e)
        {
            RefreshJobStatus();
        }

        void _job_OnJobState(object sender, JobStateEventArg e)
        {
            RefreshJobStatus();
        }

        public void RefreshJobStatus()
        {
            if (_job != null)
            {
                LogEntry oldVersion = (LogEntry)this.MemberwiseClone();

                _job.Refresh();
                JobState = _job.State;
                ISchedulerJobCounters counters = _job.GetCounters();
                string stateStr = string.Format("{0}/{1}/{2}/{3}", counters.QueuedTaskCount, counters.RunningTaskCount, counters.FailedTaskCount, counters.FinishedTaskCount);
                FailedTaskCount = counters.FailedTaskCount;
                TaskStatus = stateStr;
                if (FailedTaskCount > 0)
                {
                    IEnumerable<ISchedulerTask> tasklist = GetFailedTasks(_job);
                    string failedTaskRangeAsString = tasklist.Select(task => task.TaskId.JobTaskId).StringJoin(",");
                    if ("" != failedTaskRangeAsString)
                    {
                        this.FailedTasks = RangeCollection.Parse(failedTaskRangeAsString).ToString();
                    }
                    else
                    {
                        FailedTasks = "";
                    }
                }
                else
                {
                    FailedTasks = "";
                }

                if (JobState == JobState.Finished)
                {
                    if (WallTime.Ticks == 0)
                    {
                        DateTime startTime = _job.SubmitTime;
                        DateTime endTime = _job.EndTime;
                        WallTime = endTime - startTime;
                    }
                    if (CpuTime.Ticks == 0)
                    {
                        var tasklist = _job.GetTaskList(null, null, true).Cast<ISchedulerTask>();
                        var totalTicks = tasklist.Select(task => (task.EndTime - task.StartTime).Ticks).Sum();
                        CpuTime = new TimeSpan(totalTicks);
                    }
                }

                bool taskStateChanged = FailedTasks != oldVersion.FailedTasks || TaskStatus != oldVersion.TaskStatus;
                bool jobStateChanged = JobState != oldVersion.JobState ||
                    (FailedTaskCount == 0) != (oldVersion.FailedTaskCount == 0) ||
                    string.IsNullOrEmpty(FailedTasks) != string.IsNullOrEmpty(oldVersion.FailedTasks) ||
                    CpuTime != oldVersion.CpuTime || WallTime != oldVersion.WallTime;
                //if (_taskStateChangedSinceLastEvent != taskStateChanged || _jobStateChangedSinceLastEvent != jobStateChanged)
                //    Console.WriteLine("bad");

                if (taskStateChanged)
                    RaiseTaskStateChangedEvent();
                if (jobStateChanged)
                    RaiseJobStateChangedEvent();

            }
        }

        private void RaiseJobStateChangedEvent()
        {
            //_jobStateChangedSinceLastEvent = false;
            if (OnJobStateChanged != null)
            {
                OnJobStateChanged(this, new EventArgs());
            }
        }

        private void RaiseTaskStateChangedEvent()
        {
            //_taskStateChangedSinceLastEvent = false;
            if (OnTaskStateChanged != null)
            {
                OnTaskStateChanged(this, new EventArgs());
            }
        }

        /// <summary>
        /// Refreshes both the job state and the task status
        /// </summary>
        /// <returns>Flags specifying whether the JobStatus and/or TaskState changed.</returns>
        public UpdateResult RefreshJobStatus_old()
        {
            UpdateResult result = UpdateResult.NoChange;
            if ((string.IsNullOrEmpty(TaskStatus) ||
                    JobState != JobState.Finished && JobState != JobState.Canceled && JobState != JobState.Failed) &&
                _job != null)
            //CCSLibSettings.ActiveClusters.Contains(Cluster, StringComparer.CurrentCultureIgnoreCase) &&
            //Connect())
            {
                try
                {
                    _job.Refresh();
                    //ISchedulerJob job = GetJob();
                    //if (job == null) return result;

                    if (JobState != _job.State)
                    {
                        JobState = _job.State;
                        result |= UpdateResult.JobStateChanged;
                    }
                    ISchedulerJobCounters counters = _job.GetCounters();
                    string stateStr = string.Format("{0}/{1}/{2}/{3}", counters.QueuedTaskCount, counters.RunningTaskCount, counters.FailedTaskCount, counters.FinishedTaskCount);

                    if (FailedTaskCount != counters.FailedTaskCount || (string.IsNullOrEmpty(FailedTasks) != (FailedTaskCount == 0)))
                    {
                        UpdateFailedTaskString(_job);
                        result |= UpdateResult.TaskStatusChanged;
                    }

                    FailedTaskCount = counters.FailedTaskCount;
                    if (stateStr != TaskStatus)
                    {
                        TaskStatus = stateStr;
                        result |= UpdateResult.TaskStatusChanged;
                    }
                }
                catch
                {
                    // probably a problem with the scheduler.
                    _scheduler = null;
                    return result;
                }
            }
            return result;
        }

        static object _getJobLocker = new object();
        private ISchedulerJob GetJob()
        {
            lock (_getJobLocker)
            {
                HpcLib.TryGetJob(_scheduler, ClusterArgs.Username, ClusterArgs.JobID, out _job);
                return _job;
            }
            //IFilterCollection filter = _scheduler.CreateFilterCollection();
            //filter.Add(FilterOperator.Equal, PropId.Job_Name, ClusterArgs.RunName);
            //IIntCollection potentialIDs = _scheduler.GetJobIdList(filter, null);
            //var matchingIDs = potentialIDs.Where(id => id == ClusterArgs.JobID);
            //if (matchingIDs.Count() == 1)
            //{
            //    int id = matchingIDs.First();
            //    ISchedulerJob job = _scheduler.OpenJob(id);
            //    return job;
            //}
            //return null;
        }



        private void UpdateFailedTaskString(ISchedulerJob job)
        {
            IEnumerable<ISchedulerTask> tasklist = GetFailedTasks(job);
            this.FailedTasks = tasklist.Select(task => task.TaskId.JobTaskId).StringJoin(",");
        }

        private List<ISchedulerTask> GetFailedTasks(ISchedulerJob job)
        {

            IFilterCollection filters = _scheduler.CreateFilterCollection();
            filters.Add(FilterOperator.Equal, PropId.Task_State, TaskState.Failed);
            var failedTasks = job.GetTaskList(filters, null, true).Cast<ISchedulerTask>().ToList();

            return failedTasks;
        }

        /// <summary>
        /// Requeues all failed or canceled tasks.
        /// </summary>
        /// <returns>True iff all tasks were successfully requeued.</returns>
        public bool RequeueFailedTasks()
        {
            if (_job != null)
            {
                try
                {
                    //ISchedulerJob job = GetJob();
                    _job.Refresh();
                    if (_job.State == JobState.Running)
                    {
                        var failedTasks = GetFailedTasks(_job);
                        foreach (ISchedulerTask task in failedTasks)
                        {
                            _job.RequeueTask(task.TaskId);
                        }
                        FailedTaskCount = 0;
                        FailedTasks = "";
                        return true;
                    }
                    else
                    {
                        _scheduler.ConfigureJob(_job.Id);
                        _scheduler.SubmitJob(_job, null, null);
                    }
                }
                catch
                {

                }
            }
            return false;
        }



        private bool Connect()
        {
            if (_scheduler != null) return true;

            return HpcLib.TryConnect(Cluster, out _scheduler);

        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Date, Cluster, JobName, JobState);
        }

        public static void SaveEntries(List<LogEntry> entries, string logFileName)
        {
            FileStream filestream;
            if (MBT.Escience.FileUtils.TryToOpenFile(logFileName, new TimeSpan(0, 3, 0), FileMode.Create, FileAccess.ReadWrite, FileShare.None, out filestream))
            {
                using (TextWriter writer = new StreamWriter(filestream))
                {
                    SaveEntries(entries, writer);
                }
            }
        }

        public static void SaveEntries(List<LogEntry> entries, TextWriter writer)
        {
            entries.Sort();
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<LogEntry>));
            serializer.Serialize(writer, entries);
        }
        /// <summary>
        /// Loads the log entries from the specified file name. If the file doesn't exist,
        /// returns an empty list.
        /// </summary>
        /// <param name="logFileName"></param>
        /// <param name="maxWaitTime">If we can't get a lock on the file in this amount of time, a TimeoutException will be thrown.</param>
        /// <exception cref="TimeoutException">If a lock can't be obtained in maxWaitTime</exception>
        /// <exception cref="IOException">If the file doesn't exist.</exception>
        /// <returns></returns>
        public static List<LogEntry> LoadEntries(string logFileName, TimeSpan maxWaitTime)
        {
            FileInfo file = new FileInfo(logFileName);
            if (!file.Exists || file.Length == 0)
            {
                return new List<LogEntry>();
            }

            FileStream filestream;
            if (MBT.Escience.FileUtils.TryToOpenFile(logFileName, maxWaitTime, FileMode.Open, FileAccess.Read, FileShare.Read, out filestream))
            {
                using (TextReader reader = new StreamReader(filestream))
                {
                    return LoadEntries(reader);
                }
            }
            if (!File.Exists(logFileName))
                throw new IOException("File " + logFileName + " does not exist.");
            else
                throw new TimeoutException("Unable to open log file.");
        }

        public static List<LogEntry> LoadEntries(TextReader reader)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<LogEntry>));
            List<LogEntry> result = (List<LogEntry>)serializer.Deserialize(reader);
            result.Sort();
            //result.ForEach(entry => entry.PostDeserializationInitialize());
            return result;
        }

        //private void PostDeserializationInitialize()
        //{
        //    this._jobStateChangedSinceLastEvent = false;
        //    this._taskStateChangedSinceLastEvent = false; 
        //}


        #region IComparable<LogEntry> Members

        public int CompareTo(LogEntry other)
        {
            return other.Date.CompareTo(this.Date);
        }

        #endregion

        #region FirstTry

        //public static List<LogEntry> LoadEntries(string logFileName)
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(new XmlTextReader(logFileName));
        //    List<LogEntry> entries = new List<LogEntry>();

        //    foreach (XmlNode node in doc.GetElementsByTagName("Submission"))
        //    {
        //        LogEntry entry = new LogEntry((XmlElement)node);
        //        entries.Add(entry);
        //    }

        //    entries.Sort((e1, e2) => e1.Date.CompareTo(e2.Date));

        //    return entries;
        //}

        //public XmlElement GetXmlElement(XmlDocument doc)
        //{
        //    XmlElement element = doc.CreateElement("Submission");

        //    element.SetAttribute("date", Date.ToString());
        //    element.SetAttribute("localDir", LocalDir);
        //    //element.InnerText = ClusterArgs.ToString();
        //    XmlElement clusterArgsElt = CreateClusterArgsXmlElement(doc);
        //    element.AppendChild(clusterArgsElt);

        //    return element;
        //}

        //private XmlElement CreateClusterArgsXmlElement(XmlDocument doc)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    XmlSerializer serializer = new XmlSerializer(typeof(ClusterSubmitterArgs));
        //    serializer.Serialize(ms, ClusterArgs);
        //    ms.Position = 0;

        //    XmlDocument caDoc = new XmlDocument();
        //    caDoc.Load(new XmlTextReader(ms));
        //    XmlElement result = (XmlElement)doc.ImportNode(caDoc.GetElementsByTagName("ClusterSubmitterArgs")[0], true);
        //    return result;
        //}

        //private ClusterSubmitterArgs DeserializeClusterArgs(string xmlString)
        //{
        //    StringReader sr = new StringReader(xmlString);
        //    XmlSerializer serializer = new XmlSerializer(typeof(ClusterSubmitterArgs));
        //    ClusterSubmitterArgs clusterArgs = (ClusterSubmitterArgs)serializer.Deserialize(sr);
        //    return clusterArgs;
        //}
        #endregion


        public string CancelJob()
        {
            if (_job != null)
            {
                try
                {
                    _scheduler.CancelJob(_job.Id, "Cancelled from Cluster Log Viewer.");
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
            return "";
        }

        private string FormatTime(TimeSpan time)
        {
            return string.Format("{0} {1:00}:{2:00}:{3:00}", time.Days > 0 ? time.Days + "d" : "", time.Hours, time.Minutes, time.Seconds);
        }
    }
}
