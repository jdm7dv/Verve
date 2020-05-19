using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.IO.SAM;
using MBF.IO.BAM;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;
using MBF.Matrix;
using System.Globalization;

namespace MBF.Tests
{
    /// <summary>
    /// Test the paired read.
    /// </summary>
    [TestClass]
    public class PairedReadTest
    {
        /// <summary>
        /// Test GetPairedReads.
        /// </summary>
        [TestMethod]
        public void TestGettingPairedReads()
        {
            string bamfilePath = @"TestUtils\BAM\SeqAlignment.bam";
            BAMParser parser = null;
            try
            {
                parser = new BAMParser();
                SequenceAlignmentMap alignmentMap = parser.Parse(bamfilePath);
                Assert.IsTrue(alignmentMap != null);
                IList<PairedRead> pairedReads = alignmentMap.GetPairedReads();
                Assert.IsTrue(pairedReads.Count > 0);

                pairedReads = alignmentMap.GetPairedReads(250, 50);
                Assert.IsTrue(pairedReads.Count > 0);
            }
            finally
            {
                if (parser != null)
                    parser.Dispose();
            }
        }

        /// <summary>
        /// Test the paired read type from GetPairedReads() using a sam file
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"), TestMethod]
        public void TestALLtypePairedReadsInSAMFile()
        {
            string samfilePath = @"TestUtils\SAM\PairedReadsTest.sam";
            using (SAMParser parser = new SAMParser())
            {
                SequenceAlignmentMap map = parser.Parse(samfilePath);

                int TotalReadsCount = 77;
                int unpairedReadsCount = 2;
                int multipleHitsPairCount = 2;
                int multipleHitReadsCount = 8;
                int normalPairCount = 4;
                int normalreadsCount = 2 * normalPairCount;
                int orphanPairsCount = 3;
                int orphanreadsCount = 5;
                int chimerapaircount = 15;
                int chimerareadsCount = 2 * chimerapaircount;
                int strucAnomPairCount = 3;
                int strucAnomReadsCount = 2 * strucAnomPairCount;
                int lenAnomPairCount = 9;
                int LenAnomReadsCount = 2 * lenAnomPairCount;

                int total = unpairedReadsCount + multipleHitReadsCount + normalreadsCount + orphanreadsCount + chimerareadsCount + strucAnomReadsCount + LenAnomReadsCount;
                Assert.AreEqual(TotalReadsCount, total);

                IList<PairedRead> reads = map.GetPairedReads(200, 50);
                IList<PairedRead> multipleHits = reads.Where(PE => PE.PairedType == PairedReadType.MultipleHits).ToList();
                IList<PairedRead> normal = reads.Where(PE => PE.PairedType == PairedReadType.Normal).ToList();
                IList<PairedRead> orphan = reads.Where(PE => PE.PairedType == PairedReadType.Orphan).ToList();
                IList<PairedRead> chimera = reads.Where(PE => PE.PairedType == PairedReadType.Chimera).ToList();
                IList<PairedRead> strucAnom = reads.Where(PE => PE.PairedType == PairedReadType.StructuralAnomaly).ToList();
                IList<PairedRead> lenAnom = reads.Where(PE => PE.PairedType == PairedReadType.LengthAnomaly).ToList();

                Assert.AreEqual(TotalReadsCount, map.QuerySequences.Count);

                Assert.AreEqual(multipleHitsPairCount, multipleHits.Count());
                Assert.AreEqual(multipleHitReadsCount, multipleHits.Sum(PE => PE.Reads.Count));

                Assert.AreEqual(normalPairCount, normal.Count());
                Assert.AreEqual(normalreadsCount, normal.Sum(PE => PE.Reads.Count));

                Assert.AreEqual(orphanPairsCount, orphan.Count());
                Assert.AreEqual(orphanreadsCount, orphan.Sum(PE => PE.Reads.Count));

                Assert.AreEqual(chimerapaircount, chimera.Count());
                Assert.AreEqual(chimerareadsCount, chimera.Sum(PE => PE.Reads.Count));

                Assert.AreEqual(strucAnomPairCount, strucAnom.Count());
                Assert.AreEqual(strucAnomReadsCount, strucAnom.Sum(PE => PE.Reads.Count));

                Assert.AreEqual(lenAnomPairCount, lenAnom.Count());
                Assert.AreEqual(LenAnomReadsCount, lenAnom.Sum(PE => PE.Reads.Count));
            }
        }

