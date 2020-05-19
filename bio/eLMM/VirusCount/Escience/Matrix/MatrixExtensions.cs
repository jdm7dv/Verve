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

using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;
using System.Threading;
using Bio.Util;
using Bio.Matrix;
using System.Threading.Tasks;
using System.Linq;
using System;


namespace MBT.Escience.Matrix
{
    /// <summary>
    /// Provides a set of static methods for Matrix objects.
    /// </summary>
    public static class MatrixExtensions
    {
        public static bool IsEmpty<TRow, TCol, TVal>(this Matrix<TRow, TCol, TVal> x)
        {
            return x == null || x.RowCount == 0 || x.ColCount == 0;
        }

        public static Matrix<string, string, TVal> CheckAndPossiblyFixKeysForTrailingSpaces<TVal>(this Matrix<string, string, TVal> x, bool fixProblems)
        {
            //because pre-MBF version used to trim whitepace from the ends of row/col keys, add this in otherwise get mismatches on old data!!!
            List<string> rowKeysWithSpaces = x.RowKeys.ToList().GetElementsWithTrailingWhiteSpace();
            List<string> colKeysWithSpaces = x.ColKeys.ToList().GetElementsWithTrailingWhiteSpace();

            if (rowKeysWithSpaces.Count > 0)
            {
                string msg = "trailing white spaces in row keys: " + rowKeysWithSpaces.StringJoin(", ");
                if (!fixProblems)
                {
                    throw new Exception(msg);
                }
                else
                {
                    Console.WriteLine(msg);
                    Dictionary<string, string> rowMapping = SpecialFunctions.EnumerateTwo<string, string>(x.RowKeys.ToList(), x.RowKeys.ToList()).ToDictionary();
                    foreach (string elt in rowKeysWithSpaces)
                    {
                        rowMapping[elt.TrimEnd(' ')] = elt;
                    }
                    x = x.RenameRowsView(rowMapping);
                    Console.WriteLine("fixed the problem by removing trailing white spaces");
                }
            }
            if (colKeysWithSpaces.Count > 0)
            {
                string msg = "trailing white spaces in col keys: " + colKeysWithSpaces.StringJoin(",");
                if (!fixProblems)
                {
                    throw new Exception("trailing white spaces in col keys: " + colKeysWithSpaces.StringJoin(","));
                }
                else
                {
                    Console.WriteLine(msg);
                    Dictionary<string, string> colMapping = SpecialFunctions.EnumerateTwo<string, string>(x.ColKeys.ToList(), x.ColKeys.ToList()).ToDictionary();
                    foreach (string elt in colKeysWithSpaces)
                    {
                        colMapping[elt.TrimEnd(' ')] = elt;
                    }
                    x = x.RenameColsView(colMapping);
                    Console.WriteLine("fixed the problem by removing trailing white spaces");
                }
            }
            return x;
        }

        static public Matrix<TRowKey, TColKey, TValue2> MergeRowsAndColsView<TRowKey, TColKey, TValue2>(Matrix<TRowKey, TColKey, TValue2>[,] matrixPieces2D)
        {
            var mergeMatrixList = new List<Matrix<TRowKey, TColKey, TValue2>>();
            for (int rowIndex = 0; rowIndex < matrixPieces2D.GetLength(0); ++rowIndex)
            {
                var row = Enumerable.Range(0, matrixPieces2D.GetLength(1)).Select(colIndex => matrixPieces2D[rowIndex, colIndex]).ToArray();
                if (row.Length == 1)
                {
                    mergeMatrixList.Add(row[0]);
                }
                else
                {
                    mergeMatrixList.Add(new MergeColsView<TRowKey, TColKey, TValue2>(/*mustMatch*/ true, row));
                }
            }
            if (mergeMatrixList.Count == 1)
            {
                return mergeMatrixList[0];
            }

            var output = new MergeRowsView<TRowKey, TColKey, TValue2>(/*mustMatch*/ true, mergeMatrixList.ToArray());
            return output;
        }

