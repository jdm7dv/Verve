// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * TranslationP1TestCases.cs
 * 
 * This file contains the Translation P1 Test Cases which includes Codons, 
 * Complementation, ProteinTranslation and Transcription.
 * 
******************************************************************************/

using System;

using MBF.Algorithms.Translation;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.Algorithms.Translation
{

    /// <summary>
    /// Test Automation code for MBF Translation and P1 level validations.
    /// </summary>
    [TestFixture]
    public class TranslationP1TestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static TranslationP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region  Codons P1 TestCases

        /// <summary>
        /// Validate a aminoacod for a given RNA Sequence with 12 characters.
        /// Input Data :Sequence - 'AUGCCUGUUUGA'.
        /// Output Data : Corresponding amino acid 'Aspartic Acid'.
        /// </summary>
        [Test]
        public void ValidateLookupWithRnaSequence()
        {
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.ExpectedNormalString);
            string expectedAminoAcid = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.OffsetZeroTwelveCharsAminoAcid);
            string expectedOffset = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.OffsetVaule);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Validate Codons lookup method.
            AminoAcid aminoAcid = Codons.Lookup(seq, Convert.ToInt32(expectedOffset, null));

            // Validate amino acids for a given sequence.
            Assert.AreEqual(aminoAcid.Name.ToString(), expectedAminoAcid);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Amino Acid {0} is expected.", aminoAcid));
            ApplicationLog.WriteLine(
                "Translation P1: Amino Acid validation for a given sequence was completed successfully.");
        }

        /// <summary>
        /// Validate a aminoacod for a given RNA Sequence with offset value "6".
        /// Input Data : Valid Sequence - 'AUGCCUGUUUGA'.
        /// Output Data : Corresponding amino acid 'Arginine'.
        /// </summary>
        [Test]
        public void ValidateLookupWithOffsetValueSix()
        {
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.ExpectedNormalString);
            string expectedAminoAcid = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.OffsetZeroSixCharsAminoAcid);
            string expectedOffset = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.OffsetVaule2);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Validate Codons lookup method.
            AminoAcid aminoAcid = Codons.Lookup(seq, Convert.ToInt32(expectedOffset, null));

            // Validate amino acids for a given sequence.
            Assert.AreEqual(aminoAcid.Name.ToString(), expectedAminoAcid);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Amino Acid {0} is expected.", aminoAcid));
            ApplicationLog.WriteLine(
                "Translation P1: Amino Acid validation for a given sequence was completed successfully.");
        }

        /// <summary>
        /// Validate a aminoacod for a given RNA Sequence with Six Characters and offset value "6".
        /// Input Data : Valid Sequence - 'GUUUGA'.
        /// Output Data : Corresponding amino acid 'Arginine'.
        /// </summary>
        [Test]
        public void ValidateLookupWithSixChars()
        {
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.SequenceWithSixChars);
            string expectedAminoAcid = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.OffsetZeroSixCharsAminoAcid);
            string expectedOffset = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.OffsetVaule2);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Validate Codons lookup method.
            AminoAcid aminoAcid = Codons.Lookup(seq, Convert.ToInt32(expectedOffset, null));

            // Validate amino acids for a given sequencet.
            Assert.AreEqual(aminoAcid.Name.ToString(), expectedAminoAcid);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Amino Acid {0} is expected.", aminoAcid));
            ApplicationLog.WriteLine(
                "Translation P1: Amino Acid validation for a given sequence was completed successfully.");
        }

        /// <summary>
        /// Validate a aminoacod for a given DNA Sequence with offset value "0".
        /// Input Data : Valid Sequence - 'ATGGCG'.
        /// Output Data : Corresponding amino acid 'Tyrosine'.
        /// </summary>
        [Test]
        public void ValidateLookupWithDnaSequence()
        {
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.DnaSequence);
            string expectedAminoAcid = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.DnaSeqAminoAcid);
            string expectedOffset = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.OffsetVaule);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Transcribe DNA to RNA.
            ISequence transcribe = Transcription.Transcribe(seq);

            // Validate Codons lookup method.
            AminoAcid aminoAcid = Codons.Lookup(transcribe, Convert.ToInt32(expectedOffset, null));

            // Validate amino acids for a given sequence.
            Assert.AreEqual(aminoAcid.Name.ToString(), expectedAminoAcid);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Amino Acid {0} is expected.", aminoAcid));
            ApplicationLog.WriteLine(
                "Translation P1: Amino Acid validation for a given sequence was completed successfully.");
        }

        /// <summary>
        /// Validates a aminoacid for a given 3 characters RNA Sequence with offset value "0".
        /// Input Data : Valid Sequence - 'AUG'.
        /// Output Data : Corresponding amino acid ''Methionine'.
        /// </summary>
        [Test]
        public void ValidateRnaCodonsTraslationWithOffset()
        {
            ValidateCodonsTranslationWithOffset(Constants.RnaSequenceWithThreeChars,
                Constants.OffsetVaule, Constants.AminoAcidForThreeChars);
        }

        /// <summary>
        /// Validates a aminoacid for a given 12 characters RNA Sequence with offset value "3".
        /// Input Data : Valid Sequence - 'AUGCGCCCGAUG'.
        /// Output Data : Corresponding amino acid 'Arginine'.
        /// </summary>
        [Test]
        public void ValidateRnaCodonsTraslationWithThreeOffset()
        {
            ValidateCodonsTranslationWithOffset(Constants.RnaSequenceWithTwelveChars,
                Constants.OffsetVaule1, Constants.AminoAcidForOffsetTwelve);
        }

        #endregion Codons P1 TestCases

        #region Complement P1 TestCases

        /// <summary>
        /// Validate Complement of Single DNA Synbol.
        /// Input Data : Valid DNA Symbol - 'A'
        /// Output Data : Complement of DNA Symbol - 'T'
        /// </summary>
        [Test]
        public void ValidateSingleDnaSymbolComplementation()
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSymbol);
            string expectedComplement = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSymbolComplement);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Complement DNA Sequence.
            ISequence complement = Complementation.Complement(seq);

            // Validate Single DNA Symbol Complement.
            Assert.AreEqual(complement.ToString(), expectedComplement);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Complement {0} is expected.", complement));
            ApplicationLog.WriteLine(
                "Translation P1: Complement of Single DNA Symbol was validate successfully.");
        }

        /// <summary>
        /// Validate Complement of DNA Sequence with more than twelve characters.
        /// Input Data : Valid DNA Symbol - 'ATATGTAGGTACCCGATA'
        /// Output Data : Complement of DNA Symbol - 'TATACATCCATGGGCTAT'
        /// </summary>
        [Test]
        public void ValidateMoreThanTwelveCharsDnaComplementation()
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSymbol);
            string expectedComplement = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSymbolComplement);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Complement DNA Sequence.
            ISequence complement = Complementation.Complement(seq);

            // Validate Complement for a given sequence.
            Assert.AreEqual(complement.ToString(), expectedComplement);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Complement {0} is expected.", complement));
            ApplicationLog.WriteLine(
                "Translation P1: Complement of More than twelve DNA characters sequence was validate successfully.");
        }

        /// <summary>
        /// Validate Reverse Complement of Single symbol DNA Synbol.
        /// Input Data : Valid DNA Symbol - 'A'
        /// Output Data : Reverse Complement of DNA Symbol - 'T'
        /// </summary>
        [Test]
        public void ValidateSingleSymbolDnaRevComplementation()
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSymbol);
            string expectedRevComplement = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSymbolRevComplement);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Reverse Complement DNA Sequence.
            ISequence revComplement = Complementation.ReverseComplement(seq);

            // Validate Single DNA Symbol Reverse Complement.
            Assert.AreEqual(revComplement.ToString(), expectedRevComplement);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Complement {0} is expected.", revComplement));
            ApplicationLog.WriteLine(
                "Translation P1: Reverse Complement of Single DNA Symbol was validate successfully.");
        }

        /// <summary>
        /// Validate Reverse Complement of DNA Sequence with more than twelve characters.
        /// Input Data : Valid DNA Sequence - 'ATATGTAGGTACCCGATA'
        /// Output Data : Reverse Complement of DNA Sequence - 'TATACATCCATGGGCTAT'
        /// </summary>
        [Test]
        public void ValidateMoreThanTwelveCharsDnaRevComplementation()
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSequenceWithMoreThanTwelveChars);
            string expectedComplement = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaRevComplementForMoreThanTwelveChars);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Reverse Complement DNA Sequence.
            ISequence revComplement = Complementation.ReverseComplement(seq);

            // Validate Reverse Complement. for a given sequence.
            Assert.AreEqual(revComplement.ToString(), expectedComplement);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Rev Complement {0} is expected.", revComplement));
            ApplicationLog.WriteLine(
                "Translation P1: Reverse Complement of More than twelve DNA characters sequence was validate successfully.");
        }

        /// <summary>
        /// Validates Complement of DNA Sequence with twelve characters.
        /// Input Data : Valid DNA Sequence - 'AGGTACCCGATA'
        /// Output Data : Complement of DNA Sequence - 'TCCATGGGCTAT'
        /// </summary>
        [Test]
        public void ValidateDnaComplementationWithTweleveChars()
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSequence);
            string expectedComplement = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaComplement);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Complement DNA Sequence.
            ISequence complement = Complementation.Complement(seq);

            // Validate Complement for a given sequence.
            Assert.AreEqual(complement.ToString(), expectedComplement);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Complement {0} is expected.", complement));
            ApplicationLog.WriteLine(
                "Translation P1: Complement of twelve DNA characters sequence validated successfully.");
        }

        /// <summary>
        /// Validates Reverse Complement of DNA Sequence with twelve characters.
        /// Input Data : Valid DNA Sequence - 'AGGTACCCGATA'
        /// Output Data : Reverse Complement of DNA Sequence - 'TATCGGGTACCT'
        /// </summary>
        [Test]
        public void ValidateDnaReverseComplementationWithTweleveChars()
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSequence);
            string expectedRevComplement = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaRevComplement);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Reverse Complement DNA Sequence.
            ISequence revComplement = Complementation.ReverseComplement(seq);

            // Validate Reverse Complement for a given sequence.
            Assert.AreEqual(revComplement.ToString(), expectedRevComplement);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Reverse Complement {0} is expected.", revComplement));
            ApplicationLog.WriteLine(
                "Translation P1: Reverse Complement of twelve DNA characters sequence validated successfully.");
        }

        #endregion Complement P1 TestCases

        #region Transcribe P1 TestCases

        /// <summary>
        /// Validate Single symbol transcribe.
        /// Input Data : Valid DNA Symbol - 'A'
        /// Output Data : Transcribe - 'T'
        /// </summary>
        [Test]
        public void ValidateSingleSymbolTranscribe()
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.DnaSymbol);
            string expectedTranscribe = Utility._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.DnaSymbolTranscribe);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Transcribe DNA Symbol.
            ISequence transcribe = Transcription.Transcribe(seq);

            // Validate Single DNA Symbol transcribe.
            Assert.AreEqual(transcribe.ToString(), expectedTranscribe);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Transcribe {0} is expected.", transcribe));
            ApplicationLog.WriteLine(
                "Translation P1: Transcribe of Single DNA Symbol was validate successfully.");
        }

        /// <summary>
        /// Validate DNA Sequence Transcribe with more than twelve characters.
        /// Input Data : Valid DNA Sequence - 'ATATGTAGGTACCCGATA'
        /// Output Data : Transcribe Seqeunce - 'UAUACAUCCAUGGGCUAU'
        /// </summary>
        [Test]
        public void ValidateMoreThanTwelveCharsDnaTranscribe()
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSequenceWithMoreThanTwelveChars);
            string expectedTranscribe = Utility._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.DnaTranscribeForMoreThanTwelveChars);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Transcribe DNA Symbol.
            ISequence transcribe = Transcription.Transcribe(seq);

            // Validate transcribe.for a given sequence.
            Assert.AreEqual(transcribe.ToString(), expectedTranscribe);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Transcribe {0} is expected.", transcribe));
            ApplicationLog.WriteLine(
                "Translation P1: Transcribe of Single DNA Symbol was validate successfully.");
        }

        /// <summary>
        /// Validate Reverse Transcribe for a single DNA Synbol.
        /// Input Data : Valid DNA Symbol - 'G'
        /// Output Data : Transcribe Symbol - 'G'
        /// </summary>
        [Test]
        public void ValidateSingleSymbolDnaReverseTranscribe()
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.DnaSymbol);
            string expectedTranscribe = Utility._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.DnaSymbolRevTranscribe);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Reverse Transcribe DNA Symbol.
            ISequence transcribe = Transcription.Transcribe(seq);
            ISequence revTranscribe = Transcription.ReverseTranscribe(transcribe);

            // Validate Single DNA Symbol Reverse transcribe.
            Assert.AreEqual(revTranscribe.ToString(), expectedTranscribe);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Reverse Transcribe {0} is expected.", revTranscribe));
            ApplicationLog.WriteLine(
                "Translation P1: Reverse Transcribe of Single DNA Symbol was validate successfully.");
        }

        /// <summary>
        /// Validate DNA Sequence Reverse Transcribe with more than twelve characters.
        /// Input Data : Valid DNA Sequence - 'ATATGTAGGTACCCGATA'
        /// Output Data : Reverse Transcribe Seqeunce - 'ATATGTAGGTACCCGATA'
        /// </summary>
        [Test]
        public void ValidateMoreThanTwelveCharsDnaReverseTranscribe()
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.ComplementNode, Constants.DnaSequenceWithMoreThanTwelveChars);
            string expectedTranscribe = Utility._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.DnaRevTranscribeForMoreThanTwelveChars);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Reverse Transcribe DNA Sequence.
            ISequence transcribe = Transcription.Transcribe(seq);
            ISequence revTranscribe = Transcription.ReverseTranscribe(transcribe);

            // Validate reverse transcribe.for a given sequence.
            Assert.AreEqual(revTranscribe.ToString(), expectedTranscribe);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Reverse Transcribe {0} is expected.", revTranscribe));
            ApplicationLog.WriteLine(
                "Translation P1: Reverse Transcribe of Single DNA Symbol was validate successfully.");
        }

        /// <summary>
        /// Validates DNA Sequence Transcribe with three characters.
        /// Input Data : Valid DNA Sequence - 'ACG'
        /// Output Data : Transcribe Seqeunce - 'UGC'
        /// </summary>
        [Test]
        public void ValidateDnaToRnaTranscribeWithThreeChars()
        {
            ValidateDnaToRnaTranscribe(Constants.DnaSequenceWithThreeChars,
                Constants.TranscribeForThreeChars);
        }

        /// <summary>
        /// Validates DNA Sequence Transcribe with twelve characters.
        /// Input Data : Valid DNA Sequence - 'ATATGTAGGTAC'
        /// Output Data : Transcribe Seqeunce - 'UAUACAUCCAUG'
        /// </summary>
        [Test]
        public void ValidateDnaToRnaTranscribeWithTwelveChars()
        {
            ValidateDnaToRnaTranscribe(Constants.DnaSequenceWithTweleveChars,
                Constants.TranscribeForTweleveChars);
        }

        /// <summary>
        /// Validates RNA Sequence reverse Transcribe with three characters.
        /// Input Data : Valid RNA Sequence - 'UGC'
        /// Output Data : Reverse Transcribe Seqeunce - 'ACG'
        /// </summary>
        [Test]
        public void ValidateRnaToDnaReverseTranscribeWithThreeChars()
        {
            ValidateRnaToDnaReverseTranscribe(Constants.TranscribeForThreeChars,
                Constants.DnaSequenceWithThreeChars);
        }

        /// <summary>
        /// Validates DNA Sequence Transcribe with twelve characters.
        /// Input Data : Valid RNA Sequence - 'UAUACAUCCAUG'
        /// Output Data : Reverse Transcribe Seqeunce - 'ATATGTAGGTAC'
        /// </summary>
        [Test]
        public void ValidateRnaToDnaReverseTranscribeWithTwelveChars()
        {
            ValidateRnaToDnaReverseTranscribe(Constants.TranscribeForTweleveChars,
                Constants.DnaSequenceWithTweleveChars);
        }

        #endregion Transcribe P1 TestCases

        #region Translation P1 TestCases

        /// <summary>
        /// Validate a Protein translation for a twelve characters sequence.
        /// Input Data : Seqeunce - 'CGCAUGCCGAUG'
        /// Output Data : "RMPM".
        /// </summary>
        [Test]
        public void ValidateTwelveCharsProteinTranslation()
        {
            // Get Node values from XML.
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.RnaSequenceWithTwelveChars);
            string expectedProtein = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.AminoAcidForTwelveChars);

            // Translate twelve characters RNA to protein.
            ISequence proteinTranslation = new Sequence(Alphabets.RNA, expectedSeq);
            ISequence protein = ProteinTranslation.Translate(proteinTranslation);

            // Validate Protein Translation.
            Assert.AreEqual(protein.ToString(), expectedProtein);
            ApplicationLog.WriteLine(
                "Translation P1: Protein translation validation is completed successfully.");
        }

        /// <summary>
        /// Validate a Protein translation for a given symbol.
        /// Input Data : Valid Symbol - 'A'
        /// Output Data : "Null"
        /// </summary>
        [Test]
        public void ValidateSingleSymbolTranslation()
        {
            // Get Node values from XML.
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.DnaSymbol);

            // Translate Single char RNA to protein.
            ISequence proteinTranslation = new Sequence(Alphabets.RNA, expectedSeq);
            ISequence protein = ProteinTranslation.Translate(proteinTranslation);

            // Validate Protein Translation.
            Assert.AreEqual(protein.ToString(), string.Empty);

            ApplicationLog.WriteLine(
                "Translation P1: Amino Acid validation for a given sequence was completed successfully.");
        }

        /// <summary>
        /// Validate a Protein translation for more than twelve characters sequence.
        /// Input Data : Sequence - 'AUGCGCCCGAUGCGC'
        /// Output Data : "Methionine,Arginine,Proline,Methionine,Arginine".
        /// </summary>
        [Test]
        public void ValidateMoreThanTwelveCharsProteinTranslation()
        {
            // Get Node values from XML.
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.RnaSequenceWithMoreThanTwelveChars);
            string expectedProtein = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.AminoAcidForMoreThanTwelveChars);

            // Translate more than twelve characters RNA to protein.
            ISequence proteinTranslation = new Sequence(Alphabets.RNA, expectedSeq);
            ISequence protein = ProteinTranslation.Translate(proteinTranslation);

            // Validate Protein Translation.
            Assert.AreEqual(protein.ToString(), expectedProtein);
            ApplicationLog.WriteLine(
                "Translation P1: Protein translation validation is completed successfully.");
        }

        /// <summary>
        /// Validate a Protein translation for more Six Chars with Offset value "0".
        /// Input Data : Sequence - 'UACCGC'
        /// Output Data : "Tyrosine,Arginine".
        /// </summary>
        [Test]
        public void ValidateSixCharsProteinTranslation()
        {
            // Get Node values from XML.
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.RnaSequence);
            string expectedProtein = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.AminoAcidForSixChars);
            string expectedOffset = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.OffsetVaule);

            // Translate Six characters RNA to protein.
            ISequence proteinTranslation = new Sequence(Alphabets.RNA, expectedSeq);
            ISequence protein = ProteinTranslation.Translate(proteinTranslation,
                Convert.ToInt32(expectedOffset, null));

            // Validate Protein Translation.
            Assert.AreEqual(protein.ToString(), expectedProtein);
            ApplicationLog.WriteLine(
                "Translation P1: Protein translation validation is completed successfully.");
        }

        /// <summary>
        /// Validate a Protein translation for a twelve characters sequence with offset value "3".
        /// Input Data : Sequence - 'CGCAUGCCGAUG'
        /// Output Data : "Arginine,Methionine,Proline,Methionine".
        /// </summary>
        [Test]
        public void ValidateTwelveCharsProteinTranslationWithOffset()
        {
            // Get Node values from XML.
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.RnaSequenceWithTwelveChars);
            string expectedProtein = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.AminoAcidForMoreOffsetThree);
            string expectedOffset = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.OffsetVaule1);

            // Translate twelve characters RNA to protein.
            ISequence proteinTranslation = new Sequence(Alphabets.RNA, expectedSeq);
            ISequence protein = ProteinTranslation.Translate(proteinTranslation,
                Convert.ToInt32(expectedOffset, null));

            // Validate Protein Translation.
            Assert.AreEqual(protein.ToString(), expectedProtein);
            ApplicationLog.WriteLine(
                "Translation P1: Protein translation validation is completed successfully.");
        }

        /// <summary>
        /// Validate Protein Translation for a given DNA.
        /// Input Data : Sequence - 'ATGGCG'
        /// Output Data : "Tyrosine,Arginine".
        /// </summary>
        [Test]
        public void ValidateDnaProteinTranslation()
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.TranscribeNode, Constants.DnaSequence);
            string expectedProtein = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.AminoAcidForSixChars);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Transcription of DNA Sequence.
            ISequence transcribe = Transcription.Transcribe(seq);

            // Protein Translation.
            ISequence proteinTranslation = new Sequence(Alphabets.RNA,
                transcribe.ToString());
            ISequence protein = ProteinTranslation.Translate(proteinTranslation);

            // Validate Protein Translation.
            Assert.AreEqual(protein.ToString(), expectedProtein);
            ApplicationLog.WriteLine(
                "Translation P1: Protein translation validation is completed successfully.");
        }

        /// <summary>
        /// Validate a Protein translation for a twelve characters sequence with offset value "6".
        /// Input Data : Sequence - 'CGCAUGCCGAUG'
        /// Output Data : "PM".
        /// </summary>
        [Test]
        public void ValidateTwelveCharsProteinTranslationWithOffsetSix()
        {
            // Get Node values from XML.
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.RnaSequenceWithTwelveChars);
            string expectedProtein = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.AminoAcidForOffsetSix);
            string expectedOffset = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.OffsetVaule2);

            // Translate twelve characters RNA to protein with  offset value "6"
            ISequence proteinTranslation = new Sequence(Alphabets.RNA, expectedSeq);
            ISequence protein = ProteinTranslation.Translate(proteinTranslation,
                Convert.ToInt32(expectedOffset, null));

            // Validate Protein Translation.
            Assert.AreEqual(protein.ToString(), expectedProtein);
            ApplicationLog.WriteLine(
                "Translation P1: Protein translation validation is completed successfully.");
        }

        /// <summary>
        /// Validate a Protein translation for a twelve characters sequence with offset value "12".
        /// Input Data : Sequence - 'CGCAUGCCGAUG'
        /// Output Data : Empty string.
        /// </summary>
        [Test]
        public void ValidateTwelveCharsProteinTranslationWithOffsetTwelve()
        {
            // Get Node values from XML.
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.RnaSequenceWithTwelveChars);
            string expectedOffset = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.OffsetVaule3);

            // Translate twelve characters RNA to protein with  offset value "12".
            ISequence proteinTranslation = new Sequence(Alphabets.RNA, expectedSeq);
            ISequence protein = ProteinTranslation.Translate(proteinTranslation,
                Convert.ToInt32(expectedOffset, null));

            // Validate Protein Translation.
            Assert.AreEqual(protein.ToString(), string.Empty);
            ApplicationLog.WriteLine(
                "Translation P1: Protein translation validation is completed successfully.");
        }

        /// <summary>
        /// Validate a Protein translation for a twelve characters sequence with offset value "12".
        /// Input Data : Sequence - 'AUGCGCCCGAUGCGC'
        /// Output Data : "Arginine".
        /// </summary>
        [Test]
        public void ValidateMoreThanTwelveCharsProteinTranslationWithOffsetTwelve()
        {
            // Get Node values from XML.
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.RnaSequenceWithMoreThanTwelveChars);
            string expectedProtein = Utility._xmlUtil.GetTextValue(
                Constants.TranslationNode, Constants.AminoAcidForOffsetTwelve);
            string expectedOffset = Utility._xmlUtil.GetTextValue(
                Constants.CodonsNode, Constants.OffsetVaule3);

            // Translate twelve characters RNA to protein with  offset value "12".
            ISequence proteinTranslation = new Sequence(Alphabets.RNA, expectedSeq);
            ISequence protein = ProteinTranslation.Translate(proteinTranslation,
                Convert.ToInt32(expectedOffset, null));

            // Validate Protein Translation.
            Assert.AreEqual(protein.ToString(), expectedProtein);
            ApplicationLog.WriteLine(
                "Translation P1: Protein translation validation is completed successfully.");
        }

        /// <summary>
        /// Validates a Protein Translation for a given DNA sequence with offset value "3".
        /// Input Data : Sequence - 'ATGGCG'
        /// Output Data : "Arginine".
        /// </summary>
        [Test]
        public void ValidateDnaProteinTranslationWithOffset()
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(Constants.TranscribeNode,
                Constants.DnaSequence);
            string expectedProtein = Utility._xmlUtil.GetTextValue(Constants.TranslationNode,
                Constants.AminoAcidForOffsetTwelve);
            string expectedOffset = Utility._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.OffsetVaule1);
            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Transcription of DNA Sequence.
            ISequence transcribe = Transcription.Transcribe(seq);

            // Protein Translation by passing offset value.
            ISequence proteinTranslation = new Sequence(Alphabets.RNA, transcribe.ToString());
            ISequence protein = ProteinTranslation.Translate(proteinTranslation,
                Convert.ToInt32(expectedOffset, null));

            // Validate Protein Translation.
            Assert.AreEqual(protein.ToString(), expectedProtein);
            ApplicationLog.WriteLine(
                "Translation P1: Protein translation validation is completed successfully.");
        }

        /// <summary>
        /// Validates a Protein Translation for a given RNA sequence with offset value "3".
        /// Input Data : Sequence - 'AUGCGCCCGAUG'
        /// Output Data : "RPM".
        /// </summary>
        [Test]
        public void ValidateRnaProteinTranslationWithThreeOffset()
        {
            // Get Node values from XML.
            string expectedSeq = Utility._xmlUtil.GetTextValue(Constants.TranslationNode,
                Constants.RnaSequenceWithTwelveChars);
            string expectedProtein = Utility._xmlUtil.GetTextValue(Constants.TranslationNode,
                Constants.AminoAcidForMoreOffsetThree);
            string expectedOffset = Utility._xmlUtil.GetTextValue(Constants.CodonsNode,
                Constants.OffsetVaule1);

            // Translate twelve characters RNA to protein.
            ISequence proteinTranslation = new Sequence(Alphabets.RNA, expectedSeq);
            ISequence protein = ProteinTranslation.Translate(proteinTranslation,
                Convert.ToInt32(expectedOffset, null));

            // Validate Protein Translation.
            Assert.AreEqual(protein.ToString(), expectedProtein);
            ApplicationLog.WriteLine(
                "Translation P1: Protein translation validation is completed successfully.");
        }

        #endregion Translation P1 TestCases

        #region Helper Methods

        /// <summary>
        /// Passes a valid RNA sequence with offset value and validates if
        /// Lookup(sequence) method is returning corresponding amino acid.
        /// </summary>
        /// <param name="sequenceNode">Sequence xml node.</param>
        /// <param name="offsetNode">Offset node with value.</param>
        /// <param name="aminoAcidNode">Expected amino acid value.</param>
        void ValidateCodonsTranslationWithOffset(string sequenceNode,
            string offsetNode, string aminoAcidNode)
        {
            // Get Node values from XML.
            string expectedSeq = Utility._xmlUtil.GetTextValue(Constants.TranslationNode,
                sequenceNode);
            string expectedAminoAcid = Utility._xmlUtil.GetTextValue(Constants.TranslationNode,
                aminoAcidNode);
            string expectedOffset = Utility._xmlUtil.GetTextValue(Constants.CodonsNode, offsetNode);

            // Translate Rna to corresponding amino acid.
            ISequence codonsTranslation = new Sequence(Alphabets.RNA, expectedSeq);
            AminoAcid aminoAcid = Codons.Lookup(codonsTranslation, int.Parse(expectedOffset, null));

            // Validate Codon Translation.
            Assert.AreEqual(expectedAminoAcid.ToString(), aminoAcid.Symbol.ToString());
            ApplicationLog.WriteLine(
                "Translation P1: Codon translation with offset validation is completed successfully.");
        }

        /// <summary>
        /// Passes a valid DNA sequence and validates corresponding transcribe sequence i.e. RNA Sequence
        /// </summary>
        /// <param name="sequenceNode">Sequence xml node.</param>
        /// <param name="revTranscribeNode">Expected transcribe sequence.</param>
        void ValidateDnaToRnaTranscribe(string sequenceNode, string transcribeNode)
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(Constants.TranscribeNode,
                sequenceNode);
            string expectedTranscribe = Utility._xmlUtil.GetTextValue(Constants.TranscribeNode,
                transcribeNode);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedSeq);

            // Transcribe DNA Sequence.
            ISequence transcribe = Transcription.Transcribe(seq);

            // Validate Dna to Rna transcribe.
            Assert.AreEqual(transcribe.ToString(), expectedTranscribe);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Transcribe {0} is expected.", transcribe));
            ApplicationLog.WriteLine(
                "Translation P1: Transcribe to Dna to Rna sequence completed successfully.");
        }

        /// <summary>
        /// Passes a valid RNA sequence and validates corresponding Reverse transcribe sequence i.e. DNA Sequence
        /// </summary>
        /// <param name="sequenceNode">Sequence xml node.</param>
        /// <param name="revTranscribeNode">Expected reverse transcribe sequence.</param>
        void ValidateRnaToDnaReverseTranscribe(string sequenceNode, string revTranscribeNode)
        {
            // Get Node values from XML.
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleRnaAlphabetNode,
                Constants.AlphabetNameNode);
            string expectedRnaSeq = Utility._xmlUtil.GetTextValue(Constants.TranscribeNode,
                sequenceNode);
            string expectedReverseTranscribe = Utility._xmlUtil.GetTextValue(Constants.TranscribeNode,
                revTranscribeNode);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), expectedRnaSeq);

            // Reverse Transcribe Rna Sequence.
            ISequence revTranscribe = Transcription.ReverseTranscribe(seq);

            // Validate Rna to Dna Reverse Transcribe.
            Assert.AreEqual(revTranscribe.ToString(), expectedReverseTranscribe);
            ApplicationLog.WriteLine(string.Format(null,
                "Translation P1: Reverse Transcribe {0} is expected.", revTranscribe));
            ApplicationLog.WriteLine(
                "Translation P1: Reverse Transcribe from Rna to Dna sequence completed successfully.");
        }

        #endregion
    }
}
