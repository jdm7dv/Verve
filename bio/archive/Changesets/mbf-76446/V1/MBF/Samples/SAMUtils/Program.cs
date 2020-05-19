// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using SAMUtils.Properties;

namespace SAMUtils
{
    /// <summary>
    /// Class having main method for SAMUtility.
    /// </summary>
    public class Program
    {
        #region MainMethod

        /// <summary>
        /// Main Method for parsing command line arguments.
        /// </summary>
        /// <param name="args">Command line arguments passed.</param>
        public static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    DisplayErrorMessage(Resources.SAMUtilHelp);
                }
                else
                {
                    string[] arguments = new string[args.Length - 1];
                    for (int index = 1; index < args.Length; index++)
                    {
                        arguments[index - 1] = args[index];
                    }

                    if (args[0].Equals("Import", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ImportOption(arguments);
                    }
                    else if (args[0].Equals("Sort", StringComparison.InvariantCultureIgnoreCase))
                    {
                        SortOption(arguments);
                    }
                    else if (args[0].Equals("Merge", StringComparison.InvariantCultureIgnoreCase))
                    {
                        MergeOption(arguments);
                    }
                    else if (args[0].Equals("View", StringComparison.InvariantCultureIgnoreCase))
                    {
                        ViewOption(arguments);
                    }
                    else if (args[0].Equals("Index", StringComparison.InvariantCultureIgnoreCase))
                    {
                        IndexOption(arguments);
                    }
                    else
                    {
                        DisplayErrorMessage(Resources.SAMUtilHelp);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    DisplayErrorMessage(ex.InnerException.Message);
                }
                else
                {
                    DisplayErrorMessage(ex.Message);
                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parse command line arguments for view command.
        /// </summary>
        /// <param name="args">Array of command line arguments.</param>
        private static void ViewOption(string[] args)
        {
            View options = new View();
            if (args.Length > 0 && CommandLine.Parser.ParseArguments(args, options))
            {
                if (options.Usage)
                {
                    DisplayErrorMessage(Resources.ViewHelp);
                }
                else
                {
                    options.ViewResult();
                }
            }
            else
            {
                DisplayErrorMessage(Resources.ViewHelp);
            }
        }

        /// <summary>
        /// Parse command line arguments for index command.
        /// </summary>
        /// <param name="args">Array of command line arguments.</param>
        private static void IndexOption(string[] args)
        {
            Index options = new Index();
            if (args.Length > 0 && CommandLine.Parser.ParseArguments(args, options))
            {
                if (options.Usage)
                {
                    DisplayErrorMessage(Resources.IndexHelp);
                }
                else
                {
                    options.GenerateIndexFile();
                }
            }
            else
            {
                DisplayErrorMessage(Resources.IndexHelp);
            }
        }

        /// <summary>
        /// Parse command line arguments for Import command
        /// </summary>
        /// <param name="args">Array of command line arguments.</param>
        private static void ImportOption(string[] args)
        {
            Import options = new Import();
            if (args != null && args.Length > 0 && CommandLine.Parser.ParseArguments(args, options))
            {
                if (options.Usage)
                {
                    DisplayErrorMessage(Resources.ImportHelp);
                }
                else
                {
                    options.DoImport();
                }
            }
            else
            {
                DisplayErrorMessage(Resources.ImportHelp);
            }
        }

        /// <summary>
        /// Parse command line arguments for Merge command
        /// </summary>
        /// <param name="args">Array of command line arguments.</param>
        private static void MergeOption(string[] args)
        {
            Merge options = new Merge();
            if (args != null && args.Length > 2 && CommandLine.Parser.ParseArguments(args, options))
            {
                if (options.Usage)
                {
                    DisplayErrorMessage(Resources.MergeHelp);
                }
                else
                {
                    options.DoMerge();
                }
            }
            else
            {
                DisplayErrorMessage(Resources.MergeHelp);
            }
        }

        /// <summary>
        /// Parse command line arguments for sort command
        /// </summary>
        /// <param name="args">Array of command line arguments.</param>
        private static void SortOption(string[] args)
        {
            Sort options = new Sort();
            if (args.Length > 0 && CommandLine.Parser.ParseArguments(args, options))
            {
                if (options.Usage)
                {
                    DisplayErrorMessage(Resources.SortHelp);
                }
                else
                {
                    options.DoSort();
                }
            }
            else
            {
                DisplayErrorMessage(Resources.SortHelp);
            }
        }

        /// <summary>
        /// Display error message on console.
        /// </summary>
        /// <param name="message">Error message.</param>
        private static void DisplayErrorMessage(string message)
        {
            Console.Write(message);
        }

        #endregion
    }
}