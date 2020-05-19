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
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Bio.Util;
using MBT.Escience;
using MBT.Escience.Parse;

namespace MBT.Escience.HpcLib
{
    public class HpcLibSettings
    {
        public const string SettingsFileName = "HpcLibSettings.xml";
        public const string LocalSettingsFileName = "HpcLibSettings.local.xml";

        private SharedSettings _sharedSettings;
        private LocalSettings _localSettings;

        private static HpcLibSettings Singleton = new HpcLibSettings();

        public static Dictionary<string, ClusterInfo> KnownClusters { get { return Singleton._sharedSettings.KnownClusters; } }
        public static string LogFile { get { return Singleton._localSettings.LogFile; } }
        public static string DefaultResultsCopyPattern { get { return Singleton._localSettings.DefaultResultsCopyPattern; } }
        public static List<string> ActiveClusters { get { return Singleton._sharedSettings.ActiveClusters; } }

        private HpcLibSettings()
        {
            string username = Environment.GetEnvironmentVariable("username");
            _sharedSettings = SharedSettings.Load(SettingsFileName);
            _localSettings = LocalSettings.Load(LocalSettingsFileName, username);
        }

        public static void CreateExampleSettingsFiles(string sharedFileName, string localFileName)
        {
            KnownClusters.Add("cluster1", new ClusterInfo("cluster1", "path1"));
            KnownClusters.Add("cluster2", new ClusterInfo("cluster2", "path2"));
            ActiveClusters.Add("cluster1");
            ActiveClusters.Add("cluster2");
            Singleton._localSettings.LogFile = @"c:\path\to\clusterLog.xml";
            Singleton._localSettings.DefaultResultsCopyPattern = @"raw\*raw*,raw\*tab*";

            Singleton._localSettings.Username = Environment.GetEnvironmentVariable("username");

            Singleton._localSettings.Save(localFileName);
            Singleton._sharedSettings.Save(sharedFileName);
        }

        /// <summary>
        /// Tries to write to the log file. If no log file is defined, returns false.
        /// </summary>
        public static bool TryWriteToLog(ClusterSubmitterArgs clusterArgs)
        {
            if (!string.IsNullOrEmpty(LogFile))
            {
                FileStream filestream = null;
                try
                {
                    bool exists = File.Exists(LogFile);
                    if (MBT.Escience.FileUtils.TryToOpenFile(LogFile, new TimeSpan(0, 3, 0), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, out filestream))
                    {
                        List<LogEntry> logEntries = exists && filestream.Length > 0 ? LogEntry.LoadEntries(new StreamReader(filestream)) : new List<LogEntry>();
                        LogEntry newSubmission = new LogEntry(clusterArgs);
                        logEntries.Add(newSubmission);
                        filestream.Position = 0;
                        LogEntry.SaveEntries(logEntries, new StreamWriter(filestream));
                        filestream.Dispose();

                        #region first try
                        //XmlDocument doc = new XmlDocument();
                        //XmlElement root;
                        //if (File.Exists(LogFile))
                        //{
                        //    XmlTextReader reader = new XmlTextReader(LogFile);
                        //    doc.Load(reader);
                        //    reader.Close();
                        //    root = (XmlElement)doc.GetElementsByTagName("SubmissionLog")[0];
                        //}
                        //else
                        //{
                        //    XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
                        //    doc.AppendChild(dec);
                        //    root = doc.CreateElement("SubmissionLog");
                        //    doc.AppendChild(root);
                        //}
                        //XmlElement newEntry = new LogEntry(clusterArgs).GetXmlElement(doc);
                        //root.AppendChild(newEntry);

                        //XmlWriterSettings settings = new XmlWriterSettings();
                        //settings.Indent = true;
                        //XmlWriter writer = XmlWriter.Create(LogFile, settings);

                        //doc.WriteContentTo(writer);
                        //writer.Flush();
                        #endregion

                        return true;
                    }
                }
                catch (System.Xml.XmlException exception)
                {
                    Console.WriteLine(exception);
                    return false;
                }
                finally
                {
                    if (filestream != null)
                        filestream.Dispose();
                }
            }
            return false;
        }

        public static string LogEntryFileName(string runName)
        {
            return runName + "_logEntry.xml";
        }

        public static void WriteLogEntryToClusterDirectory(ClusterSubmitterArgs clusterArgs)
        {
            string directory = clusterArgs.ExternalRemoteDirectoryName;
            string filename = LogEntryFileName(clusterArgs.Name);
            LogEntry logEntry = new LogEntry(clusterArgs);
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(LogEntry));
            using (var writer = File.Create(directory + "\\" + filename))
            {
                serializer.Serialize(writer, logEntry);
            }
        }

        public static LogEntry LoadLogEntryFile(string clusterPath, string runName)
        {
            return LoadLogEntryFile(new FileInfo(clusterPath + "\\" + LogEntryFileName(runName)));
        }

