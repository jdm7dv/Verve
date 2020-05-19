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
using System.Linq;
using System.Windows.Forms;
using Microsoft.Hpc.Scheduler;
using MBT.Escience.Parse;
using Bio.Util;
using MBT.Escience;
using Microsoft.Hpc.Scheduler.Properties;

namespace MBT.Escience.HpcLib
{
    public static class HpcLib
    {

        /// <summary>
        /// For each pattern in filePatternsToCopy, will create and call xcopy /d /c /i baseSrcDir\pattern baseDestnDir\patternPath, 
        /// where patternPath is the path part of the pattern. Set baseDestnDir or baseSrcDir to the empty string to make it use
        /// the current working directory. 
        /// </summary>
        /// <param name="filePatternsToCopy"></param>
        /// <param name="baseSrcDir"></param>
        /// <param name="baseDestnDir"></param>
        public static void CopyFiles(IEnumerable<string> filePatternsToCopy, string baseSrcDir, string baseDestnDir)
        {
            //string xcopyArgsFormatString = GetXCopyArgsFormatString(filePatternsToCopy.First(), baseSrcDir, baseDestnDir);
            //Console.WriteLine("xcopyArgsFormatString=" + xcopyArgsFormatString);

            foreach (string file in filePatternsToCopy)
            {
                string xcopyArgsFormatString = GetXCopyArgsFormatString(file, baseSrcDir, baseDestnDir);
                string srcFilePath = Path.Combine(baseSrcDir, file);
                Process proc = new Process();
                proc.StartInfo.FileName = "xcopy";
                proc.StartInfo.Arguments = string.Format(xcopyArgsFormatString, srcFilePath);

                //Console.WriteLine("srcFilePath for xcopy=" + srcFilePath);

                proc.StartInfo.UseShellExecute = false;
                //proc.StartInfo.RedirectStandardOutput = true;
                //Console.WriteLine("xcopy " + proc.StartInfo.Arguments);
                proc.Start();
                proc.WaitForExit();
                if (proc.ExitCode > 0)
                    throw new Exception("Unable to copy file using command xcopy " + proc.StartInfo.Arguments);
            }
        }

        public static string GetXCopyArgsFormatString(string exampleFileToCopy, string baseSrcDir, string baseDestnDir)
        {
            string relativeDirName = MBT.Escience.FileUtils.GetDirectoryName(exampleFileToCopy, baseSrcDir);
            string outputDir = Path.Combine(baseDestnDir, relativeDirName);

            if (string.IsNullOrWhiteSpace(outputDir))
                outputDir = ".";

            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            string args = "/d /c /i /y \"{0}\" \"" + outputDir + "\"";
            return args;
        }

        public static void DeleteFiles(IEnumerable<string> filePatternsToDelete, string baseDir)
        {
            foreach (string filePattern in filePatternsToDelete)
            {
                string directory = Path.Combine(baseDir, Path.GetDirectoryName(filePattern));
                string filePatternNoDir = Path.GetFileName(filePattern);
                foreach (string file in Directory.GetFiles(directory, filePatternNoDir))
                {
                    File.Delete(file);
                }
            }
        }

        public static string GetMachineNameFromUNCName(string directoryName)
        {
            //!!!be nicer to use Regex
            Helper.CheckCondition(directoryName.StartsWith(@"\\"), "The directory name should start with '\\'");
            string[] partCollecton = directoryName.Substring(2).Split(new char[] { '\\' }, 2);
            Helper.CheckCondition(partCollecton.Length == 2, "Expected the directory name to have a machine part and a file part");
            return partCollecton[0];
        }



        public static void CreateNewExeDirectory(string niceName, string directoryName, out string newExeDirectoryName, out string exeRelativeDirectoryName)
        {
            string exeRelativeDirectoryNameMostly = string.Format(@"exes\{0}{1}", niceName, DateTime.Now.ToShortDateString().Replace("/", ""));
            for (int suffixIndex = 0; ; ++suffixIndex)
            {
                string suffix = suffixIndex == 0 ? "" : suffixIndex.ToString();
                newExeDirectoryName = string.Format(@"{0}\{1}{2}", directoryName, exeRelativeDirectoryNameMostly, suffix);
                //!!!Two instances of this program could (it is possible) create the same directory.
                if (!Directory.Exists(newExeDirectoryName))
                {
                    try
                    {
                        Directory.CreateDirectory(newExeDirectoryName);
                    }
                    catch (IOException e)
                    {
                        throw new IOException("Could not create directory " + newExeDirectoryName, e);
                    }
                    exeRelativeDirectoryName = exeRelativeDirectoryNameMostly + suffix;
                    break;
                }
            }
        }

