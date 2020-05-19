// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * BioHPCBlastBvtTestCases.cs
 * 
 * This file contains the BioHPC Blast Web Service BVT test cases.
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
using MBF.Web.BioHPC;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.TestUtils.SimulatorUtility;
using System.Runtime.Serialization.Formatters.Binary;

namespace MBF.TestAutomation.Web.BioHPCBlast
{
    /// <summary>
    /// Test Automation code for MBF BioHPC Blast Web Service 
    /// and BVT level validations.
    /// </summary>
    [TestClass]
    public class BioHPCBlastBvtTestCases
    {
        #region Enum

        /// <summary>
        /// Submit methods of BioHPC Blast Web Service
        /// </summary>
        enum RequestType
        {
            DnalstSubmit,
            ProteinlstSubmit,
            DnaStrSubmit,
            ProteinStrSubmit,
            FetchSyncUsingDnaSeq,
            FetchSyncUsingProteinSeq,
            FetchASyncUsingDnaSeq,
            FetchASyncUsingProteinSeq,
        }

        #endregion Enum

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\BioHPCTestConfigs.xml");

        private TestContext testContextInstance;

        private static TestCaseSimulator _TestCaseSimulator;

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

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other 
        /// settings needed for test
        /// </summary>
        static BioHPCBlastBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region BioHPCBlast Bvt TestCases

