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
using System.IO;
using System.Linq;
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
    public class DensePairAnsiBvtTestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static DensePairAnsiBvtTestCases()
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
        /// Input : Valid values for DensePairAnsi
        /// Validation : Col Keys In File
        /// </summary>
        [Test]
        public void ValidateDensePairAnsiColKeysInFile()
        {
            DenseMatrix<string, string, double> denseMatObj =
                GetDenseMatrix();
            ParallelOptions parOptObj = new ParallelOptions();

            denseMatObj.WriteDenseAnsi(Constants.FastQTempTxtFileName,
                parOptObj);
            string[] colkey =
                DensePairAnsi.ColKeysInFile(Constants.FastQTempTxtFileName);
            for (int i = 0; i < colkey.Length; i++)
            {
                Assert.AreEqual(denseMatObj.ColKeys[i], colkey[i]);
            }

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DensePairAnsi BVT : Validation of ColKeysInFile() method successful");
            ApplicationLog.WriteLine(
                "DensePairAnsi BVT : Validation of ColKeysInFile() method successful");
        }

        /// <summary>
        /// Validates CreateEmptyInstance method
        /// Input : Valid values for DensePairAnsi
        /// Validation : create empty instance method
        /// </summary>
        [Test]
        public void ValidateDensePairAnsiCreateEmptyInstance()
        {
            UOPair<char> uoPairObj = new UOPair<char>('?', '?');
            DensePairAnsi dpaObj =
                DensePairAnsi.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" },
                new string[] { "C0", "C1", "C2", "C3" },
                uoPairObj);

            Assert.IsNotNull(dpaObj);

            Console.WriteLine(
                "DensePairAnsi BVT : Validation of CreateEmptyInstance() method successful");
            ApplicationLog.WriteLine(
                "DensePairAnsi BVT : Validation of CreateEmptyInstance() method successful");
        }

        /// <summary>
        /// Validates GetInstance method
        /// Input : Valid values for DensePairAnsi
        /// Validation : gets instance
        /// </summary>
        [Test]
        public void ValidateDensePairAnsiGetInstance()
        {
            ParallelOptions parOptObj = new ParallelOptions();

            UOPair<char> uoPairObj = new UOPair<char>('?', '?');
            DensePairAnsi dpaObj =
                DensePairAnsi.CreateEmptyInstance(
                new string[] { "R0", "R1", "R2" },
                new string[] { "C0", "C1", "C2", "C3" },
                uoPairObj);

            dpaObj.WriteDensePairAnsi(Constants.FastQTempTxtFileName,
                parOptObj);

            Assert.AreEqual(4, dpaObj.ColCount);
            Assert.AreEqual(3, dpaObj.RowCount);
            Assert.AreEqual(new string[] { "R0", "R1", "R2" }, dpaObj.RowKeys);
            Assert.AreEqual(new string[] { "C0", "C1", "C2", "C3" }, dpaObj.ColKeys);

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DensePairAnsi BVT : Validation of GetInstance() method successful");
            ApplicationLog.WriteLine(
                "DensePairAnsi BVT : Validation of GetInstance() method successful");
        }

        /// <summary>
        /// Validates GetInstance method with IEnum parameters
        /// Input : Valid values for DensePairAnsi
        /// Validation : gets instance with IEnum parameters
        /// </summary>
        [Test]
        public void ValidateDensePairAnsiGetInstanceIEnum()
        {
            UOPair<char> uoPairObj = new UOPair<char>('?', '?');

            KeyValuePair<string, UOPair<char>> keyValPair =
                new KeyValuePair<string, UOPair<char>>("R0", uoPairObj);

            List<KeyValuePair<string, UOPair<char>>> keyValListObj =
                new List<KeyValuePair<string, UOPair<char>>>();

            var enumObj = keyValListObj.GroupBy(x => x.Key);

            DensePairAnsi dpaObj = DensePairAnsi.GetInstance(enumObj, '?');

            Assert.IsNotNull(dpaObj);

            Console.WriteLine(
                "DensePairAnsi BVT : Validation of GetInstance(enum, char) method successful");
            ApplicationLog.WriteLine(
                "DensePairAnsi BVT : Validation of GetInstance(enum, char) method successful");
        }

        /// <summary>
        /// Validates GetInstanceFromSparse method
        /// Input : Valid values for DensePairAnsi
        /// Validation : gets instance from sparse
        /// </summary>
        [Test]
        public void ValidateDensePairAnsiGetInstanceFromSparse()
        {
            DenseMatrix<string, string, double> denseMatObj =
               CreateSimpleDenseMatrix();

            denseMatObj.WriteSparse(Constants.FastQTempTxtFileName);

            DensePairAnsi dpaObj =
                DensePairAnsi.GetInstanceFromSparse(Constants.FastQTempTxtFileName);

            Assert.AreEqual(denseMatObj.ColCount, dpaObj.ColCount);
            Assert.AreEqual(denseMatObj.RowCount, dpaObj.RowCount);
            Assert.AreEqual(denseMatObj.RowKeys, dpaObj.RowKeys);
            Assert.AreEqual(denseMatObj.ColKeys, dpaObj.ColKeys);
            Assert.IsNotNull(DensePairAnsi.StaticMissingValue);
            Assert.IsNotNull(DensePairAnsi.StaticStoreMissingValue);

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DensePairAnsi BVT : Validation of GetInstanceFromSparse() method successful");
            ApplicationLog.WriteLine(
                "DensePairAnsi BVT : Validation of GetInstanceFromSparse() method successful");
        }

        /// <summary>
        /// Validates GetInstanceFromSparse method IEnum
        /// Input : Valid values for DensePairAnsi
        /// Validation : gets instance from sparse with IEnum
        /// </summary>
        [Test]
        public void ValidateDensePairAnsiGetInstanceFromSparseEnum()
        {
            UOPair<char> uoPairObj = new UOPair<char>('?', '?');

            RowKeyColKeyValue<string, string, UOPair<char>> rowColKey =
                new RowKeyColKeyValue<string, string, UOPair<char>>("R0", "C0", uoPairObj);

            List<RowKeyColKeyValue<string, string, UOPair<char>>> enumObj =
                new List<RowKeyColKeyValue<string, string, UOPair<char>>>();

            DensePairAnsi dpaObj = DensePairAnsi.GetInstanceFromSparse(enumObj);

            Assert.IsNotNull(dpaObj);

            Console.WriteLine(
                "DensePairAnsi BVT : Validation of GetInstanceFromSparse(Ienum) method successful");
            ApplicationLog.WriteLine(
                "DensePairAnsi BVT : Validation of GetInstanceFromSparse(Ienum) method successful");
        }

        /// <summary>
        /// Validates RowKeysInFile method
        /// Input : Valid values for DensePairAnsi
        /// Validation : Row Keys In File
        /// </summary>
        [Test]
        public void ValidateDensePairAnsiRowKeysInFile()
        {
            DenseMatrix<string, string, double> denseMatObj =
                GetDenseMatrix();
            ParallelOptions parOptObj = new ParallelOptions();

            denseMatObj.WriteDenseAnsi(Constants.FastQTempTxtFileName,
                parOptObj);
            IEnumerable<string> rowKeys =
                DensePairAnsi.RowKeysInFile(Constants.FastQTempTxtFileName);

            int i = 0;
            foreach (string rowKey in rowKeys)
            {
                Assert.AreEqual(denseMatObj.RowKeys[i], rowKey);
                i++;
            }

            if (File.Exists(Constants.FastQTempTxtFileName))
                File.Delete(Constants.FastQTempTxtFileName);

            Console.WriteLine(
                "DensePairAnsi BVT : Validation of RowKeysInFile() method successful");
            ApplicationLog.WriteLine(
                "DensePairAnsi BVT : Validation of RowKeysInFile() method successful");
        }

        /// <summary>
        /// Validates GetInstanceFromSparseInternal method
        /// Input : Valid values for DensePairAnsi
        /// Validation : gets instance from sparse Internal
        /// </summary>
        /// This test case will be enabled once the clarification is done.
        [Test]
        public void ValidateDensePairAnsiGetInstanceFromSparseInternal()
        {
            UOPair<char> uoPairObjMissing = new UOPair<char>('?', '?');
            UOPair<char> uoPairObjGood = new UOPair<char>('A', 'T');

            RowKeyColKeyValue<string, string, UOPair<char>> rowColKey =
                new RowKeyColKeyValue<string, string, UOPair<char>>("R0", "C0", uoPairObjGood);

            List<RowKeyColKeyValue<string, string, UOPair<char>>> enumObj =
                new List<RowKeyColKeyValue<string, string, UOPair<char>>>();
            enumObj.Add(rowColKey);

            DensePairAnsi dpaObj = DensePairAnsi.CreateEmptyInstance(
                new string[] { "R0" },
                new string[] { "C0" },
                uoPairObjMissing);

            dpaObj.GetInstanceFromSparseInternal(enumObj);

            Assert.AreEqual("R0", dpaObj.RowKeys[0]);
            Assert.AreEqual("C0", dpaObj.ColKeys[0]);
            Assert.AreEqual(0, dpaObj.IndexOfRowKey["R0"]);

            Console.WriteLine(
                "DensePairAnsi BVT : Validation of GetInstanceFromSparseInternal() method successful");
            ApplicationLog.WriteLine(
                "DensePairAnsi BVT : Validation of GetInstanceFromSparseInternal() method successful");
        }

        /// <summary>
        /// Validates WriteDensePairAnsi method
        /// Input : Valid values for DensePairAnsi
        /// Validation : WriteDensePairAnsi method
        /// </summary>
        [Test]
        public void ValidateDensePairAnsiWriteDensePairAnsi()
        {
            UOPair<char> uoPairObj = new UOPair<char>('?', '?');

            KeyValuePair<string, UOPair<char>> keyValPair =
                new KeyValuePair<string, UOPair<char>>("R0", uoPairObj);

            List<KeyValuePair<string, UOPair<char>>> keyValListObj =
                new List<KeyValuePair<string, UOPair<char>>>();

            var enumObj = keyValListObj.GroupBy(x => x.Key);

            DensePairAnsi dpaObj = DensePairAnsi.GetInstance(enumObj, '?');

            ParallelOptions paObj = new ParallelOptions();
            dpaObj.WriteDensePairAnsi(Constants.FastQTempTxtFileName, paObj);

            DensePairAnsi newDpaObj =
                DensePairAnsi.GetInstance(Constants.FastQTempTxtFileName, paObj);

            Assert.IsNotNull(newDpaObj);

            Console.WriteLine(
                "DensePairAnsi BVT : Validation of WriteDensePairAnsi() method successful");
            ApplicationLog.WriteLine(
                "DensePairAnsi BVT : Validation of WriteDensePairAnsi() method successful");
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
            double[,] twoDArray = new double[,] { { 11, 11, 11, 11 }, { 12, 13, 14, 15 }, { 13, 14, double.NaN, 15 } };

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

        #endregion;
    }
}
