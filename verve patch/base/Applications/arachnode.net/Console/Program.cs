#region License : arachnode.net

// Copyright (c) 2013 http://arachnode.net, arachnode.net, LLC
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Arachnode.Configuration;
using Arachnode.DataAccess;
using Arachnode.DataAccess.Managers;
using Arachnode.Renderer.Value.Enums;
using Arachnode.SiteCrawler;
using Arachnode.SiteCrawler.Core;
using Arachnode.SiteCrawler.Value;
using Arachnode.SiteCrawler.Value.AbstractClasses;
using Arachnode.SiteCrawler.Value.Enums;
using Arachnode.Utilities;
using Arachnode.SiteCrawler.Managers;

#endregion

namespace Arachnode.Console
{
    internal class Program
    {
        //Installation Instructions: http://arachnode.net/Content/InstallationInstructions.aspx

        private static Crawler _crawler;
        private static Stopwatch _stopwatch;
        private static bool _hasCrawlCompleted;

        //to generate the test site pages for the 'http://localhost:56830/Test/1.htm', execute '\Web\Test\TestSite.exe'

        private static bool? _generateTheTestSite = false;
        private static bool? _resetDatabase = true;
        private static bool? _resetDirectories = true;
        private static bool? _resetCrawler = true;
        private static bool? _clearIECache = false;
        private static bool? _resetIIS = false;
        private static bool? _startPerfmon = false;
        private static bool? _crawlTheTestSite = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {            
            try
            {
                //README: Program.cs contains information/code necessary to understand how arachnode.net functions.  Please read this file.
                //README: If you receive the following error, execute the following T-SQL command: "EXEC [dbo].[arachnode_usp_arachnode.net_RESET_DATABASE]".
                //Error	1	CREATE ASSEMBLY for assembly 'Arachnode.Functions' failed because assembly 'Arachnode.Functions' is not authorized for PERMISSION_SET = UNSAFE.  The assembly is authorized when either of the following is true: the database owner (DBO) has UNSAFE ASSEMBLY permission and the database has the TRUSTWORTHY database property on; or the assembly is signed with a certificate or an asymmetric key that has a corresponding login with UNSAFE ASSEMBLY permission.	Functions

                //remove limits from service point manager
                ServicePointManager.MaxServicePoints = int.MaxValue;
                ServicePointManager.DefaultConnectionLimit = int.MaxValue;
                ServicePointManager.CheckCertificateRevocationList = true;
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.MaxServicePointIdleTime = 1000*30;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                ServicePointManager.UseNagleAlgorithm = false;

                _stopwatch = new Stopwatch();

                //HELP: http://arachnode.net/media/p/41.aspx
                //HELP: http://arachnode.net/Content/FrequentlyAskedQuestions.aspx
                //HELP: http://arachnode.net/forums/

                //README: Connection string configuration: Project 'Configuration'\ConnectionStrings.config.

                #region !RELEASE helper code 1.

#if !RELEASE
                System.Console.WriteLine("arachnode.net !RELEASE helper code.");
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

                System.Console.WriteLine("\nReset Database:");
                System.Console.WriteLine("  -> EXEC [dbo].[arachnode_usp_arachnode.net_RESET_DATABASE] (SQL).");
                System.Console.WriteLine("  -> Resets all user data (SQL).");

                System.Console.WriteLine("\nInitial setup tasks:");
                System.Console.WriteLine("  -> Populate settings for missing values in cfg.Configuration (SQL).");
                System.Console.WriteLine("  -> Populate settings for missing values in cfg.CrawlActions (SQL).");

                System.Console.WriteLine("\nReset Database and perform initial setup tasks?  (y/n)");

                //README: This directory will not contain files unless 'OutputConsoleToLogs' is 'true', and 'EnableConsoleOutput' is 'true'.
                string consoleOutputLogsDirectory = Path.Combine(Environment.CurrentDirectory, "ConsoleOutputLogs");

                //README: These directories are necessary if you wish to store downloaded Discoveries on disk, and not in the database.
                string downloadedFilesDirectory = Path.Combine(Environment.CurrentDirectory, "DownloadedFiles");
                string downloadedImagesDirectory = Path.Combine(Environment.CurrentDirectory, "DownloadedImages");
                string downloadedWebPagesDirectory = Path.Combine(Environment.CurrentDirectory, "DownloadedWebPages");
                string luceneDotNetIndexDirectory = Path.Combine(Environment.CurrentDirectory, "LuceneDotNetIndex");

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

                    //reset the database...
                    ArachnodeDAO arachnodeDAO = new ArachnodeDAO();
                    //running with '1' as the input parameter removes all user data.
                    //running this stored procedure alleviates many installation issues.
                    arachnodeDAO.ExecuteSql("EXEC [dbo].[arachnode_usp_arachnode.net_RESET_DATABASE]");

                    //running with '1' as the input parameter resets the database to the intial configuration state and removes all user data.
                    //arachnodeDAO.ExecuteSql("EXEC [dbo].[arachnode_usp_arachnode.net_RESET_DATABASE] 1");

                    //perform common setup tasks.

                    //update the config values for the on-disk storage.
                    arachnodeDAO.ExecuteSql("UPDATE cfg.Configuration SET [Value] = '" + consoleOutputLogsDirectory + "' WHERE [Key] = 'ConsoleOutputLogsDirectory'");
                    arachnodeDAO.ExecuteSql("UPDATE cfg.Configuration SET [Value] = '" + downloadedFilesDirectory + "' WHERE [Key] = 'DownloadedFilesDirectory'");
                    arachnodeDAO.ExecuteSql("UPDATE cfg.Configuration SET [Value] = '" + downloadedImagesDirectory + "' WHERE [Key] = 'DownloadedImagesDirectory'");
                    arachnodeDAO.ExecuteSql("UPDATE cfg.Configuration SET [Value] = '" + downloadedWebPagesDirectory + "' WHERE [Key] = 'DownloadedWebPagesDirectory'");
                    arachnodeDAO.ExecuteSql("UPDATE cfg.Configuration SET [Value] = '" + luceneDotNetIndexDirectory + "' WHERE [Key] = 'LuceneDotNetIndexDirectory'");

                    string settings = "AutoCommit=true|LuceneDotNetIndexDirectory=" + luceneDotNetIndexDirectory + "|CheckIndexes=false|IndexFiles=true|IndexImages=true|IndexWebPages=true|RebuildIndexOnLoad=false|FileIDLowerBound=1|FileIDUpperBound=100000|ImageIDLowerBound=1|ImageIDUpperBound=100000|WebPageIDLowerBound=1|WebPageIDUpperBound=100000";

                    //update the indexing location for the lucene.net functionality.
                    arachnodeDAO.ExecuteSql("UPDATE cfg.CrawlActions SET [Settings] = '" + settings + "' WHERE TypeName = 'Arachnode.Plugins.CrawlActions.ManageLuceneDotNetIndexes'");
                }

                System.Console.WriteLine("\nReset Directories:");
                System.Console.WriteLine("  -> Deletes cfg.Configuration.ConsoleOutputLogsDirectory (Filesystem/Disk).");
                System.Console.WriteLine("  -> Deletes cfg.Configuration.DownloadedFilesDirectory (Filesystem/Disk).");
                System.Console.WriteLine("  -> Deletes cfg.Configuration.DownloadedImagesDirectory (Filesystem/Disk).");
                System.Console.WriteLine("  -> Deletes cfg.Configuration.DownloadedWebPagesDirectory (Filesystem/Disk).");
                System.Console.WriteLine("  -> Deletes cfg.Configuration.LuceneDotNetIndexDirectory (Filesystem/Disk).");
                System.Console.WriteLine("  -> Resets all user data (Filesystem/Disk).\n");

                System.Console.WriteLine("Reset Directories?  (y/n)");

                if (_resetDirectories.HasValue)
                {
                    System.Console.WriteLine(_resetDirectories.Value);
                }
                if ((_resetDirectories.HasValue && _resetDirectories.Value) || (!_resetDirectories.HasValue && System.Console.ReadLine().ToLower() == "y"))
                {
                    try
                    {
                        System.Console.ForegroundColor = ConsoleColor.Green;
                        System.Console.WriteLine("Resetting Directories...");
                        System.Console.ForegroundColor = ConsoleColor.White;

                        System.Console.WriteLine("Resetting " + new DirectoryInfo(consoleOutputLogsDirectory).FullName + "...");
                        try
                        {
                            if (Directory.Exists(consoleOutputLogsDirectory))
                            {
                                Directories.DeleteDirectory(consoleOutputLogsDirectory);
                            }
                        }
                        catch (Exception exception)
                        {
                            System.Console.WriteLine(exception.Message);
                        }

                        System.Console.WriteLine("Resetting " + new DirectoryInfo(downloadedFilesDirectory).FullName + "...");
                        try
                        {
                            if (Directory.Exists(downloadedFilesDirectory))
                            {
                                Directories.DeleteDirectory(downloadedFilesDirectory);
                            }
                        }
                        catch (Exception exception)
                        {
                            System.Console.WriteLine(exception.Message);
                        }

                        System.Console.WriteLine("Resetting " + new DirectoryInfo(downloadedImagesDirectory).FullName + "...");
                        try
                        {
                            if (Directory.Exists(downloadedImagesDirectory))
                            {
                                Directories.DeleteDirectory(downloadedImagesDirectory);
                            }
                        }
                        catch (Exception exception)
                        {
                            System.Console.WriteLine(exception.Message);
                        }

                        System.Console.WriteLine("Resetting " + new DirectoryInfo(downloadedWebPagesDirectory).FullName + "...");
                        try
                        {
                            if (Directory.Exists(downloadedWebPagesDirectory))
                            {
                                Directories.DeleteDirectory(downloadedWebPagesDirectory);
                            }
                        }
                        catch (Exception exception)
                        {
                            System.Console.WriteLine(exception.Message);
                        }

                        System.Console.WriteLine("Resetting " + new DirectoryInfo(luceneDotNetIndexDirectory).FullName + "...");
                        try
                        {
                            if (Directory.Exists(luceneDotNetIndexDirectory))
                            {
                                Directories.DeleteDirectory(luceneDotNetIndexDirectory);
                            }
                        }
                        catch (Exception exception)
                        {
                            System.Console.WriteLine(exception.Message);
                        }

                        System.Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (Exception)
                    {
                    }
                }
#endif
                System.Console.WriteLine();

                #region multi-server caching...

                //List<CrawlerPeer> crawlerPeers = new List<CrawlerPeer>();

                //crawlerPeers.Add(new CrawlerPeer(IPAddress.Parse("127.0.0.1"), 80));
                //crawlerPeers.Add(new CrawlerPeer(IPAddress.Parse("192.168.1.124"), 80));

                //List<DatabasePeer> databasePeers = new List<DatabasePeer>();

                //DatabasePeer databasePeer = new DatabasePeer(ApplicationSettings.ConnectionString);

                //databasePeers.Add(databasePeer);

                //_crawler = new Crawler(CrawlMode.DepthFirstByPriority, crawlerPeers, databasePeers, false);

                /**/

                //custom cookie processing...

                //CookieCollection cookieCollection = CookieManager.BuildCookieCollection("");

                //CookieContainer cookieContainer = new CookieContainer();

                //CookieManager.AddCookieCollectionToCookieContainer("http://facebook.com", cookieContainer, cookieCollection);

                //_crawler = new Crawler(CrawlMode.DepthFirstByPriority, cookieContainer, null, false);

                #endregion

                _crawler = new Crawler(CrawlMode.DepthFirstByPriority, false);

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

                    //Discoveries keep track of where the crawler has been.  See the database table 'Discoveries' after Stopping a crawl with 'CTRL+C' - rows should be present in 'dbo.Discoveries'.
                    //No Discoveries will be present in the database table 'dbo.Discoveries' if a crawl completes.
                    //If you do not clear dbo.Discoveries (or the Discoveries in RAM), then the Crawler will treat a 'new' request for an AbsoluteUri as 'already crawled' if the Discovery (AbsoluteUri) is found in memory, or in the database table 'dbo.Discoveries'.
                    _crawler.ClearDiscoveries();

                    //TODO: Domain or Host?
                    //this method clears the information collected about how frequently the crawler has visited a Domain.
                    //it isn't necessary to call this before each crawl, but may be useful to call if you wish to reset the politeness counts, while a crawl is paused.
                    _crawler.ClearPolitenesses();

                    //if you want to reset a crawl, call this method to remove CrawlRequests that may still be on disk.
                    //also, call _crawler.ClearDiscoveries()
                    _crawler.ClearUncrawledCrawlRequests();

                    //and, check the DisallowedAbsoluteUris table - you may need to clear this to give broken links a chance to resolve, or to allow new AbsoluteUris to be crawled as a result of rule changes...
                    _crawler.ClearDisallowedAbsoluteUris();
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

                if (_crawler.Engine != null)
                {
                    /**/

                    //assign settings before starting a crawl

#if DEBUG
                    AssignApplicationSettingsForDebug();
#endif

#if DEMO
                    AssignApplicationSettingsForDemo();
#endif

#if RELEASE
                    AssignApplicationSettingsForRelease();
#endif
                    //enables all options...
                    //AssignApplicationSettingsForAllApplicationSettingsOn();
                    //enables all options and plugins...
                    //AssignApplicationSettingsForAllApplicationSettingsOnAndAllPluginsEnabled();
                    //use this to store your custom options while preserving the default options from SVN...
                    //AssignApplicationSettingsForUser();

                    ConfigurationManager.CheckForIncorrectConfigurationValues();

                    /**/

                    _crawler.Engine.CrawlRequestCanceled += Engine_CrawlRequestCanceled;
                    _crawler.Engine.CrawlRequestCompleted += Engine_CrawlRequestCompleted;
                    _crawler.Engine.CrawlRequestRetrying += Engine_CrawlRequestRetrying;
                    _crawler.Engine.CrawlRequestThrottled += Engine_CrawlRequestThrottled;
                    _crawler.Engine.CrawlCompleted += Engine_CrawlCompleted;

                    /**/

                    //add crawl requests

                    //A depth of 1 means 'crawl this page only'.  A depth of two means crawl the first page, and follow every HyperLink from the first page.
                    //Setting the Depth to int.Max means to crawl the first page, and then int.MaxValue - 1 hops away from the initial CrawlRequest AbsoluteUri.
                    //The higher the value for 'Priority', the higher the Priority.
#if !RELEASE
                    System.Console.WriteLine("\nCrawl the TestSite? (y/n)");
                    System.Console.WriteLine("  -> http://localhost:56830/Test/1.htm (14606 WebPages)");

                    int depth = 1;
                    UriClassificationType restrictCrawlTo = UriClassificationType.Domain;
                    UriClassificationType restrictDiscoveriesTo = UriClassificationType.None;
                    RenderType renderType = _crawler.AreRenderersEnabled ? RenderType.Render : RenderType.None;
                    RenderType renderTypeForChildren = renderType;

                    bool wasTheCrawlRequestAddedForCrawling;
                    int count = 0;

                    if (_crawlTheTestSite.HasValue)
                    {
                        System.Console.WriteLine(_crawlTheTestSite.Value);
                    }
                    if ((_crawlTheTestSite.HasValue && _crawlTheTestSite.Value) || (!_crawlTheTestSite.HasValue && System.Console.ReadLine().ToLower() == "y"))
                    {
                        for (int i = 1; i <= 14606; i++)
                        {
                            wasTheCrawlRequestAddedForCrawling = _crawler.Crawl(new CrawlRequest(new Discovery("http://localhost:56830/Test/" + i + ".htm"), depth, restrictCrawlTo, restrictDiscoveriesTo, 1, renderType, renderTypeForChildren));
                            //wasTheCrawlRequestAddedForCrawling = _crawler.Crawl(new CrawlRequest(new Discovery("http://test.arachnode.net/" + i + ".htm"), depth, restrictCrawlTo, restrictDiscoveriesTo, 1, renderType, renderTypeForChildren));
#if !DEMO
                            //helpful if you are loading a large list of AbsoluteUris/CrawlRequests and don't want to wait for the full list to load before starting the crawl...
                            if (++count == 1000)
                            {
                                _crawler.Engine.Start();
                            }
#endif
                        }
                    }
                    else
                    {
                        //READ ME: http://arachnode.net/blogs/arachnode_net/archive/2010/04/29/troubleshooting-crawl-result-differences-between-different-crawl-environments.aspx

                        foreach (string absoluteUri in File.ReadAllLines("CrawlRequests.txt"))
                        {
                            string absoluteUri2 = absoluteUri.Trim();

                            if (!absoluteUri2.StartsWith("http://"))
                            {
                                wasTheCrawlRequestAddedForCrawling = _crawler.Crawl(new CrawlRequest(new Discovery("http://" + absoluteUri2), depth, restrictCrawlTo, restrictDiscoveriesTo, 1, renderType, renderTypeForChildren));
                            }
                            else
                            {
                                wasTheCrawlRequestAddedForCrawling = _crawler.Crawl(new CrawlRequest(new Discovery(absoluteUri2), depth, restrictCrawlTo, restrictDiscoveriesTo, 1, renderType, renderTypeForChildren));
                            }
#if !DEMO
                            //helpful if you are loading a large list of AbsoluteUris/CrawlRequests and don't want to wait for the full list to load before starting the crawl...
                            if (++count == 1000)
                            {
                                _crawler.Engine.Start();
                            }
#endif
                        }
                    }
#endif
                    /**/

                    //If you stop a Crawl all CrawlRequests and Discoveries will be saved to the CrawlRequests and Discoveries tables.  These tables are used to maintain Crawl state and should not be modified by the user.

                    //If you are questioning why an AbsoluteUri wasn't crawled, check the database tables 'DisallowedAbsoluteUris' and 'Exceptions'.
#if DEMO
                    ConsoleManager.OutputString("\narachnode.net DEMO restrictions:", ConsoleColor.White, ConsoleColor.Gray);
                    ConsoleManager.OutputString("1.) Crawl limited to 2 minutes.", ConsoleColor.White, ConsoleColor.Gray);
                    ConsoleManager.OutputString("2.) Crawl rate limited by decompilation protection.", ConsoleColor.White, ConsoleColor.Gray);
                    ConsoleManager.OutputString("The LICENSED version does not impose these restrictions.", ConsoleColor.White, ConsoleColor.Gray);
                    ConsoleManager.OutputString("Press [ENTER] to continue.", ConsoleColor.White, ConsoleColor.Gray);

                    System.Console.ReadLine();
#endif
                    _stopwatch.Start();
                    System.Console.WriteLine("\nElapsed: " + _stopwatch.Elapsed + "\n");

                    //add all CrawlRequests before starting the Engine...
                    _crawler.Engine.Start();
                }
            }
            catch (Exception exception)
            {
                System.Console.WriteLine("\n" + exception.StackTrace + "\n");
                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine(exception.Message);
                System.Console.ForegroundColor = ConsoleColor.Gray;
            }

            /**/

            //necessary for the Rendering functionality.
            if (_crawler != null)
            {
                //may be null if all configuration settings are not initialized in the database...
                while (!_hasCrawlCompleted && _crawler.AreRenderersEnabled)
                {
                    Application.DoEvents();

                    Thread.Sleep(100);
                }
            }

            System.Console.ReadLine();

            if (_crawler != null && _crawler.Engine != null)
            {
                _crawler.Engine.Stop();
            }

            //if you would like to view Files and Images when running the Web project, see here: http://arachnode.net/forums/p/1027/12031.aspx
        }

