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



namespace Bio
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
