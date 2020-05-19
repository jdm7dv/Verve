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
using MBT.Escience;
using Bio.Util;

namespace MBT.Escience.Biology
{
    [Serializable()]
    public class GeneticCodeMapping
    {
        public GeneticCodeMapping(string codon, string aminoAcid, bool normal)
        {
            Helper.CheckCondition(codon.Length == 3); //!!!raise error
            Codon = codon;
            AminoAcid = aminoAcid;
            Normal = normal;
        }
        public string Codon;
        public string AminoAcid;
        public bool Normal;
    }

}
