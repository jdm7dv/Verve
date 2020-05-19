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
using Bio.Util;
using MBT.Escience.Parse;
using MBT.Escience;
using System.Text.RegularExpressions;

namespace MBT.Escience.Biology.Hla
{

    public enum HlaILocus { A = 1, B, C };
    public enum HlaIResolution { Group = 1, Protein, Synonymous };
    /// <summary>
    /// A class for parsing and manipulatig HLAs. Knows about 2 and 4 digit differences.
    /// </summary>
    public class HlaI : IComparable<HlaI>
    {
        private static Regex UnambiguousHlaExpr = new Regex(@"^(?<locus>[ABC]*)[w*]*(?<number>[\d]+)[a-z]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex AmbiguousHlaExpr = new Regex(@"^(?<locus>[ABC]*)[w*]*(?<commonNum>[\d]+)\((?<range>[\d,\-]+)\)[a-z]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        //private static Dictionary<string,List<string>> _hlaToSupertype = FileUtils.ReadDi

        HashSet<string> _groups, _proteins, _synChanges;//, _supertypes;

        protected HlaI()
        {
            _groups = new HashSet<string>();
            _proteins = new HashSet<string>();
            _synChanges = new HashSet<string>();
        }

        public HlaI(HlaILocus locus)
            : this()
        {
            Locus = locus;
        }

        public HlaILocus Locus { get; private set; }

        public HlaIResolution MaxResolution
        {
            get { return _synChanges.Count > 0 ? HlaIResolution.Synonymous : _proteins.Count > 0 ? HlaIResolution.Protein : HlaIResolution.Group; }
        }




        /// <summary>
        /// Gets a value indicating if this Hla definition includes multiple HLAs of the current MaxResolution or less.
        /// </summary>
        public bool IsAmbiguous
        {
            get { return !(_groups.Count == 1 && _proteins.Count <= 1 && _synChanges.Count <= 1); }
        }

        public static HlaI Parse(string hlaStr)
        {
            HlaI hla = new HlaI();
            hla.ParseInto(hlaStr);
            return hla;
        }

        public void ParseInto(string hlaStr)
        {
            foreach (string pattern in hlaStr.Split('/'))
                ParseIntoInternal(pattern);
        }

        private void ParseIntoInternal(string hlaStr)
        {
            Match unambMatch = UnambiguousHlaExpr.Match(hlaStr);

            if (unambMatch.Success)
            {
                string locusStr = unambMatch.Groups["locus"].Value;
                ParseLocusAndCheckAgainstExisting(hlaStr, locusStr);

                TryAdd(unambMatch.Groups["number"].Value).Enforce("{0} cannot be parsed. Must have 2, 4 or 6 digits.", hlaStr);
            }
            else
            {
                Match ambMatch = AmbiguousHlaExpr.Match(hlaStr);
                if (ambMatch.Success)
                {
                    string locusStr = ambMatch.Groups["locus"].Value;
                    ParseLocusAndCheckAgainstExisting(hlaStr, locusStr);

                    string commonNum = ambMatch.Groups["commonNum"].Value;
                    string rangeStr = ambMatch.Groups["range"].Value;
                    RangeCollection range = RangeCollection.Parse(rangeStr);

                    foreach (long endingNum in range)
                    {
                        string endingNumStr = endingNum >= 100 ? endingNum.ToString("0000") : endingNum.ToString("00"); // if six digits, then this chunk will be >=100. Else, will be the last 2 digits of a 4 digit type.
                        string typeToAdd = commonNum + endingNumStr;
                        TryAdd(typeToAdd).Enforce("Tried to create an HLA type out of {0} but failed. Must be 2, 4 or 6 digits long.", typeToAdd);
                    }
                }
                else
                {
                    throw new ParseException("Can't parse hla string {0}.", hlaStr);
                }
            }
        }

        private void ParseLocusAndCheckAgainstExisting(string hlaStr, string locusStr)
        {
            if (string.IsNullOrEmpty(locusStr))
                Helper.CheckCondition<ParseException>((int)Locus >= 1, "No locus has been specified. Please specify A, B or C.");
            else
            {
                HlaILocus locus = MBT.Escience.Parse.Parser.Parse<HlaILocus>(locusStr);
                if ((int)Locus == 0)    // Locus hasn't been initialized
                    Locus = locus;
                else
                    Helper.CheckCondition<ParseException>(Locus == locus, "Can't parse {0}. Locus {1} doesn't match existing locus {2}.", hlaStr, locus, Locus);
            }
        }

        /// <summary>
        /// Adds the specific 2, 4 or 6 digit type.
        /// </summary>
        /// <param name="p"></param>
        private bool TryAdd(string hlaNumber)
        {
            if (hlaNumber.Length < 2)
                return false;

            _groups.Add(GetGroupStringFromHigherResolution(hlaNumber));

            int endOfProtein = 4;
            if (hlaNumber.Length % 2 != 0)
                Console.Error.WriteLine("Warning:Hla number {0} has an odd number of digits. Is this a problem?", hlaNumber);

            if (hlaNumber.Length >= 4)
                _proteins.Add(hlaNumber.Substring(0, endOfProtein));

            if (hlaNumber.Length > endOfProtein)
                _synChanges.Add(hlaNumber);

            return true;
        }

        /// <summary>
        /// Returns true if and only if unambiguousHla has the same or lessor resolution as the current HLA, and the max resolution of the 
        /// unambiguousHla exactly matches this HLA at the same resolution. We use uncertainty symantics here. So if the other has higher resolution, 
        /// we return false if the lower resolutions don't match, and null (unknown) if they do match, since we can't evaluate the higher resolution.
        /// Likeliwise, if at any resolution the current Hla has a match, but has additional possibilities, we return null (unknown). If the other's
        /// type at a given resolution is not one of the types in this Hla, we return false.
        /// </summary>
        /// <param name="unambiguousHla">Can only do this with an hla for which IsAmbiguous returns false. Will throw an exception if that's not the case.</param>
        /// <returns></returns>
        public bool? Matches(HlaI unambiguousHla, MixtureSemantics mixtureSemantics = MixtureSemantics.Uncertainty)
        {
            Helper.CheckCondition<ArgumentException>(!unambiguousHla.IsAmbiguous, "Matches(hla) can only be called on with an unambiguous argument.");

            if (unambiguousHla.Locus != this.Locus)
                return false;   // locus don't even match...

            if (!_groups.Contains(unambiguousHla._groups.First()))
                return false;  // two digits don't match, then we're done

            if (_groups.Count > 1 || unambiguousHla._proteins.Count == 1 && this._proteins.Count == 0)
                return ResolveMixture(mixtureSemantics);    // two digits match, but there are more than two of them, so we don't know if this is really the other.
            // or, other has 4 digit types, but we don't. Our two digits match, but we don't know if our 4 digits do.

            if (unambiguousHla._proteins.Count == 0)
                return true;    // we've matched at 2 digit level, and other guy doesn't have 4, so we're done.

            if (!_proteins.Contains(unambiguousHla._proteins.First()))
                return false;   // other has a four digit that we don't have

            if (_proteins.Count > 1 || unambiguousHla._synChanges.Count == 1 && this._synChanges.Count == 0)
                return ResolveMixture(mixtureSemantics);     // 4 digits match, but there are more than two of them, so we don't know if this is really the other.
            // or, other has 6 digit types, but we don't. Our 4 digits match, but we don't know if our 6 digits do.

            if (unambiguousHla._synChanges.Count == 0)
                return true;    // we've matched at 4 digit level, and other guy doesn't have 6, so we're done.

            if (!_synChanges.Contains(unambiguousHla._synChanges.First()))
                return false;   // other has a six digit that we don't have

            if (_synChanges.Count > 1)
                return ResolveMixture(mixtureSemantics);    // 6 digits match, but there are more than two of them, so we don't know if htis is really the other.

            return true;    // wow, we match all the way to 6 digits.
        }

        private bool? ResolveMixture(MixtureSemantics mixtureSemantics)
        {
            switch (mixtureSemantics)
            {
                case MixtureSemantics.Uncertainty:
                    return null;
                case MixtureSemantics.Any:
                    return true;
                case MixtureSemantics.Pure:
                    return false;
                default:
                    throw new NotImplementedException("Can't be here");
            }
        }

        /// <summary>
        /// Clears all resolution details that are higher than the specified resolution.
        /// </summary>
        /// <param name="resolution"></param>
        public void SetMaxResolution(HlaIResolution resolution)
        {
            switch (resolution)
            {
                case HlaIResolution.Synonymous:
                    return;
                case HlaIResolution.Protein:
                    this._synChanges.Clear();
                    return;
                case HlaIResolution.Group:
                    this._synChanges.Clear();
                    this._proteins.Clear();
                    return;
                default:
                    throw new NotImplementedException("Shouldn't be possible.");
            }
        }

        /// <summary>
        /// Returns all the hla types contained in this Hla, separated by /. For example: B5701/B5702/B58.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return EnumerateAll().StringJoin("/");
        }

