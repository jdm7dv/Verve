﻿// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DeveloperPreRequisiteCheck
{
    /// <summary>
    /// This class implements IComponentValidator. 
    /// This class validates if the specified version of WIX is installed on the m/c.
    /// </summary>
    public class WIXValidator : IComponentValidator
    {
        /// <summary>
        /// Name of the environment variable which contains the path of installed WIX Path.
        /// </summary>
        private const string InstalledPath = "WIX_INSTALLPATH";

        /// <summary>
        /// Assembly or executable name of installed component.
        /// </summary>
        private const string FileName = "light.exe";

        /// <summary>
        /// Required config file.
        /// </summary>
        private const string RequiredConfig = "light.exe.config";

        /// <summary>
        /// Minimum required version of Windows Installer Xml.
        /// </summary>
        private const string MinimumVersion = "3.0.5419.0";

        /// <summary>
        /// Parameter required by Validator component.
        /// </summary>
        private Dictionary<string, string> _parameters = null;

        /// <summary>
        /// Default Constructor: Creates an instance of WIXValidator class.
        /// </summary>
        public WIXValidator()
        {
            _parameters = new Dictionary<string, string>();
        }

        /// <summary>
        /// Gets the name of component.
        /// </summary>
        public string Name
        {
            get { return Properties.Resources.WIX_NAME; }
        }

        /// <summary>
        /// Gets the short description of component.
        /// </summary>
        public string Description
        {
            get { return Properties.Resources.WIX_DESCRIPTION; }
        }

        /// <summary>
        /// Gets the minimum supported version of component.
        /// </summary>
        public string Version
        {
            get { return MinimumVersion; }
        }

        /// <summary>
        /// Gets the parameter required by Validator component.
        /// </summary>
        public Dictionary<string, string> Parameters { get { return _parameters; } }

        /// <summary>
        /// Validate if the component is installed.
        ///  1. If not, provide a message to install the component.
        ///  2.	If yes, provide a message directing user to copy the folders/assemblies to required target folder.
        /// </summary>
        /// <returns></returns>
        public ValidationResult Validate()
        {
            string filePath = string.Empty;
            ValidationResult result = null;

            if (_parameters.TryGetValue(Utility.PARAM_FILEPATH, out filePath))
            {
                result = VerifyVersion(filePath);
                if (result.Result)
                {
                    Utility.WriteEnvironmentVariable(InstalledPath, filePath);
                }
            }
            else if (Utility.ReadEnvironmentVariable(InstalledPath, out filePath))
            {
                result = VerifyVersion(filePath);
            }
            else
            {
                result = new ValidationResult(false,
                        string.Format(CultureInfo.CurrentCulture,
                            Properties.Resources.WIX_NOTFOUND,
                            MinimumVersion));
            }

            if (result.Result)
            {
                string content = string.Format(CultureInfo.InvariantCulture,
                    "SET {0}={1}", InstalledPath, filePath);
                Utility.WriteToFile(
                        _parameters[Utility.PARAM_RESETENVVARFILEPATH],
                        content);
            }

            return result;
        }

        /// <summary>
        /// Verify if the file exist, if yes, check the version.
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>Validation result</returns>
        private static ValidationResult VerifyVersion(string filePath)
        {
            string version, verifyFilePath;
            ValidationResult result = new ValidationResult(false,
                    string.Format(CultureInfo.CurrentCulture,
                        Properties.Resources.WIX_NOTFOUND,
                        MinimumVersion));

            verifyFilePath = Path.Combine(filePath, FileName);

            if (Utility.GetVersion(verifyFilePath, out version))
            {
                if (Utility.CompareVersion(MinimumVersion, version))
                {
                    result = new ValidationResult(true,
                            string.Format(CultureInfo.CurrentCulture,
                                Properties.Resources.WIX_FOUND,
                                version,
                                RequiredConfig,
                                filePath));
                }
            }

            return result;
        }
    }
}