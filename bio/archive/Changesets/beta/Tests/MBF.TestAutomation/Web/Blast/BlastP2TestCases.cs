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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Runtime.Serialization;

namespace MBF.TestAutomation.Web.Blast
{
    /// <summary>
    /// Test Automation code for MBF Blast Web Service and P2 level validations.
    /// </summary>
    [TestClass]
    public class BlastP2TestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

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
        }

        #endregion Constructor

        #region NcbiBlast P2 Test Cases

        /// <summary>
        /// Invalidate Ncbi Web Service by passing null config parameters to Ncbi constructor.
        /// Input Data : Null config parameters
        /// Output Data : Invalid results  
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateBlastResultsUsingConstructorPam()
        {
            // create Ncbi Blast service object.
            ConfigParameters configParams = null;
            IBlastServiceHandler service = null;
            // Validate NcbiWebService ctor by passing null parser.
            try
            {
                service = new NCBIBlastHandler(configParams);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "NcbiWebService P2 : Successfully validated the Argument null exception");
                Console.WriteLine(
                    "NcbiWebService P2 : Successfully validated the Argument null exception");
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }

        }

        /// <summary>
        /// Invalidate Ncbi Web Service by passing null config or null Blast parameters to Ncbi constructor.
        /// Input Data : Null config parameters or null Blast parameters
        /// Output Data : Invalid results  
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateBlastResultsUsingConstructorPams()
        {
            // create Ncbi Blast service object.
            ConfigParameters configParams = new ConfigParameters();
            configParams.UseBrowserProxy = true;

            NCBIBlastHandler service = null;
            // Validate NcbiWebService ctor by passing null parser.
            try
            {
                service = new NCBIBlastHandler(null, configParams);
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
                service = new NCBIBlastHandler(null, configParams);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "NcbiWebService P2 : Successfully validated the Argument null exception");
                Console.WriteLine(
                    "NcbiWebService P2 : Successfully validated the Argument null exception");
            }
            finally
            {
                if (service != null)
                    service.Dispose();
            }
        }

        /// <summary>
        /// Invalidate Cancel request by passing null request identifier.
        /// Input Data :Invalid Request Identifier.
        /// Output Data : Invalid results 
        /// </summary>
        /// Suppressing the Error "DoNotCatchGeneralExceptionTypes" because the exception is being thrown by DEV code
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateNcbiCancelRequest()
        {
            IBlastServiceHandler service = null;
            // Validate ServiceMeta ctor by passing null config.
            try
            {
                service = new NCBIBlastHandler();
                service.CancelRequest(null);
                Assert.Fail();
            }
            catch (Exception)
            {
                ApplicationLog.WriteLine(
                    "NcbiWebService P2 : Successfully validated the exception");
                Console.WriteLine(
                    "NcbiWebService P2 : Successfully validated the exception");
            }
            finally
            {
                if (service != null)
                    ((IDisposable)service).Dispose();
            }
        }

        /// <summary>
        /// Validate a Ncbi Blast Service Request without setting any config parameters.
        /// Input : Invalid config parameters.
        /// Output : Invalidate request status.
        /// </summary>
        /// Suppressing the Error "DoNotCatchGeneralExceptionTypes" because the exception is being thrown by DEV code
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InvalidateNcbiWebServiceRequestStatusWithoutConfigPams()
        {
            // Gets the search query parameter and their values.
            string alphabet = _utilityObj._xmlUtil.GetTextValue(
                Constants.EbiBlastResultsNode, Constants.AlphabetNameNode);
            string querySequence = _utilityObj._xmlUtil.GetTextValue(
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

        /// <summary>
        /// Invalidate GetObjectData() method.
        /// Input Data : Null value.
        /// Output Data : Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateBlastXmlMetadataGetObjectData()
        {
            SerializationInfo info = null;
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            BlastXmlMetadata bxmObj = new BlastXmlMetadata();

            try
            {
                bxmObj.GetObjectData(info, context);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Ncbi Blast P2 : InValidated the GetObjectData() method successfully.");
                ApplicationLog.WriteLine("Ncbi Blast P2 : InValidated the GetObjectData() method successfully.");
            }
        }

        /// <summary>
        /// Invalidate GetObjectData() method.
        /// Input Data : Null value.
        /// Output Data : Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateHspMetadataGetObjectData()
        {
            SerializationInfo info = null;
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            Hsp hspObj = new Hsp();

            try
            {
                hspObj.GetObjectData(info, context);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Ncbi Blast P2 : InValidated the GetObjectData() method of Hsp successfully.");
                ApplicationLog.WriteLine("Ncbi Blast P2 : InValidated the GetObjectData() method of Hsp successfully.");
            }
        }


        /// <summary>
        /// Invalidate GetObjectData() method.
        /// Input Data : Null value.
        /// Output Data : Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateBlastResultMetadataGetObjectData()
        {
            SerializationInfo info = null;
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            BlastResult brObj = new BlastResult();

            try
            {
                brObj.GetObjectData(info, context);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Ncbi Blast P2 : InValidated the GetObjectData() method of BlastResult successfully.");
                ApplicationLog.WriteLine("Ncbi Blast P2 : InValidated the GetObjectData() method of BlastResult successfully.");
            }
        }

        /// <summary>
        /// Invalidate GetObjectData() method.
        /// Input Data : Null value.
        /// Output Data : Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateBlastSearchRecordMetadataGetObjectData()
        {
            SerializationInfo info = null;
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            BlastSearchRecord bsrObj = new BlastSearchRecord();

            try
            {
                bsrObj.GetObjectData(info, context);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Ncbi Blast P2 : InValidated the GetObjectData() method of BlastSearchRecord successfully.");
                ApplicationLog.WriteLine("Ncbi Blast P2 : InValidated the GetObjectData() method of BlastSearchRecord successfully.");
            }
        }

        /// <summary>
        /// Invalidate GetObjectData() method.
        /// Input Data : Null value.
        /// Output Data : Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateBlastStatisticsGetObjectData()
        {
            SerializationInfo info = null;
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            BlastStatistics bsObj = new BlastStatistics();

            try
            {
                bsObj.GetObjectData(info, context);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Ncbi Blast P2 : InValidated the GetObjectData() method of BlastStatistics successfully.");
                ApplicationLog.WriteLine("Ncbi Blast P2 : InValidated the GetObjectData() method of BlastStatistics successfully.");
            }
        }

        /// <summary>
        /// Invalidate GetObjectData() method.
        /// Input Data : Null value.
        /// Output Data : Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateHitMetadataGetObjectData()
        {
            SerializationInfo info = null;
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            Hit hitObj = new Hit();

            try
            {
                hitObj.GetObjectData(info, context);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Ncbi Blast P2 : InValidated the GetObjectData() method of Hit successfully.");
                ApplicationLog.WriteLine("Ncbi Blast P2 : InValidated the GetObjectData() method of Hit successfully.");
            }
        }

        #endregion NcbiBlast P2 Test Cases
    }
}
