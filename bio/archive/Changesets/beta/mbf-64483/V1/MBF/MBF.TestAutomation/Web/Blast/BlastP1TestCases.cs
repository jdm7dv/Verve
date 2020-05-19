﻿// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * BlastP1TestCases.cs
 * 
 * This file contains the Blast P1 level Test cases.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using MBF.Web;
using MBF.Web.Blast;

using NUnit.Framework;

namespace MBF.TestAutomation.Web.Blast
{
    /// <summary>
    /// Test Automation code for MBF Blast Web Service and P1 level validations.
    /// </summary>
    [TestFixture]
    public class BlastP1TestCases
    {

        #region Enum

        /// <summary>
        /// NcbiBlastHandler Constructor parameters
        /// Used for the different test cases.
        /// </summary>
        enum NcbiWebServiceCtorParameters
        {
            ConfigPams,
            ParserAndConfigPams,
            Default
        };

        #endregion Enum

        #region Global Variables

        static bool _IsWebServiceAvailable = true;

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static BlastP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");

            _IsWebServiceAvailable = ValidateWebServiceConnection();
        }

        #endregion Constructor

        #region Blast P1 TestCases

        /// <summary>
        /// Validate AddIfAbsent() method by passing protein 
        /// sequence as search query value.
        /// Input Data :Valid protein sequence.
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        [Test]
        public void ValidateAddIfAbsentMethodForProteinSequence()
        {
            ValidateAddIfAbsentMethod(Constants.BlastProteinSequenceParametersNode);
        }

        /// <summary>
        /// Validate AddIfAbsent() method by passing dna sequence 
        /// as query value.
        /// Input Data :Valid dna sequence.
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        [Test]
        public void ValidateAddIfAbsentMethodForDnaSequence()
        {
            ValidateAddIfAbsentMethod(Constants.BlastDnaSequenceParametersNode);
        }

        /// <summary>
        /// Validate AddIfAbsent() method by passing Default 
        /// Database value as search parameter.
        /// Input Data :Valid default database value.
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        [Test]
        public void ValidateAddMethodForSwissprotDatabaseValue()
        {
            // Gets the search query parameter and their values.
            string databaseParameter = Utility._xmlUtil.GetTextValue(
                Constants.BlastDataBaseNode, Constants.DatabaseParameter);
            string defaultDataBaseValue = Utility._xmlUtil.GetTextValue(
                Constants.BlastDataBaseNode, Constants.DefaultDatabaseValue);
            string expectedParametersCount = Utility._xmlUtil.GetTextValue(
                Constants.BlastProgramNode, Constants.expectedParameterCount);

            // Set Service confiruration parameters true.
            IBlastServiceHandler blastService = new NCBIBlastHandler();
            ConfigParameters configParameters = new ConfigParameters();
            configParameters.UseBrowserProxy = true;
            blastService.Configuration = configParameters;

            // Create search parameters object.
            BlastParameters queryParams = new BlastParameters();

            // Add a default database parameter 
            queryParams.AddIfAbsent(databaseParameter, defaultDataBaseValue);

            // Validate default database parameter
            Assert.IsTrue(queryParams.Settings.ContainsValue(defaultDataBaseValue));
            Assert.AreEqual(queryParams.Settings.Count.ToString((IFormatProvider)null),
                expectedParametersCount);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Blast P1: Validation of default database parameter  was completed successfully."));
            Console.WriteLine(string.Format(null,
                "Blast P1: Default database {0} is as expected.", defaultDataBaseValue));
        }

        /// <summary>
        /// Validate Add() method by passing different database values.
        /// Input Data :Valid Database values..
        /// Output Data : Validation of Add() method.
        /// </summary>
        [Test]
        public void ValidateAddMethodForDifferentDatabaseValue()
        {
            // Gets the search query parameter and their values.
            string databaseParameter = Utility._xmlUtil.GetTextValue(
                Constants.BlastDataBaseNode, Constants.DatabaseParameter);
            string swissprotDBValue = Utility._xmlUtil.GetTextValue(
                Constants.BlastDataBaseNode, Constants.SwissprotDBValue);
            string expectedParametersCount = Utility._xmlUtil.GetTextValue(
                Constants.BlastProgramNode, Constants.expectedParameterCount);

            // Set Service confiruration parameters true.
            IBlastServiceHandler blastService = new NCBIBlastHandler();
            ConfigParameters configParameters = new ConfigParameters();
            configParameters.UseBrowserProxy = true;
            blastService.Configuration = configParameters;

            // Create search parameters object.
            BlastParameters queryParams = new BlastParameters();

            // Add a swissprot database parameter 
            queryParams.Add(databaseParameter, swissprotDBValue);

            // Validate swissprot database parameter
            Assert.IsTrue(queryParams.Settings.ContainsValue(swissprotDBValue));
            Assert.AreEqual(queryParams.Settings.Count.ToString(
                (IFormatProvider)null), expectedParametersCount);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Blast P1: Validation of Swissprot database parameter  was completed successfully."));
            Console.WriteLine(string.Format(null,
                "Blast P1: Default database {0} is as expected.", swissprotDBValue));
        }

        /// <summary>
        /// Validate AddIfAbsent() method by passing different program values.
        /// Input Data :Valid program values..
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        [Test]
        public void ValidateAddIfAbsentForDifferentProgramValue()
        {
            // Gets the search query parameter and their values.
            string programParameter = Utility._xmlUtil.GetTextValue(
                Constants.BlastProgramNode, Constants.ProgramParameter);
            string tblastXValue = Utility._xmlUtil.GetTextValue(
                Constants.BlastProgramNode, Constants.blastxValue);
            string expectedParametersCount = Utility._xmlUtil.GetTextValue(
                Constants.BlastProgramNode, Constants.expectedParameterCount);

            // Set Service confiruration parameters true.
            IBlastServiceHandler blastService = new NCBIBlastHandler();
            ConfigParameters configParameters = new ConfigParameters();
            configParameters.UseBrowserProxy = true;
            blastService.Configuration = configParameters;

            // Create search parameters object.
            BlastParameters queryParams = new BlastParameters();

            // Add a tblastx parameter value 
            queryParams.AddIfAbsent(programParameter, tblastXValue);

            // Validate tblastx parameter value
            Assert.IsTrue(queryParams.Settings.ContainsValue(tblastXValue));
            Assert.AreEqual(queryParams.Settings.Count.ToString(
                (IFormatProvider)null), expectedParametersCount);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Blast P1: Validation of tblastx program parameter  was completed successfully."));
            Console.WriteLine(string.Format(null,
                "Blast P1: tblastx parameter {0} is as expected.", tblastXValue));
        }

