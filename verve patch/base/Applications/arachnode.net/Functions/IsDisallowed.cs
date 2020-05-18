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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

#endregion

public partial class UserDefinedFunctions
{
    #region DisallowedType enum

    public enum DisallowedType
    {
        None = 0,
        AbsoluteUri = 1,
        Domain = 2,
        Extension = 3,
        FileExtension = 4,
        ResponseHeaders = 5,
        Scheme = 6,
        Source = 7
    }

    #endregion

    private static readonly object _isDisallowedLock = new object();

    private static string _connectionString;

    private static Dictionary<string, string> _disallowedAbsoluteUris;
    private static Dictionary<string, string> _disallowedDomains;
    private static Dictionary<string, string> _disallowedExtensions;
    private static Dictionary<string, string> _disallowedFileExtensions;
    private static Dictionary<string, string> _disallowedHosts;
    private static Dictionary<string, string> _disallowedSchemes;
    private static Dictionary<string, string> _disallowedWordsForAbsoluteUri;
    private static Dictionary<string, string> _disallowedWordsForResponseHeaders;
    private static Dictionary<string, string> _disallowedWordsForSource;

    /// <summary>
    /// 	Gets the connection string.
    /// </summary>
    /// <value>The connection string.</value>
    public static string ConnectionString
    {
        get
        {
            if (_connectionString == null)
            {
                if (AppDomain.CurrentDomain.FriendlyName.StartsWith("arachnode.net"))
                {
                    //being called from the database...
                    _connectionString = "Context Connection=true";
                }
                else
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["ArachnodeConnectionString"].ConnectionString;
                }
            }
            return _connectionString;
        }
        set { _connectionString = value; }
    }

    /// <summary>
    /// 	Gets or sets the disallowed absolute uris.
    /// </summary>
    /// <value>The disallowed absolute uris.</value>
    public static Dictionary<string, string> DisallowedAbsoluteUris
    {
        get
        {
            if (_disallowedAbsoluteUris == null)
            {
                RefreshDisallowed();
            }
            return _disallowedAbsoluteUris;
        }
        set { _disallowedAbsoluteUris = value; }
    }

    /// <summary>
    /// 	Gets or sets the disallowed domains.
    /// </summary>
    /// <value>The disallowed domains.</value>
    public static Dictionary<string, string> DisallowedDomains
    {
        get
        {
            if (_disallowedDomains == null)
            {
                RefreshDisallowed();
            }
            return _disallowedDomains;
        }
        set { _disallowedDomains = value; }
    }

    /// <summary>
    /// 	Gets or sets the disallowed extensions.
    /// </summary>
    /// <value>The disallowed extensions.</value>
    public static Dictionary<string, string> DisallowedExtensions
    {
        get
        {
            if (_disallowedExtensions == null)
            {
                RefreshDisallowed();
            }
            return _disallowedExtensions;
        }
        set { _disallowedExtensions = value; }
    }

    /// <summary>
    /// 	Gets or sets the disallowed file extensions.
    /// </summary>
    /// <value>The disallowed file extensions.</value>
    public static Dictionary<string, string> DisallowedFileExtensions
    {
        get
        {
            if (_disallowedFileExtensions == null)
            {
                RefreshDisallowed();
            }
            return _disallowedFileExtensions;
        }
        set { _disallowedFileExtensions = value; }
    }

    /// <summary>
    /// 	Gets or sets the disallowed hosts.
    /// </summary>
    /// <value>The disallowed hosts.</value>
    public static Dictionary<string, string> DisallowedHosts
    {
        get
        {
            if (_disallowedHosts == null)
            {
                RefreshDisallowed();
            }
            return _disallowedHosts;
        }
        set { _disallowedHosts = value; }
    }

    /// <summary>
    /// 	Gets or sets the disallowed schemes.
    /// </summary>
    /// <value>The disallowed schemes.</value>
    public static Dictionary<string, string> DisallowedSchemes
    {
        get
        {
            if (_disallowedSchemes == null)
            {
                RefreshDisallowed();
            }
            return _disallowedSchemes;
        }
        set { _disallowedSchemes = value; }
    }

    /// <summary>
    /// 	Gets or sets the disallowed words for absolute URI.
    /// </summary>
    /// <value>The disallowed words for absolute URI.</value>
    public static Dictionary<string, string> DisallowedWordsForAbsoluteUri
    {
        get
        {
            if (_disallowedWordsForAbsoluteUri == null)
            {
                RefreshDisallowed();
            }
            return _disallowedWordsForAbsoluteUri;
        }
        set { _disallowedWordsForAbsoluteUri = value; }
    }

    /// <summary>
    /// 	Gets or sets the disallowed words for response headers.
    /// </summary>
    /// <value>The disallowed words for response headers.</value>
    public static Dictionary<string, string> DisallowedWordsForResponseHeaders
    {
        get
        {
            if (_disallowedWordsForResponseHeaders == null)
            {
                RefreshDisallowed();
            }
            return _disallowedWordsForResponseHeaders;
        }
        set { _disallowedWordsForResponseHeaders = value; }
    }

    /// <summary>
    /// 	Gets or sets the disallowed words for source.
    /// </summary>
    /// <value>The disallowed words for source.</value>
    public static Dictionary<string, string> DisallowedWordsForSource
    {
        get
        {
            if (_disallowedWordsForSource == null)
            {
                RefreshDisallowed();
            }
            return _disallowedWordsForSource;
        }
        set { _disallowedWordsForSource = value; }
    }

    /// <summary>
    /// 	Determines whether [is disallowed for absolute URI] [the specified absolute URI].
    /// </summary>
    /// <param name = "absoluteUri">The absolute URI.</param>
    /// <param name = "checkIsDisallowedForWords">The check is disallowed for words.</param>
    /// <param name = "refreshDisallowed">The refresh disallowed.</param>
    /// <returns>
    /// 	<c>true</c> if [is disallowed for absolute URI] [the specified absolute URI]; otherwise, <c>false</c>.
    /// </returns>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static bool IsDisallowedForAbsoluteUri(SqlString absoluteUri, SqlBoolean checkIsDisallowedForWords, SqlBoolean refreshDisallowed)
    {
        if (refreshDisallowed || _disallowedAbsoluteUris == null)
        {
            RefreshDisallowed();
        }

        if (!absoluteUri.IsNull && _disallowedAbsoluteUris.ContainsKey(absoluteUri.Value.ToLower()))
        {
            return true;
        }

        if (checkIsDisallowedForWords)
        {
            if (IsDisallowedForWords(absoluteUri, false))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 	Determines whether [is disallowed for domain] [the specified absolute URI].
    /// </summary>
    /// <param name = "absoluteUri">The absolute URI.</param>
    /// <param name = "checkIsDisallowedForWords">The check is disallowed for words.</param>
    /// <param name = "refreshDisallowed">The refresh disallowed.</param>
    /// <returns>
    /// 	<c>true</c> if [is disallowed for domain] [the specified absolute URI]; otherwise, <c>false</c>.
    /// </returns>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static bool IsDisallowedForDomain(SqlString absoluteUri, SqlBoolean checkIsDisallowedForWords, SqlBoolean refreshDisallowed)
    {
        if (refreshDisallowed || _disallowedDomains == null)
        {
            RefreshDisallowed();
        }

        string domain = ExtractDomain(absoluteUri).Value;

        if (domain != null && _disallowedDomains.ContainsKey(domain.ToLower()))
        {
            return true;
        }

        if (checkIsDisallowedForWords)
        {
            if (IsDisallowedForWords(domain, false))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 	Determines whether [is disallowed for extension] [the specified absolute URI].
    /// </summary>
    /// <param name = "absoluteUri">The absolute URI.</param>
    /// <param name = "checkIsDisallowedForWords">The check is disallowed for words.</param>
    /// <param name = "refreshDisallowed">The refresh disallowed.</param>
    /// <returns>
    /// 	<c>true</c> if [is disallowed for extension] [the specified absolute URI]; otherwise, <c>false</c>.
    /// </returns>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static bool IsDisallowedForExtension(SqlString absoluteUri, SqlBoolean checkIsDisallowedForWords, SqlBoolean refreshDisallowed)
    {
        if (refreshDisallowed || _disallowedExtensions == null)
        {
            RefreshDisallowed();
        }

        string extension = ExtractExtension(absoluteUri, false).Value;

        if (extension != null && _disallowedExtensions.ContainsKey(extension.ToLower()))
        {
            return true;
        }

        if (checkIsDisallowedForWords)
        {
            if (IsDisallowedForWords(extension, false))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 	Determines whether [is disallowed for file extension] [the specified absolute URI].
    /// </summary>
    /// <param name = "absoluteUri">The absolute URI.</param>
    /// <param name = "checkIsDisallowedForWords">The check is disallowed for words.</param>
    /// <param name = "refreshDisallowed">The refresh disallowed.</param>
    /// <returns>
    /// 	<c>true</c> if [is disallowed for file extension] [the specified absolute URI]; otherwise, <c>false</c>.
    /// </returns>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static bool IsDisallowedForFileExtension(SqlString absoluteUri, SqlBoolean checkIsDisallowedForWords, SqlBoolean refreshDisallowed)
    {
        if (refreshDisallowed || _disallowedFileExtensions == null)
        {
            RefreshDisallowed();
        }

        string fileExtension = ExtractFileExtension(absoluteUri).Value;

        if (fileExtension != null && _disallowedFileExtensions.ContainsKey(fileExtension.ToLower()))
        {
            return true;
        }

        if (checkIsDisallowedForWords)
        {
            if (IsDisallowedForWords(fileExtension, false))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 	Determines whether [is disallowed for host] [the specified absolute URI].
    /// </summary>
    /// <param name = "absoluteUri">The absolute URI.</param>
    /// <param name = "checkIsDisallowedForWords">The check is disallowed for words.</param>
    /// <param name = "refreshDisallowed">The refresh disallowed.</param>
    /// <returns>
    /// 	<c>true</c> if [is disallowed for host] [the specified absolute URI]; otherwise, <c>false</c>.
    /// </returns>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static bool IsDisallowedForHost(SqlString absoluteUri, SqlBoolean checkIsDisallowedForWords, SqlBoolean refreshDisallowed)
    {
        if (refreshDisallowed || _disallowedHosts == null)
        {
            RefreshDisallowed();
        }

        string host = ExtractHost(absoluteUri).Value;

        if (host != null && _disallowedHosts.ContainsKey(host.ToLower()))
        {
            return true;
        }

        if (checkIsDisallowedForWords)
        {
            if (IsDisallowedForWords(host, false))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 	Determines whether [is disallowed for response headers] [the specified response headers].
    /// </summary>
    /// <param name = "responseHeaders">The response headers.</param>
    /// <param name = "refreshDisallowed">The refresh disallowed.</param>
    /// <returns>
    /// 	<c>true</c> if [is disallowed for response headers] [the specified response headers]; otherwise, <c>false</c>.
    /// </returns>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static bool IsDisallowedForResponseHeaders(SqlString responseHeaders, SqlBoolean refreshDisallowed)
    {
        if (refreshDisallowed || _disallowedWordsForResponseHeaders == null)
        {
            RefreshDisallowed();
        }

        foreach (string disallowedWord in _disallowedWordsForResponseHeaders.Values)
        {
            if (Regex.IsMatch(responseHeaders.Value, disallowedWord, RegexOptions.IgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 	Determines whether [is disallowed for scheme] [the specified absolute URI].
    /// </summary>
    /// <param name = "absoluteUri">The absolute URI.</param>
    /// <param name = "checkIsDisallowedForWords">The check is disallowed for words.</param>
    /// <param name = "refreshDisallowed">The refresh disallowed.</param>
    /// <returns>
    /// 	<c>true</c> if [is disallowed for scheme] [the specified absolute URI]; otherwise, <c>false</c>.
    /// </returns>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static bool IsDisallowedForScheme(SqlString absoluteUri, SqlBoolean checkIsDisallowedForWords, SqlBoolean refreshDisallowed)
    {
        if (refreshDisallowed || _disallowedSchemes == null)
        {
            RefreshDisallowed();
        }

        string scheme = ExtractScheme(absoluteUri, false).Value;

        if (scheme != null && _disallowedHosts.ContainsKey(scheme.ToLower()))
        {
            return true;
        }

        if (checkIsDisallowedForWords)
        {
            if (IsDisallowedForWords(scheme, false))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 	Determines whether [is disallowed for source] [the specified source].
    /// </summary>
    /// <param name = "source">The source.</param>
    /// <param name = "refreshDisallowed">The refresh disallowed.</param>
    /// <returns>
    /// 	<c>true</c> if [is disallowed for source] [the specified source]; otherwise, <c>false</c>.
    /// </returns>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static bool IsDisallowedForSource(SqlString source, SqlBoolean refreshDisallowed)
    {
        if (refreshDisallowed || _disallowedWordsForSource == null)
        {
            RefreshDisallowed();
        }

        foreach (string disallowedWord in _disallowedWordsForSource.Values)
        {
            if (Regex.IsMatch(source.Value, disallowedWord, RegexOptions.IgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 	Determines whether [is disallowed for words] [the specified absolute URI].
    /// </summary>
    /// <param name = "absoluteUriOrAbsoluteUriClassification">The absolute URI.</param>
    /// <param name = "refreshDisallowed">The refresh disallowed.</param>
    /// <returns>
    /// 	<c>true</c> if [is disallowed for words] [the specified absolute URI]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsDisallowedForWords(SqlString absoluteUriOrAbsoluteUriClassification, SqlBoolean refreshDisallowed)
    {
        if (refreshDisallowed || _disallowedAbsoluteUris == null)
        {
            RefreshDisallowed();
        }

        if (absoluteUriOrAbsoluteUriClassification.Value != null)
        {
            foreach (string disallowedWord in _disallowedWordsForAbsoluteUri.Values)
            {
                if (Regex.IsMatch(absoluteUriOrAbsoluteUriClassification.Value, disallowedWord, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 	Refreshes all Disallowed patterns.  Call this method before calling IsDisallowed to avoid a thread race condition.  This function pulls from DisallowedAbsoluteUris and cfg.Disallowed*.  DisallowedAbsoluteUris is populated by Crawl Rules.
    /// </summary>
    /// <returns></returns>
    [SqlFunction(DataAccess = DataAccessKind.Read)]
    public static SqlBoolean RefreshDisallowed()
    {
        lock (_isDisallowedLock)
        {
            if (_disallowedAbsoluteUris == null)
            {
                _disallowedAbsoluteUris = new Dictionary<string, string>();
            }
            else
            {
                _disallowedAbsoluteUris.Clear();
            }

            if (_disallowedDomains == null)
            {
                _disallowedDomains = new Dictionary<string, string>();
            }
            else
            {
                _disallowedDomains.Clear();
            }

            if (_disallowedExtensions == null)
            {
                _disallowedExtensions = new Dictionary<string, string>();
            }
            else
            {
                _disallowedExtensions.Clear();
            }

            if (_disallowedFileExtensions == null)
            {
                _disallowedFileExtensions = new Dictionary<string, string>();
            }
            else
            {
                _disallowedExtensions.Clear();
            }

            if (_disallowedHosts == null)
            {
                _disallowedHosts = new Dictionary<string, string>();
            }
            else
            {
                _disallowedHosts.Clear();
            }

            if (_disallowedSchemes == null)
            {
                _disallowedSchemes = new Dictionary<string, string>();
            }
            else
            {
                _disallowedSchemes.Clear();
            }

            if (_disallowedWordsForAbsoluteUri == null)
            {
                _disallowedWordsForAbsoluteUri = new Dictionary<string, string>();
            }
            else
            {
                _disallowedWordsForAbsoluteUri.Clear();
            }

            if (_disallowedWordsForResponseHeaders == null)
            {
                _disallowedWordsForResponseHeaders = new Dictionary<string, string>();
            }
            else
            {
                _disallowedWordsForResponseHeaders.Clear();
            }

            if (_disallowedWordsForSource == null)
            {
                _disallowedWordsForSource = new Dictionary<string, string>();
            }
            else
            {
                _disallowedWordsForSource.Clear();
            }

            SqlCommand sqlCommand = new SqlCommand("", new SqlConnection(ConnectionString));

            sqlCommand.Connection.Open();

            //Select the Configuration to set the SqlCommandTimeout for the UDF's.
            sqlCommand.CommandText = "dbo.arachnode_omsp_Configuration_SELECT";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    if (sqlDataReader.GetString(1) == "SqlCommandTimeoutInMinutes")
                    {
                        sqlCommand.CommandTimeout = int.Parse(sqlDataReader.GetString(2))*60;
                    }
                }
            }

            //Select all DisallowedAbsoluteUris that were disallowed due to WebExceptions or CrawlRules.
            sqlCommand.CommandText = "dbo.arachnode_omsp_DisallowedAbsoluteUris_SELECT";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    if (!_disallowedAbsoluteUris.ContainsKey(sqlDataReader.GetString(0).ToLower()))
                    {
                        _disallowedAbsoluteUris.Add(sqlDataReader.GetString(0).ToLower(), null);
                    }
                }
            }

            sqlCommand.CommandText = "dbo.arachnode_omsp_DisallowedDomains_SELECT";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    if (!_disallowedDomains.ContainsKey(sqlDataReader.GetString(0).ToLower()))
                    {
                        _disallowedDomains.Add(sqlDataReader.GetString(0).ToLower(), null);
                    }
                }
            }

            sqlCommand.CommandText = "dbo.arachnode_omsp_DisallowedExtensions_SELECT";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    if (!_disallowedExtensions.ContainsKey(sqlDataReader.GetString(0).ToLower()))
                    {
                        _disallowedExtensions.Add(sqlDataReader.GetString(0).ToLower(), null);
                    }
                }
            }

            sqlCommand.CommandText = "dbo.arachnode_omsp_DisallowedFileExtensions_SELECT";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    if (!_disallowedFileExtensions.ContainsKey(sqlDataReader.GetString(0).ToLower()))
                    {
                        _disallowedFileExtensions.Add(sqlDataReader.GetString(0).ToLower(), null);
                    }
                }
            }

            sqlCommand.CommandText = "dbo.arachnode_omsp_DisallowedHosts_SELECT";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    if (!_disallowedHosts.ContainsKey(sqlDataReader.GetString(0).ToLower()))
                    {
                        _disallowedHosts.Add(sqlDataReader.GetString(0), null);
                    }
                }
            }

            sqlCommand.CommandText = "dbo.arachnode_omsp_DisallowedSchemes_SELECT";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    if (!_disallowedSchemes.ContainsKey(sqlDataReader.GetString(0).ToLower()))
                    {
                        _disallowedSchemes.Add(sqlDataReader.GetString(0), null);
                    }
                }
            }

            sqlCommand.CommandText = "dbo.arachnode_omsp_DisallowedWords_SELECT";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    if (sqlDataReader.GetBoolean(1))
                    {
                        if (!_disallowedWordsForAbsoluteUri.ContainsKey(sqlDataReader.GetString(0).ToLower()))
                        {
                            _disallowedWordsForAbsoluteUri.Add(sqlDataReader.GetString(0).ToLower().Replace(" ", "%20"), sqlDataReader.GetString(0).ToLower().Replace(" ", "%20"));
                        }
                    }

                    if (sqlDataReader.GetBoolean(2))
                    {
                        if (!_disallowedWordsForResponseHeaders.ContainsKey(sqlDataReader.GetString(0).ToLower()))
                        {
                            _disallowedWordsForResponseHeaders.Add(sqlDataReader.GetString(0).ToLower(), sqlDataReader.GetString(0).ToLower());
                        }
                    }

                    if (sqlDataReader.GetBoolean(3))
                    {
                        if (!_disallowedWordsForSource.ContainsKey(sqlDataReader.GetString(0).ToLower()))
                        {
                            _disallowedWordsForSource.Add(sqlDataReader.GetString(0).ToLower(), sqlDataReader.GetString(0).ToLower());
                        }
                    }
                }
            }

            sqlCommand.Connection.Close();
        }

        return true;
    }
} ;