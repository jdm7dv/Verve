// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * GenBankBvtTestCases.cs
 * 
 *   This file contains the GenBank - Parsers and Formatters Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using MBF.IO;
using MBF.IO.GenBank;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using MBF.Encoding;

using NUnit.Framework;

namespace MBF.TestAutomation.IO.GenBank
{
    /// <summary>
    /// GenBank Bvt parser and formatter Test case implementation.
    /// </summary>
    [TestFixture]
    public class GenBankBvtTestCases
    {

        #region Global Variables

        // Global variables which store the information of xml file values and is used across the class file.
        static string _filepath;
        static string _alpName;
        static string _molType;
        static string _isReadOnly;
        static string _seqId;
        static string _strTopo;
        static string _strType;
        static string _div;
        static string _version;
        static string _date;
        static string _primId;
        static string _expSeq;

        #endregion Global Variables

        #region Properties

        static string AlphabetName
        {
            get { return GenBankBvtTestCases._alpName; }
            set { GenBankBvtTestCases._alpName = value; }
        }

        static string FilePath
        {
            get { return GenBankBvtTestCases._filepath; }
            set { GenBankBvtTestCases._filepath = value; }
        }

        static string MolType
        {
            get { return GenBankBvtTestCases._molType; }
            set { GenBankBvtTestCases._molType = value; }
        }

        static string IsSequenceReadOnly
        {
            get { return GenBankBvtTestCases._isReadOnly; }
            set { GenBankBvtTestCases._isReadOnly = value; }
        }

        static string SeqId
        {
            get { return GenBankBvtTestCases._seqId; }
            set { GenBankBvtTestCases._seqId = value; }
        }

        static string StrandTopology
        {
            get { return GenBankBvtTestCases._strTopo; }
            set { GenBankBvtTestCases._strTopo = value; }
        }

        static string StrandType
        {
            get { return GenBankBvtTestCases._strType; }
            set { GenBankBvtTestCases._strType = value; }
        }

        static string Div
        {
            get { return GenBankBvtTestCases._div; }
            set { GenBankBvtTestCases._div = value; }
        }

        static string Version
        {
            get { return GenBankBvtTestCases._version; }
            set { GenBankBvtTestCases._version = value; }
        }

        static string SequenceDate
        {
            get { return GenBankBvtTestCases._date; }
            set { GenBankBvtTestCases._date = value; }
        }

        static string PrimaryId
        {
            get { return GenBankBvtTestCases._primId; }
            set { GenBankBvtTestCases._primId = value; }
        }

