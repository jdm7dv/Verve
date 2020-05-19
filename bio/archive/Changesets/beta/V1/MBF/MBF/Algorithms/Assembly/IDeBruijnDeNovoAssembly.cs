// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF.Algorithms.Assembly
{
    /// <summary>
    /// An IDeBruijnDeNovoAssembly is the result of running De Bruijn graph based 
    /// De Novo Assembly on a set of sequences. 
    /// </summary>
    public interface IDeBruijnDeNovoAssembly : IDeNovoAssembly
    {
        /// <summary>
        /// Gets list of contig sequences created by assembler
        /// </summary>
        IList<ISequence> ContigSequences { get; }

        /// <summary>
        /// Gets the list of assembler scaffolds
        /// </summary>
        IList<ISequence> Scaffolds { get; }
    }
}
