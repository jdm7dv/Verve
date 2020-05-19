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



using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Bio.Properties;
using Bio.Util;

namespace Bio
{
    /// <summary>
    /// DerivedSequence maintains source sequence along with changes.
    /// Source property provides the source sequence.
    /// 
    /// This class internally maintains separate indices to keep track of the changes, this is called internal indices. 
    /// Count of internal indices will increase with insert of sequence items and it will decrease only when previously 
    /// inserted sequence items are removed.
    /// Note that it will not decrease if the removed item had not been inserted previously.
    /// 
    /// For example:
    /// Consider a source sequence "AGCT". 
    /// 
    ///                   A G C T
    /// original indices  0 1 2 3 
    /// current indices   0 1 2 3 
    /// internal indices  0 1 2 3
    /// 
    /// Insert sequence item "A" at position 2. This will increase the count of internal indices.
    ///                   A G A C T
    /// original indices  0 1   2 3 
    /// current indices   0 1 2 3 4
    /// internal indices  0 1 2 3 4
    /// 
    /// Remove item at position 1. This will not decrease the count of internal indices.
    ///                   A G A C T
    /// original indices  0 1   2 3 
    /// current indices   0   1 2 3
    /// internal indices  0 1 2 3 4
    /// 
    /// Remove item at position 1 which was previously inserted. This will reduce the count of interal indices.
    /// 
    ///                   A G C T
    /// original indices  0 1 2 3                  
    /// current indices   0   1 2 
    /// internal indices  0 1 2 3 
    /// </summary>
    [Serializable]
    public class DerivedSequence : IDerivedSequence
    {
        #region Fields

        /// <summary>
        /// Holds original sequence.
        /// </summary>
        private ISequence _source;

        /// <summary>
        /// Holds changes made to this DerivedSequence.
        /// </summary>
        private SortedDictionary<int, UpdatedSequenceItem> _updatedItems;
        #endregion Fields

        #region Constructors

        /// <summary>
        /// Constructor for clone method.
        /// </summary>
        /// <param name="otherDerivedSequence">otherDerivedSequence to clone.</param>
        private DerivedSequence(DerivedSequence otherDerivedSequence)
        {
            _source = otherDerivedSequence._source.Clone();
            _updatedItems = new SortedDictionary<int, UpdatedSequenceItem>();

            foreach (int key in otherDerivedSequence._updatedItems.Keys)
            {
                UpdatedSequenceItem item = new UpdatedSequenceItem(
                    otherDerivedSequence._updatedItems[key].SequenceItem,
                    otherDerivedSequence._updatedItems[key].Type);

                if (item.SequenceItem != Alphabet.LookupBySymbol(item.SequenceItem.Symbol))
                {
                    item.SequenceItem = item.SequenceItem.Clone();
                }

                _updatedItems.Add(key, item);
            }
        }

        /// <summary>
        /// Creates DerivedSequence from the specified source sequence.
        /// </summary>
        /// <param name="source">source sequence.</param>
        public DerivedSequence(ISequence source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameSource);
            }

            _source = source;
            _updatedItems = new SortedDictionary<int, UpdatedSequenceItem>();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Creates a new DerivedSequence that is a copy of the current DerivedSequence.
        /// </summary>
        /// <returns>A new DerivedSequence that is a copy of this DerivedSequence.</returns>
        public DerivedSequence Clone()
        {
            return new DerivedSequence(this);
        }

        /// <summary>
        /// Returns the changes made to the derived sequence as a list of IndexedItem&lt;UpdatedSequenceItem&gt;.
        /// Note that this method returns internal indices and not the current indices. 
        /// In case of insert and replace operations UpdatedSequenceItem will contain sequence item which 
        /// was inserted or replaced. For remove operation it will contain removed sequence item.
        /// </summary>
        /// <returns>List of IndexedItem&lt;UpdatedSequenceItem&gt;.</returns>
        public IList<IndexedItem<UpdatedSequenceItem>> GetUpdatedItems()
        {
            List<IndexedItem<UpdatedSequenceItem>> seqItems = new List<IndexedItem<UpdatedSequenceItem>>();
            foreach (int key in _updatedItems.Keys)
            {
                seqItems.Add(new IndexedItem<UpdatedSequenceItem>(key, new UpdatedSequenceItem(_updatedItems[key])));
            }

            return seqItems;
        }

