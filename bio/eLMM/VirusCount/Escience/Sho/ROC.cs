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
using MBT.Escience;
using Bio.Util;
using ShoNS.Array;
using System.Threading;
using System.Threading.Tasks;


namespace MBT.Escience.Sho
{
    public class ROC
    {

        public DoubleArray _classLabels;
        public DoubleArray _classProbs;
        public DoubleArray _probThresholds;

        public DoubleArray _numFalsePositives;
        public DoubleArray _numFalseNegatives;
        public DoubleArray _numTruePositives;

        public DoubleArray _falsePositiveRate;
        public DoubleArray _falseNegativeRate;
        public DoubleArray _truePositiveRate;

        public bool _lowerScoreIsMoreLikelyClass1 = false;

        //public IntArray _sortInd;
        public int _numNegatives;
        public int _numPositives;

        public int _numCases;
        public double _AUC;
        //public Dictionary<double, double> _AUCpartial = new Dictionary<double, double>();

        //for partial AUCs
        public double _maxFPR = 1.0;
        public double _AucAtMaxFpr;

        private static double POS_LABEL = 1;
        private static double NEG_LABEL = 0;


        public ROC(DoubleArray classLabels, DoubleArray classProbs)
            : this(classLabels, classProbs, false, 1)
        {
        }

        public ROC(DoubleArray classLabels, DoubleArray classProbs, bool lowerScoreIsMoreLikelyClass1, double maxFPR)
            : this(classLabels, classProbs, lowerScoreIsMoreLikelyClass1, maxFPR, false)
        {
        }

        /// <summary>
        /// Compute ROC statistics using specified thresholds 
        /// </summary>
        /// <param name="classLabels">Must be in the set {0,1}, denoting class 0 and class 1</param>
        /// <param name="classProbs">Probabilitiy that instance belongs to class 1 (label=1)</param>
        public ROC(DoubleArray classLabels, DoubleArray classProbs, bool lowerScoreIsMoreLikelyClass1, double maxFPR, bool classLabelsAlreadySorted)//, DoubleArray thresholds)
        {
            Helper.CheckCondition(classProbs.Max() <= 1 && classProbs.Min() >= 0, "classProbs must be between 0 and 1");

            _maxFPR = maxFPR;

            _lowerScoreIsMoreLikelyClass1 = lowerScoreIsMoreLikelyClass1;
            if (_lowerScoreIsMoreLikelyClass1)
            {
                classProbs = 1 - classProbs;
            }
            Helper.CheckCondition(classLabels.IsBinaryArray(), "targets are not exclusively 0/1");

            if (classLabels.Length == 0 || classLabels.Length != classProbs.Length)
            {
                throw new Exception("inputs are not valid");
            }

            //want to sort by class probs:
            if (!classLabelsAlreadySorted)
            {
                IntArray sortInd;
                //order from smallest to largest on the classProbs
                classProbs = classProbs.SortWithInd(out sortInd);
                //Helper.CheckCondition(thresholds.InNonDecreasingOrder(), "thresholds must be in increasing order");
                classLabels = classLabels.ToVector().GetCols(sortInd);
            }
            else
            {
                //will catch this when computing AUC instead
                //Helper.CheckCondition(classProbs.IsSorted(), "expected class probs to be sorted");
            }

            _classProbs = classProbs;
            _classLabels = DoubleArray.From(classLabels);

            _numCases = _classLabels.Length;

            _numNegatives = _classLabels.ElementEQ(NEG_LABEL).Sum();
            _numPositives = _classLabels.ElementEQ(POS_LABEL).Sum();

            //System.Console.WriteLine("ROC constructor: _numNegatives=" + _numNegatives + ", numPositives=" + _numPositives + ", classLabels.Sum=" + _classLabels.Sum());

            int numUniqueProbs = classProbs.GetUniqueValues().Length;//number  distinct threshold we will use

            //call everyhing negative at the beginning
            _numFalsePositives = new DoubleArray(1, numUniqueProbs + 1);
            _numFalseNegatives = new DoubleArray(1, numUniqueProbs + 1);
            _numTruePositives = new DoubleArray(1, numUniqueProbs + 1);
            _probThresholds = new DoubleArray(1, numUniqueProbs + 1);

            /*iterate through, each time updating the counts based on the previous threshold used. 
            need to be careful because of ties--this makes it NOT an on-line algorithm (which is how Carl does it)*/
            int currentCaseInd = 0;
            int currentThreshInd = 0;

            // when we call all things positive, then increasingly less and less later on
            _numFalsePositives[currentThreshInd] = _numNegatives;
            _numFalseNegatives[currentThreshInd] = 0;
            _numTruePositives[currentThreshInd] = _numPositives;
            _probThresholds[currentThreshInd] = 0;
            currentThreshInd++;

            //step through the cases to get all the thresholds
            double currentThresh = _classProbs[currentCaseInd];
            while (currentCaseInd < _numCases)
            {
                if (_classProbs[currentCaseInd] == currentThresh)
                {
                    //same threshold as previous case, so aggregate results
                    if (_classLabels[currentCaseInd] == POS_LABEL)
                    {
                        _numTruePositives[currentThreshInd]++;
                    }
                    else
                    {
                        _numFalsePositives[currentThreshInd]++;
                    }
                    currentCaseInd++;
                }
                else //found a new threshold, so update counts and reset 
                {
                    _numTruePositives[currentThreshInd] = _numTruePositives[currentThreshInd - 1] - _numTruePositives[currentThreshInd];
                    _numFalsePositives[currentThreshInd] = _numFalsePositives[currentThreshInd - 1] - _numFalsePositives[currentThreshInd];
                    _numFalseNegatives[currentThreshInd] = _numPositives - _numTruePositives[currentThreshInd];

                    _probThresholds[currentThreshInd] = currentThresh;
                    currentThreshInd++;
                    currentThresh = _classProbs[currentCaseInd];
                }
            }
            //one last time to get the last computations
            _numTruePositives[currentThreshInd] = _numTruePositives[currentThreshInd - 1] - _numTruePositives[currentThreshInd];
            _numFalsePositives[currentThreshInd] = _numFalsePositives[currentThreshInd - 1] - _numFalsePositives[currentThreshInd];
            _numFalseNegatives[currentThreshInd] = _numPositives - _numTruePositives[currentThreshInd];
            _probThresholds[currentThreshInd] = currentThresh;

            //compute the rates from the raw counts
            _falsePositiveRate = _numFalsePositives.Divide(_numNegatives);
            _falseNegativeRate = _numFalseNegatives.Divide(_numPositives);
            _truePositiveRate = _numTruePositives.Divide(_numPositives);

            //now compute the AUC of this, using the correct way of integrating (shortcut methods do not account for ties)
            this.ComputeAUC();
        }

