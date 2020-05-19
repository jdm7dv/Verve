// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * FileVirtualSequenceProviderBvtTestCases.cs
 * 
 *   This file contains the FileVirtualSequenceProvider Bvt test cases.
 * 
***************************************************************************/

using System;

using MBF.IO;
using MBF.IO.Fasta;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// FileVirtualSequenceProvider Bvt Test case implementation.
    /// </summary>
    [TestClass]
    public class FileVirtualSequenceProviderBvtTestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static FileVirtualSequenceProviderBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region FileVirtualSequenceProvider Test Cases

        /// <summary>
        /// Validate FileVirtualSequenceProvider(IVirtualSequenceParser, SequencePointer) constructor.
        /// Input : Valid IVirtualSequenceParser
        /// Validation : If all the required objects are initialized.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void
            ValidateFVSPVirtualSeqParserSeqPointerConstructor()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            SequencePointer seqPointerObj = GetSequencePointer();

            Assert.AreEqual(1301, provObj.Count);
            Assert.AreEqual(seqPointerObj.Id, provObj.SequencePointerInstance.Id);

            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the constructor 
                FileVirtualSequenceProvider(IVirtualSequenceParser, SequencePointer)");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the constructor 
                FileVirtualSequenceProvider(IVirtualSequenceParser, SequencePointer)");
        }

        /// <summary>
        /// Validate Add() method.
        /// Input : Valid SequenceItem.
        /// Validation : If successfully added.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPAdd()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            ISequence seqObj =
                new Sequence(Alphabets.DNA, "AGGCT");
            provObj.Add(seqObj[0]);

            Assert.AreEqual(seqObj[0], provObj[provObj.Count - 1]);
            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the Add() method");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the Add() method");
        }

        /// <summary>
        /// Validate Clear() method.
        /// Input : null.
        /// Validation : clear() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPClear()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();
            // Clear
            provObj.Clear();

            Assert.AreEqual(0, provObj.Count);
            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the Clear() method");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the Clear() method");
        }

        /// <summary>
        /// Validate CopyTo() method.
        /// Input : valid input.
        /// Validation : CopyTo() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPCopyTo()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            ISequence seqObj =
                new Sequence(Alphabets.DNA, "AGGCT");
            provObj.Add(seqObj[0]);
            ISequenceItem[] aryObj = new ISequenceItem[provObj.Count];
            provObj.CopyTo(aryObj, 0);

            Assert.AreEqual(aryObj[provObj.Count - 1], seqObj[0]);

            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the CopyTo() method");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the CopyTo() method");
        }

        /// <summary>
        /// Validate Contains() method.
        /// Input : valid input.
        /// Validation : Contains() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPContains()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            ISequence seqObj =
                new Sequence(Alphabets.DNA, "AGGCT");
            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            Assert.IsTrue(provObj.Contains(seqObj[0]));

            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the Contains() method");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the Contains() method");
        }

        /// <summary>
        /// Validate IndexOf() method.
        /// Input : valid input.
        /// Validation : IndexOf() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPIndexOf()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            ISequence seqObj =
                new Sequence(Alphabets.DNA, "AGGCT");
            provObj.Add(seqObj[0]);
            Assert.AreEqual(0, provObj.IndexOf(seqObj[0]));

            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the IndexOf() method");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the IndexOf() method");
        }

        /// <summary>
        /// Validate Insert(int, char) method.
        /// Input : valid index and character.
        /// Validation : Insert(int, char) method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPInsertChar()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            ISequence seqObj = new Sequence(Alphabets.DNA, "A");
            provObj.Insert(0, 'A');
            Assert.AreEqual(seqObj[0], provObj[0]);

            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the Insert(int, char) method");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the Insert(int, char) method");
        }

        /// <summary>
        /// Validate Insert(int, seqItem) method.
        /// Input : valid index and sequence item.
        /// Validation : Insert(int, seqItem) method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPInsertSeqItem()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            ISequence seqObj = new Sequence(Alphabets.DNA, "A");
            provObj.Insert(0, seqObj[0]);
            Assert.AreEqual(seqObj[0], provObj[0]);

            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the Insert(int, seqItem) method");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the Insert(int, seqItem) method");
        }

        /// <summary>
        /// Validate InsertRange() method.
        /// Input : valid index and sequence.
        /// Validation : InsertRange() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPInsertRange()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            ISequence seqObj = new Sequence(Alphabets.DNA, "GTTCG");
            provObj.InsertRange(0, "GTTCG");

            Assert.AreEqual(seqObj[0], provObj[0]);
            Assert.AreEqual(seqObj[1], provObj[1]);
            Assert.AreEqual(seqObj[2], provObj[2]);
            Assert.AreEqual(seqObj[3], provObj[3]);
            Assert.AreEqual(seqObj[4], provObj[4]);

            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the InsertRange() method");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the InsertRange() method");
        }

        /// <summary>
        /// Validate Remove() method.
        /// Input : valid sequence item.
        /// Validation : Remove() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPRemove()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            ISequence seqObj = new Sequence(Alphabets.DNA, "GTTCG");
            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            provObj.Add(seqObj[2]);
            provObj.Remove(provObj[1]);

            Assert.AreEqual(seqObj[2], provObj[2]);

            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the Remove() method");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the Remove() method");
        }

        /// <summary>
        /// Validate RemoveAt() method.
        /// Input : valid sequence item.
        /// Validation : RemoveAt() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPRemoveAt()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            ISequence seqObj = new Sequence(Alphabets.DNA, "GTTCG");
            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            provObj.Add(seqObj[2]);
            provObj.RemoveAt(0);

            Assert.AreEqual(seqObj[1].Name, provObj[0].Name);
            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the RemoveAt() method");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the RemoveAt() method");
        }

        /// <summary>
        /// Validate RemoveRange() method.
        /// Input : valid sequence item.
        /// Validation : RemoveRange() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPRemoveRange()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();
            ISequence seqObj = new Sequence(Alphabets.DNA, "GTTCG");
            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            provObj.Add(seqObj[2]);
            provObj.RemoveRange(0, 1);

            Assert.AreEqual(seqObj[1], provObj[0]);

            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the RemoveRange() method");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the RemoveRange() method");
        }

        /// <summary>
        /// Validate Replace(int, char) method.
        /// Input : valid index and Character.
        /// Validation : Replace(int, char) method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPReplaceChar()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            ISequence seqObj = new Sequence(Alphabets.DNA, "GTTCG");

            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            provObj.Add(seqObj[2]);
            provObj.Add(seqObj[3]);
            provObj.Replace(0, 'C');

            Assert.AreEqual(seqObj[3], provObj[0]);

            ApplicationLog.WriteLine(
                "FVSP Bvt : Successfully validated the Replace(int, char) method");
            Console.WriteLine(
                "FVSP Bvt : Successfully validated the Replace(int, char) method");
        }

        /// <summary>
        /// Validate Replace(int, seqItem) method.
        /// Input : valid index and Character.
        /// Validation : Replace(int, seqItem) method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPReplaceSeqItem()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            ISequence seqObj = new Sequence(Alphabets.DNA, "GTTCG");

            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            provObj.Add(seqObj[2]);
            provObj.Add(seqObj[3]);
            provObj.Replace(0, seqObj[3]);

            Assert.AreEqual(seqObj[3], provObj[0]);

            ApplicationLog.WriteLine(
                "FVSP Bvt : Successfully validated the Replace(int, seqItem) method");
            Console.WriteLine(
                "FVSP Bvt : Successfully validated the Replace(int, seqItem) method");
        }

        /// <summary>
        /// Validate ReplaceRange() method.
        /// Input : valid sequence item.
        /// Validation : ReplaceRange() method works as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPReplaceRange()
        {
            FileVirtualSequenceProvider provObj =
                GetVirtualSequenceProvider();

            ISequence seqObj = new Sequence(Alphabets.DNA, "GTTCG");

            provObj.Add(seqObj[0]);
            provObj.Add(seqObj[1]);
            provObj.Add(seqObj[2]);
            provObj.Add(seqObj[3]);
            provObj.ReplaceRange(0, "CT");

            Assert.AreEqual(seqObj[3], provObj[0]);
            Assert.AreEqual(seqObj[2], provObj[1]);

            ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated the ReplaceRange() method");
            Console.WriteLine(@"FVSP Bvt : Successfully validated the ReplaceRange() method");
        }

        /// <summary>
        /// Validate All properties.
        /// Input : update all properties.
        /// Validation : all properties.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFVSPProperties()
        {
            IVirtualSequenceParser parserObj = new FastaParser();
            try
            {
                FileVirtualSequenceProvider provObj =
                    new FileVirtualSequenceProvider(parserObj, GetSequencePointer());
                provObj.BlockSize = 5;
                provObj.IsReadOnly = false;
                provObj.MaxNumberOfBlocks = 10;
                provObj.SequencePointerInstance = GetSequencePointer();
                SequencePointer seqPoint = GetSequencePointer();
                Assert.AreEqual(5, provObj.BlockSize);
                Assert.AreEqual(10, provObj.MaxNumberOfBlocks);
                Assert.IsFalse(provObj.IsReadOnly);
                Assert.AreEqual(seqPoint.AlphabetName, provObj.SequencePointerInstance.AlphabetName);
                Assert.AreEqual(1301, provObj.Count);

                ApplicationLog.WriteLine(@"FVSP Bvt : Successfully validated all the properties");
                Console.WriteLine(@"FVSP Bvt : Successfully validated all the properties");
            }
            finally
            {
                (parserObj as FastaParser).Dispose();
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

        #endregion FileVirtualSequenceProvider TestCases

        #region Supported Methods

        /// <summary>
        /// Gets the VirtualSequenceProvider
        /// </summary>
        /// <returns>Virtual Sequence Provider</returns>
        FileVirtualSequenceProvider GetVirtualSequenceProvider()
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastaDnaNodeName, Constants.FilePathNode);

            IVirtualSequenceParser parserObj = new FastaParser();
            try
            {
                parserObj.Parse(filePath);

                FileVirtualSequenceProvider provObj =
                    new FileVirtualSequenceProvider(parserObj,
                        GetSequencePointer());

                return provObj;
            }
            finally
            {
                (parserObj as FastaParser).Dispose();
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
                "gi|186972394|gb|EU490707.1| Selenipedium aequinoctiale maturase K (matK) gene, partial cds; chloroplast";
            pointerObj.IndexOffsets[0] = 104;
            pointerObj.IndexOffsets[1] = 1405;
            pointerObj.StartingLine = 1;

            return pointerObj;
        }

        #endregion Supported Methods
    }
}
