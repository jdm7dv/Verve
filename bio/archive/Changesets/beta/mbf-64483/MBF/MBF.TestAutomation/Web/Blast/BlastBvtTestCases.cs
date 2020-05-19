// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * BlastBvtTestCases.cs
 * 
 * This file contains the Blast Web Service BVT test cases.
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.TestUtils.SimulatorUtility;


namespace MBF.TestAutomation.Web.Blast
{
    /// <summary>
    /// Test Automation code for MBF Blast Web Service and BVT level validations.
    /// </summary>
    [TestClass]
    public class BlastBvtTestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

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
        static BlastBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Blast Bvt TestCases

        /// <summary>
        /// Validate a Paraemters Add() method with mandatory values.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of Add() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAddMethodForValidMandatoryparameters()
        {
            ValidateAddGeneralTescases(Constants.BlastParametersNode, "Add");
        }

        /// <summary>
        /// Validate a Paraemters Add() method with mandatory values by passing 
        /// protein seqeunce as parameter.
        /// Input Data :Valid protein seqeunce, Database value and program value.
        /// Output Data : Validation of Add() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAddMethodWithProteinSeqeunce()
        {
            ValidateAddGeneralTescases(Constants.BlastProteinSequenceParametersNode, "Add");
        }

        /// <summary>
        /// Validate a Paraemters Add() method with mandatory values by passing Dna seqeunce as parameter.
        /// Input Data :Valid Dna seqeunce, Database value and program value.
        /// Output Data : Validation of Add() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAddMethodForDnaSeqeunce()
        {
            ValidateAddGeneralTescases(Constants.BlastDnaSequenceParametersNode, "Add");
        }

        /// <summary>
        /// Validate a Paraemters AddIfAbsent() method with mandatory values.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of AddIfAbsent() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAddIfAbsentMethodWithValidMandatoryparameters()
        {
            ValidateAddGeneralTescases(Constants.BlastDnaSequenceParametersNode, "AddIfAbsent");
        }

