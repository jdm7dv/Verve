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
    /// Test for Step 3 in Parallel De Novo Assembly
    /// This step performs error correction on the input graph.
    /// It removes dangling links in the graph.
    /// </summary>
    [TestClass]
    public class DanglingLinksPurgerTests : ParallelDeNovoAssembler
    {
        /// <summary>
        /// Test Step 3 - Dangling Link Purger class
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestDanglingLinksPurger()
        {
            const int KmerLength = 11;
            const int DangleThreshold = 3;
            List<ISequence> readSeqs = TestInputs.GetDanglingReads();
            SequenceReads.Clear();
            this.AddSequenceReads(readSeqs);
            this.KmerLength = KmerLength;
            DanglingLinksThreshold = DangleThreshold;
            DanglingLinksPurger = new DanglingLinksPurger(DangleThreshold);

            CreateGraph();
            int graphCount = Graph.Nodes.Count;
            int graphEdges = Graph.Nodes.Select(n => n.ExtensionsCount).Sum();
            HashSet<string> graphNodes = new HashSet<string>(
            Graph.Nodes.Select(n => Graph.GetNodeSequence(n).ToString()));

            DanglingLinksThreshold = DangleThreshold;
            UnDangleGraph();
            int dangleRemovedGraphCount = Graph.Nodes.Count;
            int dangleRemovedGraphEdge = Graph.Nodes.Select(n => n.ExtensionsCount).Sum();
            HashSet<string> dangleRemovedGraphNodes = new HashSet<string>(
            Graph.Nodes.Select(n => Graph.GetNodeSequence(n).ToString()));

            // Compare the two graphs
            Assert.AreEqual(2, graphCount - dangleRemovedGraphCount);
            Assert.AreEqual(4, graphEdges - dangleRemovedGraphEdge);
            graphNodes.ExceptWith(dangleRemovedGraphNodes);
            Assert.IsTrue(graphNodes.Contains("TCGAACGATGA"));
            Assert.IsTrue(graphNodes.Contains("ATCGAACGATG"));
        }
    }
}