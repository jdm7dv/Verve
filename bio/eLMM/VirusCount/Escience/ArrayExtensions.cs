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
using System.Linq;
using Bio.Util;
using System.Collections.Generic;

namespace MBT.Escience
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Returns -1 if not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public static int FindIndex<T>(this T[] arr, Predicate<T> match)
        {
#if !SILVERLIGHT
            return Array.FindIndex(arr, match);
#else
            for (int i = 0; i < arr.Length; ++i)
            {
                if (match(arr[i]))
                {
                    return i;
                }
            }
            return -1;
#endif
        }

        public static void Normalize(this double[] e)
        {
            e.Normalize(false);
        }

        public static void Normalize(this double[] e, bool isLogspace)
        {
            if (isLogspace)
            {
                double logSum = SpecialFunctions.LogSumParams(e);
                for (int i = 0; i < e.Length; i++)
                {
                    e[i] -= logSum;
                }
            }
            else
            {
                double sum = e.Sum();
                for (int i = 0; i < e.Length; i++)
                {
                    e[i] /= sum;
                }
            }
        }

        public static void Normalize(this double[][] e)
        {
            e.Normalize(false);
        }
        public static void Normalize(this double[][] e, bool isLogspace)
        {
            if (isLogspace)
            {
                double logSum = e.SelectMany(d => d).LogSum();
                for (int i = 0; i < e.Length; i++)
                    for (int j = 0; j < e[i].Length; j++)
                        e[i][j] -= logSum;
            }
            else
            {
                double sum = e.SelectMany(d => d).Sum();
                for (int i = 0; i < e.Length; i++)
                    for (int j = 0; j < e[i].Length; j++)
                        e[i][j] /= sum;
            }
        }

        public static string TableView(this double[][] m)
        {
            string result = m.Select(row => row.StringJoin("\t")).StringJoin("\n");
            return result;
        }

        public static string TableView(this double[,] m)
        {
            string result = Enumerable.Range(0, m.GetLength(0)).Select(row => Enumerable.Range(0, m.GetLength(1)).Select(col => m[row, col]).StringJoin("\t")).StringJoin("\n");
            return result;
        }

        public static void Multiply(this IList<double> arr, double x)
        {
            for (int i = 0; i < arr.Count; i++)
                arr[i] *= x;
        }

        public static void Multiply(this double[][] arr, double x)
        {
            for (int i = 0; i < arr.Length; i++)
                for (int j = 0; j < arr[i].Length; j++)
                    arr[i][j] *= x;
        }

        public static void Multiply(this double[,] arr, double x)
        {
            for (int i = 0; i < arr.GetLength(0); i++)
                for (int j = 0; j < arr.GetLength(1); j++)
                    arr[i, j] *= x;
        }

        public static void Addition(this double[] sourceArr, double element)
        {
            for (int i = 0; i < sourceArr.Length; i++)
                sourceArr[i] += element;
        }

        public static void Addition(this double[] sourceArr, double[] elementsToAdd)
        {
            Helper.CheckCondition(sourceArr.Length == elementsToAdd.Length, "Arrays are not the same length.");

            for (int i = 0; i < sourceArr.Length; i++)
                sourceArr[i] += elementsToAdd[i];
        }

        public static void Addition(this double[,] sourceArr, double[,] elementsToAdd)
        {
            Helper.CheckCondition(sourceArr.Length == elementsToAdd.Length, "Arrays are not the same length.");

            for (int i = 0; i < sourceArr.GetLength(0); i++)
                for (int j = 0; j < sourceArr.GetLength(1); j++)
                    sourceArr[i, j] += elementsToAdd[i, j];
        }

        public static void Addition(this IList<double> sourceArr, IList<double> elementsToAdd)
        {
            Helper.CheckCondition(sourceArr.Count == elementsToAdd.Count, "Arrays are not the same length.");

            for (int i = 0; i < sourceArr.Count; i++)
                sourceArr[i] += elementsToAdd[i];
        }

        public static void Addition(this double[] sourceArr, IList<double> elementsToAdd)
        {
            Helper.CheckCondition(sourceArr.Length == elementsToAdd.Count, "Arrays are not the same length.");

            for (int i = 0; i < sourceArr.Length; i++)
                sourceArr[i] += elementsToAdd[i];
        }


        public static void Addition(this double[][] sourceArr, double[][] elementsToAdd)
        {
            Helper.CheckCondition(sourceArr.Length == elementsToAdd.Length, "Arrays are not the same length.");

            for (int i = 0; i < sourceArr.Length; i++)
                for (int j = 0; j < sourceArr[i].Length; j++)
                {
                    Helper.CheckCondition(sourceArr[i].Length == elementsToAdd[i].Length, "Arrays are not the same length.");
                    sourceArr[i][j] += elementsToAdd[i][j];
                }
        }

        public static void Addition(this double[][] sourceArr, double element)
        {
            for (int i = 0; i < sourceArr.Length; i++)
                for (int j = 0; j < sourceArr[i].Length; j++)
                {
                    sourceArr[i][j] += element;
                }
        }

        public static void Subtract(this double[] sourceArr, double element)
        {
            for (int i = 0; i < sourceArr.Length; i++)
                sourceArr[i] -= element;
        }

        //public static void Subtract(this double[] sourceArr, double[] elementsToAdd)
        //{
        //    Helper.CheckCondition(sourceArr.Length == elementsToAdd.Length, "Arrays are not the saame length.");

        //    for (int i = 0; i < sourceArr.Length; i++)
        //        sourceArr[i] -= elementsToAdd[i];
        //}

        public static void Subtract(this double[] sourceArr, IList<double> elementsToAdd)
        {
            Helper.CheckCondition(sourceArr.Length == elementsToAdd.Count, "Arrays are not the same length.");

            for (int i = 0; i < sourceArr.Length; i++)
                sourceArr[i] -= elementsToAdd[i];
        }


        public static void Subtract(this double[][] sourceArr, double[][] elementsToAdd)
        {
            Helper.CheckCondition(sourceArr.Length == elementsToAdd.Length, "Arrays are not the same length.");

            for (int i = 0; i < sourceArr.Length; i++)
                for (int j = 0; j < sourceArr[i].Length; j++)
                {
                    Helper.CheckCondition(sourceArr[i].Length == elementsToAdd[i].Length, "Arrays are not the same length.");
                    sourceArr[i][j] -= elementsToAdd[i][j];
                }
        }

        public static void Subtract(this double[][] sourceArr, double element)
        {
            for (int i = 0; i < sourceArr.Length; i++)
                for (int j = 0; j < sourceArr[i].Length; j++)
                {
                    sourceArr[i][j] -= element;
                }
        }

        public static T[][] DeepClone<T>(this T[][] array)
        {
            if (array == null) return null;

            T[][] result = new T[array.Length][];
            for (int i = 0; i < array.Length; i++)
            {
                result[i] = new T[array[i].Length];
                Array.Copy(array[i], result[i], array[i].Length);
            }
            return result;
        }
    }
}
