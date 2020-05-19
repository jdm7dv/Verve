// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * AzureBlastP2TestCases.cs
 * 
 * This file contains the Azure Blast Web Service P2 test cases.
 * 
******************************************************************************/

using System;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using MBF.Web;
using MBF.Web.Blast;

using NUnit.Framework;

namespace MBF.TestAutomation.Web.AzureBlast
{
    /// <summary>
    /// Test Automation code for MBF Azure Blast Web Service and P2 level validations.
    /// </summary>
    public class AzureBlastP2TestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static AzureBlastP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\AzureBlastConfig.xml");
        }

        #endregion Constructor

        #region Azure Blast P2 Test Cases

        /// <summary>
        /// Validate the Azure Blast Service Request status Queued.
        /// Input : Invalid request parameters.
        /// Output : Invalidate request status.
        /// </summary>
        public void InvalidateAzureWebServiceRequestStatus()
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

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabet), querySequence);

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            configParams.DefaultTimeout = 1;
            configParams.RetryInterval = 10;
            configParams.RetryCount = 1;
            configParams.Connection = new Uri(Constants.AzureUri);

            // Set Service confiruration parameters true.
            IBlastServiceHandler service = new AzureBlastHandler(configParams);

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
            }
            catch (Exception ex)
            {
                ApplicationLog.WriteLine(string.Format(
                    "AzureWebService P2 : Connection Failed with the error '{0}'", ex.Message));
                Console.WriteLine(string.Format(
                    "AzureWebService P2 : Connection Failed with the error '{0}'", ex.Message));
                Assert.Ignore("Test case ignored due to connection failure");
            }

            try
            {
                object responseResults = service.FetchResultsSync(reqId, searchParams);
                Assert.IsNotNull(responseResults);
                Assert.Fail();
            }
            catch (Exception)
            {
                ApplicationLog.WriteLine("AzureWebService P2 : Successfully validated the exception");
                Console.WriteLine("AzureWebService P2 : Successfully validated the exception");
            }
        }

        /// <summary>
        /// Validate a Azure Blast Service constructor with invalid parameters.
        /// Input : Invalid service config parameters.
        /// Output : Invalidate Azure web service constructor.
        /// </summary>
        public void InvalidateAzureWebHandlerCtor()
        {
            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            configParams.DefaultTimeout = 10;
            configParams.Connection = new Uri(Constants.AzureUri);
            BlastXmlParser parser = new BlastXmlParser();

            // Validate AzureWebService ctor by passing null parser.
            try
            {
                IBlastServiceHandler service = new AzureBlastHandler(null, configParams);
                Assert.IsNotNull(service);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "AzureWebService P2 : Successfully validated the Argument null exception");
                Console.WriteLine(
                    "AzureWebService P2 : Successfully validated the Argument null exception");
            }

            // Validate AzureWebService ctor by passing null configuration parameters.
            try
            {
                IBlastServiceHandler service = new AzureBlastHandler(parser, null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "AzureWebService P2 : Successfully validated the Argument null exception");
                Console.WriteLine(
                    "AzureWebService P2 : Successfully validated the Argument null exception");
            }
        }

        /// <summary>
        /// Validate a Azure Blast Service Request status for invalid Sequence.
        /// Input : Invalid sequence.
        /// Output : Valdiate request status for invalid sequence.
        /// </summary>
        public void InvalidateAzureWebServiceRequestStatusForInvalidConfigPams()
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

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabet), querySequence);

            // Set Service confiruration parameters true.
            AzureBlastHandler service = new AzureBlastHandler();

            // Dispose Azure Blast Handler.
            service.Dispose();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            configParams.DefaultTimeout = 10;
            configParams.Connection = new Uri(Constants.AzureUri);
            service.Configuration = configParams;

            // Create search parameters object.
            BlastParameters searchParams = new BlastParameters();

            // Add mandatory parameter values to search query parameters.
            searchParams.Add(queryProgramParameter, queryProgramValue);
            searchParams.Add(queryDatabaseParameter, queryDatabaseValue);

            // Get Request identifier from web service.
            try
            {
                service.SubmitRequest(seq, null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "AzureWebService P2 : Successfully validated the exception");
                Console.WriteLine(
                    "AzureWebService P2 : Successfully validated the exception");
            }
        }

        /// <summary>
        /// Validate a Azure Blast Service Request without setting any config parameters.
        /// Input : Invalid config parameters.
        /// Output : Invalidate request status.
        /// </summary>
        public void InvalidateAzureWebServiceRequestStatusWithoutConfigPams()
        {
            // Gets the search query parameter and their values.
            string alphabet = Utility._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.AlphabetNameNode);
            string querySequence = Utility._xmlUtil.GetTextValue(
                Constants.AzureBlastResultsNode, Constants.QuerySequency);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabet), querySequence);
            string reqId = string.Empty;

            // Set Service confiruration parameters true.
            AzureBlastHandler service = new AzureBlastHandler();

            // Dispose Azure Blast Handler.
            service.Dispose();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            configParams.DefaultTimeout = 1;
            configParams.Connection = new Uri(Constants.AzureUri);
            service.Configuration = configParams;
            BlastParameters searchParams = new BlastParameters();

            // Get Request identifier from web service.
            try
            {
                reqId = service.SubmitRequest(seq, searchParams);
                Assert.IsNotEmpty(reqId);
                Assert.Fail();
            }
            catch (Exception)
            {
                ApplicationLog.WriteLine("AzureWebService P2 : Successfully validated the exception");
                Console.WriteLine("AzureWebService P2 : Successfully validated the exception");
            }
        }

        #endregion Azure Blast P2 Test Cases
    }
}
