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
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Linq;
using MBT.Escience;
using MBT.Escience.Parse;
using v1 = Microsoft.ComputeCluster;
using v2008R2 = Microsoft.Hpc.Scheduler;
using Bio.Util;
using System.Web;
using Microsoft.Hpc.Scheduler.Properties;
using MBT.Escience.Distribute;
using System.Threading.Tasks;

namespace MBT.Escience.HpcLib
{
    /// <summary>
    /// This class allows other exes to easily submit themselves to the cluster.
    /// </summary>
    /// <remarks>
    /// Typical usage: in main class, test for the optional argument -cluster. If found, call this method. Example from PhyloD:
    /// <code>
    ///if (ArgumentCollection.ExtractOptionalFlag("cluster"))
    ///{
    ///    ClusterSubmitter.Submit(args, CheckForSkipFileAndTabulateFile, PhyloDArgs.Validate);
    ///    return;
    ///}
    ///</code>
    /// </remarks>
    public static class ClusterSubmitter
    {
        public static object _submitterLockObj = new object();

        /// <summary>
        /// Calls the corresponding Submit function, but waits for the cluster to Finish, Fail, or be Canceled. If the final state is
        /// Finished, returns silently. Otherwise, it throws and Exception. For a description of the other parameters, see Submit().
        /// *** NOTE: ONLY WORKS WITH V2 CLUSTERS. ****
        /// </summary>
        public static ClusterSubmitterArgs SubmitAndWait(ArgumentCollection argumentCollection, int maxSubmitAfterTasksFail = 0)
        {
            if (argumentCollection.PeekOptional<string>("cluster", "help").Equals("help", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine("");
                Console.WriteLine(HelpMessage);
                return null;
            }

            ClusterSubmitterArgs clusterArgs = new ClusterSubmitterArgs(argumentCollection);
            SubmitAndWait(clusterArgs, argumentCollection, maxSubmitAfterTasksFail);
            return clusterArgs;
        }

        /// <summary>
        /// Calls the corresponding Submit function, but waits for the cluster to Finish, Fail, or be Canceled. If the final state is
        /// Finished, returns silently. Otherwise, it throws and Exception. For a description of the other parameters, see Submit().
        /// *** NOTE: ONLY WORKS WITH V2 CLUSTERS. ****
        /// </summary>
        public static void SubmitAndWait(ClusterSubmitterArgs clusterArgs, ArgumentCollection applicationArgs, int maxSubmitAfterTasksFail = 0)
        {
            CommandArguments cmd = applicationArgs as CommandArguments;
            Helper.CheckCondition<ArgumentException>(cmd != null, "Can only provide command arguments to the cluster submitter");

            SubmitAndWait(clusterArgs, new DistributableWrapper(cmd), maxSubmitAfterTasksFail: maxSubmitAfterTasksFail);
        }

        public static void SubmitAndWait(ClusterSubmitterArgs clusterArgs, IDistributable distributableObj, int maxSubmitAfterTasksFail = 0)
        {

            using(ParallelOptionsScope.Suspend())
            {
                int numberOfTries = 0;

            retry:

                Submit(clusterArgs, distributableObj);

                JobWaitingParams jobWaitingParams = WaitForJobInternal(clusterArgs);

                if (jobWaitingParams.JobState == v2008R2.Properties.JobState.Canceled)
                {
                    throw new Exception("Job canceled.");
                }
                else if (jobWaitingParams.JobState == v2008R2.Properties.JobState.Failed)
                {
                    if (numberOfTries < maxSubmitAfterTasksFail)
                    {
                        ++numberOfTries;
                        Console.WriteLine("Job failed, trying again...");
                        goto retry;
                    }
                    throw new Exception("Job failed.");
                }
                //HpcLib.HpcLib.CopyFiles(new List<String> { "" }, _remoteTaskOutDir, TASK_OUT_DIR);
            }
        }

        /// <summary>
        /// Waits for the job specified by clusterArgs to finish. If the state is Canceled or Failed,
        /// will throw an exception. Attempts to connect to the cluster if not already connected.
        /// </summary>
        /// <param name="clusterArgs"></param>
        public static void WaitForJob(ClusterSubmitterArgs clusterArgs)
        {
            JobWaitingParams jobWaitingParams = WaitForJobInternal(clusterArgs);

            if (jobWaitingParams.JobState == v2008R2.Properties.JobState.Canceled)
            {
                throw new Exception("Job canceled.");
            }
            else if (jobWaitingParams.JobState == v2008R2.Properties.JobState.Failed)
            {
                throw new Exception("Job failed.");
            }
        }

        private static JobWaitingParams WaitForJobInternal(ClusterSubmitterArgs clusterArgs)
        {
            v2008R2.ISchedulerJob job = clusterArgs.GetV2Job();
            var jobState = job.State;
            //clusterArgs.JobV2.Refresh();
            //clusterArgs.JobState = clusterArgs.JobV2.State;

            JobWaitingParams jobWaitingParams = new JobWaitingParams
            {
                Job = job,
                JobState = jobState,
                ManualResetEvent = new ManualResetEvent(false)
            };


            SetupJobEventHandler(jobWaitingParams);

            int heartBeatPeriod = 60 * 1000; // beat once a minute

            // put in a using statement to guarantee dispose will be called and the timer will be shutdown.
            using (Timer timer = HeartbeatTimer(clusterArgs.JobID, clusterArgs.Cluster, jobWaitingParams, heartBeatPeriod))
            {
                //wait
                jobWaitingParams.Job.Refresh();
                if (!JobIsFinished(jobWaitingParams.Job.State))
                    jobWaitingParams.ManualResetEvent.WaitOne();
                timer.Change(Timeout.Infinite, Timeout.Infinite);   // shutdown the timer
            }
            return jobWaitingParams;
        }

        private static bool JobIsFinished(v2008R2.Properties.JobState state)
        {
            //return state == v2.Properties.JobState.Finished || state == v2.Properties.JobState.Canceled || state == v2.Properties.JobState.Failed;
            if (state == v2008R2.Properties.JobState.Canceled) Console.WriteLine("N.B. Job has been cancelled, but program is continuing to wait. Requeue or kill app or wait forever.");
            return state == v2008R2.Properties.JobState.Finished || state == v2008R2.Properties.JobState.Failed;
        }

        private static Timer HeartbeatTimer(int jobID, string clusterName, JobWaitingParams jobWaitingParams, int heartBeatPeriod)
        {
            Timer timer = new Timer(state =>
            {
                try
                {
                    jobWaitingParams.Job.Refresh();
                    //Console.WriteLine("Job is still connected. Status is " + jobWaitingParams.JobState);
                }
                catch
                {
                    Console.WriteLine("Lost connection to job. Attempting reconnect.");
                    v2008R2.IScheduler scheduler = null;
                    for (int iTry = 0; iTry < 10 && scheduler == null; iTry++)
                    {
                        HpcLib.TryConnect(clusterName, out scheduler);
                    }
                    if (scheduler == null)
                    {
                        Console.WriteLine("Unable to reconnect to cluster. Going back to sleep.");
                    }
                    else
                    {
                        jobWaitingParams.Job = scheduler.OpenJob(jobID);
                        jobWaitingParams.Job.Refresh();
                        SetupJobEventHandler(jobWaitingParams);
                        Console.WriteLine("Reconnect succeeded.");
                    }
                }

                jobWaitingParams.JobState = jobWaitingParams.Job.State;

                if (JobIsFinished(jobWaitingParams.Job.State))
                {
                    jobWaitingParams.ManualResetEvent.Set();
                }
            }, null, heartBeatPeriod, heartBeatPeriod);

            return timer;
        }

        private static void SetupJobEventHandler(JobWaitingParams jobWaitingParams)
        {
            jobWaitingParams.Job.OnJobState += (sender, args) =>
            {
                if (JobIsFinished(args.NewState))
                {
                    jobWaitingParams.JobState = args.NewState;
                    jobWaitingParams.ManualResetEvent.Set();
                }
            };
        }

        private class JobWaitingParams
        {
            public v2008R2.ISchedulerJob Job;
            public v2008R2.Properties.JobState JobState;
            public ManualResetEvent ManualResetEvent;
        }


        /// <summary>
        /// Submits the ArgumentCollection to the cluster, telling the cluster to run whichever exe is currently running using a new set of args that divids the work up in to tasks. 
        /// IMPORTANT: For this to work, the calling exe needs to except two OPTIONAL args:
        ///     1) -TaskCount n  which tells the exe how many pieces to divide the work in to
        ///     2) -Tasks i which tells the exe which piece that instance is in change of. Submit() will create TaskCount instances of the exe and do a parameter sweek on Tasks
        /// For the complete list of optional and required args, see the HelpString.
        /// </summary>
        /// <param name="argumentCollection">The set of args, including the args for the exe and the args for the cluster submission, all of which are encoded as optional args (though some are required)</param>
        /// <param name="funcToCheckForSkipFileAndReturnFalseIfShouldNotRunOrNull">If you supply this function, it should check to see if there is a skip file that needs to be created and
        /// check to see if this job should actually be submitted (if so, return true, otherwise, false). This is useful to see if the results are already computed and if a completedRows
        /// file should be made into a skipFile automatically. See PhyloDMain.cs for an example.  Note that the out parameter is the ArgumentCollection that will actually be used to run the jobs.
        /// This is so the function can add in a new SkipFile parameter if needed. Also, be careful: often times checking to see if you need a skipFile requires reading arguments from
        /// the input args, which will modify that object. Your first step should be to Clone args and asign the result to argsToUseWithPossiblyUpdatedSkipFile.</param>
        /// <param name="funcToValidateParamsOrNull">This method returns true if the args that are generated by Submit do in fact result in valid arguments for the calling exe.
        /// This saves you from having to wait for failures on the cluster to know that something is wrong. Only the first task is checked. If it passes, all others are assumed
        /// to be valid. If this function is not supplied (is null), then no checks are made.
        /// </param>
        public static ClusterSubmitterArgs Submit(ArgumentCollection argumentCollection)
        {
            if (argumentCollection.PeekOptional<string>("cluster", "help").Equals("help", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine("");
                Console.WriteLine(HelpMessage);
                return null;
            }

            ClusterSubmitterArgs clusterArgs = new ClusterSubmitterArgs(argumentCollection);
            Submit(clusterArgs, argumentCollection);
            return clusterArgs;
        }

        public static void Submit(ClusterSubmitterArgs clusterArgs, ArgumentCollection applicationArgs)
        {
            CommandArguments cmd = applicationArgs as CommandArguments;
            Helper.CheckCondition<ArgumentException>(cmd != null, "Can only provide command arguments to the cluster submitter");
            Submit(clusterArgs, new DistributableWrapper(cmd));
        }

        public static void Submit(ClusterSubmitterArgs clusterArgs, IDistributable distributableObj)
        {
            for (int numTries = 0; numTries < clusterArgs.MaxSubmitTries; numTries++)
            {
                try
                {
                    SubmitInternal(clusterArgs, distributableObj);
                    return;
                }
                catch (Exception exception)
                {
                    Console.WriteLine("\n\nError submitting to cluster " + clusterArgs.Cluster + ": " + exception.Message);
                    Console.WriteLine("numTry=" + numTries + " of " + clusterArgs.MaxSubmitTries);
                    Console.WriteLine(exception.StackTrace);
                    Console.WriteLine("\n\nUse -cluster help to see usage.");
                    Thread.Sleep(new TimeSpan(0, 10, 0));
                }
            }
            throw new Exception("max number of cluster submitter tries (" + clusterArgs.MaxSubmitTries + ") exceeded");
        }

        private static void SubmitInternal(ClusterSubmitterArgs clusterArgs, IDistributable distributableObj)
        {
            lock (_submitterLockObj)  // for now, just let one thread submit at a time.
            {
                if (clusterArgs.Archive != null)
                {
                    MBT.Escience.FileUtils.ArchiveExes(clusterArgs.Archive);
                }

                //ArgumentCollection argsToUse = (ArgumentCollection)applicationArgs.Clone();

                CopyExes(clusterArgs);

                clusterArgs.StdErrDirName = CreateUniqueDirectory(clusterArgs.ExternalRemoteDirectoryName, "Stderr", clusterArgs.Name);
                clusterArgs.StdOutDirName = CreateUniqueDirectory(clusterArgs.ExternalRemoteDirectoryName, "Stdout", clusterArgs.Name);


                if (clusterArgs.CopyInputFiles.Count > 0)
                {
                    CopyInputFiles(clusterArgs.CopyInputFiles, clusterArgs.ExternalRemoteDirectoryName);
                }

                using (ParallelOptionsScope.Suspend())
                {
                    switch (clusterArgs.Version)
                    {
                        case 1:
                            SubmitViaAPI1(clusterArgs, distributableObj);
                            break;
                        case 2:
                            Console.Error.WriteLine("Api2 and 3 are the same. Submitting via Api3.");
                            SubmitViaAPI3(clusterArgs, distributableObj);
                            break;
                        case 3:
                            SubmitViaAPI3(clusterArgs, distributableObj);
                            break;
                        default:
                            throw new NotSupportedException(string.Format("Cluster version {0} is not supported.", clusterArgs.Version));
                    }
                }
                Console.WriteLine("Processed job to cluster {0} with path {1}", clusterArgs.Cluster, clusterArgs.ExternalRemoteDirectoryName);


                Console.WriteLine("Writing log file");
                HpcLibSettings.TryWriteToLog(clusterArgs);
                Console.WriteLine("Writing log entry to cluster directory");
                HpcLibSettings.WriteLogEntryToClusterDirectory(clusterArgs);
                Console.WriteLine("Done");
            }
            return;
        }

        private static void CopyExes(ClusterSubmitterArgs clusterArgs)
        {
            //!! why was this removed??
            //var lockObj = CCSLibSettings.KnownClusters[clusterArgs.Cluster]; // use an object that is specific to this cluster.
            //lock (lockObj)  // we should only have one thread trying to copy to the cluster at a time. Esp since copying is smart and will reuse identical exes if they're already there.
            {
                if (true || clusterArgs.ExeRelativeDirectoryName == null)
                {
                    clusterArgs.ExeRelativeDirectoryName = HpcLib.CopyExesToCluster(clusterArgs.ExternalRemoteDirectoryName, clusterArgs.Name);
                }
                else
                {
                    Console.WriteLine("Using exe directory specified by user: " + clusterArgs.ExeRelativeDirectoryName);
                    string absoluteExeDir = clusterArgs.ExternalRemoteDirectoryName + "\\" + clusterArgs.ExeRelativeDirectoryName;
                    Helper.CheckCondition(Directory.Exists(absoluteExeDir), "Directory {0} does not exist!", absoluteExeDir);
                    //string exeRelativeName = "exes\\" + clusterArgs.ExeRelativeDirectoryName;
                    //clusterArgs.ExeRelativeDirectoryName = "\"" + exeRelativeName + "\"";
                }
            }
        }

        private static void OpenExplorerWindowAtCluster(string externalRemoteDirectoryName)
        {
            Process explorer = new Process();
            explorer.StartInfo.FileName = "explorer";
            explorer.StartInfo.Arguments = externalRemoteDirectoryName;
            explorer.Start();
        }

        private static void OpenJobConsole(string clusterName)
        {
            Process jobConsole = new Process();
            jobConsole.StartInfo.FileName = "HpcJobManager";
            jobConsole.StartInfo.Arguments = clusterName;
            jobConsole.Start();
        }


        public static void CopyInputFiles(List<string> filesToCopy, string baseDestnDir)
        {
            HpcLib.CopyFiles(filesToCopy, "", baseDestnDir);
            //string xcopyArgsFormatString = GetXCopyArgsFormatString(filesToCopy[0], baseDestnDir);

            //foreach (string file in filesToCopy)
            //{
            //    Process proc = new Process();
            //    proc.StartInfo.FileName = "xcopy";
            //    proc.StartInfo.Arguments = string.Format(xcopyArgsFormatString, file);
            //    proc.StartInfo.UseShellExecute = false;
            //    //proc.StartInfo.RedirectStandardOutput = true;
            //    //Console.WriteLine("xcopy " + proc.StartInfo.Arguments);
            //    proc.Start();
            //    proc.WaitForExit();
            //    if (proc.ExitCode > 0)
            //        throw new Exception("Unable to copy file using command xcopy " + proc.StartInfo.Arguments); 
            //}
        }

        //private static string GetXCopyArgsFormatString(string exampleFileToCopy, string baseDestnDir)
        //{
        //    string relativeDirName = FileUtils.GetDirectoryName(exampleFileToCopy);
        //    string outputDir = Path.Combine(baseDestnDir, relativeDirName);

        //    if(!Directory.Exists(outputDir))
        //        Directory.CreateDirectory(outputDir);

        //    string args = "/d /c /i \"{0}\" \"" + outputDir + "\"";
        //    return args;
        //}



        //private static List<string> GetExclusionNodes(string nodeExclusionList, int apiLevel)
        //{
        //    List<string> nodesToExclude = new List<string>();
        //    if (null != nodeExclusionList)
        //    {
        //        throw new Exception("never managed to get this to work");
        //        if (apiLevel == 1) throw new Exception("cannot use -nodeExclusionList with version 1 API");
        //        nodesToExclude = nodeExclusionList.Split(',').ToList();
        //    }
        //    return nodesToExclude;
        //}

        private static void SetPriority(ArgumentCollection argumentCollection, int apiLevel, out v1.JobPriority priorityV1, out v2008R2.Properties.JobPriority priorityV2)
        {
            priorityV1 = v1.JobPriority.Normal;
            priorityV2 = v2008R2.Properties.JobPriority.Normal;
            switch (apiLevel)
            {
                case 1:
                    priorityV1 = argumentCollection.ExtractOptional<v1.JobPriority>("priority", v1.JobPriority.Normal);
                    break;
                case 2:
                    priorityV2 = argumentCollection.ExtractOptional<v2008R2.Properties.JobPriority>("priority", v2008R2.Properties.JobPriority.Normal);
                    break;
                default:
                    throw new NotSupportedException(string.Format("Cluster version {0} is not supported.", apiLevel));
            }
        }



        #region API 1

        private static void SubmitViaAPI1(ClusterSubmitterArgs clusterArgs, IDistributable distributableObj)
        {
            Console.WriteLine("Submitting using API version 1");

            v1.ICluster cluster = new v1.Cluster();
            cluster.Connect(clusterArgs.Cluster);


            foreach (v1.ITask task in EnumerateTasks(clusterArgs, distributableObj))
            {
                v1.IJob job = CreateJobApi1(cluster, (v1.JobPriority)clusterArgs.ApiPriority, task, task.Name);
                cluster.QueueJob(job, clusterArgs.Username, clusterArgs.Password, true, 0);
            }
            Console.WriteLine();
        }

        private static IEnumerable<v1.ITask> EnumerateTasks(ClusterSubmitterArgs clusterArgs, IDistributable distributableObj)
        {
            //bool checkIfValid = ValidateParamsOrNull != null;


            for (int pieceIndex = 0; pieceIndex < clusterArgs.TaskCount; ++pieceIndex)
            {
                if (clusterArgs.TaskRange.Contains(pieceIndex))
                {
                    ArgumentCollection thisTasksArgs;
                    //if (TryCreateTaskArgsAndValidate(args,  pieceIndex.ToString(), out thisTasksArgs))
                    {
                        v1.ITask task = CreateTask(clusterArgs, pieceIndex, distributableObj);
                        yield return task;
                    }
                }
            }
        }


        private static v1.IJob CreateJobApi1(v1.ICluster cluster, v1.JobPriority priority, v1.ITask task, string runName)
        {
            v1.IJob job = cluster.CreateJob();

            job.Name = runName; // Helper.CreateDelimitedString(" ", runName, firstPieceIndex + "-" + lastPieceIndex);
            Helper.CheckCondition(job.Name.Length < 80, "Job name is too long. Must be < 80 but is " + job.Name.Length + ". " + job.Name);
            Console.Write("\r" + job.Name);

            job.AddTask(task);
            job.Runtime = task.Runtime;
            job.IsExclusive = false;
            job.MinimumNumberOfProcessors = 1;
            job.MaximumNumberOfProcessors = 1;
            job.Priority = priority;

            return job;
        }

        private static v1.ITask CreateTask(ClusterSubmitterArgs clusterArgs, int taskNum, IDistributable distributableObj)
        {
            v1.ITask task = new v1.Task();
            task.WorkDirectory = clusterArgs.InternalRemoteDirectory;

            Distribute.Locally local = new Distribute.Locally()
            {
                Cleanup = false,
                TaskCount = clusterArgs.TaskCount,
                ParallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 1 },
                Tasks = new RangeCollection(taskNum)
            };

            Distribute.Distribute distributeExe = new Distribute.Distribute()
            {
                Distributable = distributableObj,
                Distributor = local
            };

            string taskString = CreateTaskString(distributeExe, clusterArgs.MinimalCommandLine);
            string exeName = distributableObj is DistributableWrapper ? clusterArgs.ExeName : distributeExe.GetType().Assembly.GetName().Name;

            string taskCommandLine = string.Format("{0}\\{1} {2}", clusterArgs.ExeRelativeDirectoryName, exeName, taskString);
            task.CommandLine = taskCommandLine;

            task.Name = Helper.CreateDelimitedString(" ", clusterArgs.Name, taskNum);
            task.IsExclusive = false;
            task.MinimumNumberOfProcessors = 1;
            task.MaximumNumberOfProcessors = 1;

            task.Stderr = string.Format(@"{0}\{1}.txt", clusterArgs.StdErrDirName, taskNum);
            task.Stdout = string.Format(@"{0}\{1}.txt", clusterArgs.StdOutDirName, taskNum);

            task.Runtime = "Infinite";
            return task;
        }



        #endregion




        #region API 3

        private static void SubmitViaAPI3(ClusterSubmitterArgs clusterArgs, IDistributable distributableObj)
        {
            Console.WriteLine(string.Format("Connecting to cluster {0} using API version 3 .", clusterArgs.Cluster));

            using (v2008R2.IScheduler scheduler = new v2008R2.Scheduler())
            {
                scheduler.Connect(clusterArgs.Cluster);
                v2008R2.ISchedulerJob job = scheduler.CreateJob();
                job.Name = clusterArgs.Name;
                job.Priority = (v2008R2.Properties.JobPriority)clusterArgs.ApiPriority;

                if (clusterArgs.JobTemplate != null)
                {
                    Microsoft.Hpc.Scheduler.IStringCollection jobTemplates = scheduler.GetJobTemplateList();
                    string decodedJobTemplate = HttpUtility.UrlDecode(clusterArgs.JobTemplate);
                    if (jobTemplates.Contains(decodedJobTemplate))
                    {
                        job.SetJobTemplate(decodedJobTemplate);
                    }
                    else
                    {
                        Console.WriteLine("Job template '" + decodedJobTemplate + "' does not exist at specified cluster. Existing templates are:");
                        foreach (var template in jobTemplates)
                        {
                            Console.Write("'" + template + "' ");
                        }
                        Console.WriteLine("\nUsing Default job template...");
                    }
                }


                if (clusterArgs.NumCoresPerTask != null)
                {
                    clusterArgs.IsExclusive = false;
                }

                v2008R2.IStringCollection nodesToUse = null;

                if (clusterArgs.NodeExclusionList != null && clusterArgs.NodeExclusionList.Count > 0)
                {
                    nodesToUse = GetNodesToUse(clusterArgs, scheduler, job);
                }
                else if (clusterArgs.NodesToUseList != null && clusterArgs.NodesToUseList.Count > 0)
                {
                    nodesToUse = scheduler.CreateStringCollection();
                    foreach (string nodeName in clusterArgs.NodesToUseList)
                    {
                        nodesToUse.Add(nodeName);
                    }
                }
                else if (clusterArgs.NumCoresPerTask != null)
                {
                    job.AutoCalculateMax = true;
                    job.AutoCalculateMin = true;
                }
                else if (clusterArgs.IsExclusive)
                {
                    job.UnitType = Microsoft.Hpc.Scheduler.Properties.JobUnitType.Node;
                    if (clusterArgs.MinimumNumberOfNodes != null)
                    {
                        job.MaximumNumberOfNodes = clusterArgs.MaximumNumberOfNodes.Value;
                        job.MinimumNumberOfNodes = clusterArgs.MinimumNumberOfNodes.Value;
                    }
                }
                else if (clusterArgs.MinimumNumberOfCores != null)
                {
                    job.MaximumNumberOfCores = clusterArgs.MaximumNumberOfCores.Value;
                    Helper.CheckCondition(clusterArgs.MinimumNumberOfCores != null, "must provide both MinCores and MaxCores, not just one");
                    job.MinimumNumberOfCores = clusterArgs.MinimumNumberOfCores.Value;
                    job.AutoCalculateMax = false;
                    job.AutoCalculateMin = false;
                }
                else
                {
                    job.AutoCalculateMax = true;
                    job.AutoCalculateMin = true;
                }


                //bool checkIfValid = ValidateParamsOrNull != null;

                if (!clusterArgs.OnlyDoCleanup)
                {

                    if (clusterArgs.TaskRange.IsContiguous())
                    {
                        if (clusterArgs.TaskRange.LastElement > clusterArgs.TaskCount - 1)
                            clusterArgs.TaskRange = new RangeCollection(clusterArgs.TaskRange.FirstElement, clusterArgs.TaskCount - 1);
                        v2008R2.ISchedulerTask task = CreateTask(null, clusterArgs, job, distributableObj, nodesToUse);

                        task.IsParametric = true; // IsParametric is marked as obsolete. But is it necessary to submit to a v2 cluster??

                        //task.Type = TaskType.ParametricSweep;

                        task.StartValue = 0;
                        task.EndValue = clusterArgs.TaskCount - 1;

                        job.AddTask(task);
                    }
                    else
                    {
                        job.AddTasks(clusterArgs.TaskRange.Select(taskNum => CreateTask((int)taskNum, clusterArgs, job, distributableObj, nodesToUse)).ToArray());
                    }
                }
                else
                {
                    clusterArgs.Cleanup = true;
                }

                v2008R2.ISchedulerTask cleanupTask = null;
                if (clusterArgs.Cleanup)
                {
                    cleanupTask = AddCleanupTaskToJob(clusterArgs, scheduler, job, distributableObj);
                }

                Console.WriteLine("Submitting job.");
                scheduler.SubmitJob(job, null, null);
                clusterArgs.JobID = job.Id;
                Console.WriteLine(job.Name + " submitted.");
            }
        }

        private static v2008R2.IStringCollection GetNodesToUse(ClusterSubmitterArgs clusterArgs, v2008R2.IScheduler scheduler, v2008R2.ISchedulerJob job)
        {
            job.AutoCalculateMax = false;
            job.AutoCalculateMin = false;
            var availableNodes = scheduler.GetNodeList(null, null);
            v2008R2.IStringCollection nodesToUse = scheduler.CreateStringCollection();
            List<string> nodesFound = new List<string>();
            foreach (var node in availableNodes)
            {
                string nodeName = ((Microsoft.Hpc.Scheduler.SchedulerNode)node).Name;
                if (!clusterArgs.NodeExclusionList.Contains(nodeName))
                {
                    nodesToUse.Add(nodeName);
                }
                else
                {
                    nodesFound.Add(nodeName);
                }
            }
            Helper.CheckCondition(nodesFound.Count != clusterArgs.NodeExclusionList.Count, "not all nodes in exclusion list found: check for typo " + clusterArgs.NodeExclusionList);

            return nodesToUse;
        }

        private static v2008R2.ISchedulerTask AddCleanupTaskToJob(ClusterSubmitterArgs clusterArgs, v2008R2.IScheduler scheduler, v2008R2.ISchedulerJob job, IDistributable distributableJob)
        {
            v2008R2.ISchedulerCollection taskList = job.GetTaskList(scheduler.CreateFilterCollection(), scheduler.CreateSortCollection(), true);
            v2008R2.IStringCollection dependencyTasks = scheduler.CreateStringCollection();

            if (!clusterArgs.OnlyDoCleanup)
            {
                dependencyTasks.Add(((v2008R2.ISchedulerTask)taskList[0]).Name);
            }
            v2008R2.ISchedulerTask cleanupTask = CreateCleanupTask(job, clusterArgs.ExternalRemoteDirectoryName, clusterArgs.StdErrDirName, clusterArgs.StdOutDirName, "cleanup", true);

            Distribute.Locally local = new Distribute.Locally()
            {
                Cleanup = true,
                TaskCount = clusterArgs.TaskCount,
                Tasks = new RangeCollection(),
                ParallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 1 }
            };

            Distribute.Distribute distributeExe = new Distribute.Distribute()
            {
                Distributor = local,
                Distributable = distributableJob
            };

            string exeName = distributableJob is DistributableWrapper ? clusterArgs.ExeName : distributeExe.GetType().Assembly.GetName().Name;

            //args.AddOptionalFlag("cleanup");
            //args.AddOptional("tasks", "empty");
            string taskCommandLine = string.Format("{0}\\{1} {2}", clusterArgs.ExeRelativeDirectoryName, exeName, CreateTaskString(distributeExe, clusterArgs.MinimalCommandLine));
            cleanupTask.CommandLine = taskCommandLine;

            if (!clusterArgs.OnlyDoCleanup)
            {
                cleanupTask.DependsOn = dependencyTasks;
            }
            job.AddTask(cleanupTask);
            return cleanupTask;
        }


