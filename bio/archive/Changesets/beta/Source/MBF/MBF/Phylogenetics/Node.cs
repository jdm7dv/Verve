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

namespace MBF.Phylogenetics
{
    /// <summary>
    /// Node : Node of the tree which can be either Leaf or another branch node.
    /// </summary>
    public class Node : ICloneable
    {
        #region -- Member Variables --
        private Dictionary<Node, Edge> _children;
        #endregion  -- Member Variables --

        #region -- Constructors --
        /// <summary>
        /// Default constructor
        /// </summary>
        public Node()
        {
            _children= new Dictionary<Node,Edge>();
        }
        #endregion -- Constructors --

        #region -- Properties --
        /// <summary>
        /// Get Childern nodes
        /// </summary>
        public Dictionary<Node, Edge> Children 
        { 
            get 
            { 
                return _children; 
            } 
        }

        /// <summary>
        /// Get list of Nodes
        /// </summary>
        public IList<Node> Nodes 
        {
            get 
            { 
                return _children.Keys.ToList(); 
            }
        }

        /// <summary>
        /// Get list of Edges
        /// </summary>
        public IList<Edge> Edges 
        { 
            get 
            { 
                return _children.Values.ToList(); 
            } 
        }

        /// <summary>
        /// Either node is leaf or not
        /// </summary>
        public bool IsLeaf 
        { 
            get 
            {
                return Children.Keys.Count == 0; 
            } 
        }

        /// <summary>
        /// Either node is root node or not
        /// </summary>
        public bool IsRoot { set; get; }
        
        /// <summary>
        /// If its leaf node, then use name
        /// </summary>
        public string Name { get; set; }
        #endregion -- Properties --

        #region -- Methods --
        /// <summary>
        /// Clone object
        /// </summary>
        /// <returns>Node as object</returns>
        public object Clone()
        {
            Node newNode = (Node)this.MemberwiseClone();
            return newNode;
        }
        #endregion -- Methods --
    }

}
