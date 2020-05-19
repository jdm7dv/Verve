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
using System.Threading.Tasks;
using MBT.Escience.Parse;

namespace MBT.Escience.Distribute
{
    /// <summary>
    /// Runs tasks locally.
    /// </summary>
    public class Locally : IDistributor, IParsable
    {
        /// <summary>
        /// How many pieces should the work be divided into?
        /// </summary>
        public int TaskCount = 1;

        /// <summary>
        /// The set of Tasks that should be run by this instance
        /// </summary>
        public RangeCollection Tasks = new RangeCollection(0);

        /// <summary>
        /// Specifies whether cleanup should be run when this task is complete.
        /// </summary>
        public bool Cleanup = true;

        /// <summary>
        /// Specifies the local parallel options
        /// </summary>
        [Parse(ParseAction.Optional, typeof(ParallelOptionsParser))]
        public ParallelOptions ParallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };

        /// <summary>
        /// Runs Tasks locally on distributableObject.
        /// </summary>
        /// <param name="distributableObject">The object that will run the tasks.</param>
        public void Distribute(IDistributable distributableObject)
        {
            using (ParallelOptionsScope.Create(ParallelOptions))
            {
                distributableObject.RunTasks(Tasks, TaskCount);

                if (Cleanup)
                    distributableObject.Cleanup(TaskCount);
            }
        }

        public void FinalizeParse()
        {
            Helper.CheckCondition<ParseException>(TaskCount > 0, "TaskCount must be at least 1.");
            Helper.CheckCondition<ParseException>(Tasks.Count() == 0 || Tasks.FirstElement >= 0 && Tasks.LastElement < TaskCount, "The tasks range {0} is not between 0 and TaskCount {1}", Tasks, TaskCount);
        }
    }
}
