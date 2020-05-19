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
using MBT.Escience.Matrix;
using System.IO;
using Bio.Util;
using Bio.Matrix;

namespace MBT.Escience.Parse
{
    [DoNotParseInherited]
    public class RussAltman : FileMatrix<string>, IParsable
    {
        [Parse(ParseAction.Required, typeof(InputFile))]
        public FileInfo SnpFile;

        public override Matrix<string, string, string> LoadParent()
        {

            var pairAnsi = DensePairAnsi.GetInstanceFromSparse(TripleEnumerable());

            Helper.CheckCondition(pairAnsi.RowKeys.All(rowKey => rowKey == rowKey.Trim()), "Expect all snps in snp file to be trimmed");
            Helper.CheckCondition(pairAnsi.ColKeys.All(colKey => colKey == colKey.Trim()), "Expect all cids in snp file to be trimmed");


            var pairAnsiWithStringValues = pairAnsi.ConvertValueView(MBT.Escience.ValueConverter.UOPairToString, "??");

            return pairAnsiWithStringValues;
        }



        private IEnumerable<RowKeyColKeyValue<string, string, UOPair<char>>> TripleEnumerable()
        {

            //int? totalLineCountOrNull = null;
            //int? messageIntervalOrNull = 10000;
            //using (TextReader textReader = SnpFile.OpenText())
            //{
            //    string line = textReader.ReadLine();
            //    if (null != line || line.Length == 0)
            //    {
            //        totalLineCountOrNull = (int?)(SnpFile.Length / (long)(line.Length +  2 /*line end*/));
            //        messageIntervalOrNull = null;
            //    }
            //}


            CounterWithMessages counterWithMessages = new CounterWithMessages(SnpFile); //"Reading " + SnpFile.Name, messageIntervalOrNull, totalLineCountOrNull);

            using (TextReader textReader = SnpFile.OpenText())
            {
                string line;
                while (null != (line = textReader.ReadLine()))
                {
                    counterWithMessages.Increment();
                    string[] field = line.Split('\t');
                    Helper.CheckCondition(field.Length == 3, "Expect lines of snp file to have three fields. " + line);
                    string cid = field[0];
                    string snp = field[1];
                    string value = field[2];
                    if (value == "00")
                    {
                        continue; //not break;
                    }

                    Helper.CheckCondition(value.Length == 2 && value.All(c => "ACTG".Contains(c)), () => "Expect values in snp file to be a pair of ACTG or 00. " + value);

                    yield return RowKeyColKeyValue.Create(snp, cid, UOPair.Create(value[0], value[1]));
                }
                counterWithMessages.Finished();

            }

        }

        #region IParsable Members

        void IParsable.FinalizeParse()
        {
            base.FinalizeParse();
        }

        #endregion
    }
}
