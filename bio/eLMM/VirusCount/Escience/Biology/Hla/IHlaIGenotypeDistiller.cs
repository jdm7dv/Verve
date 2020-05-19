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
    /// A definition for take the results of HlaComplete and picking the genotypes that you want to work with.
    /// </summary>
    /// <remarks>Designed for parsing</remarks>
    public interface IHlaIGenotypeDistiller
    {
        /// <summary>
        /// Given a list of genotypes for each patient, return a new list of genotypes for each patient.
        /// </summary>
        /// <param name="pidAndPossibleGenotypes"></param>
        /// <returns></returns>
        IEnumerable<KeyValuePair<string, List<HlaIGenotype>>> DistillGenotypes(IEnumerable<KeyValuePair<string, List<HlaIGenotype>>> pidAndPossibleGenotypes);
    }
}
