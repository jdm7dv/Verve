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
using MBT.Escience;
using Bio.Util;
using MBT.Escience.Matrix;
using Bio.Matrix;
using ShoNS.Array;
using System.Threading.Tasks;
using System.IO;

namespace MBT.Escience.Sho
{
    public static class MatrixExtensionsJenn
    {
        public static List<TColKey> FindColKeysWhereFirstRowSatisfies<TRowKey, TColKey, TValue>(this Matrix<TRowKey, TColKey, TValue> x, Predicate<TValue> testPredicate)
        {
            List<TColKey> tmpList = FindColIndsWhereFirstRowSatisfies(x, testPredicate).Select(elt => x.ColKeys[elt]).ToList();
            return tmpList;
        }

        public static List<int> FindColIndsWhereFirstRowSatisfies<TRowKey, TColKey, TValue>(this Matrix<TRowKey, TColKey, TValue> x, Predicate<TValue> testPredicate)
        {
            List<TColKey> colKeys = x.ColKeys.ToList();
            List<int> r = new List<int>();
            for (int j = 0; j < x.ColCount; j++)
            {
                if (testPredicate(x[0, j]))
                {
                    r.Add(j);
                }
            }
            return r;
        }

        public static List<string> ReadColKeysFromNonSparseMatrix(string filename)
        {
            List<string> keys = null;
            using (TextReader textReader = Bio.Util.FileUtils.OpenTextStripComments(filename))
            {
                keys = textReader.ReadLine().Split('\t').ToList();
            }
            return keys.Except(keys[0]).ToList();
        }


        public static bool TryPutIfKeysExistOrDoNothing<TRowKey, TColKey, TVal>(this Matrix<TRowKey, TColKey, TVal> x, TRowKey rowKey, TColKey colKey, TVal val)
        {
            bool containsRowAndColKey = x.ContainsRowAndColKeys(rowKey, colKey);
            if (containsRowAndColKey)
            {
                x[rowKey, colKey] = val;
            }
            return containsRowAndColKey;
        }

        public static Matrix<TRowKey, TColKey, TValue> SelectRandomRows<TRowKey, TColKey, TValue>(this Matrix<TRowKey, TColKey, TValue> x, Random myRand, int numToKeep)
        {
            List<TRowKey> rowsToKeep = x.RowKeys.ToList().Shuffle(myRand).ToList();
            return x.SelectRowsView(rowsToKeep.SubSequence(0, numToKeep));
        }

        public static Matrix<TRowKey, TColKey, TValue> ApplyFunctionToRowKeys<TRowKey, TColKey, TValue>(this Matrix<TRowKey, TColKey, TValue> x, Func<TRowKey, TRowKey> myFunc)
        {
            List<TRowKey> featuresLowerCase = x.RowKeys.Select(myFunc).ToList();
            var featuresLowerCaseAndOldFeatureNameSequence = x.RowKeys.Select(rowKey => new KeyValuePair<TRowKey, TRowKey>(myFunc(rowKey), rowKey));
            x = x.RenameRowsView(featuresLowerCaseAndOldFeatureNameSequence);
            return x;
        }

        public static List<TColKey> FindColKeysCorrespondingFirstFeatureBeingInExternalList<TRowKey, TColKey, TValue>(this Matrix<TRowKey, TColKey, TValue> x, List<TValue> featuresToKeep)
        {
            int firstFeatureInd = 0;
            List<TColKey> colKeysToKeep = new List<TColKey>();
            List<TColKey> colKeys = x.ColKeys.ToList();
            for (int i = 0; i < colKeys.Count; i++)
            {
                TColKey thisCid = colKeys[i];
                TValue thisFeature = x[firstFeatureInd, i];
                if (featuresToKeep.Contains(thisFeature))
                {
                    colKeysToKeep.Add(thisCid);
                }
            }
            List<TValue> tmpVals = x.SelectRowsAndColsView(new List<TRowKey> { x.RowKeys.ToList()[0] }, colKeysToKeep).Values.ToList<TValue>();
            Helper.CheckCondition(SpecialFunctions.SetEquals(tmpVals, featuresToKeep), "not all featuresToKeep were in matrix");
            return colKeysToKeep;
        }
        static public Matrix<TRowKey, TColKey, TValue> SelectColsViewComplement<TRowKey, TColKey, TValue>(this Matrix<TRowKey, TColKey, TValue> parentMatrix,
                   IEnumerable<TColKey> colKeyEnumerable)
        {
            return parentMatrix.SelectRowsAndColsView(parentMatrix.RowKeys, parentMatrix.ColKeys.Except(colKeyEnumerable));
        }

