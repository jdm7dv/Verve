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
using MBF.Algorithms.Assembly.PaDeNA.Properties;

namespace MBF.Algorithms.Assembly.PaDeNA.Scaffold
{
    /// <summary>
    /// Generates Scaffolds using Graph.
    /// Algorithm:
    /// Step1: Generate contig overlap graph. 
    /// Step2: Map Reads to contigs.
    /// Step3: Generate Contig Mate Pair Map.
    /// Step4: Filter Paired Reads.
    /// Step5: Distance Orientation.
    /// Step6: Trace Scaffold Paths.
    /// Step7: Assemble paths.
    /// Step8: Generate sequence of scaffolds.
    /// </summary>
    public class GraphScaffoldBuilder : IGraphScaffoldBuilder
    {
        #region Fields

        /// <summary>
        /// Number of paired read required to connect two contigs.
        /// </summary>
        private int _redundancy;

        /// <summary>
        /// Depth for graph traversal.
        /// </summary>
        private int _depth;

        /// <summary>
        /// kmer length.
        /// </summary>
        private int _kmerLength;

        /// <summary>
        /// Mapping reads to mate pairs.
        /// </summary>
        private IMatePairMapper _mapPairedReads;

        /// <summary>
        /// Mapping reads to contigs.
        /// </summary>
        private IReadContigMapper _readContigMap;

        /// <summary>
        /// Filtering of mate pairs based on orientation of contigs.
        /// </summary>
        private IOrientationBasedMatePairFilter _pairedReadFilter;

        /// <summary>
        ///Calculation of distance between contigs using mate pairs.
        /// </summary>
        private IDistanceCalculator _distanceCalculator;

        /// <summary>
        /// Traversal of contig overlap graph.
        /// </summary>
        private ITracePath _tracePath;

        /// <summary>
        /// Removal of containing paths and removal of overlapping paths.
        /// </summary>
        private IPathPurger _pathAssembler;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the GraphScaffoldBuilder class. 
        /// </summary>
        public GraphScaffoldBuilder()
        {
            _mapPairedReads = new MatePairMapper();
            _readContigMap = new ReadContigMapper();
            _pairedReadFilter = new OrientationBasedMatePairFilter();
            _distanceCalculator = new DistanceCalculator();
            _tracePath = new TracePath();
            _pathAssembler = new PathPurger();

            //Hierarchical Scaffolding With Bambus
            //by: Mihai Pop, Daniel S. Kosack, Steven L. Salzberg
            //Genome Research, Vol. 14, No. 1. (January 2004), pp. 149-159.
            _redundancy = 2;

            //Memory and performance optimization.
            _depth = 10;
        }

        /// <summary>
        /// Initializes a new instance of the GraphScaffoldBuilder class.
        /// </summary>
        /// <param name="mapPairedReads">Mapping reads to mate pairs.</param>
        /// <param name="readContigMap"> Mapping reads to contigs.</param>
        /// <param name="pairedReadFilter">Filtering of mate pairs.</param>
        /// <param name="distanceCalculator">Calculation of distance between 
        /// contigs using mate pairs.</param>
        /// <param name="tracePath">Traversal of contig overlap graph.</param>
        /// <param name="pathAssembler">Removal of containing paths and removal of overlapping paths.</param>
        public GraphScaffoldBuilder(
            IMatePairMapper mapPairedReads,
            IReadContigMapper readContigMap,
            IOrientationBasedMatePairFilter pairedReadFilter,
            IDistanceCalculator distanceCalculator,
            ITracePath tracePath,
            IPathPurger pathAssembler)
        {
            if (null != mapPairedReads)
            {
                _mapPairedReads = mapPairedReads;
            }
            else
            {
                _mapPairedReads = new MatePairMapper();
            }

            if (null != readContigMap)
            {
                _readContigMap = readContigMap;
            }
            else
            {
                _readContigMap = new ReadContigMapper();
            }

            if (null != pairedReadFilter)
            {
                _pairedReadFilter = pairedReadFilter;
            }
            else
            {
                _pairedReadFilter = new OrientationBasedMatePairFilter();
            }

            if (null != distanceCalculator)
            {
                _distanceCalculator = distanceCalculator;
            }
            else
            {
                _distanceCalculator = new DistanceCalculator();
            }

            if (null != tracePath)
            {
                _tracePath = tracePath;
            }
            else
            {
                _tracePath = new TracePath();
            }

            if (null != pathAssembler)
            {
                _pathAssembler = pathAssembler;
            }
            else
            {
                _pathAssembler = new PathPurger();
            }

            //Hierarchical Scaffolding With Bambus
            //by: Mihai Pop, Daniel S. Kosack, Steven L. Salzberg
            //Genome Research, Vol. 14, No. 1. (January 2004), pp. 149-159.
            _redundancy = 2;

            //Memory and performance optimization.
            _depth = 10;
        }

