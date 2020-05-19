// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * AzureBlastP1TestCases.cs
 * 
 * This file contains the Azure Blast Web Service P1 test cases.
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
    /// Test Automation code for MBF Azure Blast Web Service and P1 level validations.
    /// </summary>
    public class AzureBlastP1TestCases
    {

        # region Enums

        /// <summary>
        /// AzureBlastHandler Constructor parameters
        /// Used for the different test cases.
        /// </summary>
        enum AzureWebServiceCtorParameters
        {
            ConfigPams,
            ParserAndConfigPams,
            Default
        };

        # endregion Enums

        #region Global Variables

        static bool _IsWebServiceAvailable = true;

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static AzureBlastP1TestCases()
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

        #region AzureBlast P1 TestCases

        /// <summary>
        /// Validate a Add() method with BlastX program parameter value.
        /// Input Data : Valid search query, Database value and program value.
        /// Output Data : Validation of Add() method.
        /// </summary>
        public void ValidateAddMethodForValidDnaBlastXPam()
        {
            ValidateAddGeneralTescases(Constants.DnaSeqAsynchronousResultsWithtBlastxNode, "Add");
        }

        /// <summary>
        /// Validate a AddIfAbsent() method with BlastX program parameter value.
        /// Input Data : Valid search query, Database value and program value.
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        public void ValidateAddIfAbsentMethodForValidDnaBlastXPam()
        {
            ValidateAddGeneralTescases(Constants.DnaSeqAsynchronousResultsWithtBlastxNode,
                "AddIfAbsent");
        }

        /// <summary>
        /// Validate a Add() method with Alu database value.
        /// Input Data : Valid search query, Database value and program value.
        /// Output Data : Validation of Add() method.
        /// </summary>
        public void ValidateAddMethodForValidDnaWithAluDbPam()
        {
            ValidateAddGeneralTescases(Constants.AluDatabaseParametersNode, "Add");
        }

        /// <summary>
        /// Validate a Add() method with Default  database value.
        /// Input Data : Valid search query, default Database value and program value.
        /// Output Data : Validation of Add() method.
        /// </summary>
        public void ValidateAddMethodForValidDnaWithDefaultDbPam()
        {
            ValidateAddGeneralTescases(Constants.DefaultDatabaseParameters, "Add");
        }

        /// <summary>
        /// Validate Request status returned from Azure web service by passing 
        /// request Identifier for DNA  sequence With BlastX program.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        public void ValidateAzureRequestStatusMethodForDnaWithBlastXProgram()
        {
            if (_IsWebServiceAvailable)
            {
                ValidateGeneralGetRequestStatusMethod(
                    Constants.DnaSeqAsynchronousResultsWithtBlastxNode);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Request status returned from Azure web service by passing 
        /// request Identifier for Rna  sequence 
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        public void ValidateAzureRequestStatusMethodForRnaSequence()
        {
            if (_IsWebServiceAvailable)
            {
                ValidateGeneralGetRequestStatusMethod(
                    Constants.RnaAzureResultsNode);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Request status returned from Azure web service by passing 
        /// request Identifier for Protein  sequence 
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        public void ValidateAzureRequestStatusMethodForProteinSequence()
        {
            if (_IsWebServiceAvailable)
            {
                ValidateGeneralGetRequestStatusMethod(
                    Constants.AzureBlastResultsNode);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Request status returned from Azure web service by passing 
        /// request Identifier for Dna Sequence with alu.a database
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        public void ValidateAzureRequestStatusMethodForAluDbPam()
        {
            if (_IsWebServiceAvailable)
            {
                ValidateGeneralGetRequestStatusMethod(
                    Constants.AluDatabaseParametersNode);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Request status returned from Azure web service by passing 
        /// request Identifier for Medium sized Dna Sequence with alu.a database
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        public void ValidateAzureRequestStatusMethodForMediumSizedDnaSeq()
        {
            if (_IsWebServiceAvailable)
            {
                ValidateGeneralGetRequestStatusMethod(
                    Constants.EBlastMediumSizeDnaSequenceParametersNode);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Request status returned from Azure web service by passing 
        /// request Identifier for Medium sized Protein Sequence with alu.a database
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        public void ValidateAzureRequestStatusMethodForMediumSizedProteinSeq()
        {
            if (_IsWebServiceAvailable)
            {
                ValidateGeneralGetRequestStatusMethod(
                    Constants.EbiBlastMediumSizeProteinSequenceParametersNode);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Cancelling Submitted request for Dna Sequence query.
        /// Input Data : Dna Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        public void ValidateCancelDnaQuerySequenceRequest()
        {
            if (_IsWebServiceAvailable)
            {
                ValidateCancelSubmittedJob(
                    Constants.DnaSeqAsynchronousResultsWithtBlastxNode);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Cancelling Submitted request for Medium sized Dna Sequence query.
        /// Input Data : Medium sized Dna Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        public void ValidateCancelRequestForMediumSizedDnaSequence()
        {
            if (_IsWebServiceAvailable)
            {
                ValidateCancelSubmittedJob(
                    Constants.EBlastMediumSizeDnaSequenceParametersNode);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Cancelling Submitted request for Medium sized Protein Sequence query.
        /// Input Data : Medium sized Protein Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        public void ValidateCancelProteinQuerySequenceRequest()
        {
            if (_IsWebServiceAvailable)
            {
                ValidateCancelSubmittedJob(
                    Constants.EbiBlastMediumSizeProteinSequenceParametersNode);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Azure Web Service Alu db results.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        public void ValidateBlastResultsForAluDBPam()
        {
            if (_IsWebServiceAvailable)
            {
                GeneralMethodToValidateResults(Constants.AluDatabaseParametersNode,
                    AzureWebServiceCtorParameters.ConfigPams, false);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Azure Web Service BlastX program results.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        public void ValidateBlastResultsForRnaSeq()
        {
            if (_IsWebServiceAvailable)
            {
                GeneralMethodToValidateResults(
                    Constants.RnaAzureResultsNode,
                    AzureWebServiceCtorParameters.ConfigPams, false);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Azure Web Service Alu db results by passing config parameters 
        /// to Azure constructor.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        public void ValidateBlastResultsForAluDBPamUsingConstructorPams()
        {
            if (_IsWebServiceAvailable)
            {
                GeneralMethodToValidateResults(Constants.AluDatabaseParametersNode,
                    AzureWebServiceCtorParameters.ParserAndConfigPams, false);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Azure Web Service BlastX program results by passing config 
        /// parameters to Azure constructor..
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        public void ValidateBlastResultsForBlastXPamUsingConstructorPams()
        {
            if (_IsWebServiceAvailable)
            {
                GeneralMethodToValidateResults(
                    Constants.DnaSeqAsynchronousResultsWithtBlastxNode,
                    AzureWebServiceCtorParameters.ParserAndConfigPams, false);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Azure Web Service Alu db results.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        public void ValidateBlastResultsForAluDBPamWithConfigPams()
        {
            if (_IsWebServiceAvailable)
            {
                GeneralMethodToValidateResults(Constants.AluDatabaseParametersNode,
                    AzureWebServiceCtorParameters.ConfigPams, false);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Azure Web Service Alu db results by fetching results synchronously.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        public void ValidateBlastResultsSynchronouslyForAluDBPamWithConfigPams()
        {
            if (_IsWebServiceAvailable)
            {
                GeneralMethodToValidateResults(Constants.AluDatabaseParametersNode,
                    AzureWebServiceCtorParameters.ConfigPams, true);
            }
            else
            {
                Assert.Ignore("The test case ignored due to connection failure");
            }
        }

        /// <summary>
        /// Validate Azure Web Service Submit request for List of Sequences.
        /// Input Data :Valid search queries, Database value and program value.
        /// Output Data : Validation of blast results 
        /// </summary>
        public void ValidateSubmitSearchRequestForQueryList()
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

            IList<ISequence> seqList = new List<ISequence>();
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabet), querySequence);
            seqList.Add(seq);

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

            // Get Request identifier from web service for seqList.
            // Automated this case to hit the Code.
            try
            {
                service.SubmitRequest(seqList, searchParams);
                Assert.Fail();
            }
            catch (NotImplementedException)
            {
                Console.WriteLine(string.Format(null,
                    "Azure Blast P1 : Validated the exception successfully."));
            }
        }

        /// <summary>
        /// Validate Azure webservice Properties.
        /// </summary>
        public void ValidateAzureWebServiceProperties()
        {
            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            configParams.DefaultTimeout = 10;
            configParams.Connection = new Uri(Constants.AzureUri);

            IBlastServiceHandler service = new AzureBlastHandler(configParams);

            // Validate Azure Web Service properties.
            Assert.AreEqual(Constants.AzureWebServiceDescription, service.Description);
            Assert.AreEqual(Constants.AzureWebServiceName, service.Name);
        }

        #endregion AzureBlast P1 TestCases

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
                "Azure Blast P1: Query Sequence {0} is as expected.",
                querySequence));
            Console.WriteLine(string.Format(null,
                "Azure Blast P1: DataBase Value {0} is as expected.",
                queryDatabaseValue));
            Console.WriteLine(string.Format(null,
                "Azure Blast P1: Program Value {0} is as expected.",
                queryProgramValue));
        }

        /// <summary>
        /// Validates general Add method test cases with the xml node name specified.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="azureCtorPam">Azure constructor different parameters</azureCtorPam>
        /// <param name="IsSynchronousFetch">True for SynchronousFetch validation.</param>
        static void GeneralMethodToValidateResults(string nodeName,
            AzureWebServiceCtorParameters azureCtorPam, bool IsSynchronousFetch)
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
            string expectedBitScore = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BitScore);
            string expectedHitSequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.HitSequence);
            string expectedHspHitsCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.HspHitsCount);
            string expectedSleepTime = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SleepTime);

            string reqId = string.Empty;
            object responseResults = null;
            int maxAttempts = 20;
            int attempt = 1;

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabet), querySequence);
            IBlastServiceHandler service;
            BlastXmlParser parser = new BlastXmlParser();
            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            configParams.DefaultTimeout = 10;
            configParams.Connection = new Uri(Constants.AzureUri);

            // Set Service confiruration parameters 
            switch (azureCtorPam)
            {
                case AzureWebServiceCtorParameters.ConfigPams:
                    service = new AzureBlastHandler(configParams);
                    break;
                case AzureWebServiceCtorParameters.ParserAndConfigPams:
                    service = new AzureBlastHandler(parser, configParams);
                    break;
                default:
                    service = new AzureBlastHandler();
                    break;
            }

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
            while (attempt <= maxAttempts && info.Status != ServiceRequestStatus.Ready
                && info.Status != ServiceRequestStatus.Error)
            {
                System.Threading.Thread.Sleep(Convert.ToInt32(expectedSleepTime));
                ++attempt;
                info = service.GetRequestStatus(reqId);
            }

            // Get results.
            if (IsSynchronousFetch)
            {
                responseResults = service.FetchResultsSync(reqId, searchParams);
            }
            else
            {
                responseResults = service.GetResult(reqId, searchParams);
            }
            Assert.IsNotNull(responseResults);

            if (!IsSynchronousFetch)
            {
                //Parse and validate results
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
                Assert.AreEqual(hit.Hsps.Count.ToString((IFormatProvider)null),
                    expectedHspHitsCount);
                Assert.AreEqual(hit.Hsps[0].BitScore.ToString((IFormatProvider)null),
                    expectedBitScore);
                Assert.AreEqual(hit.Hsps[0].HitSequence.ToString((IFormatProvider)null),
                    expectedHitSequence);
                Console.WriteLine(string.Format(null,
                    "Azure Blast P1: Hits count '{0}'.", blastResults.Count));
                Console.WriteLine(string.Format(null,
                    "Azure Blast P1: Accession '{0}'.", hit.Accession));
                Console.WriteLine(string.Format(null,
                    "Azure Blast P1: Hit Id '{0}'.", hit.Id));
                Console.WriteLine(string.Format(null,
                    "Azure Blast P1: Hits Count '{0}'.", hit.Hsps.Count));
                ApplicationLog.WriteLine(string.Format(null,
                    "Azure Blast P1: Hits count '{0}'.", blastResults.Count));
                ApplicationLog.WriteLine(string.Format(null,
                    "Azure Blast P1: Accession '{0}'.", hit.Accession));
                ApplicationLog.WriteLine(string.Format(null,
                    "Azure Blast P1: Hit Id '{0}'.", hit.Id));
                ApplicationLog.WriteLine(string.Format(null,
                    "Azure Blast P1: Hits Count '{0}'.", hit.Hsps.Count));
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
            if (reqInfo.Status != ServiceRequestStatus.Waiting
                && reqInfo.Status != ServiceRequestStatus.Ready
                && reqInfo.Status != ServiceRequestStatus.Queued)
            {
                string error = ApplicationLog.WriteLine(string.Format(
                    null, "Unexpected error", reqInfo.Status));
                Assert.Fail(error);
                Console.WriteLine(string.Format(null,
                    "Azure Blast P1: Unexpected error", reqInfo.Status));
            }
            else
            {
                Console.WriteLine(string.Format(null,
                    "Azure Blast P1: Client Request status has been validated successfully."));
                Console.WriteLine(string.Format(null,
                    "Azure Blast P1: Request status {0} ", reqInfo.Status));
            }
        }

        /// <summary>
        /// Validate Cancel submitted job by passing job id.
        /// <param name="nodeName">different alphabet node name</param>
        /// </summary>
        static void ValidateCancelSubmittedJob(string nodeName)
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
                "Azure Blast P1 : Submitted job cancelled was successfully.",
                queryProgramValue));
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
                        "Azure P1 : Connection not successful with no Response result");
                    ApplicationLog.WriteLine(
                        "Azure P1 : Connection not successful with no Response result");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format(
                    "Azure P1 : Connection not successful with error '{0}'", ex.Message));
                ApplicationLog.WriteLine(string.Format(
                    "Azure P1 : Connection not successful with error '{0}'", ex.Message));
                return false;
            }

            Console.WriteLine("Azure P1 : Connection Successful");
            return true;
        }

        #endregion Supporting Methods
    }
}