        private static void AssignApplicationSettingsForDebug()
        {
            //CrawlActions, CrawlRules and EngineActions can be added and set from code, augmenting and/or overriding Database settings.

            //cfg.CrawlActions
            foreach (ACrawlAction crawlAction in _crawler.CrawlActions.Values)
            {
                if (crawlAction.TypeName != "Arachnode.Plugins.CrawlActions.ManageLuceneDotNetIndexes")
                {
                    crawlAction.IsEnabled = false;
                }
            }

            //cfg.CrawlRules
            //CrawlRules limit where the Crawl can go, where not limited by 'UriClassificationType' in the CrawlRequest constructor.
            foreach (ACrawlRule crawlRule in _crawler.CrawlRules.Values)
            {
                crawlRule.IsEnabled = false;
            }

            //cfg.EngineActions
            //EngineActions are invoked when CrawlRequests are retrived from the database.  Most users wil not need these.
            foreach (AEngineAction engineAction in _crawler.Engine.EngineActions.Values)
            {
                engineAction.IsEnabled = false;
            }

            //ApplicationSettings can be set from code, overriding Database settings found in cfg.Configuration.
            ApplicationSettings.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            ApplicationSettings.AcceptEncoding = "gzip,deflate,sdch";
            ApplicationSettings.AllowAutoRedirect = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForFiles = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForHyperLinks = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForImages = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForWebPages = true;
            ApplicationSettings.AssignEmailAddressDiscoveries = false;
            ApplicationSettings.AssignFileAndImageDiscoveries = true;
            ApplicationSettings.AssignHyperLinkDiscoveries = true;
            ApplicationSettings.AutoThrottleHttpWebRequests = false;
            ApplicationSettings.ClassifyAbsoluteUris = false;
            //ApplicationSettings.ConnectionString = "";
            //ApplicationSettings.ConsoleOutputLogsDirectory = "";
            ApplicationSettings.CrawlRequestTimeoutInMinutes = 1;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseCrawlRequests = true;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseFiles = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseHyperLinks = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseImages = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseWebPages = false;
            ApplicationSettings.DesiredMaximumMemoryUsageInMegabytes = 4096;
            ApplicationSettings.DiscoverySlidingExpirationInSeconds = 120;
            //ApplicationSettings.DownloadedFilesDirectory = "";
            //ApplicationSettings.DownloadedImagesDirectory = "";
            //ApplicationSettings.DownloadedWebPagesDirectory = "";
            ApplicationSettings.EnableConsoleOutput = true;
            ApplicationSettings.ExtractFileMetaData = false;
            ApplicationSettings.ExtractImageMetaData = false;
            ApplicationSettings.ExtractWebPageMetaData = false;
            ApplicationSettings.HttpWebRequestRetries = 5;
            ApplicationSettings.InsertCrawlRequests = true;
            ApplicationSettings.InsertDisallowedAbsoluteUriDiscoveries = false;
            ApplicationSettings.InsertDisallowedAbsoluteUris = true;
            ApplicationSettings.InsertDisallowedDiscoveries = false;
            ApplicationSettings.InsertDiscoveries = true;
            ApplicationSettings.InsertEmailAddressDiscoveries = false;
            ApplicationSettings.InsertEmailAddresses = false;
            ApplicationSettings.InsertExceptions = true;
            ApplicationSettings.InsertFileDiscoveries = true;
            ApplicationSettings.InsertFileMetaData = false;
            ApplicationSettings.InsertFiles = true;
            ApplicationSettings.InsertFileSource = false;
            ApplicationSettings.InsertHyperLinkDiscoveries = false;
            ApplicationSettings.InsertHyperLinks = false;
            ApplicationSettings.InsertImageDiscoveries = true;
            ApplicationSettings.InsertImageMetaData = false;
            ApplicationSettings.InsertImages = true;
            ApplicationSettings.InsertImageSource = false;
            ApplicationSettings.InsertWebPageMetaData = false;
            ApplicationSettings.InsertWebPages = true;
            ApplicationSettings.InsertWebPageSource = false;
            ApplicationSettings.MaximumNumberOfAutoRedirects = 50;
            ApplicationSettings.MaximumNumberOfCrawlRequestsToCreatePerBatch = 1000;
            ApplicationSettings.MaximumNumberOfCrawlThreads = 10;
            ApplicationSettings.MaximumNumberOfHostsAndPrioritiesToSelect = 10000;
            ApplicationSettings.OutputConsoleToLogs = false;
            ApplicationSettings.OutputStatistics = false;
            ApplicationSettings.OutputWebExceptions = true;
            ApplicationSettings.ProcessDiscoveriesAsynchronously = true;
            ApplicationSettings.SaveDiscoveredFilesToDisk = true;
            ApplicationSettings.SaveDiscoveredImagesToDisk = true;
            ApplicationSettings.SaveDiscoveredWebPagesToDisk = true;
            ApplicationSettings.SetRefererToParentAbsoluteUri = true;
            ApplicationSettings.SqlCommandTimeoutInMinutes = 60;
            ApplicationSettings.UniqueIdentifier = "";
            ApplicationSettings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.71 Safari/534.24"; //If you find yourself blocked from crawling a website, change this to a common crawler string, such as 'Googlebot' or 'Slurp'...
            ApplicationSettings.VerboseOutput = false;
            //enable VerboseOutput to see each Discovery and the the status of each Discovery returned from each Discovery.  (e.g. WebPages from each WebPage and Files/Images from each WebPage.)
        }

