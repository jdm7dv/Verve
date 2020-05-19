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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Bio.Util;

namespace Bio
{
    /// <summary>
    /// SparseSequence can hold discontinuous sequence. Use this class for storing the sequence items 
    /// with their known position from a long continuous sequence.  This class uses SortedDictionary to store 
    /// the sequence items with their position. Position is zero based indexes at which a sequence items 
    /// are present in the original continues sequence.
    /// For example: 
    /// To store sequence items at position 10, 101, 200, 1501 this class can be used as shown in the below code.
    /// 
    /// // Create a SparseSequence by specifying the Alphabet.
    /// SparseSequence mySparseSequence= new SparseSequence(Alphabets.DNA);
    /// 
    /// // By default count will be set to zero. To insert a sequence item at a postion greater than zero,
    /// // Count has to be set to a value greater than the maximum position value. 
    /// // If try to insert a sequence item at a position greater than the count an exception will occur.
    /// // You can limit the SparseSequence length by setting the count to desired value. In this example it 
    /// will be 1502 as the maximum index is 1501.
    /// mySparseSequence.Count = 1502;
    /// 
    /// // After setting the count to desired value you can use insert method or indexer to set the value.
    /// 
    /// // Useing insert method to set the value.
    /// mySparseSequence.Insert(10, DnaAlphabet.A);
    /// mySparseSequence.Insert(200, DnaAlphabet.C);
    ///
    /// // Using Indexer to set the value.
    /// mySpareseSequence[101] = DnaAlphabet.A;
    /// mySpareseSequence[1501] = DnaAlphabet.G;
    /// 
    /// // To access the value in a SparseSequence use Indexer or an Enumerator like below.
    ///
    /// // Accessing SparsesSequence using Indexer.
    /// ISequenceItem seqItem1 = mySparseSequence [10] ;  // this will return sequence item A.
    /// ISequenceItem seqItem2 = mySparseSequence [1501] ;  // this will return sequence item G.
    /// ISequenceItem seqItem3 = mySparseSequence [102] ;  // this will return null as there is no sequence item at this position.
    /// 
    /// // Accessing SparsesSequence using Enumerator.
    /// foreach(ISequenceItem seqItem in mySparseSequence) {…}
    /// 
    ///
    /// This class is marked with Serializable attribute thus instances of this 
    /// class can be serialized and stored to files and the stored files 
    /// can be de-serialized to restore the instances.
    /// </summary>
    [Serializable]
    public class SparseSequence : ISequence
    {
        #region Field

        /// <summary>
        /// Holds sequence items with their position.
        /// </summary>
        private SortedDictionary<int, ISequenceItem> _sparseSeqItems = new SortedDictionary<int, ISequenceItem>();

        /// <summary>
        /// Holds the metadata of the sequence.
        /// </summary>
        private BasicSequenceInfo _seqInfo;

        /// <summary>
        /// Holds statistical data of the sequence.
        /// </summary>
        private SequenceStatistics _statistics;

        /// <summary>
        /// Holds size of this sequence.
        /// </summary>
        private int _count = 0;

        #endregion Field

        #region Constructors

        /// <summary>
        /// Private constructor for clone method.
        /// </summary>
        private SparseSequence() { }

        /// <summary>
        /// Creates a SparseSequence with no sequence data and sets the IsReadOnly flag to false.
        /// 
        /// Count property of SparseSequence instance created by using this constructor will be set to zero,
        /// thus before inserting a sequence item which is at a position greater than the count property of 
        /// the instance, set the count to desired value. However you can use Add() method to add a 
        /// sequence item even though the count is zero, in this case specified sequence item position will 
        /// be default to the value of the count property and then count prperty will be incremented by one.
        /// For example: 
        /// If the count property of a sparse sequence is zero, using Add() method will add 
        /// the specified sequence item to the position zero and count will become one. 
        /// 
        /// For working with sequences that never have sequence data, but are
        /// only used for metadata storage (like keeping an ID or various features
        /// but no direct sequence data) consider using the VirtualSequence
        /// class instead.
        /// </summary>
        /// <param name="alphabet"> 
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA or Alphabets.Protein)
        /// </param>
        public SparseSequence(IAlphabet alphabet)
            : this(alphabet, 0) { }

