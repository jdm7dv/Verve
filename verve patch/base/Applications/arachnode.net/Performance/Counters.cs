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

using System.Diagnostics;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

#endregion

namespace Arachnode.Performance
{
    public class Counters
    {
        private static readonly object _lock = new object();

        private static Counters _counters;

        #region arachnode.net:Cache

        private PerformanceCounter _cachedCrawlRequests;
        private PerformanceCounter _cachedDiscoveries;
        private PerformanceCounter _cachedPolitenesses;
        private PerformanceCounter _crawlRequestsAddedPerSecond;
        private PerformanceCounter _crawlRequestsRemovedPerSecond;
        private PerformanceCounter _discoveriesAddedPerSecond;
        private PerformanceCounter _discoveriesRemovedPerSecond;
        private int _numberOfCrawlRequests;
        private int _numberOfCrawlRequestsAdded;
        private int _numberOfCrawlRequestsRemoved;
        private int _numberOfDiscoveries;
        private int _numberOfDiscoveriesAdded;
        private int _numberOfDiscoveriesRemoved;
        private int _numberOfPolitenesses;
        private int _numberOfPolitenessesAdded;
        private int _numberOfPolitenessesRemoved;
        private PerformanceCounter _politenessesAddedPerSecond;
        private PerformanceCounter _politenessesRemovedPerSecond;

        #endregion

        #region arachnode.net:ArachnodeDAO

        private int _crawlRequestsDeleted;
        private PerformanceCounter _crawlRequestsDeletedPerSecond;
        private int _crawlRequestsInserted;
        private PerformanceCounter _crawlRequestsInsertedPerSecond;
        private int _disallowedAbsoluteUriDiscoveriesInserted;
        private PerformanceCounter _disallowedAbsoluteUriDiscoveriesInsertedPerSecond;
        private int _disallowedAbsoluteUrisInserted;
        private PerformanceCounter _disallowedAbsoluteUrisInsertedPerSecond;
        private int _emailAddressDiscoveriesInserted;
        private PerformanceCounter _emailAddressDiscoveriesInsertedPerSecond;
        private int _emailAddressesInserted;
        private PerformanceCounter _emailAddressesInsertedPerSecond;
        private int _exceptionsInserted;
        private PerformanceCounter _exceptionsInsertedPerSecond;
        private int _fileDiscoveriesInserted;
        private PerformanceCounter _fileDiscoveriesInsertedPerSecond;
        private int _fileMetaDataInserted;
        private PerformanceCounter _fileMetaDataInsertedPerSecond;
        private int _filesInserted;
        private PerformanceCounter _filesInsertedPerSecond;
        private int _hyperLinkDiscoveriesInserted;
        private PerformanceCounter _hyperLinkDiscoveriesInsertedPerSecond;
        private int _hyperLinksInserted;
        private PerformanceCounter _hyperLinksInsertedPerSecond;
        private int _imageDiscoveriesInserted;
        private PerformanceCounter _imageDiscoveriesInsertedPerSecond;
        private int _imageMetaDataInserted;
        private PerformanceCounter _imageMetaDataInsertedPerSecond;
        private int _imagesInserted;
        private PerformanceCounter _imagesInsertedPerSecond;
        private int _webPageMetaDataInserted;
        private PerformanceCounter _webPageMetaDataInsertedPerSecond;
        private int _webPagesInserted;
        private PerformanceCounter _webPagesInsertedPerSecond;
        private PerformanceCounter _webPagesInsertedTotal;

        #endregion

        #region arachnode.net:Crawl

        private PerformanceCounter _averageDepth;
        private int _crawlRequestsProcessed;
        private PerformanceCounter _crawlRequestsProcessedPerSecond;
        private double _currentDepth;
        private int _emailAddressesDiscovered;
        private PerformanceCounter _emailAddressesDiscoveredPerSecond;
        private int _filesAndImagesDiscovered;
        private PerformanceCounter _filesAndImagesDiscoveredPerSecond;
        private int _filesDiscovered;
        private PerformanceCounter _filesDiscoveredPerSecond;
        private int _hyperLinksDiscovered;
        private PerformanceCounter _hyperLinksDiscoveredPerSecond;
        private int _imagesDiscovered;
        private PerformanceCounter _imagesDiscoveredPerSecond;
        private long _totalBytesDiscovered;
        private PerformanceCounter _totalBytesDiscoveredPerSecond;
        private int _webPagesDiscovered;
        private PerformanceCounter _webPagesDiscoveredPerSecond;

        #endregion

        /// <summary>
        /// 	Initializes a new instance of the <see cref = "Counters" /> class.
        /// </summary>
        private Counters()
        {
        }

