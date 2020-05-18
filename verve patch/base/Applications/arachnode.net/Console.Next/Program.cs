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
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using Arachnode.DataAccess;
using Arachnode.Plugins.Next;
using Arachnode.SiteCrawler.Next;
using Arachnode.SiteCrawler.Next.DataAccess;
using Arachnode.SiteCrawler.Next.Value;
using Arachnode.SiteCrawler.Next.Value.AbstractClasses;
using Arachnode.SiteCrawler.Next.Value.Enums;
using System.Linq;
using Arachnode.Utilities;

#endregion

namespace Arachnode.Console.Next
{
    public class Program
    {
        //Installation Instructions: http://arachnode.net/Content/InstallationInstructions.aspx

        //console output lock used to ensure correct colors
        //writing to console slows crawling.  remove when comfortable with crawling execution.
        //private const string _initialAbsoluteUri = "http://localhost:56830/Test/1.htm";
        private static bool _hasCrawlCompleted;
        private static readonly object _consoleOutputLock = new object();
        private static ArachnodeDAO _arachnodeDAO;
        private static ArachnodeNextDAO _arachnodeNextDAO;

        private static bool _useSqlServer = false;

        //to generate the test site pages for the 'http://localhost:56830/Test/1.htm', execute '\Web\Test\TestSite.exe'

        private static bool? _generateTheTestSite = false;
        private static bool? _resetDatabase = true;
        private static bool? _resetDirectories = true;
        private static bool? _resetCrawler = true;
        private static bool? _clearIECache = false;
        private static bool? _resetIIS = false;
        private static bool? _startPerfmon = false;
        private static bool? _crawlTheTestSite = false;

        private static Crawler<YourDiscovery, YourCrawlRequest> _crawler;

