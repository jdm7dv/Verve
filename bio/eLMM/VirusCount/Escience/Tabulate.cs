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
using System.IO;
using System.Linq;
using Bio.Util;
using MBT.Escience.Matrix;
using Bio.Matrix;

namespace MBT.Escience
{
    public static class Tabulate
    {

        public const string STOREY_METHOD_NAME = "StoreyTibshirani";
        public const string STOREY_METHOD_NAME_LOWER = "storeytibshirani";  // useful for switch statements

        private static COL_TO_TABULATE _columnToTabulate; //added so can tabulate "TestStatistic" or "PValue"
        public enum COL_TO_TABULATE
        {
            PVALUE,
            TESTSTATISTIC,
            NULL
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns>bool indicating successful tabulate. False indicates the audit failed, in which case the outputFileName will be used
        ///// to create a skip file.</returns>
        //public static bool CreateTabulateReport(DirectoryInfo dirinfo, ICollection<string> inputFilePatternCollection, string outputFileName,
        //    KeepTest<Dictionary<string, string>> globalKeepTest, double maxPValue, bool auditRowIndexValues, bool useStoreyTibsharaniMethod)
        //{
        //    return CreateTabulateReport(dirinfo, inputFilePatternCollection, outputFileName, globalKeepTest, new List<KeepTest<Dictionary<string,string>>>(), 
        //        maxPValue, auditRowIndexValues, useStoreyTibsharaniMethod);
        //}


        public static bool CreateTabulateReport(DirectoryInfo dirinfo, string inputFilePattern, string outputFileName,
           KeepTest<Dictionary<string, string>> globalKeepTest, List<KeepTest<Dictionary<string, string>>> splitKeepTestList, double maxPValue,
           bool auditRowIndexValues, bool useStoreyTibsharaniMethod)
        {
            return CreateTabulateReport(dirinfo, SpecialFunctions.CreateSingletonList(inputFilePattern), outputFileName, globalKeepTest, splitKeepTestList, maxPValue, auditRowIndexValues, useStoreyTibsharaniMethod);
        }

        public static bool CreateTabulateReport(DirectoryInfo dirinfo, ICollection<string> inputFilePatternCollection, string outputFileName,
                   KeepTest<Dictionary<string, string>> globalKeepTest, List<KeepTest<Dictionary<string, string>>> splitKeepTestList, double maxPValue,
                   bool auditRowIndexValues, bool useStoreyTibsharaniMethod)
        {
            int numTestsStoreyTibsOverride = -1;
            return CreateTabulateReport(dirinfo, inputFilePatternCollection, outputFileName, globalKeepTest, splitKeepTestList, maxPValue, auditRowIndexValues, useStoreyTibsharaniMethod, numTestsStoreyTibsOverride);
        }

        public static bool CreateTabulateReport(DirectoryInfo dirinfo, ICollection<string> inputFilePatternCollection, string outputFileName,
                          KeepTest<Dictionary<string, string>> globalKeepTest, List<KeepTest<Dictionary<string, string>>> splitKeepTestList, double maxPValue,
                          bool auditRowIndexValues, bool useStoreyTibsharaniMethod, bool doLocalTabulation)
        {
            int numTestsStoreyTibsOverride = -1;
            return CreateTabulateReport(dirinfo, inputFilePatternCollection, outputFileName, globalKeepTest, splitKeepTestList, maxPValue, auditRowIndexValues, useStoreyTibsharaniMethod, numTestsStoreyTibsOverride, doLocalTabulation);

        }

