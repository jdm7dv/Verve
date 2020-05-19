// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
namespace BioExcel
{
    using System;

    /// <summary>
    /// Class to hold the cached data and related information in the cache
    /// </summary>
    class CachedSequenceData
    {
        /// <summary>
        /// Data being held in cache
        /// </summary>
        public object CachedData { get; set; }

        /// <summary>
        /// Last accessed time of this object
        /// Used to remove items which is not recently accessed when the cache is full
        /// </summary>
        public DateTime LastAccessTime { get; set; }

        /// <summary>
        /// Creates a new CachedSequence object
        /// </summary>
        /// <param name="objectToCache">Object to the added to the cache</param>
        public CachedSequenceData(object objectToCache)
        {
            this.CachedData = objectToCache;
            this.LastAccessTime = DateTime.Now;
        }
    }
}
