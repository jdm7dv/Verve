//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



using System;

namespace Bio
{
    /// <summary>
    /// Cache manager for Data Virtualization. 
    /// Single block of data of type "T" is wrapped in CacheBox and 
    /// the blocks are maintained with last access time
    /// </summary>
    /// <typeparam name="T">Type of object to be cached.</typeparam>
    public class CacheBox<T>
    {
        #region Fields
        /// <summary>
        /// Size of a single block.
        /// </summary>
        private readonly long _blockSize;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the CacheBox class 
        /// </summary>
        public CacheBox(long blockSize)
        {
            Touch();
            _blockSize = blockSize;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets block starting range
        /// </summary>
        public long StartRange { get; set; }

        /// <summary>
        /// Gets or sets block ending range
        /// </summary>
        public long EndRange { get; set; }

        /// <summary>
        /// Gets block size.
        /// </summary>
        public long BlockSize
        {
            get
            {
                return _blockSize;
            }
        }

        /// <summary>
        /// Gets or sets last access time
        /// </summary>
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// Gets or sets block of data
        /// </summary>
        public T Data { get; set; }
        #endregion

        #region Public Method(s)
        /// <summary>
        /// Update last access time
        /// </summary>
        public void Touch()
        {
            LastAccessTime = DateTime.Now;
        }
        #endregion
    }
}
