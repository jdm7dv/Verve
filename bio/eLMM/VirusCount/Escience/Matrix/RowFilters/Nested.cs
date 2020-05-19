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
    /// applies each row filter in turn
    /// </summary>
    public class Nested : RowFilter
    {
        [Parse(ParseAction.Required)]
        List<EScienceLearn.RowFilters.RowFilter> RowFilterList;

        public override bool IsGood<T>(Matrix<string, string, T> rowMatrix, Matrix<string, string, T> target)
        {
            return RowFilterList.All(rowFilter => rowFilter.IsGood(rowMatrix, target));
        }
    }
}
