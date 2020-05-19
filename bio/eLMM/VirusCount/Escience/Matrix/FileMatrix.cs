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
using MBT.Escience.Parse;
using System.IO;

namespace MBT.Escience.Matrix
{
    //[Parsable]
    [ParseAsNonCollection]
    public class FileMatrix<TValue> : Matrix<string, string, TValue>, IParsable
    {

        /// <summary>
        /// The character used for missing values.
        /// </summary>
        public TValue MissingValueForParse = default(TValue);
        /// <summary>
        /// A ValueConverter for converting between char and TValue. This is used
        /// if you want to leave the data on disk
        /// </summary>
        public ValueConverter<char, TValue> CharToTValConverter = null;
        public ValueConverter<double, TValue> DoubleToTValConverter = null;

        /// <summary>
        /// Indicates that all rows or cols must match when merging matrices.
        /// </summary>
        public bool MatchRequired = true;

        /// <summary>
        /// Send messages to console telling what processing is happening.
        /// </summary>
        public bool Verbose = false;

        /// <summary>
        /// Specifies if you want to merge the rows of the matrices or the cols.
        /// </summary>
        public MergeType MergeType = MergeType.Row;

        /// <summary>
        /// Loads DenseAnsi or RowKeysAnsi files as a RowKeysAnsi matrix. This leaves the data on the disk.
        /// </summary>
        [Parse(ParseAction.Optional)]
        public bool LeaveOnDiskIfPossible = false;

        /// <summary>
        /// If the file is in DenseAnsi format and loaded as a RowKeyAnsi matrix, this will write a RowKeysAnsi file
        /// to the disk. The name of this file not include any directory information. It will be created in the DenseAnsi files's directory.
        /// In later runs, the RowsKeyAnsi file can be used in place of the DenseAnsi file for even faster loading.
        /// </summary>
        [Parse(ParseAction.Optional)]
        public string CreateRowKeysFileOrNull = null;


        [Parse(ParseAction.Required, typeof(InputFileList))]
        public List<FileInfo> InputFiles;


        [Parse(ParseAction.Ignore)]
        public Lazy<Matrix<string, string, TValue>> _lazyMatrix = null;

        [Parse(ParseAction.Ignore)]
        public Matrix<string, string, TValue> Parent
        {
            get
            {
                return _lazyMatrix.Value;
            }
        }

        [Parse(ParseAction.Ignore)]
        public MatrixFactory<string, string, TValue> MatrixFactory = MatrixFactory<string, string, TValue>.GetInstance();

        //!!!similar code elsewhere
        public virtual Matrix<string, string, TValue> LoadParent()
        {
            var matrixArray =
                (
                    from file in InputFiles //Bio.Util.FileUtils.GetFiles(Pattern, /*zeroIsOK:*/ false)
                    let matrix = LoadParentMatrix(file.ToString())
                    select matrix
                ).ToArray();

            if (matrixArray.Length == 1)
            {
                return matrixArray[0];
            }
            switch (MergeType)
            {
                case MergeType.Col:
                    return new MergeColsView<string, string, TValue>(rowsMustMatch: MatchRequired, matrices: matrixArray);
                case MergeType.Row:
                    return new MergeRowsView<string, string, TValue>(colsMustMatch: MatchRequired, matrices: matrixArray);
                case MergeType.Both:
                    return MatrixExtensions.MergeRowsAndColsView<string, string, TValue>(mustMatch: MatchRequired, matrices: matrixArray);
                default:
                    throw new Exception("Not possible");
            }
        }


