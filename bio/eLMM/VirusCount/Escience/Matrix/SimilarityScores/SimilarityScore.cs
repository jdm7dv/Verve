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
using System.Threading.Tasks;
using Bio.Matrix;
using ShoNS.Array;
using MBT.Escience.Sho;
using Bio.Util;
using MBT.Escience.Matrix.RowNormalizers;
using MBT.Escience.Parse;
using MBT.Escience.Matrix.KernelNormalizers;

namespace MBT.Escience.Matrix.SimilarityScores
{
    abstract public class SimilarityScore : IParsable
    {
        [Parse(ParseAction.Required)]
        public RowNormalizer RowNormalizer;

        [Parse(ParseAction.Optional)]
        public IList<KernelNormalizer> KernelNormalizerList = new List<KernelNormalizer>();

        private Matrix<string, string, double> ToKernel(ref ShoMatrix shoMatrix)
        {
            RowNormalizer.NormalizeInPlace(ref shoMatrix);
            Matrix<string, string, double> kernel = JustKernel(shoMatrix);
            KernelNormalizeInPlace(ref kernel);
            return kernel;
        }

        private Matrix<string, string, double> ToKernel(Matrix<string, string, double> matrixIn)
        {
            ShoMatrix shoMatrix = matrixIn.ToShoMatrix(); //Will always make a copy
            Matrix<string, string, double> kernel = ToKernel(ref shoMatrix);
            return kernel;
        }

        public Matrix<string,string,double> ToKernel(Matrix<string, string, double> unnormalizedInput, ParallelOptions parallelOptions, int? cidInBatchCountOrNull = null)
        {
            Matrix<string, string, double> kernel = null;
            using(ParallelOptionsScope.Create(parallelOptions))
            {
                kernel = ToKernel(unnormalizedInput, cidInBatchCountOrNull);
            }
            return kernel;

        }

        // Does RowNormalize, but doesn't kernel normalize
        public Matrix<string, string, double> ToSubKernel(Matrix<string, string, double> unnormalizedAndUnfilteredInput, int cidInBatchCount, int batchCount, int i, int j, List<EScienceLearn.RowFilters.RowFilter> RowFilterList)
        {

            //!!!could cache the twoMatrices, so if one or both is the same, wouldn't reload.
            var unfilteredInput = RowNormalizer.Normalize(unnormalizedAndUnfilteredInput);
            var input = EScienceLearn.RowFilters.RowFilter.ApplyList(RowFilterList, unfilteredInput, null);


            var cidListList = SpecialFunctions.DivideListIntoEqualChunksFromChunkSize<string>(input.ColKeys, cidInBatchCount);
            Helper.CheckCondition(cidListList.Count == batchCount, "Expect the cidListList.Count == batchCount");
            Console.WriteLine("cids divided into {0} batches of about {1}", cidListList.Count, cidInBatchCount);
            Helper.CheckCondition(cidListList.Sum(l => l.Count) == input.ColCount, "real assert");


            Console.WriteLine("Loading batch {0}, size {1}x{2}", i, cidListList[i].Count, input.RowCount);
            var matrixI = input.SelectColsView(cidListList[i]).ToShoMatrix(verbose: true);

            Console.WriteLine("Loading batch {0}, size {1}x{2}", j, cidListList[j].Count, input.RowCount);

            if (i == j)
            {
                ShoMatrix kii = JustKernel(matrixI);
                return kii;
            }
            else
            {
                var matrixJ = input.SelectColsView(cidListList[j]).ToShoMatrix(verbose: true);
                ShoMatrix kij = JustKernel(matrixI, matrixJ);
                return kij;
            }

        }


        public Matrix<string, string, double> ToKernel(Matrix<string, string, double> unnormalizedInput, int? cidInBatchCountOrNull = null)
        {
            if (null == cidInBatchCountOrNull)
            {
                return ToKernel(unnormalizedInput);
            }

            var input = RowNormalizer.Normalize(unnormalizedInput);


            var cidListList = SpecialFunctions.DivideListIntoEqualChunksFromChunkSize<string>(input.ColKeys, cidInBatchCountOrNull.Value);
            Console.WriteLine("cids divided into {0} batches of about {1}", cidListList.Count, cidInBatchCountOrNull);
            Helper.CheckCondition(cidListList.Sum(l => l.Count) == input.ColCount, "real assert");


            var counterWithMessages = new CounterWithMessages("kernel combintations ", 1, (cidListList.Count * cidListList.Count + cidListList.Count) / 2);

            var kernelPieces2D = new Matrix<string, string, double>[cidListList.Count,cidListList.Count];
            for (int i = 0; i < cidListList.Count; ++i)
            {
                Console.WriteLine("Loading batch {0}, size {1}x{2}", i, cidListList[i].Count, unnormalizedInput.RowCount);
                var matrixI = input.SelectColsView(cidListList[i]).ToShoMatrix(verbose:true);

                Parallel.For(i, cidListList.Count, ParallelOptionsScope.Current, j =>
                {
                    Console.WriteLine("Loading batch {0}, size {1}x{2}", j, cidListList[j].Count, unnormalizedInput.RowCount);

                    if (i == j)
                    {
                        ShoMatrix kii = JustKernel(matrixI);
                        kernelPieces2D[i, i] = kii;
                    }
                    else
                    {
                        var matrixJ = input.SelectColsView(cidListList[j]).ToShoMatrix(verbose:true);
                        ShoMatrix kij = JustKernel(matrixI, matrixJ);
                        kernelPieces2D[i, j] = kij;
                        kernelPieces2D[j, i] = kij.TransposeView().ToShoMatrix();
                    }
                    counterWithMessages.Increment();
                });
            }
            counterWithMessages.Finished();


            var output = MatrixExtensions.MergeRowsAndColsView(kernelPieces2D);
            Helper.CheckCondition(output.RowKeys.SequenceEqual(output.ColKeys) && output.ColKeys.SequenceEqual(unnormalizedInput.ColKeys), "Assert: MergeRows isn't working as expected");

            KernelNormalizeInPlace(ref output);
            return output;
        }

