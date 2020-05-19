// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using MBF.IO.FastQ;

namespace MBF.IO
{
    /// <summary>
    /// This class holds the virtual data (specific to QualitativeSequence) and parser info for each QualitativeSequence
    /// This class is used by the Data Virtualization on Large data where it reads block by block.
    /// </summary>
    public class FileVirtualQualitativeSequenceProvider : IVirtualQualitativeSequenceProvider
    {
        #region Fields
        /// <summary>
        /// Instance of a Virtual data holder.
        /// </summary>
        private readonly VirtualData<byte[]> _virtualData;

        /// <summary>
        /// Instance of a Virtual Sequence Parser.
        /// </summary>
        private readonly IVirtualSequenceParser _parser;

        /// <summary>
        /// The number of sequence items contained in the sequence.
        /// </summary>
        private int _count;

        /// <summary>
        /// Holds Alphabet of the sequence.
        /// </summary>
        private IAlphabet _alphabet;

        /// <summary>
        /// A collection of edited sequences.
        /// </summary>
        private readonly Dictionary<int, CacheBox<IDerivedSequence>> _editedSequences = new Dictionary<int, CacheBox<IDerivedSequence>>();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the MBF.IO.FileVirtualQualitativeSequenceProvider  
        /// class to hold a sequence pointer with a parser.
        /// </summary>
        /// <param name="parser">A virtual parser object.</param>
        /// <param name="pointer">The sequence pointer.</param>
        public FileVirtualQualitativeSequenceProvider (IVirtualSequenceParser parser, SequencePointer pointer)
        {
            if (parser == null)
            {
                throw new ArgumentNullException("parser");
            }

            if (pointer == null)
            {
                throw new ArgumentNullException("pointer");
            }

            _parser = parser;
            _count = (int)(pointer.IndexOffsets[1] - pointer.IndexOffsets[0]);
            SequencePointerInstance = pointer;
            _alphabet = Alphabets.All.FirstOrDefault(A => A.Name.Equals(pointer.AlphabetName));

            //set the default DV properties.
            _virtualData = new VirtualData<byte[]>
                               {
                                   BlockSize = FileLoadHelper.DefaultBlockSize,
                                   MaxNumberOfBlocks = FileLoadHelper.DefaultMaxNumberOfBlocks
                               };
            
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the block size.
        /// </summary>
        public int BlockSize
        {
            get { return _virtualData.BlockSize; }
            set { _virtualData.BlockSize = value; }
        }

        /// <summary>
        /// Gets or sets maximum number of blocks per sequence
        /// </summary>
        public int MaxNumberOfBlocks
        {
            get { return _virtualData.MaxNumberOfBlocks; }
            set { _virtualData.MaxNumberOfBlocks = value; }
        }

        /// <summary>
        /// Gets the number of symbols in the sequence.
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the sequence is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get;
            set;
        }

        /// <summary>
        /// Holds a sequence pointer for DV to access the sequence
        /// </summary>
        public SequencePointer SequencePointerInstance
        {
            get;
            set;
        }
        #endregion

