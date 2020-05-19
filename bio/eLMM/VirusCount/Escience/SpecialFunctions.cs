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
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq;



#if !SILVERLIGHT
using System.Drawing;
using System.IO.Compression;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Numerics;
#else
using System.Windows;
#endif

using System.Threading.Tasks;
using MBT.Escience.Parse;
using Bio.Matrix;
using Bio.Util;


namespace MBT.Escience
{
    /// <summary>
    /// Summary description for SpecialFunctions.
    /// </summary>
    /// 
    public class SpecialFunctions
    {

        public static bool IsInDescendingOrder(List<double> myList)
        {
            for (int i = 1; i < myList.Count; i++)
            {
                bool isSmaller = myList[i] < myList[i - 1];
                if (!isSmaller)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Create a list of pairs of all (unordered combinations of elements in myList).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myList"></param>
        /// <returns></returns>
        public static List<Pair<T, T>> GetAllPairs<T>(List<T> myList)
        {
            List<Pair<T, T>> pairs = new List<Pair<T, T>>();
            for (int i = 0; i < myList.Count; i++)
            {
                for (int j = i + 1; j < myList.Count; j++)
                {
                    pairs.Add(new Pair<T, T>(myList[i], myList[j]));
                }
            }
            return pairs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="sortedInd">x(sortedInd)=sortedVals</param>
        /// <returns>sortedVals</returns>
        public static List<TVal> SortWithInd<TVal>(IList<TVal> x, out List<int> sortedInd)
        {
            TVal[] tmp1 = x.ToArray();
            int[] tmp2 = Enumerable.Range(0, x.Count).ToArray();
            Array.Sort(tmp1, tmp2);
            sortedInd = tmp2.ToList();
            List<TVal> sortedVals = tmp1.ToList();
            return sortedVals;
        }

        /// <summary>
        /// Sorts the data, but also gives you indexes to undo the sort
        /// </summary>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="listInput"></param>
        /// <param name="sortedInd">x(sortedInd)=sortedVals</param>
        /// <param name="sortedInd2">sortedVals(sortedInd2)=x</param>
        /// <returns>sort(listInput)</returns>
        public static List<TVal> SortWithInd<TVal>(List<TVal> listInput, out List<int> sortedInd, out List<int> sortedInd2)
        {
            List<TVal> list = SortWithInd(listInput, out sortedInd);
            List<int> garb = SortWithInd(sortedInd, out sortedInd2);
            return list;
        }

        /// <summary>
        /// Compute the rank of each object in the list, using the average rank for those values that are tied.
        /// </summary>
        /// <returns></returns>
        public static List<double> RanksWithTies(List<double> listInput)
        {
            List<int> garb, sortedInd;
            List<double> list = SortWithInd(listInput, out garb, out sortedInd);

            int N = list.Count;
            double[] ranksWithTies = new double[N];
            double lastVal = double.NaN;

            double currentRank = 1;

            List<double> ranksForThisVal = new List<double>();
            List<int> indexesForThisVal = new List<int>();
            double averageRank = double.NaN;

            for (int i = 0; i < N; i++)
            {
                double thisVal = list[i];
                Helper.CheckCondition(!double.IsNaN(thisVal));
                if ((thisVal == lastVal) || (ranksForThisVal.Count == 0))
                {
                    ranksForThisVal.Add(currentRank++);
                    indexesForThisVal.Add(i);
                }
                else//tally up the previous ranks and reset
                {
                    averageRank = ranksForThisVal.Average();
                    foreach (int idx in indexesForThisVal)
                    {
                        ranksWithTies[idx] = averageRank;
                    }

                    ranksForThisVal = new List<double>();
                    indexesForThisVal = new List<int>();
                    ranksForThisVal.Add(currentRank++);
                    indexesForThisVal.Add(i);
                }
                lastVal = thisVal;
            }
            averageRank = ranksForThisVal.Average();
            foreach (int idx in indexesForThisVal)
            {
                ranksWithTies[idx] = averageRank;
            }
            Helper.CheckCondition(ranksWithTies.Sum() == (long)N * (N + 1) / 2, "ranks do not add up");
            return ranksWithTies.ToList().SubList(sortedInd.ToList());
        }

#if !SILVERLIGHT

        static public IEnumerable<List<KeyValuePair<double, int>>> Permute01Targets<T>(IList<T> inputList, Func<T, double> scoreFunc, Func<T, int> zeroOneFunction, int sampleThreshold)
        {
            int randomSeed = -34359393;//was parameter in original version
            Helper.CheckCondition(inputList.Count > 0, "Must have some items in the list"); //Count just yeild break
            //find # of 0's and 1's
            var labelToCount =
                (from row in inputList
                 let zeroOne = zeroOneFunction(row)
                 group zeroOne by zeroOne into g
                 let entry = new { key = g.Key, count = g.Count() }
                 select entry).ToDictionary(keyAndCount => keyAndCount.key, keyAndCount => keyAndCount.count);

            Helper.CheckCondition(labelToCount.Keys.ToHashSet().IsSubsetOf(new int[] { 0, 1 }), "expect labels of 0 or 1");
            int zeroCount = labelToCount.GetValueOrDefault(0,/*insertIfMissing*/ false);
            int oneCount = labelToCount.GetValueOrDefault(1,/*insertIfMissing*/ false);
            Helper.CheckCondition(zeroCount + oneCount == inputList.Count, "Real assert: 0's and 1's miscounted");

            BigInteger exactPermutationCount = Factorial(zeroCount + oneCount) / (Factorial(zeroCount) * Factorial(oneCount));

            if (exactPermutationCount <= sampleThreshold)
            {
                //get it exactly right now
                exactPermutationCount = Factorial(zeroCount + oneCount) / (Factorial(zeroCount) * Factorial(oneCount));

                //We think of the 0's sitting in a line zeroCount long.
                //We then insert 1's before, between, and/or after the 0's.
                //There are zeroCount+1 places to insert a 1. There are oneCount 1's to insert. So this is like putting oneCount
                // marbles into zeroCount+1 urns.
                //We start by inserting all the 1's in the first urn
                //We then move out of the first urn, into the second urn.
                //      If the first urn is empty, then we find the first nonempty urn, X
                //                move one marble from X to X+1 and move the rest back to 1.
                //The total # of permunations is (zeroCount+oneCount)!/(zeroCount! x oneZount!)

                int permutationIndex = 0;
                int[] urns = new int[zeroCount + 1];
                urns[0] = oneCount;
                while (true)
                {
                    var result = CreateResultFromUrns<T>(inputList.ToList(), scoreFunc, zeroCount, oneCount, urns);
                    yield return result;
                    ++permutationIndex;

                    if (!TryToMoveMarbleToNextConfiguration(urns))
                    {
                        break; //leave the while
                    }
                }
                Helper.CheckCondition(permutationIndex == exactPermutationCount, "Real assert: PermuteExactly01Targets had wrong number of permutations");
            }
            else
            {
                /*this is now a dead branch, more efficient code exists one level up*/
                Random random = new Random(randomSeed);
                for (int sampleIndex = 0; sampleIndex < sampleThreshold; ++sampleIndex)
                {
                    List<int> zeroOneList = Enumerable.Repeat(0, zeroCount).Concat(Enumerable.Repeat(1, oneCount)).Shuffle(random);

                    var result = new List<KeyValuePair<double, int>>(zeroCount + oneCount);
                    for (int i = 0; i < zeroOneList.Count; ++i)
                    {
                        var pairWithOne = new KeyValuePair<double, int>(scoreFunc(inputList[i]), zeroOneList[i]);
                        result.Add(pairWithOne);
                    }
                    yield return result;
                }
            }

        }
#endif

        public static Random GetRandFromSeedAndNullIndex(string seed, int nullIndex)
        {
            Random randObj = new MachineInvariantRandom(seed + nullIndex);
            return randObj;
        }

        public static string RemovePostfixFromFileName(string myFileName)
        {
            int postFixLength = 4;
            return myFileName.Substring(0, myFileName.Length - postFixLength);
        }

        public static string AddPostfixToFileName(string myFileName, string postfix)
        {
            Helper.CheckCondition(postfix != null, "postfix is null");
            int startIndex = myFileName.Length - 4;
            string newFileName = myFileName.Insert(startIndex, postfix);
            return newFileName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirinfo"></param>
        /// <param name="inputFilePattern"></param>
        /// <param name="outputFileName"></param>
        /// <param name="keepEveryKthValue">e.g. using 1000 means to keep one out of every 1000 p-values</param>
        /// <returns></returns>
        public static void ExtractPvaluesFromSpecialFileWithPonlyLines(DirectoryInfo dirinfo, string inputFilePattern, string outputFileName, int keepEveryKthValue, double countNumPvalsBelowThis)
        {
            List<FileInfo> filesToBeProcessed = new List<FileInfo>();
            FileInfo[] listOfFiles = dirinfo.EnumerateFiles(inputFilePattern).ToArray();
            Console.WriteLine("ExtractPvaluesFromSpecialFileWithPonlyLines inputFilePattern: " + inputFilePattern + "(found " + listOfFiles.Count() + " files)");

            Bio.Util.FileUtils.CreateDirectoryForFileIfNeeded(outputFileName);

            int pValCol = 7;
            List<double> windowOfAllowedPvalues = new List<double>(keepEveryKthValue);

            int fileNum = 0;
            int REPORT_FILE_FREQ = 500;

            int numLowPvals = 0;
            int numPvals = 0;

            Stopwatch st = new Stopwatch();
            st.Start();

            using (TextWriter textWriter = File.CreateText(outputFileName))
            {
                //textWriter.WriteLine("pVals");

                int numPrinted = 0;
                int numSkipped = 1;

                foreach (FileInfo fileinfo in listOfFiles)
                {
                    string thisFile = fileinfo.FullName;

                    Helper.CheckCondition(!thisFile.EndsWith("Tab.txt"), "found a Tab file");
                    Helper.CheckCondition(!thisFile.Contains("INFO"), "found a file with INFO in the name");

                    //filesToBeProcessed.Add(fileinfo); //don't need this right now
                    string nxtFile = thisFile;//dirinfo.FullName + @"\" + thisFile;

                    if (REPORT_FILE_FREQ != 0 && fileNum < 10 || ((fileNum % REPORT_FILE_FREQ) == 0))
                    {
                        st.Stop();
                        System.Console.WriteLine("file " + fileNum + " of " + listOfFiles.Count() + " " + fileinfo.Name + " time=" + st.Elapsed.ToString());
                        st.Start();
                    }
                    fileNum++;

                    using (TextReader textReader = File.OpenText(nxtFile))
                    {
                        double thisPval;
                        string header = textReader.ReadLine();
                        string line = null;
                        while (null != (line = textReader.ReadLine()))
                        {

                            string[] fields = line.Split('\t');
                            if (fields.Length == 1)
                            {
                                thisPval = double.Parse(fields[0]);
                            }
                            else
                            {
                                thisPval = double.Parse(fields[pValCol]);
                            }

                            numPvals++;
                            if (thisPval < countNumPvalsBelowThis) numLowPvals++;

                            bool printThisOne = false;

                            if (numSkipped == keepEveryKthValue)
                            {
                                printThisOne = true;
                            }
                            else
                            {
                                numSkipped++;
                            }

                            //textWriter.WriteLine("hello");
                            if (printThisOne)
                            {
                                //Console.WriteLine(thisPval);
                                textWriter.WriteLine(thisPval);
                                numPrinted++;
                                numSkipped = 1;
                            }
                        }

                    }
                }
            }
            Console.WriteLine("ExtractPvaluesFromSpecialFileWithPonlyLines inputFilePattern: " + inputFilePattern + "(found " + listOfFiles.Count() + " files)");
            Console.WriteLine("# lowPvalues found=" + numLowPvals + " of " + numPvals + " (pValThresh=" + countNumPvalsBelowThis);
        }


        public static List<List<T>> DivideListIntoEqualChunksFromNumChunks<T>(List<T> list, int numChunks)
        {
            int chunkSize = (int)Math.Floor((double)list.Count / numChunks);
            return DivideListIntoEqualChunksFromChunkSize(list, chunkSize);
        }

        /// <summary>
        /// Splits a <see cref="List{T}"/> into multiple chunks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list to be chunked.</param>
        /// <param name="chunkSize">The size of each chunk.</param>
        /// <returns>A list of chunks.</returns>
        public static List<List<T>> DivideListIntoEqualChunksFromChunkSize<T>(IList<T> list, int chunkSize)
        {
            if (chunkSize <= 0)
            {
                throw new ArgumentException("chunkSize must be greater than 0.");
            }

            List<List<T>> retVal = new List<List<T>>();
            int index = 0;
            while (index < list.Count)
            {
                int count = list.Count - index > chunkSize ? chunkSize : list.Count - index;
                retVal.Add(list.GetRange(index, count));

                index += chunkSize;
            }

            return retVal;
        }

        // !!! us myList.StringJoin("\t");
        //public static string WriteListToOneTabDelimitedLine<T>(List<T> myList)
        //{
        //    string result = "";
        //    for (int c = 0; c < myList.Count - 1; c++)
        //    {
        //        result += myList[c].ToString() + "\t";
        //    }
        //    result += myList[myList.Count - 1].ToString();
        //    result += "\n";
        //    return result;
        //}

        //public static void WriteListToOneTabDelimitedLine<T>(TextWriter textWriter, List<T> myList)
        //{
        //    for (int c = 0; c < myList.Count - 1; c++)
        //    {
        //        textWriter.Write(myList[c].ToString() + "\t");
        //    }
        //    textWriter.WriteLine(myList[myList.Count - 1].ToString());
        //}

        //public static void WriteListToOneTabDelimitedLineNoNewLine<T>(TextWriter textWriter, List<T> myList)
        //{
        //    for (int c = 0; c < myList.Count - 1; c++)
        //    {
        //        textWriter.Write(myList[c].ToString() + "\t");
        //    }
        //    textWriter.Write(myList[myList.Count - 1].ToString());
        //}

        public static List<T>[,] CreateArrayWithDefaultElements<T>(int numRows, int numCols)
        {
            List<T>[,] myArray = new List<T>[numRows, numCols];
            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    myArray[r, c] = new List<T>();
                }
            }
            return myArray;
        }

        /// <summary>
        /// If have HashSet objects use IsSubsetOf directly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns>firstListIsSubsetOfSecond</returns>
        public static bool IsSubsetOf<T>(List<T> t1, List<T> t2)
        {
            bool firstListIsSubsetOfSecond = !t1.Except(t2).Any();
            return firstListIsSubsetOfSecond;
        }


        public static List<T> CreateListWithInitElements<T>(int capacity, T initEl)
        {
            List<T> l = new List<T>(capacity);
            for (int i = 0; i < capacity; i++) l.Add(initEl);
            return l;
        }
        public static List<T> CreateListWithDefaultElements<T>(int capacity)
        {
            return CreateListWithInitElements(capacity, default(T));
        }


        /// <summary>
        /// Make a string out of the date---I often use this to postpend to a filename for an experiment
        /// </summary>
        /// <returns></returns>
        public static string GetDateStamp()
        {
            DateTime date = DateTime.Now;
            string dateTimeString = date.Year.ToString() + "_" + date.ToString("MM") + "_" + date.Day + "-" + date.ToString("HH_mm_ss");
            return dateTimeString;
        }

        public static double Log2Pi = Math.Log(2 * Math.PI);
        /*
         Given a contingency table 
                X=0	   X=1
        Y=0	  a		  b
        Y=1	  c		  d
        the Bayes score for a dependency between X and Y is
            gammaln(a+1) + gammaln(b+1) + gammaln(c+1) + gammaln(d+1) +gammln(a+b+c+d+4) 
            - gammaln(a+b+2) - gammaln(c+d+2) - gammaln(a+c+2) -gammaln(b+c+2) - gammaln(4).

         */
        static public double BayesScore(double a, double b, double c, double d)
        {
            double rBayesScore =
                        SpecialFunctions.LogGamma(a + 1)
                    + SpecialFunctions.LogGamma(b + 1)
                    + SpecialFunctions.LogGamma(c + 1)
                    + SpecialFunctions.LogGamma(d + 1)
                    + SpecialFunctions.LogGamma(a + b + c + d + 4)
                    - SpecialFunctions.LogGamma(a + b + 2)
                    - SpecialFunctions.LogGamma(c + d + 2)
                    - SpecialFunctions.LogGamma(a + c + 2)
                    - SpecialFunctions.LogGamma(b + d + 2)
                    - SpecialFunctions.LogGamma(4);

            return rBayesScore;

        }

        //static public double PData(double a, double b, double c, double d)

        /// <summary>
        /// The hypergeometric probability of a two by two table of counts: [[a b] [c d]]
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        static public double HypergeometricProbability(double a, double b, double c, double d)
        {
            double logr =
                      SpecialFunctions.LogGamma(a + 1)
                    + SpecialFunctions.LogGamma(b + 1)
                    + SpecialFunctions.LogGamma(c + 1)
                    + SpecialFunctions.LogGamma(d + 1)
                    + SpecialFunctions.LogGamma(a + b + c + d + 1)
                    - SpecialFunctions.LogGamma(a + b + 1)
                    - SpecialFunctions.LogGamma(c + d + 1)
                    - SpecialFunctions.LogGamma(a + c + 1)
                    - SpecialFunctions.LogGamma(b + d + 1);
            double r = Math.Exp(-logr);
            return r;
        }

        static public double LogGamma(double x)
        {
            const double c1 = 0.918938533204673;
            const double c2 = 0.000595238095238;
            const double c3 = 0.000793650793651;
            const double c4 = 0.002777777777778;
            const double c5 = 0.083333333333333;

            double f, z;

            if (x < 7.0)
            {
                f = 1.0;
                for (z = x; z < 7.0; z += 1.0)
                {
                    x = z;
                    f *= z;
                }
                x += 1.0;
                f = -Math.Log(f);
            }
            else
            {
                f = 0.0;
            }
            z = 1.0 / (x * x);

            return f + Math.Log(x) * (x - 0.5) - x + c1 +
                (((-c2 * z + c3) * z - c4) * z + c5) / x;
        }
        static public void UnitTest(ParallelOptions parallelOptions)
        {

            Helper.CheckCondition(Math.Abs(0.921039736 - LikelihoodRatioTestOnNByMCounts(new int[,] { { 3, 5 }, { 1, 2 }, { 7, 9 } })) < .000001);


            Helper.CheckCondition(1.0 == RocAreaUnderCurve(new KeyValuePair<double, bool>[] { new KeyValuePair<double, bool>(.75, true), new KeyValuePair<double, bool>(.25, false) }, parallelOptions));
            Helper.CheckCondition(0.0 == RocAreaUnderCurve(new KeyValuePair<double, bool>[] { new KeyValuePair<double, bool>(.25, true), new KeyValuePair<double, bool>(.75, false) }, parallelOptions));
            Helper.CheckCondition(0.5 == RocAreaUnderCurve(new KeyValuePair<double, bool>[] { new KeyValuePair<double, bool>(.5, true), new KeyValuePair<double, bool>(.5, false) }, parallelOptions));
            Helper.CheckCondition(1 - 0.40625 == RocAreaUnderCurve(new KeyValuePair<double, bool>[] {
                new KeyValuePair<double, bool>(1, true),
                new KeyValuePair<double, bool>(.5, false),
                new KeyValuePair<double, bool>(.5, true),
                new KeyValuePair<double, bool>(.5, true),
                new KeyValuePair<double, bool>(.5, false),
                new KeyValuePair<double, bool>(.5, false),
                new KeyValuePair<double, bool>(0, true),
                new KeyValuePair<double, bool>(0, false),
            }, parallelOptions));




            double p0 = .99999999;
            double p1 = .000000001;

            Helper.CheckCondition(Math.Abs(SpecialFunctions.LogSum(Math.Log(p0), Math.Log(p1)) - Math.Log(p0 + p1)) < .000001);



            Helper.CheckCondition(Math.Abs(SpecialFunctions.BayesScore(5, 131, 3, 7) - 3.330435168) < .000001); // real Debug.Assert
            Helper.CheckCondition(Math.Abs(SpecialFunctions.BayesScore(new int[,] { { 5, 131 }, { 3, 7 } }) - 3.330435168) < .000001); // real Debug.Assert

            Debug.WriteLine(SpecialFunctions.FisherExactTest(0, 113, 1, 0));
            Debug.WriteLine(SpecialFunctions.FisherExactTest(0, 1, 113, 0));


            Helper.CheckCondition(Math.Abs(Math.Exp(SpecialFunctions.LogGamma(4)) - 6.0) < .0000001); // real Debug.Assert
            Helper.CheckCondition(Math.Abs(SpecialFunctions.BayesScore(5, 131, 3, 7) - 3.330435168) < .000001); // real Debug.Assert
            Helper.CheckCondition(Math.Abs(SpecialFunctions.FisherExactTest(5, 131, 3, 7) - 0.011) < .005);
            Helper.CheckCondition(Math.Abs(SpecialFunctions.FisherExactTest(5, 50, 10, 100) - 1) < .005);
            Helper.CheckCondition(Math.Abs(SpecialFunctions.FisherExactTest(100, 200, 3000, 5000) - 0.145153831) < 1e-6);

            //			Debug.WriteLine(SpecialFunctions.GaussTail(0, false)); //0.5
            //			Debug.WriteLine(SpecialFunctions.Normal(0)); //0.5
            //			Debug.WriteLine(SpecialFunctions.GaussTail(10000.0, false)); //1
            //			Debug.WriteLine(SpecialFunctions.Normal(10000.0)); //1
            //			Debug.WriteLine(SpecialFunctions.GaussTail(-10000.0, false)); //1
            //			Debug.WriteLine(SpecialFunctions.Normal(-10000.0)); //1
            //			Debug.WriteLine(SpecialFunctions.GaussTail(1, false)); //0.841344746068515
            //			Debug.WriteLine(SpecialFunctions.Normal(1)); //0.841344746068515
            //			Debug.WriteLine(SpecialFunctions.GaussTail(-1, false)); //0.841344746068515
            //			Debug.WriteLine(SpecialFunctions.Normal(-1)); //0.841344746068515

            //			Debug.WriteLine(SpecialFunctions.InverseCumulativeGaussian(.5));
            //			Debug.WriteLine(SpecialFunctions.InverseCumulativeGaussian(0.841344746068515));

        }

        static public void Swap<T>(ref T i, ref T j)
        {
            T k;
            k = i;
            i = j;
            j = k;
        }

        static public double FisherExactTest(int[,] counts)
        {
            Helper.CheckCondition(counts.GetLength(0) == 2 && counts.GetLength(1) == 2);
            return FisherExactTest(counts[1, 1], counts[1, 0], counts[0, 1], counts[0, 0]);
        }


        static private double PDataDeleteMe(int a, int b, int c, int d)
        {
            double pt = 1.0;

            //(a+b)!/a!
            int aPlusB = a + b;
            for (int z = a + 1; z <= aPlusB; ++z)
            {
                pt = pt * z;
            }

            // (a+c)!/c!
            int aPlusC = a + c;
            for (int z = c + 1; z <= aPlusC; ++z)
            {
                pt = pt * z;
            }

            //(c+d)!/d!
            int cPlusD = c + d;
            for (int z = d + 1; z <= cPlusD; ++z)
            {
                pt = pt * z;
            }

            //(b+d)!/b!
            int bPlusD = b + d;
            for (int z = b + 1; z <= bPlusD; ++z)
            {
                pt = pt * z;
            }

            Helper.CheckCondition(!double.IsInfinity(pt), "The Fisher Exact Test calculation overflowed on [[{0},{1}],[{2},{3}]]", a, b, c, d);

            //(a+b+c+d)!
            int sumABCD = a + b + c + d;
            for (int z = 1; z <= sumABCD; ++z)
            {
                pt = pt / z;
            }

            return pt;
        }



        /// <summary>
        /// Returns 2 sided FET.  Uses "Fisher statistic" for determining
        /// whether counts are more extreme.
        /// See http://www.med.uio.no/imb/stat/two-by-two/methods.html.
        /// </summary>
        static public double FisherExactTest(int inA, int inB, int inC, int inD)
        {
            //Based on TurboPascal code

            if ((inA + inB == 0) || (inA + inC == 0) || (inB + inD == 0) || (inC + inD == 0))
            {
                return 1.0;
            }

            double p0 = HypergeometricProbability(inA, inB, inC, inD); // the prob of seeing the actual data
            double p = 0.0;
            int minA = Math.Min(inA + inB, inA + inC);
            for (int j = 0; j <= minA; ++j)
            {
                int a = j;
                int b = inA + inB - j;
                int c = inA + inC - j;
                int d = inD - inA + j;
                if ((a >= 0) && (b >= 0) && (c >= 0) && (d >= 0))
                {
                    // using Fisher stat -- the probability of the data --
                    //   to determine which cell counts are "more extreme" *)
                    double pt = HypergeometricProbability(a, b, c, d);
                    if (pt <= p0)
                    {
                        p = p + pt;
                    }
                }
            }

            return Math.Min(p, 1.0);
        }

        /// <summary>
        /// Computes the minimum Fishers exact test pvalue that coule be achieved with the marginal counts specified by the table [a,b,c,d].
        /// </summary>
        /// <param name="inA"></param>
        /// <param name="inB"></param>
        /// <param name="inC"></param>
        /// <param name="inD"></param>
        /// <returns></returns>
        static public double MinFisherExactTestPValue(int inA, int inB, int inC, int inD)
        {
            double leftDelta = Math.Min(inA, inD);
            double a = inA - leftDelta, b = inB + leftDelta, c = inC + leftDelta, d = inD - leftDelta;
            double leftSide = SpecialFunctions.HypergeometricProbability(a, b, c, d);

            double rightDelta = Math.Min(inB, inC);
            a = inA + rightDelta; b = inB - rightDelta; c = inC - rightDelta; d = inD + rightDelta;
            double rightSide = SpecialFunctions.HypergeometricProbability(a, b, c, d);

            double minP = rightSide == leftSide ? 2 * rightSide : Math.Min(leftSide, rightSide);
            minP = Math.Min(minP, 1);

            return minP;
        }


        static public double OneWayPower(
            int HLANotMut, int HLAMut, int NotHLANotMut, int NotHLAMut,
            double OddsRatio, double PValue)
        {

            double PCPosOR = OddsRatio;
            double PCNegOR = 1.0 / PCPosOR;
            double PCpvalue = PValue;
            double TestStat = SpecialFunctions.NormalDensityInverse(PCpvalue);
            int HLACount = HLANotMut + HLAMut;
            int MutCount = HLAMut + NotHLAMut;
            int Count = HLANotMut + HLAMut + NotHLANotMut + NotHLAMut;
            double MaxPower = double.NegativeInfinity;
            for (int I = 1; I <= 2; ++I)
            {
                double n1 = HLACount;
                double n2 = Count - HLACount;
                double p = (double)MutCount / (double)Count;
                double RR;
                if (I == 1)
                {
                    RR = PCPosOR;
                }
                else
                {
                    RR = PCNegOR;
                }
                double p1 = (RR * p * (n1 + n2)) / (RR * n1 + n2);
                double p2 = p1 / RR;

                if ((p1 <= 1) && (p1 >= 0) && (p2 <= 1) && (p2 >= 0))
                {
                    double q = 1 - p;
                    double q1 = 1 - p1;
                    double q2 = 1 - p2;

                    if (p1 * q1 + p2 * q2 > 0)
                    {
                        double X1 = (-TestStat * Math.Sqrt((p * q) / n1 + (p * q) / n2) - (p1 - p2)) / (Math.Sqrt(p1 * q1 / n1 + p2 * q2 / n2));
                        double Power1;
                        if (X1 < 0)
                        {
                            Power1 = SpecialFunctions.NormalDensity(X1) / 2;
                        }
                        else
                        {
                            Power1 = 1 - SpecialFunctions.NormalDensity(X1) / 2;
                        }
                        double X2 = (TestStat * Math.Sqrt((p * q) / n1 + (p * q) / n2) - (p1 - p2)) / (Math.Sqrt(p1 * q1 / n1 + p2 * q2 / n2));
                        double Power2;
                        if (X2 < 0)
                        {
                            Power2 = 1 - SpecialFunctions.NormalDensity(X2) / 2.0;
                        }
                        else
                        {
                            Power2 = SpecialFunctions.NormalDensity(X2) / 2.0;
                        }
                        double Power_ = Power1 + Power2;

                        if (Power_ >= MaxPower)
                        {
                            MaxPower = Power_;
                        }
                    }
                }
            }
            return MaxPower;
        }

        public static double NormalDensity(double Z)
        {
            if (Z == double.NegativeInfinity || Z == double.PositiveInfinity)
            {
                return double.NaN;
            }

            if (Z < 0)
            {
                Z = -Z;
            }

            double Result = Math.Pow((1 + (Z * (0.0498673470 + Z * (0.0211410061 + Z *
                (0.0032776263 + Z * (0.0000380036 + Z *
                (0.0000488906 + Z *
                0.0000053830))))))), -16);
            return Result;
        }




        //		// GaussianIntegral.cxx
        //
        //		// Gaussian tail area - Algorithm 304 in Collected Algorithms From CACM
        //		// Calculates tail area from dblX to infinity (if bUpper is true), or
        //		// from -infinity to dblX (if bUpper is false).
        //
        public static double GaussTail(double dblX, bool bUpper)
        {
            const double dblOneOverSqrt2Pi = 0.3989422804014;		// one over the square root of two pi

            if (dblX == 0)
            {
                return 0.5;
            }
            else if (dblX < 0)
            {
                bUpper = !bUpper;
                dblX = -dblX;
            }

            double dblXSquared = dblX * dblX;
            double dblY = dblOneOverSqrt2Pi * Math.Exp(dblXSquared * -0.5);

            double dblN = dblY / dblX;

            if (0.0 == dblN)
            {
                return (bUpper ? 0.0 : 1.0);
            }

            double dblS;
            double dblT;

            if (dblX > (bUpper ? 2.32 : 3.5))
            {
                double dblP1, dblP2, dblQ1, dblQ2, dblM;

                dblQ1 = dblX;
                dblP2 = dblX * dblY;
                dblP1 = dblY;
                dblQ2 = dblXSquared + 1.0;
                if (bUpper)
                {
                    dblM = dblP1 / dblQ1;
                    dblS = dblM;
                    dblT = dblP2 / dblQ2;
                }
                else
                {
                    dblM = 1.0 - dblP1 / dblQ1;
                    dblS = dblM;
                    dblT = 1.0 - dblP2 / dblQ2;
                }
                for (dblN = 2.0; dblM != dblT && dblS != dblT; dblN += 1.0)
                {
                    dblS = dblX * dblP2 + dblN * dblP1;
                    dblP1 = dblP2;
                    dblP2 = dblS;
                    dblS = dblX * dblQ2 + dblN * dblQ1;
                    dblQ1 = dblQ2;
                    dblQ2 = dblS;
                    dblS = dblM;
                    dblM = dblT;
                    dblT = dblP2 / dblQ2;

                    if (!bUpper)
                    {
                        dblT = 1.0 - dblT;
                    }
                }
                return dblT;
            }
            else
            {
                dblX *= dblY;
                dblS = dblX;
                dblT = 0.0;

                for (dblN = 3.0; dblS != dblT; dblN += 2.0)
                {
                    dblT = dblS;
                    dblX *= dblXSquared / dblN;
                    dblS += dblX;
                }

                return 0.5 + (bUpper ? -dblS : dblS);
            }
        }


        public static double InverseCumulativeGaussian(double pvalue, double mean, double stdDev)
        {
            double result = InverseCumulativeGaussian(pvalue) * stdDev + mean;
            return result;
        }

        // From Max's C++ code
        //
        // Odeh, R. E. & Evans, J. O. (1974) Algorithm AS 70: Percentage points
        //  of the normal distribution. Applied Statistics, 23, 96-97. 
        //  also, Kennedy, W. J. & Gentle, J. E. (1980) Statistical Computing
        // returns # of std devs from mean for probability desired
        public static double InverseCumulativeGaussian(double dblProb)
        {
            const double RTINY = 1.0e-20;			//  A number very close to zero (from Numerical Recipies)


            // P's are used in numerator of fraction, Q's are used in the denominator
            const double dblP0 = -0.322232431088;
            const double dblP1 = -1.0;
            const double dblP2 = -0.342242088547;
            const double dblP3 = -0.0204231210245;
            const double dblP4 = -0.453642210148E-4;

            const double dblQ0 = 0.0993484626060;
            const double dblQ1 = 0.588581570495;
            const double dblQ2 = 0.531103462366;
            const double dblQ3 = 0.103537752850;
            const double dblQ4 = 0.38560700634E-2;

            double dblPosResult;					// a positive result
            double dblPLeft;						// always <= 0.5

            // this is exactly the mean, so it's a special case
            if (dblProb == 0.5)
            {
                return (0.0);
            }

            // we always do our calculation for one side of the gaussian
            // and negate at the end if we were actually on the other side
            if (dblProb > 0.5)
            {
                dblPLeft = 1.0 - dblProb;
            }
            else
            {
                dblPLeft = dblProb;
            }

            if (dblPLeft < RTINY)
            {
                dblPLeft = RTINY;					// minimum value to prevent underflow */
            }

            double dblY;							// intermediate value
            dblY = Math.Sqrt(Math.Log(1.0 / (dblPLeft * dblPLeft)));

            dblPosResult = dblY + ((((dblP4 * dblY + dblP3) * dblY + dblP2) * dblY + dblP1) * dblY + dblP0) /
                        ((((dblQ4 * dblY + dblQ3) * dblY + dblQ2) * dblY + dblQ1) * dblY + dblQ0);

            return (dblProb < 0.5 ? -dblPosResult : dblPosResult);

        }

        public static double NormalDensityInverse(double pvalue)
        {
            if (pvalue >= 1)
            {
                return 100;
            }
            else
            {
                double ts1 = 0;
                double ts2 = 100;

                while (true)
                {
                    double ts = ts1 + (ts2 - ts1) / 2;
                    double p = NormalDensity(ts);
                    if (p < pvalue)
                    {
                        ts2 = ts;
                    }
                    else
                    {
                        ts1 = ts;
                    }

                    if (Math.Abs(p - pvalue) < 0.0005)
                    {
                        return ts;
                    }
                }
            }
        }



        /// <summary>
        ///		We also exclude those covariates that have a value in the 2x2 table 
        ///		that is less than 5 and have an expected value &lt; 5. This, together 
        ///		with the power elimination rule, tries minimise the chance of the 
        ///		logistics models going wacky with covariates with small amounts of 
        ///		data.
        /// </summary>
        /// <returns>True, if these counts should be excluded</returns>
        static public double MinOfCountOrExpCount(
            int HLANotMut, int HLAMut, int NotHLANotMut, int NotHLAMut
            )
        {
            //		N stands for "not HLA" 
            //		and NS for "non-synonymous mutation (i.e. polymorphism from 
            //		consensus)". Hence;

            double NSumNS = NotHLAMut; // NSumNS = the number of patients without the HLA and with an amino acid (or ambiguity) different to the consensus
            double SumS = HLANotMut;	 // SumS = the number of patients with the HLA and with the consensus  amino acid
            double NSumS = NotHLANotMut;  // NSumS = the number of patients without the HLA and with the consensus amino acid
            double SumNS = HLAMut;  // SumNS = the number of patients with the HLA and with an amino acid (or ambiguity) different to the consensus

            double ExpNSumS = (double)(NSumS + NSumNS) * (NSumS + SumS) / (double)(SumNS + SumS + NSumNS + NSumS);
            double ExpSumNS = (double)(SumS + SumNS) * (NSumNS + SumNS) / (double)(SumNS + SumS + NSumNS + NSumS);
            double ExpNSumNS = (double)(NSumS + NSumNS) * (NSumNS + SumNS) / (double)(SumNS + SumS + NSumNS + NSumS);
            double ExpSumS = (double)(SumS + SumNS) * (NSumS + SumS) / (double)(SumNS + SumS + NSumNS + NSumS);

            double rMin =
                Min(Math.Max(NSumNS, ExpNSumNS),
                    Math.Max(SumS, ExpSumS),
                    Math.Max(NSumS, ExpNSumS),
                    Math.Max(SumNS, ExpSumNS));
            return rMin;
        }



        ////!!!not a special math function
        //public static void Helper.CheckCondition(bool condition)
        //{
        //    Helper.CheckCondition(condition, "A condition failed.");
        //}

        ///// <summary>
        ///// This version of CheckCondition does not evaluate the message string unless the condition fails
        ///// </summary>
        ///// <param name="condition"></param>
        ///// <param name="messageFunction"></param>
        //public static void Helper.CheckCondition(bool condition, Func<string> messageFunction)
        //{
        //    if (!condition)
        //    {
        //        string message = messageFunction();
        //        throw new Exception(message);
        //    }
        //}

        ///// <summary>
        ///// Warning: The message with be evaluated even if the condition is true, so don't make it's calculation slow.
        /////           Avoid this with the "messageFunction" version.
        ///// </summary>
        ///// <param name="condition"></param>
        ///// <param name="message"></param>
        //public static void Helper.CheckCondition(bool condition, string message)
        //{
        //    if (!condition)
        //    {
        //        throw new Exception(message);
        //    }
        //}

        //public static void Helper.CheckCondition(bool condition, string messageToFormat, params object[] formatValues)
        //{
        //    if (!condition)
        //    {
        //        throw new Exception(string.Format(messageToFormat, formatValues));
        //    }
        //}

        //public static void CheckCondition<T>(bool condition) where T : Exception
        //{
        //    CheckCondition<T>(condition, "A condition failed.");
        //}

        //public static void CheckCondition<T>(bool condition, string message) where T : Exception
        //{
        //    if (!condition)
        //    {
        //        Type t = typeof(T);
        //        ConstructorInfo constructor = t.GetConstructor(new Type[] { typeof(string) });
        //        T exception = (T)constructor.Invoke(new object[] { message });
        //        throw exception;
        //    }
        //}

        //public static void CheckCondition<T>(bool condition, string messageToFormat, params object[] formatValues) where T : Exception
        //{
        //    if (!condition)
        //    {
        //        CheckCondition<T>(condition, string.Format(messageToFormat, formatValues));
        //    }
        //}

        ///// <summary>
        ///// This version of CheckCondition does not evaluate the message string unless the condition fails
        ///// </summary>
        //public static void CheckCondition<T>(bool condition, Func<string> messageFunction) where T : Exception
        //{
        //    if (!condition)
        //    {
        //        string message = messageFunction();
        //        CheckCondition<T>(condition, message);
        //    }
        //}

        //public static string CreateDelimitedString(string delimiter, params object[] objectCollection)
        //{
        //    return objectCollection.StringJoin(delimiter);
        //}

        //[Obsolete("This has be superseded  by 'ienum.StringJoin(separator)'")]
        //public static string CreateDelimitedString2<T>(string delimiter, IEnumerable<T> objectCollection)
        //{
        //    return objectCollection.StringJoin(delimiter);
        //}

        ////!!!not a special math function
        //public static string CreateTabString(params object[] objectCollection)
        //{
        //    return objectCollection.StringJoin("\t");
        //}


        ////!!!not a special math function
        //[Obsolete("This has be superseded  by 'ienum.StringJoin(separator)'")]
        //public static string CreateTabString2<T>(IEnumerable<T> objectCollection)
        //{
        //    return objectCollection.StringJoin("\t");

        //}



        static public int Min(params int[] collection)
        {
            Helper.CheckCondition(collection.Length > 0); //!!!raise error
            int iMin = collection[0];
            for (int i = 1; i < collection.Length; ++i)
            {
                iMin = Math.Min(iMin, collection[i]);
            }
            return iMin;
        }

        static public double Min(params double[] collection)
        {
            Helper.CheckCondition(collection.Length > 0); //!!!raise error
            double rMin = collection[0];
            for (int i = 1; i < collection.Length; ++i)
            {
                rMin = Math.Min(rMin, collection[i]);
            }
            return rMin;
        }

        static public int Max(params int[] collection)
        {
            Helper.CheckCondition(collection.Length > 0); //!!!raise error
            int iMax = collection[0];
            for (int i = 1; i < collection.Length; ++i)
            {
                iMax = Math.Max(iMax, collection[i]);
            }
            return iMax;
        }

        static public double Max(params double[] collection)
        {
            Helper.CheckCondition(collection.Length > 0); //!!!raise error
            double rMax = collection[0];
            for (int i = 1; i < collection.Length; ++i)
            {
                rMax = Math.Max(rMax, collection[i]);
            }
            return rMax;
        }

        public static double BayesScore(int[,] counts)
        {
            // Returns log p(dependent)/p(independent), assuming a K2-like
            // parameter prior and assuming that initially we're 50/50.

            // counts[iStateVar1][iStateVar2] contains the number of rows in which
            // variable1 occurs in state iStateVar1 AND variable2 occurs in state iStateVar2.

            // The score is computed as log p(variable2 data | variable1 data) / p(variable2 data)
            // In the numerator, we use a Dirichlet prior where each state has an ESS of 1, and in 
            // the denominator, we use a Dirichlet prior where each state has an ESS of cStateVar1.

            // See ftp://ftp.research.microsoft.com/pub/tr/tr-95-06.pdf, page 9. For the dependant
            // model, we split up the data and apply Equation (13) for each state of variable 1.
            // For the independent model, we sum over all states of variable 1 to get the marginal
            // counts for variable 2.

            int cStateVar1 = counts.GetLength(0);
            int cStateVar2 = counts.GetLength(1);

            double dblScoreDep = 0.0;	// Score for the dependent model
            double dblScoreIndep = 0.0;	// Score for the independent model

            // Calculate the dependence score

            for (int iStateVar1 = 0; iStateVar1 < cStateVar1; iStateVar1++)
            {
                // Add in the terms in the product of Equation (13) for this 
                // particular state of variable 1. 

                int ccaseStateVar1 = 0; // Accumulate the marginal count for this state of var 1

                for (int iStateVar2 = 0; iStateVar2 < cStateVar2; iStateVar2++)
                {
                    int ccaseCoOccur = counts[iStateVar1, iStateVar2];

                    ccaseStateVar1 += ccaseCoOccur;
                    dblScoreDep += LogGamma(1 + ccaseCoOccur);

                    // Note that Gamma(1) = 1, so no division is necessary
                }

                // Now add in the first term in Equation (13) for this particular state.
                // The \alpha is the number of states of variable 2, and the N is the number
                // of cases where variable 1 is in state iStateVar1.

                dblScoreDep += LogGamma(cStateVar2) - LogGamma(cStateVar2 + ccaseStateVar1);
            }

            // For the independence model, we need the histogram for variable 2. This should 
            // probably be passed in, as it will most likely be available to the caller. 
            // Alternatively, we can construct the histogram within the loop above, after 
            // allocating memory in which to store it. Below we're simply swapping the loops
            // from above and thus accomplishing an O(n) calculation in O(n^2). Because we're
            // calling LogGamma() n^2 times above anyway, this isn't going to slow things
            // down much, but its somewhat distasteful.

            int ccaseTotal = 0;	// Sum of the elements in counts

            for (int iStateVar2 = 0; iStateVar2 < cStateVar2; iStateVar2++)
            {
                // Get the number of marginal counts for this state. (i.e., get the
                // height of the histogram for state iStateVar2)

                int ccaseStateVar2 = 0;

                for (int iStateVar1 = 0; iStateVar1 < cStateVar1; iStateVar1++)
                {
                    ccaseStateVar2 += counts[iStateVar1, iStateVar2];
                }

                // Put in the corresponding product term from Equation 13, using
                // \alpha_k = cStateVar1

                dblScoreIndep += LogGamma(cStateVar1 + ccaseStateVar2) - LogGamma(cStateVar1);

                ccaseTotal += ccaseStateVar2;

            }

            // Finally, add in the first term of Equation (13) for the independence model.
            // Because the ESS for each state of variable 2 is cStateVar1, \apha is equal
            // to cStateVar1 * cStateVar2

            double dblAlpha = cStateVar2 * cStateVar1;

            dblScoreIndep += LogGamma(dblAlpha) - LogGamma(dblAlpha + ccaseTotal);

            return dblScoreDep - dblScoreIndep;
        }



#if !SILVERLIGHT
        private SortedList[] _rgDegreeOfFreedomToSigToValue = null;
        public double ChiSquareInvOneSidedValue(double sig, int dependentVariableValueCount)
        {
            //double rSigBF = sig / (double) independentVariableCount;
            int iDF = dependentVariableValueCount - 1;
            if (iDF == 0)
            {
                return double.PositiveInfinity;
            }
            Helper.CheckCondition(0 < iDF && iDF < _rgDegreeOfFreedomToSigToValue.Length); //!!!raise error
            SortedList rgSigToValue = (SortedList)_rgDegreeOfFreedomToSigToValue[iDF];
            Helper.CheckCondition(rgSigToValue.ContainsKey(sig)); //!!!raise error
            double rValue = (double)rgSigToValue[sig];
            return rValue;
        }
#endif


        private SpecialFunctions()
        {
#if !SILVERLIGHT
            LoadLogLikelihoodRatioTestTable();
#endif
        }

        static public SpecialFunctions GetInstance()
        {
            return _singleton.Value;
        }
        static Lazy<SpecialFunctions> _singleton = new Lazy<SpecialFunctions>(() => new SpecialFunctions());


#if !SILVERLIGHT
        double[] LogLikelihoodRatioTestKeyTable;
        double[] LogLikelihoodRatioTestValueTable;

        private void LoadLogLikelihoodRatioTestTable()
        {
            string header = "DOF\tRowCount\tRowFraction\tColumn3\tlogLikelihoodRatio\tPvalue\tln(Pvalue)";
            List<Dictionary<string, string>> rowList = //SpecialFunctions.TabFileTableAsList(@"DataFiles\ChiSquareIsLogLogLinear.txt", header, false);
                SpecialFunctions.TabFileTableAsList(Assembly.GetAssembly(typeof(SpecialFunctions)),
                "MBT.Escience.DataFiles.", "ChiSquareIsLogLogLinear.txt", header, false);

            LogLikelihoodRatioTestKeyTable = new double[rowList.Count + 1];
            LogLikelihoodRatioTestValueTable = new double[rowList.Count + 1];
            for (int i = 0; i < rowList.Count; ++i)
            {
                Dictionary<string, string> row = rowList[i];
                double logLikelihoodRatio = double.Parse(row["logLikelihoodRatio"]);
                double logPValue = double.Parse(row["ln(Pvalue)"]);
                LogLikelihoodRatioTestKeyTable[i] = logLikelihoodRatio;
                LogLikelihoodRatioTestValueTable[i] = logPValue;
            }
            LogLikelihoodRatioTestKeyTable[rowList.Count] = double.PositiveInfinity;
            LogLikelihoodRatioTestValueTable[rowList.Count] = double.NegativeInfinity;
            Array.Sort(LogLikelihoodRatioTestKeyTable, LogLikelihoodRatioTestValueTable);
        }

        public double LogLikelihoodRatioTest(double logLikelihoodRatio, double fractionalDegreesOfFreedom)
        {
            int low = (int)Math.Floor(fractionalDegreesOfFreedom);
            int high = (int)Math.Ceiling(fractionalDegreesOfFreedom);
            if (low == high)
                return LogLikelihoodRatioTest(logLikelihoodRatio, low);

            Helper.CheckCondition(Math.Abs(fractionalDegreesOfFreedom - 1.5) <= double.Epsilon, "TEMP CHECK: we expect df to be 1.5 if we're here. it's really " + fractionalDegreesOfFreedom);

            double pLow = LogLikelihoodRatioTest(logLikelihoodRatio, low);
            double pHigh = LogLikelihoodRatioTest(logLikelihoodRatio, high);

            double proportionHigh = fractionalDegreesOfFreedom - low;   // what percent of final result comes from high?

            double p = proportionHigh * pHigh + (1 - proportionHigh) * pLow;
            return p;
        }

        [Obsolete("Consider using ShoUtils.LRTest instead")]
        public double LogLikelihoodRatioTest(double logLikelihoodRatio, int degreesOfFreedom)
        {
            if (double.IsNaN(logLikelihoodRatio))
            {
                return double.NaN;
            }

            double lnp;

            switch (degreesOfFreedom)
            {
                case 0:
                    lnp = 0; break;
                case 1:
                    lnp = LinearInterpolation(logLikelihoodRatio, LogLikelihoodRatioTestKeyTable, LogLikelihoodRatioTestValueTable);
                    break;
                case 2:
                    lnp = -logLikelihoodRatio;  // it just so happens...
                    break;
                default:
                    throw new NotSupportedException(string.Format("LogLikelihoodRatioTest only support 0, 1 and 2 df. {0} is invalid.", degreesOfFreedom));
            }

            double p = Math.Exp(lnp);

            return p;

        }

        private double LinearInterpolation(double keyValue, double[] keyArray, double[] valueArray)
        {
            double result;
            int indexOrNot = Array.BinarySearch<double>(keyArray, keyValue);
            if (indexOrNot >= 0)
            {
                result = valueArray[indexOrNot];
            }

            else
            {
                int indexHigh = ~indexOrNot;

                int indexLow = indexHigh - 1;
                Helper.CheckCondition(0 <= indexLow && indexHigh < valueArray.Length);

                //If the difference is very large, then return the smallest non-zero pValue in the charts
                if (indexHigh == valueArray.Length - 1)
                {
                    result = valueArray[indexLow];
                }
                else
                {
                    double keyLow = keyArray[indexLow];
                    double keyHigh = keyArray[indexHigh];

                    double fraction = (keyValue - keyLow) / (keyHigh - keyLow);
                    Debug.Assert(0 <= fraction && fraction <= 1); // real assert

                    double valueLow = valueArray[indexLow];
                    double valueHigh = valueArray[indexHigh];

                    double logPValue = (valueHigh - valueLow) * fraction + valueLow;
                    result = logPValue;
                }
            }
            return result;
        }
#endif

        static public double LogSumParams(params double[] logProbabilityCollection)
        {
            //Debug.Assert(Math.Exp(double.NegativeInfinity) == 0); //real assert

            double logMax = double.NegativeInfinity;
            int celem = logProbabilityCollection.Length;

            for (int ielem = 0; ielem < celem; ielem++)
            {
                if (double.IsNaN(logProbabilityCollection[ielem]))
                    return double.NaN;  // NaN is sticky
                if (logProbabilityCollection[ielem] > logMax)
                    logMax = logProbabilityCollection[ielem];
            }
            if (double.IsNegativeInfinity(logMax))
            {
                return logMax;
            }
            //Debug.Assert(logMax > double.NegativeInfinity); //!!!raise exception

            double logK = Math.Log(double.MaxValue) - Math.Log((double)celem + 1.0) - logMax;

            double sum = 0;
            for (int ielem = 0; ielem < celem; ielem++)
            {
                sum += Math.Exp(logProbabilityCollection[ielem] + logK);
            }

            double dbl = -logK + Math.Log(sum);

            //Debug.Assert(double.NegativeInfinity < dbl && dbl < double.PositiveInfinity);

            return -logK + Math.Log(sum);
        }

        static double LogSumConst = Math.Log(double.MaxValue) - Math.Log(3.0);

        /// <summary>
        /// logsum([a b c])=log(sum(exp([a b c])))x
        /// </summary>
        /// <param name="logP1"></param>
        /// <param name="logP2"></param>
        /// <returns></returns>
        static public double LogSum(double logP1, double logP2)
        {
            if (double.IsNegativeInfinity(logP1))
            {
                if (double.IsNegativeInfinity(logP2))
                {
                    return double.NegativeInfinity;
                }
                else
                {
                    return logP2;
                }
            }
            if (double.IsNegativeInfinity(logP2))
            {
                return logP1;
            }
            if ((double.IsInfinity(logP1) && !double.IsNaN(logP2))
                || (double.IsInfinity(logP2) && !double.IsNaN(logP1)))
            {
                return double.PositiveInfinity;
            }

            double logK = LogSumConst - Math.Max(logP1, logP2);
            double sum = Math.Exp(logP1 + logK) + Math.Exp(logP2 + logK);
            double r = Math.Log(sum) - logK;
            //Debug.Assert(double.NegativeInfinity < r && r < double.PositiveInfinity);
            return r;
        }

        static public double LogSubtract(double logP1, double logP2)
        {
            if (double.IsNegativeInfinity(logP1) && double.IsNegativeInfinity(logP2))
            {
                return double.NegativeInfinity;
            }
            double logK = LogSumConst - Math.Max(logP1, logP2);
            double sum = Math.Exp(logP1 + logK) - Math.Exp(logP2 + logK);
            double r = Math.Log(sum) - logK;
            //Debug.Assert(double.NegativeInfinity < r && r < double.PositiveInfinity);
            return r;
        }

        static public double LogSubtractParams(params double[] logProbabilityCollection)
        {
            //Debug.Assert(Math.Exp(double.NegativeInfinity) == 0); //real assert

            double logMax = double.NegativeInfinity;
            int celem = logProbabilityCollection.Length;

            for (int ielem = 0; ielem < celem; ielem++)
                if (logProbabilityCollection[ielem] > logMax)
                    logMax = logProbabilityCollection[ielem];

            if (double.IsNegativeInfinity(logMax))
            {
                return logMax;
            }
            //Debug.Assert(logMax > double.NegativeInfinity); //!!!raise exception

            double logK = Math.Log(double.MaxValue) - Math.Log((double)celem + 1.0) - logMax;

            double sum = 0;
            for (int ielem = 0; ielem < celem; ielem++)
            {
                sum -= Math.Exp(logProbabilityCollection[ielem] + logK);
            }

            double dbl = -logK + Math.Log(sum);

            //Debug.Assert(double.NegativeInfinity < dbl && dbl < double.PositiveInfinity);

            return -logK + Math.Log(sum);
        }

        public static double LogOdds(double probability)
        {
            return Math.Log(probability / (1.0 - probability));
        }

        public static double InverseLogOdds(double logOdds)
        {
            if (double.IsInfinity(logOdds))
            {
                if (logOdds > 0)
                {
                    return 1.0;
                }
                else
                {
                    return 0.0;
                }
            }
            else
            {
                double odds = Math.Exp(logOdds);
                if (double.IsPositiveInfinity(odds))
                {
                    return 1.0;
                }
                return odds / (1.0 + odds);
            }
        }


        /// <summary>
        /// 
        /// Wikipedia on 6/24/2010 http://en.wikipedia.org/w/index.php?title=Multinomial_logit&oldid=364643091
        ///    sets the odds of the class #0 as 1 and then the odds of the other classes is relative to that.
        /// an alternative to that (http://justindomke.wordpress.com/logistic-regression/) does not distingish
        /// class #0 from the others but that means it a weight that could be calculated from the other weights.
        /// </summary>
        /// <param name="logOddsList"></param>
        /// <param name="AssumeAnotherEntryWithOddsOf1"></param>
        /// <returns></returns>
        public static List<double> InverseLogOdds(IList<double> logOddsList, bool AssumeAnotherEntryWithOddsOf1)
        {
            Helper.CheckCondition(!AssumeAnotherEntryWithOddsOf1, "Current code assumes AssumeAnotherEntryWithOddsOf1 is false");

            var oddsList = logOddsList.Select(w => Math.Exp(w)).ToList();
            double sum = oddsList.Sum();
            Helper.CheckCondition(!double.IsNaN(sum), "Expect sum of odds to be a real value");
            var probabilityList = oddsList.Select(odds => odds / sum).ToList();
            return probabilityList;
        }


        //This could be replaced with the quick-sort-like linear expect time algorithm. See, for example, http://www2.toki.or.id/book/AlgDesignManual/BOOK/BOOK4/NODE150.HTM
        // OR http://en.wikipedia.org/wiki/Median#Efficient_computation
        // There was also a discussion in http://codecoop around 9/19/2007
        //   Return NaN if the list is empty and return the mean of the middle two values if the list has even length.

        public static double Median(ref List<double> list)
        {
            if (list.Count == 0)
            {
                return double.NaN;
            }

            list.Sort();

            //Examples of indexs of interest
            //0 1 2 -> 1
            //0 1 2 3 -> 1,2

            int lowIndex = (list.Count - 1) / 2;
            if (list.Count % 2 == 1)
            {
                return list[lowIndex];
            }
            else
            {
                return (list[lowIndex] + list[lowIndex + 1]) / 2.0;
            }
        }

        [Obsolete("Made an extention method of IEnumberable. That version is smart for lists.")]
        public static double Median(ref List<int> list)
        {
            if (list.Count == 0)
            {
                return double.NaN;
            }

            list.Sort();

            //Examples of indexs of interest
            //0 1 2 -> 1
            //0 1 2 3 -> 1,2

            int lowIndex = (list.Count - 1) / 2;
            if (list.Count % 2 == 1)
            {
                return (double)list[lowIndex];
            }
            else
            {
                return (double)(list[lowIndex] + list[lowIndex + 1]) / 2.0;
            }
        }

        [Obsolete("Made an extention method of IEnumberable.")]
        public static double Median(IEnumerable<int> listIn)
        {

            List<int> list = new List<int>(listIn);
            if (list.Count == 0)
            {
                return double.NaN;
            }
            list.Sort();

            //Examples of indexs of interest
            //0 1 2 -> 1
            //0 1 2 3 -> 1,2

            int lowIndex = (list.Count - 1) / 2;
            if (list.Count % 2 == 1)
            {
                return (double)list[lowIndex];
            }
            else
            {
                return (double)(list[lowIndex] + list[lowIndex + 1]) / 2.0;
            }
        }

        [Obsolete("Made an extention method of IEnumberable.")]
        public static double Median(IEnumerable<double> listIn)
        {

            List<double> list = new List<double>(listIn);
            if (list.Count == 0)
            {
                return double.NaN;
            }
            list.Sort();

            //Examples of indexs of doubleerest
            //0 1 2 -> 1
            //0 1 2 3 -> 1,2

            int lowIndex = (list.Count - 1) / 2;
            if (list.Count % 2 == 1)
            {
                return list[lowIndex];
            }
            else
            {
                return (list[lowIndex] + list[lowIndex + 1]) / 2.0;
            }
        }


        public static double Bound(double low, double high, double value)
        {
            Helper.CheckCondition(low <= high);
            if (value < low)
            {
                return low;
            }
            else if (value > high)
            {
                return high;
            }
            else
            {
                return value;
            }
        }




        public static double BIC(double logLikelihood, int degreesOfFreedom, int caseCount)
        {
            return logLikelihood - ((double)degreesOfFreedom / 2.0) * Math.Log(caseCount);
        }

        //General
        //!!!could use this in many, many places
        //!!!It is not super fast, but probabily faster than a real database or reading from the file.

        //!!!instead of opening the file once to get the header and again to the lines, could only open it once
        static public IEnumerable<Dictionary<string, string>> TabFileTable(string filename, bool includeWholeLine)
        {
            string header = Bio.Util.FileUtils.ReadLine(filename);
            return TabFileTable(filename, header, includeWholeLine);
        }



        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(string filename, bool includeWholeLine, out string header)
        {
            header = Bio.Util.FileUtils.ReadLine(filename);
            return TabFileTable(null, null, filename, header, includeWholeLine, '\t', true);
        }

        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(string filename, bool includeWholeLine, char separator, out string header)
        {
            header = Bio.Util.FileUtils.ReadLine(filename);
            return TabFileTable(null, null, filename, header, includeWholeLine, separator, true);
        }

        static public IEnumerable<Dictionary<string, string>> TabReader(TextReader reader, bool includeWholeLine = false, char separator = '\t')
        {
            string ignoreHeader;
            return TabReader(reader, out ignoreHeader, includeWholeLine, separator);
        }

        static public IEnumerable<Dictionary<string, string>> TabReader(string filename, bool includeWholeLine = false, char separator = '\t')
        {
            string ignoreHeader;
            return TabReader(filename, out ignoreHeader, includeWholeLine, separator);
        }

        static public IEnumerable<Dictionary<string, string>> TabReader(TextReader reader, out string header, bool includeWholeLine = false, char separator = '\t')
        {
            header = reader.ReadLine();
            return TabReader(reader, header, includeWholeLine, separator, checkHeaderMatch: false, includeHeaderAsFirstLine: false, hasNoHeader: true);
        }

        static public IEnumerable<Dictionary<string, string>> TabReader(string filename, out string header, bool includeWholeLine = false, char separator = '\t')
        {
            header = Bio.Util.FileUtils.ReadLine(filename);
            return TabReader(filename, header, includeWholeLine, separator);
        }


        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(FileInfo file, bool includeWholeLine, out string header)
        {
            header = Bio.Util.FileUtils.ReadLine(file);
            return TabFileTable(null, null, file, header, includeWholeLine, '\t', true);
        }

        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(string filename, char separator)
        {
            string header = Bio.Util.FileUtils.ReadLine(filename);
            return TabFileTable(null, null, filename, header, false, separator, true);
        }

        /// <summary>
        /// Method to read delimited, formatted file. Need not be delimited by tabs as suggested by the name. Delimiter is implied
        ///by the way the header parameter is delimited, and by the param 'separator'. See also ReadDelimitedFile which doesn't require
        ///the output to be parsed into proper data type, but which is slower and it's output arguments cannot be passed on.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="header">e.g. @"name, start, end, region, startLocal, endLocal, startAdjusted, endAdjusted"</param>
        /// <param name="separator">","</param>
        /// <returns></returns>
        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(string filename, string header, char separator)
        {
            return TabFileTable(null, null, filename, header, false, separator, true);
        }

        static public IEnumerable<Dictionary<string, string>> TabFileTableFixHeader(string filename, char separator)
        {
            string header = Bio.Util.FileUtils.ReadLine(filename);
            string header2 = FixupHeader(separator, header);
            return TabFileTable(null, null, filename, header2, false, separator, false);
        }

        private static string FixupHeader(char separator, string header)
        {
            string[] headerCollection = header.Split(separator);
            Set<string> seenIt = Set<string>.GetInstance();
            List<string> headerCollection2 = new List<string>();
            foreach (string head in headerCollection)
            {
                if (seenIt.Contains(head))
                {
                    for (int i = 2; ; ++i)
                    {
                        string head2 = head + "#" + i.ToString();
                        if (!seenIt.Contains(head2))
                        {
                            headerCollection2.Add(head2);
                            seenIt.AddNew(head2);
                            break;
                        }
                    }
                }
                else
                {
                    headerCollection2.Add(head);
                    seenIt.AddNew(head);
                }

            }
            string header2 = headerCollection2.StringJoin(separator.ToString());
            return header2;
        }

        //[Obsolete("Moved to FileUtils")]
        //public static string ReadLine(FileInfo file)
        //{
        //    using (StreamReader streamReader = file.OpenTextStripComments())
        //    {
        //        return streamReader.ReadLine();
        //    }
        //}

        //[Obsolete("Moved to FileUtils")]
        //public static string ReadLine(string filename)
        //{
        //    FileInfo file = new FileInfo(filename);
        //    return ReadLine(file);
        //    //using (StreamReader streamReader = Bio.Util.FileUtils.OpenTextStripComments(filename))
        //    //{
        //    //    return streamReader.ReadLine();
        //    //}
        //}



        // 
        /// <summary>
        /// iterates through all files in a directory that match the given pattern. If checkHeader, each file must have the same header that 
        /// matches the given header.
        /// </summary>
        static public IEnumerable<Dictionary<string, string>> TabDirectoryTable(string directoryName, string filePattern,
            string header, bool includeWholeLine, bool checkHeaderMatch)
        {
            foreach (string filename in Directory.EnumerateFiles(directoryName, filePattern))
            {
                //Console.WriteLine("SpecialFunction.TabDirectoryTable: opening file " + new FileInfo(filename).Name);
                foreach (Dictionary<string, string> row in TabFileTable(filename, header, includeWholeLine, checkHeaderMatch))
                {
                    yield return row;
                }
            }
        }

        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(string filename, string header, bool includeWholeLine)
        {
            return TabFileTable(null, null, filename, header, includeWholeLine, '\t', true);
        }
        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(string filename, string header, bool includeWholeLine, bool checkHeaderMatch)
        {
            return TabFileTable(null, null, filename, header, includeWholeLine, '\t', checkHeaderMatch);
        }
        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(TextReader textReader, string header, bool includeWholeLine)
        {
            return TabFileTable(textReader, "STREAM", header, includeWholeLine, '\t', true);
        }

        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(TextReader textReader, string header, bool includeWholeLine, bool checkHeaderMatch)
        {
            return TabFileTable(textReader, "STREAM", header, includeWholeLine, '\t', checkHeaderMatch, false);
        }

        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(TextReader textReader, string header, bool includeWholeLine, bool checkHeaderMatch, bool includeHeaderAsFirstLine)
        {
            return TabFileTable(textReader, "STREAM", header, includeWholeLine, '\t', checkHeaderMatch, includeHeaderAsFirstLine);
        }


        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(Assembly assembly, string resourcePrefix, string filename,
            string header, bool includeWholeLine, char separator, bool checkHeaderMatch)
        {
            using (TextReader textReader = OpenResourceOrFile(assembly, resourcePrefix, filename))
            {
                foreach (Dictionary<string, string> row in TabFileTable(textReader, filename, header, includeWholeLine, separator, checkHeaderMatch))
                {
                    yield return row;
                }
            }
        }

        static public IEnumerable<Dictionary<string, string>> TabReader(string filename,
            string header, bool includeWholeLine = false, char separator = '\t', bool checkHeaderMatch = true)
        {
            using (TextReader textReader = new FileInfo(filename).OpenTextStripComments())
            {
                foreach (Dictionary<string, string> row in TabReader(textReader, filename, header, includeWholeLine, separator, checkHeaderMatch))
                {
                    yield return row;
                }
            }
        }

        static public IEnumerable<Dictionary<string, string>> TabReader(TextReader textReader,
            string header, bool includeWholeLine = false, char separator = '\t', bool checkHeaderMatch = true, bool includeHeaderAsFirstLine = false, bool continueIfEmptyFileFound = false, bool hasNoHeader = false)
        {
            foreach (Dictionary<string, string> row in TabReader(textReader, "reader", header, includeWholeLine, separator, checkHeaderMatch, includeHeaderAsFirstLine, continueIfEmptyFileFound, hasNoHeader))
            {
                yield return row;
            }
        }


        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(Assembly assembly, string resourcePrefix, FileInfo file,
            string header, bool includeWholeLine, char separator, bool checkHeaderMatch)
        {
            using (TextReader textReader = OpenResourceOrFile(assembly, resourcePrefix, file))
            {
                foreach (Dictionary<string, string> row in TabFileTable(textReader, file.Name, header, includeWholeLine, separator, checkHeaderMatch, false))
                {
                    yield return row;
                }
            }
        }


        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(TextReader textReader, string inputName,
            string header, bool includeWholeLine, char separator, bool checkHeaderMatch)
        {
            return TabFileTable(textReader, inputName, header, includeWholeLine, separator, checkHeaderMatch, false);
        }

        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(TextReader textReader, string inputName,
            string header, bool includeWholeLine, char separator, bool checkHeaderMatch, bool includeHeaderAsFirstLine)
        {
            bool continueIfEmptyFileFound = false;
            return TabFileTable(textReader, inputName, header, includeWholeLine, separator, checkHeaderMatch, includeHeaderAsFirstLine, continueIfEmptyFileFound);
        }

        [Obsolete("Use TabReader")]
        static public IEnumerable<Dictionary<string, string>> TabFileTable(TextReader textReader, string inputName,
            string header, bool includeWholeLine, char separator, bool checkHeaderMatch, bool includeHeaderAsFirstLine, bool continueIfEmptyFileFound)
        {
            return TabReader(textReader, inputName, header, includeWholeLine, separator, checkHeaderMatch, includeHeaderAsFirstLine, continueIfEmptyFileFound);
        }

        static public IEnumerable<Dictionary<string, string>> TabReader(TextReader textReader, string inputName,
            string header, bool includeWholeLine = false, char separator = '\t', bool checkHeaderMatch = true, bool includeHeaderAsFirstLine = false, bool continueIfEmptyFileFound = false, bool fileHasNoHeader = false)
        {
            Helper.CheckCondition<ArgumentException>(!fileHasNoHeader || !includeHeaderAsFirstLine && !checkHeaderMatch, "Inconsistent arguments. If file has no header, can't return the header or check if the header is a match!");
            //checkHeaderMatch = false;

            string line = textReader.ReadLine();

            if (!fileHasNoHeader)
            {
                Helper.CheckCondition(null != line, "Input is empty so can't read header. " + inputName);

                if (checkHeaderMatch)
                {
                    Helper.CheckCondition(line.Equals(header, StringComparison.CurrentCultureIgnoreCase), "The input doesn't have the exact expected header.\nEXPECTED:\n {0}\nOBSERVED:\n{1}\nINPUT NAME:\n{2}", header, line, inputName); //!!!raise error
                }
                else
                {
                    header = line;
                }
            }

            string[] headerCollection = header.Split(separator);

            //            Helper.CheckCondition(null == expandSingleValueDelgateOrNull || headerCollection.Length > 1, "If pvalue only lines are allowed, then the header must have more than one column");

            //while (null != (line = textReader.ReadLine()))
            bool firstTime = true;
            Dictionary<string, string> previousRow = null;

            // use do-while so we can return header as the first row, if requested.
            do
            {
                if (!fileHasNoHeader && firstTime && !includeHeaderAsFirstLine)
                {
                    firstTime = false;
                    continue;
                }

                if (line.Length == 0) continue;

                string[] fieldCollection = line.Split(separator);
                //if (checkHeaderMatch)
                //{
                //    Helper.CheckCondition(fieldCollection.Length == headerCollection.Length || (null != expandSingleValueDelgateOrNull && fieldCollection.Length == 1),
                //        "The input doesn't have the expected number of columns. Header Length:{0}, LineLength:{1}\nHeader:{2}\nLine:{3}\nInputName:{4}",
                //        headerCollection.Length, fieldCollection.Length, header, line, inputName);
                //}

                // if we're not checking for a header match, we still can't deal with lines of the wrong length. just ignore them.
                //if ((fieldCollection.Length != headerCollection.Length) && (expandSingleValueDelgateOrNull == null))
                //    continue;

                Dictionary<string, string> row = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
                if (includeWholeLine)
                {
                    row.Add("", line);
                }
                //if (null != expandSingleValueDelgateOrNull && fieldCollection.Length == 1)
                //{
                //    row = expandSingleValueDelgateOrNull(fieldCollection[0], previousRow);
                //}
                //else
                {
                    for (int iField = 0; iField < fieldCollection.Length; ++iField)
                    {
                        if (headerCollection[iField] == "")
                        {
                            Helper.CheckCondition(fieldCollection[iField] == "", "Cannot have an unnamed column if that column contains data.");
                        }
                        else
                        {
                            if (!row.ContainsKey(headerCollection[iField]))
                            {
                                row.Add(headerCollection[iField], fieldCollection[iField]);
                            }
                        }
                    }
                }
                yield return row;
                previousRow = row;

            } while (null != (line = textReader.ReadLine()));
        }
        //General
        public static List<Dictionary<string, string>> TabFileTableAsList(Assembly assembly, string resourcePrefix, string filename, string header, bool includeWholeLine)
        {
            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            foreach (Dictionary<string, string> row in TabFileTable(assembly, resourcePrefix, filename, header, includeWholeLine, '\t', true))
            {
                list.Add(row);
            }
            return list;
        }

        /// <summary>
        /// Returns the value associated with key in the dictionary. If not present, adds the default value to the dictionary and returns that
        /// value.
        /// </summary>
        [Obsolete("This has be superseded  by 'dictionary.GetValueOrDefault(key)'")]
        public static TValue GetValueOrDefault<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
        {
            return dictionary.GetValueOrDefault(key);
        }

        /// <summary>
        /// Returns the value associated with key in the dictionary. If not present, adds the default value to the dictionary and returns that
        /// value.
        /// </summary>
        [Obsolete("This has be superseded  by 'dictionary.GetValueOrDefault(key, defaultValue)'")]
        public static TValue GetValueOrDefault<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            return dictionary.GetValueOrDefault(key, defaultValue);
        }

        public static StreamReader OpenResource(Assembly assembly, string prefix, string fileName)
        {
            string s = prefix + fileName;
            Stream stream = assembly.GetManifestResourceStream(s);
            Helper.CheckCondition(stream != null, "Couldn't find input resource " + s);
            StreamReader streamReader = new StreamReader(stream);
            return streamReader;
        }

        //public static StreamReader OpenResourceOrFile(Assembly assembly, string resourcePrefix, string filename)
        //{
        //    return OpenResourceOrFile(assembly, resourcePrefix, filename);
        //}

        public static StreamReader OpenResourceOrFile(Assembly assembly, string resourcePrefix, FileInfo fileinfo)
        {
            return OpenResourceOrFile(assembly, resourcePrefix, fileinfo.FullName);
        }

        public static StreamReader OpenResourceOrFile(Assembly assembly, string resourcePrefix, string fileName)
        {
#if !SILVERLIGHT
           FileInfo file = new FileInfo(fileName);
            if (file.Exists || resourcePrefix == null)
            {
                return OpenTextOrZippedText(file);
            }
            else
#endif
            {
                return OpenResource(assembly, resourcePrefix, fileName);
            }
        }

#if !SILVERLIGHT
        public static void GzipFile(string filename, bool deleteOrigFile)
        {
            FileStream sourceFile = File.OpenRead(filename);
            FileStream destFile = File.Create(filename + ".gz");

            GZipStream compStream = new GZipStream(destFile, CompressionMode.Compress);
            try
            {
                int theByte = sourceFile.ReadByte();
                while (theByte != -1)
                {
                    compStream.WriteByte((byte)theByte);
                    theByte = sourceFile.ReadByte();
                }
            }
            finally
            {
                compStream.Dispose();
                sourceFile.Dispose();
                destFile.Dispose();
            }
            if (deleteOrigFile)
            {                
                FileUtils.DeleteFilePatiently(filename);
            }
        }

#endif

        public static StreamReader OpenTextOrZippedText(FileInfo file)
        {
            if (file.Extension.Equals(".gz", StringComparison.CurrentCultureIgnoreCase) ||
                file.Extension.Equals(".gzip", StringComparison.CurrentCultureIgnoreCase))
            {
#if SILVERLIGHT
                  throw new Exception("Compressed files are not supported in Silverlight.");
#else
                FileStream infile = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
                GZipStream zipStream = new GZipStream(infile, CompressionMode.Decompress);
                StreamReader reader = Bio.Util.FileUtils.StripComments(zipStream);//new StreamReader(zipStream);
                
                return reader;
#endif
            }
            else
            {
                return file.OpenTextStripComments();//file.OpenText();
            }
        }


        public static StreamReader OpenTextOrZippedText(string filename)
        {
            FileInfo file = new FileInfo(filename);
            return OpenTextOrZippedText(file);
            //FileInfo fileInfo = new FileInfo(filename);
            //if (fileInfo.Extension.Equals(".gz", StringComparison.CurrentCultureIgnoreCase) ||
            //    fileInfo.Extension.Equals(".gzip", StringComparison.CurrentCultureIgnoreCase))
            //{
            //    FileStream infile = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            //    GZipStream zipStream = new GZipStream(infile, CompressionMode.Decompress);
            //    StreamReader reader = FileUtils.StripComments(zipStream);//new StreamReader(zipStream);
            //    return reader;
            //}
            //else
            //{
            //    return Bio.Util.FileUtils.OpenTextStripComments(filename);
            //    //return fileInfo.OpenText();
            //}
        }


        /// <summary>
        /// Does not reuse or modify the input or output itemCollection
        /// </summary>
        public static IEnumerable<List<T>> EveryPermutation<T>(List<T> itemCollection)
        {
            Helper.CheckCondition(itemCollection.Count >= 0);
            if (itemCollection.Count == 0)
            {
                yield return new List<T>();
            }
            else
            {
                for (int i = 0; i < itemCollection.Count; ++i)
                {
                    T first = itemCollection[i];
                    List<T> rest = new List<T>(itemCollection);
                    rest.RemoveAt(i);
                    foreach (List<T> restPerm in EveryPermutation(rest))
                    {
                        List<T> returnThis = new List<T>(itemCollection.Count);
                        returnThis.Add(first);
                        returnThis.AddRange(restPerm);
                        yield return returnThis;
                    }
                }
            }
        }


        public static IEnumerable<List<T>> EveryCombination<T>(IEnumerable<IEnumerable<T>> listList)
        {
            IEnumerable<T> choicesFor1stPosition;
            if (TryFirst<IEnumerable<T>>(listList, out choicesFor1stPosition))
            {
                foreach (T t in choicesFor1stPosition)
                {
                    foreach (List<T> comboFromRest in EveryCombination(listList.Skip(1)))
                    {
                        List<T> combination = new List<T>();
                        combination.Add(t);
                        combination.AddRange(comboFromRest);
                        yield return combination;
                    }
                }
            }
            else
            {
                yield return new List<T>();
            }
        }


        public static IEnumerable<List<T>> EveryCombinationOfListEnum<T>(IEnumerable<List<T>> listList)
        {
            List<T> choicesFor1stPosition;
            if (TryFirst<List<T>>(listList, out choicesFor1stPosition))
            {
                foreach (T t in choicesFor1stPosition)
                {
                    foreach (List<T> comboFromRest in EveryCombinationOfListEnum(listList.Skip(1)))
                    {
                        List<T> combination = new List<T>();
                        combination.Add(t);
                        combination.AddRange(comboFromRest);
                        yield return combination;
                    }
                }
            }
            else
            {
                yield return new List<T>();
            }
        }


        //Why can't this be combined with the previous one?
        public static IEnumerable<List<T>> EveryCombination<T>(IEnumerable<Set<T>> listSet)
        {
            Set<T> choicesFor1stPosition;
            if (TryFirst<Set<T>>(listSet, out choicesFor1stPosition))
            {
                foreach (T t in choicesFor1stPosition)
                {
                    foreach (List<T> comboFromRest in EveryCombination(listSet.Skip(1)))
                    {
                        List<T> combination = new List<T>();
                        combination.Add(t);
                        combination.AddRange(comboFromRest);
                        yield return combination;
                    }
                }
            }
            else
            {
                yield return new List<T>();
            }
        }

#if !SILVERLIGHT
        //Why can't this be combined with the previous one?
        public static IEnumerable<List<T>> EveryCombination<T>(IEnumerable<HashSet<T>> listSet)
        {
            HashSet<T> choicesFor1stPosition;
            if (TryFirst<HashSet<T>>(listSet, out choicesFor1stPosition))
            {
                foreach (T t in choicesFor1stPosition)
                {
                    foreach (List<T> comboFromRest in EveryCombination(listSet.Skip(1)))
                    {
                        List<T> combination = new List<T>();
                        combination.Add(t);
                        combination.AddRange(comboFromRest);
                        yield return combination;
                    }
                }
            }
            else
            {
                yield return new List<T>();
            }
        }
#endif

        //!!!Why have both Join and CreateDelimitedString And CreateTabString?
        //[Obsolete("This has be superseded  by 'ienum.StringJoin(separator)'")]
        //public static string Join<T>(string separator, IEnumerable<T> list)
        //{
        //    return CreateDelimitedString2(separator, list);
        //}

        public static void AddWithCheck<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                Helper.CheckCondition(dictionary[key].Equals(value), "Key '{0}' added to dictionary with both value '{1}' and value '{2}'", key, dictionary[key], value);
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        //public static IEnumerable<Dictionary<string, string>> TabFileTable(string fileInput, string header, bool includeWholeLine)
        //{
        //	return SpecialFunctions.TabFileTable(Assembly.GetExecutingAssembly(), "VirusCount.DataFiles.", fileInput, header, includeWholeLine); //!!!const
        //}
        static public List<Dictionary<string, string>> TabFileTableAsList(string fileInput, string header, bool includeWholeLine)
        {
            return SpecialFunctions.TabFileTableAsList(Assembly.GetExecutingAssembly(), "VirusCount.DataFiles.", fileInput, header, includeWholeLine); //!!!const
        }



        [Obsolete("This has be superseded  by the System.Linq Count<T> extension method")]
        public static int Count<T>(IEnumerable<T> enumerable)
        {
            // no sense in the long version if there's a shortcut.
            if (enumerable is ICollection<T>)
            {
                return ((ICollection<T>)enumerable).Count;
            }

            int count = 0;
            foreach (T item in enumerable)
            {
                ++count;
            }
            return count;
        }


#if !SILVERLIGHT
        //!!!should make sure that T is sortable?
        static public IList<T> GetRandomSortedCollection<T>(int goalCount, IList<T> list, ref Random random)
        {
            //!!!could make a version that is efficient even if all are wanted
            Helper.CheckCondition(goalCount * 2 <= list.Count, "Number of items desired must be less than half the number available.");
            SortedList<T, bool> results = new SortedList<T, bool>();
            GetRandomSortedCollectionInternal(goalCount, list, ref random, ref results);
            return results.Keys;
        }

        static private void GetRandomSortedCollectionInternal<T>(int goalCount, IList<T> list, ref Random random, ref SortedList<T, bool> results)
        {
            if (goalCount == 0)
            {
                return;
            }

            while (true)
            {
                T randomItem = list[random.Next(list.Count)];
                if (!results.ContainsKey(randomItem))
                {
                    results.Add(randomItem, true);
                    GetRandomSortedCollectionInternal(goalCount - 1, list, ref random, ref results);
                    return;
                }
            }
        }
#endif


        static public IEnumerable<Dictionary<string, string>> TabFileTableNoHeaderInFile(string filename, string header, bool includeWholeLine)
        {
            return TabFileTableNoHeaderInFile(null, null, filename, header, includeWholeLine, '\t');
        }

        static public IEnumerable<Dictionary<string, string>> TabFileTableNoHeaderInFile(string filename, string header, char separator)
        {
            return TabFileTableNoHeaderInFile(null, null, filename, header, false, separator);
        }

        public static IEnumerable<Dictionary<string, string>> TabFileTableNoHeaderInFile(Assembly assembly, string resourcePrefix, string filename, string header, bool includeWholeLine, char separator)
        {
            using (StreamReader streamReader = OpenResourceOrFile(assembly, resourcePrefix, filename))
            {
                string line = null;
                string[] headerCollection = header.Split(separator);
                while (null != (line = streamReader.ReadLine()))
                {
                    string[] fieldCollection = line.Split(separator);
                    if (fieldCollection.Length <= 1 && headerCollection.Length > 1)
                        continue;

                    Helper.CheckCondition(fieldCollection.Length == headerCollection.Length); //!!!raise error

                    Dictionary<string, string> row = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
                    if (includeWholeLine)
                    {
                        row.Add("", line);
                    }
                    for (int iField = 0; iField < fieldCollection.Length; ++iField)
                    {
                        if (headerCollection[iField] == "")
                        {
                            Helper.CheckCondition(fieldCollection[iField] == "");
                        }
                        else
                        {
                            if (!row.ContainsKey(headerCollection[iField]))
                            {
                                row.Add(headerCollection[iField], fieldCollection[iField]);
                            }
                            else
                            {
                                if (row[headerCollection[iField]] != fieldCollection[iField])
                                {
                                    try
                                    {
                                        double r1 = double.Parse(row[headerCollection[iField]]);
                                        double r2 = double.Parse(fieldCollection[iField]);
                                        Debug.Assert(Math.Abs(r1 - r2) < .000000001);
                                    }
                                    finally
                                    {
                                    }
                                }
                            }
                        }
                    }
                    //Dictionary<string, string> row = new Dictionary<string, string>(rowX, StringComparer.CurrentCultureIgnoreCase);
                    yield return row;
                }
            }
        }

        [Obsolete("Use the linq version.")]
        public static double Sum(IEnumerable<double> doubleList)
        {
            double total = 0.0;
            foreach (double r in doubleList)
            {
                total += r;
            }
            return total;
        }

        //!!!If C# allowed generic types to be constrained numbers the versions of Sum could be combined.
        [Obsolete("Use the linq version.")]
        public static int Sum(IEnumerable<int> intList)
        {
            int total = 0;
            foreach (int r in intList)
            {
                total += r;
            }
            return total;
        }

        [Obsolete("Use the linq version.")]
        public static double Mean(IEnumerable<double> doubleList)
        {
            int count = 0;
            double total = 0.0;
            foreach (double r in doubleList)
            {
                total += r;
                ++count;
            }

            Helper.CheckCondition(count > 0, "Mean of zero items is not defined.");
            double mean = total / (double)count;
            return mean;
        }

        [Obsolete("This has be superseded  by the List.Shuffle extension method")]
        public static List<T> Shuffle<T>(IEnumerable<T> input, ref Random random)
        {
            List<T> list = new List<T>();
            foreach (T t in input)
            {
                list.Add(t); //We put the value here to get the new space allocated
                int oldIndex = random.Next(list.Count);
                list[list.Count - 1] = list[oldIndex];
                list[oldIndex] = t;
            }
            return list;
        }


        [Obsolete("This has be superseded  by the List.ShuffleInPlace extension method")]
        public static void ShuffleInPlace<T>(ref List<T> listToShuffle, ref Random random)
        {
            for (int i = 0; i < listToShuffle.Count; i++)
            {
                int j = random.Next(listToShuffle.Count);
                Swap(listToShuffle, i, j);
            }
        }


        [Obsolete("This has be superseded  by the List.Swap extension method")]
        private static void Swap<T>(List<T> list, int i, int j)
        {
            T tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }

        public static double ComputeFDR(double testStatistic,
            List<double> sortedPooledPValueCollectionFromData,
            List<double> sortedPooledPValueCollectionFromRandomization, // "potentially significant" randomized pValues
            double numberOfRandomizationRuns, out double pValFromRandomization,   // number of null runs (eg, if ran -1-50, number of null runs is 50)
            bool doLocalTabulation
            )
        {

            int numRealHypotheses = sortedPooledPValueCollectionFromData.Count;
            double smoothingFudge = 1.0;
            int dataAssociationsBelowOrAt = CountItemsBelowOrAt(sortedPooledPValueCollectionFromData, testStatistic);
            int randomizedAssociationsBelowOrAt = CountItemsBelowOrAt(sortedPooledPValueCollectionFromRandomization, testStatistic);

            pValFromRandomization = ((double)randomizedAssociationsBelowOrAt + smoothingFudge) /
                                    ((double)sortedPooledPValueCollectionFromRandomization.Count + smoothingFudge);//smooth it by 1 always so decreasing permutations doesn't give better p-values
            Helper.CheckCondition(pValFromRandomization <= 1 && pValFromRandomization >= 0, "pValFromRandomization must not have been computed properly");

            // if all of pValue is no better than any of the randomization runs
            // return a flag value that says we know nothing except that this is a bad pValue.
            // It may be that the correct answer is 1,
            // but in order to allow data compression, we need to be agnostic.
            if (randomizedAssociationsBelowOrAt == sortedPooledPValueCollectionFromRandomization.Count)
            {
                return 10.0;
            }

            double fdr;
            if (!doLocalTabulation)
            {
                // do logs to prevent over/under flow.
                fdr = randomizedAssociationsBelowOrAt /
                     (double)(dataAssociationsBelowOrAt * numberOfRandomizationRuns);
            }
            else //doing local
            {
                Helper.CheckCondition(randomizedAssociationsBelowOrAt <= numberOfRandomizationRuns);//numRealHypotheses);
                fdr = double.NaN;//we'll compute this outside this method
            }

            return fdr;
        }


        static public int CountItemsBelowOrAt(List<double> sortedPooledPValueCollection, double pValue)
        {
            int index = sortedPooledPValueCollection.BinarySearch(pValue);
            if (index < 0)
            {
                index = ~index;
            }
            else
            {
                // if pValue is in the collection, BinarySearch returns one of the instances, with no guarantee
                // of which one. thus, we keep incrementing index until it's equal to the size of the collection
                // or it points to the first element in the collection that is strictly greater than pValue.
                while (++index < sortedPooledPValueCollection.Count &&
                    sortedPooledPValueCollection[index] <= pValue) { }
            }


            //Check that sorted
            if (index < sortedPooledPValueCollection.Count - 1)
            {
                Helper.CheckCondition(sortedPooledPValueCollection[index] <= sortedPooledPValueCollection[index + 1],
                    "sortedPooledPValueCollection isn't sorted: " +
                    String.Format("sortedPooled...[{0}] = {1}; sortedPolled...[{2}] = {3}",
                        index, sortedPooledPValueCollection[index], index + 1, sortedPooledPValueCollection[index + 1])
                    );
            }

            return index;
        }

        public static int ListCompareTo<T>(IList<T> l1, IList<T> l2) where T : IComparable
        {
            switch (l1.Count.CompareTo(l2.Count))
            {
                case 1:
                    return 1;
                case -1:
                    return -1;
                case 0:
                    {

                        for (int i1 = 0; i1 < l1.Count; ++i1)
                        {
                            int ci = l1[i1].CompareTo(l2[i1]);
                            if (ci != 0)
                            {
                                return ci;
                            }
                        }
                        return 0;
                    }
                default:
                    throw new Exception("assert");
            }
        }


        public static int DoubleGreaterThan(double r1, double r2)
        {
            return r1.CompareTo(r2);
        }

        public static int DoubleLessThan(double r1, double r2)
        {
            return r2.CompareTo(r1);
        }

        public static int IntGreaterThan(int i1, int i2)
        {
            return i1.CompareTo(i2);
        }

        public static int IntLessThan(int i1, int i2)
        {
            return i2.CompareTo(i1);
        }

        public static Dictionary<TRow, double> ComputeQValues<TRow>(
                ref List<TRow> realRowCollectionToSort,
                Converter<TRow, double> AccessValueFromRow,
                ref List<double> nullValueCollectionToBeSorted, // only those pValues that are potentially of some interest
                double numberOfRandomizationRuns) // should be around 0..50. if way more, you don't understand what this is. it's the number of runs, not trials.
        {

            if (numberOfRandomizationRuns > 0)
            {
                return ComputeQValuesUseNulls(ref realRowCollectionToSort, AccessValueFromRow, ref nullValueCollectionToBeSorted, numberOfRandomizationRuns);
            }
            else
            {
                return ComputeQValuesUseStoreyTibsharani(ref realRowCollectionToSort, AccessValueFromRow, ref nullValueCollectionToBeSorted);
            }
        }

        public static Dictionary<TRow, double> ComputeQValuesUseNulls<TRow>(
                       ref List<TRow> realRowCollectionToSort,
                       Converter<TRow, double> AccessValueFromRow,
                       ref List<double> nullValueCollectionToBeSorted, // only those pValues that are potentially of some interest
                       double numberOfRandomizationRuns) // should be around 1..50. if way more, you don't understand what this is. it's the number of runs, not trials.
        {
            Dictionary<double, double> garb;
            return ComputeQValuesUseNulls(ref realRowCollectionToSort, AccessValueFromRow, ref nullValueCollectionToBeSorted, numberOfRandomizationRuns, out garb);
        }


        public static Dictionary<TRow, double> ComputeQValuesUseNulls<TRow>(
                ref List<TRow> realRowCollectionToSort,
                Converter<TRow, double> AccessValueFromRow,
                ref List<double> nullValueCollectionToBeSorted, // only those pValues that are potentially of some interest
                double numberOfRandomizationRuns,// should be around 1..50. if way more, you don't understand what this is. it's the number of runs, not trials.
               out Dictionary<double, double> pValToPvalFromRandomizations)
        {
            pValToPvalFromRandomizations = new Dictionary<double, double>();//to explicitly hold the p-values computed from the randomizations
            //rowindexToPvalFromPerms = new Dictionary<double, double>();

            realRowCollectionToSort.Sort(delegate(TRow row1, TRow row2)
            {
                double pValue1 = AccessValueFromRow(row1);
                double pValue2 = AccessValueFromRow(row2);
                return pValue1.CompareTo(pValue2);
            });
            nullValueCollectionToBeSorted.Sort();

            //!!! untested: I think this will work if we just take out the explicit type for the generic...
            //#if SILVERLIGHT
            //              List<double> realPValueCollectionSorted = realRowCollectionToSort.ConvertAll<TRow, double>(AccessValueFromRow);
            //#else
            //              List<double> realPValueCollectionSorted = realRowCollectionToSort.ConvertAll<double>(AccessValueFromRow);
            //#endif

            List<double> realPValueCollectionSorted = realRowCollectionToSort.Select(item => AccessValueFromRow(item)).ToList();

            //// TEMPORARY!!!
            //for (int i = 0; i < 50; i++)
            //{
            //	Console.WriteLine("Real: {0} Null: {1}", realPValueCollectionSorted[i], nullValueCollectionToBeSorted[i]);
            //}

            double qValueRunning = double.PositiveInfinity;
            Dictionary<double, double> pValueToFdr = new Dictionary<double, double>();  //Fdr = false discovery rate
            Dictionary<double, double> pValueToQValue = new Dictionary<double, double>();
            realRowCollectionToSort.Reverse(); //Go through backwards so can compute qValue in one pass
            foreach (TRow row in realRowCollectionToSort)
            {
                double testStatistic = AccessValueFromRow(row);//formerly called pValue
                double pValFromRandomization;
                double fdr = ComputeFDR(testStatistic, realPValueCollectionSorted, nullValueCollectionToBeSorted, numberOfRandomizationRuns, out pValFromRandomization, false);
                pValueToFdr[testStatistic] = fdr; // OK to override value because will be the same
                qValueRunning = Math.Min(qValueRunning, fdr);
                pValueToQValue[testStatistic] = qValueRunning; // OK to override value because will be the same
                pValToPvalFromRandomizations[testStatistic] = pValFromRandomization;
            }
            realRowCollectionToSort.Reverse();

            Dictionary<TRow, double> results = new Dictionary<TRow, double>();
            foreach (TRow row in realRowCollectionToSort)
            {
                double testStatistic = AccessValueFromRow(row);
                double fdr = pValueToFdr[testStatistic];
                double qValueFinal = pValueToQValue[testStatistic];
                results[row] = qValueFinal; // OK to override value because will be the same
                //pValFromPermsToReturn[row] = pValToPvalFromRandomizations[testStatistic];
            }
            return results;
        }

        /// <summary>
        /// column "groupId" is used to do local p-value tabulations, if that's asked for
        /// </summary>
        /// <typeparam name="TRow"></typeparam>
        /// <param name="realRowCollectionToSort"></param>
        /// <param name="nullValueCollectionToBeSorted"></param>
        /// <param name="numberOfRandomizationRuns"></param>
        /// <param name="doLocalTabulation"></param>
        /// <param name="AccessGroupIdFromRow"></param>
        /// <param name="AccessRowIndexFromRow"></param>
        /// <param name="AccessTestStatistcValueFromRow"></param>
        /// <param name="rowToPvalFromRandomizations"></param>
        /// <returns></returns>
        public static Dictionary<TRow, double> ComputeQValuesUseNulls<TRow>(
                       ref List<TRow> realRowCollectionToSort,
                       Converter<TRow, double> AccessTestStatistcValueFromRow,
                       Converter<TRow, int> AccessGroupIdFromRow,
                       Converter<TRow, int> AccessRowIndexFromRow,
            //Converter<TRow,int> AccessRowAndCovertToGroupIdForLocalTabulation,
                       ref Dictionary<int, List<double>> nullValueCollectionToBeSorted, // only those pValues that are potentially of some interest
                       double numberOfRandomizationRuns,// should be around 1..50. if way more, you don't understand what this is. it's the number of runs, not trials.
                      out Dictionary<double, double> rowToPvalFromRandomizations,
                      bool doLocalTabulation)
        {
            realRowCollectionToSort.Sort(delegate(TRow row1, TRow row2)
            {
                double pValue1 = AccessTestStatistcValueFromRow(row1);
                double pValue2 = AccessTestStatistcValueFromRow(row2);
                return pValue1.CompareTo(pValue2);
            });

            rowToPvalFromRandomizations = new Dictionary<double, double>();//to explicitly hold the p-values computed from the randomizations

            foreach (int key in nullValueCollectionToBeSorted.Keys)
            {
                nullValueCollectionToBeSorted[key].Sort();
            }

            //!!! untested: I think this will work if we just take out the explicit type for the generic...
            //#if SILVERLIGHT
            //              List<double> realPValueCollectionSorted = realRowCollectionToSort.ConvertAll<TRow, double>(AccessValueFromRow);
            //#else
            //              List<double> realPValueCollectionSorted = realRowCollectionToSort.ConvertAll<double>(AccessValueFromRow);
            //#endif

            List<double> realPValueCollectionSorted = realRowCollectionToSort.Select(item => AccessTestStatistcValueFromRow(item)).ToList();

            //// TEMPORARY!!!
            //for (int i = 0; i < 50; i++)
            //{
            //	Console.WriteLine("Real: {0} Null: {1}", realPValueCollectionSorted[i], nullValueCollectionToBeSorted[i]);
            //}
            int numRealHypotheses = realPValueCollectionSorted.Count;

            double qValueRunning = double.PositiveInfinity;
            Dictionary<double, double> pValueToFdr = new Dictionary<double, double>();  //Fdr = false discovery rate
            Dictionary<double, double> pValueToQValue = new Dictionary<double, double>();
            realRowCollectionToSort.Reverse(); //Go through backwards so can compute qValue in one pass

            Dictionary<TRow, double> results = new Dictionary<TRow, double>();

            if (!doLocalTabulation)
            {
                foreach (TRow row in realRowCollectionToSort)
                {
                    int groupId = 0;
                    double testStatistic = AccessTestStatistcValueFromRow(row);//formerly called pValue
                    double pValFromRandomization;
                    List<double> nullValueCollectionToBeSortedForOneIndex = nullValueCollectionToBeSorted[groupId];
                    double fdr = ComputeFDR(testStatistic, realPValueCollectionSorted, nullValueCollectionToBeSortedForOneIndex, numberOfRandomizationRuns, out pValFromRandomization, doLocalTabulation);
                    pValueToFdr[testStatistic] = fdr; // OK to override value because will be the same
                    rowToPvalFromRandomizations.Add(AccessRowIndexFromRow(row), pValFromRandomization);
                    qValueRunning = Math.Min(qValueRunning, fdr);
                    pValueToQValue[testStatistic] = qValueRunning; // OK to override value because will be the same
                }

                //realRowCollectionToSort.Reverse();

                foreach (TRow row in realRowCollectionToSort)
                {
                    double testStatistic = AccessTestStatistcValueFromRow(row);
                    double fdr = pValueToFdr[testStatistic];
                    double qValueFinal = pValueToQValue[testStatistic];
                    results[row] = qValueFinal; // OK to override value because will be the same
                    //pValFromPermsToReturn[row] = pValToPvalFromRandomizations[testStatistic];
                }
            }
            else//do local tabulation in which case first have to get all pVals from randomizatoins, and then get qvals
            {
                //can no longer map the testStatistic/p-value to randomizationPvalue, because it's a local mapping

                Dictionary<double, double> pValMapping;
                //this just gets the p-values from randomization--not really getting FDR. that comes after
                foreach (TRow row in realRowCollectionToSort)
                {
                    int groupId = AccessGroupIdFromRow(row);
                    double testStatistic = AccessTestStatistcValueFromRow(row);//formerly called pValue
                    double pValFromRandomization;
                    List<double> nullValueCollectionToBeSortedForOneIndex = nullValueCollectionToBeSorted[groupId];
                    double fdr = ComputeFDR(testStatistic, realPValueCollectionSorted, nullValueCollectionToBeSortedForOneIndex, numberOfRandomizationRuns, out pValFromRandomization, doLocalTabulation);
                    //pValueToFdr[testStatistic] = fdr; // OK to override value because will be the same
                    //qValueRunning = Math.Min(qValueRunning, fdr);
                    //pValueToQValue[testStatistic] = qValueRunning; // OK to override value because will be the same
                    rowToPvalFromRandomizations.Add(AccessRowIndexFromRow(row), pValFromRandomization);
                }


                /* compute the FDRs now from the randomized p-values
                    but note that hypotheses are not necessarily in the right order now, so I need to resort, or else do a two-pass
                    */
                qValueRunning = double.PositiveInfinity;
                List<double> allRealRandomizedPvaluesSorted = rowToPvalFromRandomizations.Values.ToList();
                allRealRandomizedPvaluesSorted.Sort();

                //need a copy of it to use inside a lambda expression
                pValMapping = rowToPvalFromRandomizations;

                //       mist.Sort((x, y) => -x.CompareTo(y));
                realRowCollectionToSort.Sort((elt1, elt2) => pValMapping[AccessRowIndexFromRow(elt1)].CompareTo(
                                                            pValMapping[AccessRowIndexFromRow(elt2)]));
                //realRowCollectionToSort.Sort((elt1, elt2) => pValMapping[AccessTestStatistcValueFromRow(elt1)].CompareTo(
                //                                                            pValMapping[AccessTestStatistcValueFromRow(elt2)]));
                realRowCollectionToSort.Reverse();//so can do q-values in one pass
                //Helper.CheckCondition(realRowCollectionToSort[0] > realRowCollectionToSort[realRowCollectionToSort.Count - 1], "wrong order");

                foreach (TRow row in realRowCollectionToSort)
                {
                    int thisRow = AccessRowIndexFromRow(row);
                    double thisPvalFromRand = rowToPvalFromRandomizations[thisRow];
                    int dataAssociationsBelowOrAt = CountItemsBelowOrAt(allRealRandomizedPvaluesSorted, thisPvalFromRand);
                    double fdr = numRealHypotheses * thisPvalFromRand / dataAssociationsBelowOrAt;
                    pValueToFdr[thisRow] = fdr; // OK to override value because will be the same
                    qValueRunning = Math.Min(qValueRunning, fdr);
                    pValueToQValue[thisRow] = qValueRunning; // OK to override value because will be the same
                }

                //realRowCollectionToSort.Sort((elt1, elt2) => pValMapping[AccessTestStatistcValueFromRow(elt1)].CompareTo(
                //                                            pValMapping[AccessTestStatistcValueFromRow(elt2)]));
                realRowCollectionToSort.Sort((elt1, elt2) => pValMapping[AccessRowIndexFromRow(elt1)].CompareTo(
                                                                         pValMapping[AccessRowIndexFromRow(elt2)]));
                //realRowCollectionToSort.Reverse();

                foreach (TRow row in realRowCollectionToSort)
                {
                    int thisRow = AccessRowIndexFromRow(row);
                    double fdr = pValueToFdr[thisRow];
                    double qValueFinal = pValueToQValue[thisRow];
                    results[row] = qValueFinal; // OK to override value because will be the same
                    //pValFromPermsToReturn[row] = pValToPvalFromRandomizations[testStatistic];
                }
            }
            return results;
        }

        public static Dictionary<TRow, double> ComputeQValuesUseStoreyTibsharani<TRow>(
            ref List<TRow> realRowCollectionToSort,
            Converter<TRow, double> AccessValueFromRow,
            ref List<double> allPValues)
        {
            return ComputeQValuesUseStoreyTibsharani(ref realRowCollectionToSort, AccessValueFromRow, allPValues.Count);
        }

        //!!!very similar to other code
        //public static Dictionary<double, List<double>> ComputeQValuesUseStoreyTibsharani(
        public static Dictionary<double, double> ComputeQValuesUseStoreyTibsharani(
            ref List<double> lowPValueListToSort,
            long allPValuesCount)
        {
            Console.WriteLine("Computing Storey-Tibshirani q's over m={0}", allPValuesCount);

            lowPValueListToSort.Sort();

            Dictionary<double, double> results = new Dictionary<double, double>();
            double currentQ = double.MaxValue;
            for (int j = lowPValueListToSort.Count - 1; j >= 0; j--)
            {
                double pvalue = lowPValueListToSort[j];
                double eNumNull = pvalue * allPValuesCount;
                double numReal = j + 1;
                double fdr = eNumNull / numReal;
                fdr = Math.Min(fdr, 1.0);
                currentQ = Math.Min(fdr, currentQ);

                //results.GetValueOrDefault(pvalue).Add(currentQ);
                double thisQ;
                bool hasPvalAlready = results.TryGetValue(pvalue, out thisQ);
                if (hasPvalAlready)
                {
                    Helper.CheckCondition(thisQ == currentQ);
                }
                else
                {
                    results.Add(pvalue, currentQ);
                }
            }

            return results;
        }

        public static Dictionary<TRow, double> ComputeQValuesUseStoreyTibsharani<TRow>(
            ref List<TRow> realRowCollectionToSort,
            Converter<TRow, double> AccessValueFromRow,
            long allPValuesCount)
        {
            //allPValues.Sort();
            //int i = allPValues.BinarySearch(0.1);
            //if (i < 0)
            //    i = ~i;

            //double sum = 0;
            //int count = 0;
            //for (; i < allPValues.Count && allPValues[i] < 0.7; i++)
            //{
            //    count++;
            //    double estOfN = i / allPValues[i];
            //    sum += estOfN;
            //}
            //double avgEstOfN = sum / count;
            //double avgEstOfN = (from p in allPValues
            //             //where p < 1
            //             select p).Count();

            Console.WriteLine("Computing Storey-Tibshirani q's over m={0}", allPValuesCount);

            realRowCollectionToSort.Sort((row1, row2) => AccessValueFromRow(row1).CompareTo(AccessValueFromRow(row2)));

            Dictionary<TRow, double> results = new Dictionary<TRow, double>();
            double currentQ = 1.0;//double.MaxValue;
            for (int j = realRowCollectionToSort.Count - 1; j >= 0; j--)
            {
                double pvalue = AccessValueFromRow(realRowCollectionToSort[j]);
                //pvalue /= 2;    // TEMP!!
                double eNumNull = pvalue * allPValuesCount;
                double numReal = j + 1;
                double fdr = eNumNull / numReal;
                currentQ = Math.Min(fdr, currentQ);

                results[realRowCollectionToSort[j]] = currentQ;
            }

            return results;
        }

        ////!!!very similar to other code
        //public static Dictionary<double, List<double>> ComputeQValuesUseStoreyTibsharani(
        //    ref List<double> lowPValueListToSort,
        //    long allPValuesCount)
        //{
        //    Console.WriteLine("Computing Storey-Tibshirani q's over m={0}", allPValuesCount);

        //    lowPValueListToSort.Sort();

        //    Dictionary<double, List<double>> results = new Dictionary<double, List<double>>();
        //    double currentQ = double.MaxValue;
        //    for (int j = lowPValueListToSort.Count - 1; j >= 0; j--)
        //    {
        //        double pvalue = lowPValueListToSort[j];
        //        double eNumNull = pvalue * allPValuesCount;
        //        double numReal = j + 1;
        //        double fdr = eNumNull / numReal;
        //        currentQ = Math.Min(fdr, currentQ);

        //        results.GetValueOrDefault(pvalue).Add(currentQ);
        //    }

        //    return results;
        //}



        //Is there a new Linq Zip operator to do this? http://community.bartdesmet.net/blogs/bart/archive/2008/11/03/c-4-0-feature-focus-part-3-intermezzo-linq-s-new-zip-operator.aspx
        public static IEnumerable<KeyValuePair<T1, T2>> EnumerateTwo<T1, T2>(IEnumerable<T1> enum1, IEnumerable<T2> enum2, bool checkSameLength = true)
        {
            IEnumerator<T1> eor1 = enum1.GetEnumerator();
            IEnumerator<T2> eor2 = enum2.GetEnumerator();
            while (true)
            {
                bool b1 = eor1.MoveNext();
                bool b2 = eor2.MoveNext();
                if (!b1 || !b2)
                {
                    Helper.CheckCondition(!checkSameLength || b1 == b2, "Enumerations must be the same length");
                    break;
                }
                yield return new KeyValuePair<T1, T2>(eor1.Current, eor2.Current);
            }
        }

        //Is there a new Linq Zip operator to do this? http://community.bartdesmet.net/blogs/bart/archive/2008/11/03/c-4-0-feature-focus-part-3-intermezzo-linq-s-new-zip-operator.aspx
        public static IEnumerable<List<T>> EnumerateZip<T>(IEnumerable<IEnumerable<T>> queryOfQuery, bool checkSameLength = true)
        {
            List<IEnumerator<T>> enumeratorList = queryOfQuery.Select(query => query.GetEnumerator()).ToList();
            while (true)
            {
                List<bool> boolList = enumeratorList.Select(enumerator => enumerator.MoveNext()).ToList();
                if (!boolList.All(b => b)) // are any false?
                {
                    //if checkSameLength, then assert that all are false
                    Helper.CheckCondition(!checkSameLength || boolList.All(b => !b), "Enumerations must be the same length");
                    break;
                }

                List<T> currentList = enumeratorList.Select(enumerator => enumerator.Current).ToList();
                yield return currentList;
            }
        }

        public static IEnumerable<T> EnumerateSortedMerge<T, TKey>(IEnumerable<IEnumerable<T>> queryOfQuery, Func<T, TKey> keyExtractor)
        {
            List<IEnumerator<T>> enumeratorList = queryOfQuery.Select(query => query.GetEnumerator()).ToList();
            List<bool> boolList = enumeratorList.Select(enumerator => enumerator.MoveNext()).ToList();
            //If all false, quit
            if (boolList.All(b => !b))
            {
                yield break;
            }
            List<TKey> currentKeys = enumeratorList.Select(enumerator => keyExtractor(enumerator.Current)).ToList();
            while (true)
            {
                //Find the least item among those not at their end
                int indexOfLeastItem =
                    (from keyAndIndex in currentKeys.Select((key, index) => new KeyValuePair<TKey, int>(key, index))
                     where boolList[keyAndIndex.Value]
                     orderby keyAndIndex.Key
                     select keyAndIndex.Value
                     ).First();

                //yeild it
                IEnumerator<T> enumerator = enumeratorList[indexOfLeastItem];
                yield return enumerator.Current;
                //increment it
                boolList[indexOfLeastItem] = enumerator.MoveNext();
                currentKeys[indexOfLeastItem] = keyExtractor(enumerator.Current);
                //If all false, quit
                if (boolList.All(b => !b))
                {
                    yield break;
                }
            }
        }



        //Is there a new Linq Zip operator to do this? http://community.bartdesmet.net/blogs/bart/archive/2008/11/03/c-4-0-feature-focus-part-3-intermezzo-linq-s-new-zip-operator.aspx
        public static IEnumerable<KeyValuePair<T1, T2>> EnumerateTwoLonger<T1, T2>(IEnumerable<T1> enum1, IEnumerable<T2> enum2)
        {
            IEnumerator<T1> eor1 = enum1.GetEnumerator();
            IEnumerator<T2> eor2 = enum2.GetEnumerator();
            while (true)
            {
                bool b1 = eor1.MoveNext();
                bool b2 = eor2.MoveNext();
                if (!b1 && !b2)
                {
                    break;
                }

                yield return new KeyValuePair<T1, T2>(b1 ? eor1.Current : default(T1), b2 ? eor2.Current : default(T2));
            }
        }


        public static IEnumerable<List<T>> EnumerateSortedGroups<T, TKey>(IEnumerable<T> query, Func<T, TKey> keyExtractor)
        {
            IEnumerator<T> eor1 = query.GetEnumerator();

            TKey oldKey = default(TKey); //this might the the same as the first key seen. Must to the right thing.
            List<T> returnList = new List<T>();
            while (true)
            {
                bool b1 = eor1.MoveNext();
                if (!b1)
                {
                    break;
                }
                TKey newKey = keyExtractor(eor1.Current);
                if (!newKey.Equals(oldKey))
                {
                    if (returnList != null && returnList.Count > 0)
                    {
                        yield return returnList;
                    }
                    returnList = new List<T>();
                    oldKey = newKey;
                }
                returnList.Add(eor1.Current);
            }
            if (returnList != null && returnList.Count > 0)
            {
                yield return returnList;
            }
        }



        public static IEnumerable<KeyValuePair<T1, T2>> EnumerateCrossProduct<T1, T2>(IEnumerable<T1> enum1, IEnumerable<T2> enum2)
        {
            foreach (T1 elt1 in enum1)
            {
                foreach (T2 elt2 in enum2)
                {
                    yield return new KeyValuePair<T1, T2>(elt1, elt2);
                }
            }
        }

        public static IEnumerable<RowKeyColKeyValue<T1, T2, T3>> EnumerateCrossProduct<T1, T2, T3>(IEnumerable<T1> enum1, IEnumerable<T2> enum2, IEnumerable<T3> enum3)
        {
            foreach (T1 elt1 in enum1)
            {
                foreach (T2 elt2 in enum2)
                {
                    foreach (T3 elt3 in enum3)
                    {
                        yield return new Bio.Matrix.RowKeyColKeyValue<T1, T2, T3>(elt1, elt2, elt3);
                    }
                }
            }
        }


        ///// <summary>
        ///// Enumerates two enumerables.
        ///// Checks that enumerations are the same length
        ///// </summary>
        //public static IEnumerable<KeyValuePair<T1, T2>> EnumerateTwo<T1, T2>(IEnumerable<T1> enum1, IEnumerable<T2> enum2)
        //{
        //    return EnumerateTwo(enum1, enum2, true);
        //}

        /// <summary>
        /// Return each item in each enumerable.
        /// </summary>
        public static IEnumerable<T> EnumerateAll<T>(params IEnumerable<T>[] enumerables)
        {
            foreach (IEnumerable<T> enumerable in enumerables)
            {
                foreach (T item in enumerable)
                {
                    yield return item;
                }
            }
        }

        //public static bool ListEqual<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        //{
        //	foreach(KeyValuePair<T,T> itemAndItem in DoubleEnum(list1, list2))
        //	{
        //		if (!itemAndItem.Key.Equals(itemAndItem.Value))
        //		{
        //			return false;
        //		}
        //	}
        //	return true;
        //}

        [Obsolete("Moved to FileUtils")]
        public static IEnumerable<string> ReadEachLine(string fileName)
        {
            using (TextReader textReader = Bio.Util.FileUtils.OpenTextStripComments(fileName))
            {
                string line;
                while (null != (line = textReader.ReadLine()))
                {
                    yield return line;
                }
            }
        }

        [Obsolete("Moved to FileUtils")]
        public static IEnumerable<string> ReadEachLine(TextReader textReader)
        {
            string line;
            while (null != (line = textReader.ReadLine()))
            {
                yield return line;
            }
        }


        public static Dictionary<T1, T2> LoadDictionary<T1, T2>(string filename, string header = null, IEqualityComparer<T1> comparer = null)
        {
            using (TextReader reader = Bio.Util.FileUtils.OpenTextStripComments(filename))
            {
                return LoadDictionary<T1, T2>(reader, header, comparer);
            }
        }

        public static Dictionary<T1, T2> LoadDictionary<T1, T2>(FileInfo file, string header = null, IEqualityComparer<T1> comparer = null)
        {
            using (TextReader reader = Bio.Util.FileUtils.OpenTextStripComments(file))
            {
                return LoadDictionary<T1, T2>(reader, header, comparer);
            }
        }


        /// <summary>
        /// Reads a 2-column file into a dictionary.
        /// </summary>
        /// <typeparam name="T1">Type of the key, which is in the first column</typeparam>
        /// <typeparam name="T2">Type of the value, which is in the second column</typeparam>
        /// <param name="reader"></param>
        /// <param name="header">If not null, checks that the headers match.</param>
        /// <param name="comparer">If not null, will set the dictionary comparer to the specified comparer.</param>
        /// <returns></returns>
        public static Dictionary<T1, T2> LoadDictionary<T1, T2>(TextReader reader, string header = null, IEqualityComparer<T1> comparer = null)
        {
            Dictionary<T1, T2> result = comparer == null ? new Dictionary<T1, T2>() : new Dictionary<T1, T2>(comparer);
            bool firstLine = true;
            foreach (string line in Bio.Util.FileUtils.ReadEachLine(reader))
            {
                if (firstLine)
                {
                    Helper.CheckCondition(header == null || line.Equals(header, StringComparison.CurrentCultureIgnoreCase), "File header \"{0}\" doesn't match expected header \"{1}\"",
                        line, header);
                    firstLine = false;
                }
                else
                {
                    string[] fields = line.Split('\t');
                    Helper.CheckCondition<ParseException>(fields.Length == 2, "Expect a file with 2 tab-delimited fields. There are {0}", fields.Length);
                    T1 key = MBT.Escience.Parse.Parser.Parse<T1>(fields[0]);
                    T2 value = MBT.Escience.Parse.Parser.Parse<T2>(fields[1]);
                    result.Add(key, value);
                }
            }
            return result;
        }

        /// <summary>
        /// Parses each line using the Parse object and the type specifed by T. Will through an exception if a line can't be parsed.
        /// </summary>
        public static IEnumerable<T> ParseEachLine<T>(string filename)
        {
            using (TextReader reader = Bio.Util.FileUtils.OpenTextStripComments(filename))
            {
                return ParseEachLine<T>(reader);
            }
        }

        /// <summary>
        /// Parses each line using the Parse object and the type specifed by T. Will through an exception if a line can't be parsed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="textReader"></param>
        /// <returns></returns>
        public static IEnumerable<T> ParseEachLine<T>(TextReader textReader)
        {
            string line;
            while (null != (line = textReader.ReadLine()))
            {
                T parsedObj = MBT.Escience.Parse.Parser.Parse<T>(line);
                yield return parsedObj;
            }
        }

        public static IEnumerable<int> CreateRange(int c)
        {
            for (int i = 0; i < c; ++i)
            {
                yield return i;
            }
        }

        public static Dictionary<T2, T1> ReverseOneToOneDictionary<T1, T2>(IDictionary<T1, T2> dictionary)
        {
            Dictionary<T2, T1> result = new Dictionary<T2, T1>();
            foreach (KeyValuePair<T1, T2> t1AndT2 in dictionary)
            {
                result.Add(t1AndT2.Value, t1AndT2.Key);
            }
            return result;
        }

        public static Dictionary<T2, Set<T1>> ReverseOneToManyDictionary<T1, T2>(IDictionary<T1, T2> dictionary)
        {
            Dictionary<T2, Set<T1>> result = new Dictionary<T2, Set<T1>>();
            foreach (KeyValuePair<T1, T2> t1AndT2 in dictionary)
            {
                T1 t1 = t1AndT2.Key;
                T2 t2 = t1AndT2.Value;

                Set<T1> set = result.GetValueOrDefault(t2);
                set.AddNew(t1);
            }
            return result;
        }



        public static int CountIf<T>(IEnumerable<T> collection, Predicate<T> testDelegate)
        {
            int total = 0;
            foreach (T t in collection)
            {
                if (testDelegate(t))
                {
                    ++total;
                }
            }
            return total;
        }


        [Obsolete("This has be superseded by the System.Linq Skip<T>(1) extension method")]
        public static IEnumerable<T> Rest<T>(IEnumerable<T> args)
        {
            return args.Skip(1);
        }

        [Obsolete("This has be superseded by the System.Linq Skip<T> extension method")]
        public static IEnumerable<T> Rest<T>(IEnumerable<T> enumeration, int skipCount)
        {
            return enumeration.Skip(skipCount);
        }

        [Obsolete("This has be superseded  by the System.Linq First<T> extension method")]
        public static T First<T>(IEnumerable<T> enumeration)
        {
            T t;
            bool isOK = TryFirst(enumeration, out t);
            Helper.CheckCondition(isOK, "Can't get 1st item from an empty enumeration");
            return t;
        }


        public static IEnumerable<T> SubEnumeration<T>(IEnumerable<T> enumeration, int startIndex, int count)
        {
            foreach (T t in enumeration)
            {
                if (startIndex > 0)
                {
                    --startIndex;
                }
                else if (count > 0)
                {
                    --count;
                    yield return t;
                }
                else
                {
                    yield break;
                }
            }
        }

        public static IEnumerable<T> First<T>(IEnumerable<T> enumeration, int keepCount)
        {
            return SubEnumeration(enumeration, 0, keepCount);
        }

        public static T FirstAndOnly<T>(IEnumerable<T> enumeration)
        {
            IEnumerator<T> enumor = enumeration.GetEnumerator();
            Helper.CheckCondition(enumor.MoveNext(), "Can't get first item");
            T t = enumor.Current;
            Helper.CheckCondition(!enumor.MoveNext(), "More than one item available");
            return t;
        }



        public static IEnumerable<KeyValuePair<T, T>> Neighbors<T>(IEnumerable<T> collection)
        {
            T previous = default(T);
            bool first = true;
            foreach (T item in collection)
            {
                if (!first)
                {
                    yield return new KeyValuePair<T, T>(previous, item);
                }
                else
                {
                    first = false;
                }
                previous = item;
            }
        }

        public static bool KeysEqual<T, Tv1, Tv2>(IDictionary<T, Tv1> set1, IDictionary<T, Tv2> set2)
        {
            if (set1.Count != set2.Count)
            {
                return false;
            }

            foreach (T key in set1.Keys)
            {
                //Debug.Assert(set1[key]); // real assert - all values must be "true"
                if (!set2.ContainsKey(key))
                {
                    return false;
                }
                else
                {
                    // Debug.Assert(set2[key]); // real assert - all values must be "true"
                }
            }
            return true;
        }




        public static Set<Tkey> ExistenceMapToSet<Tkey>(Dictionary<Tkey, bool?> dictionary)
        {
            Set<Tkey> result = new Set<Tkey>();
            //Console.WriteLine("dictionary null ? " + (dictionary == null));
            foreach (Tkey key in dictionary.Keys)
            {
                bool? value = dictionary[key];
                if (value != null && (bool)value)
                {
                    result.AddNewOrOld(key);
                }
            }
            return result;
        }

        // randomizes the given mapping. returns a new mapping without changing that formal.
        public static Dictionary<Tkey, Tvalue> RandomizeMapping<Tkey, Tvalue>(IDictionary<Tkey, Tvalue> mapping, ref Random random)
        {
            IEnumerable<Tvalue> shuffledValues = mapping.Values.Shuffle(random);
            Dictionary<Tkey, Tvalue> result = new Dictionary<Tkey, Tvalue>(mapping.Count);
            // if always a Dictionary, could us mapping.Keys; however, we're not generally guaranteed, that the keys don't include some instances in
            // which there is no corresponding value (see, e.g. IVectorView).
            foreach (KeyValuePair<Tkey, Tvalue> keyAndValue in EnumerateTwo(mapping.AsEnumerable().Select(kvp => kvp.Key), shuffledValues))
            {
                result.Add(keyAndValue.Key, keyAndValue.Value);
            }
            return result;
        }

        /// <summary>
        /// Given two disjoint mappings, returns a new mapping that is the union of the two.
        /// </summary>
        public static Dictionary<Tkey, Tvalue> CombineMappings<Tkey, Tvalue>(IDictionary<Tkey, Tvalue> mapping1, IDictionary<Tkey, Tvalue> mapping2)
        {
            Dictionary<Tkey, Tvalue> result = new Dictionary<Tkey, Tvalue>(mapping1.Count + mapping2.Count);
            foreach (KeyValuePair<Tkey, Tvalue> keyAndValue in mapping1)
            {
                result.Add(keyAndValue.Key, keyAndValue.Value);
            }
            foreach (KeyValuePair<Tkey, Tvalue> keyAndValue in mapping2)
            {
                result.Add(keyAndValue.Key, keyAndValue.Value);
            }
            return result;
        }

        //This works, but it is not used anywhere, so delete it after a while
        //// randomizes the given mapping. returns a new mapping without changing that formal.
        //public static IEnumerable<KeyValuePair<Tkey, Tvalue>> RandomizeMapping<Tkey, Tvalue>(IEnumerable<KeyValuePair<Tkey, Tvalue>> mapping, Random random)
        //{
        //	IEnumerable<KeyValuePair<Tkey, Tvalue>> shuffledMapping = Shuffle<KeyValuePair<Tkey, Tvalue>>(mapping, ref random);
        //	//Dictionary<Tkey, Tvalue> result = new Dictionary<Tkey, Tvalue>();
        //	foreach (KeyValuePair<KeyValuePair<Tkey, Tvalue>, KeyValuePair<Tkey, Tvalue>> keyAndValueAndShuffledKeyAndValue in EnumerateTwo(mapping, shuffledMapping))
        //	{
        //		yield return new KeyValuePair<Tkey, Tvalue>(keyAndValueAndShuffledKeyAndValue.Key.Key, keyAndValueAndShuffledKeyAndValue.Value.Value);
        //	}
        //}


        // creates a random mapping between the elements in keys and those in values.
        // For now, we require that keys and values be the same size, though this could be generalized in the future.
        // Since we're making a dictionary, if there are duplicate values in keys, collisions will cause the first values to be
        // erased. It'd be best to make this a set, but there's no generic set interface in C# and it's difficult to use our own, since
        // this method is often used with the keySet of a map, which of course is not of type Set.
        public static Dictionary<Tkey, Tvalue> CreateRandomMapping<Tkey, Tvalue>(IEnumerable<Tkey> keys, IEnumerable<Tvalue> values, ref Random random)
        {
            Dictionary<Tkey, Tvalue> result = new Dictionary<Tkey, Tvalue>();

            IEnumerable<Tvalue> shuffledValues = values.Shuffle(random);
            IEnumerator<Tvalue> shuffledValsEnum = shuffledValues.GetEnumerator();

            foreach (Tkey key in keys)
            {
                // for now, require that values and keys are the same size. In general, we could make this wrap around
                Helper.CheckCondition(shuffledValsEnum.MoveNext(), "There are more keys than values in the random mapping");
                result.Add(key, shuffledValsEnum.Current);
            }

            // for now, require that values and keys are the same size. In general, we could make this wrap around
            // At this point, if we can move next, then there were more values than keys.
            Helper.CheckCondition(!shuffledValsEnum.MoveNext(), "There are more values than keys in the random mapping");  // for now, require that values and keys are the same size.

            return result;
        }



        public static List<T> RemoveDuplicatesFromSortedList<T>(List<T> list)
        {
            List<T> result = new List<T>(list.Count);
            if (list.Count == 0)
                return result;

            T last = list[0];
            result.Add(last);

            foreach (T item in list)
            {
                if (!item.Equals(last))
                    result.Add(item);
                last = item;
            }
            return result;
        }

        /// <summary>
        /// Returns the cube root of x. Is smart about the fact that cuberoot of a negative number is possible.
        /// </summary>
        public static double CubeRoot(double x)
        {
            bool isNeg = x < 0;
            if (isNeg)
                x = -x;

            double y = Math.Pow(x, 1.0 / 3.0);
            if (isNeg)
                y = -y;

            return y;
        }


#if !SILVERLIGHT

        /// <summary>
        /// For now, just cast to double. Will fail if c has a complex component.
        /// </summary>
        public static ComplexNumber CubeRoot(ComplexNumber c)
        {
            return CubeRoot((double)c);
        }

        public static ComplexNumber Sqrt(double d)
        {
            if (d >= 0)
                return Math.Sqrt(d);
            else
                return new ComplexNumber(0, Math.Sqrt(-d));
        }

        //!!!how about having this be a static on ComplexNumber
        /// <summary>
        /// For now, just cast to double. Will fail if c has a complex component.
        /// </summary>
        public static ComplexNumber Sqrt(ComplexNumber c)
        {
            return SpecialFunctions.Sqrt((double)c);
        }

#endif

        public static string ExpandParentheticalCode(string line, string varRoot)
        {
            StringBuilder newVariables = new StringBuilder();
            int numVariables = 0;
            int endParen;
            int matchingBeginParen;
            string subexpression;
            string variableName;

            while ((endParen = line.IndexOf(')')) >= 0)
            {
                matchingBeginParen = line.LastIndexOf('(', endParen);

                subexpression = line.Substring(matchingBeginParen, endParen - matchingBeginParen + 1);
                variableName = varRoot + (++numVariables);

                newVariables.AppendLine(string.Format("{0} = {1};", variableName, subexpression.Substring(1, subexpression.Length - 2)));   // take of the parens.
                line = line.Replace(subexpression, variableName);
            }

            newVariables.AppendLine(line.Replace(";", ";\n"));

            return newVariables.ToString();
        }

#if !SILVERLIGHT
        public static string NormalizeCode(string line, string varRoot)
        {
            // regular expression to fully parenthesize rational numbers
            Regex rationalNumberRegEx = new Regex(@"[\d\.\s]+\/[\d\.\s]+");
            MatchEvaluator rationalNumberMatchEvaluator = delegate(Match m)
            {
                return "(" + m.Value + ")";
            };

            // regex to convert ints to doubles
            Regex intToFloatRegEx = new Regex(@"\W\d+");
            MatchEvaluator intToFloatMatchEvaluator = delegate(Match m)
            {
                return m.Value + ".0";
            };

            string powerDefinitions;
            string lineWithNewPowerDefinitions = SimplifyPowerTerms(line, out powerDefinitions);

            string fullyParenthesizedLines = rationalNumberRegEx.Replace(lineWithNewPowerDefinitions, rationalNumberMatchEvaluator);
            string doubleizedLines = intToFloatRegEx.Replace(fullyParenthesizedLines, intToFloatMatchEvaluator);
            string transformedLines = SpecialFunctions.ExpandParentheticalCode(doubleizedLines, varRoot);

            return powerDefinitions + transformedLines;
        }

        private static string SimplifyPowerTerms(string line, out string powerDefinitions)
        {
            StringBuilder powerDefs = new StringBuilder();
            SortedDictionary<string, string> knownPowersToDefinition = new SortedDictionary<string, string>();

            Regex powerRegEx = new Regex(@"(\w+)\s*\^\s*(\d+)");
            MatchEvaluator powerReplacer = delegate(Match m)
            {
                string baseLabel = m.Groups[1].Value;
                int pow = int.Parse(m.Groups[2].Value);

                string replacement = baseLabel + pow;
                if (!knownPowersToDefinition.ContainsKey(replacement))
                {
                    knownPowersToDefinition.Add(replacement, string.Format("{0} * {1};", baseLabel, pow == 2 ? baseLabel : baseLabel + (pow - 1)));
                }
                return replacement;
            };

            string result = powerRegEx.Replace(line, powerReplacer);

            foreach (KeyValuePair<string, string> replacementAndDefinition in knownPowersToDefinition)
            {
                powerDefs.AppendLine(string.Format("{0} = {1}", replacementAndDefinition.Key, replacementAndDefinition.Value));
            }

            powerDefinitions = powerDefs.ToString();
            return result;
        }
#endif

        public static string MatlabToCSharp(string normalizedCode)
        {
            Regex powerReplacerRegEx = new Regex(@"([\w\d\s\.]+)\^([\w\d\s\.]+)");
            MatchEvaluator powerReplacer = delegate(Match m)
            {
                return string.Format("ComplexNumber.Pow({0}, {1})", m.Groups[1], m.Groups[2]);
            };

            string cSharpCode = powerReplacerRegEx.Replace(normalizedCode, powerReplacer);
            cSharpCode = "i = new ComplexNumber(0, 1);\n" + cSharpCode;

            return cSharpCode;
        }

        //!!!could the FileInfo() class be used??
        public static string BaseFileName(string filename)
        {
            int fileDelim = filename.LastIndexOf('\\');
            if (fileDelim >= 0)
                filename = filename.Substring(fileDelim + 1);

            int suffixIdx = filename.LastIndexOf('.');
            if (suffixIdx == filename.Length - 4) //!!!where does "4" come from??? does it assume 3 letter suffix?
                filename = filename.Substring(0, suffixIdx);

            return filename;
        }


        public static Dictionary<T, int> ValueToCount<T>(IEnumerable<T> valueCollection)
        {
            Dictionary<T, int> valueToCount = new Dictionary<T, int>();
            foreach (T t in valueCollection)
            {
                valueToCount[t] = 1 + valueToCount.GetValueOrDefault(t);
            }
            return valueToCount;
        }

        public static void ConvertBinarySeqToSparse(string inputFileName, string outputFileName)
        {
            using (TextWriter textWriter = File.CreateText(outputFileName))
            {
                textWriter.WriteLine(Helper.CreateTabString("var", "cid", "val"));
                foreach (Dictionary<string, string> row in SpecialFunctions.TabFileTable(inputFileName, "n1pos	aa	pid	val", false))
                {
                    if (row["val"] != "M")
                    {
                        Helper.CheckCondition(row["val"] == "T" || row["val"] == "F", "Expect val of 'T', 'F', or 'M'");
                        string val = (row["val"] == "T") ? "1" : "0";
                        string cid = row["pid"];
                        string var = string.Format("{0}@{1}", row["aa"], row["n1pos"]);
                        textWriter.WriteLine(Helper.CreateTabString(var, cid, val));
                    }
                }
            }
        }


        //public static List<T> SubList<T>(IList<T> list, int startIndex, int length)
        //{
        //    List<T> result = new List<T>(length);
        //    for (int i = 0; i < length; ++i)
        //    {
        //        result.Add(list[startIndex + i]);
        //    }
        //    return result;
        //}

        public static IEnumerable<string> SubstringEnumeration(string s, int length)
        {
            for (int startIndex = 0; startIndex <= s.Length - length; ++startIndex)
            {
                yield return s.Substring(startIndex, length);
            }
        }


        public static T[] SubArray<T>(T[] args, int startingIndex)
        {
            return SubArray(args, startingIndex, args.Length - startingIndex);
        }

        public static T[] SubArray<T>(T[] args, int startingIndex, int length)
        {
            T[] result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = args[i + startingIndex];
            }
            return result;
        }

