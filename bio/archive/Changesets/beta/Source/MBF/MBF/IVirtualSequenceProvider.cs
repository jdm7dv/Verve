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
    public interface IVirtualSequenceProvider : IList<byte>
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

        #region Methods to work with char
        /// <summary>
        /// Finds the index of the first occurrence of a given symbol in the current sequence.
        /// </summary>
        /// <param name="item">The symbol whose index is required.</param>
        /// <returns>The index of the first occurrence of the symbol in the current sequence.</returns>
        int IndexOf(char item);

        /// <summary>
        /// Inserts a symbol into the sequence at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the symbol should be inserted.</param>
        /// <param name="item">The symbol to insert.</param>
        void Insert(int index, char item);

        /// <summary>
        /// Gets the symbol at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the symbol.</param>
        /// <returns> The symbol at the specified index.</returns>
        char GetItem(int index);

        /// <summary>
        /// Sets the symbol at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the symbol.</param>
        /// <param name="item">Symbol to set.</param>
        void SetItem(int index, char item);

        /// <summary>
        /// Adds a sequence item to the end of the sequence.
        /// </summary>
        /// <param name="item">The sequence item to be added to the end of the sequence.</param>
        void Add(char item);

        /// <summary>
        /// Determines whether a specific sequence item is in the current sequence.
        /// </summary>
        /// <param name="item">The symbol to locate in the current sequence.</param>
        /// <returns>true if the sequence item is found in the sequence; otherwise, false</returns>
        bool Contains(char item);

        /// <summary>
        /// Copies the entire sequence to a compatible one-dimensional array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements
        /// copied from the current sequence. The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        void CopyTo(char[] array, int arrayIndex);

        /// <summary>
        /// Removes the first occurrence of a specific symbol from the current sequence.
        /// </summary>
        /// <param name="item">The symbol to remove from the sequence.</param>
        /// <returns>true if item is successfully removed; otherwise, false.</returns>
        bool Remove(char item);

        #endregion

        #region Methods to work with ISequenceItem
        /// <summary>
        /// Finds the index of the first occurrence of a given symbol in the current sequence.
        /// </summary>
        /// <param name="item">The symbol whose index is required.</param>
        /// <returns>The index of the first occurrence of the symbol in the current sequence.</returns>
        int IndexOf(ISequenceItem item);

        /// <summary>
        /// Inserts a symbol into the sequence at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the symbol should be inserted.</param>
        /// <param name="item">The symbol to insert.</param>
        void Insert(int index, ISequenceItem item);

        /// <summary>
        /// Gets the symbol at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the symbol.</param>
        /// <returns> The symbol at the specified index.</returns>
        ISequenceItem GetISequenceItem(int index);

        /// <summary>
        /// Sets the symbol at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the symbol.</param>
        /// <param name="item">Symbol to set.</param>
        void SetISequenceItem(int index, ISequenceItem item);

        /// <summary>
        /// Adds a sequence item to the end of the sequence.
        /// </summary>
        /// <param name="item">The sequence item to be added to the end of the sequence.</param>
        void Add(ISequenceItem item);

        /// <summary>
        /// Determines whether a specific sequence item is in the current sequence.
        /// </summary>
        /// <param name="item">The symbol to locate in the current sequence.</param>
        /// <returns>true if the sequence item is found in the sequence; otherwise, false</returns>
        bool Contains(ISequenceItem item);

        /// <summary>
        /// Copies the entire sequence to a compatible one-dimensional array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements
        /// copied from the current sequence. The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        void CopyTo(ISequenceItem[] array, int arrayIndex);

        /// <summary>
        /// Removes the first occurrence of a specific symbol from the current sequence.
        /// </summary>
        /// <param name="item">The symbol to remove from the sequence.</param>
        /// <returns>true if item is successfully removed; otherwise, false.</returns>
        bool Remove(ISequenceItem item);
        #endregion
    }
}
