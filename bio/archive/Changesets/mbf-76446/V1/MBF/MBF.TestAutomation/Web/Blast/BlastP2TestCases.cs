// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * BlastP2TestCases.cs
 * 
 * This file contains the Blast P2 level Test cases.
 * 
******************************************************************************/

using System;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using MBF.Web;
using MBF.Web.Blast;

using NUnit.Framework;

namespace MBF.TestAutomation.Web.Blast
{
    /// <summary>
    /// Test Automation code for MBF Blast Web Service and P2 level validations.
    /// </summary>
    [TestFixture]
    public class BlastP2TestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static BlastP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region NcbiBlast P2 Test Cases

        /// <summary>
        /// Invalidate Ncbi Web Service by passing null config parameters to Ncbi constructor.
        /// Input Data : Null config parameters
        /// Output Data : Invalid results  
        /// </summary>
        [Test]
        public void InvalidateBlastResultsUsingConstructorPam()
        {
            // create Ncbi Blast service object.
            ConfigParameters configParams = null;

            // Validate NcbiWebService ctor by passing null parser.
            try
            {
                new NCBIBlastHandler(configParams);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "NcbiWebService P2 : Successfully validated the Argument null exception");
                Console.WriteLine(
                    "NcbiWebService P2 : Successfully validated the Argument null exception");
            }
        }

        /// <summary>
        /// Invalidate Ncbi Web Service by passing null config or null Blast parameters to Ncbi constructor.
        /// Input Data : Null config parameters or null Blast parameters
        /// Output Data : Invalid results  
        /// </summary>
        [Test]
        public void InvalidateBlastResultsUsingConstructorPams()
        {
            // create Ncbi Blast service object.
            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;

            // Validate NcbiWebService ctor by passing null parser.
            try
            {
                new NCBIBlastHandler(null, configParams);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "NcbiWebService P2 : Successfully validated the Argument null exception");
                Console.WriteLine(
                    "NcbiWebService P2 : Successfully validated the Argument null exception");
            }

            // Validate NcbiWebService ctor by passing null config.
            try
            {
                new NCBIBlastHandler(new BlastXmlParser(), null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "NcbiWebService P2 : Successfully validated the Argument null exception");
                Console.WriteLine(
                    "NcbiWebService P2 : Successfully validated the Argument null exception");
            }
        }

        /// <summary>
        /// Invalidate Cancel request by passing null request identifier.
        /// Input Data :Invalid Request Identifier.
        /// Output Data : Invalid results 
        /// </summary>
        [Test]
        public void InvalidateNcbiCancelRequest()
        {

            // Validate ServiceMeta ctor by passing null config.
            try
            {
                new NCBIBlastHandler().CancelRequest(null);
                Assert.Fail();
            }
            catch (Exception)
            {
                ApplicationLog.WriteLine(
                    "NcbiWebService P2 : Successfully validated the exception");
                Console.WriteLine(
                    "NcbiWebService P2 : Successfully validated the exception");
            }
        }

        /// <summary>
        /// Validate a Ncbi Blast Service Request without setting any config parameters.
        /// Input : Invalid config parameters.
        /// Output : Invalidate request status.
        /// </summary>
        [Test]
        public void InvalidateNcbiWebServiceRequestStatusWithoutConfigPams()
        {
            // Gets the search query parameter and their values.
            string alphabet = Utility._xmlUtil.GetTextValue(
                Constants.EbiBlastResultsNode, Constants.AlphabetNameNode);
            string querySequence = Utility._xmlUtil.GetTextValue(
                Constants.EbiBlastResultsNode, Constants.QuerySequency);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabet), querySequence);

            // Set Service confiruration parameters true.
            NCBIBlastHandler service = new NCBIBlastHandler();

            // Dispose Ncbi Blast Handler.
            service.Dispose();

            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;
            service.Configuration = configParams;
            BlastParameters searchParams = new BlastParameters();

            // Get Request identifier from web service.
            try
            {
                service.SubmitRequest(seq, searchParams);
                Assert.Fail();
            }
            catch (Exception)
            {
                ApplicationLog.WriteLine(
                    "NcbiWebService P2 : Successfully validated the exception");
                Console.WriteLine(
                    "NcbiWebService P2 : Successfully validated the exception");
            }
        }

        #endregion NcbiBlast P2 Test Cases
    }
}
