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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using ShoNS.Viz;
using System.Windows.Forms;
using MBT.Escience;
using Bio.Util;
using ShoNS.Array;
using ShoNS.MathFunc;
//using ShoNS.Visualization;
using Bio.Matrix;

namespace MBT.Escience.Sho
{
    public static class DoubleArrayExtension
    {


 
        public static bool IsSorted(this DoubleArray x)
        {
            Helper.CheckCondition(x.IsVector());
            throw new NotImplementedException();

        }

        /// <summary>
        /// Useful in EM for mixture models when going from the unnormalized posterior of each component for each data pont to the normalized 'responsiblities'/posteriors
        /// </summary>
        /// <param name="x">log of some probabilities</param>
        /// <returns>exponentiate the data, then normalize it so that each row adds up to 1.0</returns>
        public static DoubleArray ExponentiateAndNormalizeRowSumsToOne(this DoubleArray x)
        {
            DoubleArray xNorm = new DoubleArray(x.size0, x.size1);
            //DoubleArray rowSums = x.LogSumAcrossEachRow().Exp();//x.Sum(DimOp.EachRow);
            DoubleArray logRowSums = x.LogSumAcrossEachRow();
            for (int c = 0; c < x.size1; c++)
            {
                //xNorm.SetCol(c, x.GetCol(c).Exp().ElementDiv(rowSums));
                xNorm.SetCol(c, (x.GetCol(c) - logRowSums).Exp());
            }
            return xNorm;
        }

        public static double MostCommonEntryIgnoringNans(this DoubleArray x)
        {
            List<KeyValuePair<double, int>> myKeyValues = HistogramValuesInDescendingOrderIgnoringNan(x);
            return myKeyValues[0].Key;
        }

        public static DoubleArray RepMatColDir(this DoubleArray x, int numRep)
        {
            DoubleArray result = DoubleArray.From(x);
            for (int j = 1; j < numRep; j++)
            {
                result = ShoUtils.CatArrayCol(result, x);
            }
            return result;
        }

        public static DoubleArray RepMatRowDir(this DoubleArray x, int numRep)
        {
            DoubleArray result = DoubleArray.From(x);
            for (int j = 1; j < numRep; j++)
            {
                result = ShoUtils.CatArrayRow(result, x);
            }
            return result;
        }

        public static DoubleArray GetRows(this DoubleArray x, IList<int> theseRows)
        {
            if (x.size1 == 1)
            {
                Slice[] s = new Slice[1];
                s[0] = new Slice(theseRows);
                DoubleArray newX = x.GetSliceDeep(s);
                return newX;
            }
            else
            {
                Slice[] s = new Slice[2];
                s[0] = new Slice(theseRows);
                s[1] = Slice.All;
                DoubleArray newX = x.GetSliceDeep(s);
                return newX;
            }
        }

        public static DoubleArray GetCols(this DoubleArray x, IList<int> theseCols)
        {
            if (x.size0 == 1)
            {
                Slice[] s = new Slice[1];
                s[0] = new Slice(theseCols);
                DoubleArray newX = x.GetSliceDeep(s);
                return newX;
            }
            else
            {
                Slice[] s = new Slice[2];
                s[0] = Slice.All;
                s[1] = new Slice(theseCols);
                DoubleArray newX = x.GetSliceDeep(s);
                return newX;
            }
        }
               

        public static void SetRows(this DoubleArray x, IList<int> theseRows, double thisVal)
        {
            DoubleArray theseVals = ShoUtils.DoubleArrayOnes(theseRows.Count, x.size1) * thisVal;
            x.SetRows(theseRows, theseVals);
        }

        public static void SetCols(this DoubleArray x, IList<int> theseCols, double thisVal)
        {
            DoubleArray theseVals = ShoUtils.DoubleArrayOnes(x.size0, theseCols.Count) * thisVal;
            x.SetCols(theseCols, theseVals);
        }

        public static void SetCols(this DoubleArray x, IList<int> theseCols, DoubleArray theseVals)
        {
            if (x.size0 > 1)
            {
                Slice[] s = new Slice[2];
                s[0] = Slice.All;
                s[1] = new Slice(theseCols);
                x.SetSlice(theseVals, s);
            }
            else
            {
                Slice[] s = new Slice[1];
                s[0] = new Slice(theseCols);
                x.SetSlice(theseVals, s);
            }
        }

        public static void SetRows(this DoubleArray x, IList<int> theseRows, DoubleArray theseVals)
        {
            //if (x.size0 == 1)
            //{
            //    Helper.CheckCondition(theseRows.Count == 1 && theseRows[0] == 0);
            //    Helper.CheckCondition(theseVals.size0 == 1 && theseVals.size1 == x.size1);
            //    for (int i = 0; i < theseVals.Count; i++)
            //    {
            //        x[i] = theseVals[i];
            //    }
            //}
            //else 
            if (x.size1 > 0)
            {
                Slice[] s = new Slice[2];
                s[0] = new Slice(theseRows);
                s[1] = Slice.All;
                x.SetSlice(theseVals, s);
            }
            else
            {
                Slice[] s = new Slice[1];
                s[0] = new Slice(theseRows);
                x.SetSlice(theseVals, s);
            }
        }

        public static void SetRow(this DoubleArray x, int thisRow, DoubleArray thisVal)
        {
            x.SetRows(new List<int> { thisRow }, thisVal);
        }
        public static void SetCol(this DoubleArray x, int thisCol, DoubleArray thisVal)
        {
            x.SetCols(new List<int> { thisCol }, thisVal);
        }

        public static DoubleArray PermuteEachColDifferently(this DoubleArray x, Random rand)
        {
            return PermuteEachRowDifferently(x.T, rand).T;
        }

        public static DoubleArray PermuteEachRowDifferently(this DoubleArray x, Random rand)
        {
            DoubleArray newX = new DoubleArray(x.size0, x.size1);
            IntArray currentPerm = ShoUtils.MakeRangeIntInclusive(0, x.size1 - 1);
            Helper.CheckCondition(currentPerm.Count == x.size1);
            for (int r = 0; r < x.size0; r++)
            {
                IntArray permToUse = currentPerm.RandomPermutation(rand);
                //Console.WriteLine("permToUse: " + permToUse);
                DoubleArray tmpRow = x.GetRow(r);
                tmpRow = tmpRow.GetCols(permToUse);
                newX.SetRow(r, tmpRow);
            }
            return newX;
        }

        public static DoubleArray NormalizeRowSumsToOne(this DoubleArray x)
        {
            DoubleArray xNorm = new DoubleArray(x.size0, x.size1);
            DoubleArray rowSums = x.Sum(DimOp.EachRow);
            for (int c = 0; c < x.size1; c++)
            {
                xNorm.SetCol(c, x.GetCol(c).ElementDivide(rowSums));
            }
            return xNorm;
        }

        public static DoubleArray RemoveRow(this DoubleArray x, int thisRowIndex)
        {
            int numRows = x.size0;
            List<int> inds = SpecialFunctions.CreateRange(numRows).ToList();
            bool success = inds.Remove(thisRowIndex);
            Helper.CheckCondition(success);
            DoubleArray inputDataNull = x.GetRows(inds);
            return inputDataNull;
        }

        public static DoubleArray SetRowsAndCol(this DoubleArray x, IntArray rowInd, int colInd, DoubleArray newVals)
        {
            DoubleArray newArray = DoubleArray.From(x);
            Helper.CheckCondition((rowInd.Count == newVals.Count), "rowInd and newVals must all be the same length");
            for (int i = 0; i < rowInd.Count; i++)
            {
                newArray[rowInd[i], colInd] = newVals[i];
            }
            return newArray;
        }

