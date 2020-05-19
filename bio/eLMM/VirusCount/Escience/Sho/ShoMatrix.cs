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
using MBT.Escience;
using Bio.Util;
using MBT.Escience.Parse;
using Bio.Matrix;
using ShoNS.Array;
using System.Threading.Tasks;
using System.IO;
using MBT.Escience.Matrix;
using System.Diagnostics;

namespace MBT.Escience.Sho
{
    ///!!!matrix: similar code in specialFunctions.matrix
    [Serializable]
    public class ShoMatrix : Matrix<string, string, double>{
      

        public static ShoMatrix CreateEmptyShoMatrix()
        {
            return new ShoMatrix(new DoubleArray(0,0), new List<string> { }, new List<string> { }, double.NaN);            
        }

        //uncomment this if want to do non-lazy loading of  ShoMatrix from auto parsing
        //public static implicit operator ShoMatrix(FileMatrix input)
        //{
        //    return input.ToShoMatrix();
        //}

        public static List<string> IntersectOnColKeys(ref ShoMatrix m1, ref ShoMatrix m2)
        {
            IList<string> cidsPhen = m2.ColKeys;
            IList<string> cidsSnp = m1.ColKeys;
            List<string> cids = cidsPhen.Intersect(cidsSnp).ToList();
            m1 = m1.SelectColsView(cids).AsShoMatrix();
            m2 = m2.SelectColsView(cids).AsShoMatrix();
            return cids;
        }

        public List<string> GetRowKeysFromIndexes(List<int> keyIndList)
        {
            return keyIndList.Select(i => RowKeys[i]).ToList();
        }
        public List<string> GetColKeysFromIndexes(List<int> keyIndList)
        {
            return keyIndList.Select(i => ColKeys[i]).ToList();
        }

        public IntArray FindColsWithAllMissingData()
        {
            return this.DoubleArray.FindColsWithAllNans();
        }

        public ShoMatrix RemoveColsWithAnyMissingData()
        {
            List<string> garb;
            return RemoveColsWithAnyMissingData(out garb);
        }

        public ShoMatrix RemoveColsWithAnyMissingData(out List<string> rowIdsRetained)
        {
            IntArray badInd = this.DoubleArray.FindColsWithAtLeastOneNan();
            List<string> badRowIds = GetColKeysFromIndexes(badInd.ToList());
            rowIdsRetained = this.ColKeys.Except(badRowIds).ToList();
            return this.SelectColsViewComplement(badRowIds).AsShoMatrix();
        }

        public ShoMatrix RemoveRowsWithAnyMissingData()
        {
            IntArray badInd = this.DoubleArray.FindRowsWithAtLeastOneNan();
            List<string> badRowIds = GetRowKeysFromIndexes(badInd.ToList());
            return this.SelectRowsViewComplement(badRowIds).AsShoMatrix();
        }

        /// <summary>
        /// Add a cols of data to the matrix
        /// </summary>
        public ShoMatrix AddCols(DoubleArray extraCols, List<string> colKeys, bool addAsLastCol)
        {
            List<string> newColKeys = this.ColKeys.ToList();
            DoubleArray newD;
            if (addAsLastCol)
            {
                newD = ShoUtils.CatArrayCol(this.DoubleArray, extraCols);
                newColKeys.AddRange(colKeys);
            }
            else
            {
                newD = ShoUtils.CatArrayCol(extraCols, this.DoubleArray);
                newColKeys.InsertRange(0, colKeys);
            }

            ShoMatrix x = new ShoMatrix(newD, this.RowKeys, newColKeys, this.MissingValue);
            return x;
        }

        /// <summary>
        /// Add a rows of data to the matrix
        /// </summary>
        public ShoMatrix AddRows(DoubleArray extraRows, List<string> rowKeys, bool addAsLastRows)
        {
            List<string> newRowKeys = this.RowKeys.ToList();
            DoubleArray newD;
            if (addAsLastRows)
            {
                newD = ShoUtils.CatArrayRow(this.DoubleArray, extraRows);
                newRowKeys.AddRange(rowKeys);
            }
            else
            {
                newD = ShoUtils.CatArrayRow(extraRows, this.DoubleArray);
                newRowKeys.InsertRange(0, rowKeys);
            }

            ShoMatrix x = new ShoMatrix(newD, newRowKeys, this.ColKeys, this.MissingValue);
            return x;
        }

        /// <summary>
        /// Add a row of data to the matrix
        /// </summary>
        /// <param name="extraRow"></param>
        /// <param name="rowKey"></param>
        ///<param name="addAsLastRow"></param>
        /// <returns></returns>
        public ShoMatrix AddRow(DoubleArray extraRow, string rowKey, bool addAsLastRow)
        {
            List<string> newRowKeys = this.RowKeys.ToList();
            DoubleArray newD;
            if (addAsLastRow)
            {
                newD = ShoUtils.CatArrayRow(this.DoubleArray, extraRow);
                newRowKeys.Add(rowKey);
            }
            else
            {
                newD = ShoUtils.CatArrayRow(extraRow, this.DoubleArray);
                newRowKeys.Insert(0, rowKey);
            }

            ShoMatrix x = new ShoMatrix(newD, newRowKeys, this.ColKeys, this.MissingValue);
            return x;
        }

        /// <summary>
        /// Do element-by-element wise addition of the entries in s to the entries of this ShoMatrix
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public void AddInto(ShoMatrix s)
        {
            Helper.CheckCondition(this.RowKeys.ToHashSet().SetEquals(s.RowKeys), "row keys must be the same");
            Helper.CheckCondition(this.ColKeys.ToHashSet().SetEquals(s.ColKeys), "col keys must be the same");
            s = s.SelectRowsAndColsView(this.RowKeys, this.ColKeys).AsShoMatrix();
            this.DoubleArray += s.DoubleArray;
        }


        public override string ToString()
        {
            return "RowCount=" + this.RowCount + ", ColCount=" + this.ColCount;
        }


