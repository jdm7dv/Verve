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
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using Arachnode.Configuration;
using Arachnode.DataAccess;
using Arachnode.Renderer.Value.Enums;
using Arachnode.SiteCrawler;
using Arachnode.SiteCrawler.Core;
using Arachnode.SiteCrawler.Value;
using Arachnode.SiteCrawler.Value.AbstractClasses;
using Arachnode.SiteCrawler.Value.Enums;

#endregion

namespace Arachnode.Service
{
    public partial class Service1 : ServiceBase
    {
        private static Crawler _crawler;
        private static readonly object _crawlRequestsLock = new object();
        private static FileSystemWatcher _fileSystemWatcher;

        /// <summary>
        /// 	Initializes a new instance of the <see cref = "Service1" /> class.
        /// </summary> 
        public Service1()
        {
            InitializeComponent();

            //OnStart(null);

            //README: To install the Service, open the VS2008 command prompt and type "installutil /I [Path to Arachnode.Service.exe]".
            //README: Use 'net start arachnode.net' and 'net stop arachnode.net' to start and stop the service while debugging.
            //README: If the Windows Service Manager is open, and viewable to the user, the machine may need to be rebooted to complete the installation or removal.
            //README: Thus, use 'net start/stop'.
        }

        /// <summary>
        /// 	When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name = "args">Data passed by the start command.</param>
        protected override void OnStart(string[] args)
        {
            //If you receive a TypeInitializationException, copy ConnectionStrings.config from \Console\bin\[Debug/Release] to \Service\bin\[Debug/Release]...

            try
            {
                new EventLog("Application", Environment.MachineName, "arachnode.net").WriteEntry("The arachnode.net service has started.", EventLogEntryType.Information);

                if (!File.Exists("ConnectionStrings.config"))
                {
                    if (File.Exists(@"..\..\..\Configuration\ConnectionStrings.config"))
                    {
                        File.Copy(@"..\..\..\Configuration\ConnectionStrings.config", "ConnectionStrings.config");
                    }
                }

                _crawler = new Crawler(CrawlMode.BreadthFirstByPriority, true);

                if (!File.Exists("CrawlRequests.txt"))
                {
                    File.Create("CrawlRequests.txt");
                }

                _fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(Environment.CommandLine.Replace("\"", string.Empty)), "CrawlRequests.txt");
                _fileSystemWatcher.EnableRaisingEvents = true;
                _fileSystemWatcher.Changed += _fileSystemWatcher_Changed;

                //Be careful with this setting.  Setting this to ProcessPriorityClass.RealTime may lock the machine.
                Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;

                ResetCrawler();

                //CrawlActions, CrawlRules and EngineActions can be set from code, overriding Database settings.
                OverrideDatabaseSettings();

                ParseCrawlRequests();

                _crawler.Engine.StopEngineOnCrawlCompleted = false;
                _crawler.Engine.Start();
                _crawler.Engine.CrawlRequestCompleted += Engine_CrawlRequestCompleted;
                _crawler.Engine.CrawlCompleted += Engine_CrawlCompleted;
            }
            catch (Exception exception)
            {
                new ArachnodeDAO(false, false).InsertException(null, null, exception, true);

                OnStop();
            }
        }

