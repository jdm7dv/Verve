// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * PhylipBvtTestCases.cs
 * 
 *   This file contains the Phylip - Parsers and Formatters Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

using MBF.Algorithms.Alignment;
using MBF.Encoding;
using MBF.IO;
using MBF.IO.Phylip;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.IO.Phylip
{
    /// <summary>
    /// Phylip Bvt parser and formatter Test case implementation.
    /// </summary>
    [TestFixture]
    public class PhylipBvtTestCases
    {

        #region Enums

        /// <summary>
        /// Additional parameters to validate different scenarios.
        /// </summary>
        enum ParserTestAttributes
        {
            Parse,
            ParseOne,
            ParseTextReader,
            ParseOneTextReader,
            ParseReadOnly,
            ParseTextReaderReadOnly,
            ParseOneReadOnly,
            ParseOneTextReaderReadOnly,
            ParseEncoding
        };

        /// <summary>
        /// Additional parameters to validate Common Sequence Parser scenarios.
        /// </summary>
        enum CommonSequenceParserAttributes
        {
            ParseRNA,
            ParseProtein
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static PhylipBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\PhylipTestsConfig.xml");
        }

        #endregion Constructor

        #region Phylip Parser BVT Test cases

        /// <summary>
        /// Parse a valid Phylip file (Small size sequence less than 35 kb) and 
        /// convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : Phylip File
        /// Validation: Sequence Alignment list
        /// </summary>
        [Test]
        public void PhylipParserValidateParseFileName()
        {
            ParserGeneralTestCases(Constants.SmallSizePhylipNodeName,
                ParserTestAttributes.Parse);
        }

        /// <summary>
        /// Parse a valid Phylip file (Small size sequence less than 35 kb) and 
        /// convert the same to one sequence using ParseOne(file-name) method and 
        /// validate with the expected sequence.
        /// Input : Phylip File
        /// Validation: Sequence Alignment list
        /// </summary>
        [Test]
        public void PhylipParserValidateParseOneFileName()
        {
            ParserGeneralTestCases(Constants.SmallSizePhylipNodeName,
                ParserTestAttributes.ParseOne);
        }

        /// <summary>
        /// Parse a valid Phylip file (Small size sequence less than 35 kb) and 
        /// convert the same to one sequence using ParseOne(file-name, read-only) method and 
        /// validate with the expected sequence.
        /// Input : Phylip File
        /// Validation: Sequence Alignment list
        /// </summary>
        [Test]
        public void PhylipParserValidateParseOneFileNameReadOnly()
        {
            ParserGeneralTestCases(Constants.SmallSizePhylipNodeName,
                ParserTestAttributes.ParseOneReadOnly);
        }

        /// <summary>
        /// Parse a valid Phylip file (Small size sequence less than 35 kb) and 
        /// convert the same to one sequence using Parse(file-name, read-only) method and 
        /// validate with the expected sequence.
        /// Input : Phylip File
        /// Validation: Sequence Alignment list
        /// </summary>
        [Test]
        public void PhylipParserValidateParseFileNameReadOnly()
        {
            ParserGeneralTestCases(Constants.SmallSizePhylipNodeName,
                ParserTestAttributes.ParseReadOnly);
        }

        /// <summary>
        /// Parse a valid Phylip file (Small size sequence less than 35 kb) and 
        /// convert the same to one sequence using ParseOne(text-reader) method and 
        /// validate with the expected sequence.
        /// Input : Phylip File
        /// Validation: Sequence Alignment list
        /// </summary>
        [Test]
        public void PhylipParserValidateParseOneTextReader()
        {
            ParserGeneralTestCases(Constants.SmallSizePhylipNodeName,
                ParserTestAttributes.ParseOneTextReader);
        }

        /// <summary>
        /// Parse a valid Phylip file (Small size sequence less than 35 kb) and 
        /// convert the same to one sequence using ParseOne(text-reader, read-only) method and 
        /// validate with the expected sequence.
        /// Input : Phylip File
        /// Validation: Sequence Alignment list
        /// </summary>
        [Test]
        public void PhylipParserValidateParseOneTextReaderReadOnly()
        {
            ParserGeneralTestCases(Constants.SmallSizePhylipNodeName,
                ParserTestAttributes.ParseOneTextReaderReadOnly);
        }

        /// <summary>
        /// Parse a valid Phylip file (Small size sequence less than 35 kb) and 
        /// convert the same to one sequence using Parse(text-reader) method and 
        /// validate with the expected sequence.
        /// Input : Phylip File
        /// Validation: Sequence Alignment list
        /// </summary>
        [Test]
        public void PhylipParserValidateParseTextReader()
        {
            ParserGeneralTestCases(Constants.SmallSizePhylipNodeName,
                ParserTestAttributes.ParseTextReader);
        }

        /// <summary>
        /// Parse a valid Phylip file (Small size sequence less than 35 kb) and 
        /// convert the same to one sequence using Parse(text-reader, read-only) method and 
        /// validate with the expected sequence.
        /// Input : Phylip File
        /// Validation: Sequence Alignment list
        /// </summary>
        [Test]
        public void PhylipParserValidateParseTextReaderReadOnly()
        {
            ParserGeneralTestCases(Constants.SmallSizePhylipNodeName,
                ParserTestAttributes.ParseTextReaderReadOnly);
        }

        /// <summary>
        /// Parse a valida Phylip Parser object and validate its properties
        /// Input : Valide Object
        /// Output : Validatation of properties
        /// </summary>
        [Test]
        public void ValidatePhylipParserProperties()
        {
            PhylipParser parser = new PhylipParser(Encodings.Ncbi4NA);
            Assert.AreEqual(
                Utility._xmlUtil.GetTextValue(Constants.PhylipPropertyNode,
                Constants.PhylipDescriptionNode),
                parser.Description);
            Assert.AreEqual(
                Utility._xmlUtil.GetTextValue(Constants.PhylipPropertyNode,
                Constants.PhylipNameNode),
                parser.Name);
            Assert.AreEqual(
                Utility._xmlUtil.GetTextValue(Constants.PhylipPropertyNode,
                Constants.PhylipFileTypesNode),
                parser.FileTypes);
            Assert.AreEqual(null, parser.Alphabet);
            Assert.AreEqual(
                Utility._xmlUtil.GetTextValue(Constants.PhylipPropertyNode,
                Constants.PhylipEncodingNode),
                parser.Encoding.ToString());
        }

        /// <summary>
        /// Parse a valid Phylip file (Small size sequence less than 35 kb) and 
        /// convert the same to one sequence using Parse(text-reader, read-only) method and 
        /// validate with the expected sequence.
        /// Input : Phylip File
        /// Validation: Sequence Alignment list
        /// </summary>
        [Test]
        public void PhylipParserValidateParseEncoding()
        {
            ParserGeneralTestCases(Constants.PhylipParserEncodingNode,
                ParserTestAttributes.ParseEncoding);
        }

        /// <summary>
        /// Validate IdentifyAlphabet methods of CommonSequenceParser 
        /// by passing valid RNA sequences.
        /// Input : Valid sequence
        /// Validation: Validate Alphabet
        /// </summary>
        [Test]
        public void CommonSequenceParserValidateIdentifyAlphabetRna()
        {
            CommonSequenceParserGeneralTestCases(Constants.CommonSequenceParserRNA,
                CommonSequenceParserAttributes.ParseRNA);
        }

        /// <summary>
        /// Validate IdentifyAlphabet methods of CommonSequenceParser 
        /// by passing valid protein sequences.
        /// Input : Valid sequence
        /// Validation: Validate Alphabet
        /// </summary>
        [Test]
        public void CommonSequenceParserValidateIdentifyAlphabetProtein()
        {
            CommonSequenceParserGeneralTestCases(Constants.CommonSequenceParserProtein,
                CommonSequenceParserAttributes.ParseProtein);
        }

        /// <summary>
        /// Validate GetMoleculeType(strType) methods of CommonSequenceParser 
        /// by passing valid sequences.
        /// Input : Valid sequence
        /// Validation: Validate MoleculeType
        /// </summary>
        [Test]
        public void CommonSequenceParserValidateGetMoleculeTypeString()
        {
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetMoleculeType(
                Constants.DNA).ToString());
            Assert.AreEqual(MoleculeType.DNA,
                CommonSequenceParser.GetMoleculeType(Constants.DNA));
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetMoleculeType(
                Constants.NA).ToString());
            Assert.AreEqual(MoleculeType.NA,
                CommonSequenceParser.GetMoleculeType(Constants.NA));
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetMoleculeType(
                Constants.RNA).ToString());
            Assert.AreEqual(MoleculeType.RNA,
                CommonSequenceParser.GetMoleculeType(Constants.RNA));
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetMoleculeType(
                Constants.TRNA).ToString());
            Assert.AreEqual(MoleculeType.tRNA,
                CommonSequenceParser.GetMoleculeType(Constants.TRNA));
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetMoleculeType(
                Constants.RRNA).ToString());
            Assert.AreEqual(MoleculeType.rRNA,
                CommonSequenceParser.GetMoleculeType(Constants.RRNA));
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetMoleculeType(
                Constants.MRNA).ToString());
            Assert.AreEqual(MoleculeType.mRNA,
                CommonSequenceParser.GetMoleculeType(Constants.MRNA));
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetMoleculeType(
                Constants.URNA).ToString());
            Assert.AreEqual(MoleculeType.uRNA,
                CommonSequenceParser.GetMoleculeType(Constants.URNA));
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetMoleculeType(
                Constants.SNRNA).ToString());
            Assert.AreEqual(MoleculeType.snRNA,
                CommonSequenceParser.GetMoleculeType(Constants.SNRNA));
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetMoleculeType(
                Constants.SNORNA).ToString());
            Assert.AreEqual(MoleculeType.snoRNA,
                CommonSequenceParser.GetMoleculeType(Constants.SNORNA));
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetMoleculeType(
                Constants.PROTEIN).ToString());
            Assert.AreEqual(MoleculeType.Protein,
                CommonSequenceParser.GetMoleculeType(Constants.PROTEIN));
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetMoleculeType(
                String.Empty).ToString());
            Assert.AreEqual(MoleculeType.Invalid,
                CommonSequenceParser.GetMoleculeType(String.Empty));

            ApplicationLog.WriteLine(
                "CommonSequenceParser BVT : All the features validated successfully.");
            Console.WriteLine(
                "CommonSequenceParser BVT : All the features validated successfully.");
        }

        /// <summary>
        /// Validate GetAlphabets methods of CommonSequenceParser 
        /// by passing valid sequences.
        /// Input : Valid Molecule Type
        /// Validation: Validate Alphabet
        /// </summary>
        [Test]
        public void CommonSequenceParserValidateGetAlphabets()
        {
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetAlphabet(
                MoleculeType.NA).ToString());
            Assert.AreEqual(Alphabets.DNA,
                CommonSequenceParser.GetAlphabet(MoleculeType.NA));
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetAlphabet(
                MoleculeType.snoRNA).ToString());
            Assert.AreEqual(Alphabets.RNA,
                CommonSequenceParser.GetAlphabet(MoleculeType.snoRNA));
            Assert.IsNotNullOrEmpty(CommonSequenceParser.GetAlphabet(
                MoleculeType.RNA).ToString());
            Assert.AreEqual(Alphabets.Protein,
                CommonSequenceParser.GetAlphabet(MoleculeType.Protein));
            Assert.IsNull(
                CommonSequenceParser.GetAlphabet(MoleculeType.Invalid));

            ApplicationLog.WriteLine(
                "CommonSequenceParser BVT : All the features validated successfully.");
            Console.WriteLine(
                "CommonSequenceParser BVT : All the features validated successfully.");
        }

        #endregion Phylip Parser BVT Test cases

        #region Supported Methods

        /// <summary>
        /// Parsers the Phylip file for different test cases based
        /// on Additional parameter
        /// </summary>
        /// <param name="nodeName">Xml Node name</param>
        /// <param name="addParam">Additional parameter</param>
        static void ParserGeneralTestCases(string nodeName,
            ParserTestAttributes addParam)
        {
            // Gets the Filename
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);

            Assert.IsNotEmpty(filePath);
            ApplicationLog.WriteLine(string.Format(
                "Phylip Parser BVT: Reading the File from location '{0}'", filePath));
            Console.WriteLine(string.Format(
                "Phylip Parser BVT: Reading the File from location '{0}'", filePath));

            // Get the rangelist after parsing.
            PhylipParser parserObj = new PhylipParser();

            IList<ISequenceAlignment> sequenceAlignmentList = null;
            ISequenceAlignment sequenceAlignment = null;

            // Gets the SequenceAlignment list based on the parameters.
            switch (addParam)
            {
                case ParserTestAttributes.Parse:
                    sequenceAlignmentList = parserObj.Parse(filePath);
                    break;
                case ParserTestAttributes.ParseOne:
                    sequenceAlignment = parserObj.ParseOne(filePath);
                    break;
                case ParserTestAttributes.ParseTextReader:
                    sequenceAlignmentList = parserObj.Parse(
                        new StreamReader(filePath));
                    break;
                case ParserTestAttributes.ParseOneTextReader:
                    sequenceAlignment = parserObj.ParseOne(
                        new StreamReader(filePath));
                    break;
                case ParserTestAttributes.ParseOneTextReaderReadOnly:
                    sequenceAlignment = parserObj.ParseOne(
                        new StreamReader(filePath), false);
                    break;
                case ParserTestAttributes.ParseTextReaderReadOnly:
                    sequenceAlignmentList = parserObj.Parse(
                        new StreamReader(filePath), false);
                    break;
                case ParserTestAttributes.ParseReadOnly:
                    sequenceAlignmentList = parserObj.Parse(filePath,
                        false);
                    break;
                case ParserTestAttributes.ParseOneReadOnly:
                    sequenceAlignment = parserObj.ParseOne(filePath,
                        false);
                    break;
                case ParserTestAttributes.ParseEncoding:
                    PhylipParser parser =
                        new PhylipParser(Encodings.Ncbi4NA);
                    sequenceAlignmentList = parser.Parse(
                        new StreamReader(filePath), false);
                    break;
                default:
                    break;
            }

            // Gets all the expected values from xml.
            IList<Dictionary<string, string>> expectedAlignmentList =
                new List<Dictionary<string, string>>();
            Dictionary<string, string> expectedAlignmentObj =
                new Dictionary<string, string>();

            XmlNode expectedAlignmentNodes = Utility._xmlUtil.GetNode(
                nodeName, Constants.ExpectedAlignmentNode);
            XmlNodeList alignNodes = expectedAlignmentNodes.ChildNodes;

            // Create a ISequenceAlignment List
            switch (addParam)
            {
                case ParserTestAttributes.ParseOne:
                case ParserTestAttributes.ParseOneTextReader:
                case ParserTestAttributes.ParseOneTextReaderReadOnly:
                case ParserTestAttributes.ParseOneReadOnly:
                    sequenceAlignmentList = new List<ISequenceAlignment>();
                    sequenceAlignmentList.Add(sequenceAlignment);
                    break;
                default:
                    break;
            }

            foreach (XmlNode expectedAlignment in alignNodes)
            {
                expectedAlignmentObj[expectedAlignment.Name] =
                    expectedAlignment.InnerText;
            }

            expectedAlignmentList.Add(expectedAlignmentObj);

            Assert.IsTrue(CompareOutput(sequenceAlignmentList,
                expectedAlignmentList));
            ApplicationLog.WriteLine(
                "Phylip Parser BVT: Successfully validated all the Alignment Sequences");
            Console.WriteLine(
                "Phylip Parser BVT: Successfully validated all the Alignment Sequences");
        }

        /// <summary>
        /// General Test Case to validate CommonSequenceParser
        /// </summary>
        static void CommonSequenceParserGeneralTestCases(
            string sequence,
            CommonSequenceParserAttributes addParam)
        {
            IAlphabet sequenceAlphabet = null;
            CommonSequenceParser parser = new CommonSequenceParser();

            switch (addParam)
            {
                case CommonSequenceParserAttributes.ParseRNA:
                    sequenceAlphabet = parser.IdentifyAlphabet(Alphabets.RNA, sequence);
                    Assert.IsNotNull(sequenceAlphabet);
                    Assert.AreEqual(sequenceAlphabet, Alphabets.RNA);
                    break;
                case CommonSequenceParserAttributes.ParseProtein:
                    sequenceAlphabet = parser.IdentifyAlphabet(Alphabets.Protein, sequence);
                    Assert.IsNotNull(sequenceAlphabet);
                    Assert.AreEqual(sequenceAlphabet, Alphabets.Protein);
                    break;
            }

            ApplicationLog.WriteLine(
                "CommonSequenceParser BVT : All the features validated successfully.");
            Console.WriteLine(
                "CommonSequenceParser BVT : All the features validated successfully.");
        }

        /// <summary>
        /// Compare the actual output with expected output
        /// </summary>
        /// <param name="actualOutput">Actual output</param>
        /// <param name="expectedOutput">Expected output</param>
        /// <returns>True, if comparison is successful</returns>
        static bool CompareOutput(
                IList<ISequenceAlignment> actualOutput,
                IList<Dictionary<string, string>> expectedOutput)
        {
            if (expectedOutput.Count != actualOutput.Count)
            {
                return false;
            }

            int alignmentIndex = 0;

            // Validate each output alignment
            foreach (ISequenceAlignment alignment in actualOutput)
            {
                Dictionary<string, string> expectedAlignment =
                    expectedOutput[alignmentIndex];

                foreach (Sequence actualSequence in alignment.AlignedSequences[0].Sequences)
                {
                    if (0 != string.Compare(actualSequence.ToString(),
                            expectedAlignment[actualSequence.ID], true,
                            CultureInfo.CurrentCulture))
                    {
                        Console.WriteLine("Failed for " + actualSequence.ID);
                        return false;
                    }
                }

                alignmentIndex++;
            }

            return true;
        }

        #endregion Supported Methods
    }
}