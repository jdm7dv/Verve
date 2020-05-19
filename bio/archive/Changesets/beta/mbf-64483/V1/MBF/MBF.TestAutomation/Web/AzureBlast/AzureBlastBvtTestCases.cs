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

using NUnit.Framework;

namespace MBF.TestAutomation.Web.AzureBlast
{
    /// <summary>
    /// Test Automation code for MBF Azure Blast Web Service and BVT level validations.
    /// </summary>
    public class AzureBlastBvtTestCases
    {

        #region Global Variables

        static bool _IsWebServiceAvailable = true;

        #endregion Global Variables

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

            Utility._xmlUtil = new XmlUtility(@"TestUtils\AzureBlastConfig.xml");

            _IsWebServiceAvailable = ValidateWebServiceConnection();
        }

        #endregion Constructor

        #region Azure Blast BVT TestCases

        /// <summary>
        /// Validate a Add() method with mandatory values.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of Add() method.
        /// </summary>
        public void ValidateAddMethodForValidMandatoryparameters()
        {
            ValidateAddGeneralTescases(Constants.BlastParametersNode, "Add");
        }

        /// <summary>
        /// Validate a AddIfAbsent() method with mandatory values.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        public void ValidateAddIfAbsentMethodWithValidMandatoryparameters()
        {
            ValidateAddGeneralTescases(Constants.BlastDnaSequenceParametersNode, "AddIfAbsent");
        }

