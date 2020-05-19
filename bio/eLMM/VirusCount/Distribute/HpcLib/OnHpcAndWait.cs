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

namespace MBT.Escience.HpcLib
{
    [Serializable]
    public class OnHpcAndWait : OnHpc
    {
        /// <summary>
        /// A list of file patterns that will be copied from the cluster working directory to the local working directory.
        /// </summary>
        public List<string> CopyResults = new List<string>();

        public int MaxSubmitAfterTasksFail = 0;

        public override void Distribute(Distribute.IDistributable distributableObject)
        {
            ClusterSubmitter.SubmitAndWait(this, distributableObject, MaxSubmitAfterTasksFail);

            if(CopyResults.Count > 0)
                HpcLib.CopyFiles(CopyResults, ExternalRemoteDirectoryName, ".");
        }
    }
}
