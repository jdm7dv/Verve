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
using System.Collections;
using System.Collections.Generic;
using Bio.Util;

namespace MBT.Escience
{
    public class NoisyAdd
    {
        public enum StateIndex
        {
            Negative = 0, Zero, Positive
        };

#if false
        private static LazyInit<Cache<StatisticsList, NoisyAdd>> _lazyCachedStats = new LazyInit<Cache<StatisticsList, NoisyAdd>>(
            () => new Cache<StatisticsList, NoisyAdd>(1000, 10), LazyInitMode.ThreadLocal);

        private static Cache<StatisticsList, NoisyAdd> _cachedStats
        {
            get { return _lazyCachedStats.Value; }
        }
#else
        //private static Cache<StatisticsList, NoisyAdd> _cachedStats = new Cache<StatisticsList, NoisyAdd>(1000, 10);
#endif

        private List<TermSumNode> _predictorNodes;
        private SumNode _rootSumNode;
        private bool _logspace;

        public bool IsEmpty
        {
            get { return _predictorNodes.Count == 0; }
        }

        public static NoisyAdd GetInstance(StatisticsList positiveAndNegativeStats)
        {
            NoisyAdd noisyAdd = new NoisyAdd(positiveAndNegativeStats);
            //if (!_cachedStats.TryGetValue(positiveAndNegativeStats, out noisyAdd))
            //{
            //    noisyAdd = new NoisyAdd(positiveAndNegativeStats);
            //    _cachedStats.Add(positiveAndNegativeStats, noisyAdd);
            //}
            return noisyAdd;
        }

        private NoisyAdd(StatisticsList positiveAndNegativeStats)
        {
            //Helper.CheckCondition(positiveAndNegativeStats.Count > 0, "NoisyAdd with no nodes is kinda silly.");

            _predictorNodes = new List<TermSumNode>(positiveAndNegativeStats.Count);
            foreach (SufficientStatistics stat in positiveAndNegativeStats)
            {
                int sign = stat.AsDiscreteStatistics();
                if (sign < 0)
                {
                    _predictorNodes.Add(TermSumNode.GetNegativeInstance(0));
                }
                else if (sign > 0)
                {
                    _predictorNodes.Add(TermSumNode.GetPositiveInstance(0));
                }
                else
                {
                    _predictorNodes.Add(TermSumNode.GetNullInstance());
                }
            }

            if (_predictorNodes.Count == 1)
            {
                _rootSumNode = _predictorNodes[0];
            }
            else if (_predictorNodes.Count > 1)
            {
                _rootSumNode = new SumNode(_predictorNodes[0], _predictorNodes[1]);
                for (int i = 2; i < _predictorNodes.Count; i++)
                {
                    _rootSumNode = new SumNode(_predictorNodes[i], _rootSumNode);
                }
            }
        }

        public void SetPriors(List<double> posOrNegProbsInOrderOfStructure, bool convertToLog)
        {
            Helper.CheckCondition(posOrNegProbsInOrderOfStructure.Count == _predictorNodes.Count, "Different number of probs supplied that the number of nodes in the model.");
            for (int i = 0; i < posOrNegProbsInOrderOfStructure.Count; i++)
            {
                double p = posOrNegProbsInOrderOfStructure[i];
                double absP = Math.Abs(p);
                _predictorNodes[i].PTrue = convertToLog ? Math.Log(absP) : absP;
            }
            _logspace = convertToLog;
        }

        public double[] ComputeNegZeroPlus()
        {
            if (!_logspace)
                return IsEmpty ? new double[] { 0, 1, 0 } : _rootSumNode.ComputeMinusZeroPlusProbs(_logspace);
            else
                return IsEmpty ? new double[] { double.NegativeInfinity, 0, double.NegativeInfinity } : _rootSumNode.ComputeMinusZeroPlusProbs(_logspace);
        }

