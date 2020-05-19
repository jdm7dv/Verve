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
    /// Implements the ISuffixTree interface. A multi-way suffix tree is a data structure used to store suffixes of a bigger string in a tree format. This data structure allows better scalability on high end machines. Each node can have a maximum of N children where N is the number of distinct elements in the sequence.
    /// This class persists the tree on a storable device instead of in-memory data structure.
    /// </summary>
    public class PersistentMultiWaySuffixTree : MultiWaySuffixTree, IDisposable
    {
        #region -- Member Variables --

        /// <summary>
        /// Storage object for persistent edges
        /// </summary>
        private ISuffixEdgeStorage _edgeStore = null;

        /// <summary>
        /// Threshold at which the edges are pushed to storable device.
        /// </summary>
        private int _persisntThreshold = -1;

        #endregion

        #region -- Constructor(s) --

        /// <summary>
        /// Default constructor: Initializes the maximum number of child edges allowed in tree
        /// </summary>
        /// <param name="sequence">Sequence of the tree</param>
        /// <param name="maximumChildrenCount">Number of allowed child edges</param>
        /// <param name="persisntThreshold">Threshold at which the edges are pushed to storable device.</param>
        /// _edgeStore.Dispose() is being called in Dispose(true) method.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public PersistentMultiWaySuffixTree(
                ISequence sequence,
                int maximumChildrenCount,
                int persisntThreshold)
            : this(sequence, maximumChildrenCount, persisntThreshold, new FileSuffixEdgeStorage())
        {
        }

        /// <summary>
        /// Default constructor: Initializes the maximum number of child edges allowed in tree
        /// </summary>
        /// <param name="sequence">Sequence of the tree</param>
        /// <param name="maximumChildrenCount">Number of allowed child edges</param>
        /// <param name="persisntThreshold">Threshold at which the edges are pushed to storable device.</param>
        /// <param name="edgeStorage">Persistent edge storage</param>
        public PersistentMultiWaySuffixTree(
                ISequence sequence,
                int maximumChildrenCount,
                int persisntThreshold,
                ISuffixEdgeStorage edgeStorage)
            : base(sequence, maximumChildrenCount)
        {
            _persisntThreshold = persisntThreshold;
            _edgeStore = edgeStorage;
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Gets or sets storage object for persistent edges
        /// </summary>
        public ISuffixEdgeStorage EdgeStore { get { return _edgeStore; } }

        /// <summary>
        /// Gets or sets threshold at which the edges are pushed to storable device.
        /// </summary>
        public int PersisntThreshold { get { return _persisntThreshold; } }

        /// <summary>
        /// Insert a edge at given parent.
        /// </summary>
        /// <param name="parentEdge">Parent edge.</param>
        /// <param name="startIndex">Start index of edge</param>
        /// <param name="endIndex">End index of edge</param>
        /// <returns>New edge created</returns>
        public override IEdge Insert(IEdge parentEdge, int startIndex, int endIndex)
        {
            // This is a leaf node with both start and end indices pointing to
            // terminating symbol. Don't insert this.
            if (startIndex >= Sequence.Count)
            {
                return null;
            }

            if (PersisntThreshold > Count)
            {
                return base.Insert(parentEdge, startIndex, endIndex);
            }

            PersistentMultiWaySuffixEdge edge = new PersistentMultiWaySuffixEdge(
                    startIndex,
                    endIndex,
                    MaximumChildrenCount);
            edge.Key = EdgeStore.Write(edge);

            // Parent edge of type PersistentMultiWaySuffixEdge
            PersistentMultiWaySuffixEdge pmwParentEdge = parentEdge as PersistentMultiWaySuffixEdge;
            if (pmwParentEdge != null)
            {
                pmwParentEdge.AddChild(edge.Key);
                Count++;
                return edge;
            }

            // Parent edge of type MultiWaySuffixEdge
            MultiWaySuffixEdge mwParentEdge = parentEdge as MultiWaySuffixEdge;
            if (mwParentEdge != null)
            {
                if (mwParentEdge.GetChildren() == null)
                {
                    mwParentEdge.AddChild(edge);
                    Count++;
                    return edge;
                }

                if (mwParentEdge.GetChildren().Length < MaximumChildrenCount)
                {
                    mwParentEdge.AddChild(edge);
                    Count++;
                    return edge;
                }

                // No more children edge can be added.
                throw new InvalidOperationException(string.Format(
                        CultureInfo.CurrentCulture,
                        "Cannot add more than {0} child nodes to edge.",
                        MaximumChildrenCount));
            }

            return edge;
        }

        /// <summary>
        /// Remove the edge from tree, this requires bubbling all the child edges one level up.
        /// </summary>
        /// <param name="parentEdge">Parent edge.</param>
        /// <param name="edge">Edge to be removed.</param>
        /// <returns></returns>
        public override bool Remove(IEdge parentEdge, IEdge edge)
        {
            PersistentMultiWaySuffixEdge pmwParentEdge = parentEdge as PersistentMultiWaySuffixEdge;
            if (pmwParentEdge == null)
            {
                return base.Remove(parentEdge, edge);
            }

            PersistentMultiWaySuffixEdge pmwedge = edge as PersistentMultiWaySuffixEdge;
            if (pmwedge == null)
            {
                throw new ArgumentNullException("edge");
            }

            // Find the edge
            // Set the children reference to -1;
            for (int index = 0; index < pmwParentEdge.GetChildren().Length; index++)
            {
                if (pmwParentEdge.GetChildren()[index] == pmwedge.Key)
                {
                    pmwParentEdge.GetChildren()[index] = -1;
                    EdgeStore.Write(pmwParentEdge);
                    EdgeStore.Remove(pmwedge.Key);
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
        public override bool Update(IEdge parentEdge, IEdge oldEdge, IEdge newEdge)
        {
            PersistentMultiWaySuffixEdge pmwParentEdge = parentEdge as PersistentMultiWaySuffixEdge;
            if (pmwParentEdge == null)
            {
                return base.Update(parentEdge, oldEdge, newEdge);
            }

            PersistentMultiWaySuffixEdge pmwOldEdge = oldEdge as PersistentMultiWaySuffixEdge;
            if (pmwOldEdge == null)
            {
                throw new ArgumentNullException("oldEdge");
            }

            PersistentMultiWaySuffixEdge pmwNewEdge = newEdge as PersistentMultiWaySuffixEdge;
            if (pmwNewEdge == null)
            {
                throw new ArgumentNullException("newEdge");
            }

            // Find the edge
            // Replace the edge
            for (int index = 0; index < pmwParentEdge.GetChildren().Length; index++)
            {
                if (pmwParentEdge.GetChildren()[index] == pmwOldEdge.Key)
                {
                    pmwParentEdge.GetChildren()[index] = pmwNewEdge.Key;
                    EdgeStore.Write(pmwParentEdge);
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
        public override IEdge Split(IEdge edge, int splitAt)
        {
            // This is a leaf node with both start and end indices pointing to
            // terminating symbol. Don't insert this.
            if (splitAt == Sequence.Count)
            {
                return edge;
            }

            PersistentMultiWaySuffixEdge pmwCurrentEdge = edge as PersistentMultiWaySuffixEdge;
            if (pmwCurrentEdge == null)
            {
                return base.Split(edge, splitAt);
            }

            // Create the new edge
            PersistentMultiWaySuffixEdge newEdge =
                    new PersistentMultiWaySuffixEdge(splitAt + 1, pmwCurrentEdge.EndIndex, MaximumChildrenCount);

            // Copy the children of old edge to new edge
            newEdge.ReplaceChildren(pmwCurrentEdge.GetChildren());

            // Write the edge to storage
            newEdge.Key = EdgeStore.Write(newEdge);

            // Update the old edge
            pmwCurrentEdge.EndIndex = splitAt;

            // Set new edge as child edge to old edge
            pmwCurrentEdge.ClearChildren();
            pmwCurrentEdge.AddChild(newEdge.Key);
            Count++;

            // Update the edge in storage
            EdgeStore.Write(pmwCurrentEdge);

            return pmwCurrentEdge;
        }

        /// <summary>
        /// Find the child edge, whose first character starts with given character.
        /// </summary>
        /// <param name="edge">Parent edge</param>
        /// <param name="character">First character of required edge</param>
        /// <returns>Edge found</returns>
        public override IEdge Find(IEdge edge, byte character)
        {
            PersistentMultiWaySuffixEdge pmwCurrentEdge = edge as PersistentMultiWaySuffixEdge;
            if (pmwCurrentEdge == null)
            {
                return base.Find(edge, character);
            }

            // Find from the storage
            IEdge edgeFound = null;
            PersistentMultiWaySuffixEdge pmwEdge = null;
            int charIndex = 0;

            for (int index = 0; index < pmwCurrentEdge.GetChildren().Length; index++)
            {
                if (pmwCurrentEdge.GetChildren()[index] != -1)
                {
                    pmwEdge = (PersistentMultiWaySuffixEdge)EdgeStore.Read(pmwCurrentEdge.GetChildren()[index]);
                    charIndex = pmwEdge.StartIndex;

                    if (charIndex < Sequence.Count)
                    {
                        if ((Sequence as IList<byte>)[charIndex] == character)
                        {
                            edgeFound = pmwEdge;
                            break;
                        }
                    }
                }
            }

            return edgeFound;
        }

        #endregion

        #region -- IDisposable --

        /// <summary>
        /// Dispose the resources
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
                if (_edgeStore != null)
                {
                    ((FileSuffixEdgeStorage)_edgeStore).Dispose();
                }
            }
        }

        #endregion
    }
}