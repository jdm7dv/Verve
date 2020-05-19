// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Linq;

namespace DeveloperPreRequisiteCheck
{
    /// <summary>
    /// Entry point to Developer Pre-requisite check exe.
    /// Runs through a check for installation / existence of various components required 
    /// and reports appropriate actions to developer should perform to successfully 
    /// modify / compile MBF solution.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// User option for enabling silent validation.
        /// </summary>
        private static readonly string[] SilentSwitch = new string[] { "/s", "/S", "-s", "-S" };

        /// <summary>
        /// Entry method to Developer Pre-requisite check exe.
        /// </summary>
        /// <param name="args">command line arguments</param>
        public static int Main(string[] args)
        {
            bool isValidationSilent = false;

            try
            {
                if (args.Length == 1)
                {
                    if (SilentSwitch.Contains(args[0]))
                    {
                        isValidationSilent = true;
                    }
                    else
                    {
                        throw new Exception(string.Format(Properties.Resources.UNKNOWN_SWITCH, args[0]));
                    }
                }
                else if (args.Length > 1)
                {
                    throw new Exception(Properties.Resources.UNKNOWN_SWITCHES);
                }


                Validator validator = new Validator(isValidationSilent);
                validator.Validate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (!isValidationSilent)
                {
                    Console.WriteLine(Properties.Resources.PRESS_KEY);
                    Console.ReadKey();
                }
            }

            return 0;
        }
    }
}