        [STAThread]
        private static void Main(string[] args)
        {
            //remove limits from service point manager
            ServicePointManager.MaxServicePoints = int.MaxValue;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.MaxServicePointIdleTime = 1000 * 30;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.UseNagleAlgorithm = false;

            try
            {
                #region !RELEASE helper code 1.

#if !RELEASE
                System.Console.WriteLine("arachnode.net AN.Next !RELEASE helper code.");
                System.Console.WriteLine("Always run Visual Studio 2008/2010 as an Administrator.");
#if DEMO
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine("\nDefault values have been supplied for the following prompts.");
                System.Console.WriteLine("Examine the private member variables in Program.cs to customize.  (<Type>?)");
                System.Console.ForegroundColor = ConsoleColor.Gray;
#endif
                System.Console.WriteLine("\nGenerate the TestSite? (y/n)");
                System.Console.WriteLine("  -> http://localhost:56830/Test/1.htm (14606 WebPages)");

                if (_generateTheTestSite.HasValue)
                {
                    System.Console.WriteLine(_generateTheTestSite.Value);
                }
                if ((_generateTheTestSite.HasValue && _generateTheTestSite.Value) || (!_generateTheTestSite.HasValue && System.Console.ReadLine().ToLower() == "y"))
                {
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine("Generating " + 14606 + " WebPages...");
                    System.Console.ForegroundColor = ConsoleColor.Gray;

                    ProcessStartInfo processStartInfo = new ProcessStartInfo("TestSite.exe");
                    processStartInfo.WorkingDirectory = "..\\..\\..\\Web\\Test";

                    Process process = Process.Start(processStartInfo);

                    process.WaitForExit();
                }

                if (_useSqlServer)
                {
                    _arachnodeDAO = new ArachnodeDAO();

                    //these calls assume arachnode.net database usage
                    //AN.Next is storage agnostic, and relies on plugin methods to determine cache presence, crawl procession and persistent storage for crawl requests which do not fit into memory

                    System.Console.WriteLine("\nReset Database:");
                    System.Console.WriteLine("  -> EXEC [dbo].[arachnode_usp_arachnode.net_RESET_DATABASE] (SQL).");
                    System.Console.WriteLine("  -> Resets all user data (SQL).\n");

                    System.Console.WriteLine("Reset Database and perform initial setup tasks?  (y/n)");

                    if (_resetDatabase.HasValue)
                    {
                        System.Console.WriteLine(_resetDatabase.Value);
                    }
                    if ((_resetDatabase.HasValue && _resetDatabase.Value) || (!_resetDatabase.HasValue && System.Console.ReadLine().ToLower() == "y"))
                    {
                        System.Console.ForegroundColor = ConsoleColor.Green;
                        System.Console.WriteLine("Resetting Database...");
#if DEMO
                        System.Console.WriteLine("If this hangs, check Configuration\\ConnectionStrings.config and that MS-SQL Server is installed.");
#endif
                        System.Console.ForegroundColor = ConsoleColor.Gray;

                        //completely resets the database.  (optional)  however, the setup procedures for arachnode.net apply for AN.Next.
                        _arachnodeDAO.ExecuteSql("EXEC [dbo].[arachnode_usp_arachnode.net_RESET_DATABASE]");
                    }

                    //clear the state of the previous crawl
                    _arachnodeDAO.DeleteCrawlRequest(null, null);
                    _arachnodeDAO.DeleteDiscoveries();
                }
                else
                {
                    _arachnodeNextDAO = new ArachnodeNextDAO();

                    //clear the state of the previous crawl
                    _arachnodeNextDAO.DeleteCrawlRequests();
                    _arachnodeNextDAO.DeleteDiscoveries();
                }
#endif
                System.Console.WriteLine();

                _crawler = new Crawler<YourDiscovery, YourCrawlRequest>("CoreConfiguration.xml", File.ReadAllText("FileAndImageAbsoluteUriRegularExpression.txt"), File.ReadAllText("HyperLinkAbsoluteUriRegularExpression.txt"));

                //wire up an event handler to detect application exit and shutdown events.
                System.Console.TreatControlCAsInput = false;
                System.Console.CancelKeyPress += Console_CancelKeyPress;

                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("Press CTRL+C to properly terminate the Console.");
                System.Console.ForegroundColor = ConsoleColor.Gray;

#if !RELEASE
                System.Console.WriteLine("\nReset Crawler:");
                System.Console.WriteLine("  -> Deletes Discoveries in RAM and dbo.Discoveries (SQL).");
                System.Console.WriteLine("  -> Deletes Politenesses in RAM.");
                System.Console.WriteLine("  -> Deletes dbo.CrawlRequests (SQL).");
                System.Console.WriteLine("  -> Deletes dbo.DisallowedAbsoluteUris (SQL).");
                System.Console.WriteLine("  -> Retains all user data (SQL and Filesystem/Disk).\n");

                System.Console.WriteLine("Reset Crawler?  (y/n)");

                if (_resetCrawler.HasValue)
                {
                    System.Console.WriteLine(_resetCrawler.Value);
                }
                if ((_resetCrawler.HasValue && _resetCrawler.Value) || (!_resetCrawler.HasValue && System.Console.ReadLine().ToLower() == "y"))
                {
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine("Resetting Crawler...");
                    System.Console.ForegroundColor = ConsoleColor.Gray;

                    //the Crawler uses arachnode.cache, which uses the ASP.Net cache to determine cache presence before checking persistent storage in the arachnode.net specific plugins
                    _crawler.ClearDistributedCacheInMemoryObjects();

                    if (_useSqlServer)
                    {
                        ////Discoveries keep track of where the crawler has been.  See the database table 'Discoveries' after Stopping a crawl with 'CTRL+C' - rows should be present in 'dbo.Discoveries'.
                        ////No Discoveries will be present in the database table 'dbo.Discoveries' if a crawl completes.
                        ////If you do not clear dbo.Discoveries (or the Discoveries in RAM), then the Crawler will treat a 'new' request for an AbsoluteUri as 'already crawled' if the Discovery (AbsoluteUri) is found in memory, or in the database table 'dbo.Discoveries'.
                        //_crawler.ClearDiscoveries();

                        ////TODO: Domain or Host?
                        ////this method clears the information collected about how frequently the crawler has visited a Domain.
                        ////it isn't necessary to call this before each crawl, but may be useful to call if you wish to reset the politeness counts, while a crawl is paused.
                        //_crawler.ClearPolitenesses();

                        ////if you want to reset a crawl, call this method to remove CrawlRequests that may still be on disk.
                        ////also, call _crawler.ClearDiscoveries()
                        //_crawler.ClearUncrawledCrawlRequests();

                        ////and, check the DisallowedAbsoluteUris table - you may need to clear this to give broken links a chance to resolve, or to allow new AbsoluteUris to be crawled as a result of rule changes...
                        //_crawler.ClearDisallowedAbsoluteUris();
                    }
                    else
                    {
                        //clear the state of the previous crawl
                        _arachnodeNextDAO.DeleteCrawlRequests();
                        _arachnodeNextDAO.DeleteDiscoveries();
                    }
                }

                System.Console.WriteLine("\nClear IE's cache?  (Applicable if using the Renderers and need cachable content (re-)downloaded with eash request.)  (y/n)");

                if (_clearIECache.HasValue)
                {
                    System.Console.WriteLine(_clearIECache.Value);
                }
                if ((_clearIECache.HasValue && _clearIECache.Value) || (!_clearIECache.HasValue && System.Console.ReadLine().ToLower() == "y"))
                {
                    Web.ClearIECache();
                }

                //useful when testing crawler performance against [Web Project]\Test.aspx
                System.Console.WriteLine("\nReset IIS?  (Applicable if running the Web applications under IIS.)  (y/n)");

                if (_resetIIS.HasValue)
                {
                    System.Console.WriteLine(_resetIIS.Value);
                }
                if ((_resetIIS.HasValue && _resetIIS.Value) || (!_resetIIS.HasValue && System.Console.ReadLine().ToLower() == "y"))
                {
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine("Resetting IIS...");
                    System.Console.ForegroundColor = ConsoleColor.Gray;

                    Web.ResetIIS();
                }

                //arachnode.net provides performance counters in three categories.
                // 1.) ArachnodeDAO (how is the Database performing)
                // 2.) Cache (how is the Cache performing)
                // 3.) Crawl (how is the Crawl performing)
                //http://arachnode.net/blogs/arachnode_net/archive/2011/05/05/performance-counters.aspx
                System.Console.WriteLine("\nStart perfmon.exe and monitor arachnode.net's performance?  (y/n)");

                if (_startPerfmon.HasValue)
                {
                    System.Console.WriteLine(_startPerfmon.Value);
                }
                if ((_startPerfmon.HasValue && _startPerfmon.Value) || (!_startPerfmon.HasValue && System.Console.ReadLine().ToLower() == "y"))
                {
                    Process.Start("perfmon.exe");
                }
#endif

                #endregion

                /***********************************/

                /**/

                System.Console.WriteLine("\nTip: Unwiring the status event handlers greatly improves performance.");
                System.Console.WriteLine("  -> Logging to a console is an expensive operation for most applications.");

                //wire the event handlers
                //unwiring the status event handlers GREATLY improves performance
                //(logging to the Console is a relatively expensive operation in code, generally)

                _crawler.CrawlRequestStatus += crawler_CrawlRequestStatus;
                _crawler.FileOrImageAbsoluteUriStatus += crawler_FileAndImageAbsoluteUriStatus;
                _crawler.HyperLinkAbsoluteUriStatus += crawler_HyperLinkAbsoluteUriStatus;
                _crawler.FileCrawled += crawler_FileCrawled;
                _crawler.ImageCrawled += crawler_ImageCrawled;
                _crawler.WebPageCrawled += crawler_WebPageCrawled;

                /**/

                //wire the event handlers
                //unwiring the status event handlers GREATLY improves performance
                //(logging to the Console is a relatively expensive operation in code, generally)

                _crawler.ExceptionCaught += crawler_ExceptionCaught;
                //the CrawlerStatus event handler should remain wired to indicate crawl state...
                _crawler.CrawlerStatus += crawler_CrawlerStatus;

                /**/

                //plugins are called in the order attached

                CrawlerPlugins<YourDiscovery, YourCrawlRequest> crawlerPlugins = new CrawlerPlugins<YourDiscovery, YourCrawlRequest>(_crawler, _useSqlServer);

                _crawler.LoadCrawlRequestsFromPersistentStoragePlugins += crawlerPlugins.LoadCrawlRequestsFromPersistentStorage;
                _crawler.SaveCrawlRequestsToPersistentStoragePlugins += crawlerPlugins.SaveCrawlRequestToPersistentStorage;

                CrawlRequestPlugins<YourDiscovery, YourCrawlRequest> crawlRequestPlugins = new CrawlRequestPlugins<YourDiscovery, YourCrawlRequest>(_crawler, "CrawlRequestPluginConfiguration.xml", _useSqlServer);

                //_crawler.CrawlRequestPlugins += crawlRequestPlugins.ThrottleCrawlingByDomainCrawlDelay;
                //_crawler.CrawlRequestPlugins += crawlRequestPlugins.ProcessRobotsDotText;
                _crawler.CrawlRequestPlugins += crawlRequestPlugins.ModifyHttpWebRequest;
                _crawler.CrawlRequestPlugins += crawlRequestPlugins.ProcessCookies;
                _crawler.CrawlRequestPlugins += crawlRequestPlugins.CheckForUnknownDiscoveryType;
                //_crawler.CrawlRequestPlugins += crawlRequestPlugins.DetectDuplicates;
                _crawler.CrawlRequestPlugins += crawlRequestPlugins.AssignFullTextIndexType;
                _crawler.CrawlRequestPlugins += crawlRequestPlugins.DownloadDataAndDecodeHtml;
                //_crawler.CrawlRequestPlugins += crawlRequestPlugins.DownloadDataRenderAndDecodeHtml;
                _crawler.CrawlRequestPlugins += crawlRequestPlugins.DetectWebPageDownloadedWhereFileOrImageWasExpected;

                FileAndImageDiscoveryPlugins<YourDiscovery, YourCrawlRequest> fileAndImageDiscoveryPlugins = new FileAndImageDiscoveryPlugins<YourDiscovery, YourCrawlRequest>(_crawler, _useSqlServer);

                //_crawler.FileAndImageDiscoveryPlugins += fileAndImageDiscoveryPlugins.DoNotCrawlFilesAndImages;
                _crawler.FileAndImageDiscoveryPlugins += fileAndImageDiscoveryPlugins.ValidateFileAndImageDiscoveries;
                //_crawler.FileAndImageDiscoveryPlugins += fileAndImageDiscoveryPlugins.RestrictFileAndImageDiscoveriesToParentDomain;
                //TIP: use rules to filter FileAndImages before checking persistent storage.  doing so usually improves performance.
                _crawler.FileAndImageDiscoveryPlugins += fileAndImageDiscoveryPlugins.DetermineCachePresence;

                HyperLinkDiscoveryPlugins<YourDiscovery, YourCrawlRequest> hyperLinkDiscoveryPlugins = new HyperLinkDiscoveryPlugins<YourDiscovery, YourCrawlRequest>(_crawler, _useSqlServer);

                //_crawler.HyperLinkDiscoveryPlugins += hyperLinkDiscoveryPlugins.DoNotCrawlHyperLinks;
                _crawler.HyperLinkDiscoveryPlugins += hyperLinkDiscoveryPlugins.ValidateHyperLinkDiscoveries;
                _crawler.HyperLinkDiscoveryPlugins += hyperLinkDiscoveryPlugins.DoNotCrawlNamedAnchors;
                _crawler.HyperLinkDiscoveryPlugins += hyperLinkDiscoveryPlugins.DoNotCrawlQueryStrings;
                _crawler.HyperLinkDiscoveryPlugins += hyperLinkDiscoveryPlugins.RestrictHyperLinkDiscoveriesToParentDomain;
                //TIP: use rules to filter HyperLinks before checking persistent storage.  doing so usually improves performance.
                _crawler.HyperLinkDiscoveryPlugins += hyperLinkDiscoveryPlugins.DetermineCachePresence;
                //_crawler.HyperLinkDiscoveryPlugins += hyperLinkDiscoveryPlugins.SaveHyperLinkDiscoveriesToCache;

                /**/

                //plugins are called in the order attached

                FilePlugins<YourDiscovery, YourCrawlRequest> filePlugins = new FilePlugins<YourDiscovery, YourCrawlRequest>(_crawler, _useSqlServer);

                _crawler.FilePlugins += filePlugins.SaveFileToCache;
                _crawler.FilePlugins += filePlugins.FilePluginOne;
                _crawler.FilePlugins += filePlugins.FilePluginTwo;
                _crawler.FilePlugins += filePlugins.FilePluginThree;

                ImagePlugins<YourDiscovery, YourCrawlRequest> imagePlugins = new ImagePlugins<YourDiscovery, YourCrawlRequest>(_crawler, _useSqlServer);

                _crawler.ImagePlugins += imagePlugins.SaveImageToCache;
                _crawler.ImagePlugins += imagePlugins.ImagePluginOne;
                _crawler.ImagePlugins += imagePlugins.ImagePluginTwo;
                _crawler.ImagePlugins += imagePlugins.ImagePluginThree;

                WebPagePlugins<YourDiscovery, YourCrawlRequest> webPagePlugins = new WebPagePlugins<YourDiscovery, YourCrawlRequest>(_crawler, _useSqlServer);

                _crawler.WebPagePlugins += webPagePlugins.SaveWebPageToCache;
                _crawler.WebPagePlugins += webPagePlugins.WebPagePluginOne;
                _crawler.WebPagePlugins += webPagePlugins.WebPagePluginTwo;
                _crawler.WebPagePlugins += webPagePlugins.WebPagePluginThree;

                /**/

                //add crawl requests

                //A depth of 1 means 'crawl this page only'.  A depth of two means crawl the first page, and follow every HyperLink from the first page.
                //Setting the Depth to int.Max means to crawl the first page, and then int.MaxValue - 1 hops away from the initial CrawlRequest AbsoluteUri.
                //The higher the value for 'Priority', the higher the Priority.
#if !RELEASE
                System.Console.WriteLine("\nCrawl the TestSite? (y/n)");
                System.Console.WriteLine("  -> http://localhost:56830/Test/1.htm (14606 WebPages)");

                bool wasTheCrawlRequestAddedForCrawling;

                if (_crawlTheTestSite.HasValue)
                {
                    System.Console.WriteLine(_crawlTheTestSite.Value);
                }
                if ((_crawlTheTestSite.HasValue && _crawlTheTestSite.Value) || (!_crawlTheTestSite.HasValue && System.Console.ReadLine().ToLower() == "y"))
                {
                    for (int i = 1; i <= 14606; i++)
                    {
                        wasTheCrawlRequestAddedForCrawling = _crawler.Crawl(new YourCrawlRequest(new YourDiscovery(new Uri("http://localhost:56830/Test/" + i + ".htm")), 1, 1), SHA1.Create());
                        //wasTheCrawlRequestAddedForCrawling = _crawler.Crawl(new YourCrawlRequest(new YourDiscovery(new Uri("http://test.arachnode.net/" + i + ".htm")), 1, 1), SHA1.Create());
                    }
                }
                else
                {
                    //READ ME: http://arachnode.net/blogs/arachnode_net/archive/2010/04/29/troubleshooting-crawl-result-differences-between-different-crawl-environments.aspx

                    foreach (string absoluteUri in File.ReadAllLines("CrawlRequests.txt"))
                    {
                        int depth = 1;

                        string absoluteUri2 = absoluteUri.Trim();

                        if (!absoluteUri2.StartsWith("http://"))
                        {
                            wasTheCrawlRequestAddedForCrawling = _crawler.Crawl(new YourCrawlRequest(new YourDiscovery(new Uri("http://" + absoluteUri2)), depth, 1), SHA1.Create());
                        }
                        else
                        {
                            wasTheCrawlRequestAddedForCrawling = _crawler.Crawl(new YourCrawlRequest(new YourDiscovery(new Uri(absoluteUri2)), depth, 1), SHA1.Create());
                        }
                    }
                }
#endif
#if DEMO
                System.Console.ForegroundColor = ConsoleColor.White;

                System.Console.WriteLine("\narachnode.net AN.Next DEMO restrictions:");
                System.Console.WriteLine("1.) Crawl limited to 2 minutes.");
                System.Console.WriteLine("2.) Crawl rate limited by decompilation protection.");
                System.Console.WriteLine("The LICENSED version does not impose these restrictions.");
                System.Console.WriteLine("Press [ENTER] to continue.");

                System.Console.ForegroundColor = ConsoleColor.Gray;

                System.Console.ReadLine();
#endif
                _crawler.Start();
            }
            catch (Exception exception)
            {
                System.Console.WriteLine("\n" + exception.StackTrace + "\n");
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine(exception.Message);
                System.Console.ForegroundColor = ConsoleColor.Gray;
            }

            /**/

            //necessary for the Rendering functionality if you have enabled the Plugin 'DownloadDataRenderAndDecodeHtml'
            //if (_crawler != null)
            //{
            //    //may be null if all configuration settings are not initialized in the database
            //    while (!_hasCrawlCompleted)
            //    {
            //        Application.DoEvents();

            //        Thread.Sleep(100);
            //    }
            //}

            /**/

            System.Console.ReadLine();
        }