        /// <summary>
        /// Validate BioHPC Blast Web Service Constructor
        /// </summary>
        /// Input data : Valid alphabet and sequence
        /// OutPut data : Validate Request Id
        [TestMethod]
        [Priority(0)]
        public void ValidateBioHPCCtor()
        {
            // Gets the search query parameter and their values.
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode,
                Constants.AlphabetNameNode);
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode,
                Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode,
                Constants.DatabaseValue);
            string email = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode,
                Constants.EmailAdress);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode,
                Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode,
                Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode,
                Constants.ProgramParameter);
            string expect = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode, Constants.Expectparameter);
            string emailNotify = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode, Constants.EmailNotifyParameterNode);
            string jobName = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode, Constants.JobNameParameterNode);
            string expectValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode, Constants.ExpectNode);
            string emailNotifyValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode, Constants.EmailNotifyNode);
            string jobNameValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BioHPCAsynchronousResultsNode, Constants.JobNameNode);

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();
            IBlastServiceHandler service = null;
            object resultsObject = null;
            try
            {
                service = new BioHPCBlastHandler();
                ConfigParameters configParameters = new ConfigParameters();
                configParameters.EmailAddress = email;
                configParameters.Password = String.Empty;
                configParameters.UseBrowserProxy = true;
                service.Configuration = configParameters;

                // Add mandatory parameter values to search query parameters.
                queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
                queryParams.Add(queryProgramParameter, queryProgramValue);
                queryParams.Add(expect, expectValue);
                queryParams.Add(emailNotify, emailNotifyValue);
                queryParams.Add(jobName, jobNameValue);


                Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
                testCaseParms.Add(Constants.BlastParmsConst, queryParams);
                testCaseParms.Add(Constants.QuerySeqString, querySequence);
                testCaseParms.Add(Constants.AlphabetString, alphabetName);
                testCaseParms.Add(Constants.EmailString, email);

                // Get request Identifier
                TestCaseParameters parameters = new TestCaseParameters(
                     Constants.BioHPCRequestIdentifierForDnaSeqTest,
                     null, GetRequestIdentifier, testCaseParms);
                resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
                Assert.IsNotNull(resultsObject as string);
            }
            catch (Exception ex)
            {
                Assert.Fail();
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "BioHPC Blast Bvt : Connection not successful with error '{0}'",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "BioHPC Blast Bvt : Connection not successful with error '{0}'",
                    ex.Message));
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }
        }

        /// <summary>
        /// Validate fetching results asynchronous for DNA
        /// Input Data :Valid query sequence, email address, program and database value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAsyncDnaResultsFetch()
        {
            ValidateBioHPCBlastResultsFetch(
                Constants.BioHPCAsynchronousResultsForDnaNode,
                RequestType.FetchASyncUsingDnaSeq);
        }

        /// <summary>
        /// Validate fetching results Synchronous for DNA
        /// Input Data :Valid query sequence, email address, program and database value.
        /// Output Data : Validation of blast results by synchronous fetching.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSyncDnaResultsFetch()
        {
            ValidateBioHPCBlastResultsFetch(
                Constants.BioHPCAsynchronousResultsForDnaNode,
                 RequestType.FetchSyncUsingDnaSeq);
        }

        /// <summary>
        /// Validate fetching results asynchronous for Protein 
        /// Input Data :Valid query sequence, email address, program and database value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAsyncProteinResultsFetch()
        {
            ValidateBioHPCBlastResultsFetch(
                Constants.BioHPCAsynchronousResultsForProteinNode,
                RequestType.FetchASyncUsingProteinSeq);
        }

        /// <summary>
        /// Validate fetching results Synchronous for Protein
        /// Input Data :Valid query sequence, email address, program and database value.
        /// Output Data : Validation of blast results by synchronous fetching.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSyncProteinResultsFetch()
        {
            ValidateBioHPCBlastResultsFetch(
                Constants.BioHPCAsynchronousResultsForProteinNode,
                  RequestType.FetchSyncUsingProteinSeq);
        }

        /// <summary>
        /// Validate Request status returned from BioHPC Blast Web Service 
        /// by passing request Identifier for DNA  sequence.
        /// Input Data :Valid search query, Database value and program value and email adress.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBioHPCGetRequestStatusMethodForDna()
        {
            ValidateBioHPCGeneralGetRequestStatusMethod(
                Constants.BioHPCAsynchronousResultsForDnaNode, "Dna");
        }

        /// <summary>
        /// Validate Request status returned from BioHPC Blast Web Service
        /// by passing request Identifier for Protein  sequence.
        /// Input Data :Valid search query, Database value and program value and email adress.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBioHPCGetRequestStatusMethodForProtein()
        {
            ValidateBioHPCGeneralGetRequestStatusMethod(
                Constants.BioHPCAsynchronousResultsForProteinNode, "Protein");
        }

        /// <summary>
        /// Validate Cancelling Submitted request for Dna Sequence query.
        /// Input Data : Dna Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateCancelRequestForDnaSequence()
        {
            ValidateCancelSubmittedJob(
                Constants.BioHPCBlastDnaSequenceCancelParametersNode,
                RequestType.DnaStrSubmit);
        }

        /// <summary>
        /// Validate Cancelling Submitted request for Protein Sequence query.
        /// Input Data : Protein Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateCancelRequenstForProteinSequence()
        {
            ValidateCancelSubmittedJob(
                Constants.BioHPCAsynchronousResultsForProteinNode,
                RequestType.ProteinStrSubmit);
        }

        /// <summary>
        /// Validate Cancelling Submitted request for list of Dna Sequence query.
        /// Input Data : Dna Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSubmitRequestForDnaSequence()
        {
            ValidateCancelSubmittedJob(
                Constants.BioHPCBlastDnaSequenceCancelParametersNode,
                RequestType.DnalstSubmit);
        }

        /// <summary>
        /// Validate Cancelling Submitted request for list of Protein Sequence query.
        /// Input Data : Protein Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSubmitRequenstForProteinSequence()
        {
            ValidateCancelSubmittedJob(
                Constants.BioHPCBlastProteinSequenceCancelParametersNode,
                RequestType.ProteinlstSubmit);
        }

        /// <summary>
        /// Validate BioHPC Blast Web Service Properties.
        /// Input Data : Valid Config Parameter.
        /// Output Data : Validation of BioHPC Blast Web Service properties.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBioHPCBlastWebServiceProperties()
        {
            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;

            BioHPCBlastHandler service = null;
            try
            {
                service = new BioHPCBlastHandler(configParams);

                // Validate BioHPC Blast Web Service properties.
                Assert.AreEqual(Constants.BioHPCWebServiceDescription,
                    service.Description);
                Assert.AreEqual(Constants.BioHPCWebServiceName,
                    service.Name);
                Assert.IsNotNull(service.Configuration);
                Assert.IsNotNull(service.Parser);

                ApplicationLog.WriteLine(
                    "BioHPC Blast Bvt : Successfully validated the BioHPC Blast WebService Properties");
                Console.WriteLine(
                    "BioHPC Blast Bvt : Successfully validated the BioHPC Blast WebService Properties");
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }
        }

        #endregion BioHPCBlast Bvt TestCases

        #region Supported Methods

        /// <summary>
        /// Validate general fetching results.by passing 
        /// differnt parameters for BioHPC Blast web service.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="isFetchSynchronous">Is Fetch Synchronous?</param>
        /// </summary>
        void ValidateBioHPCBlastResultsFetch(
            string nodeName,
            RequestType reqType)
        {
            // Gets the search query parameter and their values.
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseValue);
            string email = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EmailAdress);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName,
                Constants.ProgramValue);
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
            string expectedEntropyStatistics =
                _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EntropyStatistics);
            string expectedKappaStatistics = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.KappaStatistics);
            string expectedLambdaStatistics = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.LambdaStatistics);
            string expectedLength = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Length);
            int maxAttempts = int.Parse(_utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.MaxAttemptsNode), (IFormatProvider)null);
            int waitingTime = int.Parse(_utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.WaitTimeNode), (IFormatProvider)null);
            string expect = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Expectparameter);
            string emailNotify = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EmailNotifyParameterNode);
            string jobName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.JobNameParameterNode);
            string expectValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectNode);
            string emailNotifyValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EmailNotifyNode);
            string jobNameValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.JobNameNode);

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();

            // Add mandatory parameter values to search query parameters.
            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);
            queryParams.Add(expect, expectValue);
            queryParams.Add(emailNotify, emailNotifyValue);
            queryParams.Add(jobName, jobNameValue);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);
            testCaseParms.Add(Constants.AlphabetString, alphabetName);
            testCaseParms.Add(Constants.EmailString, email);
            testCaseParms.Add(Constants.MaxAttemptString, maxAttempts.ToString((IFormatProvider)null));
            testCaseParms.Add(Constants.WaitTimeString, waitingTime);

            TestCaseParameters parameters = null;
            object resultsObject = null;

            switch (reqType)
            {
                case RequestType.FetchSyncUsingDnaSeq:
                    parameters = new TestCaseParameters(
                        Constants.BioHPCFetchResultsSyncTest,
                        null, FetchResultsSyncTest, testCaseParms);
                    resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
                    break;
                case RequestType.FetchASyncUsingDnaSeq:
                    parameters = new TestCaseParameters(
                        Constants.BioHPCFetchResultsASyncTest,
                        null, FetchResultsASyncTest, testCaseParms);
                    resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
                    break;
                case RequestType.FetchSyncUsingProteinSeq:
                    parameters = new TestCaseParameters(
                       Constants.FetchResultsSyncUsingProteinSeq,
                       null, FetchResultsSyncTest, testCaseParms);
                    resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
                    break;
                case RequestType.FetchASyncUsingProteinSeq:
                    parameters = new TestCaseParameters(
                        Constants.FetchResultsUsingProteinSeq,
                        null, FetchResultsASyncTest, testCaseParms);
                    resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
                    break;
            }

            // Validate blast results.
            Assert.IsNotNull(resultsObject);
            List<BlastResult> bioHPCResults = resultsObject as List<BlastResult>;
            Assert.IsNotNull(bioHPCResults);
            Assert.AreEqual(bioHPCResults.Count.ToString(
                (IFormatProvider)null), expectedResultCount);
            Assert.AreEqual(bioHPCResults[0].Records.Count.ToString((
                IFormatProvider)null), expectedResultCount);
            BlastSearchRecord record = bioHPCResults[0].Records[0];
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
            Assert.AreEqual(hit.Length.ToString((IFormatProvider)null),
                expectedLength);
            Assert.AreEqual(hit.Id.ToString((IFormatProvider)null),
                expectedHitId);
            Assert.AreEqual(hit.Hsps.Count.ToString((IFormatProvider)null),
                expectedResultCount);
            Console.WriteLine(string.Format((IFormatProvider)null,
               "BioHPC Blast BVT : Hits count '{0}'.", bioHPCResults.Count));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "BioHPC Blast BVT : Accession '{0}'.", hit.Accession));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "BioHPC Blast BVT : Hit Id '{0}'.", hit.Id));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "BioHPC Blast BVT : Hits Count '{0}'.", hit.Hsps.Count));
        }

        /// <summary>
        /// Validate general http request status by
        /// differnt parameters for BioHPC Blast Web Service.
        /// <param name="nodeName">different alphabet node name</param>
        /// </summary>
        void ValidateBioHPCGeneralGetRequestStatusMethod(string nodeName,
            string moleculeType)
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
            string email = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EmailAdress);
            string expect = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Expectparameter);
            string emailNotify = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EmailNotifyParameterNode);
            string jobName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.JobNameParameterNode);
            string expectValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectNode);
            string emailNotifyValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EmailNotifyNode);
            string jobNameValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.JobNameNode);

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();

            // Add mandatory parameter values to search query parameters.
            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);
            queryParams.Add(expect, expectValue);
            queryParams.Add(emailNotify, emailNotifyValue);
            queryParams.Add(jobName, jobNameValue);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);
            testCaseParms.Add(Constants.AlphabetString, alphabetName);
            testCaseParms.Add(Constants.EmailString, email);

            TestCaseParameters parameters = null;
            object resultsObject = null;

            if ("Dna" == moleculeType)
            {
                parameters = new TestCaseParameters(
                    Constants.BioHPCGetRequestStatusInfoForDnaSeqTest,
                    null, GetRequestStatus, testCaseParms);
                resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
            }
            else
            {
                parameters = new TestCaseParameters(
                   Constants.BioHPCGetRequestStatusInfoForProteinSeqTest,
                   null, GetRequestStatus, testCaseParms);
                resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
            }

            // Get service request status information.
            ServiceRequestInformation reqInfo = resultsObject as ServiceRequestInformation;


            // Validate job status.
            if (reqInfo.Status != ServiceRequestStatus.Waiting
                && reqInfo.Status != ServiceRequestStatus.Ready
                && reqInfo.Status != ServiceRequestStatus.Queued)
            {
                string error = ApplicationLog.WriteLine(string.Format(
                (IFormatProvider)null, "Unexpected error", reqInfo.Status));
                Assert.Fail(error);
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Unexpected error", reqInfo.Status));
            }
            else
            {
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Request status {0} ", reqInfo.Status));
            }
        }

        /// <summary>
        /// Validate Cancel submitted job by passing job id.
        /// <param name="nodeName">different alphabet node name</param>
        /// </summary>
        void ValidateCancelSubmittedJob(
            string nodeName,
            RequestType type)
        {
            // Gets the search query parameter and their values.
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseValue);
            string email = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EmailAdress);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ProgramParameter);
            string expect = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Expectparameter);
            string emailNotify = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EmailNotifyParameterNode);
            string jobName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.JobNameParameterNode);
            string expectValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectNode);
            string emailNotifyValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.EmailNotifyNode);
            string jobNameValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.JobNameNode);

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();

            IBlastServiceHandler service = new BioHPCBlastHandler();
            ConfigParameters configParameters = new ConfigParameters();
            configParameters.EmailAddress = email;
            configParameters.Password = String.Empty;
            configParameters.UseBrowserProxy = true;
            service.Configuration = configParameters;

            // Add mandatory parameter values to search query parameters.
            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);
            queryParams.Add(expect, expectValue);
            queryParams.Add(emailNotify, emailNotifyValue);
            queryParams.Add(jobName, jobNameValue);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);
            testCaseParms.Add(Constants.AlphabetString, alphabetName);
            testCaseParms.Add(Constants.EmailString, email);

            TestCaseParameters parameters = null;
            object resultsObject = null;

            // Create a request without passing sequence.
            switch (type)
            {
                case RequestType.DnaStrSubmit:
                    parameters = new TestCaseParameters(
                        Constants.BioHPCCancelRequestUsingDnaSeq,
                        null, GetRequestIdentifier, testCaseParms);
                    resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
                    break;
                case RequestType.ProteinStrSubmit:
                    parameters = new TestCaseParameters(
                        Constants.BioHPCRequestIdentifierForProteinSeqTest,
                        null, GetRequestIdentifier, testCaseParms);
                    resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
                    break;
                case RequestType.DnalstSubmit:
                    parameters = new TestCaseParameters(
                       Constants.BioHPCRequestIdentifierForDnaSeqTest,
                       null, GetRequestIdentifierUsingSeqList, testCaseParms);
                    resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
                    break;
                case RequestType.ProteinlstSubmit:
                    parameters = new TestCaseParameters(
                       Constants.BioHPCCancelRequestUsingProteinSeq,
                       null, GetRequestIdentifierUsingSeqList, testCaseParms);
                    resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
                    break;
            }

            // Cancel subitted job.
            bool result = service.CancelRequest(resultsObject as string);

            // validate the cancelled job.
            Assert.IsTrue(result);

            Console.WriteLine(string.Format(null,
                "BioHPC Blast Bvt : Submitted job cancelled was successfully.",
                queryProgramValue));
        }

        /// <summary>
        /// Get BioHPC web service request status.
        /// </summary>
        /// <param name="blastParameters">Blast config parameters</param>
        /// <returns>BioHPC webservice request status</returns>
        private TestCaseOutput GetRequestStatus(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            string alphabetName = blastParameters[Constants.AlphabetString] as string;
            string email = blastParameters[Constants.EmailString] as string;
            Sequence sequence = new Sequence(Utility.GetAlphabet(alphabetName),
                sequenceString);

            //Set BioHPC web service config parameters
            IBlastServiceHandler service = null;
            object serviceInfo = null;
            try
            {
                service = new BioHPCBlastHandler();
                ConfigParameters configParameters = new ConfigParameters();
                configParameters.EmailAddress = email;
                configParameters.Password = String.Empty;
                configParameters.UseBrowserProxy = true;
                service.Configuration = configParameters;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Submit request and get request identifier
                string reqId = service.SubmitRequest(sequence, blastSearchPams);

                // Get service request status.

                serviceInfo = service.GetRequestStatus(reqId);
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }

            return new TestCaseOutput(serviceInfo, false);
        }

        /// <summary>
        /// Get BioHPC web service request identifier for Sequence List
        /// </summary>
        /// <param name="blastParameters">Blast config parameters</param>
        /// <returns>BioHPC webservice request identifier</returns>
        private TestCaseOutput GetRequestIdentifier(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            string alphabetName = blastParameters[Constants.AlphabetString] as string;
            string email = blastParameters[Constants.EmailString] as string;
            Sequence sequence = new Sequence(Utility.GetAlphabet(alphabetName),
                sequenceString);

            //Set BioHPC web service config parameters
            IBlastServiceHandler service = null;
            object reqId = null;
            try
            {
                service = new BioHPCBlastHandler();
                ConfigParameters configParameters = new ConfigParameters();
                configParameters.EmailAddress = email;
                configParameters.Password = String.Empty;
                configParameters.UseBrowserProxy = true;
                service.Configuration = configParameters;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Submit request and get request identifier
                reqId = service.SubmitRequest(sequence, blastSearchPams);
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }

            return new TestCaseOutput(reqId, false);
        }

        /// <summary>
        /// Get BioHPC web service request identifier
        /// </summary>
        /// <param name="blastParameters">Blast config parameters</param>
        /// <returns>BioHPC webservice request identifier</returns>
        private TestCaseOutput GetRequestIdentifierUsingSeqList(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            string alphabetName = blastParameters[Constants.AlphabetString] as string;
            string email = blastParameters[Constants.EmailString] as string;
            Sequence sequence = new Sequence(Utility.GetAlphabet(alphabetName),
                sequenceString);

            //Set BioHPC web service config parameters
            IBlastServiceHandler service = null;
            object reqId = null;
            try
            {
                service = new BioHPCBlastHandler();
                ConfigParameters configParameters = new ConfigParameters();
                configParameters.EmailAddress = email;
                configParameters.Password = String.Empty;
                configParameters.UseBrowserProxy = true;
                service.Configuration = configParameters;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Submit request with Sequence List and get request identifier
                IList<ISequence> lstSeq = new List<ISequence>();
                lstSeq.Add(sequence);

                reqId = service.SubmitRequest(lstSeq, blastSearchPams);
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }

            return new TestCaseOutput(reqId, false);
        }

        /// <summary>
        /// Fetch synchronous results
        /// </summary>
        /// <param name="blastParameters">Blast config parameters</param>
        /// <returns>BioHPC Web service results</returns>
        private TestCaseOutput FetchResultsSyncTest(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            string alphabetName = blastParameters[Constants.AlphabetString] as string;
            string email = blastParameters[Constants.EmailString] as string;
            Sequence sequence = new Sequence(Utility.GetAlphabet(alphabetName),
                sequenceString);
            string maxAttempts = blastParameters[Constants.MaxAttemptString] as string;
            string waitTime = blastParameters[Constants.WaitTimeString] as string;

            //Set BioHPC web service config parameters
            IBlastServiceHandler service = null;
            IList<BlastResult> syncBlastResults = null;
            try
            {
                service = new BioHPCBlastHandler();
                ConfigParameters configParameters = new ConfigParameters();
                configParameters.EmailAddress = email;
                configParameters.Password = String.Empty;
                configParameters.UseBrowserProxy = true;
                service.Configuration = configParameters;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Submit request and get request identifier
                string reqId = service.SubmitRequest(sequence, blastSearchPams);

                // validate request identifier.
                Assert.IsNotNull(reqId);

                ServiceRequestInformation info = service.GetRequestStatus(reqId);
                if (info.Status != ServiceRequestStatus.Waiting
                    && info.Status != ServiceRequestStatus.Ready
                    && info.Status != ServiceRequestStatus.Queued)
                {
                    string err =
                        ApplicationLog.WriteLine("Unexpected status: '{0}'",
                        info.Status);
                    Assert.Fail(err);
                }

                // get async results, poll until ready
                int attempt = 1;
                while (attempt <= Int32.Parse(maxAttempts, (IFormatProvider)null)
                        && info.Status != ServiceRequestStatus.Error
                        && info.Status != ServiceRequestStatus.Ready)
                {
                    ++attempt;

                    info = service.GetRequestStatus(reqId);
                    Thread.Sleep(
                        info.Status == ServiceRequestStatus.Waiting
                        || info.Status == ServiceRequestStatus.Queued
                        ? Int32.Parse(waitTime, (IFormatProvider)null) * attempt : 0);
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
        /// Fetch Asynchronous results
        /// </summary>
        /// <param name="blastParameters">Blast config parameters</param>
        /// <returns>BioHPC Web service results</returns>
        private TestCaseOutput FetchResultsASyncTest(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            string alphabetName = blastParameters[Constants.AlphabetString] as string;
            string email = blastParameters[Constants.EmailString] as string;
            Sequence sequence = new Sequence(Utility.GetAlphabet(alphabetName),
                sequenceString);
            string maxAttempts = blastParameters[Constants.MaxAttemptString] as string;
            string waitTime = blastParameters[Constants.WaitTimeString] as string;

            //Set BioHPC web service config parameters
            IBlastServiceHandler service = null;
            object responseResults = null;
            try
            {
                service = new BioHPCBlastHandler();
                ConfigParameters configParameters = new ConfigParameters();
                configParameters.EmailAddress = email;
                configParameters.Password = String.Empty;
                configParameters.UseBrowserProxy = true;
                service.Configuration = configParameters;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Submit request and get request identifier
                string reqId = service.SubmitRequest(sequence, blastSearchPams);

                // validate request identifier.
                Assert.IsNotNull(reqId);

                ServiceRequestInformation info = service.GetRequestStatus(reqId);
                if (info.Status != ServiceRequestStatus.Waiting
                    && info.Status != ServiceRequestStatus.Ready
                    && info.Status != ServiceRequestStatus.Queued)
                {
                    string err =
                        ApplicationLog.WriteLine("Unexpected status: '{0}'",
                        info.Status);
                    Assert.Fail(err);
                }

                // get async results, poll until ready
                int attempt = 1;
                while (attempt <= Int32.Parse(maxAttempts, (IFormatProvider)null)
                        && info.Status != ServiceRequestStatus.Error
                        && info.Status != ServiceRequestStatus.Ready)
                {
                    ++attempt;

                    info = service.GetRequestStatus(reqId);
                    Thread.Sleep(
                      info.Status == ServiceRequestStatus.Waiting
                      || info.Status == ServiceRequestStatus.Queued
                      ? Int32.Parse(waitTime, (IFormatProvider)null) * attempt : 0);
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
        #endregion Supported Methods
    }
}