        /// <summary>
        /// Validate Azure Web Service protein query results.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        /// Disabling this test case till web service is up.
        public void ValidateProteinQueryResults()
        {
            if (_IsWebServiceAvailable)
            {
                GeneralMethodToValidateResults(Constants.AzureBlastResultsNode, false);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Azure Web Service Dna query results.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        /// Disabling this test case till web service is up.
        public void ValidateDnaQueryResults()
        {
            if (_IsWebServiceAvailable)
            {
                GeneralMethodToValidateResults(
                    Constants.DnaSeqAsynchronousResultsWithtBlastxNode, false);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Fetch Blast results synchronous and Validate 
        /// Azure Web Service Dna query results.
        /// Input Data : Valid search query, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        /// Disabling this test case till web service is up.
        public void FetchResultsSynchronous()
        {
            if (_IsWebServiceAvailable)
            {
                GeneralMethodToValidateResults(Constants.AzureBlastResultsNode, true);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Request status returned from Azure web service by passing 
        /// request Identifier for DNA  sequence.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        /// Disabling this test case till web service is up.
        public void ValidateAzureRequestStatusMethodForDna()
        {
            if (_IsWebServiceAvailable)
            {
                ValidateGeneralGetRequestStatusMethod(Constants.AzureBlastResultsNode);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Cancelling submitted Job.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of Cancelling submitted Job.
        /// </summary>
        /// Invalidated the test case for the Bug 115
        /// Disabling this test case till web service is up.
        public void ValidateCancelSubmittedJob()
        {
            if (_IsWebServiceAvailable)
            {
                // Gets the search query parameter and their values.
                string alphabet = Utility._xmlUtil.GetTextValue(
                    Constants.AzureBlastResultsNode, Constants.AlphabetNameNode);
                string querySequence = Utility._xmlUtil.GetTextValue(
                    Constants.AzureBlastResultsNode, Constants.QuerySequency);
                string queryDatabaseValue = Utility._xmlUtil.GetTextValue(
                    Constants.AzureBlastResultsNode, Constants.DatabaseValue);
                string queryProgramValue = Utility._xmlUtil.GetTextValue(
                    Constants.AzureBlastResultsNode, Constants.ProgramValue);
                string queryDatabaseParameter = Utility._xmlUtil.GetTextValue(
                    Constants.AzureBlastResultsNode, Constants.DatabaseParameter);
                string queryProgramParameter = Utility._xmlUtil.GetTextValue(
                    Constants.AzureBlastResultsNode, Constants.ProgramParameter);
                string reqId = string.Empty;
                bool result = false;

                Sequence seq = new Sequence(Utility.GetAlphabet(alphabet), querySequence);

                // Set Service confiruration parameters true.
                IBlastServiceHandler service = new AzureBlastHandler();

                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                configParams.DefaultTimeout = 5;
                configParams.Connection = new Uri(Constants.AzureUri);
                service.Configuration = configParams;

                // Create search parameters object.
                BlastParameters searchParams = new BlastParameters();

                // Add mandatory parameter values to search query parameters.
                searchParams.Add(queryProgramParameter, queryProgramValue);
                searchParams.Add(queryDatabaseParameter, queryDatabaseValue);

                // Get Request identifier from web service.
                reqId = service.SubmitRequest(seq, searchParams);

                // Cancel subitted job.
                result = service.CancelRequest(reqId);

                // validate the cancelled job.
                Assert.IsTrue(result);

                Console.WriteLine(string.Format(null,
                    "Azure Blast BVT : Submitted job cancelled was successfully.",
                    queryProgramValue));
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        #endregion Azure Blast BVT TestCases

        #region Supporting Methods

        /// <summary>
        /// Validates general Add method test cases with the xml node name specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">Name of the Add method</param>
        static void ValidateAddGeneralTescases(string nodeName, string methodName)
        {
            // Gets the search query parameter and their values.
            string querySequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.QuerySequency);
            string queryDatabaseValue = Utility._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.DatabaseValue);
            string queryProgramValue = Utility._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.ProgramValue);
            string querySequenceParameter = Utility._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.QuerySequencyparameter);
            string queryDatabaseParameter = Utility._xmlUtil.GetTextValue(
                Constants.BlastParametersNode, Constants.DatabaseParameter);
            string queryProgramParameter = Utility._xmlUtil.GetTextValue(
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
            Console.WriteLine(string.Format(null,
                "Azure Blast BVT : Query Sequence{0} is as expected.",
                querySequence));
            Console.WriteLine(string.Format(null,
                "Azure Blast BVT : DataBase Value{0} is as expected.",
                queryDatabaseValue));
            Console.WriteLine(string.Format(null,
                "Azure Blast BVT : Program Value {0} is as expected.",
                queryProgramValue));
        }

        /// <summary>
        /// Validates general Add method test cases with the xml node name specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="IsSynchFetch">True for Synchronous fetch</param>
        static void GeneralMethodToValidateResults(string nodeName, bool IsSynchFetch)
        {
            // Gets the search query parameter and their values.
            string alphabet = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
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
            string expectedResultCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ResultsCount);
            string expectedHitsCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.HitsCount);
            string expectedEntropyStatistics = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.EntropyStatistics);
            string expectedKappaStatistics = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.KappaStatistics);
            string expectedLambdaStatistics = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.LambdaStatistics);
            string expectedHspHitsCount = Utility._xmlUtil.GetTextValue(
               nodeName, Constants.HspHitsCount);
            string expectedSleepTime = Utility._xmlUtil.GetTextValue(
               nodeName, Constants.SleepTime);
            string reqId = string.Empty;
            object responseResults = null;
            int maxAttempts = 20;
            int attempt = 1;

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabet), querySequence);

            // Set Service confiruration parameters true.
            IBlastServiceHandler service = new AzureBlastHandler();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            configParams.DefaultTimeout = 5;
            configParams.Connection = new Uri(Constants.AzureUri);
            service.Configuration = configParams;

            // Create search parameters object.
            BlastParameters searchParams = new BlastParameters();

            // Add mandatory parameter values to search query parameters.
            searchParams.Add(queryProgramParameter, queryProgramValue);
            searchParams.Add(queryDatabaseParameter, queryDatabaseValue);

            // Get Request identifier from web service.
            reqId = service.SubmitRequest(seq, searchParams);

            // Get request information for first time.
            ServiceRequestInformation info = service.GetRequestStatus(reqId);

            // Ping service until request staus is ready.
            while (attempt <= maxAttempts && info.Status != ServiceRequestStatus.Ready &&
                info.Status != ServiceRequestStatus.Error)
            {
                System.Threading.Thread.Sleep(Convert.ToInt32(expectedSleepTime));
                ++attempt;
                info = service.GetRequestStatus(reqId);
            }

            // Get results.
            if (IsSynchFetch)
            {
                responseResults = service.FetchResultsSync(reqId, searchParams);
            }
            else
            {
                responseResults = service.GetResult(reqId, searchParams);
            }

            Assert.IsNotNull(responseResults);

            if (!IsSynchFetch)
            {
                //Parse and validate results
                BlastXmlParser parser = new BlastXmlParser();
                IList<BlastResult> blastResults =
                    parser.Parse(new StringReader(responseResults.ToString()));
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
                Console.WriteLine(string.Format(null,
                    "Azure Blast BVT: Hits count '{0}'.", blastResults.Count));
                Console.WriteLine(string.Format(null,
                    "Azure Blast BVT: Accession '{0}'.", hit.Accession));
                Console.WriteLine(string.Format(null,
                    "Azure Blast BVT: Hit Id '{0}'.", hit.Id));
                Console.WriteLine(string.Format(null,
                    "Azure Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));
                ApplicationLog.WriteLine(string.Format(null,
                    "Azure Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));
                ApplicationLog.WriteLine(string.Format(null,
                    "Azure Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));
                ApplicationLog.WriteLine(string.Format(null,
                    "Azure Blast BVT: Hits count '{0}'.", blastResults.Count));
                ApplicationLog.WriteLine(string.Format(null,
                    "Azure Blast BVT: Accession '{0}'.", hit.Accession));
            }
        }

        /// <summary>
        /// Validate general http request status by
        /// differnt parameters for Azure web service..
        /// <param name="nodeName">different alphabet node name</param>
        /// </summary>
        static void ValidateGeneralGetRequestStatusMethod(string nodeName)
        {
            // Gets the search query parameter and their values.
            string alphabet = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
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
            string reqId = string.Empty;

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabet), querySequence);

            // Set Service confiruration parameters true.
            IBlastServiceHandler service = new AzureBlastHandler();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            configParams.DefaultTimeout = 5;
            configParams.Connection = new Uri(Constants.AzureUri);
            service.Configuration = configParams;

            // Create search parameters object.
            BlastParameters searchParams = new BlastParameters();

            // Add mandatory parameter values to search query parameters.
            searchParams.Add(queryProgramParameter, queryProgramValue);
            searchParams.Add(queryDatabaseParameter, queryDatabaseValue);

            // Get Request identifier from web service.
            reqId = service.SubmitRequest(seq, searchParams);

            // Get request information for first time.
            ServiceRequestInformation reqInfo = service.GetRequestStatus(reqId);

            // Validate job status.
            if (reqInfo.Status != ServiceRequestStatus.Waiting &&
                reqInfo.Status != ServiceRequestStatus.Ready
                && reqInfo.Status != ServiceRequestStatus.Queued)
            {
                string error = ApplicationLog.WriteLine(string.Format(
                    null, "Unexpected error", reqInfo.Status));
                Assert.Fail(error);
                Console.WriteLine(string.Format(null,
                    "Azure Blast BVT: Unexpected error", reqInfo.Status));
            }
            else
            {
                Console.WriteLine(string.Format(null,
                    "Azure Blast BVT: Client Request status has been validated successfully."));
                Console.WriteLine(string.Format(null,
                    "Azure Blast BVT: Request status {0} ", reqInfo.Status));
            }
        }

        /// <summary>
        /// Validate if the connection is successful
        /// </summary>
        /// <returns>True, if connection is successful</returns>
        static bool ValidateWebServiceConnection()
        {
            // Gets the search query parameter and their values.
            string alphabet = Utility._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.AlphabetNameNode);
            string querySequence = Utility._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.QuerySequency);
            string queryDatabaseValue = Utility._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.DatabaseValue);
            string queryProgramValue = Utility._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.ProgramValue);
            string queryDatabaseParameter = Utility._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.DatabaseParameter);
            string queryProgramParameter = Utility._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.ProgramParameter);
            string expectedSleepTime = Utility._xmlUtil.GetTextValue(
               Constants.AzureBlastResultsNode, Constants.SleepTime);
            string reqId = string.Empty;
            object responseResults = null;
            int maxAttempts = 20;
            int attempt = 1;

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabet), querySequence);

            // Set Service confiruration parameters true.
            IBlastServiceHandler service = new AzureBlastHandler();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            configParams.DefaultTimeout = 5;
            configParams.Connection = new Uri(Constants.AzureUri);
            service.Configuration = configParams;

            // Create search parameters object.
            BlastParameters searchParams = new BlastParameters();

            // Add mandatory parameter values to search query parameters.
            searchParams.Add(queryProgramParameter, queryProgramValue);
            searchParams.Add(queryDatabaseParameter, queryDatabaseValue);

            try
            {
                // Get Request identifier from web service.
                reqId = service.SubmitRequest(seq, searchParams);

                // Get request information for first time.
                ServiceRequestInformation info = service.GetRequestStatus(reqId);

                // Ping service until request staus is ready.
                while (attempt <= maxAttempts && info.Status != ServiceRequestStatus.Ready &&
                    info.Status != ServiceRequestStatus.Error)
                {
                    System.Threading.Thread.Sleep(Convert.ToInt32(expectedSleepTime));
                    ++attempt;
                    info = service.GetRequestStatus(reqId);
                }

                responseResults = service.GetResult(reqId, searchParams);

                if (null == responseResults)
                {
                    Console.WriteLine(
                        "Azure BVT : Connection not successful with no Response result");
                    ApplicationLog.WriteLine(
                        "Azure BVT : Connection not successful with no Response result");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(
                    "Azure BVT : Connection not successful with error '{0}'", ex.Message));
                ApplicationLog.WriteLine(string.Format(
                    "Azure BVT : Connection not successful with error '{0}'", ex.Message));
                return false;
            }

            Console.WriteLine("Azure BVT : Connection Successful");
            return true;
        }

        #endregion Supporting Methods
    }
}