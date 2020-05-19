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
using ShoNS.Array;
using ShoNS.MathFunc;
//using System.Windows.Forms;
using System.Diagnostics;
//using ShoNS.IO;
//using Sparse2Dense;
using MBT.Escience.Sho;
//using ShoNS.Viz;
using System.Collections;
using MBT.Escience;
using MBT.Escience.Matrix;
using Bio.Util;
using Bio.Matrix;
using System.IO;
using Eigenstrat;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Text.RegularExpressions;



namespace GenerateKernels
{
    public enum GeneSetFileType
    {
        GMT_SET_TO_GENE,
        SPARSE_SET_TO_TRANSCRIPTID
    }
    public enum INPUT_ENCODING_TYPE
    {
        ZeroOne = 1,
        ZeroOneTwo = 2,
        RealValued = 10,
        Null = -200
    }

    public class LoadData
    {
        public static bool _QUIET = false;

        public static void WriteSvdToFile(List<string> colKeys, DoubleArray svdU, DoubleArray svdDiag, string svdUFile, string svdDiagFile,bool useGzip)
        {
            int numEigenvectorsInU = svdU.size1;
            IEnumerable<string> colKeysShort = colKeys.SubSequence(0, numEigenvectorsInU);
            (new ShoMatrix(svdU, colKeys, colKeysShort, double.NaN)).WriteDense(svdUFile);
            (new ShoMatrix(svdDiag, new List<string>() { "svdDiag" }, colKeysShort, double.NaN)).WriteDense(svdDiagFile);
            Console.WriteLine("svdUFile=" + svdUFile);
            Console.WriteLine("svdDiagFile=" + svdDiagFile);

            if (useGzip)
            {
                SpecialFunctions.GzipFile(svdUFile,true);                                
            }
            
            Console.WriteLine("wrote svd to file and exiting");
        }

        /// <summary>
        /// eventually Carl will check this, but for now, just cut and paste what he sent me by email
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Matrix<string, string, double> LoadParentMatrix(string fileName, bool Verbose, bool LeaveOnDiskIfPossible, ParallelOptions parallelOptions)
        {
            var MissingValueForParse = double.NaN;
            var CharToTValConverter = Bio.Util.ValueConverter.CharToDouble;
            string CreateRowKeysFileOrNull = null;//set it to the name it should write to output--cannot have folder or directory, will automatically get handy

            if (Verbose)
            {
                Console.WriteLine("Loading " + fileName);
            }

            if (LeaveOnDiskIfPossible)
            {
                try
                {
                    RowKeysAnsi predictorCharOriginal0 = RowKeysAnsi.GetInstanceFromRowKeysAnsi(fileName, parallelOptions);

                    Matrix<string, string, double> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(CharToTValConverter, MissingValueForParse);
                    //Matrix<string,string,TValue> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(Bio.Util.ValueConverter.CharToDouble, double.NaN);

                    Helper.CheckCondition(null == CreateRowKeysFileOrNull, "Already reading from a RowKeyAnsi file, so can't create one.");
                    return predictorCharOriginal;
                }
                catch (MatrixFormatException)
                {//ignore
                }


                try
                {
                    RowKeysAnsi predictorCharOriginal0 = RowKeysAnsi.GetInstanceFromDenseAnsi(fileName, parallelOptions);


                    if (null != CreateRowKeysFileOrNull)
                    {
                        predictorCharOriginal0.WriteRowKeys(CreateRowKeysFileOrNull);
                    }

                    Matrix<string, string, double> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(CharToTValConverter, MissingValueForParse);
                    //Matrix<string, string, TValue> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(Bio.Util.ValueConverter.CharToDouble, double.NaN);
                    return predictorCharOriginal;
                }
                catch (MatrixFormatException)
                {//ignore
                }

                //Carl says comment this out because it is for PaddedDouble which I won't be using
                //try
                //{
                //    RowKeysPaddedDouble predictorCharOriginal0 = RowKeysPaddedDouble.GetInstanceFromRowKeys(fileName, parallelOptions);

                //    Matrix<string, string, double> predictorCharOriginal = predictorCharOriginal0.ConvertValueView<string, string, double, char>(Bio.Util.ValueConverter.CharToDouble, '?');//MissingValueForParse);
                //        //DoubleToTValConverter, MissingValueForParse);
                //    //Matrix<string,string,TValue> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(Bio.Util.ValueConverter.CharToDouble, double.NaN);

                //    Helper.CheckCondition(null == CreateRowKeysFileOrNull, "Already reading from a RowKeyAnsi file, so can't create one.");
                //    return predictorCharOriginal;
                //}
                //catch (MatrixFormatException)
                //{//ignore
                //}


                //try
                //{
                //    RowKeysPaddedDouble predictorCharOriginal0 = RowKeysPaddedDouble.GetInstanceFromPaddedDouble(fileName, parallelOptions);


                //    if (null != CreateRowKeysFileOrNull)
                //    {
                //        predictorCharOriginal0.WriteRowKeys(CreateRowKeysFileOrNull);
                //    }

                //    Matrix<string, string, double> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(DoubleToTValConverter, MissingValueForParse);
                //    //Matrix<string, string, TValue> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(Bio.Util.ValueConverter.CharToDouble, double.NaN);
                //    return predictorCharOriginal;
                //}
                //catch (MatrixFormatException)
                //{//ignore
                //}
            }

            MatrixFactory<string, string, double> MatrixFactory = MatrixFactory<string, string, double>.GetInstance();
            Matrix<string, string, double> predictorCharOriginalX = MatrixFactory.Parse(fileName, MissingValueForParse, parallelOptions);
            Helper.CheckCondition(null == CreateRowKeysFileOrNull, "Not creating from a DenseAnsi file, so can't create a RowKeyAnsi file.");
            return predictorCharOriginalX;
        }




        public static List<SVD> getGroupKernelsFromFullKernel(Dictionary<int, List<int>> members, ShoMatrix kernel)
        {
            int numGroups = members.Count;
            List<SVD> groupSVD;
            groupSVD = new List<SVD>();

            for (int g = 0; g < numGroups; g++)
            {
                int G = (int)members[g].Count;//groupSizes[g];
                DoubleArray K = new DoubleArray(G, G);

                for (int g0 = 0; g0 < G; g0++)
                {
                    int n0 = members[g][g0];
                    K[g0, g0] = kernel[n0, n0];
                    for (int g1 = g0 + 1; g1 < G; g1++)
                    {
                        int n1 = members[g][g1];
                        K[g0, g1] = kernel[n0, n1];
                        K[g1, g0] = K[g0, g1];//symmetry
                    }
                }

                groupSVD.Add(new SVD(K));
            }
            return groupSVD;
        }


        public static int[] CreateGroupsFromGroupFile(string groupsFile, Matrix<string, string, double> snpMatrix, out Dictionary<int, List<int>> members, ParallelOptions parallelOptions)
        {
            List<string> garb;
            return CreateGroupsFromGroupFile(groupsFile, snpMatrix, out members, out garb, parallelOptions);
        }

        public static int[] CreateGroupsFromGroupFile(string groupsFile, Matrix<string, string, double> snpMatrix, out Dictionary<int, List<int>> members, out List<string> groupNames, ParallelOptions parallelOptions)
        {
            Matrix<string, string, string> groupMatrix = LoadData.CreateStringMatrixFromMatrixFile(groupsFile, parallelOptions);
            Helper.CheckCondition(groupMatrix.ColKeys.SequenceEqual(snpMatrix.ColKeys), "group matrix cids do not match up with the rest of the data");
            Helper.CheckCondition(groupMatrix.RowCount == 1, "groupFile should contain just one row feature which is the group label of each individuals, as an arbitrary string");
            int N = groupMatrix.ColCount;
            groupNames = groupMatrix.Values.Distinct().ToList();
            int[] groups = new int[N];
            for (int c = 0; c < N; c++)
            {
                groups[c] = groupNames.FindIndex(elt => elt == groupMatrix[0, c]);
            }
            members = new Dictionary<int, List<int>>(); // each element of members is a list of nodes within this group.
            for (int i = 0; i < groups.Length; i++)
            {
                members.GetValueOrDefault(groups[i]).Add(i);
            }
            return groups;
        }

        public static ShoMatrix ComputeAndWriteIBSKernelFromMarkersAfterMeanImputation(string rawSnpInputFile, string outputFile, ParallelOptions parallelOptions)
        {
            Matrix<string, string, double> snpData = LoadData.CreateMatrixFromMatrixFile(rawSnpInputFile, parallelOptions);
            return ComputeAndWriteIBSKernelFromMarkersAfterMeanImputation(snpData, outputFile, parallelOptions);
        }

        public static ShoMatrix ComputeIBSKernelIgnoringMissingMarkers(ShoMatrix snpData, string outputFile, ParallelOptions parallelOptions)
        {
            LoadData.CheckSnpDataEncoding(INPUT_ENCODING_TYPE.ZeroOneTwo, snpData);
            DoubleArray snps = snpData.DoubleArray;
            int M = snpData.RowCount;
            int N = snpData.ColCount;
            var counterWithMessages = new CounterWithMessages("IbsKernelIgnoringMissingMarkers", 100, N);
            DoubleArray ibsKernel = new DoubleArray(N, N);
            Parallel.For(0, N, parallelOptions, n0 =>
            //for (int n0 = 0; n0 < N; n0++)
            {
                counterWithMessages.Increment();
                DoubleArray s0 = snps.GetCol(n0);
                for (int n1 = n0; n1 < N; n1++)
                {
                    DoubleArray s1 = snps.GetCol(n1);
                    //IntArray nonMissingInd = (s0 + s1).FindRowsWithNoNans();
                    //double thisIbs = GetIbsPairL1Score(s0.GetRows(nonMissingInd), s1.GetRows(nonMissingInd));
                    double thisIbs = GetIbsPairL1Score(s0, s1);
                    ibsKernel[n0, n1] = thisIbs;
                    ibsKernel[n1, n0] = thisIbs;
                }
            }
            );
            counterWithMessages.Finished();
            return new ShoMatrix(ibsKernel, snpData.ColKeys, snpData.ColKeys, double.NaN);
        }

        private static double GetIbsPairL1Score(DoubleArray d0, DoubleArray d1)
        {
            int numNonMissingEntries = 0;
            double ibsScore = 0;
            for (int i = 0; i < d0.Length; i++)
            {
                if (!double.IsNaN(d0[i]) && !double.IsNaN(d1[i]))
                {
                    numNonMissingEntries++;
                    ibsScore += Math.Abs(d0[i] - d1[i]);
                }
            }
            if (numNonMissingEntries < d0.Count)
            {
                Console.WriteLine("found missing data");
            }
            ibsScore = 2.0 - ibsScore / (double)numNonMissingEntries;
            ibsScore = ibsScore / 2.0;
            Helper.CheckCondition(ibsScore <= 1 && ibsScore >= 0);
            return ibsScore;
        }

        /// <summary>
        /// from Emma paper
        /// </summary>
        /// <param name="d0"></param>
        /// <param name="d1"></param>
        /// <returns></returns>
        private static double GetIbsPairOneHotAllowMissingScore(DoubleArray d0, DoubleArray d1, int numAlleles)
        {
            double ibsScore = 0;
            for (int i = 0; i < d0.Length; i++)
            {

            }
            return ibsScore;
        }


        public static ShoMatrix ComputeAndWriteIBSKernelFromMarkersAfterMeanImputation(Matrix<string, string, double> snpData, string outputFile, ParallelOptions parallelOptions)
        {
            ShoMatrix kernelMatrix = ComputeIBSKernelFromMarkersAfterMeanImputation(snpData, outputFile, parallelOptions);
            Console.Write("writing IBS kernel to file " + outputFile + "...");
            kernelMatrix.WriteDense(outputFile);//WriteDense(outputFile, .ColKeys.ToArray(), snpDataToUseForKernel.ColKeys.ToArray(), kernelMatrix);
            Console.WriteLine("...done");
            return kernelMatrix;
        }


        //moved from Ying's project to here for general use
        public static ShoMatrix ComputeIBSKernelFromMarkersAfterMeanImputation(Matrix<string, string, double> snpData, string outputFile, ParallelOptions parallelOptions)
        {


            //----------------------------------------------
            //throw new Exception("obsolete because not a smart way to do this..");
            //string workingDirectory = outputFile + ".dir";
            //Bio.Util.FileUtils.CreateDirectoryForFileIfNeeded(workingDirectory + @"\tmp.txt");

            //double[] averageSNP = FindMeanForEachSnp(snpData, parallelOptions);

            //CreateBinaryFileForEachCid(snpData, parallelOptions, averageSNP, workingDirectory);

            //DoubleArray kernelMatrix = CreateIBSMatrixObsolete(snpData, parallelOptions, workingDirectory);
            //--------------------------
            DoubleArray kernelMatrix = CreateIBSMatrixFromMeanImputedData(snpData, parallelOptions);

            Console.WriteLine("done");
            //WriteDense(outputFile, snpDataToUseForKernel.ColKeys.ToArray(), snpDataToUseForKernel.ColKeys.ToArray(), kernelMatrix);
            ShoMatrix resultMatrix = new ShoMatrix(kernelMatrix, snpData.ColKeys.ToList(), snpData.ColKeys.ToList(), double.NaN);
            return resultMatrix;
        }



        private static DoubleArray CreateIBSMatrixFromMeanImputedData(Matrix<string, string, double> snpData, ParallelOptions parallelOptions)
        {

            bool useMean = true;
            DoubleArray snpArray = snpData.AsShoMatrix().DoubleArray.ImputeNanValuesWithRowMedian(parallelOptions, useMean);
            int N = snpArray.size1;
            var counterWithMessages = new CounterWithMessages("IbsKernelUsingMeanImputation", 100, N);

            DoubleArray ibs = ShoUtils.DoubleArrayNaNs(N, N);
            Parallel.For(0, N, parallelOptions, n0 =>
                {
                    counterWithMessages.Increment();
                    for (int n1 = n0; n1 < N; n1++)
                    {
                        ibs[n0, n1] = ComputeIbsScoreBetweenTwoPeople(snpArray.GetCol(n0), snpArray.GetCol(n1));
                        ibs[n1, n0] = ibs[n0, n1];
                    }
                }
        );
            counterWithMessages.Finished();

            return ibs;
        }