        /// <summary>
        /// Validate AddIfAbsent() method by passing optional
        /// values For Ex:Expect(Threshold) .
        /// Input Data :Valid optional values..
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        [Test]
        public void ValidateAddIfAbsentForOptionalQuerySearchValues()
        {
            // Gets the search query parameter and their values.
            string optionalParameter = Utility._xmlUtil.GetTextValue(
                Constants.BlastProgramNode, Constants.Expectparameter);
            string optionalParameterValue = Utility._xmlUtil.GetTextValue(
                Constants.BlastProgramNode, Constants.optionalValue);
            string expectedParametersCount = Utility._xmlUtil.GetTextValue(
                Constants.BlastProgramNode, Constants.expectedParameterCount);

            // Set Service confiruration parameters true.
            IBlastServiceHandler blastService = new NCBIBlastHandler();
            ConfigParameters configParameters = new ConfigParameters();
            configParameters.UseBrowserProxy = true;
            blastService.Configuration = configParameters;

            // Create search parameters object.
            BlastParameters queryParams = new BlastParameters();

            // Add a threshold parameter value.
            queryParams.AddIfAbsent(optionalParameter, optionalParameterValue);

            // Validate threshold parameter value.
            Assert.IsTrue(queryParams.Settings.ContainsValue(optionalParameterValue));
            Assert.AreEqual(queryParams.Settings.Count.ToString((
                IFormatProvider)null), expectedParametersCount);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Blast P1: Validation of expect parameter  was completed successfully."));
            Console.WriteLine(string.Format(null,
                "Blast P1: expect value  parameter {0} is as expected.", optionalParameterValue));
        }

        /// <summary>
        /// Validate AddIfAbsent() method by passing medium sized
        /// dna sequence(>10KB) as query value.
        /// Input Data :Valid medium sized dna sequence.
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        [Test]
        public void ValidateAddIfAbsentMethodForMediumSizedDnaSequence()
        {
            ValidateAddIfAbsentMethod(
                Constants.BlastMediumSizeDnaSequenceParametersNode);
        }

        /// <summary>
        /// Validate AddIfAbsent() method by passing medium sized 
        /// protein sequence(>10KB) as query value.
        /// Input Data :Valid medium sized protein sequence.
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        [Test]
        public void ValidateAddIfAbsentMethodForMediumSizedProteinSequence()
        {
            ValidateAddIfAbsentMethod(
                Constants.BlastMediumSizeProteinSequenceParametersNode);
        }

        /// <summary>
        /// Validate AddIfAbsent() method by passing Dna sequence contains "&" char.
        /// Input Data :Valid DNA sequence contains "&" char.
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        [Test]
        public void ValidateAddIfAbsentMethodFortDnaSeqContainsAmpersandChar()
        {
            ValidateAddIfAbsentMethod(
                Constants.BlastDnaSequenceWithAmpersandLetter);
        }

        /// <summary>
        /// Validate AddIfAbsent() method by passing Protein sequence contains "&" char.
        /// Input Data :Valid Protein sequence contains "&" char.
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        [Test]
        public void ValidateAddIfAbsentMethodFortProteinSeqContainsAmpersandChar()
        {
            ValidateAddIfAbsentMethod(
                Constants.BlastProteinSequenceWithAmpersandLetter);
        }

        /// <summary>
        /// Validate SubmitSearchRequest() method by passing medium sized
        /// DNA sequence(>10KB) as query value.
        /// Input Data :Valid medium sized DNA sequence.
        /// Output Data : Validation of SubmitSearchRequest() method.
        /// </summary>
        [Test]
        public void ValidateSubmitSearchRequestMethodForMediumSizedDnaSequence()
        {
            ValidateSubmitSearchMethod(
                Constants.BlastMediumSizeDnaSequenceParametersNode, null, null);

        }

        /// <summary>
        /// Validate SubmitSearchRequest() method by passing medium
        /// sized Protein sequence(>10KB) as query value.
        /// Input Data :Valid medium sized Protein sequence.
        /// Output Data : Validation of SubmitSearchRequest() method.
        /// </summary>
        [Test]
        public void ValidateSubmitSearchRequestMethodForMediumSizedProteinSequence()
        {
            ValidateSubmitSearchMethod(
                Constants.BlastMediumSizeProteinSequenceParametersNode, null, null);
        }

        /// <summary>
        /// Validate SubmitSearchRequest() method by passing Dna 
        /// Sequence with blastx program.
        /// Input Data :Valid dna sequence and blastx program parameters.
        /// Output Data : Validation of SubmitSearchRequest() method.
        /// </summary>
        [Test]
        public void ValidateSubmitSearchRequestMethodForBlastxProgram()
        {
            ValidateSubmitSearchMethod(
                Constants.BlastRequestNodeWithBlastxProgramParameterNode, null, null);
        }

        /// <summary>
        /// Validate SubmitSearchRequest() method by passing Dna 
        /// Sequence with "tblastx" program and "nr" database.
        /// Input Data :Valid dna sequence and tblastx program parameter.
        /// Output Data : Validation of SubmitSearchRequest() method.
        /// </summary>
        [Test]
        public void ValidateSubmitSearchRequestMethodFortBlastxProgramAndNRDatabase()
        {
            ValidateSubmitSearchMethod(
                Constants.BlastRequestNodeWithtBlastxProgramParameter, null, null);
        }

        /// <summary>
        /// Validate SubmitSearchRequest() method by passing Dna 
        /// Sequence with "blastn" program and "nr" database.
        /// Input Data :Valid dna sequence and blastn program
        /// and nr database parameters.
        /// Output Data : Validation of SubmitSearchRequest() method.
        /// </summary>
        [Test]
        public void ValidateSubmitSearchRequestMethodFortBlastnProgramAndNRDatabase()
        {
            ValidateSubmitSearchMethod(
                Constants.BlastRequestNodeWithBlastnProgramParameter, null, null);
        }

        /// <summary>
        /// Validate SubmitSearchRequest() method by passing Dna
        /// Sequence with "tblastn" program and "nr" database.
        /// Input Data :Valid dna sequence and tblastn program 
        /// and "nr" database parameter.
        /// Output Data : Validation of SubmitSearchRequest() method.
        /// </summary>
        [Test]
        public void ValidateSubmitSearchRequestMethodFortBlastnProgram()
        {
            ValidateSubmitSearchMethod(
                Constants.BlastRequestNodeWithBlastnProgramParameter, null, null);
        }

        /// <summary>
        /// Validate SubmitSearchRequest() method by passing Protein
        /// Sequence with swissprot database..
        /// Input Data :Valid Protein sequence and swissprot database. 
        /// Output Data : Validation of SubmitSearchRequest() method.
        /// </summary>
        [Test]
        public void ValidateSubmitSearchRequestMethodForSwissprotDatabase()
        {
            ValidateSubmitSearchMethod(
                Constants.BlastRequestNodeWithtSwissprotDataseParameter, null, null);
        }

        /// <summary>
        /// Validate SubmitSearchRequest() method by passing Protein
        /// Sequence with Pat(Patended Protein sequence) database..
        /// Input Data :Valid Protein sequence and Pat database. 
        /// Output Data : Validation of SubmitSearchRequest() method.
        /// </summary>
        [Test]
        public void ValidateSubmitSearchRequestMethodForPatDatabase()
        {
            ValidateSubmitSearchMethod(
                 Constants.BlastProteinSequenceWithPatDatabaseParameter, null, null);
        }

        /// <summary>
        /// Validate SubmitSearchRequest() method by passing Protein
        /// Sequence with Optional values expect(thresold) and 
        /// compositionbasedstatics.
        /// Input Data :Valid Protein sequence and Pat database. 
        /// Output Data : Validation of SubmitSearchRequest() method.
        /// </summary>
        [Test]
        public void ValidateSubmitSearchRequestMethodForOptionalParameters()
        {
            ValidateSubmitSearchMethod(
                Constants.BlastProteinSequenceWithPatDatabaseParameter,
                "expect", "compositionbasedStatics");
        }

