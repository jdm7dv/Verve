using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Diagnostics;
using System.Timers;
using System.Web;

namespace Arachnode.Console.Benchmark
{
    class Program
    {
        private static Stopwatch _stopwatch = new Stopwatch();
        private static bool? _generateTestSite = false;

        private static object _queueLock = new object();
        private static Queue<string> _absoluteUris = new Queue<string>();
        private static double _numberOfAbsoluteUrisDownloaded;

        private static int _numberOfThreads = 100;

        private static bool _assignHyperLinkDiscoveries;
        private static bool _cacheToFileSystem;
        private static bool _saveWebPagesToDisk;

        static void Main(string[] args)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("arachnode.net AN.Benchmark " + Assembly.GetExecutingAssembly().GetName().Version + "\n");
            System.Console.ForegroundColor = ConsoleColor.White;
          
            System.Console.WriteLine("Generate the TestSite?  (http://localhost:56830/Test/)  (y/n)");

            if ((_generateTestSite.HasValue && _generateTestSite.Value) || (!_generateTestSite.HasValue && System.Console.ReadLine().ToLower() == "y"))
            {
                System.Console.WriteLine("Generating " + 14606 + " WebPages...");

                ProcessStartInfo processStartInfo = new ProcessStartInfo("TestSite.exe");
                processStartInfo.WorkingDirectory = "..\\..\\..\\Web\\Test";

                Process process = Process.Start(processStartInfo);

                process.WaitForExit();
            }

            //remove limits from service point manager
            ServicePointManager.MaxServicePoints = int.MaxValue;
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            ServicePointManager.CheckCertificateRevocationList = true;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.MaxServicePointIdleTime = 1000 * 30;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.UseNagleAlgorithm = false;

            System.Timers.Timer timer = new System.Timers.Timer();

            timer.Interval = 1000;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Enabled = true;

            _assignHyperLinkDiscoveries = false;
            _cacheToFileSystem = false;
            _saveWebPagesToDisk = false;
            timer.Start();
            System.Console.WriteLine("\n[START]: Crawling without HyperLink parsing or WebPage storage...");
            _stopwatch.Start();
            Crawl();

            while (_absoluteUris.Count != 0)
            {
                Thread.Sleep(1000);
            }
            timer.Stop();

            System.Console.WriteLine("\n[FINISH]: Crawling without HyperLink parsing or WebPage storage...");
            TimeSpan timeSpan1 = _stopwatch.Elapsed;
            System.Console.WriteLine("Elapsed: " + timeSpan1);
            System.Console.WriteLine("Press any key to continue.");
            System.Console.ReadLine();

            /**/

            _assignHyperLinkDiscoveries = true;
            _cacheToFileSystem = false;
            _saveWebPagesToDisk = false;
            timer.Start();
            System.Console.WriteLine("\n[START]: Crawling with HyperLink parsing but without WebPage storage...");
            _stopwatch.Reset();
            _stopwatch.Start();
            Crawl();

            while (_absoluteUris.Count != 0)
            {
                Thread.Sleep(1000);
            }
            timer.Stop();

            System.Console.WriteLine("\n[FINISH]: Crawling with HyperLink parsing but without WebPage storage...");
            TimeSpan timeSpan2 = _stopwatch.Elapsed;
            System.Console.WriteLine("Elapsed: " + timeSpan1);
            System.Console.WriteLine("Elapsed: " + timeSpan2);
            System.Console.WriteLine("Press any key to continue.");
            System.Console.ReadLine();

            /**/

            _assignHyperLinkDiscoveries = true;
            _cacheToFileSystem = false;
            _saveWebPagesToDisk = true;
            timer.Start();
            System.Console.WriteLine("\n[START]: Crawling with HyperLink parsing and WebPage storage...");
            _stopwatch.Reset();
            _stopwatch.Start();
            Crawl();

            while (_absoluteUris.Count != 0)
            {
                Thread.Sleep(1000);
            }
            timer.Stop();

            System.Console.WriteLine("\n[FINISH]: Crawling with HyperLink parsing and WebPage storage...");
            TimeSpan timeSpan3 = _stopwatch.Elapsed;
            System.Console.WriteLine("Elapsed: " + timeSpan1);
            System.Console.WriteLine("Elapsed: " + timeSpan2);
            System.Console.WriteLine("Elapsed: " + timeSpan3);
            System.Console.WriteLine("Press any key to continue.");
            System.Console.ReadLine();

            /**/

            System.Console.WriteLine("Here is where most free crawlers stop: Non-RAM based caching...\n");
            System.Console.WriteLine("As the AbsoluteUris were explicitly specified and each AbsoluteUri is checked for on disk (multiple times as each WebPage's HyperLinks intersect other WebPages' HyperLinks), this case represents a less than 'Best Case Scenario' for disk-backed RAM-based caching for this small crawl set, but approaches a 'Best Case Scenario' for a large crawl where the number of discovered AbsoluteUris greatly outnumbers the amount of available RAM needed to store those AbsoluteUris.\n");
            System.Console.WriteLine("Press any key to continue.");
            System.Console.ReadLine();

