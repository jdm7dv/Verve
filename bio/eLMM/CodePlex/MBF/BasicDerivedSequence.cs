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
using System.Runtime.Serialization;
using System.Text;
using Bio.Util.Logging;

namespace Bio
{
    /// <summary>
    /// A BasicDerivedSequence is a simple type of derived sequence that 
    /// provides range, reversal, and complementation of a source sequence,
    /// and facilitates creation of other specialized subclasses.
    /// 
    /// This class implements ICloneable interface. To create a copy 
    /// of the BasicDerivedSequence call Clone() method. For example:
    /// 
    /// BasicDerivedSequence basicDerivedSeq = new BasicDerivedSequence();
    /// BasicDerivedSequence basicDerivedSeqCopy = basicDerivedSeq.Clone();
    ///
    /// This class is marked with Serializable attribute thus instances of this 
    /// class can be serialized and stored to files and the stored files 
    /// can be de-serialized to restore the instances.
    /// </summary>
    [Serializable]
    public class BasicDerivedSequence : IDerivedSequence
    {
        #region Constructors

        /// <summary>
        /// Construct a derived sequence, specifying all properties.
        /// </summary>
        /// <param name="source">The source sequence.</param>
        /// <param name="reversed">Whether to reverse the symbol order.</param>
        /// <param name="complemented">Whether to complement the symbols.</param>
        /// <param name="rangeStart">The first symbol index (in the original) wanted.</param>
        /// <param name="rangeLength">0 to return all symbols, or the number of symbols wanted.</param>
        public BasicDerivedSequence(ISequence source, bool reversed, bool complemented,
            int rangeStart, int rangeLength)
        {
            Source = source;
            Reversed = reversed;
            Complemented = complemented;
            RangeStart = rangeStart;
            RangeLength = rangeLength;
            if (Complemented && (Source.Alphabet != Alphabets.DNA && Source.Alphabet != Alphabets.RNA))
            {
                string message = "BasicDerivedSequence: Complement is only allowed for nucleotide sequences.";
                Trace.Report(message);
                throw new NotSupportedException(message);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// True if the source symbols will be reversed.
        /// </summary>
        public bool Reversed { set; get; }

        /// <summary>
        /// True if the source symbols will be complemented.
        /// </summary>
        public bool Complemented { set; get; }

        /// <summary>
        /// If positive, an offset into the source sequence.
        /// Otherwise the starting source index is zero.
        /// </summary>
        public int RangeStart { set; get; }

        /// <summary>
        /// If positive, the number of symbols the virtual sequence should present.
        /// Otherwise the length is not limited.
        /// </summary>
        public int RangeLength { set; get; }

        #endregion

        #region Methods
        /// <summary>
        /// Creates a new BasicDerivedSequence that is a copy of the current BasicDerivedSequence.
        /// Source property of cloned BasicDerivedSequence will contains a cloned copy of the 
        /// Source property of current BasicDerivedSequence.
        /// </summary>
        /// <returns>A new BasicDerivedSequence that is a copy of this BasicDerivedSequence.</returns>
        public BasicDerivedSequence Clone()
        {
            // Create new object of BasicDerivedSequence by passing the clone copy of the sequence.
            return new BasicDerivedSequence(Source.Clone(), Reversed, Complemented, RangeStart, RangeLength);
        }
        #endregion

        #region IList<ISequenceItem> Members

        /// <summary>
        /// Returns the index of the first item matching the item
        /// passed in to the parameter. This does not do a symbol
        /// comparison. The match must be the exact same ISequenceItem.
        /// </summary>
        /// <returns>The index of the first matched item. Counting starts at 0.</returns>
        public int IndexOf(ISequenceItem item)
        {
            int index = 0;
            foreach (ISequenceItem testItem in this)
            {
                if (testItem == item)
                {
                    return index;
                }
                ++index;
            }
            return -1;  // not found
        }

        /// <summary>
        /// Allows the sequence to function like an array, getting and setting
        /// the sequence item at the particular index specified. Note that the
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

                int sourceLength = Source.Count;
                int trueIndex = index;
                int start = RangeStart;

                // If RangeStart is negative then treat it as zero.
                if (start < 0)
                {
                    start = 0;
                }

                // Get the true index (index of the source sequence).
                if (Reversed)
                {
                    trueIndex = (start + Count - 1) - index;
                }
                else
                {
                    trueIndex += start;
                }

                ISequenceItem item = Source[trueIndex];
                // SparseSequence will return null if there is no sequence item present in the specified position.
                if ( item == null)
                {
                    return null;
                }

                if (Complemented)
                {
                    if (Alphabet == Alphabets.DNA)
                    {
                        return Complementation.GetDnaComplement((Nucleotide)item);
                    }
                    else
                    {
                        return Complementation.GetRnaComplement((Nucleotide)item);
                    }
                }
                else
                {
                    return item;
                }
            }
            set
            {
                string message = "Modifying a source sequence through a derived sequence is not supported.";
                Trace.Report(message);
                throw new NotSupportedException(message);
            }
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
            string message = "Modifying a source sequence through a derived sequence is not supported.";
            Trace.Report(message);
            throw new NotSupportedException(message);
        }

