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
    /// remove variables that don’t have at least Count minor values.
    /// </summary>
    public class RemoveRows : RowFilter
    {
        [Parse(ParseAction.Required, typeof(FileList<string>))]
        IList<string> RowToRemoveList;

        /// <summary>
        /// If true, raise an error if any of the rowkeys given are not found in the input matrix.
        /// </summary>
        [Parse(ParseAction.Optional)]
        bool MatchRequired = true;

        //!!!similar code elsewhere and  and missing ISGOOD

        public override Matrix<string, string, T> Filter<T>(Matrix<string, string, T> input, Matrix<string, string, T> target)
        {
            Helper.CheckCondition(!MatchRequired || RowToRemoveList.ToHashSet().IsSubsetOf(input.RowKeys), "Expect all items in RowToRemoveList to be in the matrix.");
            var output = input.SelectRowsView(input.RowKeys.Except(RowToRemoveList));
            return output;

        }

        public override bool IsGood<T>(Matrix<string, string, T> rowMatrix, Matrix<string, string, T> target)
        {
            throw new NotImplementedException();
        }

    }
}