        private void OverrideDatabaseSettings()
        {
            foreach (ACrawlAction crawlAction in _crawler.CrawlActions.Values)
            {
                //if (crawlAction.TypeName != "Arachnode.Plugins.CrawlActions.ManageLuceneDotNetIndexes")
                //{
                    crawlAction.IsEnabled = false;
                //}
            }

            //cfg.CrawlRules
            foreach (ACrawlRule crawlRule in _crawler.CrawlRules.Values)
            {
                crawlRule.IsEnabled = false;
            }

            //cfg.EngineActions
            foreach (AEngineAction engineAction in _crawler.Engine.EngineActions.Values)
            {
                engineAction.IsEnabled = false;
            }

            //ApplicationSettings can be set from code, overriding Database settings found in cfg.Configuration.
            ApplicationSettings.AssignCrawlRequestPrioritiesForFiles = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForHyperLinks = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForImages = true;
            ApplicationSettings.AssignCrawlRequestPrioritiesForWebPages = true;
            ApplicationSettings.AssignEmailAddressDiscoveries = false;
            ApplicationSettings.AssignFileAndImageDiscoveries = true;
            ApplicationSettings.AssignHyperLinkDiscoveries = true;
            ApplicationSettings.ClassifyAbsoluteUris = false;
            //ApplicationSettings.ConnectionString = "";
            //ApplicationSettings.ConsoleOutputLogsDirectory = "";
            ApplicationSettings.CrawlRequestTimeoutInMinutes = 1;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseCrawlRequests = true;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseFiles = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseHyperLinks = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseImages = false;
            ApplicationSettings.CreateCrawlRequestsFromDatabaseWebPages = false;
            ApplicationSettings.DesiredMaximumMemoryUsageInMegabytes = 1024;
            //ApplicationSettings.DownloadedFilesDirectory = "";
            //ApplicationSettings.DownloadedImagesDirectory = "";
            //ApplicationSettings.DownloadedWebPagesDirectory = "";
            ApplicationSettings.EnableConsoleOutput = true;
            ApplicationSettings.ExtractFileMetaData = false;
            ApplicationSettings.ExtractImageMetaData = false;
            ApplicationSettings.ExtractWebPageMetaData = false;
            ApplicationSettings.HttpWebRequestRetries = 5;
            ApplicationSettings.InsertDisallowedAbsoluteUriDiscoveries = false;
            ApplicationSettings.InsertDisallowedAbsoluteUris = true;
            ApplicationSettings.InsertEmailAddressDiscoveries = false;
            ApplicationSettings.InsertEmailAddresses = false;
            ApplicationSettings.InsertExceptions = true;
            ApplicationSettings.InsertFileDiscoveries = false;
            ApplicationSettings.InsertFileMetaData = false;
            ApplicationSettings.InsertFiles = true;
            ApplicationSettings.InsertFileSource = false;
            ApplicationSettings.InsertHyperLinkDiscoveries = false;
            ApplicationSettings.InsertHyperLinks = false;
            ApplicationSettings.InsertImageDiscoveries = false;
            ApplicationSettings.InsertImageMetaData = false;
            ApplicationSettings.InsertImages = true;
            ApplicationSettings.InsertImageSource = false;
            ApplicationSettings.InsertWebPageMetaData = false;
            ApplicationSettings.InsertWebPages = true;
            ApplicationSettings.InsertWebPageSource = false;
            ApplicationSettings.MaximumNumberOfCrawlRequestsToCreatePerBatch = 1000;
            ApplicationSettings.MaximumNumberOfCrawlThreads = 10;
            ApplicationSettings.MaximumNumberOfHostsAndPrioritiesToSelect = 10000;
            ApplicationSettings.OutputConsoleToLogs = false;
            ApplicationSettings.OutputStatistics = false;
            ApplicationSettings.SaveDiscoveredFilesToDisk = true;
            ApplicationSettings.SaveDiscoveredImagesToDisk = true;
            ApplicationSettings.SaveDiscoveredWebPagesToDisk = true;
            ApplicationSettings.SqlCommandTimeoutInMinutes = 60;
            ApplicationSettings.UserAgent = "Your unique UserAgent string.";
            ApplicationSettings.VerboseOutput = false;
        }

