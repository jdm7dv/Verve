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



using System.Collections.Generic;

namespace Bio
{
    /// <summary>
    /// Interface to virtual sequence data.
    /// Classes which implement this interface should hold virtual data of sequence.
    /// </summary>
    public interface IVirtualSequenceProvider : IList<ISequenceItem>
    {
        /// <summary>
        /// Gets or sets block size.
        /// </summary>
        int BlockSize { get; set; }

        /// <summary>
        /// Gets or sets maximum number of blocks per sequence
        /// </summary>
        int MaxNumberOfBlocks { get; set; }

        /// <summary>
        /// Inserts a string of symbols into the sequence at the specified position.
        /// </summary>
        /// <param name="position">The zero-based index at which the new symbols should be inserted.</param>
        /// <param name="sequence">The string of symbols which should be inserted into the sequence.</param>
        void InsertRange(int position, string sequence);

        /// <summary>
        /// Removes a range of symbols from the sequence.
        /// </summary>
        /// <param name="position">The zero-based starting index of the range of symbols to remove.</param>
        /// <param name="length">The number of symbols to remove.</param>
        void RemoveRange(int position, int length);
    }
}
