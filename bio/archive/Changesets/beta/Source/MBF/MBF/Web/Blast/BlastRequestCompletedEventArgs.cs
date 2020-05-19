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
using System.Linq;
using System.Text;

namespace MBF.Web.Blast
{
    /// <summary>
    /// Event arguments used to notify the user when the job is completed.
    /// </summary>
    public class BlastRequestCompletedEventArgs : RequestCompletedEventArgs
    {
        /// <summary>
        /// Result of blast search
        /// </summary>
        private IList<BlastResult> _searchResult;

        /// <summary>
        /// Job identifier
        /// </summary>
        private string _requestIdentifier;

        /// <summary>
        /// Initializes a new instance of the RequestCompletedEventArgs class
        /// </summary>
        /// <param name="requestIdentifier">Job identifier</param>
        /// <param name="isSearchSuccessful">Is search successful</param>
        /// <param name="searchResult">Search result records</param>
        /// <param name="error">Exception if any</param>
        /// <param name="errorMessage">Error message if any</param>
        /// <param name="isCanceled">Was request cancelled</param>
        public BlastRequestCompletedEventArgs(
                string requestIdentifier,
                bool isSearchSuccessful,
                IList<BlastResult> searchResult,
                Exception error,
                string errorMessage,
                bool isCanceled)
            : base(isSearchSuccessful, error, errorMessage, isCanceled)
        {
            _requestIdentifier = requestIdentifier;
            _searchResult = searchResult;
        }

        /// <summary>
        /// Gets result of blast search
        /// </summary>
        public IList<BlastResult> SearchResult
        {
            get { return _searchResult; }
        }

        /// <summary>
        /// Gets job identifier
        /// </summary>
        public string RequestIdentifier
        {
            get { return _requestIdentifier; }
        }
    }
}