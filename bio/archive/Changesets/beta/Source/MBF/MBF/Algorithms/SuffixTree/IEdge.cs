// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Runtime.Serialization;

namespace MBF.Algorithms.SuffixTree
{
    /// <summary>
    /// This interface defines the contract that has to be implemented by and class implement Edge of suffix tree.
    /// </summary>
    public interface IEdge : ISerializable
    {
        /// <summary>
        /// Gets or sets index of last character
        /// </summary>
        int EndIndex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the edge is at the leaf.
        /// </summary>
        bool IsLeaf { get; }

        /// <summary>
        /// Gets or sets index of first character
        /// </summary>
        int StartIndex { get; set; }
    }
}
