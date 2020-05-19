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

namespace MBT.Escience
{
    static public class RangeCollectionExtensions
    {

        //!!!could the begin and last be removed and just use long.MinValue, etc????
        public static RangeCollection Except(this RangeCollection rangeCollectionMain, RangeCollection rangeCollectionToSubtract, long fullRangeBegin, long fullRangeLast) 
        {
            if (rangeCollectionToSubtract.IsEmpty)
            {
                return rangeCollectionMain;
            }
            RangeCollection rangeCollection = rangeCollectionMain.Complement(fullRangeBegin, fullRangeLast);
            rangeCollection.AddRangeCollection(rangeCollectionToSubtract);
            RangeCollection result = rangeCollection.Complement(fullRangeBegin, fullRangeLast);
            return result;
        }

        public static RangeCollection Union(this RangeCollection rangeCollection1, RangeCollection rangeCollection2)
        {
            RangeCollection result = new RangeCollection(rangeCollection1);
            result.AddRangeCollection(rangeCollection2);
            return result;
        }

    }
}
