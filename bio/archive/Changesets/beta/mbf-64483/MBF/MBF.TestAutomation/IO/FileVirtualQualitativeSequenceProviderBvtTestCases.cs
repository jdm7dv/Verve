// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * FileVirtualQualitativeSequenceProviderBvtTestCases.cs
 * 
 *   This file contains the FileVirtualQualitativeSequenceProvider Bvt test cases.
 * 
***************************************************************************/

using System;

using MBF.IO;
using MBF.IO.FastQ;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// FileVirtualQualitativeSequenceProvider Bvt Test case implementation.
    /// </summary>
    [TestClass]
    public class FileVirtualQualitativeSequenceProviderBvtTestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\FastQTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static FileVirtualQualitativeSequenceProviderBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region FileVirtualQualitativeSequenceProvider Test Cases

        /// <summary>
        /// Validate FileVirtualQualitativeSequenceProvider(IVirtualSequenceParser, SequencePointer) constructor.
        /// Input : Valid IVirtualSequenceParser
        /// Validation : If all the required objects are initialized.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPVirtualSeqParserSeqPointerConstructor()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            SequencePointer seqPointerObj = GetSequencePointer();

            Assert.AreEqual(26, provObj.Count);
            Assert.AreEqual(seqPointerObj.Id, provObj.SequencePointerInstance.Id);

            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the constructor 
                FileVirtualQualitativeSequenceProvider(IVirtualSequenceParser, SequencePointer)");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the constructor 
                FileVirtualQualitativeSequenceProvider(IVirtualSequenceParser, SequencePointer)");
        }

        /// <summary>
        /// Validate Add() method.
        /// Input : Valid SequenceItem.
        /// Validation : If successfully added.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPAdd()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "AGGCT");

            provObj.Add(seqObj[0]);

            Assert.AreEqual(seqObj[0].Name, provObj[provObj.Count - 1].Name);
            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the Add() method");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the Add() method");
        }

        /// <summary>
        /// Validate Clear() method.
        /// Input : null.
        /// Validation : clear() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPClear()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            // Clear
            provObj.Clear();

            Assert.AreEqual(0, provObj.Count);
            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the Clear() method");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the Clear() method");
        }

        /// <summary>
        /// Validate CopyTo() method.
        /// Input : valid input.
        /// Validation : CopyTo() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPCopyTo()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "AGGCT");

            provObj.Add(seqObj[0]);
            ISequenceItem[] aryObj = new ISequenceItem[provObj.Count];
            provObj.CopyTo(aryObj, 0);

            Assert.AreEqual(aryObj[provObj.Count - 1].Name, seqObj[0].Name);

            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the CopyTo() method");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the CopyTo() method");
        }

        /// <summary>
        /// Validate Contains() method.
        /// Input : valid input.
        /// Validation : Contains() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPContains()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "AGGCT");

            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            Assert.IsTrue(provObj.Contains(seqObj[0]));

            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the Contains() method");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the Contains() method");
        }

        /// <summary>
        /// Validate IndexOf() method.
        /// Input : valid input.
        /// Validation : IndexOf() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPIndexOf()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "AGGCT");

            provObj.Add(seqObj[0]);
            Assert.AreEqual(5, provObj.IndexOf(seqObj[0]));

            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the IndexOf() method");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the IndexOf() method");
        }

        /// <summary>
        /// Validate Insert(int, char) method.
        /// Input : valid index and character.
        /// Validation : Insert(int, char) method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPInsertChar()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                 new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "A");
            provObj.Insert(0, 'A');
            Assert.AreEqual(seqObj[0], provObj[0]);

            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the Insert(int, char) method");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the Insert(int, char) method");
        }

        /// <summary>
        /// Validate Insert(int, seqItem) method.
        /// Input : valid index and sequence item.
        /// Validation : Insert(int, seqItem) method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPInsertSeqItem()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                 new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "A");
            provObj.Insert(0, seqObj[0]);
            Assert.AreEqual(seqObj[0], provObj[0]);

            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the Insert(int, seqItem) method");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the Insert(int, seqItem) method");
        }

        /// <summary>
        /// Validate InsertRange() method.
        /// Input : valid index and sequence.
        /// Validation : InsertRange() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPInsertRange()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                 new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "AGGCT");
            provObj.InsertRange(0, "AGGCT");

            Assert.AreEqual(seqObj[0], provObj[0]);
            Assert.AreEqual(seqObj[1], provObj[1]);
            Assert.AreEqual(seqObj[2], provObj[2]);
            Assert.AreEqual(seqObj[3], provObj[3]);
            Assert.AreEqual(seqObj[4], provObj[4]);

            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the InsertRange() method");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the InsertRange() method");
        }

        /// <summary>
        /// Validate Remove() method.
        /// Input : valid sequence item.
        /// Validation : Remove() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPRemove()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                 new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "AGGCT");
            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            provObj.Add(seqObj[2]);
            provObj.Remove(provObj[1]);

            Assert.AreEqual(seqObj[2], provObj[2]);

            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the Remove() method");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the Remove() method");
        }

        /// <summary>
        /// Validate RemoveAt() method.
        /// Input : valid sequence item.
        /// Validation : RemoveAt() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPRemoveAt()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                 new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "AGGCT");
            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            provObj.Add(seqObj[2]);
            provObj.RemoveAt(0);

            Assert.AreEqual(seqObj[1].Name, provObj[0].Name);
            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the RemoveAt() method");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the RemoveAt() method");
        }

        /// <summary>
        /// Validate RemoveRange() method.
        /// Input : valid sequence item.
        /// Validation : RemoveRange() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPRemoveRange()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                 new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "AGGCT");
            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            provObj.Add(seqObj[2]);
            provObj.RemoveRange(0, 1);

            Assert.AreEqual(seqObj[1], provObj[0]);

            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the RemoveRange() method");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the RemoveRange() method");
        }

        /// <summary>
        /// Validate Replace(int, char) method.
        /// Input : valid index and Character.
        /// Validation : Replace(int, char) method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPReplaceChar()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                 new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "AGGCT");

            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            provObj.Add(seqObj[2]);
            provObj.Add(seqObj[3]);
            provObj.Replace(0, 'C');

            Assert.AreEqual(seqObj[3], provObj[0]);

            ApplicationLog.WriteLine(
                "FVQSP Bvt : Successfully validated the Replace(int, char) method");
            Console.WriteLine(
                "FVQSP Bvt : Successfully validated the Replace(int, char) method");
        }

        /// <summary>
        /// Validate Replace(int, seqItem) method.
        /// Input : valid index and Character.
        /// Validation : Replace(int, seqItem) method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPReplaceSeqItem()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                  new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "AGGCT");

            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            provObj.Add(seqObj[2]);
            provObj.Add(seqObj[3]);
            provObj.Replace(0, seqObj[3]);

            Assert.AreEqual(seqObj[3], provObj[0]);

            ApplicationLog.WriteLine(
                "FVQSP Bvt : Successfully validated the Replace(int, seqItem) method");
            Console.WriteLine(
                "FVQSP Bvt : Successfully validated the Replace(int, seqItem) method");
        }

        /// <summary>
        /// Validate ReplaceRange() method.
        /// Input : valid sequence item.
        /// Validation : ReplaceRange() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPReplaceRange()
        {
            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            IQualitativeSequence seqObj =
                 new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "AGGC");

            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            provObj.ReplaceRange(provObj.Count - 2, "GC");

            Assert.AreEqual(seqObj[2].Name, provObj[provObj.Count - 2].Name);
            Assert.AreEqual(seqObj[3].Name, provObj[provObj.Count - 1].Name);

            ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated the ReplaceRange() method");
            Console.WriteLine(@"FVQSP Bvt : Successfully validated the ReplaceRange() method");
        }

        /// <summary>
        /// Validate All properties.
        /// Input : update all properties.
        /// Validation : all properties.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVQSPProperties()
        {
            IVirtualSequenceParser parserObj = new FastQParser();

            try
            {
                FileVirtualQualitativeSequenceProvider provObj =
                    new FileVirtualQualitativeSequenceProvider(parserObj, GetSequencePointer());
                provObj.BlockSize = 5;
                provObj.IsReadOnly = false;
                provObj.MaxNumberOfBlocks = 10;
                provObj.SequencePointerInstance = GetSequencePointer();
                SequencePointer seqPoint = GetSequencePointer();
                Assert.AreEqual(5, provObj.BlockSize);
                Assert.AreEqual(10, provObj.MaxNumberOfBlocks);
                Assert.IsFalse(provObj.IsReadOnly);
                Assert.AreEqual(seqPoint.AlphabetName, provObj.SequencePointerInstance.AlphabetName);
                Assert.AreEqual(26, provObj.Count);

                ApplicationLog.WriteLine(@"FVQSP Bvt : Successfully validated all the properties");
                Console.WriteLine(@"FVQSP Bvt : Successfully validated all the properties");
            }
            finally
            {
                (parserObj as FastQParser).Dispose();
            }
        }

        /// <summary>
        /// Validate All properties in Sequence Pointer.
        /// Input : update all properties.
        /// Validation : all properties.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSequencePointerProperties()
        {
            SequencePointer pointerObj = new SequencePointer();
            pointerObj.AlphabetName = "Dna";
            pointerObj.Id = "PointerID";
            pointerObj.IndexOffsets[0] = 1;
            pointerObj.IndexOffsets[1] = 10;
            pointerObj.StartingLine = 1;

            Assert.AreEqual("Dna", pointerObj.AlphabetName);
            Assert.AreEqual("PointerID", pointerObj.Id);
            Assert.AreEqual(1, pointerObj.IndexOffsets[0]);
            Assert.AreEqual(10, pointerObj.IndexOffsets[1]);
            Assert.AreEqual(1, pointerObj.StartingLine);

            ApplicationLog.WriteLine(
                "Sequence Pointer Bvt : Successfully validated all the properties");
            Console.WriteLine(
                "Sequence Pointer Bvt : Successfully validated all the properties");
        }

        #endregion FileVirtualQualitativeSequenceProvider TestCases

        #region Supported Methods

        /// <summary>
        /// Gets the VirtualSequenceProvider
        /// </summary>
        /// <returns>Virtual Sequence Provider</returns>
        FileVirtualQualitativeSequenceProvider GetVirtualSequenceProvider()
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SingleSequenceSangerFastQNode, Constants.FilePathNode);

            using (FastQParser parserObj = new FastQParser())
            {
                parserObj.Parse(filePath);

                FileVirtualQualitativeSequenceProvider provObj =
                    new FileVirtualQualitativeSequenceProvider(parserObj,
                        GetSequencePointer());

                return provObj;
            }
        }

        /// <summary>
        /// Gets the SequencePointer
        /// </summary>
        /// <returns>Sequence Pointer</returns>
        private static SequencePointer GetSequencePointer()
        {
            SequencePointer pointerObj = new SequencePointer();
            pointerObj.AlphabetName = "DNA";
            pointerObj.Id =
                "SRR002012.1 Oct4:5:1:871:340 length=26";
            pointerObj.IndexOffsets[0] = 40;
            pointerObj.IndexOffsets[1] = pointerObj.IndexOffsets[0] + 26;
            pointerObj.StartingLine = 1;

            return pointerObj;
        }

        #endregion Supported Methods
    }
}
