// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBF.Algorithms.Assembly.Graph;

namespace MBF.Algorithms.Assembly.PaDeNA
{
    /// <summary>
    /// Dangling links are caused by errors occuring at the end of read.
    /// This class implements the methods for detecting dangling links
    /// and removing the nodes on dangling links from the graph.
    /// This also implements graph erosion, where ends of graph which 
    /// have low coverage are removed. 
    /// </summary>
    public class DanglingLinksPurger : IGraphErrorPurger, IGraphEndsEroder
    {
        #region Fields, Constructor, Properties
        /// <summary>
        /// Threshold length for dangling links 
        /// </summary>
        private int _lengthThreshold;

        /// <summary>
        /// User Input Parameter
        /// Threshold for eroding low coverage ends
        /// </summary>
        private int _erodeThreshold;
        
        /// <summary>
        /// Field used to keep track of the minimum length of 
        /// dangling link that crosses the length threshold.
        /// This is used to update the threshold for next round.
        /// </summary>
        private SortedSet<int> _danglingLinkLengths;

        /// <summary>
        /// Tasks queued to extend dangling links following
        /// graph clean-up after erosion.
        /// </summary>
        private List<Task<int>> _danglingLinkExtensionTasks;

        /// <summary>
        /// Initializes a new instance of the DanglingLinksPurger class.
        /// </summary>
        /// <param name="threshold">Threshold for dangling links</param>
        /// <param name="erodeThreshold">Threshold for eroding endpoints</param>
        public DanglingLinksPurger(int threshold = 0, int erodeThreshold = -1)
        {
            _lengthThreshold = threshold;
            _erodeThreshold = erodeThreshold;
        }

        /// <summary>
        /// Gets the name of the algorithm
        /// </summary>
        public string Name
        {
            get { return Properties.Resource.DanglingLinksPurger; }
        }

        /// <summary>
        /// Gets the description of algorithm
        /// </summary>
        public string Description
        {
            get { return Properties.Resource.DanglingLinksPurgerDescription; }
        }

        /// <summary>
        /// Gets or sets the threshold length
        /// </summary>
        public int LengthThreshold
        {
            get { return _lengthThreshold; }
            set { _lengthThreshold = value; }
        }

        #endregion

        /// <summary>
        /// Erode ends of graph that have coverage less than given erodeThreshold.
        /// As optimization, we also check for dangling links and keeps track of the
        /// lengths of the links found. No removal is done at this step.
        /// This is done to get an idea of the different lengths at 
        /// which to run the dangling links purger step.
        /// This method returns the lengths of dangling links found.
        /// Locks: Method only does reads. No locking necessary here. 
        /// </summary>
        /// <param name="graph">Input graph</param>
        /// <param name="erodeThreshold">Threshold for erosion</param>
        /// <returns>List of lengths of dangling links detected</returns>
        public IEnumerable<int> ErodeGraphEnds(DeBruijnGraph graph, int erodeThreshold = -1)
        {
            if (graph == null)
            {
                throw new ArgumentNullException("graph");
            }

            _erodeThreshold = erodeThreshold;
            _danglingLinkLengths = new SortedSet<int>();
            _danglingLinkExtensionTasks = new List<Task<int>>();
            ICollection<DeBruijnNode> graphNodes = graph.Nodes;

            do
            {
                // Make graphNodes into an Array so that Range Partitioning can be used.
                DeBruijnNode[] graphNodesList = graphNodes.ToArray();
                int rangeSize = (int)Math.Ceiling((float)graph.Nodes.Count / Environment.ProcessorCount);

                if (rangeSize != 0 && graphNodes.Count != 0)
                {
                    _danglingLinkLengths.UnionWith(
                        Partitioner.Create(0, graphNodesList.Length, rangeSize).AsParallel().SelectMany(chunk =>
                        {
                            SortedSet<int> linkLengths = new SortedSet<int>();
                            for (int i = chunk.Item1; i < chunk.Item2; i++)
                            {
                                DeBruijnNode node = graphNodesList[i];
                                if (node.ExtensionsCount == 0)
                                {
                                    if (_erodeThreshold != -1 && node.KmerCount < _erodeThreshold)
                                    {
                                        // Mark node for erosion
                                        node.MarkNode();
                                    }
                                    else
                                    {
                                        // Single node island
                                        linkLengths.Add(1);
                                    }
                                }
                                else if (node.RightExtensionNodes.Count == 0)
                                {
                                    // End of possible dangling link
                                    // Traceback to see if it is part of a dangling link
                                    DeBruijnPath link = TraceDanglingExtensionLink(false, new DeBruijnPath(), node, true);
                                    if (link != null && link.PathNodes.Count > 0)
                                    {
                                        linkLengths.Add(link.PathNodes.Count);
                                    }
                                }
                                else if (node.LeftExtensionNodes.Count == 0)
                                {
                                    // End of possible dangling link
                                    // Traceback to see if it is part of a dangling link
                                    DeBruijnPath link = TraceDanglingExtensionLink(true, new DeBruijnPath(), node, true);
                                    if (link != null && link.PathNodes.Count > 0)
                                    {
                                        linkLengths.Add(link.PathNodes.Count);
                                    }
                                }
                            }
                            return linkLengths;
                        }));

                    // Remove eroded nodes. In the out paranter, get the list of new 
                    // end-points that was created by removing eroded nodes.
                    RemoveErodedNodes(graph, out graphNodes);
                }

            } while (graphNodes != null && graphNodes.Count > 0);

            _erodeThreshold = -1;
            ExtendDanglingLinks();
            return _danglingLinkLengths;
        }