        /// <summary>
        /// Test the paired read type from GetPairedReads() using a bam file
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity"), TestMethod]
        public void TestALLtypePairedReadsInBAMFile()
        {
            string bamfilePath = @"TestUtils\BAM\PairedReadsTest.bam";
            using (BAMParser parser = new BAMParser())
            {
                SequenceAlignmentMap map = parser.Parse(bamfilePath);

                int TotalReadsCount = 77;
                int unpairedReadsCount = 2;
                int multipleHitsPairCount = 2;
                int multipleHitReadsCount = 8;
                int normalPairCount = 4;
                int normalreadsCount = 2 * normalPairCount;
                int orphanPairsCount = 3;
                int orphanreadsCount = 5;
                int chimerapaircount = 15;
                int chimerareadsCount = 2 * chimerapaircount;
                int strucAnomPairCount = 3;
                int strucAnomReadsCount = 2 * strucAnomPairCount;
                int lenAnomPairCount = 9;
                int LenAnomReadsCount = 2 * lenAnomPairCount;

                int total = unpairedReadsCount + multipleHitReadsCount + normalreadsCount + orphanreadsCount + chimerareadsCount + strucAnomReadsCount + LenAnomReadsCount;
                Assert.AreEqual(TotalReadsCount, total);

                IList<PairedRead> reads = map.GetPairedReads(200, 50);
                IList<PairedRead> multipleHits = reads.Where(PE => PE.PairedType == PairedReadType.MultipleHits).ToList();
                IList<PairedRead> normal = reads.Where(PE => PE.PairedType == PairedReadType.Normal).ToList();
                IList<PairedRead> orphan = reads.Where(PE => PE.PairedType == PairedReadType.Orphan).ToList();
                IList<PairedRead> chimera = reads.Where(PE => PE.PairedType == PairedReadType.Chimera).ToList();
                IList<PairedRead> strucAnom = reads.Where(PE => PE.PairedType == PairedReadType.StructuralAnomaly).ToList();
                IList<PairedRead> lenAnom = reads.Where(PE => PE.PairedType == PairedReadType.LengthAnomaly).ToList();

                Assert.AreEqual(TotalReadsCount, map.QuerySequences.Count);

                Assert.AreEqual(multipleHitsPairCount, multipleHits.Count());
                Assert.AreEqual(multipleHitReadsCount, multipleHits.Sum(PE => PE.Reads.Count));

                Assert.AreEqual(normalPairCount, normal.Count());
                Assert.AreEqual(normalreadsCount, normal.Sum(PE => PE.Reads.Count));

                Assert.AreEqual(orphanPairsCount, orphan.Count());
                Assert.AreEqual(orphanreadsCount, orphan.Sum(PE => PE.Reads.Count));

                Assert.AreEqual(chimerapaircount, chimera.Count());
                Assert.AreEqual(chimerareadsCount, chimera.Sum(PE => PE.Reads.Count));

                Assert.AreEqual(strucAnomPairCount, strucAnom.Count());
                Assert.AreEqual(strucAnomReadsCount, strucAnom.Sum(PE => PE.Reads.Count));

                Assert.AreEqual(lenAnomPairCount, lenAnom.Count());
                Assert.AreEqual(LenAnomReadsCount, lenAnom.Sum(PE => PE.Reads.Count));
            }
        }

        /// <summary>
        /// Tests Chimeric stats using a sam file.
        /// </summary>
        [TestMethod]
        public void TestChimeraDataUsingSAMfile()
        {
            string samFilename = @"TestUtils\SAM\PairedReadsTest.sam";
            using (SAMParser bamParser = new SAMParser())
            {
                SequenceAlignmentMap alignmentMapobj = bamParser.Parse(samFilename);
                TestChimeraData(alignmentMapobj);
            }
        }

        /// <summary>
        /// Tests Chimeric stats using a bam file.
        /// </summary>
        [TestMethod]
        public void TestChimeraDataUsingBAMfile()
        {
            string bamFilename = @"TestUtils\BAM\PairedReadsTest.bam";
            using (BAMParser bamParser = new BAMParser())
            {
                SequenceAlignmentMap alignmentMapobj = bamParser.Parse(bamFilename);
                TestChimeraData(alignmentMapobj);
            }
        }

        /// <summary>
        /// Tests Orphan regions using a sam file.
        /// </summary>
        [TestMethod]
        public void TestOrphanRegionssUsingSAMFile()
        {
            string samFilename = @"TestUtils\SAM\PairedReadsTest.sam";
            using (SAMParser bamParser = new SAMParser())
            {
                SequenceAlignmentMap alignmentMapobj = bamParser.Parse(samFilename);
                TestOrphanRegions(alignmentMapobj);
            }
        }