        private static void AssignApplicationSettingsForDemo()
        {
            //CrawlActions, CrawlRules and EngineActions can be added and set from code, augmenting and/or overriding Database settings.

            //cfg.CrawlActions
            //for demo purposes, all CrawlActions are disabled except for 'Arachnode.Plugins.CrawlActions.ManageLuceneDotNetIndexes'.
            foreach (ACrawlAction crawlAction in _crawler.CrawlActions.Values)
            {
                if (crawlAction.TypeName != "Arachnode.Plugins.CrawlActions.ManageLuceneDotNetIndexes")
                {
                    crawlAction.IsEnabled = false;
                }
            }

            //cfg.CrawlRules
            //for demo purposes, all CrawlRules are disabled.
            //CrawlRules limit where the Crawl can go, where not limited by 'UriClassificationType' in the CrawlRequest constructor.
            foreach (ACrawlRule crawlRule in _crawler.CrawlRules.Values)
            {
                crawlRule.IsEnabled = false;
            }

            //cfg.EngineActions
            //for demo purposes, all EngineActions are disabled.
            //EngineActions are invoked when CrawlRequests are retrived from the database.  Most users wil not need these.
            foreach (AEngineAction engineAction in _crawler.Engine.EngineActions.Values)
            {
                engineAction.IsEnabled = false;
            }

            //ApplicationSettings can be set from code, overriding Database settings found in cfg.Configuration.
            ApplicationSettings.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            ApplicationSettings.AcceptEncoding = "gzip,deflate,sdch";
            ApplicationSettings.AllowAutoRedirect = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForFiles = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForHyperLinks = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForImages = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForWebPages = true;
            ApplicationSettings.AssignEmailAddressDiscoveries = false;
            ApplicationSettings.AssignFileAndImageDiscoveries = true;
            ApplicationSettings.AssignHyperLinkDiscoveries = true;
            ApplicationSettings.AutoThrottleHttpWebRequests = false;
            ApplicationSettings.ClassifyAbsoluteUris = false;
            //ApplicationSettings.ConnectionString = "";
            //ApplicationSettings.ConsoleOutputLogsDirectory = "";
            ApplicationSettings.CrawlRequestTimeoutInMinutes = 1;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseCrawlRequests = true;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseFiles = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseHyperLinks = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseImages = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseWebPages = false;
            ApplicationSettings.DesiredMaximumMemoryUsageInMegabytes = 4096;
            ApplicationSettings.DiscoverySlidingExpirationInSeconds = 120;
            //ApplicationSettings.DownloadedFilesDirectory = "";
            //ApplicationSettings.DownloadedImagesDirectory = "";
            //ApplicationSettings.DownloadedWebPagesDirectory = "";
            ApplicationSettings.EnableConsoleOutput = true;
            ApplicationSettings.ExtractFileMetaData = false;
            ApplicationSettings.ExtractImageMetaData = false;
            ApplicationSettings.ExtractWebPageMetaData = false;
            ApplicationSettings.HttpWebRequestRetries = 5;
            ApplicationSettings.InsertCrawlRequests = true;
            ApplicationSettings.InsertDisallowedAbsoluteUriDiscoveries = false;
            ApplicationSettings.InsertDisallowedAbsoluteUris = true;
            ApplicationSettings.InsertDisallowedDiscoveries = false;
            ApplicationSettings.InsertDiscoveries = true;
            ApplicationSettings.InsertEmailAddressDiscoveries = false;
            ApplicationSettings.InsertEmailAddresses = false;
            ApplicationSettings.InsertExceptions = true;
            ApplicationSettings.InsertFileDiscoveries = true;
            ApplicationSettings.InsertFileMetaData = false;
            ApplicationSettings.InsertFiles = true;
            ApplicationSettings.InsertFileSource = false;
            ApplicationSettings.InsertHyperLinkDiscoveries = false;
            ApplicationSettings.InsertHyperLinks = false;
            ApplicationSettings.InsertImageDiscoveries = true;
            ApplicationSettings.InsertImageMetaData = false;
            ApplicationSettings.InsertImages = true;
            ApplicationSettings.InsertImageSource = false;
            ApplicationSettings.InsertWebPageMetaData = false;
            ApplicationSettings.InsertWebPages = true;
            ApplicationSettings.InsertWebPageSource = false;
            ApplicationSettings.MaximumNumberOfAutoRedirects = 50;
            ApplicationSettings.MaximumNumberOfCrawlRequestsToCreatePerBatch = 1000;
            ApplicationSettings.MaximumNumberOfCrawlThreads = 10;
            ApplicationSettings.MaximumNumberOfHostsAndPrioritiesToSelect = 10000;
            ApplicationSettings.OutputConsoleToLogs = false;
            ApplicationSettings.OutputStatistics = false;
            ApplicationSettings.OutputWebExceptions = true;
            ApplicationSettings.ProcessDiscoveriesAsynchronously = true;
            ApplicationSettings.SaveDiscoveredFilesToDisk = true;
            ApplicationSettings.SaveDiscoveredImagesToDisk = true;
            ApplicationSettings.SaveDiscoveredWebPagesToDisk = true;
            ApplicationSettings.SetRefererToParentAbsoluteUri = true;
            ApplicationSettings.SqlCommandTimeoutInMinutes = 60;
            ApplicationSettings.UniqueIdentifier = "";
            ApplicationSettings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.71 Safari/534.24"; //If you find yourself blocked from crawling a website, change this to a common crawler string, such as 'Googlebot' or 'Slurp'...
            ApplicationSettings.VerboseOutput = false;
            //enable VerboseOutput to see each Discovery and the the status of each Discovery returned from each Discovery.  (e.g. WebPages from each WebPage and Files/Images from each WebPage.)
        }

