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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bio.Util;
using MBT.Escience;

namespace MBT.Escience.Biology
{
    public class CaseIdToAASeq
    {
        private CaseIdToAASeq()
        {
        }

        private Dictionary<string, AASeq> _caseIdToAASeq;
        public Dictionary<string, AASeq> Dictionary
        {
            get
            {
                return _caseIdToAASeq;
            }
        }
        public int? SequenceLengthOrNull = null;

        static public CaseIdToAASeq GetInstance()
        {
            CaseIdToAASeq caseId = new CaseIdToAASeq();
            caseId._caseIdToAASeq = new Dictionary<string, AASeq>();
            return caseId;
        }

        //	/*
        //	1189MB	MEPVDPNLEPWNHPGSQPKTPCTNCYCKHCSYHCLVCFQTKGLGISYGRK
        //	J112MA	MEPVDPNLEPWNHPGSQPITACNKCYCKYCSYHCLVCFQTKGLGISYGRK
        //	1157M3M   MEPVDPNLEPWNHPGSQPKTPCNKCYCKHCSYHCLVCFQTKGLGISYGRK
        //	1195MB	MEPVDPNLEPWNHPGSQPKTPCNKCYCKYCSYHCLVCFQTKGLGISYGRK
        //	 */
        static public CaseIdToAASeq GetInstance(TextReader textReader, MixtureSemantics mixtureSemantics, int offset)
        {
            CaseIdToAASeq caseIdToAASeq = CaseIdToAASeq.GetInstance();
            foreach (Dictionary<string, string> row in SpecialFunctions.TabFileTable(textReader, "cid\taaSeq", false))
            {
                string caseId = row["cid"]; //!!!const
                string aaSeqAsString = row["aaSeq"];//!!!const
                AASeq aaSeq = AASeq.GetInstance(aaSeqAsString, mixtureSemantics, offset);
                caseIdToAASeq.Add(caseId, aaSeq);
            }

            return caseIdToAASeq;
        }


        static public CaseIdToAASeq GetInstance(string aaSeqFileName, MixtureSemantics mixtureSemantics, int offset)
        {
            using (TextReader textReader = Bio.Util.FileUtils.OpenTextStripComments(aaSeqFileName))
            {
                return GetInstance(textReader, mixtureSemantics, offset);
            }
        }

        //static public CaseIdToAASeq GetInstance(TextReader textReader, bool mixture)
        //{
        //	return GetInstance(textReader, mixture, 0);
        //}


        public void Add(string caseId, AASeq aaSeq)
        {
            Helper.CheckCondition(!_caseIdToAASeq.ContainsKey(caseId), string.Format("caseId {0} appears more than once", caseId));
            //!!!1205x
            if (null == SequenceLengthOrNull)
            {
                SequenceLengthOrNull = aaSeq.Count;
            }
            if (SequenceLengthOrNull != aaSeq.Count)
            {
                Console.Error.WriteLine("Warning: Not all amino acid sequences are of the same length");
            }
            _caseIdToAASeq.Add(caseId, aaSeq);
        }

        //private static char AnyAminoAcid = '?';

        //public static string AA1PosToAtAA1(int aa1Pos)
        //{
        //}

        public static string SparseHeader = "var\tcid\tval";
        public void CreateSparseStream(TextWriter textWriter, bool keepOneValueVariables)
        {
            textWriter.WriteLine(SparseHeader);
            foreach (string line in SparseLineEnumeration(keepOneValueVariables))
            {
                textWriter.WriteLine(line);
            }
        }

