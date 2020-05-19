// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF.Algorithms.Alignment
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