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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Bio.Matrix;
using Bio.Util;
using System.Threading.Tasks;

namespace MBT.Escience.Matrix
{
    public class LinearTransformRowsView<TRowKey, TColKey> : Matrix<TRowKey, TColKey, double>
    {
        public Matrix<TRowKey, TColKey, double> UntransformedMatrix;
        public Matrix<TRowKey, string, LinearTransform> LinearTransformMatrix;

        public override bool TryGetValue(TRowKey rowKey, TColKey colKey, out double value)
        {
            double untransformedValue = UntransformedMatrix.GetValueOrMissing(rowKey, colKey);
            LinearTransform linearTransform = LinearTransformMatrix[rowKey, ""];
            value = linearTransform.Transform(untransformedValue);

            if (IsMissing(value))
            {
                value = double.NaN;
                return false;
            }

            return true;
        }

        public override bool TryGetValue(int rowIndex, int colIndex, out double value)
        {
            double untransformedValue = UntransformedMatrix.GetValueOrMissing(rowIndex, colIndex);
            LinearTransform linearTransform = LinearTransformMatrix[rowIndex, 0];
            value = linearTransform.Transform(untransformedValue);

            if (IsMissing(value))
            {
                value = double.NaN;
                return false;
            }

            return true;
        }

        public override void SetValueOrMissing(TRowKey rowKey, TColKey colKey, double value)
        {
            LinearTransform linearTransform = LinearTransformMatrix[rowKey, ""];
            double untransformedValue = linearTransform.Inverse.Transform(value);
            Helper.CheckCondition(!IsMissing(untransformedValue), "Untransform to the missing value is not allowed.");
            UntransformedMatrix.SetValueOrMissing(rowKey, colKey, untransformedValue);
        }

        public override void SetValueOrMissing(int rowIndex, int colIndex, double value)
        {
            LinearTransform linearTransform = LinearTransformMatrix[rowIndex, 0];
            double untransformedValue = linearTransform.Inverse.Transform(value);
            Helper.CheckCondition(!IsMissing(untransformedValue), "Untransform to the missing value is not allowed.");
            UntransformedMatrix.SetValueOrMissing(rowIndex, colIndex, untransformedValue);
        }

        public override ReadOnlyCollection<TRowKey> RowKeys
        {
            get { return UntransformedMatrix.RowKeys; }
        }

        public override ReadOnlyCollection<TColKey> ColKeys
        {
            get { return UntransformedMatrix.ColKeys; }
        }

        public override IDictionary<TRowKey, int> IndexOfRowKey
        {
            get { return UntransformedMatrix.IndexOfRowKey; }
        }

        public override IDictionary<TColKey, int> IndexOfColKey
        {
            get { return UntransformedMatrix.IndexOfColKey; }
        }

        public override double MissingValue
        {
            get { return double.NaN; }
        }


        //!!!could also have a version with a dictionary from RowKeys to LinearTransforms
        internal void SetUp(Matrix<TRowKey, TColKey, double> parentMatrix, IList<LinearTransform> linearTransformList)
        {
            Helper.CheckCondition(parentMatrix.IsMissing(double.NaN), "The parent matrix of a linear transform rows view must have NaN as its missing value");
            UntransformedMatrix = parentMatrix;
            LinearTransformMatrix = DenseMatrix<TRowKey, string, LinearTransform>.CreateDefaultInstance(parentMatrix.RowKeys, "".AsSingletonEnumerable(), null);
            Helper.CheckCondition(linearTransformList.Count == parentMatrix.RowCount, "Expect one item in linearTransformList for each row of the parent matrix");
            for (int rowIndex = 0; rowIndex < linearTransformList.Count; ++rowIndex)
            {
                LinearTransformMatrix[rowIndex, 0] = linearTransformList[rowIndex];
            }
        }
        internal void SetUp(Matrix<TRowKey, TColKey, double> parentMatrix, Matrix<TRowKey, string, LinearTransform> linearTransformMatrix)
        {
            Helper.CheckCondition(parentMatrix.IsMissing(double.NaN), "The parent matrix of a linear transform rows view must have NaN as its missing value");
            Helper.CheckCondition(parentMatrix.RowKeys.SequenceEqual(linearTransformMatrix.RowKeys), "Expect the parentMatrix and the linearTransformMatrix to have the same rowKeys in the same order.");
            UntransformedMatrix = parentMatrix;
            LinearTransformMatrix = linearTransformMatrix;
        }

    }
}
