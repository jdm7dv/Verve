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
using MBT.Escience.Matrix;
using Bio.Matrix;
using ShoNS.Array;

namespace MBT.Escience.Sho
{
    public static class DenseAnsiExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DenseAnsi"></param>
        /// <param name="orderedCidList">Columns are sorted according to this list</param>
        /// <param name="missingValue">Any character '?' is replaced with missingValue in the returned DoubleArray</param>
        /// <returns></returns>
        public static DoubleArray ToDoubleArray(this DenseAnsi DenseAnsi, IList<string> orderedCidList, double missingValue)
        {
            DoubleArray snpData = new DoubleArray(DenseAnsi.RowCount, orderedCidList.Count);

            //Carl would have used generic 'var' type:
            //foreach (var varAndValueList in DenseAnsi.VarToCidSerialNumberToVal)
            int snpIndex = -1;
            //foreach (KeyValuePair<string, List<byte>> varAndValueList in DenseAnsi.VarToCidSerialNumberToVal)
            foreach (string snpId in DenseAnsi.RowKeys)
            {
                //String snpId = varAndValueList.Key;
                ++snpIndex;//this is more efficient thatn snpIndex++ but here generates the same results
                int cidIndex = -1;
                foreach (string cid in orderedCidList)
                {
                    ++cidIndex;
                    char snpChar = DenseAnsi[snpId, cid];
                    if (snpChar == '?')
                    {
                        snpData[snpIndex, cidIndex] = missingValue;
                    }
                    else
                    {
                        //snpData[snpIndex, cidIndex] = int.Parse(snpChar.ToString());
                        snpData[snpIndex, cidIndex] = Double.Parse(snpChar.ToString());
                    }

                }
            }
            return snpData;
        }

        public static DoubleArray ToDoubleArray(this DenseAnsi DenseAnsi, int missingValue)
        {
            return DenseAnsi.ToDoubleArray(DenseAnsi.ColKeys, missingValue);
        }


    }
}
