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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MBT.Escience.Matrix;
using Bio.Matrix;
using Bio.Util;

namespace MBT.Escience
{
    abstract public class TwoByTwo
    {
        public static string Header = Helper.CreateTabString("var1", "var2", "TT", "TF", "FT", "FF");
        public static string CountsHeader = Helper.CreateTabString("TT", "TF", "FT", "FF");

        public enum ParameterIndex
        {
            TT = 0, TF, FT, FF
        }

        public override string ToString()
        {
            return Helper.CreateTabString(Var1, Var2, Counts[1, 1], Counts[1, 0], Counts[0, 1], Counts[0, 0]);
        }
        public string CountsString()
        {
            return Helper.CreateTabString(Counts[1, 1], Counts[1, 0], Counts[0, 1], Counts[0, 0]);
        }

        public int this[bool var1True, bool var2True]
        {
            get
            {
                return Counts[var1True ? 1 : 0, var2True ? 1 : 0];
            }
            set
            {
                Counts[var1True ? 1 : 0, var2True ? 1 : 0] = value;
                Invalidate();
            }
        }

        private void Invalidate()
        {
            _fisherExactTest = new ThreadLocal<double>(() => SpecialFunctions.FisherExactTest(Counts));
            _bayesScore = new ThreadLocal<double>(() => SpecialFunctions.BayesScore(Counts));
            _chiSquareTest = new ThreadLocal<double>(() => SpecialFunctions.LikelihoodRatioTestOnNByMCounts(Counts));
        }

        public abstract int[,] Counts
        {
            get;
        }

        public int TT
        {
            get { return Counts[1, 1]; }
            set
            {
                Counts[1, 1] = value;
                Invalidate();
            }
        }
        public int TF
        {
            get { return Counts[1, 0]; }
            set
            {
                Counts[1, 0] = value;
                Invalidate();
            }
        }
        public int FT
        {
            get { return Counts[0, 1]; }
            set
            {
                Counts[0, 1] = value;
                Invalidate();
            }
        }
        public int FF
        {
            get { return Counts[0, 0]; }
            set
            {
                Counts[0, 0] = value;
                Invalidate();
            }
        }

        public int[] ToOneDArray()
        {
            int[] fisherCounts = new int[4];
            fisherCounts[(int)TwoByTwo.ParameterIndex.TT] = Counts[1, 1];
            fisherCounts[(int)TwoByTwo.ParameterIndex.TF] = Counts[1, 0];
            fisherCounts[(int)TwoByTwo.ParameterIndex.FT] = Counts[0, 1];
            fisherCounts[(int)TwoByTwo.ParameterIndex.FF] = Counts[0, 0];
            return fisherCounts;
        }

        public string Var1;
        public string Var2;

        ThreadLocal<double> _fisherExactTest;
        public double FisherExactTest
        {
            get
            {
                return _fisherExactTest.Value;
            }
        }

        ThreadLocal<double> _chiSquareTest;

        /// <summary>
        /// Could also be called Likelihood Ratio Test
        /// </summary>
        public double ChiSquareTest
        {
            get
            {
                return _chiSquareTest.Value;
            }
        }


        ThreadLocal<double> _bayesScore;
        public double BayesScore
        {
            get
            {
                return _bayesScore.Value;
            }
        }

        /// <summary>
        /// This minimum p-value that could arise from a table with the marginal counts of this table.
        /// </summary>
        public double MinP
        {
            get
            {
                double minP = SpecialFunctions.MinFisherExactTestPValue(TT, TF, FT, FF);
                return minP;
            }
        }

        public static List<TwoByTwo> GetSortedCollection(string inputSparseFileName1, string inputSparseFileName2, bool lowMemory, bool rememberCases)
        {
            IEnumerable<TwoByTwo> unsorted = GetUnsortedCollection(inputSparseFileName1, inputSparseFileName2, lowMemory, rememberCases);
            List<TwoByTwo> results = new List<TwoByTwo>(unsorted);
            results.Sort(delegate(TwoByTwo a, TwoByTwo b) { return a.FisherExactTest.CompareTo(b.FisherExactTest); });
            return results;
        }

