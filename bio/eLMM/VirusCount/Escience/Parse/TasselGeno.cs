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
    public class TasselGeno : FileMatrix<string>, IParsable
    {
        [Parse(ParseAction.Required, typeof(InputFile))]
        public FileInfo SnpFile;


        public override Matrix<string, string, string> LoadParent()
        {

            var pairAnsi = DensePairAnsi.GetInstanceFromSparse(TripleEnumerable());

            var pairAnsiWithStringValues = pairAnsi.ConvertValueView(MBT.Escience.ValueConverter.UOPairToString, "??");

            return pairAnsiWithStringValues;
        }


        private IEnumerable<RowKeyColKeyValue<string, string, UOPair<char>>> TripleEnumerable()
        {

            CounterWithMessages counterWithMessages = new CounterWithMessages("Reading " + SnpFile.Name, messageIntervalOrNull: 1000);

            using (TextReader textReader = SnpFile.OpenText())
            {
                string headerLine = textReader.ReadLine();
                Helper.CheckCondition(headerLine != null, "Expect file to contain a first line");
                string[] headerFields = headerLine.Split('\t');
                Helper.CheckCondition(headerFields.Length > 0 && headerFields[0] == "", "Expect first column of first line to be blank");

                string line;
                while (null != (line = textReader.ReadLine()))
                {
                    counterWithMessages.Increment();
                    string[] fields = line.Split('\t');
                    Helper.CheckCondition(fields.Length == headerFields.Length, "Expect all lines to have the same # of columns");
                    string cid = fields[0];
                    for (int snpIndex = 1; snpIndex < headerFields.Length; ++snpIndex) // start at one to skip over 1st column
                    {
                        string snp = headerFields[snpIndex];
                        string valueInFile = fields[snpIndex];
                        UOPair<char> uoPair;
                        if (valueInFile == "-")
                        {
                            continue; // not break;
                        }
                        else if (valueInFile.Length == 1)
                        {
                            char c = valueInFile[0];
                            Helper.CheckCondition("ACTG".Contains(c), () => "Expect values in snp file to be ACT or G. " + valueInFile);
                            uoPair = UOPair.Create(c, c);
                        }
                        else
                        {
                            Helper.CheckCondition(valueInFile.Length == 3 && valueInFile[1] == '/' && "ACTG".Contains(valueInFile[0]) && "ACTG".Contains(valueInFile[2]), () => "Expect longer values in snp file be of the form 'a/b' where a & b are ACT or G");
                            uoPair = UOPair.Create(valueInFile[0], valueInFile[2]);
                        }
                        yield return RowKeyColKeyValue.Create(snp, cid, uoPair);
                    }
                }
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