        static public Matrix<TRowKey, TColKey, TValue2> MergeRowsAndColsView<TRowKey, TColKey, TValue2>(bool mustMatch, IList<Matrix<TRowKey, TColKey, TValue2>> matrices)
        {
            var inNeed = matrices.ToHashSet();
            var mergeMatrixList = new List<Matrix<TRowKey, TColKey, TValue2>>();
            while (inNeed.Count > 0)
            {
                var first = inNeed.First();
                var colKeySet = first.ColKeys.ToHashSet();
                var nextSet = first.AsSingletonEnumerable().ToHashSet();
                foreach (var matrix in inNeed)
                {
                    if (matrix == first || colKeySet.IntersectAny(matrix.ColKeys))
                    {
                        nextSet.Add(matrix);
                        colKeySet.AddNewOrOldRange(matrix.ColKeys);
                    }
                }
                inNeed.RemoveAll(nextSet);
                if (nextSet.Count == 1)
                {
                    mergeMatrixList.Add(nextSet.First());
                }
                else
                {
                    mergeMatrixList.Add(new MergeRowsView<TRowKey, TColKey, TValue2>(mustMatch, nextSet.ToArray()));
                }
            }


            if (mergeMatrixList.Count == 1)
            {
                return mergeMatrixList[0];
            }

            var output = new MergeColsView<TRowKey, TColKey, TValue2>(mustMatch, mergeMatrixList.ToArray());
            return output;
        }


        public static Matrix<TRowKey, TColKey, TValue> UnLazy<TRowKey, TColKey, TValue>(this Matrix<TRowKey, TColKey, TValue> inputMatrix)
        {
            FileMatrix<TValue> fileMatrix = inputMatrix as FileMatrix<TValue>;
            if (null != fileMatrix)
            {
                return (Matrix<TRowKey, TColKey, TValue>)(object)fileMatrix.Parent; //Force the conversion because we know it must work
            }
            else
            {
                return inputMatrix;
            }
        }

        public static void WriteRowKeysAnsi<T>(this Matrix<string, string, T> matrix, IList<string> filenameList, ParallelOptions parallelOptions)
        {
            Helper.CheckCondition(filenameList.Count == 2, "Expect two files names, one for the denseAnsi file and one for the RowKeysAnsi file");
            RowKeysAnsi rowKeysAnsi = RowKeysAnsi.GetInstanceFromDenseAnsi(filenameList[0], ParallelOptionsScope.Current, FileAccess.ReadWrite, FileShare.ReadWrite);
            rowKeysAnsi.WriteRowKeys(filenameList[1]);
        }

        public static bool TryGetDenseAnsiInstance(string filename, SufficientStatistics missingValue, ParallelOptions parallelOptions, out Matrix<string, string, SufficientStatistics> matrix)
        {
            matrix = null;
            Matrix<string, string, char> sscMatrix;
            if (!DenseAnsi.TryGetInstance(filename, '?', parallelOptions, out sscMatrix))
                return false;
            matrix = sscMatrix.ConvertValueView(MBT.Escience.ValueConverter.CharToSufficientStatistics, missingValue);
            //matrix = sscMatrix.ConvertValueView(new ValueConverter<char, SufficientStatistics>(c => DiscreteStatistics.GetInstance((int)c), ss => (char)ss.AsDiscreteStatistics()), missingValue);

            return true;
        }

        /// <summary>
        /// Same as TryGetValue, but will return false if the rowKey or colKey is not a valid key, rather than throwing an exception.
        /// </summary>
        public static bool TryGetValueNoExceptions<TRowKey, TColKey, TVal>(this Matrix<TRowKey, TColKey, TVal> m, TRowKey rowKey, TColKey colKey, out TVal value)
        {
            value = m.MissingValue;
            return m.ContainsRowAndColKeys(rowKey, colKey) && m.TryGetValue(rowKey, colKey, out value);
        }
        static public List<Matrix<TRowKey, TColKey, TValue>> IntersectOnColKeys<TRowKey, TColKey, TValue>(ParallelOptions parallelOption, params Matrix<TRowKey, TColKey, TValue>[] matrixArray)
        {
            List<TColKey> colKeyList =
                (from matrix in matrixArray
                    .AsParallel().AsOrdered().WithDegreeOfParallelism(parallelOption.MaxDegreeOfParallelism)
                 select (IEnumerable<TColKey>)matrix.ColKeys
                    ).Aggregate((c1, c2) => c1.Intersect(c2)).ToList();


            var matrixList =
                (from matrix in matrixArray
                    .AsParallel().AsOrdered().WithDegreeOfParallelism(parallelOption.MaxDegreeOfParallelism)
                 select matrix.UnLazy().SelectColsView(colKeyList)
                 ).ToList();

            return matrixList;
        }

        /// <summary>