        private static void AssignApplicationSettingsForRelease()
        {
            //CrawlActions, CrawlRules and EngineActions can be added and set from code, augmenting and/or overriding Database settings.

            //cfg.CrawlActions
            foreach (ACrawlAction crawlAction in _crawler.CrawlActions.Values)
            {
                if (crawlAction.TypeName != "Arachnode.Plugins.CrawlActions.ManageLuceneDotNetIndexes")
                {
                    crawlAction.IsEnabled = false;
                }
            }

            //cfg.CrawlRules
            //CrawlRules limit where the Crawl can go, where not limited by 'UriClassificationType' in the CrawlRequest constructor.
            foreach (ACrawlRule crawlRule in _crawler.CrawlRules.Values)
            {
                crawlRule.IsEnabled = false;
            }

            //cfg.EngineActions
            //EngineActions are invoked when CrawlRequests are retrived from the database.  Most users wil not need these.
            foreach (AEngineAction engineAction in _crawler.Engine.EngineActions.Values)
            {
                engineAction.IsEnabled = false;
            }

            //ApplicationSettings can be set from code, overriding Database settings found in cfg.Configuration.
            ApplicationSettings.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            ApplicationSettings.AcceptEncoding = "gzip,deflate,sdch";
            ApplicationSettings.AllowAutoRedirect = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForFiles = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForHyperLinks = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForImages = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForWebPages = true;
            ApplicationSettings.AssignEmailAddressDiscoveries = false;
            ApplicationSettings.AssignFileAndImageDiscoveries = true;
            ApplicationSettings.AssignHyperLinkDiscoveries = true;
            ApplicationSettings.AutoThrottleHttpWebRequests = false;
            ApplicationSettings.ClassifyAbsoluteUris = false;
            //ApplicationSettings.ConnectionString = "";
            //ApplicationSettings.ConsoleOutputLogsDirectory = "";
            ApplicationSettings.CrawlRequestTimeoutInMinutes = 1;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseCrawlRequests = true;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseFiles = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseHyperLinks = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseImages = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseWebPages = false;
            ApplicationSettings.DesiredMaximumMemoryUsageInMegabytes = 4096;
            ApplicationSettings.DiscoverySlidingExpirationInSeconds = 120;
            //ApplicationSettings.DownloadedFilesDirectory = "";
            //ApplicationSettings.DownloadedImagesDirectory = "";
            //ApplicationSettings.DownloadedWebPagesDirectory = "";
            ApplicationSettings.EnableConsoleOutput = false;
            ApplicationSettings.ExtractFileMetaData = false;
            ApplicationSettings.ExtractImageMetaData = false;
            ApplicationSettings.ExtractWebPageMetaData = false;
            ApplicationSettings.HttpWebRequestRetries = 5;
            ApplicationSettings.InsertCrawlRequests = true;
            ApplicationSettings.InsertDisallowedAbsoluteUriDiscoveries = false;
            ApplicationSettings.InsertDisallowedAbsoluteUris = true;
            ApplicationSettings.InsertDisallowedDiscoveries = false;
            ApplicationSettings.InsertDiscoveries = true;
            ApplicationSettings.InsertEmailAddressDiscoveries = false;
            ApplicationSettings.InsertEmailAddresses = false;
            ApplicationSettings.InsertExceptions = true;
            ApplicationSettings.InsertFileDiscoveries = true;
            ApplicationSettings.InsertFileMetaData = false;
            ApplicationSettings.InsertFiles = true;
            ApplicationSettings.InsertFileSource = false;
            ApplicationSettings.InsertHyperLinkDiscoveries = false;
            ApplicationSettings.InsertHyperLinks = false;
            ApplicationSettings.InsertImageDiscoveries = true;
            ApplicationSettings.InsertImageMetaData = false;
            ApplicationSettings.InsertImages = true;
            ApplicationSettings.InsertImageSource = false;
            ApplicationSettings.InsertWebPageMetaData = false;
            ApplicationSettings.InsertWebPages = true;
            ApplicationSettings.InsertWebPageSource = false;
            ApplicationSettings.MaximumNumberOfAutoRedirects = 50;
            ApplicationSettings.MaximumNumberOfCrawlRequestsToCreatePerBatch = 1000;
            ApplicationSettings.MaximumNumberOfCrawlThreads = 10;
            ApplicationSettings.MaximumNumberOfHostsAndPrioritiesToSelect = 10000;
            ApplicationSettings.OutputConsoleToLogs = false;
            ApplicationSettings.OutputStatistics = false;
            ApplicationSettings.OutputWebExceptions = true;
            ApplicationSettings.ProcessDiscoveriesAsynchronously = true;
            ApplicationSettings.SaveDiscoveredFilesToDisk = true;
            ApplicationSettings.SaveDiscoveredImagesToDisk = true;
            ApplicationSettings.SaveDiscoveredWebPagesToDisk = true;
            ApplicationSettings.SetRefererToParentAbsoluteUri = true;
            ApplicationSettings.SqlCommandTimeoutInMinutes = 60;
            ApplicationSettings.UniqueIdentifier = "";
            ApplicationSettings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.71 Safari/534.24"; //If you find yourself blocked from crawling a website, change this to a common crawler string, such as 'Googlebot' or 'Slurp'...
            ApplicationSettings.VerboseOutput = false;
            //enable VerboseOutput to see each Discovery and the the status of each Discovery returned from each Discovery.  (e.g. WebPages from each WebPage and Files/Images from each WebPage.)
        }