        /// <summary>
        /// 	Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static Counters GetInstance()
        {
            if (_counters == null)
            {
                lock (_lock)
                {
                    if (_counters == null)
                    {
                        _counters = new Counters();

                        _counters.InstantiatePeformanceCounters();
                    }
                }
            }

            return _counters;
        }

        /// <summary>
        /// 	Instantiates the peformance counters.
        /// </summary>
        private void InstantiatePeformanceCounters()
        {
            PerformanceCounter.CloseSharedResources();

            CreateArachnodeDotNetCacheCounters();
            CreateArachnodeDotNetArachnodeDAOCounters();
            CreateArachnodeDotNetCrawlCounters();

            Timer timer = new Timer(1000);

            timer.Elapsed += UpdatePerformanceCounters;

            timer.Start();
        }

        /// <summary>
        /// 	Updates the performance counters.
        /// </summary>
        /// <param name = "sender">The sender.</param>
        /// <param name = "e">The <see cref = "System.Timers.ElapsedEventArgs" /> instance containing the event data.</param>
        private void UpdatePerformanceCounters(object sender, ElapsedEventArgs e)
        {
            #region arachnode.net:Cache

            _cachedCrawlRequests.RawValue = _numberOfCrawlRequests;
            _cachedDiscoveries.RawValue = _numberOfDiscoveries;
            _cachedPolitenesses.RawValue = _numberOfPolitenesses;

            _crawlRequestsAddedPerSecond.RawValue = _numberOfCrawlRequestsAdded;
            _discoveriesAddedPerSecond.RawValue = _numberOfDiscoveriesAdded;
            _politenessesAddedPerSecond.RawValue = _numberOfPolitenessesAdded;

            _numberOfCrawlRequestsAdded = 0;
            _numberOfDiscoveriesAdded = 0;
            _numberOfPolitenessesAdded = 0;

            _crawlRequestsRemovedPerSecond.RawValue = _numberOfCrawlRequestsRemoved;
            _discoveriesRemovedPerSecond.RawValue = _numberOfDiscoveriesRemoved;
            _politenessesRemovedPerSecond.RawValue = _numberOfPolitenessesRemoved;

            _numberOfCrawlRequestsRemoved = 0;
            _numberOfDiscoveriesRemoved = 0;
            _numberOfPolitenessesRemoved = 0;

            #endregion

            #region arachnode.net:ArachnodeDAO

            _crawlRequestsDeletedPerSecond.RawValue = _crawlRequestsDeleted;
            _crawlRequestsInsertedPerSecond.RawValue = _crawlRequestsInserted;
            _disallowedAbsoluteUrisInsertedPerSecond.RawValue = _disallowedAbsoluteUrisInserted;
            _disallowedAbsoluteUriDiscoveriesInsertedPerSecond.RawValue = _disallowedAbsoluteUriDiscoveriesInserted;
            _emailAddressesInsertedPerSecond.RawValue = _emailAddressesInserted;
            _emailAddressDiscoveriesInsertedPerSecond.RawValue = _emailAddressDiscoveriesInserted;
            _exceptionsInsertedPerSecond.RawValue = _exceptionsInserted;
            _filesInsertedPerSecond.RawValue = _filesInserted;
            _fileDiscoveriesInsertedPerSecond.RawValue = _fileDiscoveriesInserted;
            _fileMetaDataInsertedPerSecond.RawValue = _fileMetaDataInserted;
            _hyperLinksInsertedPerSecond.RawValue = _hyperLinksInserted;
            _hyperLinkDiscoveriesInsertedPerSecond.RawValue = _hyperLinkDiscoveriesInserted;
            _imagesInsertedPerSecond.RawValue = _imagesInserted;
            _imageDiscoveriesInsertedPerSecond.RawValue = _imageDiscoveriesInserted;
            _imageMetaDataInsertedPerSecond.RawValue = _imageMetaDataInserted;
            _webPagesInsertedPerSecond.RawValue = _webPagesInserted;
            _webPagesInsertedTotal.RawValue += _webPagesInserted;
            _webPageMetaDataInsertedPerSecond.RawValue = _webPageMetaDataInserted;

            _crawlRequestsDeleted = 0;
            _crawlRequestsInserted = 0;
            _disallowedAbsoluteUrisInserted = 0;
            _disallowedAbsoluteUriDiscoveriesInserted = 0;
            _emailAddressesInserted = 0;
            _emailAddressDiscoveriesInserted = 0;
            _exceptionsInserted = 0;
            _filesInserted = 0;
            _fileDiscoveriesInserted = 0;
            _fileMetaDataInserted = 0;
            _hyperLinksInserted = 0;
            _hyperLinkDiscoveriesInserted = 0;
            _imagesInserted = 0;
            _imageDiscoveriesInserted = 0;
            _imageMetaDataInserted = 0;
            _webPagesInserted = 0;
            _webPageMetaDataInserted = 0;

            #endregion

            #region arachnode.net:Crawl

            if (_currentDepth != 0 && _crawlRequestsProcessed != 0)
            {
                try
                {
                    _averageDepth.RawValue = (long) _currentDepth/_crawlRequestsProcessed;
                }
                catch
                {
                    _averageDepth.RawValue = 0;    
                }
            }
            else
            {
                _averageDepth.RawValue = 0;
            }

            _crawlRequestsProcessedPerSecond.RawValue = _crawlRequestsProcessed;

            _emailAddressesDiscoveredPerSecond.RawValue = _emailAddressesDiscovered;
            _filesAndImagesDiscoveredPerSecond.RawValue = _filesAndImagesDiscovered;
            _filesDiscoveredPerSecond.RawValue = _filesDiscovered;
            _hyperLinksDiscoveredPerSecond.RawValue = _hyperLinksDiscovered;
            _imagesDiscoveredPerSecond.RawValue = _imagesDiscovered;
            _webPagesDiscoveredPerSecond.RawValue = _webPagesDiscovered;
            _totalBytesDiscoveredPerSecond.RawValue = _totalBytesDiscovered;

            _currentDepth = 0;

            _crawlRequestsProcessed = 0;

            _emailAddressesDiscovered = 0;
            _filesAndImagesDiscovered = 0;
            _filesDiscovered = 0;
            _hyperLinksDiscovered = 0;
            _imagesDiscovered = 0;
            _webPagesDiscovered = 0;
            _totalBytesDiscovered = 0;

            #endregion
        }

