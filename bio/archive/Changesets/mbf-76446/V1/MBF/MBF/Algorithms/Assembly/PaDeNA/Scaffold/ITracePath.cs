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

namespace MBF.Algorithms.Assembly.PaDeNA.Scaffold
{
    /// <summary>
    /// Traverse through Contig overalp graphs to generate scaffold paths.
    /// </summary>
    public interface ITracePath
    {
       /// <summary>
        /// Performs Breadth First Search to traverse through graph to generate scaffold paths.
        /// </summary>
        /// <param name="graph">Contig Overlap Graph.</param>
        /// <param name="contigPairedReadMaps">InterContig Distances.</param>
        /// <param name="kmerLength">Length of Kmer</param>
        /// <param name="depth">Depth to which graph is searched.</param>
        /// <returns>List of paths/scaffold</returns>
        IList<ScaffoldPath> FindPaths(
            DeBruijnGraph graph,
            ContigMatePairs contigPairedReadMaps,
            int kmerLength,
            int depth = 10);
    }
}
