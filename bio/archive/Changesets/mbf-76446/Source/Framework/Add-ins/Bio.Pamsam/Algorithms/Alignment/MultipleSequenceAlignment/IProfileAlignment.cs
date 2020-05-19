// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bio.Algorithms.Alignment.MultipleSequenceAlignment
{
    /// <summary>
    /// IProfileAlignment is an extension of ISequenceAlignment
    /// with two new members:
    /// 
    ///     - derived profiles from the set of alignment sequences in ISequenceAlignment
    ///     - float version alignment score
    ///     
    /// IProfileAlignment represents multiple sequence alignment as well as the profiles.
    /// (ISequenceAlignment represents a pair of aligned sequences).
    /// </summary>
    public interface IProfileAlignment : ISequenceAlignment
    {

        /// <summary>
        /// The score for the alignment. Higher scores mean better alignments.
        /// The score is determined by the alignment algorithm used.
        /// </summary>
        new float Score { set; get; }

        /// <summary>
        /// The profiles converted from ISequenceAlignment
        /// </summary>
        IProfiles ProfilesMatrix { get; set; }

        /// <summary>
        /// The number of sequences in the profile alignment
        /// </summary>
        int NumberOfSequences { get; set; }
    }
}