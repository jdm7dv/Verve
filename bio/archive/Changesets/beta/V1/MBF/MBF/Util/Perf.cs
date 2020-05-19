// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Util
{
    using MBF.Util.Logging;
    using System;
    using System.Reflection;
    using System.Globalization;
    /// <summary>
    /// Performance counters
    /// </summary>
    public static class Perf
    {
        private static System.Diagnostics.Stopwatch _watch;
        private static long _memoryStart;
        private static string _methodName;
        
        /// <summary>
        /// Start the meters
        /// </summary>
        /// <param name="name">method name</param>
        public static void Start(string name)
        {
            _methodName = name;
            _watch = System.Diagnostics.Stopwatch.StartNew();
            _memoryStart = System.GC.GetTotalMemory(true);
        }

        /// <summary>
        /// End it and write into log file
        /// </summary>
        public static void End()
        {
            long memoryUsed = System.GC.GetTotalMemory(true) - _memoryStart;
            _watch.Stop();
            ApplicationLog.WriteLine(string.Format(CultureInfo.CurrentCulture, "Perf on: {0}      => Memory Used : {1} Bytes        => Time Taken : {2} Milliseconds", _methodName, memoryUsed, _watch.ElapsedMilliseconds));
        }


    }
}
