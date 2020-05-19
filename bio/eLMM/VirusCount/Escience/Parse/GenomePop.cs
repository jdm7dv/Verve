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
using System.Threading.Tasks;
using Bio.Matrix;
using Bio.Util;
using System.IO;
using MBT.Escience.Matrix;

namespace MBT.Escience.Parse
{
    [DoNotParseInherited]
    public class GenomePop : FileMatrix<string>, IParsable
    {

        [Parse(ParseAction.Required, typeof(InputFile))]
        public FileInfo InputFile;


        #region IParsable Members

        void IParsable.FinalizeParse()
        {
            _lazyMatrix = new Lazy<Matrix<string, string, string>>(LoadParent);

        }

        public Matrix<string, string, string> LoadParent()
        {
            Matrix<string, string, UOPair<char>> densePairAnsi;
            bool isOK = GenomePopParser.TryGetInstance(InputFile.FullName, DensePairAnsi.StaticMissingValue, ParallelOptionsScope.Current, out densePairAnsi);
            Helper.CheckCondition<MatrixFormatException>(isOK, "Matrix parse error " + InputFile.Name);

            var stringValuedMatrix = densePairAnsi.ConvertValueView(MBT.Escience.ValueConverter.UOPairToString, "??");
            return stringValuedMatrix;
        }

        #endregion
    }



    public static class GenomePopParser
    {
        /// <summary>
        /// This awkward method is provided for the sake of MatrixFactory. Right now it simply catches exceptions. Should switch and make it fail silently when doesn't work.
        /// </summary>
        public static bool TryGetInstance(string genomePopFileName, UOPair<char> mustProvide_StaticMissingValue_OrExceptionWillBeThrown, ParallelOptions parallelOptions, out Matrix<string, string, UOPair<char>> paddedPair)
        {

            paddedPair = null;
            if (!mustProvide_StaticMissingValue_OrExceptionWillBeThrown.Equals(DensePairAnsi.StaticMissingValue)) //OK to use Equals because double can't be null
            {
                Console.Error.WriteLine("Missing value {0} doesn't match expected value {1}", DensePairAnsi.StaticMissingValue, mustProvide_StaticMissingValue_OrExceptionWillBeThrown);
                return false;
            }

            try
            {
                paddedPair = DensePairAnsi.GetInstanceFromSparse(TripleSequence(genomePopFileName));
                return true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return false;
            }
        }

        private static IEnumerable<RowKeyColKeyValue<string, string, UOPair<char>>> TripleSequence(string genomePopFileName)
        {
            using (TextReader textReader = File.OpenText(genomePopFileName))
            {
                string firstLine = textReader.ReadLine(); //Ignore first line
                Helper.CheckCondition(firstLine != null, "Expect genome pop file to contain at least one line");
                string snpLine = textReader.ReadLine();
                Helper.CheckCondition(snpLine != null, "Expect genome pop file to contain at least two lines");
                string[] snpArray = snpLine.Split(',');
                string line;
                int cidIndex = -1;
                while (null != (line = textReader.ReadLine()))
                {
                    if (line == "pop")
                    {
                        continue; //not break
                    }
                    ++cidIndex;
                    string cid = string.Format("cid{0}", cidIndex);
                    throw new Exception("Why did the next line have a ', StringSplitOptions.RemoveEmptyEntries'???");
                    string[] twoParts = line.Split(new char[] { ',' }, 2);
                    string[] valueArray = twoParts[1].TrimStart().Split(' ');
                    Helper.CheckCondition(valueArray.Length == snpArray.Length, "Expect each line to contain one entry per snp. " + cid);
                    for (int snpIndex = 0; snpIndex < snpArray.Length; ++snpIndex)
                    {
                        string value = valueArray[snpIndex];
                        string snp = snpArray[snpIndex];
                        switch (value)
                        {
                            case "0101":
                                yield return RowKeyColKeyValue.Create(snp, cid, UOPair.Create('1', '1'));
                                break;
                            case "0102":
                            case "0201":
                                yield return RowKeyColKeyValue.Create(snp, cid, UOPair.Create('1', '2'));
                                break;
                            case "0202":
                                yield return RowKeyColKeyValue.Create(snp, cid, UOPair.Create('2', '2'));
                                break;
                            default:
                                throw new MatrixFormatException("Illegal value " + value);
                        }
                    }
                }
            }
        }
    }
}
