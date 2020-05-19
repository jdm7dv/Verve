// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Test
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using MBF.Algorithms.Alignment;
    using MBF.Algorithms.Assembly;
    using MBF.Util.Logging;
    using NUnit.Framework;
    using MBF.SimilarityMatrices;

    /// <summary>
    /// Test the assembly algorithms.
    /// </summary>
    [TestFixture]
    public class AssemblerTest
    {
        /// <summary>
        /// Initializes static members of the AssemblerTest class.
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static AssemblerTest()
        {
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Do an assembly using SimpleSequenceAssembler and verify results.
        /// </summary>
        [Test]
        public void TestSimpleSequenceAssembler()
        {
            Trace.Set(Trace.AssemblyDetails);   // turn on log dump

            // test parameters
            int matchScore = 1;
            int mismatchScore = -8;
            int gapCost = -8;
            double mergeThreshold = 4;
            double consensusThreshold = 66;

            Sequence seq1 = new Sequence(Alphabets.DNA, "GCCAAAATTTAGGC");
            Sequence seq2 = new Sequence(Alphabets.DNA, "TTATGGCGCCCACGGA");
            Sequence seq3 = new Sequence(Alphabets.DNA, "TATAAAGCGCCAA");

            // here is how the above sequences should align:
            // TATAAAGCGCCAA
            //         GCCAAAATTTAGGC
            //                   AGGCACCCGCGGTATT   <= reversed
            // 
            // TATAAAGCGCCAAAATTTAGGCACCCGCGGTATT
            OverlapDeNovoAssembler assembler = new OverlapDeNovoAssembler();
            assembler.MergeThreshold = mergeThreshold;
            assembler.OverlapAlgorithm = new PairwiseOverlapAligner();
            ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).SimilarityMatrix = new DiagonalSimilarityMatrix(matchScore, mismatchScore, MoleculeType.DNA);
            ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).GapOpenCost = gapCost;
            assembler.ConsensusResolver = new SimpleConsensusResolver(consensusThreshold);
            assembler.AssumeStandardOrientation = false;

            List<ISequence> inputs = new List<ISequence>();
            inputs.Add(seq1);
            inputs.Add(seq2);
            inputs.Add(seq3);

            IOverlapDeNovoAssembly seqAssembly = (IOverlapDeNovoAssembly)assembler.Assemble(inputs);

            Assert.AreEqual(0, seqAssembly.UnmergedSequences.Count);
            Assert.AreEqual(1, seqAssembly.Contigs.Count);
            Contig contig0 = seqAssembly.Contigs[0];
            Assert.AreEqual("TATAAAGCGCCAAAATTTAGGCACCCGCGGTATT", contig0.Consensus.ToString());
            Assert.AreEqual(3, contig0.Sequences.Count);
        }

        /// <summary>
        /// Do an assembly using SimpleSequenceAssembler using Swine Flu DNA sequences
        /// </summary>
        [Test]
        public void TestSimpleSequenceAssemblerWithSwineflu()
        {
            Trace.Set(Trace.AssemblyDetails);   // turn on log dump
            // test parameters
            int matchScore = 5;
            int mismatchScore = -4;
            int gapCost = -10;
            double mergeThreshold = 4;
            double consensusThreshold = 66;

            Sequence seq2 = new Sequence(Alphabets.DNA, "ACAAAAGCAACAAAAATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATGCAGACACATTATGTATAGGTTATCATGCGAACAATTCAACAGACACTGTAGACACAGTACTAGAAAAGAATGTAACAGTAACACACTCTGTTAACCTTCTAGAAGACAAGCATAACGGGAAACTATGCAAACTAAGAGGAGTAGCCCCATTGCATTTGGGTAAATGTAACATTGCTGGCTGGATCCTGGGAAATCCAGAGTGTGAATCACTCTCCACAGCAAGCTCATGGTCCTACATTGTGGAAACATCTAGTTCAGACAATGGAACGTGTTACCCAGGAGATTTCATCGATTATGAGGAGCTAAGAGAGCAATTGAGCTCAGTGTCATCATTTGAAAGGTTTGAGATATTCCCCAAGACAAGTTCATGGCCCAATCATGACTCGAACAAAGGTGTAACGGCAGCATGTCCTCATGCTGGAGCAAAAAGCTTCTACAAAAATTTAATATGGCTAGTTAAAAAAGGAAATTCATACCCAAAGCTCAGCAAATCCTACATTAATGATAAAGGGAAAGAAGTCCTCGTGCTATGGGGCATTCACCATCCATCTACTAGTGCTGACCAACAAAGTCTCTATCAGAATGCAGATGCATATGTTTTTGTGGGGTCATCAAGATATAGCAAGAAGTTCAAGCCGGAAATAGCAATAAGACCCAAAGTGAGGGATCAAGAAGGGAGAATGAACTATTACTGGACACTAGTAGAGCCGGGAGACAAAATAACATTCGAAGCAACTGGAAATCTAGTGGTACCGAGATATGCATTCGCAATGGAAAGAAATGCTGGATCTGGTATTATCATTTCAGATACACCAGTCCACGATTGCAATACAACTTGTCAGACACCCAAGGGTGCTATAAACACCAGCCTCCCATTTCAGAATATACATCCGATCACAATTGGAAAATGTCCAAAATATGTAAAAAGCACAAAATTGAGACTGGCCACAGGATTGAGGAATGTCCCGTCTATTCAATCTAGAGGCCTATTTGGGGCCATTGCCGGTTTCATTGAAGGGGGGTGGACAGGGATGGTAGATGGATGGTACGGTTATCACCATCAAAATGAGCAGGGGTCAGGATATGCAGCCGACCTGAAGAGCACACAGAATGCCATTGACGAGATTACTAACAAAGTAAATTCTGTTATTGAAAAGATGAATATACAGTTCACAGCAGTAGGTAAAGAGTTCAACCACCTGGAAAAAAGAATAGAGAATTTAAATAAAAAAGTTGATGATGGTTTCCTGGACATTTGGACTTACAATGCCGAACTGTTGGTTCTATTGGAAAATGAAAGAACTTTGGACTACCACGATTCGAATGTGAAGAACTTATATGAAAAGGTAAGAAGCCAGCTAAAAAACAATGCCAAGGAAATTGGAAACGGCTGCTTTGAATTTTACCACAAATGCGATAACACGTGCATGGAAAGTGTCAAAAATGGGACTTATGACTACCCAAAATACTCAGAGGAAGCAAAATTAAACAGAGAAGAAATAGATGGGGTAAAGCTGGAATCAACAAGGATTTACCAGATTTTGGCGATCTATTCAACTGTCGCCAGTTCATTGGTACTGGTAGTCTCCCTGGGGGCAATCAGTTTCTGGATGTGCTCTAATGGGTCTCTACAGTGTAGAATATGTATTTAACATTAGGATTTCAGAAGCATGAGAAA");
            Sequence seq1 = new Sequence(Alphabets.DNA, "ATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATGCAGACACATTATGTATAGGTTATCATGCGAACAATTCAACAGACACTGTAGACACAGTACTAGAAAAGAATGTAACAGTAACACACTCTGTTAACCTTCTAGAAGACAAGCATAACGGGAAACTATGCAAACTAAGAGGGGTAGCCCCATTGCATTTGGGTAAATGTAACATTGCTGGCTGGATCCTGGGAAATCCAGAGTGTGAATCACTCTCCACAGCAAGCTCATGGTCCTACATTGTGGAAACATCTAGTTCAGACAATGGAACGTGTTACCCAGGAGATTTCATCGATTATGAGGAGCTAAGAGAGCAATTGAGCTCAGTGTCATCATTTGAAAGGTTTGAGATATTCCCCAAGACAAGTTCATGGCCCAATCATGACTCGAACAAAGGTGTAACGGCAGCATGTCCTCATGCTGGAGCAAAAAGCTTCTACAAAAATTTAATATGGCTAGTTAAAAAAGGAAATTCATACCCAAAGCTCAGCAAATCCTACATTAATGATAAAGGGAAAGAAGTCCTCGTGCTATGGGGCATTCACCATCCATCTACTAGTGCTGACCAACAAAGTCTCTATCAGAATGCAGATGCATATGTTTTTGTGGGGACATCAAGATACAGCAAGAAGTTCAAGCCGGAAATAGCAATAAGACCCAAAGTGAGGGATCAAGAAGGGAGAATGAACTATTACTGGACACTAGTAGAGCCGGGAGACAAAATAACATTCGAAGCAACTGGAAATCTAGTGGTACCGAGATATGCATTCGCAATGGAAAGAAATGCTGGATCTGGTATTATCATTTCAGATACACCAGTCCACGATTGCAATACAACTTGTCAGACACCCAAGGGTGCTATAAACACCAGCCTCCCATTTCAGAATATACATCCGATCACAATTGGAAAATGTCCAAAATATGTAAAAAGCACAAAATTGAGACTGGCCACAGGATTGAGGAATGTCCCGTCTATTCAATCTAGAGGCCTATTTGGGGCCATTGCCGGTTTCATTGAAGGGGGGTGGACAGGGATGGTAGATGGATGGTACGGTTATCACCATCAAAATGAGCAGGGGTCAGGATATGCAGCCGACCTGAAGAGCACACAGAATGCCATTGACGAGATTACTAACAAAGTAAATTCTGTTATTGAAAAGATGAATACACAGTTCACAGCAGTAGGTAAAGAGTTCAACCACCTGGAAAAAAGAATAGAGAATTTAAATAAAAAAATTGATGATGGTTTCCTGGACATTTGGACTTACAATGCCGAACTGTTGGTTCTATTGGAAAATGAAAGAACTTTGGACTACCACGATTCAAATGTGAAGAACTTATATGAAAAGGTAAGAAGCCAGTTAAAAAACAATGCCAAGGAAATTGGAAACGGCTGCTTTGAATTTTACCACAAATGCGATAACACGTGCATGGAAAGTGTCAAAAATGGGACTTATGACTACCCAAAATACTCAGAGGAAGCAAAATTAAACAGAGAAGAAATAGATGGGGTAAAGCTGGAATCAACAAGGATTTACCAGATTTTGGCGATCTATTCAACTGTCGCCAGTTCATTGGTACTGGTAGTCTCCCTGGGGGCAATCAGTTTCTGGATGTGCTCTAATGGGTCTCTACAGTGTAGAATATGTATTTAA");
           
            OverlapDeNovoAssembler assembler = new OverlapDeNovoAssembler();
            assembler.MergeThreshold = mergeThreshold;
            assembler.OverlapAlgorithm = new NeedlemanWunschAligner();
            ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).SimilarityMatrix = new DiagonalSimilarityMatrix(matchScore, mismatchScore, MoleculeType.DNA);
            ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).GapOpenCost = gapCost;
            assembler.ConsensusResolver = new SimpleConsensusResolver(consensusThreshold);
            assembler.AssumeStandardOrientation = false;

            List<ISequence> inputs = new List<ISequence>();
            inputs.Add(seq1);
            inputs.Add(seq2);

            IOverlapDeNovoAssembly seqAssembly = (IOverlapDeNovoAssembly)assembler.Assemble(inputs);

            Assert.AreEqual(0, seqAssembly.UnmergedSequences.Count);
            Assert.AreEqual(1, seqAssembly.Contigs.Count);
            Contig contig0 = seqAssembly.Contigs[0];
            Assert.AreEqual("ACAAAAGCAACAAAAATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATGCAGACACATTATGTATAGGTTATCATGCGAACAATTCAACAGACACTGTAGACACAGTACTAGAAAAGAATGTAACAGTAACACACTCTGTTAACCTTCTAGAAGACAAGCATAACGGGAAACTATGCAAACTAAGAGGRGTAGCCCCATTGCATTTGGGTAAATGTAACATTGCTGGCTGGATCCTGGGAAATCCAGAGTGTGAATCACTCTCCACAGCAAGCTCATGGTCCTACATTGTGGAAACATCTAGTTCAGACAATGGAACGTGTTACCCAGGAGATTTCATCGATTATGAGGAGCTAAGAGAGCAATTGAGCTCAGTGTCATCATTTGAAAGGTTTGAGATATTCCCCAAGACAAGTTCATGGCCCAATCATGACTCGAACAAAGGTGTAACGGCAGCATGTCCTCATGCTGGAGCAAAAAGCTTCTACAAAAATTTAATATGGCTAGTTAAAAAAGGAAATTCATACCCAAAGCTCAGCAAATCCTACATTAATGATAAAGGGAAAGAAGTCCTCGTGCTATGGGGCATTCACCATCCATCTACTAGTGCTGACCAACAAAGTCTCTATCAGAATGCAGATGCATATGTTTTTGTGGGGWCATCAAGATAYAGCAAGAAGTTCAAGCCGGAAATAGCAATAAGACCCAAAGTGAGGGATCAAGAAGGGAGAATGAACTATTACTGGACACTAGTAGAGCCGGGAGACAAAATAACATTCGAAGCAACTGGAAATCTAGTGGTACCGAGATATGCATTCGCAATGGAAAGAAATGCTGGATCTGGTATTATCATTTCAGATACACCAGTCCACGATTGCAATACAACTTGTCAGACACCCAAGGGTGCTATAAACACCAGCCTCCCATTTCAGAATATACATCCGATCACAATTGGAAAATGTCCAAAATATGTAAAAAGCACAAAATTGAGACTGGCCACAGGATTGAGGAATGTCCCGTCTATTCAATCTAGAGGCCTATTTGGGGCCATTGCCGGTTTCATTGAAGGGGGGTGGACAGGGATGGTAGATGGATGGTACGGTTATCACCATCAAAATGAGCAGGGGTCAGGATATGCAGCCGACCTGAAGAGCACACAGAATGCCATTGACGAGATTACTAACAAAGTAAATTCTGTTATTGAAAAGATGAATAYACAGTTCACAGCAGTAGGTAAAGAGTTCAACCACCTGGAAAAAAGAATAGAGAATTTAAATAAAAAARTTGATGATGGTTTCCTGGACATTTGGACTTACAATGCCGAACTGTTGGTTCTATTGGAAAATGAAAGAACTTTGGACTACCACGATTCRAATGTGAAGAACTTATATGAAAAGGTAAGAAGCCAGYTAAAAAACAATGCCAAGGAAATTGGAAACGGCTGCTTTGAATTTTACCACAAATGCGATAACACGTGCATGGAAAGTGTCAAAAATGGGACTTATGACTACCCAAAATACTCAGAGGAAGCAAAATTAAACAGAGAAGAAATAGATGGGGTAAAGCTGGAATCAACAAGGATTTACCAGATTTTGGCGATCTATTCAACTGTCGCCAGTTCATTGGTACTGGTAGTCTCCCTGGGGGCAATCAGTTTCTGGATGTGCTCTAATGGGTCTCTACAGTGTAGAATATGTATTTAACATTAGGATTTCAGAAGCATGAGAAA", contig0.Consensus.ToString());
            Assert.AreEqual(2, contig0.Sequences.Count);

            assembler.OverlapAlgorithm = new SmithWatermanAligner();

            seqAssembly = (IOverlapDeNovoAssembly)assembler.Assemble(inputs);

            Assert.AreEqual(0, seqAssembly.UnmergedSequences.Count);
            Assert.AreEqual(1, seqAssembly.Contigs.Count);
            contig0 = seqAssembly.Contigs[0];
            Assert.AreEqual("ACAAAAGCAACAAAAATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATGCAGACACATTATGTATAGGTTATCATGCGAACAATTCAACAGACACTGTAGACACAGTACTAGAAAAGAATGTAACAGTAACACACTCTGTTAACCTTCTAGAAGACAAGCATAACGGGAAACTATGCAAACTAAGAGGRGTAGCCCCATTGCATTTGGGTAAATGTAACATTGCTGGCTGGATCCTGGGAAATCCAGAGTGTGAATCACTCTCCACAGCAAGCTCATGGTCCTACATTGTGGAAACATCTAGTTCAGACAATGGAACGTGTTACCCAGGAGATTTCATCGATTATGAGGAGCTAAGAGAGCAATTGAGCTCAGTGTCATCATTTGAAAGGTTTGAGATATTCCCCAAGACAAGTTCATGGCCCAATCATGACTCGAACAAAGGTGTAACGGCAGCATGTCCTCATGCTGGAGCAAAAAGCTTCTACAAAAATTTAATATGGCTAGTTAAAAAAGGAAATTCATACCCAAAGCTCAGCAAATCCTACATTAATGATAAAGGGAAAGAAGTCCTCGTGCTATGGGGCATTCACCATCCATCTACTAGTGCTGACCAACAAAGTCTCTATCAGAATGCAGATGCATATGTTTTTGTGGGGWCATCAAGATAYAGCAAGAAGTTCAAGCCGGAAATAGCAATAAGACCCAAAGTGAGGGATCAAGAAGGGAGAATGAACTATTACTGGACACTAGTAGAGCCGGGAGACAAAATAACATTCGAAGCAACTGGAAATCTAGTGGTACCGAGATATGCATTCGCAATGGAAAGAAATGCTGGATCTGGTATTATCATTTCAGATACACCAGTCCACGATTGCAATACAACTTGTCAGACACCCAAGGGTGCTATAAACACCAGCCTCCCATTTCAGAATATACATCCGATCACAATTGGAAAATGTCCAAAATATGTAAAAAGCACAAAATTGAGACTGGCCACAGGATTGAGGAATGTCCCGTCTATTCAATCTAGAGGCCTATTTGGGGCCATTGCCGGTTTCATTGAAGGGGGGTGGACAGGGATGGTAGATGGATGGTACGGTTATCACCATCAAAATGAGCAGGGGTCAGGATATGCAGCCGACCTGAAGAGCACACAGAATGCCATTGACGAGATTACTAACAAAGTAAATTCTGTTATTGAAAAGATGAATAYACAGTTCACAGCAGTAGGTAAAGAGTTCAACCACCTGGAAAAAAGAATAGAGAATTTAAATAAAAAARTTGATGATGGTTTCCTGGACATTTGGACTTACAATGCCGAACTGTTGGTTCTATTGGAAAATGAAAGAACTTTGGACTACCACGATTCRAATGTGAAGAACTTATATGAAAAGGTAAGAAGCCAGYTAAAAAACAATGCCAAGGAAATTGGAAACGGCTGCTTTGAATTTTACCACAAATGCGATAACACGTGCATGGAAAGTGTCAAAAATGGGACTTATGACTACCCAAAATACTCAGAGGAAGCAAAATTAAACAGAGAAGAAATAGATGGGGTAAAGCTGGAATCAACAAGGATTTACCAGATTTTGGCGATCTATTCAACTGTCGCCAGTTCATTGGTACTGGTAGTCTCCCTGGGGGCAATCAGTTTCTGGATGTGCTCTAATGGGTCTCTACAGTGTAGAATATGTATTTAACATTAGGATTTCAGAAGCATGAGAAA", contig0.Consensus.ToString());
            Assert.AreEqual(2, contig0.Sequences.Count);

            assembler.OverlapAlgorithm = new PairwiseOverlapAligner();

            seqAssembly = (IOverlapDeNovoAssembly)assembler.Assemble(inputs);

            Assert.AreEqual(0, seqAssembly.UnmergedSequences.Count);
            Assert.AreEqual(1, seqAssembly.Contigs.Count);
            contig0 = seqAssembly.Contigs[0];
            Assert.AreEqual("ACAAAAGCAACAAAAATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATGCAGACACATTATGTATAGGTTATCATGCGAACAATTCAACAGACACTGTAGACACAGTACTAGAAAAGAATGTAACAGTAACACACTCTGTTAACCTTCTAGAAGACAAGCATAACGGGAAACTATGCAAACTAAGAGGRGTAGCCCCATTGCATTTGGGTAAATGTAACATTGCTGGCTGGATCCTGGGAAATCCAGAGTGTGAATCACTCTCCACAGCAAGCTCATGGTCCTACATTGTGGAAACATCTAGTTCAGACAATGGAACGTGTTACCCAGGAGATTTCATCGATTATGAGGAGCTAAGAGAGCAATTGAGCTCAGTGTCATCATTTGAAAGGTTTGAGATATTCCCCAAGACAAGTTCATGGCCCAATCATGACTCGAACAAAGGTGTAACGGCAGCATGTCCTCATGCTGGAGCAAAAAGCTTCTACAAAAATTTAATATGGCTAGTTAAAAAAGGAAATTCATACCCAAAGCTCAGCAAATCCTACATTAATGATAAAGGGAAAGAAGTCCTCGTGCTATGGGGCATTCACCATCCATCTACTAGTGCTGACCAACAAAGTCTCTATCAGAATGCAGATGCATATGTTTTTGTGGGGWCATCAAGATAYAGCAAGAAGTTCAAGCCGGAAATAGCAATAAGACCCAAAGTGAGGGATCAAGAAGGGAGAATGAACTATTACTGGACACTAGTAGAGCCGGGAGACAAAATAACATTCGAAGCAACTGGAAATCTAGTGGTACCGAGATATGCATTCGCAATGGAAAGAAATGCTGGATCTGGTATTATCATTTCAGATACACCAGTCCACGATTGCAATACAACTTGTCAGACACCCAAGGGTGCTATAAACACCAGCCTCCCATTTCAGAATATACATCCGATCACAATTGGAAAATGTCCAAAATATGTAAAAAGCACAAAATTGAGACTGGCCACAGGATTGAGGAATGTCCCGTCTATTCAATCTAGAGGCCTATTTGGGGCCATTGCCGGTTTCATTGAAGGGGGGTGGACAGGGATGGTAGATGGATGGTACGGTTATCACCATCAAAATGAGCAGGGGTCAGGATATGCAGCCGACCTGAAGAGCACACAGAATGCCATTGACGAGATTACTAACAAAGTAAATTCTGTTATTGAAAAGATGAATAYACAGTTCACAGCAGTAGGTAAAGAGTTCAACCACCTGGAAAAAAGAATAGAGAATTTAAATAAAAAARTTGATGATGGTTTCCTGGACATTTGGACTTACAATGCCGAACTGTTGGTTCTATTGGAAAATGAAAGAACTTTGGACTACCACGATTCRAATGTGAAGAACTTATATGAAAAGGTAAGAAGCCAGYTAAAAAACAATGCCAAGGAAATTGGAAACGGCTGCTTTGAATTTTACCACAAATGCGATAACACGTGCATGGAAAGTGTCAAAAATGGGACTTATGACTACCCAAAATACTCAGAGGAAGCAAAATTAAACAGAGAAGAAATAGATGGGGTAAAGCTGGAATCAACAAGGATTTACCAGATTTTGGCGATCTATTCAACTGTCGCCAGTTCATTGGTACTGGTAGTCTCCCTGGGGGCAATCAGTTTCTGGATGTGCTCTAATGGGTCTCTACAGTGTAGAATATGTATTTAACATTAGGATTTCAGAAGCATGAGAAA", contig0.Consensus.ToString());
            Assert.AreEqual(2, contig0.Sequences.Count);

            assembler.OverlapAlgorithm = new MUMmer3();

            seqAssembly = (IOverlapDeNovoAssembly)assembler.Assemble(inputs);

            Assert.AreEqual(0, seqAssembly.UnmergedSequences.Count);
            Assert.AreEqual(1, seqAssembly.Contigs.Count);
            contig0 = seqAssembly.Contigs[0];
            Assert.AreEqual("ACAAAAGCAACAAAAATGAAGGCAATACTAGTAGTTCTGCTATATACATTTGCAACCGCAAATGCAGACACATTATGTATAGGTTATCATGCGAACAATTCAACAGACACTGTAGACACAGTACTAGAAAAGAATGTAACAGTAACACACTCTGTTAACCTTCTAGAAGACAAGCATAACGGGAAACTATGCAAACTAAGAGGRGTAGCCCCATTGCATTTGGGTAAATGTAACATTGCTGGCTGGATCCTGGGAAATCCAGAGTGTGAATCACTCTCCACAGCAAGCTCATGGTCCTACATTGTGGAAACATCTAGTTCAGACAATGGAACGTGTTACCCAGGAGATTTCATCGATTATGAGGAGCTAAGAGAGCAATTGAGCTCAGTGTCATCATTTGAAAGGTTTGAGATATTCCCCAAGACAAGTTCATGGCCCAATCATGACTCGAACAAAGGTGTAACGGCAGCATGTCCTCATGCTGGAGCAAAAAGCTTCTACAAAAATTTAATATGGCTAGTTAAAAAAGGAAATTCATACCCAAAGCTCAGCAAATCCTACATTAATGATAAAGGGAAAGAAGTCCTCGTGCTATGGGGCATTCACCATCCATCTACTAGTGCTGACCAACAAAGTCTCTATCAGAATGCAGATGCATATGTTTTTGTGGGGWCATCAAGATAYAGCAAGAAGTTCAAGCCGGAAATAGCAATAAGACCCAAAGTGAGGGATCAAGAAGGGAGAATGAACTATTACTGGACACTAGTAGAGCCGGGAGACAAAATAACATTCGAAGCAACTGGAAATCTAGTGGTACCGAGATATGCATTCGCAATGGAAAGAAATGCTGGATCTGGTATTATCATTTCAGATACACCAGTCCACGATTGCAATACAACTTGTCAGACACCCAAGGGTGCTATAAACACCAGCCTCCCATTTCAGAATATACATCCGATCACAATTGGAAAATGTCCAAAATATGTAAAAAGCACAAAATTGAGACTGGCCACAGGATTGAGGAATGTCCCGTCTATTCAATCTAGAGGCCTATTTGGGGCCATTGCCGGTTTCATTGAAGGGGGGTGGACAGGGATGGTAGATGGATGGTACGGTTATCACCATCAAAATGAGCAGGGGTCAGGATATGCAGCCGACCTGAAGAGCACACAGAATGCCATTGACGAGATTACTAACAAAGTAAATTCTGTTATTGAAAAGATGAATAYACAGTTCACAGCAGTAGGTAAAGAGTTCAACCACCTGGAAAAAAGAATAGAGAATTTAAATAAAAAARTTGATGATGGTTTCCTGGACATTTGGACTTACAATGCCGAACTGTTGGTTCTATTGGAAAATGAAAGAACTTTGGACTACCACGATTCRAATGTGAAGAACTTATATGAAAAGGTAAGAAGCCAGYTAAAAAACAATGCCAAGGAAATTGGAAACGGCTGCTTTGAATTTTACCACAAATGCGATAACACGTGCATGGAAAGTGTCAAAAATGGGACTTATGACTACCCAAAATACTCAGAGGAAGCAAAATTAAACAGAGAAGAAATAGATGGGGTAAAGCTGGAATCAACAAGGATTTACCAGATTTTGGCGATCTATTCAACTGTCGCCAGTTCATTGGTACTGGTAGTCTCCCTGGGGGCAATCAGTTTCTGGATGTGCTCTAATGGGTCTCTACAGTGTAGAATATGTATTTAACATTAGGATTTCAGAAGCATGAGAAA", contig0.Consensus.ToString());
            Assert.AreEqual(2, contig0.Sequences.Count);
        }

        /// <summary>
        /// Do an assembly using SimpleSequenceAssembler and a short semi-random sequence.
        /// This is a regression test.
        /// </summary>
        [Test]
        public void TestSimpleSequenceAssemblerWithSemiRandomSequence()
        {
            // test parameters
            int matchScore = 1;
            int mismatchScore = -8;
            int gapCost = -8;
            double mergeThreshold = 4;
            double consensusThreshold = 66;
            const int MasterLength = 30;
            const int ReadLength = 10;
            const int NumReads = 5;
            const bool AssumeOrientedReads = false;

            // if this is uncommented, assembly details appear in log.
            // this is extremely verbose.
            Trace.Set(Trace.AssemblyDetails);

            // make random master sequence
            // (use seed for repeatability, or omit seed for 
            // different test each time)
            // Random randGen = new Random();
            Random randGen = new Random(654321);

            StringBuilder randSeq = new StringBuilder();
            for (int i = 0; i < MasterLength; ++i)
            {
                int randm = randGen.Next(8);
                if (randm < 2)
                {
                    randSeq.Append('A');
                }
                else if (randm < 4)
                {
                    randSeq.Append('C');
                }
                else if (randm < 6)
                {
                    randSeq.Append('G');
                }
                else
                {
                    randSeq.Append('T');
                }
            }

            Sequence master = new Sequence(Alphabets.DNA, randSeq.ToString());

            // create the reads
            List<ISequence> inputs = new List<ISequence>();
            for (int i = 0; i < NumReads; ++i)
            {
                int pos = 5 * i;
                string data = master.ToString().Substring(pos, ReadLength);
                bool revcomp = randGen.Next(2) > 0;
                bool reverse = randGen.Next(2) > 0 && !AssumeOrientedReads;
                ISequence read;
                if (reverse && revcomp)
                {
                    Sequence tmp = new Sequence(Alphabets.DNA, data);
                    read = new Sequence(Alphabets.DNA, tmp.Reverse.ToString());
                }
                else if (revcomp)
                {
                    Sequence tmp = new Sequence(Alphabets.DNA, data);
                    read = new Sequence(Alphabets.DNA, tmp.ReverseComplement.ToString());
                }
                else
                {
                    read = new Sequence(Alphabets.DNA, data);
                }

                inputs.Add(read);
            }

            OverlapDeNovoAssembler assembler = new OverlapDeNovoAssembler();
            assembler.MergeThreshold = mergeThreshold;
            assembler.OverlapAlgorithm = new PairwiseOverlapAligner();
            ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).SimilarityMatrix = new DiagonalSimilarityMatrix(matchScore, mismatchScore, MoleculeType.DNA);
            ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).GapOpenCost = gapCost;
            assembler.ConsensusResolver = new SimpleConsensusResolver(consensusThreshold);
            assembler.AssumeStandardOrientation = AssumeOrientedReads;

            IOverlapDeNovoAssembly seqAssembly = (IOverlapDeNovoAssembly)assembler.Assemble(inputs);

            Assert.AreEqual(0, seqAssembly.UnmergedSequences.Count);
            Assert.AreEqual(1, seqAssembly.Contigs.Count);
            Contig contig0 = seqAssembly.Contigs[0];
            ApplicationLog.WriteLine("master sequence and contig 0 consensus:");
            ApplicationLog.WriteLine(master.ToString());
            ApplicationLog.WriteLine(contig0.Consensus.ToString());

            // note that this is tricky, esp. without oriented reads - consensus
            // could be reversed and/or complemented relative to original
            Assert.AreEqual(master.ToString(), contig0.Consensus.ToString());
        }

        /// <summary>
        /// Do an assembly using SimpleSequenceAssembler and a long, random seqeunce
        /// with shorter reads randomly selected from it.
        /// </summary>
        [Test]
        public void TestSimpleSequenceAssemblerWithRandomSequence()
        {
            // Test parameters.
            //
            // In theory, as long as all positions in the master sequence are 
            // covered by at least one read, we should be able to pass this test.
            // But some parameter settings will make the test fail, for
            // various reasons, including:
            // 1. Short reads, caused by the strategy used to ensure full coverage
            //  at the ends, might not score well enough to merge.
            // 2. Uncovered positions are always possible due to the random 
            //  generation of reads. (Increasing the number of reads helps with this)
            // 3. The assembler might construct the reverse or complement (or both)
            //  of the master sequence.
            // 4. Too low a merge threshold could cause incorrect merges, which
            //  the algorithm will not repair.
            int matchScore = 1;
            int mismatchScore = -8;
            int gapCost = -8;
            double mergeThreshold = 3;
            double consensusThreshold = 99;
            const int MasterLength = 100;
            const int MinReadLength = 10;
            const int MaxReadLength = 30;
            const int NumReads = 200;
            const bool AssumeOrientedReads = true;

            // if this is uncommented, assembly details appear in log.
            // this is extremely verbose.
            // Trace.Set(Trace.AssemblyDetails);

            // make random master sequence
            // (use seed for repeatability, or omit seed for 
            // different test each time)
            // Random randGen = new Random();
            Random randGen = new Random(654321);

            StringBuilder randSeq = new StringBuilder();
            for (int i = 0; i < MasterLength; ++i)
            {
                int randm = randGen.Next(8);
                if (randm < 2)
                {
                    randSeq.Append('A');
                }
                else if (randm < 4)
                {
                    randSeq.Append('C');
                }
                else if (randm < 6)
                {
                    randSeq.Append('G');
                }
                else
                {
                    randSeq.Append('T');
                }
            }

            Sequence master = new Sequence(Alphabets.DNA, randSeq.ToString());

            // create the reads
            List<ISequence> inputs = new List<ISequence>();
            for (int i = 0; i < NumReads; ++i)
            {
                // try for uniform coverage clear to the ends (this can lead to short reads, though)
                int rndPos = Math.Max(0, randGen.Next(-MinReadLength, MasterLength - 1));
                int rndLen = Math.Min(MasterLength - rndPos, randGen.Next(MinReadLength, MaxReadLength + 1));
                string data = master.ToString().Substring(Math.Max(0, rndPos), rndLen);
                bool revcomp = randGen.Next(2) > 0;
                bool reverse = randGen.Next(2) > 0 && !AssumeOrientedReads;
                ISequence read;
                if (reverse && revcomp)
                {
                    Sequence tmp = new Sequence(Alphabets.DNA, data);
                    read = new Sequence(Alphabets.DNA, tmp.Reverse.ToString());
                }
                else if (revcomp)
                {
                    Sequence tmp = new Sequence(Alphabets.DNA, data);
                    read = new Sequence(Alphabets.DNA, tmp.ReverseComplement.ToString());
                }
                else
                {
                    read = new Sequence(Alphabets.DNA, data);
                }

                ApplicationLog.WriteLine("read {0}: {1}", i, read);
                inputs.Add(read);
            }

            OverlapDeNovoAssembler assembler = new OverlapDeNovoAssembler();
            assembler.MergeThreshold = mergeThreshold;
            assembler.OverlapAlgorithm = new PairwiseOverlapAligner();
            ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).SimilarityMatrix = new DiagonalSimilarityMatrix(matchScore, mismatchScore, MoleculeType.DNA);
            ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).GapOpenCost = gapCost;
            assembler.ConsensusResolver = new SimpleConsensusResolver(consensusThreshold);
            assembler.AssumeStandardOrientation = AssumeOrientedReads;

            IOverlapDeNovoAssembly seqAssembly = (IOverlapDeNovoAssembly)assembler.Assemble(inputs);

            ApplicationLog.WriteLine(
                "Assembly finished. Contigs: {0}. Unmerged sequences: {1}.",
                seqAssembly.Contigs.Count, 
                seqAssembly.UnmergedSequences.Count);
            Contig contig0 = seqAssembly.Contigs[0];
            ApplicationLog.WriteLine("master sequence and contig 0 consensus:");
            ApplicationLog.WriteLine(master.ToString());
            ApplicationLog.WriteLine(contig0.Consensus.ToString());

            Assert.AreEqual(2, seqAssembly.UnmergedSequences.Count);
            Assert.AreEqual(1, seqAssembly.Contigs.Count);

            // note that this is tricky, esp. without oriented reads - consensus
            // could be reversed and/or complemented relative to original
            Assert.AreEqual(master.ToString(), contig0.Consensus.ToString());
        }

        /// <summary>
        /// Tests the name and description property of the assemblers.
        /// </summary>
        [Test]
        public void TestAssemblerProperties()
        {
            IDeNovoAssembler assembler = new OverlapDeNovoAssembler();

            Assert.AreEqual(assembler.Name, Properties.Resource.SIMPLE_NAME);
            Assert.AreEqual(assembler.Description, Properties.Resource.SIMPLE_DESCRIPTION);
        }
    }
}
