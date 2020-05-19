// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF.Algorithms.Kmer
{
    /// <summary>
    /// Structure that maintains sequence index, count information 
    /// and orientation for k-mer.
    /// </summary>
    public class KmerIndexer
    {
        /// <summary>
        /// Index to retreive source sequence
        /// </summary>
        private int _sequenceIndex;

        /// <summary>
        /// Positions of k-mer within this source sequence
        /// </summary>
        private IList<int> _positions;

        /// <summary>
        /// Initializes a new instance of the KmerIndexer class.
        /// </summary>
        /// <param name="sequenceIndex">Index of source sequence</param>
        /// <param name="positions">List of k-mer positions</param>
        public KmerIndexer(int sequenceIndex, IList<int> positions)
        {
            _sequenceIndex = sequenceIndex;
            _positions = positions;
        }

        /// <summary>
        /// Gets the starting position within sequence
        /// </summary>
        public IList<int> Positions
        {
            get { return _positions; }
        }

        /// <summary>
        /// Gets sequence index
        /// </summary>
        public int SequenceIndex
        {
            get { return _sequenceIndex; }
        }
    }
}