        #region IVirtualQualitativeSequenceProvider Members
        /// <summary>
        /// Gets or sets the symbol at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the symbol to get or set.</param>
        /// <returns> The symbol at the specified index.</returns>
        public byte this[int index]
        {
            get
            {
                int position = index;
                if (_editedSequences != null && _editedSequences.Values.Count > 0)
                {
                    CacheBox<IDerivedSequence> editedBlock = GetEditedBlock(position);
                    if (editedBlock != null)
                    {
                        IDerivedSequence derivedSeq = editedBlock.Data;
                        return (byte)derivedSeq[position - (int)editedBlock.StartRange].Symbol;
                    }
                    else
                    {
                        position = position + (int)GetPositionDiff(position);
                    }
                }

                CacheBox<byte[]> block = GetCacheBlock(position);
                if (block != null)
                {
                    return block.Data[position - (int)block.StartRange];
                }
                else
                {
                    return default(byte);
                }
            }

            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);
                Replace(index, value);
            }
        }

        /// <summary>
        /// Gets the symbol at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the symbol.</param>
        /// <returns> The symbol at the specified index.</returns>
        public char GetItem(int index)
        {
            return (char)this[index];
        }

        /// <summary>
        /// Gets the symbol at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the symbol.</param>
        /// <returns> The symbol at the specified index.</returns>
        public ISequenceItem GetISequenceItem(int index)
        {
            return _alphabet.LookupByValue(this[index]);
        }

        /// <summary>
        /// Sets the symbol at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the symbol.</param>
        /// <param name="item">Symbol to set.</param>
        public void SetItem(int index, char item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);
            Replace(index, item);
        }

        /// <summary>
        /// Sets the symbol at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the symbol.</param>
        /// <param name="item">Symbol to set.</param>
        public void SetISequenceItem(int index, ISequenceItem item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);
            Replace(index, item);
        }


        /// <summary>
        /// Finds the index of the first occurrence of a given symbol in the current sequence.
        /// </summary>
        /// <param name="item">The symbol whose index is required.</param>
        /// <returns>The index of the first occurrence of the symbol in the current sequence.</returns>
        public int IndexOf(ISequenceItem item)
        {
            if (item == null)
            {
                return -1;
            }

            return IndexOf(item.Symbol);
        }

        /// <summary>
        /// Finds the index of the first occurrence of a given symbol in the current sequence.
        /// </summary>
        /// <param name="item">The symbol whose index is required.</param>
        /// <returns>The index of the first occurrence of the symbol in the current sequence.</returns>
        public int IndexOf(byte item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (item == this[i])
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Finds the index of the first occurrence of a given symbol in the current sequence.
        /// </summary>
        /// <param name="item">The symbol whose index is required.</param>
        /// <returns>The index of the first occurrence of the symbol in the current sequence.</returns>
        public int IndexOf(char item)
        {
            return IndexOf((byte)item);
        }

        /// <summary>
        /// Inserts a sequence item into the sequence at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="item">The sequence item to insert.</param>
        public void Insert(int index, ISequenceItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameItem);
            }

            Insert(index, item.Symbol);
        }

        /// <summary>
        /// Inserts a symbol into the sequence at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the item should be inserted.</param>
        /// <param name="item">The symbol to insert.</param>
        public void Insert(int index, byte item)
        {
            Insert(index, (char)item);
        }

        /// <summary>
        /// Inserts a symbol into the sequence at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the symbol should be inserted.</param>
        /// <param name="item">The symbol to insert.</param>
        public void Insert(int index, char item)
        {
            InsertRange(index, item.ToString());
        }

        /// <summary>
        /// Removes the symbol at the specified index of the sequence.
        /// </summary>
        /// <param name="index">The zero-based index of the symbol to remove.</param>
        public void RemoveAt(int index)
        {
            RemoveRange(index, 1);
        }

        /// <summary>
        /// Replaces a sequence item at a specified index of the sequence.
        /// </summary>
        /// <param name="index">The zero-based index of the sequence item to be replaced.</param>
        /// <param name="item">The sequence item to replace the existing item with.</param>
        public void Replace(int index, ISequenceItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            Replace(index, item.Symbol);
        }

        /// <summary>
        /// Replaces a symbol at a specified index of the sequence.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the symbol to be replaced.
        /// </param>
        /// <param name="item">The symbol to replace the existing symbol with.</param>
        public void Replace(int index, char item)
        {
            RemoveAt(index);
            InsertRange(index, item.ToString());
        }

        /// <summary>
        /// Replaces a symbol at a specified index of the sequence.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the symbol to be replaced.
        /// </param>
        /// <param name="item">The symbol to replace the existing symbol with.</param>
        public void Replace(int index, byte item)
        {
            Replace(index, (char)item);
        }

        #endregion

        #region ICollection<ISequenceItem> Members

        /// <summary>
        /// Adds a sequence item to the end of the sequence.
        /// </summary>
        /// <param name="item">The sequence item to be added to the end of the sequence.</param>
        public void Add(ISequenceItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameItem);
            }

            Add(item.Symbol);
        }

        /// <summary>
        /// Adds a sequence item to the end of the sequence.
        /// </summary>
        /// <param name="item">The sequence item to be added to the end of the sequence.</param>
        public void Add(char item)
        {
            InsertRange(Count, item.ToString());
        }

        /// <summary>
        /// Adds a sequence item to the end of the sequence.
        /// </summary>
        /// <param name="item">The sequence item to be added to the end of the sequence.</param>
        public void Add(byte item)
        {
            Add((char)item);
        }

        /// <summary>
        /// Removes all symbols from the sequence.
        /// </summary>
        public void Clear()
        {
            RemoveRange(0, Count);
        }

        /// <summary>
        /// Determines whether a specific sequence item is in the current sequence.
        /// </summary>
        /// <param name="item">The sequence item to locate in the current sequence.</param>
        /// <returns>true if the sequence item is found in the sequence; otherwise, false</returns>
        public bool Contains(ISequenceItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameItem);
            }
            return Contains(item.Symbol);
        }

        /// <summary>
        /// Determines whether a specific sequence item is in the current sequence.
        /// </summary>
        /// <param name="item">The symbol to locate in the current sequence.</param>
        /// <returns>true if the sequence item is found in the sequence; otherwise, false</returns>
        public bool Contains(char item)
        {
            return Contains((byte)item);
        }

        /// <summary>
        /// Determines whether a specific sequence item is in the current sequence.
        /// </summary>
        /// <param name="item">The sequence item to locate in the current sequence.</param>
        /// <returns>true if the sequence item is found in the sequence; otherwise, false</returns>
        public bool Contains(byte item)
        {
            if (IndexOf(item) > -1)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Copies the entire sequence to a compatible one-dimensional array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements
        /// copied from the current sequence. The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(ISequenceItem[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameArray);
            }

            int index = arrayIndex;
            foreach(byte item in this)
            {
                array[index++] = _alphabet.LookupBySymbol((char)item);
            }
        }

        /// <summary>
        /// Copies the entire sequence to a compatible one-dimensional array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements
        /// copied from the current sequence. The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(char[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameArray);
            }

            int index = arrayIndex;
            foreach (byte item in this)
            {
                array[index++] = (char)item;
            }
        }

        /// <summary>
        /// Copies the entire sequence to a compatible one-dimensional array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements
        /// copied from the current sequence. The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(byte[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameArray);
            }

            if (arrayIndex < 0 || (arrayIndex + Count) > array.Length)
            {
                throw new ArgumentException(Properties.Resource.DestArrayNotLargeEnoughError);
            }

            int index = arrayIndex;
            foreach (byte item in this)
            {
                array[index++] = item;
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific sequence item from the current sequence.
        /// </summary>
        /// <param name="item">The sequence item to remove from the sequence.</param>
        /// <returns>true if item is successfully removed; otherwise, false.</returns>
        public bool Remove(ISequenceItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameItem);
            }

            return Remove(item.Symbol);
        }

        /// <summary>
        /// Removes the first occurrence of a specific symbol from the current sequence.
        /// </summary>
        /// <param name="item">The symbol to remove from the sequence.</param>
        /// <returns>true if item is successfully removed; otherwise, false.</returns>
        public bool Remove(char item)
        {
            return Remove((byte)item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific symbol from the current sequence.
        /// </summary>
        /// <param name="item">The symbol to remove from the sequence.</param>
        /// <returns>true if item is successfully removed; otherwise, false.</returns>
        public bool Remove(byte item)
        {
            int index = IndexOf(item);

            if (index > -1)
            {
                RemoveRange(index, 1);
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable<ISequenceItem> Members
        /// <summary>
        /// Get the enumerator to the symbols in sequence
        /// </summary>
        /// <returns>Enumerator to the symbols in sequence</returns>
        public IEnumerator<byte> GetEnumerator()
        {
            return new GenericIListEnumerator<byte>(this);
        }

        #endregion

        #region IVirtualSequenceProvider Members
        /// <summary>
        /// Removes a range of symbols from the sequence.
        /// </summary>
        /// <param name="position">The zero-based starting index of the range of symbols to remove.</param>
        /// <param name="length">The number of symbols to remove.</param>
        public void RemoveRange(int position, int length)
        {
            int currentPosition = position;
            int localPosition;
            IDerivedSequence derivedSequence;
            CacheBox<IDerivedSequence> editedBlock = GetEditedBlock(currentPosition);

            if (editedBlock != null)
            {
                derivedSequence = editedBlock.Data;
                localPosition = currentPosition - (int)editedBlock.StartRange;
            }
            else
            {
                int positionDiff = (int)GetPositionDiff(currentPosition);
                currentPosition = currentPosition + positionDiff;
                editedBlock = CreateEditedSequenceBlock(currentPosition);
                derivedSequence = editedBlock.Data;
                editedBlock.StartRange -= positionDiff;
                editedBlock.EndRange -= positionDiff;
                localPosition = position - (int)editedBlock.StartRange;
            }

            int removableLength = length;
            if ( (derivedSequence.Count -localPosition) < length)
            {
                removableLength = derivedSequence.Count - localPosition;
            }

            derivedSequence.RemoveRange(localPosition, removableLength);
            editedBlock.EndRange = editedBlock.EndRange - removableLength;
            UpdateStartAndEndRanges(editedBlock.StartRange, -removableLength);
            _count -= removableLength;

            if (removableLength != length)
            {
                RemoveRange(position, length - removableLength);
            }
        }

        /// <summary>
        /// Inserts a string of symbols into the sequence at the specified index.
        /// </summary>
        /// <param name="position">The zero-based index at which the new symbols should be inserted.</param>
        /// <param name="sequence">The string of symbols which should be inserted into the sequence.</param>
        public void InsertRange(int position, string sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequence);
            }

            int currentPosition = position;
            int localPosition;
            IDerivedSequence derivedSequence;
            CacheBox<IDerivedSequence> editedBlock;

            if (position < Count)
            {
                editedBlock = GetEditedBlock(currentPosition);
            }
            else
            {
                editedBlock = GetEditedBlock(currentPosition - 1);
            }

            if (editedBlock != null)
            {
                derivedSequence = editedBlock.Data;
                localPosition = currentPosition - (int)editedBlock.StartRange;
            }
            else
            {
                int positionDiff = (int)GetPositionDiff(currentPosition);
                currentPosition = currentPosition + positionDiff;
                if (position < Count)
                {
                    editedBlock = CreateEditedSequenceBlock(currentPosition);
                }
                else
                {
                    editedBlock = CreateEditedSequenceBlock(currentPosition - 1);
                }
                derivedSequence = editedBlock.Data;
                editedBlock.StartRange -= positionDiff;
                editedBlock.EndRange -= positionDiff;
                localPosition = position - (int)editedBlock.StartRange;
            }

            derivedSequence.InsertRange(localPosition, sequence);
            editedBlock.EndRange = editedBlock.EndRange + sequence.Length;
            UpdateStartAndEndRanges(editedBlock.StartRange, sequence.Length);
            _count += sequence.Length;
        }

        /// <summary>
        /// Replaces the symbols starting from a specified index of a sequence.
        /// </summary>
        /// <param name="index">The zero-based starting index of the symbols to replace in the sequence.</param>
        /// <param name="sequence">The symbols to replace the existing symbols with.</param>
        public void ReplaceRange(int index, string sequence)
        {
            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequence);
            }

            RemoveRange(index, sequence.Length);
            InsertRange(index, sequence);
        }

        #endregion IVirtualSequenceProvider Members

        #region IVirtualQualitativeSequenceProvider Members
        /// <summary>
        /// Get the quality scores of a particular sequence.
        /// </summary>
        /// <returns>The quality scores.</returns>
        public byte[] GetScores()
        {
            int includesNewline = SequencePointerInstance.StartingLine * Environment.NewLine.Length;
            long qualScoresStartingIndex = SequencePointerInstance.IndexOffsets[1] + 1 + SequencePointerInstance.Id.Length + (Environment.NewLine.Length * 2)
                + includesNewline;

            return ((FastQParser)_parser).GetQualityScores(qualScoresStartingIndex);
        }
        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Get the enumerator to the symbols in sequence
        /// </summary>
        /// <returns>Enumerator to the symbols in sequence</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Updates the start and end ranges of the edited sequences.
        /// </summary>
        /// <param name="position">The positions to be edited.</param>
        /// <param name="value">The value to update the ranges by.</param>
        private void UpdateStartAndEndRanges(long position, int value)
        {
            if (_editedSequences != null && _editedSequences.Values.Count > 0)
            {
                List<CacheBox<IDerivedSequence>> list = _editedSequences.Values.Where(E => E.StartRange > position).ToList();
                foreach (CacheBox<IDerivedSequence> editedBlock in list)
                {
                    editedBlock.StartRange += value;
                    editedBlock.EndRange += value;
                }
            }
        }

        /// <summary>
        /// Gets a derived sequence block if the specified position is contained in it.
        /// </summary>
        /// <param name="position">The position inside the derived sequence block.</param>
        /// <returns>The derived sequence block.</returns>
        private CacheBox<IDerivedSequence> GetEditedBlock(int position)
        {
            return _editedSequences.Values.FirstOrDefault(E => E.StartRange <= position && E.EndRange >= position);
        }

        /// <summary>
        /// Creates an edited sequence block at the specified position.
        /// </summary>
        /// <param name="position">The position to create the block at.</param>
        /// <returns>The edited sequence block.</returns>
        private CacheBox<IDerivedSequence> CreateEditedSequenceBlock(int position)
        {
            CacheBox<byte[]> block = GetCacheBlock(position);
            char[] chars = new char[block.Data.Length];
            block.Data.CopyTo(chars, 0);
            Sequence seq = new Sequence(_alphabet, new string(chars));
            IDerivedSequence derivedSeq = new DerivedSequence(seq);
            int listindex = position / BlockSize;

            CacheBox<IDerivedSequence> cache = new CacheBox<IDerivedSequence>(block.BlockSize)
                                                   {
                                                       StartRange = block.StartRange,
                                                       EndRange = block.EndRange,
                                                       Data = derivedSeq
                                                   };

            _editedSequences.Add(listindex, cache);
            return cache;
        }

        /// <summary>
        /// Gets a block from the cache at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the block.</param>
        /// <returns>The cached sequence block.</returns>
        private CacheBox<byte[]> GetCacheBlock(int index)
        {
            CacheBox<byte[]> block = _virtualData.FirstOrDefault(C => index >= C.StartRange && index <= C.EndRange);

            if (block != null)
            {
                return block;
            }
            else
            {

                int startIndex = index - (index % BlockSize);
                byte[] seq = _parser.ParseRange(startIndex, BlockSize, SequencePointerInstance);

                if (seq == null)
                {
                    return null;
                }
                else
                {
                    block = new CacheBox<byte[]>(seq.Length) { StartRange = startIndex };
                    block.EndRange = block.StartRange + seq.Length - 1;
                    block.Data = seq;

                    _virtualData.Add(block);
                    return block;
                }
            }
        }

        /// <summary>
        /// Gets the difference in positions between the cached and edited version of a block.
        /// </summary>
        /// <param name="position">The position in the cached block.</param>
        /// <returns>The position in the edited block.</returns>
        private long GetPositionDiff(int position)
        {
            return _editedSequences.Values.Where(E => E.EndRange < position).Sum(E => (E.BlockSize - (E.EndRange + 1 - E.StartRange)));
        }

        #endregion
    }
}
