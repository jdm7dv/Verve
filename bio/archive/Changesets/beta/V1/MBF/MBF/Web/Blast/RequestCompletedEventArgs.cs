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

namespace MBF.Web.Blast
{
    /// <summary>
    /// Event arguments used to notify the user when the job is completed.
    /// </summary>
    public class RequestCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Is the search successful
        /// </summary>
        private bool _isSearchSuccessful;

        /// <summary>
        /// Result of blast search
        /// </summary>
        private IList<BlastResult> _searchResult;

        /// <summary>
        /// Error message on failure
        /// </summary>
        private string _errorMessage;

        /// <summary>
        /// Exception occured
        /// </summary>
        private Exception _error;

        /// <summary>
        /// Job identifier
        /// </summary>
        private string _requestIdentifier;

        /// <summary>
        /// Is this request cancelled.
        /// </summary>
        private bool _isCanceled;

        /// <summary>
        /// Initializes a new instance of the RequestCompletedEventArgs class
        /// </summary>
        /// <param name="requestIdentifier">Job identifier</param>
        /// <param name="isSearchSuccessful">Is search successful</param>
        /// <param name="searchResult">Search result records</param>
        /// <param name="error">Exception if any</param>
        /// <param name="errorMessage">Error message if any</param>
        /// <param name="isCanceled">Was request cancelled</param>
        public RequestCompletedEventArgs(
                string requestIdentifier,
                bool isSearchSuccessful,
                IList<BlastResult> searchResult,
                Exception error,
                string errorMessage,
                bool isCanceled)
        {
            _requestIdentifier = requestIdentifier;
            _isSearchSuccessful = isSearchSuccessful;
            _searchResult = searchResult;
            _error = error;
            _errorMessage = errorMessage;
            _isCanceled = isCanceled;
        }

        /// <summary>
        /// Gets a value indicating whether the search  is successful
        /// </summary>
        public bool IsSearchSuccessful
        {
            get { return _isSearchSuccessful; }
        }

        /// <summary>
        /// Gets result of blast search
        /// </summary>
        public IList<BlastResult> SearchResult
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
        /// Gets job identifier
        /// </summary>
        public string RequestIdentifier
        {
            get { return _requestIdentifier; }
        }

        /// <summary>
        /// Gets a value indicating whether the search  is cancelled.
        /// </summary>
        public bool IsCanceled
        {
            get { return _isCanceled; }
        }
    }
}