        /// <summary>
        /// Tests Orphan regions using a bam file.
        /// </summary>
        [TestMethod]
        public void TestOrphansRegionsUsingBAMFile()
        {
            string bamFilename = @"TestUtils\BAM\PairedReadsTest.bam";
            using (BAMParser bamParser = new BAMParser())
            {
                SequenceAlignmentMap alignmentMapobj = bamParser.Parse(bamFilename);
                TestOrphanRegions(alignmentMapobj);
            }
        }

        /// <summary>
        /// Tests Length Anomalies
        /// </summary>
        [TestMethod]
        public void TestLengthAnomaliesUsingSAMfile()
        {
            string samFilename = @"TestUtils\SAM\PairedReadsTest.sam";
            using (SAMParser bamParser = new SAMParser())
            {
                SequenceAlignmentMap alignmentMapobj = bamParser.Parse(samFilename);
                TestLengthAnomalies(alignmentMapobj);
            }
        }

        /// <summary>
        /// Tests Length Anomalies
        /// </summary>
        [TestMethod]
        public void TestLengthAnomaliesUsingBAMfile()
        {
            string bamFilename = @"TestUtils\BAM\PairedReadsTest.bam";
            using (BAMParser bamParser = new BAMParser())
            {
                SequenceAlignmentMap alignmentMapobj = bamParser.Parse(bamFilename);
                TestLengthAnomalies(alignmentMapobj);
            }
        }

        /// <summary>
        /// Tests Coverage profile using sam file.
        /// </summary>
        [TestMethod]
        public void TestCoverageProfileUsingSAMFile()
        {
            using (SAMParser parser = new SAMParser())
            {
                SequenceAlignmentMap map = parser.Parse(@"TestUtils\SAM\PairedReadsTest.sam");
                TestCoverage("chr1", map, "true");
            }
        }

        /// <summary>
        /// Tests Coverage profile using bam file.
        /// </summary>
        [TestMethod]
        public void TestCoverageProfileUsingBAMFile()
        {
            using (BAMParser parser = new BAMParser())
            {
                SequenceAlignmentMap map = parser.Parse(@"TestUtils\BAM\PairedReadsTest.bam");
                TestCoverage("chr1", map, "true");
            }
        }

        /// <summary>
        /// Tests Posibility of SNPs using sam file.
        /// </summary>
        [TestMethod]
        public void TestSNPDetectionUsingSAMFile()
        {
            using (SAMParser parser = new SAMParser())
            {
                SequenceAlignmentMap map = parser.Parse(@"TestUtils\SAM\PairedReadsTest.sam");
                TestCoverage("chr1", map, "true");
            }
        }

        /// <summary>
        /// Tests Posibility of SNPs using bam file.
        /// </summary>
        [TestMethod]
        public void TestSNPDetectionUsingBAMFile()
        {
            using (BAMParser parser = new BAMParser())
            {
                SequenceAlignmentMap map = parser.Parse(@"TestUtils\BAM\PairedReadsTest.bam");
                TestCoverage("chr1", map, "true");
            }
        }

        /// <summary>
        /// Tests Chimeric stats.
        /// </summary>
        private static void TestChimeraData(SequenceAlignmentMap alignmentMapobj)
        {
            string expectedOutput;
            string actualOutput;

            expectedOutput = "varchr1chr2chr3chr4chr10320chr22040chr33100chr40000";

            // get reads from sequence alignment map object.
            IList<PairedRead> pairedReads = null;

            pairedReads = alignmentMapobj.GetPairedReads(200, 50);

            // select chimeras from reads.
            var chimeras = pairedReads.Where(PE => PE.PairedType == PairedReadType.Chimera);

            // Group chimeras based on first reads chromosomes name.
            var groupedChimeras =
            chimeras.GroupBy(PR => PR.Read1.RName);

            IList<string> chrs = alignmentMapobj.GetRefSequences();

            // Declare sparse matrix to store statistics.
            SparseMatrix<string, string, string> statistics =
                SparseMatrix<string, string, string>.CreateEmptyInstance(
                       chrs, chrs, "0");

            // For each group create sub group depending on the second reads chromosomes.
            foreach (var group in groupedChimeras)
            {
                foreach (var subgroup in group.GroupBy(PE => PE.Read2.RName))
                {
                    // store the count to stats
                    statistics[group.Key, subgroup.Key] = subgroup.Count().ToString(CultureInfo.InvariantCulture);
                }
            }

            actualOutput = statistics.ToString2D().Replace(Environment.NewLine, "").Replace("\t", "");
            Assert.AreEqual(expectedOutput, actualOutput);
        }

