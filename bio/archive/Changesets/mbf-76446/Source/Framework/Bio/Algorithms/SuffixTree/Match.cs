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
namespace Bio.Algorithms.SuffixTree
{
    /// <summary>
    /// Structure to hold the match information.
    /// </summary>
    public struct Match
    {
        /// <summary>
        /// Gets or sets the length of match.
        /// </summary>
        public long Length;

        /// <summary>
        /// Gets or sets the start index of this match in reference sequence.
        /// </summary>
        public long ReferenceSequenceOffset;

        /// <summary>
        /// Gets or sets start index of this match in query sequence.
        /// </summary>
        public long QuerySequenceOffset;
    }
}