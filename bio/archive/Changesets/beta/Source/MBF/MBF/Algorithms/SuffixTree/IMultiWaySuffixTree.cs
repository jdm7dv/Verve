// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Algorithms.SuffixTree
{
    /// <summary>
    /// Extends ISuffix Tree interface.
    /// This interface defines the contract that has to be implemented by and class implementing multi way suffix tree.
    /// </summary>
    public interface IMultiWaySuffixTree : ISuffixTree
    {
        /// <summary>
        /// Gets or sets the root node (edge) in suffix tree
        /// </summary>
        IEdge Root { get; }

        /// <summary>
        /// Gets total number of edges in suffix tree.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Merge the given branch at the root of Suffix Tree.
        /// Asummption:
        ///  The root node of the given branch contains only one edge, which is the branch to be merged.
        /// </summary>
        /// <param name="branch">Branch to be merged.</param>
        /// <returns>Success flag.</returns>
        bool Merge(IMultiWaySuffixTree branch);

        /// <summary>
        /// Insert a edge at given parent.
        /// </summary>
        /// <param name="parentEdge">Parent edge.</param>
        /// <param name="startIndex">Start index of edge</param>
        /// <param name="endIndex">End index of edge</param>
        /// <returns>New edge created</returns>
        IEdge Insert(IEdge parentEdge, int startIndex, int endIndex);

        /// <summary>
        /// Remove the edge from tree, this requires bubbling all the child edges one level up.
        /// </summary>
        /// <param name="parentEdge">Parent edge.</param>
        /// <param name="edge">Edge to be removed.</param>
        /// <returns></returns>
        bool Remove(IEdge parentEdge, IEdge edge);

        /// <summary>
        /// Split the edge at given index.
        /// </summary>
        /// <param name="edge">Edge to be split</param>
        /// <param name="splitAt">Index at which the edge has to be split</param>
        IEdge Split(IEdge edge, int splitAt);

        /// <summary>
        /// Update the old node with new node, requires merging the child edges.
        /// </summary>
        /// <param name="parentEdge">Parent edge.</param>
        /// <param name="oldEdge">Old edge</param>
        /// <param name="newEdge">new edge</param>
        /// <returns></returns>
        bool Update(IEdge parentEdge, IEdge oldEdge, IEdge newEdge);

        /// <summary>
        /// Find the child edge, whose first character starts with given character.
        /// </summary>
        /// <param name="edge">Parent edge</param>
        /// <param name="character">First character of required edge</param>
        /// <returns>Edge found</returns>
        IEdge Find(IEdge edge, byte character);
    }
}
