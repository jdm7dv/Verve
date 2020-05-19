// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF
{
    /// <summary>
    /// Contract to be implemented by Virtual data holder for any type (T).
    /// Encapsulate data from the user and load it on demand.
    /// </summary>
    /// <typeparam name="T">Object type</typeparam>
    public interface IVirtualData<T> : IList<CacheBox<T>>
    {
        /// <summary>
        /// Gets or sets maximum number of blocks per sequence
        /// </summary>
        int MaxNumberOfBlocks { get; set; }

        /// <summary>
        /// Gets or sets single block length or size
        /// </summary>
        int BlockSize { get; set; }

        /// <summary>
        /// Get the specified block of data from cache
        /// </summary>
        /// <param name="blockIndex">block index</param>
        /// <returns>block of data</returns>
        T GetData(int blockIndex);

        /// <summary>
        /// Add block of data into cache
        /// </summary>
        /// <param name="blockType">Block Type</param>
        /// <param name="startRange">Block starting index</param>
        /// <param name="endRange">Block ending index</param>
        void AddData(T blockType, long startRange, long endRange);

        /// <summary>
        /// Get all the blocks of data
        /// </summary>
        /// <returns>List of blocks of data</returns>
        IList<T> GetAllData();

        /// <summary>
        /// Clear unused data
        /// </summary>
        void ClearStaleData();
    }
}