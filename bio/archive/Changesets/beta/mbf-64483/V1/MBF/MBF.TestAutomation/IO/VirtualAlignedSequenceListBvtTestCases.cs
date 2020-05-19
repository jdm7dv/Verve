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

using MBF.IO;
using MBF.IO.Fasta;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;
using MBF.Algorithms.Alignment;
using MBF.IO.SAM;
using MBF.IO.BAM;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// VirtualAlignedSequence BVT TestCases implementation.
    /// </summary>
    [TestFixture]
    public class VirtualAlignedSequenceListBvtTestCases
    {
        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static VirtualAlignedSequenceListBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\SAMBAMTestData\SAMBAMTestsConfig.xml");
        }

        #endregion Constructor

        #region VirtualAlignedSequence Test Cases

        /// <summary>
        /// Validate if VirtualAlignedSeq is present in the 
        /// VirtualAlignedSeqList or not
        /// Input : VASeq
        /// Output : Validation of VASeq in the VASeqList.
        /// </summary>
        [Test]
        public void ValidateVirtualSAMAlignedSequenceListContains()
        {
            // Get values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(Constants.SAMFileWithAllFieldsNode,
                Constants.FilePathNode1);

            // Parse a SAM file.
            SAMParser samParserObj = new SAMParser();
            samParserObj.EnforceDataVirtualization = true;

            SequenceAlignmentMap alignedSeqList = samParserObj.Parse(filePath);
            IList<SAMAlignedSequence> samAlignedList = alignedSeqList.QuerySequences;

            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                GetSAMAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            // Validate contains.
            Assert.IsTrue(virtualASeqList.Contains(virtualASeqList.FirstOrDefault(
                Q => Q.QuerySequence.ToString().Equals(samAlignedList[0].QuerySequence.ToString()))));
            Assert.IsTrue(virtualASeqList.Contains(virtualASeqList.FirstOrDefault(
                Q => Q.QuerySequence.ToString().Equals(samAlignedList[10].QuerySequence.ToString()))));
            Assert.IsTrue(virtualASeqList.Contains(virtualASeqList.FirstOrDefault(
                Q => Q.QName.ToString().Equals(samAlignedList[0].QName.ToString()))));
            Assert.IsTrue(virtualASeqList.Contains(virtualASeqList.FirstOrDefault(
                Q => Q.QName.ToString().Equals(samAlignedList[10].QName.ToString()))));

            // Log to Nunit GUI.
            ApplicationLog.WriteLine(string.Format(null,
                "Virtual AlignedSequenceList Bvt : VAS {0} is present in the virtualAlignedSequence List",
                virtualASeqList[10]));
            Console.WriteLine(string.Format(null,
                 "Virtual AlignedSequenceList Bvt : VAS {0} is present in the virtualAlignedSequence List",
                virtualASeqList[10]));
        }
        
        /// <summary>
        /// Validate Virtual Aligned Sequence CopyTo.
        /// Input : SAMAlignedSequence and array.
        /// Output : Validation of copied aligned sequence in an array.
        /// </summary>
        [Test]
        public void ValidateVirtualSAMAlignedSequenceListCopyTo()
        {
            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                GetSAMAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            SAMAlignedSequence[] samAlignedSeqList = new
                SAMAlignedSequence[virtualASeqList.Count];

            // Copy virtual aligned sequence to sam aligned sequence lilst array.
            virtualASeqList.CopyTo(samAlignedSeqList, 0);

            // Validate copied aligned sequences.
            for (int i = 0; i < virtualASeqList.Count; i++)
            {
                Assert.AreEqual(samAlignedSeqList[i].QuerySequence.ToString(),
                    virtualASeqList[i].QuerySequence.ToString());
            }

            ApplicationLog.WriteLine(string.Format(null,
                "Virtual AlignedSequenceList Bvt : Validated the VAS CopyTo"));
            Console.WriteLine(string.Format(null,
                 "Virtual AlignedSequenceList Bvt : Validated the VAS CopyTo"));
        }

        /// <summary>
        /// Validat index of virtualAligned sequence.
        /// Input : Virtual Aligned sequence.
        /// Output : Index of SAM aligned sequence.
        /// </summary>
        [Test]
        public void ValidateIndexOfVirtualSAMAlignedSequence()
        {
            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                GetSAMAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            // Validate Index of Virtual Aligned Sequence items
            for (int index = 0; index > virtualASeqList.Count; index++)
            {
                Assert.AreEqual(index, virtualASeqList[index]);
            }

            ApplicationLog.WriteLine(string.Format(null,
                "Virtual AlignedSequenceList Bvt : Validated the VAS IndexOf"));
            Console.WriteLine(string.Format(null,
                 "Virtual AlignedSequenceList Bvt : Validated the VAS IndexOf"));
        }

        /// <summary>
        /// Validate if VirtualBAMAlignedSeq is present in the 
        /// VirtualAlignedSeqList or not
        /// Input : VASeq
        /// Output : Validation of VASeq in the VASeqList.
        /// </summary>
        [Test]
        public void ValidateVirtualBAMAlignedSequenceListContains()
        {
            // Get values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.BAMFileWithMultipleAlignedSeqsNode,
                Constants.FilePathNode1);

            // Parse a BAM file.
            BAMParser bamParserObj = new BAMParser();
            bamParserObj.EnforceDataVirtualization = true;

            SequenceAlignmentMap alignedSeqList = bamParserObj.Parse(filePath);
            IList<SAMAlignedSequence> bamAlignedList = alignedSeqList.QuerySequences;

            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                GetBAMAlignedSequence(Constants.BAMFileWithMultipleAlignedSeqsNode);

            // Validate contains.
            Assert.IsTrue(virtualASeqList.Contains(virtualASeqList.FirstOrDefault(
                Q => Q.QuerySequence.ToString().Equals(
                    bamAlignedList[0].QuerySequence.ToString()))));
            Assert.IsTrue(virtualASeqList.Contains(virtualASeqList.FirstOrDefault(
                Q => Q.QuerySequence.ToString().Equals(
                    bamAlignedList[10].QuerySequence.ToString()))));
            Assert.IsTrue(virtualASeqList.Contains(virtualASeqList.FirstOrDefault(
                Q => Q.QName.ToString().Equals(bamAlignedList[0].QName.ToString()))));
            Assert.IsTrue(virtualASeqList.Contains(virtualASeqList.FirstOrDefault(
                Q => Q.QName.ToString().Equals(bamAlignedList[10].QName.ToString()))));

            // Log to Nunit GUI.
            ApplicationLog.WriteLine(string.Format(null,
                "Virtual AlignedSequenceList Bvt : VAS {0} is present in the virtualAlignedSequence List",
                virtualASeqList[10]));
            Console.WriteLine(string.Format(null,
                 "Virtual AlignedSequenceList Bvt : VAS {0} is present in the virtualAlignedSequence List",
                virtualASeqList[10]));
        }

        /// <summary>
        /// Validate Virtual Aligned Sequence CopyTo.
        /// Input : BAMAlignedSequence and array.
        /// Output : Validation of copied aligned sequence in an array.
        /// </summary>
        [Test]
        public void ValidateVirtualBAMAlignedSequenceListCopyTo()
        {
            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                GetBAMAlignedSequence(Constants.BAMFileWithMultipleAlignedSeqsNode);

            SAMAlignedSequence[] samAlignedSeqList = new
                SAMAlignedSequence[virtualASeqList.Count];

            // Copy virtual aligned sequence to sam aligned sequence lilst array.
            virtualASeqList.CopyTo(samAlignedSeqList, 0);

            // Validate copied aligned sequences.
            for (int i = 0; i < virtualASeqList.Count; i++)
            {
                Assert.AreEqual(samAlignedSeqList[i].QuerySequence.ToString(),
                    virtualASeqList[i].QuerySequence.ToString());
            }

            ApplicationLog.WriteLine(string.Format(null,
                "Virtual AlignedSequenceList Bvt : Validated the VAS CopyTo"));
            Console.WriteLine(string.Format(null,
                 "Virtual AlignedSequenceList Bvt : Validated the VAS CopyTo"));
        }

        /// <summary>
        /// Validat index of virtualBAMAligned sequence.
        /// Input : BAM Aligned sequence.
        /// Output : Index of BAM Aligned sequence in VirtualAlignedSeqList
        /// </summary>
        [Test]
        public void ValidateIndexOfVirtualBAMAlignedSequence()
        {
            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                GetBAMAlignedSequence(Constants.BAMFileWithMultipleAlignedSeqsNode);

            // Validate Index of Virtual Aligned Sequence items
            for (int index = 0; index > virtualASeqList.Count; index++)
            {
                Assert.AreEqual(index, virtualASeqList[index]);
            }

            ApplicationLog.WriteLine(string.Format(null,
                "Virtual AlignedSequenceList Bvt : Validated the VAS IndexOf"));
            Console.WriteLine(string.Format(null,
                 "Virtual AlignedSequenceList Bvt : Validated the VAS IndexOf"));
        }

        /// <summary>
        /// Validate ReadOnlyAlignedSequence Collection.
        /// Input : Aligned sequence List
        /// Output : Validation of ReadOnlyAlignedSequence collection.
        /// </summary>
        [Test]
        public void ValidateReadOnlyAlignedSeqCtors()
        {
            string expectedAlignedSeqCount = Utility._xmlUtil.GetTextValue(
                Constants.SAMFileWithAllFieldsNode,
                Constants.ExpectedAlignedSeqCountNode);
            ReadOnlyAlignedSequenceCollection readOnlyAlignedSeq =
                GetReadOnlyAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            // Validate readonly property.
            Assert.AreEqual(Int32.Parse(expectedAlignedSeqCount), readOnlyAlignedSeq.Count);
            Assert.IsTrue(readOnlyAlignedSeq.IsReadOnly);

            ApplicationLog.WriteLine(string.Format(null,
               "ReadOnlyAlignedSeqCollection Bvt : Validated the ReadOnlyAlignedSeq ctor"));
            Console.WriteLine(string.Format(null,
                "ReadOnlyAlignedSeqCollection Bvt : Validated the ReadOnlyAlignedSeq ctor"));
        }

        /// <summary>
        /// Validate ReadOnlyAlignedSeq contains method
        /// Input ReadOnlyAlignedSeq
        /// Output : Validation of ReadOnlyAlignedSeq contains method.
        /// </summary>
        [Test]
        public void ValidateReadOnlyAlignedSeqContains()
        {
            ReadOnlyAlignedSequenceCollection readOnlyAlignedSeq =
                GetReadOnlyAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            // Validate ReadOnlyCollection contains
            bool result = readOnlyAlignedSeq.Contains(readOnlyAlignedSeq[0]);

            Assert.IsTrue(result);
            ApplicationLog.WriteLine(string.Format(null,
               "ReadOnlyAlignedSeqCollection Bvt : Validated the ReadOnlyAlignedSeq contains"));
            Console.WriteLine(string.Format(null,
                "ReadOnlyAlignedSeqCollection Bvt : Validated the ReadOnlyAlignedSeq contains"));
        }

        /// <summary>
        /// Validate ReadOnlyAlignedSeq copyTo method
        /// Input ReadOnlyAlignedSeq
        /// Output : Validation of ReadOnlyAlignedSeq CopyTo method.
        /// </summary>
        [Test]
        public void ValidateReadOnlyAlignedSeqCopyTo()
        {
            ReadOnlyAlignedSequenceCollection readOnlyAlignedSeq =
                 GetReadOnlyAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            IAlignedSequence[] IAlignedSeqArray = new
                IAlignedSequence[readOnlyAlignedSeq.Count];

            readOnlyAlignedSeq.CopyTo(IAlignedSeqArray, 0);

            for (int index = 0; index < readOnlyAlignedSeq.Count; index++)
            {
                Assert.AreEqual(IAlignedSeqArray[index].Sequences[0].ToString(),
                    readOnlyAlignedSeq[index].Sequences[0].ToString());
            }

            ApplicationLog.WriteLine(string.Format(null,
               "ReadOnlyAlignedSeqCollection Bvt : Validated the ReadOnlyAlignedSeq CopyTo"));
            Console.WriteLine(string.Format(null,
                "ReadOnlyAlignedSeqCollection Bvt : Validated the ReadOnlyAlignedSeq CopyTo"));
        }

        /// <summary>
        /// Validate ReadOnlyAlignedSeq indexOf method
        /// Input ReadOnlyAlignedSeq
        /// Output : Validation of ReadOnlyAlignedSeq indexOf method.
        /// </summary>
        [Test]
        public void ValidateReadOnlyAlignedSeqIndexOf()
        {
            ReadOnlyAlignedSequenceCollection readOnlyAlignedSeq =
                 GetReadOnlyAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            Assert.AreEqual(0, readOnlyAlignedSeq.IndexOf(readOnlyAlignedSeq[0]));
            Assert.AreEqual(10, readOnlyAlignedSeq.IndexOf(readOnlyAlignedSeq[10]));

            ApplicationLog.WriteLine(string.Format(null,
               "ReadOnlyAlignedSeqCollection Bvt : Validated the ReadOnlyAlignedSeq IndexOf successfully"));
            Console.WriteLine(string.Format(null,
                "ReadOnlyAlignedSeqCollection Bvt : Validated the ReadOnlyAlignedSeq IndexOf successfully"));
        }

        #endregion VirtualAlignedSequence Test Cases

        #region Helper Methods

        /// <summary>
        /// Gets the VirtualSAMAlignedSequence.
        /// </summary>
        /// <param name="nodeName">XML nodename used for the different testcases</param>
        /// <returns>Virtual SAM AlignedSequence list</returns>
        static VirtualAlignedSequenceList<SAMAlignedSequence> GetSAMAlignedSequence(
            string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            SAMParser samParserObj = new SAMParser();

            samParserObj.EnforceDataVirtualization = true;
            SequenceAlignmentMap alignedSeqList = samParserObj.Parse(filePath);
            IList<SAMAlignedSequence> samAlignedList = alignedSeqList.QuerySequences;


            VirtualAlignedSequenceList<SAMAlignedSequence> virtualSamAlignedSeqList =
                (VirtualAlignedSequenceList<SAMAlignedSequence>)samAlignedList;

            return virtualSamAlignedSeqList;
        }

        /// <summary>
        /// Gets the VirtualSAMAlignedSequence.
        /// </summary>
        /// <param name="nodeName">XML nodename used for the different testcases</param>
        /// <returns>Virtual SAM AlignedSequence list</returns>
        static VirtualAlignedSequenceList<SAMAlignedSequence> GetBAMAlignedSequence(
            string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            BAMParser bamParserObj = new BAMParser();

            bamParserObj.EnforceDataVirtualization = true;
            SequenceAlignmentMap alignedSeqList = bamParserObj.Parse(filePath);
            IList<SAMAlignedSequence> bamAlignedList = alignedSeqList.QuerySequences;


            VirtualAlignedSequenceList<SAMAlignedSequence> virtualBamAlignedSeqList =
                (VirtualAlignedSequenceList<SAMAlignedSequence>)bamAlignedList;

            return virtualBamAlignedSeqList;
        }

        /// <summary>
        /// Gets ReadOnlyCollections
        /// </summary>
        /// <param name="nodeName">XML node name used for different test cases</param>
        /// <returns></returns>
        static ReadOnlyAlignedSequenceCollection GetReadOnlyAlignedSequence(
            string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
               Constants.FilePathNode);

            SAMParser samParserObj = new SAMParser();

            samParserObj.EnforceDataVirtualization = true;
            SequenceAlignmentMap alignedSeqList = samParserObj.Parse(filePath);
            IList<SAMAlignedSequence> samAlignedList = alignedSeqList.QuerySequences;

            ReadOnlyAlignedSequenceCollection readOnlyCollections =
                new ReadOnlyAlignedSequenceCollection(samAlignedList);

            return readOnlyCollections;
        }

        #endregion Helper Methods

    }
}
