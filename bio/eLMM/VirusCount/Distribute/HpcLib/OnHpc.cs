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
using MBT.Escience.Parse;
using MBT.Escience.Distribute;

namespace MBT.Escience.HpcLib
{
    [Serializable]
    public class OnHpc : ClusterSubmitterArgs, IDistributor
    {
        public virtual void Distribute(IDistributable distributableObject)
        {
            ClusterSubmitter.Submit(this, distributableObject);
        }
    }
}
