#region License : arachnode.net

// Copyright (c) 2012 http://arachnode.net, arachnode.net, LLC
//  
// Permission is hereby granted, upon purchase, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, merge and modify copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following
// conditions:
// 
// LICENSE (ALL VERSIONS/EDITIONS): http://arachnode.net/r.ashx?3
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

#region

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Xml;
using Arachnode.Cache;
using Arachnode.DataAccess;
using Arachnode.SiteCrawler.Next;
using Arachnode.SiteCrawler.Next.Managers;
using Arachnode.SiteCrawler.Next.Utilities;
using Arachnode.SiteCrawler.Next.Value;
using Arachnode.SiteCrawler.Next.Value.AbstractClasses;
using Arachnode.SiteCrawler.Next.Value.Enums;
using Arachnode.Renderer.Value.Enums;
using mshtml;

#endregion

namespace Arachnode.Plugins.Next
{
    public class CrawlRequestPlugins<TDiscovery, TCrawlRequest> : APluginBase<TDiscovery, TCrawlRequest>
        where TDiscovery : ADiscovery
        where TCrawlRequest : ACrawlRequest
    {
        private readonly Dictionary<string, DateTime> _frequencyByDomain = new Dictionary<string, DateTime>();
        private readonly object _frequencyByDomainLock = new object();
        private readonly Dictionary<string, string> _fullTextIndexTypes = new Dictionary<string, string>();

        private readonly int _maximumDownloadableContentLengthInBytes;

        private readonly Dictionary<int, Renderer.Renderer> _renderers = new Dictionary<int, Renderer.Renderer>();
        private readonly Dictionary<string, RobotsDotText> _robotsDotTextsByDomain = new Dictionary<string, RobotsDotText>();
        private readonly object _robotsDotTextsByDomainLock = new object();
        private readonly int _throttleCrawlingByDomainCrawlDelayInMilliseconds;

        private CookieContainer _cookieContainer;

        public CrawlRequestPlugins(Crawler<TDiscovery, TCrawlRequest> crawler, string pluginConfigurationFileLocation, bool useSqlServer)
            : base(crawler, useSqlServer)
        {
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.Load(pluginConfigurationFileLocation);

            _throttleCrawlingByDomainCrawlDelayInMilliseconds = int.Parse(xmlDocument.DocumentElement.SelectSingleNode("ThrottleCrawlingByDomainCrawlDelayInMilliseconds").Attributes["value"].Value);

            _maximumDownloadableContentLengthInBytes = int.Parse(xmlDocument.DocumentElement.SelectSingleNode("MaximumDownloadableContentLengthInBytes").Attributes["value"].Value);

            foreach (XmlNode xmlNode in xmlDocument.DocumentElement.SelectNodes("FullTextIndexTypes/FullTextIndexType"))
            {
                _fullTextIndexTypes.Add(xmlNode.Attributes["key"].Value.ToLowerInvariant().Trim(), xmlNode.Attributes["value"].Value.ToLowerInvariant().Trim());
            }
        }

        public CookieContainer CookieContainer
        {
            get
            {
                if (_cookieContainer == null)
                {
                    _cookieContainer = new CookieContainer();
                }

                return _cookieContainer;
            }
            set { _cookieContainer = value; }
        }

        public CredentialCache CredentialCache { get; set; }
        public IWebProxy Proxy { get; set; }

        public void ThrottleCrawlingByDomainCrawlDelay(ACrawlRequest crawlRequest, DistributedCache crawlRequestsDistributedCache, SHA1 sha1, ref CrawlRequestDirective crawlRequestDirective)
        {
            if (crawlRequestDirective != CrawlRequestDirective.ContinueCrawling)
            {
                return;
            }

            switch (crawlRequest.DownloadStatus)
            {
                case DownloadStatus.PreCreateRequest:
                    string domain = crawlRequest.Discovery.Uri.GetValidatedDomain();

                    lock (_frequencyByDomainLock)
                    {
                        if (!_frequencyByDomain.ContainsKey(domain))
                        {
                            _frequencyByDomain.Add(domain, DateTime.MinValue);
                        }

                        if (DateTime.Now.Subtract(_frequencyByDomain[domain]).TotalMilliseconds <= _throttleCrawlingByDomainCrawlDelayInMilliseconds)
                        {
                            crawlRequestDirective = CrawlRequestDirective.Enqueue;
                        }
                        else
                        {
                            _frequencyByDomain[domain] = DateTime.Now;
                        }
                    }
                    break;
                case DownloadStatus.PreGetResponse:
                    break;
                case DownloadStatus.PostGetResponse:
                    break;
                case DownloadStatus.PreDownload:
                    break;
                case DownloadStatus.Download:
                    break;
                case DownloadStatus.PostDownload:
                    break;
                case DownloadStatus.PreDiscoveryCreation:
                    break;
                case DownloadStatus.PostDiscoveryCreation:
                    break;
                case DownloadStatus.PreCrawlRequestCreation:
                    break;
                case DownloadStatus.PostCrawlRequestCreation:
                    break;
            }
        }

        public void ProcessRobotsDotText(ACrawlRequest crawlRequest, DistributedCache crawlRequestsDistributedCache, SHA1 sha1, ref CrawlRequestDirective crawlRequestDirective)
        {
            if (crawlRequestDirective != CrawlRequestDirective.ContinueCrawling)
            {
                return;
            }

            switch (crawlRequest.DownloadStatus)
            {
                case DownloadStatus.PreCreateRequest:
                    string domain = crawlRequest.Discovery.Uri.GetValidatedDomain();
                    string host = crawlRequest.Discovery.Uri.GetValidatedHost();

                    RobotsDotText robotsDotText = null;

                    lock (_robotsDotTextsByDomainLock)
                    {
                        if (!_robotsDotTextsByDomain.ContainsKey(domain))
                        {
                            TCrawlRequest robotsDotTextCrawlRequest = null;

                            try
                            {
                                robotsDotTextCrawlRequest = CrawlRequest.Create<TCrawlRequest>(new Discovery(new Uri("http://" + host + "/robots.txt"), true, DiscoveryStatus.Ok), 1, 1);

                                HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(robotsDotTextCrawlRequest.Discovery.Uri.AbsoluteUri);
                                httpWebRequest.AllowAutoRedirect = true;
                                httpWebRequest.MaximumAutomaticRedirections = 50;

                                robotsDotTextCrawlRequest.HttpWebRequest = httpWebRequest;

                                /**/

                                CrawlRequestDirective crawlRequestDirective2 = CrawlRequestDirective.ContinueCrawling;

                                robotsDotTextCrawlRequest.DownloadStatus = DownloadStatus.PreGetResponse;

                                ModifyHttpWebRequest(robotsDotTextCrawlRequest, crawlRequestsDistributedCache, sha1, ref crawlRequestDirective2);

                                /**/

                                HttpWebResponse httpWebResponse = (HttpWebResponse) httpWebRequest.GetResponse();

                                robotsDotTextCrawlRequest.HttpWebResponse = httpWebResponse;
                                robotsDotTextCrawlRequest.DownloadStatus = DownloadStatus.Download;

                                /**/

                                DownloadDataAndDecodeHtml(robotsDotTextCrawlRequest, crawlRequestsDistributedCache, sha1, ref crawlRequestDirective2);

                                /**/

                                robotsDotText = RobotsDotTextManager.ParseRobotsDotTextSource(robotsDotTextCrawlRequest.Discovery.Uri, robotsDotTextCrawlRequest.HttpWebRequest.UserAgent, robotsDotTextCrawlRequest.Discovery.Data);

                                _robotsDotTextsByDomain.Add(domain, robotsDotText);
                            }
                            catch (Exception exception)
                            {
                                _robotsDotTextsByDomain.Add(domain, null);

                                //we need to throw to get the Source and the StackTrace...
                                try
                                {
                                    throw new Exception("Could not process the robots.txt file.", exception);
                                }
                                catch (Exception exception2)
                                {
                                    Crawler.ProcessException(robotsDotTextCrawlRequest, exception2);
                                }
                            }
                        }
                        else
                        {
                            robotsDotText = _robotsDotTextsByDomain[domain];
                        }
                    }

                    if (robotsDotText != null)
                    {
                        if (robotsDotText.CrawlDelay != 0 && DateTime.Now.Subtract(robotsDotText.LastAccessed).TotalMilliseconds <= robotsDotText.CrawlDelay*1000)
                        {
                            crawlRequestDirective = CrawlRequestDirective.Enqueue;

                            return;
                        }

                        foreach (string disallowedPath in robotsDotText.DisallowedPaths)
                        {
                            if (HttpUtility.UrlDecode(crawlRequest.Discovery.Uri.AbsoluteUri).StartsWith(HttpUtility.UrlDecode(disallowedPath)))
                            {
                                crawlRequestDirective = CrawlRequestDirective.StopCrawling;

                                return;
                            }
                        }

                        robotsDotText.LastAccessed = DateTime.Now;
                    }
                    break;
                case DownloadStatus.PreGetResponse:
                    break;
                case DownloadStatus.PostGetResponse:
                    break;
                case DownloadStatus.PreDownload:
                    break;
                case DownloadStatus.Download:
                    break;
                case DownloadStatus.PostDownload:
                    break;
                case DownloadStatus.PreDiscoveryCreation:
                    break;
                case DownloadStatus.PostDiscoveryCreation:
                    break;
                case DownloadStatus.PreCrawlRequestCreation:
                    break;
                case DownloadStatus.PostCrawlRequestCreation:
                    break;
            }
        }

        public void ModifyHttpWebRequest(ACrawlRequest crawlRequest, DistributedCache crawlRequestsDistributedCache, SHA1 sha1, ref CrawlRequestDirective crawlRequestDirective)
        {
            if (crawlRequestDirective != CrawlRequestDirective.ContinueCrawling)
            {
                return;
            }

            switch (crawlRequest.DownloadStatus)
            {
                case DownloadStatus.PreCreateRequest:
                    break;
                case DownloadStatus.PreGetResponse:
                    crawlRequest.HttpWebRequest.AllowAutoRedirect = true;
                    crawlRequest.HttpWebRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                    crawlRequest.HttpWebRequest.CookieContainer = CookieContainer;
                    if (CredentialCache != null)
                    {
                        crawlRequest.HttpWebRequest.Credentials = CredentialCache;
                    }
                    crawlRequest.HttpWebRequest.KeepAlive = false;
                    crawlRequest.HttpWebRequest.MaximumAutomaticRedirections = 50;
                    crawlRequest.HttpWebRequest.Method = "GET";
                    crawlRequest.HttpWebRequest.Proxy = Proxy;
                    crawlRequest.HttpWebRequest.ReadWriteTimeout = 30000;
                    crawlRequest.HttpWebRequest.Timeout = 30000;
                    crawlRequest.HttpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.71 Safari/534.24";
                    break;
                case DownloadStatus.PostGetResponse:
                    break;
                case DownloadStatus.PreDownload:
                    break;
                case DownloadStatus.Download:
                    break;
                case DownloadStatus.PostDownload:
                    break;
                case DownloadStatus.PreDiscoveryCreation:
                    break;
                case DownloadStatus.PostDiscoveryCreation:
                    break;
                case DownloadStatus.PreCrawlRequestCreation:
                    break;
                case DownloadStatus.PostCrawlRequestCreation:
                    break;
            }
        }

        public void ProcessCookies(ACrawlRequest crawlRequest, DistributedCache crawlRequestsDistributedCache, SHA1 sha1, ref CrawlRequestDirective crawlRequestDirective)
        {
            if (crawlRequestDirective != CrawlRequestDirective.ContinueCrawling)
            {
                return;
            }

            switch (crawlRequest.DownloadStatus)
            {
                case DownloadStatus.PreCreateRequest:
                    break;
                case DownloadStatus.PreGetResponse:
                    break;
                case DownloadStatus.PostGetResponse:
                    CookieManager.UpdateCookies(crawlRequest.Discovery.Uri.AbsoluteUri, CookieContainer, crawlRequest.HttpWebResponse.Cookies);
                    break;
                case DownloadStatus.PreDownload:
                    break;
                case DownloadStatus.Download:
                    break;
                case DownloadStatus.PostDownload:
                    break;
                case DownloadStatus.PreDiscoveryCreation:
                    break;
                case DownloadStatus.PostDiscoveryCreation:
                    break;
                case DownloadStatus.PreCrawlRequestCreation:
                    break;
                case DownloadStatus.PostCrawlRequestCreation:
                    break;
            }
        }

        public void CheckForUnknownDiscoveryType(ACrawlRequest crawlRequest, DistributedCache crawlRequestsDistributedCache, SHA1 sha1, ref CrawlRequestDirective crawlRequestDirective)
        {
            if (crawlRequestDirective != CrawlRequestDirective.ContinueCrawling)
            {
                return;
            }

            switch (crawlRequest.DownloadStatus)
            {
                case DownloadStatus.PreCreateRequest:
                    break;
                case DownloadStatus.PreGetResponse:
                    break;
                case DownloadStatus.PostGetResponse:
                    if (crawlRequest.Discovery.DiscoveryType == DiscoveryType.Unknown)
                    {
                        if (UseSqlServer)
                        {
                            ArachnodeDAO arachnodeDAO = _arachnodeDAOs[crawlRequest.ThreadNumber];

                            arachnodeDAO.InsertDisallowedAbsoluteUri(1, 1, crawlRequest.Discovery.Parent.Uri.AbsoluteUri, crawlRequest.Discovery.Uri.AbsoluteUri, "Unknown DiscoveryType.  (" + crawlRequest.Discovery.ContentType + ")");
                        }
                    }
                    break;
                case DownloadStatus.PreDownload:
                    break;
                case DownloadStatus.Download:
                    break;
                case DownloadStatus.PostDownload:
                    break;
                case DownloadStatus.PreDiscoveryCreation:
                    break;
                case DownloadStatus.PostDiscoveryCreation:
                    break;
                case DownloadStatus.PreCrawlRequestCreation:
                    break;
                case DownloadStatus.PostCrawlRequestCreation:
                    break;
            }
        }

        public void AssignFullTextIndexType(ACrawlRequest crawlRequest, DistributedCache crawlRequestsDistributedCache, SHA1 sha1, ref CrawlRequestDirective crawlRequestDirective)
        {
            if (crawlRequestDirective != CrawlRequestDirective.ContinueCrawling)
            {
                return;
            }

            switch (crawlRequest.DownloadStatus)
            {
                case DownloadStatus.PreCreateRequest:
                    break;
                case DownloadStatus.PreGetResponse:
                    break;
                case DownloadStatus.PostGetResponse:
                    CrawlRequest crawlRequest2 = null;

                    if (crawlRequest is CrawlRequest)
                    {
                        crawlRequest2 = (CrawlRequest) crawlRequest;
                    }
                    else
                    {
                        return;
                    }

                    string fullTextIndexType;
                    _fullTextIndexTypes.TryGetValue(crawlRequest.Discovery.ContentType, out fullTextIndexType);
                    crawlRequest2.FullTextIndexType = fullTextIndexType;
                    break;
                case DownloadStatus.PreDownload:
                    break;
                case DownloadStatus.Download:
                    break;
                case DownloadStatus.PostDownload:
                    break;
                case DownloadStatus.PreDiscoveryCreation:
                    break;
                case DownloadStatus.PostDiscoveryCreation:
                    break;
                case DownloadStatus.PreCrawlRequestCreation:
                    break;
                case DownloadStatus.PostCrawlRequestCreation:
                    break;
            }
        }

        public void FilterByMaximumDownloadableContentLength(ACrawlRequest crawlRequest, DistributedCache crawlRequestsDistributedCache, SHA1 sha1, ref CrawlRequestDirective crawlRequestDirective)
        {
            if (crawlRequestDirective != CrawlRequestDirective.ContinueCrawling)
            {
                return;
            }

            switch (crawlRequest.DownloadStatus)
            {
                case DownloadStatus.PreCreateRequest:
                    break;
                case DownloadStatus.PreGetResponse:
                    break;
                case DownloadStatus.PostGetResponse:
                    break;
                case DownloadStatus.PreDownload:
                    if (crawlRequest.HttpWebResponse.ContentLength > _maximumDownloadableContentLengthInBytes)
                    {
                        ArachnodeDAO arachnodeDAO = _arachnodeDAOs[crawlRequest.ThreadNumber];

                        arachnodeDAO.InsertDisallowedAbsoluteUri(1, 1, crawlRequest.Discovery.Parent.Uri.AbsoluteUri, crawlRequest.Discovery.Uri.AbsoluteUri, "Disallowed by ContentLength.");

                        crawlRequestDirective = CrawlRequestDirective.StopCrawling;
                    }
                    break;
                case DownloadStatus.Download:
                    break;
                case DownloadStatus.PostDownload:
                    break;
                case DownloadStatus.PreDiscoveryCreation:
                    break;
                case DownloadStatus.PostDiscoveryCreation:
                    break;
                case DownloadStatus.PreCrawlRequestCreation:
                    break;
                case DownloadStatus.PostCrawlRequestCreation:
                    break;
            }
        }

        public void DownloadDataAndDecodeHtml(ACrawlRequest crawlRequest, DistributedCache crawlRequestsDistributedCache, SHA1 sha1, ref CrawlRequestDirective crawlRequestDirective)
        {
            if (crawlRequestDirective != CrawlRequestDirective.ContinueCrawling)
            {
                return;
            }

            switch (crawlRequest.DownloadStatus)
            {
                case DownloadStatus.PreCreateRequest:
                    break;
                case DownloadStatus.PreGetResponse:
                    break;
                case DownloadStatus.PostGetResponse:
                    break;
                case DownloadStatus.PreDownload:
                    break;
                case DownloadStatus.Download:
                    crawlRequest.Discovery.Data = DataManager.DownloadData(crawlRequest.HttpWebResponse);

                    EncodingManager.DecodeHtml(crawlRequest);
                    break;
                case DownloadStatus.PostDownload:
                    break;
                case DownloadStatus.PreDiscoveryCreation:
                    break;
                case DownloadStatus.PostDiscoveryCreation:
                    break;
                case DownloadStatus.PreCrawlRequestCreation:
                    break;
                case DownloadStatus.PostCrawlRequestCreation:
                    break;
            }
        }

        /// <summary>
        /// Renders JavaScript.
        /// </summary>
        /// <param name="crawlRequest"></param>
        /// <param name="crawlRequestsDistributedCache"></param>
        /// <param name="sha1"></param>
        /// <param name="crawlRequestDirective"></param>
        public void DownloadDataRenderAndDecodeHtml(ACrawlRequest crawlRequest, DistributedCache crawlRequestsDistributedCache, SHA1 sha1, ref CrawlRequestDirective crawlRequestDirective)
        {
            if (crawlRequestDirective != CrawlRequestDirective.ContinueCrawling)
            {
                return;
            }

            switch (crawlRequest.DownloadStatus)
            {
                case DownloadStatus.PreCreateRequest:
                    break;
                case DownloadStatus.PreGetResponse:
                    break;
                case DownloadStatus.PostGetResponse:
                    break;
                case DownloadStatus.PreDownload:
                    break;
                case DownloadStatus.Download:
                    if (crawlRequest.Discovery.DiscoveryType == DiscoveryType.WebPage)
                    {
                        //creates a Renderer on the appropriate thread...
                        if (!_renderers.ContainsKey(crawlRequest.ThreadNumber))
                        {
                            _renderers[crawlRequest.ThreadNumber] = new Renderer.Renderer();
                        }

                        Renderer.Renderer renderer = _renderers[crawlRequest.ThreadNumber];

                        HTMLDocumentClass htmlDocument = renderer.Render(crawlRequest.Discovery.Uri.AbsoluteUri, RenderAction.Render, RenderType.Render, crawlRequest.HttpWebRequest.UserAgent, 1);

                        if (htmlDocument.documentElement != null)
                        {
                            crawlRequest.Discovery.Data = Encoding.Default.GetBytes(htmlDocument.documentElement.outerHTML);
                        }
                    }
                    else
                    {
                        crawlRequest.Discovery.Data = DataManager.DownloadData(crawlRequest.HttpWebResponse);
                    }

                    EncodingManager.DecodeHtml(crawlRequest);
                    break;
                case DownloadStatus.PostDownload:
                    break;
                case DownloadStatus.PreDiscoveryCreation:
                    break;
                case DownloadStatus.PostDiscoveryCreation:
                    break;
                case DownloadStatus.PreCrawlRequestCreation:
                    break;
                case DownloadStatus.PostCrawlRequestCreation:
                    break;
            }
        }

        public void DetectDuplicates(ACrawlRequest crawlRequest, DistributedCache crawlRequestsDistributedCache, SHA1 sha1, ref CrawlRequestDirective crawlRequestDirective)
        {
            if (crawlRequestDirective != CrawlRequestDirective.ContinueCrawling)
            {
                return;
            }

            switch (crawlRequest.DownloadStatus)
            {
                case DownloadStatus.PreCreateRequest:
                    break;
                case DownloadStatus.PreGetResponse:
                    break;
                case DownloadStatus.PostGetResponse:
                    break;
                case DownloadStatus.PreDownload:
                    break;
                case DownloadStatus.Download:
                    break;
                case DownloadStatus.PostDownload:
                    if (crawlRequestsDistributedCache.Read(crawlRequest.Discovery.Data, sha1) == null)
                    {
                        crawlRequestsDistributedCache.Write(crawlRequest.Discovery.Data, "", sha1, true);

                        crawlRequest.Information = "Unique detected.";
                    }
                    else
                    {
                        crawlRequest.Information = "Duplicate detected.";
                        crawlRequestDirective = CrawlRequestDirective.StopCrawling;
                    }
                    break;
                case DownloadStatus.PreDiscoveryCreation:
                    break;
                case DownloadStatus.PostDiscoveryCreation:
                    break;
                case DownloadStatus.PreCrawlRequestCreation:
                    break;
                case DownloadStatus.PostCrawlRequestCreation:
                    break;
            }
        }

        public void DetectWebPageDownloadedWhereFileOrImageWasExpected(ACrawlRequest crawlRequest, DistributedCache crawlRequestsDistributedCache, SHA1 sha1, ref CrawlRequestDirective crawlRequestDirective)
        {
            if (crawlRequestDirective != CrawlRequestDirective.ContinueCrawling)
            {
                return;
            }

            switch (crawlRequest.DownloadStatus)
            {
                case DownloadStatus.PreCreateRequest:
                    break;
                case DownloadStatus.PreGetResponse:
                    break;
                case DownloadStatus.PostGetResponse:
                    break;
                case DownloadStatus.PreDownload:
                    if (crawlRequest.Discovery.ExpectFileOrImage && (crawlRequest.Discovery.DiscoveryType != DiscoveryType.Image && crawlRequest.Discovery.DiscoveryType != DiscoveryType.File))
                    {
                        //especially useful in contraining crawls to a domain
                        //move to DownloadStatus.PreDiscoveryCreation to allow submission to the WebPagePlugins
                        crawlRequest.Information = "WebPage returned where a file or image was expected.";
                        crawlRequestDirective = CrawlRequestDirective.StopCrawling;
                    }
                    break;
                case DownloadStatus.Download:
                    break;
                case DownloadStatus.PostDownload:
                    break;
                case DownloadStatus.PreDiscoveryCreation:
                    break;
                case DownloadStatus.PostDiscoveryCreation:
                    break;
                case DownloadStatus.PreCrawlRequestCreation:
                    break;
                case DownloadStatus.PostCrawlRequestCreation:
                    break;
            }
        }
    }
}