//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bio.Algorithms.Alignment
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
