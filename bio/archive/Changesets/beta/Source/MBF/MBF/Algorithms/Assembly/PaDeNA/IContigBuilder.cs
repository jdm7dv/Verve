// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using MBF.Algorithms.Assembly.Graph;

namespace MBF.Algorithms.Assembly.PaDeNA
{
    /// <summary>
    /// Framework for building contig sequence from de bruijn graph.
    /// </summary>
    public interface IContigBuilder
    {
        /// <summary>
        /// Contructs the contigs by performing graph walking
        /// or graph modification.
        /// </summary>
        /// <param name="graph">Input graph</param>
        /// <returns>List of contigs</returns>
        IList<ISequence> Build(DeBruijnGraph graph);
    }
}
