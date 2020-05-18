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
using System.Data;
using System.Diagnostics;
using System.IO;
using Arachnode.Configuration;
using Arachnode.Configuration.Value.Enums;
using Arachnode.DataAccess.Managers;
using Arachnode.DataSource;
using Arachnode.DataSource.ArachnodeDataSetTableAdapters;
using Arachnode.Performance;

#endregion

//using Arachnode.Configuration;

namespace Arachnode.DataAccess
{
    /// <summary>
    /// 	Contains all database access methods.  This class is not thread safe.
    /// </summary>
    public class ArachnodeDAO
    {
        private readonly AllowedDataTypesTableAdapter _allowedDataTypesTableAdapter = new AllowedDataTypesTableAdapter();
        private readonly ConfigurationTableAdapter _configurationTableAdapter = new ConfigurationTableAdapter();
        private readonly ContentTypesTableAdapter _contentTypesTableAdapter = new ContentTypesTableAdapter();
        private readonly CrawlActionsTableAdapter _crawlActionsTableAdapter = new CrawlActionsTableAdapter();
        private readonly CrawlRequestsTableAdapter _crawlRequestsTableAdapter = new CrawlRequestsTableAdapter();
        private readonly CrawlRulesTableAdapter _crawlRulesTableAdapter = new CrawlRulesTableAdapter();
        private readonly ArachnodeDataSet.DiscoveriesDataTable _discoveriesDataTable = new ArachnodeDataSet.DiscoveriesDataTable();
        private readonly DiscoveriesTableAdapter _discoveriesTableAdapter = new DiscoveriesTableAdapter();
        private readonly DomainsTableAdapter _domainsTableAdapter = new DomainsTableAdapter();
        private readonly EngineActionsTableAdapter _engineActionsTableAdapter = new EngineActionsTableAdapter();
        private readonly ExtensionsTableAdapter _extensionsTableAdapter = new ExtensionsTableAdapter();
        private readonly ArachnodeDataSet.FilesDataTable _filesDataTable = new ArachnodeDataSet.FilesDataTable();
        private readonly ArachnodeDataSet.FilesDiscoveriesDataTable _filesDiscoveriesDataTable = new ArachnodeDataSet.FilesDiscoveriesDataTable();
        private readonly FilesDiscoveriesTableAdapter _filesDiscoveriesTableAdapter = new FilesDiscoveriesTableAdapter();
        private readonly ArachnodeDataSet.FilesMetaDataDataTable _filesMetaDataDataTable = new ArachnodeDataSet.FilesMetaDataDataTable();
        private readonly FilesMetaDataTableAdapter _filesMetaDataTableAdapter = new FilesMetaDataTableAdapter();
        private readonly FilesTableAdapter _filesTableAdapter = new FilesTableAdapter();
        private readonly HostsTableAdapter _hostsTableAdapter = new HostsTableAdapter();
        private readonly ArachnodeDataSet.ImagesDataTable _imagesDataTable = new ArachnodeDataSet.ImagesDataTable();
        private readonly ArachnodeDataSet.ImagesDiscoveriesDataTable _imagesDiscoveriesDataTable = new ArachnodeDataSet.ImagesDiscoveriesDataTable();
        private readonly ImagesDiscoveriesTableAdapter _imagesDiscoveriesTableAdapter = new ImagesDiscoveriesTableAdapter();
        private readonly ArachnodeDataSet.ImagesMetaDataDataTable _imagesMetaDataDataTable = new ArachnodeDataSet.ImagesMetaDataDataTable();
        private readonly ImagesMetaDataTableAdapter _imagesMetaDataTableAdapter = new ImagesMetaDataTableAdapter();
        private readonly ImagesTableAdapter _imagesTableAdapter = new ImagesTableAdapter();
        private readonly PrioritiesTableAdapter _prioritiesTableAdapter = new PrioritiesTableAdapter();
        private readonly QueriesTableAdapter _queriesTableAdapter = new QueriesTableAdapter();
        private readonly SchemesTableAdapter _schemesTableAdapter = new SchemesTableAdapter();
        private readonly VersionTableAdapter _versionTableAdapter = new VersionTableAdapter();
        private readonly ArachnodeDataSet.WebPagesDataTable _webPagesDataTable = new ArachnodeDataSet.WebPagesDataTable();
        private readonly WebPagesTableAdapter _webPagesTableAdapter = new WebPagesTableAdapter();

        private long? _lastCrawlRequestID;
        private long? _lastDisallowedAbsoluteUriID;
        private long? _lastEmailAddressID;
        private long? _lastExceptionID;
        private string _lastExceptionMessage;
        private long? _lastFileID;
        private long? _lastHyperLinkID;
        private long? _lastImageID;
        private long? _lastWebPageID;

        /// <summary>
        /// 	Initializes a new instance of the <see cref = "ArachnodeDAO" /> class.
        /// 	This method does not initialize the ApplicationSettings or WebSettings classes.
        /// </summary>
        public ArachnodeDAO() : this(ApplicationSettings.ConnectionString, false, false)
        {
        }

        /// <summary>
        /// 	Initializes a new instance of the <see cref = "ArachnodeDAO" /> class.
        /// 	This method does not initialize the ApplicationSettings or WebSettings classes.
        /// </summary>
        /// <param name = "connectionString">The connection string.</param>
        public ArachnodeDAO(string connectionString) : this(connectionString, false, false)
        {
        }

        /// <summary>
        /// 	Initializes a new instance of the <see cref = "ArachnodeDAO" /> class.
        /// </summary>
        /// <param name = "initializeApplicationConfiguration">if set to <c>true</c> [initialize application configuration].</param>
        /// <param name = "initializeWebConfiguration">if set to <c>true</c> [initialize web configuration].</param>
        public ArachnodeDAO(bool initializeApplicationConfiguration, bool initializeWebConfiguration) : this(ApplicationSettings.ConnectionString, initializeApplicationConfiguration, initializeWebConfiguration)
        {
        }

