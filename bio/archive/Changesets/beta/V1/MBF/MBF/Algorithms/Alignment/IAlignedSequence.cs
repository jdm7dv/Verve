// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MBF.Algorithms.Alignment
{
    /// <summary>
    /// Interface to hold single aligned unit of alignment.
    /// </summary>
    public interface IAlignedSequence : ISerializable
    {
        /// <summary>
        /// Gets information about the AlignedSequence, like score, offsets, consensus, etc..
        /// </summary>
        Dictionary<string, object> Metadata { get; }

        /// <summary>
        /// Gets list of sequences, aligned as part of an alignment.
        /// </summary>
        IList<ISequence> Sequences { get; }
    }
}
