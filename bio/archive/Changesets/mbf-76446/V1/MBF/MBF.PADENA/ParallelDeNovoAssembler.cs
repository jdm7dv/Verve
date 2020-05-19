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
using MBF.Algorithms.Assembly.Graph;
using MBF.Algorithms.Assembly.PaDeNA.Scaffold;
using MBF.Registration;

namespace MBF.Algorithms.Assembly.PaDeNA
{
    /// <summary>
    /// Implements a de bruijn based approach for
    /// assembly of DNA sequences.
    /// </summary>
    [RegistrableAttribute(true)]
    public class ParallelDeNovoAssembler : IDeBruijnDeNovoAssembler, IDisposable
    {
        #region Fields
        /// <summary>
        /// User Input Parameter
        /// Length of k-mer
        /// </summary>
        private int _kmerLength;

        /// <summary>
        /// User Input Parameter
        /// Threshold for removing dangling ends in graph
        /// </summary>
        private int _dangleThreshold = 0;

        /// <summary>
        /// Bool to do erosion or not.
        /// </summary>
        private bool _isErosionEnabled = false;

        /// <summary>
        /// User Input Parameter
        /// Threshold for eroding low coverage ends
        /// </summary>
        private int _erosionThreshold = -1;

        /// <summary>
        /// User Input Parameter
        /// Length Threshold for removing redundant paths in graph
        /// </summary>
        private int _redundantPathLengthThreshold = 0;

        /// <summary>
        /// Threshold used for removing low-coverage contigs
        /// </summary>
        private double _contigCoverageThreshold = -1;

        /// <summary>
        /// Class implementing Low coverage contig removal.
        /// </summary>
        private ILowCoverageContigPurger _lowCoverageContigPurger;

        /// <summary>
        /// List of input sequence reads. Different steps in the assembly 
        /// may access this. Should be set before starting the assembly process.
        /// </summary>
        private List<ISequence> _sequenceReads;

        /// <summary>
        /// Holds the de bruijn graph used for assembly process.
        /// Graph creation modules sets this, so that further steps 
        /// can access this for modifications.
        /// </summary>
        private DeBruijnGraph _graph;

        /// <summary>
        /// Class implementing dangling links purging
        /// </summary>
        private IGraphErrorPurger _danglingLinksPurger;

        /// <summary>
        /// Class implementing redundant paths purger
        /// </summary>
        private IGraphErrorPurger _redundantPathsPurger;

        /// <summary>
        /// Class implementing contig building
        /// </summary>
        private IContigBuilder _contigBuilder;

        /// <summary>
        /// Class implementing scaffold building
        /// </summary>
        private IGraphScaffoldBuilder _scaffoldBuilder;

        #endregion

        /// <summary>
        /// Initializes a new instance of the ParallelDeNovoAssembler class.
        /// Sets thresholds to default values.
        /// Also initializes instances implementing different steps 
        /// </summary>
        public ParallelDeNovoAssembler()
        {
            // Initialize to default here.
            // Values set to -1 here will be reset based on input sequences.
            _kmerLength = -1;
            _dangleThreshold = -1;
            _redundantPathLengthThreshold = -1;
            _sequenceReads = new List<ISequence>();

            // Contig and scaffold Builder are required modules. Set this to default.
            _contigBuilder = new SimplePathContigBuilder();

            // Default values for parameters used in building scaffolds.
            ScaffoldRedundancy = 2;
            Depth = 10;
            AllowKmerLengthEstimation = true;
        }

        #region Properties
        /// <summary>
        /// Gets the name of the current assembly algorithm used.
        /// This property returns the Name of our assembly algorithm i.e 
        /// Parallel De Novo algorithm.
        /// </summary>
        public string Name
        {
            get { return Properties.Resource.PaDeNA; }
        }

        /// <summary>
        /// Gets the description of the current assembly algorithm used.
        /// This property returns a simple description of what 
        ///  Parallel De Novo class implements.
        /// </summary>
        public string Description
        {
            get { return Properties.Resource.PaDeNADescription; }
        }

        /// <summary>
        /// Gets or sets the kmer length
        /// </summary>
        public int KmerLength
        {
            get { return _kmerLength; }
            set 
            { 
                _kmerLength = value;
                AllowKmerLengthEstimation = false;
            }
        }