        public IEnumerable<string> SparseLineEnumeration(bool keepOneValueVariables, int? merLengthOrNull)
        {
            if (null == merLengthOrNull || (int)merLengthOrNull == 1)
            {
                return SparseLineEnumeration(keepOneValueVariables);
            }
            else
            {
                return SparseLineMerEnumeration(keepOneValueVariables, (int)merLengthOrNull);
            }
        }
        public IEnumerable<string> SparseLineEnumeration(bool keepOneValueVariables)
        {
            if (_caseIdToAASeq.Count == 0)
            {
                Debug.Assert(SequenceLengthOrNull == null); // real assert
                yield break;
            }
            Helper.CheckCondition(SequenceLengthOrNull != null, "This converter to sparse assumes all sequences have the same length");

            /*
            n1pos	aa	pid	val
            880	A	3	F
            880	A	5	F
            880	A	9	F
            880	A	13	F
            880	A	14	F
            880	A	15	T
            ...
                */


            for (int aa0Pos = 0; aa0Pos < (int)SequenceLengthOrNull; ++aa0Pos)
            {
                Set<char> everyAminoAcid = EveryAminoAcid(aa0Pos);
                if (!keepOneValueVariables && everyAminoAcid.Count == 1)
                {
                    continue;
                }

                string posName = null;
                foreach (char aa in everyAminoAcid)
                {
                    Set<bool> valueSet = Set<bool>.GetInstance();
                    Dictionary<string, bool> caseToVal = new Dictionary<string, bool>();
                    foreach (string caseId in _caseIdToAASeq.Keys)
                    {
                        AASeq aaSeq = _caseIdToAASeq[caseId];

                        if (aa0Pos >= aaSeq.Count) continue;

                        //Helper.CheckCondition(aaSeq.IsUsingOriginalPositions(), "This converter to sparse assumes all sequences are using their original positions");
                        Set<char> strainAASet = aaSeq[aa0Pos];
                        if (posName == null)
                        {
                            posName = aaSeq.OriginalAA1Position(aa0Pos);
                            //if (posName.Contains("68.3B"))
                            //{
                            //    Console.WriteLine("Found it first");
                            //}
                        }
                        else
                        {
                            Helper.CheckCondition(posName == aaSeq.OriginalAA1Position(aa0Pos));
                        }
                        // missing: e.g.  A/Any   or   A/AB
                        // 1: e.g. A/A
                        // 0: e.g. A/B	or  A/BCD
                        if (strainAASet.Equals(AASeq.Any))
                        {
                            //Do nothing - missing
                        }
                        else if (strainAASet.Contains(aa))
                        {
                            if (strainAASet.Count > 1)
                            {
                                switch (aaSeq.MixtureSemantics)
                                {
                                    case MixtureSemantics.Pure:
                                        caseToVal.Add(caseId, false);
                                        valueSet.AddNewOrOld(false);
                                        break;
                                    case MixtureSemantics.Uncertainty:
                                        // Do nothing = missing
                                        break;
                                    case MixtureSemantics.Any:
                                        caseToVal.Add(caseId, true);
                                        valueSet.AddNewOrOld(true);
                                        break;
                                    default:
                                        Helper.CheckCondition(false, "Unknown mixturesemantics " + aaSeq.MixtureSemantics.ToString());
                                        break;
                                }
                            }
                            else
                            {
                                caseToVal.Add(caseId, true);
                                valueSet.AddNewOrOld(true);
                            }
                        }
                        else
                        {
                            caseToVal.Add(caseId, false);
                            valueSet.AddNewOrOld(false);
                        }
                    }
                    Helper.CheckCondition(posName != null);
                    if (keepOneValueVariables || valueSet.Count == 2)
                    {
                        foreach (KeyValuePair<string, bool> caseIdAndVal in caseToVal)
                        {
                            string variableName = string.Format("{0}@{1}", posName, aa);
                            //string variableName = string.Format("{1}@{0}", posName, aa);
                            //if (variableName.Contains("68.3B"))
                            //{
                            //    Console.WriteLine("Found it first");
                            //}
                            yield return Helper.CreateTabString(
                                variableName, caseIdAndVal.Key, caseIdAndVal.Value ? 1 : 0);
                        }
                    }
                }
            }
        }


