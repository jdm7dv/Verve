// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF
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
