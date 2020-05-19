// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;

namespace MBF.IO.BAM
{
    /// <summary>
    /// Class to hold offset of a BAM file.
    /// </summary>
    public class FileOffset
    {
        /// <summary>
        /// Gets or sets BGZF block start offset.
        /// </summary>
        public UInt64 CompressedBlockOffset { get; set; }

        /// <summary>
        /// Gets or sets an offset of uncompressed block inside a BGZF block 
        /// from which aligned sequences starts or ends.
        /// </summary>
        public UInt16 UncompressedBlockOffset { get; set; }
    }
}
