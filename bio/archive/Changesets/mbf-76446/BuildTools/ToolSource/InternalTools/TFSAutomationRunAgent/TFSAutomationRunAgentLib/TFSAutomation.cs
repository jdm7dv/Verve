// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using TFSAutomationRunAgentLib.Properties;

[assembly: CLSCompliant(true)]
namespace TFSAutomationRunAgentLib
{
    /// <summary>
    /// TFS Automation class which runs the automation script
    /// </summary>
    public static class TFSAutomation
    {

        #region Fields

        /// <summary>
        /// The name of the log file to which the build output data is logged.
        /// </summary>
        private static string automationLogFileName = string.Empty;

        /// <summary>
        /// Logs data to an output stream.
        /// </summary>
        private static StreamWriter logWriter;

        #endregion

        #region Public Methods

        /// <summary>
        /// Starts the automation after loading the configuration information
        /// </summary>
        /// <param name="tests">Tests which needs to be started.</param>
        public static void StartAutomationRun(string tests)
        {
            // reload the XML configuration file
            XmlHelper.LoadConfiguration();

            // Initializes the arguments for running the automation script
            InitializeAndRunAutomation(tests);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the arguments for running the automation script 
        /// i.e., path and other information
        /// </summary>
        /// <param name="tests">Tests which needs to be started.</param>
        private static void InitializeAndRunAutomation(string tests)
        {
            string buildPath = XmlHelper.LocalRunLocation
                + string.Format((IFormatProvider)null,
                "\\{0}.{1}.{2}.1",
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day);

            string dropPath = XmlHelper.DropLocation
                + string.Format((IFormatProvider)null,
                "\\{0}.{1}.{2}.1",
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day);

            try
            {
                if (!Directory.Exists(buildPath))
                {
                    // Creates a build location path
                    Directory.CreateDirectory(buildPath);
                }

                // Initialize log file.
                automationLogFileName = buildPath + XmlHelper.AutomationLogFileName;

                string copyArguments = "\"" + dropPath + "\" \"" + buildPath + "\"";
                RunProcess(@".\GetDropFolder.cmd", copyArguments);

                WriteLine("Drop Path considered is : " + dropPath);
                WriteLine("Location Run location is : " + buildPath);

                WriteLine(Resources.AutomationStartTime + DateTime.Now);

                // Gets all the required paths for running automation
                string msTestPath = XmlHelper.MSTestPath;

                string automationDllPath =
                    buildPath + XmlHelper.AutomationDllPath.Replace("Tests", tests);
                string testSettingsPath = buildPath + XmlHelper.TestSettingsPath;
                string automationCategory = XmlHelper.AutomationCategory;
                string automationResultFolder =
                    XmlHelper.AutomationResultPath.Substring(0,
                    XmlHelper.AutomationResultPath.LastIndexOf("\\",
                    StringComparison.CurrentCulture));

                string automationResultPath = buildPath + XmlHelper.AutomationResultPath;

                if (CreateFolder(buildPath + automationResultFolder))
                {
                    // Building the arguments
                    StringBuilder arguments = new StringBuilder();
                    arguments.Append("\"");
                    arguments.Append(msTestPath);
                    arguments.Append("\" ");
                    arguments.Append("\"");
                    arguments.Append(automationDllPath);
                    arguments.Append("\" ");
                    arguments.Append("\"");
                    arguments.Append(testSettingsPath);
                    arguments.Append("\" ");
                    arguments.Append("\"");
                    arguments.Append(automationCategory);
                    arguments.Append("\" ");
                    arguments.Append("\"");
                    arguments.Append(automationResultPath);
                    arguments.Append("\"");

                    // Run the MSTest.cmd
                    RunProcess(@".\Mstest.cmd", arguments.ToString());
                    WriteLine(Resources.AutomationFinishTime + DateTime.Now);
                }
                else
                {
                    WriteLine("Could not create folder for saving automation result");
                }
            }
            finally
            {
                // Close the Log Writer
                if (logWriter != null)
                {
                    logWriter.Flush();
                    logWriter.Close();
                    logWriter = null;
                }
            }
        }

        /// <summary>
        /// Creates the folder/directory if it doesn't exist
        /// </summary>
        /// <param name="path">Folder/Directory path</param>
        /// <returns>Success/Fail</returns>
        /// Catching any type of exception which is occured while creating the Folder
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static bool CreateFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                WriteLine(string.Format((IFormatProvider)null, "Error '{0}' while creating Folder ",
                    ex.Message));
                return false;
            }

            return true;
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
                logWriter = new StreamWriter(automationLogFileName);
            }

            logWriter.Write(logMessage);
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
        /// Launches the specified process with a redirected output stream.
        /// </summary>
        /// <param name="processName">The process to be launched.</param>
        /// <param name="arguments">The command line arguments to the process.</param>
        /// Suppressed this message to execute the below code which requires exposure of methods
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
            "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
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
