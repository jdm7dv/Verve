// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using TFSAutomationRunAgentLib;

namespace TFSAutomationRunConsole
{
    /// <summary>
    /// TFSAutomationRun Console which starts the automation run
    /// </summary>
    class TFSAutomationRunConsole
    {
        /// <summary>
        /// Main method where the console application starts
        /// </summary>
        /// <param name="args">Arguments i.e., Tests/TestAutomation/PerfTests</param>
        static void Main(string[] args)
        {
            Console.WriteLine("Automation Run Started..");

            // Start automation run with the required arguments.
            TFSAutomation.StartAutomationRun(args[0]);
            
            Console.WriteLine("Automation Run Completed.");
        }
    }
}
