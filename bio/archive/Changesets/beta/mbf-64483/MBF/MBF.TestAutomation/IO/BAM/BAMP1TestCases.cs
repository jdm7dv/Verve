// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * BAMP1TestCases.cs
 * 
 *   This file contains the BAM - Parsers and Formatters P1 test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.IO;
using MBF.IO.BAM;
using MBF.IO.SAM;
using System.IO;
using MBF.IO.Fasta;
using MBF.Algorithms.Alignment;
using MBF.Encoding;

namespace MBF.TestAutomation.IO.BAM
{
    /// <summary>
    /// BAM P1 parser and formatter Test case implementation.
    /// </summary>
    [TestClass]
    public class BAMP1TestCases
    {
        #region Enums

        /// <summary>
        /// BAM Parser ctor parameters used for different test cases.
        /// </summary>
        enum BAMParserParameters
        {
            StreamReader,
            StreamReaderWithReadOnly,
            FileName,
            FileNameWithReadOnly,
            ParseRangeFileName,
            ParseRangeWithIndex,
            ParseRangeFileNameWithReadOnly,
            IndexFile,
            StreamWriter,
            ParseRangeWithSequenceRange,
            ParseRangeWithReadOnlySequenceRange,
            StreamAndIndexFile,
            Stream,
            ParseRangeWithMaxValue,
            ParseOne,
            ParseOneReadOnly,
        }

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\SAMBAMTestData\SAMBAMTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static BAMP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        # region BAM Parser P1 Test Cases

        /// <summary>
        /// Validate BAM Parse(Stream) by passing Multiple aligned sequence 
        /// BAM file.
        /// Input : Multiple aligned seq BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserWithMultipleAlignedSeqUsingStream()
        {
            ValidateBAMParser(Constants.BAMFileWithMultipleAlignedSeqsNode,
                BAMParserParameters.StreamReader, false, true);
        }

        /// <summary>
        /// Validate BAM Parse(filename) by passing Multiple aligned sequence 
        /// BAM file
        /// Input : Multiple aligned seq BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserWithMultipleAlignedSeq()
        {
            ValidateBAMParser(Constants.BAMFileWithMultipleAlignedSeqsNode,
                BAMParserParameters.FileName, false, true);
        }

        /// <summary>
        /// Validate BAM Parse(filename,IsReadOnly) by passing Multiple aligned 
        /// sequence BAM file
        /// Input : Multiple aligned seq BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserWithReadOnlyMultipleAlignedSeq()
        {
            ValidateBAMParser(Constants.BAMFileWithMultipleAlignedSeqsNode,
                BAMParserParameters.FileNameWithReadOnly, false, true);
        }

        /// <summary>
        /// Validate BAM Parse(Stream,IsReadOnly) by passing Multiple aligned sequence 
        /// BAM file
        /// Input : Multiple aligned seq BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserMultipleAlignedSeqWithReadOnlyProperty()
        {
            ValidateBAMParser(Constants.BAMFileWithMultipleAlignedSeqsNode,
                BAMParserParameters.StreamReaderWithReadOnly, false, true);
        }

        /// <summary>
        /// Validate BAM Parse(Stream) by passing BAM file with aligned
        /// sequences with quality values.
        /// Input : Aligned sequence with quality values BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserByPassingSeqsWithQualityUsingStreamReader()
        {
            ValidateBAMParserForQualitySequences(
                Constants.BAMFileWithQualityValuesNode,
                BAMParserParameters.StreamReader);
        }

        /// <summary>
        /// Validate BAM Parse(filename) by passing BAM file with aligned
        /// sequences with quality values.
        /// Input : BAM file with Aligned sequence and quality values
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserAlignedSeqsWithQualityValues()
        {
            ValidateBAMParserForQualitySequences(
                Constants.BAMFileWithQualityValuesNode,
                BAMParserParameters.FileName);
        }

        /// <summary>
        /// Validate BAM Parse(filename,ReadOnly) by passing BAM file with aligned
        /// sequences with quality values.
        /// Input : Aligned sequence with quality values BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserReadOnlyAlignedSeqsWithQualityValues()
        {
            ValidateBAMParserForQualitySequences(
                Constants.BAMFileWithQualityValuesNode,
                BAMParserParameters.FileNameWithReadOnly);
        }

        /// <summary>
        /// Validate BAM Parse(Stream,ReadOnly) by passing BAM file with aligned
        /// sequences with quality values.
        /// Input : Aligned sequence with quality values BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserQualitySeqsWithReadOnlySeqs()
        {
            ValidateBAMParserForQualitySequences(
                Constants.BAMFileWithQualityValuesNode,
                BAMParserParameters.StreamReaderWithReadOnly);
        }

        /// <summary>
        /// Validate BAM ParseRange(sequenceRange) by passing BAM file and 
        /// sequenceRange.
        /// Input : BAM file and sequenceRange.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserWithSequenceRange()
        {
            ValidateBAMParser(Constants.BAMFileWithSequenceRangeSeqsNode,
                BAMParserParameters.ParseRangeWithSequenceRange, false, true);
        }

        /// <summary>
        /// Validate BAM ParseRange(sequenceRange,ReadOnly) by passing Medium size
        /// BAM file and sequenceRange
        /// Input : BAM file and sequenceRange.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserSequenceRangeForMediumSizeBAMFile()
        {
            ValidateBAMParser(Constants.MediumSizeBAMFileNode,
                BAMParserParameters.ParseRangeWithReadOnlySequenceRange, false, true);
        }

        /// <summary>
        /// Validate BAM ParseRange(sequenceRange,ReadOnly) by passing Medium 
        /// size BAM file and Smaller sequenceRange.
        /// Input : BAM file and sequenceRange.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserSequenceRangeWithSmallerEndIndex()
        {
            ValidateBAMParser(Constants.MediumSizeBAMFileWithSmallerEndIndexNode,
                BAMParserParameters.ParseRangeWithReadOnlySequenceRange, false, true);
        }

        /// <summary>
        /// Validate BAM ParseRange(filename,refIndex) by passing Medium size
        /// BAM file and sequenceRange.
        /// Input : BAM file and sequenceRange.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserSequenceRangeWithRefIndexForMediumSizeBAM()
        {
            ValidateBAMParser(Constants.MediumSizeBAMFileNode,
                BAMParserParameters.ParseRangeWithIndex, false, true);
        }

        /// <summary>
        /// Validate BAM ParseRange(filename,refIndex) by passing Medium size
        /// BAM file and Smaller sequenceRange
        /// Input : BAM file and sequenceRange
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserSequenceRangeWithRefIndexForSmallerEndIndex()
        {
            ValidateBAMParser(Constants.MediumSizeBAMFileWithSmallerEndIndexNode,
                BAMParserParameters.ParseRangeWithIndex, false, true);
        }

