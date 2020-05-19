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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

using Bio.Util;

namespace Bio
{
    /// <summary>
    /// SegmentedSequence contains a list that can hold sequences; a large sequence can be 
    /// fragmented into list of sequences and stored in an instance of this class.
    /// For example:
    /// Consider a DNA sequence "ATGCATGCTTGTGCATGCCCGA" which is fragmented in to following 
    /// three sequences "ATGCATGC", "TTGTGCATGC" and "CCGA". Following code shows how to 
    /// create a segmented sequence from these sequences.
    /// 
    /// List&lt;ISequence&gt; sequenceList= new List&lt;ISequence&gt;();
    /// sequenceList.Add(new Sequence(Alphabets.DNA, "ATGCATGC");
    /// sequenceList.Add(new Sequence(Alphabets.DNA, "TTGTGCATGC");
    /// sequenceList.Add(new Sequence(Alphabets.DNA, "CCGA");
    /// 
    /// SegmentedSequence mySegmentedSequence = new SegmentedSequence(sequenceList);
    /// The sequence data can be accessed in following ways.
    /// 1. Using ToString() method
    /// string  nucleotides = mySegmentedSequence.ToString() ;
    /// Provides the string representation of the entire sequence, thus nucleotides will contain "ATGCATGCTTGTGCATGCCCGA".
    ///  
    /// 2. Using enumerator
    /// foreach (Nucleotide nucleotide in mySegmentedSequence) { ... }
    /// This is by treating the SegmentedSequence as a list of SequenceItems.
    /// 
    /// 3. Using indexer
    /// Nucleotide nucleotide = mySegmentedSequence[9];
    /// Indexer is a zero based array representation of ISequenceItems, thus nucleotide will contain “T”.
    /// 
    /// 4. Using Sequences property
    ///    Sequences property can be used to access the list of ISequences in the SegmentedSequence.
    ///    
    ///    foreach(ISequence seq in mySegmentedSequence.Sequences){...}
    ///    In this case seqList will contain three Sequences. 
    ///    seqList[0] will be ISequence instance containg "ATGCATGC".
    ///    seqList[1] will be ISequence instance containg "TTGTGCATGC"
    ///    seqList[2] will be ISequence instance containg "CCGA".
    ///
    /// This class is marked with Serializable attribute thus instances of this 
    /// class can be serialized and stored to files and the stored files 
    /// can be de-serialized to restore the instances.
    /// </summary>
    [Serializable]
    public class SegmentedSequence : ISequence
    {
        #region Fields

        /// <summary>
        /// Holds underlying sequences.
        /// Observable collection is used to handle the scenarios like, a new sequence 
        /// is inserted or replaced with the existing one and Alphabet of the new sequence 
        /// does not match with Alphabet of this segmented sequence.
        /// 
        /// Observable collection notifies any changes to the collection through the 
        /// CollectionChanged event. A new sequence which is a result of an insert or 
        /// a replace action can be validated by listening to this event.
        /// </summary>
        private ObservableCollection<ISequence> _sequences;

        /// <summary>
        /// Holds metadata of the sequence.
        /// </summary>
        private BasicSequenceInfo _seqInfo;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sequences providers the list of sequences in this SegmentedSequence.
        /// 
        /// Sequences property can be used to add, insert, replace, remove and access the sequences in this
        /// SegmentedSequence even though the IsReadOnly property is true. However if the IsReadOnly property 
        /// of this segmented sequence is false then the new sequence that has to be added, inserted or 
        /// replaced with existing sequence must be an editable sequence.
        /// 
        /// Sequence that has to be added, inserted or replaced must have matching Alphabet of this
        /// SegmentedSequence else an ArgumentException will occur.
        /// </summary>
        public IList<ISequence> Sequences
        {
            get
            {
                return _sequences;
            }
        }

        #endregion Properties

        #region Constructors
        /// <summary>
        /// Private constructor for clone method.
        /// </summary>
        private SegmentedSequence() { }