        #region arachnode.net:Cache

        /// <summary>
        /// 	CrawlRequests the added.
        /// </summary>
        public void CrawlRequestAdded()
        {
            Interlocked.Increment(ref _numberOfCrawlRequests);

            _numberOfCrawlRequestsAdded++;
        }

        /// <summary>
        /// 	CrawlRequests the removed.
        /// </summary>
        public void CrawlRequestRemoved()
        {
            Interlocked.Decrement(ref _numberOfCrawlRequests);

            _numberOfCrawlRequestsRemoved++;
        }

        /// <summary>
        /// 	Discoveries the added.
        /// </summary>
        public void DiscoveryAdded()
        {
            Interlocked.Increment(ref _numberOfDiscoveries);

            _numberOfDiscoveriesAdded++;
        }

        /// <summary>
        /// 	Discoveries the removed.
        /// </summary>
        public void DiscoveryRemoved()
        {
            Interlocked.Decrement(ref _numberOfDiscoveries);

            _numberOfDiscoveriesRemoved++;
        }

        /// <summary>
        /// 	Politenesses the added.
        /// </summary>
        public void PolitenessAdded()
        {
            Interlocked.Increment(ref _numberOfPolitenesses);

            _numberOfPolitenessesAdded++;
        }

        /// <summary>
        /// 	Politenesses the removed.
        /// </summary>
        public void PolitenessRemoved()
        {
            Interlocked.Decrement(ref _numberOfPolitenesses);

            _numberOfPolitenessesRemoved++;
        }

        #endregion

        #region arachnode.net:ArachnodeDAO

        /// <summary>
        /// 	Crawls the request deleted.
        /// </summary>
        public void CrawlRequestDeleted()
        {
            _crawlRequestsDeleted++;
        }

        /// <summary>
        /// 	Crawls the request inserted.
        /// </summary>
        public void CrawlRequestInserted()
        {
            _crawlRequestsInserted++;
        }

        /// <summary>
        /// 	Disalloweds the absolute URI inserted.
        /// </summary>
        public void DisallowedAbsoluteUriInserted()
        {
            _disallowedAbsoluteUrisInserted++;
        }

        /// <summary>
        /// 	Disalloweds the absolute URI discovery inserted.
        /// </summary>
        public void DisallowedAbsoluteUriDiscoveryInserted()
        {
            _disallowedAbsoluteUriDiscoveriesInserted++;
        }

        /// <summary>
        /// 	Emails the address inserted.
        /// </summary>
        public void EmailAddressInserted()
        {
            _emailAddressesInserted++;
        }

        /// <summary>
        /// 	Emails the address discovery inserted.
        /// </summary>
        public void EmailAddressDiscoveryInserted()
        {
            _emailAddressDiscoveriesInserted++;
        }

        /// <summary>
        /// 	Exceptions the inserted.
        /// </summary>
        public void ExceptionInserted()
        {
            _exceptionsInserted++;
        }

        /// <summary>
        /// 	Files the inserted.
        /// </summary>
        public void FileInserted()
        {
            _filesInserted++;
        }

        /// <summary>
        /// 	Files the discovery inserted.
        /// </summary>
        public void FileDiscoveryInserted()
        {
            _fileDiscoveriesInserted++;
        }

