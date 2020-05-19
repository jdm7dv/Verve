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
using MBT.Escience;
using Bio.Util;
using MBT.Escience.Parse;

namespace MBT.Escience.Optimization
{
    abstract public class OneDOptimization
    {
        [Parse(ParseAction.Ignore)]
        public int DebugCount = -1;

        double _tol = 0.001;
        public virtual double Tolerance { get { return _tol; } set { _tol = value; } }

        public OneDOptimization() { }

        /// <summary>
        /// Runs maximization for f(x) and returns the best f(x). The corresponding argmax x is in the paramToOptimize.
        /// </summary>
        abstract public double Run(Converter<OptimizationParameter, double> oneDRealFunction, OptimizationParameter paramToOptimize);
        //abstract public double Run(Converter<OptimizationParameter, double> oneDRealFunction, OptimizationParameter paramToOptimize, int gridLineCount, out double bestInput);

        [Obsolete("Should switch to using automatic parsing.")]
        public static OneDOptimization GetInstance(string optimizerName)
        {

            //Console.WriteLine("Using {0} for 1D optimization.", optimizerName);
            if (optimizerName.Equals("default", StringComparison.CurrentCultureIgnoreCase))
            {
                optimizerName = "brentthengrid";
            }

            optimizerName = optimizerName.ToLower();
            if (optimizerName == "grid")
            {
                return new OneDGrid();
            }

            string brentwithnogrid = "brentwithnogrid";
            if (optimizerName.StartsWith(brentwithnogrid))
            {
                double tol = GetTolOrDefault(optimizerName, brentwithnogrid);
                return new Brent() { Tolerance = tol };
            }

            string brentthengrid = "brentthengrid";
            if (optimizerName.StartsWith(brentthengrid))
            {
                double tol = GetTolOrDefault(optimizerName, brentthengrid);
                return new BrentThenGrid() { Tolerance = tol };
            }

            Helper.CheckCondition(false, "The optimizer named {0} is not known", optimizerName);
            return null;

        }

        private static double GetTolOrDefault(string optimizerName, string brentwithnogrid)
        {
            string tolAsString = optimizerName.Substring(brentwithnogrid.Length);
            double tol = (tolAsString == "") ? .001 : double.Parse(tolAsString);
            return tol;
        }
    }
}