        private static bool ExesAreAlreadyOnCluster(string niceName, string rootDirectory, string executingExeDirectoryName, out string newExeDirectoryName, out string exeRelativeDirectoryName)
        {
            newExeDirectoryName = exeRelativeDirectoryName = null;

            DirectoryInfo rootExeDir = new DirectoryInfo(rootDirectory + "\\exes");
            if (!rootExeDir.Exists)
                return false;

            var orderedDirs = rootExeDir.GetDirectories().OrderByDescending(dir => dir.LastWriteTime);
            if (orderedDirs.Count() == 0)
                return false;

            DirectoryInfo lastExeDir = orderedDirs.First();
            DirectoryInfo executingDir = new DirectoryInfo(executingExeDirectoryName);

            bool sameExes = lastExeDir.GetFiles("*", SearchOption.AllDirectories).SequenceEqual(executingDir.GetFiles("*", SearchOption.AllDirectories), new FileAccessComparer());
            if (sameExes)
            {
                newExeDirectoryName = lastExeDir.FullName;
                exeRelativeDirectoryName = "exes\\" + lastExeDir.Name;
            }

            return sameExes;
        }





        public static string GetPassword(out string id)
        {
            Dictionary<string, string> hack1 = new Dictionary<string, string>();
            GetPassword getPassword = new GetPassword();
            getPassword.KludgeDirectory = hack1;
            Application.Run(getPassword);
            id = hack1["id"];
            string password = hack1["password"];
            return password;
        }

        public static Dictionary<string, string> _dirNameToExeDirName = new Dictionary<string, string>();
        public static string CopyExesToCluster(string directoryName, string niceName)
        {
            string finalResult;
            //!! note that there is a small chance of a copy conflict with another process. Should come up with a safe way around that.
            if (!_dirNameToExeDirName.TryGetValue(directoryName, out finalResult))
            {
                Console.WriteLine("Checking to see if exe's are already there.");
                string newExeDirectoryName;
                string exeNewRelativeDirectoryName;
                string executingExeDirectoryName = Path.GetDirectoryName(Bio.Util.FileUtils.GetEntryOrCallingAssembly().Location);

                bool needToCopyExesToCluster = !ExesAreAlreadyOnCluster(niceName, directoryName, executingExeDirectoryName, out newExeDirectoryName, out exeNewRelativeDirectoryName);
                if (needToCopyExesToCluster)
                {
                    Console.WriteLine("Copying Exes");
                    Directory.CreateDirectory(directoryName);
                    CreateNewExeDirectory(niceName, directoryName, out newExeDirectoryName, out exeNewRelativeDirectoryName);
                    SpecialFunctions.CopyDirectory(executingExeDirectoryName, newExeDirectoryName, /*recursive*/ true);
                    Console.WriteLine("Done copying.");
                }
                finalResult = "\"" + exeNewRelativeDirectoryName + "\"";
                _dirNameToExeDirName.Add(directoryName, finalResult);
            }
            return finalResult;
        }

        public static bool TryConnect(string headnode, out IScheduler scheduler)
        {
            IScheduler s = new Scheduler();

            bool success = false;
            //using (FunctionTimeout func = new FunctionTimeout(false))
            //{
            //    Action connect = () => s.Connect(headnode);
            //    try
            //    {
            //        success = func.TryExecuteFunction(connect, new TimeSpan(0, 0, 2));
            //    }
            //    catch
            //    {
            //        success = false;
            //    }
            //}
            try
            {
                s.Connect(headnode);
                success = true;
            } 
            catch
            {
                success = false;
            }
            scheduler = success ? s : null;
            return success;
        }

        public static bool TryGetJob(IScheduler scheduler, string username, int jobID, out ISchedulerJob job)
        {
            IFilterCollection filter = scheduler.CreateFilterCollection();
            filter.Add(FilterOperator.Equal, PropId.Job_UserName, username);

            if (scheduler.GetJobIdList(filter, null).Contains(jobID))
            {
                job = scheduler.OpenJob(jobID);
            }
            else
            {
                job = null;
            }

            return job != null;
        }


        internal static string GetCurrentUsername()
        {
            string domain = Environment.GetEnvironmentVariable("USERDOMAIN");
            string username = Environment.GetEnvironmentVariable("USERNAME");
            return domain + "\\" + username;
        }

        /// <summary>
        /// Returns the cluster with the fewest idle cores that will still run on minIdleCoreCount cores. If none exists, 
        /// returns the cluster with the most idle cores. N10 is excluded, as this is a special cluster for long running jobs.
        /// </summary>
        /// <param name="minIdleCoreCount">The minimum number of cores you want. Will return a cluster with at least this many free, if one exists.</param>
        /// <returns></returns>
        public static ClusterStatus GetMostAppropriateCluster(int minIdleCoreCount)
        {
            string longRunningJobsCluster = "msr-gcb-n10";

            List<ClusterStatus> clusterStatusList = HpcLibSettings.ActiveClusters.Where(name => !name.Equals(longRunningJobsCluster, StringComparison.CurrentCultureIgnoreCase))
                                                    .Select(clusterName => new ClusterStatus(clusterName)).ToList();
            if(ParallelOptionsScope.Exists)
                clusterStatusList.AsParallel().WithParallelOptionsScope().ForAll(cs => cs.Refresh());
            else
                clusterStatusList.ForEach(cs => cs.Refresh());

            clusterStatusList.Sort((cs1, cs2) => cs1.IdleCores.CompareTo(cs2.IdleCores));

            foreach (var cluster in clusterStatusList)
            {
                if (cluster.IdleCores >= minIdleCoreCount + 8) // the count is always off by 8, because it includes the head node's cores, which are unavailable.
                    return cluster;
            }
            return clusterStatusList[clusterStatusList.Count - 1];
        }
    }
}
