// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MBF.Util
{
    /// <summary>
    /// Extension methods related to HashSet
    /// </summary>
    public static class HashSetExtensions
    {

        /// <summary>
        /// Add a range of values to a hashset. It is OK if the values are already of the hashset.
        /// </summary>
        /// <typeparam name="T">The type of the hashset's elements</typeparam>
        /// <param name="hashSet">The hashset to add values to</param>
        /// <param name="sequence">A sequence of values to add to the hashset.</param>
        public static void AddNewOrOldRange<T>(this HashSet<T> hashSet, IEnumerable<T> sequence)
        {
            foreach (T t in sequence)
            {
                hashSet.Add(t);
            }
        }
    }
}