        /// <summary>
        /// Detect nodes that are part of dangling links 
        /// Locks: Method only does reads. No locking necessary here or its callees. 
        /// </summary>
        /// <param name="graph">Input graph</param>
        /// <returns>List of nodes in dangling links</returns>
        public DeBruijnPathList DetectErroneousNodes(DeBruijnGraph graph)
        {
            if (graph == null)
                throw new ArgumentNullException("graph");

            DeBruijnNode[] graphNodesArray = graph.Nodes.ToArray();
            int rangeSize = (int)Math.Ceiling((float)graphNodesArray.Length / Environment.ProcessorCount);
            
            DeBruijnPathList danglingNodesList = new DeBruijnPathList(
                Partitioner.Create(0, graphNodesArray.Length, rangeSize).AsParallel().SelectMany(chunk =>
                {
                    List<DeBruijnPath> danglingLinks = new List<DeBruijnPath>();
                    for (int i = chunk.Item1; i < chunk.Item2; i++)
                    {
                        DeBruijnNode node = graphNodesArray[i];
                        if (node.ExtensionsCount == 0)
                        {
                            // Single node island
                            danglingLinks.Add(new DeBruijnPath(node));
                        }
                        else if (node.RightExtensionNodes.Count == 0)
                        {
                            // End of possible dangling link
                            // Traceback to see if it is part of a dangling link
                            var link = TraceDanglingExtensionLink(false, new DeBruijnPath(), node, true);
                            if (link != null)
                                danglingLinks.Add(link);
                        }
                        else if (node.LeftExtensionNodes.Count == 0)
                        {
                            // End of possible dangling link
                            // Traceback to see if it is part of a dangling link
                            var link = TraceDanglingExtensionLink(true, new DeBruijnPath(), node, true);
                            if (link != null)
                                danglingLinks.Add(link);
                        }
                    }
                    return danglingLinks;
                }));

            return danglingNodesList;
        }

        /// <summary>
        /// Removes nodes that are part of dangling links 
        /// </summary>
        /// <param name="graph">Input graph</param>
        /// <param name="nodesList">List of dangling link nodes</param>
        public void RemoveErroneousNodes(DeBruijnGraph graph, DeBruijnPathList nodesList)
        {
            // Arugument Validation
            if (graph == null)
                throw new ArgumentNullException("graph");

            if (nodesList == null)
            {
                throw new ArgumentNullException("nodesList");
            }

            HashSet<DeBruijnNode> lastNodes = new HashSet<DeBruijnNode>(nodesList.Paths.Select(nl => nl.PathNodes.Last()));

            // Update extensions and Delete nodes from graph
            graph.RemoveNodes(
                nodesList.Paths.AsParallel().SelectMany(nodes =>
                {
                    RemoveLinkNodes(nodes, lastNodes);
                    return nodes.PathNodes;
                }));
        }

        /// <summary>
        /// Delete nodes marked for erosion. Update adjacent nodes to update their extension tables.
        /// After nodes are deleted, some new end-points might be created. We need to check for 
        /// dangling links at these new points. This list is returned in the out parameter.
        /// </summary>
        /// <param name="graph">De Bruijn Graph</param>
        /// <param name="graphNodes">Out parameter. List of graph nodes to check for dangling links</param>
        private static void RemoveErodedNodes(DeBruijnGraph graph, out ICollection<DeBruijnNode> graphNodes)
        {
            int eroded = graph.Nodes.RemoveWhere(n => n.IsMarked());
            graphNodes = null;

            if (eroded > 0)
            {
                graphNodes =
                    graph.Nodes.AsParallel().Where(n =>
                    {
                        bool wasEndPoint = (n.LeftExtensionNodes.Count == 0 || n.RightExtensionNodes.Count == 0);
                        n.RemoveMarkedExtensions();
                        // Check if this is a new end point.
                        return (wasEndPoint || (n.LeftExtensionNodes.Count == 0 || n.RightExtensionNodes.Count == 0));
                    }).ToList();
            }
        }