        /// <summary>
        /// Same as TryGetValue, but will return false if the rowIdx or colIdx is out of range, rather than throwing an exception.
        /// </summary>
        public static bool TryGetValueNoExceptions<TRowKey, TColKey, TVal>(this Matrix<TRowKey, TColKey, TVal> m, int rowIdx, int colIdx, out TVal value)
        {
            value = m.MissingValue;
            return rowIdx >= 0 && colIdx >= 0 && rowIdx < m.RowCount && colIdx < m.ColCount && m.TryGetValue(rowIdx, colIdx, out value);
        }

        /// <summary>
        /// Loads a single mergeView matrix from the list of files.
        /// </summary>
        /// <typeparam name="TRowKey"></typeparam>
        /// <typeparam name="TColKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="files"></param>
        /// <param name="missingVal"></param>
        /// <param name="colsMustMatch"></param>
        /// <returns></returns>
        public static Matrix<TRowKey, TColKey, TVal> LoadAsSingleMatrix<TRowKey, TColKey, TVal>(this IEnumerable<FileInfo> files, TVal missingVal, bool colsMustMatch = false)
        {
            MatrixFactory<TRowKey, TColKey, TVal> mf = MatrixFactory<TRowKey, TColKey, TVal>.GetInstance();
            //var matrices = files.Select(dataFile => mf.Parse(dataFile.FullName, missingVal, ParallelOptionsScope.Current)).ToArray();
            var matrices = (from f in files
                            let name = f.Name == "-" ? MBT.Escience.FileUtils.WriteStdInToTempFile() : f.FullName
                            select mf.Parse(name, missingVal, ParallelOptionsScope.Current)).ToArray();

            var result = new MergeRowsView<TRowKey, TColKey, TVal>(colsMustMatch, matrices);
            return result;
        }

        /// <summary>
        /// Enumerates tuples of (rowKey,colKey) where the corresponding value is missing.
        /// </summary>
        public static IEnumerable<Tuple<TRowKey, TColKey>> GetMissingKeys<TRowKey, TColKey, TVal>(this Matrix<TRowKey, TColKey, TVal> m)
        {
            for (int i = 0; i < m.RowCount; i++)
                for (int j = 0; j < m.RowCount; j++)
                    if (m.IsMissing(i, j))
                        yield return new Tuple<TRowKey, TColKey>(m.RowKeys[i], m.ColKeys[j]);
        }

        /// <summary>
        /// # of missing entries
        /// </summary>
        public static int GetNumMissingEntries<TRowKey, TColKey, TVal>(this Matrix<TRowKey, TColKey, TVal> m)
        {
            return m.GetMissingIndexes().Count();
        }


        /// <summary>
        /// Enumerates tuples of (rowIndex,colIndex) where the corresponding value is missing.
        /// </summary>
        public static IEnumerable<Tuple<int, int>> GetMissingIndexes<TRowKey, TColKey, TVal>(this Matrix<TRowKey, TColKey, TVal> m)
        {
            for (int i = 0; i < m.RowCount; i++)
                for (int j = 0; j < m.ColCount; j++)
                    if (m.IsMissing(i, j))
                        yield return new Tuple<int, int>(i, j);
        }

        static public Matrix<TRowKey, TColKey, double> LinearTransformRowsView<TRowKey, TColKey>(this Matrix<TRowKey, TColKey, double> parentMatrix, IList<LinearTransform> linearTransformList)
        {
            Helper.CheckCondition(linearTransformList.Count == parentMatrix.RowCount, "Expect one item in linearTransformList for each row of the parent matrix");

            //If the new won't change anything, just return the parent
            if (linearTransformList.All(linearTransform => linearTransform.IsIdentity))
            {
                return parentMatrix;
            }

            //!!!Could check of this is a SelectRowsAndColsView of a SelectRowsAndColsView and simplify (see TransposeView for an example)

            var matrixView = new LinearTransformRowsView<TRowKey, TColKey>();
            matrixView.SetUp(parentMatrix, linearTransformList);
            return matrixView;
        }


        static public Matrix<TRowKey, TColKey, double> LinearTransformRowsView<TRowKey, TColKey>(this Matrix<TRowKey, TColKey, double> parentMatrix, Matrix<TRowKey, string, LinearTransform> linearTransformMatrix)
        {
            Helper.CheckCondition(parentMatrix.RowKeys.SequenceEqual(linearTransformMatrix.RowKeys), "Expect the parentMatrix and the linearTransformMatrix to have the same rowKeys in the same order.");

            //If the new won't change anything, just return the parent
            if (linearTransformMatrix.Values.All(linearTransform => linearTransform.IsIdentity))
            {
                return parentMatrix;
            }

            //!!!Could check of this is a SelectRowsAndColsView of a SelectRowsAndColsView and simplify (see TransposeView for an example)

            var matrixView = new LinearTransformRowsView<TRowKey, TColKey>();
            matrixView.SetUp(parentMatrix, linearTransformMatrix);
            return matrixView;
        }


