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
    /// Generates consensus of alignment (contigs) from alignment layout. 
    /// </summary>
    public static class ConsensusGeneration
    {
        /// <summary>
        /// Generates consensus sequences from alignment layout.
        /// </summary>
        /// <param name="alignmentBetweenReferenceAndReads">Input list of reads.</param>
        /// <returns>List of contigs.</returns>
        public static IEnumerable<ISequence> GenerateConsensus(IEnumerable<DeltaAlignment> alignmentBetweenReferenceAndReads)
        {
            if (alignmentBetweenReferenceAndReads == null)
            {
                throw new ArgumentNullException("alignmentBetweenReferenceAndReads");
            }

            SimpleConsensusResolver resolver = new SimpleConsensusResolver(AmbiguousDnaAlphabet.Instance);
            Dictionary<long, Sequence> outputSequences = new Dictionary<long, Sequence>();
            Dictionary<DeltaAlignment, ISequence> deltasInCurrentContig = new Dictionary<DeltaAlignment, ISequence>();
            IEnumerator<DeltaAlignment> deltaEnumerator = alignmentBetweenReferenceAndReads.GetEnumerator();

            long currentAlignmentStartOffset = 0;
            long currentIndex = 0;
            long inDeltaIndex = 0;
            DeltaAlignment lastDelta;

            List<byte> currentContig = new List<byte>();
            List<DeltaAlignment> deltasToRemove = new List<DeltaAlignment>();

            // no deltas
            if (!deltaEnumerator.MoveNext())
            {
                return outputSequences.Values;
            }

            lastDelta = deltaEnumerator.Current;
            do
            {
                // Starting a new contig
                if (deltasInCurrentContig.Count == 0)
                {
                    currentAlignmentStartOffset = lastDelta.FirstSequenceStart;
                    currentIndex = 0;
                    currentContig.Clear();
                }

                // loop through all deltas at current index and find consensus
                do
                {
                    // Proceed creating consensus till we find another delta stats aligning
                    while (lastDelta != null && lastDelta.FirstSequenceStart == currentAlignmentStartOffset + currentIndex)
                    {
                        deltasInCurrentContig.Add(lastDelta, lastDelta.QuerySequence.GetSubSequence(lastDelta.SecondSequenceStart, (lastDelta.SecondSequenceEnd - lastDelta.SecondSequenceStart) + 1));

                        // Get next delta
                        if (deltaEnumerator.MoveNext())
                        {
                            lastDelta = deltaEnumerator.Current;
                            continue; // see if new delta starts from the same offset
                        }
                        else
                        {
                            lastDelta = null;
                        }
                    }

                    byte[] symbolsAtCurrentIndex = new byte[deltasInCurrentContig.Count];
                    int symbolCounter = 0;

                    foreach (var delta in deltasInCurrentContig)
                    {
                        inDeltaIndex = currentIndex - (delta.Key.FirstSequenceStart - currentAlignmentStartOffset);
                        symbolsAtCurrentIndex[symbolCounter++] = delta.Value[inDeltaIndex];

                        if (inDeltaIndex == delta.Value.Count - 1)
                        {
                            deltasToRemove.Add(delta.Key);
                        }
                    }
                    
                    if (deltasToRemove.Count > 0)
                    {
                        foreach (var deltaToRemove in deltasToRemove)
                        {
                            deltasInCurrentContig.Remove(deltaToRemove);
                        }

                        deltasToRemove.Clear();
                    }

                    byte consensusSymbol = resolver.GetConsensus(symbolsAtCurrentIndex);
                    currentContig.Add(consensusSymbol);

                    currentIndex++;

                    // See if another delta is adjacent
                    if (deltasInCurrentContig.Count == 0 && lastDelta != null && lastDelta.FirstSequenceStart == currentAlignmentStartOffset + currentIndex)
                    {
                        deltasInCurrentContig.Add(lastDelta, lastDelta.QuerySequence.GetSubSequence(lastDelta.SecondSequenceStart, (lastDelta.SecondSequenceEnd - lastDelta.SecondSequenceStart) + 1));

                        // check next delta
                        if (deltaEnumerator.MoveNext())
                        {
                            lastDelta = deltaEnumerator.Current;
                            continue; // read next delta to see if it starts from current reference sequence offset
                        }
                        else
                        {
                            lastDelta = null;
                        }
                    }
                }
                while (deltasInCurrentContig.Count > 0);

                outputSequences.Add(currentAlignmentStartOffset, new Sequence(AmbiguousDnaAlphabet.Instance, currentContig.ToArray(), false));
            }
            while (lastDelta != null);

            return outputSequences.Values;
        }       
    }
}
