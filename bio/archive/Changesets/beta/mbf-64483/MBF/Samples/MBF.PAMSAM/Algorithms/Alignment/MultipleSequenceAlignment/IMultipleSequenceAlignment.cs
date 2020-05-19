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
    /// 
    /// (Adapted from Wikipedia:)
    /// A multiple sequence alignment (MSA) is a sequence alignment 
    /// of three or more biological sequences, generally protein, 
    /// DNA, or RNA. In many cases, the input set of query sequences 
    /// are assumed to have an evolutionary relationship by which 
    /// they share a lineage and are descended from a common ancestor. 
    /// From the resulting MSA, sequence homology can be inferred and 
    /// phylogenetic analysis can be conducted to assess the sequences' 
    /// shared evolutionary origins. In multiple sequence alignment 
    /// visualization, differing characters in a single 
    /// alignment column, and insertion or deletion mutations 
    /// (indels or gaps) that appear as hyphens in one or more of the 
    /// sequences in the alignment. 
    /// Multiple sequence alignment is often used to assess sequence 
    /// conservation of protein domains, tertiary and secondary 
    /// structures, and even individual amino acids or nucleotides.
    /// </summary>
    public interface IMultipleSequenceAlignment
    {
        /// <summary>
        /// Aligned sequences with equal length by inserting gaps '-' at
        /// appropriate positions so that the alignment score is optimized.
        /// </summary>
        List<ISequence> AlignedSequences { get; }

        /// <summary>
        /// The alignment score of the multiple sequence alignment.
        /// A typical score is the summation of pairwise alignment scores.
        /// </summary>
        float AlignmentScore { get; }

        /// <summary>
        /// The method to align multiple sequences.
        /// The gap penalty is affine gap score.
        /// </summary>
        /// <param name="sequences">a set of unaligned sequences</param>
        void Align(List<ISequence> sequences);

        /// <summary>
        /// The name of multiple sequence alignment method.
        /// </summary>
        string Name { get; }
    }
}
