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
using MBT.Escience.Sho;
using ShoNS.Array;
using Bio.Util;
using MBT.Escience.Parse;
using System.Threading.Tasks;

namespace MBT.Escience.Matrix.KernelNormalizers
{
    /// <summary>
    /// a way to normalize kernels for GPR (also described here: http://www.cbrc.jp/~tsuda/pdf/kmcb_primer.pdf)
    /// </summary>
    public class Special : KernelNormalizer
    {

        public override void NormalizeInPlace(ref ShoMatrix shoMatrix)
        {
            DoubleArray x = shoMatrix.DoubleArray;

            var invSqrtDiagonal = x.Diagonal.Select(v => 1.0 / Math.Sqrt(v)).ToArray();


            Parallel.For(0, x.size0, ParallelOptionsScope.Current, r =>
            {
                for (int c = 0; c < x.size1; c++)
                {
                    x[r, c] *= invSqrtDiagonal[r] * invSqrtDiagonal[c];
                }
            });
        }

    }
}
