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

namespace MBT.Escience.Matrix.RowNormalizers
{
    public class MeanCenter : RowNormalizer
    {
        public override LinearTransform CreateLinearTransform(IList<double> rowArrayWithNaN, string rowKeyOrNull)
        {
            double mean = SpecialFunctions.MeanWithNaN(rowArrayWithNaN); ;
            var linearTransform = new LinearTransform(1.0, -mean, 0);
            return linearTransform;
        }
    }
}
