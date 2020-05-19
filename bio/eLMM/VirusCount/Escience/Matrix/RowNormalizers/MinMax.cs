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
using MBT.Escience;
using MBT.Escience.Parse;
using MBT.Escience.Matrix;

namespace MBT.Escience.Matrix.RowNormalizers
{
    [Serializable]
    /// <summary>
    /// Makes the range between the largest and the smallest value be one. 
    /// If zeroCentered is true, the mean will be zero
    /// Otherwise and ll values are positive, map from 0 to 1; otherwise from -1/2 to 1/2
    /// </summary>
    public class MinMax : RowNormalizer
    {
        [Parse(ParseAction.Optional)]
        public double FillValue = double.NaN;

        /// <summary>
        /// Make the mean zero
        /// </summary>
        [Parse(ParseAction.Required)]
        public bool ZeroCentered = false;

        public override LinearTransform CreateLinearTransform(IList<double> rowArrayWithNaN, string rowKeyOrNull)
        {

            Tuple<double, double> minAndMax = SpecialFunctions.MinAndMaxWithNaN(rowArrayWithNaN);
            double diff = minAndMax.Item2 - minAndMax.Item1;
            LinearTransform linearTransform;
            if (diff == 0) //If all items are the same, use the offset to make them all zero
            {
                linearTransform = new LinearTransform(1.0, -minAndMax.Item1, FillValue);
            }
            else if (ZeroCentered) // make the range between largest and smallest be 1 and make the mean be 0
            {
                double unnormalizedMean = SpecialFunctions.MeanWithNaN(rowArrayWithNaN); ;
                linearTransform = new LinearTransform(1.0 / diff, -unnormalizedMean / diff, FillValue);

            }
            else if (minAndMax.Item1 >= 0) //If all >= 0, make them 0 to 1
            {
                linearTransform = new LinearTransform(1.0 / diff, -minAndMax.Item1 / diff, FillValue);
            }
            else // If some negative, make -1/2 to 1/2
            {
                linearTransform = new LinearTransform(1.0 / diff, -minAndMax.Item1 / diff - .5, FillValue);
            }

            return linearTransform;
        }

    }
}
