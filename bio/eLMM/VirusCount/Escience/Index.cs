//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBT.Escience;
using Bio.Util;
using System.Collections;

namespace MBT.Escience
{
    /// <summary>
    /// A combination dictionary and array, allowing you to look up an index number by name, or name by index number.
    /// </summary>
    [Serializable]
    public class Index<K> : IEnumerable<K>, IEnumerable
    {
        private IDictionary<K, int> _keyToIndex;
        private List<K> _indexToKey;

        public Index() : this(10) { }

        public Index(IEnumerable<K> keys, bool frozen=false)
            : this()
        {
            keys.ForEach(key => Add(key));
            Frozen = frozen;
        }

        /// <summary>
        /// Copy constructory. Makes a shallow copy.
        /// </summary>
        /// <param name="other">Value from which to copy.</param>
        public Index(Index<K> other)
        {
            _keyToIndex = new Dictionary<K, int>(other._keyToIndex);
            _indexToKey = new List<K>(other._indexToKey);
        }

        /// <summary>
        /// Creates a new instance with the specified capacity. Use this if you know approximately how many items there will be.
        /// </summary>
        /// <param name="capacity">Will ensure this much capacity.</param>
        public Index(int capacity)
        {
            _keyToIndex = new Dictionary<K, int>(capacity);
            _indexToKey = new List<K>(capacity);
        }

        /// <summary>
        /// Gets the total number of indexed items.
        /// </summary>
        public int Count { get { return _indexToKey.Count; } }

        private bool _frozen = false;
        /// <summary>
        /// Gets or sets a value specifying whether this Index if frozen. If frozen, calls to Add will throw an exception.
        /// </summary>
        public bool Frozen
        {
            get { return _frozen; }
            set
            {
                _frozen = value;
                if (value)
                    _indexToKey.TrimExcess();
            }
        }

        /// <summary>
        /// Gets the current list of keys as readonly.
        /// </summary>
        public IList<K> Keys { get { return _indexToKey.AsReadOnly(); } }

        /// <summary>
        /// Get's the Key at the specified index.
        /// </summary>
        /// <param name="index">The index of the key.</param>
        /// <exception>ArgumentOutOfBoundsException if the index is out of bounds.</exception>
        /// <returns>The key at index</returns>
        public K this[int index] { get { return _indexToKey[index]; } }

        /// <summary>
        /// Gets the index of the specified key. If the key is not present, and the Index is not Frozen, will 
        /// add the key. If the key is not present and the index if Frozen, will return -1.
        /// </summary>
        /// <param name="key">The key. </param>
        /// <returns>The index of Key, or -1 if not present and the Index if Frozen.</returns>
        public int this[K key]
        {
            get
            {
                int index = GetIndex(key);
                if (index >= 0 || Frozen)
                    return index;

                Add(key);
                return _indexToKey.Count - 1;
            }
        }

        /// <summary>
        /// Adds the key to the Index. If the key is already there, no action is taken.
        /// </summary>
        /// <param name="key">The key to be added</param>
        /// <exception cref="InvalidOperationException">Thrown if the Index is currently Frozen.</exception>
        public void Add(K key)
        {
            Helper.CheckCondition<InvalidOperationException>(!Frozen, "Cannot add a key when the Index is Frozen.");

            if (!_keyToIndex.ContainsKey(key))
            {
                _indexToKey.Add(key);
                _keyToIndex.Add(key, _indexToKey.Count - 1);
            }
        }

        /// <summary>
        /// Gets an enumerator over the keys of the collection.
        /// </summary>
        public IEnumerator<K> GetEnumerator()
        {
            return _keyToIndex.Keys.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _keyToIndex.Keys.GetEnumerator();
        }

        /// <summary>
        /// Gets the index of the specified key. Returns -1 if the key is not in the Index.
        /// </summary>
        public int GetIndex(K key)
        {
            int index;
            if (_keyToIndex.TryGetValue(key, out index))
                return index;
            else
                return -1;
        }
    }
}
