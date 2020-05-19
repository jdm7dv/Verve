// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using MBF.Registration;
using MBF.Util;
using MBF.WebServiceHandlers.Properties;

namespace MBF.Web.Blast
{
    /// <summary>
    /// This class implements IBlastService interface and defines all the atomic
    /// operation required by the interface. Each method necessarily 
    /// invokes/instantiates an atomic operation on the server (Ebi Wu server).
    /// </summary>
    [RegistrableAttribute(true)]
    public class EbiWuBlastHandler : IBlastServiceHandler, IDisposable
    {
        #region Constants

        /// <summary>
        /// Default interval of time in seconds to check the status of job
        /// </summary>
        private const int DefaultRetryInterval = 10000;

        /// <summary>
        /// Default number of retries to be made to check the status
        /// </summary>
        private const int DefaultNoOfRetries = 10;

        /// <summary>
        /// Job status is Queued
        /// </summary>
        private const string StatusPending = "PENDING";

        /// <summary>
        /// Job status is Running
        /// </summary>
        private const string StatusRunning = "RUNNING";

        /// <summary>
        /// Job Status is Completed successfully
        /// </summary>
        private const string StatusDone = "DONE";

        /// <summary>
        /// Database parameter
        /// </summary>
        private const string ParameterDatabase = "DATABASE";

        /// <summary>
        /// Program parameter
        /// </summary>
        private const string ParameterProgram = "PROGRAM";

        /// <summary>
        /// EMAIL parameter
        /// </summary>
        private const string ParameterEmail = "EMAIL";

        /// <summary>
        /// FILTER parameter
        /// </summary>
        private const string ParameterFilter = "FILTER";

        /// <summary>
        /// Number of alignments to return parameter
        /// </summary>
        private const string ParameterAlignments = "ALIGNMENTS";

        /// <summary>
        /// Similarity Matrix name parameter
        /// </summary>
        private const string ParameterMatrixName = "MATRIX_NAME";

        /// <summary>
        /// Expect value parameter
        /// </summary>
        private const string ParameterExpect = "EXPECT";

        /// <summary>
        /// Type of input provided to blast service
        /// </summary>
        private const string SequenceType = "sequence";

        /// <summary>
        /// Xml output type
        /// </summary>
        private const string AppXmlYes = "yes";

        /// <summary>
        /// Databases meta data type
        /// (Gets the list of databases)
        /// </summary>
        public const string MetadataDatabases = "Databases";

        /// <summary>
        /// Filters meta data type
        /// (Gets the list of filters)
        /// </summary>
        public const string MetadataFilter = "Filters";

        /// <summary>
        /// Matrices meta data type
        /// (Gets the list of matrices)
        /// </summary>
        public const string MetadataMatrices = "Matrices";

        /// <summary>
        /// Programs meta data type
        /// (Gets the list of programs)
        /// </summary>
        public const string MetadataPrograms = "Programs";

        /// <summary>
        /// Sensitivity meta data type
        /// (Gets the list of sensitivity)
        /// </summary>
        public const string MetadataSensitivity = "Sensitivity";

        /// <summary>
        /// Sort meta data type
        /// (Gets the list of sort supported)
        /// </summary>
        public const string MetadataSort = "Sort";

        /// <summary>
        /// Stats meta data type
        /// (Gets the list of Statistics)
        /// </summary>
        public const string MetadataStatistics = "Stats";

        /// <summary>
        /// XmlFormats meta data type
        /// (Gets the list of xml formats)
        /// </summary>
        public const string MetadataXmlFormats = "XmlFormats";

        #endregion

        #region Member Variables

        /// <summary>
        /// WSWUB blast client object
        /// </summary>
        private WSWUBlastService _blastClient;

        /// <summary>
        /// Parser object that can parse the Blast Output
        /// </summary>
        private IBlastParser _blastParser;

        /// <summary>
        /// Background worker thread that tracks the status of job and notifies
        /// user on completion.
        /// </summary>
        private BackgroundWorker _workerThread;

