// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * AzureBlastBvtTestCases.cs
 * 
 * This file contains the Azure Blast Web Service BVT test cases.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using MBF.Web;
using MBF.Web.Blast;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.TestUtils.SimulatorUtility;

namespace MBF.TestAutomation.Web.AzureBlast
{
    /// <summary>
    /// Test Automation code for MBF Azure Blast Web Service and BVT level validations.
    /// </summary>
    [TestClass]
    public class AzureBlastBvtTestCases
    {
        #region Enum

        /// <summary>
        /// Submit methods of Azure Web Service
        /// </summary>
        enum RequestType
        {
            FetchSyncUsingDnaSeq,
            FetchSyncUsingProteinSeq,
            FetchASyncUsingDnaSeq,
        }

        #endregion Enum

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\AzureBlastConfig.xml");

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

        #endregion Global Variables

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
        static AzureBlastBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Azure Blast BVT TestCases

        /// <summary>
        /// Validate a Add() method with mandatory values.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of Add() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateAddMethodForValidMandatoryparameters()
        {
            ValidateAddGeneralTescases(Constants.BlastParametersNode, "Add");
        }

        /// <summary>
        /// Validate a AddIfAbsent() method with mandatory values.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateAddIfAbsentMethodWithValidMandatoryparameters()
        {
            ValidateAddGeneralTescases(Constants.BlastDnaSequenceParametersNode, "AddIfAbsent");
        }

        /// <summary>
        /// Validate Azure Web Service protein query results.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateProteinQueryResults()
        {
            GeneralMethodToValidateResults(Constants.AzureBlastResultsNode,
                RequestType.FetchSyncUsingProteinSeq);
        }

        /// <summary>
        /// Validate Azure Web Service Dna query results.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaQueryResults()
        {
            GeneralMethodToValidateResults(
                Constants.DnaSeqAsynchronousResultsWithtBlastxNode,
                RequestType.FetchASyncUsingDnaSeq);
        }

        /// <summary>
        /// Fetch Blast results synchronous and Validate 
        /// Azure Web Service Dna query results.
        /// Input Data : Valid search query, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void FetchResultsSynchronous()
        {
            GeneralMethodToValidateResults(Constants.AzureBlastResultsNode,
                RequestType.FetchSyncUsingDnaSeq);
        }

        /// <summary>
        /// Validate Request status returned from Azure web service by passing 
        /// request Identifier for DNA  sequence.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateAzureRequestStatusMethodForDna()
        {
            ValidateGeneralGetRequestStatusMethod(Constants.AzureBlastResultsNode);
        }

        /// <summary>
        /// Validate Cancelling submitted Job.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of Cancelling submitted Job.
        /// </summary>
        /// Invalidated the test case for the Bug 115
        public void ValidateCancelSubmittedJob()
        {
            // Gets the search query parameter and their values.
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.DatabaseValue);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.ProgramParameter);

