// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SAMBvtTestCases.cs
 * 
 *   This file contains the Sam - Parsers and Formatters Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using MBF.Algorithms.Alignment;
using MBF.Encoding;
using MBF.IO;
using MBF.IO.BAM;
using MBF.IO.Fasta;
using MBF.IO.SAM;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.IO.SAM
{
    /// <summary>
    /// SAM Bvt parser and formatter Test case implementation.
    /// </summary>
    [TestClass]
    public class SAMBvtTestCases
    {
        #region Enums

        /// <summary>
        /// Additional parameters to validate different scenarios.
        /// </summary>
        enum ParseOrFormatTypes
        {
            ParseOrFormatText,
            ParseOrFormatTextWithFlag,
            ParseOrFormatFileName,
            ParseOrFormatFileNameWithFlag,
            ParseWithDV,
        }

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\SAMBAMTestData\SAMBAMTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SAMBvtTestCases()
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
        /// Validate SAM Parse(textreader) by parsing dna sam file.
        /// Input : sam file
        /// Output: alignments
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMParserWithTextReader()
        {
            ValidateSAMParser(Constants.SmallSAMFileNode,
                ParseOrFormatTypes.ParseOrFormatText);
        }

        /// <summary>
        /// Validate SAM Parse(textreader, isreadonly) by parsing dna sam file.
        /// Input : sam file
        /// Output: alignments
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateReadOnlySAMParserWithTextReader()
        {
            ValidateSAMParser(Constants.SmallSAMFileNode,
                ParseOrFormatTypes.ParseOrFormatTextWithFlag);
        }

        /// <summary>
        /// Validate SAM Parse(filename) by parsing dna sam file.
        /// Input : sam file
        /// Output: alignments
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMParserWithFileName()
        {
            ValidateSAMParser(Constants.SmallSAMFileNode,
                ParseOrFormatTypes.ParseOrFormatFileName);
        }

        /// <summary>
        /// Validate SAM Parse(filename,isreadonly) by parsing dna sam file.
        /// Input : sam file
        /// Output: alignments
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateReadOnlySAMParserWithFileName()
        {
            ValidateSAMParser(Constants.SmallSAMFileNode,
                ParseOrFormatTypes.ParseOrFormatFileNameWithFlag);
        }

        /// <summary>
        /// Validate SAM ParseOne(textreader) by parsing dna sam file.
        /// Input : sam file
        /// Output: alignments
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMParserParseOneWithTextReader()
        {
            ValidateSAMParserWithParseOne(Constants.SmallSAMFileNode,
                ParseOrFormatTypes.ParseOrFormatText);
        }

        /// <summary>
        /// Validate SAM ParseOne(textreader, isreadonly) by parsing dna sam file.
        /// Input : sam file
        /// Output: alignments
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateReadOnlySAMParserParseOneWithTextReader()
        {
            ValidateSAMParserWithParseOne(Constants.SmallSAMFileNode,
                ParseOrFormatTypes.ParseOrFormatTextWithFlag);
        }

        /// <summary>
        /// Validate SAM ParseOne(filename) by parsing dna sam file.
        /// Input : sam file
        /// Output: alignments
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMParserParseOneWithFileName()
        {
            ValidateSAMParserWithParseOne(Constants.SmallSAMFileNode,
                ParseOrFormatTypes.ParseOrFormatFileName);
        }

        /// <summary>
        /// Validate SAM ParseOne(filename, isReadOnly) by parsing dna sam file.
        /// Input : sam file
        /// Output: alignments
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateReadOnlySAMParserParseOneWithFileName()
        {
            ValidateSAMParserWithParseOne(Constants.SmallSAMFileNode,
                ParseOrFormatTypes.ParseOrFormatFileNameWithFlag);
        }

        /// <summary>
        /// Validate SAMParser(IEncoding) by parsing dna sam file.
        /// Input : sam file
        /// Output: alignments
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMParserWithEncoding()
        {
            ValidateSAMParserWithEncoding(Constants.SmallSAMFileNode,
                Encodings.Ncbi4NA);
        }

        /// <summary>
        /// Validate properties in SAM Parser class
        /// Input : Create a SAM Parser object.
        /// Validation : Validate the properties
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMProperties()
        {
            using (SAMParser parser = new SAMParser())
            {
                Assert.AreEqual(Constants.SAMParserDescription, parser.Description);
                Assert.AreEqual(Constants.SAMFileType, parser.FileTypes);
                Assert.AreEqual(Constants.SAMName, parser.Name);
            }
            Console.WriteLine("Successfully validated all the properties of SAM Parser class.");
            ApplicationLog.WriteLine("Successfully validated all the properties of SAM Parser class.");
        }

        /// <summary>
        /// Validate Parse(reader) by parsing dna sam file.
        /// Input : sam file
        /// Output: Validation of Sequence Alignment Map 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMParserWithReader()
        {
            ValidateSAMParserSeqAlign(Constants.SmallSAMFileNode,
                ParseOrFormatTypes.ParseOrFormatText);
        }

        /// <summary>
        /// Validate ParserSAMHeader by parsing SAM file
        /// Input : SAM file
        /// Output: Validation of Sequence Alignment Header
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMParserHeader()
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSAMFileNode, Constants.FilePathNode);
            string[] expectedHeaderTagValues = _utilityObj._xmlUtil.GetTextValue(
               Constants.SmallSAMFileNode, Constants.RecordTagValuesNode).Split(',');
            string[] expectedHeaderTagKeys = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSAMFileNode, Constants.RecordTagKeysNode).Split(',');
            string[] expectedHeaderTypes = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSAMFileNode, Constants.HeaderTyepsNodes).Split(',');
            SAMAlignmentHeader aligntHeader =
                SAMParser.ParseSAMHeader(filePath);

            int tagKeysCount = 0;
            int tagValuesCount = 0;

            for (int index = 0; index < aligntHeader.RecordFields.Count; index++)
            {
                Assert.AreEqual(expectedHeaderTypes[index].Replace("/", ""),
                     aligntHeader.RecordFields[index].Typecode.ToString((IFormatProvider)null).Replace("/", ""));
                for (int tags = 0; tags < aligntHeader.RecordFields[index].Tags.Count; tags++)
                {
                    Assert.AreEqual(
                        expectedHeaderTagKeys[tagKeysCount].Replace("/", ""),
                        aligntHeader.RecordFields[index].Tags[tags].Tag.ToString((IFormatProvider)null).Replace("/", ""));
                    Assert.AreEqual(
                        expectedHeaderTagValues[tagValuesCount].Replace("/", ""),
                        aligntHeader.RecordFields[index].Tags[tags].Value.ToString((IFormatProvider)null).Replace("/", "").Replace("\r", "").Replace("\n", ""));
                    tagKeysCount++;
                    tagValuesCount++;
                }
            }

        }

        /// <summary>
        /// Validate ParseQualityNSequence() by parsing dna sam file.
        /// Input : sam file
        /// Output: Validation of Sequence Alignment Map 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMParserQualityNSeq()
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.OneEmptySequenceSamFileNode, Constants.FilePathNode);
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.OneEmptySequenceSamFileNode, Constants.ExpectedSequence);

            using (SAMParser parser = new SAMParser())
            {
                SequenceAlignmentMap alignments = null;

                using (TextReader reader = new StreamReader(filePath))
                {
                    alignments = parser.Parse(reader, true);
                }

                Assert.AreEqual(expectedSequence.ToString(),
                            alignments.QuerySequences[0].Sequences[0].ToString());
                Assert.AreEqual(0,
                            alignments.QuerySequences[1].Sequences.Count);
            }
        }

        #endregion

        #region SAM Formatter TestCases

        /// <summary>
        /// Validate SAM Formatter Format(alignment, filename) by parsing and 
        /// formatting the dna sam file
        /// Input : alignment
        /// Output: sam file
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMFormatterWithFileName()
        {
            ValidateSAMFormatter(Constants.SmallSAMFileNode,
                ParseOrFormatTypes.ParseOrFormatFileName);
        }

        /// <summary>
        /// Validate SAM Formatter Format(list of alignments, filename) by parsing and 
        /// formatting the dna sam file
        /// Input : alignment
        /// Output: sam file
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMFormatterWithFileNameAndAlignments()
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSAMFileNode, Constants.FilePathNode);
            ISequenceAlignmentParser parser = new SAMParser();
            try
            {
                IList<ISequenceAlignment> alignments = parser.Parse(filePath);
                SAMFormatter formatter = new SAMFormatter();
                try
                {
                    formatter.Format(alignments, Constants.SAMTempFileName);
                    Assert.Fail();
                }
                catch (NotSupportedException)
                {
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                       "SAM Parser BVT : Validated the exception successfully"));
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "SAM Parser BVT : Validated the exception successfully"));
                }
            }
            finally
            {
                (parser as SAMParser).Dispose();
            }
        }

        /// <summary>
        /// Validate SAM Formatter Format(list of alignments, textwriter) by 
        /// parsing and formatting the dna sam file
        /// Input : alignment
        /// Output: sam file
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMFormatterWithTextWriterAndAlignments()
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSAMFileNode, Constants.FilePathNode);
            ISequenceAlignmentParser parser = new SAMParser();
            try
            {
                IList<ISequenceAlignment> alignments = parser.Parse(filePath);
                SAMFormatter formatter = new SAMFormatter();
                try
                {
                    using (TextWriter writer =
                             new StreamWriter(Constants.SAMTempFileName))
                    {
                        formatter.Format(alignments, writer);
                    }
                    Assert.Fail();
                }
                catch (NotSupportedException)
                {
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                       "SAM Parser BVT : Validated the exception successfully"));
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "SAM Parser BVT : Validated the exception successfully"));
                }

            }
            finally
            {
                (parser as SAMParser).Dispose();
            }
        }

        /// <summary>
        /// Validate SAM Formatter Format(alignment, filename) by 
        /// parsing and formatting the dna sam file
        /// Input : alignment
        /// Output: sam file
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMFormatterWithTextWriter()
        {
            ValidateSAMFormatter(Constants.SmallSAMFileNode,
                ParseOrFormatTypes.ParseOrFormatText);
        }

        /// <summary>
        /// Validate parser and formatter by parsing the sam file with quality values
        /// Input : sam file with quality values
        /// Output: alignment contains qualitative sequences
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMParseAndFormatWithQualityValues()
        {
            ValidateSAMParseAndFormatWithQualityValues(
                Constants.SAMFileWithAllFieldsNode);
        }

        /// <summary>
        /// Validate parser and formatter by parsing the same file which contains 
        /// extended CIGAR string. Validate the CIGAR property in aligned sequence
        /// metadata information is updated as expected.
        /// Input : sam file with CIGAR format
        /// Output: alignment
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMParseAndFormatWithCIGAR()
        {
            ValidateSAMParseAndFormatWithCIGARFormat(
                Constants.SAMFileWithAllFieldsNode);
        }

        /// <summary>
        /// Validate properties in SAM Formatter class
        /// Input : Create a SAM Formatter object.
        /// Validation : Validate the properties
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMFormatterProperties()
        {
            SAMFormatter parser = new SAMFormatter();
            Assert.AreEqual(Constants.SAMFormatterDescription, parser.Description);
            Assert.AreEqual(Constants.SAMFileType, parser.FileTypes);
            Assert.AreEqual(Constants.SAMName, parser.Name);

            Console.WriteLine("Successfully validated all the properties of SAM Parser class.");
            ApplicationLog.WriteLine("Successfully validated all the properties of SAM Parser class.");
        }

        /// <summary>
        /// Validate SAM Formatter Format(sequenceAlignmentMap, writer) by parsing and 
        /// formatting the dna sam file
        /// Input : alignment
        /// Output: sam file
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMFormatterSeqAlignMap()
        {
            ValidateSAMFormatterSeqAlign(Constants.SmallSAMFileNode,
                ParseOrFormatTypes.ParseOrFormatText);
        }

        /// <summary>
        /// Validate SAM Formatter FormatString(IsequenceAlignment) by parsing and 
        /// formatting the dna sam file
        /// Input : alignment
        /// Output: sam file
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMFormatterFormatString()
        {
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SamFormatterFileNode,
                Constants.FilePathNode);
            ISequenceAlignmentParser parser = new SAMParser();
            try
            {
                IList<ISequenceAlignment> alignment = parser.Parse(filePath);

                SAMFormatter formatter = new SAMFormatter();
                string writer = formatter.FormatString(alignment[0]);

                Assert.AreEqual(writer, Constants.FormatterString);
            }
            finally
            {
                (parser as SAMParser).Dispose();
            }
        }

        #endregion

        #endregion

        #region Helper Methods

        /// <summary>
        /// Validate parser parse method overloads with filePath\textreader
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="parseTypes">enum type to execute different overload</param>
        void ValidateSAMParser(string nodeName, ParseOrFormatTypes parseTypes)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedSequenceFile = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            ISequenceAlignmentParser parser = new SAMParser();
            try
            {
                IList<ISequenceAlignment> alignments = null;

                // Parse SAM File
                switch (parseTypes)
                {
                    case ParseOrFormatTypes.ParseOrFormatText:
                        using (TextReader reader = new StreamReader(filePath))
                        {
                            alignments = parser.Parse(reader);
                        }
                        break;
                    case ParseOrFormatTypes.ParseOrFormatTextWithFlag:
                        using (TextReader reader = new StreamReader(filePath))
                        {
                            alignments = parser.Parse(reader, true);
                        }
                        break;
                    case ParseOrFormatTypes.ParseOrFormatFileName:
                        alignments = parser.Parse(filePath);
                        break;
                    case ParseOrFormatTypes.ParseOrFormatFileNameWithFlag:
                        alignments = parser.Parse(filePath, true);
                        break;
                }

                // Get expected sequences
                using (FastaParser parserObj = new FastaParser())
                {
                    IList<ISequence> expectedSequences = parserObj.Parse(expectedSequenceFile);

                    // Validate parsed output with expected output
                    int count = 0;
                    for (int index = 0; index < alignments.Count; index++)
                    {
                        for (int ialigned = 0; ialigned <
                            alignments[index].AlignedSequences.Count; ialigned++)
                        {
                            for (int iseq = 0; iseq <
                                alignments[index].AlignedSequences[ialigned].Sequences.Count; iseq++)
                            {
                                Assert.AreEqual(expectedSequences[count].ToString(),
                                    alignments[index].AlignedSequences[ialigned].Sequences[iseq].ToString());
                                count++;
                            }
                        }
                    }
                }
            }
            finally
            {
                (parser as SAMParser).Dispose();
            }
        }

        /// <summary>
        /// Validate parser parse one method overloads with filePath\textreader
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="parseTypes">enum type to execute different overload</param>
        void ValidateSAMParserWithParseOne(string nodeName,
            ParseOrFormatTypes parseTypes)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedSequenceFile = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            ISequenceAlignmentParser parser = new SAMParser();
            try
            {
                ISequenceAlignment alignment = null;

                // Parse SAM File
                switch (parseTypes)
                {
                    case ParseOrFormatTypes.ParseOrFormatText:
                        using (TextReader reader = new StreamReader(filePath))
                        {
                            alignment = parser.ParseOne(reader);
                        }
                        break;
                    case ParseOrFormatTypes.ParseOrFormatTextWithFlag:
                        using (TextReader reader = new StreamReader(filePath))
                        {
                            alignment = parser.ParseOne(reader, true);
                        }
                        break;
                    case ParseOrFormatTypes.ParseOrFormatFileName:
                        alignment = parser.ParseOne(filePath);
                        break;
                    case ParseOrFormatTypes.ParseOrFormatFileNameWithFlag:
                        alignment = parser.ParseOne(filePath, true);
                        break;
                }

                // Get expected sequences
                using (FastaParser parserObj = new FastaParser())
                {
                    IList<ISequence> expectedSequences = parserObj.Parse(expectedSequenceFile);

                    // Validate parsed output with expected output
                    int count = 0;

                    for (int ialigned = 0; ialigned <
                        alignment.AlignedSequences.Count; ialigned++)
                    {
                        for (int iseq = 0; iseq <
                            alignment.AlignedSequences[ialigned].Sequences.Count; iseq++)
                        {
                            Assert.AreEqual(expectedSequences[count].ToString(),
                                alignment.AlignedSequences[ialigned].Sequences[iseq].ToString());
                            count++;
                        }
                    }
                }
            }
            finally
            {
                (parser as SAMParser).Dispose();
            }
        }

        /// <summary>
        /// Validate formatter all format method overloads with filePath\textwriter
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="formatTypes">enum type to execute different overload</param>
        void ValidateSAMFormatter(string nodeName,
            ParseOrFormatTypes formatTypes)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedSequenceFile = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            ISequenceAlignmentParser parser = new SAMParser();
            try
            {
                IList<ISequenceAlignment> alignments = parser.Parse(filePath);
                SAMFormatter formatter = new SAMFormatter();
                switch (formatTypes)
                {
                    case ParseOrFormatTypes.ParseOrFormatText:
                        using (TextWriter writer =
                            new StreamWriter(Constants.SAMTempFileName))
                        {
                            formatter.Format(alignments[0], writer);
                        }
                        break;
                    case ParseOrFormatTypes.ParseOrFormatFileName:
                        formatter.Format(alignments[0], Constants.SAMTempFileName);
                        break;
                    case ParseOrFormatTypes.ParseOrFormatFileNameWithFlag:
                        formatter.Format(alignments, Constants.SAMTempFileName);
                        break;
                }
                alignments = parser.Parse(Constants.SAMTempFileName);

                // Get expected sequences
                using (FastaParser parserObj = new FastaParser())
                {
                    IList<ISequence> expectedSequences = parserObj.Parse(expectedSequenceFile);

                    // Validate parsed output with expected output
                    int count = 0;
                    for (int index = 0; index < alignments.Count; index++)
                    {
                        for (int ialigned = 0; ialigned <
                            alignments[index].AlignedSequences.Count; ialigned++)
                        {
                            for (int iseq = 0; iseq <
                                alignments[index].AlignedSequences[ialigned].Sequences.Count; iseq++)
                            {
                                Assert.AreEqual(expectedSequences[count].ToString(),
                                    alignments[index].AlignedSequences[ialigned].Sequences[iseq].ToString());
                                count++;
                            }
                        }
                    }
                }
            }
            finally
            {
                (parser as SAMParser).Dispose();
            }
        }

        /// <summary>
        /// Validate parser using specified encoding
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="encoding">encoding instance</param>
        void ValidateSAMParserWithEncoding(string nodeName,
            IEncoding encoding)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedSequenceFile = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);

            // Create parser using encoding
            ISequenceAlignmentParser parser = new SAMParser(encoding);
            try
            {
                IList<ISequenceAlignment> alignments = parser.Parse(filePath);

                // Get expected sequences
                using (FastaParser parserObj = new FastaParser())
                {
                    IList<ISequence> expectedSequences =
                        parserObj.Parse(expectedSequenceFile);

                    // Validate parsed output with expected output
                    int count = 0;
                    for (int index = 0; index < alignments.Count; index++)
                    {
                        for (int ialigned = 0; ialigned <
                            alignments[index].AlignedSequences.Count; ialigned++)
                        {
                            for (int iseq = 0; iseq <
                                alignments[index].AlignedSequences[ialigned].Sequences.Count; iseq++)
                            {
                                Assert.AreEqual(expectedSequences[count].ToString(),
                                    alignments[index].AlignedSequences[ialigned].Sequences[iseq].ToString());
                                count++;
                            }
                        }
                    }
                }
            }
            finally
            {
                (parser as SAMParser).Dispose();
            }
        }

        /// <summary>
        /// Validate parser and formatter by parsing the sam file with quality values
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        void ValidateSAMParseAndFormatWithQualityValues(string nodeName)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedSequenceFile = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string scoreCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ScoresCount);
            // Create parser using encoding
            ISequenceAlignmentParser parser = new SAMParser();
            try
            {
                IList<ISequenceAlignment> alignments = parser.Parse(filePath);

                // Get expected sequences
                using (FastaParser parserObj = new FastaParser())
                {
                    IList<ISequence> expectedSequences =
                        parserObj.Parse(expectedSequenceFile);

                    // Validate parsed output with expected output
                    int count = 0;
                    for (int index = 0; index < alignments.Count; index++)
                    {
                        for (int ialigned = 0; ialigned <
                            alignments[index].AlignedSequences.Count; ialigned++)
                        {
                            for (int iseq = 0; iseq <
                                alignments[index].AlignedSequences[ialigned].Sequences.Count; iseq++)
                            {
                                Assert.IsInstanceOfType(alignments[index].AlignedSequences[ialigned].Sequences[iseq],
                                    typeof(QualitativeSequence));
                                QualitativeSequence qualSequence =
                                 (QualitativeSequence)alignments[index].AlignedSequences[ialigned].Sequences[iseq];
                                Assert.AreEqual(scoreCount, qualSequence.Scores.Length.ToString((IFormatProvider)null));
                                Assert.AreEqual(expectedSequences[count].ToString(), qualSequence.ToString());
                                count++;
                            }
                        }
                    }
                }
            }
            finally
            {
                (parser as SAMParser).Dispose();
            }
        }

        /// <summary>
        /// Validate parser and formatter by parsing the same file which contains 
        /// extended CIGAR string. Validate the CIGAR property in aligned sequence
        /// metadata information is updated as expected.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        void ValidateSAMParseAndFormatWithCIGARFormat(string nodeName)
        {
            // Gets the expected sequence from the Xml
            string filePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedSequenceFile = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string expectedCIGARString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.CIGARNode);
            // Create parser using encoding
            ISequenceAlignmentParser parser = new SAMParser();
            try
            {
                IList<ISequenceAlignment> alignments = parser.Parse(filePath);

                // Get expected sequences
                using (FastaParser parserObj = new FastaParser())
                {
                    IList<ISequence> expectedSequences = parserObj.Parse(expectedSequenceFile);

                    // Validate parsed output with expected output
                    int count = 0;
                    for (int index = 0; index < alignments.Count; index++)
                    {
                        for (int ialigned = 0; ialigned <
                            alignments[index].AlignedSequences.Count; ialigned++)
                        {
                            for (int iseq = 0; iseq <
                                alignments[index].AlignedSequences[ialigned].Sequences.Count; iseq++)
                            {
                                Assert.AreEqual(expectedSequences[count].ToString(),
                                    alignments[index].AlignedSequences[ialigned].Sequences[iseq].ToString());
                                foreach (string key in alignments[index].AlignedSequences[ialigned].Metadata.Keys)
                                {
                                    SAMAlignedSequenceHeader header = (SAMAlignedSequenceHeader)
                                        alignments[index].AlignedSequences[ialigned].Metadata[key];
                                    Assert.AreEqual(expectedCIGARString, header.CIGAR.ToString((IFormatProvider)null));
                                }
                                count++;
                            }
                        }
                    }
                }
            }
            finally
            {
                (parser as SAMParser).Dispose();
            }
        }

        /// <summary>
        /// General method to validate SAM parser method.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="parseTypes">enum type to execute different overload</param>
        void ValidateSAMParserSeqAlign(
            string nodeName,
            ParseOrFormatTypes method)
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
                switch (method)
                {
                    case ParseOrFormatTypes.ParseOrFormatText:
                        using (TextReader reader = new StreamReader(filePath))
                        {
                            alignments = parser.Parse(reader);
                        }
                        break;
                    case ParseOrFormatTypes.ParseOrFormatTextWithFlag:
                        using (TextReader reader = new StreamReader(filePath))
                        {
                            alignments = parser.Parse(reader, true);
                        }
                        break;
                    case ParseOrFormatTypes.ParseOrFormatFileName:
                        alignments = parser.Parse(filePath);
                        break;
                    case ParseOrFormatTypes.ParseOrFormatFileNameWithFlag:
                        alignments = parser.Parse(filePath, true);
                        break;
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
        void ValidateSAMFormatterSeqAlign(
            string nodeName,
            ParseOrFormatTypes parseTypes)
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
                switch (parseTypes)
                {
                    case ParseOrFormatTypes.ParseOrFormatText:
                        using (TextWriter writer =
                            new StreamWriter(Constants.SAMTempFileName))
                        {
                            formatter.Format(alignments, writer);
                        }
                        break;
                    case ParseOrFormatTypes.ParseOrFormatFileName:
                        formatter.Format(alignments, Constants.SAMTempFileName);
                        break;
                    case ParseOrFormatTypes.ParseOrFormatFileNameWithFlag:
                        formatter.Format(alignments, Constants.SAMTempFileName);
                        break;
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

        /// <summary>
        /// Validate SAM to BAM file conversion.
        /// Input : SAM file.
        /// Output : BAM file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMToBAMConversionWithDVEnabled()
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

                    // Enforce DV
                    samParserObj.EnforceDataVirtualization = true;

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

                    // Log message to NUnit GUI.
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "SAM Parser BVT : Validated the SAM->BAM conversion successfully"));
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "SAM Parser BVT : Validated the SAM->BAM conversion successfully"));

                    // Delete temporary file.
                    File.Delete(Constants.BAMTempFileName);
                    ApplicationLog.WriteLine("Deleted the temp file created.");
                }
            }
        }

        /// <summary>
        /// Validate Parse aligned sequence with sequence pointer.
        /// Input : SAM file.
        /// Output :SAM Aligned sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMParseAlignedSeqWithSeqPointer()
        {
            // Get values from XML node.
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SAMFileWithAllFieldsNode, Constants.ExpectedSeqWithPointersNode);
            string samFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SAMFileWithAllFieldsNode, Constants.FilePathNode);
            string lineNumberForPointer = _utilityObj._xmlUtil.GetTextValue(
                Constants.SAMFileWithAllFieldsNode, Constants.LineNumberToPointNode);

            // Parse a SAM file
            using (SAMParser parserObj = new SAMParser())
            {
                parserObj.EnforceDataVirtualization = true;

                SequenceAlignmentMap seqList = parserObj.Parse(samFilePath);
                Assert.IsNotNull(seqList);

                // Get a pointer object
                SequencePointer pointerObj = GetSequencePointer(
                    Int32.Parse(lineNumberForPointer, (IFormatProvider)null));
                pointerObj.IndexOffsets[0] = 156;
                pointerObj.IndexOffsets[1] = 304;

                // Parse a SAM file using Sequence Pointer.
                SAMAlignedSequence alignedSeq = (SAMAlignedSequence)parserObj.ParseAlignedSequence(pointerObj);

                // Validate parsed SAM aligned sequence.
                Assert.AreEqual(expectedSequence,
                    alignedSeq.QuerySequence.ToString());

                Console.WriteLine(string.Format((IFormatProvider)null,
                    "SAM Parser BVT : Sequence alignment aligned seq {0} validate successfully",
                    alignedSeq.Sequences[0].ToString()));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "SAM Parser BVT : Sequence alignment aligned seq validate successfully"));
            }
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
                        "SAM Parser BVT : Sequence alignment aligned seq does not match"));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                       "SAM Parser BVT : Sequence alignment aligned seq does match"));
                    return false;
                }
            }

            return true;
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
                            "SAM Parser BVT : Sequence alignment header does not match"));
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "SAM Parser BVT : Sequence alignment header does not match"));
                        return false;
                    }
                    tagKeysCount++;
                    tagValuesCount++;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets Sequence pointer.
        /// </summary>
        /// <param name="startLine">Set starting index of the pointer</param>
        /// <returns>sequence pointer</returns>
        private static SequencePointer GetSequencePointer(int startLine)
        {
            SequencePointer seqPointer = new SequencePointer();
            seqPointer.AlphabetName = "DNA";
            //seqPointer.EndingIndex = 0;
            seqPointer.StartingLine = startLine;
            seqPointer.Id = null;
            // seqPointer.StartingIndex = 0;
            return seqPointer;
        }
        #endregion
    }
}
