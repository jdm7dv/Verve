// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MBF.Matrix;
using MBF.TestAutomation.Util;
using MBF.Util;
using MBF.Util.Logging;
using NUnit.Framework;

namespace MBF.TestAutomation.Matrix
{
    /// <summary>
    /// Bvt test cases to confirm the features of Dense Matrix
    /// </summary>
    [TestFixture]
    public class SparseMatrixBvtTestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SparseMatrixBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\MatrixTestsConfig.xml");
        }

        #endregion

        #region Test Cases

        /// <summary>
        /// Creates a Sparse matrix instance with the help of 
        /// CreateEmptyInstance method
        /// Input : Valid values for SparseMatrix
        /// Validation : Proper instance of SparseMatrix
        /// </summary>
        [Test]
        public void ValidateSparseMatrixCreateEmptyInstance()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.IsNotNull(sparseMatrixObj);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of CreateEmptyInstance() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of CreateEmptyInstance() method successful");
        }

        /// <summary>
        /// Creates a Dense Ansi
        /// Input : Valid values for SparseMatrix
        /// Validation : Dense Ansi Object
        /// </summary>
        [Test]
        public void ValidateSparseMatrixAsDenseAnsi()
        {
            SparseMatrix<string, string, double> sparseMatrixObj = GetSparseMatrix();

            ParallelOptions parallelOptObj = new ParallelOptions();

            DenseAnsi denseAnsiObj =
                sparseMatrixObj.AsDenseAnsi<double>(parallelOptObj);

            // Validate all properties of Ansi and SparseMatrix
            Assert.AreEqual(sparseMatrixObj.ColCount, denseAnsiObj.ColCount);
            Assert.AreEqual(sparseMatrixObj.ColKeys, denseAnsiObj.ColKeys);
            Assert.AreEqual(sparseMatrixObj.IndexOfColKey, denseAnsiObj.IndexOfColKey);
            Assert.AreEqual(sparseMatrixObj.IndexOfRowKey, denseAnsiObj.IndexOfRowKey);
            Assert.AreEqual("?", denseAnsiObj.MissingValue.ToString());
            Assert.AreEqual(sparseMatrixObj.RowCount, denseAnsiObj.RowCount);
            Assert.AreEqual(sparseMatrixObj.RowKeys, denseAnsiObj.RowKeys);
            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleMatrixNodeName,
                Constants.DenseAnsiStringNode),
                denseAnsiObj.ToString2D().Replace("\r", "").Replace("\n", "").Replace("\t", ""));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of AsDenseAnsi() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of AsDenseAnsi() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrixfrom SparseMatrix
        /// Input : Valid values for SparseMatrix
        /// Validation : Dense Matrix Object
        /// </summary>
        [Test]
        public void ValidateSparseMatrixAsDenseMatrix()
        {
            SparseMatrix<string, string, double> originalDenseMatObj = GetSparseMatrix();

            DenseMatrix<string, string, double> denseMatrixObj =
                originalDenseMatObj.AsDenseMatrix<string, string, double>();

            // Validate all properties of Ansi and SparseMatrix
            Assert.AreEqual(originalDenseMatObj.ColCount, denseMatrixObj.ColCount);
            Assert.AreEqual(originalDenseMatObj.ColKeys, denseMatrixObj.ColKeys);
            Assert.AreEqual(originalDenseMatObj.IndexOfColKey, denseMatrixObj.IndexOfColKey);
            Assert.AreEqual(originalDenseMatObj.IndexOfRowKey, denseMatrixObj.IndexOfRowKey);
            Assert.AreEqual(originalDenseMatObj.MissingValue, denseMatrixObj.MissingValue);
            Assert.AreEqual(originalDenseMatObj.RowCount, denseMatrixObj.RowCount);
            Assert.AreEqual(originalDenseMatObj.RowKeys, denseMatrixObj.RowKeys);
            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleMatrixNodeName,
                Constants.DenseMatrixStringNode),
                denseMatrixObj.ToString2D().Replace("\r", "").Replace("\n", "").Replace("\t", ""));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of AsDenseMatrix() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of AsDenseMatrix() method successful");
        }

        /// <summary>
        /// Creates a Dense padded double object
        /// Input : Valid values for SparseMatrix
        /// Validation : Padded Double object
        /// </summary>
        [Test]
        public void ValidateSparseMatrixPaddedDouble()
        {
            SparseMatrix<string, string, double> sparseMatrixObj = GetSparseMatrix();

            ParallelOptions parallelOptObj = new ParallelOptions();

            PaddedDouble padDoub = sparseMatrixObj.AsPaddedDouble(parallelOptObj);

            // Validate all properties of Ansi and SparseMatrix
            Assert.AreEqual(sparseMatrixObj.ColCount, padDoub.ColCount);
            Assert.AreEqual(sparseMatrixObj.ColKeys, padDoub.ColKeys);
            Assert.AreEqual(sparseMatrixObj.IndexOfColKey, padDoub.IndexOfColKey);
            Assert.AreEqual(sparseMatrixObj.IndexOfRowKey, padDoub.IndexOfRowKey);
            Assert.AreEqual(sparseMatrixObj.MissingValue, padDoub.MissingValue);
            Assert.AreEqual(sparseMatrixObj.RowCount, padDoub.RowCount);
            Assert.AreEqual(sparseMatrixObj.RowKeys, padDoub.RowKeys);
            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleMatrixNodeName,
                Constants.DenseMatrixStringNode),
                padDoub.ToString2D().Replace("\r", "").Replace("\n", "").Replace("\t", ""));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of PaddedDouble() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of PaddedDouble() method successful");
        }

        /// <summary>
        /// Creates a Sparse matrix object
        /// Input : Valid values for SparseMatrix
        /// Validation : Sparse Matrix object
        /// </summary>
        [Test]
        public void ValidateSparseMatrixAsSparseMatrix()
        {
            SparseMatrix<string, string, double> sparseMatrixObj = GetSparseMatrix();

            SparseMatrix<string, string, double> newSparseMatrixObj =
                sparseMatrixObj.AsSparseMatrix<string, string, double>();

            // Validate all properties of Ansi and SparseMatrix
            Assert.AreEqual(sparseMatrixObj.ColCount, newSparseMatrixObj.ColCount);
            Assert.AreEqual(sparseMatrixObj.ColKeys, newSparseMatrixObj.ColKeys);
            Assert.AreEqual(sparseMatrixObj.IndexOfColKey, newSparseMatrixObj.IndexOfColKey);
            Assert.AreEqual(sparseMatrixObj.IndexOfRowKey, newSparseMatrixObj.IndexOfRowKey);
            Assert.AreEqual(sparseMatrixObj.MissingValue, newSparseMatrixObj.MissingValue);
            Assert.AreEqual(sparseMatrixObj.RowCount, newSparseMatrixObj.RowCount);
            Assert.AreEqual(sparseMatrixObj.RowKeys, newSparseMatrixObj.RowKeys);
            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleMatrixNodeName,
                Constants.DenseMatrixStringNode),
                newSparseMatrixObj.ToString2D().Replace("\r", "").Replace("\n", "").Replace("\t", ""));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of AsSparseMatrix() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of AsSparseMatrix() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrixwith Col View Index
        /// Input : Valid values for SparseMatrix
        /// Validation : Dictionary of Col View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixColViewIndex()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            IDictionary<string, double> colView =
                sparseMatrixObj.ColView(0);

            for (int i = 0; i < sparseMatrixObj.RowKeys.Count; i++)
            {
                Assert.AreEqual(colView["R" + i.ToString()],
                    sparseMatrixObj[i, 0]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ColView(index) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ColView(index) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrixwith Col View Key
        /// Input : Valid values for SparseMatrix
        /// Validation : Dictionary of Col View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixColViewKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            IDictionary<string, double> colView =
                sparseMatrixObj.ColView("C0");

            for (int i = 0; i < sparseMatrixObj.RowKeys.Count; i++)
            {
                Assert.AreEqual(colView["R" + i.ToString()],
                    sparseMatrixObj[i, 0]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ColView(key) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ColView(key) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrixwith Row View Index
        /// Input : Valid values for SparseMatrix
        /// Validation : Dictionary of Row View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixRowViewIndex()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            IDictionary<string, double> rowView =
                sparseMatrixObj.RowView(0);

            for (int i = 0; i < sparseMatrixObj.ColKeys.Count; i++)
            {
                Assert.AreEqual(rowView["C" + i.ToString()],
                    sparseMatrixObj[0, i]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of RowView(index) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of RowView(index) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrixwith Row View Index
        /// Input : Valid values for SparseMatrix
        /// Validation : Dictionary of Row View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixRowViewKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            IDictionary<string, double> rowView =
                sparseMatrixObj.RowView("R0");

            for (int i = 0; i < sparseMatrixObj.ColKeys.Count; i++)
            {
                Assert.AreEqual(rowView["C" + i.ToString()],
                    sparseMatrixObj[0, i]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of RowView(key) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of RowView(key) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates ContainsColKey
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Contains Column Key
        /// </summary>
        [Test]
        public void ValidateSparseMatrixContainsColKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();
            Assert.IsTrue(sparseMatrixObj.ContainsColKey("C0"));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ContainsColKey() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ContainsColKey() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates ContainsRowAndColKeys
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Contains Row And Col Keys
        /// </summary>
        [Test]
        public void ValidateSparseMatrixContainsRowAndColKeys()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();
            Assert.IsTrue(sparseMatrixObj.ContainsRowAndColKeys("R0", "C0"));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ContainsRowAndColKeys() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ContainsRowAndColKeys() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates ContainsRowKey
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Contains Row Key
        /// </summary>
        [Test]
        public void ValidateSparseMatrixContainsRowKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();
            Assert.IsTrue(sparseMatrixObj.ContainsRowKey("R0"));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ContainsRowKey() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ContainsRowKey() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates ConvertValueView
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Convert Value View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixConvertValueView()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, int> matObj =
                sparseMatrixObj.ConvertValueView<string, string, double, int>(
                ValueConverter.DoubleToInt,
                int.MaxValue);

            // Validate all properties of Ansi and SparseMatrix
            Assert.AreEqual(sparseMatrixObj.ColCount, matObj.ColCount);
            Assert.AreEqual(sparseMatrixObj.ColKeys, matObj.ColKeys);
            Assert.AreEqual(sparseMatrixObj.IndexOfColKey, matObj.IndexOfColKey);
            Assert.AreEqual(sparseMatrixObj.IndexOfRowKey, matObj.IndexOfRowKey);
            Assert.AreEqual("2147483647", matObj.MissingValue.ToString());
            Assert.AreEqual(sparseMatrixObj.RowCount, matObj.RowCount);
            Assert.AreEqual(sparseMatrixObj.RowKeys, matObj.RowKeys);
            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleMatrixNodeName,
                Constants.DenseConvertValueStringNode),
                matObj.ToString2D().Replace("\r", "").Replace("\n", "").Replace("\t", ""));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ConvertValueView() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ConvertValueView() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates GetEnumerator
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Gets Enumerator
        /// </summary>
        [Test]
        public void ValidateSparseMatrixGetEnumerator()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            var enumObj = sparseMatrixObj.RowKeyColKeyValues.GetEnumerator();
            enumObj.MoveNext();

            Assert.AreEqual(sparseMatrixObj[0, 0].ToString(),
                enumObj.Current.Value.ToString());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of GetEnumerator() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of GetEnumerator() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates GetValueOrMissing
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Get Value Or Missing
        /// </summary>
        [Test]
        public void ValidateSparseMatrixGetValueOrMissing()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.AreEqual(sparseMatrixObj[0, 1].ToString(),
                sparseMatrixObj.GetValueOrMissing(0, 1).ToString());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of GetValueOrMissing() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of GetValueOrMissing() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates GetValueOrMissing 
        /// with key
        /// Input : Valid values for SparseMatrix
        /// Validation : Get Value Or Missing (key)
        /// </summary>
        [Test]
        public void ValidateSparseMatrixGetValueOrMissingKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.AreEqual(sparseMatrixObj["R0", "C0"].ToString(),
                sparseMatrixObj.GetValueOrMissing("R0", "C0").ToString());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of GetValueOrMissing(key) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of GetValueOrMissing(key) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates HashableView 
        /// with key
        /// Input : Valid values for SparseMatrix
        /// Validation : Hashable View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixHashableView()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, double> hasview =
                sparseMatrixObj.HashableView<string, string, double>();

            // Validate the parent matrix.
            HashableView<string, string, double> parentHashableViewOrNull =
                hasview as HashableView<string, string, double>;
            Matrix<string, string, double> parentMatrixObj =
                parentHashableViewOrNull.ParentMatrix;

            // Validate all properties of Ansi and SparseMatrix
            Assert.AreEqual(sparseMatrixObj.ColCount, parentMatrixObj.ColCount);
            Assert.AreEqual(sparseMatrixObj.ColKeys, parentMatrixObj.ColKeys);
            Assert.AreEqual(sparseMatrixObj.IndexOfColKey, parentMatrixObj.IndexOfColKey);
            Assert.AreEqual(sparseMatrixObj.IndexOfRowKey, parentMatrixObj.IndexOfRowKey);
            Assert.AreEqual(sparseMatrixObj.MissingValue, parentMatrixObj.MissingValue);
            Assert.AreEqual(sparseMatrixObj.RowCount, parentMatrixObj.RowCount);
            Assert.AreEqual(sparseMatrixObj.RowKeys, parentMatrixObj.RowKeys);
            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleMatrixNodeName,
                Constants.DenseMatrixStringNode),
                parentMatrixObj.ToString2D().Replace("\r", "").Replace("\n", "").Replace("\t", ""));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of HashableView() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of HashableView() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates IsMissing 
        /// with key
        /// Input : Valid values for SparseMatrix
        /// Validation : Is Missing
        /// </summary>
        [Test]
        public void ValidateSparseMatrixIsMissing()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.IsTrue(sparseMatrixObj.IsMissing(double.NaN));
            Assert.IsFalse(sparseMatrixObj.IsMissing(1.0));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of IsMissing() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of IsMissing() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates IsMissing
        /// with Index
        /// with key
        /// Input : Valid values for SparseMatrix
        /// Validation : Is Missing(Index)
        /// </summary>
        [Test]
        public void ValidateSparseMatrixIsMissingIndex()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            for (int i = 0; i < sparseMatrixObj.RowCount; i++)
            {
                for (int j = 0; j < sparseMatrixObj.ColCount; j++)
                {
                    try
                    {
                        if (double.NaN.ToString() != sparseMatrixObj[i, j].ToString())
                        {
                            Assert.IsFalse(sparseMatrixObj.IsMissing(i, j));
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        break;
                    }
                }
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of IsMissing(index) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of IsMissing(index) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates IsMissing
        /// with key
        /// Input : Valid values for SparseMatrix
        /// Validation : Is Missing(Key)
        /// </summary>
        [Test]
        public void ValidateSparseMatrixIsMissingKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            for (int i = 0; i < sparseMatrixObj.RowCount; i++)
            {
                for (int j = 0; j < sparseMatrixObj.ColCount; j++)
                {
                    try
                    {
                        if (double.NaN.ToString() != sparseMatrixObj["R" + i.ToString(), "C" + j.ToString()].ToString())
                        {
                            Assert.IsFalse(sparseMatrixObj.IsMissing("R" + i.ToString(), "C" + j.ToString()));
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        break;
                    }
                }
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of IsMissing(key) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of IsMissing(key) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates IsMissingAll
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Is Missing All
        /// </summary>
        [Test]
        public void ValidateSparseMatrixIsMissingAll()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.IsFalse(sparseMatrixObj.IsMissingAll());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAll() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAll() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates IsMissingAllInCol
        /// with index
        /// Input : Valid values for SparseMatrix
        /// Validation : Is Missing All In Col(index)
        /// </summary>
        [Test]
        public void ValidateSparseMatrixIsMissingAllInColIndex()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.IsFalse(sparseMatrixObj.IsMissingAllInCol(0));

            // Creates a temp sparse matrix with all values set to Missing value
            SparseMatrix<string, string, double> tempDenseMatObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" }, new string[] { "C0" }, double.NaN);
            double[,] twoDArray = new double[,] { { double.NaN }, { double.NaN }, { double.NaN } };

            UpdateSparseMatrixValues(twoDArray, ref tempDenseMatObj, 3, 1);

            Assert.IsTrue(tempDenseMatObj.IsMissingAllInCol(0));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAllInCol(index) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAllInCol(index) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates IsMissingAllInCol
        /// with key
        /// Input : Valid values for SparseMatrix
        /// Validation : Is Missing All In Col(key)
        /// </summary>
        [Test]
        public void ValidateSparseMatrixIsMissingAllInColKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.IsFalse(sparseMatrixObj.IsMissingAllInCol("C0"));

            // Creates a temp sparse matrix with all values set to Missing value
            SparseMatrix<string, string, double> tempDenseMatObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" }, new string[] { "C0" }, double.NaN);
            double[,] twoDArray = new double[,] { { double.NaN }, { double.NaN }, { double.NaN } };

            UpdateSparseMatrixValues(twoDArray, ref tempDenseMatObj, 3, 1);

            Assert.IsTrue(tempDenseMatObj.IsMissingAllInCol("C0"));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAllInCol(key) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAllInCol(key) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates IsMissingAllInRow
        /// with index
        /// Input : Valid values for SparseMatrix
        /// Validation : Is Missing All In Row(index)
        /// </summary>
        [Test]
        public void ValidateSparseMatrixIsMissingAllInRowIndex()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.IsFalse(sparseMatrixObj.IsMissingAllInRow(0));

            // Creates a temp sparse matrix with all values set to Missing value
            SparseMatrix<string, string, double> tempDenseMatObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" }, new string[] { "C0" }, double.NaN);
            double[,] twoDArray = new double[,] { { double.NaN }, { double.NaN }, { double.NaN } };

            UpdateSparseMatrixValues(twoDArray, ref tempDenseMatObj, 3, 1);

            Assert.IsTrue(tempDenseMatObj.IsMissingAllInRow(0));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAllInRow(index) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAllInRow(index) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates IsMissingAllInRow
        /// with key
        /// Input : Valid values for SparseMatrix
        /// Validation : Is Missing All In Row(key)
        /// </summary>
        [Test]
        public void ValidateSparseMatrixIsMissingAllInRowKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.IsFalse(sparseMatrixObj.IsMissingAllInRow("R0"));

            // Creates a temp sparse matrix with all values set to Missing value
            SparseMatrix<string, string, double> tempDenseMatObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" }, new string[] { "C0" }, double.NaN);
            double[,] twoDArray = new double[,] { { double.NaN }, { double.NaN }, { double.NaN } };

            UpdateSparseMatrixValues(twoDArray, ref tempDenseMatObj, 3, 1);

            Assert.IsTrue(tempDenseMatObj.IsMissingAllInRow("R0"));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAllInRow(key) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAllInRow(key) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates IsMissingAllInSomeCol
        /// Input : Valid values for SparseMatrix
        /// Validation : Is Missing All In Some Col
        /// </summary>
        [Test]
        public void ValidateSparseMatrixIsMissingAllInSomeCol()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.IsFalse(sparseMatrixObj.IsMissingAllInSomeCol());

            // Creates a temp sparse matrix with all values set to no Missing value
            SparseMatrix<string, string, double> tempDenseMatObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" }, new string[] { "C0" }, double.NaN);
            double[,] twoDArray = new double[,] { { double.NaN }, { double.NaN }, { double.NaN } };

            UpdateSparseMatrixValues(twoDArray, ref tempDenseMatObj, 3, 1);

            Assert.IsTrue(tempDenseMatObj.IsMissingAllInSomeCol());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAllInSomeCol() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAllInSomeCol() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates IsMissingAllInSomeRow
        /// Input : Valid values for SparseMatrix
        /// Validation : Is Missing All In Some Row
        /// </summary>
        [Test]
        public void ValidateSparseMatrixIsMissingAllInSomeRow()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.IsFalse(sparseMatrixObj.IsMissingAllInSomeRow());

            // Creates a temp sparse matrix with all values set to no Missing value
            SparseMatrix<string, string, double> tempDenseMatObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" }, new string[] { "C0" }, double.NaN);
            double[,] twoDArray = new double[,] { { double.NaN }, { double.NaN }, { double.NaN } };

            UpdateSparseMatrixValues(twoDArray, ref tempDenseMatObj, 3, 1);

            Assert.IsTrue(tempDenseMatObj.IsMissingAllInSomeRow());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAllInSomeRow() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingAllInSomeRow() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates IsMissingSome
        /// Input : Valid values for SparseMatrix
        /// Validation : Is Missing Some
        /// </summary>
        [Test]
        public void ValidateSparseMatrixIsMissingSome()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.IsTrue(sparseMatrixObj.IsMissingSome());

            // Creates a temp sparse matrix with all values set to no Missing value
            SparseMatrix<string, string, double> tempDenseMatObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" }, new string[] { "C0" }, double.NaN);
            double[,] twoDArray = new double[,] { { 1 }, { 1 }, { 1 } };

            UpdateSparseMatrixValues(twoDArray, ref tempDenseMatObj, 3, 1);

            Assert.IsFalse(tempDenseMatObj.IsMissingSome());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingSome() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of IsMissingSome() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates MatrixEquals
        /// Input : Valid values for SparseMatrix
        /// Validation : Matrix Equals
        /// </summary>
        [Test]
        public void ValidateSparseMatrixMatrixEquals()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.IsTrue(sparseMatrixObj.MatrixEquals((Matrix<string, string, double>)sparseMatrixObj));

            // Creates a temp sparse matrix with all values set to no Missing value
            SparseMatrix<string, string, double> tempDenseMatObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" }, new string[] { "C0" }, double.NaN);
            double[,] twoDArray = new double[,] { { 1 }, { 1 }, { 1 } };

            UpdateSparseMatrixValues(twoDArray, ref tempDenseMatObj, 3, 1);

            Assert.IsFalse(tempDenseMatObj.MatrixEquals(sparseMatrixObj));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of MatrixEquals() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of MatrixEquals() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates MergeColsView
        /// Input : Valid values for SparseMatrix
        /// Validation : Merge Cols View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixMergeColsView()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" },
                new string[] { "C0", "C1", "C2" }, double.NaN);

            UpdateSparseMatrixValues(
                new double[,] { { 7, 7, 7 }, { 7, 7, 7 }, { 7, 7, 7 } },
                ref sparseMatrixObj, 3, 3);


            SparseMatrix<string, string, double> tempDenseMatObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "M0", "M1", "M2" },
                new string[] { "N0" }, double.NaN);

            UpdateSparseMatrixValues(
                new double[,] { { 7 }, { 7 }, { 7 } },
                ref tempDenseMatObj, 3, 1);

            Matrix<string, string, double> nonMergeMatrix =
                sparseMatrixObj.MergeColsView<string, string, double>(false, tempDenseMatObj);

            Assert.AreEqual(sparseMatrixObj.ColCount + 1, nonMergeMatrix.ColCount);
            Assert.AreEqual(sparseMatrixObj.RowCount - 3, nonMergeMatrix.RowCount);

            tempDenseMatObj =
                       SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" },
                new string[] { "N0" }, double.NaN);

            UpdateSparseMatrixValues(
                new double[,] { { 7 }, { 7 }, { 7 } },
                ref tempDenseMatObj, 3, 1);

            Matrix<string, string, double> mergeMatrix =
                sparseMatrixObj.MergeColsView<string, string, double>(true, tempDenseMatObj);

            Assert.AreEqual(sparseMatrixObj.RowCount, mergeMatrix.RowCount);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of MergeColsView() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of MergeColsView() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates MergeRowsView
        /// Input : Valid values for SparseMatrix
        /// Validation : Merge Rows View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixMergeRowsView()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "R0" },
                new string[] { "C0", "C1", "C2" }, double.NaN);

            UpdateSparseMatrixValues(
                new double[,] { { 7, 7, 7 } },
                ref sparseMatrixObj, 1, 3);


            SparseMatrix<string, string, double> tempDenseMatObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "M0" },
                new string[] { "N0", "N1", "N2" }, double.NaN);

            UpdateSparseMatrixValues(
                new double[,] { { 7, 7, 7 } },
                ref tempDenseMatObj, 1, 3);

            Matrix<string, string, double> nonMergeMatrix =
                sparseMatrixObj.MergeRowsView<string, string, double>(false, tempDenseMatObj);

            Assert.AreEqual(sparseMatrixObj.ColCount - 3, nonMergeMatrix.ColCount);
            Assert.AreEqual(sparseMatrixObj.RowCount + 1, nonMergeMatrix.RowCount);

            tempDenseMatObj =
                 SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "M0" },
                new string[] { "C0", "C1", "C2" }, double.NaN);

            UpdateSparseMatrixValues(
                new double[,] { { 7, 7, 7 } },
                ref tempDenseMatObj, 1, 3);

            Matrix<string, string, double> mergeMatrix =
                sparseMatrixObj.MergeRowsView<string, string, double>(true, tempDenseMatObj);

            Assert.AreEqual(sparseMatrixObj.ColCount, mergeMatrix.ColCount);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of MergeRowsView() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of MergeRowsView() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates PermuteColValuesForEachRowView
        /// with Random value
        /// Input : Valid values for SparseMatrix
        /// Validation : Permute Col Values For Each Row View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixPermuteColValuesForEachRowViewRand()
        {
            SparseMatrix<string, string, double> sparseMatObj = GetSparseMatrix();
            Random randomObj = new Random();
            Matrix<string, string, double> matObj =
                sparseMatObj.PermuteColValuesForEachRowView(ref randomObj);

            Assert.AreEqual(sparseMatObj.ColCount, matObj.ColCount);
            Assert.AreEqual(sparseMatObj.RowCount, matObj.RowCount);
            Assert.AreEqual(sparseMatObj.ColKeys, matObj.ColKeys);
            Assert.AreEqual(sparseMatObj.RowKeys, matObj.RowKeys);
            Assert.AreEqual(sparseMatObj.MissingValue, matObj.MissingValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of PermuteColValuesForEachRowView(random) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of PermuteColValuesForEachRowView(random) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates PermuteColValuesForEachRowView
        /// with Int value
        /// Input : Valid values for SparseMatrix
        /// Validation : Permute Col Values For Each Row View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixPermuteColValuesForEachRowViewInt()
        {
            SparseMatrix<string, string, double> sparseMatObj =
                CreateSimpleSparseMatrix();

            Matrix<string, string, double> matObj =
                sparseMatObj.PermuteColValuesForEachRowView(
                new int[] { 0, 1, 2, 3 });

            Assert.AreEqual(sparseMatObj.ColCount, matObj.ColCount);
            Assert.AreEqual(sparseMatObj.RowCount, matObj.RowCount);
            Assert.AreEqual(sparseMatObj.ColKeys, matObj.ColKeys);
            Assert.AreEqual(sparseMatObj.RowKeys, matObj.RowKeys);
            Assert.AreEqual(sparseMatObj.MissingValue, matObj.MissingValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of PermuteColValuesForEachRowView(Int) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of PermuteColValuesForEachRowView(Int) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates PermuteColValuesForEachRowView
        /// with Key value
        /// Input : Valid values for SparseMatrix
        /// Validation : Permute Col Values For Each Row View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixPermuteColValuesForEachRowViewKey()
        {
            SparseMatrix<string, string, double> sparseMatObj =
                CreateSimpleSparseMatrix();

            Matrix<string, string, double> matObj =
                sparseMatObj.PermuteColValuesForEachRowView(
                new string[] { "C0", "C1", "C2", "C3" });

            Assert.AreEqual(sparseMatObj.ColCount, matObj.ColCount);
            Assert.AreEqual(sparseMatObj.RowCount, matObj.RowCount);
            Assert.AreEqual(sparseMatObj.ColKeys, matObj.ColKeys);
            Assert.AreEqual(sparseMatObj.RowKeys, matObj.RowKeys);
            Assert.AreEqual(sparseMatObj.MissingValue, matObj.MissingValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of PermuteColValuesForEachRowView(key) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of PermuteColValuesForEachRowView(key) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates PermuteRowValuesForEachColView
        /// with Random value
        /// Input : Valid values for SparseMatrix
        /// Validation : Permute Row Values For Each Col View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixPermuteRowValuesForEachColViewRand()
        {
            SparseMatrix<string, string, double> sparseMatObj = GetSparseMatrix();
            Random randomObj = new Random();
            Matrix<string, string, double> matObj =
                sparseMatObj.PermuteRowValuesForEachColView(ref randomObj);

            Assert.AreEqual(sparseMatObj.ColCount, matObj.ColCount);
            Assert.AreEqual(sparseMatObj.RowCount, matObj.RowCount);
            Assert.AreEqual(sparseMatObj.ColKeys, matObj.ColKeys);
            Assert.AreEqual(sparseMatObj.RowKeys, matObj.RowKeys);
            Assert.AreEqual(sparseMatObj.MissingValue, matObj.MissingValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of PermuteRowValuesForEachColView(random) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of PermuteRowValuesForEachColView(random) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates PermuteRowValuesForEachColView
        /// with Int value
        /// Input : Valid values for SparseMatrix
        /// Validation : Permute Row Values For Each Col View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixPermuteRowValuesForEachColViewInt()
        {
            SparseMatrix<string, string, double> sparseMatObj =
                CreateSimpleSparseMatrix();

            Matrix<string, string, double> matObj =
                sparseMatObj.PermuteRowValuesForEachColView(
                new int[] { 0, 1, 2 });

            Assert.AreEqual(sparseMatObj.ColCount, matObj.ColCount);
            Assert.AreEqual(sparseMatObj.RowCount, matObj.RowCount);
            Assert.AreEqual(sparseMatObj.ColKeys, matObj.ColKeys);
            Assert.AreEqual(sparseMatObj.RowKeys, matObj.RowKeys);
            Assert.AreEqual(sparseMatObj.MissingValue, matObj.MissingValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of PermuteRowValuesForEachColView(Int) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of PermuteRowValuesForEachColView(Int) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates Remove
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Remove method
        /// </summary>
        [Test]
        public void ValidateSparseMatrixRemove()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            for (int i = 0; i < sparseMatrixObj.ColCount; i++)
            {
                Assert.IsTrue(sparseMatrixObj.Remove(0, i));
            }

            try
            {
                double val = sparseMatrixObj[0, 0];
                Assert.Fail(string.Format("Value {0} found instead of null", val));
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine(
                    "SparseMatrix BVT : Validation of Remove() method successful");
                ApplicationLog.WriteLine(
                    "SparseMatrix BVT : Validation of Remove() method successful");
            }
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates Remove
        /// method with Key
        /// Input : Valid values for SparseMatrix
        /// Validation : Remove(key)
        /// </summary>
        [Test]
        public void ValidateSparseMatrixRemoveKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            for (int i = 0; i < sparseMatrixObj.ColCount; i++)
            {
                Assert.IsTrue(sparseMatrixObj.Remove("R0", "C" + i.ToString()));
            }

            try
            {
                double val = sparseMatrixObj["R0", "C0"];
                Assert.Fail(string.Format("Value {0} found instead of null", val));
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine(
                    "SparseMatrix BVT : Validation of Remove(key) method successful");
                ApplicationLog.WriteLine(
                    "SparseMatrix BVT : Validation of Remove(key) method successful");
            }
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates RenameColsView
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Rename Cols View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixRenameColsView()
        {
            SparseMatrix<string, string, double> sparseMatrixObj = CreateSimpleSparseMatrix();

            Matrix<string, string, double> newMatObj =
                sparseMatrixObj.RenameColsView<string, string, double>(
                new Dictionary<string, string> { { "G0", "C0" }, 
                { "G1", "C1" }, { "G2", "C2" }, { "G3", "C3" } });

            Assert.AreEqual("1", newMatObj["R0", "G3"].ToString());
            Assert.AreEqual("3", newMatObj["R1", "G1"].ToString());
            Assert.AreEqual("3", newMatObj["R2", "G0"].ToString());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of RenameColsView() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of RenameColsView() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates RenameRowsView
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Rename Rows View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixRenameRowsView()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                CreateSimpleSparseMatrix();

            Matrix<string, string, double> newMatObj =
                sparseMatrixObj.RenameRowsView<string, string, double>(
                new Dictionary<string, string> { { "G0", "R0" }, 
                { "G1", "R1" }, { "G2", "R2" }});

            Assert.AreEqual("1", newMatObj["G0", "C3"].ToString());
            Assert.AreEqual("3", newMatObj["G1", "C1"].ToString());
            Assert.AreEqual("3", newMatObj["G2", "C0"].ToString());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of RenameRowsView() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of RenameRowsView() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SelectColsView
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Select Cols View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSelectColsView()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, double> viewObj = null;

            if (sparseMatrixObj.ColCount > 3)
            {
                viewObj =
                    sparseMatrixObj.SelectColsView<string, string, double>(
                    new int[] { 0, 1 });

                Assert.AreEqual("2", viewObj.ColCount.ToString());
                Assert.AreEqual(sparseMatrixObj.RowCount, viewObj.RowCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
                Assert.AreEqual(sparseMatrixObj[0, 1], viewObj[0, 1]);
            }
            else
            {
                viewObj =
                    sparseMatrixObj.SelectColsView<string, string, double>(
                    new int[] { 0 });

                Assert.AreEqual("1", viewObj.ColCount.ToString());
                Assert.AreEqual(sparseMatrixObj.RowCount, viewObj.RowCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SelectColsView() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SelectColsView() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SelectColsView
        /// method with key
        /// Input : Valid values for SparseMatrix
        /// Validation : Select Cols View with key
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSelectColsViewKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, double> viewObj = null;

            if (sparseMatrixObj.ColCount > 3)
            {
                viewObj =
                    sparseMatrixObj.SelectColsView<string, string, double>(
                    new string[] { "C0", "C1" });

                Assert.AreEqual("2", viewObj.ColCount.ToString());
                Assert.AreEqual(sparseMatrixObj.RowCount, viewObj.RowCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
                Assert.AreEqual(sparseMatrixObj[0, 1], viewObj[0, 1]);
            }
            else
            {
                viewObj =
                    sparseMatrixObj.SelectColsView<string, string, double>(
                    new string[] { "C0" });

                Assert.AreEqual("1", viewObj.ColCount.ToString());
                Assert.AreEqual(sparseMatrixObj.RowCount, viewObj.RowCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SelectColsView(key) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SelectColsView(key) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SelectColsView
        /// method with Index Sequence
        /// Input : Valid values for SparseMatrix
        /// Validation : Select Cols View with Index Sequence
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSelectColsViewColIndexSeq()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, double> viewObj = null;

            if (sparseMatrixObj.ColCount > 3)
            {
                int[] colIndexSeq = new int[] { 0, 1 };
                viewObj =
                    sparseMatrixObj.SelectColsView<string, string, double>(
                    (IEnumerable<int>)colIndexSeq);

                Assert.AreEqual("2", viewObj.ColCount.ToString());
                Assert.AreEqual(sparseMatrixObj.RowCount, viewObj.RowCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
                Assert.AreEqual(sparseMatrixObj[0, 1], viewObj[0, 1]);
            }
            else
            {
                int[] colIndexSeq = new int[] { 0 };
                viewObj =
                    sparseMatrixObj.SelectColsView<string, string, double>(
                    (IEnumerable<int>)colIndexSeq);

                Assert.AreEqual("1", viewObj.ColCount.ToString());
                Assert.AreEqual(sparseMatrixObj.RowCount, viewObj.RowCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SelectColsView(ColIndexSeq) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SelectColsView(ColIndexSeq) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SelectColsView
        /// method with key sequence
        /// Input : Valid values for SparseMatrix
        /// Validation : Select Cols View with key
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSelectColsViewKeySeq()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, double> viewObj = null;

            if (sparseMatrixObj.ColCount > 3)
            {
                string[] colKeySeq = new string[] { "C0", "C1" };
                viewObj =
                    sparseMatrixObj.SelectColsView<string, string, double>(
                    (IEnumerable<string>)colKeySeq);

                Assert.AreEqual("2", viewObj.ColCount.ToString());
                Assert.AreEqual(sparseMatrixObj.RowCount, viewObj.RowCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
                Assert.AreEqual(sparseMatrixObj[0, 1], viewObj[0, 1]);
            }
            else
            {
                string[] colKeySeq = new string[] { "C0" };
                viewObj =
                    sparseMatrixObj.SelectColsView<string, string, double>(
                    (IEnumerable<string>)colKeySeq);

                Assert.AreEqual("1", viewObj.ColCount.ToString());
                Assert.AreEqual(sparseMatrixObj.RowCount, viewObj.RowCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SelectColsView(keyseq) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SelectColsView(keyseq) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SelectRowsView
        /// method 
        /// Input : Valid values for SparseMatrix
        /// Validation : Select Rows View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSelectRowsView()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, double> viewObj = null;

            if (sparseMatrixObj.RowCount > 2)
            {
                viewObj =
                    sparseMatrixObj.SelectRowsView<string, string, double>(
                    new int[] { 0, 1 });

                Assert.AreEqual("2", viewObj.RowCount.ToString());
                Assert.AreEqual(sparseMatrixObj.ColCount, viewObj.ColCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
                Assert.AreEqual(sparseMatrixObj[1, 0], viewObj[1, 0]);
            }
            else
            {
                viewObj =
                    sparseMatrixObj.SelectRowsView<string, string, double>(
                    new int[] { 0 });

                Assert.AreEqual("1", viewObj.RowCount.ToString());
                Assert.AreEqual(sparseMatrixObj.ColCount, viewObj.ColCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SelectRowsView() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SelectRowsView() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SelectRowsView
        /// method with key
        /// Input : Valid values for SparseMatrix
        /// Validation : Select Rows View with key
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSelectRowsViewKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, double> viewObj = null;

            if (sparseMatrixObj.RowCount > 2)
            {
                viewObj =
                    sparseMatrixObj.SelectRowsView<string, string, double>(
                    new string[] { "R0", "R1" });

                Assert.AreEqual("2", viewObj.RowCount.ToString());
                Assert.AreEqual(sparseMatrixObj.ColCount, viewObj.ColCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
                Assert.AreEqual(sparseMatrixObj[1, 0], viewObj[1, 0]);
            }
            else
            {
                viewObj =
                    sparseMatrixObj.SelectRowsView<string, string, double>(
                    new string[] { "R0" });

                Assert.AreEqual("1", viewObj.RowCount.ToString());
                Assert.AreEqual(sparseMatrixObj.ColCount, viewObj.ColCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SelectRowsView(key) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SelectRowsView(key) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SelectRowsView
        /// method 
        /// Input : Valid values for SparseMatrix
        /// Validation : Select Rows View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSelectRowsViewIndexSeq()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, double> viewObj = null;

            if (sparseMatrixObj.RowCount > 2)
            {
                int[] rowIndexSeq = new int[] { 0, 1 };
                viewObj =
                    sparseMatrixObj.SelectRowsView<string, string, double>(
                    (IEnumerable<int>)rowIndexSeq);

                Assert.AreEqual("2", viewObj.RowCount.ToString());
                Assert.AreEqual(sparseMatrixObj.ColCount, viewObj.ColCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
                Assert.AreEqual(sparseMatrixObj[1, 0], viewObj[1, 0]);
            }
            else
            {
                int[] rowIndexSeq = new int[] { 0 };
                viewObj =
                    sparseMatrixObj.SelectRowsView<string, string, double>(
                    (IEnumerable<int>)rowIndexSeq);

                Assert.AreEqual("1", viewObj.RowCount.ToString());
                Assert.AreEqual(sparseMatrixObj.ColCount, viewObj.ColCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SelectRowsView(indexseq) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SelectRowsView(indexseq) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SelectRowsView
        /// method with key sequence
        /// Input : Valid values for SparseMatrix
        /// Validation : Select Rows View with key sequence
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSelectRowsViewKeyKeySeq()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, double> viewObj = null;

            if (sparseMatrixObj.RowCount > 2)
            {
                string[] rowKeySeq = new string[] { "R0", "R1" };
                viewObj =
                    sparseMatrixObj.SelectRowsView<string, string, double>(
                    (IEnumerable<string>)rowKeySeq);

                Assert.AreEqual("2", viewObj.RowCount.ToString());
                Assert.AreEqual(sparseMatrixObj.ColCount, viewObj.ColCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
                Assert.AreEqual(sparseMatrixObj[1, 0], viewObj[1, 0]);
            }
            else
            {
                string[] rowKeySeq = new string[] { "R0" };
                viewObj =
                    sparseMatrixObj.SelectRowsView<string, string, double>(
                    (IEnumerable<string>)rowKeySeq);

                Assert.AreEqual("1", viewObj.RowCount.ToString());
                Assert.AreEqual(sparseMatrixObj.ColCount, viewObj.ColCount);
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SelectRowsView(keyseq) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SelectRowsView(keyseq) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SelectRowsAndColsView
        /// method 
        /// Input : Valid values for SparseMatrix
        /// Validation : Select Rows And Cols View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSelectColsRowsViewIndexSeq()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, double> viewObj = null;

            if (sparseMatrixObj.RowCount > 2 &&
                sparseMatrixObj.ColCount > 2)
            {
                int[] rowIndexSeq = new int[] { 0, 1 };
                int[] colIndexSeq = new int[] { 0, 1 };
                viewObj =
                    sparseMatrixObj.SelectRowsAndColsView<string, string, double>(
                    (IEnumerable<int>)rowIndexSeq, (IEnumerable<int>)colIndexSeq);

                Assert.AreEqual("2", viewObj.RowCount.ToString());
                Assert.AreEqual("2", viewObj.ColCount.ToString());
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
                Assert.AreEqual(sparseMatrixObj[1, 0], viewObj[1, 0]);
            }
            else
            {
                int[] rowIndexSeq = new int[] { 0 };
                int[] colIndexSeq = new int[] { 0 };
                viewObj =
                    sparseMatrixObj.SelectRowsAndColsView<string, string, double>(
                    (IEnumerable<int>)rowIndexSeq, (IEnumerable<int>)colIndexSeq);

                Assert.AreEqual("1", viewObj.RowCount.ToString());
                Assert.AreEqual("1", viewObj.ColCount.ToString());
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SelectRowsAndColsView(indexseq) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SelectRowsAndColsView(indexseq) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SelectRowsAndColsView
        /// method with key sequence
        /// Input : Valid values for SparseMatrix
        /// Validation : Select Rows and Col View with key sequence
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSelectColRowsViewKeySeq()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, double> viewObj = null;

            if (sparseMatrixObj.RowCount > 2 &&
                sparseMatrixObj.ColCount > 2)
            {
                string[] rowIndexSeq = new string[] { "R0", "R1" };
                string[] colIndexSeq = new string[] { "C0", "C1" };
                viewObj =
                    sparseMatrixObj.SelectRowsAndColsView<string, string, double>(
                    (IEnumerable<string>)rowIndexSeq, (IEnumerable<string>)colIndexSeq);

                Assert.AreEqual("2", viewObj.RowCount.ToString());
                Assert.AreEqual("2", viewObj.ColCount.ToString());
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
                Assert.AreEqual(sparseMatrixObj[1, 0], viewObj[1, 0]);
            }
            else
            {
                string[] rowIndexSeq = new string[] { "R0" };
                string[] colIndexSeq = new string[] { "C0" };
                viewObj =
                    sparseMatrixObj.SelectRowsAndColsView<string, string, double>(
                    (IEnumerable<string>)rowIndexSeq,
                    (IEnumerable<string>)colIndexSeq);

                Assert.AreEqual("1", viewObj.RowCount.ToString());
                Assert.AreEqual("1", viewObj.ColCount.ToString());
                Assert.AreEqual(sparseMatrixObj[0, 0], viewObj[0, 0]);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SelectRowsAndColsView(keyseq) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SelectRowsAndColsView(keyseq) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SetValueOrMissing
        /// method with index
        /// Input : Valid values for SparseMatrix
        /// Validation : Set Value Or Missing
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSetValueOrMissingIndex()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            sparseMatrixObj.SetValueOrMissing(0, 0, 10);
            sparseMatrixObj.SetValueOrMissing(sparseMatrixObj.RowCount - 1,
                sparseMatrixObj.ColCount - 1, 10);

            Assert.AreEqual("10", sparseMatrixObj[0, 0].ToString());
            Assert.AreEqual("10", sparseMatrixObj[sparseMatrixObj.RowCount - 1,
                sparseMatrixObj.ColCount - 1].ToString());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SetValueOrMissing(index) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SetValueOrMissing(index) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SetValueOrMissing
        /// method with key
        /// Input : Valid values for SparseMatrix
        /// Validation : Set Value Or Missing
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSetValueOrMissingKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            sparseMatrixObj.SetValueOrMissing("R0", "C0", 10);
            sparseMatrixObj.SetValueOrMissing(
                "R" + (sparseMatrixObj.RowCount - 1).ToString(),
                "C" + (sparseMatrixObj.ColCount - 1).ToString(), 10);

            Assert.AreEqual("10", sparseMatrixObj[0, 0].ToString());
            Assert.AreEqual("10", sparseMatrixObj[
                sparseMatrixObj.RowCount - 1,
                sparseMatrixObj.ColCount - 1].ToString());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SetValueOrMissing(key) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SetValueOrMissing(key) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates Shuffle
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Shuffle
        /// </summary>
        [Test]
        public void ValidateSparseMatrixShuffle()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Random rdm = new Random(2);
            List<RowKeyColKeyValue<string, string, double>> rkValsObj =
                sparseMatrixObj.RowKeyColKeyValues.Shuffle(rdm);

            // Validate if the shuffle doesnt return R0 or C0 at the first Row/col key
            Assert.AreNotEqual("R0", rkValsObj[0].RowKey);
            Assert.AreNotEqual("C0", rkValsObj[0].ColKey);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of Shuffle() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of Shuffle() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates StringJoin
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : StringJoin
        /// </summary>
        [Test]
        public void ValidateSparseMatrixStringJoin()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            string strJoin =
                sparseMatrixObj.RowKeyColKeyValues.StringJoin().Replace("<", "").Replace(">", "");
            string expStrJoin =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.DenseStringJoinNode);

            // Validate if the shuffle doesnt return R0 or C0 at the first Row/col key
            Assert.AreEqual(expStrJoin, strJoin);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of StringJoin() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of StringJoin() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates StringJoin
        /// method with separator
        /// Input : Valid values for SparseMatrix
        /// Validation : String Join with separator
        /// </summary>
        [Test]
        public void ValidateSparseMatrixStringJoinSeparator()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            string strJoin =
                sparseMatrixObj.RowKeyColKeyValues.StringJoin(".").Replace("<", "").Replace(">", "");
            string expStrJoin =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.DenseStringJoinSeparatorNode);

            // Validate if the shuffle doesnt return R0 or C0 at the first Row/col key
            Assert.AreEqual(expStrJoin, strJoin);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of StringJoin(separator) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of StringJoin(separator) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates StringJoin
        /// method with separator, max, etc
        /// Input : Valid values for SparseMatrix
        /// Validation : String Join with separator, max, etc
        /// </summary>
        [Test]
        public void ValidateSparseMatrixStringJoinSeparatorEtc()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            string strJoin =
                sparseMatrixObj.RowKeyColKeyValues.StringJoin(".", 2, "N").Replace("<", "").Replace(">", "");
            string expStrJoin =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.DenseStringJoinSeparatorEtcNode);

            // Validate if the shuffle doesnt return R0 or C0 at the first Row/col key
            Assert.AreEqual(expStrJoin, strJoin);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of StringJoin(separator, max, etc) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of StringJoin(separator, max, etc) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates SubSequence
        /// Input : Valid values for SparseMatrix
        /// Validation : SubSequence
        /// </summary>
        [Test]
        public void ValidateSparseMatrixSubSequence()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            IEnumerable<RowKeyColKeyValue<string, string, double>> rowKeyColKeyObj =
                sparseMatrixObj.RowKeyColKeyValues.SubSequence(0, 2);

            List<RowKeyColKeyValue<string, string, double>> rowKeyColKeyList =
                new List<RowKeyColKeyValue<string, string, double>>();

            foreach (RowKeyColKeyValue<string, string, double> keyVal in rowKeyColKeyObj)
            {
                rowKeyColKeyList.Add(keyVal);
            }

            Assert.AreEqual(2, rowKeyColKeyList.Count);
            Assert.AreEqual(sparseMatrixObj[0, 0], rowKeyColKeyList[0].Value);
            Assert.AreEqual(sparseMatrixObj[0, 1], rowKeyColKeyList[1].Value);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of SubSequence() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of SubSequence() method successful");
        }

        /// <summary>
        /// Creates a Dense Ansi object
        /// Input : Valid values for SparseMatrix
        /// Validation : Dense Ansi Object
        /// </summary>
        [Test]
        public void ValidateSparseMatrixToDenseAnsi()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            ParallelOptions pOptObj = new ParallelOptions();
            DenseAnsi denseAnsiObj =
                sparseMatrixObj.ToDenseAnsi(pOptObj);

            // Validate all properties of Ansi and SparseMatrix
            Assert.AreEqual(sparseMatrixObj.ColCount, denseAnsiObj.ColCount);
            Assert.AreEqual(sparseMatrixObj.ColKeys, denseAnsiObj.ColKeys);
            Assert.AreEqual(sparseMatrixObj.IndexOfColKey, denseAnsiObj.IndexOfColKey);
            Assert.AreEqual(sparseMatrixObj.IndexOfRowKey, denseAnsiObj.IndexOfRowKey);
            Assert.AreEqual("?", denseAnsiObj.MissingValue.ToString());
            Assert.AreEqual(sparseMatrixObj.RowCount, denseAnsiObj.RowCount);
            Assert.AreEqual(sparseMatrixObj.RowKeys, denseAnsiObj.RowKeys);
            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleMatrixNodeName,
                Constants.DenseAnsiStringNode),
                denseAnsiObj.ToString2D().Replace("\r", "").Replace("\n", "").Replace("\t", ""));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ToDenseAnsi() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ToDenseAnsi() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix object
        /// Input : Valid values for SparseMatrix
        /// Validation : Dense Matrix Object
        /// </summary>
        [Test]
        public void ValidateSparseMatrixToDenseMatrix()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            DenseMatrix<string, string, double> newDenseObj =
                sparseMatrixObj.ToDenseMatrix();

            // Validate all properties of Ansi and SparseMatrix
            Assert.AreEqual(sparseMatrixObj.ColCount, newDenseObj.ColCount);
            Assert.AreEqual(sparseMatrixObj.ColKeys, newDenseObj.ColKeys);
            Assert.AreEqual(sparseMatrixObj.IndexOfColKey, newDenseObj.IndexOfColKey);
            Assert.AreEqual(sparseMatrixObj.IndexOfRowKey, newDenseObj.IndexOfRowKey);
            Assert.AreEqual(sparseMatrixObj.MissingValue, newDenseObj.MissingValue);
            Assert.AreEqual(sparseMatrixObj.RowCount, newDenseObj.RowCount);
            Assert.AreEqual(sparseMatrixObj.RowKeyColKeyValues, newDenseObj.RowKeyColKeyValues);
            Assert.AreEqual(sparseMatrixObj.RowKeys, newDenseObj.RowKeys);
            Assert.AreEqual(sparseMatrixObj.Values, newDenseObj.Values);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ToDenseMatrix() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ToDenseMatrix() method successful");
        }

        /// <summary>
        /// Creates a HashSet from Dense Matrix
        /// Input : Valid values for SparseMatrix
        /// Validation : Hash Set
        /// </summary>
        [Test]
        public void ValidateSparseMatrixToHashSet()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            HashSet<RowKeyColKeyValue<string, string, double>> hashSetObj =
                sparseMatrixObj.RowKeyColKeyValues.ToHashSet();

            foreach (RowKeyColKeyValue<string, string, double> keyVal
                in hashSetObj)
            {
                Assert.AreEqual(sparseMatrixObj[keyVal.RowKey, keyVal.ColKey],
                    keyVal.Value);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ToHashSet() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ToHashSet() method successful");
        }

        /// <summary>
        /// Creates a HashSet from Dense Matrix with Comparer
        /// Input : Valid values for SparseMatrix
        /// Validation : Hash Set with comparer
        /// </summary>
        [Test]
        public void ValidateSparseMatrixToHashSetComparer()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            EqualityComparer<RowKeyColKeyValue<string, string, double>> compObj =
                new TempEqualityComparer();

            HashSet<RowKeyColKeyValue<string, string, double>> hashSetObj =
                sparseMatrixObj.RowKeyColKeyValues.ToHashSet(compObj);

            foreach (RowKeyColKeyValue<string, string, double> keyVal
                in hashSetObj)
            {
                Assert.AreEqual(sparseMatrixObj[keyVal.RowKey, keyVal.ColKey],
                    keyVal.Value);
            }

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ToHashSet(comparer) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ToHashSet(comparer) method successful");
        }

        /// <summary>
        /// Creates a Dense padded double object
        /// Input : Valid values for SparseMatrix
        /// Validation : Padded Double object
        /// </summary>
        [Test]
        public void ValidateSparseMatrixToPaddedDouble()
        {
            SparseMatrix<string, string, double> sparseMatrixObj = GetSparseMatrix();

            ParallelOptions parallelOptObj = new ParallelOptions();

            PaddedDouble padDoub = sparseMatrixObj.ToPaddedDouble(parallelOptObj);

            // Validate all properties of Ansi and SparseMatrix
            Assert.AreEqual(sparseMatrixObj.ColCount, padDoub.ColCount);
            Assert.AreEqual(sparseMatrixObj.ColKeys, padDoub.ColKeys);
            Assert.AreEqual(sparseMatrixObj.IndexOfColKey, padDoub.IndexOfColKey);
            Assert.AreEqual(sparseMatrixObj.IndexOfRowKey, padDoub.IndexOfRowKey);
            Assert.AreEqual(sparseMatrixObj.MissingValue, padDoub.MissingValue);
            Assert.AreEqual(sparseMatrixObj.RowCount, padDoub.RowCount);
            Assert.AreEqual(sparseMatrixObj.RowKeys, padDoub.RowKeys);
            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleMatrixNodeName,
                Constants.DenseMatrixStringNode),
                padDoub.ToString2D().Replace("\r", "").Replace("\n", "").Replace("\t", ""));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ToPaddedDouble() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ToPaddedDouble() method successful");
        }

        /// <summary>
        /// Creates a Sparse matrix object
        /// Input : Valid values for SparseMatrix
        /// Validation : Sparse Matrix object
        /// </summary>
        [Test]
        public void ValidateSparseMatrixToSparseMatrix()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            SparseMatrix<string, string, double> newSparseMatrixObj =
                sparseMatrixObj.ToSparseMatrix<string, string, double>();

            // Validate all properties of Ansi and SparseMatrix
            Assert.AreEqual(sparseMatrixObj.ColCount, newSparseMatrixObj.ColCount);
            Assert.AreEqual(sparseMatrixObj.ColKeys, newSparseMatrixObj.ColKeys);
            Assert.AreEqual(sparseMatrixObj.IndexOfColKey, newSparseMatrixObj.IndexOfColKey);
            Assert.AreEqual(sparseMatrixObj.IndexOfRowKey, newSparseMatrixObj.IndexOfRowKey);
            Assert.AreEqual(sparseMatrixObj.MissingValue, newSparseMatrixObj.MissingValue);
            Assert.AreEqual(sparseMatrixObj.RowCount, newSparseMatrixObj.RowCount);
            Assert.AreEqual(sparseMatrixObj.RowKeys, newSparseMatrixObj.RowKeys);
            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleMatrixNodeName,
                Constants.DenseMatrixStringNode),
                newSparseMatrixObj.ToString2D().Replace("\r", "").Replace("\n", "").Replace("\t", ""));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ToSparseMatrix() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ToSparseMatrix() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates ToString2D
        /// Input : Valid values for SparseMatrix
        /// Validation : 2 D string
        /// </summary>
        [Test]
        public void ValidateSparseMatrixToString2D()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleMatrixNodeName,
                Constants.DenseMatrixStringNode),
                sparseMatrixObj.ToString2D().Replace("\r", "").Replace("\n", "").Replace("\t", ""));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ToString2D() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ToString2D() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates ToQueue
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : To Queue
        /// </summary>
        [Test]
        public void ValidateSparseMatrixToQueue()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Queue<RowKeyColKeyValue<string, string, double>> rkValsObj =
                sparseMatrixObj.RowKeyColKeyValues.ToQueue();

            List<RowKeyColKeyValue<string, string, double>> rowKeyColKeyList =
                new List<RowKeyColKeyValue<string, string, double>>();

            foreach (RowKeyColKeyValue<string, string, double> keyVal in rkValsObj)
            {
                rowKeyColKeyList.Add(keyVal);
            }

            // Validate if the queue does return R0 or C0 at the first Row/col key
            Assert.AreEqual("R0", rowKeyColKeyList[0].RowKey);
            Assert.AreEqual("C0", rowKeyColKeyList[0].ColKey);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of ToQueue() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of ToQueue() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates TransposeView 
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Transpose View
        /// </summary>
        [Test]
        public void ValidateSparseMatrixTransposeView()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            Matrix<string, string, double> transViewObj =
                sparseMatrixObj.TransposeView();

            // Validate all properties of Ansi and SparseMatrix
            Assert.AreEqual(sparseMatrixObj.ColCount, transViewObj.RowCount);
            Assert.AreEqual(sparseMatrixObj.ColKeys, transViewObj.RowKeys);
            Assert.AreEqual(sparseMatrixObj.IndexOfColKey, transViewObj.IndexOfRowKey);
            Assert.AreEqual(sparseMatrixObj.IndexOfRowKey, transViewObj.IndexOfColKey);
            Assert.AreEqual(sparseMatrixObj.MissingValue, transViewObj.MissingValue);
            Assert.AreEqual(sparseMatrixObj.RowCount, transViewObj.ColCount);
            Assert.AreEqual(sparseMatrixObj.RowKeys, transViewObj.ColKeys);
            Assert.AreEqual(Utility._xmlUtil.GetTextValue(
                Constants.SimpleMatrixNodeName,
                Constants.DenseTransposeStringNode),
                transViewObj.ToString2D().Replace("\r", "").Replace("\n", "").Replace("\t", ""));

            Console.WriteLine(
                "SparseMatrix BVT : Validation of TransposeView() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of TransposeView() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates TryGetValue 
        /// with index
        /// Input : Valid values for SparseMatrix
        /// Validation : Try Get Value with index
        /// </summary>
        [Test]
        public void ValidateSparseMatrixTryGetValueIndex()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            double tempVal = 0;
            Assert.IsTrue(sparseMatrixObj.TryGetValue(0, 0, out tempVal));
            Assert.AreEqual(tempVal, sparseMatrixObj[0, 0]);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of TryGetValue(index) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of TryGetValue(index) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates TryGetValue 
        /// with key
        /// Input : Valid values for SparseMatrix
        /// Validation : Try Get Value with key
        /// </summary>
        [Test]
        public void ValidateSparseMatrixTryGetValueKey()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            double tempVal = 0;
            Assert.IsTrue(sparseMatrixObj.TryGetValue("R0", "C0", out tempVal));
            Assert.AreEqual(tempVal, sparseMatrixObj["R0", "C0"]);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of TryGetValue(key) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of TryGetValue(key) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates Values 
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : Values
        /// </summary>
        [Test]
        public void ValidateSparseMatrixValues()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            IEnumerable<double> enumList = sparseMatrixObj.Values;
            StringBuilder strBuilder = new StringBuilder();
            foreach (double val in enumList)
            {
                strBuilder.Append(val.ToString());
            }
            string expectedValue =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.ValueStringNode);
            Assert.AreEqual(expectedValue, strBuilder.ToString());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of Values() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of Values() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates WriteDense 
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : writes dense matrix
        /// </summary>
        [Test]
        public void ValidateSparseMatrixWriteDense()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            sparseMatrixObj.WriteDense(Constants.FastQTempTxtFileName);

            string expectedFileValue =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.DenseMatrixStringNode);

            ValidateWriteMethod(expectedFileValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of WriteDense() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of WriteDense() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates WriteDense 
        /// method
        /// Input : Valid values for SparseMatrix
        /// Validation : writes dense matrix
        /// </summary>
        [Test]
        public void ValidateSparseMatrixWriteDenseWriter()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            using (TextWriter writer =
                new StreamWriter(Constants.FastQTempTxtFileName))
            {
                sparseMatrixObj.WriteDense(writer);
            }

            string expectedFileValue =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.DenseMatrixStringNode);

            ValidateWriteMethod(expectedFileValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of WriteDense(Writer) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of WriteDense(Writer) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates WriteDense 
        /// method with T
        /// Input : Valid values for SparseMatrix
        /// Validation : writes dense matrix with T
        /// </summary>
        [Test]
        public void ValidateSparseMatrixWriteDenseT()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();

            sparseMatrixObj.WriteDense<string, string, double>(
                Constants.FastQTempTxtFileName);

            string expectedFileValue =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.DenseMatrixStringNode);

            ValidateWriteMethod(expectedFileValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of WriteDense<T>() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of WriteDense<T>() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates WriteDense Ansi
        /// method with filename
        /// Input : Valid values for SparseMatrix
        /// Validation : writes dense matrix ansi with filename
        /// </summary>
        [Test]
        public void ValidateSparseMatrixWriteDenseAnsi()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();
            ParallelOptions optObj = new ParallelOptions();

            sparseMatrixObj.WriteDenseAnsi(
                Constants.FastQTempTxtFileName, optObj);

            string expectedFileValue =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.DenseAnsiStringNode);

            ValidateWriteMethod(expectedFileValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of WriteDenseAnsi() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of WriteDenseAnsi() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates WriteDense Ansi
        /// method with writer
        /// Input : Valid values for SparseMatrix
        /// Validation : writes dense matrix ansi with writer
        /// </summary>
        [Test]
        public void ValidateSparseMatrixWriteDenseAnsiWriter()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();
            ParallelOptions optObj = new ParallelOptions();

            string expectedFileValue =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.DenseAnsiStringNode);

            using (TextWriter writer = new StreamWriter(Constants.FastQTempTxtFileName))
            {
                sparseMatrixObj.WriteDenseAnsi(writer, optObj);
            }

            ValidateWriteMethod(expectedFileValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of WriteDenseAnsi(writer) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of WriteDenseAnsi(writer) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates padded double
        /// method with filename
        /// Input : Valid values for SparseMatrix
        /// Validation : writes padded double with filename
        /// </summary>
        [Test]
        public void ValidateSparseMatrixWritePaddedDouble()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();
            ParallelOptions optObj = new ParallelOptions();

            sparseMatrixObj.WritePaddedDouble(
                Constants.FastQTempTxtFileName, optObj);

            string expectedFileValue =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.DenseMatrixStringNode);

            ValidateWriteMethod(expectedFileValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of WritePaddedDouble() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of WritePaddedDouble() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates Write padded double
        /// method with writer
        /// Input : Valid values for SparseMatrix
        /// Validation : writes padded double with writer
        /// </summary>
        [Test]
        public void ValidateSparseMatrixWritePaddedDoubleWriter()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();
            ParallelOptions optObj = new ParallelOptions();

            string expectedFileValue =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.DenseMatrixStringNode);

            using (TextWriter writer =
                new StreamWriter(Constants.FastQTempTxtFileName))
            {
                sparseMatrixObj.WritePaddedDouble(writer, optObj);
            }

            ValidateWriteMethod(expectedFileValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of WritePaddedDouble(writer) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of WritePaddedDouble(writer) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates write Sparse
        /// method with filename
        /// Input : Valid values for SparseMatrix
        /// Validation : write Sparse with filename
        /// </summary>
        [Test]
        public void ValidateSparseMatrixWriteSparse()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();
            sparseMatrixObj.WriteSparse(
                Constants.FastQTempTxtFileName);

            string expectedFileValue =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.SparseMatrixStringNode);

            ValidateWriteMethod(expectedFileValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of WriteDenseAnsi() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of WriteDenseAnsi() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates Write sparse
        /// method with writer
        /// Input : Valid values for SparseMatrix
        /// Validation : writes sparse with writer
        /// </summary>
        [Test]
        public void ValidateSparseMatrixWriteSparseWriter()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();
            string expectedFileValue =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.SparseMatrixStringNode);

            using (TextWriter writer =
                new StreamWriter(Constants.FastQTempTxtFileName))
            {
                sparseMatrixObj.WriteSparse(writer);
            }

            ValidateWriteMethod(expectedFileValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of WriteDense(writer) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of WriteDense(writer) method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates write Sparse
        /// method with filename
        /// Input : Valid values for SparseMatrix
        /// Validation : write Sparse with filename
        /// </summary>
        [Test]
        public void ValidateSparseMatrixWriteSparseT()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();
            sparseMatrixObj.WriteSparse<string, string, double>(
                Constants.FastQTempTxtFileName);

            string expectedFileValue =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.SparseMatrixStringNode);

            ValidateWriteMethod(expectedFileValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of WriteSparse<T>() method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of WriteSparse<T>() method successful");
        }

        /// <summary>
        /// Creates a Sparse Matrix and validates Write sparse
        /// method with writer
        /// Input : Valid values for SparseMatrix
        /// Validation : writes sparse with writer
        /// </summary>
        [Test]
        public void ValidateSparseMatrixWriteSparseWriterT()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                GetSparseMatrix();
            string expectedFileValue =
                Utility._xmlUtil.GetTextValue(Constants.SimpleMatrixNodeName,
                Constants.SparseMatrixStringNode);

            using (TextWriter writer =
                new StreamWriter(Constants.FastQTempTxtFileName))
            {
                sparseMatrixObj.WriteSparse<string, string, double>(writer);
            }

            ValidateWriteMethod(expectedFileValue);

            Console.WriteLine(
                "SparseMatrix BVT : Validation of WriteSparse<T>(writer) method successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of WriteSparse<T>(writer) method successful");
        }

        /// <summary>
        /// Validates all properties
        /// Input : Valid values for SparseMatrix
        /// Validation : All properties
        /// </summary>
        [Test]
        public void ValidateSparseMatrixAllProperties()
        {
            SparseMatrix<string, string, double> sparseMatrixObj =
                CreateSimpleSparseMatrix();

            ReadOnlyCollection<string> colKeys = sparseMatrixObj.ColKeys;
            ReadOnlyCollection<string> rowKeys = sparseMatrixObj.RowKeys;
            IDictionary<string, int> indexCol = sparseMatrixObj.IndexOfColKey;
            IDictionary<string, int> indexRow = sparseMatrixObj.IndexOfRowKey;

            Assert.AreEqual("C0", sparseMatrixObj.ColKeys[0].ToString());
            Assert.AreEqual("4", sparseMatrixObj.ColCount.ToString());
            Assert.AreEqual("C0", colKeys[0].ToString());
            Assert.AreEqual("0", indexCol["C0"].ToString());
            Assert.AreEqual("0", indexRow["R0"].ToString());
            Assert.AreEqual("NaN", sparseMatrixObj.MissingValue.ToString());
            Assert.AreEqual("3", sparseMatrixObj.RowCount.ToString());
            Assert.AreEqual("R0", rowKeys[0].ToString());

            Console.WriteLine(
                "SparseMatrix BVT : Validation of all properties successful");
            ApplicationLog.WriteLine(
                "SparseMatrix BVT : Validation of all properties successful");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Gets the two D array from the xml
        /// </summary>
        /// <param name="nodeName">Node Name of the xml to be parsed</param>
        /// <param name="maxRows">Maximum rows</param>
        /// <param name="maxColumns">Maximum columns</param>
        /// <returns>2 D Array</returns>
        static double[,] GetTwoDArray(string nodeName, out int maxRows,
            out int maxColumns)
        {
            string[] rowArray = Utility._xmlUtil.GetTextValues(nodeName, Constants.RowsNode);

            // Gets the max number columns in the array
            maxColumns = 0;
            maxRows = rowArray.Length;
            for (int i = 0; i < maxRows; i++)
            {
                string[] colArray = rowArray[i].Split(',');
                if (maxColumns < colArray.Length)
                    maxColumns = colArray.Length;
            }

            // Creates a 2 D with max row and column length
            double[,] twoDArray = new double[maxRows, maxColumns];
            for (int i = 0; i < maxRows; i++)
            {
                string[] colArray = rowArray[i].Split(',');
                for (int j = 0; j < colArray.Length; j++)
                {
                    twoDArray[i, j] = double.Parse(colArray[j]);
                }
            }

            return twoDArray;
        }

        /// <summary>
        /// Gets the key sequence with the max length specified
        /// </summary>
        /// <param name="maxKey">Max length of the key sequence</param>
        /// <param name="isRow">If Row, append R else append C</param>
        /// <returns>Key Sequence Array</returns>
        static string[] GetKeySequence(int maxKey, bool isRow)
        {
            string[] keySeq = new string[maxKey];
            string tempSeq = string.Empty;

            if (isRow)
                tempSeq = "R";
            else
                tempSeq = "C";

            for (int i = 0; i < maxKey; i++)
            {
                keySeq[i] = tempSeq + i.ToString();
            }

            return keySeq;
        }

        /// <summary>
        /// Creates a SparseMatrix instance and returns the same.
        /// </summary>
        /// <returns>SparseMatrix Instance</returns>
        static SparseMatrix<string, string, double> GetSparseMatrix()
        {
            int maxRows = 0;
            int maxColumns = 0;
            double[,] twoDArray = GetTwoDArray(Constants.SimpleMatrixNodeName,
                out maxRows, out maxColumns);

            string[] rowKeySeq = GetKeySequence(maxRows, true);
            string[] colKeySeq = GetKeySequence(maxColumns, false);

            SparseMatrix<string, string, double> sparseMatrixObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                rowKeySeq, colKeySeq, double.NaN);

            UpdateSparseMatrixValues(twoDArray, ref sparseMatrixObj,
                maxRows, maxColumns);

            return sparseMatrixObj;
        }

        /// <summary>
        /// Creates a simple matrix for local validation
        /// </summary>
        /// <returns>Dense Matrix</returns>
        static SparseMatrix<string, string, double> CreateSimpleSparseMatrix()
        {
            double[,] twoDArray = new double[,] { { 1, 1, 1, 1 }, { 2, 3, 4, 5 },
            { 3, 4, double.NaN, 5 } };

            SparseMatrix<string, string, double> sparseMatrixObj =
                SparseMatrix<string, string, double>.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" },
                new string[] { "C0", "C1", "C2", "C3" }, double.NaN);

            UpdateSparseMatrixValues(twoDArray, ref sparseMatrixObj, 3, 4);

            return sparseMatrixObj;
        }

        /// <summary>
        /// Validates the test cases which uses Write method
        /// </summary>
        /// <param name="expectedFileValue">Expected file contents</param>
        static void ValidateWriteMethod(string expectedFileValue)
        {
            // Reads the file and validates the results
            string actualFileValue = string.Empty;
            using (TextReader rdrObj =
                new StreamReader(Constants.FastQTempTxtFileName))
            {
                actualFileValue =
                    rdrObj.ReadToEnd().Replace("\t",
                    "").Replace("\n", "").Replace("\r", "").Replace(" ", "");
            }

            Assert.AreEqual(expectedFileValue, actualFileValue);

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);
        }

        /// <summary>
        /// Updates Sparse Matrix with values for rows and columns
        /// </summary>
        /// <param name="twoDArray">Two D array</param>
        /// <param name="sparseMatrixObj">Sparse Matrix Object</param>
        /// <param name="maxRows">Row length</param>
        /// <param name="maxColumns">Column length</param>
        static void UpdateSparseMatrixValues(double[,] twoDArray,
           ref SparseMatrix<string, string, double> sparseMatrixObj,
            int maxRows, int maxColumns)
        {
            for (int i = 0; i < maxRows; i++)
            {
                for (int j = 0; j < maxColumns; j++)
                {
                    if (0 != string.Compare("NaN", twoDArray[i, j].ToString()))
                        sparseMatrixObj[i, j] = twoDArray[i, j];
                }
            }
        }

        #endregion;
    }
}