        /// <summary>
        /// Removes the sequence data item at the indicated position
        /// </summary>
        /// <param name="position">
        /// The position within the data to remove the data item. Note that this
        /// position starts its counting from 0. Thus to remove the first item
        /// of the sequence, set this parameter to 0.
        /// </param>
        public void RemoveAt(int position)
        {
            string message = "Modifying a source sequence through a derived sequence is not supported.";
            Trace.Report(message);
            throw new NotSupportedException(message);
        }

        #endregion

        #region ICollection<ISequenceItem> Members
        /// <summary>
        /// Adds a sequence item to the end of the sequence. The Sequence
        /// must not be marked as read only in order to make this change.
        /// </summary>
        /// <param name="item">The item to add to the end of the sequence</param>
        public void Add(ISequenceItem item)
        {
            string message = "Modifying a source sequence through a derived sequence is not supported.";
            Trace.Report(message);
            throw new NotSupportedException(message);
        }

        /// <summary>
        /// Removes all sequence data from the Sequence.  The Sequence
        /// must not be marked as read only in order to make this change.
        /// </summary>
        public void Clear()
        {
            string message = "Modifying a source sequence through a derived sequence is not supported.";
            Trace.Report(message);
            throw new NotSupportedException(message);
        }

        /// <summary>
        /// Indicates if a sequence item is contained in the sequence anywhere.
        /// Note that the SequenceItem must be taken from the alphabet defined
        /// for this sequence in order for this method to return true.
        /// </summary>
        /// <param name="item">The SequenceItem to check for the existance of in a Sequence</param>
        public bool Contains(ISequenceItem item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Copies the sequence items into a preallocated array.
        /// </summary>
        ///
        /// <param name="array">An array of SequenceItems to which sequence items are copied</param>
        /// <param name="arrayIndex">The index of the array in which to start the copy</param> 
        public void CopyTo(ISequenceItem[] array, int arrayIndex)
        {
            if (arrayIndex < 0 || (arrayIndex + Count) > array.Length)
            {
                throw new ArgumentOutOfRangeException("Destination array was not long enough");
            }

            foreach (ISequenceItem item in this)
            {
                array[arrayIndex++] = item;
            }
        }

        /// <summary>
        /// The number of sequence items contained in the Sequence.
        /// </summary>
        public int Count
        {
            get
            {
                int sourceLength = Source.Count;
                if (RangeStart >= 0)
                {
                    if (RangeLength >= 0)
                    {
                        if (RangeStart + RangeLength < sourceLength)
                        {
                            return RangeLength;
                        }
                    }

                    // If RangeLength is negative then return source length - rangestart.
                    if (sourceLength - RangeStart > 0)
                    {
                        return sourceLength - RangeStart;
                    }
                    else
                    {
                        // If rangeStart is greater than or equal to the source length then return count as zero.
                        return 0;
                    }
                }

                if (RangeLength >= 0)
                {
                    if (RangeLength < sourceLength)
                    {
                        return RangeLength;
                    }
                }

                // If both RangeLength and RangeStart are negative then return source length.
                return sourceLength;
            }
        }

        /// <summary>
        /// A flag indicating whether or not edits can be made to this Sequence.
        /// A BasicDerivedSequence does not support editing of the underlying
        /// sequence, and is always read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Removes the first instance found of a particular sequence item.
        /// This item must be from the alphabet defined for the Sequence.
        /// </summary>
        /// <param name="item">The items to search for and remove.</param>
        /// <returns>True if the item was found and removed, false if the item was not found.</returns>
        public bool Remove(ISequenceItem item)
        {
            string message = "Modifying a source sequence through a derived sequence is not supported.";
            Trace.Report(message);
            throw new NotSupportedException(message);
        }

        #endregion

        #region IEnumerable<ISequenceItem> Members

        /// <summary>
        /// Retrieves an enumerator for this sequence.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<ISequenceItem> GetEnumerator()
        {
            return new BasicDerivedSequenceEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new BasicDerivedSequenceEnumerator(this);
        }

        #endregion

        #region ISequence members

        /// <summary>
        /// An identification provided to distinguish the sequence to others
        /// being worked with.
        /// </summary>
        public string ID
        {
            get
            {
                return Source.ID;
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
                return Source.ID;
            }
        }

        /// <summary>
        /// The alphabet to which string representations of the sequence should
        /// conform.
        /// </summary>
        public IAlphabet Alphabet
        {
            get
            {
                return Source.Alphabet;
            }
        }

        /// <summary>
        /// The molecule type (DNA, protein, or various kinds of RNA) the sequence encodes.
        /// </summary>
        public MoleculeType MoleculeType
        {
            get
            {
                return Source.MoleculeType;
            }
        }

        /// <summary>
        /// Keeps track of the number of occurrances of each symbol within a sequence.  This is
        /// recalculated each time the getter is called, in case there has been a change to the
        /// source sequence.
        /// </summary>
        public SequenceStatistics Statistics
        {
            get
            {
                if (RangeStart > 0 || RangeLength > 0)
                {
                    return new SequenceStatistics(this);
                }
                else if (Complemented)
                {
                    return Source.Statistics.Complement;
                }
                return Source.Statistics;
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
            get
            {
                return Source.Metadata;
            }
        }

        /// <summary>
        /// The Documentation object is intended for tracking the history, provenance,
        /// and experimental context of a sequence. The user can adopt any desired
        /// convention for use of this object.
        /// </summary>
        public Object Documentation
        {
            get
            {
                return Source.Documentation;
            }
            set
            {
                string message = "Modifying a source sequence through a derived sequence is not supported.";
                Trace.Report(message);
                throw new NotSupportedException(message);
            }
        }

        /// <summary>
        /// Return the string representation of the Source sequence, as
        /// modified by this object's properties.
        /// </summary>
        /// <returns>The string representing the sequence.</returns>
        public override string ToString()
        {
            StringBuilder ret = new StringBuilder();
            for (int i = 0; i < this.Count; ++i)
            {
                ret.Append(this[i].Symbol);
            }
            return ret.ToString();
        }

        /// <summary>
        /// Return a virtual sequence representing this sequence with the orientation reversed.
        /// </summary>
        public ISequence Reverse
        {
            get
            {
                return new BasicDerivedSequence(this, true, false, -1, -1);
            }
        }

        /// <summary>
        /// Return a virtual sequence representing the complement of this sequence.
        /// </summary>
        public ISequence Complement
        {
            get
            {
                return new BasicDerivedSequence(this, false, true, -1, -1);
            }
        }

        /// <summary>
        /// Return a virtual sequence representing the reverse complement of this sequence.
        /// </summary>
        public ISequence ReverseComplement
        {
            get
            {
                return new BasicDerivedSequence(this, true, true, -1, -1);
            }
        }

        /// <summary>
        /// Return a virtual sequence representing a range (substring) of this sequence.
        /// </summary>
        /// <param name="start">The index of the first symbol in the range.</param>
        /// <param name="length">The number of symbols in the range.</param>
        /// <returns>The virtual sequence.</returns>
        public ISequence Range(int start, int length)
        {
            return new BasicDerivedSequence(this, false, false, start, length);
        }

        /// <summary>
        /// Modifying a source sequence through a derived sequence is not supported and thus 
        /// this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="character">The item to insert. Examples for DNA include: 'G' or 'C'</param>
        public void Insert(int position, char character)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedBasicDerivedSequence);
        }