        public static IEnumerable<TwoByTwo> GetUnsortedCollection(string inputSparseFileName1, string inputSparseFileName2, bool lowMemory, bool rememberCases)
        {
            IEnumerable<KeyValuePair<string, IDictionary<string, int>>> varToCidToVal2 = GroupByVariable(inputSparseFileName2, lowMemory);
            IEnumerable<TwoByTwo> unsorted = ProcessCore(inputSparseFileName1, varToCidToVal2, rememberCases);
            return unsorted;
        }

        public static IEnumerable<KeyValuePair<string, IDictionary<string, int>>> GroupByVariable(string inputSparseFileName2, bool lowMemory)
        {
            IEnumerable<KeyValuePair<string, IDictionary<string, int>>> varToCidToVal;
            if (lowMemory)
            {
                varToCidToVal = GroupByVariableLowMemory(inputSparseFileName2);
            }
            else
            {
                varToCidToVal = GroupByVariableInMemory(inputSparseFileName2);
            }
            return varToCidToVal;
        }



        public void TabulateCounts(IDictionary<string, int> cidToVal1, IDictionary<string, int> cidToVal2)
        {
            TabulateCountsInternal(cidToVal1, cidToVal2);
            Invalidate();
        }

        abstract protected void TabulateCountsInternal(IDictionary<string, int> cidToVal1, IDictionary<string, int> cidToVal2);

        private static IEnumerable<KeyValuePair<string, IDictionary<string, int>>> GroupByVariableLowMemory(string inputSparseFileName)
        {
            string header = @"var	cid	val";
            //Not using SpecialFunctions.TabFileTable because this will be an inner loop and we want it very fast
            Set<string> varSet = Set<string>.GetInstance();
            string var = null;
            IDictionary<string, int> cidToVal = null;
            using (TextReader textReader = Bio.Util.FileUtils.OpenTextStripComments(inputSparseFileName))
            {
                string line = textReader.ReadLine();
                Helper.CheckCondition(line == header, @"Expected header to be ""{0}"". File is ""{1}"".", header, inputSparseFileName);
                while (null != (line = textReader.ReadLine()))
                {
                    Debug.Assert((var == null) == (cidToVal == null)); // real assert
                    string[] fieldCollection = line.Split('\t');
                    Helper.CheckCondition(fieldCollection.Length == 3, @"Input lines should have three tab-delimited columns. File is ""{0}"". Line is ""{1}"".", inputSparseFileName, line);
                    if (fieldCollection[0] != var)
                    {
                        if (var != null)
                        {
                            yield return new KeyValuePair<string, IDictionary<string, int>>(var, cidToVal);
                        }
                        var = fieldCollection[0];
                        Helper.CheckCondition(!varSet.Contains(var), @"Input file should be grouped by variable. Variable ""{0}"" appears as more than one group. File is ""{1}"".", var, inputSparseFileName);
                        varSet.AddNew(var);
                        cidToVal = new Dictionary<string, int>();
                    }
                    string cid = fieldCollection[1];
                    Helper.CheckCondition(!cidToVal.ContainsKey(cid), @"cid ""{0}"" appears twice in variable ""{1}"". File is ""{2}"".", cid, var, inputSparseFileName);
                    int val;
                    Helper.CheckCondition(int.TryParse(fieldCollection[2], out val) && (val == 0 || val == 1), string.Format(@"Expected 0 or 1 values. Variable is ""{0}"". Cid is ""{1}"". File is ""{2}"".", var, cid, inputSparseFileName));
                    cidToVal.Add(cid, val);
                }
                Debug.Assert((var == null) == (cidToVal == null)); // real assert
                if (var != null)
                {
                    yield return new KeyValuePair<string, IDictionary<string, int>>(var, cidToVal);
                }

            }
        }


        private static Dictionary<string, IDictionary<string, int>> GroupByVariableInMemory(string inputSparseFileName2)
        {
            Dictionary<string, IDictionary<string, int>> varToCidToVal2 = new Dictionary<string, IDictionary<string, int>>();
            foreach (KeyValuePair<string, IDictionary<string, int>> varAndCidToVal2 in GroupByVariableLowMemory(inputSparseFileName2))
            {
                varToCidToVal2.Add(varAndCidToVal2.Key, varAndCidToVal2.Value);
            }
            return varToCidToVal2;
        }

