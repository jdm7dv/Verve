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



using System;
using System.Collections.Generic;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// Represents a alignment object in terms of delta.
    /// Delta is an encoded representation of alignments between input sequences.
    /// It contains the start and end indices of alignment in reference and
    /// query sequence followed by error values and list of integera in 
    /// following lines. Each integer represent an insertion (+ve) in reference
    /// sequence and deletion (-ve) in reference sequence.
    /// This class represents such alignment with required properties and
    /// utility methods.
    /// </summary>
    public class DeltaAlignment
    {
        /// <summary>
        /// List of integers that pointing the insertion and deletion indices
        /// </summary>
        private IList<int> _deltas;

        /// <summary>
        /// Reference sequence
        /// </summary>
        private ISequence _referenceSequence;

        /// <summary>
        /// Query sequence
        /// </summary>
        private ISequence _querySequence;

        /// <summary>
        /// Initializes a new instance of the DeltaAlignment class
        /// </summary>
        /// <param name="referenceSequence">Reference Sequence</param>
        /// <param name="querySequence">Query Sequence</param>
        public DeltaAlignment(ISequence referenceSequence, ISequence querySequence)
        {
            _deltas = new List<int>();
            _referenceSequence = referenceSequence;
            _querySequence = querySequence;
        }

        /// <summary>
        /// Gets or sets the query sequence direction
        ///     FORWARD_CHAR or REVERSE_CHAR
        /// </summary>
        public string QueryDirection { get; set; }

        /// <summary>
        /// Gets or sets the start index of first sequence
        /// </summary>
        public int FirstSequenceStart { get; set; }

        /// <summary>
        /// Gets or sets the end index of first sequence
        /// </summary>
        public int FirstSequenceEnd { get; set; }

        /// <summary>
        /// Gets or sets the start index of second sequence
        /// </summary>
        public int SecondSequenceStart { get; set; }

        /// <summary>
        /// Gets or sets the end index of second sequence
        /// </summary>
        public int SecondSequenceEnd { get; set; }

        /// <summary>
        /// Gets or sets errors
        /// </summary>
        public int Errors { get; set; }

        /// <summary>
        /// Gets or sets similarity errors
        /// </summary>
        public int SimilarityErrors { get; set; }

        /// <summary>
        /// Gets or sets number of non alphabets encountered during alignment
        /// </summary>
        public int NonAlphas { get; set; }

        /// <summary>
        /// Gets or sets the value of delta reference position
        /// </summary>
        public int DeltaReferencePosition { get; set; }

        /// <summary>
        /// Gets list of integers that pointing the insertion and 
        /// deletion indices
        /// </summary>
        public IList<int> Deltas
        {
            get { return _deltas; }
        }

        /// <summary>
        /// Gets reference sequence
        /// </summary>
        public ISequence ReferenceSequence
        {
            get { return _referenceSequence; }
        }

        /// <summary>
        /// Gets query sequence
        /// </summary>
        public ISequence QuerySequence
        {
            get { return _querySequence; }
        }

        /// <summary>
        /// Create a new delta alignment
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequence">Query sequence</param>
        /// <param name="cluster">Cluster object</param>
        /// <param name="match">Match object</param>
        /// <returns>Newly created DeltaAlignment object</returns>
        internal static DeltaAlignment NewAlignment(
                ISequence referenceSequence,
                ISequence querySequence,
                Cluster cluster,
                MaxUniqueMatchExtension match)
        {
            DeltaAlignment deltaAlignment = new DeltaAlignment(referenceSequence, querySequence);
            deltaAlignment.FirstSequenceStart = match.FirstSequenceStart;
            deltaAlignment.SecondSequenceStart = match.SecondSequenceStart;
            deltaAlignment.FirstSequenceEnd = match.FirstSequenceStart
                    + match.Length
                    - 1;
            deltaAlignment.SecondSequenceEnd = match.SecondSequenceStart
                + match.Length
                - 1;

            deltaAlignment.QueryDirection = cluster.QueryDirection;

            return deltaAlignment;
        }

        /// <summary>
        /// Convert the delta alignment object to its sequence representation
        /// </summary>
        /// <returns>Reference sequence alignment at 0th index and
        /// Query sequence alignment at 1st index</returns>
        internal PairwiseAlignedSequence ConvertDeltaToSequences()
        {
            PairwiseAlignedSequence alignedSequence = new PairwiseAlignedSequence();
            Sequence referenceSequence = null;
            Sequence querySequence = null;
            int gap = 0;
            int length = 0;
            List<int> startOffsets = new List<int>(2);
            List<int> endOffsets = new List<int>(2);
            List<int> insertions = new List<int>(2);

            startOffsets.Add(FirstSequenceStart);
            startOffsets.Add(SecondSequenceStart);
            endOffsets.Add(FirstSequenceEnd);
            endOffsets.Add(SecondSequenceEnd);

            insertions.Add(0);
            insertions.Add(0);

            // Create the new sequence object with given start and end indices
            referenceSequence = new Sequence(ReferenceSequence.Alphabet);
            referenceSequence.IsReadOnly = false;
            length = FirstSequenceEnd - FirstSequenceStart + 1;
            referenceSequence.InsertRange(
                    0,
                    ReferenceSequence.Range(FirstSequenceStart, length).ToString());

            querySequence = new Sequence(QuerySequence.Alphabet);
            querySequence.IsReadOnly = false;
            length = SecondSequenceEnd - SecondSequenceStart + 1;
            querySequence.InsertRange(
                    0,
                    QuerySequence.Range(SecondSequenceStart, length).ToString());

            // Insert the Alignment character at delta postion
            // +ve delta: Insertion in reference sequence
            // -ve delta: Insertion in query sequence (deletion in reference sequence)
            foreach (int delta in Deltas)
            {
                gap += Math.Abs(delta);
                if (delta < 0)
                {
                    referenceSequence.Insert(gap - 1, DnaAlphabet.Instance.Gap.Symbol);
                    insertions[0]++;
                }
                else
                {
                    querySequence.Insert(gap - 1, DnaAlphabet.Instance.Gap.Symbol);
                    insertions[1]++;
                }
            }

            alignedSequence.FirstSequence = referenceSequence;
            alignedSequence.SecondSequence = querySequence;

            alignedSequence.Metadata["StartOffsets"] = startOffsets;
            alignedSequence.Metadata["EndOffsets"] = endOffsets;
            alignedSequence.Metadata["Insertions"] = insertions;

            return alignedSequence;
        }
    }
}
