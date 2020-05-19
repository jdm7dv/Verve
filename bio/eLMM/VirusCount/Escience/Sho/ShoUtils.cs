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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MBT.Escience;
using Bio.Util;
using MBT.Escience.Matrix;
using Bio.Matrix;
using ShoNS;
using ShoNS.Array;
using ShoNS.Optimizer;
using ShoNS.MathFunc;
using ShoNS.Stats;
using System.Threading.Tasks;
using ShoNS.Pickling;
//using ShoNS.Optimizer;
//using ShoNS.Visualization;
//using ShoNS.Pickling;

namespace MBT.Escience.Sho
{
    public static class ShoUtils
    {
        //Can't use this because of parallelization (Carl told me about this) --instead, pass Random objects to everything that are created elsewhere
        //private static int _seed = 123456;
        private static double _LOG2PI = Math.Log(Math.PI * 2);
        private static double _LOGPI = Math.Log(Math.PI);

        private static int ShoProcCount = -1;
        private static bool ShoEnvVarHasBeenInitialized = false;

        static ShoUtils()
        {
            //SetShoDirEnvironmentVariable(Environment.ProcessorCount, quiet: true);
            //setting to 1 always because of problems i've had with multi-threaded MKL in the past
            SetShoDirEnvironmentVariable(1, quiet: true);
        }



        public static DoubleArray CorrelationMatrixFromCovarianceMatrix(DoubleArray cov)
        {
            Helper.CheckCondition(cov.IsSymmetric(1e-5));
            Helper.CheckCondition(cov.IsSquare());
            int N = cov.size0;
            DoubleArray corr = new DoubleArray(N, N);
            for (int n0 = 0; n0 < N; n0++)
            {
                corr[n0, n0] = 1;
                for (int n1 = n0 + 1; n1 < N; n1++)
                {
                    corr[n0, n1] = cov[n0, n1] / Math.Sqrt(cov[n0, n0] * cov[n1, n1]);
                    corr[n1, n0] = corr[n0, n1];//symmmetry
                }
            }
            Helper.CheckCondition(corr.Max(0).Max(1).ToScalar() <= 1);
            Helper.CheckCondition(corr.Min(0).Min(1).ToScalar() >= -1);
            return corr;
        }


        /// <summary>
        /// Wrap sho optimizer so that we can actually get the function value out at the minimum...
        /// </summary>
        /// <param name="initParams"></param>
        /// <param name="myOpt"></param>
        /// <param name="myObjGradDelegate"></param>
        /// <param name="objectiveValueAtMin"></param>
        /// <returns></returns>
        public static DoubleArray MinimizeAndGetObjectiveFunctionValue(DoubleArray initParams, ShoNS.Optimizer.QuasiNewton myOpt, DiffFunc myObjGradDelegate, out double objectiveValueAtMin)
        {
            double[] newParams = myOpt.Minimize(myObjGradDelegate, initParams);
            //bool didConverge;
            IList<double> tmpGradOut = new double[newParams.Count()];
            objectiveValueAtMin = myObjGradDelegate(newParams, tmpGradOut);
            if (false)//(!didConverge)
            {
                //throw new Exception("optimizer did not converge");
                Console.Error.WriteLine("optimizer did not converge");
                Console.WriteLine("optimizer did not converge");
            }
            return DoubleArray.From(newParams);
        }

        public static double TakeAverageOrSingletonIfOneIsNaN(double freqA1, double freqA2)
        {
            double thisAverage;
            if (!double.IsNaN(freqA1) && !double.IsNaN(freqA2))
            {
                thisAverage = (freqA1 + freqA2) / 2.0;
            }
            else if (!double.IsNaN(freqA1))
            {
                thisAverage = freqA1;
            }
            else
            {
                thisAverage = freqA2;
            }
            return thisAverage;
        }







        public static DoubleArray AddBiasColLastToInputData(DoubleArray inputData, int numInd)
        {
            DoubleArray biasTerm = new DoubleArray(numInd, 1);
            biasTerm.FillValue(1.0);
            if (null != inputData)
            {
                inputData = ShoUtils.CatArrayCol(inputData, biasTerm);
            }
            else
            {
                inputData = biasTerm;
            }
            return inputData;
        }

        public static DoubleArray AddBiasRowLastToInputData(DoubleArray inputData, int numInd)
        {
            DoubleArray biasTerm = new DoubleArray(1, numInd);
            biasTerm.FillValue(1.0);
            if (null != inputData)
            {
                inputData = ShoUtils.CatArrayRow(inputData, biasTerm);
            }
            else
            {
                inputData = biasTerm;
            }
            return inputData;
        }


        //Does one hot encoding of values in v. Assumes no missing data. See function below (not fully implemented) if want to handle missing data
        /// </summary>
        public static DoubleArray OneHotEncoding(DoubleArray v, int maxNumValuesExpected)
        {
            Helper.CheckCondition(v.IsVector());
            Helper.CheckCondition(!v.HasNaN(), "data has Nans so cannot 1-hot encode");
            DoubleArray possibleValues = v.Unique();
            int numVals = possibleValues.Length;
            Helper.CheckCondition(numVals <= maxNumValuesExpected, "number of values to encode is larger than expected");

            Dictionary<double, int> mapping = new Dictionary<double, int>();
            for (int j = 0; j < numVals; j++) mapping.Add(possibleValues[j], j);

            DoubleArray vHot;
            bool doTranspose = false;
            if (v.size0 > 1) doTranspose = true;
            vHot = new DoubleArray(possibleValues.Length, v.Length);
            for (int i = 0; i < v.Length; i++)
            {
                if (mapping.ContainsKey(v[i]))
                {
                    vHot[mapping[v[i]], i] = 1.0;
                }
                else
                {
                    throw new Exception("should not get to here");
                }
            }
            if (doTranspose) vHot = vHot.T;
            return vHot;
        }



        /// <summary>
        /// Uses algorithm specified in http://www.psych.nyu.edu/cohen/Calc_ANOVA.pdf)
        /// </summary>
        /// <param name="dataOrganizedByCells"></param>
        /// <param name="fstatInteraction"></param>
        /// <returns></returns>
        public static double Anova2UnbalancedInteractionTest(DoubleArray[,] dataOrganizedByCells, out double fstatInteraction)
        {

            //----------------------------------- 
            /* test case from paper,  Analysis of variance with unbalanced data: an update for ecology & evolution, 2010, 
                                      Andy Hector*, Stefanie von Felten and Bernhard Schmid   
                                      http://www3.interscience.wiley.com/cgi-bin/fulltext/123201865/PDFSTART        
            Should give:
            residualSS =  747.7500
            Nh =  10.1053
            SSbetween = 5.4165e+003
            SSrows =  4.8079e+003
            Scols =  597.1974
            SSinteraction =   11.4079
            fstatInteraction = 0.1068
            pVal = 0.7534
            */
            //dataOrganizedByCells = new DoubleArray[2, 2];
            //dataOrganizedByCells[0, 0] = new DoubleArray(new List<double>(){50, 57 });
            //dataOrganizedByCells[0, 1] = new DoubleArray(new List<double>() {57, 71, 85 });
            //dataOrganizedByCells[1, 0] = new DoubleArray(new List<double>() {91, 94, 102, 110 });
            //dataOrganizedByCells[1, 1] = new DoubleArray(new List<double>() {120, 105});
            //-----------------------------------

            int L1 = dataOrganizedByCells.GetLength(0);//number of levels for factor 1
            int L2 = dataOrganizedByCells.GetLength(1);//number of levels for factor 2
            int numCells = L1 * L2;

            Helper.CheckCondition(L1 > 0 && L2 > 0, "must have at least 1 level for each factor");

            DoubleArray cellTotals = ShoUtils.DoubleArrayZeros(L1, L2);
            DoubleArray numPerCell = ShoUtils.DoubleArrayZeros(L1, L2);
            for (int e = 0; e < L1; e++)
            {
                for (int d = 0; d < L2; d++)
                {
                    cellTotals[e, d] = dataOrganizedByCells[e, d].Sum();
                    numPerCell[e, d] = dataOrganizedByCells[e, d].Count;
                }
            }
            DoubleArray cellMeans = cellTotals.ElementDivide(numPerCell);
            double N = numPerCell.Sum();

            double residualSS = 0;
            for (int e = 0; e < L1; e++)
            {
                for (int d = 0; d < L2; d++)
                {
                    residualSS += dataOrganizedByCells[e, d].VarN() * numPerCell[e, d];
                }
            }

            DoubleArray rowMeans = cellMeans.Mean(DimOp.EachRow);
            DoubleArray colMeans = cellMeans.Mean(DimOp.EachCol);

            //harmonic mean to get effective N (see, for e.g. http://www.psych.nyu.edu/cohen/Calc_ANOVA.pdf)
            double Nh = Math.Pow(numCells, 2) / numPerCell.DotInv().Sum();

            double SSbetween = Nh * cellMeans.ToVector().VarN();
            double SSrow = Nh * rowMeans.VarN();
            double SScol = Nh * colMeans.VarN();
            double SSinteraction = SSbetween - SSrow - SScol;

            double dof1 = L1 - 1;
            double dof2 = L2 - 1;
            double dofInteraction = dof1 * dof2;
            double dofResidual = N - L2 * L1;

            fstatInteraction = (SSinteraction / (dofInteraction) / (residualSS / dofResidual));
            double pVal = 1.0 - (double)ShoNS.Stats.F.Cdf(fstatInteraction, dofInteraction, dofResidual);
            return pVal;
        }

        public static double pValueFromFstatistic(double Fstat, int dofNum, int dofDenom)
        {
            double pVal = 1.0 - (double)ShoNS.Stats.F.Cdf(Fstat, dofNum, dofDenom);
            return pVal;
        }


