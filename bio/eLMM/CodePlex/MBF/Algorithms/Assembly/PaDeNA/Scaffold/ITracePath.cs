//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



using System.Collections.Generic;
using Bio.Algorithms.Assembly.Graph;

namespace Bio.Algorithms.Assembly.PaDeNA.Scaffold
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
