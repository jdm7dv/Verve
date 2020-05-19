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
using Bio.Matrix;

namespace MBT.Escience.Matrix.KernelNormalizers
{
    public class WriteKernel : KernelNormalizer
    {
        public string OutputFileName;

        public override void NormalizeInPlace(ref ShoMatrix shoMatrix)
        {
            Bio.Util.FileUtils.CreateDirectoryForFileIfNeeded(OutputFileName);
            shoMatrix.WriteDense(OutputFileName);
        }
    }
}
