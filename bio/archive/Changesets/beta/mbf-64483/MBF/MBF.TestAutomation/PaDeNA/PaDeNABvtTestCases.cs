// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * PaDeNABvtTestCases.cs
 * 
 *  This file contains the PaDeNA Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using MBF.Algorithms.Assembly;
using MBF.Algorithms.Assembly.Graph;
using MBF.Algorithms.Assembly.PaDeNA;
using MBF.Algorithms.Assembly.PaDeNA.Scaffold;
using MBF.Algorithms.Kmer;
using MBF.IO.Fasta;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.Algorithms.Assembly.PaDeNA
{
    /// <summary>
    /// The class contains Bvt test cases to confirm PaDeNA assembler.
    /// </summary>
    [TestClass]
    public class PaDeNABvtTestCases
    {


        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static PaDeNABvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region PaDeNAStep1TestCases

        /// <summary>
        /// Validate ParallelDeNovoAssembler is building valid kmers 
        /// using 4 one line reads in a fasta file and kmerLength 4
        /// Input : 4 one line input reads from dna base sequence and kmerLength 4
        /// Output : kmers sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep1BuildKmers()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateDe2AssemblerBuildKmers(Constants.OneLineReadsNode);
            }
        }

        /// <summary>
        /// Validate SequenceRangeToKmerBuilder Build(lstsequences,kmerLength) 
        /// is building valid kmers using one line 4 reads in a fasta file and kmerLength 4
        /// Input : 4 one line input reads from dna base sequence and kmerLength 4
        /// Output : kmers sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep1KmerBuilderBuild()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateKmerBuilderBuild(Constants.OneLineReadsNode);
            }
        }

        /// <summary>
        /// Validate SequenceRangeToKmerBuilder Build(sequence,kmerLength) is 
        /// building valid kmers using one line sequence in a fasta file and kmerLength 4
        /// Input : 4 one line input reads from dna base sequence and kmerLength 4
        /// Output : kmers sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep1KmerBuilderBuildWithSequence()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateKmerBuilderBuildWithSequence(Constants.OneLineReadsNode);
            }
        }

        /// <summary>
        /// Validate KmersOfSequence ctor (sequence, length, set of kmers) 
        /// by passing small size chromsome sequence and kmer length 28
        /// after building kmers
        /// Input : Build kmeres from 4000 input reads of small size 
        /// chromosome sequence and kmerLength 28
        /// Output : kmers of sequence object with build kmers
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep1KmersOfSequenceCtorWithBuildKmers()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateKmersOfSequenceCtorWithBuildKmers(Constants.SmallChromosomeReadsNode);
            }
        }

        /// <summary>
        /// Validate KmersOfSequence ctor (sequence, length) by passing 
        /// one line sequences, kmerLength 4 
        /// and populate kmers after building it. 
        /// Input : Build kmeres from 4 input reads of one line sequence
        /// and kmerLength 28
        /// Output : kmers of sequence object with build kmers
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep1KmersOfSequenceCtor()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateKmersOfSequenceCtor(Constants.OneLineReadsNode);
            }
        }

        /// <summary>
        /// Validate KmersOfSequence ToSequences() method using one line reads
        /// Input: Build kmers using 4 reads of one line sequence and kmerLength 4
        /// Ouput: kmers sequences
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep1KmersOfSequenceToSequences()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateKmersOfSequenceToSequences(Constants.OneLineReadsNode);
            }
        }

        /// <summary>
        /// Validate KmersOfSequence GetKmerSequence() method after populating kmers 
        /// using 4 reads from one line sequences and kmerLength 4
        /// Input: Build kmers using 4 reads of one line sequence and kmerLength 4
        /// Ouput: kmers sequences
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep1KmersOfSequenceGetKmers()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateKmersOfSequenceCtor(Constants.OneLineReadsNode);
            }
        }

        #endregion

        #region PaDeNAStep2TestCases

        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with one line reads and kmerLength 4
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep2BuildGraph()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateDe2AssemblerBuildGraph(Constants.OneLineStep2GraphNode);
            }
        }

        /// <summary>
        /// Validate Graph after building graph with DeBruijnGraph.Build()
        /// with kmers
        /// Input : Kmers
        /// Output: Graph
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep2DeBruijnGraph()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateDeBruijnGraphBuild(Constants.OneLineStep2GraphNode);
            }
        }

        /// <summary>
        /// Validate the DeBruijnNode ctor by passing kmer, kmerLength and graph object
        /// Input: kmer
        /// Output: DeBruijn Node
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep2DeBruijnNode()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateDeBruijnNodeCtor(Constants.OneLineStep2GraphNode);
            }
        }

        /// <summary>
        /// Create dbruijn node by passing kmer and create another node.
        /// Add new node as leftendextension of first node. Validate the 
        /// AddLeftEndExtension() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep2DeBruijnNodeAddLeftExtension()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateDeBruijnNodeAddLeftExtension(Constants.OneLineStep2GraphNode);
            }
        }

        /// <summary>
        /// Create dbruijn node by passing kmer and create another node.
        /// Add new node as leftendextension of first node. Validate the 
        /// AddRightEndExtension() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep2DeBruijnNodeAddRightExtension()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateDeBruijnNodeAddRightExtension(Constants.OneLineStep2GraphNode);
            }
        }

        #endregion

        #region PaDeNAStep3TestCases

        /// <summary>
        /// Validate the PaDeNA step3 
        /// which removes dangling links from the graph
        /// Input: Graph with dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep3UndangleGraph()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateDe2AssemblerUnDangleGraph(Constants.OneLineStep3GraphNode);
            }
        }

        /// <summary>
        /// Validate the DanglingLinksPurger DetectErrorNodes() method 
        /// is identying the dangling nodes as expected
        /// Input: Graph with dangling links
        /// Output: dangling nodes
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep3DetectErrorNodes()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidatePaDeNADetectErrorNodes(Constants.OneLineStep3GraphNode);
            }
        }

        /// <summary>
        /// Validate the DanglingLinksPurger is removing the dangling link nodes
        /// from the graph
        /// Input: Graph and dangling node
        /// Output: Graph without any dangling nodes
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep3RemoveErrorNodes()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidatePaDeNARemoveErrorNodes(Constants.OneLineStep3GraphNode);
            }
        }
        #endregion

        #region PaDeNAStep4TestCases

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovoAssembler.RemoveRedundancy() by passing graph 
        /// using one line reads such that it will create bubbles in the graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep4RemoveRedundancy()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateDe2AssemblerRemoveRedundancy(
                    Constants.OneLineStep4ReadsAfterRemoveRedundancy);
            }
        }

        /// <summary>
        /// Validate PaDeNA step4 Simp.RemoveRedundancy() by passing graph 
        /// using one line reads such that it will create bubbles in the graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep4RedundantPathPurgerCtor()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateRedundantPathPurgerCtor(Constants.OneLineStep4ReadsAfterErrorRemove);
            }
        }

        /// <summary>
        /// Validate PaDeNA DetectErrorNodes() by passing graph with bubbles
        /// Input : Graph with bubbles
        /// Output: Nodes list
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep4DetectErrorNodes()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateRedundantPathPurgerCtor(Constants.OneLineStep4ReadsAfterErrorRemove);
            }
        }

        /// <summary>
        /// Validate PaDeNA RemoveErrorNodes() by passing redundant nodes list and graph
        /// Input : graph and redundant nodes list
        /// Output: Graph without bubbles
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep4RemoveErrorNodes()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateRedundantPathPurgerCtor(Constants.OneLineStep4ReadsAfterErrorRemove);
            }
        }

        #endregion

        #region PaDeNAStep5TestCases

        /// <summary>
        /// Validate PaDeNA step5 by passing graph and validating the contigs
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep5BuildContigs()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateDe2AssemblerBuildContigs(Constants.OneLineStep5ReadsNode);
            }
        }

        /// <summary>
        /// Validate PaDeNA step5 SimpleContigBuilder.BuildContigs() by passing graph 
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep5SimpleContigBuilderBuildContigs()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateSimpleContigBuilderBuild(Constants.OneLineStep5ReadsNode);
            }
        }

        #endregion

        #region PaDeNAStep6:Step1:TestCases

        /// <summary>
        /// Validate paired reads for X1, Y1 format map reads.
        /// Input : X1,Y1 format map reads.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6X1Y1FormatPairedReads()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidatePairedReads(Constants.X1AndY1PairedReadsNode);
            }
        }

        /// <summary>
        /// Validate paired reads for 1 and 2 format map reads.
        /// Input : 1,2 format map reads.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6OneAndTwoFormatPairedReads()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidatePairedReads(Constants.OneAndTwoPairedReadsNode);
            }
        }

        /// <summary>
        /// Validate paired reads for F and R format map reads.
        /// Input : F,R format map reads.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6FAndRFormatPairedReads()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidatePairedReads(Constants.FAndRPairedReadsNode);
            }
        }

        /// <summary>
        /// Validate Adding new library information to library list.
        /// Input : Library name,Standard deviation and mean length.
        /// Output : Validate forward and reverse reads.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6Libraryinformation()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.AddLibraryInformation(Constants.X1AndY1FormatPairedReadAddLibrary);
            }
        }

        #endregion PaDeNAStep6:Step1:TestCases

        #region PaDeNAStep6:Step2:TestCases

        /// <summary>
        /// Validate MapReads to contigs for One line reads.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6MapReadsToContigs()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateMapReadsToContig(Constants.MapReadsToContigFullOverlapNode, true);
            }
        }

        /// <summary>
        /// Validate MapReads to contigs for One line reads for Partial overlap.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6MapReadsToContigsForPartialOverlap()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateMapReadsToContig(Constants.MapReadsToContigPartialOverlapNode, false);
            }
        }

        #endregion PaDeNAStep6:Step2:TestCases

        #region PaDeNAStep6:Step3:TestCases

        /// <summary>
        /// Validate contig graph for Sequence reads with 3-4 Line.
        /// Input : 3-4 Line sequence reads.
        /// Output : Contig Graph
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6ContigBuildGraphForSequenceReads()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateContigGraph(Constants.ContigGraphForSmallReadsNode);
            }
        }

        /// <summary>
        /// Validate ContigBuildGraph properties.
        /// Input : 3-4 Line sequence reads.
        /// Output : Contig Graph
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6ContigBuildGraphProperties()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateContigGraph(Constants.ContigGraphForSmallReadsNode);
            }
        }

        /// <summary>
        /// Validate contig graph for ClustalW contigs
        /// Input : ClustalW contigs
        /// Output : Contig Graph
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6ContigBuildGraphForClustalW()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateContigGraph(Constants.ContigGraphWithClusterContigsNode);
            }
        }

        #endregion PaDeNAStep6:Step3:TestCases

        #region PaDeNAStep6:Step4:TestCases

        /// <summary>
        /// Validate filter Contig Pairs formed in Forward direction 
        /// with all paired reads supports orientation.
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6FilterPairedReadsForForwardOrientation()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateFilterPaired(Constants.FilterPairedReadContigsNode);
            }
        }

        /// <summary>
        /// Validate filter Contig Pairs formed in Forward direction 
        /// using FilterPairedReads(redundancy)
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6FilterPairedReadsUsingRedundancy()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateFilterPaired(Constants.FilterPairedReadContigsUsingRedundancy);
            }
        }

        /// <summary>
        /// Validate filter Contig Pairs formed in Reverse direction with 
        /// all paired reads support orientation.
        /// with all paired reads support orientation.
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6FilterPairedReadsForReverseOrientation()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateFilterPaired(Constants.FilterPairedReadReverseOrietnationContigsNode);
            }
        }

        #endregion PaDeNAStep6:Step4:TestCases

        #region PaDeNAStep6:Step5:TestCases

        /// <summary>
        /// Calculate distance between Contig Pairs formed in Forward
        /// direction with all paired reads support orientation.
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6CalculateDistanceForForwardPairedContigs()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateContigDistance(Constants.FilterPairedReadContigsNode);
            }
        }

        /// <summary>
        /// Calculate distance between Contig Pairs formed in Reverse
        /// direction with all paired reads support orientation.
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6CalculateDistanceForReversePairedContigs()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateContigDistance(Constants.FilterPairedReadReverseOrietnationContigsNode);
            }
        }

        #endregion PaDeNAStep6:Step5:TestCases

        #region PaDeNAStep6:Step6:TestCases

        /// <summary>
        /// Validate scaffold path for Contig Pairs formed in Forward 
        /// direction with all paired reads support orientation
        /// Input : 3-4 Line sequence reads.
        /// Output : Scaffold path 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6ScaffoldPathForForwardOrientation()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateScaffoldPath(Constants.ScaffoldPathWithOverlapNode);
            }
        }

        /// <summary>
        /// Validate scaffold path for Contig Pairs formed in Reverse 
        /// direction with all paired reads support orientation
        /// Input : 3-4 Line sequence reads.
        /// Output : Scaffold path 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6ScaffoldPathForReverseOrientation()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateScaffoldPath(Constants.ScaffoldPathWithOverlapReverseNode);
            }
        }

        #endregion PaDeNAStep6:Step5:TestCases

        #region PaDeNAStep6:Step7:TestCases

        /// <summary>
        /// Validate merging assembled path for scaffold paths 
        /// which have overalap.
        /// Input : 3-4 Line sequence reads.
        /// Output : Assembled path 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6AssembledPathWithOverlapContigs()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateAssembledPath(Constants.AssembledPathWithOverlapNode);
            }
        }

        /// <summary>
        /// Validate merging assembled path for scaffold paths which 
        /// have partial overlaps.
        /// Input : 3-4 Line sequence reads.
        /// Output : Assembled path 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6AssembledPathWithPartialOverlapContigs()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidateAssembledPath(Constants.AssembledPathWithoutOverlap);
            }
        }

        /// <summary>
        /// Validate Assembled sequences for one line reads.
        /// Input : 3-4 Line sequence reads.
        /// Output : Assembled sequences.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAStep6AssembledSequencesWithSmallReads()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidatePaDeNAAssembledSeqs(
                    Constants.AssembledSequencesForSequenceReadsNode, true, false,
                    false);
            }
        }

        /// <summary>
        /// Validate ParallelDenovoAssembler class properties.
        /// Input : Sequence reads.
        /// Output : Validate ParallerlDenovoAssembler properties.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateParallelDenovoAssemblerProperties()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ParallelDenovoAssemblyProperties(
                    Constants.AssembledSequencesForSequenceReadsNode);
            }
        }

        /// <summary>
        /// Validate sequence contigs for one line reads.
        /// Input : 3-4 Line sequence reads.
        /// Output : Sequence contigs.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNASequenceContigsWithSmallReads()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidatePaDeNAAssembledSeqs(
                    Constants.AssembledContigsForSequenceReadsNode,
                    false, false, false);
            }
        }

        /// <summary>
        /// Validate sequence contigs for one line reads with Erosion enabled.
        /// Input : 3-4 Line sequence reads.
        /// Output : Sequence contigs.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAAssembledSeqsForSmallReadsWithErosion()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidatePaDeNAAssembledSeqs(
                    Constants.AssembledSequencesForSequenceReadsWithErosionNode,
                    true, false, true);
            }
        }

        /// <summary>
        /// Validate sequence contigs for one line reads with 
        /// Low coverage contig enabled.
        /// Input : 3-4 Line sequence reads.
        /// Output : Sequence contigs.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAAssembledSeqsForSmallReadsWithLowCoverageContig()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidatePaDeNAAssembledSeqs(
                    Constants.AssembledSequencesForSequenceReadsWithLCCNode,
                    true, true, false);
            }
        }

        /// <summary>
        /// Validate sequence contigs for one line reads with 
        /// Erosion and Low coverage contig enabled.
        /// Input : 3-4 Line sequence reads.
        /// Output : Sequence contigs.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidatePaDeNAAssembledSeqsWithErosionAndLowCoverageContig()
        {
            using (PaDeNABvtTest testObj = new PaDeNA.PaDeNABvtTest())
            {
                testObj.ValidatePaDeNAAssembledSeqs(
                    Constants.AssembledSequencesForSequenceReadsWithErosionAndLCCNode,
                    true, true, true);
            }
        }

        #endregion PaDeNAStep6:Step5:TestCases
    }

    /// <summary>
    /// The class contains helper methods for PaDeNA assembler.
    /// </summary>
    internal class PaDeNABvtTest : ParallelDeNovoAssembler
    {
        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\PaDeNATestData\PaDeNATestsConfig.xml");

        #endregion Global Variables

        #region HelperMethods

        /// <summary>
        /// Validate ParallelDeNovoAssembler step1 Build kmers 
        /// </summary>
        /// <param name="nodeName">xml node for test data</param>
        internal void ValidateDe2AssemblerBuildKmers(string nodeName)
        {
            // Read all the input sequences from xml config file
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

            // set kmerLength
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // set all the input reads and execute build kmers
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IEnumerable<KmersOfSequence> lstKmers =
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength);

            ValidateKmersList(new List<KmersOfSequence>(lstKmers), sequenceReads, nodeName);

            Console.WriteLine(
                @"PaDeNA BVT : Validation of Build with all input reads using 
                    ParallelDeNovoAssembler completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT : Validation of Build with all input reads using 
                    ParallelDeNovoAssembler sequence completed successfully");
        }

        /// <summary>
        /// Validate SequenceRangeToKmerBuilder Build() method which build kmers
        /// </summary>
        /// <param name="nodeName">xml node name for test data</param>
        internal void ValidateKmerBuilderBuild(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.KmerLengthNode);

            // Get the input reads
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Pass all the input reads and kmerLength to generate kmers
            SequenceToKmerBuilder builder = new SequenceToKmerBuilder();
            IEnumerable<KmersOfSequence> lstKmers = builder.Build(sequenceReads,
              int.Parse(kmerLength, (IFormatProvider)null));

            // Validate kmers list
            ValidateKmersList(new List<KmersOfSequence>(lstKmers), sequenceReads, nodeName);

            Console.WriteLine(
                @"PaDeNA BVT : Validation of Build with all input reads 
                    using ParallelDeNovoAssembler completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT : Validation of Build with all input reads 
                    sequence completed successfully");
        }

        /// <summary>
        /// Validate SequenceRangeToKmerBuilder Build() which build kmers 
        /// using one base sequence 
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateKmerBuilderBuildWithSequence(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.KmerLengthNode);

            // Get the input reads
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Pass each input read and kmerLength
            // Add all the generated kmers to kmer list
            SequenceToKmerBuilder builder = new SequenceToKmerBuilder();
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>();
            foreach (ISequence sequence in sequenceReads)
            {
                lstKmers.Add(builder.Build(sequence, int.Parse(kmerLength, (IFormatProvider)null)));
            }

            // Validate all the kmers
            ValidateKmersList(lstKmers, sequenceReads, nodeName);

            Console.WriteLine(
                @"PaDeNA BVT : Validation of Build with each input read sequence 
                    completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT : Validation of Build with each input read sequence 
                    completed successfully");
        }

        /// <summary>
        /// Validate the generated kmers using expected output kmer file
        /// </summary>
        /// <param name="lstKmers">generated kmers</param>
        /// <param name="inputReads">input base sequence reads</param>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateKmersList(IList<KmersOfSequence> lstKmers,
            IList<ISequence> inputReads, string nodeName)
        {
            string kmerOutputFile = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.KmersOutputFileNode);
            Assert.AreEqual(inputReads.Count, lstKmers.Count);

            // Get the array of kmer sequence using kmer positions
            string[] aryKmer = new string[lstKmers.Count];
            for (int kmerIndex = 0; kmerIndex < lstKmers.Count; kmerIndex++)
            {
                HashSet<KmersOfSequence.KmerPositions> kmers = lstKmers[kmerIndex].Kmers;
                StringBuilder strKmers = new StringBuilder();
                int ikmer = 0;
                foreach (KmersOfSequence.KmerPositions kmer in kmers)
                {
                    ISequence sequence = lstKmers[kmerIndex].KmerToSequence(kmer);
                    if (0 != ikmer)
                    {
                        strKmers.Append(",");
                    }
                    strKmers.Append(sequence.ToString());
                    ikmer++;
                }
                aryKmer[kmerIndex] = strKmers.ToString();
                Assert.AreEqual(inputReads[kmerIndex], lstKmers[kmerIndex].BaseSequence);
            }

            // Validate all the generated kmer sequence with the expected kmer sequence
            using (StreamReader kmerFile = new StreamReader(kmerOutputFile))
            {
                int count = 0;
                string line = string.Empty;
                while (null != (line = kmerFile.ReadLine()))
                {
                    Assert.AreEqual(line, aryKmer[count]);
                    count++;
                }
            }
        }

        /// <summary>
        /// Validate kmersofsequence ctor() by passing kmers, kmer length and input reads
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateKmersOfSequenceCtor(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Validate the KmersOfSequence ctor by passing build kmers
            // Validate the kmersof sequence instance using GetKmerSequence()
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IEnumerable<KmersOfSequence> lstKmers =
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength);
            IList<KmersOfSequence> newKmersList = new List<KmersOfSequence>();
            int index = 0;
            foreach (KmersOfSequence kmer in lstKmers)
            {
                KmersOfSequence newkmer = new KmersOfSequence(sequenceReads[index],
                  int.Parse(kmerLength, (IFormatProvider)null), kmer.Kmers);
                newKmersList.Add(newkmer);
                index++;
            }

            ValidateKmersList(newKmersList, sequenceReads, nodeName);

            Console.WriteLine(
                @"PaDeNA BVT : KmersOfSequence ctor validation for 
                    PaDeNA step1 completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT : KmersOfSequence ctor validation for 
                    PaDeNA step1 completed successfully");
        }

        /// <summary>
        /// Validate KmersOfSequence ctor by passing base sequence reads, kmer length and
        /// built kmers
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        internal void ValidateKmersOfSequenceCtorWithBuildKmers(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

            // Get the input reads
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }
            SequenceToKmerBuilder builder = new SequenceToKmerBuilder();
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>();

            // Validate KmersOfSequence ctor using build kmers
            foreach (ISequence sequence in sequenceReads)
            {
                KmersOfSequence kmer = builder.Build(sequence, int.Parse(kmerLength, (IFormatProvider)null));
                KmersOfSequence kmerSequence = new KmersOfSequence(sequence,
                  int.Parse(kmerLength, (IFormatProvider)null), kmer.Kmers);
                lstKmers.Add(kmerSequence);
            }

            ValidateKmersList(lstKmers, sequenceReads, nodeName);

            Console.WriteLine(
                @"PaDeNA BVT : KmersOfSequence ctor with build 
                    kmers validation completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT : KmersOfSequence ctor with build 
                    kmers method validation completed successfully");
        }

        /// <summary>
        /// Validate KmersOfSequence ToSequences() method which returns kmers sequence
        /// using its positions
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateKmersOfSequenceToSequences(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength));

            // Get the array of kmer sequence using ToSequence()
            int index = 0;
            string[] aryKmerSequence = new string[sequenceReads.Count];
            foreach (ISequence sequenceRead in sequenceReads)
            {
                KmersOfSequence kmerSequence = new KmersOfSequence(sequenceRead,
                  int.Parse(kmerLength, (IFormatProvider)null), lstKmers[index].Kmers);
                IList<ISequence> sequences = kmerSequence.KmersToSequences();
                StringBuilder strSequence = new StringBuilder();
                int iseq = 0;
                foreach (ISequence sequence in sequences)
                {
                    if (0 != iseq)
                    {
                        strSequence.Append(",");
                    }
                    strSequence.Append(sequence);
                    iseq++;
                }
                aryKmerSequence[index] = strSequence.ToString();
                index++;
            }

            // Validate the generated kmer sequence with the expected output
            string kmerOutputFile = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmersOutputFileNode);
            using (StreamReader kmerFile = new StreamReader(kmerOutputFile))
            {
                int count = 0;
                string line = string.Empty;
                while (null != (line = kmerFile.ReadLine()))
                {
                    Assert.AreEqual(line, aryKmerSequence[count]);
                    count++;
                }
            }

            Console.WriteLine(
                @"PaDeNA BVT : KmersOfSequence ToSequences() 
                    method validation completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT : KmersOfSequence ToSequences() method 
                    validation completed successfully");
        }

        /// <summary>
        /// Validate graph generated using ParallelDeNovoAssembler.CreateGraph() with kmers
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDe2AssemblerBuildGraph(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;
            ValidateGraph(graph, nodeName);

            Console.WriteLine(
                @"PaDeNA BVT : ParallelDeNovoAssembler CreateGraph() 
                    validation for PaDeNA step2 completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT : ParallelDeNovoAssembler CreateGraph() validation 
                    for PaDeNA step2 completed successfully");
        }

        /// <summary>
        /// Validate graph generated using DeBruijnGraph.CreateGraph() with kmers
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnGraphBuild(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            using (DeBruijnGraph graph = new DeBruijnGraph())
            {
                graph.Build(this.SequenceReads, this.KmerLength);
                ValidateGraph(graph, nodeName);
            }
            Console.WriteLine(
                @"PaDeNA BVT : DeBruijnGraph Build() validation 
                    for PaDeNA step2 completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT : DeBruijnGraph Build() validation 
                    for PaDeNA step2 completed successfully");

        }

        /// <summary>
        /// Validate the graph nodes sequence, left edges and right edges
        /// </summary>
        /// <param name="graph">graph object</param>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateGraph(DeBruijnGraph graph, string nodeName)
        {
            string nodesSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.NodesSequenceNode);
            string nodesLeftEdges = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.NodesLeftEdgesCountNode);
            string nodesRightEdges = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.NodeRightEdgesCountNode);

            string[] leftEdgesCount = nodesLeftEdges.Split(',');
            string[] rightEdgesCount = nodesRightEdges.Split(',');
            string[] nodesSequences = nodesSequence.Split(',');

            // Validate the nodes 
            for (int iseq = 0; iseq < nodesSequences.Length; iseq++)
            {
                DeBruijnNode dbnodes = graph.Nodes.Where(n =>
                  graph.GetNodeSequence(n).ToString() == nodesSequences[iseq]
                  || graph.GetNodeSequence(n).ReverseComplement.ToString() ==
                  nodesSequences[iseq]).First();

                //Due to parallelization the left edges and right edges count
                //can be swapped while processing. if actual left edges count 
                //is either equal to expected left edges count or right edges count and vice versa.
                Assert.IsTrue(
                  dbnodes.LeftExtensionNodes.Count.ToString((IFormatProvider)null) == leftEdgesCount[iseq] ||
                  dbnodes.LeftExtensionNodes.Count.ToString((IFormatProvider)null) == rightEdgesCount[iseq]);
                Assert.IsTrue(
                  dbnodes.RightExtensionNodes.Count.ToString((IFormatProvider)null) == leftEdgesCount[iseq] ||
                  dbnodes.RightExtensionNodes.Count.ToString((IFormatProvider)null) == rightEdgesCount[iseq]);
            }

        }

        /// <summary>
        /// Validate the DeBruijnNode ctor by passing the kmer and validating 
        /// the node object.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnNodeCtor(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build the kmers using assembler
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength));

            // Validate the node creation
            // Create node and add left node.
            DeBruijnNode node = new DeBruijnNode(int.Parse(kmerLength, (IFormatProvider)null), 0,
              lstKmers[0].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            DeBruijnNode leftnode = new DeBruijnNode(int.Parse(kmerLength, (IFormatProvider)null), 1,
              lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            node.AddLeftEndExtension(leftnode, true);

            Assert.AreEqual(lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Count,
              node.LeftExtensionNodes.Count);

            Console.WriteLine(
                "PaDeNA BVT : DeBruijnNode ctor() validation for PaDeNA step2 completed successfully");
            ApplicationLog.WriteLine(
                "PaDeNA BVT : DeBruijnNode ctor() validation for PaDeNA step2 completed successfully");
        }

        /// <summary>
        /// Validate AddLeftEndExtension() method of DeBruijnNode 
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnNodeAddLeftExtension(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength));

            // Create node and add left node.
            DeBruijnNode node = new DeBruijnNode(int.Parse(kmerLength, (IFormatProvider)null), 0,
              lstKmers[0].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            DeBruijnNode leftnode = new DeBruijnNode(int.Parse(kmerLength, (IFormatProvider)null), 1,
              lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            node.AddLeftEndExtension(leftnode, true);

            Assert.AreEqual(lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Count,
              node.LeftExtensionNodes.Count);
            Console.WriteLine(
                @"PaDeNA BVT : DeBruijnNode AddLeftExtension() validation 
                    for PaDeNA step2 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA BVT :DeBruijnNode AddLeftExtension() validation 
                    for PaDeNA step2 completed successfully");
        }

        /// <summary>
        /// Validate AddRightEndExtension() method of DeBruijnNode 
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnNodeAddRightExtension(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength));

            // Create node and add right node.
            DeBruijnNode node = new DeBruijnNode(int.Parse(kmerLength, (IFormatProvider)null), 0,
              lstKmers[0].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            DeBruijnNode rightnode = new DeBruijnNode(int.Parse(kmerLength, (IFormatProvider)null), 1,
              lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            node.AddRightEndExtension(rightnode, false);

            Assert.AreEqual(lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Count,
              node.RightExtensionNodes.Count);
            Console.WriteLine(
                @"PaDeNA BVT : DeBruijnNode AddRightExtension() validation 
                    for PaDeNA step2 completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT :DeBruijnNode AddRightExtension() validation 
                    for PaDeNA step2 completed successfully");
        }

        /// <summary>
        /// Validate the ParallelDeNovoAssembler unDangleGraph() method which removes the dangling link
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDe2AssemblerUnDangleGraph(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // and remove the dangling links from graph in step3
            // Validate the graph
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();

            ValidateGraph(this.Graph, nodeName);
            Console.WriteLine(
                @"PaDeNA BVT : ParallelDeNovoAssembler.UndangleGraph() 
                    validation for PaDeNA step3 completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT :ParallelDeNovoAssembler.UndangleGraph() 
                    validation for PaDeNA step3 completed successfully");
        }

        /// <summary>
        /// Validate the PaDeNA DetectErrorNodes() method is 
        /// returning dangling nodes as expected 
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidatePaDeNADetectErrorNodes(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.KmerLengthNode);
            string danglingSequence = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.DangleNodeSequenceNode);
            string[] expectedDanglings = danglingSequence.Split(',');

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // and remove the dangling links from graph in step3
            // Validate the graph
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();

            // Find the dangling node
            DanglingLinksPurger danglingLinksPurger =
                new DanglingLinksPurger(int.Parse(kmerLength, (IFormatProvider)null) + 1);
            DeBruijnPathList danglingnodes =
                danglingLinksPurger.DetectErroneousNodes(this.Graph);
            foreach (DeBruijnPath dbnodes in danglingnodes.Paths)
            {
                foreach (DeBruijnNode node in dbnodes.PathNodes)
                {
                    expectedDanglings.Contains(this.Graph.GetNodeSequence(node).ToString());
                }
            }
            Console.WriteLine(
                @"PaDeNA BVT : DeBruijnGraph.DetectErrorNodes() 
                    validation for PaDeNA step3 completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT :DeBruijnGraph.DetectErrorNodes() 
                    validation for PaDeNA step3 completed successfully");
        }

        /// <summary>
        /// Validate RemoveErrorNodes() method is removing dangling nodes from the graph
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidatePaDeNARemoveErrorNodes(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // and remove the dangling links from graph in step3
            // Validate the graph
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;

            // Find the dangling nodes and remove the dangling node
            DanglingLinksPurger danglingLinksPurger =
                new DanglingLinksPurger(int.Parse(kmerLength, (IFormatProvider)null) + 1);
            DeBruijnPathList danglingnodes =
                danglingLinksPurger.DetectErroneousNodes(graph);
            danglingLinksPurger.RemoveErroneousNodes(graph, danglingnodes);
            Assert.IsFalse(graph.Nodes.Contains(danglingnodes.Paths[0].PathNodes[0]));

            Console.WriteLine(
                @"PaDeNA BVT : DeBruijnGraph.RemoveErrorNodes() validation 
                    for PaDeNA step3 completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT :DeBruijnGraph.RemoveErrorNodes() validation 
                    for PaDeNA step3 completed successfully");
        }

        /// <summary>
        /// Validate ParallelDeNovoAssembler.RemoveRedundancy() which removes bubbles formed in the graph
        /// and validate the graph
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDe2AssemblerRemoveRedundancy(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles from graph in step4
            // Validate the graph
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();
            this.RedundantPathsPurger =
                new RedundantPathsPurger(int.Parse(kmerLength, (IFormatProvider)null) + 1);
            this.RemoveRedundancy();

            ValidateGraph(this.Graph, nodeName);
            Console.WriteLine(
                @"PaDeNA BVT : ParallelDeNovoAssembler.RemoveRedundancy() 
                    validation for PaDeNA step4 completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT :ParallelDeNovoAssembler.RemoveRedundancy() 
                    validation for PaDeNA step4 completed successfully");
        }

        /// <summary>
        /// Creates RedundantPathPurger instance by passing pathlength and count. Detect 
        /// redundant error nodes and remove these nodes from the graph. Validate the graph.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateRedundantPathPurgerCtor(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Validate the graph
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();

            // Create RedundantPathPurger instance, detect redundant nodes and remove error nodes
            RedundantPathsPurger redundantPathPurger =
                new RedundantPathsPurger(int.Parse(kmerLength, (IFormatProvider)null) + 1);
            DeBruijnPathList redundantnodelist =
                redundantPathPurger.DetectErroneousNodes(this.Graph);
            redundantPathPurger.RemoveErroneousNodes(this.Graph, redundantnodelist);

            ValidateGraph(this.Graph, nodeName);
            Console.WriteLine(
                @"PaDeNA BVT : RedundantPathsPurger ctor and methods validation for 
                    PaDeNA step4 completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT :RedundantPathsPurger ctor and methods validation for 
                    PaDeNA step4 completed successfully");
        }

        /// <summary>
        /// Validate ParallelDeNovoAssembler.BuildContigs() by passing graph object
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDe2AssemblerBuildContigs(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.KmerLengthNode);
            string expectedContigsString = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ContigsNode);
            string[] expectedContigs = expectedContigsString.Split(',');

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate the contigs
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();
            this.RedundantPathsPurger =
                new RedundantPathsPurger(int.Parse(kmerLength, (IFormatProvider)null) + 1);
            this.RemoveRedundancy();
            this.ContigBuilder = new SimplePathContigBuilder();
            IList<ISequence> contigs = this.BuildContigs();

            for (int index = 0; index < contigs.Count; index++)
            {
                Assert.IsTrue(expectedContigs.Contains(contigs[index].ToString()));
            }
            Console.WriteLine(
                @"PaDeNA BVT : ParallelDeNovoAssembler.BuildContigs() 
                    validation for PaDeNA step5 completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT :ParallelDeNovoAssembler.BuildContigs() 
                    validation for PaDeNA step5 completed successfully");
        }

        /// <summary>
        /// Validate the SimpleContigBuilder Build() method using step 4 graph
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateSimpleContigBuilderBuild(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);
            string expectedContigsString =
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ContigsNode);
            string[] expectedContigs = expectedContigsString.Split(',');

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles from graph in step4
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();
            this.RedundantPathsPurger =
                new RedundantPathsPurger(int.Parse(kmerLength, (IFormatProvider)null) + 1);
            this.RemoveRedundancy();

            // Validate the SimpleContigBuilder.Build() by passing graph
            SimplePathContigBuilder builder = new SimplePathContigBuilder();
            IList<ISequence> contigs = builder.Build(this.Graph);

            // Validate the contigs
            for (int index = 0; index < contigs.Count; index++)
            {
                Assert.IsTrue(expectedContigs.Contains(contigs[index].ToString()));
            }
            Console.WriteLine(
                @"PaDeNA BVT : SimpleContigBuilder.BuildContigs() validation for 
                    PaDeNA step5 completed successfully");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT :SimpleContigBuilder.BuildContigs() validation for 
                    PaDeNA step5 completed successfully");
        }

        /// <summary>
        /// Validate generating Map paired reads.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidatePairedReads(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string expectedPairedReadsCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.PairedReadsCountNode);
            string[] backwardReadsNode = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.BackwardReadsNode);
            string[] forwardReadsNode = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.ForwardReadsNode);
            string[] expectedLibrary = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.LibraryNode);
            string[] expectedMean = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.MeanLengthNode);
            string[] deviationNode = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.DeviationNode);

            IList<ISequence> sequenceReads = new List<ISequence>();
            IList<MatePair> pairedreads = new List<MatePair>();

            // Get the input reads 
            IList<ISequence> sequences = null;
            using (FastaParser parser = new FastaParser())
            {
                sequences = parser.Parse(filePath);
            }

            foreach (ISequence seq in sequences)
            {
                sequenceReads.Add(seq);
            }

            // Convert reads to map paired reads.
            MatePairMapper pair = new MatePairMapper();
            pairedreads = pair.Map(sequenceReads);

            // Validate Map paired reads.
            Assert.AreEqual(expectedPairedReadsCount, pairedreads.Count.ToString((IFormatProvider)null));

            for (int index = 0; index < pairedreads.Count; index++)
            {
                Assert.IsTrue(forwardReadsNode.Contains(pairedreads[index].GetForwardRead(sequenceReads).ToString()));
                Assert.IsTrue(backwardReadsNode.Contains(pairedreads[index].GetReverseRead(sequenceReads).ToString()));
                Assert.IsTrue(
                    deviationNode.Contains(pairedreads[index].StandardDeviationOfLibrary.ToString((IFormatProvider)null)));
                Assert.IsTrue(expectedMean.Contains(pairedreads[index].MeanLengthOfLibrary.ToString((IFormatProvider)null)));
                Assert.IsTrue(expectedLibrary.Contains(pairedreads[index].Library.ToString((IFormatProvider)null)));
            }

            Console.WriteLine(@"PaDeNA BVT : Map paired reads has been verified successfully");
            ApplicationLog.WriteLine(@"PaDeNA BVT : Map paired reads has been verified successfully");
        }

        /// <summary>
        /// Validate Add library information to existing libraries.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void AddLibraryInformation(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string expectedPairedReadsCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.PairedReadsCountNode);
            string[] backwardReadsNode = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.BackwardReadsNode);
            string[] forwardReadsNode = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.ForwardReadsNode);
            string[] expectedLibrary = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.LibraryNode);
            string[] expectedMean = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.MeanLengthNode);
            string[] deviationNode = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.DeviationNode);
            string libraray = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.LibraryName);
            string StdDeviation = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviation);
            string mean = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.Mean);

            IList<ISequence> sequenceReads = new List<ISequence>();
            IList<MatePair> pairedreads = new List<MatePair>();

            // Get the input reads 
            IList<ISequence> sequences = null;
            using (FastaParser parser = new FastaParser())
            {
                sequences = parser.Parse(filePath);
            }
            foreach (ISequence seq in sequences)
            {
                sequenceReads.Add(seq);
            }

            // Add a new library infomration.
            CloneLibrary.Instance.AddLibrary(libraray,
                float.Parse(mean, (IFormatProvider)null), float.Parse(StdDeviation, (IFormatProvider)null));

            // Convert reads to map paired reads. 
            MatePairMapper pair = new MatePairMapper();
            pairedreads = pair.Map(sequenceReads);

            // Validate Map paired reads.
            Assert.AreEqual(expectedPairedReadsCount, pairedreads.Count.ToString((IFormatProvider)null));

            for (int index = 0; index < pairedreads.Count; index++)
            {
                Assert.IsTrue(forwardReadsNode.Contains(pairedreads[index].GetForwardRead(sequenceReads).ToString()));
                Assert.IsTrue(backwardReadsNode.Contains(pairedreads[index].GetReverseRead(sequenceReads).ToString()));
                Assert.IsTrue(deviationNode.Contains(pairedreads[index].StandardDeviationOfLibrary.ToString((IFormatProvider)null)));
                Assert.IsTrue(expectedLibrary.Contains(pairedreads[index].Library.ToString((IFormatProvider)null)));
                Assert.IsTrue(expectedMean.Contains(pairedreads[index].MeanLengthOfLibrary.ToString((IFormatProvider)null)));
            }

            Console.WriteLine(@"PaDeNA BVT : Map paired reads has been verified successfully");
            ApplicationLog.WriteLine(@"PaDeNA BVT : Map paired reads has been verified successfully");
        }

        /// <summary>
        /// Validate building map reads to contigs.
        /// </summary>
        /// <param name="nodeName">xml node name used for a different testcases</param>
        /// <param name="IsFullOverlap">True if full overlap else false</param>
        internal void ValidateMapReadsToContig(string nodeName, bool IsFullOverlap)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.KmerLengthNode);
            string daglingThreshold = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.DanglingLinkThresholdNode);
            string redundantThreshold = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.RedundantThreshold);
            string readMapLengthString = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ReadMapLength);
            string readStartPosString = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ReadStartPos);
            string contigStartPosString = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ContigStartPos);
            string[] expectedReadmapLength = readMapLengthString.Split(',');
            string[] expectedReadStartPos = readStartPosString.Split(',');
            string[] expectedContigStartPos = contigStartPosString.Split(',');

            // Get the input reads and build kmerssequences
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            this.KmerLength = Int32.Parse(kmerLength, (IFormatProvider)null);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null); ;
            this.DanglingLinksPurger =
                new DanglingLinksPurger(Int32.Parse(daglingThreshold, (IFormatProvider)null));
            this.RedundantPathsPurger =
                new RedundantPathsPurger(Int32.Parse(redundantThreshold, (IFormatProvider)null));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();
            this.ContigBuilder = new SimplePathContigBuilder();
            this.RemoveRedundancy();

            IList<ISequence> contigs = this.BuildContigs();
            IList<ISequence> sortedContigs = SortContigsData(contigs);
            ReadContigMapper mapper = new ReadContigMapper();
            ReadContigMap maps = mapper.Map(sortedContigs, sequenceReads, this.KmerLength);
            Assert.AreEqual(maps.Count, sequenceReads.Count);

            Dictionary<ISequence, IList<ReadMap>> readMaps = maps[sequenceReads[0].DisplayID];
            IList<ReadMap> readMap = null;

            for (int i = 0; i < sortedContigs.Count; i++)
            {
                readMap = readMaps[sortedContigs[i]];
                if (IsFullOverlap)
                {
                    Assert.AreEqual(expectedReadmapLength[i], readMap[0].Length.ToString((IFormatProvider)null));
                    Assert.AreEqual(expectedContigStartPos[i],
                        readMap[0].StartPositionOfContig.ToString((IFormatProvider)null));
                    Assert.AreEqual(expectedReadStartPos[i], readMap[0].StartPositionOfRead.ToString((IFormatProvider)null));
                    Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);
                }
                else
                {
                    Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.PartialOverlap);
                    break;
                }
            }

            Console.WriteLine(
                "PaDeNA BVT : ReadContigMapper.Map() validation for PaDeNA step6:step2 completed successfully");
            ApplicationLog.WriteLine(
                "PaDeNA BVT :ReadContigMapper.Map() validation for PaDeNA step6:step2 completed successfully");
        }

        /// <summary>
        /// Validate build contig graphs for contigs.
        /// </summary>
        /// <param name="nodeName">xml node name used for a different testcase.</param>
        internal void ValidateContigGraph(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.KmerLengthNode);
            string daglingThreshold = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.RedundantThreshold);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            this.KmerLength = Int32.Parse(kmerLength, (IFormatProvider)null);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null); ;
            this.DanglingLinksPurger =
                new DanglingLinksPurger(Int32.Parse(daglingThreshold, (IFormatProvider)null));
            this.RedundantPathsPurger =
                new RedundantPathsPurger(Int32.Parse(redundantThreshold, (IFormatProvider)null));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();

            // Build contig.
            this.ContigBuilder = new SimplePathContigBuilder();
            this.RemoveRedundancy();
            IList<ISequence> contigs = this.BuildContigs();

            // build contig graph.
            this.Graph.BuildContigGraph(contigs, this.KmerLength);

            int contigGraphCount = this.Graph.Nodes.Count;

            // validate contig graph.
            Assert.AreEqual(contigs.Count, contigGraphCount);
            ValidateGraph(this.Graph, nodeName);
            Console.WriteLine(
                "PaDeNA BVT : BuildContigGraph() validation for PaDeNA step6:step3 completed successfully");
            ApplicationLog.WriteLine(
                "PaDeNA BVT :BuildContigGraph validation for PaDeNA step6:step3 completed successfully");
        }

        /// <summary>
        /// Validate Filter contig nodes.
        /// </summary>
        /// <param name="nodeName">xml node name used for a differnt testcase.</param>
        /// <param name="IsRedundancy">True if passing redundancy, else false</param>
        internal void ValidateFilterPaired(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.KmerLengthNode);
            string daglingThreshold = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RedundantThreshold);
            string expectedContigPairedReadsCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ContigPairedReadsCount);
            string forwardReadStartPos = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ForwardReadStartPos);
            string reverseReadStartPos = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ReverseReadStartPos);
            string reverseComplementStartPos = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RerverseReadReverseCompPos);
            string[] expectedForwardReadStartPos = forwardReadStartPos.Split(',');
            string[] expectedReverseReadStartPos = reverseReadStartPos.Split(',');
            string[] expectedReverseComplementStartPos = reverseComplementStartPos.Split(',');

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            this.KmerLength = Int32.Parse(kmerLength, (IFormatProvider)null);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null); ;
            this.DanglingLinksPurger =
                new DanglingLinksPurger(Int32.Parse(daglingThreshold, (IFormatProvider)null));
            this.RedundantPathsPurger =
                new RedundantPathsPurger(Int32.Parse(redundantThreshold, (IFormatProvider)null));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();

            // Build contig.
            this.ContigBuilder = new SimplePathContigBuilder();
            this.RemoveRedundancy();
            IList<ISequence> contigs = this.BuildContigs();
            IList<ISequence> sortedContigs = SortContigsData(contigs);
            ReadContigMapper mapper = new ReadContigMapper();

            ReadContigMap maps = mapper.Map(sortedContigs, sequenceReads, this.KmerLength);

            // Find map paired reads.
            MatePairMapper mapPairedReads = new MatePairMapper();
            ContigMatePairs pairedReads = mapPairedReads.MapContigToMatePairs(sequenceReads,
                maps);

            // Filter paired reads based on the contig orientation.
            OrientationBasedMatePairFilter filter =
                new OrientationBasedMatePairFilter();
            ContigMatePairs contigpairedReads = null;

            contigpairedReads = filter.FilterPairedReads(pairedReads, 0);

            Assert.AreEqual(expectedContigPairedReadsCount,
                contigpairedReads.Values.Count.ToString((IFormatProvider)null));

            // Validate Contig paired reads after filtering contig sequences.
            Dictionary<ISequence, IList<ValidMatePair>> map =
                contigpairedReads[sortedContigs[0]];
            IList<ValidMatePair> valid = SortPairedReads(map[sortedContigs[1]], sequenceReads);

            for (int index = 0; index < valid.Count; index++)
            {
                Assert.AreEqual(expectedForwardReadStartPos[index],
                  valid[index].ForwardReadStartPosition[0].ToString((IFormatProvider)null));
                Assert.AreEqual(expectedReverseReadStartPos[index],
                  valid[index].ReverseReadReverseComplementStartPosition[0].ToString((IFormatProvider)null));
                Assert.AreEqual(expectedReverseComplementStartPos[index],
                  valid[index].ReverseReadStartPosition[0].ToString((IFormatProvider)null));
            }

            Console.WriteLine(
                "PaDeNA BVT : FilterPairedReads() validation for PaDeNA step6:step4 completed successfully");
            ApplicationLog.WriteLine(
                "PaDeNA BVT : FilterPairedReads() validation for PaDeNA step6:step4 completed successfully");
        }

        /// <summary>
        /// Validate FilterPairedRead.FilterPairedRead() by passing graph object
        /// </summary>
        /// <param name="nodeName">xml node name used for a differnt testcase.</param>
        /// <param name="IsForward">True for Forward orientation else false.</param>
        internal void ValidateContigDistance(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.KmerLengthNode);
            string daglingThreshold = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RedundantThreshold);
            string expectedContigPairedReadsCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ContigPairedReadsCount);
            string distanceBetweenFirstContigs = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DistanceBetweenFirstContig);
            string distanceBetweenSecondContigs = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DistanceBetweenSecondContig);
            string firstStandardDeviation = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FirstContigStandardDeviation);
            string secondStandardDeviation = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SecondContigStandardDeviation);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            this.KmerLength = Int32.Parse(kmerLength, (IFormatProvider)null);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null); ;
            this.DanglingLinksPurger =
                new DanglingLinksPurger(Int32.Parse(daglingThreshold, (IFormatProvider)null));
            this.RedundantPathsPurger =
                new RedundantPathsPurger(Int32.Parse(redundantThreshold, (IFormatProvider)null));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();

            // Build contig.
            this.ContigBuilder = new SimplePathContigBuilder();
            this.RemoveRedundancy();
            IList<ISequence> contigs = this.BuildContigs();
            IList<ISequence> sortedContigs = SortContigsData(contigs);
            ReadContigMapper mapper = new ReadContigMapper();

            ReadContigMap maps = mapper.Map(sortedContigs, sequenceReads, this.KmerLength);

            // Find map paired reads.
            MatePairMapper mapPairedReads = new MatePairMapper();
            ContigMatePairs pairedReads = mapPairedReads.MapContigToMatePairs(sequenceReads, maps);

            // Filter contigs based on the orientation.
            OrientationBasedMatePairFilter filter = new OrientationBasedMatePairFilter();
            ContigMatePairs contigpairedReads = filter.FilterPairedReads(pairedReads, 0);

            // Calculate the distance between contigs.
            DistanceCalculator calc = new DistanceCalculator();
            calc.CalculateDistance(contigpairedReads);
            Assert.AreEqual(expectedContigPairedReadsCount,
                contigpairedReads.Values.Count.ToString((IFormatProvider)null));

            Dictionary<ISequence, IList<ValidMatePair>> map;
            IList<ValidMatePair> valid;

            map = contigpairedReads[sortedContigs[0]];
            valid = map[sortedContigs[1]];

            // Validate distance and standard deviation between contigs.
            Assert.AreEqual(float.Parse(distanceBetweenFirstContigs, (IFormatProvider)null),
                valid.First().DistanceBetweenContigs[0]);
            Assert.AreEqual(float.Parse(distanceBetweenSecondContigs, (IFormatProvider)null),
                valid.First().DistanceBetweenContigs[1]);
            Assert.AreEqual(float.Parse(firstStandardDeviation, (IFormatProvider)null),
                valid.First().StandardDeviation[0]);
            Assert.AreEqual(float.Parse(secondStandardDeviation, (IFormatProvider)null),
                valid.First().StandardDeviation[1]);

            Console.WriteLine(
                "PaDeNA BVT : DistanceCalculator() validation for PaDeNA step6:step5 completed successfully");
            ApplicationLog.WriteLine(
                "PaDeNA BVT : DistanceCalculator() validation for PaDeNA step6:step5 completed successfully");
        }

        /// <summary>
        /// Validate scaffold paths for a given input reads.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateScaffoldPath(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.KmerLengthNode);
            string daglingThreshold = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RedundantThreshold);
            string[] expectedScaffoldNodes = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.ScaffoldNodes);
            string libraray = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.LibraryName);
            string StdDeviation = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviation);
            string mean = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.Mean);
            string expectedDepth = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.DepthNode);
            string expectedScaffoldPathCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ScaffoldPathCount);

            IList<ScaffoldPath> paths = new List<ScaffoldPath>();

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            this.KmerLength = Int32.Parse(kmerLength, (IFormatProvider)null);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null); ;
            this.DanglingLinksPurger =
                new DanglingLinksPurger(Int32.Parse(daglingThreshold, (IFormatProvider)null));
            this.RedundantPathsPurger =
                new RedundantPathsPurger(Int32.Parse(redundantThreshold, (IFormatProvider)null));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;
            this.UnDangleGraph();

            // Build contig.
            this.ContigBuilder = new SimplePathContigBuilder();
            this.RemoveRedundancy();
            IList<ISequence> contigs = this.BuildContigs();
            IList<ISequence> sortedContigs = SortContigsData(contigs);
            ReadContigMapper mapper = new ReadContigMapper();

            ReadContigMap maps = mapper.Map(sortedContigs, sequenceReads, this.KmerLength);

            // Find map paired reads.
            CloneLibrary.Instance.AddLibrary(libraray, float.Parse(mean, (IFormatProvider)null),
              float.Parse(StdDeviation, (IFormatProvider)null));
            MatePairMapper mapPairedReads = new MatePairMapper();
            ContigMatePairs pairedReads = mapPairedReads.MapContigToMatePairs(sequenceReads, maps);

            // Filter contigs based on the orientation.
            OrientationBasedMatePairFilter filter = new OrientationBasedMatePairFilter();
            ContigMatePairs contigpairedReads = filter.FilterPairedReads(pairedReads, 0);

            DistanceCalculator dist = new DistanceCalculator();
            dist.CalculateDistance(contigpairedReads);
            this.Graph.BuildContigGraph(contigs, this.KmerLength);

            // Validate ScaffoldPath using BFS.
            TracePath trace = new TracePath();
            paths = trace.FindPaths(graph, contigpairedReads, Int32.Parse(kmerLength, (IFormatProvider)null),
              Int32.Parse(expectedDepth, (IFormatProvider)null));

            ScaffoldPath scaffold = paths.First();
            Assert.AreEqual(expectedScaffoldPathCount, paths.Count.ToString((IFormatProvider)null));
            for (int i = 0; i < scaffold.Count; i++)
            {
                Assert.IsTrue(expectedScaffoldNodes.Contains(
                 this.Graph.GetNodeSequence(scaffold[i].Key).ToString())
                 || expectedScaffoldNodes.Contains(this.Graph.GetNodeSequence(scaffold[i].Key).ReverseComplement.ToString()));
            }

            Console.WriteLine(
                "PaDeNA BVT : FindPaths() validation for PaDeNA step6:step6 completed successfully");
            ApplicationLog.WriteLine(
                "PaDeNA BVT : FindPaths() validation for PaDeNA step6:step6 completed successfully");
        }

        /// <summary>
        /// Validate Assembled paths for a given input reads.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateAssembledPath(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.KmerLengthNode);
            string daglingThreshold = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RedundantThreshold);
            string libraray = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.LibraryName);
            string StdDeviation = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviation);
            string mean = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.Mean);
            string expectedDepth = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.DepthNode);
            string expectedScaffoldPathCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ScaffoldPathCount);
            string[] assembledPath = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.SequencePathNode);

            IList<ScaffoldPath> paths = new List<ScaffoldPath>();

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            this.KmerLength = Int32.Parse(kmerLength, (IFormatProvider)null);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null); ;
            this.DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold, (IFormatProvider)null));
            this.RedundantPathsPurger = new RedundantPathsPurger(Int32.Parse(redundantThreshold, (IFormatProvider)null));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;
            this.UnDangleGraph();

            // Build contig.
            this.ContigBuilder = new SimplePathContigBuilder();
            this.RemoveRedundancy();
            IList<ISequence> contigs = this.BuildContigs();
            IList<ISequence> sortedContigs = SortContigsData(contigs);
            ReadContigMapper mapper = new ReadContigMapper();

            ReadContigMap maps = mapper.Map(sortedContigs, sequenceReads, this.KmerLength);

            // Find map paired reads.
            CloneLibrary.Instance.AddLibrary(libraray, float.Parse(mean, (IFormatProvider)null),
              float.Parse(StdDeviation, (IFormatProvider)null));
            MatePairMapper mapPairedReads = new MatePairMapper();
            ContigMatePairs pairedReads = mapPairedReads.MapContigToMatePairs(sequenceReads, maps);

            // Filter contigs based on the orientation.
            OrientationBasedMatePairFilter filter = new OrientationBasedMatePairFilter();
            ContigMatePairs contigpairedReads = filter.FilterPairedReads(pairedReads, 0);

            DistanceCalculator dist = new DistanceCalculator();
            dist.CalculateDistance(contigpairedReads);
            this.Graph.BuildContigGraph(contigs, this.KmerLength);

            // Validate ScaffoldPath using BFS.
            TracePath trace = new TracePath();
            paths = trace.FindPaths(graph, contigpairedReads, Int32.Parse(kmerLength, (IFormatProvider)null),
              Int32.Parse(expectedDepth, (IFormatProvider)null));

            Assert.AreEqual(expectedScaffoldPathCount, paths.Count.ToString((IFormatProvider)null));

            // Assemble paths.
            PathPurger pathsAssembler = new PathPurger();
            pathsAssembler.PurgePath(paths);
            IList<ISequence> seqList = new List<ISequence>();

            // Get a sequence from assembled path.
            foreach (ScaffoldPath temp in paths)
            {
                seqList.Add(temp.BuildSequenceFromPath(graph,
                  Int32.Parse(kmerLength, (IFormatProvider)null)));
            }

            // Validate assembled sequence paths.
            for (int index = 0; index < seqList.Count; index++)
            {
                Assert.IsTrue(assembledPath.Contains(seqList[index].ToString()));
            }

            Console.WriteLine(
                "PaDeNA BVT : AssemblePath() validation for PaDeNA step6:step7 completed successfully");
            ApplicationLog.WriteLine(
                "PaDeNA BVT : AssemblePath() validation for PaDeNA step6:step7 completed successfully");
        }

        /// <summary>
        /// Validate Parallel Denovo Assembly Assembled sequences.
        /// </summary>
        /// <param name="nodeName">XML node used to validate different test scenarios</param>
        internal void ValidatePaDeNAAssembledSeqs(string nodeName,
            bool IsScaffold, bool EnableLowerContigRemoval, bool AllowErosion)
        {
            // Get values from XML node.
            string filePath = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.KmerLengthNode);
            string daglingThreshold = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RedundantThreshold);
            string library = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.LibraryName);
            string stdDeviation = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviation);
            string mean = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.Mean);
            string erosionThreshold = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ErosionNode);
            string lowCCThreshold = _utilityObj._xmlUtil.GetTextValue(nodeName,
               Constants.LowCoverageContigNode);
            string assembledSequences = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.SequencePathNode);
            string assembledSeqCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.AssembledSeqCountNode);
            string[] updatedAssembledSeqs = assembledSequences.Split(',');

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Create a ParallelDeNovoAssembler instance.
            ParallelDeNovoAssembler denovoObj = null;
            try
            {
                denovoObj = new ParallelDeNovoAssembler();

                denovoObj.KmerLength = Int32.Parse(kmerLength, (IFormatProvider)null);
                denovoObj.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null);
                denovoObj.RedundantPathLengthThreshold = Int32.Parse(
                    redundantThreshold, (IFormatProvider)null);
                if (EnableLowerContigRemoval)
                {
                    denovoObj.AllowLowCoverageContigRemoval = EnableLowerContigRemoval;
                    denovoObj.ContigCoverageThreshold = double.Parse(lowCCThreshold, (IFormatProvider)null);
                }

                if (AllowErosion)
                {
                    denovoObj.AllowErosion = AllowErosion;
                    denovoObj.ErosionThreshold = Int32.Parse(erosionThreshold, (IFormatProvider)null);
                }


                CloneLibrary.Instance.AddLibrary(library, float.Parse(mean, (IFormatProvider)null),
                 float.Parse(stdDeviation, (IFormatProvider)null));

                IDeNovoAssembly assembly = denovoObj.Assemble(sequenceReads.ToList(),
                    IsScaffold);

                // Validate assembled sequences.
                Assert.AreEqual(assembledSeqCount,
                    assembly.AssembledSequences.Count.ToString((IFormatProvider)null));

                for (int i = 0; i < assembly.AssembledSequences.Count; i++)
                {
                    Assert.IsTrue(assembledSequences.Contains(
                        assembly.AssembledSequences[i].ToString())
                        || updatedAssembledSeqs.Contains(
                        assembly.AssembledSequences[i].ReverseComplement.ToString()));
                }

                Console.WriteLine(
                   "PaDeNA BVT : Assemble() validation for PaDeNA step6:step7 completed successfully");
                ApplicationLog.WriteLine(
                    "PaDeNA BVT : Assemble() validation for PaDeNA step6:step7 completed successfully");
            }
            finally
            {
                if (denovoObj != null)
                    denovoObj.Dispose();
            }
        }

        /// <summary>
        /// Validate ParallelDenovoAssembler class properties.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ParallelDenovoAssemblyProperties(string nodeName)
        {
            // Get values from XML node.
            string filePath = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.KmerLengthNode);
            string library = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.LibraryName);
            string StdDeviation = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviation);
            string mean = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.Mean);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate the contigs
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();
            this.RedundantPathsPurger =
                new RedundantPathsPurger(int.Parse(kmerLength, (IFormatProvider)null) + 1);
            this.RemoveRedundancy();
            this.ContigBuilder = new SimplePathContigBuilder();

            // Build contigs
            IList<ISequence> contigs = this.BuildContigs();

            // Build scaffolds.
            CloneLibrary.Instance.AddLibrary(library, float.Parse(mean, (IFormatProvider)null),
            float.Parse(StdDeviation, (IFormatProvider)null));
            IList<ISequence> scaffolds = BuildScaffolds(contigs);
            PaDeNAAssembly denovoAssembly = new PaDeNAAssembly();

            denovoAssembly.AddContigs(contigs);
            denovoAssembly.AddScaffolds(scaffolds);

            Assert.AreEqual(denovoAssembly.ContigSequences.Count,
                contigs.Count);
            Assert.AreEqual(denovoAssembly.Scaffolds.Count, scaffolds.Count);
            Assert.IsNull(denovoAssembly.Documentation);

            // Validate ParallelDenovoAssembler properties.
            Console.WriteLine(
                @"PaDeNA BVT : Validated ParallelDenovo Assembly properties");
            ApplicationLog.WriteLine(
                @"PaDeNA BVT : Validated ParallelDenovo Assembly properties");
        }

        /// <summary>
        /// Sort Contig List based on the contig sequence
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        static IList<ISequence> SortContigsData(IList<ISequence> contigsList)
        {
            return (from ContigData in contigsList
                    orderby ContigData.ToString()
                    select ContigData).ToList();
        }

        ///<summary>
        /// Sort Valid Paired reads based on forward reads.
        /// For consistent output due to parallel implementation.
        /// </summary>
        /// <param name="list">List of Paired Reads</param>
        /// <param name="reads">Input list of reads.</param>
        /// <returns>Sorted List of Paired reads</returns>
        static IList<ValidMatePair> SortPairedReads(
            IList<ValidMatePair> list, IList<ISequence> reads)
        {
            return (from valid in list
                    orderby valid.PairedRead.GetForwardRead(reads).ToString()
                    select valid).ToList();
        }

        #endregion
    }
}