        public static DoubleArray ReadDoubleArrayFromDelimitedFile(string dir, string filename, char delim)
        {

            Stopwatch st = new Stopwatch();
            st.Start();

            string fullFileName;
            if (dir == null)
            {
                //fullFileName = ShoUtils.GetCSVFilenameNoDate(filename);
                fullFileName = filename;
            }
            else
            {
                fullFileName = dir + "\\" + filename;
            }

            //List<string[]> templist = new List<string[]>();
            List<double[]> templist = new List<double[]>();
            //double nanFlag = double.MaxValue;

            using (TextReader sr = File.OpenText(fullFileName))
            {
                //code that Erin gave me to avoid the problem in the code below:

                // Create a list of each line in the file
                string sline;
                while (null != (sline = sr.ReadLine()))
                {
                    // Split the line, breaking on the comma
                    string[] sarr = sline.Split(delim);

                    double[] sarrDouble = StringListToDoubleList(sarr);

                    //this is very slow, so first parse everything into numbers, and then call the constructor

                    // Stuff it into the list
                    templist.Add(sarrDouble);
                    //templist.Add(sarr);
                }
                // Pass the List to DoubleArray and let it convert
            }

            /********************************************************************************/
            //this is really slow when there are NaN's in the file, for some reason

            //int maxTries = 1;
            //bool SUCCESS = false;
            //DoubleArray tmp = null;
            //for (int j = 0; j < maxTries; j++)
            //{
            //    try
            //    {
            //        tmp = ShoNS.IO.DlmReader.dlmreadDoubleMatrix(fullFileName, ",");

            //        SUCCESS = true;
            //    }
            //    catch { System.Threading.Thread.Sleep(5000); }
            //    if (SUCCESS) break;
            //}
            //Helper.CheckCondition(tmp != null);
            //return tmp;

            st.Stop();
            Console.WriteLine("Finished reading in {0} " + fullFileName, st.Elapsed.ToString());

            return DoubleArray.From(templist);

        }

        public static DoubleArray ReadDoubleArrayFromDelimitedFile(string filename, char delim)
        {
            return ReadDoubleArrayFromDelimitedFile(null, filename, delim);
        }


        public static bool EntriesAreUniquelyZero(List<ShoMatrix> meanMatrixes)
        {
            for (int k = 0; k < meanMatrixes.Count; k++)
            {
                List<double> uniqueVals = meanMatrixes[k].Unique();
                bool tmpAreZero = (uniqueVals.Count == 1 && uniqueVals[0] == 0);
                if (!tmpAreZero)
                {
                    return false;
                }
            }
            return true;
        }

        public static DoubleArray RangeCollectionToDoubleArray(RangeCollection rc)
        {
            int N = (int)rc.Count();
            DoubleArray x = new DoubleArray(1, N);
            for (int j = 0; j < N; j++)
            {
                x[j] = rc.ElementAt(j);
            }
            return x;
        }


        public static IntArray RangeCollectionToIntArray(RangeCollection rc)
        {
            int N = (int)rc.Count();
            IntArray x = new IntArray(1, N);
            for (int j = 0; j < N; j++)
            {
                x[j] = (int)rc.ElementAt(j);
            }
            return x;
        }

        public static bool MergeShoMatrixFilesByAddition(DirectoryInfo dirinfo, string inputFilePattern, string outputFileName,
                         KeepTest<Dictionary<string, string>> globalKeepTest, List<KeepTest<Dictionary<string, string>>> splitKeepTestList,
                         bool auditMatrixIndex, List<string> rowKeys, List<string> colKeys, bool deleteFilesOnSuccess)
        {
            double tmpDiagFudge = 0.0;
            double tmpNumToDivideBy = 1.0;
            bool checkPassesCholesky = true;
            return MergeShoMatrixFilesByAddition(dirinfo, inputFilePattern, outputFileName, globalKeepTest, splitKeepTestList,
                   auditMatrixIndex, rowKeys, colKeys, deleteFilesOnSuccess, tmpNumToDivideBy, tmpDiagFudge, checkPassesCholesky);
        }

        /// <summary>
        /// Motivated by CreateTabulateReport, but instead does: reads in a set of Matrix files and adds them all together.
        ///For speed, I have changed it to read in Pickled DoubleArrays which must be immediately ready (without col/row reoridering) to
        ///add together
        /// </summary>
        ///<param name="auditMatrixIndex"> make sure all the files we expect to be present are, 
        /// based on their filenames ending in the pattern pieceIndexRange.totalNumPieces.txt</param>
        public static bool MergeShoMatrixFilesByAddition(DirectoryInfo dirinfo, string inputFilePattern, string outputFileName,
                   KeepTest<Dictionary<string, string>> globalKeepTest, List<KeepTest<Dictionary<string, string>>> splitKeepTestList,
                   bool auditMatrixIndex, List<string> rowKeys, List<string> colKeys, bool deleteFilesOnSuccess,
                   double numToDivideFinalMatrixBy, double diagFudge, bool checkPassesCholesky)
        {
            RangeCollection piecesAccumulated = new RangeCollection();
            //ShoMatrix accumulator = null;
            DoubleArray accumulator = null;
            int totalNumPieces = -1;
            //string currentPath = new FileInfo(outputFileName).DirectoryName;

            List<FileInfo> filesInMerge = new List<FileInfo>();
            RangeCollection pieceIndexRangeCollection = new RangeCollection();

            string previousFile = "";

            FileInfo[] listOfFiles = dirinfo.GetFiles(inputFilePattern);
            Console.WriteLine("MergeShoMatrixes inputFilePattern: " + inputFilePattern);
            foreach (FileInfo fileinfo in listOfFiles)
            {
                string thisFile = fileinfo.Name;
                filesInMerge.Add(fileinfo);
                //string nxtMatrixFile = currentPath + @"\" + thisFile;
                string nxtMatrixFile = dirinfo.FullName + @"\" + thisFile;
                //ShoMatrix nxtMatrix = ShoMatrix.ReadSymmetric(nxtMatrixFile).AsShoMatrix();
                DoubleArray nxtMatrix = (DoubleArray)ShoUtils.Unpickle(nxtMatrixFile);
                Helper.CheckCondition(!nxtMatrix.HasNaN(), "next matrix has NanS after reading in file " + nxtMatrixFile);
                System.Console.WriteLine("adding in next matrix in: " + thisFile);

                if (accumulator == null)
                {
                    accumulator = nxtMatrix;
                }
                else
                {
                    accumulator = accumulator + nxtMatrix;
                }

                /*now keep track of which pieces we have read: the file name ending is something like: "*0-1_5.txt" which 
                means that this file contains pieces 0-1 of 5*/
                string[] tmpString = thisFile.Split('.');
                string[] pieceInfo = tmpString[tmpString.Length - 2].Split('_');
                int newNumPieces = int.Parse(pieceInfo[1].Replace("Q", ""));
                if (totalNumPieces != -1)
                {
                    if (totalNumPieces != newNumPieces)
                    {
                        System.Console.WriteLine("last file: " + previousFile + " numPieces=" + totalNumPieces);
                        System.Console.WriteLine("current file: " + thisFile + " numPieces=" + newNumPieces);
                        throw new Exception("number of pieces does not matchin MergeShomatrixFilesByAddition");
                    }
                    Helper.CheckCondition(totalNumPieces == newNumPieces,
                          "number of pieces must be the same in all files:");
                    previousFile = thisFile;
                }
                else
                {
                    totalNumPieces = newNumPieces;
                }

                string thesePieces = pieceInfo[0];
                RangeCollection currentPieces = RangeCollection.Parse(thesePieces);
                Helper.CheckCondition(currentPieces.IsBetween(0, totalNumPieces - 1),
                                                string.Format(@"piecesIndexes must be at least zero and less than totalNumPieces (File ""{0}"")", thisFile));
                bool tryAdd = pieceIndexRangeCollection.TryAddRangeCollection(currentPieces);

                if (!tryAdd) return false;
            }
            if (deleteFilesOnSuccess)
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

            if (!pieceIndexRangeCollection.IsComplete(totalNumPieces))
            {
                using (TextWriter textWriterSkipFile = File.CreateText(outputFileName))
                {
                    textWriterSkipFile.WriteLine(pieceIndexRangeCollection);
                }
                Console.WriteLine("Not all needed pieces were found over {0}.", inputFilePattern);
                Console.WriteLine("Found rows:\n{0}", pieceIndexRangeCollection.ToString());
                //Console.WriteLine("Missing rows\n{0}", pieceIndexRangeCollection.Complement());
                Console.WriteLine("{0} created as skip file.", outputFileName);
                return false;
            }

            Helper.CheckCondition(!accumulator.HasNaN(), "accumulated matrix contains NaNs");
            accumulator = accumulator / numToDivideFinalMatrixBy;
            Helper.CheckCondition(!accumulator.HasNaN(), "accumulated matrix contains NaNs after division by " + totalNumPieces);

            ShoMatrix accumulatorShoMatrix = null;
            if (rowKeys == null)
            {
                accumulatorShoMatrix = ShoMatrix.CreateDefaultRowAndColKeyInstance(accumulator, double.NaN);
            }
            else
            {
                accumulatorShoMatrix = new ShoMatrix(accumulator, rowKeys, colKeys, double.NaN);
            }
            //always add 1e-5 to the diagonal, which is like an inverse-Wishart prior
            accumulatorShoMatrix.DoubleArray = accumulatorShoMatrix.DoubleArray.AddToDiagonal(diagFudge);
            if (checkPassesCholesky)
            {
                bool success;
                accumulatorShoMatrix.DoubleArray.TryCholesky(out success);
                if (!success) throw new Exception("kernel learned during M step does not pass Cholesky");
            }

            accumulatorShoMatrix.WriteToFile(outputFileName);
            return true;
        }

        //private static DoubleArray Unpickle(string nxtMatrixFile)
        //{
        //    throw new NotImplementedException("///Commenting out for now as part of .NET 4 upgrade");
        //}


        public static DoubleArray CreateLowRankCovApproxFromSvdProjections(DoubleArray pcProjections, int npc)
        {
            throw new NotImplementedException("fix syntax below");
            DoubleArray pseudoSnpData = null;//pcProjections.ColSliceE(0, 1, npc - 1).T;
            List<string> tmpFeatureIds = new List<string>();
            DoubleArray newK = pseudoSnpData.T.Multiply(pseudoSnpData);
            return newK;
        }