        /// <summary>
        /// Returns count of removed items.
        /// </summary>
        /// <returns>Total number of removed items.</returns>
        private int GetAllRemovedItemsCount()
        {
            return _updatedItems.Count(T => T.Value.Type == UpdateType.Removed);
        }

        /// <summary>
        /// Returns total number of inserted items.
        /// </summary>
        /// <returns>Total number of inserted items.</returns>
        private int GetAllInsertedItemsCount()
        {
            return _updatedItems.Count(T => T.Value.Type == UpdateType.Inserted);
        }

        /// <summary>
        /// Returns removed items from zero to specified index (including specified index).
        /// </summary>
        /// <param name="index">Zero based index.</param>
        /// <returns>Returns count of removed items.</returns>
        private int GetRemovedItemsCount(int index)
        {
            return _updatedItems.Count(T => T.Key <= index && T.Value.Type == UpdateType.Removed);
        }

        /// <summary>
        /// Returns inserted items from zero to specified index (excluding specified index).
        /// </summary>
        /// <param name="index">Zero based index.</param>
        /// <returns>Returns count of inserted items.</returns>
        private int GetInsertedItemsCount(int index)
        {
            return _updatedItems.Count(T => T.Key < index && T.Value.Type == UpdateType.Inserted);
        }

        /// <summary>
        /// Updates indices of items which is greater than or equal to the specified index
        /// with the specified value.
        /// </summary>
        /// <param name="index">Index from which update has to be done.</param>
        /// <param name="value">Value with which positions have to be updated.</param>
        private void UpdatePositions(int index, int value)
        {
            List<int> itemsPositionToUpdate = _updatedItems.Keys.Where(I => I >= index).ToList();
            itemsPositionToUpdate.Sort();

            if (value > 0)
            {
                for (int i = itemsPositionToUpdate.Count - 1; i >= 0; i--)
                {
                    int key = itemsPositionToUpdate[i];
                    UpdatedSequenceItem item = _updatedItems[key];
                    _updatedItems.Remove(key);
                    _updatedItems.Add(key + value, item);
                }
            }
            else if (value < 0)
            {
                for (int i = 0; i < itemsPositionToUpdate.Count; i++)
                {
                    int key = itemsPositionToUpdate[i];
                    UpdatedSequenceItem item = _updatedItems[key];
                    _updatedItems.Remove(key);
                    _updatedItems.Add(key + value, item);
                }
            }
        }

        /// <summary>
        /// Gets the internal index for the specified index.
        /// </summary>
        /// <param name="index">Index for which the internal index is required.</param>
        /// <returns>Returns internal index for specified index.</returns>
        private int GetInternalIndex(int index)
        {
            int internalIndex = index;

            while ((internalIndex - GetRemovedItemsCount(internalIndex)) != index)
            {
                internalIndex++;
            }

            return internalIndex;
        }

        /// <summary>
        /// Gets the item present at specified index.
        /// </summary>
        /// <param name="index">index at which the sequence item is required.</param>
        /// <returns>Sequence item present at specified index.</returns>
        private ISequenceItem GetItem(int index)
        {
            index = GetInternalIndex(index);

            if (_updatedItems.ContainsKey(index))
            {
                return _updatedItems[index].SequenceItem;
            }
            else
            {
                return GetItemFromSource(index);
            }
        }

        /// <summary>
        /// Gets the sequence item present at specified internal index.
        /// </summary>
        /// <param name="internalIndex">Internal index.</param>
        /// <returns>Returns sequence item.</returns>
        private ISequenceItem GetItemFromSource(int internalIndex)
        {
            // Get source index from internal index.
            internalIndex = internalIndex - GetInsertedItemsCount(internalIndex);
            return _source[internalIndex];
        }

        #endregion Methods

        #region IDerivedSequence Members

        /// <summary>
        /// Gets source sequence specified while creating this instance.
        /// </summary>
        public ISequence Source
        {
            get { return _source; }
        }

        #endregion

        #region ISequence Members
        /// <summary>
        /// An identification provided to distinguish the sequence to others
        /// being worked with.
        /// </summary>
        public string ID
        {
            get
            {
                return _source.ID;
            }
        }

        /// <summary>
        /// An identification of the sequence that is meant to be understood
        /// by human users when displayed in an application or file format.
        /// </summary>
        public string DisplayID
        {
            get
            {
                return _source.DisplayID;
            }
        }

