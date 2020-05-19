﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bio.Registration;
using Bio.SimilarityMatrices;
using System.Linq;
using System.Diagnostics;

namespace Bio.Algorithms.Alignment.MultipleSequenceAlignment
{
    /// <summary>
    /// Implementation of modified MUSCLE multiple sequence alignment algorithm.
    /// Detailed can be found from paper: MUSCLE Edgar 2004.
    /// 
    /// There are three stages of MUSCLE (in the figure, green, cyan, and purple rectangles). 
    /// In Stage 1 the unaligned sequences are aligned in a fast and rough manner. Stage 2 and 
    /// stage 3 refine the alignment and can be iterated. The whole procedure can terminate at 
    /// the end of any stage and generate multiple sequence alignment (MSA). More iterations 
    /// yield more accurate MSA.
    ///
    /// Stage 1: draft progressive. In this stage the algorithm roughly aligns the sequences to 
    ///             generate the first multiple sequence alignment (MSA1). Stage 1 can be broken 
    ///             down into a few steps: a distance matrix (DM1) is first constructed via 
    ///             'k-mer counting', which will then be clustered into a binary guide tree (T1) 
    ///             by hierarchical clustering methods, e.g. UPGMA, neighbor-joining, or an even 
    ///             faster algorithm PartTree. In the guide tree, the leaves are the sequences.  
    ///             Next step is a general 'progressive alignment', where the sequences at the 
    ///             leaves of the tree will be aligned first, and then preceded to internal 
    ///             nodes in a bottom up manner.  A MSA is generated once the root is aligned. 
    ///             Progressive alignment employs profile-profile pair wise alignment algorithm 
    ///             (profile is a subset of aligned sequences), such as profile-profile 
    ///             Needleman-Wunsch algorithm in which a profile-profile function needs to be 
    ///             defined. 
    /// State 2: Improved progressive. With MSA1, a more accurate distance matrix can be constructed 
    ///             with Kimura distance matrix algorithm and the rest steps are the same as in 
    ///             step 1. Notice that in stage 2, accurate hierarchical clustering algorithms 
    ///             should be used, so PartTree is not an option here.
    /// Stage 3: Refinement. In this stage, the edges of guide tree will be  
    ///             selected in top-down order and broken, resulting in two sub-trees.which will be 
    ///             aligned separately. The two new MSAs are combined by 
    ///             profile-profile aligner. This step can be iterated until convergence or maximum 
    ///             iteration step is reached.
    /// 
    /// </summary>
    [RegistrableAttribute(true)]
    public class PAMSAMMultipleSequenceAligner : IMultipleSequenceAligner
    {
        #region Fields

        // Aligned sequences in the three stages.
        private List<ISequence> _alignedSequencesA, _alignedSequencesB, _alignedSequencesC;

        // The final aligned sequences.
        private List<ISequence> _alignedSequences;

        // The alignment score
        private float _alignmentScore = float.MinValue;

        // The alignment score in the three stages.
        private float _alignmentScoreA = float.MinValue, _alignmentScoreB = float.MinValue, _alignmentScoreC = float.MinValue;

        // The alphabet of this class - assigned from the first input sequence
        private IAlphabet _alphabet;

        // The number of partitions for parallelization
        // It is set to 1 for non-parallelization, and set to the number of processor cores for parallelization.
        private int _numberOfPartitions = 1;

        // The number of cores used by parallel extension
        private int _degreeOfParallelism = 1;

        #endregion

        #region Properties

        /// <summary>
        /// Distance function name (enum)
        /// </summary>
        public DistanceFunctionTypes DistanceFunctionName { get; set; }

        /// <summary>
        /// Cluster update method (enum)
        /// </summary>
        public UpdateDistanceMethodsTypes HierarchicalClusteringMethodName { get; set; }

        /// <summary>
        /// Profile Aligner name (enum)
        /// </summary>
        public ProfileAlignerNames ProfileAlignerName { get; set; }

        /// <summary>
        /// Profile distance function (enum)
        /// </summary>
        public ProfileScoreFunctionNames ProfileProfileFunctionName { get; set; }

        /// <summary>
        /// Kmer Length
        /// </summary>
        public int KmerLength { get; set; }

        /// <summary>
        /// Gets or sets the object that will be used to compute the alignment's consensus.
        /// </summary>
        public IConsensusResolver ConsensusResolver { set; get; }

