// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
namespace Bio.Web.Blast {
    using System.Diagnostics;
    using System.Web.Services;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WSWUBlastSoapBinding", Namespace="http://www.ebi.ac.uk/WSWUBlast")]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(outData))]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(WSFile))]
    [System.Xml.Serialization.SoapIncludeAttribute(typeof(data))]
    public partial class WSWUBlastService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback blastpOperationCompleted;
        
        private System.Threading.SendOrPostCallback blastnOperationCompleted;
        
        private System.Threading.SendOrPostCallback getOutputOperationCompleted;
        
        private System.Threading.SendOrPostCallback getXMLOperationCompleted;
        
        private System.Threading.SendOrPostCallback runWUBlastOperationCompleted;
        
        private System.Threading.SendOrPostCallback checkStatusOperationCompleted;
        
        private System.Threading.SendOrPostCallback pollOperationCompleted;
        
        private System.Threading.SendOrPostCallback getResultsOperationCompleted;
        
        private System.Threading.SendOrPostCallback getIdsOperationCompleted;
        
        private System.Threading.SendOrPostCallback getMatricesOperationCompleted;
        
        private System.Threading.SendOrPostCallback getProgramsOperationCompleted;
        
        private System.Threading.SendOrPostCallback getDatabasesOperationCompleted;
        
        private System.Threading.SendOrPostCallback getSortOperationCompleted;
        
        private System.Threading.SendOrPostCallback getStatsOperationCompleted;
        
        private System.Threading.SendOrPostCallback getXmlFormatsOperationCompleted;
        
        private System.Threading.SendOrPostCallback getSensitivityOperationCompleted;
        
        private System.Threading.SendOrPostCallback getFiltersOperationCompleted;
        
        private System.Threading.SendOrPostCallback polljobOperationCompleted;
        
        private System.Threading.SendOrPostCallback doWUBlastOperationCompleted;
        
        /// <remarks/>
        public WSWUBlastService() {
            this.Url = "http://www.ebi.ac.uk/Tools/es/ws-servers/WSWUBlast";
        }
        
        /// <remarks/>
        public event blastpCompletedEventHandler blastpCompleted;
        
        /// <remarks/>
        public event blastnCompletedEventHandler blastnCompleted;
        
        /// <remarks/>
        public event getOutputCompletedEventHandler getOutputCompleted;
        
        /// <remarks/>
        public event getXMLCompletedEventHandler getXMLCompleted;
        
        /// <remarks/>
        public event runWUBlastCompletedEventHandler runWUBlastCompleted;
        
        /// <remarks/>
        public event checkStatusCompletedEventHandler checkStatusCompleted;
        
        /// <remarks/>
        public event pollCompletedEventHandler pollCompleted;
        
        /// <remarks/>
        public event getResultsCompletedEventHandler getResultsCompleted;
        
        /// <remarks/>
        public event getIdsCompletedEventHandler getIdsCompleted;
        
        /// <remarks/>
        public event getMatricesCompletedEventHandler getMatricesCompleted;
        
        /// <remarks/>
        public event getProgramsCompletedEventHandler getProgramsCompleted;
        
        /// <remarks/>
        public event getDatabasesCompletedEventHandler getDatabasesCompleted;
        
        /// <remarks/>
        public event getSortCompletedEventHandler getSortCompleted;
        
        /// <remarks/>
        public event getStatsCompletedEventHandler getStatsCompleted;
        
        /// <remarks/>
        public event getXmlFormatsCompletedEventHandler getXmlFormatsCompleted;
        
        /// <remarks/>
        public event getSensitivityCompletedEventHandler getSensitivityCompleted;
        
        /// <remarks/>
        public event getFiltersCompletedEventHandler getFiltersCompleted;
        
        /// <remarks/>
        public event polljobCompletedEventHandler polljobCompleted;
        
