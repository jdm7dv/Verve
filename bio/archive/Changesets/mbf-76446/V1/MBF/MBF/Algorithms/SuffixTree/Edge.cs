// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;

namespace MBF.Algorithms.SuffixTree
{
    /// <summary>
    /// Data storage representation of an Edge in Suffix Tree
    /// </summary>
    public class Edge
    {
        #region -- Member Variables --

        /// <summary>
        /// Count of nodes
        /// </summary>
        private static int _nodeCount = 1;

        /// <summary>
        /// object to lock the nodeCount
        /// </summary>
        private static object lockObject = new object();

        #endregion

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the Edge class
        ///     Edge from given edge
        /// </summary>
        /// <param name="edge">Input Edge</param>
        public Edge(Edge edge)
            : this(edge.StartNode)
        {
            if (edge == null)
            {
                throw new ArgumentNullException("edge");
            }

            StartIndex = edge.StartIndex;
            EndIndex = edge.EndIndex;
            EndNode = edge.EndNode;
        }

        /// <summary>
        /// Initializes a new instance of the Edge class
        ///     Index of First Character
        ///     Index of Last Character
        ///     Start node of edge
        /// </summary>
        /// <param name="startIndex">Index of the First Character</param>
        /// <param name="endIndex">Index of the Last Character</param>
        /// <param name="startNode">Node where the Edge starts</param>
        public Edge(int startIndex, int endIndex, int startNode)
            : this(startNode)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;

            lock (lockObject)
            {
                EndNode = NodeCount++;
            }
        }

        /// <summary>
        /// Initializes a new instance of the Edge class
        ///     Set the Start node to root (-1)
        /// </summary>
        public Edge()
            : this(-1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Edge class
        ///     Start node of Edge
        /// </summary>
        /// <param name="startNode">Start Node</param>
        private Edge(int startNode)
        {
            StartNode = startNode;
            IsLeaf = true;
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Gets or sets index of first character
        /// </summary>
        public int StartIndex { get; set; }

        /// <summary>
        /// Gets or sets index of last character
        /// </summary>
        public int EndIndex { get; set; }

        /// <summary>
        /// Gets or sets node where the Edge starts
        /// </summary>
        public int StartNode { get; set; }

        /// <summary>
        /// Gets or sets node where the Edge ends
        /// </summary>
        public int EndNode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the edge is at the leaf.
        /// </summary>
        public bool IsLeaf { get; set; }

        /// <summary>
        /// Gets or sets the total count of nodes.
        /// </summary>
        public static int NodeCount
        {
            get { return _nodeCount; }
            set { _nodeCount = value; }
        }
        #endregion
    }
}