        /// <summary>
        /// Given a matrix that is expected to be symmetric: 1) check that any entries i,j and j,i are the same,
        /// and if one of them is missing, fill it in with the other.
        /// </summary>
        /// <returns></returns>
        public ShoMatrix FillInAndCheckSymmetricMatrix(double tol)
        {
            HashSet<string> tmpKeys = this.RowKeys.ToHashSet<string>();
            tmpKeys.UnionWith(this.ColKeys);
            List<string> unionCids = tmpKeys.ToList();
            int N = unionCids.Count;
            DoubleArray x = new DoubleArray(N, N);
            for (int i = 0; i < N; i++)
            {
                string key1 = unionCids[i];

                for (int j = 0; j < N; j++)
                {
                    string key2 = unionCids[j];

                    double val1 = double.NaN;
                    double val2 = double.NaN;
                    //bool r1 = this.ContainsRowAndColKeys(key1, key2) && this.TryGet(key1, key2, out val1);
                    //bool r2 = this.ContainsRowAndColKeys(key2, key1) && this.TryGet(key2, key1, out val2);
                    bool r1 = this.ContainsRowKey(key1) && this.ContainsColKey(key2) && this.TryGetValue(key1, key2, out val1);
                    bool r2 = this.ContainsRowKey(key1) && this.ContainsColKey(key2) && this.TryGetValue(key2, key1, out val2);

                    //System.Console.WriteLine("[" + key1  + ", " + key2 + "]  " + val1 + ", " + val2);

                    if (r1 && r2)
                    {
                        if (Math.Abs(val1 - val2) > tol) throw new Exception("ShoMatrix does not represent a symmetric matrix");
                        x[i, j] = x[j, i] = val1;
                    }
                    else if (r1 && !r2)
                    {
                        x[i, j] = x[j, i] = val1;
                    }
                    else if (!r1 && r2)
                    {
                        x[i, j] = x[j, i] = val2;
                    }
                    else
                    {
                        x[i, j] = x[j, i] = double.NaN;
                    }
                }
            }
            Helper.CheckCondition(x.IsSymmetric(), "matrix is not symmetric");
            ShoMatrix newMatrix = new ShoMatrix(x, unionCids, unionCids, this.MissingValue);
            return newMatrix;
        }

        /// <summary>
        /// to get them back in a specific order, and to throw an exception if any are missing
        /// </summary>
        /// <returns></returns>
        public List<double> ValuesListCannotBeMissing()
        {
            List<double> allVals = new List<double>();
            for (int rowIndex = 0; rowIndex < RowCount; ++rowIndex)
            {
                string rowKey = RowKeys[rowIndex];
                for (int colIndex = 0; colIndex < ColCount; ++colIndex)
                {
                    double value;
                    if (TryGetValue(rowIndex, colIndex, out value))
                    {
                        string colKey = ColKeys[colIndex];
                        allVals.Add(value);
                    }
                    else
                    {
                        throw new Exception("missing value found");
                    }
                }
            }
            return allVals;
        }

        public static ShoMatrix CreateShoMatrixFromDenseFileFormat(string dataFile)
        {
            return CreateShoMatrixFromDenseFileFormat(dataFile, new ParallelOptions() { MaxDegreeOfParallelism = 1 });
        }

        public static ShoMatrix CreateShoMatrixFromDenseFileFormat(string dataFile, ParallelOptions parallelOptions)
        {
            string errMsg;
            ShoMatrix x;
            bool succeeded = TryParseRFileWithDefaultMissingIntoShoMatrix(dataFile, parallelOptions, out x, out errMsg);
            Helper.CheckCondition(succeeded, "coul not use TryParseRFileWithDefault, use a more general matrix parser");
            return x;
        }

