#region License : arachnode.net

// // Copyright (c) 2013 http://arachnode.net, arachnode.net, LLC
// //  
// // Permission is hereby granted, upon purchase, to any person
// // obtaining a copy of this software and associated documentation
// // files (the "Software"), to deal in the Software without
// // restriction, including without limitation the rights to use,
// // copy, merge and modify copies of the Software, and to permit persons
// // to whom the Software is furnished to do so, subject to the following
// // conditions:
// // 
// // LICENSE (ALL VERSIONS/EDITIONS): http://arachnode.net/r.ashx?3
// // 
// // The above copyright notice and this permission notice shall be
// // included in all copies or substantial portions of the Software.
// // 
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// // EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// // OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// // NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// // HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// // WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// // FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// // OTHER DEALINGS IN THE SOFTWARE.

#endregion

#region

using System;
using System.Collections.Generic;
using System.Web;
using Arachnode.DataAccess;
using Arachnode.Renderer.Value.Enums;
using Arachnode.SiteCrawler.Components;
using Arachnode.SiteCrawler.Core;
using Arachnode.SiteCrawler.Managers;
using Arachnode.SiteCrawler.Value;
using Arachnode.SiteCrawler.Value.AbstractClasses;
using Arachnode.SiteCrawler.Value.Enums;

#endregion

namespace Arachnode.Plugins.CrawlRules
{
    public class RobotsDotText : ACrawlRule
    {
        public override void AssignSettings(Dictionary<string, string> settings)
        {
        }

        /// <summary>
        /// 	Determines whether the specified crawl request is disallowed.
        /// </summary>
        /// <param name = "crawlRequest">The crawl request.</param>
        /// <param name = "arachnodeDAO">The arachnode DAO.</param>
        /// <returns>
        /// 	<c>true</c> if the specified crawl request is disallowed; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsDisallowed(CrawlRequest crawlRequest, ArachnodeDAO arachnodeDAO)
        {
            //ANODET: When you add the multi-server caching, the robots.txt file will need to be sent to all other CachePeers.

            //if we're not being called by the Engine prior to assigning to a Crawl...
            if (crawlRequest.Crawl != null)
            {
                string robotsDotTextAbsoluteUri = crawlRequest.Discovery.Uri.Scheme + Uri.SchemeDelimiter + crawlRequest.Discovery.Uri.Host + "/robots.txt";

                crawlRequest.OutputIsDisallowedReason = OutputIsDisallowedReason;

                if (!UserDefinedFunctions.IsDisallowedForAbsoluteUri(robotsDotTextAbsoluteUri, false, false))
                {
                    if (crawlRequest.Politeness.DisallowedPaths == null || (crawlRequest.Politeness.DisallowedPaths != null && DateTime.Now.Subtract(crawlRequest.Politeness.DisallowedPathsSince) > TimeSpan.FromDays(1)))
                    {
                        CrawlRequest robotsDotTextRequest = new CrawlRequest(crawlRequest, Cache.GetDiscovery(robotsDotTextAbsoluteUri, arachnodeDAO), 1, 1, (short) UriClassificationType.Host, (short) UriClassificationType.Host, double.MaxValue, RenderType.None, RenderType.None);
                        robotsDotTextRequest.Discovery.DiscoveryState = DiscoveryState.Undiscovered;
                        robotsDotTextRequest.Politeness = crawlRequest.Politeness;

                        Crawl crawl = new Crawl(crawlRequest.Crawl.Crawler, false);

                        crawl.ProcessCrawlRequest(robotsDotTextRequest, false, false);

                        crawlRequest.Politeness.DisallowedPathsSince = DateTime.Now;

                        //The DataManager will not download the byte stream is ApplicationSettings.AssignFileAndImageDicoveries is set to false.  This is by design.
                        if (robotsDotTextRequest.Data != null && robotsDotTextRequest.Data.Length == 0 && robotsDotTextRequest.WebClient.WebException == null)
                        {
                            robotsDotTextRequest.Data = robotsDotTextRequest.WebClient.DownloadHttpData(crawlRequest.Discovery.Uri.AbsoluteUri, robotsDotTextRequest.WebClient.HttpWebResponse.ContentEncoding.ToLowerInvariant() == "gzip", robotsDotTextRequest.WebClient.HttpWebResponse.ContentEncoding.ToLowerInvariant() == "deflate", crawlRequest.Crawl.Crawler.CookieContainer);
                        }

                        SiteCrawler.Value.RobotsDotText robotsDotText = RobotsDotTextManager.ParseRobotsDotTextSource(new Uri(crawlRequest.Discovery.Uri.Scheme + Uri.SchemeDelimiter + crawlRequest.Discovery.Uri.Host), robotsDotTextRequest.Data);

                        crawlRequest.Politeness.CrawlDelayInMilliseconds = robotsDotText.CrawlDelay*1000;
                        crawlRequest.Politeness.DisallowedPaths = robotsDotText.DisallowedPaths;
                    }

                    if (crawlRequest.Politeness != null)
                    {
                        if (crawlRequest.Politeness.DisallowedPaths != null)
                        {
                            foreach (string disallowedPath in crawlRequest.Politeness.DisallowedPaths)
                            {
                                if (HttpUtility.UrlDecode(crawlRequest.Discovery.Uri.AbsoluteUri).StartsWith(HttpUtility.UrlDecode(disallowedPath)))
                                {
                                    crawlRequest.IsDisallowedReason = "Prohibited by robots.txt.";
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 	Determines whether the specified crawl request is disallowed.
        /// </summary>
        /// <param name = "discovery">The discovery.</param>
        /// <param name = "arachnodeDAO">The arachnode DAO.</param>
        /// <returns>
        /// 	<c>true</c> if the specified crawl request is disallowed; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsDisallowed(Discovery discovery, ArachnodeDAO arachnodeDAO)
        {
            return false;
        }

        /// <summary>
        /// 	Stops this instance.
        /// </summary>
        public override void Stop()
        {
        }
    }
}