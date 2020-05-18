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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Arachnode.Configuration;
using Arachnode.DataAccess;
using Arachnode.GraphicalUserInterface;
using Arachnode.Renderer.Value.Enums;
using Arachnode.SiteCrawler;
using Arachnode.SiteCrawler.Core;
using Arachnode.SiteCrawler.Value;
using Arachnode.SiteCrawler.Value.AbstractClasses;
using Arachnode.SiteCrawler.Value.Enums;
using Timer = System.Windows.Forms.Timer;

#endregion

namespace GraphicalUserInterface
{
    public partial class Form1 : Form
    {
        private readonly Dictionary<int, PerformanceCounter> _arachnodeDAOCounters = new Dictionary<int, PerformanceCounter>();
        private readonly ArachnodeDataSourceDataContext _arachnodeDataSourceDataContext = new ArachnodeDataSourceDataContext(ApplicationSettings.ConnectionString);
        private readonly Dictionary<int, PerformanceCounter> _cacheCounters = new Dictionary<int, PerformanceCounter>();
        private readonly Dictionary<int, PerformanceCounter> _crawlCounters = new Dictionary<int, PerformanceCounter>();

        private readonly Crawler _crawler;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Timer _timer = new Timer();

        public Form1()
        {
            InitializeComponent();

            try
            {
                //instantiate the crawler...
                _crawler = new Crawler(CrawlMode.BreadthFirstByPriority, false);

                _crawler.Engine.CrawlRequestCompleted += Engine_CrawlRequestCompleted;
                _crawler.Engine.CrawlCompleted += Engine_CrawlCompleted;
                _crawler.Engine.CrawlRequestThrottled += new Engine.OnCrawlRequestThrottledEventHandler(Engine_CrawlRequestThrottled);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + Environment.NewLine + exception.StackTrace, "arachnode.net | graphical user interface");

                Process.GetCurrentProcess().Kill();
            }

            //CrawlActions, CrawlRules and EngineActions can be set from code, overriding Database settings.

            //cfg.CrawlActions
            foreach (ACrawlAction crawlAction in _crawler.CrawlActions.Values)
            {
                if (crawlAction.TypeName != "Arachnode.Plugins.CrawlActions.ManageLuceneDotNetIndexes")
                {
                    crawlAction.IsEnabled = false;
                }
            }

