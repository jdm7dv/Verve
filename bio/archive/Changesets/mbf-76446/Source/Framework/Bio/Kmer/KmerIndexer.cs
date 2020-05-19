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

namespace Bio.Algorithms.Kmer
{
    /// <summary>
    /// Structure that maintains sequence index, count information 
    /// and orientation for k-mer.
    /// </summary>
    public class KmerIndexer
    {
        /// <summary>
        /// Index to retrieve source sequence.
        /// </summary>
        private long sequenceIndex;

        /// <summary>
        /// Positions of k-mer within this source sequence.
        /// </summary>
        private IList<long> positions;

        /// <summary>
        /// Initializes a new instance of the KmerIndexer class.
        /// </summary>
        /// <param name="sequenceIndex">Index of source sequence.</param>
        /// <param name="positions">List of k-mer positions.</param>
        public KmerIndexer(long sequenceIndex, IList<long> positions)
        {
            this.sequenceIndex = sequenceIndex;
            this.positions = positions;
        }

        /// <summary>
        /// Gets the starting position within sequence.
        /// </summary>
        public IList<long> Positions
        {
            get { return this.positions; }
        }

        /// <summary>
        /// Gets sequence index.
        /// </summary>
        public long SequenceIndex
        {
            get { return this.sequenceIndex; }
        }
    }
}