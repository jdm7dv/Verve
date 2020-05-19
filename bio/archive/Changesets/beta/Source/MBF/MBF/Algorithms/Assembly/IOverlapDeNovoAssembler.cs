// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using MBF.Algorithms.Alignment;

namespace MBF.Algorithms.Assembly
{
    /// <summary>
    /// Representation of any sequence assembly algorithm.
    /// This interface defines contract for classes implementing 
    /// overlap based De Novo Sequence assembler.
    /// </summary>
    public interface IOverlapDeNovoAssembler : IDeNovoAssembler
    {
        /// <summary>
        /// Gets or sets Threshold that determines how much overlap is needed 
        /// for two sequences to be merged. The score from the overlap algorithm 
        /// must at least equal Threshold for a merge to occur.
        /// </summary>
        double MergeThreshold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether standard orientation is assumed.
        /// if true, assume that the input sequences are in 5'-to-3' orientation.
        /// This means that only normal and reverse-complement overlaps need to be tested.
        /// if false, need to try both orientations for overlaps.
        /// </summary>
        bool AssumeStandardOrientation { get; set; }

        /// <summary>
        /// Gets or sets the pairwise sequence aligner that will be used to compute overlap during assembly.
        /// </summary>
        ISequenceAligner OverlapAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the method that will be used to compute a contig's consensus during assembly.
        /// </summary>
        IConsensusResolver ConsensusResolver { get; set; }
    }
}