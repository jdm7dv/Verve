// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SAMP1TestCases.cs
 * 
 *   This file contains the Sam - Parsers and Formatters P1 test cases.
 * 
***************************************************************************/

using System.Collections.Generic;
using System.IO;

using MBF.Algorithms.Alignment;
using MBF.Encoding;
using MBF.IO.Fasta;
using MBF.IO.SAM;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.IO;
using System;

namespace MBF.TestAutomation.IO.SAM
{
    /// <summary>
    /// SAM P1 parser and formatter Test case implementation.
    /// </summary>
    [TestClass]
    public class SAMP1TestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\SAMBAMTestData\SAMBAMTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SAMP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Test Cases

        #region SAM Parser TestCases

        /// <summary>
        /// Validate Parse(reader) by parsing medium size
        /// dna sam file.
        /// Input : medium size sam file
        /// Output: Validation of Sequence Alignment Map 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSAMParserWithReader()
        {
            ValidateSAMParser(Constants.SAMFileWithRefNode);
        }

        /// <summary>
        /// Validate Parse(mbfReader, isReadOnly) by parsing empty
        /// SAM file.
        /// Input : Empty file
        /// Output: Validation of null Sequence Alignment Map 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSAMParserWithEmptyAlignmentMap()
        {
            using (SAMParser parser = new SAMParser())
            {
                SequenceAlignmentMap alignment =
                    parser.Parse(_utilityObj._xmlUtil.GetTextValue(
                    Constants.EmptySamFileNode,
                    Constants.FilePathNode));

                Assert.AreEqual(null, alignment);
            }
        }

