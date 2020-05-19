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
using Bio.Util;

namespace MBT.Escience
{
    public class HashableEnumerable<T> : IEnumerable<T>
    {
        IEnumerable<T> Enumerable;

        public HashableEnumerable(IEnumerable<T> sequence)
        {
            Enumerable = sequence;

            _hashCode = typeof(T).GetHashCode() ^ 934032323;

            foreach (T t in sequence)
            {
                _hashCode = Helper.WrapAroundLeftShift(_hashCode, 1) ^ t.GetHashCode();
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Enumerable.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Enumerable.GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            HashableEnumerable<T> other = obj as HashableEnumerable<T>;
            if (other == null)
            {
                return false;
            }
            else
            {
                return _hashCode == other._hashCode && this.SequenceEqual(other);
            }
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        int _hashCode;


    }
}
