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
using Bio.Matrix;
using MBT.Escience.Sho;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using Bio.Util;

namespace Eigenstrat
{
    public class EigenstratMain
    {
        static string UsageMessage = @"Example: Eigenstrat ...";

        public static void TestIt(IList<IList<double>> r)
        {
            for (int rowIndex = 0; rowIndex < r.Count; ++rowIndex)
            {
                for (int colIndex = 0; colIndex < r[rowIndex].Count; ++colIndex)
                {
                    Console.WriteLine(r[rowIndex][colIndex]);
                }
            }

        }

        public static void Main(string[] args)
        {
            //SpecialFunctions.CheckDate(2010, 4, 16);
            //double[][] ragged = new double[][]{new double[]{1,2,3},new double[]{4,5,6}};
            //TestIt(ragged);
            //double[,] twoD = new double[,] {{ 1, 2, 3 },{ 4, 5, 6 } };
            ////TestIt(twoD);Nope
            //var sparse = SparseMatrix<string, string, double>.CreateEmptyInstance(new[] { "key1", "key2" }, new[] { "cid1" }, double.NaN);
            //TestIt(sparse);


            ////BioMatrixSample.BioMatrixSample.DemoMatrix(Console.Out);
            ////Bio.Matrix.MatrixUnitTest.MainTest(doOutOfRangeTest: true, parallelOptions: new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount });
            //return;

            Console.WriteLine(Environment.MachineName);
            Console.WriteLine(Helper.CreateDelimitedString(" ", args));

            try
            {
                ShoUtils.SetShoDirEnvironmentVariable(1);

                ArgumentCollection argumentCollection = new CommandArguments(args);

                if (argumentCollection.ExtractOptionalFlag("help"))
                {
                    Console.WriteLine("");
                    Console.WriteLine(UsageMessage);
                    return;
                }

                bool useCorrel = argumentCollection.ExtractOptionalFlag("correl");
                //bool doubleUp = argCollection.ExtractOptionalFlag("doubleUp");
                ParallelOptions parallelOptions = new ParallelOptions {MaxDegreeOfParallelism = argumentCollection.ExtractOptional("MaxDegreeOfParallelism", Environment.ProcessorCount) };
                int randomSeed = argumentCollection.ExtractOptional<int>("randomSeed", (int)MachineInvariantRandom.GetSeedUInt("Eigenstrat"));
                int? randomRowCountOrNull = argumentCollection.ExtractOptional<int?>("randomRowCount", null);


                argumentCollection.CheckNoMoreOptions(3);
                int maxValue = argumentCollection.ExtractNext<int>("maxValue");
                string inputDenseFileName = argumentCollection.ExtractNext<string>("inputDenseFile");
                string outputCovFileName = argumentCollection.ExtractNext<string>("outputCovFile");
                argumentCollection.CheckThatEmpty();

                Console.WriteLine("Reading input file " + inputDenseFileName);
                //var originalMatrix = MatrixFactorySSC.Parse(inputDenseFileName, '?', parallelOptions);

                Console.WriteLine("Using 'GetInstanceFromDenseAnsi' How about 'GetInstanceFromRowKeysAnsi', too?");
                using (var originalMatrix = RowKeysAnsi.GetInstanceFromDenseAnsi(inputDenseFileName, parallelOptions))
                {
                    Matrix<string, string, char> matrixOptionallyCutDown;
                    if (null != randomRowCountOrNull)
                    {
                        Random random = new Random(randomSeed);
                        var sampleRowKeys = SpecialFunctions.SelectRandom(originalMatrix.RowKeys, randomRowCountOrNull.Value, ref random);
                        matrixOptionallyCutDown = originalMatrix.SelectRowsView(sampleRowKeys);
                    }
                    else
                    {
                        matrixOptionallyCutDown = originalMatrix;
                    }

                    var gMatrix = matrixOptionallyCutDown.ConvertValueView(new CharToDoubleWithLimitsConverter(maxValue), double.NaN);

                    //DenseMatrix<string, string, double>.CreateDefaultInstance
                    var xMatrix = StandardizeGToCreateX<ShoMatrix>(maxValue, gMatrix, ShoMatrix.CreateDefaultInstance, parallelOptions);

                    var psiMatrix = CreatePsiTheMatrixOfCovarianceValues(useCorrel, xMatrix, /*isOKToDestroyXMatrix*/ true, parallelOptions);

                    Console.WriteLine("Writing output file " + outputCovFileName);
                    psiMatrix.WriteDense(outputCovFileName);
                }


            }
            catch (Exception exception)
            {
                Console.WriteLine("");
                Console.WriteLine(exception.Message);
                if (exception.InnerException != null)
                {
                    Console.WriteLine(exception.InnerException.Message);
                }

                Console.WriteLine("");
                Console.WriteLine(UsageMessage);

                Console.WriteLine(exception.StackTrace);
                throw new Exception("", exception);
            }
        }

