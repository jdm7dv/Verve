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

using System.Collections.Generic;

namespace MBT.Escience
{
    public class Cache<TKey, TVal>
    {
        readonly int _maxSize;
        readonly int _recoverySize;
        readonly Queue<KeyValuePair<TKey, TVal>> _lastItemsAdded;
        readonly Dictionary<TKey, TVal> _cache;


        public Cache(int maxSize, int recoverySize) : this(maxSize, recoverySize, null) { }

        public Cache(int maxSize, int recoverySize, IEqualityComparer<TKey> comparer)
        {
            _maxSize = maxSize;
            _recoverySize = recoverySize;
            _lastItemsAdded = new Queue<KeyValuePair<TKey, TVal>>(_recoverySize);
            _cache = comparer == null ? new Dictionary<TKey, TVal>(_maxSize) : new Dictionary<TKey, TVal>(_maxSize, comparer);
        }

        public TVal this[TKey key]
        {
            get
            {
                return _cache[key];
            }
            set
            {
                TVal currentVal;
                if (_cache.TryGetValue(key, out currentVal))
                {
                    if (!value.Equals(currentVal))
                    {
                        Remove(key);
                        Add(key, value);
                    }
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _cache.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TVal value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public void Add(TKey key, TVal val)
        {
            if (_cache.Count >= _maxSize)
            {
                Clear();
            }
            _cache.Add(key, val);

            if (_lastItemsAdded.Count == _recoverySize)
            {
                _lastItemsAdded.Dequeue();
            }
            _lastItemsAdded.Enqueue(new KeyValuePair<TKey, TVal>(key, val));
        }

        public bool Remove(TKey key)
        {
            bool itemRemoved = _cache.Remove(key);
            if (itemRemoved)
            {
                int countOfLast = _lastItemsAdded.Count;
                for (int i = 0; i < countOfLast; i++)
                {
                    KeyValuePair<TKey, TVal> keyVal = _lastItemsAdded.Dequeue();
                    if (!keyVal.Key.Equals(key))
                    {
                        _lastItemsAdded.Enqueue(keyVal);
                    }
                }
            }
            return itemRemoved;
        }

        public void Clear()
        {
            _cache.Clear();
            foreach (KeyValuePair<TKey, TVal> keyValue in _lastItemsAdded)
            {
                _cache.Add(keyValue.Key, keyValue.Value);
            }
        }

        public void ClearAll()
        {
            _cache.Clear();
            _lastItemsAdded.Clear();
        }
    }
}
