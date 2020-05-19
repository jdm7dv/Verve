// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * QualitativeSequenceBVTTestCases.cs
 * 
 * This file contains the Qualitative BVT test cases.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;

using MBF.Encoding;
using MBF.IO;
using MBF.IO.Fasta;
using MBF.IO.GenBank;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using NUnit.Framework;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Qalitative sequence validations.
    /// </summary>
    [TestFixture]
    public class QualitativeSequenceBvtTestCases
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
            Encoding,
            ByteArray,
            InsertItem,
            InsertChar,
            Replace,
            ReplaceRange,
            Default
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static QualitativeSequenceBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\QualitativeTestsConfig.xml");
        }

        #endregion Constructor

        #region Qualitative Sequence Test Cases

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna alphabet.
        /// Input Data : Dna Alphabet.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateQualitativeSequence()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaAlphabetNode,
                QualitativeSequenceParameters.Alphabets);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna 
        /// alphabet with "Sanger" format type.
        /// Input Data : Dna Alphabet and "Sanger" FastQ format type.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateQualitativeSequenceWithSangerFormatType()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaAlphabetNode,
                QualitativeSequenceParameters.FormatType);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna 
        /// alphabet with "Solexa" format type.
        /// Input Data : Dna Alphabet and "Solexa" FastQ format type.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateQualitativeSequenceWithSolexaFormatType()
        {
            GeneralQualitativeSequence(Constants.SolexaTypeNode,
                QualitativeSequenceParameters.FormatType);
        }
        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna 
        /// alphabet with "Illumina" format type.
        /// Input Data : Dna Alphabet and "Illumina" FastQ format type.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateQualitativeSequenceWithIlluminaFormatType()
        {
            GeneralQualitativeSequence(Constants.IlluminaTypeNode,
                QualitativeSequenceParameters.FormatType);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Sanger FastQFormat.
        /// Input Data : Dna Alphabet,Dna Sequence and "Sanger" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateDnaQualitativeSequenceWithSangerFormatType()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSangerNode,
                QualitativeSequenceParameters.Sequence);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with "Illimina" FastQFormat
        /// Input Data : Dna Alphabet,Dna Sequence and "Illumina" FastQFormat..
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateDnaQualitativeSequenceWithIlluminaFormatType()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaIlluminaNode,
                QualitativeSequenceParameters.Sequence);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence.
        /// with "Solexa" FastQFormat
        /// Input Data : Dna Alphabet,Dna Sequence and "Solexa" FastQFormat..
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateDnaQualitativeSequenceWithSolexaFormatType()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSolexaNode,
                QualitativeSequenceParameters.Sequence);
        }


        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with "Illimina" FastQFormat with Ncbi2NA Encoding
        /// Input Data : Dna Alphabet,Dna Sequence and "Illumina" FastQFormat..
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateDnaQualitativeSequenceWithIlluminaFormatTypeUsingEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaIlluminaNode,
                QualitativeSequenceParameters.Encoding);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence.
        /// with "Solexa" FastQFormat with Ncbi2NA Encoding.
        /// Input Data : Dna Alphabet,Dna Sequence and "Solexa" FastQFormat..
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateDnaQualitativeSequenceWithSolexaFormatTypeUsingEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSolexaNode,
                QualitativeSequenceParameters.Encoding);
        }


        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Sanger FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat.
        /// and Score "120" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateSangerFormatTypeDnaQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSangerNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Solexa FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat.
        /// and Score "104" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateSolexaFormatTypeDnaQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSolexaNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Illumina FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat.
        /// and Score "104" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateIlluminaFormatTypeDnaQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaIlluminaNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Sanger FastQFormat and Iupac encoding.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat.
        /// and Score "120" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateSangerFormatTypeDnaQualitativeSequenceWithIupacNAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSangerNode,
                QualitativeSequenceParameters.Encoding);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Solexa FastQFormat and Iupac encoding.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat.
        /// and Score "104" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateSolexaFormatTypeDnaQualitativeSequenceWithIupacNAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSolexaNode,
                QualitativeSequenceParameters.Encoding);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Illumina FastQFormat and Iupac encoding.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat.
        /// and Score "104" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateIlluminaFormatTypeDnaQualitativeSequenceWithIupacNAEncoding()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaIlluminaNode,
                QualitativeSequenceParameters.Encoding);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Solexa FastQFormat and Byte array.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence with score.
        /// </summary>
        [Test]
        public void ValidateSolexaFormatTypeDnaQualitativeSequenceWithByteArray()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSolexaNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Illumina FastQFormat and Byte array.
        /// Input Data : Dna Alphabet,Dna Sequence,"Illumina" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence with score.
        /// </summary>
        [Test]
        public void ValidateIlluminaFormatTypeDnaQualitativeSequenceWithByteArray()
        {
            GeneralQualitativeSequence(Constants.SimpleDNAIlluminaByteArrayNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Sanger FastQFormat and Byte array.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence with score.
        /// </summary>
        [Test]
        public void ValidateSangerFormatTypeDnaQualitativeSequenceWithByteArray()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSangerNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate addition of Qualitative Sequence item.
        /// Input Data : Dna Sequence.
        /// Output Data : Validate addition of Sequence item to Quality Sequence.
        /// </summary>
        [Test]
        public void ValidateAdditionOfSeqItemForDnaSequence()
        {
            ValidateAdditionOfSequenceItems(Constants.SimpleDnaSolexaNode);
        }

        /// <summary>
        /// Validate addition of Qualitative Sequence item for Rna Sequence.
        /// Input Data : Rna Sequence.
        /// Output Data : Validate addition of Sequence item to Quality Sequence.
        /// </summary>
        [Test]
        public void ValidateAdditionOfSeqItemForRnaSequence()
        {
            ValidateAdditionOfSequenceItems(Constants.SimpleRnaSolexaNode);
        }

        /// <summary>
        /// Validate clear Qualitative Sequence
        /// Input Data : Dna Sequence.
        /// Output Data :Empty Qualitative Sequence..
        /// </summary>
        [Test]
        public void ValidateClearSequenceItems()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
            Constants.SimpleDnaSolexaNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
            Utility._xmlUtil.GetTextValue(Constants.SimpleDnaSolexaNode,
            Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = Utility._xmlUtil.GetTextValue(
            Constants.SimpleDnaSolexaNode, Constants.inputSequenceNode);

            // Create a Qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                    alphabet, expectedFormatType, inputSequence);

            // Clear a sequence items from Qualitative Sequence.
            createdQualitativeSequence.IsReadOnly = false;
            createdQualitativeSequence.Clear();

            // Validate Qualitative Sequence after addition of Seq Item.
            Assert.IsEmpty(createdQualitativeSequence.ToString());
            Assert.AreEqual(createdQualitativeSequence.Count, 0);
            Assert.AreEqual(createdQualitativeSequence.Scores.Length, 0);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT: Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence.ToString()));
        }


        /// <summary>
        /// Validate Quality Sequence items present in  Qualitative Sequence.
        /// Input Data : Dna Sequence.
        /// Output Data :Validatation of Quality Sequence items using Contains()
        /// method.
        /// </summary>
        [Test]
        public void ValidateQualitySequenceItems()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
            Constants.SimpleDnaSolexaNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
            Utility._xmlUtil.GetTextValue(Constants.SimpleDnaSolexaNode, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = Utility._xmlUtil.GetTextValue(
            Constants.SimpleDnaSolexaNode, Constants.inputSequenceNode);

            // Create a quality Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                    alphabet, expectedFormatType, inputSequence);

            // Add a sequence item to Qualitative Sequence.
            createdQualitativeSequence.IsReadOnly = false;
            createdQualitativeSequence.Add(createdQualitativeSequence[0]);

            // Validate quality SequenceItems after addition of Seq Item.
            Assert.IsTrue(createdQualitativeSequence.Contains(createdQualitativeSequence[25]));
            Assert.IsTrue(createdQualitativeSequence.Contains(createdQualitativeSequence[10]));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT: Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence[25]));
        }

        /// <summary>
        /// Validate Quality Sequence Cloning..
        /// Input Data : Dna Sequence,Alphabet and format Type.
        /// Output Data :Validatation of Quality Sequence Cloning.
        /// method.
        /// </summary>
        [Test]
        public void ValidateQualitySequenceClonig()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                Utility._xmlUtil.GetTextValue(Constants.SimpleDnaSolexaNode,
                Constants.FastQFormatType));
            string expectedScore = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.ExpectedScore);
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.inputSequenceNode);
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.ExpectedSequenceNode);
            string expectedSequenceCount = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.QSequenceCount);

            // Create a quality Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                    alphabet, expectedFormatType, inputSequence);

            // Create a cloned Qualitative Sequence.
            QualitativeSequence ClonedQualitySequence = createdQualitativeSequence.Clone();

            // Validate Cloned Sequence.
            Assert.IsNotNull(ClonedQualitySequence);
            Assert.AreEqual(ClonedQualitySequence.Alphabet, alphabet);
            Assert.AreEqual(ClonedQualitySequence.ToString(), expectedSequence);
            Assert.AreEqual(ClonedQualitySequence.Count.ToString(), expectedSequenceCount);
            Assert.AreEqual(ClonedQualitySequence.Scores.Length.ToString(), expectedScore);
            Assert.AreEqual(ClonedQualitySequence.Type, expectedFormatType);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Cloned Qualitative Sequence {0} is as expected.",
                ClonedQualitySequence.ToString()));
        }

        /// <summary>
        /// Validate Remove Qualitative Sequence Items.
        /// Input Data : Dna Sequence.
        /// Output Data :Validate removed Qualitative Sequence. Items.
        /// </summary>
        [Test]
        public void ValidateRemoveSequenceItems()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                Utility._xmlUtil.GetTextValue(Constants.SimpleDnaSolexaNode, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.inputSequenceNode);
            string expectedSeqAfterRemoveSeqItem = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.ExpectedSequenceAfterRemove);

            // Create a quality Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                    alphabet, expectedFormatType, inputSequence);

            // Add a sequence item to Qualitative Sequence.
            createdQualitativeSequence.IsReadOnly = false;
            createdQualitativeSequence.Add(createdQualitativeSequence[0]);

            //Remove sequence Items from Qualitative sequence.
            createdQualitativeSequence.Remove(createdQualitativeSequence[0]);

            // Validate quality Sequence after removing Seq Item.
            Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSeqAfterRemoveSeqItem);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT: Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence.ToString()));
        }

        /// <summary>
        /// Validate CopyTo method of Qualitative Sequence.
        /// Input Data : Dna Sequence, Sanger format.
        /// Output Data :Validate copied sequence items in array.
        /// </summary>
        [Test]
        public void ValidateCopiedQualitativeSeqItems()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                Utility._xmlUtil.GetTextValue(Constants.SimpleDnaSolexaNode, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.inputSequenceNode);
            ISequenceItem[] iSeqItems = new ISequenceItem[26];
            int index = 0;

            // Create a Qualitative Sequence.
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

                Console.WriteLine(string.Format(null,
                    "Qualitative Sequence BVT:Qualitative Sequence {0} is as expected.",
                    createdQualitativeSequence[index].Symbol));
            }
        }

        /// <summary>
        /// Validate Inserting Sequence itmes in Qualitative sequence
        /// Input Data : Dna Sequence.
        /// Output Data :Validate insertion of Sequence items.
        /// </summary>
        [Test]
        public void ValidateInsertSequenceItems()
        {
            ValidateInsertionOfSequenceItems(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.InsertItem);
        }

        /// <summary>
        /// Validate Insertion of characters in Qualitative sequence
        /// Input Data : Dna Sequence.
        /// Output Data :Validate insertion of characters in Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateInsertChars()
        {
            ValidateInsertionOfSequenceItems(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.InsertChar);
        }

        /// <summary>
        /// Validate Insertion of Sequence in Qualitative sequence
        /// using InsertRange() method.
        /// Input Data : Dna Sequence.
        /// Output Data :Validate insertion of sequence in Qualitative sequence.
        /// </summary>
        [Test]
        public void ValidateInsertSequenceWithInsertRange()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                Utility._xmlUtil.GetTextValue(Constants.SimpleDnaSolexaNode, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.inputSequenceNode);
            string expectedSeqAfterInsertSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.ExpectedSequenceAfterInsertSeq);
            string SequenceToInsert = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.ItemToBeInserted);

            // Create a Qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                    alphabet, expectedFormatType, inputSequence);
            createdQualitativeSequence.IsReadOnly = false;

            // Insert sequence item at first position.
            createdQualitativeSequence.InsertRange(0, SequenceToInsert);

            // Validate sequence after inserting sequence item.
            Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSeqAfterInsertSequence);

            // Log Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence.ToString()));
        }

        /// <summary>
        /// Validate Remove Sequence within the specifeid range.
        /// using InsertRange() method.
        /// Input Data : Dna Sequence.
        /// Output Data :Validate remove part of sequence 
        /// with range specified.
        /// </summary>
        [Test]
        public void ValidateRemoveSequenceWithRange()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                Utility._xmlUtil.GetTextValue(Constants.SimpleDnaSolexaNode, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.inputSequenceNode);
            string SequenceToInsert = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.ItemToBeInserted);

            // Create a Qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                    alphabet, expectedFormatType, inputSequence);
            createdQualitativeSequence.IsReadOnly = false;

            // Insert sequence at first position.
            createdQualitativeSequence.InsertRange(Constants.StartPosition, SequenceToInsert);

            // Remove sequence with range specified.
            createdQualitativeSequence.RemoveRange(
                Constants.StartPosition, Constants.SequenceLength);

            // Validate sequence after removing part of sequence
            Assert.AreEqual(createdQualitativeSequence.ToString(), inputSequence);

            // Log Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence.ToString()));
        }

        /// <summary>
        /// Validate Replace Sequence Items.
        /// Input Data : Dna Sequence.
        /// Output Data :Validate replacing Sequence items with other sequence item.
        /// </summary>
        [Test]
        public void ValidateReplaceSequenceItem()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                Utility._xmlUtil.GetTextValue(Constants.SimpleDnaSolexaNode, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.inputSequenceNode);
            string expectedSeqAfterReplace = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.SequenceAferReplace);
            string SequenceToInsert = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.ItemToBeInserted);
            string expectedScoreLength = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaSolexaNode, Constants.ExpectedScore);

            // Create a Qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                alphabet, expectedFormatType, inputSequence);
            createdQualitativeSequence.IsReadOnly = false;

            //Replace sequence item with other seq item.
            createdQualitativeSequence.Replace(2, SequenceToInsert[0]);

            // Valildate Sequence sfter replacing with other sequence item.
            Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSeqAfterReplace);
            Assert.AreEqual(createdQualitativeSequence.Scores.Length.ToString(), expectedScoreLength);

            // Log Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence.ToString()));
        }

        /// <summary>
        /// Validate Replace Sequence string.
        /// Input Data : Dna Sequence.
        /// Output Data :Validate replacing Sequence string with other string..
        /// </summary>
        [Test]
        public void ValidateReplaceSequenceString()
        {
            ValidateReplaceSequenceWithRange(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.Replace);
        }

        /// <summary>
        /// Validate Replace Sequence string with score specified.
        /// Input Data : Dna Sequence.
        /// Output Data :Validate replacing Sequence string with other string..
        /// </summary>
        [Test]
        public void ValidateReplaceSequenceStringWithScore()
        {
            ValidateReplaceSequenceWithRange(
                Constants.SimpleDnaSolexaNode, QualitativeSequenceParameters.ReplaceRange);
        }

        /// <summary>
        /// Validate convert from Sanger to solexa and Illumina.
        /// Input Data : Sanger quality value, Sanger format sequence.
        /// Output Data :Validate convert from Sanger to Illumina and Solexa.
        /// </summary>
        [Test]
        public void ConvertSangerToSolexaAndIllumina()
        {
            // Gets the actual sequence and the Qual score from the Xml
            string sangerSequence = Utility._xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.SangerSequence);
            string expectedSolexaSequence = Utility._xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.SolexaSequence);
            string expectedIlluminaSequence = Utility._xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.IlluminaSequence);
            string sangerQualScore = Utility._xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.SangerQualScore);
            string expectedSolexaQualScore = Utility._xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.SolexaQualScore);
            string expectedIlluminaQualScore = Utility._xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.IlluminaQualScore);

            string solexaQualScore = null;
            string illuminaQualScore = null;
            byte[] scoreValue = ASCIIEncoding.ASCII.GetBytes(sangerQualScore);

            // Create a Sanger qualitative sequence.
            QualitativeSequence sangerQualSequence = new QualitativeSequence(
                Alphabets.DNA, FastQFormatType.Sanger, sangerSequence, scoreValue);

            // Convert Sanger to Solexa.
            QualitativeSequence solexaQualSequence = sangerQualSequence.ConvertTo(
                FastQFormatType.Solexa);
            solexaQualScore = ASCIIEncoding.ASCII.GetString(solexaQualSequence.Scores);

            // Validate converted solexa score.
            Assert.AreEqual(solexaQualScore, expectedSolexaQualScore);
            Assert.AreEqual(solexaQualSequence.ToString(), expectedSolexaSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Solexa score type {0} is as expected.",
                solexaQualScore));
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Solexa score type {0} is as expected.",
            solexaQualSequence.ToString()));

            // Convert Sanger to Illumina.
            QualitativeSequence illuminaQualSequence = sangerQualSequence.ConvertTo(
                FastQFormatType.Illumina);
            illuminaQualScore = ASCIIEncoding.ASCII.GetString(illuminaQualSequence.Scores);

            // Validate converted illumina score.
            Assert.AreEqual(illuminaQualScore, expectedIlluminaQualScore);
            Assert.AreEqual(illuminaQualSequence.ToString(), expectedIlluminaSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Illumina score type {0} is as expected.",
                illuminaQualScore));
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Illumina score type {0} is as expected.",
                illuminaQualSequence.ToString()));
        }

        /// <summary>
        /// Validate convert from Solexa to Sanger and Illumina.
        /// Input Data : Solexa quality value, Solexa format sequence.
        /// Output Data : Validate convert from Solexa to Sanger and Illumina.
        /// </summary>
        [Test]
        public void ConvertSolexaToSangerAndIllumina()
        {
            // Gets the actual sequence and the Qual score from the Xml
            string solexaSequence = Utility._xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.SolexaSequence);
            string expectedSangerSequence = Utility._xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.SangerSequence);
            string expectedIlluminaSequence = Utility._xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.IlluminaSequence);
            string solexaQualScore = Utility._xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.SolexaQualScore);
            string expectedSangerQualScore = Utility._xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.SangerQualScore);
            string expectedIlluminaQualScore = Utility._xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.IlluminaQualScore);
            byte[] byteValue = ASCIIEncoding.ASCII.GetBytes(solexaQualScore);
            string sangerQualScore = null;
            string illuminaQualScore = null;

            // Create a Solexa qualitative sequence.
            QualitativeSequence solexaQualSequence = new QualitativeSequence(Alphabets.DNA,
                FastQFormatType.Solexa, solexaSequence, byteValue);

            // Convert Solexa to Sanger.
            QualitativeSequence sangerQualSequence = solexaQualSequence.ConvertTo(
                FastQFormatType.Sanger);
            sangerQualScore = ASCIIEncoding.ASCII.GetString(sangerQualSequence.Scores);

            // Validate converted sanger score.
            Assert.AreEqual(sangerQualScore, expectedSangerQualScore);
            Assert.AreEqual(sangerQualSequence.ToString(), expectedSangerSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Sanger score type {0} is as expected.",
                sangerQualScore));
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Sanger score type {0} is as expected.",
                sangerQualSequence.ToString()));

            // Convert Solexa to Illumina.
            QualitativeSequence illuminaQualSequence =
                solexaQualSequence.ConvertTo(FastQFormatType.Illumina);
            illuminaQualScore = ASCIIEncoding.ASCII.GetString(illuminaQualSequence.Scores);

            // Validate converted illumina score.
            Assert.AreEqual(illuminaQualScore, expectedIlluminaQualScore);
            Assert.AreEqual(illuminaQualSequence.ToString(), expectedIlluminaSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Illumina score type {0} is as expected.",
                illuminaQualScore));
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Illumina score type {0} is as expected.",
                illuminaQualSequence.ToString()));
        }

        /// <summary>
        /// Validate convert from Illumina to Sanger and Solexa.
        /// Input Data : Illumina quality value, Illumina format sequence.
        /// Output Data : Validate convert from Illumina to Sanger and Solexa.
        /// </summary>
        [Test]
        public void ConvertIlluminaToSangerAndSolexa()
        {
            // Gets the actual sequence and the Qual score from the Xml
            string illuminaSequence = Utility._xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.IlluminaSequence);
            string expectedSangerSequence = Utility._xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.SangerSequence);
            string expectedSolexaSequence = Utility._xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.SolexaSequence);
            string illuminaQualScore = Utility._xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.IlluminaQualScore);
            string expectedSangerQualScore = Utility._xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.SangerQualScore);
            string expectedSolexaQualScore = Utility._xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.SolexaQualScore);
            byte[] byteValue = ASCIIEncoding.ASCII.GetBytes(illuminaQualScore);
            string sangerQualScore = null;
            string solexaQualScore = null;

            // Create a Illumina qualitative sequence.
            QualitativeSequence illuminaQualSequence = new QualitativeSequence(Alphabets.DNA,
            FastQFormatType.Illumina, illuminaSequence, byteValue);

            // Convert Illumina to Sanger.
            QualitativeSequence sangerQualSequence =
                illuminaQualSequence.ConvertTo(FastQFormatType.Sanger);
            sangerQualScore = ASCIIEncoding.ASCII.GetString(sangerQualSequence.Scores);

            // Validate converted sanger score.
            Assert.AreEqual(sangerQualScore, expectedSangerQualScore);
            Assert.AreEqual(sangerQualSequence.ToString(), expectedSangerSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Sanger score type {0} is as expected.",
                sangerQualScore));
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Sanger score type {0} is as expected.",
                sangerQualSequence.ToString()));

            // Convert Illumina to solexa.
            QualitativeSequence solexaQualSequence =
                illuminaQualSequence.ConvertTo(FastQFormatType.Solexa);
            solexaQualScore = ASCIIEncoding.ASCII.GetString(solexaQualSequence.Scores);

            // Validate converted illumina score.
            Assert.AreEqual(solexaQualScore, expectedSolexaQualScore);
            Assert.AreEqual(solexaQualSequence.ToString(), expectedSolexaSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Solexa format type {0} is as expected.",
                illuminaQualScore));
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Solexa format type {0} is as expected.",
                solexaQualSequence.ToString()));
        }

        #endregion QualitativeSequence Bvt TestCases

        #region Supporting Methods

        /// <summary>
        /// General method to validate creation of Qualitative sequence.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="parameters">Different Qualitative Sequence parameters.</param>
        /// </summary>
        static void GeneralQualitativeSequence(
            string nodeName, QualitativeSequenceParameters parameters)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                Utility._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            string expectedScore = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedScore);
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.QSequenceCount);
            string expectedMaxScore = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.MaxScoreNode);
            string inputScore = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.InputScoreNode);
            string expectedOuptutScore = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.InputScoreNode);
            IEncoding encoding = Encodings.IupacNA;
            string inputQuality = Utility._xmlUtil.GetTextValue(
            nodeName, Constants.InputByteArrayNode);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(inputQuality);
            int index = 0;

            // Create and validate Qualitative Sequence.
            switch (parameters)
            {
                case QualitativeSequenceParameters.Alphabets:
                    createdQualitativeSequence = new QualitativeSequence(alphabet);
                    Assert.IsFalse(createdQualitativeSequence.IsReadOnly);
                    break;
                case QualitativeSequenceParameters.FormatType:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType);
                    Assert.IsFalse(createdQualitativeSequence.IsReadOnly);
                    break;
                case QualitativeSequenceParameters.Sequence:
                    createdQualitativeSequence = new QualitativeSequence(
                    alphabet, expectedFormatType, inputSequence);
                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore, Convert.ToInt32(expectedMaxScore));
                    }
                    Assert.IsTrue(createdQualitativeSequence.IsReadOnly);
                    break;
                case QualitativeSequenceParameters.Encoding:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                    encoding, inputSequence);
                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore, Convert.ToInt32(expectedMaxScore));
                    }
                    Assert.IsTrue(createdQualitativeSequence.IsReadOnly);
                    break;
                case QualitativeSequenceParameters.Score:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                    inputSequence, Convert.ToByte(inputScore));
                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore, Convert.ToInt32(expectedOuptutScore));
                    }
                    Assert.IsTrue(createdQualitativeSequence.IsReadOnly);
                    break;
                case QualitativeSequenceParameters.ByteArray:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                    inputSequence, byteArray);

                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.Scores)
                    {
                        Assert.AreEqual(qualScore, Convert.ToInt32(byteArray[index]));
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
            Assert.AreEqual(createdQualitativeSequence.Count.ToString(), expectedSequenceCount);
            Assert.AreEqual(createdQualitativeSequence.Scores.Length.ToString(), expectedScore);
            Assert.AreEqual(createdQualitativeSequence.Type, expectedFormatType);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
            "Qualitative Sequence BVT:Qualitative Sequence {0} is as expected.",
            createdQualitativeSequence.ToString()));

            Console.WriteLine(string.Format(null,
            "Qualitative Sequence BVT:Qualitative Sequence Score {0} is as expected.",
             createdQualitativeSequence.Scores.Length.ToString()));

            Console.WriteLine(string.Format(null,
            "Qualitative Sequence BVT:Qualitative format type {0} is as expected.",
            createdQualitativeSequence.Type));
        }

        /// <summary>
        /// General method to validate addition of sequence.items to quality Sequence.
        /// <param name="nodeName">xml node name.</param>
        /// </summary>
        static void ValidateAdditionOfSequenceItems(string nodeName)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                Utility._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = Utility._xmlUtil.GetTextValue(nodeName, Constants.inputSequenceNode);
            string expectedSeqAfterAddSeqItem = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSeqAfterAdd);
            string expectedSeqCountAfterAdd = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceCountAfterAdd);

            // Create a quality Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                alphabet, expectedFormatType, inputSequence);

            // Add a sequence item to qualilty Sequence.
            createdQualitativeSequence.IsReadOnly = false;
            createdQualitativeSequence.Add(createdQualitativeSequence[0]);

            // Validate quality Sequence after addition of Seq Item.
            Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSeqAfterAddSeqItem);
            Assert.AreEqual(createdQualitativeSequence.Count.ToString(), expectedSeqCountAfterAdd);
            Assert.AreEqual(createdQualitativeSequence.Scores.Length.ToString(),
                expectedSeqCountAfterAdd);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT : Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence.ToString()));
        }

        /// <summary>
        /// Validate insertion of sequence items in qualitative sequence.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="paramName">Different qualitative sequence method name.</param>
        /// </summary>
        static void ValidateInsertionOfSequenceItems(
        string nodeName, QualitativeSequenceParameters paramName)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                Utility._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string newScore = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ScoreToBeUpdated);
            string expectedSeqAfterInsertSeqItem = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSeqAfterAdd);
            string charToInsert = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ItemToBeInserted);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(newScore);

            // Create a Qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                    alphabet, expectedFormatType, inputSequence);
            createdQualitativeSequence.IsReadOnly = false;

            switch (paramName)
            {
                case QualitativeSequenceParameters.InsertItem:
                    // Insert sequence item at first position.
                    createdQualitativeSequence.Insert(26, createdQualitativeSequence[0], byteArray[0]);
                    break;
                case QualitativeSequenceParameters.InsertChar:
                    createdQualitativeSequence.Insert(26, charToInsert[0], byteArray[0]);
                    break;
                default:
                    break;
            }

            // Validate sequence after inserting sequence item.
            Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSeqAfterInsertSeqItem);

            // Log Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence.ToString()));
        }

        /// <summary>
        /// Validate replacing Sequence items with string sequence.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="paramName">Different qualitatvie sequence methods name.</param>
        /// </summary>
        static void ValidateReplaceSequenceWithRange(
            string nodeName, QualitativeSequenceParameters paramName)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                Utility._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string expectedSeqAfterReplace = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceAfterReplaceSeq);
            string SequenceToInsert = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ItemToBeInserted);
            string expectedScoreLength = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedScore);
            string newScore = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ScoreToBeUpdated);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(newScore);

            // Create a Qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                alphabet, expectedFormatType, inputSequence);
            createdQualitativeSequence.IsReadOnly = false;

            switch (paramName)
            {
                case QualitativeSequenceParameters.Replace:
                    //Replace part of sequence with string.
                    createdQualitativeSequence.ReplaceRange(
                        Constants.StartPosition, SequenceToInsert);
                    break;
                case QualitativeSequenceParameters.ReplaceRange:
                    createdQualitativeSequence.ReplaceRange(
                        Constants.StartPosition, SequenceToInsert, byteArray);
                    break;
                default:
                    break;
            }

            // Valildate Sequence After replacing with string.
            Assert.AreEqual(createdQualitativeSequence.ToString(), expectedSeqAfterReplace);
            Assert.AreEqual(createdQualitativeSequence.Scores.Length.ToString(), expectedScoreLength);

            // Log Nunit GUI.
            Console.WriteLine(string.Format(null,
                "Qualitative Sequence BVT:Qualitative Sequence {0} is as expected.",
                createdQualitativeSequence.ToString()));
        }

        #endregion Supporting Methods
    }
}