        private static v2008R2.ISchedulerTask CreateCleanupTask(v2008R2.ISchedulerJob job, string internalRemoteDirectoryName, string stdErrDirName, string stdOutDirName, string name, bool isFinalCleanup)
        {
            v2008R2.ISchedulerTask cleanupTask = job.CreateTask();

            cleanupTask.WorkDirectory = internalRemoteDirectoryName;
            cleanupTask.Name = name;
            cleanupTask.IsExclusive = false;
            cleanupTask.StdErrFilePath = string.Format(@"{0}\{1}.txt", stdErrDirName, name);
            cleanupTask.StdOutFilePath = string.Format(@"{0}\{1}.txt", stdOutDirName, name);

            if (isFinalCleanup)
            {

            }
            else
            {
                cleanupTask.CommandLine = "ECHO The cleanup task is running.";
            }

            return cleanupTask;
        }

        private static v2008R2.ISchedulerTask CreateTask(int? taskNumber, ClusterSubmitterArgs clusterArgs, v2008R2.ISchedulerJob job, IDistributable distributableObj, v2008R2.IStringCollection nodesToUse)
        {
            Distribute.Locally local = new Distribute.Locally()
            {
                Cleanup = false,
                TaskCount = clusterArgs.TaskCount,
                Tasks = taskNumber.HasValue ? new RangeCollection(taskNumber.Value) : null,
                ParallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 1 }
            };

