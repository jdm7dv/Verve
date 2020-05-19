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
using Bio.Algorithms.Assembly.Graph;

namespace Bio.Algorithms.Assembly.Padena
{
    /// <summary>
    /// Interface for eroding graph nodes that have
    /// low coverage.
    /// </summary>
    public interface IGraphEndsEroder
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
        /// Erode ends of graph that have low coverage.
        /// For optimization of another step (dangling link purger)
        /// in assembly process, this returns a list of integers.
        /// In case this optimization is not used, a single element
        /// list containing the number of eroded nodes can be returned.
        /// </summary>
        /// <param name="graph">Input graph.</param>
        /// <param name="erosionThreshold">Threshold for erosion.</param>
        /// <returns>List containing the number of nodes eroded.</returns>
        IEnumerable<int> ErodeGraphEnds(DeBruijnGraph graph, int erosionThreshold);
    }
}
