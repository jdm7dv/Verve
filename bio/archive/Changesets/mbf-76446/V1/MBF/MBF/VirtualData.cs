// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MBF
{
    /// <summary>
    /// This class maintains a data virtualization list for caching purpose
    /// Caching mechanism  works with T and least-used-out approach based on
    /// the last accessed time. The cache data (T) is maintained for 10 sec 
    /// as default. Data Virtualization uses this class to hold the partial 
    /// sequences based on max number of blocks.
    /// </summary>
    /// <typeparam name="T">Underlying type</typeparam>
    public class VirtualData<T> : IVirtualData<T> where T : class
    {
        #region Fields
        /// <summary>
        /// time out to clean from cache
        /// </summary>
        private readonly int _timeout;

        /// <summary>
        /// cache holder
        /// </summary>
        private readonly List<CacheBox<T>> _cachedData;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the VirtualData class.
        /// </summary>
        public VirtualData()
        {
            _cachedData = new List<CacheBox<T>>();

            // default timeout is 10sec
            _timeout = 10;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets maximum number of blocks per sequence
        /// </summary>
        public int MaxNumberOfBlocks { get; set; }

        /// <summary>
        /// Gets or sets block size
        /// </summary>
        public int BlockSize { get; set; }

        /// <summary>
        /// cache count
        /// </summary>
        public int Count
        {
            get { return _cachedData.Count; }
        }

        /// <summary>
        /// Default read only
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region IList<CacheBox<T>>
        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="index">index</param>
        /// <returns>cachebox of T</returns>
        public CacheBox<T> this[int index]
        {
            get { return _cachedData[index]; }
            set { _cachedData[index] = value; }
        }

        /// <summary>
        /// Gets index of given item
        /// </summary>
        /// <param name="item">Item whose index is required</param>
        /// <returns>index of item</returns>
        public int IndexOf(CacheBox<T> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return _cachedData.IndexOf(item);
        }

        /// <summary>
        /// Insert into cached data
        /// </summary>
        /// <param name="index">index at which item has to be inserted</param>
        /// <param name="item">cachebox item</param>
        public void Insert(int index, CacheBox<T> item)
        {
            if (index <= -1)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            _cachedData.Insert(index, item);
        }

        /// <summary>
        /// remove at the index
        /// </summary>
        /// <param name="index">index at which item has to be removed</param>
        public void RemoveAt(int index)
        {
            if (index <= -1 )
            {
                throw new ArgumentOutOfRangeException("index");
            }

            _cachedData.RemoveAt(index);
        }
        #endregion

        #region IVirtualData<T> Members
        /// <summary>
        /// pass file block index to get the data from cache
        /// </summary>
        /// <param name="blockIndex">block index</param>
        /// <returns>Return the data from block</returns>
        public T GetData(int blockIndex)
        {
            if (blockIndex <= -1)
            {
                throw new ArgumentOutOfRangeException("blockIndex");
            }

            CacheBox<T> block = this[blockIndex];
            return block.Data;
        }

        /// <summary>
        /// add block of data into cache
        /// </summary>
        /// <param name="blockType">Block Type</param>
        /// <param name="startRange">Block starting index</param>
        /// <param name="endRange">Block ending index</param>
        public void AddData(T blockType, long startRange, long endRange)
        {
            if (blockType == null)
            {
                throw new ArgumentNullException("blockType");
            }

            if (startRange <= -1)
            {
                throw new ArgumentOutOfRangeException("startRange");
            }

            if (endRange <= -1)
            {
                throw new ArgumentOutOfRangeException("endRange");
            }

            if (startRange >= endRange)
            {
                throw new ArgumentOutOfRangeException("startRange");
            }

            if (_cachedData.Count >= MaxNumberOfBlocks && _timeout > 0)
            {
                ClearStaleData();
            }

            CacheBox<T> block = new CacheBox<T>(endRange - startRange + 1)
                                    {
                                        StartRange = startRange,
                                        EndRange = endRange,
                                        Data = blockType
                                    };
            _cachedData.Add(block);
        }

        /// <summary>
        /// get all the blocks currently
        /// </summary>
        /// <returns>List of Ts</returns>
        public IList<T> GetAllData()
        {
            return _cachedData.ConvertAll(C => C.Data);
        }

        /// <summary>
        /// clear unused data
        /// </summary>
        public void ClearStaleData()
        {
            int countBefore = _cachedData.Count;
            var staleData = (from entry in _cachedData.AsEnumerable()
                             let now = DateTime.Now
                             where (now - entry.LastAccessTime).TotalSeconds > _timeout
                             select entry).ToList();

            lock (_cachedData)
            {
                foreach (var key in staleData)
                {
                    _cachedData.Remove(key);
                }
            }

            if (_cachedData.Count >= countBefore)
            {
                //try to clean one of block
                ForceMinimumTimeRemove();
            }
        }
        #endregion

        #region ICollection<T> Members
        /// <summary>
        /// add into cached data
        /// </summary>
        /// <param name="item">cachebox item</param>
        public void Add(CacheBox<T> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (_cachedData.Count >= MaxNumberOfBlocks && _timeout > 0)
            {
                ClearStaleData();
            }

            _cachedData.Add(item);
        }

        /// <summary>
        /// clear cache
        /// </summary>
        public void Clear()
        {
            _cachedData.Clear();
        }

        /// <summary>
        /// Contatains or not
        /// </summary>
        /// <param name="item">cachebox item</param>
        /// <returns>true or false</returns>
        public bool Contains(CacheBox<T> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return _cachedData.Contains(item);
        }

        /// <summary>
        /// copyto array
        /// </summary>
        /// <param name="array">array</param>
        /// <param name="arrayIndex">array index</param>
        public void CopyTo(CacheBox<T>[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (arrayIndex <= -1)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }

            _cachedData.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// remove item from cache
        /// </summary>
        /// <param name="item">item to be removed</param>
        /// <returns>true or false</returns>
        public bool Remove(CacheBox<T> item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return _cachedData.Remove(item);
        }
        #endregion

        #region IEnumerator<CacheBox<T>> Members
        /// <summary>
        /// Get enumerator of cache T
        /// </summary>
        /// <returns>enumerator of cachebox</returns>
        public IEnumerator<CacheBox<T>> GetEnumerator()
        {
            return _cachedData.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Get enumerator of cache
        /// </summary>
        /// <returns>enumerator of cachebox</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cachedData.GetEnumerator();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Force for single space
        /// </summary>
        private void ForceMinimumTimeRemove()
        {
            var sortedData = (from cachebox in _cachedData
                              orderby cachebox.LastAccessTime
                              select cachebox).ToList();

            lock (_cachedData)
            {
                if (sortedData.Any(key => _cachedData.Remove(key)))
                {
                    return;
                }
            }
        }
        #endregion
    }
}