        /// <summary>
        /// 	Initializes a new instance of the <see cref = "ArachnodeDAO" /> class.
        /// </summary>
        /// <param name = "connectionString">The connection string.</param>
        /// <param name = "initializeApplicationConfiguration">if set to <c>true</c> [initialize application configuration].</param>
        /// <param name = "initializeWebConfiguration">if set to <c>true</c> [initialize web configuration].</param>
        public ArachnodeDAO(string connectionString, bool initializeApplicationConfiguration, bool initializeWebConfiguration)
        {
            ConnectionString.Value = connectionString;

            CheckVersion();

            if (initializeApplicationConfiguration)
            {
                ConfigurationManager.InitializeConfiguration(ConfigurationType.Application, this);
            }

            if (initializeWebConfiguration)
            {
                ConfigurationManager.InitializeConfiguration(ConfigurationType.Web, this);
            }

            UpdateCommandTimeout(_allowedDataTypesTableAdapter.SqlCommands);
            UpdateCommandTimeout(_configurationTableAdapter.SqlCommands);
            UpdateCommandTimeout(_contentTypesTableAdapter.SqlCommands);
            UpdateCommandTimeout(_crawlActionsTableAdapter.SqlCommands);
            UpdateCommandTimeout(_crawlRequestsTableAdapter.SqlCommands);
            UpdateCommandTimeout(_crawlRulesTableAdapter.SqlCommands);
            UpdateCommandTimeout(_domainsTableAdapter.SqlCommands);
            UpdateCommandTimeout(_engineActionsTableAdapter.SqlCommands);
            UpdateCommandTimeout(_extensionsTableAdapter.SqlCommands);
            UpdateCommandTimeout(_filesTableAdapter.SqlCommands);
            UpdateCommandTimeout(_filesDiscoveriesTableAdapter.SqlCommands);
            UpdateCommandTimeout(_filesMetaDataTableAdapter.SqlCommands);
            UpdateCommandTimeout(_hostsTableAdapter.SqlCommands);
            UpdateCommandTimeout(_imagesTableAdapter.SqlCommands);
            UpdateCommandTimeout(_imagesDiscoveriesTableAdapter.SqlCommands);
            UpdateCommandTimeout(_imagesMetaDataTableAdapter.SqlCommands);
            UpdateCommandTimeout(_prioritiesTableAdapter.SqlCommands);
            UpdateCommandTimeout(_queriesTableAdapter.SqlCommands);
            UpdateCommandTimeout(_schemesTableAdapter.SqlCommands);
            UpdateCommandTimeout(_versionTableAdapter.SqlCommands);
            UpdateCommandTimeout(_webPagesTableAdapter.SqlCommands);

            //extremely advanced...  ask if you have performance problems...
            //OpenCommandConnections(_allowedDataTypesTableAdapter.SqlCommands);
            //OpenCommandConnections(_configurationTableAdapter.SqlCommands);
            //OpenCommandConnections(_contentTypesTableAdapter.SqlCommands);
            //OpenCommandConnections(_crawlActionsTableAdapter.SqlCommands);
            //OpenCommandConnections(_crawlRequestsTableAdapter.SqlCommands);
            //OpenCommandConnections(_crawlRulesTableAdapter.SqlCommands);
            //OpenCommandConnections(_domainsTableAdapter.SqlCommands);
            //OpenCommandConnections(_engineActionsTableAdapter.SqlCommands);
            //OpenCommandConnections(_extensionsTableAdapter.SqlCommands);
            //OpenCommandConnections(_filesTableAdapter.SqlCommands);
            //OpenCommandConnections(_filesDiscoveriesTableAdapter.SqlCommands);
            //OpenCommandConnections(_filesMetaDataTableAdapter.SqlCommands);
            //OpenCommandConnections(_hostsTableAdapter.SqlCommands);
            //OpenCommandConnections(_imagesTableAdapter.SqlCommands);
            //OpenCommandConnections(_imagesDiscoveriesTableAdapter.SqlCommands);
            //OpenCommandConnections(_imagesMetaDataTableAdapter.SqlCommands);
            //OpenCommandConnections(_prioritiesTableAdapter.SqlCommands);
            //OpenCommandConnections(_queriesTableAdapter.SqlCommands);
            //OpenCommandConnections(_schemesTableAdapter.SqlCommands);
            //OpenCommandConnections(_versionTableAdapter.SqlCommands);
            //OpenCommandConnections(_webPagesTableAdapter.SqlCommands);
        }

        ~ArachnodeDAO()
        {
            //extremely advanced...  ask if you have performance problems...
            //CloseCommandConnections(_allowedDataTypesTableAdapter.SqlCommands);
            //CloseCommandConnections(_configurationTableAdapter.SqlCommands);
            //CloseCommandConnections(_contentTypesTableAdapter.SqlCommands);
            //CloseCommandConnections(_crawlActionsTableAdapter.SqlCommands);
            //CloseCommandConnections(_crawlRequestsTableAdapter.SqlCommands);
            //CloseCommandConnections(_crawlRulesTableAdapter.SqlCommands);
            //CloseCommandConnections(_domainsTableAdapter.SqlCommands);
            //CloseCommandConnections(_engineActionsTableAdapter.SqlCommands);
            //CloseCommandConnections(_extensionsTableAdapter.SqlCommands);
            //CloseCommandConnections(_filesTableAdapter.SqlCommands);
            //CloseCommandConnections(_filesDiscoveriesTableAdapter.SqlCommands);
            //CloseCommandConnections(_filesMetaDataTableAdapter.SqlCommands);
            //CloseCommandConnections(_hostsTableAdapter.SqlCommands);
            //CloseCommandConnections(_imagesTableAdapter.SqlCommands);
            //CloseCommandConnections(_imagesDiscoveriesTableAdapter.SqlCommands);
            //CloseCommandConnections(_imagesMetaDataTableAdapter.SqlCommands);
            //CloseCommandConnections(_prioritiesTableAdapter.SqlCommands);
            //CloseCommandConnections(_queriesTableAdapter.SqlCommands);
            //CloseCommandConnections(_schemesTableAdapter.SqlCommands);
            //CloseCommandConnections(_versionTableAdapter.SqlCommands);
            //CloseCommandConnections(_webPagesTableAdapter.SqlCommands);
        }

        /// <summary>
        /// 	The last ExceptionID submitted to table 'DisallowedAbsoluteUris'.
        /// </summary>
        /// <value>The last disallowed absolute URI ID.</value>
        public long? LastDisallowedAbsoluteUriID
        {
            get { return _lastDisallowedAbsoluteUriID; }
            private set { _lastDisallowedAbsoluteUriID = value; }
        }

        /// <summary>
        /// 	The last EmailAddressID submitted to table 'EmailAddresses'.
        /// </summary>
        /// <value>The last email address ID.</value>
        public long? LastEmailAddressID
        {
            get { return _lastEmailAddressID; }
            private set { _lastEmailAddressID = value; }
        }

        /// <summary>
        /// 	The last ExceptionID returned from table 'Exceptions'.
        /// </summary>
        /// <value>The last exception ID.</value>
        public long? LastExceptionID
        {
            get { return _lastExceptionID; }
            private set { _lastExceptionID = value; }
        }

        /// <summary>
        /// 	The last Message submitted to table 'Exceptions'.
        /// </summary>
        /// <value>The last exception message.</value>
        public string LastExceptionMessage
        {
            get { return _lastExceptionMessage; }
            private set { _lastExceptionMessage = value; }
        }

        /// <summary>
        /// 	The last FileID submitted to table 'Files'.
        /// </summary>
        /// <value>The last file ID.</value>
        public long? LastFileID
        {
            get { return _lastFileID; }
            private set { _lastFileID = value; }
        }

        /// <summary>
        /// 	The last HyperLinksID submitted to table 'HyperLinks'.
        /// </summary>
        /// <value>The last hyper link ID.</value>
        public long? LastHyperLinkID
        {
            get { return _lastHyperLinkID; }
            private set { _lastHyperLinkID = value; }
        }

        /// <summary>
        /// 	The last ImageID submitted to table 'Images'.
        /// </summary>
        /// <value>The last image ID.</value>
        public long? LastImageID
        {
            get { return _lastImageID; }
            private set { _lastImageID = value; }
        }

        /// <summary>
        /// 	The last WebPageID submitted to table 'WebPages'.
        /// </summary>
        public long? LastWebPageID
        {
            get { return _lastWebPageID; }
            private set { _lastWebPageID = value; }
        }