            //cfg.CrawlRules
            foreach (ACrawlRule crawlRule in _crawler.CrawlRules.Values)
            {
                if (crawlRule.TypeName != "Arachnode.Plugins.CrawlRules.AbsoluteUri")
                {
                    crawlRule.IsEnabled = false;
                }
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
            ApplicationSettings.InsertHyperLinks = true;
            ApplicationSettings.InsertImageDiscoveries = false;
            ApplicationSettings.InsertImageMetaData = false;
            ApplicationSettings.InsertImages = true;
            ApplicationSettings.InsertImageSource = false;
            ApplicationSettings.InsertWebPageMetaData = false;
            ApplicationSettings.InsertWebPages = true;
            ApplicationSettings.InsertWebPageSource = true;
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

            var thread = new Thread(() =>
                                        {
                                            //load application settings...
                                            var applicationSettings = new ApplicationSettings();

                                            foreach (PropertyInfo propertyInfo in applicationSettings.GetType().GetProperties())
                                            {
                                                object value = propertyInfo.GetValue(applicationSettings, null);

                                                dgvApplicationSettings.Rows.Add(propertyInfo.Name, value);
                                            }

                                            //load web settings...
                                            var webSettings = new WebSettings();

                                            //load configuration data...
                                            foreach (PropertyInfo propertyInfo in webSettings.GetType().GetProperties())
                                            {
                                                object value = propertyInfo.GetValue(webSettings, null);

                                                dgvWebSettings.Rows.Add(propertyInfo.Name, value);
                                            }

                                            //load configuration data...
                                            foreach (PropertyInfo propertyInfo in Type.GetType("Arachnode.GraphicalUserInterface.ArachnodeDataSourceDataContext").GetProperties())
                                            {
                                                if (propertyInfo.PropertyType.FullName.StartsWith("System.Data.Linq.Table"))
                                                {
                                                    dgvTableName.Rows.Add(propertyInfo.Name);
                                                }
                                            }

                                            dgvTableName.Rows[0].Selected = true;

                                            //initialize performance counters...
                                            foreach (PerformanceCounterCategory performanceCounterCategory in PerformanceCounterCategory.GetCategories())
                                            {
                                                if (performanceCounterCategory.CategoryName == "arachnode.net:ArachnodeDAO")
                                                {
                                                    foreach (PerformanceCounter performanceCounter in performanceCounterCategory.GetCounters())
                                                    {
                                                        int index = dgvCounters.Rows.Add(performanceCounter.CategoryName.Replace("arachnode.net:", string.Empty), performanceCounter.CounterName.Replace("arachnode.net - ", string.Empty), 0, 0, 0, 0);

                                                        _arachnodeDAOCounters.Add(index, performanceCounter);
                                                    }
                                                }

                                                if (performanceCounterCategory.CategoryName == "arachnode.net:Cache")
                                                {
                                                    foreach (PerformanceCounter performanceCounter in performanceCounterCategory.GetCounters())
                                                    {
                                                        int index = dgvCounters.Rows.Add(performanceCounter.CategoryName.Replace("arachnode.net:", string.Empty), performanceCounter.CounterName.Replace("arachnode.net - ", string.Empty), 0, 0, 0, 0);

                                                        _cacheCounters.Add(index, performanceCounter);
                                                    }
                                                }

                                                if (performanceCounterCategory.CategoryName == "arachnode.net:Crawl")
                                                {
                                                    foreach (PerformanceCounter performanceCounter in performanceCounterCategory.GetCounters())
                                                    {
                                                        int index = dgvCounters.Rows.Add(performanceCounter.CategoryName.Replace("arachnode.net:", string.Empty), performanceCounter.CounterName.Replace("arachnode.net - ", string.Empty), 0, 0, 0, 0);

                                                        _crawlCounters.Add(index, performanceCounter);
                                                    }
                                                }
                                            }

                                            _timer.Start();
                                        });

            thread.Start();

            _timer.Enabled = true;
            _timer.Interval = 1000;
            _timer.Tick += _timer_Tick;

            //HACK: Remove me...
            dgvCrawlRequests.Rows.Add("http://arachnode.net/", 2, 1);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(_crawler.Engine.State != EngineState.Stop)
            {
                if(MessageBox.Show("The crawler is not stopped.  Quit anyway?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    Process.GetCurrentProcess().Kill();
                }
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            tsslEngineState.Text = "Engine State: " + _crawler.Engine.State;
            tsslElapasedTime.Text = "Elapsed Time: " + _stopwatch.Elapsed;

            foreach (var keyValuePair in _arachnodeDAOCounters)
            {
                dgvCounters.Rows[keyValuePair.Key].Cells["dgvtbcValue"].Value = keyValuePair.Value.RawValue;
            }

            foreach (var keyValuePair in _cacheCounters)
            {
                dgvCounters.Rows[keyValuePair.Key].Cells["dgvtbcValue"].Value = keyValuePair.Value.RawValue;
            }

            foreach (var keyValuePair in _crawlCounters)
            {
                dgvCounters.Rows[keyValuePair.Key].Cells["dgvtbcValue"].Value = keyValuePair.Value.RawValue;
            }

            foreach (DataGridViewRow dataGridViewRow in dgvCounters.Rows)
            {
                double value = double.Parse(dataGridViewRow.Cells["dgvtbcValue"].Value.ToString());

                /**/

                double total = double.Parse(dataGridViewRow.Cells["dgvtbcTotal"].Value.ToString());

                total += value;

                dataGridViewRow.Cells["dgvtbcTotal"].Value = total;

                /**/

                double counted = double.Parse(dataGridViewRow.Cells["dgvtbcCounted"].Value.ToString());

                counted++;

                dataGridViewRow.Cells["dgvtbcCounted"].Value = counted;

                /**/

                double average = total/counted;

                dataGridViewRow.Cells["dgvtbcAverage"].Value = Math.Round(average, 2);

                /**/

                if (value > average)
                {
                    dataGridViewRow.Cells["dgvtbcValue"].Style.BackColor = Color.MediumSeaGreen;
                }
                else if (value == average)
                {
                    dataGridViewRow.Cells["dgvtbcValue"].Style.BackColor = Color.Empty;
                }
                else if (value < average)
                {
                    dataGridViewRow.Cells["dgvtbcValue"].Style.BackColor = Color.LightCoral;
                }
            }
        }

        private void dgvTableName_SelectionChanged(object sender, EventArgs e)
        {
            PropertyInfo propertyInfo = _arachnodeDataSourceDataContext.GetType().GetProperty(dgvTableName.SelectedRows[0].Cells["dgvtbcTableName"].Value.ToString());

            //System.Data.Linq.Table<AllowedDataType> configurationTable = new System.Data.Linq.Table<AllowedDataType>();

            //propertyInfo.GetValue(configurationTable, null);

            //BindingSource bindingSource = new BindingSource(memberInfo, "ID");

            //dgvConfiguration.b
            //dgvConfiguration.DataSource = (System.Data.Linq.Table<System.Data.Linq.) memberInfo;
        }

        private void ResetDatabase()
        {
            var arachnodeDAO = new ArachnodeDAO();

            arachnodeDAO.ExecuteSql("EXEC [dbo].[arachnode_usp_arachnode.net_RESET_DATABASE]");
        }

        private void ResetCrawler()
        {
            _crawler.ClearDiscoveries();
            _crawler.ClearPolitenesses();
            _crawler.ClearUncrawledCrawlRequests();
            _crawler.ClearDisallowedAbsoluteUris();
        }

        #region Button Events

        private void btnLoadCrawlRequests_Click(object sender, EventArgs e)
        {
            if (ofdCrawlRequests.ShowDialog() == DialogResult.OK)
            {
                dgvCrawlRequests.Rows.Clear();

                string[] allLines = File.ReadAllLines(ofdCrawlRequests.FileName);

                foreach(string line in allLines)
                {
                    string[] lineSplit = line.Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    if (lineSplit.Length != 0)
                    {
                        dgvCrawlRequests.Rows.Add(lineSplit[0], lineSplit[1], lineSplit[2]);
                    }
                }
            }
        }

        private void btnSaveCrawlRequests_Click(object sender, EventArgs e)
        {
            if (sfdCrawlRequests.ShowDialog() == DialogResult.OK)
            {
                StringBuilder stringBuilder = new StringBuilder();

                foreach (DataGridViewRow dataGridViewRow in dgvCrawlRequests.Rows)
                {
                    foreach(DataGridViewCell dataGridViewCell in dataGridViewRow.Cells)
                    {
                        stringBuilder.Append(dataGridViewCell.Value + "\t");
                    }

                    stringBuilder.Append(Environment.NewLine);
                }

                File.WriteAllText(sfdCrawlRequests.FileName, stringBuilder.ToString());
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;

            if (resetDatabaseOnStartToolStripMenuItem.Checked)
            {
                ResetDatabase();
            }

            if (resetCrawlerOnStartToolStripMenuItem.Checked)
            {
                ResetCrawler();
            }

            foreach (DataGridViewRow dataGridViewRow in dgvCrawlRequests.Rows)
            {
                try
                {
                    string absoluteUri = dataGridViewRow.Cells["dgvtbcCRAbsoluteUri"].Value.ToString();
                    int depth = int.Parse(dataGridViewRow.Cells["dgvtbcCRDepth"].Value.ToString());
                    double priority = double.Parse(dataGridViewRow.Cells["dgvtbcCRPriority"].Value.ToString());

                    var crawlRequest = new CrawlRequest(new Discovery(absoluteUri), depth, UriClassificationType.Domain, UriClassificationType.Domain, priority, RenderType.None, RenderType.None);

                    bool wasTheCrawlRequestAddedForCrawling = _crawler.Crawl(crawlRequest);

                    dataGridViewRow.Cells["dgvtbcCRAdded"].Value = wasTheCrawlRequestAddedForCrawling;

                    if (wasTheCrawlRequestAddedForCrawling)
                    {
                        dataGridViewRow.Cells["dgvtbcCRAdded"].Style.BackColor = Color.MediumSeaGreen;
                    }
                    else
                    {
                        dataGridViewRow.Cells["dgvtbcCRAdded"].Style.BackColor = Color.LightCoral;
                    }
                }
                catch
                {
                    dataGridViewRow.Cells["dgvtbcCRAdded"].Style.BackColor = Color.Red;
                }
            }

            dgvCrawls.Rows.Clear();

            for (int i = 1; i <= ApplicationSettings.MaximumNumberOfCrawlThreads; i++)
            {
                dgvCrawls.Rows.Add(i, string.Empty, string.Empty);
            }

            _stopwatch.Reset();
            _stopwatch.Start();

            _crawler.Engine.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;

            _crawler.Engine.Stop();

            btnStart.Enabled = true;
            btnStop.Enabled = true;
        }

        #endregion

        #region Engine Events

        private void Engine_CrawlRequestCompleted(CrawlRequest crawlRequest)
        {
            while (crawlRequest.Crawl.IsProcessingDiscoveriesAsynchronously)
            {
                Thread.Sleep(1);
            }

            if (dgvCrawls.Rows.Count != 0)
            {
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcThreadNumber"].Style.BackColor = Color.Empty;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcAbsoluteUri"].Value = crawlRequest.Discovery.Uri.AbsoluteUri;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcLastRequested"].Value = crawlRequest.Politeness.LastHttpWebRequestRequested;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcLastCanceled"].Value = crawlRequest.Politeness.LastHttpWebRequestCanceled;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcLastCompleted"].Value = crawlRequest.Politeness.LastHttpWebRequestCompleted;
                if (crawlRequest.WebClient.HttpWebResponse != null)
                {
                    dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcStatusCode"].Value = crawlRequest.WebClient.HttpWebResponse.StatusCode;
                }
                else
                {
                    dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcStatusCode"].Value = "NULL";
                }
                if (dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcStatusCode"].Value.ToString() == "OK")
                {
                    dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcStatusCode"].Style.BackColor = Color.MediumSeaGreen;
                }
                else
                {
                    dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcStatusCode"].Style.BackColor = Color.LightCoral;
                }
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcDiscoveryType"].Value = crawlRequest.Discovery.DiscoveryType;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcHyperLinkDiscoveries"].Value = crawlRequest.Discoveries.HyperLinks.Count;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcFileAndImageDiscoveries"].Value = crawlRequest.Discoveries.FilesAndImages.Count;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcCurrentDepth"].Value = crawlRequest.CurrentDepth;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcMaximumDepth"].Value = crawlRequest.MaximumDepth;
            }
        }

