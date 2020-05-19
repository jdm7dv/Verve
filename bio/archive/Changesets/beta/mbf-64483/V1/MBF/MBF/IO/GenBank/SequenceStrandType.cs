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
    /// A StrandType specifies whether sequence occurs as a single stranded,
    /// double stranded or mixed stranded. 
    /// </summary>
    [Serializable]
    public enum SequenceStrandType
    {
        /// <summary>
        /// None - StrandType is unspecified.
        /// </summary>
        None,

        /// <summary>
        /// Single-stranded (ss).
        /// </summary>
        Single,

        /// <summary>
        /// Double-stranded (ds).
        /// </summary>
        Double,

        /// <summary>
        /// Mixed-stranded (ms).
        /// </summary>
        Mixed
    }
}