        private void _fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            ParseCrawlRequests();
        }

        private static void ParseCrawlRequests()
        {
            try
            {
                lock (_crawlRequestsLock)
                {
                    string[] crawlRequests = File.ReadAllLines(Path.Combine(Path.GetDirectoryName(Environment.CommandLine.Replace("\"", string.Empty)), "CrawlRequests.txt"));

                    foreach (string crawlRequest in crawlRequests)
                    {
                        if (crawlRequest.Trim().StartsWith("//"))
                        {
                            continue;
                        }

                        string[] crawlRequestSplit = crawlRequest.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        UriClassificationType restrictCrawlTo = UriClassificationType.None;

                        foreach (string uriClassificationType in crawlRequestSplit[2].Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                        {
                            restrictCrawlTo |= (UriClassificationType) Enum.Parse(typeof (UriClassificationType), uriClassificationType);
                        }

                        UriClassificationType restrictDiscoveriesTo = UriClassificationType.None;

                        foreach (string uriClassificationType in crawlRequestSplit[3].Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                        {
                            restrictDiscoveriesTo |= (UriClassificationType) Enum.Parse(typeof (UriClassificationType), uriClassificationType);
                        }

                        CrawlRequest crawlRequest2 = new CrawlRequest(new Discovery(crawlRequestSplit[0]), int.Parse(crawlRequestSplit[1]), restrictCrawlTo, restrictDiscoveriesTo, double.Parse(crawlRequestSplit[4]), (RenderType) Enum.Parse(typeof (RenderType), crawlRequestSplit[5]), (RenderType) Enum.Parse(typeof (RenderType), crawlRequestSplit[6]));

                        _crawler.Crawl(crawlRequest2);
                    }
                }
            }
            catch (Exception exception)
            {
                new ArachnodeDAO(false, false).InsertException(null, null, exception, true);
            }
        }

        /// <summary>
        /// 	When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            _crawler.Engine.Stop();

            new EventLog("Application", Environment.MachineName, "arachnode.net").WriteEntry("The arachnode.net service has stopped.", EventLogEntryType.Information);
        }

        protected override void OnPause()
        {
            base.OnPause();

            _crawler.Engine.Pause();

            new EventLog("Application", Environment.MachineName, "arachnode.net").WriteEntry("The arachnode.net service has paused.", EventLogEntryType.Information);
        }

        protected override void OnContinue()
        {
            base.OnContinue();

            _crawler.Engine.Resume();

            new EventLog("Application", Environment.MachineName, "arachnode.net").WriteEntry("The arachnode.net service has resumed.", EventLogEntryType.Information);
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();

            new EventLog("Application", Environment.MachineName, "arachnode.net").WriteEntry("The arachnode.net service is stopping due to system shutdown.", EventLogEntryType.Warning);

            _crawler.Engine.Stop();
        }

        private void Engine_CrawlRequestCompleted(CrawlRequest crawlRequest)
        {
        }

        /// <summary>
        /// 	Handles the OnCrawlCompleted event of the Engine control.
        /// </summary>
        private void Engine_CrawlCompleted(Engine engine)
        {
            //logic for re-crawling a list of CrawlRequests from CrawlRequests.txt.

            new EventLog("Application", Environment.MachineName, "arachnode.net").WriteEntry("The arachnode.net Crawl has completed.", EventLogEntryType.Information);

            try
            {
                //double locking is used to coordinate a possible SCM pause and changes to the CrawlRequests.txt file...
                //we want to ensure that if CrawlRequests.txt has been modified that it will wait until our work here is done, or vice versa, and
                //if we receive a SCM message for Pause, that we honor this Pause and allow the event to be refired from the Engine once Resume or Stop is called...
                if (_crawler.Engine.State == EngineState.Start)
                {
                    lock (_crawlRequestsLock)
                    {
                        if (_crawler.Engine.State == EngineState.Start)
                        {
                            //all crawl theads will complete their current action before pause completes.
                            //the double locking above should never occur as Pause DOES wait for all Crawls to stop at the ResetEvent.
                            _crawler.Engine.Pause();

                            //resets the state of the Crawler to as it was when the service was first started...
                            ResetCrawler();

                            //resets the Actions and Rules, loading settings from the database...
                            _crawler.ResetCrawlActions();
                            _crawler.ResetCrawlRules();

                            //provides application specific settings, like in Console\Program.cs...
                            OverrideDatabaseSettings();

                            //read the CrawlRequests from CrawlRequests.txt...
                            ParseCrawlRequests();

                            //resume the Engine...
                            _crawler.Engine.Resume();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                new ArachnodeDAO(false, false).InsertException(null, null, exception, true);
            }
        }

        private void ResetCrawler()
        {
            //clear the Discoveries...
            _crawler.ClearDiscoveries();

            //clear the Politeness...
            _crawler.ClearPolitenesses();

            //clear the uncrawled CrawlRequests...
            _crawler.ClearUncrawledCrawlRequests();

            //clear the DisallowedAbsoluteUris table...
            _crawler.ClearDisallowedAbsoluteUris();
        }
    }
}