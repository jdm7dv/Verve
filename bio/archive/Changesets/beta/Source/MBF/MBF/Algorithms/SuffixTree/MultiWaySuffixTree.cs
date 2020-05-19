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
using System.Globalization;

namespace MBF.Algorithms.SuffixTree
{
    /// <summary>
    /// Implements IMultiWaySuffixTree interface. Represents an in-memory suffix tree and exposes all the properties and methods required by Add/update tree.
    /// </summary>
    public class MultiWaySuffixTree : IMultiWaySuffixTree
    {
        #region -- Constants --

        /// <summary>
        /// Character used as terminating symbol for Suffix Tree
        /// </summary>
        private const byte TERMINATING_SYMBOL = 36;

        /// <summary>
        /// Character used as Concatenating symbol for Suffix Tree
        /// Note: This is specific to NUCmer implementation
        /// </summary>
        private const byte CONCATENATING_SYMBOL = 43;

        #endregion

        #region -- Member Variables --

        /// <summary>
        /// Maximum number of child edges allowed in tree
        /// </summary>
        private int _maximumChildrenCount = 0;

        /// <summary>
        /// Sequence of Suffix Tree
        /// </summary>
        private ISequence _sequence = null;

        /// <summary>
        /// Reference sequence's encoded values are in the byte array.
        /// </summary>
        private byte[] _referenceSymbols = null;

        /// <summary>
        /// If reference sequence is segmented sequence then store sequence's encoded values in 
        /// segemented sequence in list of byte array.
        /// </summary>
        private IList<byte[]> _referenceSymbolsList = null;

        #endregion

        #region -- Constructor(s) --

        /// <summary>
        /// Default constructor: Initializes the maximum number of child edges allowed in tree
        /// Creates child node of type “IMultiWaySuffixEdge”, that implies that the references to child node will be in-memory.
        /// </summary>
        /// <param name="sequence">Sequence of the tree</param>
        /// <param name="maximumChildrenCount">Number of allowed child edges</param>
        public MultiWaySuffixTree(ISequence sequence, int maximumChildrenCount)
        {
            InitializeReferenceSequence(sequence);
            _maximumChildrenCount = maximumChildrenCount;
            Root = new MultiWaySuffixEdge();
            Count++;
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Gets total number of edges in suffix tree.
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// Gets or sets the root node (edge) in suffix tree
        /// </summary>
        public IEdge Root { get; protected set; }

        /// <summary>
        /// Gets sequence of Suffix Tree.
        /// </summary>
        public ISequence Sequence
        {
            get { return _sequence; }
        }

        /// <summary>
        /// Gets maximum number of child edges allowed in tree
        /// </summary>
        protected int MaximumChildrenCount
        {
            get { return _maximumChildrenCount; }
        }

        #endregion

        #region -- Public methods --

        /// <summary>
        /// Merge the given branch at the root of Suffix Tree.
        /// Asummption:
        ///  The root node of the given branch contains only one edge, which is the branch to be merged.
        /// </summary>
        /// <param name="branch">Branch to be merged.</param>
        /// <returns>Success flag.</returns>
        public bool Merge(IMultiWaySuffixTree branch)
        {
            if (branch == null)
            {
                throw new NotImplementedException("branch");
            }

            MultiWaySuffixEdge mwBranchEdge = branch.Root as MultiWaySuffixEdge;
            if (mwBranchEdge.GetChildren() == null)
            {
                return false;
            }

            MultiWaySuffixEdge mwRoot = Root as MultiWaySuffixEdge;
            if (mwRoot.GetChildren() == null)
            {
                mwRoot.AddChild(mwBranchEdge.GetChildren()[0]);
                Count += (branch.Count - 1); // - the original root edge of branch
                return true;
            }

            if (mwRoot.GetChildren().Length < _maximumChildrenCount)
            {
                mwRoot.AddChild(mwBranchEdge.GetChildren()[0]);
                Count += (branch.Count - 1); // - the original root edge of branch
                return true;
            }

            // No more children edge can be added.
            throw new InvalidOperationException(string.Format(
                CultureInfo.CurrentCulture,
                "Cannot add more than {0} child nodes to edge.",
                _maximumChildrenCount));
        }

        /// <summary>
        /// Insert a edge at given parent.
        /// </summary>
        /// <param name="parentEdge">Parent edge.</param>
        /// <param name="startIndex">Start index of edge</param>
        /// <param name="endIndex">End index of edge</param>
        /// <returns>New edge created</returns>
        public virtual IEdge Insert(IEdge parentEdge, int startIndex, int endIndex)
        {
            // This is a leaf node with both start and end indices pointing to
            // terminating symbol. Don't insert this.
            if (startIndex >= _sequence.Count)
            {
                return null;
            }

            MultiWaySuffixEdge mwParentEdge = parentEdge as MultiWaySuffixEdge;
            if (mwParentEdge == null)
            {
                throw new ArgumentNullException("parentEdge");
            }

            if (mwParentEdge.GetChildren() ==  null)
            {
                IEdge edge = new MultiWaySuffixEdge(startIndex, endIndex);
                mwParentEdge.AddChild(edge);
                Count++;
                return edge;
            }
            
            if (mwParentEdge.GetChildren().Length < _maximumChildrenCount)
            {
                IEdge edge = new MultiWaySuffixEdge(startIndex, endIndex);
                mwParentEdge.AddChild(edge);
                Count++;
                return edge;
            }

            // No more children edge can be added.
            throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Cannot add more than {0} child nodes to edge.",
                    _maximumChildrenCount));
        }

