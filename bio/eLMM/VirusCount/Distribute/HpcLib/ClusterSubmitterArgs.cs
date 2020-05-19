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
using System.Xml.Serialization;
using Microsoft.Hpc.Scheduler.Properties;
using MBT.Escience;
using MBT.Escience.Parse;
using v1 = Microsoft.ComputeCluster;
using v2 = Microsoft.Hpc.Scheduler;
using Bio.Util;

namespace MBT.Escience.HpcLib
{
    [Serializable]
    [XmlInclude(typeof(OnHpc)), XmlInclude(typeof(OnHpcAndWait))]

    public class ClusterSubmitterArgs : IParsable
    {

        /// <summary>
        /// The cluster name, or :auto: or $auto$ to take the cluster with the most idle cores.
        /// </summary>
        [Parse(ParseAction.Required)]
        public string Cluster { get; set; }

        /// <summary>
        /// The number of tasks that the work will be divided into.
        /// </summary>
        [Parse(ParseAction.Required)]
        public int TaskCount { get; set; }



        public string Dir = null;
        public bool RelativeDir = false;
        public bool UsePasswordHack = false;
        public string InternalRemoteDirectory = null;
        public List<string> NodeExclusionList = new List<string>();
        public List<string> NodesToUseList = new List<string>();
        public bool IsExclusive = false;
        public bool MinimalCommandLine = false;

        public string Name = null;

        public int? MaximumNumberOfCores = null;
        public int? MinimumNumberOfCores = null;

        public int? MaximumNumberOfNodes = null;
        public int? MinimumNumberOfNodes = null;

        public string Archive = null;
        public bool Cleanup = true;
        public bool OnlyDoCleanup = false;
        public List<string> CopyInputFiles = new List<string>();

        public bool OpenJobConsole = false;
        public int Version = 3;
        public string Priority = "Normal";
        public string Username = HpcLib.GetCurrentUsername();
        public int MaxSubmitTries = 1;
        //[Parse(ParseAction.Optional)]
        //public int MaxResubmitsAfterFailed = 0;
        public bool UseMPI = false;
        public string JobTemplate = null;
        public string ExeRelativeDirectoryName = null;//specify just the last part of the the path, so that there should be no slashes
        //[Parse(ParseAction.Optional)]
        //public bool SubmitViaV3Beta = false;

        [XmlIgnore]// don't want to store passwords in unprotected files!!
        [Parse(ParseAction.Optional)]
        public string Password = null;
        [Parse(ParseAction.Optional)]
        public string ExeName = null;
        [Parse(ParseAction.Optional)]
        public int? NumCoresPerTask = null;//each task will use this many processors/cores on a given node (range is typically 1 to 8)
        [Parse(ParseAction.Optional)]
        public int Runtime = 86000;//==24 * 60 * 60;//in seconds for a job


        [Parse(ParseAction.Optional)]
        public RangeCollection TaskRange = new RangeCollection(0, int.MaxValue - 1); //!!!!why???

        [Parse(ParseAction.Ignore)]
        public string ProgramParams { get; set; }


        //private v2.ISchedulerJob _jobV2;

        ///// <summary>
        ///// If the Job is a v2 job, then this properties will hold a direct connection to the job after submission.
        ///// </summary>
        //[XmlIgnore]
        //[Parse(ParseAction.Ignore)]
        //public v2.ISchedulerJob JobV2
        //{
        //    get
        //    {
        //        if (_jobV2 == null)
        //        {
        //            v2.IScheduler scheduler;
        //            HpcLib.TryConnect(Cluster, out scheduler).Enforce("Unable to connect to head node {0}", Cluster);
        //            HpcLib.TryGetJob(scheduler, Username, JobID, out _jobV2).Enforce("Unable to load jobID {0} for user {1}", JobID, Username);
        //        }
        //        return _jobV2;
        //    }
        //    set { _jobV2 = value; }
        //}

        //[Parse(ParseAction.Ignore)]
        //public JobState JobState { get; set; }
        [Parse(ParseAction.Ignore)]
        public int JobID { get; set; }

        [Parse(ParseAction.Ignore)]
        public object ApiPriority
        {
            get
            {
                try
                {
                    switch (Version)
                    {
                        case 1:
                            return MBT.Escience.Parse.Parser.Parse<v1.JobPriority>(Priority);
                        case 2:
                        case 3:
                            return MBT.Escience.Parse.Parser.Parse<v2.Properties.JobPriority>(Priority);
                        default:
                            throw new NotSupportedException("Don't know Version number " + Version);
                    }
                }
                catch
                {
                    throw new ArgumentException(string.Format("Cannot parse string {0} into a job priority for API Version {1}", Priority, Version));
                }
            }
        }


