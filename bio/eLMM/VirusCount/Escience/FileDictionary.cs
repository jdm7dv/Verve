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
using MBT.Escience.Parse;
using System.IO;
using Bio.Util;

namespace MBT.Escience
{
    [ParseAsNonCollection]
    public class FileDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IParsable
    {
        [Parse(ParseAction.Ignore)]
        public Lazy<IDictionary<TKey, TValue>> _lazyDictionary = null;

        [Parse(ParseAction.Ignore)]
        public IDictionary<TKey, TValue> Parent
        {
            get
            {
                return _lazyDictionary.Value;
            }
        }

        //public static implicit operator FileDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        //{
        //    return new FileDictionary<TKey, TValue> { _lazyDictionary = new Lazy<IDictionary<TKey, TValue>>(() => dictionary) };
        //}
           


        public void FinalizeParse()
        {
            _lazyDictionary = new Lazy<IDictionary<TKey, TValue>>(LoadParent);
        }

        [Parse(ParseAction.Required, typeof(InputFile))]
        public FileInfo InputFile;

        public IDictionary<TKey, TValue> LoadParent()
        {
            using(TextReader textReader = InputFile.OpenText())
            {
                string header = textReader.ReadLine();
                Helper.CheckCondition(header != null, "Expect header");
                Helper.CheckCondition(header.Split('\t').Length == 2, "Expect two-column header in " + InputFile.Name);

                var dictionary = new Dictionary<TKey, TValue>();
                string line;
                while (null != (line = textReader.ReadLine()))
                {
                    string[] fields = line.Split('\t');
                    Helper.CheckCondition(fields.Length == 2, "Expect two-column lines in " + InputFile.Name);
                    TKey key = Parse.Parser.Parse<TKey>(fields[0]);
                    TValue value = Parse.Parser.Parse<TValue>(fields[1]);
                    dictionary.Add(key, value);
                }
                return dictionary;
            }
        }



        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(TKey key)
        {
            return Parent.ContainsKey(key);
        }

        public ICollection<TKey> Keys
        {
            get { return Parent.Keys; }
        }

        public bool Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return Parent.TryGetValue(key, out value);
        }

        public ICollection<TValue> Values
        {
            get { return Parent.Values; }
        }

        public TValue this[TKey key]
        {
            get
            {
                return Parent[key];
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return Parent.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Parent.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Parent.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Parent.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)Parent).GetEnumerator();
        }

        #endregion
    }
}
