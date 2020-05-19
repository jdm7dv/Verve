// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * EbiBlastP2TestCases.cs
 * 
 * This file contains the Ebi Blast Web Service P2 test cases.
 * 
******************************************************************************/

using System;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using MBF.Web;
using MBF.Web.Blast;

using NUnit.Framework;

namespace MBF.TestAutomation.Web.EbiBlast
{
    /// <summary>
    /// Test Automation code for MBF Ebi Blast Web Service and P2 level validations.
    /// </summary>
    [TestFixture]
    public class EbiBlastP2TestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static EbiBlastP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region EbiBlast P2 Test Cases

        /// <summary>
        /// Invalidate Ebi Web Service by passing null config parameters to Ebi constructor.
        /// Input Data : Null config parameters
        /// Output Data : Invalid results  
        /// </summary>
        [Test]
        public void InvalidateBlastResultsUsingConstructorPam()
        {
            // create Ebi Blast service object.
            ConfigParameters configParams = null;

            // Validate EbiWebService ctor by passing null parser.
            try
            {
                new EbiWuBlastHandler(configParams);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "EbiWebService P2 : Successfully validated the Argument null exception");
                Console.WriteLine(
                    "EbiWebService P2 : Successfully validated the Argument null exception");
            }
        }

        /// <summary>
        /// InValidate Ebi Web Service by passing null config or null Blast parameters to Ebi constructor.
        /// Input Data : Null config parameters or null Blast parameters
        /// Output Data : Invalid results  
        /// </summary>
        [Test]
        public void InvalidateBlastResultsUsingConstructorPams()
        {
            // create Ebi Blast service object.
            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;

            // Validate EbiWebService ctor by passing null parser.
            try
            {
                new EbiWuBlastHandler(null, configParams);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "EbiWebService P2 : Successfully validated the Argument null exception");
                Console.WriteLine(
                    "EbiWebService P2 : Successfully validated the Argument null exception");
            }

            // Validate EbiWebService ctor by passing null config.
            try
            {
                new EbiWuBlastHandler(new BlastXmlParser(), null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "EbiWebService P2 : Successfully validated the Argument null exception");
                Console.WriteLine(
                    "EbiWebService P2 : Successfully validated the Argument null exception");
            }
        }

        /// <summary>
        /// Invalidate service meta data by passing null.
        /// Input Data : Null data
        /// Output Data : Invalid results
        /// </summary>
        [Test]
        public void InvalidateServiceMetaData()
        {
            // Validate ServiceMeta ctor by passing null config.
            try
            {
                new EbiWuBlastHandler().GetServiceMetadata(null);
                Assert.Fail();
            }
            catch (Exception)
            {
                ApplicationLog.WriteLine(
                    "EbiWebService P2 : Successfully validated the exception");
                Console.WriteLine(
                    "EbiWebService P2 : Successfully validated the exception");
            }
        }

        /// <summary>
        /// Invalidate Cancel request by passing null request identifier.
        /// Input Data :Invalid Request Identifier.
        /// Output Data : Invalid results 
        /// </summary>
        [Test]
        public void InvalidateEbiCancelRequest()
        {
            // Validate ServiceMeta ctor by passing null config.
            try
            {
                new EbiWuBlastHandler().CancelRequest(null);
                Assert.Fail();
            }
            catch (Exception)
            {
                ApplicationLog.WriteLine(
                    "EbiWebService P2 : Successfully validated the exception");
                Console.WriteLine(
                    "EbiWebService P2 : Successfully validated the exception");
            }
        }

        /// <summary>
        /// Validate a Ebi Blast Service Request without setting any config parameters.
        /// Input : Invalid config parameters.
        /// Output : Invalidate request status.
        /// </summary>
        [Test]
        public void InvalidateEbiWebServiceRequestStatusWithoutConfigPams()
        {
            // Gets the search query parameter and their values.
            string alphabet = Utility._xmlUtil.GetTextValue(
                Constants.EbiBlastResultsNode, Constants.AlphabetNameNode);
            string querySequence = Utility._xmlUtil.GetTextValue(
                Constants.EbiBlastResultsNode, Constants.QuerySequency);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabet),
                querySequence);

            // Set Service confiruration parameters true.
            EbiWuBlastHandler service = new EbiWuBlastHandler();

            // Dispose Ebi Blast Handler.
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
                    "EbiWebService P2 : Successfully validated the exception");
                Console.WriteLine(
                    "EbiWebService P2 : Successfully validated the exception");
            }
        }

        #endregion EbiBlast P2 Test Cases
    }
}