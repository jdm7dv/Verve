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
using System.Linq;
using System.Threading.Tasks;
using MBF.Algorithms.Assembly.Graph;

namespace MBF.Algorithms.Assembly.PaDeNA
{
    /// <summary>
    /// Class implements algorithm for extracting contig sequences from de 
    /// bruijn graph. It detects simple paths in graph, and for each simple 
    /// path in the graph, it generates corresponding sequence as a contig.
    /// </summary>
    public class SimplePathContigBuilder : IContigBuilder, ILowCoverageContigPurger
    {
        /// <summary>
        /// Holds reference to assembler graph
        /// </summary>
        private DeBruijnGraph _graph;

        /// <summary>
        /// Holds value of the coverage threshold to be
        /// used for filtering contigs.
        /// </summary>
        private double _coverageThreshold = -1;

        /// <summary>
        /// Build contigs from graph. For contigs whose coverage is less than 
        /// the specified threshold, remove graph nodes belonging to them.
        /// </summary>
        /// <param name="graph">DeBruijn Graph</param>
        /// <param name="coverageThreshold">Coverage Threshold for contigs</param>
        /// <returns>Number of nodes removed</returns>
        public int RemoveLowCoverageContigs(DeBruijnGraph graph, double coverageThreshold)
        {
            if (coverageThreshold <= 0)
                throw new ArgumentException("For removing low coverage contigs, coverage threshold should be a positive number");

            _coverageThreshold = coverageThreshold;
            _graph = graph;
            DeBruijnGraph.ValidateGraph(_graph);
            ExcludeAmbiguousExtensions();
            _graph.Nodes.AsParallel().ForAll(n => n.ComputeValidExtensions());
            GetSimplePaths();
            _graph.Nodes.AsParallel().ForAll(n => n.UndoAmbiguousExtensions());
            return _graph.Nodes.RemoveWhere(n => n.IsMarked());
        }

        /// <summary>
        /// Build contig sequences from the graph.
        /// </summary>
        /// <param name="graph">De Bruijn graph</param>
        /// <returns>List of contig data</returns>
        public IList<ISequence> Build(DeBruijnGraph graph)
        {
            _graph = graph;
            _coverageThreshold = -1;
            DeBruijnGraph.ValidateGraph(_graph);
            ExcludeAmbiguousExtensions();
            _graph.Nodes.AsParallel().ForAll(n => n.PurgeInvalidExtensions());
            return GetSimplePaths();
        }

        /// <summary>
        /// For nodes that have more than one extension in either direction,
        /// mark the extensions invalid. For nodes that have palidromic sequence, 
        /// all extensions are marked invalid. This is because for a palidromic sequence, 
        /// left and right extensions are inter-changable and this causes ambiguity.
        /// Locks: No locks used as extensions are only marked invalid, not deleted.
        /// Write locks not used because in only possible conflict both threads will 
        /// try to write same value to memory. So race is harmless.
        /// </summary>
        private void ExcludeAmbiguousExtensions()
        {
            Parallel.ForEach(_graph.Nodes, node =>
            {
                // Palindromes cause small cycles in the graph. Such reference cycles will 
                // be skipped by the contig extension algorithm. Hence, in order to terminate 
                // contig extension at these points, we remove extensions from palindromic nodes.
                // Reference: ABySS Release Notes 1.0.2 - "Terminate contig extensions at palindromic kmers"
                bool isPalindrome = IsPalindrome(node);

                if (isPalindrome || node.LeftExtensionNodes.Count > 1)
                {
                    // Ambiguous. Remove all extensions
                    foreach (DeBruijnNode left in node.LeftExtensionNodes.Keys)
                    {
                        left.MarkExtensionInvalid(node);
                        node.LeftExtensionNodes[left].IsValid = false;
                    }
                }
                else
                {
                    // Remove self loops
                    if (node.LeftExtensionNodes.Count == 1 && node.LeftExtensionNodes.First().Key == node)
                    {
                        node.LeftExtensionNodes[node].IsValid = false;
                    }
                }

                if (isPalindrome || node.RightExtensionNodes.Count > 1)
                {
                    // Ambiguous. Remove all extensions
                    foreach (DeBruijnNode right in node.RightExtensionNodes.Keys)
                    {
                        right.MarkExtensionInvalid(node);
                        node.RightExtensionNodes[right].IsValid = false;
                    }
                }
                else
                {
                    // Remove self loops
                    if (node.RightExtensionNodes.Count == 1 && node.RightExtensionNodes.First().Key == node)
                    {
                        node.RightExtensionNodes[node].IsValid = false;
                    }
                }
            });
        }