        private static double ComputeIbsScoreBetweenTwoPeople(DoubleArray p1, DoubleArray p2)
        {
            double score = 0;
            int S = p1.Length;
            Helper.CheckCondition(S == p2.Length, "vectors are not of the same length");
            for (int s = 0; s < S; s++)
            {
                score = score + 2 - Math.Abs(p1[s] - p2[s]);
            }
            //score = score * 1.0 / S / 2;
            score = score / (2 * S);
            return score;
        }

        private static DoubleArray CreateIBSMatrixObsolete(Matrix<string, string, double> snpData, ParallelOptions parallelOptions, string workingDirectory)
        {
            throw new Exception("this is a really bad way to do things... making obsolete");

            Helper.CheckCondition(!snpData.IsMissingSome(), "cannot have missing values");
            DoubleArray kernelMatrix = new DoubleArray(snpData.ColCount, snpData.ColCount);

            CounterWithMessages counterWithMessages3 = new CounterWithMessages("Computing IBS values", null, (snpData.RowCount * snpData.RowCount + snpData.RowCount) / 2);

            var binaryFormatter = new BinaryFormatter();

            Parallel.For(0, kernelMatrix.size0, parallelOptions, i =>
            //for (int i = 0; i < kernelMatrix.size0; i++)
            {
                counterWithMessages3.Increment();

                double[] cidArrayI = ReadCidVector(workingDirectory, binaryFormatter, i);


                //Console.Write(string.Format("\r{0}\t", i));
                for (int j = i; j < kernelMatrix.size0; j++)
                {
                    counterWithMessages3.Increment();

                    double[] cidArrayJ = ReadCidVector(workingDirectory, binaryFormatter, j);

                    //int[] tempi = { i };
                    //int[] tempj = { j };
                    double score = 0;
                    for (int k = 0; k < snpData.RowCount; k++)
                    {
                        score = score + 2 - Math.Abs(cidArrayI[k] - cidArrayJ[k]);
                    }

                    score = score * 1.0 / snpData.RowCount / 2;
                    kernelMatrix[i, j] = score;
                    kernelMatrix[j, i] = score;
                }
            }
            );
            counterWithMessages3.Finished();
            Console.WriteLine("");
            return kernelMatrix;
        }