            v2008R2.ISchedulerTask task = job.CreateTask();
            if (nodesToUse != null)
            {
                task.RequiredNodes = nodesToUse;
            }
            if (clusterArgs.NumCoresPerTask != null)
            {
                task.MinimumNumberOfCores = clusterArgs.NumCoresPerTask.Value;
                task.MaximumNumberOfCores = clusterArgs.NumCoresPerTask.Value;
                task.MaximumNumberOfNodes = 1;
                local.ParallelOptions.MaxDegreeOfParallelism = clusterArgs.NumCoresPerTask.Value;
            }
            else if (clusterArgs.IsExclusive)
            {
                //task.MinimumNumberOfCores = 1;
                //task.MaximumNumberOfCores = 8;
                //task.MaximumNumberOfNodes = 1;
            }
            task.WorkDirectory = clusterArgs.ExternalRemoteDirectoryName;

            Distribute.Distribute distributeExe = new Distribute.Distribute()
            {
                Distributable = distributableObj,
                Distributor = local
            };

            string taskArgString = CreateTaskString(distributeExe, clusterArgs.MinimalCommandLine);
            string exeName = distributeExe.Distributable is DistributableWrapper ? clusterArgs.ExeName : distributeExe.GetType().Assembly.GetName().Name;

            string taskCommandLine = null;
            if (clusterArgs.UseMPI)
            {
                taskCommandLine = string.Format("mpiexec -n {0} {1}\\{2} {3}", clusterArgs.NumCoresPerTask, clusterArgs.ExeRelativeDirectoryName, exeName, taskArgString);
            }
            else
            {
                taskCommandLine = string.Format("{0}\\{1} {2}", clusterArgs.ExeRelativeDirectoryName, exeName, taskArgString);
            }
            task.CommandLine = taskCommandLine;