        /// <summary>
        /// Gets or sets whether to estimate kmer length.
        /// </summary>
        public bool AllowKmerLengthEstimation { get; set; }

        /// <summary>
        /// Gets the assembler de-bruijn graph
        /// </summary>
        public DeBruijnGraph Graph
        {
            get { return _graph; }
        }

        /// <summary>
        /// Gets or sets the instance that implements
        /// dangling links purging step.
        /// </summary>
        public IGraphErrorPurger DanglingLinksPurger
        {
            get { return _danglingLinksPurger; }
            set { _danglingLinksPurger = value; }
        }

        /// <summary>
        /// Gets or sets the threshold length 
        /// for dangling link purger.
        /// </summary>
        public int DanglingLinksThreshold
        {
            get { return _dangleThreshold; }
            set { _dangleThreshold = value; }
        }

        /// <summary>
        /// Gets or sets value to allow erosion of the graph.
        /// </summary>
        public bool AllowErosion
        {
            get { return _isErosionEnabled; }
            set { _isErosionEnabled = value; }
        }

        /// <summary>
        /// Gets or sets the threshold length for eroding low coverage graph 
        /// ends. In case erosion step is not to be done, set this to 0.
        /// As an performance optimization in assembler process, erosion and 
        /// dangling link purging step are done together in a single step. 
        /// Note that because of this optimization, unless the danglingLinkPurger 
        /// implements IGraphErodePurger, erosion will not be done irrespective 
        /// of the threshold value provided. 
        /// </summary>
        public int ErosionThreshold
        {
            get { return _erosionThreshold; }
            set { _erosionThreshold = value; }
        }

        /// <summary>
        /// Gets or sets the instance that implements
        /// redundant paths purging step.
        /// </summary>
        public IGraphErrorPurger RedundantPathsPurger
        {
            get { return _redundantPathsPurger; }
            set { _redundantPathsPurger = value; }
        }

        /// <summary>
        /// Gets or sets the length threshold 
        /// for redundant paths purger.
        /// </summary>
        public int RedundantPathLengthThreshold
        {
            get { return _redundantPathLengthThreshold; }
            set { _redundantPathLengthThreshold = value; }
        }

        /// <summary>
        /// Gets or sets instance of class implementing Low coverage contig removal.
        /// </summary>
        public ILowCoverageContigPurger LowCoverageContigPurger
        {
            get { return _lowCoverageContigPurger; }
            set { _lowCoverageContigPurger = value; }
        }

        /// <summary>
        /// Gets or sets the value to enable removal of low coverage contigs.
        /// </summary>
        public bool AllowLowCoverageContigRemoval { get; set; }

        /// <summary>
        /// Threshold used for removing low-coverage contigs
        /// </summary>
        public double ContigCoverageThreshold
        {
            get { return _contigCoverageThreshold; }
            set { _contigCoverageThreshold = value; }
        }

        /// <summary>
        /// Gets or sets the instance that implements
        /// contig building step.
        /// </summary>
        public IContigBuilder ContigBuilder
        {
            get { return _contigBuilder; }
            set { _contigBuilder = value; }
        }

        /// <summary>
        /// Gets or sets the instance that implements
        /// scaffold building step.
        /// </summary>
        public IGraphScaffoldBuilder ScaffoldBuilder
        {
            get { return _scaffoldBuilder; }
            set { _scaffoldBuilder = value; }
        }

        /// <summary>
        /// Gets or sets value of redundancy for building scaffolds.
        /// </summary>
        public int ScaffoldRedundancy { get; set; }

        /// <summary>
        /// Gets or sets the Depth for graph traversal in scaffold builder step.
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        /// Gets the list of sequence reads
        /// </summary>
        protected IList<ISequence> SequenceReads
        {
            get { return _sequenceReads; }
        }

        #endregion

        /// <summary>
        /// Add given list of sequences to input read list
        /// </summary>
        /// <param name="sequences">List of sequences to add</param>
        public void AddSequenceReads(IList<ISequence> sequences)
        {
            _sequenceReads.AddRange(sequences);
        }

