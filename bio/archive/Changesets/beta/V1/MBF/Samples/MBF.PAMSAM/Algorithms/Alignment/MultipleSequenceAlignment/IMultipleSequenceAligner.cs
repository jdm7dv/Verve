// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF.Algorithms.Alignment.MultipleSequenceAlignment
{
    /// <summary>
    /// Multiple sequences alignment interface.
    /// Multiple sequence alignment (MSA) is used to align three or  
    /// more sequences in preparation for further analysis.  
    /// More info on MSA can be found at 
    /// http://en.wikipedia.org/wiki/Multiple_sequence_alignment)
    /// </summary>
    public interface IMultipleSequenceAligner : ISequenceAligner
    {
        /// <summary>
        /// Gets aligned sequences with equal length by inserting gaps '-' at
        /// appropriate positions so that the alignment score is optimized.
        /// </summary>
        List<ISequence> AlignedSequences { get; }

        /// <summary>
        /// Gets the alignment score of the multiple sequence alignment.
        /// A typical score is the summation of pairwise alignment scores.
        /// </summary>
        float AlignmentScore { get; }
    }
}