            // Set Blast Parameters
            IBlastServiceHandler service = null;
            try
            {
                service = new AzureBlastHandler();
                BlastParameters queryParams = new BlastParameters();
                queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
                queryParams.Add(queryProgramParameter, queryProgramValue);

                Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
                testCaseParms.Add(Constants.BlastParmsConst, queryParams);
                testCaseParms.Add(Constants.QuerySeqString, querySequence);

                // Get request identifier
                TestCaseParameters parameters = new TestCaseParameters(
                    Constants.AzureWebServiceCancelSubmitRequestTest, null,
                    GetRequestIdentifier, testCaseParms);

                object resultsObject = _TestCaseSimulator.Simulate(parameters).Result;

                // Cancel subitted job.
                bool result = service.CancelRequest(resultsObject as string);

                // validate the cancelled job.
                Assert.IsTrue(result);

                Console.WriteLine(string.Concat(
                    "Azure Blast BVT : Submitted job cancelled was successfully.",
                    queryProgramValue));
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }
        }

        #endregion Azure Blast BVT TestCases

        #region Supporting Methods

        /// <summary>
        /// Validates general Add method test cases with the xml node name specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">Name of the Add method</param>
        void ValidateAddGeneralTescases(string nodeName, string methodName)
        {
            // Gets the search query parameter and their values.
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.DatabaseValue);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.ProgramValue);
            string querySequenceParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.QuerySequencyparameter);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.ProgramParameter);

            // Set Service confiruration parameters true.
            ConfigParameters configParameters = new ConfigParameters();
            configParameters.UseBrowserProxy = true;

            // Create search parameters object.
            BlastParameters queryParams = new BlastParameters();

            // Add mandatory parameter values to search query parameters.

            if (0 == string.Compare(methodName, "Add", true, CultureInfo.CurrentCulture))
            {
                queryParams.Add(querySequenceParameter, querySequence);
                queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
                queryParams.Add(queryProgramParameter, queryProgramValue);
            }
            else
            {
                queryParams.AddIfAbsent(querySequenceParameter, querySequence);
                queryParams.AddIfAbsent(queryDatabaseParameter, queryDatabaseValue);
                queryParams.AddIfAbsent(queryProgramParameter, queryProgramValue);
            }

            // Validate search query parameters.
            Assert.IsTrue(queryParams.Settings.ContainsValue(querySequence));
            Assert.IsTrue(queryParams.Settings.ContainsValue(queryDatabaseValue));
            Assert.IsTrue(queryParams.Settings.ContainsValue(queryProgramValue));
            Assert.AreEqual(queryParams.Settings.Count, 3);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Azure Blast BVT : Query Sequence{0} is as expected.",
                querySequence));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Azure Blast BVT : DataBase Value{0} is as expected.",
                queryDatabaseValue));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Azure Blast BVT : Program Value {0} is as expected.",
                queryProgramValue));
        }

        /// <summary>
        /// Validates general Add method test cases with the xml node name specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="IsSynchFetch">True for Synchronous fetch</param>
        void GeneralMethodToValidateResults(string nodeName, RequestType reqType)
        {
            // Gets the search query parameter and their values.           
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
            string expectedHspHitsCount = _utilityObj._xmlUtil.GetTextValue(
               nodeName, Constants.HspHitsCount);

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();
            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);

            TestCaseParameters parameters = null;
            object resultsObject = null;
            switch (reqType)
            {
                case RequestType.FetchASyncUsingDnaSeq:
                    parameters = new TestCaseParameters(
                        Constants.AzureWebServiceFetchResultsAsyncForDna, null,
                        FetchResultsAsync, testCaseParms);
                    resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
                    break;
                case RequestType.FetchSyncUsingProteinSeq:
                    parameters = new TestCaseParameters(
                        Constants.AzureWebServiceFetchResultsAsyncForProtein, null,
                        FetchResultsAsync, testCaseParms);
                    resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
                    break;
                case RequestType.FetchSyncUsingDnaSeq:
                    parameters = new TestCaseParameters(
                        Constants.AzureWebServiceFetchResultsSyncForDnaTest, null,
                        FetchResultsSync, testCaseParms);
                    resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
                    break;
            }

            Assert.IsNotNull(resultsObject);

            // Validate blast results.
            List<BlastResult> blastResults = resultsObject as List<BlastResult>;
            Assert.IsNotNull(blastResults);
            Assert.IsNotNull(blastResults);
            Assert.AreEqual(blastResults.Count.ToString(
                (IFormatProvider)null), expectedHitsCount);
            Assert.AreEqual(blastResults[0].Records.Count.ToString(
                (IFormatProvider)null), expectedHitsCount);
            BlastSearchRecord record = blastResults[0].Records[0];
            Assert.AreEqual(record.Statistics.Kappa.ToString(
                (IFormatProvider)null), expectedKappaStatistics);
            Assert.AreEqual(record.Statistics.Lambda.ToString(
                (IFormatProvider)null), expectedLambdaStatistics);
            Assert.AreEqual(record.Statistics.Entropy.ToString(
                (IFormatProvider)null), expectedEntropyStatistics);
            Assert.AreEqual(record.Hits.Count.ToString(
                (IFormatProvider)null), expectedResultCount);
            Hit hit = record.Hits[0];
            Assert.AreEqual(hit.Accession, expectedAccession);
            Assert.AreEqual(hit.Id.ToString((IFormatProvider)null), expectedHitId);
            Assert.AreEqual(hit.Hsps.Count.ToString((IFormatProvider)null), expectedHspHitsCount);

            Console.WriteLine(string.Format((IFormatProvider)null,
                    "Azure Blast BVT: Hits count '{0}'.", blastResults.Count));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Azure Blast BVT: Accession '{0}'.", hit.Accession));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Azure Blast BVT: Hit Id '{0}'.", hit.Id));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Azure Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Azure Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Azure Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Azure Blast BVT: Hits count '{0}'.", blastResults.Count));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Azure Blast BVT: Accession '{0}'.", hit.Accession));
        }

        /// <summary>
        /// Validate general http request status by
        /// differnt parameters for Azure web service..
        /// <param name="nodeName">different alphabet node name</param>
        /// </summary>
        void ValidateGeneralGetRequestStatusMethod(string nodeName)
        {
            // Gets the search query parameter and their values.
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

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();
            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);

            TestCaseParameters parameters = new TestCaseParameters(
                Constants.AzureWebServiceRequestStatusForDnaTest, null,
                GetRequestStatus, testCaseParms);

            object resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
            ServiceRequestInformation reqInfo = resultsObject as ServiceRequestInformation;

            // Validate job status.
            if (reqInfo.Status != ServiceRequestStatus.Waiting &&
                reqInfo.Status != ServiceRequestStatus.Ready
                && reqInfo.Status != ServiceRequestStatus.Queued)
            {
                string error = ApplicationLog.WriteLine(string.Concat(
                    "Unexpected error", reqInfo.Status));
                Assert.Fail(error);
                Console.WriteLine(string.Concat(
                    "Azure Blast BVT: Unexpected error ", reqInfo.Status));
            }
            else
            {
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Azure Blast BVT: Client Request status has been validated successfully."));
                Console.WriteLine(string.Concat(
                    "Azure Blast BVT: Request status ", reqInfo.Status));
            }
        }

        /// <summary>
        /// Fetch results asynchronous
        /// </summary>
        /// <param name="blastParameters">Blast Input config parameters</param>
        /// <returns></returns>
        private TestCaseOutput FetchResultsAsync(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            Sequence sequence = new Sequence(Alphabets.DNA, sequenceString);

            // Set NCBIHandler configuration services
            IBlastServiceHandler service = null;
            object resultsObject = null;
            try
            {
                service = new AzureBlastHandler();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                configParams.DefaultTimeout = 5;
                configParams.Connection = new Uri(Constants.AzureUri);
                service.Configuration = configParams;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Get Request identifier from web service.
                string reqId = service.SubmitRequest(sequence, blastSearchPams);

                // Get request information for first time.
                ServiceRequestInformation info = service.GetRequestStatus(reqId);

                // Ping service until request staus is ready.
                int maxAttempts = 10;
                int attempt = 1;

                while (attempt <= maxAttempts && info.Status != ServiceRequestStatus.Ready &&
                    info.Status != ServiceRequestStatus.Error)
                {
                    System.Threading.Thread.Sleep(2000);
                    ++attempt;
                    info = service.GetRequestStatus(reqId);
                }


                IBlastParser blastXmlParser = new BlastXmlParser();
                using (StringReader reader = new StringReader(service.GetResult(reqId, blastSearchPams)))
                {
                    resultsObject = blastXmlParser.Parse(reader);
                }
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }

            return new TestCaseOutput(resultsObject, false);
        }

        /// <summary>
        /// Fetch results synchronous
        /// </summary>
        /// <param name="blastParameters">Blast Input config parameters</param>
        /// <returns></returns>
        private TestCaseOutput FetchResultsSync(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            Sequence sequence = new Sequence(Alphabets.DNA, sequenceString);

            // Set NCBIHandler configuration services
            IList<BlastResult> resultsObject = null;
            IBlastServiceHandler service = null;
            try
            {
                service = new AzureBlastHandler();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                configParams.DefaultTimeout = 5;
                configParams.Connection = new Uri(Constants.AzureUri);
                service.Configuration = configParams;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Get Request identifier from web service.
                string reqId = service.SubmitRequest(sequence, blastSearchPams);

                // Get request information for first time.
                ServiceRequestInformation info = service.GetRequestStatus(reqId);

                // Ping service until request staus is ready.
                int maxAttempts = 10;
                int attempt = 1;
                while (attempt <= maxAttempts && info.Status != ServiceRequestStatus.Ready &&
                    info.Status != ServiceRequestStatus.Error)
                {
                    System.Threading.Thread.Sleep(2000);
                    ++attempt;
                    info = service.GetRequestStatus(reqId);
                }

                resultsObject = service.FetchResultsSync(
                     reqId, blastSearchPams) as List<BlastResult>;
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }
            return new TestCaseOutput(resultsObject, false);
        }

        /// <summary>
        /// Get Request Status 
        /// </summary>
        /// <param name="blastParameters">Blast Input config parameters</param>
        /// <returns></returns>
        private TestCaseOutput GetRequestStatus(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            Sequence sequence = new Sequence(Alphabets.DNA, sequenceString);

            // Set NCBIHandler configuration services
            IBlastServiceHandler service = null;
            object requestStatus = null;
            try
            {
                service = new AzureBlastHandler();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                configParams.DefaultTimeout = 5;
                configParams.Connection = new Uri(Constants.AzureUri);
                service.Configuration = configParams;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Get Request identifier from web service.
                string reqId = service.SubmitRequest(sequence, blastSearchPams);

                // Get request information for first time.
                requestStatus = service.GetRequestStatus(reqId);
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }

            return new TestCaseOutput(requestStatus, false);
        }

        /// <summary>
        /// Get Request identifier for submitted job. 
        /// </summary>
        /// <param name="blastParameters">Blast Input config parameters</param>
        /// <returns></returns>
        private TestCaseOutput GetRequestIdentifier(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            Sequence sequence = new Sequence(Alphabets.DNA, sequenceString);

            // Set NCBIHandler configuration services
            IBlastServiceHandler service = null;
            object reqId = null;
            try
            {
                service = new AzureBlastHandler();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                configParams.DefaultTimeout = 5;
                configParams.Connection = new Uri(Constants.AzureUri);
                service.Configuration = configParams;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Get Request identifier from web service.
                reqId = service.SubmitRequest(sequence, blastSearchPams);
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }

            return new TestCaseOutput(reqId, false);
        }
        #endregion Supporting Methods


    }
}
