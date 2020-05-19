// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * FastQBvtTestCases.cs
 * 
 *This file contains FastQ Parsers and Formatters Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MBF.IO;
using MBF.IO.FastQ;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.IO.FastQ
{
    /// <summary>
    /// FASTQ Bvt parser and formatter Test cases implementation.
    /// </summary>
    [TestClass]
    public class FastQBvtTestCases
    {

        #region Enums

        /// <summary>
        /// FastQ Formatter Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum FastQFileParameters
        {
            FastQ,
            Fq,
            TextWriter,
            TextWriterList,
            FormatFileName,
            TextReader,
            TextReaderReadOnly,
            FileName,
            FileNameReadOnly
        };

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\FastQTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static FastQBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region FastQ Parser & Formatter Bvt Test cases

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParserWithIlluminaUsingFastQFile()
        {
            ValidateFastQParser(Constants.SimpleIlluminaFastQNode);
        }

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : FastQ fq file extension with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParserWithIlluminaUsingFastQFqFile()
        {
            ValidateFastQParser(Constants.SimpleIlluminaFqFastQNode);
        }

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : FastQ fq file extension with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParserWithSolexaUsingFastQFqFile()
        {
            ValidateFastQParser(Constants.SimpleSolexaFqFastQNode);
        }

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParserWithSangerUsingFastQFile()
        {
            ValidateFastQParser(Constants.SimpleSangerFastQNode);
        }

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using ParseOne(file-name) method and validate with the 
        /// expected sequence.
        /// Input : FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneWithSangerUsingFastQFile()
        {
            ValidateFastQParseOne(Constants.SingleSequenceSangerFastQNode);
        }

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using ParseOne(file-name) method and validate with the 
        /// expected sequence.
        /// Input : FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneWithIlluminaUsingFastQFile()
        {
            ValidateFastQParseOne(Constants.SingleSequenceIlluminaFastQNode);
        }

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using ParseOne(file-name) method and validate with the 
        /// expected sequence.
        /// Input : FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneWithSolexaUsingFastQFile()
        {
            ValidateFastQParseOne(Constants.SingleSequenceSolexaFastQNode);
        }

        /// <summary>
        /// Format a valid small size Sequence to FastQ file, Parse a temporary file and 
        /// convert the same to sequence using ParseOne(file-name) method and 
        /// validate with the expected sequence.
        /// Input : FastQ file with Sanger format.
        /// Output : Validation of formatting sequence to temporary FastQ file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFastQFileFormat()
        {
            ValidateFastQFormatter(Constants.SimpleSangerFastQNode,
                FastQFileParameters.FastQ);
        }

        /// <summary>
        /// Format a valid small size to FastQ file with Fq extension, Parse a temporary 
        /// file and convert the same to sequence using ParseOne(file-name) method and 
        /// validate with the expected sequence.
        /// Input : FastQ file with Sanger format.
        /// Output : Validation of formatting sequence to temporary FastQ Fq file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFastQFqFileFormat()
        {
            ValidateFastQFormatter(Constants.SimpleSangerFastQNode,
                FastQFileParameters.Fq);
        }

        /// <summary>
        /// Format a valid small size sequence to FastQ file using TextWriter,
        /// Parse a temporary file and convert the same to sequence using Parse(file-name)
        /// method and validate with the expected sequence.
        /// Input : FastQ file with Sanger format.
        /// Output : Validation of formatting sequence to temporary FastQ file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterWithTextWriter()
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleSangerFastQNode, Constants.FilePathNode);
            string expectedQualitativeSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleSangerFastQNode, Constants.ExpectedSequenceNode);
            string expectedSequenceId = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleSangerFastQNode, Constants.SequenceIdNode);
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(Constants.SimpleSangerFastQNode,
                Constants.FastQFormatType));
            string expectedSeqCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleSangerFastQNode, Constants.SeqsCount);

            // Parse a FastQ file.
            using (FastQParser fastQParserObj = new FastQParser())
            {
                fastQParserObj.AutoDetectFastQFormat = false;
                fastQParserObj.FastqType = expectedFormatType;

                IList<IQualitativeSequence> qualSequenceList = null;
                qualSequenceList = fastQParserObj.Parse(filePath);

                // Format a Sequence using Text writer.
                FastQFormatter fastQFormatter = new FastQFormatter();
                using (TextWriter txtWriter = new StreamWriter(
                Constants.FastQTempFileName))
                {
                    foreach (IQualitativeSequence newQualSeq in qualSequenceList)
                    {
                        fastQFormatter.Format(newQualSeq, txtWriter);
                    }
                }

                // Read the new file and validate Sequences.
                IList<IQualitativeSequence> seqsNew = null;
                seqsNew = fastQParserObj.Parse(Constants.FastQTempFileName);

                // Validate qualittative Sequence upon parsing FastQ file.
                Assert.AreEqual(seqsNew.Count.ToString((IFormatProvider)null), expectedSeqCount);
                Assert.AreEqual(seqsNew[0].ToString(), expectedQualitativeSequence);
                Assert.AreEqual(seqsNew[0].Type, expectedFormatType);
                Assert.AreEqual(seqsNew[0].ID.ToString((IFormatProvider)null), expectedSequenceId);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence '{0}' validation after Parse() is found to be as expected.",
                    qualSequenceList[0]));

                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                    qualSequenceList[0].ToString()));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                    qualSequenceList[0].ID.ToString((IFormatProvider)null)));
            }
            File.Delete(Constants.FastQTempFileName);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file using FormatString() method and validate the same.
        /// Input : Sanger format FastQ Sequence
        /// Validation : Validate the output of FormatString() method with the expected sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFormatStringForSanger()
        {
            ValidateFastQFormatString(Constants.SimpleSangerFastQNode);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file FormatString() method and validate the same.
        /// Input : Solexa format FastQ Sequence
        /// Validation : Validate the output of FormatString() method with the expected sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFormatStringForSolexa()
        {
            ValidateFastQFormatString(Constants.SimpleSolexaFqFastQNode);
        }

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using Parse(file-name, isReadOnly) method and validate with the 
        /// expected sequence.
        /// Input : FastQ fq file extension with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParserReadOnlyWithIlluminaUsingFastQFqFile()
        {
            ValidateFastQParser(Constants.SimpleIlluminaFqFastQNode,
                FastQFileParameters.FileNameReadOnly);
        }

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using Parse(file-name, isReadOnly) method and validate with the 
        /// expected sequence.
        /// Input : FastQ fq file extension with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParserReadOnlyWithSolexaUsingFastQFqFile()
        {
            ValidateFastQParser(Constants.SimpleSolexaFqFastQNode,
                FastQFileParameters.FileNameReadOnly);
        }

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using Parse(file-name, isReadOnly) method and validate with the 
        /// expected sequence.
        /// Input : FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParserReadOnlyWithSangerUsingFastQFile()
        {
            ValidateFastQParser(Constants.SimpleSangerFastQNode,
                FastQFileParameters.TextReaderReadOnly);
        }

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using ParseOne(file-name, isReadOnly) method and validate with the 
        /// expected sequence.
        /// Input : FastQ file with Sanger format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneReadOnlyWithSangerUsingFastQFile()
        {
            ValidateFastQParseOne(Constants.SingleSequenceSangerFastQNode,
                FastQFileParameters.FileNameReadOnly);
        }

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using ParseOne(file-name, isReadOnly) method and validate with the 
        /// expected sequence.
        /// Input : FastQ file with Illumina format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneReadOnlyWithIlluminaUsingFastQFile()
        {
            ValidateFastQParseOne(Constants.SingleSequenceIlluminaFastQNode,
                FastQFileParameters.FileNameReadOnly);
        }

        /// <summary>
        /// Parse a valid small size FastQ file and convert the same to 
        /// sequence using ParseOne(file-name, isReadOnly) method and validate with the 
        /// expected sequence.
        /// Input : FastQ file with Solexa format.
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneReadOnlyWithSolexaUsingFastQFile()
        {
            ValidateFastQParseOne(Constants.SingleSequenceSolexaFastQNode,
                FastQFileParameters.TextReaderReadOnly);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file ParseOne(reader, isReadOnly) method and validate the same.
        /// Input : Solexa format FastQ Sequence
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneForSolexa()
        {
            ValidateFastQParseOne(Constants.SimpleSolexaFqFastQNode,
                FastQFileParameters.TextReaderReadOnly);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file ParseOne(reader) method and validate the same.
        /// Input : Solexa format FastQ Sequence
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneReadOnlyForSolexa()
        {
            ValidateFastQParseOne(Constants.SimpleSolexaFqFastQNode,
                FastQFileParameters.TextReader);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file ParseOne(reader, isReadOnly) method and validate the same.
        /// Input : Sanger format FastQ Sequence
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneForSanger()
        {
            ValidateFastQParseOne(Constants.SimpleSangerFastQNode,
                FastQFileParameters.TextReaderReadOnly);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file ParseOne(reader) method and validate the same.
        /// Input : Sanger format FastQ Sequence
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneReadOnlyForSanger()
        {
            ValidateFastQParseOne(Constants.SimpleSangerFastQNode,
                FastQFileParameters.TextReader);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file ParseOne(reader, isReadOnly) method and validate the same.
        /// Input : Illumina format FastQ Sequence
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneForIllumina()
        {
            ValidateFastQParseOne(Constants.SimpleIlluminaFqFastQNode,
                FastQFileParameters.TextReaderReadOnly);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file ParseOne(reader) method and validate the same.
        /// Input : Illumina format FastQ Sequence
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneReadOnlyForIllumina()
        {
            ValidateFastQParseOne(Constants.SimpleSangerFastQNode,
                FastQFileParameters.TextReader);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file Parse(reader, isReadOnly) method and validate the same.
        /// Input : Solexa format FastQ Sequence
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseForSolexa()
        {
            ValidateFastQParser(Constants.SimpleSolexaFqFastQNode,
                FastQFileParameters.TextReaderReadOnly);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file Parse(reader) method and validate the same.
        /// Input : Solexa format FastQ Sequence
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseReadOnlyForSolexa()
        {
            ValidateFastQParser(Constants.SimpleSolexaFqFastQNode,
                FastQFileParameters.TextReader);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file Parse(reader, isReadOnly) method and validate the same.
        /// Input : Sanger format FastQ Sequence
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseForSanger()
        {
            ValidateFastQParser(Constants.SimpleSangerFastQNode,
                FastQFileParameters.TextReaderReadOnly);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file Parse(reader) method and validate the same.
        /// Input : Sanger format FastQ Sequence
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseReadOnlyForSanger()
        {
            ValidateFastQParser(Constants.SimpleSangerFastQNode,
                FastQFileParameters.TextReader);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to
        /// FastQ file Parse(reader, isReadOnly) method and validate the same.
        /// Input : Illumina format FastQ Sequence
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseForIllumina()
        {
            ValidateFastQParser(Constants.SimpleIlluminaFqFastQNode,
                FastQFileParameters.TextReaderReadOnly);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file Parse(reader) method and validate the same.
        /// Input : Illumina format FastQ Sequence
        /// Output : Validation of Expected sequence, Sequence Id,Sequence Type.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseReadOnlyForIllumina()
        {
            ValidateFastQParser(Constants.SimpleIlluminaFqFastQNode,
                FastQFileParameters.TextReader);
        }

        /// <summary>
        /// Format a valid Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file GetQualityScores(qualScoresStartingIndex) method and 
        /// validate the same.
        /// Input : Valid FastQ file
        /// Output : Validation of Expected sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseQualityScore()
        {
            ValidateDataVirtulization(
                Constants.FastQDataVirtulizationMultiSequenceFileNode,
                FastQFileParameters.FileName);
        }

        /// <summary>
        /// Format a valid Sequence (Small size sequence less than 35 kb) to a 
        /// FastQ file Parse(textReader,isReadOnly) method and validate the same.
        /// Input : Valid FastQ file
        /// Output : Validation of Expected sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseDataVirtulizationParseFileName()
        {
            ValidateDataVirtulization(
                Constants.FastQDataVirtulizationMultiSequenceFileNode,
                FastQFileParameters.FileNameReadOnly);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb)
        /// and validate the Properties
        /// Input : Valide FastQ file
        /// Output : Validatation of properties
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParserProperties()
        {
            using (FastQParser parser = new FastQParser())
            {
                Assert.AreEqual(parser.EnforceDataVirtualization, false);
                Assert.AreEqual(parser.IsDataVirtualizationEnabled, false);
            }
        }

        /// <summary>
        /// Format a valid Single Sequence and validate the 
        /// BasicSequenceParser.ParseOne() method
        /// Input : Valide FastQ file
        /// Output : Validate ParseOne method
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFastQParseOneWithSpecificFormatUsingFastQFile()
        {
            // Gets the expected sequence from the Xml
            string filePath =
                _utilityObj._xmlUtil.GetTextValue(
                Constants.SingleSequenceSangerFastQNode,
                Constants.FilePathNode);
            string expectedQualitativeSequence =
                _utilityObj._xmlUtil.GetTextValue(
                Constants.SingleSequenceSangerFastQNode,
                Constants.ExpectedSequenceNode);
            string expectedSequenceId =
                _utilityObj._xmlUtil.GetTextValue(
                Constants.SingleSequenceSangerFastQNode,
                Constants.SequenceIdNode);
            string expectedSeqCount =
                _utilityObj._xmlUtil.GetTextValue(
                Constants.SingleSequenceSangerFastQNode,
                Constants.SeqLength);

            // Parse a FastQ file using parseOne method.
            ISequenceParser fastQParserObj = new FastQParser();
            try
            {
                ISequence qualSequence = null;
                qualSequence = fastQParserObj.ParseOne(filePath);

                // Validate qualittative Sequence upon parsing FastQ file.
                Assert.AreEqual(qualSequence.Count.ToString((IFormatProvider)null), expectedSeqCount);
                Assert.AreEqual(qualSequence.ToString(), expectedQualitativeSequence);
                Assert.AreEqual(qualSequence.ID.ToString((IFormatProvider)null), expectedSequenceId);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence '{0}' validation after ParseOne() is found to be as expected.",
                    qualSequence));

                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after ParseOne() is found to be as expected.",
                    qualSequence.ToString()));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after ParseOne() is found to be as expected.",
                    qualSequence.ID.ToString((IFormatProvider)null)));
            }
            finally
            {
                (fastQParserObj as FastQParser).Dispose();
            }
        }


        /// <summary>
        /// Format(Sequence, Writer) a valid small size Sequence to FastQ file,
        /// Parse a temporary file and convert the same to sequence using
        /// ParseOne(file-name) method and validate with the expected sequence.
        /// Input : FastQ file with Sanger format.
        /// Output : Validation of formatting sequence to temporary FastQ file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFastQFileFormatSeqWriterSanger()
        {
            ValidateFastQFormatter(Constants.SimpleSangerFastQNode,
                FastQFileParameters.TextWriter);
        }

        /// <summary>
        /// Format(Sequence, Writer) a valid small size Sequence to FastQ file,
        /// Parse a temporary file and convert the same to sequence using
        /// ParseOne(file-name) method and validate with the expected sequence.
        /// Input : FastQ file with Solexa format.
        /// Output : Validation of formatting sequence to temporary FastQ file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFastQFileFormatSeqWriterSolexa()
        {
            ValidateFastQFormatter(Constants.SimpleSolexaFqFastQNode,
                FastQFileParameters.TextWriter);
        }

        /// <summary>
        /// Format(Sequence, Writer) a valid small size Sequence to FastQ file,
        /// Parse a temporary file and convert the same to sequence using
        /// ParseOne(file-name) method and validate with the expected sequence.
        /// Input : FastQ file with Illumina format.
        /// Output : Validation of formatting sequence to temporary FastQ file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFastQFileFormatSeqWriterIllumina()
        {
            ValidateFastQFormatter(Constants.SimpleIlluminaFqFastQNode,
                FastQFileParameters.TextWriter);
        }

        /// <summary>
        /// Format(collection_of_Sequence, Writer) a valid small size Sequence
        /// to FastQ file, Parse a temporary file and convert the same to
        /// sequence using ParseOne(file-name) method and validate with the
        /// expected sequence.
        /// Input : FastQ file with Sanger format.
        /// Output : Validation of formatting sequence to temporary FastQ file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFastQFileFormatCollectWriterSanger()
        {
            ValidateFastQFormatter(Constants.SimpleSangerFastQNode,
                FastQFileParameters.TextWriterList);
        }

        /// <summary>
        /// Format(collection_of_Sequence, Writer) a valid small size Sequence
        /// to FastQ file, Parse a temporary file and convert the same to
        /// sequence using ParseOne(file-name) method and validate with the
        /// expected sequence.
        /// Input : FastQ file with Solexa format.
        /// Output : Validation of formatting sequence to temporary FastQ file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFastQFileFormatCollectWriterSolexa()
        {
            ValidateFastQFormatter(Constants.SimpleSolexaFqFastQNode,
                FastQFileParameters.TextWriterList);
        }

        /// <summary>
        /// Format(collection_of_Sequence, Writer) a valid small size Sequence
        /// to FastQ file, Parse a temporary file and onvert the same to
        /// sequence using ParseOne(file-name) method and validate with the
        /// expected sequence.
        /// Input : FastQ file with Illumina format.
        /// Output : Validation of formatting sequence to temporary FastQ file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFastQFileFormatCollectWriterIllumina()
        {
            ValidateFastQFormatter(Constants.SimpleIlluminaFqFastQNode,
                FastQFileParameters.TextWriterList);
        }

        /// <summary>
        /// Format(collection_of_Sequence, file-name) a valid small size Sequence
        /// to FastQ file, Parse a temporary file and convert the same to sequence
        /// using ParseOne(file-name) method and validate with the expected sequence.
        /// Input : FastQ file with Sanger format.
        /// Output : Validation of formatting sequence to temporary FastQ file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFastQFileFormatCollectFileNameSanger()
        {
            ValidateFastQFormatter(Constants.SimpleSangerFastQNode,
                FastQFileParameters.FormatFileName);
        }

        /// <summary>
        /// Format(collection_of_Sequence, file-name) a valid small size Sequence to
        /// FastQ file, Parse a temporary file and convert the same to sequence 
        /// using ParseOne(file-name) method and validate with the expected sequence.
        /// Input : FastQ file with Solexa format.
        /// Output : Validation of formatting sequence to temporary FastQ file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFastQFileFormatCollectFileNameSolexa()
        {
            ValidateFastQFormatter(Constants.SimpleSolexaFqFastQNode,
                FastQFileParameters.FormatFileName);
        }

        /// <summary>
        /// Format(collection_of_Sequence, file-name) a valid small size Sequence to
        /// FastQ file, Parse a temporary file and convert the same to sequence using
        /// ParseOne(file-name) method and validate with the expected sequence.
        /// Input : FastQ file with Illumina format.
        /// Output : Validation of formatting sequence to temporary FastQ file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FastQFormatterValidateFastQFileFormatCollectFileNameIllumina()
        {
            ValidateFastQFormatter(Constants.SimpleIlluminaFqFastQNode,
                FastQFileParameters.FormatFileName);
        }

        #endregion FastQ Parser & Formatter Bvt Test cases

        #region Supporting Methods

        /// <summary>
        /// General method to validate FastQ Parser.
        /// <param name="nodeName">xml node name.</param>
        /// </summary>
        void ValidateFastQParser(string nodeName)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedQualitativeSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceId = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode);
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            string expectedSeqCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SeqsCount);

            // Parse a FastQ file.
            using (FastQParser fastQParserObj = new FastQParser())
            {
                fastQParserObj.AutoDetectFastQFormat = false;
                fastQParserObj.FastqType = expectedFormatType;

                IList<IQualitativeSequence> qualSequenceList = null;
                qualSequenceList = fastQParserObj.Parse(filePath);

                // Validate qualittative Sequence upon parsing FastQ file.
                Assert.AreEqual(qualSequenceList.Count.ToString((IFormatProvider)null), expectedSeqCount);
                Assert.AreEqual(qualSequenceList[0].ToString(), expectedQualitativeSequence);
                Assert.AreEqual(qualSequenceList[0].Type, expectedFormatType);
                Assert.AreEqual(qualSequenceList[0].ID.ToString((IFormatProvider)null), expectedSequenceId);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence '{0}' validation after Parse() is found to be as expected.",
                    qualSequenceList[0]));

                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                    qualSequenceList[0].ToString()));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                    qualSequenceList[0].ID.ToString((IFormatProvider)null)));
            }
        }

        /// <summary>
        /// General method to validate FastQ ParseOne.
        /// <param name="nodeName">xml node name.</param>
        /// </summary>
        void ValidateFastQParseOne(string nodeName)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedQualitativeSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceId = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode);
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            string expectedSeqCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SeqLength);

            // Parse a FastQ file using parseOne method.
            using (FastQParser fastQParserObj = new FastQParser())
            {
                fastQParserObj.AutoDetectFastQFormat = false;
                fastQParserObj.FastqType = expectedFormatType;

                IQualitativeSequence qualSequence = null;
                qualSequence = fastQParserObj.ParseOne(filePath);

                // Validate qualittative Sequence upon parsing FastQ file.
                Assert.AreEqual(qualSequence.Count.ToString((IFormatProvider)null), expectedSeqCount);
                Assert.AreEqual(qualSequence.ToString(), expectedQualitativeSequence);
                Assert.AreEqual(qualSequence.Type, expectedFormatType);
                Assert.AreEqual(qualSequence.ID.ToString((IFormatProvider)null), expectedSequenceId);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence '{0}' validation after Parse() is found to be as expected.",
                    qualSequence));

                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                    qualSequence.ToString()));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                    qualSequence.ID.ToString((IFormatProvider)null)));
            }
        }

        /// <summary>
        /// General method to validate FastQ Formatter.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="fileExtension">Different temporary file extensions</param>
        /// </summary>
        void ValidateFastQFormatter(string nodeName,
            FastQFileParameters fileExtension)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedQualitativeSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceId = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode);
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));

            // Parse a FastQ file using parseOne method.
            using (FastQParser fastQParserObj = new FastQParser())
            {
                fastQParserObj.AutoDetectFastQFormat = false;
                fastQParserObj.FastqType = expectedFormatType;

                IList<IQualitativeSequence> qualSequenceList = new List<IQualitativeSequence>();
                IQualitativeSequence qualSequence = null;
                qualSequence = fastQParserObj.ParseOne(filePath);
                qualSequenceList.Add(qualSequence);

                // New Sequence after formatting file.
                IQualitativeSequence newQualSeq = null;

                FastQFormatter fastQFormatter = new FastQFormatter();
                BasicSequenceFormatter fastQAbsParser = new FastQFormatter();

                // Format Parsed Sequence to temp file with different extension.
                switch (fileExtension)
                {
                    case FastQFileParameters.FastQ:
                        fastQFormatter.Format(qualSequence,
                            Constants.FastQTempFileName);
                        newQualSeq = fastQParserObj.ParseOne(
                            Constants.FastQTempFileName);
                        break;
                    case FastQFileParameters.Fq:
                        fastQFormatter.Format(qualSequence,
                            Constants.FastQTempFqFileName);
                        newQualSeq = fastQParserObj.ParseOne(
                            Constants.FastQTempFqFileName);
                        break;
                    case FastQFileParameters.FormatFileName:
                        fastQFormatter.Format(qualSequenceList,
                            Constants.StreamWriterFastQTempFileName);
                        using (TextReader txtReader = new StreamReader(
                            Constants.StreamWriterFastQTempFileName))
                        {
                            newQualSeq = fastQParserObj.ParseOne(txtReader, true);
                        }
                        break;
                    case FastQFileParameters.TextWriter:
                        using (TextWriter txtWriter = new StreamWriter(
                            Constants.StreamWriterFastQTempFileName))
                        {
                            fastQAbsParser.Format(qualSequence, txtWriter);
                        }
                        newQualSeq = fastQParserObj.ParseOne(
                            Constants.StreamWriterFastQTempFileName);
                        break;
                    case FastQFileParameters.TextWriterList:
                        using (TextWriter txtWriter = new StreamWriter(
                            Constants.StreamWriterFastQTempFileName))
                        {
                            fastQFormatter.Format(qualSequenceList, txtWriter);
                        }
                        newQualSeq = fastQParserObj.ParseOne(
                            Constants.StreamWriterFastQTempFileName);
                        break;
                    default:
                        break;
                }

                // Validate qualittative parsing temporary file.
                Assert.AreEqual(newQualSeq.ToString(), expectedQualitativeSequence);
                Assert.AreEqual(newQualSeq.Type, expectedFormatType);
                Assert.AreEqual(newQualSeq.ID.ToString((IFormatProvider)null), expectedSequenceId);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Formatter BVT: The FASTQ sequence '{0}' validation after Format() and Parse() is found to be as expected.",
                    newQualSeq));

                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Formatter BVT: The FASTQ sequence '{0}' validation after Format() and Parse() is found to be as expected.",
                    newQualSeq.ToString()));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Formatter BVT: The FASTQ sequence '{0}' validation after Format() and Parse() is found to be as expected.",
                    newQualSeq.ID.ToString((IFormatProvider)null)));
                File.Delete(Constants.FastQTempTxtFileName);
                File.Delete(Constants.FastQTempFqFileName);
                File.Delete(Constants.FastQTempFileName);
                File.Delete(Constants.StreamWriterFastQTempFileName);
            }
        }

        /// <summary>
        /// General method to validate FastQ FormatString.
        /// <param name="nodeName">xml node name.</param>
        /// </summary>
        void ValidateFastQFormatString(string nodeName)
        {
            // Gets the actual sequence and the alphabet from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string formatStrOrginal = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FormatStringNode);

            // Parse a string 
            using (FastQParser parser = new FastQParser())
            {
                IQualitativeSequence seq = parser.ParseOne(filePath);
                FastQFormatter formatter = new FastQFormatter();

                // Format a string and validate a string.
                string formattedString = formatter.FormatString(seq);
                string newFormattedStr = formattedString.Replace("\r", "").Replace("\n", "");

                // Validate a formatted string.
                Assert.AreEqual(formatStrOrginal, newFormattedStr);

                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                    formattedString));
            }
        }

        /// <summary>
        /// General method to validate FastQ Parser.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">Name of Method</param>
        /// </summary>
        void ValidateFastQParser(string nodeName,
            FastQFileParameters methodName)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedQualitativeSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceId = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode);
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            string expectedSeqCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SeqsCount);

            // Parse a FastQ file.
            using (FastQParser fastQParserObj = new FastQParser())
            {
                fastQParserObj.AutoDetectFastQFormat = false;
                fastQParserObj.FastqType = expectedFormatType;

                IList<IQualitativeSequence> newqualSequence = null;
                switch (methodName)
                {
                    case FastQFileParameters.TextReaderReadOnly:
                        using (TextReader txtReader = new StreamReader(filePath))
                            newqualSequence = fastQParserObj.Parse(txtReader, true);
                        break;
                    case FastQFileParameters.TextReader:
                        using (TextReader txtReader = new StreamReader(filePath))
                            newqualSequence = fastQParserObj.Parse(txtReader);
                        break;
                    case FastQFileParameters.FileName:
                        newqualSequence = fastQParserObj.Parse(filePath);
                        break;
                    case FastQFileParameters.FileNameReadOnly:
                        newqualSequence = fastQParserObj.Parse(filePath, true);
                        break;
                    default:
                        break;
                }

                // Validate qualittative Sequence upon parsing FastQ file.
                Assert.AreEqual(expectedSeqCount, newqualSequence.Count.ToString((IFormatProvider)null));
                Assert.AreEqual(newqualSequence[0].ToString(), expectedQualitativeSequence);
                Assert.AreEqual(newqualSequence[0].Type, expectedFormatType);
                Assert.AreEqual(newqualSequence[0].ID.ToString((IFormatProvider)null), expectedSequenceId);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence '{0}' validation after Parse() is found to be as expected.",
                    newqualSequence[0]));

                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                    newqualSequence[0].ToString()));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                    newqualSequence[0].ID.ToString((IFormatProvider)null)));
            }
        }

        /// <summary>
        /// General method to validate FastQ ParseOne.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">Name of Method</param>
        /// </summary>
        void ValidateFastQParseOne(string nodeName,
            FastQFileParameters methodName)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedQualitativeSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
            string expectedSequenceId = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode);
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));
            string expectedSeqCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SeqLength);

            // Parse a FastQ file using parseOne method.
            using (FastQParser fastQParserObj = new FastQParser())
            {
                fastQParserObj.AutoDetectFastQFormat = false;
                fastQParserObj.FastqType = expectedFormatType;

                IQualitativeSequence qualSequence = null;
                switch (methodName)
                {
                    case FastQFileParameters.TextReaderReadOnly:
                        using (TextReader txtReader = new StreamReader(filePath))
                            qualSequence = fastQParserObj.ParseOne(txtReader, true);
                        break;
                    case FastQFileParameters.TextReader:
                        using (TextReader txtReader = new StreamReader(filePath))
                            qualSequence = fastQParserObj.ParseOne(txtReader);
                        break;
                    case FastQFileParameters.FileName:
                        qualSequence = fastQParserObj.ParseOne(filePath);
                        break;
                    case FastQFileParameters.FileNameReadOnly:
                        qualSequence = fastQParserObj.ParseOne(filePath, true);
                        break;
                    default:
                        break;
                }

                // Validate qualittative Sequence upon parsing FastQ file.
                Assert.AreEqual(qualSequence.Count.ToString((IFormatProvider)null), expectedSeqCount);
                Assert.AreEqual(qualSequence.ToString(), expectedQualitativeSequence);
                Assert.AreEqual(qualSequence.Type, expectedFormatType);
                Assert.AreEqual(qualSequence.ID.ToString((IFormatProvider)null), expectedSequenceId);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence '{0}' validation after Parse() is found to be as expected.",
                    qualSequence));

                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                    qualSequence.ToString()));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastQ Parser BVT: The FASTQ sequence ID '{0}' validation after Parse() is found to be as expected.",
                    qualSequence.ID.ToString((IFormatProvider)null)));
            }
        }

        /// <summary>
        /// General method to validate FastQ Parser Virtulization method.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">Name of Method</param>
        /// </summary>
        void ValidateDataVirtulization(string nodeName,
            FastQFileParameters methodName)
        {
            string filepathOriginal = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            Assert.IsTrue(File.Exists(filepathOriginal));

            string filepathTmp = Path.GetTempFileName();
            using (FastQParser fastQParserObj = new FastQParser())
            {
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

                string originalIds = string.Empty;
                string newSeqIds = string.Empty;
                int count = seqsOriginal.Count;
                int i;

                for (i = 0; i < count; i++)
                {
                    originalIds += seqsOriginal[i].ID.ToString((IFormatProvider)null) + ",";
                    newSeqIds += seqsNew[i].ID.ToString((IFormatProvider)null) + ",";
                    string orgSeq = seqsOriginal[i].ToString();
                    string newSeq = seqsNew[i].ToString();
                    string orgscores =
                        ASCIIEncoding.ASCII.GetString(seqsOriginal[i].Scores);
                    string newscores =
                        ASCIIEncoding.ASCII.GetString(seqsNew[i].Scores);
                    Assert.AreEqual(orgSeq, newSeq);
                    Assert.AreEqual(orgscores, newscores);
                }

                for (i = 0; i < count; i++)
                {
                    Assert.IsTrue(originalIds.Contains(seqsNew[i].ID));
                }

            }

            File.Delete(filepathTmp);
        }

        #endregion Supporting Methods
    }
}