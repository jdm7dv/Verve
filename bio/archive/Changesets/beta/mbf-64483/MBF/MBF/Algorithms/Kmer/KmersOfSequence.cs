// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;

namespace MBF.Algorithms.Kmer
{
    /// <summary>
    /// Contains base sequence, and information regarding associated k-mers
    /// </summary>
    public class KmersOfSequence
    {
        #region Fields
        /// <summary>
        /// Holds an instance of sequence used for building k-mer
        /// </summary>
        private ISequence _baseSequence;

        /// <summary>
        /// Length of k-mer
        /// </summary>
        private int _length;

        /// <summary>
        /// Positions and count of k-mers occurring in base sequence
        /// </summary>
        private HashSet<KmerPositions> _kmers;
        #endregion

        #region Constructors, Properties
        /// <summary>
        /// Initializes a new instance of the KmersOfSequence class.
        /// Takes k-mer sequence and occurring position.
        /// </summary>
        /// <param name="sequence">Source sequence</param>
        /// <param name="kmerLength">Length of k-mer</param>
        /// <param name="kmers">Set of associated k-mers</param>
        public KmersOfSequence(ISequence sequence, int kmerLength, HashSet<KmerPositions> kmers)
        {
            _baseSequence = sequence;
            _length = kmerLength;
            _kmers = kmers;
        }

        /// <summary>
        /// Initializes a new instance of the KmersOfSequence class.
        /// Takes k-mer sequence and k-mer length.
        /// </summary>
        /// <param name="sequence">Source sequence</param>
        /// <param name="kmerLength">Length of k-mer</param>
        public KmersOfSequence(ISequence sequence, int kmerLength)
        {
            _baseSequence = sequence;
            _length = kmerLength;
            _kmers = null;
        }

        /// <summary>
        /// Gets the length of associated k-mers
        /// </summary>
        public int Length
        {
            get { return _length; }
        }

        /// <summary>
        /// Gets the set of associated Kmers
        /// </summary>
        public HashSet<KmerPositions> Kmers
        {
            get { return _kmers; }
        }

        /// <summary>
        /// Gets the base sequence
        /// </summary>
        public ISequence BaseSequence
        {
            get { return _baseSequence; }
        }
        #endregion

        /// <summary>
        /// Returns the associated k-mers as a list of k-mer sequences
        /// </summary>
        /// <returns>List of k-mer sequences</returns>
        public IList<ISequence> KmersToSequences()
        {
            return new List<ISequence>(_kmers.Select(k => _baseSequence.Range(k.Positions.First(), _length)));
        }

        /// <summary>
        /// Builds the sequence corresponding to input kmer, 
        /// using base sequence
        /// </summary>
        /// <param name="kmer">Input k-mer</param>
        /// <returns>Sequence corresponding to input k-mer</returns>
        public ISequence KmerToSequence(KmerPositions kmer)
        {
            if (kmer == null)
            {
                throw new ArgumentNullException("kmer");
            }

            return _baseSequence.Range(kmer.Positions.First(), _length);
        }

        #region Nested Structure
        /// <summary>
        /// Contains information regarding k-mer
        /// position in the base sequence
        /// </summary>
        public class KmerPositions
        {
            /// <summary>
            /// List of positions
            /// </summary>
            List<int> _kmerPositions = new List<int>();

            /// <summary>
            /// Initializes a new instance of the KmerPositions class.
            /// </summary>
            /// <param name="positions">List of positions</param>
            public KmerPositions(IList<int> positions)
            {
                _kmerPositions.AddRange(positions);
            }

            /// <summary>
            /// Gets the list of positions for the k-mer
            /// </summary>
            public IList<int> Positions
            {
                get { return _kmerPositions; }
            }

            /// <summary>
            /// Gets the number of positions
            /// </summary>
            public int Count
            {
                get { return _kmerPositions.Count; }
            }
        }
        #endregion
    }
}
