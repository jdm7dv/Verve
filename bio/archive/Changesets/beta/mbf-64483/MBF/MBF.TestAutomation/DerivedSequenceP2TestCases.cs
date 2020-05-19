// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * DerivedSequenceP2TestCases.cs
 * 
 * This file contains the Derived Sequence P2 test case validation.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Derived Sequences and P2 validations.
    /// </summary>
    [TestClass]
    public class DerivedSequenceP2TestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static DerivedSequenceP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Test Cases

        /// <summary>
        /// Validate null for Add() method.
        /// Input data : Null value.
        /// Output Data : Validate the exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedAddNull()
        {
            ISequence seqObj = new Sequence(Alphabets.DNA, "AGCT");
            DerivedSequence deriSeq = new DerivedSequence(seqObj);

            // Pass null value and get the exception required
            try
            {
                deriSeq.Add(null);
                Assert.IsTrue(false);
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the null exception");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the null exception");
            }
        }

        /// <summary>
        /// Validate invalid sequence item for Add() method.
        /// Input data : Invalid sequence item.
        /// Output Data : Validate the exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedAddInvalidSeqItem()
        {
            ISequence seqObj = new Sequence(Alphabets.DNA, "AGCT");
            DerivedSequence deriSeq = new DerivedSequence(seqObj);
            ISequence proObj = new Sequence(Alphabets.Protein, "EEE");

            // Insert invalid item and get Argument exception
            try
            {
                deriSeq.Add(proObj[0]);
                Assert.IsTrue(false);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the argument exception");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the argument exception");
            }
        }

        /// <summary>
        /// Validate valid protein sequence item for Clear() method.
        /// Input data : valid sequence item.
        /// Output Data : Validate the clear method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedClearSeqItem()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.Protein, "EEE");
            ISequence tempObj = new Sequence(Alphabets.Protein, "KIE");
            ISequenceItem seqItemObj = tempObj[0];

            DerivedSequence deriSeq = new DerivedSequence(seqObj);
            // Add a sequence item to the derived sequence object
            deriSeq.Add(seqItemObj);
            // Validate if the Derived sequence is updated
            Assert.AreEqual(string.Concat(seqObj.ToString(), "K"), deriSeq.ToString());
            deriSeq.Clear();
            // Validate after clearing
            Assert.AreEqual(seqObj.ToString(), deriSeq.ToString());
            ApplicationLog.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the Clear method");
            Console.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the Clear method");
        }

        /// <summary>
        /// Validate invalid protein sequence item for Contains() method.
        /// Input data : invalid sequence item.
        /// Output Data : Validate the result for Contains() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedContainsInvalidSeqItem()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.Protein, "EEE");
            ISequence tempObj = new Sequence(Alphabets.Protein, "KIE");
            ISequenceItem seqItemObj = tempObj[0];

            DerivedSequence deriSeq = new DerivedSequence(seqObj);
            // Validate in valid sequence item in the derived sequence object
            Assert.IsFalse(deriSeq.Contains(seqItemObj));
            ApplicationLog.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the Contains method for invalid sequence item");
            Console.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the Contains method for invalid sequence item");
        }

        /// <summary>
        /// Validate null sequence item for Contains() method.
        /// Input data : null sequence item.
        /// Output Data : Validate the expection for Contains() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedContainsNullSeqItem()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.Protein, "EEE");
            DerivedSequence deriSeq = new DerivedSequence(seqObj);

            // Validate null sequence item in the derived sequence object
            Assert.IsFalse(deriSeq.Contains(null));
            ApplicationLog.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the Contains method for null sequence item");
            Console.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the Contains method for null sequence item");
        }

        /// <summary>
        /// Validate dna sequence with rna sequence item for Contains() method.
        /// Input data : dna sequence with rna sequence item .
        /// Output Data : Validate the result for Contains() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedContainsDnaRnaSeqItem()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.DNA, "GCT");
            ISequence tempObj = new Sequence(Alphabets.RNA, "UUU");
            ISequenceItem seqItemObj = tempObj[0];

            DerivedSequence deriSeq = new DerivedSequence(seqObj);
            // Validate in valid sequence item in the derived sequence object
            Assert.IsFalse(deriSeq.Contains(seqItemObj));
            ApplicationLog.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the Contains method for Rna sequence item");
            Console.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the Contains method for Rna sequence item");
        }

        /// <summary>
        /// Validate null sequence item for CopyTo() method.
        /// Input data : null sequence item.
        /// Output Data : Validate the expection for CopyTo() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedNullCopyTo()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.Protein, "EEE");
            DerivedSequence deriSeq = new DerivedSequence(seqObj);

            // Pass null value and get the exception required
            try
            {
                deriSeq.CopyTo(null, 0);
                Assert.IsTrue(false);
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the null exception for CopyTo() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the null exception for CopyTo() method");
            }
        }

        /// <summary>
        /// Validate sequence item for CopyTo() method with invalid index.
        /// Input data : sequence item with invalid index.
        /// Output Data : Validate the expection for CopyTo() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedInvalidIndexCopyTo()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.Protein, "EEE");
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);

            ISequence tempSeqObj = new Sequence(Alphabets.Protein, "KIE");
            ISequenceItem[] seqItemObj = tempSeqObj.ToArray();

            // Pass invalid index value and get the exception required
            try
            {
                deriSeqObj.CopyTo(seqItemObj, -1);
                Assert.IsTrue(false);
            }
            catch (IndexOutOfRangeException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the index out of range exception for CopyTo() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the index out of range exception for CopyTo() method");
            }
        }

        /// <summary>
        /// Validate sequence item for IndexOf() method with valid values.
        /// Input data : sequence item with valid index.
        /// Output Data : Validate IndexOf() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedIndexOfValid()
        {
            Sequence seqObj = new Sequence(Alphabets.Protein, "KIE");
            ISequenceItem seqItemObj = seqObj[1];

            // Valid index of validation
            DerivedSequence deriSeq = new DerivedSequence(seqObj);
            Assert.AreEqual(1, deriSeq.IndexOf(seqItemObj));
            ApplicationLog.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated IndexOf() method with valid values");
            Console.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the IndexOf() method with valid values");
        }

        /// <summary>
        /// Validate sequence item for IndexOf() method with null values.
        /// Input data : sequence item with null index.
        /// Output Data : Validate IndexOf() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedIndexOfNull()
        {
            Sequence seqObj = new Sequence(Alphabets.Protein, "KIE");

            // Valid index of validation
            DerivedSequence deriSeq = new DerivedSequence(seqObj);
            Assert.AreEqual(-1, deriSeq.IndexOf(null));
            ApplicationLog.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated IndexOf() method with null values");
            Console.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the IndexOf() method with null values");
        }

        /// <summary>
        /// Validate sequence item for Insert() method with invalid index.
        /// Input data : sequence item with invalid index.
        /// Output Data : Validate the expection for Insert() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedInvalidIndexInsert()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.DNA, "AAGGTT");
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);

            // Pass invalid index value and get the exception required
            try
            {
                deriSeqObj.Insert(-1, 'C');
                Assert.IsTrue(false);
            }
            catch (ArgumentOutOfRangeException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the index out of range exception for Insert() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the index out of range exception for Insert() method");
            }
        }

        /// <summary>
        /// Validate sequence item for Insert() method with null value.
        /// Input data : sequence item with null value.
        /// Output Data : Validate the expection for Insert() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedNullSeqItemInsert()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.DNA, "AAGGTT");
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);

            // Pass invalid index value and get the exception required
            try
            {
                deriSeqObj.Insert(0, null);
                Assert.IsTrue(false);
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Insert() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Insert() method");
            }
        }

        /// <summary>
        /// Validate sequence item for Insert() method with invalid index.
        /// Input data : sequence item with invalid index.
        /// Output Data : Validate the expection for Insert() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedInvalidIndexSeqItemInsert()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.DNA, "AAGGTT");
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);
            ISequenceItem seqItemObj = seqObj[0];
            // Pass invalid index value and get the exception required
            try
            {
                deriSeqObj.Insert(-1, seqItemObj);
                Assert.IsTrue(false);
            }
            catch (ArgumentOutOfRangeException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the index out of range exception for Insert() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the index out of range exception for Insert() method");
            }
        }

        /// <summary>
        /// Validate character for Insert() method with null value.
        /// Input data : Char with null value.
        /// Output Data : Validate the expection for Insert() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedNullCharInsert()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.DNA, "AAGGTT");
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);

            // Pass invalid index value and get the exception required
            try
            {
                deriSeqObj.Insert(0, null);
                Assert.IsTrue(false);
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Insert() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Insert() method");
            }
        }

        /// <summary>
        /// Validate Range() method with invalid value.
        /// Input data : invalid range value.
        /// Output Data : Validate the expection for Range() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedSeqInvalidRange()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.DNA, "AAGGTT");
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);

            // Pass invalid index value and get the exception required
            try
            {
                deriSeqObj.Range(-1, -1);
                Assert.IsTrue(false);
            }
            catch (ArgumentOutOfRangeException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Range() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Range() method");
            }

            // Pass length value lesser than zero.
            try
            {
                deriSeqObj.Range(1, -21);
                Assert.IsTrue(false);
            }
            catch (ArgumentOutOfRangeException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Range() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Range() method");
            }

            // Pass invalid length and startPos value.
            try
            {
                deriSeqObj.Range(2, 6);
                Assert.IsTrue(false);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Range() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Range() method");
            }
        }

        /// <summary>
        /// Validate RemoveAt() method with invalid value.
        /// Input data : invalid range value.
        /// Output Data : Validate the expection for RemoveAt() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedInvalidRemoveAt()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.DNA, "AAGGTT");
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);

            // Pass invalid index value and get the exception required
            try
            {
                deriSeqObj.RemoveAt(-1);
                Assert.IsTrue(false);
            }
            catch (ArgumentOutOfRangeException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for RemoveAt() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for RemoveAt() method");
            }
        }

        /// <summary>
        /// Validate Remove() method with invalid value.
        /// Input data : invalid range value.
        /// Output Data : Validate the expection for Remove() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedInvalidRemove()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.DNA, "AAGGTT");
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);

            ISequence tempSeq = new Sequence(Alphabets.DNA, "C");

            Assert.IsFalse(deriSeqObj.Remove(tempSeq[0]));

            ApplicationLog.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the exception for RemoveAt() method");
            Console.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the exception for RemoveAt() method");

        }

        /// <summary>
        /// Validate RemoveRange() method with invalid value.
        /// Input data : invalid RemoveRange value.
        /// Output Data : Validate the expection for RemoveRange() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedInvalidRemoveRange()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.DNA, "AAGGTT");
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);

            // Pass invalid index value and get the exception required
            try
            {
                deriSeqObj.RemoveRange(-1, 0);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for RemoveRange() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for RemoveRange() method");
            }

            // Pass invalid length value and get the exception required
            try
            {
                deriSeqObj.RemoveRange(3, -1);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for RemoveRange() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for RemoveRange() method");
            }

            // Pass invalid length, index value and get the exception required
            try
            {
                deriSeqObj.RemoveRange(1, 10);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for RemoveRange() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for RemoveRange() method");
            }
        }

        /// <summary>
        /// Validate Replace() method with invalid value.
        /// Input data : invalid Replace value.
        /// Output Data : Validate the expection for Replace() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedInvalidReplaceSequence()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.DNA, "AAGGTT");
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);

            // Pass invalid index value and get the exception required
            try
            {
                deriSeqObj.Replace(-1, 'E');
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Replace() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Replace() method");
            }

            // Pass invalid Sequence Item value and get the exception required
            try
            {
                deriSeqObj.Replace(0, 'E');
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Replace() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for Replace() method");
            }
        }

        /// <summary>
        /// Validate ReplaceRange() method with invalid value.
        /// Input data : invalid ReplaceRange value.
        /// Output Data : Validate the expection for ReplaceRange() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedInvalidReplaceRangeSequence()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.DNA, "AAGGTT");
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);

            // Pass invalid index value and get the exception required
            try
            {
                deriSeqObj.ReplaceRange(-1, "E");
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for ReplaceRange() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for ReplaceRange() method");
            }

            // Pass invalid sequence and get the exception required
            try
            {
                deriSeqObj.ReplaceRange(1, null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for ReplaceRange() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for ReplaceRange() method");
            }

            // Pass invalid length value and get the exception required
            try
            {
                deriSeqObj.ReplaceRange(1, "AAAGGGTTTT");
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for ReplaceRange() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for ReplaceRange() method");
            }
        }

        /// <summary>
        /// Validate Remove() after adding a particular sequence item to derived sequence 
        /// and validate the GetUpdatedItems() method.
        /// Input data : Valid Dna Sequence.
        /// Output Data : Validate the updatedItems are zero.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedValidSequenceRemoveGetUpdatedItems()
        {
            // Create sequence item and add the same to the derived sequence and validate on removing
            ISequence seqObj = new Sequence(Alphabets.DNA, "ACTCT");
            ISequence tempObj = new Sequence(Alphabets.DNA, "GGG");
            ISequenceItem seqItemObj = tempObj[0];
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);
            deriSeqObj.Add(seqItemObj);
            deriSeqObj.Remove(seqItemObj);
            IList<IndexedItem<UpdatedSequenceItem>> updateItems = deriSeqObj.GetUpdatedItems();
            Assert.AreEqual(0, updateItems.Count);
            ApplicationLog.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the GetUpdatedItems() method to be zero after removal");
            Console.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the GetUpdatedItems() method to be zero after removal");
        }

        /// <summary>
        /// Validate RemoveRange() after adding a particular sequence items to derived sequence 
        /// and validate the GetUpdatedItems() method.
        /// Input data : Valid Dna Sequence.
        /// Output Data : Validate the updatedItems are zero.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedValidSequenceRemoveRangeGetUpdatedItems()
        {
            // Create sequence item and add the same to the derived sequence and validate on removing
            ISequence seqObj = new Sequence(Alphabets.DNA, "ACTCT");
            ISequence tempObj = new Sequence(Alphabets.DNA, "GGG");
            ISequenceItem seqItemObj = tempObj[0];
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);
            deriSeqObj.Add(seqItemObj);
            seqItemObj = tempObj[1];
            deriSeqObj.Add(seqItemObj);
            seqItemObj = tempObj[2];
            deriSeqObj.Add(seqItemObj);
            deriSeqObj.RemoveRange(5, 3);
            IList<IndexedItem<UpdatedSequenceItem>> updateItems = deriSeqObj.GetUpdatedItems();
            Assert.AreEqual(0, updateItems.Count);
            ApplicationLog.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the GetUpdatedItems() method to be zero after remove range");
            Console.WriteLine(
                "DerivedSequenceP2TestCases : Successfully validated the GetUpdatedItems() method to be zero after remove range");
        }

        /// <summary>
        /// Validate null for Constructor.
        /// Input data : Null value.
        /// Output Data : Validate the exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateDerivedInvalidConstructor()
        {
            // Pass null value and get the exception required
            try
            {
                DerivedSequence deriSeq = new DerivedSequence(null);
                Assert.IsNotNull(deriSeq);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the null exception");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the null exception");
            }
        }

        /// <summary>
        /// Validate Replace() method with invalid values.
        /// Input data : Invalid ReplaceRange values.
        /// Output Data : Validate the expection for Replace() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InValidateReplaceDerivedSequenceItems()
        {
            // Pass protein sequences
            ISequence seqObj = new Sequence(Alphabets.DNA, "AAGGTT");
            DerivedSequence deriSeqObj = new DerivedSequence(seqObj);

            // Pass invalid index value and get the exception required
            try
            {
                deriSeqObj.Replace(-1, deriSeqObj[0]);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for ReplaceRange() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for ReplaceRange() method");
            }

            // Pass invalid sequence and get the exception required
            try
            {
                deriSeqObj.Replace(1, null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for ReplaceRange() method");
                Console.WriteLine(
                    "DerivedSequenceP2TestCases : Successfully validated the exception for ReplaceRange() method");
            }
        }
        #endregion Test Cases
    }
}
