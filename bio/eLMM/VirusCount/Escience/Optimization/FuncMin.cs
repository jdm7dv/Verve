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

#undef TRACE

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MBT.Escience.Optimization
{

    static public class FuncMin
    {
        ///Initial bracketing:
        ///Input: ftAx and ftBx, two initial guesses
        ///Output: ftAx, ftBx, and ftCx, three points bracketing a minimum
        ///Raises exception of minimum not found
        ///i.e. ftAx &lt; ftBx &lt; ftCx, and Fn(ftAx) &gt; Fn(ftBx), and Fn(ftBx) &lt; Fn(ftCx)
        ///Note that this will just return bracketing points for the first minimum found between pftAx and pftBx.
        ///If there are multiple minima, you're not guaranteed that the one returned is global.
        ///This is based on the description (but not the code!) of the function mnbrak in Numerical Recipes in C++, p. 404
        static public void InitialBracket(ref double ftAx, ref double ftBx, out double ftCx, out double FNOfA, out double FNOfB, out double FNOfC, Converter<double, double> fn)
        {
            Trace.WriteLine("Entering InitialBracket, using values " + ftAx.ToString() + " and " + ftBx.ToString());
            const int MAX_ITERATIONS = 1000;

            Debug.Assert(ftAx != ftBx);

            double ftFofAx = fn(ftAx);
            Debug.WriteLine(string.Format("a\t{0}\t{1}", ftAx, ftFofAx));
            double ftFofBx = fn(ftBx);
            Debug.WriteLine(string.Format("b\t{0}\t{1}", ftBx, ftFofBx));
            double ftFofCx = 0;
            double ftRatio = 1.5;	//The ratio of the difference between Ax and Bx that we'll use to step at the next step
            //So long as this is greater than 1, we should converge quadratically (taking bigger and bigger steps)

            //Make sure ftFofAx is smaller than ftFofBx
            if (ftFofBx > ftFofAx)
            {
                Swap(ref ftFofAx, ref ftFofBx);
                Swap(ref ftAx, ref ftBx);
            }

            //Now, just step downhill (larger and larger steps) until we find a Fn evaluation that's greater than ftFofBx
            int iNumIterations = 1;
            ftCx = ftBx + (ftRatio * ftBx - ftAx);

            ftFofCx = fn(ftCx);
            Debug.WriteLine(string.Format("c\t{0}\t{1}", ftCx, ftFofCx));


            double ftPrev = double.MaxValue;

            while (ftFofCx <= ftFofBx)
            {
                ++iNumIterations;
                if (iNumIterations > MAX_ITERATIONS)
                {
                    throw new Exception("Exceeded maximum number of iterations");
                }
                ftCx = ftBx + (ftRatio * (ftBx - ftAx));
                Debug.Assert(ftCx != ftPrev);
                ftPrev = ftCx;

                ftFofCx = fn(ftCx);
                Debug.WriteLine(string.Format("c\t{0}\t{1}", ftCx, ftFofCx));


                if (ftFofCx <= ftFofBx)
                {
                    ftFofBx = ftFofCx;
                    ftBx = ftCx;
                }

            }

            //If we're backwards (i.e. the two points passed in were both to the right of a minimum), then reverse the points.
            if (ftAx > ftBx && ftBx > ftCx)
            {
                double ftTmp = ftAx;
                ftAx = ftCx;
                ftCx = ftTmp;
            }

            FNOfA = ftFofAx;
            FNOfB = ftFofBx;
            FNOfC = ftFofCx;

            Trace.WriteLine("Leaving InitialBracket after" + iNumIterations.ToString() + " iterations");
            Trace.WriteLine("Bracketing triple is: " + ftAx.ToString() + "(" + ftFofAx.ToString() + ")" + ", " + ftBx.ToString() + "(" +
                ftFofBx.ToString() + ")" + ", " + ftCx.ToString() + "(" + ftFofCx.ToString() + ")");


            //Postconditions: 
            Debug.Assert(ftAx < ftBx && ftBx < ftCx);
            Debug.Assert(ftFofAx >= ftFofBx && ftFofBx <= ftFofCx);
            Debug.Assert(FNOfB <= FNOfA && FNOfB <= FNOfC);
        }

        private static void Swap(ref double r1, ref double r2)
        {
            double temp = r1;
            r1 = r2;
            r2 = temp;
        }


        /// <summary>
        /// GoldenSectionSearch.
        /// Input: Three points, ftA, ftB and ftC, such that ftA &lt; ftB &lt; ftC, and double(ftA) &gt; double(ftB) AND double(ftC) &gt; double(ftB)
        /// Output: Return value (And ftB) will be set to a value within ftTol of a minimum of the function
        /// Based on description (but not the code!) of "Golden Section Search in One dimension", Numerical Recipes in C++, pp. 401-404.
        /// </summary>
        static public double FuncMinGoldenSection(double ftA, ref double ftB, double ftC, double FNOfA, double FNOfB, double FNOfC, double ftTol, Converter<double, double> fn)
        {
            Trace.WriteLine("Entering GoldenSectionSearch");

            Debug.Assert(ftA < ftB);
            Debug.Assert(ftB < ftC);
            Debug.Assert(FNOfA >= FNOfB);
            Debug.Assert(FNOfC >= FNOfB);
            Debug.Assert(ftTol > 0);

            double ftDiff = (ftC - ftA);
            if (ftDiff < 0)
            {
                ftDiff = -ftDiff;
            }
            int iNumGoldenIterations = 0;
            double ftPrevX = double.MaxValue;
            while (ftDiff > ftTol)
            {
                ++iNumGoldenIterations;
                //Choose a new point, x, between either a & b or between b & c
                //This point should be a fraction of 0.38197 into the larger of the two intervals (a-b || b-c).

                double ftX = 0;
                double ftAToB = ftB - ftA;
                double ftBToC = ftC - ftB;
                const double ftGold = 0.38197;
                Debug.Assert(ftAToB > 0 && ftBToC > 0);

                if (ftAToB > ftBToC)
                {
                    ftX = ftB - (ftGold * ftAToB);
                }
                else
                {
                    ftX = ftB + (ftGold * ftBToC);
                }

                Debug.Assert(ftX != ftPrevX);
                Debug.Assert(ftX > ftA && ftX < ftC);

                ftPrevX = ftX;

                //Evaluate the function there.

                //Choose the new triplet appropriately, such that b is the best minimum we've found so far.
                //if x is between b and c, 
                double ftFOfX = fn(ftX);
                if (ftX > ftB)
                //then the new triplet will be {a, b, x} if f(b) < f(x), and {b, x, c} otherwise.
                {
                    if (FNOfB < ftFOfX)
                    {
                        ftC = ftX;
                        FNOfC = ftFOfX;
                    }
                    else
                    {
                        ftA = ftB;
                        FNOfA = FNOfB;
                        ftB = ftX;
                        FNOfB = ftFOfX;
                    }
                }
                else //X was between a and b - analogous to condition above
                {
                    Debug.Assert(ftA < ftX && ftX < ftB);
                    if (FNOfB < ftFOfX)
                    {
                        ftA = ftX;
                        FNOfA = ftFOfX;
                    }
                    else
                    {
                        ftC = ftB;
                        FNOfC = FNOfB;
                        ftB = ftX;
                        FNOfB = ftFOfX;
                    }
                }

                //Check for done
                ftDiff = (ftC - ftA);
                if (ftDiff < 0)
                {
                    ftDiff = -ftDiff;
                }


                Trace.WriteLine("A == " + ftA + ", B == " + ftB + ", C == " + ftC);
                Trace.WriteLine("Diff: " + ftDiff);

                Debug.Assert(ftDiff >= 0);
            } //end while

            Trace.WriteLine("Exiting GoldenSectionSearch - took " + iNumGoldenIterations + " iterations");

            Debug.Assert(ftB >= ftA && ftB <= ftC);
            Debug.Assert(FNOfA > FNOfB && FNOfC > FNOfB);
            Debug.Assert(ftDiff <= ftTol);
            return ftB;
        }


        //Brent's Method
        //Input: Three points, ftA, ftB and ftC, such that ftA < ftB < ftC, and double(ftA) > double(ftB) && double(ftC) > double(ftB)
        //Output: Return value (And ftB) will be set to a value within ftTol of a minimum of the function
        //Based on the description (but not the code!) in Numerical Recipes in C++, pp. 406-408
        static public void FuncMinBrent(double ftA, ref double ftB, double ftC, double FNOfA, ref double FNOfB, double FNOfC, double ftTol, Converter<double, double> fn)
        {
            Debug.Assert(ftA < ftB && ftB < ftC);
            Debug.Assert(FNOfA >= FNOfB && FNOfC >= FNOfB);
            const int MAX_ITERATIONS = 1000;
            const double GOLDEN_RATIO = 0.38197;
            //const double TINY_NONZERO_NUMBER = 0.000000001;

            //For the (nonexistent) previous movements, let's just pretend we came from the midpoint between ftA and ftC.
            //(After the first two iterations we'll be on track anyway)
            double ftTotalBracketingDistance = ftC - ftA;
            Debug.Assert(ftTotalBracketingDistance > 0);
            double ftMidPoint = ftA + ftTotalBracketingDistance / 2;
            double ftSupposedMovement = Math.Abs(ftMidPoint - ftB);
            if (ftSupposedMovement < ftTol)
            {
                throw new Exception("point ftB is too close to the middle of ftA and ftC");
            }
            double ftUltimateMovement = ftSupposedMovement;
            double ftPenultimateMovement = ftSupposedMovement;


            for (int i = 0; i < MAX_ITERATIONS; ++i)
            {
                //loop invariants
                Debug.Assert(ftA < ftB && ftB < ftC);
                Debug.Assert(FNOfA >= FNOfB && FNOfC >= FNOfB);

                Trace.WriteLine("Entering Brent iteration " + i);
                Trace.WriteLine("A: " + ftA);
                Trace.WriteLine("B: " + ftB);
                Trace.WriteLine("C: " + ftC);

                //The tolerance we'll be using, scaled to the current best point
                //TINY_NONZERO_NUMBER is a little hack for the case where the minimum is at zero.
                //double ftRealTol = ftTol * Math.Abs(ftB) + TINY_NONZERO_NUMBER;
                double ftRealTol = ftTol;
                Debug.Assert(ftRealTol > 0);

                ftMidPoint = (ftC + ftA) / 2;
                Debug.Assert(ftMidPoint > ftA && ftMidPoint < ftC);

                //Check for done:
                if (Math.Abs(ftMidPoint - ftB) <= ftRealTol)
                {
                    Trace.WriteLine("Found minimum of " + FNOfB + " at " + ftB);

                    return;
                }

                double ftX = 0; //This is the new trial point we'll be evaluating

                //Compute the parabolic step, and see if we can use it.
                double ftNumerator = ((ftB - ftA) * (ftB - ftA) * (FNOfB - FNOfC)) - ((ftB - ftC) * (ftB - ftC) * (FNOfB - FNOfA));
                double ftDenominator = ((ftB - ftA) * (FNOfB - FNOfC)) - ((ftB - ftC) * (FNOfB - FNOfA));
                double ftParabolic = ftB - (0.5 * (ftNumerator / ftDenominator));

                //According to n.r., "To be acceptable, the parabolic step must:
                //	i) Fall within the boundary interval (a,b), and
                //	ii)imply a movement from the best current value x that is LESS THAN half the movement of the step before last.
                //This second criterion insures that the parabolic steps are actually converging to something, rather than, say, bouncing
                //around in some nonconvergent limit cycle."
                bool fInCurrentInterval = ftParabolic > ftA && ftParabolic < ftC;
                bool fMovementAcceptable = Math.Abs(ftParabolic - ftB) < ftPenultimateMovement / 2;

                if (fInCurrentInterval && fMovementAcceptable)
                {
                    Trace.WriteLine("Trying parabolic move");

                    ftX = ftParabolic;
                }

                else //One of the conditions above failed: use golden ratio movement instead
                {
                    Trace.WriteLine("Trying golden section move");

                    if (ftB - ftA > ftC - ftB) //Bigger sector is between A and B - take a golden step from B toward A
                    {
                        ftX = ftB - (GOLDEN_RATIO * (ftB - ftA));
                        Debug.Assert(ftX < ftB && ftA < ftX);
                    }
                    else //take a golden step from B toward C
                    {
                        ftX = ftB + (GOLDEN_RATIO * (ftC - ftB));
                        Debug.Assert(ftX < ftC && ftX > ftB);
                    }
                }


                //Make sure that this candidate point, however we computed it, is
                //sufficiently far away from any evaluated points, because there's no point
                //doing the evaluation otherwise since the new point will fall within our roundoff error.
                if (Math.Abs(ftX - ftB) <= ftRealTol)
                {
                    Trace.WriteLine("Move to " + ftX + " not far enough from tol - setting to tol");

                    if (ftB - ftA > ftC - ftB) //Bigger sector is between A and B
                    {
                        ftX = ftB - ftRealTol;
                    }
                    else
                    {
                        ftX = ftB + ftRealTol;
                    }
                }

                //Now we have a candidate point ftX somewhere between ftA and ftC, which we'll use to create a new bracketing
                Debug.Assert(ftX > ftA && ftX < ftC);

                //Figure out the actual amount of movement so we can remember it for the iteration after next
                double ftMovement = Math.Abs(ftX - ftB);

                //Evaluate the fn at the candidate point
                double FNOfX = fn(ftX);

                //Now we reset all our variables accordingly.
                if (FNOfX < FNOfB) //X will be our new abscissa
                {
                    if (ftX < ftB) //X is between A and B, and is the best minimum, so the new bracket is a x b
                    {
                        ftC = ftB;
                        FNOfC = FNOfB;
                        ftB = ftX;
                        FNOfB = FNOfX;
                    }
                    else //X is between B and C, and is still the best minimum, so the new bracket is b x c
                    {
                        ftA = ftB;
                        FNOfA = FNOfB;
                        ftB = ftX;
                        FNOfB = FNOfX;
                    }

                }
                else //B remains the abscissa, we just move one of A or C to be X
                {
                    if (ftX < ftB) //X between A and B - new bracket is X B C
                    {
                        ftA = ftX;
                        FNOfA = FNOfX;
                    }
                    else //X between B and C - new bracket is A B X
                    {
                        ftC = ftX;
                        FNOfC = FNOfX;
                    }

                }

                ftPenultimateMovement = ftUltimateMovement;
                ftUltimateMovement = ftMovement;
            }

            throw new Exception("Max Brent reached!!!");
        }

        //Adapts multidimensional function double to a single-dimensional function
        //for minimization along a specified direction
        static Converter<double, double> OneDAdapter(Converter<List<double>, double> multiDimFunc, List<double> vftCurrentPointSet, List<double> vftCurrentDirectionSet)
        {
            Debug.Assert(vftCurrentPointSet.Count > 0);
            Debug.Assert(vftCurrentPointSet.Count == vftCurrentDirectionSet.Count);

            List<double> vftCurrentPoint = new List<double>(vftCurrentPointSet);
            List<double> vftCurrentDirection = new List<double>(vftCurrentDirectionSet);

            Converter<double, double> fn1d = delegate(double ftScalar)
                {
                    Debug.Assert(vftCurrentPoint.Count > 0);
                    //The point at which we'll evaluate the actual multidimensional function
                    List<double> vftEvalPoint = new List<double>(vftCurrentPoint.Count);
                    for (int st = 0; st < vftCurrentPoint.Count; ++st)
                    {
                        vftEvalPoint.Add(vftCurrentPoint[st] + ftScalar * vftCurrentDirection[st]);
                    }
                    double ftRet = multiDimFunc(vftEvalPoint);
                    return ftRet;
                };
            return fn1d;

        }


        //struct OneDAdapter : unary_function<double, double>
        //{

        //    void SetState(List<double> vftCurrentPointSet, List<double> vftCurrentDirectionSet, int stDimSet)
        //    {
        //        Debug.Assert(stDimSet > 1);
        //        Debug.Assert(vftCurrentPointSet.Count);
        //        Debug.Assert(vftCurrentPointSet.Count == vftCurrentDirectionSet.Count);
        //        vftCurrentPoint = vftCurrentPointSet;
        //        vftCurrentDirection = vftCurrentDirectionSet;
        //        stNumDimensions = stDimSet;
        //    }

        //    //Evaluate the multidimensional function by setting points based on the current
        //    //point, current direction, and scalara value and then calling the wrapped function
        //    double operator()(const double ftScalar) const
        //    {
        //        Debug.Assert(vftCurrentPoint.Count && vftCurrentDirection.Count == vftCurrentPoint.Count);
        //        Debug.Assert(stNumDimensions == vftCurrentPoint.Count);

        //        //The point at which we'll evaluate the actual multidimensional function
        //        List<double> vftEvalPoint(stNumDimensions);
        //        for(int st=0; st < stNumDimensions; ++st)
        //        {
        //            vftEvalPoint[st] = vftCurrentPoint[st] + ftScalar * vftCurrentDirection[st];
        //        }
        //        double ftRet = MultiDimFuncType()(vftEvalPoint);

        //        #if DEBUG
        //        Trace.Write("OneDAdapter::operator(): evaluating function at point ");
        //        for(int st=0; st < stNumDimensions; ++st)
        //        {
        //            Trace.Write(vftEvalPoint[st] + " ");
        //        }
        //        Trace.WriteLine("");
        //        Trace.Write("Current point: ");
        //        for(int st=0; st < stNumDimensions; ++st)
        //        {
        //            Trace.Write(vftCurrentPoint[st] + " ";
        //        }
        //        Trace.WriteLine("");
        //        Trace.Write("Current direction: ");
        //        for(int st=0; st < stNumDimensions; ++st)
        //        {
        //            Trace.Write(vftCurrentDirection[st] + " ");
        //        }
        //        Trace.WriteLine("");
        //        Trace.WriteLine("Scalar: " + ftScalar);
        //        Trace.WriteLine("Result: " + ftRet);
        //        #endif

        //        return ftRet;
        //    }

        //    typedef double MultiDimFuncType;
        //    //The number of dimensions in the multidimensional fn that we're wrapping

        //    private:


        //    //Current point of evaluation and current direction.
        //    //The caller can set these to define the relationship between the scalar that will actually be
        //    //evaluated and the multidimensional function we're wrapping.

        //    //Note that this could get especially nasty in a multithreaded situation.

        //    List<double> vftCurrentPoint;
        //    List<double> vftCurrentDirection;
        //    int stNumDimensions;


        //}


        //Minimize multidimensional function double along direction vftDirection, starting from point vftCurr,
        //by using FuncMinBrent
        //ftInitialPoint1 and ftInitialPoint2 are initial guesses for the bracketing of the minimum
        //double is the floating point type, and double is multidimensional function to be minimized.
        static double MinimizeAlong1Dimension(Converter<List<double>, double> FN, List<double> vftCurr, List<double> vftDirection,
            double ftInitialPoint1, double ftInitialPoint2, double ftTol)
        {
            Debug.Assert(vftCurr.Count > 0 && vftDirection.Count == vftCurr.Count);
            Debug.Assert(ftInitialPoint1 != ftInitialPoint2);

            double ftA = ftInitialPoint1, ftB = ftInitialPoint2, ftC = 0, ftFNOfA = 0, ftFNOfB = 0, ftFNOfC = 0;

            //Set the current point and direction
            //Singleton<OneDAdapter<double, double> >::Instance().SetState(vftCurr, vftDirection, double::stNumDimensions);
            Converter<double, double> fn1d = OneDAdapter(FN, vftCurr, vftDirection);

            //Get an initial bracketing of a minimum
            InitialBracket(ref ftA, ref ftB, out ftC, out ftFNOfA, out ftFNOfB, out ftFNOfC, fn1d);

            Debug.Assert(ftA < ftB && ftB < ftC);
            Debug.Assert(ftFNOfA >= ftFNOfB && ftFNOfC >= ftFNOfB);

            //Now minimize the function. The OneDAdapter trick turns the scalar minimization into
            //a multidimensional minimization instead
            FuncMinBrent(ftA, ref ftB, ftC, ftFNOfA, ref ftFNOfB, ftFNOfC, ftTol, fn1d);

            for (int st = 0; st < vftCurr.Count; ++st)
            {
                vftCurr[st] += vftDirection[st] * ftB;
            }

            return ftFNOfB;
        }



        //TODO: This REALLY needs to be fixed up somehow - way too many parameters!!!
        //Replace one point with another, updating vftValues and vftPointSum
        static void DoPointReplacement(int index, List<double> new_point, double new_point_val, List<double> vftValues,
                List<double> vftPointSum, int stNumDimensions, List<List<double>> vvftPoints, double ftMin, int itMin, double ftMax, int itMax,
                double ftMaxButOne, int itMaxButOne)
        {

#if DEBUG
            List<double> vftPointSumCheck = new List<double>(stNumDimensions);
            for (int itCurrDim = 0; itCurrDim < stNumDimensions; ++itCurrDim)
            {
                vftPointSumCheck.Add(0.0);
                for (int itCurrPoint = 0; itCurrPoint < vvftPoints.Count; ++itCurrPoint)
                {
                    vftPointSumCheck[itCurrDim] += vvftPoints[itCurrPoint][itCurrDim];
                }
                Debug.Assert(Math.Abs(vftPointSumCheck[itCurrDim] - vftPointSum[itCurrDim]) < 0.01);
            }
#endif

            vftValues[index] = new_point_val;
            for (int itDim = 0; itDim < stNumDimensions; ++itDim)
            {
                vftPointSum[itDim] -= vvftPoints[index][itDim];
                vftPointSum[itDim] += new_point[itDim];
                vvftPoints[index][itDim] = new_point[itDim];
            }
            /*
            if(new_point_val < ftMin)
            {
                ftMin = new_point_val;
                itMin = index;
            }
            else if(new_point_val > ftMax)
            {
                ftMaxButOne = ftMax;
                itMaxButOne = itMax;
                ftMax = new_point_val;
                itMax = index;
            }
            else if(new_point_val > ftMaxButOne)
            {
                ftMaxButOne = new_point_val;
                itMaxButOne = index;
            }
            */
#if DEBUG
            //vftPointSumCheck.resize(stNumDimensions, 0);
            Debug.Assert(vftPointSumCheck.Count == stNumDimensions);
            for (int itCurrDim = 0; itCurrDim < stNumDimensions; ++itCurrDim)
            {
                vftPointSumCheck[itCurrDim] = 0;
                for (int itCurrPoint = 0; itCurrPoint < vvftPoints.Count; ++itCurrPoint)
                {
                    vftPointSumCheck[itCurrDim] += vvftPoints[itCurrPoint][itCurrDim];
                }
                Debug.Assert(Math.Abs(vftPointSumCheck[itCurrDim] - vftPointSum[itCurrDim]) < 0.001);
            }
#endif

        }

        static void ResetMaxMin(int stNumPoints, List<double> vftValues, Converter<List<double>, double> fn, List<List<double>> vvftPoints, int itMin, double ftMin, int itMax, double ftMax, int itMaxButOne, double ftMaxButOne, int stNumDimensions, List<double> vftPointSum)
        {
            ftMin = double.MaxValue;
            itMin = 0;
            ftMax = double.MinValue;
            itMax = 0;
            ftMaxButOne = double.MinValue;
            itMaxButOne = 0;
            itMin = itMax = itMaxButOne = 0;
            for (int itCurrPoint = 0; itCurrPoint < stNumPoints; ++itCurrPoint)
            {
                vftValues[itCurrPoint] = fn(vvftPoints[itCurrPoint]);
                if (vftValues[itCurrPoint] < ftMin)
                {
                    itMin = itCurrPoint;
                    ftMin = vftValues[itCurrPoint];
                }
                if (vftValues[itCurrPoint] > ftMaxButOne)
                {
                    if (vftValues[itCurrPoint] > ftMax)
                    {
                        ftMaxButOne = ftMax;
                        itMaxButOne = itMax;

                        ftMax = vftValues[itCurrPoint];
                        itMax = itCurrPoint;
                    }
                    else
                    {
                        ftMaxButOne = vftValues[itCurrPoint];
                        itMaxButOne = itCurrPoint;
                    }

                }
                //Debug.Assert(ftMin <= ftMaxButOne && ftMaxButOne <= ftMax);

            }

        }

        static void SetPointSum(int stNumDimensions, int stNumPoints, ref List<double> vftPointSum, List<List<double>> vvftPoints)
        {
            for (int itCurrDim = 0; itCurrDim < stNumDimensions; ++itCurrDim)
            {
                vftPointSum[itCurrDim] = 0;
                for (int itCurrPoint = 0; itCurrPoint < stNumPoints; ++itCurrPoint)
                {
                    vftPointSum[itCurrDim] += vvftPoints[itCurrPoint][itCurrDim];
                }
            }
#if DEBUG
            List<double> vftPointSumCheck = new List<double>(stNumDimensions);
            for (int itCurrDim = 0; itCurrDim < stNumDimensions; ++itCurrDim)
            {
                vftPointSumCheck.Add(0.0);
                for (int itCurrPoint = 0; itCurrPoint < stNumPoints; ++itCurrPoint)
                {
                    vftPointSumCheck[itCurrDim] += vvftPoints[itCurrPoint][itCurrDim];
                }
                Debug.Assert(vftPointSumCheck[itCurrDim] == vftPointSum[itCurrDim]);
            }
#endif

        }

    }
}
