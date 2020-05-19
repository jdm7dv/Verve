// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * FastaP1TestCases.cs
 * 
 *   This file contains the Fasta - Parsers and Formatters Priority One test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using MBF.Encoding;
using MBF.IO;
using MBF.IO.Fasta;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;
using System.Text;

namespace MBF.TestAutomation.IO.Fasta
{
    /// <summary>
    /// FASTA Priority One parser and formatter test cases implementation.
    /// </summary>
    [TestFixture]
    public class FastaP1TestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static FastaP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region Fasta Parser P1 Test cases

        /// <summary>
        /// Parse a valid FastA file (DNA) and using Parse(file-name) method and 
        /// validate the expected sequence
        /// Input : DNA FastA File
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithDnaSequence()
        {
            ValidateParseGeneralTestCases(Constants.SimpleFastaDnaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file (Protein) and using Parse(file-name) method and 
        /// validate the expected sequence
        /// Input : Protein FastA File
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithProteinSequence()
        {
            ValidateParseGeneralTestCases(Constants.SimpleFastaProteinNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file (RNA) and using Parse(file-name) method and 
        /// validate the expected sequence
        /// Input : RNA FastA File
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithRnaSequence()
        {
            ValidateParseGeneralTestCases(Constants.SimpleFastaRnaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file (DNA) which is of less than 100KB
        /// and using Parse(file-name) method and validate the expected 
        /// sequence
        /// Input : Medium Size FastA File
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithMediumSizeSequence()
        {
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeFastaNodeName, Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser : File Exists in the Path '{0}'.", filePath));

            IList<ISequence> seqs = null;
            FastaParser parserObj = new FastaParser();

            seqs = parserObj.Parse(filePath);

            Assert.IsNotNull(seqs);
            Assert.AreEqual(1, seqs.Count);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: Number of Sequences found are '{0}'.",
                seqs.Count.ToString((IFormatProvider)null)));

            // Gets the expected sequence from the Xml
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeFastaNodeName, Constants.ExpectedSequenceNode);

            Sequence seq = (Sequence)seqs[0];
            Assert.IsNotNull(seq);

            // Replace all the empty spaces, paragraphs and new line for validation
            string updatedExpSequence =
                expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            string updatedActualSequence =
                seq.ToString().Replace("\r", "").Replace("\n", "").Replace(" ", "");
            Assert.AreEqual(updatedExpSequence, updatedActualSequence);
            ApplicationLog.WriteLine(
                string.Format(null, "FastA Parser: Sequence is '{0}' and is as expected.",
                updatedActualSequence));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(
                string.Format(null, "FastA Parser: Sequence is '{0}' and is as expected.",
                updatedActualSequence));

            Assert.AreEqual(updatedExpSequence.Length, updatedActualSequence.Length);
            ApplicationLog.WriteLine(
                string.Format(null, "FastA Parser: Sequence Length is '{0}' and is as expected.",
                updatedActualSequence.Length));

            Assert.IsNotNull(seq.Alphabet);
            Assert.AreEqual(seq.Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                Utility._xmlUtil.GetTextValue(Constants.MediumSizeFastaNodeName,
                Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture));
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: The Sequence Alphabet is '{0}' and is as expected.",
                seq.Alphabet.Name));

            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeFastaNodeName, Constants.SequenceIdNode), seq.ID);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: Sequence ID is '{0}' and is as expected.", seq.ID));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "FastA Parser: Sequence ID is '{0}' and is as expected.", seq.ID));

        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences and using 
        /// Parse(file-name) method and validate the expected sequence
        /// Input : Multiple sequences FastA File
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithMultipleSequences()
        {
            ValidateParseMultiSeqTestCases(Constants.MultipleSequenceFastaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file with one line sequence and using 
        /// Parse(file-name) method and validate the expected sequence
        /// Input : One line sequence FastA File
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithOneLineSequence()
        {
            ValidateParseGeneralTestCases(Constants.OneLineSequenceFastaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file with Encoding passed in constructor 
        /// and using Parse(file-name) method and 
        /// validate the expected sequence
        /// Input : DNA FastA File with Encoding specified
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithEncodingAsConstructor()
        {
            IEncoding enc = Encodings.Ncbi2NA;
            FastaParser parserObj = new FastaParser(enc);

            ValidateParserGeneralTestCases(parserObj);
        }

        /// <summary>
        /// Parse a valid FastA file with Encoding passed as property 
        /// and using Parse(file-name) method and 
        /// validate the expected sequence
        /// Input : DNA FastA File with Encoding specified
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithEncodingAsProperty()
        {
            FastaParser parserObj = new FastaParser();
            parserObj.Encoding = Encodings.Ncbi2NA;

            ValidateParserGeneralTestCases(parserObj);
        }

        /// <summary>
        /// Parse a valid FastA file with Alphabet passed as property 
        /// and using Parse(file-name) method and 
        /// validate the expected sequence
        /// Input : DNA FastA File with Encoding specified
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithAlphabetAsProperty()
        {
            FastaParser parserObj = new FastaParser();
            parserObj.Alphabet = Utility.GetAlphabet(
                Utility._xmlUtil.GetTextValue(Constants.SimpleFastaDnaNodeName,
                Constants.AlphabetNameNode));

            ValidateParserGeneralTestCases(parserObj);
        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences (one DNA and one RNA)
        /// and using Parse(file-name) method and validate the expected sequence
        /// Input : Multiple sequences FastA File (DNA and RNA)
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithMultiSeqDnaRna()
        {
            ValidateParseMultiSeqTestCases(Constants.MultipleSequenceDnaRnaFastaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences (one RNA and one Protein)
        /// and using Parse(file-name) method and validate the expected sequence
        /// Input : Multiple sequences FastA File (RNA and Protein)
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithMultiSeqRnaProtein()
        {
            ValidateParseMultiSeqTestCases(Constants.MultipleSequenceRnaProFastaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences (one DNA and one Protein)
        /// and using Parse(file-name) method and validate the expected sequence
        /// Input : Multiple sequences FastA File (DNA and Protein)
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithMultiSeqDnaProtein()
        {
            ValidateParseMultiSeqTestCases(Constants.MultipleSequenceDnaProFastaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences (one DNA, 
        /// one RNA and one Protein) and using Parse(file-name) method 
        /// and validate the expected sequence
        /// Input : Multiple sequences FastA File (DNA, RNA and Protein)
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithMultiSeqDnaRnaProtein()
        {
            ValidateParseMultiSeqTestCases(Constants.MultipleSequenceDnaRnaProFastaNodeName);
        }

        /// <summary>
        /// Parse a valid large FastA file and validate its properties.
        /// Input : Large sequence FastA File (greater than 100 KB and less than 350 KB)
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate properties like StrandType; StrandTopology; Division; Date; Version; 
        /// PrimaryID; Sequence; Metadata Count and Sequence ID.
        /// </summary>
        [Test]
        public void FastaParserValidatePropertiesWithLargeSizeSequence()
        {
            ValidateParseWithPropertiesTestCases(Constants.LargeSizeFasta);
        }

        /// <summary>
        /// Parse a valid FastA file (DNA) and using Parse(file-name) method and 
        /// validate the expected sequence with DV enabled
        /// Input : DNA FastA File with DV enabled
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseWithDV()
        {
            ValidateParseGeneralTestCasesWithDV(Constants.SimpleFastaDnaDVNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file (DNA) and using Parse(file-name) method and 
        /// validate the expected sequence with DV enabled
        /// Input : DNA FastA File with DV enabled
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseHugeFileYAxisWithDV()
        {
            ValidateParseGeneralTestCasesWithDV(Constants.HugeFastAFileYAxisNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file (Protein) and using Parse(file-name) method and 
        /// validate the expected sequence with DV enabled
        /// Input : DNA FastA File with DV enabled
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseHugeFileXAxisWithDV()
        {
            ValidateParseGeneralTestCasesWithDV(Constants.HugeFastAFileXAxisNodeName);
        }

        #endregion Fasta Parser P1 Test cases

        #region Fasta Formatter P1 Test cases

        /// <summary>
        /// Format a valid DNA Sequence to a 
        /// FastA file Format() method and validate the same.
        /// Input : FastA DNA Sequence
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatWithDnaSequence()
        {
            ValidateFormatterGeneralTestCases(Constants.SimpleFastaDnaNodeName);
        }

        /// <summary>
        /// Format a valid RNA Sequence to a 
        /// FastA file Format() method and validate the same.
        /// Input : FastA RNA Sequence
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatWithRnaSequence()
        {
            ValidateFormatterGeneralTestCases(Constants.SimpleFastaRnaNodeName);
        }

        /// <summary>
        /// Format a valid Protein Sequence to a 
        /// FastA file Format() method and validate the same.
        /// Input : FastA Protein Sequence
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatWithProteinSequence()
        {
            ValidateFormatterGeneralTestCases(Constants.SimpleFastaProteinNodeName);
        }

        /// <summary>
        /// Parse a FastA DNA File using Parse() method and Format the 
        /// same to a FastA file using Format() method and validate the same.
        /// Input : FastA DNA File which would be parsed
        /// Validation : Read the New FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterValidateWithParseDnaSequence()
        {
            ValidateParseFormatGeneralTestCases(Constants.SimpleFastaDnaNodeName);
        }

        /// <summary>
        /// Parse a FastA RNA File using Parse() method and Format the 
        /// same to a FastA file using Format() method and validate the same.
        /// Input : FastA RNA File which would be parsed
        /// Validation : Read the New FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterValidateWithParseRnaSequence()
        {
            ValidateParseFormatGeneralTestCases(Constants.SimpleFastaRnaNodeName);
        }

        /// <summary>
        /// Parse a FastA Protein File using Parse() method and Format the 
        /// same to a FastA file using Format() method and validate the same.
        /// Input : FastA Protein File which would be parsed
        /// Validation : Read the New FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterValidateWithParseProteinSequence()
        {
            ValidateParseFormatGeneralTestCases(Constants.SimpleFastaProteinNodeName);
        }

        /// <summary>
        /// Format a valid medium size i.e., less than 100KB Fasta File 
        /// using Format() method and validate the same.
        /// Input : Medium size FastA file
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatWithMediumSizeSequence()
        {
            ValidateFormatterGeneralTestCases(Constants.MediumSizeFastaNodeName);
        }

        /// <summary>
        /// Format a valid large size i.e., greater than 100 KB and less tha 350 KB
        /// using Format() method and validate the same.
        /// Input : Large size FastA file
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatWithLargeSizeSequence()
        {
            ValidateFormatterGeneralTestCases(Constants.LargeSizeFasta);
        }

        /// <summary>
        /// Parse a FastA File (Medium size sequence less than 100 Kb) using Parse() method 
        /// and Format the same to a FastA file using FormatString() method and validate the same.
        /// Input :  Medium size FastA File which would be parsed
        /// Validation : Validate the output of FormatString() method with the expected sequence
        /// </summary>
        [Test]
        public void FastaFormatterValidateWithParseFormatStringMediumSizeSeq()
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(Constants.MediumSizeFastaNodeName,
                Constants.FilePathNode);
            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: File Exists in the Path '{0}'.", filePath));

            FastaParser parserObj = new FastaParser();
            FastaFormatter formatter = new FastaFormatter();

            // Read the original file
            IList<ISequence> seqsOriginal = null;
            parserObj = new FastaParser();
            seqsOriginal = parserObj.Parse(filePath);
            Assert.IsNotNull(seqsOriginal);

            string testSequenceOrginal =
                Utility._xmlUtil.GetTextValue(Constants.MediumSizeFastaNodeName,
                Constants.FormatStringNode);
            string formattedString = formatter.FormatString(seqsOriginal[0]).Trim();
            testSequenceOrginal =
                testSequenceOrginal.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            formattedString =
                formattedString.Replace("\r", "").Replace("\n", "").Replace(" ", "");

            Assert.AreEqual(testSequenceOrginal, formattedString);
            Console.WriteLine(string.Format(null,
                "FastA Formatter BVT: The FASTA sequences '{0}' are matching with FormatString() method and is as expected.",
                testSequenceOrginal));
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: The FASTA sequences '{0}' are matching with FormatString() method.",
                testSequenceOrginal));
        }

        /// <summary>
        /// Parse a medium size FastA File i.e., less than 100 KB 
        /// using Parse() method and Format the 
        /// same to a FastA file using Format() method and validate the same.
        /// Input : Medium size FastA File which would be parsed
        /// Validation : Read the New FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterValidateWithParseMediumSizeSequence()
        {
            ValidateParseFormatGeneralTestCases(Constants.MediumSizeFastaNodeName);
        }

        /// <summary>
        /// Parse a large size FastA File i.e., greater than 100 KB and less than 350 KB
        /// using Parse() method and Format the 
        /// same to a FastA file using Format() method and validate the same.
        /// Input : Large size FastA File which would be parsed
        /// Validation : Read the New FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterValidateWithParseLargeSizeSequence()
        {
            ValidateParseFormatGeneralTestCases(Constants.LargeSizeFasta);
        }

        /// <summary>
        /// Format a valid Sequence to a FastA file using Format() method and 
        /// validate the same by Parsing it back.
        /// Input : FastA Sequence
        /// Validation : Read the FastA file using Parse() and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterValidateWithFormatAndParse()
        {
            ValidateFormatterGeneralTestCases(Constants.SimpleFastaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences (one DNA and one RNA)
        /// and using Parse(file-name) method and Format the same to temp file
        /// and validate the expected sequence
        /// Input : Multiple sequences FastA File (DNA and RNA)
        /// Validation : Format the sequences read and validate the FastA file to 
        /// which the sequence was formatted and validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseFormatWithMultiSeqDnaRna()
        {
            ValidateParseFormatMultiSeqTestCases(Constants.MultipleSequenceDnaRnaFastaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences (one RNA and one Protein)
        /// and using Parse(file-name) method and Format the same to temp file
        /// and validate the expected sequence
        /// Input : Multiple sequences FastA File (RNA and Protein)
        /// Validation : Format the sequences read and validate the FastA file to 
        /// which the sequence was formatted and validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseFormatWithMultiSeqRnaProtein()
        {
            ValidateParseFormatMultiSeqTestCases(Constants.MultipleSequenceRnaProFastaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences (one DNA and one Protein)
        /// and using Parse(file-name) method and Format the same to temp file
        /// and validate the expected sequence
        /// Input : Multiple sequences FastA File (DNA and Protein)
        /// Validation : Format the sequences read and validate the FastA file to 
        /// which the sequence was formatted and validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseFormatWithMultiSeqDnaProtein()
        {
            ValidateParseFormatMultiSeqTestCases(Constants.MultipleSequenceDnaProFastaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences (one DNA, one RNA and one Protein)
        /// and using Parse(file-name) method and Format the same to temp file
        /// and validate the expected sequence
        /// Input : Multiple sequences FastA File (DNA, RNA and Protein)
        /// Validation : Format the sequences read and validate the FastA file to 
        /// which the sequence was formatted and validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaParserValidateParseFormatWithMultiSeqDnaRnaProtein()
        {
            ValidateParseFormatMultiSeqTestCases(Constants.MultipleSequenceDnaRnaProFastaNodeName);
        }

        /// <summary>
        /// Format a valid Sequence to a FastA file using FormatString() method and validate the same by Parsing the same file.
        /// Input : valid sequence
        /// Validation: Format the valid sequence to a FastA file and validate format string(). Validate again by reparsing the same.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithValidSeq()
        {
            ValidateFormatStringValidSeqTestCases(Constants.SimpleFastaNodeName, true);
        }

        /// <summary>
        /// Format a valid DNA sequence using FormatString() method and validate the same.
        /// Input : valid sequence
        /// Validation: Format the valid sequence using format string() and validate the same.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithValidDnaSequence()
        {
            ValidateFormatStringValidSeqTestCases(Constants.SimpleFastaDnaNodeName, false);
        }

        /// <summary>
        /// Format a valid RNA sequence using FormatString() method and validate the same.
        /// Input : valid sequence
        /// Validation: Format the valid sequence using format string() and validate the same.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithValidRnaSequence()
        {
            ValidateFormatStringValidSeqTestCases(Constants.SimpleFastaRnaNodeName, false);
        }

        /// <summary>
        /// Format a valid Protein sequence using FormatString() method and validate the same.
        /// Input : valid sequence
        /// Validation: Format the valid sequence using format string() and validate the same.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithValidProteinSequence()
        {
            ValidateFormatStringValidSeqTestCases(Constants.SimpleFastaProteinNodeName, false);
        }

        /// <summary>
        /// Format a valid large sequence using FormatString() method and validate the same.
        /// Input : valid sequence
        /// Validation: Format the valid large sequence using format string() and validate the same.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithValidLargeSequence()
        {
            ValidateFormatStringValidSeqTestCases(Constants.LargeSizeFasta, false);
        }

        /// <summary>
        /// Parse DNA sequence FastA File and validate the same using FormatString() method.
        /// Input : DNA sequence FastA File.
        /// Validation: Parse DNA sequence FastA File and validate the same using FormatString() method.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithDnaSequence()
        {
            ValidateParseFormatStringTestCases(Constants.SimpleFastaDnaNodeName);
        }

        /// <summary>
        /// Parse RNA sequence FastA File and validate the same using FormatString() method.
        /// Input : RNA sequence FastA File.
        /// Validation: Parse RNA sequence FastA File and validate the same using FormatString() method.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithRnaSequence()
        {
            ValidateParseFormatStringTestCases(Constants.SimpleFastaRnaNodeName);
        }

        /// <summary>
        /// Parse Protein sequence FastA File and validate the same using FormatString() method.
        /// Input : Protein sequence FastA File.
        /// Validation: Protein DNA sequence FastA File and validate the same using FormatString() method.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithProteinSequence()
        {
            ValidateParseFormatStringTestCases(Constants.SimpleFastaProteinNodeName);
        }

        /// <summary>
        /// Parse a medium size FastA file i.e. less than 100 kb using FormatString() 
        /// method and validate the same.
        /// Input : Medium size FastA file
        /// Validation:  Parse a medium size FastA file i.e. less than 100 kb using 
        /// FormatString() method and validate the same.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithMediumSizeSequence()
        {
            ValidateParseFormatStringTestCases(Constants.MediumSizeFastaNodeName);
        }

        /// <summary>
        /// Parse a large size FastA file i.e. Large size sequence greater than 
        /// 100 kb and less than 350kb using FormatString() method and validate the same.
        /// Input : Large size FastA file
        /// Validation:  Parse a medium size FastA file i.e. less than 100 kb using 
        /// FormatString() method and validate the same.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithLargeSizeSequence()
        {
            ValidateParseFormatStringTestCases(Constants.LargeSizeFasta);
        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences (one RNA and one DNA)
        /// and validate each sequence using FormatString() method
        /// Input : Multiple sequences FastA File (DNA and RNA)
        /// Validation : Validate each sequences using FormatString() method.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithMultiSeqDnaRna()
        {
            ValidateParseFastaFileFormatStringMultiSeqTestCases(
                Constants.MultipleSequenceDnaRnaFastaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences (one RNA and one Protein)
        /// and validate each sequence using FormatString() method
        /// Input : Multiple sequences FastA File (RNA and Protein)
        /// Validation : Validate each sequences using FormatString() method.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithMultiSeqRnaProtein()
        {
            ValidateParseFastaFileFormatStringMultiSeqTestCases(
                Constants.MultipleSequenceRnaProFastaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences (one DNA and one Protein)
        /// and validate each sequence using FormatString() method
        /// Input : Multiple sequences FastA File (DNA and Protein)
        /// Validation : Validate each sequences using FormatString() method.
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatStringWithMultiSeqDnaProtein()
        {
            ValidateParseFastaFileFormatStringMultiSeqTestCases(
                Constants.MultipleSequenceDnaProFastaNodeName);
        }

        /// <summary>
        /// Parse a valid FastA file with multiple sequences (one DNA, one RNA and one Protein)
        /// and validate each sequence using FormatString() method
        /// Input : Multiple sequences FastA File (DNA, RNA and Protein)
        /// Validation : Validate each sequences using FormatString() method.
        /// </summary>
        [Test]
        public void FastaFormatterWithParseValidateFormatStringWithMultiSeqDnaRnaProtein()
        {
            ValidateParseWithTextReaderFormatStringMultiSeqTestCases(
                Constants.MultipleSequenceDnaRnaProFastaNodeName);
        }

        #endregion Fasta Formatter P1 Test cases

        #region Supporting Methods

        /// <summary>
        /// Validates general Parse test cases with the xml node name specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateParseGeneralTestCases(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser : File Exists in the Path '{0}'.", filePath));

            IList<ISequence> seqs = null;
            FastaParser parserObj = new FastaParser();

            seqs = parserObj.Parse(filePath);

            Assert.IsNotNull(seqs);
            Assert.AreEqual(1, seqs.Count);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: Number of Sequences found are '{0}'.",
                seqs.Count.ToString((IFormatProvider)null)));

            // Gets the expected sequence from the Xml
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);

            Sequence seq = (Sequence)seqs[0];
            Assert.IsNotNull(seq);
            Assert.AreEqual(expectedSequence, seq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: Sequence is '{0}' and is as expected.",
                seq.ToString()));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "FastA Parser: Sequence is '{0}' and is as expected.", seq.ToString()));

            Assert.AreEqual(expectedSequence.Length, seq.EncodedValues.Length);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: Sequence Length is '{0}' and is as expected.",
                expectedSequence.Length));

            Assert.IsNotNull(seq.Alphabet);
            Assert.AreEqual(seq.Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture));
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: The Sequence Alphabet is '{0}' and is as expected.",
                seq.Alphabet.Name));

            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode), seq.ID);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: Sequence ID is '{0}' and is as expected.",
                seq.ID));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "FastA Parser: Sequence ID is '{0}' and is as expected.",
                seq.ID));
        }

        /// <summary>
        /// Validates Parse test cases for files with multiple sequences 
        /// with the xml node name specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateParseMultiSeqTestCases(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(
                          nodeName, Constants.FilePathNode);

            ValidateParseMultiSeqTestCases(nodeName, filePath);
        }

        /// <summary>
        /// Validates Parse test cases for files with multiple sequences 
        /// with the xml node name, fasta file path specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="filePath">input Fasta file path.</param>
        static void ValidateParseMultiSeqTestCases(string nodeName, string filePath)
        {
            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser : File Exists in the Path '{0}'.", filePath));

            IList<ISequence> seqs = null;
            FastaParser parserObj = new FastaParser();

            seqs = parserObj.Parse(filePath);

            int seqCount = int.Parse(Utility._xmlUtil.GetTextValue(
                nodeName, Constants.NumberOfSequencesNode), null);

            Assert.IsNotNull(seqs);
            Assert.AreEqual(seqCount, seqs.Count);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: Number of Sequences found are '{0}'.",
                seqs.Count.ToString((IFormatProvider)null)));

            // Gets the expected sequences from the Xml, in the test cases
            // we are just validating with 2 sequences and maximum 3 
            // sequences. So, based on that we are validating.
            string expectedSequence1 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode1);
            string expectedSequence2 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode2);

            string[] expSeqs = null;

            if (2 == seqCount)
            {
                expSeqs = new string[2] { expectedSequence1, expectedSequence2 };
            }
            else
            {
                string expectedSequence3 = Utility._xmlUtil.GetTextValue(
                               nodeName, Constants.ExpectedSequenceNode3);
                expSeqs = new string[3] { expectedSequence1, 
                    expectedSequence2, expectedSequence3 };
            }

            string seqId1 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode1);
            string seqId2 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode2);

            string[] seqIds = null;

            if (2 == seqCount)
            {
                seqIds = new string[2] { seqId1, seqId2 };
            }
            else
            {
                string seqId3 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode3);
                seqIds = new string[3] { seqId1, seqId2, seqId3 };
            }

