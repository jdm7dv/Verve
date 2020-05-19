// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SnpBvtTestCases.cs
 * 
 *   This file contains the Snp - Parsers Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using MBF.IO;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// Snp Bvt parser Test case implementation.
    /// </summary>
    [TestFixture]
    public class SnpBvtTestCases
    {

        #region Enums

        /// <summary>
        /// Additional Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AdditionalParameters
        {
            ParseOneTextReader,
            ParseOneFilePath,
            ParseTextReader,
            ParseFilePath,
            ParseAlleleTwo
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SnpBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region Test cases

        /// <summary>
        /// Parse a valid Snp file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Snp File
        /// Validation : Expected sequence, Chromosome position, Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        public void SnpBvtParserValidateParseFilePath()
        {
            SnpParserGeneralTestCases(Constants.SimpleSnpNodeName,
                AdditionalParameters.ParseFilePath);
        }

        /// <summary>
        /// Parse a valid Snp file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(text-reader) method and validate with the 
        /// expected sequence.
        /// Input : Snp File
        /// Validation : Expected sequence, Chromosome position, Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        public void SnpBvtParserValidateParseTextReader()
        {
            SnpParserGeneralTestCases(Constants.SimpleSnpNodeName,
                AdditionalParameters.ParseTextReader);
        }

        /// <summary>
        /// Parse a valid Snp file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using ParseOne(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Snp File
        /// Validation : Expected sequence, Chromosome position, Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        public void SnpBvtParserValidateParseOneFilePath()
        {
            SnpParserGeneralTestCases(Constants.SimpleSnpNodeName,
                AdditionalParameters.ParseOneFilePath);
        }

        /// <summary>
        /// Parse a valid Snp file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using ParseOne(text-reader) method and validate with the 
        /// expected sequence.
        /// Input : Snp File
        /// Validation : Expected sequence, Chromosome position, Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        public void SnpBvtParserValidateParseOneTextReader()
        {
            SnpParserGeneralTestCases(Constants.SimpleSnpNodeName,
                AdditionalParameters.ParseOneTextReader);
        }

        /// <summary>
        /// Validate All properties in Snp parser class
        /// Input : One line sequence and update all properties
        /// Validation : Validate the properties
        /// </summary>
        [Test]
        public void SnpBvtParserProperties()
        {
            SimpleSnpParser snpParser = new SimpleSnpParser();
            Assert.AreEqual(Constants.SnpDescription, snpParser.Description);
            Assert.AreEqual(Constants.SnpFileTypes, snpParser.FileTypes);
            Assert.AreEqual(Constants.SnpName, snpParser.Name);
            Assert.IsTrue(snpParser.ParseAlleleOne);
            Assert.AreEqual(Constants.SnpFileTypes, snpParser.FileTypes);

            Console.WriteLine(
                "Successfully validated all the properties of Snp Parser class.");
            ApplicationLog.WriteLine
                ("Successfully validated all the properties of Snp Parser class.");
        }

        /// <summary>
        /// Parse a valid Snp file and convert the same to one sequence 
        /// using Parse(file-name) method, with ParseAlleleOne property set to false
        /// and validate with the expected sequence.
        /// Input : Snp File
        /// Validation : Expected sequence, Chromosome position, Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        public void SnpBvtParserValidateParseAlleleTwo()
        {
            SnpParserGeneralTestCases(Constants.SimpleSnpNodeName,
                AdditionalParameters.ParseAlleleTwo);
        }

        /// <summary>
        /// Parse a valid Snp file with one line and convert the 
        /// same to sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Snp File
        /// Validation : Expected sequence, Chromosome position, Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        public void SnpBvtParserValidateParseFilePathOneLine()
        {
            SnpParserGeneralTestCases(Constants.OneLineSnpNode,
                AdditionalParameters.ParseFilePath);
        }

        #endregion Test cases

        #region XsvSnpReader Test cases

        /// <summary>
        /// Parse a valid Snp file and validate the SkipToChromosomePosition() method
        /// Input : Snp File
        /// Validation : Validate if the Skip is successful
        /// </summary>
        [Test]
        public void XsvBvtSnpReaderSkipToChromosomePosition()
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(Constants.SimpleSnpNodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "XsvSnpReader BVT: File Exists in the Path '{0}'.",
                filePath));

            XsvSnpReader rdr = new XsvSnpReader(
                new StreamReader(filePath), new Char[1] { '\t' },
                false, true, 0, 1, 2, 3);

            // Validate with IsChromosomeSorted and IsChromosomePositionStarted set to false
            rdr.MoveNext();
            Assert.IsFalse(rdr.SkipToChromosomePosition(0));
            // Validates the Comment Line Property
            Assert.IsNull(rdr.CommentLine);
            rdr.Dispose();

            rdr = new XsvSnpReader(new StreamReader(filePath), new Char[1] { '\t' },
                false, true, 0, 1, 2, 3);
            rdr.MoveNext();
            rdr.IsChromosomeSorted = true;
            // Validate with IsChromosomeSorted property set
            Assert.IsFalse(rdr.SkipToChromosomePosition(0, 0));

            rdr.MoveNext();
            rdr.IsChromosomePositionSorted = true;
            // Validate with IsChromosomePositionSorted property set
            Assert.IsFalse(rdr.SkipToChromosomePosition(0, 0));

            // Logs information to the log file
            ApplicationLog.WriteLine(
                "XsvSnpReader BVT: Successfully validated the SkipToChromosomePosition() method");
            Console.WriteLine(
                "XsvSnpReader BVT: Successfully validated the SkipToChromosomePosition() method");
        }

        #endregion XsvSnpReader Test cases

        #region Supporting Methods

        /// <summary>
        /// Snp parser generic method called by all the test cases 
        /// to validate the test case based on the parameters passed.
        /// </summary>
        /// <param name="nodename">Xml node Name.</param>
        /// <param name="additionalParam">Additional parameter 
        /// based on which the validation of  test case is done.</param>
        static void SnpParserGeneralTestCases(string nodename, AdditionalParameters additionalParam)
        {
            // Gets the expected sequence from the Xml
            string filepath = Utility._xmlUtil.GetTextValue(nodename,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filepath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Snp Parser BVT: File Exists in the Path '{0}'.",
                filepath));

            IList<ISequence> seqList = null;
            SparseSequence sparseSeq = null;
            SimpleSnpParser parser = new SimpleSnpParser();

            string expectedPosition = Utility._xmlUtil.GetTextValue(nodename,
                Constants.ExpectedPositionNode);

            string[] expectedPositions = expectedPosition.Split(',');
            string[] expectedCharacters = null;

            switch (additionalParam)
            {
                case AdditionalParameters.ParseAlleleTwo:
                    parser.ParseAlleleOne = false;
                    string expectedAlleleTwoSequence =
                        Utility._xmlUtil.GetTextValue(nodename,
                        Constants.ExpectedSequenceAllele2Node);
                    expectedCharacters = expectedAlleleTwoSequence.Split(',');
                    break;
                default:
                    string expectedSequence = Utility._xmlUtil.GetTextValue(nodename,
              Constants.ExpectedSequenceNode);
                    expectedCharacters = expectedSequence.Split(',');
                    break;
            }

            switch (additionalParam)
            {
                case AdditionalParameters.ParseTextReader:
                    seqList = parser.Parse(new StreamReader(filepath));
                    sparseSeq = (SparseSequence)seqList[0];
                    break;
                case AdditionalParameters.ParseOneTextReader:
                    sparseSeq =
                        (SparseSequence)parser.ParseOne(new StreamReader(filepath));
                    break;
                case AdditionalParameters.ParseOneFilePath:
                    sparseSeq = (SparseSequence)parser.ParseOne(filepath);
                    break;
                default:
                    seqList = parser.Parse(filepath);
                    sparseSeq = (SparseSequence)seqList[0];
                    break;
            }

            if (null == sparseSeq)
            {
                Assert.IsNotNull(seqList);
                Assert.AreEqual(1, seqList.Count);
                ApplicationLog.WriteLine(string.Format(null,
                    "Snp Parser BVT: Number of Sequences found are '{0}'.",
                    seqList.Count.ToString((IFormatProvider)null)));
            }

            for (int i = 0; i < expectedPositions.Length; i++)
            {
                ISequenceItem item = sparseSeq[int.Parse(expectedPositions[i])];
                Assert.AreEqual(expectedCharacters[i], item.Symbol.ToString());
            }

            ApplicationLog.WriteLine(
                "Snp Parser BVT: The Snp sequence with position is validated successfully with Parse() method.");
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(
                "Snp Parser BVT: The Snp sequence with position is validated successfully with Parse() method.");

            Assert.IsNotNull(sparseSeq.Alphabet);
            Assert.AreEqual(sparseSeq.Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                Utility._xmlUtil.GetTextValue(nodename,
                Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture));

            ApplicationLog.WriteLine(string.Format(null,
                "Snp Parser BVT: The Sequence Alphabet is '{0}' and is as expected.",
                sparseSeq.Alphabet.Name));

            string expSequenceID = Utility._xmlUtil.GetTextValue(nodename,
                Constants.SequenceIdNode);

            Assert.AreEqual(expSequenceID, sparseSeq.ID);
            ApplicationLog.WriteLine(string.Format(null,
                "Snp Parser BVT: The Sequence ID is '{0}' and is as expected.", sparseSeq.ID));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Snp Parser BVT: The Sequence ID is '{0}' and is as expected.", sparseSeq.ID));
        }

        #endregion Supporting Methods
    }
}