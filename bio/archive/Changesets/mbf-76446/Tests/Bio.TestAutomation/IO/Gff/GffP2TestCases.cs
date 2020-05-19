// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * GffP2TestCases.cs
 * 
 *   This file contains the Gff - Parsers and Formatters P2 test cases.
 * 
***************************************************************************/

using System;
using System.IO;

using Bio.IO.Gff;
using Bio.TestAutomation.Util;
using Bio.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bio.TestAutomation.IO.GFF
{
    /// <summary>
    /// Gff P2 parser and formatter Test case implementation.
    /// </summary>
    [TestClass]
    public class GffP2TestCases
    {

        #region Enum

        /// <summary>
        /// Additional parameters to validate different scenarios.
        /// </summary>
        enum ParserOrFormatterType
        {
            parseFileName,
            parseTextReader
        }

        #endregion Enum

        #region Global Variables

        Utility utilityObj = new Utility(@"TestUtils\GffTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static GffP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("bio.automation.log");
            }
        }

        #endregion Constructor

        #region Gff Parser P2 Test cases

        /// <summary>
        /// Parse a invalid Gff file to ParseFeatures()
        /// Input : Invalid Gff File
        /// Output: Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGffParserFeatureLength()
        {
            InvalidateGffParserFeatures(
                Constants.InvalidFeatureLengthNode,
                ParserOrFormatterType.parseFileName);
        }

        /// <summary>
        /// Parse a invalid Gff file to ParseFeatures()
        /// Input : Invalid Gff File
        /// Output: Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGffParserFeatureStart()
        {
            InvalidateGffParserFeatures(
                Constants.InvalidateStartFeatureNode,
                ParserOrFormatterType.parseFileName);
        }

        /// <summary>
        /// Parse a invalid Gff file to ParseFeatures()
        /// Input : Invalid Gff File
        /// Output: Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGffParserFeatureEnd()
        {
            InvalidateGffParserFeatures(
                Constants.InvalidateEndFeatureNode,
                ParserOrFormatterType.parseFileName);
        }

        /// <summary>
        /// Parse a invalid Gff file to ParseFeatures()
        /// Input : Invalid Gff File
        /// Output: Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGffParserFeatureScore()
        {
            InvalidateGffParserFeatures(
                Constants.InvalidateScoreFeatureNode,
                ParserOrFormatterType.parseFileName);
        }

        /// <summary>
        /// Parse a invalid Gff file to ParseFeatures()
        /// Input : Invalid Gff File
        /// Output: Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGffParserFeatureStrand()
        {
            InvalidateGffParserFeatures(
                Constants.InvalidateStrandFeatureNode,
                ParserOrFormatterType.parseFileName);
        }

        /// <summary>
        /// Parse a invalid Gff file to ParseFeatures()
        /// Input : Invalid Gff File
        /// Output: Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGffParserFeatureFrame()
        {
            InvalidateGffParserFeatures(
                Constants.InvalidateFrameFeatureNode,
                ParserOrFormatterType.parseFileName);
        }

        /// <summary>
        /// Parse a invalid Gff file to ParseFeatures()
        /// Input : Invalid Gff File
        /// Output: Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGffParserNullFeatures()
        {
            InvalidateGffParserFeatures(
                Constants.InvalidateNullFeatureNode,
                ParserOrFormatterType.parseTextReader);
        }

        /// <summary>
        /// Parse a invalid Gff file to ParseHeader()
        /// Input : Invalid Gff File
        /// Output: Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGffParserHeaderVersion()
        {
            InvalidateGffParserFeatures(
                Constants.InvalidateParseHeaderVersionNode,
                ParserOrFormatterType.parseFileName);
        }

        /// <summary>
        /// Parse a invalid Gff file to ParseHeader()
        /// Input : Invalid Gff File
        /// Output: Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGffParserHeaderDateFormat()
        {
            InvalidateGffParserFeatures(
                Constants.InvalidateParseHeaderDateFormateNode,
                ParserOrFormatterType.parseFileName);
        }

        /// <summary>
        /// Parse a invalid Gff file to ParseHeader()
        /// Input : Invalid Gff File
        /// Output: Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGffParserHeaderFieldLen()
        {
            InvalidateGffParserFeatures(
                Constants.InvalidateParseHeaderFieldLenNode,
                ParserOrFormatterType.parseFileName);
        }

        /// <summary>
        /// Parse a invalid Gff file to ParseHeader()
        /// Input : Invalid Gff File
        /// Output: Expected Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGffParserHeaderProtein()
        {
            InvalidateGffParserFeatures(
                Constants.InvalidateParseHeaderProteinNode,
                ParserOrFormatterType.parseFileName);
        }

        #endregion Gff Parser P2 Test cases

        #region Helper Method

        /// <summary>
        /// General method to invalida Gff Parser Features.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="method">Gff Parse method parameters</param>
        void InvalidateGffParserFeatures(
            string nodeName,
            ParserOrFormatterType method)
        {
            try
            {
                // Gets the expected sequence from the Xml
                string filePath = utilityObj.xmlUtil.GetTextValue(
                    nodeName,
                    Constants.FilePathNode);

                switch (method)
                {
                    case ParserOrFormatterType.parseFileName:
                        new GffParser(filePath).Parse();
                        break;
                    case ParserOrFormatterType.parseTextReader:
                        new GffParser(filePath).Parse();
                        break;
                    default:
                        break;
                }

                Assert.Fail();
            }
            catch (InvalidDataException)
            {
                ApplicationLog.WriteLine(
                    "GFF Parser P2 : All the features validated successfully.");
                Console.WriteLine(
                    "GFF Parser P2 : All the features validated successfully.");
            }
            catch (InvalidOperationException)
            {
                ApplicationLog.WriteLine(
                    "GFF Parser P2 : All the features validated successfully.");
                Console.WriteLine(
                    "GFF Parser P2 : All the features validated successfully.");
            }
            catch (FormatException)
            {
                ApplicationLog.WriteLine(
                    "GFF Parser P2 : All the features validated successfully.");
                Console.WriteLine(
                    "GFF Parser P2 : All the features validated successfully.");
            }
            catch (NotSupportedException)
            {
                ApplicationLog.WriteLine(
                    "GFF Parser P2 : All the features validated successfully.");
                Console.WriteLine(
                    "GFF Parser P2 : All the features validated successfully.");
            }
        }

        #endregion Helper Method
    }
}