        public static int GetSeed()
        {//increment it so don't always get the same random numbers with each call
            //trying to avoid this now because of Parallel issues: probably want everything to take Random as a parameter instead
            throw new Exception("obsolete--need to redo code if this gets called");
            //return _seed++;
        }

        public static DoubleArray GetPCProjectionsFromSvd(DoubleArray svdU, DoubleArray svdD, int numPC)
        {
            ////System.Console.Write("Computing PC projections..");
            //could do this more computationally efficiently, but leaving for now...
            DoubleArray trainProj = svdU.Multiply(svdD.Sqrt()).T;//principleCoord=(A*sqrt(S1))'; //projections of each individual               
            DoubleArray trainProjSlice = trainProj.GetRowSlice(0, 1, numPC - 1).T;//.RowSliceE(0, 1, numPC - 1).T;
            ////System.Console.WriteLine("...finished.");
            return trainProjSlice;
        }


        /// <summary>
        /// get the projections (of column indexes into reduced row space) onto the first numPC PCs 
        /// </summary>
        /// <returns>rows are the new dimensions</returns>
        public static DoubleArray GetPCProjectionsFromSvd(SVD svd, int numPC)
        {
            ////System.Console.Write("Computing PC projections..");
            DoubleArray trainProj = svd.U.Multiply(svd.D.Sqrt()).T;//principleCoord=(A*sqrt(S1))'; //projections of each individual               
            DoubleArray trainProjSlice = trainProj.GetRowSlice(0, 1, numPC - 1).T;//.RowSliceE(0, 1, numPC - 1).T;
            ////System.Console.WriteLine("...finished.");
            return trainProjSlice;
        }

        [Obsolete("See escience matrix similarityMeasures and KernelNormalizers")]
        public static ShoMatrix ComputeKernelMatrixFromInnerProductOfMeanCenteredData(ShoMatrix snpMatrix)
        {
            DoubleArray K = ComputeKernelMatrixFromInnerProductOfMeanCenteredData(snpMatrix.DoubleArray);
            Helper.CheckCondition(!K.HasNaN(), "should not contain NaNs in data");
            return new ShoMatrix(K, snpMatrix.ColKeys, snpMatrix.ColKeys, double.NaN);
        }

        [Obsolete("Use MBT.Escience.Matrix.SimilarityScores.Covariance.StaticNormalizedToKernel instead")]
        public static DoubleArray ComputeKernelMatrixFromInnerProductOfMeanCenteredData(DoubleArray snpData)
        {
            Helper.CheckCondition(!snpData.HasNaN(), "should not contain NaNs in data");
            //first remove the mean of each individual
            ShoUtils.RemoveMeanColValueFromEachEntry(snpData);
            DoubleArray K = snpData.T.Multiply(snpData);  //snpData'*snpData
            //was having scaling problems, so:
            Console.WriteLine("cannot estimate time to completion for cov computation because using one big Sho matrix multiplication");
            K = K * (1.0 / (double)snpData.size0);//this line not yet checked.
            Helper.CheckCondition(!K.HasNaN(), "should not contain NaNs in data");
            return K;
        }

        public static void CheckThatRowsAreOneHotEncoded(this DoubleArray x)
        {
            DoubleArray rowsSums = x.Sum(DimOp.EachRow);
            List<double> uniqueRowSums = rowsSums.Unique().ToList();
            Helper.CheckCondition(uniqueRowSums.Count() == 1 && uniqueRowSums[0] == 1, "row sums are not exclusively equal to 1");
            Helper.CheckCondition(x.IsBinaryArray(), "data is not only 0/1");
        }

        /// <summary>
        /// assumes that we need to encode values from 0 through to maxVal with possible missing NaN values which will be filled in according
        /// to the missingValVec. For Emma kernel purposes, this would be the square root of the probability of seeing that value
        /// </summary>
        public static DoubleArray OneHotEncodingWithMissing(DoubleArray v, DoubleArray possibleIntegerValue, DoubleArray missingValVec)
        {
            Helper.CheckCondition(v.IsVector());
            Helper.CheckCondition(possibleIntegerValue.IsVector());
            Helper.CheckCondition(possibleIntegerValue.Length == missingValVec.Length);

            int numVals = possibleIntegerValue.Length;

            Dictionary<double, int> mapping = new Dictionary<double, int>();
            for (int j = 0; j < numVals; j++) mapping.Add(possibleIntegerValue[j], j);

            DoubleArray vHot;
            bool doTranspose = false;
            if (v.size0 > 1) doTranspose = true;
            vHot = new DoubleArray(possibleIntegerValue.Length, v.Length);
            for (int i = 0; i < v.Length; i++)
            {
                if (mapping.ContainsKey(v[i]))
                {
                    vHot[mapping[v[i]], i] = 1.0;
                }
                else
                {
                    //assign soft one-hot labels according to missingValVec
                    for (int j = 0; j < numVals; j++)
                    {
                        //vHot[mapping[v[possibleIntegerValue[j]],i] = missingValVec[j];
                    }
                }
            }
            if (doTranspose) vHot = vHot.T;
            return vHot;
        }

        /// <summary>
        /// assumes that we need to encode values from 0 through to maxVal 
        /// </summary>
        public static DoubleArray OneHotEncoding(DoubleArray v, DoubleArray possibleIntegerValue)
        {
            Helper.CheckCondition(v.IsVector());
            Helper.CheckCondition(possibleIntegerValue.IsVector());

            Dictionary<double, int> mapping = new Dictionary<double, int>();
            for (int j = 0; j < possibleIntegerValue.Length; j++) mapping.Add(possibleIntegerValue[j], j);

            DoubleArray vHot;
            vHot = (v.size0 == 1) ? new DoubleArray(possibleIntegerValue.Length, v.Length) : new DoubleArray(v.Length, possibleIntegerValue.Length);
            for (int i = 0; i < v.Length; i++)
            {
                if (v.size0 == 1)
                {
                    vHot[mapping[v[i]], i] = 1.0;
                }
                else
                {
                    vHot[i, mapping[v[i]]] = 1.0;
                }
            }
            return vHot;
        }

        public static void GetLinearComboOfDoubleArraysInPlace(List<DoubleArray> K, DoubleArray weights)
        {
            Helper.CheckCondition(K.Count == weights.Count, "length of matrixes to be added up linearly do not match length of weights");
            DoubleArray r = ShoUtils.DoubleArrayZeros(K[0].size0, K[0].size1);
            GetLinearComboOfDoubleArraysInPlaceStartingWithT(K, weights, ref r);
        }

        public static DoubleArray GetLinearComboOfDoubleArrays(List<DoubleArray> K, DoubleArray weights)
        {
            Helper.CheckCondition(K.Count == weights.Count, "length of matrixes to be added up linearly do not match length of weights");
            DoubleArray result = DoubleArray.From(K[0]) * weights[0];
            for (int k = 1; k < K.Count; k++)
            {
                result += weights[k] * K[k];
            }
            return result;
        }

        /// <summary>
        /// T=T +\sum_i deltaK[i]*K[i]
        /// </summary>
        /// <param name="K">list of Matrixes</param>
        /// <param name="weights">vector of scalars</param>
        /// <param name="T"></param>
        public static void GetLinearComboOfDoubleArraysInPlaceStartingWithT(List<DoubleArray> K, DoubleArray weights, ref DoubleArray T)
        {
            for (int k = 0; k < K.Count; k++)
            {
                //T = T + deltaK[k] * K[k];
                T.MultiplyAccum(weights[k], K[k]);//more efficient apparently
            }
        }

        /// <summary>
        /// Computes the inverse of a matrix from its Cholesky decomposition. 
        ///This is handy if we want to compute both the inverse and the
        ///log determinant of a matrix since both can be done from Cholesky.
        /// </summary>
        /// <param name="chol"></param>
        /// <returns></returns>
        public static DoubleArray InvFromChol(Cholesky chol)
        {
            //throw new Exception("not working, look at it");
            return chol.Solve(ShoUtils.Identity(chol.U.size0, chol.U.size1));
        }

        /// <summary>
        /// Computes the log determinant of a matrix from its cholesky decomposition.
        /// log|A| = 2 \sum_i L_ii where L_ii are diagnoal entries of the cholesky decomposition of symmetric, semi-positive def. matrix A
        ///This is handy if we want to compute both the inverse and the
        ///log determinant of a matrix since both can be done from Cholesky.
        /// </summary>
        /// <param name="chol"></param>
        /// <returns></returns>
        public static double LogDetFromChol(Cholesky chol)
        {
            double logDet = 2.0 * chol.U.Diagonal.Log().Sum();
            Helper.CheckCondition(!double.IsInfinity(logDet));
            return logDet;
        }

        /// <summary>
        /// useful for centering data before computing PCA (covariance)
        /// </summary>
        /// <param name="d"></param>
        public static void RemoveMeanColValueFromEachEntry(DoubleArray d)
        {
            DoubleArray meanForEachCol = d.Mean(DimOp.EachCol);
            for (int j = 0; j < d.size0; j++)
            {
                //throw new NotImplementedException("Change to SetSlice()");
                d.SetRow(j, d.GetRow(j) - meanForEachCol);
            }
        }

        public static DoubleArray InvertFromSvd(SVD svd)
        {            
            return InvertFromSvd(svd.U, svd.D);
        }

        /// <summary>
        /// Assumes that [U D U']=svd(K) (and thus that K is symmetric, positive definite)
        /// </summary>
        /// <param name="svdU"></param>
        /// <param name="svdD"></param>
        /// <returns></returns>
        public static DoubleArray InvertFromSvd(DoubleArray svdU, DoubleArray svdD)
        {
            DoubleArray Kinv = svdU.Multiply(ShoUtils.MakeDiag(1 / svdD.Diagonal)).Multiply(svdU.T);//yes, the transpose should be last, as in the reconstruction
            return Kinv;
        }

