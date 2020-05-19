// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * XsvSparseBvtTestCases.cs
 * 
 *   This file contains the XsvSparse - Parsers & Formatter Bvt test cases.
 * 
***************************************************************************/

using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

using MBF.IO;
using MBF.Encoding;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.Algorithms.Assembly;

namespace MBF.TestAutomation.IO
{
    /// <summary>
    /// XsvSparse Bvt parser Test case implementation.
    /// </summary>
    [TestClass]
    public class XsvSparseBvtTestCases
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
            ParseOneTextReaderReadOnly,
            ParseOneFilePathReadOnly,
            ParseTextReaderReadOnly,
            ParseFilePathReadOnly,
            FormatFilePath,
            ForamtListWithFilePath,
            FormatTextWriter,
            FormatListTextWriter,
            FormatTextWriterWithOffset,
        };

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static XsvSparseBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Parser Test cases

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : XsvSparse File
        /// Validation : Expected sequence, Sequence Alphabet, Sequence ID.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtParserValidateParseFilePath()
        {
            XsvSparseParserGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.ParseFilePath);
        }

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(text-reader) method and validate with the 
        /// expected sequence.
        /// Input : XsvSparse File
        /// Validation : Expected sequence, Sequence Alphabet, Sequence ID.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtParserValidateParseTextReader()
        {
            XsvSparseParserGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.ParseTextReader);
        }

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using ParseOne(file-name) method and validate with the 
        /// expected sequence.
        /// Input : XsvSparse File
        /// Validation : Expected sequence, Sequence Alphabet, Sequence ID.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtParserValidateParseOneFilePath()
        {
            XsvSparseParserGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.ParseOneFilePath);
        }

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using ParseOne(text-reader) method and validate with the 
        /// expected sequence.
        /// Input : XsvSparse File
        /// Validation : Expected sequence, Sequence Alphabet, Sequence ID.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtParserValidateParseOneTextReader()
        {
            XsvSparseParserGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.ParseOneTextReader);
        }

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name, Read-only) method and validate with the 
        /// expected sequence.
        /// Input : XsvSparse File
        /// Validation : Expected sequence, Sequence Alphabet, Sequence ID.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtParserValidateParseFilePathReadOnly()
        {
            XsvSparseParserGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.ParseFilePathReadOnly);
        }

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(text-reader) method and validate with the 
        /// expected sequence.
        /// Input : XsvSparse File
        /// Validation : Expected sequence, Sequence Alphabet, Sequence ID.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtParserValidateParseTextReaderReadOnly()
        {
            XsvSparseParserGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.ParseTextReaderReadOnly);
        }

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using ParseOne(file-name) method and validate with the 
        /// expected sequence.
        /// Input : XsvSparse File
        /// Validation : Expected sequence, Sequence Alphabet, Sequence ID.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtParserValidateParseOneFilePathReadOnly()
        {
            XsvSparseParserGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.ParseOneFilePathReadOnly);
        }

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using ParseOne(text-reader) method and validate with the 
        /// expected sequence.
        /// Input : XsvSparse File
        /// Validation : Expected sequence, Sequence Alphabet, Sequence ID.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtParserValidateParseOneTextReaderReadOnly()
        {
            XsvSparseParserGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.ParseOneTextReaderReadOnly);
        }

        /// <summary>
        /// Validate All properties in XsvSparse parser class
        /// Input : One line sequence and update all properties
        /// Validation : Validate the properties
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtParserProperties()
        {
            XsvContigParser xsvParser = new XsvContigParser(Encodings.IupacNA,
                Alphabets.DNA, ',', '#');
            Assert.AreEqual(Constants.XsvSparseDescription, xsvParser.Description);
            Assert.AreEqual(Constants.XsvSparseFileTypes, xsvParser.FileTypes);
            Assert.AreEqual(Constants.XsvSparseName, xsvParser.Name);
            Assert.AreEqual(Constants.XsvSparseFileTypes, xsvParser.FileTypes);
            Assert.AreEqual(Encodings.IupacNA, xsvParser.Encoding);

            Console.WriteLine(
                "Successfully validated all the properties of XsvSparse Parser class.");
            ApplicationLog.WriteLine
                ("Successfully validated all the properties of XsvSparse Parser class.");
        }

        #endregion Test cases

        #region Formatter Test cases

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and format back using 
        /// Format(isequence, filename)
        /// Input : XsvSparse File
        /// Validation : Format is successful.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtFormatterValidateFilePath()
        {
            XsvSparseFormatterGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.FormatFilePath);
        }

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and format back using 
        /// Format(ListSequence, filename)
        /// Input : XsvSparse File
        /// Validation : Format is successful.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtFormatterValidateFilePathWithSeqList()
        {
            XsvSparseFormatterGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.ForamtListWithFilePath);
        }

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and format back using 
        /// Format(Seq, textWrite)
        /// Input : XsvSparse File
        /// Validation : Format is successful.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtFormatterValidateTextWriter()
        {
            XsvSparseFormatterGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.FormatTextWriter);
        }

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and format back using 
        /// Format(Seq,OffSet, textWrite)
        /// Input : XsvSparse File
        /// Validation : Format is successful.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtFormatterValidateTextWriterWithOffset()
        {
            XsvSparseFormatterGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.FormatTextWriterWithOffset);
        }

        /// <summary>
        /// Parse a valid XsvSparse file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and format back using 
        /// Format(SeqList, textWrite)
        /// Input : XsvSparse File
        /// Validation : Format is successful.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtFormatterValidateTextWithSeqList()
        {
            XsvSparseFormatterGeneralTestCases(Constants.SimpleXsvSparseNodeName,
                AdditionalParameters.FormatListTextWriter);
        }

        /// <summary>
        /// Validate All properties in XsvSparse formatter class
        /// Input : One line sequence and update all properties
        /// Validation : Validate the properties
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtFormatterProperties()
        {
            XsvSparseFormatter formatterObj = new XsvSparseFormatter(Constants.CharSeperator,
                Constants.SequenceIDPrefix);
            Assert.AreEqual(Constants.XsvSparseFormatterDescription, formatterObj.Description);
            Assert.AreEqual(Constants.XsvSparseFileTypes, formatterObj.FileTypes);
            Assert.AreEqual(Constants.XsvSparseFormatterNode, formatterObj.Name);
            Assert.AreEqual(Constants.XsvSeperator, formatterObj.Separator);
            Assert.AreEqual(Constants.XsvSeqIdPrefix, formatterObj.SequenceIDPrefix);

            Console.WriteLine(
                "Successfully validated all the properties of XsvSparse Formatter class.");
            ApplicationLog.WriteLine
                ("Successfully validated all the properties of XsvSparse Formatter class.");
        }

        /// <summary>
        /// Validate Sparse FormatString()
        /// Input : Xsv file.
        /// Validation : Validation of formatString() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseBvtFormatterFormatString()
        {
            // Gets the expected sequence from the Xml
            string filePathObj = _utilityObj._xmlUtil.GetTextValue(Constants.SimpleXsvSparseNodeName,
                Constants.FilePathNode);
            string expectedString = _utilityObj._xmlUtil.GetTextValue(Constants.SimpleXsvSparseNodeName,
               Constants.expectedString);

            Assert.IsTrue(File.Exists(filePathObj));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "XsvSparse Formatter BVT: File Exists in the Path '{0}'.",
                filePathObj));

            IList<ISequence> seqList = null;
            SparseSequence sparseSeq = null;
            XsvContigParser parserObj = new XsvContigParser(Encodings.IupacNA, Alphabets.DNA,
                Constants.CharSeperator, Constants.SequenceIDPrefix);

            seqList = parserObj.Parse(filePathObj);
            sparseSeq = (SparseSequence)seqList[0];


            XsvSparseFormatter formatterObj = new XsvSparseFormatter(Constants.CharSeperator,
                Constants.SequenceIDPrefix);

            string formattedString = formatterObj.FormatString(sparseSeq);
            formattedString = formattedString.Replace("\r", "").Replace("\n", "");

            Assert.AreEqual(expectedString, formattedString);

            // Log to Nunit GUI.
            Console.WriteLine("Successfully validated the formatString Xsv file");
            ApplicationLog.WriteLine("Successfully validated the formatString Xsv file");
        }

        /// <summary>
        /// Validate SparseContigFormatter Format()
        /// Input : Xsv file.
        /// Validation : Validation of Format() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void XsvSparseContigFormatterFormat()
        {
            // Gets the expected sequence from the Xml
            string filePathObj = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleXsvSparseNodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePathObj));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "XsvSparse Formatter BVT: File Exists in the Path '{0}'.",
                filePathObj));

            IEncoding encode = Encodings.IupacNA;
            XsvContigParser parserObj = new XsvContigParser(encode,
                Alphabets.DNA, ',', '#');
            Contig contig, expectedContig;

            FileStream filStreamObj = null;
            try
            {
                filStreamObj = File.OpenRead(filePathObj);

                // Parse a file.
                using (TextReader tr = new StreamReader(filStreamObj))
                {
                    contig = parserObj.ParseContig(tr, false);
                }
            }
            finally
            {
                if (filStreamObj != null)
                    filStreamObj.Dispose();
            }

            string seqId = string.Empty;
            foreach (Contig.AssembledSequence seq in contig.Sequences)
            {
                seqId += seq.Sequence.ID + ",";
            }

            // Format Xsv file.
            XsvContigFormatter formatObj = new XsvContigFormatter(',', '#');
            try
            {
                filStreamObj = File.OpenWrite(Constants.XsvTempFileName);

                using (TextWriter tw = new StreamWriter(filStreamObj))
                {
                    formatObj.Format(contig, tw);
                }
            }

            finally
            {
                if (filStreamObj != null)
                    filStreamObj.Dispose();
            }
            try
            {
                filStreamObj = File.OpenRead(Constants.XsvTempFileName);

                // Parse formatted TempFile.
                using (TextReader tr = new StreamReader(filStreamObj))
                {
                    expectedContig = parserObj.ParseContig(tr, false);
                }
            }
            finally
            {
                if (filStreamObj != null)
                    filStreamObj.Dispose();
            }
            string expectedseqId = string.Empty;
            foreach (Contig.AssembledSequence seq in expectedContig.Sequences)
            {
                expectedseqId += seq.Sequence.ID + ",";
            }

            // Validate parsed temp file with original Xsv file.
            Assert.AreEqual(contig.Length, expectedContig.Length);
            Assert.AreEqual(contig.Consensus.Count, expectedContig.Consensus.Count);
            Assert.AreEqual(contig.Consensus.DisplayID, expectedContig.Consensus.DisplayID);
            Assert.AreEqual(contig.Consensus.ID, expectedContig.Consensus.ID);
            Assert.AreEqual(contig.Sequences.Count, expectedContig.Sequences.Count);
            Assert.AreEqual(seqId.Length, expectedseqId.Length);
            Assert.AreEqual(seqId, expectedseqId);

            // Log to Nunit GUI.
            Console.WriteLine("Successfully validated the format Xsv file");
            ApplicationLog.WriteLine("Successfully validated the format Xsv file");
        }

        #endregion

        #region Supporting Methods

        /// <summary>
        /// XsvSparse parser generic method called by all the test cases 
        /// to validate the test case based on the parameters passed.
        /// </summary>
        /// <param name="nodename">Xml node Name.</param>
        /// <param name="additionalParam">Additional parameter 
        /// based on which the validation of  test case is done.</param>
        void XsvSparseParserGeneralTestCases(string nodename,
            AdditionalParameters additionalParam)
        {
            // Gets the expected sequence from the Xml
            string filePathObj = _utilityObj._xmlUtil.GetTextValue(nodename,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePathObj));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "XsvSparse Parser BVT: File Exists in the Path '{0}'.",
                filePathObj));

            IList<ISequence> seqList = null;
            SparseSequence sparseSeq = null;
            XsvContigParser parserObj = new XsvContigParser(Encodings.IupacNA, Alphabets.DNA,
                Constants.CharSeperator, Constants.SequenceIDPrefix);

            string expectedSeqIds = _utilityObj._xmlUtil.GetTextValue(nodename,
                Constants.SequenceIdNode);

            using (StreamReader strReaderObj = new StreamReader(filePathObj))
            {
                switch (additionalParam)
                {
                    case AdditionalParameters.ParseTextReader:
                        seqList = parserObj.Parse(strReaderObj);
                        sparseSeq = (SparseSequence)seqList[0];
                        break;
                    case AdditionalParameters.ParseOneTextReader:
                        sparseSeq =
                            (SparseSequence)parserObj.ParseOne(strReaderObj);
                        break;
                    case AdditionalParameters.ParseOneFilePath:
                        sparseSeq = (SparseSequence)parserObj.ParseOne(filePathObj);
                        break;
                    case AdditionalParameters.ParseFilePath:
                        seqList = parserObj.Parse(filePathObj);
                        sparseSeq = (SparseSequence)seqList[0];
                        break;
                    case AdditionalParameters.ParseTextReaderReadOnly:
                        seqList = parserObj.Parse(strReaderObj, false);
                        sparseSeq = (SparseSequence)seqList[0];
                        break;
                    case AdditionalParameters.ParseOneTextReaderReadOnly:
                        sparseSeq =
                            (SparseSequence)parserObj.ParseOne(strReaderObj,
                            false);
                        break;
                    case AdditionalParameters.ParseOneFilePathReadOnly:
                        sparseSeq = (SparseSequence)parserObj.ParseOne(filePathObj,
                            false);
                        break;
                    default:
                        seqList = parserObj.Parse(filePathObj, false);
                        sparseSeq = (SparseSequence)seqList[0];
                        break;
                }
            }

            if (null == sparseSeq)
            {
                string expCount = _utilityObj._xmlUtil.GetTextValue(nodename,
                    Constants.SequenceCountNode);

                Assert.IsNotNull(seqList);
                Assert.AreEqual(expCount, seqList.Count);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "XsvSparse Parser BVT: Number of Sequences found are '{0}'.",
                    expCount));

                StringBuilder actualId = new StringBuilder();
                foreach (ISequence seq in seqList)
                {
                    SparseSequence sps = (SparseSequence)seq;
                    actualId.Append(sps.ID);
                    actualId.Append(",");
                }

                Assert.AreEqual(expectedSeqIds, actualId.ToString());
            }
            else
            {
                string[] idArray = expectedSeqIds.Split(',');
                Assert.AreEqual(sparseSeq.DisplayID, idArray[0]);
            }

            ApplicationLog.WriteLine(
                "XsvSparse Parser BVT: The XsvSparse sequence is validated successfully with Parse() method.");
            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(
                "XsvSparse Parser BVT: The XsvSparse sequence is validated successfully with Parse() method.");

            Assert.IsNotNull(sparseSeq.Alphabet);
            Assert.AreEqual(sparseSeq.Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                _utilityObj._xmlUtil.GetTextValue(nodename,
                Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture));

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "XsvSparse Parser BVT: The Sequence Alphabet is '{0}' and is as expected.",
                sparseSeq.Alphabet.Name));
        }

        /// <summary>
        /// XsvSparse formatter generic method called by all the test cases 
        /// to validate the test case based on the parameters passed.
        /// </summary>
        /// <param name="nodename">Xml node Name.</param>
        /// <param name="additionalParam">Additional parameter 
        /// based on which the validation of  test case is done.</param>
        void XsvSparseFormatterGeneralTestCases(string nodename,
            AdditionalParameters additionalParam)
        {
            // Gets the expected sequence from the Xml
            string filePathObj = _utilityObj._xmlUtil.GetTextValue(nodename,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePathObj));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "XsvSparse Formatter BVT: File Exists in the Path '{0}'.",
                filePathObj));

            IList<ISequence> seqList = null;
            SparseSequence sparseSeq = null;
            XsvContigParser parserObj = new XsvContigParser(Encodings.IupacNA, Alphabets.DNA,
                Constants.CharSeperator, Constants.SequenceIDPrefix);

            seqList = parserObj.Parse(filePathObj);
            sparseSeq = (SparseSequence)seqList[0];

            IList<IndexedItem<ISequenceItem>> sparseSeqItems =
               sparseSeq.GetKnownSequenceItems();

            XsvSparseFormatter formatterObj = new XsvSparseFormatter(Constants.CharSeperator,
                Constants.SequenceIDPrefix);

            switch (additionalParam)
            {
                case AdditionalParameters.FormatFilePath:
                    formatterObj.Format(sparseSeq, Constants.XsvTempFileName);
                    break;
                default:
                    break;
                case AdditionalParameters.ForamtListWithFilePath:
                    formatterObj.Format(seqList, Constants.XsvTempFileName);
                    break;
                case AdditionalParameters.FormatTextWriter:
                    using (TextWriter writer = new StreamWriter(Constants.XsvTempFileName))
                    {
                        formatterObj.Format(sparseSeq, writer);
                    }
                    break;
                case AdditionalParameters.FormatTextWriterWithOffset:
                    using (TextWriter writer = new StreamWriter(Constants.XsvTempFileName))
                    {
                        formatterObj.Format(sparseSeq, 0, writer);
                    }
                    break;
                case AdditionalParameters.FormatListTextWriter:
                    using (TextWriter writer = new StreamWriter(Constants.XsvTempFileName))
                    {
                        formatterObj.Format(seqList, writer);
                    }
                    break;
            }

            // Parse a formatted Xsv file and validate.
            SparseSequence expectedSeq;
            seqList = parserObj.Parse(Constants.XsvTempFileName);
            expectedSeq = (SparseSequence)seqList[0];

            IList<IndexedItem<ISequenceItem>> expectedSparseSeqItems =
                expectedSeq.GetKnownSequenceItems();

            for (int i = 0; i < sparseSeqItems.Count; i++)
            {
                IndexedItem<ISequenceItem> seqItem = sparseSeqItems[i];
                IndexedItem<ISequenceItem> expectedSeqItem = expectedSparseSeqItems[i];
                Assert.AreEqual(seqItem.Index, expectedSeqItem.Index);
            }

            // Log to Nunit GUI.
            Console.WriteLine("Successfully validated the format Xsv file");
            ApplicationLog.WriteLine("Successfully validated the format Xsv file");

            // Delete the temporary file.
            if (File.Exists(Constants.XsvTempFileName))
                File.Delete(Constants.XsvTempFileName);
        }

        #endregion Supporting Methods
    }
}