        /// <summary>
        /// assumes 0/1/2 encoding already present
        /// </summary>
        /// <param name="tpedFile"></param>
        /// <param name="tfamFile"></param>
        /// <param name="parallelOptions"></param>
        /// <param name="result"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool ParseTPed012FamPairIntoShoMatrix(string tpedFile, string tfamFile, ParallelOptions parallelOptions, out ShoMatrix result, out string errorMsg)
        {
            throw new NotImplementedException();
            //char separatorArray = '\ '
            //var matrix = new ShoMatrix();//DenseMatrix<TRowKey, TColKey, TValue>();
            //result = matrix;
            //matrix._missingValue = double.NaN;//missingValue;

            ////first, the ped file which contains the SNP data
            //int rowCount = Bio.Util.FileUtils.ReadEachLine(tpedFile).Count() - 1;
            //Debug.Assert(rowCount >= 0); // real assert
            //List<string> unparsedRowNames = new List<string>();
            //List<string> unparsedColNames = new List<string>();

            //using (TextReader textReader = Bio.Util.FileUtils.OpenTextStripComments(tpedFile))
            //{
            //    string firstLine = textReader.ReadLine();                
            //    if (null == firstLine)
            //    {
            //        errorMsg = "Expect file to have first line. ";
            //        return false;
            //    }         else{

            //    }                      

            //    matrix.DoubleArray = new DoubleArray(rowCount, unparsedColNames.Count);
            //    string line;
            //    int rowIndex = -1;

            //    while (!string.IsNullOrEmpty(line = textReader.ReadLine()))
            //    {
            //        ++rowIndex;
            //        string[] fields = line.Split(separatorArray);
            //        //Helper.CheckCondition(fields.Length >= 1, string.Format("Expect each line to have at least one field (file={0}, rowIndex={1})", rFileName, rowIndex));
            //        if (fields.Length == 0)
            //        {
            //            errorMsg = string.Format("Expect each line to have at least one field (file={0}, rowIndex={1})", tpedFile, rowIndex);
            //            return false;
            //        }

            //        string rowKey = fields[0];
            //        unparsedRowNames.Add(rowKey);

            //        // if the first data row has same length as header row, then header row much contain a name for the column of row names. Remove it and proceed.
            //        if (rowIndex == 0 && fields.Length == unparsedColNames.Count)
            //        {
            //            unparsedColNames.RemoveAt(0);
            //        }

            //        //Helper.CheckCondition(fields.Length == matrix.ColKeys.Count + 1, string.Format("Line has {0} fields instead of the epxected {1} fields (file={2}, rowKey={3}, rowIndex={4})", fields.Length, matrix.ColKeys.Count + 1, rFileName, rowKey, rowIndex));
            //        if (fields.Length != unparsedColNames.Count + 1)
            //        {
            //            errorMsg = string.Format("Line has {0} fields instead of the expected {1} fields (file={2}, rowKey={3}, rowIndex={4})", fields.Length, unparsedColNames.Count + 1, tpedFile, rowKey, rowIndex);
            //            return false;
            //        }

            //        //for (int colIndex = 0; colIndex < matrix.ValueArray.GetLength(0); ++colIndex)
            //        for (int colIndex = 0; colIndex < unparsedColNames.Count; ++colIndex)
            //        {
            //            double r;
            //            if (!double.TryParse(fields[colIndex + 1], out r))
            //            {
            //                errorMsg = string.Format("Unable to parse {0} because field {1} cannot be parsed into an instance of type {2}", tpedFile, fields[colIndex + 1], typeof(double));
            //                return false;
            //            }
            //            matrix.DoubleArray[rowIndex, colIndex] = r;

            //        }
            //    }

            //    IList<string> rowKeys;

            //    if (!Bio.Util.Parser.TryParseAll<string>(unparsedRowNames, out rowKeys))
            //    {
            //        errorMsg = string.Format("Unable to parse {0} because row names cannot be parsed into an instance of type {1}", tpedFile, typeof(string));
            //        return false;
            //    }
            //    IList<string> colKeys;
            //    if (!Bio.Util.Parser.TryParseAll<string>(unparsedColNames, out colKeys))
            //    {
            //        errorMsg = string.Format("Unable to parse {0} because col names cannot be parsed into an instance of type {1}", tpedFile, typeof(string));
            //        return false;
            //    }
            //    matrix._rowKeys = new ReadOnlyCollection<string>(rowKeys);
            //    matrix._colKeys = new ReadOnlyCollection<string>(colKeys);
            //}

            ////In the case of sparse files, many of the row keys will be the same and so we return false
            //if (matrix._rowKeys.Count != matrix._rowKeys.Distinct().Count())
            //{
            //    errorMsg = string.Format("Some rows have the same values as other (look for blank rows). " + tpedFile);
            //    return false;
            //}


            //matrix._indexOfRowKey = matrix.RowKeys.Select((key, index) => new { key, index }).ToDictionary(pair => pair.key, pair => pair.index);
            //matrix._indexOfColKey = matrix.ColKeys.Select((key, index) => new { key, index }).ToDictionary(pair => pair.key, pair => pair.index);

            //return true;

        }

