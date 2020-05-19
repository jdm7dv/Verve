// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Runtime.Serialization;

namespace MBF.Algorithms.SuffixTree
{
    /// <summary>
    /// Implements IEdge interface for MWaySuffixTree. Represents an edge in suffix tree which exposes all the properties required by IEdge and pointers to child edges.
    /// </summary>
    [Serializable]
    public class MultiWaySuffixEdge : IEdge
    {
        #region -- Member Variables --

        private IEdge[] _children = null;

        #endregion

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the Edge class
        ///     Index of First Character
        ///     Index of Last Character
        ///     Start node of edge
        /// </summary>
        /// <param name="startIndex">Index of the First Character</param>
        /// <param name="endIndex">Index of the Last Character</param>
        public MultiWaySuffixEdge(int startIndex, int endIndex)
            : this()
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        /// <summary>
        /// Initializes a new instance of the Edge class
        /// </summary>
        public MultiWaySuffixEdge()
        {
        }

        #endregion

        #region -- ISerializable Members --

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected MultiWaySuffixEdge(SerializationInfo info, StreamingContext context)
        {
            // Do not implement serialization/de-serialization here, becuase serializing root edge will
            // end up serializing the complete tree (serialization of children property)
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method for serializing the sequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Do not implement serialization/de-serialization here, becuase serializing root edge will
            // end up serializing the complete tree (serialization of children property)
            throw new NotImplementedException();
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
        /// Gets or sets a value indicating whether the edge is at the leaf.
        /// </summary>
        public bool IsLeaf
        {
            get 
            {
                return _children == null ? true : false;
            }
        }

        #endregion

        #region -- Public methods --

        /// <summary>
        /// Pointers to all the child edges
        /// </summary>
        public IEdge[] GetChildren()
        {
            return _children;
        }

        /// <summary>
        /// Add an edge to children array
        /// </summary>
        /// <param name="edge">Edge to be added</param>
        public void AddChild(IEdge edge)
        {
            if (_children == null)
            {
                _children = new IEdge[1];
                _children[0] = edge;
            }
            else
            {
                IEdge[] temp = new IEdge[_children.Length + 1];
                int index = 0;
                for (index = 0; index < _children.Length; index++)
                {
                    temp[index] = _children[index];
                }

                temp[index] = edge;
                _children = temp;
            }
        }

        /// <summary>
        /// Replace the current set of children with given set of children
        /// </summary>
        /// <param name="children">new set of children</param>
        public void ReplaceChildren(IEdge[] children)
        {
            _children = children;
        }

        /// <summary>
        /// Clear the children list of current edge
        /// </summary>
        public void ClearChildren()
        {
            _children = null;
        }

        #endregion
    }
}