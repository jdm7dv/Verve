// Copyright Microsoft Corporation. All rights reserved.

using System.Collections.Generic;
using System;

namespace Bio.Algorithms.Alignment.MultipleSequenceAlignment
{
    /// <summary>
    /// Hierarchically clusters sequences based on distance matrix.
    /// 
    /// Steps: 
    /// 1) Initially all sequences are clusters themselves.
    /// 2) Iteratively: identify the cloesest pair of sequences (clusters) in the distance matrix,
    /// cluster them into one cluster, and update distances of this cluster with the rest clusters.
    /// 3) Terminate when only one cluster left. 
    /// 
    /// A binary guide tree is then generated: 
    /// the root of the tree is the final cluster; leaves are sequence clusters.
    /// From bottom up, the nodes order represents the clustering order,
    /// and it's kept in a node list.
    /// The progressive aligner will then follow this order to align the set of sequences.
    /// </summary>
    public sealed class HierarchicalClusteringParallel : IHierarchicalClustering
    {
        #region Fields
        // The node list in the generated binary tree
        private List<BinaryGuideTreeNode> _nodes = null;

        // The edge list 
        private List<BinaryGuideTreeEdge> _edges = null;
        
        // The number of clusters; initially it's the number of sequences,
        // The clustering terminates when the number of clusters becomes 1
        private int _numberOfClusters;

        // The list stores the current clusters generated
        private List<int> _clusters = null;

        // The index of the cloest pair of clusters
        private int _nextA, _nextB;

        // Incrementally indicates the next generated cluster ID
        // and use it as the node ID which represent the new cluster
        private int _currentClusterID;

        // Delegate function for updating distances
        private UpdateDistanceMethodSelector _updateDistanceMethod;

        // Temporary variables store the smallest distance between clusters
        private float _smallestDistance, _currentDistance;
        #endregion

        #region Properties

        /// <summary>
        /// The node list of this class
        /// </summary>
        public List<BinaryGuideTreeNode> Nodes 
        { 
            get { return _nodes; } 
        }