        /// <summary>
        /// Validate SubmitHttpRequest() method for medium sized
        /// (Sequence with >10KB)Dna sequence.
        /// Input Data :Valid medium sized Dna sequence
        /// Output Data : Validation of SubmitHttpRequest() method.
        /// </summary>
        [Test]
        public void ValidateSubmitHttpRequestMethodForMediumSizedDnaSequence()
        {
            ValidateSubmitHttpRequestMethod(
                Constants.BlastMediumSizeDnaSequenceParametersNode);
        }

        /// <summary>
        /// Validate SubmitHttpRequest() method for medium sized
        /// (Sequence with >10KB)Protein sequence.
        /// Input Data :Valid medium sized Protein sequence
        /// Output Data : Validation of SubmitHttpRequest() method.
        /// </summary>
        [Test]
        public void ValidateSubmitHttpRequestMethodForMediumSizedProteinSequence()
        {
            ValidateSubmitHttpRequestMethod(
                Constants.BlastMediumSizeProteinSequenceParametersNode);
        }

        /// <summary>
        /// Validate Parse xml file using parse(file-name) for 
        /// a medium sized blast xml with size >10KB.
        /// Input Data :Valid medium sized Blast xml file..
        /// Output Data : Validation of Blast xml record results.
        /// </summary>
        [Test]
        public void ValidateParseWithMediumSizeBlastXml()
        {
            ValidateParseXmlGeneralTestCases(
                Constants.MediumSizedBlastXmlNode, false);
        }

        /// <summary>
        /// Validate Parse xml file using parse(text-reader) for 
        /// a medium sized blast xml with size >10KB.
        /// Input Data :Valid medium sized Blast xml file..
        /// Output Data : Validation of Blast xml record results.
        /// </summary>
        [Test]
        public void ValidateParseTextReaderWithMediumSizeBlastXml()
        {
            ValidateParseXmlGeneralTestCases(
                Constants.MediumSizedBlastXmlNode, true);
        }

        /// <summary>
        /// Validate Parse xml file using parse(file-name) for 
        /// a large sized blast xml with size >100KB.
        /// Input Data :Valid large sized Blast xml file..
        /// Output Data : Validation of Blast xml record results.
        /// </summary>
        [Test]
        public void ValidateParseWithLargeSizedBlastXml()
        {
            ValidateParseXmlGeneralTestCases(
                Constants.LargeSizedBlastXmlNode, false);
        }

        /// <summary>
        /// Validate Parse xml file using parse(text-reader) for
        /// a large sized blast xml with size >100KB.
        /// Input Data :Valid large sized Blast xml file..
        /// Output Data : Validation of Blast xml record results.
        /// </summary>
        [Test]
        public void ValidateParseTextReaderWithLargeSizedBlastXml()
        {
            ValidateParseXmlGeneralTestCases(
                Constants.LargeSizedBlastXmlNode, true);
        }

        /// <summary>
        /// Validate Parse xml file using parse(file-name) for a 
        /// small sized blast xml with more than three records.
        /// Input Data :Valid small sized Blast xml file..
        /// Output Data : Validation of Blast xml record results.
        /// </summary>
        [Test]
        public void ValidateParseBlastXmlWithMoreThanThreeRecords()
        {
            ValidateParseXmlGeneralTestCases(
                Constants.BlastResultWithMoreThanThreeRecords, false);
        }

        /// <summary>
        /// Validate Parse xml file using parse(text-reader) for small
        /// sized blast xml with more than three records.
        /// Input Data :Valid large sized Blast xml file..
        /// Output Data : Validation of Blast xml record results.
        /// </summary>
        [Test]
        public void ValidateParseTextReaderBlastXmlWithMoreThanThreeRecords()
        {
            ValidateParseXmlGeneralTestCases(
                Constants.BlastResultWithMoreThanThreeRecords, true);
        }

        /// <summary>
        /// Validate Parse xml file using parse(file-name) for a 
        /// small sized blast xml with more than ten records.
        /// Input Data :Valid small sized Blast xml file..
        /// Output Data : Validation of Blast xml record results.
        /// </summary>
        [Test]
        public void ValidateParseBlastXmlWithMorethanTenRecords()
        {
            ValidateParseXmlGeneralTestCases(
                Constants.BlastWithMoreThanTenRecordsNode, false);
        }

        /// <summary>
        /// Validate Parse xml file using parse(text-reader) for a 
        /// small sized blast xml with more than ten records.
        /// Input Data :Valid small sized Blast xml file..
        /// Output Data : Validation of Blast xml record results.
        /// </summary>
        [Test]
        public void ValidateParseTextReaderBlastXmlWithMorethanTenRecords()
        {
            ValidateParseXmlGeneralTestCases(
                Constants.BlastWithMoreThanTenRecordsNode, true);
        }

        /// <summary>
        /// Validate fetching results asynchronous for a valid protein sequence.
        /// Input Data :Valid protein sequence, "nr" Database value and "blastp" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsAsyncForProteinSequence()
        {
            ValidateGeneralFetchResults(
                Constants.ProteinFetchResults, null, null, false);
        }

        /// <summary>
        /// Validate fetching results asynchronous for a valid
        /// protein sequence with swissprot database value.
        /// Input Data :Valid protein sequence, "swissprot" 
        /// database value and "blastp" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsAsyncForProteinSequenceWithSwissprotDatabase()
        {
            ValidateGeneralFetchResults(
                Constants.ProteinFetchResultsWithSwissprot,
                null, null, false);
        }

        /// <summary>
        /// Validate fetching results asynchronous for a valid protein
        /// sequence with Pat database value.
        /// Input Data :Valid protein sequence, "pat" database value and "blastp" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsAsyncForProteinSequenceWithPatDatabase()
        {
            ValidateGeneralFetchResults(
                Constants.ProteinFetchResultsWithPatDatabaseNode,
                null, null, false);
        }

        /// <summary>
        /// Validate fetching results asynchronous for a valid
        /// protein sequence with Pdb database value.
        /// Input Data :Valid protein sequence, "pdb" database 
        /// value and "blastp" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsAsyncForProteinSequenceWithPdbDatabase()
        {
            ValidateGeneralFetchResults(
                Constants.ProteinFetchResultsWithPdbDatabase,
                null, null, false);
        }

        /// <summary>
        /// Validate fetching results asynchronous for a valid 
        /// Dna sequence with blastn program value.
        /// Input Data :Valid Dna sequence, "nr" database value
        /// and "blastn" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsAsyncForDnaSequenceWithBlastnProgram()
        {
            ValidateGeneralFetchResults(
                Constants.DnaSeqFetchResultsWithBlastnProgramNode,
                null, null, false);
        }

        /// <summary>
        /// Validate fetching results asynchronous for a valid 
        /// Dna sequence with blastx program value.
        /// Input Data :Valid Dna sequence, "swissprot" database value 
        /// and "blastx" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsAsyncForDnaSequenceWithBlastXProgram()
        {
            ValidateGeneralFetchResults(
                Constants.DnaSeqFetchResultsWithBlastxProgramNode,
                null, null, false);
        }