        public static LogEntry LoadLogEntryFile(FileInfo file)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(LogEntry));
            using (var reader = file.OpenRead())
            {
                return (LogEntry)serializer.Deserialize(reader);
            }
        }


        #region first try
        //private static void LoadUserSettings()
        //{
        //    string asmPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //    string localSettingsPath = Path.Combine(asmPath, LocalSettingsFileName);
        //    if (File.Exists(localSettingsPath))
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.Load(new XmlTextReader(localSettingsPath));
        //        var logNodes = doc.GetElementsByTagName("Log");
        //        Helper.CheckCondition(logNodes.Count <= 1, "Can have at most 1 log file.");
        //        if (logNodes.Count == 1)
        //        {
        //            LogFile = logNodes[0].InnerText;
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("No user settings found in {0}", localSettingsPath);
        //    }
        //}

        //private static void LoadSharedSettings()
        //{
        //    string asmPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(new XmlTextReader(Path.Combine(asmPath, SettingsFileName)));
        //    foreach (XmlNode node in doc.GetElementsByTagName("Cluster"))
        //    {
        //        ClusterNode clusterNode = new ClusterNode((XmlElement)node);
        //        KnownClusters.Add(clusterNode.Name, clusterNode);
        //    }
        //}
        #endregion

        [Serializable]
        public class SharedSettings
        {
            [XmlArray("KnownClusters")]
            public List<ClusterInfo> DummyField;
            public List<string> ActiveClusters = new List<string>();

            [XmlIgnore()]
            public Dictionary<string, ClusterInfo> KnownClusters { get; private set; }

            public static SharedSettings Load(string filename)
            {
                try
                {
                    string asmPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string settingsPath = Path.Combine(asmPath, filename);
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SharedSettings));

                    using (TextReader reader = File.OpenText(settingsPath))
                    {
                        SharedSettings result = (SharedSettings)serializer.Deserialize(reader);
                        result.KnownClusters = new Dictionary<string, ClusterInfo>(StringComparer.CurrentCultureIgnoreCase);
                        result.DummyField.ForEach(node => result.KnownClusters.Add(node.Name, node));

                        return result;
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Problem loading " + filename);
                    Console.Error.WriteLine(e.Message);
                    SharedSettings result = new SharedSettings();
                    result.KnownClusters = new Dictionary<string, ClusterInfo>();
                    return result;
                }
            }

            public void Save(string filename)
            {
                string asmPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string settingsPath = Path.Combine(asmPath, filename);
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(SharedSettings));

                DummyField = KnownClusters.Values.ToList();
                using (TextWriter writer = File.CreateText(settingsPath))
                {
                    serializer.Serialize(writer, this);
                }
            }
        }

        [Serializable]
        public class LocalSettings
        {
            [XmlAttribute]
            public string Username;
            public string LogFile = "";
            public string DefaultResultsCopyPattern = "";

            //public static LocalSettings Load(string filename, string username)
            //{
            //    try
            //    {
            //        LocalSettings result = GetLocalSettings(filename, username);
            //        if (result == null)
            //            throw new ArgumentException("No local settings entry for user " + username);
            //        else
            //            return result;
            //    }
            //    catch (Exception e)
            //    {
            //        Console.Error.WriteLine("Problem loading " + filename);
            //        Console.Error.WriteLine(e.Message);
            //        return new LocalSettings();
            //    }
            //}

            public void Save(string filename)
            {
                List<LocalSettings> userSettings = LoadAll(filename);
                bool alreadyExists = false;
                for (int i = 0; i < userSettings.Count; i++)
                {
                    if (userSettings[i].Username.Equals(Username, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Helper.CheckCondition(!alreadyExists, "Multiple entries for " + Username);
                        userSettings[i] = this;
                        alreadyExists = true;
                    }
                }
                if (!alreadyExists)
                    userSettings.Add(this);

                SaveAll(userSettings, filename);
            }

            public static void SaveAll(List<LocalSettings> settings, string filename)
            {
                string asmPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string settingsPath = Path.Combine(asmPath, filename);
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<LocalSettings>));

                using (TextWriter writer = File.CreateText(settingsPath))
                {
                    serializer.Serialize(writer, settings);
                }
            }

            public static LocalSettings Load(string filename, string username)
            {
                List<LocalSettings> userSettings = LoadAll(filename);
                foreach (LocalSettings user in userSettings)
                {
                    if (user.Username.Equals(username, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return user;
                    }
                }
                Console.Error.WriteLine("No local settings entry for user " + username);
                return new LocalSettings();
            }

            public static List<LocalSettings> LoadAll(string filename)
            {
                try
                {
                    string asmPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string settingsPath = Path.Combine(asmPath, filename);
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<LocalSettings>));

                    using (TextReader reader = File.OpenText(settingsPath))
                    {
                        List<LocalSettings> userSettings = (List<LocalSettings>)serializer.Deserialize(reader);
                        return userSettings;
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Problem loading " + filename);
                    Console.Error.WriteLine(e.Message);
                    return new List<LocalSettings>();
                }
            }
        }

        [Serializable]
        public class UserSettings
        {
            [XmlAttribute]
            public string Username;
            public LocalSettings LocalSettings;

            public UserSettings() { }
            public UserSettings(string username, LocalSettings localSettings)
            {
                Username = username;
                LocalSettings = localSettings;
            }




        }


    }



    [Serializable]
    public class ClusterInfo
    {
        //public ClusterNode(XmlElement elt)
        //{
        //    Name = elt.GetAttribute("Name");
        //    StoragePath = elt.GetAttribute("StoragePath");
        //}
        public ClusterInfo() { }
        public ClusterInfo(string name, string path)
        {
            Name = name;
            StoragePath = path;
        }

        [XmlAttribute()]
        public string Name { get; set; }
        [XmlAttribute()]
        public string StoragePath { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