        /// <summary>
        /// The edge list of this class
        /// </summary>
        public List<BinaryGuideTreeEdge> Edges 
        { 
            get { return _edges; } 
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Construct clusters based on distance matrix.
        /// The default distance update method is 'average'
        /// </summary>
        /// <param name="distanceMatrix">IDistanceMatrix</param>
        public HierarchicalClusteringParallel(IDistanceMatrix distanceMatrix) 
                        : this(distanceMatrix, UpdateDistanceMethodsTypes.Aaverage)
        {
        }

        /// <summary>
        /// Construct clusters using different update methods
        /// </summary>
        /// <param name="distanceMatrix">IDistanceMatrix</param>
        /// <param name="updateDistanceMethodName">enum EUpdateDistanceMethods</param>
        public HierarchicalClusteringParallel(IDistanceMatrix distanceMatrix, UpdateDistanceMethodsTypes updateDistanceMethodName)
        {
            if (distanceMatrix.Dimension <= 0)
            {
                throw new Exception("Invalid distance matrix dimension");
            }

            // The number of nodes in the final tree is 2N-2:
            // N sequence nodes (leaves) and N-2 internal nodes
            // where N is the number of input sequences
            _nodes = new List<BinaryGuideTreeNode>(distanceMatrix.Dimension * 2 - 2);
            _edges = new List<BinaryGuideTreeEdge>();

            // The number of clusters is the number of leaves at the beginning
            // As the algorithm merges clusters, only one cluster remains.
            _clusters = new List<int>(distanceMatrix.Dimension);

            // Choose a update-distance method
            switch(updateDistanceMethodName)
            {
                case(UpdateDistanceMethodsTypes.Aaverage):
                    _updateDistanceMethod = new UpdateDistanceMethodSelector(UpdateAverage);
                    break;
                case(UpdateDistanceMethodsTypes.Single):
                    _updateDistanceMethod = new UpdateDistanceMethodSelector(UpdateSingle);
                    break;
                case(UpdateDistanceMethodsTypes.Complete):
                    _updateDistanceMethod = new UpdateDistanceMethodSelector(UpdateComplete);
                    break;
                case(UpdateDistanceMethodsTypes.WeightedMAFFT):
                    _updateDistanceMethod = new UpdateDistanceMethodSelector(UpdateWeightedMAFFT);
                    break;
                default:
                    throw new Exception("invalid update method");
            }

            // Initialize the clusters
            Initialize(distanceMatrix);

            // Clustering...
            while (_numberOfClusters > 1)
            {
                GetNextPairOfCluster(distanceMatrix);
                CreateCluster();
                UpdateDistance(distanceMatrix);
                UpdateClusters();
            }
        }
        #endregion

        #region Cluster related methods
        /// <summary>
        /// Initialize: make each sequence a cluster
        /// </summary>
        /// <param name="distanceMatrix">distance matrix</param>
        private void Initialize(IDistanceMatrix distanceMatrix)
        {
            _numberOfClusters = distanceMatrix.Dimension;
            for (int i = 0; i < _numberOfClusters; ++i)
            {
                // Both node ID and sequence ID equal to the sequence index
                _nodes.Add(new BinaryGuideTreeNode(i));
                _clusters.Add(i);
            }
            _currentClusterID = distanceMatrix.Dimension - 1;
        }

        /// <summary>
        /// O(N^2) algorithm to get the next closest pair of clusters
        /// </summary>
        /// <param name="distanceMatrix">distance matrix</param>
        private void GetNextPairOfCluster(IDistanceMatrix distanceMatrix)
        {
            _nextA = _clusters[0];
            _nextB = _clusters[1];
            _smallestDistance = distanceMatrix[Nodes[_nextA].SequenceID, Nodes[_nextB].SequenceID];

            for (int i = 0; i < _clusters.Count - 1; ++i)
            {
                for (int j = i + 1; j < _clusters.Count; ++j)
                {
                    _currentDistance = distanceMatrix[Nodes[_clusters[i]].SequenceID, Nodes[_clusters[j]].SequenceID];
                    if (_currentDistance < _smallestDistance)
                    {
                        _smallestDistance = _currentDistance;
                        _nextA = _clusters[i];
                        _nextB = _clusters[j];
                    }
                }
            }
        }

        /// <summary>
        /// Combine cluster nextA and nextB into a new cluster
        /// </summary>
        private void CreateCluster()
        {
            BinaryGuideTreeNode _node = new BinaryGuideTreeNode(++_currentClusterID);

            // link the two nodes nextA and nextB with the new node
            _node.LeftChildren = Nodes[_nextA];
            _node.RightChildren = Nodes[_nextB];
            Nodes[_nextA].Parent = _node;
            Nodes[_nextB].Parent = _node;

            // use the leftmost leave's sequenceID
            _node.SequenceID = Nodes[_nextA].SequenceID;

            Nodes.Add(_node);

            BinaryGuideTreeEdge _edge1 = new BinaryGuideTreeEdge(Nodes[_nextA].ID);
            BinaryGuideTreeEdge _edge2 = new BinaryGuideTreeEdge(Nodes[_nextB].ID);
            _edge1.ParentNode = _node;
            _edge2.ParentNode = _node;
            _edge1.ChildNode = Nodes[_nextA];
            _edge2.ChildNode = Nodes[_nextB];

            // the length of the edge is the percent identity of two node sequences
            // or the average of identities between two sets of sequences
            //_edge1.Length = KimuraDistanceScoreCalculator.calculateDistanceScore(
            //    seqs[nodes[next1].sequenceID], seqs[nodes[next2].sequenceID]);
            
            // modified: define kimura distance as sequence distance
            _edge1.Length = _smallestDistance;
            _edge2.Length = _smallestDistance;
            
            _edges.Add(_edge1);
            _edges.Add(_edge2);

        }

        /// <summary>
        /// Update the distance between new clusters with the rest clusters
        /// </summary>
        private void UpdateDistance(IDistanceMatrix distanceMatrix)
        {
            foreach (int i in _clusters)
            {
                if (i != _nextA && i != _nextB)
                {
                    distanceMatrix[Nodes[_currentClusterID].SequenceID, Nodes[i].SequenceID] =
                            _updateDistanceMethod(distanceMatrix, Nodes[_nextA].SequenceID, Nodes[_nextB].SequenceID, Nodes[i].SequenceID);
                }
            }
        }

        /// <summary>
        /// Update clusters:
        /// remove clusters next1 and next2 and add a new merged cluster[next1, next2]
        /// </summary>
        private void UpdateClusters()
        {
            _clusters.Remove(_nextA);
            _clusters.Remove(_nextB);
            _clusters.Add(_currentClusterID);
            --_numberOfClusters;
        }
        #endregion


        #region Update cluster methods

        // Check out enum UpdateDistanceMethodsTypes for details
        private float UpdateAverage(IDistanceMatrix dm, int next1, int next2, int other)
        {
            return (dm[next1, other] + dm[next2, other]) / 2;
        }

        private float UpdateSingle(IDistanceMatrix dm, int next1, int next2, int other)
        {
            return Math.Min(dm[next1, other], dm[next2, other]);
        }

        private float UpdateComplete(IDistanceMatrix dm, int next1, int next2, int other)
        {
            return Math.Max(dm[next1, other], dm[next2, other]);
        }

        private float UpdateWeightedMAFFT(IDistanceMatrix dm, int next1, int next2, int other)
        {
            return (float)(0.9*Math.Min(dm[next1, other], dm[next2, other]) 
                        + 0.1*(dm[next1, other] + dm[next2, other]) / 2);
        }
        #endregion
    }
}
