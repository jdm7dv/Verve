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
using MBF.Algorithms.Assembly.Graph;
using MBF.Algorithms.Assembly.PaDeNA;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.Tests
{
    /// <summary>
    /// Test for contig graph building sub-step of 
    /// Step 6 in Parallel De Novo Assembly.
    /// This step converts kmer graph to contig graph
    /// </summary>
    [TestClass]
    public class ContigGraphBuilderTests : ParallelDeNovoAssembler
    {
        /// <summary>
        /// Test Contig Graph Builder Class
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestContigGraphBuilder1()
        {
            const int KmerLength = 11;
            const int DangleThreshold = 3;
            const int RedundantThreshold = 10;

            List<ISequence> readSeqs = TestInputs.GetDanglingReads();
            this.SequenceReads.Clear();
            this.AddSequenceReads(readSeqs);
            this.KmerLength = KmerLength;
            this.DanglingLinksThreshold = DangleThreshold;
            this.RedundantPathLengthThreshold = RedundantThreshold;
            this.DanglingLinksPurger = new DanglingLinksPurger(DangleThreshold);
            this.RedundantPathsPurger = new RedundantPathsPurger(RedundantThreshold);
            this.ContigBuilder = new SimplePathContigBuilder();

            this.CreateGraph();
            this.UnDangleGraph();
            this.RemoveRedundancy();
            IList<ISequence> contigs = this.BuildContigs();
            this.Graph.BuildContigGraph(contigs, KmerLength);

            int contigGraphCount = this.Graph.Nodes.Count;
            int contigGraphEdges = this.Graph.Nodes.Select(n => n.ExtensionsCount).Sum();

            Assert.AreEqual(contigs.Count, contigGraphCount);
            Assert.AreEqual(0, contigGraphEdges);
            HashSet<string> contigSeqs = new HashSet<string>(contigs.Select(c => c.ToString()));
            foreach (DeBruijnNode node in this.Graph.Nodes)
            {
                Assert.IsTrue(contigSeqs.Contains(this.Graph.GetNodeSequence(node).ToString()));
            }
        }

        /// <summary>
        /// Test Contig Graph Builder Class
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestContigGraphBuilder2()
        {
            const int KmerLength = 6;
            const int RedundantThreshold = 10;

            List<ISequence> readSeqs = TestInputs.GetRedundantPathReads();
            this.SequenceReads.Clear();
            this.AddSequenceReads(readSeqs);
            this.KmerLength = KmerLength;
            this.RedundantPathLengthThreshold = RedundantThreshold;
            this.RedundantPathsPurger = new RedundantPathsPurger(RedundantThreshold);
            this.ContigBuilder = new SimplePathContigBuilder();

            this.CreateGraph();
            this.RemoveRedundancy();
            IList<ISequence> contigs = this.BuildContigs();
            this.Graph.BuildContigGraph(contigs, KmerLength);

            int contigGraphCount = this.Graph.Nodes.Count;
            int contigGraphEdges = this.Graph.Nodes.Select(n => n.ExtensionsCount).Sum();

            Assert.AreEqual(contigs.Count, contigGraphCount);
            Assert.AreEqual(0, contigGraphEdges);
            HashSet<string> contigSeqs = new HashSet<string>(contigs.Select(c => c.ToString()));
            foreach (DeBruijnNode node in this.Graph.Nodes)
            {
                Assert.IsTrue(contigSeqs.Contains(this.Graph.GetNodeSequence(node).ToString()));
            }
        }
    }
}