        private static Dictionary<string, List<double>> CreateCidToDoubleList(Matrix<string, string, double> matrix)
        {
            CounterWithMessages counterWithMessages = new CounterWithMessages("loading cid columns ", null, matrix.ColCount, quiet: matrix.ColCount < 10);
            var listOfLists = ((IList<IList<double>>)matrix.TransposeView());
            var cidToDoubleList =
                (from colKey in matrix.ColKeys
                    .AsParallel().WithParallelOptionsScope()
                 let colIndex = matrix.IndexOfColKey[colKey]
                 let doubleList = CreateDoubleList(colIndex, listOfLists, counterWithMessages)
                 select Tuple.Create(colKey, doubleList)
                ).ToDictionary();
            counterWithMessages.Finished();
            return cidToDoubleList;
        }

        private ShoMatrix JustKernel(ShoMatrix normalizedShoMatrix)
        {
            Helper.CheckCondition(double.IsNaN(normalizedShoMatrix.MissingValue), "Expect shoMatrix's missing value to be NaN");
            DoubleArray normalizedArray = normalizedShoMatrix.DoubleArray;
            DoubleArray doubleArrayKernel = JustKernel(normalizedArray);
            ShoMatrix kernel = new ShoMatrix(doubleArrayKernel, normalizedShoMatrix.ColKeys, normalizedShoMatrix.ColKeys, double.NaN);

            return kernel;
        }

        private ShoMatrix JustKernel(ShoMatrix normalizedShoMatrixA, ShoMatrix normalizedShoMatrixB)
        {
            Helper.CheckCondition(double.IsNaN(normalizedShoMatrixA.MissingValue) && double.IsNaN(normalizedShoMatrixA.MissingValue), "Expect the input matrices' missing value to be NaN");
            DoubleArray normalizedArrayA = normalizedShoMatrixA.DoubleArray;
            DoubleArray normalizedArrayB = normalizedShoMatrixB.DoubleArray;
            DoubleArray doubleArrayKernel = JustKernel(normalizedArrayA, normalizedArrayB);
            ShoMatrix kernel = new ShoMatrix(doubleArrayKernel, normalizedShoMatrixA.ColKeys, normalizedShoMatrixB.ColKeys, double.NaN);

            return kernel;
        }


        protected virtual DoubleArray JustKernel(DoubleArray normalizedArrayA, DoubleArray normalizedArrayB)
        {
            DoubleArray normalizedArray0 = normalizedArrayA;
            DoubleArray normalizedArray1 = normalizedArrayB;
            int N0 = normalizedArrayA.size1;
            int N1 = normalizedArrayB.size1;
            DoubleArray outputMatrix = new DoubleArray(N0, N1); //Don't init becase every value will be set.
            Parallel.For(0, N0, ParallelOptionsScope.Current, n0 =>
            {
                for (int n1 = 0; n1 < N1; n1++)
                {
                    double score = JustKernel((IList<double>)normalizedArray0.GetCol(n0), (IList<double>)normalizedArray1.GetCol(n1));
                    outputMatrix[n0, n1] = score;
                }
            });
            return outputMatrix;

        }


        protected virtual DoubleArray JustKernel(DoubleArray normalizedArray)
        {
            DoubleArray normalizedArray2 = normalizedArray;
            int N = normalizedArray.size1;
            DoubleArray outputMatrix = new DoubleArray(N, N); //Don't init becase every value will be set.
            Parallel.For(0, N, ParallelOptionsScope.Current, n0 =>
            {
                for (int n1 = n0; n1 < N; n1++)
                {
                    double score = JustKernel((IList<double>)normalizedArray2.GetCol(n0), (IList<double>)normalizedArray2.GetCol(n1));
                    outputMatrix[n0, n1] = score;
                    outputMatrix[n1, n0] = score;
                }
            });
            return outputMatrix;
        }

        protected abstract double JustKernel(IList<double> normalizedList1, IList<double> normalizedList2);

        public void KernelNormalizeInPlace(ref Matrix<string,string,double> kernel)
        {
            foreach (var kernelNormalizer in KernelNormalizerList)
            {
                kernelNormalizer.NormalizeInPlace(ref kernel);
            }
        }

        //Create a method just so we can have the side effect of counting with counterWithMessages
        private static List<double> CreateDoubleList(int colIndex, IList<IList<double>> listOfLists, CounterWithMessages counterWithMessages)
        {
            counterWithMessages.Increment();
            return listOfLists[colIndex].ToList();
        }



        #region IParsable Members

        public void FinalizeParse()
        {
        }

        #endregion


    }
}