        /// <summary>
        /// Assemble the list of sequence reads
        /// </summary>
        /// <param name="inputSequences">List of input sequences</param>
        /// <returns>Assembled output</returns>
        public IDeNovoAssembly Assemble(IList<ISequence> inputSequences)
        {
            ValidateAllSequences(inputSequences);

            // Remove ambiguous reads and set up fields for assembler process
            Initialize(inputSequences);

            // Step 1, 2: Create k-mers from reads and build de bruijn graph
            CreateGraph();

            // Estimate and set default value for erosion and coverage thresholds
            EstimateDefaultThresholds();

            // Step 3: Remove dangling links from graph
            UnDangleGraph();

            // Step 4: Remove redundant paths from graph
            RemoveRedundancy();

            // Perform dangling link purger step once more.
            // This is done to remove any links created by redundant paths purger.
            UnDangleGraph();

            // Step 5: Build Contigs
            IList<ISequence> contigSequences = BuildContigs();

            PaDeNAAssembly result = new PaDeNAAssembly();
            result.AddContigs(contigSequences);

            _graph.Dispose();
            return result;
        }

        /// <summary>
        /// Assemble the list of sequence reads. Also performs the 
        /// scaffold building step as part of assembly process.
        /// </summary>
        /// <param name="inputSequences">List of input sequences</param>
        /// <param name="includeScaffolds">Boolean indicating whether scaffold building step has to be run</param>
        /// <returns>Assembled output</returns>
        public IDeNovoAssembly Assemble(IList<ISequence> inputSequences, bool includeScaffolds)
        {
            PaDeNAAssembly assemblyResult = (PaDeNAAssembly)Assemble(inputSequences);

            if (includeScaffolds)
            {
                // Step 6: Build _scaffolds
                IList<ISequence> scaffolds = BuildScaffolds(assemblyResult.ContigSequences);

                if (scaffolds != null)
                {
                    assemblyResult.AddScaffolds(scaffolds);
                }
            }

            return assemblyResult;
        }

        /// <summary>
        /// For optimal graph formation, k-mer length should not be less 
        /// than half the length of the longest input sequence and 
        /// cannot be more than the length of the shortest input sequence. 
        /// Reference for estimating kmerlength from reads: Supplement material from 
        /// publication "ABySS: A parallel assembler for short read sequence data"
        /// </summary>
        /// <param name="sequences">List of input sequences</param>
        /// <returns>Estimated optimal kmer length</returns>
        public static int EstimateKmerLength(IList<ISequence> sequences)
        {
            // kmer length should be less than input sequence lengths
            float maxLength = sequences.AsParallel().Min(s => s.Count);

            // for optimal purpose, kmer length should be more than half of longest sequence
            float minLength = (float)sequences.AsParallel().Max(s => s.Count) / 2;

            if (minLength < maxLength)
            {
                // Choose median value between the end-points
                return (int)Math.Ceiling((minLength + maxLength) / 2);
            }
            else
            {
                // In this case pick maxLength, since this is a hard limit
                return (int)Math.Floor(maxLength);
            }
        }

        /// <summary>
        /// Estimates and sets erosion and coverage threshold for contigs.
        /// Median value of kmer coverage is set as default value.
        /// Reference: ABySS Release Notes 1.1.1 - "The default threshold 
        /// is the square root of the median k-mer coverage"
        /// </summary>
        protected void EstimateDefaultThresholds()
        {
            if (_isErosionEnabled || AllowLowCoverageContigRemoval)
            {
                // In case of low coverage data, set default as 2.
                // Reference: ABySS Release Notes 1.0.15
                // Before calculating median, discard thresholds less than 2
                List<int> kmerCoverage =
                    _graph.Nodes.AsParallel().Aggregate(new List<int>(), (kmerList, n) =>
                    {
                        if (n.KmerCount > 2)
                            kmerList.Add(n.KmerCount);
                        return kmerList;
                    });

                double threshold;
                if (kmerCoverage.Count == 0)
                    threshold = 2; // For low coverage data, set default as 2
                else
                {
                    kmerCoverage.Sort();
                    int midPoint = kmerCoverage.Count / 2;
                    double median = (kmerCoverage.Count % 2 == 1 || midPoint == 0) ?
                        kmerCoverage[midPoint] :
                        ((float)(kmerCoverage[midPoint] + kmerCoverage[midPoint - 1])) / 2;
                    threshold = Math.Sqrt(median);
                }

                // Set coverage threshold
                if (AllowLowCoverageContigRemoval && _contigCoverageThreshold == -1)
                {
                    _contigCoverageThreshold = threshold;
                }

                if (_isErosionEnabled && _erosionThreshold == -1)
                {
                    // Erosion threshold is an int, so round it off
                    _erosionThreshold = (int)Math.Round(threshold);
                }
            }
        }

