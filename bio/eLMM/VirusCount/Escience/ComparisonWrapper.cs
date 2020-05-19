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

namespace MBT.Escience
{
    public class ComparisonWrapper<T> : IComparer<T>
    {
        public readonly Comparison<T> Comparison;

        public ComparisonWrapper(Comparison<T> comparison)
        {
            Comparison = comparison;
        }

        public int Compare(T x, T y)
        {
            return Comparison(x, y);
        }

    }
}
