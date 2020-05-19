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
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Bio.IO
{
    /// <summary>
    /// Used for data virtualization to determine the block info
    /// </summary>
    public class FileLoadHelper
    {
        /// <summary>
        /// file size
        /// </summary>
        private readonly long fileSize;

        /// <summary>
        /// 1 KB
        /// </summary>
        public const int KBytes = 1024;

        /// <summary>
        /// 1 MB
        /// </summary>
        public const int MBytes = 1024 * KBytes;

        /// <summary>
        /// Default block size
        /// </summary>
        public const int DefaultBlockSize = 4096;

        /// <summary>
        /// Default number of blocks
        /// </summary>
        public const int DefaultMaxNumberOfBlocks = 5;

        /// <summary>
        /// Full load default block size
        /// </summary>
        public const int DefaultFullLoadBlockSize = -1;

        /// <summary>
        /// Gets or sets block size
        /// </summary>
        public int BlockSize { get; set; }

        /// <summary>
        /// Gets or sets maximum number of blocks in cache
        /// </summary>
        public int MaxNumberOfBlocks { get; set; }

        /// <summary>
        /// Attach the performance counter on Memory
        /// </summary>
        private static PerformanceCounter performanceCounter = new PerformanceCounter("Memory", "Available MBytes");

        /// <summary>
        /// Initializes a new instance of the FileLoadHelper class.
        /// </summary>
        public FileLoadHelper(string fileName)
        {
            MaxNumberOfBlocks = DefaultMaxNumberOfBlocks;
            BlockSize = DefaultFullLoadBlockSize;

            FileInfo fileInfo = new FileInfo(fileName);
            fileSize = fileInfo.Length;

            //get the available memory from perf counter
            long totalAvailableMemory = performanceCounter.RawValue * MBytes;

            // DV is limited to use 25% of available physical memory
            totalAvailableMemory = totalAvailableMemory / 4L;

            if (fileSize >= totalAvailableMemory)
            {
                //let DV kick-in with default size
                BlockSize = DefaultBlockSize;
            }
        }
    }
}
