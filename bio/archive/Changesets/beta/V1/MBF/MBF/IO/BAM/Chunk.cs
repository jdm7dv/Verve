// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.IO.BAM
{
    /// <summary>
    /// Class to hold start and end offsets of a BAM file chunk related to a bin.
    /// </summary>
    public class Chunk
    {
        /// <summary>
        /// Gets or sets start offset of this chunk.
        /// </summary>
        public FileOffset ChunkStart { get; set; }

        /// <summary>
        /// Gets or sets end offset of this chunk.
        /// </summary>
        public FileOffset ChunkEnd { get; set; }
    }
}
