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
using System.Text;
using MBT.Escience;
using Bio.Util;
using MBT.Escience.Parse;
using MBT.Escience.Biology;

namespace MBT.Escience.Biology
{
    public class NamedSequence
    {
        public string Name;
        public string Protein;

        private string _seq;
        public string Sequence
        {
            get { return _seq; }
            set { _seq = value; _aaSeq = null; }
        }

        private AASeq _aaSeq;
        /// <summary>
        /// Either the DnaSeq or AASeq version of sequence.
        /// </summary>
        public AASeq AASeq
        {
            get
            {
                if (_aaSeq == null)
                {
                    if (IsDna())
                        _aaSeq = DnaSeq.GetInstance(Sequence, MixtureSemantics.Uncertainty);
                    else
                        _aaSeq = AASeq.GetInstance(Sequence, MixtureSemantics.Uncertainty);
                }
                return _aaSeq;
            }
        }

        public static readonly IComparer<NamedSequence> NameComparerIgnoreCase =
            new ComparisonWrapper<NamedSequence>(delegate(NamedSequence s1, NamedSequence s2)
            {
                return s1.Name.ToLower().CompareTo(s2.Name.ToLower());
            });

        public static readonly IComparer<NamedSequence> SeqComparerIgnoreCase =
            new ComparisonWrapper<NamedSequence>(delegate(NamedSequence s1, NamedSequence s2)
            {
                return s1.Sequence.ToLower().CompareTo(s2.Sequence.ToLower());
            });

        private NamedSequence() { }

        public NamedSequence(string name, string sequence) : this(name, "", sequence) { }
        public NamedSequence(string name, string protein, string sequence)
        {
            Name = name;
            Protein = protein;
            Sequence = sequence;
        }

        public static NamedSequence Parse(string constructorArgs)
        {
            ConstructorArguments constArgs = new ConstructorArguments("(" + constructorArgs + ")");
            NamedSequence result = new NamedSequence();
            result.Protein = constArgs.ExtractOptional<string>("Protein", null);
            result.Name = constArgs.ExtractNext<string>("name");
            return result;
        }

        public string ToParsableNameString()
        {
            return Name + (string.IsNullOrEmpty(Protein) ? "" : ",Protein:" + Protein);
        }


        public NamedSequence Translate(bool dashAsMissing)
        {
            string aaSeq = Translate(Sequence, dashAsMissing);
            NamedSequence namedAaSeq = new NamedSequence(Name, Protein, aaSeq);
            return namedAaSeq;
        }

        public static string Translate(string seq, bool dashAsMissing)
        {
            StringBuilder sb = new StringBuilder();
            //Helper.CheckCondition(seq.Length % 3 == 0, "Nucleotide sequence must be divisible by three to convert to amino acid sequence. Length " + seq.Length + " is not divisible by 3.");

            for (int iNuc0 = 0; iNuc0 + 2 < seq.Length; iNuc0 = iNuc0 + 3)
            {
                string sCodon = seq.Substring(iNuc0, 3);
                String aaCharSetString = CodonToAACharSetString(sCodon, dashAsMissing);
                if (aaCharSetString.Length == 1)
                {
                    sb.Append(aaCharSetString);
                }
                else
                {
                    sb.AppendFormat("{{{0}}}", aaCharSetString);
                }
            }

            return sb.ToString();
            //NamedSequence aaSeq = new NamedSequence(Name, sb.ToString());
            //return aaSeq;
        }

        static public string CodonToAACharSetString(string sCodon, bool dashAsMissing)
        {
            sCodon = sCodon.ToUpper();
            SimpleAminoAcidSet rgAminoAcid = Biology.GetInstance().GenticCode(22, sCodon, dashAsMissing);
            if (rgAminoAcid == null)
            {
                return "?";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (string aminoAcid in rgAminoAcid.Positives)
                {
                    char aaChar = Biology.GetInstance().ThreeLetterAminoAcidAbbrevTo1Letter[aminoAcid];
                    sb.Append(aaChar);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Returns this NamedSequence with the sequence reversed (no complement). AA mixtures are handled correctly.
        /// </summary>
        /// <returns></returns>
        public NamedSequence ReverseSequence()
        {
            var newSeq = Sequence.Reverse().ToCharArray();
            for (int i = 0; i < newSeq.Length; i++)
            {
                if (newSeq[i] == '{')
                    newSeq[i] = '}';
                else if (newSeq[i] == '}')
                    newSeq[i] = '{';
            }
            return new NamedSequence(Name, Protein, new string(newSeq));
        }

        /// <summary>
        /// Returns the reverse complement. Throws an exception if IsDna() is false. 
        /// </summary>
        /// <exception cref="Exception">If IsDna() is false.</exception>
        /// <returns>The reverse complement</returns>
        public NamedSequence ReverseCompelement()
        {
            Helper.CheckCondition(IsDna(), "Cannot take the reverse complement of a sequence that is not DNA.");
            Biology Biology = Biology.GetInstance();
            StringBuilder revComp = new StringBuilder(Sequence.Length);
            for (int i = Sequence.Length - 1; i >= 0; i--)
            {
                Helper.CheckCondition(Biology.DnaComplementarityCode.ContainsKey(Sequence[i]), "Cannot compute complement for " + Sequence[i]);
                char comp = Biology.DnaComplementarityCode[Sequence[i]];
                revComp.Append(comp);
            }
            return new NamedSequence(Name, Protein, revComp.ToString());
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            NamedSequence other = obj as NamedSequence;
            if (other == null)
            {
                return false;
            }

            return other.Name == this.Name && other.Sequence == this.Sequence;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Sequence.GetHashCode();
        }

        public bool IsDna()
        {
            return IsDna(Sequence);
        }

        public static bool IsDna(string sequence)
        {
            foreach (char c in sequence)
            {
                switch (char.ToUpper(c))
                {
                    case 'L':
                    case 'I':
                    case 'F':
                    case 'P':
                    case 'Q':
                    case 'E':
                        return false;
                    default:
                        break;
                }
            }
            return true;
        }


    }
}