        /// <summary>
        /// 	Updates the command timeout.
        /// </summary>
        /// <param name = "dbCommands">The db commands.</param>
        private void UpdateCommandTimeout(IEnumerable<IDbCommand> dbCommands)
        {
            foreach (IDbCommand iDbCommand in dbCommands)
            {
                iDbCommand.CommandTimeout = ApplicationSettings.SqlCommandTimeoutInMinutes*60;
            }
        }

        /// <summary>
        /// Opens all database connections.  Doing so before crawling allows CRUD operations to operate faster than opening and closing the connection each time an operation is requested.
        /// Extremely advanced...  ask if you have performance problems...
        /// </summary>
        /// <param name="dbCommands"></param>
        private void OpenCommandConnections(IEnumerable<IDbCommand> dbCommands)
        {
            try
            {
                foreach (IDbCommand iDbCommand in dbCommands)
                {
                    try
                    {
                        if (iDbCommand.Connection.State != ConnectionState.Open)
                        {
                            iDbCommand.Connection.ConnectionString += "Pooling='false';";

                            iDbCommand.Connection.Open();
                        }
                    }
                    catch (Exception exception)
                    {
                        InsertException(null, null, exception, true);
                    }
                }
            }
            catch (Exception exception)
            {
                InsertException(null, null, exception, true);
            }
        }

        /// <summary>
        /// Closes all dastabase connections.  Called by the ArachnodeDAO destructor.
        /// Extremely advanced...  ask if you have performance problems...
        /// </summary>
        /// <param name="dbCommands"></param>
        private void CloseCommandConnections(IEnumerable<IDbCommand> dbCommands)
        {
            try
            {
                foreach (IDbCommand iDbCommand in dbCommands)
                {
                    try
                    {
                        if (iDbCommand.Connection.State != ConnectionState.Closed)
                        {
                            iDbCommand.Connection.Close();
                        }
                    }
                    catch (Exception exception)
                    {
                        InsertException(null, null, exception, true);
                    }
                }
            }
            catch (Exception exception)
            {
                InsertException(null, null, exception, true);
            }
        }

        public const string VERSION = "3.0.0.0";

        public void CheckVersion()
        {
            ArachnodeDataSet.VersionDataTable versionDataTable = GetVersion();

            if (versionDataTable.Count == 0)
            {
                throw new Exception("EXCEPTION: The installed database version is not equal to the expected applicaton version.\nEXPECTED: " + VERSION + "\nACTUAL: N/A\n\nPlease contact 'arachnode.net' at http://arachnode.net.");
            }
            //ANODET: Update the version for the release.
            if (versionDataTable.Rows[0]["Value"].ToString() != VERSION)
            {
                if (versionDataTable.Rows[0]["Value"].ToString() == "2.0.0.0")
                {
                    throw new Exception("EXCEPTION: The installed database version is not equal to the expected applicaton version.\nEXPECTED: " + VERSION + "\nACTUAL: " + versionDataTable.Rows[0]["Value"] + "\n\nExecute the SQL script 'UpdateDatabase_2.0.0.0_to_2.5.0.0.sql'.");
                }

                if (versionDataTable.Rows[0]["Value"].ToString() == "2.5.0.0")
                {
                    throw new Exception("EXCEPTION: The installed database version is not equal to the expected applicaton version.\nEXPECTED: " + VERSION + "\nACTUAL: " + versionDataTable.Rows[0]["Value"] + "\n\nExecute the SQL script 'UpdateDatabase_2.5.0.0_to_2.5.0.12.sql'.");
                }

                if (versionDataTable.Rows[0]["Value"].ToString() == "2.5.0.12")
                {
                    throw new Exception("EXCEPTION: The installed database version is not equal to the expected applicaton version.\nEXPECTED: " + VERSION + "\nACTUAL: " + versionDataTable.Rows[0]["Value"] + "\n\nExecute the SQL script 'UpdateDatabase_2.5.0.12_to_3.0.0.0.sql'.");
                }

                throw new Exception("EXCEPTION: The installed database version is not equal to the expected applicaton version.\nEXPECTED: " + VERSION + "\nACTUAL: " + versionDataTable.Rows[0]["Value"] + "\n\nPlease contact arachnode.net.");
            }
        }

        /// <summary>
        /// 	Deletes a row from table 'CrawlRequests'.
        /// 	Calling this method with absoluteUri1 set to 'null' and absoluteUri2 set to 'null' deletes all CrawlRequests from the 'CrawlRequests' table.
        /// </summary>
        /// <param name = "absoluteUri1">The absolute uri1.</param>
        /// <param name = "absoluteUri2">The absolute uri2.</param>
        public void DeleteCrawlRequest(string absoluteUri1, string absoluteUri2)
        {
            try
            {
                _queriesTableAdapter.DeleteCrawlRequest(absoluteUri1, absoluteUri2);

                Counters.GetInstance().CrawlRequestDeleted();
            }

            catch (Exception exception)
            {
                InsertException(absoluteUri1, absoluteUri2, exception, false);
            }
        }

        /// <summary>
        /// 	Deletes all rows from table 'Discoveries'.
        /// </summary>
        public void DeleteDiscoveries()
        {
            try
            {
                _queriesTableAdapter.DeleteDiscoveries();
            }
            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }
        }

        /// <summary>
        /// 	Deletes a row from table 'WebPages'.
        /// </summary>
        /// <param name = "webPageAbsoluteUriOrID">The WebPage AbsoluteUri or ID.</param>
        public void DeleteWebPage(string webPageAbsoluteUriOrID)
        {
            try
            {
                _queriesTableAdapter.DeleteWebPage(webPageAbsoluteUriOrID);
            }

            catch (Exception exception)
            {
                InsertException(webPageAbsoluteUriOrID, null, exception, false);
            }
        }

        /// <summary>
        /// 	Executes dynamic SQL.
        /// </summary>
        /// <param name = "query">The query.</param>
        public void ExecuteSql(string query)
        {
            try
            {
                _queriesTableAdapter.ExecuteSql(query);
//#if DEBUG

//#else
                //throw new Exception("ArachnodeDAO.ExecuteSQL() should not be used in the 'Release' configuration.");
//#endif
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }
        }

        /// <summary>
        /// 	DataTypes represent mappings between the ResponseHeader 'Content-type:' and the type to be used by SQL for Full-text indexing.
        /// 	If you find that content you wish to crawl isn't being crawled, check the table 'DisallowedAbsoluteUris'.  If you see 'Disallowed by unassigned DataType', add the DataType to table 'AllowedDataTypes'.
        /// </summary>
        /// <returns></returns>
        public ArachnodeDataSet.AllowedDataTypesDataTable GetAllowedDataTypes()
        {
            try
            {
                return _allowedDataTypesTableAdapter.GetData();
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }

            return new ArachnodeDataSet.AllowedDataTypesDataTable();
        }

        /// <summary>
        /// 	Gets the configuration.
        /// </summary>
        /// <returns></returns>
        public ArachnodeDataSet.ConfigurationDataTable GetConfiguration()
        {
            try
            {
                ArachnodeDataSet.ConfigurationDataTable configurationDataTable = _configurationTableAdapter.GetData();

                try
                {
                    if (!File.Exists("cfg.Configuration.xml") || !ApplicationSettings.OverrideDatabaseConfigurationWithXmlConfiguration)
                    {
                        configurationDataTable.WriteXml("cfg.Configuration.xml");
                    }

                    if (ApplicationSettings.OverrideDatabaseConfigurationWithXmlConfiguration)
                    {
                        configurationDataTable.ReadXml("cfg.Configuration.xml");
                    }
                }
                catch (Exception exception)
                {
                    InsertException(null, null, exception, true);
                }

                return configurationDataTable;
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, true);
            }

