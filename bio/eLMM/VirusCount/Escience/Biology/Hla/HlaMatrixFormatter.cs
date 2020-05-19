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
using Bio.Matrix;
using MBT.Escience;
using System.IO;

namespace MBT.Escience.Biology.Hla
{
    public class HlaMatrixFormatter : HlaFormatter
    {
        /// <summary>
        /// How should HLA mixtures be interpreted?
        /// </summary>
        public MixtureSemantics MixtureSemantics = MixtureSemantics.Uncertainty;

        /// <summary>
        /// The method used to enumerate the HLA types that you want in the matrix.
        /// </summary>
        public HlaEnumerator HlaEnumerator = new Observed();

        protected override void WriteDistilled(IEnumerable<KeyValuePair<string, List<HlaIGenotype>>> patientsAndGenotypes, bool multipleGenotypesPerPatient)
        {
            var patientsAndGenotypesAsList = patientsAndGenotypes.ToList();

            Dictionary<string, MultinomialStatistics> pidToMultinomial = new Dictionary<string, MultinomialStatistics>();
            Dictionary<string, HlaIGenotype> pidGenoNameToGeno = new Dictionary<string, HlaIGenotype>(patientsAndGenotypesAsList.Count);

            foreach (var pidAndGenos in patientsAndGenotypes)
            {
                if (pidAndGenos.Value.Count == 1)
                {
                    pidGenoNameToGeno.Add(pidAndGenos.Key, pidAndGenos.Value.Single());
                }
                else
                {
                    pidToMultinomial.Add(pidAndGenos.Key, MultinomialStatistics.GetInstance(pidAndGenos.Value.Select(g => g.Probability))); // keep track of the probabilities in this dictionary

                    HlaIGenotype unionGenotype = HlaIGenotype.Union(pidAndGenos.Value.ToArray());
                    pidGenoNameToGeno.Add(pidAndGenos.Key, unionGenotype);  //Add the union genotype to the matrix.

                    for (int i = 0; i < pidAndGenos.Value.Count; i++)
                    {
                        string pidGenoId = pidAndGenos.Key + "_" + i;
                        pidGenoNameToGeno.Add(pidGenoId, pidAndGenos.Value[i]);
                    }
                }
            }

            Matrix<string, string, SufficientStatistics> m = DenseMatrix<string, string, SufficientStatistics>.CreateDefaultInstance(
                HlaEnumerator.GenerateHlas(patientsAndGenotypes.SelectMany(pidAndGenos => pidAndGenos.Value)), 
                pidGenoNameToGeno.Keys, 
                MissingStatistics.GetInstance());

            foreach (string hlastr in m.RowKeys)
            {
                HlaI hla = HlaI.Parse(hlastr);
                foreach (string pid in m.ColKeys)
                {
                    bool? match = pidGenoNameToGeno[pid].Matches(hla, MixtureSemantics);
                    m.SetValueOrMissing(hlastr, pid, BooleanStatistics.GetInstance(match));
                    //m.SetValueOrMissing(hlastr, pid, !match.HasValue ? "?" : match.Value ? "1" : "0");
                }
            }

            m.WriteDense(this.Out.CreateTextOrUseConsole());

            string baseName = Path.GetFileNameWithoutExtension(this.Out.ToString());
            string probabilityFile = this.Out.ToString() == "-" ? "HaplotypeCompletionProbs.txt" : this.Out.ToString().Replace(baseName, baseName + "_haplotypeProbs");

            pidToMultinomial.WriteDelimitedFile(probabilityFile);
        }

        private string GetProbabilityFile()
        {
            string outStr = Out.ToString();
            if (outStr == "-")
                return "HaplotypeProbabilities.txt";

            string basename = Path.GetFileNameWithoutExtension(outStr);
            string newName = basename + "_haplotypeProbabilities";
            string result = outStr.Replace(basename, newName);
            return result;
        }
    }
}
