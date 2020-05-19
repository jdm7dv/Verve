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

namespace Bio.Algorithms.Alignment
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
        /// Gets or sets maximum seperation between the adjacent matches in clusters
        /// </summary>
        int MaximumSeparation { get; set; }

        /// <summary>
        /// Gets or sets minimum output score
        /// </summary>
        int MinimumScore { get; set; }

        /// <summary>
        /// Gets or sets seperation factor. Fraction equal to 
        /// (diagonal difference / match seperation) where higher values
        /// increase the indel tolerance
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
