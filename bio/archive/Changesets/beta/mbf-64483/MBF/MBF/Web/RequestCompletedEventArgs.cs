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

namespace MBF.Web
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
        /// Error message on failure
        /// </summary>
        private string _errorMessage;

        /// <summary>
        /// Exception occured
        /// </summary>
        private Exception _error;

        /// <summary>
        /// Is this request cancelled.
        /// </summary>
        private bool _isCanceled;

        /// <summary>
        /// Initializes a new instance of the RequestCompletedEventArgs class
        /// </summary>
        /// <param name="isSearchSuccessful">Is search successful</param>
        /// <param name="error">Exception if any</param>
        /// <param name="errorMessage">Error message if any</param>
        /// <param name="isCanceled">Was request cancelled</param>
        public RequestCompletedEventArgs(
                bool isSearchSuccessful,
                Exception error,
                string errorMessage,
                bool isCanceled)
        {
            _isSearchSuccessful = isSearchSuccessful;
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
        public bool IsCanceled
        {
            get { return _isCanceled; }
        }
    }
}