        public static DoubleArray SetRowAndCols(this DoubleArray x, int rowInd, IntArray colInd, DoubleArray newVals)
        {
            DoubleArray newArray = DoubleArray.From(x);
            Helper.CheckCondition((colInd.Count == newVals.Count), "colInd, and newVals must all be the same length");
            for (int i = 0; i < colInd.Count; i++)
            {
                newArray[rowInd, colInd[i]] = newVals[i];
            }
            return newArray;
        }

        /// <summary>
        ///NOT SURE WHO WROTE THIS, but my code never did this
        /// wrap Sho's slice so that it behaves like our own stuff (exclusive of the last index)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="firstRow"></param>
        /// <param name="rowStep"></param>
        /// <param name="maxRow"></param>
        /// <param name="firstCol"></param>
        /// <param name="colStep"></param>
        /// <param name="maxCol"></param>
        /// <returns></returns>
        //public static DoubleArray GetSlice(this DoubleArray x, int firstRow, int rowStep, int maxRow, int firstCol, int colStep, int maxCol)
        //{
        //    //if (maxRow == 0 || maxCol)
        //    //{
        //    //    return new DoubleArray(0);
        //    //}

        //    return x.GetSlice(firstRow, rowStep, maxRow - 1, firstCol, colStep, maxCol - 1);
        //    //return x.Slice(firstRow, rowStep, maxRow - 1, firstCol, colStep, maxCol - 1);
        //}
        /// <summary>
        /// exclusive of last index
        /// </summary>
        /// <param name="x"></param>
        /// <param name="firstRow"></param>
        /// <param name="rowStep"></param>
        /// <param name="maxRow"></param>
        /// <returns></returns>
        public static DoubleArray GetRowSlice(this DoubleArray x, int firstRow, int rowStep, int maxRow)
        {
            return x.GetSliceDeep(firstRow, maxRow, rowStep, 0, x.size1 - 1, 1);

            //if (maxRow == 0)
            //{
            //    return new DoubleArray(0);
            //}
            //Slice[] s = new Slice[2];
            //s[0] = new Slice(ShoUtils.MakeRangeIntInclusive(firstRow, rowStep, maxRow));
            //s[1] = Slice.All;
            //DoubleArray r = x.GetSliceDeep(s);//.GetSlice(s);
            //return r;
        }
        /// <summary>
        /// exclusive of last index
        /// </summary>
        /// <param name="x"></param>
        /// <param name="firstCol"></param>
        /// <param name="colStep"></param>
        /// <param name="maxCol"></param>
        /// <returns></returns>
        public static DoubleArray GetColSlice(this DoubleArray x, int firstCol, int colStep, int maxCol)
        {
            return x.GetSliceDeep(0, x.size0 - 1, 1, firstCol, maxCol, colStep);
        }

        public static List<KeyValuePair<double, int>> HistogramValuesInDescendingOrderIgnoringNan(this DoubleArray x)
        {
            Dictionary<double, int> hist = x.HistogramIgnoringNan();
            var sortedKeyValuePairQuery =
            from keyValuePair in hist
            orderby keyValuePair.Value descending
            select keyValuePair;
            return sortedKeyValuePairQuery.ToList();
        }


        //count up how many of each unique value there is
        public static Dictionary<double, int> HistogramIgnoringNan(this DoubleArray x)
        {
            DoubleArray uniqueVals = x.Unique();
            int N = uniqueVals.Count;
            Dictionary<double, int> histogram = new Dictionary<double, int>(N);
            for (int n = 0; n < N; n++)
            {
                double thisVal = uniqueVals[n];
                if (!double.IsNaN(thisVal))
                {
                    IntArray posOfTheseVals = x.Find(elt => elt == thisVal);
                    histogram.Add(thisVal, posOfTheseVals.Count);
                }
            }
            return histogram;
        }

        public static bool AllZeros(this DoubleArray x, double tol)
        {
            IntArray tmpInd = x.Find(elt => Math.Abs(elt) > tol);
            return tmpInd.Count == 0;
        }

        /// <summary>
        /// check that each element is the same (within tol) of each other: useful for checking belief prop log likelihoods, for e.g.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool ValuesAreTheSameWithinTol(this DoubleArray x, double tol)
        {
            double oneVal = x[0, 0];
            for (int i = 0; i < x.size0; i++)
            {
                for (int j = 0; j < x.size1; j++)
                {
                    if (Math.Abs(oneVal - x[i, j]) > tol) return false;
                }
            }
            return true;
        }

        /// <summary>
        ///Currently does this by calling it on each row--not sure how efficient that is
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static DoubleArray LogSumAcrossEachRow(this DoubleArray x)
        {
            int numRows = x.size0;
            DoubleArray logsum = new DoubleArray(numRows, 1);
            for (int r = 0; r < numRows; r++)
            {
                logsum[r] = SpecialFunctions.LogSumParams(x.GetRow(r).ToArray<double>());
            }
            return logsum;
        }

        /// <summary>
        /// Useful in EM for mixture models when going from the unnormalized posterior of each component for each data pont to the normalized 'responsiblities'/posteriors
        /// </summary>
        /// <param name="x">log of some probabilities</param>
        /// <returns>exponentiate the data, then normalize it so that each row adds up to 1.0</returns>
        //public static DoubleArray ExponentiateAndNormalizeRowSumsToOne(this DoubleArray x)
        //{
        //    DoubleArray xNorm = new DoubleArray(x.size0, x.size1);
        //    //DoubleArray rowSums = x.LogSumAcrossEachRow().Exp();//x.Sum(DimOp.EachRow);
        //    DoubleArray logRowSums = x.LogSumAcrossEachRow();
        //    for (int c = 0; c < x.size1; c++)
        //    {
        //        //xNorm.SetCol(c, x.GetCol(c).Exp().ElementDivide(rowSums));
        //        xNorm.SetCol(c, (x.GetCol(c) - logRowSums).Exp());
        //    }
        //    return xNorm;
        //}

        //public static DoubleArray NormalizeColSumsToOne(this DoubleArray x)
        //{
        //    return x.T.NormalizeRowSumsToOne().T;
        //}

        //public static DoubleArray NormalizeRowSumsToOne(this DoubleArray x)
        //{
        //    DoubleArray xNorm = new DoubleArray(x.size0, x.size1);
        //    DoubleArray rowSums = x.Sum(DimOp.EachRow);
        //    for (int c = 0; c < x.size1; c++)
        //    {
        //        xNorm.SetCol(c, x.GetCol(c).ElementDivide(rowSums));
        //    }
        //    return xNorm;
        //}


        /// <summary>
        /// normalize the matrix so that the sum of all of its entries sum to one
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static DoubleArray NormalizeToSumToOne(this DoubleArray x)
        {
            return x / x.Sum();
        }

        public static int GetFirstIndexOfMaxVal(this DoubleArray x, out double maxVal)
        {
            maxVal = x.Max();//so can use it inside the lambda expression below
            double tmpMax = maxVal;
            IntArray maxIndArr = x.Find(elt => elt == tmpMax);
            return maxIndArr[0];
        }

        public static int GetFirstIndexOfMinVal(this DoubleArray x, out double minVal)
        {
            IntArray tmp = GetIndexesOfMinVal(x, out minVal);
            return tmp[0];
        }

        public static int GetFirstIndexOfMinVal(this DoubleArray x)
        {
            IntArray tmp = GetIndexesOfMinVal(x);
            return tmp[0];
        }

        public static IntArray GetIndexesOfMinVal(this DoubleArray x)
        {
            double garb;
            return GetIndexesOfMinVal(x, out garb);
        }