        /// <summary>
        /// Gets the alphabet to which string representations of the sequence should
        /// conform.
        /// </summary>
        public IAlphabet Alphabet
        {
            get { return _source.Alphabet; }
        }

        /// <summary>
        /// Gets the molecule type (DNA, protein, or various kinds of RNA) the sequence encodes.
        /// </summary>
        public MoleculeType MoleculeType
        {
            get { return _source.MoleculeType; }
        }

        /// <summary>
        /// Keeps track of the number of occurrances of each symbol within a sequence.
        /// </summary>
        public SequenceStatistics Statistics
        {
            get { return new SequenceStatistics(this); }
        }

        /// <summary>
        /// Gets the metadata of the source sequence.
        /// Note that changing the metadata will change the metadata of source sequence.
        /// </summary>
        public Dictionary<string, object> Metadata
        {
            get { return _source.Metadata; }
        }

        /// <summary>
        /// Gets or sets the documentation of source sequence.
        /// </summary>
        public object Documentation
        {
            get
            {
                return _source.Documentation;
            }

            set
            {
                _source.Documentation = value;
            }
        }

        /// <summary>
        /// Return a sequence representing this sequence with the orientation reversed.
        /// </summary>
        public ISequence Reverse
        {
            get
            {
                // reversed = true, complemented = false, range is a no-op.
                return new BasicDerivedSequence(this, true, false, -1, -1);
            }
        }

        /// <summary>
        /// Return a sequence representing the complement of this sequence.
        /// </summary>
        public ISequence Complement
        {
            get
            {
                // reversed = false, complemented = true, range is a no-op.
                return new BasicDerivedSequence(this, false, true, -1, -1);
            }
        }

        /// <summary>
        /// Return a sequence representing the reverse complement of this sequence.
        /// </summary>
        public ISequence ReverseComplement
        {
            get
            {
                // reversed = true, complemented = true, range is a no-op.
                return new BasicDerivedSequence(this, true, true, -1, -1);
            }
        }

        /// <summary>
        /// Return a sequence representing a range (substring) of this sequence.
        /// </summary>
        /// <param name="start">The index of the first symbol in the range.</param>
        /// <param name="length">The number of symbols in the range.</param>
        /// <returns>The sequence.</returns>
        public ISequence Range(int start, int length)
        {
            if (start < 0 || start >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameStart,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameLength,
                    Properties.Resource.ParameterMustNonNegative);
            }

            if ((Count - start) < length)
            {
                throw new ArgumentException(Properties.Resource.InvalidStartAndLength);
            }