        #endregion

        #region Members of IGraphScaffoldBuilder

        /// <summary>
        /// Builds scaffolds from list of reads and contigs
        /// </summary>
        /// <param name="reads">List of reads</param>
        /// <param name="contigs">List of contigs</param>
        /// <param name="kmerLength">Kmer Length</param>
        /// <param name="depth">Depth for graph traversal</param>
        /// <param name="redundancy">Number of mate pairs required to create a link between two contigs.
        ///  Hierarchical Scaffolding With Bambus
        ///  by: Mihai Pop, Daniel S. Kosack, Steven L. Salzberg
        ///  Genome Research, Vol. 14, No. 1. (January 2004), pp. 149-159.</param>
        /// <returns>List of scaffold sequences</returns>
        public IList<ISequence> BuildScaffold(
            IList<ISequence> reads,
            IList<ISequence> contigs,
            int kmerLength,
            int depth = 10,
            int redundancy = 2)
        {
            if (contigs == null)
            {
                throw new ArgumentNullException("contigs");
            }

            if (null == reads)
            {
                throw new ArgumentNullException("reads");
            }

            if (kmerLength <= 0)
            {
                throw new ArgumentException(Properties.Resource.KmerLength);
            }

            if (_depth <= 0)
            {
                throw new ArgumentException(Resource.Depth);
            }

            if (redundancy < 0)
            {
                throw new ArgumentException(Resource.NegativeRedundancy);
            }

            _depth = depth;
            _redundancy = redundancy;
            _kmerLength = kmerLength;
               
            //Step1: Generate contig overlap graph.
            DeBruijnGraph contigGraph = GenerateContigOverlapGraph(contigs);
            IEnumerable<DeBruijnNode> nodes = contigGraph.Nodes.Where(t => t.ExtensionsCount == 0);
            foreach (DeBruijnNode node in nodes)
            {
                contigs.Remove(contigGraph.GetNodeSequence(node));
            }

            // Step2: Map Reads to contigs.
            ReadContigMap readContigMap = ReadContigMap(contigs, reads);
            contigs = null;

            // Step3: Generate Contig Mate Pair Map.
            ContigMatePairs contigMatePairs = MapPairedReadsToContigs(readContigMap, reads);
            readContigMap = null;

            // Step4: Filter Paired Reads.
            contigMatePairs = FilterReadsBasedOnOrientation(contigMatePairs);

            // Step5: Distance Calculation.
            CalculateDistanceBetweenContigs(contigMatePairs);

            // Step6: Trace Scaffold Paths.
            IList<ScaffoldPath> paths = TracePath(contigGraph, contigMatePairs);
            contigMatePairs = null;

            // Step7: Assemble paths.
            PathPurger(paths);

            // Step8: Generate sequence of scaffolds.
            return GenerateScaffold(contigGraph, paths);
        }

        #endregion

        #region Private members

        /// <summary>
        /// Generate contig overlap graph.
        /// </summary>
        /// <param name="contigs">List of contig sequences.</param>
        /// <returns>Contig Graph.</returns>
        protected DeBruijnGraph GenerateContigOverlapGraph(IList<ISequence> contigs)
        {
            if (contigs == null)
            {
                throw new ArgumentNullException("contigs");
            }

            DeBruijnGraph contigGraph = new DeBruijnGraph();
            contigGraph.BuildContigGraph(contigs, _kmerLength);
            return contigGraph;
        }