        [Obsolete("See escience matrix similarityMeasures")]        
        /// <summary>
        /// only compute the diagonal elements and make all else missing/0.0
        /// </summary>
        /// <param name="useCorrel"></param>
        /// <param name="xMatrix"></param>
        /// <param name="weightForEachFeature"></param>
        /// <param name="isOKToDestroyXMatrix"></param>
        /// <param name="parallelOptions"></param>
        /// <param name="dataMean"></param>
        /// <returns></returns>
        public static Matrix<string, string, double> CreatePsiTheMatrixOfDiagonalCovarianceValuesWithWeights(bool useCorrel, Matrix<string, string, double> xMatrix, IList<double> weightForEachFeature, bool isOKToDestroyXMatrix, ParallelOptions parallelOptions, out List<double> dataMean)
        {
            Matrix<string, string, double> xMatrixWithColumnMeansZero = CreateXMatrixWithColumnMeansZeroWithWeights(xMatrix, isOKToDestroyXMatrix, weightForEachFeature, parallelOptions, out dataMean);

            //Console.WriteLine("create CreatePsiTheMatrixOfCovarianceValuesWithWeights");
            Matrix<string, string, double> psiMatrix = DenseMatrix<string, string, double>.CreateDefaultInstance(xMatrix.ColKeys, xMatrix.ColKeys, 0.0);
            Parallel.ForEach(Enumerable.Range(0, xMatrix.ColCount), parallelOptions, cidIndex1 =>
            {
                psiMatrix[cidIndex1, cidIndex1] = CovarianceOrCorrelWithWeights(xMatrixWithColumnMeansZero, cidIndex1, cidIndex1, useCorrel, weightForEachFeature);
            });
            return psiMatrix;
        }

        [Obsolete("See escience matrix similarityMeasures")]        
        /// <summary>
        /// Computing correlation/covariance matrix with weighted features
        /// </summary>
        /// <param name="useCorrel"></param>
        /// <param name="snpMatrix"></param>
        /// <param name="weightForEachFeature"></param>
        /// <param name="p"></param>
        /// <param name="parallelOptions"></param>
        /// <returns></returns>
        public static Matrix<string, string, double> CreatePsiTheMatrixOfCovarianceValuesWithWeights(bool useCorrel, Matrix<string, string, double> xMatrix, IList<double> weightForEachFeature, bool isOKToDestroyXMatrix, ParallelOptions parallelOptions, out List<double> dataMean)
        {
            Matrix<string, string, double> xMatrixWithColumnMeansZero = CreateXMatrixWithColumnMeansZeroWithWeights(xMatrix, isOKToDestroyXMatrix, weightForEachFeature, parallelOptions, out dataMean);

            //Console.WriteLine("create CreatePsiTheMatrixOfCovarianceValuesWithWeights");
            Matrix<string, string, double> psiMatrix = DenseMatrix<string, string, double>.CreateDefaultInstance(xMatrix.ColKeys, xMatrix.ColKeys, double.NaN);
            Parallel.ForEach(Enumerable.Range(0, xMatrix.ColCount), parallelOptions, cidIndex1 =>
            {
                //Console.Write(string.Format("\r{0}/{1}\t", cidIndex1, xMatrix.ColCount));
                Parallel.ForEach(Enumerable.Range(cidIndex1, xMatrix.ColCount - cidIndex1), parallelOptions, cidIndex2 =>
                {
                    psiMatrix[cidIndex1, cidIndex2] = CovarianceOrCorrelWithWeights(xMatrixWithColumnMeansZero, cidIndex1, cidIndex2, useCorrel, weightForEachFeature);
                    psiMatrix[cidIndex2, cidIndex1] = psiMatrix[cidIndex1, cidIndex2]; //If the indexes, this does nothing. That is OK.
                });
            });
            //Console.WriteLine();
            return psiMatrix;
        }