        /// <summary>
        /// 	Files the meta data inserted.
        /// </summary>
        public void FileMetaDataInserted()
        {
            _fileMetaDataInserted++;
        }

        /// <summary>
        /// 	Hypers the link inserted.
        /// </summary>
        public void HyperLinkInserted()
        {
            _hyperLinksInserted++;
        }

        /// <summary>
        /// 	Hypers the link discovery inserted.
        /// </summary>
        public void HyperLinkDiscoveryInserted()
        {
            _hyperLinkDiscoveriesInserted++;
        }

        /// <summary>
        /// 	Images the inserted.
        /// </summary>
        public void ImageInserted()
        {
            _imagesInserted++;
        }

        /// <summary>
        /// 	Images the discovery inserted.
        /// </summary>
        public void ImageDiscoveryInserted()
        {
            _imageDiscoveriesInserted++;
        }

        /// <summary>
        /// 	Images the meta data inserted.
        /// </summary>
        public void ImageMetaDataInserted()
        {
            _imageMetaDataInserted++;
        }

        /// <summary>
        /// 	Webs the page inserted.
        /// </summary>
        public void WebPageInserted()
        {
            _webPagesInserted++;
        }

        /// <summary>
        /// 	Webs the page meta data inserted.
        /// </summary>
        public void WebPageMetaDataInserted()
        {
            _webPageMetaDataInserted++;
        }

        #endregion

        #region arachnode.net:Crawl

        /// <summary>
        /// 	Reports the current depth.
        /// </summary>
        /// <param name = "depth">The current depth.</param>
        public void ReportCurrentDepth(int currentDepth)
        {
            _currentDepth += currentDepth;
        }

        /// <summary>
        /// 	Crawls the request processed.
        /// </summary>
        public void CrawlRequestProcessed()
        {
            _crawlRequestsProcessed++;
        }

        /// <summary>
        /// 	Emails the addresses discovered.
        /// </summary>
        /// <param name = "numberOfEmailAddressesDiscovered">The number of email addresses discovered.</param>
        public void EmailAddressesDiscovered(int numberOfEmailAddressesDiscovered)
        {
            _emailAddressesDiscovered += numberOfEmailAddressesDiscovered;
        }

        public void FilesAndImagesDiscovered(int numberOfFilesAndImagesDiscovered)
        {
            _filesAndImagesDiscovered += numberOfFilesAndImagesDiscovered;
        }

        /// <summary>
        /// 	Fileses the discovered.
        /// </summary>
        /// <param name = "numberOfFilesDiscovered">The number of files discovered.</param>
        public void FilesDiscovered(int numberOfFilesDiscovered)
        {
            _filesDiscovered += numberOfFilesDiscovered;
        }

        /// <summary>
        /// 	Hypers the links discovered.
        /// </summary>
        /// <param name = "numberOfHyperLinksDiscovered">The number of hyper links discovered.</param>
        public void HyperLinksDiscovered(int numberOfHyperLinksDiscovered)
        {
            _hyperLinksDiscovered += numberOfHyperLinksDiscovered;
        }

        /// <summary>
        /// 	Imageses the discovered.
        /// </summary>
        /// <param name = "numberOfImagesDiscovered">The number of images discovered.</param>
        public void ImagesDiscovered(int numberOfImagesDiscovered)
        {
            _imagesDiscovered += numberOfImagesDiscovered;
        }

        /// <summary>
        /// 	Webs the pages discovered.
        /// </summary>
        /// <param name = "numberOfWebPagesDiscovered">The number of web pages discovered.</param>
        public void WebPagesDiscovered(int numberOfWebPagesDiscovered)
        {
            _webPagesDiscovered += numberOfWebPagesDiscovered;
        }

        /// <summary>
        /// 	Totals the bytes discovered.
        /// </summary>
        /// <param name = "numberOfTotalBytesDiscovered">The number of total bytes discovered.</param>
        public void TotalBytesDiscovered(long numberOfTotalBytesDiscovered)
        {
            _totalBytesDiscovered += numberOfTotalBytesDiscovered;
        }

        #endregion

        #region arachnode.net:Cache

