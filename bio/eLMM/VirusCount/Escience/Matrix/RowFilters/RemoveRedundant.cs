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
using Bio.Matrix;
using Bio.Util;
using System.Threading.Tasks;
using MBT.Escience; using MBT.Escience.Parse;

namespace EScienceLearn.RowFilters
{
    [Serializable]
    /// <summary>
    /// Removes variables that have constant values or that are the same as an earlier row.
    /// </summary>
    //[Parsable]
    public class RemoveRedundant : RowFilter
    {

        public override Matrix<string, string, T> Filter<T>(Matrix<string, string, T> predictorIn, Matrix<string, string, T> target)
        {
            IEnumerable<string> goodFeatures = FindNonRedunantFeatures(predictorIn);
            var predictor = predictorIn.SelectRowsView(goodFeatures);
            return predictor;

        }

        //!!!Similar code elsewhere
        //There is likely similar code elsewhere
        private IEnumerable<string> FindNonRedunantFeatures<T>(Matrix<string, string, T> matrixIn)
        {
            var goodRowList =
                from rowKey in matrixIn.RowKeys
                  .AsParallel().WithParallelOptionsScope()
                let row = matrixIn.RowView(rowKey)
                let distinctValueList = row.Values.Distinct().ToList()
                where distinctValueList.Count > 1
                select rowKey;

            var matrix2 = matrixIn.SelectRowsView(goodRowList);

            //Creates a list of the alphabetically later duplicate row keys
            var duplicateRowKey =
                (
                from rowKey1 in matrix2.RowKeys
                  .AsParallel().WithParallelOptionsScope()
                let row1 = matrix2.RowView(rowKey1)
                from rowKey2 in matrix2.RowKeys
                where rowKey2.CompareTo(rowKey1) == 1
                let row2 = matrix2.RowView(rowKey2)
                where SpecialFunctions.DictionaryEqual(row1, row2)
                select rowKey2
                ).ToHashSet();

            return matrix2.RowKeys.Except(duplicateRowKey);
        }

        public override bool IsGood<T>(Matrix<string, string, T> rowMatrix, Matrix<string, string, T> target)
        {
            throw new NotImplementedException("by design, not implemented");
        }
    }
}
