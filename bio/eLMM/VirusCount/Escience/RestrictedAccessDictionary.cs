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

//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace MBT.Escience
//{
//    [Flags]
//    public enum AccessFlags : int
//    {
//        Add = 1, Remove = 2, ChangeElements = 4
//    };

//    /// <summary>
//    /// A thin wrapper around Dictionary that allows access permissions to be set. Any changes not allowed result in an exception.
//    /// </summary>
//    /// <typeparam name="TKey"></typeparam>
//    /// <typeparam name="TValue"></typeparam>
//    public class RestrictedAccessDictionary<TKey, TValue> : IDictionary<TKey, TValue>
//    {
//        private AccessFlags _accessFlags;
//        private IDictionary<TKey, TValue> _dict;


//        public RestrictedAccessDictionary(IDictionary<TKey, TValue> baseDictionary) :
//            this(baseDictionary, 0) { }

//        public RestrictedAccessDictionary(IDictionary<TKey, TValue> baseDictionary, AccessFlags accessFlags)
//        {
//            _dict = baseDictionary;
//            _accessFlags = accessFlags;
//        }



//        public bool AddIsAllowed
//        {
//            get { return (_accessFlags & AccessFlags.Add) == AccessFlags.Add; }
//        }

//        public bool RemoveIsAllowed
//        {
//            get { return (_accessFlags & AccessFlags.Remove) == AccessFlags.Remove; }
//        }

//        public bool ChangeElementsIsAllowed
//        {
//            get { return (_accessFlags & AccessFlags.ChangeElements) == AccessFlags.ChangeElements; }
//        }

//        #region IDictionary<TKey,TValue> Members

//        public void Add(TKey key, TValue value)
//        {
//            if (!AddIsAllowed)
//            {
//                throw new NotSupportedException("This restricted Dictionary does not allow adding items.");
//            }
//            _dict.Add(key, value);
//        }

//        public bool ContainsKey(TKey key)
//        {
//            return _dict.ContainsKey(key);
//        }

//        public ICollection<TKey> Keys
//        {
//            get { return _dict.Keys; }
//        }

//        public bool Remove(TKey key)
//        {
//            if (!RemoveIsAllowed)
//            {
//                throw new NotSupportedException("This restricted access dictionary does not allow removing items.");
//            }
//            return _dict.Remove(key);
//        }

//        public bool TryGetValue(TKey key, out TValue value)
//        {
//            return _dict.TryGetValue(key, out value);
//        }

//        public ICollection<TValue> Values
//        {
//            get { return _dict.Values; }
//        }

//        public TValue this[TKey key]
//        {
//            get
//            {
//                return _dict[key];
//            }
//            set
//            {
//                if (!ChangeElementsIsAllowed && _dict.ContainsKey(key))
//                {
//                    throw new NotSupportedException("This restricted access dictionary does not allow changing entry values.");
//                }
//                if (!AddIsAllowed && !_dict.ContainsKey(key))
//                {
//                    throw new NotSupportedException("This restricted access dictionary does not allow adding items.");
//                }
//                _dict[key] = value;
//            }
//        }

//        #endregion

//        #region ICollection<KeyValuePair<TKey,TValue>> Members

//        public void Add(KeyValuePair<TKey, TValue> item)
//        {
//            if (!AddIsAllowed)
//            {
//                throw new NotSupportedException("This restricted access dictionary does not allow adding items.");
//            }
//            _dict.Add(item);
//        }

//        public void Clear()
//        {
//            if (!RemoveIsAllowed)
//            {
//                throw new NotSupportedException("This restricted access dictionary does not allow removing items.");
//            }
//            _dict.Clear();
//        }

//        public bool Contains(KeyValuePair<TKey, TValue> item)
//        {
//            return _dict.Contains(item);
//        }

//        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
//        {
//            _dict.CopyTo(array, arrayIndex);
//        }

//        public int Count
//        {
//            get { return _dict.Count(); }
//        }

//        public bool IsReadOnly
//        {
//            get { return !AddIsAllowed || !RemoveIsAllowed || !ChangeElementsIsAllowed; }
//        }

//        public bool Remove(KeyValuePair<TKey, TValue> item)
//        {
//            if (!RemoveIsAllowed)
//            {
//                throw new NotSupportedException("This restricted access dictionary does not allow removing items.");
//            }
//            return _dict.Remove(item);
//        }

//        #endregion

//        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

//        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
//        {
//            return _dict.GetEnumerator();
//        }

//        #endregion

//        #region IEnumerable Members

//        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
//        {
//            return _dict.GetEnumerator();
//        }

//        #endregion
//    }
//}