        /// <summary>
        /// Validate BAM ParseRange(filename,refIndex,ReadOnly) by passing Medium size
        /// BAM file and sequenceRange
        /// Input : BAM file and sequenceRange
        /// Output : Validation of aligned sequence
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserRefIndexForReadOnlyMediumSizeBAM()
        {
            ValidateBAMParser(Constants.MediumSizeBAMFileWithRefIndexNode,
                BAMParserParameters.ParseRangeFileNameWithReadOnly, false, true);
        }

        /// <summary>
        /// Validate BAM ParseRange(filename,refIndex,ReadOnly) by passing 
        /// multiple aligned sequence.
        /// Input : BAM file and sequenceRange.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserRefIndexForReadOnlyMultipleAlignedSeq()
        {
            ValidateBAMParser(Constants.BAMFileWithMultipleAlignedSeqsNode,
                BAMParserParameters.ParseRangeFileNameWithReadOnly, false, true);
        }

        /// <summary>
        /// Validate BAM ParseRange(sequenceRange,ReadOnly) by passing 
        /// multiple aligned sequence BAM file and sequenceRange.
        /// Input : Multiple aligned seq BAM file and sequenceRange.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserSequenceRangeForMultipleAlignedSeqs()
        {
            ValidateBAMParser(Constants.MediumSizeBAMFileWithSmallerEndIndexNode,
                BAMParserParameters.ParseRangeWithReadOnlySequenceRange, false, true);
        }

        /// <summary>
        /// Validate BAM Parser using ISequenceAlignment Parser.
        /// Input : BAM file.
        /// Output : Aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateISequenceAlignmentBAMParserForSmallSizeBAMFile()
        {
            ValidateISequenceAlignmentBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.FileNameWithReadOnly);
        }

