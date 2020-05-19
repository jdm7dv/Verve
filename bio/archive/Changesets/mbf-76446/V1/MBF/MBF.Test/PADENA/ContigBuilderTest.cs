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
using NUnit.Framework;
using MBF.Algorithms.Assembly.Graph;

namespace MBF.Test
{
    /// <summary>
    /// Test for Step 5 in Parallel De Novo Assembly
    /// This step builds contigs from input graph.
    /// </summary>
    [TestFixture]
    public class ContigBuilderTest : ParallelDeNovoAssembler
    {
        /// <summary>
        /// Test Step 5 - Contig Builder Class
        /// </summary>
        [Test]
        public void TestContigBuilder1()
        {
            const int KmerLength = 11;
            const int DangleThreshold = 3;
            const int RedundantThreshold = 10;

            List<ISequence> readSeqs = TestInputs.GetDanglingReads();
            this.SequenceReads.Clear();
            this.AddSequenceReads(readSeqs);
            this.KmerLength = KmerLength;
            DanglingLinksThreshold = DangleThreshold;
            DanglingLinksPurger = new DanglingLinksPurger(DangleThreshold);
            RedundantPathLengthThreshold = RedundantThreshold;
            RedundantPathsPurger = new RedundantPathsPurger(RedundantThreshold);
            ContigBuilder = new SimplePathContigBuilder();

            CreateGraph();
            UnDangleGraph();
            RemoveRedundancy();
            int graphCount = Graph.Nodes.Count;
            int graphEdges = Graph.Nodes.Select(n => n.ExtensionsCount).Sum();

            IList<ISequence> contigs = BuildContigs();
            int contigsBuiltGraphCount = this.Graph.Nodes.Count;
            int contigsBuilt = Graph.Nodes.Select(n => n.ExtensionsCount).Sum();

            // Compare the two graphs
            Assert.AreEqual(1, contigs.Count);
            HashSet<string> expectedContigs = new HashSet<string>() 
            { 
                "ATCGCTAGCATCGAACGATCATT" 
            };

            foreach (ISequence contig in contigs)
            {
                Assert.IsTrue(expectedContigs.Contains(contig.ToString()));
            }

            Assert.AreEqual(graphCount, contigsBuiltGraphCount);
            Assert.AreEqual(graphEdges, contigsBuilt);
        }

        /// <summary>
        /// Test Step 5 - Contig Builder Class
        /// </summary>
        [Test]
        public void TestContigBuilder2()
        {
            const int KmerLength = 6;
            const int RedundantThreshold = 10;

            List<ISequence> readSeqs = TestInputs.GetRedundantPathReads();
            SequenceReads.Clear();
            this.AddSequenceReads(readSeqs);
            this.KmerLength = KmerLength;
            RedundantPathLengthThreshold = RedundantThreshold;
            RedundantPathsPurger = new RedundantPathsPurger(RedundantThreshold);
            ContigBuilder = new SimplePathContigBuilder();

            CreateGraph();
            RemoveRedundancy();
            int graphCount = Graph.Nodes.Count;
            int graphEdges = Graph.Nodes.Select(n => n.ExtensionsCount).Sum();

            IList<ISequence> contigs = BuildContigs();
            int contigsBuiltGraphCount = Graph.Nodes.Count;
            int contigsBuilt = Graph.Nodes.Select(n => n.ExtensionsCount).Sum();

            // Compare the two graphs
            Assert.AreEqual(1, contigs.Count);
            Assert.AreEqual("ATGCCTCCTATCTTAGCGATGCGGTGT", contigs[0].ToString());
            Assert.AreEqual(graphCount, contigsBuiltGraphCount);
            Assert.AreEqual(graphEdges, contigsBuilt);
        }
    }
}