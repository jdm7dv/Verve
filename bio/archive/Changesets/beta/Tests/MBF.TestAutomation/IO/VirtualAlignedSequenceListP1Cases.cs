// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * VirtualAlignedSequenceListBvtTestCases.cs
 * 
 *   This file contains the VirtualAlignedSequenceList Bvt test cases.
 * 
***************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MBF.IO;
using MBF.IO.Fasta;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.Algorithms.Alignment;
using MBF.IO.SAM;
using MBF.IO.BAM;
using System.Collections;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// VirtualAlignedSequenceList P1 TestCases implementation.
    /// </summary>
    [TestClass]
    public class VirtualAlignedSequenceListP1Cases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\SAMBAMTestData\SAMBAMTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static VirtualAlignedSequenceListP1Cases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region VirtualAlignedSequenceList P1 Test Cases.

        /// <summary>
        /// Validate the VirtualAlignedSeq present at the first position
        /// in the VirtualAlignedSeqList.
        /// Input : VASeq
        /// Output : Validation of VASeq in the VASeqList.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateVirtualSAMAlignedSequenceListContains()
        {
            // Get values from XML node.
            string filePath = _utilityObj._xmlUtil.GetTextValue(Constants.SAMFileWithAllFieldsNode,
                Constants.FilePathNode1);

            // Parse a SAM file.
            using (SAMParser samParserObj = new SAMParser())
            {
                samParserObj.EnforceDataVirtualization = true;

                SequenceAlignmentMap alignedSeqList = samParserObj.Parse(filePath);
                IList<SAMAlignedSequence> samAlignedList = alignedSeqList.QuerySequences;

                VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                    GetSAMAlignedSequence(Constants.SAMFileWithAllFieldsNode);

                // Validate contains.
                Assert.IsTrue(virtualASeqList.Contains(virtualASeqList.First(
                    Q => Q.QuerySequence.ToString().Equals(
                        samAlignedList[0].QuerySequence.ToString()))));

                // Log to Nunit GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Virtual AlignedSequenceList P1 : VAS {0} is present in the virtualAlignedSequence List",
                    virtualASeqList[10]));
                Console.WriteLine(string.Format((IFormatProvider)null,
                     "Virtual AlignedSequenceList P1 : VAS {0} is present in the virtualAlignedSequence List",
                    virtualASeqList[10]));
            }
        }

        /// <summary>
        /// Validate the index of aligned sequence at different position in 
        /// VirtualAlignedSequenceList for large size SAM files.
        /// Input : VASeq
        /// Output : Validation of VASeq in the VASeqList.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateIndexOfVirtualSAMAlignedSequenceWithMediumSizeSAMFile()
        {
            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                GetSAMAlignedSequence(
                Constants.SAMFileWithRefNode);

            // Validate Index of Virtual Aligned Sequence items
            for (int index = 0; index > virtualASeqList.Count; index++)
            {
                Assert.AreEqual(index, virtualASeqList[index]);
            }

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Virtual AlignedSequenceList P1 : Validated the VAS IndexOf"));
            Console.WriteLine(string.Format((IFormatProvider)null,
                 "Virtual AlignedSequenceList P1 : Validated the VAS IndexOf"));
        }

        /// <summary>
        /// Validate VirtualAlignedSeqList all properties.
        /// Input : VASeq
        /// Output : Validation of virtual aligned sequence list properties.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateVAlignedSeqListProperties()
        {
            // Get values from XML node.
            string expectedCount = _utilityObj._xmlUtil.GetTextValue(Constants.SAMFileWithAllFieldsNode,
                Constants.VirtualAlignedSeqCountNode);

            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                GetSAMAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            // Validate VirtualAlignedSequenceList properties.
            Assert.AreEqual(Int32.Parse(expectedCount, (IFormatProvider)null),
                virtualASeqList.Count);
            Assert.IsTrue(virtualASeqList.IsReadOnly);

            // Log to Nunit GUI.
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Virtual AlignedSequenceList P1 : Validated the virtualAlignedSequence properties"));
            Console.WriteLine(string.Format((IFormatProvider)null,
                 "Virtual AlignedSequenceList P1 : Validated the virtualAlignedSequence properties"));
        }

        /// <summary>
        /// Validate VirtualAlignedSeq Insert()
        /// Input : VASeq
        /// Output : NotSupportedException.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateVAlignedSeqListInsert()
        {
            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                 GetSAMAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            try
            {
                virtualASeqList.Insert(0, virtualASeqList[0]);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                // Log to Nunit GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Virtual AlignedSequenceList P1 : Validated the Insert method successfullly"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                     "Virtual AlignedSequenceList P1 :  Validated the Insert method successfullly"));
            }
        }

        /// <summary>
        /// Validate VirtualAlignedSeq Add()
        /// Input : VASeq
        /// Output : NotSupportedException.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateVAlignedSeqListAdd()
        {
            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                 GetSAMAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            try
            {
                virtualASeqList.Add(virtualASeqList[0]);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                // Log to Nunit GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Virtual AlignedSequenceList P1 : Validated the Add method successfullly"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                     "Virtual AlignedSequenceList P1 :  Validated the Add method successfullly"));
            }
        }

        /// <summary>
        /// Validate VirtualAlignedSeq Clear()
        /// Input : VASeq
        /// Output : NotSupportedException.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateVAlignedSeqListClear()
        {
            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                 GetSAMAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            try
            {
                virtualASeqList.Clear();
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                // Log to Nunit GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Virtual AlignedSequenceList P1 : Validated the Clear method successfullly"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                     "Virtual AlignedSequenceList P1 :  Validated the Clear method successfullly"));
            }
        }

        /// <summary>
        /// Validate VirtualAlignedSeq RemoveAt()
        /// Input : VASeq
        /// Output : NotSupportedException.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateVAlignedSeqListRemoveAt()
        {
            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                 GetSAMAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            try
            {
                virtualASeqList.RemoveAt(0);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                // Log to Nunit GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Virtual AlignedSequenceList P1 : Validated the RemoveAt method successfullly"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                     "Virtual AlignedSequenceList P1 :  Validated the RemoveAt method successfullly"));
            }
        }

        /// <summary>
        /// Validate VirtualAlignedSeq Remove()
        /// Input : VASeq
        /// Output : NotSupportedException.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateVAlignedSeqListRemove()
        {
            VirtualAlignedSequenceList<SAMAlignedSequence> virtualASeqList =
                 GetSAMAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            try
            {
                virtualASeqList.Remove(virtualASeqList[0]);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                // Log to Nunit GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Virtual AlignedSequenceList P1 : Validated the RemoveAt method successfullly"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                     "Virtual AlignedSequenceList P1 :  Validated the RemoveAt method successfullly"));
            }
        }

        /// <summary>
        /// Validate ReadOnlyAlignedSequence Insert.
        /// Input : ReadOnly Aligned Seq
        /// Output : Not Supported Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReadOnlyAlignedSeqInsert()
        {
            ReadOnlyAlignedSequenceCollection readOnlyAlignedSeq =
                 GetReadOnlyAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            // Validate readonly Aligned sequence 
            try
            {
                readOnlyAlignedSeq.Insert(0, readOnlyAlignedSeq[10]);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq insert"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq insert"));
            }
        }

        /// <summary>
        /// Validate ReadOnlyAlignedSequence RemoveAt.
        /// Input : ReadOnly Aligned Seq
        /// Output : Not Supported Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReadOnlyAlignedSeqRemoveAt()
        {
            ReadOnlyAlignedSequenceCollection readOnlyAlignedSeq =
                 GetReadOnlyAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            // Validate readonly Aligned sequence RemoveAt 
            try
            {
                readOnlyAlignedSeq.RemoveAt(0);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq RemoveAt"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq RemoveAt"));
            }
        }

        /// <summary>
        /// Validate ReadOnlyAlignedSequence Add.
        /// Input : ReadOnly Aligned Seq
        /// Output : Not Supported Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReadOnlyAlignedSeqAdd()
        {
            ReadOnlyAlignedSequenceCollection readOnlyAlignedSeq =
                 GetReadOnlyAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            // Validate readonly Aligned sequence Add
            try
            {
                readOnlyAlignedSeq.Add(readOnlyAlignedSeq[0]);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq Add"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq Add"));
            }
        }

        /// <summary>
        /// Validate ReadOnlyAlignedSequence Clear.
        /// Input : ReadOnly Aligned Seq
        /// Output : Not Supported Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReadOnlyAlignedSeqClear()
        {
            ReadOnlyAlignedSequenceCollection readOnlyAlignedSeq =
                 GetReadOnlyAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            // Validate readonly Aligned sequence Clear
            try
            {
                readOnlyAlignedSeq.Clear();
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq Clear"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq Clear"));
            }
        }

        /// <summary>
        /// Validate ReadOnlyAlignedSequence Remove.
        /// Input : ReadOnly Aligned Seq
        /// Output : Not Supported Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReadOnlyAlignedSeqRemove()
        {
            ReadOnlyAlignedSequenceCollection readOnlyAlignedSeq =
                 GetReadOnlyAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            // Validate readonly Aligned sequence Remove
            try
            {
                readOnlyAlignedSeq.Remove(readOnlyAlignedSeq[0]);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq Remove"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq Remove"));
            }
        }

        /// <summary>
        /// Validate ReadOnlyAlignedSequence GetEnumerator.
        /// Input : ReadOnly Aligned Seq
        /// Output : Validatation of GetEnumerator.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReadOnlyAlignedSeqGetEnumerator()
        {
            ReadOnlyAlignedSequenceCollection readOnlyAlignedSeq =
                 GetReadOnlyAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            IEnumerator<IAlignedSequence> alignedList = readOnlyAlignedSeq.GetEnumerator();

            // Validate enumerator.
            int index = 0;
            while (alignedList.MoveNext())
            {
                Assert.AreEqual(alignedList.Current.Sequences[0].ToString(),
                    readOnlyAlignedSeq[index].Sequences[0].ToString());
                index++;
            }

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
               "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq GetEnumerator"));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq GetEnumerator"));
        }

        /// <summary>
        /// Validate ReadOnlyAlignedSequence GetEnumerator using IEnumerable.
        /// Input : ReadOnly Aligned Seq
        /// Output : Validatation of GetEnumerator.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReadOnlyAlignedSeqGetIEnumerable()
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(Constants.SAMFileWithAllFieldsNode,
                Constants.FilePathNode);

            using (SAMParser samParserObj = new SAMParser())
            {
                samParserObj.EnforceDataVirtualization = true;
                SequenceAlignmentMap alignedSeqList = samParserObj.Parse(filePath);
                IList<SAMAlignedSequence> samAlignedList = alignedSeqList.QuerySequences;

                IEnumerable readOnlyCollections =
                    new ReadOnlyAlignedSequenceCollection(samAlignedList);

                IEnumerator enumerator = readOnlyCollections.GetEnumerator();

                Assert.IsNotNull(enumerator);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq GetEnumerator"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq GetEnumerator"));
            }
        }

        /// <summary>
        /// Validate ReadOnlyAlignedSequence Properties.
        /// Input : ReadOnly Aligned Seq
        /// Output : Validatation of RedOnlySequence properties.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReadOnlyAlignedSeqProperties()
        {
            // Get values from XML node.
            string expectedCount = _utilityObj._xmlUtil.GetTextValue(Constants.SAMFileWithAllFieldsNode,
                Constants.VirtualAlignedSeqCountNode);

            ReadOnlyAlignedSequenceCollection readOnlyAlignedSeq =
                 GetReadOnlyAlignedSequence(Constants.SAMFileWithAllFieldsNode);

            // Validate ReadOnlySequenceCollection properties.
            Assert.IsTrue(readOnlyAlignedSeq.IsReadOnly);
            Assert.AreEqual(expectedCount, readOnlyAlignedSeq.Count.ToString((IFormatProvider)null));

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
               "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq Properties"));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "ReadOnlyAlignedSeqCollection P1 : Validated the ReadOnlyAlignedSeq Properties"));
        }

        #endregion VirtualAlignedSequenceList P1 Test Cases.

        #region Helper Methods

        /// <summary>
        /// Gets the VirtualSAMAlignedSequence.
        /// </summary>
        /// <param name="nodeName">XML nodename used for the different testcases</param>
        /// <returns>Virtual SAM AlignedSequence list</returns>
        VirtualAlignedSequenceList<SAMAlignedSequence> GetSAMAlignedSequence(
            string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            using (SAMParser samParserObj = new SAMParser())
            {
                samParserObj.EnforceDataVirtualization = true;
                SequenceAlignmentMap alignedSeqList = samParserObj.Parse(filePath);
                IList<SAMAlignedSequence> samAlignedList = alignedSeqList.QuerySequences;


                VirtualAlignedSequenceList<SAMAlignedSequence> virtualSamAlignedSeqList =
                    (VirtualAlignedSequenceList<SAMAlignedSequence>)samAlignedList;

                return virtualSamAlignedSeqList;
            }
        }

        /// <summary>
        /// Gets ReadOnlyCollections
        /// </summary>
        /// <param name="nodeName">XML node name used for different test cases</param>
        /// <returns></returns>
        ReadOnlyAlignedSequenceCollection GetReadOnlyAlignedSequence(
            string nodeName)
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
               Constants.FilePathNode);

            using (SAMParser samParserObj = new SAMParser())
            {
                samParserObj.EnforceDataVirtualization = true;
                SequenceAlignmentMap alignedSeqList = samParserObj.Parse(filePath);
                IList<SAMAlignedSequence> samAlignedList = alignedSeqList.QuerySequences;

                ReadOnlyAlignedSequenceCollection readOnlyCollections =
                    new ReadOnlyAlignedSequenceCollection(samAlignedList);

                return readOnlyCollections;
            }
        }

        #endregion Helper Methods
    }
}