        /// <summary>
        /// Tests chromoses with orphan regions
        /// </summary>
        /// <param name="alignmentMapobj">Sequence alignment map.</param>
        private static void TestOrphanRegions(SequenceAlignmentMap alignmentMapobj)
        {
            string expectedOutput;
            string actualOutput;

            expectedOutput = "9437-9447:";
            actualOutput = string.Empty;

            // get reads from sequence alignment map object.
            IList<PairedRead> pairedReads = null;

            pairedReads = alignmentMapobj.GetPairedReads(0, 0);

            // Get the orphan regions.
            var orphans = pairedReads.Where(PR => PR.PairedType == PairedReadType.Orphan);

            if (orphans.Count() == 0)
            {
                Assert.Fail();
            }

            List<ISequenceRange> orphanRegions = new List<ISequenceRange>(orphans.Count());
            foreach (PairedRead orphanRead in orphans)
            {
                orphanRegions.Add(GetRegion(orphanRead.Read1));
            }

            // Get sequence range grouping object.
            SequenceRangeGrouping rangeGroup = new SequenceRangeGrouping(orphanRegions);

            SequenceRangeGrouping mergedRegions = rangeGroup.MergeOverlaps();

            foreach (var range in mergedRegions.GroupRanges)
                actualOutput += range.Start + "-" + range.End + ":";

            Assert.AreEqual(expectedOutput, actualOutput);
        }

        /// <summary>
        /// Tests Length Anomalies
        /// </summary>
        /// <param name="alignmentMapobj">Sequence alignment map.</param>
        private static void TestLengthAnomalies(SequenceAlignmentMap alignmentMapobj)
        {
            string expectedOutput;
            string actualOutput;

            expectedOutput = "9437-9447:9440-9447:";
            actualOutput = string.Empty;

            // get reads from sequence alignment map object.
            IList<PairedRead> pairedReads = null;

            pairedReads = alignmentMapobj.GetPairedReads(200, 50);

            // Get the orphan regions.
            var orphans = pairedReads.Where(PR => PR.PairedType == PairedReadType.Orphan);


            if (orphans.Count() == 0)
            {
                Assert.Fail();
            }

            List<ISequenceRange> orphanRegions = new List<ISequenceRange>(orphans.Count());
            foreach (PairedRead orphanRead in orphans)
            {
                orphanRegions.Add(GetRegion(orphanRead.Read1));
            }

            // Get sequence range grouping for Orphan regions.
            SequenceRangeGrouping orphanRangegroup = new SequenceRangeGrouping(orphanRegions);

            // Get the Length anomalies regions.
            var lengthAnomalies = pairedReads.Where(PE => PE.PairedType == PairedReadType.LengthAnomaly);

            if (lengthAnomalies.Count() == 0)
            {
                Assert.Fail();
            }

            List<ISequenceRange> lengthAnomalyRegions = new List<ISequenceRange>(lengthAnomalies.Count());
            foreach (PairedRead laRead in lengthAnomalies)
            {
                SequenceRange range = new SequenceRange();
                range.ID = laRead.Read1.RName;
                range.Start = laRead.Read1.Pos;
                range.End = laRead.Read1.Pos + laRead.InsertLength;
                lengthAnomalyRegions.Add(range);
            }

            // Get sequence range grouping for length anomaly regions.
            SequenceRangeGrouping lengthAnomalyRangegroup =
                                new SequenceRangeGrouping(lengthAnomalyRegions);

            SequenceRangeGrouping intersectedRegions =
                lengthAnomalyRangegroup.Intersect(orphanRangegroup);

            foreach (var range in intersectedRegions.GroupRanges)
                actualOutput += range.Start + "-" + range.End + ":";

            Assert.AreEqual(expectedOutput, actualOutput);
        }
                
