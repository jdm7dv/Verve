// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;

namespace MBF.Algorithms.SuffixTree
{
    /// <summary>
    /// This is a static Factory class, that enables user to easily create a best suited
    /// suffix tree builder for the user.
    /// 1. If # of edges is within dictionary count limitation, use KurtzSuffixTreeBuilder.
    /// 2. If memory required of tree is within the physical RAM, use SimpleSuffixTree
    ///     a. Internally SimpleSuffixTree uses persistent suffix tree if memory required exceeds limitiation.
    /// </summary>
    public static class Factory
    {
        #region -- Constants --

        /// <summary>
        /// Maximum numbers of entries allowed in dictionary on 32-bit m/c
        /// </summary>
        private const int DictionaryLimit32Bit = 23997907;

        /// <summary>
        /// Maximum numbers of entries allowed in dictionary on 64-bit m/c
        /// </summary>
        private const int DictionaryLimit64Bit = 47995853;

        #endregion

        #region -- Public Method(s) --

        /// <summary>
        /// Create a most appropriate suffix tree builder based on the input parameters and
        /// environment variables and return.
        /// </summary>
        /// <param name="sequence">Input sequence</param>
        /// <returns>Suffix tree builder instance</returns>
        /// Cannot dispose "SimpleSuffixTreeBuilder" as it is returned by the method
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static ISuffixTreeBuilder CreateNew(ISequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            if (Environment.Is64BitProcess)
            {
                if ((2 * sequence.Count) <= DictionaryLimit64Bit)
                {
                    // If 2N (# of nodes) required is < Dictionary capacity limit on 32 bit
                    return new KurtzSuffixTreeBuilder();
                }
            }
            else
            {
                if ((2 * sequence.Count) <= DictionaryLimit32Bit)
                {
                    // If 2N (# of nodes) required is < Dictionary capacity limit on 32 bit
                    return new KurtzSuffixTreeBuilder();
                }
            }

            // This will written a in-memory simple suffix tree (or) disc based simple suffix tree
            return new SimpleSuffixTreeBuilder();
        }

        #endregion
    }
}