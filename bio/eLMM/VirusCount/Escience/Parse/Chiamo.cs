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
#if !SILVERLIGHT
using System.IO.Compression;
#endif

namespace MBT.Escience.Parse
{
    /// <summary>
    /// ??? WTCC data format ???
    /// </summary>
    [DoNotParseInherited]
    public class Chiamo : FileMatrix<string>, IParsable
    {
        [Parse(ParseAction.Required, typeof(InputFile))]
        public FileInfo SnpFile;

        [Parse(ParseAction.Required, typeof(InputFile))]
        public FileInfo CidExcludeFile;

        [Parse(ParseAction.Required)]
        public List<string> ExcludeExcludeList;

        [Parse(ParseAction.Required, typeof(InputFile))]
        public FileInfo SnpExcludeFile;

        [Parse(ParseAction.Required)]
        public double MissingThreshold;

        [Parse(ParseAction.Optional)]
        public bool GZ = false;


        public override Matrix<string, string, string> LoadParent()
        {

            var cidExcludeSet = CreateCidExcludeSet();

            var snpExcludeSet = CreateSnpExcludeSet();



            var pairAnsi = DensePairAnsi.GetInstanceFromSparse(TripleEnumerable(cidExcludeSet, snpExcludeSet));

            Helper.CheckCondition(pairAnsi.RowKeys.All(rowKey => rowKey == rowKey.Trim()), "Expect all snps in snp file to be trimmed");
            Helper.CheckCondition(pairAnsi.ColKeys.All(colKey => colKey == colKey.Trim().ToUpperInvariant()), "Expect all cids in snp file to be trimmed and in upper case");

            var pairAnsiWithStringValues = pairAnsi.ConvertValueView(MBT.Escience.ValueConverter.UOPairToString, "??");

            return pairAnsiWithStringValues;
        }

        private HashSet<string> CreateSnpExcludeSet()
        {
            if (null == SnpExcludeFile)
            {
                return new HashSet<string>();
            };

            var snpExcludeSet = new HashSet<string>();
            using (TextReader textReader = SnpExcludeFile.OpenText())
            {
                string line;
                bool seenFirst = false;
                while (null != (line = textReader.ReadLine()))
                {
                    if (line.StartsWith("#")) //Skip comments
                    {
                        continue; //Not break;
                    }
                    if (!seenFirst)
                    {
                        seenFirst = true;
                        Helper.CheckCondition(line == "CHR	AFFY_ID	RS_ID	FILTER", "Expect header of 'CHR	AFFY_ID	RS_ID	FILTER'");
                        continue; // not break;
                    }
                    string[] field = line.Split('\t');
                    Helper.CheckCondition(field.Length == 4, "Expect lines of exclude snp file to have four fields. " + line);
                    string snp1 = field[1];
                    Helper.CheckCondition(snp1.StartsWith("SNP_A"), "Expect the snp in the 2nd column to start 'SNP_A'. " + snp1);
                    snpExcludeSet.Add(snp1);

                    string snp2 = field[2];
                    if (snp2 != "---------" && snp1 != snp2)
                    {
                        Helper.CheckCondition(snp2.StartsWith("rs"), "Expect the snp in the 3rd column to start 'rs'. " + snp2);
                        snpExcludeSet.Add(snp2);
                    }

                }
            }
            return snpExcludeSet;
        }

        private HashSet<string> CreateCidExcludeSet()
        {
            var cidExcludeSet = new HashSet<string>();
            if (null == CidExcludeFile)
            {
                Helper.CheckCondition(ExcludeExcludeList.Count == 0, "When there is no exclusion file, expect the exclude exclude list to be empty");
                return cidExcludeSet;
            }
            using (TextReader textReader = CidExcludeFile.OpenText())
            {
                string line;
                while (null != (line = textReader.ReadLine()))
                {
                    if (line.StartsWith("#")) //Skip comments
                    {
                        continue; //Not break;
                    }
                    string[] field = line.Split('\t');
                    Helper.CheckCondition(field.Length == 4, "Expect lines of exclude file to have four fields. " + line);
                    string cid = field[2];
                    Helper.CheckCondition(cid == cid.Trim() && cid == cid.ToUpperInvariant(), "Expect cids in exclude file to be trimmed of whitespace and in uppercase. ", cid);
                    string[] reasonList = field[3].Split(',');
                    Helper.CheckCondition(reasonList.Length > 0, "Expect exclude file's reason to to have at least one reason. ", cid);
                    Helper.CheckCondition(reasonList.All(reason => reason == reason.ToLowerInvariant()), "Expect exclude files's reasons to use only lower case letters. " + field[3]);
                    var remainingReasonList = reasonList.Except(ExcludeExcludeList);
                    if (remainingReasonList.Count() > 0)
                    {
                        cidExcludeSet.Add(cid);
                    }
                }
            }
            return cidExcludeSet;
        }

        private IEnumerable<RowKeyColKeyValue<string, string, UOPair<char>>> TripleEnumerable(HashSet<string> cidExcludeList, HashSet<string> snpExcludeSet)
        {
            CounterWithMessages counterWithMessages = new CounterWithMessages(SnpFile, compressionRatio: GZ ? .9 : 0);

            using (TextReader textReader = 
#if !SILVERLIGHT

                GZ ? SnpFile.UnGZip()  :
#endif
                SnpFile.OpenText())
            {
                string line;
                while (null != (line = textReader.ReadLine()))
                {
                    counterWithMessages.Increment();
                    string[] field = line.Split('\t');
                    Helper.CheckCondition(field.Length == 4, "Expect lines of snp file to have four fields. " + line);
                    string snp = field[0];
                    string cid = field[1];
                    string value = field[2];
                    double confidence = double.Parse(field[3]);
                    Helper.CheckCondition(value.Length == 2 && value.All(c => "ACTG".Contains(c)), () => "Expect values in snp file to be a pair of ACT or G. " + value);
                    if (cidExcludeList.Contains(cid) || confidence < MissingThreshold || snpExcludeSet.Contains(snp))
                    {
                        continue; //not break;
                    }

                    yield return RowKeyColKeyValue.Create(snp, cid, UOPair.Create(value[0], value[1]));
                }
            }

            counterWithMessages.Finished();
        }

        //!!!will this leave many things open??
        // MOVED TO FILEUTILS
        //public static StreamReader UnGZip(FileInfo SnpFile)
        //{
        //    FileStream fileStream = SnpFile.OpenRead();
        //    GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress);
        //    StreamReader streamReader = new StreamReader(gzipStream);
        //    return streamReader;
        //}

        #region IParsable Members

        void IParsable.FinalizeParse()
        {
            Helper.CheckCondition(ExcludeExcludeList.All(reason => reason == reason.ToLowerInvariant()), "Expect ExcludeExcludeList to use only lower case letters. ");
            Helper.CheckCondition(0 <= MissingThreshold && MissingThreshold <= 1, "Expect a MissingThreshold between 0 and 1 (inclusive)");
            base.FinalizeParse();
        }

        #endregion
    }
}
