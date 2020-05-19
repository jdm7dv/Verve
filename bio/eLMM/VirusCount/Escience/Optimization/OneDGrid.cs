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
using System.Diagnostics;
using MBT.Escience;
using Bio.Util;

namespace MBT.Escience.Optimization
{
    public class OneDGrid : OneDOptimization
    {
        public int GridLineCount = 5;

        public OneDGrid()
        {
        }

        override public double Run(Converter<OptimizationParameter, double> oneDRealFunction,
            OptimizationParameter param)
        {
            double low = param.LowForSearch;
            double high = param.HighForSearch;
            Converter<double, double> oneDRealFunctionInDoubleSpace = delegate(double d)
            {
                param.ValueForSearch = d;
                return oneDRealFunction(param);
            };

            double initParamValue = param.ValueForSearch;
            BestSoFar<double, double> bestFound = OneDOptimizationX(oneDRealFunctionInDoubleSpace, low, high, GridLineCount, Tolerance);
            bestFound.Compare(oneDRealFunctionInDoubleSpace(initParamValue), initParamValue); // make sure we didn't get any worse

            //bestInput = bestFound.Champ;
            param.ValueForSearch = bestFound.Champ;
            return bestFound.ChampsScore;
        }

        static public BestSoFar<double, double> OneDOptimizationX(Converter<double, double> oneDRealFunction,
                double low, double high, int gridLineCount, double tol)
        {


            double rangeIncrement = (high - low) / (double)gridLineCount;

            BestSoFar<double, double> bestParameterSoFar = BestSoFar<double, double>.GetInstance(SpecialFunctions.DoubleGreaterThan);

            //Debug.WriteLine(Helper.CreateTabString("parameter", "score"));
            //Debug.WriteLine(Helper.CreateTabString(low, high, rangeIncrement));
            for (int gridLine = 0; gridLine <= gridLineCount; ++gridLine)
            {
                double parameter = low + gridLine * rangeIncrement;
                //double parameter = SpecialFunctions.Probability(parameterLogOdds);
                double score = oneDRealFunction(parameter);
                //Debug.WriteLine(Helper.CreateTabString(parameter, score));
                bestParameterSoFar.Compare(score, parameter);
            }
            //Debug.WriteLine("");

            if (high - low < tol)
            {
                return bestParameterSoFar;
            }

            return OneDOptimizationX(oneDRealFunction, bestParameterSoFar.Champ - rangeIncrement, bestParameterSoFar.Champ + rangeIncrement, gridLineCount, tol);
        }

        private BestSoFar<double, double> OneDOptimizationX(double parameterInit, Converter<double, double> oneDRealFunction)
        {
            return OneDOptimizationInternal(oneDRealFunction, SpecialFunctions.LogOdds(.001), SpecialFunctions.LogOdds(.999), .001);
        }

        private BestSoFar<double, double> OneDOptimizationInternal(Converter<double, double> oneDRealFunction,
                double logOddsLow, double logOddsHigh, double logOddsPrecision)
        {
            int gridLineCount = 10;

            double logOddsRangeIncrement = (logOddsHigh - logOddsLow) / (double)gridLineCount;

            BestSoFar<double, double> bestParameterSoFar = BestSoFar<double, double>.GetInstance(SpecialFunctions.DoubleGreaterThan);

            Debug.WriteLine(Helper.CreateTabString("parameter", "parameterLogOdds", "score"));
            for (int gridLine = 0; gridLine <= gridLineCount; ++gridLine)
            {
                double parameterLogOdds = logOddsLow + gridLine * logOddsRangeIncrement;
                double parameter = SpecialFunctions.InverseLogOdds(parameterLogOdds);
                double score = oneDRealFunction(parameter);
                Debug.WriteLine(Helper.CreateTabString(parameter, parameterLogOdds, score));
                bestParameterSoFar.Compare(score, parameterLogOdds);
            }
            Debug.WriteLine("");

            if (logOddsHigh - logOddsLow < logOddsPrecision)
            {
                return bestParameterSoFar;
            }

            return OneDOptimizationInternal(oneDRealFunction, bestParameterSoFar.Champ - logOddsRangeIncrement, bestParameterSoFar.Champ + logOddsRangeIncrement, logOddsPrecision);
        }


    }
}