            return new ArachnodeDataSet.ConfigurationDataTable();
        }

        /// <summary>
        /// 	Gets the crawl actions.  Used by the ActionManager.
        /// </summary>
        /// <returns></returns>
        public ArachnodeDataSet.CrawlActionsDataTable GetCrawlActions()
        {
            try
            {
                ArachnodeDataSet.CrawlActionsDataTable crawlActionsDataTable = _crawlActionsTableAdapter.GetData();

                try
                {
                    if (!File.Exists("cfg.CrawlActions.xml") || !ApplicationSettings.OverrideDatabaseConfigurationWithXmlConfiguration)
                    {
                        crawlActionsDataTable.WriteXml("cfg.CrawlActions.xml");
                    }

                    if (ApplicationSettings.OverrideDatabaseConfigurationWithXmlConfiguration)
                    {
                        crawlActionsDataTable.ReadXml("cfg.CrawlActions.xml");
                    }
                }
                catch (Exception exception)
                {
                    InsertException(null, null, exception, true);
                }

                return crawlActionsDataTable;
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }

            return new ArachnodeDataSet.CrawlActionsDataTable();
        }

        /// <summary>
        /// 	Gets the crawl rules.  Used by the RuleManager.
        /// </summary>
        /// <returns></returns>
        public ArachnodeDataSet.CrawlRulesDataTable GetCrawlRules()
        {
            try
            {
                ArachnodeDataSet.CrawlRulesDataTable crawlRulesDataTable = _crawlRulesTableAdapter.GetData();

                try
                {
                    if (!File.Exists("cfg.CrawlRules.xml") || !ApplicationSettings.OverrideDatabaseConfigurationWithXmlConfiguration)
                    {
                        crawlRulesDataTable.WriteXml("cfg.CrawlRules.xml");
                    }

                    if (ApplicationSettings.OverrideDatabaseConfigurationWithXmlConfiguration)
                    {
                        crawlRulesDataTable.ReadXml("cfg.CrawlRules.xml");
                    }
                }
                catch (Exception exception)
                {
                    InsertException(null, null, exception, true);
                }

                return crawlRulesDataTable;
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }

            return new ArachnodeDataSet.CrawlRulesDataTable();
        }

        /// <summary>
        /// 	Gets all ContentTypes.
        /// 	A ContentType would be text/html, image/jpeg, etc.
        /// </summary>
        /// <returns></returns>
        public ArachnodeDataSet.ContentTypesDataTable GetContentTypes()
        {
            try
            {
                return _contentTypesTableAdapter.GetData();
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }

            return new ArachnodeDataSet.ContentTypesDataTable();
        }

        /// <summary>
        /// 	Returns a DataTable containing the database representation of a CrawlRequest.
        /// 	This procedure creates CrawlRequests for the Engine to process from rows in table 'CrawlRequests'.
        /// </summary>
        /// <param name = "_maximumNumberOfCrawlRequestsToCreatePerBatch">See: ApplicationSettings.MaximumNumberOfCrawlRequestsToCreatePerBatch</param>
        /// <param name = "createCrawlRequestsFromDatabaseCrawlRequests">See: ApplicationSettings.CreateCrawlRequestsFromDatabaseCrawlRequests</param>
        /// <param name = "createCrawlRequestsFromDatabaseFiles">See: ApplicationSettings.CreateCrawlRequestsFromDatabaseFiles</param>
        /// <param name = "assignCrawlRequestPrioritiesForFiles">if set to <c>true</c> [assign crawl request priorities for files].</param>
        /// <param name = "createCrawlRequestsFromDatabaseHyperLinks">See: ApplicationSettings.CreateCrawlRequestsFromDatabaseHyperLinks</param>
        /// <param name = "assignCrawlRequestPrioritiesForHyperLinks">if set to <c>true</c> [assign crawl request priorities for hyper links].</param>
        /// <param name = "createCrawlRequestsFromDatabaseImages">if set to <c>true</c> [create crawl requests from database images].</param>
        /// <param name = "assignCrawlRequestPrioritiesForImages">if set to <c>true</c> [assign crawl request priorities for images].</param>
        /// <param name = "createCrawlRequestsFromDatabaseWebPages">See: ApplicationSettings.CreateCrawlRequestsFromDatabaseWebPages</param>
        /// <param name = "assignCrawlRequestPrioritiesForWebPages">if set to <c>true</c> [assign crawl request priorities for web pages].</param>
        /// <returns></returns>
        public ArachnodeDataSet.CrawlRequestsDataTable GetCrawlRequests(int _maximumNumberOfCrawlRequestsToCreatePerBatch, bool createCrawlRequestsFromDatabaseCrawlRequests, bool createCrawlRequestsFromDatabaseFiles, bool assignCrawlRequestPrioritiesForFiles, bool createCrawlRequestsFromDatabaseHyperLinks, bool assignCrawlRequestPrioritiesForHyperLinks, bool createCrawlRequestsFromDatabaseImages, bool assignCrawlRequestPrioritiesForImages, bool createCrawlRequestsFromDatabaseWebPages, bool assignCrawlRequestPrioritiesForWebPages)
        {
            try
            {
                return _crawlRequestsTableAdapter.GetData(_maximumNumberOfCrawlRequestsToCreatePerBatch, createCrawlRequestsFromDatabaseCrawlRequests, createCrawlRequestsFromDatabaseFiles, assignCrawlRequestPrioritiesForFiles, createCrawlRequestsFromDatabaseHyperLinks, assignCrawlRequestPrioritiesForHyperLinks, createCrawlRequestsFromDatabaseImages, assignCrawlRequestPrioritiesForImages, createCrawlRequestsFromDatabaseWebPages, assignCrawlRequestPrioritiesForWebPages);
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }

            return new ArachnodeDataSet.CrawlRequestsDataTable();
        }

        /// <summary>
        /// 	Gets a Discovery from the Discoveries table.  If the Discovery isn't present, null is returned.
        /// 	This method is used by the Cache to determine if a Discovery has been encountered in a crawl run.
        /// 	If a Discovery hasn't been 'Discovered', then it is eligible to have a CrawlRequest created from it and to be crawled.
        /// </summary>
        /// <param name = "absoluteUri">The absolute URI.</param>
        /// <returns></returns>
        public ArachnodeDataSet.DiscoveriesRow GetDiscovery(string absoluteUri)
        {
            try
            {
                _discoveriesDataTable.Clear();

                _discoveriesTableAdapter.Fill(_discoveriesDataTable, absoluteUri, null);

                if (_discoveriesDataTable.Count != 0)
                {
                    return (ArachnodeDataSet.DiscoveriesRow) _discoveriesDataTable.Rows[0];
                }

                return null;
            }

            catch (Exception exception)
            {
                InsertException(absoluteUri, null, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Gets the discoveries.
        /// </summary>
        /// <param name = "numberOfDiscoveriesToReturn">The number of discoveries to return.</param>
        /// <returns></returns>
        public ArachnodeDataSet.DiscoveriesDataTable GetDiscoveries(int numberOfDiscoveriesToReturn)
        {
            try
            {
                return _discoveriesTableAdapter.GetData(null, numberOfDiscoveriesToReturn);
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Gets all Domains.
        /// </summary>
        /// <returns></returns>
        public ArachnodeDataSet.DomainsDataTable GetDomains()
        {
            try
            {
                return _domainsTableAdapter.GetData();
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }

            return new ArachnodeDataSet.DomainsDataTable();
        }

        /// <summary>
        /// 	Gets the engine actions.  Used by the ActionManager.
        /// </summary>
        /// <returns></returns>
        public ArachnodeDataSet.EngineActionsDataTable GetEngineActions()
        {
            try
            {
                ArachnodeDataSet.EngineActionsDataTable engineActionsDataTable = _engineActionsTableAdapter.GetData();

                try
                {
                    if (!File.Exists("cfg.EngineActions.xml") || !ApplicationSettings.OverrideDatabaseConfigurationWithXmlConfiguration)
                    {
                        engineActionsDataTable.WriteXml("cfg.EngineActions.xml");
                    }

                    if (ApplicationSettings.OverrideDatabaseConfigurationWithXmlConfiguration)
                    {
                        engineActionsDataTable.ReadXml("cfg.EngineActions.xml");
                    }
                }
                catch (Exception exception)
                {
                    InsertException(null, null, exception, true);
                }

                return engineActionsDataTable;
            }
            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }

            return new ArachnodeDataSet.EngineActionsDataTable();
        }

        /// <summary>
        /// 	Gets all Extensions.
        /// </summary>
        /// <returns></returns>
        public ArachnodeDataSet.ExtensionsDataTable GetExtensions()
        {
            try
            {
                return _extensionsTableAdapter.GetData();
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }

            return new ArachnodeDataSet.ExtensionsDataTable();
        }

        /// <summary>
        /// 	Selects a row from table 'Files' by AbsoluteUri or ID.
        /// </summary>
        /// <param name = "fileAbsoluteUriOrID">The file absolute URI or ID.</param>
        /// <returns></returns>
        public ArachnodeDataSet.FilesRow GetFile(string fileAbsoluteUriOrID)
        {
            try
            {
                _filesDataTable.Clear();

                _filesTableAdapter.Fill(_filesDataTable, fileAbsoluteUriOrID);

                if (_filesDataTable.Count != 0)
                {
                    return (ArachnodeDataSet.FilesRow) _filesDataTable.Rows[0];
                }

                return null;
            }
            catch (Exception exception)
            {
                InsertException(fileAbsoluteUriOrID, null, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Selects a row from table 'Files_Discoveries' by FileID.
        /// </summary>
        /// <param name = "fileID">The file ID.</param>
        /// <returns></returns>
        public ArachnodeDataSet.FilesDiscoveriesDataTable GetFileDiscoveries(long fileID)
        {
            try
            {
                _filesDiscoveriesDataTable.Clear();

                _filesDiscoveriesTableAdapter.Fill(_filesDiscoveriesDataTable, fileID);

                if (_filesDiscoveriesDataTable.Count != 0)
                {
                    return _filesDiscoveriesDataTable;
                }

                return null;
            }
            catch (Exception exception)
            {
                InsertException(fileID.ToString(), null, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Gets all Hosts.
        /// </summary>
        /// <returns></returns>
        public ArachnodeDataSet.HostsDataTable GetHosts()
        {
            try
            {
                return _hostsTableAdapter.GetData();
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }

            return new ArachnodeDataSet.HostsDataTable();
        }

        /// <summary>
        /// 	Gets the Database Version.  Used by the Crawler to ensure the client is communicating with the proper Database Version.
        /// </summary>
        /// <returns></returns>
        public ArachnodeDataSet.VersionDataTable GetVersion()
        {
            try
            {
                return _versionTableAdapter.GetData();
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);

                throw new Exception(exception.Message, exception);
            }
        }

        /// <summary>
        /// 	Selects a row from table 'Files_MetaData' by FileID.
        /// </summary>
        /// <param name = "fileID">The file ID.</param>
        /// <returns></returns>
        public ArachnodeDataSet.FilesMetaDataRow GetFileMetaData(long fileID)
        {
            try
            {
                _filesMetaDataDataTable.Clear();

                _filesMetaDataTableAdapter.Fill(_filesMetaDataDataTable, fileID);

                if (_filesMetaDataDataTable.Count != 0)
                {
                    return (ArachnodeDataSet.FilesMetaDataRow) _filesMetaDataDataTable.Rows[0];
                }

                return null;
            }
            catch (Exception exception)
            {
                InsertException(fileID.ToString(), null, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Selects a row from table 'Images' by AbsoluteUri or ID.
        /// </summary>
        /// <param name = "imageAbsoluteUriOrID">The image absolute URI or ID.</param>
        /// <returns></returns>
        public ArachnodeDataSet.ImagesRow GetImage(string imageAbsoluteUriOrID)
        {
            try
            {
                _imagesDataTable.Clear();

                _imagesTableAdapter.Fill(_imagesDataTable, imageAbsoluteUriOrID);

                if (_imagesDataTable.Count != 0)
                {
                    return (ArachnodeDataSet.ImagesRow) _imagesDataTable.Rows[0];
                }

                return null;
            }
            catch (Exception exception)
            {
                InsertException(imageAbsoluteUriOrID, null, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Selects a row from table 'Images_Discoveries' by ImageID.
        /// </summary>
        /// <param name = "imageID">The image ID.</param>
        /// <returns></returns>
        public ArachnodeDataSet.ImagesDiscoveriesDataTable GetImageDiscoveries(long imageID)
        {
            try
            {
                _imagesDiscoveriesDataTable.Clear();

                _imagesDiscoveriesTableAdapter.Fill(_imagesDiscoveriesDataTable, imageID);

                if (_imagesDiscoveriesDataTable.Count != 0)
                {
                    return _imagesDiscoveriesDataTable;
                }

                return null;
            }
            catch (Exception exception)
            {
                InsertException(imageID.ToString(), null, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Selects a row from table 'Images_MetaData' by ImageID.
        /// </summary>
        /// <param name = "imageID">The image ID.</param>
        /// <returns></returns>
        public ArachnodeDataSet.ImagesMetaDataRow GetImageMetaData(long imageID)
        {
            try
            {
                _imagesMetaDataDataTable.Clear();

                _imagesMetaDataTableAdapter.Fill(_imagesMetaDataDataTable, imageID);

                if (_imagesMetaDataDataTable.Count != 0)
                {
                    return (ArachnodeDataSet.ImagesMetaDataRow) _imagesMetaDataDataTable.Rows[0];
                }

                return null;
            }
            catch (Exception exception)
            {
                InsertException(imageID.ToString(), null, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Gets all Priorities.  Priorities are used by CrawlRequests, the ReportingManager and by the lucene.net indexing functionality
        /// 	to determine crawl priority, and to enhance the boosts of lucene.net documents.  A document for "C#" from domain "XYZ" may score a higher
        /// 	relevancy score than a document for "C#" from microsoft.com, but actual popularity/priority dictates that the result from microsoft.com should
        /// 	receive a higher boost and thereby a higher overall relevancy score than the document from domain "XYZ".
        /// </summary>
        /// <returns></returns>
        public ArachnodeDataSet.PrioritiesDataTable GetPriorities(int maximumNumberOfPriorities)
        {
            try
            {
                return _prioritiesTableAdapter.GetData(maximumNumberOfPriorities);
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }

            return new ArachnodeDataSet.PrioritiesDataTable();
        }

        /// <summary>
        /// 	Gets all Schemes.
        /// </summary>
        /// <returns></returns>
        public ArachnodeDataSet.SchemesDataTable GetSchemes()
        {
            try
            {
                return _schemesTableAdapter.GetData();
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }

            return new ArachnodeDataSet.SchemesDataTable();
        }

        /// <summary>
        /// 	Selects a row from table 'WebPages' by AbsoluteUri or ID.
        /// </summary>
        /// <param name = "webPageAbsoluteUriOrID">The web page absolute URI or ID.</param>
        /// <returns></returns>
        public ArachnodeDataSet.WebPagesRow GetWebPage(string webPageAbsoluteUriOrID)
        {
            try
            {
                _webPagesDataTable.Clear();

                _webPagesTableAdapter.Fill(_webPagesDataTable, webPageAbsoluteUriOrID);

                if (_webPagesDataTable.Count != 0)
                {
                    return (ArachnodeDataSet.WebPagesRow) _webPagesDataTable.Rows[0];
                }

                return null;
            }
            catch (Exception exception)
            {
                InsertException(webPageAbsoluteUriOrID, null, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Inserts the crawl request.
        /// 	See: http://arachnode.net/forums/p/564/10762.aspx#10762 if you experience difficulty with this function.
        /// 	Use this function if you want to insert CrawlRequests into the database.  The overload with the split 'currentDepth' and 'maximumDepth' parameters is intended for use by the Crawler itself.
        /// </summary>
        /// <param name = "created">The created.</param>
        /// <param name = "absoluteUri1">The absolute uri1.</param>
        /// <param name = "absoluteUri2">The absolute uri2.</param>
        /// <param name = "depth">The depth.</param>
        /// <param name = "restrictCrawlTo">The restrict crawl to.  Restricting a Crawl to a specific UriQualificationType means that the Crawl will only crawl WebPages that match the initial UriQualificationType.</param>
        /// <param name = "restrictDiscoveriesTo">The restrict discoveries to. Restricting a Crawl's Discoveries to a specific UriQualificationType means that the Crawl will only collect Discoveries that match the initial UriQualificationType.</param>
        /// <param name = "priority">The priority.</param>
        /// <returns></returns>
        public long? InsertCrawlRequest(DateTime created, string absoluteUri0, string absoluteUri1, string absoluteUri2, int depth, short restrictCrawlTo, short restrictDiscoveriesTo, double priority, byte renderType, byte renderTypeForChildren)
        {
            return InsertCrawlRequest(created, absoluteUri0, absoluteUri1, absoluteUri2, 1, depth, restrictCrawlTo, restrictDiscoveriesTo, priority, renderType, renderTypeForChildren);
        }

        /// <summary>
        /// 	Inserts a row into table 'CrawlRequests'.
        /// 	See: http://arachnode.net/forums/p/564/10762.aspx#10762 if you experience difficulty with this function.
        /// 	This is an advanced function, primarily intended for use by the Crawler.
        /// </summary>
        /// <param name = "created">The created.</param>
        /// <param name = "absoluteUri1">The absolute uri1.</param>
        /// <param name = "absoluteUri2">The absolute uri2.</param>
        /// <param name = "currentDepth">The current depth.</param>
        /// <param name = "maximumDepth">The maximum depth.</param>
        /// <param name = "restrictCrawlTo">The restrict crawl to.  Restricting a Crawl to a specific UriQualificationType means that the Crawl will only crawl WebPages that match the initial UriQualificationType.</param>
        /// <param name = "restrictDiscoveriesTo">The restrict discoveries to. Restricting a Crawl's Discoveries to a specific UriQualificationType means that the Crawl will only collect Discoveries that match the initial UriQualificationType.</param>
        /// <param name = "priority">The priority.</param>
        /// <returns></returns>
        public long? InsertCrawlRequest(DateTime created, string absoluteUri0, string absoluteUri1, string absoluteUri2, int currentDepth, int maximumDepth, short restrictCrawlTo, short restrictDiscoveriesTo, double priority, byte renderType, byte renderTypeForChildren)
        {
            try
            {
                if (ApplicationSettings.InsertCrawlRequests)
                {
                    _queriesTableAdapter.InsertCrawlRequest(created, absoluteUri0, absoluteUri1, absoluteUri2, currentDepth, maximumDepth, restrictCrawlTo, restrictDiscoveriesTo, priority, renderType, renderTypeForChildren, ref _lastCrawlRequestID);

                    Counters.GetInstance().CrawlRequestInserted();
                }

                return _lastCrawlRequestID;
            }
            catch (Exception exception)
            {
                InsertException(absoluteUri1, absoluteUri2, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Inserts an AbsoluteUri into the Discoveries table.
        /// </summary>
        /// <param name = "discoveryID">The discovery ID.</param>
        /// <param name = "absoluteUri">The absolute URI.</param>
        /// <param name = "discoveryStateID">The discovery state ID.</param>
        /// <param name = "discoveryTypeID">The discovery type ID.</param>
        /// <param name = "expectFileOrImage">if set to <c>true</c> [expect file or image].</param>
        /// <param name = "numberOfTimesDiscovered">The number of times discovered.</param>
        public void InsertDiscovery(long? discoveryID, string absoluteUri, int discoveryStateID, int discoveryTypeID, bool expectFileOrImage, int numberOfTimesDiscovered)
        {
            try
            {
                if (ApplicationSettings.InsertDiscoveries)
                {
                    _queriesTableAdapter.InsertDiscovery(discoveryID, absoluteUri, discoveryStateID, discoveryTypeID, expectFileOrImage, numberOfTimesDiscovered);

                    //TODO: Wrong counter.  Needs to be  ArachnodeDAO 'Inserted'.
                    //Counters.GetInstance().DiscoveryAdded();
                }
            }
            catch (Exception exception)
            {
                InsertException(absoluteUri, null, exception, false);
            }
        }

        /// <summary>
        /// 	Inserts a row into table 'DisallowedAbsoluteUris'.
        /// </summary>
        /// <param name = "contentTypeID">The content type ID.</param>
        /// <param name = "discoveryTypeID">The discovery type ID.</param>
        /// <param name = "webPageAbsoluteUri">The web page absolute URI.</param>
        /// <param name = "disallowedAbsoluteUriAbsoluteUri">The disallowed absolute URI absolute URI.</param>
        /// <param name = "reason">The reason.</param>
        public void InsertDisallowedAbsoluteUri(int contentTypeID, int discoveryTypeID, string webPageAbsoluteUri, string disallowedAbsoluteUriAbsoluteUri, string reason)
        {
            try
            {
                _queriesTableAdapter.InsertDisallowedAbsoluteUri(contentTypeID, discoveryTypeID, webPageAbsoluteUri, disallowedAbsoluteUriAbsoluteUri, reason, ApplicationSettings.ClassifyAbsoluteUris, ref _lastDisallowedAbsoluteUriID);

                Counters.GetInstance().DisallowedAbsoluteUriInserted();
            }

            catch (Exception exception)
            {
                InsertException(null, null, exception, false);
            }
        }

        /// <summary>
        /// 	Inserts a row into table 'DisallowedAbsoluteUris_Discoveries'.
        /// </summary>
        /// <param name = "webPageAbsoluteUri">The AbsoluteUri of the WebPage where the DisallowedAbsoluteUri was discovered.</param>
        /// <param name = "disallowedAbsoluteUriAbsoluteUri">The AbsoluteUri of the DisallowedAbsoluteUri.</param>
        public void InsertDisallowedAbsoluteUriDiscovery(string webPageAbsoluteUri, string disallowedAbsoluteUriAbsoluteUri)
        {
            try
            {
                _queriesTableAdapter.InsertDisallowedAbsoluteUriDiscovery(webPageAbsoluteUri, disallowedAbsoluteUriAbsoluteUri);

                Counters.GetInstance().DisallowedAbsoluteUriDiscoveryInserted();
            }
            catch (Exception exception)
            {
                InsertException(webPageAbsoluteUri, disallowedAbsoluteUriAbsoluteUri, exception, false);
            }
        }

        /// <summary>
        /// 	Inserts a row into table 'EmailAddresses'.
        /// </summary>
        /// <param name = "webPageAbsoluteUri">The AbsoluteUri of the WebPage where the EmailAddress was discovered.</param>
        /// <param name = "emailAddressAbsoluteUri">The AbsoluteUri of the EmailAddress.</param>
        /// <returns></returns>
        public long? InsertEmailAddress(string webPageAbsoluteUri, string emailAddressAbsoluteUri)
        {
            try
            {
                _queriesTableAdapter.InsertEmailAddress(webPageAbsoluteUri, emailAddressAbsoluteUri, ApplicationSettings.ClassifyAbsoluteUris, ref _lastEmailAddressID);

                Counters.GetInstance().EmailAddressInserted();

                return _lastEmailAddressID;
            }

            catch (Exception exception)
            {
                InsertException(webPageAbsoluteUri, null, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Inserts a row into table 'EmailAddresses_Discoveries'.
        /// </summary>
        /// <param name = "webPageAbsoluteUri">The AbsoluteUri of the WebPage where the EmailAddress was discovered.</param>
        /// <param name = "emailAddressAbsoluteUri">The AbsoluteUri of the EmailAddress.</param>
        public void InsertEmailAddressDiscovery(string webPageAbsoluteUri, string emailAddressAbsoluteUri)
        {
            try
            {
                _queriesTableAdapter.InsertEmailAddressDiscovery(webPageAbsoluteUri, emailAddressAbsoluteUri);

                Counters.GetInstance().EmailAddressDiscoveryInserted();
            }

            catch (Exception exception)
            {
                InsertException(webPageAbsoluteUri, null, exception, false);
            }
        }

        /// <summary>
        /// 	Inserts a row into table 'Exceptions'.
        /// </summary>
        /// <param name = "absoluteUri1">The absolute uri1.</param>
        /// <param name = "absoluteUri2">The absolute uri2.</param>
        /// <param name = "exception">The exception.</param>
        /// <param name = "insertExceptionInWindowsApplicationLog">if set to <c>true</c> [insert exception in windows event log].</param>
        /// <returns>
        /// 	The last ExceptionID returned from table 'Exceptions'.
        /// </returns>
        public long? InsertException(string absoluteUri1, string absoluteUri2, Exception exception, bool insertExceptionInWindowsApplicationLog)
        {
            try
            {
                if (ApplicationSettings.EnableConsoleOutput && ApplicationSettings.VerboseOutput)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    Console.WriteLine("InsertException: " + exception.Message);

                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                if (ApplicationSettings.InsertExceptions)
                {
                    _queriesTableAdapter.InsertException(absoluteUri1, absoluteUri2, exception.HelpLink, exception.Message, exception.Source, exception.StackTrace, ApplicationSettings.ClassifyAbsoluteUris, ref _lastExceptionID);
                }

                if (insertExceptionInWindowsApplicationLog)
                {
                    InsertExceptionIntoWindowsApplicationLog(exception);
                }

                Counters.GetInstance().ExceptionInserted();

                _lastExceptionMessage = exception.Message;

                return _lastExceptionID;
            }
            catch (Exception exception2)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                if (ApplicationSettings.EnableConsoleOutput)
                {
                    Console.WriteLine("InsertException: " + exception.Message);
                }

                Console.ForegroundColor = ConsoleColor.Gray;

                Console.Beep();
                Console.WriteLine(exception2);

                InsertExceptionIntoWindowsApplicationLog(exception2);
            }

            return null;
        }

        /// <summary>
        /// 	Inserts the exception into the Windows 'Application' log.
        /// </summary>
        /// <param name = "exception">The exception.</param>
        public static void InsertExceptionIntoWindowsApplicationLog(Exception exception)
        {
            new EventLog("Application", Environment.MachineName, "arachnode.net").WriteEntry(exception.Message + "\n\n" + exception.StackTrace, EventLogEntryType.Error);
        }

        /// <summary>
        /// 	Inserts a row into table 'Files'.
        /// </summary>
        /// <param name = "webPageAbsoluteUri">The AbsoluteUri of the WebPage where the File was discovered.</param>
        /// <param name = "fileAbsoluteUri">The AbsoluteUri of the File.</param>
        /// <param name = "responseHeaders">The ResponseHeaders returned from the HttpRequest.</param>
        /// <param name = "source">The binary data of the File.</param>
        /// <param name = "fullTextIndexType">Full type of the text index.</param>
        /// <returns></returns>
        public long? InsertFile(string webPageAbsoluteUri, string fileAbsoluteUri, string responseHeaders, byte[] source, string fullTextIndexType)
        {
            try
            {
                _queriesTableAdapter.InsertFile(webPageAbsoluteUri, fileAbsoluteUri, responseHeaders, source, fullTextIndexType, ApplicationSettings.ClassifyAbsoluteUris, ref _lastFileID);

                Counters.GetInstance().FileInserted();

                return _lastFileID;
            }
            catch (Exception exception)
            {
                InsertException(webPageAbsoluteUri, fileAbsoluteUri, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Inserts a row into table 'Files_Discoveries'.
        /// </summary>
        /// <param name = "webPageAbsoluteUri">The AbsoluteUri of the WebPage where the File was discovered.</param>
        /// <param name = "fileAbsoluteUri">The AbsoluteUri of the File.</param>
        public void InsertFileDiscovery(string webPageAbsoluteUri, string fileAbsoluteUri)
        {
            try
            {
                _queriesTableAdapter.InsertFileDiscovery(webPageAbsoluteUri, fileAbsoluteUri);

                Counters.GetInstance().FileDiscoveryInserted();
            }
            catch (Exception exception)
            {
                InsertException(webPageAbsoluteUri, fileAbsoluteUri, exception, false);
            }
        }

        /// <summary>
        /// 	Inserts a row into table 'Files_MetaData'.
        /// </summary>
        /// <param name = "fileAbsoluteUri">The AbsoluteUri of the File.</param>
        /// <param name = "fileID">The FileID of the File.</param>
        public void InsertFileMetaData(string fileAbsoluteUri, long fileID)
        {
            try
            {
                _queriesTableAdapter.InsertFileMetaData(fileID);

                Counters.GetInstance().FileMetaDataInserted();
            }
            catch (Exception exception)
            {
                InsertException(null, fileAbsoluteUri, exception, false);
            }
        }

        /// <summary>
        /// 	Inserts a row into table 'HyperLinks'.
        /// </summary>
        /// <param name = "webPageAbsoluteUri">The AbsoluteUri of the WebPage where the HyperLink was discovered.</param>
        /// <param name = "hyperLinkAbsoluteUri">The AbsoluteUri of the HyperLink.</param>
        /// <returns></returns>
        public long? InsertHyperLink(string webPageAbsoluteUri, string hyperLinkAbsoluteUri)
        {
            try
            {
                _queriesTableAdapter.InsertHyperLink(webPageAbsoluteUri, hyperLinkAbsoluteUri, ApplicationSettings.ClassifyAbsoluteUris, ref _lastHyperLinkID);

                Counters.GetInstance().HyperLinkInserted();

                return _lastHyperLinkID;
            }
            catch (Exception exception)
            {
                //ANODET: Suppress the 'PRIMARY KEY' exceptions - OK to ignore since we don't lock/transact on the table.
                InsertException(webPageAbsoluteUri, hyperLinkAbsoluteUri, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Inserts a row into table 'HyperLinks_Discoveries'.
        /// </summary>
        /// <param name = "webPageAbsoluteUri">The AbsoluteUri of the WebPage where the HyperLink was discovered.</param>
        /// <param name = "hyperLinkAbsoluteUri">The AbsoluteUri of the HyperLink.</param>
        public void InsertHyperLinkDiscovery(string webPageAbsoluteUri, string hyperLinkAbsoluteUri)
        {
            try
            {
                _queriesTableAdapter.InsertHyperLinkDiscovery(webPageAbsoluteUri, hyperLinkAbsoluteUri);

                Counters.GetInstance().HyperLinkDiscoveryInserted();
            }
            catch (Exception exception)
            {
                InsertException(webPageAbsoluteUri, hyperLinkAbsoluteUri, exception, false);
            }
        }

        /// <summary>
        /// 	Inserts a row into table 'Images'.
        /// </summary>
        /// <param name = "webPageAbsoluteUri">The AbsoluteUri of the WebPage where the Image was discovered.</param>
        /// <param name = "imageAbsoluteUri">The AbsoluteUri of the Image.</param>
        /// <param name = "responseHeaders">The ResponseHeaders returned from the HttpRequest.</param>
        /// <param name = "source">The binary data of the Image.</param>
        /// <param name = "fullTextIndexType">Full type of the text index.</param>
        /// <returns></returns>
        public long? InsertImage(string webPageAbsoluteUri, string imageAbsoluteUri, string responseHeaders, byte[] source, string fullTextIndexType)
        {
            try
            {
                _queriesTableAdapter.InsertImage(webPageAbsoluteUri, imageAbsoluteUri, responseHeaders, source, fullTextIndexType, ApplicationSettings.ClassifyAbsoluteUris, ref _lastImageID);

                Counters.GetInstance().ImageInserted();

                return _lastImageID;
            }
            catch (Exception exception)
            {
                InsertException(webPageAbsoluteUri, imageAbsoluteUri, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Inserts a row into table 'Images_Discoveries'.
        /// </summary>
        /// <param name = "webPageAbsoluteUri">The AbsoluteUri of the WebPage where the Image was discovered.</param>
        /// <param name = "imageAbsoluteUri">The AbsoluteUri of the Image.</param>
        public void InsertImageDiscovery(string webPageAbsoluteUri, string imageAbsoluteUri)
        {
            try
            {
                _queriesTableAdapter.InsertImageDiscovery(webPageAbsoluteUri, imageAbsoluteUri);

                Counters.GetInstance().ImageDiscoveryInserted();
            }
            catch (Exception exception)
            {
                InsertException(webPageAbsoluteUri, imageAbsoluteUri, exception, false);
            }
        }

        /// <summary>
        /// 	Inserts a row into table 'Images_MetaData'.
        /// </summary>
        /// <param name = "imageAbsoluteUri">The AbsoluteUri of the Image.</param>
        /// <param name = "imageID">The ImageID of the Image.</param>
        /// <param name = "exifData">The ExifData of the Image.</param>
        /// <param name = "flags">The Flags of the Image.</param>
        /// <param name = "height">The Height of the Image.</param>
        /// <param name = "horizontalResolution">The HorizontalResolution of the Image.</param>
        /// <param name = "verticalResolution">The VerticalResoution of the Image.</param>
        /// <param name = "width">The Width of the Image.</param>
        public void InsertImageMetaData(string imageAbsoluteUri, long imageID, string exifData, int flags, int height, double horizontalResolution, double verticalResolution, int width)
        {
            try
            {
                _queriesTableAdapter.InsertImageMetaData(imageID, exifData, flags, height, horizontalResolution, verticalResolution, width);

                Counters.GetInstance().ImageMetaDataInserted();
            }
            catch (Exception exception)
            {
                InsertException(null, imageAbsoluteUri, exception, false);
            }
        }

        /// <summary>
        /// 	Inserts a row into table 'WebPages'.
        /// </summary>
        /// <param name = "webPageAbsoluteUri">The web page absolute URI.</param>
        /// <param name = "responseHeaders">The response headers.</param>
        /// <param name = "source">The source.</param>
        /// <param name = "codePage">The code page.</param>
        /// <param name = "fullTextIndexType">Full type of the text index.</param>
        /// <param name = "crawlDepth">The crawl depth.</param>
        /// <returns></returns>
        public long? InsertWebPage(string webPageAbsoluteUri, string responseHeaders, byte[] source, int codePage, string fullTextIndexType, int crawlDepth)
        {
            try
            {
                _queriesTableAdapter.InsertWebPage(webPageAbsoluteUri, responseHeaders, source, codePage, fullTextIndexType, crawlDepth, ApplicationSettings.ClassifyAbsoluteUris, ref _lastWebPageID);

                Counters.GetInstance().WebPageInserted();

                return _lastWebPageID;
            }
            catch (Exception exception)
            {
                InsertException(webPageAbsoluteUri, null, exception, false);
            }

            return null;
        }

        /// <summary>
        /// 	Inserts a row into table 'WebPages_MetaData'.
        /// </summary>
        /// <param name = "webPageID">The web page ID.</param>
        /// <param name = "webPageAbsoluteUri">The web page absolute URI.</param>
        /// <param name = "text">The text.</param>
        /// <param name = "xml">The XML.</param>
        public void InsertWebPageMetaData(long webPageID, string webPageAbsoluteUri, byte[] text, string xml)
        {
            try
            {
                _queriesTableAdapter.InsertWebPageMetaData(webPageID, text, xml);

                Counters.GetInstance().WebPageMetaDataInserted();
            }

            catch (Exception exception)
            {
                InsertException(webPageAbsoluteUri, null, exception, false);
            }
        }
    }
}