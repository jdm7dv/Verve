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
using Bio.Algorithms.Assembly.Graph;
using Bio.Algorithms.Assembly.Padena.Scaffold.ContigOverlapGraph;

namespace Bio.Algorithms.Assembly.Padena.Scaffold
{
    /// <summary>
    /// Stores information about the scaffold paths.
    /// </summary>
    public class ScaffoldPath : List<KeyValuePair<Node, Edge>>
    {
        /// <summary>
        /// Converts the scaffold path into its sequence.
        /// </summary>
        /// <param name="graph">De Bruijn graph.</param>
        /// <param name="kmerLength">Kmer Length.</param>
        /// <returns>Scaffold Sequence.</returns>
        public ISequence BuildSequenceFromPath(ContigGraph graph, int kmerLength)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }

            Node startNode = this[0].Key;
            bool isForwardDirection = this[0].Value.IsSameOrientation;
            startNode.MarkNode();
            List<byte> scaffoldSequence = new List<byte>();
            scaffoldSequence.InsertRange(0, graph.GetNodeSequence(startNode));
            this.RemoveAt(0);

            // There is overlap of (k-1) symbols between adjacent contigs
            if (kmerLength > 1)
            {
                kmerLength--;
            }

            bool sameOrientation = true;
            ISequence nextNodeSequence = null;
            foreach (KeyValuePair<Node, Edge> extensions in this)
            {
                sameOrientation = !(sameOrientation ^ extensions.Value.IsSameOrientation);
                nextNodeSequence = sameOrientation ? graph.GetNodeSequence(extensions.Key) :
                    graph.GetNodeSequence(extensions.Key).GetReverseComplementedSequence();

                // Extend scaffold sequence using symbols from contig beyond the overlap
                if (isForwardDirection)
                {
                    scaffoldSequence.InsertRange(
                        scaffoldSequence.Count,
                        nextNodeSequence.GetSubSequence(kmerLength, nextNodeSequence.Count - kmerLength));
                }
                else
                {
                    scaffoldSequence.InsertRange(0, nextNodeSequence.GetSubSequence(0, nextNodeSequence.Count - kmerLength));
                }

                extensions.Key.MarkNode();
            }

            if (nextNodeSequence == null)
            {
                return null;
            }

            return new Sequence(nextNodeSequence.Alphabet, scaffoldSequence.ToArray(), false);
        }
    }
}