        /// <summary>
        /// Removes nodes in link from the graph.
        /// Parallelization Note: Locks required here. We are modifying graph structure here.
        /// </summary>
        /// <param name="nodes">List of nodes to remove</param>
        /// <param name="lastNodes">Set of all nodes occuring at end of dangling links</param>
        private static void RemoveLinkNodes(DeBruijnPath nodes, HashSet<DeBruijnNode> lastNodes)
        {
            // Nodes in the list are part of a single dangling link.
            // Only the last element of link can have left or right extensions that are valid parts of graph
            DeBruijnNode linkStartNode = nodes.PathNodes.Last();

            // Update adjacency of nodes connected to the last node. 
            // Read lock not required as linkStartNode's dictionary will not get updated
            // Locks used during removal of extensions
            foreach (DeBruijnNode graphNode in
                linkStartNode.LeftExtensionNodes.Keys.Union(linkStartNode.RightExtensionNodes.Keys))
            {
                // Condition to avoid updating other linkStartNode's dictionary. Reduces conflicts.
                if (!lastNodes.Contains(graphNode))
                {
                    graphNode.RemoveExtensionThreadSafe(linkStartNode);
                }
            }
        }

        /// <summary>
        /// Starting from potential end of dangling link, trace back along 
        /// extension edges in graph to find if it is a valid dangling link.
        /// Parallelization Note: No locks used in TraceDanglingLink. 
        /// We only read graph structure here. No modifications are made.
        /// </summary>
        /// <param name="isForwardDirection">Boolean indicating direction of dangling link</param>
        /// <param name="link">Dangling Link</param>
        /// <param name="node">Node that is next on the link</param>
        /// <param name="sameOrientation">Orientation of link</param>
        /// <returns>List of nodes in dangling link</returns>
        private DeBruijnPath TraceDanglingExtensionLink(bool isForwardDirection, DeBruijnPath link, DeBruijnNode node, bool sameOrientation)
        {
            Dictionary<DeBruijnNode, DeBruijnEdge> sameDirectionExtensions, oppDirectionExtensions;

            bool reachedEndPoint = false;
            while (!reachedEndPoint)
            {
                // Get extensions going in same and opposite directions.
                if (isForwardDirection ^ sameOrientation)
                {
                    sameDirectionExtensions = node.LeftExtensionNodes;
                    oppDirectionExtensions = node.RightExtensionNodes;
                }
                else
                {
                    sameDirectionExtensions = node.RightExtensionNodes;
                    oppDirectionExtensions = node.LeftExtensionNodes;
                }

                if (sameDirectionExtensions.Count == 0)
                {
                    // Found other end of dangling link
                    // Add this and return
                    return CheckAndAddDanglingNode(link, node, out reachedEndPoint);
                }
                else if (oppDirectionExtensions.Count > 1)
                {
                    // Have reached a point of ambiguity. Return list without updating it
                    if (_erodeThreshold != -1 && !node.IsMarked())
                    {
                        lock (_danglingLinkExtensionTasks)
                        {
                            _danglingLinkExtensionTasks.Add(new Task<int>((o) =>
                                ExtendDanglingLink(isForwardDirection, link, node, sameOrientation, false),
                                TaskCreationOptions.None));
                        }
                        return null;
                    }

                    return link;
                }
                else if (sameDirectionExtensions.Count > 1)
                {
                    // Have reached a point of ambiguity. Return list after updating it
                    link = CheckAndAddDanglingNode(link, node, out reachedEndPoint);
                    if (_erodeThreshold != -1 && reachedEndPoint != true && !node.IsMarked())
                    {
                        lock (_danglingLinkExtensionTasks)
                        {
                            _danglingLinkExtensionTasks.Add(new Task<int>((o) =>
                                ExtendDanglingLink(isForwardDirection, link, node, sameOrientation, true),
                                TaskCreationOptions.None));
                        }
                        return null;
                    }

                    return link;
                }
                else
                {
                    // (sameDirectionExtensions == 1 && oppDirectionExtensions == 1)
                    // Continue traceback. Add this node to that list and recurse.
                    link = CheckAndAddDanglingNode(link, node, out reachedEndPoint);
                    if (reachedEndPoint)
                    {
                        // Loop is found or threshold length has been exceeded.
                        return link;
                    }
                    else
                    {
                        node = sameDirectionExtensions.First().Key;
                        sameOrientation = !(sameOrientation ^ sameDirectionExtensions.First().Value.IsSameOrientation);
                    }
                }
            }

            return null; // code will never reach here. Valid returns happen within the while loop.
        }