        static public Matrix<TRowKey, TColKey, TValue> SelectRowsViewComplement<TRowKey, TColKey, TValue>(this Matrix<TRowKey, TColKey, TValue> parentMatrix,
                         IEnumerable<TRowKey> rowKeyEnumerable)
        {
            return parentMatrix.SelectRowsAndColsView(parentMatrix.RowKeys.Except(rowKeyEnumerable), parentMatrix.ColKeys);
        }


        public static Matrix<TRowKey, TColKey, TValue> FilterMatrixColsForMaxFracMissingRowEntries<TRowKey, TColKey, TValue>(this Matrix<TRowKey, TColKey, TValue> x, double maxFracMissing)
        {
            var colsToKeep =
                from col in x.ColKeys.AsParallel().AsOrdered()
                let colView = x.ColView(col)
                let nonMissingCount = colView.Count()
                let nonMissingFraction = (double)nonMissingCount / (double)x.RowCount
                where nonMissingFraction >= 1 - maxFracMissing
                select col;
            return x.SelectColsView(colsToKeep);
        }

        public static ShoMatrix AverageColSubsetsInSameGroup(this ShoMatrix M, Matrix<string, string, string> groupIds, out ShoMatrix numInstancesPerGroup, ParallelOptions parallelOptions)
        {
            return AverageColSubsetsInSameGroup(M, groupIds, out numInstancesPerGroup, parallelOptions, -1);
        }

        //for each row, average any columns that have the same groupId if numRepeat=-1
        public static ShoMatrix AverageColSubsetsInSameGroup(this ShoMatrix M, Matrix<string, string, string> groupIds, out ShoMatrix numInstancesPerGroup, ParallelOptions parallelOptions, int repeatNum)
        {
            M.CheckEqualColKeys(groupIds.ColKeys.ToList());
            Helper.CheckCondition(groupIds.RowCount == 1, "group ids should have only one row");

            List<string> uniqueGroupIds = groupIds.Unique().ToList();
            int G = uniqueGroupIds.Count;
            DoubleArray averagedResults = new DoubleArray(M.RowCount, G);
            DoubleArray numInstancesPerGroupArray = ShoUtils.DoubleArrayZeros(1, G);
            BoolArray haveProcessedGroupG = new BoolArray(1, G);

            for (int g = 0; g < G; g++)
            {
                string gId = uniqueGroupIds[g];
                List<int> theseCols = new List<int>();
                for (int n = 0; n < groupIds.ColCount; n++)
                {
                    if (groupIds[0, n] == uniqueGroupIds[g])
                    {
                        if (repeatNum < 0 || !haveProcessedGroupG[g])//averaging, or else it is the right repeat
                        {
                            theseCols.Add(n);
                            haveProcessedGroupG[g] = true;
                        }
                    }
                }

                DoubleArray avgValues = M.DoubleArray.GetCols(theseCols).Mean(DimOp.OverCol);
                averagedResults.SetCol(g, avgValues);
                numInstancesPerGroupArray[0, g] = theseCols.Count();
            }
            numInstancesPerGroup = new ShoMatrix(numInstancesPerGroupArray, new List<string>() { "count" }, uniqueGroupIds, double.NaN);
            return new ShoMatrix(averagedResults, M.RowKeys, uniqueGroupIds, double.NaN);
        }