        public IEnumerable<string> SparseLineMerEnumeration(bool keepOneValueVariables, int merLength)
        {
            if (_caseIdToAASeq.Count == 0)
            {
                Debug.Assert(SequenceLengthOrNull == null); // real assert
                yield break;
            }
            Helper.CheckCondition(SequenceLengthOrNull != null, "This converter to sparse assumes all sequences have the same length");

            Dictionary<string, AASeq> caseToCompressedAASeq = RemoveDeletesAndStopsFromData(false, Console.Error);
            foreach (string mer in EveryUnambiguousStopFreeMer(merLength, caseToCompressedAASeq))
            {
                Regex merAsRegex = AASeq.CreateMerRegex(mer); //!!!look for similar code elsewhere

                foreach (string protein in EveryProtein())
                {
                    Set<bool> valueSet = Set<bool>.GetInstance();
                    Dictionary<string, bool> caseToVal = new Dictionary<string, bool>();
                    foreach (string caseId in caseToCompressedAASeq.Keys)
                    {
                        AASeq aaSeq = caseToCompressedAASeq[caseId];
                        Helper.CheckCondition(aaSeq.MixtureSemantics == MixtureSemantics.Uncertainty, "Code does not expect Mixture semantics");
                        bool? containsOrNull = aaSeq.ContainsMer(mer, merAsRegex, protein);
                        if (null == containsOrNull)
                        {
                            continue;
                        }
                        else if ((bool)containsOrNull)
                        {
                            caseToVal.Add(caseId, true);
                            valueSet.AddNewOrOld(true);
                        }
                        else
                        {
                            caseToVal.Add(caseId, false);
                            valueSet.AddNewOrOld(false);
                        }
                    }
                    if (keepOneValueVariables || valueSet.Count == 2)
                    {
                        foreach (KeyValuePair<string, bool> caseIdAndVal in caseToVal)
                        {
                            string variableName = protein + "@" + mer;
                            yield return Helper.CreateTabString(
                                variableName, caseIdAndVal.Key, caseIdAndVal.Value ? 1 : 0);
                        }
                    }
                }
            }
        }

        private IEnumerable<string> EveryProtein()
        {
            AASeq firstAASeq = GetFirstAASeq();
            return firstAASeq.EveryProtein();
        }

        private AASeq GetFirstAASeq()
        {
            AASeq firstAASeq = this._caseIdToAASeq.Values.First();
            return firstAASeq;
        }

        public Set<char> EveryAminoAcid(int aa0Pos)
        {
            Set<char> every = Set<char>.GetInstance();
            foreach (AASeq aaSequence in _caseIdToAASeq.Values)
            {
                if (aa0Pos < aaSequence.Count)
                {
                    every.AddNewOrOldRange(aaSequence[aa0Pos]);
                }
            }
            every.RemoveIfPresent('?');
            return every;
        }

        public void CreateAASeqStream(TextWriter textWriter)
        {

            //	/*
            //	caseId	aaSeq
            //	1189MB	MEPVDPNLEPWNHPGSQPKTPCTNCYCKHCSYHCLVCFQTKGLGISYGRK
            //	J112MA	MEPVDPNLEPWNHPGSQPITACNKCYCKYCSYHCLVCFQTKGLGISYGRK
            //	1157M3M   MEPVDPNLEPWNHPGSQPKTPCNKCYCKHCSYHCLVCFQTKGLGISYGRK
            //	1195MB	MEPVDPNLEPWNHPGSQPKTPCNKCYCKYCSYHCLVCFQTKGLGISYGRK
            //	 */

            textWriter.WriteLine("cid\taaSeq");
            foreach (KeyValuePair<string, AASeq> caseIdAndAASeq in _caseIdToAASeq)
            {
                textWriter.WriteLine("{0}\t{1}", caseIdAndAASeq.Key, caseIdAndAASeq.Value);
            }
        }

        public void CreateAASeqFile(string outputFileName)
        {
            using (TextWriter textWriter = File.CreateText(outputFileName))
            {
                CreateAASeqStream(textWriter);
            }
        }

