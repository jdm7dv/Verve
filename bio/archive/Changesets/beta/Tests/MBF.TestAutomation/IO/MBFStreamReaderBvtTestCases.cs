// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * MBFStreamReaderBvtTestCases.cs
 * 
 *   This file contains the MBFStreamReader Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.Util.Logging;
using MBF.TestAutomation.Util;
using MBF.IO;
using System.IO;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// MBFStreamReader Bvt Test case implementation.
    /// </summary>
    [TestClass]
    public class MBFStreamReaderBvtTestCases
    {
        #region Enums

        /// <summary>
        /// MBFStreamInput parameters used for different test cases.
        /// </summary>
        enum StreamReaderInputType
        {
            FileName,
            FileNameWithSkipBlankLines,
            Stream,
            StreamWithSkipBlankLines
        }

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\MBFStreamReaderConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static MBFStreamReaderBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region MBFStreamReader Test cases

        /// <summary>
        /// Validate read FastA file
        /// Input : FastA file
        /// Ouput : Validation of FastA file read
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateFastAStream()
        {
            ValidateMBFStreamReader(
                Constants.SimpleFastAStreamReaderNode,
                StreamReaderInputType.FileName);
        }

        /// <summary>
        /// Validate read FastQ file
        /// Input : FastQ file
        /// Ouput : Validation of FastQ file read
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateFastQStream()
        {
            ValidateMBFStreamReader(
                Constants.SimpleFastQStreamReaderNode,
                StreamReaderInputType.FileName);
        }

        /// <summary>
        /// Validate read SAM file
        /// Input : SAM file
        /// Ouput : Validation of SAM file read
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMStream()
        {
            ValidateMBFStreamReader(
                Constants.SimpleSAMStreamReaderNode,
                StreamReaderInputType.FileName);
        }

        /// <summary>
        /// Validate read FastA file with skipping blank lines
        /// Input : FastA file
        /// Ouput : Validation of FastA file read
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateFastAStreamWithSkipBlankLines()
        {
            ValidateMBFStreamReader(
                Constants.SimpleFastAStreamReaderWithBlankLinesNode,
                StreamReaderInputType.FileNameWithSkipBlankLines);
        }

        /// <summary>
        /// Validate read FastQ file with skipping blank lines
        /// Input : FastQ file
        /// Ouput : Validation of FastQ file read
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateFastQStreamWithSkipBlankLines()
        {
            ValidateMBFStreamReader(
                Constants.SimpleFastQStreamReaderWithBlankLinesNode,
                StreamReaderInputType.FileNameWithSkipBlankLines);
        }

        /// <summary>
        /// Validate read SAM file with skipping blank lines
        /// Input : SAM file
        /// Ouput : Validation of SAM file read
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMStreamWithSkipBlankLines()
        {
            ValidateMBFStreamReader(
                Constants.SimpleSAMStreamReaderWithBlankLinesNode,
                StreamReaderInputType.FileNameWithSkipBlankLines);
        }

        /// <summary>
        /// Validate read FastA file using BioStream(stream)
        /// Input : FastA file
        /// Ouput : Validation of FastA file read
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateFastAStreamUsingStream()
        {
            ValidateMBFStreamReader(
                Constants.SimpleFastAStreamReaderWithBlankLinesNode,
                StreamReaderInputType.Stream);
        }

        /// <summary>
        /// Validate read FastQ file using BioStream(stream)
        /// Input : FastQ file
        /// Ouput : Validation of FastQ file read
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateFastQStreamUsingStream()
        {
            ValidateMBFStreamReader(
                Constants.SimpleFastQStreamReaderWithBlankLinesNode,
                StreamReaderInputType.Stream);
        }

        /// <summary>
        /// Validate read SAM file using BioStream(stream)
        /// Input : SAM file
        /// Ouput : Validation of SAM file read
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMStreamUsingStream()
        {
            ValidateMBFStreamReader(
                Constants.SimpleSAMStreamReaderWithBlankLinesNode,
                StreamReaderInputType.Stream);
        }

        /// <summary>
        /// Validate read FastA file using BioStream(stream,SkipBlankLines)
        /// Input : FastA file
        /// Ouput : Validation of FastA file read
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateFastAStreamUsingStreamWithSkipBlankLines()
        {
            ValidateMBFStreamReader(
                Constants.SimpleFastAStreamReaderWithBlankLinesNode,
                StreamReaderInputType.StreamWithSkipBlankLines);
        }

        /// <summary>
        /// Validate read FastQ file using BioStream(stream,SkipBlankLines)
        /// Input : FastQ file
        /// Ouput : Validation of FastQ file read
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateFastQStreamUsingStreamWithSkipBlankLines()
        {
            ValidateMBFStreamReader(
                Constants.SimpleFastQStreamReaderWithBlankLinesNode,
                StreamReaderInputType.StreamWithSkipBlankLines);
        }

        /// <summary>
        /// Validate read SAM file using BioStream(stream,SkipBlankLines)
        /// Input : SAM file
        /// Ouput : Validation of SAM file read
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSAMStreamUsingStreamWithSkipBlankLines()
        {
            ValidateMBFStreamReader(
                Constants.SimpleSAMStreamReaderWithBlankLinesNode,
                StreamReaderInputType.StreamWithSkipBlankLines);
        }

        /// <summary>
        /// Validate sub string of current line of FastA stream
        /// Input : FastA file
        /// Output : Substring validation.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateLineSubstringForFastAStream()
        {
            ValidateSubString(Constants.SimpleFastAStreamReaderNode,
                false);
        }

        /// <summary>
        /// Validate sub string of current line of FastQ stream
        /// Input : FastQ file
        /// Output : Substring validation.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateLineSubstringForFastQStream()
        {
            ValidateSubString(Constants.SimpleFastQStreamReaderNode,
                true);
        }

        /// <summary>
        /// Validate sub string of current line of SAM file stream
        /// Input : SAM file
        /// Output : Substring validation.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateLineSubstringForSAMFileStream()
        {
            ValidateSubString(Constants.SimpleSAMStreamReaderNode,
                true);
        }

        /// <summary>
        /// Validate current line characters for FastA file stream
        /// Input : FastA file
        /// Output : Current line characters validation
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateLineCharsForFastAStream()
        {
            ValidateChars(Constants.SimpleFastAStreamReaderNode);
        }

        /// <summary>
        /// Validate current line characters for FastQ file stream
        /// Input : FastQ file
        /// Output : Current line characters validation
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateLineCharsForFastQStream()
        {
            ValidateChars(Constants.SimpleFastQStreamReaderNode);
        }

        /// <summary>
        /// Validate current line characters for SAM file stream
        /// Input : SAM file
        /// Output : Current line characters validation
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateLineCharsForSAMStream()
        {
            ValidateChars(Constants.SimpleSAMStreamReaderNode);
        }

        /// <summary>
        /// Validate MBFStreamReader class properties.
        /// Input : fastA file
        /// Output : Validate properties.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateMBFStreamReaderProperties()
        {
            // Get values from xml
            string FilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastAStreamReaderNode, Constants.FilePathNode);
            string newLineCharsCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastAStreamReaderNode, Constants.NewLineCharacterCountNode);
            string pos = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastAStreamReaderNode, Constants.PositionNode);
            string startingIndex = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastAStreamReaderNode, Constants.CurrentLineStartingIndexNode);

            using (MBFStreamReader streamReader = new MBFStreamReader(FilePath, true))
            {
                // Validate Properties
                Assert.IsTrue(streamReader.CanRead);
                Assert.IsTrue(streamReader.SkipBlankLines);
                Assert.IsTrue(streamReader.HasLines);
                Assert.AreEqual(newLineCharsCount,
                    streamReader.NewLineCharacterCount.ToString((IFormatProvider)null));
                Assert.AreEqual(pos,
                    streamReader.Position.ToString((IFormatProvider)null));
                Assert.AreEqual(newLineCharsCount,
                    streamReader.NewLineCharacterCount.ToString((IFormatProvider)null));
                Assert.AreEqual(startingIndex,
                    streamReader.CurrentLineStartingIndex.ToString((IFormatProvider)null));

                Console.WriteLine("Validated the StreamReader properties successfully");
                ApplicationLog.WriteLine("Validated the StreamReader properties successfully");
            }
        }

        /// <summary>
        /// Validate position of the reader
        /// Input : FastA file
        /// output : Validation of reader position
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidatePosition()
        {
            string FilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastAStreamReaderNode, Constants.FilePathNode);
            string pos = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastAStreamReaderNode, Constants.PositionNode);

            using (MBFStreamReader reader = new MBFStreamReader(FilePath))
            {
                // Set position at the begining.
                reader.Seek(Int32.Parse(pos, (IFormatProvider)null), SeekOrigin.Begin);

                // Validate the set position
                Assert.AreEqual(pos, reader.Position.ToString((IFormatProvider)null));

                Console.WriteLine("Validate the position successfulyy");
                ApplicationLog.WriteLine("Validated the position successfully");
            }
        }

        #endregion MBFStreamReader Test cases

        #region MBFTextReader Test cases

        /// <summary>
        /// Validate MBFTextReader all constructor.
        /// Input : GenBank file
        /// output : Validation of MBFTextReader all constructor.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateMBFTextReaderConstructor()
        {
            string FilePath = _utilityObj._xmlUtil.GetTextValue(
               Constants.MBFTextReaderValidationNode, Constants.FilePathNode);
            string expectedLine = _utilityObj._xmlUtil.GetTextValue(
                Constants.MBFTextReaderValidationNode, Constants.ExpectedLineNode);
            string expectedHeader = _utilityObj._xmlUtil.GetTextValue(
               Constants.MBFTextReaderValidationNode, Constants.ExpectedHeaderNode);

            // MBFTextReader(string).
            using (MBFTextReader mbfReader = new MBFTextReader(FilePath))
            {
                Assert.AreEqual(expectedHeader, mbfReader.ReadBlock(0, 0, 32, 5));
                Assert.AreEqual(FilePath, mbfReader.FileName);
                Assert.AreEqual(expectedLine, mbfReader.Line);
            }

            // MBFTextReader(Stream).
            using (FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                MBFTextReader mbfReader = new MBFTextReader(stream);
                Assert.AreEqual(expectedHeader, mbfReader.LineHeader);
                Assert.IsNull(mbfReader.FileName);
            }

            // MBFTextReader(TextReader).
            using (StreamReader reader = new StreamReader(FilePath))
            {
                MBFTextReader mbfReader = new MBFTextReader(reader);

                // Start reading from begining
                Assert.AreEqual(expectedHeader, mbfReader.LineHeader);
                Assert.IsNull(mbfReader.FileName);

                // skip line.
                mbfReader.SkipToNextSection();
                Assert.IsNotNull(mbfReader.Line);
            }

            // Data indent specifies the number of chars that are considered the line header.
            int dataIndent = 2;
            expectedHeader = expectedHeader.Substring(0, 2);

            // MBFTextReader(string,DataIndent).
            using (MBFTextReader mbfReader = new MBFTextReader(FilePath, dataIndent))
            {
                Assert.AreEqual(expectedHeader, mbfReader.LineHeader);
                Assert.AreEqual(FilePath, mbfReader.FileName);
            }

            // MBFTextReader(string,DataIndent,bool skipLines).
            using (MBFTextReader mbfReader = new MBFTextReader(FilePath, dataIndent, false))
            {
                Assert.AreEqual(expectedHeader, mbfReader.LineHeader);
                Assert.AreEqual(FilePath, mbfReader.FileName);
            }


            // MBFTextReader(Stream, int) .
            using (FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                MBFTextReader mbfReader = new MBFTextReader(stream, dataIndent);
                Assert.AreEqual(expectedHeader, mbfReader.LineHeader);
                Assert.IsNull(mbfReader.FileName);
            }

            // MBFTextReader(Stream, int, bool) .
            using (FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                MBFTextReader mbfReader = new MBFTextReader(stream, dataIndent, true);
                Assert.AreEqual(expectedHeader, mbfReader.LineHeader);
                Assert.IsNull(mbfReader.FileName);
            }

            // MBFTextReader(TextReader).
            using (StreamReader reader = new StreamReader(FilePath))
            {
                MBFTextReader mbfReader = new MBFTextReader(reader, dataIndent);
                Assert.AreEqual(expectedHeader, mbfReader.LineHeader);
                Assert.IsNull(mbfReader.FileName);
            }

            // MBFTextReader(TextReader,skipLines).
            using (StreamReader reader = new StreamReader(FilePath))
            {
                MBFTextReader mbfReader = new MBFTextReader(reader, dataIndent, true);
                Assert.AreEqual(expectedHeader, mbfReader.LineHeader);
                Assert.IsNull(mbfReader.FileName);
            }

        }
        #endregion MBFTextReader Test cases

        #region Helper Methods

        /// <summary>
        /// Validate Read Biological sequences using MBFStreamReader
        /// </summary>
        /// <param name="nodeName">Name of the node used for different test case.</param>
        /// <param name="inputType">Different streaming ipnuts used for different test cases</param>
        void ValidateMBFStreamReader(string nodeName,
           StreamReaderInputType inputType)
        {
            // Get values from xml
            string FilePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string[] expectedOutput = _utilityObj._xmlUtil.GetTextValues(
                nodeName, Constants.ExpectedLinesNode);
            MBFStreamReader streamReader = null;
            try
            {
                // Read Fasta file.
                switch (inputType)
                {
                    case StreamReaderInputType.FileName:
                        streamReader = new MBFStreamReader(FilePath);
                        break;
                    case StreamReaderInputType.FileNameWithSkipBlankLines:
                        streamReader = new MBFStreamReader(FilePath, true);
                        break;
                    case StreamReaderInputType.Stream:
                        using (Stream stream = new FileStream(FilePath, FileMode.Open,
                           FileAccess.ReadWrite))
                        {
                            streamReader = new MBFStreamReader(stream);
                        }
                        break;
                    case StreamReaderInputType.StreamWithSkipBlankLines:
                        using (Stream stream = new FileStream(FilePath, FileMode.Open,
                            FileAccess.ReadWrite))
                        {
                            streamReader = new MBFStreamReader(stream, true);
                        }
                        break;
                }
                for (int i = 0; i < expectedOutput.Length; i++)
                {
                    Assert.AreEqual(expectedOutput[i], streamReader.Line);

                    Console.WriteLine("Validated the line {0} successfully", streamReader.Line);
                    ApplicationLog.WriteLine("Validated the MBF StreamReader successfully");

                    // Move to next line
                    streamReader.GoToNextLine();
                }
            }
            finally
            {
                if (streamReader != null)
                    streamReader.Dispose();
            }
        }

        /// <summary>
        /// Validate Read Biological sequences using MBFStreamReader
        /// </summary>
        /// <param name="nodeName">Name of the node used for different test case.</param>
        /// <param name="IsStartAndEndIndex">True if validating from start to end index substring,
        /// else false</param>
        void ValidateSubString(string nodeName,
            bool IsStartAndEndIndex)
        {
            // Get values from xml
            string FilePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedString);
            string startIndex = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.StartIndexNode);
            string endIndex = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.EndIndexNode);

            string subString = string.Empty;

            using (MBFStreamReader streamReader = new MBFStreamReader(FilePath))
            {
                if (IsStartAndEndIndex)
                {
                    subString = streamReader.GetLineField(Int32.Parse(startIndex, (IFormatProvider)null),
                        Int32.Parse(endIndex, (IFormatProvider)null));
                }
                else
                {
                    subString = streamReader.GetLineField(Int32.Parse(startIndex, (IFormatProvider)null));
                }

                // Validate sub string of a line.
                Assert.AreEqual(expectedString, subString);
                Console.WriteLine("The expected substring is {0}", subString);
                ApplicationLog.WriteLine("Validated the substring successfully");
            }
        }

        /// <summary>
        /// Validate Read characters from curent line
        /// </summary>
        /// <param name="nodeName">Name of the node used for different test case.</param>
        void ValidateChars(string nodeName)
        {
            // Get values from xml
            string FilePath = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string startIndex = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.CharsStartIndexNode);
            string count = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.CharsCountNode);

            using (MBFStreamReader streamReader = new MBFStreamReader(FilePath))
            {
                string currentLine = streamReader.Line;
                char[] charsArray = streamReader.ReadChars(Int32.Parse(startIndex, (IFormatProvider)null),
                    Int32.Parse(count, (IFormatProvider)null));

                // Validate array.
                for (int i = 0; i < charsArray.Length; i++)
                {
                    Assert.AreEqual(currentLine[i], charsArray[i]);
                    Console.WriteLine("Validated the char {0} successfully", charsArray[i]);
                    ApplicationLog.WriteLine("Validated the char successfully");
                }
            }
        }

        #endregion Helper Methods

    }
}