        public static DoubleArray SampleFromMultivariateGaussianUsingCholDoubleArray(DoubleArray mean, DoubleArray cholDoubleArrayT, Random randObj)
        {
            //throw new NotImplementedException("Need to upgrade to .NET 4 version of Sho");
            return SampleFromMultivariateGaussianUsingCholDoubleArray(mean, cholDoubleArrayT, 1, randObj)[0];
        }

        ///Commenting out for now as part of .NET 4 upgrade
        //public static DoubleArray SampleFromMultivariateGaussianUsingCholDoubleArray(DoubleArray mean, DoubleArray cholDoubleArrayT, int numSamples, Random randObj)
        //{
        //    List<DoubleArray> samples = SampleFromMultivariateGaussianUsingCholDoubleArray(mean, cholDoubleArrayT, numSamples, randObj);
        //    DoubleArray tmp = new DoubleArray(samples.Count, samples[0].Count);
        //    for (int s = 0; s < numSamples; s++)
        //    {
        //    }
        //}


        public static DoubleArray SampleFromMultivariateSphericalGaussian(DoubleArray mean, double stddev, int numInd, Random randObj)
        {
            return SampleFromMultivariateSphericalGaussian(mean, stddev, numInd, 1, randObj)[0];
        }

        public static List<DoubleArray> SampleFromMultivariateSphericalGaussian(DoubleArray mean, double stddev, int numInd, int numSamples, Random randObj)
        {
            List<DoubleArray> result = new List<DoubleArray>();

            for (int s = 0; s < numSamples; s++)
            {
                DoubleArray randVec = MVNormRandZeroSpherical(numInd, randObj).T;
                result.Add(randVec * stddev + mean.ToVector().Transpose());
            }

            return result;
        }

        ////Commenting out for now as part of .NET 4 upgrade
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="cholDoubleArrayT">this would be from new Cholesky(X).U.Transpose()</param>
        /// <param name="numSamples"></param>
        /// <returns></returns>
        public static List<DoubleArray> SampleFromMultivariateGaussianUsingCholDoubleArray(DoubleArray mean, DoubleArray cholDoubleArrayT, int numSamples, Random randObj)
        {
            List<DoubleArray> result = new List<DoubleArray>();

            //instead, sample from the independent gaussians, and then correct it
            int numInd = cholDoubleArrayT.size0;
                        
            //MVNormRand z = new MVNormRand(new DoubleArray(mean.size0, mean.size1), ShoUtils.Identity(numInd,numInd), GetSeed());

            //DoubleArray UT = covChol.U.Transpose();
            DoubleArray UT = cholDoubleArrayT;
            for (int s = 0; s < numSamples; s++)
            {
                DoubleArray randVec = MVNormRandZeroSpherical(numInd, randObj).T;
                //result.Add(UT.Multiply(z.NextVector().T) + mean.ToVector().Transpose());
                result.Add(UT.Multiply(randVec) + mean.ToVector().Transpose());
                //result.Add(UT.Multiply(randVec) + mean.ToVector().Transpose());
            }

            return result;
        }

        ////Commenting out for now as part of .NET 4 upgrade
        /// <summary>
        /// Get 1 sample from a zero-mean, standardized spherical multi-D gaussian with numDim dimensions
        /// </summary>
        /// <param name="numDim"></param>
        /// <param name="randObj"></param>
        /// <returns></returns>
        private static DoubleArray MVNormRandZeroSpherical(int numDim, Random randObj)
        {
            DoubleArray r = new DoubleArray(1, numDim);
            GaussRand tmpGen = new GaussRand(randObj);
            for (int d = 0; d < numDim; d++)
            {
                r[d] = tmpGen.NextDouble();
            }
            return r;
        }

        /// <summary>
        /// just provide a single sample instead of a user-specified number
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="cov"></param>
        /// <returns></returns>
        //public static DoubleArray SampleFromMultivariateGaussian(DoubleArray mean, DoubleArray cov, Random randObj)
        //{
        //    return SampleFromMultivariateGaussian(mean, cov, randObj, 1)[0];
        //}

        /// <summary>
        /// Sample numSamp times from a multivariate gaussian. First we sample vector z from
        /// a spherical (diag entries 1) gaussian, and then we get the sample x from
        /// x = Lz + mu, where L is from a cholesky decomp (cov=L*L.T), and mu is the mean of the
        /// gaussian we're sampling from
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="cov"></param>
        /// <param name="numSamples"></param>
        /// <returns>array with columns as the number of samples, and each row a single sample</returns>
        //public static List<DoubleArray> SampleFromMultivariateGaussian(DoubleArray mean, DoubleArray cov, Random randObj, int numSamples)
        //{
        //    List<DoubleArray> result = new List<DoubleArray>();

        //    //not working right now for some reason
        //    if (false)
        //    {
        //        MVNormRand tmp = new MVNormRand(mean, cov, GetSeed());
        //        for (int s = 0; s < numSamples; s++)
        //        { result.Add(tmp.NextVector()); }
        //    }
        //    else//workaround Erin's code which seems to always complain about the covariance matrix
        //    {
        //        //instead, sample from the independent gaussians, and ten correct it
        //        //MVNormRand z = new MVNormRand(new DoubleArray(mean.size0,mean.size1), ShoUtils.Identity(cov.size0, cov.size1), GetSeed());
        //        Cholesky ch = new Cholesky(cov);
        //        //result = SampleFromMultivariateGaussian(mean, ch.U.Transpose(), numSamples);
        //        result = SampleFromMultivariateGaussianUsingCholDoubleArray(mean, ch.U.Transpose(), numSamples, randObj);
        //    }
        //    return result;
        //}

        public static void CheckSnpDataEncoding(string SNP_ENCODING_STR, Matrix<string, string, double> snpMatrix)
        {
            int SNP_ENCODING = (SNP_ENCODING_STR == "01") ? 1 : 2;
            CheckSnpDataEncoding(SNP_ENCODING, snpMatrix);
        }

        public static void CheckSnpDataEncoding(int SNP_ENCODING, Matrix<string, string, double> snpMatrix)
        {
            //DoubleArray uniqueSnpVals = snpMatrix.DoubleArray.UniqueVals();
            //DoubleArray uniqueSnpVals = snpMatrix.DoubleArray.Unique();
            IEnumerable<double> uniqueSnpVals = snpMatrix.Unique();
            Helper.CheckCondition(uniqueSnpVals.Max() == SNP_ENCODING);
            Helper.CheckCondition(uniqueSnpVals.Min() == 0);
            Helper.CheckCondition(uniqueSnpVals.Count() <= (SNP_ENCODING + 2));
        }

        /// <summary>
        /// retarded fix so that I can call this in intialization of a static variable to make sure this is called before I do anything
        /// </summary>
        /// <returns></returns>
        public static bool HaveSetShoDirEnvironmentVariable()
        {
            return HaveSetShoDirEnvironmentVariable(1);
        }

        public static void PrintEnvVarOmpNumThreadsForSho(ref string runString)
        {
            string tmpStr = "OMP_NUM_THREADS=" + Environment.GetEnvironmentVariable("OMP_NUM_THREADS");
            Console.WriteLine(tmpStr);
            runString += tmpStr + "\n";
        }

        public static bool HaveSetShoDirEnvironmentVariable(int processorCountForMultiThreading)
        {
            SetShoDirEnvironmentVariable(processorCountForMultiThreading);
            return true;
        }

        [Obsolete("use the version with one argument")]///use the version with one argument
        static public bool SetShoDirEnvironmentVariable()
        {
            Console.WriteLine("WARNING: Calling Sho with default value for OMP_NUM_THREADS");
            return SetShoDirEnvironmentVariable(1);
        }

        static public bool SetShoDirEnvironmentVariable(int processorCountForMultiThreading)
        {
            return SetShoDirEnvironmentVariable(processorCountForMultiThreading, false);
        }

