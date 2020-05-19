// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * FastQP2TestCases.cs
 * 
 *This file contains FastQ Parsers and Formatters P2 test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

using MBF.IO;
using MBF.IO.FastQ;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.IO.FastQ
{
    /// <summary>
    /// FASTQ parser and formatter P2 Test cases implementation.
    /// </summary>
    [TestFixture]
    public class FastQP2TestCases
    {
        #region Enums

        /// <summary>
        /// FastQ Formatter Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum FastQFormatParameters
        {
            TextWriter,
            Sequence,
            QualitativeSequence,
            Default
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static FastQP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\FastQTestsConfig.xml");
        }

        #endregion Constructor

        #region FastQ Parser P2 Test cases

        /// <summary>
        /// Invalidate FastQ Parser with invalid FastQ file.
        /// Input : Qualitative sequence without @ at first line
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQParserWithInvalidSeqId()
        {
            InValidateFastQParser(Constants.FastQSequenceWithInvalidSeqIdNode,
                false);
        }

        /// <summary>
        /// Invalidate FastQ Parser with empty sequence.
        /// Input : FastQ empty sequence.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQParserWithEmptySequence()
        {
            InValidateFastQParser(Constants.FastQParserEmptySequenceNode,
                false);
        }

        /// <summary>
        /// Invalidate FastQ Parser with invalid Qual scores.
        /// Input : FastQ file with invalid qual score.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQParserWithInvalidQualScore()
        {
            InValidateFastQParser(Constants.FastQParserWithInvalidQualScoreNode,
                false);
        }

        /// <summary>
        /// Invalidate FastQ Parser with empty Qual scores.
        /// Input : FastQ file with empty qual score.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQParserWithEmptyQualScore()
        {
            InValidateFastQParser(Constants.FastQParserWithEmptyQualScoreNode,
                false);
        }

        /// <summary>
        /// Invalidate FastQ Parser with empty Qual scores and Empty Qual Id.
        /// Input : FastQ file with empty qual score and Id.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQParserWithEmptyQualScoreAndQualId()
        {
            InValidateFastQParser(Constants.FastQParserWithEmptyQualScoreAndQualID,
                false);
        }

        /// <summary>
        /// Invalidate FastQ Parser with invalid alphabet.
        /// Input : Invalid alphabet.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQParserWithInvalidAlphabet()
        {
            InValidateFastQParser(Constants.FastQParserWithInvalidAlphabet,
                false);
        }

        /// <summary>
        /// Invalidate empty fastq file using Parse().
        /// Input : Empty fastq file.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateParseEmptyFastQFile()
        {
            InValidateFastQParser(Constants.EmptyFastQFileNode, false);
        }

        /// <summary>
        /// Invalidate empty fastq file using ParseOne().
        /// Input : Empty fastq file.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateParseOneEmptyFastQFile()
        {
            InValidateFastQParser(Constants.EmptyFastQFileNode, true);
        }

        /// <summary>
        /// Invalidate fastq formatter with text writer as null value.
        /// Input : Empty TextWrite file.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQFormatterWithInvalidTextWriter()
        {
            InValidateFastQFormatter(FastQFormatParameters.TextWriter);
        }

        /// <summary>
        /// Invalidate fastq formatter with Sequence as null value.
        /// Input : Invalid sequence.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQFormatterWithInvalidSequence()
        {
            InValidateFastQFormatter(FastQFormatParameters.Sequence);
        }

        /// <summary>
        /// Invalidate fastq formatter with Qual Sequence as null value.
        /// Input : Invalid Qualitative sequence.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQFormatterWithInvalidQualSequence()
        {
            InValidateFastQFormatter(FastQFormatParameters.QualitativeSequence);
        }

        /// <summary>
        /// Invalidate fastq formatter with Qual Sequence and TextWriter null value.
        /// Input : Invalid Qualitative sequence.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQFormatterWithInvalidQualSequenceAndTextWriter()
        {
            InValidateFastQFormatter(FastQFormatParameters.Default);
        }

        /// <summary>
        /// Invalidate fastq ParseRange with negative startIndex and count
        /// Input : Invalid startIndex or Invalid count.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQParseRange()
        {
            try
            {
                new FastQParser().ParseRange(-1, 0, new SequencePointer());
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                ApplicationLog.WriteLine(
                    "FastQ Parser P2 : Successfully validated the exception");
                Console.WriteLine(
                    "FastQ Parser P2 : Successfully validated the exception");
            }

            try
            {
                new FastQParser().ParseRange(0, -1, new SequencePointer());
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                ApplicationLog.WriteLine(
                    "FastQ Parser P2 : Successfully validated the exception");
                Console.WriteLine(
                    "FastQ Parser P2 : Successfully validated the exception");
            }
        }

        /// <summary>
        /// Invalidate fastq GetSequenceID with sequencePointer
        /// as null value.
        /// Input : Invalid sequencePointer.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQParserGetSequenceId()
        {
            try
            {
                new FastQParser().GetSequenceID(null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "FastQ Parser P2 : Successfully validated the exception");
                Console.WriteLine(
                    "FastQ Parser P2 : Successfully validated the exception");
            }
        }

        /// <summary>
        /// Invalidate FastQParseOneWithFastQFormat with invalid format
        /// Input : Invalid format.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQParseOneWithFastQFormat()
        {
            try
            {
                ISequenceParser parser = new FastQParser();
                parser.ParseOne(
                    Utility._xmlUtil.GetTextValue(
                    Constants.FastQInvalidFormatFileNode,
                    Constants.FilePathNode));
                Assert.Fail();
            }
            catch (FormatException)
            {
                ApplicationLog.WriteLine(
                    "FastQ Parser P2 : Successfully validated the exception");
                Console.WriteLine(
                    "FastQ Parser P2 : Successfully validated the exception");
            }
        }

        /// <summary>
        /// Invalidate Parse(file-name, isReadOnly) with null as file-name
        /// Input : Invalid file-name.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQParseNoFileName()
        {
            try
            {
                new FastQParser().Parse(null as string, true);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "FastQ Parser P2 : Successfully validated the exception");
                Console.WriteLine(
                    "FastQ Parser P2 : Successfully validated the exception");
            }
        }

        /// <summary>
        /// Invalidate IdentifyFastQFormatType(qualScores) with
        /// invalid quality score
        /// Input : Invalid quality score file-name.
        /// Output : Validate Exception.
        /// </summary>
        [Test]
        public void InvalidateFastQParseWithQualityScore()
        {
            try
            {
                ISequenceParser parser = new FastQParser();
                parser.ParseOne(
                    Utility._xmlUtil.GetTextValue(
                    Constants.FastQInvalidQualScoreFileNode,
                    Constants.FilePathNode));
                Assert.Fail();
            }
            catch (FormatException)
            {
                ApplicationLog.WriteLine(
                    "FastQ Parser P2 : Successfully validated the exception");
                Console.WriteLine(
                    "FastQ Parser P2 : Successfully validated the exception");
            }
        }

        #endregion FastQ Parser P2 Test cases

        #region Helper Methods

        /// <summary>
        /// General method to Invalidate FastQ Parser.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="IsParseOne">True for FastQParseOne validations, else false</param>
        /// </summary>
        static void InValidateFastQParser(string nodeName, bool IsParseOne)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                Utility._xmlUtil.GetTextValue(nodeName, Constants.FastQFormatType));

            // Create a FastQ Parser object.
            FastQParser fastQParserObj = new FastQParser();
            fastQParserObj.AutoDetectFastQFormat = true;
            fastQParserObj.FastqType = expectedFormatType;

            if (IsParseOne)
            {
                try
                {
                    fastQParserObj.ParseOne(filePath);
                    Assert.Fail();
                }
                catch (Exception)
                {
                    ApplicationLog.WriteLine(
                        "FastQ Parser P2 : Successfully validated the exception");
                    Console.WriteLine(
                        "FastQ Parser P2 : Successfully validated the exception");
                }
            }
            else
            {
                try
                {
                    fastQParserObj.Parse(filePath);
                    Assert.Fail();
                }
                catch (Exception)
                {
                    ApplicationLog.WriteLine(
                        "FastQ Parser P2 : Successfully validated the exception");
                    Console.WriteLine(
                        "FastQ Parser P2 : Successfully validated the exception");
                }
            }
        }

        /// <summary>
        /// General method to Invalidate FastQ Parser.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="param">FastQ Formatter different parameters</param>
        /// </summary>
        static void InValidateFastQFormatter(FastQFormatParameters param)
        {
            // Gets the expected sequence from the Xml
            string filepath = Utility._xmlUtil.GetTextValue(
                Constants.MultiSeqSangerRnaProNode, Constants.FilePathNode);
            FastQFormatType expectedFormatType = Utility.GetFastQFormatType(
                Utility._xmlUtil.GetTextValue(Constants.MultiSeqSangerRnaProNode,
                Constants.FastQFormatType));

            // Parse a FastQ file.
            FastQParser fastQParser = new FastQParser();
            fastQParser.AutoDetectFastQFormat = true;
            fastQParser.FastqType = expectedFormatType;

            IQualitativeSequence sequence = null;
            sequence = fastQParser.ParseOne(filepath);
            FastQFormatter fastQFormatter = new FastQFormatter();
            TextWriter txtWriter = null;

            switch (param)
            {
                case FastQFormatParameters.TextWriter:
                    try
                    {
                        fastQFormatter.Format(sequence, null as TextWriter);
                        Assert.Fail();
                    }

                    catch (Exception)
                    {
                        ApplicationLog.WriteLine(
                            "FastQ Parser P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "FastQ Parser P2 : Successfully validated the exception");
                    }
                    break;
                case FastQFormatParameters.Sequence:
                    try
                    {
                        fastQFormatter.Format(null as ISequence, txtWriter);
                        Assert.Fail();
                    }

                    catch (Exception)
                    {
                        ApplicationLog.WriteLine(
                            "FastQ Parser P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "FastQ Parser P2 : Successfully validated the exception");
                    }
                    break;
                case FastQFormatParameters.QualitativeSequence:
                    try
                    {
                        fastQFormatter.Format(null as IQualitativeSequence, txtWriter);
                        Assert.Fail();
                    }

                    catch (Exception)
                    {
                        ApplicationLog.WriteLine(
                            "FastQ Parser P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "FastQ Parser P2 : Successfully validated the exception");
                    }
                    break;
                default:
                    try
                    {
                        fastQFormatter.Format(sequence as QualitativeSequence, null as TextWriter);
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        ApplicationLog.WriteLine(
                            "FastQ Parser P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "FastQ Parser P2 : Successfully validated the exception");
                    }
                    break;
            }
        }

        #endregion Helper Methods
    }
}
