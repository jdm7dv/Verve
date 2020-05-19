// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using CommandLine;
using RepeatResolutionUtil.Properties;

namespace RepeatResolutionUtil
{
    /// <summary>
    /// Class that provides options to execute step 2 (RepeatResolution) of Comparative assembly.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method of the utility.
        /// </summary>
        /// <param name="args">Arguments to the main method.</param>
        public static void Main(string[] args)
        {
            try
            {
                if ((args == null) || (args.Length < 1))
                {
                    DisplayErrorMessage(Resources.RepeatResolutionUtilHelp);
                }
                else
                {
                    if (args[0].Equals("Help", StringComparison.OrdinalIgnoreCase))
                    {
                        DisplayErrorMessage(Resources.RepeatResolutionUtilHelp);
                    }
                    else
                    {
                        RepeatResolver(args);
                    }
                }
            }
            catch (Exception ex)
            {
                CatchInnerException(ex);
            }
        }

        #region Private Methods

        /// <summary>
        /// Catches Inner Exception Messages.
        /// </summary>
        /// <param name="ex">The Exception.</param>
        private static void CatchInnerException(Exception ex)
        {
            if (ex.InnerException == null || string.IsNullOrEmpty(ex.InnerException.Message))
            {
                DisplayErrorMessage(ex.Message);
            }
            else
            {
                CatchInnerException(ex.InnerException);
            }
        }

        /// <summary>
        /// Parses RepeatResolver command line parameters.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void RepeatResolver(string[] args)
        {
            RepeatResolutionArguments arguments = new RepeatResolutionArguments();
            if (args.Length > 1 && Parser.ParseArguments(args, arguments))
            {
                if (!string.IsNullOrEmpty(arguments.FilePath))
                {
                    arguments.ResolveAmbiguity();
                }
                else
                {
                    DisplayErrorMessage(Resources.RepeatResolutionUtilHelp);
                }
            }
            else
            {
                DisplayErrorMessage(Resources.RepeatResolutionUtilHelp);
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

        /// <summary>
        /// Removes the command line.
        /// </summary>
        /// <param name="args">Arguments to remove.</param>
        /// <returns>Returns the arguments.</returns>
        private static string[] RemoveCommandLine(string[] args)
        {
            string[] arguments = new string[args.Length - 1];
            for (int index = 0; index < args.Length - 1; index++)
            {
                arguments[index] = args[index + 1];
            }

            return arguments;
        }

        #endregion
    }
}