        /// <summary>
        /// Create a ShoMatrix from a file in RFile format.
        /// </summary>
        /// <param name="rFileName">a file in RFile format with delimited columns</param>
        /// <param name="missingValue">The special value that represents 'missing'</param>
        /// <param name="separatorArray">An array of character delimiters</param>
        /// <param name="parallelOptions">A ParallelOptions instance that configures the multithreaded behavior of this operation.</param>
        /// <param name="result">The DenseMatrix created</param>
        /// <param name="errorMsg">If the file is not parsable, an error message about the problem.</param>
        /// <returns>True if the file is parsable; otherwise false</returns>
        /*!!! code mostly copied from TryParseRFileWithDefaultMissing in Bio.Matrix.DenseMatrix.cs*/
        public static bool TryParseRFileWithDefaultMissingIntoShoMatrix(string rFileName,
            ParallelOptions parallelOptions, out ShoMatrix result, out string errorMsg)
        {
            //throw new Exception("this seems to have a bug in it as I get a matrix of Size 94x95 DOubleARray, but the rowKeys and colKeys are each of length 94");

            char separatorArray = '\t';
            errorMsg = "";
            var matrix = new ShoMatrix();//DenseMatrix<TRowKey, TColKey, TValue>();
            result = matrix;
            matrix._missingValue = double.NaN;//missingValue;

            //int rowCount = Bio.Util.FileUtils.ReadEachLine(rFileName).Count() - 1;
            int rowCount = FileUtils.ReadEachLineAllowGzip(rFileName).Count() - 1;//Bio.Util.FileUtils.ReadEachLine(rFileName).Count() - 1;

            //using (TextReader textReader = Bio.Util.FileUtils.OpenTextStripComments(rFileName))
            using (TextReader textReader = SpecialFunctions.OpenTextOrZippedText(rFileName))
            {
                string firstLine = textReader.ReadLine();
                //Helper.CheckCondition(null != firstLine, "Expect file to have first line. ");
                if (null == firstLine)
                {
                    errorMsg = "Expect file to have first line. ";
                    return false;
                }
                Debug.Assert(rowCount >= 0); // real assert

                List<string> unparsedRowNames = new List<string>(rowCount);
                List<string> unparsedColNames = firstLine.Split(separatorArray).ToList();
                // matrix.ValueArray = new double[rowCount, unparsedColNames.Count];
                matrix.DoubleArray = new DoubleArray(rowCount, unparsedColNames.Count - 1);
                string line;
                int rowIndex = -1;

                while (!string.IsNullOrEmpty(line = textReader.ReadLine()))
                {
                    ++rowIndex;
                    string[] fields = line.Split(separatorArray);
                    //Helper.CheckCondition(fields.Length >= 1, string.Format("Expect each line to have at least one field (file={0}, rowIndex={1})", rFileName, rowIndex));
                    if (fields.Length == 0)
                    {
                        errorMsg = string.Format("Expect each line to have at least one field (file={0}, rowIndex={1})", rFileName, rowIndex);
                        return false;
                    }

                    string rowKey = fields[0];
                    unparsedRowNames.Add(rowKey);

                    // if the first data row has same length as header row, then header row much contain a name for the column of row names. Remove it and proceed.
                    if (rowIndex == 0 && fields.Length == unparsedColNames.Count)
                    {
                        unparsedColNames.RemoveAt(0);
                    }

                    //Helper.CheckCondition(fields.Length == matrix.ColKeys.Count + 1, string.Format("Line has {0} fields instead of the epxected {1} fields (file={2}, rowKey={3}, rowIndex={4})", fields.Length, matrix.ColKeys.Count + 1, rFileName, rowKey, rowIndex));
                    if (fields.Length != unparsedColNames.Count + 1)
                    {
                        errorMsg = string.Format("Line has {0} fields instead of the expected {1} fields (file={2}, rowKey={3}, rowIndex={4})", fields.Length, unparsedColNames.Count + 1, rFileName, rowKey, rowIndex);
                        return false;
                    }

                    //for (int colIndex = 0; colIndex < matrix.ValueArray.GetLength(0); ++colIndex)
                    for (int colIndex = 0; colIndex < unparsedColNames.Count; ++colIndex)
                    {
                        double r;
                        if (!double.TryParse(fields[colIndex + 1], out r))
                        {
                            errorMsg = string.Format("Unable to parse {0} because field {1} cannot be parsed into an instance of type {2}", rFileName, fields[colIndex + 1], typeof(double));
                            return false;
                        }
                        matrix.DoubleArray[rowIndex, colIndex] = r;

                        //TValue r;
                        //if (!Parser.TryParse<TValue>(fields[colIndex + 1], out r))
                        //{
                        //    errorMsg = string.Format("Unable to parse {0} because field {1} cannot be parsed into an instance of type {2}", rFileName, fields[colIndex + 1], typeof(double));
                        //    return false;
                        //}
                        //matrix.ValueArray[rowIndex, colIndex] = r;
                    }
                }

                IList<string> rowKeys;

                if (!Bio.Util.Parser.TryParseAll<string>(unparsedRowNames, out rowKeys))
                {
                    errorMsg = string.Format("Unable to parse {0} because row names cannot be parsed into an instance of type {1}", rFileName, typeof(string));
                    return false;
                }
                IList<string> colKeys;
                if (!Bio.Util.Parser.TryParseAll<string>(unparsedColNames, out colKeys))
                {
                    errorMsg = string.Format("Unable to parse {0} because col names cannot be parsed into an instance of type {1}", rFileName, typeof(string));
                    return false;
                }
                matrix._rowKeys = new ReadOnlyCollection<string>(rowKeys);
                matrix._colKeys = new ReadOnlyCollection<string>(colKeys);
            }

            //In the case of sparse files, many of the row keys will be the same and so we return false
            if (matrix._rowKeys.Count != matrix._rowKeys.Distinct().Count())
            {
                errorMsg = string.Format("Some rows have the same values as other (look for blank rows). " + rFileName);
                return false;
            }

            matrix._indexOfRowKey = matrix.RowKeys.Select((key, index) => new { key, index }).ToDictionary(pair => pair.key, pair => pair.index);
            matrix._indexOfColKey = matrix.ColKeys.Select((key, index) => new { key, index }).ToDictionary(pair => pair.key, pair => pair.index);

            Helper.CheckCondition(matrix.RowCount == matrix.DoubleArray.size0, "inconsistent row dimension");
            Helper.CheckCondition(matrix.ColCount == matrix.DoubleArray.size1, "inconsistent col dimension");

            return true;
            //return matrix;
        }

        public static ShoMatrix CreateShoMatrixFromMatrixFile(string dataFile, ParallelOptions parallelOptions)
        {
            Matrix<string, string, double> tmp = null;
            if (true)
            {
                Console.WriteLine("reading in file: " + dataFile);
                MatrixFactory<string, string, double> myFactory = MatrixFactory<string, string, double>.GetInstance();
                tmp = myFactory.Parse(dataFile, double.NaN, parallelOptions);
            }
            else //for debugging, so we can see what the error message is (go to MatrixFactory to see the list of parsers it's trying, and call the one you think should work)
            {
                string errMsg;
                bool result = DenseMatrix<string, string, double>.TryParseRFileWithDefaultMissing(dataFile, double.NaN, parallelOptions, out tmp, out errMsg);//new Tuple<string, string>("MBT.Escience.Matrix.DenseMatrix", "TryParseRFileWithDefaultMissing");
                Console.WriteLine("asadgfasdf");
            }
            return new ShoMatrix(tmp);
        }


        static public Matrix<string, string, double> ReadAndTrimPredictor(string fileName, ParallelOptions parallelOptions)
        {
            MatrixFactory<string, string, char> matrixFactorySSC = MatrixFactory<string, string, char>.GetInstance();
            var matrixIn = matrixFactorySSC.Parse(fileName, '?', parallelOptions);
            var matrix = matrixIn.ConvertValueView(Bio.Util.ValueConverter.CharToDouble, double.NaN);
            //var matrix = matrixIn.ConvertValueView(IntAsCharToDouble, DoubleToIntAsChar, double.NaN);
            return matrix;
        }

        static public double IntAsCharToDouble(char c)
        {
            return (double)int.Parse(c.ToString());
        }

        static public char DoubleToIntAsChar(double r)
        {
            return SpecialFunctions.FirstAndOnly(r.ToString());
        }



        public void WriteToFile(string myFile)
        {
            this.WriteDense<string, string, double>(myFile);
            //SpecialFunctions.WriteToFile(this.ToString2D(), myFile);
        }


        //!!!removed for now by Carl because PSDnaDistMain is not a project in utils.sln
        ///// <summary>
        /////standardize the way Eigenstrat does, using other code
        ///// </summary>
        ///// <returns></returns>
        //public ShoMatrix Standardize(int SNP_ENCODING)
        //{
        //    double maxMatrixVal = SNP_ENCODING;//assumes that data are integ
        //    ShoMatrix snpMatrixNew = PSDnaDistMain.Standardize(this, maxMatrixVal).ToShoMatrix();
        //    return snpMatrixNew;
        //}

