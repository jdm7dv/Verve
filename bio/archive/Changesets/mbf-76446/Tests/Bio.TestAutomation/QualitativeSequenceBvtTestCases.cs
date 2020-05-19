// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
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
using Bio.IO;
using Bio.IO.FastA;
using Bio.IO.GenBank;
using Bio.TestAutomation.Util;
using Bio.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bio;

namespace Bio.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Qalitative sequence validations.
    /// </summary>
    [TestClass]
    public class QualitativeSequenceBvtTestCases
    {

        #region Enums

        /// <summary>
        /// Qualitative Sequence method Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum QualitativeSequenceParameters
        {
            Score,
            ByteArray,
            Default
        };

        #endregion Enums

        #region Global Variables

        Utility utilityObj = new Utility(@"TestUtils\QualitativeTestsConfig.xml");
        ASCIIEncoding encodingObj = new ASCIIEncoding();

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static QualitativeSequenceBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("bio.automation.log");
            }
        }

        #endregion Constructor

        #region Qualitative Sequence Test Cases

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Sanger FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat.
        /// and Score "120" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSangerFormatTypeDnaQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSangerNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate Reverse and complement sequence.
        /// Input Data : Dna Alphabet,Dna Sequence,"Sanger" FastQFormat.
        /// Output Data : Validation of Reverse/Complement Sequence.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "compSequence"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "inputScoreArray"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "byteArray"), TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSangerFormatTypeDnaReverseComplement()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                utilityObj.xmlUtil.GetTextValue(Constants.SimpleDnaSangerNode, Constants.FastQFormatType));
            string inputSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.inputSequenceNode);
            string compSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.ComplementQualSeqNode);
            string expectedRevCompSeq = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.RevComplement);
            string expectedRevSeq = utilityObj.xmlUtil.GetTextValue(
                Constants.SimpleDnaSangerNode, Constants.ReverseQualSeq);
            //IEncoding encoding = Encodings.IupacNA;
            string inputQuality = utilityObj.xmlUtil.GetTextValue(
            Constants.SimpleDnaSangerNode, Constants.InputByteArrayNode);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(inputQuality);
            Byte[] inputScoreArray = encodingObj.GetBytes(inputQuality);


            QualitativeSequence createdQualitativeSequence =
                new QualitativeSequence(alphabet, expectedFormatType,
                    inputSequence, inputQuality);

            ISequence revSeq = createdQualitativeSequence.GetReversedSequence();
            ISequence revCompSeq = createdQualitativeSequence.GetReverseComplementedSequence();
            ISequence compSeq = createdQualitativeSequence.GetComplementedSequence();

            Assert.AreEqual(expectedRevSeq, new string(revSeq.Select(a => (char)a).ToArray()));
            Assert.AreEqual(expectedRevCompSeq, new string(revCompSeq.Select(a => (char)a).ToArray()));
            Assert.AreEqual(compSequence, new string(compSeq.Select(a => (char)a).ToArray()));

            ApplicationLog.WriteLine("Qualitative BVT: Successfully validated Reverse, Complement and ReverseComplement sequence");
            Console.WriteLine("Qualitative BVT: Successfully validated Reverse, Complement and ReverseComplement sequence");
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Solexa FastQFormat and specified score.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat.
        /// and Score "104" 
        /// Output Data : Validation of Created Qualitative sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
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
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateIlluminaFormatTypeDnaQualitativeSequenceWithScore()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaIlluminaNode,
                QualitativeSequenceParameters.Score);
        }

        /// <summary>
        /// Validate creation of Qualitative Sequence for Dna Sequence
        /// with Solexa FastQFormat and Byte array.
        /// Input Data : Dna Alphabet,Dna Sequence,"Solexa" FastQFormat.
        /// Output Data : Validation of Created Qualitative sequence with score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
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
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
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
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSangerFormatTypeDnaQualitativeSequenceWithByteArray()
        {
            GeneralQualitativeSequence(Constants.SimpleDnaSangerNode,
                QualitativeSequenceParameters.ByteArray);
        }

        /// <summary>
        /// Validate clear Qualitative Sequence
        /// Input Data : Dna Sequence.
        /// Output Data: Qualitative sequence having sequence data.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateQualititaiveSequenceConstructor()
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(
            Constants.SimpleDnaSolexaNode, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
            utilityObj.xmlUtil.GetTextValue(Constants.SimpleDnaSolexaNode,
            Constants.FastQFormatType));
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = utilityObj.xmlUtil.GetTextValue(
            Constants.SimpleDnaSolexaNode, Constants.inputSequenceNode);

            byte[] qualScores = new byte[inputSequence.Count()];

            for (int i = 0; i < inputSequence.Count(); i++)
            {
                qualScores[i] = (byte)'{';
            }

            // Create a Qualitative Sequence.
            createdQualitativeSequence = new QualitativeSequence(
                    alphabet, expectedFormatType, encodingObj.GetBytes(inputSequence), qualScores);

            string qualSequence = new string(createdQualitativeSequence.Select(a => (char)a).ToArray());

            // Validate Qualitative Sequence after addition of Seq Item.
            Assert.IsTrue(!string.IsNullOrEmpty(qualSequence));
            Assert.AreEqual(inputSequence, qualSequence);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence BVT: Qualitative Sequence {0} is as expected.",
                qualSequence));
        }

        /// <summary>
        /// Validate convert from Sanger to solexa and Illumina.
        /// Input Data : Sanger quality value, Sanger format sequence.
        /// Output Data :Validate convert from Sanger to Illumina and Solexa.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "sangerSequence"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "expectedSolexaSequence"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "expectedIlluminaSequence"), TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ConvertSangerToSolexaAndIllumina()
        {
            // Gets the actual sequence and the Qual score from the Xml
            string sangerSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.SangerSequence);
            string expectedSolexaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.SolexaSequence);
            string expectedIlluminaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.IlluminaSequence);
            string sangerQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.SangerQualScore);
            string expectedSolexaQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.SolexaQualScore);
            string expectedIlluminaQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.SangerToSolexaAndIlluminaNode, Constants.IlluminaQualScore);

            //string solexaQualScore = null;
            //string illuminaQualScore = null;
            byte[] scoreValue = ASCIIEncoding.ASCII.GetBytes(sangerQualScore);

            // Create a Sanger qualitative sequence.
            //QualitativeSequence sangerQualSequence = new QualitativeSequence(
            //    Alphabets.DNA, FastQFormatType.Sanger, sangerSequence, scoreValue);

            //QualitativeSequence sangerQualSequence = new QualitativeSequence(*********************
            //    Alphabets.DNA, FastQFormatType.Sanger, sangerSequence, sangerQualScore);


            // Convert Sanger to Solexa.
            //QualitativeSequence solexaQualSequence = sangerQualSequence.ConvertTo(
            //    FastQFormatType.Solexa);

            byte[] sangerToSolexa = QualitativeSequence.ConvertFromSangerToSolexa(scoreValue);


            //solexaQualScore = ASCIIEncoding.ASCII.GetString(solexaQualSequence.Scores);

            // Validate converted solexa score.

            string qualSequence = new string(encodingObj.GetChars(sangerToSolexa));

            Assert.AreEqual(expectedSolexaQualScore, qualSequence);


            //Assert.AreEqual(solexaQualScore, expectedSolexaQualScore);

            //Assert.AreEqual(solexaQualSequence.ToString(), expectedSolexaSequence);




            //ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
            //    "Qualitative Sequence BVT:Qualitative Solexa score type {0} is as expected.",
            //    solexaQualScore));
            //Console.WriteLine(string.Format((IFormatProvider)null,
            //    "Qualitative Sequence BVT:Qualitative Solexa score type {0} is as expected.",
            //solexaQualSequence.ToString()));

            // Convert Sanger to Illumina.

            //QualitativeSequence illuminaQualSequence = sangerQualSequence.ConvertTo(
            //    FastQFormatType.Illumina);
            //illuminaQualScore = ASCIIEncoding.ASCII.GetString(illuminaQualSequence.Scores);

            byte[] sangerToIllumina = QualitativeSequence.ConvertFromSangerToIllumina(scoreValue);

            //// Validate converted illumina score.
            //Assert.AreEqual(illuminaQualScore, expectedIlluminaQualScore);
            //Assert.AreEqual(illuminaQualSequence.ToString(), expectedIlluminaSequence);

            string sangerToIlluminaString = new string(encodingObj.GetChars(sangerToIllumina));

            Assert.AreEqual(expectedIlluminaQualScore, sangerToIlluminaString);

            //ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
            //    "Qualitative Sequence BVT:Qualitative Illumina score type {0} is as expected.",
            //    illuminaQualScore));
            //Console.WriteLine(string.Format((IFormatProvider)null,
            //    "Qualitative Sequence BVT:Qualitative Illumina score type {0} is as expected.",
            //    illuminaQualSequence.ToString()));



        }

        /// <summary>
        /// Validate convert from Solexa to Sanger and Illumina.
        /// Input Data : Solexa quality value, Solexa format sequence.
        /// Output Data : Validate convert from Solexa to Sanger and Illumina.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "expectedSangerSequence"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "solexaSequenceinBytes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "expectedIlluminaSequence"), TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ConvertSolexaToSangerAndIllumina()
        {
            // Gets the actual sequence and the Qual score from the Xml
            string solexaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.SolexaSequence);
            string expectedSangerSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.SangerSequence);
            string expectedIlluminaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.IlluminaSequence);
            string solexaQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.SolexaQualScore);
            string expectedSangerQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.SangerQualScore);
            string expectedIlluminaQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.SolexaToSangerAndIlluminaNode, Constants.IlluminaQualScore);
            byte[] byteValue = ASCIIEncoding.ASCII.GetBytes(solexaQualScore);

            //string sangerQualScore = null;
            //string illuminaQualScore = null;

            //byte[] qualScores = new byte[solexaSequence.Count()];

            //for (int i = 0; i < solexaSequence.Count(); i++)
            //{
            //    qualScores[i] = (byte)'{';
            //}

            Byte[] solexaSequenceinBytes = encodingObj.GetBytes(solexaSequence);

            // Create a Solexa qualitative sequence.
            //QualitativeSequence solexaQualSequence = new QualitativeSequence(Alphabets.DNA,
            //    FastQFormatType.Solexa, solexaSequence, byteValue);

            //QualitativeSequence solexaQualSequence = new QualitativeSequence(Alphabets.DNA,********************
            //    FastQFormatType.Solexa, solexaSequenceinBytes, byteValue);

            //// Convert Solexa to Sanger.
            //QualitativeSequence sangerQualSequence = solexaQualSequence.ConvertTo(
            //    FastQFormatType.Sanger);
            //sangerQualScore = ASCIIEncoding.ASCII.GetString(sangerQualSequence.Scores);

            byte[] sangerQualScore = QualitativeSequence.ConvertFromSolexaToSanger(byteValue);
            // Validate converted sanger score.

            string qualSequence = new string(encodingObj.GetChars(sangerQualScore));

            Assert.AreEqual(expectedSangerQualScore, qualSequence);

            //Assert.AreEqual(sangerQualSequence.ToString(), expectedSangerSequence);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence BVT:Qualitative Sanger score type {0} is as expected.",
                sangerQualScore));
            //Console.WriteLine(string.Format((IFormatProvider)null,
            //    "Qualitative Sequence BVT:Qualitative Sanger score type {0} is as expected.",
            //    sangerQualSequence.ToString()));

            // Convert Solexa to Illumina.
            //QualitativeSequence illuminaQualSequence =
            //    solexaQualSequence.ConvertTo(FastQFormatType.Illumina);
            //illuminaQualScore = ASCIIEncoding.ASCII.GetString(illuminaQualSequence.Scores);

            byte[] illuminaQualScore = QualitativeSequence.ConvertFromSolexaToIllumina(byteValue);
            string qualSequenceIllumina = new string(encodingObj.GetChars(illuminaQualScore));
            Assert.AreEqual(expectedIlluminaQualScore, qualSequenceIllumina);

            // Validate converted illumina score.
            //Assert.AreEqual(illuminaQualScore, expectedIlluminaQualScore);
            //Assert.AreEqual(illuminaQualSequence.ToString(), expectedIlluminaSequence);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence BVT:Qualitative Illumina score type {0} is as expected.",
                illuminaQualScore));
            //Console.WriteLine(string.Format((IFormatProvider)null,
            //    "Qualitative Sequence BVT:Qualitative Illumina score type {0} is as expected.",
            //    illuminaQualSequence.ToString()));
        }

        /// <summary>
        /// Validate convert from Illumina to Sanger and Solexa.
        /// Input Data : Illumina quality value, Illumina format sequence.
        /// Output Data : Validate convert from Illumina to Sanger and Solexa.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "illuminaSequenceinBytes"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "expectedSolexaSequence"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "expectedSangerSequence"), TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ConvertIlluminaToSangerAndSolexa()
        {
            // Gets the actual sequence and the Qual score from the Xml
            string illuminaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.IlluminaSequence);
            string expectedSangerSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.SangerSequence);
            string expectedSolexaSequence = utilityObj.xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.SolexaSequence);
            string illuminaQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.IlluminaQualScore);
            string expectedSangerQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.SangerQualScore);
            string expectedSolexaQualScore = utilityObj.xmlUtil.GetTextValue(
                Constants.IlluminaToSangerAndSolexaNode, Constants.SolexaQualScore);


            byte[] illuminaSequenceinBytes = encodingObj.GetBytes(illuminaSequence);
            byte[] illuminaQualScoreinBytes = encodingObj.GetBytes(illuminaQualScore);
            byte[] sangerQualScore = QualitativeSequence.ConvertFromIlluminaToSanger(illuminaQualScoreinBytes);

            string qualSequenceSanger = new string(encodingObj.GetChars(sangerQualScore));
            // Validate converted sanger score.
            Assert.AreEqual(expectedSangerQualScore, qualSequenceSanger);

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence BVT:Qualitative Sanger score type {0} is as expected.",
                sangerQualScore));
            byte[] solexaQualScore = QualitativeSequence.ConvertFromIlluminaToSolexa(illuminaQualScoreinBytes);

            string qualSequenceSolexa = new string(encodingObj.GetChars(solexaQualScore));

            // Validate converted illumina score.
            Assert.AreEqual(expectedSolexaQualScore, qualSequenceSolexa);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Qualitative Sequence BVT:Qualitative Solexa format type {0} is as expected.",
                illuminaQualScore));
        }

        #endregion QualitativeSequence Bvt TestCases

        #region Supporting Methods

        /// <summary>
        /// General method to validate creation of Qualitative sequence.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="parameters">Different Qualitative Sequence parameters.</param>
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "inputScore"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "expectedOuptutScore"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "expectedMaxScore"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "byteArray")]
        void GeneralQualitativeSequence(
            string nodeName, QualitativeSequenceParameters parameters)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            string expectedScore = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedScore);
            QualitativeSequence createdQualitativeSequence = null;
            string inputSequence = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.inputSequenceNode);
            string expectedSequence = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceCount = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.QSequenceCount);
            string expectedMaxScore = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.MaxScoreNode);
            string inputScore = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.InputScoreNode);
            string expectedOuptutScore = utilityObj.xmlUtil.GetTextValue(
                nodeName, Constants.InputScoreNode);
            string inputQuality = utilityObj.xmlUtil.GetTextValue(
            nodeName, Constants.InputByteArrayNode);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(inputQuality);
            Byte[] inputScoreArray = encodingObj.GetBytes(inputQuality);


            // Create and validate Qualitative Sequence.
            switch (parameters)
            {

                case QualitativeSequenceParameters.Score:
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                    inputSequence, inputQuality);
                    int count = 0;
                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.QualityScores)
                    {
                        Assert.AreEqual(qualScore, inputScoreArray[count]);
                        count++;
                    }
                    break;
                case QualitativeSequenceParameters.ByteArray:
                    byte[] scoreValue = ASCIIEncoding.ASCII.GetBytes(inputSequence);
                    int index = 0;
                    createdQualitativeSequence = new QualitativeSequence(alphabet, expectedFormatType,
                    scoreValue, inputScoreArray);

                    // Validate score
                    foreach (byte qualScore in createdQualitativeSequence.QualityScores)
                    {
                        Assert.AreEqual(qualScore, inputScoreArray[index]);
                        index++;
                    }
                    break;
                default:
                    break;
            }

            string qualitativeSequence = new string(createdQualitativeSequence.Select(a => (char)a).ToArray());
            // Validate createdSequence qualitative sequence.
            Assert.IsNotNull(createdQualitativeSequence);
            Assert.AreEqual(alphabet, createdQualitativeSequence.Alphabet);
            Assert.AreEqual(expectedSequence, qualitativeSequence);
            Assert.AreEqual(expectedSequenceCount, createdQualitativeSequence.Count.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedScore, createdQualitativeSequence.QualityScores.Count().ToString((IFormatProvider)null));
            Assert.AreEqual(expectedFormatType, createdQualitativeSequence.FormatType);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format((IFormatProvider)null,
            "Qualitative Sequence BVT:Qualitative Sequence {0} is as expected.",
            qualitativeSequence));

            Console.WriteLine(string.Format((IFormatProvider)null,
            "Qualitative Sequence BVT:Qualitative Sequence Score {0} is as expected.",
             createdQualitativeSequence.Count().ToString((IFormatProvider)null)));

            Console.WriteLine(string.Format((IFormatProvider)null,
            "Qualitative Sequence BVT:Qualitative format type {0} is as expected.",
            createdQualitativeSequence.FormatType));
        }

        #endregion Supporting Methods
    }
}
