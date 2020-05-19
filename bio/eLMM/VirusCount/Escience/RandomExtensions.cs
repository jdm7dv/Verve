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
using Bio.Util;

namespace MBT.Escience
{
    public static class RandomExtensions
    {
        public static T NextListItem<T>(this Random random, IList<T> list)
        {
            T t = list[random.Next(list.Count)];
            return t;
        }

        //Selects an item, uniform randomly, from an enumeration
        public static T NextEnumerationItem<T>(this Random random, IEnumerable<T> enumerable)
        {
            T currentSelection = default(T);
            int index = -1;
            foreach (T item in enumerable)
            {
                ++index;
                if (random.NextDouble() < (1.0 / (double)index))
                {
                    currentSelection = item;
                }
            }
            Helper.CheckCondition(index >= 0, "Enumeration is empty, so cannot select an item");
            return currentSelection;
        }
    }
}
