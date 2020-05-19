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
using System.Net;

namespace MBF.Web
{
    /// <summary>
    /// This class contains the inputs that are required to instantiate and invoke web method.
    /// </summary>
    public class AsyncWebMethodRequest
    {
        /// <summary>
        /// Uri of web reqeust.
        /// </summary>
        private Uri _url = null;

        /// <summary>
        /// Credential to be used for web request.
        /// </summary>
        private ICredentials _credential = null;

        /// <summary>
        /// Parameters to be passed in web request header.
        /// </summary>
        private Dictionary<string, string> _parameter = null;

        /// <summary>
        /// Post data string for web request.
        /// </summary>
        private string _postData = string.Empty;

        /// <summary>
        /// Function pointer to be invoked after the completion of web request.
        /// </summary>
        private AsyncWebMethodCompleted _callback = null;

        /// <summary>
        /// State to be web request.
        /// </summary>
        private object _state = null;

        /// <summary>
        /// Constructor: Initialize the instance of type WebMethodInput
        /// </summary>
        /// <param name="url">Uri of web reqeust.</param>
        /// <param name="credential">Credential to be used for web request.</param>
        /// <param name="parameters">Request parameters.</param>
        /// <param name="postData">Post data string for web request.</param>
        /// <param name="callback">Function pointer to be invoked after the completion of web request.</param>
        /// <param name="state">State of the Async web method</param>
        public AsyncWebMethodRequest(Uri url, 
            ICredentials credential, 
            Dictionary<string, string> parameters, 
            string postData, 
            AsyncWebMethodCompleted callback,
            object state)
        {
            _url = url;
            _credential = credential;
            _parameter = parameters;
            _postData = postData;
            _callback = callback;
            _state = state;
        }

        /// <summary>
        /// Gets the Uri of web reqeust.
        /// </summary>
        public Uri Url
        {
            get { return _url; }
        }

        /// <summary>
        /// Gets the credential to be used for web request.
        /// </summary>
        public ICredentials Credential
        {
            get { return _credential; }
        }

        /// <summary>
        /// Gets parameters to be passed in web request header.
        /// </summary>
        public Dictionary<string, string> Parameter
        {
            get { return _parameter; }
        }

        /// <summary>
        /// Gets the post data string for web request.
        /// </summary>
        public string PostData
        {
            get { return _postData; }
        }

        /// <summary>
        /// Gets the function pointer to be invoked after the completion of web request.
        /// </summary>
        public AsyncWebMethodCompleted Callback
        {
            get { return _callback; }
        }

        /// <summary>
        /// Gets or sets the webrequest instance.
        /// </summary>
        public WebRequest Request { get; set; }

        /// <summary>
        /// Gets the state of Web request
        /// </summary>
        public object State 
        {
            get { return _state; }
        }
    }
}
