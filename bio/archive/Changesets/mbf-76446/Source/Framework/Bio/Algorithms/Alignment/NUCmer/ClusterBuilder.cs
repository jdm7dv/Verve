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
using System;
using System.Collections.Generic;
using System.Linq;
using Bio.Algorithms.SuffixTree;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// Clustering is a process in which individual matches are grouped in larger
    /// set called Cluster. The matches in cluster are decided based on paramters 
    /// like fixed difference allowed, maximum difference allowed, minimum score
    /// and separation factor that should be satisfied.
    /// This class implements IClusterBuilder interface.
    /// </summary>
    public class ClusterBuilder : IClusterBuilder
    {
        /// <summary>
        /// Default fixed Separation
        /// </summary>
        internal const int DefaultFixedSeparation = 5;

        /// <summary>
        /// Default Maximum Separation
        /// </summary>
        internal const int DefaultMaximumSeparation = 1000;

        /// <summary>
        /// Default Minimum Output Score
        /// </summary>
        internal const int DefaultMinimumScore = 200;

        /// <summary>
        /// Default separation factor
        /// </summary>
        internal const float DefaultSeparationFactor = 0.05f;

        /// <summary>
        /// Property refering to Second sequence start in MUM
        /// </summary>
        private const string SecondSequenceStart = "SecondSequenceStart";

        /// <summary>
        /// Property refering to ID of Cluster
        /// </summary>
        private const string ClusterID = "ClusterID";

        /// <summary>
        /// This is a list of number which are used to generate the ID of cluster
        /// </summary>
        private IList<long> unionFind;

        /// <summary>
        /// Initializes a new instance of the ClusterBuilder class
        /// </summary>
        public ClusterBuilder()
        {
            FixedSeparation = DefaultFixedSeparation;
            MaximumSeparation = DefaultMaximumSeparation;
            MinimumScore = DefaultMinimumScore;
            SeparationFactor = DefaultSeparationFactor;
        }

        /// <summary>
        /// Gets or sets maximum fixed diagonal difference
        /// </summary>
        public int FixedSeparation { get; set; }

        /// <summary>
        /// Gets or sets maximum separation between the adjacent matches in clusters
        /// </summary>
        public int MaximumSeparation { get; set; }

        /// <summary>
        /// Gets or sets minimum output score
        /// </summary>
        public int MinimumScore { get; set; }

        /// <summary>
        /// Gets or sets separation factor. Fraction equal to 
        /// (diagonal difference / match separation) where higher values
        /// increase the insertion or deletion (indel) tolerance
        /// </summary>
        public float SeparationFactor { get; set; }

        /// <summary>
        /// Get the Cluster from given inputs of matches.
        /// Steps are as follows:
        ///     1. Sort MUMs based on query sequence start.
        ///     2. Removing overlapping MUMs (in both sequences) and MUMs with same 
        ///         diagonal offset (usually adjacent)
        ///     3. Check for  separation between two MUMs
        ///     4. Check the diagonal separation
        ///     5. If MUMs passes above conditions merge them in one cluster.
        ///     6. Sort MUMs using cluster id
        ///     7. Process clusters (Joining clusters)</summary>
        /// <param name="matchExtensions">List of maximum unique matches</param>
        /// <returns>List of Cluster</returns>
        public IList<Cluster> BuildClusters(IList<MatchExtension> matchExtensions)
        {
            // Validate the input
            if (null == matchExtensions)
            {
                return null;
            }

            if (0 == matchExtensions.Count)
            {
                return null;
            }

            unionFind = new List<long>(matchExtensions.Count);

            // populate unionFind
            foreach (MatchExtension match in matchExtensions)
            {
                unionFind.Add(-1);
            }

            // Get the cluster and return it
            return GetClusters(matchExtensions);
        }

        /// <summary>
        /// Removes the duplicate and overlapping maximal unique matches.
        /// </summary>
        /// <param name="matches">List of matches</param>
        private static void FilterMatches(IList<MatchExtension> matches)
        {
            int counter1, counter2;

            for (counter1 = 0; counter1 < matches.Count; counter1++)
            {
                matches[counter1].IsGood = true;
            }

            for (counter1 = 0; counter1 < matches.Count - 1; counter1++)
            {
                long diagonalIndex, endIndex;

                if (!matches[counter1].IsGood)
                {
                    continue;
                }

                diagonalIndex = matches[counter1].QuerySequenceOffset
                        - matches[counter1].ReferenceSequenceOffset;
                endIndex = matches[counter1].QuerySequenceOffset
                        + matches[counter1].Length;

                for (counter2 = counter1 + 1;
                        counter2 < matches.Count &&
                            matches[counter2].QuerySequenceOffset <= endIndex;
                        counter2++)
                {
                    long overlap;
                    long diagonalj;

                    if (matches[counter1].QuerySequenceOffset <= matches[counter2].QuerySequenceOffset)
                    {
                        if (!matches[counter2].IsGood)
                        {
                            continue;
                        }

                        diagonalj = matches[counter2].QuerySequenceOffset
                                - matches[counter2].ReferenceSequenceOffset;
                        if (diagonalIndex == diagonalj)
                        {
                            long extentj;

                            extentj = matches[counter2].Length
                                    + matches[counter2].QuerySequenceOffset
                                    - matches[counter1].QuerySequenceOffset;
                            if (extentj > matches[counter1].Length)
                            {
                                matches[counter1].Length = extentj;
                                endIndex = matches[counter1].QuerySequenceOffset
                                        + extentj;
                            }

                            // match lies on the same diagonal, this match cannot be part of
                            // any cluster
                            matches[counter2].IsGood = false;
                        }
                        else if (matches[counter1].ReferenceSequenceOffset == matches[counter2].ReferenceSequenceOffset)
                        {
                            // look for overlaps in second(query) sequence
                            overlap = matches[counter1].QuerySequenceOffset
                                    + matches[counter1].Length
                                    - matches[counter2].QuerySequenceOffset;

                            if (matches[counter1].Length < matches[counter2].Length)
                            {
                                if (overlap >= matches[counter1].Length / 2)
                                {
                                    // match is overlapping, this match cannot be part of 
                                    // any cluster
                                    matches[counter1].IsGood = false;
                                    break;
                                }
                            }
                            else if (matches[counter2].Length < matches[counter1].Length)
                            {
                                if (overlap >= matches[counter2].Length / 2)
                                {
                                    // match is overlapping, this match cannot be part of 
                                    // any cluster
                                    matches[counter2].IsGood = false;
                                }
                            }
                            else
                            {
                                if (overlap >= matches[counter1].Length / 2)
                                {
                                    matches[counter2].IsTentative = true;
                                    if (matches[counter1].IsTentative)
                                    {
                                        // match is overlapping, this match cannot be part of 
                                        // any cluster
                                        matches[counter1].IsGood = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (matches[counter1].QuerySequenceOffset == matches[counter2].QuerySequenceOffset)
                        {
                            // look for overlaps in first(reference) sequence
                            overlap = matches[counter1].ReferenceSequenceOffset
                                    + matches[counter1].Length
                                    - matches[counter2].ReferenceSequenceOffset;

                            if (matches[counter1].Length < matches[counter2].Length)
                            {
                                if (overlap >= matches[counter1].Length / 2)
                                {
                                    // match is overlapping, this match cannot be part of 
                                    // any cluster
                                    matches[counter1].IsGood = false;
                                    break;
                                }
                            }
                            else if (matches[counter2].Length < matches[counter1].Length)
                            {
                                if (overlap >= matches[counter2].Length / 2)
                                {
                                    // match is overlapping, this match cannot be part of 
                                    // any cluster
                                    matches[counter2].IsGood = false;
                                }
                            }
                            else
                            {
                                if (overlap >= matches[counter1].Length / 2)
                                {
                                    matches[counter2].IsTentative = true;
                                    if (matches[counter1].IsTentative)
                                    {
                                        // match is overlapping, this match cannot be part of 
                                        // any cluster
                                        matches[counter1].IsGood = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // Move all the matches that needs to be removed at the end of list
            for (counter1 = counter2 = 0; counter1 < matches.Count; counter1++)
            {
                if (matches[counter1].IsGood)
                {
                    if (counter1 != counter2)
                    {
                        matches[counter1].CopyTo(matches[counter2]);
                    }

                    counter2++;
                }
            }

            // Remove the matches that are not part of any cluster
            for (counter1 = matches.Count - 1; counter1 >= counter2; counter1--)
            {
                matches.RemoveAt(counter1);
            }

            for (counter1 = 0; counter1 < matches.Count; counter1++)
            {
                matches[counter1].IsGood = false;
            }
        }

        /// <summary>
        /// Sort by Cluster by specified column
        /// </summary>
        /// <param name="matches">List of matches</param>
        /// <param name="sortBy">Column to be sorted by</param>
        /// <returns>Sorted list of cluster</returns>
        private static IList<MatchExtension> Sort(
                IList<MatchExtension> matches,
                string sortBy)
        {
            IEnumerable<MatchExtension> sortedMatches = null;

            switch (sortBy)
            {
                case SecondSequenceStart:
                    sortedMatches = from cluster in matches
                                    orderby cluster.QuerySequenceOffset
                                    select cluster;
                    break;

                case ClusterID:
                    sortedMatches = from cluster in matches
                                    orderby cluster.ID
                                    orderby cluster.QuerySequenceOffset
                                    orderby cluster.ReferenceSequenceOffset
                                    select cluster;
                    break;

                default:
                    break;
            }

            return sortedMatches.ToList();
        }

        /// <summary>
        /// Process the matches and create clusters
        /// </summary>
        /// <param name="matches">List of matches</param>
        /// <returns>List of clusters</returns>
        private IList<Cluster> GetClusters(IList<MatchExtension> matches)
        {
            long separation, firstMatchIndex, secondMatchIndex;
            int clusterSize;
            int counter1, counter2;
            IList<Cluster> clusters = new List<Cluster>();

            // Sort matches by second sequence start
            matches = Sort(matches, SecondSequenceStart);

            // Remove overlapping and duplicate matches in cluster
            FilterMatches(matches);

            // Fnd the diagonal distance
            // If diagonal distance is less than user defined, they are clustered together
            for (counter1 = 0; counter1 < matches.Count - 1; counter1++)
            {
                long endIndex = matches[counter1].QuerySequenceOffset
                        + matches[counter1].Length;
                long diagonalIndex = matches[counter1].QuerySequenceOffset
                        - matches[counter1].ReferenceSequenceOffset;

                for (counter2 = counter1 + 1; counter2 < matches.Count; counter2++)
                {
                    long diagonalDifference;

                    separation = matches[counter2].QuerySequenceOffset - endIndex;
                    if (separation > MaximumSeparation)
                    {
                        break;
                    }

                    diagonalDifference = Math.Abs(
                            (matches[counter2].QuerySequenceOffset - matches[counter2].ReferenceSequenceOffset)
                            - diagonalIndex);
                    if (diagonalDifference <= Math.Max(FixedSeparation, SeparationFactor * separation))
                    {
                        firstMatchIndex = Find(counter1);
                        secondMatchIndex = Find(counter2);
                        if (firstMatchIndex != secondMatchIndex)
                        {
                            // add both the matches to the cluster
                            Union((int)firstMatchIndex, (int)secondMatchIndex);
                        }
                    }
                }
            }

            // Set the cluster id of each match
            for (counter1 = 0; counter1 < matches.Count; counter1++)
            {
                matches[counter1].ID = Find(counter1);
            }

            // Sort the matches by cluster id
            matches = Sort(matches, ClusterID);

            for (counter1 = 0; counter1 < matches.Count; counter1 += clusterSize)
            {
                counter2 = counter1 + 1;
                while (counter2 < matches.Count
                        && matches[counter1].ID == matches[counter2].ID)
                {
                    counter2++;
                }

                clusterSize = counter2 - counter1;
                ProcessCluster(
                        clusters,
                        matches.Where((Match, Index) => Index >= counter1).ToList(),
                        clusterSize);
            }

            return clusters;
        }

        /// <summary>
        /// Return the id of the set containing "a" in Union-Find.
        /// </summary>
        /// <param name="matchIndex">Index of the maximal unique match in UnionFind</param>
        /// <returns>Cluster id</returns>
        private long Find(int matchIndex)
        {
            long clusterId, counter1, counter2;

            if (unionFind[matchIndex] < 0)
            {
                return matchIndex;
            }

            for (clusterId = matchIndex; unionFind[(int)clusterId] > 0;)
            {
                clusterId = unionFind[(int)clusterId];
            }

            for (counter1 = matchIndex; unionFind[(int)counter1] != clusterId; counter1 = counter2)
            {
                counter2 = unionFind[(int)counter1];
                unionFind[(int)counter1] = clusterId;
            }

            return clusterId;
        }

        /// <summary>
        /// Group the matches in Union
        /// </summary>
        /// <param name="firstMatchIndex">Id of first cluster</param>
        /// <param name="secondMatchIndex">Id of second cluster</param>
        private void Union(int firstMatchIndex, int secondMatchIndex)
        {
            if (unionFind[firstMatchIndex] < 0 && unionFind[secondMatchIndex] < 0)
            {
                if (unionFind[firstMatchIndex] < unionFind[secondMatchIndex])
                {
                    unionFind[firstMatchIndex] += unionFind[secondMatchIndex];
                    unionFind[secondMatchIndex] = firstMatchIndex;
                }
                else
                {
                    unionFind[secondMatchIndex] += unionFind[firstMatchIndex];
                    unionFind[firstMatchIndex] = secondMatchIndex;
                }
            }
        }

        /// <summary>
        /// Process the clusters
        /// </summary>
        /// <param name="clusters">List of clusters</param>
        /// <param name="matches">List of matches</param>
        /// <param name="clusterSize">Size of cluster</param>
        private void ProcessCluster(
                IList<Cluster> clusters,
                IList<MatchExtension> matches,
                int clusterSize)
        {
            IList<MatchExtension> clusterMatches;
            long total, endIndex, startIndex, score;
            int counter1, counter2, counter3, best;

            do
            {
                // remove cluster overlaps
                for (counter1 = 0; counter1 < clusterSize; counter1++)
                {
                    matches[counter1].Score = matches[counter1].Length;
                    matches[counter1].Adjacent = 0;
                    matches[counter1].From = -1;

                    for (counter2 = 0; counter2 < counter1; counter2++)
                    {
                        long cost, overlap, overlap1, overlap2;

                        overlap1 = matches[counter2].ReferenceSequenceOffset
                                + matches[counter2].Length
                                - matches[counter1].ReferenceSequenceOffset;
                        overlap = Math.Max(0, overlap1);
                        overlap2 = matches[counter2].QuerySequenceOffset
                                + matches[counter2].Length -
                                matches[counter1].QuerySequenceOffset;
                        overlap = Math.Max(overlap, overlap2);

                        // cost matches which are not on same diagonal
                        cost = overlap
                                + Math.Abs((matches[counter1].QuerySequenceOffset - matches[counter1].ReferenceSequenceOffset)
                                - (matches[counter2].QuerySequenceOffset - matches[counter2].ReferenceSequenceOffset));

                        if (matches[counter2].Score + matches[counter1].Length - cost > matches[counter1].Score)
                        {
                            matches[counter1].From = counter2;
                            matches[counter1].Score = matches[counter2].Score
                                    + matches[counter1].Length
                                    - cost;
                            matches[counter1].Adjacent = overlap;
                        }
                    }
                }

                // Find the match which has highest score
                best = 0;
                for (counter1 = 1; counter1 < clusterSize; counter1++)
                {
                    if (matches[counter1].Score > matches[best].Score)
                    {
                        best = counter1;
                    }
                }

                total = 0;
                endIndex = int.MinValue;
                startIndex = int.MaxValue;

                // TODO: remove below cast
                for (counter1 = best; counter1 >= 0; counter1 = (int)matches[counter1].From)
                {
                    matches[counter1].IsGood = true;
                    total += matches[counter1].Length;
                    if (matches[counter1].QuerySequenceOffset + matches[counter1].Length > endIndex)
                    {
                        // Set the cluster end index
                        endIndex = matches[counter1].ReferenceSequenceOffset + matches[counter1].Length;
                    }

                    if (matches[counter1].ReferenceSequenceOffset < startIndex)
                    {
                        // Set the cluster start index
                        startIndex = matches[counter1].ReferenceSequenceOffset;
                    }
                }

                score = endIndex - startIndex;

                // If the current score exceeds the minimum score
                // and the matches to cluster
                if (score >= this.MinimumScore)
                {
                    clusterMatches = new List<MatchExtension>();

                    for (counter1 = 0; counter1 < clusterSize; counter1++)
                    {
                        if (matches[counter1].IsGood)
                        {
                            clusterMatches.Add(matches[counter1]);
                        }
                    }

                    // adding the cluster to list
                    if (0 < clusterMatches.Count)
                    {
                        clusters.Add(new Cluster(clusterMatches));
                    }
                }

                // Correcting the cluster indices
                for (counter1 = counter3 = 0; counter1 < clusterSize; counter1++)
                {
                    if (!matches[counter1].IsGood)
                    {
                        if (counter1 != counter3)
                        {
                            matches[counter3] = matches[counter1];
                        }

                        counter3++;
                    }
                }

                clusterSize = counter3;
            }
            while (clusterSize > 0);
        }
    }
}
