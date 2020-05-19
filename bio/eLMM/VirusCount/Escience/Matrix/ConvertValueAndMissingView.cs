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
using System.Collections.ObjectModel;
using Bio.Util;
using System.Collections.Generic;

namespace Bio.Matrix
{
    /// <summary>
    /// The converters here convert both non-missing values and the missing value. (In contrast, the converters in ConvertValueView only convert non-missing values)
    /// </summary>
    /// <typeparam name="TRowKey"></typeparam>
    /// <typeparam name="TColKey"></typeparam>
    /// <typeparam name="TValueView"></typeparam>
    /// <typeparam name="TValueParent"></typeparam>
    public class ConvertValueAndMissingView<TRowKey, TColKey, TValueView, TValueParent> : ConvertValueView<TRowKey, TColKey, TValueView, TValueParent>
    {

        internal ConvertValueAndMissingView() : base()
        {
        }

#pragma warning disable 1591
        public override bool TryGetValue(TRowKey rowKey, TColKey colKey, out TValueView valueView)
#pragma warning restore 1591
        {
            TValueParent parentValueOrMissing = ParentMatrix.GetValueOrMissing(rowKey, colKey);
            valueView = ParentValueToViewValue(parentValueOrMissing);
            if (!IsMissing(valueView))
            {
                return true;
            }
            else
            {
                valueView = MissingValue;
                return false;
            }
        }

#pragma warning disable 1591
        public override bool TryGetValue(int rowIndex, int colIndex, out TValueView valueView)
#pragma warning restore 1591
        {
            TValueParent parentValueOrMissing = ParentMatrix.GetValueOrMissing(rowIndex, colIndex);
            valueView = ParentValueToViewValue(parentValueOrMissing);
            if (!IsMissing(valueView))
            {
                return true;
            }
            else
            {
                valueView = MissingValue;
                return false;
            }
        }


#pragma warning disable 1591
        public override void SetValueOrMissing(TRowKey rowKey, TColKey colKey, TValueView value)
#pragma warning restore 1591
        {
            ParentMatrix.SetValueOrMissing(rowKey, colKey, ViewValueToParentValue(value));
        }

#pragma warning disable 1591
        public override void SetValueOrMissing(int rowIndex, int colIndex, TValueView value)
#pragma warning restore 1591
        {
            ParentMatrix.SetValueOrMissing(rowIndex, colIndex, ViewValueToParentValue(value));
        }


        internal void SetUp(Matrix<TRowKey, TColKey, TValueParent> parentMatrix, ValueConverter<TValueParent, TValueView> converter, TValueView missingValue)
        {
            base.SetUp(parentMatrix, converter, missingValue);
        }
    }
}