        /// <summary>
        /// 	Creates the arachnode dot net cache counters.
        /// </summary>
        private void CreateArachnodeDotNetCacheCounters()
        {
            if (!PerformanceCounterCategory.Exists("arachnode.net:Cache"))
            {
                CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();

                AddCacheCountsCounters(counterCreationDataCollection);

                AddCacheAddedCounters(counterCreationDataCollection);

                AddCacheRemovedCounters(counterCreationDataCollection);

                PerformanceCounterCategory.Create("arachnode.net:Cache", "arachnode.net:Cache", PerformanceCounterCategoryType.SingleInstance, counterCreationDataCollection);
            }

            /**/

            _cachedCrawlRequests = new PerformanceCounter("arachnode.net:Cache", "arachnode.net - Cached CrawlRequests", false);
            _cachedDiscoveries = new PerformanceCounter("arachnode.net:Cache", "arachnode.net - Cached Discoveries", false);
            _cachedPolitenesses = new PerformanceCounter("arachnode.net:Cache", "arachnode.net - Cached Politenesses", false);

            /**/

            _crawlRequestsAddedPerSecond = new PerformanceCounter("arachnode.net:Cache", "arachnode.net - CrawlRequests Added/s", false);
            _discoveriesAddedPerSecond = new PerformanceCounter("arachnode.net:Cache", "arachnode.net - Discoveries Added/s", false);
            _politenessesAddedPerSecond = new PerformanceCounter("arachnode.net:Cache", "arachnode.net - Politenesses Added/s", false);

            /**/

            _crawlRequestsRemovedPerSecond = new PerformanceCounter("arachnode.net:Cache", "arachnode.net - CrawlRequests Removed/s", false);
            _discoveriesRemovedPerSecond = new PerformanceCounter("arachnode.net:Cache", "arachnode.net - Discoveries Removed/s", false);
            _politenessesRemovedPerSecond = new PerformanceCounter("arachnode.net:Cache", "arachnode.net - Politenesses Removed/s", false);
        }

        /// <summary>
        /// 	Adds the cache counts counters.
        /// </summary>
        /// <param name = "counterCreationDataCollection">The counter creation data collection.</param>
        private void AddCacheCountsCounters(CounterCreationDataCollection counterCreationDataCollection)
        {
            CounterCreationData cachedCrawlRequests = new CounterCreationData();

            cachedCrawlRequests.CounterHelp = "arachnode.net - Cached CrawlRequests";
            cachedCrawlRequests.CounterName = "arachnode.net - Cached CrawlRequests";
            cachedCrawlRequests.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(cachedCrawlRequests);

            /**/

            CounterCreationData cachedDiscoveries = new CounterCreationData();

            cachedDiscoveries.CounterHelp = "arachnode.net - Cached Discoveries";
            cachedDiscoveries.CounterName = "arachnode.net - Cached Discoveries";
            cachedDiscoveries.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(cachedDiscoveries);

            /**/

            CounterCreationData cachedPolitenesses = new CounterCreationData();

            cachedPolitenesses.CounterHelp = "arachnode.net - Cached Politenesses";
            cachedPolitenesses.CounterName = "arachnode.net - Cached Politenesses";
            cachedPolitenesses.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(cachedPolitenesses);
        }

        /// <summary>
        /// 	Adds the cache added counters.
        /// </summary>
        /// <param name = "counterCreationDataCollection">The counter creation data collection.</param>
        private void AddCacheAddedCounters(CounterCreationDataCollection counterCreationDataCollection)
        {
            CounterCreationData crawlRequestsAddedPerSecond = new CounterCreationData();

            crawlRequestsAddedPerSecond.CounterHelp = "arachnode.net - CrawlRequests Added/s";
            crawlRequestsAddedPerSecond.CounterName = "arachnode.net - CrawlRequests Added/s";
            crawlRequestsAddedPerSecond.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(crawlRequestsAddedPerSecond);

            /**/

            CounterCreationData discoveriesAddedPerSecond = new CounterCreationData();

            discoveriesAddedPerSecond.CounterHelp = "arachnode.net - Discoveries Added/s";
            discoveriesAddedPerSecond.CounterName = "arachnode.net - Discoveries Added/s";
            discoveriesAddedPerSecond.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(discoveriesAddedPerSecond);

            /**/

            CounterCreationData politenessesAddedPerSecond = new CounterCreationData();

            politenessesAddedPerSecond.CounterHelp = "arachnode.net - Politenesses Added/s";
            politenessesAddedPerSecond.CounterName = "arachnode.net - Politenesses Added/s";
            politenessesAddedPerSecond.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(politenessesAddedPerSecond);
        }

        /// <summary>
        /// 	Adds the cache removed counters.
        /// </summary>
        /// <param name = "counterCreationDataCollection">The counter creation data collection.</param>
        private void AddCacheRemovedCounters(CounterCreationDataCollection counterCreationDataCollection)
        {
            CounterCreationData crawlRequestsRemovedPerSecond = new CounterCreationData();

            crawlRequestsRemovedPerSecond.CounterHelp = "arachnode.net - CrawlRequests Removed/s";
            crawlRequestsRemovedPerSecond.CounterName = "arachnode.net - CrawlRequests Removed/s";
            crawlRequestsRemovedPerSecond.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(crawlRequestsRemovedPerSecond);

            /**/

            CounterCreationData discoveriesRemovedPerSecond = new CounterCreationData();

            discoveriesRemovedPerSecond.CounterHelp = "arachnode.net - Discoveries Removed/s";
            discoveriesRemovedPerSecond.CounterName = "arachnode.net - Discoveries Removed/s";
            discoveriesRemovedPerSecond.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(discoveriesRemovedPerSecond);

            /**/

            CounterCreationData politenessesRemovedPerSecond = new CounterCreationData();

            politenessesRemovedPerSecond.CounterHelp = "arachnode.net - Politenesses Removed/s";
            politenessesRemovedPerSecond.CounterName = "arachnode.net - Politenesses Removed/s";
            politenessesRemovedPerSecond.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(politenessesRemovedPerSecond);
        }