        /// <summary>
        /// Map reads to contigs.
        /// </summary>
        /// <param name="contigs">List of sequences of contigs.</param>
        /// <param name="reads">List of sequences of reads.</param>
        /// <returns>Map of reads and contigs.</returns>
        protected ReadContigMap ReadContigMap(IList<ISequence> contigs, IList<ISequence> reads)
        {
            return _readContigMap.Map(contigs, reads, _kmerLength);
        }

        /// <summary>
        /// Map paired reads to contigs using FASTA sequence header.
        /// </summary>
        /// <param name="readContigMap">Map between reads and contigs</param>
        /// <param name="reads">Sequences of reads.</param>
        /// <returns>Contig Mate Pair map.</returns>
        protected ContigMatePairs MapPairedReadsToContigs(ReadContigMap readContigMap, IList<ISequence> reads)
        {
            ContigMatePairs contigMatePairs = new ContigMatePairs();
            contigMatePairs = _mapPairedReads.MapContigToMatePairs(reads, readContigMap);
            return contigMatePairs;
        }

        /// <summary>
        /// Filter reads based on orientation of contigs.
        /// </summary>
        /// <param name="contigMatePairs">Contig Mate Pair map.</param>
        /// <returns>Contig Mate Pair map.</returns>
        protected ContigMatePairs FilterReadsBasedOnOrientation(ContigMatePairs contigMatePairs)
        {
            return _pairedReadFilter.FilterPairedReads(contigMatePairs, _redundancy);
        }

        /// <summary>
        /// Calculate distance between contigs using paired reads.
        /// </summary>
        /// <param name="contigMatePairs">Contig Mate Pair map.</param>
        /// <returns>Number of contig-read pairs</returns>
        protected int CalculateDistanceBetweenContigs(ContigMatePairs contigMatePairs)
        {
            if (contigMatePairs == null)
            {
                throw new ArgumentNullException("contigMatePairs");
            }

            _distanceCalculator.CalculateDistance(contigMatePairs);
            // this dictionary is updated in this step
            return contigMatePairs.Count;
        }

        /// <summary>
        /// Performs Breadth First Search in contig overlap graph.
        /// </summary>
        /// <param name="contigGraph">Contig Graph.</param>
        /// <param name="contigMatePairs">Contig Mate Pair map.</param>
        /// <returns>List of Scaffold Paths</returns>
        protected IList<ScaffoldPath> TracePath(DeBruijnGraph contigGraph, ContigMatePairs contigMatePairs)
        {
            return _tracePath.FindPaths(contigGraph, contigMatePairs, _kmerLength, _depth);
        }

        /// <summary>
        /// Remove containing and overlapping paths.
        /// </summary>
        /// <param name="paths">List of Scaffold Paths</param>
        /// <returns>Number of final paths</returns>
        protected int PathPurger(IList<ScaffoldPath> paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }

            _pathAssembler.PurgePath(paths);
            return paths.Count;
        }

        /// <summary>
        /// Generate sequences from list of contig nodes.
        /// </summary>
        /// <param name="contigGraph">Contig Overlap Graph.</param>
        /// <param name="paths">Scaffold paths.</param>
        /// <returns>List of sequences of scaffolds.</returns>
        protected IList<ISequence> GenerateScaffold(
            DeBruijnGraph contigGraph,
            IList<ScaffoldPath> paths)
        {
            if (contigGraph == null)
            {
                throw new ArgumentNullException("contigGraph");
            }

            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }

            List<ISequence> scaffolds = paths.AsParallel().Select(t => t.BuildSequenceFromPath(contigGraph, _kmerLength)).ToList();
            IEnumerable<DeBruijnNode> visitedNodes = contigGraph.Nodes.AsParallel().Where(t => !t.IsMarked());
            scaffolds.AddRange(visitedNodes.AsParallel().Select(t => contigGraph.GetNodeSequence(t)));
            contigGraph.Dispose();
            return scaffolds;
        }

        #endregion

        #region IDisposable Members
        /// <summary>
        /// Implements dispose to supress GC finalize
        /// This is done as this class creates an instance 
        /// of DeBruijnGraph which extends IDisposable.
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
                _distanceCalculator = null;
                _mapPairedReads = null;
                _pairedReadFilter = null;
                _pathAssembler = null;
                _readContigMap = null;
                _tracePath = null;
            }
        }

        #endregion
    }
}
