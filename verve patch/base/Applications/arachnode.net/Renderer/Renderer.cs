#region License : arachnode.net

// Copyright (c) 2012 http://arachnode.net, arachnode.net, LLC
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
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Messaging;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Serialization.Formatters;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using Arachnode.DataAccess;
using Arachnode.Renderer.Value;
using Arachnode.Renderer.Value.Enums;
using Arachnode.Renderer.Value.EventArgs;
using AxSHDocVw;
using mshtml;
using SHDocVw;
using Message=System.Messaging.Message;
using Timer=System.Windows.Forms.Timer;

#endregion

namespace Arachnode.Renderer
{
    [ComVisible(true)]
    public partial class Renderer : Form, IOleClientSite, IDocHostShowUI
    {
        private readonly Queue _absoluteUris = new Queue();
        private readonly ArachnodeDAO _arachnodeDAO = new ArachnodeDAO();
        private readonly HtmlRenderer _htmlRenderer;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly Stopwatch _stopwatchTotal = new Stopwatch();
        private readonly Thread _thread;
        private bool _abortThread;
        private object _absoluteUri;
        private bool _debugMultipleAbsoluteUris;
        private bool _debugSingleAbsoluteUri;
        private MessageQueue _engineMessageQueue;
        private IHTMLDocument2 _htmlDocument2;
        private bool _modifyDOM;
        private int _numberOfRenderedAbsoluteUris;
        private RendererMessage _rendererMessage;
        private bool _showDebugHtml = true;
        private bool _useAxWebBrowser;