        /// <summary>
        /// Sum the area under the discrete curve. (Do this literally as the area under the curve, not with the shortcuts
        /// which don't account for ties---this can be CRITICAL). If you don't care about ties, use the faster ComputeAUCfromOrderedList().
        /// Note this more expensive method also allows you to compute partial AUCs quite easily.
        /// </summary>
        public void ComputeAUC()
        {
            //translated line-by-line from my Matlab code
            double total = 0;
            double partialAucTotal = 0;

            //using x and y so I can more easily copy my Matlab code
            DoubleArray x = _falsePositiveRate;//descending
            //DoubleArray y = _falseNegativeRate;//increasing
            DoubleArray y = _truePositiveRate;//descending

            //number of vertical chunks whos area we will add up (by taking the midpoint height of the chunk
            int numChunks = x.Length - 1;
            //double minYVal = 0;//in case want to use just area under part of the ROC, often done in bio

            for (int ch = 0; ch < numChunks; ch++)
            {
                int ch1 = ch;
                int ch2 = ch + 1;
                double width = x[ch1] - x[ch2];
                Helper.CheckCondition(width >= 0, "false positive rate is not sorted, but is expected to be");
                Helper.CheckCondition(y[ch1] - y[ch2] >= 0, "false negative rate is not sorted, but is expected to be");
                double height = (y[ch1] + y[ch2]) / 2;// - minYVal;
                double newAreaToAdd = width * height;
                total += newAreaToAdd;

                if (x[ch1] < _maxFPR)
                {
                    partialAucTotal += newAreaToAdd;
                }
            }

            _AUC = total;
            _AucAtMaxFpr = partialAucTotal;

            //_AUC = 1 - total;
            //_AucAtMaxFpr = 1 - partialAucTotal;
            Helper.CheckCondition(_AucAtMaxFpr <= _AUC);
            //_AUC = total;
        }

        /// <summary>
        /// COmpute the AUC in a quick way which doesn not account for data that has ties
        /// in the probability of items with different class labels. This is fine for use
        /// in permutation tests since it falls out in the wash.
        /// </summary>
        /// <param name="orderedList">list or 0/1 target labels in order of class 1 posterior</param>
        /// <returns></returns>
        public static double ComputeAUCfromOrderedList(DoubleArray orderedList)
        {
            double N = (double)orderedList.Length;
            double numPos = orderedList.Sum();
            IntArray tmp = 1 + orderedList.Find(v => v == POS_LABEL);
            //note that need to add 1 to get rank because C# is 0-based;
            double meanRankPosClass = (double)(tmp.Sum()) / numPos;
            double AUC = (meanRankPosClass - (numPos + 1) / 2) / (N - numPos);
            return AUC;
            //return 1 - AUC;
        }