            // Gets the expected alphabets from the Xml
            string alp1 = Utility._xmlUtil.GetTextValue(
               nodeName, Constants.AlphabetNameNode1);
            string alp2 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode2);

            string[] alps = null;

            if (2 == seqCount)
            {
                alps = new string[2] { alp1, alp2 };
            }
            else
            {
                string alp3 = Utility._xmlUtil.GetTextValue(
                               nodeName, Constants.AlphabetNameNode3);
                alps = new string[3] { alp1, alp2, alp3 };
            }

            for (int i = 0; i < seqCount; i++)
            {
                Sequence seq = (Sequence)seqs[i];
                Assert.IsNotNull(seq);
                string updatedExpSequence =
                    expSeqs[i].Replace("\r", "").Replace("\n", "").Replace(" ", "");
                string updatedActualSequence =
                    seq.ToString().Replace("\r", "").Replace("\n", "").Replace(" ", "");

                Assert.AreEqual(updatedExpSequence, updatedActualSequence);
                ApplicationLog.WriteLine(string.Format(null,
                    "FastA Parser: Sequence is '{0}' and is as expected.",
                    seq.ToString()));
                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format(null,
                    "FastA Parser: Sequence is '{0}' and is as expected.", seq.ToString()));

                Assert.AreEqual(updatedExpSequence.Length, updatedActualSequence.Length);
                ApplicationLog.WriteLine(string.Format(null,
                    "FastA Parser: Sequence Length is '{0}' and is as expected.",
                    updatedActualSequence.Length));

                Assert.IsNotNull(seq.Alphabet);
                Assert.AreEqual(seq.Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                    alps[i].ToLower(CultureInfo.CurrentCulture));
                ApplicationLog.WriteLine(string.Format(null,
                    "FastA Parser: The Sequence Alphabet is '{0}' and is as expected.",
                    seq.Alphabet.Name));

                Assert.AreEqual(seqIds[i], seq.ID);
                ApplicationLog.WriteLine(string.Format(null,
                    "FastA Parser: Sequence ID is '{0}' and is as expected.",
                    seq.ID));
                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format(null,
                    "FastA Parser: Sequence ID is '{0}' and is as expected.",
                    seq.ID));
            }
        }

        /// <summary>
        /// Validates general Parse test cases with Fasta parser object name specified.
        /// </summary>
        /// <param name="parserObj">fasta parser object.</param>
        static void ValidateParserGeneralTestCases(FastaParser parserObj)
        {
            IList<ISequence> seqs = null;
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaDnaNodeName, Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser : File Exists in the Path '{0}'.", filePath));

            seqs = parserObj.Parse(filePath);

            Assert.IsNotNull(seqs);
            Assert.AreEqual(1, seqs.Count);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser with Alphabet: Number of Sequences found are '{0}'.",
                seqs.Count.ToString((IFormatProvider)null)));

            // Gets the expected sequence from the Xml
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaDnaNodeName, Constants.ExpectedSequenceNode);

            Sequence seq = (Sequence)seqs[0];
            Assert.IsNotNull(seq);
            Assert.AreEqual(expectedSequence, seq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser with Alphabet: DNA Sequence is '{0}' and is as expected.",
                seq.ToString()));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "FastA Parser with Alphabet: DNA Sequence is '{0}' and is as expected.",
                seq.ToString()));

            Assert.AreEqual(expectedSequence.Length, seq.EncodedValues.Length);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser with Alphabet: DNA Sequence Length is '{0}' and is as expected.",
                expectedSequence.Length));

            Assert.IsNotNull(seq.Alphabet);
            Assert.AreEqual(seq.Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                Utility._xmlUtil.GetTextValue(Constants.SimpleFastaDnaNodeName,
                Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture));
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser with Alphabet: The Sequence Alphabet is '{0}' and is as expected.",
                seq.Alphabet.Name));

            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaDnaNodeName, Constants.SequenceIdNode), seq.ID);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser with Alphabet: DNA Sequence ID is '{0}' and is as expected.",
                seq.ID));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "FastA Parser with Alphabet: DNA Sequence ID is '{0}' and is as expected.",
                seq.ID));
        }

        /// <summary>
        /// Validates general FastA Formatter test cases with the xml node name specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateFormatterGeneralTestCases(string nodeName)
        {
            FastaFormatter formatter = new FastaFormatter();

            // Gets the actual sequence and the alphabet from the Xml
            string actualSequence = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.ExpectedSequenceNode);
            string alphabet = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter : Validating with Sequence '{0}' and Alphabet '{1}'.",
                actualSequence, alphabet));

            // Replacing all the empty characters, Paragraphs and null entries added 
            // while formatting the xml.
            Sequence seqOriginal = new Sequence(Utility.GetAlphabet(alphabet),
                actualSequence.Replace("\r", "").Replace("\n", "").Replace(" ", ""));
            Assert.IsNotNull(seqOriginal);

            // Use the formatter to write the original sequences to a temp file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter : Creating the Temp file '{0}'.",
                Constants.FastaTempFileName));
            using (TextWriter writer = new StreamWriter(Constants.FastaTempFileName))
            {
                formatter.Format(seqOriginal, writer);
            }

            // Read the new file, then compare the sequences
            IList<ISequence> seqsNew = null;
            FastaParser parserObj = new FastaParser();
            seqsNew = parserObj.Parse(Constants.FastaTempFileName);
            Assert.IsNotNull(seqsNew);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter : New Sequence is '{0}'.",
                seqsNew[0].ToString()));

            // Now compare the sequences.
            int countNew = seqsNew.Count();
            Assert.AreEqual(1, countNew);
            ApplicationLog.WriteLine("The Number of sequences are matching.");

            Assert.AreEqual(seqOriginal.ID, seqsNew[0].ID);
            string orgSeq = seqOriginal.ToString();
            string newSeq = seqsNew[0].ToString();
            Assert.AreEqual(orgSeq, newSeq);
            Console.WriteLine(string.Format(null,
                "FastA Formatter : The FASTA sequences '{0}' are matching with Format() method and is as expected.",
                seqsNew[0].ToString()));
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter : The FASTA sequences '{0}' are matching with Format() method.",
                seqsNew[0].ToString()));

            // Passed all the tests, delete the tmp file. If we failed an Assert,
            // the tmp file will still be there in case we need it for debugging.
            File.Delete(Constants.FastaTempFileName);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        /// <summary>
        /// Validates general FastA Parser test cases which are further Formatted
        /// with the xml node name specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateParseFormatGeneralTestCases(string nodeName)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter : File Exists in the Path '{0}'.",
                filePath));

            FastaParser parserObj = new FastaParser();
            FastaFormatter formatter = new FastaFormatter();

            // Read the original file
            IList<ISequence> seqsOriginal = null;
            parserObj = new FastaParser();
            seqsOriginal = parserObj.Parse(filePath);
            Assert.IsNotNull(seqsOriginal);

            // Use the formatter to write the original sequences to a temp file
            string filepathTmp = "tmp.ffn";
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter : Creating the Temp file '{0}'.",
                filepathTmp));
            using (TextWriter writer = new StreamWriter(filepathTmp))
            {
                foreach (Sequence s in seqsOriginal)
                {
                    formatter.Format(s, writer);
                }
            }

            // Read the new file, then compare the sequences
            IList<ISequence> seqsNew = null;
            parserObj = new FastaParser();
            seqsNew = parserObj.Parse(filepathTmp);
            Assert.IsNotNull(seqsNew);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter : New Sequence is '{0}'.",
                seqsNew[0].ToString()));

            // Now compare the sequences.
            int countOriginal = seqsOriginal.Count();
            int countNew = seqsNew.Count();
            Assert.AreEqual(countOriginal, countNew);
            ApplicationLog.WriteLine("FastA Formatter :The Number of sequences are matching.");

            int i;
            for (i = 0; i < countOriginal; i++)
            {
                Assert.AreEqual(seqsOriginal[i].ID, seqsNew[i].ID);
                string orgSeq = seqsOriginal[i].ToString();
                string newSeq = seqsNew[i].ToString();
                Assert.AreEqual(orgSeq, newSeq);
                Console.WriteLine(
                    string.Format(null,
                    "FastA Formatter : The FASTA sequences '{0}' are matching with Format() method and is as expected.",
                    seqsNew[i].ID));
                ApplicationLog.WriteLine(
                    string.Format(null, "FastA Formatter : The FASTA sequences '{0}' are matching with Format() method.",
                    seqsNew[i].ID));
            }

            // Passed all the tests, delete the tmp file. If we failed an Assert,
            // the tmp file will still be there in case we need it for debugging.
            File.Delete(filepathTmp);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        /// <summary>
        /// Validates general FastA Parser test cases (multi Sequences)
        /// which are further Formatted with the xml node name specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateParseFormatMultiSeqTestCases(string nodeName)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter : File Exists in the Path '{0}'.",
                filePath));

            FastaParser parserObj = new FastaParser();
            FastaFormatter formatter = new FastaFormatter();

            // Read the original file
            IList<ISequence> seqsOriginal = null;
            parserObj = new FastaParser();
            seqsOriginal = parserObj.Parse(filePath);
            Assert.IsNotNull(seqsOriginal);

            // Use the formatter to write the original sequences to a temp file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter : Creating the Temp file '{0}'.",
                Constants.FastaTempFileName));

            // Formats a particular string.
            formatter.Format(seqsOriginal, Constants.FastaTempFileName);

            ValidateParseMultiSeqTestCases(nodeName, Constants.FastaTempFileName);

            // Passed all the tests, delete the tmp file. If we failed an Assert,
            // the tmp file will still be there in case we need it for debugging.
            File.Delete(Constants.FastaTempFileName);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        /// <summary>
        /// Validate the format string using valid sequence.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="reParse">true, if need to reparse the file and validation of the same.</param>
        static void ValidateFormatStringValidSeqTestCases(string nodeName, bool reParse)
        {
            // Create a valid sequence.
            string expectedSequence = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.ExpectedSequenceNode);
            string alphabetName = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode);
            Sequence vaildSeq = new Sequence(Utility.GetAlphabet(alphabetName),
                expectedSequence);
            Assert.IsNotNull(vaildSeq);
            vaildSeq.ID = Utility._xmlUtil.GetTextValue(nodeName, Constants.SequenceIdNode);

            // Validate the format string.
            FastaFormatter formatter = new FastaFormatter();
            string formatStrOrginalSeq = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FormatStringNode);
            string formatStrNew = formatter.FormatString(vaildSeq).Trim();
            string formatStrNewSeq =
                formatStrNew.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            formatStrOrginalSeq =
                formatStrOrginalSeq.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            Assert.AreEqual(formatStrOrginalSeq, formatStrNewSeq);
            ApplicationLog.WriteLine(
                string.Format(null,
                "FastA Formatter P1: The FASTA sequences '{0}' are matching with FormatString() method.",
                formatStrOrginalSeq));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(
                string.Format(null,
                "FastA Formatter P1: The FASTA sequences '{0}' are matching with FormatString() method.",
                formatStrOrginalSeq));

            if (reParse)
            {
                // Write the valid sequence to file.
                using (StreamWriter sw = new StreamWriter(Constants.FastaTempFileName))
                {
                    sw.WriteLine(formatStrNew);
                    sw.Close();
                }

                // Read the new file, then compare the sequences.
                IList<ISequence> seqsNew = null;
                FastaParser parserObj = new FastaParser();
                seqsNew = parserObj.Parse(Constants.FastaTempFileName);
                Assert.IsNotNull(seqsNew);
                ApplicationLog.WriteLine(string.Format(null,
                    "FastA Formatter P1: New Sequence is '{0}'.",
                    seqsNew[0].ToString()));

                // Compare the number of sequences.
                int countNew = seqsNew.Count();
                Assert.AreEqual(1, countNew);
                ApplicationLog.WriteLine("FastA Formatter P1 :The Number of sequences are matching.");

                // Compare the sequence.
                Assert.AreEqual(vaildSeq.ID, seqsNew[0].ID);
                Assert.AreEqual(vaildSeq.ToString(), seqsNew[0].ToString());
                ApplicationLog.WriteLine(
                    string.Format(null,
                    "FastA Formatter P1 : The FASTA sequences '{0}' are matching with Format() method.",
                    seqsNew[0].ID));
                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(
                    string.Format(null,
                    "FastA Formatter P1 : The FASTA sequences '{0}' are matching with Format() method.",
                    seqsNew[0].ID));

                // Passed all the tests, delete the tmp file. If we failed an Assert,
                // the tmp file will still be there in case we need it for debugging.
                File.Delete(Constants.FastaTempFileName);
                ApplicationLog.WriteLine("Deleted the temp file created.");
            }
        }

        /// <summary>
        /// Parse a FastA file and validate the same using FormatString() method
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateParseFormatStringTestCases(string nodeName)
        {
            // Get the file path.
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter P1: File Exists in the Path '{0}'.", filePath));

            FastaParser parserObj = new FastaParser();
            FastaFormatter formatter = new FastaFormatter();

            // Read the original file
            IList<ISequence> seqsOriginal = null;
            parserObj = new FastaParser();
            seqsOriginal = parserObj.Parse(filePath);
            Assert.IsNotNull(seqsOriginal);

            // Get the sequences using Format string() method.
            // Compare the sequences.
            string formatStrExpected = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FormatStringNode);
            string formatStrOriginal = formatter.FormatString(seqsOriginal[0]).Trim();
            formatStrOriginal =
                formatStrOriginal.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            formatStrExpected =
                formatStrExpected.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            Assert.AreEqual(formatStrExpected, formatStrOriginal);
            Console.WriteLine(
                string.Format(null,
                "FastA Formatter P1: The FASTA sequences '{0}' are matching with FormatString() method and is as expected.",
                formatStrExpected));
            ApplicationLog.WriteLine(
                string.Format(null,
                "FastA Formatter P1: The FASTA sequences '{0}' are matching with FormatString() method.",
                formatStrExpected));
        }

        /// <summary>
        /// Validate each sequence from multi sequence list using FormatString() method
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="seqsOriginal">multi sequence list.</param>
        static void ValidateParseFormatStringMultiSeqTestCases(string nodeName,
            IList<ISequence> seqsOriginal)
        {
            // Create expected format strings array for each sequence
            string[] formatStringsExpected = null;
            string formatString1Expected = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FormatStringNode1);
            string formatString2Expected = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FormatStringNode2);
            if (seqsOriginal.Count == 2)
            {
                formatStringsExpected =
                    new string[2] { formatString1Expected, formatString2Expected };
            }
            else
            {
                string formatString3Expected = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.FormatStringNode3);
                formatStringsExpected =
                    new string[3] { formatString1Expected, formatString2Expected, 
                        formatString3Expected };
            }

            // Compare the sequences using FormatString() method.
            FastaFormatter formatter = new FastaFormatter();
            for (int seqCount = 0; seqCount < seqsOriginal.Count; seqCount++)
            {
                string formatStrOriginal =
                    formatter.FormatString(seqsOriginal[seqCount]).Trim();
                formatStrOriginal =
                    formatStrOriginal.Replace("\r", "").Replace("\n", "").Replace(" ", "");
                formatStringsExpected[seqCount] =
                    formatStringsExpected[seqCount].Trim().Replace("\r", "").Replace("\n", "").Replace(" ", "");
                Assert.AreEqual(formatStringsExpected[seqCount], formatStrOriginal);
                Console.WriteLine(string.Format(null,
                    "FastA Formatter P1: The FASTA sequence '{0}' are matching with FormatString() method and is as expected.",
                    formatStringsExpected[seqCount]));
                ApplicationLog.WriteLine(string.Format(null,
                    "FastA Formatter P1: The FASTA sequence '{0}' are matching with FormatString() method.",
                    formatStringsExpected[seqCount]));
            }
        }

        /// <summary>
        /// Parse multi sequence FastA file and validate each sequence using FormatString() method
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateParseFastaFileFormatStringMultiSeqTestCases(string nodeName)
        {
            // Get the file path.
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter P1: File Exists in the Path '{0}'.", filePath));

            FastaParser parserObj = new FastaParser();

            // Read the original file
            IList<ISequence> seqsOriginal = null;
            parserObj = new FastaParser();
            seqsOriginal = parserObj.Parse(filePath);
            Assert.IsNotNull(seqsOriginal);

            // Validate each sequences.
            ValidateParseFormatStringMultiSeqTestCases(nodeName, seqsOriginal);
        }

        /// <summary>
        /// Parse multi sequence FastA file using text reader and 
        /// validate each sequence using FormatString() method
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateParseWithTextReaderFormatStringMultiSeqTestCases(string nodeName)
        {
            // Get the file path.
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter P1: File Exists in the Path '{0}'.", filePath));

            FastaParser parserObj = new FastaParser();

            // Read the original file using text reader
            IList<ISequence> seqsOriginal = null;
            using (TextReader reader = new StreamReader(filePath))
            {
                seqsOriginal = parserObj.Parse(reader);
                Assert.IsNotNull(seqsOriginal);
            }

            // Validate each sequences.
            ValidateParseFormatStringMultiSeqTestCases(nodeName, seqsOriginal);
        }

        /// <summary>
        /// Validates Parse with properties
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateParseWithPropertiesTestCases(string nodeName)
        {
            // Get the file path.
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter P1: File Exists in the Path '{0}'.", filePath));

            FastaParser parserObj = new FastaParser();
            // Read the original file using text reader
            IList<ISequence> seqsOriginal = parserObj.Parse(filePath);
            Assert.IsNotNull(seqsOriginal);

            // Create a Sequence with all attributes.
            // Parse and update the properties instead of parsing entire file.
            string expectedSequence =
                Utility._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode);
            string alphabetName =
                Utility._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string expectedUpdatedSequence =
                expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            Sequence orgSeq =
                new Sequence(Utility.GetAlphabet(alphabetName), expectedUpdatedSequence);
            orgSeq.ID = seqsOriginal[0].ID;

            ISequenceFormatter formatter = new FastaFormatter();
            using (TextWriter writer = new StreamWriter(Constants.GenBankTempFileName))
            {
                formatter.Format(orgSeq, writer);
            }

            // Parse
            IList<ISequence> seqList = parserObj.Parse(Constants.GenBankTempFileName);
            ISequence seq = seqList[0];

            // Get the expected properties values.
            string seqId = Utility._xmlUtil.GetTextValue(nodeName, Constants.SequenceIdNode);
            Assert.AreEqual(Utility.GetAlphabet(alphabetName), seq.Alphabet);
            Assert.AreEqual(seqId, seq.ID);
            ApplicationLog.WriteLine(
                "FastA Formatter P1: Successfully validated the Alphabet and Sequence ID");

            // Test that we're correctly putting all types of metadata in the right places
            Assert.AreEqual(orgSeq.Metadata.Count, seq.Metadata.Count);
            ApplicationLog.WriteLine(
                "FastA Formatter P1 : Successfully validated the Metadata Count");

            string truncatedExpectedSequence =
                expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(CultureInfo.CurrentCulture);
            string truncatedActualSequence =
                seq.ToString().Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(CultureInfo.CurrentCulture);
            // Test the sequence string
            Assert.AreEqual(truncatedExpectedSequence, truncatedActualSequence);
            ApplicationLog.WriteLine(
                "FastA Formatter P1: Successfully validated the Sequence");
            Console.WriteLine(string.Format(null,
                "FastA Formatter P1: Successfully validated the Sequence '{0}'",
                truncatedExpectedSequence));

            File.Delete(Constants.GenBankTempFileName);
        }

        /// <summary>
        /// Validates general Parse test cases with DV enabled and 
        /// with the xml node name specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateParseGeneralTestCasesWithDV(string nodeName)
        {
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser : File Exists in the Path '{0}'.", filePath));

            IList<ISequence> seqs = null;
            FastaParser parserObj = new FastaParser();
            parserObj.EnforceDataVirtualization = true;

            seqs = parserObj.Parse(filePath, false);

            // Gets the expected count from the Xml
            int expectedCount = int.Parse(Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceCountNode));

            Assert.AreEqual(expectedCount, seqs.Count);
            ApplicationLog.WriteLine(
                "FastA Parser: Sequence count is as expected.");
            Console.WriteLine(
                "FastA Parser: Sequence count is as expected.");

            // Gets the expected sequence from the Xml
            string expectedSequence = Utility._xmlUtil.GetFileTextValue(
                nodeName, Constants.ExpectedSequenceNode).Replace("\r", "").Replace("\n", "");

            StringBuilder strBuildObj = new StringBuilder();
            foreach (ISequence seqObj in seqs)
            {
                strBuildObj.Append(string.Concat(seqObj.ToString(), ","));
            }

            Assert.AreEqual(expectedSequence,
                strBuildObj.ToString());

            ApplicationLog.WriteLine(
                "FastA Parser: Sequences are as expected.");
            Console.WriteLine(
                "FastA Parser: Sequences are as expected.");

            Assert.AreEqual(expectedSequence.Length, strBuildObj.ToString().Length);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: Sequence Length is '{0}' and is as expected.",
                expectedSequence.Length));

            Assert.IsNotNull(seqs[0].Alphabet);
            Assert.AreEqual(seqs[0].Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture));
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: The Sequence Alphabet is '{0}' and is as expected.",
                seqs[0].Alphabet.Name));

            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode), seqs[0].ID);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: Sequence ID is '{0}' and is as expected.",
                seqs[0].ID));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "FastA Parser: Sequence ID is '{0}' and is as expected.",
                seqs[0].ID));
        }

        #endregion Supporting Methods
    }
}