        //!!!Could set this as part of setting a static variable, but would it get init'ed if the class ShoUtils wasn't otherwise used?
        static public bool SetShoDirEnvironmentVariable(int processorCountForMultiThreading, bool quiet)
        {
            if (processorCountForMultiThreading > 0 && processorCountForMultiThreading != ShoProcCount)
            {
                Environment.SetEnvironmentVariable("OMP_NUM_THREADS", processorCountForMultiThreading.ToString());
                ShoProcCount = processorCountForMultiThreading;
            }
            else
            {
                Environment.SetEnvironmentVariable("OMP_NUM_THREADS", processorCountForMultiThreading.ToString());
            }

            if (!ShoEnvVarHasBeenInitialized)
            {
                Environment.SetEnvironmentVariable("LALDIR",
                                            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Sho");


                //ShoNS.Array.ArraySettings.EnableFastMath().Enforce("Expecting Fast math to be enabled and it is not");
                ShoNS.Array.ArraySettings.EnableFastMath();
                if (!ShoNS.Array.ArraySettings.FastMathEnabled)
                {
                    throw new Exception("Fast math not successfully enabled");
                }
                if (!quiet) Console.WriteLine("Fast math enabled.");
                ShoEnvVarHasBeenInitialized = true;
            }
            if (!quiet)
            {
                string tmpStr = "OMP_NUM_THREADS=" + Environment.GetEnvironmentVariable("OMP_NUM_THREADS");
                Console.WriteLine(tmpStr);
            }
            return true;
        }

        public static string RemovePostfixFromFileName(string myFileName)
        {
            int postFixLength = 4;
            return myFileName.Substring(0, myFileName.Length - postFixLength);
        }


        public static string AddPostfixToFileName(string myFileName, string postfix)
        {
            int startIndex = myFileName.Length - 4;
            string newFileName = myFileName.Insert(startIndex, postfix);
            return newFileName;
        }

        /// <summary>
        /// Compute the 2-sided FET from two paired feature vectors
        /// </summary>
        /// <param name="thisPhen"></param>
        /// <param name="thisSnpDat"></param>
        /// <returns>2-sided p-value</returns>
        public static double FisherExactTestFromDoubleArrays(DoubleArray thisPhen, DoubleArray thisSnpDat)
        {
            int[,] counts = ShoUtils.ComputeContingencyTable(thisPhen, thisSnpDat);
            //Helper.CheckCondition(tmpCount == numInd,"contingency table does not add up");
            return SpecialFunctions.FisherExactTest(counts);
            //note there is also built-in Sho Fisher in the Stats package
        }

        /// <summary>
        /// Given two vectors of 0's and 1's, compute the contingency table (in format suitable for SpecialFunctions.FisherExactTest
        /// </summary>
        /// <param name="thisPhen"></param>
        /// <param name="thisSnpDat"></param>
        /// <returns></returns>
        public static int[,] ComputeContingencyTable(DoubleArray thisPhen, DoubleArray thisSnpDat)
        {
            Helper.CheckCondition(thisPhen.IsBinaryArray(), "input arrays must contain only 0's and 1's");
            int[,] counts = new int[2, 2] { { 0, 0 }, { 0, 0 } };

            int tmpCount = 0;
            for (int snpVal = 0; snpVal <= 1; snpVal++)
            {
                for (int phenVal = 0; phenVal <= 1; phenVal++)
                {
                    //counts[snpVal, phenVal] = (thisSnpDat.ElementEQ(snpVal).ElementMultiply(thisPhen.ElementEQ(phenVal))).Find(x => x == 1).Length;
                    counts[snpVal, phenVal] = (thisSnpDat.ElementEQ(snpVal).ElementMultiply(thisPhen.ElementEQ(phenVal))).Find(delegate(double x) { return x == 1; }).Length;

                    //for (int ind = 0; ind < numInd; ind++)
                    //{
                    //    if (thisSnpDat[ind] == snpVal && thisPhen[ind] == phenVal)
                    //    {
                    //        counts[snpVal, phenVal]++;
                    //    }
                    //}
                    tmpCount += counts[snpVal, phenVal];
                }
            }
            return counts;
        }

        public static DoubleArray AddBiasLastToInputData(DoubleArray inputData, int numInd)
        {
            DoubleArray biasTerm = new DoubleArray(numInd, 1);
            biasTerm.FillValue(1.0);
            if (null != inputData)
            {
                inputData = ShoUtils.CatArrayCol(inputData, biasTerm);
            }
            else
            {
                inputData = biasTerm;
            }
            return inputData;
        }

        /// <summary>
        /// Useful for regression models where the bias/intercept is modelled as an extra feature always equal to 1.
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="targetData"></param>
        /// <returns></returns>
        public static DoubleArray AddBiasToInputData(DoubleArray inputData, DoubleArray targetData)
        {
            Helper.CheckCondition(targetData.size0 >= 1 && targetData.size1 == 1, "targetData is the wrong size");
            return AddBiasLastToInputData(inputData, targetData.size0);

            //old version
            //Helper.CheckCondition(targetData.size1 <= 1, "targetData is the wrong size");
            //DoubleArray biasTerm = new DoubleArray(targetData.size0, 1);
            //biasTerm.FillValue(1.0);
            //inputData = ShoUtils.CatArrayCol(inputData, biasTerm);
            //return inputData;
        }

        /// <summary>
        /// Extract the columns of the csv file into a DoubleArray
        /// Assumes: one header, entries in specified columns are doubles
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="colIndsToUse"></param>
        /// <returns></returns>
        public static DoubleArray ExtractColumnsFromDelimFile(string fileName, List<int> colIndsToUse, out List<string> header, char delim)
        {
            //store as a list first, then covert to DoubleArray, since we don't knwo from the outset how large an array to make
            List<List<double>> data = new List<List<double>>(colIndsToUse.Count);
            foreach (int elt in colIndsToUse)
            {
                data.Add(new List<double>());
            }

            using (TextReader textReader = File.OpenText(fileName))
            {
                string firstLine = textReader.ReadLine();
                Helper.CheckCondition(null != firstLine, "Expect file to have first line: " + fileName);
                string[] fullHeader = firstLine.Split(delim);
                header = new List<string>();
                foreach (int elt in colIndsToUse)
                {
                    header.Add(fullHeader[elt]);
                }
                string line;
                while (null != (line = textReader.ReadLine()))
                {
                    string[] fields = line.Split(',');
                    for (int j = 0; j < colIndsToUse.Count; j++)
                    {
                        data[j].Add(Double.Parse(fields[colIndsToUse[j]]));
                    }
                }
            }
            DoubleArray x = DoubleArray.From(data);
            return x;
        }

        public static DoubleArray Identity(double size0, double size1)
        {
            DoubleArray x = new DoubleArray((int)size0, (int)size1);
            x.FillIdentity();
            return x;
        }

        public static DoubleArray Identity(int size0, int size1)
        {
            DoubleArray x = new DoubleArray(size0, size1);
            x.FillIdentity();
            return x;
        }

        /// <summary>
        /// currently the constructor makes things have zeros, but i'm paranoid Sho will change this
        /// </summary>
        /// <param name="size0"></param>
        /// <param name="size1"></param>
        /// <returns></returns>
        public static DoubleArray DoubleArrayZeros(int size0, int size1)
        {
            DoubleArray x = new DoubleArray(size0, size1);
            x.FillValue(0.0);
            return x;
        }

        public static DoubleArray DoubleArrayNaNs(int size0, int size1)
        {
            DoubleArray x = new DoubleArray(size0, size1);
            x.FillValue(double.NaN);
            return x;
        }

        public static DoubleArray DoubleArrayOnes(int size0, int size1)
        {
            DoubleArray x = new DoubleArray(size0, size1);
            x.FillValue(1.0);
            return x;
        }

        public static IntArray IntArrayOnes(int size0, int size1)
        {
            IntArray x = new IntArray(size0, size1);
            x.FillValue(1);
            return x;

        }

        //public static DoubleArray RandNormal(int size0, int size1)
        //{
        //    DoubleArray randVec = (new DoubleArray(size0, size1)).FillRand(
        //    return randVec;
        //}

        /// <summary>
        /// Create an array populated by random numbers drawn from a uniform distribution
        /// </summary>
        /// <param name="size0"></param>
        /// <param name="size1"></param>
        /// <returns></returns>
        public static DoubleArray RandUniform(int size0, int size1, Random rand)
        {
            DoubleArray randVec = (new DoubleArray(size0, size1)).FillRandUseSeed(rand);
            return randVec;
        }

        public static DoubleArray RandUniform(DoubleArray x, Random rand)
        {
            return RandUniform(x.size0, x.size1, rand);
        }



        public static DoubleArray ReadDoubleColumnFromDelimitedFile(string fullFileName, string fieldName)
        {

            List<double> results = new List<double>();
            using (TextReader textReader = File.OpenText(fullFileName))
            {

                List<string> header = textReader.ReadLine().Split('\t').ToList();
                int? colNum = null;
                for (int j = 0; j < header.Count(); j++)
                {
                    if (header[j] == fieldName) colNum = j;
                }
                if (!colNum.HasValue) throw new Exception("field: " + fieldName + " does not exist in " + fullFileName);
                string line;
                while (null != (line = textReader.ReadLine()))
                {
                    double nextElt = double.Parse(line.Split('\t')[colNum.Value].Trim());
                    results.Add(nextElt);
                }
            }
            return DoubleArray.From(results);
        }


        public static List<string> ReadStringColumnFromDelimitedFile(string fullFileName, string fieldName)
        {

            List<string> results = new List<string>();
            using (TextReader textReader = File.OpenText(fullFileName))
            {

                List<string> header = textReader.ReadLine().Split('\t').ToList();
                int? colNum = null;
                for (int j = 0; j < header.Count(); j++)
                {
                    if (header[j] == fieldName) colNum = j;
                }
                if (!colNum.HasValue) throw new Exception("field: " + fieldName + " does not exist in " + fullFileName);
                string line;
                while (null != (line = textReader.ReadLine()))
                {
                    string nextElt = line.Split('\t')[colNum.Value].Trim();
                    results.Add(nextElt);
                }
            }
            return results;
        }





        /// <summary>
        /// convert array of strings representing doubles to an array of doubles.
        ///replaces NaN values with nanFlag to speed up parsing alter on.
        /// </summary>
        public static double[] StringListToDoubleList(string[] strarr)
        {
            double[] sarrDouble = new double[strarr.Length];
            for (int j = 0; j < strarr.Length; j++)
            {
                if (!String.Equals(strarr[j], "NaN"))
                {
                    sarrDouble[j] = double.Parse(strarr[j]);
                }
                else
                {
                    sarrDouble[j] = double.NaN;// nanFlag;
                }
            }
            return sarrDouble;
        }




        public static IntArray ToIntArray(List<int> list)
        {
            IntArray x = new IntArray(1, list.Count());
            int count = 0;
            foreach (int elt in list)
            {
                x[count++] = elt;
            }
            return x;
        }

        public static DoubleArray ToDoubleArray(List<double> list)
        {
            DoubleArray x = new DoubleArray(1, list.Count());
            int count = 0;
            foreach (double elt in list)
            {
                x[count++] = elt;
            }
            return x;
        }



        /// <summary>
        /// Append filename with date time for uniqueness
        /// </summary>
        /// <returns></returns>
        public static string AddFileStamp(string filename)
        {
            string dateTimeString = SpecialFunctions.GetDateStamp();
            return filename + "_" + dateTimeString;
        }

        /*moved this to SpecialFunctions
        public static string GetDateStamp()
        {
            DateTime date = DateTime.Now;
            string dateTimeString = "_" + date.Year.ToString() + "_" + date.ToString("MM") + "_" + date.Day + "-" + date.ToString("HH_mm_ss");
            return dateTimeString;
        }*/


        /// <summary>
        /// Use Sho pickling to retrieve this object to a file for later retrieval
        /// (see also Pickle)
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static object Unpickle(string filename)
        {
            object x = null;
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                Helper.CheckCondition(null != stream, "stream is null for file: " + filename);

                x = ShoPickleHelper.DeserializeObj(stream);
            }
            //System.Console.WriteLine("done");
            return x;
        }

