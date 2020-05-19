// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * BedP1TestCases.cs
 * 
 *  This file contains the Bed - Parsers and Formatters P1 test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

using MBF.IO.Bed;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.IO.Bed
{
    /// <summary>
    /// Bed P1 parser and formatter Test case implementation.
    /// </summary>
    [TestFixture]
    public class BedP1TestCases
    {

        #region Enums

        /// <summary>
        /// Additional parameters to validate different scenarios.
        /// </summary>
        enum AdditionalParameters
        {
            RangeFileName,
            RangeTextReader,
            RangeGroupFileName,
            RangeTextWriter,
            RangeGroupTextWriter,
            RangeGroupTextReader,
            ParseRange,
            ParseRangeGroup,
            ParseRangeTextWriter,
            ParseRangeGroupTextWriter
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static BedP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\BedTestsConfig.xml");
        }

        #endregion Constructor

        #region Bed Parser P1 Test cases

        /// <summary>
        /// Parse a valid Bed file (one line) and 
        /// convert the same Range using ParseRange(text-reader) method and 
        /// validate the same
        /// Input : Bed File
        /// Validation: Range properties like ID, Start and End.
        /// </summary>
        [Test]
        public void BedParserValidateOneLineParseRangeTextReader()
        {
            ParserGeneralTestCases(Constants.OneLineBedNodeName,
                AdditionalParameters.RangeTextReader);
        }

        /// <summary>
        /// Parse a valid Bed file (one line) and 
        /// convert the same Range using ParseRangeGrouping(file-name) method and 
        /// validate the same
        /// Input : Bed File
        /// Validation: Range properties like ID, Start and End.
        /// </summary>
        [Test]
        public void BedParserValidateOneLineParseRangeGroupFileName()
        {
            ParserGeneralTestCases(Constants.OneLineBedNodeName,
                AdditionalParameters.RangeGroupFileName);
        }

        /// <summary>
        /// Parse a valid Bed file (three chromosomes) and 
        /// convert the same Range using ParseRange(file-name) method and 
        /// validate the same
        /// Input : Bed File
        /// Validation: Range properties like ID, Start and End.
        /// </summary>
        [Test]
        public void BedParserValidateThreeChromosomeParseRangeFileName()
        {
            ParserGeneralTestCases(Constants.ThreeChromoBedNodeName,
                AdditionalParameters.RangeFileName);
        }

        #endregion Bed Parser P1 Test cases

        #region Bed Formatter P1 Test cases

        /// <summary>
        /// Format a valid Range (small size file) to a 
        /// Bed file using Format(Range, file-path) method and 
        /// validate the same.
        /// Input : Bed Range
        /// Validation : Read the Bed file to which the range was formatted 
        /// using File-Info and Validate Properties like ID, Start and End
        /// </summary>
        [Test]
        public void BedFormatterValidateParseSmallSizeFormatRangeFileName()
        {
            FormatterGeneralTestCases(Constants.SmallSizeBedNodeName,
                AdditionalParameters.ParseRange);
        }

        /// <summary>
        /// Format a valid Range (small size file) to a 
        /// Bed file using Format(Range, text-writer) method and 
        /// validate the same.
        /// Input : Bed Range
        /// Validation : Read the Bed file to which the range was formatted 
        /// using File-Info and Validate Properties like ID, Start and End
        /// </summary>
        [Test]
        public void BedFormatterValidateParseSmallSizeFormatRangeTextWriter()
        {
            FormatterGeneralTestCases(Constants.SmallSizeBedNodeName,
                AdditionalParameters.ParseRangeTextWriter);
        }

        /// <summary>
        /// Format a valid Range (one line file) to a 
        /// Bed file using Format(Range, file-name) method and 
        /// validate the same.
        /// Input : Bed Range
        /// Validation : Read the Bed file to which the range was formatted 
        /// using File-Info and Validate Properties like ID, Start and End
        /// </summary>
        [Test]
        public void BedFormatterValidateParseOneLineFormatRangeFileName()
        {
            FormatterGeneralTestCases(Constants.OneLineBedNodeName,
                AdditionalParameters.ParseRange);
        }

        /// <summary>
        /// Format a valid RangeGroup (small size file) to a 
        /// Bed file using Format(RangeGroup, file-path) method and 
        /// validate the same.
        /// Input : Bed Range
        /// Validation : Read the Bed file to which the range was formatted 
        /// using File-Info and Validate Properties like ID, Start and End
        /// </summary>
        [Test]
        public void BedFormatterValidateParseSmallSizeFormatRangeGroupFileName()
        {
            FormatterGeneralTestCases(Constants.SmallSizeBedNodeName,
                AdditionalParameters.ParseRangeGroup);
        }

        /// <summary>
        /// Format a valid RangeGroup (small size file) to a 
        /// Bed file using Format(RangeGroup, text-writer) method and 
        /// validate the same.
        /// Input : Bed Range
        /// Validation : Read the Bed file to which the range was formatted 
        /// using File-Info and Validate Properties like ID, Start and End
        /// </summary>
        [Test]
        public void BedFormatterValidateParseSmallSizeFormatRangeGroupTextWriter()
        {
            FormatterGeneralTestCases(Constants.SmallSizeBedNodeName,
                AdditionalParameters.ParseRangeGroupTextWriter);
        }

        /// <summary>
        /// Format a valid RangeGroup (one line file) to a 
        /// Bed file using Format(RangeGroup, file-path) method and 
        /// validate the same.
        /// Input : Bed Range
        /// Validation : Read the Bed file to which the range was formatted 
        /// using File-Info and Validate Properties like ID, Start and End
        /// </summary>
        [Test]
        public void BedFormatterValidateParseOneLineFormatRangeGroupFileName()
        {
            FormatterGeneralTestCases(Constants.OneLineBedNodeName,
                AdditionalParameters.ParseRangeGroup);
        }

        /// <summary>
        /// Format a valid Range (one line file) to a 
        /// Bed file using Format(Range, Text-Writer) method and 
        /// validate the same.
        /// Input : Bed Range
        /// Validation : Read the Bed file to which the range was formatted 
        /// using File-Info and Validate Properties like ID, Start and End
        /// </summary>
        [Test]
        public void BedFormatterValidateParseOneLineFormatRangeTextWriter()
        {
            FormatterGeneralTestCases(Constants.OneLineBedNodeName,
                AdditionalParameters.RangeTextWriter);
        }

        /// <summary>
        /// Format a valid RangeGroup (one line file) to a 
        /// Bed file using Format(RangeGroup, Text-Writer) method and 
        /// validate the same.
        /// Input : Bed Range
        /// Validation : Read the Bed file to which the range was formatted 
        /// using File-Info and Validate Properties like ID, Start and End
        /// </summary>
        [Test]
        public void BedFormatterValidateParseOneLineFormatRangeGroupTextWriter()
        {
            FormatterGeneralTestCases(Constants.OneLineBedNodeName,
                AdditionalParameters.RangeGroupTextWriter);
        }

        /// <summary>
        /// Format a valid Range (three chromosome file) to a 
        /// Bed file using Format(Range, file-path) method and 
        /// validate the same.
        /// Input : Bed Range
        /// Validation : Read the Bed file to which the range was formatted 
        /// using File-Info and Validate Properties like ID, Start and End
        /// </summary>
        [Test]
        public void BedFormatterValidateParseThreeChromosomeFormatRangeFileName()
        {
            FormatterGeneralTestCases(Constants.ThreeChromoBedNodeName,
                AdditionalParameters.ParseRange);
        }

        /// <summary>
        /// Format a valid RangeGroup (three chromosome file) to a 
        /// Bed file using Format(RangeGroup, file-path) method and 
        /// validate the same.
        /// Input : Bed Range
        /// Validation : Read the Bed file to which the range was formatted 
        /// using File-Info and Validate Properties like ID, Start and End
        /// </summary>
        [Test]
        public void BedFormatterValidateParseThreeChromosomeFormatRangeGroupFileName()
        {
            FormatterGeneralTestCases(Constants.ThreeChromoBedNodeName,
                AdditionalParameters.ParseRangeGroup);
        }

        /// <summary>
        /// Format a valid Range (Long Start End file) to a 
        /// Bed file using Format(Range, file-path) method and 
        /// validate the same.
        /// Input : Bed Range
        /// Validation : Read the Bed file to which the range was formatted 
        /// using File-Info and Validate Properties like ID, Start and End
        /// </summary>
        [Test]
        public void BedFormatterValidateParseLongStartEndFormatRangeFileName()
        {
            FormatterGeneralTestCases(Constants.LongStartEndBedNodeName,
                AdditionalParameters.ParseRange);
        }

        /// <summary>
        /// Format a valid RangeGroup (Long Start End file) to a 
        /// Bed file using Format(RangeGroup, file-path) method and 
        /// validate the same.
        /// Input : Bed Range
        /// Validation : Read the Bed file to which the range was formatted 
        /// using File-Info and Validate Properties like ID, Start and End
        /// </summary>
        [Test]
        public void BedFormatterValidateParseLongStartEndFormatRangeGroupFileName()
        {
            FormatterGeneralTestCases(Constants.LongStartEndBedNodeName,
                AdditionalParameters.ParseRangeGroup);
        }

        #endregion Bed Formatter P1 Test cases

        #region Supported Methods

        /// <summary>
        /// Parsers the Bed file for different test cases based
        /// on Additional parameter
        /// </summary>
        /// <param name="nodeName">Xml Node name</param>
        /// <param name="addParam">Additional parameter</param>
        static void ParserGeneralTestCases(string nodeName,
            AdditionalParameters addParam)
        {
            // Gets the Filename
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);

            Assert.IsNotEmpty(filePath);
            ApplicationLog.WriteLine(string.Format(
                "Bed Parser P1: Reading the File from location '{0}'", filePath));

            // Get the rangelist after parsing.
            BedParser parserObj = new BedParser();

            IList<ISequenceRange> rangeList = null;
            SequenceRangeGrouping rangeGroup = null;

            // Gets the Range list/Range Group based on the parameters.
            switch (addParam)
            {
                case AdditionalParameters.RangeFileName:
                    rangeList = parserObj.ParseRange(filePath);
                    break;
                case AdditionalParameters.RangeTextReader:
                    rangeList = parserObj.ParseRange(
                        new StreamReader(filePath));
                    break;
                case AdditionalParameters.RangeGroupFileName:
                    rangeGroup = parserObj.ParseRangeGrouping(filePath);
                    break;
                case AdditionalParameters.RangeGroupTextReader:
                    rangeGroup = parserObj.ParseRangeGrouping(
                        new StreamReader(filePath));
                    break;
                default:
                    break;
            }

            // Gets the Range list from Group
            switch (addParam)
            {
                case AdditionalParameters.RangeGroupTextReader:
                case AdditionalParameters.RangeGroupFileName:
                    IEnumerable<string> grpIDsObj = rangeGroup.GroupIDs;
                    string rangeID = string.Empty;
                    foreach (string grpID in grpIDsObj)
                    {
                        rangeID = grpID;
                    }
                    rangeList = rangeGroup.GetGroup(rangeID);
                    break;
                default:
                    break;
            }

            // Gets all the expected values from xml.
            string expectedIDs = Utility._xmlUtil.GetTextValue(
                   nodeName, Constants.IDNode);
            string expectedStarts = Utility._xmlUtil.GetTextValue(
                   nodeName, Constants.StartNode);
            string expectedEnds = Utility._xmlUtil.GetTextValue(
                   nodeName, Constants.EndNode);

            string actualStarts = string.Empty;
            string actualEnds = string.Empty;
            string actualIDs = string.Empty;

            // Reads all the ranges with comma seperated for validation
            foreach (ISequenceRange range in rangeList)
            {
                actualStarts = string.Concat(actualStarts, range.Start.ToString(), ",");
                actualEnds = string.Concat(actualEnds, range.End.ToString(), ",");
                actualIDs = string.Concat(actualIDs, range.ID.ToString(), ",");
            }

            Assert.AreEqual(expectedIDs, actualIDs.Substring(0, actualIDs.Length - 1));
            Assert.AreEqual(expectedStarts, actualStarts.Substring(0, actualStarts.Length - 1));
            Assert.AreEqual(expectedEnds, actualEnds.Substring(0, actualEnds.Length - 1));
            ApplicationLog.WriteLine(
                "Bed Parser P1: Successfully validated the ID, Start and End Ranges");
            Console.WriteLine(
                "Bed Parser P1: Successfully validated the ID, Start and End Ranges");
        }

        /// <summary>
        /// Formats the Range/RangeGroup for different test cases based
        /// on Additional parameter
        /// </summary>
        /// <param name="nodeName">Xml Node name</param>
        /// <param name="addParam">Additional parameter</param>
        static void FormatterGeneralTestCases(string nodeName,
            AdditionalParameters addParam)
        {
            IList<ISequenceRange> rangeList = new List<ISequenceRange>();
            SequenceRangeGrouping rangeGroup = new SequenceRangeGrouping();

            // Gets the file name.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);

            // Condition to check if Parse() happens before Format()
            switch (addParam)
            {
                case AdditionalParameters.ParseRangeGroup:
                case AdditionalParameters.ParseRangeGroupTextWriter:
                    BedParser initialParserGroupObj = new BedParser();
                    rangeGroup =
                        initialParserGroupObj.ParseRangeGrouping(filePath);
                    break;
                case AdditionalParameters.ParseRange:
                case AdditionalParameters.ParseRangeTextWriter:
                    BedParser initialParserObj = new BedParser();
                    rangeList = initialParserObj.ParseRange(filePath);
                    break;
                default:
                    // Gets all the expected values from xml.
                    string expectedID = Utility._xmlUtil.GetTextValue(
                        nodeName, Constants.IDNode);
                    string expectedStart = Utility._xmlUtil.GetTextValue(
                        nodeName, Constants.StartNode);
                    string expectedEnd = Utility._xmlUtil.GetTextValue(
                        nodeName, Constants.EndNode);

                    string[] expectedIDs = expectedID.Split(',');
                    string[] expectedStarts = expectedStart.Split(',');
                    string[] expectedEnds = expectedEnd.Split(',');

                    // Gets the Range Group or Range based on the additional parameter
                    switch (addParam)
                    {
                        case AdditionalParameters.RangeGroupTextWriter:
                        case AdditionalParameters.RangeGroupFileName:
                            for (int i = 0; i < expectedIDs.Length; i++)
                            {
                                SequenceRange rangeObj1 =
                                    new SequenceRange(expectedIDs[i],
                                        long.Parse(expectedStarts[i]),
                                        long.Parse(expectedEnds[i]));
                                rangeGroup.Add(rangeObj1);
                            }
                            break;
                        default:
                            for (int i = 0; i < expectedIDs.Length; i++)
                            {
                                SequenceRange rangeObj2 =
                                    new SequenceRange(expectedIDs[i],
                                        long.Parse(expectedStarts[i]),
                                        long.Parse(expectedEnds[i]));
                                rangeList.Add(rangeObj2);
                            }
                            break;
                    }
                    break;
            }

            BedFormatter formatterObj = new BedFormatter();

            // Gets the Range list/Range Group based on the parameters.
            switch (addParam)
            {
                case AdditionalParameters.RangeFileName:
                case AdditionalParameters.ParseRange:
                    formatterObj.Format(rangeList, Constants.BedTempFileName);
                    break;
                case AdditionalParameters.RangeTextWriter:
                case AdditionalParameters.ParseRangeTextWriter:
                    using (TextWriter txtWriter =
                        new StreamWriter(Constants.BedTempFileName))
                    {
                        formatterObj.Format(rangeList, txtWriter);
                    }
                    break;
                case AdditionalParameters.RangeGroupFileName:
                case AdditionalParameters.ParseRangeGroup:
                    formatterObj.Format(rangeGroup, Constants.BedTempFileName);
                    break;
                case AdditionalParameters.RangeGroupTextWriter:
                case AdditionalParameters.ParseRangeGroupTextWriter:
                    using (TextWriter txtWriter =
                        new StreamWriter(Constants.BedTempFileName))
                    {
                        formatterObj.Format(rangeGroup, txtWriter);
                    }
                    break;
                default:
                    break;
            }

            // Reparse to validate the results
            BedParser parserObj = new BedParser();
            IList<ISequenceRange> newRangeList =
              parserObj.ParseRange(Constants.BedTempFileName);

            // Validation of all the properties.
            for (int i = 0; i < rangeList.Count; i++)
            {
                Assert.AreEqual(rangeList[0].ID, newRangeList[0].ID);
                Assert.AreEqual(rangeList[0].Start, newRangeList[0].Start);
                Assert.AreEqual(rangeList[0].End, newRangeList[0].End);
            }

            ApplicationLog.WriteLine(
                "Bed Formatter P1: Successfully validated the ID, Start and End Ranges");
            Console.WriteLine(
                "Bed Formatter P1: Successfully validated the ID, Start and End Ranges");

            // Cleanup the file.
            if (File.Exists(Constants.BedTempFileName))
                File.Delete(Constants.BedTempFileName);
        }

        #endregion Supported Methods
    }
}
