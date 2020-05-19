// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * VirtualQualitativeSequenceListBvtTestCases.cs
 * 
 *   This file contains the VirtualQualitativeSequenceList Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

using MBF.IO;
using MBF.IO.FastQ;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// VirtualQualitativeSequenceList Bvt Test case implementation.
    /// </summary>
    [TestClass]
    public class VirtualQualitativeSequenceListBvtTestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\FastQTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static VirtualQualitativeSequenceListBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region VirtualQualitativeSequenceList Test Cases

        /// <summary>
        /// Validate Contains() method.
        /// Input : Valid parser
        /// Validation : True/False.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVQSLContains()
        {
            VirtualQualitativeSequenceList seqList =
                GetVirtualQualitativeSequenceList(
                Constants.LargeSizeDnaIlluminaFastQFileNode);

            Assert.IsTrue(seqList.Contains(seqList[0]));
            Assert.IsTrue(seqList.Contains(seqList[10]));

            // Cleanup the weak reference dictionary
            for (int i = 0; i < Constants.MaximumDictionaryLength; i++)
            {
                Assert.IsTrue(seqList.Contains(seqList[i]));
            }
            Assert.IsFalse(seqList.Contains(null));

            ICollection<ISequence> seqContains = seqList;
            Assert.IsTrue(seqContains.Contains(seqList[0]));

            ApplicationLog.WriteLine(
                "VQSL Bvt : Successfully validated the Contains() method");
            Console.WriteLine(
                "VQSL Bvt : Successfully validated the Contains() method");
        }

        /// <summary>
        /// Validate GetEnumerator() method.
        /// Input : Valid Values
        /// Validation : Validate Enumerator
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVQSLGetEnum()
        {
            VirtualQualitativeSequenceList seqList =
                GetVirtualQualitativeSequenceList(
                Constants.LargeSizeDnaIlluminaFastQFileNode);

            IEnumerator<IQualitativeSequence> getEnum = seqList.GetEnumerator();
            Assert.IsNull(getEnum.Current);

            IEnumerable<ISequence> getEnumSeq = seqList;
            getEnumSeq.GetEnumerator();
            Assert.IsNull(getEnum.Current);

            IEnumerable getQualSeq = seqList;
            getQualSeq.GetEnumerator();
            Assert.IsNull(getEnum.Current);

            ApplicationLog.WriteLine(
                "VQSL Bvt : Successfully validated the GetEnumerator() method");
            Console.WriteLine(
                "VQSL Bvt : Successfully validated the GetEnumerator() method");
        }

        /// <summary>
        /// Validate CopyTo() method.
        /// Input : Valid parser
        /// Validation : Copy is successful.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVQSLCopyTo()
        {
            VirtualQualitativeSequenceList seqList =
                GetVirtualQualitativeSequenceList(
                Constants.LargeSizeDnaIlluminaFastQNode);
            ICollection<ISequence> seqCopy = seqList;
            int i = seqCopy.Count;

            IQualitativeSequence[] seqArray = new IQualitativeSequence[i];

            seqList.CopyTo(seqArray, 0);
            Assert.AreEqual(seqList[0].ToString(),
                seqArray[0].ToString());
            Assert.AreEqual(seqList[i - 1].ToString(),
                seqArray[i - 1].ToString());

            seqCopy.CopyTo(seqArray, 0);
            Assert.AreEqual(seqList[0].ToString(),
                seqArray[0].ToString());
            Assert.AreEqual(seqList[i - 1].ToString(),
                seqArray[i - 1].ToString());

            ApplicationLog.WriteLine(
                "VQSL Bvt : Successfully validated the CopyTo() method");
            Console.WriteLine(
                "VQSL Bvt : Successfully validated the CopyTo() method");
        }

        /// <summary>
        /// Validate Count() method.
        /// Input : Valid parser
        /// Validation : Count is successful.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVQSLCount()
        {
            VirtualQualitativeSequenceList seqList =
                GetVirtualQualitativeSequenceList(
                Constants.LargeSizeDnaIlluminaFastQNode);
            ICollection<ISequence> seqCount = seqList;
            Assert.AreEqual(Constants.VirtualQualSeqCount, seqCount.Count);

            ICollection<IQualitativeSequence> seqCountQual = seqList;
            Assert.AreEqual(Constants.VirtualQualSeqCount, seqCountQual.Count);

            ApplicationLog.WriteLine(
                "VQSL Bvt : Successfully validated the Count() method");
            Console.WriteLine(
                "VQSL Bvt : Successfully validated the Count() method");
        }

        /// <summary>
        /// Validate IndexOf() method.
        /// Input : Valid parser
        /// Validation : Index is got successful.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVQSLIndexOf()
        {
            VirtualQualitativeSequenceList seqList =
                GetVirtualQualitativeSequenceList(
                Constants.LargeSizeDnaIlluminaFastQNode);

            Assert.AreEqual(
                Constants.SeqIndexNumber, seqList.IndexOf(seqList[10]));

            IList<ISequence> seqIndexOf = seqList;
            Assert.AreEqual(
                Constants.SeqIndexNumber, seqIndexOf.IndexOf(seqList[10]));

            ApplicationLog.WriteLine(
                "VQSL Bvt : Successfully validated the IndexOf() method");
            Console.WriteLine(
                "VQSL Bvt : Successfully validated the IndexOf() method");
        }

        /// <summary>
        /// Validate all properties.
        /// Input : Valid values
        /// Validation : All set and get properties are working as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateVQSLProperties()
        {
            VirtualQualitativeSequenceList seqList =
                GetVirtualQualitativeSequenceList(
                Constants.LargeSizeDnaIlluminaFastQNode);
            seqList.CreateSequenceAsReadOnly = true;
            Assert.IsTrue(seqList.CreateSequenceAsReadOnly);

            ICollection<ISequence> seqProp = seqList;
            Assert.IsTrue(seqProp.IsReadOnly);

            ICollection<IQualitativeSequence> seqQualProp = seqList;
            Assert.IsTrue(seqQualProp.IsReadOnly);

            IList<ISequence> seq = seqList;
            Assert.IsNotNull(seq[0]);

            ApplicationLog.WriteLine(
                "VQSL Bvt : Successfully validated the all properties");
            Console.WriteLine(
                "VQSL Bvt : Successfully validated the all properties");
        }

        #endregion VirtualQualitativeSequenceList TestCases

        #region Supported Methods

        /// <summary>
        /// Gets the virtual sequence list
        /// </summary>
        /// <param name="nodeName">Xml node name</param>
        /// <returns>Virtual Sequence List</returns>
        VirtualQualitativeSequenceList
            GetVirtualQualitativeSequenceList(string nodeName)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName,
                Constants.FilePathNode);

            using (FastQParser parserObj = new FastQParser())
            {
                parserObj.EnforceDataVirtualization = true;
                VirtualQualitativeSequenceList seqList =
                    (VirtualQualitativeSequenceList)parserObj.Parse(filePath);

                return seqList;
            }
        }

        #endregion Supported Methods
    }
}