// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SparseSequenceBVTTestCases.cs
 * 
 * This file contains the Sparse Sequence BVT test case validation.
 * 
******************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Sparse Sequence BVT level validations
    /// </summary>
    [TestFixture]
    public class SparseSequenceBvtTestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SparseSequenceBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region Sparse Sequence BVT Test Cases

        /// <summary>
        /// Creates sparse sequence object and insert all seq items of alphabet.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        [Test]
        public void ValidateDnaSparseSequence()
        {
            ValidateSparseSequence(Alphabets.DNA);
        }

        /// <summary>
        /// Creates sparse sequence object using seq item and index overload.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        [Test]
        public void ValidateDnaSparseSequenceWithSeqItem()
        {
            ValidateSparseSequenceWithSeqItem(Alphabets.DNA);
        }

        /// <summary>
        /// Creates sparse sequence object using seq item list overload.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        [Test]
        public void ValidateDnaSparseSequenceWithSeqItemList()
        {
            ValidateSparseSequenceWithSeqItemList(Alphabets.DNA);
        }

        /// <summary>
        /// Creates sparse sequence object and changes IsReadOnly value to "false".
        /// Adds all sequence items of dna alphabet and validates if all items are added properly.
        /// </summary>
        [Test]
        public void ValidateDnaSparseSequenceAdd()
        {
            ValidateSparseSequenceAdd(Alphabets.DNA);
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of dna alphabet. 
        /// Delete all sequence items using Clear() method and 
        /// validates if all items are deleted from sparse sequence object.
        /// </summary>
        [Test]
        public void ValidateDnaSparseSequenceClear()
        {
            ValidateSparseSequenceClear(Alphabets.DNA);
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet. 
        /// Creates copy of sparse sequence object using clone() method.
        /// and Validates if copy of sparse sequence is as expected.
        /// </summary>
        [Test]
        public void ValidateDnaSparseSequenceCopyTo()
        {
            ValidateSparseSequenceCopyTo(Alphabets.DNA);
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet. 
        /// Creates copy of sparse sequence object using clone() method.
        /// and Validates if copy of sparse sequence is as expected.
        /// </summary>
        [Test]
        public void ValidateDnaSparseSequenceCopy()
        {
            ValidateSparseSequenceCopy(Alphabets.DNA);
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet 
        /// and removes all sequence items.
        /// Validates if items are getting removed.
        /// </summary>
        [Test]
        public void ValidateDnaSparseSequenceRemove()
        {
            ValidateSparseSequenceRemove(Alphabets.DNA);
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items and 
        /// replace the item using another sequence item symbol
        /// Validates if item is replaced.
        /// </summary>
        [Test]
        public void ValidateDnaSparseSequenceReplaceWithChar()
        {
            ValidateSparseSequenceReplaceWithChar(Alphabets.DNA);
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items and 
        /// replace the item using another sequence item
        /// Validates if item is replaced.
        /// </summary>
        [Test]
        public void ValidateDnaSparseSequenceReplaceWithSequenceItem()
        {
            ValidateSparseSequenceReplaceWithSequenceItem(Alphabets.DNA);
        }

        /// <summary>
        /// Creates a sparse a sequence and inserts all sequence items of alphabet. 
        /// Validates various properties present in the sparse class.
        /// </summary>
        [Test]
        public void ValidateDnaSparseSequenceProperties()
        {
            ValidateSparseSequenceAllProperties(Alphabets.DNA, Constants.DnaSparseSequenceNode);
        }

        /// <summary>
        /// Creates a sparse sequence and validates IndexOfNotNull method
        /// </summary>
        [Test]
        public void ValidateSparseSequenceIndexOfNotNull()
        {
            SparseSequence sparseSeqObj = new SparseSequence(Alphabets.DNA);
            sparseSeqObj.Count = 1000;
            sparseSeqObj[9] = null;
            sparseSeqObj[10] = Alphabets.DNA.A;
            sparseSeqObj[20] = Alphabets.DNA.GA;
            sparseSeqObj[501] = Alphabets.DNA.ACT;
            sparseSeqObj[905] = Alphabets.DNA.Gap;
            sparseSeqObj[906] = null;

            Assert.AreEqual(10, sparseSeqObj.IndexOfNotNull());
            Assert.AreEqual(20, sparseSeqObj.IndexOfNotNull(10));
            Assert.AreEqual(10, sparseSeqObj.IndexOfNotNull(9));
            Assert.AreEqual(10, sparseSeqObj.IndexOfNotNull(0));
            Assert.AreEqual(905, sparseSeqObj.IndexOfNotNull(501));
            Assert.AreEqual(-1, sparseSeqObj.IndexOfNotNull(sparseSeqObj.Count));

            sparseSeqObj = new SparseSequence(Alphabets.DNA);
            sparseSeqObj.Count = 1000;
            Assert.AreEqual(-1, sparseSeqObj.IndexOfNotNull());
            Assert.AreEqual(-1, sparseSeqObj.IndexOfNotNull(10));
            Assert.AreEqual(-1, sparseSeqObj.IndexOfNotNull(sparseSeqObj.Count));

            Console.WriteLine(
                "SparseSequenceBVT: Validation of IndexOfNotNull() method successfully completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT: Validation of IndexOfNotNull() method successfully completed");
        }

        /// <summary>
        /// Creates a sparse sequence and validates LastIndexOfNotNull method
        /// </summary>
        [Test]
        public void ValidateSparseSequenceLastIndexOfNotNull()
        {
            SparseSequence sparseSeqObj = new SparseSequence(Alphabets.DNA);
            sparseSeqObj.Count = 1000;
            sparseSeqObj[9] = null;
            sparseSeqObj[10] = Alphabets.DNA.A;
            sparseSeqObj[20] = Alphabets.DNA.GA;
            sparseSeqObj[501] = Alphabets.DNA.ACT;
            sparseSeqObj[905] = Alphabets.DNA.Gap;
            sparseSeqObj[906] = null;

            Assert.AreEqual(905, sparseSeqObj.LastIndexOfNotNull());
            Assert.AreEqual(501, sparseSeqObj.LastIndexOfNotNull(905));
            Assert.AreEqual(905, sparseSeqObj.LastIndexOfNotNull(906));
            Assert.AreEqual(905, sparseSeqObj.LastIndexOfNotNull(sparseSeqObj.Count));
            Assert.AreEqual(10, sparseSeqObj.LastIndexOfNotNull(20));
            Assert.AreEqual(-1, sparseSeqObj.LastIndexOfNotNull(10));
            Assert.AreEqual(-1, sparseSeqObj.LastIndexOfNotNull(0));

            sparseSeqObj = new SparseSequence(Alphabets.DNA);
            sparseSeqObj.Count = 1000;
            Assert.AreEqual(-1, sparseSeqObj.LastIndexOfNotNull());
            Assert.AreEqual(-1, sparseSeqObj.LastIndexOfNotNull(300));
            Assert.AreEqual(-1, sparseSeqObj.LastIndexOfNotNull(sparseSeqObj.Count));
            Assert.AreEqual(-1, sparseSeqObj.LastIndexOfNotNull(0));
            Console.WriteLine(
                "SparseSequenceBVT: Validation of LastIndexOfNotNull() method successfully completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT: Validation of LastIndexOfNotNull() method successfully completed");
        }

        /// <summary>
        /// Creates a sparse sequence and validates IndexOfNonGap method
        /// </summary>
        [Test]
        public void ValidateSparseSequenceIndexOfNonGap()
        {
            SparseSequence sparseSeqObj = new SparseSequence(Alphabets.DNA);
            sparseSeqObj.Count = 1000;
            sparseSeqObj[9] = null;
            sparseSeqObj[10] = Alphabets.DNA.Gap;
            sparseSeqObj[20] = Alphabets.DNA.GA;
            sparseSeqObj[501] = Alphabets.DNA.A;
            sparseSeqObj[905] = Alphabets.DNA.Gap;
            sparseSeqObj[906] = null;
            Assert.AreEqual(20, sparseSeqObj.IndexOfNonGap());
            Assert.AreEqual(20, sparseSeqObj.IndexOfNonGap(10));
            Assert.AreEqual(20, sparseSeqObj.IndexOfNonGap(20));

            Console.WriteLine(
                "SparseSequenceBVT: Validation of IndexOfNonGap() method successfully completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT: Validation of IndexOfNonGap() method successfully completed");
        }

        /// <summary>
        /// Creates a sparse sequence and validates LastIndexOfNonGap method
        /// </summary>
        [Test]
        public void ValidateSparseSequenceLastIndexOfNonGap()
        {
            SparseSequence sparseSeqObj = new SparseSequence(Alphabets.DNA);
            sparseSeqObj.Count = 1000;
            sparseSeqObj[9] = null;
            sparseSeqObj[10] = Alphabets.DNA.ACT;
            sparseSeqObj[20] = Alphabets.DNA.GA;
            sparseSeqObj[501] = Alphabets.DNA.A;
            sparseSeqObj[905] = Alphabets.DNA.Gap;
            sparseSeqObj[906] = null;
            Assert.AreEqual(501, sparseSeqObj.LastIndexOfNonGap());
            Assert.AreEqual(501, sparseSeqObj.LastIndexOfNonGap(910));
            Assert.AreEqual(501, sparseSeqObj.LastIndexOfNonGap(905));
            Assert.AreEqual(501, sparseSeqObj.LastIndexOfNonGap(501));

            Console.WriteLine(
                "SparseSequenceBVT: Validation of LastIndexOfNonGap() method successfully completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT: Validation of LastIndexOfNonGap() method successfully completed");
        }

        /// <summary>
        /// Creates a sparse sequence and validates Isequence Clone() method
        /// </summary>
        [Test]
        public void ValidateSparseISequenceClone()
        {
            SparseSequence sparseSequence = CreateSparseSequence(Alphabets.DNA, 15);
            ISequence iSeq = (ISequence)sparseSequence;
            SparseSequence sparseSeqCopy = (SparseSequence)iSeq.Clone();

            // Retrieve all stored sequence items and validate Sparse Sequence
            int retrieveIndex = 15;
            foreach (ISequenceItem item in Alphabets.DNA)
            {
                // Validate new added items
                Assert.IsTrue(sparseSeqCopy.Contains(item));
                Assert.AreEqual(sparseSeqCopy.IndexOf(item), retrieveIndex);
                Assert.AreEqual(item, sparseSeqCopy[retrieveIndex]);
                Assert.AreEqual(item.Symbol, sparseSeqCopy[retrieveIndex].Symbol);
                retrieveIndex++;
            }

            Console.WriteLine(
                "SparseSequenceBVT: Validation of ISequence Clone is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT:  Validation of ISequence Clone is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and validates GetObjectData() method
        /// </summary>
        [Test]
        public void ValidateSparseSequenceGetObjectData()
        {
            SerializationInfo info = new SerializationInfo(typeof(SparseSequence),
               new FormatterConverter());
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            SparseSequence sparseSeqObj = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeqObj.GetObjectData(info, context);

            // Validate if there are no exceptions from the public method.
            Assert.IsNotNull(sparseSeqObj);
            Console.WriteLine(
                "SparseSequenceBVT: Validation of GetObjectData() is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT:  Validation of GetObjectData() is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and validates GetEnumerator() method
        /// </summary>
        [Test]
        public void ValidateSparseSequenceGetEnumerator()
        {
            SparseSequence sparseSeqObj = CreateSparseSequence(Alphabets.DNA, 0);
            IEnumerator<ISequenceItem> enumResult = sparseSeqObj.GetEnumerator();

            // Validate if there are no exceptions from the public method and enum is not null.
            Assert.IsNotNull(enumResult);
            Console.WriteLine(
                "SparseSequenceBVT: Validation of GetEnumerator() is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT:  Validation of GetEnumerator() is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and Serialize & Deserialize 
        /// and validate the same
        /// </summary>
        [Test]
        public void ValidateSparseSequenceSerialize()
        {
            Stream stream = null;

            try
            {
                SparseSequence seq = new SparseSequence(Alphabets.DNA);
                seq.Count = 50;
                seq.Insert(2, Alphabets.DNA.A);
                seq.Insert(8, Alphabets.DNA.T);
                seq.Insert(9, Alphabets.DNA.T);
                seq.Insert(10, Alphabets.DNA.A);
                seq.Insert(21, Alphabets.DNA.G);
                seq.Insert(45, Alphabets.DNA.C);
                CompoundNucleotide cn = new CompoundNucleotide('M', "Compound");
                cn.Add(new Nucleotide('A', "Item A"), 30);
                cn.Add(new Nucleotide('C', "Item C"), 20);
                seq[46] = cn;
                seq.Add(new Nucleotide('A', "Question"));
                stream = File.Open("SparseSequence.data", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, seq);

                stream.Seek(0, SeekOrigin.Begin);
                SparseSequence deserializedSeq = (SparseSequence)formatter.Deserialize(stream);

                Assert.AreNotSame(seq, deserializedSeq);
                Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);

                Assert.AreEqual(seq.Count, deserializedSeq.Count);
                Assert.AreEqual(seq.DisplayID, deserializedSeq.DisplayID);
                Assert.AreEqual(seq.ID, deserializedSeq.ID);
                Assert.AreEqual(seq.IsReadOnly, deserializedSeq.IsReadOnly);
                Assert.AreEqual(seq.MoleculeType, deserializedSeq.MoleculeType);

                Assert.AreSame(seq[0], deserializedSeq[0]);
                Assert.AreSame(seq[2], deserializedSeq[2]);
                Assert.AreSame(seq[8], deserializedSeq[8]);
                Assert.AreSame(seq[9], deserializedSeq[9]);
                Assert.AreSame(seq[10], deserializedSeq[10]);
                Assert.AreSame(seq[21], deserializedSeq[21]);
                Assert.AreSame(seq[45], deserializedSeq[45]);
                Assert.AreNotSame(seq[46], deserializedSeq[46]);
                cn = deserializedSeq[46] as CompoundNucleotide;
                IList<ISequenceItem> sequenceItems = cn.SequenceItems;
                Assert.AreEqual(sequenceItems.Count, 2);
                Assert.IsTrue(sequenceItems.First(I => I.Symbol == 'A') != null);
                Assert.IsTrue(sequenceItems.First(I => I.Symbol == 'C') != null);
                Assert.AreEqual(seq[46].Symbol, 'M');

                Assert.AreNotSame(seq[seq.Count - 1], deserializedSeq[deserializedSeq.Count - 1]);
                Assert.AreEqual(seq.Statistics.GetCount('A'), deserializedSeq.Statistics.GetCount('A'));
                stream.Close();
                stream = null;
                seq = new SparseSequence(Alphabets.DNA);
                stream = File.Open(Constants.SparseSerializeFile, FileMode.Create);
                formatter = new BinaryFormatter();
                formatter.Serialize(stream, seq);

                stream.Seek(0, SeekOrigin.Begin);
                deserializedSeq = (SparseSequence)formatter.Deserialize(stream);

                Assert.AreNotSame(seq, deserializedSeq);
                Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);

                Assert.AreEqual(seq.Count, deserializedSeq.Count);
                Assert.AreEqual(seq.DisplayID, deserializedSeq.DisplayID);
                Assert.AreEqual(seq.ID, deserializedSeq.ID);
                Assert.AreEqual(seq.IsReadOnly, deserializedSeq.IsReadOnly);
                Assert.AreEqual(seq.MoleculeType, deserializedSeq.MoleculeType);
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates sparse sequence object and insert all seq items of alphabet.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private void ValidateSparseSequence(IAlphabet alphabet)
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

            Console.WriteLine("SparseSequenceBVT: Validation of Insert() method  is completed");
            ApplicationLog.WriteLine("SparseSequenceBVT: Validation of Insert() method  is completed");
        }

        /// <summary>
        /// Creates sparse sequence object using seq item and index overload.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private void ValidateSparseSequenceWithSeqItem(IAlphabet alphabet)
        {
            int insertIndex = 0;
            int[] randomNumbers = Utility.RandomNumberGenerator(50, alphabet.Count);
            foreach (ISequenceItem item in alphabet)
            {
                // Add sequence item to sparse seq.
                SparseSequence sparseSeq = new SparseSequence(alphabet,
                    randomNumbers[insertIndex], item);

                // Validate newly added item
                Assert.IsTrue(sparseSeq.Contains(item));
                Assert.AreEqual(sparseSeq.IndexOf(item), randomNumbers[insertIndex]);
                Assert.AreEqual(item, sparseSeq[randomNumbers[insertIndex]]);
                Assert.AreEqual(item.Symbol, sparseSeq[randomNumbers[insertIndex]].Symbol);
                insertIndex++;
            }

            Console.WriteLine(
                "SparseSequenceBVT: Validation of Sparse Sequence constructor with Sequence item and index  is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT: Validation of Sparse Sequence constructor with Sequence item and index is completed");
        }

        /// <summary>
        /// Creates sparse sequence object using seq item list overload.
        /// Validates if all items are present in sparse sequence instance.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private void ValidateSparseSequenceWithSeqItemList(IAlphabet alphabet)
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
                "SparseSequenceBVT: Validation of Sparse Sequence constructor with Sequence item list is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT: Validation of Sparse Sequence constructor with Sequence item list is completed");

        }

        /// <summary>
        /// Creates sparse sequence object and changes IsReadOnly value to "false".
        /// Adds all sequence items of alphabet and validates if all items are added properly.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private void ValidateSparseSequenceAdd(IAlphabet alphabet)
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

            Console.WriteLine("SparseSequenceBVT: Validation of Add() method is completed");
            ApplicationLog.WriteLine("SparseSequenceBVT: Validation of Add() method is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet. 
        /// Delete all sequence items using Clear() method and 
        /// validates if all items are deleted from sparse sequence object.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private void ValidateSparseSequenceClear(IAlphabet alphabet)
        {
            SparseSequence sparseSeq = CreateSparseSequence(alphabet, 10);
            sparseSeq.IsReadOnly = false;

            // Validate if sparse sequence conatins all sequence items.
            Assert.AreEqual(alphabet.Count + 10, sparseSeq.Count);

            // Clear the sparse sequence.
            sparseSeq.Clear();

            // Validate if all sequence items are deleted.
            Assert.AreEqual(0, sparseSeq.Count);

            Console.WriteLine("SparseSequenceBVT: Validation of Clear() method is completed");
            ApplicationLog.WriteLine("SparseSequenceBVT: Validation of Clear() method is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet. 
        /// Creates copy of sparse sequence object using clone() method.
        /// and Validates if copy of sparse sequence is as expected.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private void ValidateSparseSequenceCopyTo(IAlphabet alphabet)
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

            Console.WriteLine("SparseSequenceBVT: Validation of CopyTo() method is completed");
            ApplicationLog.WriteLine("SparseSequenceBVT: Validation of CopyTo() method is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet. 
        /// Creates copy of sparse sequence object using clone() method.
        /// and Validates if copy of sparse sequence is as expected.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private void ValidateSparseSequenceCopy(IAlphabet alphabet)
        {
            SparseSequence sparseSequence = CreateSparseSequence(alphabet, 15);
            SparseSequence sparseSeqCopy = sparseSequence.Clone();

            // Retrieve all stored sequence items and validate Sparse Sequence
            int retrieveIndex = 15;
            foreach (ISequenceItem item in alphabet)
            {
                // Validate new added items
                Assert.IsTrue(sparseSeqCopy.Contains(item));
                Assert.AreEqual(sparseSeqCopy.IndexOf(item), retrieveIndex);
                Assert.AreEqual(item, sparseSeqCopy[retrieveIndex]);
                Assert.AreEqual(item.Symbol, sparseSeqCopy[retrieveIndex].Symbol);
                retrieveIndex++;
            }

            Console.WriteLine(
                "SparseSequenceBVT: Validation of copy of sparse sequence object is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT:  Validation of copy of sparse sequence object is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items of alphabet 
        /// and removes all sequence items.
        /// Validates if items are getting removed.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private void ValidateSparseSequenceRemove(IAlphabet alphabet)
        {
            SparseSequence sparseSequence = CreateSparseSequence(alphabet, 0);
            sparseSequence.IsReadOnly = false;

            // Remove all sequence items
            Assert.AreEqual(alphabet.Count, sparseSequence.Count);
            foreach (ISequenceItem item in alphabet)
            {
                sparseSequence.Remove(item);
            }

            // Validate if all items are removed.
            Assert.AreEqual(0, sparseSequence.Count);

            Console.WriteLine(
                "SparseSequenceBVT: Validation of Replace() method by passing char is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT: Validation of Replace() method by passing char is completed");
        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items at even position of alphabet 
        /// and replace the sequence item with sequence item symbol present at odd position.
        /// Validates if item is replaced.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private void ValidateSparseSequenceReplaceWithChar(IAlphabet alphabet)
        {
            // Create sequence item array
            ISequenceItem[] sequenceItemArray = new ISequenceItem[alphabet.Count];
            int index = 0;
            foreach (ISequenceItem item in alphabet)
            {
                sequenceItemArray[index] = item;
                index++;
            }

            /// Validate the Replace method with char
            /// Add even sequence items from sequence item array at random position 
            /// and replace with odd sequence item symbol from array.
            int replaceIndex = 1;
            int[] randomNumbers = Utility.RandomNumberGenerator(1502, alphabet.Count);
            index = 0;
            for (int addIndex = 0; addIndex < alphabet.Count; addIndex = addIndex + 2)
            {
                SparseSequence sparseSeq =
                    new SparseSequence(alphabet, randomNumbers[index], sequenceItemArray[addIndex]);
                sparseSeq.IsReadOnly = false;
                sparseSeq.Replace(randomNumbers[index], sequenceItemArray[replaceIndex].Symbol);

                // Validate if item is replaced as expected.
                Assert.AreEqual(sparseSeq[randomNumbers[index]].Symbol,
                    sequenceItemArray[replaceIndex].Symbol);
                Assert.AreEqual(sparseSeq[randomNumbers[index]],
                    sequenceItemArray[replaceIndex]);

                replaceIndex = replaceIndex + 2;
                index++;
            }

            Console.WriteLine(
                "SparseSequenceBVT: Validation of Replace() method by passing char is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT: Validation of Replace() method by passing char is completed");

        }

        /// <summary>
        /// Creates a sparse sequence and inserts sequence items at even position of alphabet 
        /// and replace with sequence item present at odd position.
        /// Validates if item is replaced.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        private void ValidateSparseSequenceReplaceWithSequenceItem(IAlphabet alphabet)
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
                "SparseSequenceBVT: Validation of Replace() method with sequence item is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT: Validation of Replace() method with sequence item is completed");
        }

        /// <summary>
        /// Creates a sparse a sequence and inserts all sequence items of alphabet. 
        /// Validates various properties present in the sparse class.
        /// </summary>
        /// <param name="alphabet">alphabet instance.</param>
        /// <param name="nodeName">xml node.</param>
        private void ValidateSparseSequenceAllProperties(IAlphabet alphabet, string nodeName)
        {
            // Get expected values from xml file.
            string expectedComplement = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Complement);
            string expectedReverse = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Reverse);
            string expectedReverseComplement = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ReverseComplement);

            // Create sparse sequence object
            int insertPosition = 0;
            SparseSequence sparseSequence = CreateSparseSequence(alphabet, insertPosition);
            sparseSequence.DisplayID = "Display ID";

            //Validate all properties
            Assert.AreEqual(alphabet.Count + insertPosition, sparseSequence.Count);
            Assert.AreEqual(alphabet, sparseSequence.Alphabet);
            Assert.AreEqual(expectedComplement, sparseSequence.Complement.ToString());
            Assert.AreEqual("Display ID", sparseSequence.DisplayID);
            Assert.IsNull(sparseSequence.Documentation);
            Assert.IsEmpty(sparseSequence.ID);
            Assert.IsTrue(sparseSequence.IsReadOnly);
            Assert.IsEmpty(sparseSequence.Metadata);
            Assert.AreEqual(MoleculeType.Invalid, sparseSequence.MoleculeType);
            Assert.AreEqual(expectedReverse, sparseSequence.Reverse.ToString());
            Assert.AreEqual(expectedReverseComplement, sparseSequence.ReverseComplement.ToString());
            Assert.IsNotNull(sparseSequence.Statistics);
            Assert.IsNotNull(sparseSequence.GetKnownSequenceItems());

            Console.WriteLine(
                "SparseSequenceBVT: Validation of all properties of sparse sequence instance is completed");
            ApplicationLog.WriteLine(
                "SparseSequenceBVT: Validation of all properties of sparse sequence instance is completed");
        }

        /// <summary>
        ///  Create sparse sequence and insert all sequence items of alphabet.
        /// </summary>
        /// <param name="alphabet"></param>
        /// <returns></returns>
        private SparseSequence CreateSparseSequence(IAlphabet alphabet, int insertPosition)
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