        /// <summary>
        /// divides each element in the array by the mean absolute value
        /// </summary>
        /// <returns></returns>
        public ShoMatrix Normalize()
        {
            //throw new Exception("don't use this, it causes problems because I forget, and don't use it consistently. Plus I don't need it anymore if I normalize the kernel in the first place");
            return this.MultByConst(1 / this.DoubleArray.Norm());
        }

        public ShoMatrix NormalizeSpecial()
        {
            this.DoubleArray = this.DoubleArray.NormalizeSpecial();
            return this;
        }

        /// <summary>
        /// multiply each element in the array by a single number
        /// </summary>
        /// <param name="multFac"></param>
        /// <returns></returns>
        public ShoMatrix MultByConst(double multFac)
        {
            return new ShoMatrix(this.DoubleArray * multFac, this.RowKeys, this.ColKeys, this.MissingValue);
        }

        public static ShoMatrix CreateDefaultRowKeyInstance(DoubleArray x, List<string> colKeys, double missingValue)
        {
            int R = x.size0;
            List<string> rowKeys = SpecialFunctions.MakeStringListOfIncreasingIntegers(0, R);
            return new ShoMatrix(x, rowKeys, colKeys, missingValue);
        }


        /// <summary>
        /// create a ShoMatrix with default row/col keys of "1", "2", etc,
        ///This is useful when we have a doubleArray, but want to use a ShoMatrix-based method
        /// </summary>
        /// <param name="x"></param>
        ///<param name="missingValue"></param>
        public static ShoMatrix CreateDefaultRowAndColKeyInstance(DoubleArray x, double missingValue)
        {
            int R = x.size0;
            int C = x.size1;
            List<string> rowKeys = SpecialFunctions.MakeStringListOfIncreasingIntegers(0, R);
            List<string> colKeys = SpecialFunctions.MakeStringListOfIncreasingIntegers(0, C);
            return new ShoMatrix(x, rowKeys, colKeys, missingValue);
        }

        //!!!Matrix: Should we use "new" or "GetInstance"
        public ShoMatrix(double[,] csArray, IEnumerable<string> varList, IEnumerable<string> cidList, double missingValue)
        {
            _rowKeys = varList.ToList();
            _colKeys = cidList.ToList();
            Helper.CheckCondition(csArray.GetLength(0) == _rowKeys.Count, "Expect the # of rows in the input array to match the # of items in varList");
            Helper.CheckCondition(csArray.GetLength(1) == _colKeys.Count, "Expect the # of cols in the input array to match the # of items in cidList");

            //!!!Matrix - these lines appear in many places
            _indexOfRowKey = RowKeys.Select((cid, index) => new { cid, index }).ToDictionary(pair => pair.cid, pair => pair.index).AsRestrictedAccessDictionary();
            _indexOfColKey = ColKeys.Select((cid, index) => new { cid, index }).ToDictionary(pair => pair.cid, pair => pair.index).AsRestrictedAccessDictionary();


            DoubleArray = DoubleArray.From(csArray);
            for (int rowIndex = 0; rowIndex < DoubleArray.size0; ++rowIndex)
            {
                for (int colIndex = 0; colIndex < DoubleArray.size1; ++colIndex)
                {
                    DoubleArray[rowIndex, colIndex] = csArray[rowIndex, colIndex];
                }
            }
            _missingValue = missingValue;
        }

        public ShoMatrix(DoubleArray doubleArray, IEnumerable<string> rowKeys, IEnumerable<string> colKeys, double missingValue)
            : this(ref doubleArray, rowKeys, colKeys, missingValue)
        {
        }

        /// <summary>
        /// Note that doubleArray is not copied. It is used by reference.
        /// </summary>
        /// <param name="doubleArray"></param>
        /// <param name="varList"></param>
        /// <param name="cidList"></param>
        /// <param name="missingValue"></param>
        public ShoMatrix(ref DoubleArray doubleArray, IEnumerable<string> varList, IEnumerable<string> cidList, double missingValue)
        {
            _rowKeys = varList.ToList();
            _colKeys = cidList.ToList();
            Helper.CheckCondition(doubleArray.size0 == _rowKeys.Count, "Expect the # of rows in the input array to match the # of items in varList");
            Helper.CheckCondition(doubleArray.size1 == _colKeys.Count, "Expect the # of cols in the input array to match the # of items in cidList");

            //!!!Matrix - these lines appear in many places
            _indexOfRowKey = RowKeys.Select((cid, index) => new { cid, index }).ToDictionary(pair => pair.cid, pair => pair.index).AsRestrictedAccessDictionary();
            _indexOfColKey = ColKeys.Select((cid, index) => new { cid, index }).ToDictionary(pair => pair.cid, pair => pair.index).AsRestrictedAccessDictionary();

            DoubleArray = doubleArray;
            _missingValue = missingValue;
        }

        [Obsolete("Use .TransposeView instead?")]
        public ShoMatrix Transpose()
        {
            var matrix = new ShoMatrix();
            matrix._rowKeys = ColKeys;
            matrix._colKeys = RowKeys;
            matrix._indexOfRowKey = IndexOfColKey;
            matrix._indexOfColKey = IndexOfRowKey;
            matrix._missingValue = MissingValue;
            matrix.DoubleArray = DoubleArray.Transpose();
            return matrix;
        }

        public static ShoMatrix CreateDefaultInstance(IEnumerable<string> rowEnumerable, IEnumerable<string> colEnumerable, double missingValue)
        {
            var matrix = new ShoMatrix();
            matrix._rowKeys = rowEnumerable.ToList();
            matrix._colKeys = colEnumerable.ToList();
            matrix._indexOfRowKey = matrix.RowKeys.Select((cid, index) => new { cid, index }).ToDictionary(pair => pair.cid, pair => pair.index).AsRestrictedAccessDictionary();
            matrix._indexOfColKey = matrix.ColKeys.Select((cid, index) => new { cid, index }).ToDictionary(pair => pair.cid, pair => pair.index).AsRestrictedAccessDictionary();
            matrix._missingValue = missingValue;
            matrix.DoubleArray = new DoubleArray(matrix.RowCount, matrix.ColCount);
            return matrix;
        }