        #endregion

        #region arachnode.net:ArachnodeDAO

        /// <summary>
        /// 	Creates the arachnode dot net arachnode DAO counters.
        /// </summary>
        private void CreateArachnodeDotNetArachnodeDAOCounters()
        {
            if (!PerformanceCounterCategory.Exists("arachnode.net:ArachnodeDAO"))
            {
                CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();

                AddArachnodeDAOInsertedCounters(counterCreationDataCollection);

                PerformanceCounterCategory.Create("arachnode.net:ArachnodeDAO", "arachnode.net:ArachnodeDAO", PerformanceCounterCategoryType.SingleInstance, counterCreationDataCollection);
            }

            /**/

            _crawlRequestsDeletedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - CrawlRequests Deleted/s", false);
            _crawlRequestsInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - CrawlRequests Inserted/s", false);
            _disallowedAbsoluteUrisInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - Disallowed AbsoluteUris Inserted/s", false);
            _disallowedAbsoluteUriDiscoveriesInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - Disallowed AbsoluteUris Discoveries Inserted/s", false);
            _emailAddressesInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - EmailAddresses Inserted/s", false);
            _emailAddressDiscoveriesInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - EmailAddress Discoveries Inserted/s", false);
            _exceptionsInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - Exceptions Inserted/s", false);
            _filesInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - Files Inserted/s", false);
            _fileDiscoveriesInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - File Discoveries Inserted/s", false);
            _fileMetaDataInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - File MetaData Inserted/s", false);
            _hyperLinksInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - HyperLinks Inserted/s", false);
            _hyperLinkDiscoveriesInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - HyperLink Discoveries Inserted/s", false);
            _imagesInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - Images Inserted/s", false);
            _imageDiscoveriesInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - Image Discoveries Inserted/s", false);
            _imageMetaDataInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - Image MetaData Inserted/s", false);
            _webPagesInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - WebPages Inserted/s", false);
            _webPagesInsertedTotal = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - WebPages Inserted (Total)", false);
            //ANODET: Finish the 'Totals' counters...
            _webPageMetaDataInsertedPerSecond = new PerformanceCounter("arachnode.net:ArachnodeDAO", "arachnode.net - WebPage MetaData Inserted/s", false);
        }

