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
    public class FileList<T> : IList<T>, IParsable
    {
        [Parse(ParseAction.Ignore)]
        public Lazy<IList<T>> _lazyList = null;

        [Parse(ParseAction.Ignore)]
        public IList<T> Parent
        {
            get
            {
                return _lazyList.Value;
            }
        }

        public void FinalizeParse()
        {
            _lazyList = new Lazy<IList<T>>(LoadParent);
        }

        [Parse(ParseAction.Required, typeof(InputFile))]
        public FileInfo InputFile;

        public IList<T> LoadParent()
        {
            var list = Bio.Util.FileUtils.ReadEachLine(InputFile.FullName).Select(v => Parse.Parser.Parse<T>(v)).ToList();
            return list;
        }


        #region IList<T> Members

        public int IndexOf(T item)
        {
            return Parent.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            Parent.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Parent.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return Parent[index];
            }
            set
            {
                Parent[index] = value;
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            Parent.Add(item);
        }

        public void Clear()
        {
            Parent.Clear();
        }

        public bool Contains(T item)
        {
            return Parent.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Parent.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return Parent.Count; }
        }

        public bool IsReadOnly
        {
            get { return Parent.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return Parent.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
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
