// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;

namespace DeveloperPreRequisiteCheck
{
    /// <summary>
    /// This class runs through all the compenents and validate there version compatibility.
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// File to be which script to reset environment variable has to be written
        /// </summary>
        private const string ResetEnvVariableBatchFile = "ResetEnvironmentVariable.bat";

        /// <summary>
        /// SandCastle validator component.
        /// </summary>
        private IComponentValidator _SandCastleValidator = null;

        /// <summary>
        /// WIX validator component.
        /// </summary>
        private IComponentValidator _WIXValidator = null;

        /// <summary>
        /// HHC validator component.
        /// </summary>
        private IComponentValidator _HHCValidator = null;

        /// <summary>
        /// Flag to check whether the validation should run in silent mode.
        /// </summary>
        private bool _IsValidationSilent = false;

        /// <summary>
        /// Default constructor: Initialized an instance of Validator class.
        /// </summary>
        /// <param name="isValidationSilent">Is validation silent.</param>
        public Validator(bool isValidationSilent)
        {
            _IsValidationSilent = isValidationSilent;
        }

        /// <summary>
        /// Initialize validators and validate all the components
        /// </summary>
        public void Validate()
        {
            // Initialize the components.
            IList<IComponentValidator> components = Initialize();
            string lineBreak = "";

            lineBreak = lineBreak.PadLeft(80, '*');
            // Validate the components.
            foreach (IComponentValidator component in components)
            {
                Validate(component);
                Console.WriteLine(String.Empty);
                Console.WriteLine(lineBreak);
            }
        }

        /// <summary>
        /// Validate the requried component.
        /// </summary>
        /// <param name="component">component to be validated.</param>
        private void Validate(IComponentValidator component)
        {
            ValidationResult result = component.Validate();

            bool repeat = false;
            do
            {
                // Write an empty line.
                Console.WriteLine(String.Empty);
                Console.WriteLine(result.Message);

                if (result.Result)
                {
                    return;
                }

                if (!_IsValidationSilent && 
                    (component == _SandCastleValidator
                    || component == _WIXValidator
                    || component == _HHCValidator))
                {
                    Console.Write(Properties.Resources.ACCEPT_CHOICE);
                    string choice = Console.ReadLine();

                    if (choice.Trim().Equals("y", StringComparison.OrdinalIgnoreCase))
                    {
                        string sampelFilePath = string.Empty;

                        if (component == _SandCastleValidator)
                        {
                            sampelFilePath = Utility.SAMPLE_SANDCASTLEPATH;
                        }
                        else if (component == _WIXValidator)
                        {
                            sampelFilePath = Utility.SAMPLE_WIXPATH;
                        }
                        else if (component == _HHCValidator)
                        {
                            sampelFilePath = Utility.SAMPLE_HHCPATH;
                        }

                        Console.Write(string.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resources.ACCEPT_FILEPATH,
                            sampelFilePath));
                        string filePath = Console.ReadLine();

                        if (component == _SandCastleValidator)
                        {
                            component.Parameters[Utility.PARAM_FILEPATH] = filePath;
                        }
                        else if (component == _WIXValidator)
                        {
                            component.Parameters[Utility.PARAM_FILEPATH] = filePath;
                        }
                        else if (component == _HHCValidator)
                        {
                            component.Parameters[Utility.PARAM_FILEPATH] = filePath;
                        }

                        result = component.Validate();
                        repeat = true;
                    }
                    else
                    {
                        repeat = false;
                    }
                }
            }
            while (repeat == true);
        }

        /// <summary>
        /// Initialize the validators
        /// </summary>
        /// <returns></returns>
        private IList<IComponentValidator> Initialize()
        {
            IList<IComponentValidator> components = new List<IComponentValidator>();
            Utility.DeleteFile(ResetEnvVariableBatchFile);

            _SandCastleValidator = new SandCastleValidator();
            _SandCastleValidator.Parameters[Utility.PARAM_RESETENVVARFILEPATH] = ResetEnvVariableBatchFile;
            components.Add(_SandCastleValidator);

            _HHCValidator = new HHCValidator();
            _HHCValidator.Parameters[Utility.PARAM_RESETENVVARFILEPATH] = ResetEnvVariableBatchFile;
            components.Add(_HHCValidator);

            _WIXValidator = new WIXValidator();
            _WIXValidator.Parameters[Utility.PARAM_RESETENVVARFILEPATH] = ResetEnvVariableBatchFile;
            components.Add(_WIXValidator);

            return components;
        }
    }
}
