// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * QualitativeSequenceP1TestCases.cs
 * 
 * This file contains the Qualitative Sequence P1 test cases.
 * 
******************************************************************************/

using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.Serialization;

using MBF.Encoding;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.IO;
using MBF.IO.FastQ;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Qualitative sequence validations.
    /// </summary>
    [TestClass]
    public class QualitativeSequenceP1TestCases
    {
        #region Enums

        /// <summary>
        /// Qualitative Sequence method Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum QualitativeSequenceParameters
        {
            Alphabets,
            FormatType,
            Sequence,
            Score,
            Ncbi2NA,
            Ncbi4NA,
            IupacNA,
            NcbiEAA,
            IupacNAWithScore,
            ByteArray,
            InsertItem,
            InsertRange,
            InsertChar,
            InsertCharWithByteArray,
            Replace,
            ReplaceChar,
            ReplaceWithScore,
            ReplaceRangeWithByteArray,
            RemoveAt,
            RemoveRange,
            Range,
            IndexOf,
            IndexOfNonGap,
            IndexOfNonGapWithParam,
            LastIndexOf,
            LastIndexOfWithPam,
            ReplaceRange,
            DefaultScoreWithAlphabets,
            DefaultScoreWithSequence,
            MaxDefaultScore,
            MinDefaultScore,
            Encoder,
            EncoderWithScore,
            EncoderWithByteValue,
            ReplaceQualityScore,
            GetEnumerator,
            GetObjectData,
            ReplaceRangeQualityScores,
            Default
        };

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\QualitativeTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static QualitativeSequenceP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Qualitative P1 TestCases

        /// <summary>
        /// Validate creation of Qualitative Sequence for Rna Sequence
        /// with Sanger FastQFormat and specified score.
        /// Input Data : Rna Alphabet,Rna Sequence,"Sanger" FastQFormat.
        /// and Score "120" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSangerFormatTypeRnaQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleRnaSangerNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Rna Sequence
        /// with Solexa FastQFormat and specified score.
        /// Input Data : Rna Alphabet,Rna Sequence,"Solexa" FastQFormat.
        /// and Score "104" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSolexaFormatTypeRnaQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleRnaSolexaNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Rna Sequence
        /// with Illumina FastQFormat and specified score.
        /// Input Data : Rna Alphabet,Rna Sequence,"Illumina" FastQFormat.
        /// and Score "104" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateIlluminaFormatTypeRnaQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleRnaIlluminaNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Protein Sequence
        /// with Sanger FastQFormat and specified score.
        /// Input Data : Protein Alphabet,Protein Sequence,"Sanger" FastQFormat.
        /// and Score "120" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSangerFormatTypeProteinQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleProteinSangerNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Protein Sequence
        /// with Solexa FastQFormat and specified score.
        /// Input Data : Protein Alphabet,Protein Sequence,"Solexa" FastQFormat.
        /// and Score "104" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSolexaFormatTypeProteinQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleProteinSolexaNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Protein Sequence
        /// with Illumina FastQFormat and specified score.
        /// Input Data : Protein Alphabet,Protein Sequence,"Illumina" FastQFormat.
        /// and Score "104" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateIlluminaFormatTypeProteinQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleProteinIlluminaNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Sanger FastQFormat and Ncbi2NA encoding.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSangerFormatTypeDnaQualitativeSequenceWithNcbi2NAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSangerNode,
                QualitativeSequenceParameters.Ncbi2NA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Sanger FastQFormat and Ncbi4NA encoding.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSangerFormatTypeDnaQualitativeSequenceWithNcbi4NAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSangerNode,
                QualitativeSequenceParameters.Ncbi4NA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Rna Sequence
        /// with Sanger FastQFormat and Ncbi2NA encoding.
        /// Input Data : Rna Alphabet,Rna Sequence,"Sanger" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSangerFormatTypeRnaQualitativeSequenceWithNcbi2NAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleRnaSangerNode,
                QualitativeSequenceParameters.Ncbi2NA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Illumina FastQFormat and IupacNA encoding.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateIlluminaFormatTypeDnaQualitativeSequenceWithIupacNAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaIlluminaNode,
                QualitativeSequenceParameters.IupacNA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Solexa FastQFormat and IupacNA encoding.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSolexaFormatTypeDnaQualitativeSequenceWithIupacNAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSolexaNode,
                QualitativeSequenceParameters.IupacNA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Protein Sequence
        /// with Solexa FastQFormat and NcbiEAA encoding.
        /// Input Data : Protein Alphabet,Protein Sequence,"Solexa" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSolexaFormatTypeProteinQualitativeSequenceWithNcbiEAAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleProteinSolexaNode,
                QualitativeSequenceParameters.NcbiEAA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Protein Sequence
        /// with Sanger FastQFormat and NcbiEAA encoding.
        /// Input Data : Protein Alphabet,Protein Sequence,"Sanger" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSangerFormatTypeProteinQualitativeSequenceWithNcbiEAAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleProteinSangerNode,
                QualitativeSequenceParameters.NcbiEAA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Protein Sequence
        /// with Illumina FastQFormat and NcbiEAA encoding.
        /// Input Data : Protein Alphabet,Protein Sequence,"Illumina" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateIlluminaFormatTypeProteinQualitativeSequenceWithNcbiEAAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleProteinIlluminaNode,
                QualitativeSequenceParameters.NcbiEAA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Sanger FastQFormat and IupacNA encoding and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat and input score.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSangerDnaQualitativeSequenceWithIupacNAEncodingAndScore()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSangerNode,
                QualitativeSequenceParameters.IupacNAWithScore);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Solexa FastQFormat and IupacNA encoding and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat and input score.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSolexaDnaQualitativeSequenceWithIupacNAEncodingAndScore()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSolexaNode,
                QualitativeSequenceParameters.IupacNAWithScore);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Rna Sequence
        /// with Illumina FastQFormat and IupacNA encoding.
        /// Input Data : Rna Alphabet,Rna Sequence,"Illumina" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateIlluminaFormatTypeRnaQualitativeSequenceWithIupacNAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleRnaIlluminaNode,
                QualitativeSequenceParameters.IupacNA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Rna Sequence
        /// with Solexa FastQFormat and IupacNA encoding.
        /// Input Data : Rna Alphabet,Rna Sequence,"Solexa" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSolexaFormatTypeRnaQualitativeSequenceWithIupacNAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleRnaSolexaNode,
                QualitativeSequenceParameters.IupacNA);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Illumina FastQFormat and Byte array.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence with score.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateIlluminaFormatTypeDnaQualitativeSequenceWithByteArray()
        {
            GeneralQualitativeSequence(Constants.SimpleDNAIlluminaByteArrayNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate addition of Qualitative Sequence item.
        /// Input Data : Dna Sequence and score.
        /// Output Data : Validate addition of Sequence item to Quality Sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateAdditionOfSeqItemWithScoreForDnaSequence()
        {
            ValidateAdditionOfSequenceItems(Constants.SimpleDnaSolexaNode, "SeqWithScore");
        }

        /// <summary>
        /// Validate addition of Qualitative Sequence item for Rna Sequence.
        /// Input Data : Rna Sequence and score.
        /// Output Data : Validate addition of Sequence item to Quality Sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateAdditionOfSeqItemWithScoreForRnaSequence()
        {
            ValidateAdditionOfSequenceItems(Constants.SimpleRnaSolexaNode, "SeqWithScore");
        }

        /// <summary>
        /// Validate addition of Qualitative Sequence item.
        /// Input Data : Protein Sequence and score.
        /// Output Data : Validate addition of Sequence item to Quality Sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateAdditionOfSeqItemWithScoreForProteinSequence()
        {
            ValidateAdditionOfSequenceItems(Constants.SimpleProteinSolexaNode, "SeqWithScore");
        }

        /// <summary>
        /// Validate addition of Qualitative Sequence item.
        /// Input Data : Protein Sequence and score.
        /// Output Data : Validate addition of Sequence item to Quality Sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateAdditionOfSeqItemForProteinSequence()
        {
            ValidateAdditionOfSequenceItems(Constants.SimpleProteinSolexaNode, null);
        }

        /// <summary>
        /// Validate whether Seq item present in the qualitative sequence 
        /// by passing qual score to contains method.
        /// Input Data : Dna Sequence and score.
        /// Output Data : Validate qualitative sequence item.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateQualitativeSeqItem()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
               Constants.SimpleDnaSolexaNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(Constants.SimpleDnaSolexaNode, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.inputSequenceNode);
            string inputScore = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.InputScoreNode);

            // Create a quality Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                alphabet, expectedFormatType, inputSequence);

            // Add a sequence item to qualilty Sequence.
            createdQualitativeSequence.IsReadOnly = false;
            createdQualitativeSequence.Add(createdQualitativeSequence[0], Convert.ToByte(inputScore, (IFormatProvider)null));

            // Validate quality Sequence after addition of Seq Item.
            Assert.IsTrue(createdQualitativeSequence.ContainsQualityScore(Convert.ToByte(inputScore, (IFormatProvider)null)));
            Assert.IsFalse(createdQualitativeSequence.ContainsQualityScore(20));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Concat(
                "Qualitative Sequence P1 : Qualitative SequenceItems validation completed successfully.",
                createdQualitativeSequence.ToString()));
        }

        /// <summary>
        /// Validate CopyTo method of Rna Qualitative Sequence.
        /// Input Data : Rna Sequence, Sanger format.
        /// Output Data :Validate copied sequence items in array.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCopiedRnaQualitativeSeqItems()
        {
            ValidateCopiedQualitativeSeqItems(Constants.SimpleRnaSangerNode);
        }

        /// <summary>
        /// Validate CopyTo method of Protein Qualitative Sequence.
        /// Input Data : Rna Sequence, Solexa format.
        /// Output Data :Validate copied sequence items in array.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCopiedProteinQualitativeSeqItems()
        {
            ValidateCopiedQualitativeSeqItems(Constants.SimpleProteinSolexaNode);
        }

        /// <summary>
        /// Validate IndexOf Qualitative Sequence Items.
        /// Input Data : Dna Sequence and score.
        /// Output Data : Validate qualitative sequence item indices.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateQualitativeSeqItemIndexes()
        {
            ValidateGeneralQualitativeSeqItemIndices(
                Constants.SimpleDnaSangerNode, QualitativeSequenceParameters.IndexOf);
        }

        /// <summary>
        /// Validate IndexOf Non Gap characters present in Qualitative Sequence Items.
        /// Input Data : Dna Sequence and score.
        /// Output Data : Validate IndexOf Non Gap characters present in Qualitative Sequence Items.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateQualitativeSeqItemIndexOfNonGapChars()
        {
            ValidateGeneralQualitativeSeqItemIndices(
                Constants.SimpleDnaSangerNode, QualitativeSequenceParameters.IndexOfNonGap);
        }

        /// <summary>
        /// Validate IndexOf Non Gap characters present in Qualitative Sequence
        /// Items by passing Sequence Item position to IndexOfNonGap() method.
        /// Input Data : Dna Sequence and score.
        /// Output Data : Validate IndexOf Non Gap characters present in Qualitative Sequence Items.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateQualitativeSeqItemIndexOfNonGapCharsUsingPam()
        {
            ValidateGeneralQualitativeSeqItemIndices(
                Constants.SimpleDnaSangerNode, QualitativeSequenceParameters.IndexOfNonGapWithParam);
        }

        /// <summary>
        /// Validate Last IndexOf Non Gap characters present in Qualitative Sequence Items.
        /// Input Data : Dna Sequence and score.
        /// Output Data : Validate Last IndexOf Non Gap characters present in Qualitative Sequence Items.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateQualitativeSeqItemLastIndexOfNonGapChars()
        {
            ValidateGeneralQualitativeSeqItemIndices(
                Constants.SimpleDnaSangerNode, QualitativeSequenceParameters.LastIndexOf);
        }

        /// <summary>
        /// Validate LastIndexOf Non Gap characters present in Qualitative Sequence
        /// Items by passing Sequence Item position to LastIndexOfNonGap() method.
        /// Input Data : Dna Sequence and score.
        /// Output Data : Validate IndexOf Non Gap characters present in Qualitative Sequence Items.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateQualitativeSeqItemLastIndexOfNonGapCharsUsingPam()
        {
            ValidateGeneralQualitativeSeqItemIndices(
                Constants.SimpleDnaSangerNode, QualitativeSequenceParameters.LastIndexOfWithPam);
        }

        /// <summary>
        /// Validate Inserting Sequence itmes to Qualitative sequence
        /// Input Data : Dna Sequence.
        /// Output Data :Validate insertion of Sequence items.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateInsertSequenceItems()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.InsertItem);
        }

        /// <summary>
        /// Validate Insertion of characters to Qualitative sequence
        /// Input Data : Dna Sequence.
        /// Output Data :Validate insertion of characters in Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateInsertChars()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.InsertChar);
        }

        /// <summary>
        /// Validate Insertion of Sequence to Qualitative sequence
        /// Input Data : Dna Sequence.
        /// Output Data :Validate insertion of sequence in Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateInsertSequence()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.InsertRange);
        }

        /// <summary>
        /// Validate Insertion of Sequence to Qualitative sequence
        /// with Byte array score
        /// Input Data : Dna Sequence.
        /// Output Data :Validate insertion of sequence in Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateInsertSequenceWithByteArrayScore()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.InsertCharWithByteArray);
        }

        /// <summary>
        /// Validate Insertion of Rna Sequence to Qualitative sequence
        /// with Byte array score
        /// Input Data : Rna Sequence.
        /// Output Data :Validate insertion of sequence in Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateInsertRnaSequenceWithByteArrayScore()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleRnaSolexaNode, QualitativeSequenceParameters.InsertCharWithByteArray);
        }

        /// <summary>
        /// Validate Insertion of Protein Sequence to Qualitative sequence
        /// with Byte array score
        /// Input Data : Protein Sequence.
        /// Output Data :Validate insertion of sequence in Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateInsertProteinSequenceWithByteArrayScore()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleProteinSolexaNode, QualitativeSequenceParameters.InsertCharWithByteArray);
        }

        /// <summary>
        /// Validate Subset of Qualitative Sequence using Range() method.
        /// Input Data : Dna Solexa Sequence.and specified range.
        /// Output Data :Validate subset of qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSubsetOfQualitativeSequence()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.Range);
        }

        /// <summary>
        /// Validate Remove Sequence Item from Qualitative Sequence
        /// using RemoveAt()
        /// Input Data : Dna Solexa Sequence.and Seq item to be removed
        /// Output Data : Validate Remove Sequence Item from Qualitative Sequence
        /// using RemoveAt() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateRemoveSeqItemUsingRemoveAt()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.RemoveAt);
        }

        /// <summary>
        /// Validate Remove Sequence from Qualitative Sequence
        /// using RemoveRange().
        /// Input Data : Rna Solexa Sequence.and specified range.
        /// Output Data : Validate Remove Sequence from Qualitative
        /// Sequence using RemoveRange().
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateRemoveRnaSeqItemUsingRemoveRange()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleRnaSolexaNode, QualitativeSequenceParameters.RemoveRange);
        }

        /// <summary>
        /// Validate Remove Sequence from Qualitative Sequence using RemoveRange().
        /// Input Data : Protein Solexa Sequence.and specified range.
        /// Output Data : Validate Remove Sequence from Qualitative Sequence using RemoveRange().
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateRemoveProteinSeqItemUsingRemoveRange()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleProteinSolexaNode, QualitativeSequenceParameters.RemoveRange);
        }

        /// <summary>
        /// Validate Replace Sequence Items.
        /// Input Data : Dna Sequence
        /// Output Data :Validate replacing Sequence items with
        /// other sequence item.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReplaceSequenceItem()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.Replace);
        }

        /// <summary>
        /// Validate Replace Sequence Items with score specified.
        /// Input Data : Dna Sequence,Score and Item to replace.
        /// Output Data :Validate replacing Sequence items with other
        /// sequence item.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReplaceSequenceItemWithScore()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.ReplaceWithScore);
        }

        /// <summary>
        /// Validate Replace Sequence Items with score specified.
        /// Input Data : Rna Sequence,Score,Item to replace.
        /// Output Data :Validate replacing Sequence items with 
        /// other sequence item.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReplaceRnaSequenceItemWithScore()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleRnaSolexaNode, QualitativeSequenceParameters.ReplaceWithScore);
        }

        /// <summary>
        /// Validate Replace Sequence Items with score specified 
        /// Using ReplaceRange.
        /// Input Data : Dna Sequence,Score, Item to replace.
        /// Output Data : Validate Replace Sequence Items with score
        /// specified Using ReplaceRange
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReplaceDnaSequenceItemWithScoreUsingReplaceRange()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.ReplaceRange);
        }

        /// <summary>
        /// Validate Replace Sequence Items with Byte array 
        /// score specified Using ReplaceRange.
        /// Input Data : Dna Sequence,Byte array score, Item to replace.
        /// Output Data : Validate Replace Sequence Items with score 
        /// specified Using ReplaceRange
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReplaceDnaSequenceItemWithByteArrayScoreUsingReplaceRange()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.ReplaceRangeWithByteArray);
        }

        /// <summary>
        /// Validate Replace Sequence Items with Byte array 
        /// score specified Using ReplaceRange.
        /// Input Data : Rna Sequence,Byte array score, Item to replace.
        /// Output Data : Validate Replace Sequence Items with score
        /// specified Using ReplaceRange
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReplaceRnaSequenceItemWithByteArrayScoreUsingReplaceRange()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleRnaSolexaNode, QualitativeSequenceParameters.ReplaceRangeWithByteArray);
        }

        /// <summary>
        /// Validate default score for Dna solexa FastQ sequence.
        /// Input Data :Dna Alphabet,Solexa FastQ format.
        /// Output Data : Validate FastQ Sanger format type default score.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateDefaultQualScoreForDnaSolexa()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.DefaultScoreWithAlphabets);
        }

        /// <summary>
        /// Validate FastQ Sanger format type default score.
        /// Input Data :Dna Alphabet, Sanger FastQ format.
        /// Output Data : Validate FastQ Sanger format type default score.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateDefaultQualScoreForDnaSanger()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleDnaSangerNode, QualitativeSequenceParameters.DefaultScoreWithAlphabets);
        }

        /// <summary>
        /// Validate FastQ Sanger format type default score.
        /// Input Data :Protein Alphabet, Sanger FastQ format.
        /// Output Data : Validate FastQ Sanger format type default score.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateDefaultQualScoreForProteinSanger()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleProteinSangerNode, QualitativeSequenceParameters.DefaultScoreWithAlphabets);
        }

        /// <summary>
        /// Validate FastQ Illumina format type default score.
        /// Input Data :Dna Alphabet, Illumina FastQ format.
        /// Output Data : Validate FastQ Illumina format type default score.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateDefaultQualScoreForDnaIllumina()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleDnaIlluminaNode, QualitativeSequenceParameters.DefaultScoreWithAlphabets);
        }

        /// <summary>
        /// Validate FastQ Illumina format type default score.
        /// Input Data :Rna Alphabet, Illumina FastQ format.
        /// Output Data : Validate FastQ Illumina format type default score.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateDefaultQualScoreForRnaIllumina()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleRnaIlluminaNode, QualitativeSequenceParameters.DefaultScoreWithAlphabets);
        }

        /// <summary>
        /// Validate FastQ Illumina format type default score.
        /// Input Data :Protein Alphabet, Illumina FastQ format.
        /// Output Data : Validate FastQ Illumina format type default score.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateDefaultQualScoreForProteinIllumina()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleProteinIlluminaNode, QualitativeSequenceParameters.DefaultScoreWithAlphabets);
        }

        /// <summary>
        /// Validate FastQ Solexa format type default score.
        /// Input Data :Protein Sequence, Solexa FastQ format.
        /// Output Data : Validate FastQ Solexa format type default score.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateDefaultQualScoreForProteinSequenceSolexa()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleProteinSolexaNode, QualitativeSequenceParameters.DefaultScoreWithSequence);
        }

        /// <summary>
        /// Validate FastQ Solexa format type default score.
        /// Input Data :Dna Sequence, Solexa FastQ format.
        /// Output Data : Validate FastQ Solexa format type default score.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateDefaultQualScoreForDnaSequenceSolexa()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.DefaultScoreWithSequence);
        }

        /// <summary>
        /// Validate FastQ Solexa format type default score.
        /// Input Data :Rna Sequence, Solexa FastQ format.
        /// Output Data : Validate FastQ Solexa format type default score.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateDefaultQualScoreForRnaSequenceSolexa()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleRnaSolexaNode, QualitativeSequenceParameters.DefaultScoreWithSequence);
        }

        /// <summary>
        /// Validate Maximum score for Dna Sanger FastQ.
        /// Input Data :Dna Sequence, Sanger FastQ format.
        /// Output Data : Validate Maximum score for Dna Sanger FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMaxQualScoreForDnaSanger()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleDnaSangerNode, QualitativeSequenceParameters.MaxDefaultScore);
        }

        /// <summary>
        /// Validate Maximum score for Rna Sanger FastQ.
        /// Input Data :Rna Sequence, Sanger FastQ format.
        /// Output Data : Validate Maximum score for Rna Sanger FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMaxQualScoreForRnaSanger()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleRnaSangerNode, QualitativeSequenceParameters.MaxDefaultScore);
        }

        /// <summary>
        /// Validate Maximum score for Protein Sanger FastQ.
        /// Input Data :Protein Sequence,Sanger FastQ format.
        /// Output Data : Validate Maximum score for Protein Sanger FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMaxQualScoreForProteinSanger()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleProteinSangerNode, QualitativeSequenceParameters.MaxDefaultScore);
        }

        /// <summary>
        /// Validate Maximum score for Dna Illumina FastQ.
        /// Input Data :Dna Sequence,Illumina FastQ format.
        /// Output Data : Validate Maximum score for Dna Illumina FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMaxQualScoreForDnaIllumina()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleDnaIlluminaNode, QualitativeSequenceParameters.MaxDefaultScore);
        }

        /// <summary>
        /// Validate Maximum score for Rna Illumina FastQ.
        /// Input Data :Rna Sequence,Illumina FastQ format.
        /// Output Data : Validate Maximum score for Rna Illumina FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMaxQualScoreForRnaIllumina()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleRnaIlluminaNode, QualitativeSequenceParameters.MaxDefaultScore);
        }

        /// <summary>
        /// Validate Maximum score for Protein Illumina FastQ.
        /// Input Data :Protein Sequence,Illumina FastQ format.
        /// Output Data : Validate Maximum score for Protein Illumina FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMaxQualScoreForProteinIllumina()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleProteinIlluminaNode, QualitativeSequenceParameters.MaxDefaultScore);
        }

        /// <summary>
        /// Validate Maximum score for Dna Solexa FastQ.
        /// Input Data :Dna Sequence,Solexa FastQ format.
        /// Output Data : Validate Maximum score for Dna Solexa FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMaxQualScoreForDnaSolexa()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.MaxDefaultScore);
        }

        /// <summary>
        /// Validate Maximum score for Rna Solexa FastQ.
        /// Input Data :Rna Sequence,Solexa FastQ format.
        /// Output Data : Validate Maximum score for Rna Solexa FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMaxQualScoreForRnaSolexa()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleRnaSolexaNode, QualitativeSequenceParameters.MaxDefaultScore);
        }

        /// <summary>
        /// Validate Maximum score for Protein Solexa FastQ.
        /// Input Data :Protein Sequence,Solexa FastQ format.
        /// Output Data : Validate Maximum score for Protein Solexa FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMaxQualScoreForProteinSolexa()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleProteinSolexaNode, QualitativeSequenceParameters.MaxDefaultScore);
        }

        /// <summary>
        /// Validate Minimum score for Dna Sanger FastQ.
        /// Input Data :Dna Sequence, Sanger FastQ format.
        /// Output Data : Validate Minimum score for Dna Sanger FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMinimumQualScoreForDnaSanger()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleDnaSangerNode, QualitativeSequenceParameters.MinDefaultScore);
        }

        /// <summary>
        /// Validate Minimum score for Rna Sanger FastQ.
        /// Input Data :Rna Sequence, Sanger FastQ format.
        /// Output Data : Validate Minimum score for Rna Sanger FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMinimumQualScoreForRnaSanger()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleRnaSangerNode, QualitativeSequenceParameters.MinDefaultScore);
        }

        /// <summary>
        /// Validate Minimum score for Protein Sanger FastQ.
        /// Input Data :Protein Sequence,Sanger FastQ format.
        /// Output Data : Validate Minimum score for Protein Sanger FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMinimumQualScoreForProteinSanger()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleProteinSangerNode, QualitativeSequenceParameters.MinDefaultScore);
        }

        /// <summary>
        /// Validate Minimum score for Dna Illumina FastQ.
        /// Input Data :Dna Sequence,Illumina FastQ format.
        /// Output Data : Validate Minimum score for Dna Illumina FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMinimumQualScoreForDnaIllumina()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleDnaIlluminaNode, QualitativeSequenceParameters.MinDefaultScore);
        }

        /// <summary>
        /// Validate Minimum score for Rna Illumina FastQ.
        /// Input Data :Rna Sequence,Illumina FastQ format.
        /// Output Data : Validate Minimum score for Rna Illumina FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMinimumQualScoreForRnaIllumina()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleRnaIlluminaNode, QualitativeSequenceParameters.MinDefaultScore);
        }

        /// <summary>
        /// Validate Minimum score for Protein Illumina FastQ.
        /// Input Data :Protein Sequence,Illumina FastQ format.
        /// Output Data : Validate Minimum score for Protein Illumina FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMinimumQualScoreForProteinIllumina()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleProteinIlluminaNode, QualitativeSequenceParameters.MinDefaultScore);
        }

        /// <summary>
        /// Validate Minimum score for Dna Solexa FastQ.
        /// Input Data :Dna Sequence,Solexa FastQ format.
        /// Output Data : Validate Minimum score for Dna Solexa FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMinimumQualScoreForDnaSolexa()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.MinDefaultScore);
        }

        /// <summary>
        /// Validate Minimum score for Rna Solexa FastQ.
        /// Input Data :Rna Sequence,Solexa FastQ format.
        /// Output Data : Validate Minimum score for Rna Solexa FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMinimumQualScoreForRnaSolexa()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleRnaSolexaNode, QualitativeSequenceParameters.MinDefaultScore);
        }

        /// <summary>
        /// Validate Minimum score for Protein Solexa FastQ.
        /// Input Data :Protein Sequence,Solexa FastQ format.
        /// Output Data : Validate Minimum score for Protein Solexa FastQ.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMinimumQualScoreForProteinSolexa()
        {
            ValidateFastQDefaultScores(
                Constants.SimpleProteinSolexaNode, QualitativeSequenceParameters.MinDefaultScore);
        }

        /// <summary>
        /// Validate Qualitative sequence constructor by passing Encoder and decoder.
        /// Input Data : Dna Sequence,Sanger FastQ format,Iupac encoding, sequence
        /// string.
        /// Output Data : Validate created qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateQualSequenceCtrWithEncoderAndDecoder()
        {
            GeneralQualitativeSequence(
                Constants.SimpleDnaSangerNode, QualitativeSequenceParameters.Encoder);
        }

        /// <summary>
        /// Validate Qualitative sequence constructor by passing Encoder,Decoder and QualityScore.
        /// Input Data : Dna Sequence,Sanger FastQ format,Iupac encoding, sequence
        /// string and quality score.
        /// Output Data : Validate created qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateQualSequenceCtrWithEncoderAndQualityScore()
        {
            GeneralQualitativeSequence(
                Constants.SimpleDnaSangerNode, QualitativeSequenceParameters.EncoderWithScore);
        }

        /// <summary>
        /// Validate Qualitative sequence constructor by passing Encoder,Decoder and ByteArray.
        /// Input Data : Dna Sequence,Sanger FastQ format,Iupac encoding, sequence
        /// string and quality score byte array.
        /// Output Data : Validate created qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateQualSequenceCtrWithEncoderAndByteArrayScore()
        {
            GeneralQualitativeSequence(
                Constants.SimpleDnaSangerNode, QualitativeSequenceParameters.EncoderWithByteValue);
        }

        /// <summary>
        /// Validate Replace Quality score with new quality score.
        /// Input Data : Qualitative sequence and new quality score.
        /// Output Data : Validate replaced quality score.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReplaceQualityScore()
        {
            ValidateQualitativeSequenceMethods(
                 Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.ReplaceQualityScore);
        }

        /// <summary>
        /// Validate replace Quality sequence char.
        /// Input Data : Qualitative sequence and Character to be inserted.
        /// Output Data : Validate replaced quality sequence
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReplaceQualitySeqChar()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleProteinSolexaNode, QualitativeSequenceParameters.ReplaceChar);
        }

        /// <summary>
        /// Validate GetEnumerator().Retrives an enumerator for
        /// the qualitative sequence
        /// Input Data : Qualitative sequence.
        /// Output Data : Validation of GetEnumerator() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateGetEnumeratorForQualSeq()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleProteinSolexaNode, QualitativeSequenceParameters.GetEnumerator);
        }

        /// <summary>
        /// Validate GetObjectData() method of serializing qualitative sequence
        /// Input Data : Qualitative sequence.
        /// Output Data : Validation of GetObjectData() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateGetObjectDataForQualSeq()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleProteinSolexaNode, QualitativeSequenceParameters.GetObjectData);
        }

        /// <summary>
        /// Validate Replace range quality scores.
        /// Input Data : Dna Sequence, Byte array to be replaced.
        /// Output Data : Validate Replaced quality scores.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateReplaceRangeQualityScores()
        {
            ValidateQualitativeSequenceMethods(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.ReplaceRangeQualityScores);
        }

        /// <summary>
        /// Validate Insert char to qualitative sequence.
        /// Input Data : Dna Sequence,Char 'A'.
        /// Output Data : Validate Inserted Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateInsertCharToQualSeq()
        {
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.inputSequenceNode);
            string expectedSeqAfterInsertSeqItem = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.ExpectedSeqAfterAdd);
            string itemToBeInserted = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode, Constants.ItemToBeInserted);
            char charToBeInserted = itemToBeInserted[0];

            // Create a qualitative sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Sanger, inputSequence);
            qualSeq.IsReadOnly = false;

            // Insert char 'A' at position 1.
            qualSeq.Insert(1, charToBeInserted);

            // Validate updated qualitative sequence.
            Assert.AreEqual(expectedSeqAfterInsertSeqItem, qualSeq.ToString());
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P1: Updated qualitative sequence is : {0}",
                qualSeq.ToString()));
        }

        /// <summary>
        /// Validate QualitativeSequence Range using Range method.
        /// Input Data : Dna Sequence
        /// Output Data : Expected sequence range.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSeqRange()
        {
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.inputSequenceNode);
            string expectedSeqRange = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.ExpectedSeqRangeNode);
            ISequence newSeq = null;

            // Create a qualitative sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Sanger, inputSequence);
            qualSeq.IsReadOnly = false;

            // Get a sequence range.
            newSeq = qualSeq.Range(0, 3);

            // Validate sequence.
            Assert.AreEqual(expectedSeqRange, newSeq.ToString());
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P1: Updated sequence range is : {0}",
                newSeq.ToString()));
        }

        /// <summary>
        /// Validate QualitativeSequence replace seq items.
        /// Input Data : Dna Sequence, sequence to be replaced.
        /// Output Data : Expected replaced sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateSeqItemReplace()
        {
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.inputSequenceNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(
                Constants.QualitativeSequenceInsertSeqItemNode,
                Constants.SequenceAfterReplaceNode);
            QualitativeSequence cloneSeq = null;

            // Create a qualitative sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Sanger, Encodings.IupacNA, inputSequence);

            SequenceStatistics seqCount = qualSeq.Statistics;
            Assert.IsNotNull(seqCount.GetCount('A'));

            qualSeq.IsReadOnly = false;

            // Replace Sequence item at 1 position.
            qualSeq.Replace(0, qualSeq[3]);


            // Validate sequence.
            Assert.AreEqual(expectedSeq, qualSeq.ToString());
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P1: Validated the updated sequence : {0}",
                qualSeq.ToString()));

            // Replace Sequence item at 1 position with byte score.
            qualSeq.Replace(0, qualSeq[3], 72);

            // Validate sequence.
            Assert.AreEqual(expectedSeq, qualSeq.ToString());

            // Validate clone sequence.
            cloneSeq = qualSeq.Clone();
            Assert.AreEqual(expectedSeq, cloneSeq.ToString());

            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P1: Validated the updated sequence : {0}",
                qualSeq.ToString()));
        }

        /// <summary>
        /// Validate Qualitative sequence properties
        /// Input : Qualitative sequence
        /// Output : Qual Sequence properties validation.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateQualSequenceProperties()
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
            string expectedDocumentation = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.DocumentaionNode);

            // Create a qualitative sequence object
            QualitativeSequence qualObj = new QualitativeSequence(alphabet,
                expectedFormatType, inputSequence);

            // Enable Data virtualization
            SequencePointer pointerObj = new SequencePointer();
            pointerObj.AlphabetName = "DNA";
            pointerObj.Id = seqId;
            pointerObj.IndexOffsets[0] = 20;
            pointerObj.IndexOffsets[1] = pointerObj.IndexOffsets[0] + 50;
            pointerObj.StartingLine = 1;

            IVirtualSequenceParser parserObj = null;
            try
            {
                parserObj = new FastQParser();

                FileVirtualQualitativeSequenceProvider provObj =
                   new FileVirtualQualitativeSequenceProvider(parserObj, pointerObj);

                qualObj.VirtualQualitativeSequenceProvider = provObj;

                // Set qualSequence properties
                qualObj.BlockSize = 5;
                qualObj.DisplayID = seqId;
                qualObj.Documentation = expectedDocumentation;
                qualObj.ID = seqId;
                qualObj.IsReadOnly = false;
                qualObj.MaxNumberOfBlocks = 10;
                qualObj.MoleculeType = MoleculeType.DNA;

                // Validate Qualitative sequence properties
                Assert.AreEqual(5, qualObj.BlockSize);
                Assert.AreEqual(10, qualObj.MaxNumberOfBlocks);
                Assert.AreEqual(seqId, qualObj.ID);
                Assert.AreEqual(seqId, qualObj.DisplayID);
                Assert.AreEqual(expectedDocumentation, qualObj.Documentation);
                Assert.AreEqual(expectedFormatType, qualObj.Type);

                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Successfully validated the qualitative sequence properties"));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Successfully validated the qualitative sequence properties"));

                // validate serialzation.
                QualitativeSequence seq = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Sanger, "ACGTACGT", 65);
                using (Stream stream = File.Open("QualitativeSequence.data", FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, seq);

                    stream.Seek(0, SeekOrigin.Begin);
                    QualitativeSequence deserializedSeq = (QualitativeSequence)formatter.Deserialize(stream);
                    Assert.AreNotSame(seq, deserializedSeq);
                    Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);
                    Assert.AreSame(seq.Encoding, deserializedSeq.Encoding);
                }
            }
            finally
            {
                if (parserObj != null)
                    ((IDisposable)parserObj).Dispose();
            }
        }


        /// <summary>
        /// Validate Qualitative sequence cloning.
        /// Input : Qualitative sequence
        /// Output : Clone sequence validation
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateQualSequenceCloning()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(Constants.SimpleDnaSangerNode,
                Constants.FastQFormatType));
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.inputSequenceNode);

            // Create a qualitative sequence object
            QualitativeSequence qualObj = new QualitativeSequence(alphabet,
                expectedFormatType, inputSequence);
            ISequence iSeqQualObj = new QualitativeSequence(alphabet,
                expectedFormatType, inputSequence);
            ICloneable cloneableQualSeq = new QualitativeSequence(alphabet,
                expectedFormatType, inputSequence);

            // Clone a qual sequence.
            QualitativeSequence cloneQualSeq = qualObj.Clone();
            ISequence iSeqClone = iSeqQualObj.Clone();
            object cloneablelSeq = cloneableQualSeq.Clone();

            Assert.AreEqual(cloneQualSeq.ToString(), qualObj.ToString());
            Assert.AreEqual(iSeqClone.ToString(), iSeqQualObj.ToString());
            Assert.AreEqual(cloneablelSeq.ToString(), cloneableQualSeq.ToString());

            Console.WriteLine(string.Format((IFormatProvider)null,
                "Successfully validated the qualitative sequence cloning"));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Successfully validated the qualitative sequence cloning"));
        }

        /// <summary>
        /// Validate Qual sequence general methods with DV.
        /// Input : Qual sequence
        /// Output : Qual sequence methods validation.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateQualSequenceMethodsWithDV()
        {
            ValidateQualSequenceGeneralMethods(QualitativeSequenceParameters.ReplaceRangeQualityScores);

            ValidateQualSequenceGeneralMethods(QualitativeSequenceParameters.InsertCharWithByteArray);

            ValidateQualSequenceGeneralMethods(QualitativeSequenceParameters.InsertRange);

            ValidateQualSequenceGeneralMethods(QualitativeSequenceParameters.ReplaceRange);

            ValidateQualSequenceGeneralMethods(QualitativeSequenceParameters.IndexOf);
        }

        #endregion Qualitative P1 TestCases

        #region Supporting Methods

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
            string expectedScore = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedScore);
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.QSequenceCount);
            string expectedMaxScore = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.MaxScoreNode);
            string inputScore = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InputScoreNode);
            string inputScoreforIUPAC = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.MaxScoreNode);
            string expectedOuptutScore = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InputScoreNode);
            string inputQuality = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InputByteArrayNode);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(inputQuality);
            string[] encodedValues = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EncodedValuesNode).Split(',');
            string expectedRevComplement = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RevComplement);
            int index = 0;

            // Create and validate Qualitative Sequence.
            switch (parameters)
            {
                case QualitativeSequenceParameters.Alphabets:
                    createdQualitativeSequence = new QualitativeSequence(alphabet);
                    Assert.IsFalse(createdQualitativeSequence.IsReadOnly);
                    break;
                case QualitativeSequenceParameters.FormatType:
                    createdQualitativeSequence = new QualitativeSequence(alphabet,
                        expectedFormatType);
                    Assert.IsFalse(createdQualitativeSequence.IsReadOnly);
                    break;
                case QualitativeSequenceParameters.Sequence:
                    createdQualitativeSequence = new QualitativeSequence(
                    alphabet, expectedFormatType, inputSequence);
                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore, Convert.ToInt32(expectedMaxScore, (IFormatProvider)null));
                    }
                    Assert.IsTrue(createdQualitativeSequence.IsReadOnly);
                    break;
                case QualitativeSequenceParameters.Ncbi2NA:
                    createdQualitativeSequence = new QualitativeSequence(alphabet,
                        expectedFormatType, Encodings.Ncbi2NA, inputSequence);

                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore, Convert.ToInt32(expectedMaxScore, (IFormatProvider)null));
                    }

                    // Validate encoding,Format type and qualitative sequence.
                    Assert.IsTrue(createdQualitativeSequence.IsReadOnly);
                    Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSequence);
                    Assert.AreEqual(createdQualitativeSequence.Type, expectedFormatType);
                    Assert.AreEqual(createdQualitativeSequence.Encoding, Encodings.Ncbi2NA);
                    break;
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
                case QualitativeSequenceParameters.IupacNA:
                    createdQualitativeSequence = new QualitativeSequence(alphabet,
                        expectedFormatType, Encodings.IupacNA, inputSequence);

                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore, Convert.ToInt32(expectedMaxScore, (IFormatProvider)null));
                    }

                    // Validate encoding,Format type and qualitative sequence.
                    Assert.IsTrue(createdQualitativeSequence.IsReadOnly);
                    Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSequence);
                    Assert.AreEqual(createdQualitativeSequence.Type, expectedFormatType);
                    Assert.AreEqual(createdQualitativeSequence.Encoding, Encodings.IupacNA);
                    break;

                case QualitativeSequenceParameters.NcbiEAA:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                    Encodings.NcbiEAA, inputSequence);

                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore, Convert.ToInt32(expectedMaxScore, (IFormatProvider)null));
                    }

                    // Validate encoding,Format type and qualitative sequence.
                    Assert.IsTrue(createdQualitativeSequence.IsReadOnly);
                    Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSequence);
                    Assert.AreEqual(createdQualitativeSequence.Type, expectedFormatType);
                    Assert.AreEqual(createdQualitativeSequence.Encoding, Encodings.NcbiEAA);
                    break;
                case QualitativeSequenceParameters.IupacNAWithScore:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                        Encodings.IupacNA, inputSequence, Convert.ToByte(inputScoreforIUPAC, (IFormatProvider)null));

                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore, Convert.ToInt32(expectedMaxScore, (IFormatProvider)null));
                    }

                    // Validate encoding,Format type and qualitative sequence.
                    Assert.IsTrue(createdQualitativeSequence.IsReadOnly);
                    Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSequence);
                    Assert.AreEqual(createdQualitativeSequence.Type, expectedFormatType);
                    Assert.AreEqual(createdQualitativeSequence.Encoding, Encodings.IupacNA);
                    break;

                case QualitativeSequenceParameters.Score:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                        inputSequence, Convert.ToByte(inputScore, (IFormatProvider)null));
                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore, Convert.ToInt32(expectedOuptutScore, (IFormatProvider)null));
                    }
                    Assert.IsTrue(createdQualitativeSequence.IsReadOnly);
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
                case QualitativeSequenceParameters.EncoderWithScore:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                        Encodings.IupacNA, inputSequence, Convert.ToByte(inputScore, (IFormatProvider)null));

                    // Validate created qualitative sequence.
                    Assert.AreEqual(createdQualitativeSequence.Encoding, Encodings.IupacNA);
                    Assert.IsTrue(createdQualitativeSequence.IsReadOnly);
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore,
                            Convert.ToInt32(expectedOuptutScore, (IFormatProvider)null));
                    }

                    Assert.AreEqual(MoleculeType.Invalid, createdQualitativeSequence.MoleculeType);
                    Assert.IsTrue(string.IsNullOrEmpty(createdQualitativeSequence.DisplayID));
                    Assert.IsNull(createdQualitativeSequence.Documentation);
                    Assert.AreEqual(0, createdQualitativeSequence.Metadata.Count);

                    byte[] tmpEncodedSeq = new byte[createdQualitativeSequence.Count];
                    (createdQualitativeSequence as IList<byte>).CopyTo(tmpEncodedSeq, 0);
                    for (int i = 0; i < tmpEncodedSeq.Length; i++)
                    {
                        Assert.AreEqual(encodedValues[i],
                            tmpEncodedSeq[i].ToString((IFormatProvider)null));
                    }

                    Assert.AreEqual(expectedRevComplement,
                        createdQualitativeSequence.ReverseComplement.ToString());
                    break;
                case QualitativeSequenceParameters.Encoder:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                        Encodings.IupacNA, inputSequence, Convert.ToByte(inputScore, (IFormatProvider)null));

                    // Validate created qualitative sequence.
                    Assert.AreEqual(createdQualitativeSequence.Encoding, Encodings.IupacNA);
                    Assert.IsTrue(createdQualitativeSequence.IsReadOnly);
                    Assert.AreEqual(expectedRevComplement,
                        createdQualitativeSequence.ReverseComplement.ToString());
                    break;
                case QualitativeSequenceParameters.EncoderWithByteValue:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                        Encodings.IupacNA, inputSequence, byteArray);
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
                "Qualitative Sequence P1:Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence.ToString()));

            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P1:Qualitative Sequence Score {0} is as expected.",
                createdQualitativeSequence.Scores.Length.ToString((IFormatProvider)null)));

            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P1:Qualitative format type {0} is as expected.",
                createdQualitativeSequence.Type));
        }

        /// <summary>
        /// General method to validate addition of sequence.items to qualitative Sequence.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="SeqItemWithScore">String value "SeqWithScore".</param>
        /// </summary>
        void ValidateAdditionOfSequenceItems(string nodeName, string SeqItemWithScore)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string expectedSeqAfterAddSeqItem = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSeqAfterAdd);
            string expectedSeqCountAfterAdd = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceCountAfterAdd);
            string inputScore = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InputScoreNode);

            // Create a qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                alphabet, expectedFormatType, inputSequence);

            // Add a sequence item to qualitative Sequence.
            createdQualitativeSequence.IsReadOnly = false;
            if (0 == string.Compare(SeqItemWithScore, "SeqWithScore", true, CultureInfo.CurrentCulture))
            {
                createdQualitativeSequence.Add(createdQualitativeSequence[0], Convert.ToByte(inputScore, (IFormatProvider)null));
            }
            else
            {
                createdQualitativeSequence.Add(createdQualitativeSequence[0]);
            }

            // Validate quality Sequence after addition of Seq Item.
            Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSeqAfterAddSeqItem);
            Assert.AreEqual(createdQualitativeSequence.Count.ToString((IFormatProvider)null), expectedSeqCountAfterAdd);
            Assert.AreEqual(createdQualitativeSequence.Scores.Length.ToString((IFormatProvider)null), expectedSeqCountAfterAdd);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P1 : Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence.ToString()));
        }

        /// <summary>
        /// General method to validate CopyTo method of qualitative Sequence.
        /// <param name="nodeName">xml node name.</param>
        /// </summary>
        void ValidateCopiedQualitativeSeqItems(string nodeName)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            ISequenceItem[] iSeqItems = new ISequenceItem[26];
            int index = 0;

            // Create a qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                alphabet, expectedFormatType, inputSequence);

            // Copy sequence items to array and validate.
            createdQualitativeSequence.CopyTo(iSeqItems, index);

            // Validate array.
            for (index = 0; index < 26; index++)
            {
                Assert.AreEqual(iSeqItems[index], createdQualitativeSequence[index]);
                Assert.AreEqual(iSeqItems[index], createdQualitativeSequence[index]);
                Assert.AreEqual(iSeqItems[index].Symbol, createdQualitativeSequence[index].Symbol);

                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Qualitative Sequence P1:Qualitative Sequence {0} is as expected.",
                    createdQualitativeSequence[index].Symbol));
            }
        }

        /// <summary>
        /// General method to validate Index of Qualitative Sequence Items.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="indexParam">Different Qualitative Sequence parameters.</param>
        /// </summary>
        void ValidateGeneralQualitativeSeqItemIndices(
            string nodeName, QualitativeSequenceParameters indexParam)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string expectedFirstItemIdex = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FirstItemIndex);
            string expectedLastItemIdex = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.LastItemIndex);
            string expectedGapIndex = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.IndexOfGap);
            int lastItemIndex;
            int index;

            // Create a qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                alphabet, expectedFormatType, inputSequence);
            createdQualitativeSequence.IsReadOnly = false;

            // Get a Index of qualitative sequence items
            switch (indexParam)
            {
                case QualitativeSequenceParameters.IndexOf:
                    index = createdQualitativeSequence.IndexOf(createdQualitativeSequence[0]);
                    lastItemIndex = createdQualitativeSequence.IndexOf(createdQualitativeSequence[25]);

                    // Validate Qualitative sequence item indices.
                    Assert.AreEqual(index, Convert.ToInt32(expectedFirstItemIdex, (IFormatProvider)null));
                    Assert.AreEqual(lastItemIndex, Convert.ToInt32(expectedFirstItemIdex, (IFormatProvider)null));
                    break;
                case QualitativeSequenceParameters.IndexOfNonGap:
                    index = createdQualitativeSequence.IndexOfNonGap();

                    // Validate Qualitative sequence item indices.
                    Assert.AreEqual(index, Convert.ToInt32(expectedFirstItemIdex, (IFormatProvider)null));
                    break;
                case QualitativeSequenceParameters.IndexOfNonGapWithParam:
                    index = createdQualitativeSequence.IndexOfNonGap(5);

                    // Validate Qualitative sequence item indices.
                    Assert.AreEqual(index, Convert.ToInt32(expectedGapIndex, (IFormatProvider)null));
                    break;
                case QualitativeSequenceParameters.LastIndexOf:
                    lastItemIndex = createdQualitativeSequence.LastIndexOfNonGap();

                    // Validate Qualitative sequence item indices.
                    Assert.AreEqual(lastItemIndex, Convert.ToInt32(expectedLastItemIdex, (IFormatProvider)null));
                    break;
                case QualitativeSequenceParameters.LastIndexOfWithPam:
                    lastItemIndex = createdQualitativeSequence.LastIndexOfNonGap(5);

                    // Validate Qualitative sequence item indices.
                    Assert.AreEqual(lastItemIndex, Convert.ToInt32(expectedGapIndex, (IFormatProvider)null));
                    break;
                default:
                    break;
            }

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P1 : Qualitative SequenceItems {0} indices validation completed successfully.",
                createdQualitativeSequence.IndexOf(createdQualitativeSequence[0])));
        }

        /// <summary>
        /// Validate insertion of sequence items in qualitative sequence.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="paramName">Different qualitative sequence method name.</param>
        /// </summary>
        void ValidateQualitativeSequenceMethods(
            string nodeName, QualitativeSequenceParameters paramName)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string newByteScore = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.NewScoreToInsert);
            string expectedSeqAfterInsertSeqItem = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSeqAfterAdd);
            string expectedSeqAfterInsertSeq = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceAfterInsertString);
            string SequenceToInsert = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ItemToBeInserted);
            string byteScoretoInsert = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InsertByteArray);
            string expectedSequenceWithinRange = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.QualSeqWithinRange);
            string expectedSeqAfterRemoveSeqItem = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceAfterRemove);
            string expectedSeqAfterRemoveSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceAfterRemoveWithRange);
            string expectedSeqAfterReplace = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceAferReplace);
            string expectedSeqAfterReplaceWithByteArray = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceAfterReplaceSeq);
            string expectedScoreLength = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedScore);
            string ReplaceChar = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ReplaceChar);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(byteScoretoInsert);
            SerializationInfo info = new SerializationInfo(typeof(QualitativeSequence),
                new FormatterConverter());
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            // Create a Qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                alphabet, expectedFormatType, inputSequence);
            createdQualitativeSequence.IsReadOnly = false;

            switch (paramName)
            {
                case QualitativeSequenceParameters.InsertItem:
                    // Insert sequence item at first position.
                    createdQualitativeSequence.Insert(26, createdQualitativeSequence[0],
                        byteArray[0]);

                    // Validate sequence after inserting sequence item.
                    Assert.AreEqual(createdQualitativeSequence.ToString(),
                        expectedSeqAfterInsertSeqItem);
                    break;
                case QualitativeSequenceParameters.InsertChar:
                    createdQualitativeSequence.Insert(26, SequenceToInsert[0],
                        byteArray[0]);

                    // Validate sequence after inserting sequence item.
                    Assert.AreEqual(createdQualitativeSequence.ToString(),
                        expectedSeqAfterInsertSeqItem);
                    break;
                case QualitativeSequenceParameters.InsertRange:
                    createdQualitativeSequence.InsertRange(20, SequenceToInsert,
                        Convert.ToByte(newByteScore, (IFormatProvider)null));

                    // Validate sequence after inserting sequence item.
                    Assert.AreEqual(createdQualitativeSequence.ToString(),
                        expectedSeqAfterInsertSeq);
                    break;
                case QualitativeSequenceParameters.InsertCharWithByteArray:
                    createdQualitativeSequence.InsertRange(20, SequenceToInsert,
                        byteArray);

                    // Validate sequence after inserting sequence item.
                    Assert.AreEqual(createdQualitativeSequence.ToString(),
                        expectedSeqAfterInsertSeq);
                    break;
                case QualitativeSequenceParameters.Range:
                    ISequence qualSeqRange = createdQualitativeSequence.Range(0,
                        Constants.SequenceLength);

                    // Validate Qualitative Sequence Within specified range.
                    Assert.AreEqual(qualSeqRange.ToString(), expectedSequenceWithinRange);
                    break;
                case QualitativeSequenceParameters.RemoveAt:
                    createdQualitativeSequence.Add(createdQualitativeSequence[0]);

                    // Remove Qual Seq Item at 0th Position.
                    createdQualitativeSequence.RemoveAt(0);

                    // Validate Qualitative Sequence after removing sequence item.
                    Assert.AreEqual(createdQualitativeSequence.ToString(),
                        expectedSeqAfterRemoveSeqItem);
                    break;
                case QualitativeSequenceParameters.RemoveRange:

                    // Remove Qual Seq with Range
                    createdQualitativeSequence.RemoveRange(0, Constants.SequenceLength);

                    // Validate Qualitative Sequence after removing sequence.
                    Assert.AreEqual(createdQualitativeSequence.ToString(),
                        expectedSeqAfterRemoveSequence);
                    break;
                case QualitativeSequenceParameters.Replace:
                    createdQualitativeSequence.Replace(2, SequenceToInsert[0]);

                    // Valildate Sequence after replacing with other sequence item.
                    Assert.AreEqual(createdQualitativeSequence.ToString(),
                        expectedSeqAfterReplace);
                    Assert.AreEqual(createdQualitativeSequence.Scores.Length.ToString((IFormatProvider)null),
                        expectedScoreLength);
                    break;
                case QualitativeSequenceParameters.ReplaceWithScore:
                    createdQualitativeSequence.Replace(2, SequenceToInsert[0],
                        Convert.ToByte(newByteScore, (IFormatProvider)null));

                    // Valildate Sequence after replacing with other sequence item.
                    Assert.AreEqual(createdQualitativeSequence.ToString(),
                        expectedSeqAfterReplace);
                    Assert.AreEqual(createdQualitativeSequence.Scores.Length.ToString((IFormatProvider)null),
                        expectedScoreLength);
                    break;
                case QualitativeSequenceParameters.ReplaceRange:
                    createdQualitativeSequence.ReplaceRange(Constants.StartPosition,
                        SequenceToInsert, Convert.ToByte(newByteScore, (IFormatProvider)null));

                    // Valildate Sequence after replacing with other sequence item.
                    Assert.AreEqual(createdQualitativeSequence.ToString(),
                        expectedSeqAfterReplaceWithByteArray);
                    Assert.AreEqual(createdQualitativeSequence.Scores.Length.ToString((IFormatProvider)null),
                        expectedScoreLength);
                    break;

                case QualitativeSequenceParameters.ReplaceRangeWithByteArray:
                    createdQualitativeSequence.ReplaceRange(Constants.StartPosition,
                        SequenceToInsert, byteArray);

                    // Valildate Sequence after replacing with other sequence item.
                    Assert.AreEqual(createdQualitativeSequence.ToString(),
                        expectedSeqAfterReplaceWithByteArray);
                    Assert.AreEqual(createdQualitativeSequence.Scores.Length.ToString((IFormatProvider)null),
                        expectedScoreLength);
                    break;
                case QualitativeSequenceParameters.ReplaceQualityScore:
                    createdQualitativeSequence.Replace(0, Convert.ToByte(newByteScore, (IFormatProvider)null));

                    // Valildate replaced scores.
                    Assert.AreEqual(newByteScore, createdQualitativeSequence.Scores[0].ToString((IFormatProvider)null));
                    break;
                case QualitativeSequenceParameters.ReplaceChar:
                    createdQualitativeSequence.Replace(0, ReplaceChar[0]);

                    // Validate replaced char.
                    Assert.AreEqual(ReplaceChar[0], createdQualitativeSequence[0].Symbol);
                    break;
                case QualitativeSequenceParameters.GetEnumerator:
                    IEnumerator<ISequenceItem> list = createdQualitativeSequence.GetEnumerator();
                    int index = 0;

                    //Retrives an enumerator for the qualitative sequence.
                    while (list.MoveNext())
                    {
                        Assert.AreEqual(list.Current.Symbol, createdQualitativeSequence[index].Symbol);
                        index++;
                    }
                    break;
                case QualitativeSequenceParameters.GetObjectData:
                    createdQualitativeSequence.GetObjectData(info, context);
                    Assert.IsNotNull(createdQualitativeSequence);
                    break;
                case QualitativeSequenceParameters.ReplaceRangeQualityScores:
                    createdQualitativeSequence.ReplaceRange(0, byteArray);

                    // Valildate replaced scores.
                    for (int i = 0; i < byteArray.Length; i++)
                    {
                        Assert.IsTrue(createdQualitativeSequence.Scores.Contains(byteArray[i]));
                    }
                    break;
                default:
                    break;
            }

            // Log Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence P1:Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence.ToString()));
        }

        /// <summary>
        /// General method to validate default score for different FastQ 
        /// format with different sequence.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="parameters">Different Qualitative Score method parameter.</param>
        /// </summary>
        void ValidateFastQDefaultScores(
            string nodeName, QualitativeSequenceParameters parameters)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            string inputSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string expectedMaxScore = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DefualtMaxScore);
            string expectedMinScore = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DefaultMinScore);

            QualitativeSequence createdQualitativeSequence = null;

            switch (parameters)
            {
                case QualitativeSequenceParameters.DefaultScoreWithAlphabets:
                    createdQualitativeSequence = new QualitativeSequence(
                        alphabet, expectedFormatType);

                    // Validate default score.
                    foreach (byte qualitativeScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualitativeScore,
                            QualitativeSequence.GetDefaultQualScore(expectedFormatType));
                    }

                    // Log Nunit GUI.
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "Qualitative Sequence P1:Qualitative Sequence Default score {0} is as expected.",
                        QualitativeSequence.GetDefaultQualScore(expectedFormatType)));
                    break;
                case QualitativeSequenceParameters.DefaultScoreWithSequence:
                    createdQualitativeSequence = new QualitativeSequence(alphabet,
                        expectedFormatType, inputSequence);

                    // Validate default score.
                    foreach (byte qualitativeScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualitativeScore,
                            QualitativeSequence.GetDefaultQualScore(expectedFormatType));
                    }

                    // Log Nunit GUI.
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "Qualitative Sequence P1:Qualitative Sequence Default score {0} is as expected.",
                        QualitativeSequence.GetDefaultQualScore(expectedFormatType)));
                    break;
                case QualitativeSequenceParameters.MaxDefaultScore:
                    createdQualitativeSequence = new QualitativeSequence(
                        alphabet, expectedFormatType, inputSequence,
                        Convert.ToByte(expectedMaxScore, (IFormatProvider)null));

                    // Validate default maximum score.
                    foreach (byte qualitativeScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualitativeScore,
                            QualitativeSequence.GetMaxQualScore(expectedFormatType));
                    }

                    // Log Nunit GUI.
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "Qualitative Sequence P1:Qualitative Sequence Maximum score {0} is as expected.",
                        QualitativeSequence.GetMaxQualScore(expectedFormatType)));
                    break;
                case QualitativeSequenceParameters.MinDefaultScore:
                    createdQualitativeSequence = new QualitativeSequence(
                        alphabet, expectedFormatType, inputSequence,
                        Convert.ToByte(expectedMinScore, (IFormatProvider)null));

                    // Validate default minimum score.
                    foreach (byte qualitativeScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualitativeScore,
                            QualitativeSequence.GetMinQualScore(expectedFormatType));
                    }

                    // Log Nunit GUI.
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "Qualitative Sequence P1:Qualitative Sequence Minimum score {0} is as expected.",
                        QualitativeSequence.GetMinQualScore(expectedFormatType)));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Validate Qualitative sequence General methods with DV enabled.
        /// </summary>
        void ValidateQualSequenceGeneralMethods(QualitativeSequenceParameters pams)
        {
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes("hhhhh");

            FileVirtualQualitativeSequenceProvider provObj =
                GetVirtualSequenceProvider();

            QualitativeSequence seqObj =
                 new QualitativeSequence(Alphabets.DNA, FastQFormatType.Illumina, "AGGCT");

            seqObj.VirtualQualitativeSequenceProvider = provObj;
            seqObj.IsReadOnly = false;
            switch (pams)
            {
                case QualitativeSequenceParameters.InsertRange:
                    seqObj.InsertRange(0, "AGGCT", 66);
                    Assert.AreEqual("AGGCTGGCGCACTTACACCCTACATCCATTG", seqObj.ToString());
                    break;
                case QualitativeSequenceParameters.InsertCharWithByteArray:

                    seqObj.InsertRange(0, "AGGCT", byteArray);
                    Assert.AreEqual("AGGCTGGCGCACTTACACCCTACATCCATTG", seqObj.ToString());
                    break;
                case QualitativeSequenceParameters.ReplaceRange:

                    seqObj.ReplaceRange(0, "AGGCT", 66);
                    Assert.AreEqual("AGGCTACTTACACCCTACATCCATTG", seqObj.ToString());
                    break;
                case QualitativeSequenceParameters.ReplaceRangeQualityScores:

                    seqObj.ReplaceRange(0, "AGGCT", byteArray);
                    Assert.AreEqual("AGGCTACTTACACCCTACATCCATTG", seqObj.ToString());
                    break;
                case QualitativeSequenceParameters.IndexOf:

                    int index = seqObj.IndexOf(seqObj[0]);
                    Assert.AreEqual(0, index);
                    break;
                default: break;
            }

            ApplicationLog.WriteLine(@"Qualitative sequence P1 : Successfully validated the InsertRange() method");
            Console.WriteLine(@"Qualitative sequence P1 : Successfully validated the InsertRange() method");
        }

        /// <summary>
        /// Gets the VirtualSequenceProvider
        /// </summary>
        /// <returns>Virtual Sequence Provider</returns>
        FileVirtualQualitativeSequenceProvider GetVirtualSequenceProvider()
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SingleSequenceSangerFastQNode, Constants.FilePathNode);
            FastQParser parserObj = null;
            FileVirtualQualitativeSequenceProvider provObj = null;
            try
            {
                parserObj = new FastQParser();

                parserObj.Parse(filePath);

                provObj = new FileVirtualQualitativeSequenceProvider(parserObj,
                    GetSequencePointer());
            }
            finally
            {
                if (parserObj != null)
                    parserObj.Dispose();
            }
            return provObj;
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
        #endregion Supporting Methods
    }
}
