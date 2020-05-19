// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using MBF.Util;
using MBF.Util.Logging;
using MBF.Matrix;
using MBF.TestAutomation.Util;

using NUnit.Framework;

namespace MBF.TestAutomation.Matrix
{
    /// <summary>
    /// Bvt test cases to confirm the features of Dense Matrix
    /// </summary>
    [TestFixture]
    public class DenseAnsiBvtTestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static DenseAnsiBvtTestCases()
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
        /// Validates ColKeysInFile method
        /// Input : Valid values for DenseAnsi
        /// Validation : Col Keys In File
        /// </summary>
        [Test]
        public void ValidateDenseAnsiColKeysInFile()
        {
            DenseMatrix<string, string, double> denseMatObj =
                GetDenseMatrix();
            ParallelOptions parOptObj = new ParallelOptions();

            denseMatObj.WriteDenseAnsi(Constants.FastQTempTxtFileName, parOptObj);

            string[] colkey =
                DenseAnsi.ColKeysInFile(Constants.FastQTempTxtFileName);
            for (int i = 0; i < colkey.Length; i++)
            {
                Assert.AreEqual(denseMatObj.ColKeys[i], colkey[i]);
            }

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DenseAnsi BVT : Validation of ColKeysInFile() method successful");
            ApplicationLog.WriteLine(
                "DenseAnsi BVT : Validation of ColKeysInFile() method successful");
        }

        /// <summary>
        /// Validates CreateEmptyInstance method
        /// Input : Valid values for DenseAnsi
        /// Validation : create empty instance method
        /// </summary>
        [Test]
        public void ValidateDenseAnsiCreateEmptyInstance()
        {
            DenseAnsi dpaObj =
                DenseAnsi.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" },
                new string[] { "C0", "C1", "C2", "C3" },
                '?');

            Assert.IsNotNull(dpaObj);

