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

using System.Collections.Generic;
using System.Linq;
//using ShoNS;
using ShoNS.Array;


namespace MBT.Escience.Sho
{
    public class CrossValidation
    {
        public List<List<int>> _trainInds = new List<List<int>>();
        public List<List<int>> _testInds = new List<List<int>>();
        public int _numFolds;

        /// <summary>
        /// Given a one-d array of target labels, create stratified cross-validation train/testing groups.
        /// This version does it systematically the same way and does not randomize (provided the targetLabels are given
        ///in the same order).
        /// </summary>
        /// <param name="targetLabels"></param>
        public CrossValidation(DoubleArray targetLabels, int numFolds)
        {
            _numFolds = numFolds;
            IEnumerable<double> distinctVals = targetLabels.Distinct();
            int numDistinictTarg = distinctVals.Count();

            //make sure it is always a row array
            //targetLabels = targetLabels.ToVector();
            targetLabels = targetLabels.ToVector();    // !!!JC: I think this is the conversion

            //initialize the lists
            for (int fold = 0; fold < numFolds; fold++)
            {
                _testInds.Add(new List<int>());
                _trainInds.Add(new List<int>());
            }
            //fill up the lists
            int currentFold = 0;
            foreach (double val in distinctVals)
            {
                IntArray hasVal = targetLabels.ElementEQ(val);

                for (int r = 0; r < hasVal.size0; r++)
                {
                    for (int c = 0; c < hasVal.size1; c++)
                    {

                        {
                            if (hasVal[r, c] == 1)
                            {
                                //System.Console.WriteLine("index: " + c + " with label " + val + " is added to fold: " + currentFold % numFolds);
                                _testInds[currentFold % numFolds].Add(c);
                                currentFold++;
                            }
                        }
                    }
                }
            }

            for (int testFold = 0; testFold < numFolds; testFold++)
            {
                for (int fold = 0; fold < numFolds; fold++)
                {
                    if (testFold != fold)
                    {
                        foreach (int indVal in _testInds[fold])
                            //_trainInds[fold].Add(indVal);
                            _trainInds[testFold].Add(indVal);
                    }
                }
            }
        }
    }
}
