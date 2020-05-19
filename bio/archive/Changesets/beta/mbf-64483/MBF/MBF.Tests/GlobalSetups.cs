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
using System.Linq;
using System.Text;

using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.Tests
{
    /// <summary>
    /// this is global one-time initialization code for the test run.
    /// </summary>
    public class GlobalSetups
    {
        static bool ranSetup = false;

        /// <summary>
        /// Each TestClass class can call this from its Setup() to
        /// run any one-time setup code (primarily ensuring that the
        /// log is open with a known name).
        /// </summary>
        public static void Run()
        {
            if (!ranSetup)
            {
                Trace.Set(Trace.SeqWarnings);
                ApplicationLog.Open("MBF.Tests.log");
                ranSetup = true;
            }
        }
    }
}