        public static IEnumerable<string> Unique(this Matrix<string, string, string> x)
        {
            return x.Values.Distinct().ToList();
        }

        public static Matrix<TRowKey, TcolKey, TValue> SubSampleDataMatrixRandomlyRows<TRowKey, TcolKey, TValue>(this Matrix<TRowKey, TcolKey, TValue> snpData, int numSnpsToKeep, Random rand)
        {
            var snpsToKeep = snpData.RowKeys.Shuffle(rand).GetRange(0, numSnpsToKeep);
            return snpData.SelectRowsView(snpsToKeep);
        }
        public static Matrix<TRowKey, TcolKey, TValue> SubSampleDataMatrixRandomlyCols<TRowKey, TcolKey, TValue>(this Matrix<TRowKey, TcolKey, TValue> snpData, int numColsToKeep, Random rand)
        {
            var colsToKeep = snpData.ColKeys.Shuffle(rand).GetRange(0, numColsToKeep);
            return snpData.SelectColsView(colsToKeep);
        }


        public static List<double> Unique(this Matrix<string, string, double> x)
        {
            return x.Values.Distinct().ToList();
        }


        public static double Min(this Matrix<string, string, double> x)
        {
            return x.Values.Min();
        }


        public static double Max(this Matrix<string, string, double> x)
        {
            return x.Values.Max();
        }

        public static void IntersectOnColAndRowKeysShoMatrix(ref ShoMatrix m1, ref ShoMatrix m2)
        {
            IntersectOnColKeysShoMatrix(ref m1, ref m2);
            IntersectOnRowKeysShoMatrix(ref m1, ref m2);

        }

        public static List<string> IntersectOnColKeysShoMatrix(ref ShoMatrix m1, ref ShoMatrix m2)
        {
            IList<string> cidsPhen = m2.ColKeys;
            IList<string> cidsSnp = m1.ColKeys;
            List<string> cids = cidsPhen.Intersect(cidsSnp).ToList();
            m1 = m1.SelectColsView(cids).AsShoMatrix();
            m2 = m2.SelectColsView(cids).AsShoMatrix();
            return cids;
        }

        public static List<string> IntersectOnRowKeysShoMatrix(ref ShoMatrix m1, ref ShoMatrix m2)
        {
            IList<string> var2 = m2.RowKeys;
            IList<string> var1 = m1.RowKeys;
            List<string> var = var2.Intersect(var1).ToList();
            m1 = m1.SelectRowsView(var).AsShoMatrix();
            m2 = m2.SelectRowsView(var).AsShoMatrix();
            return var;
        }

        public static void IntersectOnColAndRowKeys(ref Matrix<string, string, double> m1, ref Matrix<string, string, double> m2)
        {
            IntersectOnColKeys(ref m1, ref m2);
            IntersectOnRowKeys(ref m1, ref m2);
        }

        public static List<string> IntersectOnColKeys(ref ShoMatrix m1, ref ShoMatrix m2)
        {
            IList<string> cidsPhen = m2.ColKeys;
            IList<string> cidsSnp = m1.ColKeys;
            List<string> cids = cidsPhen.Intersect(cidsSnp).ToList();
            m1 = m1.SelectColsView(cids).AsShoMatrix();
            m2 = m2.SelectColsView(cids).AsShoMatrix();
            return cids;
        }

        public static List<string> IntersectOnColKeys(ref ShoMatrix m1, ref Matrix<string, string, double> m2)
        {
            IList<string> cidsPhen = m2.ColKeys;
            IList<string> cidsSnp = m1.ColKeys;
            List<string> cids = cidsPhen.Intersect(cidsSnp).ToList();
            m1 = m1.SelectColsView(cids).AsShoMatrix();
            m2 = m2.SelectColsView(cids).AsShoMatrix();
            return cids;
        }

