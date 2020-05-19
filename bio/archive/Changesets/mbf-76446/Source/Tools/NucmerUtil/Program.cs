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
using System.Diagnostics;
using CommandLine;
using NucmerUtil.Properties;

namespace NucmerUtil
{
    /// <summary>
    /// Class having main method for NUCmer Utility.
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
            DisplayErrorMessage(Resources.NUCmerSplashScreen);
            try
            {
                if ((args == null) || (args.Length < 2))
                {
                    DisplayErrorMessage(Resources.NUCmerUtilHelp);
                }
                else
                {
                    Stopwatch nucmerWatch = new Stopwatch();
                    nucmerWatch.Restart();
                    Alignment(args);
                    nucmerWatch.Stop();
                    Console.WriteLine("           Total NucmerUtil Runtime: {0}", nucmerWatch.Elapsed);
                }
            }
            catch (Exception ex)
            {
                DisplayException(ex);
            }

            
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Parses NUCmer command line parameters.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Alignment(string[] args)
        {
            NucmerArguments arguments = new NucmerArguments();
            
            if (args.Length > 1 && Parser.ParseArguments(args, arguments))
            {
                if (arguments.FilePath.Length == 2)
                {
                    arguments.Align();
                }
                else
                {
                    DisplayErrorMessage(Resources.NUCmerUtilHelp);
                }
            }
            else
            {
                DisplayErrorMessage(Resources.NUCmerUtilHelp);
            }
        }

        /// <summary>
        /// Display Exception Messages, if inner exception found then displays the inner exception.
        /// </summary>
        /// <param name="ex">The Exception.</param>
        private static void DisplayException(Exception ex)
        {
            if (ex.InnerException == null || string.IsNullOrEmpty(ex.InnerException.Message))
            {
                DisplayErrorMessage(ex.Message);
            }
            else
            {
                DisplayException(ex.InnerException);
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
