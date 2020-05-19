// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * QualitativeSequenceP2TestCases.cs
 * 
 * This file contains the Qualitative Sequence P2 test case validation.
 * 
******************************************************************************/

using System;
using System.Globalization;
using System.Text;

using MBF.Encoding;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MBF.IO;
using MBF.IO.FastQ;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Qualitative Sequence P2 level validations.
    /// </summary>
    [TestClass]
    public class QualitativeSequenceP2TestCases
    {

        #region Enums

        /// <summary>
        /// Qualitative Sequence method Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum QualitativeSequenceParameters
        {
            FormatType,
            ByteArray,
            InvalidEncoding,
            Add,
            Remove,
            Replace,
            Clear,
            Insert,
            Alphabets,
            Sequence,
            Score,
            Ncbi2NA,
            Ncbi,
            Ncbi4NA,
            IupacNA,
            NcbiEAA,
            EncoderWithScore,
            Encoder,
            EncoderWithByteValue,
            IupacNAWithScore,
            Default
        };

        /// <summary>
        /// Qualitative sequence format type parameters.
        /// </summary>
        enum QualitativeSeqFormatTypePam
        {
            SangerToIllumina,
            SangerToSolexa,
            SolexaToSanger,
            SolexaToIllumina,
            IlluminaToSanger,
            IlluminaToSolexa,
            Default
        };

        # endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\QualitativeTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static QualitativeSequenceP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        # region Qualitative Sequence P2 TestCases

        /// <summary>
        /// Invalidate Qualsequence with null alphabet
        /// Input Data : Null Sequence.
        /// Output Data : Validation of Exception by passing null value.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateQualSequenceWithNullValue()
        {
            // Get values from xml.
            string expectedErrorMessage = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.AlphabetNullExceptionNode);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;
            QualitativeSequence qualSeq = null;

            //create Qualitative sequence by passing null value.
            try
            {
                qualSeq = new QualitativeSequence(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;

                // Validate an expected exception.
                updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                    updatedActualError.ToLower(CultureInfo.CurrentCulture));
                Assert.IsNull(qualSeq);
            }

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2: Qualitative Sequence Null exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        /// Invalidate Qualsequence with empty sequence.
        /// Input Data : Empty Sequence.
        /// Output Data : Validation of Exception by passing empty sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateQualSequenceWithEmptySequence()
        {
            QualitativeSequence qualSeq = new QualitativeSequence(
                Alphabets.DNA, FastQFormatType.Sanger, "");

            Assert.IsNotNull(qualSeq);

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2: Qualitative empty Sequence was validated successfully {0}",
                qualSeq));
        }

        /// <summary>
        /// QualSequence  of FastQ Format "Sanger" with invalid score.
        /// Input Data : Valid Dna sanger Sequence, Invalid quality score.
        /// Output Data : Validate Exception by passing invalid quality score.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateSangerQualSequenceWithInvalidQualScore()
        {
            InValidateQualSequence(Constants.SimpleDnaSangerNode,
                Constants.InvalidQualityScore, QualitativeSequenceParameters.FormatType);
        }

        /// <summary>
        /// QualSequence of FastQ Format "Illumina" with invalid score.
        /// Input Data : Valid Dna Illumina Sequence, Invalid quality score.
        /// Output Data : Validate Exception by passing invalid quality score.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateIlluminaQualSequenceWithInvalidQualScore()
        {
            InValidateQualSequence(Constants.SimpleDnaIlluminaNode,
                Constants.InvalidQualityScore, QualitativeSequenceParameters.FormatType);
        }

        /// <summary>
        /// QualSequence of FastQ Format "Solexa" with invalid score.
        /// Input Data : Valid Dna Solexa Sequence, Invalid quality score.
        /// Output Data : Validate Exception by passing invalid quality score.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateSolexaQualSequenceWithInvalidQualScore()
        {
            InValidateQualSequence(Constants.SimpleDnaSolexaNode,
                Constants.InvalidQualityScore, QualitativeSequenceParameters.FormatType);
        }

        /// <summary>
        /// QualSequence  of FastQ Format "Sanger" with few invalid score
        /// in byte array.
        /// Input Data : Valid Dna sanger Sequence, Invalid quality score.
        /// Output Data : Validate Exception by passing invalid quality score.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateSangerQualSequenceWithInvalidByteArrayScore()
        {
            InValidateQualSequence(Constants.SimpleDnaSangerNode,
                Constants.InvalidByteQualScore, QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// QualSequence  of FastQ Format "Solexa" with few invalid score
        /// in byte array.
        /// Input Data : Valid Dna Solexa Sequence, Invalid quality score.
        /// Output Data : Validate Exception by passing invalid quality score.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateSolexaQualSequenceWithInvalidByteArrayScore()
        {
            InValidateQualSequence(Constants.SimpleDnaSolexaNode,
               Constants.InvalidByteQualScore, QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// QualSequence  of FastQ Format "Illumina" with few invalid score
        /// in byte array.
        /// Input Data : Valid Dna Illumina Sequence, Invalid quality score.
        /// Output Data : Validate Exception by passing invalid quality score.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateIlluminaQualSequenceWithInvalidByteArrayScore()
        {
            InValidateQualSequence(Constants.SimpleDnaIlluminaNode,
                Constants.InvalidByteQualScore, QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// QualSequence  of FastQ Format "Sanger" with invalid Encoding type.
        /// Input Data : Valid Dna Illumina Sequence, Invalid Encoding type.
        /// Output Data : Validate Exception by passing invalid Encoding .
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateSangerQualSequenceWithInvalidEncoding()
        {
            InValidateQualSequence(Constants.SimpleDnaSangerNode,
                Constants.EncodingError, QualitativeSequenceParameters.InvalidEncoding);
        }

        /// <summary>
        /// Invalidate Qualsequence with invalid characters in the sequence.
        /// Input Data : Null Sequence.
        /// Output Data : Validation of Exception by passing seq with invalid characters.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateQualSequenceWithInvalidChars()
        {
            // Get values from xml.
            string expectedErrorMessage = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.InvalidAlphabetErrorMessage);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;
            QualitativeSequence qualSeq = null;

            //Try creating Qualitative sequence by passing invalid seq chars.
            try
            {
                qualSeq = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Sanger, "AGTZ");
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                actualError = ex.Message;
                // Validate an expected exception.
                updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                    updatedActualError.ToLower(CultureInfo.CurrentCulture));
                Assert.IsNull(qualSeq);
            }

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2: Qualitative Sequence Null exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        /// Validate Add() with IsReadOnly True
        /// Input Data : Qual Sequence.
        /// Output Data : Validate an exception by modifying ReadOnly Qual
        /// Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateReadOnlyQualSequence()
        {
            // Create a Dna Sanger Qualitative Sequence.
            QualitativeSequence createdQualSeq =
                CreateQualitativeSequence(Constants.SimpleDnaSangerNode);

            // Set Qual sequence to ReadOnly.
            createdQualSeq.IsReadOnly = true;

            // Validate an excception by modifying ReadOnly Qual Sequence.
            InValidateQualSequenceGenearlMethods(Constants.SimpleDnaSangerNode,
                Constants.ReadOnlyException, createdQualSeq, QualitativeSequenceParameters.Add);
        }

        /// <summary>
        /// Validate Remove() with IsReadOnly True
        /// Input Data : Qual Sequence.
        /// Output Data : Validate an exception by modifying ReadOnly Qual
        /// Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateRemovingSeqItemFromReadOnlyQualSequence()
        {
            // Create a Dna Sanger Qualitative Sequence.
            QualitativeSequence createdQualSeq =
                CreateQualitativeSequence(Constants.SimpleDnaSangerNode);

            // Set Qual sequence to ReadOnly.
            createdQualSeq.IsReadOnly = true;

            // Validate an excception by modifying ReadOnly Qual Sequence.
            InValidateQualSequenceGenearlMethods(Constants.SimpleDnaSangerNode,
                Constants.ReadOnlyException, createdQualSeq, QualitativeSequenceParameters.Remove);
        }

        /// <summary>
        /// Validate Replace() with IsReadOnly True
        /// Input Data : Qual Sequence.
        /// Output Data : Validate an exception by replacing ReadOnly Qual
        /// Sequence.item.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateReplacingSeqItemWithReadOnlyQualSequenceItem()
        {
            // Create a Dna Sanger Qualitative Sequence.
            QualitativeSequence createdQualSeq =
                CreateQualitativeSequence(Constants.SimpleDnaSangerNode);

            // Set Qual sequence to ReadOnly.
            createdQualSeq.IsReadOnly = true;

            // Validate an excception by modifying ReadOnly Qual Sequence.
            InValidateQualSequenceGenearlMethods(Constants.SimpleDnaSangerNode,
                Constants.ReadOnlyException, createdQualSeq, QualitativeSequenceParameters.Replace);
        }

        /// <summary>
        /// Validate Insert() with IsReadOnly True
        /// Input Data : Qual Sequence.
        /// Output Data : Validate an exception by inserting seqeunce item 
        /// to ReadOnly Qual Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateInsertinggSeqItemToReadOnlyQualSequence()
        {
            // Create a Dna Sanger Qualitative Sequence.
            QualitativeSequence createdQualSeq =
                CreateQualitativeSequence(Constants.SimpleDnaSangerNode);

            // Set Qual sequence to ReadOnly.
            createdQualSeq.IsReadOnly = true;

            // Validate an excception by modifying ReadOnly Qual Sequence.
            InValidateQualSequenceGenearlMethods(Constants.SimpleDnaSangerNode,
                Constants.ReadOnlyException, createdQualSeq, QualitativeSequenceParameters.Replace);
        }

        /// <summary>
        /// Validate Clear() with IsReadOnly True
        /// Input Data : Qual Sequence.
        /// Output Data : Validate an exception by deleting seqeunce item 
        /// of ReadOnly Qual Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateDeletingSeqItemToReadOnlyQualSequence()
        {
            // Create a Dna Sanger Qualitative Sequence.
            QualitativeSequence createdQualSeq =
                CreateQualitativeSequence(Constants.SimpleDnaSangerNode);

            // Set Qual sequence to ReadOnly.
            createdQualSeq.IsReadOnly = true;

            // Validate an excception by modifying ReadOnly Qual Sequence.
            InValidateQualSequenceGenearlMethods(Constants.SimpleDnaSangerNode,
                Constants.ReadOnlyException, createdQualSeq, QualitativeSequenceParameters.Clear);
        }

        /// <summary>
        /// Validate Reverse()of Dna Qualitative Sequence.
        /// Input Data : Qual Sequence.
        /// Output Data : Reverse of an Qualitative Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateReverseOfDnaQualSeq()
        {
            // Get Values from xml node.
            string reversedSeq = _utilityObj._xmlUtil.GetTextValue(
               Constants.SimpleDnaSangerNode, Constants.ReverseQualSeq);

            // Create a Dna Sanger Qualitative Sequence.
            QualitativeSequence createdQualSeq =
                CreateQualitativeSequence(Constants.SimpleDnaSangerNode);

            // Validate an Reverse of Qual Sequence.
            ISequence reverseQual = createdQualSeq.Reverse;
            Assert.AreEqual(reversedSeq, reverseQual.ToString());

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2: Reverse of Qualitative Sequence {0}",
                reverseQual));
        }

        /// <summary>
        /// Validate Reverse()of Rna Qualitative Sequence.
        /// Input Data : Qual Sequence.
        /// Output Data : Reverse of an Rna Qualitative Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateReverseOfRnaQualSeq()
        {
            // Get Values from xml node.
            string reversedSeq = _utilityObj._xmlUtil.GetTextValue(
               Constants.SimpleRnaSangerNode, Constants.ReverseQualSeq);

            // Create a Dna Sanger Qualitative Sequence.
            QualitativeSequence createdQualSeq =
                CreateQualitativeSequence(Constants.SimpleRnaSangerNode);

            // Validate an Reverse of Qual Sequence.
            ISequence reverseQual = createdQualSeq.Reverse;
            Assert.AreEqual(reversedSeq, reverseQual.ToString());

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2: Reverse of Qualitative Sequence {0}",
                reverseQual));
        }

        /// <summary>
        /// Validate Reverse()of Protein Qualitative Sequence.
        /// Input Data : Qual Sequence.
        /// Output Data : Reverse of an Protein Qualitative Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateReverseOfProteinQualSeq()
        {
            // Get Values from xml node.
            string reversedSeq = _utilityObj._xmlUtil.GetTextValue(
               Constants.SimpleProteinSangerNode, Constants.ReverseQualSeq);

            // Create a Dna Sanger Qualitative Sequence.
            QualitativeSequence createdQualSeq =
                CreateQualitativeSequence(Constants.SimpleProteinSangerNode);

            // Validate an Reverse of Protein Qual Sequence.
            ISequence reverseQual = createdQualSeq.Reverse;
            Assert.AreEqual(reversedSeq, reverseQual.ToString());

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2: Reverse of Qualitative Sequence {0}",
                reverseQual));
        }

        /// <summary>
        /// Validate Complement of Dna Qualitative Sequence.
        /// Input Data : Qual Sequence.
        /// Output Data : Complement of an Qualitative Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateComplementOfDnaQualSeq()
        {
            // Get Values from xml node.
            string complementSeq = _utilityObj._xmlUtil.GetTextValue(
               Constants.SimpleDnaSangerNode, Constants.ComplementQualSeqNode);

            // Create a Dna Sanger Qualitative Sequence.
            QualitativeSequence createdQualSeq =
                CreateQualitativeSequence(Constants.SimpleDnaSangerNode);

            // Validate Reverse of Qual Sequence.
            ISequence complementQual = createdQualSeq.Complement;
            Assert.AreEqual(complementSeq, complementQual.ToString());

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2: complement Qual of Qualitative Sequence {0}",
                complementQual));
        }

        /// <summary>
        /// Validate Reverse()of Rna Qualitative Sequence.
        /// Input Data : Qual Sequence.
        /// Output Data : Reverse of an Rna Qualitative Sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateComplementOfRnaQualSeq()
        {
            // Get Values from xml node.
            string complementSeq = _utilityObj._xmlUtil.GetTextValue(
               Constants.SimpleRnaSangerNode, Constants.ComplementQualSeqNode);

            // Create a Dna Sanger Qualitative Sequence.
            QualitativeSequence createdQualSeq =
                CreateQualitativeSequence(Constants.SimpleRnaSangerNode);

            // Validate complement of Qual Sequence.
            ISequence complementQual = createdQualSeq.Complement;
            Assert.AreEqual(complementSeq, complementQual.ToString());

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2: complement Qual of Qualitative Sequence {0}",
                complementQual));
        }

        /// <summary>
        /// Validate Exception when try complement Protein Qual sequence..
        /// Input Data : Qual Sequence.
        /// Output Data : Exception while getting Protein complement for Qual Sequence..
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateComplementOfProteinQualSeq()
        {
            // Get values from xml.
            string expectedErrorMessage = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleProteinSangerNode, Constants.ComplementException);
            string actualError = string.Empty;
            ISequence seq = null;

            // Create a Dna Sanger Qualitative Sequence.
            QualitativeSequence createdQualSeq = CreateQualitativeSequence(
                Constants.SimpleProteinSangerNode);

            // Try getting commplement of Protein sequences.
            try
            {
                seq = createdQualSeq.Complement;
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                actualError = ex.Message;
                // Validate an expected exception.
                Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                    actualError.ToLower(CultureInfo.CurrentCulture));
            }

            Assert.IsNull(seq);
            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2: Reverse of Qualitative Sequence {0}",
                actualError));
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Medium size less than 100KB Dna Sequence
        /// with Solexa FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateSolexaFormatTypeMediumSizeDnaQualitativeSequence()
        {
            GeneralQualitativeSequence(Constants.MediumSizeDNASolexaNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Medium size less than 100KB Dna Sequence
        /// with Solexa FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat, NCBINA Encoding.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateSolexaFormatTypeMediumSizeDnaQualitativeSequenceWithNCBI4NAEncoding()
        {
            GeneralQualitativeSequence(Constants.MediumSizeDNASolexaNode,
                QualitativeSequenceParameters.Ncbi4NA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Medium size less than 100KB Dna Sequence
        /// with Solexa FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat, NCBI4NA Encoding.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateSangerFormatTypeMediumSizeDnaQualitativeSequence()
        {
            GeneralQualitativeSequence(Constants.MediumSizeDNASangerNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Medium size less than 100KB Dna Sequence
        /// with Sanger FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat, NCBI4NA Encoding.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateSangerFormatTypeMediumSizeDnaQualitativeSequenceWithNCBI4NAEncoding()
        {
            GeneralQualitativeSequence(Constants.MediumSizeDNASangerNode,
                QualitativeSequenceParameters.Ncbi4NA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Medium size less than 100KB Dna Sequence
        /// with Illumina FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat, NCBI4NA Encoding.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateIlluminaFormatTypeMediumSizeDnaQualitativeSequence()
        {
            GeneralQualitativeSequence(Constants.MediumSizeDNAIlluminaNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Medium size less than 100KB Dna Sequence
        /// with Illumina FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat, NCBI4NA Encoding.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateIlluminaFormatTypeMediumSizeDnaQualitativeSequenceWithNCBI4NAEncoding()
        {
            GeneralQualitativeSequence(Constants.MediumSizeDNAIlluminaNode,
                QualitativeSequenceParameters.Ncbi4NA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Large size greater than 
        /// 100KB Dna Sequence with Solexa FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateSolexaFormatTypeLargeSizeDnaQualitativeSequence()
        {
            GeneralQualitativeSequence(Constants.LargeSizeDNASolexaNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Large size greater than
        /// 100KB Dna Sequence with Solexa FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat, NCBINA Encoding.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateSolexaFormatTypeLargeSizeDnaQualitativeSequenceWithNCBI4NAEncoding()
        {
            GeneralQualitativeSequence(Constants.LargeSizeDNASolexaNode,
                QualitativeSequenceParameters.Ncbi4NA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Large size greater than 
        /// 100KB Dna Sequence with Illumina FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateIlluminaFormatTypeLargeSizeDnaQualitativeSequence()
        {
            GeneralQualitativeSequence(Constants.LargeSizeDNAIlluminaNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Large size greater than
        /// 100KB Dna Sequence with Illumina FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat, NCBINA Encoding.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateIlluminaFormatTypeLargeSizeDnaQualitativeSequenceWithNCBI4NAEncoding()
        {
            GeneralQualitativeSequence(Constants.LargeSizeDNAIlluminaNode,
                QualitativeSequenceParameters.Ncbi4NA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Large size greater than 
        /// 100KB Dna Sequence with Sanger FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateSangerFormatTypeLargeSizeDnaQualitativeSequence()
        {
            GeneralQualitativeSequence(Constants.LargeSizeDNASangerNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Large size greater than
        /// 100KB Dna Sequence with Sanger FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat, NCBINA Encoding.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateSangerFormatTypeLargeSizeDnaQualitativeSequenceWithNCBI4NAEncoding()
        {
            GeneralQualitativeSequence(Constants.LargeSizeDNASangerNode,
                QualitativeSequenceParameters.Ncbi4NA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Very Large size greater 
        /// than 200KB Dna Sequence with Illumina FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateIlluminaFormatTypeVeryLargeSizeDnaQualitativeSequence()
        {
            GeneralQualitativeSequence(Constants.VeryLargeSizeDNAIlluminaNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Very Large size greater than
        /// 200KB Dna Sequence with Sanger FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat, NCBINA Encoding.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateIlluminaFormatTypeVeryLargeSizeDnaQualitativeSequenceWithNCBI4NAEncoding()
        {
            GeneralQualitativeSequence(Constants.VeryLargeSizeDNAIlluminaNode,
                QualitativeSequenceParameters.Ncbi4NA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Very Large size greater 
        /// than 200KB Dna Sequence with Sanger FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateSangerFormatTypeVeryLargeSizeDnaQualitativeSequence()
        {
            GeneralQualitativeSequence(Constants.VeryLargeSizeDNASangerNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Very Large size greater than
        /// 200KB Dna Sequence with Sanger FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat, NCBINA Encoding.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateSangerFormatTypeVeryLargeSizeDnaQualitativeSequenceWithNCBI4NAEncoding()
        {
            GeneralQualitativeSequence(Constants.VeryLargeSizeDNASangerNode,
                QualitativeSequenceParameters.Ncbi4NA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Very Large size greater 
        /// than 200KB Dna Sequence with Solexa FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateSolexaFormatTypeVeryLargeSizeDnaQualitativeSequence()
        {
            GeneralQualitativeSequence(Constants.VeryLargeSizeDNASolexaNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Very Large size greater than
        /// 200KB Dna Sequence with Solexa FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat, NCBINA Encoding.
        /// and ByteArray score
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateSolexaFormatTypeVeryLargeSizeDnaQualitativeSequenceWithNCBI4NAEncoding()
        {
            GeneralQualitativeSequence(Constants.VeryLargeSizeDNASolexaNode,
                QualitativeSequenceParameters.Ncbi4NA);
        }

        /// <summary>
        /// Invalidate convert from Sanger to Illumnia with invalid input values.
        /// Input Data : Invalid quality scores.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateConvertSangerToIllumina()
        {
            InvalidateFormatTypeConvertion(
                QualitativeSeqFormatTypePam.SangerToIllumina);
        }

        /// <summary>
        /// Invalidate convert from Sanger to Solexa with invalid input values.
        /// Input Data : Invalid quality scores.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateConvertSangerToSolexa()
        {
            InvalidateFormatTypeConvertion(
                QualitativeSeqFormatTypePam.SangerToSolexa);
        }

        /// <summary>
        /// Invalidate convert from Solexa to Illumnia with invalid input values.
        /// Input Data : Invalid quality scores.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateConvertSolexaToIllumina()
        {
            InvalidateFormatTypeConvertion(
                QualitativeSeqFormatTypePam.SolexaToIllumina);
        }

        /// <summary>
        /// Invalidate convert from Solexa to Sanger with invalid input values.
        /// Input Data : Invalid quality scores.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateConvertSolexaToSanger()
        {
            InvalidateFormatTypeConvertion(
                QualitativeSeqFormatTypePam.SolexaToSanger);
        }

        /// <summary>
        /// Invalidate convert from Illumina to Sanger with invalid input values.
        /// Input Data : Invalid quality scores.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateConvertIlluminaToSanger()
        {
            InvalidateFormatTypeConvertion(
                QualitativeSeqFormatTypePam.IlluminaToSanger);
        }

        /// <summary>
        /// Invalidate convert from Illumina to Solexa with invalid input values.
        /// Input Data : Invalid quality scores.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateConvertIlluminaToSolexa()
        {
            InvalidateFormatTypeConvertion(
                QualitativeSeqFormatTypePam.IlluminaToSolexa);
        }

        /// <summary>
        /// Invalidate contains() method by passing invalid inputs.
        /// Input Data : Invalid quality scores and sequence Items.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateContainsSeqItems()
        {
            // Get Input values from xml config file.
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(Constants.SimpleDnaSangerNode,
                Constants.FastQFormatType));
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.inputSequenceNode);
            bool result = false;
            byte invalidScore = 02;

            // Create a qualitative sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(Alphabets.DNA,
               expectedFormatType, inputSequence);

            // Validate whether quality score present in quality list or not.
            result = qualSeq.ContainsQualityScore(invalidScore);

            Assert.IsFalse(result);

            qualSeq.IsReadOnly = false;
            // Validate whether quality score present in quality list or not.
            result = qualSeq.ContainsQualityScore(invalidScore);

            Assert.IsFalse(result);

            // Log to NUnit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2 : Validated contains method successfully"));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2 : Validated contains method successfully"));
        }

        /// <summary>
        /// Invalidate insert seq items.
        /// Input Data : Invalid input values.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateInsertSeqItems()
        {
            // Get Input values from xml config file.
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                 Constants.QualitativeSequenceInsertSeqItemNode,
                 Constants.inputSequenceNode);
            string readOnlyError = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.ReadOnlyException);
            string actualError = null;

            // Create a qual sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Sanger, inputSequence);

            // Invalidate InsertRange with read-only sequence.
            try
            {
                qualSeq.InsertRange(1, "AC", 3);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                actualError = ex.Message;
            }

            // Validate an error.
            Assert.AreEqual(readOnlyError, actualError);

            // Validate an exception for invalid quality score values.
            qualSeq.IsReadOnly = false;
            try
            {
                qualSeq.InsertRange(1, "AC", 3);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an exception for invalid sequence Item.
            qualSeq.IsReadOnly = false;
            try
            {
                qualSeq.Insert(1, null, 3);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            // Log to NUnit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2 : Validated the exception successfully"));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2 : Validated the exception successfully"));
        }

        /// <summary>
        /// Invalidate insert Range of sequences.
        /// Input Data : Invalid input values.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateInsertRangeSeq()
        {
            // Get Input values from xml config file.
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.inputSequenceNode);
            string readOnlyError = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.ReadOnlyException);
            string invalidScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.InvalidScoreErrorNode);

            IEnumerable<byte> qualScores = null;
            string actualError = null;
            string updatedError = null;

            // Create a qual sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Sanger, inputSequence);

            // Invalidate InsertRange with read-only sequence.
            try
            {
                qualSeq.InsertRange(1, "AC", qualScores);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                actualError = ex.Message;
            }

            // Validate an error.
            Assert.AreEqual(readOnlyError, actualError);

            // Validate an exception for invalid position.
            qualSeq.IsReadOnly = false;
            try
            {
                qualSeq.InsertRange(-1, "AC", qualScores);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an exception for invalid sequence.
            qualSeq.IsReadOnly = false;
            try
            {
                qualSeq.InsertRange(1, null, qualScores);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            // Validate an exception for invalid quality score.
            qualSeq.IsReadOnly = false;
            try
            {
                qualSeq.InsertRange(1, "AC", null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            updatedError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(invalidScoreError, updatedError);

            // Log to NUnit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2 : Validated the exception successfully"));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2 : Validated the exception successfully"));
        }

        /// <summary>
        /// Invalidate GetObjectData.
        /// Input Data : Invalid input values.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGetObjectData()
        {
            // Get Input values from xml config file.
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                 Constants.QualitativeSequenceInsertSeqItemNode,
                 Constants.inputSequenceNode);
            string infoNullError = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.InfoNullErrorNode);
            string actualError = null;

            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            // Create a qual sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Sanger, inputSequence);

            // Invalidate GetObjectData.
            try
            {
                qualSeq.GetObjectData(null, context);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            // Validate an error.
            string updatedError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(infoNullError, updatedError);

            // Log to NUnit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2 : Validated the exception successfully"));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2 : Validated the exception successfully"));
        }

        /// <summary>
        /// Invalidate Range  method.
        /// Input Data : Invalid input values.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateRangeWithInvalidValues()
        {
            // Get Input values from xml config file.
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.inputSequenceNode);
            string startOutOfRangeException = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.OutOfRangeExceptionNode);
            string lengthOutOfRangeException = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.LengthOutOfRangeExceptionNode);
            string actualError = null;

            // Create a qual sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Sanger, inputSequence);

            // Invalidate Range method with invalid start and length values.
            try
            {
                qualSeq.Range(2, 200);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                actualError = ex.Message;
            }

            // Invalidate Range method with invalid start value.
            try
            {
                qualSeq.Range(-10, 2);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an error.
            ValidateException(actualError, startOutOfRangeException);

            // Invalidate Range method with invalid length value.
            try
            {
                qualSeq.Range(2, -90);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an error.
            ValidateException(actualError, lengthOutOfRangeException);
        }

        /// <summary>
        /// Invalidate Replace  method.
        /// Input Data : Invalid input values.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateReplaceMethod()
        {
            // Get Input values from xml config file.
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.inputSequenceNode);
            string readOnlyException = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.ReadOnlyException);
            string invalidPostionError = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.InvalidPositionErrorNode);

            string actualError = null;

            // Create a qual sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Sanger, inputSequence);

            // Invalidate replace method with read-only sequence.
            try
            {
                qualSeq.Replace(1, qualSeq[0], 3);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                actualError = ex.Message;
            }

            try
            {
                qualSeq.Replace(1, 3);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                actualError = ex.Message;
            }

            Assert.AreEqual(actualError, readOnlyException);

            // Invalidate replace method with invalid seq item.
            qualSeq.IsReadOnly = false;
            try
            {
                qualSeq.Replace(1, null, 3);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            // Invalidate replace method with invalid quality score.
            try
            {
                qualSeq.Replace(1, 200);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Invalidate replace method with invalid position value.
            try
            {
                qualSeq.Replace(-1, 3);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            ValidateException(actualError, invalidPostionError);
        }

        /// <summary>
        /// Invalidate ReplaceRange  method.
        /// Input Data : Invalid input values.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateReplaceRangeMethod()
        {
            // Get Input values from xml config file.
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.inputSequenceNode);
            string readOnlyException = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.ReadOnlyException);
            string invalidSeqException = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.InvalidSeqNode);
            string invalidPostionError = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.InvalidPositionErrorNode);
            IEnumerable<byte> byteScore = null;
            string actualError = null;
            byte[] validByteScore = ASCIIEncoding.ASCII.GetBytes("hhh");
            byte[] invalidByteScore = ASCIIEncoding.ASCII.GetBytes("!@#$");

            // Create a qual sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Illumina, inputSequence);

            // Invalidate replace method with read-only sequence.
            try
            {
                qualSeq.ReplaceRange(1, byteScore);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                actualError = ex.Message;
            }

            try
            {
                qualSeq.ReplaceRange(1, "AC", 3);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                actualError = ex.Message;
            }

            try
            {
                qualSeq.ReplaceRange(1, "AC", byteScore);
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                actualError = ex.Message;
            }

            Assert.AreEqual(actualError, readOnlyException);

            // Invalidate replaceRange method with invalid position error.
            qualSeq.IsReadOnly = false;
            try
            {
                qualSeq.ReplaceRange(-1, "AC", 3);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            try
            {
                qualSeq.ReplaceRange(-1, byteScore);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            try
            {
                qualSeq.ReplaceRange(-1, "AC", byteScore);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            ValidateException(actualError, invalidPostionError);

            // Invalidate replaceRange method with invalid sequence error.
            qualSeq.IsReadOnly = false;
            try
            {
                qualSeq.ReplaceRange(1, null, 3);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            try
            {
                qualSeq.ReplaceRange(1, null, byteScore);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            ValidateException(actualError, invalidSeqException);

            // Invalidate replaceRange method with invalid quality score.
            qualSeq.IsReadOnly = false;
            try
            {
                qualSeq.ReplaceRange(1, "AT", 200);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Invalidate replaceRange method with sequence length >Sequence count..
            try
            {
                qualSeq.ReplaceRange(2, "ATCT", 75);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                actualError = ex.Message;
            }

            try
            {
                qualSeq.ReplaceRange(2, "ATCT", byteScore);
                Assert.Fail();
            }
            catch (NullReferenceException ex)
            {
                actualError = ex.Message;
            }

            // Invalidate ReplaceRange(Po,Seq,byteScore) with invalid input values.

            try
            {
                qualSeq.ReplaceRange(-10, "ACT", 12);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Log to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
            }

            try
            {
                qualSeq.ReplaceRange(2, "ACT", 66);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                // Log to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
            }

            // Invalidate ReplaceRange(Po,Seq,IEnumerable) with invalid input values.

            try
            {
                qualSeq.ReplaceRange(-10, "ACT", validByteScore);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Log to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
            }

            try
            {
                qualSeq.ReplaceRange(2, "ACT", validByteScore);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                // Log to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
            }

            try
            {
                qualSeq.ReplaceRange(2, "ACT", invalidByteScore);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                // Log to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
            }

            // Invalidate ReplaceRange(Pos,IEnumerable) with invalid input values.

            try
            {
                qualSeq.ReplaceRange(-10, validByteScore);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Log to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
            }

            try
            {
                qualSeq.ReplaceRange(2, validByteScore);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                // Log to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
            }

            try
            {
                qualSeq.ReplaceRange(2, invalidByteScore);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                // Log to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    ex.Message));
            }
        }

        /// <summary>
        /// Invalidate Qualitative constructor.
        /// Input Data : Invalid input values.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateQualitativeSeqCtor()
        {
            // Get Input values from xml config file.
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.inputSequenceNode);
            string QualScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.NullExceptionError);

            string actualError = null;

            QualitativeSequence seq = null;
            // Create a qual sequence.
            try
            {
                seq = new QualitativeSequence(Alphabets.DNA,
                     FastQFormatType.Sanger, Encodings.IupacNA, inputSequence, null);

            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            finally
            {
                if (seq != null)
                    ((IDisposable)seq).Dispose();
            }

            ValidateException(actualError, QualScoreError);
        }

        /// <summary>
        /// Invalidate Qualitative sequence properties.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateQualSequenceProperties()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(Constants.SimpleDnaSangerNode,
                Constants.FastQFormatType));
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.inputSequenceNode);
            string seqId = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.DisplayId);

            // Create a qualitative sequence object
            QualitativeSequence qualObj = new QualitativeSequence(alphabet,
                expectedFormatType, inputSequence);

            // Set Block size and Maximum block Size without enabling DV.
            try
            {
                qualObj.MaxNumberOfBlocks = 10;
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                // Log Error message to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2: Validated exception {0}  successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2: Validated exception {0}  successfully",
                    ex.Message));
            }

            try
            {
                qualObj.BlockSize = 10;
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                // Log Error message to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2: Validated exception {0}  successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2: Validated exception {0}  successfully",
                    ex.Message));
            }

            // Enable Data virtualization
            SequencePointer pointerObj = new SequencePointer();
            pointerObj.AlphabetName = "DNA";
            pointerObj.Id = seqId;
            pointerObj.IndexOffsets[0] = 20;
            pointerObj.IndexOffsets[1] = pointerObj.IndexOffsets[0] + 5;
            pointerObj.StartingLine = 1;

            IVirtualSequenceParser parserObj = new FastQParser();

            FileVirtualQualitativeSequenceProvider provObj =
               new FileVirtualQualitativeSequenceProvider(parserObj, pointerObj);

            qualObj.VirtualQualitativeSequenceProvider = provObj;

            // Set invalid properties
            try
            {
                qualObj.BlockSize = -5;
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                // Log Error message to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2: Validated exception {0}  successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2: Validated exception {0}  successfully",
                    ex.Message));
            }

            try
            {
                qualObj.MaxNumberOfBlocks = -5;
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                // Log Error message to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2: Validated exception {0}  successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2: Validated exception {0}  successfully",
                    ex.Message));
            }

            // Set invalid blocksize greater than start index.
            try
            {
                qualObj.BlockSize = 500;
                Assert.Fail();
            }
            catch (InvalidOperationException ex)
            {
                // Log Error message to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2: Validated exception {0}  successfully",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2: Validated exception {0}  successfully",
                    ex.Message));
            }

        }

        /// <summary>
        /// Invalidate Qualitative sequence ctor with invalid parameters.
        /// Input : Invalid qual sequence ctor.
        /// Output : Validation of Qual sequence ctor exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateQualSeqCtorWithInvalidByteScores()
        {
            ValidateQualitativeSequenceCtorException(Constants.SimpleDnaAlphabetNode,
                QualitativeSequenceParameters.EncoderWithByteValue);
        }

        /// <summary>
        /// Invalidate insert seq range items.
        /// Input Data : Invalid input values.
        /// Output Data : Validation of an expected exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateInsertRangeSeqItems()
        {
            // Get Input values from xml config file.
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                 Constants.QualitativeSequenceInsertSeqItemNode,
                 Constants.inputSequenceNode);
            string actualError = null;
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes("hhh");
            byte[] invalidByteArray = ASCIIEncoding.ASCII.GetBytes("!@#$");

            // Create a qual sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Illumina, inputSequence);

            qualSeq.IsReadOnly = false;

            // Invalidate InsertRange with invalid position.
            try
            {
                qualSeq.InsertRange(-10, "AC", byteArray);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
                // Log to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    actualError));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    actualError));
            }

            // Invalidate InsertRange with invalid qual score.
            try
            {
                qualSeq.InsertRange(1, "AC", byteArray);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                actualError = ex.Message;
                // Log to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    actualError));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    actualError));
            }

            // Invalidate InsertRange with invalid qual score.
            try
            {
                qualSeq.InsertRange(1, "ACGT", invalidByteArray);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
                // Log to NUnit GUI.
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    actualError));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P2 : Validated the exception {0} successfully",
                    actualError));
            }

        }
        #endregion QualitativeSequence P2 TestCases

        #region Supporting Methods

        /// <summary>
        /// General method to invalidate creation of Qualitative sequence
        /// with invalid qual score..
        /// <param name="nodeName">Name of the Format type xml node.</param>
        /// <param name="errorMessage">Error message xml node name.</param>
        /// <param name="qualPam">Qualitative sequence constructor paramter.</param>
        /// </summary>
        void InValidateQualSequence(string nodeName, string errorMessage,
            QualitativeSequenceParameters qualPam)
        {
            // Get values from xml.
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            string expectedErrorMessage = _utilityObj._xmlUtil.GetTextValue(
                nodeName, errorMessage);
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string actualError = string.Empty;
            string updatedActualError = string.Empty;
            QualitativeSequence qualSeq = null;
            byte[] byteArray = { 65, 64, 66, 68, 69, 67, 65, 65, 65, 65, 65,
                                   200, 3, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 65, 4 };
            switch (qualPam)
            {
                case QualitativeSequenceParameters.FormatType:
                    //Try to create Qualitative sequence by invalid Quality score
                    try
                    {
                        qualSeq = new QualitativeSequence(Alphabets.DNA, expectedFormatType,
                            inputSequence, Convert.ToByte(200));
                        Assert.Fail();
                    }
                    catch (ArgumentException ex)
                    {
                        actualError = ex.Message;
                        // Validate an expected exception.
                        updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                        Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                            updatedActualError.ToLower(CultureInfo.CurrentCulture));
                    }
                    break;
                case QualitativeSequenceParameters.ByteArray:
                    //Try to create Qualitative sequence by invalid Quality score
                    try
                    {
                        qualSeq = new QualitativeSequence(Alphabets.DNA, expectedFormatType,
                            inputSequence, byteArray);
                        Assert.Fail();
                    }
                    catch (ArgumentException ex)
                    {
                        actualError = ex.Message;
                        // Validate an expected exception.
                        updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                        Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                            updatedActualError.ToLower(CultureInfo.CurrentCulture));
                    }
                    break;
                case QualitativeSequenceParameters.InvalidEncoding:
                    // Try to create Qualitative sequence by invalid Encoding(Pass AminoAcid 
                    // encoding to Nucleaotide sequence
                    try
                    {
                        qualSeq = new QualitativeSequence(Alphabets.DNA, expectedFormatType,
                            Encodings.NcbiEAA, inputSequence, Convert.ToByte(65));
                        Assert.Fail();
                    }
                    catch (ArgumentException ex)
                    {
                        actualError = ex.Message;
                        // Validate an expected exception.
                        updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
                        Assert.AreEqual(expectedErrorMessage.ToLower(CultureInfo.CurrentCulture),
                            updatedActualError.ToLower(CultureInfo.CurrentCulture));
                    }
                    break;
                default:
                    break;
            }

            // Log to Nunit GUI.
            Assert.IsNull(qualSeq);
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2: Qualitative Sequence Invalid score exception was validated successfully {0}",
                updatedActualError));
        }

        /// <summary>
        /// General method to create a Qualitative sequence.
        /// <param name="nodeName">xml node name of diferent FastQ format.</param>
        /// </summary>
        private QualitativeSequence CreateQualitativeSequence(string nodeName)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string inputQuality = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InputByteArrayNode);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(inputQuality);

            // Create a Qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                alphabet, expectedFormatType, inputSequence, byteArray);

            return createdQualitativeSequence;
        }

        /// <summary>
        /// Invalidate Qualitative Sequence General methods
        /// Add, Remove,Replace.
        /// <param name="nodeName">Name of the Format type xml node.</param>
        /// <param name="errorMessage">Error message node name.</param>
        /// <param name="qualPam">Qualitaitve methods name.</param>
        /// <param name="qualSeq">Qualitative Sequence object</param>
        /// </summary>
        void InValidateQualSequenceGenearlMethods(string nodeName, string errorMessage,
            QualitativeSequence qualSeq, QualitativeSequenceParameters qualPam)
        {
            // Get values from xml.
            string expectedErrorMessage = _utilityObj._xmlUtil.GetTextValue(
                nodeName, errorMessage);
            string actualError = string.Empty;
            switch (qualPam)
            {
                case QualitativeSequenceParameters.Add:
                    //Try editing ReadOnly Qualitative sequence 
                    try
                    {
                        qualSeq.Add(Alphabets.DNA.A);
                        Assert.Fail();
                    }
                    catch (InvalidOperationException ex)
                    {
                        actualError = ex.Message;
                        // Validate an expected exception.
                        ValidateException(actualError, expectedErrorMessage);
                    }
                    break;
                case QualitativeSequenceParameters.Remove:
                    //Try removing ReadOnly Qualitative sequence 
                    try
                    {
                        qualSeq.Remove(Alphabets.DNA.A);
                        Assert.Fail();
                    }
                    catch (InvalidOperationException ex)
                    {
                        actualError = ex.Message;
                        ValidateException(actualError, expectedErrorMessage);
                    }
                    break;
                case QualitativeSequenceParameters.Replace:
                    //Try replacing ReadOnly Qualitative sequence 
                    try
                    {
                        qualSeq.Replace(5, 'T');
                        Assert.Fail();
                    }
                    catch (InvalidOperationException ex)
                    {
                        actualError = ex.Message;
                        ValidateException(actualError, expectedErrorMessage);
                    }
                    break;
                case QualitativeSequenceParameters.Insert:
                    //Try inserting ReadOnly Qualitative sequence 
                    try
                    {
                        qualSeq.Insert(10, 'A');
                        Assert.Fail();
                    }
                    catch (InvalidOperationException ex)
                    {
                        actualError = ex.Message;
                        ValidateException(actualError, expectedErrorMessage);
                    }
                    break;
                case QualitativeSequenceParameters.Clear:
                    //Try deleting ReadOnly Qualitative sequence items.
                    try
                    {
                        qualSeq.Clear();
                        Assert.Fail();
                    }
                    catch (InvalidOperationException ex)
                    {
                        actualError = ex.Message;
                        ValidateException(actualError, expectedErrorMessage);
                    }
                    break;
                default:
                    break;
            }

            // Log to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2: Qualitative Sequence exception was validated successfully {0}",
                actualError));
        }

        /// <summary>
        /// General method to validate creation of Qualitative sequence.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="parameters">Different Qualitative Sequence parameters.</param>
        /// </summary>
        void GeneralQualitativeSequence(
            string nodeName, QualitativeSequenceParameters parameters)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string expectedScore = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedScore);
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.QSequenceCount);
            string expectedMaxScore = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.MaxScoreNode);
            string inputQuality = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InputByteArrayNode);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(inputQuality);
            int index = 0;

            // Create and validate Qualitative Sequence.
            switch (parameters)
            {
                case QualitativeSequenceParameters.Ncbi4NA:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                    Encodings.Ncbi4NA, inputSequence);

                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore, Convert.ToInt32(expectedMaxScore, (IFormatProvider)null));
                    }

                    // Validate encoding,Format type and qualitative sequence.
                    Assert.IsTrue(createdQualitativeSequence.IsReadOnly);
                    Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSequence);
                    Assert.AreEqual(createdQualitativeSequence.Type, expectedFormatType);
                    Assert.AreEqual(createdQualitativeSequence.Encoding, Encodings.Ncbi4NA);
                    break;
                case QualitativeSequenceParameters.ByteArray:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                    inputSequence, byteArray);

                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore, Convert.ToInt32(byteArray[index], (IFormatProvider)null));
                        index++;
                    }
                    break;
                default:
                    break;
            }

            // Validate createdSequence qualitative sequence.
            Assert.IsNotNull(createdQualitativeSequence);
            Assert.AreEqual(createdQualitativeSequence.Alphabet, alphabet);
            Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSequence);
            Assert.AreEqual(createdQualitativeSequence.Count.ToString((IFormatProvider)null), expectedSequenceCount);
            Assert.AreEqual(createdQualitativeSequence.Scores.Length.ToString((IFormatProvider)null), expectedScore);
            Assert.AreEqual(createdQualitativeSequence.Type, expectedFormatType);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2:Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence.ToString()));

            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2:Qualitative Sequence Score {0} is as expected.",
                createdQualitativeSequence.Scores.Length.ToString((IFormatProvider)null)));

            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2:Qualitative format type {0} is as expected.",
                createdQualitativeSequence.Type));
        }

        /// <summary>
        /// Invalidate convert from one FastQ format type to other.
        /// <param name="formatTypePam">Type of the format want to be converted</param>       
        /// </summary>
        void InvalidateFormatTypeConvertion(
            QualitativeSeqFormatTypePam formatTypePam)
        {
            // Invalidate Qualitative sequence format type convertion.
            switch (formatTypePam)
            {
                case QualitativeSeqFormatTypePam.SangerToIllumina:
                    ConvertSangerToIllumina();
                    break;
                case QualitativeSeqFormatTypePam.SangerToSolexa:
                    ConvertSangerToSolexa();
                    break;
                case QualitativeSeqFormatTypePam.IlluminaToSanger:
                    ConvertIlluminaToSanger();
                    break;
                case QualitativeSeqFormatTypePam.IlluminaToSolexa:
                    ConvertIlluminaToSolexa();
                    break;
                case QualitativeSeqFormatTypePam.SolexaToIllumina:
                    ConvertSolexaToIllumina();
                    break;
                case QualitativeSeqFormatTypePam.SolexaToSanger:
                    ConvertSolexaToSanger();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Invalidate convert from Sanger to Solexa format type.
        /// </summary>
        void ConvertSangerToSolexa()
        {
            string expectedNullErrorMessage = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.NullExceptionError);
            string expectedInvalidScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.InvalidScoreErrorNode);
            string expectedInvalidQualityScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.InvalidByteScoreErrorNode);
            byte[] byteArray = { 12, 24 };
            byte qualScore = 12;
            string actualError = null;

            try
            {
                QualitativeSequence.ConvertFromSangerToSolexa(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError, expectedNullErrorMessage);

            // Validate an expected error message for invalid qual score array.
            try
            {
                QualitativeSequence.ConvertFromSangerToSolexa(byteArray);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError, expectedInvalidScoreError);

            // Validate an expected error message for invalid qual scores.
            try
            {
                QualitativeSequence.ConvertFromSangerToSolexa(qualScore);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError,
                expectedInvalidQualityScoreError);
        }

        /// <summary>
        /// Invalidate convert from Sanger to Illumina format type.
        /// </summary>
        void ConvertSangerToIllumina()
        {
            string expectedNullErrorMessage = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.NullExceptionError);
            string expectedInvalidScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.InvalidScoreErrorNode);
            string expectedInvalidQualityScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.InvalidByteScoreErrorNode);
            byte[] byteArray = { 12, 24 };
            byte qualScore = 12;
            string actualError = null;

            try
            {
                QualitativeSequence.ConvertFromSangerToIllumina(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError, expectedNullErrorMessage);

            // Validate an expected error message for invalid qual score array.
            try
            {
                QualitativeSequence.ConvertFromSangerToIllumina(byteArray);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError, expectedInvalidScoreError);

            // Validate an expected error message for invalid qual scores.
            try
            {
                QualitativeSequence.ConvertFromSangerToIllumina(qualScore);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError,
                expectedInvalidQualityScoreError);
        }

        /// <summary>
        /// Invalidate convert from Solexa to Illumina format type.
        /// </summary>
        void ConvertSolexaToIllumina()
        {
            string expectedNullErrorMessage = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.NullExceptionError);
            string expectedInvalidScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.InvalidScoreErrorNode);
            string expectedInvalidQualityScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.InvalidByteScoreErrorNode);
            byte[] byteArray = { 12, 24 };
            byte qualScore = 12;
            string actualError = null;

            try
            {
                QualitativeSequence.ConvertFromSolexaToIllumina(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError, expectedNullErrorMessage);

            // Validate an expected error message for invalid qual score array.
            try
            {
                QualitativeSequence.ConvertFromSolexaToIllumina(byteArray);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError, expectedInvalidScoreError);

            // Validate an expected error message for invalid qual scores.
            try
            {
                QualitativeSequence.ConvertFromSolexaToIllumina(qualScore);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError,
                expectedInvalidQualityScoreError);
        }

        /// <summary>
        /// Invalidate convert from Illumina to Sanger format type.
        /// </summary>
        void ConvertIlluminaToSanger()
        {
            string expectedNullErrorMessage = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.NullExceptionError);
            string expectedInvalidScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.InvalidScoreErrorNode);
            string expectedInvalidQualityScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.InvalidByteScoreErrorNode);
            byte[] byteArray = { 12, 24 };
            byte qualScore = 12;
            string actualError = null;

            try
            {
                QualitativeSequence.ConvertFromIlluminaToSanger(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError, expectedNullErrorMessage);

            // Validate an expected error message for invalid qual score array.
            try
            {
                QualitativeSequence.ConvertFromIlluminaToSanger(byteArray);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError, expectedInvalidScoreError);

            // Validate an expected error message for invalid qual scores.
            try
            {
                QualitativeSequence.ConvertFromIlluminaToSanger(qualScore);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError,
                expectedInvalidQualityScoreError);
        }

        /// <summary>
        /// Invalidate convert from Illumina to Solexa format type.
        /// </summary>
        void ConvertIlluminaToSolexa()
        {
            string expectedNullErrorMessage = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.NullExceptionError);
            string expectedInvalidScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.InvalidScoreErrorNode);
            string expectedInvalidQualityScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.InvalidByteScoreErrorNode);
            byte[] byteArray = { 12, 24 };
            byte qualScore = 12;
            string actualError = null;

            try
            {
                QualitativeSequence.ConvertFromIlluminaToSolexa(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError, expectedNullErrorMessage);

            // Validate an expected error message for invalid qual score array.
            try
            {
                QualitativeSequence.ConvertFromIlluminaToSolexa(byteArray);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError, expectedInvalidScoreError);

            // Validate an expected error message for invalid qual scores.
            try
            {
                QualitativeSequence.ConvertFromIlluminaToSolexa(qualScore);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError,
                expectedInvalidQualityScoreError);
        }

        /// <summary>
        /// Invalidate convert from one Solexa to Sanger format type.
        /// </summary>
        void ConvertSolexaToSanger()
        {
            string expectedNullErrorMessage = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.NullExceptionError);
            string expectedInvalidScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.InvalidScoreErrorNode);
            string expectedInvalidQualityScoreError = _utilityObj._xmlUtil.GetTextValue(
                Constants.FormatTypeConvertionErrosNode,
                Constants.InvalidByteScoreErrorNode);
            byte[] byteArray = { 12, 24 };
            byte qualScore = 12;
            string actualError = null;

            try
            {
                QualitativeSequence.ConvertFromSolexaToSanger(null);
                Assert.Fail();
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError, expectedNullErrorMessage);

            // Validate an expected error message for invalid qual score array.
            try
            {
                QualitativeSequence.ConvertFromSolexaToSanger(byteArray);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError, expectedInvalidScoreError);

            // Validate an expected error message for invalid qual scores.
            try
            {
                QualitativeSequence.ConvertFromSolexaToSanger(qualScore);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                actualError = ex.Message;
            }

            // Validate an expected error message.
            ValidateException(actualError,
                expectedInvalidQualityScoreError);
        }

        /// <summary>
        /// Validate an exception.
        /// <param name="actualError">Actual Error by the code</param>
        /// <param name="expectedError">Expected Error</param>
        /// </summary>
        static void ValidateException(string actualError, string expectedError)
        {
            string updatedActualError = null;
            // Validate an expected exception.
            updatedActualError = actualError.Replace("\r", "").Replace("\n", "");
            Assert.AreEqual(expectedError.ToLower(CultureInfo.CurrentCulture),
                updatedActualError.ToLower(CultureInfo.CurrentCulture));

            // Log Error message to NUnit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P2: Validated exception {0}  successfully",
                updatedActualError));
        }

        /// <summary>
        /// General method to validate creation of Qualitative sequence.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="parameters">Different Qualitative Sequence parameters.</param>
        /// </summary>
        void ValidateQualitativeSequenceCtorException(
            string nodeName, QualitativeSequenceParameters parameters)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string inputQuality = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InputByteArrayNode);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(inputQuality);

            // Create and validate Qualitative Sequence.
            switch (parameters)
            {
                case QualitativeSequenceParameters.EncoderWithByteValue:

                    // Invalidte Null sequence
                    try
                    {
                        createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                            Encodings.IupacNA, null, byteArray);
                        Assert.Fail();
                    }
                    catch (ArgumentException ex)
                    {
                        // Log to Nunit GUI.
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                            ex.Message));
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                            ex.Message));
                    }

                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                            Encodings.IupacNA, null, null);
                    Assert.IsTrue(string.IsNullOrEmpty(createdQualitativeSequence.ToString()));

                    // Invalidte quality score
                    try
                    {
                        createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                            Encodings.IupacNA, "AC", byteArray);
                        Assert.Fail();
                    }
                    catch (ArgumentException ex)
                    {
                        // Log to Nunit GUI.
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                            ex.Message));
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                            ex.Message));
                    }

                    byte[] invlaidByteArray = ASCIIEncoding.ASCII.GetBytes("!@#$!@#$!@#$!@#$!@#$!@#$!@#$!@#$!@#$!");
                    try
                    {
                        createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                            Encodings.IupacNA, inputSequence, invlaidByteArray);
                        Assert.Fail();
                    }
                    catch (ArgumentException ex)
                    {
                        // Log to Nunit GUI.
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                            ex.Message));
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "Segmented Sequence P2: Segmented Sequence Exception was validated successfully {0}",
                            ex.Message));
                    }
                    break;
                default:
                    break;
            }
        }
        # endregion Supporting Methods
    }
}
