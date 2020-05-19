// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bio.Util;

namespace Bio.Matrix
{
    /// <summary>
    /// Creates a view on a matrix in which values converted one-to-one. For example, Suppose you have a matrix with
    /// char values such as '0', '1', '2' (with '?' for missing) and you need doubles (with double.NaN as missing).
    /// With this class you can wrap your original matrix, making it act like the matrix you need.
    /// </summary>
    /// <typeparam name="TRowKey">The type of the row key. Usually "String"</typeparam>
    /// <typeparam name="TColKey">The type of the col key. Usually "String"</typeparam>
    /// <typeparam name="TValueView">The type of desired values.</typeparam>
    /// <typeparam name="TValueParent">The type of the current values.</typeparam>
    public class ConvertValueView<TRowKey, TColKey, TValueView, TValueParent> : Matrix<TRowKey, TColKey, TValueView>
    {

        /// <summary>
        /// Get the original matrix that this view wraps.
        /// </summary>
        public Matrix<TRowKey, TColKey, TValueParent> ParentMatrix { get; internal set; }


        internal ConvertValueView()
        {
        }

        private TValueView _missingValue;

        /// <summary>
        /// A function that converts a value of the wrapped matrix into a value of the wrapping matrix.
        /// </summary>
        public Converter<TValueParent, TValueView> ParentValueToViewValue { get; private set; }
        /// <summary>
        /// A function that converts value of the wrapped matrix into a value of the wrapped matrix.
        /// </summary>
        public Converter<TValueView, TValueParent> ViewValueToParentValue { get; private set; }

        internal void SetUp(Matrix<TRowKey, TColKey, TValueParent> parentMatrix, ValueConverter<TValueParent, TValueView> converter, TValueView missingValue)
        {
            ParentMatrix = parentMatrix;
            _missingValue = missingValue;
            ParentValueToViewValue = converter.ConvertForward;
            ViewValueToParentValue = converter.ConvertBackward;
        }

        #pragma warning disable 1591
        public override ReadOnlyCollection<TRowKey> RowKeys
        #pragma warning restore 1591
        {
            get { return ParentMatrix.RowKeys; }
        }

        #pragma warning disable 1591
        public override ReadOnlyCollection<TColKey> ColKeys
        #pragma warning restore 1591
        {
            get { return ParentMatrix.ColKeys; }
        }

        #pragma warning disable 1591
        public override IDictionary<TRowKey, int> IndexOfRowKey
        #pragma warning restore 1591
        {
            get { return ParentMatrix.IndexOfRowKey; }
        }

        #pragma warning disable 1591
        public override IDictionary<TColKey, int> IndexOfColKey
        #pragma warning restore 1591
        {
            get { return ParentMatrix.IndexOfColKey; }
        }

        #pragma warning disable 1591
        public override bool TryGetValue(TRowKey rowKey, TColKey colKey, out TValueView value)
        #pragma warning restore 1591
        {
            TValueParent valueParent;
            if (ParentMatrix.TryGetValue(rowKey, colKey, out valueParent))
            {
                value = ParentValueToViewValue(valueParent);
                return true;
            }
            else
            {
                value = MissingValue;
                return false;
            }
        }

        #pragma warning disable 1591
        public override bool TryGetValue(int rowIndex, int colIndex, out TValueView value)
        #pragma warning restore 1591
        {
            TValueParent valueParent;
            if (ParentMatrix.TryGetValue(rowIndex, colIndex, out valueParent))
            {
                value = ParentValueToViewValue(valueParent);
                return true;
            }
            else
            {
                value = MissingValue;
                return false;
            }
        }

        #pragma warning disable 1591
        public override TValueView MissingValue
        #pragma warning restore 1591
        {
            get { return _missingValue; }
        }

#pragma warning disable 1591
        public override void SetValueOrMissing(TRowKey rowKey, TColKey colKey, TValueView value)
#pragma warning restore 1591
        {
            ParentMatrix.SetValueOrMissing(rowKey, colKey, ViewValueOrMissingToParentValueOrMissing(value));
        }

#pragma warning disable 1591
        public override void SetValueOrMissing(int rowIndex, int colIndex, TValueView value)
#pragma warning restore 1591
        {
            ParentMatrix.SetValueOrMissing(rowIndex, colIndex, ViewValueOrMissingToParentValueOrMissing(value));
        }

        private TValueParent ViewValueOrMissingToParentValueOrMissing(TValueView value)
        {
            if (IsMissing(value))
            {
                return ParentMatrix.MissingValue;
            }
            else
            {
                return ViewValueToParentValue(value);
            }
        }

    }
}
