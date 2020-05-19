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
using NUnit.Framework;
using MBF.Util.Logging;
using MBF.TestAutomation.Util;
using MBF.IO;
using System.IO;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// MBFStreamReader Bvt Test case implementation.
    /// </summary>
    [TestFixture]
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

            Utility._xmlUtil = new XmlUtility(@"TestUtils\MBFStreamReaderConfig.xml");
        }

        #endregion Constructor

        #region MBFStreamReader Test cases

        /// <summary>
        /// Validate read FastA file
        /// Input : FastA file
        /// Ouput : Validation of FastA file read
        /// </summary>
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
        public void ValidateLineCharsForFastAStream()
        {
            ValidateChars(Constants.SimpleFastAStreamReaderNode);
        }

        /// <summary>
        /// Validate current line characters for FastQ file stream
        /// Input : FastQ file
        /// Output : Current line characters validation
        /// </summary>
        [Test]
        public void ValidateLineCharsForFastQStream()
        {
            ValidateChars(Constants.SimpleFastQStreamReaderNode);
        }

        /// <summary>
        /// Validate current line characters for SAM file stream
        /// Input : SAM file
        /// Output : Current line characters validation
        /// </summary>
        [Test]
        public void ValidateLineCharsForSAMStream()
        {
            ValidateChars(Constants.SimpleSAMStreamReaderNode);
        }

        /// <summary>
        /// Validate MBFStreamReader class properties.
        /// Input : fastA file
        /// Output : Validate properties.
        /// </summary>
        [Test]
        public void ValidateMBFStreamReaderProperties()
        {
            // Get values from xml
            string FilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastAStreamReaderNode, Constants.FilePathNode);
            string newLineCharsCount = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastAStreamReaderNode, Constants.NewLineCharacterCountNode);
            string pos = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastAStreamReaderNode, Constants.PositionNode);
            string startingIndex = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastAStreamReaderNode, Constants.CurrentLineStartingIndexNode);

            MBFStreamReader streamReader = new MBFStreamReader(FilePath, true);

            // Validate Properties
            Assert.IsTrue(streamReader.CanRead);
            Assert.IsTrue(streamReader.SkipBlankLines);
            Assert.IsTrue(streamReader.HasLines);
            Assert.AreEqual(newLineCharsCount,
                streamReader.NewLineCharacterCount.ToString());
            Assert.AreEqual(pos,
                streamReader.Position.ToString());
            Assert.AreEqual(newLineCharsCount,
                streamReader.NewLineCharacterCount.ToString());
            Assert.AreEqual(startingIndex,
                streamReader.CurrentLineStartingIndex.ToString());

            Console.WriteLine("Validated the StreamReader properties successfully");
            ApplicationLog.WriteLine("Validated the StreamReader properties successfully");

            // Dispose StreamReader.            
            streamReader.Close();
            streamReader.Dispose();
        }

        /// <summary>
        /// Validate position of the reader
        /// Input : FastA file
        /// output : Validation of reader position
        /// </summary>
        [Test]
        public void ValidatePosition()
        {
            string FilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastAStreamReaderNode, Constants.FilePathNode);
            string pos = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastAStreamReaderNode, Constants.PositionNode);

            MBFStreamReader reader = new MBFStreamReader(FilePath);

            // Set position at the begining.
            reader.Seek(Int32.Parse(pos), SeekOrigin.Begin);

            // Validate the set position
            Assert.AreEqual(pos, reader.Position.ToString());

            Console.WriteLine("Validate the position successfulyy");
            ApplicationLog.WriteLine("Validated the position successfully");
        }

        #endregion MBFStreamReader Test cases

        #region Helper Methods

        /// <summary>
        /// Validate Read Biological sequences using MBFStreamReader
        /// </summary>
        /// <param name="nodeName">Name of the node used for different test case.</param>
        /// <param name="inputType">Different streaming ipnuts used for different test cases</param>
        private void ValidateMBFStreamReader(string nodeName,
           StreamReaderInputType inputType)
        {
            // Get values from xml
            string FilePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string[] expectedOutput = Utility._xmlUtil.GetTextValues(
                nodeName, Constants.ExpectedLinesNode);
            MBFStreamReader streamReader = null;

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

            // Dispose stream reader.
            streamReader.Close();
            streamReader.Dispose();

        }

        /// <summary>
        /// Validate Read Biological sequences using MBFStreamReader
        /// </summary>
        /// <param name="nodeName">Name of the node used for different test case.</param>
        /// <param name="IsStartAndEndIndex">True if validating from start to end index substring,
        /// else false</param>
        private void ValidateSubString(string nodeName,
            bool IsStartAndEndIndex)
        {
            // Get values from xml
            string FilePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedString);
            string startIndex = Utility._xmlUtil.GetTextValue(
               nodeName, Constants.StartIndexNode);
            string endIndex = Utility._xmlUtil.GetTextValue(
               nodeName, Constants.EndIndexNode);

            string subString = string.Empty;

            MBFStreamReader streamReader = new MBFStreamReader(FilePath);
            if (IsStartAndEndIndex)
            {
                subString = streamReader.GetLineField(Int32.Parse(startIndex),
                    Int32.Parse(endIndex));
            }
            else
            {
                subString = streamReader.GetLineField(Int32.Parse(startIndex));
            }

            // Validate sub string of a line.
            Assert.AreEqual(expectedString, subString);
            Console.WriteLine("The expected substring is {0}", subString);
            ApplicationLog.WriteLine("Validated the substring successfully");

            // Dispose stream reader.
            streamReader.Close();
            streamReader.Dispose();
        }

        /// <summary>
        /// Validate Read characters from curent line
        /// </summary>
        /// <param name="nodeName">Name of the node used for different test case.</param>
        private void ValidateChars(string nodeName)
        {
            // Get values from xml
            string FilePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string startIndex = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CharsStartIndexNode);
            string count = Utility._xmlUtil.GetTextValue(
               nodeName, Constants.CharsCountNode);

            MBFStreamReader streamReader = new MBFStreamReader(FilePath);
            string currentLine = streamReader.Line;
            char[] charsArray = streamReader.ReadChars(Int32.Parse(startIndex), Int32.Parse(count));

            // Validate array.
            for (int i = 0; i < charsArray.Length; i++)
            {
                Assert.AreEqual(currentLine[i], charsArray[i]);
                Console.WriteLine("Validated the char {0} successfully", charsArray[i]);
                ApplicationLog.WriteLine("Validated the char successfully");
            }

            // Dispose stream reader.
            streamReader.Close();
            streamReader.Dispose();
        }

        #endregion Helper Methods

    }
}
