// Copyright (c) Microsoft Corporation. All rights reserved.

namespace MBF.Algorithms.Kmer
{
    /// <summary>
    /// Holds sequence index and start position for kmer
    /// </summary>
    public class KmerData
    {
        /// <summary>
        /// Index of base sequence
        /// </summary>
        public int SequenceIndex { get; set; }

        /// <summary>
        /// Start Position of kmer
        /// </summary>
        public int KmerPosition { get; set; }

        /// <summary>
        /// Number of occurrences of kmer
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Number of occurrences of kmer's reverse complement
        /// </summary>
        public int CountRC { get; set; }

        /// <summary>
        /// Initializes a new instance of the class KmerData.
        /// </summary>
        /// <param name="sequenceIndex">Index of base sequence</param>
        /// <param name="kmerPosition">Start position of kmer</param>
        /// <param name="orientation">This boolean indicates whether 
        /// the kmer represented here is same as first occurance of kmer in read</param>
        public KmerData(int sequenceIndex, int kmerPosition, bool orientation)
        {
            SequenceIndex = sequenceIndex;
            KmerPosition = kmerPosition;
            if (orientation)
                Count++;
            else
                CountRC++;
        }

        /// <summary>
        /// Increment count information based on the orientation
        /// </summary>
        /// <param name="isNormalOrientation">This boolean indicates whether 
        /// the kmer represented here is same as first occurance of kmer in read</param>
        public void IncrementCount(bool isNormalOrientation)
        {
            if (isNormalOrientation)
                Count++;
            else
                CountRC++;
        }
    }
}
