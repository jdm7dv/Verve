// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * PhylipP2TestCases.cs
 * 
 *   This file contains the Phylip - Parsers and Formatters P2 test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

using MBF.Algorithms.Alignment;
using MBF.IO;
using MBF.IO.Phylip;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.IO.Phylip
{
    /// <summary>
    /// Phylip P2 parser and formatter Test case implementation.
    /// </summary>
    [TestClass]
    public class PhylipP2TestCases
    {

        #region Enums

        /// <summary>
        /// Additional parameters to validate different scenarios.
        /// </summary>
        enum ParserTestAttributes
        {
            Parse,
            ParseOne
        };

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\PhylipTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static PhylipP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Phylip Parser P2 Test cases

        /// <summary>
        /// Parse a empty Phylip file and invalidate Parse(file-name)
        /// Input : Invalid File
        /// Output: Validation of exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidateReadOnlyPhylipParserParseReader()
        {
            InvalidatePhylipParserTestCases(Constants.EmptyPhylipParserFileNode,
                ParserTestAttributes.Parse);
        }

        /// <summary>
        /// Parse a Empty Phylip file and invalidate ParseOne(reader, isReadOnly)
        /// Input : Invalid File
        /// Output: Validation of exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidateReadOnlyPhylipParserOneReader()
        {
            InvalidatePhylipParserTestCases(Constants.EmptyPhylipParserFileNode,
                ParserTestAttributes.ParseOne);
        }

        /// <summary>
        /// Parse a invalid Phylip file and invalidate ParseOneWithSpecificFormat()
        /// Input : Invalid File
        /// Output: Validation of exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidatePhylipParserHeader()
        {
            InvalidatePhylipParserTestCases(
                Constants.InvalidatePhylipParserCountNode,
                ParserTestAttributes.Parse);
        }

        /// <summary>
        /// Parse a invalid Phylip file and invalidate Alphabet
        /// Input : Invalid File
        /// Output: Validation of exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidatePhylipParserAlphabet()
        {
            InvalidatePhylipParserTestCases(
                Constants.InvalidatePhylipParserAlphabetNode,
                ParserTestAttributes.Parse);
        }

        /// <summary>
        /// Parse a invalid Phylip file and invalidate Align Alphabet
        /// Input : Invalid File
        /// Output: Validation of exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidatePhylipParserAlignAlphabet()
        {
            InvalidatePhylipParserTestCases(
                Constants.InvalidatePhylipParserAlignAlphabetNode,
                ParserTestAttributes.Parse);
        }

        /// <summary>
        /// Parse a invalid Phylip file and invalidate Sequence length
        /// Input : Invalid File
        /// Output: Validation of exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidatePhylipParserSeqLength()
        {
            InvalidatePhylipParserTestCases(
                Constants.InvalidatePhylipParserSeqLengthNode,
                ParserTestAttributes.Parse);
        }

        /// <summary>
        /// Parse a invalid value and invalidate IdentifyAlphabet
        /// Input : Invalid Values
        /// Output: Validation of exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidateCommonSeqParserIdentify()
        {
            Assert.IsNull(
                new CommonSequenceParser().IdentifyAlphabet(
                Alphabets.RNA,
                String.Empty));
            Assert.IsNull(
                new CommonSequenceParser().IdentifyAlphabet(
                Alphabets.RNA,
                null));

            ApplicationLog.WriteLine(
                    "CommonSequenceParser P2 : All the features invalidated successfully.");
            Console.WriteLine(
                "CommonSequenceParser P2 : All the features invalidated successfully.");
        }

        /// <summary>
        /// Parse a invalid value and invalidate GetMoleculeType(strType)
        /// Input : Invalid Values
        /// Output: Validation of exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidateCommonSeqParserGetMoleculeTypeStr()
        {
            try
            {
                CommonSequenceParser.GetMoleculeType(null as string);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "CommonSequenceParser P2 : All the features invalidated successfully.");
                Console.WriteLine(
                    "CommonSequenceParser P2 : All the features invalidated successfully.");
            }
        }

        /// <summary>
        /// Parse a invalid value and invalidate GetMoleculeType(Alphabets)
        /// Input : Invalid Values
        /// Output: Validation of exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidateCommonSeqParserGetMoleculeTypeAlphabet()
        {
            Assert.IsNotNull(
                CommonSequenceParser.GetMoleculeType(null as IAlphabet));
            Assert.AreEqual(
                MoleculeType.Invalid,
                CommonSequenceParser.GetMoleculeType(null as IAlphabet));

            ApplicationLog.WriteLine(
                "CommonSequenceParser P2 : All the features invalidated successfully.");
            Console.WriteLine(
                "CommonSequenceParser P2 : All the features invalidated successfully.");
        }
        #endregion Phylip Parser P2 Test cases

        #region Helper Method

        /// <summary>
        /// General method to invalidate Phylip parser
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="method">Phylip Parser method parameters</param>
        void InvalidatePhylipParserTestCases(
            string nodeName,
            ParserTestAttributes method)
        {
            try
            {
                string filePath = _utilityObj._xmlUtil.GetTextValue(
                    nodeName,
                    Constants.FilePathNode);
                PhylipParser parser = new PhylipParser();

                switch (method)
                {
                    case ParserTestAttributes.Parse:
                        parser.Parse(filePath, true);
                        break;
                    case ParserTestAttributes.ParseOne:
                        parser.ParseOne(filePath, true);
                        break;
                    default:
                        break;
                }

                Assert.Fail();
            }
            catch (InvalidDataException)
            {
                ApplicationLog.WriteLine(
                    "Phylip Parser P2 : All the features validated successfully.");
                Console.WriteLine(
                    "Phylip Parser P2 : All the features validated successfully.");
            }
            catch (FormatException)
            {
                ApplicationLog.WriteLine(
                    "Phylip Parser P2 : All the features validated successfully.");
                Console.WriteLine(
                    "Phylip Parser P2 : All the features validated successfully.");
            }
        }

        #endregion
    }
}