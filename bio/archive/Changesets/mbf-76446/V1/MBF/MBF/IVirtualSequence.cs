// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF
{
    /// <summary>
    /// Interface provides the data virtualization on top of ISequence
    /// </summary>
    public interface IVirtualSequence : ISequence
    {
        /// <summary>
        /// Gets or sets maximum number of blocks per sequence
        /// </summary>
        int MaxNumberOfBlocks { get; set; }

        /// <summary>
        /// Gets or sets single block length or size
        /// </summary>
        int BlockSize { get; set; }
    }
}