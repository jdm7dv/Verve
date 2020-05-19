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
using Bio.Util;
using ShoNS.Array;
using System.Threading.Tasks;
using MBT.Escience.Sho;
using MBT.Escience.Parse;
using MBT.Escience.Matrix.RowNormalizers;

namespace MBT.Escience.Matrix.SimilarityScores
{
    public class Covariance : SimilarityScore
    {


        protected override DoubleArray JustKernel(DoubleArray normalizedArray)
        {
            return StaticNormalizedToKernel(normalizedArray);
        }

        protected override DoubleArray JustKernel(DoubleArray normalizedArrayA, DoubleArray normalizedArrayB)
        {
            return StaticNormalizedToKernel(normalizedArrayA, normalizedArrayB);
        }

        public static DoubleArray StaticNormalizedToKernel(DoubleArray normalizedArray)
        {
            DoubleArray mu = normalizedArray.Mean(DimOp.EachCol);
            DoubleArray K = normalizedArray.MultiplyTranspose(true);
            K.MultiplyInto(-1.0, mu, true, mu, false, 1.0 / normalizedArray.size0);
            return K;
        }

        public static DoubleArray StaticNormalizedToKernel(DoubleArray normalizedArray1, DoubleArray normalizedArray2)
        {
            Helper.CheckCondition(normalizedArray1.size0 == normalizedArray2.size0, "Expect two arrays to have same # of rows");
            DoubleArray mu1 = normalizedArray1.Mean(DimOp.EachCol);
            DoubleArray mu2 = normalizedArray2.Mean(DimOp.EachCol);
            DoubleArray K = normalizedArray1.T.Multiply(normalizedArray2);
            K.MultiplyInto(-1.0, mu1, true, mu2, false, 1.0 / normalizedArray1.size0);
            return K;
        }



        protected override double JustKernel(IList<double> normalizedList1, IList<double> normalizedList2)
        {
            throw new NotImplementedException();
        }
    }

}
