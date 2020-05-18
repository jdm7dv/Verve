using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Arachnode.Plugins.CrawlActions;
using Arachnode.Proxy.Clients;

namespace Arachnode.Plugins.CrawlActions
{
    public class CustomHttpListener : HttpListener
    {
        protected override void httpClient_OnReceiveQueryEnd(Socket destination, HttpClient httpClient)
        {
            base.httpClient_OnReceiveQueryEnd(destination, httpClient);
        }
    }
}