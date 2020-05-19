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
using MBT.Escience.Parse;
using Bio.Util;

namespace MBT.Escience.Biology.Hla
{
    /// <summary>
    /// Keeps only those haplotypes whose completion probability is at least MinProbability.
    /// </summary>
    public class FilterOnCompletionProb : IParsable, IHlaIGenotypeDistiller
    {
        [Parse(ParseAction.Required)]
        public double MinProbability;

        /// <summary>
        /// If true, then all the probabilities that pass the filter will be renormalized.
        /// </summary>
        public bool Renormalize = true;

        public void FinalizeParse()
        {
            Helper.CheckCondition<ParseException>(MinProbability >= 0 && MinProbability <= 1);
        }

        public IEnumerable<KeyValuePair<string, List<HlaIGenotype>>> DistillGenotypes(IEnumerable<KeyValuePair<string, List<HlaIGenotype>>> pidAndPossibleGenotypes)
        {
            if (MinProbability == 0)
                return pidAndPossibleGenotypes;

            var result = from pidAndGeno in pidAndPossibleGenotypes
                         let thoseThatPass = pidAndGeno.Value.Where(g => g.Probability >= MinProbability)
                         let sumOfThoseThatPass = thoseThatPass.Sum(g => g.Probability)
                         select new KeyValuePair<string, List<HlaIGenotype>>(
                             pidAndGeno.Key,
                             Renormalize ?
                                thoseThatPass.Select(g => { g.Probability /= sumOfThoseThatPass; return g; }).ToList() :
                                thoseThatPass.ToList()
                             );

            return result;
        }
    }
}
