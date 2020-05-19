// -------------------------------------------------------------------------------------
// <copyright file="ParallelDynamicProgrammingTest.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Contains test cases for ParallelDynamicProgramming class.
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
    using System.Threading.Tasks;
    using NUnit.Framework;
    using MBF.SimilarityMatrices;

    /// <summary>
    /// Test for ParallelDynamicProgramming class
    /// </summary>
    [TestFixture]
    public class ParallelDynamicProgrammingTest
    {
        /// <summary>
        /// Test ParallelDynamicProgramming class
        /// </summary>
        [Test]
        public void TestParallelDynamicProgramming()
        {
            ISequence templateSequence = new Sequence(Alphabets.DNA, "ATGCSWRYKMBVHDN-");
            Dictionary<ISequenceItem, int> itemSet = new Dictionary<ISequenceItem, int>();
            for (int i = 0; i < templateSequence.Count; ++i)
            {
                itemSet.Add(templateSequence[i], i);
            }
            Profiles.ItemSet = itemSet;

            NeedlemanWunschProfileAlignerSerial profileAligner = new NeedlemanWunschProfileAlignerSerial();
            SimilarityMatrix similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            int gapOpenPenalty = -8;
            int gapExtendPenalty = -1;

            profileAligner.SimilarityMatrix = similarityMatrix;
            profileAligner.GapOpenCost = gapOpenPenalty;
            profileAligner.GapExtensionCost = gapExtendPenalty;

            

            int numberOfRows = 8, numberOfCols = 7;
            int numberOfPartitions = 4;
            
            int startPosition = 1, endPosition = 100;            Dictionary<int, List<int[]>> parallelIndexMaster = profileAligner.ParallelIndexMasterGenerator(numberOfRows, numberOfCols, numberOfPartitions);
            foreach (var pair in parallelIndexMaster)
            {
                Console.Write("{0} ->: ", pair.Key);
                for (int i = 0; i < pair.Value.Count; ++i)
                {
                    Console.WriteLine("iteration: {0}: {1}-{2};", i, pair.Value[i][0], pair.Value[i][1]);
                }
            }

            for (int partitionIndex = 0; partitionIndex < numberOfPartitions; ++partitionIndex)
            {
                int[] indexPositions = profileAligner.IndexLocator(startPosition, endPosition, numberOfPartitions, partitionIndex);
                Console.Write("Index number: {0}: {1}-{2}", partitionIndex, indexPositions[0], indexPositions[1]);
            }

            int numberOfIterations = numberOfPartitions * 2 - 1;

            for (int i = 0; i < numberOfIterations; ++i)
            {
                foreach (var pair in parallelIndexMaster)
                {
                    List<int[]> indexPositions = parallelIndexMaster[pair.Key];

                    // Parallel in anti-diagonal direction
                    Parallel.ForEach(indexPositions, indexPosition =>
                    {
                        int[] rowPositions = profileAligner.IndexLocator(1, 100, numberOfPartitions, indexPosition[0]);
                        int[] colPositions = profileAligner.IndexLocator(1, 200, numberOfPartitions, indexPosition[0]);
                        Console.Write("row positions: {0}-{1}", rowPositions[0], rowPositions[1]);
                        Console.Write("col positions: {0}-{1}", colPositions[0], colPositions[1]);
                    });
                }
            }
        }
    }
}