        /// <summary>
        /// Get indexes in Array of where the min val lies (possibly more than one)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="minVal"></param>
        /// <returns></returns>
        public static IntArray GetIndexesOfMinVal(this DoubleArray x, out double minVal)
        {
            minVal = x.Min();//so can use it inside the lambda expression below
            double tmpMin = minVal;
            IntArray minIndArr = x.Find(elt => elt == tmpMin);
            return minIndArr;
        }


                [Obsolete("See escience matrix similarityMeasures and KernelNormalizers")]        
        /// <summary>
        /// the way Ashish normalizes his kernels for GPR (also described here: http://www.cbrc.jp/~tsuda/pdf/kmcb_primer.pdf)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static DoubleArray NormalizeSpecial(this DoubleArray x)
        {
            DoubleArray xnew = new DoubleArray(x.size0, x.size1);
            for (int r = 0; r < x.size0; r++)
            {
                for (int c = 0; c < x.size1; c++)
                {
                    xnew[r, c] = x[r, c] / (Math.Sqrt(x[r, r]) * Math.Sqrt(x[c, c]));
                }
            }
            return xnew;
        }

        public static bool EqualsFast(this DoubleArray x, DoubleArray y)
        {
            for (int j = 0; j < x.Length; j++)
            {
                if (x[j] != y[j]) return false;
            }
            return true;
        }

        public static string ToStringKey(this DoubleArray x)
        {
            //x = x.ToVector();
            x = x.ToVector();
            StringBuilder sb = new StringBuilder();
            foreach (int elt in x)
            {
                sb.Append(elt);
            }
            return sb.ToString();

        }

        public static double ToScalar(this DoubleArray x)
        {
            Helper.CheckCondition(x.IsScalar());
            return x[0];
        }


        /// <summary>
        /// Take integer features which lie in rows, and make windows of them into single integer features
        ///which we then convert to oneHot encoding. Was using this to compute "windowed" kernels in GWAS, but the one-hot encoding may not be necessary and
        /// be making things really, really slow. instead, I am now using  DoubleArray.ComputeWindowedSimilarity to give the kernel resulting from one window feature
        /// </summary>
        /// <param name="x"></param>
        /// <param name="winSize"></param>
        /// <returns></returns>
        public static DoubleArray ConvertIntRowFeaturesToWindowedOneHotWithPossibleMissingVals(this DoubleArray x, int winSize, IntArray possibleVals, ParallelOptions parallelOptions)
        {

            throw new Exception("to use this, should change Dictionary to be <string><int> instead of <DoubleArray><int>, but really this is obsolete anyhow as I now use ComputeWindowedSimilarity");
            bool QUIET = true;
            int M = x.size0;
            int N = x.size1;
            int numVals = possibleVals.Length;
            int numPossibleFeaturesPerWindow = (int)Math.Pow(numVals, winSize);
            int numWindows = M - winSize + 1;
            int numTotalOneHotFeatures = numPossibleFeaturesPerWindow * numWindows;

            IEqualityComparer<DoubleArray> myVectorArrayComparer = new VectorArrayComparer();

            DoubleArray newDat = new DoubleArray(numTotalOneHotFeatures, N);

            //keep mapping between different windows and a int encoding
            //slide through every window now
            int featNum = 0;
            for (int m = winSize - 1; m < M; m++)
            //doing the parallelization up a level now, because changed things to save on memory useage, and because the Dictionary is not thread-safe
            //IN FACT, AS SOMETIMES USED, THIS LOOP ONLY GOES THROUGH ONCE NOW
            //Parallel.For(winSize - 1, M, parallelOptions, m =>
            {
                Dictionary<DoubleArray, double> countUniqueObservedWindows = new Dictionary<DoubleArray, double>(myVectorArrayComparer);
                List<int> hasMissingVals = new List<int>();

                IntArray featIndForThisWindow = ShoUtils.MakeRangeIntInclusive(m - winSize + 1, m);

                if (!QUIET) System.Console.WriteLine("working on window " + m + " of " + M + " in DoubleArray.ConvertIntRowFeaturesToWindowedOneHotWithPossibleMissingVals");

                if (!QUIET) System.Console.Write("counting unique observed windows...");
                for (int n = 0; n < N; n++)
                {
                    DoubleArray thisDat = x.GetRowsAndCols(featIndForThisWindow, n);
                    if (!thisDat.HasNaN())
                    {
                        countUniqueObservedWindows[thisDat] = countUniqueObservedWindows.GetValueOrDefault<DoubleArray, double>(thisDat, 0.0) + 1.0;
                    }
                    else
                    {
                        hasMissingVals.Add(n);
                    }
                }
                if (!QUIET) System.Console.WriteLine("...done 1");

                if (!QUIET) System.Console.Write("make a mapping from unique windows to integers and use this in a one-hot encoding");
                //now make a mapping from unique windows to integers and use this in a one-hot encoding
                Dictionary<DoubleArray, int> mappingFromDoubleArrayToInt = new Dictionary<DoubleArray, int>(myVectorArrayComparer);
                List<DoubleArray> uniqueDoubleArrays = countUniqueObservedWindows.Keys.ToList();
                for (int j = 0; j < countUniqueObservedWindows.Count; j++)
                {
                    mappingFromDoubleArrayToInt.Add(uniqueDoubleArrays[j], j);
                }
                if (!QUIET) System.Console.WriteLine("...done 2");

                if (!QUIET) System.Console.Write("converting counts to relative probabilities for imputation");
                //convert counts to relative probabilities for imputation
                double N2 = (double)(N - hasMissingVals.Count);
                foreach (var key in countUniqueObservedWindows.Keys.ToList())
                {
                    countUniqueObservedWindows[key] = countUniqueObservedWindows[key] / N2;
                }
                (DoubleArray.From(countUniqueObservedWindows.Values.ToList())).CheckVectorSumsToOne();
                if (!QUIET) System.Console.WriteLine("...done 3");

                if (!QUIET) System.Console.Write("converting to one-hot encoding with another loop through...");
                //now convert to one-hot encoding with another loop through:
                int firstInd = featNum * numPossibleFeaturesPerWindow;
                int lastInd = firstInd + numPossibleFeaturesPerWindow - 1;
                IntArray allowedInd = ShoUtils.MakeRangeIntInclusive(firstInd, lastInd);
                for (int n = 0; n < N; n++)
                {
                    DoubleArray thisDat = x.GetRowsAndCols(featIndForThisWindow, n);
                    if (!thisDat.HasNaN())
                    {
                        newDat[allowedInd[mappingFromDoubleArrayToInt[thisDat]], n] = 1;
                    }
                    else
                    {
                        foreach (DoubleArray key in mappingFromDoubleArrayToInt.Keys)
                        {
                            //this was supposed to be a square root... adding it in now
                            //newDat[allowedInd[mappingFromDoubleArrayToInt[key]], n] = countUniqueObservedWindows[key];
                            newDat[allowedInd[mappingFromDoubleArrayToInt[key]], n] = Math.Sqrt(countUniqueObservedWindows[key]);
                        }
                    }
                }
                if (!QUIET) System.Console.WriteLine("...done 4");
                featNum++;
            }
            //);
            return newDat;
        }

        public static void CheckVectorSumsToOne(this DoubleArray x)
        {
            Helper.CheckCondition(Math.Abs(1 - x.ToVector().Sum()) <= 1e-5);
            //Helper.CheckCondition(Math.Abs(1 - x.ToVector().Sum()) <= 1e-5);
        }

        public static void CheckRowsSumToOne(this DoubleArray marginalProbs)
        {
            DoubleArray uniqueVals = marginalProbs.Sum(DimOp.EachRow).Unique();
            double tol = 1e-5;
            foreach (double elt in uniqueVals) Helper.CheckCondition(Math.Abs(elt - 1) < tol);
        }