        private static void AssignApplicationSettingsForAllApplicationSettingsOn()
        {
            //CrawlActions, CrawlRules and EngineActions can be added and set from code, augmenting and/or overriding Database settings.

            //cfg.CrawlActions
            foreach (ACrawlAction crawlAction in _crawler.CrawlActions.Values)
            {
                if (crawlAction.TypeName != "Arachnode.Plugins.CrawlActions.ManageLuceneDotNetIndexes")
                {
                    crawlAction.IsEnabled = false;
                }
            }

            //cfg.CrawlRules
            //CrawlRules limit where the Crawl can go, where not limited by 'UriClassificationType' in the CrawlRequest constructor.
            foreach (ACrawlRule crawlRule in _crawler.CrawlRules.Values)
            {
                crawlRule.IsEnabled = false;
            }

            //cfg.EngineActions
            //EngineActions are invoked when CrawlRequests are retrived from the database.  Most users wil not need these.
            foreach (AEngineAction engineAction in _crawler.Engine.EngineActions.Values)
            {
                engineAction.IsEnabled = false;
            }

            //ApplicationSettings can be set from code, overriding Database settings found in cfg.Configuration.
            ApplicationSettings.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            ApplicationSettings.AcceptEncoding = "gzip,deflate,sdch";
            ApplicationSettings.AllowAutoRedirect = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForFiles = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForHyperLinks = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForImages = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForWebPages = true;
            ApplicationSettings.AssignEmailAddressDiscoveries = true;
            ApplicationSettings.AssignFileAndImageDiscoveries = true;
            ApplicationSettings.AssignHyperLinkDiscoveries = true;
            ApplicationSettings.AutoThrottleHttpWebRequests = true;
            ApplicationSettings.ClassifyAbsoluteUris = true;
            //ApplicationSettings.ConnectionString = "";
            //ApplicationSettings.ConsoleOutputLogsDirectory = "";
            ApplicationSettings.CrawlRequestTimeoutInMinutes = 1;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseCrawlRequests = true;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseFiles = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseHyperLinks = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseImages = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseWebPages = false;
            ApplicationSettings.DesiredMaximumMemoryUsageInMegabytes = 4096;
            ApplicationSettings.DiscoverySlidingExpirationInSeconds = 120;
            //ApplicationSettings.DownloadedFilesDirectory = "";
            //ApplicationSettings.DownloadedImagesDirectory = "";
            //ApplicationSettings.DownloadedWebPagesDirectory = "";
            ApplicationSettings.EnableConsoleOutput = true;
            ApplicationSettings.ExtractFileMetaData = true;
            ApplicationSettings.ExtractImageMetaData = true;
            ApplicationSettings.ExtractWebPageMetaData = true;
            ApplicationSettings.HttpWebRequestRetries = 5;
            ApplicationSettings.InsertCrawlRequests = true;
            ApplicationSettings.InsertDisallowedAbsoluteUriDiscoveries = true;
            ApplicationSettings.InsertDisallowedAbsoluteUris = true;
            ApplicationSettings.InsertDisallowedDiscoveries = true;
            ApplicationSettings.InsertDiscoveries = true;
            ApplicationSettings.InsertEmailAddressDiscoveries = true;
            ApplicationSettings.InsertEmailAddresses = true;
            ApplicationSettings.InsertExceptions = true;
            ApplicationSettings.InsertFileDiscoveries = true;
            ApplicationSettings.InsertFileMetaData = true;
            ApplicationSettings.InsertFiles = true;
            ApplicationSettings.InsertFileSource = true;
            ApplicationSettings.InsertHyperLinkDiscoveries = true;
            ApplicationSettings.InsertHyperLinks = true;
            ApplicationSettings.InsertImageDiscoveries = true;
            ApplicationSettings.InsertImageMetaData = true;
            ApplicationSettings.InsertImages = true;
            ApplicationSettings.InsertImageSource = false;
            ApplicationSettings.InsertWebPageMetaData = true;
            ApplicationSettings.InsertWebPages = true;
            ApplicationSettings.InsertWebPageSource = true;
            ApplicationSettings.MaximumNumberOfAutoRedirects = 50;
            ApplicationSettings.MaximumNumberOfCrawlRequestsToCreatePerBatch = 1000;
            ApplicationSettings.MaximumNumberOfCrawlThreads = 10;
            ApplicationSettings.MaximumNumberOfHostsAndPrioritiesToSelect = 10000;
            ApplicationSettings.OutputConsoleToLogs = false;
            ApplicationSettings.OutputStatistics = false;
            ApplicationSettings.OutputWebExceptions = true;
            ApplicationSettings.ProcessDiscoveriesAsynchronously = true;
            ApplicationSettings.SaveDiscoveredFilesToDisk = true;
            ApplicationSettings.SaveDiscoveredImagesToDisk = true;
            ApplicationSettings.SaveDiscoveredWebPagesToDisk = true;
            ApplicationSettings.SetRefererToParentAbsoluteUri = true;
            ApplicationSettings.SqlCommandTimeoutInMinutes = 60;
            ApplicationSettings.UniqueIdentifier = "";
            ApplicationSettings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.71 Safari/534.24"; //If you find yourself blocked from crawling a website, change this to a common crawler string, such as 'Googlebot' or 'Slurp'...
            ApplicationSettings.VerboseOutput = false;
            //enable VerboseOutput to see each Discovery and the the status of each Discovery returned from each Discovery.  (e.g. WebPages from each WebPage and Files/Images from each WebPage.)
        }

