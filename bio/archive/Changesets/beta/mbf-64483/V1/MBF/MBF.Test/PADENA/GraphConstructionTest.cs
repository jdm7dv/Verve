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
using MBF.Algorithms.Assembly.Graph;
using MBF.Algorithms.Kmer;
using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// Test for Step 2 in Parallel De Novo Assembly
    /// This step creates de bruijn graph from kmers.
    /// </summary>
    [TestFixture]
    public class GraphConstructionTest : ParallelDeNovoAssembler
    {
        /// <summary>
        /// Test Step 2 in De Novo algorithm - graph building
        /// </summary>
        [Test]
        public void TestDeBruijnGraphBuilderTiny()
        {
            const int KmerLength = 3;
            List<ISequence> reads = TestInputs.GetTinyReads();
            this.KmerLength = KmerLength;
            this.SequenceReads.Clear();
            this.AddSequenceReads(reads);

            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;

            Assert.AreEqual(9, graph.Nodes.Count);
            HashSet<string> nodeStrings = new HashSet<string>(graph.Nodes.Select(n => graph.GetNodeSequence(n).ToString()));
            Assert.IsTrue(nodeStrings.Contains("ATG") || nodeStrings.Contains("CAT"));
            Assert.IsTrue(nodeStrings.Contains("TGC") || nodeStrings.Contains("GCA"));
            Assert.IsTrue(nodeStrings.Contains("GCC") || nodeStrings.Contains("GGC"));
            Assert.IsTrue(nodeStrings.Contains("TCC") || nodeStrings.Contains("GGA"));
            Assert.IsTrue(nodeStrings.Contains("CCT") || nodeStrings.Contains("AGG"));
            Assert.IsTrue(nodeStrings.Contains("CTA") || nodeStrings.Contains("TAG"));
            Assert.IsTrue(nodeStrings.Contains("TAT") || nodeStrings.Contains("ATA"));
            Assert.IsTrue(nodeStrings.Contains("ATC") || nodeStrings.Contains("GAT"));
            Assert.IsTrue(nodeStrings.Contains("CTC") || nodeStrings.Contains("GAG"));
            int totalEdges = graph.Nodes.Select(n => n.ExtensionsCount).Sum();
            Assert.AreEqual(31, totalEdges);
        }

        /// <summary>
        /// Test Step 2 in De Novo algorithm - graph building
        /// </summary>
        [Test]
        public void TestDeBruijnGraphBuilderSmall()
        {
            const int KmerLength = 6;
            List<ISequence> reads = TestInputs.GetSmallReads();
            this.KmerLength = KmerLength;
            this.SequenceReads.Clear();
            this.AddSequenceReads(reads);

            this.CreateGraph();
            DeBruijnGraph graph = this.Graph;

            Assert.AreEqual(20, graph.Nodes.Count);
            HashSet<string> nodeStrings = GetGraphNodesForSmallReads();
            string nodeStr, nodeStrRC;
            foreach (DeBruijnNode node in graph.Nodes)
            {
                nodeStr = graph.GetNodeSequence(node).ToString();
                nodeStrRC = graph.GetNodeSequence(node).ReverseComplement.ToString();
                Assert.IsTrue(nodeStrings.Contains(nodeStr) || nodeStrings.Contains(nodeStrRC));
            }

            int totalEdges = graph.Nodes.Select(n => n.ExtensionsCount).Sum();
            Assert.AreEqual(51, totalEdges);
        }

        #region Expected Output
        /// <summary>
        /// Expected graph nodes for sequences in GetSmallReads()
        /// </summary>
        /// <returns>Expected graph nodes</returns>
        private HashSet<string> GetGraphNodesForSmallReads()
        {
            HashSet<string> nodes = new HashSet<string>();
            nodes.Add("GATGCC");
            nodes.Add("ATGCCT");
            nodes.Add("TGCCTC");
            nodes.Add("GCCTCC");
            nodes.Add("CCTCCT");
            nodes.Add("CTCCTA");
            nodes.Add("TCCTAT");
            nodes.Add("CCTATC");
            nodes.Add("CTATCG");
            nodes.Add("TATCGA");
            nodes.Add("ATCGAT");
            nodes.Add("TCGATC");
            nodes.Add("CGATCG");
            nodes.Add("GATCGT");
            nodes.Add("ATCGTC");
            nodes.Add("TCGTCG");
            nodes.Add("CGTCGA");
            nodes.Add("GTCGAT");
            nodes.Add("TCGATG");
            nodes.Add("CGATGC");
            return nodes;
        }
        #endregion
    }
}