        public List<double> InferPredictorNodeValuesGivenNegZeroPosLikelihood(double[] negZeroPosLikelihood)
        {
            if (!IsEmpty)
            {
                _rootSumNode.RunInference(negZeroPosLikelihood, _logspace);
                List<double> inferenceResults = new List<double>(_predictorNodes.Count);
                foreach (TermSumNode termNode in _predictorNodes)
                {
                    inferenceResults.Add(termNode.LikelihoodTrue);
                }
                return inferenceResults;
            }
            else
            {
                return new List<double>(0);
            }
        }

        public static double[] ComputeDistribution(List<double> probNegOrZero, List<double> probPosOrZero)
        {
            double[] cumProbs;

            // reduces to NoisyOr if one of the prob arrays is empty.
            if (probPosOrZero.Count == 0)
            {
                double pNeg = SpecialFunctions.NoisyOr(probNegOrZero);
                cumProbs = new double[] { pNeg, 1 - pNeg, 0 };
            }
            else if (probNegOrZero.Count == 0)
            {
                double pPos = SpecialFunctions.NoisyOr(probPosOrZero);
                cumProbs = new double[] { 0, 1 - pPos, pPos };
            }
            else
            {
                cumProbs = ComputeStateIndexProbs(EnumerateAllNetStatesAndProbs(probNegOrZero, probPosOrZero));
            }

            return cumProbs;
        }

        public static double[] ComputeStateIndexProbs(IEnumerable<Tuple<StateIndex, double>> netMutationsAndProbs)
        {
            double[] cumProbs = new double[3];

            foreach (Tuple<StateIndex, double> netMutationAndProb in netMutationsAndProbs)
            {
                cumProbs[(int)netMutationAndProb.Item1] += netMutationAndProb.Item2;
            }
            return cumProbs;
        }

        public static IEnumerable<Tuple<StateIndex, double>> EnumerateAllNetStatesAndProbs(List<double> probNegOrZero, List<double> probPosOrZero)
        {
            //List<Tuple<int, double>> mutAwaySumAndProbs = new List<Tuple<int, double>>(ComputeSumToProb(probNegOrZero));
            double[] sumToProbNeg = ComputeSumToProb(probNegOrZero);
            double[] sumToProbPos = ComputeSumToProb(probPosOrZero);

            for (int posSum = 0; posSum < sumToProbPos.Length; posSum++)
            {
                for (int negSum = 0; negSum < sumToProbNeg.Length; negSum++)
                {
                    int net = posSum - negSum;
                    StateIndex mutationDirection = (StateIndex)(Math.Sign(net) + 1);
                    double prob = sumToProbPos[posSum] * sumToProbNeg[negSum];

                    yield return Tuple.Create(mutationDirection, prob);
                }
            }
        }

        public static double[] ComputeSumToProb(List<double> probs)
        {
            int itemCount = probs.Count;
            double[] sumToProb = new double[itemCount + 1];
            int enumCount = (int)Math.Pow(2, itemCount);
            double[] workList = probs.ToArray();
            int sum;
            double p;

            BitArray bitArr = new BitArray(itemCount, false);

            for (int i = 0; i < enumCount; i++)
            {
                ComputeSumAndProduct(probs, bitArr, out sum, out p);
                //yield return Tuple.Create(sum, p);
                sumToProb[sum] += p;
                SpecialFunctions.IncrementBitArray(bitArr);
            }
            return sumToProb;
        }

        public static void ComputeSumAndProduct(List<double> probs, BitArray bitArr, out int sum, out double p)
        {
            int itemCount = probs.Count;
            sum = 0;
            p = 1;
            for (int i = 0; i < itemCount; i++)
            {
                if (bitArr[i])
                {
                    p *= 1 - probs[i];
                }
                else
                {
                    p *= probs[i];
                    sum++;
                }
            }
        }




        private class SumNode : IEnumerable<KeyValuePair<int, double>>
        {
            readonly int _min, _max;
            readonly double[] _pSum;
            readonly double[] _likelihoodSum;
            readonly double[] _lastPNegZeroPos;
            readonly SumNode _in1, _in2;

            public double this[int sum]
            {
                get
                {
                    return _pSum[sum - _min];
                }
                set
                {
                    _pSum[sum - _min] = value;
                }
            }

