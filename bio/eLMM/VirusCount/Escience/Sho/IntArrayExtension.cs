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
using MBT.Escience;
using Bio.Util;
using ShoNS.Array;

namespace MBT.Escience.Sho
{
    public static class IntArrayExtension
    {

        //[duplicated code from DoubleArrayExtension]
        public static IntArray GetRowSlice(this IntArray x, int firstRow, int rowStep, int maxRow)
        {
            return x.GetSliceDeep(firstRow, maxRow, rowStep, 0, x.size1 - 1, 1);
        }

        //[duplicated code from DoubleArrayExtension]
        public static IntArray GetColSlice(this IntArray x, int firstCol, int colStep, int maxCol)
        {
            return x.GetSliceDeep(0, x.size0 - 1, 1, firstCol, maxCol, colStep);
        }

        //[duplicated code from DoubleArrayExtension]
        public static IntArray GetRows(this IntArray x, IList<int> theseRows)
        {
            if (x.size1 == 1)
            {
                Slice[] s = new Slice[1];
                s[0] = new Slice(theseRows);
                IntArray newX = x.GetSliceDeep(s);
                return newX;
            }
            else
            {
                Slice[] s = new Slice[2];
                s[0] = new Slice(theseRows);
                s[1] = Slice.All;
                IntArray newX = x.GetSliceDeep(s);
                return newX;
            }
        }

        //[duplicated code from DoubleArrayExtension]
        public static IntArray GetCols(this IntArray x, IList<int> theseCols)
        {
            if (x.size0 == 1)
            {
                Slice[] s = new Slice[1];
                s[0] = new Slice(theseCols);
                IntArray newX = x.GetSliceDeep(s);
                return newX;
            }
            else
            {
                Slice[] s = new Slice[2];
                s[0] = Slice.All;
                s[1] = new Slice(theseCols);
                IntArray newX = x.GetSliceDeep(s);
                return newX;
            }
        }


        public static bool HasNaN(this IntArray x)
        {
            throw new Exception("IntArrays cannot store NaNs must use an alternative such as int.MaxValue");
            //return false;
        }


        public static IntArray Unique(this IntArray x)
        {
            return IntArray.From(x.Distinct().ToList());
        }

        /// <summary>
        /// element-wise square root
        /// </summary>
        /// <returns></returns>
        public static DoubleArray Sqrt(this IntArray x)
        {
            return (DoubleArray)ShoNS.MathFunc.ArrayMath.Sqrt(x);
        }

        public static bool EqualsFast(this IntArray x, IntArray y)
        {
            for (int j = 0; j < x.Length; j++)
            {
                if (x[j] != y[j]) return false;
            }
            return true;
        }

        public static string ToStringKey(this IntArray x)
        {
            x = x.ToVector();
            //x = x.ToVector();
            StringBuilder sb = new StringBuilder();
            foreach (int elt in x)
            {
                sb.Append(elt);
            }
            return sb.ToString();

        }

        public static bool HasMissing(this IntArray x, int missingVal)
        {
            for (int j0 = 0; j0 < x.size0; j0++)
            {
                for (int j1 = 0; j1 < x.size1; j1++)
                {
                    if (x[j0, j1] == missingVal)
                        return true;
                }
            }
            return false;
        }

        //private static Random _myRand = new Random(123456);

        /// <summary>
        /// Like doing ind=find(x satisfies some condition) in Matlab
        /// </summary>
        /// <param name="x"></param>
        /// <param name="testPredicate"></param>
        /// <returns></returns>
        public static IntArray Find(this IntArray x, Predicate<double> testPredicate)
        {
            List<int> ind = new List<int>();
            if (x.IsVector())
            {
                for (int j = 0; j < x.Length; j++)
                {
                    if (testPredicate(x[j]))
                    {
                        ind.Add(j);
                    }
                }
            }
            else
            {
                throw new NotImplementedException();
            }
            return ShoUtils.ToIntArray(ind);

        }


        /// <summary>
        /// Built-in ToList seems to carsh if x is empty, so wrap around it (I may be wrong and this is useless--
        /// hard to say with lack of Sho documetnation...
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static List<int> ToListOrEmpty(this IntArray x)
        {
            List<int> listInd;
            if (!x.IsEmpty())
            {
                listInd = x.ToList();
            }
            else
            {
                listInd = new List<int>();
            }
            return listInd;
        }

        public static bool IsEmpty(this IntArray x)
        {
            if (x.size0 == 0 && x.size1 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static DoubleArray ToDoubleArray(this IntArray x)
        {
            DoubleArray xD = DoubleArray.From(x);
            for (int i0 = 0; i0 < x.size0; i0++)
            {
                for (int i1 = 0; i1 < x.size1; i1++)
                {
                    xD[i0, i1] = (int)x[i0, i1];
                }
            }
            return xD;
        }

        public static bool IsVector(this IntArray x)
        {
            if (x.size0 > 1 && x.size1 > 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public static IntArray RandomPermutation(this IntArray x)
        {
            //return RandomPermutation(x, _myRand);
            throw new Exception("change this to RandomPermutation(Random myRand)");
        }

        /// <summary>
        /// Given a 1D matrix (a vector), permute it's entries
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static IntArray RandomPermutation(this IntArray x, Random rand)
        {
            Helper.CheckCondition(x.IsVector(), "method only takes vectors not matrixes");
            List<int> xList = x.ToList();
            //Random myRand = new Random();
            xList.ShuffleInPlace(rand);
            return IntArray.From(xList);
        }

        ///Commenting out for now as part of .NET 4 upgrade
        //public static string WriteToCSV(this IntArray x, string filename)
        //{
        //    string fullFileName = ShoUtils.GetCSVFilename(filename);
        //    ShoNS.IO.DlmWriter.dlmwrite(fullFileName, ",", x);
        //    return fullFileName;
        //}

        //public static string WriteToCSVNoDate(this IntArray x, string filename)
        //{
        //    string fullFileName = ShoUtils.GetCSVFilenameNoDate(filename);
        //    ShoNS.IO.DlmWriter.dlmwrite(fullFileName, ",", x);
        //    return fullFileName;
        //}

        /// <summary>
        /// Gives similar functionality to Matlab's [sortVal,sortIn]=sort(x), except that it is more
        /// general in that it allows one to sort any vector of values and not just those in x, by
        /// the ordering imposed by x. 
        /// </summary>
        /// <param name="x">list of values to sort</param>
        /// <param name="sortedInd">Outputs the sorted values</param>
        /// <returns>The indexes of the sorted values</returns>
        public static IntArray SortWithInd(this IntArray x, out IntArray sortedInd)
        {
            Helper.CheckCondition(x.IsVector());
            x = x.ToVector();

            //ensures that if the values are all tied, the ordering does not change
            //if (x.Unique().Length == 1 && x.Unique()[0]==0)
            //{
            //    sortedInd = ShoUtils.MakeRangeInt(0, x.Length-1);
            //    return x;
            //}

            if (!x.IsEmpty())
            {
                Helper.CheckCondition(x.IsVector());
                x = x.ToVector();
                Array tmp1 = x.ToList().ToArray();
                Array tmp2 = Enumerable.Range(0, x.size1).ToArray();
                Array.Sort(tmp1, tmp2);
                sortedInd = IntArray.From(tmp2);
                IntArray sortedVals = IntArray.From(tmp1);
                return sortedVals;
            }
            else
            {
                sortedInd = new IntArray(0);
                return x;
            }
        }

    }
}