        /// <summary>
        /// Checks if 'node' can be added to 'link' without 
        /// violating any conditions pertaining to dangling links.
        /// Returns null if loop is found or length exceeds threshold.
        /// Otherwise, adds node to link and returns
        /// </summary>
        /// <param name="link">Dangling link</param>
        /// <param name="node">Node to be added</param>
        /// <param name="reachedErrorEndPoint">Indicates if we have reached end of dangling link</param>
        /// <returns>Updated dangling link</returns>
        private DeBruijnPath CheckAndAddDanglingNode(DeBruijnPath link, DeBruijnNode node, out bool reachedErrorEndPoint)
        {
            if (_erodeThreshold != -1
                && link.PathNodes.Count == 0
                && node.KmerCount < _erodeThreshold)
            {
                if (node.IsMarked())
                {
                    // There is a loop in this link. No need to update link. 
                    // Set flag for end point reached as true and return.
                    reachedErrorEndPoint = true;
                    return link;
                }
                else
                {
                    node.MarkNode();
                    reachedErrorEndPoint = false;
                    return link;
                }
            }

            if (link.PathNodes.Contains(node))
            {
                // There is a loop in this link. No need to update link. 
                // Set flag for end point reached as true and return.
                reachedErrorEndPoint = true;
                return link;
            }

            if (link.PathNodes.Count >= _lengthThreshold)
            {
                // Length crosses threshold. Not a dangling link.
                // So set reached error end point as true and return null.
                reachedErrorEndPoint = true;
                return null;
            }

            // No error conditions found. Add node to link.
            reachedErrorEndPoint = false;
            link.PathNodes.Add(node);
            return link;
        }

        /// <summary>
        /// Try and extend previously terminated dangling links
        /// </summary>
        private void ExtendDanglingLinks()
        {
            if (_danglingLinkExtensionTasks != null && _danglingLinkExtensionTasks.Count > 0)
            {
                _danglingLinkExtensionTasks.ForEach(t => t.Start());
                Task.WaitAll(_danglingLinkExtensionTasks.ToArray());
                _danglingLinkLengths.UnionWith(_danglingLinkExtensionTasks.AsParallel().
                    Select(t => t.Result).AsParallel().
                    Where(l => l > 0));

                _danglingLinkExtensionTasks = null;
            }
        }

        /// <summary>
        /// Try and extend dangling links following
        /// graph clean-up after erosion.
        /// </summary>
        /// <param name="isForwardDirection">Boolean indicating direction of dangling link</param>
        /// <param name="danglingLink">Dangling Link</param>
        /// <param name="node">Node that is next on the link</param>
        /// <param name="sameOrientation">Orientation of link</param>
        /// <param name="removeLast">Boolean indicating if last node 
        /// in link has to be removed before extending</param>
        /// <returns>Length of dangling link found after extension</returns>
        private int ExtendDanglingLink(bool isForwardDirection, DeBruijnPath danglingLink, DeBruijnNode node, bool sameOrientation, bool removeLast)
        {
            if (removeLast)
                danglingLink.PathNodes.Remove(node);

            if (danglingLink.PathNodes.Count == 0)
            {
                // DanglingLink is empty. So check if node is an end-point.
                if (node.RightExtensionNodes.Count == 0)
                {
                    danglingLink = TraceDanglingExtensionLink(false, new DeBruijnPath(), node, true);
                }
                else if (node.LeftExtensionNodes.Count == 0)
                {
                    danglingLink = TraceDanglingExtensionLink(true, new DeBruijnPath(), node, true);
                }
                else
                {
                    // Not an end-point. Return length as 0
                    return 0;
                }
            }
            else
            {
                // Extend existing link
                danglingLink = TraceDanglingExtensionLink(isForwardDirection, danglingLink, node, sameOrientation);
            }

            // Return length of dangling link found
            if (danglingLink == null)
                return 0;
            else
                return danglingLink.PathNodes.Count;
        }
    }
}