        private static IEnumerable<TwoByTwo> ProcessCore(string inputSparseFileName1, IEnumerable<KeyValuePair<string, IDictionary<string, int>>> varToCidToVal2, bool rememberCases)
        {
            foreach (KeyValuePair<string, IDictionary<string, int>> varAndCidToVal1 in GroupByVariableLowMemory(inputSparseFileName1))
            {
                foreach (KeyValuePair<string, IDictionary<string, int>> varAndCidToVal2 in varToCidToVal2)
                {
                    TwoByTwo twoByTwo = TwoByTwo.GetInstance(varAndCidToVal1, varAndCidToVal2, rememberCases);
                    yield return twoByTwo;
                }
            }
        }

        public static TwoByTwo GetInstance(KeyValuePair<string, IDictionary<string, int>> varAndCidToVal1, KeyValuePair<string, IDictionary<string, int>> varAndCidToVal2)
        {
            return GetInstance(varAndCidToVal1, varAndCidToVal2, false);
        }
        public static TwoByTwo GetInstance(KeyValuePair<string, IDictionary<string, int>> varAndCidToVal1, KeyValuePair<string, IDictionary<string, int>> varAndCidToVal2, bool rememberCases)
        {
            TwoByTwo twoByTwo = rememberCases ? (TwoByTwo)new TwoByTwoCases() : (TwoByTwo)new TwoByTwoCounts();
            twoByTwo.Var1 = varAndCidToVal1.Key;
            twoByTwo.Var2 = varAndCidToVal2.Key;
            twoByTwo.TabulateCounts(varAndCidToVal1.Value, varAndCidToVal2.Value);
            return twoByTwo;
        }

        public static TwoByTwo GetInstance(IDictionary<string, int> cidToVal1, IDictionary<string, int> cidToVal2)
        {
            return GetInstance(cidToVal1, cidToVal2, false);
        }

        public static TwoByTwo GetInstance(IDictionary<string, int> cidToVal1, IDictionary<string, int> cidToVal2, bool rememberCases)
        {
            TwoByTwo twoByTwo = rememberCases ? (TwoByTwo)new TwoByTwoCases() : (TwoByTwo)new TwoByTwoCounts();
            twoByTwo.Var1 = null;
            twoByTwo.Var2 = null;
            twoByTwo.TabulateCounts(cidToVal1, cidToVal2);
            twoByTwo.Invalidate();
            return twoByTwo;
        }


        public static TwoByTwo GetInstance(int[] fisherCounts)
        {
            Helper.CheckCondition(fisherCounts.Length == 4);
            TwoByTwoCounts twoByTwo = new TwoByTwoCounts();
            twoByTwo.Var1 = null;
            twoByTwo.Var2 = null;
            twoByTwo._counts = new int[2, 2]; //C# init's to 0's

            twoByTwo.Counts[1, 1] = fisherCounts[(int)TwoByTwo.ParameterIndex.TT];
            twoByTwo.Counts[1, 0] = fisherCounts[(int)TwoByTwo.ParameterIndex.TF];
            twoByTwo.Counts[0, 1] = fisherCounts[(int)TwoByTwo.ParameterIndex.FT];
            twoByTwo.Counts[0, 0] = fisherCounts[(int)TwoByTwo.ParameterIndex.FF];
            twoByTwo.Invalidate();

            return twoByTwo;
        }

        public static TwoByTwo GetInstance(int tt, int tf, int ft, int ff)
        {
            TwoByTwoCounts twoByTwo = new TwoByTwoCounts();
            twoByTwo.Var1 = null;
            twoByTwo.Var2 = null;
            twoByTwo._counts = new int[2, 2]; //C# init's to 0's

            twoByTwo.Counts[1, 1] = tt;
            twoByTwo.Counts[1, 0] = tf;
            twoByTwo.Counts[0, 1] = ft;
            twoByTwo.Counts[0, 0] = ff;

            twoByTwo.Invalidate();

            return twoByTwo;
        }