        /// <summary>
        /// Compares the result of ToString().
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is HlaI && this.Equals(obj.ToString());
        }

        /// <summary>
        /// Gets the hashcode of the ToString representation.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Compares the result of ToString().
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(HlaI other)
        {
            return this.ToString().CompareTo(other.ToString());
        }

        /// <summary>
        /// Enumerates all unique types. If B5701 is a type, then will NOT enumerate B57.
        /// If B5701 and B58 are types, then will enumerate both.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> EnumerateAll()
        {
            HashSet<string> reportedTwoDigit = new HashSet<string>();
            HashSet<string> reportedFourDigit = new HashSet<string>();

            foreach (string six in _synChanges)
            {
                reportedFourDigit.Add(six.Substring(0, 4));
                reportedTwoDigit.Add(GetGroupStringFromHigherResolution(six));
                yield return Locus.ToString() + six;
            }

            foreach (string four in _proteins)
            {
                if (!reportedFourDigit.Contains(four))
                {
                    reportedTwoDigit.Add(GetGroupStringFromHigherResolution(four));
                    yield return Locus.ToString() + four;
                }
            }

            foreach (string two in _groups)
            {
                if (!reportedTwoDigit.Contains(two))
                {
                    yield return Locus.ToString() + two;
                }
            }
        }

        private string GetGroupStringFromHigherResolution(string s)
        {
            string result = s.Substring(0, 2);
            if (Locus == HlaILocus.A && result == "92")
                result = "02";
            else if (Locus == HlaILocus.B && result == "95")
                result = "15";
            return result;
        }

        /// <summary>
        /// Enumerates all values at the given resolution.
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public IEnumerable<string> Enumerate(HlaIResolution resolution)
        {
            switch (resolution)
            {
                case HlaIResolution.Group:
                    return _groups.Select(type => Locus.ToString() + type);
                case HlaIResolution.Protein:
                    return _proteins.Select(type => Locus.ToString() + type);
                case HlaIResolution.Synonymous:
                    return _synChanges.Select(type => Locus.ToString() + type);
                default:
                    throw new NotImplementedException("Can't get here.");
            }
        }
    }
}
