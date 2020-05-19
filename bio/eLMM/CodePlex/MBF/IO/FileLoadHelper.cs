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



using System.IO;
using System.Runtime.InteropServices;

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
        private readonly long _fileSize;

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
        /// Win32 struct for memory 
        /// </summary>
        public struct MemoryStatus
        {
            /// <summary>
            /// Length
            /// </summary>
            public uint Length;
            /// <summary>
            /// Memory Load
            /// </summary>
            public uint MemoryLoad;
            /// <summary>
            /// Total Physical memory
            /// </summary>
            public uint TotalPhysical;
            /// <summary>
            /// Available Physical memory
            /// </summary>
            public uint AvailablePhysical;
            /// <summary>
            /// Total Page File size
            /// </summary>
            public uint TotalPageFile;
            /// <summary>
            /// Available Page File size
            /// </summary>
            public uint AvailablePageFile;
            /// <summary>
            /// Total Virtual memory
            /// </summary>
            public uint TotalVirtual;
            /// <summary>
            /// Available Virtual memory
            /// </summary>
            public uint AvailableVirtual;
        }

        /// <summary>
        /// Get the memory status
        /// </summary>
        /// <param name="memoryStatus">status struct</param>
        [DllImport("kernel32.dll")]
        public static extern void GlobalMemoryStatus(out MemoryStatus memoryStatus);

        /// <summary>
        /// Initializes a new instance of the FileLoadHelper class.
        /// </summary>
        public FileLoadHelper(string fileName)
        {
            MaxNumberOfBlocks = DefaultMaxNumberOfBlocks;
            BlockSize = DefaultFullLoadBlockSize;

            FileInfo fileInfo = new FileInfo(fileName);
            _fileSize = fileInfo.Length;
            
            if (_fileSize  >= VirtualizationMemoryLimit)
            {
                //let DV kick-in with default size
                BlockSize = DefaultBlockSize;
            }
        }

        /// <summary>
        /// Max memory for virtualization
        /// </summary>
        private static long VirtualizationMemoryLimit
        {
            get
            {
                // providing 1/3 memory for DV
                //return AvailableMemory / 3;
                // default above 100 mb DV kicks in
                return 109951162777600;
            }
        }

        /// <summary>
        /// System's available memory
        /// </summary>
        private static long AvailableMemory
        {
            get
            {
                MemoryStatus stat = new MemoryStatus();
                try
                {
                    GlobalMemoryStatus(out stat);

                    // 64 bit is retruning available memory as 0.
                    if (stat.AvailablePhysical == 0)
                    {
                        //TODO: get the right memory on 64 bit.
                        // Temp: assume 10MB is free
                        return 10485760;
                    }
                    return stat.AvailablePhysical;
                }
                catch // On any exception
                {
                    return 10485760;
                }
            }
        }
    }
}