        private static void AssignApplicationSettingsForAllApplicationSettingsOnAndAllPluginsEnabled()
        {
            //CrawlActions, CrawlRules and EngineActions can be added and set from code, augmenting and/or overriding Database settings.

            //cfg.CrawlActions
            foreach (ACrawlAction crawlAction in _crawler.CrawlActions.Values)
            {
                crawlAction.IsEnabled = false;
            }

            //cfg.CrawlRules
            //CrawlRules limit where the Crawl can go, where not limited by 'UriClassificationType' in the CrawlRequest constructor.
            foreach (ACrawlRule crawlRule in _crawler.CrawlRules.Values)
            {
                crawlRule.IsEnabled = false;
            }

            //cfg.EngineActions
            //EngineActions are invoked when CrawlRequests are retrived from the database.  Most users wil not need these.
            foreach (AEngineAction engineAction in _crawler.Engine.EngineActions.Values)
            {
                engineAction.IsEnabled = false;
            }

            //ApplicationSettings can be set from code, overriding Database settings found in cfg.Configuration.
            ApplicationSettings.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            ApplicationSettings.AcceptEncoding = "gzip,deflate,sdch";
            ApplicationSettings.AllowAutoRedirect = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForFiles = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForHyperLinks = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForImages = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForWebPages = true;
            ApplicationSettings.AssignEmailAddressDiscoveries = true;
            ApplicationSettings.AssignFileAndImageDiscoveries = true;
            ApplicationSettings.AssignHyperLinkDiscoveries = true;
            ApplicationSettings.AutoThrottleHttpWebRequests = true;
            ApplicationSettings.ClassifyAbsoluteUris = true;
            //ApplicationSettings.ConnectionString = "";
            //ApplicationSettings.ConsoleOutputLogsDirectory = "";
            ApplicationSettings.CrawlRequestTimeoutInMinutes = 1;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseCrawlRequests = true;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseFiles = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseHyperLinks = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseImages = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseWebPages = false;
            ApplicationSettings.DesiredMaximumMemoryUsageInMegabytes = 4096;
            ApplicationSettings.DiscoverySlidingExpirationInSeconds = 120;
            //ApplicationSettings.DownloadedFilesDirectory = "";
            //ApplicationSettings.DownloadedImagesDirectory = "";
            //ApplicationSettings.DownloadedWebPagesDirectory = "";
            ApplicationSettings.EnableConsoleOutput = true;
            ApplicationSettings.ExtractFileMetaData = true;
            ApplicationSettings.ExtractImageMetaData = true;
            ApplicationSettings.ExtractWebPageMetaData = true;
            ApplicationSettings.HttpWebRequestRetries = 5;
            ApplicationSettings.InsertCrawlRequests = true;
            ApplicationSettings.InsertDisallowedAbsoluteUriDiscoveries = true;
            ApplicationSettings.InsertDisallowedAbsoluteUris = true;
            ApplicationSettings.InsertDisallowedDiscoveries = true;
            ApplicationSettings.InsertDiscoveries = true;
            ApplicationSettings.InsertEmailAddressDiscoveries = true;
            ApplicationSettings.InsertEmailAddresses = true;
            ApplicationSettings.InsertExceptions = true;
            ApplicationSettings.InsertFileDiscoveries = true;
            ApplicationSettings.InsertFileMetaData = true;
            ApplicationSettings.InsertFiles = true;
            ApplicationSettings.InsertFileSource = true;
            ApplicationSettings.InsertHyperLinkDiscoveries = true;
            ApplicationSettings.InsertHyperLinks = true;
            ApplicationSettings.InsertImageDiscoveries = true;
            ApplicationSettings.InsertImageMetaData = true;
            ApplicationSettings.InsertImages = true;
            ApplicationSettings.InsertImageSource = false;
            ApplicationSettings.InsertWebPageMetaData = true;
            ApplicationSettings.InsertWebPages = true;
            ApplicationSettings.InsertWebPageSource = true;
            ApplicationSettings.MaximumNumberOfAutoRedirects = 50;
            ApplicationSettings.MaximumNumberOfCrawlRequestsToCreatePerBatch = 1000;
            ApplicationSettings.MaximumNumberOfCrawlThreads = 10;
            ApplicationSettings.MaximumNumberOfHostsAndPrioritiesToSelect = 10000;
            ApplicationSettings.OutputConsoleToLogs = false;
            ApplicationSettings.OutputStatistics = false;
            ApplicationSettings.OutputWebExceptions = true;
            ApplicationSettings.ProcessDiscoveriesAsynchronously = true;
            ApplicationSettings.SaveDiscoveredFilesToDisk = true;
            ApplicationSettings.SaveDiscoveredImagesToDisk = true;
            ApplicationSettings.SaveDiscoveredWebPagesToDisk = true;
            ApplicationSettings.SetRefererToParentAbsoluteUri = true;
            ApplicationSettings.SqlCommandTimeoutInMinutes = 60;
            ApplicationSettings.UniqueIdentifier = "";
            ApplicationSettings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.71 Safari/534.24"; //If you find yourself blocked from crawling a website, change this to a common crawler string, such as 'Googlebot' or 'Slurp'...
            ApplicationSettings.VerboseOutput = false;
            //enable VerboseOutput to see each Discovery and the the status of each Discovery returned from each Discovery.  (e.g. WebPages from each WebPage and Files/Images from each WebPage.)
        }

