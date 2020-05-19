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
    public class TwoByOne
    {
        private TwoByOne()
        {
        }

        public static TwoByOne GetInstance()
        {
            TwoByOne twoByOne = new TwoByOne();
            twoByOne.Total = 0;
            twoByOne.TrueCount = 0;
            return twoByOne;
        }

        public static TwoByOne GetInstance<T>(IEnumerable<T> tEnum, Predicate<T> predicate)
        {
            TwoByOne twoByOne = new TwoByOne();
            twoByOne.Total = 0;
            twoByOne.TrueCount = 0;
            foreach (T t in tEnum)
            {
                twoByOne.Add(predicate(t));
            }
            return twoByOne;
        }

        public void Add(bool? testOrNull)
        {
            if (testOrNull.HasValue)
            {
                Add(testOrNull.Value);
            }
        }

        public void Add(bool test)
        {
            if (test)
            {
                ++TrueCount;
            }
            ++Total;
        }

        public int Total { get; private set; }
        public int TrueCount { get; private set; }
        public int FalseCount
        {
            get
            {
                return Total - TrueCount;
            }
        }

        public int FalseOrTrueCount(bool b)
        {
            if (b)
            {
                return TrueCount;
            }
            else
            {
                return FalseCount;
            }
        }

            
        public double Freq
        {
            get
            {
                return (double)TrueCount / (double)Total;
            }
        }

        public double CompFreq
        {
            get
            {
                return 1.0 - Freq;
            }
        }
    }
}
