// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Runtime.InteropServices;

namespace MBF.Algorithms.Alignment.MultipleSequenceAlignment
{
    /// <summary>
    /// 
    /// </summary>
    public class Performance
    {
        //
        [DllImport("Kernel32.dll")]
        private static extern int GetTickCount();

        
        /// <summary>
        /// DateTime.Now.Ticks;
        /// </summary>
        public static void Start()
        {
            start = GetTickCount();
        }

        /// <summary>
        /// DateTime.Now.Ticks;
        /// </summary>
        /// <param name="activity"></param>
        public static void Snapshot(string activity)
        {
            int stop = GetTickCount();
            Console.Out.WriteLine((stop - start) + "\t" + activity);
        }

        /// <summary>
        /// 
        /// </summary>
        static int start;

        /*
        long m_memoryStart = 0;
        long m_memoryEnd = 0;
 
        Thread.MemoryBarrier();
        m_memoryStart = System.GC.GetTotalMemory(true);
 
        List<int> testList1 = new List<int>();
        Thread.MemoryBarrier();
        m_memoryEnd = System.GC.GetTotalMemory(true);
        */
    }
}
