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
using MBT.Escience.Biology;

namespace MBT.Escience.Biology.Hla
{
    public abstract class HlaEnumerator
    {
        /// <summary>
        /// A list of types to exclude from enumeration.
        /// </summary>
        public HashSet<string> Except = new HashSet<string>();

        /// <summary>
        /// A list of types to add to the enumeration list.
        /// </summary>
        public HashSet<string> Plus = new HashSet<string>();

        public IEnumerable<string> GenerateHlas(IEnumerable<HlaIGenotype> genotypes)
        {
            //var result = from hla in GenerateHlasBeforeFiltering(genotypes)
            //             where !Except.
            var result = GenerateHlasBeforeFiltering(genotypes).ToHashSet();
            result.RemoveWhere(hla => Except.Contains(hla));
            result.AddNewOrOldRange(Plus);
            
            return result;
        }

        /// <summary>
        /// Generates a list of HLAs. Duplicates are fine, as they will be filtered out later.
        /// </summary>
        protected abstract IEnumerable<string> GenerateHlasBeforeFiltering(IEnumerable<HlaIGenotype> genotypes);
    }

    /// <summary>
    /// Enumerates all the observed HLAs at they're highest resolution available.
    /// </summary>
    public class Observed : HlaEnumerator
    {
        protected override IEnumerable<string> GenerateHlasBeforeFiltering(IEnumerable<HlaIGenotype> genotypes)
        {
            return genotypes.SelectMany(g => g.EnumerateAll()).OrderBy(hla => hla);
        }
    }

    /// <summary>
    /// Enumerates HLAs at the specified resolutions.
    /// </summary>
    public class AtResolution : HlaEnumerator
    {
        /// <summary>
        /// The resolution at which to generate HLAs.
        /// </summary>
        [Parse(ParseAction.Required)]
        List<HlaIResolution> Resolution;

        protected override IEnumerable<string> GenerateHlasBeforeFiltering(IEnumerable<HlaIGenotype> genotypes)
        {
            return Resolution.SelectMany(resolution => genotypes.SelectMany(g => g.Enumerate(resolution)).OrderBy(hla => hla));
        }
    }

    /// <summary>
    /// Enumerates HLAs at two digit resolution. You can specify a list of two digit hlas that should be enumerated at 4 digit only.
    /// </summary>
    public class SelectTwoDigit : HlaEnumerator
    {
        /// <summary>
        /// A list of two digits that will only be parsed as four digits.
        /// </summary>
        public List<string> AsFourDigit = new List<string>() { "A68", "B15" };

        protected override IEnumerable<string> GenerateHlasBeforeFiltering(IEnumerable<HlaIGenotype> genotypes)
        {
            Except.AddNewOrOldRange(AsFourDigit);
            return genotypes.SelectMany(g => g.Enumerate(HlaIResolution.Group)).Union(
                genotypes.SelectMany(g => g.Enumerate(HlaIResolution.Protein)).Where(hla => AsFourDigit.Contains(hla.Substring(0, 3)))).OrderBy(hla => hla);
        }
    }
}