            // reversed = false, complemented = false, range is as passed.
            return new BasicDerivedSequence(this, false, false, start, length);
        }

        /// <summary>
        /// Converts the specified character to a sequence item and insert at the specified index.
        /// </summary>
        /// <param name="index">Index at which the sequence to be inserted.</param>
        /// <param name="character">A character which indicates a sequence item.</param>
        public void Insert(int index, char character)
        {
            InsertRange(index, character.ToString());
        }

        /// <summary>
        /// Converts each character in the specified sequence string to sequence items
        /// and inserts them to the specified index. 
        /// </summary>
        /// <param name="index">Index at which the sequence to be inserted.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        public void InsertRange(int index, string sequence)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameIndex,
                    Properties.Resource.ParameterMustLessThanOrEqualToCount);
            }

            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequence);
            }

            List<ISequenceItem> seqItemList = Helper.GetSequenceItems(Alphabet, sequence);

            // Get internal index.
            index = GetInternalIndex(index);

            // To insert an ISequenceItem to a index, indices greater than or equl to the inserting item’s 
            // index have to be incremented. 
            UpdatePositions(index, seqItemList.Count);
            foreach (ISequenceItem seqItem in seqItemList)
            {
                _updatedItems.Add(index++, new UpdatedSequenceItem(seqItem, UpdateType.Inserted));
            }
        }

        /// <summary>
        /// Removes specified length of sequence items present in this sequence from the specified index.
        /// </summary>
        /// <param name="index">Index from which the sequence items to be removed.</param>
        /// <param name="length">Number of sequence items to be removed.</param>
        public void RemoveRange(int index, int length)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameIndex,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(Properties.Resource.ParameterNameLength);
            }

            if ((Count - index) < length)
            {
                throw new ArgumentException(Properties.Resource.InvalidIndexAndLength);
            }

            // Get internal index.
            index = GetInternalIndex(index);

            // If internal index is already added to the updatedItems list update it, else add the item
            // to updateditems list.
            for (int i = index; i < (index + length); i++)
            {
                if (_updatedItems.ContainsKey(i))
                {
                    // If item to remove is previously inserted item then remove it from the updated list.
                    if (_updatedItems[i].Type == UpdateType.Inserted)
                    {
                        _updatedItems.Remove(index);

                        // update the indices which are greater than or equal to the removed item, with -1.
                        UpdatePositions(i, -1);

                        // Update index and length with -1.
                        i = i - 1;
                        length = length - 1;
                    }
                    else
                    {
                        _updatedItems[i].SequenceItem = GetItemFromSource(i);
                        _updatedItems[i].Type = UpdateType.Removed;
                    }
                }
                else
                {
                    _updatedItems.Add(i, new UpdatedSequenceItem(GetItemFromSource(i), UpdateType.Removed));
                }
            }
        }

        /// <summary>
        /// Replaces the sequence item present in the specified index in this sequence 
        /// with a sequence item which is represented by specified character. 
        /// </summary>
        /// <param name="index">Index at which the sequence item has to be replaced.</param>
        /// <param name="character">Character which represent a sequence item.</param>
        public void Replace(int index, char character)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(Properties.Resource.ParameterNameIndex);
            }

            ISequenceItem seqItem = Alphabet.LookupBySymbol(character);

            if (seqItem == null)
            {
                throw new ArgumentException(
                         string.Format(
                         CultureInfo.CurrentCulture,
                         Properties.Resource.InvalidSymbol,
                         character));
            }

            Replace(index, seqItem);
        }

        /// <summary>
        /// Replaces the sequence item present in the specified index in this sequence with the specified sequence item. 
        /// </summary>
        /// <param name="index">Index at which the sequence item has to be replaced.</param>
        /// <param name="item">Sequence item to be placed at the specified position.</param>
        public void Replace(int index, ISequenceItem item)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameIndex,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            // if item is null throw exception.
            if (item == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameItem);
            }

            // Get the item from alphabet.
            ISequenceItem seqItem = Alphabet.LookupBySymbol(item.Symbol);

            if (seqItem == null)
            {
                throw new ArgumentException(
                    string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resource.InvalidSymbol,
                    item.Symbol));
            }

            // Get internal index.
            index = GetInternalIndex(index);

            // If the index is present in updated items, just update the item no need to change the updated type.
            if (_updatedItems.ContainsKey(index))
            {
                _updatedItems[index].SequenceItem = item;
            }
            else
            {
                _updatedItems.Add(index, new UpdatedSequenceItem(item, UpdateType.Replaced));
            }
        }

        /// <summary>
        /// Replaces the sequence items present in the specified index with the specified sequence item.
        /// </summary>
        /// <param name="index">Index from which the replace of sequence items has to be started.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        public void ReplaceRange(int index, string sequence)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameIndex,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequence);
            }

            if ((Count - index) < sequence.Length)
            {
                throw new ArgumentException(
                    Properties.Resource.InvalidIndexAndLength,
                    Properties.Resource.ParameterNameSequence);
            }

            List<ISequenceItem> seqItemList = Helper.GetSequenceItems(Alphabet, sequence);

            foreach (ISequenceItem item in seqItemList)
            {
                Replace(index++, item);
            }
        }

        /// <summary>
        /// Gets the index of first non gap character.
        /// </summary>
        /// <returns>If found returns an zero based index of the first non gap character, otherwise returns -1.</returns>
        public int IndexOfNonGap()
        {
            if (Count > 0)
            {
                return IndexOfNonGap(0);
            }

            return -1;
        }

        /// <summary>
        /// Returns the position of the first item from startPos that does not 
        /// have a Gap character.
        /// </summary>
        /// <param name="startPos">Index value above which to search for non-Gap character.</param>
        /// <returns>If found returns an zero based index of the first non gap character, otherwise returns -1.</returns>
        public int IndexOfNonGap(int startPos)
        {
            return BasicSequenceInfo.IndexOfNonGap(this, startPos);
        }

        /// <summary>
        /// Gets the index of last non gap character.
        /// </summary>
        /// <returns>If found returns an zero based index of the last non gap character, otherwise returns -1.</returns>
        public int LastIndexOfNonGap()
        {
            if (Count > 0)
            {
                return LastIndexOfNonGap(Count - 1);
            }

            return -1;
        }

        /// <summary>
        /// Gets the index of last non gap character within the specified end position.
        /// </summary>
        /// <param name="endPos">Index value below which to search for non-Gap character.</param>
        /// <returns>If found returns an zero based index of the last non gap character, otherwise returns -1.</returns>
        public int LastIndexOfNonGap(int endPos)
        {
            return BasicSequenceInfo.LastIndexOfNonGap(this, endPos);
        }

        /// <summary>
        /// Creates a new DerivedSequence that is a copy of the current DerivedSequence.
        /// </summary>
        /// <returns>A new DerivedSequence that is a copy of this DerivedSequence.</returns>
        ISequence ISequence.Clone()
        {
            return Clone();
        }

        #endregion

        #region IList<ISequenceItem> Members

        /// <summary>
        /// Returns the index of first occurance of the specified sequence item in this sequence.
        /// </summary>
        /// <param name="item">Sequence item of which the index is required.</param>
        /// <returns>If found returns zero based index of first occurance of the specified sequence item else returns -1.</returns>
        public int IndexOf(ISequenceItem item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (GetItem(i) == item)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Inserts the specified sequence item to a specified index in this sequence.
        /// </summary>
        /// <param name="index">Index at which the sequence item has to be inserted.</param>
        /// <param name="item">Sequence item to be inserted.</param>
        public void Insert(int index, ISequenceItem item)
        {
            if (index < 0 || index > Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameIndex,
                    Properties.Resource.ParameterMustLessThanOrEqualToCount);
            }

            // if item is null throw exception.
            if (item == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameItem);
            }

            // Get the item from alphabet.
            ISequenceItem seqItem = Alphabet.LookupBySymbol(item.Symbol);

            if (seqItem == null)
            {
                throw new ArgumentException(
                    string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resource.InvalidSymbol,
                    item.Symbol));
            }

            // Get internal index.
            index = GetInternalIndex(index);

            // To insert an ISequenceItem to a index, indices greater than or equl to the inserting item’s 
            // index have to be incremented. 
            UpdatePositions(index, 1);
            _updatedItems.Add(index, new UpdatedSequenceItem(item, UpdateType.Inserted));
        }

        /// <summary>
        /// Removes the sequence item present in the specified index.
        /// </summary>
        /// <param name="index">Index at which the sequence item has to be removed.</param>
        public void RemoveAt(int index)
        {
            RemoveRange(index, 1);
        }

        /// <summary>
        /// Allows the sequence to function like an array, gets or sets
        /// the sequence item at the specified index. Note that the
        /// index value starts its count at 0.
        /// </summary>
        /// <param name="index">Zero based index.</param>
        /// <returns>Returns sequence item present at specified index.</returns>
        public ISequenceItem this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(
                         Properties.Resource.ParameterNameIndex,
                         Properties.Resource.ParameterMustLessThanCount);
                }

                return GetItem(index);
            }

            set
            {
                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(
                         Properties.Resource.ParameterNameIndex,
                         Properties.Resource.ParameterMustLessThanCount);
                }

                Replace(index, value);
            }
        }

        #endregion

        #region ICollection<ISequenceItem> Members
        /// <summary>
        /// Adds the specified sequence item to the end of this sequence.
        /// </summary>
        /// <param name="item">Sequence item to be added.</param>
        public void Add(ISequenceItem item)
        {
            Insert(Count, item);
        }

        /// <summary>
        /// Clears the changes made to this sequence.
        /// Note that this method will not clear the source sequence.
        /// </summary>
        public void Clear()
        {
            _updatedItems.Clear();
        }

        /// <summary>
        /// Indicates if a sequence item is contained in the sequence anywhere.
        /// </summary>
        /// <param name="item">Sequence item to be verified.</param>
        /// <returns>If found returns true else returns false.</returns>
        public bool Contains(ISequenceItem item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Copies the sequence items in this instace into a preallocated array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">A preallocated array of ISequenceItem to which the 
        /// ISequenceItems in this instance has to be copied.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(ISequenceItem[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameArray);
            }

            int index = arrayIndex;
            foreach (ISequenceItem seqItem in this)
            {
                array[index] = seqItem;
                index++;
            }
        }

        /// <summary>
        /// The number of sequence items contained in the Sequence.
        /// </summary>
        public int Count
        {
            get { return _source.Count + GetAllInsertedItemsCount() - GetAllRemovedItemsCount(); }
        }

        /// <summary>
        /// A flag indicating whether or not edits can be made to this Sequence.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Removes first occurance of the specified sequence item in this sequence.
        /// </summary>
        /// <param name="item">Sequence item to be removed.</param>
        /// <returns>True if the item was found and removed, false if the item was not found.</returns>
        public bool Remove(ISequenceItem item)
        {
            int index = IndexOf(item);
            if (index < 0)
            {
                return false;
            }
            else
            {
                RemoveAt(index);
                return true;
            }
        }

        #endregion

        #region IEnumerable<ISequenceItem> Members

        /// <summary>
        /// Retrieves an enumerator for this sequence
        /// </summary>
        /// <returns>IEnumerator of ISequenceItem.</returns>
        public IEnumerator<ISequenceItem> GetEnumerator()
        {
            return new SequenceEnumerator(this);
        }

        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Retrieves an enumerator for this sequence
        /// </summary>
        /// <returns>IEnumerator of ISequenceItem.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SequenceEnumerator(this);
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Creates a new DerivedSequence that is a copy of the current DerivedSequence.
        /// </summary>
        /// <returns>A new object that is a copy of this DerivedSequence.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion

        #region ISerializable Members
        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected DerivedSequence(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _source = (ISequence)info.GetValue("DerivedSequence:source", typeof(ISequence));

            _updatedItems = new SortedDictionary<int, UpdatedSequenceItem>();

            // Get the changes from info.
            string convertedSeq = info.GetString("DerivedSequence:Changes");
            List<ISequenceItem> customSequenceItems = (List<ISequenceItem>)info.GetValue("DerivedSequence:CustomItems", typeof(List<ISequenceItem>));

            string[] seqWithIndex = convertedSeq.Split(',');
            int customItemIndex = 0;
            for (int i = 0; i < seqWithIndex.Length; i++)
            {
                string[] keyValuePair = seqWithIndex[i].Split(':');
                ISequenceItem item = null;
                if (keyValuePair[1] == "?")
                {
                    if (customItemIndex < customSequenceItems.Count)
                    {
                        item = customSequenceItems[customItemIndex++];
                    }
                    else
                    {
                        throw new FormatException("Invalid format.");
                    }
                }
                else
                {
                    item = Alphabet.LookupBySymbol(keyValuePair[1]);
                }

                _updatedItems.Add(
                    Convert.ToInt32(keyValuePair[0], CultureInfo.InvariantCulture),
                    new UpdatedSequenceItem(
                       item,
                        (UpdateType)Convert.ToInt32(keyValuePair[2], CultureInfo.InvariantCulture)));
            }
        }

        /// <summary>
        /// Method for serializing the DerivedSequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("DerivedSequence:source", _source);

            // Store changes in the info so that while constructing the object 
            // back we can get the changes back.
            StringBuilder strBuilder = new StringBuilder();

            string format = "{0}:{1}:{2}";
            bool isfirstSeq = true;
            List<ISequenceItem> customSequenceItems = new List<ISequenceItem>();
            foreach (KeyValuePair<int, UpdatedSequenceItem> keyValuePair in _updatedItems)
            {
                char symbol = keyValuePair.Value.SequenceItem.Symbol;
                if (keyValuePair.Value.SequenceItem != Alphabet.LookupBySymbol(symbol))
                {
                    customSequenceItems.Add(keyValuePair.Value.SequenceItem);
                    symbol = '?';
                }

                strBuilder.AppendFormat(
                    format,
                    keyValuePair.Key,
                   symbol,
                    (int)keyValuePair.Value.Type);


                if (isfirstSeq)
                {
                    format = ",{0}:{1}:{2}";
                    isfirstSeq = false;
                }
            }

            info.AddValue("DerivedSequence:Changes", strBuilder.ToString());
            info.AddValue("DerivedSequence:CustomItems", customSequenceItems);
        }

        #endregion

        /// <summary>
        /// Returns a string representation of the sequence data. This representation
        /// will come from the symbols in the alphabet defined for the sequence.
        /// 
        /// Thus a Sequence whose Alphabet is Alphabets.DNA may return a value like
        /// 'GATTCCA'
        /// </summary>
        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();

            for (int i = 0; i < Count; i++)
            {
                ISequenceItem item = this[i];
                buff.Append(item.Symbol);
            }

            return buff.ToString();
        }
    }
}