            string taskNumberAsString = taskNumber.HasValue ? taskNumber.Value.ToString() : "*";
            task.Name = Helper.CreateDelimitedString(" ", clusterArgs.Name, taskNumberAsString);
            task.StdErrFilePath = string.Format(@"{0}\{1}.txt", clusterArgs.StdErrDirName, taskNumberAsString);
            task.StdOutFilePath = string.Format(@"{0}\{1}.txt", clusterArgs.StdOutDirName, taskNumberAsString);

            if (task.StdErrFilePath.Length >= 160)
                Console.WriteLine("Caution, std error file path is {0} characters, which will probably cause HPC to crash.", task.StdErrFilePath.Length);

            return task;
        }

        #endregion

        private static string CreateUniqueDirectory(string internalRemoteDirectoryName, string subDirectoryName, string dirNameBase)
        {
            int i = 0;
            string dirName;
            string cleanTimeString = GetCleanTimeString();

            do
            {
                dirName = Path.Combine(internalRemoteDirectoryName, string.Format(@"{0}\{1}_{2}{3}", subDirectoryName, dirNameBase, cleanTimeString, i == 0 ? "" : "_" + i));
                i++;
            } while (Directory.Exists(dirName));

            Directory.CreateDirectory(dirName);
            return dirName;
        }

