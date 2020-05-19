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
using System.Globalization;
using System.Linq;
using System.Text;
using Bio.Algorithms.SuffixTree;
using Bio.SimilarityMatrices;
using Bio.Util.Logging;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// MUMmer is a system for rapidly aligning entire genomes or very large protein
    /// sequences. It is a pair wise sequence algorithm. The algorithm assumes the 
    /// sequences are closely related, and using this assumption can quickly compare
    /// sequences that are millions of nucleotides in length. The algorithm is 
    /// designed to perform high resolution comparison of genome-length sequences. 
    /// MUMmer is the base abstract class defines contract for classes implementing
    /// MUMmer algorithm. Using Template Method Pattern, MUMmer defines the skeleton
    /// of the MUMmer algorithm and deferring some steps to drived class. 
    /// </summary>
    public abstract class MUMmer : ISequenceAligner
    {
        #region -- Member Variables --

        /// <summary>
        /// Alignment Char
        /// </summary>
        private const char AlignmentChar = '-';

        /// <summary>
        /// Holds the reference sequence.
        /// </summary>
        private ISequence _referenceSequence;

        /// <summary>
        /// Holds a reference Suffix tree.
        /// </summary>
        private SequenceSuffixTree _suffixTree;

        /// <summary>
        /// List of mum.
        /// </summary>
        private IList<MaxUniqueMatch> _mumList;

        /// <summary>
        /// List of sorted mums.
        /// </summary>
        private IList<MaxUniqueMatch> _sortedMumList;

        /// <summary>
        /// List of final mums.
        /// </summary>
        private IList<MaxUniqueMatch> _finalMumList;

        /// <summary>
        /// Stores list of MUMs found for each query sequence
        /// </summary>
        private IDictionary<ISequence, IList<MaxUniqueMatch>> _mums;

        /// <summary>
        /// Stores list of MUMs after applying Longest Increasing Subsequence
        /// algorithm to order and merge MUMs, for each query sequence.
        /// </summary>
        private IDictionary<ISequence, IList<MaxUniqueMatch>> _finalMums;

        /// <summary>
        /// Boolean indicating whether the MUMs generated 
        /// during alignment are to be stored for later access.
        /// </summary>
        private bool _storeMUMs = false;

        #endregion -- Member Variables --

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the MUMmer class.
        /// Constructor for all the pairwise aligner 
        /// (NeedlemanWunsch, SmithWaterman, Overlap).
        /// Sets default similarity matrix and gap penalty.
        /// Users will typically reset these using parameters 
        /// specific to their particular sequences
        /// and needs.
        /// </summary>
        protected MUMmer()
        {
            // Set default similarity matrix and gap penalty.
            // User will typically choose their own parameters, these
            // defaults are reasonable for many cases.

            // Default is set to 20
            LengthOfMUM = 20;

            SimilarityMatrix = null;

            GapOpenCost = -13; // 5, -4 diagonal matrix for Dna

            // default affine gap is -1
            GapExtensionCost = -8;

            // Set the default alignment algorithm to Smith Waterman
            PairWiseAlgorithm = new NeedlemanWunschAligner();
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Gets or sets the length of MUM
        /// </summary>
        public long LengthOfMUM { get; set; }

        /// <summary>
        /// Gets or sets similarity matrix for use in alignment algorithms.
        /// </summary>
        public SimilarityMatrix SimilarityMatrix { get; set; }

        /// <summary> 
        /// Gets or sets gap open penalty for use in alignment algorithms. 
        /// For alignments using a single gap penalty, this is the gap penalty.
        /// For alignments using an affine gap, this is the penalty to open a new gap.
        /// This is a negative number, for example GapOpenCost = -8, not +8.
        /// </summary>
        public int GapOpenCost { get; set; }

        /// <summary> 
        /// Gets or sets gap extension penalty for use in alignment algorithms. 
        /// Not used for alignments using a single gap penalty.
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
        /// Gets or sets the pair wise aligner which will be executed 
        /// by end of Mummer
        /// </summary>
        public IPairwiseSequenceAligner PairWiseAlgorithm { get; set; }

        /// <summary>
        /// Gets the name of the Aligner. Intended to be filled in 
        /// by classes deriving from DynamicProgrammingPairwiseAligner class
        /// with the exact name of the Alignment algorithm.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets or sets the id of reference sequence
        /// </summary>
        public abstract int ReferenceSequenceNumber { get; set; }

        /// <summary>
        /// Gets the description of the Aligner. Intended to be filled in 
        /// by classes deriving from DynamicProgrammingPairwiseAligner class
        /// with the exact details of the Alignment algorithm.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets or sets the boolean value indicating 
        /// whether MUMs generated are to be stored or not.
        /// Set to false by default.
        /// Note: Storing MUMs incur memory overhead.
        /// </summary>
        public bool StoreMUMs
        {
            get { return _storeMUMs; }
            set { _storeMUMs = value; }
        }

        /// <summary>
        /// Gets the list of MUMs found for each query sequence
        /// </summary>
        public IDictionary<ISequence, IList<MaxUniqueMatch>> MUMs
        {
            get { return _mums; }
        }

        /// <summary>
        /// Gets the list of MUMs after applying Longest Increasing Subsequence
        /// algorithm to order and merge MUMs, for each query sequence.
        /// </summary>
        public IDictionary<ISequence, IList<MaxUniqueMatch>> FinalMUMs
        {
            get { return _finalMums; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to run Align or AlignSimple
        /// </summary>
        private bool UseGapExtensionCost { get; set; }

        #endregion -- Properties --

        #region -- Main Execute Method --

        /// <summary>
        /// Align the list of input sequences using simple gap model.
        /// </summary>
        /// <param name="inputSequences">The sequences to align.</param>
        /// <returns>A list of sequence alignments.</returns>
        IList<ISequenceAlignment> ISequenceAligner.AlignSimple(IList<ISequence> inputSequences)
        {
            return AlignSimple(inputSequences).ToList().ConvertAll(SA => SA as ISequenceAlignment);
        }

        /// <summary>
        /// Align the list of input sequences using simple gap model.
        /// </summary>
        /// <param name="inputSequences">The sequences to align.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> AlignSimple(IList<ISequence> inputSequences)
        {
            UseGapExtensionCost = false;
            return DetermineSequence(inputSequences);
        }

        /// <summary>
        /// Align the reference sequence and query sequences using simple gap model.
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequenceList">List of query sequence</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> AlignSimple(
               ISequence referenceSequence,
               IList<ISequence> querySequenceList)
        {
            UseGapExtensionCost = false;
            return Alignment(referenceSequence, querySequenceList);
        }

        /// <summary>
        /// Align aligns the set of input sequences using the afine gap model 
        /// (gap open and gap extension penalties)
        /// and returns the best alignment found.
        /// </summary>
        /// <param name="inputSequences">The sequences to align.</param>
        /// <returns>A list of sequence alignments.</returns>
        IList<ISequenceAlignment> ISequenceAligner.Align(IList<ISequence> inputSequences)
        {
            return Align(inputSequences).ToList().ConvertAll(SA => SA as ISequenceAlignment);
        }

        /// <summary>
        /// Align aligns the set of input sequences using the afine gap model 
        /// (gap open and gap extension penalties)
        /// and returns the best alignment found.
        /// </summary>
        /// <param name="inputSequences">The sequences to align.</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> Align(IList<ISequence> inputSequences)
        {
            UseGapExtensionCost = true;
            return DetermineSequence(inputSequences);
        }

        /// <summary>
        ///  Align aligns the reference sequence with querly sequences using the affine gap model 
        ///  (gap open and gap extension penalties)
        /// and returns the best alignment found.
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequenceList">List of query sequence</param>
        /// <returns>A list of sequence alignments.</returns>
        public IList<IPairwiseSequenceAlignment> Align(
                ISequence referenceSequence,
                IList<ISequence> querySequenceList)
        {
            UseGapExtensionCost = true;
            return Alignment(referenceSequence, querySequenceList);
        }

        #endregion -- Main Execute Method --

        #region -- Abstract Methods --

        /// <summary>
        /// Generates list of MUMs for each query sequence.
        /// The MUMs are not sorted or processed using 
        /// Longest Increasing Subsequence (LIS).
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequenceList">List of query sequences</param>
        /// <returns>List of MUMs for each query sequence</returns>
        public abstract IDictionary<ISequence, IList<MaxUniqueMatch>> GetMUMs(
            ISequence referenceSequence,
            IList<ISequence> querySequenceList);

        /// <summary>
        /// Generates list of MUMs for each query sequence.
        /// If 'performLIS' is true, MUMs are sorted and processed 
        /// using Longest Increasing Subsequence (LIS). If 'performLIS' 
        /// is false, MUMs are returned immediately after streaming.
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequenceList">List of query sequences</param>
        /// <param name="performLIS">Boolean indicating whether Longest Increasing 
        /// Subsequence (LIS) modules is run on MUMs before returning</param>
        /// <returns>List of MUMs for each query sequence</returns>
        public abstract IDictionary<ISequence, IList<MaxUniqueMatch>> GetMUMs(
            ISequence referenceSequence,
            IList<ISequence> querySequenceList,
            bool performLIS);

        /// <summary>
        /// Build Suffix Tree using reference sequence
        /// </summary>
        /// <param name="referenceSequence">Reference sequence to build SuffixTree</param>
        /// <returns>Suffix Tree</returns>
        protected abstract SequenceSuffixTree BuildSuffixTree(ISequence referenceSequence);

        /// <summary>
        /// Traverse the suffix tree using query sequence and return list of MUMs
        /// </summary>
        /// <param name="suffixTree">Suffix tree</param>
        /// <param name="sequence">Query sequence</param>
        /// <param name="lengthOfMUM">Minimum length of MUM</param>
        /// <returns>List of MUMs</returns>
        protected abstract IList<MaxUniqueMatch> Streaming(
                SequenceSuffixTree suffixTree,
                ISequence sequence,
                long lengthOfMUM);

        /// <summary>
        /// Sort the MUM list in increasing order of its position in reference sequence
        /// </summary>
        /// <param name="mumList">List of MUMs</param>
        /// <returns>Sorted list of MUMs</returns>
        protected abstract IList<MaxUniqueMatch> SortMum(IList<MaxUniqueMatch> mumList);

        /// <summary>
        /// Get the MUMs in the order of Longest Increasing Subsequence using position 
        /// in query sequence
        /// </summary>
        /// <param name="sortedMumList">Sorted list of MUMs</param>
        /// <returns>MUMs in longest increasing subsequence order</returns>
        protected abstract IList<MaxUniqueMatch> CollectLongestIncreasingSubsequence(
                IList<MaxUniqueMatch> sortedMumList);

        #endregion -- Abstract Methods --

        #region -- Private Methods --

        /// <summary>
        /// Create a default gap sequence of given length, pad the symbol - in sequence
        /// </summary>
        /// <param name="length">Length of gap</param>
        /// <returns>hyphen padded sequence</returns>
        private static string CreateDefaultGap(int length)
        {
            StringBuilder sequenceBuilder = new StringBuilder();
            sequenceBuilder.Append(AlignmentChar, length);
            return sequenceBuilder.ToString();
        }

        /// <summary>
        /// Determine the reference sequence and query sequences from list of input sequences.
        /// </summary>
        /// <param name="inputSequences">Sequences needs to be aligned</param>
        /// <returns>A sequence alignment object</returns>
        private IList<IPairwiseSequenceAlignment> DetermineSequence(IList<ISequence> inputSequences)
        {
            IList<IPairwiseSequenceAlignment> result = null;

            if (null != inputSequences)
            {
                if (inputSequences.Count >= 2)
                {
                    ISequence referenceSequence = inputSequences[ReferenceSequenceNumber];
                    result = Alignment(referenceSequence, inputSequences);
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
        /// <param name="referenceSequence">reference sequence</param>
        /// <param name="querySequenceList">list of input sequences</param>
        /// <returns>A list of sequence alignments</returns>
        private IList<IPairwiseSequenceAlignment> Alignment(
                ISequence referenceSequence,
                IList<ISequence> querySequenceList)
        {
            // Initializations
            if (ConsensusResolver == null)
            {
                ConsensusResolver = new SimpleConsensusResolver(referenceSequence.Alphabet);
            }
            else
            {
                ConsensusResolver.SequenceAlphabet = referenceSequence.Alphabet;
            }

            if (StoreMUMs)
            {
                return AlignmentWithAccumulatedMUMs(referenceSequence, querySequenceList);
            }
            else
            {
                return AlignmentWithoutAccumulatedMUMs(referenceSequence, querySequenceList);
            }
        }

        /// <summary>
        /// This method is considered as main execute method which defines the
        /// step by step algorithm. Drived class flows the defined flow by this
        /// method. Does not store MUMs, processes MUMs and gaps to find 
        /// alignment directly. 
        /// </summary>
        /// <param name="referenceSequence">reference sequence</param>
        /// <param name="querySequenceList">list of input sequences</param>
        /// <returns>A list of sequence alignments</returns>
        private IList<IPairwiseSequenceAlignment> AlignmentWithoutAccumulatedMUMs(
                ISequence referenceSequence,
                IList<ISequence> querySequenceList)
        {
            IList<IPairwiseSequenceAlignment> results = new List<IPairwiseSequenceAlignment>();
            IPairwiseSequenceAlignment alignment = null;

            if (Validate(referenceSequence, querySequenceList))
            {
                // Safety check for public methods to ensure that null 
                // inputs are handled.
                if (referenceSequence == null || querySequenceList == null)
                {
                    return null;
                }

                // Getting refernce sequence
                _referenceSequence = referenceSequence;

                // Step1 : building suffix trees using reference sequence
                _suffixTree = BuildSuffixTree(_referenceSequence);

                // On each query sequence aligned with reference sequence
                foreach (ISequence sequence in querySequenceList)
                {
                    if (sequence.Equals(referenceSequence))
                    {
                        continue;
                    }

                    alignment = new PairwiseSequenceAlignment(referenceSequence, sequence);

                    // Step2 : streaming process is performed with the query sequence
                    _mumList = Streaming(_suffixTree, sequence, LengthOfMUM);

                    // Step3(a) : sorted mum list based on reference sequence
                    _sortedMumList = SortMum(_mumList);

                    if (_sortedMumList.Count > 0)
                    {
                        // Step3(b) : LIS using greedy cover algorithm
                        _finalMumList = CollectLongestIncreasingSubsequence(_sortedMumList);

                        if (_finalMumList.Count > 0)
                        {
                            // Step 4 : get all the gaps in each sequence and call 
                            // pairwise alignment
                            alignment.PairwiseAlignedSequences.Add(ProcessGaps(referenceSequence, sequence));
                        }

                        results.Add(alignment);
                    }
                    else
                    {
                        IList<IPairwiseSequenceAlignment> sequenceAlignment = RunPairWise(
                                referenceSequence,
                                sequence);

                        foreach (IPairwiseSequenceAlignment pairwiseAlignment in sequenceAlignment)
                        {
                            results.Add(pairwiseAlignment);
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// This method is considered as main execute method which defines the
        /// step by step algorithm. Drived class flows the defined flow by this
        /// method. Store generated MUMs in properties MUMs, SortedMUMs.
        /// Alignment first finds MUMs for all the query sequence, and then 
        /// runs pairwise algorithm on gaps to produce alignments.
        /// </summary>
        /// <param name="referenceSequence">reference sequence</param>
        /// <param name="querySequenceList">list of input sequences</param>
        /// <returns>A list of sequence alignments</returns>
        private IList<IPairwiseSequenceAlignment> AlignmentWithAccumulatedMUMs(
                ISequence referenceSequence,
                IList<ISequence> querySequenceList)
        {
            // Get MUMs
            IDictionary<ISequence, IList<MaxUniqueMatch>> queryMums = new Dictionary<ISequence, IList<MaxUniqueMatch>>();
            _mums = new Dictionary<ISequence, IList<MaxUniqueMatch>>();
            _finalMums = new Dictionary<ISequence, IList<MaxUniqueMatch>>();

            if (Validate(referenceSequence, querySequenceList))
            {
                IList<MaxUniqueMatch> mumList;

                // Step1 : building suffix trees using reference sequence
                SequenceSuffixTree suffixTree = BuildSuffixTree(referenceSequence);

                // On each query sequence aligned with reference sequence
                foreach (ISequence sequence in querySequenceList)
                {
                    if (sequence.Equals(referenceSequence))
                    {
                        continue;
                    }

                    // Step2 : streaming process is performed with the query sequence
                    mumList = Streaming(suffixTree, sequence, LengthOfMUM);
                    _mums.Add(sequence, mumList);

                    // Step3(a) : sorted mum list based on reference sequence
                    mumList = SortMum(mumList);

                    if (mumList.Count > 0)
                    {
                        // Step3(b) : LIS using greedy cover algorithm
                        mumList = CollectLongestIncreasingSubsequence(mumList);
                    }
                    else
                    {
                        mumList = null;
                    }

                    _finalMums.Add(sequence, mumList);
                }
            }

            IList<IPairwiseSequenceAlignment> results = new List<IPairwiseSequenceAlignment>();
            IPairwiseSequenceAlignment alignment = null;

            if (MUMs != null && FinalMUMs != null)
            {
                // Getting refernce sequence
                _referenceSequence = referenceSequence;

                // On each query sequence aligned with reference sequence
                foreach (var finalMum in FinalMUMs)
                {
                    var sequence = finalMum.Key;
                    _mumList = MUMs[sequence];
                    _finalMumList = finalMum.Value;

                    alignment = new PairwiseSequenceAlignment(referenceSequence, sequence);

                    if (_mumList.Count > 0)
                    {
                        if (_finalMumList.Count > 0)
                        {
                            // Step 4 : get all the gaps in each sequence and call 
                            // pairwise alignment
                            alignment.PairwiseAlignedSequences.Add(ProcessGaps(referenceSequence, sequence));
                        }

                        results.Add(alignment);
                    }
                    else
                    {
                        IList<IPairwiseSequenceAlignment> sequenceAlignment = RunPairWise(
                                referenceSequence,
                                sequence);

                        foreach (IPairwiseSequenceAlignment pairwiseAlignment in sequenceAlignment)
                        {
                            results.Add(pairwiseAlignment);
                        }
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Validate the inputs
        /// </summary>
        /// <param name="referenceSequence">reference sequence</param>
        /// <param name="querySequenceList">list of input sequences</param>
        /// <returns>Are inputs valid</returns>
        protected bool Validate(
                ISequence referenceSequence,
                IList<ISequence> querySequenceList)
        {
            bool isValidLength = false;

            if (null == referenceSequence)
            {
                string message = Properties.Resource.ReferenceSequenceCannotBeNull;
                Trace.Report(message);
                throw new ArgumentNullException("referenceSequence");
            }

            if (null == querySequenceList)
            {
                string message = Properties.Resource.QueryListCannotBeNull;
                Trace.Report(message);
                throw new ArgumentNullException("querySequenceList");
            }

            if ((referenceSequence.Alphabet != Alphabets.DNA) && (referenceSequence.Alphabet != Alphabets.RNA))
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resource.OnlyDNAOrRNAInput,
                    "MUMmer");
                Trace.Report(message);
                throw new ArgumentException(message, "referenceSequence");
            }

            // setting default similarity matrix based on DNA or RNA
            if (SimilarityMatrix == null)
            {
                if (referenceSequence.Alphabet == Alphabets.RNA)
                {
                    SimilarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousRna);
                }
                else if (referenceSequence.Alphabet == Alphabets.DNA)
                {
                    SimilarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
                }
            }

            if (!SimilarityMatrix.ValidateSequence(referenceSequence))
            {
                string message = Properties.Resource.FirstInputSequenceMismatchSimilarityMatrix;
                Trace.Report(message);
                throw new ArgumentException(message, "referenceSequence");
            }

            if (referenceSequence.Count < LengthOfMUM)
            {
                string message = String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.InputSequenceMustBeGreaterThanMUM,
                        LengthOfMUM);
                Trace.Report(message);
                throw new ArgumentException(message, "referenceSequence");
            }

            foreach (ISequence querySequence in querySequenceList)
            {
                if (null == querySequence)
                {
                    string message = Properties.Resource.QuerySequenceCannotBeNull;
                    Trace.Report(message);
                    throw new ArgumentNullException("querySequenceList", message);
                }

                if (referenceSequence.Alphabet != querySequence.Alphabet)
                {
                    string message = Properties.Resource.InputAlphabetsMismatch;
                    Trace.Report(message);
                    throw new ArgumentException(message);
                }

                if (!SimilarityMatrix.ValidateSequence(querySequence))
                {
                    string message = Properties.Resource.SecondInputSequenceMismatchSimilarityMatrix;
                    Trace.Report(message);
                    throw new ArgumentException(message, "querySequenceList");
                }

                if (querySequence.Count > LengthOfMUM)
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
                throw new ArgumentException(message, "querySequenceList");
            }

            if (1 > LengthOfMUM)
            {
                string message = Properties.Resource.MUMLengthTooSmall;
                Trace.Report(message);
                throw new ArgumentException(message);
            }

            return true;
        }

        /// <summary>
        /// Get the alignment using pair wise
        /// </summary>
        /// <param name="seq1">Sequence 1</param>
        /// <param name="seq2">Sequence 2</param>
        /// <returns>A list of sequence alignments.</returns>
        private IList<IPairwiseSequenceAlignment> RunPairWise(ISequence seq1, ISequence seq2)
        {
            IList<IPairwiseSequenceAlignment> sequenceAlignment = null;

            if (PairWiseAlgorithm == null)
            {
                PairWiseAlgorithm = new NeedlemanWunschAligner();
            }

            PairWiseAlgorithm.SimilarityMatrix = SimilarityMatrix;
            PairWiseAlgorithm.GapOpenCost = GapOpenCost;
            PairWiseAlgorithm.ConsensusResolver = this.ConsensusResolver;

            if (UseGapExtensionCost)
            {
                PairWiseAlgorithm.GapExtensionCost = GapExtensionCost;
                sequenceAlignment = PairWiseAlgorithm.Align(seq1, seq2);
            }
            else
            {
                sequenceAlignment = PairWiseAlgorithm.AlignSimple(seq1, seq2);
            }

            // MUMmer does not support other aligners. Throw exception.
            //throw new NotSupportedException(Properties.Resource.MUMmerIncompatibleAligner);

            return sequenceAlignment;
        }

        /// <summary>
        /// get all the gaps in each sequence and call pairwise alignment
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="sequence">Query sequence</param>
        /// <returns>Aligned sequences</returns>
        private PairwiseAlignedSequence ProcessGaps(
                ISequence referenceSequence,
                ISequence sequence)
        {
            Sequence sequenceResult1;
            Sequence sequenceResult2;
            Sequence consensusResult;
            MaxUniqueMatch mum1 = null;
            MaxUniqueMatch mum2 = null;
            PairwiseAlignedSequence alignedSequence = new PairwiseAlignedSequence();

            sequenceResult1 = new Sequence(referenceSequence.Alphabet);
            sequenceResult1.IsReadOnly = false;
            sequenceResult1.ID = referenceSequence.ID;
            sequenceResult1.DisplayID = referenceSequence.DisplayID;

            sequenceResult2 = new Sequence(referenceSequence.Alphabet);
            sequenceResult2.IsReadOnly = false;
            sequenceResult2.ID = sequence.ID;
            sequenceResult2.DisplayID = sequence.DisplayID;

            consensusResult = new Sequence(referenceSequence.Alphabet);
            consensusResult.IsReadOnly = false;
            consensusResult.ID = sequence.ID;
            consensusResult.DisplayID = sequence.DisplayID;

            // Run the alignment for gap before first MUM
            List<int> insertions = new List<int>(2);
            insertions.Add(0);
            insertions.Add(0);

            List<int> gapInsertions;
            mum1 = _finalMumList[0];
            alignedSequence.Score += AlignGap(
                    referenceSequence,
                    sequence,
                    sequenceResult1,
                    sequenceResult2,
                    consensusResult,
                    null, // Here the first MUM does not exist
                    mum1,
                    out gapInsertions);

            insertions[0] += gapInsertions[0];
            insertions[1] += gapInsertions[1];

            // Run the alignment for all the gaps between MUM
            for (int index = 1; index < _finalMumList.Count; index++)
            {
                mum2 = _finalMumList[index];

                alignedSequence.Score += AlignGap(
                        referenceSequence,
                        sequence,
                        sequenceResult1,
                        sequenceResult2,
                        consensusResult,
                        mum1,
                        mum2,
                        out gapInsertions);

                insertions[0] += gapInsertions[0];
                insertions[1] += gapInsertions[1];

                mum1 = mum2;
            }

            // Run the alignment for gap after last MUM
            alignedSequence.Score += AlignGap(
                    referenceSequence,
                    sequence,
                    sequenceResult1,
                    sequenceResult2,
                    consensusResult,
                    mum1,
                    null,
                    out gapInsertions);

            insertions[0] += gapInsertions[0];
            insertions[1] += gapInsertions[1];

            alignedSequence.FirstSequence = sequenceResult1;
            alignedSequence.SecondSequence = sequenceResult2;
            alignedSequence.Consensus = consensusResult;

            // Offset is not required as Smith Waterman will  fragmented alignment. 
            // Offset is the starting position of alignment of sequence1 with respect to sequence2.
            if (PairWiseAlgorithm is NeedlemanWunschAligner)
            {
                alignedSequence.FirstOffset = sequenceResult1.IndexOfNonGap() - referenceSequence.IndexOfNonGap();
                alignedSequence.SecondOffset = sequenceResult2.IndexOfNonGap() - sequence.IndexOfNonGap();
            }


            List<int> startOffsets = new List<int>(2);
            List<int> endOffsets = new List<int>(2);
            startOffsets.Add(0);
            startOffsets.Add(0);

            endOffsets.Add(referenceSequence.Count -1);
            endOffsets.Add(sequence.Count -1);

            alignedSequence.Metadata["StartOffsets"] = startOffsets;
            alignedSequence.Metadata["EndOffsets"] = endOffsets;
            alignedSequence.Metadata["Insertions"] = insertions;

            // return the aligned sequence
            return alignedSequence;
        }

        /// <summary>
        /// Align the Gap by executing pairwise alignment
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequence">Query Sequence</param>
        /// <param name="sequenceResult1">Editable sequence containing alignment first result</param>
        /// <param name="sequenceResult2">Editable sequence containing alignment second result</param>
        /// <param name="consensusResult">Editable sequence containing consensus sequence</param>
        /// <param name="mum1">First MUM of Gap</param>
        /// <param name="mum2">Second MUM of Gap</param>
        /// <param name="insertions">Insetions made to the aligned sequences.</param>
        /// <returns>Score of alignment</returns>
        private int AlignGap(
                ISequence referenceSequence,
                ISequence querySequence,
                ISequence sequenceResult1,
                ISequence sequenceResult2,
                ISequence consensusResult,
                MaxUniqueMatch mum1,
                MaxUniqueMatch mum2,
                out List<int> insertions)
        {
            int score = 0;
            ISequence sequence1 = null;
            ISequence sequence2 = null;
            IList<IPairwiseSequenceAlignment> sequenceAlignment = null;
            string mum1String = string.Empty;
            string mum2String = string.Empty;

            insertions = new List<int>(2);
            insertions.Add(0);
            insertions.Add(0);

            int mum1ReferenceStartIndex = 0;
            int mum1QueryStartIndex = 0;
            int mum1Length = 0;
            int mum2ReferenceStartIndex = 0;
            int mum2QueryStartIndex = 0;
            int mum2Length = 0;

            if (null != mum1)
            {
                mum1ReferenceStartIndex = mum1.FirstSequenceStart;
                mum1QueryStartIndex = mum1.SecondSequenceStart;
                mum1Length = mum1.Length;
            }

            if (null != mum2)
            {
                mum2ReferenceStartIndex = mum2.FirstSequenceStart;
                mum2QueryStartIndex = mum2.SecondSequenceStart;
                mum2Length = mum2.Length;
            }
            else
            {
                mum2ReferenceStartIndex = referenceSequence.Count;
                mum2QueryStartIndex = querySequence.Count;
            }

            int referenceGapStartIndex = mum1ReferenceStartIndex + mum1Length;
            int queryGapStartIndex = mum1QueryStartIndex + mum1Length;

            if (mum2ReferenceStartIndex > referenceGapStartIndex
                && mum2QueryStartIndex > queryGapStartIndex)
            {
                sequence1 = referenceSequence.Range(
                    referenceGapStartIndex,
                    mum2ReferenceStartIndex - referenceGapStartIndex);
                sequence2 = querySequence.Range(
                    queryGapStartIndex,
                    mum2QueryStartIndex - queryGapStartIndex);

                sequenceAlignment = RunPairWise(sequence1, sequence2);

                if (sequenceAlignment != null)
                {
                    foreach (IPairwiseSequenceAlignment pairwiseAlignment in sequenceAlignment)
                    {
                        foreach (PairwiseAlignedSequence alignment in pairwiseAlignment.PairwiseAlignedSequences)
                        {
                            sequenceResult1.InsertRange(
                                    sequenceResult1.Count,
                                    alignment.FirstSequence.ToString());
                            sequenceResult2.InsertRange(
                                    sequenceResult2.Count,
                                    alignment.SecondSequence.ToString());
                            consensusResult.InsertRange(
                                consensusResult.Count,
                                    alignment.Consensus.ToString());

                            score += alignment.Score;

                            if (alignment.Metadata.ContainsKey("Insertions"))
                            {
                                List<int> gapinsertions = alignment.Metadata["Insertions"] as List<int>;
                                if (gapinsertions != null)
                                {
                                    if (gapinsertions.Count > 0)
                                    {
                                        insertions[0] += gapinsertions[0];
                                    }

                                    if (gapinsertions.Count > 1)
                                    {
                                        insertions[1] += gapinsertions[1];
                                    }
                                }
                            }

                        }
                    }
                }
            }
            else if (mum2ReferenceStartIndex > referenceGapStartIndex)
            {
                sequence1 = referenceSequence.Range(
                    referenceGapStartIndex,
                    mum2ReferenceStartIndex - referenceGapStartIndex);

                sequenceResult1.InsertRange(sequenceResult1.Count, sequence1.ToString());
                sequenceResult2.InsertRange(sequenceResult2.Count, CreateDefaultGap(sequence1.Count));
                consensusResult.InsertRange(consensusResult.Count, sequence1.ToString());

                insertions[1] += sequence1.Count;

                if (UseGapExtensionCost)
                {
                    score = GapOpenCost + ((sequence1.Count - 1) * GapExtensionCost);
                }
                else
                {
                    score = sequence1.Count * GapOpenCost;
                }
            }
            else if (mum2QueryStartIndex > queryGapStartIndex)
            {
                sequence2 = querySequence.Range(
                    queryGapStartIndex,
                    mum2QueryStartIndex - queryGapStartIndex);

                sequenceResult1.InsertRange(sequenceResult1.Count, CreateDefaultGap(sequence2.Count));
                sequenceResult2.InsertRange(sequenceResult2.Count, sequence2.ToString());
                consensusResult.InsertRange(consensusResult.Count, sequence2.ToString());

                insertions[0] += sequence2.Count;

                if (UseGapExtensionCost)
                {
                    score = GapOpenCost + ((sequence2.Count - 1) * GapExtensionCost);
                }
                else
                {
                    score = sequence2.Count * GapOpenCost;
                }
            }

            // Add the MUM to the result
            if (0 < mum2Length)
            {
                mum1String = referenceSequence.Range(
                        mum2ReferenceStartIndex,
                        mum2Length).ToString();
                sequenceResult1.InsertRange(sequenceResult1.Count, mum1String);

                mum2String = querySequence.Range(
                        mum2QueryStartIndex,
                        mum2Length).ToString();
                sequenceResult2.InsertRange(sequenceResult2.Count, mum2String);
                consensusResult.InsertRange(consensusResult.Count, mum1String);

                // Get the byte array (indices of symbol in MUM)
                byte[] indices = SimilarityMatrix.ToByteArray(mum1String);

                // Calculate the score
                foreach (byte index in indices)
                {
                    score += SimilarityMatrix[index, index];
                }
            }

            return score;
        }

        #endregion -- Private Methods --
    }
}