        /// <summary>
        /// Settings for web access, such as user-agent string and 
        /// proxy configuration
        /// </summary>
        private ConfigParameters _configuration;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the EbiWuBlastHandler class.
        /// </summary>
        /// <param name="parser">Parser to parse the Blast output</param>
        /// <param name="configurations">Configuration Parameters</param>
        public EbiWuBlastHandler(
                IBlastParser parser,
                ConfigParameters configurations)
        {
            if (null == parser)
            {
                throw new ArgumentNullException("parser");
            }

            if (null == configurations)
            {
                throw new ArgumentNullException("configurations");
            }

            Configuration = configurations;
            _blastParser = parser;
        }

        /// <summary>
        /// Initializes a new instance of the EbiWuBlastHandler class.
        /// </summary>
        /// <param name="configurations">Configuration Parameters</param>
        public EbiWuBlastHandler(ConfigParameters configurations)
            : this(new BlastXmlParser(), configurations)
        {
        }

        /// <summary>
        /// Initializes a new instance of the EbiWuBlastHandler class.
        /// </summary>
        public EbiWuBlastHandler()
            : this(new BlastXmlParser(), new ConfigParameters())
        {
        }

        #endregion

        #region Events

        /// <summary>
        /// This event is raised when Blast search is complete. It could be either a success or failure.
        /// </summary>
        public event EventHandler<RequestCompletedEventArgs> RequestCompleted;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets settings for web access, such as user-agent string and 
        /// proxy configuration
        /// </summary>
        public ConfigParameters Configuration
        {
            get
            {
                return _configuration;
            }

            set
            {
                _configuration = value;
                InitializeConfiguration();
                InitializeBlastClient();
            }
        }

        /// <summary>
        /// Gets user-friendly implementation name
        /// </summary>
        public string Name
        {
            get { return Resources.EBIWUBLAST_NAME; }
        }

        /// <summary>
        /// Gets user-friendly implementation description
        /// </summary>
        public string Description
        {
            get { return Resources.EBIWUBLAST_DESCRIPTION; }
        }

        /// <summary>
        /// Gets an instance of object that can parse the Blast Output
        /// </summary>
        public IBlastParser Parser
        {
            get { return _blastParser; }
        }

        /// <summary>
        /// Gets or sets the number of seconds between retries when a service request is pending. (This
        /// specifies the first interval, and subsequent retries occur at increasing multiples.)
        /// The caller can override the default by setting ConfigurationParameters.RetryInterval.
        /// </summary>
        private int RetryInterval { get; set; }

        /// <summary>
        /// Gets or sets the number of times to retry when a service request is pending. The caller
        /// can override the default value by setting ConfigurationParameters.RetryCount.
        /// </summary>
        private int RetryCount { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Submit the search request with the user supplied configuration parameters 
        /// and sequence. Implementation should make use of the MBF.IO formatters 
        /// to convert the sequence into the web interface compliant sequence format.
        /// This method performs parameter validation and throw Exception on invalid input.
        /// </summary>
        /// <remarks>An exception is thrown if the request does not succeed.</remarks>
        /// <param name="sequence">The sequence to search with</param>
        /// <param name="parameters">Blast input parameters</param>
        /// <returns>Request Identifier</returns>
        public string SubmitRequest(ISequence sequence, BlastParameters parameters)
        {
            if (null == sequence)
            {
                throw new ArgumentNullException("sequence");
            }

            if (null == parameters)
            {
                throw new ArgumentNullException("parameters");
            }

            string requestIdentifier = string.Empty;

            // Validate the Parameter
            ParameterValidationResult valid = ValidateParameters(parameters);
            if (!valid.IsValid)
            {
                throw new Exception(valid.ValidationErrors);
            }

            // Submit the job to server
            inputParams blastRequest = GetRequestParameter(parameters);

            blastRequest.appxml = AppXmlYes;
            blastRequest.async = true;

            data[] mydata = new data[1];
            mydata[0] = new data();
            mydata[0].type = SequenceType;
            mydata[0].content = sequence.ToString();

            requestIdentifier = _blastClient.runWUBlast(blastRequest, mydata);

            // Only if the event is registered, invoke the thread
            if (null != RequestCompleted)
            {
                ThreadParameter threadParameter = new ThreadParameter(
                        requestIdentifier,
                        sequence,
                        parameters);

                // Start the BackGroundThread to check the status of job
                _workerThread = new BackgroundWorker();
                _workerThread.WorkerSupportsCancellation = true;
                _workerThread.DoWork += new DoWorkEventHandler(ProcessRequestThread);
                _workerThread.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompletedRequestThread);
                _workerThread.RunWorkerAsync(threadParameter);
            }

            return requestIdentifier;
        }

