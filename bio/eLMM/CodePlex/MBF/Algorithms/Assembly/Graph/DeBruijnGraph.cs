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
using System.Threading;
using System.Threading.Tasks;
using Bio.Algorithms.Kmer;
using Bio.Util;

namespace Bio.Algorithms.Assembly.Graph
{
    /// <summary>
    /// Representation of a De Bruijn Graph.
    /// Graph is encoded as a collection of de Bruijn nodes.
    /// The nodes themselves hold the adjacency information.
    /// </summary>
    public class DeBruijnGraph : IDisposable
    {
        /// <summary>
        /// Constant used to mark node as contig start node
        /// </summary>
        private const int ContigStartNodeIndicator = -1;

        /// <summary>
        /// Constant used to mark node as contig end node
        /// </summary>
        private const int ContigEndNodeIndicator = -2;

        /// <summary>
        /// Array of DNA Symbols
        /// </summary>
        private readonly char[] DnaSymbols = new char[] { 'A', 'T', 'G', 'C' };

        /// <summary>
        /// Array containing the complement of DNA symbols 
        /// (in same order as DnaSymbols)
        /// </summary>
        private readonly char[] DnaSymbolsComplement = new char[] { 'T', 'A', 'C', 'G' };

        /// <summary>
        /// Base sequence that holds the list of input sequences.
        /// Nodes reference into base sequence for k-mers.
        /// </summary>
        private IList<ISequence> _baseSequence;

        /// <summary>
        /// List of graph nodes
        /// </summary>
        private HashSet<DeBruijnNode> _kmerNodes;

        /// <summary>
        /// Gets the list of nodes in graph.
        /// </summary>
        public HashSet<DeBruijnNode> Nodes
        {
            get { return _kmerNodes; }
        }

