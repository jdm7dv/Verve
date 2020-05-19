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
using System.Linq;
using Bio.Algorithms.Alignment;
using System;

namespace Bio.Algorithms.Assembly.Comparative
{
    /// <summary>
    /// Class Layout Refiner.
    /// </summary>
    public static class LayoutRefiner
    {
        /// <summary>
        /// Refines alignment layout by taking in consideration indels (insertions and deletions) and rearrangements between two genomes. 
        /// Requires mate-pair information to resolve ambiguity.
        /// </summary>
        /// <param name="orderedDeltas">Order deltas.</param>
        public static void RefineLayout(IList<DeltaAlignment> orderedDeltas)
        {
            if (orderedDeltas == null)
            {
                throw new ArgumentNullException("orderedDeltas");
            }

            if (orderedDeltas.Count == 0)
            {
                return;
            }

            List<DeltaAlignment> deltasOverlappingAtCurrentIndex = new List<DeltaAlignment>();

            long currentProcessedOffset = 0;
            deltasOverlappingAtCurrentIndex.Add(orderedDeltas[0]);
            DeltaAlignment deltaWithLargestEndIndex = orderedDeltas[0];
            for (int currentIndex = 0; currentIndex < orderedDeltas.Count - 1; currentIndex++)
            {
                DeltaAlignment nextDelta = orderedDeltas[currentIndex + 1];
                nextDelta.FirstSequenceStart += currentProcessedOffset;
                nextDelta.FirstSequenceEnd += currentProcessedOffset;
                // Check if next delta is just adjacent
                if (nextDelta.FirstSequenceStart - 1 == deltaWithLargestEndIndex.FirstSequenceEnd) 
                {
                    // If next delta is adjacent there is a possible insertion in target (deletion in reference)
                    // Try to extend the deltas from both sides and make them meet
                    List<DeltaAlignment> leftSideDeltas = deltasOverlappingAtCurrentIndex.Where(a => a.FirstSequenceEnd >= deltaWithLargestEndIndex.FirstSequenceEnd).ToList();

                    // Find all deltas starting at the adjacent right side
                    List<DeltaAlignment> rightSideDeltas = new List<DeltaAlignment>(4);
                    rightSideDeltas.AddRange(orderedDeltas.Skip(currentIndex + 1).TakeWhile(a => a.FirstSequenceStart == nextDelta.FirstSequenceStart));

                    long offset = ExtendDeltas(leftSideDeltas, rightSideDeltas);

                    nextDelta.FirstSequenceStart += offset;
                    nextDelta.FirstSequenceEnd += offset;
                    currentProcessedOffset += offset;
                }
                else
                if (nextDelta.FirstSequenceStart <= deltaWithLargestEndIndex.FirstSequenceEnd) 
                {
                    // Check if next delta overlaps with current overlap group
                    deltasOverlappingAtCurrentIndex.Add(nextDelta);

                    // Check if nextDelta is reaching farther than the current farthest delta
                    if (nextDelta.FirstSequenceEnd > deltaWithLargestEndIndex.FirstSequenceEnd)
                    {
                        deltaWithLargestEndIndex = nextDelta;
                    }
                }
                else
                {
                    // No overlap with nextDelta, so there is a gap at the end of deltaWithLargestEndIndex
                    // Try fix insertion in reference by pulling together two ends of deltas on both sides of the gap
                    List<DeltaAlignment> leftSideDeltas = deltasOverlappingAtCurrentIndex.Where(a => a.FirstSequenceEnd >= deltaWithLargestEndIndex.FirstSequenceEnd).ToList();
                    
                    // Find all deltas starting at the right end of the gap
                    List<DeltaAlignment> rightSideDeltas = new List<DeltaAlignment>(4);
                    rightSideDeltas.AddRange(orderedDeltas.Skip(currentIndex + 1).TakeWhile(a => a.FirstSequenceStart == nextDelta.FirstSequenceStart));

                    int score = 0;
                    foreach (var l in leftSideDeltas)
                    {
                        foreach (var r in rightSideDeltas)
                        {
                            if (object.ReferenceEquals(l.QuerySequence, r.QuerySequence))
                            {
                                score++;
                                break;
                            }
                            else
                            {
                                score--;
                            }
                        }
                    }

                    // Score > 0 means most deltas share same query sequence at both ends, so close this gap
                    if (score > 0)
                    {
                        long gaplength = (nextDelta.FirstSequenceStart - deltaWithLargestEndIndex.FirstSequenceEnd) - 1;
                        currentProcessedOffset -= gaplength;

                        // Pull deltas on right side to close the gap
                        foreach (DeltaAlignment delta in rightSideDeltas)
                        {
                            delta.FirstSequenceStart -= gaplength;
                            delta.FirstSequenceEnd -= gaplength;
                        }
                    }

                    // Start a new group from the right side of the gap
                    deltaWithLargestEndIndex = nextDelta;
                    deltasOverlappingAtCurrentIndex.Clear();
                    deltasOverlappingAtCurrentIndex.Add(nextDelta);
                }
            }
        }