        //public override Matrix<string, string, double> SelectRowsAndCols(IEnumerable<int> rowIndexEnumerable, IEnumerable<int> colIndexEnumerable)
        //{
        //    return SelectRowsAndCols(rowIndexEnumerable.Select(rowIndex => RowKeys[rowIndex]), colIndexEnumerable.Select(colIndex => ColKeys[colIndex]));
        //}

        //public override Matrix<string, string, double> SelectRowsAndCols(IEnumerable<string> rowKeyEnumerable, IEnumerable<string> colKeyEnumerable)
        //{
        //    var matrix = CreateDefaultInstance(rowKeyEnumerable, colKeyEnumerable, MissingValue);
        //    List<int> oldRowIndexList = rowKeyEnumerable.Select(newRowKey => IndexOfRowKey[newRowKey]).ToList();
        //    List<int> oldColIndexList = colKeyEnumerable.Select(newColKey => IndexOfColKey[newColKey]).ToList();

        //    for (int newRowIndex = 0; newRowIndex < matrix.RowCount; ++newRowIndex)
        //    {
        //        int oldRowIndex = oldRowIndexList[newRowIndex];

        //        for (int newColIndex = 0; newColIndex < matrix.ColCount; ++newColIndex)
        //        {
        //            matrix.DoubleArray[newRowIndex, newColIndex] = DoubleArray[oldRowIndex, oldColIndexList[newColIndex]];
        //        }
        //    }

        //    return matrix;
        //}




        public ShoMatrix(Matrix<string, string, double> inputMatrix, bool verbose = false)
        {
            if (verbose)
            {
                ShoMatrixVerbose(inputMatrix);
            }
            else
            {
                ShoMatrixNotVerbose(inputMatrix);
            }
        }

        private void ShoMatrixVerbose(Matrix<string, string, double> inputMatrix)
        {
            Init(inputMatrix);

            var counterWithMessages = new CounterWithMessages("ToShoMatrix ", null, inputMatrix.RowCount * inputMatrix.ColCount);
            DoubleArray = new DoubleArray(RowCount, ColCount);
            for (int rowIndex = 0; rowIndex < DoubleArray.size0; ++rowIndex)
            {
                for (int colIndex = 0; colIndex < DoubleArray.size1; ++colIndex)
                {
                    counterWithMessages.Increment();
                    DoubleArray[rowIndex, colIndex] = inputMatrix.GetValueOrMissing(rowIndex, colIndex);
                }
            }
            counterWithMessages.Finished();
        }

        private void ShoMatrixNotVerbose(Matrix<string, string, double> inputMatrix)
        {
            Init(inputMatrix);

            DoubleArray = new DoubleArray(RowCount, ColCount);
            for (int rowIndex = 0; rowIndex < DoubleArray.size0; ++rowIndex)
            {
                for (int colIndex = 0; colIndex < DoubleArray.size1; ++colIndex)
                {
                    DoubleArray[rowIndex, colIndex] = inputMatrix.GetValueOrMissing(rowIndex, colIndex);
                }
            }
        }

        private void Init(Matrix<string, string, double> inputMatrix)
        {
            _rowKeys = inputMatrix.RowKeys.ToList(); //!!!matrix: how come we copy at some places and not at others?
            _colKeys = inputMatrix.ColKeys.ToList();

            //!!!Matrix - these lines appear in many places
            _indexOfRowKey = RowKeys.Select((cid, index) => new { cid, index }).ToDictionary(pair => pair.cid, pair => pair.index).AsRestrictedAccessDictionary();
            _indexOfColKey = ColKeys.Select((cid, index) => new { cid, index }).ToDictionary(pair => pair.cid, pair => pair.index).AsRestrictedAccessDictionary();


            _missingValue = inputMatrix.MissingValue;
        }


        static public ShoMatrix GetInstance<TValue>(Matrix<string, string, TValue> inputMatrix, Func<TValue, double> valueConverter, double missingValue)
        {
            ShoMatrix shoMatrix = new ShoMatrix();
            shoMatrix._rowKeys = inputMatrix.RowKeys.ToList(); //!!!matrix: how come we copy at some places and not at others?
            shoMatrix._colKeys = inputMatrix.ColKeys.ToList();

            //!!!Matrix - these lines appear in many places
            shoMatrix._indexOfRowKey = shoMatrix.RowKeys.Select((cid, index) => new { cid, index }).ToDictionary(pair => pair.cid, pair => pair.index).AsRestrictedAccessDictionary();
            shoMatrix._indexOfColKey = shoMatrix.ColKeys.Select((cid, index) => new { cid, index }).ToDictionary(pair => pair.cid, pair => pair.index).AsRestrictedAccessDictionary();


            shoMatrix._missingValue = missingValue;

            shoMatrix.DoubleArray = new DoubleArray(shoMatrix.RowCount, shoMatrix.ColCount);
            for (int rowIndex = 0; rowIndex < shoMatrix.DoubleArray.size0; ++rowIndex)
            {
                for (int colIndex = 0; colIndex < shoMatrix.DoubleArray.size1; ++colIndex)
                {
                    TValue oldValue;
                    double newValue;
                    if (inputMatrix.TryGetValue(rowIndex, colIndex, out oldValue))
                    {
                        newValue = valueConverter(oldValue);
                    }
                    else
                    {
                        newValue = missingValue;
                    }
                    shoMatrix.DoubleArray[rowIndex, colIndex] = newValue;
                }
            }
            return shoMatrix;
        }

        public int[] Size
        {
            get
            {
                return this.DoubleArray.Size;
            }
        }

        private DoubleArray _doubleArray;
        public DoubleArray DoubleArray
        {
            set
            {
                if (_doubleArray != null)
                {
                    if (value.size0 != _doubleArray.size0) throw new Exception("DoubleArray is not of the right size0");
                    if (value.size1 != _doubleArray.size1) throw new Exception("DoubleArray is not of the right size1");
                }
                _doubleArray = value;
            }
            get
            {
                return _doubleArray;
            }
        }