            protected SumNode(int min, int max)
            {
                _min = min;
                _max = max;
                _in1 = null;
                _in2 = null;
                _pSum = new double[_max - _min + 1];
                _likelihoodSum = new double[_pSum.Length];
                _lastPNegZeroPos = new double[3];
            }

            public SumNode(SumNode in1, SumNode in2)
            {
                _min = in1._min + in2._min;
                _max = in1._max + in2._max;
                _in1 = in1;
                _in2 = in2;
                _pSum = new double[_max - _min + 1];
                _likelihoodSum = new double[_pSum.Length];
                _lastPNegZeroPos = new double[3];
            }


            public double[] ComputeMinusZeroPlusProbs(bool logspace)
            {
                if (UpdateProbs(logspace))
                {
                    if (logspace)
                        SpecialFunctions.SetAllValues(_lastPNegZeroPos, double.NegativeInfinity);
                    else
                        Array.Clear(_lastPNegZeroPos, 0, 3);

                    for (int sum = _min; sum <= _max; sum++)
                    {
                        int idx = Math.Sign(sum) + 1;
                        if (logspace)
                            _lastPNegZeroPos[idx] = SpecialFunctions.LogSum(_lastPNegZeroPos[idx], this[sum]);
                        else
                            _lastPNegZeroPos[idx] += this[sum];
                    }
                }

                double[] minusZeroPlus = new double[3];
                Array.Copy(_lastPNegZeroPos, minusZeroPlus, 3);

                return minusZeroPlus;
            }

            public void RunInference(double[] negZeroPosLikelihood, bool logspace)
            {
                //UpdateProbs();
                double[] negZeroPosProbs = ComputeMinusZeroPlusProbs(logspace);
                for (int sum = _min; sum <= _max; sum++)
                {
                    double pHat = negZeroPosLikelihood[Math.Sign(sum) + 1];
                    double p = negZeroPosProbs[Math.Sign(sum) + 1];
                    _likelihoodSum[sum - _min] = logspace ?
                        double.IsNegativeInfinity(p) ? p : pHat + this[sum] - p :
                        SpecialFunctions.SafeDivide(pHat * this[sum], p, 1e-12);  // def 0/0 == 0
                }
                PropogateLikelihoodChange(logspace);
            }


            protected virtual bool UpdateProbs(bool logspace)
            {
                bool updated1 = _in1.UpdateProbs(logspace);
                bool updated2 = _in2.UpdateProbs(logspace);

                if (updated1 || updated2)
                {
                    if (logspace)
                        SpecialFunctions.SetAllValues(_pSum, double.NegativeInfinity);
                    else
                        Array.Clear(_pSum, 0, _pSum.Length);

                    foreach (KeyValuePair<int, double> pSum1 in _in1)
                    {
                        foreach (KeyValuePair<int, double> pSum2 in _in2)
                        {
                            int sum = pSum1.Key + pSum2.Key;
                            if (logspace)
                            {
                                double p = pSum1.Value + pSum2.Value;
                                this[sum] = SpecialFunctions.LogSum(this[sum], p);
                            }
                            else
                            {
                                double p = pSum1.Value * pSum2.Value;
                                this[sum] += p;
                            }
                        }
                    }
                    //double testSum = 0;
                    //foreach (double p in _pSum)
                    //{
                    //    testSum += p;
                    //}
                    //Helper.CheckCondition(ComplexNumber.ApproxEqual(testSum, 1, 1e-9));
                    return true;
                }
                return false;
            }


