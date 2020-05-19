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
using System.Threading;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using Bio.Util;

namespace MBT.Escience
{
    public static class IEnumerableExtensions
    {

        public static List<T> AsList<T>(this IEnumerable<T> values)
        {
            if (values is List<T>)
            {
                return (List<T>)values;
            }
            else
            {
                return values.ToList();
            }
        }

        public static IList<T> AsIList<T>(this IEnumerable<T> values)
        {
            if (values is IList<T>)
            {
                return (IList<T>)values;
            }
            else
            {
                return values.ToList();
            }
        }


        public static ReadOnlyCollection<T> AsReadOnlyCollection<T>(this IEnumerable<T> values)
        {
            if (values is ReadOnlyCollection<T>)
            {
                return (ReadOnlyCollection<T>)values;
            }

            return new ReadOnlyCollection<T>(values.AsList());
        }

        public static void WriteEachLineToFile(this IEnumerable values, string filename)
        {
            using (TextWriter writer = File.CreateText(filename))
            {
                foreach (var val in values)
                    writer.WriteLine(val.ToString());
            }
        }


        public static IEnumerable<T> SampleAtRandomWithoutReplacement<T>(this IEnumerable<T> sequence, int count, Random random)
        {
            List<T> shuffledList = sequence.Shuffle(random);
            Helper.CheckCondition(count <= shuffledList.Count, "Can't sample without replacement more items that there are");
            return shuffledList.Take(count);
        }


        //public static List<T> Shuffle<T>(this IEnumerable<T> input, Random random)
        //{
        //    List<T> list = new List<T>();
        //    foreach (T t in input)
        //    {
        //        list.Add(t); //We put the value here to get the new space allocated
        //        int oldIndex = random.Next(list.Count);
        //        list[list.Count - 1] = list[oldIndex];
        //        list[oldIndex] = t;
        //    }
        //    return list;
        //}

        //        public static string StringJoin(this System.Collections.IEnumerable list)
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            foreach (object obj in list)
        //            {
        //                if (obj == null)
        //                {
        //                    sb.Append("null");
        //                }
        //                else
        //                {
        //                    sb.Append(obj.ToString());
        //                }
        //            }
        //            return sb.ToString();
        //        }

        //        public static string StringJoin(this System.Collections.IEnumerable list, string separator)
        //        {
        //            StringBuilder aStringBuilder = new StringBuilder();
        //            bool isFirst = true;
        //            foreach (object obj in list)
        //            {
        //                if (!isFirst)
        //                {
        //                    aStringBuilder.Append(separator);
        //                }
        //                else
        //                {
        //                    isFirst = false;
        //                }

        //                if (obj == null)
        //                {
        //                    aStringBuilder.Append("null");
        //                }
        //                else
        //                {
        //                    aStringBuilder.Append(obj.ToString());
        //                }
        //            }
        //            return aStringBuilder.ToString();
        //        }

        //        public static string StringJoin(this System.Collections.IEnumerable list, string separator, int maxLength, string etcString)
        //        {
        //            Helper.CheckCondition(maxLength >= 2, "maxLength must be at least 2");
        //            StringBuilder aStringBuilder = new StringBuilder();
        //            int i = -1;
        //            foreach (object obj in list)
        //            {
        //                ++i;
        //                if (i > 1)
        //                {
        //                    aStringBuilder.Append(separator);
        //                }

        //                if (i >= maxLength)
        //                {
        //                    aStringBuilder.Append(etcString);
        //                    break; // really break, not continue;
        //                }
        //                else if (obj == null)
        //                {
        //                    aStringBuilder.Append("null");
        //                }
        //                else
        //                {
        //                    aStringBuilder.Append(obj.ToString());
        //                }
        //            }
        //            return aStringBuilder.ToString();
        //        }


        //        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> list)
        //        {
        //            return new HashSet<T>(list);
        //        }

        //        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> list, IEqualityComparer<T> comparer)
        //        {
        //            return new HashSet<T>(list, comparer);
        //        }