        /// <remarks/>
        public event doWUBlastCompletedEventHandler doWUBlastCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#blastp", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("jobid")]
        public string blastp(string database, string sequence, string email) {
            object[] results = this.Invoke("blastp", new object[] {
                        database,
                        sequence,
                        email});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult Beginblastp(string database, string sequence, string email, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("blastp", new object[] {
                        database,
                        sequence,
                        email}, callback, asyncState);
        }
        
        /// <remarks/>
        public string Endblastp(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void blastpAsync(string database, string sequence, string email) {
            this.blastpAsync(database, sequence, email, null);
        }
        
        /// <remarks/>
        public void blastpAsync(string database, string sequence, string email, object userState) {
            if ((this.blastpOperationCompleted == null)) {
                this.blastpOperationCompleted = new System.Threading.SendOrPostCallback(this.OnblastpOperationCompleted);
            }
            this.InvokeAsync("blastp", new object[] {
                        database,
                        sequence,
                        email}, this.blastpOperationCompleted, userState);
        }
        
        private void OnblastpOperationCompleted(object arg) {
            if ((this.blastpCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.blastpCompleted(this, new blastpCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#blastn", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("jobid")]
        public string blastn(string database, string sequence, string email) {
            object[] results = this.Invoke("blastn", new object[] {
                        database,
                        sequence,
                        email});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult Beginblastn(string database, string sequence, string email, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("blastn", new object[] {
                        database,
                        sequence,
                        email}, callback, asyncState);
        }
        
        /// <remarks/>
        public string Endblastn(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void blastnAsync(string database, string sequence, string email) {
            this.blastnAsync(database, sequence, email, null);
        }
        
        /// <remarks/>
        public void blastnAsync(string database, string sequence, string email, object userState) {
            if ((this.blastnOperationCompleted == null)) {
                this.blastnOperationCompleted = new System.Threading.SendOrPostCallback(this.OnblastnOperationCompleted);
            }
            this.InvokeAsync("blastn", new object[] {
                        database,
                        sequence,
                        email}, this.blastnOperationCompleted, userState);
        }
        
        private void OnblastnOperationCompleted(object arg) {
            if ((this.blastnCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.blastnCompleted(this, new blastnCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#getOutput", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public string getOutput(string jobid) {
            object[] results = this.Invoke("getOutput", new object[] {
                        jobid});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetOutput(string jobid, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getOutput", new object[] {
                        jobid}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndgetOutput(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getOutputAsync(string jobid) {
            this.getOutputAsync(jobid, null);
        }
        
        /// <remarks/>
        public void getOutputAsync(string jobid, object userState) {
            if ((this.getOutputOperationCompleted == null)) {
                this.getOutputOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetOutputOperationCompleted);
            }
            this.InvokeAsync("getOutput", new object[] {
                        jobid}, this.getOutputOperationCompleted, userState);
        }
        
        private void OngetOutputOperationCompleted(object arg) {
            if ((this.getOutputCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getOutputCompleted(this, new getOutputCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#getXML", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public string getXML(string jobid) {
            object[] results = this.Invoke("getXML", new object[] {
                        jobid});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetXML(string jobid, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getXML", new object[] {
                        jobid}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndgetXML(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void getXMLAsync(string jobid) {
            this.getXMLAsync(jobid, null);
        }
        
        /// <remarks/>
        public void getXMLAsync(string jobid, object userState) {
            if ((this.getXMLOperationCompleted == null)) {
                this.getXMLOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetXMLOperationCompleted);
            }
            this.InvokeAsync("getXML", new object[] {
                        jobid}, this.getXMLOperationCompleted, userState);
        }
        
        private void OngetXMLOperationCompleted(object arg) {
            if ((this.getXMLCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getXMLCompleted(this, new getXMLCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#runWUBlast", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("jobid")]
        public string runWUBlast(inputParams @params, data[] content) {
            object[] results = this.Invoke("runWUBlast", new object[] {
                        @params,
                        content});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginrunWUBlast(inputParams @params, data[] content, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("runWUBlast", new object[] {
                        @params,
                        content}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndrunWUBlast(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void runWUBlastAsync(inputParams @params, data[] content) {
            this.runWUBlastAsync(@params, content, null);
        }
        
        /// <remarks/>
        public void runWUBlastAsync(inputParams @params, data[] content, object userState) {
            if ((this.runWUBlastOperationCompleted == null)) {
                this.runWUBlastOperationCompleted = new System.Threading.SendOrPostCallback(this.OnrunWUBlastOperationCompleted);
            }
            this.InvokeAsync("runWUBlast", new object[] {
                        @params,
                        content}, this.runWUBlastOperationCompleted, userState);
        }
        
        private void OnrunWUBlastOperationCompleted(object arg) {
            if ((this.runWUBlastCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.runWUBlastCompleted(this, new runWUBlastCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#checkStatus", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("status")]
        public string checkStatus(string jobid) {
            object[] results = this.Invoke("checkStatus", new object[] {
                        jobid});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegincheckStatus(string jobid, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("checkStatus", new object[] {
                        jobid}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndcheckStatus(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void checkStatusAsync(string jobid) {
            this.checkStatusAsync(jobid, null);
        }
        
        /// <remarks/>
        public void checkStatusAsync(string jobid, object userState) {
            if ((this.checkStatusOperationCompleted == null)) {
                this.checkStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OncheckStatusOperationCompleted);
            }
            this.InvokeAsync("checkStatus", new object[] {
                        jobid}, this.checkStatusOperationCompleted, userState);
        }
        
        private void OncheckStatusOperationCompleted(object arg) {
            if ((this.checkStatusCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.checkStatusCompleted(this, new checkStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#poll", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result", DataType="base64Binary")]
        public byte[] poll(string jobid, string type) {
            object[] results = this.Invoke("poll", new object[] {
                        jobid,
                        type});
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult Beginpoll(string jobid, string type, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("poll", new object[] {
                        jobid,
                        type}, callback, asyncState);
        }
        
        /// <remarks/>
        public byte[] Endpoll(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void pollAsync(string jobid, string type) {
            this.pollAsync(jobid, type, null);
        }
        
        /// <remarks/>
        public void pollAsync(string jobid, string type, object userState) {
            if ((this.pollOperationCompleted == null)) {
                this.pollOperationCompleted = new System.Threading.SendOrPostCallback(this.OnpollOperationCompleted);
            }
            this.InvokeAsync("poll", new object[] {
                        jobid,
                        type}, this.pollOperationCompleted, userState);
        }
        
        private void OnpollOperationCompleted(object arg) {
            if ((this.pollCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.pollCompleted(this, new pollCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#getResults", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("results")]
        public WSFile[] getResults(string jobid) {
            object[] results = this.Invoke("getResults", new object[] {
                        jobid});
            return ((WSFile[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetResults(string jobid, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getResults", new object[] {
                        jobid}, callback, asyncState);
        }
        
        /// <remarks/>
        public WSFile[] EndgetResults(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((WSFile[])(results[0]));
        }
        
        /// <remarks/>
        public void getResultsAsync(string jobid) {
            this.getResultsAsync(jobid, null);
        }
        
        /// <remarks/>
        public void getResultsAsync(string jobid, object userState) {
            if ((this.getResultsOperationCompleted == null)) {
                this.getResultsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetResultsOperationCompleted);
            }
            this.InvokeAsync("getResults", new object[] {
                        jobid}, this.getResultsOperationCompleted, userState);
        }
        
        private void OngetResultsOperationCompleted(object arg) {
            if ((this.getResultsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getResultsCompleted(this, new getResultsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#getIds", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public string[] getIds(string jobid) {
            object[] results = this.Invoke("getIds", new object[] {
                        jobid});
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetIds(string jobid, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getIds", new object[] {
                        jobid}, callback, asyncState);
        }
        
        /// <remarks/>
        public string[] EndgetIds(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string[])(results[0]));
        }
        
        /// <remarks/>
        public void getIdsAsync(string jobid) {
            this.getIdsAsync(jobid, null);
        }
        
        /// <remarks/>
        public void getIdsAsync(string jobid, object userState) {
            if ((this.getIdsOperationCompleted == null)) {
                this.getIdsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetIdsOperationCompleted);
            }
            this.InvokeAsync("getIds", new object[] {
                        jobid}, this.getIdsOperationCompleted, userState);
        }
        
        private void OngetIdsOperationCompleted(object arg) {
            if ((this.getIdsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getIdsCompleted(this, new getIdsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#getMatrices", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public outData[] getMatrices() {
            object[] results = this.Invoke("getMatrices", new object[0]);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetMatrices(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getMatrices", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public outData[] EndgetMatrices(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public void getMatricesAsync() {
            this.getMatricesAsync(null);
        }
        
        /// <remarks/>
        public void getMatricesAsync(object userState) {
            if ((this.getMatricesOperationCompleted == null)) {
                this.getMatricesOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetMatricesOperationCompleted);
            }
            this.InvokeAsync("getMatrices", new object[0], this.getMatricesOperationCompleted, userState);
        }
        
        private void OngetMatricesOperationCompleted(object arg) {
            if ((this.getMatricesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getMatricesCompleted(this, new getMatricesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#getPrograms", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public outData[] getPrograms() {
            object[] results = this.Invoke("getPrograms", new object[0]);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetPrograms(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getPrograms", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public outData[] EndgetPrograms(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public void getProgramsAsync() {
            this.getProgramsAsync(null);
        }
        
        /// <remarks/>
        public void getProgramsAsync(object userState) {
            if ((this.getProgramsOperationCompleted == null)) {
                this.getProgramsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetProgramsOperationCompleted);
            }
            this.InvokeAsync("getPrograms", new object[0], this.getProgramsOperationCompleted, userState);
        }
        
        private void OngetProgramsOperationCompleted(object arg) {
            if ((this.getProgramsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getProgramsCompleted(this, new getProgramsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#getDatabases", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public outData[] getDatabases() {
            object[] results = this.Invoke("getDatabases", new object[0]);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetDatabases(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getDatabases", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public outData[] EndgetDatabases(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public void getDatabasesAsync() {
            this.getDatabasesAsync(null);
        }
        
        /// <remarks/>
        public void getDatabasesAsync(object userState) {
            if ((this.getDatabasesOperationCompleted == null)) {
                this.getDatabasesOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetDatabasesOperationCompleted);
            }
            this.InvokeAsync("getDatabases", new object[0], this.getDatabasesOperationCompleted, userState);
        }
        
        private void OngetDatabasesOperationCompleted(object arg) {
            if ((this.getDatabasesCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getDatabasesCompleted(this, new getDatabasesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#getSort", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public outData[] getSort() {
            object[] results = this.Invoke("getSort", new object[0]);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetSort(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getSort", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public outData[] EndgetSort(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public void getSortAsync() {
            this.getSortAsync(null);
        }
        
        /// <remarks/>
        public void getSortAsync(object userState) {
            if ((this.getSortOperationCompleted == null)) {
                this.getSortOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetSortOperationCompleted);
            }
            this.InvokeAsync("getSort", new object[0], this.getSortOperationCompleted, userState);
        }
        
        private void OngetSortOperationCompleted(object arg) {
            if ((this.getSortCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getSortCompleted(this, new getSortCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#getStats", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public outData[] getStats() {
            object[] results = this.Invoke("getStats", new object[0]);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetStats(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getStats", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public outData[] EndgetStats(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public void getStatsAsync() {
            this.getStatsAsync(null);
        }
        
        /// <remarks/>
        public void getStatsAsync(object userState) {
            if ((this.getStatsOperationCompleted == null)) {
                this.getStatsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetStatsOperationCompleted);
            }
            this.InvokeAsync("getStats", new object[0], this.getStatsOperationCompleted, userState);
        }
        
        private void OngetStatsOperationCompleted(object arg) {
            if ((this.getStatsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getStatsCompleted(this, new getStatsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#getXmlFormats", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public outData[] getXmlFormats() {
            object[] results = this.Invoke("getXmlFormats", new object[0]);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetXmlFormats(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getXmlFormats", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public outData[] EndgetXmlFormats(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public void getXmlFormatsAsync() {
            this.getXmlFormatsAsync(null);
        }
        
        /// <remarks/>
        public void getXmlFormatsAsync(object userState) {
            if ((this.getXmlFormatsOperationCompleted == null)) {
                this.getXmlFormatsOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetXmlFormatsOperationCompleted);
            }
            this.InvokeAsync("getXmlFormats", new object[0], this.getXmlFormatsOperationCompleted, userState);
        }
        
        private void OngetXmlFormatsOperationCompleted(object arg) {
            if ((this.getXmlFormatsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getXmlFormatsCompleted(this, new getXmlFormatsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#getSensitivity", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public outData[] getSensitivity() {
            object[] results = this.Invoke("getSensitivity", new object[0]);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetSensitivity(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getSensitivity", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public outData[] EndgetSensitivity(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public void getSensitivityAsync() {
            this.getSensitivityAsync(null);
        }
        
        /// <remarks/>
        public void getSensitivityAsync(object userState) {
            if ((this.getSensitivityOperationCompleted == null)) {
                this.getSensitivityOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetSensitivityOperationCompleted);
            }
            this.InvokeAsync("getSensitivity", new object[0], this.getSensitivityOperationCompleted, userState);
        }
        
        private void OngetSensitivityOperationCompleted(object arg) {
            if ((this.getSensitivityCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getSensitivityCompleted(this, new getSensitivityCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#getFilters", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result")]
        public outData[] getFilters() {
            object[] results = this.Invoke("getFilters", new object[0]);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegingetFilters(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("getFilters", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public outData[] EndgetFilters(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((outData[])(results[0]));
        }
        
        /// <remarks/>
        public void getFiltersAsync() {
            this.getFiltersAsync(null);
        }
        
        /// <remarks/>
        public void getFiltersAsync(object userState) {
            if ((this.getFiltersOperationCompleted == null)) {
                this.getFiltersOperationCompleted = new System.Threading.SendOrPostCallback(this.OngetFiltersOperationCompleted);
            }
            this.InvokeAsync("getFilters", new object[0], this.getFiltersOperationCompleted, userState);
        }
        
        private void OngetFiltersOperationCompleted(object arg) {
            if ((this.getFiltersCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.getFiltersCompleted(this, new getFiltersCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#polljob", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result", DataType="base64Binary")]
        public byte[] polljob(string jobid, string outformat) {
            object[] results = this.Invoke("polljob", new object[] {
                        jobid,
                        outformat});
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult Beginpolljob(string jobid, string outformat, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("polljob", new object[] {
                        jobid,
                        outformat}, callback, asyncState);
        }
        
        /// <remarks/>
        public byte[] Endpolljob(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void polljobAsync(string jobid, string outformat) {
            this.polljobAsync(jobid, outformat, null);
        }
        
        /// <remarks/>
        public void polljobAsync(string jobid, string outformat, object userState) {
            if ((this.polljobOperationCompleted == null)) {
                this.polljobOperationCompleted = new System.Threading.SendOrPostCallback(this.OnpolljobOperationCompleted);
            }
            this.InvokeAsync("polljob", new object[] {
                        jobid,
                        outformat}, this.polljobOperationCompleted, userState);
        }
        
        private void OnpolljobOperationCompleted(object arg) {
            if ((this.polljobCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.polljobCompleted(this, new polljobCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapRpcMethodAttribute("http://www.ebi.ac.uk/WSWUBlast#doWUBlast", RequestNamespace="http://www.ebi.ac.uk/WSWUBlast", ResponseNamespace="http://www.ebi.ac.uk/WSWUBlast")]
        [return: System.Xml.Serialization.SoapElementAttribute("result", DataType="base64Binary")]
        public byte[] doWUBlast(inputParams @params, [System.Xml.Serialization.SoapElementAttribute(DataType="base64Binary")] byte[] content) {
            object[] results = this.Invoke("doWUBlast", new object[] {
                        @params,
                        content});
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BegindoWUBlast(inputParams @params, byte[] content, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("doWUBlast", new object[] {
                        @params,
                        content}, callback, asyncState);
        }
        
        /// <remarks/>
        public byte[] EnddoWUBlast(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((byte[])(results[0]));
        }
        
        /// <remarks/>
        public void doWUBlastAsync(inputParams @params, byte[] content) {
            this.doWUBlastAsync(@params, content, null);
        }
        
        /// <remarks/>
        public void doWUBlastAsync(inputParams @params, byte[] content, object userState) {
            if ((this.doWUBlastOperationCompleted == null)) {
                this.doWUBlastOperationCompleted = new System.Threading.SendOrPostCallback(this.OndoWUBlastOperationCompleted);
            }
            this.InvokeAsync("doWUBlast", new object[] {
                        @params,
                        content}, this.doWUBlastOperationCompleted, userState);
        }
        
        private void OndoWUBlastOperationCompleted(object arg) {
            if ((this.doWUBlastCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.doWUBlastCompleted(this, new doWUBlastCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://www.ebi.ac.uk/WSWUBlast")]
    public partial class inputParams {
        
        private string programField;
        
        private string databaseField;
        
        private string matrixField;
        
        private System.Nullable<float> expField;
        
        private System.Nullable<bool> echofilterField;
        
        private string filterField;
        
        private System.Nullable<int> numalField;
        
        private System.Nullable<int> scoresField;
        
        private string sensitivityField;
        
        private string sortField;
        
        private string statsField;
        
        private string strandField;
        
        private string outformatField;
        
        private System.Nullable<int> topcombonField;
        
        private string appxmlField;
        
        private System.Nullable<bool> asyncField;
        
        private string emailField;
        
        /// <remarks/>
        public string program {
            get {
                return this.programField;
            }
            set {
                this.programField = value;
            }
        }
        
        /// <remarks/>
        public string database {
            get {
                return this.databaseField;
            }
            set {
                this.databaseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string matrix {
            get {
                return this.matrixField;
            }
            set {
                this.matrixField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public System.Nullable<float> exp {
            get {
                return this.expField;
            }
            set {
                this.expField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public System.Nullable<bool> echofilter {
            get {
                return this.echofilterField;
            }
            set {
                this.echofilterField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string filter {
            get {
                return this.filterField;
            }
            set {
                this.filterField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public System.Nullable<int> numal {
            get {
                return this.numalField;
            }
            set {
                this.numalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public System.Nullable<int> scores {
            get {
                return this.scoresField;
            }
            set {
                this.scoresField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string sensitivity {
            get {
                return this.sensitivityField;
            }
            set {
                this.sensitivityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string sort {
            get {
                return this.sortField;
            }
            set {
                this.sortField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string stats {
            get {
                return this.statsField;
            }
            set {
                this.statsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string strand {
            get {
                return this.strandField;
            }
            set {
                this.strandField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string outformat {
            get {
                return this.outformatField;
            }
            set {
                this.outformatField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public System.Nullable<int> topcombon {
            get {
                return this.topcombonField;
            }
            set {
                this.topcombonField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string appxml {
            get {
                return this.appxmlField;
            }
            set {
                this.appxmlField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public System.Nullable<bool> async {
            get {
                return this.asyncField;
            }
            set {
                this.asyncField = value;
            }
        }
        
        /// <remarks/>
        public string email {
            get {
                return this.emailField;
            }
            set {
                this.emailField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://www.ebi.ac.uk/WSWUBlast")]
    public partial class outData {
        
        private string print_nameField;
        
        private string nameField;
        
        private string selectedField;
        
        private string data_typeField;
        
        private string input_typeField;
        
        private string search_typeField;
        
        /// <remarks/>
        public string print_name {
            get {
                return this.print_nameField;
            }
            set {
                this.print_nameField = value;
            }
        }
        
        /// <remarks/>
        public string name {
            get {
                return this.nameField;
            }
            set {
                this.nameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string selected {
            get {
                return this.selectedField;
            }
            set {
                this.selectedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string data_type {
            get {
                return this.data_typeField;
            }
            set {
                this.data_typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string input_type {
            get {
                return this.input_typeField;
            }
            set {
                this.input_typeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.SoapElementAttribute(IsNullable=true)]
        public string search_type {
            get {
                return this.search_typeField;
            }
            set {
                this.search_typeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://www.ebi.ac.uk/WSWUBlast")]
    public partial class WSFile {
        
        private string typeField;
        
        private string extField;
        
        /// <remarks/>
        public string type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        public string ext {
            get {
                return this.extField;
            }
            set {
                this.extField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.SoapTypeAttribute(Namespace="http://www.ebi.ac.uk/WSWUBlast")]
    public partial class data {
        
        private string typeField;
        
        private string contentField;
        
        /// <remarks/>
        public string type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /// <remarks/>
        public string content {
            get {
                return this.contentField;
            }
            set {
                this.contentField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void blastpCompletedEventHandler(object sender, blastpCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class blastpCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal blastpCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void blastnCompletedEventHandler(object sender, blastnCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class blastnCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal blastnCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void getOutputCompletedEventHandler(object sender, getOutputCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getOutputCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getOutputCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void getXMLCompletedEventHandler(object sender, getXMLCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getXMLCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getXMLCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void runWUBlastCompletedEventHandler(object sender, runWUBlastCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class runWUBlastCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal runWUBlastCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void checkStatusCompletedEventHandler(object sender, checkStatusCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class checkStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal checkStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void pollCompletedEventHandler(object sender, pollCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class pollCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal pollCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void getResultsCompletedEventHandler(object sender, getResultsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getResultsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getResultsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public WSFile[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((WSFile[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void getIdsCompletedEventHandler(object sender, getIdsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getIdsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getIdsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void getMatricesCompletedEventHandler(object sender, getMatricesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getMatricesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getMatricesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public outData[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((outData[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void getProgramsCompletedEventHandler(object sender, getProgramsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getProgramsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getProgramsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public outData[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((outData[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void getDatabasesCompletedEventHandler(object sender, getDatabasesCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getDatabasesCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getDatabasesCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public outData[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((outData[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void getSortCompletedEventHandler(object sender, getSortCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getSortCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getSortCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public outData[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((outData[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void getStatsCompletedEventHandler(object sender, getStatsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getStatsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getStatsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public outData[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((outData[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void getXmlFormatsCompletedEventHandler(object sender, getXmlFormatsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getXmlFormatsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getXmlFormatsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public outData[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((outData[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void getSensitivityCompletedEventHandler(object sender, getSensitivityCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getSensitivityCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getSensitivityCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public outData[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((outData[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void getFiltersCompletedEventHandler(object sender, getFiltersCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class getFiltersCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal getFiltersCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public outData[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((outData[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void polljobCompletedEventHandler(object sender, polljobCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class polljobCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal polljobCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    public delegate void doWUBlastCompletedEventHandler(object sender, doWUBlastCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.1432")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class doWUBlastCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal doWUBlastCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
    }
}
