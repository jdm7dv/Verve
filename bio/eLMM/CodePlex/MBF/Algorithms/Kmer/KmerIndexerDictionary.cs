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
using System.Linq;
using System.Threading.Tasks;
using Bio.Algorithms.Assembly.Graph;
using Bio.Properties;
using Bio.Util;

namespace Bio.Algorithms.Kmer
{
    /// <summary>
    /// Wrapper for dictionary that maps kmer strings 
    /// to list of sequence index and positions of occurance. 
    /// </summary>
    public class KmerIndexerDictionary
    {
        /// <summary>
        /// Maps kmer to list of KmerIndexer.
        /// Each KmerIndexer point to places of occurance of kmer.
        /// </summary>
        Dictionary<string, IList<KmerIndexer>> _kmerIndexer = new Dictionary<string, IList<KmerIndexer>>();

        /// <summary>
        /// Returns an enumerator that iterates through the 
        /// kmer and corresponding list of positions
        /// </summary>
        /// <returns>Enumerator over kmers</returns>
        public Dictionary<string, IList<KmerIndexer>>.Enumerator GetEnumerator()
        {
            return _kmerIndexer.GetEnumerator();
        }

        /// <summary>
        /// Determines whether kmer dictionary contains specified key.
        /// </summary>
        /// <param name="key">The key to locate</param>
        /// <returns>Boolean indicating if key exists</returns>
        public bool ContainsKey(string key)
        {
            return _kmerIndexer.ContainsKey(key);
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key
        /// </summary>
        /// <param name="key">The key of the value to get or set</param>
        /// <returns>The value associated with the specified key</returns>
        public IList<KmerIndexer> this[string key]
        {
            get { return _kmerIndexer[key]; }
            set { _kmerIndexer[key] = value; }
        }

        /// <summary>
        /// Gets the value associated with the specified key
        /// </summary>
        /// <param name="key">The key of the value to get</param>
        /// <param name="value">Contains value associated with 
        /// the specified key, if key is found</param>
        /// <returns>Boolean indicating if key was found</returns>
        public bool TryGetValue(string key, out IList<KmerIndexer> value)
        {
            return _kmerIndexer.TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add</param>
        /// <param name="value">The value of the element to add</param>
        public void Add(string key, IList<KmerIndexer> value)
        {
            _kmerIndexer.Add(key, value);
        }
    }
}