        public static List<string> IntersectOnColKeys(ref Matrix<string, string, double> m1, ref ShoMatrix m2)
        {
            return IntersectOnColKeys(ref m2, ref m1);
        }

        public static List<string> IntersectOnColKeys(ref Matrix<string, string, double> m1, ref Matrix<string, string, double> m2)
        {
            IList<string> cidsPhen = m2.ColKeys;
            IList<string> cidsSnp = m1.ColKeys;
            List<string> cids = cidsPhen.Intersect(cidsSnp).ToList();
            m1 = m1.SelectColsView(cids);
            m2 = m2.SelectColsView(cids);
            return cids;
        }

        public static List<string> IntersectOnRowKeys(ref Matrix<string, string, double> m1, ref Matrix<string, string, double> m2)
        {
            IList<string> var2 = m2.RowKeys;
            IList<string> var1 = m1.RowKeys;
            List<string> var = var2.Intersect(var1).ToList();
            m1 = m1.SelectRowsView(var);//.AsShoMatrix();
            m2 = m2.SelectRowsView(var);//.AsShoMatrix();
            return var;
        }

        public static Matrix<string, string, double> RemoveRowsThatDoNotVary(this Matrix<string, string, double> x)
        {
            List<int> rowsThatVary = x.FindRowsThatVary();
            x = x.SelectRowsView(rowsThatVary).AsShoMatrix();
            return x;
        }

        public static List<int> FindRowsThatVary(this Matrix<string, string, double> x)
        {
            return FindRows(x, HasUniqueValueExcludingNans);
        }


        /// <summary>
        /// For each row in x, concatenate it with the matrix y and see if y is full rank.  If it is not,
        /// remove the row from x.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y">Must have the same number of columns as x</param>
        /// <returns>The new matrix x</returns>
        public static Matrix<string, string, double> RemoveLinearlyDependentRows(this Matrix<string, string, double> x, Matrix<string, string, double> y)
        {
            List<int> linearlyIndependentRows = x.FindLinearlyIndependentRows(y);
            x = x.SelectRowsView(linearlyIndependentRows).AsShoMatrix();
            return x;
        }

        /// <summary>
        /// For each row in x, concatenate it with the matrix y and see if y is full rank.  If so,
        /// add the row's index to the list of linearly independent rows.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y">Must have the same number of columns as x</param>
        /// <returns>A list of rows in x that are linearly independent</returns>
        public static List<int> FindLinearlyIndependentRows(this Matrix<string, string, double> x, Matrix<string, string, double> y)
        {
            return FindRows(x, (row => row.MergeRowsView(true, y).AsShoMatrix().DoubleArray.IsFullRank()));
        }

        public static bool HasUniqueValueExcludingNans(this Matrix<string, string, double> x)
        {
            return x.Unique().Count() > 1;
            //DoubleArray tmp = new DoubleArray(x.UniqueValues());
            //tmp = tmp.RemoveNans();
            //return tmp.Count > 1;
        }

        public static bool NotHasUniqueValueExcludingNans(this Matrix<string, string, double> x)
        {
            return x.Unique().Count() <= 1;
            //DoubleArray tmp = new DoubleArray(x.UniqueValues());
            //tmp = tmp.RemoveNans();
            //return tmp.Count > 1;
        }

        public static List<int> FindCols(this Matrix<string, string, double> x, Predicate<Matrix<string, string, double>> testPredicate)
        {
            List<int> ind = new List<int>();
            {
                for (int j0 = 0; j0 < x.ColCount; j0++)
                {
                    if (testPredicate(x.SelectColsView(j0)))
                    {
                        ind.Add(j0);
                    }
                }
            }
            return ind;
        }

        public static List<int> FindRows(this Matrix<string, string, double> x, Predicate<Matrix<string, string, double>> testPredicate)
        {
            List<int> ind = new List<int>();
            {
                for (int j0 = 0; j0 < x.RowCount; j0++)
                {
                    if (testPredicate(x.SelectRowsView(j0)))
                    {
                        ind.Add(j0);
                    }
                }
            }
            return ind;
        }

