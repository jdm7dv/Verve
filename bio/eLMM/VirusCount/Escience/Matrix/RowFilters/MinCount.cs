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
using Bio.Matrix;
using Bio.Util;
using System.Threading.Tasks;
using MBT.Escience; using MBT.Escience.Parse;

namespace EScienceLearn.RowFilters
{
    [Serializable]
    /// <summary>
    /// remove variables that don’t have at least Count minor values.
    /// </summary>
    public class MinCount : RowFilter
    {
        [Parse(ParseAction.Required)]
        public int Count;


        public override bool IsGood<T>(Matrix<string, string, T> rowMatrix, Matrix<string, string, T> target)
        {
            
            Helper.CheckCondition(rowMatrix.RowCount == 1, "Expect rowMatrix to have one row");

            //Call (one of) the most frequence value, the major value. Call all the other values, the minor values.
            //Each row must have at least, e.g. 3 minor values.
            //  For example,  1 2 3 4 - "1" can be the major value, "2 3 4" are the minor values. There are three of them so this row passes.
            //                0 0 0 0 0 0 1 1 1 - "0" is the major value, "1" is the minor value(s). There are three of them os this row passes.

            var valueAndCount = new Dictionary<T, int>();
            int totalCount = 0;
            int majorValueCount = 0;
            foreach (T value in rowMatrix.Values)
            {
                ++totalCount;
                int valueCount = 1 + valueAndCount.GetValueOrDefault(value, /*insertIfMissing*/ false);
                if (valueCount > majorValueCount)
                {
                    majorValueCount = valueCount;
                }
                int minorValuesCount = totalCount - majorValueCount;
                if (minorValuesCount >= Count)
                {
                    return true;
                }
                valueAndCount[value] = valueCount;
            }
            return false;
        }

    }
}
