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
using System.Collections.Generic;
using Bio.Algorithms.Alignment;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// A sequence alignment algorithm that aligns exactly two 
    /// sequences. This may diverge from ISequenceAligner at some 
    /// point; meanwhile, it's important to maintain the distinction
    /// (e.g., assembly requires a pairwise algorithm).
    /// </summary>
    public interface IPairwiseSequenceAligner : ISequenceAligner
    {
        /// <summary>
        /// A convenience method - we know there are exactly two inputs.
        /// AlignSimple uses a linear gap penalty.
        /// </summary>
        /// <param name="sequence1">First input sequence.</param>
        /// <param name="sequence2">Second input sequence.</param>
        /// <returns>List of Aligned Sequences.</returns>
        IList<IPairwiseSequenceAlignment> AlignSimple(ISequence sequence1, ISequence sequence2);

        /// <summary>
        /// A convenience method - we know there are exactly two inputs.
        /// Align uses the affine gap model, which requires a gap open and a gap extension penalty.
        /// </summary>
        /// <param name="sequence1">First input sequence.</param>
        /// <param name="sequence2">Second input sequence.</param>
        /// <returns>List of Aligned Sequences.</returns>
        IList<IPairwiseSequenceAlignment> Align(ISequence sequence1, ISequence sequence2);
    }
}
