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
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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

#endregion

namespace Arachnode.Plugins.Next
{
    public class HyperLinkDiscoveryPlugins<TDiscovery, TCrawlRequest> : APluginBase<TDiscovery, TCrawlRequest>
        where TDiscovery : ADiscovery
        where TCrawlRequest : ACrawlRequest
    {
        private readonly Regex _relRegularExpression = new Regex("rel\\s*=\\s*[\\\"\\'](?<Rel>.*?)[\\\"\\']", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public HyperLinkDiscoveryPlugins(Crawler<TDiscovery, TCrawlRequest> crawler, bool useSqlServer)
            : base(crawler, useSqlServer)
        {
        }

        public void DoNotCrawlHyperLinks(ACrawlRequest crawlRequest, DistributedCache hyperLinkAbsoluteUrisDistributedCache, SHA1 sha1)
        {
            crawlRequest.Discovery.HyperLinkDiscoveries.Clear();
        }

        public void ValidateHyperLinkDiscoveries(ACrawlRequest crawlRequest, DistributedCache hyperLinkAbsoluteUrisDistributedCache, SHA1 sha1)
        {
            //there may exist conditions where regular expression parsing misses a condition.  use a plugin to correct the condition.
            List<ADiscovery> fileAndImageDiscoveries = new List<ADiscovery>(crawlRequest.Discovery.FileAndImageDiscoveries.Count);

            foreach (ADiscovery fileAndImageDiscovery in crawlRequest.Discovery.FileAndImageDiscoveries)
            {
                Match match = _relRegularExpression.Match(fileAndImageDiscovery.Match.Value);

                if (!string.IsNullOrEmpty(match.Groups["Rel"].Value))
                {
                    if (match.Groups["Rel"].Value.ToLowerInvariant() == "alternate")
                    {
                        fileAndImageDiscoveries.Add(fileAndImageDiscovery);
                    }
                }
                else
                {
                    fileAndImageDiscoveries.Add(fileAndImageDiscovery);
                }
            }

            crawlRequest.Discovery.FileAndImageDiscoveries = fileAndImageDiscoveries;

            //there may exist conditions where regular expression parsing misses a condition.  use a plugin to correct the condition.
            crawlRequest.Discovery.HyperLinkDiscoveries = new List<ADiscovery>(crawlRequest.Discovery.HyperLinkDiscoveries.Where(hld => hld.Uri.Scheme.ToLowerInvariant() != "mailto"));
        }

        public void DoNotCrawlNamedAnchors(ACrawlRequest crawlRequest, DistributedCache hyperLinkAbsoluteUrisDistributedCache, SHA1 sha1)
        {
            crawlRequest.Discovery.HyperLinkDiscoveries = new List<ADiscovery>(crawlRequest.Discovery.HyperLinkDiscoveries.Where(hld => !hld.Uri.AbsoluteUri.Contains("#")));
        }

        public void DoNotCrawlQueryStrings(ACrawlRequest crawlRequest, DistributedCache hyperLinkAbsoluteUrisDistributedCache, SHA1 sha1)
        {
            crawlRequest.Discovery.HyperLinkDiscoveries = new List<ADiscovery>(crawlRequest.Discovery.HyperLinkDiscoveries.Where(hld => !hld.Uri.AbsoluteUri.Contains("?")));
        }

        public void RestrictHyperLinkDiscoveriesToParentDomain(ACrawlRequest crawlRequest, DistributedCache hyperLinkAbsoluteUrisDistributedCache, SHA1 sha1)
        {
            List<ADiscovery> hyperLinkDiscoveries = new List<ADiscovery>(crawlRequest.Discovery.HyperLinkDiscoveries.Count);

            foreach (ADiscovery hyperLinkDiscovery in crawlRequest.Discovery.HyperLinkDiscoveries)
            {
                if (UserDefinedFunctions.ExtractDomain(crawlRequest.Discovery.Parent.Uri.AbsoluteUri).Value == UserDefinedFunctions.ExtractDomain(hyperLinkDiscovery.Uri.AbsoluteUri).Value)
                {
                    hyperLinkDiscoveries.Add(hyperLinkDiscovery);
                }
            }

            crawlRequest.Discovery.HyperLinkDiscoveries = hyperLinkDiscoveries;
        }

        public void DetermineCachePresence(ACrawlRequest crawlRequest, DistributedCache hyperLinkAbsoluteUrisDistributedCache, SHA1 sha1)
        {
            lock (Crawler.CrawlRequestsToCrawlLock)
            {
                foreach (ADiscovery hyperLinkDiscovery in crawlRequest.Discovery.HyperLinkDiscoveries)
                {
                    if (hyperLinkAbsoluteUrisDistributedCache.Read("HLD_" + hyperLinkDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, sha1) == null)
                    {
                        hyperLinkAbsoluteUrisDistributedCache.Write("HLD_" + hyperLinkDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, hyperLinkDiscovery.Uri.AbsoluteUri, sha1, false);

                        if (UseSqlServer)
                        {
                            ArachnodeDAO arachnodeDAO = _arachnodeDAOs[crawlRequest.ThreadNumber];

                            ArachnodeDataSet.DiscoveriesRow discoveriesRow = arachnodeDAO.GetDiscovery(hyperLinkDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier);

                            if (discoveriesRow == null)
                            {
                                arachnodeDAO.InsertDiscovery(null, hyperLinkDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, 4, 0, false, 1);

                                hyperLinkDiscovery.Status = DiscoveryStatus.NotPresentInCache;
                            }
                            else
                            {
                                if (discoveriesRow.IsIDNull())
                                {
                                    arachnodeDAO.InsertDiscovery(null, hyperLinkDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, discoveriesRow.DiscoveryStateID, discoveriesRow.DiscoveryTypeID, discoveriesRow.ExpectFileOrImage, ++discoveriesRow.NumberOfTimesDiscovered);
                                }
                                else
                                {
                                    arachnodeDAO.InsertDiscovery(discoveriesRow.ID, hyperLinkDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, discoveriesRow.DiscoveryStateID, discoveriesRow.DiscoveryTypeID, discoveriesRow.ExpectFileOrImage, ++discoveriesRow.NumberOfTimesDiscovered);
                                }

                                hyperLinkDiscovery.Status = DiscoveryStatus.PresentInCache;
                            }
                        }
                        else
                        {
                            ArachnodeNextDAO arachnodeNextDAO = _arachnodeNextDAOs[crawlRequest.ThreadNumber];

                            ArachnodeNextDataSet.DiscoveriesRow discoveriesRow = arachnodeNextDAO.GetDiscovery(hyperLinkDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier);

                            if (discoveriesRow == null)
                            {
                                arachnodeNextDAO.InsertDiscovery(null, hyperLinkDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, 4, 0, false, 1);

                                hyperLinkDiscovery.Status = DiscoveryStatus.NotPresentInCache;
                            }
                            else
                            {
                                if (discoveriesRow.IsIDNull())
                                {
                                    arachnodeNextDAO.UpdateDiscovery(null, hyperLinkDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, discoveriesRow.DiscoveryStateID, discoveriesRow.DiscoveryTypeID, discoveriesRow.ExpectFileOrImage, ++discoveriesRow.NumberOfTimesDiscovered);
                                }
                                else
                                {
                                    arachnodeNextDAO.UpdateDiscovery(discoveriesRow.ID, hyperLinkDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, discoveriesRow.DiscoveryStateID, discoveriesRow.DiscoveryTypeID, discoveriesRow.ExpectFileOrImage, ++discoveriesRow.NumberOfTimesDiscovered);
                                }

                                hyperLinkDiscovery.Status = DiscoveryStatus.PresentInCache;
                            }
                        }
                    }
                    else
                    {
                        hyperLinkDiscovery.Status = DiscoveryStatus.PresentInCache;

                        continue;
                    }
                }
            }
        }

        public void SaveHyperLinkDiscoveriesToCache(ACrawlRequest crawlRequest, DistributedCache hyperLinkAbsoluteUrisDistributedCache, SHA1 sha1)
        {
            if (UseSqlServer)
            {
                ArachnodeDAO arachnodeDAO = _arachnodeDAOs[crawlRequest.ThreadNumber];

                foreach (ADiscovery hyperLinkDiscovery in crawlRequest.Discovery.HyperLinkDiscoveries)
                {
                    arachnodeDAO.InsertHyperLink(crawlRequest.Discovery.Parent.Uri.AbsoluteUri, hyperLinkDiscovery.Uri.AbsoluteUri);
                }
            }
            else
            {
                
            }
        }
    }
}