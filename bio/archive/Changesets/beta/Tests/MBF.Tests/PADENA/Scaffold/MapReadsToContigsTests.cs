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
using MBF.Algorithms.Assembly.PaDeNA.Scaffold;
using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.Tests
{
    /// <summary>
    /// Maps Reads to contigs
    /// </summary>
    [TestClass]
    public class MapReadsToContigsTests : ParallelDeNovoAssembler
    {
        static MapReadsToContigsTests()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.tests.log");
            }
        }

        /// <summary>
        /// Test for read contig alignment.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void MapReadToContig1()
        {
            IList<ISequence> contigs = new List<ISequence>();
            IList<ISequence> reads = new List<ISequence>();
            Sequence seq = new Sequence(Alphabets.DNA, "TCTGATAAGG");
            seq.DisplayID = "1";
            contigs.Add(seq);
            Sequence read = new Sequence(Alphabets.DNA, "CTGATAAGG");
            read.DisplayID = "2";
            reads.Add(read);
            const int kmerLength = 6;
            ReadContigMapper mapper = new ReadContigMapper();
            ReadContigMap map = mapper.Map(contigs, reads, kmerLength);
            Assert.AreEqual(map.Count, reads.Count);
            Dictionary<ISequence, IList<ReadMap>> alignment = map[reads[0].DisplayID];
            IList<ReadMap> readMap = alignment[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 9);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 1);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);
        }

        /// <summary>
        /// Test for read contig alignment.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void MapReadToContig2()
        {
            IList<ISequence> contigs = new List<ISequence>();
            IList<ISequence> reads = new List<ISequence>();
            Sequence seq = new Sequence(Alphabets.DNA, "TCTGATAAGG");
            seq.DisplayID = "1";
            contigs.Add(seq);
            Sequence read = new Sequence(Alphabets.DNA, "CCTTATCAG");
            read.DisplayID = "2";
            reads.Add(read);
            const int kmerLength = 6;
            ReadContigMapper mapper = new ReadContigMapper();
            ReadContigMap map = mapper.Map(contigs, reads, kmerLength);
            Assert.AreEqual(map.Count, reads.Count);
            Dictionary<ISequence, IList<ReadMap>> alignment = map[reads[0].DisplayID];
            IList<ReadMap> readMap = alignment[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 9);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 1);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);
        }

        /// <summary>
        /// Test for Contig Read mapping using single contig.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void MapReadsWithSingleContigRightTraversal()
        {
            const int kmerLength = 6;

            IList<ISequence> readSeqs = new List<ISequence>();
            Sequence read = new Sequence(Alphabets.DNA, "GATGCCTC");
            read.DisplayID = "0";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "CCTCCTAT");
            read.DisplayID = "1";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "TCCTATC");
            read.DisplayID = "2";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "GCCTCCTAT");
            read.DisplayID = "3";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "TGCCTCCT");
            read.DisplayID = "4";
            readSeqs.Add(read);

            IList<ISequence> contigs = new List<ISequence> { new Sequence(Alphabets.DNA, "GATGCCTCCTATC") };

            ReadContigMapper mapper = new ReadContigMapper();
            ReadContigMap maps = mapper.Map(contigs, readSeqs, kmerLength);

            Assert.AreEqual(maps.Count, readSeqs.Count);
            Dictionary<ISequence, IList<ReadMap>> map = maps[readSeqs[0].DisplayID];

            IList<ReadMap> readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 8);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 0);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[1].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 8);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 4);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[2].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 7);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 6);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[3].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 9);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 3);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[4].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 8);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 2);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);
        }

        /// <summary>
        /// Test for Contig Read mapping using single contig generated by 
        /// left traversal of graph.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void MapReadsWithSingleContigLeftTraversal()
        {
            const int kmerLength = 6;
            IList<ISequence> readSeqs = new List<ISequence>();
            Sequence read = new Sequence(Alphabets.DNA, "ATGCCTC");
            read.DisplayID = "0";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "CCTCCTAT");
            read.DisplayID = "1";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "TCCTATC");
            read.DisplayID = "2";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "TGCCTCCT");
            read.DisplayID = "3";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "ATCTTAGC");
            read.DisplayID = "4";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "CTATCTTAG");
            read.DisplayID = "5";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "CTTAGCG");
            read.DisplayID = "6";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "GCCTCCTAT");
            read.DisplayID = "7";
            readSeqs.Add(read);

            IList<ISequence> contigs = new List<ISequence> { new Sequence(Alphabets.DNA, "ATGCCTCCTATCTTAGCG") };
            ReadContigMapper mapper = new ReadContigMapper();
            ReadContigMap maps = mapper.Map(contigs, readSeqs, kmerLength);
            Assert.AreEqual(maps.Count, readSeqs.Count);

            Dictionary<ISequence, IList<ReadMap>> map = maps[readSeqs[0].DisplayID];

            IList<ReadMap> readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 7);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 0);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[1].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 8);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 3);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[2].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 7);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 5);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[3].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 8);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 1);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[4].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 8);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 9);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[5].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].StartPositionOfContig, 7);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[6].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 7);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 11);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[7].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 9);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 2);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);
        }

        /// <summary>
        /// Test for Contig Read mapping having two contigs generated in right traversal of graph
        /// and having partial overlap of reads.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void MapReadsWithTwoContigRightTraversal()
        {
            const int kmerLength = 6;
            IList<ISequence> readSeqs = new List<ISequence>();
            Sequence read = new Sequence(Alphabets.DNA, "GATCTGATAA");
            read.DisplayID = "0";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "ATCTGATAAG");
            read.DisplayID = "1";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "TCTGATAAGG");
            read.DisplayID = "2";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "TTTTTGATGG");
            read.DisplayID = "3";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "TTTTGATGGC");
            read.DisplayID = "4";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "TTTGATGGCA");
            read.DisplayID = "5";
            readSeqs.Add(read);

            IList<ISequence> contigs = new List<ISequence> { new Sequence(Alphabets.DNA, "GATCTGATAAGG"), new Sequence(Alphabets.DNA, "TTTTTGATGGCA") };

            ReadContigMapper mapper = new ReadContigMapper();
            ReadContigMap maps = mapper.Map(contigs, readSeqs, kmerLength);
            Assert.AreEqual(maps.Count, readSeqs.Count);

            Dictionary<ISequence, IList<ReadMap>> map = maps[readSeqs[0].DisplayID];

            IList<ReadMap> readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 10);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 0);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[1].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 10);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 1);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[2].DisplayID];
            readMap = map[contigs[0]];
            Assert.AreEqual(readMap[0].Length, 10);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 2);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[3].DisplayID];
            readMap = map[contigs[1]];
            Assert.AreEqual(readMap[0].Length, 10);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 0);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[4].DisplayID];
            readMap = map[contigs[1]];
            Assert.AreEqual(readMap[0].Length, 10);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 1);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);

            map = maps[readSeqs[5].DisplayID];
            readMap = map[contigs[1]];
            Assert.AreEqual(readMap[0].Length, 10);
            Assert.AreEqual(readMap[0].StartPositionOfContig, 2);
            Assert.AreEqual(readMap[0].StartPositionOfRead, 0);
            Assert.AreEqual(readMap[0].ReadOverlap, ContigReadOverlapType.FullOverlap);
        }
    }
}