            _assignHyperLinkDiscoveries = true;
            _cacheToFileSystem = true;
            _saveWebPagesToDisk = true;
            timer.Start();
            System.Console.WriteLine("\n[START]: Crawling with HyperLink parsing and WebPage storage and filesystem caching...");
            _stopwatch.Reset();
            _stopwatch.Start();
            Crawl();

            while (_absoluteUris.Count != 0)
            {
                Thread.Sleep(1000);
            }
            timer.Stop();

            System.Console.WriteLine("\n[FINISH]: Crawling with HyperLink parsing and WebPage storage and filesystem caching...");
            TimeSpan timeSpan4 = _stopwatch.Elapsed;
            System.Console.WriteLine("Elapsed: " + timeSpan1);
            System.Console.WriteLine("Elapsed: " + timeSpan2);
            System.Console.WriteLine("Elapsed: " + timeSpan3);
            System.Console.WriteLine("Elapsed: " + timeSpan4);
            System.Console.WriteLine("Press any key to continue.");
            System.Console.ReadLine();

            /**/

            System.Console.WriteLine("When comparing crawlers 'Crawl Rate' is not an adequate measure of viability.  Many crawlers omit ");
        }

        private static void Crawl()
        {
            Regex hyperLinkAbsoluteUriRegex = new Regex("<\\s*(?<Tag>(a|base|form|frame))\\s*.*?(?<AttributeName>(action|href|src))\\s*=\\s*([\\\"\\'])(?<HyperLink>.*?)\\3", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            for (int i = 1; i <= 14606; i++)
            {
                _absoluteUris.Enqueue("http://localhost:56830/Test/" + i + ".htm");
            }

            for (int i = 0; i < _numberOfThreads; i++)
            {
                Thread thread = new Thread((o) =>
                                               {
                                                   int threadNumber = (int)o;

                                                   WebClient webClient = new WebClient();

                                                   while (_absoluteUris.Count != 0)
                                                   {
                                                       string absoluteUri = null;

                                                       lock (_queueLock)
                                                       {
                                                           if (_absoluteUris.Count != 0)
                                                           {
                                                               absoluteUri = _absoluteUris.Dequeue();
                                                           }
                                                       }

                                                       try
                                                       {
                                                           System.Console.WriteLine(absoluteUri + " :: " + threadNumber);

                                                           byte[] data = webClient.DownloadData(absoluteUri);

                                                           if(_assignHyperLinkDiscoveries)
                                                           {
                                                               //this is appropriate for our "English only" test site, on an "English installation" of Windows, but incorrect if a WebPage contains non-English characters... (Kanji/Cyrillic/etc...)
                                                               string decodedHtml = HttpUtility.HtmlDecode(Encoding.Default.GetString(data));

                                                               MatchCollection matchCollection = hyperLinkAbsoluteUriRegex.Matches(decodedHtml);

                                                               //when assigning HyperLinks, we'll need to be able to reference something other than RAM to determine whether a HyperLink is accounted for...
                                                               if(_cacheToFileSystem)
                                                               {
                                                                   foreach(Match match in matchCollection)
                                                                   {
                                                                       string tempFileName = Path.Combine(Path.GetTempPath(), Path.GetFileName(match.Groups["HyperLink"].Value));

                                                                       if(File.Exists(tempFileName))
                                                                       {
                                                                           
                                                                       }
                                                                   }
                                                               }
                                                           }

                                                           if(_saveWebPagesToDisk)
                                                           {
                                                               string tempFileName = Path.Combine(Path.GetTempPath(), Path.GetFileName(absoluteUri));

                                                               File.WriteAllBytes(tempFileName, data);
                                                           }
                                                       }
                                                       catch(Exception exception)
                                                       {
                                                           System.Console.ForegroundColor = ConsoleColor.Red;
                                                           System.Console.WriteLine(exception.Message);
                                                           System.Console.ForegroundColor = ConsoleColor.White;
                                                       }

                                                       lock (_queueLock)
                                                       {
                                                           _numberOfAbsoluteUrisDownloaded++;
                                                       }
                                                   }
                                               });

                thread.Start(i);
            }
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lock (_queueLock)
            {
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine((_numberOfAbsoluteUrisDownloaded) + " AbsoluteUris/s :: " + _absoluteUris.Count + " remaining.");
                System.Console.ForegroundColor = ConsoleColor.White;

                _numberOfAbsoluteUrisDownloaded = 0;
            }
        }
    }
}