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
    /// does nothing (quickly)
    /// </summary>
    public class Identity : RowFilter
    {
        public override Matrix<string, string, T> Filter<T>(Matrix<string, string, T> predictorIn, Matrix<string, string, T> target)
        {
            return predictorIn;
        }

        public override bool IsGood<T>(Matrix<string, string, T> rowMatrix, Matrix<string, string, T> target)
        {
            return true;
        }
    }
}