        /// <summary>
        /// Validate fetching results asynchronous for a
        /// valid Protein sequence with tblastn program value.
        /// Input Data :Valid Protein sequence, "nr" database value 
        /// and "tblastn" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsAsyncForProteinSequenceWithtBlastnProgram()
        {
            ValidateGeneralFetchResults(
                Constants.ProteinSeqFetchResultsWithtBlastnNode,
                null, null, false);
        }

        /// <summary>
        /// Validate fetching results asynchronous for a valid 
        /// Dna sequence with tblastx program value.
        /// Input Data :Valid Dna sequence, "nr" database value and 
        /// "tblastx" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsAsyncForDnaSequenceWithtBlastXProgram()
        {
            ValidateGeneralFetchResults(
                Constants.DnaSeqFetchResultsWithtBlastxNode,
                null, null, false);
        }

        /// <summary>
        /// Validate fetching results asynchronous for a valid Dna sequence with mandatory fields only.
        /// Input Data :Valid Dna sequence, database value program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsAsyncForDnaSeqWithOnlyMandatoryParameters()
        {
            ValidateGeneralFetchResults(
                Constants.DnaSeqAFetchResultsWithMandatoryFieldsOnlyNode,
                null, null, false);
        }

        /// <summary>
        /// Validate fetching results asynchronous for valid Dna
        /// sequence with mandatory fields and some optional parameters.
        /// Input Data :Valid Dna sequence, "nr" Database value,"tblastx" program value
        /// .and expect value and compositionBasedStaticsParameter. .
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsAsyncForDnaSeqWithOptionalParameters()
        {
            ValidateGeneralFetchResults(
                Constants.DnaSeqAFetchResultsWithMandatoryFieldsOnlyNode,
                "expectParameter", "compositionBasedStaticParameter", false);
        }

        /// <summary>
        /// Validate fetching results Synchronous for a valid protein sequence.
        /// Input Data :Valid protein sequence, "nr" Database value and "blastp" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsSyncForProteinSequence()
        {
            ValidateGeneralFetchResults(
                Constants.ProteinFetchResults, null, null, true);
        }

        /// <summary>
        /// Validate fetching results Synchronous for a valid
        /// protein sequence with swissprot database value.
        /// Input Data :Valid protein sequence, "swissprot" 
        /// database value and "blastp" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsSyncForProteinSequenceWithSwissprotDatabase()
        {
            ValidateGeneralFetchResults(
                Constants.ProteinFetchResultsWithSwissprot,
                null, null, true);
        }

        /// <summary>
        /// Validate fetching results Synchronous for a valid protein
        /// sequence with Pat database value.
        /// Input Data :Valid protein sequence, "pat" database value and "blastp" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsSyncForProteinSequenceWithPatDatabase()
        {
            ValidateGeneralFetchResults(
                Constants.ProteinFetchResultsWithPatDatabaseNode,
                null, null, true);
        }

        /// <summary>
        /// Validate fetching results Synchronous for a valid
        /// protein sequence with Pdb database value.
        /// Input Data :Valid protein sequence, "pdb" database 
        /// value and "blastp" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsSyncForProteinSequenceWithPdbDatabase()
        {
            ValidateGeneralFetchResults(
                Constants.ProteinFetchResultsWithPdbDatabase,
                null, null, true);
        }

        /// <summary>
        /// Validate fetching results Synchronous for a valid 
        /// Dna sequence with blastn program value.
        /// Input Data :Valid Dna sequence, "nr" database value
        /// and "blastn" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsSyncForDnaSequenceWithBlastnProgram()
        {
            ValidateGeneralFetchResults(
                Constants.DnaSeqFetchResultsWithBlastnProgramNode,
                null, null, true);
        }

        /// <summary>
        /// Validate fetching results Synchronous for a valid 
        /// Dna sequence with blastx program value.
        /// Input Data :Valid Dna sequence, "swissprot" database value 
        /// and "blastx" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsSyncForDnaSequenceWithBlastXProgram()
        {
            ValidateGeneralFetchResults(
                Constants.DnaSeqFetchResultsWithBlastxProgramNode,
                null, null, true);
        }

        /// <summary>
        /// Validate fetching results Synchronous for a
        /// valid Dna sequence with tblastn program value.
        /// Input Data :Valid Dna sequence, "nr" database value 
        /// and "tblastn" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsSyncForDnaSequenceWithtBlastnProgram()
        {
            ValidateGeneralFetchResults(
                Constants.ProteinSeqFetchResultsWithtBlastnNode,
                null, null, true);
        }

        /// <summary>
        /// Validate fetching results Synchronous for a valid 
        /// Dna sequence with tblastx program value.
        /// Input Data :Valid Dna sequence, "nr" database value and 
        /// "tblastx" program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsSyncForDnaSequenceWithtBlastXProgram()
        {
            ValidateGeneralFetchResults(
                Constants.DnaSeqFetchResultsWithtBlastxNode,
                null, null, true);
        }

        /// <summary>
        /// Validate fetching results Synchronous for a valid Dna sequence with mandatory fields only.
        /// Input Data :Valid Dna sequence, database value program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsSyncForDnaSeqWithOnlyMandatoryParameters()
        {
            ValidateGeneralFetchResults(
                Constants.DnaSeqAFetchResultsWithMandatoryFieldsOnlyNode,
                null, null, true);
        }

        /// <summary>
        /// Validate fetching results Synchronous for valid Dna
        /// sequence with mandatory fields and some optional parameters.
        /// Input Data :Valid Dna sequence, "nr" Database value,"tblastx" program value
        /// .and expect value and compositionBasedStaticsParameter. .
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [Test]
        public void ValidateFetchResultsSyncForDnaSeqWithOptionalParameters()
        {
            ValidateGeneralFetchResults(
                Constants.DnaSeqAFetchResultsWithMandatoryFieldsOnlyNode,
                "expectParameter", "compositionBasedStaticParameter", true);
        }

        /// <summary>
        /// Validate Confiuration parameters and validate GetBrowserProxy() method 
        /// Input Data : Configuration parameters.
        /// Output Data : Validation of GetBrowserProxy() method.
        /// </summary>
        [Test]
        public void ValidateGetBrowserProxyMethod()
        {
            // Gets the search query parameter and their values.
            string emailId = Utility._xmlUtil.GetTextValue(
                Constants.ConfigurationParametersNode, Constants.EmailAdress);
            string defaultTime = Utility._xmlUtil.GetTextValue(
                Constants.ConfigurationParametersNode, Constants.DefaultTimeOut);
            string retryCount = Utility._xmlUtil.GetTextValue(
                Constants.ConfigurationParametersNode, Constants.RetryCount);
            string retryInterval = Utility._xmlUtil.GetTextValue(
                Constants.ConfigurationParametersNode, Constants.RetryInterval);

            // Set Service confiruration parameters true.
            IBlastServiceHandler blastService = new NCBIBlastHandler();
            ConfigParameters configParameters = new ConfigParameters();
            configParameters.UseBrowserProxy = true;
            configParameters.EmailAddress = emailId;
            configParameters.DefaultTimeout = Convert.ToInt32(defaultTime, null);
            configParameters.RetryCount = Convert.ToInt32(retryCount, null);
            configParameters.UseAsyncMode = true;
            configParameters.UseHttps = true;
            configParameters.RetryInterval = Convert.ToInt32(retryInterval, null);
            blastService.Configuration = configParameters;

            // get a default proxy server.
            WebAccessor accessor = new WebAccessor();
            accessor.GetBrowserProxy();

            // Validate default proxy server.
            Assert.IsNotEmpty(accessor.Proxy.Address.AbsoluteUri.ToString());

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Blast P1: Validation of GetBrowserProxy method was completed successfully."));
            Console.WriteLine(string.Format(null,
                "Blast P1: Proxy web uri {0} is as expected.",
                accessor.Proxy.Address.AbsoluteUri.ToString()));
        }