        /// <summary>
        /// Use Sho pickling to save this object to a file for later retrieval
        /// (see also Unpickle)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="filename"></param>
        public static void Pickle(object x, string filename)
        {
            //System.Console.Write("Pickling to: " + filename + " ");
            using (FileStream stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))
            {
                ShoPickleHelper.SerializeObj(x, stream);
                //System.Console.WriteLine("... done.");
            }
        }


        public static void printDateAndTime()
        {
            DateTime currentSystemTime = DateTime.Now;
            Console.WriteLine(currentSystemTime);
        }

        /// <summary>
        /// Apply log operator to every element of a DoubleArray and return a DoubleArray
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static DoubleArray log(DoubleArray v)
        {
            return DoubleArray.From(ShoNS.MathFunc.ArrayMath.Log(v.ToList()));
        }




        public static List<Color> GetColorList()
        {
            List<Color> colList = new List<Color>();
            colList.Add(System.Drawing.Color.Black);
            colList.Add(System.Drawing.Color.Blue);
            colList.Add(System.Drawing.Color.Green);
            colList.Add(System.Drawing.Color.Orange);
            colList.Add(System.Drawing.Color.Red);
            colList.Add(System.Drawing.Color.OrangeRed);
            colList.Add(System.Drawing.Color.HotPink);
            colList.Add(System.Drawing.Color.Gold);
            return colList;
        }



        /// <summary>
        /// Like Matlab's : operator. That is, in Matlab, theRange=firstInd:steptSize:lastInd
        /// </summary>
        /// <param name="firstInd"></param>
        /// <param name="lastInd"></param>
        /// <param name="stepSize"></param>
        /// <returns></returns>
        public static DoubleArray MakeRange(double firstInd, double lastInd, double stepSize)
        {
            //DoubleRange theRange = new DoubleRange(firstInd, lastInd, stepSize);
            Helper.CheckCondition(stepSize > 0);//not yet implemented
            List<double> listRange = new List<double>();
            for (double ind = firstInd; ind <= lastInd; ind = ind + stepSize)
            {
                listRange.Add(ind);
            }
            DoubleArray theRange;
            if (listRange.Count > 0)
            {
                theRange = DoubleArray.From(listRange);
            }
            else
            {
                theRange = new DoubleArray(0);
            }
            return theRange;
        }

        /// <summary>
        /// Like Matlab's : operatore. That is, in Matlab, theRange=firstInd:lastInd
        /// </summary>
        /// <param name="firstInd"></param>
        /// <param name="lastInd"></param>
        /// <returns></returns>
        public static DoubleArray MakeRangeInclusive(int firstInd, int lastInd)
        {
            int stepSize = 1;
            return MakeRange(firstInd, lastInd, stepSize);
        }

        /// <summary>
        /// Like Matlab's : operatore. That is, in Matlab, theRange=firstInd:lastInd 
        /// </summary>
        /// <param name="firstInd"></param>
        /// <param name="lastInd"></param>
        /// <param name="stepSize"></param>
        /// <returns></returns>
        public static IntArray MakeRangeIntInclusive(int firstInd, int lastInd, int stepSize)
        {
            Helper.CheckCondition(stepSize > -1);
            if (stepSize == 0)
            {
                return new IntArray(0);
            }
            else
            {
                IntArray theRange = IntArray.From(MakeRange(firstInd, lastInd, stepSize));
                return theRange;
            }
        }

        public static IntArray MakeRangeIntInclusiveReverse(int firstInd, int lastInd)
        {
            List<int> tmpList = MakeRangeIntInclusive(firstInd, lastInd).ToList();
            tmpList.Reverse();
            return IntArray.From(tmpList);
        }

        /// <summary>
        /// Like Matlab's : operatore. That is, in Matlab, theRange=firstInd:lastInd
        /// </summary>
        /// <param name="firstInd"></param>
        /// <param name="lastInd"></param>
        /// <returns></returns>
        public static IntArray MakeRangeIntInclusive(int firstInd, int lastInd)
        {
            int stepSize = 1;
            IntArray theRange = IntArray.From(MakeRange(firstInd, lastInd, stepSize));
            return theRange;
        }

        public static double Ftest(DoubleArray beta, DoubleArray vInv, DoubleArray X, int coordToTest)
        {
            return Ftest(beta, vInv, X, new List<int> { coordToTest });
        }


        public static double FtestLinearRegression(DoubleArray beta, double residualVariance, DoubleArray X, int coordsToTest)
        {
            return FtestLinearRegression(beta, residualVariance, X, new List<int> { coordsToTest });
        }
        /// <summary>
        /// F-test as described in the Emma paper, but modified for linear regression. See Ftest for more general one used by Mixed model
        /// </summary>
        /// <param name="beta"></param>
        /// <param name="vInv"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        public static double FtestLinearRegression(DoubleArray beta, double residualVariance, DoubleArray X, IList<int> coordsToTest)
        {
            double vInv = 1.0 / residualVariance;
            int Q = beta.Length;//beta.Length - 1;//omitting the intercept
            int N = X.size0;

            //This is done outside of here now
            //X = ShoUtils.AddBiasToInputData(X, N); 

            int P = coordsToTest.Count;
            double dof = P;//p is notation used in Emma paper

            DoubleArray M = ShoUtils.DoubleArrayZeros(P, Q);
            int i = 0;
            foreach (int elt in coordsToTest)
            {
                M[i++, elt] = 1;
            }

            DoubleArray term1 = M.Multiply(beta);
            DoubleArray middleTerm = M.Multiply(X.Transpose().Multiply(vInv * X).Inv()).Multiply(M.Transpose()).Inv();
            DoubleArray numerator = term1.Transpose().Multiply(middleTerm).Multiply(term1);
            Helper.CheckCondition(numerator.Length == 1);
            double Fstatistic = numerator[0] / P;
            double pValue = 1.0 - (double)ShoNS.Stats.F.Cdf(Fstatistic, 1, N - Q);
            Helper.CheckCondition(pValue >= 0 && pValue <= 1, "pValue not in range [0,1]");
            return pValue;
        }

        /// <summary>
        /// F-test as described in the Emma paper. Here, vInv is (\sigma_g_2*H)^-1.
        /// see also p. 59 of the SAS manual for PROC ANOVA for Balanced designs, http://www.okstate.edu/sas/v8/saspdf/stat/chap4.pdf 
        /// </summary>
        /// <param name="beta"></param>
        /// <param name="vInv"></param>
        /// <param name="X"></param>
        /// <returns></returns>
        public static double Ftest(DoubleArray beta, DoubleArray vInv, DoubleArray X, IList<int> coordsToTest)
        {

            //throw new Exception("has not been tested and may be buggy or incorrect");
            //hard-code for one degree of freedom--the first of two params in beta
            //Helper.CheckCondition(beta.Length == 2);//until we start going to more than one SNP
            int Q = beta.Length;//beta.Length - 1;//omitting the intercept
            int N = X.size0;

            //This is done outside of here now
            //X = ShoUtils.AddBiasToInputData(X, N); 

            int P = coordsToTest.Count;
            double dof = P;//p is notation used in Emma paper

            DoubleArray M = ShoUtils.DoubleArrayZeros(P, Q);
            int i = 0;
            foreach (int elt in coordsToTest)
            {
                M[i++, elt] = 1;
            }

            DoubleArray term1 = M.Multiply(beta);
            DoubleArray middleTerm = M.Multiply(X.Transpose().Multiply(vInv.Multiply(X)).Inv()).Multiply(M.Transpose()).Inv();
            DoubleArray numerator = term1.Transpose().Multiply(middleTerm).Multiply(term1);
            Helper.CheckCondition(numerator.Length == 1);
            double Fstatistic = numerator[0] / P;
            double pValue = 1.0 - (double)ShoNS.Stats.F.Cdf(Fstatistic, 1, N - Q);
            Helper.CheckCondition(pValue >= 0 && pValue <= 1, "pValue not in range [0,1]");
            return pValue;
        }





        /// <summary>
        /// Compute the p-value from a log likelihood ratio test. If the richer model has a worse
        /// log likelihood than the other model, it will return p=1+eps as a special symbol. No 
        /// adjustments are made here for boundary conditions. That is, if the parameter setting of
        /// the richer model lies on the boundary for the null model, then this computation will be
        /// incorrect, and will need to be modified as appropriate.
        /// </summary>
        /// <param name="logLikeRicherModel"></param>
        /// <param name="logLikeOtherModel"></param>
        /// <param name="dof">Degrees of Freedom difference between the two models</param>
        /// <returns></returns>
        public static double LRTest(double logLikeRicherModel, double logLikeOtherModel, double dof)
        {
            double logLikeRatio = logLikeRicherModel - logLikeOtherModel;
            return LRTest(logLikeRatio, dof);
        }
        public static double LRTest(double logLikeRatio, double dof)
        {
            double pValue = Double.NaN;
            if (logLikeRatio <= 0)
            {
                //special demarkation to say the likelihood went the wrong way. This can happen
                //when using numerical routines where convergence is never perfect and there is little
                //difference between the models
                pValue = 1 + Double.Epsilon;
            }
            else
            {
                // This give a high resolution answers when the pValues are very small, e.g. 1e-7, 1e-21, etc.
                pValue = Gamma.GammaQ(logLikeRatio, dof / 2);
            }
            return pValue;
        }
        //        if (dof < 2)
        //        {
        //            pValue = SpecialFunctions.GetInstance().LogLikelihoodRatioTest(logLikeRatio, dof);
        //        }
        //        else
        //        {
        //            double testStatistic = 2 * logLikeRatio;
        //            double tmpChi = ShoNS.Stats.Chi2.Cdf(testStatistic, dof);
        //            //double tmpChi = ShoNS.Stats.Chi2.chi2cdf(testStatistic, dof);
        //            double pValue2 = 1 - tmpChi;
        //            Console.WriteLine("{0},{1}", pValue2, pValue);
        //            Helper.CheckCondition(pValue2 != 0, "need higher prediction LRT");
        //        }
        //    }
        //    return pValue;
        //}

