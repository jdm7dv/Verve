// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * FastaBvtTestCases.cs
 * 
 *   This file contains the Fasta - Parsers and Formatters Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using MBF.IO.Fasta;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.IO.Fasta
{
    /// <summary>
    /// FASTA Bvt parser and formatter Test case implementation.
    /// </summary>
    [TestFixture]
    public class FastaBvtTestCases
    {

        #region Enums

        /// <summary>
        /// Additional parameters to validate different scenarios.
        /// </summary>
        enum AdditionalParameters
        {
            Parse,
            ParseOne,
            ParseReadOnly,
            ParseOneReadOnly,
            ParseReader,
            ParseReaderReadOnly,
            Properties
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static FastaBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region Fasta Parser Bvt Test cases

        /// <summary>
        /// Parse a valid FastA file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : FastA File
        /// Validation : Expected sequence, Sequence Length, Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        public void FastaParserValidateParse()
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser BVT: File Exists in the Path '{0}'.", filePath));

            IList<ISequence> seqsList = null;
            FastaParser parser = new FastaParser();

            seqsList = parser.Parse(filePath);

            Assert.IsNotNull(seqsList);
            Assert.AreEqual(1, seqsList.Count);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser BVT: Number of Sequences found are '{0}'.",
                seqsList.Count.ToString((IFormatProvider)null)));

            string expectedSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
               Constants.ExpectedSequenceNode);

            Sequence seq = (Sequence)seqsList[0];
            Assert.IsNotNull(seq);
            Assert.AreEqual(expectedSequence, seq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser BVT: The FASTA sequence '{0}' validation after Parse() is found to be as expected.",
                seq.ToString()));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "FastA Parser BVT: The FASTA sequence '{0}' validation after Parse() is found to be as expected.",
                seq.ToString()));

            Assert.AreEqual(expectedSequence.Length, seq.EncodedValues.Length);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser BVT: The FASTA Length sequence '{0}' is as expected.",
                expectedSequence.Length));

            Assert.IsNotNull(seq.Alphabet);
            Assert.AreEqual(seq.Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture));
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser BVT: The Sequence Alphabet is '{0}' and is as expected.",
                seq.Alphabet.Name));

            Assert.AreEqual(Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.SequenceIdNode), seq.ID);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser BVT: The Sequence ID is '{0}' and is as expected.", seq.ID));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "FastA Parser BVT: The Sequence ID is '{0}' and is as expected.", seq.ID));
        }

        /// <summary>
        /// Parse a valid FastA file (Small size sequence less than 35 kb) and 
        /// set the values for the properties BlockSize and MaxNumberOfBlocks and validate
        /// Parse(filename).
        /// Input : FastA File
        /// Validation : Expected sequence with BlockSize and MaxNumberOfBlocks set.
        /// </summary>
        [Test]
        public void FastaParserValidateVirtualParse()
        {
            ParseGeneralTestCases(Constants.MultipleSequenceDNADNAFastaNodeName,
                AdditionalParameters.Parse);
        }

        /// <summary>
        /// Parse a valid FastA file (Small size sequence less than 35 kb) and 
        /// set the values for the properties BlockSize and MaxNumberOfBlocks and validate
        /// ParseOne(filename).
        /// Input : FastA File
        /// Validation : Expected sequence with BlockSize and MaxNumberOfBlocks set.
        /// </summary>
        [Test]
        public void FastaParserValidateVirtualParseOne()
        {
            ParseGeneralTestCases(Constants.MultipleSequenceDNADNAFastaNodeName,
                AdditionalParameters.ParseOne);
        }

        /// <summary>
        /// Parse a valid FastA file (Small size sequence less than 35 kb) and 
        /// set the values for the properties BlockSize and MaxNumberOfBlocks and validate
        /// Parse(filename, readonly).
        /// Input : FastA File
        /// Validation : Expected sequence with BlockSize and MaxNumberOfBlocks set.
        /// </summary>
        [Test]
        public void FastaParserValidateVirtualParseReadOnly()
        {
            ParseGeneralTestCases(Constants.MultipleSequenceDNADNAFastaNodeName,
                AdditionalParameters.ParseReadOnly);
        }

        /// <summary>
        /// Parse a valid FastA file (Small size sequence less than 35 kb) and 
        /// set the values for the properties BlockSize and MaxNumberOfBlocks and validate
        /// ParseOne(filename, readonly).
        /// Input : FastA File
        /// Validation : Expected sequence with BlockSize and MaxNumberOfBlocks set.
        /// </summary>
        [Test]
        public void FastaParserValidateVirtualParseOneReadOnly()
        {
            ParseGeneralTestCases(Constants.MultipleSequenceDNADNAFastaNodeName,
                AdditionalParameters.ParseOneReadOnly);
        }

        /// <summary>
        /// Parse a valid FastA file (Small size sequence less than 35 kb) and 
        /// set the values for the properties BlockSize and MaxNumberOfBlocks and validate
        /// the property values.
        /// Input : FastA File
        /// Validation : Expected BlockSize and MaxNumberOfBlocks set.
        /// </summary>
        [Test]
        public void FastaParserValidateVirtualProperties()
        {
            ParseGeneralTestCases(Constants.MultipleSequenceDNADNAFastaNodeName,
                AdditionalParameters.Properties);
        }

        /// <summary>
        /// Parse a valid FastA file (Small size sequence less than 35 kb)
        /// and validate the ParseOne(reader).
        /// Input : FastA File
        /// Output : Expected result set.
        /// </summary>
        [Test]
        public void FastaParseOneValidateReader()
        {
            ParseReaderGeneralTestCases(Constants.SimpleFastaDnaNodeName,
                AdditionalParameters.ParseOne);
        }

        /// <summary>
        /// Parse a valid FastA file (Small size sequence less than 35 kb)
        /// and validate the ParseOne(reader, isReadOnly).
        /// Input : FastA File
        /// Output : Expected result set.
        /// </summary>
        [Test]
        public void FastaParseOneValidateReaderBool()
        {
            ParseReaderGeneralTestCases(Constants.SimpleFastaDnaNodeName,
                AdditionalParameters.ParseOneReadOnly);
        }

        /// <summary>
        /// Parse a valid FastA file (Small size sequence less than 35 kb)
        /// and validate the Parse(reader).
        /// Input : FastA File
        /// Output : Expected result set.
        /// </summary>
        [Test]
        public void FastaParseReaderValidateReader()
        {
            ParseReaderGeneralTestCases(Constants.SimpleFastaDnaNodeName,
                AdditionalParameters.ParseReader);
        }

        /// <summary>
        /// Parse a valid FastA file (Small size sequence less than 35 kb)
        /// and validate the Parse(reader, isReadOnly).
        /// Input : FastA File
        /// Output : Expected result set.
        /// </summary>
        [Test]
        public void FastaParseReaderValidateReaderBool()
        {
            ParseReaderGeneralTestCases(Constants.SimpleFastaDnaNodeName,
                AdditionalParameters.ParseReaderReadOnly);
        }

        #endregion Fasta Parser BVT Test cases

        #region Fasta Formatter Bvt Test cases

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastA file Format() method with Sequence and Writer as parameter
        /// and validate the same.
        /// Input : FastA Sequence
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormat()
        {
            FastaFormatter formatter = new FastaFormatter();

            // Gets the actual sequence and the alphabet from the Xml
            string actualSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequenceNode);
            string alpName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: Validating with Sequence '{0}' and Alphabet '{1}'.",
                actualSequence, alpName));

            Sequence seqOriginal = new Sequence(Utility.GetAlphabet(alpName),
                actualSequence);
            Assert.IsNotNull(seqOriginal);

            // Use the formatter to write the original sequences to a temp file            
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: Creating the Temp file '{0}'.", Constants.FastaTempFileName));

            formatter.Format(seqOriginal, Constants.FastaTempFileName);


            // Read the new file, then compare the sequences
            IList<ISequence> seqsNew = null;
            FastaParser parser = new FastaParser();
            seqsNew = parser.Parse(Constants.FastaTempFileName);
            Assert.IsNotNull(seqsNew);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: New Sequence is '{0}'.",
                seqsNew[0].ToString()));

            // Now compare the sequences.
            int countNew = seqsNew.Count();
            Assert.AreEqual(1, countNew);
            ApplicationLog.WriteLine("The Number of sequences are matching.");

            Assert.AreEqual(seqOriginal.ID, seqsNew[0].ID);
            string orgSeq = seqOriginal.ToString();
            string newSeq = seqsNew[0].ToString();
            Assert.AreEqual(orgSeq, newSeq);
            Console.WriteLine(string.Format(null,
                "FastA Formatter BVT: The FASTA sequences '{0}' are matching with Format() method and is as expected.",
                seqsNew[0].ToString()));
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: The FASTA sequences '{0}' are matching with Format() method.",
                seqsNew[0].ToString()));

            // Passed all the tests, delete the tmp file. If we failed an Assert,
            // the tmp file will still be there in case we need it for debugging.
            File.Delete(Constants.FastaTempFileName);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastA file FormatString() method and validate the same.
        /// Input : FastA Sequence
        /// Validation : Validate the output of FormatString() method with the expected sequence
        /// </summary>
        [Test]
        public void FastaFormatterValidateFormatString()
        {
            FastaFormatter formatter = new FastaFormatter();

            // Gets the actual sequence and the alphabet from the Xml
            string actualSequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequenceNode);
            string alpName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: Validating with Sequence '{0}' and Alphabet '{1}'.",
                actualSequence, alpName));

            Sequence seqOriginal = new Sequence(Utility.GetAlphabet(alpName),
                actualSequence);
            Assert.IsNotNull(seqOriginal);
            seqOriginal.ID = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.SequenceIdNode);

            string formatStrOrginal = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.FormatStringNode);
            string formatStr = formatter.FormatString(seqOriginal);
            formatStr = formatStr.Replace("\r", "").Replace("\n", "");

            Assert.AreEqual(formatStrOrginal, formatStr);
            Console.WriteLine(
                string.Format(null,
                "FastA Formatter BVT: The FASTA sequences '{0}' are matching with FormatString() method and is as expected.",
                formatStrOrginal));
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: The FASTA sequences '{0}' are matching with FormatString() method.",
                formatStrOrginal));
        }

        /// <summary>
        /// Parse a FastA File (Small size sequence less than 35 kb) using Parse() method 
        /// and Format the same to a FastA file using Format() method with sequence
        /// and writer as parameter and validate the same.
        /// Input : FastA File which would be parsed
        /// Validation : Read the New FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [Test]
        public void FastaFormatterWithParseValidateFormat()
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.FilePathNode);
            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: File Exists in the Path '{0}'.", filePath));

            FastaParser parser = new FastaParser();
            FastaFormatter formatter = new FastaFormatter();

            // Read the original file
            IList<ISequence> seqsOriginal = null;
            parser = new FastaParser();
            seqsOriginal = parser.Parse(filePath);
            Assert.IsNotNull(seqsOriginal);

            // Use the formatter to write the original sequences to a temp file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: Creating the Temp file '{0}'.",
                Constants.FastaTempFileName));

            using (TextWriter writer = new StreamWriter(Constants.FastaTempFileName))
            {
                foreach (Sequence s in seqsOriginal)
                {
                    formatter.Format(s, writer);
                }
            }

            // Read the new file, then compare the sequences
            IList<ISequence> seqsNew = null;
            parser = new FastaParser();
            seqsNew = parser.Parse(Constants.FastaTempFileName);
            Assert.IsNotNull(seqsNew);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: New Sequence is '{0}'.",
                seqsNew[0].ToString()));

            // Now compare the sequences.
            int countOriginal = seqsOriginal.Count();
            int countNew = seqsNew.Count();
            Assert.AreEqual(countOriginal, countNew);
            ApplicationLog.WriteLine("The Number of sequences are matching.");

            int i;
            for (i = 0; i < countOriginal; i++)
            {
                Assert.AreEqual(seqsOriginal[i].ID, seqsNew[i].ID);
                string orgSeq = seqsOriginal[i].ToString();
                string newSeq = seqsNew[i].ToString();
                Assert.AreEqual(orgSeq, newSeq);
                Console.WriteLine(string.Format(null,
                    "FastA Formatter BVT: The FASTA sequences '{0}' are matching with Format() method and is as expected.",
                    seqsNew[i].ID));
                ApplicationLog.WriteLine(string.Format(null,
                    "FastA Formatter BVT: The FASTA sequences '{0}' are matching with Format() method.",
                    seqsNew[i].ID));
            }

            // Passed all the tests, delete the tmp file. If we failed an Assert,
            // the tmp file will still be there in case we need it for debugging.
            File.Delete(Constants.FastaTempFileName);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        /// <summary>
        /// Parse a FastA File (Small size sequence less than 35 kb) using Parse() method 
        /// and Format the same to a FastA file using FormatString() method and validate the same.
        /// Input :  FastA File which would be parsed
        /// Validation : Validate the output of FormatString() method with the expected sequence
        /// </summary>
        [Test]
        public void FastaFormatterWithParseValidateFormatString()
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: File Exists in the Path '{0}'.", filePath));

            FastaParser parser = new FastaParser();
            FastaFormatter formatter = new FastaFormatter();

            // Read the original file
            IList<ISequence> seqsOriginal = null;
            parser = new FastaParser();
            seqsOriginal = parser.Parse(filePath);
            Assert.IsNotNull(seqsOriginal);

            string formatStrOrginal = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName,
                Constants.FormatStringNode);
            string formatStr = formatter.FormatString(seqsOriginal[0]).Trim();
            formatStr = formatStr.Replace("\r", "").Replace("\n", "");

            Assert.AreEqual(formatStrOrginal, formatStr);
            Console.WriteLine(string.Format(null,
                "FastA Formatter BVT: The FASTA sequences '{0}' are matching with FormatString() method and is as expected.",
                formatStrOrginal));
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Formatter BVT: The FASTA sequences '{0}' are matching with FormatString() method.",
                formatStrOrginal));
        }

        #endregion Fasta Formatter Bvt Test cases

        #region Supported Methods

        /// <summary>
        /// Parse General test cases for Data Virtualization
        /// </summary>
        /// <param name="nodeName">Xml node name</param>
        /// <param name="addParam">Additional parameter</param>
        static void ParseGeneralTestCases(string nodeName,
            AdditionalParameters addParam)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser BVT: File Exists in the Path '{0}'.", filePath));
            Console.WriteLine(string.Format(null,
                "FastA Parser BVT: File Exists in the Path '{0}'.", filePath));

            IList<ISequence> seqsList = null;
            FastaParser parserObj = new FastaParser();
            parserObj.EnforceDataVirtualization = true;
            Sequence parseOneSeq = null;

            string expectedSequence = Utility._xmlUtil.GetTextValue(nodeName,
               Constants.ExpectedSequenceNode);

            string[] expectedSequences = expectedSequence.Split(',');

            // Gets the SequenceAlignment list based on the parameters.
            switch (addParam)
            {
                case AdditionalParameters.Parse:
                case AdditionalParameters.Properties:
                    seqsList = parserObj.Parse(filePath);
                    break;
                case AdditionalParameters.ParseOne:
                    parseOneSeq = (Sequence)parserObj.ParseOne(filePath);
                    break;
                case AdditionalParameters.ParseReadOnly:
                    seqsList = parserObj.Parse(filePath,
                        false);
                    break;
                case AdditionalParameters.ParseOneReadOnly:
                    parseOneSeq = (Sequence)parserObj.ParseOne(filePath,
                        false);
                    break;
                default:
                    break;
            }

            // Check if ParseOne or Parse was used for parsing
            if (null == seqsList)
            {
                seqsList = new List<ISequence>();
                seqsList.Add(parseOneSeq);
                ApplicationLog.WriteLine(string.Format(null,
                    "FastA Parser BVT: Number of Sequences found are '{0}'.",
                    seqsList.Count.ToString((IFormatProvider)null)));
                Console.WriteLine(string.Format(null,
                    "FastA Parser BVT: Number of Sequences found are '{0}'.",
                    seqsList.Count.ToString((IFormatProvider)null)));
            }
            else
            {
                Assert.IsNotNull(seqsList);
                Assert.AreEqual(2, seqsList.Count);
                ApplicationLog.WriteLine(string.Format(null,
                    "FastA Parser BVT: Number of Sequences found are '{0}'.",
                    seqsList.Count.ToString((IFormatProvider)null)));
                Console.WriteLine(string.Format(null,
                    "FastA Parser BVT: Number of Sequences found are '{0}'.",
                    seqsList.Count.ToString((IFormatProvider)null)));
            }

            // Validating by setting the BlockSize and MaxNumberOfBlocks
            int seqNumber = 0;
            foreach (Sequence seq in seqsList)
            {
                //seq.BlockSize = 5;
                seq.MaxNumberOfBlocks = 5;
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < seq.Count; i++)
                {
                    sb.Append(seq[i].Symbol.ToString());
                }

                switch (addParam)
                {
                    case AdditionalParameters.Properties:
                        //Assert.AreEqual(5, seq.BlockSize);
                        Assert.AreEqual(5, seq.MaxNumberOfBlocks);
                        ApplicationLog.WriteLine("FastA Parser BVT: The Properties are as expected.");
                        Console.WriteLine("FastA Parser BVT: The Properties are as expected.");
                        break;
                    default:
                        Assert.AreEqual(expectedSequences[seqNumber], sb.ToString());

                        ApplicationLog.WriteLine(string.Format(null,
                            "FastA Parser BVT: The FASTA sequence '{0}' validation after Parse() is found to be as expected.",
                            expectedSequences[seqNumber]));
                        // Logs to the NUnit GUI (Console.Out) window
                        Console.WriteLine(string.Format(null,
                            "FastA Parser BVT: The FASTA sequence '{0}' validation after Parse() is found to be as expected.",
                            expectedSequences[seqNumber]));

                        Assert.AreEqual(expectedSequences[seqNumber].Length, seq.Count);
                        ApplicationLog.WriteLine(string.Format(null,
                            "FastA Parser BVT: The FASTA Length sequence '{0}' is as expected.",
                            expectedSequences[seqNumber].Length));

                        string[] alphabets = Utility._xmlUtil.GetTextValue(nodeName,
                            Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture).Split(',');
                        Assert.IsNotNull(seq.Alphabet);
                        Assert.AreEqual(seq.Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                           alphabets[seqNumber]);
                        ApplicationLog.WriteLine(string.Format(null,
                            "FastA Parser BVT: The Sequence Alphabet is '{0}' and is as expected.",
                            seq.Alphabet.Name));

                        string[] seqIDs = Utility._xmlUtil.GetTextValue(nodeName,
                            Constants.SequenceIdNode).ToLower(CultureInfo.CurrentCulture).Split('/');
                        Assert.AreEqual(seqIDs[seqNumber].ToLower(CultureInfo.CurrentCulture)
                            , seq.ID.ToLower(CultureInfo.CurrentCulture));
                        ApplicationLog.WriteLine(string.Format(null,
                            "FastA Parser BVT: The Sequence ID is '{0}' and is as expected.",
                            seq.ID));
                        // Logs to the NUnit GUI (Console.Out) window
                        Console.WriteLine(string.Format(null,
                            "FastA Parser BVT: The Sequence ID is '{0}' and is as expected.",
                            seq.ID));
                        break;
                }
                seqNumber++;
            }
        }

        /// <summary>
        /// ParseOne General test cases
        /// </summary>
        /// <param name="nodeName">Xml node name</param>
        /// <param name="addParam">Additional parameter</param>
        static void ParseReaderGeneralTestCases(string nodeName,
            AdditionalParameters addParam)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser BVT: File Exists in the Path '{0}'.", filePath));

            ISequence seqsObj = null;
            FastaParser parserObj = new FastaParser();

            switch (addParam)
            {
                case AdditionalParameters.ParseOne:
                    using (TextReader reader = new StreamReader(filePath))
                    {
                        seqsObj = parserObj.ParseOne(reader);
                    }
                    break;
                case AdditionalParameters.ParseOneReadOnly:
                    using (TextReader reader = new StreamReader(filePath))
                    {
                        seqsObj = parserObj.ParseOne(reader, true);
                    }
                    break;
                case AdditionalParameters.ParseReader:
                    using (TextReader reader = new StreamReader(filePath))
                    {
                        seqsObj = (Sequence)parserObj.Parse(reader)[0];
                    }
                    break;
                case AdditionalParameters.ParseReaderReadOnly:
                    using (TextReader reader = new StreamReader(filePath))
                    {
                        seqsObj = (Sequence)parserObj.Parse(reader, true)[0];
                    }
                    break;
                default:
                    break;
            }

            Assert.IsNotNull(seqsObj);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser BVT: Number of Sequences found are '{0}'.",
                seqsObj.Count.ToString((IFormatProvider)null)));

            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaDnaNodeName,
                Constants.ExpectedSequenceNode);

            Assert.IsNotNull(seqsObj);
            Assert.AreEqual(expectedSequence, seqsObj.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser BVT: The FASTA sequence '{0}' validation after Parse() is found to be as expected.",
                seqsObj.ToString()));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "FastA Parser BVT: The FASTA sequence '{0}' validation after Parse() is found to be as expected.",
                seqsObj.ToString()));

            Assert.IsNotNull(seqsObj.Alphabet);
            Assert.AreEqual(
                seqsObj.Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                Utility._xmlUtil.GetTextValue(Constants.SimpleFastaDnaNodeName,
                Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture));
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser BVT: The Sequence Alphabet is '{0}' and is as expected.",
                seqsObj.Alphabet.Name));

            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaDnaNodeName,
                Constants.SequenceIdNode), seqsObj.ID);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser BVT: The Sequence ID is '{0}' and is as expected.",
                seqsObj.ID));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "FastA Parser BVT: The Sequence ID is '{0}' and is as expected.",
                seqsObj.ID));
        }

        #endregion Supported Methods
    }
}