        /// <summary>
        /// Removes input sequences that have ambiguous symbols.
        /// Updates the field holding sequence reads.
        /// </summary>
        /// <param name="inputSequences">List of input sequences</param>
        protected void RemoveAmbiguousReads(IList<ISequence> inputSequences)
        {
            _sequenceReads = new List<ISequence>(
                inputSequences.AsParallel().Where(s => s.All(c => !c.IsAmbiguous && !c.IsGap)));
            if (_sequenceReads.Count == 0)
            {
                throw new InvalidOperationException(Properties.Resource.AmbiguousCharacter);
            }
        }

        /// <summary>
        /// Step 1: Building k-mers from sequence reads
        /// Step 2: Build de bruijn graph for input set of k-mers.
        /// Sets the _assemblerGraph field.
        /// </summary>
        protected void CreateGraph()
        {
            _graph = new DeBruijnGraph();
            _graph.Build(_sequenceReads, _kmerLength);
        }

        /// <summary>
        /// Step 3: Remove dangling links from graph
        /// </summary>
        protected void UnDangleGraph()
        {
            if (_danglingLinksPurger != null && _dangleThreshold > 0)
            {
                DeBruijnPathList danglingNodes = null;

                // Observe lenghts of dangling links in the graph
                // This is an optimization - instead of incrementing threshold by 1 and 
                // running the purger iteratively, we first determine the lengths of the 
                // danglings links found in the graph and run purger only for those lengths.
                _danglingLinksPurger.LengthThreshold = _dangleThreshold - 1;

                IEnumerable<int> danglingLengths;
                IGraphEndsEroder graphEndsEroder = _danglingLinksPurger as IGraphEndsEroder;
                if (graphEndsEroder != null && _isErosionEnabled)
                {
                    // If eroder is implemented, while getting lengths of dangling links, 
                    // it also erodes the low coverage ends.
                    danglingLengths = graphEndsEroder.ErodeGraphEnds(_graph, _erosionThreshold);
                }
                else
                {
                    // Perform dangling purger at all incremental values till dangleThreshold.
                    danglingLengths = Enumerable.Range(1, _dangleThreshold - 1);
                }

                // Erosion is to be only once. Reset erode threshold to -1.
                _erosionThreshold = -1;

                // Start removing dangling links
                foreach (int threshold in danglingLengths)
                {
                    if (_graph.Nodes.Count >= threshold)
                    {
                        _danglingLinksPurger.LengthThreshold = threshold;
                        danglingNodes = _danglingLinksPurger.DetectErroneousNodes(_graph);
                        _danglingLinksPurger.RemoveErroneousNodes(_graph, danglingNodes);
                    }
                }

                // Removing dangling links can in turn create more dangling links
                // In order to remove all links within threshold, we therefore run
                // purger at threshold length until there is no more change in graph.
                do
                {
                    danglingNodes = null;
                    if (_graph.Nodes.Count >= _dangleThreshold)
                    {
                        _danglingLinksPurger.LengthThreshold = _dangleThreshold;
                        danglingNodes = _danglingLinksPurger.DetectErroneousNodes(_graph);
                        _danglingLinksPurger.RemoveErroneousNodes(_graph, danglingNodes);
                    }
                }
                while (danglingNodes != null && danglingNodes.Paths.Count > 0);
            }
        }

        /// <summary>
        /// Step 4: Remove redundant paths from graph
        /// </summary>
        protected void RemoveRedundancy()
        {
            if (_redundantPathsPurger != null)
            {
                DeBruijnPathList redundantNodes;
                do
                {
                    redundantNodes = _redundantPathsPurger.DetectErroneousNodes(_graph);
                    _redundantPathsPurger.RemoveErroneousNodes(_graph, redundantNodes);
                } while (redundantNodes.Paths.Count > 0);
            }
        }