        public static double ROCswapPermTest(ROC roc1, ROC roc2, int numTrial, ParallelOptions parallelOptions)
        {
            return ROCswapPermTest(roc1, roc2, numTrial, 1, parallelOptions);
        }

        /// <summary>
        ///  pVal is the probability that we would observe as big an AUC diff as we
        /// did if the ROC curves were drawn from the null hypothesis (which is that 
        /// one model does not perform better than the other)
        ///
        /// think of it this way: we want a null distribution which says that there
        /// is no difference between the ROCs. Since an AUC difference
        /// cannot arise from any entries in the ordered lists that match up, we can
        /// ignore these (though we could include them as well, but it would fall out
        /// in the wash). So instead, we assume (know) that all the information in the 
        /// differences in AUCs is contained in the mismatched pairs, and we want to
        /// destroy this info for the null, so we swap the values between the two
        /// models. However, we want to keep the number of positive/negative samples
        /// the same, so when we swap one pair, we must also swap another in the
        /// other direction.
        ///
        /// Note that in this method, we use a fast AUC computation which ignores ties.
        /// This is fine for random experiemnts since the ties will fall out in the wash.
        /// </summary>
        /// <param name="roc1"></param>
        /// <param name="roc2"></param>
        /// <returns>pValue</returns>
        public static double ROCswapPermTest(ROC roc1, ROC roc2, int numTrial, double maxFPR, ParallelOptions parallelOptions)
        {
            string randomStringSeed = "78923";//"123456";

            Helper.CheckCondition(roc1._lowerScoreIsMoreLikelyClass1 == roc2._lowerScoreIsMoreLikelyClass1);

            double realAUCdiff = Math.Abs(roc1._AUC - roc2._AUC);
            DoubleArray permDiffs = new DoubleArray(1, numTrial);

            //to make it the same as my matlab code, rename these:
            DoubleArray orderedTargets1 = DoubleArray.From(roc1._classLabels);
            DoubleArray orderedTargets2 = DoubleArray.From(roc2._classLabels);

            //orderedTargets1.WriteToCSVNoDate("orderedTargets1");
            //orderedTargets2.WriteToCSVNoDate("orderedTargets2");                    

            if (orderedTargets1.Length == 0) throw new Exception("empty ROCs given as input");

            DoubleArray targetDiff = orderedTargets1 - orderedTargets2;
            IntArray posDiffInd = targetDiff.Find(v => v > 0);
            IntArray negDiffInd = targetDiff.Find(v => v < 0);

            int numPos = posDiffInd.Length;
            int numNeg = negDiffInd.Length;
            //Helper.CheckCondition(Math.Abs(numPos - numNeg) <= 1, "don't think this should happen when non-truncated ROCs are used");

            double pVal;
            if (numPos == 0 || numNeg == 0)
            {//ROCs are identical
                pVal = 1;
                return pVal;
            }

            //bug checking:
            int numPos1 = roc1._classLabels.ElementEQ(POS_LABEL).Sum();
            int numNeg1 = roc1._classLabels.ElementEQ(NEG_LABEL).Sum();
            int numPos2 = roc2._classLabels.ElementEQ(POS_LABEL).Sum();
            int numNeg2 = roc2._classLabels.ElementEQ(NEG_LABEL).Sum();

            //these won't be true if we're using a truncated ROC test
            //Helper.CheckCondition(numPos1 == numPos2, "rocs must correspond to the same labelled data (i.e, # of 1/0 must be the same)");
            //Helper.CheckCondition(numNeg1 == numNeg2, "rocs must correspond to the same labelled data (i.e, # of 1/0 must be the same)");

            //for bug checking, keep track of avg number of swaps:
            //DoubleArray numPairedSwaps = new DoubleArray(1, numTrial);
            //DoubleArray auc1tmp= new DoubleArray(1, numTrial);
            //DoubleArray auc2tmp = new DoubleArray(1, numTrial);

            int numPairs = Math.Min(numPos, numNeg);

            //for (int t = 0; t < numTrial; t++)
            Parallel.For(0, numTrial, t =>
            {
                //Helper.CheckCondition((orderedTargets1 - orderedTargets2).Abs().Sum() != 0);

                Random myRand = SpecialFunctions.GetRandFromSeedAndNullIndex(randomStringSeed, t + 1);

                //randomly pair up each positive mismatch with each negative mismatch
                IntArray posIndRand = posDiffInd.RandomPermutation(myRand);
                IntArray negIndRand = negDiffInd.RandomPermutation(myRand);

                //throw new NotImplementedException("Change to GetSlice()");
                IntArray possiblePosPairs = posIndRand.GetColSlice(0, 1, numPairs - 1);//ColSlice(0, 1, numPairs - 1);
                IntArray possibleNegPairs = negIndRand.GetColSlice(0, 1, numPairs - 1); //ColSlice(0, 1, numPairs - 1);
                IntArray possiblePairs = ShoUtils.CatArrayCol(possiblePosPairs, possibleNegPairs).T;
                //Helper.CheckCondition(possiblePairs.size1 == numPairs, "something went wrong");

                //randomly pick each pair with prob=0.5 to include in the swap:
                DoubleArray randVec = (new DoubleArray(1, numPairs)).FillRandUseSeed(myRand);

                IntArray pairsOfPairsToBothSwapInd = randVec.Find(v => v >= 0.5);
                List<int> listInd = pairsOfPairsToBothSwapInd.T.ToListOrEmpty();
                //numPairedSwaps[t] = listInd.Count;

                DoubleArray newTarg1 = DoubleArray.From(orderedTargets1);
                DoubleArray newTarg2 = DoubleArray.From(orderedTargets2);

                if (listInd.Count > 0)
                {
                    //throw new NotImplementedException("Change to GetSlice()");
                    List<int> swapThesePairs = possiblePairs.GetRows(pairsOfPairsToBothSwapInd.T.ToList()).ToVector().ToList();

                    //swap the chosen pairs with a 1-x
                    //Helper.CheckCondition((newTarg1.GetColsE(swapThesePairs) - newTarg2.GetColsE(swapThesePairs)).Abs().Sum() == swapThesePairs.Count); 

                    //throw new NotImplementedException("Change to SetSlice()");
                    newTarg1.SetCols(swapThesePairs, 1 - newTarg1.GetCols(swapThesePairs));//.GetColsE(swapThesePairs));
                    newTarg2.SetCols(swapThesePairs, 1 - newTarg2.GetCols(swapThesePairs));//GetColsE(swapThesePairs));

                    //newTarg1.WriteToCSVNoDate("newTarg1Swapped");
                    //newTarg2.WriteToCSVNoDate("newTarg2Swapped");

                    //Helper.CheckCondition(newTarg1.Sum() == orderedTargets1.Sum());
                    //Helper.CheckCondition((newTarg1 - newTarg2).Abs().Sum() == numPos + numNeg);
                    //Helper.CheckCondition((newTarg1 - newTarg2).Find(v => v != 0).Length == numPos + numNeg);
                    //Helper.CheckCondition((newTarg1 - orderedTargets1).Abs().Sum() == swapThesePairs.Count);
                    //Helper.CheckCondition((newTarg2 - orderedTargets2).Abs().Sum() == swapThesePairs.Count);
                    //Helper.CheckCondition((orderedTargets1 - orderedTargets2).Abs().Sum() != 0);
                }

                double AUC1, AUC2;

                if (maxFPR == 1)
                {
                    //do it the cheap way
                    AUC1 = ComputeAUCfromOrderedList(newTarg1);
                    AUC2 = ComputeAUCfromOrderedList(newTarg2);
                }
                else
                {
                    //do it with manual integration, the more expensive way
                    AUC1 = new ROC(newTarg1, roc1._classProbs, roc1._lowerScoreIsMoreLikelyClass1, maxFPR, true)._AucAtMaxFpr;
                    AUC2 = new ROC(newTarg2, roc2._classProbs, roc2._lowerScoreIsMoreLikelyClass1, maxFPR, true)._AucAtMaxFpr;
                }

                //auc1tmp[t] = AUC1;
                //auc2tmp[t] = AUC2;

                //permDiffs[t] = Math.Abs(AUC1 - AUC2);
                permDiffs[t] = (AUC1 - AUC2);

            }
            );

            //double markerSize = 0.1;
            //ShoUtils.MakePlotAndView(permDiffs, "permDiffs", false, markerSize, ".");
            //ShoUtils.MakePlotAndView(numPairedSwaps, "numPairedSwaps", false, markerSize, "*");
            permDiffs = permDiffs.Map(v => Math.Abs(v));
            //debugging:
            //permDiffs.WriteToCSVNoDate("permDiffs");
            //numPairedSwaps.WriteToCSVNoDate("numPairedSwapsC#");


            double pseudoCount = 1;
            pVal = (pseudoCount + (double)(permDiffs >= realAUCdiff).Sum()) / (double)numTrial;
            pVal = Math.Min(pVal, 1);

            //ShoUtils.MakePlotAndView((auc1tmp-auc2tmp).Map(v=>Math.Abs(v)), "auc1-auc2", false, 0.2, ".");

            //System.Console.WriteLine("Avg # swaps: " + numPairedSwaps.Mean() + " ( of " + numPairs + " total), numGreaterSwaps=" + (double)(permDiffs >= realAUCdiff).Sum() + ", p=" + pVal + ", realAUCdiff=" + String.Format("{0:0.00000}", realAUCdiff));


            return pVal;

        }



    }
}
