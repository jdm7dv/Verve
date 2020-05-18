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

using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using Arachnode.Cache;
using Arachnode.DataAccess;
using Arachnode.DataSource;
using Arachnode.DataSource.Next;
using Arachnode.SiteCrawler.Next;
using Arachnode.SiteCrawler.Next.DataAccess;
using Arachnode.SiteCrawler.Next.ExtensionMethods;
using Arachnode.SiteCrawler.Next.Value;
using Arachnode.SiteCrawler.Next.Value.AbstractClasses;
using Arachnode.SiteCrawler.Next.Value.Enums;
using Arachnode.SiteCrawler.Next.ExtensionMethods;

#endregion

namespace Arachnode.Plugins.Next
{
    public class WebPagePlugins<TDiscovery, TCrawlRequest> : APluginBase<TDiscovery, TCrawlRequest>
        where TDiscovery : ADiscovery
        where TCrawlRequest : ACrawlRequest
    {
        public WebPagePlugins(Crawler<TDiscovery, TCrawlRequest> crawler, bool useSqlServer) 
            : base(crawler, useSqlServer)
        {
        }

        public void SaveWebPageToCache(ACrawlRequest crawlRequest, DistributedCache webPagesDistributedCache, SHA1 sha1)
        {
            CrawlRequest crawlRequest2 = null;

            if (crawlRequest is CrawlRequest)
            {
                crawlRequest2 = (CrawlRequest) crawlRequest;
            }
            else
            {
                return;
            }

            if (webPagesDistributedCache.Read("WP_" + crawlRequest.Discovery.Uri.CacheKey() + Crawler.UniqueIdentifier, sha1) == null)
            {
                webPagesDistributedCache.Write("WP_" + crawlRequest.Discovery.Uri.CacheKey() + Crawler.UniqueIdentifier, crawlRequest.Discovery.Uri.AbsoluteUri, sha1, false);
                webPagesDistributedCache.Write("FAID_" + crawlRequest.Discovery.Uri.CacheKey() + Crawler.UniqueIdentifier, crawlRequest.Discovery.Uri.AbsoluteUri, sha1, false);
                webPagesDistributedCache.Write("HLD_" + crawlRequest.Discovery.Uri.CacheKey() + Crawler.UniqueIdentifier, crawlRequest.Discovery.Uri.AbsoluteUri, sha1, false);
            }

            crawlRequest2.Discovery.Status = DiscoveryStatus.PresentInCache;

            if (UseSqlServer)
            {
                ArachnodeDAO arachnodeDAO = _arachnodeDAOs[crawlRequest.ThreadNumber];

                crawlRequest2.Discovery.ID = arachnodeDAO.InsertWebPage(crawlRequest.Discovery.Uri.AbsoluteUri, crawlRequest.HttpWebResponse.Headers.ToString(), crawlRequest.Discovery.Data, crawlRequest.Discovery.Encoding.CodePage, crawlRequest2.FullTextIndexType, crawlRequest.CurrentDepth);

                /**/

                ArachnodeDataSet.DiscoveriesRow discoveriesRow = arachnodeDAO.GetDiscovery(crawlRequest.Discovery.Uri.CacheKey() + Crawler.UniqueIdentifier);

                if (discoveriesRow == null)
                {
                    arachnodeDAO.InsertDiscovery(crawlRequest2.Discovery.ID, crawlRequest2.Discovery.Uri.CacheKey() + Crawler.UniqueIdentifier, 1, 7, false, 1);
                }
                else
                {
                    arachnodeDAO.InsertDiscovery(crawlRequest2.Discovery.ID, crawlRequest2.Discovery.Uri.CacheKey() + Crawler.UniqueIdentifier, 1, 7, false, discoveriesRow.NumberOfTimesDiscovered);
                }

                /**/

                arachnodeDAO.DeleteCrawlRequest(crawlRequest2.Discovery.Parent.Uri.AbsoluteUri + Crawler.UniqueIdentifier, crawlRequest2.Discovery.Uri.AbsoluteUri + Crawler.UniqueIdentifier);
            }
            else
            {
                ArachnodeNextDAO arachnodeNextDAO = _arachnodeNextDAOs[crawlRequest.ThreadNumber];

                //crawlRequest2.Discovery.ID = arachnodeNextDAO.InsertWebPage(crawlRequest.Discovery.Uri.AbsoluteUri, crawlRequest.HttpWebResponse.Headers.ToString(), crawlRequest.Discovery.Data, crawlRequest.Discovery.Encoding.CodePage, crawlRequest2.FullTextIndexType, crawlRequest.CurrentDepth);

                /**/

                ArachnodeNextDataSet.DiscoveriesRow discoveriesRow = arachnodeNextDAO.GetDiscovery(crawlRequest.Discovery.Uri.CacheKey() + Crawler.UniqueIdentifier);

                if (discoveriesRow == null)
                {
                    arachnodeNextDAO.InsertDiscovery(crawlRequest2.Discovery.ID, crawlRequest2.Discovery.Uri.CacheKey() + Crawler.UniqueIdentifier, 1, 7, false, 1);
                }
                else
                {
                    arachnodeNextDAO.UpdateDiscovery(crawlRequest2.Discovery.ID, crawlRequest2.Discovery.Uri.CacheKey() + Crawler.UniqueIdentifier, 1, 7, false, discoveriesRow.NumberOfTimesDiscovered);
                }

                /**/

                arachnodeNextDAO.DeleteCrawlRequest(crawlRequest2.Discovery.Parent.Uri.AbsoluteUri + Crawler.UniqueIdentifier, crawlRequest2.Discovery.Uri.AbsoluteUri + Crawler.UniqueIdentifier);
            }
        }

        public void WebPagePluginOne(ACrawlRequest crawlRequest, DistributedCache webPagesDistributedCache, SHA1 sha1)
        {
        }

        public void WebPagePluginTwo(ACrawlRequest crawlRequest, DistributedCache webPagesDistributedCache, SHA1 sha1)
        {
        }

        public void WebPagePluginThree(ACrawlRequest crawlRequest, DistributedCache webPagesDistributedCache, SHA1 sha1)
        {
        }
    }
}