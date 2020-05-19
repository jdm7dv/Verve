// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * FastQP1TestCases.cs
 * 
 *This file contains FastQ Parsers and Formatters P1 test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using MBF.Encoding;
using MBF.IO;
using MBF.IO.FastQ;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.IO.FastQ
{
    /// <summary>
    /// FASTQ P1 parser and formatter Test cases implementation.
    /// </summary>
    [TestFixture]
    public class FastQP1TestCases
    {

        #region Enums

        /// <summary>
        /// FastQ Parser Property Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum FastQPropertyParameters
        {
            FastQConstructorPam,
            EncodingProperty,
            AlphabetProperty,
            Default
        };

        /// <summary>
        /// FastQ Formatter Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum FastQFileParameters
        {
            FileName,
            FileNameReadOnly,
            ParseFileName
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static FastQP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\FastQTestsConfig.xml");
        }

        #endregion Constructor

        #region FastQ P1 Test cases

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateFastQParserWithSangerUsingFastQExtensionFile()
        {
            ValidateFastQParser(Constants.SimpleSangerFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid small size Rna FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Rna FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateFastQParserWithRnaSanger()
        {
            ValidateFastQParser(Constants.SimpleRnaSangerFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid small size Rna FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Rna FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateFastQParserWithRnaIllumina()
        {
            ValidateFastQParser(Constants.SimpleRnaIlluminaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid small size Rna FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Rna FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateFastQParserWithRnaSolexa()
        {
            ValidateFastQParser(Constants.SimpleRnaSolexaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid small size Dna FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Dna FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateFastQParserWithDnaSanger()
        {
            ValidateFastQParser(Constants.SimpleSangerFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid small size Dna FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Rna FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateFastQParserWithDnaIllumina()
        {
            ValidateFastQParser(Constants.SimpleIlluminaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid small size Dna FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Rna FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateFastQParserWithDnaSolexa()
        {
            ValidateFastQParser(Constants.SimpleSolexaFqFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid small size Protein FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Protein FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateFastQParserWithProteinSanger()
        {
            ValidateFastQParser(Constants.SimpleProteinSangerFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid small size Protein FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Protein FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateFastQParserWithProteinIllumina()
        {
            ValidateFastQParser(Constants.SimpleProteinIlluminaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid small size Protein FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Protein FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateFastQParserWithProteinSolexa()
        {
            ValidateFastQParser(Constants.SimpleProteinSolexaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid Medium size Sanger FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Medium size FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithMediumSizeSangerDnaSequence()
        {
            ValidateFastQParser(Constants.MediumSizeDnaSangerFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid Medium size Illumina FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Medium size FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithMediumSizeIlluminaDnaSequence()
        {
            ValidateFastQParser(Constants.MediumSizeDnaIlluminaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid Medium size Solexa FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Medium size FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithMediumSizeSolexaDnaSequence()
        {
            ValidateFastQParser(Constants.MediumSizeDnaSolexaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid Large size Sanger FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Large size FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithLargeSizeSangerDnaSequence()
        {
            ValidateFastQParser(Constants.LargeSizeDnaSangerFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid Large size Illumina FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Large size FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithLargeSizeIlluminaDnaSequence()
        {
            ValidateFastQParser(Constants.LargeSizeDnaIlluminaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid Large size Solexa FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Large size FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithLargeSizeSolexaDnaSequence()
        {
            ValidateFastQParser(Constants.LargeSizeDnaSolexaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid One line sequence Illumina FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : One line Dna sequence Illumina FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithOneLineIluminaDnaSequence()
        {
            ValidateFastQParser(Constants.SingleSequenceIlluminaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid One line sequence Sanger FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : One line Dna sequence Sanger FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithOneLineSangerDnaSequence()
        {
            ValidateFastQParser(Constants.SingleSequenceSangerFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid One line sequence Solexa FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : One line Dna sequence Solexa FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithOneLineSolexaDnaSequence()
        {
            ValidateFastQParser(Constants.SingleSequenceSolexaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid Two line Medium size sequence Sanger FastQ file and
        /// convert the same to sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : Two line medium size Dna sequence Sanger FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithTwoLineMediumSizeSangerDnaSequence()
        {
            ValidateFastQParser(Constants.TwoLineDnaSangerFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid Two line Medium size sequence Illumina FastQ file and
        /// convert the same to sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : Two line medium size Dna sequence Sanger FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithTwoLineMediumSizeIlluminaDnaSequence()
        {
            ValidateFastQParser(Constants.TwoLineDnaIlluminaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid Two line Medium size sequence Solexa FastQ file and
        /// convert the same to sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : Two line medium size Dna sequence Solexa FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithTwoLineMediumSizeSolexaDnaSequence()
        {
            ValidateFastQParser(Constants.TwoLineDnaSolexaFastQNode,
              FastQPropertyParameters.Default);
        }

        /// <summary>
        /// Parse a valid simple Rna sequence Sanger FastQ file by
        /// passing encoding in FastQ constructor.and validate with
        /// the expected sequence.
        /// Input : Valid simple Rna sequence with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithSangerByPassingEncoding()
        {
            ValidateFastQParser(Constants.SimpleRnaSangerNode,
              FastQPropertyParameters.FastQConstructorPam);
        }

        /// <summary>
        /// Parse a valid simple Rna sequence Illumina FastQ file by
        /// passing encoding in FastQ constructor.and validate with
        /// the expected sequence.
        /// Input : Valid simple Rna sequence with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithIlluminaByPassingEncoding()
        {
            ValidateFastQParser(Constants.SimpleRnaIlluminaFastQNode,
              FastQPropertyParameters.FastQConstructorPam);
        }

        /// <summary>
        /// Parse a valid simple Rna sequence Solexa FastQ file by
        /// passing encoding in FastQ constructor.and validate with
        /// the expected sequence.
        /// Input : Valid simple Rna sequence with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithSolexaByPassingEncoding()
        {
            ValidateFastQParser(Constants.SimpleRnaSolexaFastQNode,
              FastQPropertyParameters.FastQConstructorPam);
        }

        /// <summary>
        /// Parse a valid simple Rna sequence Sanger FastQ file by
        /// updating encoding property and validate with the expected sequence.
        /// Input : Valid simple Rna sequence with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithSangerByUpdatingEncodingProperty()
        {
            ValidateFastQParser(Constants.SimpleRnaSangerNode,
              FastQPropertyParameters.EncodingProperty);
        }

        /// <summary>
        /// Parse a valid simple Rna sequence Illumina FastQ file by
        /// updating encoding property and validate with the expected sequence.
        /// Input : Valid simple Rna sequence with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithIlluminaByUpdatingEncodingProperty()
        {
            ValidateFastQParser(Constants.SimpleRnaIlluminaFastQNode,
              FastQPropertyParameters.EncodingProperty);
        }

        /// <summary>
        /// Parse a valid simple Rna sequence Solexa FastQ file by
        /// updating encoding property and validate with the expected sequence.
        /// Input : Valid simple Rna sequence with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithSolexaByUpdatingEncodingProperty()
        {
            ValidateFastQParser(Constants.SimpleRnaSolexaFastQNode,
              FastQPropertyParameters.EncodingProperty);
        }

        /// <summary>
        /// Parse a valid simple Rna sequence Sanger FastQ file by
        /// updating Alphabets property and validate with the expected sequence.
        /// Input : Valid simple Rna sequence with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithSangerByUpdatingAlphabetProperty()
        {
            ValidateFastQParser(Constants.SimpleRnaSangerNode,
              FastQPropertyParameters.EncodingProperty);
        }

        /// <summary>
        /// Parse a valid simple Rna sequence Illumina FastQ file by
        /// updating Alphabets property and validate with the expected sequence.
        /// Input : Valid simple Rna sequence with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithIlluminaByUpdatingAlphabetProperty()
        {
            ValidateFastQParser(Constants.SimpleRnaIlluminaFastQNode,
              FastQPropertyParameters.EncodingProperty);
        }

        /// <summary>
        /// Parse a valid simple Rna sequence Solexa FastQ file by
        /// updating Alphabets property and validate with the expected sequence.
        /// Input : Valid simple Rna sequence with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithSolexaByUpdatingAlphabetProperty()
        {
            ValidateFastQParser(Constants.SimpleRnaSolexaFastQNode,
              FastQPropertyParameters.EncodingProperty);
        }

        /// <summary>
        /// Parse a valid Dna Rna multiple sequence FastQ file and 
        /// convert the same to sequence using Parse(file-name) method 
        /// and validate with the expected sequence.
        /// Input : Multiple sequence FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateDnaRnaMultipleSeqFastQParserWithSanger()
        {
            ValidateMulitpleSequenceFastQParser(Constants.MultiSeqSangerDnaRnaNode, null);
        }

        /// <summary>
        /// Parse a valid Dna Rna multiple sequence FastQ file and 
        /// convert the same to sequence using Parse(file-name) method 
        /// and validate with the expected sequence.
        /// Input : Multiple sequence FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateDnaRnaMultipleSeqFastQParserWithIllumina()
        {
            ValidateMulitpleSequenceFastQParser(Constants.MultiSeqIlluminaDnaRnaNode, null);
        }

        /// <summary>
        /// Parse a valid Dna Rna multiple sequence FastQ file and 
        /// convert the same to sequence using Parse(file-name) method 
        /// and validate with the expected sequence.
        /// Input : Multiple sequence FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateDnaRnaMultipleSeqFastQParserWithSolexa()
        {
            ValidateMulitpleSequenceFastQParser(Constants.MultiSeqSolexaDnaRnaNode, null);
        }

        /// <summary>
        /// Parse a valid Rna Protein multiple sequence FastQ file and 
        /// convert the same to sequence using Parse(file-name) method 
        /// and validate with the expected sequence.
        /// Input : Multiple sequence FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateRnaProteinMultipleSeqFastQParserWithSanger()
        {
            ValidateMulitpleSequenceFastQParser(Constants.MultiSeqSangerRnaProNode, null);
        }

        /// <summary>
        /// Parse a valid Rna Protein multiple sequence FastQ file and 
        /// convert the same to sequence using Parse(file-name) method 
        /// and validate with the expected sequence.
        /// Input : Multiple sequence FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateRnaProteinMultipleSeqFastQParserWithIllumina()
        {
            ValidateMulitpleSequenceFastQParser(Constants.MultiSeqIlluminaRnaProNode, null);
        }

        /// <summary>
        /// Parse a valid Rna Protein multiple sequence FastQ file and 
        /// convert the same to sequence using Parse(file-name) method 
        /// and validate with the expected sequence.
        /// Input : Multiple sequence FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateRnaProteinMultipleSeqFastQParserWithSolexa()
        {
            ValidateMulitpleSequenceFastQParser(Constants.MultiSeqSolexaRnaProNode, null);
        }

        /// <summary>
        /// Parse a valid Dna Protein multiple sequence FastQ file and 
        /// convert the same to sequence using Parse(file-name) method 
        /// and validate with the expected sequence.
        /// Input : Multiple sequence FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateDnaProteinMultipleSeqFastQParserWithSanger()
        {
            ValidateMulitpleSequenceFastQParser(Constants.MultiSeqSangerDnaProNode, null);
        }

        /// <summary>
        /// Parse a valid Dna Protein multiple sequence FastQ file and 
        /// convert the same to sequence using Parse(file-name) method 
        /// and validate with the expected sequence.
        /// Input : Multiple sequence FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateDnaProteinMultipleSeqFastQParserWithIllumina()
        {
            ValidateMulitpleSequenceFastQParser(Constants.MultiSeqIlluminaDnaProNode, null);
        }

        /// <summary>
        /// Parse a valid Dna Protein multiple sequence FastQ file and 
        /// convert the same to sequence using Parse(file-name) method 
        /// and validate with the expected sequence.
        /// Input : Multiple sequence FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateDnaProteinMultipleSeqFastQParserWithSolexa()
        {
            ValidateMulitpleSequenceFastQParser(Constants.MultiSeqSolexaDnaProNode, null);
        }

        /// <summary>
        /// Parse a valid Dna Rna Protein sequences FastQ file and 
        /// convert the same to sequence using Parse(file-name) method 
        /// and validate with the expected sequence.
        /// Input : Multiple sequence FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateDnaRnaProteinMultipleSeqFastQParserWithSanger()
        {
            ValidateMulitpleSequenceFastQParser(Constants.MultiSeqSangerDnaRnaProNode,
              "MultiSequenceFastQ");
        }

        /// <summary>
        /// Parse a valid Dna Rna Protein sequences FastQ file and 
        /// convert the same to sequence using Parse(file-name) method 
        /// and validate with the expected sequence.
        /// Input : Multiple sequence FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateDnaRnaProteinMultipleSeqFastQParserWithIllumina()
        {
            ValidateMulitpleSequenceFastQParser(Constants.MultiSeqIlluminaDnaRnaProNode,
              "MultiSequenceFastQ");
        }

        /// <summary>
        /// Parse a valid Dna Rna Protein sequences FastQ file and 
        /// convert the same to sequence using Parse(file-name) method 
        /// and validate with the expected sequence.
        /// Input : Multiple sequence FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateDnaRnaProteinMultipleSeqFastQParserWithSolexa()
        {
            ValidateMulitpleSequenceFastQParser(Constants.MultiSeqSolexaDnaRnaProNode,
              "MultiSequenceFastQ");
        }

        /// <summary>
        /// Format a valid Sanger Qualitative Sequence to FastQ file using
        /// FomatString and validate the formatted string.
        /// Input : Sanger qualitative sequence.
        /// Output : Validate sanger format string.
        /// </summary>
        [Test]
        public void FastQFormatterSangerFormatString()
        {
            ValidateFastQFormatStringByFormattingQualSeqeunce(Constants.SimpleSangerFastQNode);
        }

        /// <summary>
        /// Format a valid Ilumina Qualitative Sequence to FastQ file using
        /// FomatString and validate the formatted string.
        /// Input : Sanger qualitative sequence.
        /// Output : Validate sanger format string.
        /// </summary>
        [Test]
        public void FastQFormatterIluminaFormatString()
        {
            ValidateFastQFormatStringByFormattingQualSeqeunce(Constants.SimpleIlluminaFastQNode);
        }

        /// <summary>
        /// Parse a Sanger FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using FomatString and validate the formatted string.
        /// Input : Sanger FastQ file
        /// Output : Validate sanger format string.
        /// </summary>
        [Test]
        public void FastQFormatStringParseSangerFastQ()
        {
            ValidateFastQFormatStringForamttingFastQFile(Constants.SimpleSangerFastQNode);
        }

        /// <summary>
        /// Parse a Illumina FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using FomatString and validate the formatted string.
        /// Input : Illumina FastQ file
        /// Output : Validate Illumina format string.
        /// </summary>
        [Test]
        public void FastQFormatStringParseIlluminaFastQ()
        {
            ValidateFastQFormatStringForamttingFastQFile(Constants.SimpleIlluminaFastQNode);
        }

        /// <summary>
        /// Parse a Sanger FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using Format(text-writer) and validate Sequence.
        /// Input : Sanger FastQ file
        /// Output : Validate format Sanger FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatSangerFastQ()
        {
            ValidateFastQFormatter(Constants.SimpleSangerFastQNode, true);
        }

        /// <summary>
        /// Parse a Sanger FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using Format(text-writer) and validate Sequence.
        /// Input :Dna Sanger FastQ file
        /// Output : Validate format Sanger FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatIlluminaFastQ()
        {
            ValidateFastQFormatter(Constants.SimpleIlluminaFastQNode, true);
        }

        /// <summary>
        /// Parse a Sanger FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using Format(file-name) and validate Sequence.
        /// Input : Sanger FastQ file
        /// Output : Validate format Sanger FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatFastQFileWithDnaSanger()
        {
            ValidateFastQFormatByFormattingQualSeqeunce(Constants.SimpleSangerFastQNode);
        }

        /// <summary>
        /// Parse a Illumina FastQ file and Format a valid Illumina Qualitative Sequence 
        /// to FastQ file using Format(file-name) and validate Sequence.
        /// Input :Dna Illumina FastQ file
        /// Output : Validate format Illumina FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatFastQFileWithDnaIllumina()
        {
            ValidateFastQFormatByFormattingQualSeqeunce(Constants.SimpleIlluminaFastQNode);
        }

        /// <summary>
        /// Parse a Solexa FastQ file and Format a valid Solexa Qualitative Sequence 
        /// to FastQ file using Format(file-name) and validate Sequence.
        /// Input :Dna Solexa FastQ file
        /// Output : Validate format Solexa FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatFastQFileWithDnaSolexa()
        {
            ValidateFastQFormatByFormattingQualSeqeunce(Constants.SimpleSolexaFqFastQNode);
        }

        /// <summary>
        /// Parse a Sanger FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using Format(file-name) and validate Sequence.
        /// Input : Rna Sanger FastQ file
        /// Output : Validate format Sanger FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatFastQFileWithRnaSanger()
        {
            ValidateFastQFormatByFormattingQualSeqeunce(Constants.SimpleRnaSangerFastQNode);
        }

        /// <summary>
        /// Parse a Illumina FastQ file and Format a valid Illumina Qualitative Sequence 
        /// to FastQ file using Format(file-name) and validate Sequence.
        /// Input : Rna Illumina FastQ file
        /// Output : Validate format Illumina FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatFastQFileWithRnaIllumina()
        {
            ValidateFastQFormatByFormattingQualSeqeunce(Constants.SimpleRnaIlluminaFastQNode);
        }

        /// <summary>
        /// Parse a Solexa FastQ file and Format a valid Solexa Qualitative Sequence 
        /// to FastQ file using Format(file-name) and validate Sequence.
        /// Input : Rna Solexa FastQ file
        /// Output : Validate format Solexa FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatFastQFileWithRnaSolexa()
        {
            ValidateFastQFormatByFormattingQualSeqeunce(Constants.SimpleRnaSolexaFastQNode);
        }

        /// <summary>
        /// Parse a Sanger FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using Format(file-name) and validate Sequence.
        /// Input : Protein Sanger FastQ file
        /// Output : Validate format Sanger FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatFastQFileWithProteinSanger()
        {
            ValidateFastQFormatByFormattingQualSeqeunce(Constants.SimpleProteinSangerFastQNode);
        }

        /// <summary>
        /// Parse a Illumina FastQ file and Format a valid Illumina Qualitative Sequence 
        /// to FastQ file using Format(file-name) and validate Sequence.
        /// Input : Protein Illumina FastQ file
        /// Output : Validate format Illumina FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatFastQFileWithProteinIllumina()
        {
            ValidateFastQFormatByFormattingQualSeqeunce(Constants.SimpleProteinIlluminaFastQNode);
        }

        /// <summary>
        /// Parse a Solexa FastQ file and Format a valid Solexa Qualitative Sequence 
        /// to FastQ file using Format(file-name) and validate Sequence.
        /// Input : Protein Solexa FastQ file
        /// Output : Validate format Solexa FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatFastQFileWithProteinSolexa()
        {
            ValidateFastQFormatByFormattingQualSeqeunce(Constants.SimpleProteinSolexaFastQNode);
        }

        /// <summary>
        /// Parse a Dna Sanger FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using Format(text-writer) and validate Sequence.
        /// Input :Dna Sanger FastQ file
        /// Output : Validate format Sanger FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatDnaSangerFastQUsingTextWriter()
        {
            ValidateFastQFormatter(Constants.SimpleSangerFastQNode, true);
        }

        /// <summary>
        /// Parse a Dna Illumina FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using Format(text-writer) and validate Sequence.
        /// Input :Dna Illumina FastQ file
        /// Output : Validate format Illumina FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatDnaIlluminaFastQUsingTextWriter()
        {
            ValidateFastQFormatter(Constants.SimpleIlluminaFastQNode, true);
        }

        /// <summary>
        /// Parse a Dna Solexa FastQ file and Format a valid Solexa Qualitative Sequence 
        /// to FastQ file using Format(text-writer) and validate Sequence.
        /// Input :Dna Solexa FastQ file
        /// Output : Validate format Solexa FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatDnaSolexaFastQUsingTextWriter()
        {
            ValidateFastQFormatter(Constants.SimpleSolexaFqFastQNode, true);
        }

        /// <summary>
        /// Parse a Rna Sanger FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using Format(text-writer) and validate Sequence.
        /// Input :Rna Sanger FastQ file
        /// Output : Validate format Sanger FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatRnaSangerFastQUsingTextWriter()
        {
            ValidateFastQFormatter(Constants.SimpleRnaSangerFastQNode, true);
        }

        /// <summary>
        /// Parse a Rna Illumina FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using Format(text-writer) and validate Sequence.
        /// Input :Rna Illumina FastQ file
        /// Output : Validate format Illumina FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatRnaIlluminaFastQUsingTextWriter()
        {
            ValidateFastQFormatter(Constants.SimpleRnaIlluminaFastQNode, true);
        }

        /// <summary>
        /// Parse a Rna Solexa FastQ file and Format a valid Solexa Qualitative Sequence 
        /// to FastQ file using Format(text-writer) and validate Sequence.
        /// Input :Rna Solexa FastQ file
        /// Output : Validate format Solexa FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatRnaSolexaFastQUsingTextWriter()
        {
            ValidateFastQFormatter(Constants.SimpleRnaSolexaFastQNode, true);
        }

        /// <summary>
        /// Parse a Protein Sanger FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using Format(text-writer) and validate Sequence.
        /// Input :Protein Sanger FastQ file
        /// Output : Validate format Sanger FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatProteinSangerFastQUsingTextWriter()
        {
            ValidateFastQFormatter(Constants.SimpleProteinSangerFastQNode, true);
        }

        /// <summary>
        /// Parse a Protein Illumina FastQ file and Format a valid Sanger Qualitative Sequence 
        /// to FastQ file using Format(text-writer) and validate Sequence.
        /// Input :Protein Illumina FastQ file
        /// Output : Validate format Illumina FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatProteinIlluminaFastQUsingTextWriter()
        {
            ValidateFastQFormatter(Constants.SimpleProteinIlluminaFastQNode, true);
        }

        /// <summary>
        /// Parse a Protein Solexa FastQ file and Format a valid Solexa Qualitative Sequence 
        /// to FastQ file using Format(text-writer) and validate Sequence.
        /// Input :Protein Solexa FastQ file
        /// Output : Validate format Solexa FastQ file to temp file.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatProteinSolexaFastQUsingTextWriter()
        {
            ValidateFastQFormatter(Constants.SimpleProteinSolexaFastQNode, true);
        }

        /// <summary>
        /// Parse a Dna Sanger FastQ file and Format a valid Sanger Qualitative Sequence 
        /// using FormatString() and validate the formatted string.
        /// Input :Dna Sanger FastQ file
        /// Output : Validate formatting Sanger FastQ string.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringWithDnaSanger()
        {
            ValidateFastQFormatStringForamttingFastQFile(Constants.SimpleSangerFastQNode);
        }

        /// <summary>
        /// Parse a Dna Solexa FastQ file and Format a valid Solexa Qualitative Sequence 
        /// using FormatString() and validate the formatted string.
        /// Input :Dna Solexa FastQ file
        /// Output : Validate formatting Solexa FastQ string.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringWithDnaSolexa()
        {
            ValidateFastQFormatStringForamttingFastQFile(Constants.SimpleSolexaFqFastQNode);
        }

        /// <summary>
        /// Parse a Dna Illumina FastQ file and Format a valid Illumina Qualitative Sequence 
        /// using FormatString() and validate the formatted string.
        /// Input :Dna Illumina FastQ file
        /// Output : Validate formatting Illumina FastQ string.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringWithDnaIllumina()
        {
            ValidateFastQFormatStringForamttingFastQFile(Constants.SimpleIlluminaFastQNode);
        }

        /// <summary>
        /// Parse a Rna Sanger FastQ file and Format a valid Sanger Qualitative Sequence 
        /// using FormatString() and validate the formatted string.
        /// Input :Rna Sanger FastQ file
        /// Output : Validate formatting Sanger FastQ string.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringWithRnaSanger()
        {
            ValidateFastQFormatStringForamttingFastQFile(Constants.SimpleRnaSangerFastQNode);
        }

        /// <summary>
        /// Parse a Rna Solexa FastQ file and Format a valid Solexa Qualitative Sequence 
        /// using FormatString() and validate the formatted string.
        /// Input :Rna Solexa FastQ file
        /// Output : Validate formatting Solexa FastQ string.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringWithRnaSolexa()
        {
            ValidateFastQFormatStringForamttingFastQFile(Constants.SimpleRnaSolexaFastQNode);
        }

        /// <summary>
        /// Parse a Rna Illumina FastQ file and Format a valid Illumina Qualitative Sequence 
        /// using FormatString() and validate the formatted string.
        /// Input :Rna Illumina FastQ file
        /// Output : Validate formatting Illumina FastQ string.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringWithRnaIllumina()
        {
            ValidateFastQFormatStringForamttingFastQFile(Constants.SimpleRnaIlluminaFastQNode);
        }

        /// <summary>
        /// Parse a Protein Sanger FastQ file and Format a valid Sanger Qualitative Sequence 
        /// using FormatString() and validate the formatted string.
        /// Input :Protein Sanger FastQ file
        /// Output : Validate formatting Sanger FastQ string.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringWithProteinSanger()
        {
            ValidateFastQFormatStringForamttingFastQFile(Constants.SimpleProteinSangerFastQNode);
        }

        /// <summary>
        /// Parse a Protein Solexa FastQ file and Format a valid Solexa Qualitative Sequence 
        /// using FormatString() and validate the formatted string.
        /// Input :Protein Solexa FastQ file
        /// Output : Validate formatting Solexa FastQ string.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringWithProteinSolexa()
        {
            ValidateFastQFormatStringForamttingFastQFile(Constants.SimpleProteinSolexaFastQNode);
        }

        /// <summary>
        /// Parse a Protein Illumina FastQ file and Format a valid Illumina Qualitative Sequence 
        /// using FormatString() and validate the formatted string.
        /// Input :Protein Illumina FastQ file
        /// Output : Validate formatting Illumina FastQ string.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringWithProteinIllumina()
        {
            ValidateFastQFormatStringForamttingFastQFile(Constants.SimpleProteinIlluminaFastQNode);
        }

        /// <summary>
        /// Create a Dna Sanger Qualitative Sequence and format a 
        /// using FormatString() and validate the formatted string.
        /// Input :Dna Sanger FastQ file
        /// Output : Validate formatting Sanger FastQ string.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringWithDnaSangerQualSeq()
        {
            ValidateFastQFormatStringByFormattingQualSeqeunce(Constants.SimpleSangerFastQNode);
        }

        /// <summary>
        /// Create a Rna Illumina Qualitative Sequence and format a 
        /// using FormatString() and validate the formatted string.
        /// Input :Rna Illumina FastQ file
        /// Output : Validate formatting Illumina FastQ string.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringWithRnaIlluminaQualSeq()
        {
            ValidateFastQFormatStringByFormattingQualSeqeunce(Constants.SimpleRnaIlluminaFastQNode);
        }

        /// <summary>
        /// Create a Protein Solexa Qualitative Sequence and format a 
        /// using FormatString() and validate the formatted string.
        /// Input :Protein Solexa FastQ file
        /// Output : Validate formatting Solexa FastQ string.
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringWithProteinSolexaQualSeq()
        {
            ValidateFastQFormatStringByFormattingQualSeqeunce(Constants.SimpleProteinSolexaFastQNode);
        }

        /// <summary>
        /// Format a medium size Solexa Qualitative sequence to FastQ
        /// file.using format(Text-Writer) method and validate the same.
        /// Input Data : Solexa Medium size sequence.
        /// Output Data : Validation of fromatting medium size Solexa 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatMediumSizeDnaSolexaUsingTextWriter()
        {
            ValidateFastQFormatter(Constants.MediumSizeDnaSolexaFastQNode, true);
        }

        /// <summary>
        /// Format a medium size Illumina Qualitative sequence to FastQ
        /// file.using format(Text-Writer) method and validate the same.
        /// Input Data : Illumina Medium size sequence.
        /// Output Data : Validation of fromatting medium size Illumina 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatMediumSizeDnaIlluminaUsingTextWriter()
        {
            ValidateFastQFormatter(Constants.MediumSizeDnaIlluminaFastQNode, true);
        }

        /// <summary>
        /// Format a medium size Sanger Qualitative sequence to FastQ
        /// file.using format(Text-Writer) method and validate the same.
        /// Input Data : Sanger Medium size sequence.
        /// Output Data : Validation of fromatting medium size sanger 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatMediumSizeDnaSangerUsingTextWriter()
        {
            ValidateFastQFormatter(Constants.MediumSizeDnaSangerFastQNode, true);
        }

        /// <summary>
        /// Format a medium size Solexa Qualitative sequence to FastQ
        /// file.using format(file-name) method and validate the same.
        /// Input Data : Solexa Medium size sequence.
        /// Output Data : Validation of fromatting medium size Solexa 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatMediumSizeDnaSolexaUsingFile()
        {
            ValidateFastQFormatter(Constants.MediumSizeDnaSolexaFastQNode, false);
        }

        /// <summary>
        /// Format a medium size Illumina Qualitative sequence to FastQ
        /// file.using format(file-name) method and validate the same.
        /// Input Data : Illumina Medium size sequence.
        /// Output Data : Validation of fromatting medium size Illumina 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatMediumSizeDnaIlluminaUsingFile()
        {
            ValidateFastQFormatter(Constants.MediumSizeDnaIlluminaFastQNode, false);
        }

        /// <summary>
        /// Format a medium size Sanger Qualitative sequence to FastQ
        /// file.using format(file-name) method and validate the same.
        /// Input Data : Sanger Medium size sequence.
        /// Output Data : Validation of fromatting medium size sanger 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatMediumSizeDnaSangerUsingFile()
        {
            ValidateFastQFormatter(Constants.MediumSizeDnaSangerFastQNode, false);
        }

        /// <summary>
        /// Format a medium size Sanger Qualitative sequence to FastQ
        /// file.using FormatString(Text-Writer) method and validate the same.
        /// Input Data : Sanger Medium size sequence.
        /// Output Data : Validation of formatting medium size sanger using ForomatString 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatStringMediumSizeDnaSangerSeq()
        {
            ValidateFastQFormatStringByFormattingQualSeqeunce(
              Constants.MediumSizeDnaSangerFastQNode);
        }

        /// <summary>
        /// Format a medium size Solexa Qualitative sequence to FastQ
        /// file.using FormatString(Text-Writer) method and validate the same.
        /// Input Data : Solexa Medium size sequence.
        /// Output Data : Validation of fromatting medium size Solexa using ForomatString 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatStringMediumSizeDnaSolexaSeq()
        {
            ValidateFastQFormatStringByFormattingQualSeqeunce(
              Constants.MediumSizeDnaSolexaFastQNode);
        }

        /// <summary>
        /// Format a medium size Illumina Qualitative sequence to FastQ
        /// file.using FormatString(Text-Writer) method and validate the same.
        /// Input Data : Illumina Medium size sequence.
        /// Output Data : Validation of fromatting medium size Illumina using ForomatString 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatStringMediumSizeDnaIlluminaSeq()
        {
            ValidateFastQFormatStringByFormattingQualSeqeunce(
              Constants.MediumSizeDnaIlluminaFastQNode);
        }

        /// <summary>
        /// Parse and Format a medium size Sanger Qualitative sequence to FastQ
        /// file.using FormatString(Text-Writer) method and validate the same.
        /// Input Data : Sanger Medium size sequence.
        /// Output Data : Validation of fromatting medium size sanger using ForomatString 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatStringWithParseMediumSizeDnaSangerSeq()
        {
            ValidateFastQFormatStringForamttingFastQFile(
              Constants.MediumSizeDnaSangerFastQNode);
        }

        /// <summary>
        /// Parse and Format a medium size Solexa Qualitative sequence to FastQ
        /// file.using FormatString(Text-Writer) method and validate the same.
        /// Input Data : Solexa Medium size sequence.
        /// Output Data : Validation of fromatting medium size Solexa using ForomatString 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatStringWithParseMediumSizeDnaSolexaSeq()
        {
            ValidateFastQFormatStringForamttingFastQFile(
              Constants.MediumSizeDnaSolexaFastQNode);
        }

        /// <summary>
        /// Parse and Format a medium size Illumina Qualitative sequence to FastQ
        /// file.using FormatString(Text-Writer) method and validate the same.
        /// Input Data : Illumina Medium size sequence.
        /// Output Data : Validation of fromatting medium size Illumina using ForomatString 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatStringWithParseMediumSizeDnaIlluminaSeq()
        {
            ValidateFastQFormatStringForamttingFastQFile(
              Constants.MediumSizeDnaIlluminaFastQNode);
        }

        /// <summary>
        /// Format a Large size(>100KB) Sanger Qualitative sequence to FastQ
        /// file.using Format() method and validate the same.
        /// Input Data : Sanger Large size sequence.
        /// Output Data : Validation of fromatting Large size Sanger 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatLargeSizeDnaSangerSeq()
        {
            ValidateFastQFormatByFormattingQualSeqeunce(
              Constants.LargeSizeDnaSangerFastQNode);
        }

        /// <summary>
        /// Format a Large size(>100KB) Illumina Qualitative sequence to FastQ
        /// file.using Format() method and validate the same.
        /// Input Data : Illumina Large size sequence.
        /// Output Data : Validation of fromatting Large size Illumina 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatLargeSizeDnaIlluminaSeq()
        {
            ValidateFastQFormatByFormattingQualSeqeunce(
              Constants.LargeSizeDnaIlluminaFastQNode);
        }

        /// <summary>
        /// Format a Large size(>100KB) Solexa Qualitative sequence to FastQ
        /// file.using Format() method and validate the same.
        /// Input Data : Solexa Large size sequence.
        /// Output Data : Validation of fromatting Large size Solexa 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatLargeSizeDnaSolexaSeq()
        {
            ValidateFastQFormatByFormattingQualSeqeunce(
              Constants.LargeSizeDnaSolexaFastQNode);
        }

        /// <summary>
        /// Format a Large size >100KB Sanger Qualitative sequence to FastQ
        /// file.using FormatString() method and validate the same.
        /// Input Data : Sanger Large size sequence.
        /// Output Data : Validation of fromatting medium size sanger using ForomatString 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatStringLargeSizeDnaSangerSeq()
        {
            ValidateFastQFormatStringByFormattingQualSeqeunce(
              Constants.LargeSizeDnaSangerFastQNode);
        }

        /// <summary>
        /// Format a medium size >100KB Solexa Qualitative sequence to FastQ
        /// file.using FormatString() method and validate the same.
        /// Input Data : Solexa Large size sequence.
        /// Output Data : Validation of fromatting medium size Solexa using ForomatString 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatStringLargeSizeDnaSolexaSeq()
        {
            ValidateFastQFormatStringByFormattingQualSeqeunce(
              Constants.LargeSizeDnaSolexaFastQNode);
        }

        /// <summary>
        /// Format a medium size >100KB Illumina Qualitative sequence to FastQ
        /// file.using FormatString() method and validate the same.
        /// Input Data : Illumina Large size sequence.
        /// Output Data : Validation of fromatting medium size Illumina using ForomatString 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatStringLargeSizeDnaIlluminaSeq()
        {
            ValidateFastQFormatStringByFormattingQualSeqeunce(
              Constants.LargeSizeDnaIlluminaFastQNode);
        }

        /// <summary>
        /// Parse and Format a Large size(>100KB) Sanger Qualitative sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Sanger Large size sequence.
        /// Output Data : Validation of fromatting Large size Sanger 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatLargeSizeDnaSangerFile()
        {
            ValidateFastQFormatter(Constants.LargeSizeDnaSangerFastQNode, false);
        }

        /// <summary>
        /// Parse and Format a Large size(>100KB) Illumina Qualitative sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Illumina Large size sequence.
        /// Output Data : Validation of fromatting Large size Illumina 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatLargeSizeDnaIlluminaFile()
        {
            ValidateFastQFormatter(Constants.LargeSizeDnaIlluminaFastQNode, false);
        }

        /// <summary>
        /// Parse and Format a Large size(>100KB) Solexa Qualitative sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Solexa Large size sequence.
        /// Output Data : Validation of fromatting Large size Solexa 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatLargeSizeDnaSolexaFile()
        {
            ValidateFastQFormatter(Constants.LargeSizeDnaSolexaFastQNode, false);
        }

        /// <summary>
        /// Parse and Format a Large size >100KB Sanger Qualitative sequence to FastQ
        /// file.using FormatString() method and validate the same.
        /// Input Data : Sanger Large size sequence.
        /// Output Data : Validation of fromatting medium size sanger using ForomatString 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatStringLargeSizeDnaSangerFile()
        {
            ValidateFastQFormatStringForamttingFastQFile(
              Constants.LargeSizeDnaSangerFastQNode);
        }

        /// <summary>
        /// Format a medium size >100KB Solexa Qualitative sequence to FastQ
        /// file.using FormatString() method and validate the same.
        /// Input Data : Solexa Large size sequence.
        /// Output Data : Validation of fromatting medium size Solexa using ForomatString 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatStringLargeSizeDnaSolexaFile()
        {
            ValidateFastQFormatStringForamttingFastQFile(
              Constants.LargeSizeDnaSolexaFastQNode);
        }

        /// <summary>
        /// Format a medium size >100KB Illumina Qualitative sequence to FastQ
        /// file.using FormatString() method and validate the same.
        /// Input Data : Illumina Large size sequence.
        /// Output Data : Validation of fromatting medium size Illumina using ForomatString 
        /// qualitative sequence to valid FastQ file.
        /// </summary>
        [Test]
        public void FastQFormatStringLargeSizeDnaIlluminaFie()
        {
            ValidateFastQFormatStringForamttingFastQFile(
              Constants.LargeSizeDnaIlluminaFastQNode);
        }

        /// <summary>
        /// Parse and Format a Sanger Qualitative Dna Rna sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Sanger Dna Rna multi sequence file.
        /// Output Data : Validation of Multi sequence FastQ format.
        /// </summary>
        [Test]
        public void FastQFormatDnaRnaSangerFile()
        {
            ValidateMultiSeqFastQFormatter(Constants.MultiSeqSangerDnaRnaNode);
        }

        /// <summary>
        /// Parse and Format a Illumina Qualitative Dna Rna sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Illumina Dna Rna multi sequence file.
        /// Output Data : Validation of Multi sequence FastQ format.
        /// </summary>
        [Test]
        public void FastQFormatDnaRnaIlluminaFile()
        {
            ValidateMultiSeqFastQFormatter(Constants.MultiSeqIlluminaDnaRnaNode);
        }

        /// <summary>
        /// Parse and Format a Solexa Qualitative Dna Rna sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Solexa Dna Rna multi sequence file.
        /// Output Data : Validation of Multi sequence FastQ format.
        /// </summary>
        [Test]
        public void FastQFormatDnaRnaSolexaFile()
        {
            ValidateMultiSeqFastQFormatter(Constants.MultiSeqSolexaDnaRnaNode);
        }

        /// <summary>
        /// Parse and Format a Sanger Qualitative Rna Protein sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Sanger Rna Protein multi sequence file.
        /// Output Data : Validation of Multi sequence FastQ format.
        /// </summary>
        [Test]
        public void FastQFormatRnaProteinSangerFile()
        {
            ValidateMultiSeqFastQFormatter(Constants.MultiSeqSangerRnaProNode);
        }

        /// <summary>
        /// Parse and Format a Illumina Qualitative Rna Protein sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Illumina Rna Protein multi sequence file.
        /// Output Data : Validation of Multi sequence FastQ format.
        /// </summary>
        [Test]
        public void FastQFormatRnaProteinIlluminaFile()
        {
            ValidateMultiSeqFastQFormatter(Constants.MultiSeqIlluminaRnaProNode);
        }

        /// <summary>
        /// Parse and Format a Solexa Qualitative Rna Protein sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Solexa Rna Protein multi sequence file.
        /// Output Data : Validation of Multi sequence FastQ format.
        /// </summary>
        [Test]
        public void FastQFormatRnaProteinSolexaFile()
        {
            ValidateMultiSeqFastQFormatter(Constants.MultiSeqSolexaRnaProNode);
        }

        /// <summary>
        /// Parse and Format a Sanger Qualitative Dna Protein sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Sanger Dna Protein multi sequence file.
        /// Output Data : Validation of Multi sequence FastQ format.
        /// </summary>
        [Test]
        public void FastQFormatDnaProteinSangerFile()
        {
            ValidateMultiSeqFastQFormatter(Constants.MultiSeqSangerDnaProNode);
        }

        /// <summary>
        /// Parse and Format a Illumina Qualitative Dna Protein sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Illumina Dna Protein multi sequence file.
        /// Output Data : Validation of Multi sequence FastQ format.
        /// </summary>
        [Test]
        public void FastQFormatDnaProteinIlluminaFile()
        {
            ValidateMultiSeqFastQFormatter(Constants.MultiSeqIlluminaDnaProNode);
        }

        /// <summary>
        /// Parse and Format a Solexa Qualitative Dna Protein sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Solexa Dna Protein multi sequence file.
        /// Output Data : Validation of Multi sequence FastQ format.
        /// </summary>
        [Test]
        public void FastQFormatDnaProteinSolexaFile()
        {
            ValidateMultiSeqFastQFormatter(Constants.MultiSeqSolexaDnaProNode);
        }

        /// <summary>
        /// Parse and Format a Sanger Qualitative Dna Rna Protein sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Sanger Dna Rna Protein multi sequence file.
        /// Output Data : Validation of Multi sequence FastQ format.
        /// </summary>
        [Test]
        public void FastQFormatDnaRnaProteinSangerFile()
        {
            ValidateMultiSeqFastQFormatter(Constants.MultiSeqSangerDnaRnaProNode);
        }

        /// <summary>
        /// Parse and Format a Illumina Qualitative Dna Rna Protein sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Illumina Dna Rna Protein multi sequence file.
        /// Output Data : Validation of Multi sequence FastQ format.
        /// </summary>
        [Test]
        public void FastQFormatDnaRnaProteinIlluminaFile()
        {
            ValidateMultiSeqFastQFormatter(Constants.MultiSeqIlluminaDnaRnaProNode);
        }

        /// <summary>
        /// Format a Default Qualitative Rna Protein sequence
        /// to FastQ file.using FormatString() method and validate the same..
        /// Input Data : Default Rna Protein multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringDnaProteinDefaultFastQFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqSolexaDnaProNode, false);
        }

        /// <summary>
        /// Format a Default Qualitative Dna Protein sequence
        /// to FastQ file.using FormatString() method and validate the same..
        /// Input Data : Solexa Dna Protein multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringRnaProteinDefaultFastQFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqSolexaRnaProNode, false);
        }

        /// <summary>
        /// Format a Default Qualitative Dna Rna sequence
        /// to FastQ file.using FormatString() method and validate the same..
        /// Input Data : Default Dna Rna multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringDnaRnaDefaultFastQFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqSolexaDnaRnaNode, false);
        }

        /// <summary>
        /// Parse and Format a Solexa Qualitative Dna Rna Protein sequence
        /// to FastQ file.using Format() method and validate the same.
        /// Input Data : Solexa Dna Rna Protein multi sequence file.
        /// Output Data : Validation of Multi sequence FastQ format.
        /// </summary>
        [Test]
        public void FastQFormatDnaRnaProteinSolexaFile()
        {
            ValidateMultiSeqFastQFormatter(Constants.MultiSeqSolexaDnaRnaProNode);
        }

        /// <summary>
        /// Format a Sanger Qualitative Dna Rna Protein sequence
        /// to FastQ file.using FormatString() method and validate the same.
        /// Input Data : Sanger Dna Rna Protein multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringDnaRnaProteinSangerFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqSangerDnaRnaProNode, true);
        }

        /// <summary>
        /// Format a Illumina Qualitative Dna Rna Protein sequence
        /// to FastQ file.using FormatString() method and validate the same.
        /// Input Data : Illumina Dna Rna Protein multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringDnaRnaProteinIlluminaFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqIlluminaDnaRnaProNode, true);
        }

        /// <summary>
        /// Format a Solexa Qualitative Dna Rna Protein sequence
        /// to FastQ file.using FormatString() method and validate the same..
        /// Input Data : Solexa Dna Rna Protein multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringDnaRnaProteinSolexaFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqSolexaDnaRnaProNode, true);
        }

        /// <summary>
        /// Format a Sanger Qualitative Dna Rna sequence
        /// to FastQ file.using FormatString() method and validate the same.
        /// Input Data : Sanger Dna Rna multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringDnaRnaSangerFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqSangerDnaRnaNode, false);
        }

        /// <summary>
        /// Format a Illumina Qualitative Dna Rna sequence
        /// to FastQ file.using FormatString() method and validate the same.
        /// Input Data : Illumina Dna Rna multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringDnaRnaIlluminaFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqIlluminaDnaRnaNode, false);
        }

        /// <summary>
        /// Format a Solexa Qualitative Dna Rna sequence
        /// to FastQ file.using FormatString() method and validate the same..
        /// Input Data : Solexa Dna Rna multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringDnaRnaSolexaFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqSolexaDnaRnaNode, false);
        }

        /// <summary>
        /// Format a Sanger Qualitative Rna Protein sequence
        /// to FastQ file.using FormatString() method and validate the same.
        /// Input Data : Sanger Rna Protein multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringRnaProteinSangerFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqSangerRnaProNode, false);
        }

        /// <summary>
        /// Format a Illumina Qualitative Rna Protein sequence
        /// to FastQ file.using FormatString() method and validate the same.
        /// Input Data : Illumina Rna Protein multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringRnaProteinIlluminaFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqIlluminaRnaProNode, false);
        }

        /// <summary>
        /// Format a Solexa Qualitative Dna Protein sequence
        /// to FastQ file.using FormatString() method and validate the same..
        /// Input Data : Solexa Dna Protein multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringRnaProteinSolexaFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqSolexaRnaProNode, false);
        }

        /// <summary>
        /// Format a Sanger Qualitative Dna Protein sequence
        /// to FastQ file.using FormatString() method and validate the same.
        /// Input Data : Sanger Dna Protein multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringDnaProteinSangerFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqSangerDnaProNode, false);
        }

        /// <summary>
        /// Format a Illumina Qualitative Dna Protein sequence
        /// to FastQ file.using FormatString() method and validate the same.
        /// Input Data : Illumina RDna Protein multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringDnaProteinIlluminaFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqIlluminaDnaProNode, false);
        }

        /// <summary>
        /// Format a Solexa Qualitative Rna Protein sequence
        /// to FastQ file.using FormatString() method and validate the same..
        /// Input Data : Solexa Rna Protein multi sequence file.
        /// Output Data : Validation of formatting multi sequence using FormatString().
        /// </summary>
        [Test]
        public void FastQFormatValidateFormatStringDnaProteinSolexaFile()
        {
            ValidateMultiSeqFastQFormatStringForamttingFastQFile(
              Constants.MultiSeqSolexaDnaProNode, false);
        }

        /// <summary>
        /// Parse a valid Medium size sequence Illumina FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Medium size Dna sequence Illumina FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithMediumIlluminaDnaSequenceUsingEncoding()
        {
            ValidateFastQParser(Constants.MediumSizeDnaIlluminaFastQNode,
              FastQPropertyParameters.EncodingProperty);
        }

        /// <summary>
        /// Parse a valid Medium size sequence Sanger FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : One line Dna sequence Sanger FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithMediumSangerDnaSequenceUsingEncoding()
        {
            ValidateFastQParser(Constants.MediumSizeDnaSangerFastQNode,
              FastQPropertyParameters.EncodingProperty);
        }

        /// <summary>
        /// Parse a valid Medium size sequence Solexa FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Medium size Dna sequence Solexa FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithMediumSolexaDnaSequenceUsingEncoding()
        {
            ValidateFastQParser(Constants.MediumSizeDnaSolexaFastQNode,
              FastQPropertyParameters.EncodingProperty);
        }

        /// <summary>
        /// Parse a valid FastQ file (DNA) and using Parse(file-name) method and 
        /// validate the expected sequence with DV enabled
        /// Input : DNA FastQ File with DV enabled
        /// Validation : Read the FastQ file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastQParserValidateParseWithDV()
        {
            ValidateParseGeneralTestCasesWithDV(Constants.SimpleFastQDnaDVNodeName);
        }

        /// <summary>
        /// Parse a valid FastQ file (DNA) and using Parse(file-name) method and 
        /// validate the expected sequence with DV enabled
        /// Input : DNA FastQ File with DV enabled
        /// Validation : Read the FastQ file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastQParserValidateParseHugeFileYAxisWithDV()
        {
            ValidateParseGeneralTestCasesWithDV(Constants.HugeFastQFileYAxisNodeName);
        }

        /// <summary>
        /// Parse a valid FastQ file (Protein) and using Parse(file-name) method and 
        /// validate the expected sequence with DV enabled
        /// Input : DNA FastQ File with DV enabled
        /// Validation : Read the FastQ file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastQParserValidateParseHugeFileXAxisWithDV()
        {
            ValidateParseGeneralTestCasesWithDV(Constants.HugeFastQFileXAxisNodeName);
        }

        /// <summary>
        /// Parse a valid small size Dna FastQ file and convert the same to 
        /// sequence using Parse(file-name, isReadOnly) method and validate
        /// with the expected sequence.
        /// Input : Dna FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [Test]
        public void ValidateFastQParserDataVirtulizationWithDnaSolexa()
        {
            ValidateFastQParserDataVirtulization(
                Constants.SimpleSolexaFqFastQNode,
                FastQFileParameters.FileNameReadOnly);
        }

        /// <summary>
        /// Parse a valid small size Dna FastQ file and convert the same to 
        /// sequence using ParseOne(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Dna FastQ file with Sanger format.
        /// Output : Validation of Expected sequence.
        /// </summary>
        [Test]
        public void ValidateFastQParserBasicSequenceWithDnaSanger()
        {
            ValidateBasicSequenceParser(
                Constants.SimpleSangerFastQNode,
                FastQFileParameters.ParseFileName);
        }

        /// <summary>
        /// Parse a valid small size Dna FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Dna FastQ file with Sanger format.
        /// Output : Validation of Expected sequence.
        /// </summary>
        [Test]
        public void ValidateFastQParserQualityScoreWithDna()
        {
            ValidateFastQParserDataVirtulization(
                Constants.SimpleIlluminaFastQNode,
                FastQFileParameters.FileName);
        }

        /// <summary>
        /// Parse a valid small size Dna FastQ file and identify the parser,
        /// Parse the file and validate if isc is created.
        /// Input : Dna FastQ file with Sanger format.
        /// Output : Validation of ISC file creation.
        /// </summary>
        [Test]
        public void ValidateFastQParserWithDVIscFile()
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
              Constants.SimpleFastQDVIscNodeName, Constants.FilePathNode);

            ISequenceParser iSeqParser =
                SequenceParsers.FindParserByFile(filePath);
            if (null != iSeqParser)
            {
                IVirtualSequenceParser vParserObj =
                    iSeqParser as IVirtualSequenceParser;
                if (null != vParserObj)
                {
                    vParserObj.EnforceDataVirtualization = true;
                }
                else
                {
                    Assert.Fail(
                        "FastQ Parser P1: Could not find the FastQ Parser Object.");
                }

                string iscFilePath = string.Concat(filePath, ".isc");

                iSeqParser.Parse(filePath);

                if (File.Exists(iscFilePath))
                {
                    Console.WriteLine(
                        "FastQ Parser P1: DV enabled as expected and isc file created successfully.");
                    ApplicationLog.WriteLine(
                        "FastQ Parser P1: DV enabled as expected and isc file created successfully.");
                }
                else
                {
                    Assert.Fail("FastQ Parser P1: DV not enabled as expected.");
                }
            }
            else
            {
                Assert.Fail("FastQ Parser P1: Could not find the FastQ file");
            }
        }

        #endregion FastQ P1 Test cases

        #region Supporting Methods

        /// <summary>
        /// General method to validate FastQ Parser.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="fastQProperty">FastQ Parser properties.</param>
        /// </summary>
        static void ValidateFastQParser(string nodeName,
          FastQPropertyParameters fastQProperty)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string expectedQualitativeSequence = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceId = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.SequenceIdNode);
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
              Utility._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            string expectedSeqCount = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.SeqsCount);
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
               nodeName, Constants.AlphabetNameNode));

            FastQParser fastQParserObj = null;

            switch (fastQProperty)
            {
                case FastQPropertyParameters.FastQConstructorPam:
                    fastQParserObj = new FastQParser(Encodings.IupacNA);
                    fastQParserObj.AutoDetectFastQFormat = true;
                    fastQParserObj.FastqType = expectedFormatType;
                    break;
                case FastQPropertyParameters.EncodingProperty:
                    fastQParserObj = new FastQParser();
                    fastQParserObj.AutoDetectFastQFormat = true;
                    fastQParserObj.FastqType = expectedFormatType;
                    fastQParserObj.Encoding = Encodings.IupacNA;
                    break;
                case FastQPropertyParameters.AlphabetProperty:
                    fastQParserObj = new FastQParser();
                    fastQParserObj.AutoDetectFastQFormat = true;
                    fastQParserObj.FastqType = expectedFormatType;
                    fastQParserObj.Alphabet = alphabet;
                    break;
                default:
                    fastQParserObj = new FastQParser();
                    fastQParserObj.AutoDetectFastQFormat = true;
                    fastQParserObj.FastqType = expectedFormatType;
                    break;
            }

            IList<IQualitativeSequence> qualSequenceList = null;

            qualSequenceList = fastQParserObj.Parse(filePath);

            // Validate qualitative Sequence upon parsing FastQ file.
            Assert.AreEqual(qualSequenceList.Count.ToString(), expectedSeqCount);
            Assert.AreEqual(qualSequenceList[0].ToString(), expectedQualitativeSequence);
            Assert.AreEqual(qualSequenceList[0].Type, expectedFormatType);
            Assert.AreEqual(qualSequenceList[0].ID.ToString(), expectedSequenceId);
            Assert.AreEqual(qualSequenceList[0].Alphabet, alphabet);

            ApplicationLog.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence '{0}' validation after Parse() is found to be as expected.",
              qualSequenceList[0]));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              qualSequenceList[0].ToString()));
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              qualSequenceList[0].ID.ToString()));
        }

        /// <summary>
        /// General method to validate FastQ Parser for Multiple sequence with 
        /// different alphabets.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="triSeq">Tri Sequence</param>
        /// </summary>
        static void ValidateMulitpleSequenceFastQParser(string nodeName, string triSeq)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string expectedFirstQualitativeSequence = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedSequence1Node);
            string expectedSecondQualitativeSequence = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedSequence2Node);
            string expectedthirdQualitativeSequence = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedSequence3Node);
            string expectedSequenceId = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.SequenceIdNode);
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
              Utility._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            string expectedSeqCount = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.SeqsCount);
            IAlphabet firstAlphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
              nodeName, Constants.AlphabetName1Node));
            IAlphabet secondAlphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
              nodeName, Constants.AlphabetName2Node));
            IAlphabet thirdAlphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
              nodeName, Constants.AlphabetName3Node));

            // Parse a multiple sequence FastQ file.
            FastQParser fastQParserObj = new FastQParser();
            fastQParserObj.AutoDetectFastQFormat = true;
            fastQParserObj.FastqType = expectedFormatType;

            IList<IQualitativeSequence> qualSequenceList = null;
            qualSequenceList = fastQParserObj.Parse(filePath);

            // Validate first qualitative Sequence upon parsing FastQ file.
            Assert.AreEqual(qualSequenceList.Count.ToString(), expectedSeqCount);
            Assert.AreEqual(qualSequenceList[0].ToString(),
              expectedFirstQualitativeSequence);
            Assert.AreEqual(qualSequenceList[0].Type, expectedFormatType);
            Assert.AreEqual(qualSequenceList[0].ID.ToString(), expectedSequenceId);
            Assert.AreEqual(qualSequenceList[0].Alphabet, firstAlphabet);

            // Validate second qualitative Sequence upon parsing FastQ file.
            Assert.AreEqual(qualSequenceList.Count.ToString(), expectedSeqCount);
            Assert.AreEqual(qualSequenceList[1].ToString(),
              expectedSecondQualitativeSequence);
            Assert.AreEqual(qualSequenceList[1].Type, expectedFormatType);
            Assert.AreEqual(qualSequenceList[1].ID.ToString(), expectedSequenceId);
            Assert.AreEqual(qualSequenceList[1].Alphabet, secondAlphabet);

            // Validate third sequence in FastQ file if it is tri sequence FastQ file.
            if (0 == string.Compare(triSeq, "MultiSequenceFastQ", true,
              CultureInfo.CurrentCulture))
            {
                // Validate second qualitative Sequence upon parsing FastQ file.
                Assert.AreEqual(qualSequenceList.Count.ToString(), expectedSeqCount);
                Assert.AreEqual(qualSequenceList[2].ToString(),
                  expectedthirdQualitativeSequence);
                Assert.AreEqual(qualSequenceList[2].Type, expectedFormatType);
                Assert.AreEqual(qualSequenceList[2].ID.ToString(), expectedSequenceId);
                Assert.AreEqual(qualSequenceList[2].Alphabet, thirdAlphabet);
                Console.WriteLine(string.Format(null,
                  "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                  qualSequenceList[2].ToString()));
                Console.WriteLine(string.Format(null,
                  "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                  qualSequenceList[2].ID.ToString()));
            }

            ApplicationLog.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence '{0}' validation after Parse() is found to be as expected.",
              qualSequenceList[0]));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              qualSequenceList[0].ToString()));
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              qualSequenceList[0].ID.ToString()));
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              qualSequenceList[1].ToString()));
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              qualSequenceList[1].ID.ToString()));
        }

        /// <summary>
        /// General method to validate FastQ FormatString by parsing FastQ file.
        /// <param name="nodeName">xml node name.</param>
        /// </summary>
        static void ValidateFastQFormatStringForamttingFastQFile(string nodeName)
        {
            // Gets the actual sequence and the alphabet from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string formatStrOrginal = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.FormatStringNode);

            // Parse a string 
            FastQParser parserObj = new FastQParser();
            IQualitativeSequence seq = parserObj.ParseOne(filePath);
            FastQFormatter formatter = new FastQFormatter();

            // Format a string and validate a string.
            string formattedString = formatter.FormatString(seq);
            string newFormattedStr =
              formattedString.Replace("\r", "").Replace("\n", "");

            // Validate a formatted string.
            Assert.AreEqual(formatStrOrginal, newFormattedStr);

            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              formattedString));
        }

        /// <summary>
        /// General method to validate FastQ FormatString by formatting 
        /// Qualitative Sequence.
        /// <param name="nodeName">xml node name.</param>
        /// </summary>
        static void ValidateFastQFormatStringByFormattingQualSeqeunce(string nodeName)
        {
            // Gets the actual sequence and the alphabet from the Xml
            string expectedString = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.SeqFormatString);
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
              nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
              Utility._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            string qualSequence = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedSequenceNode);

            // Create a Qualitative Sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(alphabet,
              expectedFormatType, qualSequence);
            FastQFormatter formatter = new FastQFormatter();

            // Format a string and validate a string.
            string formattedString = formatter.FormatString(qualSeq);
            string newFormattedStr = formattedString.Replace("\r", "").Replace("\n", "");

            // Validate a formatted string.
            Assert.AreEqual(expectedString, newFormattedStr);

            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              formattedString));
        }

        /// <summary>
        /// General method to validate FastQ formatting 
        /// Qualitative Sequence by passing TextWriter as a parameter
        /// <param name="nodeName">xml node name.</param>
        /// </summary>
        static void ValidateFastQFormatByFormattingQualSeqeunce(string nodeName)
        {
            // Gets the actual sequence and the alphabet from the Xml
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
              nodeName, Constants.AlphabetNameNode));
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
              Utility._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            string qualSequence = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedSequenceNode);
            string expectedQualitativeSequence = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedSequenceNode);

            // Create a Qualitative Sequence.
            QualitativeSequence qualSeq = new QualitativeSequence(
              alphabet, expectedFormatType, qualSequence);
            FastQFormatter formatter = new FastQFormatter();
            FastQParser fastQParserObj = new FastQParser();

            //Format Qualitative sequence using Text Writer.
            using (TextWriter txtWriter = new StreamWriter(
              Constants.FastQTempFileName))
            {
                formatter.Format(qualSeq, txtWriter);
            }

            // Read the new file and validate Sequences.
            IList<IQualitativeSequence> seqsNew =
              fastQParserObj.Parse(Constants.FastQTempFileName);

            // Validate qualitative Sequence upon parsing FastQ file.
            Assert.AreEqual(seqsNew[0].ToString(), expectedQualitativeSequence);
            Assert.IsEmpty(seqsNew[0].ID);

            ApplicationLog.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence '{0}' validation after Parse() is found to be as expected.",
              seqsNew[0]));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              seqsNew[0].ToString()));
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              seqsNew[0].ID.ToString()));

            File.Delete(Constants.FastQTempFileName);
        }

        /// <summary>
        /// General method to validate FastQ Formatter by Passing Writer as parameter.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="isTextWriter">FastQ formatter Format() method parameter</param>
        static void ValidateFastQFormatter(string nodeName, bool isTextWriter)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string expectedQualitativeSequence = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceId = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.SequenceIdNode);
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
              Utility._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));

            // Parse a FastQ file.
            FastQParser fastQParserObj = new FastQParser();
            fastQParserObj.AutoDetectFastQFormat = true;
            fastQParserObj.FastqType = expectedFormatType;

            IList<IQualitativeSequence> seqsNew = null;
            IList<IQualitativeSequence> qualSequenceList = null;
            qualSequenceList = fastQParserObj.Parse(filePath);
            FastQFormatter fastQFormatter = new FastQFormatter();

            // Format a Sequence using Text writer.
            if (isTextWriter)
            {
                using (TextWriter txtWriter = new StreamWriter(
                  Constants.FastQTempFileName))
                {
                    foreach (IQualitativeSequence newQualSeq in qualSequenceList)
                    {
                        fastQFormatter.Format(newQualSeq, txtWriter);
                    }
                }
                // Read the new file and validate Sequences.
                seqsNew = fastQParserObj.Parse(Constants.FastQTempFileName);
            }
            else
            {
                fastQFormatter.Format(qualSequenceList[0], Constants.FastQTempFileName);
                seqsNew = fastQParserObj.Parse(Constants.FastQTempFileName);
            }

            // Validate qualitative Sequence upon parsing FastQ file.
            Assert.AreEqual(seqsNew[0].ToString(), expectedQualitativeSequence);
            Assert.AreEqual(seqsNew[0].Type, expectedFormatType);
            Assert.AreEqual(seqsNew[0].ID.ToString(), expectedSequenceId);

            ApplicationLog.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence '{0}' validation after Parse() is found to be as expected.",
              seqsNew[0]));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              seqsNew[0].ToString()));
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              seqsNew[0].ID.ToString()));

            File.Delete(Constants.FastQTempFileName);
        }

        /// <summary>
        /// General method to validate multi sequence FastQ Format.
        /// <param name="nodeName">xml node name.</param>
        static void ValidateMultiSeqFastQFormatter(string nodeName)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string expectedQualitativeSequence = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceId = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.SequenceIdNode);
            string expectedSecondQualitativeSequence = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedSequence2Node);
            string expectedSecondSeqID = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ExpectedSequenceId1Node);
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
              Utility._xmlUtil.GetTextValue(nodeName,
              Constants.FastQFormatType));

            // Parse a FastQ file.
            FastQParser fastQParserObj = new FastQParser();
            fastQParserObj.AutoDetectFastQFormat = true;
            fastQParserObj.FastqType = expectedFormatType;

            IList<IQualitativeSequence> seqsNew = null;
            IList<IQualitativeSequence> qualSequenceList = null;
            qualSequenceList = fastQParserObj.Parse(filePath);
            FastQFormatter fastQFormatter = new FastQFormatter();

            // Format a first Qualitative sequence
            fastQFormatter.Format(qualSequenceList[0],
              Constants.FastQTempFileName);
            seqsNew = fastQParserObj.Parse(Constants.FastQTempFileName);

            // Format a Second Qualitative sequence
            fastQFormatter.Format(qualSequenceList[1],
              Constants.StreamWriterFastQTempFileName);
            IList<IQualitativeSequence> secondSeqsNew =
              fastQParserObj.Parse(Constants.StreamWriterFastQTempFileName);

            // Validate Second qualitative Sequence upon parsing FastQ file.
            Assert.AreEqual(secondSeqsNew[0].ToString(), expectedSecondQualitativeSequence);
            Assert.AreEqual(secondSeqsNew[0].Type, expectedFormatType);
            Assert.AreEqual(secondSeqsNew[0].ID.ToString(), expectedSecondSeqID);

            // Validate first qualitative Sequence upon parsing FastQ file.
            Assert.AreEqual(seqsNew[0].ToString(), expectedQualitativeSequence);
            Assert.AreEqual(seqsNew[0].Type, expectedFormatType);
            Assert.AreEqual(seqsNew[0].ID.ToString(), expectedSequenceId);

            ApplicationLog.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence '{0}' validation after Parse() is found to be as expected.",
              seqsNew[0]));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              seqsNew[0].ToString()));
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
              seqsNew[0].ID.ToString()));

            File.Delete(Constants.FastQTempFileName);
            File.Delete(Constants.StreamWriterFastQTempFileName);
        }

        /// <summary>
        /// General method to validate FastQ FormatString by parsing FastQ file
        /// for Multi Sequence FastQ files..
        /// <param name="nodeName">xml node name.</param>
        /// <param name="isTriSeq">Is Tri Sequence?</param>
        /// </summary>
        static void ValidateMultiSeqFastQFormatStringForamttingFastQFile(string nodeName,
            bool isTriSeq)
        {
            // Gets the actual sequence and the alphabet from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string formatStrOrginal = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.SeqFormatString);
            string formatSecondString = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.SecondFormatString);
            string formatThridString = Utility._xmlUtil.GetTextValue(
              nodeName, Constants.ThirdFormatString);
            string formattedFirst = formatStrOrginal.Replace("\\\\", "\\");
            string formattedSecondString = formatSecondString.Replace("\\\\", "\\");
            string formattedThirdExpString = formatThridString.Replace("\\\\", "\\");

            // Parse a string 
            FastQParser parserObj = new FastQParser();
            IList<IQualitativeSequence> seq = parserObj.Parse(filePath);
            FastQFormatter formatter = new FastQFormatter();

            // Format a first Seq string and validate the same.
            string formattedString = formatter.FormatString(seq[0]);
            string firstFormattedStr =
              formattedString.Replace("\r", "").Replace("\n", "");

            // Format a second Seq string and validate the same.
            string formatedSecondString = formatter.FormatString(seq[1]);
            string secondFormattedStr =
              formatedSecondString.Replace("\r", "").Replace("\n", "");

            if (isTriSeq)
            {
                // Format a third Seq string and validate the same.
                string formattedThirdString = formatter.FormatString(seq[2]);
                string thirdFormattedStr =
                  formattedThirdString.Replace("\r", "").Replace("\n", "");
                Assert.AreEqual(thirdFormattedStr, formattedThirdExpString);
            }

            // Validate a formatted strings for different sequences.
            Assert.AreEqual(firstFormattedStr, formattedFirst);
            Assert.AreEqual(secondFormattedStr, formattedSecondString);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
              "FastQ Parser P1: The Second sequence string is as expected {0}.",
              secondFormattedStr));
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
                "FastQ Parser : File Exists in the Path '{0}'.", filePath));

            IList<IQualitativeSequence> seqs = null;
            FastQParser parserObj = new FastQParser();
            parserObj.EnforceDataVirtualization = true;

            seqs = parserObj.Parse(filePath, false);

            // Gets the expected count from the Xml
            int expectedCount = int.Parse(Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceCountNode));

            Assert.AreEqual(expectedCount, seqs.Count);
            ApplicationLog.WriteLine(
                "FastQ Parser: Sequence count is as expected.");
            Console.WriteLine(
                "FastQ Parser: Sequence count is as expected.");

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
                "FastQ Parser: Sequences are as expected.");
            Console.WriteLine(
                "FastQ Parser: Sequences are as expected.");

            Assert.AreEqual(expectedSequence.Length, strBuildObj.ToString().Length);
            ApplicationLog.WriteLine(string.Format(null,
                "FastQ Parser: Sequence Length is '{0}' and is as expected.",
                expectedSequence.Length));

            Assert.IsNotNull(seqs[0].Alphabet);
            Assert.AreEqual(seqs[0].Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture));
            ApplicationLog.WriteLine(string.Format(null,
                "FastQ Parser: The Sequence Alphabet is '{0}' and is as expected.",
                seqs[0].Alphabet.Name));

            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode), seqs[0].ID);
            ApplicationLog.WriteLine(string.Format(null,
                "FastQ Parser: Sequence ID is '{0}' and is as expected.",
                seqs[0].ID));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "FastQ Parser: Sequence ID is '{0}' and is as expected.",
                seqs[0].ID));
        }

        /// <summary>
        /// General method to validate FastQ Parser Data Virtulization
        /// and Quality Score.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">Name of Method</param>
        /// </summary>
        static void ValidateFastQParserDataVirtulization(
            string nodeName,
            FastQFileParameters methodName)
        {
            string filepathOriginal =
                Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedQualitativeSequence =
                Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceId =
                Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode);
            Assert.IsTrue(File.Exists(filepathOriginal));

            FastQParser fastQParserObj = new FastQParser();
            FastQFormatter formatter = new FastQFormatter();

            // Read the original file
            IList<IQualitativeSequence> seqsOriginal = null;

            switch (methodName)
            {
                case FastQFileParameters.FileName:
                    fastQParserObj.EnforceDataVirtualization = true;
                    seqsOriginal = fastQParserObj.Parse(filepathOriginal);
                    break;
                case FastQFileParameters.FileNameReadOnly:
                    fastQParserObj.EnforceDataVirtualization = false;
                    seqsOriginal = fastQParserObj.Parse(filepathOriginal, true);
                    break;
                default:
                    break;
            }

            Assert.IsNotNull(seqsOriginal);

            // Use the formatter to write the original sequences to a temp file
            string filepathTmp = Path.GetTempFileName();

            using (TextWriter writer = new StreamWriter(filepathTmp))
            {
                foreach (IQualitativeSequence qualSeq in seqsOriginal)
                {
                    formatter.Format(qualSeq, writer);
                }
            }

            // Read the new file, then compare the sequences
            IList<IQualitativeSequence> seqsNew = null;
            seqsNew = fastQParserObj.Parse(filepathTmp);
            Assert.IsNotNull(seqsNew);

            Assert.AreEqual(seqsOriginal[0].ID, expectedSequenceId);
            Assert.AreEqual(
                seqsOriginal[0].ToString(),
                expectedQualitativeSequence);
            Assert.AreEqual(
                ASCIIEncoding.ASCII.GetString(seqsOriginal[0].Scores),
                ASCIIEncoding.ASCII.GetString(seqsNew[0].Scores));

            // Dispose FastQ Parser object
            fastQParserObj.Dispose();
            File.Delete(filepathTmp);
        }

        /// <summary>
        /// General method to validate BasicSequence Parser.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">Name of Method</param>
        /// </summary>
        static void ValidateBasicSequenceParser(
            string nodeName,
            FastQFileParameters methodName)
        {
            // Gets the expected sequence from the Xml
            string filepathOriginal =
                Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedQualitativeSequence =
                Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceId =
                Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode);
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
               nodeName, Constants.AlphabetNameNode));
            Assert.IsTrue(File.Exists(filepathOriginal));

            ISequenceParser fastQParserObj = new FastQParser();
            FastQFormatter formatter = new FastQFormatter();

            // Read the original file
            ISequence seqsOriginal = null;

            switch (methodName)
            {
                case FastQFileParameters.ParseFileName:
                    seqsOriginal = fastQParserObj.ParseOne(filepathOriginal);
                    break;
                default:
                    break;
            }

            Assert.IsNotNull(seqsOriginal);

            // Use the formatter to write the original sequences to a temp file
            string filepathTmp = Path.GetTempFileName();

            using (TextWriter writer = new StreamWriter(filepathTmp))
            {
                formatter.Format(seqsOriginal, writer);
            }

            // Read the new file, then compare the sequences
            ISequence seqsNew = null;
            seqsNew = fastQParserObj.ParseOne(filepathTmp);
            Assert.IsNotNull(seqsNew);

            // Validate qualitative Sequence upon parsing FastQ file.
            Assert.AreEqual(
                seqsOriginal.ToString(),
                expectedQualitativeSequence);
            Assert.AreEqual(
                seqsOriginal.ID.ToString(),
                expectedSequenceId);
            Assert.AreEqual(
                seqsOriginal.Alphabet.Name,
                alphabet.Name);

            ApplicationLog.WriteLine(string.Format(null,
                "FastQ Parser P1: The FASTQ sequence '{0}' validation after Parse() is found to be as expected.",
                seqsOriginal[0]));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "FastQ Parser P1: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                seqsOriginal[0].ToString()));

            File.Delete(filepathTmp);
        }

        #endregion Supporting Methods
    }
}
