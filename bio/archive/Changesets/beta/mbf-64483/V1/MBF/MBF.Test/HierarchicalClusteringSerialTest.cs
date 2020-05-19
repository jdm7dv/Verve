// -------------------------------------------------------------------------------------
// <copyright file="HierarchicalClusteringSerialTest.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Contains test cases for Hierarchical Clustering algorithm.
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
    /// Test for Hierarchical Clustering Serial Algorithm
    /// </summary>
    [TestFixture]
    public class HierarchicalClusteringSerialTest
    {
        /// <summary>
        /// Test Hierarcical clustering algorithm
        /// </summary>
        [Test]
        public void TestDistanceMatrix()
        {
            int dimension = 4;
            IDistanceMatrix distanceMatrix = new SymmetricDistanceMatrix(dimension);
            for (int i = 0; i < distanceMatrix.Dimension - 1; ++i)
            {
                for (int j = i + 1; j < distanceMatrix.Dimension; ++j)
                {
                    distanceMatrix[i, j] = i + j;
                    distanceMatrix[j, i] = i + j;
                }
            }

            IHierarchicalClustering hierarchicalClustering = new HierarchicalClusteringSerial(distanceMatrix);

            Assert.AreEqual(7, hierarchicalClustering.Nodes.Count);
            for (int i = 0; i < dimension * 2 - 1; ++i)
            {
                Assert.AreEqual(i, hierarchicalClustering.Nodes[i].ID);
            }

            for (int i = dimension; i < hierarchicalClustering.Nodes.Count; ++i)
            {
                Console.WriteLine(hierarchicalClustering.Nodes[i].LeftChildren.ID);
                Console.WriteLine(hierarchicalClustering.Nodes[i].RightChildren.ID);
            }

            //Assert.AreEqual(0, hierarchicalClustering.Nodes[4].LeftChildren.ID);
            //Assert.AreEqual(1, hierarchicalClustering.Nodes[4].RightChildren.ID);
            //Assert.AreEqual(2, hierarchicalClustering.Nodes[5].LeftChildren.ID);
            //Assert.AreEqual(4, hierarchicalClustering.Nodes[5].RightChildren.ID);
            //Assert.AreEqual(3, hierarchicalClustering.Nodes[6].LeftChildren.ID);
            //Assert.AreEqual(5, hierarchicalClustering.Nodes[6].RightChildren.ID);
        }
    }
}