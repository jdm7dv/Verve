// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Tests
{
    using System.Collections.Generic;
    using MBF.Algorithms.Alignment;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test cases for Cluster using mgaps.
    /// </summary>
    [TestClass]
    public class ClusterTests
    {
        /// <summary>
        /// Test Cluster with MUM set which has neither
        /// crosses nor overlaps
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestClusterWithoutCrossAndOverlap()
        {
            // Create a list of Mum classes.
            List<MaxUniqueMatch> matches = new List<MaxUniqueMatch>();
            MaxUniqueMatch match = null;

            match = new MaxUniqueMatch();
            match.FirstSequenceMumOrder = 1;
            match.FirstSequenceStart = 3;
            match.Length = 3;
            match.SecondSequenceMumOrder = 1;
            match.SecondSequenceStart = 1;
            matches.Add(match);

            match = new MaxUniqueMatch();
            match.FirstSequenceMumOrder = 2;
            match.FirstSequenceStart = 2;
            match.Length = 4;
            match.SecondSequenceMumOrder = 2;
            match.SecondSequenceStart = 2;
            matches.Add(match);

            match = new MaxUniqueMatch();
            match.FirstSequenceMumOrder = 3;
            match.FirstSequenceStart = 8;
            match.Length = 4;
            match.SecondSequenceMumOrder = 3;
            match.SecondSequenceStart = 5;
            matches.Add(match);

            match = new MaxUniqueMatch();
            match.FirstSequenceMumOrder = 4;
            match.FirstSequenceStart = 8;
            match.Length = 5;
            match.SecondSequenceMumOrder = 4;
            match.SecondSequenceStart = 6;
            matches.Add(match);

            IClusterBuilder clusterBuilder = new ClusterBuilder();
            clusterBuilder.MinimumScore = 2;
            clusterBuilder.FixedSeparation = 0;
            IList<Cluster> actualOutput = clusterBuilder.BuildClusters(matches);

            IList<Cluster> expectedOutput = new List<Cluster>();
            IList<MaxUniqueMatchExtension> clusterMatches = new List<MaxUniqueMatchExtension>();
            match = new MaxUniqueMatch();
            match.FirstSequenceMumOrder = 2;
            match.FirstSequenceStart = 2;
            match.Length = 4;
            match.SecondSequenceMumOrder = 2;
            match.SecondSequenceStart = 2;
            clusterMatches.Add(new MaxUniqueMatchExtension(match));
            expectedOutput.Add(new Cluster(clusterMatches));

            clusterMatches = new List<MaxUniqueMatchExtension>();
            match = new MaxUniqueMatch();
            match.FirstSequenceMumOrder = 1;
            match.FirstSequenceStart = 3;
            match.Length = 3;
            match.SecondSequenceMumOrder = 1;
            match.SecondSequenceStart = 1;
            clusterMatches.Add(new MaxUniqueMatchExtension(match));

            match = new MaxUniqueMatch();
            match.FirstSequenceMumOrder = 4;
            match.FirstSequenceStart = 8;
            match.Length = 5;
            match.SecondSequenceMumOrder = 4;
            match.SecondSequenceStart = 6;
            clusterMatches.Add(new MaxUniqueMatchExtension(match));
            expectedOutput.Add(new Cluster(clusterMatches));

            Assert.IsTrue(CompareMumList(actualOutput, expectedOutput));
        }

        /// <summary>
        /// Test Cluster with MUM set which has crosses.
        /// First MUM is bigger
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestClusterWithCross()
        {
            // Create a list of Mum classes.
            List<MaxUniqueMatch> matches = new List<MaxUniqueMatch>();
            MaxUniqueMatch match = null;

            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 0;
            match.Length = 4;
            match.SecondSequenceStart = 4;
            matches.Add(match);

            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 4;
            match.Length = 3;
            match.SecondSequenceStart = 0;
            matches.Add(match);

            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 10;
            match.Length = 3;
            match.SecondSequenceStart = 10;
            matches.Add(match);

            IClusterBuilder clusterBuilder = new ClusterBuilder();
            clusterBuilder.MinimumScore = 2;
            clusterBuilder.FixedSeparation = 0;
            IList<Cluster> actualOutput = clusterBuilder.BuildClusters(matches);

            IList<Cluster> expectedOutput = new List<Cluster>();
            IList<MaxUniqueMatchExtension> clusterMatches = new List<MaxUniqueMatchExtension>();
            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 0;
            match.Length = 4;
            match.SecondSequenceStart = 4;
            clusterMatches.Add(new MaxUniqueMatchExtension(match));
            expectedOutput.Add(new Cluster(clusterMatches));

            clusterMatches = new List<MaxUniqueMatchExtension>();
            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 4;
            match.Length = 3;
            match.SecondSequenceStart = 0;
            clusterMatches.Add(new MaxUniqueMatchExtension(match));
            expectedOutput.Add(new Cluster(clusterMatches));

            clusterMatches = new List<MaxUniqueMatchExtension>();
            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 10;
            match.Length = 3;
            match.SecondSequenceStart = 10;
            clusterMatches.Add(new MaxUniqueMatchExtension(match));
            expectedOutput.Add(new Cluster(clusterMatches));

            Assert.IsTrue(CompareMumList(actualOutput, expectedOutput));
        }

        /// <summary>
        /// Test Cluster with MUM set which has overlap
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestClusterWithCrossAndOverlap()
        {
            // Create a list of Mum classes.
            List<MaxUniqueMatch> matches = new List<MaxUniqueMatch>();
            MaxUniqueMatch match = null;

            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 0;
            match.Length = 5;
            match.SecondSequenceStart = 5;
            matches.Add(match);

            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 3;
            match.Length = 5;
            match.SecondSequenceStart = 0;
            matches.Add(match);

            IClusterBuilder clusterBuilder = new ClusterBuilder();
            clusterBuilder.MinimumScore = 2;
            clusterBuilder.FixedSeparation = 0;
            IList<Cluster> actualOutput = clusterBuilder.BuildClusters(matches);

            IList<Cluster> expectedOutput = new List<Cluster>();
            IList<MaxUniqueMatchExtension> clusterMatches = new List<MaxUniqueMatchExtension>();
            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 0;
            match.Length = 5;
            match.SecondSequenceStart = 5;
            clusterMatches.Add(new MaxUniqueMatchExtension(match));
            expectedOutput.Add(new Cluster(clusterMatches));

            clusterMatches = new List<MaxUniqueMatchExtension>();
            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 3;
            match.Length = 5;
            match.SecondSequenceStart = 0;
            clusterMatches.Add(new MaxUniqueMatchExtension(match));
            expectedOutput.Add(new Cluster(clusterMatches));

            Assert.IsTrue(CompareMumList(actualOutput, expectedOutput));
        }

        /// <summary>
        /// Test Cluster with MUM set which has overlap
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestClusterWithOverlap()
        {
            // Create a list of Mum classes.
            List<MaxUniqueMatch> matches = new List<MaxUniqueMatch>();
            MaxUniqueMatch match = null;

            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 0;
            match.Length = 4;
            match.SecondSequenceStart = 0;
            matches.Add(match);

            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 2;
            match.Length = 5;
            match.SecondSequenceStart = 4;
            matches.Add(match);

            IClusterBuilder clusterBuilder = new ClusterBuilder();
            clusterBuilder.MinimumScore = 2;
            clusterBuilder.FixedSeparation = 0;
            IList<Cluster> actualOutput = clusterBuilder.BuildClusters(matches);

            IList<Cluster> expectedOutput = new List<Cluster>();
            IList<MaxUniqueMatchExtension> clusterMatches = new List<MaxUniqueMatchExtension>();
            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 0;
            match.Length = 4;
            match.SecondSequenceStart = 0;
            clusterMatches.Add(new MaxUniqueMatchExtension(match));
            expectedOutput.Add(new Cluster(clusterMatches));

            clusterMatches = new List<MaxUniqueMatchExtension>();
            match = new MaxUniqueMatch();
            match.FirstSequenceStart = 2;
            match.Length = 5;
            match.SecondSequenceStart = 4;
            clusterMatches.Add(new MaxUniqueMatchExtension(match));
            expectedOutput.Add(new Cluster(clusterMatches));

            Assert.IsTrue(CompareMumList(actualOutput, expectedOutput));
        }

        /// <summary>
        /// Compares two list of Mum
        /// </summary>
        /// <param name="actualOutput">First list to be compared.</param>
        /// <param name="expectedOutput">Second list to be compared.</param>
        /// <returns>true if the MUMs are same.</returns>
        private static bool CompareMumList(
                IList<Cluster> actualOutput,
                IList<Cluster> expectedOutput)
        {
            if (actualOutput.Count == expectedOutput.Count)
            {
                bool correctOutput = true;

                for (int clusterCounter = 0; clusterCounter < expectedOutput.Count; clusterCounter++)
                {
                    Cluster actualCluster = actualOutput[clusterCounter];
                    Cluster expectedCluster = expectedOutput[clusterCounter];

                    if (actualCluster.Matches.Count == expectedCluster.Matches.Count)
                    {
                        for (int index = 0; index < expectedCluster.Matches.Count; index++)
                        {
                            if (actualCluster.Matches[index].FirstSequenceStart != expectedCluster.Matches[index].FirstSequenceStart)
                            {
                                correctOutput = false;
                                break;
                            }

                            if (actualCluster.Matches[index].Length != expectedCluster.Matches[index].Length)
                            {
                                correctOutput = false;
                                break;
                            }

                            if (actualCluster.Matches[index].SecondSequenceStart != expectedCluster.Matches[index].SecondSequenceStart)
                            {
                                correctOutput = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

                return correctOutput;
            }

            return false;
        }
    }
}