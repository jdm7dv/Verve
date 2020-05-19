//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Bio.Util.Logging;

namespace Bio.Util
{
    /// <summary>
    /// A flexible options class, implemented as a dictionary that maps key strings
    /// to lists of one or more value strings. The static Options.Global object holds
    /// library-wide options.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Options.Global is the storehouse for options that are effective across all
        /// of the MBF library. The settings can be persisted across sessions if desired.
        /// </summary>
        public static Options Global = new Options();

        private Dictionary<string, List<string>> _data = new Dictionary<string, List<string>>();

        /// <summary>
        /// Parse one or more options strings into a dictionary, allowing multiple values per key.
        /// The format of options strings is flexible. One string can contain one, or more than
        /// one setting (separated by ';' if multiple). Each setting is either of the form 
        /// "key=value", or simply "key" (resulting in a null value, as does "key=".
        /// </summary>
        /// <param name="args">A variable list of options strings.</param>
        public Options(params string[] args)
        {
            _data = new Dictionary<string, List<string>>();
            foreach (string s in args)
            {
                int len = s.Length;
                string key, val;
                int start = 0;
                while (start < len - 1)
                {
                    int eq = s.IndexOf('=', start) - start;
                    int sc = s.IndexOf(';', start) - start;
                    if (sc == 0)
                    {
                        ++start;
                        continue;   // null setting
                    }
                    if (sc == 0 || eq == 0)
                    {
                        string message = String.Format(CultureInfo.InvariantCulture,
                            "Options: Bad string '{0}'.",
                            s);
                        Trace.Report(message);
                        throw new ArgumentException(message);
                    }
                    int end = (sc < 0 ? len : sc + start);
                    if (eq < 0 || eq + start >= end)
                    {
                        key = s.Substring(start, end - start);
                        val = null;
                    }
                    else if (start + eq + 1 == end)
                    {
                        key = s.Substring(start, eq);
                        val = null;
                    }
                    else
                    {
                        key = s.Substring(start, eq);
                        val = s.Substring(start + eq + 1, end - (start + eq + 1));
                    }
                    Add(key, val);
                    if (sc < 0)
                    {
                        break;
                    }
                    else
                    {
                        start = start + sc + 1;
                    }
                }
            }
        }

        /// <summary>
        /// Ensure a value is on the list for a key.
        /// note that this does not check for the value already being present.
        /// </summary>
        /// <param name="key">The key to add, unless already present.</param>
        /// <param name="val">The value to add (unconditionally).</param>
        public void Add(string key, string val)
        {
            if (!_data.ContainsKey(key))
            {
                _data[key] = new List<string>();
            }
            _data[key].Add(val);
        }

        /// <summary>
        /// Like Add, but removes any existing value for the key, replacing it
        /// with the new, single value.
        /// </summary>
        /// <param name="key">The key to create.</param>
        /// <param name="val">The value.</param>
        public void Set(string key, string val)
        {
            _data[key] = new List<string>();
            _data[key].Add(val);
        }

        /// <summary>
        /// Return the value for the key (or an empty string if there is no value).
        /// This should only be called if the key is known to be single-valued; it
        /// will throw an exception if there are multiple values.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The single string value, or an empty string.</returns>
        public string Get(string key)
        {
            if (!_data.ContainsKey(key) || _data[key] == null || _data[key].Count == 0)
            {
                return "";
            }
            if (_data[key].Count != 1)
            {
                string message = String.Format(CultureInfo.InvariantCulture,
                        "Options.Get: Key '{0}' is multivalued.",
                        key);
                Trace.Report(message);
                throw new ArgumentException(message);
            }
            return _data[key][0];
        }

        /// <summary>
        /// Delete the key and any associated values.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Delete(string key)
        {
            _data.Remove(key);
        }

        /// <summary>
        /// If the key/value pair exists, delete it. Does not delete the key,
        /// or any other values already associated with the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="val">The value to remove, if present.</param>
        public void Delete(string key, string val)
        {
            if (!_data.ContainsKey(key))
            {
                return;
            }
            _data[key].Remove(val);
        }

        /// <summary>
        /// Turn all the options settings into a string that is human-readable,
        /// and can also be parsed back into an Options object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string first = "";
            StringBuilder ret = new StringBuilder();
            foreach (KeyValuePair<string, List<string>> kv in _data)
            {
                foreach (string s in kv.Value)
                {
                    ret.Append(first);
                    if (first == "")
                    {
                        first = ";";
                    }
                    string v = (s == null ? "" : "=" + s);
                    ret.Append(kv.Key);
                    ret.Append(v);
                }
            }
            return ret.ToString();
        }
    }
}
