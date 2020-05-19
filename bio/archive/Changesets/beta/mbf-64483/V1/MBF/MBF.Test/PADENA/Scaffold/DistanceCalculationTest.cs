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
    /// Calculate Distance between Contigs using mate pairs.
    /// </summary>
    [TestFixture]
    public class DistanceCalculationTest : ParallelDeNovoAssembler
    {
        static DistanceCalculationTest()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Calculate Distance between contigs, both contig sequences are 
        /// in forward direction.
        /// </summary>
        [Test]
        public void DistanceCalculationwithTwoContigs()
        {
            const int kmerLength = 6;
            IList<ISequence> sequences = new List<ISequence>();
            Sequence seq = new Sequence(Alphabets.DNA, "GATCTGATAA");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "substrate 1 (IRS1) on chromosome 2.X1:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ATCTGATAAG");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "on chromosome 2.F:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TCTGATAAGG");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
            + "on chromosome 2.2:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTTGATGG");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "substrate 1 (IRS1) on chromosome 2.Y1:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTGATGGC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "on chromosome 2.R:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTGATGGCA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
            + "on chromosome 2.1:0.5K";
            sequences.Add(seq);

            
            IList<ISequence> contigs = new List<ISequence> { 
                new Sequence(Alphabets.DNA, "GATCTGATAAGG"), 
                new Sequence(Alphabets.DNA, "TTTTTGATGGCA") };
            ReadContigMapper mapper = new ReadContigMapper();
            ReadContigMap maps = mapper.Map(contigs, sequences, kmerLength);
            MatePairMapper mapPairedReads = new MatePairMapper();
            ContigMatePairs pairs = mapPairedReads.MapContigToMatePairs(sequences, maps);

            OrientationBasedMatePairFilter filter = new OrientationBasedMatePairFilter();
            ContigMatePairs contigpairedReads = filter.FilterPairedReads(pairs);
            DistanceCalculator calc = new DistanceCalculator();
            calc.CalculateDistance(contigpairedReads);
            Assert.AreEqual(contigpairedReads.Values.Count, 1);
            Assert.IsTrue(contigpairedReads.ContainsKey(contigs[0]));

            Dictionary<ISequence, IList<ValidMatePair>> map = contigpairedReads[contigs[0]];
            Assert.IsTrue(map.ContainsKey(contigs[1]));
            IList<ValidMatePair> valid = map[contigs[1]];

            Assert.AreEqual(valid.First().DistanceBetweenContigs[0], (float)478.000031);
            Assert.AreEqual(valid.First().DistanceBetweenContigs[1], (float)477.0);
            Assert.AreEqual(valid.First().StandardDeviation[0], (float)14.1421356);
            Assert.AreEqual(valid.First().StandardDeviation[1], (float)14.1421356);
            Assert.AreEqual(valid.First().Weight, 2);
        }

        /// <summary>
        /// Contig formed in forward direction and reverse complementary of contig, 
        /// but one paired read doesn't support orientation.
        /// </summary>
        [Test]
        public void DistanceCalculationWithTwoContigsReverseComplement()
        {
            const int kmerLength = 6;
            List<ISequence> sequences = new List<ISequence>();
            Sequence seq = new Sequence(Alphabets.DNA, "GATCTGATAA");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "substrate 1 (IRS1) on chromosome 2.X1:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ATCTGATAAG");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "on chromosome 2.F:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TCTGATAAGG");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
            + "on chromosome 2.2:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "CCATCAAAAA");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "substrate 1 (IRS1) on chromosome 2.Y1:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTGATGGC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "on chromosome 2.R:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTGATGGCA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
            + "on chromosome 2.1:0.5K";
            sequences.Add(seq);
            IList<ISequence> contigs = new List<ISequence> { 
                new Sequence(Alphabets.DNA, "GATCTGATAAGG"), 
                new Sequence(Alphabets.DNA, "TGCCATCAAAAA") };
            
            ReadContigMapper mapper = new ReadContigMapper();
            ReadContigMap maps = mapper.Map(contigs, sequences, kmerLength);
            MatePairMapper mapPairedReads = new MatePairMapper();
            ContigMatePairs pairedReads = mapPairedReads.MapContigToMatePairs(sequences, maps);
            OrientationBasedMatePairFilter filter = new OrientationBasedMatePairFilter();
            ContigMatePairs contigpairedReads = filter.FilterPairedReads(pairedReads);
            DistanceCalculator calc = new DistanceCalculator();
            calc.CalculateDistance(contigpairedReads);

            Assert.AreEqual(contigpairedReads.Values.Count, 1);
            Assert.IsTrue(contigpairedReads.ContainsKey(contigs[0]));
            Dictionary<ISequence, IList<ValidMatePair>> map = contigpairedReads[contigs[0]];
            Assert.IsTrue(map.ContainsKey(contigs[1]));

            IList<ValidMatePair> valid = map[contigs[1]];

            Assert.AreEqual(valid.First().DistanceBetweenContigs[1], (float)478.000031);
            Assert.AreEqual(valid.First().DistanceBetweenContigs[0], (float)477.0);
            Assert.AreEqual(valid.First().StandardDeviation[0], (float)14.1421356);
            Assert.AreEqual(valid.First().StandardDeviation[1], (float)14.1421356);
            Assert.AreEqual(valid.First().Weight, 2);
        }

        /// <summary>
        /// Calculate Distance between contigs, both contig sequences are 
        /// in forward direction using weighted mean.
        /// </summary>
        [Test]
        public void DistanceCalculationwithTwoContigsWeightedMean()
        {
            const int kmerLength = 6;
            List<ISequence> sequences = new List<ISequence>();
            Sequence seq = new Sequence(Alphabets.DNA, "GATCTGATAA");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "substrate 1 (IRS1) on chromosome 2.x1:2K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ATCTGATAAG");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "on chromosome 2.f:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TCTGATAAGG");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
            + "on chromosome 2.2:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTTGATGG");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "substrate 1 (IRS1) on chromosome 2.y1:2K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTGATGGC");
            seq.ID = ">gi|263191773|ref|NG_015830.1| Homo sapiens insulin receptor"
            + "on chromosome 2.r:0.5K";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTGATGGCA");
            seq.ID = ">gi|263191773|ref | Homo sapiens ........insulin receptor"
            + "on chromosome 2.1:0.5K";
            sequences.Add(seq);
            IList<ISequence> contigs = new List<ISequence> { 
                new Sequence(Alphabets.DNA, "GATCTGATAAGG"), 
                new Sequence(Alphabets.DNA, "TTTTTGATGGCA") };

            ReadContigMapper mapper = new ReadContigMapper();
            ReadContigMap maps = mapper.Map(contigs, sequences, kmerLength);

            MatePairMapper mapPairedReads = new MatePairMapper();
            ContigMatePairs pairedReads = mapPairedReads.MapContigToMatePairs(sequences, maps);
            OrientationBasedMatePairFilter filter = new OrientationBasedMatePairFilter();
            ContigMatePairs contigpairedReads = filter.FilterPairedReads(pairedReads);
            DistanceCalculator calc = new DistanceCalculator();
            calc.CalculateDistance(contigpairedReads);
            Assert.AreEqual(contigpairedReads.Values.Count, 1);
            Assert.IsTrue(contigpairedReads.ContainsKey(contigs[0]));

            Dictionary<ISequence, IList<ValidMatePair>> map = contigpairedReads[contigs[0]];
            Assert.IsTrue(map.ContainsKey(contigs[1]));
            IList<ValidMatePair> valid = map[contigs[1]];

            Assert.AreEqual(valid.First().DistanceBetweenContigs[0], (float)1228.0);
            Assert.AreEqual(valid.First().DistanceBetweenContigs[1], (float)1227.0);
            Assert.AreEqual(valid.First().StandardDeviation[0], (float)60);
            Assert.AreEqual(valid.First().StandardDeviation[1], (float)60);
            Assert.AreEqual(valid.First().Weight, 2);
        }

        /// <summary>
        /// Sort contigs.
        /// For consistent output due to parallel implementation.
        /// </summary>
        /// <param name="contigs">Read contig map</param>
        /// <returns>Sorted List of Pair</returns>
        private IList<ISequence> Sort(IList<ISequence> contigs)
        {
            return (from contig in contigs
                    orderby contig.ToString()
                    select contig).ToList();
        }
    }
}