        /// <summary>
        /// Creates a SparseSequence with no sequence data and sets the IsReadOnly flag to false.
        /// 
        /// Count property of SparseSequence instance created by using this constructor will be 
        /// set a value specified by size parameter.
        /// 
        /// For working with sequences that never have sequence data, but are
        /// only used for metadata storage (like keeping an ID or various features
        /// but no direct sequence data) consider using the VirtualSequence
        /// class instead.
        /// </summary>
        /// <param name="alphabet"> 
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA or Alphabets.Protein)
        /// </param>
        /// <param name="size">A value indicating the size of this sequence.</param>
        public SparseSequence(IAlphabet alphabet, int size)
        {
            if (alphabet == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameAlphabet);
            }

            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(Properties.Resource.ParameterNameSize, Properties.Resource.ParameterMustNonNegative);
            }

            Count = size;
            _seqInfo = new BasicSequenceInfo();
            Alphabet = alphabet;
            IsReadOnly = false;

            _statistics = new SequenceStatistics(alphabet);
        }

        /// <summary>
        /// Creates a sparse sequence based on the specified parameters and sets the IsReadOnly flag to true.
        /// To edit the sequence set IsReadOnly flag to false.
        /// 
        /// The sequenceItem parameter must contain an alphabet as specified in the alphabet parameter,
        /// else an exception will occur.
        /// 
        /// The index parameter value must be a non negative value.
        /// Count property of an instance created by this constructor will be set to value of index + 1.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA or Alphabets.Protein)</param>
        /// <param name="index">Position of the specified sequence item.</param>
        /// <param name="item">A sequence item which is known by the alphabet.</param>
        public SparseSequence(IAlphabet alphabet, int index, ISequenceItem item)
            : this(alphabet)
        {
            if (index < 0 || index == int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameIndex,
                    Properties.Resource.SparseSequenceConstructorIndexOutofRange);
            }

            if (item == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameItem);
            }

            ISequenceItem seqItem = alphabet.LookupBySymbol(item.Symbol);

            if (seqItem == null)
            {
                throw new ArgumentException(
                    string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resource.InvalidSymbol,
                    item.Symbol));
            }

            _statistics = new SequenceStatistics(alphabet);

            _sparseSeqItems.Add(index, item);
            _statistics.Add(item);

            Count = index + 1;
            IsReadOnly = true;
        }

        /// <summary>
        /// Creates a sparse sequence based on the specified parameters and sets the IsReadOnly flag to true.
        /// To edit the sequence set IsReadOnly flag to false.
        /// 
        /// The sequenceItems parameter must contain sequence items known by the specified alphabet,
        /// else an exception will occur.
        /// 
        /// The index parameter value must be a non negative. 
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA or Alphabets.Protein)</param>
        /// <param name="index">A non negative value which indicates the start position of the specified sequence items.</param>
        /// <param name="sequenceItems">
        /// A sequence which contain sequence items known by the alphabet.</param>
        public SparseSequence(IAlphabet alphabet, int index, IList<ISequenceItem> sequenceItems)
            : this(alphabet)
        {
            if (index < 0 || index == int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameIndex,
                    Properties.Resource.SparseSequenceConstructorIndexOutofRange);
            }

            if (sequenceItems == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequenceItems);
            }

            _statistics = new SequenceStatistics(alphabet);

            int position = index;
            foreach (ISequenceItem sequenceItem in sequenceItems)
            {
                // If the sequence item is null then increase the index. 
                if (sequenceItem != null)
                {
                    ISequenceItem seqItem = alphabet.LookupBySymbol(sequenceItem.Symbol);

                    if (seqItem == null)
                    {
                        throw new ArgumentException(string.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resource.InvalidSymbol,
                            sequenceItem.Symbol));
                    }

                    _sparseSeqItems.Add(position, sequenceItem);
                    _statistics.Add(sequenceItem);
                }

                position++;
            }

            if (sequenceItems.Count > 0)
            {
                Count = index + sequenceItems.Count;
            }

            IsReadOnly = true;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Returns known sequence items with their position as ReadOnlyCollection of IndexedSequenceItem.
        /// </summary>
        public IList<IndexedItem<ISequenceItem>> GetKnownSequenceItems()
        {
            List<IndexedItem<ISequenceItem>> indexedSeqItems = new List<IndexedItem<ISequenceItem>>();

            foreach (int key in _sparseSeqItems.Keys)
            {
                indexedSeqItems.Add(new IndexedItem<ISequenceItem>(key, _sparseSeqItems[key]));
            }

            return indexedSeqItems.AsReadOnly();
        }

        #endregion Properties

        #region Methods
        /// <summary>
        /// Returns the position of the first item that does not have a null ISequenceItem
        /// </summary>
        /// <returns>Lowest position which has a sequence item that is not null. 
        /// -1 if no non-null items in sequence.</returns>
        public int IndexOfNotNull()
        {
            // Note: _sparseSeqItems contains only non-null items.
            if (_sparseSeqItems.Keys.Count > 0)
            {
                return _sparseSeqItems.Keys.First();
            }

            return -1;
        }

        /// <summary>
        /// Returns the position of the first item beyond startPos that does not 
        /// have a null ISequenceItem.
        /// </summary>
        /// <param name="startPos">index value above which to search for non-null value</param>
        /// /// <returns>Lowest position greater than startPos which has a sequence item 
        /// that is not null. -1 if no non-null items in sequence at position 
        /// greater than startPos.</returns>
        public int IndexOfNotNull(int startPos)
        {
            try
            {
                // Note: LINQ is quicker *on an average* than foreach scan
                // Note: _sparseSeqItems contains only non-null items.
                return
                    _sparseSeqItems.Keys.
                    Where(i => i > startPos).
                    First();
            }
            catch (InvalidOperationException)
            {
                return -1;
            }
        }

        /// <summary>
        /// Returns the position of the last item that does not have a null ISequenceItem
        /// </summary>
        /// <returns>Highest position which has a sequence item that is not null. 
        /// -1 if no non-null items in sequence.</returns>
        public int LastIndexOfNotNull()
        {
            // Note: LINQ is quicker *on an average* than foreach scan
            // Note: keys.Reverse().First() is quicker than keys.Last()
            // Note: _sparseSeqItems contains only non-null items.
            if (_sparseSeqItems.Count > 0)
            {
                return
                  _sparseSeqItems.Keys.
                  Reverse().
                  First();
            }

            return -1;
        }

        /// <summary>
        /// Returns the position of the last item before endPos that does 
        /// not have a null ISequenceItem
        /// </summary>
        /// <param name="endPos">index value below which to search for non-null value</param>
        /// <returns>Highest position less than endPos which has a sequence item 
        /// that is not null. -1 if no non-null items in sequence at position less 
        /// than endPos.</returns>
        public int LastIndexOfNotNull(int endPos)
        {
            try
            {
                // Note: LINQ is quicker *on an average* than foreach scan
                // Note: keys.Reverse().First() is quicker than keys.Last()
                // Note: _sparseSeqItems contains only non-null items.
                return
                  _sparseSeqItems.Keys.
                  Where(i => i < endPos).
                  Reverse().
                  First();
            }
            catch (InvalidOperationException)
            {
                return -1;
            }
        }

        /// <summary>
        /// Creates a new SparseSequence that is a copy of the current SparseSequence.
        /// </summary>
        /// <returns>A new SparseSequence that is a copy of this SparseSequence.</returns>
        public SparseSequence Clone()
        {
            SparseSequence sparseSeqClone = new SparseSequence();
            sparseSeqClone._seqInfo = _seqInfo.Clone();
            sparseSeqClone.Count = Count;

            foreach (int key in _sparseSeqItems.Keys)
            {
                ISequenceItem item = _sparseSeqItems[key];
                if (item != Alphabet.LookupBySymbol(item.Symbol))
                {
                    item = item.Clone();
                }

                sparseSeqClone._sparseSeqItems.Add(key, item);
            }

            sparseSeqClone.IsReadOnly = IsReadOnly;

            sparseSeqClone._statistics = _statistics.Clone();

            return sparseSeqClone;
        }

        /// <summary>
        /// Updates position of items which is greater than the specified index
        /// with the specified value.
        /// </summary>
        /// <param name="position">Position from which update has to be done.</param>
        /// <param name="value">Value with which positions have to be updated.</param>
        private void UpdatePositions(int position, int value)
        {
            List<int> itemsPositionToUpdate = _sparseSeqItems.Keys.Where(I => I >= position).ToList();
            itemsPositionToUpdate.Sort();

            if (value > 0)
            {
                for (int i = itemsPositionToUpdate.Count - 1; i >= 0; i--)
                {
                    int key = itemsPositionToUpdate[i];
                    ISequenceItem item = _sparseSeqItems[key];
                    _sparseSeqItems.Remove(key);
                    _sparseSeqItems.Add(key + value, item);
                }
            }
            else if (value < 0)
            {
                for (int i = 0; i < itemsPositionToUpdate.Count; i++)
                {
                    int key = itemsPositionToUpdate[i];
                    ISequenceItem item = _sparseSeqItems[key];
                    _sparseSeqItems.Remove(key);
                    _sparseSeqItems.Add(key + value, item);
                }
            }
        }
        #endregion Methods

        #region ISequence Members

        /// <summary>
        /// An identification provided to distinguish the sequence to others
        /// being worked with.
        /// </summary>
        public string ID
        {
            get { return _seqInfo.ID; }
            set { _seqInfo.ID = value; }
        }

        /// <summary>
        /// An identification of the sequence that is meant to be understood
        /// by human users when displayed in an application or file format.
        /// </summary>
        public string DisplayID
        {
            get { return _seqInfo.DisplayID; }
            set { _seqInfo.DisplayID = value; }
        }

        /// <summary>
        /// Converts each character in the specified sequence string to sequence items
        /// and inserts them to the specified position. 
        /// </summary>
        /// <param name="position">Position at which the sequence to be inserted.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        public void InsertRange(int position, string sequence)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position > Count)
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanOrEqualToCount);

            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequence);
            }

            List<ISequenceItem> seqItemList = Helper.GetSequenceItems(Alphabet, sequence);

            // To insert an ISequenceItem to a position, positions greater than the inserting item’s 
            // position have to be incremented by length. 
            UpdatePositions(position, sequence.Length);

            foreach (ISequenceItem seqItem in seqItemList)
            {
                _sparseSeqItems.Add(position++, seqItem);
                _statistics.Add(seqItem);
            }

            Count = Count + sequence.Length;
        }

        /// <summary>
        /// Converts the specified character to a sequence item and insert at the specified position.
        /// </summary>
        /// <param name="position">Position at which the sequence to be inserted.</param>
        /// <param name="character">A character which indicates a sequence item.</param>
        public void Insert(int position, char character)
        {
            InsertRange(position, character.ToString());
        }

        /// <summary>
        /// Removes specified length of sequence items present in this sequence from the specified postion.
        /// </summary>
        /// <param name="position">Position from which the sequence items to be removed.</param>
        /// <param name="length">Number of sequence items to be removed.</param>
        public void RemoveRange(int position, int length)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(Properties.Resource.ParameterNameLength);
            }

            if ((Count - position) < length)
            {
                throw new ArgumentException(Properties.Resource.InvalidPositionAndLength);
            }

            for (int i = position; i < (position + length); i++)
            {
                if (_sparseSeqItems.ContainsKey(i))
                {
                    _statistics.Remove(_sparseSeqItems[i]);
                    _sparseSeqItems.Remove(i);
                }
            }

            // After removing sequence items, remaining items having position greater than the removed item’s 
            // position have to be decremented by specified length. 
            UpdatePositions(position, -length);

            Count = Count - length;
        }

        /// <summary>
        /// Replaces the sequence item present in the specified postion in this sequence with the specified sequence item. 
        /// </summary>
        /// <param name="position">Position at which the sequence item has to be replaced.</param>
        /// <param name="item">Sequence item to be placed at the specified position.</param>
        public void Replace(int position, ISequenceItem item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            if (item == null)
            {
                if (_sparseSeqItems.ContainsKey(position))
                {
                    _statistics.Remove(_sparseSeqItems[position]);
                    _sparseSeqItems.Remove(position);
                }
            }
            else
            {
                ISequenceItem seqItem = Alphabet.LookupBySymbol(item.Symbol);

                if (seqItem == null)
                {
                    throw new ArgumentException(
                        string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.InvalidSymbol,
                        item.Symbol));
                }

                if (_sparseSeqItems.ContainsKey(position))
                {
                    _statistics.Remove(_sparseSeqItems[position]);
                    _sparseSeqItems[position] = item;
                }
                else
                {
                    _sparseSeqItems.Add(position, item);
                }

                _statistics.Add(item);
            }
        }

        /// <summary>
        /// Replaces the sequence items present in the specified position in this sequence with the specified sequence.
        /// </summary>
        /// <param name="position">Position from which the replace of sequence items has to be started.</param>
        /// <param name="sequence">A string containing the description of a sequence.</param>
        public void ReplaceRange(int position, string sequence)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position >= Count)
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanCount);

            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequence);
            }

            if ((Count - position) < sequence.Length)
            {
                throw new ArgumentException(
                    Properties.Resource.InvalidPositionAndLength,
                    Properties.Resource.ParameterNameSequence);
            }

            List<ISequenceItem> seqItemList = Helper.GetSequenceItems(Alphabet, sequence);

            foreach (ISequenceItem item in seqItemList)
            {
                Replace(position++, item);
            }
        }

        /// <summary>
        /// Replaces the sequence item present in the specified postion in this sequence 
        /// with a sequence item which is represented by specified character. 
        /// </summary>
        /// <param name="position">Position at which the sequence item has to be replaced.</param>
        /// <param name="character">Character which represent a sequence item.</param>
        public void Replace(int position, char character)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position >= Count)
            {
                throw new ArgumentOutOfRangeException(Properties.Resource.ParameterNamePosition);
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

            Replace(position, seqItem);
        }

        /// <summary>
        /// The alphabet to which string representations of the sequence should
        /// conform.
        /// </summary>
        public IAlphabet Alphabet
        {
            get { return _seqInfo.Alphabet; }
            internal set { _seqInfo.Alphabet = value; }
        }

        /// <summary>
        /// A flag indicating whether or not edits can be made to this Sequence.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// The molecule type (DNA, protein, or various kinds of RNA) the sequence encodes.
        /// </summary>
        public MoleculeType MoleculeType { get; set; }

        /// <summary>
        /// Keeps track of the number of occurrances of each symbol within a sequence.
        /// </summary>
        public SequenceStatistics Statistics
        {
            get
            {
                return _statistics;
            }
        }

        /// <summary>
        /// Many sequence representations when saved to file also contain
        /// information about that sequence. Unfortunately there is no standard
        /// around what that data may be from format to format. This property
        /// allows a place to put structured metadata that can be accessed by
        /// a particular key.
        /// 
        /// For example, if species information is stored in a particular Species
        /// class, you could add it to the dictionary by:
        /// 
        /// mySequence.Metadata["SpeciesInfo"] = mySpeciesInfo;
        /// 
        /// To fetch the data you would use:
        /// 
        /// Species mySpeciesInfo = mySequence.Metadata["SpeciesInfo"];
        /// 
        /// Particular formats may create their own data model class for information
        /// unique to their format as well. Such as:
        /// 
        /// GenBankMetadata genBankData = new GenBankMetadata();
        /// // ... add population code
        /// mySequence.MetaData["GenBank"] = genBankData;
        /// </summary>
        public Dictionary<string, object> Metadata
        {
            get { return _seqInfo.Metadata; }
        }

        /// <summary>
        /// The Documentation object is intended for tracking the history, provenance,
        /// and experimental context of a sequence. The user can adopt any desired
        /// convention for use of this object.
        /// </summary>
        public Object Documentation { set; get; }

        /// <summary>
        /// SparseSequence does not contains continuous sequence data thus 
        /// SparseSequence does not support this method.
        /// </summary>
        public override string ToString()
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedSparseSequence);
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
        /// <returns>The sequence which is sub sequence of this sequence.</returns>
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
            if (startPos < 0 || startPos >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameStartPos,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            try
            {
                if (Alphabet.HasGaps)
                {
                    return _sparseSeqItems.First(K => K.Key >= startPos && !K.Value.IsGap).Key;
                }
                else
                {
                    return _sparseSeqItems.First(K => K.Key >= startPos).Key;
                }
            }
            catch (InvalidOperationException)
            {
                return -1;
            }
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
            if (endPos < 0 || endPos >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameEndPos,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            try
            {
                if (Alphabet.HasGaps)
                {
                    return _sparseSeqItems.Reverse().First(K => K.Key <= endPos && !K.Value.IsGap).Key;
                }
                else
                {
                    return _sparseSeqItems.Reverse().First(K => K.Key <= endPos).Key;
                }
            }
            catch (InvalidOperationException)
            {
                return -1;
            }
        }

        /// <summary>
        /// Creates a new SparseSequence that is a copy of the current SparseSequence.
        /// </summary>
        /// <returns>A new ISequence that is a copy of this SparseSequence.</returns>
        ISequence ISequence.Clone()
        {
            return Clone();
        }
        #endregion ISequence Members

        #region IList<ISequenceItem> Members

        /// <summary>
        /// Returns the position of first occurance of the specified sequence item in this sequence.
        /// </summary>
        /// <param name="item">Sequence item of which the postion is required.</param>
        /// <returns>If found returns the positon of first occurance of the specified sequence item else returns -1.</returns>
        public int IndexOf(ISequenceItem item)
        {
            int index = -1;

            if (item != null)
            {
                ISequenceItem seqItem = Alphabet.LookupBySymbol(item.Symbol);
                if (seqItem != null)
                {
                    // Check for the seqitem if it is there then get the key.
                    if (_sparseSeqItems.ContainsValue(item))
                    {
                        KeyValuePair<int, ISequenceItem> keyValue = _sparseSeqItems.FirstOrDefault(K => K.Value == item);

                        if (keyValue.Value != null)
                        {
                            index = keyValue.Key;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < Count; i++)
                {
                    if (!_sparseSeqItems.ContainsKey(i))
                    {
                        index = i;
                        break;
                    }
                }
            }

            return index;
        }

        /// <summary>
        /// Inserts the specified sequence item to a specified positon in this sequence.
        /// </summary>
        /// <param name="index">Position at which the sequence item has to be inserted.</param>
        /// <param name="item">Sequence item to be inserted.</param>
        public void Insert(int index, ISequenceItem item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (index < 0 || index > Count)
                throw new ArgumentOutOfRangeException(
                     Properties.Resource.ParameterNameIndex,
                     Properties.Resource.ParameterMustLessThanOrEqualToCount);

            ISequenceItem seqItem = item;

            // If item is not null then validate the item.
            // If it is null then push the other items by one postion.
            if (item != null)
            {
                seqItem = Alphabet.LookupBySymbol(item.Symbol);

                if (seqItem == null)
                {
                    throw new ArgumentException(
                             string.Format(
                             CultureInfo.CurrentCulture,
                             Properties.Resource.InvalidSymbol,
                             item.Symbol));
                }
            }

            // To insert an ISequenceItem to a position, positions greater than the inserting item’s 
            // position have to be incremented by one position. 
            UpdatePositions(index, 1);

            // No need to store the null value, as it is assumed that if any position 
            // with in the count is not present in the _sparseSeqItems, it will be a null value.
            if (item != null)
            {
                _sparseSeqItems.Add(index, item);
                _statistics.Add(item);
            }

            Count++;
        }

        /// <summary>
        /// Removes the sequence item present in the specified position.
        /// </summary>
        /// <param name="index">Position at which the sequence item has to be removed.</param>
        public void RemoveAt(int index)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(
                     Properties.Resource.ParameterNameIndex,
                     Properties.Resource.ParameterMustLessThanCount);

            RemoveRange(index, 1);
        }

        /// <summary>
        /// Allows the sequence to function like an array, gets or sets
        /// the sequence item at the specified index. Note that the
        /// index value starts its count at 0.
        /// </summary>
        public ISequenceItem this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(
                         Properties.Resource.ParameterNameIndex,
                         Properties.Resource.ParameterMustLessThanCount);

                ISequenceItem item = null;
                if (_sparseSeqItems.ContainsKey(index))
                {
                    item = _sparseSeqItems[index];
                }

                return item;
            }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(
                         Properties.Resource.ParameterNameIndex,
                         Properties.Resource.ParameterMustLessThanCount);

                Replace(index, value);
            }
        }

        #endregion IList<ISequenceItem> Members

        #region ICollection<ISequenceItem> Members

        /// <summary>
        /// Adds the specified sequence item to the end of this sequence.
        /// </summary>
        /// <param name="item">Sequence item to be added.</param>
        public void Add(ISequenceItem item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (item != null)
            {
                ISequenceItem seqItem = Alphabet.LookupBySymbol(item.Symbol);

                if (seqItem == null)
                {
                    throw new ArgumentException(
                        string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.InvalidSymbol,
                        item.Symbol));
                }

                _sparseSeqItems.Add(Count, item);
                _statistics.Add(item);
            }

            Count++;
        }

        /// <summary>
        /// Clears the underlying sequence data in this sequence.
        /// </summary>
        public void Clear()
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            _sparseSeqItems.Clear();
            _statistics.Clear();
            Count = 0;
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
            get
            {
                return _count;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(Properties.Resource.ParameterNameValue, Properties.Resource.ParameterMustNonNegative);
                }

                _count = value;
            }
        }

        /// <summary>
        /// Removes first occurance of the specified sequence item in this sequence.
        /// </summary>
        /// <param name="item">Sequence item to be removed.</param>
        /// <returns>True if the item was found and removed, false if the item was not found.</returns>
        public bool Remove(ISequenceItem item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveRange(index, 1);
                return true;
            }

            return false;
        }

        #endregion ICollection<ISequenceItem> Members

        #region IEnumerable<ISequenceItem> Members
        /// <summary>
        /// Retrieves an enumerator for this sequence
        /// </summary>
        /// <returns>IEnumerator of ISequenceItem.</returns>
        public IEnumerator<ISequenceItem> GetEnumerator()
        {
            return new SequenceEnumerator(this);
        }

        #endregion IEnumerable<ISequenceItem> Members

        #region IEnumerable Members
        /// <summary>
        /// Retrieves an enumerator for this sequence
        /// </summary>
        /// <returns>IEnumerator of ISequenceItem.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new SequenceEnumerator(this);
        }

        #endregion IEnumerable Members

        #region ICloneable Members
        /// <summary>
        /// Creates a new SparseSequence that is a copy of the current SparseSequence.
        /// </summary>
        /// <returns>A new object that is a copy of this SparseSequence.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion ICloneable Members

        #region ISerializable Members
        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected SparseSequence(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _seqInfo = (BasicSequenceInfo)info.GetValue("SI", typeof(BasicSequenceInfo));
            Count = info.GetInt32("C");
            List<ISequenceItem> customSequenceItems = new List<ISequenceItem>();
            int customItemsCount = (int)info.GetValue("CI", typeof(int));
            if (customItemsCount > 0)
            {
                for (int i = 0; i < customItemsCount; i++)
                {
                    customSequenceItems.Insert(i, (ISequenceItem)info.GetValue("C" + i.ToString(), typeof(ISequenceItem)));
                }
            }

            int customItemIndex = 0;

            // Get the sequence from converted sequence.
            string convertedSeq = string.Empty;
            if (info.GetBoolean("CS"))
            {
                convertedSeq = info.GetString("CSD");
            }

            if (!string.IsNullOrEmpty(convertedSeq))
            {
                int index = 1;
                for (int i = 0; i < convertedSeq.Length; i++)
                {
                    if (char.IsNumber(convertedSeq[i]))
                    {
                        int j = i;
                        while (j < convertedSeq.Length && char.IsNumber(convertedSeq[j]))
                        {
                            j++;
                        }

                        string number = convertedSeq.Substring(i, j - i);
                        index = index - 1 + int.Parse(number);
                        i = j - 1;
                        continue;
                    }

                    ISequenceItem item = null;
                    if (convertedSeq[i] == '?')
                    {
                        // custom object.
                        if (customItemIndex < customSequenceItems.Count)
                        {
                            item = customSequenceItems[customItemIndex++];
                        }
                        else
                        {
                            throw new FormatException("Invalid format");
                        }
                    }
                    else
                    {
                        item = Alphabet.LookupBySymbol(convertedSeq[i]);
                    }

                    _sparseSeqItems.Add(index++, item);
                }
            }

            MoleculeType = (MoleculeType)info.GetValue("MT", typeof(int));
            Documentation = info.GetValue("D", typeof(object));
            _statistics = new SequenceStatistics(this);
        }

        /// <summary>
        /// Method for serializing the SparseSequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("SI", _seqInfo);
            info.AddValue("C", Count);

            // Store sequence items symbol in the info so that while constructing the object 
            // back we can get the sequence items from respective Alphabet.
            StringBuilder strBuilder = new StringBuilder();
            bool isFirstSeq = true;
            int lastIndex = 0;
            List<ISequenceItem> customSequenceItems = new List<ISequenceItem>();
            foreach (KeyValuePair<int, ISequenceItem> keyValuePair in _sparseSeqItems)
            {
                if (isFirstSeq)
                {
                    strBuilder.Append(keyValuePair.Key);
                    isFirstSeq = false;
                }
                else if (keyValuePair.Key > (lastIndex + 1))
                {
                    strBuilder.Append(keyValuePair.Key - lastIndex);
                }

                char symbol = keyValuePair.Value.Symbol;
                if (keyValuePair.Value != Alphabet.LookupBySymbol(symbol))
                {
                    customSequenceItems.Add(keyValuePair.Value);
                    symbol = '?';
                }

                strBuilder.Append(symbol);
                lastIndex = keyValuePair.Key;
            }

            if (strBuilder.Length > 0)
            {
                info.AddValue("CS", true);
                info.AddValue("CSD", strBuilder.ToString());
            }
            else
            {
                info.AddValue("CS", false);
            }

            info.AddValue("CI", customSequenceItems.Count);
            if (customSequenceItems.Count > 0)
            {
                for (int i = 0; i < customSequenceItems.Count; i++)
                {
                    info.AddValue("C" + i.ToString(), customSequenceItems[i]);
                }
            }

            info.AddValue("MT", (int)MoleculeType);

            if (Documentation != null && ((Documentation.GetType().Attributes &
               System.Reflection.TypeAttributes.Serializable) == System.Reflection.TypeAttributes.Serializable))
            {
                info.AddValue("D", Documentation);
            }
            else
            {
                info.AddValue("D", null);
            }
        }

        #endregion
    }
}