        public Renderer()
        {
            InitializeComponent();

            _stopwatchTotal.Reset();
            _stopwatchTotal.Start();

            _htmlRenderer = new HtmlRenderer(_arachnodeDAO);

            Closed += Renderer_Closed;

            if (_useAxWebBrowser && !DesignMode)
            {
                object o = axWebBrowser1.GetOcx();

                IOleObject oleObject = o as IOleObject;

                oleObject.SetClientSite(this);
            }

            axWebBrowser1.Silent = true;

            if (_useAxWebBrowser)
            {
                Thread thread = new Thread(() =>
                                               {
                                                   while (true)
                                                   {
                                                       Application.DoEvents();

                                                       Thread.Sleep(100);

                                                       if (_stopwatch.Elapsed.TotalMinutes > 1)
                                                       {
                                                           _stopwatch.Reset();
                                                           _stopwatch.Start();

                                                           axWebBrowser1.Stop();

                                                           axWebBrowser1_DocumentComplete(this, null);
                                                       }
                                                   }
                                               });

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }
            
            /**/

            _htmlRenderer.DocumentComplete += _htmlParser_DocumentComplete;

            /**/

            if (_debugSingleAbsoluteUri || _debugMultipleAbsoluteUris)
            {
                btnTest_Click(null, null);

                return;
            }

            /**/

            _thread = new Thread(delegate()
                                     {
                                         try
                                         {
                                             MessageQueue rendererMessageQueue = new MessageQueue(".\\private$\\Renderer_Renderers:" + 0);
                                             rendererMessageQueue.Formatter = new XmlMessageFormatter(new[] {typeof (RendererMessage)});

                                             while (rendererMessageQueue.Peek() == null)
                                             {
                                                 Thread.Sleep(10);
                                             }

                                             Message message = rendererMessageQueue.Receive();

                                             _rendererMessage = (RendererMessage) message.Body;

                                             /**/

                                             rendererMessageQueue = new MessageQueue(".\\private$\\Renderer_Renderers:" + _rendererMessage.ThreadNumber);
                                             rendererMessageQueue.Formatter = new XmlMessageFormatter(new[] {typeof (RendererMessage)});

                                             _engineMessageQueue = new MessageQueue(".\\private$\\Renderer_Engine:" + _rendererMessage.ThreadNumber);

                                             /**/

                                             //remoting code for Marshalling the HTMLDocumentClass...
                                             BinaryClientFormatterSinkProvider clientProvider = null;
                                             BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                                             serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

                                             Hashtable props = new Hashtable();
                                             props["name"] = "Renderer" + _rendererMessage.ThreadNumber;
                                             props["portName"] = "Renderer" + _rendererMessage.ThreadNumber;
                                             props["authorizedGroup"] = WindowsIdentity.GetCurrent().Name;
                                             //props["typeFilterLevel"] = TypeFilterLevel.Full;

                                             IpcChannel channel = new IpcChannel(props, clientProvider, serverProvider);

                                             ChannelServices.RegisterChannel(channel, false);

                                             RemotingConfiguration.RegisterWellKnownServiceType(typeof (Renderer), "Renderer" + _rendererMessage.ThreadNumber, WellKnownObjectMode.SingleCall);
                                             RemotingServices.Marshal(this, "Renderer" + _rendererMessage.ThreadNumber);

                                             /**/

                                             Text = ".\\private$\\Renderer_Engine:" + _rendererMessage.ThreadNumber + " Awaiting CrawlRequests...";

                                             while (true && !_abortThread)
                                             {
                                                 try
                                                 {
                                                     message = rendererMessageQueue.Receive();

                                                     _stopwatch.Reset();
                                                     _stopwatch.Start();

                                                     _rendererMessage = (RendererMessage) message.Body;
                                                     _htmlRenderer.CrawlRequestTimeoutInMinutes = _rendererMessage.CrawlRequestTimeoutInMinutes;

                                                     Text = DateTime.Now.ToLongTimeString() + " .\\private$\\Renderer_Engine:" + _rendererMessage.ThreadNumber + " " + _rendererMessage.AbsoluteUri + " Receive:" + _stopwatch.Elapsed.TotalSeconds;

                                                     if (!_rendererMessage.Kill)
                                                     {
                                                         switch (_rendererMessage.RenderAction)
                                                         {
                                                             case RenderAction.Render:
                                                                 if (_useAxWebBrowser)
                                                                 {
                                                                     object userAgent = "User-Agent: " + _rendererMessage.UserAgent;
                                                                     object o1 = null;
                                                                     object o2 = null;
                                                                     object o3 = null;
                                                                     DateTime startTime = DateTime.Now;

                                                                     axWebBrowser1.Navigate(_rendererMessage.AbsoluteUri, ref o1, ref o2, ref o3, ref userAgent);

                                                                     if (_modifyDOM)
                                                                     {
                                                                         bool wasDOMModified = false;

                                                                         while (axWebBrowser1.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE && DateTime.Now.Subtract(startTime).Duration().TotalMinutes < _rendererMessage.CrawlRequestTimeoutInMinutes)
                                                                         {
                                                                             Thread.Sleep(100);

                                                                             if (axWebBrowser1.ReadyState == tagREADYSTATE.READYSTATE_INTERACTIVE)
                                                                             {
                                                                                 if (!wasDOMModified)
                                                                                 {
                                                                                     _htmlRenderer.ModifyDOM((IHTMLDocument2) axWebBrowser1.Document, false);

                                                                                     wasDOMModified = true;
                                                                                 }
                                                                             }
                                                                         }
                                                                     }
                                                                 }
                                                                 else
                                                                 {
                                                                     _htmlRenderer.Render(_rendererMessage.AbsoluteUri);
                                                                 }
                                                                 break;
                                                             case RenderAction.Back:
                                                                 axWebBrowser1.GoBack();
                                                                 break;
                                                             case RenderAction.Forward:
                                                                 axWebBrowser1.GoForward();
                                                                 break;
                                                         }

                                                         try
                                                         {
                                                             foreach (Process process in Process.GetProcesses())
                                                             {
                                                                 if (process.ProcessName.ToLowerInvariant() == "iexplore" ||
                                                                     process.ProcessName.ToLowerInvariant() == "chrome" ||
                                                                     process.ProcessName.ToLowerInvariant() == "vsjitdebugger" ||
                                                                     process.MainWindowTitle.ToLowerInvariant() == "web browser" ||
                                                                     process.MainWindowTitle.ToLowerInvariant() == "renderer" ||
                                                                     process.MainWindowTitle.ToLowerInvariant() == "visual studio just-in-time debugger")
                                                                 {
                                                                     process.Kill();
                                                                 }
                                                             }

                                                             IntPtr window = WinApis.FindWindowByCaption(IntPtr.Zero, "Web Browser");

                                                             if (window != IntPtr.Zero)
                                                             {
                                                                 WinApis.CloseWindow(window);
                                                             }

                                                             window = WinApis.FindWindowByCaption(IntPtr.Zero, "Message from webpage");

                                                             if (window != IntPtr.Zero)
                                                             {
                                                                 WinApis.CloseWindow(window);
                                                             }
                                                         }
                                                         catch (Exception exception)
                                                         {
                                                             //MessageBox.Show(exception.Message);
                                                             //MessageBox.Show(exception.StackTrace);

                                                             _arachnodeDAO.InsertException(null, null, exception, false);
                                                         }
                                                     }
                                                     else
                                                     {
                                                         Process.GetCurrentProcess().Kill();
                                                     }
                                                 }
                                                 catch (Exception exception)
                                                 {
                                                     //MessageBox.Show(exception.Message);
                                                     //MessageBox.Show(exception.StackTrace);

                                                     _arachnodeDAO.InsertException(null, null, exception, false);
                                                 }
                                             }
                                         }
                                         catch (Exception exception)
                                         {
                                             //MessageBox.Show(exception.Message);
                                             //MessageBox.Show(exception.StackTrace);

                                             _arachnodeDAO.InsertException(null, null, exception, false);
                                         }
                                     });

            _thread.Start();

            /**/

            Timer timer = new Timer();

            timer.Enabled = true;
            timer.Interval = 5000;
            timer.Tick += timer_Tick;

            timer.Start();
        }