        private static string GetCleanTimeString()
        {
            int hour = DateTime.Now.Hour;
            string hourString = hour < 12 ? hour + "a" : hour == 12 ? hour + "p" : hour - 12 + "p";
            string timeString = string.Format("{0}_{1}", DateTime.Now.ToShortDateString(), hourString);
            timeString = timeString.Replace("/", "");
            timeString = timeString.Replace(" ", "");
            timeString = timeString.Replace(':', '_');
            return timeString;
        }

        //private static bool TryCreateTaskArgsAndValidate(ArgumentCollection args, string pieceIndexAsString, out ArgumentCollection resultArgs)
        //{
        //    var argsToValidate = (ArgumentCollection)args.Clone();
        //    argsToValidate.AddOptional("Tasks", pieceIndexAsString == "*" ? "0" : pieceIndexAsString);


        //    resultArgs = (ArgumentCollection)args.Clone();
        //    resultArgs.AddOptional("Tasks", pieceIndexAsString);

        //    //if (checkIfValid)
        //    //{
        //    //    if (!validateParamsOrNull(argsToValidate))
        //    //    {
        //    //        Console.WriteLine("The parameters are not valid for the submitted task.");
        //    //        Console.WriteLine(resultArgs);
        //    //        return false;
        //    //    }
        //    //}
        //    return true;
        //}

