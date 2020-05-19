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
using Bio.Matrix;
using Bio.Util;
using System.Threading.Tasks;
using MBT.Escience;
using MBT.Escience.Parse;
using MBT.Escience.Sho;
using MBT.Escience.Matrix;
using ShoNS.Array;

namespace MBT.Escience.Matrix.RowNormalizers
{
    [Serializable]
    abstract public class RowNormalizer
    {
        [Parse(ParseAction.Optional)]
        public bool Verbose = false;

        public virtual Matrix<string, string, LinearTransform> LinearTransformMatrix(Matrix<string, string, double> inputMatrix)
        {
            var counterWithMessages = new CounterWithMessages("RowNormalizing ", null, inputMatrix.RowCount, quiet:!Verbose);
            var linearTransformMatrix = DenseMatrix<string, string, LinearTransform>.CreateDefaultInstance(inputMatrix.RowKeys, new[] { "" }, null);
            Parallel.ForEach(inputMatrix.AppendIndex(), ParallelOptionsScope.Current, unnormalizedListAndIndex =>
            {
                counterWithMessages.Increment();
                var linearTransform = CreateLinearTransform(unnormalizedListAndIndex.Item1, inputMatrix.RowKeys[unnormalizedListAndIndex.Item2]);
                linearTransformMatrix[unnormalizedListAndIndex.Item2, 0] = linearTransform;
            });
            counterWithMessages.Finished();
            return linearTransformMatrix;
        }

        public abstract LinearTransform CreateLinearTransform(IList<double> unnormalizedList, string rowKeyOrNull);

        public void NormalizeInPlace(ref ShoMatrix matrix)
        {
            var matrix2 = matrix;
            DoubleArray doubleArray = matrix.DoubleArray;
            NormalizeInPlace(ref doubleArray, rowIndex => matrix2.RowKeys[rowIndex]);
            matrix.DoubleArray = doubleArray;
        }


        //Shooptimization -might be able to do this faster with whole row mult, add, and value fill
        public void NormalizeInPlace(ref DoubleArray doubleArray, Func<int, string> rowIndexToRowStringOrNull = null)
        {
            DoubleArray doubleArray2 = doubleArray;
            Parallel.For(0, doubleArray.size0, ParallelOptionsScope.Current, rowIndex =>
            {
                IList<double> row = doubleArray2.GetRow(rowIndex);
                string rowKey = rowIndexToRowStringOrNull == null ? null : rowIndexToRowStringOrNull(rowIndex);
                var linearTransform = CreateLinearTransform(row, rowKey);
                linearTransform.TransformInPlace(ref row);
            });
            doubleArray = doubleArray2;
        }

        public void NormalizeInPlace(ref ShoMatrix matrix, ParallelOptions parallelOptions)
        {
            var matrix2 = matrix;
            using (ParallelOptionsScope.Create(parallelOptions))
            {
                NormalizeInPlace(ref matrix2);
            }
            matrix = matrix2;
        }

        public Matrix<string, string, double> Normalize(Matrix<string, string, double> input, ParallelOptions parallelOptions)
        {
            Matrix<string, string, double> output = null;
            using(ParallelOptionsScope.Create(parallelOptions))
            {
                output = Normalize(input);
            }
            return input;
        }


        virtual public Matrix<string, string, double> Normalize(Matrix<string, string, double> input)
        {
            var linearTransformMatrix = LinearTransformMatrix(input);
            var output = input.LinearTransformRowsView(linearTransformMatrix);
            return output;
        }


        public override string ToString()
        {
            string name = ConstructorArguments.FromParsable(this).ToString();
            return name;
        }

        [Parse(ParseAction.ArgumentString)]
        public string ArgumentString; //Can't be private because if it was the derived types could not see its value.

    }
}