        static string ExpectedSequence
        {
            get { return GenBankBvtTestCases._expSeq; }
            set { GenBankBvtTestCases._expSeq = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static GenBankBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");

            // Initialization of xml strings.
            FilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankNodeName, Constants.FilePathNode);
            AlphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankNodeName, Constants.AlphabetNameNode);
            MolType = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankNodeName, Constants.MoleculeTypeNode);
            IsSequenceReadOnly = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankNodeName, Constants.IsReadOnlyNode);
            SeqId = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankNodeName, Constants.SequenceIdNode);
            StrandTopology = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankNodeName, Constants.StrandTopologyNode);
            StrandType = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankNodeName, Constants.StrandTypeNode);
            Div = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankNodeName, Constants.DivisionNode);
            Version = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankNodeName, Constants.VersionNode);
            SequenceDate = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankNodeName, Constants.DateNode);
            PrimaryId = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankNodeName, Constants.PrimaryIdNode);
            ExpectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankNodeName, Constants.ExpectedSequenceNode);
        }

        #endregion Constructor

        #region GenBank Parser BVT Test cases

        /// <summary>
        /// Parse a valid GenBank file (Small size sequence less than 35 kb) and 
        /// convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank File
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseFileName()
        {
            // parse
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(FilePath);

            ISequence seq = seqList[0];

            // test the non-metadata properties
            if (0 == string.Compare(IsSequenceReadOnly, "true", false,
                CultureInfo.CurrentCulture))
            {
                Assert.IsTrue(seq.IsReadOnly);
                ApplicationLog.WriteLine(
                    "GenBank Parser BVT: Successfully validated the ReadOnly Property");
            }

            Assert.AreEqual(Utility.GetAlphabet(AlphabetName), seq.Alphabet);
            Assert.AreEqual(Utility.GetMoleculeType(MolType), seq.MoleculeType);
            Assert.AreEqual(SeqId, seq.DisplayID);
            Assert.AreEqual(SeqId, seq.ID);
            ApplicationLog.WriteLine(
                "GenBank Parser BVT: Successfully validated the Alphabet, Molecular type, Sequence ID and Display ID");

            // test the metadata that is tricky to parse, and will not be tested implicitly by
            // testing the formatting
            GenBankMetadata metadata = (GenBankMetadata)seq.Metadata["GenBank"];
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandType,
                    metadata.Locus.Strand.ToString());
            }
            Assert.AreEqual(StrandTopology.ToUpper(CultureInfo.CurrentCulture),
                metadata.Locus.StrandTopology.ToString().ToUpper(CultureInfo.CurrentCulture));
            Assert.AreEqual(Div, metadata.Locus.DivisionCode.ToString());
            Assert.AreEqual(DateTime.Parse(SequenceDate, null),
                metadata.Locus.Date);
            Assert.AreEqual(Version, metadata.Version.Version.ToString());
            Assert.AreEqual(PrimaryId, metadata.Version.GINumber);
            ApplicationLog.WriteLine(
                "GenBank Parser BVT: Successfully validated the StrandType, StrandTopology, Division, Date, Version, PrimaryID Properties");

            // test the sequence string
            Assert.AreEqual(ExpectedSequence, seq.ToString());
            ApplicationLog.WriteLine(
                "GenBank Parser BVT: Successfully validated the Sequence");
            Console.WriteLine(string.Format(null,
                "GenBank Parser BVT: Successfully validated the Sequence '{0}'",
                ExpectedSequence));
        }

        /// <summary>
        /// Parse a valid GenBank file (Small size sequence less than 35 kb) and 
        /// convert the same to one sequence using ParseOne(file-name) method and 
        /// set Alphabet and Encoding value and validate with the expected sequence.
        /// Input : GenBank File
        /// Output : Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseOneWithSpecificFormats()
        {
            // Initialization of xml strings.
            FilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankPrimaryNode,
                Constants.FilePathNode);
            AlphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankPrimaryNode,
                Constants.AlphabetNameNode);
            MolType = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankPrimaryNode,
                Constants.MoleculeTypeNode);
            IsSequenceReadOnly = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankPrimaryNode,
                Constants.IsReadOnlyNode);
            SeqId = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankPrimaryNode,
                Constants.SequenceIdNode);
            StrandTopology = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankPrimaryNode,
                Constants.StrandTopologyNode);
            StrandType = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankPrimaryNode,
                Constants.StrandTypeNode);
            Div = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankPrimaryNode,
                Constants.DivisionNode);
            Version = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankPrimaryNode,
                Constants.VersionNode);
            SequenceDate = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankPrimaryNode,
                Constants.DateNode);
            PrimaryId = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankPrimaryNode,
                Constants.PrimaryIdNode);
            ExpectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankPrimaryNode,
                Constants.ExpectedSequenceNode);

            // parse
            BasicSequenceParser parserObj = new GenBankParser();
            parserObj.Alphabet = Alphabets.Protein;
            parserObj.Encoding = NcbiEAAEncoding.Instance;
            ISequence seq = parserObj.ParseOne(FilePath);

            Assert.AreEqual(Utility.GetAlphabet(AlphabetName),
                seq.Alphabet);
            Assert.AreEqual(Utility.GetMoleculeType(MolType),
                seq.MoleculeType);
            Assert.AreEqual(SeqId, seq.DisplayID);
            Assert.AreEqual(SeqId, seq.ID);
            ApplicationLog.WriteLine(
                "GenBank Parser BVT: Successfully validated the Alphabet, Molecular type, Sequence ID and Display ID");

            // test the metadata that is tricky to parse, and will not be tested implicitly by
            // testing the formatting
            GenBankMetadata metadata = (GenBankMetadata)seq.Metadata["GenBank"];
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandType,
                    metadata.Locus.Strand.ToString());
            }
            Assert.AreEqual(StrandTopology.ToUpper(CultureInfo.CurrentCulture),
                metadata.Locus.StrandTopology.ToString().ToUpper(
                CultureInfo.CurrentCulture));
            Assert.AreEqual(Div, metadata.Locus.DivisionCode.ToString());
            Assert.AreEqual(DateTime.Parse(SequenceDate, null),
                metadata.Locus.Date);
            Assert.AreEqual(Version, metadata.Version.Version.ToString());
            Assert.AreEqual(PrimaryId, metadata.Version.GINumber);
            ApplicationLog.WriteLine(
                "GenBank Parser BVT: Successfully validated the StrandType, StrandTopology, Division, Date, Version, PrimaryID Properties");

            // test the sequence string
            Assert.AreEqual(ExpectedSequence, seq.ToString());
            ApplicationLog.WriteLine(
                "GenBank Parser BVT: Successfully validated the Sequence");
            Console.WriteLine(string.Format(null,
                "GenBank Parser BVT: Successfully validated the Sequence '{0}'",
                ExpectedSequence));
        }
        #endregion GenBank Parser BVT Test cases

        #region GenBank Formatter BVT Test cases

        /// <summary>
        /// Format a valid Sequence (Small size sequence  less than 35 kb) to a 
        /// GenBank file using GenBankFormatter(File-Info) constructor and 
        /// validate the same.
        /// Input : GenBank Sequence
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using File-Info and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatTextWriter()
        {
            // Create a Sequence with all attributes.
            // parse and update the properties instead of parsing entire file.
            ISequenceParser parser1 = new GenBankParser();
            IList<ISequence> seqList1 = parser1.Parse(FilePath);

            string expectedUpdatedSequence =
                ExpectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            Sequence orgSeq =
                new Sequence(Utility.GetAlphabet(AlphabetName), expectedUpdatedSequence);
            orgSeq.Metadata.Add("GenBank",
                (GenBankMetadata)seqList1[0].Metadata["GenBank"]);
            orgSeq.ID = seqList1[0].ID;
            orgSeq.DisplayID = seqList1[0].DisplayID;
            orgSeq.MoleculeType = seqList1[0].MoleculeType;

            ISequenceFormatter formatter = new GenBankFormatter();
            using (TextWriter writer =
                new StreamWriter(Constants.GenBankTempFileName))
            {
                formatter.Format(orgSeq, writer);
            }

            // parse
            GenBankParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(Constants.GenBankTempFileName);

            ISequence seq = seqList[0];

            // test the non-metadata properties
            if (0 == string.Compare(IsSequenceReadOnly, "true",
                false, CultureInfo.CurrentCulture))
            {
                Assert.IsTrue(seq.IsReadOnly);
                ApplicationLog.WriteLine(
                    "GenBank Formatter BVT: Successfully validated the ReadOnly Property");
            }

            Assert.AreEqual(Utility.GetAlphabet(AlphabetName), seq.Alphabet);
            Assert.AreEqual(Utility.GetMoleculeType(MolType), seq.MoleculeType);
            Assert.AreEqual(SeqId, seq.DisplayID);
            Assert.AreEqual(SeqId, seq.ID);
            ApplicationLog.WriteLine(
            "GenBank Formatter BVT: Successfully validated the Alphabet, Molecular type, Sequence ID and Display ID");

            // test the metadata that is tricky to parse, and will not be tested implicitly by
            // testing the formatting
            GenBankMetadata metadata = (GenBankMetadata)seq.Metadata["GenBank"];
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandType, metadata.Locus.Strand.ToString());
            }
            Assert.AreEqual(StrandTopology.ToUpper(CultureInfo.CurrentCulture), metadata.Locus.StrandTopology.ToString().ToUpper(CultureInfo.CurrentCulture));
            Assert.AreEqual(Div, metadata.Locus.DivisionCode.ToString());
            Assert.AreEqual(DateTime.Parse(SequenceDate, null), metadata.Locus.Date);
            Assert.AreEqual(Version, metadata.Version.Version.ToString());
            Assert.AreEqual(PrimaryId, metadata.Version.GINumber);
            ApplicationLog.WriteLine(
            "GenBank Formatter BVT: Successfully validated the StrandType, StrandTopology, Division, Date, Version, PrimaryID Properties");

            // test the sequence string
            Assert.AreEqual(ExpectedSequence, seq.ToString());
            ApplicationLog.WriteLine("GenBank Formatter BVT: Successfully validated the Sequence");
            Console.WriteLine(string.Format(null, "GenBank Formatter BVT: Successfully validated the Sequence '{0}'", ExpectedSequence));

            File.Delete(Constants.GenBankTempFileName);
        }

        /// <summary>
        /// Format a valid Sequence (Small size sequence  less than 35 kb) to a 
        /// GenBank file using GenBankFormatter(File-Path) constructor and 
        /// validate the same.
        /// Input : GenBank Sequence
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatFilePath()
        {
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList1 = parserObj.Parse(FilePath);

            string expectedUpdatedSequence =
                ExpectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            Sequence orgSeq =
                new Sequence(Utility.GetAlphabet(AlphabetName), expectedUpdatedSequence);
            orgSeq.ID = seqList1[0].ID;
            orgSeq.MoleculeType = seqList1[0].MoleculeType;

            orgSeq.Metadata.Add("GenBank", (GenBankMetadata)seqList1[0].Metadata["GenBank"]);

            ISequenceFormatter formatter = new GenBankFormatter();
            formatter.Format(orgSeq, Constants.GenBankTempFileName);

            // parse
            parserObj = new GenBankParser();
            IList<ISequence> seqList =
                parserObj.Parse(Constants.GenBankTempFileName);

            ISequence seq = seqList[0];

            // test the non-metadata properties
            if (0 == string.Compare(IsSequenceReadOnly, "true",
                false, CultureInfo.CurrentCulture))
            {
                Assert.IsTrue(seq.IsReadOnly);
                ApplicationLog.WriteLine(
                    "GenBank Formatter BVT: Successfully validated the ReadOnly Property");
            }

            Assert.AreEqual(Utility.GetAlphabet(AlphabetName), seq.Alphabet);
            Assert.AreEqual(Utility.GetMoleculeType(MolType), seq.MoleculeType);
            Assert.AreEqual(SeqId, seq.DisplayID);
            Assert.AreEqual(SeqId, seq.ID);
            ApplicationLog.WriteLine(
                "GenBank Formatter BVT: Successfully validated the Alphabet, Molecular type, Sequence ID and Display ID");

            // test the metadata that is tricky to parse, and will not be tested implicitly by
            // testing the formatting
            GenBankMetadata metadata =
                (GenBankMetadata)orgSeq.Metadata["GenBank"];
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandType,
                    metadata.Locus.Strand.ToString());
            }
            Assert.AreEqual(StrandTopology.ToUpper(CultureInfo.CurrentCulture),
                metadata.Locus.StrandTopology.ToString().ToUpper(CultureInfo.CurrentCulture));
            Assert.AreEqual(Div, metadata.Locus.DivisionCode.ToString());
            Assert.AreEqual(DateTime.Parse(SequenceDate, null),
                metadata.Locus.Date);
            Assert.AreEqual(Version, metadata.Version.Version.ToString());
            Assert.AreEqual(PrimaryId, metadata.Version.GINumber);
            ApplicationLog.WriteLine(
                "GenBank Formatter BVT: Successfully validated the StrandType, StrandTopology, Division, Date, Version, PrimaryID Properties");

            // test the sequence string
            Assert.AreEqual(ExpectedSequence, seq.ToString());
            ApplicationLog.WriteLine(
                "GenBank Formatter BVT: Successfully validated the Sequence");
            Console.WriteLine(string.Format(null,
                "GenBank Formatter BVT: Successfully validated the Sequence '{0}'",
                ExpectedSequence));

            File.Delete(Constants.GenBankTempFileName);
        }

        /// <summary>
        /// Parse a GenBank File (Small size sequence less than 35 kb) using Parse() 
        /// method and Format the same to a GenBank file using GenBankFormatter(File-Info) 
        /// constructor and validate the same.
        /// Input : GenBank File
        /// Validation :  Read the New GenBank file to which the sequence was formatted 
        /// using File-Info and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterWithParseValidateFormatTextWriter()
        {
            // parse
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(FilePath);

            ISequence seq = seqList[0];

            ISequenceFormatter formatter = new GenBankFormatter();
            using (TextWriter writer =
                new StreamWriter(Constants.GenBankTempFileName))
            {
                formatter.Format(seq, writer);
            }

            // parse
            parserObj = new GenBankParser();
            seqList = parserObj.Parse(Constants.GenBankTempFileName);

            seq = seqList[0];

            // test the non-metadata properties
            if (0 == string.Compare(IsSequenceReadOnly, "true",
                false, CultureInfo.CurrentCulture))
            {
                Assert.IsTrue(seq.IsReadOnly);
                ApplicationLog.WriteLine(
                    "GenBank Formatter BVT: Successfully validated the ReadOnly Property");
            }

            Assert.AreEqual(Utility.GetAlphabet(AlphabetName), seq.Alphabet);
            Assert.AreEqual(Utility.GetMoleculeType(MolType), seq.MoleculeType);
            Assert.AreEqual(SeqId, seq.DisplayID);
            Assert.AreEqual(SeqId, seq.ID);
            ApplicationLog.WriteLine(
                "GenBank Formatter BVT: Successfully validated the Alphabet, Molecular type, Sequence ID and Display ID");

            // test the metadata that is tricky to parse, and will not be tested implicitly by
            // testing the formatting
            GenBankMetadata metadata = (GenBankMetadata)seq.Metadata["GenBank"];
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandType,
                    metadata.Locus.Strand.ToString());
            }
            Assert.AreEqual(StrandTopology.ToUpper(CultureInfo.CurrentCulture),
                metadata.Locus.StrandTopology.ToString().ToUpper(CultureInfo.CurrentCulture));
            Assert.AreEqual(Div, metadata.Locus.DivisionCode.ToString());
            Assert.AreEqual(DateTime.Parse(SequenceDate, null),
                metadata.Locus.Date);
            Assert.AreEqual(Version, metadata.Version.Version.ToString());
            Assert.AreEqual(PrimaryId, metadata.Version.GINumber);
            ApplicationLog.WriteLine(
                "GenBank Formatter BVT: Successfully validated the StrandType, StrandTopology, Division, Date, Version, PrimaryID Properties");

            // test the sequence string
            Assert.AreEqual(ExpectedSequence, seq.ToString());
            ApplicationLog.WriteLine(
                "GenBank Formatter BVT: Successfully validated the Sequence");
            Console.WriteLine(string.Format(null,
                "GenBank Formatter BVT: Successfully validated the Sequence '{0}'",
                ExpectedSequence));

            File.Delete(Constants.GenBankTempFileName);
        }

        /// <summary>
        /// Parse a GenBank File (Small size sequence less than 35 kb) using Parse() 
        /// method and Format the same to a GenBank file using 
        /// GenBankFormatter(File-Path) constructor and validate the same.
        /// Input : GenBank File
        /// Validation :  Read the New GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterWithParseValidateFormatFilePath()
        {
            // parse
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(FilePath);

            ISequence seq = seqList[0];

            ISequenceFormatter formatter = new GenBankFormatter();
            formatter.Format(seq, Constants.GenBankTempFileName);

            // parse
            parserObj = new GenBankParser();
            seqList = parserObj.Parse(Constants.GenBankTempFileName);

            seq = seqList[0];

            // test the non-metadata properties
            if (0 == string.Compare(IsSequenceReadOnly, "true",
                false, CultureInfo.CurrentCulture))
            {
                Assert.IsTrue(seq.IsReadOnly);
                ApplicationLog.WriteLine("Successfully validated the ReadOnly Property");
            }

            Assert.AreEqual(Utility.GetAlphabet(AlphabetName), seq.Alphabet);
            Assert.AreEqual(Utility.GetMoleculeType(MolType), seq.MoleculeType);
            Assert.AreEqual(SeqId, seq.DisplayID);
            Assert.AreEqual(SeqId, seq.ID);
            ApplicationLog.WriteLine(
                "GenBank Formatter BVT: Successfully validated the Alphabet, Molecular type, Sequence ID and Display ID");

            // test the metadata that is tricky to parse, and will not be tested implicitly by
            // testing the formatting
            GenBankMetadata metadata =
                (GenBankMetadata)seq.Metadata["GenBank"];
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandType,
                    metadata.Locus.Strand.ToString());
            }
            Assert.AreEqual(StrandTopology.ToUpper(CultureInfo.CurrentCulture),
                metadata.Locus.StrandTopology.ToString().ToUpper(CultureInfo.CurrentCulture));
            Assert.AreEqual(Div, metadata.Locus.DivisionCode.ToString());
            Assert.AreEqual(DateTime.Parse(SequenceDate, null),
                metadata.Locus.Date);
            Assert.AreEqual(Version, metadata.Version.Version.ToString());
            Assert.AreEqual(PrimaryId, metadata.Version.GINumber);
            ApplicationLog.WriteLine(
                "GenBank Formatter BVT: Successfully validated the StrandType, StrandTopology, Division, Date, Version, PrimaryID Properties");

            // test the sequence string
            Assert.AreEqual(ExpectedSequence, seq.ToString());
            ApplicationLog.WriteLine(
                "GenBank Formatter BVT: Successfully validated the Sequence");
            Console.WriteLine(string.Format(null,
                "GenBank Formatter BVT: Successfully validated the Sequence '{0}'",
                ExpectedSequence));

            File.Delete(Constants.GenBankTempFileName);
        }

        #endregion GenBank Formatter BVT Test cases
    }
}
