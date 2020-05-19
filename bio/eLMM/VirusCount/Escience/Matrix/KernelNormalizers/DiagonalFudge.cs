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

namespace MBT.Escience.Matrix.KernelNormalizers
{
    public class DiagonalFudge : KernelNormalizer, IParsable
    {
        public DiagonalFudge ()
        {
        }

        public DiagonalFudge(double fudge)
        {
            Fudge = fudge;
        }


        public double Fudge = 0.0;

        public override void NormalizeInPlace(ref ShoMatrix shoMatrix)
        {
            if (Fudge == 0)
            {
                return;
            }

            DoubleArray doubleArray = shoMatrix.DoubleArray;
            Helper.CheckCondition(doubleArray.size0 == doubleArray.size1, "Expect #rows == #cols");
            for (int i = 0; i < doubleArray.size0; ++i)
            {
                doubleArray[i,i] += Fudge;
            }
        }

        #region IParsable Members

        public void FinalizeParse()
        {
            Helper.CheckCondition(Fudge >= 0, "Fudge must be at least 0");
        }

        #endregion
    }
}
