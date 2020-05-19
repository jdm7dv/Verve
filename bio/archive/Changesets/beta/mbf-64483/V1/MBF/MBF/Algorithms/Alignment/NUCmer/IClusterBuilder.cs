// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF.Algorithms.Alignment
{
    /// <summary>
    /// Contract defined to implement class that creates clusters.
    /// Takes list of maximum unique matches as input and creates clusters
    /// </summary>
    public interface IClusterBuilder
    {
        /// <summary>
        /// Gets or sets maximum fixed diagonal difference
        /// </summary>
        int FixedSeparation { get; set; }

        /// <summary>
        /// Gets or sets maximum separation between the adjacent matches in clusters
        /// </summary>
        int MaximumSeparation { get; set; }

        /// <summary>
        /// Gets or sets minimum output score
        /// </summary>
        int MinimumScore { get; set; }

        /// <summary>
        /// Gets or sets separation factor. Fraction equal to 
        /// (diagonal difference / match separation) where higher values
        /// increase the insertion or deletion (indel) tolerance
        /// </summary>
        float SeparationFactor { get; set; }

        /// <summary>
        /// Build the list of clusters for given MUMs
        /// </summary>
        /// <param name="matches">List of MUMs</param>
        /// <returns>List of Cluster</returns>
        IList<Cluster> BuildClusters(IList<MaxUniqueMatch> matches);
    }
}