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
using System.Globalization;
using System.Linq;
using MBF.Algorithms.SuffixTree;
using MBF.SimilarityMatrices;
using MBF.Util.Logging;

namespace MBF.Algorithms.Alignment
{
    /// <summary>
    /// NUCmer is a system for rapidly aligning entire genomes or very large DNA
    /// sequences. It allows alignment of multiple reference sequences to multiple query sequences.
    /// This is commonly used to identify the position and orientation of set of sequence
    /// contigs in relation to finished sequence.
    /// NUCmer is the base abstract class which defines the contract for classes 
    /// implementing NUCmer algorithm. Using Template Method Pattern, NUCmer defines 
    /// the skeleton of the NUCmer algorithm and deferring some steps to derived class. 
    /// </summary>
    public abstract class NUCmer : ISequenceAligner
    {
        #region -- Constants --

        /// <summary>
        /// Default minimum length of Matches to be searched in streaming process
        /// </summary>
        private const int DefaultLengthOfMUM = 20;

        /// <summary>
        /// Default gap open penalty for use in alignment algorithms
        /// </summary>
        private const int DefaultGapOpenCost = -13;

        /// <summary>
        /// Default gap extension penalty for use in alignment algorithms
        /// </summary>
        private const int DefaultGapExtensionCost = -8;

        /// <summary>
        /// Represents reference sequences
        /// </summary>
        private const string ReferenceSequence = "ReferenceSequence";

        /// <summary>
        /// Represents query sequences
        /// </summary>
        private const string QuerySequence = "QuerySequence";

        #endregion

        #region -- Member Variables --

        /// <summary>
        /// Holds the reference sequence.
        /// </summary>
        private ISequence _referenceSequence;

        /// <summary>
        /// Holds a reference Suffix tree.
        /// </summary>
        private ISuffixTree _suffixTree;

        /// <summary>
        /// List of maximal unique matches.
        /// </summary>
        private IList<MaxUniqueMatch> _mumList;

        /// <summary>
        /// List of clusters
        /// </summary>
        private IList<Cluster> _clusterList;

