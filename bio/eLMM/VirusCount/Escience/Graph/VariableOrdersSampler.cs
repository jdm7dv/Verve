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
using MBT.Escience;
using Bio.Util;

namespace MBT.Escience.Graph
{
    public class VariableOrdersSampler
    {
        private DirectedGraph _restrictionGraph = null;
        private int[][] _standardRestrictionMatrix = null;
        private double[][] _C = null;
        private double[][] _R = null;
        private double[][] _M = null;
        private Dictionary<string, uint> _nameToIndex = null;
        private Dictionary<uint, string> _indexToName = null;
        private Random _randomNumbers;
        private double _bestModelScore;
        private DirectedGraph _bestModel;

        public VariableOrdersSampler(List<string> labels, DirectedGraph directedAcyclicGraph, int seed)
        {
            new VariableOrdersSampler(labels, directedAcyclicGraph, new Random(seed));
        }

        public VariableOrdersSampler(List<string> labels, DirectedGraph directedAcyclicGraph, Random randomNumbers)
        {
            HashSet<Tuple<string, string>> restrictions = null;
            List<GraphNode> nodes = new List<GraphNode>();
            List<DirectedGraphEdge> edges = new List<DirectedGraphEdge>();

            this._bestModel = null;
            this._bestModelScore = double.NegativeInfinity;

            this._randomNumbers = randomNumbers;
            restrictions = directedAcyclicGraph.GetOrderRestrictions();
            this._standardRestrictionMatrix = new int[labels.Count][];
            this._C = new double[labels.Count][];
            this._R = new double[labels.Count][];
            this._M = new double[labels.Count][];
            for (int i = 0; i < labels.Count; ++i)
            {
                this._standardRestrictionMatrix[i] = new int[labels.Count];
                this._C[i] = new double[labels.Count];
                this._R[i] = new double[i + 1];
                this._M[i] = new double[labels.Count];
            }
            // allowing all edges except self edges (identity matrix)
            // and initializing C, R and M
            for (int i = 0; i < labels.Count; ++i)
            {
                for (int j = i; j < labels.Count; ++j)
                {
                    if (i != j)
                    {
                        this._standardRestrictionMatrix[i][j] = 0;
                        this._standardRestrictionMatrix[j][i] = 0;
                        this._C[i][j] = double.NegativeInfinity;
                        this._C[j][i] = double.NegativeInfinity;
                        this._R[j][i] = double.NegativeInfinity;
                        this._M[i][j] = 0;
                        this._M[j][i] = 0;
                    }
                    else
                    {
                        this._standardRestrictionMatrix[i][j] = 1;
                        this._C[i][j] = double.NegativeInfinity;
                        this._R[j][i] = double.NegativeInfinity;
                        this._M[i][j] = 0;
                    }
                }
            }
            uint counter = 0;
            this._nameToIndex = new Dictionary<string, uint>();
            this._indexToName = new Dictionary<uint, string>();
            foreach (string label in labels)
            {
                this._nameToIndex.Add(label, counter);
                this._indexToName.Add(counter, label);
                ++counter;
            }
            foreach (Tuple<string, string> restriction in restrictions)
            {
                this._standardRestrictionMatrix[_nameToIndex[restriction.Item2]][_nameToIndex[restriction.Item1]] = 1;
                this._standardRestrictionMatrix[_nameToIndex[restriction.Item1]][_nameToIndex[restriction.Item2]] = 1;
                edges.Add(new DirectedGraphEdge(restriction.Item1, restriction.Item2, 0));
            }
            foreach (string node in labels)
            {
                nodes.Add(new GraphNode(node));
            }

            this._restrictionGraph = new DirectedGraph(nodes, edges);

        }

        public List<string> SampleOrder(int seed)
        {
            this._randomNumbers = new Random(seed);
            return this.SampleOrder();
        }

