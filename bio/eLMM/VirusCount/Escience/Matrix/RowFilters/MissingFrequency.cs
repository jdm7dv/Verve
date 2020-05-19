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
    /// remove variables that have too many missing values.
    /// </summary>
    public class MissingFrequency : RowFilter
    {
        [Parse(ParseAction.Required)]
        public double Threshold;


        public override bool IsGood<T>(Matrix<string, string, T> rowMatrix, Matrix<string, string, T> target)
        {
            Helper.CheckCondition(rowMatrix.RowCount == 1, "Expect rowMatrix to have one row");

            double missingFraction = 1.0 - (double) rowMatrix.Values.Count() / rowMatrix.ColCount;

            return (missingFraction <= Threshold);
        }

    }
}