        private static string CreateTaskString(Distribute.Distribute distributeExe, bool suppressDefaults)
        {
            DistributableWrapper wrapper = distributeExe.Distributable as DistributableWrapper;
            if (wrapper != null)
            {
                Distribute.Locally local = (Distribute.Locally)distributeExe.Distributor;
                
                string result = string.Format("{0} -TaskCount {1} -Tasks {2} -Cleanup {3}",
                    wrapper.ToString(),
                    local.TaskCount,
                    local.Tasks == null ? "*" : local.Tasks.ToString(),
                    local.Cleanup);

                return result;
            }
            else
            {
                string result = CommandArguments.ToString(distributeExe, protect:true, suppressDefaults:suppressDefaults);
                result = result.Replace("Tasks:null", "Tasks:*");
                return result;
            }
        }

        static string HelpMessage = @"
The clusterSubmitter provides a useful means for submitting jobs to the cluster. It takes a number of optional, and ""required optional"" arguments, as follows

REQUIRED
    -cluster name
        The cluster that the jobs will be submitted to. Use help to get this message.
    -dir externalDirectoryName
    -taskCount n
        The total number of tasks to run.

OPTIONAL
    -archive archivePathRoot
        Copies all the exes in the currently executing code directory into archivePathRoot. The complete directory used will be
            archivePathRoot\exeName\Major.Minor\Version
        ***To make this work, modify the AssemblyInfo.cs directory to include the attribute
            [assembly: AssemblyVersion(""1.3.*"")]
        In this example, 1 is Major, 3 is minor, and * specifies that the revisions will be updated each time you compile 
        (specifically, the revision field will be an encoding of the date/time stamp of the compile)
    -cleanup
        This flag will cause a cleanup task to be added to the job. This task will not start running until all other jobs are done,
        meaning finished, failed or canceled. This cleanup task will have the same parameters as all other tasks, with the exception
        of 
            -tasks empty
            -cleanup
        The client program must know how to deal with these optional arguments.
    -copyInputFiles filePattern[,filePattern2...]
        Uses xcopy on each filePattern. The destination is the working directory plus any folders specified in the filePattern. For example,
        if filepattern is input\*.txt, xcopy will copy the contents of the local directory input that match *.txt into a (newly created if necessary)
        directory called input on the working directory. The specific xcopy command is 
            xcopy /d /c /i filepattern inferredDestination
        where inferredDestination is workingDir\folderPathFromFilePattern.
    -internalRemoteDirectory name
        The name of the directory that will be used internally. Most clusters don't need this as they use the external directory name.
    -isExclusive
        Each task will run on its own node. This is equivalent to calling -numCoresPerTask N, where N is the number of cores on each node
    -maxCores n
        The maximum number of tasks you want run at once. Useful for load balancing. Must be an int. But if the option is not provided,
        then we will calculate automatically to use the whole cluster.
    -name name
        The name that will be displayed on the Job console. Default is the name of the currently running exe.
    -nodeExclusionList ***NOT CURRENTLY WORKING***--someone figure this out!!! (default empty)
        Takes a list of nodes as a commay delimited string that will be exluded when running. Useful when you node some nodes are failing for reasons out of your control.
        Only works for -version 2
        e.g. call:  -nodeExclusionList MSR-K25-NODE01,MSR-K25-NODE57
    -numCoresPerTask
        How many procs/cores each task will get assigned. If numCoresPerTask is specified, then isExclusive is automatically set to false.
    -oneTaskPerMachine
        Only allow one task to run on each machine (rather than on all nodes 
    -openJobConsole
        Opens the job console to the cluster.
    -priority [Lowest|BelowNormal|Normal|AboveNormal|Highest] (default Normal)
        Sets the priority for the individual tasks.
    -relativeDir
        If present, the value of -dir will be considered a relative path (in the simplest sense, don't try ..\ or anything!) and the root path
        for the cluster will be taken from CCSLibSettings.xml. Paths can be changed by modifying that file directly.
    -taskRange range
        range can be anything that will be parsed into a RangeCollection. Use this to run only a subset of the tasks.
    -usePasswordHack
        Will cause a window to pop up asking for username and password. Hopefully you won't need this.
    -version [1|2] (default 1)
        Specifies the cluster API that will be used. Note that all clusters support version 1, only some clusters support version 2.
";
    }
}
