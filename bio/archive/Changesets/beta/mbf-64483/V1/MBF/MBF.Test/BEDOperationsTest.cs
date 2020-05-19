// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBF.IO.Bed;
using NUnit.Framework;
using System.IO;

namespace MBF.Test
{
    /// <summary>
    /// Class to test BED Operations like Merge, intersect etc.
    /// </summary>
    [TestFixture]
    public class BEDOperationsTest
    {
        /// <summary>
        /// Method to test Merge operation.
        /// </summary>
        [Test]
        public void MergeOperationTest()
        {
            string filepath = @"testdata\BED\Merge\Merge_single.BED";
            string resultfilepath = "tmp_mergeresult.bed";
            string expectedresultpath = @"testdata\BED\Merge\Result_Merge_Single_MinLength0.BED";
            BedParser parser = new BedParser();
            BedFormatter formatter = new BedFormatter();
            SequenceRangeGrouping seqGrouping = null;
            SequenceRangeGrouping result = null;
            bool resultvalue = false;
            resultfilepath = "tmp_mergeresult.bed";
            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Single_MinLength0.BED";
            seqGrouping = parser.ParseRangeGrouping(filepath);

            result = seqGrouping.MergeOverlaps();
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, 0, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Single_MinLength0.BED";
            result = seqGrouping.MergeOverlaps(0, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, 0, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Single_MinLength0.BED";
            result = seqGrouping.MergeOverlaps(0, false);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, 0, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Single_MinLength0.BED";
            result = seqGrouping.MergeOverlaps(0);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, 0, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Single_MinLength0.BED";
            result = seqGrouping.MergeOverlaps(0, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, 0, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Single_MinLength0.BED";
            result = seqGrouping.MergeOverlaps(0, false);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, 0, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Single_MinLength1.BED";
            result = seqGrouping.MergeOverlaps(1);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, 1, false);
            Assert.IsTrue(resultvalue);


            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Single_MinLength-1.BED";
            result = seqGrouping.MergeOverlaps(-1);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, -1, false);
            Assert.IsTrue(resultvalue);


            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Single_MinLength-3.BED";
            result = seqGrouping.MergeOverlaps(-3);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, seqGrouping, null, -3, false);
            Assert.IsTrue(resultvalue);

            string firstFile = @"testdata\BED\Merge\Merge_twofiles_1.BED";
            string secondFile = @"testdata\BED\Merge\Merge_twofiles_2.BED";
            SequenceRangeGrouping refSeqRange = parser.ParseRangeGrouping(firstFile);
            SequenceRangeGrouping querySeqRange = parser.ParseRangeGrouping(secondFile);

            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Two_MinLength0.BED";
            result = refSeqRange.MergeOverlaps(querySeqRange);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, false);
            Assert.IsTrue(resultvalue);