        public static bool CreateTabulateReport(DirectoryInfo dirinfo, ICollection<string> inputFilePatternCollection, string outputFileName,
                   KeepTest<Dictionary<string, string>> globalKeepTest, List<KeepTest<Dictionary<string, string>>> splitKeepTestList, double maxPValue,
                   bool auditRowIndexValues, bool useStoreyTibsharaniMethod, int numTestsStoreyTibsOverride)
        {
            bool doLocalTabulation = false;
            return CreateTabulateReport(dirinfo, inputFilePatternCollection, outputFileName, globalKeepTest, splitKeepTestList, maxPValue,
                                        auditRowIndexValues, useStoreyTibsharaniMethod, numTestsStoreyTibsOverride, doLocalTabulation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>bool indicating successful tabulate. False indicates the audit failed, in which case the outputFileName will be used</returns>
        public static bool CreateTabulateReport(DirectoryInfo dirinfo, ICollection<string> inputFilePatternCollection, string outputFileName,
            KeepTest<Dictionary<string, string>> globalKeepTest, List<KeepTest<Dictionary<string, string>>> splitKeepTestList, double maxPValue,
            bool auditRowIndexValues, bool useStoreyTibsharaniMethod, int numTestsStoreyTibsOverride, bool doLocalTabulation)
        {

            using (TextWriter textWriter = File.CreateText(outputFileName)) // Do this early so that if it fails, well know
            {
                int splitCount = splitKeepTestList.Count + 1;
                List<KeyValuePair<Dictionary<string, string>, double>>[] realRowCollectionToSortArray = new List<KeyValuePair<Dictionary<string, string>, double>>[splitCount];
                //List<double>[] nullValueCollectionToBeSortedArray = new List<double>[splitCount];
                Dictionary<int, List<double>>[] nullValueCollectionToBeSortedArray = new Dictionary<int, List<double>>[splitCount];
                int[] totalPValueCount = new int[splitCount];

                for (int i = 0; i < splitCount; i++)
                {
                    realRowCollectionToSortArray[i] = new List<KeyValuePair<Dictionary<string, string>, double>>(10000);
                    //nullValueCollectionToBeSortedArray[i] = new List<double>(10000);
                    nullValueCollectionToBeSortedArray[i] = new Dictionary<int, List<double>>();
                }

                string headerSoFar = null;

                Set<int> broadRealAndNullIndexSetSoFar = null;

                foreach (string broadInputFilePattern in inputFilePatternCollection)
                {
                    Set<int> narrowRealAndNullIndexSetSetSoFar = Set<int>.GetInstance();

                    foreach (string narrowInputFilePattern in broadInputFilePattern.Split('+'))
                    {
                        Set<int> realAndNullIndexSet;
                        RowIndexTabulator tabulator = TryCreateTabulateReportInternal(out realAndNullIndexSet, dirinfo, narrowInputFilePattern,
                            globalKeepTest, splitKeepTestList, maxPValue, auditRowIndexValues, useStoreyTibsharaniMethod,
                            ref realRowCollectionToSortArray, ref nullValueCollectionToBeSortedArray, ref totalPValueCount, ref headerSoFar, doLocalTabulation);
                        if (!tabulator.IsComplete())
                        {
                            textWriter.WriteLine(tabulator.GetSkipRangeCollection());
                            Console.WriteLine("Not all needed rows were found in {0}.", narrowInputFilePattern);
                            Console.WriteLine("Found rows:\n{0}", tabulator.GetSkipRangeCollection());
                            Console.WriteLine("{0} created as skip file.", outputFileName);
                            return false;
                        }



                        //Instead of throwing an error, we could filter out the duplicated null indexes
                        Helper.CheckCondition(narrowRealAndNullIndexSetSetSoFar.IntersectionIsEmpty(realAndNullIndexSet),
                            string.Format("Within inputFilePattern {0}, multiple '+'-connected parts cover the same nullIndex(s), {1}",
                            broadInputFilePattern,
                            narrowRealAndNullIndexSetSetSoFar.Intersection(realAndNullIndexSet)));

                        narrowRealAndNullIndexSetSetSoFar.AddNewRange(realAndNullIndexSet);
                    }

                    Helper.CheckCondition(!auditRowIndexValues || narrowRealAndNullIndexSetSetSoFar.Contains(-1),
                        string.Format("The 'null' index -1 for the real data was not seen in {0}", broadInputFilePattern));


                    if (broadRealAndNullIndexSetSoFar == null)
                    {
                        broadRealAndNullIndexSetSoFar = narrowRealAndNullIndexSetSetSoFar;
                    }
                    //else
                    //{
                    //	Helper.CheckCondition(broadRealAndNullIndexSetSoFar.Equals(narrowRealAndNullIndexSetSetSoFar),
                    //		string.Format("The broad inputFilePattern {0} covers a different set of nullIndexes ({1}) than its predecessors ({2})",
                    //		broadInputFilePattern, narrowRealAndNullIndexSetSetSoFar, broadRealAndNullIndexSetSoFar));
                    //}

                }

                double numberOfRandomizationRuns = useStoreyTibsharaniMethod ? 0 : broadRealAndNullIndexSetSoFar.Count - 1;
                Console.WriteLine("Detected {0} randomized runs relative to the number of real runs.", numberOfRandomizationRuns);
                Helper.CheckCondition<InvalidDataException>(useStoreyTibsharaniMethod || numberOfRandomizationRuns > 0, "No randomization runs detected. Did you mean to include a -{0} flag?", Tabulate.STOREY_METHOD_NAME);

                //Compute q-values from p-values (and p-values from test statistic)
                List<KeyValuePair<Dictionary<string, string>, double>> rowAndQValues = new List<KeyValuePair<Dictionary<string, string>, double>>(1000);
                Dictionary<double, double> rowToPvalFromRandomizations = null;
                for (int i = 0; i < splitCount; i++)
                {

                    int numTestsToUse;
                    if (numTestsStoreyTibsOverride != -1)
                    {
                        Console.WriteLine("Using " + numTestsStoreyTibsOverride + " p-values for computation of q-values rather than the observed number (" + totalPValueCount[i] + ")");
                        numTestsToUse = numTestsStoreyTibsOverride;
                    }
                    else
                    {
                        numTestsToUse = totalPValueCount[i];
                    }


                    //List<double> placeFiller = nullValueCollectionToBeSortedArray[i][0];

                    Dictionary<Dictionary<string, string>, double> qValueList;
                    if (useStoreyTibsharaniMethod)
                    {
                        qValueList = SpecialFunctions.ComputeQValuesUseStoreyTibsharani(ref realRowCollectionToSortArray[i], row => row.Value, numTestsToUse)
                                            .ToDictionary(entry => entry.Key.Key, entry => entry.Value);
                    }
                    else if (!doLocalTabulation)
                    {
                        qValueList = SpecialFunctions.ComputeQValuesUseNulls(ref realRowCollectionToSortArray[i],
                                 row => row.Value,
                                 row => int.Parse(((KeyValuePair<System.Collections.Generic.Dictionary<string, string>, double>)row).Key["groupId"]),
                                 row => int.Parse(((KeyValuePair<System.Collections.Generic.Dictionary<string, string>, double>)row).Key["rowIndex"]),
                                 ref nullValueCollectionToBeSortedArray[i], numberOfRandomizationRuns, out rowToPvalFromRandomizations, doLocalTabulation)
                                 .ToDictionary(entry => entry.Key.Key, entry => entry.Value);
                    }
                    else//do local tabulation
                    {
                        qValueList = SpecialFunctions.ComputeQValuesUseNulls(ref realRowCollectionToSortArray[i],
                                 row => row.Value,
                                 row => int.Parse(((KeyValuePair<System.Collections.Generic.Dictionary<string, string>, double>)row).Key["groupId"]),
                                 row => int.Parse(((KeyValuePair<System.Collections.Generic.Dictionary<string, string>, double>)row).Key["rowIndex"]),
                                 ref nullValueCollectionToBeSortedArray[i], numberOfRandomizationRuns, out rowToPvalFromRandomizations, doLocalTabulation)
                                 .ToDictionary(entry => entry.Key.Key, entry => entry.Value);
                    }

                    //Dictionary<Dictionary<string, string>, double> qValueList =
                    //    (useStoreyTibsharaniMethod ?
                    //        SpecialFunctions.ComputeQValuesUseStoreyTibsharani(ref realRowCollectionToSortArray[i], row => row.Value, numTestsToUse) :
                    //        SpecialFunctions.ComputeQValuesUseNulls(ref realRowCollectionToSortArray[i], row => row.Value,
                    //                ref nullValueCollectionToBeSortedArray[i], numberOfRandomizationRuns,out pValToPvalFromRandomizations))
                    //    .ToDictionary(entry => entry.Key.Key, entry => entry.Value);

                    foreach (KeyValuePair<Dictionary<string, string>, double> rowAndQValue in qValueList)
                    {
                        rowAndQValues.Add(new KeyValuePair<Dictionary<string, string>, double>(rowAndQValue.Key, rowAndQValue.Value));
                    }
                }

                rowAndQValues.Sort((row1, row2) =>
                    row1.Value == row2.Value ?
                    AccessPValueFromPhylotreeRow(row1.Key).CompareTo(AccessPValueFromPhylotreeRow(row2.Key)) :
                    row1.Value.CompareTo(row2.Value));

                //!!!this code is repeated elsewhere
                if (COL_TO_TABULATE.TESTSTATISTIC == _columnToTabulate)
                {
                    Helper.CheckCondition(!useStoreyTibsharaniMethod, "the way its set up now, cannot use TestStatistic column with useStoreyTibshirani");
                    textWriter.WriteLine(Helper.CreateTabString(headerSoFar, "pValFromRandomizations", "qValue"));
                }
                else
                {
                    textWriter.WriteLine(Helper.CreateTabString(headerSoFar, "qValue"));
                }
                //foreach (Dictionary<string, string> row in realRowCollectionToSortArray)
                //{
                //    double qValue = qValueList[row];
                //    textWriter.WriteLine(Helper.CreateTabString(row[""], qValue));
                //}
                foreach (KeyValuePair<Dictionary<string, string>, double> rowAndQValue in rowAndQValues)
                {
                    if (COL_TO_TABULATE.TESTSTATISTIC == _columnToTabulate)
                    {
                        double thisRow = double.Parse(rowAndQValue.Key["rowIndex"]);
                        double thisPvalFromRandomization = rowToPvalFromRandomizations[thisRow];
                        textWriter.WriteLine(Helper.CreateTabString(rowAndQValue.Key[""], thisPvalFromRandomization, rowAndQValue.Value));
                    }
                    else
                    {
                        textWriter.WriteLine(Helper.CreateTabString(rowAndQValue.Key[""], rowAndQValue.Value));
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Merges all the files in dirinfo matching inputFilePattern and places them in mergedFileName. If MergedFileName exists, will append to the end. Then attempts to 
        /// tabulate with the given parameters. If it fails, then it will places the workitems that it found in completedRowsFileName in the form a range, which can be used as a skipfile.
        /// If successful, then it deletes completedRowsFileName, if it exists. This is useful for deleting previous skip files.
        /// </summary>
        /// <param name="dirinfo">location where all the files can be found.</param>
        /// <returns>True if tabulate was successful, false otherwise.</returns>
        public static bool MergeThenTabulateOrCreateSkipFile(DirectoryInfo dirinfo, string inputFilePattern, string mergedFileName, string tabulateResultFileName,
            string skipFileName, KeepTest<Dictionary<string, string>> keepTest, List<KeepTest<Dictionary<string, string>>> splitKeepTestList, double maxPForTabulate, bool useStoreyMethod)
        {
            Console.Write("Merging files...");
            Tabulate.MergeFilesUsedToTabulate(dirinfo, inputFilePattern, mergedFileName, true);
            Console.WriteLine("done merging.");


            bool tabulated = Tabulate.CreateTabulateReport(
                dirinfo,
                mergedFileName,
                tabulateResultFileName,
                keepTest,
                splitKeepTestList,
                maxPForTabulate, true /* audit */, useStoreyMethod);

            if (tabulated)
            {
                //string skipFileName = completedRowsFileName.Replace("completedRows", "skipFile");
                //File.Delete(completedRowsFileName);	// at this point we know everything's done, so delete it.
                if (File.Exists(skipFileName))
                    File.Delete(skipFileName);

                return true;


            }
            else
            {
                Console.WriteLine("Tabulation failed. Missing rows placed in {0}.", skipFileName);
                SpecialFunctions.MoveAndReplace(tabulateResultFileName, skipFileName);
                return false;
            }
        }


        public static void MergeFilesUsedToTabulate(DirectoryInfo dirinfo, string inputFilePattern, string outputFileName, bool printProgress)
        {
            MergeFilesUsedToTabulate(dirinfo, SpecialFunctions.CreateSingletonList(inputFilePattern), outputFileName, printProgress);
        }

        public static void MergeFilesUsedToTabulate(DirectoryInfo dirinfo, ICollection<string> inputFilePatternCollection, string outputFileName, bool printProgress)
        {
            FileOperator fileFlags = FileOperator.DeleteFiles | FileOperator.ContinueOnError | FileOperator.Append;
            if (printProgress)
                fileFlags |= FileOperator.PrintProgress;

            MBT.Escience.FileUtils.MergeFiles(dirinfo, EnumerateFilePatterns(dirinfo, inputFilePatternCollection), new string[0], outputFileName, fileFlags);
        }


        private static IEnumerable<FileInfo> EnumerateFiles(DirectoryInfo dirinfo, ICollection<string> inputFilePatternCollection)
        {
            foreach (string narrowInputFilePattern in EnumerateFilePatterns(dirinfo, inputFilePatternCollection))
            {
                foreach (FileInfo fileinfo in dirinfo.GetFiles(narrowInputFilePattern))
                {
                    yield return fileinfo;
                }
            }
        }


        private static IEnumerable<string> EnumerateFilePatterns(DirectoryInfo dirinfo, ICollection<string> inputFilePatternCollection)
        {
            foreach (string broadInputFilePattern in inputFilePatternCollection)
            {
                foreach (string narrowInputFilePattern in broadInputFilePattern.Split('+'))
                {
                    yield return narrowInputFilePattern;
                }
            }
        }


        public static double AccessPValueFromPhylotreeRow(Dictionary<string, string> row)
        {
            string testStatisticOrPvalue;
            bool hasPvalueColumn;
            bool hasTestStatisticColumn = false;
            hasPvalueColumn = row.TryGetValue("PValue", out testStatisticOrPvalue);
            if (hasPvalueColumn)
            {
                _columnToTabulate = COL_TO_TABULATE.PVALUE;
            }
            else
            {
                hasTestStatisticColumn = row.TryGetValue("TestStatistic", out testStatisticOrPvalue);
                if (hasTestStatisticColumn) _columnToTabulate = COL_TO_TABULATE.TESTSTATISTIC;
            }
            if (!hasPvalueColumn && !hasTestStatisticColumn) throw new Exception(@"The header must contain ""PValue"" or ""TestStatistic"" ");

            return double.Parse(testStatisticOrPvalue);
        }


        /// <summary>
        /// currently hard-coded to map the rowId to a groupId, used for localTabulation, by using hypothesisId = rowId % numRealHypotheses
        /// </summary>
        /// <param name="nullIndexSet"></param>
        /// <param name="dirinfo"></param>
        /// <param name="inputFilePattern"></param>
        /// <param name="globalKeepTest"></param>
        /// <param name="splitKeepTestList"></param>
        /// <param name="maxPValue"></param>
        /// <param name="auditRowIndexValues"></param>
        /// <param name="useStoreyTibsharaniMethod"></param>
        /// <param name="realRowCollectionToSortArray"></param>
        /// <param name="nullValueCollectionToBeSortedArrayDict"></param>
        /// <param name="totalPValueCount"></param>
        /// <param name="headerSoFar"></param>
        /// <param name="doLocalTabulationOfPermutationsToGetPvaluesFromRandomizations"></param>
        /// <returns></returns>
        private static RowIndexTabulator TryCreateTabulateReportInternal(out Set<int> nullIndexSet, DirectoryInfo dirinfo,
            string inputFilePattern,
            KeepTest<Dictionary<string, string>> globalKeepTest,
            List<KeepTest<Dictionary<string, string>>> splitKeepTestList,
            double maxPValue,
            bool auditRowIndexValues,
            bool useStoreyTibsharaniMethod,
            ref List<KeyValuePair<Dictionary<string, string>, double>>[] realRowCollectionToSortArray,
            ref Dictionary<int, List<double>>[] nullValueCollectionToBeSortedArrayDict,
            ref int[] totalPValueCount,
            ref string headerSoFar,
            bool doLocalTabulationOfPermutationsToGetPvaluesFromRandomizations
            )
        {
            //int splitCount=splitKeepTestList.Count;
            //List<double>[] nullValueCollectionToBeSortedArray = new List<double>[splitCount];
            //for (int j = 0; j < splitCount; j++) nullValueCollectionToBeSortedArray[j] = new List<double>();

            nullIndexSet = Set<int>.GetInstance();

            //!!!very similar code elsewhere
            RowIndexTabulator rowIndexTabulator = RowIndexTabulator.GetInstance(auditRowIndexValues);
            //RangeCollection unfilteredRowIndexRangeCollection = new RangeCollection();
            int lastWriteLineLength = 0;
            int nullValueCount = 0;
            foreach (FileInfo fileinfo in dirinfo.GetFiles(inputFilePattern))
            {
                try
                {
                    int sigLines = realRowCollectionToSortArray.Select(split => split.Count).Sum();
                    //nullValueCount = nullValueCollectionToBeSortedArray.Select(split => split.Count).Sum();
                    int totalLines = sigLines + nullValueCount + totalPValueCount.Sum();

                    string writeLine = string.Format("{0}/{1} lines have p<=1. Now reading {2}", sigLines, totalLines, fileinfo.FullName);
                    Console.Write("\r{0,-" + lastWriteLineLength + "}", writeLine);
                    lastWriteLineLength = writeLine.Length;

                    string headerOnFile;
                    using (TextReader reader = SpecialFunctions.GetTextReaderWithExternalReadWriteAccess(fileinfo.FullName))
                    {
                        headerOnFile = reader.ReadLine();
                        if (headerSoFar == null)
                        {
                            headerSoFar = headerOnFile;
                        }
                        else if (headerSoFar != headerOnFile)
                        {
                            Console.WriteLine("Warning: The header for file {0} is different from the 1st file read in", fileinfo.Name);
                        }
                    }

                    //KeepAa2AaOnly keepAa = KeepAa2AaOnly.GetInstance();
                    //Console.WriteLine(keepAa);

                    using (TextReader reader = SpecialFunctions.GetTextReaderWithExternalReadWriteAccess(fileinfo.FullName))
                    {
                        foreach (Dictionary<string, string> row in SpecialFunctions.TabFileTable(reader, headerOnFile, /*includeWholeLine*/ true))
                        {
                            if (rowIndexTabulator.TryAdd(row, fileinfo.FullName) && globalKeepTest.Test(row))
                            {
                                //Helper.CheckCondition(row.ContainsKey(NullIndexColumnName), string.Format(@"When tabulating a ""{0}"" column is required. (File ""{1}"")", NullIndexColumnName, fileinfo.Name));

                                //int nullIndex = int.Parse(row[NullIndexColumnName]);
                                int nullIndex = !row.ContainsKey(NullIndexColumnName) && useStoreyTibsharaniMethod ? -1 : int.Parse(row[NullIndexColumnName]);
                                nullIndexSet.AddNewOrOld(nullIndex);

                                double pValue = AccessPValueFromPhylotreeRow(row);
                                if (useStoreyTibsharaniMethod && nullIndex == -1)
                                {
                                    int splitIdx = GetSplitTabulateIndex(row, splitKeepTestList);
                                    if (pValue <= maxPValue)
                                    {
                                        realRowCollectionToSortArray[splitIdx].Add(new KeyValuePair<Dictionary<string, string>, double>(row, pValue));
                                    }
                                    //nullValueCollectionToBeSortedArray[splitIdx].Add(pValue);
                                    totalPValueCount[splitIdx]++;
                                }
                                else if (!useStoreyTibsharaniMethod)
                                {
                                    if (pValue <= maxPValue)
                                    {
                                        int splitIdx = GetSplitTabulateIndex(row, splitKeepTestList);
                                        if (nullIndex == -1)
                                        {
                                            realRowCollectionToSortArray[splitIdx].Add(new KeyValuePair<Dictionary<string, string>, double>(row, pValue));
                                            //realRowCollectionToSortArray[splitIdx].Add(row);
                                        }
                                        else
                                        {
                                            int groupId;
                                            if (!doLocalTabulationOfPermutationsToGetPvaluesFromRandomizations)
                                            {
                                                //always add it to the zero key if not doing local tabulations
                                                groupId = 0;
                                            }
                                            else
                                            {
                                                groupId = int.Parse(row[GroupIdColumnName]);
                                            }

                                            nullValueCollectionToBeSortedArrayDict[splitIdx].GetValueOrDefault(groupId).Add(pValue);
                                            nullValueCount++;
                                            //nullValueCollectionToBeSortedArray[splitIdx].Add(pValue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("\nFailure parsing {0}.", fileinfo.Name);
                    throw;
                }
            }

            Console.WriteLine("\r{0,-" + lastWriteLineLength + "}", "Read all files.");
            return rowIndexTabulator;
            //rowIndexTabulator.CheckIsComplete(inputFilePattern);

            //return nullIndexSet;
        }

        private static int GetSplitTabulateIndex(Dictionary<string, string> row, List<KeepTest<Dictionary<string, string>>> splitKeepTestList)
        {
            // assume mutual exclusivity of keep tests. return idx in [0,splitKeepTestList.count+1], so that if row doesn't 
            // match any of the keep tests, it goes in the "+1" bin. If splitKeeptestList is empty, return 0 (no split)
            int idx = 0;
            for (; idx < splitKeepTestList.Count && !splitKeepTestList[idx].Test(row); idx++) { }
            return idx;
        }

        /// <summary>
        /// Given a binary matrix, returns a mapping of multistateKeys to lists of binary keys. Keys are assumed to be sequences of the form
        /// pos@AA or AA@pos. All other keys will be left as binary. Each item is the multistateKey of the format pos@aa1#aa2#...#aaN. Each value is
        /// an enumeration of binary keys that map to that multistate key.
        /// </summary>
        /// <param name="binaryMatrix"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, IEnumerable<string>>> BinaryToMultistateMapping(Matrix<string, string, SufficientStatistics> binaryMatrix)
        {
            var multToBinaryPositions = from binaryKey in binaryMatrix.RowKeys
                                        where binaryKey.Contains('@')
                                        let merAndPos = Tabulate.GetMerAndPos(binaryKey)
                                        let pos = (int)merAndPos.Value
                                        group binaryKey by pos into g
                                        select new KeyValuePair<string, IEnumerable<string>>
                                        (
                                             g.Key + "@" + g.OrderBy(bk => bk).StringJoin("#").Replace(g.Key + "@", ""),
                                             g.ToList()
                                        );

            var nonAaKeys = from key in binaryMatrix.RowKeys
                            where !key.Contains('@')
                            select new KeyValuePair<string, IEnumerable<string>>(key, new List<string>() { key });

            var allKeys = nonAaKeys.Concat(multToBinaryPositions);

            return allKeys;
        }

        /// <summary>
        /// Given a multistate matrix, returns a mapping of multistateKeys to lists of binary keys. Keys are assumed to be sequences of the form
        /// pos@aa1#aa2#...#aaN.. Each item is the multistateKey of the format pos@aa1#aa2#...#aaN. Each value is
        /// an enumeration of binary keys that map to that multistate key.
        /// </summary>
        /// <param name="binaryMatrix"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<string, IEnumerable<string>>> MultistateToBinaryMapping(Matrix<string, string, SufficientStatistics> multistateMatrix)
        {
            var multToBinaryPositions = from multKey in multistateMatrix.RowKeys
                                        where multKey.Contains('@')
                                        let merAndPos = Tabulate.GetMerAndPos(multKey)
                                        let binaryKeys = merAndPos.Key.Split('#').Select(aa => (int)merAndPos.Value + "@" + aa)
                                        select new KeyValuePair<string, IEnumerable<string>>(multKey, binaryKeys);

            var nonAaKeys = from key in multistateMatrix.RowKeys
                            where !key.Contains('@')
                            select new KeyValuePair<string, IEnumerable<string>>(key, new List<string>() { key });

            var allKeys = nonAaKeys.Concat(multToBinaryPositions);

            return allKeys;


            //var temp = multistateMatrix.RowKeys.Select(key =>
            //    Tabulate.GetMerAndPos(key)).Select(merAndPos =>
            //        new KeyValuePair<string, IEnumerable<string>>(merAndPos.Value + "@" + merAndPos.Key,
            //        merAndPos.Key.Split('#').Select(aa => (int)merAndPos.Value + "@" + aa)));
        }

        static public KeyValuePair<string, double> GetMerAndPos(string variableName)
        {
            KeyValuePair<string, double> merAndPos;
            if (!TryGetMerAndPos(variableName, out merAndPos))
            {
                throw new ArgumentException("Cannot parse " + variableName + " into merAndPos and pos.");
            }
            return merAndPos;
        }

        static public bool TryGetMerAndPos(string variableName, out KeyValuePair<string, double> merAndPos)
        {
            string[] fields = variableName.Split('@');
            double pos = -1;
            string mer;

            int posField = -1;

            // find pos and the field that describes it.
            while (++posField < fields.Length && !double.TryParse(fields[posField], out pos)) ;

            if (posField == fields.Length)  // got to the end without finding the pos.
            {
                merAndPos = new KeyValuePair<string, double>();
                return false;
            }

            // arbitrarily choose one of the non-posFields to be the mer. This is really only designed to work with mer@pos or pos@mer.
            // if we have str1@str2@pos@str3, then we make no guarantees as to what will be called mer, except that it won't be pos.

            mer = fields[fields.Length - posField - 1];

            merAndPos = new KeyValuePair<string, double>(mer, pos);
            return true;

            //if (int.TryParse(fields[0], out pos))
            //{
            //	mer = fields[1];
            //}
            //else if (int.TryParse(fields[1], out pos))
            //{
            //	mer = fields[0];
            //}
            //else
            //{
            //	throw new ArgumentException("Cannot paris " + variableName + " into mer and pos.");
            //}
        }

        static public string NullIndexColumnName = "NullIndex";
        static public string PredictorVariableColumnName = "PredictorVariable";
        static public string ConditioningPredictorVariablesColumnName = "ConditioningPredictorVariables";
        static public string PredictorTrueNameCountColumnName = "PredictorTrueCount";
        static public string PredictorFalseNameCountColumnName = "PredictorFalseCount";
        static public string PredictorFalseNameCountBeforeTreeColumnName = "PredictorFalseCountBeforeTree"; //!!!Used in the PhyloTreeGauss regression test
        static public string PredictorNonMissingCountColumnName = "PredictorNonMissingCount";
        static public string TargetVariableColumnName = "TargetVariable";
        static public string TargetTrueNameCountColumnName = "TargetTrueCount";
        static public string TargetFalseNameCountColumnName = "TargetFalseCount";
        static public string TargetNonMissingCountColumnName = "TargetNonMissingCount";
        static public string GlobalNonMissingCountColumnName = "GlobalNonMissingCount";
        static public string PValueColumnName = "PValue";
        static public string QValueColumnName = "QValue";
        static public string RowIndexColumnName = "rowIndex";
        static public string GroupIdColumnName = "groupId";


    }
}

