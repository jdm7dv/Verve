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
using System.Globalization;
using System.Linq;
using Bio.Algorithms.MUMmer;
using Bio.Algorithms.SuffixTree;
using Bio.SimilarityMatrices;
using Bio.Util.Logging;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// NUCmer is a system for rapidly aligning entire genomes or very large DNA
    /// sequences. It allows alignment of multiple reference sequences to multiple query sequences.
    /// This is commonly used to identify the position and orientation of set of sequence
    /// contigs in relation to finished sequence.
    /// NUCmer is the base abstract class which defines the contract for classes 
    /// implementing NUCmer algorithm. Using Template Method Pattern, NUCmer defines 
    /// the skeleton of the NUCmer algorithm and deferring some steps to derived class. 
    /// </summary>
    public class NUCmer
    {
        #region -- Constants --

        /// <summary>
        /// Default minimum length of Matches to be searched in streaming process
        /// </summary>
        private const int DefaultLengthOfMUM = 20;

        /// <summary>
        /// Default gap open penalty for use in alignment algorithms
        /// </summary>
        private const int DefaultGapOpenCost = -13;

        /// <summary>
        /// Default gap extension penalty for use in alignment algorithms
        /// </summary>
        private const int DefaultGapExtensionCost = -8;

        /// <summary>
        /// Represents reference sequences
        /// </summary>
        private const string ReferenceSequence = "ReferenceSequence";

        /// <summary>
        /// Represents query sequences
        /// </summary>
        private const string QuerySequence = "QuerySequence";

        /// <summary>
        /// Property refering to Second sequence start in MUM
        /// </summary>
        private const string FirstSequenceStart = "FirstSequenceStart";

        #endregion

        #region -- Member Variables --

        /// <summary>
        /// Alignment engine
        /// </summary>
        private ModifiedSmithWaterman nucmerAligner;

        /// <summary>
        /// Holds the reference sequence.
        /// </summary>
        private ISequence internalReferenceSequence;

        /// <summary>
        /// List of maximal unique matches.
        /// </summary>
        private IList<MatchExtension> internalMumList;

        /// <summary>
        /// List of clusters
        /// </summary>
        private IList<Cluster> internalClusterList;

        #endregion -- Member Variables --

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the NUCmer class.
        /// </summary>
        public NUCmer()
        {
            // User will typically choose their own parameters, these
            // defaults are reasonable for many cases.
            nucmerAligner = new ModifiedSmithWaterman();

            // Set the default Similarity Matrix
            SimilarityMatrix = new SimilarityMatrix(
                SimilarityMatrix.StandardSimilarityMatrix.DiagonalScoreMatrix);

            // Set the defaults
            GapOpenCost = DefaultGapOpenCost;
            GapExtensionCost = DefaultGapExtensionCost;
            LengthOfMUM = DefaultLengthOfMUM;

            // Set the ClusterBuilder properties to defaults
            FixedSeparation = ClusterBuilder.DefaultFixedSeparation;
            MaximumSeparation = ClusterBuilder.DefaultMaximumSeparation;
            MinimumScore = ClusterBuilder.DefaultMinimumScore;
            SeparationFactor = ClusterBuilder.DefaultSeparationFactor;
            BreakLength = ModifiedSmithWaterman.DefaultBreakLength;
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Gets or sets minimum length of Match that can be considered as MUM.
        /// </summary>
        public long LengthOfMUM { get; set; }

        /// <summary>
        /// Gets or sets similarity matrix for use in alignment algorithms.
        /// </summary>
        public SimilarityMatrix SimilarityMatrix { get; set; }

        /// <summary> 
        /// Gets or sets gap open penalty for use in alignment algorithms. 
        /// For alignments using a linear gap penalty, this is the gap penalty.
        /// For alignments using an affine gap, this is the penalty to open a new gap.
        /// This is a negative number, for example GapOpenCost = -8, not +8.
        /// </summary>
        public int GapOpenCost { get; set; }

        /// <summary> 
        /// Gets or sets gap extension penalty for use in alignment algorithms. 
        /// Not used for alignments using a linear gap penalty.
        /// For alignments using an affine gap, this is the penalty to
        /// extend an existing gap.
        /// This is a negative number, for example GapExtensionCost = -2, not +2.
        /// </summary>
        public int GapExtensionCost { get; set; }

        /// <summary>
        /// Gets or sets the object that will be used to compute the alignment's consensus.
        /// </summary>
        public IConsensusResolver ConsensusResolver { get; set; }

        /// <summary>
        /// Gets or sets number of bases to be extended before stopping alignment
        /// </summary>
        public int BreakLength { get; set; }

        // Properties specific to ClusterBuilder

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

        #endregion -- Properties --

        #region -- Main Execute Method --

        /// <summary>
        /// Gets the clusters
        /// </summary>
        /// <param name="referenceSequences">Reference sequences.</param>
        /// <param name="querySequences">Query sequences.</param>
        /// <param name="isUniqueInReference">flag to indicate that the matches should be unique in reference.</param>
        /// <returns>Returns clusters.</returns>
        public List<IList<Cluster>> GetClusters(
            IEnumerable<ISequence> referenceSequences,
            IEnumerable<ISequence> querySequences,
            bool isUniqueInReference = true)
        {
            internalReferenceSequence = referenceSequences.ElementAt(0);
            IAlphabet alphabetSet = internalReferenceSequence.Alphabet; 

            List<IList<Cluster>> result = new List<IList<Cluster>>();

            if (1 > LengthOfMUM)
            {
                string message = Properties.Resource.MUMLengthTooSmall;
                Trace.Report(message);
                throw new ArgumentException(message);
            }

            ValidateSequenceList(referenceSequences, alphabetSet, ReferenceSequence);
            ValidateSequenceList(querySequences, alphabetSet, QuerySequence);

            // Step:1 concat all the sequences into one sequence
            if (referenceSequences.Count() > 1)
            {
                internalReferenceSequence = ConcatSequence(referenceSequences);
            }

            Sequence seq = internalReferenceSequence as Sequence;
            if (seq == null)
            {
                throw new ArgumentException(Properties.Resource.OnlySequenceClassSupported);
            }
            // Mummer for performing step 3
            MUMmer.MUMmer internalMummer = new MUMmer.MUMmer(seq) { LengthOfMUM = LengthOfMUM };

            foreach (ISequence sequence in querySequences)
            {
                if (sequence.Equals(internalReferenceSequence))
                {
                    continue;
                }

                // Step3 : streaming process is performed with the query sequence

                IEnumerable<Match> matches;
                if (isUniqueInReference)
                {
                    matches = internalMummer.GetMatchesUniqueInReference(sequence);
                }
                else
                {
                    matches = internalMummer.GetMatches(sequence);
                }

                // Convert matches to matchextensions
                internalMumList = new List<MatchExtension>();
                long mumOrder = 1;
                foreach (Match currentMatch in matches)
                {
                    internalMumList.Add(new MatchExtension(currentMatch)
                    {
                        ReferenceSequenceMumOrder = mumOrder,
                        QuerySequenceMumOrder = mumOrder,
                        Query = sequence
                    });
                }

                if (internalMumList.Count > 0)
                {
                    // Step 5 : Get the list of Clusters
                    internalClusterList = GetClusters(internalMumList);

                    result.Add(internalClusterList);
                }
                else
                {
                    result.Add(null);
                }
            }

            return result;
        }

        /// <summary>
        /// Generates Delta Alignments.
        /// </summary>
        /// <param name="referenceSequences">Enumerable of Reference Sequences.</param>
        /// <param name="querySequences">Enumerable of Query Sequences.</param>
        /// <param name="isUniqueInReference">Whether MUMs are unique in query or not.</param>
        /// <returns>List of enumerable of delta alignments.</returns>
        public List<IEnumerable<DeltaAlignment>> GetDeltaAlignments(
            IEnumerable<ISequence> referenceSequences,
            IEnumerable<ISequence> querySequences,
            bool isUniqueInReference = true)
        {
            internalReferenceSequence = referenceSequences.ElementAt(0);
            List<IEnumerable<DeltaAlignment>> result = new List<IEnumerable<DeltaAlignment>>();

            if (1 > LengthOfMUM)
            {
                string message = Properties.Resource.MUMLengthTooSmall;
                Trace.Report(message);
                throw new ArgumentException(message);
            }

            // Step:1 concat all the sequences into one sequence
            if (referenceSequences.Count() > 1)
            {
                internalReferenceSequence = ConcatSequence(referenceSequences);
            }

            Sequence seq = internalReferenceSequence as Sequence;
            if (seq == null)
            {
                throw new ArgumentException(Properties.Resource.OnlySequenceClassSupported);
            }

            long referenceSequenceCount = seq.Count;
            // Mummer for performaing step 3
            MUMmer.MUMmer internalMummer = new MUMmer.MUMmer(seq) { LengthOfMUM = LengthOfMUM };

            foreach (ISequence sequence in querySequences)
            {
                if (sequence.Equals(internalReferenceSequence))
                {
                    continue;
                }

                // Step3 : streaming process is performed with the query sequence

                IEnumerable<Match> matches;
                if (isUniqueInReference)
                {
                    matches = internalMummer.GetMatchesUniqueInReference(sequence);
                }
                else
                {
                    matches = internalMummer.GetMatches(sequence);
                }

                // Convert matches to matchextensions
                internalMumList = new List<MatchExtension>();
                long mumOrder = 1;
                foreach (Match currentMatch in matches)
                {
                    internalMumList.Add(new MatchExtension(currentMatch)
                    {
                        ReferenceSequenceMumOrder = mumOrder,
                        QuerySequenceMumOrder = mumOrder,
                        Query = sequence
                    });
                }

                if (internalMumList.Count > 0)
                {
                    // Step 5 : Get the list of Clusters
                    internalClusterList = GetClusters(internalMumList);

                    // Step 7: Process Clusters and get delta
                    result.Add(ProcessCluster(
                            referenceSequences,
                        internalClusterList, referenceSequenceCount));
                }
            }

            return result;
        }

        #endregion -- Main Execute Method --

        #region -- Private Methods --

        /// <summary>
        /// Concat all the sequences into one sequence with special character
        /// </summary>
        /// <param name="sequences">list of reference sequence</param>
        /// <returns>Concatenated sequence</returns>
        private ISequence ConcatSequence(IEnumerable<ISequence> sequences)
        {
            if (sequences == null)
            {
                throw new ArgumentNullException("sequences");
            }

            // Add the Concatenating symbol to every sequence in reference list
            // Note that, as of now protein sequence is being created out 
            // of input sequence add  concatenation character "X" to it to 
            // simplify implemenation.
            List<byte> referenceSequence = null;
            foreach (ISequence sequence in sequences)
            {
                if (null == referenceSequence)
                {
                    referenceSequence = new List<byte>(sequence);
                }
                else
                {
                    referenceSequence.AddRange(sequence);
                }

                // Add conctatinating symbol.
                referenceSequence.Add((byte)'+');
            }

            return new Sequence(AlphabetExtensions.GetMummerAlphabet(sequences.ElementAt(0).Alphabet), referenceSequence.ToArray(), false);
        }

        /// <summary>
        /// get the clusters
        /// </summary>
        /// <param name="mumList">List of maximum unique matches</param>
        /// <returns>List of clusters</returns>
        private IList<Cluster> GetClusters(
                IList<MatchExtension> mumList)
        {
            IClusterBuilder clusterBuilder = new ClusterBuilder();

            if (-1 < FixedSeparation)
            {
                clusterBuilder.FixedSeparation = FixedSeparation;
            }

            if (-1 < MaximumSeparation)
            {
                clusterBuilder.MaximumSeparation = MaximumSeparation;
            }

            if (-1 < MinimumScore)
            {
                clusterBuilder.MinimumScore = MinimumScore;
            }

            if (-1 < SeparationFactor)
            {
                clusterBuilder.SeparationFactor = SeparationFactor;
            }

            return clusterBuilder.BuildClusters(mumList);
        }

        /// <summary>
        /// Process the cluster
        /// 1. Re-map the reference sequence index to original index
        /// 2. Create synteny
        /// 3. Process synteny
        /// </summary>
        /// <param name="referenceSequenceList">reference sequences</param>
        /// <param name="clusters">List of clusters</param>
        /// <param name="referenceSequenceLength">Length of reference sequence</param>
        /// <returns>List of delta alignments</returns>
        private IList<DeltaAlignment> ProcessCluster(
            IEnumerable<ISequence> referenceSequenceList,
            IList<Cluster> clusters, long referenceSequenceLength)
        {
            if (referenceSequenceList == null)
            {
                throw new ArgumentNullException("referenceSequence");
            }

            if (clusters == null)
            {
                throw new ArgumentNullException("clusters");
            }

            ISequence currentReference = null;
            ISequence currentQuery;
            IList<Synteny> syntenies = new List<Synteny>();
            Synteny currentSynteny = null;
            ISequence querySequence = null;
            ISequence referenceSequence = null;

            nucmerAligner.SimilarityMatrix = SimilarityMatrix;
            nucmerAligner.BreakLength = BreakLength;

            foreach (Cluster clusterIterator in clusters)
            {
                IList<MatchExtension> clusterMatches;
                if (null != currentSynteny)
                {
                    // Remove the empty clusters (if any)
                    if ((0 < currentSynteny.Clusters.Count)
                            && (0 == currentSynteny.Clusters.Last().Matches.Count))
                    {
                        currentSynteny.Clusters.Remove(
                            currentSynteny.Clusters.Last());
                    }

                    clusterMatches = new List<MatchExtension>();
                    currentSynteny.Clusters.Add(new Cluster(clusterMatches));
                }

                foreach (MatchExtension matchIterator in clusterIterator.Matches)
                {
                    currentQuery = matchIterator.Query;

                    // Re-map the reference coordinate back to its original sequence
                    foreach (ISequence sequence in referenceSequenceList)
                    {
                        if (matchIterator.ReferenceSequenceOffset < sequence.Count)
                        {
                            currentReference = sequence;
                            break;
                        }
                        else
                        {
                            matchIterator.ReferenceSequenceOffset -= sequence.Count + 1;
                        }
                    }
                    if ((null == referenceSequence)
                        || (null == querySequence)
                        || (string.Compare(referenceSequence.ID, currentReference.ID, StringComparison.OrdinalIgnoreCase) != 0)
                        || string.Compare(querySequence.ID, currentQuery.ID, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        bool found = false;

                        if ((null != querySequence)
                            && (string.Compare(querySequence.ID, currentQuery.ID, StringComparison.OrdinalIgnoreCase) == 0))
                        {
                            // Check if Synteny already exists
                            // If found, mark the synteny and break
                            foreach (Synteny syntenyIterator in syntenies)
                            {
                                if ((String.Compare(
                                        syntenyIterator.ReferenceSequence.ID,
                                        currentReference.ID,
                                        StringComparison.OrdinalIgnoreCase) == 0)
                                    && (String.Compare(
                                        syntenyIterator.QuerySequence.ID,
                                        currentQuery.ID,
                                        StringComparison.OrdinalIgnoreCase) == 0))
                                {
                                    currentSynteny = syntenyIterator;
                                    found = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ProcessSynteny(syntenies, referenceSequenceLength);
                        }

                        referenceSequence = currentReference;
                        querySequence = currentQuery;

                        // Remove the empty clusters (if any)
                        if ((null != currentSynteny)
                                && (0 < currentSynteny.Clusters.Count)
                                && (0 == currentSynteny.Clusters.Last().Matches.Count))
                        {
                            currentSynteny.Clusters.Remove(
                                currentSynteny.Clusters.Last());
                        }

                        if (!found)
                        {
                            // Create a Synteny
                            currentSynteny = new Synteny(
                                    currentReference,
                                    currentQuery);

                            // Add a cluster to Synteny
                            syntenies.Add(currentSynteny);
                        }

                        clusterMatches = new List<MatchExtension>();
                        currentSynteny.Clusters.Add(new Cluster(clusterMatches));
                    }

                    if (1 < matchIterator.Length)
                    {
                        currentSynteny.Clusters.Last().Matches.Add(matchIterator);
                    }
                }
            }

            return ProcessSynteny(syntenies, referenceSequenceLength);
        }

        /// <summary>
        /// Sort the clusters by given field
        /// </summary>
        /// <param name="clusters">List of clusters to be sorted</param>
        /// <param name="sortBy">Field to be sorted by</param>
        /// <returns>List of sorted clusters</returns>
        private static IList<Cluster> SortCluster(IList<Cluster> clusters, string sortBy)
        {
            IEnumerable<Cluster> sortedClusters = null;

            switch (sortBy)
            {
                case FirstSequenceStart:
                    sortedClusters = from cluster in clusters
                                     orderby cluster.Matches.First().ReferenceSequenceOffset
                                     select cluster;
                    break;

                default:
                    break;
            }

            return sortedClusters.ToList();
        }

        /// <summary>
        /// Check if the cluster is shadowed (contained in alignment)
        /// </summary>
        /// <param name="alignments">List of alignment</param>
        /// <param name="currentCluster">current cluster</param>
        /// <param name="currentDeltaAlignment">Current delta alignment</param>
        /// <returns>Is cluster contained in alignment</returns>
        private static bool IsClusterShadowed(
                IList<DeltaAlignment> alignments,
                Cluster currentCluster,
                DeltaAlignment currentDeltaAlignment)
        {
            DeltaAlignment alignment;

            long firstSequenceStart = currentCluster.Matches.First().ReferenceSequenceOffset;
            long firstSequenceEnd = currentCluster.Matches.Last().ReferenceSequenceOffset
                    + currentCluster.Matches.Last().Length - 1;
            long secondSequenceStart = currentCluster.Matches.First().QuerySequenceOffset;
            long secondSequenceEnd = currentCluster.Matches.Last().QuerySequenceOffset
                    + currentCluster.Matches.Last().Length - 1;

            if (0 < alignments.Count)
            {
                int counter;
                for (counter = alignments.IndexOf(currentDeltaAlignment); counter >= 0; counter--)
                {
                    alignment = alignments[counter];
                    if (alignment.QueryDirection == currentCluster.QueryDirection)
                    {
                        if ((alignment.FirstSequenceEnd >= firstSequenceEnd)
                                && alignment.SecondSequenceEnd >= secondSequenceEnd
                                && alignment.FirstSequenceStart <= firstSequenceStart
                                && alignment.SecondSequenceStart <= secondSequenceStart)
                        {
                            break;
                        }
                    }
                }

                if (counter >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Extend each cluster in every synteny
        /// </summary>
        /// <param name="syntenies">List of synteny</param>
        /// <param name="referenceSequenceLength">Length of reference sequence</param>
        /// <returns>List of delta alignments</returns>
        private IList<DeltaAlignment> ProcessSynteny(IList<Synteny> syntenies, long referenceSequenceLength)
        {
            IList<DeltaAlignment> deltaAlignments = new List<DeltaAlignment>();
            foreach (Synteny synteny in syntenies)
            {
                foreach (DeltaAlignment deltaAlignment in ExtendClusters(synteny))
                {
                    deltaAlignments.Add(deltaAlignment);
                }
            }
            return deltaAlignments;
        }

        /// <summary>
        /// Extend the cluster in synteny
        /// </summary>
        /// <param name="synteny">Synteny in which cluster needs to be extened.</param>
        /// <returns>List of delta alignments</returns>
        private IList<DeltaAlignment> ExtendClusters(Synteny synteny)
        {

            bool isClusterExtended = false;
            IList<DeltaAlignment> deltaAlignments = new List<DeltaAlignment>();
            DeltaAlignment deltaAlignment = null;
            Cluster currentCluster;
            Cluster targetCluster = synteny.Clusters.Last();

            IList<Cluster> clusters = synteny.Clusters;

            // Sort the cluster by first sequence start
            clusters = SortCluster(clusters, FirstSequenceStart);

            IEnumerator<Cluster> previousCluster = clusters.GetEnumerator();
            previousCluster.MoveNext();
            IEnumerator<Cluster> cluster = clusters.GetEnumerator();

            while (cluster.MoveNext())
            {
                currentCluster = cluster.Current;

                if (!isClusterExtended
                    && (currentCluster.IsFused
                        || IsClusterShadowed(deltaAlignments, currentCluster, deltaAlignment)))
                {
                    currentCluster.IsFused = true;
                    previousCluster.MoveNext();
                    currentCluster = previousCluster.Current;
                    continue;
                }

                // Extend the match
                foreach (MatchExtension match in currentCluster.Matches)
                {
                    if (isClusterExtended)
                    {
                        if (deltaAlignment.FirstSequenceEnd != match.ReferenceSequenceOffset
                            || deltaAlignment.SecondSequenceEnd != match.QuerySequenceOffset)
                        {
                            continue;
                        }

                        deltaAlignment.FirstSequenceEnd += match.Length - 1;
                        deltaAlignment.SecondSequenceEnd += match.Length - 1;
                    }
                    else
                    {
                        //TODO: Do we need sequence here? Changed to sequence id.
                        deltaAlignment = DeltaAlignment.NewAlignment(
                                synteny.ReferenceSequence,
                                synteny.QuerySequence,
                                currentCluster,
                                match);
                        deltaAlignments.Add(deltaAlignment);

                        // Find the MUM which is a good candidate for extension in reverse direction
                        DeltaAlignment targetAlignment = GetPreviousAlignment(deltaAlignments, deltaAlignment);

                        if (ExtendToPreviousSequence(
                                synteny.ReferenceSequence,
                                synteny.QuerySequence,
                                deltaAlignments,
                                deltaAlignment,
                                targetAlignment))
                        {
                            deltaAlignment = targetAlignment;
                        }
                    }

                    int methodName = ModifiedSmithWaterman.ForwardAlignFlag;

                    long targetReference;
                    long targetQuery;
                    if (currentCluster.Matches.IndexOf(match) < currentCluster.Matches.Count - 1)
                    {
                        // extend till the match in the current cluster
                        MatchExtension nextMatch =
                            currentCluster.Matches[currentCluster.Matches.IndexOf(match) + 1];
                        targetReference = nextMatch.ReferenceSequenceOffset;
                        targetQuery = nextMatch.QuerySequenceOffset;

                        isClusterExtended = ExtendToNextSequence(
                            synteny.ReferenceSequence,
                            synteny.QuerySequence,
                            deltaAlignment,
                            targetReference,
                            targetQuery,
                            methodName);
                    }
                    else
                    {
                        // extend till next cluster
                        targetReference = synteny.ReferenceSequence.Count - 1;
                        targetQuery = synteny.QuerySequence.Count() - 1;

                        targetCluster = GetNextCluster(
                                clusters,
                                currentCluster,
                                ref targetReference,
                                ref targetQuery);

                        if (!synteny.Clusters.Contains(targetCluster))
                        {
                            methodName |= ModifiedSmithWaterman.OptimalFlag;
                        }

                        isClusterExtended = ExtendToNextSequence(
                                synteny.ReferenceSequence,
                                synteny.QuerySequence,
                                deltaAlignment,
                                targetReference,
                                targetQuery,
                                methodName);
                    }
                }

                if (!synteny.Clusters.Contains(targetCluster))
                {
                    isClusterExtended = false;
                }

                currentCluster.IsFused = true;

                if (!isClusterExtended)
                {
                    previousCluster.MoveNext();
                    currentCluster = previousCluster.Current;
                }
                else
                {
                    currentCluster = targetCluster;
                }
            }


            return deltaAlignments;
        }

        /// <summary>
        /// Extend the cluster backward
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequence">Query sequence</param>
        /// <param name="alignments">List of alignments</param>
        /// <param name="currentAlignment">current alignment object</param>
        /// <param name="targetAlignment">target alignment object</param>
        /// <returns>Was clusted extended backward</returns>
        private bool ExtendToPreviousSequence(
                ISequence referenceSequence,
                ISequence querySequence,
                IList<DeltaAlignment> alignments,
                DeltaAlignment currentAlignment,
                DeltaAlignment targetAlignment)
        {
            bool isOverflow = false;
            long targetReference;
            long targetQuery;
            int methodName = ModifiedSmithWaterman.BackwardAlignFlag;

            if (alignments.Contains(targetAlignment))
            {
                targetReference = targetAlignment.FirstSequenceEnd;
                targetQuery = targetAlignment.SecondSequenceEnd;
            }
            else
            {
                // If the target alignment is not found then extend till the 
                // start of sequence (0th Symbol)
                targetReference = 0;
                targetQuery = 0;
                methodName |= ModifiedSmithWaterman.OptimalFlag;
            }

            // If the length in first sequence exceeds maximum length then extend 
            // till score is optimized irrespective of length.
            if ((currentAlignment.FirstSequenceStart - targetReference + 1) > ModifiedSmithWaterman.MaximumAlignmentLength)
            {
                targetReference = currentAlignment.FirstSequenceStart - ModifiedSmithWaterman.MaximumAlignmentLength + 1;
                isOverflow = true;
                methodName |= ModifiedSmithWaterman.OptimalFlag;
            }

            // If the length in second sequence exceeds maximum length then extend 
            // till score is optimized irrespective of length.
            if ((currentAlignment.SecondSequenceStart - targetQuery + 1) > ModifiedSmithWaterman.MaximumAlignmentLength)
            {
                targetQuery = currentAlignment.SecondSequenceStart = ModifiedSmithWaterman.MaximumAlignmentLength + 1;
                if (!isOverflow)
                {
                    isOverflow = true;
                }

                methodName |= ModifiedSmithWaterman.OptimalFlag;
            }

            // Extend the sequence to previous sequence (aligned/extended sequence)
            bool isClusterExtended = nucmerAligner.ExtendSequence(
                referenceSequence,
                currentAlignment.FirstSequenceStart,
                ref targetReference,
                querySequence,
                currentAlignment.SecondSequenceStart,
                ref targetQuery,
                currentAlignment.Deltas,
                methodName);

            if (isOverflow || !alignments.Contains(targetAlignment))
            {
                isClusterExtended = false;
            }

            if (isClusterExtended)
            {
                // Extend the sequence to next sequence (aligned/extended sequence)
                ExtendToNextSequence(
                    referenceSequence,
                    querySequence,
                    targetAlignment,
                    currentAlignment.FirstSequenceStart,
                    currentAlignment.SecondSequenceStart,
                    ModifiedSmithWaterman.ForcedForwardAlignFlag);

                targetAlignment.FirstSequenceEnd = currentAlignment.FirstSequenceEnd;
                targetAlignment.SecondSequenceEnd = currentAlignment.SecondSequenceEnd;
            }
            else
            {
                long startReference = currentAlignment.FirstSequenceStart;
                long startQuery = currentAlignment.SecondSequenceStart;
                nucmerAligner.ExtendSequence(
                    referenceSequence,
                    targetReference,
                    ref startReference,
                    querySequence,
                    targetQuery,
                    ref startQuery,
                    currentAlignment.Deltas,
                    ModifiedSmithWaterman.ForcedForwardAlignFlag);

                currentAlignment.FirstSequenceStart = targetReference;
                currentAlignment.SecondSequenceStart = targetQuery;

                // Adjust the delta reference position
                foreach (int deltaPosition in currentAlignment.Deltas)
                {
                    currentAlignment.DeltaReferencePosition +=
                        (deltaPosition > 0)
                        ? deltaPosition
                        : Math.Abs(deltaPosition) - 1;
                }
            }

            return isClusterExtended;
        }

        /// <summary>
        /// Extend the cluster forward
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequence">Query sequence</param>
        /// <param name="currentAlignment">current alignment object</param>
        /// <param name="targetReference">target position in reference sequence</param>
        /// <param name="targetQuery">target position in query sequence</param>
        /// <param name="methodName">Name of the method to be implemented</param>
        /// <returns>Was cluster extended forward</returns>
        private bool ExtendToNextSequence(
                ISequence referenceSequence,
                ISequence querySequence,
                DeltaAlignment currentAlignment,
                long targetReference,
                long targetQuery,
                int methodName)
        {
            bool isOverflow = false;
            bool isDouble = false;

            int diagonal = currentAlignment.Deltas.Count;

            long referenceDistance = targetReference - currentAlignment.FirstSequenceEnd + 1;
            long queryDistance = targetQuery - currentAlignment.SecondSequenceEnd + 1;

            // If the length in first sequence exceeds maximum length then extend 
            // till score is optimized irrespective of length.
            if (referenceDistance > ModifiedSmithWaterman.MaximumAlignmentLength)
            {
                targetReference = currentAlignment.FirstSequenceEnd + ModifiedSmithWaterman.MaximumAlignmentLength + 1;
                isOverflow = true;
                methodName |= ModifiedSmithWaterman.OptimalFlag;
            }

            // If the length in second sequence exceeds maximum length then extend 
            // till score is optimized irrespective of length.
            if (queryDistance > ModifiedSmithWaterman.MaximumAlignmentLength)
            {
                targetQuery = currentAlignment.SecondSequenceEnd + ModifiedSmithWaterman.MaximumAlignmentLength + 1;
                if (isOverflow)
                {
                    isDouble = true;
                }
                else
                {
                    isOverflow = true;
                }

                methodName |= ModifiedSmithWaterman.OptimalFlag;
            }

            if (isDouble)
            {
                methodName &= ~ModifiedSmithWaterman.SeqendFlag;
            }

            // Extend the sequence to next sequence (aligned/extended sequence)
            bool isClusterExtended = nucmerAligner.ExtendSequence(
                referenceSequence,
                currentAlignment.FirstSequenceEnd,
                ref targetReference,
                querySequence,
                currentAlignment.SecondSequenceEnd,
                ref targetQuery,
                currentAlignment.Deltas,
                methodName);

            if (isClusterExtended && isOverflow)
            {
                isClusterExtended = false;
            }

            if (diagonal < currentAlignment.Deltas.Count)
            {
                referenceDistance =
                    (currentAlignment.FirstSequenceEnd - currentAlignment.FirstSequenceStart + 1)
                    - currentAlignment.DeltaReferencePosition - 1;
                currentAlignment.Deltas[diagonal] += (currentAlignment.Deltas[diagonal] > 0)
                    ? referenceDistance
                    : -referenceDistance;

                // Adjust the delta reference position
                foreach (int deltaPosition in currentAlignment.Deltas)
                {
                    currentAlignment.DeltaReferencePosition +=
                        (deltaPosition > 0)
                        ? deltaPosition
                        : Math.Abs(deltaPosition) - 1;
                }
            }

            currentAlignment.FirstSequenceEnd = targetReference;
            currentAlignment.SecondSequenceEnd = targetQuery;

            return isClusterExtended;
        }

        /// <summary>
        /// Find the previous eligible sequence for alignment/extension
        /// </summary>
        /// <param name="alignments">List of alignment</param>
        /// <param name="currentAlignment">Current alignment</param>
        /// <returns>Reverse alignment</returns>
        private DeltaAlignment GetPreviousAlignment(
                IList<DeltaAlignment> alignments,
                DeltaAlignment currentAlignment)
        {
            long alignmentFirstStart = currentAlignment.FirstSequenceStart;
            long alignmentSecondStart = currentAlignment.SecondSequenceStart;
            long distance = (alignmentFirstStart < alignmentSecondStart)
                    ? alignmentFirstStart
                    : alignmentSecondStart;

            DeltaAlignment deltaAlignment = null;
            foreach (DeltaAlignment alignment in alignments)
            {
                if (currentAlignment.QueryDirection == alignment.QueryDirection)
                {
                    long alignmentFirstEnd = alignment.FirstSequenceEnd;
                    long alignmentSecondEnd = alignment.SecondSequenceEnd;

                    if (alignmentFirstEnd <= alignmentFirstStart
                        && alignmentSecondEnd <= alignmentSecondStart)
                    {
                        long gapHigh;
                        long gapLow;
                        if ((alignmentFirstStart - alignmentFirstEnd)
                            > (alignmentSecondStart - alignmentSecondEnd))
                        {
                            gapHigh = alignmentFirstStart - alignmentFirstEnd;
                            gapLow = alignmentSecondStart - alignmentSecondEnd;
                        }
                        else
                        {
                            gapLow = alignmentFirstStart - alignmentFirstEnd;
                            gapHigh = alignmentSecondStart - alignmentSecondEnd;
                        }

                        if (gapHigh < BreakLength
                                || ((gapLow * nucmerAligner.ValidScore)
                                    + ((gapHigh - gapLow)
                                    * nucmerAligner.SubstitutionScore)) >= 0)
                        {
                            deltaAlignment = alignment;
                            break;
                        }
                        else if ((gapHigh << 1) - gapLow < distance)
                        {
                            deltaAlignment = alignment;
                            distance = (gapHigh << 1) - gapLow;
                        }
                    }
                }
            }

            return deltaAlignment;
        }

        /// <summary>
        /// Find the next eligible sequence for alignment/extension
        /// </summary>
        /// <param name="clusters">List of clusters</param>
        /// <param name="currentCluster">Current cluster</param>
        /// <param name="targetReference">target position in reference sequence</param>
        /// <param name="targetQuery">target position in query sequence</param>
        /// <returns>Forward cluster in the list</returns>
        private Cluster GetNextCluster(
                IList<Cluster> clusters,
                Cluster currentCluster,
                ref long targetReference,
                ref long targetQuery)
        {
            long firstSequenceStart = currentCluster.Matches.Last().ReferenceSequenceOffset
                + currentCluster.Matches.Last().Length - 1;
            long secondSequenceStart = currentCluster.Matches.Last().QuerySequenceOffset
                + currentCluster.Matches.Last().Length - 1;

            long distance = (targetReference - firstSequenceStart < targetQuery - secondSequenceStart)
                    ? targetReference - firstSequenceStart
                    : targetQuery - secondSequenceStart;

            Cluster clusterIterator = null;
            Cluster cluster = null;
            for (int clusterIndex = clusters.IndexOf(currentCluster) + 1;
                    clusterIndex < clusters.Count;
                    clusterIndex++)
            {
                clusterIterator = clusters[clusterIndex];

                if (currentCluster.QueryDirection == clusterIterator.QueryDirection)
                {
                    long firstSequenceEnd = clusterIterator.Matches.First().ReferenceSequenceOffset;
                    long secondSequenceEnd = clusterIterator.Matches.First().QuerySequenceOffset;

                    if ((firstSequenceEnd < firstSequenceStart)
                            && (clusterIterator.Matches.Last().ReferenceSequenceOffset >= firstSequenceStart)
                            && (clusterIterator.Matches.Last().QuerySequenceOffset >= secondSequenceStart))
                    {
                        foreach (MatchExtension match in clusterIterator.Matches)
                        {
                            if ((firstSequenceEnd < firstSequenceStart)
                                    || (secondSequenceEnd < secondSequenceStart))
                            {
                                firstSequenceEnd = match.ReferenceSequenceOffset;
                                secondSequenceEnd = match.QuerySequenceOffset;
                            }
                        }
                    }

                    if ((firstSequenceEnd >= firstSequenceStart)
                            && (secondSequenceEnd >= secondSequenceStart))
                    {
                        long gapHigh;
                        long gapLow;
                        if ((firstSequenceEnd - firstSequenceStart)
                                > (secondSequenceEnd - secondSequenceStart))
                        {
                            gapHigh = firstSequenceEnd - firstSequenceStart;
                            gapLow = secondSequenceEnd - secondSequenceStart;
                        }
                        else
                        {
                            gapLow = firstSequenceEnd - firstSequenceStart;
                            gapHigh = secondSequenceEnd - secondSequenceStart;
                        }

                        if (gapHigh < BreakLength
                                || ((gapLow * nucmerAligner.ValidScore) +
                                    ((gapHigh - gapLow)
                                    * nucmerAligner.SubstitutionScore)) >= 0)
                        {
                            cluster = clusterIterator;
                            targetReference = firstSequenceEnd;
                            targetQuery = secondSequenceEnd;
                            break;
                        }
                        else if ((gapHigh << 1) - gapLow < distance)
                        {
                            cluster = clusterIterator;
                            targetReference = firstSequenceEnd;
                            targetQuery = secondSequenceEnd;
                            distance = (gapHigh << 1) - gapLow;
                        }
                    }
                }
            }

            return cluster;
        }

        /// <summary>
        /// Validate the list of sequences
        /// </summary>
        /// <param name="sequenceList">List of sequence</param>
        /// <param name="alphabetSet">Alphabet set</param>
        /// <param name="sequenceType">Type of sequence</param>
        public void ValidateSequenceList(
                IEnumerable<ISequence> sequenceList,
                IAlphabet alphabetSet,
                string sequenceType)
        {
            bool isValidLength = false;

            foreach (ISequence sequence in sequenceList)
            {
                if (null == sequence)
                {
                    string message = sequenceType == ReferenceSequence
                        ? Properties.Resource.ReferenceSequenceCannotBeNull
                        : Properties.Resource.QuerySequenceCannotBeNull;
                    Trace.Report(message);
                    throw new ArgumentException(message);
                }

                if (sequence.Alphabet != alphabetSet)
                {
                    string message = Properties.Resource.InputAlphabetsMismatch;
                    Trace.Report(message);
                    throw new ArgumentException(message);
                }

                if (sequence.Count > LengthOfMUM)
                {
                    isValidLength = true;
                }
            }

            if (!isValidLength)
            {
                string message = String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.InputSequenceMustBeGreaterThanMUM,
                        LengthOfMUM);
                Trace.Report(message);
                throw new ArgumentException(message);
            }
        }

        #endregion -- Private Methods --
    }
}