        /// <summary>
        /// Aligned sequences of the stage 1 in this class.
        /// </summary>
        public List<ISequence> AlignedSequencesA
        {
            get { return _alignedSequencesA; }
        }

        /// <summary>
        /// Aligned sequences of the stage 2 in this class.
        /// </summary>
        public List<ISequence> AlignedSequencesB
        {
            get { return _alignedSequencesB; }
        }

        /// <summary>
        /// Aligned sequences of the stage 3 in this class.
        /// </summary>
        public List<ISequence> AlignedSequencesC
        {
            get { return _alignedSequencesC; }
        }

        /// <summary>
        /// The final aligned sequences in this class
        /// </summary>
        public IList<ISequence> AlignedSequences
        {
            get { return _alignedSequences; }
        }

        /// <summary>
        /// The alignment score of this class
        /// </summary>
        public float AlignmentScore
        {
            get { return _alignmentScore; }
        }

        /// <summary>
        /// The alignment score of this class in stage 1
        /// </summary>
        public float AlignmentScoreA
        {
            get { return _alignmentScoreA; }
        }

        /// <summary>
        /// The alignment score of this class in stage 2
        /// </summary>
        public float AlignmentScoreB
        {
            get { return _alignmentScoreB; }
        }

        /// <summary>
        /// The alignment score of this class in stage 3
        /// </summary>
        public float AlignmentScoreC
        {
            get { return _alignmentScoreC; }
        }

        /// <summary>
        /// The alignment method name of this class
        /// </summary>
        public string Name
        {
            get
            {
                return Resource.MuscleMultipleAlignmentMethodName;
            }
        }

        /// <summary>
        /// Gets the description of the sequence alignment algorithm
        /// </summary>
        public string Description
        {
            get
            {
                return Resource.MuscleMultipleAlignmentMethodDescription;
            }
        }

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
        #endregion

        #region Static Members
        /// <summary>
        /// Switch controls whether sequence weights are used
        /// </summary>
        static public bool UseWeights { get; set; }

        /// <summary>
        /// Control the number of cores used in parallel extensions
        /// </summary>
        static public int NumberOfCores { get; set; }

        /// <summary>
        /// Switch controls whether faster version PAMSAM is used.
        /// </summary>
        static public bool FasterVersion { get; set; }

        /// <summary>
        ///  Switch controls whether stage 2 is used
        /// </summary>
        static public bool UseStageB { get; set; }

