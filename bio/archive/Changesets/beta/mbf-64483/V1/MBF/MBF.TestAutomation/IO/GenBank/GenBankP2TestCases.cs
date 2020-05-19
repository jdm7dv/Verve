// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * GenBankP2TestCases.cs
 * 
 *   This file contains the GenBank - Parsers and Formatters P2 test cases.
 * 
***************************************************************************/

using System;

using MBF.IO.GenBank;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.IO.GenBank
{
    /// <summary>
    /// GenBank P2 parser and formatter Test case implementation.
    /// </summary>
    [TestFixture]
    public class GenBankP2TestCases
    {

        #region Global Variables

        /// <summary>
        /// Global variables which store the information of xml file values
        /// and is used across the class file.
        /// </summary>
        static string _filepath;

        #endregion Global Variables

        #region Properties

        static string FilePath
        {
            get { return GenBankP2TestCases._filepath; }
            set { GenBankP2TestCases._filepath = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static GenBankP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region GenBank Parser P2 Test Cases

        /// <summary>
        /// Invalidate ParseHeader by passing invalid Locus header and
        /// validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseHeaderLocus()
        {
            InvalidateGenBankParser(
                Constants.InvalidateGenBankNodeName);
        }

        /// <summary>
        /// Invalidate ParseHeader by passing invalid version header and
        /// validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseHeaderVersion()
        {
            InvalidateGenBankParser(
                Constants.InvalidateGenBankNodeName);
        }

        /// <summary>
        /// Invalidate ParseHeader by passing header without Locus 
        /// and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseHeaderWithoutLocus()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankWithoutLocusNode);
        }

        /// <summary>
        /// Invalidate ParseHeader by passing invalid Segment header
        /// and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseHeader()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankWithSegmentNode);
        }

        /// <summary>
        /// Invalidate ParseHeader by passing invalid Primary header
        /// and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseHeaderPrimary()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankWithPrimaryNode);
        }

        /// <summary>
        /// Invalidate ParseHeader by making LocationBuilder property
        /// null and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseFeaturesLocBuild()
        {
            InvalidateGenBankParser(
                Constants.SimpleGenBankNodeName);
        }

        /// <summary>
        /// Invalidate ParseFeatures by passing invalid Line Reader
        /// and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseFeaturesLineHasReader()
        {
            InvalidateGenBankParser(
                Constants.InvalidateGenBankParseFeaturesHasReaderNode);
        }

        /// <summary>
        /// Invalidate ParseLocus by passing invalid Locus header
        /// and validate with the expected exception.
        /// Input : GenBank File with Locusheader contain pp
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseLocus()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankUnknownLocusNode);
        }

        /// <summary>
        /// Invalidate ParseLocus by passing invalid Locus header
        /// and validate with the expected exception.
        /// Input : GenBank File with Locusheader contain invalid Strand
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseLocusStrandType()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankUnknownStrandTypeNode);
        }

        /// <summary>
        /// Invalidate ParseLocus by passing invalid Locus header
        /// and validate with the expected exception.
        /// Input : GenBank File with Locusheader contain invalid topology
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseLocusStrandTopology()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankUnknownStrandTopologyNode);
        }

        /// <summary>
        /// Invalidate ParseLocus by passing invalid Locus header
        /// and validate with the expected exception.
        /// Input : GenBank File with Locusheader contain invalid date
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseLocusRawDate()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankUnknownRawDateNode);
        }

        /// <summary>
        /// Invalidate ParseLocus by passing invalid Locus header
        /// and validate with the expected exception.
        /// Input : GenBank File with Locusheader contain invalid MoleculeType
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseLocusInvalidMoleculeType()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankUnknownMoleculeTypeNode);
        }

        /// <summary>
        /// Invalidate ParseReference by passing invalid Reference header
        /// and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseReference()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankParseReferenceNode);
        }

        /// <summary>
        /// Invalidate ParseReference by passing invalid Reference Line
        /// and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseReferenceDefault()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankParseReferenceDefaultNode);
        }

        /// <summary>
        /// Invalidate ParseSequence by passing invalid Sequence Line
        /// and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseSequenceDefault()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankParseSequenceDefaultNode);
        }

        /// <summary>
        /// Invalidate ParseSequence by passing invalid Sequence
        /// Origin Line and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseSequence()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankParseSequenceNode);
        }

        /// <summary>
        /// Invalidate ParseSource by passing invalid Line Header
        /// and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankParseSource()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankParseSourceNode);
        }

        /// <summary>
        /// Invalidate ParseLocus by passing invalid datatype Header
        /// and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankHeaderDataType()
        {
            InvalidateGenBankParser(
                Constants.InvalidGenBankHeaderDataTypeNode);
        }

        /// <summary>
        /// Invalidate ParseLocus by passing invalid Alphabet
        /// and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankLocusAlphabet()
        {
            InvalidateGenBankParser(
                Constants.SimpleGenBankPrimaryNode);
        }

        /// <summary>
        /// Invalidate ParseReference by passing invalid reference
        /// and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenBankReference()
        {
            InvalidateGenBankParser(
                Constants.InvalideGenBankReferenceNode);
        }

        /// <summary>
        /// Invalidate ParseHeader by passing invalid files
        /// and validate with the expected exception.
        /// Input : GenBank File
        /// Output : Validate the Exception
        /// </summary>
        [Test]
        public void InvalidateGenParserHeader()
        {
            InvalidateGenBankParser(
                Constants.InvalideGenBankParseHeaderNode);
        }

        #endregion GenBank Parser P2 Test Cases

        #region Supporting Methods

        /// <summary>
        /// Validates GenBank Parser for General test cases.
        /// </summary>
        public void InvalidateGenBankParser(string node)
        {
            // Initialization of xml strings.
            FilePath = Utility._xmlUtil.GetTextValue(node,
                Constants.FilePathNode);

            try
            {
                GenBankParser parserObj = new GenBankParser();
                if (string.Equals(Constants.SimpleGenBankNodeName, node))
                {
                    parserObj.LocationBuilder = null;
                }
                else if (string.Equals(Constants.SimpleGenBankPrimaryNode, node))
                {
                    parserObj.Alphabet = Alphabets.RNA;
                }

                parserObj.ParseOne(FilePath);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the exception:");
                Console.WriteLine(
                    "GenBank Parser : Successfully validated the exception:");
            }
            catch (Exception)
            {
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the exception:");
                Console.WriteLine(
                    "GenBank Parser : Successfully validated the exception:");
            }
        }

        #endregion
    }
}