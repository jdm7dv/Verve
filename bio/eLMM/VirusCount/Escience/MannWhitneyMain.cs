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
using MBT.Escience;
using MBT.Escience.Parse;
using Bio.Util;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace MannWhitney
{

    ///// <summary>
    ///// copied over from Carl's unchecked in code, and modifed becuase I'm not yet on C#4.0
    /////case to play with
    ////List<double> tmpP = new List<double> { 0.01, 0.2, 0.3, 0.5, 0.01, 0.02 };
    ////List<int> tmpInt = new List<int> { 0, 0, 0, 0, 1, 1 };//{ 1, 1, 1, 1, 0, 0 };
    ////List<KeyValuePair<double, int>> tmpDat = SpecialFunctions.EnumerateTwo(tmpP, tmpInt).ToList();
    ////var zAndP = MannWhitney.MannWhitney.MannWhitneyUTestOneSided(tmpDat, 1000, false);
    ////Console.WriteLine(zAndP);
    ///// </summary>

    public class MannWhitney
    {


        private static Predicate<int> CreateSplitIndexFilter(string splitIndexFilterNameOrNull)
        {
            if (splitIndexFilterNameOrNull == null)
            {
                return crossCount =>
                    {
                        Helper.CheckCondition(crossCount == 0, "If not explicit splitIndexFilter is given then all crossCountIndex values must be 0");
                        return true;
                    };
            }
            else
            {
                int goalValue = int.Parse(splitIndexFilterNameOrNull);
                return crossCount => (crossCount == goalValue);
            }
        }

        private static IEnumerable<KeyValuePair<string, List<InputRow>>> CreateKeyListAsStringAndRowListQuery(
                    Predicate<int> splitIndexFilter, List<KeyValuePair<string, string>> cidGroupAndInputFileNameList, string pTargetHeader, List<string> keyNameList, ParallelOptions parallelOptions)
        {

            //read from all the files at once, return all the lines with the same key. Assume (but check) all have same keys in same order
            List<IEnumerable<InputRow>> nextSetOfLinesQueryList =
                cidGroupAndInputFileNameList.Select(cidGroupAndInputFileName => CreateRowListQuery(splitIndexFilter, cidGroupAndInputFileName, pTargetHeader, keyNameList)).ToList();
            IEnumerable<InputRow> mergedInputRows = SpecialFunctions.EnumerateSortedMerge(nextSetOfLinesQueryList, inputRow => inputRow.KeyList.StringJoin("\t"));
            IEnumerable<List<InputRow>> groupedInputRowList = SpecialFunctions.EnumerateSortedGroups(mergedInputRows, inputRow => inputRow.KeyList.StringJoin("\t"));


            foreach (List<InputRow> groupedInputRow in groupedInputRowList)
            {
                string key = groupedInputRow[0].KeyList.StringJoin("\t");

                var nextItem = new KeyValuePair<string, List<InputRow>>(key, groupedInputRow);
                yield return nextItem;
            }
        }

        private static IEnumerable<List<InputRow>> NextSetOfLinesQueryList(Predicate<int> splitIndexFilter, KeyValuePair<string, string> cidGroupAndInputFileName, string pTargetHeader, List<string> keyNameList)
        {
            var inputRowQuery = CreateRowListQuery(splitIndexFilter, cidGroupAndInputFileName, pTargetHeader, keyNameList);
            return SpecialFunctions.EnumerateSortedGroups(inputRowQuery, inputRow => inputRow.KeyList.StringJoin("\t"));
        }


        //private static IEnumerable<InputRow> CreateRowListQuery(List<KeyValuePair<string, string>> cidGroupAndInputFileNameList, string pTargetHeader, List<string> keyNameList)
        //{
        //    foreach (var cidGroupAndInputFileName in cidGroupAndInputFileNameList)
        //    {
        //        string cidGroup = cidGroupAndInputFileName.Key;
        //        string inputFileName = cidGroupAndInputFileName.Value;
        //        Console.WriteLine("Reading file " + inputFileName);

        //        MBT.Escience.CounterWithMessages counterWithMessages = new MBT.Escience.CounterWithMessages("reading cidGroup " + cidGroup + " file, line #{0}", 10000, null);
        //        string header;
        //        foreach (var line in SpecialFunctions.TabFileTable(inputFileName, includeWholeLine:false, out header))
        //        {
        //            counterWithMessages.Increment();
        //            List<string> keyList = keyNameList.Select(keyName => line[keyName]).ToList();
        //            InputRow inputRow = new InputRow
        //            {
        //                //SplitIndex = int.Parse(line.GetValueOrDefault("splitIndex", "0")),
        //                Cid = line["cid"],
        //                CidGroup = cidGroup,
        //                TargetVal = int.Parse(line["TargetVal"]),
        //                PTarget = double.Parse(line[pTargetHeader]),
        //                KeyList = keyList
        //            };
        //            yield return inputRow;
        //        }
        //    }
        //}

        private static IEnumerable<InputRow> CreateRowListQuery(Predicate<int> splitIndexFilter, KeyValuePair<string, string> cidGroupAndInputFileName, string pTargetHeader, List<string> keyNameList)
        {
            string header1 = "pathway	cid	nullIndex	pTarget	targetVal	logLikelihood	splitIndex	splitCount	workIndex	workCount	testCidIndex	testCidCount";
            string header2 = "pathway	cid	nullIndex	pTarget(or prediction)	targetVal	logLikelihood(or score)	splitIndex	splitCount	workIndex	workCount	testCidIndex	testCidCount";
            string[] fields = header1.Split('\t');
            Dictionary<string, int> fieldToIndex = fields.Select((key, index) => new KeyValuePair<string, int>(key, index)).
                ToDictionary(keyAndIndex => keyAndIndex.Key, keyAndIndex => keyAndIndex.Value);
            List<int> keyIndexList = keyNameList.Select(keyName => fieldToIndex[keyName]).ToList();
            int cidIndex = fieldToIndex["cid"];
            int targetValIndex = fieldToIndex["targetVal"];
            int pTargetHeaderIndex = fieldToIndex[pTargetHeader];
            int splitIndexIndex = fieldToIndex["splitIndex"];

            string cidGroup = cidGroupAndInputFileName.Key;
            string inputFileName = cidGroupAndInputFileName.Value;
            Console.WriteLine("Reading file " + inputFileName);

            MBT.Escience.CounterWithMessages counterWithMessages = new MBT.Escience.CounterWithMessages("reading cidGroup " + cidGroup + " file, line #{0}", 100000, null);
            using (TextReader textReader = File.OpenText(inputFileName))
            {
                //pathway	cid	nullIndex	pTarget	targetVal	logLikelihood	splitIndex	splitCount	workIndex	workCount	testCidIndex	testCidCount
                //   0        1   2           3       4
                string line;
                while (null != (line = textReader.ReadLine()))
                {
                    if (line == header1 || line == header2)
                    {
                        continue; //not break;
                    }
                    counterWithMessages.Increment();
                    string[] valueList = line.Split('\t');
                    List<string> keyList = keyIndexList.Select(keyIndex => valueList[keyIndex]).ToList();

                    if (!splitIndexFilter(int.Parse(valueList[splitIndexIndex])))
                    {
                        continue; // not break;
                    }
                    InputRow inputRow = new InputRow
                    {
                        //SplitIndex = int.Parse(line.GetValueOrDefault("splitIndex", "0")),
                        Cid = valueList[cidIndex],
                        CidGroup = cidGroup,
                        TargetVal = int.Parse(valueList[targetValIndex]),
                        PTarget = double.Parse(valueList[pTargetHeaderIndex]),
                        KeyList = keyList
                    };
                    yield return inputRow;
                }
            }
        }



        private static ResultsRow CreateResultRow(KeyValuePair<string, List<InputRow>> keyListAsStringAndRowList, MBT.Escience.CounterWithMessages counterWithMessages, ParallelOptions parallelOptions)
        {
            counterWithMessages.Increment();

            string keyListAsString = keyListAsStringAndRowList.Key;
            List<InputRow> rowListAll = keyListAsStringAndRowList.Value;

            var cidGroupAndRowListList =
                (from row in rowListAll
                 group row by row.CidGroup into g
                 select new KeyValuePair<string, List<InputRow>>(g.Key, g.ToList())
                    ).ToList();


            //Creates a result for each cidGroup
            var cidGroupToPValue =
                (
                    from cidGroupAndRowList in cidGroupAndRowListList
                    let cidGroup = cidGroupAndRowList.Key
                    let rowList = cidGroupAndRowList.Value
                    orderby cidGroup
                    select new { cidGroup, Z0AndPValue = ComputeZ0AndPValue(rowList, row => row.PTarget, row => row.TargetVal, parallelOptions) }
                    ).ToDictionary(pair => pair.cidGroup, pair => pair.Z0AndPValue);


            double z0OfAll = cidGroupToPValue.Values.Select(z0AndPValue => z0AndPValue.Key).Sum() / Math.Sqrt(cidGroupToPValue.Count);
            double pOfAll = 1.0 - SpecialFunctions.ZScoreToOneTailedPValue(z0OfAll, 1e-10);


            //Create the group for all
            ResultsRow result = new ResultsRow { KeyListAsString = keyListAsString, CidGroupToZ0AndPValue = cidGroupToPValue, ComboPValue = pOfAll };

            return result;

        }

        /// <summary>
        /// Looks to see if the first enumeration of doubles is bigger than the second enumeration, using a one-tailed test.
        /// </summary>
        /// <param name="myDataAnd01Labels"></param>
        /// <param name="maxNumPermutations"></param>
        /// <param name="forceAssymptoticApprox"></param>
        /// <returns>The z score and the p-value</returns>
        public static KeyValuePair<double, double> MannWhitneyUTestOneSided(IEnumerable<double> dataAssumedToBeBigger, IEnumerable<double> dataAssumedToBeSmaller,
                                                   int maxNumPermutations, bool forceAssymptoticApprox = false, bool neverDoExactPermutations = false, ParallelOptions parallelOptionsOrNullFor1 = null)
        {
            var asSingleLabeledList = SpecialFunctions.EnumerateAll(
                dataAssumedToBeBigger.Select(d => new KeyValuePair<double, int>(d, 1)),
                dataAssumedToBeSmaller.Select(d => new KeyValuePair<double, int>(d, 0))).ToList();

            return MannWhitneyUTestOneSided(asSingleLabeledList, maxNumPermutations, forceAssymptoticApprox, neverDoExactPermutations, parallelOptionsOrNullFor1);
        }

        /// <summary>
        /// Returns the 2-sided MannWhitney p-value comparing the two data sets.
        /// </summary>
        /// <param name="dataSet1"></param>
        /// <param name="dataSet2"></param>
        /// <param name="maxNumPermutations"></param>
        /// <param name="forceAssymptoticApprox"></param>
        /// <param name="neverDoExactPermutations"></param>
        /// <param name="parallelOptionsOrNullFor1"></param>
        /// <returns></returns>
         public static double MannWhitneyUTestTwoSided(IEnumerable<double> dataSet1, IEnumerable<double> dataSet2,
                                                   int maxNumPermutations, bool forceAssymptoticApprox = false, bool neverDoExactPermutations = false, ParallelOptions parallelOptionsOrNullFor1 = null)
        {
            double oneSidedP = MannWhitneyUTestOneSided(dataSet1, dataSet2, maxNumPermutations, forceAssymptoticApprox, neverDoExactPermutations, parallelOptionsOrNullFor1).Value;
            return 2 * (oneSidedP < 0.5 ? oneSidedP : 1 - oneSidedP);
        }

        /// <summary>
        /// Looks to see if group labeled with 1 is larger than group labelled with 0, using a one-tailed test.
        /// </summary>
        /// <param name="myDataAnd01Labels"></param>
        /// <param name="maxNumPermutations"></param>
        /// <param name="forceAssymptoticApprox"></param>
         /// <returns>The z score and the p-value</returns>
         public static KeyValuePair<double, double> MannWhitneyUTestOneSided(List<KeyValuePair<double, int>> myDataAnd01Labels,
                                                   int maxNumPermutations, bool forceAssymptoticApprox = false, bool neverDoExactPermutations = false, ParallelOptions parallelOptionsOrNullFor1 = null )
        {
            return ComputeZ0AndPValue(myDataAnd01Labels, elt => elt.Key, elt => elt.Value, maxNumPermutations = 10000, forceAssymptoticApprox, neverDoExactPermutations, parallelOptionsOrNullFor1);
        }

        public static double MannWhitneyUTestTwoSided(List<KeyValuePair<double, int>> myDataAnd01Labels,
                                                   int maxNumPermutations, bool forceAssymptoticApprox = false, bool neverDoExactPermutations = false, ParallelOptions parallelOptionsOrNullFor1 = null)
         {
             double oneSidedP = MannWhitneyUTestOneSided(myDataAnd01Labels, maxNumPermutations, forceAssymptoticApprox, neverDoExactPermutations, parallelOptionsOrNullFor1).Value;
             return 2 * (oneSidedP < 0.5 ? oneSidedP : 1 - oneSidedP);
         }

        /// <returns>The z score and the p-value</returns>
        public static KeyValuePair<double, double> ComputeZ0AndPValue<T>(IList<T> rowList,
            Func<T, double> pTargetFunc, Func<T, int> targetValFunc, ParallelOptions parallelOptions)
        {
            int maxNumPerm = 10000;
            return ComputeZ0AndPValue(rowList, pTargetFunc, targetValFunc, maxNumPerm, false, true, parallelOptions);
        }


        /// <summary>
        /// See nicer wrapper: MannWhitneyUTestOneSided.
        /// this is a one-sided test looking for the case where the group labelled with 1 is larger than the group labelled with 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rowList"></param>
        /// <param name="scoreAccessor"></param>
        /// <param name="label01Accessor"></param>
        /// <param name="maxNumPermutations"></param>
        /// <param name="forceAssymptoticApprox"></param>
        /// <param name="neverDoExactPermutations"></param>
        /// <param name="parallelOptionsOrNullFor1"></param>
        /// <returns>The z score and the p-value</returns>
        public static KeyValuePair<double, double> ComputeZ0AndPValue<T>(IList<T> rowList,
            Func<T, double> scoreAccessor, Func<T, int> label01Accessor, int maxNumPermutations = 10000, bool forceAssymptoticApprox = false, bool neverDoExactPermutations = false,
            ParallelOptions parallelOptionsOrNullFor1 = null)
        {
            ParallelOptions parallelOptions = parallelOptionsOrNullFor1 ?? new ParallelOptions() { MaxDegreeOfParallelism = 1 };
            

            //var zeroAndCountThenOneAndCount = CreateZeroAndCountThenOneAndCount(rowList, pTargetFunc, targetValFunc, parallelOptions);
            //int n0 = zeroAndCountThenOneAndCount.First().Value;
            //int n1 = SpecialFunctions.FirstAndOnly(zeroAndCountThenOneAndCount.Skip(1)).Value;// the class we think has larger values for the one-tailed test

            //having problems with the parallelOptions above, so re-writing like this
            int n0 = rowList.Where(elt => label01Accessor(elt) == 0).Count();
            int n1 = rowList.Where(elt => label01Accessor(elt) == 1).Count();

            double z0;

            //Helper.CheckCondition(ignoreSafetyOfNormal || (n0 > 10 && n1 > 10), "The count should be at least 10 for the normal distribution to work");

            double p;
            if ((n0 > 10 && n1 > 10) || forceAssymptoticApprox)
            {
                z0 = ComputeZ0<T>(rowList, parallelOptions, n0, n1, scoreAccessor, label01Accessor);
                p = 1.0 - SpecialFunctions.ZScoreToOneTailedPValue(z0, 1e-10);
                SanityCheckP(z0, p);
            }
            else
            {
                ParallelOptions parallelOptions1 = new ParallelOptions { MaxDegreeOfParallelism = 1 };

                //now need to check out here if using all permutations or not to bypass Carl's code if not
                double logExactPermutationCount = SpecialFunctions.LogFactorialNMOverFactorialNFactorialMApprox(n0, n1);
                bool useExactPermutations = (logExactPermutationCount <= Math.Log(maxNumPermutations)) && !neverDoExactPermutations;
                List<double> zList;

                if (useExactPermutations)
                {
                    z0 = ComputeZ0<T>(rowList, parallelOptions, n0, n1, scoreAccessor, label01Accessor);
                    /*faster than this is to simply permute the ranks of the real data (including ties), rather than the real data itself, but leaving this in for when exact permutations are needed*/
                    zList =
                        (from permutation in SpecialFunctions.Permute01Targets(rowList, scoreAccessor, label01Accessor, maxNumPermutations)
                            .AsParallel().WithDegreeOfParallelism(parallelOptions.MaxDegreeOfParallelism)
                         let z = ComputeZ0(permutation, parallelOptions1, n0, n1, pair => pair.Key, pair => pair.Value)
                         orderby z
                         select z).ToList();
                }
                else
                {
                    /*--------------------------------------------------------------------------------------------------
                      NB there is now a dead branch in SpecialFunctions.Permute01Targets(), which formerly used to do both
                     'exact'/'complete' and 'inexact'/'subsampled' permutations. Now it only does the former ,and the 'inexact' is here. This is because I 
                     do it much faster, but didn't want to bother with doing the 'exact'. 
                    -------------------------------------------------------------------------------------------------*/
                    //don't bother converting to z, just use u instead
                    List<double> listOfAllValues = rowList.Select(elt => scoreAccessor(elt)).ToList();
                    List<double> ranksWithTies = SpecialFunctions.RanksWithTies(listOfAllValues);
                    //List<int> indsOfClass0 = Enumerable.Range(0, n0 + n1).ToList().Where(elt => targetValFunc(rowList[elt]) == 0).ToList();
                    //List<double> ranksWithTiesClass0 = ranksWithTies.SubList(indsOfClass0);
                    //double u0 = ComputeUFromRanks(ranksWithTiesClass0);
                    List<int> indsOfClass1 = Enumerable.Range(0, n0 + n1).ToList().Where(elt => label01Accessor(rowList[elt]) == 1).ToList();
                    List<double> ranksWithTiesClass1 = ranksWithTies.SubList(indsOfClass1);
                    double u1 = ComputeUFromRanks(ranksWithTiesClass1);

                    //!!!not parallelized
                    List<double> uList = new List<double>();
                    Random myRand = new MachineInvariantRandom("123456");
                    for (int perm = 0; perm < maxNumPermutations; perm++)
                    {
                        ranksWithTies.ShuffleInPlace(myRand);

                        List<double> ranksWithTies0 = ranksWithTies.SubSequence(0, n0).ToList();
                        double thisUscore0 = ComputeUFromRanks(ranksWithTies0);
                        List<double> ranksWithTies1 = ranksWithTies.SubSequence(n0, n1).ToList();
                        double thisUscore1 = ComputeUFromRanks(ranksWithTies1);

                        //if it were 2-sided, we would use this (I think)
                        //double uScore = Math.Min(thisUscore0, thisUscore1);
                        //but it's one-sided, so we use the one from the set that had labels "1"
                        double uScore = thisUscore1;

                        //double thisZ = ComputeZfromU(n0, n1, uScore);
                        uList.Add(uScore);
                    }
                    //to let the rest of the code do what it should
                    zList = uList;
                    z0 = u1;
                }
                TwoByOne twoByOne = TwoByOne.GetInstance(zList, z => z0 <= z);
                p = twoByOne.Freq;
                //Can't  SanityCheckP(z0, p) because ties mean it wont always get the right answer
            }



            ////To get two-sided, which says "are they different" use this pTwoSided = 2 * ((p < .5) ? p : (1-p));
            //ResultsRow resultRow = new ResultsRow { DataSetName = dataSetName, CidGroup= cidGroup, PValue = p, N0 = n0, N1 = n1, UScore0 = uScore0, UScore1 = uScore1, Z0 = z0, Z1 = -z0 };
            //return resultRow;
            return new KeyValuePair<double, double>(z0, p);
        }


        /// <summary>
        /// following on wikipedia
        /// </summary>
        /// <param name="ranksWithTies1"></param>
        /// <returns></returns>
        private static double ComputeUFromRanks(List<double> ranksWithTies1)
        {
            double R1 = ranksWithTies1.Sum();
            int n1 = ranksWithTies1.Count;
            return R1 - n1 * (n1 + 1) / 2;
        }


        private static void SanityCheckP(double z0, double p)
        {
            //We want the uScore of 0 to be bigger than the uScore of 1, because we want the 0's to be listed before the 1's when
            //the cases are sorted by predicted probability. When that is not the case, our predictor is worse than nothing.
            if (z0 >= 0)
            {
                Helper.CheckCondition(p <= .5, "When the predictor is correlated with the truth, p<=.5");
            }
            else
            {
                Helper.CheckCondition(p > .5, "When the predictor is correlated with the truth, p>.5");
            }
        }

        private static double ComputeZ0<T>(IList<T> rowList, ParallelOptions parallelOptions, int n0, int n1,
                    Func<T, double> pTargetFunc,
                    Func<T, int> targetValFunc
)
        {
            //table, where each row has tow parts, the first part is the input real-value, the second part is the number of trues and false that have that prediction 
            var sortedProbabilityAndTwoByOneList = CreatedSortedProbabilityAndTwoByOneList(parallelOptions, rowList, pTargetFunc, targetValFunc);
            double uScore0 = ComputeUScore(parallelOptions, sortedProbabilityAndTwoByOneList);
            double uScore1 = n0 * n1 - uScore0;
            double z0 = ComputeZfromU(n0, n1, uScore0);


            //We want the uScore of 0 to be bigger than the uScore of 1, because we want the 0's to be listed before the 1's when
            //the cases are sorted by predicted probability. When that is not the case, our predictor is worse than nothing.
            if (uScore0 >= uScore1)
            {
                Helper.CheckCondition(z0 >= 0, "When the predictor is correlated with the truth, z >0");
            }
            else
            {
                Helper.CheckCondition(z0 < 0, "When the predictor is correlated with the truth, z <0");
            }

            return z0;
        }

        private static double ComputeZfromU(int n0, int n1, double uScore0)
        {
            double z0 = (uScore0 - n0 * n1 / 2.0) / Math.Sqrt(((double)n0 * n1 * (n0 + n1 + 1)) / 12.0);
            return z0;
        }

        //private static double FindZ0OfRowList(List<InputRow> rowList, bool ignoreSafetyOfNormal, ParallelOptions parallelOptions)
        //{
        //    var zeroAndCountThenOneAndCount = CreateZeroAndCountThenOneAndCount(parallelOptions, rowList);
        //    int n0 = zeroAndCountThenOneAndCount.First().Value;
        //    int n1 = SpecialFunctions.FirstAndOnly(zeroAndCountThenOneAndCount.Skip(1)).Value;

        //    var sortedProbabilityAndTwoByOneList = CreatedSortedProbabilityAndTwoByOneList(parallelOptions, rowList);
        //    double uScore0 = ComputeUScore(parallelOptions, sortedProbabilityAndTwoByOneList);
        //    double uScore1 = n0 * n1 - uScore0;

        //    Helper.CheckCondition(ignoreSafetyOfNormal || (n0 > 10 && n1 > 10), "The count should be at least 10 for the normal distribution to work");

        //    double z0 = (uScore0 - n0 * n1 / 2.0) / Math.Sqrt(((double)n0 * n1 * (n0 + n1 + 1)) / 12.0);
        //    return z0;
        //}

        private static List<KeyValuePair<int, int>> CreateZeroAndCountThenOneAndCount<T>
                (IList<T> rowList,
                Func<T, double> pTargetFunc, Func<T, int> targetValFunc,
                ParallelOptions parallelOptions)
        {
            var zeroAndCountThenOneAndCount =
                (from row in rowList
                .AsParallel().WithDegreeOfParallelism(parallelOptions.MaxDegreeOfParallelism)
                 group row by targetValFunc(row) into g
                 let targetAndCount = new KeyValuePair<int, int>(g.Key, g.Count())
                 orderby targetAndCount.Key
                 select targetAndCount
                 ).ToList();
            return zeroAndCountThenOneAndCount;
        }

        private static List<KeyValuePair<double, TwoByOne>>
            CreatedSortedProbabilityAndTwoByOneList<T>
                    (ParallelOptions parallelOptions,
                    IList<T> rowList,
                    Func<T, double> pTargetFunc,
                    Func<T, int> targetValFunc
            )
        {
            //!!!Is it efficent to group by probability before sorting?
            var sortedProbabilityAndTwoByOneList =
                (from row in rowList
                .AsParallel().WithDegreeOfParallelism(parallelOptions.MaxDegreeOfParallelism)
                 group row by pTargetFunc(row) into g
                 orderby g.Key
                 select new KeyValuePair<double, TwoByOne>(g.Key, CreateTwoByOne(g, targetValFunc))
                ).ToList();
            return sortedProbabilityAndTwoByOneList;
        }

        private static double ComputeUScore(ParallelOptions parallelOptions, List<KeyValuePair<double, TwoByOne>> sortedProbabilityAndTwoByOneList)
        {
            double uScore =
                (from probabilityIndex1 in Enumerable.Range(0, sortedProbabilityAndTwoByOneList.Count)
                    .AsParallel().WithDegreeOfParallelism(parallelOptions.MaxDegreeOfParallelism)
                 select UScoreForOneProbabilityAndTwoByOne(sortedProbabilityAndTwoByOneList, probabilityIndex1)
                 ).Sum();
            return uScore;

        }

        private static double UScoreForOneProbabilityAndTwoByOne(List<KeyValuePair<double, TwoByOne>> sortedProbabilityAndTwoByOneList, int probabilityIndex1)
        {
            TwoByOne twoByOneTop = sortedProbabilityAndTwoByOneList[probabilityIndex1].Value;
            int falseCount = twoByOneTop.FalseCount;
            if (falseCount == 0)
            {
                return 0;
            }
            double uScoreSoFar = .5 * twoByOneTop.TrueCount * twoByOneTop.FalseCount;
            for (int probabilityIndex2 = probabilityIndex1 + 1; probabilityIndex2 < sortedProbabilityAndTwoByOneList.Count; ++probabilityIndex2)
            {
                int trueCount = sortedProbabilityAndTwoByOneList[probabilityIndex2].Value.TrueCount;
                uScoreSoFar += falseCount * trueCount;
            }
            return uScoreSoFar;
        }

        private static TwoByOne CreateTwoByOne<T>(IEnumerable<T> rowList, Func<T, int> targetValFunc)
        {
            TwoByOne twoByOne = TwoByOne.GetInstance(rowList, row =>
            {
                int targetVal = targetValFunc(row);
                Helper.CheckCondition(targetVal == 0 || targetVal == 1, "Expect the target to be 0 or 1");
                return targetVal == 1;
            });
            return twoByOne;
        }
    }


    public class ResultsRow
    {
        public string KeyListAsString;
        public Dictionary<string, KeyValuePair<double, double>> CidGroupToZ0AndPValue;
        public double ComboPValue;
        static public string ToHeaderString(List<string> keyListHeaders, List<string> cidGroupList)
        {
            return Helper.CreateTabString(
                keyListHeaders.StringJoin("\t"),
                cidGroupList.Select(cidGroup => "p" + cidGroup).StringJoin("\t"),
                "pCombo");
        }
        public string ToString(List<string> cidGroupList)
        {
            throw new NotImplementedException("can't get Carl's code to compile");
            //return Helper.CreateTabString(KeyListAsString,
            //    cidGroupList.Select(cidGroup => CidGroupToZ0AndPValue.GetValueOrDefault(cidGroup, new KeyValuePair<double, double>(double.NaN, double.NaN),
            //                        insertIfMissing:false).Value).StringJoin("\t"),
            //    ComboPValue);
        }

    }

    public class InputRow
    {
        //public int SplitIndex { get; set; }
        public List<string> KeyList { get; set; }
        public string CidGroup { get; set; }
        public string Cid { get; set; }
        public Double PTarget { get; set; }
        public int TargetVal { get; set; }
    }
}