        public List<string> SampleOrder()
        {
            DirectedGraph tempGraph = (DirectedGraph)this._restrictionGraph.Clone();
            List<string> orderedNodes = new List<string>();
            int[][] tempRestrictionMatrix = (int[][])this._standardRestrictionMatrix.Clone();
            double sum = 0;
            double randomNumber = 0;
            double tempSum = 0;
            int index = 0;

            List<Tuple<double, Tuple<uint, uint>>> choices = _getPossibleChoices(tempRestrictionMatrix, ref sum);

            while (choices.Count > 0)
            {
                index = 0;

                // if all allowed choices in M are zero, we draw the index from a uniform distribution
                // otherwise, the entry M[i,j] is proportional to its probability to be chosen
                if (sum - 0.0001 < 0 && sum + 0.0001 > 0)
                {
                    index = _randomNumbers.Next(choices.Count);
                }
                else
                {
                    randomNumber = ((double)this._randomNumbers.Next()) / Int32.MaxValue * sum;

                    //find choice that corresponds to random number

                    tempSum = 0;
                    do
                    {
                        tempSum += choices[index++].Item1;
                    }
                    while (index < choices.Count && tempSum < randomNumber - 0.0001);
                    // this should not happen
                    if (index == choices.Count)
                    {
                        --index;
                    }
                }
                //add edge to restriction graph
                DirectedGraphEdge chosenEdge = new DirectedGraphEdge(this._indexToName[choices[index].Item2.Item1],
                                                                     this._indexToName[choices[index].Item2.Item2],
                                                                     0);
                tempGraph.AddEdge(chosenEdge);
                //update restrictions
                HashSet<string> newChildren = tempGraph.GetReachableNodes(chosenEdge.To);
                newChildren.Add(chosenEdge.To);
                tempRestrictionMatrix[this._nameToIndex[chosenEdge.From]][this._nameToIndex[chosenEdge.To]] = 1;
                HashSet<string> ancestors = tempGraph.GetAncestors(chosenEdge.From);
                foreach (string newChild in newChildren)
                {
                    tempRestrictionMatrix[this._nameToIndex[newChild]][this._nameToIndex[chosenEdge.From]] = 1;
                    foreach (string ancestor in ancestors)
                    {
                        tempRestrictionMatrix[this._nameToIndex[newChild]][this._nameToIndex[ancestor]] = 1;
                    }
                }
                // prepare next round
                sum = 0;
                choices = _getPossibleChoices(tempRestrictionMatrix, ref sum);
            }
            // if there are no free choices, tempGraph contains all choices and has therefore a unique topological order
            return tempGraph.GetTopologicalOrder();
        }

        private List<Tuple<double, Tuple<uint, uint>>> _getPossibleChoices(int[][] restrictionMatrix, ref double sum)
        {
            sum = 0;
            List<Tuple<double, Tuple<uint, uint>>> choices = new List<Tuple<double, Tuple<uint, uint>>>();
            for (uint i = 0; i < this._M.Count(); ++i)
            {
                for (uint j = 0; j < this._M[0].Count(); ++j)
                {
                    if (restrictionMatrix[i][j] != 1)
                    {
                        double temp = this._M[i][j];
                        choices.Add(new Tuple<double, Tuple<uint, uint>>(temp, new Tuple<uint, uint>(i, j)));
                        sum += temp;
                    }
                }
            }
            return choices;
        }

        public void UpdateModel(DirectedGraph bestGraph, double modelScore)
        {
            if (modelScore > this._bestModelScore)
            {
                this._bestModelScore = modelScore;
                this._bestModel = (DirectedGraph)bestGraph.Clone();
            }

            bestGraph.ComputeCompelledAndReversibleEdges();
            List<DirectedGraphEdge> compelledEdges = bestGraph.GetCompelledEdges();
            this._updateCompelledMatrix(modelScore, compelledEdges);

            List<DirectedGraphEdge> reversibleEdges = bestGraph.GetReversibleEdges();
            this._updateReversibleMatrix(modelScore, reversibleEdges);

            this._updateModelMatrix();
        }

        private void _updateCompelledMatrix(double modelScore, List<DirectedGraphEdge> edges)
        {
            foreach (DirectedGraphEdge edge in edges)
            {
                uint i = this._nameToIndex[edge.From];
                uint j = this._nameToIndex[edge.To];
                if (modelScore > this._C[i][j])
                {
                    this._C[i][j] = modelScore;
                }
            }
        }

        private void _updateReversibleMatrix(double modelScore, List<DirectedGraphEdge> edges)
        {
            foreach (DirectedGraphEdge edge in edges)
            {
                uint i = this._nameToIndex[edge.From];
                uint j = this._nameToIndex[edge.To];
                // we need to store only the lower triangular matrix
                if (i < j)
                {
                    uint temp = i;
                    i = j;
                    j = temp;
                }
                if (modelScore > this._R[i][j])
                {
                    this._R[i][j] = modelScore;
                }
            }
        }

        /// <summary>
        /// Calculates the new _M matrix and shifts it such that the minimal 
        /// value of _M is 0 (similar to Goldenberg and Chickering 04).
        /// </summary>
        private void _updateModelMatrix()
        {
            int tempI = 0;
            int tempJ = 0;
            double min = double.PositiveInfinity;

            for (int i = 0; i < this._M.Count(); ++i)
            {
                for (int j = 0; j < this._M[i].Count(); ++j)
                {
                    if (i < j)
                    {
                        tempI = j;
                        tempJ = i;
                    }
                    else
                    {
                        tempI = i;
                        tempJ = j;
                    }
                    if (!double.IsNegativeInfinity(this._C[i][j])
                        && (!double.IsNegativeInfinity(this._R[tempI][tempJ])
                            || !double.IsNegativeInfinity(this._C[j][i])))
                    {
                        this._M[i][j] = this._C[i][j] - Math.Max(this._C[j][i], this._R[tempI][tempJ]);
                    }
                    if (min > this._M[i][j])
                    {
                        min = this._M[i][j];
                    }
                }
            }
            for (int i = 0; i < this._M.Count(); ++i)
            {
                for (int j = 0; j < this._M[i].Count(); ++j)
                {
                    this._M[i][j] = this._M[i][j] - min;
                }
            }
        }

    }
}
