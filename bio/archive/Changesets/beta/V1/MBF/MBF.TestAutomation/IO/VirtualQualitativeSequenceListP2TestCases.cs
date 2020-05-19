// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * VirtualQualitativeSequenceListP2TestCases.cs
 * 
 *   This file contains the VirtualQualitativeSequenceList P2 test cases.
 * 
***************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;

using MBF.IO;
using MBF.IO.FastQ;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// VirtualQualitativeSequenceList P2 Test case implementation.
    /// </summary>
    [TestFixture]
    public class VirtualQualitativeSequenceListP2TestCases
    {
        #region Enums

        /// <summary>
        /// Additional parameters to validate different scenarios.
        /// </summary>
        enum VirtualQualSeqListTestAttributes
        {
            Add,
            AddCollection,
            Remove,
            RemoveCollection,
            Insert,
            InsertSeq,
            Indexer,
            IndexerSeq,
            CopyTo,
            ClearQual,
            ClearSeq,
            RemoveAtQual,
            RemoveAtSeq
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static VirtualQualitativeSequenceListP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil =
                new XmlUtility(@"TestUtils\FastQTestsConfig.xml");
        }

        #endregion Constructor

        #region VirtualQualitativeSequenceList TestCases

        /// <summary>
        /// Invalidate Add() method.
        /// Input : Valid parser
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLAdd()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.Add);
        }

        /// <summary>
        /// Invalidate Add() method.
        /// Input : Valid parser
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLAddCollSeq()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.AddCollection);
        }

        /// <summary>
        /// Validate Remove() method.
        /// Input : Valid parser
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLRemove()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.Remove);
        }

        /// <summary>
        /// Validate Remove() method.
        /// Input : Valid parser
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLRemoveCollSeq()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.RemoveCollection);
        }

        /// <summary>
        /// Validate Insert() method.
        /// Input : Valid parser
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLInsert()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.Insert);
        }

        /// <summary>
        /// Validate Insert() method.
        /// Input : Valid parser
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLInsertListSeq()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.InsertSeq);
        }

        /// <summary>
        /// Invalidate Indexer() method of VirtualQualitySequenceList.
        /// Input : Set Indexer
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLIndexer()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.Indexer);
        }

        /// <summary>
        /// Invalidate Indexer() method of VirtualQualitySequenceList.
        /// Input : Set Indexer
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLIndexerListSeq()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.IndexerSeq);
        }

        /// <summary>
        /// Invalidate CopyTo()  method of VirtualQualitySequenceList.
        /// Input : null IQualitativeSequence
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLCopyTo()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.CopyTo);
        }

        /// <summary>
        /// Invalidate Clear() method of VirtualQualitySequenceList.
        /// Input : null IQualitativeSequence
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLClearListQualSeq()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.ClearQual);
        }

        /// <summary>
        /// Invalidate Clear() method of VirtualQualitySequenceList.
        /// Input : null IQualitativeSequence
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLClearListSeq()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.ClearSeq);
        }

        /// <summary>
        /// Invalidate RemoveAt() method of VirtualQualitySequenceList.
        /// Input : Pass Index
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLRemoveAtListQualSeq()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.RemoveAtQual);
        }

        /// <summary>
        /// Invalidate RemoveAt() method of VirtualQualitySequenceList.
        /// Input : Pass Index
        /// Validation : Exception is thrown.
        /// </summary>
        [Test]
        public void InvalidateVQSLRemoveAtListSeq()
        {
            ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes.RemoveAtSeq);
        }

        #endregion VirtualQualitativeSequenceList TestCases

        #region Supported Methods

        /// <summary>
        /// Gets the virtual sequence list
        /// </summary>
        /// <param name="nodeName">Xml node name</param>
        /// <returns>Virtual Sequence List</returns>
        static VirtualQualitativeSequenceList
            GetVirtualQualitativeSequenceList(string nodeName)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName,
                Constants.FilePathNode);

            FastQParser parserObj = new FastQParser();
            parserObj.EnforceDataVirtualization = true;
            VirtualQualitativeSequenceList seqList =
                (VirtualQualitativeSequenceList)parserObj.Parse(filePath);

            return seqList;
        }

        /// <summary>
        /// Genral method to validate virtual quality sequence list
        /// </summary>
        /// <param name="param">Method Name</param>
        static void ValidateVirtualQualitativeSequenceList(
            VirtualQualSeqListTestAttributes param)
        {
            VirtualQualitativeSequenceList seqList =
               GetVirtualQualitativeSequenceList(
               Constants.LargeSizeDnaIlluminaFastQNode);
            ICollection<ISequence> seqCollection = seqList;
            ICollection<IQualitativeSequence> seqQual = seqList;
            IList<ISequence> listSeq = seqList;
            IList<IQualitativeSequence> listQual = seqList;

            try
            {
                switch (param)
                {
                    case VirtualQualSeqListTestAttributes.Add:
                        seqList.Add((IQualitativeSequence)
                            new QualitativeSequence(
                                Alphabets.DNA,
                                FastQFormatType.Illumina, "ACCTT"));
                        break;
                    case VirtualQualSeqListTestAttributes.AddCollection:
                        seqCollection.Add(null as ISequence);
                        break;
                    case VirtualQualSeqListTestAttributes.ClearQual:
                        seqQual.Clear();
                        break;
                    case VirtualQualSeqListTestAttributes.ClearSeq:
                        seqCollection.Clear();
                        break;
                    case VirtualQualSeqListTestAttributes.CopyTo:
                        seqList.CopyTo(null, 1);
                        break;
                    case VirtualQualSeqListTestAttributes.Indexer:
                        seqList[0] = null;
                        break;
                    case VirtualQualSeqListTestAttributes.IndexerSeq:
                        listSeq[0] = null;
                        break;
                    case VirtualQualSeqListTestAttributes.Insert:
                        seqList.Insert(0,
                            (IQualitativeSequence)new QualitativeSequence(
                                Alphabets.DNA,
                                FastQFormatType.Illumina, "ACCTT"));
                        break;
                    case VirtualQualSeqListTestAttributes.InsertSeq:
                        listSeq.Insert(0,
                            (IQualitativeSequence)new QualitativeSequence(
                                Alphabets.DNA,
                                FastQFormatType.Illumina, "ACCTT"));
                        break;
                    case VirtualQualSeqListTestAttributes.Remove:
                        seqList.Remove(
                            (IQualitativeSequence)
                            new QualitativeSequence(
                                Alphabets.DNA,
                                FastQFormatType.Illumina, "ACCTT"));
                        break;
                    case VirtualQualSeqListTestAttributes.RemoveCollection:
                        seqCollection.Remove(
                            (IQualitativeSequence)
                            new QualitativeSequence(
                                Alphabets.DNA,
                                FastQFormatType.Illumina, "ACCTT"));
                        break;
                    case VirtualQualSeqListTestAttributes.RemoveAtQual:
                        listQual.RemoveAt(-1);
                        break;
                    case VirtualQualSeqListTestAttributes.RemoveAtSeq:
                        listSeq.RemoveAt(-1);
                        break;
                }
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(
                    "VQSL P2 : Successfully validated the method");
                Console.WriteLine(
                    "VQSL P2 : Successfully validated the method");
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "VQSL P2 : Successfully validated the method");
                Console.WriteLine(
                    "VQSL P2 : Successfully validated the method");
            }
        }

        #endregion Supported Methods
    }
}