        /// <summary>
        /// Remove the edge from tree, the edge and all its children will be removed.
        /// </summary>
        /// <param name="parentEdge">Parent edge.</param>
        /// <param name="edge">Edge to be removed.</param>
        /// <returns>Success flag</returns>
        public virtual bool Remove(IEdge parentEdge, IEdge edge)
        {
            MultiWaySuffixEdge mwParentEdge = parentEdge as MultiWaySuffixEdge;
            if (mwParentEdge == null)
            {
                throw new ArgumentNullException("parentEdge");
            }

            if (edge == null)
            {
                throw new ArgumentNullException("edge");
            }

            for (int index = 0; index < mwParentEdge.GetChildren().Length; index++)
            {
                if (mwParentEdge.GetChildren()[index] == edge)
                {
                    mwParentEdge.GetChildren()[index] = null;
                    Count--;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Update the old node with new node, requires merging the child edges.
        /// </summary>
        /// <param name="parentEdge">Parent edge.</param>
        /// <param name="oldEdge">Old edge</param>
        /// <param name="newEdge">new edge</param>
        /// <returns></returns>
        public virtual bool Update(IEdge parentEdge, IEdge oldEdge, IEdge newEdge)
        {
            MultiWaySuffixEdge mwParentEdge = parentEdge as MultiWaySuffixEdge;
            if (mwParentEdge == null)
            {
                throw new ArgumentNullException("parentEdge");
            }

            if (oldEdge == null)
            {
                throw new ArgumentNullException("oldEdge");
            }

            if (newEdge == null)
            {
                throw new ArgumentNullException("newEdge");
            }

            for (int index = 0; index < mwParentEdge.GetChildren().Length; index++)
            {
                if (mwParentEdge.GetChildren()[index] == oldEdge)
                {
                    mwParentEdge.GetChildren()[index] = newEdge;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Split the edge at given index.
        /// </summary>
        /// <param name="edge">Edge to be split</param>
        /// <param name="splitAt">Index at which the edge has to be split</param>
        /// <returns>New edge</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "splitAt+1")]
        public virtual IEdge Split(IEdge edge, int splitAt)
        {
            // This is a leaf node with both start and end indices pointing to
            // terminating symbol. Don't insert this.
            if (splitAt == _sequence.Count)
            {
                return edge;
            }

            MultiWaySuffixEdge mwCurrentEdge = edge as MultiWaySuffixEdge;
            if (mwCurrentEdge == null)
            {
                throw new ArgumentNullException("edge");
            }

            // Create the new edge
            MultiWaySuffixEdge newEdge =
                    new MultiWaySuffixEdge(splitAt + 1, mwCurrentEdge.EndIndex);

            // Copy the children of old edge to new edge
            newEdge.ReplaceChildren(mwCurrentEdge.GetChildren());

            // Update the old edge
            mwCurrentEdge.EndIndex = splitAt;

            // Set new edge as child edge to old edge
            mwCurrentEdge.ClearChildren();
            mwCurrentEdge.AddChild(newEdge);
            Count++;

            return mwCurrentEdge;
        }

        /// <summary>
        /// Find the child edge, whose first character starts with given character.
        /// </summary>
        /// <param name="edge">Parent edge</param>
        /// <param name="character">First character of required edge</param>
        /// <returns>Edge found</returns>
        public virtual IEdge Find(IEdge edge, byte character)
        {
            MultiWaySuffixEdge mwEdge = edge as MultiWaySuffixEdge;
            if (mwEdge == null)
            {
                throw new ArgumentNullException("edge");
            }

            if (mwEdge.GetChildren() == null)
            {
                return null;
            }

            int charIndex = 0;

            foreach (IEdge childEdge in mwEdge.GetChildren())
            {
                charIndex = childEdge.StartIndex;

                if (charIndex < _sequence.Count)
                {
                    if (GetReferenceSymbol(charIndex) == character)
                    {
                        return childEdge;
                    }
                }
            }

            return null;
        }

        #endregion

        #region -- Private methods --

        /// <summary>
        /// Initialize reference sequence.
        /// If the sequence is DV-enabled pre-fetch the sequence in local byte array
        /// </summary>
        /// <param name="sequence"></param>
        private void InitializeReferenceSequence(ISequence sequence)
        {
            // If the sequence is DV enabled
            SegmentedSequence segments = sequence as SegmentedSequence;

            if (segments == null)
            {
                _referenceSymbols = new byte[sequence.Count];
                (sequence as IList<byte>).CopyTo(_referenceSymbols, 0);
            }
            else
            {
                _referenceSymbolsList = new List<byte[]>();
                foreach (Sequence subSequence in segments.Sequences)
                {
                    byte[] tmpBytes = new byte[subSequence.Count];
                    (subSequence as IList<byte>).CopyTo(tmpBytes, 0);
                    _referenceSymbolsList.Add(tmpBytes);
                }
            }

            _sequence = sequence;
        }

        /// <summary>
        /// Get the symbol from refrence sequence at given index
        /// </summary>
        /// <param name="index">Index of symbol</param>
        /// <returns>Symbol at index</returns>
        private byte GetReferenceSymbol(int index)
        {
            if (_referenceSymbols != null)
            {
                if (index == _referenceSymbols.Length)
                {
                    return TERMINATING_SYMBOL;
                }
                else
                {
                    return _referenceSymbols[index];
                }
            }
            else
            {
                int length = 0;
                for (int sequenceIndex = 0; sequenceIndex < _referenceSymbolsList.Count; sequenceIndex++)
                {
                    index -= length;

                    if (index < _referenceSymbolsList[sequenceIndex].Length)
                    {
                        return _referenceSymbolsList[sequenceIndex][index];
                    }
                    else if (index == _referenceSymbolsList[sequenceIndex].Length)
                    {
                        if (sequenceIndex < _referenceSymbolsList.Count - 1)
                        {
                            return CONCATENATING_SYMBOL;
                        }
                        else
                        {
                            return TERMINATING_SYMBOL;
                        }
                    }

                    length = _referenceSymbolsList[sequenceIndex].Length + 1;
                }
            }

            return 0;
        }

        #endregion
    }
}