        /// <summary>
        /// Validate String Validator.
        /// Input Data :Valid parameter name.
        /// Output Data : Validate creation of new request
        /// parameter with string validator.
        /// </summary>
        [Test]
        public void ValidateConfigurationParameterswithStringValidator()
        {
            // Gets the search query parameter and their values.
            string newParameter = Utility._xmlUtil.GetTextValue(
                Constants.ConfigurationParametersNode, Constants.Emailparameter);
            string email = Utility._xmlUtil.GetTextValue(
                Constants.ConfigurationParametersNode, Constants.EmailAdress);
            string emailId1 = Utility._xmlUtil.GetTextValue(
                Constants.ConfigurationParametersNode, Constants.EmailAdress1);
            string emailID2 = Utility._xmlUtil.GetTextValue(
                Constants.ConfigurationParametersNode, Constants.EmailAdress2);

            Dictionary<string, RequestParameter> parameters =
                new Dictionary<string, RequestParameter>();

            // Add a new parameter 
            parameters.Add(newParameter, new RequestParameter(
                newParameter, newParameter, false, email, "string",
                new StringListValidator(emailId1, emailID2)));

            RequestParameter param = parameters[newParameter];

            // Validate a created parameter..
            Assert.AreEqual(param.Description.ToString(), newParameter);
            Console.WriteLine(string.Format(null,
                "Validation of new parameter was completed successfully."));
            Console.WriteLine(string.Format(null,
                "Blast P1: new parameter {0} is as expected.",
                param.Description.ToString()));
        }

        /// <summary>
        /// Validate Double range validator.
        /// Input Data :Valid new parameter name.
        /// Output Data : Validate creation of new request
        /// parameter using double validator.
        /// </summary>
        [Test]
        public void ValidateConfigurationParameterwithDoubleValidator()
        {
            // Gets the search query parameter and their values.
            string newParameter = Utility._xmlUtil.GetTextValue(
                Constants.DoubleRangeParameterNode, Constants.NewParameter);
            string newParamterValue = Utility._xmlUtil.GetTextValue(
                Constants.DoubleRangeParameterNode, Constants.newParameterValue);
            string lowestValue = Utility._xmlUtil.GetTextValue(
                Constants.DoubleRangeParameterNode, Constants.lowestRange);
            string highestValue = Utility._xmlUtil.GetTextValue(
                Constants.DoubleRangeParameterNode, Constants.highestRange);

            Dictionary<string, RequestParameter> parameters =
                new Dictionary<string, RequestParameter>();

            // Add a new parameter 
            parameters.Add(newParameter, new RequestParameter(
                newParameter, newParamterValue, false, string.Empty, "double",
                new DoubleRangeValidator(Convert.ToDouble(lowestValue, null),
                    Convert.ToDouble(highestValue, null))));

            RequestParameter param = parameters[newParameter];

            // Validate created parameter.
            Assert.AreEqual(param.Description.ToString(), newParameter);
            Console.WriteLine(string.Format(null,
                "Validation of new parameter was completed successfully."));
            Console.WriteLine(string.Format(null,
                "Blast P1: new parameter {0} is as expected.", param.Description.ToString()));
        }

        /// <summary>
        /// Validate Int range validator.
        /// Input Data :Valid new parameter name.
        /// Output Data : Validate creation of new request parameter
        /// using int range validator.
        /// </summary>
        [Test]
        public void ValidateConfigurationParameterwithIntRangeValidator()
        {
            // Gets the search query parameter and their values.
            string newParameter = Utility._xmlUtil.GetTextValue(
                Constants.IntRangeParameterNode, Constants.NewParameter);
            string newParamterValue = Utility._xmlUtil.GetTextValue(
                Constants.IntRangeParameterNode, Constants.newParameterValue);
            string lowestValue = Utility._xmlUtil.GetTextValue(
                Constants.IntRangeParameterNode, Constants.lowestRange);
            string highestValue = Utility._xmlUtil.GetTextValue(
                Constants.IntRangeParameterNode, Constants.highestRange);

            Dictionary<string, RequestParameter> parameters =
                new Dictionary<string, RequestParameter>();

            // Add a new parameter 
            parameters.Add(newParameter, new RequestParameter(
                newParameter, newParamterValue, false, string.Empty, "int",
                new IntRangeValidator(Convert.ToInt32(lowestValue, null),
                    Convert.ToInt32(highestValue, null))));

            RequestParameter param = parameters[newParameter];

            // Validate created parameter.
            Assert.AreEqual(param.Description.ToString(), newParameter);
            Console.WriteLine(string.Format(null,
                "Validation of new parameter was completed successfully."));
            Console.WriteLine(string.Format(null,
                "Blast P1: new parameter {0} is as expected.", param.Description.ToString()));
        }

        /// <summary>
        /// Validate Ncbi Web Service Em_rel db results by passing config parameters to Ncbi constructor.
        /// Input Data : Valid Config Parameters
        /// Output Data : Validation of blast results 
        /// </summary>
        [Test]
        public void ValidateNcbiBlastResultsUsingConstructorPams()
        {
            GeneralMethodToValidateResults(Constants.EmRelNcbiDatabaseParametersNode,
                NcbiWebServiceCtorParameters.ParserAndConfigPams);
        }

        /// <summary>
        /// Validate Ncbi Web Service Em_rel db results by passing config 
        /// parameters and Blast Parameter to Ncbi constructor.
        /// Input Data : Valid Config and Blast Parameters
        /// Output Data : Validation of blast results 
        /// </summary>
        [Test]
        public void ValidateNcbiBlastResultsUsingConfigPams()
        {
            GeneralMethodToValidateResults(Constants.EmRelNcbiDatabaseParametersNode,
                NcbiWebServiceCtorParameters.ConfigPams);
        }

