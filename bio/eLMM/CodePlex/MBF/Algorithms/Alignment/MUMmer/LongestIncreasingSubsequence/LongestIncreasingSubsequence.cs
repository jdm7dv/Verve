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



using System;
using System.Collections.Generic;
using System.Linq;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// This class implements an algorithm to find the longest increasing
    /// subsequence from the list of MUMs. In the process 
    ///     1. Removes the criss-cross mums.
    ///     2. Removes the overlapping portion of MUM by trimming the appropriate MUM.
    /// </summary>
    public class LongestIncreasingSubsequence : ILongestIncreasingSubsequence
    {
        #region ILongestIncreasingSubsequence Members

        /// <summary>
        /// Find the longest increasing sub sequence from the given set of MUMs
        /// </summary>
        /// <param name="sortedMums">List of sorted MUMs</param>
        /// <returns>Longest Increasing Subsequence</returns>
        public IList<MaxUniqueMatch> GetLongestSequence(IList<MaxUniqueMatch> sortedMums)
        {
            MaxUniqueMatchExtension[] matches = ConvertToMUMExtension(sortedMums);

            for (var counteri = 0; counteri < matches.Length; counteri++)
            {
                var matches_i = matches[counteri];
                // Initialize the MUM Extension
                matches_i.Score = matches[counteri].Length;
                matches_i.WrapScore = matches[counteri].Length;
                matches_i.Adjacent = 0;
                matches_i.From = -1;

                for (var counterj = 0; counterj < counteri; counterj++)
                {
                    MaxUniqueMatchExtension matches_j = matches[counterj];
                    // Find the overlap in query sequence of MUM

                    var overlap2 = matches_j.SecondSequenceStart + matches_j.Length;

                    overlap2 -= matches_i.SecondSequenceStart;
                    var overlap = overlap2 > 0 ? overlap2 : 0;

                    // Calculate the score for query sequence of MUM
                    var score = matches_j.Score
                                + matches_i.Length
                                - overlap;
                    if (score > matches_i.WrapScore)
                    {
                        matches_i.WrapScore = score;
                    }

                    // Find the overlap in reference sequence of MUM
                    var overlap1 = matches_j.FirstSequenceStart
                                   + matches_j.Length
                                   - matches_i.FirstSequenceStart;

                    overlap = overlap > overlap1 ? overlap : overlap1;

                    score = matches_j.Score
                            + matches_i.Length
                            - overlap;
                    if (score > matches_i.Score)
                    {
                        // To remove crosses, mark counteri as next MUM From counterj
                        // without any crosses
                        matches_i.From = counterj;

                        // Set the new score and overlap after removing the cross
                        matches_i.Score = score;
                        matches_i.Adjacent = overlap;
                    }

                    // Calculate the score for reference sequence of MUM
                    score = matches_j.WrapScore
                            + matches_i.Length
                            - overlap;
                    if (score >= matches_i.WrapScore)
                    {
                        matches_i.WrapScore = score;
                    }
                }
            }

            // Find the best longest increasing subsequence
            // Sequence with highest score is the longest increasing subsequence
            var best = 0;
            var bestScore = matches[best].Score;
            for (var counteri = 1; counteri < matches.Length; counteri++)
            {
                if (matches[counteri].Score > bestScore)
                {
                    best = counteri;
                    bestScore = matches[best].Score;
                }
            }

            // Mark the MUMs in longest increasing subsequence as "Good"
            for (var counteri = best; counteri >= 0; counteri = matches[counteri].From)
            {
                matches[counteri].IsGood = true;
            }

            // Clear the list
            // Perform the adjustment to the MUMs in longest increasing subsequence (remove over)
            // Add it the list
            sortedMums.Clear();
            foreach (var t in matches)
            {
                if (t.IsGood)
                {
                    var adjacent = t.Adjacent;
                    if (0 != adjacent)
                    {
                        t.FirstSequenceStart += adjacent;
                        t.SecondSequenceStart += adjacent;
                        t.Length -= adjacent;
                    }

                    if (0 < t.Length)
                    {
                        sortedMums.Add((MaxUniqueMatch)t);
                    }
                }
            }

            // Return the list of MUMs that represent the longest increasing subsequence
            return sortedMums;
        }

        #endregion

        /// <summary>
        /// Convert given list of MUMs to MaxUniqueMatchExtension
        /// </summary>
        /// <param name="sortedMums">List of MUMs</param>
        /// <returns>List of MaxUniqueMatchExtension</returns>
        private static MaxUniqueMatchExtension[] ConvertToMUMExtension(
            IList<MaxUniqueMatch> sortedMums)
        {
            return sortedMums.Select(mum => new MaxUniqueMatchExtension(mum)).ToArray();
        }
    }
}
