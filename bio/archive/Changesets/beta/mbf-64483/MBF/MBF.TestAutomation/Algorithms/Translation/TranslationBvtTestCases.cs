// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * TranslationBvtTestCases.cs
 * 
 * This file contains the Translation BVT Test Cases which includes Codons, 
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
    /// Test Automation code for MBF Translation and BVT level validations.
    /// </summary>
    [TestClass]
    public class TranslationBvtTestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static TranslationBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Translation Bvt TestCases

        /// <summary>
        /// Validate an aminoacod for a given nucleaotide triplet.
        /// Input Data : Valid triplet of nucleotide - 'UUU'.
        /// Output Data : Corresponding amino acid 'Phenylalanine'.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAminoAcid()
        {
            // Get Node values from XML.
            string expectedNucleo = _utilityObj._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.Nucleotide);
            string expectedAminoAcid = _utilityObj._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.AminoAcid);

            // Create Nucleotide objects.
            Nucleotide firstNucleo1 = new Nucleotide(expectedNucleo[0], "Uracil");
            Nucleotide firstNucleo2 = new Nucleotide(expectedNucleo[1], "Uracil");
            Nucleotide firstNucleo3 = new Nucleotide(expectedNucleo[2], "Uracil");

            // Validate Codons lookup method.
            AminoAcid aminoAcid = Codons.Lookup(firstNucleo1, firstNucleo2, firstNucleo3);

            // Validate amino acids for each triplet.
            Assert.AreEqual(aminoAcid.Name.ToString((IFormatProvider)null), expectedAminoAcid);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Translation BVT: Amino Acid {0} is expected.", aminoAcid));
            ApplicationLog.WriteLine(
                "Translation BVT: Amino Acid validation for a given triplets of nucleotide was completed successfully.");

        }

        /// <summary>
        /// Validate an aminoacod for a given valid Sequence.
        /// Input Data : Valid Sequence - 'GAUUCAAGGGCU'
        /// Output Data : Corresponding amino acid 'Serine'.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAminoAcidForSequence()
        {
            // Get Node values from XML.
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(Constants.SimpleRnaAlphabetNode,
                Constants.AlphabetNameNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.ExpectedNormalString);
            string expectedAminoAcid = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.SeqAminoAcid);
            string expectedOffset = _utilityObj._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.OffsetVaule1);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Validate Codons lookup method.
            AminoAcid aminoAcid = Codons.Lookup(seq, Convert.ToInt32(expectedOffset, null));

            // Validate amino acids for each triplet.
            Assert.AreEqual(aminoAcid.Name.ToString((IFormatProvider)null), expectedAminoAcid);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Translation BVT: Amino Acid {0} is expected.", aminoAcid));
            ApplicationLog.WriteLine(
                "Translation BVT: Amino Acid validation for a given sequence was completed successfully.");
        }

        /// <summary>
        /// Validate an Protein translation for a given sequence.
        /// Input Data : Valid Sequence - 'AUUG'
        /// Output Data : Corresponding amino acid 'I'.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateProteinTranslation()
        {
            // Get Node values from XML.
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.ExpectedSequence);
            string expectedAminoAcid = _utilityObj._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.AminoAcid);

            ISequence proteinTranslation = new Sequence(Alphabets.RNA, expectedSeq);
            ISequence protein = ProteinTranslation.Translate(proteinTranslation);

            // Validate Protein Translation.
            Assert.AreEqual(protein.Alphabet, Alphabets.Protein);
            Assert.AreEqual(protein.ToString(), expectedAminoAcid);

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Translation BVT: Amino Acid {0} is expected.", protein));
            ApplicationLog.WriteLine(
                "Translation BVT: Amino Acid validation for a given sequence was completed successfully.");
        }

        /// <summary>
        /// Validate an Protein translation for a given sequence by passing offset value.
        /// Input Data : Valid Sequence - 'AUUG'
        /// Output Data : Corresponding amino acid 'I'.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateProteinTranslationWithOffset()
        {
            // Get Node values from XML.
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.ExpectedSequence);
            string expectedAminoAcid = _utilityObj._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.AminoAcid);

            ISequence proteinTranslation = new Sequence(Alphabets.RNA, expectedSeq);
            ISequence protein = ProteinTranslation.Translate(proteinTranslation, 0);

            // Validate Protein Translation.
            Assert.AreEqual(protein.Alphabet, Alphabets.Protein);
            Assert.AreEqual(protein.ToString(), expectedAminoAcid);

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Translation BVT: Amino Acid {0} is expected.", protein));
            ApplicationLog.WriteLine(
                "Translation BVT: Amino Acid validation for a given sequence was completed successfully.");
        }

        /// <summary>
        /// Validate Complement of DNA Sequence.
        /// Input Data : Valid Sequence - 'AGGTCCGATA'
        /// Output Data : Complement of DNA - 'TCCATGGGCTAT'
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateDnaComplementation()
        {
            // Get Node values from XML.
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSequence);
            string expectedComplement = _utilityObj._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaComplement);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Complement DNA Sequence.
            ISequence complement = Complementation.Complement(seq);

            // Validate Complement.
            Assert.AreEqual(complement.ToString(), expectedComplement);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Translation BVT: Complement {0} is expected.", seq));
            ApplicationLog.WriteLine(
                "Translation BVT: Complement of DNA sequence was validate successfully.");
        }

        /// <summary>
        /// Validate Reverse Complement of DNA Sequence.
        /// Input Data : Valid Sequence - 'AGGTCCGATA'
        /// Output Data : Reverse Complement of DNA - 'TATCGGGTACCT'
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateDnaRevComplementation()
        {
            // Get Node values from XML.
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSequence);
            string expectedRevComplement = _utilityObj._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaRevComplement);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Reverse Complement of DNA Sequence.
            ISequence revComplement = Complementation.ReverseComplement(seq);

            // Validate Reverse Complement.
            Assert.AreEqual(revComplement.ToString(), expectedRevComplement);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Translation BVT: Reverse Complement {0} is expected.", seq));
            ApplicationLog.WriteLine(
                "Translation BVT: Reverse Complement of DNA sequence was validate successfully.");
        }

        /// <summary>
        /// Validate Transcribe of DNA Sequence.
        /// Input Data : Valid Sequence - 'ATGGCG'
        /// Output Data : Transcription - 'UACCGC'
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateTranscribe()
        {
            // Get Node values from XML.
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.DnaSequence);
            string expectedTranscribe = _utilityObj._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.TranscribeNode);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Transcription of DNA Sequence.
            ISequence transcribe = Transcription.Transcribe(seq);

            // Validate Transcription.
            Assert.AreEqual(transcribe.ToString(), expectedTranscribe);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Translation BVT: Transcription {0} is expected.", seq));
            ApplicationLog.WriteLine(
                "Translation BVT: Transcription of DNA sequence was validate successfully.");
        }

        /// <summary>
        /// Validate Reverse Transcribe of RNA Sequence.
        /// Input Data : Valid Sequence - 'UACCGC'
        /// Output Data : Reverse Transcription - 'ATGGCG'
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateRevTranscribe()
        {
            // Get Node values from XML.
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.RnaSequence);
            string expectedRevTranscribe = _utilityObj._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.RevTranscribe);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Reverse Transcription of RNA Sequence.
            ISequence revTranscribe = Transcription.ReverseTranscribe(seq);

            // Validate Reverse Transcription.
            Assert.AreEqual(revTranscribe.ToString(), expectedRevTranscribe);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Translation BVT: Reverse Transcription {0} is expected.", seq));
            ApplicationLog.WriteLine(
                "Translation BVT: Reverse Transcription of DNA sequence was validate successfully.");
        }

        #endregion Translation Bvt TestCases
    }
}
