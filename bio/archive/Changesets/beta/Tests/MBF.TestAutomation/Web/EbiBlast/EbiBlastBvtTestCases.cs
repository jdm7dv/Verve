﻿// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * EbiBlastBvtTestCases.cs
 * 
 * This file contains the Ebi Blast Web Service BVT test cases.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using MBF.Web;
using MBF.Web.Blast;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.TestUtils.SimulatorUtility;

namespace MBF.TestAutomation.Web.EbiBlast
{
    /// <summary>
    /// Test Automation code for MBF Ebi Blast Web Service and BVT level validations.
    /// </summary>
    [TestClass]
    public class EbiBlastBvtTestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        private TestContext testContextInstance;

        private static TestCaseSimulator _TestCaseSimulator;

        #endregion Global Variables
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        /// Test Case initialization used by the Test Class
        /// </summary>
        /// <param name="testContext"></param>
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            _TestCaseSimulator = new TestCaseSimulator();
        }

        /// <summary>
        /// Free the resources used after the test cases have run
        /// </summary>
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            _TestCaseSimulator = null;
        }

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static EbiBlastBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region EbiBlast Bvt TestCases

        /// <summary>
        /// Validate fetching results asynchronous 
        /// Input Data :Valid query sequence, email address, program and database value.
        /// Output Data : Validation of Eblast results by asynchronous fetching.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateAsyncResultsFetch()
        {
            ValidateEBIWuBlastResultsFetch(
                Constants.EbiAsynchronousResultsNode, false);
        }

        /// <summary>
        /// Validate fetching results Synchronous 
        /// Input Data :Valid query sequence, email address, program and database value.
        /// Output Data : Validation of Eblast results by synchronous fetching.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSyncResultsFetch()
        {
            ValidateEBIWuBlastResultsFetch(
                Constants.EbiSynchronousResults, true);
        }

        /// <summary>
        /// Validate Request status returned from Ebi web service by passing request 
        /// Identifier for DNA  sequence.
        /// Input Data :Valid search query, Database value and program value and email adress.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateEbiGetRequestStatusMethodForDna()
        {
            ValidateEbiGeneralGetRequestStatusMethod(
                Constants.EBlastDnaSequenceParameters, "Dna");
        }

        /// <summary>
        /// Validate Request status returned from Ebi web service by passing request Identifier 
        /// for Protein  sequence.
        /// Input Data :Valid search query, Database value and program value and email adress.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateEbiGetRequestStatusMethodForProtein()
        {
            ValidateEbiGeneralGetRequestStatusMethod(
                Constants.EbiBlastParametersNode, "Protein");
        }

        /// <summary>
        /// Validate Ebi blast result by passing search Query as parameter.
        /// Input Data :Valid search query, Database value,Email address and program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void EBIWuBlastResultsWithQueryParams()
        {
            // Gets the search query parameter and their values.
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.AlphabetNameNode);
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.DatabaseValue);
            string emailParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.Emailparameter);
            string email = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.EmailAdress);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.ProgramParameter);
            string expectedHitId = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.HitID);
            string expectedAccession = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.HitAccession);
            string expectedResultCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.ResultsCount);
            string expectedHitsCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.HitsCount);
            string expectedEntropyStatistics = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.EntropyStatistics);
            string expectedKappaStatistics = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.KappaStatistics);
            string expectedLambdaStatistics = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.LambdaStatistics);
            string expectedLength = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.Length);

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();
            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);
            queryParams.Add(emailParameter, email);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);
            testCaseParms.Add(Constants.AlphabetString, alphabetName);

            TestCaseParameters parameters = null;
            object resultsObject = null;

            parameters = new TestCaseParameters(Constants.EBiFetchResultASyncTestNode, null,
                    FetchResultsASync, testCaseParms);
            resultsObject = _TestCaseSimulator.Simulate(parameters).Result;

            // Validate blast results.
            Assert.IsNotNull(resultsObject);
            List<BlastResult> eBlastResults = resultsObject as List<BlastResult>;

            Assert.AreEqual(eBlastResults.Count.ToString(
                (IFormatProvider)null), expectedResultCount);
            Assert.AreEqual(eBlastResults[0].Records.Count.ToString((
                IFormatProvider)null), expectedResultCount);
            BlastSearchRecord record = eBlastResults[0].Records[0];
            Assert.AreEqual(record.Statistics.Kappa.ToString(
                (IFormatProvider)null), expectedKappaStatistics);
            Assert.AreEqual(record.Statistics.Lambda.ToString(
                (IFormatProvider)null), expectedLambdaStatistics);
            Assert.AreEqual(record.Statistics.Entropy.ToString(
                (IFormatProvider)null), expectedEntropyStatistics);
            Assert.AreEqual(record.Hits.Count.ToString(
                (IFormatProvider)null), expectedHitsCount);
            Hit hit = record.Hits[0];
            Assert.AreEqual(hit.Accession, expectedAccession);
            Assert.AreEqual(hit.Length.ToString((IFormatProvider)null), expectedLength);
            Assert.AreEqual(hit.Id.ToString((IFormatProvider)null), expectedHitId);
            Assert.AreEqual(hit.Hsps.Count.ToString((IFormatProvider)null),
                expectedResultCount);
            Console.WriteLine(string.Format((IFormatProvider)null,
               "Ebi Blast BVT: Hits count '{0}'.", eBlastResults.Count));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Ebi Blast BVT: Accession '{0}'.", hit.Accession));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Ebi Blast BVT: Hit Id '{0}'.", hit.Id));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Ebi Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));
        }

        /// <summary>
        /// Validate valid Ebi blast mandatory parameters.
        /// Input data : Valid ebi parameters..
        /// Ouptut data : Validation of mandatory Ebi paramters.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateEBIWuBlastManadatoryParams()
        {
            // Gets the search query parameter and their values.
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.DatabaseValue);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.ProgramParameter);
            string emailParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.Emailparameter);
            string email = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.EmailAdress);

            // Set Service confiruration parameters true.
            IBlastServiceHandler service = null;
            try
            {
                service = new EbiWuBlastHandler();
                ConfigParameters configParameters = new ConfigParameters();
                configParameters.UseBrowserProxy = true;
                service.Configuration = configParameters;

                // Create search parameters object.
                BlastParameters ebiParams = new BlastParameters();

                // Add mandatory parameter values to search query parameters.
                //ebiParams.Add(querySequenceParameter, querySequence);
                ebiParams.Add(queryDatabaseParameter, queryDatabaseValue);
                ebiParams.Add(queryProgramParameter, queryProgramValue);
                ebiParams.Add(emailParameter, email);

                // Validate search query parameters.
                ParameterValidationResult validateParameters =
                    EbiWuBlastHandler.ValidateParameters(ebiParams);
                bool result = validateParameters.IsValid;

                Assert.IsTrue(result);
                // Assert.IsTrue(ebiParams.Settings.ContainsValue(querySequence));
                Assert.IsTrue(ebiParams.Settings.ContainsValue(queryDatabaseValue));
                Assert.IsTrue(ebiParams.Settings.ContainsValue(queryProgramValue));
                Assert.IsTrue(ebiParams.Settings.ContainsValue(email));
                Assert.AreEqual(ebiParams.Settings.Count, 3);

                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format((IFormatProvider)null,
                 "Ebi Blast BVT: Query Sequence{0} is as expected.", querySequence));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Ebi Blast BVT: DataBase Value{0} is as expected.", queryDatabaseValue));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Ebi Blast BVT: Program Value {0} is as expected.", queryProgramValue));
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }
        }

        /// <summary>
        /// Validate Cancelling Submitted request for Dna Sequence query.
        /// Input Data : Dna Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateCancelDnaQuerySequenceRequest()
        {
            ValidateCancelSubmittedJob(Constants.EbiDnaSeqAsynchronousResultsNode);
        }

        /// <summary>
        /// Validate Cancelling Submitted request for Medium sized Dna Sequence query.
        /// Input Data : Medium sized Dna Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateCancelRequestForMediumSizedDnaSequence()
        {
            ValidateCancelSubmittedJob(Constants.EbiBlastMediumSizeEbiDnaSequenceParametersNode);
        }

        /// <summary>
        /// Validate Cancelling Submitted request for Medium sized Protein Sequence query.
        /// Input Data : Medium sized Protein Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateCancelProteinQuerySequenceRequest()
        {
            ValidateCancelSubmittedJob(
                Constants.EbiBlastMediumSizeEbiProteinSequenceParametersNode);
        }

        /// <summary>
        /// Validate EBI Webservice Properties.
        /// Input Data : Valid Config Parameter.
        /// Output Data : Validation of Ebi Service properties.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateEbiWebServiceProperties()
        {
            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;

            IBlastServiceHandler service = null;
            try
            {
                service = new EbiWuBlastHandler(configParams);

                // Validate EBI Web Service properties.
                Assert.AreEqual(Constants.EbiWebServiceDescription, service.Description);
                Assert.AreEqual(Constants.EbiWebServiceName, service.Name);

                ApplicationLog.WriteLine(
                    "EbiWebService : Successfully validated the Ebi WebService Properties");
                Console.WriteLine(
                    "EbiWebService : Successfully validated the Ebi WebService Properties");
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }
        }

        #endregion EbiBlast Bvt TestCases

        #region Supported Methods

        /// <summary>
        /// Validate general fetching results.by passing 
        /// differnt parameters for Ebi web service..
        /// <param name="nodeName">xml node name.</param>
        /// <param name="isFetchSynchronous">Is Fetch Synchronous?</param>
        /// </summary>
        void ValidateEBIWuBlastResultsFetch(
            string nodeName,
            bool isFetchSynchronous)
        {
            // Gets the search query parameter and their values.
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseValue);
            string emailParameter = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Emailparameter);
            string email = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EmailAdress);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiAsynchronousResultsNode, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ProgramParameter);
            string expectedHitId = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.HitID);
            string expectedAccession = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.HitAccession);
            string expectedResultCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ResultsCount);
            string expectedHitsCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.HitsCount);
            string expectedEntropyStatistics = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EntropyStatistics);
            string expectedKappaStatistics = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.KappaStatistics);
            string expectedLambdaStatistics = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.LambdaStatistics);
            string expectedLength = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Length);

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();
            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);
            queryParams.Add(emailParameter, email);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);
            testCaseParms.Add(Constants.AlphabetString, alphabetName);

            TestCaseParameters parameters = null;
            object resultsObject = null;

            if (isFetchSynchronous)
            {
                parameters = new TestCaseParameters(Constants.EBiFetchResultSyncTestNode, null,
                    FetchResultsSync, testCaseParms);
                resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
            }
            else
            {
                parameters = new TestCaseParameters(Constants.EBiFetchResultASyncTestNode, null,
                    FetchResultsASync, testCaseParms);
                resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
            }

            // Validate blast results.
            Assert.IsNotNull(resultsObject);
            List<BlastResult> eBlastResults = resultsObject as List<BlastResult>;

            Assert.AreEqual(eBlastResults.Count.ToString(
                (IFormatProvider)null), expectedResultCount);
            Assert.AreEqual(eBlastResults[0].Records.Count.ToString((
                IFormatProvider)null), expectedResultCount);
            BlastSearchRecord record = eBlastResults[0].Records[0];
            Assert.AreEqual(record.Statistics.Kappa.ToString(
                (IFormatProvider)null), expectedKappaStatistics);
            Assert.AreEqual(record.Statistics.Lambda.ToString(
                (IFormatProvider)null), expectedLambdaStatistics);
            Assert.AreEqual(record.Statistics.Entropy.ToString(
                (IFormatProvider)null), expectedEntropyStatistics);
            Assert.AreEqual(record.Hits.Count.ToString(
                (IFormatProvider)null), expectedHitsCount);
            Hit hit = record.Hits[0];
            Assert.AreEqual(hit.Accession, expectedAccession);
            Assert.AreEqual(hit.Length.ToString((IFormatProvider)null), expectedLength);
            Assert.AreEqual(hit.Id.ToString((IFormatProvider)null), expectedHitId);
            Assert.AreEqual(hit.Hsps.Count.ToString((IFormatProvider)null),
                expectedResultCount);
            Console.WriteLine(string.Format((IFormatProvider)null,
               "Ebi Blast BVT: Hits count '{0}'.", eBlastResults.Count));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Ebi Blast BVT: Accession '{0}'.", hit.Accession));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Ebi Blast BVT: Hit Id '{0}'.", hit.Id));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Ebi Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));
        }

        /// <summary>
        /// Validate general http request status by
        /// differnt parameters for Ebi web service..
        /// <param name="nodeName">different alphabet node name</param>
        /// </summary>
        void ValidateEbiGeneralGetRequestStatusMethod(string nodeName,
            string alphabet)
        {
            // Gets the search query parameter and their values.
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseValue);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ProgramParameter);
            string emailParameter = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Emailparameter);
            string email = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EmailAdress);

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();


            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);
            queryParams.Add(emailParameter, email);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);
            testCaseParms.Add(Constants.AlphabetString, alphabetName);

            TestCaseParameters parameters = null;
            object resultsObject = null;

            if (alphabet == "Protein")
            {
                parameters = parameters = new TestCaseParameters(Constants.EbiGetRequestStatusInfoForProteinSeqTest,
                    null, GetRequestStatusTest, testCaseParms);
                resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
            }
            else
            {
                parameters = parameters = new TestCaseParameters(
                    Constants.EbiGetRequestStatusInfoForDnaSeqNode, null,
                    GetRequestStatusTest, testCaseParms);
                resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
            }

            // submit request identifier and get the status
            ServiceRequestInformation reqInfo = resultsObject as ServiceRequestInformation;

            // Validate job status.
            if (reqInfo.Status != ServiceRequestStatus.Waiting
                && reqInfo.Status != ServiceRequestStatus.Ready)
            {
                string error = ApplicationLog.WriteLine(
                    string.Concat("Unexpected error", reqInfo.Status));
                Assert.Fail(error);
                Console.WriteLine(string.Concat(
                    "Unexpected error ", reqInfo.Status));
            }
            else
            {
                Console.WriteLine(string.Concat(
                    "Request status ", reqInfo.Status));
            }
        }

        /// <summary>
        /// Validate Cancel submitted job by passing job id.
        /// <param name="nodeName">different alphabet node name</param>
        /// </summary>
        void ValidateCancelSubmittedJob(string nodeName)
        {
            // Gets the search query parameter and their values.
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseValue);
            string emailParameter = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Emailparameter);
            string email = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EmailAdress);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ProgramParameter);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName),
                querySequence);

            // create Ebi Blast service object.
            IBlastServiceHandler service = null;
            try
            {
                service = new EbiWuBlastHandler();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                service.Configuration = configParams;

                BlastParameters searchParams = new BlastParameters();

                // Set Request parameters.
                searchParams.Add(queryDatabaseParameter, queryDatabaseValue);
                searchParams.Add(queryProgramParameter, queryProgramValue);
                searchParams.Add(emailParameter, email);

                // Create a request without passing sequence.
                string reqId = service.SubmitRequest(seq, searchParams);

                // Cancel subitted job.
                bool result = service.CancelRequest(reqId);

                // validate the cancelled job.
                Assert.IsTrue(result);

                Console.WriteLine(string.Concat(
                    "EBI Blast P1 : Submitted job cancelled was successfully.",
                    queryProgramValue));
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }

        }

        /// <summary>
        /// Validate Fetch results synchronous.
        /// </summary>
        /// <param name="blastParameters">Ebi blast service config parameters</param>
        /// <returns></returns>
        private TestCaseOutput FetchResultsASync(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            string alphabetName = blastParameters[Constants.AlphabetString] as string;
            Sequence sequence = new Sequence(Utility.GetAlphabet(alphabetName),
                sequenceString);

            // create Ebi Blast service object.
            object responseResults = null;
            IBlastServiceHandler service = null;
            try
            {
                service = new EbiWuBlastHandler();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                service.Configuration = configParams;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Create a request without passing sequence.
                string reqId = service.SubmitRequest(sequence, blastSearchPams);

                // validate request identifier.
                Assert.IsNotNull(reqId);

                // query the status
                ServiceRequestInformation info = service.GetRequestStatus(reqId);
                if (info.Status != ServiceRequestStatus.Waiting
                    && info.Status != ServiceRequestStatus.Ready)
                {
                    string err =
                        ApplicationLog.WriteLine("Unexpected status: '{0}'", info.Status);
                    Assert.Fail(err);
                }

                // get async results, poll until ready
                int maxAttempts = 3;
                int attempt = 1;
                while (attempt <= maxAttempts
                        && info.Status != ServiceRequestStatus.Error
                        && info.Status != ServiceRequestStatus.Ready)
                {
                    info = service.GetRequestStatus(reqId);
                    Thread.Sleep(info.Status == ServiceRequestStatus.Waiting
                        || info.Status == ServiceRequestStatus.Queued
                        ? 20000 * attempt : 0);

                    ++attempt;
                }

                IBlastParser blastXmlParser = new BlastXmlParser();
                using (StringReader reader = new StringReader(service.GetResult(reqId, blastSearchPams)))
                {
                    responseResults = blastXmlParser.Parse(reader);
                }
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }
            return new TestCaseOutput(responseResults, false);
        }

        /// <summary>
        /// Validate Fetch results Asynchronous.
        /// </summary>
        /// <param name="blastParameters">Ebi blast service config parameters</param>
        /// <returns></returns>
        private TestCaseOutput FetchResultsSync(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            string alphabetName = blastParameters[Constants.AlphabetString] as string;
            Sequence sequence = new Sequence(Utility.GetAlphabet(alphabetName),
                sequenceString);

            // create Ebi Blast service object.
            IBlastServiceHandler service = null;
            IList<BlastResult> syncBlastResults = null;
            try
            {
                service = new EbiWuBlastHandler();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                service.Configuration = configParams;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Create a request without passing sequence.
                string reqId = service.SubmitRequest(sequence, blastSearchPams);

                // validate request identifier.
                Assert.IsNotNull(reqId);

                // query the status
                ServiceRequestInformation info = service.GetRequestStatus(reqId);
                if (info.Status != ServiceRequestStatus.Waiting
                    && info.Status != ServiceRequestStatus.Ready)
                {
                    string err =
                        ApplicationLog.WriteLine("Unexpected status: '{0}'", info.Status);
                    Assert.Fail(err);
                }

                // get async results, poll until ready
                int maxAttempts = 3;
                int attempt = 1;
                while (attempt <= maxAttempts
                        && info.Status != ServiceRequestStatus.Error
                        && info.Status != ServiceRequestStatus.Ready)
                {
                    info = service.GetRequestStatus(reqId);
                    Thread.Sleep(info.Status == ServiceRequestStatus.Waiting
                        || info.Status == ServiceRequestStatus.Queued
                        ? 20000 * attempt : 0);

                    ++attempt;
                }

                syncBlastResults =
                        service.FetchResultsSync(reqId, blastSearchPams) as List<BlastResult>;
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }
            return new TestCaseOutput(syncBlastResults, false);
        }

        /// <summary>
        /// Validate Request status of the submited request.
        /// </summary>
        /// <param name="blastParameters">Ebi blast service config parameters</param>
        /// <returns></returns>
        private TestCaseOutput GetRequestStatusTest(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            string alphabetName = blastParameters[Constants.AlphabetString] as string;
            Sequence sequence = new Sequence(Utility.GetAlphabet(alphabetName),
                sequenceString);

            // create Ebi Blast service object.
            IBlastServiceHandler service = null;
            object reqInfoObject = null;
            try
            {
                service = new EbiWuBlastHandler();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                service.Configuration = configParams;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Create a request without passing sequence.
                string reqId = service.SubmitRequest(sequence, blastSearchPams);

                // validate request identifier.
                Assert.IsNotNull(reqId);

                // query the status
                reqInfoObject = service.GetRequestStatus(reqId);
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }

            return new TestCaseOutput(reqInfoObject, false);
        }
        #endregion Supported Methods
    }
}