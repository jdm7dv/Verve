// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * PaDeNAP1TestCases.cs
 * 
 *  This file contains the PaDeNA P1 test cases.
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

using NUnit.Framework;

namespace MBF.TestAutomation.Algorithms.Assembly.PaDeNA
{
    /// <summary>
    /// The class contains P1 test cases to confirm PaDeNA.
    /// </summary>
    [TestFixture]
    public class PaDeNAP1TestCases
    {
        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static PaDeNAP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\PaDeNATestData\PaDeNATestsConfig.xml");

        }

        #endregion

        #region PaDeNAStep1TestCases

        /// <summary>
        /// Validate ParallelDeNovothis is building valid kmers 
        /// using virul genome input reads in a fasta file and kmerLength 28
        /// Input : virul genome input reads and kmerLength 28
        /// Output : kmers sequence
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep1BuildKmersForViralGenomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNABuildKmers(Constants.ViralGenomeReadsNode, true);
        }

        /// <summary>
        /// Validate ParallelDeNovothis is building valid kmers 
        /// using yeast genome chromosome input reads in a fasta file and kmerLength 28
        /// Input : chromosome input reads and kmerLength 28
        /// Output : kmers sequence
        /// </summary>
        public void ValidatePaDeNAStep1BuildKmersWithChromosomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNABuildKmers(Constants.SmallChromosomeReadsNode, true);
        }

        /// <summary>
        /// Validate ParallelDeNovothis is building valid kmers 
        /// using input reads which contains sequence and reverse complement
        /// Input : input reads with reverse complement and kmerLength 20
        /// Output : kmers sequence
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep1BuildKmersWithRCReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNABuildKmers(Constants.OneLineReadsWithRCNode, false);
        }

        /// <summary>
        /// Validate ParallelDeNovothis is building valid kmers 
        /// using input reads which will generate clusters in step2 graph
        /// Input : input reads which will generate clusters and kmerLength 7
        /// Output : kmers sequence
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep1BuildKmersWithClusters()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNABuildKmers(Constants.OneLineReadsWithClustersNode, false);
        }

        /// <summary>
        /// Validate KmersOfSequence ctor (sequence, length) by passing
        /// one line sequence and kmer length 4
        /// Input : Build kmeres from one line input reads of small size 
        /// chromosome sequence and kmerLength 4
        /// Output : kmers of sequence object with build kmers
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep1KmersOfSequenceCtorWithBuildKmers()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateKmersOfSequenceCtorWithBuildKmers(Constants.OneLineReadsNode);
        }

        /// <summary>
        /// Validate KmersOfSequence ctor (sequence, length, set of kmers) 
        /// by passing small size chromsome sequence and kmer length 28
        /// after building kmers
        /// Input : Build kmeres from one line input reads of small size 
        /// chromosome sequence and kmerLength 28
        /// Output : kmers of sequence object with build kmers
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep1KmersOfSequenceCtorWithBuildKmersForSmallSizeSequences()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateKmersOfSequenceCtorWithBuildKmers(Constants.OneLineReadsNode);
        }

        /// <summary>
        /// Validate SequenceRangeToKmerBuilder Build(lstsequences,kmerLength) is 
        /// building valid kmers using small size chromosome sequences in a
        /// fasta file and kmerLength 28
        /// Input : Build kmeres from 4000 input reads of small size 
        /// chromosome sequence and kmerLength 28 
        /// Output : kmers sequence
        /// </summary>
        public void ValidatePaDeNAStep1KmerBuilderBuildWithSmallSizeFile()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateKmerBuilderBuild(Constants.SmallChromosomeReadsNode, false);
        }

        /// <summary>
        /// Validate SequenceRangeToKmerBuilder Build(lstsequences,kmerLength) is 
        /// building valid kmers using small size chromosome sequences in a
        /// fasta file and kmerLength 28
        /// Input : Build kmeres from 4000 input reads of small size 
        /// chromosome sequence and kmerLength 28 
        /// Output : kmers sequence
        /// </summary>
        public void ValidatePaDeNAStep1KmersOfSequenceToSequencesWithSmallSizeFile()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateKmerBuilderBuild(Constants.SmallChromosomeReadsNode, true);
        }

        /// <summary>
        /// Validate KmersOfSequence properties
        /// Input : Build kmeres from 4000 input reads of small size 
        /// chromosome sequence and kmerLength 4 
        /// Output : kmers sequence
        /// </summary>
        [Test]
        public void ValidateKmersOfSequenceCtrproperties()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateKmersOfSequenceCtorProperties(Constants.OneLineReadsNode);
        }

        /// <summary>
        /// Validate KmersOfSequence ToSequences() method using small size reads
        /// Input : Build kmeres from 4000 input reads of small size 
        /// chromosome sequence and kmerLength 28 
        /// Output : kmers sequence
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep1KmersOfSequenceToSequencesUsingSmallSizeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateKmersOfSequenceToSequences(Constants.OneLineReadsNode);
        }

        /// <summary>
        /// Validate ParallelDeNovothis is building valid kmers 
        /// using microorganisms genome input reads in a fasta file and kmerLength 28
        /// Input : virul genome input reads and kmerLength 28
        /// Output : kmers sequence
        /// </summary>
        public void ValidatePaDeNAStep1BuildKmersForMicroOrganisms()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateKmerBuilderBuild(Constants.MicroorganismReadsNode, true);
        }

        #endregion

        #region PaDeNAStep2TestCases

        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with virul genome reads and kmerLength 28
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep2BuildGraphForVirulGenome()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNABuildGraph(Constants.ViralGenomeReadsNode, true);
        }

        /// <summary>
        /// Validate Graph after building graph with DeBruijnGraph.Build()
        /// with kmers for small size sequences.
        /// Input : Kmers
        /// Output: Graph
        /// </summary>
        public void ValidatePaDeNAStep2DeBruijnGraphForSmallSizeRC()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateDeBruijnGraphBuild(Constants.Step2SmallChromosomeReadsNode);
        }

        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with input reads contains reverse complement and kmerLength 20
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep2BuildGraphWithRCReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNABuildGraph(Constants.OneLineWithRCStep2Node, false);
        }

        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with input reads which will generate clusters in step2 graph
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep2BuildGraphWithClusters()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNABuildGraph(Constants.OneLineReadsWithClustersNode, false);
        }

        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with input reads which will generate clusters in step2 graph
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep2BuildGraphWithSmallSizeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNABuildGraph(Constants.SmallChromosomeReadsNode, true);
        }

        ///<summary>
        /// Validate Validate DeBruijinGraph properties
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep2DeBruijinGraphProperties()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateDeBruijinGraphproperties(Constants.OneLineStep2GraphNode);
        }

        ///<summary>
        /// Validate Validate DeBruijinGraph properties for small size sequence reads.
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep2DeBruijinGraphPropertiesForSmallSizeRC()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateDeBruijinGraphproperties(Constants.OneLineReadsWithRCNode);
        }

        /// <summary>
        /// Validate DeBruijinNode ctor by passing dna 
        /// kmersof sequence and graph object of chromosome
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep2DeBruijinCtrByPassingOneLineRC()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateDeBruijnNodeCtor(Constants.OneLineStep2GraphNode);
        }

        /// <summary>
        /// Validate AddLeftExtension() method by 
        /// passing node object and orinetation 
        /// with chromosome read
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep2DeBruijnNodeAddLeftExtensionWithReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateDeBruijnNodeAddLeftExtension(Constants.OneLineWithRCStep2Node);
        }

        /// <summary>
        /// Create dbruijn node by passing kmer and create another node.
        /// Add new node as leftendextension of first node. Validate the 
        /// AddRightEndExtension() method.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep2DeBruijnNodeAddRightExtensionWithRCReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateDeBruijnNodeAddRightExtension(Constants.OneLineWithRCStep2Node);
        }

        /// <summary>
        /// Validate RemoveExtension() method by passing node 
        /// object and orientation with one line read
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep2DeBruijnNodeRemoveExtensionWithOneLineReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateDeBruijnNodeRemoveExtension(Constants.OneLineWithRCStep2Node);
        }

        /// <summary>
        /// Validate RemoveExtension() method by passing node
        /// object and orientation with chromosome read
        /// </summary>
        public void ValidatePaDeNAStep2DeBruijnNodeRemoveExtensionWithsmallSizeChromosomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateDeBruijnNodeRemoveExtension(Constants.SmallChromosomeReadsNode);
        }

        #endregion

        #region PaDeNAStep3TestCases

        /// <summary>
        /// Validate the PaDeNA step3 
        /// which removes dangling links from the graph using reads with rc kmers
        /// Input: Graph with dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep3UndangleGraphWithRCReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNAUnDangleGraph(Constants.OneLineWithRCStep2Node, true, false);
        }

        /// <summary>
        /// Validate the PaDeNA step3 
        /// which removes dangling links from the graph using virul genome kmers
        /// Input: Graph with dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep3UndangleGraphForViralGenomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNAUnDangleGraph(Constants.ViralGenomeReadsNode, true, true);
        }

        /// <summary>
        /// Validate the PaDeNA step3 using input reads which will generate clusters in step2 graph
        /// Input: Graph with dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep3UndangleGraphWithClusters()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNAUnDangleGraph(Constants.OneLineReadsWithClustersAfterDangling, false, false);
        }

        /// <summary>
        /// Validate removal of dangling links by passing input reads with 3 dangling links
        /// Input: Graph with 3 dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep3UndangleGraphWithDanglingLinks()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNAUnDangleGraph(Constants.ReadsWithDanglingLinksNode, false, false);
        }

        /// <summary>
        /// Validate removal of dangling links by passing input reads with 3 dangling links
        /// Input: Graph with 3 dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep3UndangleGraphWithMultipleDanglingLinks()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNAUnDangleGraph(Constants.ReadsWithMultipleDanglingLinksNode, false, false);
        }

        ///<summary>
        /// Validate the PaDeNA step3 
        /// which removes dangling links from the graph using microorganism genome kmers
        /// Input: Graph with dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        public void ValidatePaDeNAStep3UndangleGraphForMicroorganisms()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNAUnDangleGraph(Constants.MicroorganismReadsNode, false, false);
        }

        /// <summary>
        /// Validate the DanglingLinksPurger is removing the dangling link nodes
        /// from the graph
        /// Input: Graph and dangling node
        /// Output: Graph without any dangling nodes
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep3RemoveErrorNodesForSmallSizeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNARemoveErrorNodes(Constants.ViralGenomeReadsNode);
        }

        #endregion

        #region PaDeNAStep4TestCases

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using virul genome reads such that it will create bubbles in the graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep4RemoveRedundancyForViralGenomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNARemoveRedundancy(Constants.ViralGenomeReadsNode, true, true);
        }

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads with rc such that it will create bubbles in the graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep4RemoveRedundancyWithRCReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNARemoveRedundancy(Constants.OneLineWithRCStep2Node, true, false);
        }

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep4RemoveRedundancyWithClusters()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNARemoveRedundancy(Constants.OneLineReadsWithClustersNode, true, false);
        }

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep4RemoveRedundancyWithBubbles()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNARemoveRedundancy(Constants.ReadsWithBubblesNode, false, false);
        }

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep4RemoveRedundancyWithMultipleBubbles()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNARemoveRedundancy(Constants.ReadsWithMultipleBubblesNode, false, false);
        }

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph using Small size reads
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep4RemoveRedundancyWithSmallSizeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNARemoveRedundancy(Constants.Step4ReadsWithSmallSize, false, true);
        }

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph using Microorganism reads
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        public void ValidatePaDeNAStep4RemoveRedundancyWithMicroorganismReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePaDeNARemoveRedundancy(Constants.MicroorganismReadsNode, false, true);
        }

        /// <summary>
        /// Validate PaDeNA step4 RedundantPathPurgerCtor() by passing graph 
        /// using one line reads 
        /// Input: One line graph
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep4RedundantPathPurgerCtorWithOneLineReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateRedundantPathPurgerCtor(Constants.Step4RedundantPathReadsNode, false);
        }

        /// <summary>
        /// Validate DetectErrorNodes() by passing graph object with
        /// one line reads such that it has bubbles
        /// Input: One line graph
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep4DetectErrorNodesForOneLineReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateRedundantPathPurgerCtor(Constants.Step4RedundantPathReadsNode, false);
        }

        /// <summary>
        /// Validate PaDeNA RemoveErrorNodes() by passing redundant nodes list and graph
        /// Input : graph and redundant nodes list
        /// Output: Graph without bubbles
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep4RemoveErrorNodes()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateRedundantPathPurgerCtor(Constants.OneLineStep4ReadsNode, false);
        }

        /// <summary>
        /// Validate PaDeNA RemoveErrorNodes() by passing redundant nodes list for microorganisms.
        /// Input : graph and redundant nodes list
        /// Output: Graph without bubbles
        /// </summary>
        public void ValidatePaDeNAStep4RemoveErrorNodesForMicroorganisms()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateRedundantPathPurgerCtor(Constants.MicroorganismReadsNode, true);
        }

        #endregion

        #region PaDeNAStep5TestCases

        /// <summary>
        /// Validate PaDeNA step5 by passing graph and validating the contigs
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep5BuildContigsForViralGenomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateDe2thisBuildContigs(Constants.ViralGenomeReadsNode, true);
        }

        /// <summary>
        /// Validate PaDeNA step5 by passing graph and validating the contigs
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep5BuildContigsWithRCReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateDe2thisBuildContigs(Constants.OneLineReadsWithRCNode, false);
        }

        /// <summary>
        /// Validate PaDeNA step5 by passing graph and validating the contigs
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep5BuildContigsWithClusters()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateDe2thisBuildContigs(Constants.OneLineReadsWithClustersNode, false);
        }

        /// <summary>
        /// Validate PaDeNA step5 by passing graph and validating the
        /// contigs for small size chromosomes
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep5BuildContigsForSmallSizeChromosmes()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateDe2thisBuildContigs(Constants.SmallChromosomeReadsNode, true);
        }

        /// <summary>
        /// Validate PaDeNA step5 SimpleContigBuilder.BuildContigs() 
        /// by passing graph for small size chromosome.
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep5SimpleContigBuilderBuildContigsForSmallSizeRC()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateSimpleContigBuilderBuild(Constants.ChromosomeReads, true);
        }

        #endregion

        #region PaDeNAStep6:Step1:TestCases

        /// <summary>
        /// Validate paired reads for Seq Id Starts with ".."
        /// Input : X1,Y1 format map reads with sequence ID contains "..".
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6PairedReadsForSeqIDWithDots()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePairedReads(Constants.ReadsWithDotsNode);
        }

        /// <summary>
        /// Validate paired reads for Seq Id Starts with ".." between
        /// Input : X1,Y1 format map reads with sequence ID contains ".." between.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6PairedReadsForSeqIDWithDotsBetween()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePairedReads(Constants.ReadsWithDotsBetweenSeqIdNode);
        }

        /// <summary>
        /// Validate paired reads for Seq Id Chr1.X1:abc.X1:50K
        /// Input : X1,Y1 format map reads with sequence ID contains "X1 and Y1 letters" between.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6PairedReadsForSeqIDContainsX1Y1()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePairedReads(Constants.OneLineReadsForPairedReadsNode);
        }

        /// <summary>
        /// Validate paired reads for Seq Id with special characters
        /// Input : X1,Y1 format map reads with sequence ID contains "X1 and Y1 letters" between.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6PairedReadsForSpecialCharsSeqId()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePairedReads(Constants.ReadsWithSpecialCharsNode);
        }

        /// <summary>
        /// Validate paired reads for Mixed reads.
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6PairedReadsForMixedFormatReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePairedReads(Constants.ReadsWithDotsNode);
        }

        /// <summary>
        /// Validate paired reads for 2K and 0.5K library.
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6PairedReadsForDifferentLibrary()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePairedReads(Constants.ReadsWith2KlibraryNode);
        }

        /// <summary>
        /// Validate paired reads for 10K,50K and 100K library.
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6PairedReadsFor100kLibrary()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePairedReads(Constants.ReadsWith10KAnd50KAnd100KlibraryNode);
        }

        /// <summary>
        /// Validate paired reads for Reads without any Seq Name
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6PairedReadsForSeqsWithoutAnyID()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePairedReads(Constants.ReadsWithoutAnySeqIdNode);
        }

        /// <summary>
        /// Validate paired reads for Reads with numeric library name.
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6PairedReadsForNumericLibraryName()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidatePairedReads(Constants.ReadsWith10KAnd50KAnd100KlibraryNode);
        }

        /// <summary>
        /// Validate Adding new library information to library list.
        /// Input : Library name,Standard deviation and mean length.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6Libraryinformation()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.AddLibraryInformation(Constants.AddX1AndY1FormatPairedReadsNode, true);
        }

        /// <summary>
        /// Validate library information for 1 and 2 format paired reads.
        /// Input : 1 and 2 format paired reads.
        /// Output : Validate library information.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6GetLibraryinformation()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.GetLibraryInformation(Constants.GetLibraryInformationNode);
        }

        #endregion PaDeNAStep6:Step1:TestCases

        #region PaDeNAStep6:Step2:TestCases

        /// <summary>
        /// Validate ReadContigMapper.Map() using Contigs with small size
        /// chromosome.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        public void ValidatePaDeNAStep6MapReadsToContigForsmallSizeChromosome()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateMapReadsToContig(Constants.MapReadsToContigForSmallSizeChromosomeNode,
                true);
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using Contigs with Viral Genome reads.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        public void ValidatePaDeNAStep6MapReadsToContigForViralGenomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateMapReadsToContig(Constants.MapReadsToContigForViralGenomeNode,
                true);
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using multiple clustalW 
        /// contigs.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6MapReadsToContigForClustalW()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForClustalWContigsNode,
                true);
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using Reverse complement contig.
        /// contigs.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6MapReadsToContigForUsingReverseComplementContig()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForReverseComplementContigsNode,
                true);
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using left side contig generator.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6MapReadsToContigForLeftSideContigGenerator()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForLeftSideContigGeneratorNode,
                true);
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using Contigs generated by passing input 
        /// reads from sequence such that one read is sequence and another 
        /// read is reverse complement
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6MapReadsToContigForOneSeqReadAndOtherRevComp()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForSeqAndRevCompNode,
                false);
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using Right side contig generator.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6MapReadsToContigForRightSideGenerator()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForRightSideContigGeneratorNode,
                false);
        }

        #endregion PaDeNAStep6:Step2:TestCases

        #region PaDeNAStep6:Step3:TestCases

        /// <summary>
        /// Validate Contig build graphs Contig reads generated from viral genome
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate Contig graph.
        /// </summary>
        public void ValidatePaDeNAStep6ContigGrpahForViralGenomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateContigGraph(Constants.ContigGraphForViralGenomeReadsNode, false);
        }

        /// <summary>
        /// Validate Contig build graphs Contig reads generated from viral genome
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate Contig graph.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6ContigGraphForForwardAndReverseOrientation()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateContigGraph(Constants.ContigGraphForContigsWithBothOrientationNode, false);
        }

        /// <summary>
        /// Validate Contig build graph for Contig reads generated such that
        /// one of the Contigs should be palindrome
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate Contig graph.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6ContigGraphForPalindromeContig()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateContigGraph(Constants.ContigGraphForPalindromeContigsNode, true);
        }

        /// <summary>
        /// Validate Contig build graph for Contig reads for Chromosome reads.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate Contig graph.
        /// </summary>
        public void ValidatePaDeNAStep6ContigGraphForChromosomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateContigGraph(Constants.ContigGraphNodesForChromosomesNode, true);
        }

        #endregion PaDeNaStep6:Step3:TestCases

        #region PaDeNAStep6:Step4:TestCases

        /// <summary>
        /// Validate filter Contig Pairs formed in Forward direction with one 
        /// paired read does not support orientation.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6FilterPairedReadsWithFWReadsNotSupportOrientation()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateFilterPaired(Constants.FilterPairedReadContigsForFWOrnNode, true);
        }

        /// <summary>
        /// Validate filter Contig Pairs formed in Reverse direction with one paired
        /// read does not support orientation.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6FilterPairedReadsWithRevReadsNotSupportOrientation()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateFilterPaired(Constants.FilterPairedReadContigsForRevOrientationNode, true);
        }

        /// <summary>
        /// Validate filter Contig Pairs formed in Forward direction and reverse 
        /// complement of Contig
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6FilterPairedsForContigRevComplement()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateFilterPaired(
              Constants.FilterPairedReadContigsForFWDirectionWithRevCompContigNode, false);
        }

        /// <summary>
        /// Validate filter Contig Pairs formed in Backward direction
        /// and reverse complement of Contig
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6FilterPairedsForReverseReadAndRevComplement()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateFilterPaired(
              Constants.FilterPairedReadContigsForBackwardDirectionWithRevCompContig, true);
        }

        /// <summary>
        /// Validate filter Contig Pairs formed using small size chromosome reads.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        public void ValidatePaDeNAStep6FilterPairedReadsForChromosomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateFilterPaired(
              Constants.FilterPairedReadContigsForChromosomeReads, false);
        }

        /// <summary>
        /// Validate filter Contig Pairs formed using viral chromosome reads.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        public void ValidatePaDeNAStep6FilterPairedReadsForViralGenomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNAP1Test();
            testObj.ValidateFilterPaired(
              Constants.FilterPairedReadContigsForViralGenomeReads, false);
        }

        #endregion PaDeNaStep6:Step4:TestCases

        #region PaDeNAStep6:Step5:TestCases

        /// <summary>
        /// Calculate distance for Contig Pairs formed in Forward 
        /// direction with one paired read does not support orientation.
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6CalculateDistanceForForwardPairedReads()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateContigDistance(Constants.FilterPairedReadContigsForFWOrnNode);
        }

        /// <summary>
        /// Calculate distance for Contig Pairs formed in Forward 
        /// direction with one paired read does not support orientation.
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6CalculateDistanceForReversePairedReads()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateContigDistance(Constants.FilterPairedReadContigsForRevOrientationNode);
        }

        /// <summary>
        /// Calculate distance for Contig Pairs formed in Forward direction
        /// and reverse complement of Contig
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6CalculateDistanceForForwardPairedReadsWithRevCompl()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateContigDistance(
              Constants.FilterPairedReadContigsForFWDirectionWithRevCompContigNode);
        }

        /// <summary>
        /// Calculate distance for Contig Pairs formed in Reverse direction
        /// and reverse complement of Contig
        /// Input : 3-4 Line sequence reads.
        /// Output : Standard deviation and distance between contigs.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6CalculateDistanceForReversePairedReadsWithRevCompl()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateContigDistance(
              Constants.FilterPairedReadContigsForBackwardDirectionWithRevCompContig);
        }

        /// <summary>
        /// Calculate distance for Contig Pairs formed using small size
        /// chromosome reads
        /// Input : Chromosome reads.
        /// Output : Standard deviation and distance between contigs.
        /// </summary>
        public void ValidatePaDeNAStep6CalculateDistanceForChromosomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateContigDistance(
              Constants.FilterPairedReadContigsForChromosomeReads);
        }

        /// <summary>
        /// Calculate distance for Contig Pairs formed using viral genome
        /// reads
        /// Input : Viral genome reads.
        /// Output : Standard deviation and distance between contigs.
        /// </summary>
        public void ValidatePaDeNAStep6CalculateDistanceForViralGenomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateContigDistance(
              Constants.FilterPairedReadContigsForViralGenomeReads);
        }

        #endregion PaDeNAStep6:Step5:TestCases

        #region PaDeNAStep6:Step6:TestCases

        /// <summary>
        /// Validate scaffold path for Contig Pairs formed in Forward
        /// direction with all paired reads support orientation using
        /// FindPath(grpah;ContigPairedReads;KmerLength;Depth)
        /// Input : 3-4 Line sequence reads.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6ScaffoldPathsForForwardOrientation()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateScaffoldPath(
              Constants.ScaffoldPathWithForwardOrientationNode);
        }

        /// <summary>
        /// Validate scaffold path for Contig Pairs formed in Reverse
        /// direction with all paired reads support orientation using
        /// FindPath(grpah;ContigPairedReads;KmerLength;Depth).
        /// Input : 3-4 Line sequence reads.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6ScaffoldPathsForReverseOrientation()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateScaffoldPath(
              Constants.ScaffoldPathWithReverseOrientationNode);
        }

        /// <summary>
        /// Validate trace path for Contig Pairs formed in
        /// Forward direction and reverse complement of
        /// Contig
        /// Input : Forward read orientation and rev complement.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6ScaffoldPathsForForwardDirectionAndRevComp()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateScaffoldPath(
              Constants.ScaffoldPathWithForwardDirectionAndRevComp);
        }

        /// <summary>
        /// Validate trace path for Contig Pairs formed in
        /// Reverse direction and reverse complement of
        /// Contig
        /// Input : Reverse read orientation and rev complement.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6ScaffoldPathsForReverseDirectionAndRevComp()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateScaffoldPath(
              Constants.ScaffoldPathWithReverseDirectionAndRevComp);
        }

        /// <summary>
        /// Validate trace path for Contig Pairs formed in
        /// Forward direction and palindrome of
        /// Contig
        /// Input : Forward read orientation and palindrome of contig.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6ScaffoldPathsForForwardDirectionAndPalContig()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateScaffoldPath(
              Constants.ScaffoldPathWithForwardDirectionAndPalContig);
        }

        /// <summary>
        /// Validate trace path for Contig Pairs formed in
        /// Reverse direction and palindrome of
        /// Contig
        /// Input : Reverse read orientation and palindrome of contig.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6ScaffoldPathsForReverseDirectionAndPalContig()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateScaffoldPath(
              Constants.ScaffoldPathWithReverseDirectionAndPalContig);
        }

        /// <summary>
        /// Validate scaffold path for Contig Pairs formed using 
        /// small size chromosome reads
        /// Input : Chromosome reads.
        /// Output : Validation of scaffold paths.
        /// </summary>
        public void ValidatePaDeNAStep6ScaffoldPathsForChromosomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateScaffoldPath(
              Constants.ScaffoldPathForChromosomes);
        }

        /// <summary>
        /// Validate scaffold path for Contig Pairs formed using 
        /// viral genome reads.
        /// Input : Viral genome reads.
        /// Output : Validation of scaffold paths.
        /// </summary>
        public void ValidatePaDeNAStep6ScaffoldPathsForViralGenomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateScaffoldPath(
              Constants.ScaffoldPathForViralGenomeReads);
        }

        #endregion PaDeNAStep6:Step6:TestCases

        #region PaDeNAStep6:Step7/8:TestCases

        /// <summary>
        /// Validate assembled path by passing scaffold paths for
        /// Contig Pairs formed in Forward direction and reverse 
        /// complement of Contig
        /// Input : 3-4 Line sequence reads.
        /// Output : Assembled paths 
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6AssembledPathForForwardAndRevComplContig()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateAssembledPath(
              Constants.AssembledPathForForwardWithReverseCompl);
        }

        /// <summary>
        /// Validate assembled path by passing scaffold paths for
        /// Contig Pairs formed in Reverse direction and reverse 
        /// complement of Contig
        /// Input : 3-4 Line sequence reads.
        /// Output : Assembled paths 
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6AssembledPathForReverseAndRevComplContig()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateAssembledPath(
              Constants.AssembledPathForReverseWithReverseCompl);
        }

        /// <summary>
        /// Validate assembled path by passing scaffold for Contig Pairs 
        /// formed in Forward direction and palindrome of Contig
        /// Input : Sequence reads with Palindrome contigs.
        /// Output : Assembled paths 
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6AssembledPathForForwardAndPalContig()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateAssembledPath(
              Constants.AssembledPathForForwardAndPalContig);
        }

        /// <summary>
        /// Validate assembled path by passing scaffold for Contig Pairs 
        /// formed in Reverse direction and palindrome of Contig
        /// Input : Sequence reads with Palindrome contigs.
        /// Output : Assembled paths 
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6AssembledPathForReverseAndPalContig()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateAssembledPath(
              Constants.AssembledPathForReverseAndPalContig);
        }

        /// <summary>
        /// Validate assembled path by passing scaffold for
        /// Contig Pairs formed using small size chromosome reads
        /// Input : Chromosome reads.
        /// Output : Assembled paths 
        /// </summary>
        public void ValidatePaDeNAStep6AssembledPathForChromosomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateAssembledPath(
              Constants.AssembledPathForChromosomeReads);
        }

        /// <summary>
        /// Validate assembled path by passing scaffold for
        /// Contig Pairs formed using viral genome reads
        /// Input : Chromosome reads.
        /// Output : Assembled paths 
        /// </summary>
        public void ValidatePaDeNAStep6AssembledPathForViralGenomeReads()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidateAssembledPath(
              Constants.AssembledPathForViralGenomeReads);
        }

        /// <summary>
        /// Validate Scaffold sequence for small size sequence reads.
        /// Input : small size sequence reads.
        /// Output : Validation of Scaffold sequence.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6ScaffoldSequenceForSmallReads()
        {
            PaDeNAP1Test testObjObj = new PaDeNAP1Test();
            testObjObj.ValidateScaffoldSequence(Constants.ScaffoldSequenceNode);
        }

        /// <summary>
        ///  Validate Assembled sequences with Euler Test data reads,
        ///  Input : Euler testObj data seq reads.
        ///  output : Aligned sequences.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6AssembledSequenceWithEulerData()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidatePaDeNAAssembledSeqs(
                Constants.AssembledSequencesForEulerDataNode);
        }

        /// <summary>
        ///  Validate Assembled sequences for reads formed 
        ///  scaffold paths containing overlapping paths.
        ///  Input : Viral Genome reads.
        ///  output : Aligned sequences.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6AssembledSequenceForOverlappingScaffoldPaths()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidatePaDeNAAssembledSeqs(
                Constants.AssembledSequencesForViralGenomeReadsNode);
        }

        /// <summary>
        ///  Validate Assembled sequences for reads formed 
        ///  contigs in forward and reverse complement contig.
        ///  Input : Sequence reads.
        ///  output : Aligned sequences.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6AssembledSequenceForForwardAndRevCompl()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidatePaDeNAAssembledSeqs(
                Constants.AssembledSequencesForForwardAndRevComplContigNode);
        }

        /// <summary>
        ///  Validate Assembled sequences for reads formed 
        ///  contigs in forward and palindrome contig.
        ///  Input : Sequence reads.
        ///  output : Aligned sequences.
        /// </summary>
        [Test]
        public void ValidatePaDeNAStep6AssembledSequenceForForwardAndPalContig()
        {
            PaDeNAP1Test testObj = new PaDeNA.PaDeNAP1Test();
            testObj.ValidatePaDeNAAssembledSeqs(
                Constants.AssembledSequencesForForwardAndPalContigNode);
        }

        #endregion PaDeNAStep6:Step7/8:TestCases
    }

    /// <summary>
    /// This class contains helper methods for PaDeNA.
    /// </summary>
    internal class PaDeNAP1Test : ParallelDeNovoAssembler
    {
        #region Helper Methods

        /// <summary>
        /// Validate ParallelDeNovothis step1 Build kmers 
        /// </summary>
        /// <param name="nodeName">xml node for test data</param>
        /// <param name="IsSmallSize">Is file small size?</param>
        internal void ValidatePaDeNABuildKmers(string nodeName,
          bool IsSmallSize)
        {
            // Read all the input sequences from xml config file
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedKmersCount = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.ExpectedKmersCount);

            // Set kmerLength
            this.KmerLength = int.Parse(kmerLength);
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Set all the input reads and execute build kmers
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IEnumerable<KmersOfSequence> lstKmers =
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength);

            if (IsSmallSize)
            {
                Assert.AreEqual(expectedKmersCount, lstKmers.Count().ToString());
            }
            else
            {
                ValidateKmersList(new List<KmersOfSequence>(lstKmers), sequenceReads, nodeName);
            }

            Console.WriteLine(
              @"PaDeNA P1 : Validation of Build with all input reads using 
          ParallelDeNovothis completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 : Validation of Build with all input reads using 
          ParallelDeNovothis sequence completed successfully");
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
            string kmerOutputFile = Utility._xmlUtil.GetTextValue(nodeName,
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
            StreamReader kmerFile = new StreamReader(kmerOutputFile);
            int count = 0;
            string line = string.Empty;
            while (null != (line = kmerFile.ReadLine()))
            {
                Assert.AreEqual(line, aryKmer[count]);
                count++;
            }

            kmerFile.Close();
        }

        /// <summary>
        /// Validate graph generated using ParallelDeNovothis.CreateGraph() with kmers
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="isLargeSizeReads">Is large size reads?</param>
        internal void ValidatePaDeNABuildGraph(string nodeName, bool isLargeSizeReads)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedGraphsNodeCount = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.GraphNodesCountNode);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);
            this.KmerLength = int.Parse(kmerLength);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;

            Console.WriteLine("PaDeNA P1 : Step1,2 Completed Successfully");
            ApplicationLog.WriteLine("PaDeNA P1 : Step1,2 Completed Successfully");

            if (isLargeSizeReads)
                Assert.AreEqual(expectedGraphsNodeCount, graph.Nodes.Count.ToString());
            else
                ValidateGraph(graph, nodeName);

            Console.WriteLine(
              @"PaDeNA P1 : ParallelDeNovothis CreateGraph() 
          validation for PaDeNA step2 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 : ParallelDeNovothis CreateGraph() 
          validation for PaDeNA step2 completed successfully");
        }

        /// <summary>
        /// Validate the graph nodes sequence, left edges and right edges
        /// </summary>
        /// <param name="graph">graph object</param>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateGraph(DeBruijnGraph graph, string nodeName)
        {
            string nodesSequence = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.NodesSequenceNode);
            string nodesLeftEdges = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.NodesLeftEdgesCountNode);
            string nodesRightEdges = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.NodeRightEdgesCountNode);

            string[] leftEdgesCount =
              ReadStringFromFile(nodesLeftEdges).Replace("\r\n", "").Split(',');
            string[] rightEdgesCount =
              ReadStringFromFile(nodesRightEdges).Replace("\r\n", "").Split(',');
            string[] nodesSequences =
              ReadStringFromFile(nodesSequence).Replace("\r\n", "").Split(',');

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
                  dbnodes.LeftExtensionNodes.Count.ToString() == leftEdgesCount[iseq] ||
                  dbnodes.LeftExtensionNodes.Count.ToString() == rightEdgesCount[iseq]);
                Assert.IsTrue(
                  dbnodes.RightExtensionNodes.Count.ToString() == leftEdgesCount[iseq] ||
                  dbnodes.RightExtensionNodes.Count.ToString() == rightEdgesCount[iseq]);
            }
        }

        /// <summary>
        /// Get the input string from the file.
        /// </summary>
        /// <param name="filename">input filename</param>
        /// <returns>Reads the file and returns input string</returns>
        string ReadStringFromFile(string filename)
        {
            StreamReader reader = new StreamReader(filename);
            string readString = reader.ReadToEnd();
            reader.Close();

            return readString;
        }

        /// <summary>
        /// Validate ParallelDeNovothis.RemoveRedundancy() which removes bubbles formed in the graph
        /// and validate the graph
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="defaultThreshold">Is Default Threshold?</param>
        /// <param name="isMicroorganism">Is micro organsm?</param>
        internal void ValidatePaDeNARemoveRedundancy(string nodeName,
          bool defaultThreshold, bool isMicroorganism)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedNodesCount = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedNodesCountRemoveRedundancy);

            string danglingThreshold = null;
            string pathlengthThreshold = null;
            if (!defaultThreshold)
            {
                danglingThreshold = Utility._xmlUtil.GetTextValue(nodeName,
                  Constants.DanglingLinkThresholdNode);
                pathlengthThreshold = Utility._xmlUtil.GetTextValue(nodeName,
                  Constants.PathLengthThresholdNode);
            }

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles from graph in step4
            // Validate the graph
            if (!defaultThreshold)
            {
                this.DanglingLinksThreshold = int.Parse(danglingThreshold);
                this.DanglingLinksPurger =
                  new DanglingLinksPurger(this.DanglingLinksThreshold);
                this.RedundantPathLengthThreshold = int.Parse(pathlengthThreshold);
                this.RedundantPathsPurger =
                  new RedundantPathsPurger(this.RedundantPathLengthThreshold);
            }
            else
            {
                this.DanglingLinksPurger =
                  new DanglingLinksPurger(int.Parse(kmerLength));
                this.RedundantPathsPurger =
                  new RedundantPathsPurger(int.Parse(kmerLength) + 1);
            }
            this.KmerLength = int.Parse(kmerLength);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;
            Console.WriteLine("PaDeNA P1 : Step1,2 Completed Successfully");
            ApplicationLog.WriteLine("PaDeNA P1 : Step1,2 Completed Successfully");
            this.UnDangleGraph();
            Console.WriteLine("PaDeNA P1 : Step3 Completed Successfully");
            ApplicationLog.WriteLine("PaDeNA P1 : Step3 Completed Successfully");
            this.RemoveRedundancy();
            Console.WriteLine("PaDeNA P1 : Step4 Completed Successfully");
            ApplicationLog.WriteLine("PaDeNA P1 : Step4 Completed Successfully");
            if (isMicroorganism)
            {
                Assert.AreEqual(expectedNodesCount, graph.Nodes.Count.ToString());
            }
            else
            {
                ValidateGraph(graph, nodeName);
            }

            Console.WriteLine(
              @"PaDeNA P1 : ParallelDeNovothis.RemoveRedundancy() validation 
          for PaDeNA step4 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 :ParallelDeNovothis.RemoveRedundancy() validation 
          for PaDeNA step4 completed successfully");
        }

        /// <summary>
        /// Validate the ParallelDeNovothis unDangleGraph() method which removes the dangling link
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="defaultThreshold">Default Threshold</param>
        /// <param name="smallSizeChromosome">Small size chromosome</param>
        internal void ValidatePaDeNAUnDangleGraph(string nodeName, bool defaultThreshold,
          bool smallSizeChromosome)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedNodesCount = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.NodesCountAfterDanglingGraphNode);
            string danglingThreshold = null;
            if (!defaultThreshold)
                danglingThreshold = Utility._xmlUtil.GetTextValue(nodeName,
                  Constants.DanglingLinkThresholdNode);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            this.KmerLength = int.Parse(kmerLength);
            if (!defaultThreshold)
            {
                this.DanglingLinksThreshold = int.Parse(danglingThreshold);
            }
            else
            {
                this.DanglingLinksThreshold = int.Parse(kmerLength) + 1;
            }
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;
            Console.WriteLine("PaDeNA P1 : Step1,2 Completed Successfully");
            ApplicationLog.WriteLine("PaDeNA P1 : Step1,2 Completed Successfully");
            this.DanglingLinksPurger = new DanglingLinksPurger(this.DanglingLinksThreshold);
            this.UnDangleGraph();
            Console.WriteLine("PaDeNA P1 : Step3 Completed Successfully");
            ApplicationLog.WriteLine("PaDeNA P1 : Step3 Completed Successfully");
            if (smallSizeChromosome)
            {
                Assert.AreEqual(expectedNodesCount, graph.Nodes.Count.ToString());
            }
            else
            {
                ValidateGraph(graph, nodeName);
            }
            Console.WriteLine(
              @"PaDeNA P1 : ParallelDeNovothis.UndangleGraph() validation 
          for PaDeNA step3 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 :ParallelDeNovothis.UndangleGraph() validation 
          for PaDeNA step3 completed successfully");
        }

        /// <summary>
        /// Validate KmersOfSequence ctor by passing base sequence reads, kmer length and
        /// built kmers
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        internal void ValidateKmersOfSequenceCtorWithBuildKmers(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

            // Get the input reads
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);
            SequenceToKmerBuilder builder = new SequenceToKmerBuilder();
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>();

            // Validate KmersOfSequence ctor using build kmers
            foreach (ISequence sequence in sequenceReads)
            {
                KmersOfSequence kmer = builder.Build(sequence, int.Parse(kmerLength));
                KmersOfSequence kmerSequence = new KmersOfSequence(sequence,
                  int.Parse(kmerLength), kmer.Kmers);
                lstKmers.Add(kmerSequence);
            }

            ValidateKmersList(lstKmers, sequenceReads, nodeName);

            Console.WriteLine(
              @"PaDeNA P1 : KmersOfSequence ctor with 
          build kmers validation completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 : KmersOfSequence ctor with 
          build kmers method validation completed successfully");
        }

        /// <summary>
        /// Validate KmersOfSequence ctor by passing base sequence reads, kmer length and
        /// built kmers and validate its properties.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        internal void ValidateKmersOfSequenceCtorProperties(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.BaseSequenceNode);
            string expectedKmers = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmersCountNode);

            // Get the input reads
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);
            SequenceToKmerBuilder builder = new SequenceToKmerBuilder();

            KmersOfSequence kmer = builder.Build(sequenceReads[0],
              int.Parse(kmerLength));
            KmersOfSequence kmerSequence = new KmersOfSequence(sequenceReads[0],
              int.Parse(kmerLength), kmer.Kmers);

            // Validate KmerOfSequence properties.
            Assert.AreEqual(expectedSeq, kmerSequence.BaseSequence.ToString());
            Assert.AreEqual(expectedKmers, kmerSequence.Kmers.Count.ToString());

            Console.WriteLine(
              @"PaDeNA P1 : KmersOfSequence ctor with build kmers 
          validation completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 : KmersOfSequence ctor with build kmers method 
          validation completed successfully");
        }

        /// <summary>
        /// Validate SequenceToKmerBuilder Build() method which build kmers
        /// </summary>
        /// <param name="nodeName">xml node name for test data</param>
        /// <param name="IsListSeq">Is list sequence</param>
        internal void ValidateKmerBuilderBuild(string nodeName, bool IsListSeq)
        {
            string filePath = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.KmerLengthNode);

            // Get the input reads
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Pass all the input reads and kmerLength to generate kmers
            SequenceToKmerBuilder builder = new SequenceToKmerBuilder();
            IList<KmersOfSequence> lstKmers;

            if (IsListSeq)
            {
                lstKmers = new List<KmersOfSequence>(builder.Build(sequenceReads,
                  int.Parse(kmerLength)));
            }
            else
            {
                lstKmers = new List<KmersOfSequence>();
                foreach (ISequence sequence in sequenceReads)
                {
                    lstKmers.Add(builder.Build(sequence, int.Parse(kmerLength)));
                }
            }

            // Validate kmers list
            ValidateKmersList(lstKmers, sequenceReads, nodeName);

            Console.WriteLine(
              @"PaDeNA P1 : Validation of Build with all input reads using ParallelDeNovo 
          completed successfully");
            ApplicationLog.WriteLine(
              "PaDeNA P1 : Validation of Build with all input reads sequence completed successfully");
        }

        /// <summary>
        /// Validate KmersOfSequence ToSequences() method which returns kmers sequence
        /// using its positions
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateKmersOfSequenceToSequences(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            this.KmerLength = int.Parse(kmerLength);
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
                    int.Parse(kmerLength), lstKmers[index].Kmers);
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
            string kmerOutputFile = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmersOutputFileNode);
            StreamReader kmerFile = new StreamReader(kmerOutputFile);
            int count = 0;
            string line = string.Empty;
            while (null != (line = kmerFile.ReadLine()))
            {
                Assert.AreEqual(line, aryKmerSequence[count]);
                count++;
            }
            kmerFile.Close();

            Console.WriteLine(
              "PaDeNA P1 : KmersOfSequence ToSequences() method validation completed successfully");
            ApplicationLog.WriteLine(
              "PaDeNA P1 : KmersOfSequence ToSequences() method validation completed successfully");
        }

        /// <summary>
        /// Validate Validate DeBruijinGraph properties
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijinGraphproperties(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string ExpectedNodesCount = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.GraphNodesCountNode);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            this.KmerLength = int.Parse(kmerLength);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;

            Console.WriteLine("PaDeNA P1 : Step1,2 Completed Successfully");
            ApplicationLog.WriteLine("PaDeNA P1 : Step1,2 Completed Successfully");

            // Validate DeBruijnGraph Properties.
            Assert.AreEqual(ExpectedNodesCount, graph.Nodes.Count.ToString());

            Console.WriteLine(
              @"PaDeNA P1 : ParallelDeNovothis CreateGraph() validation for 
          PaDeNA step2 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 : ParallelDeNovothis CreateGraph() validation for 
          PaDeNA step2 completed successfully");
        }

        /// <summary>
        /// Validate AddLeftEndExtension() method of DeBruijnNode 
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnNodeAddLeftExtension(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1
            this.KmerLength = int.Parse(kmerLength);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength));

            // Create node and add left node.
            DeBruijnNode node = new DeBruijnNode(int.Parse(kmerLength), 0,
              lstKmers[0].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            DeBruijnNode leftnode = new DeBruijnNode(int.Parse(kmerLength), 1,
              lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            node.AddLeftEndExtension(leftnode, true);

            Assert.AreEqual(lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Count,
              node.LeftExtensionNodes.Count);
            Console.WriteLine(
              @"PaDeNA P1 : DeBruijnNode AddLeftExtension() validation for 
          PaDeNA step2 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 :DeBruijnNode AddLeftExtension() validation for 
          PaDeNA step2 completed successfully");
        }

        /// <summary>
        /// Validate AddRightEndExtension() method of DeBruijnNode 
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnNodeAddRightExtension(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1
            this.KmerLength = int.Parse(kmerLength);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength));

            // Create node and add right node.
            DeBruijnNode node = new DeBruijnNode(int.Parse(kmerLength), 0,
              lstKmers[0].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            DeBruijnNode rightnode = new DeBruijnNode(int.Parse(kmerLength), 1,
              lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            node.AddRightEndExtension(rightnode, false);

            Assert.AreEqual(lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Count,
              node.RightExtensionNodes.Count);
            Console.WriteLine(
              @"PaDeNA P1 : DeBruijnNode AddRightExtension() validation for 
          PaDeNA step2 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 :DeBruijnNode AddRightExtension() validation for 
          PaDeNA step2 completed successfully");
        }

        /// <summary>
        /// Validate RemoveExtension() method of DeBruijnNode 
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnNodeRemoveExtension(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1

            this.KmerLength = int.Parse(kmerLength);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength));

            // Create node and add right node.
            DeBruijnNode node = new DeBruijnNode(int.Parse(kmerLength), 0,
              lstKmers[0].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            DeBruijnNode rightnode = new DeBruijnNode(int.Parse(kmerLength), 1,
              lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            DeBruijnNode leftnode = new DeBruijnNode(int.Parse(kmerLength), 1,
              lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            node.AddRightEndExtension(rightnode, false);
            node.AddLeftEndExtension(leftnode, false);

            // Validates count before removing right and left extension nodes.
            Assert.AreEqual(lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Count,
              node.RightExtensionNodes.Count);
            Assert.AreEqual(1, node.RightExtensionNodes.Count);
            Assert.AreEqual(1, node.LeftExtensionNodes.Count);

            // Remove right and left extension nodes.
            node.RemoveExtensionThreadSafe(rightnode);
            node.RemoveExtensionThreadSafe(leftnode);

            // Validate node after removing right and left extensions.
            Assert.AreEqual(0, node.RightExtensionNodes.Count);
            Assert.AreEqual(0, node.LeftExtensionNodes.Count);
            Console.WriteLine(
              @"PaDeNA P1 : DeBruijnNode AddRightExtension() validation for 
          PaDeNA step2 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 :DeBruijnNode AddRightExtension() validation for 
          PaDeNA step2 completed successfully");
        }

        /// <summary>
        /// Validate the DeBruijnNode ctor by passing the kmer and validating 
        /// the node object.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnNodeCtor(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string nodeExtensionsCount = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.NodeExtensionsCountNode);
            string kmersCount = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmersCountNode);
            string leftNodeExtensionCount = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.LeftNodeExtensionsCountNode);
            string rightNodeExtensionCount = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.RightNodeExtensionsCountNode);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build the kmers using this
            this.KmerLength = int.Parse(kmerLength);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength));

            // Validate the node creation
            DeBruijnNode node = new DeBruijnNode(int.Parse(kmerLength), 0,
              lstKmers[0].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            DeBruijnNode rightnode = new DeBruijnNode(int.Parse(kmerLength), 1,
              lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            DeBruijnNode leftnode = new DeBruijnNode(int.Parse(kmerLength), 1,
              lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            node.AddRightEndExtension(rightnode, false);
            node.AddLeftEndExtension(leftnode, false);

            // Validate DeBruijnNode class properties.
            Assert.AreEqual(nodeExtensionsCount, node.ExtensionsCount.ToString());
            Assert.AreEqual(kmersCount, node.KmerCount.ToString());
            Assert.AreEqual(leftNodeExtensionCount, node.LeftExtensionNodes.Count.ToString());
            Assert.AreEqual(rightNodeExtensionCount, node.RightExtensionNodes.Count.ToString());
            Assert.AreEqual(kmerLength, node.KmerLength.ToString());
            Assert.AreEqual(leftNodeExtensionCount, node.LeftExtensionNodes.Count.ToString());
            Assert.AreEqual(rightNodeExtensionCount, node.RightExtensionNodes.Count.ToString());

            Console.WriteLine(
              "PaDeNA P1 : DeBruijnNode ctor() validation for PaDeNA step2 completed successfully");
            ApplicationLog.WriteLine(
              "PaDeNA P1 : DeBruijnNode ctor() validation for PaDeNA step2 completed successfully");
        }

        /// <summary>
        /// Validate graph generated using DeBruijnGraph.CreateGraph() with kmers
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateDeBruijnGraphBuild(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            this.KmerLength = int.Parse(kmerLength);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            DeBruijnGraph graph = new DeBruijnGraph();
            graph.Build(this.SequenceReads, this.KmerLength);

            ValidateGraph(graph, nodeName);
            Console.WriteLine(
              @"PaDeNA P1 : DeBruijnGraph Build() validation for 
          PaDeNA step2 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 : DeBruijnGraph Build() validation for 
          PaDeNA step2 completed successfully");
        }

        /// <summary>
        /// Validate RemoveErrorNodes() method is removing dangling nodes from the graph
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidatePaDeNARemoveErrorNodes(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            // and remove the dangling links from graph in step3
            // Validate the graph
            this.KmerLength = int.Parse(kmerLength);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;

            // Find the dangling nodes and remove the dangling node
            DanglingLinksPurger danglingLinksPurger =
              new DanglingLinksPurger(int.Parse(kmerLength) + 1);
            DeBruijnPathList danglingnodes = danglingLinksPurger.DetectErroneousNodes(graph);
            danglingLinksPurger.RemoveErroneousNodes(graph, danglingnodes);
            Assert.IsFalse(graph.Nodes.Contains(danglingnodes.Paths[0].PathNodes[0]));

            Console.WriteLine(
              @"PaDeNA P1 : DeBruijnGraph.RemoveErrorNodes() validation for 
          PaDeNA step3 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 :DeBruijnGraph.RemoveErrorNodes() validation for 
          PaDeNA step3 completed successfully");
        }

        /// <summary>
        /// Creates RedundantPathPurger instance by passing pathlength and count. Detect 
        /// redundant error nodes and remove these nodes from the graph. Validate the graph.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="isMicroOrganism">Is micro organism</param>    
        internal void ValidateRedundantPathPurgerCtor(string nodeName, bool isMicroOrganism)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);
            string expectedNodesCount = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.ExpectedNodesCountAfterDangling);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Validate the graph
            this.KmerLength = int.Parse(kmerLength);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;
            this.DanglingLinksPurger = new DanglingLinksPurger(this.KmerLength);
            this.UnDangleGraph();

            // Create RedundantPathPurger instance, detect redundant nodes and remove error nodes
            RedundantPathsPurger redundantPathPurger =
              new RedundantPathsPurger(int.Parse(kmerLength) + 1);
            DeBruijnPathList redundantnodelist = redundantPathPurger.DetectErroneousNodes(graph);
            redundantPathPurger.RemoveErroneousNodes(graph, redundantnodelist);

            if (isMicroOrganism)
                Assert.AreEqual(expectedNodesCount, graph.Nodes.Count);
            else
                ValidateGraph(graph, nodeName);

            Console.WriteLine(
              @"PaDeNA P1 : RedundantPathsPurger ctor and methods validation for 
          PaDeNA step4 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 :RedundantPathsPurger ctor and methods validation for 
          PaDeNA step4 completed successfully");
        }

        /// <summary>
        /// Validate ParallelDeNovothis.BuildContigs() by passing graph object
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="isChromosomeRC">Is chromosome RC?</param>
        internal void ValidateDe2thisBuildContigs(string nodeName, bool isChromosomeRC)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedContigsString = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.ContigsNode);
            string[] expectedContigs;
            if (!expectedContigsString.Contains("PaDeNATestData"))
                expectedContigs = expectedContigsString.Split(',');
            else
                expectedContigs =
                  ReadStringFromFile(expectedContigsString).Replace("\r\n", "").Split(',');

            string expectedContigsCount = Utility._xmlUtil.GetTextValue(nodeName, Constants.ContigsCount);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate the contigs
            this.KmerLength = int.Parse(kmerLength);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.DanglingLinksPurger = new DanglingLinksPurger(this.KmerLength);
            this.UnDangleGraph();
            this.RedundantPathsPurger = new RedundantPathsPurger(this.KmerLength + 1);
            this.RemoveRedundancy();
            this.ContigBuilder = new SimplePathContigBuilder();
            IList<ISequence> contigs = this.BuildContigs();

            // Validate contigs count only for Chromosome files. 
            if (isChromosomeRC)
            {
                Assert.AreEqual(expectedContigsCount, contigs.Count.ToString());
            }
            // validate all contigs of a sequence.
            else
            {
                for (int index = 0; index < contigs.Count; index++)
                {
                    Assert.IsTrue(expectedContigs.Contains(contigs[index].ToString()) ||
                      expectedContigs.Contains(contigs[index].ReverseComplement.ToString()));
                }
            }

            Console.WriteLine(
              @"PaDeNA P1 : ParallelDeNovothis.BuildContigs() validation for 
          PaDeNA step5 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 :ParallelDeNovothis.BuildContigs() validation for 
          PaDeNA step5 completed successfully");
        }

        /// <summary>
        /// Validate the SimpleContigBuilder Build() method using step 4 graph
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="isChromosomeRC">Is Chromosome RC?</param>
        internal void ValidateSimpleContigBuilderBuild(string nodeName, bool isChromosomeRC)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedContigsString = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.ContigsNode);
            string[] expectedContigs = expectedContigsString.Split(',');
            string expectedContigsCount = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.ContigsCount);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles from graph in step4
            this.KmerLength = int.Parse(kmerLength);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;
            this.UnDangleGraph();
            this.RemoveRedundancy();

            // Validate the SimpleContigBuilder.Build() by passing graph
            SimplePathContigBuilder builder = new SimplePathContigBuilder();
            IList<ISequence> contigs = builder.Build(graph);

            if (isChromosomeRC)
            {
                Assert.AreEqual(expectedContigsCount, contigs.Count.ToString());
            }
            else
            {
                // Validate the contigs
                for (int index = 0; index < contigs.Count; index++)
                {
                    Assert.IsTrue(expectedContigs.Contains(contigs[index].ToString()));
                }
            }
            Console.WriteLine(
              @"PaDeNA P1 : SimpleContigBuilder.BuildContigs() validation for 
          PaDeNA step5 completed successfully");
            ApplicationLog.WriteLine(
              @"PaDeNA P1 :SimpleContigBuilder.BuildContigs() validation for 
          PaDeNA step5 completed successfully");
        }

        /// <summary>
        /// Validate Map paired reads for a sequence reads.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidatePairedReads(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string expectedPairedReadsCount = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.PairedReadsCountNode);
            string[] backwardReadsNode = Utility._xmlUtil.GetTextValues(nodeName,
              Constants.BackwardReadsNode);
            string[] forwardReadsNode = Utility._xmlUtil.GetTextValues(nodeName,
              Constants.ForwardReadsNode);
            string[] expectedLibrary = Utility._xmlUtil.GetTextValues(nodeName,
              Constants.LibraryNode);
            string[] expectedMean = Utility._xmlUtil.GetTextValues(nodeName,
              Constants.MeanLengthNode);
            string[] deviationNode = Utility._xmlUtil.GetTextValues(nodeName,
              Constants.DeviationNode);

            IList<ISequence> sequenceReads = new List<ISequence>();
            IList<MatePair> pairedreads = new List<MatePair>();

            // Get the input reads 
            FastaParser parser = new FastaParser();
            IList<ISequence> sequences = parser.Parse(filePath);

            foreach (ISequence seq in sequences)
            {
                sequenceReads.Add(seq);
            }

            // Convert reads to map paired reads.
            MatePairMapper pair = new MatePairMapper();
            pairedreads = pair.Map(sequenceReads);

            // Validate Map paired reads.
            Assert.AreEqual(expectedPairedReadsCount, pairedreads.Count.ToString());

            for (int index = 0; index < pairedreads.Count; index++)
            {
                Assert.IsTrue(forwardReadsNode.Contains(pairedreads[index].GetForwardRead(sequenceReads).ToString()));
                Assert.IsTrue(backwardReadsNode.Contains(pairedreads[index].GetReverseRead(sequenceReads).ToString()));
                Assert.IsTrue(
                  deviationNode.Contains(pairedreads[index].StandardDeviationOfLibrary.ToString()));
                Assert.IsTrue(expectedMean.Contains(pairedreads[index].MeanLengthOfLibrary.ToString()));
                Assert.IsTrue(expectedLibrary.Contains(pairedreads[index].Library.ToString()));
            }

            Console.WriteLine(@"PaDeNA P1 : Map paired reads has been verified successfully");
            ApplicationLog.WriteLine(@"PaDeNA P1 : Map paired reads has been verified successfully");
        }

        /// <summary>
        /// Validate library information
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void GetLibraryInformation(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string expectedPairedReadsCount = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.PairedReadsCountNode);
            string expectedLibraray = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.LibraryName);
            string expectedStdDeviation = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.StdDeviation);
            string mean = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.Mean);

            IList<ISequence> sequenceReads = new List<ISequence>();
            IList<MatePair> pairedreads = new List<MatePair>();

            // Get the input reads 
            FastaParser parser = new FastaParser();
            IList<ISequence> sequences = parser.Parse(filePath);

            foreach (ISequence seq in sequences)
            {
                sequenceReads.Add(seq);
            }

            // Convert reads to map paired reads.
            MatePairMapper pair = new MatePairMapper();
            pairedreads = pair.Map(sequenceReads);

            // Validate Map paired reads.
            Assert.AreEqual(expectedPairedReadsCount,
              pairedreads.Count.ToString());

            // Get library infomration and validate
            CloneLibraryInformation libraryInfo =
              CloneLibrary.Instance.GetLibraryInformation
              (pairedreads[0].Library);

            Assert.AreEqual(expectedStdDeviation, libraryInfo.StandardDeviationOfInsert.ToString());
            Assert.AreEqual(expectedLibraray, libraryInfo.LibraryName.ToString());
            Assert.AreEqual(mean, libraryInfo.MeanLengthOfInsert.ToString());

            Console.WriteLine(
             @"PaDeNA P1 : Map paired reads has been verified successfully");
            ApplicationLog.WriteLine(
             @"PaDeNA P1 : Map paired reads has been verified successfully");
        }

        /// <summary>
        /// Validate Add library information in existing libraries.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="IsLibraryInfo">Is library info?</param>
        internal void AddLibraryInformation(string nodeName, bool IsLibraryInfo)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string expectedPairedReadsCount = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.PairedReadsCountNode);
            string[] backwardReadsNode = Utility._xmlUtil.GetTextValues(nodeName,
              Constants.BackwardReadsNode);
            string[] forwardReadsNode = Utility._xmlUtil.GetTextValues(nodeName,
              Constants.ForwardReadsNode);
            string expectedLibraray = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.LibraryName);
            string expectedStdDeviation = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.StdDeviation);
            string mean = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.Mean);

            IList<ISequence> sequenceReads = new List<ISequence>();
            IList<MatePair> pairedreads = new List<MatePair>();

            // Get the input reads 
            FastaParser parser = new FastaParser();
            IList<ISequence> sequences = parser.Parse(filePath);

            foreach (ISequence seq in sequences)
            {
                sequenceReads.Add(seq);
            }

            // Add a new library infomration.
            if (IsLibraryInfo)
            {
                CloneLibraryInformation libraryInfo =
                  new CloneLibraryInformation();
                libraryInfo.LibraryName = expectedLibraray;
                libraryInfo.MeanLengthOfInsert = float.Parse(mean);
                libraryInfo.StandardDeviationOfInsert = float.Parse(expectedStdDeviation);
                CloneLibrary.Instance.AddLibrary(libraryInfo);
            }
            else
            {
                CloneLibrary.Instance.AddLibrary(expectedLibraray,
                    float.Parse(mean), float.Parse(expectedStdDeviation));
            }

            // Convert reads to map paired reads.
            MatePairMapper pair = new MatePairMapper();
            pairedreads = pair.Map(sequenceReads);

            // Validate Map paired reads.
            Assert.AreEqual(expectedPairedReadsCount, pairedreads.Count.ToString());

            for (int index = 0; index < pairedreads.Count; index++)
            {
                Assert.IsTrue(forwardReadsNode.Contains(pairedreads[index].GetForwardRead(sequenceReads).ToString()));
                Assert.IsTrue(backwardReadsNode.Contains(pairedreads[index].GetReverseRead(sequenceReads).ToString()));
                Assert.AreEqual(expectedStdDeviation,
                  pairedreads[index].StandardDeviationOfLibrary.ToString());
                Assert.AreEqual(expectedLibraray, pairedreads[index].Library.ToString());
                Assert.AreEqual(mean, pairedreads[index].MeanLengthOfLibrary.ToString());
            }

            Console.WriteLine(@"PaDeNA P1 : Map paired reads has been verified successfully");
            ApplicationLog.WriteLine(@"PaDeNA P1 : Map paired reads has been verified successfully");
        }

        /// <summary>
        /// Validate building map reads to contigs.
        /// </summary>
        /// <param name="nodeName">xml node name used for a different testcases</param>
        /// <param name="IsFullOverlap">True if full overlap else false</param>
        internal void ValidateMapReadsToContig(string nodeName, bool IsFullOverlap)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string daglingThreshold = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.DanglingLinkThresholdNode);
            string redundantThreshold = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.RedundantThreshold);
            string readMapLengthString = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.ReadMapLength);
            string readStartPosString = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.ReadStartPos);
            string contigStartPosString = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.ContigStartPos);
            string[] expectedReadmapLength = readMapLengthString.Split(',');
            string[] expectedReadStartPos = readStartPosString.Split(',');
            string[] expectedContigStartPos = contigStartPosString.Split(',');

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            this.KmerLength = Int32.Parse(kmerLength);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold);
            this.DanglingLinksPurger =
              new DanglingLinksPurger(Int32.Parse(daglingThreshold));
            this.RedundantPathsPurger =
              new RedundantPathsPurger(Int32.Parse(redundantThreshold));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold);
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

            for (int i = 0; i < SortContigsData(readMaps.Keys.ToList()).Count; i++)
            {
                readMap = readMaps[SortContigsData(readMaps.Keys.ToList())[i]];

                if (IsFullOverlap)
                {
                    Assert.AreEqual(expectedReadmapLength[i], readMap[0].Length.ToString());
                    Assert.AreEqual(expectedContigStartPos[i],
                        readMap[0].StartPositionOfContig.ToString());
                    Assert.AreEqual(expectedReadStartPos[i], readMap[0].StartPositionOfRead.ToString());
                    Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);
                }
                else
                {
                    Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.PartialOverlap);
                    break;
                }
            }

            Console.WriteLine(
              "PADENA P1 : ReadContigMapper.Map() validation for PaDeNA step6:step2 completed successfully");
            ApplicationLog.WriteLine(
              "PADENA P1 :ReadContigMapper.Map() validation for PaDeNA step6:step2 completed successfully");
        }

        /// <summary>
        /// Validate build contig graphs for contigs.
        /// </summary>
        /// <param name="nodeName">xml node name used for a different testcase.</param>
        internal void ValidateContigGraph(string nodeName, bool IsChromosome)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string daglingThreshold = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.RedundantThreshold);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            this.KmerLength = Int32.Parse(kmerLength);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold);
            this.DanglingLinksPurger =
              new DanglingLinksPurger(Int32.Parse(daglingThreshold));
            this.RedundantPathsPurger =
              new RedundantPathsPurger(Int32.Parse(redundantThreshold));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();

            // Build contig.
            this.ContigBuilder = new SimplePathContigBuilder();
            this.RemoveRedundancy();
            IList<ISequence> contigs = this.BuildContigs();

            // Build contig graph.
            this.Graph.BuildContigGraph(contigs, this.KmerLength);

            int contigGraphCount = this.Graph.Nodes.Count;

            // Validate contig graph.
            if (IsChromosome)
            {
                Assert.AreEqual(contigGraphCount, contigs.Count);
            }
            else
            {
                Assert.AreEqual(contigGraphCount, contigs.Count);
                ValidateGraph(this.Graph, nodeName);
            }
            Console.WriteLine(
              "PADENA P1 : BuildContigGraph() validation for PaDeNA step6:step3 completed successfully");
            ApplicationLog.WriteLine(
              "PADENA P1 :BuildContigGraph validation for PaDeNA step6:step3 completed successfully");
        }

        /// <summary>
        /// Validate Filter contig nodes.
        /// </summary>
        /// <param name="nodeName">xml node name used for a differnt testcase.</param>
        /// <param name="isRedundancy">True if passing redundancy, else false</param>
        /// <param name="isFirstContig">Is First Contig?</param>
        internal void ValidateFilterPaired(string nodeName, bool isFirstContig)
        {
            string filePath = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.KmerLengthNode);
            string daglingThreshold = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.RedundantThreshold);
            string expectedContigPairedReadsCount = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ContigPairedReadsCount);
            string forwardReadStartPos = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ForwardReadStartPos);
            string reverseReadStartPos = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ReverseReadStartPos);
            string reverseComplementStartPos = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.RerverseReadReverseCompPos);
            string[] expectedForwardReadStartPos = forwardReadStartPos.Split(',');
            string[] expectedReverseReadStartPos = reverseReadStartPos.Split(',');
            string[] expectedReverseComplementStartPos = reverseComplementStartPos.Split(',');

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            this.KmerLength = Int32.Parse(kmerLength);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold);
            this.DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold));
            this.RedundantPathsPurger =
              new RedundantPathsPurger(Int32.Parse(redundantThreshold));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold);
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

            ReadContigMap maps = mapper.Map(
                sortedContigs, sequenceReads, this.KmerLength);

            // Find map paired reads.
            MatePairMapper mapPairedReads = new MatePairMapper();
            ContigMatePairs pairedReads = mapPairedReads.MapContigToMatePairs(sequenceReads, maps);

            // Filter contigs based on the orientation.
            OrientationBasedMatePairFilter filter = new OrientationBasedMatePairFilter();
            ContigMatePairs contigpairedReads = filter.FilterPairedReads(pairedReads, 0);


            Assert.AreEqual(expectedContigPairedReadsCount,
              contigpairedReads.Values.Count.ToString());

            Dictionary<ISequence, IList<ValidMatePair>> map = null;
            IList<ValidMatePair> valid = null;

            // Validate Contig paired reads after filtering contig sequences.
            if (isFirstContig)
            {
                map = contigpairedReads[sortedContigs[0]];
                valid = SortPairedReads(map[sortedContigs[1]], sequenceReads);
            }
            else
            {
                map = contigpairedReads[sortedContigs[1]];
                valid = SortPairedReads(map[sortedContigs[0]], sequenceReads);
            }

            for (int index = 0; index < valid.Count; index++)
            {
                Assert.AreEqual(expectedForwardReadStartPos[index],
                  valid[index].ForwardReadStartPosition[0].ToString());
                Assert.AreEqual(expectedReverseReadStartPos[index],
                  valid[index].ReverseReadStartPosition[0].ToString());
                Assert.AreEqual(expectedReverseComplementStartPos[index],
                  valid[index].ReverseReadReverseComplementStartPosition[0].ToString());

            }
            Console.WriteLine(
              "PADENA P1 : FilterPairedReads() validation for PaDeNA step6:step4 completed successfully");
            ApplicationLog.WriteLine(
              "PADENA P1 : FilterPairedReads() validation for PaDeNA step6:step4 completed successfully");
        }

        /// <summary>
        /// Validate FilterPairedRead.FilterPairedRead() by passing graph object
        /// </summary>
        /// <param name="nodeName">xml node name used for a differnt testcase.</param>
        /// <param name="IsForward">True for Forward orientation else false.</param>
        internal void ValidateContigDistance(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.KmerLengthNode);
            string daglingThreshold = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.RedundantThreshold);
            string expectedContigPairedReadsCount = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ContigPairedReadsCount);
            string distanceBetweenFirstContigs = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.DistanceBetweenFirstContig);
            string distanceBetweenSecondContigs = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.DistanceBetweenSecondContig);
            string firstStandardDeviation = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.FirstContigStandardDeviation);
            string secondStandardDeviation = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.SecondContigStandardDeviation);

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            this.KmerLength = Int32.Parse(kmerLength);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold);
            this.DanglingLinksPurger =
              new DanglingLinksPurger(Int32.Parse(daglingThreshold));
            this.RedundantPathsPurger =
              new RedundantPathsPurger(Int32.Parse(redundantThreshold));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold);
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
                contigpairedReads.Values.Count.ToString());

            Dictionary<ISequence, IList<ValidMatePair>> map;
            IList<ValidMatePair> valid;

            if (contigpairedReads.ContainsKey(sortedContigs[0]))
            {
                map = contigpairedReads[sortedContigs[0]];
            }
            else
            {
                map = contigpairedReads[sortedContigs[1]];
            }


            if (map.ContainsKey(sortedContigs[0]))
            {
                valid = map[sortedContigs[0]];
            }
            else
            {
                valid = map[sortedContigs[1]];

            }

            // Validate distance and standard deviation between contigs.
            Assert.AreEqual(float.Parse(distanceBetweenFirstContigs),
              valid.First().DistanceBetweenContigs[0]);
            Assert.AreEqual(float.Parse(distanceBetweenSecondContigs),
              valid.First().DistanceBetweenContigs[1]);
            Assert.AreEqual(float.Parse(firstStandardDeviation),
              valid.First().StandardDeviation[0]);
            Assert.AreEqual(float.Parse(secondStandardDeviation),
              valid.First().StandardDeviation[1]);

            Console.WriteLine(
              "PADENA P1 : DistanceCalculator() validation for PaDeNA step6:step5 completed successfully");
            ApplicationLog.WriteLine(
              "PADENA P1 : DistanceCalculator() validation for PaDeNA step6:step5 completed successfully");
        }

        /// <summary>
        /// Validate Assembled paths for a given input reads.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateAssembledPath(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.KmerLengthNode);
            string daglingThreshold = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.RedundantThreshold);
            string library = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.LibraryName);
            string StdDeviation = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviation);
            string mean = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.Mean);
            string expectedDepth = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.DepthNode);
            string[] assembledPath = Utility._xmlUtil.GetTextValues(nodeName,
                Constants.SequencePathNode);

            IList<ScaffoldPath> paths = new List<ScaffoldPath>();

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads

            this.KmerLength = Int32.Parse(kmerLength);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold);
            this.DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold));
            this.RedundantPathsPurger = new RedundantPathsPurger(Int32.Parse(redundantThreshold));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold);
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

            ReadContigMap maps = mapper.Map(
                sortedContigs, sequenceReads, this.KmerLength);

            // Find map paired reads.
            CloneLibrary.Instance.AddLibrary(library, float.Parse(mean),
                float.Parse(StdDeviation));
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
            paths = trace.FindPaths(graph, contigpairedReads, Int32.Parse(kmerLength),
                Int32.Parse(expectedDepth));

            // Assemble paths.
            PathPurger pathsAssembler = new PathPurger();
            pathsAssembler.PurgePath(paths);
            IList<ISequence> seqList = new List<ISequence>();

            // Get a sequence from assembled path.
            foreach (ScaffoldPath temp in paths)
            {
                seqList.Add(temp.BuildSequenceFromPath(graph, Int32.Parse(kmerLength)));
            }

            //Validate assembled sequence paths.
            for (int index = 0; index < seqList.Count; index++)
            {
                Assert.IsTrue(assembledPath.Contains(seqList[index].ToString()));
            }

            Console.WriteLine(
              "PADENA P1 : AssemblePath() validation for PaDeNA step6:step7 completed successfully");
            ApplicationLog.WriteLine(
             "PADENA P1 : AssemblePath() validation for PaDeNA step6:step7 completed successfully");
        }

        /// <summary>
        /// Sort Contig List based on the contig sequence
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        private IList<ISequence> SortContigsData(IList<ISequence> contigsList)
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
        private IList<ValidMatePair> SortPairedReads(IList<ValidMatePair> list, IList<ISequence> reads)
        {
            return (from valid in list
                    orderby valid.PairedRead.GetForwardRead(reads).ToString()
                    select valid).ToList();
        }

        /// <summary>
        /// Validate scaffold paths for a given input reads.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateScaffoldPath(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.KmerLengthNode);
            string daglingThreshold = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.RedundantThreshold);
            string[] expectedScaffoldNodes = Utility._xmlUtil.GetTextValues(nodeName,
                Constants.ScaffoldNodes);
            string libraray = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.LibraryName);
            string StdDeviation = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviation);
            string mean = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.Mean);
            string expectedDepth = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.DepthNode);

            IList<ScaffoldPath> paths =
             new List<ScaffoldPath>();

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            this.KmerLength = Int32.Parse(kmerLength);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold);
            this.DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold));
            this.RedundantPathsPurger =
              new RedundantPathsPurger(Int32.Parse(redundantThreshold));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold);
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
            CloneLibrary.Instance.AddLibrary(libraray, float.Parse(mean),
               float.Parse(StdDeviation));

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
            paths = trace.FindPaths(graph, contigpairedReads, Int32.Parse(kmerLength),
              Int32.Parse(expectedDepth));

            ScaffoldPath scaffold = paths.First();
            for (int i = 0; i < scaffold.Count; i++)
            {
                Assert.IsTrue(expectedScaffoldNodes.Contains(
                 this.Graph.GetNodeSequence(scaffold[i].Key).ToString())
                 || expectedScaffoldNodes.Contains(this.Graph.GetNodeSequence(scaffold[i].Key).ReverseComplement.ToString()));
            }

            Console.WriteLine(
                "PADENA P1 : FindPaths() validation for PaDeNA step6:step6 completed successfully");
            ApplicationLog.WriteLine(
                "PADENA P1 : FindPaths() validation for PaDeNA step6:step6 completed successfully");
        }

        /// <summary>
        /// Validate scaffold sequence for a given input reads.
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void ValidateScaffoldSequence(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.KmerLengthNode);
            string daglingThreshold = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.RedundantThreshold);
            string libraray = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.LibraryName);
            string StdDeviation = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviation);
            string mean = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.Mean);
            string expectedScaffoldPathCount = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.ScaffoldPathCount);
            string inputRedundancy = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.InputRedundancy);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ScaffoldSeq);
            string[] scaffoldSeqNodes = expectedSeq.Split(',');

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Build kmers from step1,graph in step2 
            // Remove the dangling links from graph in step3
            // Remove bubbles form the graph in step4 
            // Pass the graph and build contigs
            // Validate contig reads
            this.KmerLength = Int32.Parse(kmerLength);
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold);
            this.DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold));
            this.RedundantPathsPurger =
              new RedundantPathsPurger(Int32.Parse(redundantThreshold));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();

            // Build contig.
            this.RemoveRedundancy();
            IList<ISequence> contigs = this.BuildContigs();

            // Find map paired reads.
            CloneLibrary.Instance.AddLibrary(libraray, float.Parse(mean),
             float.Parse(StdDeviation));

            GraphScaffoldBuilder scaffold = new GraphScaffoldBuilder();

            IList<ISequence> scaffoldSeq = scaffold.BuildScaffold(
                sequenceReads, contigs, this.KmerLength, redundancy: Int32.Parse(inputRedundancy));
            Assert.AreEqual(expectedScaffoldPathCount, scaffoldSeq.Count.ToString());

            string[] actualSeqs = new string[scaffoldSeq.Count];

            for (int i = 0; i < scaffoldSeq.Count; i++)
            {
                actualSeqs[i] = scaffoldSeq[i].ToString();
            }

            Array.Sort(scaffoldSeqNodes);
            Array.Sort(actualSeqs);

            for (int i = 0; i < scaffoldSeq.Count; i++)
            {
                Assert.IsTrue(scaffoldSeqNodes.Contains(actualSeqs[i]) ||
                    scaffoldSeqNodes.Contains(new Sequence(Alphabets.DNA, actualSeqs[i]).ReverseComplement.ToString()));
            }

            Console.WriteLine(
                "PADENA P1 : Scaffold sequence : validation for PaDeNA step6:step8 completed successfully");
            ApplicationLog.WriteLine(
                "PADENA P1 : Scaffold sequence : validation for PaDeNA step6:step8 completed successfully");
        }

        /// <summary>
        /// Validate Parallel Denovo Assembly Assembled sequences.
        /// </summary>
        /// <param name="nodeName">XML node used to validate different test scenarios</param>
        internal void ValidatePaDeNAAssembledSeqs(string nodeName)
        {
            // Get values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
               nodeName, Constants.FilePathNode);
            string kmerLength = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.KmerLengthNode);
            string daglingThreshold = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DanglingLinkThresholdNode);
            string redundantThreshold = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.RedundantThreshold);
            string libraray = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.LibraryName);
            string stdDeviation = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviation);
            string mean = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.Mean);
            string assembledSequences = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SequencePathNode);
            string assembledSeqCount = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AssembledSeqCountNode);
            string[] updatedAssembledSeqs = assembledSequences.Split(',');

            // Get the input reads and build kmers
            FastaParser parser = new FastaParser();
            IList<ISequence> sequenceReads = parser.Parse(filePath);

            // Create a ParallelDeNovoAssembler instance.
            ParallelDeNovoAssembler denovoObj = new ParallelDeNovoAssembler();

            denovoObj.KmerLength = Int32.Parse(kmerLength);
            denovoObj.DanglingLinksThreshold = Int32.Parse(daglingThreshold);
            denovoObj.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold);

            CloneLibrary.Instance.AddLibrary(libraray, float.Parse(mean),
             float.Parse(stdDeviation));

            IDeNovoAssembly assembly = denovoObj.Assemble(sequenceReads.ToList(), true);

            // Validate assembled sequences.
            Assert.AreEqual(assembledSeqCount, assembly.AssembledSequences.Count.ToString());

            for (int i = 0; i < assembly.AssembledSequences.Count; i++)
            {
                Assert.IsTrue(assembledSequences.Contains(
               assembly.AssembledSequences[i].ToString())
                || updatedAssembledSeqs.Contains(
                assembly.AssembledSequences[i].ReverseComplement.ToString()));
            }

            Console.WriteLine(
               "PaDeNA P1 : Assemble() validation for PaDeNA step6:step7 completed successfully");
            ApplicationLog.WriteLine(
                "PaDeNA P1 : Assemble() validation for PaDeNA step6:step7 completed successfully");
        }

        #endregion
    }
}