        /// <summary>
        /// Modifying a source sequence through a derived sequence is not supported and thus 
        /// this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="sequence">The items to insert. Examples for DNA include: "G" or "GAAT"</param>
        public void InsertRange(int position, string sequence)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedBasicDerivedSequence);
        }

        /// <summary>
        /// Modifying a source sequence through a derived sequence is not supported and thus 
        /// this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="length">The number of continuous items to remove starting at the position</param>
        public void RemoveRange(int position, int length)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedBasicDerivedSequence);
        }

        /// <summary>
        /// Modifying a source sequence through a derived sequence is not supported and thus 
        /// this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="character">The item to insert. Examples from DNA include: 'G' or 'C'</param>
        public void Replace(int position, char character)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedBasicDerivedSequence);
        }

        /// <summary>
        /// Modifying a source sequence through a derived sequence is not supported and thus 
        /// this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="item">The item to place into the sequence</param>
        public void Replace(int position, ISequenceItem item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedBasicDerivedSequence);
        }

        /// <summary>
        /// Modifying a source sequence through a derived sequence is not supported and thus 
        /// this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="sequence">The items to insert. Examples for DNA include: "G" or "GAAT"</param>
        public void ReplaceRange(int position, string sequence)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedBasicDerivedSequence);
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
        /// Creates a new BasicDerivedSequence that is a copy of the current BasicDerivedSequence.
        /// </summary>
        /// <returns>A new ISequence that is a copy of this BasicDerivedSequence.</returns>
        ISequence ISequence.Clone()
        {
            return Clone();
        }

        #endregion

        #region IDerivedSequence members

        /// <summary>
        /// The Source sequence is the underlying sequence from which this virtual
        /// sequence is derived.
        /// </summary>
        public ISequence Source { get; set; }

        #endregion

        #region ICloneable Members
        /// <summary>
        /// Creates a new BasicDerivedSequence that is a copy of the current BasicDerivedSequence.
        /// </summary>
        /// <returns>A new object that is a copy of this BasicDerivedSequence.</returns>
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
        protected BasicDerivedSequence(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            Reversed = info.GetBoolean("BasicDerivedSequence:Reversed");
            Complemented = info.GetBoolean("BasicDerivedSequence:Complemented");
            RangeStart = info.GetInt32("BasicDerivedSequence:RangeStart");
            RangeLength = info.GetInt32("BasicDerivedSequence:RangeLength");
            Source = (ISequence)info.GetValue("BasicDerivedSequence:Source", typeof(ISequence));
        }

        /// <summary>
        /// Method for serializing the BasicDerivedSequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("BasicDerivedSequence:Reversed", Reversed);
            info.AddValue("BasicDerivedSequence:Complemented", Complemented);
            info.AddValue("BasicDerivedSequence:RangeStart", RangeStart);
            info.AddValue("BasicDerivedSequence:RangeLength", RangeLength);
            info.AddValue("BasicDerivedSequence:Source", Source);
        }

        #endregion
    }
    /// <summary>
    /// Enumerator implementation for the BasicDerivedSequence class
    /// </summary>
    public class BasicDerivedSequenceEnumerator : IEnumerator<ISequenceItem>
    {
        private BasicDerivedSequence seq;
        private int index;

        /// <summary>
        /// Constructs an enumerator for a BasicDerivedSequence object.
        /// </summary>
        public BasicDerivedSequenceEnumerator(BasicDerivedSequence sequence)
        {
            seq = sequence;
            Reset();
        }

        #region IEnumerator<ISequenceItem> Members
        /// <summary>
        /// The current item reference for the enumerator.
        /// </summary>
        public ISequenceItem Current
        {
            get
            {
                if (index < 0)
                    return null;

                return seq[index];
            }
        }

        #endregion

        #region IDisposable Members
        /// <summary>
        /// Takes care of any allocated memory
        /// </summary>
        public void Dispose()
        {
            // No op
        }

        /// <summary>
        /// Takes care of disposing memory
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        { 
            // No operation
        }

        #endregion

        #region IEnumerator Members
        /// <summary>
        /// The current item reference for the enumerator
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return seq[index]; }
        }

        /// <summary>
        /// Advances the enumerator to the next item
        /// </summary>
        /// <returns>True if the enumerator can advance. False if the end of the sequence is reached.</returns>
        public bool MoveNext()
        {
            if (index < (seq.Count - 1))
            {
                index++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets the enumerator to the start of the sequence
        /// </summary>
        public void Reset()
        {
            index = -1;
        }

        #endregion
    }
}
