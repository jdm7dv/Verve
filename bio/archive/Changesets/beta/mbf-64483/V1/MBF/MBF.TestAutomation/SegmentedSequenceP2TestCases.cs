// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SegmentedSequenceP2TestCases.cs
 * 
 * This file contains the Segmented Sequence P2 test case validation.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Segmented Sequence P2 level validations.
    /// </summary>
    [TestFixture]
    public class SegmentedSequenceP2TestCases
    {
        #region Enums
        /// <summary>
        /// Segmented sequence methods name used for different testcases.
        /// </summary>
        enum SegmentedSequenceParameters
        {
            Range,
            RemoveAt,
            RemoveRange,
            Replace,
            ReplaceNull,
            ReplaceRna,
            ReplaceProtein,
            ReplaceRange,
            ReplaceRangeNull,
            Remove,
            InsertRange,
            Default
        };
        # endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SegmentedSequenceP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion

        #region Segmented Sequence P2 TestCases

        /// <summary>
        /// Validate an exception by passing different alphabet sequences 
        /// to segmented sequence constructor.
        /// Input Data : Dna and Rna Sequence.
        /// Output Data : Validation of different alphabet Exception 
        /// </summary>
        [Test]
        public void ValidateCreationOfSegSeqwithDnaAndRnaSequences()
        {
            ValidateDifferentAlphabetSegSequence(
                 Constants.DnaSegSequenceNode, Alphabets.DNA, Alphabets.RNA);
        }

        /// <summary>
        /// Validate an exception by passing different alphabet sequences 
        /// to segmented sequence constructor.
        /// Input Data : Rna and Protein Sequence.
        /// Output Data : Validation of different alphabet Exception 
        /// </summary>
        [Test]
        public void ValidateCreationOfSegSeqwithRnaAndProteinSequences()
        {
            ValidateDifferentAlphabetSegSequence(
                  Constants.InvalidRnaSegSequenceNode, Alphabets.RNA, Alphabets.Protein);
        }

        /// <summary>
        /// Validate an exception by passing different alphabet sequences 
        /// to segmented sequence constructor.
        /// Input Data : Dna and Protein Sequence.
        /// Output Data : Validation of different alphabet Exception 
        /// </summary>
        [Test]
        public void ValidateCreationOfSegSeqwithDnaAndProteinSequences()
        {
            ValidateDifferentAlphabetSegSequence(
                  Constants.InvalidDnaSegSequenceNode, Alphabets.DNA, Alphabets.Protein);
        }

        /// <summary>
        /// Validate addition of seq items to the end of segmented 
        /// sequence with IsReadOnly property set to true.
        /// Input Data :Valid two Protein Sequences.
        /// Output Data : Validation of modifying ReadOnly Segmented sequence.
        /// </summary>
        [Test]
        public void ValidateExceptionByModifyingReadOnlySegSequence()
        {
            // Get values from xml.
            string expectedErrorMessage = Utility._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.ReadOnlyExceptionMessage);
            string actualError = string.Empty;
            string firstSequence = Utility._xmlUtil.GetTextValue(
                Constants.ProteinSegSequenceForErrorValidationNode, Constants.FirstSequence);
            string secondSeq = Utility._xmlUtil.GetTextValue(
                Constants.ProteinSegSequenceForErrorValidationNode, Constants.SecondSegSequence);
            List<ISequence> seqList = new List<ISequence>();
            SegmentedSequence segmentedSeq = null;

            // Create a seqeunce.
            Sequence seq1 = new Sequence(Alphabets.Protein, firstSequence);
            Sequence seq2 = new Sequence(Alphabets.Protein, secondSeq);

            // Set first sequence to ReadOnly.
            seq1.IsReadOnly = true;

            // Add seqeunce to sequenceList
            seqList.Add(seq1);
            seqList.Add(seq2);

            // Create a segmented sequence.
            segmentedSeq = new SegmentedSequence(seqList);

            // Try to add new seq items to ReadOnly segmented sequence.
            try
            {
                segmentedSeq.Add(Alphabets.DNA.A);
            }
            catch (InvalidOperationException e)
            {
                actualError = e.Message;
            }

            seq1.IsReadOnly = false;
            SegmentedSequence segSeq = new SegmentedSequence(seq1);

            try
            {
                segSeq.Add(null);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                ex.Message));

            }
            // Validate an expected exception.
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                actualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate addition of Rna seq items to the end of Dna segmented 
        /// sequence using Add() method.
        /// Input Data :Valid two Dna Sequences.
        /// Output Data : Validation an exception thrown by Add() method
        /// </summary>
        [Test]
        public void ValidateExceptionByAddingRnaSeqItemToDnaSegSequence()
        {
            // Get values from xml.
            string expectedErrorMessage = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.InvalidSeqItemError);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;
            string firstSequence = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.FirstSequence);
            string secondSeq = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.SecondSegSequence);
            List<ISequence> seqList = new List<ISequence>();
            SegmentedSequence segmentedSeq = null;

            // Create a seqeunce.
            Sequence seq1 = new Sequence(Alphabets.DNA, firstSequence);
            Sequence seq2 = new Sequence(Alphabets.DNA, secondSeq);

            // Add seqeunce to sequenceList
            seq1.IsReadOnly = false;
            seq2.IsReadOnly = false;
            seqList.Add(seq1);
            seqList.Add(seq2);

            // Create a segmented sequence.
            segmentedSeq = new SegmentedSequence(seqList);

            // Try to add Rna seq items to Dna Segmented sequence.
            try
            {
                segmentedSeq.Add(Alphabets.RNA.U);
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
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        /// Validate addition of Protein seq items to the end of Dna segmented 
        /// sequence using Add() method.
        /// Input Data :Valid two Dna Sequences.
        /// Output Data : Validation an exception thrown by Add() method
        /// </summary>
        [Test]
        public void ValidateExceptionByAddingProteinSeqItemToDnaSegSequence()
        {
            // Get values from xml.
            string expectedErrorMessage = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.InvalidSeqItemError);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;
            string firstSequence = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.FirstSequence);
            string secondSeq = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.SecondSegSequence);
            List<ISequence> seqList = new List<ISequence>();
            SegmentedSequence segmentedSeq = null;

            // Create a seqeunce.
            Sequence seq1 = new Sequence(Alphabets.DNA, firstSequence);
            Sequence seq2 = new Sequence(Alphabets.DNA, secondSeq);

            // Add seqeunce to sequenceList
            seq1.IsReadOnly = false;
            seq2.IsReadOnly = false;
            seqList.Add(seq1);
            seqList.Add(seq2);

            // Create a segmented sequence.
            segmentedSeq = new SegmentedSequence(seqList);

            // Try to add Rna seq items to Dna Segmented sequence.
            try
            {
                segmentedSeq.Add(Alphabets.Protein.Ile);
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
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        /// Validate deletion of sequence items from segmented sequence using 
        /// Clear() method.by setting Sequence IsReadOnly property to true.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation Exception by modifying IsReadOnly Segmented 
        /// sequences to true.
        /// </summary>
        [Test]
        public void ValidateExceptionByModifyingIsReadOnlySegSeq()
        {
            // Get values from xml.
            string expectedErrorMessage = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.SeqReadOnlyException);
            string actualError = string.Empty;

            // Create segmented sequence.with IsReadOnly property set to true.
            SegmentedSequence segmentedSeq = CreateSegmentedSequence(
                Constants.DnaSegSequenceForErrorValidationNode, Alphabets.DNA, Alphabets.DNA, true);

            // Try deleting sequence items from ReadOnly Segmented Sequence.
            try
            {
                segmentedSeq.Clear();
            }
            catch (InvalidOperationException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                actualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate an expected output by passing Rna sequence item to
        /// contains method with Dna segmented sequence.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation of expected Output.
        /// sequences to true.
        /// </summary>
        [Test]
        public void ValidateOutputByPassingRnaSeqItemToContainsMethodForDnaSegmentedSeq()
        {
            bool result;
            // Create segmented sequence.with IsReadOnly property set to true.
            SegmentedSequence segmentedSeq = CreateSegmentedSequence(
                Constants.DnaSegSequenceForErrorValidationNode, Alphabets.DNA, Alphabets.DNA, true);

            result = segmentedSeq.Contains(Alphabets.RNA.U);

            Assert.IsFalse(result);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Rna Sequence Item was not present in Dna Segmented Sequence"));
        }

        /// <summary>
        /// Validate copying segmented sequence item to array by specifying 
        /// null value to CopyTo method.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation Exception by passing null value to CopyTo() 
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingNullValueToCopyTo()
        {
            // Get values from xml.
            string expectedErrorMessage = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.SegSeqNullArrayException);
            string actualError = string.Empty;
            string updatedactualError = string.Empty;

            // Create segmented sequence.with IsReadOnly property set to true.
            SegmentedSequence segmentedSeq = CreateSegmentedSequence(
                Constants.DnaSegSequenceForErrorValidationNode, Alphabets.DNA, Alphabets.DNA, true);

            // Pass null value to copyTo method 
            try
            {
                segmentedSeq.CopyTo(null, 0);
            }
            catch (ArgumentNullException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            updatedactualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedactualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                updatedactualError));
        }

        /// <summary>
        /// Validate copying segmented sequence item to array by specifying 
        /// Invalid index value to CopyTo method.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation Exception by passing null value to CopyTo() 
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingInvalidIndexValueToCopyTo()
        {
            // Get values from xml.
            string expectedErrorMessage = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.ArrayBoundException);
            string actualError = string.Empty;

            // Create segmented sequence.with IsReadOnly property set to true.
            SegmentedSequence segmentedSeq = CreateSegmentedSequence(
                Constants.DnaSegSequenceForErrorValidationNode, Alphabets.DNA, Alphabets.DNA, true);
            ISequenceItem[] SeqItemArray = new ISequenceItem[100];

            // Pass null value to copyTo method 
            try
            {
                segmentedSeq.CopyTo(SeqItemArray, 0);
            }
            catch (IndexOutOfRangeException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                actualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        ///  Validate segmented sequence items index value using IndexOf(SeqItem)
        ///  method  by passing another sequence item other than segmented sequence item.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation of expected Output.
        /// sequences to true.
        /// </summary>
        [Test]
        public void ValidateErrorByPassingDifferentSeqItemOtherThanSegSeqItemsToIndexOf()
        {
            int index;
            // Create segmented sequence.with IsReadOnly property set to true.
            SegmentedSequence segmentedSeq = CreateSegmentedSequence(
                Constants.DnaSegSequenceForErrorValidationNode, Alphabets.DNA, Alphabets.DNA, true);
            Sequence proteinSeq = new Sequence(Alphabets.Protein, "IGH");

            index = segmentedSeq.IndexOf(proteinSeq[0]);

            // Validate the IndexOf returing -1 value.
            Assert.AreEqual(index, -1);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                index));
        }

        /// <summary>
        /// Validate Segmented sequence items insert() method  by passing invalid
        /// index but with valid sequence item. 
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation Exception by passing Invalid Index to insert() 
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingInvalidIndexValueToInsertMethod()
        {
            // Get values from xml.
            string expectedErrorMessage = Utility._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.SeqPositionError);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            // Create segmented sequence.with IsReadOnly property set to true.
            SegmentedSequence segmentedSeq = CreateSegmentedSequence(
                Constants.DnaSegSequenceForErrorValidationNode, Alphabets.DNA, Alphabets.DNA, false);
            SegmentedSequence ReadOnlysegmentedSeq = CreateSegmentedSequence(
               Constants.DnaSegSequenceForErrorValidationNode, Alphabets.DNA, Alphabets.DNA, true);

            // Insert sequence items to read-only segmented sequence
            try
            {
                ReadOnlysegmentedSeq.Insert(1, 'A');
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(string.Format(null,
                    "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                    ex.Message));
            }

            // Pass a null sequenceItems
            try
            {
                segmentedSeq.Insert(1000, null);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(string.Format(null,
                    "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                    ex.Message));
            }

            // Pass invalid value to Insert method 
            try
            {
                segmentedSeq.Insert(1000, 'A');
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
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate Segmented sequence items insert() method  by passing 
        /// null value.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation Exception by passing null value to Insert() 
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingNullValueToInsertMethod()
        {
            // Get values from xml.
            string expectedErrorMessage = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.NullSequenceItemException);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            // Create segmented sequence.with IsReadOnly property set to true.
            SegmentedSequence segmentedSeq = CreateSegmentedSequence(
                Constants.DnaSegSequenceForErrorValidationNode, Alphabets.DNA, Alphabets.DNA, false);
            SegmentedSequence ReadOnlySegmentedSeq = CreateSegmentedSequence(
                Constants.DnaSegSequenceForErrorValidationNode, Alphabets.DNA, Alphabets.DNA, true);

            // Invalidate insert seq item for ReadOnly sequence
            try
            {
                ReadOnlySegmentedSeq.Insert(0, segmentedSeq[0]);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(null,
                    "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                    ex.Message));
            }

            // Pass null value to copyTo method 
            try
            {
                segmentedSeq.Insert(0, null as ISequenceItem);
            }
            catch (ArgumentNullException e)
            {
                actualError = e.Message;
            }

            // Validate an expected exception.
            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));

            // Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate Segmented sequence items insert(index,IseqItem) method  by passing invalid
        /// index but with valid sequence item. 
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation Exception by passing Invalid Index to insert(index,IseqItem)) 
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingInvalidIndexValueToInsertionOfSegSeq()
        {
            // Get values from xml.
            string expectedErrorMessage = Utility._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.SeqPositionError);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            // Create segmented sequence.with IsReadOnly property set to true.
            SegmentedSequence segmentedSeq = CreateSegmentedSequence(
                Constants.DnaSegSequenceForErrorValidationNode, Alphabets.DNA, Alphabets.DNA, false);

            // Pass null value to copyTo method 
            try
            {
                segmentedSeq.Insert(1000, Alphabets.DNA.A);
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
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Validate segmented sequence items index value using IndexOf(SeqItem) 
        /// method by passing null value.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data :Validate Output results by passing null value to IndexOf(). 
        /// </summary>
        [Test]
        public void ValidateOutputByPassingNullValueToIndexOf()
        {
            int index;
            // Create segmented sequence.with IsReadOnly property set to true.
            SegmentedSequence segmentedSeq = CreateSegmentedSequence(
                Constants.DnaSegSequenceForErrorValidationNode, Alphabets.DNA, Alphabets.DNA, false);

            // Get index by passing null value to IndexOf().
            index = segmentedSeq.IndexOf(null);

            // Validate an ouput.
            Assert.AreEqual(-1, index);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                index));
        }

        /// <summary>
        /// Validate an error by passing invalid values to range(start; length) method.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation an exception by passing invalid values to Range().
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingInvalidStartIndexValueToRange()
        {
            InValidateSegSeqMethods(Constants.DnaSegSequenceForErrorValidationNode,
                Constants.InvalidStartIndex, SegmentedSequenceParameters.Range);
        }

        /// <summary>
        /// Validate an error by passing invalid index to RemoveAt(index) method.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation an exception by passing invalid values to RemoveAt(index).
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingInvalidPositionValueToRemoveAt()
        {
            InValidateSegSeqMethods(
                 Constants.DnaSegSequenceForErrorValidationNode, Constants.SeqPositionError,
                 SegmentedSequenceParameters.RemoveAt);
        }

        /// <summary>
        /// Validate an error by passing invalid length to RemoveRnage() method.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation an exception by passing invalid values to RemoveRnage(index).
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingInvalidPositionValueToRemoveRange()
        {
            InValidateSegSeqMethods(
                 Constants.DnaSegSequenceForErrorValidationNode, Constants.SeqPositionError,
                 SegmentedSequenceParameters.RemoveRange);
        }

        /// <summary>
        /// Validate error while replacing sequence item with invalid sequence item.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation an exception by passing invalid char to Replace().
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingInvalidPositionValueToReplace()
        {
            InValidateSegSeqMethods(
                 Constants.DnaSegSequenceForErrorValidationNode, Constants.SequenceSymbolException,
                 SegmentedSequenceParameters.Replace);
        }

        /// <summary>
        /// Validate error while replacing sequence item with null value.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation an exception by passing null char to Replace(pos,char).
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingNullValueToReplace()
        {
            InValidateSegSeqMethods(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.NullSequenceItemException,
                SegmentedSequenceParameters.ReplaceNull);
        }

        /// <summary>
        /// Validate error while replacing different alphabet sequence item 
        /// with existing segmented sequence alphabet.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation an exception by passing Rna alphabet to
        /// Dna segmented sequence to Replace() method.
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingDifferentAlphabetValueToReplace()
        {
            InValidateSegSeqMethods(
                Constants.DnaVirtualSeqNode, Constants.SymbolExceptionMessage,
                SegmentedSequenceParameters.ReplaceRna);
        }

        /// <summary>
        /// Validate error while replacing Protein alphabet sequence item 
        /// with existing Dna segmented sequence alphabet.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation an exception by passing protein alphabet
        /// Dna segmented sequence to Replace() method.
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingProteinAlphabetToReplaceDnaSeqItem()
        {
            InValidateSegSeqMethods(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.SymbolExceptionMessage,
                SegmentedSequenceParameters.ReplaceProtein);
        }

        /// <summary>
        /// Validate error while  replacing sequence item with null value 
        /// using RaplaceRange() method.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation an exception by passing null char to ReplaceRange().
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingNullValueToReplaceRange()
        {
            InValidateSegSeqMethods(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.NullSequenceException,
                SegmentedSequenceParameters.ReplaceRangeNull);
        }

        /// <summary>
        /// Validate error while  replacing sequence with invalid sequence 
        /// using RaplaceRange() method.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation an exception by passing invlaid sequence to ReplaceRange().
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingInvalidSeqToReplaceRange()
        {
            InValidateSegSeqMethods(
                Constants.DnaSegSequenceForErrorValidationNode, Constants.SequenceSymbolException,
                SegmentedSequenceParameters.ReplaceRange);
        }

        /// <summary>
        /// Validate Whether RNA Item contains in DNA segmented sequence 
        /// using contains() method.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validate whether RNA Item contains in DNA segmented sequence .
        /// </summary>
        [Test]
        public void ValidateOuptuByPassingRnaSeqItemToDnaSegSeqContainsMethod()
        {
            bool result = false;

            // Create segmented sequence.with IsReadOnly property set to true.
            SegmentedSequence segmentedSeq = CreateSegmentedSequence(
                Constants.DnaSegSequenceForErrorValidationNode, Alphabets.DNA, Alphabets.DNA, false);

            result = segmentedSeq.Contains(Alphabets.RNA.U);

            // Validate if contains() return false.
            Assert.IsFalse(result);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                result));
        }

        /// <summary>
        /// Invalidate Segmented sequence constructor with invalid input
        /// values.
        /// Input : Invalid ctor parameters
        /// Output : Invalidate segmented seq ctors
        /// </summary>
        [Test]
        public void InvalidateSegmentedSeqCtors()
        {
            // Create a sequence.
            ISequence seq = new Sequence(Alphabets.DNA);
            ISequence seq1 = null;

            ICollection<ISequence> nullSeqs = null;
            ICollection<ISequence> seqList = new Collection<ISequence>();
            seqList.Add(seq);
            seqList.Add(seq1);

            string exceptionMessage = null;
            try
            {
                new SegmentedSequence(null as ISequence);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                exceptionMessage = ex.Message;

                // Log to Nunit GUI.
                Console.WriteLine(string.Format(null,
                    "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                    exceptionMessage));
                ApplicationLog.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                exceptionMessage));
            }

            try
            {
                new SegmentedSequence(null as ICollection<ISequence>);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                exceptionMessage = ex.Message;

                // Log to Nunit GUI.
                Console.WriteLine(string.Format(null,
                    "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                    exceptionMessage));
                ApplicationLog.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                exceptionMessage));
            }


            // Validate by passing null Sequence.
            try
            {
                new SegmentedSequence(nullSeqs);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                exceptionMessage = ex.Message;

                // Log to Nunit GUI.
                Console.WriteLine(string.Format(null,
                    "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                    exceptionMessage));
                ApplicationLog.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                exceptionMessage));
            }

            // Validate by passing null Sequence in Seqlist.
            try
            {
                new SegmentedSequence(seqList);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                exceptionMessage = ex.Message;

                // Log to Nunit GUI.
                Console.WriteLine(string.Format(null,
                    "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                    exceptionMessage));
                ApplicationLog.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                exceptionMessage));
            }
        }

        /// <summary>
        /// Invalidate Segmented sequence GetObjectData method.
        /// Input : null 
        /// Output : Validation of segmented sequence
        /// </summary>
        [Test]
        public void InvalidateGetObjectData()
        {
            string firstInputSeq = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceForErrorValidationNode,
                Constants.FirstSequence);

            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            // Create a sequence.
            ISequence seq = new Sequence(Alphabets.DNA,
                firstInputSeq);
            SegmentedSequence segSeq = new SegmentedSequence(seq);

            // Invalidate GetObjectData
            try
            {
                segSeq.GetObjectData(null, context);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(string.Format(null,
                    "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format(null,
                    "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                    ex.Message));
            }

        }

        /// <summary>
        /// Invalidate Segmented sequence IndexOfNonGap characters.
        /// Input : Segmented sequence with Gap characters 
        /// Output : -1
        /// </summary>
        [Test]
        public void InvalidateIndexOfNonGap()
        {
            string sequenceString = Utility._xmlUtil.GetTextValue(
                Constants.SequenceWithGapCharsNode, Constants.ExpectedSequence);

            // Create a sequence.
            ISequence seq = new Sequence(Alphabets.DNA,
                sequenceString);
            SegmentedSequence segSeq = new SegmentedSequence(seq);

            int result = segSeq.IndexOfNonGap();

            Assert.AreEqual(-1, result);

            try
            {
                segSeq.IndexOfNonGap(-10);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(string.Format(null,
                    "Segmented Sequence P2: Validated Segmented sequence exception {0}",
                    ex.Message));
            }

            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Validated Segmented sequence IndexOfNonGap characters"));
            ApplicationLog.WriteLine(string.Format(null,
                "Segmented Sequence P2: Validated Segmented sequence IndexOfNonGap characters"));
        }

        /// <summary>
        /// Invalidate Segmented sequence LastindexOfNonGap characters.
        /// Input : Segmented sequence with Gap characters 
        /// Output : -1
        /// </summary>
        [Test]
        public void InvalidateLastIndexOfNonGap()
        {
            string sequenceString = Utility._xmlUtil.GetTextValue(
                Constants.SequenceWithGapCharsNode, Constants.ExpectedSequence);

            // Create a sequence.
            ISequence seq = new Sequence(Alphabets.DNA,
                sequenceString);
            SegmentedSequence segSeq = new SegmentedSequence(seq);

            int result = segSeq.LastIndexOfNonGap();

            Assert.AreEqual(-1, result);

            try
            {
                segSeq.LastIndexOfNonGap(-10);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(string.Format(null,
                    "Segmented Sequence P2: Validated Segmented sequence exception {0}",
                    ex.Message));
            }

            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Validated Segmented sequence IndexOfNonGap characters"));
            ApplicationLog.WriteLine(string.Format(null,
                "Segmented Sequence P2: Validated Segmented sequence IndexOfNonGap characters"));
        }

        /// <summary>
        /// Validate an error by passing invalid values to Remove(SeqItem) method.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation an exception by passing invalid values to Remove(SeqItem).
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingInvalidSeqItemValueToRemove()
        {
            InValidateSegSeqMethods(Constants.DnaSegSequenceForErrorValidationNode,
                Constants.InvalidStartIndex, SegmentedSequenceParameters.Remove);
        }

        /// <summary>
        /// Validate an error by passing invalid values to InsertRange(pos;sequence) method.
        /// Input Data : Valid two Dna Sequences.
        /// Output Data : Validation an exception by passing invalid values to InsertRange().
        /// </summary>
        [Test]
        public void ValidateExceptionByPassingInvalidSequenceToInsertRange()
        {
            InValidateSegSeqMethods(Constants.DnaSegSequenceForErrorValidationNode,
                Constants.InvalidStartIndex, SegmentedSequenceParameters.InsertRange);
        }
        #endregion Segmented Sequence P2 TestCases

        # region Supporting methods

        /// <summary>
        /// Validate an exception by passing different alphabet sequences 
        /// to segmented sequence constructor.
        /// </summary>
        /// <param name="nodeName" >Name of the error message node </param>
        static void ValidateDifferentAlphabetSegSequence(
            string nodeName, IAlphabet alphabet1, IAlphabet alphabet2)
        {
            string firstInputSeq = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FirstSequence);
            string secondInputSeq = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SecondSegSequence);
            string expectedErrorMessage = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetException);
            List<ISequence> seqList = new List<ISequence>();
            SegmentedSequence segmentedSeq = null;
            string actualError = string.Empty;

            // Create a seqeunce.
            ISequence seq1 = new Sequence(alphabet1, firstInputSeq);
            ISequence seq2 = new Sequence(alphabet2, secondInputSeq);

            // Add seqeunce to sequenceList
            seqList.Add(seq1);
            seqList.Add(seq2);

            // Try creating segmented seq with Dna and Rna seq.
            try
            {
                segmentedSeq = new SegmentedSequence(seqList);
            }
            catch (ArgumentException ex)
            {
                actualError = ex.Message;
            }

            // Validate an exception.
            Assert.AreEqual(actualError.ToLower(CultureInfo.CurrentCulture),
                expectedErrorMessage.ToLower(CultureInfo.CurrentCulture)); ;
            Assert.IsNull(segmentedSeq);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "SparseSequence P2: SparseSequence Exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// Create a segmented sequence with two different sequences.
        /// </summary>
        /// <param name="nodeName" >Name of the error message node </param>
        private SegmentedSequence CreateSegmentedSequence(
            string nodeName, IAlphabet alphabet1, IAlphabet alphabet2, bool readOnly)
        {
            string dnaInputSeq = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FirstSequence);
            string rnaInputSeq = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SecondSegSequence);
            List<ISequence> seqList = new List<ISequence>();
            SegmentedSequence segmentedSeq = null;

            // Create a seqeunce.
            Sequence seq1 = new Sequence(alphabet1, dnaInputSeq);
            Sequence seq2 = new Sequence(alphabet2, rnaInputSeq);

            // Set Sequence IsReadOnly to true
            if (readOnly)
            {
                seq1.IsReadOnly = true;
                seq2.IsReadOnly = true;
            }
            else
            {
                seq1.IsReadOnly = false;
                seq2.IsReadOnly = false;
            }

            // Add seqeunce to sequenceList
            seqList.Add(seq1);
            seqList.Add(seq2);

            // Create a segmented sequence.
            segmentedSeq = new SegmentedSequence(seqList);

            return segmentedSeq;
        }

        /// <summary>
        /// Validate an exception by passing Invalid values 
        /// to segmented sequence methods.
        /// </summary>
        /// <param name="nodeName" >Name of the error message node </param>
        private void InValidateSegSeqMethods(
            string nodeName, string exceptionMessage, SegmentedSequenceParameters methodName)
        {
            // Get values from xml.
            string expectedErrorMessage = Utility._xmlUtil.GetTextValue(
                nodeName, exceptionMessage);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;

            // Create segmented sequence.with IsReadOnly property set to true.
            SegmentedSequence segmentedSeq = CreateSegmentedSequence(
                nodeName, Alphabets.DNA, Alphabets.DNA, false);
            SegmentedSequence readOnlySegSeq = CreateSegmentedSequence(
                nodeName, Alphabets.DNA, Alphabets.DNA, true);

            int count = segmentedSeq.Count;

            switch (methodName)
            {
                case SegmentedSequenceParameters.Range:
                    // Pass null value to Range method 
                    try
                    {
                        segmentedSeq.Range(-1, 30);
                        Assert.Fail();
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        actualError = e.Message;
                        // Validate an expected exception.
                        updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                        Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                            updatedActualError.ToLower(CultureInfo.CurrentCulture));
                    }

                    try
                    {
                        segmentedSeq.Range(1, -30);
                        Assert.Fail();
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        actualError = e.Message;
                        ApplicationLog.WriteLine(
                             "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                             actualError);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                    }

                    try
                    {
                        segmentedSeq.Range(1, 300);
                        Assert.Fail();
                    }
                    catch (ArgumentException e)
                    {
                        actualError = e.Message;
                        ApplicationLog.WriteLine(
                             "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                             actualError);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                    }

                    break;
                case SegmentedSequenceParameters.RemoveAt:
                    // Validate Modify Readonly Segmented sequence.
                    try
                    {
                        readOnlySegSeq.RemoveAt(1);
                        Assert.Fail();
                    }
                    catch (InvalidOperationException ex)
                    {
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            ex.Message);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            ex.Message);
                    }

                    // Pass invalid value (Greater than the count of segmented sequence) to RemoveAt().
                    try
                    {
                        segmentedSeq.RemoveAt(count + 10);
                        Assert.Fail();
                    }
                    catch (ArgumentException e)
                    {
                        actualError = e.Message;
                        // Validate an expected exception.
                        updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                        Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                            updatedActualError.ToLower(CultureInfo.CurrentCulture));
                    }

                    break;
                case SegmentedSequenceParameters.RemoveRange:
                    // Pass invalid start position value (Greater than the count of 
                    // segmented sequence) to RemoveRnage().
                    try
                    {
                        segmentedSeq.RemoveRange(count + 10, 10);
                        Assert.Fail();
                    }
                    catch (ArgumentException e)
                    {
                        actualError = e.Message;
                        // Validate an expected exception.
                        updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                        Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                            updatedActualError.ToLower(CultureInfo.CurrentCulture));
                    }

                    // Remove sequence range from ReadOnly segmented sequence
                    try
                    {
                        readOnlySegSeq.RemoveRange(0, segmentedSeq.Count);
                        Assert.Fail();
                    }
                    catch (InvalidOperationException ex)
                    {
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            ex.Message);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            ex.Message);
                    }

                    // Pass invalid length.
                    try
                    {
                        segmentedSeq.RemoveRange(1, -10);
                        Assert.Fail();
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            ex.Message);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            ex.Message);
                    }

                    // Pass invalid start position.
                    try
                    {
                        segmentedSeq.RemoveRange(0, count + 20);
                        Assert.Fail();
                    }
                    catch (ArgumentException ex)
                    {
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            ex.Message);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            ex.Message);
                    }
                    break;
                case SegmentedSequenceParameters.Replace:
                    // Pass invalid char 'Z' to DNA Segmented sequence 
                    try
                    {
                        segmentedSeq.Replace(count - 1, 'Z');
                        Assert.Fail();
                    }
                    catch (ArgumentException e)
                    {
                        actualError = e.Message;
                        // Validate an expected exception.
                        updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                        Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                            updatedActualError.ToLower(CultureInfo.CurrentCulture));
                    }
                    break;
                case SegmentedSequenceParameters.ReplaceNull:
                    // Pass null value to DNA Segmented sequence 
                    try
                    {
                        segmentedSeq.Replace(count - 1, null);
                        Assert.Fail();
                    }
                    catch (ArgumentNullException e)
                    {
                        actualError = e.Message;
                        // Validate an expected exception.
                        updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                        Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                            updatedActualError.ToLower(CultureInfo.CurrentCulture));
                    }

                    // Replace sequence item from ReadOnly Segmented sequence.
                    try
                    {
                        readOnlySegSeq.Replace(count - 1, 'C');
                        Assert.Fail();
                    }
                    catch (InvalidOperationException e)
                    {
                        actualError = e.Message;
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                    }

                    // Pass Invalid position value.
                    try
                    {
                        segmentedSeq.Replace(-1, 'C');
                        Assert.Fail();
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        actualError = e.Message;
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                    }

                    break;
                case SegmentedSequenceParameters.ReplaceRna:
                    // Pass Rna sequence item to replace Dna segmented seq item 
                    try
                    {
                        segmentedSeq.Replace(count - 1, Alphabets.RNA.U);
                        Assert.Fail();
                    }
                    catch (ArgumentException e)
                    {
                        actualError = e.Message;
                        // Validate an expected exception.
                        updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                        Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                            updatedActualError.ToLower(CultureInfo.CurrentCulture));
                    }

                    // Replace sequence item from ReadOnly Segmented sequence.
                    try
                    {
                        readOnlySegSeq.Replace(count - 1, segmentedSeq[0]);
                        Assert.Fail();
                    }
                    catch (InvalidOperationException e)
                    {
                        actualError = e.Message;
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                    }

                    // Pass Invalid position value.
                    try
                    {
                        segmentedSeq.Replace(-1, segmentedSeq[0]);
                        Assert.Fail();
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        actualError = e.Message;
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                    }

                    break;
                case SegmentedSequenceParameters.ReplaceProtein:
                    // Pass Protein sequence item to replace Dna segmented seq item 
                    try
                    {
                        segmentedSeq.Replace(count - 1, Alphabets.Protein.Ile);
                        Assert.Fail();
                    }
                    catch (ArgumentException e)
                    {
                        actualError = e.Message;
                        // Validate an expected exception.
                        updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                        Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                            updatedActualError.ToLower(CultureInfo.CurrentCulture));
                    }
                    break;
                case SegmentedSequenceParameters.ReplaceRangeNull:
                    // Pass null string to Segmented sequence 
                    try
                    {
                        segmentedSeq.ReplaceRange(count - 4, null);
                        Assert.Fail();
                    }
                    catch (ArgumentNullException e)
                    {
                        actualError = e.Message;
                        // Validate an expected exception.
                        updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                        Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                            updatedActualError.ToLower(CultureInfo.CurrentCulture));
                    }

                    // Replace range of sequence from ReadOnly sequence
                    try
                    {
                        readOnlySegSeq.ReplaceRange(count - 1, "AC");
                        Assert.Fail();
                    }
                    catch (InvalidOperationException e)
                    {
                        actualError = e.Message;
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                    }
                    break;
                case SegmentedSequenceParameters.ReplaceRange:
                    // Pass invalid string to Segmented sequence 
                    try
                    {
                        segmentedSeq.ReplaceRange(count - 4, "Z");
                        Assert.Fail();
                    }
                    catch (ArgumentException e)
                    {
                        actualError = e.Message;
                        // Validate an expected exception.
                        updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                        Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                            updatedActualError.ToLower(CultureInfo.CurrentCulture));
                    }
                    break;
                case SegmentedSequenceParameters.Remove:

                    // Pass ReadOnly seq to Remove method and validate an exception 
                    try
                    {
                        readOnlySegSeq.Remove(readOnlySegSeq[0]);
                        Assert.Fail();
                    }
                    catch (InvalidOperationException e)
                    {
                        actualError = e.Message;
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                    }
                    break;
                case SegmentedSequenceParameters.InsertRange:

                    // Insert sequence range to ReadOnly seg sequence.
                    try
                    {
                        readOnlySegSeq.InsertRange(0, "AGT");
                        Assert.Fail();
                    }
                    catch (InvalidOperationException e)
                    {
                        actualError = e.Message;
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                    }

                    // Pass a null sequence
                    try
                    {
                        segmentedSeq.InsertRange(0, null);
                        Assert.Fail();
                    }
                    catch (ArgumentNullException e)
                    {
                        actualError = e.Message;
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                    }

                    // Pass invalid start position
                    try
                    {
                        segmentedSeq.InsertRange(-23, "AGT");
                        Assert.Fail();
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        actualError = e.Message;
                        ApplicationLog.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                        Console.WriteLine(
                            "DerivedSequenceP2TestCases : Successfully validated the exception {0} for Range() method",
                            actualError);
                    }
                    break;
                default:
                    break;
            }

            // Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                actualError));
        }

        #endregion Supporting methods
    }
}