        /// <summary>
        /// Validate input graph.
        /// Throws exception if graph is null
        /// </summary>
        /// <param name="graph">Input graph</param>
        public static void ValidateGraph(DeBruijnGraph graph)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }
        }

        /// <summary>
        /// Gets the sequence for kmer associated with input node.
        /// Uses index and position information along with base sequence 
        /// to construct sequence. 
        /// There should be atleast one valid position in the node.
        /// Since all positions indicate the same kmer sequence, 
        /// the position information from the first kmer is used
        /// to construct the sequence
        /// </summary>
        /// <param name="node">Graph Node</param>
        /// <returns>Sequence associated with input node</returns>
        public ISequence GetNodeSequence(DeBruijnNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            // Get sequence index and validate
            int sequenceIndex = node.SequenceIndex;
            if (sequenceIndex < 0 || sequenceIndex >= _baseSequence.Count)
            {
                throw new ArgumentOutOfRangeException("node", Properties.Resource.KmerIndexOutOfRange);
            }

            // Get base sequence, position and validate
            ISequence baseSequence = _baseSequence[sequenceIndex];
            int position = node.KmerPosition;
            if (position < 0 || position + node.KmerLength > baseSequence.Count)
            {
                throw new ArgumentOutOfRangeException("node", Properties.Resource.KmerPositionOutOfRange);
            }

            if (position == 0 && baseSequence.Count == node.KmerLength)
            {
                return baseSequence;
            }

            return baseSequence.Range(position, node.KmerLength);
        }

        /// <summary>
        /// Build graph nodes and edges from list of k-mers.
        /// Creates a node for every unique k-mer (and reverse-complement) 
        /// in the read. Then, generates adjacency information between nodes 
        /// by computing pairs of nodes that have overlapping regions 
        /// between node sequences.
        /// </summary>
        /// <param name="sequences">List of input sequences</param>
        /// <param name="kmerLength">Kmer Length</param>
        public void Build(IList<ISequence> sequences, int kmerLength)
        {
            if (sequences == null)
            {
                throw new ArgumentNullException("sequences");
            }

            if (kmerLength <= 0)
            {
                throw new ArgumentException(Properties.Resource.KmerLengthShouldBePositive);
            }
            
            Dictionary<string, DeBruijnNode> kmerNodeMap = new Dictionary<string, DeBruijnNode>();
            ReaderWriterLockSlim rwLockKmerNodeMap = new ReaderWriterLockSlim();
            _baseSequence = sequences;

            using (var rcBuilder = new ThreadLocal<char[]>( () => new char[kmerLength]))
            {
                Parallel.For(0, sequences.Count, sequenceIndex =>
                {
                    ISequence sequence = sequences[sequenceIndex];
                    string kmerString, kmerReverseComplementString;

                    // Sequence 'kmer' stores the k-mer in each window.
                    // Construct each k-mer using range from sequence
                    for (int position = 0; position <= sequence.Count - kmerLength; ++position)
                    {
                        kmerString = sequence.Range(position, kmerLength).ToString();
                        kmerReverseComplementString = kmerString.GetReverseComplement(rcBuilder.Value);

                        // check if this kmer or RC of kmer already exists
                        // If not, create new node. If yes, increment count. 
                        bool newNodeCreated = false;
                        rwLockKmerNodeMap.EnterReadLock();
                        bool keyFound = kmerNodeMap.ContainsKey(kmerString) || kmerNodeMap.ContainsKey(kmerReverseComplementString);
                        rwLockKmerNodeMap.ExitReadLock();

                        if (!keyFound)
                        {
                            rwLockKmerNodeMap.EnterWriteLock();
                            if (!kmerNodeMap.ContainsKey(kmerString) && !kmerNodeMap.ContainsKey(kmerReverseComplementString))
                            {
                                kmerNodeMap.Add(kmerString, new DeBruijnNode(kmerLength, sequenceIndex, position));
                                newNodeCreated = true;
                            }
                            rwLockKmerNodeMap.ExitWriteLock();
                        }

                        if (!newNodeCreated)
                        {
                            rwLockKmerNodeMap.EnterReadLock();
                            keyFound = kmerNodeMap.ContainsKey(kmerString);
                            DeBruijnNode kmerNode = keyFound ? kmerNodeMap[kmerString] : kmerNodeMap[kmerReverseComplementString];
                            rwLockKmerNodeMap.ExitReadLock();
                            if (keyFound)
                            {
                                // If kmer already exists, increase 'count' for k-mer node.
                                kmerNode.AddKmerDataThreadSafe(false);
                            }
                            else
                            {
                                // If reverseComplement(kmer) already exists, increases reverseCount for k-mer node
                                kmerNode.AddKmerDataThreadSafe(true);
                            }
                        }
                    }
                });
            }

            _kmerNodes = new HashSet<DeBruijnNode>(kmerNodeMap.Values);

            // Add edge information
            GenerateAdjacency(kmerNodeMap, kmerLength);
        }

        /// <summary>
        /// Builds a contig graph from kmer graph using contig data information.
        /// Creates a graph node for each contig, computes adjacency 
        /// for contig graph using edge information in kmer graph.
        /// Finally, all kmer nodes are deleted from the graph.
        /// </summary>
        /// <param name="contigs">List of contig data</param>
        /// <param name="kmerLength">Kmer length</param>
        public void BuildContigGraph(IList<ISequence> contigs, int kmerLength)
        {
            if (contigs == null)
            {
                throw new ArgumentNullException("contigs");
            }

            if (kmerLength <= 0)
            {
                throw new ArgumentException(Properties.Resource.KmerLengthShouldBePositive);
            }

            // Create contig nodes
            DeBruijnNode[] contigNodes = new DeBruijnNode[contigs.Count];
            Parallel.For(0, contigs.Count, ndx => contigNodes[ndx] = new DeBruijnNode(contigs[ndx].Count, ndx));

            GenerateContigAdjacency(contigs, kmerLength, contigNodes);

            // Update graph with new nodes
            _baseSequence = new List<ISequence>(contigs);
            _kmerNodes = new HashSet<DeBruijnNode>(contigNodes);
        }

        /// <summary>
        /// Remove all nodes in input list from graph
        /// </summary>
        /// <param name="nodes">Nodes to be removed</param>
        public void RemoveNodes(IEnumerable<DeBruijnNode> nodes)
        {
            _kmerNodes.ExceptWith(nodes);
        }

        /// <summary>
        /// Generate adjacency information between contig nodes
        /// by computing overlapping regions between contig sequences.
        /// </summary>
        /// <param name="contigs">List of contig data</param>
        /// <param name="kmerLength">Kmer length</param>
        /// <param name="contigNodes">Array of contig nodes</param>
        private static void GenerateContigAdjacency(IList<ISequence> contigs, int kmerLength, DeBruijnNode[] contigNodes)
        {
            // Create dictionaries that map (k-1) left and right substrings of contigs to contig indexes.
            Dictionary<string, List<int>> leftKmerMap = new Dictionary<string, List<int>>();
            Dictionary<string, List<int>> rightKmerMap = new Dictionary<string, List<int>>();
            Parallel.For(0, contigs.Count, ndx =>
            {
                ISequence contig = contigs[ndx];
                List<int> contigIndexes;
                string kmer;

                if (contig.Count < kmerLength)
                {
                    throw new ArgumentException(Properties.Resource.KmerLengthIsTooLong);
                }

                // update left map
                kmer = contig.Range(0, kmerLength - 1).ToString();
                lock (leftKmerMap)
                {
                    if (!leftKmerMap.TryGetValue(kmer, out contigIndexes))
                    {
                        contigIndexes = new List<int>();
                        leftKmerMap.Add(kmer, contigIndexes);
                    }
                }

                lock (contigIndexes)
                {
                    contigIndexes.Add(ndx);
                }


                // update right map
                kmer = contig.Range(contig.Count - (kmerLength - 1), kmerLength - 1).ToString();
                lock (rightKmerMap)
                {
                    if (!rightKmerMap.TryGetValue(kmer, out contigIndexes))
                    {
                        contigIndexes = new List<int>();
                        rightKmerMap.Add(kmer, contigIndexes);
                    }
                }

                lock (contigIndexes)
                {
                    contigIndexes.Add(ndx);
                }
            });

            AddContigGraphEdges(contigNodes, leftKmerMap, rightKmerMap, kmerLength);
        }

        /// <summary>
        /// Checks for and adds edges between contigs 
        /// based on left, right kmer maps.
        /// </summary>
        /// <param name="contigNodes">Array of contig nodes</param>
        /// <param name="leftKmerMap">Map of left k-mer to contig nodes</param>
        /// <param name="rightKmerMap">Map of right k-mer to contig nodes</param>
        /// <param name="kmerLength">Kmer Length</param>
        private static void AddContigGraphEdges(
            DeBruijnNode[] contigNodes,
            Dictionary<string, List<int>> leftKmerMap,
            Dictionary<string, List<int>> rightKmerMap,
            int kmerLength)
        {
            // Check and add left extensions. No locks used here since each iteration works with a different contigNode.
            using (ThreadLocal<char[]> rcBuilder = new ThreadLocal<char[]>(() => new char[kmerLength - 1]))
            {
                Parallel.ForEach(leftKmerMap, leftKmer =>
                {
                    List<int> positions;
                    if (rightKmerMap.TryGetValue(leftKmer.Key, out positions))
                    {
                        foreach (int leftNodeIndex in leftKmer.Value)
                        {
                            foreach (int rightNodeIndex in positions)
                            {
                                contigNodes[leftNodeIndex].AddLeftEndExtension(contigNodes[rightNodeIndex], true);
                            }
                        }
                    }

                    if (leftKmerMap.TryGetValue(leftKmer.Key.GetReverseComplement(rcBuilder.Value), out positions))
                    {
                        foreach (int leftNodeIndex in leftKmer.Value)
                        {
                            foreach (int rightNodeIndex in positions)
                            {
                                contigNodes[leftNodeIndex].AddLeftEndExtension(contigNodes[rightNodeIndex], false);
                            }
                        }
                    }
                });
            }

            // Check and add right extensions. No locks used here since each iteration works with a different contigNode.
            using (ThreadLocal<char[]> rcBuilder = new ThreadLocal<char[]>(() => new char[kmerLength - 1]))
            {
                Parallel.ForEach(rightKmerMap, rightKmer =>
                {
                    List<int> positions;
                    if (leftKmerMap.TryGetValue(rightKmer.Key, out positions))
                    {
                        foreach (int rightNodeIndex in rightKmer.Value)
                        {
                            foreach (int leftNodeIndex in positions)
                            {
                                contigNodes[rightNodeIndex].AddRightEndExtension(contigNodes[leftNodeIndex], true);
                            }
                        }
                    }

                    if (rightKmerMap.TryGetValue(rightKmer.Key.GetReverseComplement(rcBuilder.Value), out positions))
                    {
                        foreach (int rightNodeIndex in rightKmer.Value)
                        {
                            foreach (int leftNodeIndex in positions)
                            {
                                contigNodes[rightNodeIndex].AddRightEndExtension(contigNodes[leftNodeIndex], false);
                            }
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Generate adjacency information between nodes
        /// by computing overlapping regions between sequences.
        /// </summary>
        /// <param name="kmerNodeMap">Graph Nodes mapped by sequence string</param>
        /// <param name="kmerLength">Kmer Length</param>
        private void GenerateAdjacency(Dictionary<string, DeBruijnNode> kmerNodeMap, int kmerLength)
        {
            // Nothing to do if there are no nodes.
            if (kmerNodeMap.Count == 0)
            {
                return;
            }

            // All nodes have sequences of equal length.
            // Hence, obtained once and stored for optimization.
            using (ThreadLocal<Tuple<char[], char[]>> kmerBuilders = new ThreadLocal<Tuple<char[], char[]>>
                (() => Tuple.Create<char[], char[]>(new char[kmerLength], new char[kmerLength])))
            {
                Parallel.ForEach(kmerNodeMap, nodeValue =>
                {
                    string kmerString = nodeValue.Key;
                    string kmerStringRC = GetNodeSequence(nodeValue.Value).ToString().GetReverseComplement(kmerBuilders.Value.Item1);
                    DeBruijnNode node = nodeValue.Value;

                    char[] nextKmer = kmerBuilders.Value.Item1;
                    char[] nextKmerRC = kmerBuilders.Value.Item2;

                    // Query its possible four extensions in the right (forward)
                    // If it exists, set 'right' edge information in current node

                    // The kmer sequence of right extension nodes should either start with 
                    // (k-1) length right-substring or ends with its reverse complement.
                    // Get required substring from current node's sequence, and
                    // add a dummy character to make length equal to k.
                    kmerString.CopyTo(1, nextKmer, 0, kmerLength - 1); // right sub-string
                    kmerStringRC.CopyTo(0, nextKmerRC, 1, kmerLength - 1); // reverse-complement

                    for (int i = 0; i < DnaSymbols.Length; i++)
                    {
                        nextKmer[kmerLength - 1] = DnaSymbols[i]; // replace last character with dnaChar
                        DeBruijnNode nextNode;
                        if (kmerNodeMap.TryGetValue(new string(nextKmer), out nextNode)) // check if the kmer exists
                        {
                            // Add right extension with orientation set to true
                            // Ok to use unsafe add method since each parallel thread works with a different node
                            node.AddRightEndExtension(nextNode, true);
                        }
                        else
                        {
                            nextKmerRC[0] = DnaSymbolsComplement[i];
                            if (kmerNodeMap.TryGetValue(new string(nextKmerRC), out nextNode))
                            {
                                // Add right extension with orientation set to false
                                // Ok to use unsafe add method since each parallel thread works with a different node
                                node.AddRightEndExtension(nextNode, false);
                            }
                        }
                    }

                    // Repeat above exercise for left extensions
                    // The kmer sequence of left extension nodes should either end with 
                    // (k-1) length left-substring or ends with its reverse complement.
                    // Get required substring from current node's sequence, and
                    // add a dummy character to make length equal to k.
                    kmerString.CopyTo(0, nextKmer, 1, kmerLength - 1);
                    kmerStringRC.CopyTo(1, nextKmerRC, 0, kmerLength - 1);

                    for (int i = 0; i < DnaSymbols.Length; i++)
                    {
                        nextKmer[0] = DnaSymbols[i]; // replace first character with new DNA character
                        DeBruijnNode nextNode;
                        if (kmerNodeMap.TryGetValue(new string(nextKmer), out nextNode)) // check if the kmer exists
                        {
                            // Add left extension with orientation set to true
                            node.AddLeftEndExtension(nextNode, true);
                        }
                        else
                        {
                            nextKmerRC[kmerLength - 1] = DnaSymbolsComplement[i];
                            if (kmerNodeMap.TryGetValue(new string(nextKmerRC), out nextNode))
                            {
                                // Add left extension with orientation set to false
                                node.AddLeftEndExtension(nextNode, false);
                            }
                        }
                    }
                });
            }
        }

        #region IDisposable Members
        /// <summary>
        /// Implements dispose to supress GC finalize
        /// This is done as one of the methods uses ReadWriterLockSlim
        /// which extends IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose field instances
        /// </summary>
        /// <param name="disposeManaged">If disposeManaged equals true, clean all resources</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (disposeManaged)
            {
                _kmerNodes = null;
                _baseSequence = null;
            }
        }
        #endregion
    }
}
