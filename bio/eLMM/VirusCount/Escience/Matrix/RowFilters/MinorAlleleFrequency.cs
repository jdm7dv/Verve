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
    /// remove variables that have too small a MAF
    /// </summary>
    public class MinorAlleleFrequency : RowFilter
    {
        [Parse(ParseAction.Required)]
        public double Threshold;


        public override bool IsGood<T>(Matrix<string, string, T> rowMatrixGeneric, Matrix<string, string, T> target)
        {
            Helper.CheckCondition(rowMatrixGeneric.RowCount == 1, "Expect rowMatrix to have one row");

            var rowMatrix = rowMatrixGeneric as Matrix<string,string,double>;

            Helper.CheckCondition(rowMatrix != null, "Expect values to be of type double");

            int[] countsPerValue = rowMatrix.Values.CountIntegers(3);

            int denom = 2 * (countsPerValue.Sum());
            Helper.CheckCondition(denom > 0, () => "Whole row is missing so can't compute MinorAlleleFrequency. " + rowMatrix.RowKeys[0]);
            int numer = countsPerValue[1] + 2 * countsPerValue[2];

            double maf = (double)numer / denom;

            return (maf >= Threshold);
        }

    }
}
