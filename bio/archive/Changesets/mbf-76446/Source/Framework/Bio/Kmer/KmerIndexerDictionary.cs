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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bio.Algorithms.Kmer
{
    /// <summary>
    /// Wrapper for dictionary that maps kmer strings 
    /// to list of sequence index and positions of occurrence.
    /// </summary>
    public class KmerIndexerDictionary
    {
        /// <summary>
        /// Maps kmer to list of KmerIndexer.
        /// Each KmerIndexer point to places of occurrence of kmer.
        /// </summary>
        private Dictionary<ISequence, IList<KmerIndexer>> kmerIndexer = 
            new Dictionary<ISequence, IList<KmerIndexer>>(new SequenceEqualityComparer());

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <returns>The value associated with the specified key.</returns>
        public IList<KmerIndexer> this[ISequence key]
        {
            get { return this.kmerIndexer[key]; }
            set { this.kmerIndexer[key] = value; }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the 
        /// kmer and corresponding list of positions.
        /// </summary>
        /// <returns>Enumerator over kmers.</returns>
        public Dictionary<ISequence, IList<KmerIndexer>>.Enumerator GetEnumerator()
        {
            return this.kmerIndexer.GetEnumerator();
        }

        /// <summary>
        /// Determines whether kmer dictionary contains specified key.
        /// </summary>
        /// <param name="key">The key to locate.</param>
        /// <returns>Boolean indicating if key exists.</returns>
        public bool ContainsKey(ISequence key)
        {
            return this.kmerIndexer.ContainsKey(key);
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">Contains value associated with.
        /// the specified key, if key is found.</param>
        /// <returns>Boolean indicating if key was found.</returns>
        public bool TryGetValue(ISequence key, out IList<KmerIndexer> value)
        {
            return this.kmerIndexer.TryGetValue(key, out value);
        }

        /// <summary>
        /// Adds the specified key and value to the dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value of the element to add.</param>
        public void Add(ISequence key, IList<KmerIndexer> value)
        {
            this.kmerIndexer.Add(key, value);
        }
    }
}