        public static DoubleArray CoerceToBeFullRankBySvdRowRemoval(this DoubleArray D, out bool madeChange)
        {
            return D.T.CoerceToBeFullRankBySvdColRemoval(out madeChange).T;
        }

        public static DoubleArray CoerceToBeFullRankBySvdColRemoval(this DoubleArray D, out bool madeChange)
        {
            Helper.CheckCondition(D.size0 >= D.size1, "need to have at least as many individuals as features");
            double tol = 1e-8;
            //note, centering the data will remove another rank, so I think I don't want to do this
            //DoubleArray K = ShoUtils.ComputeKernelMatrixFromInnerProductOfMeanCenteredData(this.DoubleArray.T);
            //note, assuming that D is M x N where M is numFeatures, and N is numFixed effects, there are two identical ways to do this, and it does not matter 
            //whether or not we center the data (this is just for PCA interpretation of breakdown of variance
            //1) compute newX=US=XV where X=USV', or 2) compute newX=WD^(1/2) where XX'=WDW'
            DoubleArray X = D;
            SVD svd = new SVD(X);
            DoubleArray eigVals = (DoubleArray)(svd.D).Diagonal;
            IntArray badDimensions = eigVals.Find(elt => Math.Abs(elt) < tol);
            madeChange = (badDimensions.Count > 0) ? true : false;
            DoubleArray projections;
            DoubleArray newD = D;
            if (madeChange)
            {
                IntArray goodDimensions = eigVals.Find(elt => Math.Abs(elt) >= tol);
                DoubleArray Uk = svd.U.GetCols(goodDimensions);
                DoubleArray Sk = svd.D.GetRowsAndCols(goodDimensions, goodDimensions);
                projections = Uk.Multiply(Sk);
                newD = projections;
            }
            return newD;
        }

        //public static void RemoveMeanColValueFromEachEntry(this DoubleArray d)
        //{
        //    DoubleArray meanForEachCol = d.Mean(DimOp.EachCol);
        //    for (int j = 0; j < d.size0; j++)
        //    {
        //        d.SetRow(j, d.GetRow(j) - meanForEachCol);
        //    }
        //}


        public static DoubleArray PseudoInverse(this DoubleArray x)
        {
            SVD svd = new SVD(x);
            return svd.PseudoInverse();
        }

        public static List<string> ToStringList(this DoubleArray m)
        {
            return m.ToList().ConvertAll<string>(x => x.ToString());
        }

        /// <summary>
        /// Author: Carl. Given a list of keys, removes these from both the row and columns.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="cidIndexToRemove"></param>
        /// <returns></returns>
        public static DoubleArray RemoveRowAndColumnSymmetric(this DoubleArray x, int cidIndexToRemove)
        {
            //there maybe a sho-ier way to do this
            DoubleArray newX = new DoubleArray(x.size0 - 1, x.size1 - 1);
            //!!!could be in Parallel
            int newRowIndex = -1;
            for (int rowIndex = 0; rowIndex < x.size0; ++rowIndex)
            {
                if (rowIndex == cidIndexToRemove)
                {
                    continue; //not break;
                }
                ++newRowIndex;
                int newColIndex = -1;
                for (int colIndex = 0; colIndex < x.size1; ++colIndex)
                {
                    if (colIndex == cidIndexToRemove)
                    {
                        continue; //not break;
                    }
                    ++newColIndex;
                    newX[newRowIndex, newColIndex] = x[rowIndex, colIndex];
                }
            }
            return newX;
        }

        public static DoubleArray CoercePositiveDefiniteWithAddDiagonal(this DoubleArray x, double smallVal)
        {
            Helper.CheckCondition(smallVal >= 0.0);
            Helper.CheckCondition(x.IsSymmetric());
            //throw new Exception("check that this is working correctly");
            DoubleArray newX = AddToDiagonal(x, smallVal);
            return newX;
        }

        public static DoubleArray AddToDiagonal(this DoubleArray x, double smallVal)
        {
            DoubleArray newX = x + ShoUtils.Identity(x.size0, x.size0) * smallVal;
            return newX;
        }


        /// <summary>
        /// Decompose the matrix using Eigenvalue decomp. Sets all values less than smallVal to be smallVal.
        ///Precondition: matrix is symmetric
        /// </summary>
        /// <param name="x"></param>
        /// <param name="smallVal">positive value to replace 0/negative eigenvalues</param>
        /// <returns></returns>
        public static DoubleArray CoercePositiveDefiniteWithEigDecomp(this DoubleArray x, double smallVal)
        {
            //throw new Exception("consensus seems to be it is better to add a small amount to the entire diagonal, so use CoercePositiveDefiniteWithAddDiagonal");
            Helper.CheckCondition(smallVal > 0.0);
            Helper.CheckCondition(x.IsSymmetric());
            EigenSym eigen = new EigenSym(x);
            DoubleArray diagValues = eigen.D.ReplaceVal(elt => elt < smallVal, smallVal);
            DoubleArray diagMatrix = ShoUtils.MakeDiag(diagValues);
            DoubleArray posDefMatrix = eigen.V.Multiply(diagMatrix).Multiply(eigen.V.T);
            return posDefMatrix;
        }

        public static bool IsPositiveDefinite(this DoubleArray x)
        {
            return x.IsSymmetric() && (!double.IsNegativeInfinity(x.LogDet()));
            //double tol = 0.0;
            //return IsPositiveDefinite(x, tol);
        }

        //not sure how to do this with a tolerance any more...
        // public static bool IsPositiveDefinite(this DoubleArray x, double tol)
        // {
        //     return x.IsSymmetric() && (x.LogDet() + tol > 0.0);
        //return x.IsSymmetric() && (x.Det()+tol >0.0);

        //EigenVals eObject = new EigenVals(x);
        //DoubleArray eigs = ((ComplexArray) eObject.D).Real();
        //if (eigs.Min()<=tol) return false;
        //return true;
        // }

        /// <summary>
        /// not sure if MKL is smart enough to do this properly
        /// </summary>
        /// <returns></returns>
        public static DoubleArray InverseOfDiagonalMatrix(this DoubleArray x)
        {
            DoubleArray y = DoubleArray.From(x);
            DoubleArray invDiag = x.Diagonal.DotInv();
            y.Diagonal = invDiag;
            return y;
        }

        /// <summary>
        /// Not sure if this is quite stupid or not. I know that sometimes I can call x.Inv() and get an answer, but not "new Cholesky(x)", so 
        ///I'm thinking that Inv() may sometimes return an answer that is unstable, and this is my attempt to avoid that
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static DoubleArray InverseFromCholesky(this DoubleArray x)
        {
            return ShoUtils.InvFromChol(new Cholesky(x));
        }

        public static Cholesky TryCholesky(this DoubleArray x)
        {
            Cholesky chol = ShoNS.Array.Cholesky.TryCreate(x);
            return chol;
        }

        /// <summary>
        /// This is truly Sho's Cholesky (which I have mapped to somethign else (see below)
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns>should call .Succeeded on the Cholesky object returned to make sure it succeeded</returns>
        public static Cholesky TryCholesky(this DoubleArray x, out bool succeeded)
        {
            Cholesky chol = ShoNS.Array.Cholesky.TryCreate(x);
            succeeded = chol.U != null;
            return chol;
        }

        public static Cholesky Cholesky(this DoubleArray x)
        {
            Cholesky chol = new Cholesky(x);
            //double tol = 0.0;//not sure what tolerance Sho actually uses
            //if ((!chol.Succeeded) && !x.IsSymmetric(tol)) throw new Exception("Cholesky failed because matrix is not symmetric (though not sure what tol Sho uses)");
            //if ((!chol.Succeeded) && !x.IsPositiveDefinite()) throw new Exception("Cholesky failed because matrix is not positive definite (though not sure what tol Sho uses)");
            //if ((!chol.Succeeded)) throw new Exception("Cholesky failed");
            //throw new NotImplementedException("Chol.Succeeded is no longer available.");

            return chol;
        }