        internal double _missingValue;
        override public double MissingValue
        {
            get { return _missingValue; }
        }

        internal ShoMatrix()
        {
        }


        internal IList<string> _rowKeys;
        override public ReadOnlyCollection<string> RowKeys { get { return new ReadOnlyCollection<string>(_rowKeys); } }

        internal IList<string> _colKeys;
        override public ReadOnlyCollection<string> ColKeys { get { return new ReadOnlyCollection<string>(_colKeys); } }

        internal IDictionary<string, int> _indexOfRowKey;
        override public IDictionary<string, int> IndexOfRowKey { get { return _indexOfRowKey; } }

        internal IDictionary<string, int> _indexOfColKey;
        override public IDictionary<string, int> IndexOfColKey { get { return _indexOfColKey; } }


        public override void SetValueOrMissing(string rowKey, string colKey, double value)
        {
            DoubleArray[IndexOfRowKey[rowKey], IndexOfColKey[colKey]] = value;
        }

        public override void SetValueOrMissing(int rowIndex, int colIndex, double value)
        {
            DoubleArray[rowIndex, colIndex] = value;
        }


        //override public double this[int rowIndex, int colIndex]
        //{
        //    get
        //    {
        //        double value = DoubleArray[rowIndex, colIndex];
        //        Helper.CheckCondition(!value.Equals(MissingValue), "Value is missing. Consider 'GetValueOrMissing' or 'TryGet'");
        //        return value;
        //    }
        //    set
        //    {
        //        DoubleArray[rowIndex, colIndex] = value;
        //    }
        //}

        //public override double this[string rowKey, string colKey]
        //{
        //    get
        //    {
        //        return this[IndexOfRowKey[rowKey], IndexOfColKey[colKey]];
        //    }
        //    set
        //    {
        //        this[IndexOfRowKey[rowKey], IndexOfColKey[colKey]] = value;
        //    }
        //}

        public override void WriteDense(System.IO.TextWriter textWriter)
        {
            textWriter.WriteLine("var\t{0}", ColKeys.StringJoin("\t"));
            for (int rowIndex = 0; rowIndex < RowCount; ++rowIndex)
            {
                textWriter.Write(RowKeys[rowIndex]);
                for (int colIndex = 0; colIndex < ColCount; ++colIndex)
                {
                    textWriter.Write("\t");
                    textWriter.Write(DoubleArray[rowIndex, colIndex]);
                }
                textWriter.WriteLine();
            }
        }

        override public bool TryGetValue(int rowIndex, int colIndex, out double value)
        {
            value = DoubleArray[rowIndex, colIndex];
            return !value.Equals(MissingValue);
        }

        public override bool TryGetValue(string rowKey, string colKey, out double value)
        {
            return TryGetValue(IndexOfRowKey[rowKey], IndexOfColKey[colKey], out value);
        }

        public override bool Remove(string rowKey, string colKey)
        {
            return Remove(IndexOfRowKey[rowKey], IndexOfColKey[colKey]);
        }

        // true if the element is successfully found and removed; otherwise, false.
        public override bool Remove(int rowIndex, int colIndex)
        {
            if (IsMissing(DoubleArray[rowIndex, colIndex]))
            {
                return false;
            }
            else
            {
                DoubleArray[rowIndex, colIndex] = MissingValue;
                return true;
            }
        }

        /// <summary>
        /// When a matrix is singular, make it have fewer rows by using SVD and removing
        /// PCs that have singular values less than tol=1e-8;
        /// </summary>
        /// <returns>the new matrix (or null if no changes were made) </returns>
        public ShoMatrix CoerceToBeFullRankBySvdRowRemoval(out bool madeChange)
        {
            ShoMatrix newKmatrix = this;
            DoubleArray projections = this.DoubleArray.CoerceToBeFullRankBySvdRowRemoval(out madeChange);
            if (madeChange)
            {
                List<string> newRowKeys = new List<string>();
                for (int g = 0; g < projections.size0; g++) newRowKeys.Add("pseudoFeat" + g);
                newKmatrix = new ShoMatrix(projections, newRowKeys, this.ColKeys, this.MissingValue);
            }
            return newKmatrix;
        }

        /// <summary>
        /// Add a bunch of 1's in the last row, and call this row key "bias"--useful for regression
        /// </summary>
        /// <returns></returns>
        public ShoMatrix AddBiasRow()
        {
            DoubleArray newD = ShoUtils.AddBiasLastToInputData(this.DoubleArray.T, this.ColCount).T;
            List<string> newRowKeys = this.RowKeys.ToList();
            newRowKeys.Add("bias");
            ShoMatrix x = new ShoMatrix(newD, newRowKeys, this.ColKeys, this.MissingValue);
            return x;
        }


        public static Matrix<string, string, double> ReadSymmetric(string covarianceFileName, ParallelOptions parallelOptions)
        {
            Console.WriteLine("Reading symmetric file " + covarianceFileName);
            var matrixFactorySSD = MatrixFactory<string, string, double>.GetInstance();
            Matrix<string, string, double> matrix = matrixFactorySSD.Parse(covarianceFileName, double.NaN, parallelOptions);
            Helper.CheckCondition(matrix.RowKeys.SequenceEqual(matrix.ColKeys), "Expect cov files to have some rows keys as col keys, in the same order");


            //!!!could be done in parallel
            for (int rowIndex = 0; rowIndex < matrix.RowCount; ++rowIndex)
            {
                for (int colIndex = rowIndex + 1; colIndex < matrix.ColCount; ++colIndex)
                {
                    Helper.CheckCondition(matrix[rowIndex, colIndex] == matrix[colIndex, rowIndex],
                        string.Format("Expect file to be symmetric, but it is not at {0},{1}. The values are {2} and {3}. File={4}", matrix.RowKeys[rowIndex], matrix.ColKeys[colIndex], matrix[rowIndex, colIndex], matrix[colIndex, rowIndex], covarianceFileName));
                }
            }


            return matrix;
        }


