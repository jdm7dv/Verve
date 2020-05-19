// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;

namespace MBF.IO.GenBank
{
    /// <summary>
    /// A StrandTopology specifies whether the strand is linear or circular.
    /// </summary>
    [Serializable]
    public enum SequenceStrandTopology
    {
        /// <summary>
        /// None - StrandTopology is unspecified.
        /// </summary>
        None,

        /// <summary>
        /// Linear.
        /// </summary>
        Linear,

        /// <summary>
        /// Circular.
        /// </summary>
        Circular,
    }
}