        private static double[] ReadCidVector(string workingDirectory, BinaryFormatter binaryFormatter, int i)
        {
            throw new Exception("this is a really bad way to do things... making obsolete");
            string cidFileI = CidFileName(workingDirectory, i);
            double[] cidArrayI;
            using (Stream stream = new FileStream(cidFileI, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                cidArrayI = (double[])binaryFormatter.Deserialize(stream);
            }

            return cidArrayI;
        }

        private static double[] FindMeanForEachSnp(Matrix<string, string, double> snpData, ParallelOptions parallelOptions)
        {
            throw new Exception("this is a really bad way to do things... making obsolete");
            //Console.WriteLine("computing mean of each SNP (for " + markers.size0 + " SNPs)");
            CounterWithMessages counterWithMessages = new CounterWithMessages("computing mean of each SNP", null, snpData.RowCount);
            //First sweep, compute the mean of each snp based on available data.
            double[] averageSNP = new double[snpData.RowCount];
            //int[] counts = new int[markers.size0];//number of valid snp values for each snp
            //for (int i = 0; i < markers.size0; i++)
            Parallel.For(0, snpData.RowCount, parallelOptions, i =>
            {
                counterWithMessages.Increment();

                //Console.Write(string.Format("\r{0}\t", i));
                //averageSNP[i] = 0;
                //counts[i] = 0;
                double averageSnpTmp = 0;
                int counts = 0;
                for (int j = 0; j < snpData.ColCount; j++)
                {
                    double value;
                    if (snpData.TryGetValue(i, j, out value))
                    {
                        //averageSNP[i] = averageSNP[i] + markers[i, j];
                        averageSnpTmp = averageSnpTmp + value;
                        //counts[i] = counts[i] + 1;
                        counts = counts + 1;
                    }
                }
                //averageSNP[i] = averageSNP[i] / counts;
                averageSNP[i] = averageSnpTmp / counts;
            }
            );
            counterWithMessages.Finished();
            Console.WriteLine();
            return averageSNP;
        }

        private static void CreateBinaryFileForEachCid(Matrix<string, string, double> snpData, ParallelOptions parallelOptions, double[] averageSNP, string workingDirectory)
        {
            //Write a binary temp file for each column
            CounterWithMessages counterWithMessages2 = new CounterWithMessages("writing out binary file for each cid", 1, snpData.ColCount);
            var binaryFormatter = new BinaryFormatter();
            Parallel.For(0, snpData.ColCount, parallelOptions, j =>
            {
                counterWithMessages2.Increment();

                double[] columnValueArray = new double[snpData.RowCount];
                for (int i = 0; i < snpData.RowCount; i++)
                {
                    if (!snpData.TryGetValue(i, j, out columnValueArray[i]))
                    {
                        columnValueArray[i] = averageSNP[i];
                    }
                }
                string cidFile = CidFileName(workingDirectory, j);
                using (Stream stream = new FileStream(cidFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    binaryFormatter.Serialize(stream, columnValueArray);
                }
            });
            counterWithMessages2.Finished();
            Console.WriteLine();
        }

        private static string CidFileName(string workingDirectory, int j)
        {
            string cidFile = workingDirectory + @"\" + j.ToString() + ".binary";
            return cidFile;
        }

        public static ShoMatrix CreateAndWriteCovarianceKernelAfterScalingCenteringGenes(ShoMatrix expDat, string kernelOutFile, double fudgeFac, bool useAshNorm, ParallelOptions parallelOptions)
        {
            return CreateAndWriteCovarianceKernelAfterScalingCenteringGenes(expDat, kernelOutFile, fudgeFac, useAshNorm, false, parallelOptions);
        }
        public static ShoMatrix CreateAndWriteCovarianceKernelAfterScalingCenteringGenes(ShoMatrix expDat, string kernelOutFile, double fudgeFac, bool useAshNorm, bool isOkToDestroy, ParallelOptions parallelOptions)
        {
            Console.WriteLine("centering and scaling each gene before computing kernel");
            DoubleArray meanPerGene = expDat.DoubleArray.Mean(DimOp.EachRow);
            DoubleArray stdPerGene = expDat.DoubleArray.Std(DimOp.EachRow);
            DoubleArray newExpDat = expDat.DoubleArray;
            Parallel.For(0, expDat.RowCount, parallelOptions, g =>
            {
                DoubleArray newRow = (newExpDat.GetRow(g) - meanPerGene[g]) / stdPerGene[g];
                newExpDat.SetRow(g, newRow);
            }
            );
            expDat.DoubleArray = newExpDat;

            meanPerGene = expDat.DoubleArray.Mean(DimOp.EachRow);
            stdPerGene = expDat.DoubleArray.Std(DimOp.EachRow);

            //double fudgeFac = 1e-5;
            return ComputeAndWriteExpressionKernel(expDat, kernelOutFile, fudgeFac, useAshNorm, parallelOptions);
        }
        public static ShoMatrix ComputeAndWriteExpressionKernel(ShoMatrix expressionMatrix, string kernelOutName, double diagFudgeFac, bool useAshNorm, ParallelOptions parallelOptions)
        {
            return ComputeAndWriteExpressionKernel(expressionMatrix, kernelOutName, diagFudgeFac, useAshNorm, false, parallelOptions);
        }

        public static ShoMatrix ComputeAndWriteExpressionKernel(ShoMatrix expressionMatrix, string kernelOutName, double diagFudgeFac, bool useAshNorm, bool isOkToDestroy, ParallelOptions parallelOptions)
        {
            Console.WriteLine("computing and writing kernel");

            ShoMatrix K = LoadData.ComputeCovariance(expressionMatrix, parallelOptions, isOkToDestroy).AsShoMatrix();
            //coerce it to be pos. def, if it is not
            K.DoubleArray = K.DoubleArray.CoercePositiveDefiniteWithAddDiagonal(diagFudgeFac);

            if (useAshNorm)
            {
                Console.WriteLine("using Ashish kernel normalization");
                K.DoubleArray = K.DoubleArray.NormalizeSpecial();
            }

            Cholesky tmp = K.DoubleArray.TryCholesky();
            if (tmp == null) throw new Exception("kernel is not pos. def.");

            //kernelOutName = kernelOutName.Replace(".cov.txt", "F" + diagFudgeFac + ".cov.txt");
            Console.WriteLine("writing to file: " + kernelOutName);
            K.WriteDense(kernelOutName);

            Console.WriteLine("done");
            return K;
        }


        public static void CreateAndWriteCovarianceKernelAfterScalingCenteringGenes(string expDatFile, string kernelOutFile, double fudge, bool useAshNorm, ParallelOptions parallelOptions)
        {
            ShoMatrix expDat = LoadData.CreateMatrixFromMatrixFile(expDatFile, parallelOptions).ToShoMatrix();
            CreateAndWriteCovarianceKernelAfterScalingCenteringGenes(expDat, kernelOutFile, fudge, useAshNorm, parallelOptions);
        }

        public static Dictionary<string, string> CreateStringStringDictFromOneColumnMatrix(string filename, bool caseInsensitive)
        {
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = 1;
            return CreateStringStringDictFromOneColumnMatrix(filename, parallelOptions, caseInsensitive);
        }

        public static Dictionary<string, string> CreateStringStringDictFromOneColumnMatrix(string filename, ParallelOptions parallelOptions, bool caseInsensitive)
        {
            Matrix<string, string, string> oneColMatrix = LoadData.CreateStringMatrixFromMatrixFile(filename, parallelOptions);
            Helper.CheckCondition(oneColMatrix.ColCount == 1);
            Dictionary<string, string> myDict;
            if (caseInsensitive)
            {
                myDict = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            }
            else
            {
                myDict = new Dictionary<string, string>();
            }
            string colKey = oneColMatrix.ColKeys.First();
            foreach (string key in oneColMatrix.RowKeys)
            {
                string thisVal;
                bool hasVal = oneColMatrix.TryGetValue(key, colKey, out thisVal);
                if (hasVal) myDict.Add(key, thisVal);
            }
            return myDict;
        }

        public static Dictionary<string, string> ParseSnpNameToGeneNameCollapsingToOneWithPrecedenceForNonEmptyEntrezId(string snpToGeneFile, int nameColZeroBased,
                                                                                                                        int geneColZeroBased, int entrezIdColZeroBased)
        {
            Dictionary<string, string> snpNameToGene = new Dictionary<string, string>();
            Dictionary<string, string> snpNameToEntrezId = new Dictionary<string, string>();

            using (TextReader textReader = File.OpenText(snpToGeneFile))
            {
                string header = textReader.ReadLine();
                int numCols = header.Split('\t').Count();

                string line = null;
                while (null != (line = textReader.ReadLine()))
                {
                    string[] lineItems = line.Split('\t');
                    string snpName = lineItems[nameColZeroBased];
                    string geneName = lineItems[geneColZeroBased];
                    //some rs#s map to more than one gene. rather than try to deal with this, 
                    //just always take the first one, unless a previous one had an empty entrez id in which case it's less likely to be the real one
                    string entrezId = lineItems[entrezIdColZeroBased];
                    string entrezIdStored = snpNameToEntrezId.GetValueOrDefault(snpName, "");
                    if (entrezIdStored == "")
                    {

                        snpNameToGene[snpName] = geneName;
                        snpNameToEntrezId[snpName] = entrezId;
                    }
                }
            }
            return snpNameToGene;
        }

        public static Dictionary<string, List<string>> ParseGeneSetFiles(IList<string> setFileList, ParallelOptions parallelOptions)
        {
            List<GeneSetFileType> setFileType = new List<GeneSetFileType>();
            Dictionary<string, List<string>> setToGene = new Dictionary<string, List<string>>();
            foreach (string setFile in setFileList)
            {
                Dictionary<string, List<string>> setToGeneListTmp, geneToSetListTmp;
                if (new FileInfo(setFile).Extension == ".gmt")
                {
                    LoadData.ParseGeneSetGmtFile(setFile, out setToGeneListTmp, out geneToSetListTmp, true);
                    setFileType.Add(GeneSetFileType.GMT_SET_TO_GENE);
                }
                else
                {
                    throw new Exception("GeneSetEnrichment.exe only works for GMT files now, need a few minor tweaks");
                    LoadData.ParseSparseGeneSetToTranscriptIdFile(setFile, out setToGeneListTmp, parallelOptions);
                    setFileType.Add(GeneSetFileType.SPARSE_SET_TO_TRANSCRIPTID);
                }
                setToGene = setToGene.Concat(setToGeneListTmp).ToDictionary();
            }
            return setToGene;
        }

        /// <summary>
        /// the kind given, with goId in the first column, and transcriptId in the second column
        /// </summary>
        /// <param name="setFile"></param>
        /// <param name="setToGeneListTmp"></param>
        private static void ParseSparseGeneSetToTranscriptIdFile(string setFile, out Dictionary<string, List<string>> setToTranscriptId, ParallelOptions parallelOptions)
        {
            var rawMatrix = LoadData.CreateMatrixFromMatrixFile(setFile, parallelOptions);
            List<string> geneSetIds = rawMatrix.RowKeys.ToList();
            List<string> transcriptIds = rawMatrix.ColKeys.ToList();
            setToTranscriptId = new Dictionary<string, List<string>>(geneSetIds.Count);
            foreach (string goSetId in geneSetIds)
            {
                List<string> theseTranscriptIds = transcriptIds.Where(elt => !rawMatrix.IsMissing(goSetId, elt)).ToList();
                setToTranscriptId.Add(goSetId, theseTranscriptIds);
            }
        }

        public static Dictionary<string, string> ParseGeneSetGmtFile(string goSetFile)
        {
            Dictionary<string, List<string>> garb1, garb2;
            return ParseGeneSetGmtFileWithGoTerms(goSetFile, out garb1, out garb2);
        }

        /// <summary>
        /// Currently only gets the the GO code and name into a dictionary (e.g. GO:0004114, 3__5__CYCLIC_NUCLEOTIDE_PHOSPHODIESTERASE_ACTIVITY)
        /// </summary>
        /// <param name="goSetFile"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ParseGeneSetGmtFileWithGoTerms(string goSetFile, out Dictionary<string, List<string>> setToListOfGenes, out Dictionary<string, List<string>> geneToSetList)
        {
            Dictionary<string, string> codeAndName = new Dictionary<string, string>();
            setToListOfGenes = new Dictionary<string, List<string>>();
            geneToSetList = new Dictionary<string, List<string>>();

            using (TextReader textReader = File.OpenText(goSetFile))
            {
                string header = textReader.ReadLine();
                int numCols = header.Split('\t').Count();

                string line = null;
                while (null != (line = textReader.ReadLine()))
                {
                    List<string> lineItems = line.Split('\t').ToList();
                    string thisName = lineItems[0];
                    if (lineItems.Count < 1)
                    {
                        Console.WriteLine("here");
                    }
                    string thisCode = lineItems[1].Split(' ')[6].Replace(".", "");
                    Helper.CheckCondition(thisCode.StartsWith("GO:"), "GO code: " + thisCode + " is of the wrong format (GO:0004114)");
                    codeAndName.Add(thisCode, thisName);

                    List<string> genesInSetTmp = lineItems.SubSequence(2, lineItems.Count - 2).ToList();
                    List<string> genesInSet = new List<string>();
                    string[] delim = new string[] { " /// " };  // sometimes genes are written as FDPS /// LOC402397
                    foreach (string geneName in genesInSetTmp)
                    {
                        if (geneName.Contains(delim[0]))
                        {
                            Console.WriteLine("here");
                        }
                        string[] individualGenes = geneName.Split(delim, StringSplitOptions.None);
                        foreach (string tmpName in individualGenes) genesInSet.Add(tmpName);
                    }

                    setToListOfGenes.Add(thisName, genesInSet.ToList());
                    foreach (string oneGeneInSet in genesInSet)
                    {
                        List<string> thisSetList = geneToSetList.GetValueOrDefault(oneGeneInSet);
                        thisSetList.Add(thisCode);
                    }
                }
            }
            return codeAndName;
        }

        public static void ParseGeneSetGmtFile(string gmtSetFile, out Dictionary<string, List<string>> setToListOfGenes, out Dictionary<string, List<string>> geneToSetList, bool caseInsensitiveStrings)
        {
            //Dictionary<string, string> codeAndName = new Dictionary<string, string>();
            setToListOfGenes = new Dictionary<string, List<string>>();
            geneToSetList = new Dictionary<string, List<string>>();

            using (TextReader textReader = File.OpenText(gmtSetFile))
            {
                //string header = textReader.ReadLine();
                //int numCols = header.Split('\t').Count();

                string line = null;
                while (null != (line = textReader.ReadLine()))
                {
                    if (caseInsensitiveStrings)
                    {
                        line = line.ToLowerInvariant();
                    }
                    List<string> lineItems = line.Split('\t').ToList();
                    string thisName = lineItems[0];
                    string thisCode = lineItems[1];
                    //codeAndName.Add(thisCode, thisName);

                    List<string> genesInSetTmp = lineItems.SubSequence(2, lineItems.Count - 2).ToList();
                    List<string> genesInSet = new List<string>();
                    string[] delim = new string[] { " /// " };  // sometimes genes are written as FDPS /// LOC402397
                    foreach (string geneName in genesInSetTmp)
                    {
                        string[] individualGenes = geneName.Split(delim, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string tmpName in individualGenes) genesInSet.Add(tmpName);
                    }

                    setToListOfGenes.Add(thisName, genesInSet.ToList());
                    foreach (string oneGeneInSet in genesInSet)
                    {
                        List<string> thisSetList = geneToSetList.GetValueOrDefault(oneGeneInSet);
                        thisSetList.Add(thisCode);
                    }
                }
            }
            //return codeAndName;
        }

        public static void CheckCidOrderMatchesOnAllData(Matrix<string, string, double> snpMatrix, Matrix<string, string, double> phenMatrix, Matrix<string, string, double> fixedEffectsMatrix, List<Matrix<string, string, double>> kernelMatrixes)
        {
            CheckCidOrderMatchesOnAllData(snpMatrix, null, phenMatrix, fixedEffectsMatrix, kernelMatrixes);

        }

        public static void CheckCidOrderMatchesOnAllData(List<Matrix<string, string, double>> mList, List<string> mNames)
        {
            List<string> canonicalCids = null;
            for (int j = 0; j < mList.Count; j++)
            {
                if (mList[j] != null && mList[j].RowCount>0 && mList[j].ColCount>0)
                {
                    if (null == canonicalCids)
                    {
                        canonicalCids = mList[j].ColKeys.ToList();
                    }
                    else
                    {
                        string errMsg = "";
                        if (mNames != null)
                        {
                            errMsg = "cids don't match up for " + mNames[j];
                        }
                        Helper.CheckCondition(mList[j].ColKeys.SequenceEqual(canonicalCids), errMsg);
                    }
                }
            }
        }

        public static void CheckCidOrderMatchesOnAllData(Matrix<string, string, double> snpMatrix, Matrix<string, string, double> secondPredMatrix, Matrix<string, string, double> phenMatrix, Matrix<string, string, double> fixedEffectsMatrix, List<Matrix<string, string, double>> kernelMatrixes)
        {
            IList<string> canonicalOrderCids = (snpMatrix != null) ? snpMatrix.ColKeys : phenMatrix.ColKeys;

            if (phenMatrix != null) Helper.CheckCondition(phenMatrix.ColKeys.SequenceEqual(canonicalOrderCids), "The cids of the phenMatrix are not in the same order as other files.");
            if (snpMatrix != null) Helper.CheckCondition(snpMatrix.ColKeys.SequenceEqual(canonicalOrderCids), "The cids of the snpMatrix are not in the same order as other files.");
            if (secondPredMatrix != null) Helper.CheckCondition(secondPredMatrix.ColKeys.SequenceEqual(canonicalOrderCids), "The cids of the secondPredMatrix are not in the same order as other files.");
            if (fixedEffectsMatrix != null) Helper.CheckCondition(fixedEffectsMatrix.ColKeys.SequenceEqual(canonicalOrderCids), "The cids of the fixedEffectsMatrix are not in the same order as other files.");

            if (kernelMatrixes != null && kernelMatrixes.ElementAtOrDefault(0) != null)
            {
                int numKernels = kernelMatrixes.Count;
                for (int k = 0; k < numKernels; k++)
                {
                    //if we are generating data, we may have a smaller kernel, in which case, just ignore it here, if this is not the case, things will crash later anyhow
                    if (kernelMatrixes[k].ColCount == canonicalOrderCids.Count)
                    {
                        Helper.CheckCondition(kernelMatrixes[k].ColKeys.SequenceEqual(canonicalOrderCids), "covariance cids don't match the other input files");
                        Helper.CheckCondition(kernelMatrixes[k].CheckThatRowAndColKeysAreIdentical(), "covariance matrix keys are violated (rows not the same as colunns");
                    }
                }
            }
        }

        public static int ReadInKernelFiles(int maxDegreeParallelism, int numKernels, ShoMatrix kernelsToAdd, out List<Matrix<string, string, double>> kernelMatrixes, bool allowNonPosDefKernel, List<string> kernelFiles, double kernelDiagFudge)
        {
            ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeParallelism };
            return ReadInKernelFiles(parallelOptions, numKernels, kernelsToAdd, out kernelMatrixes, allowNonPosDefKernel, kernelFiles, kernelDiagFudge);
        }

        public static int ReadInKernelFiles(ParallelOptions parallelOptions, int numKernels, ShoMatrix kernelsToAdd, out List<Matrix<string, string, double>> kernelMatrixes, bool allowNonPosDefKernel, List<string> kernelFiles, double kernelDiagFudge)
        {
            kernelMatrixes = new List<Matrix<string, string, double>>();
            for (int k = 0; k < numKernels; k++)
            {
                int tmpNum = k + 1;
                if (!_QUIET) Console.Write("Reading in kernel matrix " + tmpNum + "...");
                //var nxtKernel = GenerateKernels.LoadData.ReadKernelMatrix(kernelFiles[k], parallelOptions).AsShoMatrix();
                var nxtKernel = ShoMatrix.CreateShoMatrixFromDenseFileFormat(kernelFiles[k]);
                if (kernelDiagFudge != 0)
                {
                    nxtKernel.DoubleArray = nxtKernel.DoubleArray + kernelDiagFudge * ShoUtils.Identity(nxtKernel.RowCount, nxtKernel.ColCount);
                }
                kernelMatrixes.Add(nxtKernel);
                if (!allowNonPosDefKernel)
                {
                    Helper.CheckCondition(kernelMatrixes[k].AsShoMatrix().DoubleArray.TryCholesky() == null, "Cholesky failed on kernel: " + kernelFiles[k]);
                    Helper.CheckCondition(kernelMatrixes[k].AsShoMatrix().DoubleArray.IsFullRank(), "kernel matrix " + kernelFiles[k] + " is not full rank--this shouldn't matter for single kernel models, but I'm disallowing it because it messes up multi-kernel modesl when LBFGS learning is used. It also matters for data generation");
                }
                if (!_QUIET) Console.WriteLine("...done");
            }
            if (null != kernelsToAdd)
            {
                kernelMatrixes.Add(kernelsToAdd);
                kernelFiles.Add("linearComboWeReadIn");
                numKernels++;
            }
            //if (numKernels == 0) kernelMatrixes.Add(null);
            return numKernels;
        }

        public static DoubleArray ComputeSquaredExponentialKernel(DoubleArray expData, ParallelOptions parallelOptions)
        {
            int N = expData.size1;
            DoubleArray sqExpKernel = new DoubleArray(N, N);
            Parallel.ForEach(Enumerable.Range(0, N), parallelOptions, cidIndex1 =>
            {
                Console.Write(string.Format("\r{0}/{1}\t", cidIndex1, N));
                Parallel.ForEach(Enumerable.Range(cidIndex1, N - cidIndex1), parallelOptions, cidIndex2 =>
                {
                    DoubleArray d1 = expData.GetCol(cidIndex1);
                    DoubleArray d2 = expData.GetCol(cidIndex2);

                    //would L1 norm be more robust here...?
                    sqExpKernel[cidIndex1, cidIndex2] = Math.Exp(-0.5 * (d1 - d2).Pow(2).Sum());//See Rasmussen Chpt 2 Eq 2.12, squared exponential kernel
                    sqExpKernel[cidIndex2, cidIndex1] = sqExpKernel[cidIndex1, cidIndex2];
                });
            });
            return sqExpKernel;
        }


        public static ShoMatrix AverageIndividualsInEachStrain(ParallelOptions parallelOptions, List<string> strainOrder, ShoMatrix phenMatrix, Matrix<string, string, string> strainIds, out ShoMatrix numIndPerStrain)
        {
            return AverageIndividualsInEachStrain(parallelOptions, strainOrder, phenMatrix, strainIds, out numIndPerStrain, -1);
        }

        public static ShoMatrix AverageIndividualsInEachStrain(ParallelOptions parallelOptions, List<string> strainOrder, ShoMatrix phenMatrix, Matrix<string, string, string> strainIds, out ShoMatrix numIndPerStrain, int repeatNum)
        {
            ShoMatrix newPhenMatrix = phenMatrix.AverageColSubsetsInSameGroup(strainIds, out numIndPerStrain, parallelOptions, repeatNum);
            //now re-order so that it matches the SNPs strain order
            newPhenMatrix = newPhenMatrix.SelectColsView(strainOrder).AsShoMatrix();
            numIndPerStrain = numIndPerStrain.SelectColsView(strainOrder).AsShoMatrix();
            return newPhenMatrix;
        }

        /// <summary>
        ///         /// File formats:
        ///            'phasedDataFile', output of Mach and each line looks like this, where 100000002 is the cid
        ///                    10000002->10000002 HAPLO1 222222222221221211
        ///            'matchingDatFile', input to Mach with marker info, and each line looks like this, where M is always there,
        ///                               and 1_2 is the marker name
        ///                     M 1_2
        /// NOTE: output is two ShoMatrixes--one haplotype in each
        ///
        /// </summary>
        /// <param name="phasedDataFile"></param>
        /// <param name="matchingDatFile"></param>
        /// <param name="hap1">cols are cids, rows SNPS: one haplotype</param>
        /// <param name="hap2">cols are cids, rows SNPS: other haplotype</param>
        public static void ReadMachPhasedData(string phasedDataFile, string matchingDatFile,
                                                     out ShoMatrix hap1, out ShoMatrix hap2)
        {
            List<string> cids = new List<string>();
            List<List<double>> h1 = new List<List<double>>();
            List<List<double>> h2 = new List<List<double>>();
            string[] splitStringArr = new string[2] { " ", "->" };

            //read in the phasing results
            using (TextReader tr = File.OpenText(phasedDataFile))
            {
                bool keepGoing = true;
                while (keepGoing)
                {
                    for (int h = 0; h < 2; h++)
                    {
                        string nxtLine = tr.ReadLine();
                        if (nxtLine == null && h == 0)
                        {
                            keepGoing = false;
                            break;
                        }
                        Helper.CheckCondition(null != nxtLine || h == 0);//make sure it doesn't end before we have a second haplotype

                        string[] parsedLine = nxtLine.Split(splitStringArr, System.StringSplitOptions.None);

                        List<double> nxtGenotypeSet = SpecialFunctions.CharStringToDoubleList(parsedLine[3]);
                        string nxtCid = parsedLine[0];
                        if (h == 0)
                        {
                            h1.Add(nxtGenotypeSet);
                            cids.Add(nxtCid);
                        }
                        else
                        {
                            h2.Add(nxtGenotypeSet);
                            Helper.CheckCondition(cids.Last() == nxtCid, "same cid should appear once in a row: once for each haplotype, but it didn't");
                        }

                        //if (h == 1) System.Console.WriteLine("finished reading phased data for cid {0}", nxtCid);
                    }
                }
            }

            //read in the dat file that gives us the SNP names
            int numSNP = h1.Last().Count();
            List<string> snpIds = new List<string>(numSNP);
            using (TextReader tr = File.OpenText(matchingDatFile))
            {
                string line;
                while ((line = tr.ReadLine()) != null)
                {
                    string nxtSnpId = line.Split(' ')[1];
                    snpIds.Add(nxtSnpId);
                }
            }
            Helper.CheckCondition(snpIds.Count == numSNP, ".dat file does not match up with .phased file");

            DoubleArray X = DoubleArray.From(h1).T;//(new DoubleArray(h1)).T;
            hap1 = new ShoMatrix(X, snpIds, cids, double.NaN);

            X = DoubleArray.From(h2).T;//new DoubleArray(h2)).T;
            hap2 = new ShoMatrix(X, snpIds, cids, double.NaN);

            List<double> allowedVals = new List<double> { 0, 1, 2 };
            hap1.CheckDataEncoding(allowedVals);
            hap2.CheckDataEncoding(allowedVals);
        }

        public static ShoMatrix ParseKernelListFileAndAddTogetherKernelsWithWeights(string kernelsToAddFromFile, ParallelOptions parallelOptions)
        {
            string tmp;
            return ParseKernelListFileAndAddTogetherKernelsWithWeights(kernelsToAddFromFile, parallelOptions, out tmp);
        }

        /// <summary>
        /// Two file formats allowed:
        /// (1) if we have pasted in output from multi-kernel learning, then the format may instead be:
        ///   -k0, weight0, filename0
        ///   -k1, weight1, filename1
        ///in which case we will parse this and take a linear combo given by the weights (rather than just weights of 1)
        ///(2)The other acceptable form is just one filename per line:
        ///    filename0
        ///    filename1        
        /// </summary>
        /// <param name="kernelsToAddFromFile"></param>
        /// <returns></returns>
        public static ShoMatrix ParseKernelListFileAndAddTogetherKernelsWithWeights(string kernelsToAddFromFile, ParallelOptions parallelOptions, out string tmpStr)
        {
            DoubleArray kernelWeights;
            List<string> kernelListOfFiles = LoadData.ParseKernelListFile(kernelsToAddFromFile, out kernelWeights);

            ShoMatrix newK = LoadData.ReadInAndAddTogetherKernels(kernelWeights, kernelListOfFiles, parallelOptions, out tmpStr);
            return newK;
        }

        public static ShoMatrix ReadInAndAddTogetherKernels(DoubleArray kernelWeights, List<string> kernelListOfFiles, ParallelOptions parallelOptions, out string tmpStr)
        {
            tmpStr = "";
            ShoMatrix newK = LoadData.ReadKernelMatrix(kernelListOfFiles[0], parallelOptions).AsShoMatrix();
            string nxtString = "Reading in kernel: " + kernelListOfFiles[0] + " with weight=" + kernelWeights[0] + " and adding...";
            System.Console.WriteLine(nxtString);
            tmpStr += nxtString + "\n";

            List<string> cidList = newK.RowKeys.ToList();
            for (int k = 1; k < kernelListOfFiles.Count; k++)
            {
                double wght = kernelWeights[k];
                nxtString = "Reading in kernel: " + kernelListOfFiles[k] + " with weight=" + wght + " and adding...";
                System.Console.WriteLine(nxtString);
                tmpStr += nxtString + "\n";
                newK.AddInto(LoadData.ReadKernelMatrixWithOrderedCids(kernelListOfFiles[k], cidList, parallelOptions).MultByConst(wght));
            }
            return newK;
        }

        public static List<string> ParseKernelListFile(string kernelsToAddFromFile)
        {
            DoubleArray garb;
            return ParseKernelListFile(kernelsToAddFromFile, out garb);
        }

        /// <summary>
        /// Read and parse a list of kernels to use, one per line. 
        ///The base directory it uses is the one that this file lies in
        ///There are two allowed formats:
        /// (1) if we have pasted in output from multi-kernel learning, then the format may instead be:
        ///   -k0, weight0, filename0
        ///   -k1, weight1, filename1
        ///in which case we will parse this and take a linear combo given by the weights (rather than just weights of 1)
        ///(2)The other acceptable form is just one filename per line:
        ///    filename0
        ///    filename1        
        /// </summary>
        /// <param name="kernelsToAddFromFile"></param>
        /// <param name="kernelListOfFiles"></param>
        /// <param name="kernelWeight"></param>
        public static List<string> ParseKernelListFile(string kernelsToAddFromFile, out DoubleArray kernelWeights)
        {
            List<string> kernelListTmp = Bio.Util.FileUtils.ReadEachLine(kernelsToAddFromFile).ToList();
            int K = kernelListTmp.Count;
            List<string> kernelListOfFiles = SpecialFunctions.CreateListWithDefaultElements<string>(K);
            kernelWeights = ShoUtils.DoubleArrayOnes(1, K);
            string baseDir = new FileInfo(kernelsToAddFromFile).Directory.FullName + @"\";
            bool? HAS_WEIGHTS = null;
            System.Console.WriteLine("parsing list of kernel files");
            for (int k = 0; k < K; k++)
            {
                List<string> nxtKernelInfo = kernelListTmp[k].Split(',').ToList();
                if (!HAS_WEIGHTS.HasValue)
                {
                    HAS_WEIGHTS = (nxtKernelInfo.Count == 1) ? false : true;
                }
                else
                {
                    Helper.CheckCondition((HAS_WEIGHTS.Value && nxtKernelInfo.Count == 3) ||
                                                    (!HAS_WEIGHTS.Value && nxtKernelInfo.Count == 1),
                                      "either each line contains just a filename, or each line contains: '-kX, weightX, filenameX'");
                }
                if (HAS_WEIGHTS.Value)
                {
                    kernelListOfFiles[k] = baseDir + nxtKernelInfo[2].Trim();
                    kernelWeights[k] = double.Parse(nxtKernelInfo[1]);
                    System.Console.WriteLine("------>" + k + "  " + kernelListOfFiles[k]);
                }
                else
                {
                    kernelListOfFiles[k] = baseDir + nxtKernelInfo[0].Trim();
                }
            }
            return kernelListOfFiles;
        }

        public static void IntersectCidsOfAllInputData(ref Matrix<string, string, double> snpMatrix, ref Matrix<string, string, double> phenMatrix, ref Matrix<string, string, double> fixedEffectsMatrix,
                                                              List<Matrix<string, string, double>> kernelMatrixes)
        {
            Matrix<string, string, double> secondPredMatrix = null;
            IntersectCidsOfAllInputData(ref snpMatrix, ref secondPredMatrix, ref phenMatrix, ref fixedEffectsMatrix, kernelMatrixes, null);
        }

        public static void IntersectCidsOfAllInputData(ref Matrix<string, string, double> snpMatrix, ref Matrix<string, string, double> secondPredMatrix, ref Matrix<string, string, double> phenMatrix, ref Matrix<string, string, double> fixedEffectsMatrix,
                                                              List<Matrix<string, string, double>> kernelMatrixes)
        {
            IntersectCidsOfAllInputData(ref snpMatrix, ref secondPredMatrix, ref phenMatrix, ref fixedEffectsMatrix, kernelMatrixes, null);
        }

        /// <summary>
        /// Make sure they all have the same cids AND that the cids keys are IN THE SAME ORDER!
        /// </summary>
        /// <param name="snpMatrix"></param>
        /// <param name="phenMatrix"></param>
        /// <param name="fixedEffectsMatrix"></param>
        /// <param name="kernelMatrixes"></param>
        public static void IntersectCidsOfAllInputData(ref Matrix<string, string, double> snpMatrix, ref Matrix<string, string, double> secondPredMatrix, ref Matrix<string, string, double> phenMatrix, ref Matrix<string, string, double> fixedEffectsMatrix,
                                                       List<Matrix<string, string, double>> kernelMatrixes, List<Matrix<string, string, double>> meanMatrixes)
        {
            //intersect the cids from all possible matrixes
            if (secondPredMatrix != null)
            {
                // snpMatrix shouldn't be null if the secondPredMatrix is non-null
                MatrixExtensionsJenn.IntersectOnColKeys(ref snpMatrix, ref secondPredMatrix);
            }

            if (null != snpMatrix && phenMatrix != null)
            {
                MatrixExtensionsJenn.IntersectOnColKeys(ref phenMatrix, ref snpMatrix);
            }

            int numKernels = (kernelMatrixes == null) ? 0 : kernelMatrixes.Count;
            if (null != fixedEffectsMatrix && null != kernelMatrixes)
            {
                Matrix<string, string, double> tmpMatrix1 = kernelMatrixes[0];
                MatrixExtensionsJenn.IntersectOnColKeys(ref tmpMatrix1, ref fixedEffectsMatrix);
                kernelMatrixes[0] = tmpMatrix1.IntersectColsWithRows();
            }
            for (int k = 0; k < numKernels; k++)
            {
                Matrix<string, string, double> tmpMatrix = kernelMatrixes[k];
                if (null != phenMatrix) MatrixExtensionsJenn.IntersectOnColKeys(ref phenMatrix, ref tmpMatrix);
                kernelMatrixes[k] = tmpMatrix.IntersectColsWithRows();
                Helper.CheckCondition(kernelMatrixes[k].IsSquare());

            }
            if (null != phenMatrix && null != snpMatrix) MatrixExtensionsJenn.IntersectOnColKeys(ref phenMatrix, ref snpMatrix);
            if (null != fixedEffectsMatrix && null != snpMatrix)
            {
                MatrixExtensionsJenn.IntersectOnColKeys(ref snpMatrix, ref fixedEffectsMatrix);
            }
            else if (null != fixedEffectsMatrix)
            {
                MatrixExtensionsJenn.IntersectOnColKeys(ref phenMatrix, ref fixedEffectsMatrix);
            }

            if (secondPredMatrix != null)
            {
                MatrixExtensionsJenn.IntersectOnColKeys(ref snpMatrix, ref secondPredMatrix);
            }

            /* Now make sure the keys are actually in the same order, since the operation above does not guarantee this, as I found out the hard way!*/
            List<string> cidOrder;
            if (snpMatrix != null)
            {
                cidOrder = snpMatrix.ColKeys.ToList();
            }
            else
            {
                cidOrder = phenMatrix.ColKeys.ToList();
            }
            if (phenMatrix != null) phenMatrix = phenMatrix.CheckEqualAndReorderColKeys(cidOrder);
            if (secondPredMatrix != null) secondPredMatrix = secondPredMatrix.CheckEqualAndReorderColKeys(cidOrder);
            for (int k = 0; k < numKernels; k++)
            {
                Matrix<string, string, double> tmp = kernelMatrixes[k];
                tmp = tmp.CheckEqualAndReorderColAndRowKeys(cidOrder, cidOrder);
                kernelMatrixes[k] = tmp;
                Helper.CheckCondition(kernelMatrixes[k].CheckThatRowAndColKeysAreIdentical(), "covariance matrix keys are violated");
            }
            if (null != fixedEffectsMatrix) fixedEffectsMatrix = fixedEffectsMatrix.CheckEqualAndReorderColKeys(cidOrder);

            if (meanMatrixes != null)
            {
                for (int k = 0; k < numKernels; k++)
                {
                    Matrix<string, string, double> tmp = meanMatrixes[k];
                    tmp = tmp.CheckEqualAndReorderColKeys(cidOrder);
                    meanMatrixes[k] = tmp;
                }

            }
        }

        public static void CheckCidsSequenceEqualOfAllInputData(ref Matrix<string, string, double> snpMatrix, ref Matrix<string, string, double> phenMatrix, ref Matrix<string, string, double> fixedEffectsMatrix,
                                                              List<Matrix<string, string, double>> kernelMatrixes, List<Matrix<string, string, double>> meanMatrixes)
        {
            List<string> cidOrder;
            int numKernels = kernelMatrixes.Count;
            if (snpMatrix != null)
            {
                cidOrder = snpMatrix.ColKeys.ToList();
            }
            else
            {
                cidOrder = phenMatrix.ColKeys.ToList();
            }
            if (phenMatrix != null) phenMatrix.CheckEqualOrderColKeys(cidOrder);
            for (int k = 0; k < numKernels; k++)
            {
                Matrix<string, string, double> tmp = kernelMatrixes[k];
                tmp.CheckEqualOrderColAndRowKeys(cidOrder, cidOrder);
            }
            if (null != fixedEffectsMatrix) fixedEffectsMatrix.CheckEqualOrderColKeys(cidOrder);

            if (meanMatrixes != null)
            {
                for (int k = 0; k < numKernels; k++)
                {
                    Matrix<string, string, double> tmp = meanMatrixes[k];
                    tmp.CheckEqualOrderColKeys(cidOrder);
                    meanMatrixes[k] = tmp;
                }
            }
        }


        //public static void LoadSnpAndPhenData(string snpDataFile, string phenDataFile, string strainIdFile,
        //          int maxDegreeOfParallelism,
        //          out Matrix<string, string, double> snpMatrix, out Matrix<string, string, double> phenMatrix, out Matrix<string, string, string> strainIds,
        //          bool tryToLeaveDataOnDisk)
        //{
        //    ParallelOptions parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
        //    LoadSnpAndPhenData(snpDataFile, phenDataFile, strainIdFile, parallelOptions, out snpMatrix, out phenMatrix, out strainIds, tryToLeaveDataOnDisk);
        //}

        //public static void LoadSnpAndPhenData(string snpDataFile, string phenDataFile, string strainIdFile,
        //    ParallelOptions parallelOptions,
        //    out Matrix<string, string, double> snpMatrix, out Matrix<string, string, double> phenMatrix, out Matrix<string, string, string> strainIds,
        //    bool tryToLeaveDataOnDisk)
        //{
        //    //load up the various data files (and do not process them--assume this has already been done)

        //    if (null != phenDataFile)
        //    {
        //        phenMatrix = null;

        //        if (tryToLeaveDataOnDisk)
        //        {
        //            try
        //            {
        //                if (!_QUIET) Console.WriteLine("Trying to read phenotype data file as rowKeysPaddedDouble...");
        //                phenMatrix = LoadData.CreateMatrixFromRowKeysPaddedDoubleFile(phenDataFile, parallelOptions);
        //                if (!_QUIET) Console.WriteLine("....succeeded");
        //            }
        //            catch (Exception e)
        //            {
        //                if (!_QUIET) Console.WriteLine("...failed: ", e.Message);
        //            }

        //            if (null == phenMatrix)
        //            {
        //                try
        //                {
        //                    if (!_QUIET) Console.WriteLine("Trying to read phenotype data file as rowKeysPaddedDouble...");
        //                    phenMatrix = LoadData.CreateMatrixFromPaddedDoubleFile(phenDataFile, parallelOptions);
        //                    if (!_QUIET) Console.WriteLine("....succeeded");
        //                }
        //                catch (Exception e)
        //                {
        //                    if (!_QUIET) Console.WriteLine("...failed:", e.Message);
        //                }
        //            }
        //        }

        //        if (null == phenMatrix)
        //        {
        //            if (!_QUIET) Console.Write("Reading phenotype data file...");
        //            phenMatrix = LoadData.CreateMatrixFromMatrixFile(phenDataFile, parallelOptions);
        //            if (!_QUIET) Console.WriteLine("...done");
        //        }
        //    }
        //    else
        //    {
        //        phenMatrix = null;
        //    }

        //    snpMatrix = null;
        //    if (null != snpDataFile)
        //    {
        //        //snpMatrix = LoadDenseFileOfSingleCharacterDoubles(snpDataFile, double.NaN);
        //        if (tryToLeaveDataOnDisk)
        //        {
        //            try
        //            {
        //                if (!_QUIET) Console.WriteLine("Trying to read snp data file as rowKeysPaddedDouble...");
        //                snpMatrix = LoadData.CreateMatrixFromRowKeysPaddedDoubleFile(snpDataFile, parallelOptions);
        //                if (!_QUIET) Console.WriteLine("....succeeded");
        //            }
        //            catch (Exception e)
        //            {
        //                if (!_QUIET) Console.WriteLine("...failed: ", e.Message);
        //            }

        //            if (null == snpMatrix)
        //            {
        //                try
        //                {
        //                    if (!_QUIET) Console.WriteLine("Reading in SNP data from DenseAnsi file...");
        //                    snpMatrix = LoadData.CreateDenseAnsiNumericWrapperMatrix(snpDataFile, parallelOptions);
        //                    if (!_QUIET) Console.WriteLine("....succeeded");
        //                }
        //                catch (Exception e)
        //                {
        //                    if (!_QUIET) Console.WriteLine("...failed: ", e.Message);
        //                }
        //            }

        //            if (null == snpMatrix)
        //            {
        //                try
        //                {
        //                    if (!_QUIET) Console.WriteLine("Reading in SNP data from paddedDense file...");
        //                    snpMatrix = LoadData.CreateMatrixFromPaddedDoubleFile(snpDataFile, parallelOptions);
        //                    if (!_QUIET) Console.WriteLine("....succeeded");
        //                }
        //                catch (Exception e)
        //                {
        //                    if (!_QUIET) Console.WriteLine("...failed: ", e.Message);
        //                }
        //            }
        //        }

        //        if (null == snpMatrix)
        //        {
        //            if (!_QUIET) Console.Write("Reading in SNP data from regular file...");
        //            snpMatrix = LoadData.CreateMatrixFromMatrixFile(snpDataFile, parallelOptions);//.AsShoMatrix();
        //        }
        //        if (!_QUIET) Console.WriteLine("...done");
        //    }

        //    if (null != strainIdFile)
        //    {
        //        strainIds = LoadData.CreateStringMatrixFromMatrixFile(strainIdFile, parallelOptions);

        //    }
        //    else
        //    {
        //        strainIds = null;
        //    }

        //    //MatrixExtensionsJenn.IntersectOnColKeys(ref phenMatrix, ref snpMatrix);
        //    //MatrixExtensionsJenn.CheckEqualColKeys(phenMatrix, snpMatrix.ColKeys.ToList());
        //}

        public static void LoadSnpAndPhenData(string snpDataFile, string phenDataFile, string strainIdFile,
                   LeaveDataOnDisk leaveDataOnDisk,
                   ParallelOptions parallelOptions,
                   out Matrix<string, string, double> snpMatrix, out Matrix<string, string, double> phenMatrix, out Matrix<string, string, string> strainIds)
        {
            Matrix<string, string, double> secondPredMatrix;
            LoadSnpAndPhenData(snpDataFile, null, phenDataFile, strainIdFile, leaveDataOnDisk, parallelOptions, out snpMatrix, out secondPredMatrix, out phenMatrix, out strainIds);
        }

        public static void LoadSnpAndPhenData(string snpDataFile, string secondPredFile, string phenDataFile, string strainIdFile,
                   LeaveDataOnDisk leaveDataOnDisk,
                   ParallelOptions parallelOptions,
                   out Matrix<string, string, double> snpMatrix, out Matrix<string, string, double> secondPredMatrix, out Matrix<string, string, double> phenMatrix,
                   out Matrix<string, string, string> strainIds)
        {
            //load up the various data files (and do not process them--assume this has already been done)


            Helper.CheckCondition(leaveDataOnDisk == LeaveDataOnDisk.Try || leaveDataOnDisk == LeaveDataOnDisk.IndexFileOnly || leaveDataOnDisk == LeaveDataOnDisk.No,
                "Expect -leaveDataOnDisk to be 'Try', 'No', or 'IndexFileOnly'. Don't know " + leaveDataOnDisk.ToString());
            if (null != phenDataFile)
            {
                phenMatrix = null;

                if (leaveDataOnDisk == LeaveDataOnDisk.Try || leaveDataOnDisk == LeaveDataOnDisk.IndexFileOnly)
                {
                    try
                    {
                        if (!_QUIET) Console.WriteLine("Trying to read phenotype data file as rowKeysPaddedDouble...");
                        phenMatrix = LoadData.CreateMatrixFromRowKeysPaddedDoubleFile(phenDataFile, parallelOptions);
                        if (!_QUIET) Console.WriteLine("....succeeded");
                    }
                    catch (Exception e)
                    {
                        if (leaveDataOnDisk == LeaveDataOnDisk.IndexFileOnly)
                        {
                            throw;
                        }
                        if (!_QUIET) Console.WriteLine("...failed: {0}", e.Message);
                    }

                    if (null == phenMatrix)
                    {
                        try
                        {
                            if (!_QUIET) Console.WriteLine("Trying to read phenotype data file as paddedDouble...");
                            phenMatrix = LoadData.CreateMatrixFromPaddedDoubleFile(phenDataFile, parallelOptions);
                            if (!_QUIET) Console.WriteLine("....succeeded");
                        }
                        catch (Exception e)
                        {
                            if (!_QUIET) Console.WriteLine("...failed: {0}", e.Message);
                        }
                    }
                }

                if (null == phenMatrix)
                {
                    if (!_QUIET) Console.Write("Reading phenotype data file...");
                    phenMatrix = LoadData.CreateMatrixFromMatrixFile(phenDataFile, parallelOptions);
                    if (!_QUIET) Console.WriteLine("...done");
                }
            }
            else
            {
                phenMatrix = null;
            }

            snpMatrix = null;
            if (null != snpDataFile)
            {
                //snpMatrix = LoadDenseFileOfSingleCharacterDoubles(snpDataFile, double.NaN);
                if (leaveDataOnDisk == LeaveDataOnDisk.Try || leaveDataOnDisk == LeaveDataOnDisk.IndexFileOnly)
                {
                    try
                    {
                        if (!_QUIET) Console.WriteLine("Trying to read snp data file as rowKeysPaddedDouble...");
                        snpMatrix = LoadData.CreateMatrixFromRowKeysPaddedDoubleFile(snpDataFile, parallelOptions);
                        if (!_QUIET) Console.WriteLine("....succeeded");
                    }
                    catch (Exception e)
                    {
                        if (leaveDataOnDisk == LeaveDataOnDisk.IndexFileOnly)
                        {
                            throw;
                        }
                        if (!_QUIET) Console.WriteLine("...failed: {0}", e.Message);
                    }

                    if (null == snpMatrix)
                    {
                        try
                        {
                            if (!_QUIET) Console.WriteLine("Reading in SNP data from DenseAnsi file...");
                            snpMatrix = LoadData.CreateDenseAnsiNumericWrapperMatrix(snpDataFile, parallelOptions);
                            if (!_QUIET) Console.WriteLine("....succeeded");
                        }
                        catch (Exception e)
                        {
                            if (!_QUIET) Console.WriteLine("...failed: {0}", e.Message);
                        }
                    }

                    if (null == snpMatrix)
                    {
                        try
                        {
                            if (!_QUIET) Console.WriteLine("Reading in SNP data from paddedDense file...");
                            snpMatrix = LoadData.CreateMatrixFromPaddedDoubleFile(snpDataFile, parallelOptions);
                            if (!_QUIET) Console.WriteLine("....succeeded");
                        }
                        catch (Exception e)
                        {
                            if (!_QUIET) Console.WriteLine("...failed: {0}", e.Message);
                        }
                    }
                }

                if (null == snpMatrix)
                {
                    if (!_QUIET) Console.Write("Reading in SNP data from regular file...");
                    snpMatrix = LoadData.CreateMatrixFromMatrixFile(snpDataFile, parallelOptions);//.AsShoMatrix();
                }
                if (!_QUIET) Console.WriteLine("...done");
            }

            if (phenMatrix != null && snpMatrix != null)
            {
                try
                {
                    phenMatrix = phenMatrix.SelectColsView(snpMatrix.ColKeys);
                }
                catch
                {
                    throw new Exception("could not load up phen cids in same order as SNP ids");
                }
            }


            // !!! Redundant code
            secondPredMatrix = null;
            if (null != secondPredFile)
            {
                if (leaveDataOnDisk == LeaveDataOnDisk.Try || leaveDataOnDisk == LeaveDataOnDisk.IndexFileOnly)
                {
                    try
                    {
                        if (!_QUIET) Console.WriteLine("Trying to read second predictor data file as rowKeysPaddedDouble...");
                        secondPredMatrix = LoadData.CreateMatrixFromRowKeysPaddedDoubleFile(secondPredFile, parallelOptions);
                        if (!_QUIET) Console.WriteLine("....succeeded");
                    }
                    catch (Exception e)
                    {
                        if (leaveDataOnDisk == LeaveDataOnDisk.IndexFileOnly)
                        {
                            throw;
                        }
                        if (!_QUIET) Console.WriteLine("...failed: {0}", e.Message);
                    }

                    if (null == secondPredMatrix)
                    {
                        try
                        {
                            if (!_QUIET) Console.WriteLine("Reading in second predictor data from DenseAnsi file...");
                            secondPredMatrix = LoadData.CreateDenseAnsiNumericWrapperMatrix(secondPredFile, parallelOptions);
                            if (!_QUIET) Console.WriteLine("....succeeded");
                        }
                        catch (Exception e)
                        {
                            if (!_QUIET) Console.WriteLine("...failed: {0}", e.Message);
                        }
                    }

                    if (null == secondPredMatrix)
                    {
                        try
                        {
                            if (!_QUIET) Console.WriteLine("Reading in second predictor data from paddedDense file...");
                            secondPredMatrix = LoadData.CreateMatrixFromPaddedDoubleFile(secondPredFile, parallelOptions);
                            if (!_QUIET) Console.WriteLine("....succeeded");
                        }
                        catch (Exception e)
                        {
                            if (!_QUIET) Console.WriteLine("...failed: {0}", e.Message);
                        }
                    }
                }

                if (null == secondPredMatrix)
                {
                    if (!_QUIET) Console.Write("Reading in second predictor data from regular file...");
                    secondPredMatrix = LoadData.CreateMatrixFromMatrixFile(secondPredFile, parallelOptions);
                }
                if (!_QUIET) Console.WriteLine("...done");
            }

            if (null != strainIdFile)
            {
                strainIds = LoadData.CreateStringMatrixFromMatrixFile(strainIdFile, parallelOptions);

            }
            else
            {
                strainIds = null;
            }

            //MatrixExtensionsJenn.IntersectOnColKeys(ref phenMatrix, ref snpMatrix);
            //MatrixExtensionsJenn.CheckEqualColKeys(phenMatrix, snpMatrix.ColKeys.ToList());
        }



        private static Matrix<string, string, double> CreateMatrixFromRowKeysPaddedDoubleFile(string fileName, ParallelOptions parallelOptions)
        {
            //!!!really should have a way to close the file when done
            return RowKeysPaddedDouble.GetInstanceFromRowKeys(fileName, parallelOptions);
        }

        private static Matrix<string, string, double> CreateMatrixFromPaddedDoubleFile(string fileName, ParallelOptions parallelOptions)
        {
            //!!!really should have a way to close the file when done
            return RowKeysPaddedDouble.GetInstanceFromPaddedDouble(fileName, parallelOptions);
        }


        public static bool PassesCholesky(ShoMatrix K)
        {
            bool passesCholesky = true;
            try
            {
                Cholesky tmp = new Cholesky(K.DoubleArray);
            }
            catch (Exception)
            {
                passesCholesky = false;
            }
            return passesCholesky;
        }

        public static BoolArray PassesCholesky(List<ShoMatrix> kernelMatrixes)
        {
            int numKernels = kernelMatrixes.Count;
            BoolArray passesCholesky = new BoolArray(1, numKernels);

            Parallel.For(0, numKernels, k =>
            {
                passesCholesky[k] = PassesCholesky(kernelMatrixes[k]);
            }
                         );
            return passesCholesky;

        }

        public static bool CheckKernelsAndFix(List<ShoMatrix> kernelMatrixes)
        {
            throw new Exception("this is obsolete--just check and fix seperately, not automatically like here");
            int numKernels = kernelMatrixes.Count;
            bool madeChange = false;
            double fudgeFactor = 1.0e-4;
            if (numKernels > 1)
            { //check that Cholesky won't crash 

                Parallel.For(0, numKernels, k =>
                {
                    bool passesCholesky = PassesCholesky(kernelMatrixes[k]);
                    if (!passesCholesky)
                    {
                        System.Console.WriteLine("kernel " + k + " was not pos. def, coercing it to be so");
                        DoubleArray posDefCov = kernelMatrixes[k].DoubleArray.CoercePositiveDefiniteWithEigDecomp(fudgeFactor);
                        kernelMatrixes[k].DoubleArray = posDefCov;
                        madeChange = true;
                    }
                    //try again to see if it works now
                    passesCholesky = PassesCholesky(kernelMatrixes[k]);
                    if (!passesCholesky) throw new Exception("coerce to pos. def failed");
                }
                                 );
            }
            else//check that it is stricly positive defininte (even when it passes this, it could still fail Cholesky, I think
            {
                if (!kernelMatrixes[0].DoubleArray.IsPositiveDefinite())
                {
                    throw new Exception("Kernel matrix is not positive definite, use line below to make it so");
                }
                //DoubleArray posDefCov = kernelMatrixes[0].DoubleArray.CoercePositiveDefiniteWithEigDecomp(fudgeFactor);
                //kernelMatrixes[0].DoubleArray = posDefCov;
            }
            return madeChange;
        }

        public static ShoMatrix ReadFixedEffectsAndConvertToOneHotEncoding(string fixedEffectsFile, ParallelOptions parallelOptions)
        {
            //if (!_QUIET) Console.Write("Reading fixed effects data file...");
            ShoMatrix fixedEffectsMatrix = ShoMatrix.CreateShoMatrixFromMatrixFile(fixedEffectsFile, parallelOptions);
            //if (!_QUIET) Console.WriteLine("...done");

            //convert one hot this one time, then just load that up afterwards
            ConvertToOneHotEncoding(fixedEffectsMatrix);
            throw new Exception("not meant to use this to load data for GWAS analysis, just for re-processing the data and saving to file");

        }

        /// <summary>
        /// Convert one row feature matrix to one set of one-hot encoded ones.  Easy to generalize to more, but I haven't yet.
        /// </summary>
        /// <typeparam name="TVal"></typeparam>
        /// <param name="x"></param>
        /// <returns></returns>
        public static ShoMatrix ConvertToOneHotEncoding<TVal>(Matrix<string, string, TVal> x)
        {
            List<TVal> possibleValues = x.Values.ToList().Distinct().ToList();

            Helper.CheckCondition(x.RowCount == 1, "currently only works for a matrix with one row feature");

            Helper.CheckCondition(!x.IsMissingSome(), "data has missing values so cannot 1-hot encode w/out modifying the code to do so");
            int numVals = possibleValues.Count();

            Dictionary<TVal, int> mapping = new Dictionary<TVal, int>();
            for (int j = 0; j < numVals; j++) mapping.Add(possibleValues[j], j);

            DoubleArray xOneHot = new DoubleArray(numVals, x.ColCount);
            for (int i = 0; i < x.ColCount; i++)
            {
                TVal thisValue = x[0, i];
                if (mapping.ContainsKey(thisValue))
                {
                    xOneHot[mapping[thisValue], i] = 1.0;
                }
                else
                {
                    throw new Exception("should not get to here");
                }
            }
            DoubleArray uniqueRowCounts = xOneHot.Sum(DimOp.EachCol).Unique();
            Helper.CheckCondition(uniqueRowCounts.IsScalar() && uniqueRowCounts[0] == 1, "row counts are not all 1, so an error has occurred");

            List<string> oneHotFeatureNames = possibleValues.Select(elt => elt.ToString()).ToList();
            ShoMatrix tmp = new ShoMatrix(xOneHot, oneHotFeatureNames, x.ColKeys, double.NaN);
            return tmp;
        }

        public static void ComputeAndWriteKernelMatrixFromSnpDataFile(string snpDataFile, ParallelOptions parallelOptions)
        {
            string postfix = "Cov";
            string outputFile = ShoUtils.AddPostfixToFileName(snpDataFile, postfix);
            ComputeAndWriteKernelMatrixFromSnpDataFile(snpDataFile, parallelOptions, outputFile, false, INPUT_ENCODING_TYPE.Null, false);
        }


        public static ShoMatrix ComputeAndWriteKernelMatrixFromSnpDataFile(string snpDataFile, ParallelOptions parallelOptions, string outputFile, bool doEigImp, INPUT_ENCODING_TYPE SNP_ENCODING, bool useCorrInstead)
        {

            ShoMatrix dat = ShoMatrix.CreateShoMatrixFromMatrixFile(snpDataFile, parallelOptions);//LoadDenseFileOfSingleCharacterDoubles(snpDataFile, double.NaN);

            if (doEigImp)
            {
                dat = dat.AsShoMatrix().Standardize(LoadData.GetMaxValOfSnpEncoding(SNP_ENCODING), parallelOptions);//.AsShoMatrix();
            }
            else
            {
                DoubleArray newDat = dat.AsShoMatrix().DoubleArray.ImputeNanValuesWithRowMedian(parallelOptions, true);
                dat.DoubleArray = newDat;
            }

            Helper.CheckCondition(!dat.DoubleArray.HasNaN(), "should not contain NaNs in data");
            DoubleArray K = ShoUtils.ComputeKernelMatrixFromInnerProductOfMeanCenteredData(dat.DoubleArray);//dat.DoubleArray.T.Multiply(dat.DoubleArray);
            if (useCorrInstead)
            {
                K = ShoUtils.CorrelationMatrixFromCovarianceMatrix(K);
            }
            Helper.CheckCondition(!K.HasNaN(), "should not contain NaNs in data");

            ShoMatrix tmp = new ShoMatrix(K, dat.ColKeys, dat.ColKeys, double.NaN);

            //string fileOutputString = tmp.ToString2D();
            //SpecialFunctions.WriteToFile(fileOutputString, outputFile);
            tmp.WriteToFile(outputFile);

            System.Console.WriteLine("Wrote covariance matrix to: " + outputFile);
            return tmp;
        }

        public static DenseAnsi CreateDenseAnsiMatrix(string filename, ParallelOptions parallelOptions)
        {
            Console.WriteLine("reading DenseAnsiNumericWrapperMatrix for " + filename);
            return DenseAnsi.GetInstance(filename, parallelOptions);
        }


        /// <summary>
        /// Load up one of Carl's dense files into a Sho DoubleArray
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="missingVal"></param>
        /// <returns></returns>
        public static Matrix<string, string, double> CreateDenseAnsiNumericWrapperMatrix(string filename, ParallelOptions parallelOptions)
        {
            Console.WriteLine("reading DenseAnsiNumericWrapperMatrix for " + filename);
            return LoadData.NumericWrapper(DenseAnsi.GetInstance(filename, parallelOptions));
            //var matrixFactorySSD = MatrixFactory<string, string, double>.GetInstance();
            //return matrixFactorySSD.Parse(filename, missingVal).AsShoMatrix();
        }


        /// <summary>
        /// This seems to work the same regardless of whether the first entry on the first row is a cid (strange format that Carl uses and hand-generated),
        /// or if one used Matrix.WriteDense(), which would have the string "row" as the first entry on the first row.
        /// </summary>
        /// <param name="kernelFile"></param>
        /// <param name="cids"></param>
        /// <returns></returns>
        public static ShoMatrix ReadKernelMatrixWithOrderedCids(string kernelFile, List<string> cids, ParallelOptions parallelOptions)
        {
            ShoMatrix x = ReadKernelMatrix(kernelFile, parallelOptions).AsShoMatrix();
            //now filter and reorder by the cidlist
            try
            {
                x = x.SelectRowsAndColsView(cids, cids).AsShoMatrix();
            }
            catch (KeyNotFoundException)
            {
                //List<string> kernelCids = x.RowKeys.ToList();
                //var keyIntersection = kernelCids.Intersect<string>(cids).ToList();
                HashSet<string> kernelCids = x.RowKeys.ToHashSet<string>();
                List<string> cidsIntersect = kernelCids.Intersect(cids).ToList();
                HashSet<string> cidsTmp = cids.ToHashSet<string>();
                int keySetDiff2 = cidsTmp.RemoveAll<string>(kernelCids);
                int keySetDiff1 = kernelCids.RemoveAll<string>(cids);

                System.Console.WriteLine("WARNING: kernel is missing " + cidsTmp.Count + " cids!");
                System.Console.WriteLine("Push a key to continue. Waiting...");
                System.Console.ReadKey();
                System.Console.WriteLine("... continuing");

                Helper.CheckCondition(cidsIntersect.ToHashSet<string>().IsProperSubsetOf(x.RowKeys), "intersection is not subset of cids for some strange reason");
                Helper.CheckCondition(cidsIntersect.ToHashSet<string>().IsProperSubsetOf(x.ColKeys), "intersection is not subset of cids for some strange reason");
                x = x.SelectRowsAndColsView(cidsIntersect, cidsIntersect).AsShoMatrix();
            }

            return x;
        }

        /// <summary>
        /// This seems to work the same regardless of whether the first entry on the first row is a cid (strange format that Carl uses and hand-generated),
        /// or if one used Matrix.WriteDense(), which would have the string "row" as the first entry on the first row.
        /// </summary>
        /// <param name="kernelFile"></param>
        /// <param name="cids"></param>
        /// <returns></returns>
        public static DoubleArray ReadKernelMatrixWithOrderedCidsToDoubleArray(string kernelFile, List<string> cids, ParallelOptions parallelOptions)
        {
            ShoMatrix x = ReadKernelMatrix(kernelFile, parallelOptions).AsShoMatrix();
            //now filter and reorder by the cidlist
            x = x.SelectRowsAndColsView(cids, cids).AsShoMatrix();

            return x.DoubleArray;
        }

        //useful for debugging the MatrixFactory....
        public static Matrix<string, string, double> ReadDenseMatrix(string filename, ParallelOptions parallelOptions)
        {
            Matrix<string, string, double> data;
            DenseMatrix<string, string, double>.TryParseRFileWithDefaultMissing(filename, double.NaN, parallelOptions, out data);
            return data;
        }

        //is this obsolete...?
        public static Matrix<string, string, double> ReadKernelMatrix(string kernelFile, ParallelOptions parallelOptions)
        {
            //double missingValue = Double.NaN;
            //Matrix<string, string, double> tmp = DenseMatrix<string, string, double>.ReadRFile(kernelFile, missingValue);
            //return tmp.AsShoMatrix();
            //throw new NotImplementedException("need to use Matrix Factory here to accomodate sparse files as well like Hyuns IBD");
            //ShoMatrix x = ShoMatrix.CreateShoDoubleMatrixFromMatrixFile(kernelFile);
            Matrix<string, string, double> x = LoadData.CreateMatrixFromMatrixFile(kernelFile, parallelOptions);
            try
            {
                /* this needed for the weird format I got from Hyun for the plink data--sparse, with only half the entries*/
                //x = x.FillInAndCheckSymmetricMatrix(1.0e-7);
            }
            catch (Exception)
            {
                System.Console.WriteLine("problem with file: " + kernelFile);
                System.Console.Out.Flush();
            }
            Helper.CheckCondition(x.CheckThatRowAndColKeysAreIdentical(), "row and col keys are not the same in the kernel matrix");
            Helper.CheckCondition(!x.IsMissingSome(), "kernel matrix contains NaNs");//.HasSomeMissingValues(), "kernel matrix contains NaNs");
            Helper.CheckCondition(x.IsSquare(), "kernel must be square");
            Helper.CheckCondition(x.IsSymmetric(1e-5), "kernel must be symmetric");
            return x;
        }

        public static string SnpEncodingString(INPUT_ENCODING_TYPE SNP_ENCODING)
        {
            string baseName = "_E";
            switch (SNP_ENCODING)
            {
                case INPUT_ENCODING_TYPE.RealValued:
                    return baseName + "R";
                case INPUT_ENCODING_TYPE.ZeroOne:
                    return baseName + "01";
                case INPUT_ENCODING_TYPE.ZeroOneTwo:
                    return baseName + "012";
            }
            throw new NotImplementedException();
        }


        public static INPUT_ENCODING_TYPE ConvertStringToInputEncoding(string INPUT_ENCODING_STR)
        {
            switch (INPUT_ENCODING_STR)
            {
                case "01":
                    return INPUT_ENCODING_TYPE.ZeroOne;
                case "012":
                    return INPUT_ENCODING_TYPE.ZeroOneTwo;
                case "real":
                    return INPUT_ENCODING_TYPE.RealValued;
            }
            return INPUT_ENCODING_TYPE.Null;
        }


        public static void CheckSnpDataEncoding(INPUT_ENCODING_TYPE SNP_ENCODING, Matrix<string, string, double> snpMatrix)
        {
            if (SNP_ENCODING == INPUT_ENCODING_TYPE.ZeroOne || SNP_ENCODING == INPUT_ENCODING_TYPE.ZeroOneTwo)
            {
                int maxVal = GetMaxValOfSnpEncoding(SNP_ENCODING);
                IEnumerable<double> uniqueSnpVals = snpMatrix.Unique();
                Helper.CheckCondition(uniqueSnpVals.Max() == maxVal);
                Helper.CheckCondition(uniqueSnpVals.Min() == 0);
                Helper.CheckCondition(uniqueSnpVals.Count() <= (maxVal + 2));
            }
            else
            {
                throw new Exception("cannot check this SNP encoding");
            }
        }

        private static void LoadGWASData(string snpFileName, string phenFileName, out DoubleArray snpData, out DoubleArray phenotypeData,
                                         out List<string> snpIds, out List<string> cids, out int numSNP, out int numInd,
                                         INPUT_ENCODING_TYPE SNP_ENCODING, bool STANDARDIZE, ParallelOptions parallelOptions)
        {
            DenseAnsi denseCollection = DenseAnsi.GetInstance(snpFileName, parallelOptions);
            DenseAnsi denseCollection2 = DenseAnsi.GetInstance(phenFileName, parallelOptions);

            var matrixFactorySSD = MatrixFactory<string, string, double>.GetInstance();
            Double missingVal = double.NaN;
            ShoMatrix snpMatrix = matrixFactorySSD.Parse(snpFileName, missingVal, parallelOptions).AsShoMatrix();
            ShoMatrix phenMatrix = matrixFactorySSD.Parse(phenFileName, missingVal, parallelOptions).AsShoMatrix();

            //Create two new matrices which have the common cids, in the order of the first matrix.
            IEnumerable<string> commonCids = snpMatrix.ColKeys.Where(x => phenMatrix.IndexOfColKey.ContainsKey(x));

            //remove one of the twins so that matrixes aren't rank deficient (speculating that this might make a difference)
            //string oneTwinCid ="10000087";
            //commonCids = commonCids.Where(x => !x.Equals(oneTwinCid));

            snpMatrix = snpMatrix.SelectColsView(commonCids).AsShoMatrix();
            phenMatrix = phenMatrix.SelectColsView(commonCids).AsShoMatrix();

            cids = commonCids.ToList();


            //check that data matches the encoding we specified since the Standardization depends on this 
            CheckSnpDataEncoding(SNP_ENCODING, snpMatrix);

            //standardize the way Eigenstrat does:
            int maxMatrixVal = GetMaxValOfSnpEncoding(SNP_ENCODING);//assumes that data are integ
            if (STANDARDIZE)
            {
                snpMatrix = snpMatrix.Standardize(maxMatrixVal, parallelOptions).AsShoMatrix(); //Carl's new line
                //snpMatrix = PSDnaDistMain.Standardize(snpMatrix, maxMatrixVal).ToShoMatrix(); //My old line
            }

            snpData = snpMatrix.DoubleArray;
            phenotypeData = phenMatrix.DoubleArray;

            snpIds = snpMatrix.RowKeys.ToList();

            numSNP = snpMatrix.RowCount;
            numInd = snpMatrix.ColCount;
        }

        public static bool InputEncodingIsSnpInteger(INPUT_ENCODING_TYPE SNP_ENCODING)
        {
            if (SNP_ENCODING == INPUT_ENCODING_TYPE.ZeroOne || SNP_ENCODING == INPUT_ENCODING_TYPE.ZeroOneTwo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int GetMaxValOfSnpEncoding(INPUT_ENCODING_TYPE SNP_ENCODING)
        {
            int maxVal = -100;
            if (InputEncodingIsSnpInteger(SNP_ENCODING))
            {
                if (SNP_ENCODING == INPUT_ENCODING_TYPE.ZeroOne)
                {
                    maxVal = 1;
                }
                else if (SNP_ENCODING == INPUT_ENCODING_TYPE.ZeroOneTwo)
                {
                    maxVal = 2;
                }
                return maxVal;
            }
            throw new Exception("cannot get max integer for this INPUT_ENCODING_TYPE");
        }

        public static ShoMatrix ComputeDiagonalCovarianceWithWeights(Matrix<string, string, double> snpMatrix, DoubleArray weightForEachFeature, out DoubleArray dataMean, ParallelOptions parallelOptions)
        {
            List<double> tmpDataMean;
            ShoMatrix tmp = ComputeDiagonalCovarianceWithWeights(snpMatrix, weightForEachFeature, out tmpDataMean, parallelOptions);
            dataMean = DoubleArray.From(tmpDataMean);
            return tmp;
        }

        public static ShoMatrix ComputeCovarianceWithWeights(Matrix<string, string, double> snpMatrix, DoubleArray weightForEachFeature, out DoubleArray dataMean, ParallelOptions parallelOptions)
        {
            List<double> tmpDataMean;
            ShoMatrix tmp = ComputeCovarianceWithWeights(snpMatrix, weightForEachFeature, out tmpDataMean, parallelOptions);
            dataMean = DoubleArray.From(tmpDataMean);
            return tmp;
        }

        public static ShoMatrix ComputeDiagonalCovarianceWithWeights(Matrix<string, string, double> snpMatrix, DoubleArray weightForEachFeature, out List<double> dataMean, ParallelOptions parallelOptions)
        {
            ShoMatrix Kgenotype;
            bool useCorrel = false;
            var psiMatrix = EigenstratMain.CreatePsiTheMatrixOfDiagonalCovarianceValuesWithWeights(useCorrel, snpMatrix, weightForEachFeature,/*isOKToDestroyXMatrix*/ false, parallelOptions, out dataMean);
            Kgenotype = psiMatrix.ToShoMatrix();
            return Kgenotype;
        }

        /// <summary>
        /// Note: do not make this, and the weightless version call the same code, because then the weightless one will be much more inefficient than it needs to be
        /// </summary>
        /// <param name="snpMatrix"></param>
        /// <param name="weightForEachFeature"></param>
        /// <param name="parallelOptions"></param>
        /// <returns></returns>
        public static ShoMatrix ComputeCovarianceWithWeights(Matrix<string, string, double> snpMatrix, DoubleArray weightForEachFeature, out List<double> dataMean, ParallelOptions parallelOptions)
        {
            ShoMatrix Kgenotype;
            bool useCorrel = false;
            var psiMatrix = EigenstratMain.CreatePsiTheMatrixOfCovarianceValuesWithWeights(useCorrel, snpMatrix, weightForEachFeature,/*isOKToDestroyXMatrix*/ false, parallelOptions, out dataMean);
            Kgenotype = psiMatrix.ToShoMatrix();
            return Kgenotype;
        }

        public static DoubleArray ComputeCovariance(DoubleArray snpData, ParallelOptions parallelOptions)
        {
            ShoMatrix snpMatrix = ShoMatrix.CreateDefaultRowAndColKeyInstance(snpData, double.NaN);
            ShoMatrix Kgenotype;
            bool useCorrel = false;
            var psiMatrix = EigenstratMain.CreatePsiTheMatrixOfCovarianceValues(useCorrel, snpMatrix, /*isOKToDestroyXMatrix*/ false, parallelOptions);

            Kgenotype = psiMatrix.ToShoMatrix();
            return Kgenotype.DoubleArray;
        }

        public static ShoMatrix ComputeCovariance(Matrix<string, string, double> snpMatrix, ParallelOptions parallelOptions)
        {
            return ComputeCovariance(snpMatrix, parallelOptions, false);
        }

        public static ShoMatrix ComputeCovariance(Matrix<string, string, double> snpMatrix, ParallelOptions parallelOptions, bool isOkToDestroyMatrix)
        {
            ShoMatrix Kgenotype;
            bool useCorrel = false;
            var psiMatrix = EigenstratMain.CreatePsiTheMatrixOfCovarianceValues(useCorrel, snpMatrix, isOkToDestroyMatrix, parallelOptions);

            Kgenotype = psiMatrix.ToShoMatrix();
            return Kgenotype;
        }

        public static ShoMatrix ComputeCorrelation(Matrix<string, string, double> snpMatrix, ParallelOptions parallelOptions)
        {
            return ComputeCorrelation(snpMatrix, parallelOptions, false);
        }

        public static ShoMatrix ComputeCorrelation(Matrix<string, string, double> snpMatrix, ParallelOptions parallelOptions, bool isOkToDestroyXMatrix)
        {
            ShoMatrix Kgenotype;
            bool useCorrel = true;
            var psiMatrix = EigenstratMain.CreatePsiTheMatrixOfCovarianceValues(useCorrel, snpMatrix, isOkToDestroyXMatrix, parallelOptions);

            Kgenotype = psiMatrix.ToShoMatrix();
            return Kgenotype;
        }

        public static void ComputeSVDandEigenProjections(DoubleArray trainSnpData, out DoubleArray K, out SVD svdSnp, out DoubleArray trainProj)
        {
            System.Console.Write("Computing SVD data...");
            K = trainSnpData.T.Multiply(trainSnpData);  //snpData'*snpData
            svdSnp = new SVD(K); //[U D V]=[U D U.T]=svd(K)=[A S1 V1] in matlab code
            trainProj = svdSnp.U.Multiply(svdSnp.D.Sqrt()).T; //principleCoord=(A*sqrt(S1))'; //projections of each individual               
            System.Console.WriteLine("...finished.");
        }







        public static void PreProcessDataForGwas(double maxFracInputsMissing, bool doEigImpNorm, INPUT_ENCODING_TYPE SNP_ENCODING, ParallelOptions parallelOptions, ref Matrix<string, string, double> snpMatrix,
                                                        ref Matrix<string, string, double> phenMatrixTmp, bool removeRowsThatDontVary)
        {
            Matrix<string, string, double> secondPredMatrix = null;
            PreProcessDataForGwas(maxFracInputsMissing, doEigImpNorm, SNP_ENCODING, parallelOptions, ref snpMatrix, ref secondPredMatrix, ref phenMatrixTmp, removeRowsThatDontVary);
        }

        // To see the cids, do snpMatrix.ColKeys or phenMatrixTmp.ColKeys
        public static void PreProcessDataForGwas(double maxFracInputsMissing, bool doEigImpNorm, INPUT_ENCODING_TYPE SNP_ENCODING, ParallelOptions parallelOptions, ref Matrix<string, string, double> snpMatrix,
                                                         ref Matrix<string, string, double> secondPredMatrix, ref Matrix<string, string, double> phenMatrixTmp, bool removeRowsThatDontVary,
                                                         bool checkForLinearDependence = false, Matrix<string, string, double> fixedEffectsMatrix = null)
        {

            if (!_QUIET) System.Console.Write("PreprocessingDataForGwas...");
            //to avoid compiler problems with anonymous functions:
            Matrix<string, string, double> phenMatrix = phenMatrixTmp;

            //remove individuals with missing phenotypes when using real phenotype data
            if (null != phenMatrix && phenMatrix.RowCount == 1)
            {
                string phenVar = phenMatrix.RowKeys[0];//name of the row feature
                //Get the cases that have a value
                IEnumerable<string> nonMissingCases = phenMatrix.ColKeys.Where(cid => !phenMatrix.IsMissing(phenVar, cid));
                //Create a new ShoMatrix
                phenMatrix = phenMatrix.SelectColsView(nonMissingCases);//.AsShoMatrix();
                //Helper.CheckCondition(phenMatrix.DoubleArray.Find(x => x == phenMatrix.MissingValue).IsEmpty(), "Some phenotypes have missing values");
                Helper.CheckCondition(nonMissingCases.Count() == phenMatrix.ColCount, "Some phenotypes have missing values");
            }

            List<string> cids = null;
            bool cidsInSameOrder;
            if (snpMatrix != null)
            {
                cids = snpMatrix.ColKeys.ToList();
                if (phenMatrix != null)
                {
                    cids = MatrixExtensionsJenn.IntersectOnColKeys(ref snpMatrix, ref phenMatrix);
                    //can I assume the cols are in the same order as each other?  Let's double check:
                    cidsInSameOrder = MatrixExtensionsJenn.CheckColKeysInSameOrder(snpMatrix, phenMatrix);
                    if (!cidsInSameOrder) throw new Exception("cids are not in the same order");
                }
                if (secondPredMatrix != null)
                {
                    cids = MatrixExtensionsJenn.IntersectOnColKeys(ref snpMatrix, ref secondPredMatrix);

                    if (phenMatrix != null)
                    {
                        cids = MatrixExtensionsJenn.IntersectOnColKeys(ref secondPredMatrix, ref phenMatrix);
                        cidsInSameOrder = MatrixExtensionsJenn.CheckColKeysInSameOrder(secondPredMatrix, phenMatrix);
                        if (!cidsInSameOrder) throw new Exception("cids are not in the same order");

                        // This check is almost certainly redundant, but let's be safe
                        cidsInSameOrder = MatrixExtensionsJenn.CheckColKeysInSameOrder(snpMatrix, phenMatrix);
                        if (!cidsInSameOrder) throw new Exception("cids are not in the same order");
                    }

                    cidsInSameOrder = MatrixExtensionsJenn.CheckColKeysInSameOrder(snpMatrix, secondPredMatrix);
                    if (!cidsInSameOrder) throw new Exception("cids are not in the same order");
                }
            }
            else
            {
                cids = phenMatrix.ColKeys.ToList();
            }

            // The second predictor file will not be imputed so it cannot contain missing data
            Helper.CheckCondition(secondPredMatrix == null || !secondPredMatrix.IsMissingSome(), "Second predictor file cannot contain missing data");

            //to get around compiling errors
            Matrix<string, string, double> tmpSnpMatrix = snpMatrix;

            //filter out SNPs that have too much missing data:
            if (maxFracInputsMissing < 1 && snpMatrix != null)
            {
                //this bit of code finds out which snps to keep
                var snpsToKeep =
                    from snp in snpMatrix.RowKeys.AsParallel().AsOrdered()
                    //from snp in tmpSnpMatrix.RowKeys
                    let rowView = tmpSnpMatrix.RowView(snp)
                    let nonMissingCount = rowView.Count()
                    let nonMissingFraction = (double)nonMissingCount / (double)tmpSnpMatrix.ColCount
                    where nonMissingFraction > 1 - maxFracInputsMissing //>0.95 as Carl wrote it
                    select snp;

                snpMatrix = snpMatrix.SelectRowsView(snpsToKeep).AsShoMatrix();
            }

            //remove any SNPs that don't vary (otherwise there is an inverse that crashes later:  (X.T.Multiply(X)).Inv();
            if (removeRowsThatDontVary && snpMatrix != null)
            {
                List<int> snpsThatVary = snpMatrix.FindRowsThatVary();
                snpMatrix = snpMatrix.SelectRowsView(snpsThatVary).AsShoMatrix();
            }
            if (removeRowsThatDontVary && secondPredMatrix != null)
            {
                List<int> predsThatVary = secondPredMatrix.FindRowsThatVary();
                secondPredMatrix = secondPredMatrix.SelectRowsView(predsThatVary).AsShoMatrix();
            }

            if (doEigImpNorm && snpMatrix != null)
            {
                if (snpMatrix is DenseAnsi) throw new Exception("cannot standardize dense collections--as a precautionary measure of loading up huge data sets-- would need to first convert it to something with a double");
                //standardize the way Eigenstrat does:
                Helper.CheckCondition(InputEncodingIsSnpInteger(SNP_ENCODING));
                snpMatrix = snpMatrix.AsShoMatrix().Standardize(LoadData.GetMaxValOfSnpEncoding(SNP_ENCODING), parallelOptions);//.AsShoMatrix();
            }


            // Remove any predictors that are linearly dependent with the fixed effects and bias (otherwise there is an inverse that crashes later:  (X.T.Multiply(X)).Inv();
            // This does not check pairs of predictors if two predictors are being used
            if (checkForLinearDependence)
            {
                Console.WriteLine("Checking for linear dependence");

                // !!! Adding bias and taking the cid intersection is done again later in LMMconsole after this method is called, can the redundancy be removed?
                // Add bias to fixed effects
                ShoMatrix fixedEffectsWithBias;
                if (fixedEffectsMatrix != null)
                {
                    fixedEffectsWithBias = fixedEffectsMatrix.AsShoMatrix().AddBiasRow().AsShoMatrix();
                }
                else
                {
                    fixedEffectsWithBias = new ShoMatrix(ShoUtils.DoubleArrayOnes(1, cids.Count), new List<string> { "bias" }, cids, double.NaN);
                }
                // If the fixed effects matrix is not full rank then all predictors will be removed below
                Helper.CheckCondition(fixedEffectsWithBias.DoubleArray.IsFullRank(), "Fixed effects matrix with bias row must be full rank to check for linear dependence");

                if (snpMatrix != null)
                {
                    Helper.CheckCondition(!snpMatrix.IsMissingSome(), "SNP matrix cannot be missing data when checking for linear dependence");
                    MatrixExtensionsJenn.IntersectOnColKeys(ref snpMatrix, ref fixedEffectsWithBias);
                    Helper.CheckCondition(MatrixExtensionsJenn.CheckColKeysInSameOrder(snpMatrix, fixedEffectsWithBias), "cids are not in the same order");
                    snpMatrix = snpMatrix.RemoveLinearlyDependentRows(fixedEffectsWithBias);
                }
                if (secondPredMatrix != null)
                {
                    Helper.CheckCondition(!secondPredMatrix.IsMissingSome(), "SNP matrix cannot be missing data when checking for linear dependence");
                    MatrixExtensionsJenn.IntersectOnColKeys(ref secondPredMatrix, ref fixedEffectsWithBias);
                    Helper.CheckCondition(MatrixExtensionsJenn.CheckColKeysInSameOrder(secondPredMatrix, fixedEffectsWithBias), "cids are not in the same order");
                    secondPredMatrix = secondPredMatrix.RemoveLinearlyDependentRows(fixedEffectsWithBias);
                }
            }


            if (snpMatrix != null && snpMatrix is ShoMatrix) snpMatrix = snpMatrix.AsShoMatrix();
            if (secondPredMatrix != null && secondPredMatrix is ShoMatrix) secondPredMatrix = secondPredMatrix.AsShoMatrix();
            if (phenMatrix != null && phenMatrix is ShoMatrix) phenMatrix = phenMatrix.AsShoMatrix();

            if (!_QUIET) System.Console.WriteLine("done.");
        }

        public static Matrix<string, string, string> CreateStringMatrixFromMatrixFile(string dataFile, string missingVal, ParallelOptions parallelOptions)
        {
            MatrixFactory<string, string, string> myFactory = MatrixFactory<string, string, string>.GetInstance();
            Matrix<string, string, string> tmp = myFactory.Parse(dataFile, missingVal, parallelOptions);
            tmp = tmp.CheckAndPossiblyFixKeysForTrailingSpaces(true);
            return tmp;
        }

        public static Matrix<string, string, string> CreateStringMatrixFromMatrixFile(string dataFile, ParallelOptions parallelOptions, string missingValue)
        {
            MatrixFactory<string, string, string> myFactory = MatrixFactory<string, string, string>.GetInstance();
            Matrix<string, string, string> tmp = myFactory.Parse(dataFile, missingValue, parallelOptions);
            tmp = tmp.CheckAndPossiblyFixKeysForTrailingSpaces(true);
            return tmp;
        }

        public static Matrix<string, string, string> CreateStringMatrixFromMatrixFile(string dataFile, ParallelOptions parallelOptions)
        {
            return CreateStringMatrixFromMatrixFile(dataFile, parallelOptions, "");
        }


        public static Matrix<string, string, double> CreateMatrixFromMatrixFile(string dataFile, ParallelOptions parallelOptions)
        {
            //Console.WriteLine("reading in Matrix file: " + dataFile);
            MatrixFactory<string, string, double> myFactory = MatrixFactory<string, string, double>.GetInstance();
            Matrix<string, string, double> tmp = myFactory.Parse(dataFile, double.NaN, parallelOptions);
            tmp = tmp.CheckAndPossiblyFixKeysForTrailingSpaces(true);
            return tmp;
        }

        public static Matrix<string, string, char> CreateCharMatrixFromMatrixFile(string dataFile, ParallelOptions parallelOptions)
        {
            MatrixFactory<string, string, char> myFactory = MatrixFactory<string, string, char>.GetInstance();
            Matrix<string, string, char> tmp = myFactory.Parse(dataFile, '?', parallelOptions);
            tmp = tmp.CheckAndPossiblyFixKeysForTrailingSpaces(true);
            return tmp;
        }

        public static Matrix<string, string, double> NumericWrapper(DenseAnsi x)
        {
            Matrix<string, string, double> tmp = x.ConvertValueView(Bio.Util.ValueConverter.CharToDouble, double.NaN);
            tmp = tmp.CheckAndPossiblyFixKeysForTrailingSpaces(true);
            return tmp;
        }

        /// <summary>
        /// Read in file given from a SQL dump of rs#s and chrom num and chrom position
        ///
        ///       rs      chrom   pos
        ///       rs11818629      10   130027095
        ///       rs11818641      10   26232872
        ///
        /// </summary>
        /// <param name="snpMapFile"></param>
        /// <param name="M"></param>
        /// <returns></returns>
        public static Dictionary<string, Tuple<int, int>> GetSnpMappingFromTabFile(string snpMapFile, int M, ParallelOptions parallelOptions)
        {
            Dictionary<string, Tuple<int, int>> snpMappingChromNumPosition = new Dictionary<string, Tuple<int, int>>(M);

            string header = "rs\tchrom\tpos";
            Dictionary<string, KeyValuePair<int, int>> snpMapDict = new Dictionary<string, KeyValuePair<int, int>>();
            foreach (Dictionary<string, string> row in SpecialFunctions.TabFileTable(snpMapFile, header, '\t'))
            //Parallel.ForEach(SpecialFunctions.TabFileTable(snpMapFile, header, ' '), parallelOptions, row =>
            {
                //string chromNumString = row["chrom"].Substring(3);
                string chromNumString = row["chrom"];
                int chromNum;
                try
                {
                    chromNum = int.Parse(chromNumString);
                }
                catch (Exception)
                {//if it was X or Y or W, then re-code as 23,24,25
                    if (chromNumString == "X")
                    {
                        chromNum = 23;
                    }
                    else if (chromNumString == "Y")
                    {
                        chromNum = 24;
                    }
                    else if (chromNumString == "W")
                    {
                        chromNum = 25;
                    }
                    else
                    {
                        throw new Exception("Chromosome found that is not in the set 1-22, X, Y, W");
                    }
                }
                int chromPos = int.Parse(row["pos"]);
                snpMappingChromNumPosition.Add(row["rs"], Tuple.Create(chromNum, chromPos));
                //Console.WriteLine(row["rs"] + ", " + chromNum + ", " + chromPos);
            }
            //);
            return snpMappingChromNumPosition;
        }


        /// <summary>
        /// Using this for the MDP (mouse data) where each mouse is homozygous for the SNPs, and I have converted
        ///ACTG? to 0123/NaN, and now want 0/1 for each SNP. This returns false if any row contains more than two unique values.
        ///Otherwise it uses a binary encoding 0/1 where 0 is the wild type allele
        /// </summary>
        /// <param name="snpMatrix"></param>
        public static void Convert0123SnpEncodingToBinary(Matrix<string, string, double> snpMatrix, ParallelOptions parallelOptions)
        {
            int R = snpMatrix.RowCount;
            int C = snpMatrix.ColCount;

            Console.WriteLine("converting features from 0123 to binary");

            Parallel.For(0, R, r =>
            {
                Console.Write(string.Format("{0}/{1}", r, R));
                DoubleArray thisRow = snpMatrix.SelectRowsView(r).ToShoMatrix().DoubleArray;
                List<KeyValuePair<double, int>> histogram = thisRow.HistogramValuesInDescendingOrderIgnoringNan();
                if (histogram.Count > 2)
                {//there is a SNP that takes on more than two values
                    throw new Exception("feature " + snpMatrix.RowKeys[r] + " has more than two values so cannot convert it to binary");
                }
                //use major allele: 0, minor allele: 1
                for (int c = 0; c < C; c++)
                {
                    double thisVal = thisRow[c];
                    if (thisVal == histogram[0].Key)
                    {
                        snpMatrix[r, c] = 0;
                    }
                    else if (double.IsNaN(thisVal))
                    {
                        snpMatrix.SetValueOrMissing(r, c, double.NaN);
                    }
                    else if (thisVal == histogram[1].Key)
                    {
                        snpMatrix[r, c] = 1;
                    }
                    else
                    {
                        throw new Exception("error in conversion");
                    }
                }
            }
         );

        }


        public static Dictionary<string, string> ParseTabFileWithHeaderOneColToOtherCol(string tabFile, string keyName, string valName, bool checkButIgnoreDuplicateKeys, bool caseInsensitiveStrings)
        {
            Dictionary<string, string> keyToVal = new Dictionary<string, string>();

            using (TextReader textReader = File.OpenText(tabFile))
            {
                string[] header = textReader.ReadLine().Split('\t');
                int numCols = header.Count();
                int keyCol = header.FindIndex(elt => (elt == keyName)); Helper.CheckCondition(keyCol != -1, "could not find column heading " + keyName + " in " + tabFile);
                int valCol = header.FindIndex(elt => (elt == valName)); Helper.CheckCondition(valCol != -1, "could not find column heading " + valName + " in " + tabFile);

                string line = null;
                while (null != (line = textReader.ReadLine()))
                {
                    if (caseInsensitiveStrings) line = line.ToLowerInvariant();
                    string[] lineItems = line.Split('\t');

                    string thisKey = lineItems[keyCol];
                    string thisVal = lineItems[valCol];

                    string valAlreadyIn;
                    bool keyExists = keyToVal.TryGetValue(thisKey, out valAlreadyIn);
                    if (keyExists)
                    {
                        Helper.CheckCondition(valAlreadyIn == thisVal, "key exists, and next instance of it does not have a value that matches");
                    }
                    else
                    {
                        keyToVal.Add(thisKey, thisVal);
                    }
                }
            }
            return keyToVal;
        }

        public static Dictionary<string, List<string>> ParseTabFileWithHeaderOneColToOtherColSet(string tabFile, string colNameKey, string colNameSet, bool caseInsensitiveStrings)
        {
            Dictionary<string, List<string>> colToSet = new Dictionary<string, List<string>>();

            using (TextReader textReader = File.OpenText(tabFile))
            {
                string[] header = textReader.ReadLine().Split('\t');
                int numCols = header.Count();
                int keyCol = header.FindIndex(elt => (elt == colNameKey)); Helper.CheckCondition(keyCol != -1, "could not find column heading " + colNameKey + " in " + tabFile);
                int setCol = header.FindIndex(elt => (elt == colNameSet)); Helper.CheckCondition(setCol != -1, "could not find column heading " + colNameSet + " in " + tabFile);

                string line = null;
                while (null != (line = textReader.ReadLine()))
                {
                    if (caseInsensitiveStrings)
                    {
                        line = line.ToLowerInvariant();
                    }
                    string[] lineItems = line.Split('\t');

                    string thisKey = lineItems[keyCol];
                    string thisVal = lineItems[setCol];

                    throw new NotImplementedException("need to fix error here");
                    //colToSet.AddValueToDictionaryWithValueList(thisKey, thisVal);
                }

            }
            return colToSet;
        }


        public static Dictionary<string, List<string>> ParseTabFileWithHeaderOneColToOtherColSetFromDelimitedCol(string tabFile, string colNameKey, string colNameSet, char delimiter, bool caseInsensitiveStrings)
        {
            Dictionary<string, List<string>> colToSet = new Dictionary<string, List<string>>();

            using (TextReader textReader = File.OpenText(tabFile))
            {
                string[] header = textReader.ReadLine().Split('\t');
                int numCols = header.Count();
                int keyCol = header.FindIndex(elt => (elt == colNameKey)); Helper.CheckCondition(keyCol != -1, "could not find column heading " + colNameKey + " in " + tabFile);
                int setCol = header.FindIndex(elt => (elt == colNameSet)); Helper.CheckCondition(setCol != -1, "could not find column heading " + colNameSet + " in " + tabFile);

                string line = null;
                while (null != (line = textReader.ReadLine()))
                {
                    if (caseInsensitiveStrings)
                    {
                        line = line.ToLowerInvariant();
                    }
                    string[] lineItems = line.Split('\t');

                    string thisKey = lineItems[keyCol];
                    string thisVal = lineItems[setCol];

                    colToSet.Add(thisKey, thisVal.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries).ToList());
                }

            }
            return colToSet;
        }


    }
    public enum LeaveDataOnDisk
    {
        No,
        Try,
        IndexFileOnly
    }

}
