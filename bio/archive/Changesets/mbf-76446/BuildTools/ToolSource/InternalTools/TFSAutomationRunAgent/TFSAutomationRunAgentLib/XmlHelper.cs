// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Xml;

namespace TFSAutomationRunAgentLib
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
        /// Gets or sets the Local Run Path.
        /// </summary>
        public static string LocalRunLocation
        {
            get
            {
                return GetValue("LocalRunLocation");
            }
            set
            {
                SetValue("LocalRunLocation", value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the build log file.
        /// </summary>
        public static string AutomationLogFileName
        {
            get
            {
                return GetValue("AutomationLogFileName");
            }
            set
            {
                SetValue("AutomationLogFileName", value);
            }
        }

        /// <summary>
        /// Gets or sets the relative path to the binary folder.
        /// </summary>
        public static string BinaryRelativePath
        {
            get
            {
                return GetValue("BinaryRelPath");
            }
            set
            {
                SetValue("BinaryRelPath", value);
            }

        }

        /// <summary>
        /// Gets or sets the path to the MSTest path.
        /// </summary>
        public static string MSTestPath
        {
            get
            {
                return GetValue("MSTestPath");
            }
            set
            {
                SetValue("MSTestPath", value);
            }
        }

        /// <summary>
        /// Gets or sets the path to the automation dll.
        /// </summary>
        public static string AutomationDllPath
        {
            get
            {
                return GetValue("AutomationDllPath");
            }
            set
            {
                SetValue("AutomationDllPath", value);
            }
        }

        /// <summary>
        /// Gets or sets the path to the local.testsettings.
        /// </summary>
        public static string TestSettingsPath
        {
            get
            {
                return GetValue("TestSettingsPath");
            }
            set
            {
                SetValue("TestSettingsPath", value);
            }
        }

        /// <summary>
        /// Gets or sets the automation category level.
        /// </summary>
        public static string AutomationCategory
        {
            get
            {
                return GetValue("AutomationCategory");
            }
            set
            {
                SetValue("AutomationCategory", value);
            }
        }

        /// <summary>
        /// Gets or sets the path to the automation result.
        /// </summary>
        public static string AutomationResultPath
        {
            get
            {
                return GetValue("AutomationResultPath");
            }
            set
            {
                SetValue("AutomationResultPath", value);
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