        public static TwoByTwo GetInstance(string var1, string var2, bool rememberCases)
        {
            if (rememberCases)
            {
                TwoByTwoCases twoByTwo = new TwoByTwoCases();
                twoByTwo.Var1 = var1;
                twoByTwo.Var2 = var2;
                twoByTwo.Cases = new List<string>[2, 2] { { new List<string>(), new List<string>() }, { new List<string>(), new List<string>() } };
                twoByTwo.Invalidate();
                return twoByTwo;
            }
            else
            {
                TwoByTwoCounts twoByTwo = new TwoByTwoCounts();
                twoByTwo.Var1 = var1;
                twoByTwo.Var2 = var2;
                twoByTwo._counts = new int[2, 2]; //C# init's to 0's
                twoByTwo.Invalidate();
                return twoByTwo;
            }
        }


        //Should we define a class like FilterForTwoByTwos (or Converter<TwoByTwo,bool>) and apply the list of them to make this more extensible?
        public bool PassTests(bool positiveDirectionOnly, double fishersExactTestMaximum, double countOrExpCountMinimum)
        {
            if (FisherExactTest > fishersExactTestMaximum)
            {
                return false;
            }

            if (MinOfCountOfExpCount() < countOrExpCountMinimum)
            {
                return false;
            }

            if (positiveDirectionOnly && !RightDirection())
            {
                return false;
            }


            return true;
        }

        private double MinOfCountOfExpCount()
        {
            double minOfCountOrExpCount = SpecialFunctions.MinOfCountOrExpCount(Counts[1, 1], Counts[1, 0], Counts[0, 1], Counts[0, 0]);
            return minOfCountOrExpCount;
        }

        public bool RightDirection()
        {
            bool b = (Counts[0, 0] * Counts[1, 1] < Counts[0, 1] * Counts[1, 0]);
            return b;
        }

        public bool PositiveCorrelation()
        {
            bool b = (Counts[0, 0] * Counts[1, 1] > Counts[0, 1] * Counts[1, 0]);
            return b;
        }

        public double CorrelationValue()
        {
            double d = (Counts[0, 0] * Counts[1, 1] - Counts[0, 1] * Counts[1, 0]);
            return d;
        }


        public static int GetRightSum(int[] fisherCounts)
        {
            int tt = fisherCounts[(int)TwoByTwo.ParameterIndex.TT];
            int ft = fisherCounts[(int)TwoByTwo.ParameterIndex.FT];

            int sum = tt + ft;
            return sum;

        }

        public static int GetLeftSum(int[] fisherCounts)
        {
            int tt = fisherCounts[(int)TwoByTwo.ParameterIndex.TT];
            int tf = fisherCounts[(int)TwoByTwo.ParameterIndex.TF];

            int sum = tt + tf;
            return sum;
        }

        public virtual void IncrementCell(bool var1IsTrue, bool var2IsTrue)
        {
            ++Counts[var1IsTrue ? 1 : 0, var2IsTrue ? 1 : 0];
            Invalidate();
        }

        public virtual void IncrementRow(string varName, bool varIsTrue)
        {
            int rowIdx;
            if (varName.Equals(Var1, StringComparison.CurrentCultureIgnoreCase))
                rowIdx = 0;
            else if (varName.Equals(Var2, StringComparison.CurrentCultureIgnoreCase))
                rowIdx = 1;
            else
                throw new ArgumentException(varName + " is not one of the variables.");

            ++Counts[rowIdx, varIsTrue ? 1 : 0];
        }

