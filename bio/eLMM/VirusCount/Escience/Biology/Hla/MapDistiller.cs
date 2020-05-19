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

namespace MBT.Escience.Biology.Hla
{
    /// <summary>
    /// Hla formatter that takes the most likely haplotype.
    /// </summary>
    public class MapDistiller : IHlaIGenotypeDistiller
    {

        public IEnumerable<KeyValuePair<string, List<HlaIGenotype>>> DistillGenotypes(IEnumerable<KeyValuePair<string, List<HlaIGenotype>>> pidAndPossibleGenotypes)
        {
            var maps = from pidAndGeno in pidAndPossibleGenotypes
                       let bestGeno = pidAndGeno.Value.ArgMax(g => g.Probability)
                       select new KeyValuePair<string, List<HlaIGenotype>>(pidAndGeno.Key, bestGeno.AsSingletonEnumerable().ToList());
            return maps;
        }
    }
}
