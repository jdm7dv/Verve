// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MBF.Algorithms.SuffixTree;

namespace MBF.Algorithms.Alignment
{
    /// <summary>
    /// Implementation of NUCmer3.
    /// This class uses suffix tree for DNA alignment.
    /// <see cref="NUCmer"/>
    /// </summary>
    public class NUCmer3 : NUCmer
    {
        #region -- Constants --

        /// <summary>
        /// Property refering to Second sequence start in MUM
        /// </summary>
        private const string FirstSequenceStart = "FirstSequenceStart";

        #endregion

        #region -- Member Variables --

        /// <summary>
        /// Alignment engine
        /// </summary>
        private NUCmerAligner nucmerAligner = null;

        #endregion

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the NUCmer3 class
        /// </summary>
        public NUCmer3()
        {
            nucmerAligner = new NUCmerAligner();
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Gets the name of the current Alignment algorithm used.
        /// This is a overriden property from the abstract parent.
        /// This property returns the Name of our algorithm i.e 
        /// MUMmer v3.0 algorithm.
        /// </summary>
        public override string Name
        {
            get { return Properties.Resource.NUCMER3; }
        }

        /// <summary>
        /// Gets the Description of the current Alignment algorithm used.
        /// This is a overriden property from the abstract parent.
        /// This property returns a simple description of what 
        /// MUMmer3 class implements.
        /// </summary>
        public override string Description
        {
            get { return Properties.Resource.NUCMER3DESC; }
        }

        /// <summary>
        /// Index of reference sequence in list of sequences
        /// </summary>
        public override int ReferenceSequenceNumber
        {
            get;
            set;
        }

        #endregion

        #region -- Methods --

        /// <summary>
        /// Concat all the sequences into one sequence with special character
        /// </summary>
        /// <param name="sequences">list of reference sequence</param>
        /// <returns>Concatenated sequence</returns>
        protected override ISequence ConcatSequence(IList<ISequence> sequences)
        {
            // Add the Concatenating symbol to every sequence in reference list
            // Note that, as of now protein sequence is being created out 
            // of input sequence add  concatenation character "X" to it to 
            // simplify implemenation.
            SegmentedSequence referenceSequence = null;
            foreach (ISequence sequence in sequences)
            {
                if (null == referenceSequence)
                {
                    referenceSequence = new SegmentedSequence(sequence);
                }
                else
                {
                    referenceSequence.Sequences.Add(sequence);
                }
            }

            return referenceSequence;
        }

        /// <summary>
        /// get the clusters
        /// </summary>
        /// <param name="mumList">List of maximum unique matches</param>
        /// <returns>List of clusters</returns>
        protected override IList<Cluster> GetClusters(
                IList<MaxUniqueMatch> mumList)
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
        /// Build Suffix Tree using reference sequence
        /// </summary>
        /// <param name="referenceSequence">sequence to build SuffixTree</param>
        /// <returns>Suffix Tree</returns>
        protected override ISuffixTree BuildSuffixTree(ISequence referenceSequence)
        {
            ISuffixTreeBuilder suffixTreeBuilder = Factory.CreateNew(referenceSequence);
            ISuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSequence);
            return suffixTree;
        }

        /// <summary>
        /// Traverse the suffix tree using query sequence and return list of MUMs
        /// </summary>
        /// <param name="suffixTree">Suffix tree</param>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="sequence">Query sequence</param>
        /// <param name="lengthOfMUM">Minimum length of MUM</param>
        /// <returns>List of MUMs</returns>
        protected override IList<MaxUniqueMatch> Streaming(
                ISuffixTree suffixTree,
                ISequence referenceSequence,
                ISequence sequence,
                long lengthOfMUM)
        {
            ISuffixTreeBuilder suffixTreeBuilder = Factory.CreateNew(referenceSequence);
            return suffixTreeBuilder.FindMatches(suffixTree, sequence, lengthOfMUM);
        }