        void Engine_CrawlRequestThrottled(CrawlRequest crawlRequest)
        {
            if (dgvCrawls.Rows.Count != 0)
            {
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcThreadNumber"].Style.BackColor = Color.Yellow;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcAbsoluteUri"].Value = crawlRequest.Discovery.Uri.AbsoluteUri;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcLastRequested"].Value = crawlRequest.Politeness.LastHttpWebRequestRequested;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcLastCanceled"].Value = crawlRequest.Politeness.LastHttpWebRequestCanceled;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcLastCompleted"].Value = crawlRequest.Politeness.LastHttpWebRequestCompleted;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcStatusCode"].Value = "NULL";
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcStatusCode"].Style.BackColor = Color.Empty;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcDiscoveryType"].Value = crawlRequest.Discovery.DiscoveryType;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcHyperLinkDiscoveries"].Value = crawlRequest.Discoveries.HyperLinks.Count;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcFileAndImageDiscoveries"].Value = crawlRequest.Discoveries.FilesAndImages.Count;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcCurrentDepth"].Value = crawlRequest.CurrentDepth;
                dgvCrawls.Rows[crawlRequest.Crawl.CrawlInfo.ThreadNumber - 1].Cells["dgvtbcMaximumDepth"].Value = crawlRequest.MaximumDepth;
            }
        }

        private void Engine_CrawlCompleted(Engine engine)
        {
            _stopwatch.Stop();

            btnStart.Enabled = true;
        }

        #endregion

        #region Menu Events

        private void resetDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Reset Database?", "Confirm Action", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                ResetDatabase();
            }
        }

        private void resetCrawlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Reset Crawler?", "Confirm Action", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                ResetCrawler();
            }
        }

        #endregion
    }
}