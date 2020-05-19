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
    /// An ordered list of matches between two a pair of sequences
    /// </summary>
    public class Cluster
    {
        /// <summary>
        /// Represents reverse query sequence direction
        /// </summary>
        public const string ReverseDirection = "REVERSE";

        /// <summary>
        /// Represents forward query sequence direction
        /// </summary>
        public const string ForwardDirection = "FORWARD";

        /// <summary>
        /// List of maximum unique matches inside the cluster
        /// </summary>
        private IList<MaxUniqueMatchExtension> _matches;

        /// <summary>
        /// Initializes a new instance of the Cluster class
        /// </summary>
        /// <param name="matches">List of matches</param>
        public Cluster(IList<MaxUniqueMatchExtension> matches)
        {
            _matches = matches;
            IsFused = false;
            QueryDirection = ForwardDirection;
        }

        /// <summary>
        /// Gets list of maximum unique matches inside the cluster
        /// </summary>
        public IList<MaxUniqueMatchExtension> Matches
        {
            get { return _matches; }
        }

        /// <summary>
        /// Gets or sets the query sequence direction
        ///     FORWARD or REVERSE
        /// </summary>
        public string QueryDirection { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cluster is already fused
        /// </summary>
        public bool IsFused { get; set; }
    }
}
