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
using MBF.Util.Logging;

using MBF.Algorithms.Assembly.PaDeNA.Scaffold;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.Tests
{
    /// <summary>
    /// Map Reads to Mate Pairs
    /// </summary>
    [TestClass]
    public class MatePairMapperTests
    {
        static MatePairMapperTests()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("MBF.Tests.log");
            }
        }

        /// <summary>
        /// For X1 and Y1 format of mate pairs
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void MatePairMapper1()
        {
            IList<ISequence> sequences = new List<ISequence>();
            IList<MatePair> pairedreads = new List<MatePair>();
            Sequence seq = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "substrate 1 (IRS1) on chromosome 2.X1:2K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "on chromosome 2.X1:10K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
            + "on chromosome 2.X1:50K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "substrate 1 (IRS1) on chromosome 2.Y1:2K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "on chromosome 2.Y1:10K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
            + "on chromosome 2.Y1:50K";
            sequences.Add(seq);

            MatePairMapper pair = new MatePairMapper();
            pairedreads = Sort(pair.Map(sequences), sequences);
            Assert.AreEqual(pairedreads.Count, 3);

            ISequence expectedForward = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            ISequence expectedReverse = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            string lib = "50K";
            Assert.AreEqual(pairedreads[0].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[0].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[0].Library, lib);
            Assert.AreEqual(pairedreads[0].MeanLengthOfLibrary, 65000);
            Assert.AreEqual(pairedreads[0].StandardDeviationOfLibrary, 13334);

            expectedForward = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            expectedReverse = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            lib = "10K";
            Assert.AreEqual(pairedreads[1].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[1].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[1].Library, lib);
            Assert.AreEqual(pairedreads[1].MeanLengthOfLibrary, 10000);
            Assert.AreEqual(pairedreads[1].StandardDeviationOfLibrary, 1000);

            expectedForward = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            expectedReverse = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            lib = "2K";
            Assert.AreEqual(pairedreads[2].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[2].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[2].Library, lib);
            Assert.AreEqual(pairedreads[2].MeanLengthOfLibrary, 2000);
            Assert.AreEqual(pairedreads[2].StandardDeviationOfLibrary, 100);
        }

        /// <summary>
        /// For F and R format of mate pairs
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void PairedReadMapper2()
        {

            IList<ISequence> sequences = new List<ISequence>();
            IList<MatePair> pairedreads = new List<MatePair>();
            Sequence seq;
            seq = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "substrate 1 (IRS1) on chromosome 2.F:2K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "on chromosome 2.F:10K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
                + "on chromosome 2.F:50K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "substrate 1 (IRS1) on chromosome 2.R:2K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "on chromosome 2.R:10K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
                + "on chromosome 2.R:50K";
            sequences.Add(seq);
            MatePairMapper pair = new MatePairMapper();
            pairedreads = Sort(pair.Map(sequences), sequences);

            Assert.AreEqual(pairedreads.Count, 3);

            ISequence expectedForward = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            ISequence expectedReverse = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            string lib = "50K";
            Assert.AreEqual(pairedreads[0].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[0].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[0].Library, lib);
            Assert.AreEqual(pairedreads[0].MeanLengthOfLibrary, 65000);
            Assert.AreEqual(pairedreads[0].StandardDeviationOfLibrary, 13334);

            expectedForward = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            expectedReverse = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            lib = "10K";
            Assert.AreEqual(pairedreads[1].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[1].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[1].Library, lib);
            Assert.AreEqual(pairedreads[1].MeanLengthOfLibrary, 10000);
            Assert.AreEqual(pairedreads[1].StandardDeviationOfLibrary, 1000);

            expectedForward = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            expectedReverse = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            lib = "2K";
            Assert.AreEqual(pairedreads[2].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[2].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[2].Library, lib);
            Assert.AreEqual(pairedreads[2].MeanLengthOfLibrary, 2000);
            Assert.AreEqual(pairedreads[2].StandardDeviationOfLibrary, 100);
        }

        /// <summary>
        /// For 1 and 2 format of mate pairs
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void PairedReadMapper3()
        {
            IList<ISequence> sequences = new List<ISequence>();
            IList<MatePair> pairedreads = new List<MatePair>();
            Sequence seq;
            seq = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "substrate 1 (IRS1) on chromosome 2.1:2K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "on chromosome 2.1:10K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
                + "on chromosome 2.1:50K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "substrate 1 (IRS1) on chromosome 2.2:2K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "on chromosome 2.2:10K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
                + "on chromosome 2.2:50K";
            sequences.Add(seq);
            MatePairMapper pair = new MatePairMapper();
            pairedreads = Sort(pair.Map(sequences), sequences);

            Assert.AreEqual(pairedreads.Count, 3);

            ISequence expectedForward = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            ISequence expectedReverse = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            string lib = "50K";
            Assert.AreEqual(pairedreads[0].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[0].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[0].Library, lib);
            Assert.AreEqual(pairedreads[0].MeanLengthOfLibrary, 65000);
            Assert.AreEqual(pairedreads[0].StandardDeviationOfLibrary, 13334);

            expectedForward = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            expectedReverse = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            lib = "10K";
            Assert.AreEqual(pairedreads[1].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[1].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[1].Library, lib);
            Assert.AreEqual(pairedreads[1].MeanLengthOfLibrary, 10000);
            Assert.AreEqual(pairedreads[1].StandardDeviationOfLibrary, 1000);

            expectedForward = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            expectedReverse = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            lib = "2K";
            Assert.AreEqual(pairedreads[2].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[2].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[2].Library, lib);
            Assert.AreEqual(pairedreads[2].MeanLengthOfLibrary, 2000);
            Assert.AreEqual(pairedreads[2].StandardDeviationOfLibrary, 100);
        }

        /// <summary>
        /// Mixed format of mate pairs
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void PairedReadMapper4()
        {
            IList<ISequence> sequences = new List<ISequence>();
            IList<MatePair> pairedreads = new List<MatePair>();
            Sequence seq;
            seq = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "substrate 1 (IRS1) on chromosome 2.F:2K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "on chromosome 2.1:10K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
                + "on chromosome 2.X1:50K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "substrate 1 (IRS1) on chromosome 2.R:2K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "on chromosome 2.2:10K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
                + "on chromosome 2.Y1:50K";
            sequences.Add(seq);
            MatePairMapper pair = new MatePairMapper();
            pairedreads = Sort(pair.Map(sequences), sequences);

            Assert.AreEqual(pairedreads.Count, 3);

            ISequence expectedForward = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            ISequence expectedReverse = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            string lib = "50K";
            Assert.AreEqual(pairedreads[0].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[0].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[0].Library, lib);
            Assert.AreEqual(pairedreads[0].MeanLengthOfLibrary, 65000);
            Assert.AreEqual(pairedreads[0].StandardDeviationOfLibrary, 13334);

            expectedForward = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            expectedReverse = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            lib = "10K";
            Assert.AreEqual(pairedreads[1].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[1].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[1].Library, lib);
            Assert.AreEqual(pairedreads[1].MeanLengthOfLibrary, 10000);
            Assert.AreEqual(pairedreads[1].StandardDeviationOfLibrary, 1000);

            expectedForward = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            expectedReverse = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            lib = "2K";
            Assert.AreEqual(pairedreads[2].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[2].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[2].Library, lib);
            Assert.AreEqual(pairedreads[2].MeanLengthOfLibrary, 2000);
            Assert.AreEqual(pairedreads[2].StandardDeviationOfLibrary, 100);
        }

        /// <summary>
        /// Unpaired Read in mixed mate pair format. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void PairedReadMapper5()
        {
            IList<ISequence> sequences = new List<ISequence>();
            IList<MatePair> pairedreads = new List<MatePair>();
            Sequence seq;
            seq = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "substrate 1 (IRS1) on chromosome 2.X1:2K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "on chromosome 2.F:10K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
                + "on chromosome 2.1:50K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "substrate 1 (IRS1) on chromosome 2.Y1:2K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
                + "on chromosome 2.R:10K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
                + "on chromosome 2.2:50K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            seq.ID = ">gi|263191773|Homo sapiens ........insulin receptor"
            + "on chromosome 2.F:10K";
            sequences.Add(seq);
            MatePairMapper pair = new MatePairMapper();
            pairedreads = Sort(pair.Map(sequences), sequences);

            Assert.AreEqual(pairedreads.Count, 3);

            ISequence expectedForward = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            ISequence expectedReverse = new Sequence(Alphabets.DNA, "ACGTGTGKAAAAAAA");
            string lib = "50K";
            Assert.AreEqual(pairedreads[0].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[0].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[0].Library, lib);
            Assert.AreEqual(pairedreads[0].MeanLengthOfLibrary, 65000);
            Assert.AreEqual(pairedreads[0].StandardDeviationOfLibrary, 13334);

            expectedForward = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            expectedReverse = new Sequence(Alphabets.DNA, "ACGTGTGTGTCCCCC");
            lib = "10K";
            Assert.AreEqual(pairedreads[1].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[1].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[1].Library, lib);
            Assert.AreEqual(pairedreads[1].MeanLengthOfLibrary, 10000);
            Assert.AreEqual(pairedreads[1].StandardDeviationOfLibrary, 1000);

            expectedForward = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            expectedReverse = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            lib = "2K";
            Assert.AreEqual(pairedreads[2].GetForwardRead(sequences).ToString(), expectedForward.ToString());
            Assert.AreEqual(pairedreads[2].GetReverseRead(sequences).ToString(), expectedReverse.ToString());
            Assert.AreEqual(pairedreads[2].Library, lib);
            Assert.AreEqual(pairedreads[2].MeanLengthOfLibrary, 2000);
            Assert.AreEqual(pairedreads[2].StandardDeviationOfLibrary, 100);
        }

        /// <summary>
        /// Contig paired read map.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ContigPairReadMap()
        {
            const int kmerLength = 6;
            IList<ISequence> readSeqs = new List<ISequence>();
            Sequence read = new Sequence(Alphabets.DNA, "GATCTGATAA");
            read.DisplayID = "0.x1:abc";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "ATCTGATAAG");
            read.DisplayID = "1.F:abc";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "TCTGATAAGG");
            read.DisplayID = "2.2:abc";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "TTTTTGATGG");
            read.DisplayID = "0.y1:abc";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "TTTTGATGGC");
            read.DisplayID = "1.R:abc";
            readSeqs.Add(read);
            read = new Sequence(Alphabets.DNA, "TTTGATGGCA");
            read.DisplayID = "2.1:abc";
            readSeqs.Add(read);

            IList<ISequence> contigs = new List<ISequence> { new Sequence(Alphabets.DNA, "GATCTGATAAGG"), 
                new Sequence(Alphabets.DNA, "TTTTTGATGGCA")};
            ReadContigMapper mapper = new ReadContigMapper();
            ReadContigMap maps = mapper.Map(contigs, readSeqs, kmerLength);
            MatePairMapper pair = new MatePairMapper();
            ContigMatePairs map = pair.MapContigToMatePairs(readSeqs, maps);
            Assert.AreEqual(map.Count, 2);
            Dictionary<ISequence, IList<ValidMatePair>> reverseContigs;
            Assert.IsTrue(map.TryGetValue(contigs[0], out reverseContigs));
            Assert.AreEqual(reverseContigs.Count, 1);
            IList<ValidMatePair> matePairs;
            Assert.IsTrue(reverseContigs.TryGetValue(contigs[1], out matePairs));
            Assert.AreEqual(matePairs.Count, 2);
            Assert.AreEqual(matePairs[0].ForwardReadStartPosition.First(), 0);
            Assert.AreEqual(matePairs[0].ReverseReadStartPosition.First(), 9);
            Assert.AreEqual(matePairs[1].ForwardReadStartPosition.First(), 1);
            Assert.AreEqual(matePairs[1].ReverseReadStartPosition.First(), 10);

            Assert.IsTrue(map.TryGetValue(contigs[1], out reverseContigs));
            Assert.AreEqual(reverseContigs.Count, 1);
            Assert.IsTrue(reverseContigs.TryGetValue(contigs[0], out matePairs));
            Assert.AreEqual(matePairs.Count, 1);
            Assert.AreEqual(matePairs[0].ForwardReadStartPosition.First(), 2);
            Assert.AreEqual(matePairs[0].ReverseReadStartPosition.First(), 11);
        }

        /// <summary>
        /// Sort mate pairs based on forward reads.
        /// For consistent output due to parallel implementation.
        /// </summary>
        /// <param name="pairedReads">List of mate pairs</param>
        /// <param name="reads">Forward reads.</param>
        /// <returns>Sorted List of mate pairs</returns>
        private static IList<MatePair> Sort(IList<MatePair> pairedReads, IList<ISequence> reads)
        {
            return (from pairedRead in pairedReads
                    orderby pairedRead.GetForwardRead(reads).ToString()
                    select pairedRead).ToList();
        }
    }
}