        public static DoubleArray SetDiagonal(this DoubleArray x, double val)
        {
            int minDimSize = Math.Min(x.size0, x.size1);
            for (int i = 0; i < minDimSize; i++)
            {
                x[i, i] = val;
            }
            return x;
        }

        public static bool IsSquare(this DoubleArray x)
        {
            if (x.size0 == x.size1) return true;
            else return false;
        }

        public static bool IsSymmetric(this DoubleArray arr, double tol)
        {
            if (arr.size0 != arr.size1) return false;     // must be square
            for (int i = 0; i < arr.size0; ++i)
            {
                for (int j = i + 1; j < arr.size0; ++j)
                {
                    double diff = Math.Abs(arr[i, j] - arr[j, i]);             // Check that arr[i,j] is approximattely equal to  arr[j,i]
                    if (diff > tol) return false;
                }
            }
            return true;
            //return (x - x.T).Norm(NormType.Infinity) <= tol;//SLOW and INEFFICIENT
        }

        public static bool IsSymmetric(this DoubleArray x)
        {
            double tol = 1e-7;
            return IsSymmetric(x, tol);
        }

        public static string ToStringVec(this DoubleArray myVec, string delim)
        {
            Helper.CheckCondition(myVec.IsVector(), "input must be a vector");
            string result = myVec[0].ToString();
            for (int j = 1; j < myVec.Length; j++)
            {
                result += delim + myVec[j];
            }
            return result;
        }


        public static string ToString2(this DoubleArray x)
        {
            if (!x.IsScalar())
            {
                return x.ToString().TrimEnd(new char[] { ';' });
            }
            else
            {
                return x.ToString().Trim(new char[] { ';', ']', '[' });
            }
        }

        /// <summary>
        /// Number of times to replicate the given row [column] vector to make many row [column] vectors
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        //public static DoubleArray RepVec(this DoubleArray x, int numRep)
        //{
        //    Helper.CheckCondition(x.IsVector(), "Is not a vector");
        //    bool IS_ROW_VECTOR = x.size0 == 1;
        //    int maxDimSize = Math.Max(x.size0, x.size1);
        //    DoubleArray xrep = (IS_ROW_VECTOR) ? new DoubleArray(numRep, maxDimSize) : new DoubleArray(maxDimSize, numRep);

        //    for (int r = 0; r < numRep; r++)
        //    {
        //        if (IS_ROW_VECTOR)
        //        {
        //            xrep.SetRow(r, x);
        //        }
        //        else
        //        {
        //            xrep.SetCol(r, x);
        //        }
        //    }
        //    return xrep;
        //}

        /// <summary>
        /// Similar to Matlabs' imstats. Returns a string the gives the statistics of the input matrix.
        /// </summary>
        /// <param name="x"></param>
        /// <returns>r+=</returns>
        public static string Imstats(this DoubleArray x)
        {
            //string r = "Matrix statistics:\n";
            //r+="Range: [" + x.Min() + "," + x.Max() + "]\n";
            //r += "Mean=" + x.Mean() + ", Std=" + x.Std() + ", Median=" + x.Median();

            string r = "Matrix statistics: ";
            r += "Range: [" + x.Min() + "," + x.Max() + "]";
            r += "Mean=" + x.Mean() + ", Std=" + x.Std() + ", Median=" + x.Median();
            //System.Console.WriteLine(r);
            return r;
        }

        public static string GetSizeString(this DoubleArray x, string varName)
        {
            return varName + ".size0=" + x.size0 + ", " + varName + ".size1=" + x.size1;
        }

        public static DoubleArray Unique(this DoubleArray x)
        {
            //return ShoUtils.ToDoubleArray(x.Diff().Distinct().ToList());
            return ShoUtils.ToDoubleArray(x.Distinct().ToList());
        }

        public static bool IsFullRank(this DoubleArray x)
        {
            bool isFullRank = true;
            double rank = x.Rank();
            double maxRank = Math.Min(x.size0, x.size1);
            if (rank < maxRank) isFullRank = false;
            return isFullRank;
        }

        ///<summary>
        /// First compute the SVD and then extracts the rank.
        /// </summary>
        public static double Rank(this DoubleArray x)
        {
            SVD svd = new SVD(x);
            //double tol = Double.Epsilon;
            return svd.Rank();
        }

        public static DoubleArray GetRowsAndCols(this DoubleArray x, IntArray rowInd, int colInd)
        {
            return x.GetRowsAndCols(rowInd, new IntArray(new int[] { colInd }));
        }

        public static DoubleArray GetRowsAndCols(this DoubleArray x, int rowInd, IntArray colInd)
        {
            return x.GetRowsAndCols(new IntArray(new int[] { rowInd }), colInd);
        }

        /// <summary>
        /// Select the sub matrix of x consisting of some rows and some columns. Currently a deep copy, but could be changed to shallow...
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static DoubleArray GetRowsAndCols(this DoubleArray x, IList<int> rowInd, IList<int> colInd)
        {
            //throw new NotImplementedException("I think this is right, but haven't tested it. It's adapted from Sumit's example.");
            Slice[] slices = new Slice[2];
            slices[0] = new Slice(rowInd);
            slices[1] = new Slice(colInd);
            return x.GetSliceDeep(slices);

            //throw new NotImplementedException("Need to change to GetSlice()");
            //DoubleArray r;
            //try
            //{
            //    //r = x.GetRows(rowInd).GetCols(colInd);
            //}
            //catch (Exception)
            //{
            //    r = null;
            //}
            //return r;
        }

        /// <summary>
        /// Check whether array contains any instances of a particular value
        /// </summary>
        public static bool HasX(this DoubleArray x, double thisVal)
        {

            if (double.IsNaN(thisVal))
            {
                return HasNaN(x);
            }
            else
            {
                return x.Contains(thisVal);
            }
        }

        public static bool HasInf(this DoubleArray x)
        {
            for (int j0 = 0; j0 < x.size0; j0++)
            {
                for (int j1 = 0; j1 < x.size1; j1++)
                {
                    if (double.IsInfinity(x[j0, j1]))
                        return true;
                }
            }
            return false;
        }

