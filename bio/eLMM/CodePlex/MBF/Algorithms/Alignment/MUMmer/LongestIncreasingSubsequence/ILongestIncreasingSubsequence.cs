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

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// This interface defines contract for classes implementing
    ///  Longest increasing subsequence.
    /// </summary>
    public interface ILongestIncreasingSubsequence
    {
        /// <summary>
        /// This method will run greedy version of 
        /// longest increasing subsequence algorithm on the list of Mum.        
        /// </summary>
        /// <param name="sortedMums">List of Sorted Mums</param>
        /// <returns>Returns the longest subsequence list of Mum.</returns>
        IList<MaxUniqueMatch> GetLongestSequence(IList<MaxUniqueMatch> sortedMums);
    }
}