        /// <summary>
        /// Validate Request status by passing request Identifier
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateGetRequestStatusWithRequestIdentifier()
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

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();

            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);

            TestCaseParameters parameters = new TestCaseParameters(
                Constants.GetRequestStatusInfoForProteinSeqTest, null,
                GetRequestStatusForProtein, testCaseParms);

            object resultsObject = _TestCaseSimulator.Simulate(parameters).Result;

            ServiceRequestInformation reqInfo = resultsObject as ServiceRequestInformation;

            // Validate job status.
            if (reqInfo.Status != ServiceRequestStatus.Waiting
                && reqInfo.Status != ServiceRequestStatus.Ready)
            {
                string error = ApplicationLog.WriteLine(
                    string.Format((IFormatProvider)null, "Unexpected error", reqInfo.Status));
                Assert.Fail(error);
                Console.WriteLine(string.Format((IFormatProvider)null, "Unexpected error", reqInfo.Status));
            }
            else
            {
                Console.WriteLine(string.Format((IFormatProvider)null, "Request status{0} ", reqInfo.Status));
            }
        }

        /// <summary>
        /// Validate Request status by passing request Identifier for DNA  sequence.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of GetRequestStatus() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateGetRequestStatusMethodForDna()
        {
            // Gets the search query parameter and their values.
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastDnaSequenceParametersNode, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastDnaSequenceParametersNode, Constants.DatabaseValue);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastDnaSequenceParametersNode, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastDnaSequenceParametersNode, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastDnaSequenceParametersNode, Constants.ProgramParameter);

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();

            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);

            TestCaseParameters parameters = new TestCaseParameters(
                Constants.GetRequestStatusInfoForDnaSeqTest, null,
                GetRequestStatusForDna, testCaseParms);

            object resultsObject = _TestCaseSimulator.Simulate(parameters).Result;

            ServiceRequestInformation reqInfo = resultsObject as ServiceRequestInformation;

            // Validate job status.
            if (reqInfo.Status != ServiceRequestStatus.Waiting
                && reqInfo.Status != ServiceRequestStatus.Ready)
            {
                string error = ApplicationLog.WriteLine(string.Format(
                    null, "Unexpected error", reqInfo.Status));
                Assert.Fail(error);
                Console.WriteLine(string.Format((IFormatProvider)null, "Unexpected error", reqInfo.Status));
            }
            else
            {
                Console.WriteLine(string.Format((IFormatProvider)null, "Request status {0} ", reqInfo.Status));
            }
        }

        /// <summary>
        /// Validate Parse a xml file using parse(file-name)
        /// Input Data : Valid Blast xml file.
        /// Output Data : Validation of Blast xml record results.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateParseWithSmallSizeBlastXml()
        {
            // Gets the Blast Xml file 
            string blastFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.BlastResultfilePath);
            string expectedBitScore = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.BitScore);
            string expectedDatabselength = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.DatabaseLength);
            string expectedParameterMatrix = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.ParameterMatrix);
            string expectedGapCost = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.ParameterGap);
            string expectedHitSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.HitSequence);
            string expectedAccession = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.HitAccession);
            string expectedAlignmentLength = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.AlignmentLength);
            string expectedResultCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.ResultsCount);
            string expectedHitsCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.HitsCount);

            Assert.IsTrue(File.Exists(blastFilePath));
            // Logs information to the log file
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Blast BVT: File Exists in the Path '{0}'.", blastFilePath));

            // Parse a Blast xml file.
            BlastXmlParser parser = new BlastXmlParser();
            IList<BlastResult> blastResults = parser.Parse(blastFilePath);

            // Validate Meta data 
            BlastXmlMetadata meta = blastResults[0].Metadata;

            Assert.AreEqual(meta.ParameterGapOpen.ToString((IFormatProvider)null),
                expectedGapCost);
            Assert.AreEqual(meta.ParameterMatrix.ToString((IFormatProvider)null),
                expectedParameterMatrix);

            // Validate blast records.
            BlastSearchRecord record = blastResults[4].Records[0];

            Assert.AreEqual(expectedResultCount, blastResults.Count.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedDatabselength,
                record.Statistics.DatabaseLength.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedHitsCount, record.Hits.Count.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedAccession, record.Hits[0].Accession.ToString((IFormatProvider)null));
            Assert.AreEqual(expectedHitsCount, record.Hits[0].Hsps.Count.ToString((IFormatProvider)null));

            // Validate bit score.
            Hsp highScoreSgment = record.Hits[0].Hsps[0];
            Assert.AreEqual(expectedAlignmentLength, highScoreSgment.AlignmentLength.ToString(
                (IFormatProvider)null));
            Assert.AreEqual(expectedBitScore, highScoreSgment.BitScore.ToString((
                IFormatProvider)null));
            Assert.AreEqual(expectedHitSequence, highScoreSgment.HitSequence.ToString(
                (IFormatProvider)null));

            // Log results to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null, "Blast BVT: Bit Sequence '{0}'.",
            highScoreSgment.HitSequence.ToString()));
            Console.WriteLine(string.Format((IFormatProvider)null, "Blast BVT: Bit Score '{0}'.",
                highScoreSgment.BitScore));
            Console.WriteLine(string.Format((IFormatProvider)null, "Blast BVT: Bit Alignment '{0}'.",
                highScoreSgment.AlignmentLength));
            Console.WriteLine(string.Format((IFormatProvider)null, "Blast BVT: Hits Count '{0}'.",
                record.Hits[0].Hsps.Count));
            Console.WriteLine(string.Format((IFormatProvider)null, "Blast BVT: Results Count '{0}'.",
                blastResults.Count));
        }

        /// <summary>
        /// Validate Parse a xml file using parse(text-reader)
        /// Input Data :Valid Blast xml file.
        /// Output Data : Validation of Blast xml record results.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateParseTextReaderWithSmallSizeBlastXml()
        {
            // Gets the Blast Xml file 
            string blastFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.BlastResultfilePath);
            string expectedBitScore = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.BitScore);
            string expectedDatabselength = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.DatabaseLength);
            string expectedParameterMatrix = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.ParameterMatrix);
            string expectedGapCost = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.ParameterGap);
            string expectedHitSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.HitSequence);
            string expectedAccession = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.HitAccession);
            string expectedAlignmentLength = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.AlignmentLength);
            string expectedResultCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.ResultsCount);
            string expectedHitsCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleBlastXmlNode, Constants.HitsCount);

            Assert.IsTrue(File.Exists(blastFilePath));
            // Logs information to the log file
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Blast BVT: File Exists in the Path '{0}'.", blastFilePath));

            // Parse a Blast xml file.
            BlastXmlParser parser = new BlastXmlParser();
            IList<BlastResult> blastResults = null;
            using (StreamReader reader = File.OpenText(blastFilePath))
            {
                blastResults = parser.Parse(reader);
            }
            // Validate Meta data 
            BlastXmlMetadata meta = blastResults[0].Metadata;

            Assert.AreEqual(meta.ParameterGapOpen.ToString(
                (IFormatProvider)null), expectedGapCost);
            Assert.AreEqual(meta.ParameterMatrix.ToString(
                (IFormatProvider)null), expectedParameterMatrix);

            // Validate blast records.
            BlastSearchRecord record = blastResults[4].Records[0];

            Assert.AreEqual(expectedResultCount, blastResults.Count.ToString(
                (IFormatProvider)null));
            Assert.AreEqual(expectedDatabselength, record.Statistics.DatabaseLength.ToString(
                (IFormatProvider)null));
            Assert.AreEqual(expectedHitsCount, record.Hits.Count.ToString(
                (IFormatProvider)null));
            Assert.AreEqual(expectedAccession, record.Hits[0].Accession.ToString(
                (IFormatProvider)null));
            Assert.AreEqual(expectedHitsCount, record.Hits[0].Hsps.Count.ToString(
                (IFormatProvider)null));

            // Validate bit score.
            Hsp highScoreSgment = record.Hits[0].Hsps[0];
            Assert.AreEqual(expectedAlignmentLength, highScoreSgment.AlignmentLength.ToString(
                (IFormatProvider)null));
            Assert.AreEqual(expectedBitScore, highScoreSgment.BitScore.ToString(
                (IFormatProvider)null));
            Assert.AreEqual(expectedHitSequence, highScoreSgment.HitSequence.ToString(
                (IFormatProvider)null));

            // Log results to Nunit GUI.
            Console.WriteLine(string.Format((IFormatProvider)null, "Blast BVT: Bit Sequence '{0}'.",
               highScoreSgment.HitSequence.ToString()));
            Console.WriteLine(string.Format((IFormatProvider)null, "Blast BVT: Bit Score '{0}'.",
                highScoreSgment.BitScore));
            Console.WriteLine(string.Format((IFormatProvider)null, "Blast BVT: Bit Alignment '{0}'.",
                highScoreSgment.AlignmentLength));
            Console.WriteLine(string.Format((IFormatProvider)null, "Blast BVT: Hits Count '{0}'.",
                record.Hits[0].Hsps.Count));
            Console.WriteLine(string.Format((IFormatProvider)null, "Blast BVT: Results Count '{0}'.",
                blastResults.Count));
        }

        /// <summary>
        /// Validate SubmitHttpRequest by pasing response stream.
        /// Input Data :Valid search query, Database value and program value,WebSerive Uri.
        /// Output Data : Validataion of SubmitHttpRequest() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateHttpRequestForResponseString()
        {
            // Gets the search query parameter and their values.
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.DatabaseValue);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.ProgramParameter);
            string queryParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.QuerySequencyparameter);
            string webUri = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.BlastWebServiceUri);
            WebAccessorResponse requestResult;

            // Set Service confiruration parameters true.
            IBlastServiceHandler blastService = null;
            try
            {
                blastService = new NCBIBlastHandler();
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
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Http Request was submitted successfully"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Blast BVT: DataBase Value {0} is as expected.", queryDatabaseValue));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Blast BVT: Program Value {0} is as expected.", queryProgramValue));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Blast BVT: Query sequence {0} is as expected.", querySequence));

                // Close Web service request.
                webAccessor.Close();
            }
            finally
            {
                if (blastService != null)
                    ((IDisposable)blastService).Dispose();
            }
        }

        /// <summary>
        /// Validate SubmitHttpRequest by passing response stream.
        /// Input Data :Valid search query, Database value and program value,WebSerive Uri.
        /// Output Data : Validataion of SubmitHttpRequest() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateHttpRequestForResponseStream()
        {
            // Gets the search query parameter and their values.
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.DatabaseValue);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.ProgramParameter);
            string queryParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.QuerySequencyparameter);
            string webUri = _utilityObj._xmlUtil.GetTextValue(
                Constants.BlastRequestParametersNode, Constants.BlastWebServiceUri);
            WebAccessorResponse requestResult;

            // Set Service confiruration parameters true.
            IBlastServiceHandler blastService = null;
            try
            {
                blastService = new NCBIBlastHandler();
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
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Http Request was submitted successfully"));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Blast BVT: DataBase Value {0} is as expected.", queryDatabaseValue));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Blast BVT: Program Value {0} is as expected.", queryProgramValue));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Blast BVT: Query sequence {0} is as expected.", querySequence));
                webAccessor.Close();
            }
            finally
            {
                if (blastService != null)
                    ((IDisposable)blastService).Dispose();
            }
        }

        /// <summary>
        /// Validate if blast Parameter present in the dictionary.
        /// Input Data :Valid parameter name.
        /// Output Data : Validataion of IsValid() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateIsValid()
        {
            // Gets the search query parameter and their values.
            string newParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.AddparameterNode, Constants.NewParameter);
            string webUri = _utilityObj._xmlUtil.GetTextValue(
                Constants.AddparameterNode, Constants.BlastWebServiceUri);
            bool result;

            Dictionary<string, RequestParameter> parameters =
                new Dictionary<string, RequestParameter>();

            // Add a new parameter 
            parameters.Add(newParameter, new RequestParameter(
                newParameter, newParameter, false, webUri, "string", null));

            // Validate if added parameter present in the request parameter dictionary.
            RequestParameter param = parameters[newParameter];
            result = param.IsValid(newParameter);

            // Validate the Submitted request.
            Assert.IsTrue(result);
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Validation of IsValid method was completed successfully."));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Blast BVT: new parameter {0} is as expected.", newParameter));
        }

        /// <summary>
        /// Validate fetching results asynchronous.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FetchResultsAsynchronousTest()
        {
            // Gets the search query parameter and their values.
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.DatabaseValue);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.ProgramParameter);
            string expectedHitId = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.HitID);
            string expectedAccession = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.HitAccession);
            string expectedResultCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.ResultsCount);
            string expectedHitsCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.HitsCount);
            string expectedEntropyStatistics = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.EntropyStatistics);
            string expectedKappaStatistics = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.KappaStatistics);
            string expectedLambdaStatistics = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.LambdaStatistics);

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();
            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);

            TestCaseParameters parameters = new TestCaseParameters(
                Constants.FetchResultASyncTestNode, null,
                FetchResultsAsync, testCaseParms);
            object resultsObject = _TestCaseSimulator.Simulate(parameters).Result;

            Assert.IsNotNull(resultsObject);

            // Validate blast results.
            List<BlastResult> blastResults = resultsObject as List<BlastResult>;

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

            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Hit hit = record.Hits[0];
                Assert.AreEqual(hit.Accession, expectedAccession);
                Assert.AreEqual(hit.Id.ToString((IFormatProvider)null), expectedHitId);
                Assert.AreEqual(hit.Hsps.Count.ToString((IFormatProvider)null), expectedHitsCount);
                Console.WriteLine(string.Format((IFormatProvider)null,
                "Blast BVT: Hits count '{0}'.", blastResults.Count));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Blast BVT: Accession '{0}'.", hit.Accession));
                Console.WriteLine(string.Format((IFormatProvider)null, "Blast BVT: Hit Id '{0}'.", hit.Id));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));
            }
        }

        /// <summary>
        /// Validate fetching results synchronous.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results by synchronous fetching.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void FetchResultsSynchronousTest()
        {
            // Gets the search query parameter and their values.
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.DatabaseValue);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.ProgramParameter);
            string expectedHitId = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.HitID);
            string expectedAccession = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.HitAccession);
            string expectedResultCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.ResultsCount);
            string expectedHitsCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.HitsCount);
            string expectedEntropyStatistics = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.EntropyStatistics);
            string expectedKappaStatistics = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.KappaStatistics);
            string expectedLambdaStatistics = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.LambdaStatistics);

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();

            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);

            TestCaseParameters parameters = new TestCaseParameters(
                Constants.FetchResultsSyncTestNode, null,
                FetchResultsSync, testCaseParms);
            object resultsObject = _TestCaseSimulator.Simulate(parameters).Result;

            Assert.IsNotNull(resultsObject);

            // Validate blast results.
            List<BlastResult> blastResults = resultsObject as List<BlastResult>;

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
            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Hit hit = record.Hits[0];
                Assert.AreEqual(hit.Accession, expectedAccession);
                Assert.AreEqual(hit.Id.ToString((IFormatProvider)null), expectedHitId);
                Assert.AreEqual(hit.Hsps.Count.ToString((IFormatProvider)null), expectedHitsCount);
                Console.WriteLine(string.Format(null,
                    "Blast BVT: Hits count '{0}'.", blastResults.Count));
                Console.WriteLine(string.Format(null,
                    "Blast BVT: Accession '{0}'.", hit.Accession));
                Console.WriteLine(string.Format(null, "Blast BVT: Hit Id '{0}'.", hit.Id));
                Console.WriteLine(string.Format(null,
                    "Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));
            }
        }


        /// <summary>
        /// Validate blast result by passing search Query as parameter.
        /// Input Data :Valid search query, Database value and program value.
        /// Output Data : Validation of blast results by asynchronous fetching.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateBlastResultsWithQueryAsParameter()
        {
            // Gets the search query parameter and their values.
            string queryParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.QuerySequencyparameter);
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.QuerySequency);
            string queryDatabaseValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.DatabaseValue);
            string queryProgramValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.ProgramValue);
            string queryDatabaseParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.DatabaseParameter);
            string queryProgramParameter = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.ProgramParameter);
            string expectedHitId = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.HitID);
            string expectedAccession = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.HitAccession);
            string expectedResultCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.ResultsCount);
            string expectedHitsCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.HitsCount);
            string expectedEntropyStatistics = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.EntropyStatistics);
            string expectedKappaStatistics = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.KappaStatistics);
            string expectedLambdaStatistics = _utilityObj._xmlUtil.GetTextValue(
                Constants.AsynchronousResultsNode, Constants.LambdaStatistics);

            // Set Blast Parameters
            BlastParameters queryParams = new BlastParameters();
            queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
            queryParams.Add(queryProgramParameter, queryProgramValue);
            queryParams.Add(queryParameter, querySequence);

            Dictionary<string, object> testCaseParms = new Dictionary<string, object>();
            testCaseParms.Add(Constants.BlastParmsConst, queryParams);
            testCaseParms.Add(Constants.QuerySeqString, querySequence);

            TestCaseParameters parameters = new TestCaseParameters(
                Constants.FetchResultASyncTestNode, null,
                FetchResultsAsync, testCaseParms);
            object resultsObject = _TestCaseSimulator.Simulate(parameters).Result;
            Assert.IsNotNull(resultsObject);

            // Validate blast results.
            List<BlastResult> blastResults = resultsObject as List<BlastResult>;

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

            if (null != record.Hits
                    && 0 < record.Hits.Count)
            {
                Hit hit = record.Hits[0];
                Assert.AreEqual(hit.Accession, expectedAccession);
                Assert.AreEqual(hit.Id.ToString((IFormatProvider)null), expectedHitId);
                Assert.AreEqual(hit.Hsps.Count.ToString((IFormatProvider)null), expectedHitsCount);
                Console.WriteLine(string.Format((IFormatProvider)null,
               "Blast BVT: Hits count '{0}'.", blastResults.Count));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Blast BVT: Accession '{0}'.", hit.Accession));
                Console.WriteLine(string.Format((IFormatProvider)null, "Blast BVT: Hit Id '{0}'.", hit.Id));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Blast BVT: Hits Count '{0}'.", hit.Hsps.Count));
            }
        }

        /// <summary>
        /// Validate Cancelling Submitted request for Dna Sequence query.
        /// Input Data : Dna Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateNcbiCancelDnaQuerySequenceRequest()
        {
            ValidateCancelRequest(Constants.NcbiDnaSeqAsynchronousResultsNode);
        }

        /// <summary>
        /// Validate Cancelling Submitted request for Medium sized Dna Sequence query.
        /// Input Data : Medium sized Dna Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateNcbiCancelRequestForMediumSizedDnaSequence()
        {
            ValidateCancelRequest(
                Constants.NcbiBlastMediumSizeEbiDnaSequenceParametersNode);
        }

        /// <summary>
        /// Validate Cancelling Submitted request for Medium sized Protein Sequence query.
        /// Input Data : Medium sized Protein Sequence Query.
        /// Output Data : Validation of Cancelling Submitted job.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateNcbiCancelProteinQuerySequenceRequest()
        {
            ValidateCancelRequest(
                Constants.NcbiBlastMediumSizeEbiProteinSequenceParametersNode);
        }

        /// <summary>
        /// Validate NCBI Webservice Properties.
        /// Input Data : Valid Config Parameter.
        /// Output Data : Validation of Ncbi Service properties.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateNcbiWebServiceProperties()
        {
            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;

            IBlastServiceHandler service = null;
            try
            {
                service = new NCBIBlastHandler(configParams);

                // Validate NCBI Web Service properties.
                Assert.AreEqual(Constants.NcbiWebServiceDescription, service.Description);
                Assert.AreEqual(Constants.NcbiWebServiceName, service.Name);

                ApplicationLog.WriteLine(
                    "NciWebService : Successfully validated the Ncbi WebService Properties");
                Console.WriteLine(
                    "NcbiWebService : Successfully validated the Ncbi WebService Properties");
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }
        }

        #endregion Blast Bvt TestCases

        #region Support Methods

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
            IBlastServiceHandler blastService = null;
            try
            {
                blastService = new NCBIBlastHandler();
                ConfigParameters configParameters = new ConfigParameters();
                configParameters.UseBrowserProxy = true;
                blastService.Configuration = configParameters;

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
                    "Blast BVT: Query Sequence{0} is as expected.", querySequence));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Blast BVT: DataBase Value{0} is as expected.", queryDatabaseValue));
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Blast BVT: Program Value {0} is as expected.", queryProgramValue));
            }
            finally
            {
                if (blastService != null)
                    ((IDisposable)blastService).Dispose();

            }
        }

        /// <summary>
        /// Validate Cancel submitted job by passing job id.
        /// <param name="nodeName">different alphabet node name</param>
        /// </summary>
        void ValidateCancelRequest(string nodeName)
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
            IBlastServiceHandler service = null;
            try
            {
                service = new NCBIBlastHandler();
                ConfigParameters configParameters = new ConfigParameters();
                configParameters.UseBrowserProxy = true;
                service.Configuration = configParameters;
                // Create search parameters object.
                BlastParameters queryParams = new BlastParameters();

                // Set Request parameters.
                queryParams.Add(querySequenceParameter, querySequence);
                queryParams.Add(queryDatabaseParameter, queryDatabaseValue);
                queryParams.Add(queryProgramParameter, queryProgramValue);

                ISequence seq = null;

                // Create a request without passing sequence.
                string reqId = service.SubmitRequest(seq, queryParams);

                // Cancel subitted job.
                bool result = service.CancelRequest(reqId);

                // validate the cancelled job.
                Assert.IsTrue(result);

                Console.WriteLine(string.Format(null,
                    "NCBI Blast P1 : Submitted job cancelled was successfully.",
                    queryProgramValue));
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }
        }

        /// <summary>
        /// Validate request status for the submitted request for Protein sequence.
        /// </summary>
        /// <param name="blastParameters">Blast Input config parameters</param>
        /// <returns></returns>
        private TestCaseOutput GetRequestStatusForDna(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            Sequence sequence = new Sequence(Alphabets.DNA, sequenceString);

            // Set NCBIHandler configuration services
            NCBIBlastHandler blastService = null;
            object requestStatus = null;
            try
            {
                blastService = new NCBIBlastHandler();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                blastService.Configuration = configParams;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Submit NCBI request
                string reqID = blastService.SubmitRequest(sequence, blastSearchPams);
                Assert.IsFalse(string.IsNullOrEmpty(reqID));

                requestStatus = blastService.GetRequestStatus(reqID);
            }
            finally
            {
                if (blastService != null)
                    ((IDisposable)blastService).Dispose();
            }

            return new TestCaseOutput(requestStatus, false);
        }

        /// <summary>
        /// Validate request status for the submitted request for Dna Sequence.
        /// </summary>
        /// <param name="blastParameters">Blast Input config parameters</param>
        /// <returns></returns>
        private TestCaseOutput GetRequestStatusForProtein(Dictionary<string, object> blastParameters)
        {
            // Get the input query string
            string sequenceString = blastParameters[Constants.QuerySeqString] as string;
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);

            // Set NCBIHandler configuration services
            NCBIBlastHandler blastService = null;
            object requestStatus = null;
            try
            {
                blastService = new NCBIBlastHandler();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                blastService.Configuration = configParams;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Submit NCBI request
                string reqID = blastService.SubmitRequest(sequence, blastSearchPams);
                Assert.IsFalse(string.IsNullOrEmpty(reqID));

                requestStatus = blastService.GetRequestStatus(reqID);
            }
            finally
            {
                if (blastService != null)
                    ((IDisposable)blastService).Dispose();
            }

            return new TestCaseOutput(requestStatus, false);
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
            object resultsObject = null;
            NCBIBlastHandler blastService = null;
            try
            {
                blastService = new NCBIBlastHandler();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                blastService.Configuration = configParams;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Submit NCBI request
                string reqID = blastService.SubmitRequest(sequence, blastSearchPams);
                Assert.IsFalse(string.IsNullOrEmpty(reqID));

                // Get service request staus
                ServiceRequestInformation reqInfo = blastService.GetRequestStatus(reqID);

                if (reqInfo.Status != ServiceRequestStatus.Waiting && reqInfo.Status != ServiceRequestStatus.Ready)
                {
                    string err = ApplicationLog.WriteLine("Unexpected status: '{0}'", reqInfo.Status);
                    Assert.Fail(err);
                }

                // get async results, poll until ready
                int maxAttempts = 10;
                int attempt = 1;

                while (attempt <= maxAttempts
                        && reqInfo.Status != ServiceRequestStatus.Error
                        && reqInfo.Status != ServiceRequestStatus.Ready)
                {
                    ++attempt;
                    reqInfo = blastService.GetRequestStatus(reqID);
                    Thread.Sleep(
                        reqInfo.Status == ServiceRequestStatus.Waiting
                        || reqInfo.Status == ServiceRequestStatus.Queued
                        ? 20000 * attempt
                        : 0);
                }

                IBlastParser blastXmlParser = new BlastXmlParser();
                resultsObject = blastXmlParser.Parse(
                        new StringReader(blastService.GetResult(reqID, blastSearchPams)));
            }
            finally
            {
                if (blastService != null)
                    ((IDisposable)blastService).Dispose();
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
            NCBIBlastHandler blastService = null;
            IList<BlastResult> resultsObject = null;
            try
            {
                blastService = new NCBIBlastHandler();
                ConfigParameters configParams = new ConfigParameters();
                configParams.UseBrowserProxy = true;
                blastService.Configuration = configParams;

                BlastParameters blastSearchPams = blastParameters[Constants.BlastParmsConst]
                    as BlastParameters;

                // Submit NCBI request
                string reqID = blastService.SubmitRequest(sequence, blastSearchPams);
                Assert.IsFalse(string.IsNullOrEmpty(reqID));


                // Get service request staus
                ServiceRequestInformation reqInfo = blastService.GetRequestStatus(reqID);

                if (reqInfo.Status != ServiceRequestStatus.Waiting && reqInfo.Status != ServiceRequestStatus.Ready)
                {
                    string err = ApplicationLog.WriteLine("Unexpected status: '{0}'", reqInfo.Status);
                    Assert.Fail(err);
                }

                // get async results, poll until ready
                int maxAttempts = 10;
                int attempt = 1;
                while (attempt <= maxAttempts
                        && reqInfo.Status != ServiceRequestStatus.Error
                        && reqInfo.Status != ServiceRequestStatus.Ready)
                {
                    ++attempt;
                    reqInfo = blastService.GetRequestStatus(reqID);
                    Thread.Sleep(
                        reqInfo.Status == ServiceRequestStatus.Waiting
                        || reqInfo.Status == ServiceRequestStatus.Queued
                        ? 20000 * attempt
                        : 0);
                }

                resultsObject = blastService.FetchResultsSync(
                   reqID, blastSearchPams) as List<BlastResult>;
            }
            finally
            {
                if (blastService != null)
                    ((IDisposable)blastService).Dispose();
            }

            return new TestCaseOutput(resultsObject, false);
        }

        #endregion Support Methods
    }
}
