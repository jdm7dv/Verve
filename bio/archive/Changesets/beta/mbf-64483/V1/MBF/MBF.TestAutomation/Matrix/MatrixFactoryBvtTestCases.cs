// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Threading.Tasks;

using MBF.Matrix;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.Matrix
{
    /// <summary>
    /// Bvt test cases to confirm the features of Dense Matrix
    /// </summary>
    [TestFixture]
    public class MatrixFactoryBvtTestCases
    {

        #region Global Variables

        DenseMatrix<string, string, double> _denseMatObj;

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static MatrixFactoryBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil =
                new XmlUtility(@"TestUtils\MatrixTestsConfig.xml");
        }

        #endregion

        #region Test Cases

        /// <summary>
        /// Creates a Register Matrix Parser
        /// Input : Valid values for MatrixFactory
        /// Validation : Register Matrix Parser
        /// </summary>
        [Test]
        public void ValidateMatrixFactoryRegisterMatrixParser()
        {
            MatrixFactory<String, String, Double> mfactObj =
                MatrixFactory<String, String, Double>.GetInstance();

            TryParseMatrixDelegate<string, string, double> tryParseDelObj =
                new TryParseMatrixDelegate<string, string, double>(TryParseMatrix);

            mfactObj.RegisterMatrixParser(tryParseDelObj);
            Assert.Pass(
                "No exceptions were thrown on running RegisterMatrixParser() method");
        }

        /// <summary>
        /// Creates a Dense Matrix and validate parse
        /// method
        /// Input : Valid values for MatrixFactory
        /// Validation : parse method
        /// </summary>
        [Test]
        public void ValidateMatrixFactoryParse()
        {
            _denseMatObj = GetDenseMatrix();

            MatrixFactory<String, String, Double> mfObj =
                MatrixFactory<String, String, Double>.GetInstance();

            ParallelOptions poObj = new ParallelOptions();

            TryParseMatrixDelegate<string, string, double> a =
                new TryParseMatrixDelegate<string, string, double>(this.TryParseMatrix);
            mfObj.RegisterMatrixParser(a);
            // Writes the text file
            _denseMatObj.WritePaddedDouble(Constants.FastQTempTxtFileName, poObj);

            Matrix<string, string, double> newMatObj =
                mfObj.Parse(Constants.FastQTempTxtFileName, double.NaN, poObj);

            Assert.AreEqual(_denseMatObj.RowCount, newMatObj.RowCount);
            Assert.AreEqual(_denseMatObj.RowKeys, newMatObj.RowKeys);
            Assert.AreEqual(_denseMatObj.ColCount, newMatObj.ColCount);
            Assert.AreEqual(_denseMatObj.ColKeys, newMatObj.ColKeys);
            Assert.AreEqual(_denseMatObj.Values, newMatObj.Values);

            Console.WriteLine(
                "MatrixFactory BVT : Successfully validated Parse() method");
            ApplicationLog.WriteLine(
                "MatrixFactory BVT : Successfully validated Parse() method");
        }

        /// <summary>
        /// Creates a Dense Matrix and validate try parse
        /// method
        /// Input : Valid values for MatrixFactory
        /// Validation : try parse method
        /// </summary>
        [Test]
        public void ValidateMatrixFactoryTryParse()
        {
            _denseMatObj = GetDenseMatrix();

            MatrixFactory<String, String, Double> mfObj =
                MatrixFactory<String, String, Double>.GetInstance();

            ParallelOptions poObj = new ParallelOptions();

            TryParseMatrixDelegate<string, string, double> a =
                new TryParseMatrixDelegate<string, string, double>(this.TryParseMatrix);
            mfObj.RegisterMatrixParser(a);
            // Writes the text file
            _denseMatObj.WritePaddedDouble(Constants.FastQTempTxtFileName, poObj);

            Matrix<string, string, double> newMatObj = null;
            Assert.IsTrue(mfObj.TryParse(
                Constants.FastQTempTxtFileName, double.NaN, poObj, out newMatObj));

            Assert.AreEqual(_denseMatObj.RowCount, newMatObj.RowCount);
            Assert.AreEqual(_denseMatObj.RowKeys, newMatObj.RowKeys);
            Assert.AreEqual(_denseMatObj.ColCount, newMatObj.ColCount);
            Assert.AreEqual(_denseMatObj.ColKeys, newMatObj.ColKeys);
            Assert.AreEqual(_denseMatObj.Values, newMatObj.Values);

            Console.WriteLine(
                "MatrixFactory BVT : Successfully validated TryParse() method");
            ApplicationLog.WriteLine(
                "MatrixFactory BVT : Successfully validated TryParse() method");
        }

        /// <summary>
        /// Creates a Dense Matrix and validate try parse
        /// method
        /// Input : Valid values for MatrixFactory
        /// Validation : try parse method
        /// </summary>
        [Test]
        public void ValidateMatrixFactoryAllMethods()
        {
            _denseMatObj = GetDenseMatrix();

            MatrixFactory<String, String, Double> mfObj =
                MatrixFactory<String, String, Double>.GetInstance();

            Assert.IsNullOrEmpty(mfObj.ErrorMessages);

            Console.WriteLine(
                "MatrixFactory BVT : Successfully validated All methods");
            ApplicationLog.WriteLine(
                "MatrixFactory BVT : Successfully validated All methods");
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
        /// Creates a DenseMatrix instance and returns the same.
        /// </summary>
        /// <returns>DenseMatrix Instance</returns>
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

        /// <summary>
        /// Delegate implementation
        /// </summary>
        /// <param name="filename">File name</param>
        /// <param name="missingValue">Missing value</param>
        /// <param name="parallelOptions">parallel options</param>
        /// <param name="matrix">Matrix</param>
        /// <returns>bool</returns>
        public bool TryParseMatrix(string filename, double missingValue, ParallelOptions parallelOptions,
            out Matrix<string, string, double> matrix)
        {
            matrix = _denseMatObj;
            return false;
        }

        #endregion;
    }
}
