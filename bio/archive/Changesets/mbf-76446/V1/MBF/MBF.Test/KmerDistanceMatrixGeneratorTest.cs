// -------------------------------------------------------------------------------------
// <copyright file="KmerDistanceMatrixGeneratorTest.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Contains test cases for KmerDistanceMatrixGenerator class and KmerDistanceScoreCalculator class
// </summary>
// -------------------------------------------------------------------------------------

namespace Bio.Test
{
    using System;
    using System.Collections.Generic;
    using Bio.Algorithms;
    using Bio.Algorithms.Alignment;
    using Bio.Algorithms.Alignment.MultipleSequenceAlignment;
    using Bio.Util.Logging;
    using NUnit.Framework;

    /// <summary>
    /// Test for KmerDistanceMatrixGenerator Class and KmerDistanceScoreCalculator class
    /// </summary>
    [TestFixture]
    public class KmerDistanceMatrixGeneratorTest
    {
        /// <summary>
        /// Test KmerDistanceMatrixGenerator Class and KmerDistanceScoreCalculator class
        /// </summary>
        [Test]
        public void TestKimuraDistanceMatrixGenerator()
        {
            List<ISequence> sequences = new List<ISequence>();
            sequences.Add(new Sequence(Alphabets.DNA, "ACGTAA"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCAATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCTTATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGACAAAATCAG"));

            int kmerLength = 3;
            // test of kmer counting
            KmerDistanceScoreCalculator kmerDistanceScoreCalculator = new KmerDistanceScoreCalculator(kmerLength, MoleculeType.DNA);

            Dictionary<String, float> countDictionaryA = KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[0], kmerLength);
            Dictionary<String, float> countDictionaryB = KmerDistanceScoreCalculator.CalculateKmerCounting(sequences[1], kmerLength);

            Dictionary<String, float> expectedCountDictionaryA = new Dictionary<String, float>();
            expectedCountDictionaryA.Add("ACG", 1);
            expectedCountDictionaryA.Add("CGT", 1);
            expectedCountDictionaryA.Add("GTA", 1);
            expectedCountDictionaryA.Add("TAA", 1);

            Assert.AreEqual(countDictionaryA, expectedCountDictionaryA);


            foreach (var pair in countDictionaryA)
            {
                foreach (char s in pair.Key)
                {
                    Console.Write(s + " ");
                }
                Console.WriteLine(pair.Value);
            }
            foreach (var pair in countDictionaryB)
            {
                foreach (char s in pair.Key)
                {
                    Console.Write(s + " ");
                }
                Console.WriteLine(pair.Value);
            }

            float distanceScore = kmerDistanceScoreCalculator.CalculateDistanceScore(countDictionaryA, countDictionaryB);
            Console.WriteLine(distanceScore);

            KmerDistanceMatrixGenerator kmerDistanceMatrixGenerator = new KmerDistanceMatrixGenerator(sequences, kmerLength, MoleculeType.DNA);
            
            for (int i = 0; i < sequences.Count - 1; ++i)
            {
                for (int j = i + 1; j < sequences.Count; ++j)
                {
                    Console.WriteLine("Kmer Distance of sequence {0}, and {1} is: {2}", i, j, kmerDistanceMatrixGenerator.DistanceMatrix[i,j]);
                }
            }

        }
    }
}