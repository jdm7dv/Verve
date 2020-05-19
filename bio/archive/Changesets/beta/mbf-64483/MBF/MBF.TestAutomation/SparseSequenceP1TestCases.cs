// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SparseSequenceP1TestCases.cs
 * 
 * This file contains the Sparse Sequence P1 test case validation.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Sparse Sequence P1 level validations
    /// </summary>
    [TestClass]
    public class SparseSequenceP1TestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SparseSequenceP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion

        #region Test Cases

        /// <summary>
        /// Creates sparse sequence object and store all seq items of Dna alphabet.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDnaSparseSequence()
        {
            ValidateSparseSequence(Alphabets.DNA);
        }

        /// <summary>
        /// Creates sparse sequence object and store all seq items of Rna alphabet.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaSparseSequence()
        {
            ValidateSparseSequence(Alphabets.RNA);
        }

        /// <summary>
        /// Creates sparse sequence object and store all seq items at different position of alphabet.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDnaSparseSequenceWithDifferentPosition()
        {
            ValidateSparseSequence(Alphabets.DNA);
        }

        /// <summary>
        /// Creates sparse sequence object using seq item of rna alphabet.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaSparseSequenceWithSeqItem()
        {
            ValidateSparseSequenceWithSeqItem(Alphabets.RNA);
        }

        /// <summary>
        /// Creates sparse sequence object using protein item of protein alphabet.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinSparseSequenceWithSeqItem()
        {
            ValidateSparseSequenceWithSeqItem(Alphabets.Protein);
        }

        /// <summary>
        /// Creates sparse sequence object using seq item list overload for Rna alphabet.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaSparseSequenceWithSeqItemList()
        {
            ValidateSparseSequenceWithSeqItemList(Alphabets.RNA);
        }

        /// <summary>
        /// Creates sparse sequence object using seq item list overload for Protein alphabet.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinSparseSequenceWithSeqItemList()
        {
            ValidateSparseSequenceWithSeqItemList(Alphabets.Protein);
        }

        /// <summary>
        /// Creates sparse sequence object and changes IsReadOnly value to "false".
        /// Adds all sequence items of Rna alphabet and validates if all items are added properly.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaSparseSequenceAdd()
        {
            ValidateSparseSequenceAdd(Alphabets.RNA);
        }

        /// <summary>
        /// Creates sparse sequence object and changes IsReadOnly value to "false".
        /// Adds all sequence items of protein alphabet and validates if all items are added properly.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinSparseSequenceAdd()
        {
            ValidateSparseSequenceAdd(Alphabets.Protein);
        }

        /// <summary>
        /// Creates a sparse sequence and store sequence items of Rna alphabet. 
        /// Deletes all sequence items using Clear() method and 
        /// validates if all items are deleted from sparse sequence object.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaSparseSequenceClear()
        {
            ValidateSparseSequenceClear(Alphabets.RNA);
        }

        /// <summary>
        /// Creates a sparse sequence and store sequence items of Protein alphabet. 
        /// Deletes all sequence items using Clear() method and 
        /// validates if all items are deleted from sparse sequence object.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinSparseSequenceClear()
        {
            ValidateSparseSequenceClear(Alphabets.Protein);
        }

        /// <summary>
        /// Creates a sparse sequence and store sequence items of Rna alphabet. 
        /// Delete all sequence items using Clear() method and then again add sequence items.
        /// Validates if addition and deletion of items happening properly.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaSparseSequenceClearAndAdd()
        {
            ValidateSparseSequenceClearAndAdd(Alphabets.RNA);
        }

        /// <summary>
        /// Creates a sparse sequence and store sequence items of Dna alphabet. 
        /// Validates if all items are present in sparse sequence using Conatins() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDnaSparseSequenceContains()
        {
            ValidateSparseSequenceContains(Alphabets.DNA);
        }

        /// <summary>
        /// Creates a sparse sequence and store sequence items of Rna alphabet. 
        /// Creates empty array and copy all sequence items using CopyTo()
        /// and Validates if all sequence items are copied properly.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaSparseSequenceCopyTo()
        {
            ValidateSparseSequenceCopyTo(Alphabets.RNA);
        }

        /// <summary>
        /// Creates a sparse sequence and store sequence items of Rna alphabet.
        /// Validates all sequence items using GetEnumerator() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaSparseSequenceGetEnumerator()
        {
            ValidateSparseSequenceGetEnumerator(Alphabets.RNA);
        }

        /// <summary>
        /// Creates sparse sequence object and store all seq items of Protein alphabet.
        /// Validates if all items are present in sparse sequence instance using IndexOf().
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinSparseSequenceIndexOf()
        {
            ValidateSparseSequence(Alphabets.Protein);
        }

        /// <summary>
        /// Creates sparse sequence object and insert all seq items of Dna alphabet.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDnaSparseSequenceInsert()
        {
            ValidateSparseSequenceInsert(Alphabets.DNA);
        }

        /// <summary>
        /// Creates sparse sequence object and insert all seq items of Dna alphabet using its symbol.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDnaSparseSequenceInsertWithChar()
        {
            ValidateSparseSequenceInsertWithChar(Alphabets.DNA);
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of Rna alphabet and 
        /// replace the item using another sequence item
        /// Validates if item is replaced.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaSparseSequenceReplaceWithSequenceItem()
        {
            ValidateSparseSequenceReplaceWithSequenceItem(Alphabets.RNA);
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet 
        /// and removes all sequence items.
        /// Validates if items are getting removed.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDnaSparseSequenceRemoveAt()
        {
            ValidateSparseSequenceRemoveAt(Alphabets.DNA);
        }

        /// <summary>
        /// Creates a sparse sequence and store sequence items of alphabet 
        /// and replace with expected sequence using ReplaceRange().
        /// Validates if items are replaced with expected sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDnaSparseSequenceReplaceRange()
        {
            ValidateSparseSequenceReplaceRange(Alphabets.DNA);
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet 
        /// and removes few sequence items.
        /// Validates if expected number of items are removed.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDnaSparseSequenceRemoveRange()
        {
            ValidateSparseSequenceRemoveRange(Alphabets.DNA);
        }

        /// <summary>
        /// Creates a sparse sequence and store sequence items of alphabet 
        /// Get sequence using Range() method and validates it against expected sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDnaSparseSequenceRange()
        {
            ValidateSparseSequenceRange(Alphabets.DNA);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates sparse sequence object and store all seq items of alphabet.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequence(IAlphabet alphabet)
        {
            SparseSequence sparseSeq = new SparseSequence(alphabet);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store all sequence items in sparse sequence object
            int insertIndex = 0;
            int[] randomNumbers = Utility.RandomNumberGenerator(2000, alphabet.Count);
            foreach (ISequenceItem item in alphabet)
            {
                sparseSeq[randomNumbers[insertIndex]] = item;
                insertIndex++;
            }

            // Retrieve and Validate all stored sequence items in sparse sequence
            int retriveIndex = 0;
            foreach (ISequenceItem item in alphabet)
            {
                Assert.IsTrue(sparseSeq.Contains(item));
                Assert.AreEqual(sparseSeq.IndexOf(item), randomNumbers[retriveIndex]);
                Assert.AreEqual(item, sparseSeq[randomNumbers[retriveIndex]]);
                Assert.AreEqual(item.Symbol, sparseSeq[randomNumbers[retriveIndex]].Symbol);
                retriveIndex++;
            }

            Console.WriteLine(
                "SparseSequenceP1: Validation of sprase sequence with alphabet  is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceP1: Validation of sprase sequence with alphabet  is completed");
        }

        /// <summary>
        /// Creates sparse sequence object and insert all seq items of alphabet.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceInsert(IAlphabet alphabet)
        {
            SparseSequence sparseSeq = new SparseSequence(alphabet);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store all sequence items in sparse sequence object
            int insertIndex = 0;
            int[] randomNumbers = Utility.RandomNumberGenerator(2000, alphabet.Count);
            foreach (ISequenceItem item in alphabet)
            {
                sparseSeq.Insert(randomNumbers[insertIndex], item);
                insertIndex++;
            }

            // Retrieve all stored sequence items and validate Sparse Sequence
            foreach (ISequenceItem item in alphabet)
            {
                // Validate new added items
                Assert.IsTrue(sparseSeq.Contains(item));
                int position = sparseSeq.IndexOf(item);
                Assert.AreEqual(item, sparseSeq[position]);
                Assert.AreEqual(item.Symbol, sparseSeq[position].Symbol);
            }

            Console.WriteLine("SparseSequenceP1: Validation of Insert() method  is completed");
            ApplicationLog.WriteLine("SparseSequenceP1: Validation of Insert() method  is completed");
        }

        /// <summary>
        /// Creates sparse sequence object and insert all seq items of alphabet using its symbol.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceInsertWithChar(IAlphabet alphabet)
        {
            SparseSequence sparseSeq = new SparseSequence(alphabet);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store all sequence items in sparse sequence object
            int insertIndex = 0;
            int[] randomNumbers = Utility.RandomNumberGenerator(2000, alphabet.Count);
            foreach (ISequenceItem item in alphabet)
            {
                sparseSeq.Insert(randomNumbers[insertIndex], item.Symbol);
                insertIndex++;
            }

            // Retrieve all stored sequence items and validate Sparse Sequence
            foreach (ISequenceItem item in alphabet)
            {
                // Validate new added items
                Assert.IsTrue(sparseSeq.Contains(item));
                int position = sparseSeq.IndexOf(item);
                Assert.AreEqual(item, sparseSeq[position]);
                Assert.AreEqual(item.Symbol, sparseSeq[position].Symbol);
            }

            Console.WriteLine(
                "SparseSequenceP1: Validation of Insert() method by passing char  is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceP1: Validation of Insert() method by pasing char is completed");
        }

        /// <summary>
        /// Creates sparse sequence object using seq item and index overload.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceWithSeqItem(IAlphabet alphabet)
        {
            int insertIndex = 0;
            int[] randomNumbers = Utility.RandomNumberGenerator(50, alphabet.Count);
            foreach (ISequenceItem item in alphabet)
            {
                // Add sequence item to sparse seq.
                SparseSequence sparseSeq =
                    new SparseSequence(alphabet, randomNumbers[insertIndex], item);

                // Validate newly added item
                Assert.IsTrue(sparseSeq.Contains(item));
                Assert.AreEqual(sparseSeq.IndexOf(item), randomNumbers[insertIndex]);
                Assert.AreEqual(item, sparseSeq[randomNumbers[insertIndex]]);
                Assert.AreEqual(item.Symbol, sparseSeq[randomNumbers[insertIndex]].Symbol);
                insertIndex++;
            }

            Console.WriteLine(
                "SparseSequenceP1: Validation of Sparse Sequence constructor with Sequence item and index  is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceP1: Validation of Sparse Sequence constructor with Sequence item and index is completed");
        }

        /// <summary>
        /// Creates sparse sequence object using seq item list overload.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceWithSeqItemList(IAlphabet alphabet)
        {
            SparseSequence sparseSeq = CreateSparseSequence(alphabet, 20);

            // Retrieve all stored sequence items and validate Sparse Sequence
            int retrieveIndex = 20;
            foreach (ISequenceItem item in alphabet)
            {
                // Validate new added items
                Assert.IsTrue(sparseSeq.Contains(item));
                Assert.AreEqual(sparseSeq.IndexOf(item), retrieveIndex);
                Assert.AreEqual(item, sparseSeq[retrieveIndex]);
                Assert.AreEqual(item.Symbol, sparseSeq[retrieveIndex].Symbol);
                retrieveIndex++;
            }

            Console.WriteLine(
                "SparseSequenceP1: Validation of Sparse Sequence constructor with Sequence item list is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceP1: Validation of Sparse Sequence constructor with Sequence item list is completed");

        }

        /// <summary>
        /// Creates sparse sequence object and changes IsReadOnly value to "false".
        /// Adds all sequence items of alphabet and validates if all items are added properly.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceAdd(IAlphabet alphabet)
        {
            SparseSequence sparseSeq = new SparseSequence(alphabet);
            sparseSeq.IsReadOnly = false;

            // Add all sequence items
            foreach (ISequenceItem item in alphabet)
            {
                sparseSeq.Add(item);
            }

            // Validate if all items are added properly.
            Assert.AreEqual(alphabet.Count, sparseSeq.Count);
            int index = 0;
            foreach (ISequenceItem item in alphabet)
            {
                Assert.AreEqual(item, sparseSeq[index]);
                Assert.AreEqual(item.Symbol, sparseSeq[index].Symbol);
                index++;
            }

            Console.WriteLine("SparseSequenceP1: Validation of Add() method is completed");
            ApplicationLog.WriteLine("SparseSequenceP1: Validation of Add() method is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet. 
        /// Delete all sequence items using Clear() method and 
        /// validates if all items are deleted from sparse sequence object.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceClear(IAlphabet alphabet)
        {
            SparseSequence sparseSeq = CreateSparseSequence(alphabet, 10);
            sparseSeq.IsReadOnly = false;

            // Validate if sparse sequence conatins all sequence items.
            Assert.AreEqual(alphabet.Count + 10, sparseSeq.Count);

            // Clear the sparse sequence.
            sparseSeq.Clear();

            // Validate if all sequence items are deleted.
            Assert.AreEqual(0, sparseSeq.Count);

            Console.WriteLine("SparseSequenceP1: Validation of Clear() method is completed");
            ApplicationLog.WriteLine("SparseSequenceP1: Validation of Clear() method is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet. 
        /// Delete all sequence items using Clear() method and again add sequence items.
        /// Validates Clear() and Add() is happening properly.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceClearAndAdd(IAlphabet alphabet)
        {
            SparseSequence sparseSeq = CreateSparseSequence(alphabet, 10);
            sparseSeq.IsReadOnly = false;

            // Validate if sparse sequence conatins all sequence items.
            Assert.AreEqual(alphabet.Count + 10, sparseSeq.Count);

            // Clear the sparse sequence.
            sparseSeq.Clear();

            // Validate if all sequence items are deleted.
            Assert.AreEqual(0, sparseSeq.Count);

            // Again add sequence items and validate if items are getting added.
            foreach (ISequenceItem item in alphabet)
            {
                sparseSeq.Add(item);
            }

            // Validate if all items are added back
            Assert.AreEqual(alphabet.Count, sparseSeq.Count);

            Console.WriteLine(
                "SparseSequenceP1: Validation of Clear() method with Add() is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceP1: Validation of Clear() method with Add() is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and store sequence items of alphabet. 
        /// Validates if all items are present using Contains() method.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceContains(IAlphabet alphabet)
        {
            SparseSequence sparseSequence = CreateSparseSequence(alphabet, 50);

            // Validate if items are present in sparse sequence using Contains() method
            foreach (ISequenceItem item in alphabet)
            {
                Assert.IsTrue(sparseSequence.Contains(item));
            }

            Console.WriteLine("SparseSequenceP1: Validation of Contains() is completed");
            ApplicationLog.WriteLine("SparseSequenceP1: Validation of Contains() is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and store sequence items of alphabet. 
        /// Creates empty array and copy all sequence items using CopyTo()
        /// and Validates if all sequence items are copied properly.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceCopyTo(IAlphabet alphabet)
        {

            SparseSequence sparseSequence = CreateSparseSequence(alphabet, 10);
            sparseSequence.IsReadOnly = false;

            // Copy sequence items to empty array
            ISequenceItem[] sequenceItems = new ISequenceItem[alphabet.Count + 10];
            sparseSequence.CopyTo(sequenceItems, 0);

            // Validate the copied array.
            int retrieveIndex = 10;
            foreach (ISequenceItem item in alphabet)
            {
                Assert.AreEqual(item, sequenceItems[retrieveIndex]);
                Assert.AreEqual(item.Symbol, sequenceItems[retrieveIndex].Symbol);
                retrieveIndex++;
            }

            Console.WriteLine("SparseSequenceP1: Validation of CopyTo() method is completed");
            ApplicationLog.WriteLine("SparseSequenceP1: Validation of CopyTo() method is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and store sequence items of alphabet. 
        /// Gets sequence items list using GetEnumerator() method and validates 
        /// sequence item list
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceGetEnumerator(IAlphabet alphabet)
        {
            SparseSequence sparseSequence = CreateSparseSequence(alphabet, 0);
            IEnumerator<ISequenceItem> list = sparseSequence.GetEnumerator();

            foreach (ISequenceItem item in alphabet)
            {
                list.MoveNext();
                Assert.AreEqual(list.Current, item);
            }

            Console.WriteLine("SparseSequenceP1: Validation of GetEnumerator() method is completed");
            ApplicationLog.WriteLine("SparseSequenceP1: Validation of GetEnumerator() method is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items at even position of alphabet 
        /// and replaces with sequence item present at odd position.
        /// Validates if item is replaced.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceReplaceWithSequenceItem(IAlphabet alphabet)
        {
            // Create sequence item array
            ISequenceItem[] sequenceItemArray = new ISequenceItem[alphabet.Count];
            int index = 0;
            foreach (ISequenceItem item in alphabet)
            {
                sequenceItemArray[index] = item;
                index++;
            }

            /// Validate the Replace method with sequence item.
            /// Add even sequence items from sequence item array at random position
            /// and replace with odd sequence item from array.
            int replaceIndex = 1;
            int[] randomNumbers = Utility.RandomNumberGenerator(1502, alphabet.Count);
            index = 0;
            for (int addIndex = 0; addIndex < alphabet.Count; addIndex = addIndex + 2)
            {
                SparseSequence sparseSeq =
                    new SparseSequence(alphabet, randomNumbers[index], sequenceItemArray[addIndex]);
                sparseSeq.IsReadOnly = false;
                sparseSeq.Replace(randomNumbers[index], sequenceItemArray[replaceIndex]);

                // Validate if item is replaced as expected.
                Assert.AreEqual(sparseSeq[randomNumbers[index]], sequenceItemArray[replaceIndex]);

                replaceIndex = replaceIndex + 2;
                index++;
            }

            Console.WriteLine(
                "SparseSequenceP1: Validation of Relpace() method with sequence item is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceP1: Validation of Relpace() method with sequence item is completed");

        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items at even position of alphabet 
        /// and replaces with sequence string present at odd position.
        /// Validates if items are replaced as expected.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceReplaceRange(IAlphabet alphabet)
        {
            // Create sequence item array
            ISequenceItem[] sequenceItemArray = new ISequenceItem[alphabet.Count];
            int index = 0;
            foreach (ISequenceItem item in alphabet)
            {
                sequenceItemArray[index] = item;
                index++;
            }

            // create list of sequence items at even position.
            List<ISequenceItem> lstAddSequenceItem = new List<ISequenceItem>();
            for (int addIndex = 0; addIndex < alphabet.Count; addIndex = addIndex + 2)
            {
                lstAddSequenceItem.Add(sequenceItemArray[addIndex]);
            }

            //Create sequence using sequence items at odd position
            string sequence = string.Empty;
            List<ISequenceItem> lstNewSequenceItem = new List<ISequenceItem>();
            for (int relpaceIndex = 1; relpaceIndex < alphabet.Count; relpaceIndex = relpaceIndex + 2)
            {
                sequence += sequenceItemArray[relpaceIndex].Symbol.ToString((IFormatProvider)null);
                lstNewSequenceItem.Add(sequenceItemArray[relpaceIndex]);
            }

            // Create sparse sequence
            SparseSequence sparseSequence = new SparseSequence(alphabet, 8, lstAddSequenceItem);
            Assert.AreEqual(lstAddSequenceItem.Count + 8, sparseSequence.Count);

            // Replace Range and Validate if sparse sequence items are replaced.
            sparseSequence.IsReadOnly = false;
            sparseSequence.ReplaceRange(8, sequence);
            Assert.AreEqual(lstNewSequenceItem.Count + 8, sparseSequence.Count);
            foreach (ISequenceItem item in lstNewSequenceItem)
            {
                Assert.IsTrue(sparseSequence.Contains(item));
            }

            Console.WriteLine(
                "SparseSequenceP1: Validation of RelpaceRange() method with sequence item is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceP1: Validation of RelpaceRange() method with sequence item is completed");

        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet 
        /// and removes all sequence items.
        /// Validates if items are getting removed.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceRemoveAt(IAlphabet alphabet)
        {
            SparseSequence sparseSequence = CreateSparseSequence(alphabet, 0);
            sparseSequence.IsReadOnly = false;

            // Remove all sequence items
            Assert.AreEqual(alphabet.Count, sparseSequence.Count);
            int removalCount = 0;
            while (sparseSequence.Count > 0)
            {
                // Remove last item as last item will always be reset with previous item.
                sparseSequence.RemoveAt(sparseSequence.Count - 1);
                removalCount++;
            }

            // Validate if all items are removed in each iteration.
            Assert.AreEqual(0, sparseSequence.Count);
            Assert.AreEqual(alphabet.Count, removalCount);

            Console.WriteLine(
                "SparseSequenceP1: Validation of RemoveAt() method by passing index is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceP1: Validation of RemoveAt() method by passing index is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet 
        /// and removes few sequence items using RemoveRange()
        /// Validates ifexpected number of items are removed.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceRemoveRange(IAlphabet alphabet)
        {
            SparseSequence sparseSequence = CreateSparseSequence(alphabet, 10);
            sparseSequence.IsReadOnly = false;

            // Remove all sequence items
            Assert.AreEqual(alphabet.Count + 10, sparseSequence.Count);

            sparseSequence.RemoveRange(10, 10);

            // Validate if 10 items are removed using RemoveRange
            Assert.AreEqual(alphabet.Count, sparseSequence.Count);

            Console.WriteLine(
                "SparseSequenceP1: Validation of RemoveRange() method by passing position and length is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceP1: Validation of RemoveRange() method by passing position and length is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and store sequence items of alphabet 
        /// Gets sequence using Range() method and validates it against expected sequence.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private static void ValidateSparseSequenceRange(IAlphabet alphabet)
        {
            SparseSequence sparseSequence = CreateSparseSequence(alphabet, 10);
            sparseSequence.IsReadOnly = false;

            ISequence sequence = sparseSequence.Range(10, 10);

            // Create expected sequence using only 10 elements.
            string expectedsequence = string.Empty;
            int index = 0;
            foreach (ISequenceItem item in alphabet)
            {
                expectedsequence += item.Symbol.ToString((IFormatProvider)null);
                index++;

                if (index >= 10)
                {
                    break;
                }
            }

            Assert.AreEqual(expectedsequence, sequence.ToString());

            Console.WriteLine(
                "SparseSequenceP1: Validation of Range() method by passing position and length is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceP1: Validation of Range() method by passing position and length is completed");
        }

        /// <summary>
        ///  Create sparse sequence and insert all sequence items of alphabet.
        /// </summary>
        /// <param name="alphabet">Alphabet</param>
        /// <param name="insertPosition">Position to be inserted</param>
        /// <returns>Sparse sequence</returns>
        private static SparseSequence CreateSparseSequence(IAlphabet alphabet, int insertPosition)
        {
            // Create sequence item list
            List<ISequenceItem> sequenceList = new List<ISequenceItem>();
            foreach (ISequenceItem item in alphabet)
            {
                sequenceList.Add(item);
            }

            // Store sequence item in sparse sequence object using list of sequence items
            SparseSequence sparseSeq = new SparseSequence(alphabet, insertPosition, sequenceList);

            return sparseSeq;
        }

        #endregion
    }
}