        ///!!! Should be possible to do this with a one pass algo instead of with two passes, one for the mean, and one for the rest 

[Obsolete("See escience matrix similarityMeasures")]        
public static Matrix<string, string, double> CreatePsiTheMatrixOfCovarianceValues(bool useCorrel, Matrix<string, string, double> xMatrix, bool isOKToDestroyXMatrix, ParallelOptions parallelOptions)
{
    Matrix<string, string, double> xMatrixWithColumnMeansZero = CreateXMatrixWithColumnMeansZero(xMatrix, isOKToDestroyXMatrix, parallelOptions);

    Console.WriteLine("create CreatePsiTheMatrixOfCovarianceValues");
    Matrix<string, string, double> psiMatrix = DenseMatrix<string, string, double>.CreateDefaultInstance(xMatrix.ColKeys, xMatrix.ColKeys, double.NaN);
    Parallel.ForEach(Enumerable.Range(0, xMatrix.ColCount), parallelOptions, cidIndex1 =>
    {
        Console.Write(string.Format("\r{0}//{1}\t", cidIndex1, xMatrix.ColCount));
        Parallel.ForEach(Enumerable.Range(cidIndex1, xMatrix.ColCount - cidIndex1), parallelOptions, cidIndex2 =>
        {
            psiMatrix[cidIndex1, cidIndex2] = CovarianceOrCorrel(xMatrixWithColumnMeansZero, cidIndex1, cidIndex2, useCorrel);
            psiMatrix[cidIndex2, cidIndex1] = psiMatrix[cidIndex1, cidIndex2]; //If the indexes, this does nothing. That is OK.
        });
    });
    Console.WriteLine();
    return psiMatrix;
}
        [Obsolete("See escience matrix similarityMeasures")]        
        /// <summary>
        /// feature here is each row/SNP
        /// </summary>
        /// <param name="xMatrix"></param>
        /// <param name="isOKToDestroyXMatrix"></param>
        /// <param name="weightForEachFeature"></param>
        /// <param name="parallelOptions"></param>
        /// <returns></returns>
        ///!!! Should be possible to do this with a one pass algo instead of with two passes, one for the mean, and one for the rest 
        public static Matrix<string, string, double> CreateXMatrixWithColumnMeansZeroWithWeights(Matrix<string, string, double> xMatrix, bool isOKToDestroyXMatrix, IList<double> weightForEachFeature, ParallelOptions parallelOptions, out List<double> columnMeans)
        {
            //Console.WriteLine("create xMatrixWithColumnMeansZeroWithWeights");
            List<double> columnMeansTmp = SpecialFunctions.CreateListWithDefaultElements<double>(xMatrix.ColCount);

            Matrix<string, string, double> xMatrixWithColumnMeansZero;
            {
                xMatrixWithColumnMeansZero = isOKToDestroyXMatrix ? xMatrix : DenseMatrix<string, string, double>.CreateDefaultInstance(xMatrix.RowKeys, xMatrix.ColKeys, double.NaN);
                Parallel.ForEach(Enumerable.Range(0, xMatrix.ColCount), parallelOptions, cidIndex =>
                {
                    //Console.Write(string.Format("\r{0}/{1}\t", cidIndex, xMatrix.ColCount));
                    double sum = 0;
                    double weightSum = 0;
                    for (int varIndex = 0; varIndex < xMatrix.RowCount; ++varIndex)
                    {
                        sum += weightForEachFeature[varIndex] * xMatrix[varIndex, cidIndex];
                        weightSum += weightForEachFeature[varIndex];
                    }
                    //double columnMean = sum / xMatrix.RowCount;
                    double columnMean = sum / weightSum;
                    columnMeansTmp[cidIndex] = columnMean;
                    for (int varIndex = 0; varIndex < xMatrix.RowCount; ++varIndex)
                    {
                        xMatrixWithColumnMeansZero[varIndex, cidIndex] = xMatrix[varIndex, cidIndex] - columnMean;
                    }
                });
                Console.WriteLine();
            }
            columnMeans = columnMeansTmp;
            return xMatrixWithColumnMeansZero;
        }