        public void CreateMerSparseStream(TextWriter textWriter, int merLength, bool keepOneValueVariables)
        {
            Dictionary<string, AASeq> caseToCompressedAASeq = RemoveDeletesAndStopsFromData(true, textWriter);
            Dictionary<string, string> merStringToBestOriginalAA0Position = FindMerStringToBestOriginalAA0Position(merLength, textWriter, caseToCompressedAASeq);
            Debug.Assert(Set<string>.GetInstance(merStringToBestOriginalAA0Position.Keys).Equals(
                Set<string>.GetInstance(EveryUnambiguousStopFreeMer(merLength, caseToCompressedAASeq))));


            textWriter.WriteLine("var\tcid\tval");

            foreach (string mer in merStringToBestOriginalAA0Position.Keys)
            {
                Dictionary<bool, int> valueToNonZeroCount;
                Dictionary<string, bool> caseIdToValue = FindMerValues(mer, caseToCompressedAASeq, out valueToNonZeroCount);
                Debug.Assert(valueToNonZeroCount.Count != 0);

                if (valueToNonZeroCount.Count == 1 && !keepOneValueVariables)
                {
                    continue;
                }

                foreach (string caseId in caseIdToValue.Keys)
                {
                    int val = (caseIdToValue[caseId]) ? 1 : 0;
                    string variableName = string.Format("{0}@{1}", mer, merStringToBestOriginalAA0Position[mer]);
                    textWriter.WriteLine(Helper.CreateTabString(variableName, caseId, val));
                }
            }

        }

        private Dictionary<string, bool> FindMerValues(string merAsString, Dictionary<string, AASeq> caseToCompressedAASeq, out Dictionary<bool, int> valueToNonZeroCount)
        {
            Regex merAsRegex = AASeq.CreateMerRegex(merAsString);

            Dictionary<string, bool> merValues = new Dictionary<string, bool>();
            valueToNonZeroCount = new Dictionary<bool, int>();
            foreach (KeyValuePair<string, AASeq> caseIdAndCompressedAASeq in caseToCompressedAASeq)
            {
                string caseId = caseIdAndCompressedAASeq.Key;
                AASeq compressedAASeq = caseIdAndCompressedAASeq.Value;

                bool? containsMer = compressedAASeq.ContainsMer(merAsString, merAsRegex);

                if (null != containsMer)
                {
                    merValues.Add(caseId, (bool)containsMer);
                    valueToNonZeroCount[(bool)containsMer] = 1 + valueToNonZeroCount.GetValueOrDefault((bool)containsMer);
                }

            }
            return merValues;
        }


        //internal void CreateSparseStream(TextWriter textWriter, bool keepOneValueVariables)
        //{
        //	CreateSparseStream(textWriter, keepOneValueVariables);
        //}

        public void CreateSparseFile(string outputFileName, bool keepOneValueVariables)
        {
            using (TextWriter textWriter = File.CreateText(outputFileName))
            {
                CreateSparseStream(textWriter, keepOneValueVariables);
            }
        }

        //public void CreateSparseFile(string outputFileName, bool keepOneValueVariables)
        //{
        //	CreateSparseFile(outputFileName, keepOneValueVariables);
        //}

        private Dictionary<string, AASeq> RemoveDeletesAndStopsFromData(bool stopOnStop, TextWriter textWriter)
        {
            Dictionary<string, AASeq> compressedDictionary = new Dictionary<string, AASeq>();
            foreach (KeyValuePair<string, AASeq> caseIdAndAASeq in _caseIdToAASeq)
            {
                AASeq compressedAASeq = AASeq.GetCompressedInstance(caseIdAndAASeq.Key, caseIdAndAASeq.Value, stopOnStop, textWriter);
                compressedDictionary.Add(caseIdAndAASeq.Key, compressedAASeq);
            }
            return compressedDictionary;
        }

        private IEnumerable<string> EveryUnambiguousStopFreeMer(int merLength, Dictionary<string, AASeq> caseToCompressedAASeq)
        {
            Set<string> nonMissingMerSet = Set<string>.GetInstance();
            foreach (AASeq aaSeq in caseToCompressedAASeq.Values)
            {
                foreach (AASeq mer in aaSeq.SubSeqEnumeration(merLength))
                {
                    if (mer.Ambiguous)
                    {
                        continue;
                    }
                    string merAsString = mer.ToString();
                    if (merAsString.Contains("*"))
                    {
                        continue;
                    }

                    Debug.Assert(!merAsString.Contains("-") && !merAsString.Contains("?")); // real assert
                    if (nonMissingMerSet.Contains(merAsString))
                    {
                        continue;
                    }
                    nonMissingMerSet.AddNew(merAsString);
                    yield return merAsString;
                }
            }
        }


