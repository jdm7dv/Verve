// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SparseSequenceP2TestCases.cs
 * 
 * This file contains the Sparse Sequence P2 test case validation.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Sparse Sequence P2 level validations..
    /// </summary>
    [TestClass]
    public class SparseSequenceP2TestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SparseSequenceP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion

        #region SparseSequence P2 TestCases

        /// <summary>
        /// Validate an exception by passing null value.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Null Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceNullException()
        {
            string expectedErrorMessage = GetErrorMessage(
                Constants.SparseNullExceptionMessage);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;
            SparseSequence sparseSeq = null;

            // Try Creating sparse sequence by passing null value.
            try
            {
                sparseSeq = new SparseSequence(null);
            }
            catch (ArgumentNullException e)
            {
                actualError = e.Message;
            }

            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture)); ;
            Assert.IsNull(sparseSeq);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        /// Validate creation of sparse sequence by passing negative index 
        /// to Sparse Sequence.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Exception  
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSeqNegativeIndexException()
        {
            string expectedErrorMessage = GetErrorMessage(
                Constants.NegativeIndexErrorMessage);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;
            SparseSequence sparseSeq = null;

            // Try Creating sparse sequence by passing null value.
            try
            {
                sparseSeq = new SparseSequence(Alphabets.DNA, -5, Alphabets.DNA.A);
            }
            catch (ArgumentException e)
            {
                actualError = e.Message;
            }

            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));
            Assert.IsNull(sparseSeq);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        ///  Validate creation of sparse sequence by passing null value as alphabet
        ///  to SparseSequence(alphabet,index,SeqItem).
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Null Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceAlphabetNullException()
        {
            string expectedErrorMessage = GetErrorMessage(
                Constants.SparseNullExceptionMessage);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;
            SparseSequence sparseSeq = null;

            // Try Creating sparse sequence by passing null value as alphabet.
            try
            {
                sparseSeq = new SparseSequence(null, 2, Alphabets.DNA.A);
            }
            catch (ArgumentNullException e)
            {
                actualError = e.Message;
            }

            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));
            Assert.IsNull(sparseSeq);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        ///  Validate creation of sparse sequence by passing Invalid Sequence Item.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Exception error.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceInvalidSymbolException()
        {
            string expectedErrorMessage = GetErrorMessage(
               Constants.SymbolExceptionMessage);
            string actualError = string.Empty;
            SparseSequence sparseSeq = null;
            // Try Creating sparse sequence by passing null value as alphabet.
            try
            {
                sparseSeq = new SparseSequence(Alphabets.DNA, 2, Alphabets.RNA.U);
            }
            catch (ArgumentException e)
            {
                actualError = e.Message;
            }
            Assert.IsNull(sparseSeq);
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                actualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate creation of sparse sequence by passing negative index 
        /// to Sparse Sequence(alphabet;index;IList<SeqItems>).
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseIListSeqNegativeIndexException()
        {
            string expectedErrorMessage = GetErrorMessage(
               Constants.NegativeIndexErrorMessage);

            string actualError = string.Empty;
            string updatedActualError = string.Empty;
            SparseSequence sparseSeq = null;

            // Try Creating sparse sequence by passing null value.
            try
            {
                sparseSeq = new SparseSequence(Alphabets.DNA, -5,
                    new List<ISequenceItem>() { Alphabets.DNA.A, Alphabets.DNA.C });
            }
            catch (ArgumentException e)
            {
                actualError = e.Message;
            }

            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));
            Assert.IsNull(sparseSeq);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        ///  Validate creation of sparse sequence by passing null value as alphabet
        ///  to SparseSequence(alphabet,index,IList<SqItems>).
        ///  Input Data : Valid Alphabet.
        ///  Output Data : Validate an exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseListSeqItemsAlphabetNullException()
        {
            string expectedErrorMessage = GetErrorMessage(
               Constants.SparseNullExceptionMessage);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;
            SparseSequence sparseSeq = null;

            // Try Creating sparse sequence by passing null value as alphabet.
            try
            {
                sparseSeq = new SparseSequence(null, 2,
                    new List<ISequenceItem>() { Alphabets.DNA.A, Alphabets.DNA.C });
            }
            catch (ArgumentNullException e)
            {
                actualError = e.Message;
            }

            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));
            Assert.IsNull(sparseSeq);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        /// Validate creation of sparse sequence by passing null value as
        /// alphabet parameter to Sparse Sequence(alphabet;index;IList<SeqItems>).
        ///  Input Data: Valid Alphabet.
        ///  Output Data : Validate Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseIListSeqAlphabetException()
        {
            string expectedErrorMessage = GetErrorMessage(
               Constants.SymbolExceptionMessage);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;
            SparseSequence sparseSeq = null;

            // Try Creating sparse sequence by passing null value.
            try
            {
                sparseSeq = new SparseSequence(Alphabets.DNA, 2,
                    new List<ISequenceItem>() { Alphabets.RNA.U, Alphabets.RNA.U });
            }
            catch (ArgumentException e)
            {
                actualError = e.Message;
            }

            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));
            Assert.IsNull(sparseSeq);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        ///  Validate insertion of seq items to the end of sparse sequence 
        ///  with IsReadOnly property set to true.
        ///  Input Data: Valid Alphabet.
        ///  Output Data : Validate modifying ReadOnly Sparse Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSeqWithIsReadOnlyProperty()
        {
            string expectedErrorMessage = GetErrorMessage(
               Constants.ReadOnlyExceptionMessage);
            string actualError = string.Empty;
            SparseSequence sparseSeq = null;

            //Create a sparse sequence.
            sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set IsReadOnly Property to True.
            sparseSeq.IsReadOnly = true;
            try
            {
                sparseSeq.Insert(1, 'A');
            }
            catch (InvalidOperationException e)
            {
                actualError = e.Message;
            }

            // Validate an exception.
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                actualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        ///  Validate Addition of seq items to the end of sparse sequence 
        ///  with IsReadOnly property set to true.
        ///  Input Data: Valid Alphabet.
        ///  Output Data : Validate modifying ReadOnly Sparse Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseAdditionOfSeqItemWithIsReadOnlyPropertyTrue()
        {
            string expectedErrorMessage = GetErrorMessage(
               Constants.ReadOnlyExceptionMessage);
            string actualError = string.Empty;

            //Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.Protein);

            // Set IsReadOnly Property to True.
            sparseSeq.IsReadOnly = true;
            try
            {
                sparseSeq.Add(Alphabets.Protein.Ala);

            }
            catch (InvalidOperationException e)
            {
                actualError = e.Message;
            }

            // Validate an exception.
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                actualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        ///  Validate Addition of Invalid seq items to the end of sparse sequence 
        ///  with IsReadOnly property set to False.
        ///  Input Data : Valid Alphabet.
        ///  Output Data : Validation of Exception.for adding invalid SequenceItems.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateExceptionByPassingInvalidSeqItem()
        {
            string expectedErrorMessage = GetErrorMessage(
               Constants.SymbolExceptionMessage);
            string actualError = string.Empty;

            //Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Try to add RNA Sequence Item to DNA sparse sequence.
            try
            {
                sparseSeq.Add(Alphabets.RNA.U);
            }
            catch (ArgumentException e)
            {
                actualError = e.Message;
            }

            // Validate an exception.
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                actualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        ///  Validate deletion sequence items from the Protein 
        ///  sparse sequence using Clear() method.with 
        ///  IsReadOnly property set to False.
        ///  Input Data : Alphabet DNA.
        ///  Output Data : Validation of modifying ReadOnly Sparse Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateReadOnlySparseSeqItemDeletion()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.ReadOnlyExceptionMessage);
            string actualError = string.Empty;

            //Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set IsReadOnly Property to True.
            sparseSeq.IsReadOnly = true;

            // Try to delete ReadOnly DNA Sparse Sequence with 
            // ReadOnly Property True.
            try
            {
                sparseSeq.Clear();

            }
            catch (InvalidOperationException e)
            {
                actualError = e.Message;
            }

            // Validate an exception.
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                actualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate an expected output by passing invalid sequence item to
        /// contains method.
        /// Input Data : Valid Dna Alphabet
        /// Output Data : Validation of invalid Sequence item in Sparse sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSeqItemByPassingInvalidItem()
        {
            bool result = false;
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store all sequence items in sparse sequence object
            int insertIndex = 0;
            int[] randomNumbers = Utility.RandomNumberGenerator(2000, Alphabets.DNA.Count);
            foreach (ISequenceItem item in Alphabets.DNA)
            {
                sparseSeq[randomNumbers[insertIndex]] = item;
                insertIndex++;
            }

            // Validate whether invalid sequence item contains in Sparse 
            // sequence or not.
            result = sparseSeq.Contains(Alphabets.RNA.U);

            Assert.IsFalse(result);
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence error was validated successfully {0}",
                result));
        }

        /// <summary>
        /// Validate an exception by passing Protein sequence item to contains method 
        /// with Dna sparse sequence.contains method.
        /// Input Data : Dna Alphabet.
        /// Output Data : Validation of Protein Sequence Item.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSeqItemsByPassingProteinSeqItemToDnaSequence()
        {
            bool result = false;
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store all sequence items in sparse sequence object
            int insertIndex = 0;
            int[] randomNumbers = Utility.RandomNumberGenerator(2000, Alphabets.DNA.Count);
            foreach (ISequenceItem item in Alphabets.DNA)
            {
                sparseSeq[randomNumbers[insertIndex]] = item;
                insertIndex++;
            }

            // Validate whether invalid sequence item contains in Sparse 
            // sequence or not.
            result = sparseSeq.Contains(Alphabets.Protein.Ala);

            Assert.IsFalse(result);
            Console.WriteLine(string.Format((IFormatProvider)null,
                " SparseSequence P2: SparseSequence error was validated successfully.{0}",
                result));
        }

        /// <summary>
        /// Validate an exception by passing Rna sequence item to contains method 
        /// with Dna sparse sequence.contains method.
        /// Input Data : Rna Alphabet.
        /// Output Data : Validation of Protein Sequence Item.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSeqItemsByPassingProteinSeqItemToRnaSequence()
        {
            bool result = false;
            SparseSequence sparseSeq = new SparseSequence(Alphabets.RNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store all sequence items in sparse sequence object
            int insertIndex = 0;
            int[] randomNumbers = Utility.RandomNumberGenerator(2000, Alphabets.RNA.Count);
            foreach (ISequenceItem item in Alphabets.RNA)
            {
                sparseSeq[randomNumbers[insertIndex]] = item;
                insertIndex++;
            }

            // Validate whether invalid sequence item contains in Sparse 
            // sequence or not.
            result = sparseSeq.Contains(Alphabets.Protein.Ala);

            Assert.IsFalse(result);
            Console.WriteLine(string.Format((IFormatProvider)null,
                " SparseSequence P2: SparseSequence error was validated successfully.{0}",
                result));
        }

        /// <summary>
        /// Validate Sparse sequence items insert() method  by passing 
        /// invalid index but with valid sequence item..
        /// Input Data : Valid Dna Alphabet
        /// Output Data : Validation of an exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateErrorByPassingInvalidIndexValue()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.IndexError);

            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store all sequence items in sparse sequence object
            int insertIndex = 0;
            int[] randomNumbers = Utility.RandomNumberGenerator(2000, Alphabets.DNA.Count);
            foreach (ISequenceItem item in Alphabets.DNA)
            {
                sparseSeq[randomNumbers[insertIndex]] = item;
                insertIndex++;
            }

            // Try Inserting Sequence with Invalid Index.
            try
            {
                sparseSeq.Insert(2001, Alphabets.DNA.A);
            }
            catch (ArgumentException e)
            {
                actualError = e.Message;
            }

            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        /// Validate an exception by setting sparse sequence items 
        /// position greater then sparse count.
        /// Input Data : Valid Dna Alphabet
        /// Output Data : Validation of an exception by setting sparse 
        /// sequence items greater than sprase count.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateExceptionByPassingSeqItemToInvalidSequenceItemPostion()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.InvalidSequenceCountError);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count 1000 to insert sequence items.
            sparseSeq.Count = 1000;

            // Try to store seq items at 1050 position greater than sparse
            // sequence count.
            try
            {
                sparseSeq[1050] = Alphabets.DNA.A;
            }
            catch (ArgumentOutOfRangeException e)
            {
                actualError = e.Message;
            }

            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        /// Validate copying sparse sequence item to array by 
        /// specifying null value as array.
        /// Input Data : Valid Dna Alphabet,sequence count.
        /// Output Data : Validation of an exception.by passing null value to 
        /// CopyTo() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateExceptionByPassingNullValueToCopyTo()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.NullArrayException);

            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store all sequence items in sparse sequence object
            int insertIndex = 0;
            int[] randomNumbers = Utility.RandomNumberGenerator(2000, Alphabets.DNA.Count);
            foreach (ISequenceItem item in Alphabets.DNA)
            {
                sparseSeq[randomNumbers[insertIndex]] = item;
                insertIndex++;
            }

            // Try Passing null value to CopyTo method.
            try
            {
                sparseSeq.CopyTo(null, 0);
            }
            catch (ArgumentNullException e)
            {
                actualError = e.Message;
            }

            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        /// Validate copying sparse sequence item to array by 
        /// specifying invalid index value..
        /// Input Data : Valid Dna Alphabet,sequence count and 
        /// invalid index value to CopyTo()..
        /// Output Data : Validation of an exception.by passing invalid
        /// index value CopyTo() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateExceptionByPassingInvalidIndexValueToCopyTo()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.OutOfIndexException);

            string actualError = string.Empty;

            // Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;
            ISequenceItem[] sequenceItems = new ISequenceItem[2000];

            // Store all sequence items in sparse sequence object
            int insertIndex = 0;
            int[] randomNumbers = Utility.RandomNumberGenerator(2000, Alphabets.DNA.Count);
            foreach (ISequenceItem item in Alphabets.DNA)
            {
                sparseSeq[randomNumbers[insertIndex]] = item;
                insertIndex++;
            }

            // Try Passing invalid index value to CopyTo method.
            try
            {
                sparseSeq.CopyTo(sequenceItems, 1999);
            }
            catch (IndexOutOfRangeException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                actualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate sparse sequence items index value using IndexOf(SeqItem)
        /// method by passing another sequence item other than sparse sequence item.
        /// Input Data : Valid Dna Alphabet,sequence count and IseqItem
        /// Output Data : Valiation of an expected output.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateIndexValueOfDifferentSeqItemOtherThanSparseSeqItem()
        {
            int result;

            // Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store all sequence items in sparse sequence object
            int insertIndex = 0;
            int[] randomNumbers = Utility.RandomNumberGenerator(2000, Alphabets.DNA.Count);
            foreach (ISequenceItem item in Alphabets.DNA)
            {
                sparseSeq[randomNumbers[insertIndex]] = item;
                insertIndex++;
            }

            result = sparseSeq.IndexOf(Alphabets.RNA.U);

            // Validate an expected result.
            Assert.AreEqual(result, -1);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                result));
        }

        /// <summary>
        /// Validate sparse sequence items index value using IndexOf(SeqItem)
        /// method by passing null value.
        /// Input Data : Valid Dna Alphabet,sequence count and IseqItem
        /// Output Data : Valiation of an expected output.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateIndexOfSparseSeqByPassingNullValue()
        {
            int result;

            // Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store sequence items at 10th,20th and 100th position.
            sparseSeq[10] = Alphabets.DNA.A;
            sparseSeq[20] = Alphabets.DNA.G;
            sparseSeq[100] = Alphabets.DNA.G;

            result = sparseSeq.IndexOf(null);

            // Validate an expected result.
            Assert.AreEqual(result, 0);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                result));
        }

        /// <summary>
        /// Validate Sparse sequence items insert() method by passing 
        /// invalid index but with valid sequence item to insert(pos; char).
        /// Input Data : Valid Dna Alphabet,sequence count and 
        /// invalid index 
        /// Output Data : Validation of an exception.by passing invalid
        /// index value Insert() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateExceptionByPassingInvalidIndexValueToInsertMethod()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.SeqPositionError);

            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            // Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store Sequence items at different position.
            sparseSeq[2] = Alphabets.DNA.A;
            sparseSeq[3] = Alphabets.DNA.G;

            // Try inserting seq item with invalid index value
            // greater than sequence count 2000.
            try
            {
                sparseSeq.Insert(2001, 'T');
            }
            catch (ArgumentOutOfRangeException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate an error by passing invalid values to 
        /// range(start; length) method.
        /// Input Data : Valid Dna Alphabet,sequence count and 
        /// invalid start index 
        /// Output Data : Validation of an exception.by passing invalid
        /// start index to Range() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateExceptionByPassingInvalidIndexValueToRangeMethod()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.InvalidStartIndex);

            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            // Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store Sequence items at different position.
            sparseSeq[2] = Alphabets.DNA.A;
            sparseSeq[3] = Alphabets.DNA.G;

            // Try entering invalid start index (2001) value to
            // Range() method.
            try
            {
                sparseSeq.Range(2001, 10);
            }
            catch (ArgumentOutOfRangeException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate an error by passing invalid index to 
        /// RemoveAt(index) method.
        /// Input Data : Valid Dna Alphabet,sequence count and 
        /// invalid index 
        /// Output Data : Validation of an exception.by passing invalid
        /// index to RemoveAt(index) method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateExceptionByPassingInvalidIndexValueToRemoveAt()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.InvalidSequenceCountError);

            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            // Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 2000;

            // Store Sequence items at different position.
            sparseSeq[2] = Alphabets.DNA.A;
            sparseSeq[3] = Alphabets.DNA.G;

            // Try entering invalid start index (2001) value to
            // RemoveAt() method.
            try
            {
                sparseSeq.RemoveAt(2001);
            }
            catch (ArgumentOutOfRangeException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate an error by passing invalid length to RemoveRange() method.
        /// Input Data : Valid Dna Alphabet,sequence count and 
        /// invalid length value 
        /// Output Data : Validation of an exception.by passing invalid
        /// length value to RemoveRange() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateExceptionByPassingInvalidLengthValueToRemoveRange()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.InvalidLengthError);

            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            // Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 10;

            // Store Sequence items at different position.
            sparseSeq[2] = Alphabets.DNA.A;
            sparseSeq[3] = Alphabets.DNA.G;

            // Try entering invalid start index (11) value to
            // RemoveRange() method.
            try
            {
                sparseSeq.RemoveRange(0, 11);
            }
            catch (ArgumentException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate error while replacing sequence item with invalid sequence item.
        /// Input Data : Valid Dna Alphabet,sequence count and 
        /// invalid sequence char 
        /// Output Data : Validation of an exception.by passing invalid
        /// length value to Replace() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateExceptionByPassingInvalidCharToReplace()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.SymbolExceptionMessage);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            // Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 10;

            // Store Sequence items at different position.
            sparseSeq[2] = Alphabets.DNA.A;
            sparseSeq[3] = Alphabets.DNA.G;

            // Try replacing invalid RNA alphabet 'U' with DNA alphabet 
            // using Replace() method.
            try
            {
                sparseSeq.Replace(0, 'U');
            }
            catch (ArgumentException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate error while replacing sequence item with invalid sequence
        /// item position using Replace(pos;SeqItem) method
        /// Input Data : Valid Dna Alphabet,sequence count and 
        /// invalid position 
        /// Output Data : Validation of an exception.by passing invalid
        /// position value to Replace() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateExceptionByPassingInvalidPositionValueToReplace()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.InvalidPositionError);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            // Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 10;

            // Store Sequence items at different position.
            sparseSeq[2] = Alphabets.DNA.A;
            sparseSeq[3] = Alphabets.DNA.G;

            // Try replacing 11th position item using 
            // Replace() method.
            try
            {
                sparseSeq.Replace(11, Alphabets.DNA.A);
            }
            catch (ArgumentOutOfRangeException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate error while replacing sequence item with invalid sequence 
        /// item using RaplaceRange() method..
        /// Input Data : Valid Dna Alphabet,sequence count and 
        /// invalid Sequence string 
        /// Output Data : Validation of an exception.by passing invalid
        /// sequence to RaplaceRange() method.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateExceptionByPassingInvalidSeqStringToRaplaceRange()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.SequenceSymbolException);

            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            // Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 10;

            // Store Sequence items at different position.
            sparseSeq[2] = Alphabets.DNA.A;
            sparseSeq[3] = Alphabets.DNA.G;

            // Try inserting invalid sequence string to sparse sequence.
            try
            {
                sparseSeq.ReplaceRange(0, "PQRS");
            }
            catch (ArgumentException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        ///  Validate ToString((IFormatProvider)null) method in sparse sequence class.
        /// Input Data : Valid Dna Alphabet,sequence count.
        /// Output Data : Validation of an exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceToString()
        {
            string expectedErrorMessage = GetErrorMessage(
              Constants.ToStringException);
            string actualError = string.Empty;

            // Create a sparse sequence.
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);

            // Set the count to insert sequence items.
            sparseSeq.Count = 10;

            // Store Sequence items at different position.
            sparseSeq[2] = Alphabets.DNA.A;
            sparseSeq[3] = Alphabets.DNA.G;

            // Try to convert Sparse sequence to ToString((IFormatProvider)null).
            try
            {
                sparseSeq.ToString();
            }
            catch (NotSupportedException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            actualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                actualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate an exception by passing invalid size value.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceInvalidSize()
        {
            SparseSequence sparseSeq = null;

            // Try Creating sparse sequence by passing invalid size.
            try
            {
                sparseSeq = new SparseSequence(Alphabets.DNA, -1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }

            Assert.IsNull(sparseSeq);
        }

        /// <summary>
        /// Validate an exception by passing invalid size value.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceInvalidIndex()
        {
            SparseSequence sparseSeq = null;
            ISequenceItem seqItem = null;

            // Try Creating sparse sequence by passing invalid index.
            try
            {
                sparseSeq = new SparseSequence(Alphabets.DNA, -1, seqItem);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }

            Assert.IsNull(sparseSeq);
        }

        /// <summary>
        /// Validate an exception by passing invalid sequence item.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of null Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceInvalidSeqItem()
        {
            SparseSequence sparseSeq = null;
            ISequenceItem seqItem = null;

            // Try Creating sparse sequence by passing invalid sequence item.
            try
            {
                sparseSeq = new SparseSequence(Alphabets.DNA, 0, seqItem);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                LogMessage(ex.Message);
            }

            Assert.IsNull(sparseSeq);
        }

        /// <summary>
        /// Validate an exception by passing invalid size value.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of null Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceInvalidAlphabet()
        {
            SparseSequence sparseSeq = null;
            Sequence seq = new Sequence(Alphabets.RNA, "U");
            ISequenceItem seqItem = seq[0];

            // Try Creating sparse sequence by passing invalid alphabet.
            try
            {
                sparseSeq = new SparseSequence(Alphabets.DNA, 0, seqItem);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                LogMessage(ex.Message);
            }

            Assert.IsNull(sparseSeq);
        }

        /// <summary>
        /// Validate an exception by passing invalid size value.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceListInvalidIndex()
        {
            SparseSequence sparseSeq = null;
            Sequence seq = new Sequence(Alphabets.DNA, "G");
            List<ISequenceItem> seqItemList = new List<ISequenceItem>();
            seqItemList.Add(seq[0]);

            // Try Creating sparse sequence by passing invalid index.
            try
            {
                sparseSeq = new SparseSequence(Alphabets.DNA, -1, seqItemList);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }

            Assert.IsNull(sparseSeq);
        }

        /// <summary>
        /// Validate an exception by passing invalid sequence item.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of null Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceListInvalidSeqItem()
        {
            SparseSequence sparseSeq = null;
            List<ISequenceItem> seqItemList = null;

            // Try Creating sparse sequence by passing invalid sequence item
            try
            {
                sparseSeq = new SparseSequence(Alphabets.DNA, 0, seqItemList);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                LogMessage(ex.Message);
            }

            Assert.IsNull(sparseSeq);
        }

        /// <summary>
        /// Validate an exception by passing invalid size value.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of null Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceListInvalidAlphabet()
        {
            SparseSequence sparseSeq = null;
            Sequence seq = new Sequence(Alphabets.RNA, "U");
            List<ISequenceItem> seqItemList = new List<ISequenceItem>();
            seqItemList.Add(seq[0]);

            // Try Creating sparse sequence by passing Invalid alphabet
            try
            {
                sparseSeq = new SparseSequence(Alphabets.DNA, 0, seqItemList);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                LogMessage(ex.Message);
            }

            Assert.IsNull(sparseSeq);
        }

        /// <summary>
        /// Validate an exception by passing readonly for Insert.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Invalid operation Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceInsertReadOnly()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = true;
            Sequence seq = new Sequence(Alphabets.DNA, "AGCT");
            ISequenceItem seqItem = seq[0];
            try
            {
                sparseSeq.Insert(0, seqItem);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid position for Insert.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceInsertInvalidPosition()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;
            Sequence seq = new Sequence(Alphabets.DNA, "AGCT");
            ISequenceItem seqItem = seq[0];
            try
            {
                sparseSeq.Insert(-1, seqItem);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid sequence item for Insert.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceInsertRangeInvalidSequenceItem()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;
            Sequence seq = new Sequence(Alphabets.RNA, "U");
            ISequenceItem seqItem = seq[0];

            try
            {
                sparseSeq.Insert(sparseSeq.Count - 2, seqItem);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid position for Insert Range.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceInsertRangeInvalidPosition()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;

            try
            {
                sparseSeq.InsertRange(-1, "AGCT");
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid sequence for Insert Range.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of null Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceInsertRangeInvalidSequence()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;

            try
            {
                sparseSeq.InsertRange(sparseSeq.Count - 2, "");
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by updating read only for Remove Range.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of invalid operation Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceRemoveRangeReadOnly()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = true;
            try
            {
                sparseSeq.RemoveRange(sparseSeq.Count - 2, 5);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid position for Remove Range.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceRemoveRangeInvalidPosition()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;

            try
            {
                sparseSeq.RemoveRange(-1, 5);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid length for Remove Range.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of out of range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceRemoveRangeInvalidLength()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;

            try
            {
                sparseSeq.RemoveRange(sparseSeq.Count - 2, -1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by updating read only for Replace.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of invalid operation Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceReplaceCharReadOnly()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = true;
            try
            {
                sparseSeq.Replace(sparseSeq.Count - 2, 'A');
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid position for Replace.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceReplaceCharInvalidPosition()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;

            try
            {
                sparseSeq.Replace(-1, 'A');
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid Character for Replace.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of out of range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceReplaceCharInvalidCharacter()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;

            try
            {
                sparseSeq.Replace(sparseSeq.Count - 2, 'U');
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by updating read only for Replace.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of invalid operation Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceReplaceReadOnly()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = true;
            Sequence seq = new Sequence(Alphabets.DNA, "AGC");
            ISequenceItem seqItem = seq[0];
            try
            {
                sparseSeq.Replace(sparseSeq.Count - 2, seqItem);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid position for Replace.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceReplaceInvalidPosition()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            Sequence seq = new Sequence(Alphabets.DNA, "AGC");
            ISequenceItem seqItem = seq[0];
            sparseSeq.IsReadOnly = false;

            try
            {
                sparseSeq.Replace(-1, seqItem);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing null sequence for Replace.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceReplaceInvalidCharacter()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            ISequenceItem seqItem = null;
            sparseSeq.IsReadOnly = false;
            ISequenceItem itm = sparseSeq[0];

            sparseSeq.Replace(0, seqItem);

            Assert.AreNotEqual(itm, (ISequenceItem)sparseSeq[0]);

            ApplicationLog.WriteLine("SparseSequence P2: Successfully validated null character replace");
            // Log to Nunit GUI.
            Console.WriteLine("SparseSequence P2: Successfully validated null character replace");
        }

        /// <summary>
        /// Validate an exception by updating read only for Replace Range.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of invalid operation Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceReplaceRangeReadOnly()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = true;

            try
            {
                sparseSeq.ReplaceRange(sparseSeq.Count - 2, "AGC");
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid position for ReplaceRange.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceReplaceRangeInvalidPosition()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;

            try
            {
                sparseSeq.ReplaceRange(-1, "AGCT");
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid sequence for Replace.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of out of range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceReplaceRangeInvalidCharacter()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;

            try
            {
                sparseSeq.ReplaceRange(sparseSeq.Count - 2, "U");
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid position for Range.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceRangeInvalidPosition()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);

            try
            {
                sparseSeq.Range(-1, 5);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid length for Range.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of out of range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceRangeInvalidLength()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);

            try
            {
                sparseSeq.Range(sparseSeq.Count - 2, -1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid length for Range.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceRangeInvalidLengthGreater()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);

            try
            {
                sparseSeq.Range(sparseSeq.Count - 2, sparseSeq.Count + 2);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by updating read only for RemoveAt.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of invalid operation Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceRemoveAtReadOnly()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = true;

            try
            {
                sparseSeq.RemoveAt(0);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid position for Remove.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceRemoveAtInvalidPosition()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;

            try
            {
                sparseSeq.RemoveAt(-1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid indexer value.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceInvalidIndexer()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;
            ISequenceItem seqItem = null;
            try
            {
                seqItem = sparseSeq[-1];
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }

            Assert.IsNull(seqItem);
        }

        /// <summary>
        /// Validate an exception by passing invalid indexer value.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Out of Range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceSetInvalidIndexer()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;
            ISequenceItem seqItem = null;
            try
            {
                sparseSeq[-1] = seqItem;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing indexer value for read only indexer.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Invalid operation Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceSetReadOnlyIndexer()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = true;
            ISequenceItem seqItem = new Sequence(Alphabets.DNA, "AGCT")[0];

            try
            {
                sparseSeq[0] = seqItem;
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid sequence for Add() method.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceAddInvalidSequenceItem()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;
            ISequenceItem seqItem = new Sequence(Alphabets.RNA, "U")[0];

            try
            {
                sparseSeq.Add(seqItem);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing invalid count value.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of out of range Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceInvalidCount()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = false;

            try
            {
                sparseSeq.Count = -1;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogMessage(ex.Message);
            }
        }

        /// <summary>
        /// Validate an exception by passing readonly for Remove() method.
        /// Input Data : Valid Alphabet
        /// Output Data : Validation of Exception 
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateSparseSequenceRemoveReadOnly()
        {
            SparseSequence sparseSeq = CreateSparseSequence(Alphabets.DNA, 0);
            sparseSeq.IsReadOnly = true;
            ISequenceItem seqItem = new Sequence(Alphabets.DNA, "A")[0];

            try
            {
                sparseSeq.Remove(seqItem);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                LogMessage(ex.Message);
            }
        }

        #endregion SparseSequence P2 TestCases

        #region Supporting methods

        /// <summary>
        /// Get Error messages from xml node.
        /// </summary>
        /// <param name="nodeName">Name of the error message node </param>
        string GetErrorMessage(string nodeName)
        {
            string expectedErrorMessage = _utilityObj._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, nodeName);

            return expectedErrorMessage;
        }

        /// <summary>
        ///  Create sparse sequence and insert all sequence items of alphabet.
        /// </summary>
        /// <param name="alphabet"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Logs the message to Console and Log
        /// </summary>
        /// <param name="ex">Exception to log</param>
        private static void LogMessage(string message)
        {
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                message));
            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                message));
        }

        #endregion Supproting methods.
    }
}