            result = refSeqRange.MergeOverlaps(querySeqRange, 0, false);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, false);
            Assert.IsTrue(resultvalue);

            result = refSeqRange.MergeOverlaps(querySeqRange, 0, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, true);
            Assert.IsTrue(resultvalue);

            result = refSeqRange.MergeOverlaps(querySeqRange, 0);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, false);
            Assert.IsTrue(resultvalue);

            result = refSeqRange.MergeOverlaps(querySeqRange, 0, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, true);
            Assert.IsTrue(resultvalue);

            result = refSeqRange.MergeOverlaps(querySeqRange, 0, false);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Two_MinLength1.BED";
            result = refSeqRange.MergeOverlaps(querySeqRange, 1, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 1, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Two_MinLength-1.BED";
            result = refSeqRange.MergeOverlaps(querySeqRange, -1, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, -1, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Two_MinLength-3.BED";
            result = refSeqRange.MergeOverlaps(querySeqRange, -3, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, -3, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Two_MinLength2.BED";
            result = refSeqRange.MergeOverlaps(querySeqRange, 2, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 2, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Merge\Result_Merge_Two_MinLength6.BED";
            result = refSeqRange.MergeOverlaps(querySeqRange, 6, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 6, true);
            Assert.IsTrue(resultvalue);
        }

        /// <summary>
        /// Method to test Intersect operation.
        /// </summary>
        [Test]
        public void IntersectOperationTest()
        {
            string resultfilepath = "tmp_mergeresult.bed";
            string expectedresultpath = string.Empty;
            BedParser parser = new BedParser();
            BedFormatter formatter = new BedFormatter();

            SequenceRangeGrouping result = null;
            bool resultvalue = false;
            resultfilepath = "tmp_mergeresult.bed";

            string reffile = @"testdata\BED\Intersect\Intersect_ref.BED";
            string queryFile = @"testdata\BED\Intersect\Intersect_query.BED";
            SequenceRangeGrouping refSeqRange = parser.ParseRangeGrouping(reffile);
            SequenceRangeGrouping querySeqRange = parser.ParseRangeGrouping(queryFile);

            expectedresultpath = @"testdata\BED\Intersect\Result_Intersect_MinOverlap1_OverLappingBases.BED";
            result = refSeqRange.Intersect(querySeqRange, 1,IntersectOutputType.OverlappingPiecesOfIntervals);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 1, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Intersect\Result_Intersect_MinOverlap1.BED";
            result = refSeqRange.Intersect(querySeqRange, 1, IntersectOutputType.OverlappingIntervals);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 1, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Intersect\Result_Intersect_MinOverlap0_OverLappingBases.BED";
            result = refSeqRange.Intersect(querySeqRange, 0, IntersectOutputType.OverlappingPiecesOfIntervals);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Intersect\Result_Intersect_MinOverlap0.BED";
            result = refSeqRange.Intersect(querySeqRange, 0, IntersectOutputType.OverlappingIntervals);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Intersect\Result_Intersect_MinOverlap1_OverLappingBases.BED";
            result = refSeqRange.Intersect(querySeqRange, 1, IntersectOutputType.OverlappingPiecesOfIntervals, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 1, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Intersect\Result_Intersect_MinOverlap1.BED";
            result = refSeqRange.Intersect(querySeqRange, 1, IntersectOutputType.OverlappingIntervals, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 1, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Intersect\Result_Intersect_MinOverlap0_OverLappingBases.BED";
            result = refSeqRange.Intersect(querySeqRange, 0, IntersectOutputType.OverlappingPiecesOfIntervals, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, true);
            Assert.IsTrue(resultvalue);


            expectedresultpath = @"testdata\BED\Intersect\Result_Intersect_MinOverlap0.BED";
            result = refSeqRange.Intersect(querySeqRange, 0, IntersectOutputType.OverlappingIntervals, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, true);
            Assert.IsTrue(resultvalue);
        }

        /// <summary>
        /// Test Subtract operation.
        /// </summary>
        [Test]
        public void SubtractTest()
        {
            string refSeqRangefile = @"testdata\BED\Subtract\Subtract_ref.BED";
            string querySeqRangefile = @"testdata\BED\Subtract\Subtract_query.BED";
            string resultfilepath = "tmp_mergeresult.bed";
            BedParser parser = new BedParser();
            BedFormatter formatter = new BedFormatter();
            SequenceRangeGrouping result = null;
            bool resultvalue = false;

            SequenceRangeGrouping refSeqRange = parser.ParseRangeGrouping(refSeqRangefile);
            SequenceRangeGrouping querySeqRange = parser.ParseRangeGrouping(querySeqRangefile);

            string expectedresultpath = @"testdata\BED\Subtract\Result_Subtract_minoverlap1.BED";
            result = refSeqRange.Subtract(querySeqRange, 1, SubtractOutputType.IntervalsWithNoOverlap);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 1, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Subtract\Result_Subtract_minoverlap0.BED";
            result = refSeqRange.Subtract(querySeqRange, 0, SubtractOutputType.IntervalsWithNoOverlap);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Subtract\Result_Subtract_minoverlap1.BED";
            result = refSeqRange.Subtract(querySeqRange, 1, SubtractOutputType.IntervalsWithNoOverlap, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 1, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Subtract\Result_Subtract_minoverlap0.BED";
            result = refSeqRange.Subtract(querySeqRange, 0, SubtractOutputType.IntervalsWithNoOverlap, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, true);
            Assert.IsTrue(resultvalue);


            expectedresultpath = @"testdata\BED\Subtract\Result_Subtract_minoverlap1.BED";
            result = refSeqRange.Subtract(querySeqRange, 1, SubtractOutputType.IntervalsWithNoOverlap, false);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 1, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Subtract\Result_Subtract_minoverlap0.BED";
            result = refSeqRange.Subtract(querySeqRange, 0, SubtractOutputType.IntervalsWithNoOverlap, false);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, false);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Subtract\Result_Subtract_minoverlap0_NOnOverlappingPieces.BED";
            result = refSeqRange.Subtract(querySeqRange, 0, SubtractOutputType.NonOverlappingPiecesOfIntervals, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 0, true);
            Assert.IsTrue(resultvalue);

            expectedresultpath = @"testdata\BED\Subtract\Result_Subtract_minoverlap1_NOnOverlappingPieces.BED";
            result = refSeqRange.Subtract(querySeqRange, 1, SubtractOutputType.NonOverlappingPiecesOfIntervals, true);
            formatter.Format(result, resultfilepath);
            resultvalue = CompareBEDOutput(resultfilepath, expectedresultpath);
            Assert.IsTrue(resultvalue);
            resultvalue = ValidateParentSeqRange(result, refSeqRange, querySeqRange, 1, true);
            Assert.IsTrue(resultvalue);
        }

        private bool CompareBEDOutput(string resultfile, string expectedresultfile)
        {
            StreamReader resultReader = new StreamReader(resultfile);
            StreamReader expectedResultReader = new StreamReader(expectedresultfile);
            string result = resultReader.ReadToEnd();
            string expectedResult = expectedResultReader.ReadToEnd();
            int resultvalue = string.Compare(result, expectedResult, true);
            resultReader.Close();
            expectedResultReader.Close();
            return resultvalue == 0;
        }

        /// <summary>
        /// Method to validate parent seq ranges in result.
        /// </summary>
        /// <param name="resultSeqRange">Result seq range group.</param>
        /// <param name="refSeqRange">Reference seq range group.</param>
        /// <param name="querySeqRange">Query seq range group.</param>
        /// <param name="minOverlap">Minimum overlap.</param>
        /// <param name="isParentSeqRangeRequired">Flag to indicate whether result should contain parent seq ranges or not.</param>
        /// <returns>Returns true if the parent seq ranges are valid; otherwise returns false.</returns>
        private bool ValidateParentSeqRange(SequenceRangeGrouping resultSeqRange, SequenceRangeGrouping refSeqRange, SequenceRangeGrouping querySeqRange, int minOverlap, bool isParentSeqRangeRequired)
        {
            IList<ISequenceRange> refSeqRangeList = new List<ISequenceRange>();
            IList<ISequenceRange> querySeqRangeList = new List<ISequenceRange>();

            foreach (string groupid in resultSeqRange.GroupIDs)
            {
                if (refSeqRange != null)
                {
                    refSeqRangeList = refSeqRange.GetGroup(groupid);
                }

                if (querySeqRange != null)
                {
                    querySeqRangeList = querySeqRange.GetGroup(groupid);
                }


                foreach (ISequenceRange resultRange in resultSeqRange.GetGroup(groupid))
                {
                    if (!isParentSeqRangeRequired)
                    {
                        if (resultRange.ParentSeqRanges.Count != 0)
                        {
                            return false;
                        }
                    }
                    else
                    {

                        int refCount = refSeqRangeList.Where(R => resultRange.ParentSeqRanges.Contains(R)).Count();
                        int queryCount = querySeqRangeList.Where(R => resultRange.ParentSeqRanges.Contains(R)).Count();


                        if (refCount + queryCount != resultRange.ParentSeqRanges.Count)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
