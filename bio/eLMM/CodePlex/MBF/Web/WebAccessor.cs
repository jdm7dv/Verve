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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Bio.Web
{
    /// <summary>
    /// A WebAccessor manages the process of downloading information from a 
    /// URL.
    /// </summary>
    public class WebAccessor
    {
        #region Member variables

        private HttpWebResponse _webResponse;
        private WebProxy _proxy = new WebProxy();

        #endregion

        #region Properties

        /// <summary>
        /// The WebProxy object that will be used for HTTP requests.
        /// </summary>
        public WebProxy Proxy
        {
            get
            {
                return _proxy;
            }
            set
            {
                _proxy = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get and store the default browser proxy in effect
        /// </summary>
        public void GetBrowserProxy()
        {
            WebRequest temp = WebRequest.Create(new Uri("http://www.microsoft.com"));

            _proxy = new WebProxy();
            // bug in msdn example:
            // this throws an exception: _proxy = (WebProxy)temp.Proxy;
            Uri prox = temp.Proxy.GetProxy(new Uri("http://www.microsoft.com"));
            _proxy.Address = prox;
        }

        /// <summary>
        /// Restore the default proxy
        /// </summary>
        public void GetDefaultProxy()
        {
            _proxy = new WebProxy();
        }

        /// <summary>
        /// Submit a parameterized HTTP request by either GET or POST. The 
        /// caller can ask for the response either as a string or as a stream.
        /// </summary>
        /// <remarks>
        /// If getResponse = false, the responseStream can be used by the caller
        /// to read the response. The caller must call Close() when done with the stream.
        /// If getResponse = true, the stream will be null, and Close() should not be called.
        /// </remarks>
        /// <param name="url">The URL to request</param>
        /// <param name="doPost">POST if true, GET if false.</param>
        /// <param name="requestParameters">A set of parameter/value pairs, in unencoded form.</param>
        /// <returns>Response from Web.</returns>
        public WebAccessorResponse SubmitHttpRequest(
            Uri url,
            bool doPost,
            Dictionary<string, string> requestParameters)
        {
            WebAccessorResponse webAccessorResponse = new WebAccessorResponse();
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                StringBuilder paramBlock = new StringBuilder();
                string separator = string.Empty;
                foreach (KeyValuePair<string, string> kvp in requestParameters)
                {
                    paramBlock.Append(separator);
                    separator = "&";
                    paramBlock.Append(HttpUtility.UrlEncode(kvp.Key));
                    if (!string.IsNullOrEmpty(kvp.Value))
                    {
                        paramBlock.Append("=");
                        paramBlock.Append(HttpUtility.UrlEncode(kvp.Value));
                    }
                }

                WebRequest request;
                if (doPost)
                {
                    byte[] postBytes = encoding.GetBytes(paramBlock.ToString());
                    request = WebRequest.Create(url);
                    request.Proxy = _proxy;
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = postBytes.Length;
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(postBytes, 0, postBytes.Length);
                    requestStream.Close();
                }
                else
                {
                    url = new Uri(string.Format(CultureInfo.InvariantCulture,
                            "{0}?{1}",
                            url.ToString(),
                            paramBlock));

                    request = WebRequest.Create(url);
                    request.Proxy = _proxy;
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Method = "GET";
                }

                Close();    // get rid of any old response
                _webResponse = (HttpWebResponse)request.GetResponse();
                webAccessorResponse.StatusDescription = _webResponse.StatusDescription;
                if (webAccessorResponse.StatusDescription == "OK")
                {
                        using (Stream s = _webResponse.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(s))
                            {
                                webAccessorResponse.ResponseString = reader.ReadToEnd();
                            }
                        }
                        Close();
                        webAccessorResponse.IsSuccessful = true;
                        return webAccessorResponse;
                }

                webAccessorResponse.IsSuccessful = false;
                return webAccessorResponse;
            }
            catch (WebException we)
            {
                HttpWebResponse response = (HttpWebResponse)we.Response;
                if (response == null)
                {
                    webAccessorResponse.StatusDescription = string.Format(CultureInfo.InvariantCulture,
                            "{0}{1}",
                            webAccessorResponse.StatusDescription,
                            we.Message);
                    if (we.InnerException != null)
                    {
                        webAccessorResponse.StatusDescription = string.Format(CultureInfo.InvariantCulture,
                                "{0}\n{1}",
                                webAccessorResponse.StatusDescription,
                                we.InnerException.Message);
                    }
                }
                else
                {
                    webAccessorResponse.StatusDescription = string.Format(CultureInfo.InvariantCulture,
                            "{0}WebException: {1}",
                            webAccessorResponse.StatusDescription,
                            response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                webAccessorResponse.StatusDescription = ex.Message;
                if (ex.InnerException != null)
                {
                    webAccessorResponse.StatusDescription = string.Format(CultureInfo.InvariantCulture,
                            "{0}\n{1}",
                            webAccessorResponse.StatusDescription,
                            ex.InnerException.Message);
                }
            }

            webAccessorResponse.IsSuccessful = false;
            return webAccessorResponse;
        }

        /// <summary>
        /// Close the internal HttpWebResponse, after reading from the stream returned by
        /// SubmitHttpRequest with getResponse = false.
        /// </summary>
        public void Close()
        {
            if (_webResponse != null)
            {
                _webResponse.Close();
                _webResponse = null;
            }
        }

        #endregion
    }
}
