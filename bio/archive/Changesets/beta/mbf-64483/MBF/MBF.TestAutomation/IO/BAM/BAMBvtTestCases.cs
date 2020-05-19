// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * BAMBvtTestCases.cs
 * 
 *   This file contains the BAM - Parsers and Formatters Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.IO.SAM;
using MBF.IO.BAM;
using MBF.IO.Fasta;
using MBF.Encoding;
using MBF.IO;

namespace MBF.TestAutomation.IO.BAM
{
    /// <summary>
    /// BAM Bvt parser and formatter Test case implementation.
    /// </summary>
    [TestClass]
    public class BAMBvtTestCases
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
            ParserWithEncoding,
            IndexFile,
            IndexStreamWriter,
            StreamWriter,
            Default
        }

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\SAMBAMTestData\SAMBAMTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static BAMBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region BAM Parser Test Cases

        /// <summary>
        /// Validate BAM Parse(Stream) by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBAMParserWithStreamReader()
        {
            ValidateBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.StreamReader, false, false);
        }

        /// <summary>
        /// Validate BAM Parse(Stream,IsReadOnly) by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateReadOnlyBAMParserWithStreamReader()
        {
            ValidateBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.StreamReaderWithReadOnly, false, false);
        }

        /// <summary>
        /// Validate BAM Parse(filename) by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBAMParserWithFilePath()
        {
            ValidateBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.FileName, false, false);
        }

        /// <summary>
        /// Validate BAM Parse(filename,IsReadOnly) by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateReadOnlyBAMParserWithFilePath()
        {
            ValidateBAMParser(Constants.BAMFileWithRefSeqNode,
                BAMParserParameters.FileNameWithReadOnly, false, true);
        }

        /// <summary>
        /// Validate BAM ParseRange(filename,RefIndex) by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBAMParserRangeWithFilePath()
        {
            ValidateBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.ParseRangeFileName, false, false);
        }

        /// <summary>
        /// Validate BAM ParseRange(filename,RefIndex,ReadOnly) 
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBAMParserRangeWithFilePathReadOnlyTag()
        {
            ValidateBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.ParseRangeFileNameWithReadOnly, false, false);
        }

        /// <summary>
        /// Validate BAM ParseRange(filename,RefIndex,Start,End,ReadOnly) 
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBAMParserRangeWithIndex()
        {
            ValidateBAMParser(Constants.SeqRangeBAMFileNode,
                BAMParserParameters.ParseRangeWithIndex, false, false);
        }

        /// <summary>
        /// Validate BAM Parse(filename) with encoding BAMParser ctor.
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBAMParserWithEncoding()
        {
            ValidateBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.FileName, true, false);
        }

        /// <summary>
        /// Validate Parse aligned sequence with sequence pointer.
        /// Input : BAM file.
        /// Output :BAM Aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBAMParseAlignedSeqWithSeqPointer()
        {
            // Get values from XML node.
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMToSAMConversionNode, Constants.ExpectedSeqWithPointersNode);
            string samFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMToSAMConversionNode, Constants.FilePathNode);
            string startingLineForPointer = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMToSAMConversionNode, Constants.LineNumberToPointNode);
            string startIndex = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMToSAMConversionNode, Constants.StartIndexNode);
            string endIndex = _utilityObj._xmlUtil.GetTextValue(
               Constants.BAMToSAMConversionNode, Constants.EndIndexNode);

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

                // Validate parsed SAM aligned sequence.
                Assert.AreEqual(expectedSequence,
                    alignedSeq.QuerySequence.ToString());

                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser BVT : Sequence alignment aligned seq {0} validate successfully",
                    alignedSeq.Sequences[0].ToString()));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "BAM Parser BVT : Sequence alignment aligned seq validate successfully"));
            }
        }

        #endregion BAM Parser Test Cases

        #region BAM Formatter Test Cases

        /// <summary>
        /// Validate format(seqAlignment,bamFile,indexFile) by 
        /// parsing formatting valid BAM file.
        /// Input : Small size BAM file and index file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBAMFormatterWithIndexFile()
        {
            ValidateBAMFormatter(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.IndexFile);
        }

        /// <summary>
        /// Validate format(seqAlignment, Stream) by 
        /// parsing formatting valid BAM file.
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBAMFormatterWithStreamWriter()
        {
            ValidateBAMFormatter(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.StreamWriter);
        }

        /// <summary>
        /// Validate format(seqAlignment, filename) by
        /// parsing formatting valid BAM file.
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBAMFormatterWithFilename()
        {
            ValidateBAMFormatter(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.FileName);
        }

        #endregion BAM Formatter Test Cases.

        #region SAM BAM InterConversion Test Cases

        /// <summary>
        /// Validate BAM to SAM file conversion.
        /// Input : BAM file.
        /// Output : SAM file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBAMToSAMConversion()
        {
            // Get values from xml config file.
            string expectedSamFilePath = _utilityObj._xmlUtil.GetTextValue(Constants.BAMToSAMConversionNode,
               Constants.FilePathNode1);
            string bamFilePath = _utilityObj._xmlUtil.GetTextValue(Constants.BAMToSAMConversionNode,
                Constants.FilePathNode);

            using (BAMParser bamParserObj = new BAMParser())
            {
                using (SAMParser samParserObj = new SAMParser())
                {
                    SAMFormatter samFormatterObj = new SAMFormatter();
                    SequenceAlignmentMap samSeqAlignment = null;
                    SequenceAlignmentMap bamSeqAlignment = null;

                    // Parse expected SAM file.
                    SequenceAlignmentMap expextedSamAlignmentObj = samParserObj.Parse(
                        expectedSamFilePath);

                    // Parse a BAM file.
                    bamSeqAlignment = bamParserObj.Parse(bamFilePath);

                    // Format BAM sequenceAlignment object to SAM file.
                    samFormatterObj.Format(bamSeqAlignment, Constants.SAMTempFileName);

                    // Parse a formatted SAM file.
                    samSeqAlignment = samParserObj.Parse(Constants.SAMTempFileName);

                    // Validate converted SAM file with expected SAM file.
                    Assert.IsTrue(CompareSequencedAlignmentHeader(samSeqAlignment, expextedSamAlignmentObj));

                    // Validate SAM file aligned sequences.
                    Assert.IsTrue(CompareAlignedSequences(samSeqAlignment, expextedSamAlignmentObj));
                }
            }

            // Log message to NUnit GUI.
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "BAM Parser BVT : Validated the BAM->SAM conversion successfully"));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "BAM Parser BVT : Validated the BAM->SAM conversion successfully"));

            // Delete temporary file.
            File.Delete(Constants.SAMTempFileName);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        /// <summary>
        /// Validate BAM to SAM file conversion with DV Enabled.
        /// Input : BAM file.
        /// Output : SAM file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBAMToSAMConversionWithDVEnabled()
        {
            // Get values from xml config file.
            string expectedSamFilePath = _utilityObj._xmlUtil.GetTextValue(Constants.BAMToSAMConversionNode,
               Constants.FilePathNode1);
            string bamFilePath = _utilityObj._xmlUtil.GetTextValue(Constants.BAMToSAMConversionNode,
                Constants.FilePathNode);

            using (BAMParser bamParserObj = new BAMParser())
            {
                using (SAMParser samParserObj = new SAMParser())
                {
                    SAMFormatter samFormatterObj = new SAMFormatter();
                    SequenceAlignmentMap samSeqAlignment = null;
                    SequenceAlignmentMap bamSeqAlignment = null;

                    // Parse expected SAM file.
                    SequenceAlignmentMap expextedSamAlignmentObj = samParserObj.Parse(
                        expectedSamFilePath);

                    // Parse a BAM file.
                    bamParserObj.EnforceDataVirtualization = true;
                    bamSeqAlignment = bamParserObj.Parse(bamFilePath);

                    // Format BAM sequenceAlignment object to SAM file.
                    samFormatterObj.Format(bamSeqAlignment, Constants.SAMTempFileName);

                    // Parse a formatted SAM file.
                    samSeqAlignment = samParserObj.Parse(Constants.SAMTempFileName);

                    // Validate converted SAM file with expected SAM file.
                    Assert.IsTrue(CompareSequencedAlignmentHeader(samSeqAlignment, expextedSamAlignmentObj));

                    // Validate SAM file aligned sequences.
                    Assert.IsTrue(CompareAlignedSequences(samSeqAlignment, expextedSamAlignmentObj));
                }
            }

            // Log message to NUnit GUI.
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "BAM Parser BVT : Validated the BAM->SAM conversion successfully"));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "BAM Parser BVT : Validated the BAM->SAM conversion successfully"));

            // Delete temporary file.
            File.Delete(Constants.SAMTempFileName);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        /// <summary>
        /// Validate SAM to BAM file conversion.
        /// Input : SAM file.
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSAMToBAMConversion()
        {
            // Get values from xml config file.
            string expectedBamFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMToSAMConversionNode, Constants.FilePathNode);
            string samFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.BAMToSAMConversionNode, Constants.FilePathNode1);

            using (BAMParser bamParserObj = new BAMParser())
            {
                using (SAMParser samParserObj = new SAMParser())
                {
                    BAMFormatter bamFormatterObj = new BAMFormatter();
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
                    Assert.IsTrue(CompareSequencedAlignmentHeader(bamSeqAlignment, expextedBamAlignmentObj));

                    // Validate BAM file aligned sequences.
                    Assert.IsTrue(CompareAlignedSequences(bamSeqAlignment, expextedBamAlignmentObj));
                }
            }

            // Log message to NUnit GUI.
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "BAM Parser BVT : Validated the SAM->BAM conversion successfully"));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "BAM Parser BVT : Validated the SAM->BAM conversion successfully"));

            // Delete temporary file.
            File.Delete(Constants.BAMTempFileName);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        #endregion SAM BAM InterConversion Test Cases

        #region Helper Methods

        /// <summary>
        /// Parse BAM and validate parsed aligned sequences and its properties.
        /// </summary>
        /// <param name="nodeName">Different xml nodes used for different test cases</param>
        /// <param name="BAMParserPam">BAM Parse method parameters</param>
        /// <param name="IsEncoding">True for BAMParser ctor with encoding.
        /// False otherwise </param>
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
                        break;
                    case BAMParserParameters.FileName:
                        seqAlignment = bamParser.Parse(bamFilePath);
                        break;
                    case BAMParserParameters.FileNameWithReadOnly:
                        seqAlignment = bamParser.Parse(bamFilePath, false);
                        break;
                    case BAMParserParameters.ParseRangeFileName:
                        seqAlignment = bamParser.ParseRange(bamFilePath,
                            Convert.ToInt32(refIndexValue, (IFormatProvider)null));
                        break;
                    case BAMParserParameters.ParseRangeFileNameWithReadOnly:
                        seqAlignment = bamParser.ParseRange(bamFilePath,
                            Convert.ToInt32(refIndexValue, (IFormatProvider)null), false);
                        break;
                    case BAMParserParameters.ParseRangeWithIndex:
                        seqAlignment = bamParser.ParseRange(bamFilePath,
                            Convert.ToInt32(refIndexValue, (IFormatProvider)null),
                            Convert.ToInt32(startIndexValue, (IFormatProvider)null),
                            Convert.ToInt32(endIndexValue, (IFormatProvider)null), false);
                        break;
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
                            "BAM Parser BVT : Validated Aligned sequence :{0} successfully",
                            alignedSeqs[index].QuerySequence.ToString()));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser BVT : Validated the aligned sequence :{0} successfully",
                            alignedSeqs[index].QuerySequence.ToString()));
                    }
                }
            }
            finally
            {
                bamParser.Dispose();
            }
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
        /// Validate formatted BAM file.
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

            Stream stream = null;
            SequenceAlignmentMap seqAlignment = null;
            using (BAMParser bamParserObj = new BAMParser())
            {
                // Parse a BAM file.
                seqAlignment = bamParserObj.Parse(bamFilePath);

                // Create a BAM formatter object.
                BAMFormatter formatterObj = new BAMFormatter();

                // Write/Format aligned sequences to BAM file.
                switch (BAMParserPam)
                {
                    case BAMParserParameters.StreamWriter:
                        using (stream = new
                            FileStream(Constants.BAMTempFileName,
                            FileMode.Create, FileAccess.Write))
                        {
                            formatterObj.Format(seqAlignment, stream);
                        }
                        break;
                    case BAMParserParameters.FileName:
                        formatterObj.Format(seqAlignment, Constants.BAMTempFileName);
                        break;
                    case BAMParserParameters.IndexFile:
                        formatterObj.Format(seqAlignment, Constants.BAMTempFileName,
                            Constants.BAMTempIndexFile);
                        File.Exists(Constants.BAMTempIndexFile);
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
                    IList<ISequence> expectedSequences = parserObj.Parse(expectedAlignedSeqFilePath);

                    // Validate aligned sequences from BAM file.
                    for (int index = 0; index < alignedSeqs.Count; index++)
                    {
                        Assert.AreEqual(expectedSequences[index].ToString(),
                            alignedSeqs[index].QuerySequence.ToString());

                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Formatter BVT : Validated Aligned sequence :{0} successfully",
                            alignedSeqs[index].QuerySequence.ToString()));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Formatter BVT : Validated the aligned sequence :{0} successfully",
                            alignedSeqs[index].QuerySequence.ToString()));
                    }
                }
            }
            File.Delete(Constants.BAMTempFileName);
            File.Delete(Constants.BAMTempIndexFile);
        }

        /// <summary>
        ///  Comapare Sequence Alignment Header fields
        /// </summary>
        /// <param name="actualAlignment">Actual sequence alignment object</param>
        /// <param name="expectedAlignment">Expected sequence alignment object</param>
        /// <returns></returns>
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
                            "BAM Parser BVT : Sequence alignment header does not match"));
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                           "BAM Parser BVT : Sequence alignment header does not match"));
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
        /// <returns></returns>
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
                           "BAM Parser BVT : Sequence alignment aligned seq does not match"));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                       "BAM Parser BVT : Sequence alignment aligned seq does match"));
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
        private static SequencePointer GetBAMSequencePointer(int LineNumber,
             int startIndex, int endIndex)
        {
            SequencePointer seqPointer = new SequencePointer();
            seqPointer.AlphabetName = "DNA";
            seqPointer.IndexOffsets[0] = startIndex;
            seqPointer.IndexOffsets[1] = endIndex;
            seqPointer.Id = null;
            seqPointer.StartingLine = LineNumber;
            return seqPointer;
        }

        #endregion Helper Methods
    }
}