        public static IEnumerable<TwoByTwo> GetCollectionFromMatrices(bool unsorted, string inputMatrixFileName1, string inputMatrixFileName2, ParallelOptions parallelOptions)
        {
            MatrixFactory<string, string, SufficientStatistics> mf = MatrixFactory<string, string, SufficientStatistics>.GetInstance();
            var m1 = mf.Parse(inputMatrixFileName1, MissingStatistics.GetInstance(), parallelOptions);
            var m2 = mf.Parse(inputMatrixFileName2, MissingStatistics.GetInstance(), parallelOptions);

            List<TwoByTwo> tableList = new List<TwoByTwo>();

            foreach (string key in m1.RowKeys)
            {
                if (m2.ContainsRowKey(key))
                {
                    TwoByTwo table = TwoByTwo.GetInstance(inputMatrixFileName1, key, false);
                    var m1NonMissing = m1.RowView(key).Select(kvp => kvp);
                    var m2NonMissing = m2.RowView(key).Select(kvp => kvp);

                    // m1 is T for first col, m2 is F for first Col.
                    table.TT = m1NonMissing.Select(kvp => (int)kvp.Value.AsDiscreteStatistics()).Sum();
                    table.TF = m1NonMissing.Count() - table.TT;
                    table.FT = m2NonMissing.Select(kvp => (int)kvp.Value.AsDiscreteStatistics()).Sum();
                    table.FF = m2NonMissing.Count() - table.FT;

                    if (unsorted)
                        yield return table;
                    else
                        tableList.Add(table);
                }
            }
            if (!unsorted)
            {
                tableList.Sort((t1, t2) => t1.FisherExactTest.CompareTo(t2.FisherExactTest));
                foreach (var table in tableList)
                    yield return table;
            }
        }

        public static IEnumerable<TwoByTwo> GetCollection(bool lowMemory, bool unsorted, string inputSparseFileName1, string inputSparseFileName2)
        {
            return GetCollection(lowMemory, unsorted, inputSparseFileName1, inputSparseFileName2, false);
        }

        public static IEnumerable<TwoByTwo> GetCollection(bool lowMemory, bool unsorted, string inputSparseFileName1, string inputSparseFileName2, bool rememberCases)
        {
            if (unsorted)
            {
                return TwoByTwo.GetUnsortedCollection(inputSparseFileName1, inputSparseFileName2, lowMemory, rememberCases);
            }
            else
            {
                return TwoByTwo.GetSortedCollection(inputSparseFileName1, inputSparseFileName2, lowMemory, rememberCases);
            }
        }

        public int RowSum(bool var1IsTrue)
        {
            int row = var1IsTrue ? 1 : 0;
            int sum = Counts[row, 0] + Counts[row, 1];
            return sum;
        }

        public int ColSum(bool var2IsTrue)
        {
            int col = var2IsTrue ? 1 : 0;
            int sum = Counts[0, col] + Counts[1, col];
            return sum;
        }

        public int Sum()
        {
            int sum = Counts[0, 0] + Counts[0, 1] + Counts[1, 0] + Counts[1, 1];
            return sum;
        }


        public void Clear()
        {
            Counts[0, 0] = 0;
            Counts[0, 1] = 0;
            Counts[1, 0] = 0;
            Counts[1, 1] = 0;
            Invalidate();
        }

        public TwoByTwo SampleFromNull()
        {
            return SampleFromNull(Sum());
        }

        public TwoByTwo SampleFromNull(int N)
        {
            int sum = Sum();
            double p1 = (double)RowSum(true) / sum;
            double p2 = (double)ColSum(true) / sum;
            Random rand = new Random((int)DateTime.Now.Ticks);

            TwoByTwo nullTable = TwoByTwo.GetInstance(this.Var1, this.Var2, false);
            for (int i = 0; i < N; i++)
            {
                nullTable.IncrementCell(MachineInvariantRandom.GlobalRand.NextDouble() < p1, MachineInvariantRandom.GlobalRand.NextDouble() < p2);
            }
            return nullTable;
        }

        public TwoByTwo SampleFromAlt()
        {
            return SampleFromAlt(Sum());
        }
        public TwoByTwo SampleFromAlt(int N)
        {
            int sum = Sum();
            double pRow = (double)RowSum(true) / sum;
            double pColGivenRowIsTrue = (double)TT / RowSum(true);
            double pColGivenRowIsFalse = (double)FT / RowSum(false);

            TwoByTwo altTable = TwoByTwo.GetInstance(this.Var1, this.Var2, false);
            for (int i = 0; i < N; i++)
            {
                if (MachineInvariantRandom.GlobalRand.NextDouble() < pRow)
                {
                    altTable.IncrementCell(true, MachineInvariantRandom.GlobalRand.NextDouble() < pColGivenRowIsTrue);
                }
                else
                {
                    altTable.IncrementCell(false, MachineInvariantRandom.GlobalRand.NextDouble() < pColGivenRowIsFalse);
                }
            }
            return altTable;
        }

