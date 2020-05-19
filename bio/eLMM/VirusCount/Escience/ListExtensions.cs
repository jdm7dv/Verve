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
using Bio.Util;

namespace MBT.Escience
{
    public static class ListExtensions
    {

        public static List<string> GetElementsWithTrailingWhiteSpace(this List<string> myList)
        {
            List<string> valuesWithTrailingSpaces = new List<string>();
            foreach (string elt in myList)
            {
                if (elt.EndsWith(" ")) valuesWithTrailingSpaces.Add(elt);
            }
            return valuesWithTrailingSpaces;
        }

        public static void ShuffleInPlace<T>(this IList<T> listToShuffle, Random random)
        {
            for (int i = 0; i < listToShuffle.Count; i++)
            {
                int j = random.Next(listToShuffle.Count);
                listToShuffle.Swap(i, j);
            }
        }

        public static List<T> GetRange<T>(this IList<T> list, int index, int count)
        {
            List<T> output = new List<T>(count);
            for (int i = index; i < index + count; ++i)
            {
                output.Add(list[i]);
            }
            return output;
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            T tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }

        /// <summary>
        /// will use Shuffle if you want many of the items, as it will be more efficient
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="count"></param>
        /// <param name="rand"></param>
        /// <returns></returns>
        public static List<T> SampleAtRandomWithoutReplacement<T>(this IList<T> list, int count, Random rand)
        {
            Helper.CheckCondition(count <= list.Count, "Can't sample without replacement more items that there are");
            if (count > list.Count / 4)
            {
                list.Shuffle(rand).Take(count).ToList();
            }

            //first sample the indexes, and make sure there are no duplicates:
            HashSet<int> sampledIndexes = new HashSet<int>();
            while (sampledIndexes.Count < count)
            {
                int nxtIndex = rand.Next(list.Count);

                if (!sampledIndexes.Contains(nxtIndex))
                {
                    sampledIndexes.Add(nxtIndex);
                }
            }
            Helper.CheckCondition(sampledIndexes.Distinct().Count() == count, "something went wrong--bug");

            List<T> results = list.SubList(sampledIndexes.ToList());
            return results;
        }


        public static T SampleAtRandom<T>(this IList<T> list, Random rand)
        {
            int index = rand.Next(list.Count);
            return list[index];
        }

        public static T SampleAtRandom<T>(this List<T> list, List<double> probabilityCDF, Random rand)
        {
            Helper.CheckCondition(probabilityCDF.Count == list.Count, "The size of the probability distribution must be the same as for the list.");

            double p = rand.NextDouble();
            int idx = probabilityCDF.BinarySearch(p);
            if (idx < 0)
                idx = ~idx;

            return list[idx];
        }

        public static List<T> SampleWithReplacement<T>(this IList<T> list, int count, Random random)
        {
            List<T> result = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                result.Add(list.SampleAtRandom(random));
            }
            return result;
        }

        public static List<T> SampleWithReplacement<T>(this List<T> list, int count, List<double> probabilityCDF, Random random)
        {
            List<T> result = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                result.Add(list.SampleAtRandom(probabilityCDF, random));
            }
            return result;
        }


        //public static void AddMultiple<T>(this IList<T> list, T value, int numberOfTimesToAdd)
        //{
        //    for (int i = 0; i < numberOfTimesToAdd; i++)
        //    {
        //        list.Add(value);
        //    }
        //}

        //public static List<T> SubList<T>(this IList<T> list, int start, int count)
        //{
        //    List<T> result = new List<T>(count);
        //    for (int i = start; i < start + count; i++)
        //        result.Add(list[i]);
        //    return result;
        //}


        public static List<T> SubList<T>(this IList<T> myList, List<int> indexesToGet)
        {
            return indexesToGet.Select(elt => myList[elt]).ToList();
        }

        public static List<T> SubList<T>(this IList<T> myList, params int[] indexesToGet)
        {
            return indexesToGet.Select(elt => myList[elt]).ToList();
        }

    }
}