        public static Matrix<string, string, double> RemoveMissingValuesFromVector(this Matrix<string, string, double> x)
        {
            if (!x.IsVector()) throw new Exception("only works on vector arrays");
            List<int> nonMissingInd = new List<int>();
            for (int r = 0; r < x.RowCount; r++)
            {
                for (int c = 0; c < x.ColCount; c++)
                {
                    double val;
                    bool hasVal = x.TryGetValue(r, c, out val);
                    if (hasVal)
                    {
                        if (x.IsColumnVector()) nonMissingInd.Add(c);
                        else nonMissingInd.Add(r);
                    }
                }
            }

            if (x is ShoMatrix) x = x.AsShoMatrix();

            if (x.IsRowVector())
            {
                return x.SelectColsView(nonMissingInd);
            }
            else
            {
                return x.SelectRowsView(nonMissingInd);
            }
        }


        public static bool IsColumnVector<TRowValue, TColValue, TValue>(this Matrix<TRowValue, TColValue, TValue> x)
        {
            return x.ColCount == 1;
        }

        public static bool IsRowVector<TRowValue, TColValue, TValue>(this Matrix<TRowValue, TColValue, TValue> x)
        {
            return x.RowCount == 1;
        }

        public static bool IsVector<TRowValue, TColValue, TValue>(this Matrix<TRowValue, TColValue, TValue> x)
        {
            return x.IsColumnVector() || x.IsRowVector();
        }

        /// <summary>
        /// Check if the col ids are the same length and in the same order
        /// </summary>
        public static bool CheckColKeysInSameOrder<TRowKey, TColKey, TValue>(Matrix<TRowKey, TColKey, TValue> m1, Matrix<TRowKey, TColKey, TValue> m2)
        {
            return m1.ColKeys.SequenceEqual(m2.ColKeys);
        }

        public static bool IsSquare<TRowValue, TColValue, TValue>(this Matrix<TRowValue, TColValue, TValue> x)
        {
            return x.RowCount == x.ColCount;
        }

