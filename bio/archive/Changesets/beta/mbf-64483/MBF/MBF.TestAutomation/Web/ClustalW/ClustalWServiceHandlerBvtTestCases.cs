// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * ClustalWServiceHandlerBvtTestCases.cs
 * 
 * This file contains the Bio HPC Web Service BVT test cases.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using MBF.Algorithms.Alignment;

using MBF.IO.ClustalW;
using MBF.IO.Fasta;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using MBF.Web;
using MBF.Web.ClustalW;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.Web.ClustalW
{
    /// <summary>
    /// BVT test cases to validate ClustalW service integartion classes
    /// </summary>  
    [TestClass]
    public class ClustalWServiceHandlerBvtTestCases
    {

        #region Global Variables

        AutoResetEvent _resetEvent = new AutoResetEvent(false);

        #endregion Global Variables

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\ClustalWTestData\ClustalWTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static ClustalWServiceHandlerBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion

        #region Test Cases

        /// <summary>
        /// Validate the FetchResultSync() using multiple input sequences
        /// Input : 4 dna sequences
        /// Output : aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFetchResultSync()
        {
            ValidateFetchResultSync(Constants.DefaultOptionNode);
        }

        /// <summary>
        /// Validate the FetchResultAsync() using multiple input sequences
        /// Input : 4 dna sequences
        /// Output : aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFetchResultAsync()
        {
            ValidateFetchResultAsync(Constants.DefaultOptionNode);
        }

        /// <summary>
        /// Validate GetResults using event handler with multiple input sequences
        /// Input : 4 dna sequences
        /// Output : aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateFetchResultUsingEvent()
        {
            ValidateFetchResultsUsingEvent(Constants.DefaultOptionNode);
        }

        /// <summary>
        /// Validate Cancel Request after submitting job
        /// Input: Submit job and start the service
        /// Output: job is cancelled
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateCancelRequest()
        {
            ValidateCancelRequest(Constants.DefaultOptionNode);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Validate Submit Job and Fetch ResultSync() using multiple input sequences
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        void ValidateFetchResultSync(string nodeName)
        {
            // Read input from config file
            string filepath = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string emailId = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.EmailIDNode);
            string clusterOption = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.ClusterOptionNode);
            string actionAlign = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.ActionAlignNode);

            // Initialize with parser and config params
            ConfigParameters configparams = new ConfigParameters();
            ClustalWParser clustalparser = new ClustalWParser();
            configparams.UseBrowserProxy = true;
            TestIClustalWServiceHandler handler =
              new TestIClustalWServiceHandler(clustalparser, configparams);
            ClustalWParameters parameters = new ClustalWParameters();
            parameters.Values[ClustalWParameters.Email] = emailId;
            parameters.Values[ClustalWParameters.ClusterOption] = clusterOption;
            parameters.Values[ClustalWParameters.ActionAlign] = actionAlign;

            IList<ISequence> sequence = null;

            // Get the input sequences
            using (FastaParser parser = new FastaParser())
            {
                sequence = parser.Parse(filepath);
            }

            // Submit job and validate it returned valid job id and control id 
            ServiceParameters svcparameters =
              handler.SubmitRequest(sequence, parameters);
            Assert.IsTrue(string.IsNullOrEmpty(svcparameters.JobId));
            Console.WriteLine(string.Concat("JobId", svcparameters.JobId));
            ApplicationLog.WriteLine(string.Concat("JobId", svcparameters.JobId));
            foreach (string key in svcparameters.Parameters.Keys)
            {
                Assert.IsTrue(string.IsNullOrEmpty(svcparameters.Parameters[key].ToString()));
                Console.WriteLine(string.Format((IFormatProvider)null, "{0}:{1}",
                  key, svcparameters.Parameters[key].ToString()));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null, "{0}:{1}",
                  key, svcparameters.Parameters[key].ToString()));
            }

            // Get the results and validate it is not null.
            ClustalWResult result = handler.FetchResultsSync(svcparameters);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.SequenceAlignment);
            foreach (IAlignedSequence alignSeq in result.SequenceAlignment.AlignedSequences)
            {
                Console.WriteLine("Aligned Sequence Sequences :");
                ApplicationLog.WriteLine("Aligned Sequence Sequences :");
                foreach (ISequence seq in alignSeq.Sequences)
                {
                    Console.WriteLine(string.Concat("Sequence:", seq.ToString()));
                    ApplicationLog.WriteLine(string.Concat("Sequence:", seq.ToString()));
                }
            }
            Console.WriteLine(@"ClustalWServiceHandler BVT : Submit job and Get Results is 
      successfully completed using FetchResultSync()");
            ApplicationLog.WriteLine(@"ClustalWServiceHandler BVT : Submit job and Get Results 
      is successfully completed using FetchResultSync()");
        }

        /// <summary>
        /// Validate submit job and FetchResultAsync() using multiple input sequences
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        void ValidateFetchResultAsync(string nodeName)
        {
            // Read input from config file
            string filepath = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.FilePathNode);
            string emailId = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.EmailIDNode);
            string clusterOption = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.ClusterOptionNode);
            string actionAlign = _utilityObj._xmlUtil.GetTextValue(
              nodeName, Constants.ActionAlignNode);

            ConfigParameters configparams = new ConfigParameters();
            ClustalWParser clustalparser = new ClustalWParser();
            configparams.UseBrowserProxy = true;
            TestIClustalWServiceHandler handler =
              new TestIClustalWServiceHandler(clustalparser, configparams);

            ClustalWParameters parameters = new ClustalWParameters();
            parameters.Values[ClustalWParameters.Email] = emailId;
            parameters.Values[ClustalWParameters.ClusterOption] = clusterOption;
            parameters.Values[ClustalWParameters.ActionAlign] = actionAlign;

            IList<ISequence> sequence = null;
            // Get input sequences
            using (FastaParser parser = new FastaParser())
            {
                sequence = parser.Parse(filepath);
            }

            // Submit job and validate it returned valid job id and control id 
            ServiceParameters svcparameters = handler.SubmitRequest(sequence, parameters);
            Assert.IsTrue(string.IsNullOrEmpty(svcparameters.JobId));
            Console.WriteLine(string.Concat("JobId:", svcparameters.JobId));
            foreach (string key in svcparameters.Parameters.Keys)
            {
                Assert.IsTrue(string.IsNullOrEmpty(svcparameters.Parameters[key].ToString()));
                Console.WriteLine(string.Format((IFormatProvider)null, "{0}:{1}",
                  key, svcparameters.Parameters[key].ToString()));
            }

            // Get the results and validate it is not null.
            ClustalWResult result = null;
            int retrycount = 0;
            ServiceRequestInformation info;
            do
            {
                info = handler.GetRequestStatus(svcparameters);
                if (info.Status == ServiceRequestStatus.Ready)
                {
                    break;
                }

                Thread.Sleep(
                    info.Status == ServiceRequestStatus.Waiting
                    || info.Status == ServiceRequestStatus.Queued ?
                    Constants.ClusterRetryInterval * retrycount : 0);

                retrycount++;
            }
            while (retrycount < 10);

            if (info.Status == ServiceRequestStatus.Ready)
            {
                result = handler.FetchResultsAsync(svcparameters);
            }
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.SequenceAlignment);
            foreach (IAlignedSequence alignSeq in result.SequenceAlignment.AlignedSequences)
            {
                Console.WriteLine("Aligned Sequence Sequences : ");
                ApplicationLog.WriteLine("Aligned Sequence Sequences : ");
                foreach (ISequence seq in alignSeq.Sequences)
                {
                    Console.WriteLine(string.Concat("Sequence:", seq.ToString()));
                    ApplicationLog.WriteLine(string.Concat("Sequence:", seq.ToString()));
                }
            }
            Console.WriteLine(@"ClustalWServiceHandler BVT : Submit job and Get Results is 
      successfully completed using FetchResultAsync()");
            ApplicationLog.WriteLine(@"ClustalWServiceHandler BVT : Submit job and Get Results 
      is successfully completed using FetchResultAsync()");
        }

        /// <summary>
        /// Validate submit job and Get Results using event handler
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        void ValidateFetchResultsUsingEvent(string nodeName)
        {
            // Read input from config file
            string filepath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string emailId = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.EmailIDNode);
            string clusterOption = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.ClusterOptionNode);
            string actionAlign = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.ActionAlignNode);

            ClustalWParameters parameters = new ClustalWParameters();
            parameters.Values[ClustalWParameters.Email] = emailId;
            parameters.Values[ClustalWParameters.ClusterOption] = clusterOption;
            parameters.Values[ClustalWParameters.ActionAlign] = actionAlign;

            IList<ISequence> sequence = null;

            // Get the input sequences
            using (FastaParser parser = new FastaParser())
            {
                sequence = parser.Parse(filepath);
            }

            // Register event and submit request
            ConfigParameters configparams = new ConfigParameters();
            ClustalWParser clustalparser = new ClustalWParser();
            configparams.UseBrowserProxy = true;
            TestIClustalWServiceHandler handler =
              new TestIClustalWServiceHandler(clustalparser, configparams);
            handler.RequestCompleted +=
              new EventHandler<ClustalWCompletedEventArgs>(handler_RequestCompleted);
            ServiceParameters svcparams = handler.SubmitRequest(sequence, parameters);
            WaitHandle[] aryHandler = new WaitHandle[1];
            aryHandler[0] = _resetEvent;
            WaitHandle.WaitAny(aryHandler);

            // Validate the submit job results
            Assert.IsTrue(string.IsNullOrEmpty(svcparams.JobId));
            Console.WriteLine("JobId:" + svcparams.JobId);
            foreach (string key in svcparams.Parameters.Keys)
            {
                Assert.IsTrue(string.IsNullOrEmpty(svcparams.Parameters[key].ToString()));
                Console.WriteLine(string.Format((IFormatProvider)null, "{0} : {1}",
                  key, svcparams.Parameters[key].ToString()));
            }
        }

        /// <summary>
        /// Validate the results using RequestCompleted event
        /// </summary>
        /// <param name="sender">ClustalW</param>
        /// <param name="e"></param>
        void handler_RequestCompleted(object sender, ClustalWCompletedEventArgs e)
        {
            // Validate the get results
            Assert.IsNotNull(e.SearchResult.SequenceAlignment);
            foreach (IAlignedSequence alignSeq in e.SearchResult.SequenceAlignment.AlignedSequences)
            {
                Console.WriteLine("Aligned Sequence Sequences :");
                ApplicationLog.WriteLine("Aligned Sequence Sequences :");
                foreach (ISequence seq in alignSeq.Sequences)
                {
                    Console.WriteLine(string.Concat("Sequence:", seq.ToString()));
                    ApplicationLog.WriteLine(string.Concat("Sequence:", seq.ToString()));
                }
            }

            _resetEvent.Set();

            Console.WriteLine(@"ClustalWServiceHandler BVT : Submit job and Get Results is 
      successfully completed using event");
            ApplicationLog.WriteLine(@"ClustalWServiceHandler BVT : Submit job and Get Results 
      is successfully completed using event");
        }

        /// <summary>
        /// Validate the CancelRequest()
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        void ValidateCancelRequest(string nodeName)
        {
            // Read input from config file
            string filepath = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.FilePathNode);
            string emailId = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.EmailIDNode);
            string clusterOption = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.ClusterOptionNode);
            string actionAlign = _utilityObj._xmlUtil.GetTextValue(nodeName,
              Constants.ActionAlignNode);

            ClustalWParameters parameters = new ClustalWParameters();
            parameters.Values[ClustalWParameters.Email] = emailId;
            parameters.Values[ClustalWParameters.ClusterOption] = clusterOption;
            parameters.Values[ClustalWParameters.ActionAlign] = actionAlign;

            IList<ISequence> sequence = null;
            // Get the input sequences
            using (FastaParser parser = new FastaParser())
            {
                sequence = parser.Parse(filepath);
            }

            // Submit job and cancel job
            // Validate cancel job is working as expected
            ConfigParameters configparams = new ConfigParameters();
            ClustalWParser clustalparser = new ClustalWParser();
            configparams.UseBrowserProxy = true;
            TestIClustalWServiceHandler handler =
              new TestIClustalWServiceHandler(clustalparser, configparams);
            ServiceParameters svcparams = handler.SubmitRequest(sequence, parameters);
            bool result = handler.CancelRequest(svcparams);

            Assert.IsTrue(result);
            Console.WriteLine(string.Concat("JobId:", svcparams.JobId));
            ApplicationLog.WriteLine(string.Concat("JobId:", svcparams.JobId));
            Assert.IsTrue(string.IsNullOrEmpty(svcparams.JobId));
            foreach (string key in svcparams.Parameters.Keys)
            {
                Assert.IsTrue(string.IsNullOrEmpty(svcparams.Parameters[key].ToString()));
                Console.WriteLine(string.Format((IFormatProvider)null, "{0} : {1}",
                  key, svcparams.Parameters[key].ToString()));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null, "{0} : {1}",
                  key, svcparams.Parameters[key].ToString()));
            }

            Console.WriteLine(
              "ClustalWServiceHandler BVT : Cancel job is submitted as expected");
            ApplicationLog.WriteLine(
              "ClustalWServiceHandler BVT : Cancel job is submitted as expected");
        }

        #endregion
    }
}
