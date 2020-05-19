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
            ParseRangeUsingRefSeq,
            ParseRangeUsingRefSeqAndFlag,
            ParseRangeUsingRefSeqUsingIndex,
            ParseRangeUsingIndexesAndFlag,
            Default
        }

        /// <summary>
        /// PairedReadType method parameters
        /// </summary>
        enum GetPairedReadTypeParameters
        {
            PaireReadTypeUsingLibraryName,
            PaireReadTypeUsingCloneLibraryInfo,
            PaireReadTypeUsingMeanAndDeviation,
            PaireReadTypeUsingReadsAndLibraryInfo,
            PaireReadTypeUsingReadsAndLibrary,
            GetInsertLength,
            Default
        }

        /// <summary>
        /// Get Paired parameters
        /// </summary>
        enum GetPairedReadParameters
        {
            GetPairedReadWithParameters,
            GetPairedReadWithLibraryName,
            GetPairedReadWithCloneLibraryInfo,
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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

        /// <summary>
        /// Validate BAM ParseRange(filename,refSeqName) 
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateBAMParseRangeUsingRefSeqName()
        {
            ValidateBAMParser(Constants.BAMFileWithSequenceRangeRefSeqsNode,
                BAMParserParameters.ParseRangeUsingRefSeq, false, false);
        }

        /// <summary>
        /// Validate BAM ParseRange(filename,refSeqName,flag) 
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateBAMParseRangeUsingRefSeqNameAndFlag()
        {
            ValidateBAMParser(Constants.BAMFileWithSequenceRangeRefSeqsNode,
                BAMParserParameters.ParseRangeUsingRefSeqAndFlag, false, false);
        }

        /// <summary>
        /// Validate BAM ParseRange(filename,refSeqName,Start,end) 
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateBAMParseRangeUsingStartAndEndIndex()
        {
            ValidateBAMParser(Constants.SeqRangeBAMFileNode,
                BAMParserParameters.ParseRangeUsingRefSeqUsingIndex, false, false);
        }

        /// <summary>
        /// Validate BAM ParseRange(filename,refSeqName,Start,end,bool) 
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateBAMParseRangeUsingStartAndEndIndexWithFlag()
        {
            ValidateBAMParser(Constants.SeqRangeBAMFileNode,
                BAMParserParameters.ParseRangeUsingIndexesAndFlag, false, false);
        }

        /// <summary>
        /// Validate BAM GetPairedRead(Mean,Deviation)
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidatePairedReadUsingMeanAndDeviation()
        {
            ValidatePairedReads(Constants.PairedReadForSmallFileNodeName,
                GetPairedReadParameters.GetPairedReadWithParameters);
        }

        /// <summary>
        /// Validate BAM GetPairedRead()
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidatePairedReads()
        {
            ValidatePairedReads(Constants.PairedReadForSmallFileNodeName,
                GetPairedReadParameters.Default);
        }

        /// <summary>
        /// Validate BAM GetPairedRead(libraryName)
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidatePairedReadUsingLibraryName()
        {
            ValidatePairedReads(Constants.PairedReadForSmallFileNodeName,
                GetPairedReadParameters.GetPairedReadWithLibraryName);
        }

        /// <summary>
        /// Validate BAM GetPairedRead(CloneLibraryInfo)
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidatePairedReadUsingCloneLibraryInfo()
        {
            ValidatePairedReads(Constants.PairedReadForSmallFileNodeName,
                GetPairedReadParameters.GetPairedReadWithCloneLibraryInfo);
        }

        /// <summary>
        /// Validate BAM GetPairedReadType(PairedRead,CloneLibraryInfo)
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidatePairedReadTypeUsingCloneLibraryInfo()
        {
            ValidatePairedReadTypes(Constants.PairedReadTypesNode,
                GetPairedReadTypeParameters.PaireReadTypeUsingCloneLibraryInfo);
        }


        /// <summary>
        /// Validate BAM GetPairedReadType(PairedRead,Library)
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidatePairedReadTypeUsingLibrary()
        {
            ValidatePairedReadTypes(Constants.PairedReadTypesNode,
                GetPairedReadTypeParameters.PaireReadTypeUsingLibraryName);
        }

        /// <summary>
        /// Validate BAM GetPairedReadType(PairedRead,Mean,Devn)
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidatePairedReadTypeUsingMeanAndDeviation()
        {
            ValidatePairedReadTypes(Constants.PairedReadTypesForMeanAndDeviationNode,
                GetPairedReadTypeParameters.PaireReadTypeUsingMeanAndDeviation);
        }

        /// <summary>
        /// Validate BAM GetPairedReadType(Read1,Read2,Library)
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidatePairedReadTypeUsingReadsAndLibrary()
        {
            ValidatePairedReadTypes(Constants.PairedReadTypesForLibraryInfoNode,
                GetPairedReadTypeParameters.PaireReadTypeUsingReadsAndLibrary);
        }

        /// <summary>
        /// Validate BAM GetPairedReadType(Read1,Read2,CloneLibraryInfo)
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidatePairedReadTypeUsingReadsAndCloneLibraryInfo()
        {
            ValidatePairedReadTypes(Constants.PairedReadTypesForLibraryInfoNode,
                GetPairedReadTypeParameters.PaireReadTypeUsingReadsAndLibraryInfo);
        }

        /// <summary>
        /// Validate BAM GetInsertLength(Read1,Read2)
        /// by passing valid BAM file
        /// Input : Small size BAM file.
        /// Output : Validation of aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateInsertLengthForPairedReads()
        {
            ValidatePairedReadTypes(Constants.PairedReadTypesForLibraryInfoNode,
                GetPairedReadTypeParameters.GetInsertLength);
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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
        [TestCategory("Priority0")]
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
            string refSeqName = _utilityObj._xmlUtil.GetTextValue(
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
                    case BAMParserParameters.ParseRangeUsingRefSeq:
                        seqAlignment = bamParser.ParseRange(bamFilePath, refSeqName);
                        break;
                    case BAMParserParameters.ParseRangeUsingRefSeqAndFlag:
                        seqAlignment = bamParser.ParseRange(bamFilePath, refSeqName, true);
                        break;
                    case BAMParserParameters.ParseRangeUsingRefSeqUsingIndex:
                        seqAlignment = bamParser.ParseRange(bamFilePath, refSeqName,
                            Convert.ToInt32(startIndexValue, (IFormatProvider)null),
                            Convert.ToInt32(endIndexValue, (IFormatProvider)null));
                        break;
                    case BAMParserParameters.ParseRangeUsingIndexesAndFlag:
                        seqAlignment = bamParser.ParseRange(bamFilePath, refSeqName,
                            Convert.ToInt32(startIndexValue, (IFormatProvider)null),
                            Convert.ToInt32(endIndexValue, (IFormatProvider)null), true);
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
                    arecordFields[index].Typecode.ToString((IFormatProvider)null), StringComparison.CurrentCulture))
                {
                    return false;
                }
                for (int tags = 0; tags < expectedrecordFields[index].Tags.Count; tags++)
                {
                    if ((0 != string.Compare(expectedrecordFields[index].Tags[tags].Tag.ToString((IFormatProvider)null),
                        arecordFields[index].Tags[tags].Tag.ToString((IFormatProvider)null), StringComparison.CurrentCulture))
                        || (0 != string.Compare(expectedrecordFields[index].Tags[tags].Value.ToString((IFormatProvider)null),
                        arecordFields[index].Tags[tags].Value.ToString((IFormatProvider)null), StringComparison.CurrentCulture)))
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
                    actualAlignedSeqs[0].QuerySequence.ToString(), StringComparison.CurrentCulture))
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

        /// <summary>
        /// Validate GetPaired method
        /// </summary>
        /// <param name="nodeName">XML node name</param>
        /// <param name="pams">GetPairedReads method parameters</param>
        void ValidatePairedReads(string nodeName, GetPairedReadParameters pams)
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string expectedAlignedSeqFilePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string mean = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.MeanNode);
            string deviation = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DeviationValueNode);
            string library = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.LibraryNameNode);
            string pairedReadsCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.PairedReadsNode);
            string[] insertLength = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.InsertLengthNode).Split(',');
            string[] pairedReadType = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.PairedReadTypeNode).Split(',');

            SequenceAlignmentMap seqAlignment = null;
            IList<PairedRead> pairedReads = null;
            BAMParser bamParser = new BAMParser();
            FastaParser parserObj = new FastaParser();

            try
            {
                seqAlignment = bamParser.Parse(bamFilePath);
                IList<ISequence> expectedSequences = parserObj.Parse(expectedAlignedSeqFilePath);

                switch (pams)
                {
                    case GetPairedReadParameters.GetPairedReadWithParameters:
                        pairedReads = seqAlignment.GetPairedReads(float.Parse(mean, (IFormatProvider)null),
                            float.Parse(deviation, (IFormatProvider)null));
                        break;
                    case GetPairedReadParameters.GetPairedReadWithLibraryName:
                        pairedReads = seqAlignment.GetPairedReads(library);
                        break;
                    case GetPairedReadParameters.GetPairedReadWithCloneLibraryInfo:
                        CloneLibraryInformation libraryInfo =
                            CloneLibrary.Instance.GetLibraryInformation(library);
                        pairedReads = seqAlignment.GetPairedReads(libraryInfo);
                        break;
                    case GetPairedReadParameters.Default:
                        pairedReads = seqAlignment.GetPairedReads();
                        break;
                }

                Assert.AreEqual(pairedReadsCount, pairedReads.Count.ToString());

                int i = 0;
                foreach (PairedRead read in pairedReads)
                {
                    Assert.AreEqual(insertLength[i], read.InsertLength.ToString());
                    Assert.AreEqual(pairedReadType[i], read.PairedType.ToString());

                    foreach (SAMAlignedSequence seq in read.Reads)
                    {
                        Assert.AreEqual(expectedSequences[i].ToString(),
                            seq.QuerySequence.ToString());

                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser BVT : Validated Paired read :{0} successfully",
                            seq.QuerySequence.ToString()));
                    }
                    i++;

                }
            }

            finally
            {
                bamParser.Dispose();
            }
        }

        /// <summary>
        /// Validate different paired read types
        /// </summary>
        /// <param name="nodeName">XML node name</param>
        /// <param name="pams">GetPairedReadTypes method parameters</param>
        void ValidatePairedReadTypes(string nodeName, GetPairedReadTypeParameters pams)
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string mean = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.MeanNode);
            string deviation = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DeviationValueNode);
            string library = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.LibraryNameNode);
            string[] pairedReadType = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.PairedReadTypeNode).Split(',');
            string[] insertLength = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.InsertLengthNode).Split(',');

            IList<PairedRead> pairedReads = null;
            BAMParser bamParser = new BAMParser();
            SequenceAlignmentMap seqAlignmentMapObj = bamParser.Parse(bamFilePath);
            CloneLibraryInformation libraryInfo;
            int i = 0;
            try
            {
                switch (pams)
                {
                    case GetPairedReadTypeParameters.PaireReadTypeUsingLibraryName:
                        pairedReads = seqAlignmentMapObj.GetPairedReads(float.Parse(mean, (IFormatProvider)null),
                            float.Parse(deviation, (IFormatProvider)null));
                        foreach (PairedRead read in pairedReads)
                        {
                            PairedReadType type = PairedRead.GetPairedReadType(read, library);
                            Assert.AreEqual(type.ToString(), pairedReadType[i]);
                            i++;
                        }
                        break;
                    case GetPairedReadTypeParameters.PaireReadTypeUsingCloneLibraryInfo:
                        pairedReads = seqAlignmentMapObj.GetPairedReads(float.Parse(mean, (IFormatProvider)null),
                            float.Parse(deviation, (IFormatProvider)null));
                        libraryInfo = CloneLibrary.Instance.GetLibraryInformation(library);
                        foreach (PairedRead read in pairedReads)
                        {
                            PairedReadType type = PairedRead.GetPairedReadType(read, libraryInfo);
                            Assert.AreEqual(type.ToString(), pairedReadType[i]);
                            i++;

                        }
                        break;
                    case GetPairedReadTypeParameters.PaireReadTypeUsingMeanAndDeviation:
                        pairedReads = seqAlignmentMapObj.GetPairedReads(float.Parse(mean, (IFormatProvider)null),
                            float.Parse(deviation, (IFormatProvider)null));
                        foreach (PairedRead read in pairedReads)
                        {
                            PairedReadType type = PairedRead.GetPairedReadType(read, float.Parse(mean, (IFormatProvider)null),
                                float.Parse(deviation, (IFormatProvider)null));
                            Assert.AreEqual(type.ToString(), pairedReadType[i]);
                            i++;

                        }
                        break;
                    case GetPairedReadTypeParameters.PaireReadTypeUsingReadsAndLibrary:
                        pairedReads = seqAlignmentMapObj.GetPairedReads(float.Parse(mean, (IFormatProvider)null),
                            float.Parse(deviation, (IFormatProvider)null));
                        foreach (PairedRead read in pairedReads)
                        {
                            PairedReadType type = PairedRead.GetPairedReadType(read.Read1,
                                read.Read2, library);
                            Assert.AreEqual(type.ToString(), pairedReadType[i]);
                            i++;

                        }
                        break;
                    case GetPairedReadTypeParameters.PaireReadTypeUsingReadsAndLibraryInfo:
                        pairedReads = seqAlignmentMapObj.GetPairedReads(float.Parse(mean, (IFormatProvider)null),
                            float.Parse(deviation, (IFormatProvider)null));
                        libraryInfo = CloneLibrary.Instance.GetLibraryInformation(library);
                        foreach (PairedRead read in pairedReads)
                        {
                            PairedReadType type = PairedRead.GetPairedReadType(read.Read1,
                                   read.Read2, libraryInfo);
                            Assert.AreEqual(type.ToString(), pairedReadType[i]);
                            i++;
                        }
                        break;
                    case GetPairedReadTypeParameters.GetInsertLength:
                        pairedReads = seqAlignmentMapObj.GetPairedReads(float.Parse(mean, (IFormatProvider)null),
                            float.Parse(deviation, (IFormatProvider)null));
                        libraryInfo = CloneLibrary.Instance.GetLibraryInformation(library);
                        foreach (PairedRead read in pairedReads)
                        {
                            int length = PairedRead.GetInsertLength(read.Read1, read.Read2);
                            Assert.AreEqual(length.ToString((IFormatProvider)null), insertLength[i]);
                            i++;

                        }
                        break;
                }
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser BVT : Validated Paired read Type Successfully"));
            }

            finally
            {
                bamParser.Dispose();
            }
        }
        #endregion Helper Methods
    }
}