        /// <summary>
        /// Checks if the sequence in the node is a palindrome.
        /// A sequence is palindrome if it is same as its reverse complement.
        /// Reference: http://en.wikipedia.org/wiki/Palindromic_sequence
        /// </summary>
        /// <param name="node">DeBruijn graph node</param>
        /// <returns>Boolean indicating if node represents palidromic sequence</returns>
        private bool IsPalindrome(DeBruijnNode node)
        {
            ISequence seq = _graph.GetNodeSequence(node);
            return string.CompareOrdinal(seq.ToString(), seq.ReverseComplement.ToString()) == 0;
        }

        /// <summary>
        /// Get simple paths in the graph
        /// </summary>
        /// <returns>List of simple paths</returns>
        private List<ISequence> GetSimplePaths()
        {
            List<ISequence> paths = new List<ISequence>();
            Parallel.ForEach(_graph.Nodes, node =>
            {
                int validLeftExtensionsCount, validRightExtensionsCount;
                validLeftExtensionsCount = node.LeftExtensionNodes.Count;
                validRightExtensionsCount = node.RightExtensionNodes.Count;

                if (validLeftExtensionsCount + validRightExtensionsCount == 0)
                {
                    // Island. Check coverage
                    if (_coverageThreshold == -1)
                    {
                        lock (paths)
                        {
                            paths.Add(_graph.GetNodeSequence(node));
                        }
                    }
                    else
                    {
                        if (node.KmerCount < _coverageThreshold)
                            node.MarkNode();
                    }
                }
                else if (validLeftExtensionsCount == 1 && validRightExtensionsCount == 0)
                {
                    TraceSimplePath(paths, node, false);
                }
                else if (validRightExtensionsCount == 1 && validLeftExtensionsCount == 0)
                {
                    TraceSimplePath(paths, node, true);
                }
            });

            return paths;
        }

        /// <summary>
        /// Trace simple path starting from 'node' in specified direction.
        /// </summary>
        /// <param name="assembledContigs">List of assembled contigs</param>
        /// <param name="node">Starting node of contig path</param>
        /// <param name="isForwardDirection">Boolean indicating direction of path</param>
        private void TraceSimplePath(List<ISequence> assembledContigs, DeBruijnNode node, bool isForwardDirection)
        {
            ISequence nodeSequence = _graph.GetNodeSequence(node);
            Sequence contigSequence = new Sequence(nodeSequence.Alphabet, nodeSequence.ToString());
            contigSequence.IsReadOnly = false;

            List<DeBruijnNode> contigPath = new List<DeBruijnNode> { node };
            KeyValuePair<DeBruijnNode, DeBruijnEdge> nextNode =
                isForwardDirection ? node.RightExtensionNodes.First() : node.LeftExtensionNodes.First();
            TraceSimplePathLinks(contigPath, contigSequence, isForwardDirection, nextNode.Value.IsSameOrientation, nextNode.Key);

            // Check to remove duplicates
            if (string.CompareOrdinal(
                _graph.GetNodeSequence(contigPath[0]).ToString(),
                _graph.GetNodeSequence(contigPath.Last()).ToString()) >= 0)
            {
                // Check contig coverage
                if (_coverageThreshold != -1)
                {
                    // Definition from Velvet Manual: http://helix.nih.gov/Applications/velvet_manual.pdf
                    // "k-mer coverage" is how many times a k-mer has been seen among the reads.
                    double coverage = contigPath.Average(n => n.KmerCount);
                    if (coverage < _coverageThreshold)
                    {
                        contigPath.ForEach(n => n.MarkNode());
                        return;
                    }
                }
                else
                {
                    lock (assembledContigs)
                    {
                        assembledContigs.Add(contigSequence);
                    }
                }
            }
        }