        public HTMLDocumentClass HtmlDocumentClass { get; set; }

        #region IOleClientSite Members

        int IOleClientSite.SaveObject()
        {
            return 0;
        }

        int IOleClientSite.GetMoniker(uint dwAssign, uint dwWhichMoniker, out Object ppmk)
        {
            ppmk = null;
            return 0;
        }

        int IOleClientSite.GetContainer(out IOleContainer ppContainer)
        {
            ppContainer = null;
            return 0;
        }

        int IOleClientSite.ShowObject()
        {
            return 0;
        }

        int IOleClientSite.OnShowWindow(int fShow)
        {
            return 0;
        }

        int IOleClientSite.RequestNewObjectLayout()
        {
            return 0;
        }

        #endregion

        #region Implementation of IDocHostShowUI

        public int ShowMessage(IntPtr hwnd, string lpstrText, string lpstrCaption, uint dwType, string lpstrHelpFile, uint dwHelpContext, ref int lpResult)
        {
            return 0;
        }

        public int ShowHelp(IntPtr hwnd, string pszHelpFile, uint uCommand, uint dwData, tagPOINT ptMouse, object pDispatchObjectHit)
        {
            return 0;
        }

        #endregion

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (_debugSingleAbsoluteUri || _debugMultipleAbsoluteUris)
            {
                _absoluteUri = "http://google.com";

                if (_debugMultipleAbsoluteUris)
                {
                    foreach (string absoluteUri2 in File.ReadAllLines("TestData\\AbsoluteUris_Renderer.txt"))
                    {
                        _absoluteUris.Enqueue(absoluteUri2);
                    }

                    _absoluteUri = _absoluteUris.Dequeue().ToString();
                }

                _stopwatch.Reset();
                _stopwatch.Start();

                if (_useAxWebBrowser)
                {
                    BeginInvoke(new MethodInvoker(delegate
                                                      {
                                                          axWebBrowser1.Navigate2(ref _absoluteUri);

                                                          if (_modifyDOM)
                                                          {
                                                              bool wasDOMModified = false;

                                                              while (axWebBrowser1.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE)
                                                              {
                                                                  Application.DoEvents();

                                                                  Thread.Sleep(100);

                                                                  if (axWebBrowser1.ReadyState == tagREADYSTATE.READYSTATE_INTERACTIVE)
                                                                  {
                                                                      if (!wasDOMModified)
                                                                      {
                                                                          _htmlRenderer.ModifyDOM((IHTMLDocument2) axWebBrowser1.Document, false);

                                                                          wasDOMModified = true;
                                                                      }
                                                                  }
                                                              }
                                                          }
                                                      }));
                }
                else
                {
                    _htmlRenderer.Render(_absoluteUri.ToString());
                }

                Text = _absoluteUri.ToString();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                bool wasAnArachnodeProcessFound = false;

                foreach (Process process in Process.GetProcesses())
                {
                    if (process.ProcessName.ToLowerInvariant().Contains("arachnode") && !process.ProcessName.ToLowerInvariant().Contains("arachnode.renderer"))
                    {
                        wasAnArachnodeProcessFound = true;
                    }
                }

                if (!wasAnArachnodeProcessFound)
                {
                    Application.Exit();

                    Process.GetCurrentProcess().Kill();
                }
            }
            catch (Exception exception)
            {
                _arachnodeDAO.InsertException(null, null, exception, false);
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        private void Renderer_Closed(object sender, EventArgs e)
        {
            _abortThread = true;

            Process.GetCurrentProcess().Kill();
        }

        //ANODET: Used from AN.Next.  Upgrade AN.Next.
        public HTMLDocumentClass Render(string absoluteUri, RenderAction renderAction, RenderType renderType, string userAgent, double crawlRequestTimeoutInMinutes)
        {
            return (HTMLDocumentClass) Invoke(new RenderHandler(Render2), absoluteUri, renderAction, renderType, userAgent, crawlRequestTimeoutInMinutes);
        }

        private HTMLDocumentClass Render2(string absoluteUri, RenderAction renderAction, RenderType renderType, string userAgent, double crawlRequestTimeoutInMinutes)
        {
            switch (renderAction)
            {
                case RenderAction.None:
                    break;
                case RenderAction.Render:
                    if (_useAxWebBrowser)
                    {
                        object userAgent2 = "User-Agent: " + userAgent;
                        object o1 = null;
                        object o2 = null;
                        object o3 = null;
                        DateTime startTime = DateTime.Now;

                        axWebBrowser1.Navigate(absoluteUri, ref o1, ref o2, ref o3, ref userAgent2);

                        if (_modifyDOM)
                        {
                            bool wasDOMModified = false;

                            while (axWebBrowser1.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE && DateTime.Now.Subtract(startTime).Duration().TotalMinutes < crawlRequestTimeoutInMinutes)
                            {
                                Thread.Sleep(100);

                                if (axWebBrowser1.ReadyState == tagREADYSTATE.READYSTATE_INTERACTIVE)
                                {
                                    if (!wasDOMModified)
                                    {
                                        _htmlRenderer.ModifyDOM((IHTMLDocument2) axWebBrowser1.Document, false);

                                        wasDOMModified = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        _htmlRenderer.Render(absoluteUri);

                        while (((IHTMLDocument2)_htmlRenderer.MSHTML).readyState.ToLowerInvariant() != "complete")
                        {
                            Thread.Sleep(100);
                        }
                    }
                    break;
                case RenderAction.Back:
                    axWebBrowser1.GoBack();
                    break;
                case RenderAction.Forward:
                    axWebBrowser1.GoForward();
                    break;
            }

            if (_useAxWebBrowser)
            {
                return (HTMLDocumentClass) axWebBrowser1.Document;
            }

            return (HTMLDocumentClass) _htmlRenderer.MSHTML;
        }

        [DispId(-5512)]
        public virtual int IDispatch_Invoke_Handler()
        {
            return (int) (BrowserOptions.DownloadOnly | BrowserOptions.Silent | BrowserOptions.NoActiveXDownload | BrowserOptions.DontRunActiveX | BrowserOptions.NoClientPull | BrowserOptions.NoFrameDownload | BrowserOptions.NoJava | 0);
        }

        private void axWebBrowser1_NavigateError(object sender, DWebBrowserEvents2_NavigateErrorEvent e)
        {
            e.cancel = true;
        }

        private void axWebBrowser1_FileDownload(object sender, DWebBrowserEvents2_FileDownloadEvent e)
        {
            e.cancel = true;
        }

        private void axWebBrowser1_NewWindow2(object sender, DWebBrowserEvents2_NewWindow2Event e)
        {
            e.cancel = true;
        }

        private void axWebBrowser1_NewWindow3(object sender, DWebBrowserEvents2_NewWindow3Event e)
        {
            e.cancel = true;
        }

        private void axWebBrowser1_NavigateComplete2(object sender, DWebBrowserEvents2_NavigateComplete2Event e)
        {
            _htmlDocument2 = (IHTMLDocument2) axWebBrowser1.Document;

            HTMLWindowEvents2_Event onErrorEvent = (HTMLWindowEvents2_Event) _htmlDocument2.parentWindow;

            onErrorEvent.onerror += myHTMLWindowEvents2_onerror;
        }

        private void axWebBrowser1_DocumentComplete(object sender, DWebBrowserEvents2_DocumentCompleteEvent e)
        {
            _htmlDocument2 = (IHTMLDocument2) axWebBrowser1.Document;

            HTMLWindowEvents2_Event onErrorEvent = (HTMLWindowEvents2_Event) _htmlDocument2.parentWindow;

            onErrorEvent.onerror += myHTMLWindowEvents2_onerror;

            /**/

            try
            {
                if (_rendererMessage != null)
                {
                    Text += " Render:" + _stopwatch.Elapsed.TotalSeconds;

                    HtmlDocumentClass = (HTMLDocumentClass) axWebBrowser1.Document;
                    //turn off 'Disable Script Debugging (Other)' in 'Advanced > Settings > Browsing' in IE if you encounter script errors...

                    _rendererMessage.RenderTime = _stopwatch.Elapsed;

                    _engineMessageQueue.Send(_rendererMessage);

                    Text += " Send:" + _stopwatch.Elapsed.TotalSeconds;
                }

                if (_debugSingleAbsoluteUri || _debugMultipleAbsoluteUris)
                {
                    BeginInvoke(new MethodInvoker(delegate
                                                      {
                                                          Text = _absoluteUri + " : " + _stopwatch.Elapsed;
                                                          tsslWorkingSet.Text = (Environment.WorkingSet/1024).ToString("###,###,###,###") + " KB";
                                                          tsslNumberOfRenderedAbsoluteUris.Text = (++_numberOfRenderedAbsoluteUris).ToString();
                                                          tsslElapsedTime.Text = _stopwatchTotal.Elapsed.ToString();

                                                          _stopwatch.Reset();
                                                          _stopwatch.Start();

                                                          if (axWebBrowser1.Document != null && ((HTMLDocumentClass) axWebBrowser1.Document).body != null)
                                                          {
                                                              rtbDebug.Text = ((HTMLDocumentClass) axWebBrowser1.Document).body.innerHTML;
                                                          }

                                                          if (_absoluteUris.Count != 0)
                                                          {
                                                              _absoluteUri = _absoluteUris.Dequeue().ToString();

                                                              axWebBrowser1.Navigate2(ref _absoluteUri);
                                                          }
                                                          else
                                                          {
                                                              _debugSingleAbsoluteUri = false;
                                                              _debugMultipleAbsoluteUris = false;

                                                              Text += " : Done";
                                                          }
                                                      }));

                    try
                    {
                        foreach (Process process in Process.GetProcesses())
                        {
                            if (process.ProcessName.ToLowerInvariant() == "iexplore" ||
                                process.ProcessName.ToLowerInvariant() == "chrome" ||
                                process.ProcessName.ToLowerInvariant() == "vsjitdebugger" ||
                                process.MainWindowTitle.ToLowerInvariant() == "web browser" ||
                                process.MainWindowTitle.ToLowerInvariant() == "renderer" ||
                                process.MainWindowTitle.ToLowerInvariant() == "visual studio just-in-time debugger")
                            {
                                process.Kill();
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        //MessageBox.Show(exception.Message);
                        //MessageBox.Show(exception.StackTrace);

                        _arachnodeDAO.InsertException(null, null, exception, false);
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());

                _arachnodeDAO.InsertException(null, null, exception, false);
            }
        }

        private void _htmlParser_DocumentComplete(object sender, DocumentCompleteEventArgs e)
        {
            try
            {
                if (_rendererMessage != null)
                {
                    Text += " Render:" + _stopwatch.Elapsed.TotalSeconds;
                    tsslWorkingSet.Text = (Environment.WorkingSet/1024).ToString("###,###,###,###") + " KB";
                    tsslNumberOfRenderedAbsoluteUris.Text = (++_numberOfRenderedAbsoluteUris).ToString();
                    tsslElapsedTime.Text = _stopwatchTotal.Elapsed.ToString();

                    if (_showDebugHtml)
                    {
                        if (e.HtmlDocument != null && e.HtmlDocument.body != null)
                        {
                            rtbDebug.Text = _htmlRenderer.AbsoluteUri + " :: " + ((HTMLDocumentClass) e.HtmlDocument).body.innerHTML;
                        }
                    }

                    HtmlDocumentClass = (HTMLDocumentClass) e.HtmlDocument;

                    _rendererMessage.RenderTime = _stopwatch.Elapsed;

                    _engineMessageQueue.Send(_rendererMessage);
                }

                if (_debugSingleAbsoluteUri || _debugMultipleAbsoluteUris)
                {
                    BeginInvoke(new MethodInvoker(delegate
                                                      {
                                                          Text = _htmlRenderer.AbsoluteUri + " : " + _stopwatch.Elapsed;
                                                          tsslWorkingSet.Text = (Environment.WorkingSet/1024).ToString("###,###,###,###") + " KB";
                                                          tsslNumberOfRenderedAbsoluteUris.Text = (++_numberOfRenderedAbsoluteUris).ToString();

                                                          _stopwatch.Reset();
                                                          _stopwatch.Start();

                                                          if (e.HtmlDocument != null && e.HtmlDocument.body != null)
                                                          {
                                                              rtbDebug.Text = ((HTMLDocumentClass) e.HtmlDocument).body.innerHTML;
                                                          }

                                                          if (_absoluteUris.Count != 0)
                                                          {
                                                              string absoluteUri = _absoluteUris.Dequeue().ToString();

                                                              _htmlRenderer.Render(absoluteUri);
                                                          }
                                                          else
                                                          {
                                                              _debugSingleAbsoluteUri = false;
                                                              _debugMultipleAbsoluteUris = false;

                                                              Text += " : Done";
                                                          }
                                                      }));

                    try
                    {
                        foreach (Process process in Process.GetProcesses())
                        {
                            if (process.ProcessName.ToLowerInvariant() == "iexplore" ||
                                process.ProcessName.ToLowerInvariant() == "chrome" ||
                                process.ProcessName.ToLowerInvariant() == "vsjitdebugger" ||
                                process.MainWindowTitle.ToLowerInvariant() == "web browser" ||
                                process.MainWindowTitle.ToLowerInvariant() == "renderer" ||
                                process.MainWindowTitle.ToLowerInvariant() == "visual studio just-in-time debugger")
                            {
                                process.Kill();
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        //MessageBox.Show(exception.Message);
                        //MessageBox.Show(exception.StackTrace);

                        _arachnodeDAO.InsertException(null, null, exception, false);
                    }
                }
            }
            catch (Exception exception)
            {
                //MessageBox.Show(exception.Message);
                //MessageBox.Show(exception.StackTrace);

                _arachnodeDAO.InsertException(null, null, exception, false);
            }
        }

        private void myHTMLWindowEvents2_onerror(string description, string url, int line)
        {
            IHTMLEventObj eventObj = _htmlDocument2.parentWindow.@event;

            if (eventObj != null)
            {
                eventObj.returnValue = true;
            }
        }

        private void axWebBrowser1_ThirdPartyUrlBlocked(object sender, DWebBrowserEvents2_ThirdPartyUrlBlockedEvent e)
        {
            MessageBox.Show("axWebBrowser1_ThirdPartyUrlBlocked");
            MessageBox.Show(e.uRL.ToString());
        }

        private void axWebBrowser1_RedirectXDomainBlocked(object sender, DWebBrowserEvents2_RedirectXDomainBlockedEvent e)
        {
            MessageBox.Show("axWebBrowser1_RedirectXDomainBlocked");
            MessageBox.Show(e.startURL.ToString());
        }

        #region Nested type: RenderHandler

        private delegate HTMLDocumentClass RenderHandler(string absoluteUri, RenderAction renderAction, RenderType renderType, string userAgent, double crawlRequestTimeoutInMinutes);

        #endregion
    }
}