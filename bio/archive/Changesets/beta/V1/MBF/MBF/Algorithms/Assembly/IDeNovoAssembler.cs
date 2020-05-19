// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using MBF.Algorithms.Alignment;

namespace MBF.Algorithms.Assembly
{
    /// <summary>
    /// Representation of any sequence assembly algorithm.
    /// This interface defines contract for classes implementing De Novo Sequence assembler.
    /// </summary>
    public interface IDeNovoAssembler
    {
        /// <summary>
        /// Gets the name of the sequence assembly algorithm being
        /// implemented. This is intended to give the
        /// developer some information of the current sequence assembly algorithm.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the sequence assembly algorithm being
        /// implemented. This is intended to give the
        /// developer some information of the current sequence assembly algorithm.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Assemble the input sequences into the largest possible contigs. 
        /// </summary>
        /// <param name="inputSequences">The sequences to assemble.</param>
        /// <returns>IDeNovoAssembly instance which contains list of 
        /// assembled sequences</returns>
        IDeNovoAssembly Assemble(IList<ISequence> inputSequences);
    }
}