        /// <summary>
        /// Process the cluster
        /// 1. Re-map the reference sequence index to original index
        /// 2. Create synteny
        /// 3. Process synteny
        /// </summary>
        /// <param name="referenceSequenceList">List of reference sequences</param>
        /// <param name="clusters">List of clusters</param>
        /// <returns>List of delta alignments</returns>
        protected override IList<DeltaAlignment> ProcessCluster(
            IList<ISequence> referenceSequenceList,
            IList<Cluster> clusters)
        {
            ISequence currentReference = null;
            ISequence currentQuery = null;
            IList<Synteny> syntenies = new List<Synteny>();
            IList<MaxUniqueMatchExtension> clusterMatches = null;
            Synteny currentSynteny = null;
            ISequence referenceSequence = null;
            ISequence querySequence = null;
            bool found = false;

            nucmerAligner.SimilarityMatrix = SimilarityMatrix;
            nucmerAligner.BreakLength = BreakLength;

            foreach (Cluster clusterIterator in clusters)
            {
                if (null != currentSynteny)
                {
                    // Remove the empty clusters (if any)
                    if ((null != currentSynteny)
                            && (0 < currentSynteny.Clusters.Count)
                            && (0 == currentSynteny.Clusters.Last().Matches.Count))
                    {
                        currentSynteny.Clusters.Remove(
                            currentSynteny.Clusters.Last());
                    }

                    clusterMatches = new List<MaxUniqueMatchExtension>();
                    currentSynteny.Clusters.Add(new Cluster(clusterMatches));
                }

                foreach (MaxUniqueMatchExtension matchIterator in clusterIterator.Matches)
                {
                    currentQuery = matchIterator.Query;

                    // Re-map the reference coordinate back to its original sequence
                    foreach (ISequence sequence in referenceSequenceList)
                    {
                        if (matchIterator.FirstSequenceStart < sequence.Count)
                        {
                            currentReference = sequence;
                            break;
                        }
                        else
                        {
                            matchIterator.FirstSequenceStart -= sequence.Count + 1;
                        }
                    }

                    if ((null == referenceSequence)
                        || (null == querySequence)
                        || (string.Compare(referenceSequence.ID, currentReference.ID, StringComparison.OrdinalIgnoreCase) != 0)
                        || string.Compare(querySequence.ID, currentQuery.ID, StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        found = false;

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
                            ProcessSynteny(syntenies);
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

                        clusterMatches = new List<MaxUniqueMatchExtension>();
                        currentSynteny.Clusters.Add(new Cluster(clusterMatches));
                    }

                    if (1 < matchIterator.Length)
                    {
                        currentSynteny.Clusters.Last().Matches.Add(matchIterator);
                    }
                }
            }

            return ProcessSynteny(syntenies);
        }

        /// <summary>
        /// Calculate the score of alignment
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequence">Query sequence</param>
        /// <returns>Score of the alignment</returns>
        protected override int CalculateScore(
                ISequence referenceSequence,
                ISequence querySequence)
        {
            int index, score, gapCount, row, column;
            char referenceCharacter, queryCharacter;

            score = 0;

            // For each pair of symbols (characters) in reference and query sequence
            // 1. If the character are different and not alignment character "-", 
            //      then find the cost form Similarity Matrix
            // 2. If Gap Extension cost needs to be used
            //      a. Find how many gaps exists in appropriate sequence (reference / query)
            //          and calculate the score
            // 3. Add the gap open cost
            for (index = 0; index < referenceSequence.Count; index++)
            {
                referenceCharacter = referenceSequence[index].Symbol;
                queryCharacter = querySequence[index].Symbol;

                if (DnaAlphabet.Instance.Gap.Symbol != referenceCharacter
                    && DnaAlphabet.Instance.Gap.Symbol != queryCharacter)
                {
                    row = Char.ToUpper(referenceCharacter, CultureInfo.InvariantCulture)
                            - DnaAlphabet.Instance.A.Symbol;
                    column = Char.ToUpper(queryCharacter, CultureInfo.InvariantCulture)
                            - DnaAlphabet.Instance.A.Symbol;
                    score += SimilarityMatrix[row, column];
                }
                else
                {
                    if (IsAlign)
                    {
                        if (DnaAlphabet.Instance.Gap.Symbol == referenceCharacter)
                        {
                            gapCount = FindExtensionLength(referenceSequence, index);
                        }
                        else
                        {
                            gapCount = FindExtensionLength(querySequence, index);
                        }

                        score += GapOpenCost + (gapCount * GapExtensionCost);

                        // move the index pointer to end of extension
                        index = index + gapCount - 1;
                    }
                    else
                    {
                        score += GapOpenCost;
                    }
                }
            }

            return score;
        }

        /// <summary>
        /// Convert to delta alignments to sequence alignments
        /// </summary>
        /// <param name="alignments">list of delta alignments</param>
        /// <returns>List of Sequence alignment</returns>
        protected override IList<PairwiseAlignedSequence> ConvertDeltaToAlignment(
                IList<DeltaAlignment> alignments)
        {
            IList<PairwiseAlignedSequence> alignedSequences = null;
            PairwiseAlignedSequence alignedSequence;
            int referenceStart, queryStart, difference;

            alignedSequences = new List<PairwiseAlignedSequence>();
            foreach (DeltaAlignment deltaAlignment in alignments)
            {
                alignedSequence = deltaAlignment.ConvertDeltaToSequences();

                // Find the offsets
                referenceStart = deltaAlignment.FirstSequenceStart;
                queryStart = deltaAlignment.SecondSequenceStart;
                difference = referenceStart - queryStart;
                if (0 < difference)
                {
                    alignedSequence.FirstOffset = 0;
                    alignedSequence.SecondOffset = difference;
                }
                else
                {
                    alignedSequence.FirstOffset = -1 * difference;
                    alignedSequence.SecondOffset = 0;
                }

                alignedSequences.Add(alignedSequence);
            }

            return alignedSequences;
        }

        /// <summary>
        /// Analyze the given seqquences and store a consensus into its Consensus property.
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequence">Query sequence</param>
        /// <returns>Consensus of sequences</returns>
        protected override ISequence MakeConsensus(
                ISequence referenceSequence,
                ISequence querySequence)
        {
            // For each pair of symbols (characters) in reference and query sequence
            // get the consensus symbol and append it.
            Sequence consensus = new Sequence(referenceSequence.Alphabet);
            for (int index = 0; index < referenceSequence.Count; index++)
            {
                consensus.Add(
                    ConsensusResolver.GetConsensus(
                    new List<ISequenceItem>() { referenceSequence[index], querySequence[index] }));
            }

            return consensus;
        }

        /// <summary>
        /// Find the index of extension
        /// </summary>
        /// <param name="sequence">Sequence object</param>
        /// <param name="index">Position at which extension starts</param>
        /// <returns>Last index of extension</returns>
        private static int FindExtensionLength(ISequence sequence, int index)
        {
            // Find the number of alignment characters "-" in the given sequence 
            // from positon index
            int gapCounter = index;

            while (gapCounter < sequence.Count
                    && DnaAlphabet.Instance.Gap.Symbol == sequence[gapCounter].Symbol)
            {
                gapCounter++;
            }

            return gapCounter - index;
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
                                     orderby cluster.Matches.First().FirstSequenceStart
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
            DeltaAlignment alignment = null;
            int counter;

            int firstSequenceStart = currentCluster.Matches.First().FirstSequenceStart;
            int firstSequenceEnd = currentCluster.Matches.Last().FirstSequenceStart
                    + currentCluster.Matches.Last().Length - 1;
            int secondSequenceStart = currentCluster.Matches.First().SecondSequenceStart;
            int secondSequenceEnd = currentCluster.Matches.Last().SecondSequenceStart
                    + currentCluster.Matches.Last().Length - 1;

            if (0 < alignments.Count)
            {
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
        /// <returns>List of delta alignments</returns>
        private IList<DeltaAlignment> ProcessSynteny(IList<Synteny> syntenies)
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
            DeltaAlignment targetAlignment = null;
            Cluster currentCluster = null;
            Cluster targetCluster = synteny.Clusters.Last();
            int targetReference;
            int targetQuery;
            int methodName = NUCmerAligner.ForwardAlignFlag;

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
                foreach (MaxUniqueMatchExtension match in currentCluster.Matches)
                {
                    if (isClusterExtended)
                    {
                        if (deltaAlignment.FirstSequenceEnd != match.FirstSequenceStart
                            || deltaAlignment.SecondSequenceEnd != match.SecondSequenceStart)
                        {
                            continue;
                        }

                        deltaAlignment.FirstSequenceEnd += match.Length - 1;
                        deltaAlignment.SecondSequenceEnd += match.Length - 1;
                    }
                    else
                    {
                        deltaAlignment = DeltaAlignment.NewAlignment(
                                synteny.ReferenceSequence,
                                synteny.QuerySequence,
                                currentCluster,
                                match);
                        deltaAlignments.Add(deltaAlignment);

                        // Find the MUM which is a good candidate for extension in reverse direction
                        targetAlignment = GetPreviousAlignment(deltaAlignments, deltaAlignment);

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

                    methodName = NUCmerAligner.ForwardAlignFlag;

                    if (currentCluster.Matches.IndexOf(match) < currentCluster.Matches.Count - 1)
                    {
                        // extend till the match in the current cluster
                        MaxUniqueMatchExtension nextMatch =
                            currentCluster.Matches[currentCluster.Matches.IndexOf(match) + 1];
                        targetReference = nextMatch.FirstSequenceStart;
                        targetQuery = nextMatch.SecondSequenceStart;

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
                        targetQuery = synteny.QuerySequence.Count - 1;

                        targetCluster = GetNextCluster(
                                clusters,
                                currentCluster,
                                ref targetReference,
                                ref targetQuery);

                        if (!synteny.Clusters.Contains(targetCluster))
                        {
                            methodName |= NUCmerAligner.OptimalFlag;
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
            bool isClusterExtended = false;
            bool isOverflow = false;
            int targetReference;
            int targetQuery;
            int startReference;
            int startQuery;
            int methodName = NUCmerAligner.BackwardAlignFlag;

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
                methodName |= NUCmerAligner.OptimalFlag;
            }

            // If the length in first sequence exceeds maximum length then extend 
            // till score is optimized irrespective of length.
            if ((currentAlignment.FirstSequenceStart - targetReference + 1) > NUCmerAligner.MaximumAlignmentLength)
            {
                targetReference = currentAlignment.FirstSequenceStart - NUCmerAligner.MaximumAlignmentLength + 1;
                isOverflow = true;
                methodName |= NUCmerAligner.OptimalFlag;
            }

            // If the length in second sequence exceeds maximum length then extend 
            // till score is optimized irrespective of length.
            if ((currentAlignment.SecondSequenceStart - targetQuery + 1) > NUCmerAligner.MaximumAlignmentLength)
            {
                targetQuery = currentAlignment.SecondSequenceStart = NUCmerAligner.MaximumAlignmentLength + 1;
                if (!isOverflow)
                {
                    isOverflow = true;
                }

                methodName |= NUCmerAligner.OptimalFlag;
            }

            // Extend the sequence to previous sequence (aligned/extended sequence)
            isClusterExtended = nucmerAligner.ExtendSequence(
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
                    NUCmerAligner.ForcedForwardAlignFlag);

                targetAlignment.FirstSequenceEnd = currentAlignment.FirstSequenceEnd;
                targetAlignment.SecondSequenceEnd = currentAlignment.SecondSequenceEnd;
            }
            else
            {
                startReference = currentAlignment.FirstSequenceStart;
                startQuery = currentAlignment.SecondSequenceStart;
                nucmerAligner.ExtendSequence(
                    referenceSequence,
                    targetReference,
                    ref startReference,
                    querySequence,
                    targetQuery,
                    ref startQuery,
                    currentAlignment.Deltas,
                    NUCmerAligner.ForcedForwardAlignFlag);

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
                int targetReference,
                int targetQuery,
                int methodName)
        {
            int referenceDistance;
            int queryDistance;
            int diagonal;
            bool isClusterExtended;
            bool isOverflow = false;
            bool isDouble = false;

            diagonal = currentAlignment.Deltas.Count;

            referenceDistance = targetReference - currentAlignment.FirstSequenceEnd + 1;
            queryDistance = targetQuery - currentAlignment.SecondSequenceEnd + 1;

            // If the length in first sequence exceeds maximum length then extend 
            // till score is optimized irrespective of length.
            if (referenceDistance > NUCmerAligner.MaximumAlignmentLength)
            {
                targetReference = currentAlignment.FirstSequenceEnd + NUCmerAligner.MaximumAlignmentLength + 1;
                isOverflow = true;
                methodName |= NUCmerAligner.OptimalFlag;
            }

            // If the length in second sequence exceeds maximum length then extend 
            // till score is optimized irrespective of length.
            if (queryDistance > NUCmerAligner.MaximumAlignmentLength)
            {
                targetQuery = currentAlignment.SecondSequenceEnd + NUCmerAligner.MaximumAlignmentLength + 1;
                if (isOverflow)
                {
                    isDouble = true;
                }
                else
                {
                    isOverflow = true;
                }

                methodName |= NUCmerAligner.OptimalFlag;
            }

            if (isDouble)
            {
                methodName &= ~NUCmerAligner.SeqendFlag;
            }

            // Extend the sequence to next sequence (aligned/extended sequence)
            isClusterExtended = nucmerAligner.ExtendSequence(
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
            DeltaAlignment deltaAlignment = null;
            int alignmentFirstEnd, alignmentSecondEnd, gapHigh, gapLow;
            int alignmentFirstStart = currentAlignment.FirstSequenceStart;
            int alignmentSecondStart = currentAlignment.SecondSequenceStart;
            int distance = (alignmentFirstStart < alignmentSecondStart)
                    ? alignmentFirstStart
                    : alignmentSecondStart;

            deltaAlignment = null;
            foreach (DeltaAlignment alignment in alignments)
            {
                if (currentAlignment.QueryDirection == alignment.QueryDirection)
                {
                    alignmentFirstEnd = alignment.FirstSequenceEnd;
                    alignmentSecondEnd = alignment.SecondSequenceEnd;

                    if (alignmentFirstEnd <= alignmentFirstStart
                        && alignmentSecondEnd <= alignmentSecondStart)
                    {
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
                ref int targetReference,
                ref int targetQuery)
        {
            int firstSequenceEnd;
            int secondSequenceEnd;
            int gapHigh;
            int gapLow;
            int firstSequenceStart = currentCluster.Matches.Last().FirstSequenceStart
                + currentCluster.Matches.Last().Length - 1;
            int secondSequenceStart = currentCluster.Matches.Last().SecondSequenceStart
                + currentCluster.Matches.Last().Length - 1;

            int distance = (targetReference - firstSequenceStart < targetQuery - secondSequenceStart)
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
                    firstSequenceEnd = clusterIterator.Matches.First().FirstSequenceStart;
                    secondSequenceEnd = clusterIterator.Matches.First().SecondSequenceStart;

                    if ((firstSequenceEnd < firstSequenceStart)
                            && (clusterIterator.Matches.Last().FirstSequenceStart >= firstSequenceStart)
                            && (clusterIterator.Matches.Last().SecondSequenceStart >= secondSequenceStart))
                    {
                        foreach (MaxUniqueMatchExtension match in clusterIterator.Matches)
                        {
                            if ((firstSequenceEnd < firstSequenceStart)
                                    || (secondSequenceEnd < secondSequenceStart))
                            {
                                firstSequenceEnd = match.FirstSequenceStart;
                                secondSequenceEnd = match.SecondSequenceStart;
                            }
                        }
                    }

                    if ((firstSequenceEnd >= firstSequenceStart)
                            && (secondSequenceEnd >= secondSequenceStart))
                    {
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

        #endregion
    }
}