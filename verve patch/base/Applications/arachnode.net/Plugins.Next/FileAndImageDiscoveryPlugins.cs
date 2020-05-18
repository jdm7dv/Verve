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
    public class FileAndImageDiscoveryPlugins<TDiscovery, TCrawlRequest> : APluginBase<TDiscovery, TCrawlRequest>
        where TDiscovery : ADiscovery
        where TCrawlRequest : ACrawlRequest
    {
        private readonly Regex _relRegularExpression = new Regex("rel\\s*=\\s*[\\\"\\'](?<Rel>.*?)[\\\"\\']", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        public FileAndImageDiscoveryPlugins(Crawler<TDiscovery, TCrawlRequest> crawler, bool useSqlServer)
            : base(crawler, useSqlServer)
        {
        }

        public void DoNotCrawlFilesAndImages(ACrawlRequest crawlRequest, DistributedCache fileAndImageAbsoluteUrisDistributedCache, SHA1 sha1)
        {
            crawlRequest.Discovery.FileAndImageDiscoveries.Clear();
        }

        public void ValidateFileAndImageDiscoveries(ACrawlRequest crawlRequest, DistributedCache fileAndImageAbsoluteUrisDistributedCache, SHA1 sha1)
        {
            //there may exist conditions where regular expression parsing misses a condition.  use a plugin to correct the condition.
            List<ADiscovery> fileAndImageDiscoveries = new List<ADiscovery>(crawlRequest.Discovery.FileAndImageDiscoveries.Count);

            foreach (ADiscovery fileAndImageDiscovery in crawlRequest.Discovery.FileAndImageDiscoveries)
            {
                Match match = _relRegularExpression.Match(fileAndImageDiscovery.Match.Value);

                if (!string.IsNullOrEmpty(match.Groups["Rel"].Value))
                {
                    if (match.Groups["Rel"].Value.ToLowerInvariant() == "stylesheet" ||
                        match.Groups["Rel"].Value.ToLowerInvariant() == "shortcut icon")
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
        }

        public void RestrictFileAndImageDiscoveriesToParentDomain(ACrawlRequest crawlRequest, DistributedCache fileAndImageAbsoluteUrisDistributedCache, SHA1 sha1)
        {
            List<ADiscovery> fileAndImageDiscoveries = new List<ADiscovery>(crawlRequest.Discovery.FileAndImageDiscoveries.Count);

            foreach (ADiscovery fileAndImageDiscovery in crawlRequest.Discovery.FileAndImageDiscoveries)
            {
                if (UserDefinedFunctions.ExtractDomain(crawlRequest.Discovery.Parent.Uri.AbsoluteUri).Value == UserDefinedFunctions.ExtractDomain(fileAndImageDiscovery.Uri.AbsoluteUri).Value)
                {
                    fileAndImageDiscoveries.Add(fileAndImageDiscovery);
                }
            }

            crawlRequest.Discovery.FileAndImageDiscoveries = fileAndImageDiscoveries;
        }

        public void DetermineCachePresence(ACrawlRequest crawlRequest, DistributedCache fileAndImageAbsoluteUrisDistributedCache, SHA1 sha1)
        {
            lock (Crawler.CrawlRequestsToCrawlLock)
            {
                foreach (ADiscovery fileAndImageDiscovery in crawlRequest.Discovery.FileAndImageDiscoveries)
                {
                    if (fileAndImageAbsoluteUrisDistributedCache.Read("FAID_" + fileAndImageDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, sha1) == null)
                    {
                        fileAndImageAbsoluteUrisDistributedCache.Write("FAID_" + fileAndImageDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, fileAndImageDiscovery.Uri.AbsoluteUri, sha1, false);

                        if (UseSqlServer)
                        {
                            ArachnodeDAO arachnodeDAO = _arachnodeDAOs[crawlRequest.ThreadNumber];

                            ArachnodeDataSet.DiscoveriesRow discoveriesRow = arachnodeDAO.GetDiscovery(fileAndImageDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier);

                            if (discoveriesRow == null)
                            {
                                arachnodeDAO.InsertDiscovery(null, fileAndImageDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, 4, 0, true, 1);

                                fileAndImageDiscovery.Status = DiscoveryStatus.NotPresentInCache;
                            }
                            else
                            {
                                if (discoveriesRow.IsIDNull())
                                {
                                    arachnodeDAO.InsertDiscovery(null, fileAndImageDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, discoveriesRow.DiscoveryStateID, discoveriesRow.DiscoveryTypeID, discoveriesRow.ExpectFileOrImage, ++discoveriesRow.NumberOfTimesDiscovered);
                                }
                                else
                                {
                                    arachnodeDAO.InsertDiscovery(discoveriesRow.ID, fileAndImageDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, discoveriesRow.DiscoveryStateID, discoveriesRow.DiscoveryTypeID, discoveriesRow.ExpectFileOrImage, ++discoveriesRow.NumberOfTimesDiscovered);
                                }

                                fileAndImageDiscovery.Status = DiscoveryStatus.PresentInCache;
                            }
                        }
                        else
                        {
                            ArachnodeNextDAO arachnodeNextDAO = _arachnodeNextDAOs[crawlRequest.ThreadNumber];

                            ArachnodeNextDataSet.DiscoveriesRow discoveriesRow = arachnodeNextDAO.GetDiscovery(fileAndImageDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier);

                            if (discoveriesRow == null)
                            {
                                arachnodeNextDAO.InsertDiscovery(null, fileAndImageDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, 4, 0, true, 1);

                                fileAndImageDiscovery.Status = DiscoveryStatus.NotPresentInCache;
                            }
                            else
                            {
                                if (discoveriesRow.IsIDNull())
                                {
                                    arachnodeNextDAO.UpdateDiscovery(null, fileAndImageDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, discoveriesRow.DiscoveryStateID, discoveriesRow.DiscoveryTypeID, discoveriesRow.ExpectFileOrImage, ++discoveriesRow.NumberOfTimesDiscovered);
                                }
                                else
                                {
                                    arachnodeNextDAO.UpdateDiscovery(discoveriesRow.ID, fileAndImageDiscovery.Uri.CacheKey() + Crawler.UniqueIdentifier, discoveriesRow.DiscoveryStateID, discoveriesRow.DiscoveryTypeID, discoveriesRow.ExpectFileOrImage, ++discoveriesRow.NumberOfTimesDiscovered);
                                }

                                fileAndImageDiscovery.Status = DiscoveryStatus.PresentInCache;
                            }
                        }
                    }
                    else
                    {
                        fileAndImageDiscovery.Status = DiscoveryStatus.PresentInCache;

                        continue;
                    }
                }
            }
        }
    }
}