            Console.WriteLine(
                "DenseAnsi BVT : Validation of CreateEmptyInstance() method successful");
            ApplicationLog.WriteLine(
                "DenseAnsi BVT : Validation of CreateEmptyInstance() method successful");
        }

        /// <summary>
        /// Validates GetInstance method
        /// Input : Valid values for DenseAnsi
        /// Validation : gets instance
        /// </summary>
        [Test]
        public void ValidateDenseAnsiGetInstance()
        {
            DenseMatrix<string, string, double> denseMatObj =
               GetDenseMatrix();
            ParallelOptions parOptObj = new ParallelOptions();

            denseMatObj.WriteDenseAnsi(Constants.FastQTempTxtFileName, parOptObj);

            DenseAnsi dpaObj =
                DenseAnsi.GetInstance(Constants.FastQTempTxtFileName, parOptObj);

            Assert.AreEqual(denseMatObj.ColCount, dpaObj.ColCount);
            Assert.AreEqual(denseMatObj.RowCount, dpaObj.RowCount);
            Assert.AreEqual(denseMatObj.RowKeys, dpaObj.RowKeys);
            Assert.AreEqual(denseMatObj.ColKeys, dpaObj.ColKeys);

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DenseAnsi BVT : Validation of GetInstance() method successful");
            ApplicationLog.WriteLine(
                "DenseAnsi BVT : Validation of GetInstance() method successful");
        }

        /// <summary>
        /// Validates GetInstanceFromSparse method
        /// Input : Valid values for DenseAnsi
        /// Validation : gets instance from sparse
        /// </summary>
        [Test]
        public void ValidateDenseAnsiGetInstanceFromSparse()
        {
            DenseMatrix<string, string, double> denseMatObj =
               CreateSimpleDenseMatrix();

            denseMatObj.WriteSparse(Constants.FastQTempTxtFileName);

            DenseAnsi dpaObj =
                DenseAnsi.GetInstanceFromSparse(Constants.FastQTempTxtFileName);

            Assert.AreEqual(denseMatObj.ColCount, dpaObj.ColCount);
            Assert.AreEqual(denseMatObj.RowCount, dpaObj.RowCount);
            Assert.AreEqual(denseMatObj.RowKeys, dpaObj.RowKeys);
            Assert.AreEqual(denseMatObj.ColKeys, dpaObj.ColKeys);

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DenseAnsi BVT : Validation of GetInstanceFromSparse() method successful");
            ApplicationLog.WriteLine(
                "DenseAnsi BVT : Validation of GetInstanceFromSparse() method successful");
        }

        /// <summary>
        /// Validates RowKeysInFile method
        /// Input : Valid values for DenseAnsi
        /// Validation : Row Keys In File
        /// </summary>
        [Test]
        public void ValidateDenseAnsiRowKeysInFile()
        {
            DenseMatrix<string, string, double> denseMatObj =
                GetDenseMatrix();
            ParallelOptions parOptObj = new ParallelOptions();

            denseMatObj.WriteDenseAnsi(Constants.FastQTempTxtFileName,
                parOptObj);
            IEnumerable<string> rowKeys =
                DenseAnsi.RowKeysInFile(Constants.FastQTempTxtFileName);

            int i = 0;
            foreach (string rowKey in rowKeys)
            {
                Assert.AreEqual(denseMatObj.RowKeys[i], rowKey);
                i++;
            }

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DenseAnsi BVT : Validation of RowKeysInFile() method successful");
            ApplicationLog.WriteLine(
                "DenseAnsi BVT : Validation of RowKeysInFile() method successful");
        }

        /// <summary>
        /// Validates TryGetInstance method
        /// Input : Valid values for DenseAnsi
        /// Validation : Try Get Instance
        /// </summary>
        [Test]
        public void ValidateDenseAnsiTryGetInstance()
        {
            DenseMatrix<string, string, double> denseMatObj =
                CreateSimpleDenseMatrix();
            ParallelOptions parOptObj = new ParallelOptions();
            Matrix<string, string, char> matObj = null;
            denseMatObj.WriteDenseAnsi(Constants.FastQTempTxtFileName,
                parOptObj);
            Assert.IsTrue(DenseAnsi.TryGetInstance(Constants.FastQTempTxtFileName,
                '?', parOptObj, out matObj));

            Assert.AreEqual(denseMatObj.ColCount, matObj.ColCount);
            Assert.AreEqual(denseMatObj.RowCount, matObj.RowCount);
            Assert.AreEqual(denseMatObj.RowKeys, matObj.RowKeys);
            Assert.AreEqual(denseMatObj.ColKeys, matObj.ColKeys);
            Assert.IsNotNull(DenseAnsi.StaticMissingValue);
            Assert.IsNotNull(DenseAnsi.StaticStoreMissingValue);

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DenseAnsi BVT : Validation of TryGetInstance() method successful");
            ApplicationLog.WriteLine(
                "DenseAnsi BVT : Validation of TryGetInstance() method successful");
        }

        /// <summary>
        /// Validates TryGetInstanceFromSparse method with Matrix
        /// Input : Valid values for DenseAnsi
        /// Validation : Try Get Instance from sparse with Matrix
        /// </summary>
        [Test]
        public void ValidateDenseAnsiTryGetInstanceFromSparseMatrix()
        {
            DenseMatrix<string, string, double> denseMatObj =
                CreateSimpleDenseMatrix();
            Matrix<string, string, char> matObj = null;
            denseMatObj.WriteSparse(Constants.FastQTempTxtFileName);
            Assert.IsTrue(DenseAnsi.TryGetInstanceFromSparse(
                Constants.FastQTempTxtFileName, out matObj));

            Assert.AreEqual(denseMatObj.ColCount, matObj.ColCount);
            Assert.AreEqual(denseMatObj.RowCount, matObj.RowCount);
            Assert.AreEqual(denseMatObj.RowKeys, matObj.RowKeys);
            Assert.AreEqual(denseMatObj.ColKeys, matObj.ColKeys);

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DenseAnsi BVT : Validation of TryGetInstanceFromSparse(Matrix) method successful");
            ApplicationLog.WriteLine(
                "DenseAnsi BVT : Validation of TryGetInstanceFromSparse(Matrix) method successful");
        }

        /// <summary>
        /// Validates TryGetInstanceFromSparse method with Ansi
        /// Input : Valid values for DenseAnsi
        /// Validation : Try Get Instance from sparse with Ansi
        /// </summary>
        [Test]
        public void ValidateDenseAnsiTryGetInstanceFromSparseAnsi()
        {
            DenseMatrix<string, string, double> denseMatObj =
                CreateSimpleDenseMatrix();

            DenseAnsi denseAnsiObj = null;
            denseMatObj.WriteSparse(Constants.FastQTempTxtFileName);
            Assert.IsTrue(DenseAnsi.TryGetInstanceFromSparse(
                Constants.FastQTempTxtFileName, out denseAnsiObj));

            Assert.AreEqual(denseMatObj.ColCount, denseAnsiObj.ColCount);
            Assert.AreEqual(denseMatObj.RowCount, denseAnsiObj.RowCount);
            Assert.AreEqual(denseMatObj.RowKeys, denseAnsiObj.RowKeys);
            Assert.AreEqual(denseMatObj.ColKeys, denseAnsiObj.ColKeys);

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DenseAnsi BVT : Validation of TryGetInstanceFromSparse(denseAnsi) method successful");
            ApplicationLog.WriteLine(
                "DenseAnsi BVT : Validation of TryGetInstanceFromSparse(denseAnsi) method successful");
        }

        /// <summary>
        /// Validates TryParseDenseAnsiFormatAsDoubleMatrix method
        /// Input : Valid values for DenseAnsi
        /// Validation : Try Parse DenseAnsi Format As Double Matrix
        /// </summary>
        [Test]
        public void ValidateDenseAnsiTryParseDenseAnsiFormatAsDoubleMatrix()
        {
            DenseMatrix<string, string, double> denseMatObj =
                CreateSimpleDenseMatrix();

            ParallelOptions parOptObj = new ParallelOptions();
            Matrix<string, string, double> matObj = null;
            denseMatObj.WriteDenseAnsi(Constants.FastQTempTxtFileName, parOptObj);
            Assert.IsTrue(DenseAnsi.TryParseDenseAnsiFormatAsDoubleMatrix(
                Constants.FastQTempTxtFileName, double.NaN, parOptObj, out matObj));

            Assert.AreEqual(denseMatObj.ColCount, matObj.ColCount);
            Assert.AreEqual(denseMatObj.RowCount, matObj.RowCount);
            Assert.AreEqual(denseMatObj.RowKeys, matObj.RowKeys);
            Assert.AreEqual(denseMatObj.ColKeys, matObj.ColKeys);

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DenseAnsi BVT : Validation of TryParseDenseAnsiFormatAsDoubleMatrix() method successful");
            ApplicationLog.WriteLine(
                "DenseAnsi BVT : Validation of TryParseDenseAnsiFormatAsDoubleMatrix() method successful");
        }

        /// <summary>
        /// Validates TryParseDenseAnsiFormatAsGenericMatrix method
        /// Input : Valid values for DenseAnsi
        /// Validation : Try Parse DenseAnsi Format As Generic Matrix
        /// </summary>
        [Test]
        public void ValidateDenseAnsiTryParseDenseAnsiFormatAsGenericMatrix()
        {
            DenseMatrix<string, string, double> denseMatObj =
                CreateSimpleDenseMatrix();

            ParallelOptions parOptObj = new ParallelOptions();
            Matrix<string, string, double> matObj = null;
            denseMatObj.WriteDenseAnsi(Constants.FastQTempTxtFileName, parOptObj);
            Assert.IsTrue(DenseAnsi.TryParseDenseAnsiFormatAsGenericMatrix(
                Constants.FastQTempTxtFileName, double.NaN, parOptObj, out matObj));

            Assert.AreEqual(denseMatObj.ColCount, matObj.ColCount);
            Assert.AreEqual(denseMatObj.RowCount, matObj.RowCount);
            Assert.AreEqual(denseMatObj.RowKeys, matObj.RowKeys);
            Assert.AreEqual(denseMatObj.ColKeys, matObj.ColKeys);

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DenseAnsi BVT : Validation of TryParseDenseAnsiFormatAsGenericMatrix() method successful");
            ApplicationLog.WriteLine(
                "DenseAnsi BVT : Validation of TryParseDenseAnsiFormatAsGenericMatrix() method successful");
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
        /// Creates a simple matrix for local validation
        /// </summary>
        /// <returns>Dense Matrix</returns>
        static DenseMatrix<string, string, double> CreateSimpleDenseMatrix()
        {
            double[,] twoDArray = new double[,] { { 1, 1, 1, 1 }, { 2, 3, 4, 5 }, { 3, 4, double.NaN, 5 } };

            DenseMatrix<string, string, double> denseMatObj =
                new DenseMatrix<string, string, double>(twoDArray,
                    new string[] { "R0", "R1", "R2" }, new string[] { "C0", "C1", "C2", "C3" }, double.NaN);

            return denseMatObj;
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
        /// Creates a DenseAnsi instance and returns the same.
        /// </summary>
        /// <returns>DenseAnsi Instance</returns>
        static DenseMatrix<string, string, double> GetDenseMatrix()
        {
            int maxRows = 0;
            int maxColumns = 0;
            double[,] twoDArray = GetTwoDArray(Constants.SimpleMatrixNodeName,
                out maxRows, out maxColumns);

            string[] rowKeySeq = GetKeySequence(maxRows, true);
            string[] colKeySeq = GetKeySequence(maxColumns, false);

            DenseMatrix<string, string, double> denseMatrixObj =
                new DenseMatrix<string, string, double>(twoDArray, rowKeySeq,
                    colKeySeq, double.NaN);

            return denseMatrixObj;
        }

        #endregion;
    }
}