        /// <summary>
        /// Take out only certain items of a list. In matlab, if myList were a cell array,
        /// then this would be the functionality: newList=myList{indexList}
        /// </summary>
        /// <param name="myList">the original list</param>
        /// <param name="indexList">the indexes into this list</param>
        public static List<T> SliceList<T>(List<T> myList, List<int> indexList)
        {
            List<T> newList = indexList.Select(i => myList[i]).ToList();
            return newList;

        }

        /// <summary>
        /// Take out only certain items of a list. In matlab, if myList were a cell array,
        /// then this would be the functionality: newList=myList{oneIndex}
        /// </summary>
        /// <param name="myList">the original list</param>
        /// <param name="oneIndex">a single index into the list</param>
        public static List<T> SliceList<T>(List<T> myList, int oneIndex)
        {
            List<int> indexList = new List<int> { oneIndex };
            return SliceList(myList, indexList);

        }

        /// <summary>
        /// Print out the dimensions of a show array objectec
        /// </summary>
        /// <param name="inputData">the array</param>
        /// <param name="varName">the array variable name</param>
        public static void PrintArraySize(DoubleArray inputData, String varName)
        {
            System.Console.WriteLine("size(" + varName + ")=[" + inputData.size0 + ", " + inputData.size1 + "]");
        }

        /// <summary>
        /// Matlab notation: newArray = [array0 array1]
        /// </summary>
        /// <param name="array0">array of size NxM1</param>
        /// <param name="array1">array of size NxM2</param>
        /// <returns>array of size Nx(M1+M2)</returns>
        public static DoubleArray CatArrayCol(DoubleArray array0, DoubleArray array1)
        {
            DoubleArray r = DoubleArray.HorizStack(new DoubleArray[2] { array0, array1 });
            return r;
        }

        /// <summary>
        /// Matlab notation: newArray=[array0; array1]
        /// </summary>
        /// <param name="array0">array of size N1xM</param>
        /// <param name="array1">array of size N2xM</param>
        /// <returns>array of size (N1+N2)xM</returns>
        public static DoubleArray CatArrayRow(DoubleArray array0, DoubleArray array1)
        {
            DoubleArray r = DoubleArray.VertStack(new DoubleArray[2] { array0, array1 });
            return r;
        }


        /// <summary>
        /// Matlab notation: newArray = [array0 array1]
        /// </summary>
        /// <param name="array0">array of size NxM1</param>
        /// <param name="array1">array of size NxM2</param>
        /// <returns>array of size Nx(M1+M2)</returns>
        public static IntArray CatArrayRow(IntArray array0, IntArray array1)
        {

            IntArray r = IntArray.HorizStack(new IntArray[2] { array0, array1 });
            return r;

            //String errMsg = "number of cols in each array must be the same: array0.size0=" + array0.size0 + ", array1.size0=" + array1.size0;
            //Helper.CheckCondition(array0.size0 == array1.size0, "number of columns in each array must be the same" + errMsg);
            //ArrayList tmpList = new System.Collections.ArrayList();
            //tmpList.Add(array0);
            //tmpList.Add(array1);
            //IntArray tmpInputs = IntArray.From(tmpList);
            //return tmpInputs;
        }

        /// <summary>
        /// Matlab notation: newArray=[array0; array1]
        /// </summary>
        /// <param name="array0">array of size N1xM</param>
        /// <param name="array1">array of size N2xM</param>
        /// <returns>array of size (N1+N2)xM</returns>
        public static IntArray CatArrayCol(IntArray array0, IntArray array1)
        {

            IntArray r = IntArray.VertStack(new IntArray[2] { array0, array1 });
            return r;

            //String errMsg = "number of cols in each array must be the same: array0.size1=" + array0.size1 + ", array1.size1=" + array1.size1;
            //Helper.CheckCondition(array0.size1 == array1.size1, "number of columns in each array must be the same" + errMsg);

            //ArrayList tmpList = new System.Collections.ArrayList();
            //ArrayList tmpList0 = new System.Collections.ArrayList();
            //ArrayList tmpList1 = new System.Collections.ArrayList();
            //tmpList0.Add(array0);
            //tmpList1.Add(array1);
            //tmpList.Add(tmpList0);
            //tmpList.Add(tmpList1);

            //IntArray tmpInputs = IntArray.From(tmpList);
            //return tmpInputs;
        }


        /// <summary>
        /// Function written to help use logistic regression code which does not use Sho Arrays
        /// </summary>
        /// <param name="predictorMatrix"></param>
        /// <returns></returns>
        public static IList<float[]> ConvertDoubleArrayToListOfFloatArrays(DoubleArray predictorMatrix)
        {
            IList<float[]> listOfListOfFloat = new List<float[]>(predictorMatrix.size0);
            for (int i = 0; i < predictorMatrix.size0; ++i)
            {
                float[] listOfFloat = new float[predictorMatrix.size1];
                listOfListOfFloat.Add(listOfFloat);
                for (int j = 0; j < predictorMatrix.size1; ++j)
                {
                    listOfFloat[j] = (float)predictorMatrix[i, j];
                }
            }
            return listOfListOfFloat;
        }

        //!!! Very similar code elsewhere (ConvertDoubleArrayToListOfFloatArays)
        /// <summary>
        /// Copy of the function written to help use logistic regression code which does not use Sho Arrays
        /// </summary>
        /// <param name="predictorMatrix"></param>
        /// <returns></returns>
        public static IList<double[]> ConvertDoubleArrayToListOfDoubleArrays(DoubleArray predictorMatrix)
        {
            IList<double[]> listOfListOfDouble = new List<double[]>(predictorMatrix.size0);
            for (int i = 0; i < predictorMatrix.size0; ++i)
            {
                double[] listOfDouble = new double[predictorMatrix.size1];
                listOfListOfDouble.Add(listOfDouble);
                for (int j = 0; j < predictorMatrix.size1; ++j)
                {
                    listOfDouble[j] = (double)predictorMatrix[i, j];
                }
            }
            return listOfListOfDouble;
        }


        public static double Logistic(double x)
        {
            //see http://en.wikipedia.org/wiki/Logistic_distribution
            double mu = 0;
            double sec = 1;
            //I thought I wanted the pdf, but it turns out Cdf is what gives 1/(1+exp(-x))
            return ShoNS.Stats.Logistic.Cdf(x, mu, sec);
        }

        /// <summary>
        /// Map the logistic function, element by element, to the array v
        /// </summary>
        /// <param name="v"></param>
        /// <returns>an array the same size as v</returns>
        public static DoubleArray Logistic(DoubleArray v)
        {

            DoubleArray logisticArray = new DoubleArray(v.size0, v.size1);
            for (int ind0 = 0; ind0 < v.size0; ind0++)
            {
                for (int ind1 = 0; ind1 < v.size1; ind1++)
                {
                    logisticArray[ind0, ind1] = Logistic(v[ind0, ind1]);
                }
            }
            return logisticArray;

        }

        public static DoubleArray MakeDiag(DoubleArray diagVals)
        {
            //throw new NotImplementedException("Can't find the replacement for ArrayCreate");
            return ShoNS.Array.ArrayUtils.Diag(diagVals, 0);
            //return (DoubleArray)ShoNS.MathFunc.ArrayCreate.Diag(diagVals, 0);
        }

        //Obsolete: use Sho calls: 
        //public static double Logistic(double d)
        //{
        //    return 1.0 / (1.0 + Math.Exp(-d));

        //}

        public static List<string> SelectEntriesFromList(List<string> myList, IntArray keepInd)
        {
            List<string> newList = new List<string>();
            for (int j = 0; j < myList.Count(); j++)
            {
                if (keepInd.Contains(j)) newList.Add(myList[j]);
            }
            return newList;
        }

        public static void CheckMatrixDimensionsMatch(DoubleArray X1, DoubleArray X2)
        {
            CheckColDimensionMatch(X1, X2);
            CheckRowDimensionMatch(X1, X2);

        }
        public static void CheckRowDimensionMatch(DoubleArray X1, DoubleArray X2)
        {
            Helper.CheckCondition(X1.size0 == X2.size0, "row counts don't match");

        }
        public static void CheckColDimensionMatch(DoubleArray X1, DoubleArray X2)
        {
            Helper.CheckCondition(X1.size1 == X2.size1, "col counts don't match");

        }

        /// <summary>
        /// Count the number of entries that are identical
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <returns></returns>
        public static double CountNumberOfIdenticalEntries(DoubleArray x1, DoubleArray x2)
        {

            /*requires two passes, so probably slow
            IntArray tmp = x1.ElementEQ(x2);
            int count = tmp.Sum();
            */

            int count = 0;
            for (int r = 0; r < x1.size0; r++)
            {
                for (int c = 0; c < x1.size1; c++)
                {
                    if (x1[r, c] == x2[r, c]) count++;
                }
            }

            return (double)count;
        }

        /// <summary>
        /// use the other version if you want to cache the inverse computations
        /// </summary>
        /// <param name="thisDat"></param>
        /// <param name="thisMean"></param>
        /// <param name="cov"></param>
        /// <returns></returns>
        public static double LogPdfGauss(DoubleArray thisDat, DoubleArray thisMean, DoubleArray cov)
        {
            Helper.CheckCondition(thisDat.IsVector() && thisMean.IsVector());
            Cholesky chol = new Cholesky(cov);
            DoubleArray covInv = ShoUtils.InvFromChol(chol);
            double logSqrtDet = 0.5 * ShoUtils.LogDetFromChol(chol);
            return LogPdfGauss(thisDat.ToVector(), thisMean.ToVector(), logSqrtDet, covInv);
        }

