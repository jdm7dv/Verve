// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * GenBankP1TestCases.cs
 * 
 *   This file contains the GenBank - Parsers and Formatters Priority One test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using MBF.Encoding;
using MBF.IO;
using MBF.IO.GenBank;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.IO.GenBank
{
    /// <summary>
    /// GenBank Priority One parser and formatter test cases implementation.
    /// </summary>
    [TestFixture]
    public class GenBankP1TestCases
    {

        #region Global Variables

        // Global variables which store the information of xml 
        // file values and is used across the class file.
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
            get { return GenBankP1TestCases._alpName; }
            set { GenBankP1TestCases._alpName = value; }
        }

        static string FilePath
        {
            get { return GenBankP1TestCases._filepath; }
            set { GenBankP1TestCases._filepath = value; }
        }

        static string MolType
        {
            get { return GenBankP1TestCases._molType; }
            set { GenBankP1TestCases._molType = value; }
        }

        static string IsSequenceReadOnly
        {
            get { return GenBankP1TestCases._isReadOnly; }
            set { GenBankP1TestCases._isReadOnly = value; }
        }

        static string SeqId
        {
            get { return GenBankP1TestCases._seqId; }
            set { GenBankP1TestCases._seqId = value; }
        }

        static string StrandTopology
        {
            get { return GenBankP1TestCases._strTopo; }
            set { GenBankP1TestCases._strTopo = value; }
        }

        static string StrandType
        {
            get { return GenBankP1TestCases._strType; }
            set { GenBankP1TestCases._strType = value; }
        }

        static string Div
        {
            get { return GenBankP1TestCases._div; }
            set { GenBankP1TestCases._div = value; }
        }

        static string Version
        {
            get { return GenBankP1TestCases._version; }
            set { GenBankP1TestCases._version = value; }
        }

        static string SequenceDate
        {
            get { return GenBankP1TestCases._date; }
            set { GenBankP1TestCases._date = value; }
        }

        static string PrimaryId
        {
            get { return GenBankP1TestCases._primId; }
            set { GenBankP1TestCases._primId = value; }
        }

        static string ExpectedSequence
        {
            get { return GenBankP1TestCases._expSeq; }
            set { GenBankP1TestCases._expSeq = value; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static GenBankP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region GenBank Parser P1 Test cases

        /// <summary>
        /// Parse a valid GenBank file (Medium size sequence less than 100 Kb) and 
        /// convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank File with size less than 100 Kb
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseMediumSize()
        {
            InitializeXmlVariables(Constants.MediumSizeGenBankNodeName);
            ValidateParserGeneralTestCases();
        }

        /// <summary>
        /// Parse a valid GenBank file (One line Sequence) and 
        /// convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank File with one line sequence
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseOneLineSeq()
        {
            InitializeXmlVariables(Constants.OneLineSequenceGenBankNodeName);
            ValidateParserGeneralTestCases();
        }

        /// <summary>
        /// Parse a valid GenBank file with DNA Sequence and 
        /// convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank File with DNA sequence
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseDnaSequence()
        {
            InitializeXmlVariables(Constants.SimpleGenBankDnaNodeName);
            ValidateParserGeneralTestCases();
        }

        /// <summary>
        /// Parse a valid GenBank file with RNA Sequence and 
        /// convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank File with RNA sequence
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseRnaSequence()
        {
            InitializeXmlVariables(Constants.SimpleGenBankRnaNodeName);
            ValidateParserGeneralTestCases();
        }

        /// <summary>
        /// Parse a valid GenBank file with Protein Sequence and 
        /// convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank File with Protein sequence
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseProteinSequence()
        {
            InitializeXmlVariables(Constants.SimpleGenBankProNodeName);
            ValidateParserGeneralTestCases();
        }

        /// <summary>
        /// Parse a valid GenBank file with mandatory headers and 
        /// convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank File with mandatory headers
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseMandatoryHeaders()
        {
            InitializeXmlVariables(Constants.MandatoryGenBankHeadersNodeName);
            ValidateParserGeneralTestCases();
        }

        /// <summary>
        /// Parse a valid GenBank large file and 
        /// convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank large files 
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseLargeSizeSequence()
        {
            InitializeXmlVariables(Constants.LargeSizeGenBank);
            ValidateParserGeneralTestCases();
        }

        /// <summary>
        /// Parse a valid GenBank file with two sequences and 
        /// convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank large files 
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseMultiSequence()
        {
            ValidateParseMultiSeqTestCases(Constants.MultipleSequenceGenBankNodeName);
        }

        /// <summary>
        /// Parse a valid Simple GenBank file Sequence with Encoding 
        /// Passed as a constructor and convert the same to one 
        /// sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank File with encoding passed as constructor
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseWithEncodingAsConstructor()
        {
            InitializeXmlVariables(Constants.SimpleGenBankDnaNodeName);

            // parse
            IEncoding enc = Encodings.Ncbi2NA;
            ISequenceParser parserObj = new GenBankParser(enc);
            ValidateParserSpecialTestCases(parserObj);
        }

        /// <summary>
        /// Parse a valid Simple GenBank file Sequence with Encoding 
        /// Passed as a property and convert the same to one 
        /// sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank File with encoding passed as property
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseWithEncodingAsProperty()
        {
            InitializeXmlVariables(Constants.SimpleGenBankDnaNodeName);

            // parse
            ISequenceParser parserObj = new GenBankParser();
            parserObj.Encoding = Encodings.Ncbi2NA;
            ValidateParserSpecialTestCases(parserObj);
        }

        /// <summary>
        /// Parse a valid Simple GenBank file Sequence with Alphabet 
        /// Passed as a property and convert the same to one 
        /// sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank File with Alphabet passed as property
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseWithAlphabetAsProperty()
        {
            InitializeXmlVariables(Constants.SimpleGenBankDnaNodeName);

            // parse
            ISequenceParser parserObj = new GenBankParser();
            parserObj.Alphabet = Utility.GetAlphabet(
                Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankDnaNodeName,
                Constants.AlphabetNameNode));
            ValidateParserSpecialTestCases(parserObj);
        }

        /// <summary>
        /// Parse a valid GenBank file with multiple references and 
        /// convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank large files 
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseMultipleReferenceSequence()
        {
            InitializeXmlVariables(Constants.MultipleReferenceGenBankNodeName);
            ValidateParserGeneralTestCases();
        }

        /// <summary>
        /// Parse a valid GenBank file with multiple gene and cds. 
        /// Convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank large files 
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseMultipleGeneCdsSequence()
        {
            InitializeXmlVariables(Constants.MultipleGeneCDSGenBankNodeName);
            ValidateParserGeneralTestCases();
        }

        /// <summary>
        /// Parse a valid GenBank file using text reader with multiple references and 
        /// convert the same to one sequence using Parse(file-name) method and 
        /// validate with the expected sequence.
        /// Input : GenBank large files 
        /// Validation: Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseWithTextReaderMultipleReferenceSequence()
        {
            InitializeXmlVariables(Constants.MultipleReferenceGenBankNodeName);
            ValidateParseWithTextReaderTestCases();
        }

        /// <summary>
        /// Parse a valid GenBank file (Medium size sequence greater than 35 kb 
        /// and less than 100 kb) and convert the same to one sequence using 
        /// ParseOne(file-name) method and set Alphabet and Encoding value and 
        /// validate with the expected sequence.
        /// Input : GenBank File
        /// Output : Properties like StrandType, StrandTopology, Division, Date, 
        /// Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankParserValidateParseOneFileNameWithSpecificFormats()
        {
            InitializeXmlVariables(Constants.SimpleGenBankPrimaryNode);

            // parse
            BasicSequenceParser parserObj = new GenBankParser();
            parserObj.Alphabet = Alphabets.Protein;
            parserObj.Encoding = NcbiEAAEncoding.Instance;
            ISequence seq = parserObj.ParseOne(FilePath);

            ValidateParserGeneralTestCases(seq, ExpectedSequence);
        }
        #endregion GenBank Parser P1 Test cases

        #region GenBank Formatter P1 Test cases

        /// <summary>
        /// Format a valid DNA Sequence to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : DNA GenBank Sequence
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatDnaSequence()
        {
            InitializeXmlVariables(Constants.SimpleGenBankDnaNodeName);
            ValidateFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid RNA Sequence to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : RNA GenBank Sequence
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatRnaSequence()
        {
            InitializeXmlVariables(Constants.SimpleGenBankRnaNodeName);
            ValidateFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Protein Sequence to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : Protein GenBank Sequence
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatProteinSequence()
        {
            InitializeXmlVariables(Constants.SimpleGenBankProNodeName);
            ValidateFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Sequence with Mandatory Headers to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : Protein GenBank Sequence
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatMandatoryHeaders()
        {
            InitializeXmlVariables(Constants.MandatoryGenBankHeadersNodeName);
            ValidateFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Medium Size i.e., less than 100 Kb Sequence to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : GenBank Sequence with less than 100 Kb
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatMediumSizeSequence()
        {
            InitializeXmlVariables(Constants.MediumSizeGenBankNodeName);
            ValidateFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Large Size i.e., greater than 100 Kb and less than 350 Kb Sequence to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : GenBank Sequence with greater than 100 Kb and less than 350 Kb
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatLargeSizeSequence()
        {
            InitializeXmlVariables(Constants.LargeSizeGenBank);
            ValidateFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Sequence with multiple reference to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : GenBank Sequence with greater than 100 Kb and less than 350 Kb
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatMultipleReferenceSequence()
        {
            InitializeXmlVariables(Constants.MultipleReferenceGenBankNodeName);
            ValidateFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Sequence with multiple gene and cds to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : GenBank Sequence with greater than 100 Kb and less than 350 Kb
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatMultipleGeneCdsSequence()
        {
            InitializeXmlVariables(Constants.MultipleGeneCDSGenBankNodeName);
            ValidateFormatterGeneralTestCases();
        }

        /// <summary>
        /// Parse a valid Medium Size i.e., less than 100 Kb File and Format to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : GenBank File with less than 100 Kb
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateParseFormatMediumSizeSequence()
        {
            InitializeXmlVariables(Constants.MediumSizeGenBankNodeName);
            ValidateParseFormatterGeneralTestCases();
        }

        /// <summary>
        /// Parse a valid large Size i.e., greater than 100 Kb and less than 350 Kb File 
        /// and Format to a GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : GenBank File with less than 100 Kb
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateParseFormatLargeSizeSequence()
        {
            InitializeXmlVariables(Constants.LargeSizeGenBank);
            ValidateParseFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Sequence to a GenBank file using GenBankFormatter(File-Path) 
        /// constructor and validate the same by parsing back the file.
        /// Input : DNA GenBank Sequence
        /// Validation :  Parse the GenBank file to which the sequence was formatted 
        /// using File-Path and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatAndParse()
        {
            InitializeXmlVariables(Constants.SimpleGenBankNodeName);
            ValidateFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid DNA Sequence to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : DNA GenBank Sequence
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using textreader and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterParseWithTextReaderValidateFormatDnaSequence()
        {
            InitializeXmlVariables(Constants.SimpleGenBankDnaNodeName);
            ValidateParseWithTextReaderFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid RNA Sequence to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : RNA GenBank Sequence
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using textreader and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterParseWithTextReaderValidateFormatRnaSequence()
        {
            InitializeXmlVariables(Constants.SimpleGenBankRnaNodeName);
            ValidateParseWithTextReaderFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Protein Sequence to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : Protein GenBank Sequence
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using textreader and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterParseWithTextReaderValidateFormatProteinSequence()
        {
            InitializeXmlVariables(Constants.SimpleGenBankProNodeName);
            ValidateParseWithTextReaderFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Sequence with Mandatory Headers to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : Protein GenBank Sequence
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using textreader and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterParseWithTextReaderValidateFormatMandatoryHeaders()
        {
            InitializeXmlVariables(Constants.MandatoryGenBankHeadersNodeName);
            ValidateParseWithTextReaderFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Medium Size i.e., less than 100 Kb Sequence to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : GenBank Sequence with less than 100 Kb
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using text reader and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterParseWithTextReaderValidateFormatMediumSizeSequence()
        {
            InitializeXmlVariables(Constants.MediumSizeGenBankNodeName);
            ValidateParseWithTextReaderFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid large Size i.e., greater than 100 Kb and less than 350 Kb Sequence to a 
        /// GenBank file using GenBankFormatter() constructor and Format() method 
        /// with sequence and writer as parameters and 
        /// validate the same.
        /// Input : GenBank Sequence with greater than 100 Kb and less than 350 Kb
        /// Validation :  Read the GenBank file to which the sequence was formatted 
        /// using text reader and Validate Properties like StrandType, StrandTopology,
        /// Division, Date, Version, PrimaryID, Sequence, Metadata Count and Sequence ID
        /// </summary>
        [Test]
        public void GenBankFormatterParseWithTextReaderValidateFormatLargeSizeSequence()
        {
            InitializeXmlVariables(Constants.LargeSizeGenBank);
            ValidateParseWithTextReaderFormatterGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Sequence to a GenBank file with multiple references  
        /// using GenBankFormatter(File-Path) constructor and validate by reading the 
        /// GenBank file to which the sequence was formatted and its properties. 
        /// Input : GenBank file with multiple references
        /// Validation :  Read the GenBank file and validate Properties like StrandType; 
        /// StrandTopology; Division; Date; Version; PrimaryID; Sequence; Metadata Count 
        /// and Sequence ID.
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatWithFilePathMultipleReferenceSequence()
        {
            InitializeXmlVariables(Constants.MultipleReferenceGenBankNodeName);
            ValidateFormatterWithFilePathGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Sequence to a GenBank file with multiple gene and cds values 
        /// using GenBankFormatter(File-Path) constructor and validate by reading the 
        /// GenBank file to which the sequence was formatted and its properties. 
        /// Input : GenBank file with multiple gene and cds 
        /// Validation :  Read the GenBank file and validate Properties like StrandType; 
        /// StrandTopology; Division; Date; Version; PrimaryID; Sequence; Metadata Count 
        /// and Sequence ID.
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatWithFilePathMultipleGeneCdsSequence()
        {
            InitializeXmlVariables(Constants.MultipleGeneCDSGenBankNodeName);
            ValidateFormatterWithFilePathGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Sequence to a GenBank file with large size sequence 
        /// using GenBankFormatter(File-Path) constructor and validate by reading the 
        /// GenBank file to which the sequence was formatted and its properties. 
        /// Input : GenBank Sequence with greater than 100 Kb and less than 350 Kb
        /// Validation :  Read the GenBank file and validate Properties like StrandType; 
        /// StrandTopology; Division; Date; Version; PrimaryID; Sequence; Metadata Count 
        /// and Sequence ID.
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatWithFilePathLargeSizeSequence()
        {
            InitializeXmlVariables(Constants.LargeSizeGenBank);
            ValidateFormatterWithFilePathGeneralTestCases();
        }

        /// <summary>
        /// Format a valid Sequence to a GenBank file  using GenBankFormatter(File-Path) constructor 
        /// and validate by reading the GenBank file to which the sequence was formatted and its properties. 
        /// Input : GenBank Sequence with greater than 100 Kb and less than 350 Kb
        /// Validation :  Read the GenBank file and validate Properties like StrandType; 
        /// StrandTopology; Division; Date; Version; PrimaryID; Sequence; Metadata Count 
        /// and Sequence ID.
        /// </summary>
        [Test]
        public void GenBankFormatterValidateFormatWithFilePathSimpleSequence()
        {
            InitializeXmlVariables(Constants.SimpleGenBankNodeName);
            ValidateFormatterWithFilePathGeneralTestCases();
        }

        #endregion GenBank Formatter P1 Test cases

        #region Supporting Methods

        /// <summary>
        /// Initializes xml variables for the xml node name specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void InitializeXmlVariables(string nodeName)
        {
            FilePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            AlphabetName = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            MolType = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.MoleculeTypeNode);
            IsSequenceReadOnly = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IsReadOnlyNode);
            SeqId = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode);
            StrandTopology = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.StrandTopologyNode);
            StrandType = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.StrandTypeNode);
            Div = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DivisionNode);
            Version = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.VersionNode);
            SequenceDate = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DateNode);
            PrimaryId = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.PrimaryIdNode);
            ExpectedSequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode);
        }

        /// <summary>
        /// Validates GenBank Parser for General test cases.
        /// </summary>
        static void ValidateParserGeneralTestCases()
        {
            // parse
            ISequenceParser parserObj = new GenBankParser();
            ValidateParserSpecialTestCases(parserObj);
        }

        /// <summary>
        /// Validates GenBank Parser for specific test cases
        /// which takes ISequenceParser as input.
        /// <param name="parser">ISequenceParser object.</param>
        /// </summary>
        static void ValidateParserSpecialTestCases(ISequenceParser parserObj)
        {
            Assert.IsTrue(File.Exists(FilePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "GenBank Parser : File Exists in the Path '{0}'.",
                FilePath));

            IList<ISequence> seqList = parserObj.Parse(FilePath);

            ISequence seq = seqList[0];

            // test the non-metadata properties
            if (0 == string.Compare(IsSequenceReadOnly,
                "true", false, CultureInfo.CurrentCulture))
            {
                Assert.IsTrue(seq.IsReadOnly);
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the ReadOnly Property");
            }

            Assert.AreEqual(Utility.GetAlphabet(AlphabetName), seq.Alphabet);
            Assert.AreEqual(Utility.GetMoleculeType(MolType), seq.MoleculeType);
            Assert.AreEqual(SeqId, seq.DisplayID);
            Assert.AreEqual(SeqId, seq.ID);
            ApplicationLog.WriteLine(
                "GenBank Parser : Successfully validated the Alphabet, Molecular type, Sequence ID and Display ID");

            // test the metadata that is tricky to parse, and will not be tested implicitly by
            // testing the formatting
            GenBankMetadata metadata = (GenBankMetadata)seq.Metadata["GenBank"];
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandType, metadata.Locus.Strand.ToString());
            }
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandTopology.ToUpper(CultureInfo.CurrentCulture),
                    metadata.Locus.StrandTopology.ToString().ToUpper(
                    CultureInfo.CurrentCulture));
            }
            if (metadata.Locus.DivisionCode != SequenceDivisionCode.None)
            {
                Assert.AreEqual(Div, metadata.Locus.DivisionCode.ToString());
            }
            Assert.AreEqual(DateTime.Parse(SequenceDate, null), metadata.Locus.Date);

            if (0 != string.Compare(AlphabetName, "rna", true,
                CultureInfo.CurrentCulture))
            {
                Assert.AreEqual(Version, metadata.Version.Version.ToString());
                Assert.AreEqual(PrimaryId, metadata.Version.GINumber);
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the StrandType, StrandTopology, Division, Date, Version, PrimaryID Properties");
            }
            else
            {
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the StrandType, StrandTopology, Division, Date Properties");
            }

            // Replace all the empty spaces, paragraphs and new line for validation
            string updatedExpSequence =
                ExpectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(
                CultureInfo.CurrentCulture);
            string updatedActualSequence =
                seq.ToString().Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(
                CultureInfo.CurrentCulture);

            Assert.AreEqual(updatedExpSequence, updatedActualSequence);
            ApplicationLog.WriteLine(
                "GenBank Parser : Successfully validated the Sequence");
            Console.WriteLine(string.Format(null,
                "GenBank Parser : Successfully validated the Sequence '{0}'",
                updatedActualSequence));
        }

        /// <summary>
        /// Validates GenBank Parser for specific test cases
        /// which takes sequence list object
        /// <param name="seqList">Sequence list object.</param>
        /// </summary>
        static void ValidateParserSpecialTestCases(IList<ISequence> seqList)
        {
            ISequence seq = seqList[0];

            // test the non-metadata properties
            if (0 == string.Compare(IsSequenceReadOnly, "true",
                false, CultureInfo.CurrentCulture))
            {
                Assert.IsTrue(seq.IsReadOnly);
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the ReadOnly Property");
            }

            Assert.AreEqual(Utility.GetAlphabet(AlphabetName),
                seq.Alphabet);
            Assert.AreEqual(Utility.GetMoleculeType(MolType),
                seq.MoleculeType);
            Assert.AreEqual(SeqId, seq.DisplayID);
            Assert.AreEqual(SeqId, seq.ID);
            ApplicationLog.WriteLine(
                "GenBank Parser : Successfully validated the Alphabet, Molecular type, Sequence ID and Display ID");

            // Test the metadata that is tricky to parse, and will not be tested implicitly by
            // Testing the formatting
            GenBankMetadata metadata =
                (GenBankMetadata)seq.Metadata["GenBank"];
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandType,
                    metadata.Locus.Strand.ToString());
            }
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandTopology.ToUpper(CultureInfo.CurrentCulture),
                    metadata.Locus.StrandTopology.ToString().ToUpper(CultureInfo.CurrentCulture));
            }

            Assert.AreEqual(Div, metadata.Locus.DivisionCode.ToString());
            Assert.AreEqual(DateTime.Parse(SequenceDate, null),
                metadata.Locus.Date);

            if (0 != string.Compare(AlphabetName, "rna",
                true, CultureInfo.CurrentCulture))
            {
                Assert.AreEqual(Version, metadata.Version.Version.ToString());
                Assert.AreEqual(PrimaryId, metadata.Version.GINumber.ToString());
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the StrandType, StrandTopology, Division, Date, Version, PrimaryID Properties");
            }
            else
            {
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the StrandType, StrandTopology, Division, Date Properties");
            }

            // Replace all the empty spaces, paragraphs and new line for validation
            string updatedExpSequence =
                ExpectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(
                CultureInfo.CurrentCulture);
            string updatedActualSequence =
                seq.ToString().Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(
                CultureInfo.CurrentCulture);

            Assert.AreEqual(updatedExpSequence, updatedActualSequence);
            ApplicationLog.WriteLine(
                "GenBank Parser : Successfully validated the Sequence");
            Console.WriteLine(string.Format(null,
                "GenBank Parser : Successfully validated the Sequence '{0}'",
                updatedActualSequence));
        }

        /// <summary>
        /// Validates GenBank Formatter for General test cases.
        /// </summary>
        /// <param name="seqList">sequence list.</param>
        static void ValidateFormatterGeneralTestCases(IList<ISequence> seqList1)
        {
            // Create a Sequence with all attributes.
            // Parse and update the properties instead of parsing entire file.
            string expectedUpdatedSequence =
                ExpectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            Sequence orgSeq =
                new Sequence(Utility.GetAlphabet(AlphabetName), expectedUpdatedSequence);
            orgSeq.Metadata.Add("GenBank",
                (GenBankMetadata)seqList1[0].Metadata["GenBank"]);
            orgSeq.ID = seqList1[0].ID;
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
                    "GenBank Formatter P1: Successfully validated the ReadOnly Property");
            }

            Assert.AreEqual(Utility.GetAlphabet(AlphabetName), seq.Alphabet);
            Assert.AreEqual(Utility.GetMoleculeType(MolType), seq.MoleculeType);
            Assert.AreEqual(SeqId, seq.DisplayID);
            Assert.AreEqual(SeqId, seq.ID);
            ApplicationLog.WriteLine(
                "GenBank Formatter P1: Successfully validated the Alphabet, Molecular type, Sequence ID and Display ID");

            // test the metadata that is tricky to parse, and will not be tested implicitly by
            // testing the formatting
            GenBankMetadata metadata =
                (GenBankMetadata)seq.Metadata["GenBank"];
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandType,
                    metadata.Locus.Strand.ToString());
            }
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandTopology.ToUpper(CultureInfo.CurrentCulture),
                    metadata.Locus.StrandTopology.ToString().ToUpper(CultureInfo.CurrentCulture));
            }
            if (metadata.Locus.DivisionCode != SequenceDivisionCode.None)
            {
                Assert.AreEqual(Div,
                    metadata.Locus.DivisionCode.ToString());
            }
            Assert.AreEqual(DateTime.Parse(SequenceDate, null),
                metadata.Locus.Date);

            if (0 != string.Compare(AlphabetName, "rna",
                true, CultureInfo.CurrentCulture))
            {
                Assert.AreEqual(Version, metadata.Version.Version.ToString());
                Assert.AreEqual(PrimaryId, metadata.Version.GINumber);
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the StrandType, StrandTopology, Division, Date, Version, PrimaryID Properties");
            }
            else
            {
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the StrandType, StrandTopology, Division, Date Properties");
            }

            string truncatedExpectedSequence =
                ExpectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(
                CultureInfo.CurrentCulture);
            string truncatedActualSequence =
                seq.ToString().Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(
                CultureInfo.CurrentCulture);
            // test the sequence string
            Assert.AreEqual(truncatedExpectedSequence, truncatedActualSequence);
            ApplicationLog.WriteLine(
                "GenBank Formatter P1: Successfully validated the Sequence");
            Console.WriteLine(string.Format(null,
                "GenBank Formatter P1: Successfully validated the Sequence '{0}'",
                truncatedExpectedSequence));

            File.Delete(Constants.GenBankTempFileName);
        }

        /// <summary>
        /// Validates GenBank Formatter with file path for General test cases.
        /// </summary>
        /// <param name="seqList">sequence list.</param>
        static void ValidateFormatterWithFilePathGeneralTestCases(IList<ISequence> seqList1)
        {
            // Create a Sequence with all attributes.
            // Parse and update the properties instead of parsing entire file.
            string expectedUpdatedSequence =
                ExpectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            Sequence orgSeq =
                new Sequence(Utility.GetAlphabet(AlphabetName), expectedUpdatedSequence);
            orgSeq.Metadata.Add("GenBank",
                (GenBankMetadata)seqList1[0].Metadata["GenBank"]);
            orgSeq.ID = seqList1[0].ID;
            orgSeq.MoleculeType = seqList1[0].MoleculeType;

            ISequenceFormatter formatter = new GenBankFormatter();
            formatter.Format(orgSeq, Constants.GenBankTempFileName);


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
                    "GenBank Formatter P1: Successfully validated the ReadOnly Property");
            }

            Assert.AreEqual(Utility.GetAlphabet(AlphabetName), seq.Alphabet);
            Assert.AreEqual(Utility.GetMoleculeType(MolType), seq.MoleculeType);
            Assert.AreEqual(SeqId, seq.DisplayID);
            Assert.AreEqual(SeqId, seq.ID);
            ApplicationLog.WriteLine(
                "GenBank Formatter P1: Successfully validated the Alphabet, Molecular type, Sequence ID and Display ID");

            // test the metadata that is tricky to parse, and will not be tested implicitly by
            // testing the formatting
            GenBankMetadata metadata =
                (GenBankMetadata)seq.Metadata["GenBank"];
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandType, metadata.Locus.Strand.ToString());
            }
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandTopology.ToUpper(CultureInfo.CurrentCulture),
                    metadata.Locus.StrandTopology.ToString().ToUpper(CultureInfo.CurrentCulture));
            }

            Assert.AreEqual(Div, metadata.Locus.DivisionCode.ToString());
            Assert.AreEqual(DateTime.Parse(SequenceDate, null), metadata.Locus.Date);

            if (0 != string.Compare(AlphabetName, "rna", true, CultureInfo.CurrentCulture))
            {
                Assert.AreEqual(Version, metadata.Version.Version);
                Assert.AreEqual(PrimaryId, metadata.Version.GINumber);
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the StrandType, StrandTopology, Division, Date, Version, PrimaryID Properties");
            }
            else
            {
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the StrandType, StrandTopology, Division, Date Properties");
            }


            string truncatedExpectedSequence =
                ExpectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(
                CultureInfo.CurrentCulture);
            string truncatedActualSequence =
                seq.ToString().Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(
                CultureInfo.CurrentCulture);

            // test the sequence string
            Assert.AreEqual(truncatedExpectedSequence, truncatedActualSequence);
            ApplicationLog.WriteLine(
                "GenBank Formatter P1: Successfully validated the Sequence");
            Console.WriteLine(string.Format(null,
                "GenBank Formatter P1: Successfully validated the Sequence '{0}'",
                truncatedExpectedSequence));

            if (File.Exists(Constants.GenBankTempFileName))
                File.Delete(Constants.GenBankTempFileName);
        }


        /// <summary>
        /// Parse the file and validate GenBank Formatter for General test cases.
        /// </summary>
        static void ValidateFormatterGeneralTestCases()
        {
            // Parse the file
            ISequenceParser parseObj = new GenBankParser();
            IList<ISequence> seqList = parseObj.Parse(FilePath);

            // Validate the sequence and few more properties.
            ValidateFormatterGeneralTestCases(seqList);
        }

        /// <summary>
        /// Parse the file and validate GenBank Formatter for General test cases.
        /// </summary>
        static void ValidateFormatterWithFilePathGeneralTestCases()
        {
            // Parse the file
            ISequenceParser parseObj = new GenBankParser();
            IList<ISequence> seqList = parseObj.Parse(FilePath);

            // Validate the sequence and few more properties.
            ValidateFormatterWithFilePathGeneralTestCases(seqList);
        }

        /// <summary>
        /// Validates GenBank Formatter for General test cases.
        /// </summary>
        static void ValidateParseWithTextReaderFormatterGeneralTestCases()
        {
            // Parse the file using text reader.
            ISequenceParser parseObj = new GenBankParser();
            IList<ISequence> seqList = null;
            using (TextReader reader = new StreamReader(FilePath))
            {
                seqList = parseObj.Parse(reader);
            }

            // Validate the sequence and few more properties.
            ValidateFormatterGeneralTestCases(seqList);
        }

        /// <summary>
        /// Validates GenBank parser using text reader.
        /// </summary>
        static void ValidateParseWithTextReaderTestCases()
        {
            // Parse the file using text reader.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = null;
            using (TextReader reader = new StreamReader(FilePath))
            {
                seqList = parserObj.Parse(reader);
            }

            // Validate the sequence and few more properties.
            ValidateParserSpecialTestCases(seqList);
        }

        /// <summary>
        /// Validates GenBank Formatter for the files which are 
        /// Parsed using GenBankParser for General test cases.
        /// </summary>
        static void ValidateParseFormatterGeneralTestCases()
        {
            Assert.IsTrue(File.Exists(FilePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "GenBank Parser : File Exists in the Path '{0}'.",
                FilePath));

            // parse
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(FilePath);

            ISequence seq = seqList[0];

            ISequenceFormatter formatter = new GenBankFormatter();
            formatter.Format(seq, Constants.GenBankTempFileName);

            FilePath = Constants.GenBankTempFileName;
            ValidateParserGeneralTestCases();
        }

        /// <summary>
        /// Validates Parse test cases for files with multiple sequences
        /// with the xml node name, fasta file path specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateParseMultiSeqTestCases(string nodeName)
        {
            // Initialize only required properties.
            FilePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            AlphabetName = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            MolType = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.MoleculeTypeNode);
            IsSequenceReadOnly = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IsReadOnlyNode);
            SeqId = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceIdNode);
            StrandTopology = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.StrandTopologyNode);
            StrandType = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.StrandTypeNode);
            Div = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DivisionNode);
            Version = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.VersionNode);
            SequenceDate = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DateNode);
            PrimaryId = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.PrimaryIdNode);

            Assert.IsTrue(File.Exists(FilePath));
            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser : File Exists in the Path '{0}'.", FilePath));

            IList<ISequence> seqs = null;
            GenBankParser parserObj = new GenBankParser();
            seqs = parserObj.Parse(FilePath);

            int seqCount = int.Parse(Utility._xmlUtil.GetTextValue(
                nodeName, Constants.NumberOfSequencesNode), null);
            Assert.IsNotNull(seqs);
            Assert.AreEqual(seqCount, seqs.Count);
            ApplicationLog.WriteLine(string.Format(null,
                "FastA Parser: Number of Sequences found are '{0}'.",
                seqs.Count.ToString((IFormatProvider)null)));

            // Gets the expected sequences from the Xml, in the test cases
            // we are just validating with 2 sequences and maximum 3 
            // sequences. So, based on that we are validating.
            string expectedSequence1 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode1);
            string expectedSequence2 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceNode2);
            string[] expSeqs = null;
            if (2 == seqCount)
            {
                expSeqs = new string[2] { expectedSequence1, expectedSequence2 };
            }
            else
            {
                string expectedSequence3 = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.ExpectedSequenceNode3);
                expSeqs = new string[3] { expectedSequence1, expectedSequence2, 
                    expectedSequence3 };
            }

            // Validate each sequence.
            for (int i = 0; i < seqCount; i++)
            {
                ValidateParserGeneralTestCases(seqs[i], expSeqs[i]);
            }
        }

        /// <summary>
        /// Validates GenBank Parser for each sequence.
        /// which takes sequence list as input.
        /// <param name="seq">Original Sequence.</param>
        /// <param name="expectedSequence">Expected Sequence.</param>
        /// </summary>
        static void ValidateParserGeneralTestCases(ISequence seq,
            string expectedSequence)
        {
            // test the non-metadata properties
            if (0 == string.Compare(IsSequenceReadOnly, "true",
                false, CultureInfo.CurrentCulture))
            {
                Assert.IsTrue(seq.IsReadOnly);
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the ReadOnly Property");
            }

            Assert.AreEqual(Utility.GetAlphabet(AlphabetName),
                seq.Alphabet);
            Assert.AreEqual(Utility.GetMoleculeType(MolType),
                seq.MoleculeType);
            Assert.AreEqual(SeqId, seq.DisplayID);
            Assert.AreEqual(SeqId, seq.ID);
            ApplicationLog.WriteLine(
                "GenBank Parser : Successfully validated the Alphabet, Molecular type, Sequence ID and Display ID");

            // test the metadata that is tricky to parse, and will not be tested implicitly by
            // testing the formatting
            GenBankMetadata metadata =
                (GenBankMetadata)seq.Metadata["GenBank"];
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandType, metadata.Locus.Strand.ToString());
            }
            if (metadata.Locus.Strand != SequenceStrandType.None)
            {
                Assert.AreEqual(StrandTopology.ToUpper(CultureInfo.CurrentCulture),
                    metadata.Locus.StrandTopology.ToString().ToUpper(CultureInfo.CurrentCulture));
            }
            Assert.AreEqual(Div, metadata.Locus.DivisionCode.ToString());
            Assert.AreEqual(DateTime.Parse(SequenceDate, null),
                metadata.Locus.Date);

            if (0 != string.Compare(AlphabetName, "rna",
                true, CultureInfo.CurrentCulture))
            {
                Assert.AreEqual(Version, metadata.Version.Version.ToString());
                Assert.AreEqual(PrimaryId, metadata.Version.GINumber);
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the StrandType, StrandTopology, Division, Date, Version, PrimaryID Properties");
            }
            else
            {
                ApplicationLog.WriteLine(
                    "GenBank Parser : Successfully validated the StrandType, StrandTopology, Division, Date Properties");
            }

            // Replace all the empty spaces, paragraphs and new line for validation
            string updatedExpSequence =
                expectedSequence.Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(CultureInfo.CurrentCulture);
            string updatedActualSequence =
                seq.ToString().Replace("\r", "").Replace("\n", "").Replace(" ", "").ToUpper(CultureInfo.CurrentCulture);

            Assert.AreEqual(updatedExpSequence, updatedActualSequence);
            ApplicationLog.WriteLine(
                "GenBank Parser : Successfully validated the Sequence");
            Console.WriteLine(string.Format(null,
                "GenBank Parser : Successfully validated the Sequence '{0}'",
                updatedActualSequence));
        }

        #endregion Supporting Methods
    }
}
