// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using System.Linq;
using MBF.Algorithms.Assembly.PaDeNA;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.Tests
{
    /// <summary>
    /// Test for Step 4 in Parallel De Novo Assembly
    /// This step performs error correction on the input graph.
    /// It removes redundant paths in the graph.
    /// </summary>
    [TestClass]
    public class RedundantPathsPurgerTests : ParallelDeNovoAssembler
    {
        /// <summary>
        /// Test Step 4 - Redundant Paths Purger class
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestRedundantPathsPurger()
        {
            const int KmerLength = 5;
            const int RedundantThreshold = 10;

            List<ISequence> readSeqs = TestInputs.GetRedundantPathReads();
            this.SequenceReads.Clear();
            this.AddSequenceReads(readSeqs);
            this.KmerLength = KmerLength;
            this.RedundantPathLengthThreshold = RedundantThreshold;
            this.RedundantPathsPurger = new RedundantPathsPurger(RedundantThreshold);

            this.CreateGraph();
            int graphCount = this.Graph.Nodes.Count;
            int graphEdges = this.Graph.Nodes.Select(n => n.ExtensionsCount).Sum();

            this.RemoveRedundancy();
            int redundancyRemovedGraphCount = this.Graph.Nodes.Count;
            int redundancyRemovedGraphEdge = this.Graph.Nodes.Select(n => n.ExtensionsCount).Sum();

            // Compare the two graphs
            Assert.AreEqual(5, graphCount - redundancyRemovedGraphCount);
            Assert.AreEqual(12, graphEdges - redundancyRemovedGraphEdge);
        }
    }
}