        /// <summary>
        /// Set parallel options
        /// </summary>
        static public ParallelOptions parallelOption { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor used to add PAMSAM algorithm to SequenceAligners.All
        /// </summary>
        public PAMSAMMultipleSequenceAligner()
        {
            // Set parallel extension option
            //_degreeOfParallelism = Environment.ProcessorCount;
            parallelOption = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
            _numberOfPartitions = Environment.ProcessorCount * 2;

            // Initialize parameters to general defaults - must override by setting property values prior 
            // to calling Align.
            KmerLength = 3;
            DistanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            HierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            ProfileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            ProfileProfileFunctionName = ProfileScoreFunctionNames.WeightedInnerProduct;

            // SimularityMatrix will be assigned in Align if it's not set prior through property.
        }

        /// <summary>
        /// Construct an aligner and run the alignment.
        /// </summary>
        /// <param name="sequences">input sequences</param>
        /// <param name="kmerLength">positive integer of kmer length</param>
        /// <param name="distanceFunctionName">enum: distance function name</param>
        /// <param name="hierarchicalClusteringMethodName">enum: cluster update method</param>
        /// <param name="profileAlignerMethodName">enum: profile-profile aligner name</param>
        /// <param name="profileFunctionName">enum: profile-profile distance function</param>
        /// <param name="similarityMatrix">similarity matrix</param>
        /// <param name="gapOpenPenalty">negative gapOpenPenalty</param>
        /// <param name="gapExtendPenalty">negative gapExtendPenalty</param>
        /// <param name="numberOfPartitions">the number of partitions in dynamic programming</param>
        /// <param name="degreeOfParallelism">degree of parallelism option for parallel extension</param>
        public PAMSAMMultipleSequenceAligner(
                    IList<ISequence> sequences,
                    int kmerLength,
                    DistanceFunctionTypes distanceFunctionName,
                    UpdateDistanceMethodsTypes hierarchicalClusteringMethodName,
                    ProfileAlignerNames profileAlignerMethodName,
                    ProfileScoreFunctionNames profileFunctionName,
                    SimilarityMatrix similarityMatrix,
                    int gapOpenPenalty,
                    int gapExtendPenalty,
                    int numberOfPartitions,
                    int degreeOfParallelism)
        {
            Performance.Start();

            if (null == sequences)
            {
                throw new ArgumentNullException("sequences");
            }

            if (sequences.Count == 0)
            {
                throw new ArgumentException("Empty input sequences");
            }

            // Set parallel extension option
            if (degreeOfParallelism <= 0)
            {
                throw new ArgumentException("Invalid parallel degree parameter");
            }

            //_degreeOfParallelism = degreeOfParallelism;
            parallelOption = new ParallelOptions { MaxDegreeOfParallelism = degreeOfParallelism };

            if (numberOfPartitions <= 0)
            {
                throw new ArgumentException("Invalid number of partition parameter");
            }
            _numberOfPartitions = numberOfPartitions;

            // Assign the alphabet
            SetAlphabet(sequences, similarityMatrix, false);

            // Initialize parameters
            KmerLength = kmerLength;
            DistanceFunctionName = distanceFunctionName;
            HierarchicalClusteringMethodName = hierarchicalClusteringMethodName;
            ProfileAlignerName = profileAlignerMethodName;
            ProfileProfileFunctionName = profileFunctionName;
            SimilarityMatrix = similarityMatrix;
            GapOpenCost = gapOpenPenalty;
            GapExtensionCost = gapExtendPenalty;

            MsaUtils.SetProfileItemSets(_alphabet);

            Performance.Snapshot("Start Aligning");

            // Work...
            DoAlignment(sequences);
        }
        #endregion

        #region Methods

        /// <summary>
        /// currently not implemented
        /// </summary>
        /// <param name="inputSequences"></param>
        /// <returns></returns>
        public IList<Bio.Algorithms.Alignment.ISequenceAlignment> AlignSimple(IEnumerable<ISequence> inputSequences)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Performs Stage 1, 2, and 3 as described in class description.
        /// </summary>
        /// <param name="inputSequences">Input sequences</param>
        /// <returns>Alignment results</returns>
        public IList<Alignment.ISequenceAlignment> Align(IEnumerable<ISequence> inputSequences)
        {
            // Reset all our data in case this same instance is used multiple times.
            _alignedSequences = _alignedSequencesA = _alignedSequencesB = _alignedSequencesC = null;
            _alignmentScore = _alignmentScoreA = _alignmentScoreB = _alignmentScoreC = float.MinValue;

            // Get our list of sequences.
            List<ISequence> sequences = inputSequences.ToList();
            if (sequences.Count == 0)
            {
                throw new ArgumentException("Empty input sequences");
            }

            // Assign the gap open/extension cost if it hasn't been assigned.
            if (GapOpenCost == 0)
                GapOpenCost = -4;
            if (GapExtensionCost == 0)
                GapExtensionCost = -1;

            Performance.Start();

            // Assign the alphabet
            SetAlphabet(sequences, SimilarityMatrix, true);
            MsaUtils.SetProfileItemSets(_alphabet);

            Performance.Snapshot("Start Aligning");

            // Work...
            DoAlignment(sequences);

            // just for the purpose of integrating PW and MSA with the same output
            var alignment = new Alignment.SequenceAlignment();
            IAlignedSequence aSequence = new AlignedSequence();
            foreach (var alignedSequence in AlignedSequences)
                aSequence.Sequences.Add(alignedSequence);
            foreach (var inputSequence in sequences)
                alignment.Sequences.Add(inputSequence);
            alignment.AlignedSequences.Add(aSequence);
            return new List<Alignment.ISequenceAlignment>() {alignment};
        }


        /// <summary>
        /// This method assigns the alphabet from the input sequences
        /// </summary>
        /// <param name="sequences">Input sequences</param>
        /// <param name="similarityMatrix">Matrix to use for similarity comparisons</param>
        /// <param name="fixSimilarityMatrixErrors">True to fix any similarity matrix issue related to the alphabet.</param>
        private void SetAlphabet(IList<ISequence> sequences, SimilarityMatrix similarityMatrix, bool fixSimilarityMatrixErrors)
        {
            if (sequences.Count == 0)
            {
                throw new ArgumentException("Empty input sequences");
            }

            // Validate data type
            _alphabet = Alphabets.GetAmbiguousAlphabet(sequences[0].Alphabet);
            Parallel.For(1, sequences.Count, PAMSAMMultipleSequenceAligner.parallelOption, i =>
            {
                if (!Alphabets.CheckIsFromSameBase(sequences[i].Alphabet, _alphabet))
                {
                    throw new ArgumentException("Inconsistent sequence alphabet");
                }
            });

            SimilarityMatrix bestSimilarityMatrix = null;

            if (_alphabet is DnaAlphabet)
            {
                bestSimilarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            }
            else if (_alphabet is RnaAlphabet)
            {
                bestSimilarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousRna);
            }
            else if (_alphabet is ProteinAlphabet)
            {
                bestSimilarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum50);
            }