        public static double LogPdfGauss(DoubleArray thisDat, DoubleArray thisMean, double logSqrtDetCov, DoubleArray covInv)
        {
            double D = (double)thisMean.Length;
            DoubleArray xMinusMean = thisDat - thisMean;
            return -(D / 2.0) * _LOG2PI - logSqrtDetCov - 0.5 * (xMinusMean.Multiply(covInv).Multiply(xMinusMean.T).ToScalar());
        }

        public static double LogPdfGauss(double thisDat, double thisMean, double var)
        {
            double D = 1.0;
            double logSqrtDetCov = 0.5 * Math.Log(var);
            double xMinusMean = thisDat - thisMean;

            //debugging code
            //double tmp = Math.Log(1.0 / Math.Sqrt(2 * Math.PI * var)) - 0.5 * (xMinusMean * xMinusMean / var);
            //double tmp2 = Math.Log(ShoNS.Stats.Gauss.Pdf(thisDat, thisMean, Math.Sqrt(var)));

            double result = -(D / 2.0) * _LOG2PI - logSqrtDetCov - 0.5 * (xMinusMean * (1 / var) * xMinusMean);
            return result;
        }

        /// <summary>
        /// The log PDF of a Student's t-distribution, as defined on page 432 of Bayesian Theory by
        /// Bernardo and Smith
        /// </summary>
        /// <param name="point"></param>
        /// <param name="mean"></param>
        /// <param name="precision"></param>
        /// <param name="df">degrees of freedom</param>
        /// <returns></returns>
        public static double LogPdfStudentT(double point, double mean, double precision, double df)
        {
            double xMinusMean = point - mean;
            double modifiedDf = 0.5 * (df + 1);

            double logInterior = Math.Log(1 + (1 / df) * precision * xMinusMean * xMinusMean);
            double logGammaTerms = Utils.LogGamma(modifiedDf) - Utils.LogGamma(0.5 * df);

            return -modifiedDf * logInterior + 0.5 * (Math.Log(precision) - Math.Log(df) - _LOGPI) + logGammaTerms;
        }

        /// <summary>
        /// The PDF of a Student's t-distribution, as defined on page 432 of Bayesian Theory by
        /// Bernardo and Smith
        /// </summary>
        /// <param name="point"></param>
        /// <param name="mean"></param>
        /// <param name="precision"></param>
        /// <param name="df">degrees of freedom</param>
        /// <returns></returns>
        public static double PdfStudentT(double point, double mean, double precision, double df)
        {
            double xMinusMean = point - mean;
            double modifiedDf = 0.5 * (df + 1);

            double interior = 1 + (1 / df) * precision * xMinusMean * xMinusMean;
            double gammaTerms = Utils.GammaFunc(modifiedDf) / Utils.GammaFunc(0.5 * df);

            return Math.Pow(interior, -modifiedDf) * Math.Sqrt(precision / (df * Math.PI)) * gammaTerms;
        }


        public static ShoMatrix LearnLinearRegressionModel(
            Matrix<string, string, double> predictor,
            Matrix<string, string, double> target
            )
        {
            Helper.CheckCondition(predictor.ColKeys.SequenceEqual(target.ColKeys), "Expect predictor and target to have same cids in the same order");
            Helper.CheckCondition(!predictor.ContainsRowKey(""), "The empty string may not be used as a rowKey because it will be used by the bias.");
            Helper.CheckCondition(target.RowCount == 1, "Expect target matrix to have only one row");

            DoubleArray biasRowAsDoubleArray = new DoubleArray(1, predictor.ColCount);
            biasRowAsDoubleArray.FillValue(1.0);
            ShoMatrix biasRow = new ShoMatrix(ref biasRowAsDoubleArray, new string[] { "" }, predictor.ColKeys, double.NaN);
            var predictorWithBiasRow = predictor.MergeRowsView(/*colsMustMatch*/ true, biasRow);
            DoubleArray predictorWithBiasRowAsDoubleArray = predictorWithBiasRow.TransposeView().ToShoMatrix().DoubleArray;

            DoubleArray targetRowAsDoubleArray = target.TransposeView().ToShoMatrix().DoubleArray;


            ShoNS.Array.Solver solver = new ShoNS.Array.Solver(predictorWithBiasRowAsDoubleArray);
            DoubleArray weights1 = solver.Solve(targetRowAsDoubleArray);
            ShoMatrix weights = new ShoMatrix(weights1, predictorWithBiasRow.RowKeys, new string[] { "weight" }, double.NaN); //!!!!const

            //Commented this out because it was computing the population variance instead of the sample variance
            //DoubleArray residuals = predictorWithBiasRowAsDoubleArray * weights1 - targetRowAsDoubleArray;
            //variance = residuals.Dot(residuals) / residuals.Length;


            return weights;
        }

        public static double[] DiscretizeGammaDistribution(double alpha, int numBins)
        {
            if (numBins == 1)
                return new double[] { 1 };

            double[] result = new double[numBins];

            // each bin has equal probability mass under the Gamma distribution. (1/numBins)
            // the rate associated with each bin is then either the mean or median of the 
            // proportion of the gamma distn that falls in that bin.

            // median method
            //for (int i = 0; i < numBins; i++)
            //{
            //    double p = (2 * i + 1) / (2.0 * numBins); // the median p-value for the ith bin
            //    double r = Gamma.Inv(p, alpha, 1 / alpha);
            //    result[i] = r;
            //}
            //// now normalize so that the average rate is 1.
            //double mean = result.Average();
            //for (int i = 0; i < numBins; i++)
            //{
            //    result[i] /= mean;
            //}

            //return result;

            // mean method
            double[] rCutPoints = new double[numBins - 1];
            double[] gammaPs = new double[numBins - 1];
            for (int i = 0; i < numBins - 1; i++)
            {
                double p = (i + 1.0) / numBins;
                rCutPoints[i] = Gamma.Inv(p, alpha, 1 / alpha);
                gammaPs[i] = Gamma.GammaP(rCutPoints[i] * alpha, alpha + 1);
            }

            result[0] = gammaPs[0] * numBins;
            result[numBins - 1] = (1 - gammaPs[numBins - 2]) * numBins;

            for (int i = 1; i < numBins - 1; i++)
            {
                double upperSum = gammaPs[i];
                double lowerSum = gammaPs[i - 1];
                result[i] = (upperSum - lowerSum) * numBins;
            }


            //Console.WriteLine("alpha: {0}; {1}", alpha, result.StringJoin(", "));
            return result;
        }

        /// <summary>
        /// An example of this is when I have people in terms of their HLA A data, and want one-hot features indicating which {A1,A2} group
        /// they belong to. That is, there will be N one-hot features, each representing the pair of HLA A alleles a person has. 
        /// </summary>
        /// <param name="altFeatureData"></param>
        /// <returns></returns>
        public static DoubleArray OneHotEncodeFromBinaryFeatureDataRowsAreFeatures(DoubleArray altFeatureData, int maxExpectedBinaryFeatures)
        {
            int numCid = altFeatureData.size1;
            int numOrigFeatures = altFeatureData.size0;
            Helper.CheckCondition(altFeatureData.IsBinaryArray());
            Dictionary<HashSet<int>, int> featNumAndFeatInTermsOfOriginalFeatureIndexes = new Dictionary<HashSet<int>, int>();
            DoubleArray integerEncodingOfFeatures = new DoubleArray(1, numCid);
            int featureCounter = 0;
            for (int cid = 0; cid < numCid; cid++)
            {
                HashSet<int> hasTheseFeatures = altFeatureData.GetCol(cid).Find(elt => elt == 1).ToHashSet();
                Helper.CheckCondition(hasTheseFeatures.Count() <= maxExpectedBinaryFeatures, "more than maxExpectedBinaryFeaturesFound");
                int featNum;
                bool hasEntry = featNumAndFeatInTermsOfOriginalFeatureIndexes.TryGetValue(hasTheseFeatures, out featNum);
                if (hasEntry)
                {
                    integerEncodingOfFeatures[cid] = featNum;
                }
                else
                {
                    featNumAndFeatInTermsOfOriginalFeatureIndexes.Add(hasTheseFeatures, featureCounter);
                    integerEncodingOfFeatures[cid] = featureCounter;
                    featureCounter++;
                }
            }
            int numOneHotFeatures = featNumAndFeatInTermsOfOriginalFeatureIndexes.Count;
            int maxAllowedFeatures = 1000;//300;
            Helper.CheckCondition(numOneHotFeatures < maxAllowedFeatures, "warning, more than " + maxAllowedFeatures + " one-hot features will be created! Change max to allow this");

            DoubleArray oneHotEncodedData = ShoUtils.OneHotEncoding(integerEncodingOfFeatures, integerEncodingOfFeatures.Unique());
            return oneHotEncodedData;
        }

        public static void ComputeEigenForSymmetrixMatrix(double[][] symmetricMatrix, out double[][] eigenVectors, out double[] eigenValues)
        {
            Eigen eigen = new Eigen(DoubleArray.From(symmetricMatrix));
            //Helper.CheckCondition<ArgumentException>(eigen.D is DoubleArray && eigen.V is DoubleArray, "matric must not have been symmetric, because the eigen values or vectors contain complex numbers.");

            if (eigen.V is ComplexArray)
            {
                eigenVectors = ((ComplexArray)eigen.V).ToRealArray();
                eigenValues = ((ComplexArray)eigen.D).Select(c => c.Real).ToArray();
            }
            else
            {
                eigenVectors = ((DoubleArray)eigen.V).ToRaggedArray();
                eigenValues = ((DoubleArray)eigen.D).ToOneDArray();
            }
        }


        public static double[][] MatrixExp(double[][] Q, double multiplier = 1)
        {
            DoubleArray matrix = DoubleArray.From(Q);
            ComplexArray matrixExp = matrix.MatrixExp(multiplier);

            double[][] result = new double[Q.Length][];
            for (int i = 0; i < Q.Length; i++)
            {
                result[i] = new double[Q[i].Length];
                for (int j = 0; j < Q[i].Length; j++)
                {
                    result[i][j] = matrixExp[i, j].Real;
                }
            }
            return result;
        }
    }
}