        /// <summary>
        /// Extended Deltas.
        /// </summary>
        /// <param name="leftSideDeltas">Left Side Deltas.</param>
        /// <param name="rightSideDeltas">Right Side Deltas.</param>
        /// <returns>Returns Extend Deltas.</returns>
        private static long ExtendDeltas(List<DeltaAlignment> leftSideDeltas, List<DeltaAlignment> rightSideDeltas)
        {
            long extendedIndex = 1;
            int[] symbolCount = new int[255];
            List<byte> leftExtension = new List<byte>();
            List<byte> rightExtension = new List<byte>();

            #region left extension

            // Left extension
            do
            {
                symbolCount['A'] = symbolCount['C'] = symbolCount['G'] = symbolCount['T'] = 0;

                // loop through all queries at current index and find symbol counts
                foreach (DeltaAlignment da in leftSideDeltas)
                {
                    if (da.QuerySequence.Count > da.SecondSequenceEnd + extendedIndex)
                    {
                        // TODO: consider lowercase uppsercase issue
                        byte symbol = da.QuerySequence[da.SecondSequenceEnd + extendedIndex];
                        symbolCount[symbol]++;
                    }
                }

                // no symbols at current position, then break;
                if (symbolCount['A'] == 0 && symbolCount['C'] == 0 && symbolCount['G'] == 0 && symbolCount['T'] == 0)
                {
                    break;
                }

                // find symbol with max occurence
                byte indexLargest, indexSecond;
                FindLargestAndSecondLargest(symbolCount, out indexLargest, out indexSecond);

                // Dont extend if largest symbol count is higher than double of second largest symbol count
                if (symbolCount[indexSecond] > symbolCount[indexLargest] / 2)
                {
                    return 0;
                }

                leftExtension.Add(indexLargest); // index will be the byte value of the appropriate symbol

                extendedIndex++;
            }while(true);

            #endregion

            #region Right extension

            // Right extension
            extendedIndex = 1;
            do
            {
                symbolCount['A'] = symbolCount['C'] = symbolCount['G'] = symbolCount['T'] = 0;

                // loop through all queries at current index and find symbol counts
                foreach (DeltaAlignment da in rightSideDeltas)
                {
                    if (da.SecondSequenceStart - extendedIndex >= 0)
                    {
                        // TODO: consider lowercase uppsercase issue
                        byte symbol = da.QuerySequence[da.SecondSequenceStart - extendedIndex];
                        symbolCount[symbol]++;
                    }
                }

                // no symbols at current position, then break;
                if (symbolCount['A'] == 0 && symbolCount['C'] == 0 && symbolCount['G'] == 0 && symbolCount['T'] == 0)
                {
                    break;
                }

                // find symbol with max occurence
                byte indexLargest, indexSecond;
                FindLargestAndSecondLargest(symbolCount, out indexLargest, out indexSecond);

                // Dont extend if largest symbol count is higher than double of second largest symbol count
                if (symbolCount[indexSecond] > symbolCount[indexLargest] / 2)
                {
                    return 0;
                }

                rightExtension.Insert(0, indexLargest); // index will be the byte value of the appropriate symbol

                extendedIndex++;
            } while (true);

            #endregion

            // One of the side cannot be extended, so cancel extension
            if (leftExtension.Count == 0 || rightExtension.Count == 0)
            {
                return 0;
            }

            int overlapStart = FindMaxOverlap(leftExtension, rightExtension);

            if (overlapStart == -1)
            {
                return 0;
            }
            else
            {
                // Update left side deltas
                foreach (var d in leftSideDeltas)
                {
                    d.FirstSequenceEnd += (d.QuerySequence.Count - 1) - d.SecondSequenceEnd;
                    d.SecondSequenceEnd = d.QuerySequence.Count - 1;
                }

                // Update right side deltas
                int toRightOffset = rightExtension.Count + overlapStart;
                foreach (var d in rightSideDeltas)
                {
                    d.FirstSequenceStart += toRightOffset - d.SecondSequenceStart;
                    d.FirstSequenceStart -= toRightOffset; // Subtracting as all these deltas will be processed in the outer loop
                    d.SecondSequenceStart = 0;
                }

                return toRightOffset;
            }
        }

        /// <summary>
        /// Find Max Overlap.
        /// </summary>
        /// <param name="leftExtension">Left Extension.</param>
        /// <param name="rightExtension">Right Extension.</param>
        /// <returns>Returns Max overLap.</returns>
        private static int FindMaxOverlap(List<byte> leftExtension, List<byte> rightExtension)
        {
            for (int left = 0; left < leftExtension.Count; left++)
            {
                int right;
                for (right = 0; right < leftExtension.Count - left; right++)
                {
                    if (leftExtension[left + right] != rightExtension[right])
                    {
                        break;
                    }
                }

                if (right == leftExtension.Count - left)
                {
                    return left;
                }
            }

            return -1;
        }

        /// <summary>
        /// Find Largest And SecondLargest.
        /// </summary>
        /// <param name="values">Set of Values.</param>
        /// <param name="indexLargest">Out param largest index.</param>
        /// <param name="indexSecond">Out param Second largest index.</param>
        private static void FindLargestAndSecondLargest(int[] values, out byte indexLargest, out byte indexSecond)
        {
            indexLargest = indexSecond = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > values[indexLargest])
                {
                    indexSecond = indexLargest;
                    indexLargest = (byte)i;
                }
            }
        }       
    }
}
