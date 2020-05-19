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

namespace MBT.Escience.Matrix.RowNormalizers
{
    [Serializable]
    /// <summary>
    /// Checks that no NaN's, but does nothing else.
    /// </summary>
    public class CheckNoNaN : RowNormalizer
    {
        public override Matrix<string, string, double> Normalize(Matrix<string, string, double> input)
        {
            Helper.CheckCondition(double.IsNaN(input.MissingValue) && !input.IsMissingSome(), "Expect matrix to use NaN for missing and to not have any NaN's");
            return input;
        }

        public override LinearTransform CreateLinearTransform(IList<double> list, string rowKeyOrNull)
        {
            Helper.CheckCondition(list.All(value => !double.IsNaN(value)), "List should not have any NaN's");
            return new LinearTransform(1, 0, double.NaN);
        }

    }
}
