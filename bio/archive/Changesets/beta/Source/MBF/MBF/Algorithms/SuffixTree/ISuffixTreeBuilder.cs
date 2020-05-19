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

namespace MBF.Algorithms.SuffixTree
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
        ISuffixTree BuildSuffixTree(ISequence sequence);

        /// <summary>
        /// Find the matches of sequence in suffix tree
        /// </summary>
        /// <param name="suffixTree">Suffix Tree</param>
        /// <param name="searchSequence">Query searchSequence</param>
        /// <param name="lengthOfMUM">Mininum length of MUM</param>
        /// <returns>Matches found</returns>
        IList<MaxUniqueMatch> FindMatches(ISuffixTree suffixTree, ISequence searchSequence, long lengthOfMUM);

        /// <summary>
        /// Finds all the matches of given sequence in suffix tree irrespective of the uniqueness in
        /// reference or query sequence
        /// </summary>
        /// <param name="suffixTree">Suffix Tree</param>
        /// <param name="searchSequence">Query searchSequence</param>
        /// <param name="lengthOfMUM">Mininum length of MUM</param>
        /// <returns>Matches found</returns>
        IList<MaxUniqueMatch> FindMaximumMatches(ISuffixTree suffixTree, ISequence searchSequence, long lengthOfMUM);
    }
}