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
    /// This interface extends IEdge and defines the contract that has to be implemented by and 
    /// class implementing persistent suffix edge of suffix tree.
    /// Targetting string (instead of stream) serialization as persistent storage can be anything.
    /// </summary>
    public interface IPersistentEdge : IEdge
    {
        /// <summary>
        /// Gets or sets the unique key representing the edge in persistent storage
        /// </summary>
        long Key { get; set; }

        /// <summary>
        /// Gets pointers to all the child edges
        /// </summary>
        long[] GetChildren();

        /// <summary>
        /// Serialize the given object to string format.
        /// </summary>
        /// <returns>Serialized data</returns>
        string Serialize();

        /// <summary>
        /// Deserialize given data into persistent suffix tree edge
        /// </summary>
        /// <param name="data">Serialized data</param>
        void Deserialize(string data);
    }
}