        /// <summary>
        /// Submit the search request with the user supplied configuration parameters and sequence
        /// Implementation should make use of the MBF.IO formatters to convert the sequence into 
        /// the web interface compliant sequence format
        /// </summary>
        /// <remarks>An exception is thrown if the request does not succeed.</remarks>
        /// <param name="sequences">List of sequence to search with</param>
        /// <param name="parameters">Blast input parameters</param>
        /// <returns>Request Identifier</returns>
        public string SubmitRequest(IList<ISequence> sequences, BlastParameters parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return the status of a submitted job.
        /// </summary>
        /// <param name="requestIdentifier">Identifier for the request of interest.</param>
        /// <returns>The status of the request.</returns>
        public ServiceRequestInformation GetRequestStatus(string requestIdentifier)
        {
            ServiceRequestInformation status = new ServiceRequestInformation();
            status.StatusInformation = _blastClient.checkStatus(requestIdentifier);

            switch (status.StatusInformation)
            {
                case StatusDone:
                    status.Status = ServiceRequestStatus.Ready;
                    break;

                case StatusPending:
                    status.Status = ServiceRequestStatus.Queued;
                    break;

                case StatusRunning:
                    status.Status = ServiceRequestStatus.Waiting;
                    break;

                default:
                    status.Status = ServiceRequestStatus.Error;
                    break;
            }

            return status;
        }

        /// <summary>
        /// Gets the search results for the pertinent request identifier.
        /// Implementation should have dedicated parsers to format the received results into MBF
        /// </summary>
        /// <remarks>An exception is thrown if the request does not succeed.</remarks>
        /// <param name="requestIdentifier">Identifier for the request of interest.</param>
        /// <param name="parameters">Blast input parameters</param>
        /// <returns>The search results</returns>
        public string GetResult(
                string requestIdentifier,
                BlastParameters parameters)
        {
            WSFile[] resultTypes = _blastClient.getResults(requestIdentifier);

            if (resultTypes == null)
            {
                throw new Exception(Resources.EBIWURESULTTYPEFAILED);
            }

            string response = string.Empty;
            foreach (WSFile resultType in resultTypes)
            {
                if (resultType.type == "appxmlfile")
                {
                    byte[] content = _blastClient.poll(requestIdentifier, resultType.type);
                    ASCIIEncoding enc = new ASCIIEncoding();
                    response = enc.GetString(content);
                }
            }

            if (string.IsNullOrEmpty(response))
            {
                throw new Exception(
                        String.Format(CultureInfo.InvariantCulture,
                            Resources.EMPTYRESPONSE,
                            requestIdentifier));
            }

            // we have XML results, parse them
            return response;
        }

        /// <summary>
        /// Fetch the search results synchronously for the pertinent request identifier.
        /// This is a synchronous method and will not return until the results are 
        /// available.
        /// Implementation should have dedicated parsers to format the received results into
        /// MBF
        /// </summary>
        /// <remarks>
        /// An exception is thrown if the request does not succeed.
        /// </remarks>
        /// <param name="requestIdentifier">Identifier for the request of interest</param>
        /// <param name="parameters">Blast input parameters</param>
        /// <returns>The search results</returns>
        public IList<BlastResult> FetchResultsSync(
                string requestIdentifier,
                BlastParameters parameters)
        {
            IList<BlastResult> result = null;

            ServiceRequestInformation requestInfo = new ServiceRequestInformation();
            requestInfo.Status = ServiceRequestStatus.Queued;
            int retryCount = 0;

            do
            {
                requestInfo = GetRequestStatus(requestIdentifier);

                if (requestInfo.Status == ServiceRequestStatus.Ready
                        || requestInfo.Status == ServiceRequestStatus.Error)
                {
                    break;
                }

                retryCount++;
                Thread.Sleep(RetryInterval * retryCount);
            }
            while (retryCount < RetryCount);

            string message;

            if (requestInfo.Status == ServiceRequestStatus.Ready)
            {
                string output = GetResult(
                        requestIdentifier,
                        parameters);

                result = Parser.Parse(new StringReader(output));
            }
            else if (requestInfo.Status == ServiceRequestStatus.Error)
            {
                message = String.Format(CultureInfo.InvariantCulture,
                        Resources.BLASTREQUESTFAILED,
                        requestIdentifier,
                        requestInfo.Status,
                        requestInfo.StatusInformation);

                throw new Exception(message);
            }
            else
            {
                message = String.Format(CultureInfo.InvariantCulture,
                        Resources.BLASTRETRIESEXCEEDED,
                        requestIdentifier,
                        requestInfo.Status,
                        requestInfo.StatusInformation);

                throw new Exception(message);
            }

            return result;
        }

        /// <summary>
        /// Cancels the submitted job.
        /// </summary>
        /// <param name="requestIdentifier">Identifier for the request of interest.</param>
        /// <returns>Is the job cancelled.</returns>
        public bool CancelRequest(string requestIdentifier)
        {
            if (null != _workerThread)
            {
                _workerThread.CancelAsync();
            }

            _blastClient.Abort();

            return true;
        }

        /// <summary>
        /// Get metadata of various sorts exposed by the service.
        /// </summary>
        /// <param name="kind">The kind of metadata to fetch.</param>
        /// <returns>A list of strings.</returns>
        public IList<string> GetServiceMetadata(string kind)
        {
            outData[] data = new outData[0];
            switch (kind)
            {
                case MetadataDatabases:
                    data = _blastClient.getDatabases();
                    break;

                case MetadataFilter:
                    data = _blastClient.getFilters();
                    break;

                case MetadataMatrices:
                    data = _blastClient.getMatrices();
                    break;

                case MetadataPrograms:
                    data = _blastClient.getPrograms();
                    break;

                case MetadataSensitivity:
                    data = _blastClient.getSensitivity();
                    break;

                case MetadataSort:
                    data = _blastClient.getSort();
                    break;

                case MetadataStatistics:
                    data = _blastClient.getStats();
                    break;

                case MetadataXmlFormats:
                    data = _blastClient.getXmlFormats();
                    break;
            }

            List<string> ret = new List<string>();
            foreach (outData d in data)
            {
                ret.Add(d.name);
            }

            return ret;
        }

        #endregion

        #region Private Static Method

        /// <summary>
        /// Get the blast service request object with all the request parameter set
        /// </summary>
        /// <param name="parameters">Blast parameters</param>
        /// <returns>Blast service request object</returns>
        private static inputParams GetRequestParameter(
                BlastParameters parameters)
        {
            inputParams blastParameter = new inputParams();

            // check required parameters:
            blastParameter.database = parameters.Settings[ParameterDatabase];

            // force program to uppercase, per EBI docs (though the service seems
            // to work fine regardless of the case of this parameter)
            blastParameter.program = parameters.Settings[ParameterProgram].ToUpperInvariant();

            // note: query is not part of the inputParams class, so the caller will
            // need to handle it separately.
            blastParameter.email = parameters.Settings[ParameterEmail];

            // apply any addition validation logic and set remaining supported parameters:
            // validate filters here, since QBLAST uses a different set:
            if (parameters.Settings.ContainsKey(ParameterFilter))
            {
                blastParameter.filter = parameters.Settings[ParameterFilter];
            }

            if (parameters.Settings.ContainsKey(ParameterAlignments))
            {
                blastParameter.numal = (int?)int.Parse(parameters.Settings[ParameterAlignments], CultureInfo.InvariantCulture);
            }

            if (parameters.Settings.ContainsKey(ParameterMatrixName))
            {
                blastParameter.matrix = parameters.Settings[ParameterMatrixName];
            }

            if (parameters.Settings.ContainsKey(ParameterExpect))
            {
                blastParameter.exp = (float?)float.Parse(parameters.Settings[ParameterExpect], CultureInfo.InvariantCulture);
            }

            return blastParameter;
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Process the request. This method takes care of executing the rest of the steps
        /// to complete the blast search request in a background thread. Which involves
        /// 1. Submit the job to server
        /// 2. Ping the service with the request identifier to get the status of request.
        /// 3. Repeat step 1, at "RetryInterval" for "RetryCount" till a "success"/"failure" 
        ///     status.
        /// 4. If the status is a "failure" raise an completed event to notify the user 
        ///     with appropriate details.
        /// 5. If the status "success". Get the output of search from server in xml format.
        /// 6. Parse the xml and the framework object model.
        /// 7. Raise the completed event and notify user with the output.
        /// </summary>
        /// <param name="sender">Client request EBIWU Blast search</param>
        /// <param name="argument">Thread event argument</param>
        private void ProcessRequestThread(object sender, DoWorkEventArgs argument)
        {
            ThreadParameter threadParameter = (ThreadParameter)argument.Argument;
            string requestIdentifier = threadParameter.RequestIdentifier;
            try
            {
                ServiceRequestInformation requestInfo = new ServiceRequestInformation();
                requestInfo.Status = ServiceRequestStatus.Queued;
                int retryCount = 0;

                do
                {
                    requestInfo = GetRequestStatus(requestIdentifier);

                    if (requestInfo.Status == ServiceRequestStatus.Ready
                            || requestInfo.Status == ServiceRequestStatus.Error
                            || _workerThread.CancellationPending)
                    {
                        break;
                    }

                    retryCount++;
                    Thread.Sleep(RetryInterval * retryCount);
                }
                while (retryCount < RetryCount);

                if (_workerThread.CancellationPending)
                {
                    argument.Cancel = true;
                }
                else
                {
                    RequestCompletedEventArgs eventArgument = null;
                    string message;

                    if (requestInfo.Status == ServiceRequestStatus.Ready)
                    {
                        string output = GetResult(
                                requestIdentifier,
                                threadParameter.Parameters);

                        IList<BlastResult> result = Parser.Parse(new StringReader(output));

                        eventArgument = new RequestCompletedEventArgs(
                                requestIdentifier,
                                true,
                                result,
                                null,
                                string.Empty,
                                _workerThread.CancellationPending);

                        argument.Result = eventArgument;
                    }
                    else if (requestInfo.Status == ServiceRequestStatus.Error)
                    {
                        message = String.Format(CultureInfo.InvariantCulture,
                                Resources.BLASTREQUESTFAILED,
                                requestIdentifier,
                                requestInfo.Status,
                                requestInfo.StatusInformation);

                        eventArgument = new RequestCompletedEventArgs(
                                requestIdentifier,
                                false,
                                null,
                                new Exception(message),
                                message,
                                _workerThread.CancellationPending);

                        argument.Result = eventArgument;
                    }
                    else
                    {
                        message = String.Format(CultureInfo.InvariantCulture,
                                Resources.BLASTRETRIESEXCEEDED,
                                requestIdentifier,
                                requestInfo.Status,
                                requestInfo.StatusInformation);

                        eventArgument = new RequestCompletedEventArgs(
                                requestIdentifier,
                                false,
                                null,
                                new TimeoutException(message),
                                message,
                                _workerThread.CancellationPending);

                        argument.Result = eventArgument;
                    }
                }
            }
            catch (Exception ex)
            {
                RequestCompletedEventArgs eventArgument = new RequestCompletedEventArgs(
                        string.Empty,
                        false,
                        null,
                        ex,
                        ex.Message,
                        _workerThread.CancellationPending);

                argument.Result = eventArgument;
            }
        }

        /// <summary>
        /// This method is invoked when request status is completed
        /// </summary>
        /// <param name="sender">Invoker of the event</param>
        /// <param name="eventArgument">Event arguments</param>
        private void CompletedRequestThread(
                object sender,
                RunWorkerCompletedEventArgs eventArgument)
        {
            if (null != RequestCompleted && !eventArgument.Cancelled)
            {
                RequestCompleted(null, (RequestCompletedEventArgs)eventArgument.Result);
            }
        }

        /// <summary>
        /// Check the currently set parameters for validity
        /// </summary>
        /// <param name="parameters">Blast input parameters</param>
        /// <returns>Validation result</returns>
        public static ParameterValidationResult ValidateParameters(BlastParameters parameters)
        {
            ParameterValidationResult result = new ParameterValidationResult();
            result.IsValid = true;

            // check required parameters:
            if (!parameters.Settings.ContainsKey(ParameterDatabase))
            {
                result.IsValid = false;
                result.ValidationErrors += Resources.PARAMETERDATABASEREQUIRED;
            }

            if (!parameters.Settings.ContainsKey(ParameterProgram))
            {
                result.IsValid = false;
                result.ValidationErrors += Resources.PARAMETERPROGRAMREQUIRED;
            }

            // note: query is not part of the inputParams class, so the caller will
            // need to handle it separately.
            if (!parameters.Settings.ContainsKey(ParameterEmail))
            {
                result.IsValid = false;
                result.ValidationErrors += Resources.PARAMETEREMAILREQUIRED;
            }

            if (parameters.Settings.ContainsKey(ParameterFilter))
            {
                string filter = parameters.Settings[ParameterFilter];
                if (!Helper.StringHasMatch(filter, "none", "seg", "xnu", "seg+xnu", "dust"))
                {
                    result.IsValid = false;
                    result.ValidationErrors += string.Format(CultureInfo.InvariantCulture, Resources.INVALIDBLASTFILTER, filter, "'none, 'seg', 'xnu', 'seg+xnu', 'dust'");
                }
            }

            // Any other unknown parameters
            foreach (KeyValuePair<string, string> parameter in parameters.Settings)
            {
                switch (parameter.Key)
                {
                    case ParameterDatabase:
                    case ParameterProgram:
                    case ParameterEmail:
                    case ParameterFilter:
                    case ParameterAlignments:
                    case ParameterMatrixName:
                    case ParameterExpect:
                        // These are valid parameter, so allow them.
                        break;

                    default:
                        result.IsValid = false;
                        result.ValidationErrors += string.Format(CultureInfo.InvariantCulture,
                            Resources.PARAMETERUNKNOWNEBIWU,
                            parameter.Key);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Initialize the configuration properties
        /// </summary>
        private void InitializeConfiguration()
        {
            RetryCount = Configuration.RetryCount > 0
                    ? Configuration.RetryCount
                    : DefaultNoOfRetries;

            RetryInterval = Configuration.RetryInterval > 0
                    ? Configuration.RetryInterval
                    : DefaultRetryInterval;
        }

        /// <summary>
        /// Initialize EBIWU Blast client
        /// </summary>
        private void InitializeBlastClient()
        {
            _blastClient = new WSWUBlastService();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// If the EbiWuBlastHandler was opened by this object, dispose it.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the managed resource
        /// </summary>
        /// <param name="disposing">If disposing equals true, dispose all resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != _workerThread)
                {
                    _workerThread.Dispose();
                    _workerThread = null;
                }

                if (null != _blastClient)
                {
                    _blastClient.Dispose();
                    _blastClient = null;
                }
            }
        }

        #endregion
    }
}