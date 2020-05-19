// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * BAMP2TestCases.cs
 * 
 *   This file contains the BAM - Parsers and Formatters P2 test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;

using Bio.TestAutomation.Util;
using Bio.Util.Logging;

using Bio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bio.IO.SAM;
using Bio.IO.BAM;
using Bio.Algorithms.Alignment;
using Bio.IO;
using System.IO;

namespace Bio.TestAutomation.IO.BAM
{
    /// <summary>
    /// BAM parser and formatter P2 Test case implementation.
    /// </summary>
    [TestClass]
    public class BAMP2TestCases
    {
        #region Enums

        /// <summary>
        /// BAM Parser ctor parameters used for different test cases.
        /// </summary>
        enum BAMParserParameters
        {
            StreamReader,
            FileName,
            ParseRangeFileName,
            ParseRangeWithIndex,
            TextReader,
            ParseOneTextReader,
        }

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\SAMBAMTestData\SAMBAMTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static BAMP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("bio.automation.log");
            }
        }

        #endregion Constructor

        #region BAM Parser P2 Testcases

        /// <summary>
        /// Invalidate BAM Parser Parse(TextReader)
        /// Input : BAM file.
        /// Output : NotSupportedException.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateSequenceAlignmentParseTextReader()
        {
            InValidateISequenceAlignmentBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.TextReader);
        }

        /// <summary>
        /// Invalidate BAM Parser Parse(stream)
        /// Input : Invlaid Stream.
        /// Output : Argument NullException.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateBAMParserForInvalidStreamInput()
        {
            InValidateSeqAlignmentMapBAMParser(Constants.InvalidBAMFileNode,
                BAMParserParameters.StreamReader);
        }

        /// <summary>
        /// Invalidate BAM Parser Parse(filename)
        /// Input : Invlaid filename.
        /// Output : Argument NullException.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateBAMParserForInvalidBAMFile()
        {
            InValidateSeqAlignmentMapBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.FileName);
        }

        /// <summary>
        /// Invalidate GetIndexFromBAMFile(fileName) by passing 
        /// null as BAM file.
        /// Input : Null BAM file.
        /// Output : Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGetIndexFromBAMFile()
        {
            // Create BAM Parser object
            using (BAMParser bamParserObj = new BAMParser())
            {

                try
                {
                    bamParserObj.GetIndexFromBAMFile(null as string);
                    Assert.Fail();
                }
                catch (ArgumentNullException ex)
                {
                    string exceptionMessage = ex.Message;
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P2 : Validated Exception {0} successfully",
                        exceptionMessage));
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P2 : Validated Exception {0} successfully",
                        exceptionMessage));
                }
            }
        }

        /// <summary>
        /// Invalidate GetIndexFromBAMFile(stream) by passing 
        /// null as BAM file.
        /// Input : Null BAM file.
        /// Output : Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateGetIndexFromBAMFileUsingStream()
        {
            // Create BAM Parser object
            using (BAMParser bamParserObj = new BAMParser())
            {

                try
                {
                    bamParserObj.GetIndexFromBAMFile(null as Stream);
                    Assert.Fail();
                }
                catch (ArgumentNullException ex)
                {
                    string exceptionMessage = ex.Message;
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P2 : Validated Exception {0} successfully",
                        exceptionMessage));
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P2 : Validated Exception {0} successfully",
                        exceptionMessage));
                }
            }
        }

        /// <summary>
        /// Invalidate BAM Parser ParseOne(TextReader)
        /// Input : BAM file.
        /// Output : NotSupportedException.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateSequenceAlignmentParseOneTextReader()
        {
            InValidateISequenceAlignmentBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.ParseOneTextReader);
        }

        /// <summary>
        /// Invalidate BAM Parser ParseRange(filename, RefIndex)
        /// Input : Invalid BAM file and RefIndex values.
        /// Output : NotSupportedException.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateParseRangeForInvalidInputs()
        {
            InValidateSeqAlignmentMapBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.ParseRangeWithIndex);
        }

        /// <summary>
        /// Invalidate BAM Parser ParseRange(filename,range)
        /// Input : Invalid BAM file and SequenceRange values.
        /// Output : NotSupportedException.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateParseRangeForInvalidSequenceRange()
        {
            InValidateSeqAlignmentMapBAMParser(Constants.SmallSizeBAMFileNode,
                BAMParserParameters.ParseRangeFileName);
        }

        /// <summary>
        /// Invalidate Set Alphabet.
        /// Input : Null BAM file.
        /// Output : Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateSetAlphabet()
        {
            // Create BAM Parser object
            using (BAMParser bamParserObj = new BAMParser())
            {
                // TO cover code coverage.
                try
                {
                    bamParserObj.Alphabet = Alphabets.DNA;
                    Assert.Fail();
                }
                catch (NotSupportedException ex)
                {
                    string exceptionMessage = ex.Message;
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P2 : Validated Exception {0} successfully",
                        exceptionMessage));
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "BAM Parser P2 : Validated Exception {0} successfully",
                        exceptionMessage));
                }
            }
        }

        #endregion BAM Parser P2 Testcases

        # region BAM Formatter P2 Testcases

        /// <summary>
        ///  InValidate BAM Formatter Format() methods with invalid inputs.
        ///  Input : Invalid inputs
        ///  Output : Exception validation.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateBAMFormatMethods()
        {
            InValidateBAMFormatter(Constants.SmallSizeBAMFileNode);
        }

        /// <summary>
        ///  InValidate BAM Formatter Format() methods with invalid inputs
        ///  For ISequenceAlignment.
        ///  Input : Invalid inputs
        ///  Output : Exception validation.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateBAMFormatMethodsWithISequenceAlignment()
        {
            InValidateBAMFormatterWithSequenceAlignment(
                Constants.SmallSizeBAMFileNode);
        }

        # endregion BAM Formatter P2 Testcases

        # region Supporting Methods

        /// <summary>
        /// Parse BAM File and Invalidate parsed aligned sequences by creating 
        /// ISequenceAlignment interface object and its properties.
        /// </summary>
        /// <param name="nodeName">Different xml nodes used for different test cases</param>
        /// <param name="BAMParserPam">BAM Parse method parameters</param>
        void InValidateISequenceAlignmentBAMParser(string nodeName,
            BAMParserParameters BAMParserPam)
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string exception = string.Empty;

            ISequenceAlignmentParser bamParser = null;
            bamParser = new BAMParser();

            try
            {
                // Parse a BAM file with different invalid parameters.
                switch (BAMParserPam)
                {
                    case BAMParserParameters.TextReader:
                        try
                        {
                            using (TextReader reader = new StreamReader(bamFilePath))
                            {
                                bamParser.Parse(reader);
                                Assert.Fail();
                            }
                        }
                        catch (NotSupportedException ex)
                        {
                            exception = ex.Message;
                        }
                        break;
                    case BAMParserParameters.ParseOneTextReader:
                        try
                        {
                            using (TextReader reader = new StreamReader(bamFilePath))
                            {
                                bamParser.ParseOne(reader);
                                Assert.Fail();
                            }
                        }
                        catch (NotSupportedException ex)
                        {
                            exception = ex.Message;
                        }
                        break;
                    default:
                        break;
                }

                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }
            finally
            {
                (bamParser as BAMParser).Dispose();
            }
        }

        /// <summary>
        /// Parse BAM and validate parsed aligned sequences by creating 
        /// ISequenceAlignment interface object and its properties.
        /// </summary>
        /// <param name="nodeName">Different xml nodes used for different test cases</param>
        /// <param name="BAMParserPam">BAM Parse method parameters</param>
        void InValidateSeqAlignmentMapBAMParser(string nodeName,
            BAMParserParameters BAMParserPam)
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string exception = string.Empty;

            using (BAMParser bamParser = new BAMParser())
            {
                // Parse a BAM file with different parameters.
                switch (BAMParserPam)
                {
                    case BAMParserParameters.StreamReader:
                        try
                        {
                            bamParser.Parse(null as Stream);
                            Assert.Fail();
                        }
                        catch (ArgumentNullException ex)
                        {
                            exception = ex.Message;
                        }

                        try
                        {
                            using (Stream stream = new FileStream(bamFilePath, FileMode.Open,
                             FileAccess.Read))
                            {
                                bamParser.Parse(stream);
                                Assert.Fail();
                            }
                        }
                        catch (FileFormatException ex)
                        {
                            exception = ex.Message;
                        }
                        break;
                    case BAMParserParameters.FileName:
                        try
                        {
                            bamParser.Parse(null as string);
                            Assert.Fail();
                        }
                        catch (ArgumentNullException ex)
                        {
                            exception = ex.Message;
                        }
                        break;
                    case BAMParserParameters.ParseRangeWithIndex:
                        try
                        {
                            bamParser.ParseRange(null, 0);
                            Assert.Fail();
                        }
                        catch (ArgumentNullException ex)
                        {
                            exception = ex.Message;
                        }

                        try
                        {
                            bamParser.ParseRange(bamFilePath, -2);
                            Assert.Fail();
                        }
                        catch (ArgumentOutOfRangeException ex)
                        {
                            exception = ex.Message;
                        }
                        break;
                    case BAMParserParameters.ParseRangeFileName:
                        try
                        {
                            bamParser.ParseRange(null, new SequenceRange("chr20", 0, 10));
                            Assert.Fail();
                        }
                        catch (ArgumentNullException ex)
                        {
                            exception = ex.Message;
                        }

                        try
                        {
                            bamParser.ParseRange(bamFilePath, null as SequenceRange);
                            Assert.Fail();
                        }
                        catch (ArgumentNullException ex)
                        {
                            exception = ex.Message;
                        }
                        break;
                    default:
                        break;
                }

                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }
        }

        /// <summary>
        /// Format BAM file and validate.
        /// </summary>
        /// <param name="nodeName">Different xml nodes used for different test cases</param>
        void InValidateBAMFormatter(string nodeName)
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Stream stream = null;
            SequenceAlignmentMap seqAlignment = null;
            using (BAMParser bamParserObj = new BAMParser())
            {
                string exception = string.Empty;

                using (BAMIndexFile bamIndexFileObj = new BAMIndexFile(
                                Constants.BAMTempIndexFileForIndexData,
                                FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    // Parse a BAM file.
                    seqAlignment = bamParserObj.Parse(bamFilePath);

                    // Create a BAM formatter object.
                    BAMFormatter formatterObj = new BAMFormatter();

                    // Invalidate Format(SequenceAlignmentMap, BAMFile, IndexFile)
                    try
                    {

                        formatterObj.Format(seqAlignment, null,
                            Constants.BAMTempIndexFileForIndexData);

                    }
                    catch (ArgumentNullException ex)
                    {
                        exception = ex.Message;
                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                    }

                    try
                    {

                        formatterObj.Format(seqAlignment, Constants.BAMTempFileName,
                            Constants.BAMTempFileName);

                    }
                    catch (ArgumentException ex)
                    {
                        exception = ex.Message;
                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                    }

                    try
                    {
                        formatterObj.Format(null as SequenceAlignmentMap, bamFilePath,
                            Constants.BAMTempIndexFileForIndexData);

                    }
                    catch (ArgumentNullException ex)
                    {
                        exception = ex.Message;
                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                    }

                    try
                    {
                        formatterObj.Format(seqAlignment, bamFilePath, null as string);

                    }
                    catch (ArgumentNullException ex)
                    {
                        exception = ex.Message;
                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                    }

                    // Invalidate BAM Parser Format(SeqAlignmentMap, BamFileName)
                    try
                    {
                        formatterObj.Format(null as SequenceAlignmentMap,
                            Constants.BAMTempFileName);

                    }
                    catch (ArgumentNullException ex)
                    {
                        exception = ex.Message;
                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                    }

                    try
                    {
                        formatterObj.Format(seqAlignment, null as string);

                    }
                    catch (ArgumentNullException ex)
                    {
                        exception = ex.Message;
                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                    }

                    // Invalidate Format(SequenceAlignmentMap, StreamWriter)
                    try
                    {
                        formatterObj.Format(seqAlignment, null as Stream);

                    }
                    catch (ArgumentNullException ex)
                    {
                        exception = ex.Message;
                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                    }

                    try
                    {
                        using (stream = new
                                 FileStream(Constants.BAMTempFileName,
                                 FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            formatterObj.Format(null as SequenceAlignmentMap, stream);
                        }

                    }
                    catch (ArgumentNullException ex)
                    {
                        exception = ex.Message;
                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                    }

                    // Invalidate Format(SequenceAlignmentMap, StreamWriter, IndexFile)
                    try
                    {

                        formatterObj.Format(seqAlignment, null, bamIndexFileObj);

                    }
                    catch (ArgumentNullException ex)
                    {
                        exception = ex.Message;
                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                    }

                    try
                    {
                        using (stream = new
                                 FileStream(Constants.BAMTempFileName,
                                 FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            formatterObj.Format(null as SequenceAlignmentMap, stream,
                                bamIndexFileObj);
                        }

                    }
                    catch (ArgumentNullException ex)
                    {
                        exception = ex.Message;
                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                    }

                    try
                    {
                        using (stream = new
                                  FileStream(Constants.BAMTempFileName,
                                  FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            formatterObj.Format(seqAlignment, stream, null);


                        }

                    }

                    catch (ArgumentNullException ex)
                    {
                        exception = ex.Message;
                        // Log to NUNIT GUI.
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "BAM Parser P2 : Validated Exception {0} successfully",
                            exception));
                    }

                    formatterObj = null;
                }
            }
        }

        /// <summary>
        /// Format BAM file using IsequenceAlignment object.
        /// </summary>
        /// <param name="nodeName">Different xml nodes used for different test cases</param>
        void InValidateBAMFormatterWithSequenceAlignment(string nodeName)
        {
            // Get input and output values from xml node.
            string bamFilePath = _utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            ISequenceAlignmentParser bamParserObj = new BAMParser();
            IList<ISequenceAlignment> seqList = bamParserObj.Parse(bamFilePath);
            try
            {
                using (BAMIndexFile bamIndexFileObj = new BAMIndexFile(
                    Constants.BAMTempIndexFileForInvalidData,
                    FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    // Create a BAM formatter object.
                    BAMFormatter formatterObj = new BAMFormatter();

                    InvalidateBAmFormatter(formatterObj, seqList);

                    InvalidateBAmFormatterWithWithInvalidValues(formatterObj,
                        seqList, bamFilePath, bamIndexFileObj);

                    InvalidateBAmFormatterWithWithNullValues(formatterObj,
                        seqList);
                }
            }
            finally
            {
                (bamParserObj as BAMParser).Dispose();
            }
        }


        /// <summary>
        /// Invalidate BAMFormatter.
        /// </summary>
        /// <param name="formatterObj">Bam formatter obj</param>
        /// <param name="seqList">List of sequences</param>
        static void InvalidateBAmFormatter(BAMFormatter formatterObj,
           IList<ISequenceAlignment> seqList)
        {
            // Invalidate BAM Parser Format(SeqAlignment, BamFileName)
            string exception = null;
            try
            {
                formatterObj.Format(null as ISequenceAlignment,
                    Constants.BAMTempFileName);

            }
            catch (ArgumentNullException ex)
            {
                exception = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }

            try
            {
                foreach (ISequenceAlignment seq in seqList)
                {
                    formatterObj.Format(seq, null as string);
                }
            }
            catch (ArgumentNullException ex)
            {
                exception = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }

            // Invalidate Format(IseqAlignment, BAMFile, IndexFile)
            try
            {
                foreach (ISequenceAlignment seq in seqList)
                {
                    formatterObj.Format(seq, null,
                        Constants.BAMTempIndexFileForIndexData);
                }
            }
            catch (ArgumentNullException ex)
            {
                exception = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }

            try
            {
                foreach (ISequenceAlignment seq in seqList)
                {
                    formatterObj.Format(seq, Constants.BAMTempFileName,
                        Constants.BAMTempFileName);
                }
            }
            catch (ArgumentException ex)
            {
                exception = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }
        }

        /// <summary>
        /// Invalidate BAMFormatter with invlid values.
        /// </summary>
        /// <param name="formatterObj">Bam formatter obj</param>
        /// <param name="seqList">List of sequences</param>
        static void InvalidateBAmFormatterWithWithInvalidValues(BAMFormatter formatterObj,
           IList<ISequenceAlignment> seqList, string bamFilePath, BAMIndexFile bamIndexFileObj)
        {
            string exception = string.Empty;
            try
            {
                formatterObj.Format(null as ISequenceAlignment, bamFilePath,
                    Constants.BAMTempIndexFileForIndexData);

            }
            catch (ArgumentNullException ex)
            {
                exception = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }

            try
            {
                foreach (ISequenceAlignment seq in seqList)
                {
                    formatterObj.Format(seq, bamFilePath, null as string);
                }

            }
            catch (ArgumentNullException ex)
            {
                exception = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }

            // Invalidate Format(IseqAlignment, StreamWriter, IndexFile)
            try
            {
                foreach (ISequenceAlignment seq in seqList)
                {
                    formatterObj.Format(seq, null, bamIndexFileObj);
                }
            }
            catch (ArgumentNullException ex)
            {
                exception = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }

            try
            {
                using (Stream stream = new
                    FileStream(Constants.BAMTempFileName,
                    FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    formatterObj.Format(null as ISequenceAlignment, stream,
                        bamIndexFileObj);
                }

            }
            catch (ArgumentNullException ex)
            {
                exception = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }
        }

        /// <summary>
        /// Invalidate BAMFormatter with null values.
        /// </summary>
        /// <param name="formatterObj">Bam formatter obj</param>
        /// <param name="seqList">List of sequences</param>
        static void InvalidateBAmFormatterWithWithNullValues(BAMFormatter formatterObj,
            IList<ISequenceAlignment> seqList)
        {
            string exception = string.Empty;
            try
            {
                using (Stream stream = new
                    FileStream(Constants.BAMTempFileName,
                    FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    foreach (ISequenceAlignment seq in seqList)
                    {
                        formatterObj.Format(seq, stream,
                            null);
                    }
                }

            }

            catch (ArgumentNullException ex)
            {
                exception = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }

            // Invalidate Format(IseqAlignment, StreamWriter)
            try
            {
                foreach (ISequenceAlignment seq in seqList)
                {
                    formatterObj.Format(seq, null as Stream);
                }
            }
            catch (ArgumentNullException ex)
            {
                exception = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }

            try
            {
                using (Stream stream = new
                    FileStream(Constants.BAMTempFileName,
                    FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    formatterObj.Format(null as ISequenceAlignment, stream);
                }

            }
            catch (ArgumentNullException ex)
            {
                exception = ex.Message;
                // Log to NUNIT GUI.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BAM Parser P2 : Validated Exception {0} successfully",
                    exception));
            }
        }
        # endregion Supporting Methods
    }
}
