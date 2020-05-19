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
using PadenaUtil.Properties;

namespace PadenaUtil
{
    /// <summary>
    /// Program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main function of program class.
        /// </summary>
        /// <param name="args">Arguments to the main function.</param>
        public static void Main(string[] args)
        {
            try
            {
                if ((args == null) || (args.Length < 1))
                {
                    DisplayErrorMessage(Resources.PadenaUtilHelp);
                }
                else
                {
                    if (args[0].Equals("Help", StringComparison.InvariantCultureIgnoreCase))
                    {
                        DisplayErrorMessage(Resources.PadenaUtilHelp);
                    }
                    else if (args[0].Equals("Assemble", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Assemble(RemoveCommandLine(args));
                    }
                    else if (args[0].Equals("AssembleWithScaffold", StringComparison.InvariantCultureIgnoreCase))
                    {
                        AssembleWithScaffolding(RemoveCommandLine(args));
                    }
                    else if (args[0].Equals("Scaffold", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Scaffold(RemoveCommandLine(args));
                    }
                    else
                    {
                        DisplayErrorMessage(Resources.PadenaUtilHelp);
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
        /// <param name="ex">Exception</param>
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
        /// Scaffold function.
        /// </summary>
        /// <param name="args">Arguments to Scaffold function.</param>
        private static void Scaffold(string[] args)
        {
            ScaffoldArguments options = new ScaffoldArguments();
            if (args.Length > 0 && Parser.ParseArguments(args, options))
            {
                if (options.FileNames != null)
                {
                    options.GenerateScaffold();
                }
                else
                {
                    DisplayErrorMessage(Resources.ScaffoldHelp);
                }
            }
            else
            {
                DisplayErrorMessage(Resources.ScaffoldHelp);
            }
        }

        /// <summary>
        /// Assemble With Scaffolding.
        /// </summary>
        /// <param name="args">Arguments to Scaffolding.</param>
        private static void AssembleWithScaffolding(string[] args)
        {
            AssembleWithScaffoldArguments options = new AssembleWithScaffoldArguments();
            if (args.Length > 0 && Parser.ParseArguments(args, options))
            {
                if (options.Help)
                {
                    DisplayErrorMessage(Resources.AssembleWithScaffoldHelp);
                }
                else
                {
                    options.AssembleSequences();
                }
            }
            else
            {
                DisplayErrorMessage(Resources.AssembleWithScaffoldHelp);
            }
        }

        /// <summary>
        /// Assemble function.
        /// </summary>
        /// <param name="args">Arguments to Assemble.</param>
        private static void Assemble(string[] args)
        {
            AssembleArguments options = new AssembleArguments();
            if (args.Length > 0 && Parser.ParseArguments(args, options))
            {
                if (options.Help)
                {
                    DisplayErrorMessage(Resources.AssembleHelp);
                }
                else
                {
                    options.AssembleSequences();
                }
            }
            else
            {
                DisplayErrorMessage(Resources.AssembleHelp);
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
