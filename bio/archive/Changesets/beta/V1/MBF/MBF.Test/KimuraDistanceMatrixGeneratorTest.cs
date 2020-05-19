// -------------------------------------------------------------------------------------
// <copyright file="KimuraDistanceMatrixGeneratorTest.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Contains test cases for KimuraDistanceMatrixGenerator class and KimuraDistanceScoreCalculator class
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
    /// Test for KimuraDistanceMatrixGenerator Class and KimuraDistanceScoreCalculator class
    /// </summary>
    [TestFixture]
    public class KimuraDistanceMatrixGeneratorTest
    {
        /// <summary>
        /// Test KimuraDistanceMatrixGenerator Class and KimuraDistanceScoreCalculator class
        /// </summary>
        [Test]
        public void TestKimuraDistanceMatrixGenerator()
        {
            List<ISequence> sequences = new List<ISequence>();
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAA----ATC-G"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATC-AATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCTTATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGACAA-AATCAG"));

            float distanceScore;

            distanceScore = KimuraDistanceScoreCalculator.CalculatePercentIdentity(sequences[0], sequences[1]);
            Console.WriteLine("Kimura Distance of sequence 0, and 1 is: {0}", distanceScore);

            distanceScore = KimuraDistanceScoreCalculator.CalculatePercentIdentity(sequences[0], sequences[2]);
            Console.WriteLine("Kimura Distance of sequence 0, and 2 is: {0}", distanceScore);

            distanceScore = KimuraDistanceScoreCalculator.CalculatePercentIdentity(sequences[0], sequences[3]);
            Console.WriteLine("Kimura Distance of sequence 0, and 3 is: {0}", distanceScore);

            distanceScore = KimuraDistanceScoreCalculator.CalculatePercentIdentity(sequences[1], sequences[2]);
            Console.WriteLine("Kimura Distance of sequence 1, and 2 is: {0}", distanceScore);

            distanceScore = KimuraDistanceScoreCalculator.CalculatePercentIdentity(sequences[1], sequences[3]);
            Console.WriteLine("Kimura Distance of sequence 1, and 3 is: {0}", distanceScore);

            distanceScore = KimuraDistanceScoreCalculator.CalculatePercentIdentity(sequences[2], sequences[3]);
            Console.WriteLine("Kimura Distance of sequence 2, and 3 is: {0}", distanceScore);


            KimuraDistanceMatrixGenerator kimuraDistanceMatrixGenerator = new KimuraDistanceMatrixGenerator();

            kimuraDistanceMatrixGenerator.GenerateDistanceMatrix(sequences);

            for (int i = 0; i < sequences.Count - 1; ++i)
            {
                for (int j = i + 1; j < sequences.Count; ++j)
                {
                    distanceScore = KimuraDistanceScoreCalculator.CalculateDistanceScore(sequences[i], sequences[j]);
                    Console.WriteLine("Kimura Distance of sequence {0}, and {1} is: {2}", i, j, distanceScore);

                    //Assert.AreEqual(kimuraDistanceScoreCalculator.DistanceScore, kimuraDistanceMatrixGenerator.DistanceMatrix[i, j]);
                }
            }

        }
    }
}