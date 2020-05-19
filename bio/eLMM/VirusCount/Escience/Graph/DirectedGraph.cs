//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBT.Escience;
using Bio.Util;

namespace MBT.Escience.Graph
{
    [Serializable]
    public class DirectedGraph : ICloneable
    {
        private Dictionary<string, GraphNode> _nodes;
        public Dictionary<string, DirectedGraphEdge> _edges { get; private set; }
        private uint _dfsOrder;
        private uint _componentNumber;
        public List<DirectedGraphEdge> EdgesList { get { return _edges.Values.ToList(); } }
        public IEnumerable<GraphNode> Nodes { get { return _nodes.Values.AsEnumerable(); } }
        public IEnumerable<DirectedGraphEdge> Edges { get { return _edges.Values.AsEnumerable(); } }

        public void UpdateNodeName(string oldLabel, string newLabel)
        {
            if (this._nodes.ContainsKey(oldLabel) && !this._nodes.ContainsKey(newLabel))
            {
                GraphNode tempNode = this._nodes[oldLabel];
                this._nodes.Remove(oldLabel);
                tempNode.Name = newLabel;
                this._nodes.Add(newLabel, tempNode);
            }
        }

        public void UpdateEdgeName(string key, string newFrom, string newTo)
        {
            if (this._edges.ContainsKey(key))
            {
                DirectedGraphEdge tempEdge = new DirectedGraphEdge(newFrom, newTo, 0);
                if (!this._edges.ContainsKey(tempEdge.Key))
                {
                    tempEdge = this._edges[key];
                    this._edges.Remove(key);
                    tempEdge.ChangeEdgeFrom(newFrom);
                    tempEdge.ChangeEdgeTo(newTo);
                    this._edges.Add(tempEdge.Key, tempEdge);
                }
            }

        }

        public void AddNode(GraphNode node)
        {
            if (!this._nodes.ContainsKey(node.Name))
            {
                this._nodes.Add(node.Name, node);
            }
        }

        /// <summary>
        /// Constructs an empty directed graph.
        /// 
        /// </summary>
        public DirectedGraph()
        {
            this._componentNumber = 0;
            this._dfsOrder = 0;
            this._nodes = new Dictionary<string, GraphNode>();
            this._edges = new Dictionary<string, DirectedGraphEdge>();
        }

        /// <summary>
        /// Constructs a directed graph.
        /// 
        /// Nodes are given by 'nodes' and edges are given by 'edges'. The constructor
        /// checks, whether an edge occurs twice. If this is the case, a GraphException
        /// is thrown.
        /// </summary>
        /// <param name="nodes">the nodes of the graph</param>
        /// <param name="edges">the edges of the graph</param>
        /// <exception cref="GraphException">An exception is thrown if an edge occurs 
        /// more than once.</exception>
        public DirectedGraph(List<GraphNode> nodes, List<DirectedGraphEdge> edges)
        {
            if (nodes == null)
            {
                throw new GraphException("Graph has to have at least one node.");
            }

            this._componentNumber = 0;
            this._dfsOrder = 0;
            this._nodes = new Dictionary<string, GraphNode>();
            this._edges = new Dictionary<string, DirectedGraphEdge>();
            try
            {
                foreach (GraphNode node in nodes)
                {
                    _nodes.Add(node.Name, node);
                }
            }
            catch (ArgumentException e)
            {
                throw new GraphException("A node occured twice: " + e.Message);
            }
            try
            {
                if (edges != null)
                {
                    foreach (DirectedGraphEdge edge in edges)
                    {
                        _edges.Add(edge.From + "," + edge.To, edge);
                    }
                }
            }
            catch (ArgumentException e)
            {
                throw new GraphException("An edge occured twice: " + e.Message);
            }
        }

        /// <summary>
        /// Removes an edge of the graph.
        /// 
        /// The method removes the edge from 'from' to 'to' if it is present.
        /// If an edge was removed, the method returns true, otherwise false.
        /// </summary>
        /// <param name="from">the node label of the node from which the edge originates</param>
        /// <param name="to">the node label of the node to which the edge points to</param>
        /// <returns>true, if an edge was removed, otherwise false</returns>
        public bool RemoveEdge(string from, string to)
        {
            return _edges.Remove(from + "," + to);
        }