            // Check or assign the similarity matrix.
            if (similarityMatrix == null)
            {
                SimilarityMatrix = bestSimilarityMatrix;
                if (SimilarityMatrix == null)
                    throw new ArgumentException("Unknown alphabet - could not choose SimilarityMatrix.");
            }
            else
            {
                List<String> similarityMatrixDNA = new List<String> { "AmbiguousDNA" };
                List<String> similarityMatrixRNA = new List<String> { "AmbiguousRNA" };
                List<String> similarityMatrixProtein = new List<String> { "BLOSUM45", "BLOSUM50", "BLOSUM62", "BLOSUM80", "BLOSUM90", "PAM250", "PAM30", "PAM70" };

                if (_alphabet is DnaAlphabet)
                {
                    if (!similarityMatrixDNA.Contains(similarityMatrix.Name))
                    {
                        if (fixSimilarityMatrixErrors)
                            SimilarityMatrix = bestSimilarityMatrix;
                        else
                            throw new ArgumentException("Inappropriate Similarity Matrix for DNA.");
                    }
                }
                else if (_alphabet is ProteinAlphabet)
                {
                    if (!similarityMatrixProtein.Contains(similarityMatrix.Name))
                    {
                        if (fixSimilarityMatrixErrors)
                            SimilarityMatrix = bestSimilarityMatrix;
                        else
                            throw new ArgumentException("Inappropriate Similarity Matrix for Protein.");
                    }
                }
                else if (_alphabet is RnaAlphabet)
                {
                    if (!similarityMatrixRNA.Contains(similarityMatrix.Name))
                    {
                        if (fixSimilarityMatrixErrors)
                            SimilarityMatrix = bestSimilarityMatrix;
                        else
                            throw new ArgumentException("Inappropriate Similarity Matrix for RNA.");
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid alphabet");
                }
            }
        }