        /// <summary>
        /// Validate BAM Parser Parse(filename,range,readOnly) using 
        /// ISequenceAlignment Parser.
        /// Input : BAM file.
        /// Output : Aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateParseRangeSequenceWithMaxValue()
        {
            ValidateBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.ParseRangeWithMaxValue, false, true);
        }

        /// <summary>
        /// Validate BAM Parser ParseOne(filename) for small size BAM file.
        /// Input : BAM file.
        /// Output : Validation of aligned seq using ParseOne() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserParseOneForBAMFile()
        {
            ValidateISequenceAlignmentBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.ParseOne);
        }

        /// <summary>
        /// Validate BAM Parser ParseOne(filename,ReadOnly) for small size BAM file.
        /// Input : BAM file.
        /// Output : Validation of aligned seq using ParseOne() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserParseOneForReadOnlySeqs()
        {
            ValidateISequenceAlignmentBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.ParseOneReadOnly);
        }

        /// <summary>
        /// Validate BAM Parser FormatString 
        /// Not supportedException.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParserFormatString()
        {
            // Create a BAMFormatter object.
            BAMFormatter bamFormatterObj = new BAMFormatter();

            // Validate BAM Formatter NotSupported methods.
            try
            {
                bamFormatterObj.FormatString(null);
            }
            catch (NotSupportedException ex)
            {
                string exceptionMessage = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P1 : Validated Exception {0} successfully",
                    exceptionMessage));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P1 : Validated Exception {0} successfully",
                    exceptionMessage));
            }

            try
            {
                bamFormatterObj.Format(null as ICollection<ISequenceAlignment>, null as TextWriter);
            }
            catch (NotSupportedException ex)
            {
                string exceptionMessage = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P1 : Validated Exception {0} successfully",
                    exceptionMessage));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P1 : Validated Exception {0} successfully",
                    exceptionMessage));
            }

            try
            {
                bamFormatterObj.Format(null as ICollection<ISequenceAlignment>, null as string);
            }
            catch (NotSupportedException ex)
            {
                string exceptionMessage = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P1 : Validated Exception {0} successfully",
                    exceptionMessage));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P1 : Validated Exception {0} successfully",
                    exceptionMessage));
            }
        }

        # endregion BAM Parser P1 Test Cases

        /// <summary>
        /// Validate BAM file Aligned sequence properties.
        /// Input : Valid BAM file.
        /// Output : Validation of aligned seq properties.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateAlignedSeqProperties()
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMAlignedSeqPropertiesNode, Constants.FilePathNode);
            string expectedFlagValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMAlignedSeqPropertiesNode, Constants.FlagValueNode);
            string expectedISize = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMAlignedSeqPropertiesNode, Constants.Isize);
            string expectedMapQ = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMAlignedSeqPropertiesNode, Constants.MapQValue);
            string expectedMetadataCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMAlignedSeqPropertiesNode, Constants.Metadata);
            string expectedMPos = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMAlignedSeqPropertiesNode, Constants.MPos);
            string expectedOptionalFields = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMAlignedSeqPropertiesNode, Constants.OptionalFieldsNode);
            string expectedPos = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMAlignedSeqPropertiesNode, Constants.Pos);
            string expectedQueryLength = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMAlignedSeqPropertiesNode, Constants.QueryLength);
            string expectedRName = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMAlignedSeqPropertiesNode, Constants.RName);

            // Parse a BAM file.
            using (BAMParser bamParseObj = new BAMParser())
            {
                BAMFormatter bamFormatterObj = new BAMFormatter();
                SequenceAlignmentMap seqAlignment = bamParseObj.Parse(bamFilePath);

                // Get Aligned sequences.
                IList<SAMAlignedSequence> alignedSeqs = seqAlignment.QuerySequences;

                // Validate BAM Formatter Properties.
                Assert.AreEqual(Constants.BAMFileName, bamFormatterObj.Name);
                Assert.AreEqual(Constants.BAMFileType, bamFormatterObj.FileTypes);
                Assert.AreEqual(Constants.BAMFormatterDescription,
                    bamFormatterObj.Description.Replace("\r", "").Replace("\n", ""));

                // Validate BAM Parser Properties.
                Assert.AreEqual(Constants.BAMFileName, bamParseObj.Name);
                Assert.AreEqual(Constants.BAMFileType, bamParseObj.FileTypes);
                Assert.AreEqual(Constants.BAMDescription,
                    bamParseObj.Description.Replace("\r", "").Replace("\n", ""));

                // Validate all properties of aligned sequence.
                Assert.AreEqual(expectedFlagValue, alignedSeqs[0].Flag.ToString());
                Assert.AreEqual(expectedISize, alignedSeqs[0].ISize.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedMapQ, alignedSeqs[0].MapQ.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedMetadataCount,
                    alignedSeqs[0].Metadata.Count.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedMPos, alignedSeqs[0].MPos.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedOptionalFields,
                    alignedSeqs[0].OptionalFields.Count.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedPos, alignedSeqs[0].Pos.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedQueryLength,
                    alignedSeqs[0].QueryLength.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedRName, alignedSeqs[0].RName.ToString((IFormatProvider)null));
            }

            // Log to NUNIT GUI.
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "BAM Parser P1 : Validated the Aligned sequence properties successfully"));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "BAM Parser P1 : Validated the Aligned sequence properties successfully"));
        }

        /// <summary>
        /// Validate BAM refernce indices.
        /// Input : BAM file.
        /// Output : BAM indices validation
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMIndexes()
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMIndexNode, Constants.FilePathNode);
            string expectedBAMIndices = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMIndexNode, Constants.BAMIndexCountNode);

            // Create BAMParser object.
            using (BAMParser bamParserObj = new BAMParser())
            {

                BAMIndex bamIndices = bamParserObj.GetIndexFromBAMFile(bamFilePath);

                // Validate BAM reference indices.
                Assert.AreEqual(expectedBAMIndices, bamIndices.RefIndexes.Count.ToString((IFormatProvider)null));
            }

            // Log to NUNIT GUI.
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "BAM Parser P1 : Validated the BAM indices successfully"));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "BAM Parser P1 : Validated the BAM indices successfully"));
        }

        # region BAM SAM InterConversion Test Cases.

        /// <summary>
        /// Validate SAM file to BAM file conversion for 
        /// multiple aligned seqs SAM file.
        /// Input : Multiple aligned SAM file.
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSAMToBAMConversionForMultipleAlignedSeq()
        {
            ValidateSAMToBAMConversion(
                Constants.SAMToBAMConversionForMultipleQualitySeqsNode);
        }

        /// <summary>
        /// Validate SAM file to BAM file conversion for 
        /// SAM file with quality values.
        /// Input : Multiple aligned SAM file.
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSAMToBAMConversionForAlignedSeqWithQuality()
        {
            ValidateSAMToBAMConversion(
                Constants.SAMToBAMConversionForQualitySeqsNode);
        }

        /// <summary>
        /// Validate BAM file to SAM file conversion for 
        /// single aligned seq BAM file.
        /// Input : Single aligned seq BAM file.
        /// Output : SAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMToSAMConversion()
        {
            ValidateBAMToSAMConversion(
                Constants.BAMToSAMConversionNode);
        }

        /// <summary>
        /// Validate BAM file to SAM file conversion for 
        /// multiple aligned seq BAM file.
        /// Input : Multiple aligned seq BAM file.
        /// Output : SAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMToSAMConversionForMultipleAlignedSeq()
        {
            ValidateBAMToSAMConversion(
                Constants.SAMToBAMConversionForMultipleAlignedSeqNode);
        }

        /// <summary>
        /// Validate BAM file to SAM file conversion for 
        /// large BAM file.
        /// Input : Multiple aligned seq BAM file.
        /// Output : SAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMToSAMConversionForLargeFileWithDV()
        {
            ValidateSAMToBAMConversionWithDVEnabled(
                Constants.SAMToBAMConversionForMultipleAlignedSeqWithDVNode);
        }
        # endregion BAM SAM InterConversion Test Cases.

        # region BAM formatter Test cases.

        /// <summary>
        /// Validate format BAM file with ISequenceAlignment
        /// using format(SeqAlignment,filename) method
        /// Input : List of sequence alignmens
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFormatSeqLignmentToBAMFileUsingFilename()
        {
            ValidateBAMFormatterWithSequenceAlignment(
                Constants.SmallSizeBAMFileNode, BAMParserParameters.FileName,
                false, false);
        }

        /// <summary>
        /// Validate format BAM file for Multiple aligned sequence 
        /// with ISequenceAlignment using format(SeqAlignment,filename)
        /// Input : List of sequence alignmens.
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFormatMultipleAlignedSeqToBAMFileUsingFilename()
        {
            ValidateBAMFormatterWithSequenceAlignment(
                Constants.BAMFileFormatWithMultipleAlignedSeqsNode,
                BAMParserParameters.FileName, false, false);
        }

        /// <summary>
        /// Validate format BAM file for quality sequence 
        /// with ISequenceAlignment using format(SeqAlignment,filename).
        /// Input : List of sequence alignmens.
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFormatSeqsWithQualityToBAMFileUsingFilename()
        {
            ValidateBAMFormatterWithSequenceAlignment(
                Constants.BAMFileFormatWithMultipleAlignedSeqsNode,
                BAMParserParameters.FileName, false, false);
        }

        /// <summary>
        /// Validate format BAM file with ISequenceAlignment
        /// using format(SeqAlignment,stream,BAMIndexFile).
        /// Input : List of sequence alignmens.
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFormatSeqLignmentToBAMFileUsingBAMIndexFile()
        {
            ValidateBAMFormatterWithSequenceAlignment(
                Constants.SmallSizeBAMFileNode,
                BAMParserParameters.StreamAndIndexFile, true, false);
        }

        /// <summary>
        /// Validate format BAM file with ISequenceAlignment
        /// using format(SeqAlignmentMap,stream,BAMIndexFile).
        /// Input : List of sequence alignmens.
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFormatSeqLignmentMapToBAMFileUsingBAMIndexFile()
        {
            ValidateBAMFormatter(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.StreamAndIndexFile);
        }

        /// <summary>
        /// Validate format BAM file with ISequenceAlignment
        /// using format(SeqAlignment,BamFile,BAMIndexFile).
        /// Input : List of sequence alignmens.
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFormatSeqLignmentToBAMFileUsingBAMFile()
        {
            ValidateBAMFormatterWithSequenceAlignment(
                Constants.SmallSizeBAMFileNode,
                BAMParserParameters.IndexFile, false, false);
        }

        /// <summary>
        /// Validate format BAM file with ISequenceAlignment
        /// using format(SeqAlignment,StreamWriter).
        /// Input : List of sequence alignmens.
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFormatSeqLignmentToBAMFileUsingStream()
        {
            ValidateBAMFormatterWithSequenceAlignment(
                Constants.SmallSizeBAMFileNode,
                BAMParserParameters.Stream, false, false);
        }

        /// <summary>
        /// Validate format multiple aligned sequence to BAM file with
        /// ISequenceAlignment using format(SeqAlignment,StreamWriter).
        /// Input : List of sequence alignmens.
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFormatMulitpleSeqLignmentToBAMFileUsingStream()
        {
            ValidateBAMFormatterWithSequenceAlignment(
                Constants.BAMFileFormatWithMultipleAlignedSeqsNode,
                BAMParserParameters.Stream, false, false);
        }

        /// <summary>
        /// Validate format multiple aligned sequence with quality value to BAM
        /// file with ISequenceAlignment using format(SeqAlignment,StreamWriter)
        /// Input : List of sequence alignmens.
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFormatQualitySeqLignmentToBAMFileUsingStream()
        {
            ValidateBAMFormatterWithSequenceAlignment(
                Constants.BAMFileFormatWithMultipleAlignedSeqsNode,
                BAMParserParameters.Stream, false, false);
        }

        /// <summary>
        /// Validate format(SeqAlignment,TextWriter) not implemented exception.
        /// Input : List of sequence alignmens.
        /// Output : Validate an exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFormatBAMFileWithTextWriter()
        {
            ValidateBAMFormatterWithSequenceAlignment(
                Constants.SmallSizeBAMFileNode,
                BAMParserParameters.StreamWriter, false, true);
        }

        /// <summary>
        /// Validate Parse aligned sequence with sequence pointer
        /// for medium size BAM file.
        /// Input : BAM file.
        /// Output :BAM Aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBAMParseAlignedSeqWithSeqPointerMediumSizeBAMFile()
        {
            // Get values from XML node.
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMFileWithRefSeqNode, Constants.ExpectedSeqWithPointersNode);
            string samFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMFileWithRefSeqNode, Constants.FilePathNode);
            string startingLineForPointer = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMFileWithRefSeqNode, Constants.LineNumberToPointNode);
            string startIndex = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMFileWithRefSeqNode, Constants.StartIndexNode);
            string endIndex = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMFileWithRefSeqNode, Constants.EndIndexNode);

            // Parse a BAM file
            using (BAMParser parserObj = new BAMParser())
            {
                parserObj.EnforceDataVirtualization = true;

                SequenceAlignmentMap seqList = parserObj.Parse(samFilePath);
                Assert.IsNotNull(seqList);

                // Get a pointer object
                SequencePointer pointerObj =
                     GetBAMSequencePointer(Int32.Parse(startingLineForPointer, (IFormatProvider)null),
                     Int32.Parse(startIndex, (IFormatProvider)null), Int32.Parse(endIndex, (IFormatProvider)null));

                // Parse a BAM file using Sequence Pointer.
                SAMAlignedSequence alignedSeq = (
                    SAMAlignedSequence)parserObj.ParseAlignedSequence(pointerObj);

                // Validate parsed BAM aligned sequence.
                Assert.AreEqual(expectedSequence,
                    alignedSeq.QuerySequence.ToString());

                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P1 : Sequence alignment aligned seq {0} validate successfully",
                    alignedSeq.Sequences[0].ToString()));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "BAM Parser P1 : Sequence alignment aligned seq validate successfully"));
            }
        }

        # endregion BAM Formatter Test cases.

        # region Helper Methods

        /// <summary>
        /// Parse BAM and validate parsed aligned sequences by creating 
        /// ISequenceAlignment interface object and its properties.
        /// </summary>
        /// <param name="nodeName">Different xml nodes used for different test cases</param>
        /// <param name="BAMParserPam">BAM Parse method parameters</param>
        void ValidateISequenceAlignmentBAMParser(string nodeName,
            BAMParserParameters BAMParserPam)
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string expectedAlignedSeqFilePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);

            IList<ISequenceAlignment> seqAlignmentList = null;
            ISequenceAlignmentParser bamParser = null;
            ISequenceAlignment seqAlignment = null;
            IList<IAlignedSequence> alignedSeqs = null;

            try
            {
                bamParser = new BAMParser(Encodings.IupacNA);

                // Parse a BAM file with different parameters.
                switch (BAMParserPam)
                {
                    case BAMParserParameters.FileName:
                        seqAlignmentList = bamParser.Parse(bamFilePath);
                        alignedSeqs = seqAlignmentList[0].AlignedSequences;
                        break;
                    case BAMParserParameters.FileNameWithReadOnly:
                        seqAlignmentList = bamParser.Parse(bamFilePath, false);
                        alignedSeqs = seqAlignmentList[0].AlignedSequences;
                        break;
                    case BAMParserParameters.ParseOne:
                        seqAlignment = bamParser.ParseOne(bamFilePath);
                        alignedSeqs = seqAlignment.AlignedSequences;
                        break;
                    case BAMParserParameters.ParseOneReadOnly:
                        seqAlignment = bamParser.ParseOne(bamFilePath, false);
                        alignedSeqs = seqAlignment.AlignedSequences;
                        break;
                    default:
                        break;
                }

                // Get expected sequences
                using (FastaParser parserObj = new FastaParser())
                {
                    IList<ISequence> expectedSequences = parserObj.Parse(expectedAlignedSeqFilePath);

                    // Validate aligned sequences from BAM file.
                    for (int index = 0; index < alignedSeqs.Count; index++)
                    {
                        Assert.AreEqual(expectedSequences[index].ToString(),
                            alignedSeqs[index].Sequences[0].ToString());

                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P1 : Validated Aligned sequence :{0} successfully",
                            alignedSeqs[index].Sequences.ToString()));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P1 : Validated the aligned sequence :{0} successfully",
                            alignedSeqs[index].Sequences.ToString()));
                    }
                }
            }
            finally
            {
                (bamParser as BAMParser).Dispose();
            }
        }


        /// <summary>
        /// Parse BAM and validate parsed aligned sequences and its properties.
        /// </summary>
        /// <param name="nodeName">Different xml nodes used for different test cases</param>
        /// <param name="BAMParserPam">BAM Parse method parameters</param>
        /// <param name="IsEncoding">True for BAMParser ctor with encoding False otherwise </param>
        /// <param name="IsReferenceIndex">True If validating reference index false otherwise</param>
        void ValidateBAMParser(string nodeName,
            BAMParserParameters BAMParserPam, bool IsEncoding,
            bool IsReferenceIndex)
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string expectedAlignedSeqFilePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string refIndexValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RefIndexNode);
            string startIndexValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.StartIndexNode);
            string endIndexValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EndIndexNode);
            string alignedSeqCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlignedSeqCountNode);
            string expectedChromosome = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ChromosomeNameNode);

            SequenceAlignmentMap seqAlignment = null;
            BAMParser bamParser = null;

            try
            {
                if (IsEncoding)
                {
                    bamParser = new BAMParser();
                }
                else
                {
                    bamParser = new BAMParser(Encodings.IupacNA);
                }

                // Parse a BAM file with different parameters.
                switch (BAMParserPam)
                {
                    case BAMParserParameters.StreamReader:
                        using (Stream stream = new FileStream(bamFilePath, FileMode.Open,
                            FileAccess.Read))
                        {
                            seqAlignment = bamParser.Parse(stream);
                        }
                        break;
                    case BAMParserParameters.StreamReaderWithReadOnly:
                        using (Stream stream = new FileStream(bamFilePath, FileMode.Open,
                            FileAccess.Read))
                        {
                            seqAlignment = bamParser.Parse(stream, false);
                        }

                        // Validate Aligned sequence Read-Only property.
                        Assert.IsTrue(ValidateReadOnlySequences(seqAlignment));
                        break;
                    case BAMParserParameters.FileName:
                        seqAlignment = bamParser.Parse(bamFilePath);
                        break;
                    case BAMParserParameters.FileNameWithReadOnly:
                        seqAlignment = bamParser.Parse(bamFilePath, false);

                        // Validate Aligned sequence Read-Only property.
                        Assert.IsTrue(ValidateReadOnlySequences(seqAlignment));
                        break;
                    case BAMParserParameters.ParseRangeFileName:
                        seqAlignment = bamParser.ParseRange(bamFilePath,
                            Convert.ToInt32(refIndexValue, (IFormatProvider)null));
                        break;
                    case BAMParserParameters.ParseRangeFileNameWithReadOnly:
                        seqAlignment = bamParser.ParseRange(bamFilePath,
                            Convert.ToInt32(refIndexValue, (IFormatProvider)null), false);

                        // Validate Aligned sequence Read-Only property.
                        Assert.IsTrue(ValidateReadOnlySequences(seqAlignment));
                        break;
                    case BAMParserParameters.ParseRangeWithIndex:
                        seqAlignment = bamParser.ParseRange(bamFilePath,
                            Convert.ToInt32(refIndexValue, (IFormatProvider)null),
                            Convert.ToInt32(startIndexValue, (IFormatProvider)null),
                            Convert.ToInt32(endIndexValue, (IFormatProvider)null), false);
                        break;
                    case BAMParserParameters.ParseRangeWithSequenceRange:
                        seqAlignment = bamParser.ParseRange(bamFilePath,
                            new SequenceRange(expectedChromosome, Convert.ToInt32(startIndexValue, (IFormatProvider)null),
                            Convert.ToInt32(endIndexValue, (IFormatProvider)null)));
                        break;
                    case BAMParserParameters.ParseRangeWithReadOnlySequenceRange:
                        seqAlignment = bamParser.ParseRange(bamFilePath,
                            new SequenceRange(expectedChromosome,
                                Convert.ToInt32(startIndexValue, (IFormatProvider)null),
                                Convert.ToInt32(endIndexValue, (IFormatProvider)null)), false);
                        break;
                    case BAMParserParameters.ParseRangeWithMaxValue:
                        seqAlignment = bamParser.ParseRange(bamFilePath,
                            new SequenceRange(expectedChromosome,
                                0, int.MaxValue), false);
                        break;
                    default:
                        break;
                }
            }
            finally
            {
                bamParser.Dispose();
            }


            // Validate BAM Header record fileds.
            if (!IsReferenceIndex)
            {
                ValidateBAMHeaderRecords(nodeName, seqAlignment);
            }

            IList<SAMAlignedSequence> alignedSeqs = seqAlignment.QuerySequences;

            Assert.AreEqual(alignedSeqCount, alignedSeqs.Count.ToString((IFormatProvider)null));

            // Get expected sequences
            using (FastaParser parserObj = new FastaParser())
            {
                IList<ISequence> expectedSequences = parserObj.Parse(expectedAlignedSeqFilePath);

                // Validate aligned sequences from BAM file.
                for (int index = 0; index < alignedSeqs.Count; index++)
                {
                    Assert.AreEqual(expectedSequences[index].ToString(),
                        alignedSeqs[index].QuerySequence.ToString());

                    // Log to NUNIT GUI.
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P1 : Validated Aligned sequence :{0} successfully",
                        alignedSeqs[index].QuerySequence.ToString()));
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P1 : Validated the aligned sequence :{0} successfully",
                        alignedSeqs[index].QuerySequence.ToString()));
                }
            }
        }

        /// <summary>
        /// Parse BAM and validate parsed aligned sequences and its properties.
        /// </summary>
        /// <param name="nodeName">Different xml nodes used for different test cases</param>
        /// <param name="BAMParserPam">Different Parser parameters used for different testcases</param>
        void ValidateBAMParserForQualitySequences(string nodeName,
            BAMParserParameters BAMParserPam)
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string expectedQualitySeqFilePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alignedSeqCount = _utilityObj._xmlUtil.GetTextValue(
                 nodeName, Constants.AlignedSeqCountNode);

            SequenceAlignmentMap seqAlignment = null;
            using (BAMParser bamParser = new BAMParser())
            {

                // Parse a BAM file with different parameters.
                switch (BAMParserPam)
                {
                    case BAMParserParameters.FileName:
                        seqAlignment = bamParser.Parse(bamFilePath);
                        break;
                    case BAMParserParameters.FileNameWithReadOnly:
                        seqAlignment = bamParser.Parse(bamFilePath, false);
                        break;
                    case BAMParserParameters.StreamReaderWithReadOnly:
                        using (Stream stream = new FileStream(bamFilePath, FileMode.Open,
                           FileAccess.Read))
                        {
                            seqAlignment = bamParser.Parse(stream, false);
                        }

                        // Validate Aligned sequence Read-Only property.
                        Assert.IsTrue(ValidateReadOnlySequences(seqAlignment));
                        break;
                    case BAMParserParameters.StreamReader:
                        using (Stream stream = new FileStream(bamFilePath, FileMode.Open,
                           FileAccess.Read))
                        {
                            seqAlignment = bamParser.Parse(stream);
                        }
                        break;
                    default:
                        break;
                }

                // Validate Aligned sequence CIGAR,QName and Bin index values.
                ValidateAlignedSeqValues(nodeName, seqAlignment);

                IList<SAMAlignedSequence> alignedSeqs = seqAlignment.QuerySequences;

                Assert.AreEqual(alignedSeqCount, alignedSeqs.Count.ToString((IFormatProvider)null));

                // Get expected quality sequences
                using (FastaParser parserObj = new FastaParser())
                {
                    IList<ISequence> expectedQualitySequences = parserObj.Parse(
                        expectedQualitySeqFilePath);

                    // Validate quality sequences from BAM file.
                    for (int index = 0; index < alignedSeqs.Count; index++)
                    {
                        Assert.AreEqual(expectedQualitySequences[index].ToString(),
                            alignedSeqs[index].QuerySequence.ToString());

                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P1 : Validated Quality sequence :{0} successfully",
                            alignedSeqs[index].QuerySequence.ToString()));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P1 : Validated the Quality sequence :{0} successfully",
                            alignedSeqs[index].QuerySequence.ToString()));
                    }
                }
            }
        }

        /// <summary>
        /// General method to validate BAM to SAM conversion.
        /// </summary>
        /// <param name="nodeName">Different nodeName used for different test cases.</param>
        void ValidateBAMToSAMConversion(string nodeName)
        {
            // Get values from xml config file.
            string expectedSamFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
               Constants.FilePathNode1);
            string bamFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            using (BAMParser bamParserObj = new BAMParser())
            {
                using (SAMParser samParserObj = new SAMParser())
                {
                    SAMFormatter samFormatterObj = new SAMFormatter();
                    SequenceAlignmentMap samSeqAlignment = null;
                    SequenceAlignmentMap bamSeqAlignment = null;

                    // Parse expected SAM file.
                    SequenceAlignmentMap expectedSamAlignmentObj = samParserObj.Parse(
                        expectedSamFilePath);

                    // Parse a BAM file.
                    bamSeqAlignment = bamParserObj.Parse(bamFilePath);

                    // Format BAM sequenceAlignment object to SAM file.
                    samFormatterObj.Format(bamSeqAlignment, Constants.SAMTempFileName);

                    // Parse a formatted SAM file.
                    samSeqAlignment = samParserObj.Parse(Constants.SAMTempFileName);

                    // Validate converted SAM file with expected SAM file.
                    Assert.IsTrue(CompareSequencedAlignmentHeader(samSeqAlignment,
                        expectedSamAlignmentObj));

                    // Validate SAM file aligned sequences.
                    Assert.IsTrue(CompareAlignedSequences(samSeqAlignment,
                        expectedSamAlignmentObj));

                    // Log message to NUnit GUI.
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P1 : Validated the BAM->SAM conversion successfully"));
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P1 : Validated the BAM->SAM conversion successfully"));
                }
            }

            // Delete temporary file.
            File.Delete(Constants.SAMTempFileName);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        /// <summary>
        /// Validate SAM to BAM conversion.
        /// </summary>
        /// <param name="nodeName">Different xml node name used for different test cases</param>
        void ValidateSAMToBAMConversion(string nodeName)
        {
            // Get values from xml config file.
            string expectedBamFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string samFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode1);

            using (BAMParser bamParserObj = new BAMParser())
            {
                using (SAMParser samParserObj = new SAMParser())
                {
                    BAMFormatter bamFormatterObj = new BAMFormatter();
                    bamFormatterObj.CreateSortedBAMFile = true;
                    bamFormatterObj.CreateIndexFile = true;
                    SequenceAlignmentMap samSeqAlignment = null;
                    SequenceAlignmentMap bamSeqAlignment = null;

                    // Parse expected BAM file.
                    SequenceAlignmentMap expextedBamAlignmentObj = bamParserObj.Parse(
                        expectedBamFilePath);

                    // Parse a SAM file.
                    samSeqAlignment = samParserObj.Parse(samFilePath);

                    // Format SAM sequenceAlignment object to BAM file.
                    bamFormatterObj.Format(samSeqAlignment, Constants.BAMTempFileName);

                    // Parse a formatted BAM file.
                    bamSeqAlignment = bamParserObj.Parse(Constants.BAMTempFileName);

                    // Validate converted BAM file with expected BAM file.
                    Assert.IsTrue(CompareSequencedAlignmentHeader(bamSeqAlignment,
                        expextedBamAlignmentObj));

                    // Validate BAM file aligned sequences.
                    Assert.IsTrue(CompareAlignedSequences(bamSeqAlignment,
                        expextedBamAlignmentObj));

                    // Log message to NUnit GUI.
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P1 : Validated the SAM->BAM conversion successfully"));
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P1 : Validated the SAM->BAM conversion successfully"));
                }
            }

            // Delete temporary file.
            File.Delete(Constants.BAMTempFileName);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        /// <summary>
        /// Validate SAM to BAM conversion with DV enabled.
        /// </summary>
        /// <param name="nodeName">Different xml node name used for different test cases</param>
        void ValidateSAMToBAMConversionWithDVEnabled(string nodeName)
        {
            // Get values from xml config file.
            string expectedBamFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string samFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode1);

            using (BAMParser bamParserObj = new BAMParser())
            {
                using (SAMParser samParserObj = new SAMParser())
                {
                    BAMFormatter bamFormatterObj = new BAMFormatter();
                    bamFormatterObj.CreateSortedBAMFile = true;

                    SequenceAlignmentMap samSeqAlignment = null;
                    SequenceAlignmentMap bamSeqAlignment = null;

                    // Parse expected BAM file.
                    SequenceAlignmentMap expextedBamAlignmentObj = bamParserObj.Parse(
                       expectedBamFilePath);

                    // Parse a SAM file with DV enabled
                    samParserObj.EnforceDataVirtualization = true;
                    samSeqAlignment = samParserObj.Parse(samFilePath);

                    // Format SAM sequenceAlignment object to BAM file.
                    bamFormatterObj.Format(samSeqAlignment, Constants.BAMTempFileName);

                    // Parse a formatted BAM file with DV enabled.
                    bamSeqAlignment = bamParserObj.Parse(Constants.BAMTempFileName);

                    // Validate converted BAM file with expected BAM file.
                    Assert.IsTrue(CompareSequencedAlignmentHeader(bamSeqAlignment,
                        expextedBamAlignmentObj));

                    // Validate BAM file aligned sequences.
                    Assert.IsTrue(CompareAlignedSequences(bamSeqAlignment,
                        expextedBamAlignmentObj));

                    // Log message to NUnit GUI.
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P1 : Validated the SAM->BAM conversion successfully"));
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P1 : Validated the SAM->BAM conversion successfully"));

                }
            }

            // Delete temporary file.
            File.Delete(Constants.BAMTempFileName);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        /// <summary>
        /// Format BAM file and validate.
        /// </summary>
        /// <param name="nodeName">Different xml nodes used for different test cases</param>
        /// <param name="BAMParserPam">BAM Format method parameters</param>
        void ValidateBAMFormatter(string nodeName,
             BAMParserParameters BAMParserPam)
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string expectedAlignedSeqFilePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alignedSeqCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlignedSeqCountNode);

            SequenceAlignmentMap seqAlignment = null;
            using (BAMParser bamParserObj = new BAMParser())
            {
                using (BAMIndexFile bamIndexFileObj = new BAMIndexFile(
                                    Constants.BAMTempIndexFileForIndexData,
                                    FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    // Parse a BAM file.
                    seqAlignment = bamParserObj.Parse(bamFilePath);

                    // Create a BAM formatter object.
                    BAMFormatter formatterObj = new BAMFormatter();

                    // Write/Format aligned sequences to BAM file.
                    switch (BAMParserPam)
                    {
                        case BAMParserParameters.StreamAndIndexFile:
                            using (Stream stream = new FileStream(Constants.BAMTempFileName,
                                FileMode.Create, FileAccess.ReadWrite))
                            {
                                formatterObj.Format(seqAlignment, stream, bamIndexFileObj);

                            }
                            break;
                        default:
                            break;
                    }

                    // Parse formatted BAM file and validate aligned sequences.
                    SequenceAlignmentMap expectedSeqAlignmentMap = bamParserObj.Parse(
                        Constants.BAMTempFileName);

                    // Validate Parsed BAM file Header record fileds.
                    ValidateBAMHeaderRecords(nodeName, expectedSeqAlignmentMap);

                    IList<SAMAlignedSequence> alignedSeqs = expectedSeqAlignmentMap.QuerySequences;

                    Assert.AreEqual(alignedSeqCount, alignedSeqs.Count.ToString((IFormatProvider)null));

                    // Get expected sequences
                    using (FastaParser parserObj = new FastaParser())
                    {
                        IList<ISequence> expectedSequences = parserObj.Parse(
                            expectedAlignedSeqFilePath);

                        // Validate aligned sequences from BAM file.
                        for (int index = 0; index < alignedSeqs.Count; index++)
                        {
                            Assert.AreEqual(expectedSequences[index].ToString(),
                                alignedSeqs[index].QuerySequence.ToString());

                            // Log to NUNIT GUI.
                            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                                "BAM Formatter P1 : Validated Aligned sequence :{0} successfully",
                                alignedSeqs[index].QuerySequence.ToString()));
                            Console.WriteLine(string.Format((IFormatProvider)null,
                                "BAM Formatter P1 : Validated the aligned sequence :{0} successfully",
                                alignedSeqs[index].QuerySequence.ToString()));
                        }
                    }
                }
            }

            File.Delete(Constants.BAMTempFileName);
            File.Delete(Constants.BAMTempIndexFile);
        }

        /// <summary>
        /// Format BAM file using IsequenceAlignment object.
        /// </summary>
        /// <param name="nodeName">Different xml nodes used for different test cases</param>
        /// <param name="BAMParserPam">BAM Format method parameters</param>
        /// <param name="WriteBAMIndexData">True if writting BAM index data to BAMIndex file,
        /// false otherwise</param>
        /// <param name="IsNotSupportedMethods">True if validating notsuportedMethods,
        /// false otherwise</param>
        void ValidateBAMFormatterWithSequenceAlignment(string nodeName,
            BAMParserParameters BAMParserPam, bool WriteBAMIndexData,
            bool IsNotSupportedMethods)
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string expectedAlignedSeqFilePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alignedSeqCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlignedSeqCountNode);

            BAMIndexFile bamIndexFileObj = null;
            ISequenceAlignmentParser bamParserObj = new BAMParser();
            try
            {
                using (BAMParser bamSeqMapParserObj = new BAMParser())
                {
                    IList<ISequenceAlignment> seqList = bamParserObj.Parse(bamFilePath);

                    try
                    {
                        // Write BAm index data to BAM Index File.
                        if (WriteBAMIndexData)
                        {
                            bamIndexFileObj = new BAMIndexFile(
                                Constants.BAMTempIndexFileForSequenceAlignment,
                                FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        }

                        // Create a BAM formatter object.
                        BAMFormatter formatterObj = new BAMFormatter();
                        formatterObj.CreateSortedBAMFile = true;
                        formatterObj.CreateIndexFile = true;
                        // Write/Format aligned sequences to BAM file.
                        switch (BAMParserPam)
                        {
                            case BAMParserParameters.StreamWriter:
                                try
                                {
                                    using (TextWriter streamWriter = new StreamWriter(Constants.BAMTempFileName))
                                    {
                                        foreach (ISequenceAlignment seq in seqList)
                                        {
                                            formatterObj.Format(seq, streamWriter);
                                            Assert.Fail();
                                        }
                                    }
                                }
                                catch (NotSupportedException ex)
                                {
                                    string message = ex.Message;
                                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                                        "BAM Formatter P1 : Validated the exception {0} successfully"
                                        , message));
                                }
                                break;
                            case BAMParserParameters.Stream:
                                using (Stream stream = new
                                     FileStream(Constants.BAMTempFileName,
                                     FileMode.OpenOrCreate, FileAccess.ReadWrite))
                                {
                                    foreach (ISequenceAlignment seq in seqList)
                                    {
                                        formatterObj.Format(seq, stream);
                                    }
                                }
                                File.Exists(Constants.BAMTempFileName);
                                break;
                            case BAMParserParameters.FileName:
                                foreach (ISequenceAlignment seq in seqList)
                                {
                                    formatterObj.Format(seq, Constants.BAMTempFileName);
                                }
                                File.Exists(Constants.BAMTempFileName);
                                break;
                            case BAMParserParameters.StreamAndIndexFile:
                                using (Stream stream = new
                                     FileStream(Constants.BAMTempFileName,
                                     FileMode.Create, FileAccess.ReadWrite))
                                {
                                    foreach (ISequenceAlignment seq in seqList)
                                    {
                                        formatterObj.Format(seq, stream, bamIndexFileObj);
                                    }
                                }
                                File.Exists(Constants.BAMTempFileName);
                                break;
                            case BAMParserParameters.IndexFile:
                                foreach (ISequenceAlignment seq in seqList)
                                {
                                    formatterObj.Format(seq, Constants.BAMTempFileName,
                                        Constants.BAMTempIndexFile);
                                }
                                File.Exists(Constants.BAMTempFileName);
                                break;
                            default:
                                break;
                        }

                        if (!IsNotSupportedMethods)
                        {
                            // Parse formatted BAM file and validate aligned sequences.
                            SequenceAlignmentMap expectedSeqAlignmentMap = bamSeqMapParserObj.Parse(
                                Constants.BAMTempFileName);

                            IList<SAMAlignedSequence> alignedSeqs = expectedSeqAlignmentMap.QuerySequences;

                            Assert.AreEqual(alignedSeqCount, alignedSeqs.Count.ToString((IFormatProvider)null));

                            // Get expected sequences
                            using (FastaParser parserObj = new FastaParser())
                            {
                                IList<ISequence> expectedSequences = parserObj.Parse(expectedAlignedSeqFilePath);

                                // Validate aligned sequences from BAM file.
                                for (int index = 0; index < alignedSeqs.Count; index++)
                                {
                                    Assert.AreEqual(expectedSequences[index].ToString(),
                                         alignedSeqs[index].QuerySequence.ToString());

                                    // Log to NUNIT GUI.
                                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                                        "BAM Formatter P1 : Validated Aligned sequence :{0} successfully",
                                        alignedSeqs[index].QuerySequence.ToString()));
                                    Console.WriteLine(string.Format((IFormatProvider)null,
                                        "BAM Formatter P1 : Validated the aligned sequence :{0} successfully",
                                        alignedSeqs[index].QuerySequence.ToString()));
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (bamIndexFileObj != null)
                            bamIndexFileObj.Dispose();
                    }
                }
            }
            finally
            {
                (bamParserObj as BAMParser).Dispose();
            }

            File.Delete(Constants.BAMTempFileName);
            File.Delete(Constants.BAMTempIndexFile);
        }

        /// <summary>
        /// Validate BAM file Header fields.
        /// </summary>
        /// <param name="nodeName">XML nodename used for different test cases</param>
        /// <param name="seqAlignment">seqAlignment object</param>
        void ValidateBAMHeaderRecords(string nodeName,
            SequenceAlignmentMap seqAlignment)
        {
            string expectedHeaderTagValues = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RecordTagValuesNode);
            string expectedHeaderTagKeys = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RecordTagKeysNode);
            string expectedHeaderTypes = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.HeaderTyepsNodes);
            string[] expectedHeaderTagsValues = expectedHeaderTagValues.Split(',');
            string[] expectedHeaderKeys = expectedHeaderTagKeys.Split(',');
            string[] expectedHeaders = expectedHeaderTypes.Split(',');

            SAMAlignmentHeader header = seqAlignment.Header;
            IList<SAMRecordField> recordFields = header.RecordFields;
            int tagKeysCount = 0;
            int tagValuesCount = 0;

            for (int index = 0; index < recordFields.Count; index++)
            {
                Assert.AreEqual(expectedHeaders[index].Replace("/", ""),
                    recordFields[index].Typecode.ToString((IFormatProvider)null).Replace("/", ""));
                for (int tags = 0; tags < recordFields[index].Tags.Count; tags++)
                {
                    Assert.AreEqual(expectedHeaderKeys[tagKeysCount].Replace("/", ""),
                        recordFields[index].Tags[tags].Tag.ToString((IFormatProvider)null).Replace("/", ""));
                    Assert.AreEqual(expectedHeaderTagsValues[tagValuesCount].Replace("/", ""),
                        recordFields[index].Tags[tags].Value.ToString((IFormatProvider)null).Replace("/", "").Replace("\r", "").Replace("\n", ""));
                    tagKeysCount++;
                    tagValuesCount++;
                }
            }
        }

        /// <summary>
        /// Validate Aligned sequences CIGAR,QName, and Bin index values.
        /// </summary>
        /// <param name="nodeName">XML nodename used for different test cases</param>
        /// <param name="seqAlignment">seqAlignment object</param>
        void ValidateAlignedSeqValues(string nodeName,
            SequenceAlignmentMap seqAlignment)
        {
            string expectedCigars = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.CigarsNode);
            string expectedQNames = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.QNamesNode);
            string expectedBinValues = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BinsNode);
            string[] expectedCigarValues = expectedCigars.Split(',');
            string[] expectedQNameValues = expectedQNames.Split(',');
            string[] expectedBin = expectedBinValues.Split(',');

            for (int i = 0; i < seqAlignment.AlignedSequences.Count; i++)
            {
                Assert.AreEqual(expectedCigarValues[i],
                    seqAlignment.QuerySequences[i].CIGAR.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedQNameValues[i],
                    seqAlignment.QuerySequences[i].QName.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedBin[i],
                    seqAlignment.QuerySequences[i].Bin.ToString((IFormatProvider)null));
            }
        }

        /// <summary>
        /// Validate Read-Only property of each sequences in BAM file.
        /// </summary>
        /// <param name="seqAlignment">SeqAlignment object</param>
        /// <returns>True if validation success otherwise false</returns>
        private static bool ValidateReadOnlySequences(SequenceAlignmentMap seqAlignment)
        {
            bool result = false;
            IList<SAMAlignedSequence> alignedSeqs = seqAlignment.QuerySequences;
            for (int i = 0; i < alignedSeqs.Count; i++)
            {
                if (!alignedSeqs[i].Sequences[0].IsReadOnly)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

        /// <summary>
        ///  Comapare Sequence Alignment Header fields
        /// </summary>
        /// <param name="actualAlignment">Actual sequence alignment object</param>
        /// <param name="expectedAlignment">Expected sequence alignment object</param>
        /// <returns>True is successfull, false otherwise</returns>
        private static bool CompareSequencedAlignmentHeader(SequenceAlignmentMap actualAlignment,
             SequenceAlignmentMap expectedAlignment)
        {
            SAMAlignmentHeader aheader = actualAlignment.Header;
            IList<SAMRecordField> arecordFields = aheader.RecordFields;
            SAMAlignmentHeader expectedheader = expectedAlignment.Header;
            IList<SAMRecordField> expectedrecordFields = expectedheader.RecordFields;
            int tagKeysCount = 0;
            int tagValuesCount = 0;

            for (int index = 0; index < expectedrecordFields.Count; index++)
            {
                if (0 != string.Compare(expectedrecordFields[index].Typecode.ToString((IFormatProvider)null),
                    arecordFields[index].Typecode.ToString((IFormatProvider)null)))
                {
                    return false;
                }
                for (int tags = 0; tags < expectedrecordFields[index].Tags.Count; tags++)
                {
                    if ((0 != string.Compare(expectedrecordFields[index].Tags[tags].Tag.ToString((IFormatProvider)null),
                        arecordFields[index].Tags[tags].Tag.ToString((IFormatProvider)null)))
                        || (0 != string.Compare(expectedrecordFields[index].Tags[tags].Value.ToString((IFormatProvider)null),
                        arecordFields[index].Tags[tags].Value.ToString((IFormatProvider)null))))
                    {
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P1 : Sequence alignment header does not match"));
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P1 : Sequence alignment header does not match"));
                        return false;
                    }
                    tagKeysCount++;
                    tagValuesCount++;
                }
            }

            return true;
        }

        /// <summary>
        /// Compare BAM file aligned sequences.
        /// </summary>
        /// <param name="expectedAlignment">Expected sequence alignment object</param>
        /// <param name="actualAlignment">Actual sequence alignment object</param>
        /// <returns>True is successfull, otherwise false</returns>
        private static bool CompareAlignedSequences(SequenceAlignmentMap expectedAlignment,
             SequenceAlignmentMap actualAlignment)
        {
            IList<SAMAlignedSequence> actualAlignedSeqs = actualAlignment.QuerySequences;
            IList<SAMAlignedSequence> expectedAlignedSeqs = expectedAlignment.QuerySequences;

            for (int i = 0; i < expectedAlignedSeqs.Count; i++)
            {
                if (0 != string.Compare(expectedAlignedSeqs[0].QuerySequence.ToString(),
                    actualAlignedSeqs[0].QuerySequence.ToString()))
                {
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P1 : Sequence alignment aligned seq does not match"));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P1 : Sequence alignment aligned seq does match"));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets BAM Sequence pointer.
        /// </summary>
        /// <param name="startLine">Set starting index of the pointer</param>
        /// <returns>sequence pointer</returns>
        private static SequencePointer GetBAMSequencePointer(int startLine,
             int startIndex, int endIndex)
        {
            SequencePointer seqPointer = new SequencePointer();
            seqPointer.AlphabetName = "DNA";
            seqPointer.StartingLine = startLine;
            seqPointer.IndexOffsets[0] = startIndex;
            seqPointer.IndexOffsets[1] = endIndex;
            seqPointer.Id = null;
            return seqPointer;
        }

        # endregion Helper Methods
    }

}
