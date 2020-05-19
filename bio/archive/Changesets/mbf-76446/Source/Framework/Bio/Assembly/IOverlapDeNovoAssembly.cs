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
using System.Collections.Generic;

namespace Bio.Algorithms.Assembly
{
    /// <summary>
    /// An IOverlapDeNovoAssembly is the result of running 
    /// Overlap based De Novo Assembly on a set of two or more sequences. 
    /// </summary>
    public interface IOverlapDeNovoAssembly : IDeNovoAssembly
    {
        /// <summary>
        /// Gets list of contigs created after Assembly.
        /// </summary>
        IList<Contig> Contigs { get; }

        /// <summary>
        /// Gets list of sequences that could not be merged into any contig.
        /// </summary>
        IList<ISequence> UnmergedSequences { get; }
    }
}
