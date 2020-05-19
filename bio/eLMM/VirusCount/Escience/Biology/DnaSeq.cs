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
using System.Linq;
using System.Text;
using MBT.Escience;
using Bio.Util;

namespace  MBT.Escience.Biology
{
    public class DnaSeq : AASeq
    {
        protected DnaSeq(string seqAsString, MixtureSemantics semantics, int offset)
            :
            base(seqAsString, semantics, offset) { }

        protected override MBT.Escience.Set<char> GetResidueSetFromCharAndCheckThatValid(char ch)
        {
            char chUpper = char.ToUpper(ch);
            Biology biology = Biology.GetInstance();
            Set<char> result;

            if (biology.Unambiguous1LetterNucCodes.Contains(chUpper))
            {
                result = new Set<char>(chUpper);
            }
            else if (biology.Ambiguous1LetterNucCodeToChoices.ContainsKey(chUpper))
            {
                string basesAsString = biology.Ambiguous1LetterNucCodeToChoices[chUpper];
                result = new Set<char>();
                foreach (char c in basesAsString)
                {
                    result.AddNew(c);
                }
            }
            else
            {
                throw new ArgumentException("Do not know dna base " + chUpper);
            }

            return result;
        }

        public override string ToString()
        {
            if (SeqAsString == null)
            {
                StringBuilder sb = new StringBuilder(Count);
                foreach (Set<char> charAsSet in Sequence)
                {
                    sb.Append(DnaSetAsChar(charAsSet));
                }
                SeqAsString = sb.ToString();
            }
            return SeqAsString;
        }

        new static public AASeq GetInstance(string dnaSeqAsString, MixtureSemantics mixtureSemantics)
        {
            return GetInstance(dnaSeqAsString, mixtureSemantics, 0);
        }

        new static public AASeq GetInstance(string dnaSeqAsString, MixtureSemantics mixtureSemantics, int offset)
        {
            DnaSeq dnaSeq = new DnaSeq(dnaSeqAsString, mixtureSemantics, offset);
            return dnaSeq;
        }

        public static char DnaSetAsChar(Set<char> dnas)
        {
            //if (dnas.Count == 1 && dnas.First() == '-')
            //{
            //    return '-';
            //}
            if (dnas.Count == 1)
                return dnas.First();

            return Biology.GetInstance().NucSetToAmbiguous1LetterNucCode[HashableSet<char>.GetInstance(dnas)];
        }
    }
}