        private static void AssignApplicationSettingsMatchingANDotNext()
        {
            //CrawlActions, CrawlRules and EngineActions can be added and set from code, augmenting and/or overriding Database settings.

            //cfg.CrawlActions
            //for demo purposes, all CrawlActions are disabled except for 'Arachnode.Plugins.CrawlActions.ManageLuceneDotNetIndexes'.
            foreach (ACrawlAction crawlAction in _crawler.CrawlActions.Values)
            {
                crawlAction.IsEnabled = false;
            }

            //cfg.CrawlRules
            //for demo purposes, all CrawlRules are disabled.
            //CrawlRules limit where the Crawl can go, where not limited by 'UriClassificationType' in the CrawlRequest constructor.
            foreach (ACrawlRule crawlRule in _crawler.CrawlRules.Values)
            {
                crawlRule.IsEnabled = false;
            }

            //cfg.EngineActions
            //for demo purposes, all EngineActions are disabled.
            //EngineActions are invoked when CrawlRequests are retrived from the database.  Most users wil not need these.
            foreach (AEngineAction engineAction in _crawler.Engine.EngineActions.Values)
            {
                engineAction.IsEnabled = false;
            }

            //ApplicationSettings can be set from code, overriding Database settings found in cfg.Configuration.
            ApplicationSettings.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            ApplicationSettings.AcceptEncoding = "gzip,deflate,sdch";
            ApplicationSettings.AllowAutoRedirect = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForFiles = false;
            ApplicationSettings.AssignCrawlRequestPrioritiesForHyperLinks = false;
            ApplicationSettings.AssignCrawlRequestPrioritiesForImages = false;
            ApplicationSettings.AssignCrawlRequestPrioritiesForWebPages = false;
            ApplicationSettings.AssignEmailAddressDiscoveries = false;
            ApplicationSettings.AssignFileAndImageDiscoveries = true;
            ApplicationSettings.AssignHyperLinkDiscoveries = true;
            ApplicationSettings.AutoThrottleHttpWebRequests = false;
            ApplicationSettings.ClassifyAbsoluteUris = false;
            //ApplicationSettings.ConnectionString = "";
            //ApplicationSettings.ConsoleOutputLogsDirectory = "";
            ApplicationSettings.CrawlRequestTimeoutInMinutes = 1;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseCrawlRequests = true;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseFiles = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseHyperLinks = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseImages = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseWebPages = false;
            ApplicationSettings.DesiredMaximumMemoryUsageInMegabytes = 4096;
            ApplicationSettings.DiscoverySlidingExpirationInSeconds = 120;
            //ApplicationSettings.DownloadedFilesDirectory = "";
            //ApplicationSettings.DownloadedImagesDirectory = "";
            //ApplicationSettings.DownloadedWebPagesDirectory = "";
            ApplicationSettings.EnableConsoleOutput = false;
            ApplicationSettings.ExtractFileMetaData = false;
            ApplicationSettings.ExtractImageMetaData = false;
            ApplicationSettings.ExtractWebPageMetaData = false;
            ApplicationSettings.HttpWebRequestRetries = 5;
            ApplicationSettings.InsertCrawlRequests = true;
            ApplicationSettings.InsertDisallowedAbsoluteUriDiscoveries = false;
            ApplicationSettings.InsertDisallowedAbsoluteUris = true;
            ApplicationSettings.InsertDisallowedDiscoveries = false;
            ApplicationSettings.InsertDiscoveries = true;
            ApplicationSettings.InsertEmailAddressDiscoveries = false;
            ApplicationSettings.InsertEmailAddresses = false;
            ApplicationSettings.InsertExceptions = true;
            ApplicationSettings.InsertFileDiscoveries = true;
            ApplicationSettings.InsertFileMetaData = false;
            ApplicationSettings.InsertFiles = true;
            ApplicationSettings.InsertFileSource = false;
            ApplicationSettings.InsertHyperLinkDiscoveries = false;
            ApplicationSettings.InsertHyperLinks = false;
            ApplicationSettings.InsertImageDiscoveries = true;
            ApplicationSettings.InsertImageMetaData = false;
            ApplicationSettings.InsertImages = true;
            ApplicationSettings.InsertImageSource = false;
            ApplicationSettings.InsertWebPageMetaData = false;
            ApplicationSettings.InsertWebPages = true;
            ApplicationSettings.InsertWebPageSource = true;
            ApplicationSettings.MaximumNumberOfAutoRedirects = 50;
            ApplicationSettings.MaximumNumberOfCrawlRequestsToCreatePerBatch = 1000;
            ApplicationSettings.MaximumNumberOfCrawlThreads = 10;
            ApplicationSettings.MaximumNumberOfHostsAndPrioritiesToSelect = 10000;
            ApplicationSettings.OutputConsoleToLogs = false;
            ApplicationSettings.OutputStatistics = false;
            ApplicationSettings.OutputWebExceptions = true;
            ApplicationSettings.ProcessDiscoveriesAsynchronously = true;
            ApplicationSettings.SaveDiscoveredFilesToDisk = false;
            ApplicationSettings.SaveDiscoveredImagesToDisk = false;
            ApplicationSettings.SaveDiscoveredWebPagesToDisk = false;
            ApplicationSettings.SetRefererToParentAbsoluteUri = true;
            ApplicationSettings.SqlCommandTimeoutInMinutes = 60;
            ApplicationSettings.UniqueIdentifier = "";
            ApplicationSettings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.71 Safari/534.24"; //If you find yourself blocked from crawling a website, change this to a common crawler string, such as 'Googlebot' or 'Slurp'...
            ApplicationSettings.VerboseOutput = false;
            //enable VerboseOutput to see each Discovery and the the status of each Discovery returned from each Discovery.  (e.g. WebPages from each WebPage and Files/Images from each WebPage.)
        }

