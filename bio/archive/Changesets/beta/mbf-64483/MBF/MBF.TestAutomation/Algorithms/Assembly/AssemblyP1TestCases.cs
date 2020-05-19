// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * AssemblyP1TestCases.cs
 * 
 *   This file contains the Assembly P1 test cases
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using MBF.Algorithms.Alignment;
using MBF.Algorithms.Assembly;
using MBF.Algorithms.Kmer;
using MBF.SimilarityMatrices;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.Algorithms.Assembly
{
    /// <summary>
    /// Sequence Assembly and Consensus P1 Test case implementation.
    /// </summary>
    [TestClass]
    public class AssemblyP1TestCases
    {

        #region Enums

        /// <summary>
        /// Assembly Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AssemblyParameters
        {
            Assemble,
            DiagonalSM,
            SimilarityMatrix,
            Consensus
        };

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static AssemblyP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Sequence Assembly P1 Test cases

        /// <summary>
        /// Validates if the Assemble() method assembles the Dna
        /// sequences on passing valid sequences as parameter..
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, merge threshold, consensus threshold.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SequenceAssemblerWithAssembleMethodForDna()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblySequenceAlgorithmNodeName,
                AssemblyParameters.Assemble, false);
        }

        /// <summary>
        /// Validates if the Assemble() method assembles Maximum number of 
        /// sequences on passing valid sequences as parameter..
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, merge threshold, consensus threshold.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SequenceAssemblerWithAssembleMethodForMaxSequences()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblyMaxSequenceAlgorithmNodeName,
                AssemblyParameters.Assemble, false);
        }

        /// <summary>
        /// Validates if the Assemble() method assembles Minimum number of 
        /// sequences on passing valid sequences as parameter.
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, merge threshold, consensus threshold.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SequenceAssemblerWithAssembleMethodForMinSequences()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblyMinSequenceAlgorithmNodeName,
                AssemblyParameters.Assemble, false);
        }

        /// <summary>
        /// Validates if the Assemble() method assembles the Dna
        /// sequences on passing valid sequences as parameter with valid threshold.
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, merge threshold, consensus threshold.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SequenceAssemblerWithAssembleMethodForValidThreshold()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblySequenceAlgorithmNodeName,
                AssemblyParameters.Assemble, false);
        }

        /// <summary>
        /// Validates if the Assemble() method assembles the Dna
        /// sequences on passing valid sequences as parameter and maximum threshold.
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, Maximum merge threshold, consensus threshold.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SequenceAssemblerWithAssembleMethodForMaxThreshold()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblyMaxThresholdSequenceAlgorithmNodeName,
                AssemblyParameters.Assemble, false);
        }

        /// <summary>
        /// Validates if the Assemble() method assembles the Dna
        /// sequences on passing valid sequences as parameter and minimum threshold.
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, Minimum merge threshold, consensus threshold.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SequenceAssemblerWithAssembleMethodForMinThreshold()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblyMinThresholdSequenceAlgorithmNodeName,
                AssemblyParameters.Assemble, false);
        }

        /// <summary>
        /// Validates if the Assemble() method assembles the
        /// sequences on passing valid sequences as parameter using Diagonal Similarity Matrix.
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, Minimum merge threshold, consensus threshold, Diagonal Similarity Matrix.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SequenceAssemblerWithAssembleMethodForDiagonalSM()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblySequenceAlgorithmNodeName,
                AssemblyParameters.DiagonalSM, false);
        }

        /// <summary>
        /// Validates if the Assemble() method assembles the
        /// sequences on passing valid sequences as parameter using Diagonal Similarity Matrix.
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, Minimum merge threshold, consensus threshold, Diagonal Similarity Matrix.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SequenceAssemblerWithAssembleMethodForSimilarityMatrix()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblySequenceAlgorithmNodeName,
                AssemblyParameters.SimilarityMatrix, false);
        }

        #endregion Sequence Assembly P1 Test cases

        #region Consensus P1 Test cases

        /// <summary>
        /// Validates if the MakeConsensus() method provides the Consensus for a given
        /// DNA Contig.
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, merge threshold, consensus threshold.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SimpleConsensusWithMakeConsensusMethodForDna()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblySequenceAlgorithmNodeName,
                AssemblyParameters.Consensus, false);
        }

        /// <summary>
        /// Validates if the MakeConsensus() method provides the Consensus for a given
        /// Contig with Maximum threshold value.
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, Maximum merge threshold, consensus threshold.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SimpleConsensusWithMakeConsensusMethodForMaxThreshold()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblyMaxThresholdSequenceAlgorithmNodeName,
                AssemblyParameters.Consensus, false);
        }

        /// <summary>
        /// Validates if the MakeConsensus() method provides the Consensus for a given
        /// Contig with Minimum threshold value.
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, Minimum merge threshold, consensus threshold.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SimpleConsensusWithMakeConsensusMethodForMinThreshold()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblyMinThresholdSequenceAlgorithmNodeName,
                AssemblyParameters.Consensus, false);
        }

        /// <summary>
        /// Validates if the MakeConsensus() method provides the Consensus for a given
        /// Contig with  valid Threshold and valid useAmbiguityCodes value.
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, Maximum merge threshold, consensus threshold.
        /// Validation: validates unmerged sequences count, contigs count,
        /// contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SimpleConsensusWithMakeConsensusForValidThreshold()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblySequenceAlgorithmNodeName,
                AssemblyParameters.Consensus, false);
        }

        /// <summary>
        /// Validate Sequence Assmebly default constructor.
        /// Input: Sequences with Alphabets, matchscore, mismatch score,
        /// gap cost, merge threshold, consensus threshold.
        /// Validation: Validate Sequence Assmebly default constructor,unmerged sequences count, 
        ///             contigs count,contig sequences count and concensus.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SequenceAssemblerWithAssembleMethodForDnaUsingSeqAssemblyCtr()
        {
            ValidateSequenceAssemblerGeneral(Constants.AssemblySequenceAlgorithmNodeName,
                AssemblyParameters.Assemble, true);
        }

        /// <summary>
        /// Validate SequenceAssembler Properties.
        /// Input:  Sequece Assembler algorithm
        /// Validation: Validation of SeqeunceAssembler description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSequenceAssemblerProperties()
        {
            // Get the parameters from Xml
            string SimpleSeqAlgoName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceAssemblerPropertiesNode,
                Constants.NameNode);
            string mergeThreshold = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceAssemblerPropertiesNode,
                Constants.MergeThresholdNode);
            string overlapAlgoName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceAssemblerPropertiesNode,
                Constants.OverlapAlgoNameNode);
            string algoDescription = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceAssemblerPropertiesNode,
                Constants.Description);

            // Get SimpleSequenceAssembler properties.
            IList<IDeNovoAssembler> assemberCount = SequenceAssemblers.All;
            OverlapDeNovoAssembler assemblerDescriptions = SequenceAssemblers.Simple;

            // Validate SequenceAssembler
            Assert.AreEqual(1, assemberCount.Count);
            Assert.IsTrue(assemblerDescriptions.AssumeStandardOrientation);
            Assert.AreEqual(algoDescription, assemblerDescriptions.Description);
            Assert.IsNull(assemblerDescriptions.ConsensusResolver);
            Assert.AreEqual(mergeThreshold, assemblerDescriptions.MergeThreshold.ToString((IFormatProvider)null));
            Assert.AreEqual(SimpleSeqAlgoName, assemblerDescriptions.Name.ToString((IFormatProvider)null));
            Assert.AreEqual(overlapAlgoName, assemblerDescriptions.OverlapAlgorithm.Name.ToString((IFormatProvider)null));
        }

        /// <summary>
        /// Validate Kmer constructor and its properties.
        /// Input:  Valid Sequence 
        /// Validation: Validation of Kmer Sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateKmerSequenceProperties()
        {
            ValidateKmer(Constants.AssemblyAlgorithmNodeName, false);
        }

        /// <summary>
        /// Validate building kmer by passing Kmer length.
        /// Input:  Valid Sequence and Kmer length. 
        /// Validation: Validation of Kmer builder.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBuilderKmer()
        {
            ValidateKmer(Constants.KmerbuilderNode, true);
        }

        /// <summary>
        /// Validate Compare two sequences
        /// Input:  Valid Sequences and Kmer length. 
        /// Validation: Validation of  Comparing two sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateCompareTwoSequences()
        {
            ValidateComputeFeature(
                Constants.CompareTwoSequencesNode,
                AssemblyParameters.Consensus);
        }

        /// <summary>
        /// Validate Features of two sequences
        /// Input:  Valid Sequences and Kmer length. 
        /// Validation: Validation of Features two sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateComputeFeatures()
        {
            ValidateComputeFeature(
                Constants.CompareTwoProtienSequencesNode,
                AssemblyParameters.Assemble);
        }

        /// <summary>
        /// Validate Features of two sequences
        /// Input:  Valid Sequences and Kmer length. 
        /// Validation: Validation of Features two sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateComputeMoleculeFeatures()
        {
            ValidateComputeFeature(
                Constants.CompareTwoSequencesNode,
                AssemblyParameters.Assemble);
        }

        /// <summary>
        /// Validate Properties of Difference Node
        /// Input:  Valid DifferenceNode object. 
        /// Validation: Validation of Properties
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDifferenceNodeProperty()
        {
            DifferenceNode diffObj = new DifferenceNode(1, 1, 4, 4);

            Assert.AreEqual(1, diffObj.Sequence1Start);
            Assert.AreEqual(1, diffObj.Sequence2Start);
            Assert.AreEqual(4, diffObj.Sequence1End);
            Assert.AreEqual(4, diffObj.Sequence2End);
            Assert.AreEqual("1 : 1 : 4 : 4", diffObj.ToString());
        }

        #endregion Consensus BVT Test cases

        #region Supported methods

        /// <summary>
        /// Validates the Sequence Assembler for all the general test cases.
        /// </summary>
        /// <param name="nodeName">Xml Node Name</param>
        /// <param name="additionalParameter">Additional Parameter based 
        /// on which the validations are done.</param>
        /// <param name="isSeqAssemblyctr">True if Default contructor is validated or else false.</param>
        void ValidateSequenceAssemblerGeneral(string nodeName,
            AssemblyParameters additionalParameter, bool isSeqAssemblyctr)
        {
            // Get the parameters from Xml
            int matchScore = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.MatchScoreNode), null);
            int mismatchScore = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.MisMatchScoreNode), null);
            int gapCost = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.GapCostNode), null);
            double mergeThreshold = double.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.MergeThresholdNode), null);
            double consensusThreshold = double.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ConsensusThresholdNode), null);
            string[] sequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.SequencesNode);
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));
            MoleculeType molType = Utility.GetMoleculeType(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.MoleculeTypeNode));
            string documentation = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.DocumentaionNode);
            SerializationInfo info = new SerializationInfo(typeof(OverlapDeNovoAssembly),
                new FormatterConverter());
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            List<ISequence> inputs = new List<ISequence>();

            switch (additionalParameter)
            {
                case AssemblyParameters.Consensus:
                    for (int i = 0; i < sequences.Length; i++)
                    {
                        // Logs the sequences
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "SimpleConsensusMethod P1 : Sequence '{0}' used is '{1}'.",
                            i.ToString((IFormatProvider)null), sequences[i]));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "SimpleConsensusMethod P1 : Sequence '{0}' used is '{1}'.",
                            i.ToString((IFormatProvider)null), sequences[i]));

                        Sequence seq = new Sequence(alphabet, sequences[i]);
                        inputs.Add(seq);
                    }
                    break;
                default:
                    for (int i = 0; i < sequences.Length; i++)
                    {
                        // Logs the sequences
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "SequenceAssembly P1 : Sequence '{0}' used is '{1}'.",
                            i.ToString((IFormatProvider)null), sequences[i]));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "SequenceAssembly P1 : Sequence '{0}' used is '{1}'.",
                            i.ToString((IFormatProvider)null), sequences[i]));

                        Sequence seq = new Sequence(alphabet, sequences[i]);
                        inputs.Add(seq);
                    }
                    break;
            }

            // here is how the above sequences should align:
            // TATAAAGCGCCAA
            //         GCCAAAATTTAGGC
            //                   AGGCACCCGCGGTATT   <= reversed
            // 
            // TATAAAGCGCCAAAATTTAGGCACCCGCGGTATT

            OverlapDeNovoAssembler assembler = new OverlapDeNovoAssembler();
            assembler.MergeThreshold = mergeThreshold;
            assembler.OverlapAlgorithm = new PairwiseOverlapAligner();

            switch (additionalParameter)
            {
                case AssemblyParameters.DiagonalSM:
                    ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).SimilarityMatrix =
                        new DiagonalSimilarityMatrix(matchScore, mismatchScore, molType);
                    break;
                case AssemblyParameters.SimilarityMatrix:
                    string blosumFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.BlosumFilePathNode);
                    ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).SimilarityMatrix =
                        new SimilarityMatrix(blosumFilePath);
                    break;
                default:
                    ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).SimilarityMatrix =
                        new DiagonalSimilarityMatrix(matchScore, mismatchScore, molType);
                    break;
            }

            ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).GapOpenCost = gapCost;
            assembler.ConsensusResolver = new SimpleConsensusResolver(consensusThreshold);
            assembler.AssumeStandardOrientation = false;
            IOverlapDeNovoAssembly assembly;

            // Assembles all the sequences.
            if (isSeqAssemblyctr)
            {
                assembly = new OverlapDeNovoAssembly();
                assembly = (IOverlapDeNovoAssembly)assembler.Assemble(inputs);
            }
            else
            {
                assembly = (IOverlapDeNovoAssembly)assembler.Assemble(inputs);
            }

            assembly.GetObjectData(info, context);

            // Set Documentation property.
            assembly.Documentation = documentation;

            // Get the parameters from Xml in general
            int contigSequencesCount = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ContigSequencesCountNode), null);
            string contigConsensus = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ContigConsensusNode);

            switch (additionalParameter)
            {
                case AssemblyParameters.Consensus:
                    // Read the contig from Contig method.
                    Contig contigReadForConsensus = assembly.Contigs[0];
                    contigReadForConsensus.Consensus = null;
                    OverlapDeNovoAssembler simpleSeqAssembler = new OverlapDeNovoAssembler();
                    simpleSeqAssembler.ConsensusResolver = new SimpleConsensusResolver(consensusThreshold);
                    simpleSeqAssembler.MakeConsensus(alphabet, contigReadForConsensus);

                    // Log the required info.
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "SimpleConsensusMethod BVT : Consensus read is '{0}'.",
                        contigReadForConsensus.Consensus.ToString()));
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "SimpleConsensusMethod BVT : Consensus read is '{0}'.",
                        contigReadForConsensus.Consensus.ToString()));
                    Assert.AreEqual(contigConsensus, contigReadForConsensus.Consensus.ToString());
                    break;
                default:
                    // Get the parameters from Xml for Assemble() method test cases.
                    int unMergedCount = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.UnMergedSequencesCountNode), null);
                    int contigsCount = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.ContigsCountNode), null);

                    Assert.AreEqual(unMergedCount, assembly.UnmergedSequences.Count);
                    Assert.AreEqual(contigsCount, assembly.Contigs.Count);
                    Assert.AreEqual(documentation, assembly.Documentation);
                    Contig contigRead = assembly.Contigs[0];

                    // Logs the concensus
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "SequenceAssembly BVT : Un Merged Sequences Count is '{0}'.",
                        assembly.UnmergedSequences.Count.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "SequenceAssembly BVT : Contigs Count is '{0}'.",
                        assembly.Contigs.Count.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "SequenceAssembly BVT : Contig Sequences Count is '{0}'.",
                        contigRead.Sequences.Count.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "SequenceAssembly BVT : Consensus read is '{0}'.",
                        contigRead.Consensus.ToString()));
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "SequenceAssembly BVT : Consensus read is '{0}'.",
                        contigRead.Consensus.ToString()));

                    Assert.AreEqual(contigConsensus, contigRead.Consensus.ToString());
                    Assert.AreEqual(contigSequencesCount, contigRead.Sequences.Count);
                    break;
            }
        }

        /// <summary>
        /// Validate building Kmer using sequence and kmer length.
        /// </summary>
        /// <param name="nodeName">Name of the xml node for different test cases</param>
        /// <param name="IsKmerBuilder">True if validating kmerbuilder or else false.</param>
        void ValidateKmer(string nodeName, bool IsKmerBuilder)
        {
            // Get the parameters from Xml
            string Sequence = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.SequenceNode1);
            string expectedKmerCount = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.KmrSeqCountNode);
            string expectedKmerSeq = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.KmerSequenceNode);
            string expectedKmerPos = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.PositionsNode);

            // Create a Kmer Sequence.
            ISequence seq = new Sequence(Alphabets.DNA, Sequence);
            KmersOfSequence kmerSeq;

            if (IsKmerBuilder)
            {
                // Build Kmer.
                SequenceToKmerBuilder kmerBuilder = new SequenceToKmerBuilder();
                KmersOfSequence kmerList = kmerBuilder.Build(seq, 2);

                // Validate builder kmer.
                Assert.AreEqual(expectedKmerCount, kmerList.Kmers.Count.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedKmerSeq, kmerList.BaseSequence.ToString());
                Assert.AreEqual(expectedKmerPos, kmerList.Length.ToString((IFormatProvider)null));
            }
            else
            {
                kmerSeq = new KmersOfSequence(seq, 2);

                // Validate Kmer Seq.
                Assert.AreEqual(expectedKmerSeq, kmerSeq.BaseSequence.ToString());
                Assert.AreEqual(expectedKmerPos, kmerSeq.Length.ToString((IFormatProvider)null));
            }
        }

        /// <summary>
        /// Validates the Sequences for all the general test cases.
        /// </summary>
        /// <param name="nodeName">Xml Node Name</param>
        /// <param name="additionalParameter">Additional Parameter based 
        /// on which the validations are done.</param>
        void ValidateComputeFeature(
            string node,
            AssemblyParameters additionalParameter)
        {
            // Get the parameters from Xml
            string firstSequence = _utilityObj._xmlUtil.GetTextValue(node,
                Constants.SequenceNode1);
            string secondSequence = _utilityObj._xmlUtil.GetTextValue(node,
                Constants.SequenceNode2);
            string kmerLength = _utilityObj._xmlUtil.GetTextValue(node,
                Constants.KmerLengthNode);
            string expectedFeatureCount = _utilityObj._xmlUtil.GetTextValue(node,
                Constants.FeatureCount);
            string expectedFeature = _utilityObj._xmlUtil.GetTextValue(node,
                Constants.FeatureName);
            string expectedFeatureType = _utilityObj._xmlUtil.GetTextValue(node,
                Constants.FeatureType);
            string expectedStartIndex = _utilityObj._xmlUtil.GetTextValue(node,
                Constants.StartIndexNode);
            string expectedEndIndex = _utilityObj._xmlUtil.GetTextValue(node,
                Constants.EndIndexNode);

            ISequence seq1 = null;
            ISequence seq2 = null;
            // Create a Sequence.
            switch (additionalParameter)
            {
                case AssemblyParameters.Assemble:
                    Sequence seqObj1 =
                        new Sequence(Alphabets.Protein, firstSequence);
                    seqObj1.MoleculeType = MoleculeType.Protein;
                    Sequence seqObj2 =
                        new Sequence(Alphabets.Protein, secondSequence);
                    seqObj2.MoleculeType = MoleculeType.Protein;
                    seq1 = seqObj1;
                    seq2 = seqObj2;
                    break;
                case AssemblyParameters.Consensus:
                    seq1 = new Sequence(Alphabets.DNA, firstSequence);
                    seq2 = new Sequence(Alphabets.DNA, secondSequence);
                    break;
            }

            SequenceToKmerBuilder kmerBuilder = new SequenceToKmerBuilder();
            KmersOfSequence kmerList =
                kmerBuilder.Build(seq1, int.Parse(kmerLength, (IFormatProvider)null));
            List<WordMatch> nodes =
                WordMatch.BuildMatchTable(
                kmerList,
                seq1,
                seq2,
                int.Parse(kmerLength, (IFormatProvider)null));
            List<WordMatch> matchList =
                WordMatch.GetMinimalList(nodes, int.Parse(kmerLength, (IFormatProvider)null));
            List<DifferenceNode> diffNode =
                DifferenceNode.BuildDiffList(matchList, seq1, seq2);
            List<DifferenceNode.CompareFeature> features =
                DifferenceNode.OutputDiffList(diffNode, seq1, seq2);

            // Validate difference.

            Assert.AreEqual(expectedFeatureCount, features.Count.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedFeature, features[0].Feature);
            Assert.AreEqual(expectedFeatureType, features[0].FeatureType);
            Assert.AreEqual(expectedStartIndex, features[0].Start.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedEndIndex, features[0].End.ToString((IFormatProvider)null));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Kmer P1 : Validated DifferenceNodes successfully."));
        }

        #endregion Supported methods
    }
}