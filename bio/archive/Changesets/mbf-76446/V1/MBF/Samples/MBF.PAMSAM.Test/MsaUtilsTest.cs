// -------------------------------------------------------------------------------------
// <copyright file="AlignmentScoreTest.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Contains test cases for AlignmentScore functions.
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
    using NUnit.Framework;
    using MBF.SimilarityMatrices;

    /// <summary>
    /// Test for AlignmentScore functions
    /// </summary>
    [TestFixture]
    public class AlignmentScoreTest
    {
        /// <summary>
        /// Test AlignmentScore functions
        /// </summary>
        [Test]
        public void TestAlignmentScore()
        {

            ISequence templateSequence = new Sequence(Alphabets.DNA, "ATGCSWRYKMBVHDN-");
            Dictionary<ISequenceItem, int> itemSet = new Dictionary<ISequenceItem, int>();
            for (int i = 0; i < templateSequence.Count; ++i)
            {
                itemSet.Add(templateSequence[i], i);
            }
            Profiles.ItemSet = itemSet;

            SimilarityMatrix similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            int gapOpenPenalty = -8;
            int gapExtendPenalty = -1;

            // Test PairWiseScoreFunction
            ISequence seqA = new Sequence(Alphabets.DNA, "ACG");
            ISequence seqB = new Sequence(Alphabets.DNA, "ACG");
            float score = MsaUtils.PairWiseScoreFunction(seqA, seqB, similarityMatrix, gapOpenPenalty, gapExtendPenalty);




            //Assert.AreEqual(15, score);

            seqA = new Sequence(Alphabets.DNA, "ACG");
            seqB = new Sequence(Alphabets.DNA, "ACC");

            score = MsaUtils.PairWiseScoreFunction(seqA, seqB, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            //Assert.AreEqual(6, score);

            seqA = new Sequence(Alphabets.DNA, "AC-");
            seqB = new Sequence(Alphabets.DNA, "ACC");

            score = MsaUtils.PairWiseScoreFunction(seqA, seqB, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            //Assert.AreEqual(2, score);

            seqA = new Sequence(Alphabets.DNA, "AC--");
            seqB = new Sequence(Alphabets.DNA, "ACCG");

            score = MsaUtils.PairWiseScoreFunction(seqA, seqB, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            //Assert.AreEqual(1, score);


            seqA = new Sequence(Alphabets.DNA, "A---");
            seqB = new Sequence(Alphabets.DNA, "A--C");

            score = MsaUtils.PairWiseScoreFunction(seqA, seqB, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            //Assert.AreEqual(-3, score);


            seqA = new Sequence(Alphabets.DNA, "GGGA---AAAATCAGATT");
            seqB = new Sequence(Alphabets.DNA, "GGGA--CAAAATCAG---");

            score = MsaUtils.PairWiseScoreFunction(seqA, seqB, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            //Assert.AreEqual(42, score);

            seqA = new Sequence(Alphabets.DNA, "GGG---AAAAATCAGATT");
            seqB = new Sequence(Alphabets.DNA, "GGGA--CAAAATCAG---");

            score = MsaUtils.PairWiseScoreFunction(seqA, seqB, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            //Assert.AreEqual(33, score);

            seqA = new Sequence(Alphabets.DNA, "GGGA---AAAATCAGATT");
            seqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG---");

            score = MsaUtils.PairWiseScoreFunction(seqA, seqB, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            //Assert.AreEqual(40, score);

            seqA = new Sequence(Alphabets.DNA, "GGGA--CAAAATCAG---");
            seqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG---");

            score = MsaUtils.PairWiseScoreFunction(seqA, seqB, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            //Assert.AreEqual(56, score);

            // Test MultipleAlignmentScoreFunction
            List<ISequence> sequences = new List<ISequence>();
            seqA = new Sequence(Alphabets.DNA, "GGGA---AAAATCAGATT");
            seqB = new Sequence(Alphabets.DNA, "GGGA--CAAAATCAG---");
            sequences.Add(seqA);
            sequences.Add(seqB);
            score = MsaUtils.MultipleAlignmentScoreFunction(sequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty);
            Console.WriteLine("alignment score is: {0}", score);
            for (int i = 0; i < sequences.Count; ++i)
            {
                Console.WriteLine(sequences[i].ToString());
            }
            //Assert.AreEqual(42, score);

            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG---"));
            score = MsaUtils.MultipleAlignmentScoreFunction(sequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty);
            Console.WriteLine("alignment score is: {0}", score);
            for (int i = 0; i < sequences.Count; ++i)
            {
                Console.WriteLine(sequences[i].ToString());
            }
            //Assert.AreEqual(46, score);

            sequences[0] = new Sequence(Alphabets.DNA, "GGG---AAAAATCAGATT");
            score = MsaUtils.MultipleAlignmentScoreFunction(sequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty);
            for (int i = 0; i < sequences.Count; ++i)
            {
                Console.WriteLine(sequences[i].ToString());
            }
            Console.WriteLine("alignment score is: {0}", score);
            for (int i = 0; i < sequences.Count; ++i)
            {
                Console.WriteLine(sequences[i].ToString());
            }
            //Assert.AreEqual(40, score);

            // Test CalculateOffset
            seqA = new Sequence(Alphabets.DNA, "ABCD");
            seqB = new Sequence(Alphabets.DNA, "ABCD");

            List<int> offset = MsaUtils.CalculateOffset(seqA, seqB);
            Console.WriteLine("offsets are:");
            for (int i = 0; i < offset.Count; ++i)
            {
                Console.Write("{0}\t", offset[i]);
            }

            seqA = new Sequence(Alphabets.DNA, "A-BCD");
            seqB = new Sequence(Alphabets.DNA, "AB-CD");
            offset = MsaUtils.CalculateOffset(seqA, seqB);
            Console.WriteLine("\noffsets are:");
            for (int i = 0; i < offset.Count; ++i)
            {
                Console.Write("{0}\t", offset[i]);
            }

            seqA = new Sequence(Alphabets.DNA, "A-BCD");
            seqB = new Sequence(Alphabets.DNA, "----AB-CD");
            offset = MsaUtils.CalculateOffset(seqA, seqB);
            Console.WriteLine("\noffsets are:");
            for (int i = 0; i < offset.Count; ++i)
            {
                Console.Write("{0}\t", offset[i]);
            }

            sequences.Clear();
            sequences.Add(seqA);
            sequences.Add(new Sequence(Alphabets.DNA, "ABBCG"));

            List<ISequence> sequencesRef = new List<ISequence>();
            sequencesRef.Add(seqA);
            sequencesRef.Add(new Sequence(Alphabets.DNA, "ABBCG"));

            for (int i = 0; i < sequences.Count; ++i)
            {
                offset = MsaUtils.CalculateOffset(sequences[i], sequencesRef[i]);
                Console.WriteLine("\noffsets are:");
                for (int j = 0; j < offset.Count; ++j)
                {
                    Console.Write("{0}\t", offset[j]);
                }
            }

            Console.WriteLine("Q score is: {0}", MsaUtils.CalculateAlignmentScoreQ(sequences, sequencesRef));
            Console.WriteLine("TC score is: {0}", MsaUtils.CalculateAlignmentScoreTC(sequences, sequencesRef));



            // Test on one example
            sequences.Clear();
            sequences.Add(new Sequence(Alphabets.DNA, "GGGA---A-AAATCAGATT"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCA-AAATCAG---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCA-AAATCAG---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGA---A-A--TC-G---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCA-A--TCAG---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCTTA--TCAG---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGA--CA-AAATCAG---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGA---A-AAATCAGATT"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCA-AAATCAG---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGA--CA-AAATCAG---"));

            gapOpenPenalty = -4;
            gapExtendPenalty = -1;

            Console.WriteLine("score is: {0}", MsaUtils.MultipleAlignmentScoreFunction(sequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty));

            sequences.Clear();
            sequences.Add(new Sequence(Alphabets.DNA, "GGGA---AAAATCAGATT"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGA-----AATC-G---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATC--AATCAG---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCTTA-TCAG---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGA--CAAAATCAG---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGA---AAAATCAGATT"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG---"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGA--CAAAATCAG---"));

            Console.WriteLine("score is: {0}", MsaUtils.MultipleAlignmentScoreFunction(sequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty));

            // Test Quick Sort
            float[] a = new float[5] { 0, 2, 1, 5, 4 };
            int[] aIndex = new int[5] { 0, 1, 2, 3, 4 };
            MsaUtils.QuickSort(a, aIndex, 0, a.Length - 1);
            Console.WriteLine("quicksort");
            for (int i = 0; i < a.Length; ++i)
            {
                Console.WriteLine(a[i]);
            }
            for (int i = 0; i < a.Length; ++i)
            {
                Console.WriteLine(aIndex[i]);
            }

            Console.WriteLine("quicksortM");
            a = new float[5] { 0, 2, 1, 5, 4 };
            int[] aIndexB = null;
            MsaUtils.QuickSortM(a, out aIndexB, 0, 4);
            for (int i = 0; i < a.Length; ++i)
            {
                Console.WriteLine(a[i]);
            }
            for (int i = 0; i < a.Length; ++i)
            {
                Console.WriteLine(aIndexB[i]);
            }

            Console.WriteLine("quicksort");
            a = new float[5] { 0, 2, 1, 5, 4 };
            int[] aIndexC = MsaUtils.CreateIndexArray(a.Length);
            MsaUtils.QuickSort(a, aIndexC, 0, a.Length - 1);
            for (int i = 0; i < a.Length; ++i)
            {
                Console.WriteLine(aIndexC[i]);
            }

            a = new float[5] { 1, 0, 0, 0, 0 };
            aIndex = new int[5] { 0, 1, 2, 3, 4 };
            MsaUtils.QuickSort(a, aIndex, 0, a.Length - 1);
            for (int i = 0; i < a.Length; ++i)
            {
                Console.WriteLine(a[i]);
            }
            for (int i = 0; i < a.Length; ++i)
            {
                Console.WriteLine(aIndex[i]);
            }
        }
    }
}