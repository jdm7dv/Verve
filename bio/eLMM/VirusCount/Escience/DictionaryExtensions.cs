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

    public static class DictionaryExtensions
    {

        public static Dictionary<T2, List<T1>> InvertManyToOne<T1, T2>(this IDictionary<T1, T2> dictionary)
        {
            var query =
                (from pair in dictionary
                 group pair.Key by pair.Value into g
                 select g
                 ).ToDictionary(g => g.Key, g => g.ToList());

            return query;
        }


        /// <summary>
        /// An example here would be when we have a Dictionary that maps a GeneSet to a list of Genes, and we want instead
        /// to have a Dictionary that maps each Gene to a list of GeneSets
        /// </summary>
        /// <param name="myDict"></param>
        /// <returns></returns>
        public static Dictionary<T1, List<T2>> InvertValueToListDictionary<T1, T2>(this IDictionary<T2, List<T1>> myDict)
        {
            Dictionary<T1, List<T2>> newDict = new Dictionary<T1, List<T2>>();
            foreach (T2 key in myDict.Keys)
            {
                foreach (T1 elt in myDict[key])
                {
                    newDict.GetValueOrDefault(elt).Add(key);
                }
            }
            return newDict;
        }


        /// <summary>
        /// An example here would be when we have a Dictionary that maps a transcriptCluster to a list of probeIds,
        ///and we want to have a dictionary that maps each probeId to a single transcriptCluster
        /// </summary>
        /// <param name="myDict"></param>
        /// <returns></returns>
        public static Dictionary<T1, T2> InvertDictionaryWithListToOntoMapping<T1, T2>(this IDictionary<T2, List<T1>> myDict)
        {
            Dictionary<T1, T2> newDict = new Dictionary<T1, T2>();
            foreach (T2 key in myDict.Keys)
            {
                foreach (T1 elt in myDict[key])
                {
                    newDict.Add(elt, key);
                }
            }
            return newDict;
        }

        public static List<TValue> SelectManyValuesIgnoringKeysNotPresent<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IEnumerable<TKey> keysToGetValuesFor)
        {
            List<TKey> keysToGet = keysToGetValuesFor.Intersect(dictionary.Keys, dictionary.Comparer).ToList();
            return keysToGet.Select(elt => dictionary[elt]).ToList();
        }

        public static List<TValue> SelectManyValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IEnumerable<TKey> keysToGetValuesFor)
        {
            return keysToGetValuesFor.Select(elt => dictionary[elt]).ToList();
        }

        public static Dictionary<TKey, TValue> ShuffleValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Random myRand)
        {
            Dictionary<TKey, TValue> dictWithShuffledValues = new Dictionary<TKey, TValue>(dictionary.Comparer);
            List<TValue> randValues = dictionary.Values.Shuffle(myRand);
            int i = 0;
            foreach (TKey key in dictionary.Keys)
            {
                dictWithShuffledValues.Add(key, randValues.ElementAt(i++));
            }
            return dictWithShuffledValues;
        }

        /// <summary>
        /// return a new dictionary with only the elements that match the specified list of keys
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="keysToGetValuesFor"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> SubDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, IEnumerable<TKey> keysToGetValuesFor)
        {
            return keysToGetValuesFor.Select(elt => new KeyValuePair<TKey, TValue>(elt, dictionary[elt])).ToDictionary();
        }



        public static Dictionary<TKey, TValue> Shuffle<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Random random)
        {
            List<TValue> shuffledValueList = dictionary.Values.Shuffle(random);
            var newDictionary = dictionary.Select((pair, index) => new { pair, index }).ToDictionary(pairAndIndex => pairAndIndex.pair.Key, pairAndIndex => shuffledValueList[pairAndIndex.index]);
            return newDictionary;
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<Tuple<TKey, TValue>> tupleSequence)
        {
            return tupleSequence.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
        }


    }



}
