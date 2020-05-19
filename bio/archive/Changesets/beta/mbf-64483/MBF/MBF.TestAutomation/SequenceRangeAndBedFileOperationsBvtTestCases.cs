// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SequenceRangeAndBedFileOperationsBvtTestCases.cs
 * 
 * This file contains the SequenceRange, SequenceRangeGrouping and BED file 
 * operations BVT test cases.
 * Copyright (c) Microsoft Corporation. All Rights Reserved
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

using MBF.IO.Bed;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF SequenceRange and BED file operations BVT level validations.
    /// </summary>
    [TestClass]
    public class SequenceRangeAndBedFileOperationsBvtTestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\BedTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SequenceRangeAndBedFileOperationsBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region SequenceRange And BED Operations BvtTestCases

        /// <summary>
        /// Validate creation of SequenceRange.
        /// Input Data : Valid Range ID,Start and End.
        /// Output Data : Validation of created SequenceRange.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSequenceRange()
        {
            // Get values from xml.
            string expectedRangeID = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceRangeNode, Constants.IDNode);
            string expectedStartIndex = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceRangeNode, Constants.StartNode);
            string expectedEndIndex = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceRangeNode, Constants.EndNode);

            // Create a SequenceRange.
            SequenceRange seqRange = new SequenceRange(expectedRangeID,
                long.Parse(expectedStartIndex, (IFormatProvider)null), long.Parse(expectedEndIndex, (IFormatProvider)null));

            // Validate created SequenceRange.
            Assert.AreEqual(expectedRangeID, seqRange.ID.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedStartIndex, seqRange.Start.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedEndIndex, seqRange.End.ToString((IFormatProvider)null));
            Console.WriteLine(
                "SequenceRange BVT : Successfully validated the SequenceStart,SequenceID and SequenceEnd.");
        }

        /// <summary>
        /// Validate comparison of two SequenceRanges.
        /// Input Data : Valid Range ID,Start and End.
        /// Output Data : Validation of cmompareTo.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateCompareTwoSequenceRanges()
        {
            // Get values from xml.
            string expectedRangeID = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceRangeNode, Constants.IDNode);
            string expectedStartIndex = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceRangeNode, Constants.StartNode);
            string expectedEndIndex = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceRangeNode, Constants.EndNode);
            string expectedRangeID1 = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceRangeNode, Constants.IDNode1);
            string expectedStartIndex1 = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceRangeNode, Constants.StartNode1);
            string expectedEndIndex1 = _utilityObj._xmlUtil.GetTextValue(
                Constants.SequenceRangeNode, Constants.EndNode1);

            // Create first SequenceRange.
            SequenceRange seqRange = new SequenceRange(expectedRangeID,
                long.Parse(expectedStartIndex, (IFormatProvider)null), long.Parse(expectedEndIndex, (IFormatProvider)null));

            // Create second SequenceRange.
            SequenceRange secondSeqRange = new SequenceRange(expectedRangeID1,
                long.Parse(expectedStartIndex1, (IFormatProvider)null), long.Parse(expectedEndIndex1, (IFormatProvider)null));

            // Compare two SequenceRanges which are identical.
            int result = seqRange.CompareTo(secondSeqRange);

            // Validate result of comparison.
            Assert.AreEqual(0, result);
            Console.WriteLine(
                "SequenceRange BVT : Successfully validated the SequenceRange comparison");
        }

        /// <summary>
        /// Validate creation of SequenceRangeGrouping.
        /// Input Data : Valid Range ID,Start and End.
        /// Output Data : Validation of created SequenceRangeGrouping.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSequenceRangeGrouping()
        {
            CreateSequenceRangeGrouping(Constants.SmallSizeBedNodeName);
        }

        /// <summary>
        /// Validate addition of SequenceRange to SequenceRangeGrouping.
        /// Input Data : Valid Range ID,Start and End.
        /// Output Data : Validation of adding SequenceRange to SequenceRangeGrouping.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAdditionOfSequenceRange()
        {
            CreateSequenceRangeGrouping(Constants.LongStartEndBedNodeName);
        }

        /// <summary>
        /// Validate getGroup() of SequenceRangeGrouping.
        /// Input Data : Valid Range ID,Start and End.
        /// Output Data : Validation of getGroup() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSequenceRangeGetGroup()
        {
            CreateSequenceRangeGrouping(Constants.SequenceRangeNode);
        }

        /// <summary>
        /// Validate SequenceRange MergeOveralp.
        /// Input Data : Valid small size BED file.
        /// Output Data : Validation of SequenceRange MergeOveralp.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSequenceRangeMergeOverlaps()
        {
            MergeSequenceRange(Constants.MergeBedFileNode, false, false);
        }

        /// <summary>
        /// Validate Merge two bed files.
        /// Input Data : Valid small size BED file.
        /// Output Data : Validation of Merge two bed files.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateMergeTwoBedFiles()
        {
            MergeSequenceRange(Constants.MergeTwoFiles, true, true);
        }

        /// <summary>
        /// Validate Intersect sequence range grouping without pieces of intervals
        /// Input Data : Two bed files..
        /// Output Data : Validate Intersect sequence range grouping.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateIntersectSequenceRangeGroupingWithoutPiecesOfIntervals()
        {
            IntersectSequenceRange(Constants.IntersectResultsWithoutPiecesOfIntervals, false, true);
        }

        /// <summary>
        /// Validate Intersect sequence range grouping with pieces of intervals
        /// Input Data : Two bed files..
        /// Output Data : Validate Intersect sequence range grouping.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateIntersectSequenceRangeGroupingWithPiecesOfIntervals()
        {
            IntersectSequenceRange(Constants.IntersectResultsWithPiecesOfIntervals, true, true);
        }

        /// <summary>
        /// Validate Intersect sequence range grouping without pieces of intervals
        /// for small size bed files.
        /// Input Data : Two small size bed files..
        /// Output Data : Validate Intersect sequence range grouping.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateIntersectSequenceRangeGroupingForSmallSizeBedFiles()
        {
            IntersectSequenceRange(Constants.IntersectWithoutPiecesOfIntervalsForSmallSizeFile,
                false, true);
        }

        /// <summary>
        /// Validate Intersect sequence range grouping with pieces of intervals
        /// for small size bed files.
        /// Input Data : Two small size bed files..
        /// Output Data : Validate Intersect sequence range grouping.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateIntersectSequenceRangeGroupingWithPiecesOfIntervalsForSmallSizeBedFiles()
        {
            IntersectSequenceRange(Constants.IntersectWithPiecesOfIntervalsForSmallSizeFile,
                true, false);
        }

        /// <summary>
        /// Validate Flatten method
        /// Input Data : SequenceRangeGroup
        /// Output Data : SequenceRangeList.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFlatten()
        {
            // Get values from xml.
            string expectedRangeIDs = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSizeBedNodeName, Constants.IDNode);
            string expectedStartIndex = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSizeBedNodeName, Constants.StartNode);
            string expectedEndIndex = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSizeBedNodeName, Constants.EndNode);
            string expectedSequenceRangeCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSizeBedNodeName, Constants.SequenceRangeCountNode);

            string[] rangeIDs = expectedRangeIDs.Split(',');
            string[] rangeStarts = expectedStartIndex.Split(',');
            string[] rangeEnds = expectedEndIndex.Split(',');
            SequenceRangeGrouping seqRangeGrouping = new SequenceRangeGrouping();
            List<ISequenceRange> rangeList = null;

            // Create a SequenceRange and add to SequenceRangeList.
            for (int i = 0; i < rangeIDs.Length; i++)
            {
                SequenceRange seqRange = new SequenceRange(rangeIDs[i],
                    long.Parse(rangeStarts[i], (IFormatProvider)null),
                    long.Parse(rangeEnds[i], (IFormatProvider)null));

                seqRangeGrouping.Add(seqRange);
            }

            //Convert SequenceRangeGroup to SequenceRangeList.
            rangeList = seqRangeGrouping.Flatten();

            int j = 0;
            // Validate created SequenceRanges.
            foreach (ISequenceRange seqRange in rangeList)
            {
                Assert.AreEqual(rangeStarts[j], seqRange.Start.ToString((IFormatProvider)null));
                Assert.AreEqual(rangeEnds[j], seqRange.End.ToString((IFormatProvider)null));
                Assert.AreEqual(rangeIDs[j], seqRange.ID.ToString((IFormatProvider)null));
                j++;
            }

            Assert.AreEqual(expectedSequenceRangeCount, rangeList.Count.ToString((IFormatProvider)null));
            Console.WriteLine(
                "SequenceRange BVT : Successfully validated the SequenceStart,SequenceID and SequenceEnd.");
        }

        /// <summary>
        /// Validate subtract two small size Bed files with minimal overlap and 
        /// with non overlapping pieces of intervals
        /// Input Data : Valid BED file.
        /// Output Data : Validation of subtract operation.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSubtractTwoBedFilesWithMinimalandNonOverlap()
        {
            SubtractSequenceRange(Constants.SubtractSmallBedFilesWithMinimalOverlapNodeName,
                false, true);
        }

        /// <summary>
        /// Validate subtract two small size Bed files and 
        /// with non overlapping pieces of intervals
        /// Input Data : Valid BED file.
        /// Output Data : Validation of subtract operation.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSubtractTwoBedFilesWithNonOverlapIntervals()
        {
            SubtractSequenceRange(Constants.SubtractSmallBedFilesNodeName,
                false, false);
        }

        /// <summary>
        /// Validate subtract two small size Bed files and 
        /// intervals with no overlap
        /// Input Data : Valid BED file.
        /// Output Data : Validation of subtract operation.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSubtractTwoBedFilesUsingIntervalsWithNoOverlap()
        {
            SubtractSequenceRange(Constants.SubtractSmallBedFilesWithIntervalsNodeName,
                true, true);
        }

        #endregion SequenceRange And BED Operations BvtTestCases

        #region Helper Methods

        /// <summary>
        /// Create a SequenceRangeGrouping by passing RangeID,Start and End Index.
        /// and validate created SequenceRange.
        /// </summary>
        /// <param name="nodeName">Xml Node name for different inputs.</param>
        void CreateSequenceRangeGrouping(string nodeName)
        {
            // Get values from xml.
            string expectedRangeIDs = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.IDNode);
            string expectedStartIndex = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.StartNode);
            string expectedEndIndex = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EndNode);
            string expectedSequenceRangeCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceRangeCountNode);

            string[] rangeIDs = expectedRangeIDs.Split(',');
            string[] rangeStarts = expectedStartIndex.Split(',');
            string[] rangeEnds = expectedEndIndex.Split(',');
            SequenceRangeGrouping seqRangeGrouping = new SequenceRangeGrouping();
            List<ISequenceRange> rangeList = null;

            // Create a SequenceRange and add to SequenceRangeList.
            for (int i = 0; i < rangeIDs.Length; i++)
            {
                SequenceRange seqRange = new SequenceRange(rangeIDs[i],
                    long.Parse(rangeStarts[i], (IFormatProvider)null), long.Parse(rangeEnds[i], (IFormatProvider)null));

                seqRangeGrouping.Add(seqRange);
            }

            IEnumerable<string> rangeGroupIds = seqRangeGrouping.GroupIDs;
            string rangeID = string.Empty;
            int j = 0;

            foreach (string groupID in rangeGroupIds)
            {
                rangeID = groupID;

                // Get SequenceRangeIds.
                rangeList = seqRangeGrouping.GetGroup(rangeID);

                // Validate created SequenceRanges.
                foreach (ISequenceRange seqRange in rangeList)
                {
                    Assert.AreEqual(rangeStarts[j], seqRange.Start.ToString((IFormatProvider)null));
                    Assert.AreEqual(rangeEnds[j], seqRange.End.ToString((IFormatProvider)null));
                    Assert.AreEqual(rangeIDs[j], seqRange.ID.ToString((IFormatProvider)null));
                    j++;
                }
            }
            Assert.AreEqual(expectedSequenceRangeCount,
                rangeList.Count.ToString((IFormatProvider)null));
            Console.WriteLine(
                "SequenceRange BVT : Successfully validated the SequenceStart, SequenceID and SequenceEnd.");
        }

        /// <summary>
        /// Validate Intersect SequenceRangeGrouping.
        /// </summary>
        /// <param name="nodeName">Xml Node name for different inputs.</param>
        /// <param name="overlappingBasePair">Value of overlappingBasePair</param>
        void IntersectSequenceRange(string nodeName,
            bool overlappingBasePair, bool IsParentSeqRangeRequired)
        {
            // Get values from xml.
            string[] expectedRangeIDs = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.IDNode).Split(',');
            string[] expectedStartIndex = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.StartNode).Split(',');
            string[] expectedEndIndex = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EndNode).Split(',');
            string referenceFilePath = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.FilePathNode);
            string queryFilePath = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.QueryFilePath);
            string minimalOverlap = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.OverlapValue);
            string rangeID = string.Empty;
            bool result = false;
            List<ISequenceRange> rangeList = null;

            // Parse a BED file.
            BedParser parserObj = new BedParser();
            SequenceRangeGrouping referenceGroup = parserObj.ParseRangeGrouping(referenceFilePath);
            SequenceRangeGrouping queryGroup = parserObj.ParseRangeGrouping(queryFilePath);

            IntersectOutputType outputType = IntersectOutputType.OverlappingIntervals;
            if (overlappingBasePair)
            {
                outputType = IntersectOutputType.OverlappingPiecesOfIntervals;
            }

            // Intersect a SequenceRangeGroup.
            SequenceRangeGrouping intersectGroup = referenceGroup.Intersect(queryGroup,
                long.Parse(minimalOverlap, (IFormatProvider)null), outputType);

            // Get a intersect SequenceGroup Id.
            IEnumerable<string> groupIds = intersectGroup.GroupIDs;
            int j = 0;
            foreach (string grpID in groupIds)
            {
                rangeID = grpID;

                rangeList = intersectGroup.GetGroup(rangeID);

                // Validate intersect sequence range.
                foreach (ISequenceRange range in rangeList)
                {
                    Assert.AreEqual(expectedStartIndex[j], range.Start.ToString((IFormatProvider)null));
                    Assert.AreEqual(expectedEndIndex[j], range.End.ToString((IFormatProvider)null));
                    Assert.AreEqual(expectedRangeIDs[j], range.ID.ToString((IFormatProvider)null));
                    j++;
                }
            }

            // Validate ParentSeqRanges.
            result = ValidateParentSeqRange(intersectGroup,
                referenceGroup, queryGroup, IsParentSeqRangeRequired);
            Assert.IsTrue(result);

            ApplicationLog.WriteLine(
                "Bed Parser BVT: Successfully validated the intersect SequeID, Start and End Ranges");
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Bed Parser BVT: Successfully validated the merged SequeID, Start and End Ranges"));
        }

        /// <summary>
        /// Validate Merge SequenceRangeGrouping.
        /// </summary>
        /// <param name="nodeName">Xml Node name for different inputs.</param>
        /// <param name="IsMergePam">Merge parameter</param>
        /// <param name="IsParentSeqRangesRequired">Is Parent Sequence Range required?</param>
        void MergeSequenceRange(string nodeName,
            bool IsMergePam, bool IsParentSeqRangesRequired)
        {
            // Get values from xml.
            string[] expectedRangeIDs = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.IDNode).Split(',');
            string[] expectedStartIndex = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.StartNode).Split(',');
            string[] expectedEndIndex = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EndNode).Split(',');
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string queryFilePath = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.QueryFilePath);
            string rangeID = string.Empty;
            bool result = false;
            List<ISequenceRange> rangeList = null;
            SequenceRangeGrouping mergedGroup = null;

            // Parse a BED file.
            BedParser parserObj = new BedParser();
            SequenceRangeGrouping referenceGroup = parserObj.ParseRangeGrouping(filePath);
            SequenceRangeGrouping queryGroup = parserObj.ParseRangeGrouping(queryFilePath);

            // Merge a SequenceRangeGroup.
            if (IsMergePam)
            {
                mergedGroup = referenceGroup.MergeOverlaps(queryGroup, 0, IsParentSeqRangesRequired);
            }
            else
            {
                mergedGroup = referenceGroup.MergeOverlaps();
            }

            // Get a merged SequenceGroup Id.
            IEnumerable<string> groupIds = mergedGroup.GroupIDs;

            int j = 0;
            foreach (string grpID in groupIds)
            {
                rangeID = grpID;

                rangeList = mergedGroup.GetGroup(rangeID);

                // Validate merged sequence range.
                foreach (ISequenceRange range in rangeList)
                {
                    Assert.AreEqual(expectedStartIndex[j], range.Start.ToString((IFormatProvider)null));
                    Assert.AreEqual(expectedEndIndex[j], range.End.ToString((IFormatProvider)null));
                    Assert.AreEqual(expectedRangeIDs[j], range.ID.ToString((IFormatProvider)null));
                    j++;
                }
            }

            // Validate Parent SequenceRanges.
            result = ValidateParentSeqRange(mergedGroup,
                referenceGroup, queryGroup, IsParentSeqRangesRequired);
            Assert.IsTrue(result);

            ApplicationLog.WriteLine(
                "Bed Parser BVT: Successfully validated the merged SequeID, Start and End Ranges");
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Bed Parser BVT: Successfully validated the merged SequeID, Start and End Ranges"));
        }

        /// <summary>
        /// Validate Subtract SequenceRangeGrouping.
        /// </summary>
        /// <param name="nodeName">Xml Node name for different inputs.</param>
        /// <param name="overlappingBasePair">Value of overlappingBasePair</param>
        /// <param name="IsParentSeqRangesRequired">Is Parent Sequence Range required?</param>
        void SubtractSequenceRange(string nodeName,
            bool overlappingBasePair, bool IsParentSeqRangeRequired)
        {
            // Get values from xml.
            string[] expectedRangeIDs = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.IDNode).Split(',');
            string[] expectedStartIndex = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.StartNode).Split(',');
            string[] expectedEndIndex = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EndNode).Split(',');
            string referenceFilePath = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.FilePathNode);
            string queryFilePath = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.QueryFilePath);
            string minimalOverlap = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.OverlapValue);
            string rangeID = string.Empty;
            bool result = false;

            List<ISequenceRange> rangeList = null;

            // Parse a BED file.
            BedParser parserObj = new BedParser();
            SequenceRangeGrouping referenceGroup = parserObj.ParseRangeGrouping(referenceFilePath);
            SequenceRangeGrouping queryGroup = parserObj.ParseRangeGrouping(queryFilePath);

            SubtractOutputType subtractOutputType = SubtractOutputType.NonOverlappingPiecesOfIntervals;
            if (overlappingBasePair)
            {
                subtractOutputType = SubtractOutputType.IntervalsWithNoOverlap;
            }

            // Subtract a SequenceRangeGroup.
            SequenceRangeGrouping subtractGroup = referenceGroup.Subtract(queryGroup,
                long.Parse(minimalOverlap, (IFormatProvider)null), subtractOutputType, IsParentSeqRangeRequired);

            // Get a intersect SequenceGroup Id.
            IEnumerable<string> groupIds = subtractGroup.GroupIDs;

            int j = 0;
            foreach (string grpID in groupIds)
            {
                rangeID = grpID;

                rangeList = subtractGroup.GetGroup(rangeID);

                // Validate intersect sequence range.
                foreach (ISequenceRange range in rangeList)
                {
                    Assert.AreEqual(expectedStartIndex[j], range.Start.ToString((IFormatProvider)null));
                    Assert.AreEqual(expectedEndIndex[j], range.End.ToString((IFormatProvider)null));
                    Assert.AreEqual(expectedRangeIDs[j], range.ID.ToString((IFormatProvider)null));
                    j++;
                }
            }

            // Validate ParentSeqRanges.
            result = ValidateParentSeqRange(
                subtractGroup, referenceGroup, queryGroup, IsParentSeqRangeRequired);
            Assert.IsTrue(result);

            ApplicationLog.WriteLine(
                "Bed Parser BVT: Successfully validated the subtract SequeID, Start and End Ranges");
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Bed Parser BVT: Successfully validated the subtracted SequeID, Start and End Ranges"));
        }

        /// <summary>
        /// Validate Parent Sequence ranges in result sequence range.
        /// </summary>
        /// <param name="resultSeq">Result seq range group.</param>
        /// <param name="refSeqRange">Reference seq range group.</param>
        /// <param name="querySeqRange">Query seq range group.</param>
        /// <param name="minOverlap">Minimum overlap.</param>
        /// <param name="isParentSeqRangeRequired">Flag to indicate whether 
        /// result should contain parent seq ranges or not.</param>
        /// <returns>Returns true if the parent seq ranges are valid; otherwise returns false.</returns>
        private static bool ValidateParentSeqRange(SequenceRangeGrouping resultSeq, SequenceRangeGrouping refSeq,
            SequenceRangeGrouping querySeq, bool IsParentSeqRangeRequired)
        {
            IList<ISequenceRange> refSeqRangeList = new List<ISequenceRange>();
            IList<ISequenceRange> querySeqRangeList = new List<ISequenceRange>();

            IEnumerable<string> groupIds = resultSeq.GroupIDs;

            foreach (string groupId in groupIds)
            {
                if (null != refSeq)
                {
                    refSeqRangeList = refSeq.GetGroup(groupId);
                }

                if (null != querySeq)
                {
                    querySeqRangeList = querySeq.GetGroup(groupId);
                }

                foreach (ISequenceRange resultRange in resultSeq.GetGroup(groupId))
                {
                    if (!IsParentSeqRangeRequired)
                    {
                        if (resultRange.ParentSeqRanges.Count != 0)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        int refSeqRangeCount =
                            refSeqRangeList.Where(S => resultRange.ParentSeqRanges.Contains(S)).Count();
                        int querySeqRangeCount =
                            querySeqRangeList.Where(S => resultRange.ParentSeqRanges.Contains(S)).Count();


                        if (resultRange.ParentSeqRanges.Count != refSeqRangeCount + querySeqRangeCount)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        #endregion Helper Methods
    }
}