        private static void AssignApplicationSettingsForUser()
        {
            //CrawlActions, CrawlRules and EngineActions can be added and set from code, overriding Database settings.

            //cfg.CrawlActions
            foreach (ACrawlAction crawlAction in _crawler.CrawlActions.Values)
            {
                crawlAction.IsEnabled = false;
            }

            //cfg.CrawlRules
            //CrawlRules limit where the Crawl can go, where not limited by 'UriClassificationType' in the CrawlRequest constructor.
            foreach (ACrawlRule crawlRule in _crawler.CrawlRules.Values)
            {
                crawlRule.IsEnabled = false;
            }

            //cfg.EngineActions
            //EngineActions are invoked when CrawlRequests are retrived from the database.  Most users wil not need these.
            foreach (AEngineAction engineAction in _crawler.Engine.EngineActions.Values)
            {
                engineAction.IsEnabled = false;
            }

            //ApplicationSettings can be set from code, overriding Database settings found in cfg.Configuration.
            ApplicationSettings.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            ApplicationSettings.AcceptEncoding = "gzip,deflate,sdch";
            ApplicationSettings.AllowAutoRedirect = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForFiles = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForHyperLinks = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForImages = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForWebPages = true;
            ApplicationSettings.AssignEmailAddressDiscoveries = false;
            ApplicationSettings.AssignFileAndImageDiscoveries = false;
            ApplicationSettings.AssignHyperLinkDiscoveries = true;
            ApplicationSettings.AutoThrottleHttpWebRequests = false;
            ApplicationSettings.ClassifyAbsoluteUris = false;
            //ApplicationSettings.ConnectionString = "";
            //ApplicationSettings.ConsoleOutputLogsDirectory = "";
            ApplicationSettings.CrawlRequestTimeoutInMinutes = 1;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseCrawlRequests = true;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseFiles = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseHyperLinks = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseImages = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseWebPages = false;
            ApplicationSettings.DesiredMaximumMemoryUsageInMegabytes = 4096;
            ApplicationSettings.DiscoverySlidingExpirationInSeconds = 120;
            //ApplicationSettings.DownloadedFilesDirectory = "";
            //ApplicationSettings.DownloadedImagesDirectory = "";
            //ApplicationSettings.DownloadedWebPagesDirectory = "";
            ApplicationSettings.EnableConsoleOutput = true;
            ApplicationSettings.ExtractFileMetaData = false;
            ApplicationSettings.ExtractImageMetaData = false;
            ApplicationSettings.ExtractWebPageMetaData = false;
            ApplicationSettings.HttpWebRequestRetries = 5;
            ApplicationSettings.InsertCrawlRequests = true;
            ApplicationSettings.InsertDisallowedAbsoluteUriDiscoveries = false;
            ApplicationSettings.InsertDisallowedAbsoluteUris = false;
            ApplicationSettings.InsertDisallowedDiscoveries = false;
            ApplicationSettings.InsertDiscoveries = true;
            ApplicationSettings.InsertEmailAddressDiscoveries = false;
            ApplicationSettings.InsertEmailAddresses = false;
            ApplicationSettings.InsertExceptions = true;
            ApplicationSettings.InsertFileDiscoveries = false;
            ApplicationSettings.InsertFileMetaData = false;
            ApplicationSettings.InsertFiles = false;
            ApplicationSettings.InsertFileSource = false;
            ApplicationSettings.InsertHyperLinkDiscoveries = false;
            ApplicationSettings.InsertHyperLinks = false;
            ApplicationSettings.InsertImageDiscoveries = false;
            ApplicationSettings.InsertImageMetaData = false;
            ApplicationSettings.InsertImages = false;
            ApplicationSettings.InsertImageSource = false;
            ApplicationSettings.InsertWebPageMetaData = false;
            ApplicationSettings.InsertWebPages = true;
            ApplicationSettings.InsertWebPageSource = false;
            ApplicationSettings.MaximumNumberOfAutoRedirects = 50;
            ApplicationSettings.MaximumNumberOfCrawlRequestsToCreatePerBatch = 1000;
            ApplicationSettings.MaximumNumberOfCrawlThreads = 50;
            ApplicationSettings.MaximumNumberOfHostsAndPrioritiesToSelect = 10000;
            ApplicationSettings.OutputConsoleToLogs = false;
            ApplicationSettings.OutputStatistics = false;
            ApplicationSettings.OutputWebExceptions = true;
            ApplicationSettings.ProcessDiscoveriesAsynchronously = true;
            ApplicationSettings.SaveDiscoveredFilesToDisk = false;
            ApplicationSettings.SaveDiscoveredImagesToDisk = false;
            ApplicationSettings.SaveDiscoveredWebPagesToDisk = false;
            ApplicationSettings.SetRefererToParentAbsoluteUri = true;
            ApplicationSettings.SqlCommandTimeoutInMinutes = 60;
            ApplicationSettings.UniqueIdentifier = "";
            ApplicationSettings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.71 Safari/534.24"; //If you find yourself blocked from crawling a website, change this to a common crawler string, such as 'Googlebot' or 'Slurp'...
            ApplicationSettings.VerboseOutput = false;
            //enable VerboseOutput to see each Discovery and the the status of each Discovery returned from each Discovery.  (e.g. WebPages from each WebPage and Files/Images from each WebPage.)
        }

        private static void Engine_CrawlRequestCanceled(CrawlRequest crawlRequest)
        {
            //if (crawlRequest != null)
            //{
            //    System.Console.ForegroundColor = ConsoleColor.Red;

            //    System.Console.WriteLine("Canceled: " + crawlRequest.Discovery.Uri.AbsoluteUri + " : " + crawlRequest.HttpWebResponseTime);

            //    System.Console.ForegroundColor = ConsoleColor.White;
            //}
        }

        private static void Engine_CrawlRequestCompleted(CrawlRequest crawlRequest)
        {
            //if (crawlRequest != null)
            //{
            //    System.Console.ForegroundColor = ConsoleColor.Green;

            //    System.Console.WriteLine("Completed: " + crawlRequest.Discovery.Uri.AbsoluteUri + " : " + crawlRequest.HttpWebResponseTime);

            //    System.Console.ForegroundColor = ConsoleColor.White;
            //}
        }

        private static void Engine_CrawlRequestRetrying(CrawlRequest crawlRequest)
        {
            //if (crawlRequest != null)
            //{
            //    System.Console.ForegroundColor = ConsoleColor.Green;

            //    System.Console.WriteLine("Retrying: " + crawlRequest.Discovery.Uri.AbsoluteUri);

            //    System.Console.ForegroundColor = ConsoleColor.White;
            //}
        }

        private static void Engine_CrawlRequestThrottled(CrawlRequest crawlRequest)
        {
            //if (crawlRequest != null)
            //{
            //    System.Console.ForegroundColor = ConsoleColor.Yellow;

            //    System.Console.WriteLine("Throttled: " + crawlRequest.Discovery.Uri.AbsoluteUri);

            //    System.Console.ForegroundColor = ConsoleColor.White;
            //}
        }

        /// <summary>
        /// Handles the OnCrawlCompleted event of the Engine control.
        /// </summary>
        /// <param name="engine">The Engine.</param>
        private static void Engine_CrawlCompleted(Engine engine)
        {
            _stopwatch.Stop();

            System.Console.ForegroundColor = ConsoleColor.White;

            System.Console.WriteLine("\nElapsed: " + _stopwatch.Elapsed);
            System.Console.WriteLine((_crawler.NumberOfFilesCrawled / _stopwatch.Elapsed.TotalSeconds) + " Files/sec.");
            System.Console.WriteLine((_crawler.NumberOfImagesCrawled / _stopwatch.Elapsed.TotalSeconds) + " Images/sec.");
            System.Console.WriteLine((_crawler.NumberOfWebPagesCrawled / _stopwatch.Elapsed.TotalSeconds) + " WebPages/sec.");
            System.Console.WriteLine(((_crawler.NumberOfFilesCrawled + _crawler.NumberOfImagesCrawled + _crawler.NumberOfWebPagesCrawled) / _stopwatch.Elapsed.TotalSeconds) + " Discoveries/sec.");
#if DEMO
            System.Console.WriteLine("Crawl rate is limited by DEMO protection as well as web server restrictions.\nCrawl the test site for a maximum performance benchmark.");
#endif
            System.Console.WriteLine("\nPress any key to terminate arachnode.net");
#if DEMO
            System.Console.WriteLine("\nSet 'Web' as the 'Startup Project' and examine 'Search.aspx' to view results.");
#endif
            System.Console.ForegroundColor = ConsoleColor.Gray;

            _hasCrawlCompleted = true;
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;

            _crawler.Engine.Stop();
        }
    }
}