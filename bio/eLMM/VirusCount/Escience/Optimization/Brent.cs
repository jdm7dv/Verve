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
    public class Brent : OneDOptimization
    {
        public static double GoldenRatio = (3.0 - Math.Sqrt(5.0)) / 2.0;
        public static double InvGold = 1.0 / (1.0 - GoldenRatio);

        public Brent() { }
        

        /// <summary>
        /// Runs brent's method as a MAXIMIZATION procedure (I think).
        /// </summary>
        /// <param name="oneDRealFunction"></param>
        /// <param name="param"></param>
        /// <param name="gridLineCount"></param>
        /// <returns></returns>
        override public double Run(Converter<OptimizationParameter, double> oneDRealFunction,/*ref*/ OptimizationParameter param)
        {
            ++DebugCount;

            double outputTol = .00000001;

            Converter<double, double> oneDRealFunctionInDoubleSpace = delegate(double d)
            {
                Debug.Assert(!double.IsNaN(d));
                param.ValueForSearch = d;
                try
                {
                    double odrf = oneDRealFunction(param);
                    if (double.IsNaN(odrf))
                    {
                        return double.PositiveInfinity;
                    }
                    //Debug.Assert(!double.IsNegativeInfinity(odrf)); //!!!for debugging
                    return -odrf;
                }
                catch (Exception exception)
                {
                    Console.WriteLine("Exception turned to NaN in OneDOptimizationBrent");
                    Console.WriteLine(exception.Message);
                    if (exception.InnerException != null)
                    {
                        Console.WriteLine(exception.InnerException.Message);
                    }
                    return double.NaN;
                }

            };

            double a = param.LowForSearch;
            double b = param.ValueForSearch;
            Debug.WriteLine(Helper.CreateTabString(DebugCount, "Get a and b from param\n", "", "a", a, "\n", "", "b", b, "\n"));

            double c;
            // !! why would we not want to always base our starting off low and high, rather than arbitrarily basing high of starting and low??
            // !! hm, note that changing it so this always goes in does some weird things to TreeBuilding regression tests. Maybe I'm missing something... JC
            if (a > b || Math.Abs(a - b) < Tolerance || double.IsInfinity(b))
            {
                c = param.HighForSearch;
                b = a + GoldenRatio * (c - a);
                Debug.WriteLine(Helper.CreateTabString(DebugCount, "if (a > b || Math.Abs(a - b) < inputTol || double.IsInfinity(b)){c = param.HighForSearch;b = a + goldenRatio * (c - a);}\n", "", "a", a, "\n", "", "b", b, "\n", "", "c", c, "\n"));

            }
            else
            {
                Debug.Assert(a < b && Math.Abs(a - b) > Tolerance); // real assert
                c = b + InvGold * (b - a);
                Debug.WriteLine(Helper.CreateTabString(DebugCount, "NOT if (a > b || Math.Abs(a - b) < inputTol || double.IsInfinity(b)){c = b + invGold * (b - a);}", "a", a, "", "\n", "", "b", b, "", "\n", "", "c", c, "", "\n"));
            }


            double fOfA;
            double fOfB;
            double fOfC;
            double bestInput;
            if (FindBracketWithInfinities(ref a, ref b, ref c, oneDRealFunctionInDoubleSpace, Tolerance, outputTol, out bestInput, out fOfA, out fOfB, out fOfC, DebugCount))
            {
                param.ValueForSearch = bestInput;
                return -fOfB;
            }


            Debug.WriteLine(Helper.CreateTabString(DebugCount, "About to enter Brent\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));
            Helper.CheckCondition(fOfB < fOfA && fOfB < fOfC, string.Format("About to enter Brent, but prediction, f(b)<f(a) && f(b) < f(c), has failed"));
            FuncMin.FuncMinBrent(a, ref b, c, fOfA, ref fOfB, fOfC, Tolerance, oneDRealFunctionInDoubleSpace);
            Debug.WriteLine(Helper.CreateTabString(DebugCount, "After Brent\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));
            bestInput = b;
            param.ValueForSearch = bestInput;

            return -fOfB;
        }

        static public bool FindBracketWithInfinities(ref double a, ref double b, ref double c, Converter<double, double> oneDRealFunctionInDoubleSpace, double inputTol, double outputTol, out double bestInput, out double fOfA, out double fOfB, out double fOfC, int debugCount)
        {

            fOfA = oneDRealFunctionInDoubleSpace(a);
            fOfB = oneDRealFunctionInDoubleSpace(b);
            fOfC = oneDRealFunctionInDoubleSpace(c);
            Debug.WriteLine(Helper.CreateTabString(debugCount, "Evaluate a,b,c\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));

            //double start = -13;
            //double last = 20;
            //double inc = .25;
            //Plot(oneDRealFunctionInDoubleSpace, start, last, inc);

            bool flatMinimumTowardPosInf = false;
            if (fOfC < fOfB || Math.Abs(fOfC - fOfB) < outputTol) //!!!Need to consider tolerance?
            {
                Debug.WriteLine(Helper.CreateTabString(debugCount, "if (fOfC < fOfB || Math.Abs(fOfC - fOfB) < outputTol) [about to enter BracketNearPositiveInfinity]\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));

                bool foundMin = BracketNearPositiveInfinity(out a, ref b, ref c, out fOfA, ref fOfB, ref fOfC, out flatMinimumTowardPosInf, inputTol, outputTol, oneDRealFunctionInDoubleSpace, debugCount);
                if (foundMin)
                {
                    Debug.WriteLine(Helper.CreateTabString(debugCount, "BracketNearPositiveInfinity returns true\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));
                    bestInput = b;
                    return true;
                }
                Debug.WriteLine(Helper.CreateTabString(debugCount, "BracketNearPositiveInfinity returns false\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));
                //Either everything to the right is flat or we found an  a,b,c with the right relation
                Debug.Assert(flatMinimumTowardPosInf || (fOfB < fOfA && fOfB < fOfC)); // real assert

            }

            if (flatMinimumTowardPosInf)
            {
                Debug.Assert(Math.Abs(fOfA - fOfB) < outputTol);
            }

            // why if flatMinToPosInf?
            if (fOfA < fOfB || Math.Abs(fOfA - fOfB) < outputTol || flatMinimumTowardPosInf) //!!!Need to consider tolerance?
            {
                Debug.WriteLine(Helper.CreateTabString(debugCount, "if (fOfA < fOfB || double.IsNaN(c)) [about to enter BracketNearNegativeInfinity]\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));
                bool flatMinimumTowardNegInf;
                bool foundMin = BracketNearNegativeInfinity(ref a, ref b, out c, ref fOfA, ref fOfB, out fOfC, out flatMinimumTowardNegInf, inputTol, outputTol, oneDRealFunctionInDoubleSpace, debugCount);

                if (foundMin)
                {
                    Debug.WriteLine(Helper.CreateTabString(debugCount, "BracketNearNegativeInfinity returns true\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));
                    bestInput = b;
                    return true;
                }
                Debug.WriteLine(Helper.CreateTabString(debugCount, "BracketNearNegativeInfinity returns false\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));

                //If there was a min at pos Inf
                if (flatMinimumTowardPosInf)
                {
                    //If also one a min inf, then flat
                    if (flatMinimumTowardNegInf)
                    {
                        Debug.WriteLine(Helper.CreateTabString(debugCount, "if flat [flatMinimumTowardPosInf && flatMinimumTowardNegInf] return\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));
                        bestInput = 0;
                        return true;
                    }
                    else
                    {
                        Debug.WriteLine(Helper.CreateTabString(debugCount, "if min at Pos Inf only return\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));
                        bestInput = double.PositiveInfinity;
                        return true;
                    }
                }
                if (flatMinimumTowardNegInf)
                {
                    Debug.WriteLine(Helper.CreateTabString(debugCount, "if min at Neg Inf only return\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));
                    bestInput = double.NegativeInfinity;
                    return true;
                }
                Debug.Assert(fOfB < fOfA && fOfB < fOfC); // real assert

            }

            int countToCheck = 0;
            while ((Math.Abs((a + c) / 2.0 - b) < inputTol)) //!!!can this really loop more than once?
            {
                Debug.WriteLine(Helper.CreateTabString(debugCount, "while (Math.Abs((a + c) / 2.0 - b) < inputTol)) about to {c = c + (c - a)}\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));
                c = c + (c - a);
                ++countToCheck;
                Helper.CheckCondition(countToCheck < 10);
            }
            if (countToCheck > 0)
            {
                fOfC = oneDRealFunctionInDoubleSpace(c);
            }

            Debug.WriteLine(Helper.CreateTabString(debugCount, "NOT (Math.Abs((a + c) / 2.0 - b) < inputTol))\n", "", "a", a, fOfA, "\n", "", "b", b, fOfB, "\n", "", "c", c, fOfC, "\n"));
            bestInput = double.NaN;
            return false;

        }

        static bool BracketNearNegativeInfinity(ref double ftA, ref double ftB, out double ftC,
            ref double ftFNOfA, ref double ftFNOfB, out double ftFNOfC, out bool flatMinimumTowardNegInf, double inputTol, double outputTol, Converter<double, double> oneDRealFunctionInDoubleSpace, int debugCount)
        {
            //Flip the X axis
            double antiA; ;
            double antiB = -ftB;
            double antiC = -ftA;
            double antiFOfA;
            double antiFOfB = ftFNOfB;
            double antiFOfC = ftFNOfA;
            Converter<double, double> antiFunction = delegate(double antiInput) { return oneDRealFunctionInDoubleSpace(-antiInput); };

            bool foundMin = BracketNearPositiveInfinity(out antiA, ref antiB, ref antiC, out antiFOfA, ref antiFOfB, ref antiFOfC, out flatMinimumTowardNegInf, inputTol, outputTol, antiFunction, debugCount);

            ftC = -antiA;
            ftB = -antiB;
            ftA = -antiC;

            ftFNOfC = antiFOfA;
            ftFNOfB = antiFOfB;
            ftFNOfA = antiFOfC;

            return foundMin;
        }

        static bool BracketNearPositiveInfinity(
            out double a, ref double b, ref double c,
            out double fOfA, ref double fOfB, ref double fOfC,
            out bool flatMinimumTowardPosInf,
            double inputTol, double outputTol, Converter<double, double> oneDRealFunctionInDoubleSpace, int debugCount)
        {
            Helper.CheckCondition(!double.IsNaN(fOfB) && !double.IsNaN(fOfC), "One of the starting points for BracketNearInfinity evaluates to NaN");
            flatMinimumTowardPosInf = false;
            double pseudoInfinity = double.PositiveInfinity;
            double fOfInfinity = oneDRealFunctionInDoubleSpace(pseudoInfinity);


            if (double.IsPositiveInfinity(fOfInfinity))
            {
                //Nevermind about minimizing evaluations and remembering evaluations. Just try to find a value that can take the place of infinity
                //Try 10000, 7500, .... 
                //       If gets too close to c (relative to b), just give up
                //pseudoInfinity = 10000;
                //while (true)
                //{
                //    fOfInfinity = oneDRealFunctionInDoubleSpace(pseudoInfinity);
                //    if (!double.IsPositiveInfinity(fOfInfinity))
                //    {
                //        break;
                //    }
                //    pseudoInfinity = b + (pseudoInfinity - b) * .75; //This won't work: infinity = infinity *.75 because it will go to 0 in the limit and b and c could be negative.
                //    Helper.CheckCondition(c < pseudoInfinity, "Can't find an evaluatable 'infinity' point");
                //}
                double high = 10000;
                double fOfHigh = oneDRealFunctionInDoubleSpace(high);
                if (double.IsPositiveInfinity(fOfHigh))
                {
                    BinarySearchForNonInfiniteEvaluation(c, fOfC, high, fOfHigh, out pseudoInfinity, out fOfInfinity, oneDRealFunctionInDoubleSpace, inputTol, outputTol);
                }
                else
                {
                    pseudoInfinity = high;
                    fOfInfinity = fOfHigh;
                }
            }

            if (!double.IsPositiveInfinity(pseudoInfinity) && fOfInfinity > fOfC + outputTol)
            {
                a = b;
                b = c;
                c = pseudoInfinity;
                fOfA = fOfB;
                fOfB = fOfC;
                fOfC = fOfInfinity;
                return false;
            }

            Debug.WriteLine(Helper.CreateTabString(debugCount, "Entering BracketNearPositiveInfinity\n",
                "", "b", b, fOfB, "\n",
                "", "c", c, fOfC, "\n",
                "", "inf", "inf", fOfInfinity, "\n"));

            //FindTheSmallestInputThatGivesSameAnswerAsInf
            Debug.Assert(b < c && (fOfB >= fOfC || Math.Abs(fOfB - fOfC) < outputTol)); // real assert


            //DoubleCUntilFindSFInf - the current c might be good enough
            double previous = b;
            double fOfPrevious = fOfB;
            double cAfterDoubles = c;
            double fOfCAfterDoubles = fOfC;
            double inputBetterThanInf = double.NaN;
            double outputBetterThanInf = double.NaN;

            Debug.WriteLine(Helper.CreateTabString(debugCount, "About to loop\n",
                "", "b", b, fOfB, "\n",
                "", "previous", previous, fOfPrevious, "\n",
                "", "c", c, fOfC, "\n",
                "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n",
                "", "betterThanInf", inputBetterThanInf, outputBetterThanInf, "\n",
                "", "inf", "inf", fOfInfinity, "\n"));


            while (true)
            {

                if (!double.IsPositiveInfinity(fOfInfinity) && Math.Abs(fOfCAfterDoubles - fOfInfinity) < outputTol) //!!!const
                {
                    Debug.WriteLine(Helper.CreateTabString(debugCount, "if (Math.Abs(fOfCAfterDoubles - fOfInfinity) < outputTol){break loop}\n",
                        "", "b", b, fOfB, "\n",
                        "", "previous", previous, fOfPrevious, "\n",
                        "", "c", c, fOfC, "\n",
                        "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n",
                        "", "betterThanInf", inputBetterThanInf, outputBetterThanInf, "\n",
                        "", "inf", "inf", fOfInfinity, "\n"));
                    break;
                }


                //if (fOfCAfterDoubles < fOfInfinity)
                {
                    Debug.WriteLine(Helper.CreateTabString(debugCount, "if (fOfCAfterDoubles < fOfInfinity)...\n",
                        "", "b", b, fOfB, "\n",
                        "", "previous", previous, fOfPrevious, "\n",
                        "", "c", c, fOfC, "\n",
                        "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n",
                        "", "betterThanInf", inputBetterThanInf, outputBetterThanInf, "\n",
                        "", "inf", "inf", fOfInfinity, "\n"));
                    if (fOfCAfterDoubles < fOfInfinity - outputTol && (double.IsNaN(inputBetterThanInf) || fOfCAfterDoubles < outputBetterThanInf)) // /*|| Math.Abs(fOfCAfterDoubles - outputBetterThanInf) < outputTol*/)
                    {
                        inputBetterThanInf = cAfterDoubles;
                        outputBetterThanInf = fOfCAfterDoubles;

                        Debug.WriteLine(Helper.CreateTabString(debugCount, "if (double.IsNaN(inputBetterThanInf) || fOfCAfterDoubles < outputBetterThanInf || Math.Abs(fOfCAfterDoubles - outputBetterThanInf) < outputTol){BetterThanInf = cAfterDoubles}\n",
                            "", "b", b, fOfB, "\n",
                            "", "previous", previous, fOfPrevious, "\n",
                            "", "c", c, fOfC, "\n",
                            "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n",
                            "", "betterThanInf", inputBetterThanInf, outputBetterThanInf, "\n",
                            "", "inf", "inf", fOfInfinity, "\n"));

                    }
                    else if (!double.IsNaN(inputBetterThanInf))
                    {
                        //We've found a min and after a value that is not quite as good but still better's than infinity's
                        if (c == inputBetterThanInf)
                        {
                            a = b;
                            fOfA = fOfB;

                            Debug.WriteLine(Helper.CreateTabString(debugCount, "We've found a min and after a value that is not quite as good but still better's than infinity'sWe've found a min and after a value that is not quite as good but still better's than infinity's && c == inputBetterThanInf {a =  b;}\n",
                                "", "a", a, fOfA, "\n",
                                "", "b", b, fOfB, "\n",
                                "", "previous", previous, fOfPrevious, "\n",
                                "", "c", c, fOfC, "\n",
                                "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n",
                                "", "betterThanInf", inputBetterThanInf, outputBetterThanInf, "\n",
                                "", "inf", "inf", fOfInfinity, "\n"));

                        }
                        else
                        {
                            a = c;
                            fOfA = fOfC;

                            Debug.WriteLine(Helper.CreateTabString(debugCount, "We've found a min and after a value that is not quite as good but still better's than infinity'sWe've found a min and after a value that is not quite as good but still better's than infinity's && NOT c == inputBetterThanInf {a =  c;}\n",
                                "", "a", a, fOfA, "\n",
                                "", "b", b, fOfB, "\n",
                                "", "previous", previous, fOfPrevious, "\n",
                                "", "c", c, fOfC, "\n",
                                "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n",
                                "", "betterThanInf", inputBetterThanInf, outputBetterThanInf, "\n",
                                "", "inf", "inf", fOfInfinity, "\n"));

                        }

                        b = inputBetterThanInf;
                        c = cAfterDoubles;
                        fOfB = outputBetterThanInf;
                        fOfC = fOfCAfterDoubles;

                        Debug.WriteLine(Helper.CreateTabString(debugCount, "b = inputBetterThanInf;c = cAfterDoubles;return false\n",
                            "", "a", a, fOfA, "\n",
                            "", "b", b, fOfB, "\n",
                            "", "previous", previous, fOfPrevious, "\n",
                            "", "c", c, fOfC, "\n",
                            "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n",
                            "", "betterThanInf", inputBetterThanInf, outputBetterThanInf, "\n",
                            "", "inf", "inf", fOfInfinity, "\n"));

                        return false;
                    }
                }

                previous = cAfterDoubles;
                cAfterDoubles = b + (cAfterDoubles - b) * InvGold;
                //Debug.Assert(cAfterDoubles < pseudoInfinity);
                fOfPrevious = fOfCAfterDoubles;

                // we're guaranteed that pseudoInfinity is the highest number that doesn't evaluate to infinite. 
                // if we set cAfterDoubles to pseudoInfinity, then we'll hit good stopping conditions above.
                if (cAfterDoubles > pseudoInfinity)
                {
                    cAfterDoubles = pseudoInfinity;
                    fOfCAfterDoubles = fOfInfinity;
                }
                else
                {
                    fOfCAfterDoubles = oneDRealFunctionInDoubleSpace(cAfterDoubles);
                }

                Debug.Assert(!double.IsNaN(fOfInfinity) || !double.IsInfinity(fOfCAfterDoubles));
                //if (double.IsNaN(fOfInfinity) && double.IsNaN(fOfCAfterDoubles))
                //{
                //    // everything from here on out is assumed to be NaN
                //    Debug.WriteLine(Helper.CreateTabString(DebugCount, "infinity return an exception and so did this value\n",
                //        "", "b", b, fOfB, "\n",
                //        "", "previous", previous, fOfPrevious, "\n",
                //        "", "c", c, fOfC, "\n",
                //        "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n",
                //        "", "betterThanInf", inputBetterThanInf, outputBetterThanInf, "\n",
                //        "", "inf", "inf", fOfInfinity, "\n"));

                //    Debug.WriteLine("Case of interest");

                //}

                Debug.WriteLine(Helper.CreateTabString(debugCount, "setup for next loop: previous = cAfterDoubles; cAfterDoubles = b + (cAfterDoubles - b) * invGold; fOfPrevious = fOfCAfterDoubles;\n",
                    "", "b", b, fOfB, "\n",
                    "", "previous", previous, fOfPrevious, "\n",
                    "", "c", c, fOfC, "\n",
                    "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n",
                    "", "betterThanInf", inputBetterThanInf, outputBetterThanInf, "\n",
                    "", "inf", "inf", fOfInfinity, "\n"));

            }

            //We found a value better than inf and we found a value as good as inf
            if (!double.IsNaN(inputBetterThanInf))
            {
                a = (c == inputBetterThanInf) ? b : c;
                fOfA = (c == inputBetterThanInf) ? fOfB : fOfC;
                b = inputBetterThanInf;
                fOfB = outputBetterThanInf;
                c = cAfterDoubles;
                fOfC = fOfCAfterDoubles;


                Debug.WriteLine(Helper.CreateTabString(debugCount, "out of loop, something better than infinity: a = (c == inputBetterThanInf) ? b : c; b = inputBetterThanInf; c = cAfterDoubles; return false\n",
                    "", "a", a, fOfA, "\n",
                    "", "b", b, fOfB, "\n",
                    "", "previous", previous, fOfPrevious, "\n",
                    "", "c", c, fOfC, "\n",
                    "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n",
                    "", "betterThanInf", inputBetterThanInf, outputBetterThanInf, "\n",
                    "", "inf", "inf", fOfInfinity, "\n"));

                return false;
            }


            //BinarySearchToFindEdge
            double low = previous;
            double fOfLow = fOfPrevious;

            Debug.WriteLine(Helper.CreateTabString(debugCount, "binary search for edge\n",
                "", "b", b, fOfB, "\n",
                "", "c", c, fOfC, "\n",
                "", "low", low, fOfLow, "\n",
                "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n"));

            Debug.Assert(low <= cAfterDoubles); // real assert
            if (Math.Abs(fOfLow - fOfInfinity) < outputTol)
            {
                flatMinimumTowardPosInf = true;
                a = low;
                b = cAfterDoubles;
                c = pseudoInfinity;
                fOfA = fOfLow;
                fOfB = fOfCAfterDoubles;
                fOfC = fOfInfinity;

                Debug.WriteLine(Helper.CreateTabString(debugCount, "if (Math.Abs(fOfLow - fOfCAfterDoubles) < outputTol){flatMinimumTowardPosInf = true;;return false\n",
                    "", "a", a, fOfA, "\n",
                    "", "b", b, fOfB, "\n",
                    "", "c", c, fOfC, "\n",
                    "", "low", low, fOfLow, "\n",
                    "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n"));

                return false;
            }
            Debug.Assert(double.IsNaN(fOfCAfterDoubles) || fOfLow > fOfCAfterDoubles); // real assert
            while (true)
            {
                if (Math.Abs(low - cAfterDoubles) < inputTol)
                {
                    // double.PositiveInfinity is the answer
                    a = double.NaN;
                    b = pseudoInfinity;
                    c = double.NaN;
                    fOfA = double.NaN;
                    fOfB = fOfInfinity;
                    fOfC = double.NaN;

                    Debug.WriteLine(Helper.CreateTabString(debugCount, "In loop: if (Math.Abs(low - cAfterDoubles) < inputTol){a = double.NaN; b = infinity; c = double.NaN;return true}\n",
                        "", "a", a, fOfA, "\n",
                        "", "b", b, fOfB, "\n",
                        "", "c", c, fOfC, "\n"));

                    return true;
                }
                double mid = (low + cAfterDoubles) / 2; //Change of overflow???
                double fOfMid = oneDRealFunctionInDoubleSpace(mid);
                //Debug.Assert(!double.IsNaN(fOfMid));

                Debug.WriteLine(Helper.CreateTabString(debugCount, "mid = (low + cAfterDoubles) / 2;\n",
                    "", "b", b, fOfB, "\n",
                    "", "c", c, fOfC, "\n",
                    "", "mid", mid, fOfMid, "\n",
                    "", "low", low, fOfLow, "\n",
                    "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n"));

                if (Math.Abs(fOfMid - fOfInfinity) < outputTol)
                {
                    cAfterDoubles = mid;
                    fOfCAfterDoubles = fOfMid;

                    Debug.WriteLine(Helper.CreateTabString(debugCount, "if (Math.Abs(fOfMid - fOfInfinity) < outputTol) {cAfterDoubles = mid;continue}\n",
                        "", "b", b, fOfB, "\n",
                        "", "c", c, fOfC, "\n",
                        "", "mid", mid, fOfMid, "\n",
                        "", "low", low, fOfLow, "\n",
                        "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n"));

                    continue;
                }
                if (fOfMid < fOfInfinity)
                {
                    // Found point better than infinity
                    a = low;
                    b = mid;
                    c = b + InvGold * (b - a);
                    Debug.Assert(Math.Abs(oneDRealFunctionInDoubleSpace(c) - fOfInfinity) < outputTol); // real assert
                    fOfA = fOfLow;
                    fOfB = fOfMid;
                    fOfC = fOfInfinity;

                    Debug.WriteLine(Helper.CreateTabString(debugCount, "if (fOfMid < fOfInfinity) {a = low; b = mid; c = b + invGold * (b - a); return false}\n",
                        "", "a", a, fOfA, "\n",
                        "", "b", b, fOfB, "\n",
                        "", "c", c, fOfC, "\n",
                        "", "mid", mid, fOfMid, "\n",
                        "", "low", low, fOfLow, "\n",
                        "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n"));

                    return false;

                }
                Debug.Assert(fOfMid > fOfInfinity); // real assert

                Debug.WriteLine(Helper.CreateTabString(debugCount, "get ready for next loop: low = mid;}\n",
                    "", "b", b, fOfB, "\n",
                    "", "c", c, fOfC, "\n",
                    "", "mid", mid, fOfMid, "\n",
                    "", "low", low, fOfLow, "\n",
                    "", "cAfterDoubles", cAfterDoubles, fOfCAfterDoubles, "\n"));

                low = mid;
                fOfLow = fOfMid;

            }
        }


        private static void BinarySearchForNonInfiniteEvaluation(
            double nonInfInput, double nonInfOutput,
            double infInput, double infOutput,
            out double bestInput, out double bestOutput,
            //FuncEval nonInfiniteEval, FuncEval infiniteEval, 
            Converter<double, double> oneDRealFunctionInSearchSpace, double inputTol, double outputTol)
        {
            Helper.CheckCondition(!double.IsInfinity(nonInfOutput));

            double mid = (nonInfInput + infInput) / 2.0;
            double fOfMid = oneDRealFunctionInSearchSpace(mid);

            //FuncEval mid = new FuncEval((nonInfiniteEval.X + infiniteEval.X) / 2, oneDRealFunctionInSearchSpace);

            Debug.WriteLine(string.Format("Binary search: f({0}) = {1}, f({2}) = {3}", nonInfInput, nonInfOutput, infInput, infOutput));
            if (Math.Abs(fOfMid - nonInfOutput) < outputTol)
            {
                Debug.Assert(!double.IsInfinity(fOfMid));
                bestInput = mid;
                bestOutput = fOfMid;
            }
            else if (Math.Abs(mid - nonInfInput) < inputTol)
            {
                bestInput = nonInfInput;
                bestOutput = nonInfOutput;
            }
            else if (double.IsInfinity(fOfMid))
            {
                BinarySearchForNonInfiniteEvaluation(nonInfInput, nonInfOutput, mid, fOfMid, out bestInput, out bestOutput, oneDRealFunctionInSearchSpace, inputTol, outputTol);
            }
            else
            {
                BinarySearchForNonInfiniteEvaluation(mid, fOfMid, infInput, infOutput, out bestInput, out bestOutput, oneDRealFunctionInSearchSpace, inputTol, outputTol);
            }
        }

    }
}
