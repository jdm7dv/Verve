//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bio.Util;

namespace MBT.Escience.Distribute
{
    /// <summary>
    /// An interface specifying that an object can be distributed in a delightfully parallel way.
    /// </summary>
    public interface IDistributable
    {
        /// <summary>
        /// Will run a subset of tasks.
        /// </summary>
        /// <param name="taskCount">Total number of pieces into which work will be divided.</param>
        /// <param name="tasksToRun">The pieces that should be run.</param>
        void RunTasks(RangeCollection tasksToRun, long taskCount);

        /// <summary>
        /// Called when all tasks are complete.
        /// </summary>
        void Cleanup(long taskCount);
    }
}
