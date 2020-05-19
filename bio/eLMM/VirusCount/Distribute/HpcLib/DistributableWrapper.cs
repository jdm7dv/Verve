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
using MBT.Escience.Distribute;
using MBT.Escience.Parse;
using Bio.Util;

namespace MBT.Escience.HpcLib
{
    class DistributableWrapper : IDistributable
    {
        private CommandArguments _argCollection;

        //private ClusterSubmitterArgs _clusterArgs;



        public DistributableWrapper(CommandArguments argCollection)
        {
            _argCollection = argCollection;
            _argCollection.ExtractOptional<int>("TaskCount", -1);   // get rid of this, because it will be added again later.
            _argCollection.ExtractOptional<RangeCollection>("Tasks", null);   // get rid of this, because it will be added again later.
            _argCollection.ExtractOptional<bool>("Cleanup", true);   // get rid of this, because it will be added again later.

            //_clusterArgs = clusterArgs;
        }

        public void RunTasks(Bio.Util.RangeCollection tasksToRun, long taskCount)
        {
            throw new NotImplementedException();
        }

        public void Cleanup(long taskCount)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a string representation in which the Distributor is turned into a CommandArguments string and appended to the end of the argument collection.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //CommandArguments distribAsCmd = CommandArguments.FromParsable(_clusterArgs);
            //distribAsCmd.ExtractOptional<string>("ParallelOptions", null);  // this was not guaranteed to be known about before. DistributeLocally will add it, so if your program already expected, there will now be two.
            string result = _argCollection.ToString();// +" " + distribAsCmd.ToString();
            return result;
        }
    }
}
