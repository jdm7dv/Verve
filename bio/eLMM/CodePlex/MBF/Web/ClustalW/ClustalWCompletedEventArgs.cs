//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



using System;

namespace Bio.Web.ClustalW
{
    /// <summary>
    /// Event arguments used to notify the user when the job is completed.
    /// </summary>
    public class ClustalWCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Is the search successfull
        /// </summary>
        private bool _isSearchSuccessful;

        /// <summary>
        /// Result of blast search
        /// </summary>
        private ClustalWResult _searchResult;

        /// <summary>
        /// Error message on failure
        /// </summary>
        private string _errorMessage;

        /// <summary>
        /// Exception occured
        /// </summary>
        private Exception _error;

        /// <summary>
        /// ClustalW Service parameters object
        /// </summary>
        private ServiceParameters _parameters;

        /// <summary>
        /// Is this request cancelled.
        /// </summary>
        private bool _canceled;

        /// <summary>
        /// Initializes a new instance of the ClustalWCompletedEventArgs class
        /// </summary>
        /// <param name="parameters">Service parameter</param>
        /// <param name="isSearchSuccessful">Is search successfull</param>
        /// <param name="searchResult">Search result records</param>
        /// <param name="error">Exception if any</param>
        /// <param name="errorMessage">Error message if any</param>
        /// <param name="canceled">Was request cancelled</param>
        public ClustalWCompletedEventArgs(
                ServiceParameters parameters,
                bool isSearchSuccessful,
                ClustalWResult searchResult,
                Exception error,
                string errorMessage,
                bool canceled)
        {
            _parameters = parameters;
            _isSearchSuccessful = isSearchSuccessful;
            _searchResult = searchResult;
            _error = error;
            _errorMessage = errorMessage;
            _canceled = canceled;
        }

        /// <summary>
        /// Gets a value indicating whether the search  is successfull
        /// </summary>
        public bool IsSearchSuccessful
        {
            get { return _isSearchSuccessful; }
        }

        /// <summary>
        /// Gets result of blast search
        /// </summary>
        public ClustalWResult SearchResult
        {
            get { return _searchResult; }
        }

        /// <summary>
        /// Gets the error message on failure
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        /// <summary>
        /// Gets the Exception occured
        /// </summary>
        public Exception Error
        {
            get { return _error; }
        }

        /// <summary>
        /// Gets a value indicating whether the search  is cancelled.
        /// </summary>
        public bool Canceled
        {
            get { return _canceled; }
        }

        /// <summary>
        /// Gets the ClustalW Service parameters
        /// </summary>
        public ServiceParameters Parameters
        {
            get { return _parameters; }
        }
    }
}
