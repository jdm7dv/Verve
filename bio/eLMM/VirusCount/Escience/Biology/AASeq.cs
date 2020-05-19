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
    /// <summary>
    /// Warning: Hashcode and Equals ignores (by design) the information about original aa sequence.
    /// </summary>
    public class AASeq : IEnumerable<KeyValuePair<int,Set<char>>>
    {
        protected AASeq(MixtureSemantics mixtureSemantics)
        {
            MixtureSemantics = mixtureSemantics;
        }

        protected AASeq(string aaSeqAsString, MixtureSemantics mixtureSemantics, int offset)
            : this(mixtureSemantics)
        {
            Sequence = CreateCharSetList(aaSeqAsString);
            Offset = offset;
        }

        public MixtureSemantics MixtureSemantics;
        protected List<Set<char>> Sequence;
        private List<string> _originalAA1PositionTableOrNull = null;
        private int Offset = 0;

        public string OriginalAA1Position(int currentAA0Position)
        {
            if (_originalAA1PositionTableOrNull == null)
            {
                return (currentAA0Position + 1 + Offset).ToString();
            }
            else
            {
                Debug.Assert(Offset == 0);
                return _originalAA1PositionTableOrNull[currentAA0Position];
            }
        }

        protected string SeqAsString = null;

        public Set<char> this[int zeroBasedIndex]
        {
            get
            {
                return Sequence[zeroBasedIndex];
            }
        }

        public static Set<char> Delete = Set<char>.GetInstance('-');
        public static Set<char> Stop = Set<char>.GetInstance('*');
        public static Set<char> Any = Set<char>.GetInstance('?');

        static public AASeq GetCompressedInstance(string caseId, AASeq aaSeqIn, bool stopOnStop, TextWriter errorStream)
        {
            AASeq aaSeqOut = new AASeq(aaSeqIn.MixtureSemantics);
            aaSeqOut.Sequence = new List<Set<char>>();
            aaSeqOut._originalAA1PositionTableOrNull = new List<string>();
            Debug.Assert(aaSeqOut.Offset == 0); // real assert


            for (int iChar = 0; iChar < aaSeqIn.Count; ++iChar)
            {
                Set<char> set = aaSeqIn[iChar];
                string originalAA1Position = aaSeqIn.OriginalAA1Position(iChar);
                if (set.Equals(Delete)) //!!!const
                {
                    continue;
                }
                if (set.Equals(Stop)) //!!!const
                {
                    if (iChar != aaSeqIn.Count - 1)
                    {
                        errorStream.WriteLine("Warning: The sequence for case id '{0}' contains a '*' before the last position", caseId);
                        if (stopOnStop)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                aaSeqOut.Sequence.Add(set);
                aaSeqOut._originalAA1PositionTableOrNull.Add(originalAA1Position);
            }
            return aaSeqOut;
        }

        static public AASeq GetInstance(string aaSeqAsString, MixtureSemantics mixtureSemantics)
        {
            return GetInstance(aaSeqAsString, mixtureSemantics, 0);
        }

        static public AASeq GetInstance(string aaSeqAsString, MixtureSemantics mixtureSemantics, int offset)
        {
            AASeq aaSeq = new AASeq(aaSeqAsString, mixtureSemantics, offset);
            //aaSeq.Sequence = aaSeq.CreateCharSetList(aaSeqAsString);
            //aaSeq.Offset = offset;
            return aaSeq;
        }

        static public AASeq GetInstance(string aaSeqAsString, List<string> originalAA1PositionTable, MixtureSemantics mixtureSemantics)
        {
            Helper.CheckCondition(null != originalAA1PositionTable, "must give a position table");
            AASeq aaSeq = new AASeq(mixtureSemantics);
            aaSeq.Sequence = aaSeq.CreateCharSetList(aaSeqAsString);
            Helper.CheckCondition(aaSeq.Count == originalAA1PositionTable.Count, "aaSeq and position table must be same length");
            aaSeq._originalAA1PositionTableOrNull = originalAA1PositionTable;
            return aaSeq;
        }



        public List<Set<char>> CreateCharSetList(string aaSeqAsString)
        {

            List<Set<char>> sequence = new List<Set<char>>();

            Set<char> set = null;

            foreach (char ch in aaSeqAsString)
            {
                switch (ch)
                {
                    case '{':
                        {
                            Helper.CheckCondition(set == null, "Nested '{''s are not allowed in aaSeq strings");
                            set = new Set<char>();
                            sequence.Add(set);
                            break;
                        }
                    case '}':
                        {
                            Helper.CheckCondition(set != null, "'}' must follow a '{' in aaSeq strings");
                            Helper.CheckCondition(set.Count > 0, "Empty sets not allow in aaSeq strings");
                            set = null;
                            break;
                        }
                    case ' ':
                        {
                            Helper.CheckCondition(false, "Sequences should not contain blanks. Use '?' for missing.");
                            break;
                        }
                    case '?':
                    case '-':
                        {
                            Helper.CheckCondition(set == null, string.Format("'{0}' must not appear in sets", ch));
                            sequence.Add(Set<char>.GetInstance(ch));
                            break;
                        }
                    default:
                        {
                            Set<char> charAsSet = GetResidueSetFromCharAndCheckThatValid(ch);

                            if (set == null)
                            {
                                sequence.Add(charAsSet);
                            }
                            else
                            {
                                set.AddNew(ch);
                            }
                            break;
                        }
                }
            }
            Helper.CheckCondition(set == null, "Missing '}' in aaSeq string");
            return sequence;

        }

        protected virtual Set<char> GetResidueSetFromCharAndCheckThatValid(char ch)
        {
            ch = char.ToUpper(ch);
            //!!!move this to Biology?
            Helper.CheckCondition(Biology.GetInstance().OneLetterAminoAcidAbbrevTo3Letter.ContainsKey(ch),
                string.Format("The character {0} is not an amino acid", ch));
            string aminoAcid = Biology.GetInstance().OneLetterAminoAcidAbbrevTo3Letter[ch];
            Helper.CheckCondition(Biology.GetInstance().KnownAminoAcid(aminoAcid),
                string.Format("The character {0} is not a standard amino acid", ch));
            Set<char> charAsSet = Set<char>.GetInstance(ch);

            return charAsSet;
        }

        public string AaAsString(int pos0)
        {
            return AaAsString(this[pos0]);
        }

        public static string AaAsString(Set<char> aa)
        {
            StringBuilder sb = new StringBuilder();
            if (aa.Count != 1)
            {
                sb.Append('{');
            }

            foreach (char ch in aa)
            {
                sb.Append(ch);
            }

            if (aa.Count != 1)
            {
                sb.Append('}');
            }
            return sb.ToString();
        }

        private static string SequenceToString(List<Set<char>> sequence)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Set<char> set in sequence)
            {
                sb.Append(AaAsString(set));
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            if (SeqAsString == null)
            {
                SeqAsString = SequenceToString(Sequence);
            }
            return SeqAsString;
        }

        public override int GetHashCode()
        {
            int hc = ToString().GetHashCode() ^ "AASeq".GetHashCode();
            return hc;
        }

        public override bool Equals(object obj)
        {
            AASeq other = obj as AASeq;
            if (other == null)
            {
                return false;
            }
            else
            {
                return ToString() == other.ToString();
            }
        }

        public int Count
        {
            get
            {
                return Sequence.Count;
            }
        }

        public IEnumerable<AASeq> SubSeqEnumeration(int merLength)
        {
            for (int startIndex = 0; startIndex <= Sequence.Count - merLength; ++startIndex)
            {
                AASeq aaSeqOut = SubSeqAA0Pos(startIndex, merLength);
                yield return aaSeqOut;
            }
        }


        public AASeq SubSeqAA1Pos(int aa1Pos, int merLength)
        {
            return SubSeqAA0Pos(aa1Pos - 1, merLength);
        }
        public bool TrySubSeqAA1Pos(int aa1Pos, int merLength, out AASeq aaSeq)
        {
            return TrySubSeqAA0Pos(aa1Pos - 1, merLength, out aaSeq);
        }

        public bool TrySubSeqAA0Pos(int aa0Pos, int merLength, out AASeq aaSeq)
        {
            if (aa0Pos < 0 || aa0Pos + merLength > this.Sequence.Count)
            {
                aaSeq = null;
                return false;
            }
            aaSeq = SubSeqAA0Pos(aa0Pos, merLength);
            return true;
        }

        public AASeq SubSeqAA0Pos(int aa0Pos, int merLength)
        {
            return SubSeqAA0Pos(aa0Pos, merLength, false);
        }

        public AASeq SubSeqAA0Pos(int aa0Pos, int merLength, bool skipDelete)
        {
            List<Set<char>> subSequence = new List<Set<char>>();
            AASeq aaSeqOut = new AASeq(MixtureSemantics);
            aaSeqOut.Sequence = subSequence;
            aaSeqOut._originalAA1PositionTableOrNull = new List<string>();
            Debug.Assert(aaSeqOut.Offset == 0); // real assert
            for (int aa0 = aa0Pos; aa0 < aa0Pos + merLength; ++aa0)
            {
                Set<char> charSet = Sequence[aa0];
                if (!skipDelete || !AASeq.Delete.Equals(charSet))
                {
                    string originalAA1Position = OriginalAA1Position(aa0);
                    aaSeqOut._originalAA1PositionTableOrNull.Add(originalAA1Position);
                    subSequence.Add(charSet);
                }
            }
            return aaSeqOut;
        }


        private bool? _ambiguous = null;

        public bool Ambiguous
        {
            get
            {
                if (_ambiguous == null)
                {
                    SetAmbiguous();
                }
                return (bool)_ambiguous;
            }
        }

        private void SetAmbiguous()
        {
            _ambiguous = false;
            foreach (Set<char> set in Sequence)
            {
                if (set.Count > 1 || set.Equals(Any))
                {
                    _ambiguous = true;
                    break;
                }
            }
        }

        internal bool? ContainsMer(string merAsString, Regex merAsRegex)
        {
            return ContainsMerInternal(merAsString, merAsRegex, ToString());
        }

        internal bool? ContainsMer(string merAsString, Regex merAsRegex, string proteinGoal)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Count; ++i)
            {
                string pos = _originalAA1PositionTableOrNull[i];
                string proteinActual = pos.Split('@')[0];
                if (proteinGoal == proteinActual)
                {
                    Set<char> set = Sequence[i];
                    sb.Append(AaAsString(set));
                }
            }

            return ContainsMerInternal(merAsString, merAsRegex, sb.ToString());
        }

        static internal bool? ContainsMerInternal(string merAsString, Regex merAsRegex, string aaSeqAsString)
        {
            // e.g. As strings:  "AB{CD}E" contains "AB", but not "BC"
            if (aaSeqAsString.Contains(merAsString))
            {
                return true;
            }

            // e.g. as pattern:  "AB{CD}E" does not contain "BF", but might contain BC
            // e.g. as pattern:  "AB?E" does not contain "BFG", but might contain "BFE"

            else if (merAsRegex.IsMatch(aaSeqAsString))
            {
                return null;
            }
            else
            {
                return false;
            }

        }


        static public Regex CreateMerRegex(string mer)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char aaChar in mer)
            {
                sb.AppendFormat(@"([{0}?]|({{[^}}]*{0}[^}}]*}}))", aaChar);
            }
            return new Regex(sb.ToString());
        }


        //public bool IsUsingOriginalPositions()
        //{
        //    return _originalAA0PositionTableOrNull == null;
        //}

        internal IEnumerable<string> EveryProtein()
        {
            Set<string> proteinSet = Set<string>.GetInstance();
            foreach (string pos in _originalAA1PositionTableOrNull)
            {
                string protein = pos.Split('@')[0];
                proteinSet.AddNewOrOld(protein);
            }
            return proteinSet;
        }


        /// <summary>
        /// Skips over deletes. Is allowed to be short if near end of larger sequence
        /// </summary>
        public AASeq SubSeqCenteredAA1Pos(int aa1Pos, int merLength, out Set<char> centerAA)
        {
            Helper.CheckCondition(merLength > 0, "merLength must be greater than 0");

            int? positionToStartBase1OrNull = FindStartPositionBase1OrNull(aa1Pos);
            if (positionToStartBase1OrNull == null)
            {
                centerAA = AASeq.Delete;
                return SubSeqAA0Pos(0, 0, false);
            }
            int positionToStartBase0 = positionToStartBase1OrNull.Value - 1;
            centerAA = Sequence[positionToStartBase0];

            int goalLength = (merLength - 1) / 2;

            int leftIndex = FindLeftPosBase0(positionToStartBase0, goalLength);
            int rightIndex = FindRightBase0(positionToStartBase0, goalLength);

            AASeq result = SubSeqAA0Pos(leftIndex, rightIndex - leftIndex + 1, true);
            return result;
        }

        private int FindRightBase0(int positionToStartBase0, int goalLength)
        {
            int rightLength = -1;

            int rightIndex = positionToStartBase0;
            while (true)
            {
                if (!Sequence[rightIndex].Equals(AASeq.Delete))
                {
                    ++rightLength;
                }
                if (rightLength >= goalLength || rightIndex >= Count - 1)
                {
                    break;
                }
                ++rightIndex;
            }
            return rightIndex;
        }

        private int FindLeftPosBase0(int positionToStartBase0, int goalLength)
        {
            int leftLength = -1;
            int leftIndex = positionToStartBase0;
            while (true)
            {
                if (!Sequence[leftIndex].Equals(AASeq.Delete))
                {
                    ++leftLength;
                }
                if (leftLength >= goalLength || leftIndex <= 0)
                {
                    break;
                }
                --leftIndex;
            }
            return leftIndex;
        }


        private int? FindStartPositionBase1OrNull(int posBase1)
        {
            foreach (int positionToTry in PositionToTryList(posBase1))
            {
                Set<char> aaSet = this.Sequence[positionToTry - 1];
                if (!aaSet.Equals(AASeq.Delete))
                {
                    return positionToTry;
                }
            }
            return null;

        }

        private IEnumerable<int> PositionToTryList(int posBase1)
        {
            Helper.CheckCondition(1 <= posBase1 && posBase1 <= Count, "Position is not in range. " + posBase1.ToString());

            return SpecialFunctions.EnumerateInterleave(SpecialFunctions.EnumerateRange(posBase1, 1, -1), SpecialFunctions.EnumerateRange(posBase1 + 1, Count, +1));
        }


        public bool Contains(Set<char> set)
        {
            foreach (Set<char> charSet in Sequence)
            {
                if (charSet.Equals(set))
                {
                    return true;
                }
            }
            return false;
        }




        public IEnumerable<AASeq> BlowOut()
        {
            foreach (List<char> possibleSeq in SpecialFunctions.EveryCombination<char>(this.Sequence))
            {
                List<Set<char>> subSequence = new List<Set<char>>();
                AASeq aaSeqOut = new AASeq(MixtureSemantics);
                aaSeqOut.Sequence = subSequence;
                aaSeqOut._originalAA1PositionTableOrNull = new List<string>();
                Debug.Assert(aaSeqOut.Offset == 0); // real assert
                for (int aa0 = 0; aa0 < Count; ++aa0)
                {
                    Set<char> charSet = Set<char>.GetInstance(possibleSeq[aa0]);
                    Helper.CheckCondition(!AASeq.Delete.Equals(charSet), "'delete' not expected");
                    string originalAA1Position = OriginalAA1Position(aa0);
                    aaSeqOut._originalAA1PositionTableOrNull.Add(originalAA1Position);
                    subSequence.Add(charSet);
                }
                yield return aaSeqOut;
            }
        }

        /// <summary>
        /// Counts are sorted in decreasing order.
        /// </summary>
        public static List<KeyValuePair<char, double>> CountAasAtPos(List<AASeq> aaSeqs, int pos0)
        {
            Dictionary<char, double> counts = new Dictionary<char, double>();
            foreach (AASeq seq in aaSeqs)
            {
                var aaSet = seq[pos0];
                foreach (char aa in aaSet)
                {
                    counts[aa] = counts.GetValueOrDefault(aa) + 1.0 / seq[pos0].Count;
                }
            }
            var result = counts.ToList();
            result.Sort((kvp1, kvp2) => kvp2.Value.CompareTo(kvp1.Value));
            return result;
        }

        /// <summary>
        /// Returns the sets for all positions. the Key is the pos1, the value are the observed characters.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<int,Set<char>>> GetEnumerator()
        {
            return Sequence.Select((set,pos) => new KeyValuePair<int,Set<char>>(pos+1,set)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static bool IsMissing(Set<char> set)
        {
            return set.Contains('?');
        }
    }
}