        public static bool HasNaN(this DoubleArray x)
        {
            for (int j0 = 0; j0 < x.size0; j0++)
            {
                for (int j1 = 0; j1 < x.size1; j1++)
                {
                    if (double.IsNaN(x[j0, j1]))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// will only work for pos. def matrix x
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double LogDetFromCholesky(this DoubleArray x)
        {
            bool success;
            Cholesky chol = x.TryCholesky(out success);
            Helper.CheckCondition(success, "matrix was probalby not strictly positive definite");
            return ShoUtils.LogDetFromChol(chol);
        }

        /// <summary>
        /// Stable way to compute the log of the determinant of a semi-positive-definite matrix.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double LogDet(this DoubleArray x)
        {
            SVD svd = new SVD(x);
            return svd.D.Diagonal.Log().Sum();

            //this only works for positive-definite (not semi-positive definite)
            //2*sum(log(diag(U))); (Tom Minka)
            //Cholesky chol = new Cholesky(x);
            //DoubleArray Udiag = chol.U.Diagonal;
            //return 2 * Udiag.Log().Sum(); 
        }

        /// <summary>
        /// put a single DoubleArray into a new list
        /// </summary>
        /// <returns></returns>
        public static List<DoubleArray> PutInList(this DoubleArray x)
        {
            DoubleArray[] tmpAr = { x };
            return new List<DoubleArray>(tmpAr);
        }

        /// <summary>
        /// check once at the start that SlicePythonic works as we think (that  is, with a bug)
        /// </summary>
        //private static bool _pythonicSliceCheckForce = SlicePythonicFunctionalityCheck();

        ///!!!! better not to have any global state. It makes hard to parallelize and hard to repro results
        //private static Random _myRand = new Random(123456);

        static public DoubleArray ToDoubleArray(this DoubleArray doubleArray, Func<double, double> selector)
        {
            DoubleArray result = new DoubleArray(doubleArray.size0, doubleArray.size1);
            for (int i = 0; i < doubleArray.size0; ++i)
            {
                for (int j = 0; j < doubleArray.size1; ++j)
                {
                    result[i, j] = selector(doubleArray[i, j]);
                }
            }
            return result;
        }

        static public double[][] ToRaggedArray(this DoubleArray doubleArray)
        {
            double[][] result = new double[doubleArray.size0][];//(doubleArray.size0, doubleArray.size1);
            for (int i = 0; i < doubleArray.size0; ++i)
            {
                result[i] = new double[doubleArray.size1];
                for (int j = 0; j < doubleArray.size1; ++j)
                {
                    result[i][j] = doubleArray[i, j];
                }
            }
            return result;
        }

        static public double[][] ToRealArray(this ComplexArray doubleArray)
        {
            double[][] result = new double[doubleArray.size0][];//(doubleArray.size0, doubleArray.size1);
            for (int i = 0; i < doubleArray.size0; ++i)
            {
                result[i] = new double[doubleArray.size1];
                for (int j = 0; j < doubleArray.size1; ++j)
                {
                    Helper.CheckCondition(Math.Abs(doubleArray[i, j].Imaginary) < 1E-9, "{0} is not real.", doubleArray[i,j]);
                    result[i][j] = doubleArray[i, j].Real;
                }
            }
            return result;
        }


        public static double[] ToOneDArray(this DoubleArray array)
        {
            double[] result = new double[array.Length];
            for (int i = 0; i < result.Length; i++)
                result[i] = array[i];
            return result;
        }

        /// <summary>
        /// see DotPow
        /// </summary>
        /// <param name="x"></param>
        /// <param name="powerToRaiseTo"></param>
        /// <returns></returns>
        public static DoubleArray Pow(this DoubleArray x, double powerToRaiseTo)
        {
            return (DoubleArray)ShoNS.MathFunc.ArrayMath.Pow(x, powerToRaiseTo);
        }

        public static DoubleArray LogDebug(this DoubleArray x)
        {
            return (DoubleArray)ShoNS.MathFunc.ArrayMath.Log(x);
            //return x.Map(elt => Math.Log(elt));
        }

        public static DoubleArray Log(this DoubleArray x)
        {
            return (DoubleArray)ShoNS.MathFunc.ArrayMath.Log(x);
            //return x.Map(elt => Math.Log(elt));
        }

        public static DoubleArray Exp2(this DoubleArray x)
        {
            return (DoubleArray)ShoNS.MathFunc.ArrayMath.Exp(x);
        }

        public static DoubleArray Exp(this DoubleArray x)
        {
            return (DoubleArray)ShoNS.MathFunc.ArrayMath.Exp(x);
            //return x.Map(elt => Math.Log(elt));
        }

        public static DoubleArray DotInv(this DoubleArray x)
        {            
            return x.RDivide(1);
        }

        public static DoubleArray DotPow(this DoubleArray x, double powerToRaiseTo)
        {
            return x.Map(elt => Math.Pow(elt, powerToRaiseTo));
        }

        public static DoubleArray Map(this DoubleArray x, Func<double, double> myFunc)
        {
            //return (DoubleArray)ShoNS.MathFunc.Map.DoubleMap(new DoubleFunc(myFunc, "mapping function"), x);

            DoubleArray newX = new DoubleArray(x.size0, x.size1);
            for (int j0 = 0; j0 < x.size0; j0++)
            {
                for (int j1 = 0; j1 < x.size1; j1++)
                {
                    newX[j0, j1] = myFunc(x[j0, j1]);
                }
            }
            return newX;
        }

        /// <summary>
        /// Take the Abs of an array
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static DoubleArray Abs(this DoubleArray x)
        {
            //Carl says this is a pain to use and requires writing seperate tiny classes for each function I would want to use, e.g. Abs
            //return x.Map(Math.Abs);
            return (DoubleArray)ShoNS.MathFunc.ArrayMath.Abs(x);
        }

        public static DoubleArray FillRandUseSeed(this DoubleArray x)
        {
            throw new Exception("change this to FillRandUseSeed(Random myRand)");
        }

        public static DoubleArray FillRandUseSeed(this DoubleArray x, Random myRand)
        {
            DoubleArray y = DoubleArray.From(x);

            if (false)//checking for bug in built-in Sho Function FillRand()
            {
                for (int j = 0; j < x.Length; j++)
                {
                    y[j] = myRand.NextDouble();
                }
            }
            else
            {
                y.FillRandom(myRand); //this one will be reproducible
            }
            return y;
        }

        /// <summary>
        /// Given a 1D matrix (a vector), permute it's entries
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static DoubleArray RandomPermutation(this DoubleArray x)
        {
            throw new NotImplementedException("see IntArray RandomPermutation");
        }

        public static DoubleArray RemoveNans(this DoubleArray x)
        {
            //throw new NotImplementedException("Need to switch to GetSlice()");
            if (!x.IsVector()) throw new Exception("only works on vector arrays");
            IntArray nonNanInd = x.Find(elt => !Double.IsNaN(elt));
            if (x.size0 == 1)
            {
                return x.GetCols(nonNanInd);
            }
            else
            {
                return x.GetRows(nonNanInd);
            }
        }

        /// <summary>
        /// return all rows that contain more than one value (not including missing data)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static IntArray FindRowsThatVary(this DoubleArray x)
        {
            return FindRows(x, NotHasUniqueValueExcludeNans);
        }

        public static bool NotHasUniqueValueExcludeNans(this DoubleArray x)
        {
            DoubleArray tmp = x.Unique();
            tmp = tmp.RemoveNans();
            return tmp.Count > 1;
        }

        public static IntArray FindRows(this DoubleArray x, Predicate<DoubleArray> testPredicate)
        {
            List<int> ind = new List<int>();
            {
                for (int j0 = 0; j0 < x.size0; j0++)
                {
                    if (testPredicate(x.GetRow(j0)))
                    {
                        ind.Add(j0);
                    }
                }
            }
            return ShoUtils.ToIntArray(ind);
        }

        public static IntArray FindColsWithNoNans(this DoubleArray x)
        {
            return FindRowsWithNoNans(x.T).T;
        }

        /// <summary>
        /// return the list of row indexes for which no entries are NaN
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static IntArray FindRowsWithNoNans(this DoubleArray x)
        {
            DoubleArray isNan = x.GetIsNanMask();
            DoubleArray tmpSum = isNan.Sum(DimOp.EachRow);
            IntArray goodInd = tmpSum.Find(elt => elt == 0.0);
            return goodInd;
        }

        public static IntArray FindColsWithAllNans(this DoubleArray x)
        {
            return x.T.FindRowsWithAllNans();
        }

        public static IntArray FindRowsWithAllNans(this DoubleArray x)
        {
            DoubleArray isNan = x.GetIsNanMask();
            DoubleArray tmpSum = isNan.Sum(DimOp.EachRow);
            IntArray theseInd = tmpSum.Find(elt => elt == x.size1);
            return theseInd;
        }

        public static IntArray FindColsWithAtLeastOneNan(this DoubleArray x)
        {
            return FindRowsWithAtLeastOneNan(x.T).T;
        }


        /// <summary>
        /// return the list of row indexes for which at least one entry is NaN
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static IntArray FindRowsWithAtLeastOneNan(this DoubleArray x)
        {
            DoubleArray isNan = x.GetIsNanMask();
            DoubleArray tmpSum = isNan.Sum(DimOp.EachRow);
            IntArray goodInd = tmpSum.Find(elt => elt > 0.0);
            return goodInd;
        }

        public static DoubleArray GetIsNanMask(this DoubleArray x)
        {
            DoubleArray isNanMask = new DoubleArray(x.size0, x.size1);
            for (int j0 = 0; j0 < x.size0; j0++)
            {
                for (int j1 = 0; j1 < x.size1; j1++)
                {
                    if (double.IsNaN(x[j0, j1])) isNanMask[j0, j1] = 1.0;
                }
            }
            return isNanMask;
        }

        public static List<Tuple<int, int>> FindPairs(this DoubleArray x, Predicate<double> testPredicate)
        {
            List<Tuple<int, int>> ind = new List<Tuple<int, int>>();
            for (int i = 0; i < x.size0; i++)
            {
                for (int j = 0; j < x.size1; j++)
                {
                    if (testPredicate(x[i, j]))
                    {
                        ind.Add(Tuple.Create(i, j));
                    }
                }
            }
            return ind;
        }

        /// <summary>
        /// Like doing ind=find(x satisfies some condition) in Matlab
        /// </summary>
        /// <param name="x">must be a vector, otherwise use FindPairs</param>
        /// <param name="testPredicate"></param>
        /// <returns></returns>
        public static IntArray Find(this DoubleArray x, Predicate<double> testPredicate)
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
                throw new NotImplementedException("see FindPairs");
            }
            return ShoUtils.ToIntArray(ind);

        }

        public static bool InNonDecreasingOrder(this DoubleArray x)
        {
            Helper.CheckCondition(x.IsVector());
            for (int j = 1; j < x.Length; j++)
            {
                if (x[j] < x[j - 1])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks that x contains only the values 1.0 and 0.0 (useful for functions which assume binary target variables)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsBinaryArray(this DoubleArray x)
        {
            HashSet<double> distinctVals = x.Distinct().ToHashSet();
            HashSet<double> allowedVals = new HashSet<double>(new double[] { 0, 1 });
            distinctVals.ExceptWith(allowedVals);

            return distinctVals.Count == 0;
        }


        public static DoubleArray Diff(this DoubleArray x)
        {
            Helper.CheckCondition(x.IsVector());
            DoubleArray r = new DoubleArray(x.size0, x.size1);
            for (int j = 0 + 1; j < x.Length; j++)
            {
                r[j] = x[j] - x[j - 1];
            }
            return r;
        }

        public static List<double> SortWithInd(List<double> x, out List<int> sortedInd)
        {
            IntArray sortedIndTmp;
            DoubleArray result = SortWithInd(DoubleArray.From(x), out sortedIndTmp);
            sortedInd = sortedIndTmp.ToList();
            return result.ToList();
        }

        /// <summary>
        /// Sorts the data, but also gives you indexes to undo the sort
        /// </summary>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="listInput"></param>
        /// <param name="sortedInd">x(sortedInd)=sortedVals</param>
        /// <param name="sortedInd2">x=sortedVals(sortedInd2)</param>
        /// <returns>sort(listInput)</returns>
        public static DoubleArray SortWithInd(this DoubleArray x, out IntArray sortedInd, out IntArray sortedInd2)
        {
            Helper.CheckCondition(x.IsVector(), "can only use on vectors");
            List<int> sInd, sInd2;
            List<double> sortedVals = SpecialFunctions.SortWithInd(x.ToVector().ToList(), out sInd, out sInd2);
            sortedInd = IntArray.From(sInd);
            sortedInd2 = IntArray.From(sInd2);
            DoubleArray result = DoubleArray.From(sortedVals);
            if (result.size0 != x.size0) result = result.T;
            return result;
        }


        /// <summary>
        /// Gives similar functionality to Matlab's [sortVal,sortIn]=sort(x), except that it is more
        /// general in that it allows one to sort any vector of values and not just those in x, by
        /// the ordering imposed by x. 
        /// </summary>
        /// <param name="x">list of values to sort</param>
        /// <param name="sortedInd">Outputs the sorted values</param>
        /// <returns>The indexes of the sorted values</returns>
        public static DoubleArray SortWithInd(this DoubleArray x, out IntArray sortedInd)
        {
            Helper.CheckCondition(x.IsVector());
            x = x.ToVector();
            //x = x.ToVector();

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
                //x = x.ToVector();
                Array tmp1 = x.ToList().ToArray();
                Array tmp2 = Enumerable.Range(0, x.size1).ToArray();
                Array.Sort(tmp1, tmp2);
                sortedInd = IntArray.From(tmp2);
                DoubleArray sortedVals = DoubleArray.From(tmp1);
                return sortedVals;
            }
            else
            {
                sortedInd = new IntArray(0);
                return x;
            }
        }

        /// <summary>
        /// Allow GetCols to work with an empty array
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static DoubleArray GetColsE(this DoubleArray x, IList<int> colsToGet)
        {
            throw new NotImplementedException("Need to switch to GetSlice()");
            if (colsToGet.Count > 0)
            {
                //return x.GetCols(colsToGet);
            }
            else
            {
                return new DoubleArray(0);
            }
        }

        public static double GetDouble(this DoubleArray x)
        {
            Helper.CheckCondition(x.IsScalar(), "x is not scalar");
            return x[0, 0];
        }

        public static bool IsScalar(this DoubleArray x)
        {
            if (x.size0 == 1 && x.size1 == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns>true of x has only one dimension of size great than 1</returns>
        public static bool IsVector(this DoubleArray x)
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

        public static bool IsEmpty(this DoubleArray x)
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

        ///Commenting out for now as part of .NET 4 upgrade
        //public static void ImageView(this DoubleArray arr)
        //{
        //    double minVal = arr.Min();
        //    double maxVal = arr.Max();
        //    ImageView(arr, minVal, maxVal);
        //}

        //public static void ImageView(this DoubleArray arr, double minVal, double maxVal)
        //{
        //    Form f = new Form();
        //    PictureBox pb = new PictureBox();
        //    pb.Dock = DockStyle.Fill;
        //    //Bitmap b = ArrayImage.GetArrayImage(arr, 0, 1);
        //    Bitmap b = ArrayImage.GetArrayImage(arr, minVal, maxVal);
        //    pb.Image = b;
        //    f.Controls.Add(pb);
        //    f.ShowDialog();
        //}

        /// <summary>
        /// element-wise square root
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static DoubleArray Sqrt(this DoubleArray x)
        {
            return (DoubleArray)ShoNS.MathFunc.ArrayMath.Sqrt(x);
        }

        /// <summary>
        /// Equivalent (I think) to Matlab's '\' or mldivide operator. That is
        /// A\B is similar to (but more stable than) INV(A)*B
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        public static DoubleArray Mldivide(this DoubleArray A, DoubleArray B)
        {
            DoubleArray result = new DoubleArray(0);
            try
            {
                result = new QR(A).LSSolve(B);
            }
            catch (Exception)
            {
                //System.Console.WriteLine("Error with LSSolve");
                try
                {
                    result = A.Inv().Multiply(B);
                }
                catch
                {
                    throw new Exception("ml divide failed due to singularity");
                }
            }
            return result;
        }

        ///Commenting out for now as part of .NET 4 upgrade
        ///// <summary>
        ///// Equivalent (I think) to Matlab's '/' or mrdivide operator. That is
        ///// A/B is similar to (but more stable than) A*INV(B)
        ///// </summary>
        ///// <param name="B"></param>
        ///// <returns></returns>
        //public static DoubleArray Mrdivide(this DoubleArray A, DoubleArray B)
        //{
        //    DoubleArray result = new DoubleArray(0);
        //    try
        //    {
        //        result = new QR(B.Transpose()).LSSolve(A.Transpose()).T;
        //    }
        //    catch (System.ArgumentException e)
        //    {
        //        A.WriteToCsvInBaseDirectory("A");
        //        B.WriteToCsvInBaseDirectory("B");
        //        System.Console.WriteLine(e.Message);
        //        System.Console.WriteLine("Writing matrixes to file to examine in Matlab");
        //    }
        //    return result;
        //}


        public static void WriteToDelimitedFile(this DoubleArray x, string filename, char delim)
        {
            //ShoNS.IO.DlmWriter.dlmwrite(filename, delim, x);
            ShoNS.IO.DelimFileWriter.Write(filename, delim, x);
            System.Console.Write("finished writing to: " + filename);
        }


        public static void WriteToCsv(this DoubleArray x, string filename)
        {
            WriteToDelimitedFile(x, filename, ',');
        }

        public static void WriteToTabDelimFile(this DoubleArray x, string filename)
        {
            WriteToDelimitedFile(x, filename, '\t');
        }


        /// <summary>
        /// Computes unique values seen in x and compute histogram with these as centers.
        /// (This only makes sense if x contains integers, for example
        /// </summary>
        public static Histogram MakeAndWriteHist(this DoubleArray x)
        {
            var myRange = x.Distinct().ToList();
            myRange.Remove(Double.NaN);
            return MakeAndWriteHist(x, myRange);
        }

        /// <summary>
        /// Useage: Histogram myHist = snpData.MakeAndWriteHist(new DoubleRange(-1,2,1));
        ///This specifies the bin centers as -1,0,1,2.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="centers"></param>
        /// <returns></returns>
        public static Histogram MakeAndWriteHist(this DoubleArray x, IList centers)
        {
            Histogram myHist = new ShoNS.MathFunc.Histogram(x, centers);
            Console.WriteLine(myHist);
            return myHist;
        }

        public static DoubleArray GetUniqueValues(this DoubleArray x)
        {
            DoubleArray unVals = DoubleArray.From(x.Distinct());
            return unVals.Sort();
        }

        /// <summary>
        /// print out the unique values of a DoubleArray to the console, using the name, nameString
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static void PrintUniqueVals(this DoubleArray x, string nameString)
        {
            System.Console.WriteLine("Unique vals for " + nameString + ": " + x.Distinct().StringJoin(", "));
        }

        public static DoubleArray ReplacePosInf(this DoubleArray x, double newVal)
        {
            return x.ReplaceVal(double.IsPositiveInfinity, newVal);
        }

        public static bool HasAny(this DoubleArray x, Predicate<double> testPredicate)
        {
            //int numHits = 0;
            Helper.CheckCondition(x != null);
            for (int j0 = 0; j0 < x.size0; j0++)
            {
                for (int j1 = 0; j1 < x.size1; j1++)
                {
                    if (testPredicate(x[j0, j1]))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        public static void TakeMinOfInPlace(this DoubleArray x, double val)
        {
            for (int j0 = 0; j0 < x.size0; j0++)
            {
                for (int j1 = 0; j1 < x.size1; j1++)
                {
                    x[j0, j1] = Math.Min(x[j0, j1], val);
                }
            }
        }

        public static void TakeMaxOfInPlace(this DoubleArray x, double val)
        {
            for (int j0 = 0; j0 < x.size0; j0++)
            {
                for (int j1 = 0; j1 < x.size1; j1++)
                {
                    x[j0, j1] = Math.Max(x[j0, j1], val);
                }
            }
        }


        public static DoubleArray ReplaceVal(this DoubleArray x, Predicate<double> testPredicate, double newVal)
        {
            int numHits = 0;
            Helper.CheckCondition(x != null);
            for (int j0 = 0; j0 < x.size0; j0++)
            {
                for (int j1 = 0; j1 < x.size1; j1++)
                {
                    if (testPredicate(x[j0, j1]))
                    {
                        x[j0, j1] = newVal;
                        numHits++;
                    }
                }
            }
            return x;
        }

        public static DoubleArray ReplaceVal(this DoubleArray x, double origVal, double newVal)
        {
            Helper.CheckCondition(x != null);
            for (int j0 = 0; j0 < x.size0; j0++)
            {
                for (int j1 = 0; j1 < x.size1; j1++)
                {
                    bool hit = false;
                    if (Double.IsNaN(origVal) && Double.IsNaN(x[j0, j1]))
                    {
                        hit = true;
                    }
                    else if ((x[j0, j1] == origVal))
                    {
                        hit = true;
                    }
                    if (hit)
                    {
                        x[j0, j1] = newVal;
                    }
                }
            }
            return x;
        }

        public static IntArray ToIntArray(this DoubleArray x, int missingValEntry)
        {
            IntArray xI = new IntArray(x.size0, x.size1);
            for (int i0 = 0; i0 < x.size0; i0++)
            {
                for (int i1 = 0; i1 < x.size1; i1++)
                {
                    if (double.IsNaN(x[i0, i1]))
                    {
                        xI[i0, i1] = missingValEntry;
                    }
                    else
                    {
                        xI[i0, i1] = (int)x[i0, i1];
                    }
                }
            }
            return xI;
        }

        /// <summary>
        /// override built-in ToList so that it returns an empty list when x is empty
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static List<double> ToListE(this DoubleArray x)
        {
            if (x.IsEmpty())
            {
                return new List<double>();
            }
            else
            {
                return x.ToList();
            }
        }

        /// <summary>
        ///take the element-wise square of a matrix. Matlab syntax: return X.*X
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static DoubleArray DotSquare(this DoubleArray x)
        {
            return x.ElementMultiply(x);
        }

        public static DoubleArray ImputeNanValuesWithRowMedian(this DoubleArray x, ParallelOptions parallelOptions)
        {
            return ImputeNanValuesWithRowMedian(x, parallelOptions, false);
        }

        public static DoubleArray ImputeNanValuesWithRowMedian(this DoubleArray x, ParallelOptions parallelOptions, bool useMeanInstead)
        {
            //throw new NotImplementedException("Switch to SetSlice()");

            DoubleArray xFill = ShoUtils.DoubleArrayNaNs(x.size0, x.size1);
            //for (int r = 0; r < x.size0; r++)
            Parallel.For(0, x.size0, r =>
                        {
                            DoubleArray thisRow = x.GetRow(r);
                            double thisValue;
                            if (useMeanInstead)
                            {
                                thisValue = thisRow.RemoveNans().Mean();
                            }
                            else
                            {
                                thisValue = thisRow.RemoveNans().Median();
                            }
                            
                            thisRow = thisRow.ReplaceVal(elt => double.IsNaN(elt),thisValue);
                            xFill.SetRow(r, thisRow);
                        }
            );
            return xFill;
        }

        public static ComplexArray MatrixExp(this DoubleArray matrix, double multiplier = 1)
        {
            Eigen eigenSystem = new Eigen(matrix);

            ComplexArray valuesExp = ComplexArray.From(((ComplexArray)eigenSystem.D).Select(c => System.Numerics.Complex.Exp(c * multiplier)));
            ComplexArray matrixExp = ((ComplexArray)eigenSystem.V) * valuesExp * ((ComplexArray)eigenSystem.V).Inv();
            return matrixExp;
        }
    }

}