        /// <summary>
        /// Trace simple path in specified direction
        /// </summary>
        /// <param name="contigPath">List of graph nodes corresponding to contig path</param>
        /// <param name="contigSequence">Sequence of contig being assembled</param>
        /// <param name="isForwardDirection">Boolean indicating direction of path</param>
        /// <param name="sameOrientation">Path orientation</param>
        /// <param name="node">Next node on the path</param>
        private void TraceSimplePathLinks(
            List<DeBruijnNode> contigPath,
            ISequence contigSequence,
            bool isForwardDirection,
            bool sameOrientation,
            DeBruijnNode node)
        {
            Dictionary<DeBruijnNode, DeBruijnEdge> sameDirectionExtensions;

            bool endFound = false;
            while (!endFound)
            {
                // Get extensions going in same directions.
                sameDirectionExtensions = (isForwardDirection ^ sameOrientation) ?
                    node.LeftExtensionNodes : node.RightExtensionNodes;

                if (sameDirectionExtensions.Count == 0)
                {
                    // Found end of path. Add this and return
                    CheckAndAddNode(contigPath, contigSequence, node, isForwardDirection, sameOrientation);
                    endFound = true;
                }
                else
                {
                    var sameDirectionExtension = sameDirectionExtensions.First();

                    // (sameDirectionExtensions == 1 && oppDirectionExtensions == 1)
                    // Continue traceback in the same direction. Add this node to list and continue.
                    if (!CheckAndAddNode(contigPath, contigSequence, node, isForwardDirection, sameOrientation))
                    {
                        // Loop is found. Cannot extend simple path further 
                        break;
                    }
                    else
                    {
                        node = sameDirectionExtension.Key;
                        sameOrientation =
                            !(sameOrientation ^ sameDirectionExtension.Value.IsSameOrientation);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if 'node' can be added to 'path' without causing a loop.
        /// If yes, adds node to path and returns true. If not, returns false.
        /// </summary>
        /// <param name="contigPath">List of graph nodes corresponding to contig path</param>
        /// <param name="contigSequence">Sequence of contig being assembled</param>
        /// <param name="nextNode">Next node on the path to be addded</param>
        /// <param name="isForwardDirection">Boolean indicating direction</param>
        /// <param name="isSameOrientation">Boolean indicating orientation</param>
        /// <returns>Boolean indicating if path was updated successfully</returns>
        private bool CheckAndAddNode(
            List<DeBruijnNode> contigPath,
            ISequence contigSequence,
            DeBruijnNode nextNode,
            bool isForwardDirection,
            bool isSameOrientation)
        {
            if (contigPath.Contains(nextNode))
            {
                // there is a loop in this link
                // Return false indicating no update has been made
                return false;
            }
            else
            {
                // Add node to contig list
                contigPath.Add(nextNode);

                // Update contig sequence with sequence from next node
                ISequence nextSequence = isSameOrientation ?
                    _graph.GetNodeSequence(nextNode)
                    : _graph.GetNodeSequence(nextNode).ReverseComplement;
                if (isForwardDirection)
                {
                    contigSequence.Add(nextSequence.Last());
                }
                else
                {
                    contigSequence.Insert(0, nextSequence.First());
                }

                return true;
            }
        }
    }
}
