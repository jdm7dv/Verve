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

namespace Bio.Algorithms.Assembly.Padena
{
    /// <summary>
    /// Interface removing contigs with low coverage.
    /// </summary>
    public interface ILowCoverageContigPurger
    {
        /// <summary>
        /// Build contigs from graph. For contigs whose coverage is less than 
        /// the specified threshold, remove graph nodes belonging to them.
        /// </summary>
        /// <param name="deBruijnGraph">DeBruijn Graph.</param>
        /// <param name="coverageThresholdForContigs">Coverage Threshold for contigs.</param>
        /// <returns>Number of nodes removed.</returns>
        int RemoveLowCoverageContigs(DeBruijnGraph deBruijnGraph, double coverageThresholdForContigs);
    }
}
