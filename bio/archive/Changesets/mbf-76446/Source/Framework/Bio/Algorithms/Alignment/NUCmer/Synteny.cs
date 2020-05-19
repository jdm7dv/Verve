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

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// An ordered list of clusters between two sequences A and B
    /// </summary>
    public class Synteny
    {
        /// <summary>
        /// Reference sequence
        /// </summary>
        private ISequence internalReferenceSequence;

        /// <summary>
        /// Query sequence
        /// </summary>
        private ISequence internalQuerySequence;

        /// <summary>
        /// List of clusters
        /// </summary>
        private IList<Cluster> internalClusters;

        /// <summary>
        /// Initializes a new instance of the Synteny class
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequence">Query sequence</param>
        public Synteny(
                ISequence referenceSequence,
                ISequence querySequence)
        {
            internalReferenceSequence = referenceSequence;
            internalQuerySequence = querySequence;
            internalClusters = new List<Cluster>();
        }

        /// <summary>
        /// Gets reference sequence
        /// </summary>
        public ISequence ReferenceSequence
        {
            get { return internalReferenceSequence; }
        }

        /// <summary>
        /// Gets query sequence
        /// </summary>
        public ISequence QuerySequence
        {
            get { return internalQuerySequence; }
        }

        /// <summary>
        /// Gets list of clusters
        /// </summary>
        public IList<Cluster> Clusters
        {
            get { return internalClusters; }
        }
    }
}