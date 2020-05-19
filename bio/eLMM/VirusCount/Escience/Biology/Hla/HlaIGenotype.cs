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
using MBT.Escience;

namespace MBT.Escience.Biology.Hla
{
    public class HlaIGenotype
    {
        public Tuple<HlaI,HlaI> AAlleles, BAlleles, CAlleles;

        /// <summary>
        /// If there are multiple possible haplotypes, we may assign a probability to each (e.g. the result of HlaCompletion).
        /// A negative probability implies it hasn't been set, and so is assumed to be 1.
        /// </summary>
        public double Probability = -1;

        public HlaIGenotype(HlaI A1, HlaI A2, HlaI B1, HlaI B2, HlaI C1, HlaI C2)
        {
            AAlleles = Tuple.Create(A1, A2);
            BAlleles = Tuple.Create(B1, B2);
            CAlleles = Tuple.Create(C1, C2);
        }

        public IEnumerable<HlaI> NonMissingHlas
        {
            get
            {
                if (null != AAlleles.Item1) yield return AAlleles.Item1;
                if (null != AAlleles.Item2) yield return AAlleles.Item2;

                if (null != BAlleles.Item1) yield return BAlleles.Item1;
                if (null != BAlleles.Item2) yield return BAlleles.Item2;

                if (null != CAlleles.Item1) yield return CAlleles.Item1;
                if (null != CAlleles.Item2) yield return CAlleles.Item2;
            }
        }

        /// <summary>
        /// Returns true if and only if we know for sure that this genotype matches the given Hla. Returns false if an only if we 
        /// know for sure that this genotype does not describe the given hla. Otherwise, returns null.
        /// </summary>
        /// <param name="unambiguousHla"></param>
        /// <returns></returns>
        public bool? Matches(HlaI unambiguousHla, MixtureSemantics mixtureSemantics = MixtureSemantics.Uncertainty)
        {
            Helper.CheckCondition<ArgumentException>(!unambiguousHla.IsAmbiguous, "Can only check if you have an uynambiguous Hla");

            Tuple<HlaI, HlaI> locus;
            switch (unambiguousHla.Locus)
            {
                case HlaILocus.A:
                    locus = AAlleles; break;
                case HlaILocus.B:
                    locus = BAlleles; break;
                case HlaILocus.C:
                    locus = CAlleles; break;
                default:
                    throw new Exception("Can't get here.");
            }

            UOPair<HlaI> locusToCompare = UOPair.Create(locus.Item1, locus.Item2);  // easier to treat as unordered pair here, since it will order the nulls.

            if (locusToCompare.First == null && locusToCompare.Second == null)
                return null;    // we know nothing about this locus

            // note: if one of them is null, it's first. so start with second.
            bool? secondIsOther = locusToCompare.Second.Matches(unambiguousHla, mixtureSemantics);
            if (secondIsOther.HasValue && secondIsOther.Value)
                return true;    // if second is a match, then we know it has it.

            if (locusToCompare.First == null)
                return null;    //don't know anything about first.

            bool? firstIsOther = locusToCompare.First.Matches(unambiguousHla, mixtureSemantics);
            if (firstIsOther.HasValue && firstIsOther.Value)
                return true;    // first is a match, so we know we have it

            if (!firstIsOther.HasValue || !secondIsOther.HasValue)
                return null;    // neither has it, so if either is missing, we don't know

            return false;    // if we get here, then both alleles reported false.

        }

        /// <summary>
        /// Enumerates all the alleles in this genotype at the specified resolution
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns></returns>
        public IEnumerable<string> Enumerate(HlaIResolution resolution)
        {
            foreach (HlaI hla in NonMissingHlas)
            {
                foreach (string type in hla.Enumerate(resolution))
                    yield return type;
            }
        }

        /// <summary>
        /// Enumerates all the distinct, highest-resolution types for each HLA.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> EnumerateAll()
        {
            var result = NonMissingHlas.SelectMany(hla => hla.EnumerateAll()).ToHashSet();
            return result;
        }

        private IEnumerable<HlaI> AllHlas()
        {
            yield return AAlleles.Item1;
            yield return AAlleles.Item2;
            yield return BAlleles.Item1;
            yield return BAlleles.Item2;
            yield return CAlleles.Item1;
            yield return CAlleles.Item2;
        }

        /// <summary>
        /// Clears all resolution details that are higher than the specified resolution.
        /// </summary>
        /// <param name="resolution"></param>
        public void SetMaxResolution(HlaIResolution resolution)
        {
            AllHlas().Where(hla => hla != null).ForEach(hla => hla.SetMaxResolution(resolution));
        }

        public override string ToString()
        {
            return ToString(" ");
        }

        public string ToString(string delim)
        {
            string result =
                   Helper.CreateDelimitedString(delim,
                        AAlleles.Item1 ?? (object)"?", AAlleles.Item2 ?? (object)"?",
                        BAlleles.Item1 ?? (object)"?", BAlleles.Item2 ?? (object)"?",
                        CAlleles.Item1 ?? (object)"?", CAlleles.Item2 ?? (object)"?");
            if (Probability > 0)
                result += delim + Probability;
            return result;
        }

        public override bool Equals(object obj)
        {
            return obj is HlaIGenotype && obj.ToString().Equals(this.ToString());
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        internal static HlaIGenotype Union(params HlaIGenotype[] hlaIGenotypes)
        {
            HlaI A1 = new HlaI(HlaILocus.A);
            HlaI A2 = new HlaI(HlaILocus.A);
            HlaI B1 = new HlaI(HlaILocus.B);
            HlaI B2 = new HlaI(HlaILocus.B);
            HlaI C1 = new HlaI(HlaILocus.C);
            HlaI C2 = new HlaI(HlaILocus.C);

            foreach (var geno in hlaIGenotypes)
            {
                A1.ParseInto(geno.AAlleles.Item1.ToString());
                A2.ParseInto(geno.AAlleles.Item2.ToString());

                B1.ParseInto(geno.BAlleles.Item1.ToString());
                B2.ParseInto(geno.BAlleles.Item2.ToString());

                C1.ParseInto(geno.CAlleles.Item1.ToString());
                C2.ParseInto(geno.CAlleles.Item2.ToString());
            }

            var result = new HlaIGenotype(A1, A2, B1, B2, C1, C2);
            return result;
        }
    }
}
