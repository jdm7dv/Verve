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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.Algorithms.Assembly.PaDeNA
{
    /// <summary>
    /// The class contains P1 test cases to confirm PaDeNA.
    /// </summary>
    [TestClass]
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
        }

        #endregion

        #region PaDeNAStep1TestCases

        /// <summary>
        /// Validate ParallelDeNovothis is building valid kmers 
        /// using virul genome input reads in a fasta file and kmerLength 28
        /// Input : virul genome input reads and kmerLength 28
        /// Output : kmers sequence
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep1BuildKmersForViralGenomeReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNABuildKmers(Constants.ViralGenomeReadsNode, true);
            }
        }

        /// <summary>
        /// Validate ParallelDeNovothis is building valid kmers 
        /// using input reads which contains sequence and reverse complement
        /// Input : input reads with reverse complement and kmerLength 20
        /// Output : kmers sequence
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep1BuildKmersWithRCReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNABuildKmers(Constants.OneLineReadsWithRCNode, false);
            }
        }

        /// <summary>
        /// Validate ParallelDeNovothis is building valid kmers 
        /// using input reads which will generate clusters in step2 graph
        /// Input : input reads which will generate clusters and kmerLength 7
        /// Output : kmers sequence
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep1BuildKmersWithClusters()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNABuildKmers(Constants.OneLineReadsWithClustersNode, false);
            }
        }

        /// <summary>
        /// Validate KmersOfSequence ctor (sequence, length) by passing
        /// one line sequence and kmer length 4
        /// Input : Build kmeres from one line input reads of small size 
        /// chromosome sequence and kmerLength 4
        /// Output : kmers of sequence object with build kmers
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep1KmersOfSequenceCtorWithBuildKmers()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateKmersOfSequenceCtorWithBuildKmers(Constants.OneLineReadsNode);
            }
        }

        /// <summary>
        /// Validate KmersOfSequence ctor (sequence, length, set of kmers) 
        /// by passing small size chromsome sequence and kmer length 28
        /// after building kmers
        /// Input : Build kmeres from one line input reads of small size 
        /// chromosome sequence and kmerLength 28
        /// Output : kmers of sequence object with build kmers
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep1KmersOfSequenceCtorWithBuildKmersForSmallSizeSequences()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateKmersOfSequenceCtorWithBuildKmers(Constants.OneLineReadsNode);
            }
        }

        /// <summary>
        /// Validate KmersOfSequence properties
        /// Input : Build kmeres from 4000 input reads of small size 
        /// chromosome sequence and kmerLength 4 
        /// Output : kmers sequence
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateKmersOfSequenceCtrproperties()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateKmersOfSequenceCtorProperties(Constants.OneLineReadsNode);
            }
        }

        /// <summary>
        /// Validate KmersOfSequence ToSequences() method using small size reads
        /// Input : Build kmeres from 4000 input reads of small size 
        /// chromosome sequence and kmerLength 28 
        /// Output : kmers sequence
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep1KmersOfSequenceToSequencesUsingSmallSizeReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateKmersOfSequenceToSequences(Constants.OneLineReadsNode);
            }
        }



        #endregion

        #region PaDeNAStep2TestCases

        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with virul genome reads and kmerLength 28
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep2BuildGraphForVirulGenome()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNABuildGraph(Constants.ViralGenomeReadsNode, true);
            }
        }


        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with input reads contains reverse complement and kmerLength 20
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep2BuildGraphWithRCReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNABuildGraph(Constants.OneLineWithRCStep2Node, false);
            }
        }

        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with input reads which will generate clusters in step2 graph
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep2BuildGraphWithClusters()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNABuildGraph(Constants.OneLineReadsWithClustersNode, false);
            }
        }

        /// <summary>
        /// Validate Graph after building it using build kmers 
        /// with input reads which will generate clusters in step2 graph
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep2BuildGraphWithSmallSizeReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNABuildGraph(Constants.SmallChromosomeReadsNode, true);
            }
        }

        ///<summary>
        /// Validate Validate DeBruijinGraph properties
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep2DeBruijinGraphProperties()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateDeBruijinGraphproperties(Constants.OneLineStep2GraphNode);
            }
        }

        ///<summary>
        /// Validate Validate DeBruijinGraph properties for small size sequence reads.
        /// Input: kmers
        /// Output: Graph
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep2DeBruijinGraphPropertiesForSmallSizeRC()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateDeBruijinGraphproperties(Constants.OneLineReadsWithRCNode);
            }
        }

        /// <summary>
        /// Validate DeBruijinNode ctor by passing dna 
        /// kmersof sequence and graph object of chromosome
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep2DeBruijinCtrByPassingOneLineRC()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateDeBruijnNodeCtor(Constants.OneLineStep2GraphNode);
            }
        }

        /// <summary>
        /// Validate AddLeftExtension() method by 
        /// passing node object and orinetation 
        /// with chromosome read
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep2DeBruijnNodeAddLeftExtensionWithReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateDeBruijnNodeAddLeftExtension(Constants.OneLineWithRCStep2Node);
            }
        }

        /// <summary>
        /// Create dbruijn node by passing kmer and create another node.
        /// Add new node as leftendextension of first node. Validate the 
        /// AddRightEndExtension() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep2DeBruijnNodeAddRightExtensionWithRCReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateDeBruijnNodeAddRightExtension(Constants.OneLineWithRCStep2Node);
            }
        }

        /// <summary>
        /// Validate RemoveExtension() method by passing node 
        /// object and orientation with one line read
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep2DeBruijnNodeRemoveExtensionWithOneLineReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateDeBruijnNodeRemoveExtension(Constants.OneLineWithRCStep2Node);
            }
        }

        #endregion

        #region PaDeNAStep3TestCases

        /// <summary>
        /// Validate the PaDeNA step3 
        /// which removes dangling links from the graph using reads with rc kmers
        /// Input: Graph with dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep3UndangleGraphWithRCReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNAUnDangleGraph(Constants.OneLineWithRCStep2Node, true, false);
            }
        }

        /// <summary>
        /// Validate the PaDeNA step3 
        /// which removes dangling links from the graph using virul genome kmers
        /// Input: Graph with dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep3UndangleGraphForViralGenomeReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNAUnDangleGraph(Constants.ViralGenomeReadsNode, true, true);
            }
        }

        /// <summary>
        /// Validate the PaDeNA step3 using input reads which will generate clusters in step2 graph
        /// Input: Graph with dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep3UndangleGraphWithClusters()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNAUnDangleGraph(Constants.OneLineReadsWithClustersAfterDangling, false, false);
            }
        }

        /// <summary>
        /// Validate removal of dangling links by passing input reads with 3 dangling links
        /// Input: Graph with 3 dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep3UndangleGraphWithDanglingLinks()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNAUnDangleGraph(Constants.ReadsWithDanglingLinksNode, false, false);
            }
        }

        /// <summary>
        /// Validate removal of dangling links by passing input reads with 3 dangling links
        /// Input: Graph with 3 dangling links
        /// Output: Graph without any dangling links
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep3UndangleGraphWithMultipleDanglingLinks()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNAUnDangleGraph(Constants.ReadsWithMultipleDanglingLinksNode, false, false);
            }
        }


        /// <summary>
        /// Validate the DanglingLinksPurger is removing the dangling link nodes
        /// from the graph
        /// Input: Graph and dangling node
        /// Output: Graph without any dangling nodes
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep3RemoveErrorNodesForSmallSizeReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNARemoveErrorNodes(Constants.ViralGenomeReadsNode);
            }
        }

        #endregion

        #region PaDeNAStep4TestCases

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using virul genome reads such that it will create bubbles in the graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep4RemoveRedundancyForViralGenomeReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNARemoveRedundancy(Constants.ViralGenomeReadsNode, true, true);
            }
        }

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads with rc such that it will create bubbles in the graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep4RemoveRedundancyWithRCReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNARemoveRedundancy(Constants.OneLineWithRCStep2Node, true, false);
            }
        }

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep4RemoveRedundancyWithClusters()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNARemoveRedundancy(Constants.OneLineReadsWithClustersNode, true, false);
            }
        }

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep4RemoveRedundancyWithBubbles()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNARemoveRedundancy(Constants.ReadsWithBubblesNode, false, false);
            }
        }

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep4RemoveRedundancyWithMultipleBubbles()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNARemoveRedundancy(Constants.ReadsWithMultipleBubblesNode, false, false);
            }
        }

        /// <summary>
        /// Validate PaDeNA step4 ParallelDeNovothis.RemoveRedundancy() by passing graph 
        /// using input reads which will generate clusters in step2 graph using Small size reads
        /// Input: Graph with bubbles
        /// Output: Graph without bubbles
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep4RemoveRedundancyWithSmallSizeReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNARemoveRedundancy(Constants.Step4ReadsWithSmallSize, false, true);
            }
        }

        /// <summary>
        /// Validate PaDeNA step4 RedundantPathPurgerCtor() by passing graph 
        /// using one line reads 
        /// Input: One line graph
        /// Output: Graph without bubbles
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep4RedundantPathPurgerCtorWithOneLineReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateRedundantPathPurgerCtor(Constants.Step4RedundantPathReadsNode, false);
            }
        }

        /// <summary>
        /// Validate DetectErrorNodes() by passing graph object with
        /// one line reads such that it has bubbles
        /// Input: One line graph
        /// Output: Graph without bubbles
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep4DetectErrorNodesForOneLineReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateRedundantPathPurgerCtor(Constants.Step4RedundantPathReadsNode, false);
            }
        }

        /// <summary>
        /// Validate PaDeNA RemoveErrorNodes() by passing redundant nodes list and graph
        /// Input : graph and redundant nodes list
        /// Output: Graph without bubbles
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep4RemoveErrorNodes()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateRedundantPathPurgerCtor(Constants.OneLineStep4ReadsNode, false);
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
        [Priority(1)]
        public void ValidatePaDeNAStep5BuildContigsForViralGenomeReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateDe2thisBuildContigs(Constants.ViralGenomeReadsNode, true);
            }
        }

        /// <summary>
        /// Validate PaDeNA step5 by passing graph and validating the contigs
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep5BuildContigsWithRCReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateDe2thisBuildContigs(Constants.OneLineReadsWithRCNode, false);
            }
        }

        /// <summary>
        /// Validate PaDeNA step5 by passing graph and validating the contigs
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep5BuildContigsWithClusters()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateDe2thisBuildContigs(Constants.OneLineReadsWithClustersNode, false);
            }
        }

        /// <summary>
        /// Validate PaDeNA step5 by passing graph and validating the
        /// contigs for small size chromosomes
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep5BuildContigsForSmallSizeChromosmes()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateDe2thisBuildContigs(Constants.SmallChromosomeReadsNode, true);
            }
        }

        /// <summary>
        /// Validate PaDeNA step5 SimpleContigBuilder.BuildContigs() 
        /// by passing graph for small size chromosome.
        /// Input : graph
        /// Output: Contigs
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep5SimpleContigBuilderBuildContigsForSmallSizeRC()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateSimpleContigBuilderBuild(Constants.ChromosomeReads, true);
            }
        }

        #endregion

        #region PaDeNAStep6:Step1:TestCases

        /// <summary>
        /// Validate paired reads for Seq Id Starts with ".."
        /// Input : X1,Y1 format map reads with sequence ID contains "..".
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6PairedReadsForSeqIDWithDots()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWithDotsNode);
            }
        }

        /// <summary>
        /// Validate paired reads for Seq Id Starts with ".." between
        /// Input : X1,Y1 format map reads with sequence ID contains ".." between.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6PairedReadsForSeqIDWithDotsBetween()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWithDotsBetweenSeqIdNode);
            }
        }

        /// <summary>
        /// Validate paired reads for Seq Id Chr1.X1:abc.X1:50K
        /// Input : X1,Y1 format map reads with sequence ID contains "X1 and Y1 letters" between.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6PairedReadsForSeqIDContainsX1Y1()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePairedReads(Constants.OneLineReadsForPairedReadsNode);
            }
        }

        /// <summary>
        /// Validate paired reads for Seq Id with special characters
        /// Input : X1,Y1 format map reads with sequence ID contains "X1 and Y1 letters" between.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6PairedReadsForSpecialCharsSeqId()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWithSpecialCharsNode);
            }
        }

        /// <summary>
        /// Validate paired reads for Mixed reads.
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6PairedReadsForMixedFormatReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWithDotsNode);
            }
        }

        /// <summary>
        /// Validate paired reads for 2K and 0.5K library.
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6PairedReadsForDifferentLibrary()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWith2KlibraryNode);
            }
        }

        /// <summary>
        /// Validate paired reads for 10K,50K and 100K library.
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6PairedReadsFor100kLibrary()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWith10KAnd50KAnd100KlibraryNode);
            }
        }

        /// <summary>
        /// Validate paired reads for Reads without any Seq Name
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6PairedReadsForSeqsWithoutAnyID()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWithoutAnySeqIdNode);
            }
        }

        /// <summary>
        /// Validate paired reads for Reads with numeric library name.
        /// Input : X1,Y1,F,R,1,2 format map reads
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6PairedReadsForNumericLibraryName()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePairedReads(Constants.ReadsWith10KAnd50KAnd100KlibraryNode);
            }
        }

        /// <summary>
        /// Validate Adding new library information to library list.
        /// Input : Library name,Standard deviation and mean length.
        /// Output : Validate forward and backward reads.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6Libraryinformation()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.AddLibraryInformation(Constants.AddX1AndY1FormatPairedReadsNode, true);
            }
        }

        /// <summary>
        /// Validate library information for 1 and 2 format paired reads.
        /// Input : 1 and 2 format paired reads.
        /// Output : Validate library information.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6GetLibraryinformation()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.GetLibraryInformation(Constants.GetLibraryInformationNode);
            }
        }

        #endregion PaDeNAStep6:Step1:TestCases

        #region PaDeNAStep6:Step2:TestCases

        /// <summary>
        /// Validate ReadContigMapper.Map() using multiple clustalW 
        /// contigs.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6MapReadsToContigForClustalW()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForClustalWContigsNode,
                    true);
            }
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using Reverse complement contig.
        /// contigs.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6MapReadsToContigForUsingReverseComplementContig()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForReverseComplementContigsNode,
                    true);
            }
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using left side contig generator.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6MapReadsToContigForLeftSideContigGenerator()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForLeftSideContigGeneratorNode,
                    true);
            }
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using Contigs generated by passing input 
        /// reads from sequence such that one read is sequence and another 
        /// read is reverse complement
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6MapReadsToContigForOneSeqReadAndOtherRevComp()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForSeqAndRevCompNode,
                 false);
            }
        }

        /// <summary>
        /// Validate ReadContigMapper.Map() using Right side contig generator.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate MapReads to contigs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6MapReadsToContigForRightSideGenerator()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateMapReadsToContig(Constants.MapPairedReadsToContigForRightSideContigGeneratorNode,
                    false);
            }
        }

        #endregion PaDeNAStep6:Step2:TestCases

        #region PaDeNAStep6:Step3:TestCases

        /// <summary>
        /// Validate Contig build graphs Contig reads generated from viral genome
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate Contig graph.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6ContigGraphForForwardAndReverseOrientation()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateContigGraph(Constants.ContigGraphForContigsWithBothOrientationNode, false);
            }
        }

        /// <summary>
        /// Validate Contig build graph for Contig reads generated such that
        /// one of the Contigs should be palindrome
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate Contig graph.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6ContigGraphForPalindromeContig()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateContigGraph(Constants.ContigGraphForPalindromeContigsNode, true);
            }
        }


        #endregion PaDeNaStep6:Step3:TestCases

        #region PaDeNAStep6:Step4:TestCases

        /// <summary>
        /// Validate filter Contig Pairs formed in Forward direction with one 
        /// paired read does not support orientation.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6FilterPairedReadsWithFWReadsNotSupportOrientation()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateFilterPaired(Constants.FilterPairedReadContigsForFWOrnNode, true);
            }
        }

        /// <summary>
        /// Validate filter Contig Pairs formed in Reverse direction with one paired
        /// read does not support orientation.
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6FilterPairedReadsWithRevReadsNotSupportOrientation()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateFilterPaired(Constants.FilterPairedReadContigsForRevOrientationNode, true);
            }
        }

        /// <summary>
        /// Validate filter Contig Pairs formed in Forward direction and reverse 
        /// complement of Contig
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6FilterPairedsForContigRevComplement()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateFilterPaired(
                  Constants.FilterPairedReadContigsForFWDirectionWithRevCompContigNode, false);
            }
        }

        /// <summary>
        /// Validate filter Contig Pairs formed in Backward direction
        /// and reverse complement of Contig
        /// Input : Reads,KmerLength,dangling threshold.
        /// Output : Validate filter contig pairs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6FilterPairedsForReverseReadAndRevComplement()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateFilterPaired(
                  Constants.FilterPairedReadContigsForBackwardDirectionWithRevCompContig, true);
            }
        }


        #endregion PaDeNaStep6:Step4:TestCases

        #region PaDeNAStep6:Step5:TestCases

        /// <summary>
        /// Calculate distance for Contig Pairs formed in Forward 
        /// direction with one paired read does not support orientation.
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6CalculateDistanceForForwardPairedReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateContigDistance(Constants.FilterPairedReadContigsForFWOrnNode);
            }
        }

        /// <summary>
        /// Calculate distance for Contig Pairs formed in Forward 
        /// direction with one paired read does not support orientation.
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6CalculateDistanceForReversePairedReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateContigDistance(Constants.FilterPairedReadContigsForRevOrientationNode);
            }
        }

        /// <summary>
        /// Calculate distance for Contig Pairs formed in Forward direction
        /// and reverse complement of Contig
        /// Input : 3-4 Line sequence reads.
        /// Output : Filtered contigs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6CalculateDistanceForForwardPairedReadsWithRevCompl()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateContigDistance(
                  Constants.FilterPairedReadContigsForFWDirectionWithRevCompContigNode);
            }
        }

        /// <summary>
        /// Calculate distance for Contig Pairs formed in Reverse direction
        /// and reverse complement of Contig
        /// Input : 3-4 Line sequence reads.
        /// Output : Standard deviation and distance between contigs.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6CalculateDistanceForReversePairedReadsWithRevCompl()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateContigDistance(
                  Constants.FilterPairedReadContigsForBackwardDirectionWithRevCompContig);
            }
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
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6ScaffoldPathsForForwardOrientation()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateScaffoldPath(
                  Constants.ScaffoldPathWithForwardOrientationNode);
            }
        }

        /// <summary>
        /// Validate scaffold path for Contig Pairs formed in Reverse
        /// direction with all paired reads support orientation using
        /// FindPath(grpah;ContigPairedReads;KmerLength;Depth).
        /// Input : 3-4 Line sequence reads.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6ScaffoldPathsForReverseOrientation()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateScaffoldPath(
                  Constants.ScaffoldPathWithReverseOrientationNode);
            }
        }

        /// <summary>
        /// Validate trace path for Contig Pairs formed in
        /// Forward direction and reverse complement of
        /// Contig
        /// Input : Forward read orientation and rev complement.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6ScaffoldPathsForForwardDirectionAndRevComp()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateScaffoldPath(
                  Constants.ScaffoldPathWithForwardDirectionAndRevComp);
            }
        }

        /// <summary>
        /// Validate trace path for Contig Pairs formed in
        /// Reverse direction and reverse complement of
        /// Contig
        /// Input : Reverse read orientation and rev complement.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6ScaffoldPathsForReverseDirectionAndRevComp()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateScaffoldPath(
                  Constants.ScaffoldPathWithReverseDirectionAndRevComp);
            }
        }

        /// <summary>
        /// Validate trace path for Contig Pairs formed in
        /// Forward direction and palindrome of
        /// Contig
        /// Input : Forward read orientation and palindrome of contig.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6ScaffoldPathsForForwardDirectionAndPalContig()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateScaffoldPath(
                  Constants.ScaffoldPathWithForwardDirectionAndPalContig);
            }
        }

        /// <summary>
        /// Validate trace path for Contig Pairs formed in
        /// Reverse direction and palindrome of
        /// Contig
        /// Input : Reverse read orientation and palindrome of contig.
        /// Output : Validation of scaffold paths.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6ScaffoldPathsForReverseDirectionAndPalContig()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateScaffoldPath(
                  Constants.ScaffoldPathWithReverseDirectionAndPalContig);
            }
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
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6AssembledPathForForwardAndRevComplContig()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateAssembledPath(
                  Constants.AssembledPathForForwardWithReverseCompl);
            }
        }

        /// <summary>
        /// Validate assembled path by passing scaffold paths for
        /// Contig Pairs formed in Reverse direction and reverse 
        /// complement of Contig
        /// Input : 3-4 Line sequence reads.
        /// Output : Assembled paths 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6AssembledPathForReverseAndRevComplContig()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateAssembledPath(
                  Constants.AssembledPathForReverseWithReverseCompl);
            }
        }

        /// <summary>
        /// Validate assembled path by passing scaffold for Contig Pairs 
        /// formed in Forward direction and palindrome of Contig
        /// Input : Sequence reads with Palindrome contigs.
        /// Output : Assembled paths 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6AssembledPathForForwardAndPalContig()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateAssembledPath(
                  Constants.AssembledPathForForwardAndPalContig);
            }
        }

        /// <summary>
        /// Validate assembled path by passing scaffold for Contig Pairs 
        /// formed in Reverse direction and palindrome of Contig
        /// Input : Sequence reads with Palindrome contigs.
        /// Output : Assembled paths 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6AssembledPathForReverseAndPalContig()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateAssembledPath(
                  Constants.AssembledPathForReverseAndPalContig);
            }
        }

        /// <summary>
        /// Validate Scaffold sequence for small size sequence reads.
        /// Input : small size sequence reads.
        /// Output : Validation of Scaffold sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6ScaffoldSequenceForSmallReads()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidateScaffoldSequence(Constants.ScaffoldSequenceNode);
            }
        }

        /// <summary>
        ///  Validate Assembled sequences with Euler Test data reads,
        ///  Input : Euler testObj data seq reads.
        ///  output : Aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6AssembledSequenceWithEulerData()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNAAssembledSeqs(
                    Constants.AssembledSequencesForEulerDataNode);
            }
        }

        /// <summary>
        ///  Validate Assembled sequences for reads formed 
        ///  scaffold paths containing overlapping paths.
        ///  Input : Viral Genome reads.
        ///  output : Aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6AssembledSequenceForOverlappingScaffoldPaths()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNAAssembledSeqs(
                    Constants.AssembledSequencesForViralGenomeReadsNode);
            }
        }

        /// <summary>
        ///  Validate Assembled sequences for reads formed 
        ///  contigs in forward and reverse complement contig.
        ///  Input : Sequence reads.
        ///  output : Aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6AssembledSequenceForForwardAndRevCompl()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNAAssembledSeqs(
                    Constants.AssembledSequencesForForwardAndRevComplContigNode);
            }
        }

        /// <summary>
        ///  Validate Assembled sequences for reads formed 
        ///  contigs in forward and palindrome contig.
        ///  Input : Sequence reads.
        ///  output : Aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidatePaDeNAStep6AssembledSequenceForForwardAndPalContig()
        {
            using (PaDeNAP1Test testObj = new PaDeNAP1Test())
            {
                testObj.ValidatePaDeNAAssembledSeqs(
                    Constants.AssembledSequencesForForwardAndPalContigNode);
            }
        }

        #endregion PaDeNAStep6:Step7/8:TestCases
    }

    /// <summary>
    /// This class contains helper methods for PaDeNA.
    /// </summary>
    internal class PaDeNAP1Test : ParallelDeNovoAssembler
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\PaDeNATestData\PaDeNATestsConfig.xml");

        #endregion Global Variables

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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedKmersCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.ExpectedKmersCount);

            // Set kmerLength
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Set all the input reads and execute build kmers
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IEnumerable<KmersOfSequence> lstKmers =
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength);

            if (IsSmallSize)
            {
                Assert.AreEqual(expectedKmersCount, lstKmers.Count().ToString((IFormatProvider)null));
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
        /// Validate graph generated using ParallelDeNovothis.CreateGraph() with kmers
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        /// <param name="isLargeSizeReads">Is large size reads?</param>
        internal void ValidatePaDeNABuildGraph(string nodeName, bool isLargeSizeReads)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedGraphsNodeCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.GraphNodesCountNode);

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

            Console.WriteLine("PaDeNA P1 : Step1,2 Completed Successfully");
            ApplicationLog.WriteLine("PaDeNA P1 : Step1,2 Completed Successfully");

            if (isLargeSizeReads)
                Assert.AreEqual(expectedGraphsNodeCount, graph.Nodes.Count.ToString((IFormatProvider)null));
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
            string nodesSequence = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.NodesSequenceNode);
            string nodesLeftEdges = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.NodesLeftEdgesCountNode);
            string nodesRightEdges = _utilityObj._xmlUtil.GetTextValue(nodeName,
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
                  dbnodes.LeftExtensionNodes.Count.ToString((IFormatProvider)null) == leftEdgesCount[iseq] ||
                  dbnodes.LeftExtensionNodes.Count.ToString((IFormatProvider)null) == rightEdgesCount[iseq]);
                Assert.IsTrue(
                  dbnodes.RightExtensionNodes.Count.ToString((IFormatProvider)null) == leftEdgesCount[iseq] ||
                  dbnodes.RightExtensionNodes.Count.ToString((IFormatProvider)null) == rightEdgesCount[iseq]);
            }
        }

        /// <summary>
        /// Get the input string from the file.
        /// </summary>
        /// <param name="filename">input filename</param>
        /// <returns>Reads the file and returns input string</returns>
        static string ReadStringFromFile(string filename)
        {
            string readString = null;
            using (StreamReader reader = new StreamReader(filename))
            {
                readString = reader.ReadToEnd();
            }
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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedNodesCount = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedNodesCountRemoveRedundancy);

            string danglingThreshold = null;
            string pathlengthThreshold = null;
            if (!defaultThreshold)
            {
                danglingThreshold = _utilityObj._xmlUtil.GetTextValue(nodeName,
                  Constants.DanglingLinkThresholdNode);
                pathlengthThreshold = _utilityObj._xmlUtil.GetTextValue(nodeName,
                  Constants.PathLengthThresholdNode);
            }

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
            if (!defaultThreshold)
            {
                this.DanglingLinksThreshold = int.Parse(danglingThreshold, (IFormatProvider)null);
                this.DanglingLinksPurger =
                  new DanglingLinksPurger(this.DanglingLinksThreshold);
                this.RedundantPathLengthThreshold = int.Parse(pathlengthThreshold, (IFormatProvider)null);
                this.RedundantPathsPurger =
                  new RedundantPathsPurger(this.RedundantPathLengthThreshold);
            }
            else
            {
                this.DanglingLinksPurger =
                  new DanglingLinksPurger(int.Parse(kmerLength, (IFormatProvider)null));
                this.RedundantPathsPurger =
                  new RedundantPathsPurger(int.Parse(kmerLength, (IFormatProvider)null) + 1);
            }
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
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
                Assert.AreEqual(expectedNodesCount, graph.Nodes.Count.ToString((IFormatProvider)null));
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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedNodesCount = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.NodesCountAfterDanglingGraphNode);
            string danglingThreshold = null;
            if (!defaultThreshold)
                danglingThreshold = _utilityObj._xmlUtil.GetTextValue(nodeName,
                  Constants.DanglingLinkThresholdNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build kmers from step1,graph in step2 
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            if (!defaultThreshold)
            {
                this.DanglingLinksThreshold = int.Parse(danglingThreshold, (IFormatProvider)null);
            }
            else
            {
                this.DanglingLinksThreshold = int.Parse(kmerLength, (IFormatProvider)null) + 1;
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
                Assert.AreEqual(expectedNodesCount, graph.Nodes.Count.ToString((IFormatProvider)null));
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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.BaseSequenceNode);
            string expectedKmers = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmersCountNode);

            // Get the input reads
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }
            SequenceToKmerBuilder builder = new SequenceToKmerBuilder();

            KmersOfSequence kmer = builder.Build(sequenceReads[0],
              int.Parse(kmerLength, (IFormatProvider)null));
            KmersOfSequence kmerSequence = new KmersOfSequence(sequenceReads[0],
              int.Parse(kmerLength, (IFormatProvider)null), kmer.Kmers);

            // Validate KmerOfSequence properties.
            Assert.AreEqual(expectedSeq, kmerSequence.BaseSequence.ToString());
            Assert.AreEqual(expectedKmers, kmerSequence.Kmers.Count.ToString((IFormatProvider)null));

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
            IList<KmersOfSequence> lstKmers;

            if (IsListSeq)
            {
                lstKmers = new List<KmersOfSequence>(builder.Build(sequenceReads,
                  int.Parse(kmerLength, (IFormatProvider)null)));
            }
            else
            {
                lstKmers = new List<KmersOfSequence>();
                foreach (ISequence sequence in sequenceReads)
                {
                    lstKmers.Add(builder.Build(sequence, int.Parse(kmerLength, (IFormatProvider)null)));
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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string ExpectedNodesCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.GraphNodesCountNode);

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

            Console.WriteLine("PaDeNA P1 : Step1,2 Completed Successfully");
            ApplicationLog.WriteLine("PaDeNA P1 : Step1,2 Completed Successfully");

            // Validate DeBruijnGraph Properties.
            Assert.AreEqual(ExpectedNodesCount, graph.Nodes.Count.ToString((IFormatProvider)null));

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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

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
            DeBruijnNode leftnode = new DeBruijnNode(int.Parse(kmerLength, (IFormatProvider)null), 1,
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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string nodeExtensionsCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.NodeExtensionsCountNode);
            string kmersCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmersCountNode);
            string leftNodeExtensionCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.LeftNodeExtensionsCountNode);
            string rightNodeExtensionCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.RightNodeExtensionsCountNode);

            // Get the input reads and build kmers
            IList<ISequence> sequenceReads = null;
            using (FastaParser parser = new FastaParser())
            {
                sequenceReads = parser.Parse(filePath);
            }

            // Build the kmers using this
            this.KmerLength = int.Parse(kmerLength, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            IList<KmersOfSequence> lstKmers = new List<KmersOfSequence>(
                (new SequenceToKmerBuilder()).Build(this.SequenceReads, this.KmerLength));

            // Validate the node creation
            DeBruijnNode node = new DeBruijnNode(int.Parse(kmerLength, (IFormatProvider)null), 0,
              lstKmers[0].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            DeBruijnNode rightnode = new DeBruijnNode(int.Parse(kmerLength, (IFormatProvider)null), 1,
              lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            DeBruijnNode leftnode = new DeBruijnNode(int.Parse(kmerLength, (IFormatProvider)null), 1,
              lstKmers[1].Kmers.First<KmersOfSequence.KmerPositions>().Positions[0]);
            node.AddRightEndExtension(rightnode, false);
            node.AddLeftEndExtension(leftnode, false);

            // Validate DeBruijnNode class properties.
            Assert.AreEqual(nodeExtensionsCount, node.ExtensionsCount.ToString((IFormatProvider)null));
            Assert.AreEqual(kmersCount, node.KmerCount.ToString((IFormatProvider)null));
            Assert.AreEqual(leftNodeExtensionCount, node.LeftExtensionNodes.Count.ToString((IFormatProvider)null));
            Assert.AreEqual(rightNodeExtensionCount, node.RightExtensionNodes.Count.ToString((IFormatProvider)null));
            Assert.AreEqual(kmerLength, node.KmerLength.ToString((IFormatProvider)null));
            Assert.AreEqual(leftNodeExtensionCount, node.LeftExtensionNodes.Count.ToString((IFormatProvider)null));
            Assert.AreEqual(rightNodeExtensionCount, node.RightExtensionNodes.Count.ToString((IFormatProvider)null));

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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);

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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.KmerLengthNode);
            string expectedNodesCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.ExpectedNodesCountAfterDangling);

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
            DeBruijnGraph graph = this.Graph;
            this.DanglingLinksPurger = new DanglingLinksPurger(this.KmerLength);
            this.UnDangleGraph();

            // Create RedundantPathPurger instance, detect redundant nodes and remove error nodes
            RedundantPathsPurger redundantPathPurger =
              new RedundantPathsPurger(int.Parse(kmerLength, (IFormatProvider)null) + 1);
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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedContigsString = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.ContigsNode);
            string[] expectedContigs;
            if (!expectedContigsString.Contains("PaDeNATestData"))
                expectedContigs = expectedContigsString.Split(',');
            else
                expectedContigs =
                  ReadStringFromFile(expectedContigsString).Replace("\r\n", "").Split(',');

            string expectedContigsCount = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ContigsCount);

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
            this.DanglingLinksPurger = new DanglingLinksPurger(this.KmerLength);
            this.UnDangleGraph();
            this.RedundantPathsPurger = new RedundantPathsPurger(this.KmerLength + 1);
            this.RemoveRedundancy();
            this.ContigBuilder = new SimplePathContigBuilder();
            IList<ISequence> contigs = this.BuildContigs();

            // Validate contigs count only for Chromosome files. 
            if (isChromosomeRC)
            {
                Assert.AreEqual(expectedContigsCount, contigs.Count.ToString((IFormatProvider)null));
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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.KmerLengthNode);
            string expectedContigsString = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.ContigsNode);
            string[] expectedContigs = expectedContigsString.Split(',');
            string expectedContigsCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.ContigsCount);

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
            DeBruijnGraph graph = this.Graph;
            this.UnDangleGraph();
            this.RemoveRedundancy();

            // Validate the SimpleContigBuilder.Build() by passing graph
            SimplePathContigBuilder builder = new SimplePathContigBuilder();
            IList<ISequence> contigs = builder.Build(graph);

            if (isChromosomeRC)
            {
                Assert.AreEqual(expectedContigsCount, contigs.Count.ToString((IFormatProvider)null));
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

            Console.WriteLine(@"PaDeNA P1 : Map paired reads has been verified successfully");
            ApplicationLog.WriteLine(@"PaDeNA P1 : Map paired reads has been verified successfully");
        }

        /// <summary>
        /// Validate library information
        /// </summary>
        /// <param name="nodeName">xml node name used for different testcases</param>
        internal void GetLibraryInformation(string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string expectedPairedReadsCount = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.PairedReadsCountNode);
            string expectedLibraray = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.LibraryName);
            string expectedStdDeviation = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.StdDeviation);
            string mean = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.Mean);

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
            Assert.AreEqual(expectedPairedReadsCount,
              pairedreads.Count.ToString((IFormatProvider)null));

            // Get library infomration and validate
            CloneLibraryInformation libraryInfo =
              CloneLibrary.Instance.GetLibraryInformation
              (pairedreads[0].Library);

            Assert.AreEqual(expectedStdDeviation, libraryInfo.StandardDeviationOfInsert.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedLibraray, libraryInfo.LibraryName.ToString((IFormatProvider)null));
            Assert.AreEqual(mean, libraryInfo.MeanLengthOfInsert.ToString((IFormatProvider)null));

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
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string expectedPairedReadsCount = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.PairedReadsCountNode);
            string[] backwardReadsNode = _utilityObj._xmlUtil.GetTextValues(nodeName,
              Constants.BackwardReadsNode);
            string[] forwardReadsNode = _utilityObj._xmlUtil.GetTextValues(nodeName,
              Constants.ForwardReadsNode);
            string expectedLibraray = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.LibraryName);
            string expectedStdDeviation = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.StdDeviation);
            string mean = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.Mean);

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
            if (IsLibraryInfo)
            {
                CloneLibraryInformation libraryInfo =
                  new CloneLibraryInformation();
                libraryInfo.LibraryName = expectedLibraray;
                libraryInfo.MeanLengthOfInsert = float.Parse(mean, (IFormatProvider)null);
                libraryInfo.StandardDeviationOfInsert = float.Parse(expectedStdDeviation, (IFormatProvider)null);
                CloneLibrary.Instance.AddLibrary(libraryInfo);
            }
            else
            {
                CloneLibrary.Instance.AddLibrary(expectedLibraray,
                    float.Parse(mean, (IFormatProvider)null), float.Parse(expectedStdDeviation, (IFormatProvider)null));
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
                Assert.AreEqual(expectedStdDeviation,
                  pairedreads[index].StandardDeviationOfLibrary.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedLibraray, pairedreads[index].Library.ToString((IFormatProvider)null));
                Assert.AreEqual(mean, pairedreads[index].MeanLengthOfLibrary.ToString((IFormatProvider)null));
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
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null);
            this.DanglingLinksPurger =
              new DanglingLinksPurger(Int32.Parse(daglingThreshold, (IFormatProvider)null));
            this.RedundantPathsPurger =
              new RedundantPathsPurger(Int32.Parse(redundantThreshold, (IFormatProvider)null));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, (IFormatProvider)null);
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
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null);
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
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null);
            this.DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold, (IFormatProvider)null));
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

            ReadContigMap maps = mapper.Map(
                sortedContigs, sequenceReads, this.KmerLength);

            // Find map paired reads.
            MatePairMapper mapPairedReads = new MatePairMapper();
            ContigMatePairs pairedReads = mapPairedReads.MapContigToMatePairs(sequenceReads, maps);

            // Filter contigs based on the orientation.
            OrientationBasedMatePairFilter filter = new OrientationBasedMatePairFilter();
            ContigMatePairs contigpairedReads = filter.FilterPairedReads(pairedReads, 0);


            Assert.AreEqual(expectedContigPairedReadsCount,
              contigpairedReads.Values.Count.ToString((IFormatProvider)null));

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
                  valid[index].ForwardReadStartPosition[0].ToString((IFormatProvider)null));
                Assert.AreEqual(expectedReverseReadStartPos[index],
                  valid[index].ReverseReadStartPosition[0].ToString((IFormatProvider)null));
                Assert.AreEqual(expectedReverseComplementStartPos[index],
                  valid[index].ReverseReadReverseComplementStartPosition[0].ToString((IFormatProvider)null));

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
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null);
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
            Assert.AreEqual(float.Parse(distanceBetweenFirstContigs, (IFormatProvider)null),
              valid.First().DistanceBetweenContigs[0]);
            Assert.AreEqual(float.Parse(distanceBetweenSecondContigs, (IFormatProvider)null),
              valid.First().DistanceBetweenContigs[1]);
            Assert.AreEqual(float.Parse(firstStandardDeviation, (IFormatProvider)null),
              valid.First().StandardDeviation[0]);
            Assert.AreEqual(float.Parse(secondStandardDeviation, (IFormatProvider)null),
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
            string StdDeviation = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviation);
            string mean = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.Mean);
            string expectedDepth = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.DepthNode);
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
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null);
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

            ReadContigMap maps = mapper.Map(
                sortedContigs, sequenceReads, this.KmerLength);

            // Find map paired reads.
            CloneLibrary.Instance.AddLibrary(library, float.Parse(mean, (IFormatProvider)null),
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

            // Assemble paths.
            PathPurger pathsAssembler = new PathPurger();
            pathsAssembler.PurgePath(paths);
            IList<ISequence> seqList = new List<ISequence>();

            // Get a sequence from assembled path.
            foreach (ScaffoldPath temp in paths)
            {
                seqList.Add(temp.BuildSequenceFromPath(graph, Int32.Parse(kmerLength, (IFormatProvider)null)));
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
        private static IList<ISequence> SortContigsData(IList<ISequence> contigsList)
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
        private static IList<ValidMatePair> SortPairedReads(IList<ValidMatePair> list, IList<ISequence> reads)
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

            IList<ScaffoldPath> paths =
             new List<ScaffoldPath>();

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
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null);
            this.DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold, (IFormatProvider)null));
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
            string expectedScaffoldPathCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ScaffoldPathCount);
            string inputRedundancy = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InputRedundancy);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ScaffoldSeq);
            string[] scaffoldSeqNodes = expectedSeq.Split(',');

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
            this.DanglingLinksThreshold = Int32.Parse(daglingThreshold, (IFormatProvider)null);
            this.DanglingLinksPurger = new DanglingLinksPurger(Int32.Parse(daglingThreshold, (IFormatProvider)null));
            this.RedundantPathsPurger =
              new RedundantPathsPurger(Int32.Parse(redundantThreshold, (IFormatProvider)null));
            this.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, (IFormatProvider)null);
            this.SequenceReads.Clear();
            this.AddSequenceReads(sequenceReads);
            this.CreateGraph();
            this.UnDangleGraph();

            // Build contig.
            this.RemoveRedundancy();
            IList<ISequence> contigs = this.BuildContigs();

            // Find map paired reads.
            CloneLibrary.Instance.AddLibrary(libraray, float.Parse(mean, (IFormatProvider)null),
             float.Parse(StdDeviation, (IFormatProvider)null));
            IList<ISequence> scaffoldSeq = null;

            using (GraphScaffoldBuilder scaffold = new GraphScaffoldBuilder())
            {
                scaffoldSeq = scaffold.BuildScaffold(
                   sequenceReads, contigs, this.KmerLength, redundancy: Int32.Parse(inputRedundancy, (IFormatProvider)null));
            }

            Assert.AreEqual(expectedScaffoldPathCount, scaffoldSeq.Count.ToString((IFormatProvider)null));
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
            string stdDeviation = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviation);
            string mean = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.Mean);
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
                denovoObj.RedundantPathLengthThreshold = Int32.Parse(redundantThreshold, (IFormatProvider)null);

                CloneLibrary.Instance.AddLibrary(libraray, float.Parse(mean, (IFormatProvider)null),
                 float.Parse(stdDeviation, (IFormatProvider)null));

                IDeNovoAssembly assembly = denovoObj.Assemble(sequenceReads.ToList(), true);

                // Validate assembled sequences.
                Assert.AreEqual(assembledSeqCount, assembly.AssembledSequences.Count.ToString((IFormatProvider)null));

                for (int i = 0; i < assembly.AssembledSequences.Count; i++)
                {
                    Assert.IsTrue(assembledSequences.Contains(
                   assembly.AssembledSequences[i].ToString())
                    || updatedAssembledSeqs.Contains(
                    assembly.AssembledSequences[i].ReverseComplement.ToString()));
                }
            }
            finally
            {
                if (denovoObj != null)
                    denovoObj.Dispose();
            }
            Console.WriteLine(
               "PaDeNA P1 : Assemble() validation for PaDeNA step6:step7 completed successfully");
            ApplicationLog.WriteLine(
                "PaDeNA P1 : Assemble() validation for PaDeNA step6:step7 completed successfully");
        }

        #endregion
    }
}
