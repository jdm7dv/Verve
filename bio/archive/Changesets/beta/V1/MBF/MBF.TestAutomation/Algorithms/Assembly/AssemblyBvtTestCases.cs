// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * AssemblyBvtTestCases.cs
 * 
 *   This file contains the Assembly Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;

using MBF.Algorithms.Alignment;
using MBF.Algorithms.Assembly;
using MBF.SimilarityMatrices;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.Algorithms.Assembly
{
    /// <summary>
    /// Assembly Bvt Test case implementation.
    /// </summary>
    [TestFixture]
    public class AssemblyBvtTestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static AssemblyBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region Sequence Assembly BVT Test cases

        /// <summary>
        /// Validates if the Assemble() method assembles the 
        /// sequences on passing valid sequences as parameter..
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, merge threshold, consensus threshold.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [Test]
        public void SequenceAssemblerWithAssembleMethod()
        {
            ValidateSequenceAssemblerGeneral("assemble");
        }

        /// <summary>
        /// Validate if the contig() method is retrieves a valid Contig 
        /// once a valid index is passed as parameter.
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        ///             gap cost, merge threshold, consensus threshold.
        /// Validation: Validates valid Contig is read.
        /// </summary>
        [Test]
        public void SequenceAssemblerWithContigMethod()
        {
            ValidateSequenceAssemblerGeneral("contig");
        }

        #endregion Sequence Assembly BVT Test cases

        #region Consensus BVT Test cases

        /// <summary>
        /// Validate MakeConsensus() method.
        /// Input: Sequences with Alphabets, matchscore, mismatch score, 
        /// gap cost, merge threshold, consensus threshold.
        /// Validation: Validates valid Contig is read.
        /// </summary>
        [Test]
        public void SimpleConsensusWithMakeConsensusMethod()
        {
            ValidateSequenceAssemblerGeneral("consensus");
        }

        #endregion Consensus BVT Test cases

        #region Supported methods

        /// <summary>
        /// Validate Sequence Assembler Test cases based on additional parameter values
        /// </summary>
        /// <param name="additionalParameter">Addtional parameters</param>
        static void ValidateSequenceAssemblerGeneral(string additionalParameter)
        {
            // Get the parameters from Xml
            int matchScore = int.Parse(Utility._xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                Constants.MatchScoreNode), null);
            int mismatchScore = int.Parse(Utility._xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                Constants.MisMatchScoreNode), null);
            int gapCost = int.Parse(Utility._xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                Constants.GapCostNode), null);
            double mergeThreshold = double.Parse(Utility._xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.MergeThresholdNode), null);
            double consensusThreshold = double.Parse(Utility._xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.ConsensusThresholdNode), null);
            string sequence1 = Utility._xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                Constants.SequenceNode1);
            string sequence2 = Utility._xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                Constants.SequenceNode2);
            string sequence3 = Utility._xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                Constants.SequenceNode3);
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.AlphabetNameNode));
            MoleculeType molType = Utility.GetMoleculeType(Utility._xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.MoleculeTypeNode));

            // Log based on the test cases
            switch (additionalParameter)
            {
                case "consensus":
                    // Logs the sequences
                    ApplicationLog.WriteLine(string.Format(null,
                        "SimpleConsensusMethod BVT : Sequence 1 used is '{0}'.", sequence1));
                    Console.WriteLine(string.Format(null,
                        "SimpleConsensusMethod BVT : Sequence 1 used is '{0}'.", sequence1));
                    ApplicationLog.WriteLine(string.Format(null,
                        "SimpleConsensusMethod BVT : Sequence 2 used is '{0}'.", sequence2));
                    Console.WriteLine(string.Format(null,
                        "SimpleConsensusMethod BVT : Sequence 2 used is '{0}'.", sequence2));
                    ApplicationLog.WriteLine(string.Format(null,
                        "SimpleConsensusMethod BVT : Sequence 3 used is '{0}'.", sequence3));
                    Console.WriteLine(string.Format(null,
                        "SimpleConsensusMethod BVT : Sequence 3 used is '{0}'.", sequence3));
                    break;
                default:
                    // Logs the sequences
                    ApplicationLog.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Sequence 1 used is '{0}'.", sequence1));
                    Console.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Sequence 1 used is '{0}'.", sequence1));
                    ApplicationLog.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Sequence 2 used is '{0}'.", sequence2));
                    Console.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Sequence 2 used is '{0}'.", sequence2));
                    ApplicationLog.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Sequence 3 used is '{0}'.", sequence3));
                    Console.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Sequence 3 used is '{0}'.", sequence3));
                    break;
            }

            Sequence seq1 = new Sequence(alphabet, sequence1);
            Sequence seq2 = new Sequence(alphabet, sequence2);
            Sequence seq3 = new Sequence(alphabet, sequence3);

            // here is how the above sequences should align:
            // TATAAAGCGCCAA
            //         GCCAAAATTTAGGC
            //                   AGGCACCCGCGGTATT   <= reversed
            // 
            // TATAAAGCGCCAAAATTTAGGCACCCGCGGTATT

            OverlapDeNovoAssembler assembler = new OverlapDeNovoAssembler();
            assembler.MergeThreshold = mergeThreshold;
            assembler.OverlapAlgorithm = new PairwiseOverlapAligner();
            ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).SimilarityMatrix =
                new DiagonalSimilarityMatrix(matchScore, mismatchScore, molType);
            ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).GapOpenCost = gapCost;
            assembler.ConsensusResolver = new SimpleConsensusResolver(consensusThreshold);
            assembler.AssumeStandardOrientation = false;

            List<ISequence> inputs = new List<ISequence>();
            inputs.Add(seq1);
            inputs.Add(seq2);
            inputs.Add(seq3);

            // Assembles all the sequences.
            IOverlapDeNovoAssembly assembly = (IOverlapDeNovoAssembly)assembler.Assemble(inputs);

            // Get the parameters from Xml in general
            int contigSequencesCount = int.Parse(Utility._xmlUtil.GetTextValue(
                Constants.AssemblyAlgorithmNodeName,
                Constants.ContigSequencesCountNode), null);
            string contigConsensus = Utility._xmlUtil.GetTextValue(Constants.AssemblyAlgorithmNodeName,
                Constants.ContigConsensusNode);

            switch (additionalParameter.ToLower(CultureInfo.CurrentCulture))
            {
                case "assemble":
                    // Get the parameters from Xml for Assemble() method test cases.
                    int unMergedCount = int.Parse(Utility._xmlUtil.GetTextValue(
                        Constants.AssemblyAlgorithmNodeName,
                        Constants.UnMergedSequencesCountNode), null);
                    int contigsCount = int.Parse(Utility._xmlUtil.GetTextValue(
                        Constants.AssemblyAlgorithmNodeName,
                        Constants.ContigsCountNode), null);

                    Assert.AreEqual(unMergedCount, assembly.UnmergedSequences.Count);
                    Assert.AreEqual(contigsCount, assembly.Contigs.Count);
                    Contig contigRead = assembly.Contigs[0];

                    // Logs the concensus
                    ApplicationLog.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Un Merged Sequences Count is '{0}'.",
                        assembly.UnmergedSequences.Count.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Contigs Count is '{0}'.",
                        assembly.Contigs.Count.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Contig Sequences Count is '{0}'.",
                        contigRead.Sequences.Count.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Consensus read is '{0}'.",
                        contigRead.Consensus.ToString()));
                    Console.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Consensus read is '{0}'.",
                        contigRead.Consensus.ToString()));

                    Assert.AreEqual(contigConsensus, contigRead.Consensus.ToString());
                    Assert.AreEqual(contigSequencesCount, contigRead.Sequences.Count);
                    break;
                case "contig":
                    // Read the contig from Contig method.
                    Contig contigsRead = assembly.Contigs[0];

                    // Log the required info.
                    ApplicationLog.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Consensus read is '{0}'.",
                        contigsRead.Consensus.ToString()));
                    Console.WriteLine(string.Format(null,
                        "SequenceAssembly BVT : Consensus read is '{0}'.",
                        contigsRead.Consensus.ToString()));

                    ApplicationLog.WriteLine("SequenceAssembly BVT : Successfully read the Contig.");
                    Console.WriteLine("SequenceAssembly BVT : Successfully read the Contig.");

                    Assert.AreEqual(contigConsensus, contigsRead.Consensus.ToString());
                    Assert.AreEqual(contigSequencesCount, contigsRead.Sequences.Count);
                    break;
                case "consensus":
                    // Read the contig from Contig method.
                    Contig contigReadForConsensus = assembly.Contigs[0];
                    contigReadForConsensus.Consensus = null;
                    OverlapDeNovoAssembler simpleSeqAssembler = new OverlapDeNovoAssembler();
                    simpleSeqAssembler.ConsensusResolver = new SimpleConsensusResolver(consensusThreshold);
                    simpleSeqAssembler.MakeConsensus(alphabet, contigReadForConsensus);

                    // Log the required info.
                    ApplicationLog.WriteLine(string.Format(null,
                        "SimpleConsensusMethod BVT : Consensus read is '{0}'.",
                        contigReadForConsensus.Consensus.ToString()));
                    Console.WriteLine(string.Format(null,
                        "SimpleConsensusMethod BVT : Consensus read is '{0}'.",
                        contigReadForConsensus.Consensus.ToString()));
                    Assert.AreEqual(contigConsensus, contigReadForConsensus.Consensus.ToString());
                    break;
                default:
                    break;
            }
        }

        #endregion Supported methods
    }
}
