// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using MBF.Algorithms.Assembly.Graph;

namespace MBF.Algorithms.Assembly.PaDeNA
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
        /// <param name="graph">DeBruijn Graph</param>
        /// <param name="coverageThreshold">Coverage Threshold for contigs</param>
        /// <returns>Number of nodes removed</returns>
        int RemoveLowCoverageContigs(DeBruijnGraph graph, double coverageThreshold);
    }
}
