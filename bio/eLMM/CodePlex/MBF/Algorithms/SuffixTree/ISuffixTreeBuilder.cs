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
using Bio.Algorithms.Alignment;

namespace Bio.Algorithms.SuffixTree
{
    /// <summary>
    /// This interface defines contract for classes implementing Suffix Tree Builder Algorithm.
    /// Suffix tree algorithm that takes a sequence as an input and produces an 
    /// Suffix Tree of the sequence as output.
    /// </summary>
    public interface ISuffixTreeBuilder
    {
        /// <summary>
        /// Build a Suffix Tree for the given input sequence.
        /// </summary>
        /// <param name="sequence">Sequence to build Suffix Tree for</param>
        /// <returns>Suffix Tree</returns>
        SequenceSuffixTree BuildSuffixTree(ISequence sequence);

        /// <summary>
        /// Find the matches of sequence in suffix tree
        /// </summary>
        /// <param name="suffixTree">Suffix Tree</param>
        /// <param name="searchSequence">Query searchSequence</param>
        /// <param name="lengthOfMUM">Mininum length of MUM</param>
        /// <returns>Matches found</returns>
        IList<MaxUniqueMatch> FindMatches(SequenceSuffixTree suffixTree, ISequence searchSequence, long lengthOfMUM);
    }
}