        [Obsolete("See escience matrix similarityMeasures")]        
        public static Matrix<string, string, double> CreateXMatrixWithColumnMeansZero(Matrix<string, string, double> xMatrix, bool isOKToDestroyXMatrix, ParallelOptions parallelOptions)
        {
            Console.WriteLine("create xMatrixWithColumnMeansZero");

            Matrix<string, string, double> xMatrixWithColumnMeansZero;
            //if (doubleUp)
            //{
            //    //The column means are already , by construction, so no work is necessary
            //    xMatrixWithColumnMeansZero = xMatrix;
            //}
            //else
            {
                xMatrixWithColumnMeansZero = isOKToDestroyXMatrix ? xMatrix : DenseMatrix<string, string, double>.CreateDefaultInstance(xMatrix.RowKeys, xMatrix.ColKeys, double.NaN);
                Parallel.ForEach(Enumerable.Range(0, xMatrix.ColCount), parallelOptions, cidIndex =>
                {
                    Console.Write(string.Format("\r{0}/{1}\t", cidIndex, xMatrix.ColCount));
                    double sum = 0;
                    for (int varIndex = 0; varIndex < xMatrix.RowCount; ++varIndex)
                    {
                        sum += xMatrix[varIndex, cidIndex];
                    }
                    double columnMean = sum / xMatrix.RowCount;
                    for (int varIndex = 0; varIndex < xMatrix.RowCount; ++varIndex)
                    {
                        xMatrixWithColumnMeansZero[varIndex, cidIndex] = xMatrix[varIndex, cidIndex] - columnMean;
                    }
                });
                Console.WriteLine();
            }
            return xMatrixWithColumnMeansZero;
        }


