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

namespace Bio.Util
{
    /// <summary>
    /// This class provides various extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Returns the element at a specified index in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="collection"> An System.Collections.Generic.IEnumerable to return an element from.</param>
        /// <param name="index">The zero-based index of the element to retrieve.</param>
        /// <returns>The element at the specified position in the source sequence.</returns>
        public static TSource ElementAt<TSource>(this IEnumerable<TSource> collection, long index)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            if (index < 0 || index > collection.Count())
            {
                throw new ArgumentOutOfRangeException("index");
            }

            IEnumerator<TSource> enumerator = collection.GetEnumerator();
            for (long i = 0; i <= index; i++)
            {
                enumerator.MoveNext();
            }

            return enumerator.Current;
        }
    }
}
