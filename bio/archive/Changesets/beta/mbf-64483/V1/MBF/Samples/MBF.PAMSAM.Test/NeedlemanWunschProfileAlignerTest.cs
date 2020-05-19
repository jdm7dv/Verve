// -------------------------------------------------------------------------------------
// <copyright file="NeedlemanWunschProfileAlignerTest.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Contains test cases for NeedlemanWunschProfileAligner class.
// </summary>
// -------------------------------------------------------------------------------------

namespace MBF.Test
{
    using System;
    using System.Collections.Generic;
    using MBF.Algorithms;
    using MBF.Algorithms.Alignment;
    using MBF.Algorithms.Alignment.MultipleSequenceAlignment;
    using MBF.Util.Logging;
    using MBF.IO;
    using MBF.IO.Fasta;
    using NUnit.Framework;
    using MBF.SimilarityMatrices;

    /// <summary>
    /// Test for NeedlemanWunschProfileAligner class
    /// </summary>
    [TestFixture]
    public class NeedlemanWunschProfileAlignerTest
    {
        /// <summary>
        /// Test NeedlemanWunschProfileAligner class
        /// </summary>
        [Test]
        public void TestNeedlemanWunschProfileAligner()
        {
            Console.WriteLine("Number of logical processors: {0}", Environment.ProcessorCount);

            ISequence templateSequence = new Sequence(Alphabets.DNA, "ATGCSWRYKMBVHDN-");
            Dictionary<ISequenceItem, int> itemSet = new Dictionary<ISequenceItem, int>();
            for (int i = 0; i < templateSequence.Count; ++i)
            {
                itemSet.Add(templateSequence[i], i);
            }
            Profiles.ItemSet = itemSet;



            SimilarityMatrix similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            int gapOpenPenalty = -3;
            int gapExtendPenalty = -1;

            IProfileAligner profileAligner = new NeedlemanWunschProfileAlignerSerial(similarityMatrix, ProfileScoreFunctionNames.WeightedInnerProduct,
                                                                                gapOpenPenalty, gapExtendPenalty, Environment.ProcessorCount);

            ISequence seqA = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            ISequence seqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG");

            List<ISequence> sequences = new List<ISequence>();
            sequences.Add(seqA);
            sequences.Add(seqB);

            IProfileAlignment profileAlignmentA = ProfileAlignment.GenerateProfileAlignment(sequences[0]);
            IProfileAlignment profileAlignmentB = ProfileAlignment.GenerateProfileAlignment(sequences[1]);
            profileAligner.Align(profileAlignmentA, profileAlignmentB);


            List<int> eStringSubtree = profileAligner.GenerateEString(profileAligner.AlignedA);
            List<int> eStringSubtreeB = profileAligner.GenerateEString(profileAligner.AlignedB);

            List<ISequence> alignedSequences = new List<ISequence>();

            ISequence seq = profileAligner.GenerateSequenceFromEString(eStringSubtree, sequences[0]);
            alignedSequences.Add(seq);
            seq = profileAligner.GenerateSequenceFromEString(eStringSubtreeB, sequences[1]);
            alignedSequences.Add(seq);

            float profileScore = MsaUtils.MultipleAlignmentScoreFunction(alignedSequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            Console.WriteLine("alignment score is: {0}", profileScore);

            Console.WriteLine("the aligned sequences are:");
            for (int i = 0; i < alignedSequences.Count; ++i)
            {
                Console.WriteLine(alignedSequences[i].ToString());
            }

            ISequence expectedSeqA = new Sequence(Alphabets.DNA, "GGGAA---AAATCAGATT");
            ISequence expectedSeqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG---");

            // Test on case 3: 36 sequences
            ISequenceParser parser = new FastaParser();
            string filepath = @"testdata\FASTA\RV11_BBS_all.afa";
            IList<ISequence> orgSequences = parser.Parse(filepath);

            sequences = MsaUtils.UnAlign(orgSequences);

            int numberOfSequences = orgSequences.Count;

            Console.WriteLine("Original unaligned sequences are:");
            for (int i = 0; i < numberOfSequences; ++i)
            {
                Console.WriteLine(">");
                Console.WriteLine(sequences[i].ToString());
            }

            for (int i = 1; i < numberOfSequences - 1; ++i)
            {
                for (int j = i + 1; j < numberOfSequences; ++j)
                {
                    profileAlignmentA = ProfileAlignment.GenerateProfileAlignment(sequences[i]);
                    profileAlignmentB = ProfileAlignment.GenerateProfileAlignment(sequences[j]);

                    profileAligner = new NeedlemanWunschProfileAlignerSerial(similarityMatrix, ProfileScoreFunctionNames.WeightedInnerProduct,
                                                                                gapOpenPenalty, gapExtendPenalty, Environment.ProcessorCount);
                    profileAligner.Align(profileAlignmentA, profileAlignmentB);

                    eStringSubtree = profileAligner.GenerateEString(profileAligner.AlignedA);
                    eStringSubtreeB = profileAligner.GenerateEString(profileAligner.AlignedB);

                    Console.WriteLine("Sequences lengths are: {0}-{1}", sequences[i].Count, sequences[j].Count);
                    Console.WriteLine("estring 1:");
                    for (int k = 0; k < eStringSubtree.Count; ++k)
                    {
                        Console.Write("{0}\t", eStringSubtree[k]);
                    }
                    Console.WriteLine("\nestring 2:");
                    for (int k = 0; k < eStringSubtreeB.Count; ++k)
                    {
                        Console.Write("{0}\t", eStringSubtreeB[k]);
                    }

                    alignedSequences = new List<ISequence>();

                    seq = profileAligner.GenerateSequenceFromEString(eStringSubtree, sequences[i]);
                    alignedSequences.Add(seq);
                    seq = profileAligner.GenerateSequenceFromEString(eStringSubtreeB, sequences[j]);
                    alignedSequences.Add(seq);

                    profileScore = MsaUtils.MultipleAlignmentScoreFunction(alignedSequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

                    Console.WriteLine("\nalignment score is: {0}", profileScore);

                    Console.WriteLine("the aligned sequences are:");
                    for (int k = 0; k < alignedSequences.Count; ++k)
                    {
                        Console.WriteLine(alignedSequences[k].ToString());
                    }
                }
            }
        }
    }
}