        [Obsolete("See escience matrix similarityMeasures")]        
        public static TMatrix StandardizeGToCreateX<TMatrix>(int maxValue, Matrix<string, string, double> gMatrix,
                                 MatrixFactoryDelegate<TMatrix, string, string, double> zeroMatrixFractory,
                                 ParallelOptions parallelOptions, bool onlyMeanCenter = false) where TMatrix : Matrix<string, string, double>
        {
            Console.WriteLine("StandardizeGToCreateX");
            //var xMatrix = DenseMatrix<string, string, double>.CreateDefaultInstance(gMatrix.RowKeys, gMatrix.ColKeys, double.NaN); //Inits to 0
            TMatrix xMatrix = zeroMatrixFractory(gMatrix.RowKeys, gMatrix.ColKeys, double.NaN);  //Inits to 0
            Helper.CheckCondition(xMatrix.GetValueOrMissing(0, 0) == 0, "xMatrix must start init'ed to zeros, but even at xMatrix[0,0] it is not 0");
            //First create x matrix
            //Parallel.ForEach(gMatrix.RowKeys, parallelOptions, var =>
            var counterWithMessages = new CounterWithMessages("StandardizeGToCreateX: row #{0}", 100, gMatrix.RowCount);
            foreach (string var in gMatrix.RowKeys)
            {
                counterWithMessages.Increment();

                //The paper divides by (2+2*Count) because its values are 0,1,2. Because ours are 0,1, we divide by (2+Count)
                var row = gMatrix.SelectRowsView(var);
                List<double> nonMissingValues = row.Values.ToList();
                double rowSum = nonMissingValues.Sum();
                double rowMean = rowSum / (double)nonMissingValues.Count;
                double piSmoothedCount = (1.0 + rowSum)
                                       / (2.0 + maxValue * (double)nonMissingValues.Count);
                double stdDevPi = Math.Sqrt(piSmoothedCount * (1 - piSmoothedCount));
                Helper.CheckCondition(!double.IsNaN(stdDevPi), "stdDevPi is NaN outside loop, likely because data input was not in 0/1/2 format as expected");

                Parallel.ForEach(row.RowKeyColKeyValues, parallelOptions, triple =>
                {                 
                    string cid = triple.ColKey;
                    double gValue = triple.Value;
                    double xValue;
                    if (onlyMeanCenter)
                    {
                        xValue = (gValue - rowMean);
                    }
                    else
                    {
                        xValue = (gValue - rowMean) / stdDevPi;
                    }

                    xMatrix[var, cid] = xValue;
                    //if (doubleUp)
                    //{
                    //    xMatrix[new Pair<string, bool>(var, true), cid] = -xValue;
                    //}
                });
                //});
            }
            Console.WriteLine();
            return xMatrix;
        }

        //private static IEnumerable<string> XRowKeys(ReadOnlyCollection<string> gRowKeys)
        //{
        //    foreach(var gRowKey in gRowKeys)
        //    {
        //        yield return new Pair<string,bool>(gRowKey, false);
        //        if (doubleUp)
        //        {
        //            yield return new Pair<string,bool>(gRowKey, true);
        //        }
        //    }
        //}

        [Obsolete("See escience matrix similarityMeasures")]        
        /// <summary>
        /// feature here is each row/SNP/cidIndex1
        ///see comment for CovarianceOrCorrel for more details
        /// </summary>
        /// <param name="xMatrixWithColumnMeansZero"></param>
        /// <param name="cidIndex1">appears to be obsolete</param>
        /// <param name="cidIndex2">appears to be obsolete</param>
        /// <param name="useCorrel"></param>
        /// <param name="weightForEachFeature"></param>
        /// <returns></returns>
        private static double CovarianceOrCorrelWithWeights<TRowKey, TColKey>(Matrix<TRowKey, TColKey, double> matrixWithColumnMeansZero, int columnIndex1, int columnIndex2, bool useCorrel, IList<double> weightForEachFeature)
        {
            double numerator = 0;
            double denomatorX1 = 0;
            double denomatorX2 = 0;
            double weightSum = 0;
            for (int rowIndex = 0; rowIndex < matrixWithColumnMeansZero.RowCount; ++rowIndex)
            {
                numerator += weightForEachFeature[rowIndex] * matrixWithColumnMeansZero[rowIndex, columnIndex1] * matrixWithColumnMeansZero[rowIndex, columnIndex2];
                weightSum += weightForEachFeature[rowIndex];
                if (useCorrel)
                {
                    throw new NotImplementedException("have not updated the code to handle weights for correlation");
                    denomatorX1 += matrixWithColumnMeansZero[rowIndex, columnIndex1] * matrixWithColumnMeansZero[rowIndex, columnIndex1];
                    denomatorX2 += matrixWithColumnMeansZero[rowIndex, columnIndex2] * matrixWithColumnMeansZero[rowIndex, columnIndex2];
                }
            }

            if (useCorrel)
            {
                throw new NotImplementedException("do I need to deal with the weightSum too? check if want to use this");
                double correl = numerator / Math.Sqrt(denomatorX1 * denomatorX2);
                return correl;
            }
            else
            {
                //double covariance = numerator / matrixWithColumnMeansZero.RowCount;
                double covariance = numerator / weightSum;
                return covariance;
            }
        }