        public void CreateMainKmerPositionsStream(int merLength, TextWriter textWriter)
        {
            Dictionary<string, AASeq> caseToCompressedAASeq = RemoveDeletesAndStopsFromData(true, textWriter);
            Dictionary<string, string> merStringToBestOriginalAA0Position = FindMerStringToBestOriginalAA0Position(merLength, textWriter, caseToCompressedAASeq);

            textWriter.WriteLine(Helper.CreateTabString("kMer", "MostCommonOriginalAA1Position"));
            foreach (string merString in merStringToBestOriginalAA0Position.Keys)
            {
                string bestOriginalAA0Position = merStringToBestOriginalAA0Position[merString];
                textWriter.WriteLine(Helper.CreateTabString(merString, bestOriginalAA0Position));
            }
        }

        private static Dictionary<string, string> FindMerStringToBestOriginalAA0Position(int merLength, TextWriter textWriterForWarnings, Dictionary<string, AASeq> caseToCompressedAASeq)
        {
            Dictionary<string, Dictionary<string, int>> merStringToOriginalAA0PositionToCount = CreateMerStringToOriginalAA0PositionToCount(merLength, textWriterForWarnings, caseToCompressedAASeq);
            Dictionary<string, string> merStringToBestOriginalAA0Position = new Dictionary<string, string>();
            foreach (string merString in merStringToOriginalAA0PositionToCount.Keys)
            {
                Dictionary<string, int> originalAA0PositionToCount = merStringToOriginalAA0PositionToCount[merString];
                MBT.Escience.BestSoFar<int, string> best = FindBest(originalAA0PositionToCount);
                merStringToBestOriginalAA0Position.Add(merString, best.Champ);
            }
            return merStringToBestOriginalAA0Position;
        }

        private static MBT.Escience.BestSoFar<int, string> FindBest(Dictionary<string, int> originalAA0PositionToCount)
        {
            MBT.Escience.BestSoFar<int, string> bestSoFar = MBT.Escience.BestSoFar<int, string>.GetInstance(SpecialFunctions.IntGreaterThan);
            foreach (KeyValuePair<string, int> originalAA0PositionAndCount in originalAA0PositionToCount)
            {
                bestSoFar.Compare(originalAA0PositionAndCount.Value, originalAA0PositionAndCount.Key);
            }
            return bestSoFar;
        }

        private static Dictionary<string, Dictionary<string, int>> CreateMerStringToOriginalAA0PositionToCount(int merLength, TextWriter textWriterForWarnings, Dictionary<string, AASeq> caseToCompressedAASeq)
        {
            Dictionary<string, Dictionary<string, int>> merStringToOriginalAA0PositionToCount = new Dictionary<string, Dictionary<string, int>>();
            foreach (string caseId in caseToCompressedAASeq.Keys)
            {
                AASeq aaSeq = caseToCompressedAASeq[caseId];

                Set<string> SeenIt = new Set<string>();
                foreach (AASeq mer in aaSeq.SubSeqEnumeration(merLength))
                {
                    if (mer.Ambiguous)
                    {
                        continue;
                    }

                    string merString = mer.ToString();
                    if (SeenIt.Contains(merString))
                    {
                        textWriterForWarnings.WriteLine("Warning: Mer '{0}' appears again in case '{1}'", merString, caseId);
                    }
                    SeenIt.AddNewOrOld(merString);

                    string originalAA1Position = mer.OriginalAA1Position(0);

                    Dictionary<string, int> originalAA0PositionToCount = merStringToOriginalAA0PositionToCount.GetValueOrDefault(merString);
                    originalAA0PositionToCount[originalAA1Position] = 1 + originalAA0PositionToCount.GetValueOrDefault(originalAA1Position);
                }
            }
            return merStringToOriginalAA0PositionToCount;
        }



