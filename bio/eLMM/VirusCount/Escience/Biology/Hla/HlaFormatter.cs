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
using System.IO;
using MBT.Escience.Parse;
using Bio.Util;

namespace MBT.Escience.Biology.Hla
{
    /// <summary>
    /// The definition of how a set of HlaI genotypes will be written to file.
    /// </summary>
    public class HlaFormatter
    {
        /// <summary>
        /// The destination file. Use - for stdout.
        /// </summary>
        [Parse(ParseAction.Optional, typeof(OutputFile))]
        public FileInfo Out = new FileInfo("-");

        /// <summary> 
        /// If there are multiple haplotypes per patient, which ones do you want to keep?
        /// </summary>
        public IHlaIGenotypeDistiller Distiller = new FilterOnCompletionProb() { MinProbability = 0 };

        public void Write(IEnumerable<KeyValuePair<string, List<HlaIGenotype>>> patientsAndGenotypes)
        {
            bool multipleGenotypesPerPatient = patientsAndGenotypes.Select(pidAndGeno => pidAndGeno.Value).Any(gList => gList.Count > 1);

            if (multipleGenotypesPerPatient)
            {
                foreach (var pidAndGeno in patientsAndGenotypes)
                {
                    if (pidAndGeno.Value.Count == 1 && pidAndGeno.Value[0].Probability < 0)
                    {
                        pidAndGeno.Value[0].Probability = 1;
                    }
                    double totalProb = pidAndGeno.Value.Sum(g => g.Probability);
                    Helper.CheckCondition<InvalidDataException>(Math.Abs(totalProb - 1) < 0.0001, "Probabilities of pid {0}'s genotypes sum to {1}", pidAndGeno.Key, totalProb);
                    if (totalProb != 1)
                        pidAndGeno.Value.ForEach(g => g.Probability /= totalProb);  // if here, it's close but not exactly one. renormalize.
                }

                patientsAndGenotypes = Distiller.DistillGenotypes(patientsAndGenotypes);
            }

            WriteDistilled(patientsAndGenotypes, multipleGenotypesPerPatient);
        }

        protected virtual void WriteDistilled(IEnumerable<KeyValuePair<string, List<HlaIGenotype>>> patientsAndGenotypes, bool multipleGenotypesPerPatient)
        {
            using (TextWriter writer = Out.CreateTextOrUseConsole())
            {
                string header = "pid\tA1\tA2\tB1\tB2\tC1\tC2" + (multipleGenotypesPerPatient ? "\tHaplotypeProbability" : "");
                writer.WriteLine(header);

                patientsAndGenotypes.ForEach(pidToGeno => pidToGeno.Value.ForEach(geno =>
                    writer.WriteLine(pidToGeno.Key + "\t" + geno.ToString("\t"))));
            }
        }
    }
}