        /// <summary>
        ///  check that all non-missing entries are one of the allowed vals
        /// </summary>
        /// <param name="allowedVals"></param>
        public void CheckDataEncoding(List<double> allowedVals)
        {
            for (int r = 0; r < this.RowCount; r++)
            {
                for (int c = 0; c < this.ColCount; c++)
                {
                    double val;
                    bool notMissing = this.TryGetValue(r, c, out val);
                    if (notMissing && !allowedVals.Contains(val))
                        throw new Exception(string.Format("data encoding not valid: found {0} but only allowed {1}", val, allowedVals.ToString()));
                }
            }
        }

        public static IntArray ToIntArray(Matrix<string, string, double> matrix, int missingValInt)
        {
            IntArray x = new IntArray(matrix.RowCount, matrix.ColCount);
            for (int r = 0; r < matrix.RowCount; r++)
            {
                for (int c = 0; c < matrix.ColCount; c++)
                {
                    double val;
                    bool hasVal = matrix.TryGetValue(r, c, out val);
                    x[r, c] = hasVal ? (int)val : missingValInt;
                }
            }
            return x;
        }


        public static DoubleArray GetLinearComboOfDoubleArraysInPlace(List<Matrix<string, string, double>> kernelMatrixes, DoubleArray bestLinearComboKernelWeights)
        {
            Helper.CheckCondition(kernelMatrixes.Count == bestLinearComboKernelWeights.Count);
            DoubleArray result = ShoUtils.DoubleArrayZeros(kernelMatrixes[0].RowCount, kernelMatrixes[0].ColCount);
            for (int j = 0; j < kernelMatrixes.Count; j++)
            {
                result = result + bestLinearComboKernelWeights[j] * kernelMatrixes[j].AsShoMatrix().DoubleArray;
            }
            return result;
        }

    }

    public static class IMatrixExtensions
    {


        static public ShoMatrix AsShoMatrix(this Matrix<string, string, double> inputMatrix)
        {
            if (inputMatrix is ShoMatrix)
            {
                return (ShoMatrix)inputMatrix;
            }
            else
            {
                return ToShoMatrix(inputMatrix);
            }
        }




        static public ShoMatrix ToShoMatrix(this Matrix<string, string, double> inputMatrix, bool verbose = false)
        {
            //Special code for materializing views

            //SelectRowsAndColsView
            var selectRowsAndColsView = inputMatrix as SelectRowsAndColsView<string, string, double>;
            if (null != selectRowsAndColsView && selectRowsAndColsView.ParentMatrix is ShoMatrix)
            {
                return MaterializeSelectRowsColsView(selectRowsAndColsView);
            }
            //Transpose
            var transposeView = inputMatrix as TransposeView<string, string, double>;
            if (transposeView != null && transposeView.ParentMatrix is ShoMatrix)
            {
                return MaterializeTransposeView(transposeView);
            }

            ShoMatrix outputMatrix = new ShoMatrix(inputMatrix, verbose);
            return outputMatrix;
        }

        private static ShoMatrix MaterializeTransposeView(TransposeView<string, string, double> transposeView)
        {
            ShoMatrix parent = (ShoMatrix)transposeView.ParentMatrix;

            var matrix = new ShoMatrix();
            matrix._rowKeys = transposeView.RowKeys;
            matrix._colKeys = transposeView.ColKeys;
            matrix._indexOfRowKey = transposeView.IndexOfRowKey;
            matrix._indexOfColKey = transposeView.IndexOfColKey;
            matrix._missingValue = parent.MissingValue;
            matrix.DoubleArray = new DoubleArray(matrix.RowCount, matrix.ColCount);

            for (int newRowIndex = 0; newRowIndex < matrix.RowCount; ++newRowIndex)
            {
                for (int newColIndex = 0; newColIndex < matrix.ColCount; ++newColIndex)
                {
                    matrix.DoubleArray[newRowIndex, newColIndex] = parent.DoubleArray[newColIndex, newRowIndex];
                }
            }

            return matrix;
        }

        private static ShoMatrix MaterializeSelectRowsColsView(SelectRowsAndColsView<string, string, double> selectRowsAndColsView)
        {
            ShoMatrix parent = (ShoMatrix)selectRowsAndColsView.ParentMatrix;

            var matrix = new ShoMatrix();
            matrix._rowKeys = selectRowsAndColsView.RowKeys;
            matrix._colKeys = selectRowsAndColsView.ColKeys;
            matrix._indexOfRowKey = selectRowsAndColsView.IndexOfRowKey;
            matrix._indexOfColKey = selectRowsAndColsView.IndexOfColKey;
            matrix._missingValue = parent.MissingValue;
            matrix.DoubleArray = new DoubleArray(matrix.RowCount, matrix.ColCount);

            IList<int> oldRowIndexList = selectRowsAndColsView.IndexOfParentRowKey;
            IList<int> oldColIndexList = selectRowsAndColsView.IndexOfParentColKey;

            for (int newRowIndex = 0; newRowIndex < matrix.RowCount; ++newRowIndex)
            {
                int oldRowIndex = oldRowIndexList[newRowIndex];

                for (int newColIndex = 0; newColIndex < matrix.ColCount; ++newColIndex)
                {
                    matrix.DoubleArray[newRowIndex, newColIndex] = parent.DoubleArray[oldRowIndex, oldColIndexList[newColIndex]];
                }
            }

            return matrix;
        }



        //[Obsolete("Could we use 'ConverterView' instead and thus simplify the API? Could it be as efficient?")]
        //static public ShoMatrix ToShoMatrix<TValue>(this Matrix<string, string, TValue> inputMatrix, Func<TValue, double> valueConverter, double missingValue)
        //{
        //    ShoMatrix outputMatrix = ShoMatrix.GetInstance(inputMatrix, valueConverter, missingValue);
        //    return outputMatrix;
        //}
    }
}