        /// <summary>
        /// Adds an edge to the graph. If an edge (u,v) already exists,
        /// nothing is done. Use UpdateEdgeWeights to change the weights of existing
        /// edges. The return value of this function signalizes, whether the edge was added
        /// or not.
        /// </summary>
        /// <param name="edge"></param>
        /// <returns>true if edge was added, false otherwise</returns>
        public bool AddEdge(DirectedGraphEdge edge)
        {
            string tempKey = edge.From + "," + edge.To;
            if (!this._edges.ContainsKey(tempKey))
            {
                this._edges.Add(tempKey, edge);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Can be used to get a deep copy of this instance.
        /// </summary>
        /// <returns>a deep copy of the current instance</returns>
        public Object Clone()
        {
            List<DirectedGraphEdge> edges = new List<DirectedGraphEdge>();
            List<GraphNode> nodes = new List<GraphNode>();
            GraphNode tempNode = null;
            DirectedGraph newGraph = null;

            foreach (GraphNode node in this._nodes.Values)
            {
                tempNode = (GraphNode)node.Clone();
                nodes.Add(tempNode);
            }
            foreach (KeyValuePair<string, DirectedGraphEdge> edge in this._edges)
            {
                edges.Add((DirectedGraphEdge)edge.Value.Clone());
            }
            newGraph = new DirectedGraph(nodes, edges);
            newGraph._componentNumber = this._componentNumber;
            newGraph._dfsOrder = this._dfsOrder;

            return newGraph;
        }

        /// <summary>
        /// Returns a Graph which is the transpose of the current graph (edge (u,v) is 
        /// transformed to edge (v,u).
        /// </summary>
        /// <returns>the transpose of the current graph</returns>
        public DirectedGraph GetTranspose()
        {
            DirectedGraph transpose = (DirectedGraph)this.Clone();
            transpose.Transpose();

            return transpose;
        }

        /// <summary>
        /// Transposes the actual graph. The direction of all edges is inverted.
        /// </summary>
        public void Transpose()
        {
            foreach (DirectedGraphEdge edge in this.EdgesList)
            {
                string key = edge.From + "," + edge.To;
                string keyReverse = edge.To + "," + edge.From;
                bool occursTwice = this._edges.ContainsKey(keyReverse);
                if (occursTwice && key.CompareTo(keyReverse) < 0)
                {
                    double tempCost = this._edges[keyReverse].Cost;
                    this._edges[keyReverse].Cost = this._edges[key].Cost;
                    this._edges[key].Cost = tempCost;
                }
                else if (!occursTwice)
                {
                    this._edges.Remove(edge.From + "," + edge.To);
                    DirectedGraphEdge tempEdge = (DirectedGraphEdge)edge.Clone();
                    tempEdge.Transpose();
                    this._edges.Add(edge.To + "," + edge.From, tempEdge);
                }

                //Console.WriteLine("Reverted edge (" + edge.From + ","
                //    + edge.To + ") to edge (" + key + ").");
            }
        }

        /// <summary>
        /// computes the connected components of this graph (only components with >= 2 elements are returned)
        /// </summary>
        /// <returns>the connected components of this graph (only components with >= 2 elements are returned)</returns>
        public IEnumerable<Set<GraphNode>> GetConnectedComponents()
        {
            return (from component in this.GetConnectedComponentsInDictionary().Values
                    select component).Distinct();
        }

        /// <summary>
        /// computes the connected components of this graph (only components with >= 2 elements are returned)
        /// </summary>
        /// <returns>the connected components of this graph (only components with >= 2 elements are returned)
        /// in a Dictionary. The key corresponds to the node.Name and the Value is the set, in which the corresponding node is in.
        /// </returns>
        public Dictionary<string, Set<GraphNode>> GetConnectedComponentsInDictionary()
        {
            IEnumerable<string> fromLabels = (from edge in Edges
                                              select edge.From).Distinct();
            List<string> labels = (from edge in Edges
                                   select edge.To).Distinct().ToList();
            labels.AddRange(fromLabels);
            IEnumerable<GraphNode> connectedNodes = from node in Nodes
                                                    where labels.Contains(node.Name)
                                                    select node;

            Dictionary<string, Set<GraphNode>> connectedComponents = new Dictionary<string, Set<GraphNode>>();
            // at the beginning each node is in its own component
            foreach (GraphNode node in connectedNodes)
            {
                connectedComponents.Add(node.Name, new Set<GraphNode>(node));
            }
            Set<GraphNode> set1 = null;
            Set<GraphNode> set2 = null;
            Set<GraphNode> setToAdd = null;
            Set<GraphNode> set = null;

            // for each edge we join the components if the corresponding nodes are not already in the same component
            foreach (DirectedGraphEdge edge in Edges)
            {
                if (!Object.ReferenceEquals(connectedComponents[edge.From], connectedComponents[edge.To]))
                {
                    set1 = connectedComponents[edge.From];
                    set2 = connectedComponents[edge.To];
                    // add smaller set to bigger set
                    if (set1.Count > set2.Count)
                    {
                        set = set1;
                        setToAdd = set2;
                    }
                    else
                    {
                        set = set2;
                        setToAdd = set1;
                    }
                    set.AddNewRange(setToAdd);
                    // update references in added set
                    foreach (GraphNode tempNode in setToAdd)
                    {
                        connectedComponents[tempNode.Name] = set;
                    }
                }
            }

            return connectedComponents;
        }

        public List<List<string>> GetNodesInVStructures()
        {
            List<List<string>> components = new List<List<string>>();

            var nodesWithInDegree = from edge in Edges
                                    group edge by edge.To into g
                                    select new { NodeName = g.Key, InDegree = g.Count() };

            var nodesWithInDegreeLarger1 = from nodeWithInDegree in nodesWithInDegree
                                           where nodeWithInDegree.InDegree > 1
                                           select nodeWithInDegree.NodeName;
            foreach (var nodeWithInDegreeLarger1 in nodesWithInDegreeLarger1)
            {
                components.Add((from edge in Edges
                                where edge.To.Equals(nodeWithInDegreeLarger1)
                                select edge.From).ToList());
            }

            return components;
        }


        /// <summary>
        /// This method performs a depth first search. 
        /// 
        /// The finishing times
        /// are stored in each node (node.DfsOrder). The algorithm takes
        /// the nodes in decreasing finishing times order if 'sorted' is true. Therefore,
        /// an invokation on a transposed graph containing the finishing
        /// times of the standard graph computes the strongly connected components
        /// (stored in node.ComponentNumber). The algorithm can be limited to parts
        /// of the graph by providing component numbers via 'componentNumbersRestriction'.
        /// 
        /// </summary>
        /// <param name="componentNumbersRestriction">The algorithm can be limited to parts
        /// of the graph by providing component numbers</param>
        /// <returns></returns>
        public List<uint> DepthFirstSearch(List<uint> componentNumbersRestriction, bool sorted)
        {
            List<uint> componentNumbers = new List<uint>();
            List<GraphNode> nodeQuery = null;

            if (sorted)
            {
                if (componentNumbersRestriction != null)
                {
                    nodeQuery =
                        (from node in Nodes
                         where componentNumbersRestriction.Contains(node.ComponentNumber)
                         orderby node.DfsOrder descending
                         select node).ToList();
                }
                else
                {
                    nodeQuery =
                        (from node in Nodes
                         orderby node.DfsOrder descending
                         select node).ToList();
                }
            }
            else
            {
                if (componentNumbersRestriction != null)
                {
                    nodeQuery =
                        (from node in Nodes
                         where componentNumbersRestriction.Contains(node.ComponentNumber)
                         select node).ToList();
                }
                else
                {
                    nodeQuery =
                        (from node in Nodes
                         select node).ToList();
                }
            }

            foreach (GraphNode tempNode in nodeQuery)
            {
                tempNode.Color = GraphNodeColor.WHITE;
            }

            foreach (GraphNode node in nodeQuery)
            {
                if (node.Color == GraphNodeColor.WHITE)
                {
                    this._DepthFirstSearchVisit(node);
                    componentNumbers.Add(this._componentNumber);
                    ++this._componentNumber;
                }
            }
            return componentNumbers;
        }

        /// <summary>
        /// This is a helper function of the depth first search. It colors the node 'node',
        /// and calls the function for all nodes, which are reachable from this node
        /// and have not been visited (WHITE).
        /// 
        /// </summary>
        /// <param name="node"></param>
        private void _DepthFirstSearchVisit(GraphNode node)
        {
            node.Color = GraphNodeColor.GREY;

            var edgeQuery =
                from edge in Edges
                where edge.From.Equals(node.Name)
                select edge;

            foreach (var edge in edgeQuery)
            {
                GraphNode tempNode = this._nodes[edge.To];
                if (tempNode.ComponentNumber == node.ComponentNumber
                    && tempNode.Color == GraphNodeColor.WHITE)
                {
                    this._DepthFirstSearchVisit(tempNode);
                }
            }
            node.Color = GraphNodeColor.BLACK;
            node.DfsOrder = this._dfsOrder++;
            node.ComponentNumber = this._componentNumber;
        }

        /// <summary>
        /// This function can be used to get order restrictions imposed by the edges
        /// of this graph (graph should be a DAG). If a node x precedes a node y, 
        /// then there will be a restriction in the returned HashSet as a KeyValuePair x,y.
        /// </summary>
        /// <returns>a set containing all order restrictions</returns>
        public HashSet<Tuple<string, string>> GetOrderRestrictions()
        {
            HashSet<Tuple<string, string>> orderRestrictions = new HashSet<Tuple<string, string>>();


            this.DepthFirstSearch(null, false);
            List<GraphNode> orderedNodes =
                (from node in Nodes
                 orderby node.DfsOrder descending
                 select node).ToList();
            List<string> ancestors = new List<string>();

            foreach (GraphNode node in orderedNodes)
            {
                // We have to go all paths
                foreach (GraphNode extraNode in Nodes)
                {
                    extraNode.Color = GraphNodeColor.WHITE;
                }
                ancestors.Clear();
                ancestors.Add(node.Name);
                if (node.Color == GraphNodeColor.WHITE)
                {
                    this._getOrderRestrictionsLoop(node, ref orderRestrictions, ref ancestors);
                }
            }
            return orderRestrictions;
        }

        /// <summary>
        /// This is a help function of the GetOrderRestrictions method.
        /// </summary>
        /// <param name="node">the current node</param>
        /// <param name="orderRestrictions">the current order restrictions</param>
        /// <param name="ancestors">a list containing all ancestors of 'node'</param>
        private void _getOrderRestrictionsLoop(GraphNode node, ref HashSet<Tuple<string, string>> orderRestrictions, ref List<string> ancestors)
        {
            node.Color = GraphNodeColor.GREY;

            var edgeQuery =
                from edge in Edges
                where edge.From.Equals(node.Name)
                select edge;

            foreach (var edge in edgeQuery)
            {
                GraphNode tempNode = this._nodes[edge.To];
                if (tempNode.Color == GraphNodeColor.WHITE)
                {
                    foreach (string ancestor in ancestors)
                    {
                        orderRestrictions.Add(new Tuple<string, string>(ancestor, tempNode.Name));
                    }
                    ancestors.Add(tempNode.Name);

                    this._getOrderRestrictionsLoop(tempNode, ref orderRestrictions, ref ancestors);
                    ancestors.Remove(tempNode.Name);
                }
            }
            node.Color = GraphNodeColor.BLACK;
        }

        /// <summary>
        /// This function returns all labels of nodes that can be reached
        /// from the node with label 'nodeLabel'. If the node does not exist
        /// the null reference is returned.
        /// 
        /// </summary>
        /// <param name="nodeLabel"></param>
        /// <returns>labels of reachable nodes for node 'nodeLabel' or null if 
        /// node with label 'nodeLabel' does not exist</returns>
        public HashSet<string> GetReachableNodes(string nodeLabel)
        {
            HashSet<string> reachableNodes = new HashSet<string>();
            GraphNode node = null;

            foreach (GraphNode tempNode in this.Nodes)
            {
                tempNode.Color = GraphNodeColor.WHITE;
            }

            if (!this._nodes.ContainsKey(nodeLabel))
            {
                return null;
            }
            node = this._nodes[nodeLabel];
            this._getReachableNodesLoop(node, ref reachableNodes);

            return reachableNodes;
        }

        /// <summary>
        /// Returns the names of all ancestors of the node corresponding to 'nodeLabel'
        /// </summary>
        /// <param name="nodeLabel">the label of the node, for which the ancestors should be returned</param>
        /// <returns>ancestors of the node corresponding to 'nodeLabel'</returns>
        public HashSet<string> GetAncestors(string nodeLabel)
        {
            this.Transpose();
            HashSet<string> reachableNodes = this.GetReachableNodes(nodeLabel);
            this.Transpose();
            return reachableNodes;
        }

        public IEnumerable<string> GetNodesInDfsOrder()
        {
            IEnumerable<string> nodes = from node in this.Nodes
                                        where node.DfsOrder == 1
                                        select node.Name;
            if (nodes.Count() == 0)
            {
                this.DepthFirstSearch(null, false);
            }
            nodes = (from node in this.Nodes
                     orderby node.DfsOrder
                     select node.Name).ToList();
            return nodes;
        }

        /// <summary>
        /// Help function of the GetReachableNodes function
        /// </summary>
        /// <param name="node"></param>
        /// <param name="reachableNodes"></param>
        private void _getReachableNodesLoop(GraphNode node, ref HashSet<string> reachableNodes)
        {
            node.Color = GraphNodeColor.GREY;

            var edgeQuery =
                from edge in Edges
                where edge.From.Equals(node.Name)
                select edge;

            foreach (var edge in edgeQuery)
            {
                GraphNode tempNode = this._nodes[edge.To];
                if (tempNode.Color == GraphNodeColor.WHITE)
                {
                    reachableNodes.Add(tempNode.Name);
                    this._getReachableNodesLoop(tempNode, ref reachableNodes);
                }
            }
            node.Color = GraphNodeColor.BLACK;
        }


        /// <summary>
        /// This method computes the strongly connected components (SCCs) of this graph
        /// c.f. Cormen et al.
        /// If a node label is given (nodeName != null && nodeName != ""), the SCCs will only be 
        /// computed on nodes that are known to be in the same SCCs (previous runs). If this
        /// is the first time, the method runs on this graph all nodes are used.
        /// </summary>
        /// <param name="nodeName">restricts the calculation of SCCs to the previous SCC of this node</param>
        public void ComputeStronglyConnectedComponents(String nodeName)
        {
            List<uint> componentNumbers = null;
            bool ordered = false;

            if (nodeName != null && !nodeName.Equals(""))
            {
                if (this._nodes.ContainsKey(nodeName))
                {
                    uint componentNumber = 0;
                    componentNumber = this._nodes[nodeName].ComponentNumber;
                    componentNumbers = new List<uint>();
                    componentNumbers.Add(componentNumber);
                }
            }
            else
            {
                this.Nodes.AsParallel().ForAll(node => node.ComponentNumber = 0);
            }
            componentNumbers = this.DepthFirstSearch(componentNumbers, ordered);
            this.Transpose();
            ordered = true;
            this.DepthFirstSearch(componentNumbers, ordered);
            this.Transpose();
        }

        /// <summary>
        /// Gives the parent nodes of the node with name 'nodeName'
        /// in the same strongly connected component (SCC).
        /// This is particularly useful if you want to know, from which
        /// parent nodes in the same directed cycle, the node with 
        /// name 'nodeName' can be reached.
        /// </summary>
        /// <param name="nodeName">the name of the node for which parent nodes are searched</param>
        /// <returns>a list of parent node names which are in the same SCC</returns>
        public List<string> GetParentNodesInSameSCC(string nodeName)
        {
            if (!this._nodes.ContainsKey(nodeName))
            {
                return null;
            }
            uint componentNumber = this._nodes[nodeName].ComponentNumber;

            List<string> nodes =
                (from node in Nodes
                 from edge in Edges
                 where node.Name == edge.From
                     && edge.To == nodeName
                     && node.ComponentNumber == componentNumber
                 select node.Name).ToList();

            return nodes;

        }

        public bool AdditionalEdgeInducesDirectedCycle(DirectedGraphEdge additionalEdge)
        {
            DirectedGraph tempGraph = (DirectedGraph)this.Clone();
            tempGraph.AddEdge(additionalEdge);
            tempGraph.ComputeStronglyConnectedComponents(null);
            List<GraphNode> query = (from node1 in tempGraph.Nodes
                                     from node2 in tempGraph.Nodes
                                     where ((!node1.Name.Equals(node2.Name))
                                           && node1.ComponentNumber == node2.ComponentNumber)
                                     select node1).ToList();
            return (query != null && query.Count() > 0);
        }

        public bool EdgeReversalInducesDirectedCycle(DirectedGraphEdge edge)
        {
            DirectedGraph tempGraph = (DirectedGraph)this.Clone();
            tempGraph.RemoveEdge(edge.From, edge.To);
            tempGraph.AddEdge(new DirectedGraphEdge(edge.To, edge.From, edge.Cost));
            tempGraph.ComputeStronglyConnectedComponents(null);
            List<GraphNode> query = (from node1 in tempGraph.Nodes
                                     from node2 in tempGraph.Nodes
                                     where ((!node1.Name.Equals(node2.Name))
                                           && node1.ComponentNumber == node2.ComponentNumber)
                                     select node1).ToList();
            return (query != null && query.Count() > 0);
        }

        public bool HasDirectedCycle()
        {
            DirectedGraph tempGraph = (DirectedGraph)this.Clone();
            tempGraph.ComputeStronglyConnectedComponents(null);
            IEnumerable<GraphNode> query = (from node1 in tempGraph.Nodes
                                            from node2 in tempGraph.Nodes
                                            where ((!node1.Name.Equals(node2.Name))
                                                  && node1.ComponentNumber == node2.ComponentNumber)
                                            select node1);
            return (query != null && query.Count() > 0);
        }

        /// <summary>
        /// Collects the names of all nodes, which are in non trivial
        /// strongly connected components (more than one node in SCC)
        /// and stores them in 'nodes'.
        /// </summary>
        /// <returns>a list containing all names of nodes, which are in non trivial SCCs</returns>
        public List<string> GetNodeNamesInNonTrivialSCC()
        {
            Dictionary<uint, uint> sccCounts = new Dictionary<uint, uint>();

            foreach (GraphNode node in this._nodes.Values)
            {
                if (sccCounts.ContainsKey(node.ComponentNumber))
                {
                    sccCounts[node.ComponentNumber] = sccCounts[node.ComponentNumber] + 1;
                }
                else
                {
                    sccCounts[node.ComponentNumber] = 1;
                }
            }
            return (from node in this.Nodes
                    where sccCounts[node.ComponentNumber] > 1
                    select node.Name).ToList<string>();
        }

        /// <summary>
        /// Returns the minimal edge, which can be found in a non-trivial
        /// (more than one node) SCC. This means that, only edges in directed
        /// cycles are considered.
        /// </summary>
        /// <returns>returns a minimal edge contained in a directed cycle of this graph</returns>
        public DirectedGraphEdge GetMinimalEdgeInNonTrivialSCC()
        {
            List<DirectedGraphEdge> minimalEdges = new List<DirectedGraphEdge>();
            List<string> nodes = null;

            nodes = this.GetNodeNamesInNonTrivialSCC();

            List<DirectedGraphEdge> potentialEdges =
                (from edge in this.Edges
                 where nodes.Contains(edge.From) && nodes.Contains(edge.To)
                 select edge).ToList<DirectedGraphEdge>();
            if (potentialEdges.Count > 0)
            {
                return (DirectedGraphEdge)potentialEdges.Min<DirectedGraphEdge>().Clone();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a list of edges, which are in non trivial strongly connected
        /// components (SCCs).
        /// </summary>
        /// <returns>a list of edges in non trivial SCCs</returns>
        public List<DirectedGraphEdge> GetEdgesInNonTrivialSCC()
        {
            List<DirectedGraphEdge> edges = null;
            List<DirectedGraphEdge> edgesCopy = new List<DirectedGraphEdge>();
            List<string> nodes = null;

            nodes = this.GetNodeNamesInNonTrivialSCC();

            edges =
                (from edge in this.Edges
                 where nodes.Contains(edge.From) && nodes.Contains(edge.To)
                    && this._nodes[edge.From].ComponentNumber == this._nodes[edge.To].ComponentNumber
                 select edge).ToList<DirectedGraphEdge>();
            foreach (DirectedGraphEdge edge in edges)
            {
                edgesCopy.Add((DirectedGraphEdge)edge.Clone());
            }

            return edgesCopy;
        }

        /// <summary>
        /// This method can be used to update weights of existing edges.
        /// </summary>
        /// <param name="newEdges">The 'Cost' field of these edges will be 
        /// used to update the edges in the graph.</param>
        public void UpdateEdgeWeights(List<DirectedGraphEdge> newEdges)
        {
            foreach (DirectedGraphEdge edge in newEdges)
            {
                string key = edge.From + "," + edge.To;
                if (this._edges.ContainsKey(key))
                {
                    this._edges[key].Cost = edge.Cost;
                }
                else
                {
                    throw new GraphException("The edge (" + edge.From +
                        "," + edge.To + ") was not found in current graph.");
                }
            }
        }

        /// <summary>
        /// Overrides the standard ToString() method to give a readable
        /// representation of the graph. 
        /// </summary>
        /// <returns>the readable representation of this graph instance</returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append("Nodes:");
            foreach (GraphNode node in this._nodes.Values.OrderBy(n => n.Name))
            {
                output.Append(node.ToString());
            }
            output.Append("\n Edges:");
            output.Append(this._edges.Values.Select(e => e.ToString()).OrderBy(name => name).StringJoin(""));
            //foreach (DirectedGraphEdge edge in this._edges.Values.OrderBy(e => e.n)
            //{
            //    output.Append(edge.ToString());
            //}
            return output.ToString();
        }

        private List<DirectedGraphEdge> _getOrderedEdges()
        {
            this.DepthFirstSearch(null, false);

            List<GraphNode> orderedNodes =
                (from node in Nodes
                 orderby node.DfsOrder descending
                 select node).ToList();
            List<DirectedGraphEdge> orderedEdges = new List<DirectedGraphEdge>();

            Dictionary<GraphNode, List<GraphNode>> adjacentNodes = new Dictionary<GraphNode, List<GraphNode>>();
            foreach (GraphNode tempNode in orderedNodes)
            {
                // we use dfs order which is n - topological order - 1, therefore ascending
                List<GraphNode> nodesQuery =
                    (from edge in Edges
                     from node in Nodes
                     where edge.To == tempNode.Name && edge.From == node.Name
                     orderby node.DfsOrder ascending
                     select node).ToList();

                adjacentNodes.Add(tempNode, nodesQuery);
            }

            // as long as there are unordered edges
            foreach (KeyValuePair<GraphNode, List<GraphNode>> tempNodes in adjacentNodes)
            {
                GraphNode tempToNode = tempNodes.Key;
                foreach (GraphNode tempFromNode in tempNodes.Value)
                {
                    orderedEdges.Add(this._edges[tempFromNode.Name + "," + tempToNode.Name]);
                }
            }
            return orderedEdges;
        }

        /// <summary>
        /// Helper function, which is used by ComputeCompelledAndReversibleEdges()
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="compelledEdges"></param>
        private void _addCompelledEdge(DirectedGraphEdge edge,
                                       ref Dictionary<string, List<DirectedGraphEdge>> compelledEdges)
        {
            if (edge.Label == GraphEdgeLabel.UNLABELLED)
            {
                edge.Label = GraphEdgeLabel.COMPLELLED;
                if (compelledEdges.ContainsKey(edge.To))
                {
                    compelledEdges[edge.To].Add(edge);
                }
                else
                {
                    List<DirectedGraphEdge> tempCompelledEdges = new List<DirectedGraphEdge>();
                    tempCompelledEdges.Add(edge);
                    compelledEdges.Add(edge.To, tempCompelledEdges);
                }
            }
        }

        /// <summary>
        /// Helper function, which is used by ComputeCompelledAndReversibleEdges()
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="reversibleEdges"></param>
        private void _addReversibleEdge(DirectedGraphEdge edge,
                                       ref Dictionary<string, List<DirectedGraphEdge>> reversibleEdges)
        {
            if (edge.Label == GraphEdgeLabel.UNLABELLED)
            {
                edge.Label = GraphEdgeLabel.REVERSIBLE;
                if (reversibleEdges.ContainsKey(edge.To))
                {
                    reversibleEdges[edge.To].Add(edge);
                }
                else
                {
                    List<DirectedGraphEdge> tempReversibleEdges = new List<DirectedGraphEdge>();
                    tempReversibleEdges.Add(edge);
                    reversibleEdges.Add(edge.To, tempReversibleEdges);
                }
            }
        }

        public void ComputeCompelledAndReversibleEdges()
        {
            List<DirectedGraphEdge> orderedEdges = this._getOrderedEdges();
            Dictionary<string, List<DirectedGraphEdge>> reversibleEdges = new Dictionary<string, List<DirectedGraphEdge>>();
            Dictionary<string, List<DirectedGraphEdge>> compelledEdges = new Dictionary<string, List<DirectedGraphEdge>>();
            Dictionary<string, List<DirectedGraphEdge>> unlabelledEdges = new Dictionary<string, List<DirectedGraphEdge>>();
            bool doneForThisCycle = false;

            foreach (var edge in orderedEdges)
            {
                edge.Label = GraphEdgeLabel.UNLABELLED;
                if (unlabelledEdges.ContainsKey(edge.To))
                {
                    unlabelledEdges[edge.To].Add(edge);
                }
                else
                {
                    List<DirectedGraphEdge> tempEdges = new List<DirectedGraphEdge>();
                    tempEdges.Add(edge);
                    unlabelledEdges.Add(edge.To, tempEdges);
                }
            }

            foreach (DirectedGraphEdge edge in orderedEdges)
            {
                if (edge.Label == GraphEdgeLabel.UNLABELLED)
                {
                    doneForThisCycle = false;

                    if (compelledEdges.ContainsKey(edge.From))
                    {
                        List<DirectedGraphEdge> tempCompelledEdges = compelledEdges[edge.From];
                        foreach (DirectedGraphEdge compelledEdge in tempCompelledEdges)
                        {
                            string tempKey = compelledEdge.From + "," + edge.To;
                            if (!this._edges.ContainsKey(tempKey))
                            {
                                if (edge.Label != GraphEdgeLabel.COMPLELLED)
                                {
                                    this._addCompelledEdge(edge, ref compelledEdges);
                                    unlabelledEdges[edge.To].Remove(edge);
                                }

                                if (unlabelledEdges.ContainsKey(edge.To))
                                {
                                    foreach (DirectedGraphEdge tempEdge in unlabelledEdges[edge.To])
                                    {
                                        this._addCompelledEdge(tempEdge, ref compelledEdges);
                                    }
                                    unlabelledEdges.Remove(edge.To);
                                }
                                doneForThisCycle = true;
                            }
                            else
                            {
                                DirectedGraphEdge tempEdge = this._edges[tempKey];
                                if (tempEdge.Label == GraphEdgeLabel.UNLABELLED)
                                {
                                    this._addCompelledEdge(tempEdge, ref compelledEdges);
                                    unlabelledEdges[tempEdge.To].Remove(tempEdge);
                                }
                            }
                        }
                    }
                    if (!doneForThisCycle)
                    {
                        IEnumerable<DirectedGraphEdge> query =
                            from queryEdge in Edges
                            where queryEdge.To == edge.To && queryEdge.From != edge.From
                            select queryEdge;
                        bool containsSpecialEdge = false;
                        foreach (DirectedGraphEdge testEdge in query)
                        {
                            string tempKey = testEdge.From + "," + edge.From;
                            if (!this._edges.ContainsKey(tempKey))
                            {
                                containsSpecialEdge = true;
                                break;
                            }
                        }
                        if (containsSpecialEdge)
                        {
                            this._addCompelledEdge(edge, ref compelledEdges);
                            unlabelledEdges[edge.To].Remove(edge);
                            if (unlabelledEdges.ContainsKey(edge.To))
                            {
                                foreach (DirectedGraphEdge tempEdge in unlabelledEdges[edge.To])
                                {
                                    this._addCompelledEdge(tempEdge, ref compelledEdges);
                                }
                                unlabelledEdges.Remove(edge.To);
                            }
                        }
                        else
                        {
                            this._addReversibleEdge(edge, ref reversibleEdges);
                            unlabelledEdges[edge.To].Remove(edge);
                            if (unlabelledEdges.ContainsKey(edge.To))
                            {
                                foreach (DirectedGraphEdge tempEdge in unlabelledEdges[edge.To])
                                {
                                    this._addReversibleEdge(tempEdge, ref reversibleEdges);
                                }
                                unlabelledEdges.Remove(edge.To);
                            }
                        }
                    }
                }
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a list of node names in topological order
        /// </summary>
        /// <returns></returns>
        public List<string> GetTopologicalOrder()
        {
            DirectedGraph tempGraph = (DirectedGraph) this.Clone();
            foreach (GraphNode node in tempGraph.Nodes)
            {
                node.DfsOrder = 0;
                node.ComponentNumber = 0;
            }
            tempGraph.DepthFirstSearch(componentNumbersRestriction: null, sorted: false);
            return (from node in tempGraph.Nodes
                    orderby node.DfsOrder descending
                    select node.Name).ToList();
        }

        /// <summary>
        /// Returns a list of compelled edges (the function 
        /// ComputeCompelledAndReversibleEdges() should be called in advance).
        /// </summary>
        /// <returns></returns>
        public List<DirectedGraphEdge> GetCompelledEdges()
        {
            return (from edge in this.Edges
                    where edge.Label == GraphEdgeLabel.COMPLELLED
                    select edge).ToList();
        }

        /// <summary>
        /// Returns a list of reversible edges (the function 
        /// ComputeCompelledAndReversibleEdges() should be called in advance).
        /// </summary>
        /// <returns></returns>
        public List<DirectedGraphEdge> GetReversibleEdges()
        {
            return (from edge in this.Edges
                    where edge.Label == GraphEdgeLabel.REVERSIBLE
                    select edge).ToList();
        }

        public void SaveToPhyloDvFile(string fileName, List<DirectedGraphEdge> justTheseEdgesOrNull)
        {
            string header = ("LeafDistribution\tPredictorVariable\tTargetVariable\tConditioningPredictorVariables\tTT\tTF\tFT\tFF\tCompetingP_Source\tCompetingP\tPValue\tqValue");

            bool first = true;
            IEnumerable<DirectedGraphEdge> tempEdges = justTheseEdgesOrNull == null ? this.Edges : justTheseEdgesOrNull;
            using (System.IO.TextWriter textWriter = System.IO.File.CreateText(fileName))
            {
                textWriter.WriteLine(header);
                foreach (DirectedGraphEdge edge in tempEdges)
                {
                    if (first)
                    {
                        textWriter.WriteLine("\t" + edge.From.Substring(0, edge.From.IndexOf('@') + 1) + "A\t"
                            + edge.To.Substring(0, edge.To.IndexOf('@') + 1) + "A\t\t\t\t\t\t\t\t"
                            + edge.Cost + "\t" + edge.Cost);
                        //first = false;
                    }
                    else
                    {
                        textWriter.WriteLine("\t" + edge.From + "\t" + edge.To + "\t\t\t\t\t\t\t\t"
                            + edge.Cost + "\t" + edge.Cost);
                    }
                }
                textWriter.Flush();
                textWriter.Close();
            }
        }
    }
}
