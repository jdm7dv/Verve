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
using System.IO;
using Bio.Util;
using MBT.Escience.Parse;

namespace MBT.Escience.Biology.Hla
{
    public class HlaParser
    {
        /// <summary>
        /// Must be a standard Hla file containing the following columns:
        /// <br>pid A1 A2 B1 B2 C1 C2 [ProbabilityOfCompletion]</br>
        /// The last column is optional. If present, the probability will be assigned to the given completion. 
        /// The column names can be anything, but must be in that order. 
        /// </summary>
        [Parse(ParseAction.Required, typeof(InputFile))]
        public FileInfo HlaFile;

        /// <summary>
        /// Any resolution higher than the max will be trimmed off.
        /// </summary>
        public HlaIResolution MaxResolution = HlaIResolution.Protein;

        private const string HlaCompletionHeader = @"pid	A	A	B	B	C	C	probability of completion	lower-resolution model used";

        public virtual Dictionary<string, List<HlaIGenotype>> ParseFile()
        {
            Dictionary<string, List<HlaIGenotype>> result = new Dictionary<string, List<HlaIGenotype>>();
            using (TextReader reader = HlaFile.OpenTextOrUseConsole(stripComments: true))
            {
                string header = reader.ReadLine();
                bool isHlaCompletionFile = header.Equals(HlaCompletionHeader);
                int colCount = header.Split('\t').Length;
                Helper.CheckCondition<ParseException>(isHlaCompletionFile || colCount == 7 || colCount == 8, "Expect a file with 7 or 8 columns. Read {0}", header);
                bool parseProbabilityLine = isHlaCompletionFile || colCount == 8;

                string line;
                while (null != (line = reader.ReadLine()))
                {
                    string[] fields = line.Split('\t');
                    try
                    {
                        string pid = fields[0];

                        HlaI a1 = ParseHla(HlaILocus.A, fields[1]);
                        HlaI a2 = ParseHla(HlaILocus.A, fields[2], a1);
                        HlaI b1 = ParseHla(HlaILocus.B, fields[3]);
                        HlaI b2 = ParseHla(HlaILocus.B, fields[4], b1);
                        HlaI c1 = ParseHla(HlaILocus.C, fields[5]);
                        HlaI c2 = ParseHla(HlaILocus.C, fields[6], c1);

                        HlaIGenotype genotype = new HlaIGenotype(a1, a2, b1, b2, c1, c2);
                        genotype.SetMaxResolution(MaxResolution);

                        if (parseProbabilityLine)
                        {
                            double p;
                            (double.TryParse(fields[7], out p) && p >= 0 && p <= 1).Enforce<ParseException>("The 8th field is not a valid probability. Read {0}.", p);
                            genotype.Probability = p;
                        }

                        result.GetValueOrDefault(pid).Add(genotype);
                    }
                    catch (ParseException p)
                    {
                        throw new ParseException(p.Message + " for patient" + fields[0]);
                    }
                }
            }
            if (MaxResolution == HlaIResolution.Group)
            {
                foreach (var pidAndGenos in result)
                {
                    if (pidAndGenos.Value.Count > 1)
                    {
                        // we've stripped out some of the 4 digits. The resulting list may contain duplicates. Collapse those together and sum up their probabilities.
                        var collapsedGenotypes = from geno in pidAndGenos.Value
                                                 let hlaString = geno.EnumerateAll().StringJoin(" ")
                                                 group geno by hlaString into identicalHaps
                                                 select new HlaIGenotype(
                                                     identicalHaps.First().AAlleles.Item1,
                                                     identicalHaps.First().AAlleles.Item2,
                                                     identicalHaps.First().BAlleles.Item1,
                                                     identicalHaps.First().BAlleles.Item2,
                                                     identicalHaps.First().CAlleles.Item1,
                                                     identicalHaps.First().CAlleles.Item2) { Probability = identicalHaps.Sum(h => h.Probability) };
                        pidAndGenos.Value.Clear();
                        pidAndGenos.Value.AddRange(collapsedGenotypes);
                    }
                }
            }


            return result;
        }

        private static HlaI ParseHla(HlaILocus locus, string hlaString, HlaI otherCopyAtSameLocusOrNull = null)
        {
            Helper.CheckCondition<ParseException>(!string.IsNullOrWhiteSpace(hlaString), "Blank entries are not allowed. Use ? for missing or - for homozygous (second column of locus only).");
            
            switch (hlaString)
            {
                case "?":
                    return null;
                case "-":
                    Helper.CheckCondition<ParseException>(otherCopyAtSameLocusOrNull != null, "Can't mark the first column as homozygous. Only A2, B2 or C2 can be marked with -.");
                    return otherCopyAtSameLocusOrNull;
                default:
                    HlaI hla = new HlaI(locus);
                    hla.ParseInto(hlaString);
                    return hla;
            }
        }
    }
}