        /// <summary>
        /// Performs Stage 1, 2, and 3 as described in class description.
        /// </summary>
        /// <param name="sequences">Input sequences</param>
        /// <returns>Alignment results</returns>
        private void DoAlignment(IList<ISequence> sequences)
        {
            Debug.Assert(_alphabet != null);
            Debug.Assert(sequences.Count > 0);

            // Initializations
            if (ConsensusResolver == null)
                ConsensusResolver = new SimpleConsensusResolver(_alphabet);
            else
                ConsensusResolver.SequenceAlphabet = _alphabet;

            // Get ProfileAligner ready
            IProfileAligner profileAligner = null;
            switch (ProfileAlignerName)
            {
                case (ProfileAlignerNames.NeedlemanWunschProfileAligner):
                    if (_degreeOfParallelism == 1)
                    {
                        profileAligner = new NeedlemanWunschProfileAlignerSerial(
                            SimilarityMatrix, ProfileProfileFunctionName, GapOpenCost, GapExtensionCost, _numberOfPartitions);
                    }
                    else
                    {
                        profileAligner = new NeedlemanWunschProfileAlignerParallel(
                            SimilarityMatrix, ProfileProfileFunctionName, GapOpenCost, GapExtensionCost, _numberOfPartitions);
                    }
                    break;
                case (ProfileAlignerNames.SmithWatermanProfileAligner):
                    if (_degreeOfParallelism == 1)
                    {
                        profileAligner = new SmithWatermanProfileAlignerSerial(
                        SimilarityMatrix, ProfileProfileFunctionName, GapOpenCost, GapExtensionCost, _numberOfPartitions);

                    }
                    else
                    {
                        profileAligner = new SmithWatermanProfileAlignerParallel(
                    SimilarityMatrix, ProfileProfileFunctionName, GapOpenCost, GapExtensionCost, _numberOfPartitions);

                    }
                    break;
                default:
                    throw new ArgumentException("Invalid profile aligner name");
            }

            _alignedSequences = new List<ISequence>(sequences.Count);
            float currentScore = 0;

            // STAGE 1

            Performance.Snapshot("Stage 1");
            // Generate DistanceMatrix
            KmerDistanceMatrixGenerator kmerDistanceMatrixGenerator =
                new KmerDistanceMatrixGenerator(sequences, KmerLength, _alphabet, DistanceFunctionName);

            // Hierarchical clustering
            IHierarchicalClustering hierarcicalClustering =
                new HierarchicalClusteringParallel
                    (kmerDistanceMatrixGenerator.DistanceMatrix, HierarchicalClusteringMethodName);

            // Generate Guide Tree
            BinaryGuideTree binaryGuideTree =
                new BinaryGuideTree(hierarcicalClustering);

            // Progressive Alignment
            IProgressiveAligner progressiveAlignerA = new ProgressiveAligner(profileAligner);
            progressiveAlignerA.Align(sequences, binaryGuideTree);

            currentScore = MsaUtils.MultipleAlignmentScoreFunction(progressiveAlignerA.AlignedSequences, SimilarityMatrix, GapOpenCost, GapExtensionCost);
            if (currentScore > _alignmentScoreA)
            {
                _alignmentScoreA = currentScore;
                _alignedSequencesA = progressiveAlignerA.AlignedSequences;
            }
            if (_alignmentScoreA > _alignmentScore)
            {
                _alignmentScore = _alignmentScoreA;
                _alignedSequences = _alignedSequencesA;
            }

            if (PAMSAMMultipleSequenceAligner.FasterVersion)
            {
                _alignedSequencesB = _alignedSequencesA;
                _alignedSequencesC = _alignedSequencesA;
                _alignmentScoreB = _alignmentScoreA;
                _alignmentScoreC = _alignmentScoreA;
            }
            else
            {
                BinaryGuideTree binaryGuideTreeB = null;
                IHierarchicalClustering hierarcicalClusteringB = null;
                KimuraDistanceMatrixGenerator kimuraDistanceMatrixGenerator = new KimuraDistanceMatrixGenerator();

                if (PAMSAMMultipleSequenceAligner.UseStageB)
                {
                    // STAGE 2
                    Performance.Snapshot("Stage 2");
                    // Generate DistanceMatrix from Multiple Sequence Alignment

                    while (true)
                    {
                        kimuraDistanceMatrixGenerator.GenerateDistanceMatrix(_alignedSequences);

                        // Hierarchical clustering
                        hierarcicalClusteringB = new HierarchicalClusteringParallel
                                (kimuraDistanceMatrixGenerator.DistanceMatrix, HierarchicalClusteringMethodName);

                        // Generate Guide Tree
                        binaryGuideTreeB = new BinaryGuideTree(hierarcicalClusteringB);

                        BinaryGuideTree.CompareTwoTrees(binaryGuideTreeB, binaryGuideTree);
                        binaryGuideTree = binaryGuideTreeB;

                        // Progressive Alignment
                        IProgressiveAligner progressiveAlignerB = new ProgressiveAligner(profileAligner);
                        progressiveAlignerB.Align(sequences, binaryGuideTreeB);

                        currentScore = MsaUtils.MultipleAlignmentScoreFunction(progressiveAlignerB.AlignedSequences, SimilarityMatrix, GapOpenCost, GapExtensionCost);

                        if (currentScore > _alignmentScoreB)
                        {
                            _alignmentScoreB = currentScore;
                            _alignedSequencesB = progressiveAlignerB.AlignedSequences;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (_alignmentScoreB > _alignmentScore)
                    {
                        _alignmentScore = _alignmentScoreB;
                        _alignedSequences = _alignedSequencesB;
                    }
                }
                else
                {
                    binaryGuideTreeB = binaryGuideTree;
                }


                // STAGE 3
                Performance.Snapshot("Stage 3");
                // refinement
                int maxRefineMentTime = 1;
                if (sequences.Count == 2)
                {
                    maxRefineMentTime = 0;
                }

                int refinementTime = 0;
                _alignedSequencesC = new List<ISequence>(_alignedSequences.Count);
                foreach (ISequence t in _alignedSequences)
                {
                    _alignedSequencesC.Add(new Sequence(Alphabets.GetAmbiguousAlphabet(_alphabet), t.ToArray())
                        {
                            ID = t.ID,
                            // Do not shallow copy dictionary
                            //Metadata = t.Metadata
                        });
                }

                List<int>[] allIndelPositions = null;
                List<int>[] eStrings = null;

                while (refinementTime < maxRefineMentTime)
                {
                    ++refinementTime;
                    Performance.Snapshot("Refinement iter " + refinementTime);
                    bool needRefinement = false;
                    for (int edgeIndex = 0; edgeIndex < binaryGuideTreeB.NumberOfEdges; ++edgeIndex)
                    {
                        List<int>[] leafNodeIndices = binaryGuideTreeB.SeparateSequencesByCuttingTree(edgeIndex);

                        allIndelPositions = new List<int>[2];

                        IProfileAlignment[] separatedProfileAlignments = ProfileAlignment.ProfileExtraction(_alignedSequencesC, leafNodeIndices[0], leafNodeIndices[1], out allIndelPositions);
                        eStrings = new List<int>[2];

                        if (separatedProfileAlignments[0].NumberOfSequences < separatedProfileAlignments[1].NumberOfSequences)
                        {
                            profileAligner.Align(separatedProfileAlignments[0], separatedProfileAlignments[1]);
                            eStrings[0] = profileAligner.GenerateEString(profileAligner.AlignedA);
                            eStrings[1] = profileAligner.GenerateEString(profileAligner.AlignedB);
                        }
                        else
                        {
                            profileAligner.Align(separatedProfileAlignments[1], separatedProfileAlignments[0]);
                            eStrings[0] = profileAligner.GenerateEString(profileAligner.AlignedB);
                            eStrings[1] = profileAligner.GenerateEString(profileAligner.AlignedA);
                        }

                        for (int set = 0; set < 2; ++set)
                        {
                            Parallel.ForEach(leafNodeIndices[set], PAMSAMMultipleSequenceAligner.parallelOption, i =>
                            {
                                //Sequence seq = new Sequence(_alphabet, "");
                                List<byte> seqBytes = new List<byte>();

                                int indexAllIndel = 0;
                                for (int j = 0; j < _alignedSequencesC[i].Count; ++j)
                                {
                                    if (indexAllIndel < allIndelPositions[set].Count && j == allIndelPositions[set][indexAllIndel])
                                    {
                                        ++indexAllIndel;
                                    }
                                    else
                                    {
                                        seqBytes.Add(_alignedSequencesC[i][j]);
                                    }
                                }

                                _alignedSequencesC[i] = profileAligner.GenerateSequenceFromEString(eStrings[set], new Sequence(Alphabets.GetAmbiguousAlphabet(_alphabet), seqBytes.ToArray()));
                                _alignedSequencesC[i].ID = _alignedSequencesC[i].ID;
                                // Do not shallow copy dictionary
                                //(_alignedSequencesC[i] as Sequence).Metadata = _alignedSequencesC[i].Metadata;
                            });
                        }

                        currentScore = MsaUtils.MultipleAlignmentScoreFunction(_alignedSequencesC, SimilarityMatrix, GapOpenCost, GapExtensionCost);

                        if (currentScore > _alignmentScoreC)
                        {
                            _alignmentScoreC = currentScore;
                            needRefinement = true;

                            // recreate the tree
                            kimuraDistanceMatrixGenerator.GenerateDistanceMatrix(_alignedSequencesC);
                            hierarcicalClusteringB = new HierarchicalClusteringParallel
                                    (kimuraDistanceMatrixGenerator.DistanceMatrix, HierarchicalClusteringMethodName);

                            binaryGuideTreeB = new BinaryGuideTree(hierarcicalClusteringB);
                            break;
                        }
                    }
                    if (!needRefinement)
                    {
                        refinementTime = maxRefineMentTime;
                        break;
                    }

                }
                if (_alignmentScoreC > _alignmentScore)
                {
                    _alignmentScore = _alignmentScoreC;
                    _alignedSequences = _alignedSequencesC;
                }
                Performance.Snapshot("Stop Stage 3");
            }
        }
        #endregion
    }
}
