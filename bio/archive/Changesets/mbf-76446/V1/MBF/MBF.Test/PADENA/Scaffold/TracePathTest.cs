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
using MBF.Algorithms.Assembly;
using MBF.Algorithms.Assembly.Graph;
using MBF.Algorithms.Assembly.PaDeNA;
using MBF.Algorithms.Assembly.PaDeNA.Scaffold;
using MBF.Util.Logging;
using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// Traverse Contig Overlap graph using Breadth First Search.
    /// </summary>
    [TestFixture]
    public class TracePathTest : ParallelDeNovoAssembler
    {
        static TracePathTest()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Traverse Graph with palindromic contig.
        /// </summary>
        [Test]
        public void TracePathTestWithPalindromicContig()
        {
            const int kmerLength = 6;
            const int dangleThreshold = 3;
            const int redundantThreshold = 7;
            List<ISequence> sequences = new List<ISequence>();
            Sequence seq = new Sequence(Alphabets.DNA, "ATGCCTC");
            seq.DisplayID = ">10.x1:abc";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "CCTCCTAT");
            seq.DisplayID = "1";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TCCTATC");
            seq.DisplayID = "2";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TGCCTCCT");
            seq.DisplayID = "3";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ATCTTAGC");
            seq.DisplayID = "4";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "CTATCTTAG");
            seq.DisplayID = "5";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "CTTAGCG");
            seq.DisplayID = "6";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "GCCTCCTAT");
            seq.DisplayID = ">8.x1:abc";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TAGCGCGCTA");
            seq.DisplayID = ">8.y1:abc";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "AGCGCGC");
            seq.DisplayID = ">9.x1:abc";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTTT");
            seq.DisplayID = "7";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTTAAA");
            seq.DisplayID = "8";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TAAAAA");
            seq.DisplayID = "9";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTAG");
            seq.DisplayID = "10";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTAGC");
            seq.DisplayID = "11";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "GCGCGCCGCGCG");
            seq.DisplayID = "12";
            sequences.Add(seq);

            KmerLength = kmerLength;
            SequenceReads.Clear();
            AddSequenceReads(sequences);
            CreateGraph();
            DanglingLinksThreshold = dangleThreshold;
            DanglingLinksPurger = new DanglingLinksPurger(dangleThreshold);
            RedundantPathLengthThreshold = redundantThreshold;
            RedundantPathsPurger = new RedundantPathsPurger(redundantThreshold);
            UnDangleGraph();
            RemoveRedundancy();

            IList<ISequence> contigs = BuildContigs();
            ReadContigMapper mapper = new ReadContigMapper();

            ReadContigMap maps = mapper.Map(contigs, sequences, kmerLength);
            MatePairMapper builder = new MatePairMapper();
            CloneLibrary.Instance.AddLibrary("abc", (float)5, (float)15);
            ContigMatePairs pairedReads = builder.MapContigToMatePairs(sequences, maps);

            ContigMatePairs overlap;
            OrientationBasedMatePairFilter filter = new OrientationBasedMatePairFilter();
            overlap = filter.FilterPairedReads(pairedReads, 0);
            DistanceCalculator dist = new DistanceCalculator();
            dist.CalculateDistance(overlap);
            Graph.BuildContigGraph(contigs, this.KmerLength);
            TracePath path = new TracePath();
            IList<ScaffoldPath> paths = path.FindPaths(Graph, overlap, kmerLength, 3);
            
            Assert.AreEqual(paths.Count, 3);
            Assert.AreEqual(paths.First().Count, 3);
            ScaffoldPath scaffold = paths.First();
            DeBruijnGraph graph = Graph;
            Assert.IsTrue(graph.GetNodeSequence(scaffold[0].Key).ToString().Equals("ATGCCTCCTATCTTAGC"));
            Assert.IsTrue(graph.GetNodeSequence(scaffold[1].Key).ToString().Equals("TTAGCGCG"));
            Assert.IsTrue(graph.GetNodeSequence(scaffold[2].Key).ToString().Equals("GCGCGC"));
        }
    }
}
