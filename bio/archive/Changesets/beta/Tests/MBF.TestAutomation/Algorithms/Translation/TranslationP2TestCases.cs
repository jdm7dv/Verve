// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * TranslationP2TestCases.cs
 * 
 * This file contains the Translation P2 Test Cases which includes Codons, 
 * Complementation, ProteinTranslation and Transcription.
 * 
******************************************************************************/

using System;

using MBF.Algorithms.Translation;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.Algorithms.Translation
{
    /// <summary>
    /// Test Automation code for MBF Translation and P2 level validations.
    /// </summary>
    [TestClass]
    public class TranslationP2TestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static TranslationP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region  Codons P2 TestCases

        /// <summary>
        /// Validate an aminoacod for a given RNA Sequence with More than 12 characters..
        /// Input Data :Sequence with more than 12 characters - 'AAAGGGAUGCCUGUUUGA'.
        /// Output Data : Corresponding amino acid 'Arginine'.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateLookupWithMoreThanTwelveChars()
        {
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(Constants.SimpleRnaAlphabetNode,
                Constants.AlphabetNameNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.SequenceWithmoreThanTweleveChars);
            string expectedAminoAcid = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.OffsetZeroSixCharsAminoAcid);
            string expectedOffset = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.OffsetVaule2);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Validate Codons lookup method.
            AminoAcid aminoAcid = Codons.Lookup(seq, Convert.ToInt32(expectedOffset, null));

            // Validate amino acids for a given sequence.
            Assert.AreEqual(aminoAcid.Name.ToString((IFormatProvider)null), expectedAminoAcid);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Translation P2: Amino Acid {0} is expected.", aminoAcid));
            ApplicationLog.WriteLine(
                "Translation P2: Amino Acid validation for a given sequence was completed successfully.");
        }

        /// <summary>
        /// Validate an aminoacod for a given RNA Sequence with More than 12 characters and offset value "1"..
        /// Input Data :Sequence with more than 12 characters - 'AAAGGGAUGCCUGUUUGA'.
        /// Output Data : Corresponding amino acid 'Isoleucine'.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateLookupWithZeroOffset()
        {
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(Constants.SimpleRnaAlphabetNode,
                Constants.AlphabetNameNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.SequenceWithmoreThanTweleveChars);
            string expectedAminoAcid = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.OffsetOneMoreThanTwelveCharsAminoAcid);
            string expectedOffset = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.OffsetVaule4);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Validate Codons lookup method.
            AminoAcid aminoAcid = Codons.Lookup(seq, Convert.ToInt32(expectedOffset, null));

            // Validate amino acids for a given sequence.
            Assert.AreEqual(aminoAcid.Name.ToString((IFormatProvider)null), expectedAminoAcid);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Translation P2: Amino Acid {0} is expected.", aminoAcid));
            ApplicationLog.WriteLine(
                "Translation P2: Amino Acid validation for a given sequence was completed successfully.");
        }

        /// <summary>
        /// Validate an aminoacod for a given RNA Sequence with 12 characters and offset value "1"..
        /// Input Data :Sequence with 12 characters - 'AAAGGGAUGCCU'.
        /// Output Data : Corresponding amino acid 'Isoleucine'.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateLookupWithOneOffset()
        {
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(Constants.SimpleRnaAlphabetNode,
                Constants.AlphabetNameNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.SequenceWithmoreThanTweleveChars);
            string expectedAminoAcid = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.OffsetOneMoreThanTwelveCharsAminoAcid);
            string expectedOffset = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.OffsetVaule4);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Validate Codons lookup method.
            AminoAcid aminoAcid = Codons.Lookup(seq, Convert.ToInt32(expectedOffset, null));

            // Validate amino acids for a given sequence.
            Assert.AreEqual(aminoAcid.Name.ToString((IFormatProvider)null), expectedAminoAcid);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Translation P2: Amino Acid {0} is expected.", aminoAcid));
            ApplicationLog.WriteLine(
                "Translation P2: Amino Acid validation for a given sequence was completed successfully.");
        }

        /// <summary>
        /// Validate an aminoacod for a given DNA Sequence with offset value "1".
        /// Input Data : Valid Sequence - 'ATGGCG'.
        /// Output Data : Corresponding amino acid 'Phenylalanine'.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateLookupWithDnaSeqAndOffsetValue()
        {
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(Constants.TranscribeNode,
                Constants.DnaSequence);
            string expectedAminoAcid = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.DnaSeqAminoAcidWithOffsetValueOne);
            string expectedOffset = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.OffsetVaule4);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Transcribe DNA to RNA.
            ISequence transcribe = Transcription.Transcribe(seq);

            // Validate Codons lookup method.
            AminoAcid aminoAcid = Codons.Lookup(transcribe, Convert.ToInt32(expectedOffset, null));

            // Validate amino acids for a given sequence.
            Assert.AreEqual(aminoAcid.Name.ToString((IFormatProvider)null), expectedAminoAcid);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Translation P2: Amino Acid {0} is expected.", aminoAcid));
            ApplicationLog.WriteLine(
                "Translation P2: Amino Acid validation for a given sequence was completed successfully.");
        }

        #endregion Codons P2 TestCases

        #region  Complement P2 TestCases

        /// <summary>
        /// Validate Complement by passing null value.
        /// Input Data : Invalid Sequence - 'null'
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateComplementForNull()
        {
            bool Exthrown = false;

            // Complement null Sequence.
            try
            {
                Complementation.Complement(null);
            }
            catch (NullReferenceException)
            {
                Exthrown = true;
            }
            // Validate if Complement method is throwing an exception.
            Assert.IsTrue(Exthrown);
            ApplicationLog.WriteLine(
                "Translation P2: Complement method was throwing an expection for null value.");
        }
        /// <summary>
        /// Validate Reverse Complement by passing null value.
        /// Input Data : Invalid Sequence - 'null'
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateRevComplementForNull()
        {
            bool Exthrown = false;

            // Reverse Complement null Sequence.
            try
            {
                Complementation.ReverseComplement(null);
            }
            catch (NullReferenceException)
            {
                Exthrown = true;
            }
            // Validate if Reverse Complement method is throwing an exception.
            Assert.IsTrue(Exthrown);
            ApplicationLog.WriteLine(
                "Translation P2: Reverse complement method was throwing an expection for null value.");
        }

        #endregion Complement P2 TestCases

        #region  Transcribe P2 TestCases

        /// <summary>
        /// Validate Reverse Transcribe by passing null value.
        /// Input Data : Invalid Sequence - 'null'
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateRevTranscribeForNull()
        {
            bool Exthrown = false;

            // Reverse Transcribe null Sequence.
            try
            {
                Transcription.ReverseTranscribe(null);
            }
            catch (NullReferenceException)
            {
                Exthrown = true;
            }
            // Validate if Reverse Transcribe method is throwing an exception.
            Assert.IsTrue(Exthrown);
            ApplicationLog.WriteLine(
                "Translation P2: Reverse Transcribe method was throwing an expection for null value.");
        }

        /// <summary>
        /// Validate Translation method by passing null value.
        /// Input Data : Invalid Sequence - 'null'
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateTranslationForNull()
        {
            bool Exthrown = false;

            // Translation by passing null Sequence.
            try
            {
                ProteinTranslation.Translate(null);
            }
            catch (NullReferenceException)
            {
                Exthrown = true;
            }
            // Validate if Translate method is throwing an exception.
            Assert.IsTrue(Exthrown);
            ApplicationLog.WriteLine(
                "Translation P2: Translate method was throwing an expection for null value.");
        }

        /// <summary>
        /// Validate Translation method by passing invalid sequence and valid offset value.
        /// Input Data : Sequence - 'Null' and offset "10".
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateTranslationForInvalidOffset()
        {
            // Get Node values from XML.
            bool Exthrown = false;

            // Translation by passing null Sequence.
            try
            {
                ProteinTranslation.Translate(null, 10);
            }
            catch (NullReferenceException)
            {
                Exthrown = true;
            }
            // Validate if Translate method is throwing an exception.
            Assert.IsTrue(Exthrown);
            ApplicationLog.WriteLine(
                "Translation P2: Translate method was throwing an expection for null value.");
        }


        /// <summary>
        /// Validate Translation method by passing Negative offset value.
        /// Input Data : Sequence - 'UACCGC' and offset "-10".
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateTranslationForNegativeOffset()
        {
            // Get Node values from XML.
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(Constants.TranslationNode,
                Constants.RnaSequence);
            bool Exthrown = false;

            // Translate Six characters RNA to protein.
            ISequence proteinTranslation = new Sequence(Alphabets.RNA, expectedSeq);

            // Call a translate method by passing negative offset value.
            try
            {
                ProteinTranslation.Translate(proteinTranslation, -10);
            }
            catch (ArgumentOutOfRangeException)
            {
                Exthrown = true;
            }
            // Validate if Translate method is throwing an exception.
            Assert.IsTrue(Exthrown);
            ApplicationLog.WriteLine(
                "Translation P2: Translate method was throwing an expection for null value.");
        }

        #endregion Transcribe P2 TestCases
    }
}