        #endregion -- Member Variables --

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the NUCmer class.
        /// </summary>
        protected NUCmer()
        {
            // User will typically choose their own parameters, these
            // defaults are reasonable for many cases.

            // Set the default Similarity Matrix
            SimilarityMatrix = new SimilarityMatrix(
                SimilarityMatrix.StandardSimilarityMatrix.DiagonalScoreMatrix);

            // Set the defaults
            GapOpenCost = DefaultGapOpenCost;
            GapExtensionCost = DefaultGapExtensionCost;
            LengthOfMUM = DefaultLengthOfMUM;

            // Set the ClusterBuilder properties to defaults
            FixedSeparation = ClusterBuilder.DefaultFixedSeparation;
            MaximumSeparation = ClusterBuilder.DefaultMaximumSeparation;
            MinimumScore = ClusterBuilder.DefaultMinimumScore;
            SeparationFactor = ClusterBuilder.DefaultSeparationFactor;
            BreakLength = NUCmerAligner.DefaultBreakLength;
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Gets the name of the Aligner. Intended to be filled in 
        /// by classes deriving from DynamicProgrammingPairwiseAligner class
        /// with the exact name of the Alignment algorithm.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the description of the Aligner. Intended to be filled in 
        /// by classes deriving from DynamicProgrammingPairwiseAligner class
        /// with the exact details of the Alignment algorithm.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets or sets minimum length of Match that can be considered as MUM.
        /// </summary>
        public long LengthOfMUM { get; set; }

        /// <summary>
        /// Gets or sets the id of reference sequence
        /// </summary>
        public abstract int ReferenceSequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets similarity matrix for use in alignment algorithms.
        /// </summary>
        public SimilarityMatrix SimilarityMatrix { get; set; }

        /// <summary> 
        /// Gets or sets gap open penalty for use in alignment algorithms. 
        /// For alignments using a linear gap penalty, this is the gap penalty.
        /// For alignments using an affine gap, this is the penalty to open a new gap.
        /// This is a negative number, for example GapOpenCost = -8, not +8.
        /// </summary>
        public int GapOpenCost { get; set; }

        /// <summary> 
        /// Gets or sets gap extension penalty for use in alignment algorithms. 
        /// Not used for alignments using a linear gap penalty.
        /// For alignments using an affine gap, this is the penalty to
        /// extend an existing gap.
        /// This is a negative number, for example GapExtensionCost = -2, not +2.
        /// </summary>
        public int GapExtensionCost { get; set; }

        /// <summary>
        /// Gets or sets the object that will be used to compute the alignment's consensus.
        /// </summary>
        public IConsensusResolver ConsensusResolver { get; set; }

        /// <summary>
        /// Gets or sets number of bases to be extended before stopping alignment
        /// </summary>
        public int BreakLength { get; set; }

        // Properties specific to ClusterBuilder

        /// <summary>
        /// Gets or sets maximum fixed diagonal difference
        /// </summary>
        public int FixedSeparation { get; set; }

        /// <summary>
        /// Gets or sets maximum separation between the adjacent matches in clusters
        /// </summary>
        public int MaximumSeparation { get; set; }

        /// <summary>
        /// Gets or sets minimum output score
        /// </summary>
        public int MinimumScore { get; set; }

        /// <summary>
        /// Gets or sets separation factor. Fraction equal to 
        /// (diagonal difference / match separation) where higher values
        /// increase the insertion or deletion (indel) tolerance
        /// </summary>
        public float SeparationFactor { get; set; }

        // Private properties

        /// <summary>
        /// Gets or sets a value indicating whether to run Align or AlignSimple
        /// </summary>
        protected bool IsAlign { get; set; }

        #endregion -- Properties --

        #region -- Main Execute Method --
        /// <summary>
        /// Align the list of input sequences using linear gap model.
        /// </summary>
        /// <param name="inputSequences">The sequences to align.</param>
        /// <returns>A list of sequence alignment object</returns>
        IList<ISequenceAlignment> ISequenceAligner.AlignSimple(IList<ISequence> inputSequences)
        {
            return AlignSimple(inputSequences).ToList().ConvertAll(SA => SA as ISequenceAlignment);
        }

        /// <summary>
        /// Align the list of input sequences using linear gap model.
        /// </summary>
        /// <param name="inputSequences">The sequences to align.</param>
        /// <returns>A list of sequence alignment object</returns>
        public IList<IPairwiseSequenceAlignment> AlignSimple(IList<ISequence> inputSequences)
        {
            IsAlign = false;
            return DetermineSequence(inputSequences);
        }

        /// <summary>
        /// Align the reference sequence and query sequences using linear gap model.
        /// </summary>
        /// <param name="referenceSequenceList">List of reference sequence</param>
        /// <param name="querySequenceList">List of query sequence</param>
        /// <returns>A list of sequence alignment object</returns>
        public IList<IPairwiseSequenceAlignment> AlignSimple(
               IList<ISequence> referenceSequenceList,
               IList<ISequence> querySequenceList)
        {
            IsAlign = false;
            return Alignment(referenceSequenceList, querySequenceList);
        }

        /// <summary>
        /// Align the set of input sequences using the affine gap model 
        /// (gap open and gap extension penalties)
        /// and returns the best alignment found.
        /// </summary>
        /// <param name="inputSequences">The sequences to align.</param>
        /// <returns>A list of sequence alignment.</returns>
        IList<ISequenceAlignment> ISequenceAligner.Align(IList<ISequence> inputSequences)
        {
            return Align(inputSequences).ToList().ConvertAll(SA => SA as ISequenceAlignment);
        }

        /// <summary>
        /// Align the set of input sequences using the affine gap model 
        /// (gap open and gap extension penalties)
        /// and returns the best alignment found.
        /// </summary>
        /// <param name="inputSequences">The sequences to align.</param>
        /// <returns>A list of sequence alignment.</returns>
        public IList<IPairwiseSequenceAlignment> Align(IList<ISequence> inputSequences)
        {
            IsAlign = true;
            return DetermineSequence(inputSequences);
        }

        /// <summary>
        ///  Align the reference sequence with query sequences using the affine gap model 
        ///  (gap open and gap extension penalties)
        /// and returns the best alignment found.
        /// </summary>
        /// <param name="referenceSequenceList">Reference sequence</param>
        /// <param name="querySequenceList">List of query sequence</param>
        /// <returns>A list of sequence alignment.</returns>
        public IList<IPairwiseSequenceAlignment> Align(
                IList<ISequence> referenceSequenceList,
                IList<ISequence> querySequenceList)
        {
            IsAlign = true;
            return Alignment(referenceSequenceList, querySequenceList);
        }

        #endregion -- Main Execute Method --

        #region -- Abstract Methods --

        /// <summary>
        /// Concatenate all the sequences into one sequence with special character.
        /// </summary>
        /// <param name="sequences">list of reference sequence</param>
        /// <returns>concatenated sequence</returns>
        protected abstract ISequence ConcatSequence(IList<ISequence> sequences);

        /// <summary>
        /// Build Suffix Tree using reference sequence
        /// </summary>
        /// <param name="referenceSequence">Reference sequence to build SuffixTree</param>
        /// <returns>Suffix Tree</returns>
        protected abstract ISuffixTree BuildSuffixTree(ISequence referenceSequence);

        /// <summary>
        /// Traverse the suffix tree using query sequence and return list of matches
        /// </summary>
        /// <param name="suffixTree">Suffix tree</param>
        /// <param name="referenceSequence">Reference seqeunce</param>
        /// <param name="sequence">Query sequence</param>
        /// <param name="lengthOfMUM">Minimum length of MUM</param>
        /// <returns>List of matches</returns>
        protected abstract IList<MaxUniqueMatch> Streaming(
                ISuffixTree suffixTree,
                ISequence referenceSequence,
                ISequence sequence,
                long lengthOfMUM);

        /// <summary>
        /// Get the list of clusters from list of MUMs
        /// </summary>
        /// <param name="mumList">List of maximum unique matches</param>
        /// <returns>List of clusters</returns>
        protected abstract IList<Cluster> GetClusters(IList<MaxUniqueMatch> mumList);

        /// <summary>
        /// Process clusters and get delta (compact representation of alignment)
        /// </summary>
        /// <param name="referenceSequenceList">List of reference sequences</param>
        /// <param name="clusters">List of clusters</param>
        /// <returns>List of delta alignments</returns>
        protected abstract IList<DeltaAlignment> ProcessCluster(
            IList<ISequence> referenceSequenceList,
            IList<Cluster> clusters);

        /// <summary>
        /// Convert list of Delta alignments to Sequence alignment
        /// </summary>
        /// <param name="alignments">List of delta alignment</param>
        /// <returns>A list of sequence alignment.</returns>
        protected abstract IList<PairwiseAlignedSequence> ConvertDeltaToAlignment(
                IList<DeltaAlignment> alignments);

        /// <summary>
        /// Calculate the score of alignment
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequence">Query sequence</param>
        /// <returns>Score of the alignment</returns>
        protected abstract int CalculateScore(
                ISequence referenceSequence,
                ISequence querySequence);

        /// <summary>
        /// Analyze the given seqquences and store a consensus into its Consensus property.
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequence">Query sequence</param>
        /// <returns>Consensus of sequences</returns>
        protected abstract ISequence MakeConsensus(
                ISequence referenceSequence,
                ISequence querySequence);

        #endregion -- Abstract Methods --

        #region -- Private Methods --

        /// <summary>
        /// Determine the reference sequence and query sequences from list of input sequences.
        /// </summary>
        /// <param name="inputSequences">Sequences needs to be aligned</param>
        /// <returns>A list of sequence alignment</returns>
        private IList<IPairwiseSequenceAlignment> DetermineSequence(IList<ISequence> inputSequences)
        {
            IList<IPairwiseSequenceAlignment> result = null;

            if (null != inputSequences)
            {
                if (inputSequences.Count >= 2)
                {
                    ISequence referenceSequence = inputSequences[ReferenceSequenceNumber];
                    IList<ISequence> referenceSequences = new List<ISequence>();
                    referenceSequences.Add(referenceSequence);

                    // find out how to get refs.
                    result = Alignment(referenceSequences, inputSequences);
                }
                else
                {
                    throw new ArgumentException(Properties.Resource.MinimumTwoSequences);
                }
            }

            return result;
        }

        /// <summary>
        /// This method is considered as main execute method which defines the
        /// step by step algorithm. Drived class flows the defined flow by this
        /// method.
        /// </summary>
        /// <param name="referenceSequenceList">reference sequence</param>
        /// <param name="querySequenceList">list of input sequences</param>
        /// <returns>A list of sequence alignment</returns>
        private IList<IPairwiseSequenceAlignment> Alignment(
                IList<ISequence> referenceSequenceList,
                IList<ISequence> querySequenceList)
        {
            // Initializations
            if (referenceSequenceList.Count > 0)
            {
                if (ConsensusResolver == null)
                {
                    ConsensusResolver = new SimpleConsensusResolver(referenceSequenceList[0].Alphabet);
                }
                else
                {
                    ConsensusResolver.SequenceAlphabet = referenceSequenceList[0].Alphabet;
                }
            }

            IList<IPairwiseSequenceAlignment> results = new List<IPairwiseSequenceAlignment>();
            IPairwiseSequenceAlignment sequenceAlignment = null;
            IList<DeltaAlignment> deltaAlignments = null;
            IList<PairwiseAlignedSequence> alignments = null;
            ISequence referenceSequence = null;

            // Validate the input
            Validate(referenceSequenceList, querySequenceList);

            // Step:1 concat all the sequences into one sequence
            if (referenceSequenceList.Count > 1)
            {
                referenceSequence = ConcatSequence(referenceSequenceList);
            }
            else
            {
                referenceSequence = referenceSequenceList[0];
            }

            // Getting refernce sequence
            _referenceSequence = referenceSequence;

            // Step2 : building suffix trees using reference sequence
            _suffixTree = BuildSuffixTree(_referenceSequence);

            // On each query sequence aligned with reference sequence
            foreach (ISequence sequence in querySequenceList)
            {
                if (sequence.Equals(referenceSequence))
                {
                    continue;
                }

                sequenceAlignment = new PairwiseSequenceAlignment(referenceSequence, sequence);

                // Step3 : streaming process is performed with the query sequence
                _mumList = Streaming(_suffixTree, referenceSequence, sequence, LengthOfMUM);

                if (_mumList.Count > 0)
                {
                    // Step 5 : Get the list of Clusters
                    _clusterList = GetClusters(_mumList);

                    // Step 7: Process Clusters and get delta
                    deltaAlignments = ProcessCluster(
                            referenceSequenceList,
                            _clusterList);

                    // Step 8: Convert delta alignments to sequence alignments
                    alignments = ConvertDeltaToAlignment(deltaAlignments);

                    if (alignments.Count > 0)
                    {
                        foreach (PairwiseAlignedSequence align in alignments)
                        {
                            // Calculate the score of alignment
                            align.Score = CalculateScore(
                                    align.FirstSequence,
                                    align.SecondSequence);

                            // Make Consensus
                            align.Consensus = MakeConsensus(
                                    align.FirstSequence,
                                    align.SecondSequence);

                            sequenceAlignment.PairwiseAlignedSequences.Add(align);
                        }
                    }
                }

                results.Add(sequenceAlignment);
            }

            return results;
        }

        /// <summary>
        /// Validate the inputs
        /// </summary>
        /// <param name="referenceSequenceList">list of reference sequence</param>
        /// <param name="querySequenceList">list of input sequences</param>
        /// <returns>Are inputs valid</returns>
        private bool Validate(
                IList<ISequence> referenceSequenceList,
                IList<ISequence> querySequenceList)
        {
            IAlphabet alphabetSet;

            if (null == referenceSequenceList)
            {
                string message = Properties.Resource.ReferenceListCannotBeNull;
                Trace.Report(message);
                throw new ArgumentNullException("referenceSequenceList");
            }

            if (null == querySequenceList)
            {
                string message = Properties.Resource.QueryListCannotBeNull;
                Trace.Report(message);
                throw new ArgumentNullException("querySequenceList");
            }

            alphabetSet = referenceSequenceList[0].Alphabet;

            if (alphabetSet != Alphabets.DNA
                && alphabetSet != Alphabets.RNA)
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resource.OnlyDNAOrRNAInput,
                    "NUCmer");
                Trace.Report(message);
                throw new ArgumentException(message);
            }

