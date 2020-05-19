// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * GffBvtTestCases.cs
 * 
 *   This file contains the Gff - Parsers and Formatters Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using MBF.Encoding;
using MBF.IO;
using MBF.IO.Gff;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.IO.Gff
{
    /// <summary>
    /// Gff Bvt parser and formatter Test case implementation.
    /// </summary>
    [TestFixture]
    public class GffBvtTestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static GffBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\GffTestsConfig.xml");
        }

        #endregion Constructor

        #region Gff Parser Bvt Test cases

        /// <summary>
        /// Parse a valid Gff file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Gff File
        /// Validation : Features, Expected sequence, Sequence Length, 
        /// Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        public void GffParserValidateParseFileName()
        {
            ValidateParseGeneralTestCases(Constants.SimpleGffNodeName, true);
        }

        /// <summary>
        /// Parse a valid Gff file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(text-reader) method and validate with the 
        /// expected sequence.
        /// Input : Gff File
        /// Validation : Features, Expected sequence, Sequence Length, 
        /// Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        public void GffParserValidateParseTextReader()
        {
            ValidateParseGeneralTestCases(Constants.SimpleGffNodeName, false);
        }

        /// <summary>
        /// Parse a valid Gff file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using ParseOne(file-name) method and validate with the 
        /// expected sequence.
        /// Input : Gff File
        /// Validation : Features, Expected sequence, Sequence Length, 
        /// Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        public void GffParserValidateParseOneFileName()
        {
            ValidateParseOneGeneralTestCases(Constants.SimpleGffNodeName, true);
        }

        /// <summary>
        /// Parse a valid Gff file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using ParseOne(text-reader) method and validate with the 
        /// expected sequence.
        /// Input : Gff File
        /// Validation : Features, Expected sequence, Sequence Length, 
        /// Sequence Alphabet, Sequence ID.
        /// </summary>
        [Test]
        public void GffParserValidateParseOneTextReader()
        {
            ValidateParseOneGeneralTestCases(Constants.SimpleGffNodeName, false);
        }

        /// <summary>
        /// Parse a valid Gff file with one line sequence and using Parse(file-name) method and 
        /// validate the expected sequence
        /// Input : Gff File
        /// Validation : Read the Gff file to which the sequence was formatted and 
        /// validate Features, Sequence, Sequence Count
        /// </summary>
        [Test]
        public void GffParserValidateParseWithOneLineSequence()
        {
            ValidateParseGeneralTestCases(Constants.OneLineSeqGffNodeName, true);
        }

        /// <summary>
        /// Parse a valid Gff file with one line sequence and
        /// using Parse(file-name) method and validate the expected sequence
        /// Input : Gff File
        /// Validation : Read the Gff file to which the sequence was formatted
        /// and validate Features, Sequence, Sequence Count
        /// </summary>
        [Test]
        public void GffParserValidateParseWithOneLineFeatures()
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGffFeaturesNode,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            IList<ISequence> seqs = null;
            BasicSequenceParser parserObj = new GffParser(Encodings.Ncbi4NA);

            seqs = parserObj.Parse(filePath);
            Sequence originalSequence = (Sequence)seqs[0];

            bool val = ValidateFeatures(originalSequence,
                Constants.OneLineSeqGffNodeName);

            Assert.IsTrue(val);

            filePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGffFeaturesReaderNode,
                Constants.FilePathNode);

            using (StreamReader reader = File.OpenText(filePath))
            {
                seqs.Add(parserObj.ParseOne(reader));
            }

            originalSequence = (Sequence)seqs[0];

            val = ValidateFeatures(originalSequence,
                Constants.OneLineSeqGffNodeName);

            Assert.IsTrue(val);
            ApplicationLog.WriteLine(
                "GFF Parser BVT : All the features validated successfully.");
            Console.WriteLine(
                "GFF Parser BVT : All the features validated successfully.");
        }

        #endregion Gff Parser BVT Test cases

        #region Gff Formatter BVT Test cases

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// Gff file Format(sequence, filename) method with Sequence and Writer as parameter
        /// and validate the same.
        /// Input : Gff Sequence
        /// Validation : Read the Gff file to which the sequence was formatted and 
        /// validate Features, Sequence, Sequence Count
        /// </summary>
        [Test]
        public void GffFormatterValidateFormatFileName()
        {
            ValidateFormatGeneralTestCases(Constants.SimpleGffNodeName, true, false);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// Gff file Format(sequence, text-writer) method with Sequence and Writer as parameter
        /// and validate the same.
        /// Input : Gff Sequence
        /// Validation : Read the Gff file to which the sequence was formatted and 
        /// validate Features, Sequence, Sequence Count
        /// </summary>
        [Test]
        public void GffFormatterValidateFormatTextWriter()
        {
            ValidateFormatGeneralTestCases(Constants.SimpleGffNodeName, false, false);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// Gff file Format(sequence list, filename) method with Sequence and Writer as parameter
        /// and validate the same.
        /// Input : Gff Sequence list
        /// Validation : Read the Gff file to which the sequence was formatted and 
        /// validate Features, Sequence, Sequence Count
        /// </summary>
        [Test]
        public void GffFormatterValidateFormatSequenceListFileName()
        {
            ValidateFormatGeneralTestCases(Constants.SimpleGffNodeName, true, true);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// Gff file Format(sequence list, text-writer) method with Sequence and Writer as parameter
        /// and validate the same.
        /// Input : Gff Sequence list
        /// Validation : Read the Gff file to which the sequence was formatted and 
        /// validate Features, Sequence, Sequence Count
        /// </summary>
        [Test]
        public void GffFormatterValidateFormatSequenceListTextWriter()
        {
            ValidateFormatGeneralTestCases(Constants.SimpleGffNodeName, false, true);
        }

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// Gff file FormatString() method with Sequence and Writer as parameter
        /// and validate the same.
        /// Input : Gff Sequence
        /// Validation : Read the Gff file to which the sequence was formatted and 
        /// validate Features, Sequence, Sequence Count
        /// </summary>
        [Test]
        public void GffFormatterValidateFormatString()
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(Constants.SimpleGffNodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            IList<ISequence> seqs = null;
            GffParser parserObj = new GffParser();
            seqs = parserObj.Parse(filePath);
            Sequence originalSequence = (Sequence)seqs[0];

            // Use the formatter to write the original sequences to a temp file            
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Formatter BVT: Creating the Temp file '{0}'.", Constants.GffTempFileName));

            GffFormatter formatter = new GffFormatter();
            formatter.ShouldWriteSequenceData = true;
            string formatString = formatter.FormatString(originalSequence);

            string expectedString = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGffNodeName, Constants.FormatStringNode);

            expectedString = expectedString.Replace("current-date",
                DateTime.Today.ToString("yyyy-MM-dd", null));
            expectedString =
                expectedString.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");
            string modifedformatString =
                formatString.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");

            Assert.AreEqual(modifedformatString, expectedString);
            Console.WriteLine(string.Format(null,
                "Gff Formatter BVT: The Gff Format String '{0}' are matching with FormatString() method and is as expected.",
                formatString));
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Formatter BVT: The Gff Format String '{0}' are matching with FormatString() method and is as expected.",
                formatString));

            // Passed all the tests, delete the tmp file. If we failed an Assert,
            // the tmp file will still be there in case we need it for debugging.
            File.Delete(Constants.GffTempFileName);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        #endregion Gff Formatter BVT Test cases

        #region Supported Methods

        /// <summary>
        /// Parses all test cases related to Parse() method based on the
        /// parameters passed and validates the same.
        /// </summary>
        /// <param name="nodeName">Xml Node name to be read.</param>
        /// <param name="isFilePath">Is file path passed as parameter?</param>
        static void ValidateParseGeneralTestCases(string nodeName, bool isFilePath)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Parser BVT : File Exists in the Path '{0}'.", filePath));

            IList<ISequence> seqs = null;
            GffParser parserObj = new GffParser();

            if (isFilePath)
            {
                seqs = parserObj.Parse(filePath);
            }
            else
            {
                using (StreamReader reader = File.OpenText(filePath))
                {
                    seqs = parserObj.Parse(reader);
                }
            }
            int expectedSequenceCount = 1;
            Assert.IsNotNull(seqs);
            Assert.AreEqual(expectedSequenceCount, seqs.Count);
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Parser BVT : Number of Sequences found are '{0}'.",
                seqs.Count.ToString((IFormatProvider)null)));

            Assert.IsTrue(ValidateFeatures(seqs[0], nodeName));
            ApplicationLog.WriteLine(
                "Gff Parser BVT : Successfully validated all the Features for a give Sequence in GFF File.");
            Console.WriteLine(
                "Gff Parser BVT : Successfully validated all the Features for a give Sequence in GFF File.");

            string expectedSequence = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.ExpectedSequenceNode);

            Sequence seq = (Sequence)seqs[0];
            Assert.IsNotNull(seq);
            Assert.AreEqual(expectedSequence, seq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Parser BVT: The Gff sequence '{0}' validation after Parse() is found to be as expected.",
                seq.ToString()));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Gff Parser BVT: The Gff sequence '{0}' validation after Parse() is found to be as expected.",
                seq.ToString()));

            Assert.AreEqual(expectedSequence.Length, seq.EncodedValues.Length);
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Parser BVT: The Gff Length sequence '{0}' is as expected.",
                expectedSequence.Length));

            string expectedAlphabet = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture);
            Assert.IsNotNull(seq.Alphabet);
            Assert.AreEqual(seq.Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                expectedAlphabet);
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Parser BVT: The Sequence Alphabet is '{0}' and is as expected.",
                seq.Alphabet.Name));

            string expectedSequenceId = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SequenceIdNode);
            Assert.AreEqual(expectedSequenceId, seq.DisplayID);
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Parser BVT: The Sequence ID is '{0}' and is as expected.",
                seq.DisplayID));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Gff Parser BVT: The Sequence ID is '{0}' and is as expected.",
                seq.DisplayID));

        }

        /// <summary>
        /// Parses all test cases related to ParseOne() method based on the 
        /// parameters passed and validates the same.
        /// </summary>
        /// <param name="nodeName">Xml Node name to be read.</param>
        /// <param name="isFilePath">Is file path passed as parameter?</param>
        static void ValidateParseOneGeneralTestCases(string nodeName,
            bool isFilePath)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Parser BVT : File Exists in the Path '{0}'.", filePath));

            ISequence originalSeq = null;
            GffParser parserObj = new GffParser();

            if (isFilePath)
            {
                originalSeq = parserObj.ParseOne(filePath);
            }
            else
            {
                using (StreamReader reader = File.OpenText(filePath))
                {
                    originalSeq = parserObj.ParseOne(reader);
                }
            }

            Assert.IsNotNull(originalSeq);
            Assert.IsTrue(ValidateFeatures(originalSeq, nodeName));
            ApplicationLog.WriteLine(
                "Gff Parser BVT : Successfully validated all the Features for a give Sequence in GFF File.");
            Console.WriteLine(
                "Gff Parser BVT : Successfully validated all the Features for a give Sequence in GFF File.");

            string expectedSequence = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.ExpectedSequenceNode);

            Sequence seq = (Sequence)originalSeq;
            Assert.IsNotNull(seq);
            Assert.AreEqual(expectedSequence, seq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Parser BVT: The Gff sequence '{0}' validation after ParseOne() is found to be as expected.",
                seq.ToString()));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Gff Parser BVT: The Gff sequence '{0}' validation after ParseOne() is found to be as expected.",
                seq.ToString()));

            Assert.AreEqual(expectedSequence.Length, seq.EncodedValues.Length);
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Parser BVT: The Gff Length sequence '{0}' is as expected.",
                expectedSequence.Length));

            string expectedAlphabet = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture);

            Assert.IsNotNull(seq.Alphabet);
            Assert.AreEqual(seq.Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                expectedAlphabet);
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Parser BVT: The Sequence Alphabet is '{0}' and is as expected.",
                seq.Alphabet.Name));

            string expectedSequenceId = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SequenceIdNode);

            Assert.AreEqual(expectedSequenceId, seq.DisplayID);
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Parser BVT: The Sequence ID is '{0}' and is as expected.",
                seq.DisplayID));
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Gff Parser BVT: The Sequence ID is '{0}' and is as expected.",
                seq.DisplayID));
        }

        /// <summary>
        /// Validates the Format() method in Gff Formatter based on the parameters.
        /// </summary>
        /// <param name="nodeName">Xml Node name to be read.</param>
        /// <param name="isFilePath">Is file path passed as parameter?</param>
        /// <param name="isSequenceList">Is sequence list passed as parameter?</param>
        static void ValidateFormatGeneralTestCases(string nodeName,
            bool isFilePath, bool isSequenceList)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));
            IList<ISequence> seqs = null;
            GffParser parserObj = new GffParser();
            seqs = parserObj.Parse(filePath);
            Sequence originalSequence = (Sequence)seqs[0];

            // Use the formatter to write the original sequences to a temp file            
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Formatter BVT: Creating the Temp file '{0}'.",
                Constants.GffTempFileName));

            GffFormatter formatter = new GffFormatter();
            formatter.ShouldWriteSequenceData = true;
            if (isFilePath)
            {
                if (isSequenceList)
                    formatter.Format(seqs, Constants.GffTempFileName);
                else
                    formatter.Format(originalSequence,
                        Constants.GffTempFileName);
            }
            else
            {
                if (isSequenceList)
                {
                    using (TextWriter writer =
                        new StreamWriter(Constants.GffTempFileName))
                    {
                        formatter.Format(seqs, writer);
                    }
                }
                else
                {
                    using (TextWriter writer =
                        new StreamWriter(Constants.GffTempFileName))
                    {
                        formatter.Format(originalSequence, writer);
                    }
                }
            }

            // Read the new file, then compare the sequences
            IList<ISequence> seqsNew = null;
            GffParser newParser = new GffParser();
            seqsNew = newParser.Parse(Constants.GffTempFileName);
            Assert.IsNotNull(seqsNew);
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Formatter BVT: New Sequence is '{0}'.",
                seqsNew[0].ToString()));

            bool val = ValidateFeatures(seqsNew[0], nodeName);
            Assert.IsTrue(val);
            ApplicationLog.WriteLine(
                "GFF Formatter BVT : All the features validated successfully.");
            Console.WriteLine(
                "GFF Formatter BVT : All the features validated successfully.");

            // Now compare the sequences.
            int countNew = seqsNew.Count();
            int expectedCount = 1;
            Assert.AreEqual(expectedCount, countNew);
            ApplicationLog.WriteLine("The Number of sequences are matching.");

            Assert.AreEqual(originalSequence.ID, seqsNew[0].ID);
            string orgSeq = originalSequence.ToString();
            string newSeq = seqsNew[0].ToString();
            Assert.AreEqual(orgSeq, newSeq);
            Console.WriteLine(string.Format(null,
                "Gff Formatter BVT: The Gff sequences '{0}' are matching with Format() method and is as expected.",
                seqsNew[0].ToString()));
            ApplicationLog.WriteLine(string.Format(null,
                "Gff Formatter BVT: The Gff sequences '{0}' are matching with Format() method.",
                seqsNew[0].ToString()));

            // Passed all the tests, delete the tmp file. If we failed an Assert,
            // the tmp file will still be there in case we need it for debugging.
            if (File.Exists(Constants.GffTempFileName))
                File.Delete(Constants.GffTempFileName);
            ApplicationLog.WriteLine("Deleted the temp file created.");
        }

        /// <summary>
        /// Validates the Metadata Features of a Gff Sequence for the sequence and node name specified.
        /// </summary>
        /// <param name="seq">Sequence that needs to be validated.</param>
        /// <param name="nodeName">Xml Node name to be read.</param>
        /// <returns>True/false</returns>
        static bool ValidateFeatures(ISequence seq, string nodeName)
        {
            // Gets all the Features from the Sequence for Validation
            List<MetadataListItem<List<string>>> featureList =
                (List<MetadataListItem<List<string>>>)seq.Metadata[Constants.Features];

            // Gets all the xml values for validation
            string[] sequenceNames = Utility._xmlUtil.GetTextValues(nodeName,
                Constants.SequenceNameNodeName);
            string[] sources = Utility._xmlUtil.GetTextValues(nodeName,
                Constants.SourceNodeName);
            string[] featureNames = Utility._xmlUtil.GetTextValues(nodeName,
                Constants.FeatureNameNodeName);
            string[] startValues = Utility._xmlUtil.GetTextValues(nodeName,
                Constants.StartNodeName);
            string[] endValues = Utility._xmlUtil.GetTextValues(nodeName,
                Constants.EndNodeName);
            string[] scoreValues = Utility._xmlUtil.GetTextValues(nodeName,
                Constants.ScoreNodeName);
            string[] strandValues = Utility._xmlUtil.GetTextValues(nodeName,
                Constants.StrandNodeName);
            string[] frameValues = Utility._xmlUtil.GetTextValues(nodeName,
                Constants.FrameNodeName);
            string[] attributeValues = Utility._xmlUtil.GetTextValues(nodeName,
                Constants.AttributesNodeName);
            int i = 0;

            // Loop through each and every feature and validate the same.
            foreach (MetadataListItem<List<string>> feature in featureList)
            {
                Dictionary<string, List<string>> itemList = feature.SubItems;

                // Read specific feature Item and validate
                // Validate Start
                try
                {
                    List<string> st = itemList[Constants.FeatureStart];
                    foreach (string sin in st)
                    {
                        if (0 != string.Compare(startValues[i], sin,
                            true, CultureInfo.CurrentCulture))
                            return false;
                    }
                }
                catch (KeyNotFoundException) { }

                // Validate Score
                try
                {
                    List<string> st = itemList[Constants.FeatureScore];
                    foreach (string sin in st)
                    {
                        if (0 != string.Compare(scoreValues[i],
                            sin, true, CultureInfo.CurrentCulture))
                            return false;
                    }
                }
                catch (KeyNotFoundException) { }


                // Validate Strand
                try
                {
                    List<string> st = itemList[Constants.FeatureStrand];
                    foreach (string sin in st)
                    {
                        if (0 != string.Compare(strandValues[i],
                            sin, true, CultureInfo.CurrentCulture))
                            return false;
                    }
                }
                catch (KeyNotFoundException) { }

                // Validate Source
                try
                {
                    List<string> st = itemList[Constants.FeatureSource];
                    foreach (string sin in st)
                    {
                        if (0 != string.Compare(sources[i],
                            sin, true, CultureInfo.CurrentCulture))
                            return false;
                    }
                }
                catch (KeyNotFoundException) { }

                // Validate End
                try
                {
                    List<string> st = itemList[Constants.FeatureEnd];
                    foreach (string sin in st)
                    {
                        if (0 != string.Compare(endValues[i],
                            sin, true, CultureInfo.CurrentCulture))
                            return false;
                    }
                }
                catch (KeyNotFoundException) { }


                // Validate Frame
                try
                {
                    List<string> st = itemList[Constants.FeatureFrame];
                    foreach (string sin in st)
                    {
                        if (0 != string.Compare(frameValues[i],
                            sin, true, CultureInfo.CurrentCulture))
                            return false;
                    }
                }
                catch (KeyNotFoundException) { }

                if (0 != string.Compare(feature.FreeText, attributeValues[i],
                    true, CultureInfo.CurrentCulture))
                    return false;

                if (0 != string.Compare(feature.Key, featureNames[i],
                    true, CultureInfo.CurrentCulture))
                    return false;

                if (0 != string.Compare(seq.DisplayID, sequenceNames[i],
                    true, CultureInfo.CurrentCulture))
                    return false;

                i++;
            }

            return true;
        }

        #endregion Supported Methods
    }
}
