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

using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using ShoNS.Array;

namespace MBT.Escience.Sho
{
    public class VectorArrayComparer : IEqualityComparer<DoubleArray>
    {

        /// <summary>
        /// assumes that these are both vector arrays of the same length. no checking to keep it fast
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(DoubleArray x, DoubleArray y)
        {
            for (int j = 0; j < x.Length; j++)
            {
                if (x[j] != y[j]) return false;
            }
            return true;
        }

        public int GetHashCode(DoubleArray x)
        {
            int myHashCode = x[0].GetHashCode();
            for (int j = 1; j < x.Length; j++)
            {
                //xor the HashCodes together for all the doubles
                myHashCode = myHashCode ^ x[j].GetHashCode();
            }
            return myHashCode;
        }
    }
}