        static public Matrix<TRowKey, TColKey, TValueView> ConvertValueOrMissingView<TRowKey, TColKey, TValueParent, TValueView>(
            this Matrix<TRowKey, TColKey, TValueParent> parentMatrix, ValueConverter<TValueParent, TValueView> converter, TValueView missingValue)
        {
            // catch the case where we are converting back to the original matrix
            var asConvertValueView = parentMatrix as ConvertValueView<TRowKey, TColKey, TValueParent, TValueView>;
            if (asConvertValueView != null &&
                asConvertValueView.ViewValueToParentValue.Equals(converter.ConvertForward) &&
                asConvertValueView.ParentValueToViewValue.Equals(converter.ConvertBackward))
            {
                return asConvertValueView.ParentMatrix;
            }

            var matrixView = new ConvertValueAndMissingView<TRowKey, TColKey, TValueView, TValueParent>();
            matrixView.SetUp(parentMatrix, converter, missingValue);
            return matrixView;
        }

        /// <summary>
        /// Returns an enumeration of col keys that are not present in all of the matrices
        /// </summary>
        /// <param name="colKeyComparer">The comparer to use, or null to use the default.</param>
        /// <returns></returns>
        public static IEnumerable<TColKey> ColKeyMismatches<TRowKey, TColKey, TVal>(this IEnumerable<Matrix<TRowKey, TColKey, TVal>> matrices, IEqualityComparer<TColKey> colKeyComparer = null)
        {
            var matriciesAsList = matrices.ToList();

            var unionColKeys = matrices.SelectMany(m => m.ColKeys).ToHashSet(colKeyComparer);
            
            var sharedColKeys = matriciesAsList[0].ColKeys.ToHashSet(colKeyComparer);
            for (int i = 1; i < matriciesAsList.Count; i++)
                sharedColKeys.IntersectWith(matriciesAsList[i].ColKeys);

            var mismatchKeys = unionColKeys.Except(sharedColKeys).ToHashSet(colKeyComparer);

            return mismatchKeys;
        }

        /// <summary>
        /// Returns an enumeration of row keys that are not present in all of the matrices
        /// </summary>
        /// <param name="rowKeyComparer">The comparer to use, or null to use the default.</param>
        /// <returns></returns>
        public static IEnumerable<TRowKey> RowKeyMismatches<TRowKey, TColKey, TVal>(this IEnumerable<Matrix<TRowKey, TColKey, TVal>> matrices, IEqualityComparer<TRowKey> rowKeyComparer = null)
        {
            IEnumerable<Matrix<TColKey,TRowKey,TVal>> transposedMatrices = matrices.Select(m => m.TransposeView());
            var result = ColKeyMismatches(transposedMatrices, rowKeyComparer);

            return result;
        }

        /// <summary>
        /// Throws an exception if there are any case mismatches in the col keys of the matrices.
        /// </summary>
        /// <typeparam name="TRowKey"></typeparam>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="matrices"></param>
        public static void CheckForCaseMismatchesInColKeys<TRowKey, TVal>(this IEnumerable<Matrix<TRowKey, string, TVal>> matrices)
        {
            var mismatchKeys = matrices.ColKeyMismatches();
            var caseInsensitiveMismatchKeys = matrices.ColKeyMismatches(StringComparer.CurrentCultureIgnoreCase);
            var caseInsensitiveProblems = mismatchKeys.Except(caseInsensitiveMismatchKeys).ToList();
            if (caseInsensitiveProblems.Count > 0)
            {
                caseInsensitiveProblems.Sort();
                throw new InvalidDataException(string.Format("There are case mismatches in the data files (Matrices are case sensitive): ({0})", caseInsensitiveProblems.StringJoin(",")));
            }
        }

        public static void CheckForCaseMismatchesInColKeys<TRowKey, TVal>(this Matrix<TRowKey, string, TVal> m,  Matrix<TRowKey, string, TVal> matrices)
        {
            SpecialFunctions.EnumerateAll(m.AsSingletonEnumerable(), matrices.AsSingletonEnumerable()).CheckForCaseMismatchesInColKeys();
        }

    }
}
