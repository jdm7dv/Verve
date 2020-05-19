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
using Bio.Util;
using MBT.Escience.Parse;

namespace MBT.Escience.Distribute
{
    /// <summary>
    /// Convience base class for classes that wish to be executed from the command line with a -Distribute option for submitting to cluster.
    /// </summary>
    public abstract class SelfDistributable : IDistributable, IRunnable
    {
        /// <summary>
        /// How to distribute this object.
        /// </summary>
        public IDistributor Distribute = new Locally() { ParallelOptions = new System.Threading.Tasks.ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount } };

        abstract public void RunTasks(RangeCollection tasksToRun, long taskCount);

        abstract public void Cleanup(long taskCount);

        public void Run()
        {
            var distribute = Distribute;
            this.Distribute = null;
            distribute.Distribute(this);    // clear distribute in case we're submitting to cluster. That's a lot of command line junk to send to the cluster for no reason!
            this.Distribute = distribute;
        }
    }
}
