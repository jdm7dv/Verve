using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

namespace Arachnode.Plugins.Next
{
    public class CrawlerPlugins<TDiscovery, TCrawlRequest> : APluginBase<TDiscovery, TCrawlRequest>
        where TDiscovery : ADiscovery
        where TCrawlRequest : ACrawlRequest
    {
        public CrawlerPlugins(Crawler<TDiscovery, TCrawlRequest> crawler, bool useSqlServer)
            : base(crawler, useSqlServer)
        {
        }

        public List<TCrawlRequest> LoadCrawlRequestsFromPersistentStorage(int threadNumber, DistributedCache crawlRequestsDistributedCache, SHA1 sha1)
        {
            List<TCrawlRequest> crawlRequests = new List<TCrawlRequest>();

            if (UseSqlServer)
            {
                ArachnodeDAO arachnodeDAO = _arachnodeDAOs[threadNumber];

                foreach (ArachnodeDataSet.CrawlRequestsRow crawlRequestsRow in arachnodeDAO.GetCrawlRequests(1000, true, false, false, false, false, false, false, false, false))
                {
                    //remove the UniqueIdentifier...                
                    string absoluteUri2 = null;

                    if (!string.IsNullOrEmpty(Crawler.UniqueIdentifier))
                    {
                        absoluteUri2 = crawlRequestsRow.AbsoluteUri2.Substring(0, crawlRequestsRow.AbsoluteUri2.LastIndexOf(Crawler.UniqueIdentifier));
                    }
                    else
                    {
                        absoluteUri2 = crawlRequestsRow.AbsoluteUri2;
                    }

                    //child...
                    Uri uri2 = new Uri(absoluteUri2);

                    if (crawlRequestsDistributedCache.Read("CR_" + uri2.CacheKey() + Crawler.UniqueIdentifier, sha1) == null)
                    {
                        crawlRequestsDistributedCache.Write("CR_" + uri2.CacheKey() + Crawler.UniqueIdentifier, uri2.AbsoluteUri, sha1, false);

                        ArachnodeDataSet.DiscoveriesRow discoveriesRow = arachnodeDAO.GetDiscovery(uri2.CacheKey() + Crawler.UniqueIdentifier);

                        bool expectFileOrImage = false;

                        if (discoveriesRow == null)
                        {
                            arachnodeDAO.InsertDiscovery(null, uri2.CacheKey() + Crawler.UniqueIdentifier, 3, 0, false, 1);
                        }
                        else
                        {
                            expectFileOrImage = discoveriesRow.ExpectFileOrImage;

                            if (discoveriesRow.IsIDNull())
                            {
                                arachnodeDAO.InsertDiscovery(null, discoveriesRow.AbsoluteUri + Crawler.UniqueIdentifier, discoveriesRow.DiscoveryStateID, discoveriesRow.DiscoveryTypeID, discoveriesRow.ExpectFileOrImage, ++discoveriesRow.NumberOfTimesDiscovered);
                            }
                            else
                            {
                                arachnodeDAO.InsertDiscovery(discoveriesRow.ID, discoveriesRow.AbsoluteUri + Crawler.UniqueIdentifier, discoveriesRow.DiscoveryStateID, discoveriesRow.DiscoveryTypeID, discoveriesRow.ExpectFileOrImage, ++discoveriesRow.NumberOfTimesDiscovered);
                            }
                        }

                        /**/

                        //TODO: Unclear for the novice user...!!! Select * From dbo.DiscoveryStates
                        if (discoveriesRow == null || discoveriesRow.DiscoveryStateID == 4)
                        {
                            //remove the UniqueIdentifier...                
                            string absoluteUri1 = null;

                            if (!string.IsNullOrEmpty(Crawler.UniqueIdentifier))
                            {
                                absoluteUri1 = crawlRequestsRow.AbsoluteUri1.Substring(0, crawlRequestsRow.AbsoluteUri1.LastIndexOf(Crawler.UniqueIdentifier));
                            }
                            else
                            {
                                absoluteUri1 = crawlRequestsRow.AbsoluteUri1;
                            }

                            //parent...
                            Uri uri1 = new Uri(absoluteUri1);

                            object parent = Activator.CreateInstance(typeof (TDiscovery), uri1, false, DiscoveryStatus.Ok);

                            object discovery = Activator.CreateInstance(typeof (TDiscovery), uri2, expectFileOrImage, DiscoveryStatus.Ok);

                            object crawlRequest = Activator.CreateInstance(typeof (TCrawlRequest), threadNumber, parent, discovery, crawlRequestsRow.CurrentDepth, crawlRequestsRow.MaximumDepth, crawlRequestsRow.Priority);

                            crawlRequests.Add((TCrawlRequest) crawlRequest);
                        }
                    }
                }
            }
            else
            {
                ArachnodeNextDAO arachnodeNextDAO = _arachnodeNextDAOs[threadNumber];

                foreach (ArachnodeNextDataSet.CrawlRequestsRow crawlRequestsRow in arachnodeNextDAO.GetCrawlRequests(1000))
                {
                    //remove the UniqueIdentifier...                
                    string absoluteUri2 = null;

                    if (!string.IsNullOrEmpty(Crawler.UniqueIdentifier))
                    {
                        absoluteUri2 = crawlRequestsRow.AbsoluteUri2.Substring(0, crawlRequestsRow.AbsoluteUri2.LastIndexOf(Crawler.UniqueIdentifier));
                    }
                    else
                    {
                        absoluteUri2 = crawlRequestsRow.AbsoluteUri2;
                    }

                    //child...
                    Uri uri2 = new Uri(absoluteUri2);

                    if (crawlRequestsDistributedCache.Read("CR_" + uri2.CacheKey() + Crawler.UniqueIdentifier, sha1) == null)
                    {
                        crawlRequestsDistributedCache.Write("CR_" + uri2.CacheKey() + Crawler.UniqueIdentifier, uri2.AbsoluteUri, sha1, false);

                        ArachnodeNextDataSet.DiscoveriesRow discoveriesRow = arachnodeNextDAO.GetDiscovery(uri2.CacheKey() + Crawler.UniqueIdentifier);

                        bool expectFileOrImage = false;

                        if (discoveriesRow == null)
                        {
                            arachnodeNextDAO.InsertDiscovery(null, uri2.CacheKey() + Crawler.UniqueIdentifier, 3, 0, false, 1);
                        }
                        else
                        {
                            expectFileOrImage = discoveriesRow.ExpectFileOrImage;

                            if (discoveriesRow.IsIDNull())
                            {
                                arachnodeNextDAO.UpdateDiscovery(null, discoveriesRow.AbsoluteUri + Crawler.UniqueIdentifier, discoveriesRow.DiscoveryStateID, discoveriesRow.DiscoveryTypeID, discoveriesRow.ExpectFileOrImage, ++discoveriesRow.NumberOfTimesDiscovered);
                            }
                            else
                            {
                                arachnodeNextDAO.UpdateDiscovery(discoveriesRow.ID, discoveriesRow.AbsoluteUri + Crawler.UniqueIdentifier, discoveriesRow.DiscoveryStateID, discoveriesRow.DiscoveryTypeID, discoveriesRow.ExpectFileOrImage, ++discoveriesRow.NumberOfTimesDiscovered);
                            }
                        }

                        /**/

                        //TODO: Unclear for the novice user...!!! Select * From dbo.DiscoveryStates
                        if (discoveriesRow == null || discoveriesRow.DiscoveryStateID == 4)
                        {
                            //remove the UniqueIdentifier...                
                            string absoluteUri1 = null;

                            if (!string.IsNullOrEmpty(Crawler.UniqueIdentifier))
                            {
                                absoluteUri1 = crawlRequestsRow.AbsoluteUri1.Substring(0, crawlRequestsRow.AbsoluteUri1.LastIndexOf(Crawler.UniqueIdentifier));
                            }
                            else
                            {
                                absoluteUri1 = crawlRequestsRow.AbsoluteUri1;
                            }

                            //parent...
                            Uri uri1 = new Uri(absoluteUri1);

                            object parent = Activator.CreateInstance(typeof(TDiscovery), uri1, false, DiscoveryStatus.Ok);

                            object discovery = Activator.CreateInstance(typeof(TDiscovery), uri2, expectFileOrImage, DiscoveryStatus.Ok);

                            object crawlRequest = Activator.CreateInstance(typeof(TCrawlRequest), threadNumber, parent, discovery, crawlRequestsRow.CurrentDepth, crawlRequestsRow.MaximumDepth, crawlRequestsRow.Priority);

                            crawlRequests.Add((TCrawlRequest)crawlRequest);
                        }
                    }
                }
            }

            return crawlRequests;
        }

        public void SaveCrawlRequestToPersistentStorage(ACrawlRequest crawlRequest, DistributedCache crawlRequestsDistributedCache, SHA1 sha1)
        {
            //non-functioning for the DEMO...
            //if (UseSqlServer)
            //{
            //    ArachnodeDAO arachnodeDAO = _arachnodeDAOs[crawlRequest.ThreadNumber + 1];

            //    arachnodeDAO.InsertCrawlRequest(crawlRequest.Created, null, crawlRequest.Discovery.Parent.Uri.AbsoluteUri + Crawler.UniqueIdentifier, crawlRequest.Discovery.Uri.AbsoluteUri + Crawler.UniqueIdentifier, crawlRequest.CurrentDepth, crawlRequest.MaximumDepth, 0, 0, crawlRequest.Priority, 0, 0);
            //}
            //else
            //{
            //    ArachnodeNextDAO arachnodeNextDAO = _arachnodeNextDAOs[crawlRequest.ThreadNumber + 1];

            //    arachnodeNextDAO.InsertCrawlRequest(crawlRequest.Created, crawlRequest.Discovery.Parent.Uri.AbsoluteUri + Crawler.UniqueIdentifier, crawlRequest.Discovery.Uri.AbsoluteUri + Crawler.UniqueIdentifier, crawlRequest.CurrentDepth, crawlRequest.MaximumDepth, crawlRequest.Priority);
            //}
        }
    }
}
