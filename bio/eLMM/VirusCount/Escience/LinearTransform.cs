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
using MBT.Escience.Parse;
#if !SILVERLIGHT
using ShoNS.Array;
#endif

namespace MBT.Escience
{
    [Serializable]
    /// <summary>
    /// Prepresents the function: Factor * x + Offset (however, if x is NaN, returns FillValue -- which might also be NaN)
    /// </summary>
    public class LinearTransform : IParsable
    {
        [Parse(ParseAction.Ignore)]
        private double _factor;
        [Parse(ParseAction.Required)]
        public double Factor
        {
            get
            {
                return _factor;
            }
            set
            {
                Helper.CheckCondition(value != 0, "The factor of a linearTransform cannot be zero");
                _factor = value;
            }
        }
        [Parse(ParseAction.Required)]
        public double Offset;


        [Parse(ParseAction.Required)]
        public double FillValue;


        public LinearTransform()
        {
        }


        public LinearTransform(double factor, double offset, double fillValue)
        {
            Factor = factor;
            Offset = offset;
            FillValue = fillValue;
        }

        public double Transform(double x)
        {
            if (double.IsNaN(x))
            {
                return FillValue;
            }
            else
            {
                double y = Factor * x + Offset;
                return y;
            }
        }

        [Parse(ParseAction.Ignore)]
        public LinearTransform Inverse
        {
            get
            {
                var inverse = new LinearTransform(1.0 / Factor, -Offset / Factor, double.NaN);
                inverse.FillValue = inverse.Transform(FillValue);
                return inverse;
            }
        }



        [Parse(ParseAction.Ignore)]
        public bool IsIdentity
        {
            get
            {
                return Factor == 1.0 && Offset == 0 && double.IsNaN(FillValue);
            }
        }

        public override string ToString()
        {
            string name = ConstructorArguments.FromParsable(this).ToString();
            return name;
        }

        public static LinearTransform Parse(string val)
        {
            LinearTransform linearTransform = ConstructorArguments.Construct<LinearTransform>(val);
            return linearTransform;
        }

        //Shooptimization - might be able to do this faster using sho ops when it is a 1-D double array
        public void TransformInPlace(ref IList<double> unnormalizedList)
        {
            for (int index = 0; index < unnormalizedList.Count; ++index)
            {
                unnormalizedList[index] = Transform(unnormalizedList[index]);
            }
        }


        #region IParsable Members

        public void FinalizeParse()
        {
            Helper.CheckCondition(!double.IsNaN(Factor) && !double.IsNaN(Offset), "Neither Factor nor Offset may be NaN");
        }

        #endregion
    }
}
