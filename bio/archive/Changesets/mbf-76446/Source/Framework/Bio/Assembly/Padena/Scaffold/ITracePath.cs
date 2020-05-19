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
using Bio.Algorithms.Assembly.Padena.Scaffold.ContigOverlapGraph;

namespace Bio.Algorithms.Assembly.Padena.Scaffold
{
    /// <summary>
    /// Traverse through Contig overalp graphs to generate scaffold paths.
    /// </summary>
    public interface ITracePath
    {
       /// <summary>
        /// Performs Breadth First Search to traverse through graph to generate scaffold paths.
        /// </summary>
        /// <param name="overlapGraph">Contig Overlap Graph.</param>
        /// <param name="contigPairedReadMaps">InterContig Distances.</param>
        /// <param name="lengthOfKmer">Length of Kmer</param>
        /// <param name="searchDepth">Depth to which graph is searched.</param>
        /// <returns>List of paths/scaffold</returns>
        IList<ScaffoldPath> FindPaths(
            ContigGraph overlapGraph,
            ContigMatePairs contigPairedReadMaps,
            int lengthOfKmer,
            int searchDepth = 10);
    }
}