            protected virtual void PropogateLikelihoodChange(bool logspace)
            {
                //throw new NotImplementedException("This needs work. At the end, the likelihoods sum to more than 1.");
                if (logspace)
                {
                    SpecialFunctions.SetAllValues(_in1._likelihoodSum, double.NegativeInfinity);
                    SpecialFunctions.SetAllValues(_in2._likelihoodSum, double.NegativeInfinity);
                }
                else
                {
                    Array.Clear(_in1._likelihoodSum, 0, _in1._likelihoodSum.Length);
                    Array.Clear(_in2._likelihoodSum, 0, _in2._likelihoodSum.Length);
                }

                foreach (KeyValuePair<int, double> pSum1 in _in1)
                {
                    foreach (KeyValuePair<int, double> pSum2 in _in2)
                    {
                        int sum = pSum1.Key + pSum2.Key;

                        if (logspace)
                        {
                            double p = pSum1.Value + pSum2.Value;
                            double pCond = double.IsNegativeInfinity(p) ? p : p - this[sum];
                            double likelihood = pCond + _likelihoodSum[sum - _min];

                            _in1._likelihoodSum[pSum1.Key - _in1._min] = SpecialFunctions.LogSum(_in1._likelihoodSum[pSum1.Key - _in1._min], likelihood);
                            _in2._likelihoodSum[pSum2.Key - _in2._min] = SpecialFunctions.LogSum(_in2._likelihoodSum[pSum2.Key - _in2._min], likelihood);
                        }
                        else
                        {
                            double p = pSum1.Value * pSum2.Value;
                            double pCond = p == 0 ? 0 : p / this[sum];
                            double likelihood = pCond * _likelihoodSum[sum - _min];

                            _in1._likelihoodSum[pSum1.Key - _in1._min] += likelihood;
                            _in2._likelihoodSum[pSum2.Key - _in2._min] += likelihood;
                        }
                    }
                }
                _in1.PropogateLikelihoodChange(logspace);
                _in2.PropogateLikelihoodChange(logspace);
            }

            protected double GetLikelihood(int sum)
            {
                return _likelihoodSum[sum - _min];
            }

            #region IEnumerable members
            public IEnumerator<KeyValuePair<int, double>> GetEnumerator()
            {
                for (int sum = _min; sum <= _max; sum++)
                {
                    yield return new KeyValuePair<int, double>(sum, this[sum]);
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            #endregion
        }

        private class TermSumNode : SumNode
        {
            int _oneOrMinusOne;
            bool _valueChangedSinceLastUpdate;

            protected TermSumNode(int oneOrMinusOne, double p)
                : base(Math.Min(oneOrMinusOne, 0), Math.Max(oneOrMinusOne, 0))
            {
                _oneOrMinusOne = oneOrMinusOne;
                PTrue = p;
                _valueChangedSinceLastUpdate = true;
            }

            public virtual double PTrue
            {
                get { return this[_oneOrMinusOne]; }
                set
                {
                    _valueChangedSinceLastUpdate |= this[_oneOrMinusOne] != value;   // sticky reminder that the value has changed
                    this[_oneOrMinusOne] = value;
                    this[0] = 1 - value;
                }
            }

            public double LikelihoodTrue
            {
                get { return GetLikelihood(_oneOrMinusOne); }
            }

            public static TermSumNode GetPositiveInstance(double pTrue)
            {
                return new TermSumNode(1, pTrue);
            }

            public static TermSumNode GetNegativeInstance(double pTrue)
            {
                return new TermSumNode(-1, pTrue);
            }

            public static TermSumNode GetNullInstance()
            {
                return NullInstance.GetInstance();
            }

            protected override bool UpdateProbs(bool logspace)
            {
                bool updated = _valueChangedSinceLastUpdate;
                _valueChangedSinceLastUpdate = false;
                return updated;
            }

            protected override void PropogateLikelihoodChange(bool logspace)
            {
                // do nothing. our likelihood was already updated by child sum node
            }

            public override string ToString()
            {
                return (_oneOrMinusOne * PTrue).ToString();
            }

            private class NullInstance : TermSumNode
            {
                private NullInstance() : base(0, 1) { }
                public static NullInstance GetInstance()
                {
                    return new NullInstance();
                }

                public override double PTrue
                {
                    get
                    {
                        return base.PTrue;
                    }
                    set
                    {
                        _valueChangedSinceLastUpdate |= this[_oneOrMinusOne] != 1;   // sticky reminder that the value has changed
                        this[0] = 1;
                    }
                }
            }
        }
    }
}
