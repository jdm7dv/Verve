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
using System.Diagnostics;
using MBT.Escience;
using Bio.Util;
using MBT.Escience.Parse;

namespace MBT.Escience.Optimization
{
    public class GridSearch : IOptimizer
    {
        /// <summary>
        /// How many times should we try to optimize each parameter?
        /// </summary>
        public int NIterOverParams = 10;

        /// <summary>
        /// The one dimensional optimization method used for each parameter.
        /// </summary>
        [Parse(ParseAction.Required)]
        public OneDOptimization OneDOptimization;

        public GridSearch() { }

        internal GridSearch(int numberOfIterationsOverParameters)
        {
            NIterOverParams = numberOfIterationsOverParameters;
        }

        public static GridSearch GetInstance(string optimizerName, int numberOfIterationsOverParameters, int gridLineCount)
        {
            GridSearch gridSearch = new GridSearch(numberOfIterationsOverParameters);
            gridSearch.OneDOptimization = OneDOptimization.GetInstance(optimizerName);
            return gridSearch;
        }

        public double Optimize(Converter<OptimizationParameterList, double> functionToOptimize, OptimizationParameterList paramList)
        {
            double eps = 1e-9;

            List<double> pointList = paramList.ExtractParameterValueListForSearch();
            List<double> lowList = paramList.ExtractParameterLowListForSearch();
            List<double> highList = paramList.ExtractParameterHighListForSearch();

            double oldScore = double.NaN;
            //double tempOldScore = functionToOptimize(paramList);


            //Console.WriteLine("Original score: {0}", tempOldScore);
            //Console.WriteLine(tempOldScore);
            Dictionary<int, double> paramNumberToInitValue = new Dictionary<int, double>();
            bool gridConverged = false;
            for (int iterationOverParameters = 0; iterationOverParameters < NIterOverParams; ++iterationOverParameters)
            {
                double newScore = double.NaN;
                int doSearchParameterCount = 0;
                foreach (OptimizationParameter paramToOptimize in paramList)
                {
                    //if (paramList.DoSearch(iParameter))
                    if (paramToOptimize.DoSearch)
                    {
                        ++doSearchParameterCount;

                        //double initValue = SpecialFunctions.GetValueOrDefault(paramNumberToInitValue, doSearchParameterCount, paramToOptimize.Value);
                        Debug.WriteLine("Search Param " + doSearchParameterCount.ToString());

                        //double bestInput;
                        newScore = OneDOptimization.Run(
                            delegate(OptimizationParameter param)
                            {
                                if (!double.IsNaN(param.Value) && paramList.SatisfiesConditions())
                                {
                                    return functionToOptimize(paramList);
                                }
                                else
                                {
                                    return double.NaN;
                                }
                            },
                            /*ref*/ paramToOptimize);
                        // /*ref*/ paramToOptimize, _gridLineCount, out bestInput);

                        Debug.WriteLine("bestInput " + paramToOptimize.ValueForSearch.ToString());

                        //paramToOptimize.ValueForSearch = bestInput;

                        //Debug.WriteLine("END ITER:" + Helper.StringJoin(point) + Helper.CreateTabString("", newScore));
                    }
                }
                if ((!double.IsNaN(oldScore) && Math.Abs(oldScore - newScore) < eps)
                    || doSearchParameterCount < 2) //If only 0 or 1 searchable params, then one pass is enough
                {
                    oldScore = newScore;
                    gridConverged = true;
                    break;
                }
                oldScore = newScore;
            }

            if (!gridConverged)
            {
                //Console.WriteLine("WARNING: Grid failed to converge after {0} iterations.", _numberOfIterationsOverParameters);
            }

            return oldScore;
        }




        public static void Plot(Converter<double, double> oneDRealFunctionInDoubleSpace, double start, double last, double inc)
        {
            for (double r1 = start; r1 < last; r1 += inc)
            {
                Debug.WriteLine(Helper.CreateTabString(r1, oneDRealFunctionInDoubleSpace(r1)));
            }
        }


        public int DebugCount
        {
            get
            {
                return OneDOptimization.DebugCount;
            }
        }
    }
}
