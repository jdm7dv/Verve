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
using Bio.Algorithms.Assembly.Graph;

namespace Bio.Algorithms.Assembly.PaDeNA.Scaffold
{
    /// <summary>
    /// Stores information about the scaffold paths.
    /// </summary>
    public class ScaffoldPath : List<KeyValuePair<DeBruijnNode, DeBruijnEdge>>
    {
        /// <summary>
        /// Converts the scaffold path into its sequence.
        /// </summary>
        /// <param name="graph">De Bruijn graph.</param>
        /// <param name="kmerLength">Kmer Length.</param>
        /// <returns>Scaffold Sequence.</returns>
        public ISequence BuildSequenceFromPath(DeBruijnGraph graph, int kmerLength)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }

            DeBruijnNode startNode = this[0].Key;
            bool isForwardDirection = this[0].Value.IsSameOrientation;
            startNode.MarkNodeAsVisited();
            ISequence scaffoldSequence = new Sequence(Alphabets.DNA);
            scaffoldSequence.InsertRange(0, graph.GetNodeSequence(startNode).ToString());
            this.RemoveAt(0);

            // There is overlap of (k-1) symbols between adjacent contigs
            if (kmerLength > 1)
            {
                kmerLength--;
            }

            bool sameOrientation = true;
            ISequence nextNodeSequence;
            foreach (KeyValuePair<DeBruijnNode, DeBruijnEdge> extensions in this)
            {
                sameOrientation = !(sameOrientation ^ extensions.Value.IsSameOrientation);
                nextNodeSequence = sameOrientation ? graph.GetNodeSequence(extensions.Key) :
                    graph.GetNodeSequence(extensions.Key).ReverseComplement;

                // Extend scaffold sequence using symbols from contig beyond the overlap
                if (isForwardDirection)
                {
                    scaffoldSequence.InsertRange(scaffoldSequence.Count,
                        nextNodeSequence.Range(kmerLength, nextNodeSequence.Count - kmerLength).ToString());
                }
                else
                {
                    scaffoldSequence.InsertRange(0,
                        nextNodeSequence.Range(0, nextNodeSequence.Count - kmerLength).ToString());
                }

                extensions.Key.MarkNodeAsVisited();
            }

            return scaffoldSequence;
        }
    }
}