        /// <summary>
        /// Step 5: Build contigs from de bruijn graph.
        /// If coverage threshold is set, remove low coverage contigs.
        /// </summary>
        /// <returns>List of contig sequences</returns>
        protected IList<ISequence> BuildContigs()
        {
            if (_contigBuilder == null)
            {
                throw new InvalidOperationException(Properties.Resource.NullContigBuilder);
            }

            // Step 5.1: Remove low coverage contigs
            if (AllowLowCoverageContigRemoval && _contigCoverageThreshold > 0)
            {
                _lowCoverageContigPurger.RemoveLowCoverageContigs(_graph, _contigCoverageThreshold);
            }

            // Step 5.2: Build Contigs
            return _contigBuilder.Build(_graph);
        }

        /// <summary>
        /// Step 6: Build scaffolds from contig list and paired reads
        /// </summary>
        /// <param name="contigs">List of contigs</param>
        /// <returns>List of scaffold sequences</returns>
        protected IList<ISequence> BuildScaffolds(IList<ISequence> contigs)
        {
            if (_scaffoldBuilder == null)
            {
                // Scaffold Builder is a required module for this method. Set this to default.
                _scaffoldBuilder = new GraphScaffoldBuilder();
            }

            return _scaffoldBuilder.BuildScaffold(SequenceReads, contigs, KmerLength, depth: Depth, redundancy: ScaffoldRedundancy);
        }

        /// <summary>
        /// Validate input sequences
        /// </summary>
        /// <param name="sequences">Input sequences</param>
        private static void ValidateAllSequences(IEnumerable<ISequence> sequences)
        {
            if (sequences == null)
            {
                throw new ArgumentNullException("sequences");
            }

            if (sequences.AsParallel().Any(s => s == null))
            {
                throw new ArgumentException(Properties.Resource.InputSequenceCannotBeNull);
            }
        }

        /// <summary>
        /// Sets up fields for the assembly process.
        /// </summary>
        /// <param name="sequenceReads">List of sequence reads</param>
        private void Initialize(IList<ISequence> sequenceReads)
        {
            // Reset parameters not set by user, based on sequenceReads
            if (AllowKmerLengthEstimation)
            {
                _kmerLength = EstimateKmerLength(sequenceReads);
            }
            
            if (_kmerLength <= 0)
            {
                throw new InvalidOperationException(Properties.Resource.KmerLength);
            }

            if (!sequenceReads.AsParallel().All(seq => seq.Count >= _kmerLength))
            {
                throw new InvalidOperationException(Properties.Resource.InappropriateKmerLength);
            }
            
            if (_dangleThreshold == -1)
            {
                _dangleThreshold = _kmerLength + 1;
            }

            if (_redundantPathLengthThreshold == -1)
            {
                // Reference for default threshold for redundant path purger:
                // ABySS Release Notes 1.1.2 - "Pop bubbles shorter than N bp. The default is b=3*(k + 1)."
                _redundantPathLengthThreshold = 3 * (_kmerLength + 1);
            }

            InitializeDefaultGraphModifiers();
            RemoveAmbiguousReads(sequenceReads);
        }

        /// <summary>
        /// Initializes the above defined fields. For each step in assembly
        /// we use a seperate class for implementation. This method assigns 
        /// these variables to classes with desired implementation.
        /// </summary>
        private void InitializeDefaultGraphModifiers()
        {
            // Assign uninitialized fields to default values
            if (_danglingLinksPurger == null)
            {
                _danglingLinksPurger = new DanglingLinksPurger();
            }

            if (_redundantPathsPurger == null)
            {
                _redundantPathsPurger = new RedundantPathsPurger(_redundantPathLengthThreshold);
            }

            if (_lowCoverageContigPurger == null)
            {
                _lowCoverageContigPurger = new SimplePathContigBuilder();
            }
        }

        #region IDisposable Members
        /// <summary>
        /// Implements dispose to supress GC finalize
        /// This is done as one of the methods uses ReadWriterLockSlim
        /// which extends IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose field instances
        /// </summary>
        /// <param name="disposeManaged">If disposeManaged equals true, clean all resources</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                if (_graph != null)
                {
                    _graph.Dispose();
                }

                if (_scaffoldBuilder != null)
                {
                    _scaffoldBuilder.Dispose();
                }

                _graph = null;
                _sequenceReads = null;
                _danglingLinksPurger = null;
                _redundantPathsPurger = null;
                _contigBuilder = null;
                _scaffoldBuilder = null;
            }
        }

        #endregion
    }
}
