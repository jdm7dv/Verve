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
using MBT.Escience.Sho;
using Bio.Matrix;
using Bio.Util;
using System.Threading.Tasks;
using MBT.Escience.Parse;
using MBT.Escience;

namespace MBT.Escience.Matrix.RowNormalizers
{
    public class EigenstratStandardize : RowNormalizer, IParsable
    {
        public EigenstratStandardize(int maxValue)
        {
            MaxValue = maxValue;
            FinalizeParse();
        }

        public EigenstratStandardize()
        {
        }


        [Parse(ParseAction.Required)]
        public int MaxValue;


        public override LinearTransform CreateLinearTransform(IList<double> rowArrayWithNaN, string rowKeyOrNull)
        {
            var sumAndCount = SpecialFunctions.SumAndCount(rowArrayWithNaN);
            double rowSum = sumAndCount.Item1;
            double rowCount = (double)sumAndCount.Item2;

            double rowMean = rowSum / rowCount;
            double piSmoothedCount = (1.0 + rowSum)
                                   / (2.0 + MaxValue * rowCount);
            double stdDevPi = Math.Sqrt(piSmoothedCount * (1 - piSmoothedCount));

            var linearTransform = new LinearTransform(1.0 / stdDevPi, -rowMean / stdDevPi, 0);
            return linearTransform;

        }

        //public override void Apply(ref DoubleArray inputSnpArray)
        //{
        //    {
        //        StaticApply(inputSnpArray, MaxValue);
        //    }

        //}

        //public static void StaticApply(DoubleArray x, int maxValue, bool onlyMeanCenter = false)
        //{
        //    var counterWithMessages = new CounterWithMessages("EigenstratStandardize: row #{0}", null, x.size0);

        //    Parallel.For(0, x.size0, ParallelOptionsScope.Current, rowIndex =>
        //    {
        //        counterWithMessages.Increment();

        //        //The paper divides by (2+2*Count) because its values are 0,1,2. Because ours are 0,1, we divide by (2+Count)
        //        DoubleArray row = x.GetRow(rowIndex); //GetRow does only a shallow copy
        //        var countAndSumOfNotMissing = row.Where(value => !double.IsNaN(value)).Aggregate(Tuple.Create(0, 0.0), (running, newVal) => Tuple.Create(running.Item1 + 1, running.Item2 + newVal));
        //        double rowCount = countAndSumOfNotMissing.Item1;
        //        Helper.CheckCondition(rowCount != 0, () => string.Format("Row {0} is all NaN, so can't compute mean", rowIndex));
        //        double rowSum = countAndSumOfNotMissing.Item2;
        //        double rowMean = rowSum / rowCount;
        //        double piSmoothedCount = (1.0 + rowSum)
        //                               / (2.0 + maxValue * rowCount);
        //        double stdDevPi = onlyMeanCenter ? 1 : Math.Sqrt(piSmoothedCount * (1 - piSmoothedCount));

        //        for (int colIndex = 0; colIndex < x.size1; ++colIndex)
        //        {
        //            double oldValue = x[rowIndex, colIndex];
        //            double newValue = double.IsNaN(oldValue) ? 0.0 : (oldValue - rowMean) / stdDevPi;
        //            x[rowIndex, colIndex] = newValue;
        //        }
        //    });
        //    Console.WriteLine();
        //}


        //#region IParsable Members

        public void FinalizeParse()
        {
            Helper.CheckCondition(0 < MaxValue, "MaxValue must be greater than 0.");
        }

        //#endregion

        //public override void ALinearTranformationAndFillInValueForEachRow(Matrix<string, string, double> snpArray)
        //{
        //    throw new NotImplementedException();
        //}
    }

    //public static class MatrixExtensions
    //{

    //    static public TMatrix Standardize<TMatrix>(this TMatrix matrix, int maxMatrixVal, MatrixFactoryDelegate<TMatrix, string, string, double> zeroMatrixFractory, ParallelOptions parallelOptions) where TMatrix : Matrix<string, string, double>
    //    {
    //        TMatrix snpMatrixNew = EigenstratStandardize.StandardizeGToCreateX(maxMatrixVal, matrix, zeroMatrixFractory, parallelOptions);
    //        return snpMatrixNew;
    //    }

    //    static public ShoMatrix Standardize<TMatrix>(this TMatrix matrix, int maxMatrixVal, ParallelOptions parallelOptions) where TMatrix : Matrix<string, string, double>
    //    {
    //        ShoMatrix snpMatrixNew = EigenstratStandardize.StandardizeGToCreateX<ShoMatrix>(maxMatrixVal, matrix, ShoMatrix.CreateDefaultInstance, parallelOptions);
    //        return snpMatrixNew;
    //    }


    //    static public ShoMatrix Standardize(this ShoMatrix shoMatrix, int maxMatrixVal, ParallelOptions parallelOptions, bool onlyMeanCenter)
    //    {
    //        ShoMatrix snpMatrixNew = EigenstratStandardize.StandardizeGToCreateX<ShoMatrix>(maxMatrixVal, shoMatrix, ShoMatrix.CreateDefaultInstance, parallelOptions, onlyMeanCenter);
    //        return snpMatrixNew;
    //    }

    //}

}
