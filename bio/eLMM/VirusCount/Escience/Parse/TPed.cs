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
    public class TPed : FileMatrix<string>, IParsable
    {
        [Parse(ParseAction.Required, typeof(InputFile))]
        public FileInfo TPedFile;

        [Parse(ParseAction.Required, typeof(InputFile))]
        public FileInfo TFamFile;

        public override Matrix<string, string, string> LoadParent()
        {
            List<string> cidList =
                (from row in FileUtils.ReadDelimitedFile(TFamFile.FullName, new { FamilyID = "", IndividualID = "", PaternalID = "", MaternalID = "", Sex = 0, Phenotype = 0 }, new char[] { '\t', ' ' }, hasHeader: false)
                 select row.IndividualID
                 ).ToList();



            var pairAnsi = DensePairAnsi.GetInstance(SnpGroupsCidToNucPairEnumerable(cidList), '?');
            var pairAnsiWithStringValues = pairAnsi.ConvertValueView(MBT.Escience.ValueConverter.UOPairToString, "??");
            return pairAnsiWithStringValues;
        }

        private IEnumerable<IGrouping<string, KeyValuePair<string, UOPair<char>>>> SnpGroupsCidToNucPairEnumerable(List<string> cidList)
        {
            using (TextReader textReader = TPedFile.OpenText())
            {
                string line;
                while (null != (line = textReader.ReadLine()))
                {
                    string[] fields = line.Split(new[] { '\t' }, 5);
                    string snpName = fields[1];
                    string[] pairArray = fields[4].Split('\t');
                    Helper.CheckCondition(pairArray.Length == cidList.Count, "Expect the number of nuc pairs on a line to be the same as the number of cids in the tfam file");

                    var cidAndPairQuery = cidList.Zip(pairArray, (cid, pairString) => new KeyValuePair<string, UOPair<char>>(cid, StringToUOPairConverter(pairString)));
                    yield return Grouping<string, KeyValuePair<string, UOPair<char>>>.GetInstance(snpName, cidAndPairQuery.GetEnumerator());
                }
            }
        }

        private UOPair<char> StringToUOPairConverter(string pairString)
        {
            Helper.CheckCondition(pairString.Length == 3 && pairString[1] == ' ' && pairString[0] != '?' && pairString[2] != '?', "expect pair string in tped file to be three characters long with the middle being space and neither of the others being '?'. " + pairString);
            if (pairString[0] == '0' || pairString[2] == '0')
            {
                Helper.CheckCondition(pairString[0] == '0' && pairString[2] == '0', "if either character in a pair string is '0' they should both be. " + pairString);
                return UOPair.Create('?', '?');
            }

            return UOPair.Create(pairString[0], pairString[2]);
        }


        #region IParsable Members

        void IParsable.FinalizeParse()
        {
            base.FinalizeParse();
        }

        #endregion

    }
}
