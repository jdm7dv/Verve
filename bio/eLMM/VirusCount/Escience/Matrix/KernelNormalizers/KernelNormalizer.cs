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
using Bio.Matrix;

namespace MBT.Escience.Matrix.KernelNormalizers
{
    abstract public class KernelNormalizer
    {
        abstract public void NormalizeInPlace(ref ShoMatrix shoMatrix);
        public void NormalizeInPlace(ref Matrix<string,string,double> matrix)
        {
            matrix = matrix.AsShoMatrix();
            NormalizeInPlace(ref matrix);
        }
    }
}