        /// <summary>
        /// 	Adds the arachnode DAO inserted counters.
        /// </summary>
        /// <param name = "counterCreationDataCollection">The counter creation data collection.</param>
        private void AddArachnodeDAOInsertedCounters(CounterCreationDataCollection counterCreationDataCollection)
        {
            CounterCreationData crawlRequestsDeleted = new CounterCreationData();

            crawlRequestsDeleted.CounterHelp = "arachnode.net - CrawlRequests Deleted/s";
            crawlRequestsDeleted.CounterName = "arachnode.net - CrawlRequests Deleted/s";
            crawlRequestsDeleted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(crawlRequestsDeleted);

            /**/

            CounterCreationData crawlRequestsInserted = new CounterCreationData();

            crawlRequestsInserted.CounterHelp = "arachnode.net - CrawlRequests Inserted/s";
            crawlRequestsInserted.CounterName = "arachnode.net - CrawlRequests Inserted/s";
            crawlRequestsInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(crawlRequestsInserted);

            /**/

            CounterCreationData disallowedAbsoluteUrisInserted = new CounterCreationData();

            disallowedAbsoluteUrisInserted.CounterHelp = "arachnode.net - Disallowed AbsoluteUris Inserted/s";
            disallowedAbsoluteUrisInserted.CounterName = "arachnode.net - Disallowed AbsoluteUris Inserted/s";
            disallowedAbsoluteUrisInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(disallowedAbsoluteUrisInserted);

            /**/

            CounterCreationData disallowedAbsoluteUriDiscoveriesInserted = new CounterCreationData();

            disallowedAbsoluteUriDiscoveriesInserted.CounterHelp = "arachnode.net - Disallowed AbsoluteUris Discoveries Inserted/s";
            disallowedAbsoluteUriDiscoveriesInserted.CounterName = "arachnode.net - Disallowed AbsoluteUris Discoveries Inserted/s";
            disallowedAbsoluteUriDiscoveriesInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(disallowedAbsoluteUriDiscoveriesInserted);

            /**/

            CounterCreationData emailAddressesInserted = new CounterCreationData();

            emailAddressesInserted.CounterHelp = "arachnode.net - EmailAddresses Inserted/s";
            emailAddressesInserted.CounterName = "arachnode.net - EmailAddresses Inserted/s";
            emailAddressesInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(emailAddressesInserted);

            /**/

            CounterCreationData emailAddressDiscoveriesInserted = new CounterCreationData();

            emailAddressDiscoveriesInserted.CounterHelp = "arachnode.net - EmailAddress Discoveries Inserted/s";
            emailAddressDiscoveriesInserted.CounterName = "arachnode.net - EmailAddress Discoveries Inserted/s";
            emailAddressDiscoveriesInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(emailAddressDiscoveriesInserted);

            /**/

            CounterCreationData exceptionsInserted = new CounterCreationData();

            exceptionsInserted.CounterHelp = "arachnode.net - Exceptions Inserted/s";
            exceptionsInserted.CounterName = "arachnode.net - Exceptions Inserted/s";
            exceptionsInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(exceptionsInserted);

            /**/

            CounterCreationData filesInserted = new CounterCreationData();

            filesInserted.CounterHelp = "arachnode.net - Files Inserted/s";
            filesInserted.CounterName = "arachnode.net - Files Inserted/s";
            filesInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(filesInserted);

            /**/

            CounterCreationData fileDiscoveiesInserted = new CounterCreationData();

            fileDiscoveiesInserted.CounterHelp = "arachnode.net - File Discoveries Inserted/s";
            fileDiscoveiesInserted.CounterName = "arachnode.net - File Discoveries Inserted/s";
            fileDiscoveiesInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(fileDiscoveiesInserted);

            /**/

            CounterCreationData fileMetaDataInserted = new CounterCreationData();

            fileMetaDataInserted.CounterHelp = "arachnode.net - File MetaData Inserted/s";
            fileMetaDataInserted.CounterName = "arachnode.net - File MetaData Inserted/s";
            fileMetaDataInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(fileMetaDataInserted);

            /**/

            CounterCreationData hyperLinksInserted = new CounterCreationData();

            hyperLinksInserted.CounterHelp = "arachnode.net - HyperLinks Inserted/s";
            hyperLinksInserted.CounterName = "arachnode.net - HyperLinks Inserted/s";
            hyperLinksInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(hyperLinksInserted);

            /**/

            CounterCreationData hyperLinkDiscoveriesInserted = new CounterCreationData();

            hyperLinkDiscoveriesInserted.CounterHelp = "arachnode.net - HyperLink Discoveries Inserted/s";
            hyperLinkDiscoveriesInserted.CounterName = "arachnode.net - HyperLink Discoveries Inserted/s";
            hyperLinkDiscoveriesInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(hyperLinkDiscoveriesInserted);

            /**/

            CounterCreationData imagesInserted = new CounterCreationData();

            imagesInserted.CounterHelp = "arachnode.net - Images Inserted/s";
            imagesInserted.CounterName = "arachnode.net - Images Inserted/s";
            imagesInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(imagesInserted);

            /**/

            CounterCreationData imageDiscoveiesInserted = new CounterCreationData();

            imageDiscoveiesInserted.CounterHelp = "arachnode.net - Image Discoveries Inserted/s";
            imageDiscoveiesInserted.CounterName = "arachnode.net - Image Discoveries Inserted/s";
            imageDiscoveiesInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(imageDiscoveiesInserted);

            /**/

            CounterCreationData imageMetaDataInserted = new CounterCreationData();

            imageMetaDataInserted.CounterHelp = "arachnode.net - Image MetaData Inserted/s";
            imageMetaDataInserted.CounterName = "arachnode.net - Image MetaData Inserted/s";
            imageMetaDataInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(imageMetaDataInserted);

            /**/

            CounterCreationData webPagesInserted = new CounterCreationData();

            webPagesInserted.CounterHelp = "arachnode.net - WebPages Inserted/s";
            webPagesInserted.CounterName = "arachnode.net - WebPages Inserted/s";
            webPagesInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(webPagesInserted);

            /**/

            CounterCreationData webPageMetaDataInserted = new CounterCreationData();

            webPageMetaDataInserted.CounterHelp = "arachnode.net - WebPage MetaData Inserted/s";
            webPageMetaDataInserted.CounterName = "arachnode.net - WebPage MetaData Inserted/s";
            webPageMetaDataInserted.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(webPageMetaDataInserted);

            /**/
        }

        #endregion

        #region arachnode.net:Crawl

