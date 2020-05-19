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

namespace MBT.Escience.Distribute
{
    /// <summary>
    /// An interface that describes an object that can distribute something of type IDistributable.
    /// </summary>
    public interface IDistributor
    {
        /// <summary>
        /// Run the work on this distributableObject
        /// </summary>
        /// <param name="distributableObject"></param>
        void Distribute(IDistributable distributableObject);
    }
}
