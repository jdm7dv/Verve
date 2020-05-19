using System;
using System.Globalization;
using System.Xml;
using TFSLocalBuildAgentLib.Properties;

namespace TFSLocalBuildAgentLib
{
    /// <summary>
    /// A helper class which handles the interaction with the XML configuration file.
    /// </summary>
    public static class XmlHelper
    {
        #region Fields
        /// <summary>
        /// The XML configuration document object.
        /// </summary>
        private static XmlDocument configFile;

        /// <summary>
        /// The XML configuration file name.
        /// </summary>
        private const string configFileName = "Config.xml";
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the path to the download source.
        /// </summary>
        public static string DownloadSourcePath
        {
            get
            {
                return GetValue("DownloadSourcePath");
            }
            set
            {
                SetValue("DownloadSourcePath", value);
            }
        }

        /// <summary>
        /// Gets or sets the last build id.
        /// </summary>
        public static int LastBuildNumber
        {
            get
            {
                return Int32.Parse(GetValue("LastBuildNumber"), CultureInfo.InvariantCulture);
            }
            set
            {
                SetValue("LastBuildNumber", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets or sets the date on which the solution was last built.
        /// </summary>
        public static string LastBuildDate
        {
            get
            {
                return GetValue("LastBuildDate");
            }
            set
            {
                SetValue("LastBuildDate", value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the team project collection on the TFS server.
        /// </summary>
        public static string TeamProjectCollectionName
        {
            get
            {
                return GetValue("TeamProjectCollectionName");
            }
            set
            {
                SetValue("TeamProjectCollectionName", value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the project on the TFS server.
        /// </summary>
        public static string ProjectName
        {
            get
            {
                return GetValue("ProjectName");
            }
            set
            {
                SetValue("ProjectName", value);
            }
        }

        /// <summary>
        /// Gets or sets the drop path.
        /// </summary>
        public static string DropLocation
        {
            get
            {
                return GetValue("DropLocation");
            }
            set
            {
                SetValue("DropLocation", value);
            }
        }

        /// <summary>
        /// Gets or sets the local workspace path.
        /// </summary>
        public static string LocalWorkspacePath
        {
            get
            {
                return GetValue("LocalWorkspacePath");
            }
            set
            {
                SetValue("LocalWorkspacePath", value);
            }
        }

        /// <summary>
        /// Gets or sets the TFS server URI.
        /// </summary>
        public static Uri ServerUri
        {
            get
            {
                return new Uri(GetValue("ServerUri"));
            }
            set
            {
                if ((value as Uri) == null)
                {
                    throw new ArgumentException(Resources.InvalidUriException);
                }

                SetValue("ServerUri", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets the name of the build log file.
        /// </summary>
        public static string BuildLogFileName
        {
            get
            {
                return GetValue("BuildLogFileName");
            }
            set
            {
                SetValue("BuildLogFileName", value);
            }
        }

        /// <summary>
        /// Gets or sets the id of the most recent changeset.
        /// </summary>
        public static int LastBuiltChangesetId
        {
            get
            {
                return Int32.Parse(GetValue("LastBuiltChangesetId"), CultureInfo.InvariantCulture);
            }
            set
            {
                SetValue("LastBuiltChangesetId", value.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Gets or sets the name of the local workspace.
        /// </summary>
        public static string WorkspaceName
        {
            get
            {
                return GetValue("WorkspaceName");
            }
            set
            {
                SetValue("WorkspaceName", value);
            }
        }

        /// <summary>
        /// Gets the relative path to the binary folder.
        /// </summary>
        public static string BinaryPath
        {
            get
            {
                string relativePath = GetValue("BinaryRelPath");
                return LocalWorkspacePath + relativePath;
            }
        }

        /// <summary>
        /// Gets the relative path to the installer (msi) folder.
        /// </summary>
        public static string InstallerPath
        {
            get
            {
                string relativePath = GetValue("InstallerRelPath");
                return LocalWorkspacePath + relativePath;
            }
        }

        /// <summary>
        /// Gets the relative path to the test automation cleanup file.
        /// </summary>
        public static string TestAutomationCleanupFile
        {
            get
            {
                string relativePath = GetValue("TestAutomationCleanupFileRelPath");
                return LocalWorkspacePath + relativePath;
            }
        }

        /// <summary>
        /// Gets the relative path to the test automation settings file.
        /// </summary>
        public static string TestAutomationSettingsFile
        {
            get
            {
                string relativePath = GetValue("TestAutomationSettingsRelPath");
                return LocalWorkspacePath + relativePath;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Loads the XML document into memory.
        /// </summary>
        public static void LoadConfiguration()
        {
            configFile = new XmlDocument();
            configFile.Load(configFileName);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the value for the specified tag in the XML file.
        /// </summary>
        /// <param name="tagName">The tag to get the value for.</param>
        /// <returns>The value for the specified tag.</returns>
        private static string GetValue(string tagName)
        {
            if (configFile == null)
            {
                LoadConfiguration();
            }

            return configFile.DocumentElement.SelectSingleNode(tagName).InnerText;
        }

        /// <summary>
        /// Sets the value for the specified tag in the XML file.
        /// </summary>
        /// <param name="tagName">The tag to set the value for.</param>
        /// <param name="content">The value for the specified tag.</param>
        private static void SetValue(string tagName, string content)
        {
            if (configFile == null)
            {
                LoadConfiguration();
            }

            configFile.DocumentElement.SelectSingleNode(tagName).InnerText = content;
            configFile.Save(configFileName);
        }

        #endregion
    }
}