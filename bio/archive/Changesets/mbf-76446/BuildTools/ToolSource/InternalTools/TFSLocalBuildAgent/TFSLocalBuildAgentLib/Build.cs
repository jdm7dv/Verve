using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Timers;
using Microsoft.TeamFoundation.VersionControl.Client;
using TFSLocalBuildAgentLib.Properties;

[assembly: CLSCompliant(true)]
namespace TFSLocalBuildAgentLib
{
    public static class Build
    {
        #region Fields
        /// <summary>
        /// The name of the log file to which the build output data is logged.
        /// </summary>
        private static string buildLogFileName = XmlHelper.BuildLogFileName;

        /// <summary>
        /// Logs data to an output stream.
        /// </summary>
        private static StreamWriter logWriter;

        /// <summary>
        /// The name of the team project to be downloaded.
        /// </summary>
        private static TeamProject myProject;

        /// <summary>
        /// The TFS controller that handles interaction with the TFS server.
        /// </summary>
        private static TFSController controller;
        #endregion

        #region Events
        /// <summary>
        /// Output events from processes launched by the build agent.
        /// </summary>
        public static event EventHandler<OutputReceivedEventArgs> OutputReceivedEvent;    

        #endregion

        #region Public Methods
        /// <summary>
        /// Starts the build agent process and keeps checking for new changesets in the server
        /// and if found, initiates a new build.
        /// </summary>
        public static void Start()
        {
            // reload the XML configuration file
            XmlHelper.LoadConfiguration();

            Connect();

            StartNewBuildIfNecessary();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Connects to the TFS server.
        /// </summary>
        private static void Connect()
        {
            controller = new TFSController(XmlHelper.ServerUri, XmlHelper.TeamProjectCollectionName);
            Write(Resources.ConnectingToTfs);
            controller.Connect();
            WriteLine(Resources.Done);

            foreach (TeamProject teamPrj in controller.TeamProjects)
            {
                if (string.Equals(teamPrj.Name, XmlHelper.ProjectName, StringComparison.OrdinalIgnoreCase))
                {
                    myProject = teamPrj;
                    break;
                }
            }

            if (myProject == null)
            {
                WriteLine(Resources.ProjectNotFound + XmlHelper.ProjectName);
            }
        }

        /// <summary>
        /// Gets a value which indicates whether the last build of the MBI
        /// solution was done today.
        /// </summary>
        /// <returns>
        /// true if the solution was last built today; otherwise, false.
        /// </returns>
        private static bool isLastBuiltToday()
        {
            string previousDate = XmlHelper.LastBuildDate;
            string currentDate = DateTime.Now.Year + "." + DateTime.Now.Month + "." + DateTime.Now.Day;

            if (string.Compare(previousDate, currentDate, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Downloads and builds the latest changeset if the current
        /// changeset id is more recent than the previous one.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The elapsed event args.</param>
        private static void StartNewBuildIfNecessary()
        {
            Write(Resources.GettingLatestChangesetNumber);
            int latestChangeSetID = controller.LatestChangeSetNumber;
            WriteLine(Resources.Done);
            WriteLine(Resources.LatestChangesetNumber + latestChangeSetID.ToString(CultureInfo.InvariantCulture));

            // if a newer changeset is available
            if (latestChangeSetID != XmlHelper.LastBuiltChangesetId)
            {
                try
                {
                    // remove all content in the local workspace path
                    CleanUpWorkspace();

                    // download the latest changeset
                    DownloadChangeset(latestChangeSetID);

                    // build the latest solution
                    BuildSolution();

                    // copy solution build output to drop location
                    CopyOutputToDropLocation();

                    XmlHelper.LastBuiltChangesetId = latestChangeSetID;
                }
                finally
                {
                    if (logWriter != null)
                    {
                        logWriter.Close();
                        logWriter = null;
                    }
                }
            }
            else
            {
                WriteLine(Resources.LastChangesetBuilt + XmlHelper.LastBuiltChangesetId);
                WriteLine(Resources.NoNewChangesetAvailable);
            }
        }

        /// <summary>
        /// Copies the solution binaries, installer and build log to the drop 
        /// location specified in the configuration file.
        /// </summary>
        private static void CopyOutputToDropLocation()
        {
            // assign the date stamp added to the drop folder name
            string currentBuildDate = DateTime.Now.Year + "." + DateTime.Now.Month + "." + DateTime.Now.Day;
            string folderName;

            XmlHelper.LastBuildNumber = isLastBuiltToday() ? ++XmlHelper.LastBuildNumber : 1;
            folderName = currentBuildDate + "." + XmlHelper.LastBuildNumber;

            XmlHelper.LastBuildDate = currentBuildDate;

            CopyBinariesToDropLocation(folderName);

            logWriter.Close();
            logWriter = null;

            CopyFile(@".\" + buildLogFileName, XmlHelper.DropLocation + "\\" + folderName);
        }

        /// <summary>
        /// Logs the output data received.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The event data.</param>
        private static void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            WriteLine(e.Data);
        }

        /// <summary>
        /// Writes the specified string to the configured output streams.
        /// </summary>
        /// <param name="logMessage">The output string.</param>
        private static void Write(string logMessage)
        {
            if (logWriter == null)
            {
                logWriter = new StreamWriter(buildLogFileName);
            }
            
            logWriter.Write(logMessage);

            if (OutputReceivedEvent != null)
            {
                OutputReceivedEvent.Invoke(null, new OutputReceivedEventArgs(logMessage));
            }
        }

        /// <summary>
        /// Writes the specified string appended with a newline character
        /// to the configured output streams.
        /// </summary>
        /// <param name="logMessage">The output string.</param>
        private static void WriteLine(string logMessage)
        {
            Write(logMessage + Environment.NewLine);
        }

        /// <summary>
        /// Downloads the latest changeset from the TFS server.
        /// </summary>
        /// <param name="latestChangeSetID">The latest change set ID.</param>
        private static void DownloadChangeset(int latestChangeSetID)
        {
            WriteLine(Resources.LatestChangesetNumber + latestChangeSetID);

            WriteLine(Resources.Downloading);
            WriteLine(Resources.DownloadStartTime + DateTime.Now);

            controller.DownloadSource(XmlHelper.DownloadSourcePath, latestChangeSetID, XmlHelper.WorkspaceName, XmlHelper.LocalWorkspacePath);

            WriteLine(Resources.DownloadFinishTime + DateTime.Now);
        }

        /// <summary>
        /// Builds the MBI solutions in the main branch and development branch.
        /// </summary>
        private static void BuildSolution()
        {
            string devCreateSetupPath = XmlHelper.LocalWorkspacePath + @"\MBI\Source\MBT\Installer\";

            WriteLine(Resources.Building);
            WriteLine(Resources.BuildStartTime + DateTime.Now);

            // copy setup script with silent validation enabled to installer folder
            CopyFile(@".\CreateSetupSilentValidation.cmd", devCreateSetupPath);

            RunProcess(@".\CreateSetupSilentValidation.cmd", "\"" + devCreateSetupPath + "\"");

            WriteLine(Resources.BuildFinishTime + DateTime.Now);
        }

        /// <summary>
        /// Deletes the local workspace folder.
        /// </summary>
        private static void CleanUpWorkspace()
        {
            string arguments = "/c if exist \"" + XmlHelper.LocalWorkspacePath + "\" (rmdir /s /q \"" + XmlHelper.LocalWorkspacePath + "\")";

            Write(Resources.CleaningUpWorkspace);
            RunProcess("cmd.exe", arguments);
            WriteLine(Resources.Done);
        }

        /// <summary>
        /// Copies the installer and binaries to the drop location.
        /// </summary>
        /// <param name="folderName">The folder name for this drop.</param>
        private static void CopyBinariesToDropLocation(string folderName)
        {
            string source, arguments;
            string commandInterpreter = "cmd.exe";
            string destination = "\"" + XmlHelper.DropLocation + "\\" + folderName + "\"";

            // copy binaries
            source = "\"" + XmlHelper.BinaryPath + "\"";
            arguments = String.Format(CultureInfo.InvariantCulture, "/c start xcopy /y /i /e {0} {1}", source, destination);
            Write(Resources.CopyingBinaries);
            RunProcess(commandInterpreter, arguments);
            WriteLine(Resources.Done);

            // copy installers
            source = "\"" + XmlHelper.InstallerPath + "\"";
            string installerFolderName = source.Substring(source.LastIndexOf('\\')).TrimEnd('"');

            arguments = String.Format(CultureInfo.InvariantCulture, "/c start xcopy /y /i /e {0} {1}", source, destination + installerFolderName);
            Write(Resources.CopyingInstaller);
            RunProcess(commandInterpreter, arguments);
            WriteLine(Resources.Done);

            // copy cleanup file for test automation
            source = "\"" + XmlHelper.TestAutomationCleanupFile + "\"";
            arguments = String.Format(CultureInfo.InvariantCulture, "/c start xcopy /y {0} {1}", source, destination);
            Write(Resources.CopyingTestAutomationCleanupFile);
            RunProcess(commandInterpreter, arguments);
            WriteLine(Resources.Done);

            // copy test automation settings file
            source = "\"" + XmlHelper.TestAutomationSettingsFile + "\"";
            arguments = String.Format(CultureInfo.InvariantCulture, "/c start xcopy /y {0} {1}", source, destination);
            Write(Resources.CopyingTestAutomationSettingsFile);
            RunProcess(commandInterpreter, arguments);
            WriteLine(Resources.Done);
        }

        /// <summary>
        /// Copies the specified source file to the specified destination folder.
        /// </summary>
        /// <param name="source">The full path to the source file.</param>
        /// <param name="destination">The full path to the destination folder.</param>
        private static void CopyFile(string source, string destination)
        {
            string arguments = "/c start xcopy /y /i \"" + source + "\"  \"" + destination + "\"";
            RunProcess("cmd.exe", arguments);
        }

        /// <summary>
        /// Launches the specified process with a redirected output stream.
        /// </summary>
        /// <param name="processName">The process to be launched.</param>
        /// <param name="arguments">The command line arguments to the process.</param>
        private static void RunProcess(string processName, string arguments)
        {
            using (Process p = new Process())
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(processName);
                processStartInfo.UseShellExecute = false;
                processStartInfo.Arguments = arguments;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.RedirectStandardOutput = true;

                p.OutputDataReceived += new DataReceivedEventHandler(OnOutputDataReceived);
                p.ErrorDataReceived += new DataReceivedEventHandler(OnOutputDataReceived);
                p.StartInfo = processStartInfo;

                p.Start();
                p.BeginOutputReadLine();
                p.WaitForExit();
            }
        }

        #endregion
    }
}
