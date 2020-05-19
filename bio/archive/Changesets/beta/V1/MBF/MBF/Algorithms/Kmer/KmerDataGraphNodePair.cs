// Copyright (c) Microsoft Corporation. All rights reserved.

using MBF.Algorithms.Assembly.Graph;

namespace MBF.Algorithms.Kmer
{
    /// <summary>
    /// Holds reference to a pair of KmerData and De Bruijn Node.
    /// Used to improve parallelization.
    /// </summary>
    public class KmerDataGraphNodePair
    {
        /// <summary>
        /// Gets or sets Kmer Data
        /// </summary>
        public KmerData Kmer { get; set; }

        /// <summary>
        /// Gets or sets De Bruijn Node
        /// </summary>
        public DeBruijnNode Node { get; set; }

        /// <summary>
        /// Gets or sets the boolean indicating whether the kmer 
        /// represented here is same as first occurance of the kmer 
        /// in reads kmer or reverse complement has to be be stored. 
        /// For efficiency, the lexicographically lower one is stored.
        /// </summary>
        public bool KeyHasSameOrientation { get; set; }

        /// <summary>
        /// Initializes a new instance of the TupleClass class.
        /// </summary>
        /// <param name="kmerData">Kmer Data</param>
        /// <param name="orientation">This boolean indicates whether 
        /// the kmer represented here is same as first occurance of kmer in read</param>
        public KmerDataGraphNodePair(KmerData kmerData, bool orientation)
        {
            Kmer = kmerData;
            KeyHasSameOrientation = orientation;
            Node = null;
        }
    }
}
