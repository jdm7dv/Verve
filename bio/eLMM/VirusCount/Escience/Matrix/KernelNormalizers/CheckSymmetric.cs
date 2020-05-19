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
    public class CheckSymmetric : KernelNormalizer
    {
        public override void NormalizeInPlace(ref ShoMatrix shoMatrix)
        {
            DoubleArray x = shoMatrix.DoubleArray;
            Helper.CheckCondition(x.IsSymmetric(), "Expect matrix to be symmetric");
        }

    }
}