        /// <summary>
        /// Creates a SegmentedSequence based on the sequence passed in via a ISequence
        /// parameter.
        /// 
        /// Alphabet property of the created segmented sequence will be same as that of 
        /// the specified ISequence.
        /// 
        /// A typical use of this constructor for a DNA sequence would look like:
        /// 
        /// ISequence sequence = new Sequence(Alphabets.DNA, "GATTCAAGGGCT");
        /// SegmentedSequence mySegmentedSequence = new SegmentedSequence(sequence);
        /// 
        /// The correlary for RNA:
        /// 
        /// ISequence sequence = new Sequence(Alphabets.RNA, "GAUUCAAGGGCU");
        /// SegmentedSequence mySegmentedSequence = new SegmentedSequence(sequence);
        /// </summary>
        /// <param name="sequence">
        /// An instance of ISequence. 
        /// </param>
        public SegmentedSequence(ISequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequence);
            }

            _seqInfo = new BasicSequenceInfo();
            Alphabet = sequence.Alphabet;
            _sequences = new ObservableCollection<ISequence>();
            _sequences.Add(sequence);
            _sequences.CollectionChanged += new NotifyCollectionChangedEventHandler(SequenceCollectionChanged);
        }

        /// <summary>
        /// Creates a SegmentedSequence based on list of sequence passed in via a sequences
        /// parameter. The sequences in the list of ISequence must be of the same Alphabet 
        /// else an ArgumentException will occur. If sequences parameter contain any null 
        /// references then a ArgumentNullException will occur. 
        /// 
        /// A typical use of this constructor for a DNA sequence would look like:
        /// 
        /// List&lt;ISequence&gt; sequences = new List&lt;ISequence&gt;();
        /// sequences.Add(new Sequence(Alphabets.DNA,"GATTCAAGGGCT");
        /// sequences.Add(new Sequence(Alphabets.DNA,"TACATTAGGGTTCT");
        /// 
        /// SegmentedSequence mySequence = new SegmentedSequence(sequences);
        /// 
        /// The correlary for RNA:
        /// 
        ///  List&lt;ISequence&gt; sequences = new List&lt;ISequence&gt;();
        /// sequences.Add(new Sequence(Alphabets.RNA,"GAUUCAAGGGCU");
        /// sequences.Add(new Sequence(Alphabets.RNA,"AUGUCAAGUUGCU");
        /// 
        /// SegmentedSequence mySequence = new SegmentedSequence(sequences);
        /// </summary>
        /// <param name="sequences">
        /// A list containing ISequence instances.
        /// </param>
        public SegmentedSequence(ICollection<ISequence> sequences)
        {
            if (sequences == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequences);
            }

            if (sequences.Count == 0)
            {
                throw new ArgumentException(
                    Properties.Resource.ParameterEmpty,
                    Properties.Resource.ParameterNameSequences);
            }

            _seqInfo = new BasicSequenceInfo();
            Alphabet = null;
            _sequences = new ObservableCollection<ISequence>();

            foreach (ISequence seq in sequences)
            {
                if (seq == null)
                {
                    throw new ArgumentException(
                        Properties.Resource.ParameterContainsNullValue,
                        Properties.Resource.ParameterNameSequences);
                }

                if (Alphabet == null)
                {
                    Alphabet = seq.Alphabet;
                }
                else if (seq.Alphabet != Alphabet)
                {
                    throw new ArgumentException(Properties.Resource.AlphabetMismatchInConstructor);
                }

                _sequences.Add(seq);
            }

            _sequences.CollectionChanged += new NotifyCollectionChangedEventHandler(SequenceCollectionChanged);
        }

        #endregion Constructors

        #region Methods
        /// <summary>
        /// Creates a new SegmentedSequence that is a copy of the current SegmentedSequence.
        /// </summary>
        /// <returns>A new SegmentedSequence that is a copy of this SegmentedSequence.</returns>
        public SegmentedSequence Clone()
        {
            // Create a new SegmentedSequence using private constructor.
            SegmentedSequence cloneObject = new SegmentedSequence();

            // Get the copy of basic sequence info.
            cloneObject._seqInfo = _seqInfo.Clone();

            cloneObject._sequences = new ObservableCollection<ISequence>();

            foreach (ISequence seq in _sequences)
            {
                cloneObject._sequences.Add(seq.Clone());
            }

            cloneObject._sequences.CollectionChanged += new NotifyCollectionChangedEventHandler(cloneObject.SequenceCollectionChanged);

            Object documentation = Documentation;

            // If documentation is ICloneable then get the copy of it.
            ICloneable cloneableDocument = documentation as ICloneable;
            if (cloneableDocument != null)
            {
                documentation = cloneableDocument.Clone();
            }

            cloneObject.Documentation = documentation;
            cloneObject.MoleculeType = MoleculeType;

            return cloneObject;
        }

        /// <summary>
        /// Returns the local index of the sequence in this SegmentedSequence for the specified position.
        /// Position and local index are zero based.
        /// 
        /// For example:
        /// 
        /// If the SegmentedSequence contains two DNA sequences "ACGCAA" and "GGCC".
        /// The first sequence length is 6 and second sequence length is 4 and total lenght 
        /// of the SegmentedSequence is 10. 
        /// 
        /// Sequences                   "ACGCAA"  "GGCC"
        /// Segmented Sequence Index     012345    6789
        /// Local Index                  012345    0123
        ///                             
        /// If we pass 8 as parameter to this method, this method will returns 2 as local index.
        /// This is becase the position 8 corresponds to the index 2 in the second sequence.
        /// 
        /// If we pass 10 as parameter to this method, this method will returns -1 as there is 
        /// no corresponding local index found in the sequence.
        /// </summary>
        /// <param name="position">Position for which the local index is required.</param>
        /// <returns>If found returns local index of the sequence in the sequence list for the 
        /// specified position else -1 will be returned.</returns>
        private int GetSequenceLocalPosition(int position)
        {
            if (position >= 0)
            {
                int searchIndex = 0;
                foreach (ISequence seq in _sequences)
                {
                    searchIndex += seq.Count;

                    if (position < searchIndex)
                    {
                        // Return the local index of the sequence.
                        return position - (searchIndex - seq.Count);
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns the index of the sequence in the underlying sequences list which contains 
        /// the ISequenceItem at the specified position.
        /// </summary>
        /// <param name="position">Position of the ISequenceItem in this segmented sequence.</param>
        /// <returns>List index of the sequence which contains the ISequenceItem at the specified position.</returns>
        private int GetSequenceListIndexByPosition(int position)
        {
            int listIndex = -1;
            if (position >= 0)
            {
                int searchIndex = 0;
                for (int i = 0; i < _sequences.Count; i++)
                {
                    ISequence seq = _sequences[i];
                    searchIndex += seq.Count;

                    if (position < searchIndex)
                    {
                        listIndex = i;
                        break;
                    }
                }
            }

            return listIndex;
        }

        /// <summary>
        /// Returns the sequence in the underlying sequences list which corresponds to the specified position.
        /// </summary>
        /// <param name="position">Position of which the sequence is needed.</param>
        /// <returns>If found returns the Sequence else returns null.</returns>
        private ISequence GetSequenceByPosition(int position)
        {
            if (position >= 0)
            {
                int searchIndex = 0;
                foreach (ISequence seq in _sequences)
                {
                    searchIndex += seq.Count;

                    if (position < searchIndex)
                    {
                        return seq;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Creates a new sequence from the specified sequence data.
        /// 
        /// IsReadOnly property of the sequence created by this method will set to false.
        /// 
        /// This method uses the Alphabet of this segmented sequence for creating a new sequence and thus
        /// Alphabet of this segmented sequence has to be set before calling this method.
        /// </summary>
        /// <param name="sequence">Sequence data.</param>
        /// <returns>ISequence instance containing the specified sequence data.</returns>
        private ISequence CreateSequence(string sequence)
        {
            Sequence seq = new Sequence(Alphabet);
            if (!string.IsNullOrEmpty(sequence))
            {
                seq.InsertRange(0, sequence);
            }

            return seq;
        }

        #endregion Methods

        #region Event handlers
        /// <summary>
        /// This event will be fired when there is change in the sequence list.
        /// 
        /// This method will take care of verifying the Alphabet of added, 
        /// inserted or replaced Sequence with the Alphabet of this instance.
        /// If Alphabets doesnot match then an ArgumentException will occur.
        /// </summary>
        /// <param name="sender">ObservableCollection of Sequences.</param>
        /// <param name="e">Event argument.</param>
        private void SequenceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Do the verification for the add, insert and replace actions.
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
            {
                bool isInvalidSeqAdded = false;
                bool isInvalidAlphabetSeqAdded = false;
                bool isNullSeqAdded = false;

                if (e.NewItems != null)
                {
                    foreach (ISequence seq in e.NewItems)
                    {
                        if (seq == null)
                        {
                            isInvalidSeqAdded = true;
                            isNullSeqAdded = true;
                            break;
                        }

                        if (seq.Alphabet != Alphabet)
                        {
                            isInvalidSeqAdded = true;
                            isInvalidAlphabetSeqAdded = true;
                            break;
                        }
                    }
                }

                // If any one invalid sequence found then rollback the entire action.
                if (isInvalidSeqAdded)
                {
                    // Unregister the event to avoid firing of the event while doing the rollback action.
                    _sequences.CollectionChanged -= new NotifyCollectionChangedEventHandler(SequenceCollectionChanged);

                    // If the action was add or insert then rollback it by calling remove.
                    if (e.Action == NotifyCollectionChangedAction.Add)
                    {
                        for (int i = (e.NewItems.Count + e.NewStartingIndex - 1); i >= e.NewStartingIndex; i--)
                        {
                            _sequences.RemoveAt(i);
                        }
                    }
                    else
                    {
                        // If the action was replace then rollback the action by replacing them back to original place.
                        if (e.OldItems != null)
                        {
                            for (int i = e.OldStartingIndex; i < e.OldStartingIndex + e.OldItems.Count; i++)
                            {
                                _sequences[i] = e.OldItems[i - e.OldStartingIndex] as ISequence;
                            }
                        }
                    }

                    // Register back the event.
                    _sequences.CollectionChanged += new NotifyCollectionChangedEventHandler(SequenceCollectionChanged);

                    // Throw the exception.
                    if (isInvalidAlphabetSeqAdded)
                    {
                        throw new ArgumentException(Properties.Resource.AlphabetMismatchInEdit);
                    }
                    else if (isNullSeqAdded)
                    {
                        throw new ArgumentException(Properties.Resource.CannotAddNullSequence);
                    }
                }
            }
        }

        #endregion Event handlers

        #region ISequence Members

        /// <summary>
        /// Encodes the sequence item and places it at the indicated position
        /// within the sequence data.
        /// </summary>
        /// <param name="position">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to start at the begging
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="character">The item to be encoded placed into the sequence</param>
        public void Insert(int position, char character)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position > Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanOrEqualToCount);
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

            Insert(position, seqItem);
        }

        /// <summary>
        /// Encodes sequence parameter and places the values obtained at the
        /// indicated position within the current sequence data.
        /// </summary>
        /// <param name="position">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to start at the begging
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="sequence">The sequence to be encoded placed into the sequence</param>
        public void InsertRange(int position, string sequence)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position > Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanOrEqualToCount);
            }

            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequence);
            }

            List<ISequenceItem> seqItemList = Helper.GetSequenceItems(Alphabet, sequence);

            int localPosition = 0;
            ISequence seq = null;
            int listIndex = 0;
            if (position == Count)
            {
                listIndex = _sequences.Count;
            }
            else
            {
                localPosition = GetSequenceLocalPosition(position);
                seq = GetSequenceByPosition(position);
                listIndex = GetSequenceListIndexByPosition(position);
            }

            // If the specified sequence has to be added at the beginning or end of a sequence in the 
            // sequences list then create a new sequence and add the new sequence to sequences list. 
            if (localPosition == 0)
            {
                ISequence newSeq = CreateSequence(sequence);
                _sequences.Insert(listIndex, newSeq);
            }
            else
            {
                foreach (ISequenceItem seqItem in seqItemList)
                {
                    seq.Insert(localPosition++, seqItem);
                }
            }
        }

        /// <summary>
        /// Removes the sequence data from the specified position for a specified 
        /// number of characters.
        /// </summary>
        /// <param name="position">
        /// The position within the data to remove the data item. Note that this
        /// position starts its counting from 0. Thus to remove the first item
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="length">The number of characters to remove.</param>
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
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameLength,
                    Properties.Resource.ParameterMustNonNegative);
            }

            if ((Count - position) < length)
            {
                throw new ArgumentException(
                    Properties.Resource.InvalidPositionAndLength,
                    Properties.Resource.ParameterNameLength);
            }

            int startIndex = position;
            int endIndex = position + length - 1;

            int firstSeqListIndex = GetSequenceListIndexByPosition(startIndex);
            int firstSeqLocalIndex = GetSequenceLocalPosition(startIndex);
            ISequence firstSeq = GetSequenceByPosition(startIndex);

            int lastSeqListIndex = GetSequenceListIndexByPosition(endIndex);
            int lastSeqLocalIndex = GetSequenceLocalPosition(endIndex);
            ISequence lastSeq = GetSequenceByPosition(endIndex);

            // Check for repeated sequence references in the sequences to be removed. if in the 
            // first and last sequences only a part of the sequence has to be removed and they 
            // are refering to the same sequence instance then throw an exception.
            if (firstSeqListIndex != lastSeqListIndex)
            {
                if (firstSeqLocalIndex > 0 && lastSeqLocalIndex != lastSeq.Count - 1)
                {
                    if (firstSeq == lastSeq)
                    {
                        throw new InvalidOperationException(
                            string.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resource.CannotContinueWithRemove,
                            firstSeqListIndex,
                            lastSeqListIndex));
                    }
                }
            }

            bool isSeqRemovedFromFirstSeq = false;
            // Verify whether only a part of the first sequence to be remove.
            if (firstSeqLocalIndex > 0 || (firstSeqLocalIndex == 0 && length < (firstSeq.Count - firstSeqLocalIndex)))
            {
                int lengthToRemove = firstSeq.Count - firstSeqLocalIndex;
                if (length < lengthToRemove)
                {
                    lengthToRemove = length;
                }

                for (int i = firstSeqLocalIndex + lengthToRemove - 1; i >= firstSeqLocalIndex; i--)
                {
                    firstSeq.RemoveAt(i);
                }

                isSeqRemovedFromFirstSeq = true;
            }

            // Verify whether only a part of the last sequence has to be removed.
            if (firstSeqListIndex != lastSeqListIndex)
            {
                if (lastSeqLocalIndex != lastSeq.Count - 1)
                {
                    for (int i = lastSeqLocalIndex; i >= 0; i--)
                    {
                        lastSeq.RemoveAt(i);
                    }

                    lastSeqListIndex -= 1;
                }
            }

            if (isSeqRemovedFromFirstSeq)
            {
                firstSeqListIndex++;
            }

            // If the rest of the sequences from the list.
            // Note that no need to remove the data in these sequences.
            for (int i = lastSeqListIndex; i >= firstSeqListIndex; i--)
            {
                _sequences.RemoveAt(i);
            }
        }

        /// <summary>
        /// Encodes the specified sequence item and places it at the indicated position
        /// within the current sequence data, replacing the item currently
        /// located at that position.
        /// </summary>
        /// <param name="position">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to replace the first item
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="item">The item to be encoded and placed into the sequence</param>
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

            int localPosition = GetSequenceLocalPosition(position);
            ISequence seq = GetSequenceByPosition(position);
            seq.Replace(localPosition, item);
        }

        /// <summary>
        /// Encodes the sequence item and places it at the indicated position
        /// within the current sequence data, replacing the item currently
        /// located at that position.
        /// </summary>
        /// <param name="position">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to replace the first item
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="character">The symbol of the item to be encoded and placed into the sequence</param>
        public void Replace(int position, char character)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (position < 0 || position >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            int localPosition = GetSequenceLocalPosition(position);
            ISequence seq = GetSequenceByPosition(position);
            seq.Replace(localPosition, character);
        }

        /// <summary>
        /// Encodes the sequence and places it at the indicated position
        /// within the current sequence data, replacing the items currently
        /// located within that range. The number of items replaced will
        /// match the length of the sequence passed in.
        /// </summary>
        /// <param name="position">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to replace the first item
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="sequence">The item to be encoded placed into the sequence</param>
        public void ReplaceRange(int position, string sequence)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequence);
            }

            // Check whether the all charater in the sequence are known by alphabet of this sequence or not.
            char invalidChar;
            if (!Helper.IsValidSequence(Alphabet, sequence, out invalidChar))
            {
                throw new ArgumentException(
                        string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.InvalidSymbol,
                        invalidChar));
            }

            RemoveRange(position, sequence.Length);
            InsertRange(position, sequence);
        }

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
        /// Keeps track of the number of occurrances of each symbol within a sequence.  These are
        /// recalculated every time the getter as called, as the segments of the sequence may have
        /// been externally modified.
        /// </summary>
        public SequenceStatistics Statistics
        {
            get
            {
                return new SequenceStatistics(Sequences);
            }
        }

        /// <summary>
        /// The Documentation object is intended for tracking the history, provenance,
        /// and experimental context of a sequence. The user can adopt any desired
        /// convention for use of this object.
        /// </summary>
        public Object Documentation { set; get; }

        /// <summary>
        /// The alphabet to which string representations of the sequence should
        /// conform.
        /// </summary>
        public IAlphabet Alphabet
        {
            get
            {
                return _seqInfo.Alphabet;
            }

            internal set
            {
                _seqInfo.Alphabet = value;
            }
        }

        /// <summary>
        /// The molecule type (DNA, protein, or various kinds of RNA) the sequence encodes.
        /// </summary>
        public MoleculeType MoleculeType { get; set; }

        /// <summary>
        /// A flag indicating whether or not edits can be made to this Sequence.
        /// Returns true if IsReadOnly property of any one of the underlying sequence is true.
        /// Returns false if IsReadOnly property of all underlying sequences are false.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                foreach (ISequence seq in _sequences)
                {
                    if (seq.IsReadOnly)
                    {
                        return true;

                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Returns a string representation of the sequence data. This representation
        /// will come from the symbols in the alphabet defined for all underlying 
        /// sequences in this seqmented sequence.
        /// 
        /// Thus a Sequence whose Alphabet is Alphabets.DNA may return a value like
        /// 'GATTCCA'
        /// 
        /// If any underlying sequence does not supports tostring method then an exception will ocuur.
        /// </summary>
        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();

            foreach (ISequence seq in _sequences)
            {
                buff.Append(seq.ToString());
            }

            return buff.ToString();
        }

        /// <summary>
        /// Return a readonly sequence representing this sequence with the orientation reversed.
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
        /// Return a readonly sequence representing the complement of this sequence.
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
        /// Return a readonly sequence representing the reverse complement of this sequence.
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
        /// Return a readonly sequence representing a range (substring) of this sequence.
        /// </summary>
        /// <param name="start">The index of the first symbol in the range.</param>
        /// <param name="length">The number of symbols in the range.</param>
        /// <returns>The virtual sequence.</returns>
        public ISequence Range(int start, int length)
        {
            if (start < 0 || start >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNameStart,
                    Properties.Resource.ParameterMustLessThanOrEqualToCount);
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

            int localIndex = GetSequenceLocalPosition(startPos);
            ISequence seq = GetSequenceByPosition(startPos);

            return seq.IndexOfNonGap(localIndex);
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

            int localIndex = GetSequenceLocalPosition(endPos);
            ISequence seq = GetSequenceByPosition(endPos);

            return seq.LastIndexOfNonGap(localIndex);
        }

        /// <summary>
        /// Creates a new SegmentedSequence that is a copy of the current SegmentedSequence.
        /// </summary>
        /// <returns>A new ISequence that is a copy of this SegmentedSequence.</returns>
        ISequence ISequence.Clone()
        {
            return Clone();
        }
        #endregion ISequence Members

        #region IList<ISequenceItem> Members
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
                {
                    throw new ArgumentOutOfRangeException(
                        Properties.Resource.IndexParameterName,
                        Properties.Resource.ParameterMustLessThanOrEqualToCount);
                }

                int localIndex = GetSequenceLocalPosition(index);
                ISequence seq = GetSequenceByPosition(index);
                return seq[localIndex];
            }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

                if (index < 0 || index >= Count)
                {
                    throw new ArgumentOutOfRangeException(
                        Properties.Resource.IndexParameterName,
                        Properties.Resource.ParameterMustLessThanOrEqualToCount);
                }

                int localIndex = GetSequenceLocalPosition(index);
                ISequence seq = GetSequenceByPosition(index);
                seq[localIndex] = value;
            }
        }

        /// <summary>
        /// Returns the index of the first item matching the item
        /// passed in to the parameter.
        /// </summary>
        /// <returns>The index of the first matched item. Counting starts at 0.</returns>
        public int IndexOf(ISequenceItem item)
        {
            int index = 0;
            int localIndex = -1;

            foreach (ISequence seq in _sequences)
            {
                localIndex = seq.IndexOf(item);
                if (localIndex > -1)
                {
                    break;
                }

                index = index + seq.Count;
            }

            if (localIndex == -1)
            {
                index = -1;
            }
            else
            {
                index = index + localIndex;
            }

            return index;
        }

        /// <summary>
        /// Encodes the sequence item and places it at the indicated position
        /// within the current sequence data.
        /// </summary>
        /// <param name="position">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to start at the begging
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="item">The item to be encoded placed into the sequence</param>
        public void Insert(int position, ISequenceItem item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);


            if (position < 0 || position > Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanOrEqualToCount);
            }

            if (item == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameItem);
            }

            if (Alphabet.LookupBySymbol(item.Symbol) == null)
            {
                throw new ArgumentException(
                    Properties.Resource.ParameterNotKnownByAlphabet,
                    Properties.Resource.ParameterNameItem);
            }


            ISequence seq = null;
            int localPosition = 0;

            // If insert has to be done at the end of the sequence first check the last sequence
            // if no sequence is found then add a new sequence, else get the last sequence 
            // in the list and add the ISequenceItem to it.
            if (position == Count)
            {
                if (_sequences.Count == 0)
                {
                    seq = CreateSequence(string.Empty);
                    _sequences.Add(seq);
                }
                else
                {
                    seq = _sequences[_sequences.Count - 1];
                }

                localPosition = seq.Count;
            }
            else
            {
                seq = GetSequenceByPosition(position);
                localPosition = GetSequenceLocalPosition(position);
            }

            seq.Insert(localPosition, item);
        }

        /// <summary>
        /// Removes the sequence data item at the indicated position.
        /// </summary>
        /// <param name="position">
        /// The position within the data to remove the data item. Note that this
        /// position starts its counting from 0. Thus to remove the first item
        /// of the sequence, set this parameter to 0.
        /// </param>
        public void RemoveAt(int position)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);


            if (position < 0 || position >= Count)
            {
                throw new ArgumentOutOfRangeException(
                    Properties.Resource.ParameterNamePosition,
                    Properties.Resource.ParameterMustLessThanCount);
            }

            int localPosition = GetSequenceLocalPosition(position);
            int listIndex = GetSequenceListIndexByPosition(position);
            ISequence seq = GetSequenceByPosition(position);

            if (seq.Count == 1)
            {
                _sequences.RemoveAt(listIndex);
            }
            else
            {
                seq.RemoveAt(localPosition);
            }
        }
        #endregion IList<ISequenceItem> Members

        #region ICollection<ISequenceItem> Members
        /// <summary>
        /// The number of sequence items contained in the Sequence.
        /// </summary>
        public int Count
        {
            get
            {
                int count = 0;
                for (int i = 0; i < _sequences.Count; i++)
                {
                    count += _sequences[i].Count;
                }

                return count;
            }
        }

        /// <summary>
        /// Adds a sequence item to the end of the sequence. The Sequence
        /// must not be marked as read only in order to make this change.
        /// </summary>
        /// <param name="item">The item to add to the end of the sequence</param>
        public void Add(ISequenceItem item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            if (item == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameItem);
            }

            if (Alphabet.LookupBySymbol(item.Symbol) == null)
            {
                throw new ArgumentException(
                    Properties.Resource.ParameterNotKnownByAlphabet,
                    Properties.Resource.ParameterNameItem);
            }

            if (_sequences.Count > 0)
            {
                ISequence seq = _sequences[_sequences.Count - 1];
                seq.Add(item);
            }
            else
            {
                Insert(0, item);
            }
        }

        /// <summary>
        /// Removes all underlying sequences from the sequences list of SegmentedSequence.  The Sequence
        /// must not be marked as read only in order to make this change.
        /// </summary>
        public void Clear()
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            _sequences.Clear();
        }

        /// <summary>
        /// Indicates if a sequence item is contained in the sequence anywhere.
        /// </summary>
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
            int index = arrayIndex;
            foreach (ISequence seq in _sequences)
            {
                seq.CopyTo(array, index);
                index += seq.Count;
            }
        }

        /// <summary>
        /// Removes the first occurrence of the specified sequence item.
        /// </summary>
        /// <param name="item">The sequence item to be removed.</param>
        /// <returns>True if the item was found and removed, false if the item was not found.</returns>
        public bool Remove(ISequenceItem item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException(Properties.Resource.CanNotModifyReadonlySequence);

            int position;
            position = IndexOf(item);

            if (position < 0)
                return false;

            RemoveAt(position);
            return true;
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
        /// Creates a new SegmentedSequence that is a copy of the current SegmentedSequence.
        /// </summary>
        /// <returns>A new object that is a copy of this SegmentedSequence.</returns>
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
        protected SegmentedSequence(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _seqInfo = (BasicSequenceInfo)info.GetValue("SegmentedSequence:seqInfo", typeof(BasicSequenceInfo));

            Documentation = info.GetValue("SegmentedSequence:Documentation", typeof(object));
            MoleculeType = (MoleculeType)info.GetValue("SegmentedSequence:MoleculeType", typeof(MoleculeType));

            _sequences = (ObservableCollection<ISequence>)info.GetValue(
                "SegmentedSequence:Sequences",
                typeof(ObservableCollection<ISequence>));

            _sequences.CollectionChanged += new NotifyCollectionChangedEventHandler(SequenceCollectionChanged);
        }

        /// <summary>
        /// Method for serializing the SegmentedSequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("SegmentedSequence:seqInfo", _seqInfo);

            if (Documentation != null && ((Documentation.GetType().Attributes &
                           System.Reflection.TypeAttributes.Serializable) == System.Reflection.TypeAttributes.Serializable))
            {
                info.AddValue("SegmentedSequence:Documentation", Documentation);
            }
            else
            {
                info.AddValue("SegmentedSequence:Documentation", null);
            }

            info.AddValue("SegmentedSequence:MoleculeType", MoleculeType);

            info.AddValue("SegmentedSequence:Sequences", _sequences);
        }

        #endregion
    }
}
