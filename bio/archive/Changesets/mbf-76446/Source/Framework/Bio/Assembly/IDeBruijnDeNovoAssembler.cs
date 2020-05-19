// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using Bio.Algorithms.Assembly.Graph;

namespace Bio.Algorithms.Assembly
{
    /// <summary>
    /// Representation of any sequence assembly algorithm.
    /// This interface defines contract for classes implementing 
    /// De Bruijn graph based De Novo Sequence assembler.
    /// </summary>
    public interface IDeBruijnDeNovoAssembler : IDeNovoAssembler
    {
        /// <summary>
        /// Gets or sets the kmer length.
        /// </summary>
        int KmerLength { get; set; }

        /// <summary>
        /// Gets the assembler de-bruijn graph.
        /// </summary>
        DeBruijnGraph Graph { get;  }
    }
}