        public static void CopyDirectory(string oldDirectoryName, string newDirectoryName, bool recursive)
        {
            CopyDirectory(oldDirectoryName, newDirectoryName, recursive, false);
        }
        public static void CopyDirectory(string oldDirectoryName, string newDirectoryName, bool recursive, bool laterDateOnly)
        {
            Directory.CreateDirectory(newDirectoryName);

            DirectoryInfo oldDirectory = new DirectoryInfo(oldDirectoryName);
            if (!oldDirectory.Exists)
                return;

            foreach (FileInfo fileInfo in oldDirectory.EnumerateFiles())
            {
                if (!fileInfo.Exists) continue; // file may have been moved out from under us.
                string targetFileName = newDirectoryName + @"\" + fileInfo.Name;
                if (laterDateOnly && File.Exists(targetFileName))
                {
                    DateTime sourceTime = fileInfo.LastWriteTime;
                    DateTime targetTime = File.GetLastAccessTime(targetFileName);
                    if (targetTime >= sourceTime)
                    {
                        continue;
                    }
                }

                try
                {
                    fileInfo.CopyTo(targetFileName, true);
#if !SILVERLIGHT
                    if (((File.GetAttributes(targetFileName) & FileAttributes.ReadOnly) != FileAttributes.ReadOnly))
                    {
                       File.SetLastAccessTime(targetFileName, fileInfo.LastWriteTime);
                    }
#endif
                }
                catch (FileNotFoundException)
                {
                    // do nothing. The file must have moved out from under us.
                }
            }

            if (recursive)
            {
                foreach (DirectoryInfo subdirectory in oldDirectory.EnumerateDirectories())
                {
                    CopyDirectory(subdirectory.FullName, newDirectoryName + @"\" + subdirectory.Name, recursive, laterDateOnly);
                }
            }
        }

        public static double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        public static double GaussianProbability(double x, double mean, double variance)
        {
            double k = 1.0 / Math.Sqrt(variance * 2 * Math.PI);
            double p = k * Math.Exp(-Math.Pow(x - mean, 2) / (2 * variance));
            return p;
        }

        public static int PickRandomBin(IList<double> probabilityDistribution, ref Random rand)
        {
            int classIndex = 0;
            double randomValue = rand.NextDouble();
            double sum = 0;
            while ((sum += probabilityDistribution[classIndex]) < randomValue)
            {
                classIndex++;
            }
            return classIndex;
        }

        public static double[] ParseDoubleArrayArgs(string commaDelimitedDoubles, char delim)
        {
            string[] args = commaDelimitedDoubles.Split(delim);
            double[] result = new double[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                result[i] = double.Parse(args[i]);
            }
            return result;
        }

        public static void Die(string p)
        {
            throw new Exception(string.Format("Forced Exit:\n{0}", p));
        }

        public static void ConvertDictionaryToDerivedClasses<T1, T2, T3, T4>(Dictionary<T1, T2> originalDict, out Dictionary<T3, T4> resultDict)
            where T3 : T1
            where T4 : T2
        {
            resultDict = new Dictionary<T3, T4>(originalDict.Count);
            foreach (KeyValuePair<T1, T2> keyValuePair in originalDict)
            {
                resultDict.Add((T3)keyValuePair.Key, (T4)keyValuePair.Value);
            }
        }

        public static void ConvertDictionaryToBaseClasses<T1, T2, T3, T4>(Dictionary<T1, T2> originalDict, out Dictionary<T3, T4> resultDict)
            where T1 : T3
            where T2 : T4
        {
            resultDict = new Dictionary<T3, T4>(originalDict.Count);
            foreach (KeyValuePair<T1, T2> keyValuePair in originalDict)
            {
                resultDict.Add((T3)keyValuePair.Key, (T4)keyValuePair.Value);
            }
        }

        public static List<TItem> CreateSortedList<TItem, TSortKey>(
            IEnumerable<TItem> inputCollection,
            Converter<TItem, TSortKey> accessor,
            Comparison<TSortKey> isBetter)
        {
            Helper.CheckCondition(false, "need to test this");
            List<TItem> list = new List<TItem>(inputCollection);
            list.Sort(
                    delegate(TItem item1, TItem item2)
                    {
                        TSortKey sortKey1 = accessor(item1);
                        TSortKey sortKey2 = accessor(item2);
                        return isBetter(sortKey1, sortKey2);
                    }
            );
            return list;
        }



        public static bool IgnoreCaseContains(IList<string> stringList, string item)
        {
            return IgnoreCaseIndexOf(stringList, item) >= 0;
        }

        internal static int IgnoreCaseIndexOf(IList<string> stringList, string item)
        {
            for (int i = 0; i < stringList.Count; ++i)
            {
                string s = stringList[i];
                if (s.Equals(item, StringComparison.CurrentCultureIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }


        public static List<string> Split(string line, params char[] separator)
        {
            List<string> stringList = new List<string>(line.Split(separator));
            return stringList;

        }



        //public static bool TryParse<T>(string s, out T t)
        //{
        //    return Parser.TryParse<T>(s, out t);
        //}
        public static bool TryParse<T>(string s, out T t)
        {
            return MBT.Escience.Parse.Parser.TryParse<T>(s, out t);
        }

        //internal static bool TryParseOld<T>(string s, out T t)
        //{
        //    if (typeof(T).Equals(typeof(int)))
        //    {
        //        int i;
        //        bool b = int.TryParse(s, out i);
        //        t = (T)(object)i;
        //        return b;
        //    }

        //    if (typeof(T).Equals(typeof(int?)))
        //    {
        //        if (s.ToLower() == "null")
        //        {
        //            t = (T)(object)null;
        //            return true;
        //        }
        //        int i;
        //        bool b = int.TryParse(s, out i);
        //        t = (T)(object)i;
        //        return b;
        //    }


        //    if (typeof(T).Equals(typeof(string)))
        //    {
        //        t = (T)(object)s;
        //        return true;
        //    }

        //    if (typeof(T).Equals(typeof(double)))
        //    {
        //        double r;
        //        bool b = double.TryParse(s, out r);
        //        t = (T)(object)r;
        //        return b;
        //    }

        //    if (typeof(T).Equals(typeof(double?)))
        //    {
        //        if (s.ToLower() == "null")
        //        {
        //            t = (T)(object)null;
        //            return true;
        //        }

        //        double r;
        //        bool b = double.TryParse(s, out r);
        //        t = (T)(object)r;
        //        return b;
        //    }

        //    if (typeof(T).Equals(typeof(bool)))
        //    {
        //        bool r;
        //        bool b = bool.TryParse(s, out r);
        //        t = (T)(object)r;
        //        return b;
        //    }

        //    if (typeof(T).Equals(typeof(bool?)))
        //    {
        //        if (s.ToLower() == "null")
        //        {
        //            t = (T)(object)null;
        //            return true;
        //        }

        //        bool r;
        //        bool b = bool.TryParse(s, out r);
        //        t = (T)(object)r;
        //        return b;
        //    }

        //    if (typeof(T).IsEnum)
        //    {
        //        int i;
        //        if (int.TryParse(s, out i))
        //        {
        //            //return Enum.GetName(typeof(T), i);
        //            t = (T)(object)i;
        //            return true;
        //        }

        //        try
        //        {
        //            t = (T)Enum.Parse(typeof(T), s);
        //            return true;
        //        }
        //        catch (ArgumentException)
        //        {
        //        }
        //        t = default(T);
        //        return false;

        //    }

        //    //!!!could define a ITryParse and IParse interfact
        //    if (typeof(T).Equals(typeof(RangeCollection)))
        //    {
        //        try
        //        {
        //            t = (T)(object)RangeCollection.Parse(s);
        //            return true;
        //        }
        //        catch
        //        {
        //        }
        //        t = default(T);
        //        return false;
        //    }
        //    //!!!could use more reflection to make this work for a list of any type
        //    if (typeof(T).Equals(typeof(List<double>)))
        //    {
        //        try
        //        {
        //            List<double> list = new List<double>();
        //            foreach (string itemAsString in s.Split(','))
        //            {
        //                double item = double.Parse(itemAsString);
        //                list.Add(item);
        //            }
        //            t = (T)(object)list;
        //            return true;
        //        }
        //        catch
        //        {
        //        }
        //        t = default(T);
        //        return false;
        //    }
        //    if (typeof(T).Equals(typeof(List<int>)))
        //    {
        //        try
        //        {
        //            List<int> list = new List<int>();
        //            foreach (string itemAsString in s.Split(','))
        //            {
        //                int item = int.Parse(itemAsString);
        //                list.Add(item);
        //            }
        //            t = (T)(object)list;
        //            return true;
        //        }
        //        catch
        //        {
        //        }
        //        t = default(T);
        //        return false;
        //    }
        //    if (typeof(T).Equals(typeof(List<string>)))
        //    {
        //        try
        //        {
        //            List<string> list = new List<string>();
        //            foreach (string itemAsString in s.Split(','))
        //            {
        //                //string item = string.Parse(itemAsString);
        //                list.Add(itemAsString);
        //            }
        //            t = (T)(object)list;
        //            return true;
        //        }
        //        catch
        //        {
        //        }
        //        t = default(T);
        //        return false;
        //    }

        //    if (typeof(T).Equals(typeof(TimeSpan)))
        //    {
        //        TimeSpan r;
        //        bool b = TimeSpan.TryParse(s, out r);
        //        t = (T)(object)r;
        //        return b;
        //    }

        //    //!!!could use more reflection to make this work for a Tuple of any type
        //    if (typeof(T).Equals(typeof(Tuple<double, double>)))
        //    {
        //        try
        //        {
        //            string[] itemCollection = s.Split(',');
        //            Helper.CheckCondition(itemCollection.Length == 2, "Expected two items for pair");
        //            Tuple<double, double> pair = Tuple.Create(double.Parse(itemCollection[0]), double.Parse(itemCollection[1]));
        //            t = (T)(object)pair;
        //            return true;
        //        }
        //        catch
        //        {
        //        }
        //        t = default(T);
        //        return false;
        //    }
        //    if (typeof(T).Equals(typeof(Tuple<int, int>)))
        //    {
        //        try
        //        {
        //            string[] itemCollection = s.Split(',');
        //            Helper.CheckCondition(itemCollection.Length == 2, "Expected two items for pair");
        //            Tuple<int, int> pair = Tuple.Create(int.Parse(itemCollection[0]), int.Parse(itemCollection[1]));
        //            t = (T)(object)pair;
        //            return true;
        //        }
        //        catch
        //        {
        //        }
        //        t = default(T);
        //        return false;
        //    }



        //    Helper.CheckCondition(false, "Don't know how to parse " + typeof(T));
        //    t = default(T);
        //    return false;

        //}

        public static Dictionary<TKey, TValue> PairEnumerationToDictionary<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> enumeration)
        {
            Dictionary<TKey, TValue> result = enumeration as Dictionary<TKey, TValue>;
            if (null != result)
            {
                // you gave me a Dictionary stupid.
                return result;
            }

            result = new Dictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> pair in enumeration)
            {
                result.Add(pair.Key, pair.Value);
            }
            return result;
        }



        public static IEnumerable<List<T>> Transpose<T>(IEnumerable<List<T>> rowList)
        {
            List<T> any;
            bool isAny = TryFirst(rowList, out any);
            Helper.CheckCondition(isAny, "Can't transpose empty input");
            for (int iColumn = 0; iColumn < any.Count; ++iColumn)
            {
                List<T> columnList = new List<T>();
                foreach (List<T> row in rowList)
                {
                    Helper.CheckCondition(row.Count == any.Count, "All rows must have the same length");
                    columnList.Add(row[iColumn]);
                }
                yield return columnList;
            }
        }

        /// <summary>
        /// Must be rectangular (not ragged).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T[][] TransposeArray<T>(T[][] input)
        {
            if (input.Length == 0)
                return input;

            int nRow = input.Length;
            int nCol = input[0].Length;

            T[][] result = new T[nCol][];
            for (int i = 0; i < nCol; i++)
            {
                result[i] = new T[nRow];
                for (int j = 0; j < nRow; j++)
                {
                    result[i][j] = input[j][i];
                }
            }
            return result;
        }

        public static void NormalizeRows(double[][] input)
        {
            for (int ii = 0; ii < input.Length; ii++)
            {
                double sum = input[ii].Sum();
                for (int jj = 0; jj < input[ii].Length; jj++)
                    input[ii][jj] = input[ii][jj] / sum;
            }
        }

        private static bool TryFirst<T>(IEnumerable<T> enumeration, out T any)
        {
            IEnumerator<T> enumerator = enumeration.GetEnumerator();
            if (enumerator.MoveNext())
            {
                any = enumerator.Current;
                return true;
            }
            else
            {
                any = default(T);
                return false;
            }
        }

        public static T RandomFromMultinomial<T>(Dictionary<T, int> itemToCount, ref Random random)
        {
            T t = default(T);
            int previousItemCount = 0;
            foreach (KeyValuePair<T, int> itemAndCount in itemToCount)
            {
                int count = itemAndCount.Value;
                Helper.CheckCondition(count >= 0, "Multinomial distribution must not have a negative count");
                if (random.Next(previousItemCount + count) >= previousItemCount)
                {
                    t = itemAndCount.Key;
                }
                previousItemCount += count;
            }
            Helper.CheckCondition(previousItemCount > 0, "Multinomial distribution must have a positive count");
            return t;
        }


        public static List<T> CreateAllocatedList<T>(int count)
        {
            List<T> list = new List<T>(count);
            for (int i = 0; i < count; ++i)
            {
                list.Add(default(T));
            }
            return list;
        }


#if !SILVERLIGHT

        public static Tuple<ComplexNumber, ComplexNumber> QuadraticRoots(int a, double b, double c)
        {
            ComplexNumber rootTerm = SpecialFunctions.Sqrt(b * b - 4 * a * c);

            ComplexNumber root1 = (-b + rootTerm) / (2 * a);
            ComplexNumber root2 = (-b - rootTerm) / (2 * a);

            return Tuple.Create(root1, root2);
        }
#endif

        public static bool DictionaryEqual<TKey, TValue>(IDictionary<TKey, TValue> a, IDictionary<TKey, TValue> b)
        {
            foreach (KeyValuePair<TKey, TValue> aKeyAndValue in a)
            {
                TValue bValue;
                if (!b.TryGetValue(aKeyAndValue.Key, out bValue))
                {
                    return false;
                }
                if (!aKeyAndValue.Value.Equals(bValue))
                {
                    return false;
                }
            }

            foreach (TKey bKey in b.Keys)
            {
                if (!a.ContainsKey(bKey))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// returns nonnegative long
        /// </summary>
        public static long RandomLong(ref Random random)
        {
            long result;
            do
            {
                var buffer = new byte[sizeof(long)];
                random.NextBytes(buffer);
                result = BitConverter.ToInt64(buffer, 0);
            } while (result == long.MinValue); //In the history of the program, this will likely never loop, but it is needed so that '0' doesn't have twice the probability of all the other longs.

            result &= long.MaxValue; //Kill any minus sign
            return result;
        }


        public static long RandomLong(ref Random random, long n)
        {
            if (n <= int.MaxValue)
            {
                return random.Next((int)n);
            }


            // random.Next returns a number in the range 0 ... int.MaxValue-1 (inclusive)
            while (true)
            {
                int partCount = (int)Ceiling(n, int.MaxValue);
                int part = random.Next(partCount);
                long r = part * int.MaxValue + random.Next(int.MaxValue);
                if (r < n)
                {
                    return r;
                }
            }
        }

        private static long Ceiling(long n, int p)
        {
            long partCount = n / p;
            if (p * partCount != n)
            {
                ++partCount;
            }
            return partCount;
        }

        public static Dictionary<TGroup, Dictionary<TKey, TValue>> GroupBy<TGroup, TKey, TValue>(Dictionary<TKey, TValue> inputDictionary, Converter<KeyValuePair<TKey, TValue>, TGroup> groupExtractor)
        {
            Dictionary<TGroup, Dictionary<TKey, TValue>> outputDictionary = new Dictionary<TGroup, Dictionary<TKey, TValue>>();
            foreach (KeyValuePair<TKey, TValue> keyValuePair in inputDictionary)
            {
                TGroup group = groupExtractor(keyValuePair);
                Dictionary<TKey, TValue> littleDictonary = outputDictionary.GetValueOrDefault(group);
                littleDictonary.Add(keyValuePair.Key, keyValuePair.Value);
            }
            return outputDictionary;
        }

        public static List<T> CreateSingletonList<T>(T item)
        {
            List<T> result = new List<T>();
            result.Add(item);
            return result;
        }


        public static double AreaUnderFdrFnrCurveAssumingNoTies(IList<bool> rankedListOfTrueClassifications)
        {
            int countTrue = 0;
            int countFalse = 0;
            double sum = 0;

            for (int i = 0; i < rankedListOfTrueClassifications.Count; i++)
            {
                if (rankedListOfTrueClassifications[i])
                {
                    countTrue++;
                    if (countFalse > 0) // if 0, the term should be 0. This avoids div 0 errors.
                    {
                        sum += (double)countFalse / (i + 1) + (double)countFalse / i;
                    }
                }
                else
                {
                    countFalse++;
                }
            }
            double area = sum / (2.0 * countTrue);
            if (double.IsNaN(area))
            {
                Console.WriteLine("Found nan");
            }
            return area;
        }

        /// <summary>
        /// Finds the area under the ROC curve. If all the False's have higher scores than all the Trues, the AUC will be 0.
        /// If all the True's have higher scores than all the Falses, the AUC will be 1.
        /// </summary>
        /// <param name="scoreAndTruthList">Does not need to be sorted.</param>
        /// <param name="parallelOptions"></param>
        /// <returns></returns>
        public static double RocAreaUnderCurve(IEnumerable<KeyValuePair<double, bool>> scoreAndTruthList, ParallelOptions parallelOptions)
        {
            //Gives the scores in order from largest to smallest. For every score, gives the counts of trues and falses with that score
            var sortedScoreToCountsQuery =
                from scoreAndTruth in scoreAndTruthList
                let score = scoreAndTruth.Key
                let truth = scoreAndTruth.Value
                group truth by score into scoreAndTruthGroup
                orderby scoreAndTruthGroup.Key descending
                let twoByOne = TwoByOne.GetInstance(scoreAndTruthGroup, x => x)
                select new { score = scoreAndTruthGroup.Key, twoByOne };

            int countTrue = 0;
            int countFalse = 0;
            double sumOfPositiveRanks = 0;
            double i = -1;
            foreach (var scoreAndCounts in sortedScoreToCountsQuery)
            {
                double score = scoreAndCounts.score;
                TwoByOne twoByOne = scoreAndCounts.twoByOne;


                double iLow = i + 1;
                double iHigh = i + twoByOne.Total;

                sumOfPositiveRanks += ((iLow + iHigh) / 2 + 1) * twoByOne.TrueCount;

                countTrue += twoByOne.TrueCount;
                countFalse += twoByOne.FalseCount;
                i = iHigh;
            }

            double F = sumOfPositiveRanks - (countTrue * (countTrue + 1)) / 2.0;

            double areaUnderCurve = 1 - F / (countTrue * countFalse);
            return areaUnderCurve;

        }



        /// <summary>
        /// Compute the area under the ROC curve. **Assumes there are no ties.
        /// </summary>
        /// <param name="rankedListOfTrueClassifications">Total ordered list of booleans. Values should be ordered in increasing order
        /// of significance. True indicates the value at that rank is really true (true positive), false indicates it's really false
        /// (false positive).</param>
        /// <returns></returns>
        public static double RocAreaUnderCurveAssumingNoTies(IEnumerable<bool> rankedListOfTrueClassifications)
        {
            int countTrue = 0;
            int countFalse = 0;
            int sumOfPositiveRanks = 0;
            int i = -1;
            foreach (bool trueClassification in rankedListOfTrueClassifications)
            {
                ++i;
                if (trueClassification)
                {
                    countTrue++;
                    sumOfPositiveRanks += i + 1;
                }
                else
                {
                    countFalse++;
                }
            }

            //countTrue = rankedListOfTrueClassifications.Count();
            //countFalse = countTrue;

            double F = sumOfPositiveRanks - (countTrue * (countTrue + 1)) / 2.0;

            //double areaUnderCurve = 1 - (double)sumOfPositiveRanks / (countTrue * countFalse);
            double areaUnderCurve = 1 - F / (countTrue * countFalse);
            return areaUnderCurve;
        }

        /// <summary>
        /// Returns the unnormalized AUC. That is, count the area under the TP vs Rank curve, where
        /// each TP adds height of unit 1.
        /// </summary>
        /// <param name="rankedListOfTrueClassifications"></param>
        /// <returns></returns>
        public static double RocUnNormalizedAreaUnderCurveAssumingNoTies(IList<bool> rankedListOfTrueClassifications)
        {
            int auc = 0;
            int currentHeight = 0;
            for (int i = 0; i < rankedListOfTrueClassifications.Count; i++)
            {
                if (rankedListOfTrueClassifications[i])
                {
                    currentHeight++;
                }
                else
                {
                    auc += currentHeight;
                }
            }
            return auc;
        }

        /// <summary>
        /// Runs a permutation test and the predictions of two classifiers.
        /// </summary>
        /// <param name="classifier1">True classifications of events, ordered by classifier1.</param>
        /// <param name="classifier2">True classifications of events, ordered by classifier2.</param>
        /// <param name="scoringFunction">Function that computes a score for each vector.</param>
        /// <param name="numPermutations"></param>
        /// <param name="simulatedScores">The distribution of classification differences under the null hypothesis will be dumped here.</param>
        /// <returns>Probability that the discriminatory power of the two classifiers is different</returns>
        public static double PermutationTestOfBooleanVectors(IList<bool> classifier1, IList<bool> classifier2, Converter<IList<bool>, double> scoringFunction,
            int numPermutations, bool twoTailed, out double[] simulatedScores)
        {
            Random rand = new MachineInvariantRandom("PermutationTestOfBooleanVectors");
            return PermutationTestOfBooleanVectors(classifier1, classifier2, scoringFunction, numPermutations, twoTailed, ref rand, out simulatedScores);


        }

        /// <summary>
        /// Runs a permutation test on the predictions of two classifiers.
        /// </summary>
        /// <param name="classifier1">True classifications of events, ordered by classifier1.</param>
        /// <param name="classifier2">True classifications of events, ordered by classifier2.</param>
        /// <param name="scoringFunction">Function that computes a score for each vector.</param>
        /// <param name="numPermutations">The distribution of classification differences under the null hypothesis will be dumped here.</param>
        /// <param name="simulatedScores"></param>
        /// <returns>Probability that the discriminatory power of the two classifiers is different</returns>
        public static double PermutationTestOfBooleanVectors(IList<bool> classifier1, IList<bool> classifier2, Converter<IList<bool>, double> scoringFunction,
            int numPermutations, bool twoTailed, ref Random rand, out double[] simulatedScores)
        {
            Helper.CheckCondition(classifier1.Count == classifier2.Count);

            int n = classifier1.Count;
            double observedScore = scoringFunction(classifier1) - scoringFunction(classifier2);
            if (twoTailed)
            {
                observedScore = Math.Abs(observedScore);
            }
            //double observedDifference = Math.Abs(RocAreaUnderCurve(classifier1) - RocAreaUnderCurve(classifier2));
            //double observedDifference = RocAreaUnderCurve(classifier1) - RocAreaUnderCurve(classifier2);

            simulatedScores = new double[numPermutations];
            bool[] dummyClassifier1 = new bool[n];
            bool[] dummyClassifier2 = new bool[n];

            for (int iteration = 0; iteration < numPermutations; iteration++)
            {
                for (int i = 0; i < n; i++)
                {
                    if (rand.NextDouble() < 0.5)
                    {
                        dummyClassifier1[i] = classifier1[i];
                        dummyClassifier2[i] = classifier2[i];
                    }
                    else
                    {
                        dummyClassifier1[i] = classifier2[i];
                        dummyClassifier2[i] = classifier1[i];
                    }
                }
                double fakeDiff = scoringFunction(dummyClassifier1) - scoringFunction(dummyClassifier2);
                if (twoTailed)
                {
                    fakeDiff = Math.Abs(fakeDiff);
                }

                //double fakeDiff = Math.Abs(RocAreaUnderCurve(dummyClassifier1) - RocAreaUnderCurve(dummyClassifier2));
                //double fakeDiff = RocAreaUnderCurve(dummyClassifier1) - RocAreaUnderCurve(dummyClassifier2);
                simulatedScores[iteration] = fakeDiff;
            }

            double p = ComputePValueFromSimulationData(observedScore, simulatedScores, true);
            return p;
        }

        public static double ComputePValueFromSimulationData(double observedScore, double[] simulatedScores, bool higherIsBetter)
        {
            int numPermutations = simulatedScores.Length;
            // sort in descending order

            if (higherIsBetter)
            {
                Array.Sort(simulatedScores, DoubleLessThan);
            }
            else
            {
                Array.Sort(simulatedScores, DoubleGreaterThan);
            }

            int rankOfObsDiff = 0;
            while (rankOfObsDiff < numPermutations &&
                (higherIsBetter ?
                    observedScore <= simulatedScores[rankOfObsDiff] :
                    observedScore >= simulatedScores[rankOfObsDiff]))
            {
                rankOfObsDiff++;
            }

            double p = (double)rankOfObsDiff / numPermutations;
            return p;
        }

        /// <summary>
        /// If z > 0, then the pValue will be > 0.5
        /// If z is 7, p be will about .999999
        /// if z is -7, p will be about .00000001
        /// </summary>
        /// <remarks>Note that prior to Aug 28, 2009, this was backwards. The commented line with 1-xxx reverses the claim of summary. This has now been fixed.
        /// I do not know if this breaks existing code. (JC)</remarks>
        public static double ZScoreToOneTailedPValue(double z, double eps)
        {
            z = Math.Max(-6, Math.Min(6, z)); //Keep in the range -6 to 6
            double p = 0.5 * (1 + Erf(z / Math.Sqrt(2), eps));
            return p;
        }

        public static double ZScoreToOneTailedPValueLessThanHalf(double z, double eps)
        {
            double p = ZScoreToOneTailedPValue(-Math.Abs(z), eps);
            Helper.CheckCondition(p <= .5, "P-value is > 0.5, which means we have a sign wrong somewhere.");
            return p;
        }
        public static double ZScoreToTwoTailedPValue(double z, double eps)
        {
            return 2.0 * ZScoreToOneTailedPValueLessThanHalf(z, eps);
        }

        public static double Erf(double x, double eps)
        {
            Helper.CheckCondition(!double.IsNaN(x) && !double.IsInfinity(x));
            int erfSign = x < 0 ? -1 : 1;
            x = Math.Abs(x);

            double sum = 0;
            double lastSum = double.MinValue;
            double lnX = Math.Log(x);
            double lnFactorial = 0;

            for (int i = 0; i < 10000; i++)
            {
                int sign = i % 2 == 0 ? 1 : -1;
                lnFactorial += i == 0 ? 0 : Math.Log(i);
                double logNumerator = Math.Log(x) * (2 * i + 1);
                double logDenominator = Math.Log(2 * i + 1) + lnFactorial;

                double term = sign * Math.Exp(logNumerator - logDenominator);
                sum += term;
                if (Math.Abs(sum - lastSum) < eps)
                {
                    break;
                }
                lastSum = sum;
            }

            double erf = erfSign * 2.0 * sum / Math.Sqrt(Math.PI);
            return erf;
        }

        public static void ConvertToLog(double[] dArr)
        {
            for (int i = 0; i < dArr.Length; i++)
            {
                //Helper.CheckCondition<ArithmeticException>(!double.IsNegativeInfinity(Math.Log(dArr[i])), "value {0} leads to negative infinity after log transformation", dArr[i]);
                dArr[i] = Math.Log(dArr[i]);
            }
        }
        public static void ConvertToLog(double[][] twoDArr)
        {
            for (int i = 0; i < twoDArr.Length; i++)
            {
                ConvertToLog(twoDArr[i]);
            }
        }

        public static void ConvertFromLog(double[] dArr)
        {
            for (int i = 0; i < dArr.Length; i++)
            {
                dArr[i] = Math.Exp(dArr[i]);
            }
        }
        public static void ConvertFromLog(double[][] twoDArr)
        {
            for (int i = 0; i < twoDArr.Length; i++)
            {
                ConvertFromLog(twoDArr[i]);
            }
        }

        public static void ConvertFromLog(double[,] twoDArr)
        {
            for (int i = 0; i < twoDArr.GetLength(0); i++)
            {
                for (int j = 0; j < twoDArr.GetLength(1); j++)
                    twoDArr[i, j] = Math.Exp(twoDArr[i, j]);
            }
        }

        public static IEnumerable<TItem> GroupByEnumeration<TItem, TGroup>(IEnumerable<TItem> inputList, Converter<TItem, TGroup> accessor, Random random)
        {
            Dictionary<TGroup, Set<TItem>> dictionary = new Dictionary<TGroup, Set<TItem>>();
            foreach (TItem item in inputList)
            {
                TGroup group = accessor(item);
                Set<TItem> set = dictionary.GetValueOrDefault(group);
                set.AddNew(item);
            }

            foreach (TGroup group in dictionary.Keys.Shuffle(random))
            {
                foreach (TItem item in dictionary[group])
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<T> EnumerateDefault<T>(int count) where T : new()
        {
            for (int i = 0; i < count; ++i)
            {
                yield return new T();
            }
        }

        public static IEnumerable<TItem> GroupByEnumerationTwoLevel<TItem, TGroup1, TGroup2>(IEnumerable<TItem> inputList, Converter<TItem, TGroup1> accessor1, Converter<TItem, TGroup2> accessor2, Random random)
        {
            Dictionary<TGroup1, Set<TItem>> dictionary1 = new Dictionary<TGroup1, Set<TItem>>();
            foreach (TItem item in inputList)
            {
                TGroup1 group1 = accessor1(item);
                Set<TItem> set = dictionary1.GetValueOrDefault(group1);
                set.AddNew(item);
            }

            foreach (TGroup1 group1 in dictionary1.Keys.Shuffle(random))
            {
                foreach (TItem item in GroupByEnumeration(dictionary1[group1], accessor2, random))
                {
                    yield return item;
                }
            }
        }

        public static void XCopyED(string localDirectoryName, string externalRemoteDirectoryName)
        {

        }




        public static IEnumerable<KeyValuePair<T, T>> EveryPair<T>(IEnumerable<T> pairedFeatures)
        {
            int i = 0;
            foreach (T t1 in pairedFeatures.Skip(1)) // 2nd item, 3rd, 4th, ... last
            {
                ++i;
                int j = -1;
                foreach (T t2 in pairedFeatures) // 1st item, 2nd item, ... one Less than item above
                {
                    ++j;
                    yield return new KeyValuePair<T, T>(t2, t1);
                    // 1,2,	1,3, 2,3,	1,4, 2,4, 3,4, ....
                }
            }

        }

#if !SILVERLIGHT
        public static FileStream GetExclusiveReadWriteFileStream(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            return GetExclusiveReadWriteFileStream(filename);
        }

        static int sleepTimer = 200;
        public static FileStream GetExclusiveReadWriteFileStream(FileInfo fileInfo)
        {
            Random rand = new MachineInvariantRandom(Environment.CommandLine);//for PhyloD, need something that is uniq to each process that will be competing for this file
            FileStream fileStream;
            //int sleepTimer = 1000;
            for (; ; )
            {
                try
                {
                    fileStream = fileInfo.Open(FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    fileStream.Lock(0, int.MaxValue);
                    //sleepTimer = Math.Max(1000, sleepTimer - 1000);
                    return fileStream;
                }
                catch (IOException)
                {
                    Thread.Sleep(rand.Next(sleepTimer));
                    //sleepTimer *= 2;
                }
            }
        }
#endif

        public static IEnumerable<T2> Cast<T1, T2>(IEnumerable<T1> list) where T2 : T1
        {
            foreach (T1 t1 in list)
            {
                yield return (T2)t1;
            }
        }

        public static IEnumerable<T2> Cast<T1, T2>(IEnumerable<T1> list, Converter<T1, T2> castMethod)
        {
            foreach (T1 t1 in list)
            {
                yield return castMethod(t1);
            }
        }

        public static IEnumerable<T> RemoveDuplicates<T>(IEnumerable<T> enumeration)
        {
            Set<T> seenIt = Set<T>.GetInstance();
            foreach (T t in enumeration)
            {
                if (!seenIt.Contains(t))
                {
                    seenIt.AddNew(t);
                    yield return t;
                }
            }
        }

        public static void WriteEachLine<T>(IEnumerable<T> list, string fileName)
        {
            using (TextWriter textWriter = File.CreateText(fileName))
            {
                foreach (T t in list)
                {
                    textWriter.WriteLine(t.ToString());
                }
            }
        }

        [Obsolete("Use File.WriteAllText(path, obj) !!Note the different order of parameters!!!")]
        public static void WriteToFile(object obj, string fileName)
        {
            using (TextWriter textWriter = File.CreateText(fileName))
            {
                textWriter.WriteLine(obj.ToString());
            }
        }

        /// <summary>
        /// Gives back work items that are contiguous. To do this, one needs to know ahead of time the total number of items
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="pieceIndexRangeCollection"></param>
        /// <param name="pieceCount"></param>
        /// <param name="totalNumItems"></param>
        /// <param name="skipList"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<T, long>> DivideWorkIntoContiguousItems<T>(IEnumerable<T> enumerable,
                                           RangeCollection pieceIndexRangeCollection, int pieceCount, long totalNumItems, RangeCollection skipList)
        {
            ///!!! If we had 12100 total num items and 1000 piece count, it will use a batch count of 13 (that is ceil(12.1)), it would be nice if
            ///it sometimes used 12 and somes 13. The way it is now, toward the end some of the cluster tasks will have no work.
            long batchCount = (long)Math.Ceiling((double)totalNumItems / (double)pieceCount);
            return DivideWork<T>(enumerable, pieceIndexRangeCollection, pieceCount, batchCount, skipList);
        }


        /// <summary>
        /// This method will spread out the work items--like dealing cards around the table, you only get every other K'th card,
        /// where K is the number of people, when batchCount=1. (Or else you get sets of batchCount-contiguous cards).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="pieceIndexRangeCollection"></param>
        /// <param name="pieceCount"></param>
        /// <param name="batchCount"></param>
        /// <param name="skipList"></param>
        /// <returns>The item and it's original index in the enumerable.</returns>
        public static IEnumerable<KeyValuePair<T, long>> DivideWork<T>(IEnumerable<T> enumerable, RangeCollection pieceIndexRangeCollection,
                                                                      long pieceCount, long batchCount, RangeCollection skipList)
        {
            Helper.CheckCondition(batchCount > 0, "Batch count must be greater than 0");
            long pieceIndex = 0;
            long batchIndex = 0;
            bool inRange = pieceIndexRangeCollection.Contains(pieceIndex);

            foreach (var tAndRowIndex in UseSkipList(enumerable, skipList))
            {
                if (inRange)
                {
                    yield return tAndRowIndex;
                }

                ++batchIndex;
                if (batchIndex == batchCount)
                {
                    batchIndex = 0;
                    pieceIndex = (pieceIndex + 1) % pieceCount;
                    inRange = pieceIndexRangeCollection.Contains(pieceIndex);
                }
            }
        }

        private static IEnumerable<KeyValuePair<T, long>> UseSkipList<T>(IEnumerable<T> enumerable, RangeCollection skipList)
        {
            long rowIndex = -1;
            foreach (T t in enumerable)
            {
                ++rowIndex;
                if (skipList.Contains(rowIndex))
                {
                    continue;
                }
                yield return new KeyValuePair<T, long>(t, rowIndex);
            }
        }

        /// <summary>
        /// Defines 0/0 := 0.
        /// </summary>
        public static double SafeDivide(double numerator, double denominator)
        {
            return SafeDivide(numerator, denominator, double.Epsilon);
        }
        public static double SafeDivide(double numerator, double denominator, double eps)
        {
            //if (Math.Abs(denominator) < 0.000000001)
            //{
            //    Console.WriteLine("stop");
            //}
            //double result = numerator == 0 ? 0 : numerator / denominator;
            double result = denominator == 0 && Math.Abs(numerator) < eps || double.IsNaN(numerator) ? 0 : numerator / denominator;

            if (double.IsNaN(result))
            {
                throw new ArgumentException(string.Format("divide by zero error. {0}/{1} = {2}", numerator, denominator, result));
            }
            //Helper.CheckCondition(!double.IsPositiveInfinity(result) && !double.IsNaN(result), "divide by zero error.");
            //if (double.IsInfinity(result))
            //{
            //    Console.WriteLine("stop");
            //}
            return result;
        }


#if !SILVERLIGHT

        public static ComplexNumber SafeDivide(ComplexNumber numerator, ComplexNumber denominator)
        {
            return SafeDivide(numerator, denominator, double.Epsilon);
        }
        public static ComplexNumber SafeDivide(ComplexNumber numerator, ComplexNumber denominator, double eps)
        {
            ComplexNumber result = denominator == 0 && ComplexNumber.Abs(numerator) < eps ? 0 : numerator / denominator;
            if (double.IsNaN(result.Real) || double.IsNaN(result.Img))
            {
                throw new ArgumentException(string.Format("divide by zero error. {0}/{1} = {2}", numerator, denominator, result));
            }
            return result;
        }

#endif
        /// <summary>
        /// Converts p(A,B|C) to p(A|B,C) using p(B|C).
        /// </summary>
        /// <param name="jointABGivenC"></param>
        /// <param name="marginalBGivenC"></param>
        public static void JointToConditionalProb(double[][] jointABGivenC, double[] marginalBGivenC, double[][] result, bool useLogTransform)
        {
            for (int iParentState = 0; iParentState < jointABGivenC.GetLength(0); iParentState++)
            {
                for (int iChildState = 0; iChildState < jointABGivenC[iParentState].Length; iChildState++)
                {
                    result[iParentState][iChildState] =
                        useLogTransform ?
                            double.IsNegativeInfinity(jointABGivenC[iParentState][iChildState]) && double.IsNegativeInfinity(marginalBGivenC[iParentState]) ?
                                double.NegativeInfinity :
                                jointABGivenC[iParentState][iChildState] - marginalBGivenC[iParentState] :
                            jointABGivenC[iParentState][iChildState] == 0 && marginalBGivenC[iParentState] == 0 ?
                                0 :
                                jointABGivenC[iParentState][iChildState] / marginalBGivenC[iParentState];
                }
            }
        }

        public static void ConditionalToJointProb(double[][] pAGivenB, double[] pB, double[][] result, bool useLogTransform)
        {
            for (int iParentState = 0; iParentState < pAGivenB.GetLength(0); iParentState++)
            {
                for (int iChildState = 0; iChildState < pAGivenB[iParentState].Length; iChildState++)
                {
                    result[iParentState][iChildState] =
                        useLogTransform ?
                            pAGivenB[iParentState][iChildState] + pB[iParentState] :
                            pAGivenB[iParentState][iChildState] * pB[iParentState];
                }
            }
        }

        public static void InitializeDoubleArray(ref double[] dArr, int length, bool logspace)
        {
            if (dArr == null || dArr.Length != length)
            {
                dArr = new double[length];
            }
            else if (!logspace)
            {
                Array.Clear(dArr, 0, dArr.Length);
            }
            if (logspace)
            {
                SetAllValues(dArr, double.NegativeInfinity);
            }
        }

        public static void InitializeDoubleArray(ref double[][] dArr, int length0, int length1, bool logspace)
        {
            if (dArr == null || dArr.Length != length0)
            {
                dArr = new double[length0][];
            }
            for (int i = 0; i < length0; i++)
            {
                InitializeDoubleArray(ref dArr[i], length1, logspace);
            }
        }

        public static void InitializeDoubleArray(ref double[,] dArr, int length0, int length1, bool logspace)
        {
            if (dArr == null || dArr.Length != length0)
            {
                dArr = new double[length0, length1];
            }
            else if (!logspace)
            {
                Array.Clear(dArr, 0, dArr.Length);
            }
            if (logspace)
            {
                SetAllValues(dArr, double.NegativeInfinity);
            }
        }



        public static void CreateArrayIfNull<T>(ref T[][] array, int size1, int size2)
        {
            if (array == null || array.Length != size1 || array[0].Length != size2)
            {
                array = new T[size1][];
                for (int i = 0; i < size1; i++)
                {
                    array[i] = new T[size2];
                }
            }
        }

        public static void CreateArrayIfNull<T>(ref T[] array, int size1)
        {
            if (array == null || array.Length != size1)
            {
                array = new T[size1];
            }
        }


        public static double[][] CreateIdentityMatrix(int stateCount)
        {
            double[][] id = new double[stateCount][];
            for (int i = 0; i < stateCount; i++)
            {
                id[i] = new double[stateCount];
                id[i][i] = 1;
            }
            return id;
        }

        public static void CreateEqualProbabilityMatrix(double[][] id, int stateCount)
        {
            //double[][] id = new double[stateCount][];
            for (int i = 0; i < stateCount; i++)
            {
                id[i] = new double[stateCount];
                for (int j = 0; j < stateCount; j++)
                {
                    id[i][j] = 1.0 / stateCount;
                }
            }
            //return id;
        }

        public static void SetAllValues<T>(T[] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        public static void SetAllValues<T>(T[,] array, T value)
        {
            for (int i = 0; i < array.GetLength(0); i++)
                for (int j = 0; j < array.GetLength(1); j++)
                    array[i, j] = value;
        }

        public static void SetAllValues<T>(T[][] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
                SetAllValues(array[i], value);
        }


        public static void ComputeSampleMeanAndVar(double[] values, out double mean, out double variance)
        {
            int n = values.Length;
            double sum = 0;
            double ss = 0;
            foreach (double d in values)
            {
                sum += d;
                ss += d * d;
            }

            mean = sum / n;
            variance = (ss - (sum * sum / n)) / (n - 1);
        }

        public static void ComputeSampleMeansAndVariances(double[][] setOfValues, out double[] means, out double[] variances)
        {
            double[][] transposed = TransposeArray(setOfValues);

            means = new double[transposed.Length];
            variances = new double[transposed.Length];

            for (int i = 0; i < transposed.Length; i++)
            {
                ComputeSampleMeanAndVar(transposed[i], out means[i], out variances[i]);
            }
        }


        [Obsolete("This has be superseded  by the List.SampleAtRandom extension method.")]
        public static T SelectRandom<T>(List<T> list, ref Random rand)
        {
            int index = rand.Next(list.Count);
            return list[index];
        }

        /// <summary>
        /// This could be slow, so only use it where speed is not important.
        /// </summary>
        public static void AppendLine(string fileName, string line)
        {
            using (StreamWriter streamWriter = File.AppendText(fileName))
            {
                streamWriter.WriteLine(line);
            }
        }

        public static IEnumerable<int> EnumerateRange(int start, int last, int increment)
        {
            Helper.CheckCondition(increment == 1 || increment == -1, "increment must be 1 or -1");
            for (int i = start; ; i += increment)
            {
                yield return i;
                if (i == last)
                {
                    break;
                }
            }
        }

        public static IEnumerable<T> EnumerateInterleave<T>(IEnumerable<T> enum1, IEnumerable<T> enum2)
        {
            IEnumerator<T> eor1 = enum1.GetEnumerator();
            IEnumerator<T> eor2 = enum2.GetEnumerator();
            while (true)
            {
                if (!eor1.MoveNext())
                {
                    while (eor2.MoveNext())
                    {
                        yield return eor2.Current;
                    }
                    yield break;

                }
                if (!eor2.MoveNext())
                {
                    while (true)
                    {
                        yield return eor1.Current;
                        if (!eor1.MoveNext())
                        {
                            yield break;
                        }
                    }
                }

                yield return eor1.Current;
                yield return eor2.Current;
            }
        }


        //!!!these SampleFromList, SelectRandom, and the extension method that returns one, have names that don't work well together
        /// <summary>
        /// Samples from a list with replacement. See "SelectRandom" for no replacement.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="srcList"></param>
        /// <param name="count"></param>
        /// <param name="destList"></param>
        /// <param name="rand"></param>
        public static void SampleFromList<T>(List<T> srcList, int count, ref List<T> destList, ref Random rand)
        {
            for (int i = 0; i < count; i++)
            {
                int idx = rand.Next(0, srcList.Count);
                destList.Add(srcList[idx]);
            }
        }


        public static T Next<T>(IEnumerator<T> lineCollection)
        {
            Helper.CheckCondition(lineCollection.MoveNext(), "Can't get next");
            return lineCollection.Current;
        }

        [Obsolete("Use the Linq version.")]
        public static IEnumerable<T> EnumerateFilter<T>(IEnumerable<T> iEnumerable, Predicate<T> predicate)
        {
            foreach (T t in iEnumerable)
            {
                if (!predicate(t))
                {
                    yield return t;
                }
            }
        }

        ///!!!should "new FileStream" be in a Using so Dispose gets done?
        ///JC: No, the call to this method should be in a using: using(var streamWriter = GetTextWriterWithExternalReadAccess(f)){ // do something }
        public static StreamWriter GetTextWriterWithExternalReadAccess(string filename)
        {
            return new StreamWriter(new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.Read));
        }

        ///!!!should "new FileStream" be in a Using so Dispose gets done?
        public static StreamReader GetTextReaderWithExternalReadWriteAccess(string filename)
        {
            //return new StreamReader(new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite));
            return Bio.Util.FileUtils.StripComments(new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite));
        }

        [Obsolete("Moved to FileUtils")]
        public static void DeleteFilesPatiently(Queue<FileInfo> filesToDelete, int numberOfTimesToTry, TimeSpan timeToWaitBetweenRetries)
        {
            Queue<FileInfo> filesNotDeleted = new Queue<FileInfo>();

            foreach (FileInfo fileInfo in filesToDelete)
            {
                try
                {
                    fileInfo.Delete();
                }
                catch (System.IO.IOException)
                {
                    filesNotDeleted.Enqueue(fileInfo);
                }
            }

            if (filesNotDeleted.Count > 0)
            {
                if (numberOfTimesToTry > 0)
                {
                    Console.WriteLine("Could not delete all files. Will retry after a short pause.");
                    System.Threading.Thread.Sleep(timeToWaitBetweenRetries);
                    DeleteFilesPatiently(filesNotDeleted, numberOfTimesToTry - 1, timeToWaitBetweenRetries);
                }
                else
                {
                    StringBuilder sb = new StringBuilder("Could not delete the following files: ");
                    sb.Append(filesNotDeleted.StringJoin(","));
                    throw new Exception(sb.ToString());
                }
            }
        }

        /// <summary>
        /// Computes the minimum of  max(count, E[count]) over all four counts. The expected count is computed using
        /// the marginal counts. 
        /// </summary>
        public static double FishersMinimax(int tt, int tf, int ft, int ff)
        {
            double sum = tt + tf + ft + ff;
            if (sum == 0)
                return 0;

            double v1TrueP = (tt + tf);
            double v2TrueP = (tt + ft);
            double v1FalseP = (ff + ft);
            double v2FalseP = (ff + tf);

            double ett = v1TrueP * v2TrueP / sum;
            double etf = v1TrueP * v2FalseP / sum;
            double eft = v1FalseP * v2TrueP / sum;
            double eff = v1FalseP * v2FalseP / sum;

            double minCount = Math.Min(Math.Max(tt, ett),
                              Math.Min(Math.Max(tf, etf),
                              Math.Min(Math.Max(ft, eft),
                                       Math.Max(ff, eff))));

            return minCount;
        }

        public static double MinMarginalCount(int tt, int tf, int ft, int ff)
        {
            double v1TrueP = (tt + tf);
            double v2TrueP = (tt + ft);
            double v1FalseP = (ff + ft);
            double v2FalseP = (ff + tf);

            return (Math.Min(Math.Min(Math.Min(v1TrueP, v2TrueP), v1FalseP), v2FalseP));
        }

        /// <summary>
        /// Generalization of FishersMinimax for more that 2x2 tables.
        /// </summary>
        /// <param name="counts"></param>
        /// <returns></returns>
        public static double FishersMinimaxGeneralized(int[][] counts)
        {
            double sum = 0;
            double[] sumOfRows = new double[counts.Length];
            double[] sumOfCols = new double[counts[0].Length];
            // Calculate row/column sums
            for (int ii = 0; ii < counts.Length; ii++)
            {
                for (int jj = 0; jj < counts[ii].Length; jj++)
                {
                    sum += counts[ii][jj];
                    sumOfRows[ii] += counts[ii][jj];
                    sumOfCols[jj] += counts[ii][jj];
                }
            }
            // Calculates max of observed counts and expected counts
            // and keep track of smallest such value seen so far
            double min = double.PositiveInfinity;
            for (int ii = 0; ii < counts.Length; ii++)
                for (int jj = 0; jj < counts[ii].Length; jj++)
                {
                    min = Math.Min(min, Math.Max(counts[ii][jj], sumOfRows[ii] * sumOfCols[jj] / sum));
                }
            return min;
        }

        public static double Product(double[] pValues)
        {
            double product = 0;
            foreach (double d in pValues)
            {
                product += Math.Log(d);
            }
            return Math.Exp(product);
        }

        public static void IncrementBitArray(BitArray bitArr)
        {
            IncrementBitArray(bitArr, 0);
        }

        private static void IncrementBitArray(BitArray bitArr, int idx)
        {
            bitArr[idx] = !bitArr[idx];
            if (!bitArr[idx])
                IncrementBitArray(bitArr, (idx + 1) % bitArr.Count);
        }

        internal static double NoisyOr(List<double> independentPTrueValues)
        {
            double pNot = 1;
            foreach (double pTrue_i in independentPTrueValues)
            {
                pNot *= 1 - pTrue_i;
            }
            double pTrue = 1 - pNot;
            return pTrue;
        }

        public static void ProbabilityChildGivenGrandparent(double[][] pParentGivenGrandParent, double[][] pChildGivenParent, double[][] result, bool useLog)
        {
            int stateCount = pChildGivenParent.GetLength(0);

            for (int i = 0; i < stateCount; i++)
            {
                SetAllValues(result[i], useLog ? double.NegativeInfinity : 0);

                for (int j = 0; j < stateCount; j++)
                {
                    for (int k = 0; k < stateCount; k++)
                    {
                        if (useLog)
                        {
                            result[i][j] += pParentGivenGrandParent[i][k] + pChildGivenParent[k][j];
                            throw new Exception("This seems wrong. Shouldn't we be using log sum. I just implemented below, but can't test right now");
                            //result[i][j] = LogSum(result[i][j], pParentGivenGrandParent[i][k] + pChildGivenParent[k][j]);
                        }
                        else
                        {
                            result[i][j] += pParentGivenGrandParent[i][k] * pChildGivenParent[k][j];
                        }
                    }
                }
            }
        }

        public static double Deg2Rad(double angleInDegrees)
        {
            return angleInDegrees * Math.PI / 180;
        }

        public static double Rad2Deg(double angleInRadians)
        {
            return angleInRadians * 180 / Math.PI;
        }

#if SILVERLIGHT
        public static double DistanceOfPointToLine(Point queryPoint, Point p1, Point p2)
        {
            double x2MinusX1 = p2.X - p1.X;
            double y2MinusY1 = p2.Y - p1.Y;
            double dist = Math.Abs(x2MinusX1 * (p1.Y - queryPoint.Y) - (p1.X - queryPoint.X) * y2MinusY1);
            dist /= Math.Sqrt(x2MinusX1 * x2MinusX1 + y2MinusY1 * y2MinusY1);
            return dist;
        }
#else
        public static double DistanceOfPointToLine(PointF queryPoint, PointF p1, PointF p2)
        {
            double x2MinusX1 = p2.X - p1.X;
            double y2MinusY1 = p2.Y - p1.Y;
            double dist = Math.Abs(x2MinusX1 * (p1.Y - queryPoint.Y) - (p1.X - queryPoint.X) * y2MinusY1);
            dist /= Math.Sqrt(x2MinusX1 * x2MinusX1 + y2MinusY1 * y2MinusY1);
            return dist;
        }

        [Obsolete("Moved to FileUtiles and slightly changed signature.")]
        public static void MergeFiles(string inputFilePattern, string[] columnNamesToAdd, string outputFileName, bool deleteFilesOnSuccessfullMerge, bool printProgress, bool continueOnFailure)
        {
            var files = Bio.Util.FileUtils.GetFiles(inputFilePattern, zeroIsOK: false).ToList();
            string dirName = Path.GetDirectoryName(files[0]);
            var inputFilePatternCollection =
            files.Select(fullPath =>
            {
                Helper.CheckCondition(dirName == Path.GetDirectoryName(fullPath), "all files in the pattern must be in the same directory");
                return Path.GetFileName(fullPath);
            });

            var dirInfo = new DirectoryInfo(dirName);

            MergeFiles(dirInfo, inputFilePatternCollection, columnNamesToAdd, outputFileName, deleteFilesOnSuccessfullMerge, printProgress, continueOnFailure);
        }

#endif
        //!!!would be better if the signal to use stdout was Null instead of "-"
        [Obsolete("Moved to FileUtiles and slightly changed signature.")]
        public static void MergeFiles(DirectoryInfo dirinfo, IEnumerable<string> inputFilePatternCollection, string[] columnNamesToAdd, string outputFileName, bool deleteFilesOnSuccessfullMerge, bool printProgress, bool continueOnFailure)
        {
            string tmpFile = outputFileName + new Random().Next(int.MaxValue);
            List<Tuple<string, string[]>> filePatternsAndColumnNames = new List<Tuple<string, string[]>>();
            foreach (string filePattern in inputFilePatternCollection)
            {
                string[] filePatternAndColValues = filePattern.Split(':');
                Helper.CheckCondition(filePatternAndColValues.Length == 1 + columnNamesToAdd.Length, "{0} does not have the right number of added columns specified. Expected {1}; found {2}.", filePattern, columnNamesToAdd.Length, filePatternAndColValues.Length - 1);
                string[] colVals = columnNamesToAdd.Length == 0 ? new string[0] : SpecialFunctions.SubArray(filePatternAndColValues, 1);
                filePatternsAndColumnNames.Add(Tuple.Create(filePatternAndColValues[0], colVals));
            }

            List<FileInfo> filesInMerge = new List<FileInfo>();
            //FileInfo outputFileInfo = new FileInfo(outputFileName);
            using (TextWriter textWriter = outputFileName == "-" ? Console.Out : File.CreateText(tmpFile)) // Do this early so that if it fails, well know
            {
                string universalHeader = null;

                foreach (Tuple<string, string[]> filePatternAndColVals in filePatternsAndColumnNames)
                {
                    if (printProgress)
                        Console.WriteLine();
                    foreach (FileInfo fileinfo in dirinfo.EnumerateFiles(filePatternAndColVals.Item1)) //EnumerateFiles(dirinfo, inputFilePatternCollection))
                    {
                        Console.Error.WriteLine(fileinfo.Name);
                        if (printProgress)
                            Console.Write("\rMerging {0}", fileinfo.Name);
                        filesInMerge.Add(fileinfo);
                        string headerOnFile;
                        using (TextReader reader = SpecialFunctions.GetTextReaderWithExternalReadWriteAccess(fileinfo.FullName))
                        {
                            headerOnFile = reader.ReadLine();
                            if (universalHeader == null)
                            {
                                universalHeader = headerOnFile;
                                if (columnNamesToAdd.Length > 0)
                                {
                                    textWriter.WriteLine(columnNamesToAdd.StringJoin("\t") + '\t' + universalHeader);
                                }
                                else
                                {
                                    textWriter.WriteLine(universalHeader);
                                }
                            }
                            else if (universalHeader != headerOnFile)
                            {
                                File.Delete(tmpFile);
                                throw new ArgumentException(string.Format("ERROR: The header for file {0} is different from the 1st file read in. \nCurrent header: {1}\nFirst header: {2}\nAborting file merge.",
                                    fileinfo.Name, headerOnFile, universalHeader), fileinfo.Name);
                            }
                        }

                        using (TextReader reader = SpecialFunctions.GetTextReaderWithExternalReadWriteAccess(fileinfo.FullName))
                        {
                            try
                            {
                                foreach (Dictionary<string, string> row in SpecialFunctions.TabFileTable(reader, headerOnFile, includeWholeLine: true))
                                {
                                    if (columnNamesToAdd.Length > 0)
                                    {
                                        textWriter.WriteLine(filePatternAndColVals.Item2.StringJoin("\t") + '\t' + row[""]);  // write the whole line
                                    }
                                    else
                                    {
                                        textWriter.WriteLine(row[""]);  // write the whole line
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                if (!continueOnFailure)
                                {
                                    Console.WriteLine("TEST: Caught an error, so rethrowing.");
                                    throw e;
                                }
                                else
                                {
                                    if (printProgress)
                                    {
                                        Console.Error.WriteLine("Error processing " + fileinfo.Name + ". " + e.Message);
                                    }
                                }
                            }
                        }
                    }
                }
                if (printProgress)
                    Console.WriteLine();
            }

            if (outputFileName != "-")
            {
                SpecialFunctions.MoveAndReplace(tmpFile, outputFileName);
            }

            if (deleteFilesOnSuccessfullMerge)
            {
                FileInfo outputFileInfo = new FileInfo(outputFileName);
                // if we get here, we were successful. Delete the files.
                Queue<FileInfo> filesToDelete = new Queue<FileInfo>();
                foreach (FileInfo fileinfo in filesInMerge)
                {
                    if (!fileinfo.FullName.Equals(outputFileInfo.FullName))
                    {
                        filesToDelete.Enqueue(fileinfo);
                    }
                }
                MBT.Escience.FileUtils.DeleteFilesPatiently(filesToDelete, 5, new TimeSpan(0, 1, 0)); // will try deleting for up to ~5 min before bailing.
            }
        }


        /// <summary>
        /// A wrapper for File.Move that will first delete dest if it exists.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dest"></param>
        public static void MoveAndReplace(string src, string dest)
        {
            if (File.Exists(dest))
            {
                File.Delete(dest);
            }
            try
            {
                File.Move(src, dest);
            }
            catch (Exception e)
            {
                Console.WriteLine("failed to move: " + src + " to: " + dest);
                throw e;
            }
        }




        public static Dictionary<T1, T2> MergeDictionaries<T1, T2>(params Dictionary<T1, T2>[] dictionaryArray)
        {
            Dictionary<T1, T2> result = new Dictionary<T1, T2>();
            foreach (Dictionary<T1, T2> input in dictionaryArray)
            {
                foreach (KeyValuePair<T1, T2> keyAndValue in input)
                {
                    result.Add(keyAndValue.Key, keyAndValue.Value);
                }
            }
            return result;

        }


        private static double Mean(params double[] doubleArray)
        {
            return Mean(doubleArray);
        }

        //[Obsolete("Moved to FileUtils.cs")]
        //public static void CreateDirectoryForFileIfNeeded(string fileName)
        //{
        //    string outputDirectoryName = Path.GetDirectoryName(fileName);
        //    if ("" != outputDirectoryName)
        //    {
        //        Directory.CreateDirectory(outputDirectoryName);
        //    }
        //}


        public static void Copy2DSquareArray(double[][] src, ref double[][] dest)
        {
            if (dest == null)
            {
                CreateArrayIfNull(ref dest, src.Length, src.Length);
            }
            for (int i = 0; i < src.Length; i++)
            {
                Array.Copy(src[i], dest[i], src[i].Length);
            }
        }

        [Obsolete("This has be superseded  by 'x=>x'")]
        public static T Identity<T>(T t)
        {
            return t;
        }

        [Obsolete("See escience matrix similarityMeasures")]
        /// <summary>
        /// See http://en.wikipedia.org/wiki/Pearson_r
        /// AKA CORREL, PEARSON, corr, related to RSQ in Excel
        /// NaN marks missing data
        /// </summary>
        /// <returns></returns>
        public static double PearsonRWithMissing(IEnumerable<double> vector1, IEnumerable<double> vector2, out int n, bool doCorrel = true)
        {
            double r;
            bool isOK = TryPearsonRWithMissing(vector1, vector2, out r, out n, doCorrel);
            Helper.CheckCondition(isOK, "Pearson R is not defined when an input vector is constant");
            return r;
        }

        [Obsolete("See escience matrix similarityMeasures")]
        public static double CovarianceWithMissing(IEnumerable<double> vector1, IEnumerable<double> vector2)
        {
            return PearsonRWithMissing(vector1, vector2, doCorrel: false);
        }

        [Obsolete("See escience matrix similarityMeasures")]
        public static double PearsonRWithMissing(IEnumerable<double> vector1, IEnumerable<double> vector2, bool doCorrel = true)
        {
            int n;
            double r;
            bool isOK = TryPearsonRWithMissing(vector1, vector2, out r, out n, doCorrel);
            Helper.CheckCondition(isOK, "Pearson R is not defined when an input vector is constant");
            return r;
        }


        [Obsolete("See escience matrix similarityMeasures")]
        public static bool TryPearsonRWithMissing(IEnumerable<double> vector1, IEnumerable<double> vector2, out double r, out int n, bool doCorrel = true)
        {
            Helper.CheckCondition(doCorrel, "Code gives wrong answer for Covariance");
            //One pass forumla from page 298, eq #20 in _Theory and Problems of Statistics 2/ed_, 1992 by Murray R. Spiegel (Schaum's Outline Series)

            double sumXY = 0;
            double sumXX = 0;
            double sumX = 0;
            double sumY = 0;
            double sumYY = 0;
            n = 0;
            foreach (KeyValuePair<double, double> xiAndyi in EnumerateTwo(vector1, vector2))
            {
                double xi = xiAndyi.Key;
                double yi = xiAndyi.Value;
                if (double.IsNaN(xi) || double.IsNaN(yi))
                {
                    continue; //continue, not break;
                }
                ++n;
                sumXY += xi * yi;
                sumX += xi;
                sumY += yi;
                sumXX += xi * xi;
                sumYY += yi * yi;
            }
            double num = n * sumXY - sumX * sumY;
            double den = (doCorrel) ? Math.Sqrt((n * sumXX - sumX * sumX) * (n * sumYY - sumY * sumY)) : n;
            if (den == 0)
            {
                //Pearson R is not defined when an input vector is constant
                r = double.NaN;
                return false;
            }
            r = num / den;
            return true;
        }



        //!!!Is there a better way to do this?
        [Obsolete("This has be superseded  by 'Parser.Parse<T>(string field)', though this method still needs work")]
        public static object Parse(string field, Type type)
        {
            if (type.Equals(typeof(string)))
            {
                return field;
            }
            if (type.Equals(typeof(int)))
            {
                return int.Parse(field);
            }
            if (type.Equals(typeof(double)))
            {
                return double.Parse(field);
            }
            if (type.Equals(typeof(char)))
            {
#if !SILVERLIGHT
                return char.Parse(field);
#else
                Helper.CheckCondition(field.Length == 1, "Can't parse as char. Too long. " + field);
                return field[0];
#endif
            }
            throw new Exception("Don't know how to parse type " + type.Name);
        }



        public static List<string> ParenProtectedSplit(string s, char delimiter)
        {
            List<string> result = new List<string>();
            Stack<int> openParens = new Stack<int>();

            int start = 0;
            for (int stop = 0; stop < s.Length; stop++)
            {
                if (s[stop] == '(')
                {
                    openParens.Push(stop);
                }
                else if (s[stop] == ')')
                {
                    Helper.CheckCondition(openParens.Count > 0, "Unmatched close parenthesis at position " + stop);
                    openParens.Pop();
                }
                else if (s[stop] == delimiter && openParens.Count == 0)
                {
                    string itemString = s.Substring(start, stop - start);
                    result.Add(itemString);
                    start = stop + 1;
                }
            }
            if (openParens.Count > 0)
            {
                throw new ArgumentException("Unmatched open parenthesis at position " + openParens.Pop());
            }
            result.Add(s.Substring(start));

            return result;
        }

        public static void Transform<T>(List<T> values, Converter<T, T> transformer)
        {
            for (int i = 0; i < values.Count; i++)
            {
                values[i] = transformer(values[i]);
            }
        }

        /////This seems like a bad idea because two object can hash together and not equal
        //static public int GenericCompareTo(object o1, object o2)
        //{
        //    if (o1 == o2)
        //    {
        //        return 0;
        //    }
        //    if (o1 == null)
        //    {
        //        return -1;
        //    }
        //    if (o2 == null)
        //    {
        //        return 1;
        //    }
        //    IComparable comparable = o1 as IComparable;
        //    if (null == comparable)
        //    {
        //        return o1.GetHashCode().CompareTo(o2.GetHashCode());
        //    }
        //    else
        //    {
        //        return comparable.CompareTo(o2);

        public static IEnumerable<string[]> EachSparseLine(string filePattern, bool zeroIsOK, string fileMessageOrNull)
        {
            foreach (string fileName in Bio.Util.FileUtils.GetFiles(filePattern, zeroIsOK))
            {
                if (null != fileMessageOrNull)
                {
                    Console.WriteLine(fileMessageOrNull, fileName);
                }
                using (TextReader textReader = Bio.Util.FileUtils.OpenTextStripComments(fileName))
                {
                    string header = textReader.ReadLine();
                    Helper.CheckCondition(header != null, "Expect header");
                    Helper.CheckCondition(header.Equals("var\tcid\tval", StringComparison.InvariantCultureIgnoreCase), "Expected header of 'var<tab>cid<tab>val'. Not " + header);
                    string line;
                    while (null != (line = textReader.ReadLine()))
                    {
                        string[] fields = line.Split('\t');
                        Helper.CheckCondition(fields.Length == 3, "Expect 3 fields on each line. Not " + line);
                        yield return fields;
                    }
                }
            }
        }

        /// <summary>
        /// Assumes, but doesn't check that variables are together and that they don't span files
        /// </summary>
        public static IEnumerable<List<string[]>> SparseGroupedByVar(string filePattern, bool zeroIsOK, string fileMessageOrNull)
        {

            List<string[]> group = null;
            string currentVar = null;
            foreach (string[] varCidVal in EachSparseLine(filePattern, zeroIsOK, fileMessageOrNull))
            {
                string var = varCidVal[0];
                if (currentVar != var)
                {
                    if (currentVar != null)
                    {
                        yield return group;
                    }
                    currentVar = var;
                    group = new List<string[]>();
                }
                group.Add(varCidVal);
            }
            if (currentVar != null)
            {
                yield return group;
            }
        }


        public static Dictionary<string, bool> ReadSparseTargetFile(string targetVarOrNull, string targetSparseFileName)
        {
            Dictionary<string, bool> cidToIsCase = new Dictionary<string, bool>();
            SingletonSet<string> varSet = SingletonSet<string>.GetInstance("Target file should contain only one distinct variable. " + targetSparseFileName);
            foreach (var row in MBT.Escience.FileUtils.ReadDelimitedFile(targetSparseFileName, new { var = "", cid = "", val = 0 }, new char[] { '\t' }, true))
            {
                if (targetVarOrNull != null && 0 != string.Compare(targetVarOrNull, row.var, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue; // not break;
                }
                cidToIsCase.Add(row.cid, ZeroOneToBool(row.val));
                varSet.Add(row.var);
            }
            Helper.CheckCondition(!varSet.IsEmpty, "target file should not be empty. " + targetSparseFileName);
            return cidToIsCase;
        }

        public static bool ZeroOneToBool(int val)
        {
            switch (val)
            {
                case 0:
                    return false;
                case 1:
                    return true;
                default:
                    Helper.CheckCondition(false, "Unknown target value. " + val.ToString());
                    return false;
            }

        }


        public static Dictionary<T, int> IndexList<T>(IList<T> list)
        {
            Dictionary<T, int> indexCollection = new Dictionary<T, int>(list.Count);
            for (int i = 0; i < list.Count; ++i)
            {
                indexCollection.Add(list[i], i);
            }
            return indexCollection;
        }

        public static bool EqualIfNewElseSet<T>(ref Nullable<T> oldValueOrNull, T newValue) where T : struct
        {
            if (oldValueOrNull.HasValue)
            {
                return newValue.Equals(oldValueOrNull.Value);
            }
            else
            {
                oldValueOrNull = newValue;
                return true;
            }
        }

        public static IEnumerable<IEnumerable<T>> GroupByCount<T>(IList<T> list, int subLength, bool errorIfUneven)
        {
            for (int i = 0; i < list.Count; i += subLength)
            {
                yield return NextN(list, i, subLength, errorIfUneven);
            }
        }

        private static IEnumerable<T> NextN<T>(IList<T> list, int start, int subLength, bool errorIfUneven)
        {
            for (int i = start; i < start + subLength; ++i)
            {
                if (i >= list.Count)
                {
                    Helper.CheckCondition(!errorIfUneven, "List length of the list {0} is not divisible by the subLength {1}", list.Count, subLength);
                    yield break;
                }
                yield return list[i];
            }
        }

        public static IEnumerable<KeyValuePair<T, int>> Tally<T>(IEnumerable<T> list)
        {
            Dictionary<T, int> itemToCount = new Dictionary<T, int>();
            foreach (T item in list)
            {
                itemToCount[item] = 1 + itemToCount.GetValueOrDefault(item);
            }

            var itemToSortedCount = itemToCount.OrderBy(itemAndCount => itemToCount.Values);
            return itemToCount;
        }

        /// <summary>
        /// Efficently returns a random sample (without replacement) of iEnumerable of size sampleCount.
        /// If sampleCount == int.MaxValue then returns all
        /// If sampleCount > iEnumerable's count, then returns all
        /// </summary>
        public static IEnumerable<T> SelectRandom<T>(IEnumerable<T> iEnumerable, int sampleCount, ref Random random)
        {
            if (int.MaxValue == sampleCount)
            {
                return iEnumerable;
            }
            else
            {
                List<T> buffer = new List<T>();
                int itemCount = 0;
                foreach (T item in iEnumerable)
                {
                    ++itemCount;
                    if (buffer.Count < sampleCount)
                    {
                        buffer.Add(item);
                    }
                    else
                    {
                        int randomPlace = random.Next(itemCount);
                        if (randomPlace < buffer.Count)
                        {
                            buffer[randomPlace] = item;
                        }
                    }
                }
                return buffer;
            }
        }

        /// <summary>
        /// Returns the date and time that the Assembly that type is in was compiled. Note that the time never includes daylight savings.
        /// In order to work, you must set [assembly: AssemblyVersion("1.0.*")] in AssemblyInfo.cs.  This can be done either manually or by
        /// editing Assembly Information in the project's Properties.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DateTime DateCompiled(Type type)
        {

            System.Version version = type.Assembly.GetName().Version;
            // v.Build is days since Jan. 1, 2000
            // v.Revision*2 is seconds since local midnight
            // (NEVER daylight saving time)

            DateTime time = new DateTime(
            version.Build * TimeSpan.TicksPerDay +
            version.Revision * TimeSpan.TicksPerSecond * 2
            ).AddYears(1999);

            return time;
        }

        /// <summary>
        /// Returns the date and time that the Assembly that the program started in was compiled. Note that the time never includes daylight savings.
        /// In order to work, you must set [assembly: AssemblyVersion("1.0.*")] in AssemblyInfo.cs.  This can be done either manually or by
        /// editing Assembly Information in the project's Properties.
        /// </summary>
        /// <returns></returns>
        public static DateTime DateProgramWasCompiled()
        {
            Assembly asm = SpecialFunctions.GetEntryOrCallingAssembly();
            System.Version version = asm.GetName().Version;
            // v.Build is days since Jan. 1, 2000
            // v.Revision*2 is seconds since local midnight
            // (NEVER daylight saving time)
            long tmpSum = (version.Build - 1) * TimeSpan.TicksPerDay + version.Revision * TimeSpan.TicksPerSecond * 2;
            DateTime time;
            if (tmpSum > 0)
            {
                time = new DateTime(tmpSum).AddYears(1999);
            }
            else
            {
                //not a valid build--happens when I'm in debug mode for some reason
                //time = new DateTime(0);
                FileInfo assemblyFile = new FileInfo(new Uri(asm.CodeBase).LocalPath);
                if (assemblyFile.Exists)
                    time = assemblyFile.LastWriteTime;
                else
                    time = new DateTime(0);
            }

            return time;
        }


        public static bool TryParseBoolOrNullListToInt(IList<bool?> values, out int fullyInitializedState)
        {
            Helper.CheckCondition(values.Count <= 32);
            fullyInitializedState = 0;

            int pos = values.Count - 1;
            foreach (bool? val in values)
            {
                if (!val.HasValue)
                    return false;
                if (val.Value)
                    fullyInitializedState |= 1 << pos--;
            }
            return true;
        }

        public static int ParseBoolOrNullListToInt(IList<bool?> values)
        {
            int result;
            if (!TryParseBoolOrNullListToInt(values, out result))
                throw new ArgumentException("values has some null results.");
            return result;
        }

#if !SILVERLIGHT
        public static void WriteMemoryInformation(TextWriter textWriter)
        {
            double mb = Math.Pow(2, 20);

            Process thisProc = Process.GetCurrentProcess();
            textWriter.WriteLine("Process\tActiveMem\tPeakActiveMem\tVirtualMem\tPeakVirtualMem");
            textWriter.WriteLine("Current\t{0:f2}MB\t{1:f2}MB\t{2:f2}MB\t{3:f2}MB", thisProc.WorkingSet64 / mb, thisProc.PeakWorkingSet64 / mb, thisProc.VirtualMemorySize64 / mb, thisProc.PeakVirtualMemorySize64 / mb);

            //!!! This needs admin rights.
            //foreach (Process proc in Process.GetProcesses())
            //{
            //    textWriter.Write("{0}\t{1}MB\t{2}MB\t{3}MB", proc.MainModule.ModuleName, proc.WorkingSet64 / mb, proc.VirtualMemorySize64 / mb, proc.PeakVirtualMemorySize64 / mb);
            //}
            textWriter.Flush();
        }
#endif

        public static int[] ArraySum(int[] d1, int[] d2)
        {
            Helper.CheckCondition(d1.Length == d2.Length);
            int[] result = new int[d1.Length];
            for (int i = 0; i < d1.Length; i++)
            {
                result[i] = d1[i] + d2[i];
            }
            return result;
        }

        public static double[] ArraySum(double[] d1, double[] d2, bool logspace)
        {
            Helper.CheckCondition(d1.Length == d2.Length);
            double[] result = new double[d1.Length];
            for (int i = 0; i < d1.Length; i++)
            {
                result[i] = logspace ? SpecialFunctions.LogSum(d1[i], d2[i]) : d1[i] + d2[i];
            }
            return result;
        }

        public static void ArraySum(double[] d1, double[] d2, double[] result, bool logspace)
        {
            Helper.CheckCondition(d1.Length == d2.Length && d1.Length == result.Length);
            //double[] result = new double[d1.Length];
            for (int i = 0; i < d1.Length; i++)
            {
                result[i] = logspace ? SpecialFunctions.LogSum(d1[i], d2[i]) : d1[i] + d2[i];
            }
        }

        public static void ArraySum(double[][] d1, double[][] d2, double[][] result, bool logspace)
        {
            Helper.CheckCondition(d1.Length == d2.Length && d1.Length == result.Length);
            for (int i = 0; i < d1.Length; i++)
            {
                ArraySum(d1[i], d2[i], result[i], logspace);
            }
        }

        public static int[] SampleFromMultinomial(double[] probs, int n, Random rand)
        {
            int[] counts = new int[probs.Length];
            for (int i = 0; i < n; i++)
                counts[SampleFromMultinomial(probs, rand)]++;
            return counts;
        }

        public static int SampleFromMultinomial(double[] probs, Random rand)
        {
            double d = rand.NextDouble();
            double sum = 0;
            for (int i = 0; i < probs.Length; i++)
            {
                sum += probs[i];
                if (d < sum)
                    return i;
            }
            throw new Exception("Shouldn't be able to get here.");
        }

        public static double Entropy(int[] counts)
        {
            double n = counts.Sum();
            double ent = 0;
            foreach (int cnt in counts)
            {
                double f = cnt / n;
                if (f > 0)
                    ent -= f * Math.Log(f);
            }
            return ent;
        }

        public static double Entropy(double[] probs)
        {
            double ent = 0;
            foreach (double f in probs)
            {
                if (f > 0)
                    ent -= f * Math.Log(f);
            }
            return ent;
        }

        public static bool SetEquals<T>(IEnumerable<T> e1, IEnumerable<T> e2)
        {
            HashSet<T> h1 = e1.ToHashSet<T>();
            return h1.SetEquals(e2);
        }
        [Obsolete("Use 'SetEquals'")]
        public static bool AreEqualSets<T>(IEnumerable<T> e1, IEnumerable<T> e2)
        {
            return SetEquals(e1, e2);
        }



        public static List<string> MakeStringListOfIncreasingIntegersFromBaseName(string basename, int startVal, int numVals)
        {
            List<string> x = new List<string>(numVals);
            for (int i = startVal; i < numVals; i++)
            {
                x.Add(basename + i.ToString());
            }
            return x;
        }

        public static List<string> MakeStringListOfIncreasingIntegers(int startVal, int numVals)
        {
            return MakeStringListOfIncreasingIntegersFromBaseName("", startVal, numVals);
        }

        public delegate bool Predicate<T1, T2>(T1 t1, T2 t2);

        public class CharEqualityIgnoreCase : IEqualityComparer<char>
        {
            static int dist = Math.Abs((int)'A' - (int)'a');

            public CharEqualityIgnoreCase() { }

            public bool Equals(char x, char y)
            {
                return x == y || (char.IsLetter(x) && char.IsLetter(y) && Math.Abs((int)x - (int)y) == dist);
            }

            public int GetHashCode(char obj)
            {
                return char.IsLetter(obj) ? char.ToLower(obj).GetHashCode() : obj.GetHashCode();
            }
        }


        /// <summary>
        /// take a string like "01111222000111" and give back a List&lt;double&gt; containing 0,1,1, etc.
        /// </summary>
        /// <returns></returns>
        public static List<double> CharStringToDoubleList(string p)
        {
            return p.ToCharArray().Select(c => double.Parse(c.ToString())).ToArray().ToList();

        }

        public static Assembly GetEntryOrCallingAssembly()
        {
#if !SILVERLIGHT
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            // Console.WriteLine(entryAssembly);
            //Thread.Sleep(1000);
            //Console.WriteLine("entry assembly: {0} {1} ","no!!", "test");//Assembly.GetCallingAssembly());
            //string s = string.Format("No idea why, but this is necessary for MatrixFactory to work in UnitTesting on some machines...{0} {1} ", "really??", "no way!!");
            int sum = 0;
            for (int i = 1; i < 2; i++) { sum += i; }
            if (null != entryAssembly)
            {
                return entryAssembly;
            }
#endif
            return Assembly.GetCallingAssembly();
        }



        public static string GetCommonPrefix(string s1, string s2)
        {
            StringBuilder result = new StringBuilder(s1.Length);
            for (int i = 0; i < s1.Length && i < s2.Length; i++)
            {
                if (s1[i] == s2[i])
                    result.Append(s1[i]);
            }
            return result.ToString();
        }

        /// <summary>
        ///if using a dictionary to count instances of the key, then this will either set that key to zero if it doesn't exist, or
        ///else increment it if it does
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myDict"></param>
        /// <param name="thisKey"></param>
        public static void IncrementOrIntiDictionaryForCountingKeyValues<T>(Dictionary<T, int> myDict, T thisKey)
        {
            int thisCount;
            bool keyExists = myDict.TryGetValue(thisKey, out thisCount);
            if (keyExists)
            {
                myDict[thisKey]++;
            }
            else
            {
                myDict.Add(thisKey, 1);
            }
        }

        public static string TwoDArrayAsString<T>(T[][] m, string colDelim)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T[] row in m)
            {
                if (sb.Length > 0)
                    sb.Append('\n');
                sb.Append(row.StringJoin(colDelim));
            }
            return sb.ToString();
        }

        public static void CheckDate(int year, int month, int day)
        {
            CheckDate(year, month, day, "Not the expected date of " + year + "/" + month + "/" + day);
        }

        public static void CheckDate(int year, int month, int day, string message)
        {
            Helper.CheckCondition(DateTime.Now.Date == new DateTime(year, month, day), message);
        }

        public static bool CheckDateBool(int year, int month, int day)
        {
            return DateTime.Now.Date == new DateTime(year, month, day);
        }

        /// <summary>
        /// Turns outputDistn into the identity matrix.
        /// </summary>
        public static void IdentityMatrix(double[][] outputDistn)
        {
            for (int i = 0; i < outputDistn.Length; i++)
                for (int j = 0; j < outputDistn[i].Length; j++)
                    outputDistn[i][j] = i == j ? 1 : 0;
        }

        public static T[][] MatrixShallowCopy<T>(T[][] matrix)
        {
            T[][] result = new T[matrix.Length][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new T[matrix[i].Length];
                Array.Copy(matrix[i], result[i], matrix[i].Length);
            }
            return result;
        }


#if !SILVERLIGHT
        /// <summary>
        /// Will fail if all objects in the object graph are not marked with the [Serializable] attribute.
        /// </summary>
        public static void Serialize(object obj, string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, obj);
            }
        }

        public static object Deserialize(string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return formatter.Deserialize(stream);
            }
        }
#endif




        public static double[][] Create2DArray(int count1, int count2)
        {
            double[][] result = new double[count1][];
            for (int i = 0; i < count1; i++)
                result[i] = new double[count2];
            return result;
        }
        //Make formal unit tests
        // Example Rank(new double[]{10,3,2,3,10}) => double[]{4.5,2.5,1,2.5,4.5}
        //           Rank(new double[]{0,0,1,0,1,1,1}) => double[]{2,2,5.5,2,5.5,5.5,5.5}
        public static double[] Rank(IEnumerable<double> scoreSequence)
        {
            //Give every score an index
            var scoreAndIndexSequence = scoreSequence.Select((score, index) => new { score, index }).ToList();

            //group and sort
            var groupAndSortQuery =
                from scoreAndIndex in scoreAndIndexSequence
                group scoreAndIndex by scoreAndIndex.score into g
                orderby g.Key
                select g.ToList();

            //Find the rank of each group
            double[] rankArray = new double[scoreAndIndexSequence.Count];
            int rank = 0;
            foreach (var groupAndSort in groupAndSortQuery) //can not be parallel
            {
                int rankFirst = rank + 1;
                int rankLast = rank + groupAndSort.Count;
                double averageRankForGroup = ((double)(rankFirst + rankLast)) / 2.0;
                rank = rankLast;

                foreach (var scoreAndIndex in groupAndSort)
                {
                    rankArray[scoreAndIndex.index] = averageRankForGroup;
                }
            }
            return rankArray;

        }


#if !SILVERLIGHT

        //Create unit test with this:
        //        var ints = Enumerable.Range(0,11).Select(i => (double) i).ToList();
        //Console.WriteLine(SpecialFunctions.PermuteExactly01Targets(ints, i => i, i => ((int)i % 2)).Count());

        static public IEnumerable<List<KeyValuePair<double, int>>> Permute01Targets<T>(IList<T> inputList, Func<T, double> scoreFunc, Func<T, int> zeroOneFunction, int sampleThreshold, int randomSeed = -34359393)
        {
            Helper.CheckCondition(inputList.Count > 0, "Must have some items in the list"); //Count just yeild break
            //find # of 0's and 1's
            var labelToCount =
                (from row in inputList
                 let zeroOne = zeroOneFunction(row)
                 group zeroOne by zeroOne into g
                 let entry = new { key = g.Key, count = g.Count() }
                 select entry).ToDictionary(keyAndCount => keyAndCount.key, keyAndCount => keyAndCount.count);

            Helper.CheckCondition(labelToCount.Keys.ToHashSet().IsSubsetOf(new int[] { 0, 1 }), "expect labels of 0 or 1");
            int zeroCount = labelToCount.GetValueOrDefault(0,/*insertIfMissing*/ false);
            int oneCount = labelToCount.GetValueOrDefault(1,/*insertIfMissing*/ false);
            Helper.CheckCondition(zeroCount + oneCount == inputList.Count, "Real assert: 0's and 1's miscounted");

            BigInteger exactPermutationCount = Factorial(zeroCount + oneCount) / (Factorial(zeroCount) * Factorial(oneCount));

            if (exactPermutationCount <= sampleThreshold)
            {

                //We think of the 0's sitting in a line zeroCount long.
                //We then insert 1's before, between, and/or after the 0's.
                //There are zeroCount+1 places to insert a 1. There are oneCount 1's to insert. So this is like putting oneCount
                // marbles into zeroCount+1 urns.
                //We start by inserting all the 1's in the first urn
                //We then move out of the first urn, into the second urn.
                //      If the first urn is empty, then we find the first nonempty urn, X
                //                move one marble from X to X+1 and move the rest back to 1.
                //The total # of permunations is (zeroCount+oneCount)!/(zeroCount! x oneZount!)

                int permutationIndex = 0;
                int[] urns = new int[zeroCount + 1];
                urns[0] = oneCount;
                while (true)
                {
                    var result = CreateResultFromUrns<T>(inputList, scoreFunc, zeroCount, oneCount, urns);
                    yield return result;
                    ++permutationIndex;

                    if (!TryToMoveMarbleToNextConfiguration(urns))
                    {
                        break; //leave the while
                    }
                }
                Helper.CheckCondition(permutationIndex == exactPermutationCount, "Real assert: PermuteExactly01Targets had wrong number of permutations");
            }
            else
            {
                Random random = new Random(randomSeed);
                for (int sampleIndex = 0; sampleIndex < sampleThreshold; ++sampleIndex)
                {
                    List<int> zeroOneList = Enumerable.Repeat(0, zeroCount).Concat(Enumerable.Repeat(1, oneCount)).Shuffle(random);

                    var result = new List<KeyValuePair<double, int>>(zeroCount + oneCount);
                    for (int i = 0; i < zeroOneList.Count; ++i)
                    {
                        var pairWithOne = new KeyValuePair<double, int>(scoreFunc(inputList[i]), zeroOneList[i]);
                        result.Add(pairWithOne);
                    }
                    yield return result;
                }
            }

        }

#endif

        private static bool TryToMoveMarbleToNextConfiguration(int[] urns)
        {
            if (urns[0] != 0)
            {
                --urns[0];
                ++urns[1];
            }
            else
            {
                int indexOfFirstUrnWithAMarble = 0;
                for (; urns[indexOfFirstUrnWithAMarble] == 0; ++indexOfFirstUrnWithAMarble)
                { //do nothing
                };
                if (indexOfFirstUrnWithAMarble == urns.Length - 1) //All the marbles are in the last urn
                {
                    return false;
                }
                ++urns[indexOfFirstUrnWithAMarble + 1];
                urns[0] = urns[indexOfFirstUrnWithAMarble] - 1;
                urns[indexOfFirstUrnWithAMarble] = 0;
            }
            return true;
        }

        private static List<KeyValuePair<double, int>> CreateResultFromUrns<T>(IList<T> inputList, Func<T, double> scoreFunc, int zeroCount, int oneCount, int[] urns)
        {
            var result = new List<KeyValuePair<double, int>>(zeroCount + oneCount);
            for (int urnIndex = 0; urnIndex < urns.Length; ++urnIndex)
            {
                for (int marbleIndex = 0; marbleIndex < urns[urnIndex]; ++marbleIndex)
                {
                    var pairWithOne = new KeyValuePair<double, int>(scoreFunc(inputList[result.Count]), 1);
                    result.Add(pairWithOne);
                }
                if (urnIndex != urns.Length - 1)
                {
                    var pairWithZero = new KeyValuePair<double, int>(scoreFunc(inputList[result.Count]), 0);
                    result.Add(pairWithZero);
                }
            }
            return result;
        }

        /// <summary>
        /// on the one case I tested, this was off by one, which presumably arises from using doubles of finite precision
        /// </summary>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static double LogFactorialNMOverFactorialNFactorialMApprox(int n, int m)
        {
            double logResult = 0;
            for (int i = (n + m); i > m; i--)
            {
                logResult += Math.Log(i);
            }
            logResult = logResult - LogFactorial(n);
            return logResult;
        }

        public static double LogFactorial(int n)
        {
            Helper.CheckCondition(n > 0, "n must be at least 0");
            double logFactorial = 0;
            for (int i = 2; i <= n; ++i)
            {
                logFactorial += Math.Log(i);
            }
            return logFactorial;
        }

#if !SILVERLIGHT
        public static BigInteger Factorial(int n)
        {
            Helper.CheckCondition(n > 0, "n must be at least 0");
            BigInteger factorial = 1;
            for (int i = 2; i <= n; ++i)
            {
                factorial *= i;
            }
            return factorial;
        }
#endif

        ////UnitTest: Helper.CheckCondition(Math.Abs(TwoByTwo.GetInstance(3, 5, 7, 9).ChiSquareTest - 0.769051389) < 0.000001, "Chi-square on two-by-two should pass unit test");
        //static public double LikelihoodRatioTestOnTwoByTwoCounts(int[,] counts)
        //{
        //    Helper.CheckCondition(counts.GetLength(0) == 2 && counts.GetLength(1) == 2);
        //    double llWithArc =
        //        NLogNByD(counts[0, 0], (counts[0, 0] + counts[0, 1])) +
        //        NLogNByD(counts[0, 1], (counts[0, 0] + counts[0, 1])) +
        //        NLogNByD(counts[1, 0], (counts[1, 0] + counts[1, 1])) +
        //        NLogNByD(counts[1, 1], (counts[1, 0] + counts[1, 1]));
        //    double llWithoutArc =
        //        NLogNByD(counts[0, 0] + counts[1, 0], counts[0, 0] + counts[1, 0] + counts[0, 1] + counts[1, 1]) +
        //        NLogNByD(counts[0, 1] + counts[1, 1], counts[0, 0] + counts[1, 0] + counts[0, 1] + counts[1, 1]);
        //    double deltaLL = llWithArc - llWithoutArc;
        //    double chiSquarePValue = SpecialFunctions.GetInstance().LogLikelihoodRatioTest(deltaLL, 1);
        //    return chiSquarePValue;
        //}

        ////UnitTest: Helper.CheckCondition(Math.Abs(TwoByTwo.GetInstance(3, 5, 7, 9).ChiSquareTest - 0.769051389) < 0.000001, "Chi-square on two-by-two should pass unit test");
        //static public double LikelihoodRatioTestOnThreeByTwoCounts(int[,] counts)
        //{
        //    double deltaLL = DeltaLLOnThreeByTwoCounts(counts);
        //    double chiSquarePValue = SpecialFunctions.GetInstance().LogLikelihoodRatioTest(deltaLL, 2);
        //    return chiSquarePValue;
        //}

        //public static double DeltaLLOnThreeByTwoCounts(int[,] counts)
        //{
        //    Helper.CheckCondition(counts.GetLength(0) == 3 && counts.GetLength(1) == 2);
        //    double llWithArc =
        //        NLogNByD(counts[0, 0], (counts[0, 0] + counts[0, 1])) +
        //        NLogNByD(counts[0, 1], (counts[0, 0] + counts[0, 1])) +
        //        NLogNByD(counts[1, 0], (counts[1, 0] + counts[1, 1])) +
        //        NLogNByD(counts[1, 1], (counts[1, 0] + counts[1, 1])) +
        //        NLogNByD(counts[2, 0], (counts[2, 0] + counts[2, 1])) +
        //        NLogNByD(counts[2, 1], (counts[2, 0] + counts[2, 1]));
        //    int total = counts[0, 0] + counts[1, 0] + counts[2, 0] + counts[0, 1] + counts[1, 1] + counts[2, 1];
        //    double llWithoutArc =
        //        NLogNByD(counts[0, 0] + counts[1, 0] + counts[2, 0], total) +
        //        NLogNByD(counts[0, 1] + counts[1, 1] + counts[2, 1], total);
        //    double deltaLL = llWithArc - llWithoutArc;

        //    return deltaLL;
        //}

        public static double DeltaLLOnNByMCounts(int[,] counts)
        {
            double llWithArc = 0;
            int total = 0;
            for (int rowIndex = 0; rowIndex < counts.GetLength(0); ++rowIndex)
            {
                int rowSum = 0;
                for (int colIndex = 0; colIndex < counts.GetLength(1); ++colIndex)
                {
                    rowSum += counts[rowIndex, colIndex];
                }
                total += rowSum;

                for (int colIndex = 0; colIndex < counts.GetLength(1); ++colIndex)
                {
                    llWithArc += NLogNByD(counts[rowIndex, colIndex], rowSum);
                }
            }

            double llWithoutArc = 0;
            for (int colIndex = 0; colIndex < counts.GetLength(1); ++colIndex)
            {
                int colSum = 0;
                for (int rowIndex = 0; rowIndex < counts.GetLength(0); ++rowIndex)
                {
                    colSum += counts[rowIndex, colIndex];
                }
                llWithoutArc += NLogNByD(colSum, total);
            }

            return llWithArc - llWithoutArc;

        }


        public static double LikelihoodRatioTestOnNByMCounts(int[,] counts)
        {
            double deltaLL = DeltaLLOnNByMCounts(counts);
            int dof = DegreeOfFreedomForLRT(counts);
            double chiSquarePValue = SpecialFunctions.GetInstance().LogLikelihoodRatioTest(deltaLL, dof);
            return chiSquarePValue;
        }

#if SILVERLIGHT
        private double LogLikelihoodRatioTest(double deltaLL, int dof)
        {
            throw new NotImplementedException();
        }
#endif

        public static int DegreeOfFreedomForLRT(int[,] counts)
        {
            int dof = counts.GetLength(0) * counts.GetLength(1) - (counts.GetLength(0) + counts.GetLength(1) - 1);
            return dof;
        }



        static private double NLogNByD(int n, int d)
        {
            if (n == 0)
            {
                return 0;
            }
            return n * Math.Log((double)n / d);
        }


        //Concatinates a single item to the front of a list. Named after the function in Lisp, http://en.wikipedia.org/wiki/Cons
        public static IEnumerable<T> Cons<T>(T item, IEnumerable<T> sequence)
        {
            return new T[] { item }.Concat(sequence);
        }

        //public static Tuple<double, int> MeanAndCount(IEnumerable<double> numberSequence, ParallelOptions parallelOptions)
        //{
        //    int count = 0;
        //    double total = 0;
        //    Parallel.ForEach(numberSequence, parallelOptions, number =>
        //        {
        //            ++count; //known to be thread-safe
        //            total += number; // known to be thread-safe
        //        });
        //    double mean = total / (double)count;
        //    return Tuple.Create(mean, count);
        //}

        /// <summary>
        /// computes the "softmax" function: log sum_i exp x_i
        /// </summary>
        /// <param name="inputs">A list of numbers to softmax. These are assumed to be in logspace already.</param>
        /// <returns>the softmax of the numbers</returns>
        /// <remarks>Taken from TMSN Softmax function.
        /// May have slightly lower roundoff error if inputs are sorted, smallest first</remarks>
        public static double SoftMax(IEnumerable<double> inputs)
        {
            const double LOG_TOLERANCE = 30.0;
            int i = 0;
            int maxIdx = 0;
            double max = Double.NegativeInfinity;
            foreach (double val in inputs)
            {
                if (val > max)
                {
                    maxIdx = i;
                    max = val;
                }
                i++;
            }

            int leng = i;
            if (double.IsNegativeInfinity(max))
            {
                return double.NegativeInfinity;
            }
            else if (leng == 1)
            {
                return max;
            }

            //else if (leng == 2) {
            //  return SoftMax(inputs[0], inputs[1]);
            //}

            double intermediate = 0.0;
            double cutoff = max - LOG_TOLERANCE;

            i = 0;
            foreach (double val in inputs)
            {
                if (i++ == maxIdx) continue;

                if (val > cutoff)
                {
                    intermediate += Math.Exp(val - max);
                }
            }

            if (intermediate > 0.0)
            {
                return max + Math.Log(1.0 + intermediate);
            }
            else
            {
                return max;
            }
        }

        public static double DotProduct(IList<double> x, IList<double> y)
        {
            Helper.CheckCondition<ArgumentException>(x.Count == y.Count, "Vectors are not the same length.");
            return Enumerable.Range(0, x.Count).Sum(i => x[i] * y[i]);
        }

        /// <summary>
        /// Multiplies x by c and stores the result into y.
        /// </summary>
        public static void ScaleXIntoY(IList<double> x, double c, IList<double> y)
        {
            for (int i = 0; i < x.Count; i++)
                y[i] = x[i] * c;
        }

        //public void test()
        //{
        //    IEnumerable<int> s0 = new int[] {};
        //    IEnumerable<int> s1 = new []{1,3,4,4,5,34};


        //    IEnumerable<int> s0b;
        //    if (!HasFirst(s0, out s0b))
        //    {
        //    }
        //}

        public static bool HasFirst<T>(IEnumerable<T> sequenceIn, out IEnumerable<T> sequenceOut)
        {
            IEnumerator<T> enumerator = sequenceIn.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                sequenceOut = new T[] { };
                return false;
            }

            T t = enumerator.Current;
            sequenceOut = (new T[] { t }).Concat(GetEnumerable(enumerator));
            return true;
        }

        static private IEnumerable<T> GetEnumerable<T>(IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }

        public static string ResolveValueToString(object value)
        {
            var valAsIe = value as System.Collections.IEnumerable;
            if (valAsIe != null && !(value is string))
                return valAsIe.StringJoin(",");
            else
                return value.ToString();

            //Type type = value.GetType();
            //if (type.IsGenericType && type.FindInterfaces(Module.FilterTypeNameIgnoreCase, "ICollection*").Length > 0)
            //{
            //    Type extType = typeof(IEnumerableExtensions);
            //    Type genericType = type.GetGenericArguments()[0];

            //    Type[] argTypes = new Type[] { type, typeof(string) };

            //    MethodInfo stringJoin = extType.GetMethod("StringJoin", argTypes);
            //    MethodInfo genericStringJoin = stringJoin.MakeGenericMethod(genericType);

            //    object[] args = new object[] { value, "," };
            //    string result = (string)genericStringJoin.Invoke(null, args);
            //    return result;
            //}
            //else
            //    return value.ToString();

        }

#if !SILVERLIGHT
        /// <summary>
        /// Returns any of the n! permutations of a sequence by index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputSequence"></param>
        /// <param name="permutationIndex"></param>
        /// <returns></returns>
        public static List<T> PermutationByIndex<T>(IEnumerable<T> inputSequence, BigInteger permutationIndex)
        {
            Helper.CheckCondition(0 <= permutationIndex, "Expect permutationIndex to be at least 0");

            List<T> outputList = inputSequence.ToList();
            for (int listIndex = 0; listIndex < outputList.Count - 1; ++listIndex) //The -1 is one is because we don't have to do the last item
            {
                int choiceCount = outputList.Count - listIndex;
                int choiceIndex = (int)(permutationIndex % choiceCount);
                permutationIndex = permutationIndex / choiceCount;
                outputList.Swap(listIndex, listIndex + choiceIndex);
            }
            Helper.CheckCondition(permutationIndex == 0, "Expect the permutationIndex to be strictly less than Factorial(inputSequence.Count())");
            return outputList;
        }
#endif

        public static double PearsonRWithMissing(ICollection<double> iCollection, IEnumerable<double> iEnumerable)
        {
            throw new NotImplementedException("11062010");
        }

        public static Tuple<double, double> MinAndMaxWithNaN(IList<double> listWithNaN)
        {
            double min = double.MaxValue;
            double max = double.MinValue;

            foreach (double value in listWithNaN)
            {
                if (!double.IsNaN(value))
                {
                    min = Math.Min(min, value);
                    max = Math.Max(max, value);
                }
            }
            Helper.CheckCondition(min <= max, "Expect list to have some non-missing values");

            return Tuple.Create(min, max);
        }


        public static double MeanWithNaN(IList<double> listWithNaN)
        {
            var sumAndCount = SumAndCount(listWithNaN);
            return sumAndCount.Item1 / (double)sumAndCount.Item2;

        }

        public static Tuple<double, int> SumAndCount(IList<double> listWithNaN)
        {

            double sum = 0.0;
            int count = 0;

            foreach (double value in listWithNaN)
            {
                if (!double.IsNaN(value))
                {
                    sum += value;
                    ++count;
                }
            }
            Helper.CheckCondition(count > 0, "Expect list to have some non-missing values");
            return Tuple.Create(sum, count);
        }


        public static Tuple<double, double> MeanAndSampleStandardDeviation(IList<double> listWithNaN)
        {
            int n = 0;
            double sum = 0;
            double ss = 0;
            foreach (double d in listWithNaN)
            {
                if (!double.IsNaN(d))
                {
                    ++n;
                    sum += d;
                    ss += d * d;
                }
            }
            Helper.CheckCondition(n > 0, "Expect list to have some non-missing values");

            double mean = sum / (double)n;
            double variance = (ss - (sum * sum / (double)n)) / (double)(n - 1);
            return Tuple.Create(mean, Math.Sqrt(variance));
        }


    }
}
