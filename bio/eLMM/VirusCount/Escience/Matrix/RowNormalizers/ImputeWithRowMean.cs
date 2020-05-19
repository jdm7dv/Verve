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
using ShoNS.Array;
using MBT.Escience.Sho;
using System.Threading.Tasks;
using Bio.Util;
using MBT.Escience;

namespace MBT.Escience.Matrix.RowNormalizers
{
    public class ImputeWithRowMean : RowNormalizer
    {

        public override LinearTransform CreateLinearTransform(IList<double> rowArray, string rowKeyOrNull)
        {
            double rowMean = SpecialFunctions.MeanWithNaN(rowArray);
            return new LinearTransform(1, 0, rowMean);
        }
    }
}
