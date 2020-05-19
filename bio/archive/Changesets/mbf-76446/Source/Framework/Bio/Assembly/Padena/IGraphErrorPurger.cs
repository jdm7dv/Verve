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
    /// Framework for detecting erroneous nodes and removing them.
    /// </summary>
    public interface IGraphErrorPurger
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
        /// Gets or sets the threshold length.
        /// </summary>
        int LengthThreshold { get; set; }

        /// <summary>
        /// Modifies de bruijn graph.
        /// Removes all the nodes in input.
        /// </summary>
        /// <param name="deBruijnGraph">De Bruijn Graph.</param>
        /// <param name="nodesList">List of nodes to be removed.</param>
        void RemoveErroneousNodes(DeBruijnGraph deBruijnGraph, DeBruijnPathList nodesList);

        /// <summary>
        /// Detects nodes that satisfy some error conditions .
        /// </summary>
        /// <param name="deBruijnGraph">Input graph.</param>
        /// <returns>List of error nodes.</returns>
        DeBruijnPathList DetectErroneousNodes(DeBruijnGraph deBruijnGraph);
    }
}
