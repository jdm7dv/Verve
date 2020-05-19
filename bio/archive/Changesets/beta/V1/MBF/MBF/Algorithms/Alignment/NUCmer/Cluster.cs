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