        /// <summary>
        /// useful for ensuring the integrity of a covariance Matrix
        /// </summary>
        /// <returns></returns>
        public static bool CheckThatRowAndColKeysAreIdentical(this Matrix<string, string, double> x)
        {
            if (x.RowKeys.SequenceEqual(x.ColKeys))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Matrix<string, string, double> IntersectColsWithRows(this Matrix<string, string, double> x)
        {
            List<string> commonKeys = x.RowKeys.ToList().Intersect(x.ColKeys).ToList();
            return x.SelectRowsAndColsView(commonKeys, commonKeys);
        }

        public static void CheckEqualRowKeys(this Matrix<string, string, double> x, List<string> rowKeys)
        {
            Helper.CheckCondition(SpecialFunctions.SetEquals<string>(rowKeys, x.RowKeys.ToList()), "colKey sets are not the same");
        }

        public static void CheckEqualColKeys(this Matrix<string, string, double> x, List<string> colKeys)
        {
            Helper.CheckCondition(SpecialFunctions.SetEquals<string>(colKeys, x.ColKeys.ToList()), "colKey sets are not the same");
        }

        public static void CheckEqualOrderColKeys(this Matrix<string, string, double> x, List<string> colKeys)
        {
            Helper.CheckCondition(colKeys.SequenceEqual<string>(x.ColKeys.ToList<string>()), "colKey sets are not the same");
        }


        /// <summary>
        /// reorder the keys (and corresponding data to match the order specified)
        ///throw an exception if that key  is not found
        /// </summary>
        ///<param name="colKeys">order to use for col keys</param>
        public static Matrix<string, string, double> CheckEqualAndReorderColKeys(this Matrix<string, string, double> x, List<string> colKeys)
        {
            CheckEqualColKeys(x, colKeys);
            return x.SelectColsView(colKeys);//.AsShoMatrix();
        }

        public static Matrix<TRowKey, TColKey, TValue> CheckEqualAndReorderColAndRowKeys<TRowKey, TColKey, TValue>(this Matrix<TRowKey, TColKey, TValue> x, List<TRowKey> rowKeys, List<TColKey> colKeys)
        {
            Helper.CheckCondition(SpecialFunctions.SetEquals<TRowKey>(rowKeys, x.RowKeys.ToList<TRowKey>()), "rowKey sets are not the same");
            Helper.CheckCondition(SpecialFunctions.SetEquals<TColKey>(colKeys, x.ColKeys.ToList<TColKey>()), "colKey sets are not the same");

            return x.SelectRowsAndColsView(rowKeys, colKeys);//.AsShoMatrix();
        }
        public static void CheckEqualColAndRowKeys<TRowKey, TColKey, TValue>(this Matrix<TRowKey, TColKey, TValue> x, List<TRowKey> rowKeys, List<TColKey> colKeys)
        {
            Helper.CheckCondition(SpecialFunctions.SetEquals<TRowKey>(rowKeys, x.RowKeys.ToList<TRowKey>()), "rowKey sets are not the same");
            Helper.CheckCondition(SpecialFunctions.SetEquals<TColKey>(colKeys, x.ColKeys.ToList<TColKey>()), "colKey sets are not the same");
        }
        public static void CheckEqualOrderColAndRowKeys<TRowKey, TColKey, TValue>(this Matrix<TRowKey, TColKey, TValue> x, List<TRowKey> rowKeys, List<TColKey> colKeys)
        {
            Helper.CheckCondition(rowKeys.SequenceEqual(x.RowKeys.ToList<TRowKey>()), "rowKey sets are not the same");
            Helper.CheckCondition(colKeys.SequenceEqual(x.ColKeys.ToList<TColKey>()), "colKey sets are not the same");
        }

        public static bool IsSymmetric(this Matrix<string, string, double> x, double tol)
        {
            if (!x.IsSquare()) return false;
            for (int i = 0; i < x.RowCount; ++i)
            {
                for (int j = i + 1; j < x.ColCount; ++j)
                {
                    double diff = Math.Abs(x[i, j] - x[j, i]);             // Check that arr[i,j] is approximattely equal to  arr[j,i]
                    if (diff > tol) return false;
                }
            }
            return true;
        }

        public static bool IsSymmetric(this Matrix<string, string, double> x)
        {
            double tol = 1e-7;
            return IsSymmetric(x, tol);
        }

        /// <summary>
        /// Remove any columns that contain only unique values. 
        /// </summary>
        public static Matrix<string, string, double> GetRowsThatVary(this Matrix<string, string, double> x)
        {
            List<int> goodInd = x.FindRowsThatVary();
            return x.SelectRowsView(goodInd);//this.SelectRowsView(goodInd).AsShoMatrix();
        }

        /// <summary>
        /// Add a bunch of 1's in the last row, and call this row key "bias"--useful for regression
        /// </summary>
        /// <returns></returns>
        public static Matrix<string, string, double> AddBiasRow(this Matrix<string, string, double> x)
        {
            DoubleArray biasArray = ShoUtils.DoubleArrayOnes(1, x.ColCount);//new DoubleArray(1,x.ColCount);
            ShoMatrix biasMatrix = new ShoMatrix(biasArray, new List<string> { "bias" }, x.ColKeys, x.MissingValue);
            return x.MergeRowsView<string, string, double>(true, biasMatrix);

            //DoubleArray newD = ShoUtils.AddBiasToInputData(this.DoubleArray.T, this.ColCount).T;
            //List<string> newRowKeys = this.RowKeys.ToList();
            //newRowKeys.Add("bias");
            //ShoMatrix x = new ShoMatrix(newD, newRowKeys, this.ColKeys, this.MissingValue);
            //return x;
        }


    }
}