        /// <summary>
        /// Tests Coverage and snps
        /// </summary>
        /// <param name="readname">Chromosoem name</param>
        /// <param name="alignmentMapobj">Alignment object</param>
        /// <param name="possibility">True for Nucleaotide distribution</param>
        private static void TestCoverage(string readname, SequenceAlignmentMap alignmentMapobj,
                 string possibility)
        {
            TextReader reader = null;
                StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            try
            {
                if (possibility == "true")
                {
                    reader = new StreamReader(@"TestUtils\SAM\CoverageOutput2.txt");
                }
                else
                {
                    reader = new StreamReader(@"TestUtils\SAM\CoverageOutput1.txt");
                }

                // Get reads aligned to chromosome. 
                // Reduce the output by considering only normal forward reads.
                var alignedPairedReads = alignmentMapobj.GetPairedReads(200, 50).Where(PE => PE.PairedType == PairedReadType.Normal).Select(PE => PE.Read1)
                            .Where(QS => readname.Equals(QS.RName)).ToList();

                //List<ISequenceItem> distinctChars =
                //    new List<ISequenceItem>(Alphabets.DNA);

                List<ISequenceItem> distinctChars =
                    new List<ISequenceItem> { Alphabets.DNA.A, Alphabets.DNA.T, Alphabets.DNA.G, Alphabets.DNA.C };

                // Dictionary to hold coverage profile.
                Dictionary<long, double[]> coverageProfile =
                                             new Dictionary<long, double[]>();

                // Get the position specific alphabet count.
                foreach (var read in alignedPairedReads)
                {
                    for (int i = 0; i < read.QuerySequence.Count; i++)
                    {
                        double[] values;

                        if (!coverageProfile.TryGetValue(read.Pos + i, out values))
                        {
                            coverageProfile.Add(read.Pos + i, new double[distinctChars.Count]);
                        }

                        ISequenceItem item = read.QuerySequence[i];
                        coverageProfile[read.Pos + i][distinctChars.IndexOf(item)]++;
                    }
                }


                // Get the position specific alphabet coverage.
                foreach (long i in coverageProfile.Keys)
                {
                    double count = coverageProfile[i].Sum();
                    for (int j = 0; j < distinctChars.Count; j++)
                    {
                        coverageProfile[i][j] = coverageProfile[i][j] / count;
                    }
                }

                // Display 
                foreach (long pos in coverageProfile.Keys.OrderBy(P => P))
                {
                    double[] values = coverageProfile[pos];
                    if (possibility == "true")
                    {
                        string possibleOccurence = GetMoreOccurences(values[0], values[1], values[2], values[3]);
                        writer.Write("\r\n{0,10}\t\t{1,4}%\t{2,4}%\t{3,4}%\t{4,4}%\t\t{5,4}", pos.ToString(CultureInfo.InvariantCulture),
                            values[0] * 100, values[1] * 100, values[2] * 100, values[3] * 100, possibleOccurence);
                    }
                    else
                    {
                        writer.Write("\r\n{0,10}\t\t{1,4:0.00}\t{2,4:0.00}\t{3,4:0.00}\t{4,4:0.00}", pos.ToString(CultureInfo.InvariantCulture),
                            values[0], values[1], values[2], values[3]);
                    }


                }

                writer.Flush();
                Assert.AreEqual(writer.ToString(), reader.ReadToEnd());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader = null;
                }

                if (writer != null)
                {
                    writer.Close();
                    writer = null;
                }
            }
        }

        /// <summary>
        /// Get the sequence item percentage with possibility of occurences
        /// </summary>
        /// <param name="aper">Percentage of A occurences</param>
        /// <param name="tper">Percentage of C occurences</param>
        /// <param name="gper">Percentage of G occurences</param>
        /// <param name="cper">Percentage of T occurences</param>
        /// <returns></returns>
        private static string GetMoreOccurences(
            double aper, double tper, double gper, double cper)
        {

            HashSet<ISequenceItem> symbols = new HashSet<ISequenceItem>();

            if (aper > 0.45)
            {
                symbols.Add(Alphabets.DNA.A);
            }

            if (tper > 0.45)
            {
                symbols.Add(Alphabets.DNA.T);
            }
            if (gper > 0.45)
            {
                symbols.Add(Alphabets.DNA.G);
            }
            if (cper > 0.45)
            {
                symbols.Add(Alphabets.DNA.C);
            }

            ISequenceItem item = Alphabets.DNA.GetConsensusSymbol(symbols);
            if (item.IsAmbiguous)
            {
                return item.Name;
            }

            return item.Symbol.ToString();
        }

        /// <summary>
        /// Gets an instance of SequenceRange class which represets alignment reigon of 
        /// specified aligned sequence (read) with reference sequence.
        /// </summary>
        /// <param name="alignedSequence">Aligned sequence.</param>
        private static ISequenceRange GetRegion(SAMAlignedSequence alignedSequence)
        {
            string refSeqName = alignedSequence.RName;
            long startPos = alignedSequence.Pos;
            long endPos = alignedSequence.Pos + alignedSequence.QueryLength;
            return new SequenceRange(refSeqName, startPos, endPos);
        }
    }
}
