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
using System.Linq;

namespace MBT.Escience
{
    public class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private Grouping()
        {
        }

        static public Grouping<TKey, TElement> GetInstance(TKey key, IEnumerator<TElement> enumerator)
        {
            Grouping<TKey, TElement> grouping = new Grouping<TKey, TElement>();
            grouping.Key = key;
            grouping._enumerator = enumerator;
            return grouping;
        }

        public TKey Key { get; private set; }

        private IEnumerator<TElement> _enumerator;
        public IEnumerator<TElement> GetEnumerator()
        {
            return _enumerator;
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _enumerator;
        }

    }
}