        #region Status Events

        private static void crawler_CrawlRequestStatus(ACrawlRequest crawlRequest, DiscoveryStatus discoveryStatus)
        {
            //this method is expensive as it frequently writes to the console but is useful for debugging plugins
            //return;

            lock (_consoleOutputLock)
            {
                switch (discoveryStatus)
                {
                    case DiscoveryStatus.KnownDiscoveryType:
                        System.Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case DiscoveryStatus.UnknownDiscoveryType:
                        System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                        break;
                    case DiscoveryStatus.ProhibitedByPlugin:
                        System.Console.ForegroundColor = ConsoleColor.DarkRed;
                        break;
                }
#if !DEMO
                //these status messages indicate proper content processing but may be confusing for new users
                if (crawlRequest.Information != null)
                {
                    System.Console.WriteLine("#" + crawlRequest.ThreadNumber + ", CRS: " + crawlRequest.Discovery.Uri.AbsoluteUri + " : " + discoveryStatus + " : " + crawlRequest.Information);
                }
                else
                {
                    System.Console.WriteLine("#" + crawlRequest.ThreadNumber + ", CRS: " + crawlRequest.Discovery.Uri.AbsoluteUri + " : " + discoveryStatus);
                }
#endif
                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static void crawler_FileAndImageAbsoluteUriStatus(ACrawlRequest crawlRequest, Uri fileOrImageAbsoluteUri, DiscoveryStatus discoveryStatus)
        {
            //this method is expensive as it frequently writes to the console but is useful for debugging plugins
            return;

            lock (_consoleOutputLock)
            {
                switch (discoveryStatus)
                {
                    case DiscoveryStatus.NotPresentInCache:
                        System.Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case DiscoveryStatus.PresentInCache:
                        System.Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                }

                System.Console.WriteLine("#" + crawlRequest.ThreadNumber + ", FIS: " + fileOrImageAbsoluteUri + " : " + discoveryStatus);

                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static void crawler_HyperLinkAbsoluteUriStatus(ACrawlRequest crawlRequest, Uri hyperLinkAbsoluteUri, DiscoveryStatus discoveryStatus)
        {
            //this method is expensive as it frequently writes to the console but is useful for debugging plugins
            return;

            lock (_consoleOutputLock)
            {
                switch (discoveryStatus)
                {
                    case DiscoveryStatus.NotPresentInCache:
                        System.Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case DiscoveryStatus.PresentInCache:
                        System.Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                }

                System.Console.WriteLine("#" + crawlRequest.ThreadNumber + ", HLS: " + hyperLinkAbsoluteUri + " : " + discoveryStatus);

                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }

        #endregion

        #region File, Image and/or WebPage Crawled Events

        private static void crawler_FileCrawled(ACrawlRequest crawlRequest)
        {
            lock (_consoleOutputLock)
            {
                System.Console.WriteLine("#" + crawlRequest.ThreadNumber + ", FC: Crawled: " + crawlRequest.Discovery.Uri.AbsoluteUri + " Remaining: " + _crawler.CrawlRequestsToCrawl.Count);

                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static void crawler_ImageCrawled(ACrawlRequest crawlRequest)
        {
            lock (_consoleOutputLock)
            {
                System.Console.WriteLine("#" + crawlRequest.ThreadNumber + ", IC: Crawled: " + crawlRequest.Discovery.Uri.AbsoluteUri + " Remaining: " + _crawler.CrawlRequestsToCrawl.Count);

                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private static void crawler_WebPageCrawled(ACrawlRequest crawlRequest)
        {
            lock (_consoleOutputLock)
            {
                System.Console.WriteLine("#" + crawlRequest.ThreadNumber + ", WPC: Crawled: " + crawlRequest.Discovery.Uri.AbsoluteUri + " Remaining: " + _crawler.CrawlRequestsToCrawl.Count);

                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }

        #endregion

        #region Exception Caught Event

        private static void crawler_ExceptionCaught(ACrawlRequest crawlRequest, Exception exception)
        {
            //you may handle specific WebExceptions here, such as crawling too fast and needing to re-submit to the Crawler
            lock (_consoleOutputLock)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;

                if (crawlRequest != null)
                {
                    if (_useSqlServer)
                    {
                        if (crawlRequest.Discovery.Parent != null)
                        {
                            _arachnodeDAO.InsertException(crawlRequest.Discovery.Parent.Uri.AbsoluteUri, crawlRequest.Discovery.Uri.AbsoluteUri, exception.InnerException, false);
                        }
                        else
                        {
                            _arachnodeDAO.InsertException(null, crawlRequest.Discovery.Uri.AbsoluteUri, exception.InnerException, false);
                        }
                    }
                    else
                    {
                        if (crawlRequest.Discovery.Parent != null)
                        {
                            _arachnodeNextDAO.InsertException(crawlRequest.Discovery.Parent.Uri.AbsoluteUri, crawlRequest.Discovery.Uri.AbsoluteUri, exception.InnerException);
                        }
                        else
                        {
                            _arachnodeNextDAO.InsertException(null, crawlRequest.Discovery.Uri.AbsoluteUri, exception.InnerException);
                        }
                    }
#if !DEMO
                    //these status messages indicate proper content processing but may be confusing for new users
                    System.Console.WriteLine("#" + crawlRequest.ThreadNumber + ", EC: Exception, " + crawlRequest.Discovery.Uri.AbsoluteUri + " : " + exception.InnerException.Message);
#endif
                }
                else
                {
                    if (_useSqlServer)
                    {
                        _arachnodeDAO.InsertException(null, null, exception.InnerException, false);
                    }
                    else
                    {
                        _arachnodeNextDAO.InsertException(null, null, exception.InnerException);
                    }

                    System.Console.WriteLine(exception.Message);
                }

                System.Console.ForegroundColor = ConsoleColor.White;
            }
        }

        #endregion

        #region Crawler Status

        private static void crawler_CrawlerStatus(CrawlerState crawlStatus, TimeSpan elapsedTime)
        {
            lock (_consoleOutputLock)
            {
                switch (crawlStatus)
                {
                    case CrawlerState.Started:
                        System.Console.WriteLine("\n" + crawlStatus + " : " + elapsedTime + "\n");
                        break;
                    case CrawlerState.Stopped:
                        System.Console.WriteLine("\n" + crawlStatus + " : " + elapsedTime);
                        System.Console.WriteLine((_crawler.NumberOfFilesCrawled / elapsedTime.TotalSeconds) + " Files/sec.");
                        System.Console.WriteLine((_crawler.NumberOfImagesCrawled / elapsedTime.TotalSeconds) + " Images/sec.");
                        System.Console.WriteLine((_crawler.NumberOfWebPagesCrawled / elapsedTime.TotalSeconds) + " WebPages/sec.");
                        System.Console.WriteLine(((_crawler.NumberOfFilesCrawled + _crawler.NumberOfImagesCrawled + _crawler.NumberOfWebPagesCrawled) / elapsedTime.TotalSeconds) + " Discoveries/sec.");
#if DEMO
                        System.Console.WriteLine("Crawl rate is limited by DEMO protection as well as web server restrictions.\nCrawl the test site for a maximum performance benchmark.");
#endif
                        System.Console.WriteLine("\nPress any key to terminate arachnode.net AN.Next...");
                        break;
                    case CrawlerState.Complete:
                        System.Console.WriteLine("\n" + crawlStatus + " : " + elapsedTime);
                        System.Console.WriteLine((_crawler.NumberOfFilesCrawled / elapsedTime.TotalSeconds) + " Files/sec.");
                        System.Console.WriteLine((_crawler.NumberOfImagesCrawled / elapsedTime.TotalSeconds) + " Images/sec.");
                        System.Console.WriteLine((_crawler.NumberOfWebPagesCrawled / elapsedTime.TotalSeconds) + " WebPages/sec.");
                        System.Console.WriteLine(((_crawler.NumberOfFilesCrawled + _crawler.NumberOfImagesCrawled + _crawler.NumberOfWebPagesCrawled) / elapsedTime.TotalSeconds) + " Discoveries/sec.");
#if DEMO
                        System.Console.WriteLine("Crawl rate is limited by DEMO protection as well as web server restrictions.\nCrawl the test site for a maximum performance benchmark.");
#endif
                        System.Console.WriteLine("\nPress any key to terminate arachnode.net AN.Next...");

                        _crawler.ClearDistributedCacheInMemoryObjects();

                        if (_crawlTheTestSite.HasValue && _crawlTheTestSite.Value)
                        {
                            Debug.Assert(_crawler.NumberOfWebPagesCrawled == 14606);
                        }

                        if (_useSqlServer)
                        {
                            //this call assume arachnode.net database usage
                            //AN.Next is storage agnostic, and relies on plugin methods to determine crawl procession
                            _arachnodeDAO.DeleteDiscoveries();
                        }
                        else
                        {
                            _arachnodeNextDAO.DeleteDiscoveries();
                        }

                        _hasCrawlCompleted = true;
                        break;
                }
            }
        }

        #endregion

        //writing to the console slows crawling.  it is advisable to unwire as many console methods as possible in production systems.

        //these methods are less expensive than the 'Status' event handlers as they write less frequently to the console.  they are useful for debugging crawling and to check you are crawling what you intended to crawl

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;

            _crawler.Stop();
        }
    }
}