        private Matrix<string, string, TValue> LoadParentMatrix(string name)
        {
            if (Verbose)
            {
                Console.WriteLine("Loading " + name);
            }

            if (LeaveOnDiskIfPossible)
            {
                try
                {
                    RowKeysAnsi predictorCharOriginal0 = RowKeysAnsi.GetInstanceFromRowKeysAnsi(name, ParallelOptionsScope.Current);

                    Matrix<string, string, TValue> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(CharToTValConverter, MissingValueForParse);
                    //Matrix<string,string,TValue> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(Bio.Util.ValueConverter.CharToDouble, double.NaN);

                    Helper.CheckCondition(null == CreateRowKeysFileOrNull, "Already reading from a RowKeyAnsi file, so can't create one.");
                    return predictorCharOriginal;
                }
                catch (MatrixFormatException)
                {//ignore
                }


                try
                {
                    RowKeysAnsi predictorCharOriginal0 = RowKeysAnsi.GetInstanceFromDenseAnsi(name, ParallelOptionsScope.Current);


                    if (null != CreateRowKeysFileOrNull)
                    {
                        predictorCharOriginal0.WriteRowKeys(CreateRowKeysFileOrNull);
                    }

                    Matrix<string, string, TValue> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(CharToTValConverter, MissingValueForParse);
                    //Matrix<string, string, TValue> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(Bio.Util.ValueConverter.CharToDouble, double.NaN);
                    return predictorCharOriginal;
                }
                catch (MatrixFormatException)
                {//ignore
                }

                try
                {
                    RowKeysPaddedDouble predictorCharOriginal0 = RowKeysPaddedDouble.GetInstanceFromRowKeys(name, ParallelOptionsScope.Current);

                    Matrix<string, string, TValue> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(DoubleToTValConverter, MissingValueForParse);
                    //Matrix<string,string,TValue> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(Bio.Util.ValueConverter.CharToDouble, double.NaN);

                    Helper.CheckCondition(null == CreateRowKeysFileOrNull, "Already reading from a RowKeyAnsi file, so can't create one.");
                    return predictorCharOriginal;
                }
                catch (MatrixFormatException)
                {//ignore
                }


                try
                {
                    RowKeysPaddedDouble predictorCharOriginal0 = RowKeysPaddedDouble.GetInstanceFromPaddedDouble(name, ParallelOptionsScope.Current);


                    if (null != CreateRowKeysFileOrNull)
                    {
                        predictorCharOriginal0.WriteRowKeys(CreateRowKeysFileOrNull);
                    }

                    Matrix<string, string, TValue> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(DoubleToTValConverter, MissingValueForParse);
                    //Matrix<string, string, TValue> predictorCharOriginal = predictorCharOriginal0.ConvertValueView(Bio.Util.ValueConverter.CharToDouble, double.NaN);
                    return predictorCharOriginal;
                }
                catch (MatrixFormatException)
                {//ignore
                }
            }

            Matrix<string, string, TValue> predictorCharOriginalX = MatrixFactory.Parse(name, MissingValueForParse, ParallelOptionsScope.Current);
            Helper.CheckCondition(null == CreateRowKeysFileOrNull, "Not creating from a DenseAnsi file, so can't create a RowKeyAnsi file.");
            return predictorCharOriginalX;
        }


        #region IParsable Members

        public void FinalizeParse()
        {
            //Bio.Util.FileUtils.GetFiles(Pattern, /*zeroIsOK:*/ false);
            _lazyMatrix = new Lazy<Matrix<string, string, TValue>>(LoadParent);
        }

        #endregion

        public override bool TryGetValue(string rowKey, string colKey, out TValue value)
        {
            return Parent.TryGetValue(rowKey, colKey, out value);
        }

        public override bool TryGetValue(int rowIndex, int colIndex, out TValue value)
        {
            return Parent.TryGetValue(rowIndex, colIndex, out value);
        }

        public override void SetValueOrMissing(string rowKey, string colKey, TValue value)
        {
            Parent.SetValueOrMissing(rowKey, colKey, value);
        }

        public override void SetValueOrMissing(int rowIndex, int colIndex, TValue value)
        {
            Parent.SetValueOrMissing(rowIndex, colIndex, value);
        }

        public override ReadOnlyCollection<string> RowKeys
        {
            get
            {

                return Parent.RowKeys;
            }
        }

        public override ReadOnlyCollection<string> ColKeys
        {
            get
            {

                return Parent.ColKeys;
            }
        }

        public override IDictionary<string, int> IndexOfRowKey
        {
            get
            {

                return Parent.IndexOfRowKey;
            }
        }

        public override IDictionary<string, int> IndexOfColKey
        {
            get
            {

                return Parent.IndexOfColKey;
            }
        }

        public override TValue MissingValue
        {
            get
            {
                return Parent.MissingValue;
            }
        }

    }

    public enum MergeType { Row, Col, Both };

}
