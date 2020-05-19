// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SequenceParserP1TestCases.cs
 * 
 *   This file contains the SequenceParser P1 test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;

using MBF.IO;
using MBF.IO.Fasta;
using MBF.IO.FastQ;
using MBF.IO.GenBank;
using MBF.IO.Gff;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// SequenceParser P1 Test case implementation.
    /// </summary>
    [TestClass]
    public class SequenceParserP1TestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\SequenceParser.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SequenceParserP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region SequenceParser TestCases

        /// <summary>
        /// Find a parser name, description for the Fasta file.
        /// Input : FastA Files
        /// Validation : Expected parser, parser type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFastAFileParser()
        {
            ValidateSequenceFileParser(Constants.FastAFileParserNode, true);
        }

        /// <summary>
        /// Find a parser name, description for the GenBank file.
        /// Input : GenBank Files
        /// Validation : Expected parser, parser type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGenBankFileParser()
        {
            ValidateSequenceFileParser(Constants.GenBankFileParserNode, true);
        }

        /// <summary>
        /// Find a parser name, description for the FastQ file.
        /// Input : FastQ Files
        /// Validation : Expected parser, parser type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFastQFileParser()
        {
            ValidateSequenceFileParser(Constants.FastQFileParserNode, true);
        }

        /// <summary>
        /// Find a parser name, description for the Gff file.
        /// Input : GFF Files
        /// Validation : Expected parser, parser type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGffFileParser()
        {
            ValidateSequenceFileParser(Constants.GffFileParserNode, true);
        }

        /// <summary>
        /// Find a formatter name, description for the Fasta file.
        /// Input : FastA Files
        /// Validation : Expected formatter, formatter type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFastAFileFormatter()
        {
            ValidateSequenceFileParser(Constants.FastAFileFormatterNode, false);
        }

        /// <summary>
        /// Find a formatter name, description for the GenBank file.
        /// Input : GenBank Files
        /// Validation : Expected formatter, formatter type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGenBankFileFormatter()
        {
            ValidateSequenceFileParser(Constants.GenBankFileFormatterNode, false);
        }

        /// <summary>
        /// Find a formatter name, description for the Gff file.
        /// Input : Gff Files
        /// Validation : Expected formatter, formatter type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGffFileFormatter()
        {
            ValidateSequenceFileParser(Constants.GffFileFormatterNode, false);
        }

        /// <summary>
        /// Find a formatter name, description for the FastQ file.
        /// Input : FastQ Files
        /// Validation : Expected formatter, formatter type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateFastQFileFormatter()
        {
            ValidateSequenceFileParser(Constants.FastQFileFormatterNode, false);
        }

        /// <summary>
        /// Valildate SequenceParser class properties.
        /// Validation : Expected parser, parser type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSeqParserProperties()
        {
            // Gets the expected sequence from the Xml
            string fastaParserName = _utilityObj._xmlUtil.GetTextValue(Constants.FastAFileParserNode,
                Constants.ParserNameNode);
            string genBankParserName = _utilityObj._xmlUtil.GetTextValue(Constants.GenBankFileParserNode,
                Constants.ParserNameNode);
            string gffParserName = _utilityObj._xmlUtil.GetTextValue(Constants.GffFileParserNode,
                Constants.ParserNameNode);
            string fastQParserName = _utilityObj._xmlUtil.GetTextValue(Constants.FastQFileParserNode,
                Constants.ParserNameNode);

            // Get SequenceParser class properties.
            FastaParser actualFastAParser = SequenceParsers.Fasta;
            IList<ISequenceParser> allParser = SequenceParsers.All;
            GenBankParser actualgenBankParserName = SequenceParsers.GenBank;
            FastQParser actualFastQParserName = SequenceParsers.FastQ;
            GffParser actualGffParserName = SequenceParsers.Gff;

            // Validate Sequence parsers
            Assert.AreEqual(fastaParserName, actualFastAParser.Name);
            Assert.AreEqual(4, allParser.Count);
            Assert.AreEqual(genBankParserName, actualgenBankParserName.Name);
            Assert.AreEqual(gffParserName, actualGffParserName.Name);
            Assert.AreEqual(fastQParserName, actualFastQParserName.Name);
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SequenceParser : Type of the parser is validated successfully"));
            ApplicationLog.WriteLine("Type of the parser is validated successfully");
        }

        /// <summary>
        /// Valildate SequenceFormatter class properties.
        /// Validation : Expected parser, parser type and description.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSeqFormatterProperties()
        {
            // Gets the expected sequence from the Xml
            string fastaFormatterName = _utilityObj._xmlUtil.GetTextValue(Constants.FastAFileParserNode,
                Constants.ParserNameNode);
            string genBankFormatterName = _utilityObj._xmlUtil.GetTextValue(Constants.GenBankFileParserNode,
                Constants.ParserNameNode);
            string gffFormatterName = _utilityObj._xmlUtil.GetTextValue(Constants.GffFileParserNode,
                Constants.ParserNameNode);
            string fastQFormatterName = _utilityObj._xmlUtil.GetTextValue(Constants.FastQFileParserNode,
                Constants.ParserNameNode);

            // Get SequenceFormatter class properties.
            FastaFormatter actualFastAFormatter = SequenceFormatters.Fasta;
            IList<ISequenceFormatter> allFormatters = SequenceFormatters.All;
            GenBankFormatter actualgenBankFormatterName = SequenceFormatters.GenBank;
            FastQFormatter actualFastQFormatterName = SequenceFormatters.FastQ;
            GffFormatter actualGffFormatterName = SequenceFormatters.Gff;

            // Validate Sequence Formatter
            Assert.AreEqual(fastaFormatterName, actualFastAFormatter.Name);
            Assert.AreEqual(4, allFormatters.Count);
            Assert.AreEqual(genBankFormatterName, actualgenBankFormatterName.Name);
            Assert.AreEqual(gffFormatterName, actualGffFormatterName.Name);
            Assert.AreEqual(fastQFormatterName, actualFastQFormatterName.Name);
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SequenceFormatter : Type of the parser is validated successfully"));
            ApplicationLog.WriteLine("Type of the parser is validated successfully");
        }

        #endregion SequenceParser TestCases

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
                    ISequenceParser parser = SequenceParsers.FindParserByFile(filePaths[i]);
                    string description = parser.Description.Replace("\r", "").Replace("\n", "");
                    // Validate parser name, description and the file type supported by parser.
                    Assert.AreEqual(parserName, parser.Name);
                    Assert.AreEqual(parserDescription, description);
                    Assert.AreEqual(fileTypes, parser.FileTypes);
                }
                else
                {
                    ISequenceFormatter formatter =
                        SequenceFormatters.FindFormatterByFile(filePaths[i]);
                    string description =
                        formatter.Description.Replace("\r", "").Replace("\n", "");
                    // Validate parser name, description and the file type supported by parser.
                    Assert.AreEqual(parserName, formatter.Name);
                    Assert.AreEqual(parserDescription, description);
                    Assert.AreEqual(fileTypes, formatter.FileTypes);

                }
            }

            Console.WriteLine(string.Format((IFormatProvider)null,
                "SequenceParser : Type of the parser is validated successfully"));
            ApplicationLog.WriteLine("Type of the parser is validated successfully");
        }

        #endregion Supporting Methods
    }
}