            if (!SimilarityMatrix.Name.Equals(SimilarityMatrix.StandardSimilarityMatrix.DiagonalScoreMatrix.ToString()))
            {
                string message = Properties.Resource.FirstInputSequenceMismatchSimilarityMatrix;
                throw new ArgumentException(message);
            }

            ValidateSequenceList(referenceSequenceList, alphabetSet, ReferenceSequence);

            ValidateSequenceList(querySequenceList, alphabetSet, QuerySequence);

            if (1 > LengthOfMUM)
            {
                string message = Properties.Resource.MUMLengthTooSmall;
                Trace.Report(message);
                throw new ArgumentException(message);
            }

            return true;
        }

        /// <summary>
        /// Validate the list of sequences
        /// </summary>
        /// <param name="sequenceList">List of sequence</param>
        /// <param name="alphabetSet">Alphabet set</param>
        /// <param name="sequenceType">Type of sequence</param>
        private void ValidateSequenceList(
                IList<ISequence> sequenceList,
                IAlphabet alphabetSet,
                string sequenceType)
        {
            bool isValidLength = false;

            foreach (ISequence sequence in sequenceList)
            {
                if (null == sequence)
                {
                    string message = sequenceType == ReferenceSequence
                        ? Properties.Resource.ReferenceSequenceCannotBeNull
                        : Properties.Resource.QuerySequenceCannotBeNull;
                    Trace.Report(message);
                    throw new ArgumentException(message);
                }

                if (sequence.Alphabet != alphabetSet)
                {
                    string message = Properties.Resource.InputAlphabetsMismatch;
                    Trace.Report(message);
                    throw new ArgumentException(message);
                }

                if (sequence.Count > LengthOfMUM)
                {
                    isValidLength = true;
                }
            }

            if (!isValidLength)
            {
                string message = String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.InputSequenceMustBeGreaterThanMUM,
                        LengthOfMUM);
                Trace.Report(message);
                throw new ArgumentException(message);
            }
        }

        #endregion -- Private Methods --
    }
}