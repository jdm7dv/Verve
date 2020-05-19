// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * VirtualSequenceListBvtTestCases.cs
 * 
 *   This file contains the VirtualSequenceList Bvt test cases.
 * 
***************************************************************************/

using System;

using MBF.IO;
using MBF.IO.Fasta;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// VirtualSequenceList Bvt Test case implementation.
    /// </summary>
    [TestFixture]
    public class VirtualSequenceListBvtTestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static VirtualSequenceListBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region VirtualSequenceList Test Cases

        /// <summary>
        /// Validate Add() method.
        /// Input : Valid parser
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void ValidateVSLAdd()
        {
            VirtualSequenceList seqList = GetVirtualSequenceList(
                Constants.NeedlemanWunschVeryLargeSizeProAlignAlgorithmNodeName);
            try
            {
                seqList.Add((ISequence)new Sequence(Alphabets.Protein,
                    "KKLLEE"));
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(
                    "VSL Bvt : Successfully validated the Add() method");
                Console.WriteLine(
                    "VSL Bvt : Successfully validated the Add() method");
            }
        }

        /// <summary>
        /// Validate Clear() method.
        /// Input : Valid parser
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void ValidateVSLClear()
        {
            VirtualSequenceList seqList = GetVirtualSequenceList(
                Constants.NeedlemanWunschVeryLargeSizeProAlignAlgorithmNodeName);
            try
            {
                seqList.Clear();
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(
                    "VSL Bvt : Successfully validated the Clear() method");
                Console.WriteLine(
                    "VSL Bvt : Successfully validated the Clear() method");
            }
        }

        /// <summary>
        /// Validate Remove() method.
        /// Input : Valid parser
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void ValidateVSLRemove()
        {
            VirtualSequenceList seqList = GetVirtualSequenceList(
                Constants.NeedlemanWunschVeryLargeSizeProAlignAlgorithmNodeName);
            try
            {
                seqList.Remove((ISequence)new Sequence(Alphabets.Protein,
                    "KKLLEE"));
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(
                    "VSL Bvt : Successfully validated the Remove() method");
                Console.WriteLine(
                    "VSL Bvt : Successfully validated the Remove() method");
            }
        }

        /// <summary>
        /// Validate RemoveAt() method.
        /// Input : Valid parser
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void ValidateVSLRemoveAt()
        {
            VirtualSequenceList seqList = GetVirtualSequenceList(
                Constants.NeedlemanWunschVeryLargeSizeProAlignAlgorithmNodeName);
            try
            {
                seqList.RemoveAt(0);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(
                    "VSL Bvt : Successfully validated the RemoveAt() method");
                Console.WriteLine(
                    "VSL Bvt : Successfully validated the RemoveAt() method");
            }
        }

        /// <summary>
        /// Validate Contains() method.
        /// Input : Valid parser
        /// Validation : True/False.
        /// </summary>
        [Test]
        public void ValidateVSLContains()
        {
            VirtualSequenceList seqList =
                GetVirtualSequenceList(Constants.MultiSequenceFileNodeName);

            Assert.IsTrue(seqList.Contains(seqList[10]));
            Assert.IsTrue(seqList.Contains(seqList[100]));

            ApplicationLog.WriteLine(
                "VSL Bvt : Successfully validated the Contains() method");
            Console.WriteLine(
                "VSL Bvt : Successfully validated the Contains() method");
        }

        /// <summary>
        /// Validate Insert() method.
        /// Input : Valid parser
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void ValidateVSLInsert()
        {
            VirtualSequenceList seqList = GetVirtualSequenceList(
                Constants.NeedlemanWunschVeryLargeSizeProAlignAlgorithmNodeName);
            try
            {
                seqList.Insert(0,
                    (ISequence)new Sequence(Alphabets.Protein,
                    "KKLLEE"));
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(
                    "VSL Bvt : Successfully validated the Insert() method");
                Console.WriteLine(
                    "VSL Bvt : Successfully validated the Insert() method");
            }
        }

        /// <summary>
        /// Validate CopyTo() method.
        /// Input : Valid parser
        /// Validation : Copy is successful.
        /// </summary>
        [Test]
        public void ValidateVSLCopyTo()
        {
            VirtualSequenceList seqList = GetVirtualSequenceList(
                Constants.MultiSequenceFileNodeName);

            ISequence[] seqArray = new Sequence[seqList.Count];
            seqList.CopyTo(seqArray, 0);
            Assert.AreEqual(seqList[0].ToString(),
                seqArray[0].ToString());
            Assert.AreEqual(seqList[seqList.Count - 1].ToString(),
                seqArray[seqList.Count - 1].ToString());

            ApplicationLog.WriteLine(
                "VSL Bvt : Successfully validated the CopyTo() method");
            Console.WriteLine(
                "VSL Bvt : Successfully validated the CopyTo() method");
        }

        /// <summary>
        /// Validate IndexOf() method.
        /// Input : Valid parser
        /// Validation : Index is got successful.
        /// </summary>
        [Test]
        public void ValidateVSLIndexOf()
        {
            VirtualSequenceList seqList = GetVirtualSequenceList(
                Constants.MultiSequenceFileNodeName);

            Assert.AreEqual(10, seqList.IndexOf(seqList[10]));

            ApplicationLog.WriteLine(
                "VSL Bvt : Successfully validated the IndexOf() method");
            Console.WriteLine(
                "VSL Bvt : Successfully validated the IndexOf() method");
        }

        /// <summary>
        /// Validate all properties.
        /// Input : Valid values
        /// Validation : All set and get properties are working as expected.
        /// </summary>
        [Test]
        public void ValidateVSLProperties()
        {
            VirtualSequenceList seqList = GetVirtualSequenceList(
                Constants.MultiSequenceFileNodeName);

            Assert.IsTrue(seqList.IsReadOnly);
            Assert.AreNotEqual(0, seqList.Count);

            ApplicationLog.WriteLine(
                "VSL Bvt : Successfully validated the all properties");
            Console.WriteLine(
                "VSL Bvt : Successfully validated the all properties");
        }

        #endregion VirtualSequenceList TestCases

        #region Supported Methods

        /// <summary>
        /// Gets the virtual sequence list
        /// </summary>
        /// <param name="nodeName">Xml node name</param>
        /// <returns>Virtual Sequence List</returns>
        static VirtualSequenceList GetVirtualSequenceList(string nodeName)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName,
                Constants.FilePathNode1);

            FastaParser parserObj = new FastaParser();
            parserObj.EnforceDataVirtualization = true;
            VirtualSequenceList seqList =
                (VirtualSequenceList)parserObj.Parse(filePath);

            return seqList;
        }

        #endregion Supported Methods
    }
}