        //        public static Dictionary<T1, T2> ToDictionary<T1, T2>(this IEnumerable<KeyValuePair<T1, T2>> pairList)
        //        {
        //            return pairList.ToDictionary(pair => pair.Key, pair => pair.Value);
        //        }

        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, TSource item)
        {
            return first.Concat(new TSource[] { item });
        }
        public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first, TSource item)
        {
            return first.Except(new TSource[] { item });
        }

        //        public static void ForEach<T>(this IEnumerable<T> e, Action<T> func)
        //        {
        //            foreach (T t in e)
        //            {
        //                func(t);
        //            }
        //        }

        //        /// <summary>
        //        /// Calls func on each element. func takes two arguments: the element and the index of the element.
        //        /// </summary>
        //        public static void ForEach<T>(this IEnumerable<T> e, Action<T, int> func)
        //        {
        //            int idx = 0;
        //            foreach (T t in e)
        //            {
        //                func(t, idx++);
        //            }
        //        }

        /// <summary>
        /// Computes the weighted average of sequence, weighted by weights. An exception will be thrown if weights.Count() != sequence.Count()
        /// </summary>
        /// <typeparam name="T">The type of the sequence</typeparam>
        /// <param name="seed">The starting value for the averaging</param>
        /// <param name="sequence">The sequence to be averaged.</param>
        /// <param name="weights">Any set of weights.</param>
        /// <param name="func">Func(T runningAverage, T currentElement, double currentWeight) returns T newRunningAverage</param>
        /// <returns></returns>
        public static T WeightedAverage<T>(this IEnumerable<T> sequence, T seed, IEnumerable<double> weights, Func<T, T, double, T> func)
        {
            T result = seed;
            foreach (KeyValuePair<T, double> itemAndWeight in SpecialFunctions.EnumerateTwo(sequence, weights))
            {
                result = func(result, itemAndWeight.Key, itemAndWeight.Value);
            }
            return result;
        }

        /// <summary>
        /// Computes the median of the list. If ListIn is type List, then the items of that list will be sorted.
        /// </summary>
        public static double Median(this IEnumerable<int> listIn)
        {
            List<int> list = listIn as List<int>;
            if (list == null)
                list = new List<int>(listIn).ToList();

            if (list.Count == 0)
            {
                return double.NaN;
            }
            list.Sort();

            //Examples of indexs of interest
            //0 1 2 -> 1
            //0 1 2 3 -> 1,2

            int lowIndex = (list.Count - 1) / 2;
            if (list.Count % 2 == 1)
            {
                return (double)list[lowIndex];
            }
            else
            {
                return (double)(list[lowIndex] + list[lowIndex + 1]) / 2.0;
            }
        }

        /// <summary>
        /// Returns the index of the first occurrence of the maximum value in the list. 
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Returns -1 if the list is empty.</returns>
        public static int ArgMax(this IEnumerable<double> list)
        {
            double max = double.MinValue;
            int idx = -1, argMax = -1;
            foreach (double d in list)
            {
                idx++;
                if (d > max)
                {
                    max = d;
                    argMax = idx;
                }
            }
            return argMax;
        }

        /// <summary>
        /// Returns the index of the first occurrence of the maximum value in the list. 
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Returns -1 if the list is empty.</returns>
        public static int ArgMax(this IEnumerable<int> list)
        {
            int max = int.MinValue;
            int idx = -1, argMax = -1;
            foreach (int d in list)
            {
                idx++;
                if (d > max)
                {
                    max = d;
                    argMax = idx;
                }
            }
            return argMax;
        }

        /// <summary>
        /// Returns the index of the first occurrence of the minimum value in the list. 
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Returns -1 if the list is empty.</returns>
        public static int ArgMin(this IEnumerable<double> list)
        {
            double max = double.MaxValue;
            int idx = -1, argMax = -1;
            foreach (double d in list)
            {
                idx++;
                if (d < max)
                {
                    max = d;
                    argMax = idx;
                }
            }
            return argMax;
        }

        /// <summary>
        /// Returns the index of the first occurrence of the maximum value in the list. 
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Returns -1 if the list is empty.</returns>
        public static int ArgMin(this IEnumerable<int> list)
        {
            int max = int.MaxValue;
            int idx = -1, argMax = -1;
            foreach (int d in list)
            {
                idx++;
                if (d < max)
                {
                    max = d;
                    argMax = idx;
                }
            }
            return argMax;
        }

        /// <summary>
        /// Computes the median of the list. If ListIn is type List, then the items of that list will be sorted.
        /// </summary>
        public static double Median(this IEnumerable<double> listIn)
        {
            List<double> list = listIn as List<double>;
            if (list == null)
                list = new List<double>(listIn).ToList();

            if (list.Count == 0)
            {
                return double.NaN;
            }
            list.Sort();

            //Examples of indexs of doubleerest
            //0 1 2 -> 1
            //0 1 2 3 -> 1,2

            int lowIndex = (list.Count - 1) / 2;
            if (list.Count % 2 == 1)
            {
                return list[lowIndex];
            }
            else
            {
                return (list[lowIndex] + list[lowIndex + 1]) / 2.0;
            }
        }

        /// <summary>
        /// Computes the SAMPLE variance, that is with Bessel's correction
        /// http://en.wikipedia.org/wiki/Bessel%27s_correction
        /// </summary>
        public static double Variance(this IEnumerable<double> values)
        {
            int n = 0;
            double sum = 0;
            double ss = 0;
            foreach (double d in values)
            {
                sum += d;
                ss += d * d;
                n++;
            }

            double variance = (ss - (sum * sum / n)) / (n - 1);
            return variance;
        }


        /// <summary>
        /// Computes the SAMPLE variance, that is with Bessel's correction
        /// http://en.wikipedia.org/wiki/Bessel%27s_correction
        /// </summary>
        public static double Variance(this IEnumerable<int> values)
        {
            int n = 0;
            double sum = 0;
            double ss = 0;
            foreach (double d in values)
            {
                sum += d;
                ss += d * d;
                n++;
            }

            double variance = (ss - (sum * sum / n)) / (n - 1);
            return variance;
        }


        /// <summary>
        /// Uses Bessel's correction, http://en.wikipedia.org/wiki/Bessel%27s_correction
        /// </summary>
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            return Math.Sqrt(values.Variance());
        }


        //Picks randomly among tied values
        public static double Mode(this IEnumerable<double> values, ref Random random)
        {
            var query =
                from value in values
                group value by value into g
                select new { value = g.Key, count = g.Count() };

            BestSoFarWithTies<int, double> bestSoFarWithTies = BestSoFarWithTies<int, double>.GetInstance(SpecialFunctions.IntGreaterThan);
            foreach (var pair in query)
            {
                bestSoFarWithTies.Compare(pair.count, pair.value);
            }

            double mode = bestSoFarWithTies.ChampList.SampleAtRandom(random);
            return mode;
        }


        public static double StandardDeviation(this IEnumerable<int> values)
        {
            return Math.Sqrt(values.Variance());
        }

        /// <summary>
        /// Massively parallel select statement. Behaves as normal select, but assigns one thread to each and every item in the input
        /// enumerable. This differs from AsParallel.Select, which has a max number of threads. 
        /// <b>This should only be used when each func call is slow and spends most of its time sleeping (such as waiting for a cluster job to finish)!!</b>
        /// </summary>
        /// <typeparam name="TIn">Type of the enumeration</typeparam>
        /// <typeparam name="TOut">Type of the results</typeparam>
        /// <param name="values">Each item in values will be given it's own thread when func(item) is called</param>
        /// <param name="approxThreadCount">Ideally the number of values, if they can be efficiently counted. Sets the minimum thread count for
        /// thread pool, which is more efficient that making threadpool ramp up itself as more workers are requested.</param>
        /// <param name="func">Takes one instance from the enumeration and returns a value. Should be a call that has lots of sleeping. If not, use .AsParallel</param>
        /// <returns>Items will be emitted by the calling thread</returns>
        public static IEnumerable<TOut> SelectInParallel<TIn, TOut>(this IEnumerable<TIn> values, int approxThreadCount, Func<TIn, TOut> func)
        {
            int oldWorkerCount, oldCompletionCount;
            ThreadPool.GetMinThreads(out oldWorkerCount, out oldCompletionCount);
            ThreadPool.SetMinThreads(approxThreadCount, oldCompletionCount);    // give a warning that we're going to be using lots of threads.

            int activeThreads = 0;
            Queue<TOut> resultsToEmit = new Queue<TOut>();  // will hold results as they come available. Worker threads will Enqueue, the calling thread will dequeue
            object lockObj = new object();

            // This is a method that each thread calls when finished. The thread will pass the resulting item, of time TOut.
            // This method will enqueue the item in the results, indicate that once less thread is active, then pulse the main
            // thread to let it know there's something that can be emitted. This makes sure results are emitted as they become available
            Action<TOut> RegisterFinishedThread = (resultItem) =>
            {
                lock (lockObj)
                {
                    resultsToEmit.Enqueue(resultItem);
                    activeThreads--;
                    Monitor.Pulse(lockObj); // alert the main thread that there is a new result in the queue
                }
            };

            // The parallel for. 
            foreach (TIn item in values)
            {
                TIn localItem = item; // THIS IS CRITICAL. item is an iterator. If you place it in the lambda, you'll get surprising results.
                lock (lockObj) activeThreads++; // note that one more thread is now active.

                ThreadPool.QueueUserWorkItem(state =>
                {
                    TOut result = func(localItem);
                    RegisterFinishedThread(result); // now that the function is called, register the result.
                });
            }


            // the main thread will sit here, emitting results as they come available. Use the main thread because that's the submitting thread
            // and the caller doesn't expect the results to be multithreaded (which they would if the worker threads called yield return)
            lock (lockObj)
            {
                while (true)
                {
                    if (resultsToEmit.Count == 0) Monitor.Wait(lockObj);    // nothing to do yet. Release the lock and wait for a result to be enqueued

                    TOut itemToEmit = resultsToEmit.Dequeue();

                    Monitor.Exit(lockObj);
                    yield return itemToEmit; // leave the lock, send the main thread on it's way with the result
                    Monitor.Enter(lockObj);

                    if (activeThreads == 0) // when this gets to 0, all threads are done. our work here is finished. Spit out anything that's remaining.
                    {
                        while (resultsToEmit.Count > 0)
                            yield return resultsToEmit.Dequeue();
                        break;
                    }
                }
            }

            ThreadPool.SetMinThreads(oldWorkerCount, oldCompletionCount);    // back to original counts.
        }

        /// <summary>
        /// Massively parallel for each statement. Behaves as normal for each, but assigns one thread to each and every item in the input
        /// enumerable. This differs from AsParallel.ForAll, which has a max number of threads. 
        /// <b>This should only be used when each func call is slow and spends most of its time sleeping (such as waiting for a cluster job to finish)!!</b>
        /// </summary>
        /// <typeparam name="TIn">Type of the enumeration</typeparam>
        /// <param name="values">Each item in values will be given it's own thread when func(item) is called</param>
        /// <param name="approxThreadCount">Ideally the number of values, if they can be efficiently counted. Sets the minimum thread count for
        /// thread pool, which is more efficient that making threadpool ramp up itself as more workers are requested.</param>
        /// <param name="func">Takes one instance from the enumeration and performs an action. Should be a call that has lots of sleeping. If not, use .AsParallel</param>
        public static void ForEachInParallel<TIn>(this IEnumerable<TIn> values, int approxThreadCount, Action<TIn> func)
        {
            values.SelectInParallel(approxThreadCount, v =>
            {
                func(v);
                return -1;
            }).ToList();
        }

        public static double LogSum(this IEnumerable<double> d)
        {
            return SpecialFunctions.LogSumParams(d is double[] ? (double[])d : d.ToArray());
        }

        public static double LogSum<T>(this IEnumerable<T> e, Func<T, double> doubleSelector)
        {
            return SpecialFunctions.LogSumParams(e.Select(elt => doubleSelector(elt)).ToArray());
        }
        public static IEnumerable<T> AsSingletonEnumerable<T>(this T obj)
        {
            yield return obj;
        }

        /// <summary>
        /// Return a tuple with the items and an index
        /// </summary>
        public static IEnumerable<Tuple<T,int>> AppendIndex<T>(this IEnumerable<T> sequence)
        {
            return sequence.Select((item, index) => Tuple.Create(item, index));
        }


        /// <summary>
        /// Returns the item that has the maximum value, where value is supplied by the valueSelector.
        /// </summary>
        public static T ArgMax<T>(this IEnumerable<T> enumerable, Func<T, double> valueSelector)
        {
            //Using linq allows us to get the right answer even when there is just one item and it's value is -infinity.
            //Also, in the future, it could be done in parallel.

            var itemAndValueQuery = enumerable.Select(item => new {item, value=valueSelector(item)});

            var argMaxPair =
                (from item in enumerable
                 let value = valueSelector(item)
                 select new {item, value}
                 ).Aggregate((pair1, pair2) => (pair1.value > pair2.value) ? pair1 : pair2);

            return argMaxPair.item;
        }
        //public static bool EqualItems<T>(this IEnumerable<T> enum1, IEnumerable<T> enum2)
        //{
        //    IEnumerator<T> eor1 = enum1.GetEnumerator();
        //    IEnumerator<T> eor2 = enum2.GetEnumerator();
        //    while (true)
        //    {
        //        bool b1 = eor1.MoveNext();
        //        bool b2 = eor2.MoveNext();
        //        if (!b1 && !b2) // end of both
        //        {
        //            return true;
        //        }
        //        if (!b1 || !b2) // end of just one
        //        {
        //            return false;
        //        }

        //        if (eor1.Current == null)
        //        {
        //            if (eor2.Current != null)
        //            {
        //                return false; //1 is null, but 2 is not
        //            }
        //        }
        //        else if (!eor1.Current.Equals(eor2.Current))
        //        {
        //            return false; //Different values
        //        }
        //    }
        //}

        /// <summary>
        /// Returns a HashSet from a sequence. If the sequence is already a HashSet, no new HashSet is created..
        /// </summary>
        /// <typeparam name="T">the type of elements of the sequence</typeparam>
        /// <param name="sequence">the input sequence</param>
        /// <returns>a HashSet</returns>
        public static HashSet<T> AsHashSet<T>(this IEnumerable<T> sequence)
        {
            if (sequence is HashSet<T>)
            {
                return (HashSet<T>)sequence;
            }
            else
            {
                return new HashSet<T>(sequence);
            }
        }

        public static bool AllSame<T1, T2>(this IEnumerable<T1> sequence, Func<T1, T2> func, ParallelOptions parallelOptions)
        {
            bool isFirst = true;
            T2 reference = default(T2);
            foreach (T1 t1 in sequence)
            {
                T2 t2 = func(t1);
                if (isFirst)
                {
                    reference = t2;
                    isFirst = false;
                }
                else
                {
                    if (!reference.Equals(t2))
                    {
                        return false;
                    }
                }
            }
            return true;
            //return
            //    (from t1 in sequence
            //        .AsParallel().WithDegreeOfParallelism(parallelOptions.MaxDegreeOfParallelism)
            //     select func(t1)
            //     ).Aggregate((t2a, t2b)=> 
        }

        public static string StringJoin(this System.Collections.IEnumerable list, string separator, string stringFormatArgument)
        {
            StringBuilder aStringBuilder = new StringBuilder();
            bool isFirst = true;
            foreach (object obj in list)
            {
                if (!isFirst)
                {
                    aStringBuilder.Append(separator);
                }
                else
                {
                    isFirst = false;
                }

                if (obj == null)
                {
                    aStringBuilder.Append("null");
                }
                else
                {
                    aStringBuilder.AppendFormat(stringFormatArgument, obj);
                }
            }
            return aStringBuilder.ToString();
        }

        public static int[] CountIntegers(this IEnumerable<double> list, int resultsCount)
        {
            int[] results = new int[resultsCount]; //C# inits to zeros
            foreach (double value in list)
            {
                int valueAsInt = (int)value;
                Helper.CheckCondition(value == (double)valueAsInt && 0 <= valueAsInt && valueAsInt < resultsCount, () => string.Format("Expect values to be integers at least 0 and strictly less than {0}. A value of {1} was found. ", resultsCount, value));
                ++results[valueAsInt];
            }
            return results;
        }
    }
}