        [Obsolete("See escience matrix similarityMeasures")]        
        //Cov(X,Y) = Sum[(x-xbar)*(y-ybar)]/n where xbar and ybar are the sample means
        //Correl(X,Y) = Sum[(x-xbar)*(y-ybar)]/Sqrt[Sum[(x-xbar)^2]*Sum[(y-ybar)^2]]
        //!!!similar code elsewhere
        public static double CovarianceOrCorrel<TRowKey, TColKey>(Matrix<TRowKey, TColKey, double> matrixWithColumnMeansZero, int columnIndex1, int columnIndex2, bool useCorrel)
        {
            double numerator = 0;
            double denomatorX1 = 0;
            double denomatorX2 = 0;
            for (int rowIndex = 0; rowIndex < matrixWithColumnMeansZero.RowCount; ++rowIndex)
            {
                numerator += matrixWithColumnMeansZero[rowIndex, columnIndex1] * matrixWithColumnMeansZero[rowIndex, columnIndex2];
                if (useCorrel)
                {
                    denomatorX1 += matrixWithColumnMeansZero[rowIndex, columnIndex1] * matrixWithColumnMeansZero[rowIndex, columnIndex1];
                    denomatorX2 += matrixWithColumnMeansZero[rowIndex, columnIndex2] * matrixWithColumnMeansZero[rowIndex, columnIndex2];
                }
            }

            if (useCorrel)
            {
                double correl = numerator / Math.Sqrt(denomatorX1 * denomatorX2);
                return correl;
            }
            else
            {
                double covariance = numerator / matrixWithColumnMeansZero.RowCount;
                return covariance;
            }
        }


        //static public double IntAsCharToDoubleWithLimits(char c, int minValue, int maxValue)
        //{
        //    double r = (double)int.Parse(c.ToString());
        //    Helper.CheckCondition(minValue <= r && r <= maxValue, string.Format("Expect value to between {0} and {1}, inclusive", minValue, maxValue));
        //    return r;
        //}

        //static public char DoubleToIntAsCharWithLimits(double r, int minValue, int maxValue)
        //{
        //    Helper.CheckCondition(minValue <= r && r <= maxValue, string.Format("Expect value to between {0} and {1}, inclusive", minValue, maxValue));
        //    return SpecialFunctions.FirstAndOnly(r.ToString());
        //}


        public static MatrixFactory<string, string, char> MatrixFactorySSC = MatrixFactory<string, string, char>.GetInstance();


    }

    public static class MatrixExtensions
    {

        static public TMatrix Standardize<TMatrix>(this TMatrix matrix, int maxMatrixVal, MatrixFactoryDelegate<TMatrix, string, string, double> zeroMatrixFractory, ParallelOptions parallelOptions) where TMatrix : Matrix<string, string, double>
        {
            TMatrix snpMatrixNew = EigenstratMain.StandardizeGToCreateX(maxMatrixVal, matrix, zeroMatrixFractory, parallelOptions);
            return snpMatrixNew;
        }

        static public ShoMatrix Standardize<TMatrix>(this TMatrix matrix, int maxMatrixVal, ParallelOptions parallelOptions) where TMatrix : Matrix<string, string, double>
        {
            if (matrix.RowCount > 0)
            {
                ShoMatrix snpMatrixNew = EigenstratMain.StandardizeGToCreateX<ShoMatrix>(maxMatrixVal, matrix, ShoMatrix.CreateDefaultInstance, parallelOptions);
                return snpMatrixNew;
            }
            else
            {
                return new ShoMatrix(matrix);
            }
        }
      


        static public ShoMatrix Standardize(this ShoMatrix shoMatrix, int maxMatrixVal, ParallelOptions parallelOptions, bool onlyMeanCenter)
        {
            ShoMatrix snpMatrixNew = EigenstratMain.StandardizeGToCreateX<ShoMatrix>(maxMatrixVal, shoMatrix, ShoMatrix.CreateDefaultInstance, parallelOptions,onlyMeanCenter);
            return snpMatrixNew;
        }

    }
}