        public void SplitOnProtein(string niceName, string outputDirectory)
        {
            Debug.WriteLine(niceName);
            Dictionary<string, Dictionary<string, string>> proteinToCaseIdToSequence = new Dictionary<string, Dictionary<string, string>>();
            foreach (string caseId in _caseIdToAASeq.Keys)
            {
                AASeq aaSeq = _caseIdToAASeq[caseId];

                string previousProtein = null;
                Dictionary<string, StringBuilder> proteinToSequence = new Dictionary<string, StringBuilder>();
                for (int aa0Pos = 0; aa0Pos < (int)SequenceLengthOrNull; ++aa0Pos)
                {
                    string posName = aaSeq.OriginalAA1Position(aa0Pos);
                    string[] posParts = posName.Split('@');
                    string protein = posParts[0];
                    StringBuilder sequence = proteinToSequence.GetValueOrDefault(protein);
                    if (previousProtein != protein)
                    {
                        Helper.CheckCondition(sequence.Length == 0, "Expect proteins to be contintiguous");
                        previousProtein = protein;
                    }

                    Set<char> strainAASet = aaSeq[aa0Pos];
                    sequence.Append(AASeq.AaAsString(strainAASet));
                }

                foreach (string protein in proteinToSequence.Keys)
                {
                    Dictionary<string, string> caseIdToSequence = proteinToCaseIdToSequence.GetValueOrDefault(protein);
                    caseIdToSequence.Add(caseId, proteinToSequence[protein].ToString());
                }
            }

            foreach (string protein in proteinToCaseIdToSequence.Keys)
            {
                string outputFileName = string.Format(@"{0}\{1}.{2}.aaSeq.txt", outputDirectory, protein, niceName);
                using (TextWriter textWriter = File.CreateText(outputFileName))
                {
                    textWriter.WriteLine("cid\taaSeq"); //!!!const
                    Dictionary<string, string> caseIdToSequence = proteinToCaseIdToSequence[protein];
                    foreach (string caseId in caseIdToSequence.Keys)
                    {
                        textWriter.WriteLine(Helper.CreateTabString(caseId, caseIdToSequence[caseId]));
                    }
                }
            }

        }


        public void TriplesAppend(ref Set<string> seenTriple, ref Dictionary<string, List<string>> proteinToTripleList)
        {
            AASeq aaSeq = GetFirstAASeq();
            for (int aa0Pos = 0; aa0Pos < (int)SequenceLengthOrNull; ++aa0Pos)
            {
                string posName = aaSeq.OriginalAA1Position(aa0Pos);
                string[] posParts = posName.Split('@');
                string protein = posParts[0];
                string hxb2Pos = posParts[2];

                string triple = Helper.CreateTabString(protein, hxb2Pos);
                if (!seenTriple.Contains(triple))
                {
                    seenTriple.AddNew(triple);
                    List<string> tripleList = proteinToTripleList.GetValueOrDefault(protein);
                    tripleList.Add(triple);
                }
            }
        }

        public static void TriplesReport(Dictionary<string, List<string>> proteinToTripleList)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public static void TriplesReport(Dictionary<string, List<string>> proteinToTripleList, string outputDirectoryName)
        {
            string outputFileName = string.Format(@"{0}\triples.txt", outputDirectoryName);
            using (TextWriter textWriter = File.CreateText(outputFileName))
            {
                textWriter.WriteLine(Helper.CreateTabString("protein", "HXB2Position", "linearPosition"));
                foreach (string protein in proteinToTripleList.Keys)
                {
                    List<string> tripleList = proteinToTripleList[protein];
                    for (int i = 0; i < tripleList.Count; ++i)
                    {
                        textWriter.WriteLine(Helper.CreateTabString(tripleList[i], i + 1));
                    }
                }
            }
        }

    }

    /// <summary>
    ///	<br>none (the old Mixture==false), if a position contains {ab}, consider it missing with regards to a and b, but say it doesn't have c.</br>
    ///	<br>pure (the old Mixture==true), if a position contains {ab} say that it has neither.</br>
    ///	<br>any  (a new possibility), if a position contains {ab}, say that it has both.</br>
    /// </summary>
    public enum MixtureSemantics
    {
        /// <summary>
        /// if a position contains {ab}, say ? for a and ? for b and 0 for c.
        /// </summary>
        Uncertainty,
        /// <summary>
        /// if a position contains {ab} say that it has neither.
        /// </summary>
        Pure,
        /// <summary>
        ///  if a position contains {ab}, say that it has both.
        /// </summary>
        Any,
        /// <summary>
        /// Will encode the distribution, with equal weight over the possibilities
        /// </summary>
        Distribution
    }
}