        public TwoByTwo SampleFromAlt(double gamma)
        {
            return SampleFromAlt(gamma, Sum());
        }
        /// <summary>
        /// Sample such that the marginals p(X) and p(Y) are expected to be the same as this table, but p(Y|X=true) = gamma*p(Y|X=false)
        /// </summary>
        /// <param name="gamma"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public TwoByTwo SampleFromAlt(double gamma, int N)
        {
            int sum = Sum();
            double pRow = (double)RowSum(true) / sum;
            double pCol = (double)ColSum(true) / sum;

            double pColGivenRowIsFalse = pCol / (gamma * pRow + (1 - pRow));
            double pColGivenRowIsTrue = gamma * pColGivenRowIsFalse;

            TwoByTwo altTable = TwoByTwo.GetInstance(this.Var1, this.Var2, false);
            for (int i = 0; i < N; i++)
            {
                if (MachineInvariantRandom.GlobalRand.NextDouble() < pRow)
                {
                    altTable.IncrementCell(true, MachineInvariantRandom.GlobalRand.NextDouble() < pColGivenRowIsTrue);
                }
                else
                {
                    altTable.IncrementCell(false, MachineInvariantRandom.GlobalRand.NextDouble() < pColGivenRowIsFalse);
                }
            }
            return altTable;
        }

        public void Report(TextWriter textWriter)
        {
            textWriter.WriteLine(Helper.CreateTabString("", "not " + Var2, Var2, "total"));
            textWriter.WriteLine(Helper.CreateTabString("not " + Var1, Counts[0, 0], Counts[0, 1], RowSum(false)));
            textWriter.WriteLine(Helper.CreateTabString(Var1, Counts[1, 0], Counts[1, 1], RowSum(true)));
            textWriter.WriteLine(Helper.CreateTabString("total", ColSum(false), ColSum(true), Sum()));
        }


    }

    public class TwoByTwoCounts : TwoByTwo
    {
        internal TwoByTwoCounts()
        {
        }
        public override int[,] Counts
        {
            get
            {
                return _counts;
            }
        }
        internal int[,] _counts;

        protected override void TabulateCountsInternal(IDictionary<string, int> cidToVal1, IDictionary<string, int> cidToVal2)
        {
            _counts = new int[2, 2]; //c# inits to zeros
            foreach (KeyValuePair<string, int> cidAndVal1 in cidToVal1)
            {
                int val2;
                if (cidToVal2.TryGetValue(cidAndVal1.Key, out val2))
                {
                    ++_counts[cidAndVal1.Value, val2];
                }
            }
        }
    }
    public class TwoByTwoCases : TwoByTwo
    {
        public override int[,] Counts
        {
            get
            {
                return new int[2, 2] { { Cases[0, 0].Count, Cases[0, 1].Count }, { Cases[1, 0].Count, Cases[1, 1].Count } };
            }
        }
        internal TwoByTwoCases()
        {
        }
        public List<string>[,] Cases;

        public override void IncrementCell(bool var1IsTrue, bool var2IsTrue)
        {
            throw new NotSupportedException("The cases version of TwoByTwo does not support Incrememnt(bool,bool). Uses Counts version.");
        }

        protected override void TabulateCountsInternal(IDictionary<string, int> cidToVal1, IDictionary<string, int> cidToVal2)
        {
            Cases = new List<string>[2, 2] { { new List<string>(), new List<string>() }, { new List<string>(), new List<string>() } };

            foreach (KeyValuePair<string, int> cidAndVal1 in cidToVal1)
            {
                string cid = cidAndVal1.Key;
                int val2;
                if (cidToVal2.TryGetValue(cid, out val2))
                {
                    Cases[cidAndVal1.Value, val2].Add(cid);
                }
            }
        }

    }
}