        /// <summary>
        /// 	Creates the arachnode dot net crawl counters.
        /// </summary>
        private void CreateArachnodeDotNetCrawlCounters()
        {
            if (!PerformanceCounterCategory.Exists("arachnode.net:Crawl"))
            {
                CounterCreationDataCollection counterCreationDataCollection = new CounterCreationDataCollection();

                AddCrawlCrawlRequestsProcessedCounters(counterCreationDataCollection);

                PerformanceCounterCategory.Create("arachnode.net:Crawl", "arachnode.net:Crawl", PerformanceCounterCategoryType.SingleInstance, counterCreationDataCollection);
            }

            /**/

            _averageDepth = new PerformanceCounter("arachnode.net:Crawl", "arachnode.net - Average Depth", false);

            _crawlRequestsProcessedPerSecond = new PerformanceCounter("arachnode.net:Crawl", "arachnode.net - CrawlRequests Processed/s", false);

            _emailAddressesDiscoveredPerSecond = new PerformanceCounter("arachnode.net:Crawl", "arachnode.net - EmailAddresses Discovered/s", false);
            _filesDiscoveredPerSecond = new PerformanceCounter("arachnode.net:Crawl", "arachnode.net - Files Discovered/s", false);
            _filesAndImagesDiscoveredPerSecond = new PerformanceCounter("arachnode.net:Crawl", "arachnode.net - Files and Images Discovered/s", false);
            _hyperLinksDiscoveredPerSecond = new PerformanceCounter("arachnode.net:Crawl", "arachnode.net - HyperLinks Discovered/s", false);
            _imagesDiscoveredPerSecond = new PerformanceCounter("arachnode.net:Crawl", "arachnode.net - Images Discovered/s", false);
            _webPagesDiscoveredPerSecond = new PerformanceCounter("arachnode.net:Crawl", "arachnode.net - WebPages Discovered/s", false);
            _totalBytesDiscoveredPerSecond = new PerformanceCounter("arachnode.net:Crawl", "arachnode.net - Total Bytes Discovered/s", false);
        }

        /// <summary>
        /// 	Adds the crawl crawl requests processed counters.
        /// </summary>
        /// <param name = "counterCreationDataCollection">The counter creation data collection.</param>
        private void AddCrawlCrawlRequestsProcessedCounters(CounterCreationDataCollection counterCreationDataCollection)
        {
            CounterCreationData crawlRequestsProcessed = new CounterCreationData();

            crawlRequestsProcessed.CounterHelp = "arachnode.net - CrawlRequests Processed/s";
            crawlRequestsProcessed.CounterName = "arachnode.net - CrawlRequests Processed/s";
            crawlRequestsProcessed.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(crawlRequestsProcessed);

            /**/

            CounterCreationData emailAddressDiscovered = new CounterCreationData();

            emailAddressDiscovered.CounterHelp = "arachnode.net - EmailAddresses Discovered/s";
            emailAddressDiscovered.CounterName = "arachnode.net - EmailAddresses Discovered/s";
            emailAddressDiscovered.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(emailAddressDiscovered);

            /**/

            CounterCreationData filesDiscovered = new CounterCreationData();

            filesDiscovered.CounterHelp = "arachnode.net - Files Discovered/s";
            filesDiscovered.CounterName = "arachnode.net - Files Discovered/s";
            filesDiscovered.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(filesDiscovered);

            /**/

            CounterCreationData hyperLinksDiscovered = new CounterCreationData();

            hyperLinksDiscovered.CounterHelp = "arachnode.net - HyperLinks Discovered/s";
            hyperLinksDiscovered.CounterName = "arachnode.net - HyperLinks Discovered/s";
            hyperLinksDiscovered.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(hyperLinksDiscovered);

            /**/

            CounterCreationData imagesDiscovered = new CounterCreationData();

            imagesDiscovered.CounterHelp = "arachnode.net - Images Discovered/s";
            imagesDiscovered.CounterName = "arachnode.net - Images Discovered/s";
            imagesDiscovered.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(imagesDiscovered);

            /**/

            CounterCreationData webPagesDiscovered = new CounterCreationData();

            webPagesDiscovered.CounterHelp = "arachnode.net - WebPages Discovered/s";
            webPagesDiscovered.CounterName = "arachnode.net - WebPages Discovered/s";
            webPagesDiscovered.CounterType = PerformanceCounterType.NumberOfItems32;

            counterCreationDataCollection.Add(webPagesDiscovered);

            /**/

            CounterCreationData totalBytesDiscovered = new CounterCreationData();

            totalBytesDiscovered.CounterHelp = "arachnode.net - Total Bytes Discovered/s";
            totalBytesDiscovered.CounterName = "arachnode.net - Total Bytes Discovered/s";
            totalBytesDiscovered.CounterType = PerformanceCounterType.NumberOfItems64;

            counterCreationDataCollection.Add(totalBytesDiscovered);
        }

        #endregion
    }
}