        //public string InternalRemoteDirectoryName
        //{
        //    get { return InternalRemoteDirectory == null ? ExternalRemoteDirectoryName : InternalRemoteDirectory; }
        //}

        private string _externalRemoteDirectoryName = null;
        [Parse(ParseAction.Ignore)]
        public string ExternalRemoteDirectoryName
        {
            get
            {
                if (_externalRemoteDirectoryName == null)
                {
                    if (RelativeDir)
                    {
                        Helper.CheckCondition<UnknownClusterException>(HpcLibSettings.KnownClusters.ContainsKey(Cluster),
                            "{0} is not a known cluster in {1}. Known clusters: {2}", Cluster, HpcLibSettings.SettingsFileName, HpcLibSettings.KnownClusters.Keys.StringJoin(","));
                        string basePath = HpcLibSettings.KnownClusters[Cluster].StoragePath;
                        _externalRemoteDirectoryName = Path.Combine(basePath, Dir);
                    }
                    else
                    {
                        _externalRemoteDirectoryName = Dir;
                    }
                }
                return _externalRemoteDirectoryName;
            }
        }

        //public string ExeRelativeDirectoryName { get; set; }//want this to be a parseble option now, so can point to old EXE folders on cluster for debugging
        public string StdOutDirName { get; set; }
        public string StdErrDirName { get; set; }

        public void FinalizeParse()
        {
            Helper.CheckCondition<ParseException>(NodeExclusionList.Count == 0 || Version == 2 || Version == 3, "-nodeExclusionList is only supported for API level 1");
            Helper.CheckCondition<ParseException>(Version == 1 || Version == 2 || Version == 3, "Only Version 1 or 2 or 3 is supported.");
            Helper.CheckCondition<ParseException>(TaskCount > 0, "You must specify a positive task count using -TaskCount");
            Helper.CheckCondition<ParseException>(Cluster != null, "You must specify a cluster name using -cluster name");

            if (Dir == null)
            {
                Dir = Environment.CurrentDirectory;
                string username = HpcLib.GetCurrentUsername();
                Dir = username + Dir.Substring(2); // first 2 characters are the drive. e.g. D:
                RelativeDir = true;
            }
            if (ExeName == null)
                ExeName = SpecialFunctions.GetEntryOrCallingAssembly().GetName().Name;

            if (UsePasswordHack)
                Password = HpcLib.GetPassword(out Username);

            if (Name == null)
                Name = ExeName;

            if (Cluster.Equals(":auto:", StringComparison.CurrentCultureIgnoreCase) || Cluster.Equals("$auto$", StringComparison.CurrentCultureIgnoreCase))
                Cluster = HpcLib.GetMostAppropriateCluster(TaskCount).Cluster;

        }

        public ClusterSubmitterArgs() { }
        public ClusterSubmitterArgs(string args) : this(new CommandArguments(args)) { }

        public ClusterSubmitterArgs(ArgumentCollection args)
        {
            args.ParseInto(this, checkComplete: false);
            //args.AddOptional("-TaskCount", this.TaskCount);
            ProgramParams = args.ToString();    // put what's left into here.
        }

        public override string ToString()
        {
            //return base.ToString() + " " + ProgramParams;
            return CommandArguments.ToString(this) + " " + ProgramParams;
        }

        public override bool Equals(object obj)
        {
            return obj is ClusterSubmitterArgs && this.ToString().Equals(obj.ToString());
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Attempts to connect the the scheduler specified by Cluster and retrieve the Job specified by JobID. Will throw a SchedulerException if either of those fails.
        /// </summary>
        /// <returns></returns>
        public v2.ISchedulerJob GetV2Job()
        {
            v2.ISchedulerJob job;

            v2.IScheduler scheduler;
            HpcLib.TryConnect(Cluster, out scheduler).Enforce<SchedulerException>("Unable to connect to head node {0}", Cluster);
            HpcLib.TryGetJob(scheduler, Username, JobID, out job).Enforce<SchedulerException>("Unable to load jobID {0} for user {1}", JobID, Username);

            return job;
        }
    }
}
