// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SequenceAlignmentParserP1TestCases.cs
 * 
 *   This file contains the SequenceAlignmentParser P1 test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;

using MBF.IO;
using MBF.IO.SAM;
using MBF.IO.BAM;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// SequenceAlignmentParser P1 Test case implementation.
    /// </summary>
    [TestClass]
    public class SequenceAlignmentParserP1TestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\SequenceAlignmentParser.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SequenceAlignmentParserP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region SequenceAlignmentParser TestCases

        /// <summary>
        /// Find a parser name, description for the Sam file.
        /// Input : Sam Files
        /// Validation : Expected parser, parser type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSamFileParser()
        {
            ValidateSequenceFileParser(Constants.SamFileParserNode, true);
        }

        /// <summary>
        /// Find a parser name, description for the Bam file.
        /// Input : Bam Files
        /// Validation : Expected parser, parser type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBamFileParser()
        {
            ValidateSequenceFileParser(Constants.BamFileParserNode, true);
        }

        /// <summary>
        /// Find a formatter name, description for the Sam file.
        /// Input : Sam Files
        /// Validation : Expected formatter, formatter type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSamFileFormatter()
        {
            ValidateSequenceFileParser(Constants.SamFileFormatterNode, false);
        }

        /// <summary>
        /// Find a formatter name, description for the Bam file.
        /// Input : Bam Files
        /// Validation : Expected formatter, formatter type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateBamFileFormatter()
        {
            ValidateSequenceFileParser(Constants.BamFileFormatterNode, false);
        }

        /// <summary>
        /// Valildate SequenceAlignmentParser class properties.
        /// Validation : Expected parser, parser type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSeqParserProperties()
        {
            // Gets the expected sequence from the Xml
            string samParserName = _utilityObj._xmlUtil.GetTextValue(Constants.SamFileParserNode,
                Constants.ParserNameNode);
            string bamParserName = _utilityObj._xmlUtil.GetTextValue(Constants.BamFileParserNode,
                Constants.ParserNameNode);

            // Get SequenceAlignmentParser class properties.
            SAMParser actualSamParser = SequenceAlignmentParsers.SAM;
            IList<ISequenceAlignmentParser> allParser = SequenceAlignmentParsers.All;

            BAMParser actualBamParserName = SequenceAlignmentParsers.BAM;

            // Validate Sequence parsers
            Assert.AreEqual(samParserName, actualSamParser.Name);
            Assert.AreEqual(2, allParser.Count);

            Assert.AreEqual(bamParserName, actualBamParserName.Name);
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SequenceAlignmentParser : Type of the parser is validated successfully"));
            ApplicationLog.WriteLine("Type of the parser is validated successfully");
        }

        /// <summary>
        /// Valildate SequenceAlignmentFormatter class properties.
        /// Validation : Expected parser, parser type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSeqFormatterProperties()
        {
            // Gets the expected sequence from the Xml
            string samFormatterName = _utilityObj._xmlUtil.GetTextValue(Constants.SamFileParserNode,
                Constants.ParserNameNode);
            string bamFormatterName = _utilityObj._xmlUtil.GetTextValue(Constants.BamFileParserNode,
                Constants.ParserNameNode);

            // Get SequenceAlignmentFormatter class properties.
            SAMFormatter actualSamFormatter = SequenceAlignmentFormatters.SAM;
            IList<ISequenceAlignmentFormatter> allFormatters = SequenceAlignmentFormatters.All;

            BAMFormatter actualBamFormatterName = SequenceAlignmentFormatters.BAM;

            // Validate Sequence Formatter
            Assert.AreEqual(samFormatterName, actualSamFormatter.Name);
            Assert.AreEqual(2, allFormatters.Count);

            Assert.AreEqual(bamFormatterName, actualBamFormatterName.Name);
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SequenceAlignmentFormatter : Type of the parser is validated successfully"));
            ApplicationLog.WriteLine("Type of the parser is validated successfully");
        }

        #endregion SequenceAlignmentParser TestCases

        #region Supporting Methods

        /// <summary>
        /// Validates general Sequence Parser.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="IsParser">IsParser is true if testcases is validating Parsers, 
        /// false if formatter validation</param>
        void ValidateSequenceFileParser(string nodeName, bool IsParser)
        {
            // Gets the expected sequence from the Xml
            string[] filePaths = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.FilePathsNode);
            string parserDescription = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.DescriptionNode);
            string parserName = _utilityObj._xmlUtil.GetTextValue(nodeName,
               Constants.ParserNameNode);
            string fileTypes = _utilityObj._xmlUtil.GetTextValue(nodeName,
               Constants.FileTypesNode);

            // Get a default parser for the file types.
            for (int i = 0; i < filePaths.Length; i++)
            {

                if (IsParser)
                {
                    ISequenceAlignmentParser parser = SequenceAlignmentParsers.FindParserByFile(filePaths[i]);
                    string description = parser.Description.Replace("\r", "").Replace("\n", "");
                    // Validate parser name, description and the file type supported by parser.
                    Assert.AreEqual(parserName, parser.Name);
                    Assert.AreEqual(parserDescription, description);
                    Assert.AreEqual(fileTypes, parser.FileTypes);
                }
                else
                {
                    ISequenceAlignmentFormatter formatter =
                        SequenceAlignmentFormatters.FindFormatterByFile(filePaths[i]);
                    Console.WriteLine(filePaths[i]);
                    Console.WriteLine(formatter.Description);
                    string description =
                        formatter.Description.Replace("\r", "").Replace("\n", "");
                    // Validate parser name, description and the file type supported by parser.
                    Assert.AreEqual(parserName, formatter.Name);
                    Assert.AreEqual(parserDescription, description);
                    Assert.AreEqual(fileTypes, formatter.FileTypes);
                }
            }

            Console.WriteLine(string.Format((IFormatProvider)null,
                "SequenceAlignmentParser : Type of the parser is validated successfully"));
            ApplicationLog.WriteLine("Type of the parser is validated successfully");
        }

        #endregion Supporting Methods
    }
}
