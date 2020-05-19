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
    /// This interface defines the contract that has to be implemented by and class implementing persistent Suffix edge.
    /// Performs storage function on a SuffixEdge which has child edge of type T
    /// </summary>
    public interface ISuffixEdgeStorage
    {
        /// <summary>
        /// Write the edge to database storage.
        /// 1. Serialize the given edge.
        /// 2. Write it to the database.
        /// 3. Return the Key(Id) of newly inserted row.
        /// If the edge does not have key, insert a edge in storage
        /// Else update the existing edge in storage
        /// </summary>
        /// <param name="edge">Edge to be persisted</param>
        /// <returns>Offset/Id of new Edge</returns>
        long Write(IPersistentEdge edge);

        /// <summary>
        /// Read an edge from the storage device using ginen index (offset/Id) and return the same.
        /// 1. Read the serialized data from storage.
        /// 2. De-serialize it back to IEdge.
        /// 3. Return the edge
        /// </summary>
        /// <param name="index">Index (offset/Id) of required edge</param>
        /// <returns>Edge found at the offset/Id</returns>
        IPersistentEdge Read(long index);

        /// <summary>
        /// Remove an edge from the storage device using ginen index (offset/Id).
        /// </summary>
        /// <param name="index">Index (offset/Id) of edge to be removed</param>
        /// <returns>Success flag</returns>
        bool Remove(long index);
    }
}