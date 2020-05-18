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
using System.Threading;
using System.Windows.Forms;
using Arachnode.Configuration;
using Arachnode.DataSource;
using Arachnode.SiteCrawler;
using Arachnode.SiteCrawler.Utilities;
using Arachnode.SiteCrawler.Value.AbstractClasses;
using Arachnode.SiteCrawler.Value.Enums;

#endregion

namespace Arachnode.PostProcessing
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();

            WebPageUtilities.OnWebPageProcessed += WebPageUtilities_OnWebPageProcessed;
            FileUtilities.OnFileProcessed += FileUtilities_OnFileProcessed;
            ImageUtilities.OnImageProcessed += ImageUtilities_OnImageProcessed;
        }

        private void SetCrawlActionsCrawlRulesEngineActionsAndApplicationSettings(Crawler crawler)
        {
            //CrawlActions, CrawlRules and EngineActions can be set from code, overriding Database settings.

            //cfg.CrawlActions
            foreach (ACrawlAction crawlAction in crawler.CrawlActions.Values)
            {
                //if (crawlAction.TypeName != "Arachnode.Plugins.CrawlActions.ManageLuceneDotNetIndexes")
                //{
                crawlAction.IsEnabled = false;
                //}
            }

            //cfg.CrawlRules
            foreach (ACrawlRule crawlRule in crawler.CrawlRules.Values)
            {
                crawlRule.IsEnabled = false;
            }

            //cfg.EngineActions
            foreach (AEngineAction engineAction in crawler.Engine.EngineActions.Values)
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
            ApplicationSettings.ClassifyAbsoluteUris = true;
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
            ApplicationSettings.ExtractFileMetaData = true;
            ApplicationSettings.ExtractImageMetaData = true;
            ApplicationSettings.ExtractWebPageMetaData = true;
            ApplicationSettings.HttpWebRequestRetries = 5;
            ApplicationSettings.InsertDisallowedAbsoluteUriDiscoveries = true;
            ApplicationSettings.InsertDisallowedAbsoluteUris = true;
            ApplicationSettings.InsertEmailAddressDiscoveries = true;
            ApplicationSettings.InsertEmailAddresses = true;
            ApplicationSettings.InsertExceptions = true;
            ApplicationSettings.InsertFileDiscoveries = true;
            ApplicationSettings.InsertFileMetaData = true;
            ApplicationSettings.InsertFiles = true;
            ApplicationSettings.InsertFileSource = false;
            ApplicationSettings.InsertHyperLinkDiscoveries = true;
            ApplicationSettings.InsertHyperLinks = true;
            ApplicationSettings.InsertImageDiscoveries = true;
            ApplicationSettings.InsertImageMetaData = true;
            ApplicationSettings.InsertImages = true;
            ApplicationSettings.InsertImageSource = false;
            ApplicationSettings.InsertWebPageMetaData = true;
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

        private void btnProcessWebPages_Click(object sender, EventArgs e)
        {
            btnProcessWebPages.Enabled = false;

            Thread thread = new Thread(
                delegate()
                    {
                        //used to load the CrawlActions, CrawlRules and EngineActions...
                        Crawler crawler = new Crawler(CrawlMode.BreadthFirstByPriority, false);

                        SetCrawlActionsCrawlRulesEngineActionsAndApplicationSettings(crawler);

                        WebPageUtilities.ProcessWebPages(crawler, (long) nudWebPageIDLowerBound.Value, (long) nudWebPageIDUpperBound.Value);

                        BeginInvoke(new MethodInvoker(delegate { btnProcessWebPages.Enabled = true; }));
                    }
                );

            thread.Start();
        }

        private void btnProcessFiles_Click(object sender, EventArgs e)
        {
            btnProcessFiles.Enabled = false;

            Thread thread = new Thread(
                delegate()
                    {
                        //used to load the CrawlActions, CrawlRules and EngineActions...
                        Crawler crawler = new Crawler(CrawlMode.BreadthFirstByPriority, false);

                        SetCrawlActionsCrawlRulesEngineActionsAndApplicationSettings(crawler);

                        FileUtilities.ProcessFiles(crawler, (long) nudFileIDLowerBound.Value, (long) nudFileIDUpperBound.Value);

                        BeginInvoke(new MethodInvoker(delegate { btnProcessFiles.Enabled = true; }));
                    }
                );

            thread.Start();
        }

        private void btnProcessImages_Click(object sender, EventArgs e)
        {
            btnProcessImages.Enabled = false;

            Thread thread = new Thread(
                delegate()
                    {
                        //used to load the CrawlActions, CrawlRules and EngineActions...
                        Crawler crawler = new Crawler(CrawlMode.BreadthFirstByPriority, false);

                        SetCrawlActionsCrawlRulesEngineActionsAndApplicationSettings(crawler);

                        ImageUtilities.ProcessImages(crawler, (long) nudImagesIDLowerBound.Value, (long) nudImagesIDUpperBound.Value);

                        BeginInvoke(new MethodInvoker(delegate { btnProcessImages.Enabled = true; }));
                    }
                );

            thread.Start();
        }

        private void ImageUtilities_OnImageProcessed(ArachnodeDataSet.ImagesRow imagesRow, string message)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  rtbPostProcessingStatus.Text = message + Environment.NewLine + rtbPostProcessingStatus.Text;

                                                  if (rtbPostProcessingStatus.Text.Length > 10000)
                                                  {
                                                      rtbPostProcessingStatus.Text = rtbPostProcessingStatus.Text.Substring(0, 10000);
                                                  }
                                              }));

            Application.DoEvents();

            Thread.Sleep(100);
        }

        private void FileUtilities_OnFileProcessed(ArachnodeDataSet.FilesRow filesRow, string message)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  rtbPostProcessingStatus.Text = message + Environment.NewLine + rtbPostProcessingStatus.Text;

                                                  if (rtbPostProcessingStatus.Text.Length > 10000)
                                                  {
                                                      rtbPostProcessingStatus.Text = rtbPostProcessingStatus.Text.Substring(0, 10000);
                                                  }
                                              }));

            Application.DoEvents();

            Thread.Sleep(100);
        }

        private void WebPageUtilities_OnWebPageProcessed(ArachnodeDataSet.WebPagesRow webPagesRow, string message)
        {
            BeginInvoke(new MethodInvoker(delegate
                                              {
                                                  rtbPostProcessingStatus.Text = message + Environment.NewLine + rtbPostProcessingStatus.Text;

                                                  if (rtbPostProcessingStatus.Text.Length > 10000)
                                                  {
                                                      rtbPostProcessingStatus.Text = rtbPostProcessingStatus.Text.Substring(0, 10000);
                                                  }
                                              }));

            Application.DoEvents();

            Thread.Sleep(100);
        }
    }
}