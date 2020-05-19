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
    /// does nothing (quickly)
    /// </summary>
    public class Identity : RowNormalizer
    {
        public override Matrix<string, string, double> Normalize(Matrix<string, string, double> predictorIn)
        {
            return predictorIn;
        }

        public override LinearTransform CreateLinearTransform(IList<double> rowArray, string rowKeyOrNull)
        {
            return new LinearTransform(1, 0, double.NaN);
        }

    }
}