        /// <summary>
        /// Validate Parse medium size(100MB) SAM file 
        /// with DV enabled and vlaidate the parsed aligned sequence.
        /// Input : medium size sam file
        /// Output: Validation of Sequence Alignment Map 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateParseMediumSizeSAMFileWithDVEnabled()
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.MediumSizeFileNode, Constants.FilePathNode);
            string expectedAlignedSeqCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.MediumSizeFileNode, Constants.ExpectedAlignedSeqCountNode);
            string expectedFirstAlignedSeq = _utilityObj._xmlUtil.GetTextValue(
                Constants.MediumSizeFileNode, Constants.FirstBlockSequenceNode);
            string expectedMiddleBlockAlignedSeq = _utilityObj._xmlUtil.GetTextValue(
                Constants.MediumSizeFileNode, Constants.MiddleBlockSequenceNode);
            string expectedLastAlignedSeq = _utilityObj._xmlUtil.GetTextValue(
                Constants.MediumSizeFileNode, Constants.LastBlockSequenceNode);

            using (SAMParser parser = new SAMParser())
            {
                SequenceAlignmentMap alignments = null;

                // Enable DV
                parser.EnforceDataVirtualization = true;

                // Parse SAM File
                alignments = parser.Parse(filePath);

                IList<SAMAlignedSequence> alignedSeq = alignments.QuerySequences;

                // Validate different aligned sequence by fetching from DV
                Assert.AreEqual(expectedAlignedSeqCount,
                        alignments.QuerySequences.Count.ToString((IFormatProvider)null));
                Assert.AreEqual(expectedFirstAlignedSeq,
                    alignedSeq[0].QuerySequence.ToString());
                Assert.AreEqual(expectedMiddleBlockAlignedSeq,
                    alignedSeq[1165].QuerySequence.ToString());
                Assert.AreEqual(expectedLastAlignedSeq,
                    alignedSeq[alignedSeq.Count - 1].QuerySequence.ToString());

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "SAM P1 Cases : Validated the SAM Parse successfully"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "SAM P1 Cases : Validated the SAM Parse successfully"));
            }
        }

        /// <summary>
        /// Parse a valid Sam File and identify the parser,
        /// Parse the file and validate if isc is created.
        /// Input : Valid Sam File.
        /// Output : Validation of ISC file creation.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSamParserWithDVIscFile()
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
              Constants.SAMFileWithRefNode, Constants.FilePathNode);

            ISequenceAlignmentParser iSeqParser =
                SequenceAlignmentParsers.FindParserByFile(filePath);

            if (null != iSeqParser)
            {
                IVirtualSequenceAlignmentParser vParserObj =
                    iSeqParser as IVirtualSequenceAlignmentParser;
                if (null != vParserObj)
                {
                    vParserObj.EnforceDataVirtualization = true;
                }
                else
                {
                    Assert.Fail("SAM P1 Cases : Could not find the SAM Parser Object.");
                }

                string iscFilePath =
                    string.Concat(filePath, ".isc");

                iSeqParser.Parse(filePath);

                if (File.Exists(iscFilePath))
                {
                    Console.WriteLine(
                        "SAM P1 Cases : DV enabled as expected and isc file created successfully.");
                    ApplicationLog.WriteLine(
                        "SAM P1 Cases : DV enabled as expected and isc file created successfully.");
                }
                else
                {
                    Assert.Fail("SAM P1 Cases : DV not enabled as expected.");
                }
            }
            else
            {
                Assert.Fail("SAM P1 Cases : Could not find the SAM file");
            }
        }

        #endregion

        #region Formatter TestCases

        /// <summary>
        /// Validate SAM Formatter Format(sequenceAlignmentMap, writer) by
        /// parsing and formatting the medium size dna sam file
        /// Input : alignment
        /// Output: sam file
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSAMFormatterSeqAlignReader()
        {
            ValidateSAMFormatter(Constants.SAMFileWithAllFieldsNode);
        }

        #endregion Formatter TestCases

        #endregion Test Cases

        #region Helper Methods

        /// <summary>
        /// General method to validate SAM parser method.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="parseTypes">enum type to execute different overload</param>
        void ValidateSAMParser(string nodeName)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedSequenceFile = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            using (SAMParser parser = new SAMParser())
            {
                SequenceAlignmentMap alignments = null;

                // Parse SAM File
                using (TextReader reader = new StreamReader(filePath))
                {
                    alignments = parser.Parse(reader);
                }

                // Get expected sequences
                using (FastaParser parserObj = new FastaParser())
                {
                    IList<ISequence> expectedSequences =
                        parserObj.Parse(expectedSequenceFile);

                    // Validate parsed output with expected output
                    for (int index = 0;
                        index < alignments.QuerySequences.Count;
                        index++)
                    {
                        for (int count = 0;
                            count < alignments.QuerySequences[index].Sequences.Count;
                            count++)
                        {
                            Assert.AreEqual(expectedSequences[index].ToString(),
                                alignments.QuerySequences[index].Sequences[count].ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// General method to validate SAM Formatter method.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="parseTypes">enum type to execute different overload</param>
        void ValidateSAMFormatter(string nodeName)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedSequenceFile = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            using (SAMParser parser = new SAMParser())
            {
                SequenceAlignmentMap alignments = parser.Parse(filePath);
                SAMFormatter formatter = new SAMFormatter();

                using (TextWriter writer =
                            new StreamWriter(Constants.SAMTempFileName))
                {
                    formatter.Format(alignments, writer);
                }

                alignments = parser.Parse(Constants.SAMTempFileName);

                // Get expected sequences
                using (FastaParser parserObj = new FastaParser())
                {
                    IList<ISequence> expectedSequences =
                        parserObj.Parse(expectedSequenceFile);

                    // Validate parsed output with expected output
                    for (int index = 0;
                        index < alignments.QuerySequences.Count;
                        index++)
                    {
                        for (int count = 0;
                            count < alignments.QuerySequences[index].Sequences.Count;
                            count++)
                        {
                            Assert.AreEqual(expectedSequences[index].ToString(),
                                alignments.QuerySequences[index].Sequences[count].ToString());
                        }
                    }
                }
            }
        }

        #endregion
    }
}