        /// <summary>
        /// Validate Ncbi Web Service Submit request for List of Sequences.
        /// Input Data : Valid List of sequences.
        /// Output Data : Validation of blast results 
        /// </summary>
        [Test]
        public void ValidateNcbiSubmitSearchRequestForQueryList()
        {
            string nodeName = Constants.EmRelDatabaseParametersNode;

            // Gets the search query parameter and their values.
            string querySequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.QuerySequency);
            string queryDatabaseValue = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseValue);
            string queryProgramValue = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ProgramValue);
            string queryDatabaseParameter = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseParameter);
            string queryProgramParameter = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ProgramParameter);
            string alphabetName = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string emailParameter = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Emailparameter);
            string email = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.EmailAdress);

            // Create a sequence.
            IList<ISequence> seqList = new List<ISequence>();
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), querySequence);
            seqList.Add(seq);

            // Set Service confiruration parameters true.
            IBlastServiceHandler NcbiBlastService = new NCBIBlastHandler();
            ConfigParameters configParameters = new ConfigParameters();
            configParameters.UseBrowserProxy = true;
            NcbiBlastService.Configuration = configParameters;

            // Create search parameters object.
            BlastParameters NcbiParams = new BlastParameters();

            // Add parameter values to search query parameters.
            NcbiParams.Add(queryDatabaseParameter, queryDatabaseValue);
            NcbiParams.Add(queryProgramParameter, queryProgramValue);
            NcbiParams.Add(emailParameter, email);

            // Get Request identifier from web service for SeqList.
            // Automated this case to hit the Code.
            try
            {
                NcbiBlastService.SubmitRequest(seqList, NcbiParams);
                Assert.Fail();
            }
            catch (NotImplementedException)
            {
                Console.WriteLine(string.Format(null,
                    "Ncbi Blast P1 : Validated the exception successfully."));
            }
        }

        #endregion Blast P1 TestCases

        #region Supporting Methods

        /// <summary>
        /// Validate general SubmitSearchRequest method test cases 
        /// with the different parameters specified.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="expectParameter">Expected parameter</param>
        /// <param name="compositionBasedStaticParameter">Composition based static parameter</param>
        /// </summary>
        static void ValidateSubmitSearchMethod(string nodeName,
            string expectParameter, string compositionBasedStaticParameter)
        {
            if (_IsWebServiceAvailable)
            {
                // Gets the search query parameter and their values.
                string querySequence = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.QuerySequency);
                string queryDatabaseValue = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.DatabaseValue);
                string queryProgramValue = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.ProgramValue);
                string queryDatabaseParameter = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.DatabaseParameter);
                string queryProgramParameter = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.ProgramParameter);
                string alphabetName = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.AlphabetNameNode);
                string optionalExpectParameter = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.BlastExpectParameter);
                string optionalExpectValue = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.BlastExpectparameterValue);
                string optionalCBSParameter = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.BlastCompositionBasedStaticsParameter);
                string optionalCBSParameterValue = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.BlastCompositionBasedStaticsValue);
                string reqId = string.Empty;

                // Create a sequence.
                Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName),
                    querySequence);

                // Set Service confiruration parameters true.
                IBlastServiceHandler blastService = new NCBIBlastHandler();
                ConfigParameters configParameters = new ConfigParameters();
                configParameters.UseBrowserProxy = true;
                blastService.Configuration = configParameters;

                // Create search parameters object.
                BlastParameters queryParams = new BlastParameters();

                // Add parameter values to search query parameters.
                if ((0 == string.Compare(expectParameter, null, true,
                    CultureInfo.CurrentCulture))
                    && (0 == string.Compare(compositionBasedStaticParameter, null, true,
                    CultureInfo.CurrentCulture)))
                {
                    queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
                    queryParams.Add(queryProgramParameter, queryProgramValue);
                }
                else
                {
                    queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
                    queryParams.Add(queryProgramParameter, queryProgramValue);
                    queryParams.Add(optionalExpectParameter, optionalExpectValue);
                    queryParams.Add(optionalCBSParameter, optionalCBSParameterValue);
                }

                // Submit search request with valid parameter.
                reqId = blastService.SubmitRequest(seq, queryParams);

                // Validate request identifier returned by web service.
                Assert.IsNotNull(reqId);

                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format(null,
                    "Blast P1: Validation of SubmitSearchRequest() method was completed successfully."));
                Console.WriteLine(string.Format(null,
                    "Blast P1: Request Id {0} is as expected.", reqId));
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate general AddIfAbsent() method by passing 
        /// differnt XML node name
        /// <param name="nodeName">xml node name.</param>
        /// </summary>
        static void ValidateAddIfAbsentMethod(string nodeName)
        {
            // Gets the search query parameter and their values.
            string querySequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.QuerySequency);
            string querySequenceParameter = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.QuerySequencyparameter);

            // Set Service confiruration parameters true.
            IBlastServiceHandler blastService = new NCBIBlastHandler();
            ConfigParameters configParameters = new ConfigParameters();
            configParameters.UseBrowserProxy = true;
            blastService.Configuration = configParameters;

            // Create search parameters object.
            BlastParameters queryParams = new BlastParameters();

            // Add mandatory sequence as query paramter 
            queryParams.AddIfAbsent(querySequenceParameter, querySequence);

            // Validate added sequence to request setting configuration.
            Assert.IsTrue(queryParams.Settings.ContainsValue(querySequence));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Blast P1: Validation of Add method was completed successfully."));
            Console.WriteLine(string.Format(null,
                "Blast P1: Query Sequence{0} is as expected.", querySequence));
        }

        /// <summary>
        /// Validate general SubmitHttpRequest() method by passing 
        /// differnt XML node name
        /// <param name="nodeName">xml node name.</param>
        /// </summary>
        static void ValidateSubmitHttpRequestMethod(string nodeName)
        {
            if (_IsWebServiceAvailable)
            {
                // Gets the search query parameter and their values.
                string querySequence = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.QuerySequency);
                string queryDatabaseValue = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.DatabaseValue);
                string queryProgramValue = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.ProgramValue);
                string queryDatabaseParameter = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.DatabaseParameter);
                string queryProgramParameter = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.ProgramParameter);
                string queryParameter = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.QuerySequencyparameter);
                string webUri = Utility._xmlUtil.GetTextValue(
                    Constants.BlastRequestParametersNode, Constants.BlastWebServiceUri);
                WebAccessorResponse requestResult = null;

                // Set Service confiruration parameters true.
                IBlastServiceHandler blastService = new NCBIBlastHandler();
                ConfigParameters configParameters = new ConfigParameters();
                configParameters.UseBrowserProxy = true;
                blastService.Configuration = configParameters;

                // Create search parameters object.
                BlastParameters queryParams = new BlastParameters();

                // Add mandatory parameter values to search query parameters.
                queryParams.Add(queryParameter, querySequence);
                queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
                queryParams.Add(queryProgramParameter, queryProgramValue);

                //Submit Http request
                WebAccessor webAccessor = new WebAccessor();
                webAccessor.GetBrowserProxy();
                requestResult = webAccessor.SubmitHttpRequest(new Uri(webUri), true,
                    queryParams.Settings);

                // Validate the Submitted request.
                Assert.IsTrue(requestResult.IsSuccessful);
                Console.WriteLine(string.Format(null, "Http Request was submitted successfully"));
                Console.WriteLine(string.Format(null, "Blast P1: DataBase Value {0} is as expected.",
                    queryDatabaseValue));
                Console.WriteLine(string.Format(null, "Blast P1: Program Value {0} is as expected.",
                    queryProgramValue));
                Console.WriteLine(string.Format(null, "Blast P1: Query sequence {0} is as expected.",
                    querySequence));
                webAccessor.Close();
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate general Parse() method by passing 
        /// differnt XML node name
        /// <param name="nodeName">xml node name.</param>
        /// <param name="textReader">true to validate Parse(text-reader)
        /// method else parse(file-name)</param>
        /// </summary>
        static void ValidateParseXmlGeneralTestCases(string nodeName, bool textReader)
        {
            // Gets the Blast Xml file 
            string blastFilePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BlastResultfilePath);
            string expectedBitScore = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BitScore);
            string expectedDatabselength = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseLength);
            string expectedParameterMatrix = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ParameterMatrix);
            string expectedGapCost = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ParameterGap);
            string expectedHitSequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.HitSequence);
            string expectedAccession = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.HitAccession);
            string expectedAlignmentLength = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlignmentLength);
            string expectedResultCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ResultsCount);
            string expectedHitsCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.HitsCount);

            // Validate if Xml file exists in the mentioned path.
            Assert.IsTrue(File.Exists(blastFilePath));

            // Logs information to the log file
            Console.WriteLine(string.Format(null,
                "Blast P1: File Exists in the Path '{0}'.", blastFilePath));

            // Parse a Blast xml file.
            BlastXmlParser parser = new BlastXmlParser();
            IList<BlastResult> blastResults = null;

            if (textReader)
            {
                using (StreamReader reader = File.OpenText(blastFilePath))
                {
                    blastResults = parser.Parse(reader);
                }
            }
            else
            {
                blastResults = parser.Parse(blastFilePath);
            }

            blastResults = parser.Parse(blastFilePath);

            // Validate Meta data 
            BlastXmlMetadata meta = blastResults[0].Metadata;

            Assert.AreEqual(meta.ParameterGapOpen.ToString((IFormatProvider)null),
                expectedGapCost);
            Assert.AreEqual(meta.ParameterMatrix.ToString((IFormatProvider)null),
                expectedParameterMatrix);

            // Validate blast records.
            BlastSearchRecord record = blastResults[4].Records[0];

            Assert.AreEqual(expectedResultCount,
                blastResults.Count.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedDatabselength,
                record.Statistics.DatabaseLength.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedHitsCount,
                record.Hits.Count.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedAccession,
                record.Hits[0].Accession.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedHitsCount,
                record.Hits[0].Hsps.Count.ToString((IFormatProvider)null));

            // Validate bit score.
            Hsp highScoreSgment = record.Hits[0].Hsps[0];
            Assert.AreEqual(expectedAlignmentLength,
                highScoreSgment.AlignmentLength.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedBitScore, highScoreSgment.BitScore.ToString((
                IFormatProvider)null));
            Assert.AreEqual(expectedHitSequence, highScoreSgment.HitSequence.ToString(
                (IFormatProvider)null));

            // Log results to Nunit GUI.
            Console.WriteLine(string.Format(null, "Blast P1: Bit Sequence '{0}'.",
                highScoreSgment.HitSequence.ToString((IFormatProvider)null)));
            Console.WriteLine(string.Format(null, "Blast P1: Bit Score '{0}'.",
                highScoreSgment.BitScore));
            Console.WriteLine(string.Format(null, "Blast P1: Bit Alignment '{0}'.",
                highScoreSgment.AlignmentLength));
            Console.WriteLine(string.Format(null, "Blast P1: Hits Count '{0}'.",
                record.Hits[0].Hsps.Count));
            Console.WriteLine(string.Format(null, "Blast P1: Results Count '{0}'.",
                blastResults.Count));
        }

        /// <summary>
        /// Validate general fetching results.by passing 
        /// differnt parameters.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="expectParameter">expectparameter name</param>
        /// <param name="compositionBasedStaticParameter">compositionBasedStaticParameter name</param>
        /// <param name="isFetchSynchronous">Is Fetch Synchronous?</param>
        /// </summary>
        static void ValidateGeneralFetchResults(
            string nodeName, string expectParameter,
            string compositionBasedStaticParameter,
            bool isFetchSynchronous)
        {
            // Gets the search query parameter and their values.
            string queryParameter = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.QuerySequencyparameter);
            string querySequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.QuerySequency);
            string queryDatabaseValue = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseValue);
            string queryProgramValue = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ProgramValue);
            string queryDatabaseParameter = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseParameter);
            string queryProgramParameter = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ProgramParameter);
            string optionalExpectParameter = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BlastExpectParameter);
            string optionalExpectValue = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BlastExpectparameterValue);
            string optionalCBSParameter = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BlastCompositionBasedStaticsParameter);
            string optionalCBSParameterValue = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BlastCompositionBasedStaticsValue);
            string expectedHitId = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.HitID);
            string expectedAccession = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.HitAccession);

            object responseResults = null;

            NCBIBlastHandler service = new NCBIBlastHandler();
            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            service.Configuration = configParams;
            ISequence sequence = null;
            BlastParameters searchParams = new BlastParameters();

            // Set mandatory parameters only if optional parameters are null
            if ((0 == string.Compare(expectParameter, null, true,
                CultureInfo.CurrentCulture))
                && (0 == string.Compare(compositionBasedStaticParameter, null, true,
                CultureInfo.CurrentCulture)))
            {
                searchParams.Add(queryParameter, querySequence);
                searchParams.Add(queryDatabaseParameter, queryDatabaseValue);
                searchParams.Add(queryProgramParameter, queryProgramValue);
            }
            else
            {
                searchParams.Add(queryParameter, querySequence);
                searchParams.Add(queryDatabaseParameter, queryDatabaseValue);
                searchParams.Add(queryProgramParameter, queryProgramValue);
                searchParams.Add(optionalExpectParameter, optionalExpectValue);
                searchParams.Add(optionalCBSParameter, optionalCBSParameterValue);
            }

            // Careate a request without passing sequence.
            string reqId = service.SubmitRequest(sequence, searchParams);

            // validate request identifier.
            Assert.IsNotNull(reqId);

            // submit request identifier and get the status
            ServiceRequestInformation info = service.GetRequestStatus(reqId);
            if (info.Status != ServiceRequestStatus.Waiting &&
                info.Status != ServiceRequestStatus.Ready)
            {
                string err = ApplicationLog.WriteLine("Unexpected status: '{0}'", info.Status);
                Assert.Fail(err);
            }

            // get async results, poll until ready
            int maxAttempts = 10;
            int attempt = 1;
            while (attempt <= maxAttempts
                && info.Status != ServiceRequestStatus.Error
                && info.Status != ServiceRequestStatus.Ready)
            {
                if (isFetchSynchronous)
                {
                    info = service.GetRequestStatus(reqId);
                    Thread.Sleep(info.Status == ServiceRequestStatus.Waiting
                        || info.Status == ServiceRequestStatus.Queued
                        ? 20000 * attempt : 0);
                }
                else
                {
                    Thread.Sleep(info.Status == ServiceRequestStatus.Waiting ? 30000 : 0);
                    info = service.GetRequestStatus(reqId);
                }
                ++attempt;
            }

            IBlastParser blastXmlParser = new BlastXmlParser();
            responseResults = blastXmlParser.Parse(
                new StringReader(service.GetResult(reqId, searchParams)));

            // Validate blast results.
            Assert.IsNotNull(responseResults);
            List<BlastResult> blastResults = responseResults as List<BlastResult>;
            Assert.IsNotNull(blastResults);
            BlastSearchRecord record = blastResults[0].Records[0];
            Hit hit = record.Hits[0];
            Assert.AreEqual(hit.Accession, expectedAccession);
            Assert.AreEqual(hit.Id.ToString((IFormatProvider)null), expectedHitId);
            Console.WriteLine(string.Format(null,
                "Blast BVT: Hits count '{0}'.", blastResults.Count));
            Console.WriteLine(string.Format(null,
                "Blast BVT: Accession '{0}'.", hit.Accession));
            Console.WriteLine(string.Format(null, "Blast BVT: Hit Id '{0}'.", hit.Id));
            Console.WriteLine(string.Format(null,
                "Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));

            // Validate the results Synchronously with the results got earlier.
            if (isFetchSynchronous)
            {
                IList<BlastResult> syncBlastResults =
                    service.FetchResultsSync(reqId, searchParams) as List<BlastResult>;
                Assert.IsNotNull(syncBlastResults);
                if (null != blastResults[0].Records[0].Hits
                    && 0 < blastResults[0].Records[0].Hits.Count
                    && null != blastResults[0].Records[0].Hits[0].Hsps
                    && 0 < blastResults[0].Records[0].Hits[0].Hsps.Count)
                {
                    Assert.AreEqual(blastResults[0].Records[0].Hits[0].Hsps[0].QuerySequence,
                        syncBlastResults[0].Records[0].Hits[0].Hsps[0].QuerySequence);
                }
                else
                {
                    ApplicationLog.WriteLine("No significant hits found with the these parameters.");
                    Console.WriteLine("No significant hits found with the these parameters.");
                }
            }
        }

        /// <summary>
        /// Validate general fetching results by passing 
        /// different parameters.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="NcbiCtorPam">Ncbi constructor different parameters</param>
        static void GeneralMethodToValidateResults(string nodeName,
            NcbiWebServiceCtorParameters NcbiCtorPam)
        {
            if (_IsWebServiceAvailable)
            {
                // Gets the search query parameter and their values.
                string queryParameter = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.QuerySequencyparameter);
                string querySequence = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.QuerySequency);
                string queryDatabaseValue = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.DatabaseValue);
                string queryProgramValue = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.ProgramValue);
                string queryDatabaseParameter = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.DatabaseParameter);
                string queryProgramParameter = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.ProgramParameter);
                string expectedHitId = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.HitID);
                string expectedAccession = Utility._xmlUtil.GetTextValue(
                    nodeName, Constants.HitAccession);

                object responseResults = null;

                NCBIBlastHandler service;
                IBlastParser blastXmlParser = new BlastXmlParser();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;

                switch (NcbiCtorPam)
                {
                    case NcbiWebServiceCtorParameters.ConfigPams:
                        service = new NCBIBlastHandler(configParams);
                        break;
                    case NcbiWebServiceCtorParameters.ParserAndConfigPams:
                        service = new NCBIBlastHandler(blastXmlParser, configParams);
                        break;
                    default:
                        service = new NCBIBlastHandler();
                        break;
                }

                ISequence sequence = null;
                BlastParameters searchParams = new BlastParameters();

                searchParams.Add(queryParameter, querySequence);
                searchParams.Add(queryDatabaseParameter, queryDatabaseValue);
                searchParams.Add(queryProgramParameter, queryProgramValue);

                // Careate a request without passing sequence.
                string reqId = service.SubmitRequest(sequence, searchParams);

                // validate request identifier.
                Assert.IsNotNull(reqId);

                // submit request identifier and get the status
                ServiceRequestInformation info = service.GetRequestStatus(reqId);
                if (info.Status != ServiceRequestStatus.Waiting
                    && info.Status != ServiceRequestStatus.Ready)
                {
                    string err = ApplicationLog.WriteLine("Unexpected status: '{0}'", info.Status);
                    Assert.Fail(err);
                }

                // get async results, poll until ready
                int maxAttempts = 10;
                int attempt = 1;
                while (attempt <= maxAttempts
                    && info.Status != ServiceRequestStatus.Error
                    && info.Status != ServiceRequestStatus.Ready)
                {
                    Thread.Sleep(info.Status == ServiceRequestStatus.Waiting ? 40000 : 0);
                    info = service.GetRequestStatus(reqId);
                    ++attempt;
                }

                responseResults = blastXmlParser.Parse(
                        new StringReader(service.GetResult(reqId, searchParams)));

                // Validate blast results.
                Assert.IsNotNull(responseResults);
                List<BlastResult> blastResults = responseResults as List<BlastResult>;
                Assert.IsNotNull(blastResults);
                BlastSearchRecord record = blastResults[0].Records[0];
                Hit hit = record.Hits[0];
                Assert.AreEqual(hit.Accession, expectedAccession);
                Assert.AreEqual(hit.Id.ToString((IFormatProvider)null), expectedHitId);
                Console.WriteLine(string.Format(null, "Blast BVT: Hits count '{0}'.", blastResults.Count));
                Console.WriteLine(string.Format(null, "Blast BVT: Accession '{0}'.", hit.Accession));
                Console.WriteLine(string.Format(null, "Blast BVT: Hit Id '{0}'.", hit.Id));
                Console.WriteLine(string.Format(null, "Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate if the connection is successful
        /// </summary>
        /// <returns>True, if connection is successful</returns>
        static bool ValidateWebServiceConnection()
        {
            // Gets the search query parameter and their values.
            string querySequence = Utility._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.QuerySequency);
            string queryDatabaseValue = Utility._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.DatabaseValue);
            string queryProgramValue = Utility._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.ProgramValue);
            string queryDatabaseParameter = Utility._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.DatabaseParameter);
            string queryProgramParameter = Utility._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.ProgramParameter);
            string reqId = string.Empty;

            // Create a sequence.
            Sequence seq = new Sequence(Alphabets.Protein, querySequence);

            // Set Service confiruration parameters true.
            IBlastServiceHandler blastService = new NCBIBlastHandler();
            ConfigParameters configParameters = new ConfigParameters();
            configParameters.UseBrowserProxy = true;
            blastService.Configuration = configParameters;

            // Create search parameters object.
            BlastParameters queryParams = new BlastParameters();

            // Add mandatory parameter values to search query parameters.
            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);

            try
            {
                // Create a request 
                reqId = blastService.SubmitRequest(seq, queryParams);

                if (null == reqId)
                {
                    Console.WriteLine(
                        "Blast P1 : Connection not successful with no Request ID");
                    ApplicationLog.WriteLine(
                        "Blast P1 : Connection not successful with no Request ID");
                    return false;
                }

                // submit request identifier and get the status
                ServiceRequestInformation reqInfo = blastService.GetRequestStatus(reqId);

                // Validate job status.
                if (reqInfo.Status != ServiceRequestStatus.Waiting
                    && reqInfo.Status != ServiceRequestStatus.Ready)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(
                    "Blast P1 : Connection not successful with error '{0}'", ex.Message));
                ApplicationLog.WriteLine(string.Format(
                    "Blast P1 : Connection not successful with error '{0}'", ex.Message));
                return false;
            }

            Console.WriteLine("Blast P1 : Connection Successful");
            return true;
        }

        #endregion Supporting Methods
    }
}