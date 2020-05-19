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
using System.Runtime.Serialization;
using MBF.Algorithms.StringSearch;

namespace MBF
{
    /// <summary>
    /// VirtualSequence class is ISequence implementation which contains metadata of a sequence 
    /// and it will not contain the sequence. Use this class for working with sequences 
    /// that never have sequence data, but are only used for metadata storage (like keeping an ID 
    /// or various features but no direct sequence data).
    /// 
    /// This class is marked with Serializable attribute thus instances of this 
    /// class can be serialized and stored to files and the stored files 
    /// can be de-serialized to restore the instances.
    /// </summary>
    [Serializable]
    public class VirtualSequence : ISequence
    {
        #region Fields
        /// <summary>
        /// Holds the metadata of the sequence.
        /// </summary>
        private BasicSequenceInfo _seqInfo;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Gets a value indicating whether encoding is used while storing
        /// sequence in memory
        /// </summary>
        public bool UseEncoding
        {
            get { return false; }

            set { throw new InvalidOperationException(); }
        }

        /// <summary>
        /// Gets or sets the Pattern Finder used to short string in sequence
        /// </summary>
        public IPatternFinder PatternFinder { get; set; }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Private constructor for clone method.
        /// </summary>
        private VirtualSequence()
        {
        }

        /// <summary>
        /// Creates a VirtualSequence instance.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA or Alphabets.RNA or Alphabets.Protein)
        /// </param>
        public VirtualSequence(IAlphabet alphabet)
        {
            if (alphabet == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameAlphabet);
            }

            _seqInfo = new BasicSequenceInfo();
            Alphabet = alphabet;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Creates a new VirtualSequence that is a copy of the current VirtualSequence.
        /// </summary>
        /// <returns>A new VirtualSequence that is a copy of this VirtualSequence.</returns>
        public VirtualSequence Clone()
        {
            VirtualSequence cloneVertualSeq = new VirtualSequence();
            cloneVertualSeq._seqInfo = _seqInfo.Clone();
            cloneVertualSeq.MoleculeType = MoleculeType;

            Object documentation = Documentation;

            // If documentation is ICloneable then get the copy of it.
            ICloneable cloneableDocument = documentation as ICloneable;
            if (cloneableDocument != null)
            {
                documentation = cloneableDocument.Clone();
            }

            cloneVertualSeq.Documentation = documentation;


            return cloneVertualSeq;
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
        /// The alphabet type (eg. Alphabets.DNA or Alphabets.RNA or Alphabets.Protein).
        /// </summary>
        public IAlphabet Alphabet
        {
            get { return _seqInfo.Alphabet; }
            internal set { _seqInfo.Alphabet = value; }
        }

        /// <summary>
        /// The molecule type (DNA, protein, or various kinds of RNA) the sequence encodes.
        /// </summary>
        public MoleculeType MoleculeType { get; set; }

        /// <summary>
        /// Always returns null, since there is no sequence data.
        /// </summary>
        public SequenceStatistics Statistics
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this flag always returns true.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        public override string ToString()
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        public ISequence Reverse
        {
            get
            {
                throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
            }
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        public ISequence Complement
        {
            get
            {
                throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
            }
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        public ISequence ReverseComplement
        {
            get
            {
                throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
            }
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <param name="start">The index of the first symbol in the range.</param>
        /// <param name="length">The number of symbols in the range.</param>
        /// <returns>Always throws NotSupportedException.</returns>
        public ISequence Range(int start, int length)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="character">The item to insert. Examples for DNA include: 'G' or 'C'</param>
        public void Insert(int position, char character)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="sequence">The items to insert. Examples for DNA include: "G" or "GAAT"</param>
        public void InsertRange(int position, string sequence)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="length">The number of continuous items to remove starting at the position</param>
        public void RemoveRange(int position, int length)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="character">The item to insert. Examples from DNA include: 'G' or 'C'</param>
        public void Replace(int position, char character)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="item">The item to place into the sequence</param>
        public void Replace(int position, ISequenceItem item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="sequence">The items to insert. Examples for DNA include: "G" or "GAAT"</param>
        public void ReplaceRange(int position, string sequence)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        public int IndexOfNonGap()
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        public int IndexOfNonGap(int startPos)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        public int LastIndexOfNonGap()
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        public int LastIndexOfNonGap(int endPos)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Creates a new VirtualSequence that is a copy of the current VirtualSequence.
        /// </summary>
        /// <returns>A new ISequence that is a copy of this VirtualSequence.</returns>
        ISequence ISequence.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Finds the list of string that matches any of the patterns with the indices of each occurrence in sequence.
        /// </summary>
        /// <param name="patterns">List of patterns that needs to be searched in Sequence.</param>
        /// <param name="startIndex">Minimum index in Sequence at which match has to start.</param>
        /// <param name="ignoreCase">
        /// if true ignore character casing while match.
        /// <remarks>
        /// Note that symbols in Sequence are always Upper case.
        /// </remarks>
        /// </param>
        /// <returns></returns>
        public IDictionary<string, IList<int>> FindMatches(IList<string> patterns, int startIndex = 0, bool ignoreCase = true)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }
        #endregion ISequence Members

        #region IList<ISequenceItem> Members

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        public ISequenceItem this[int index]
        {
            get
            {
                throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
            }
            set
            {
                throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
            }
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method always returns -1.
        /// </summary>
        /// <returns>Always retuns -1.</returns>
        public int IndexOf(ISequenceItem item)
        {
            return -1;
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">
        /// The position within the data to place the new data. Note that this
        /// position starts its counting from 0. Thus to start at the begging
        /// of the sequence, set this parameter to 0.
        /// </param>
        /// <param name="item">The item to be encoded placed into the sequence</param>
        public void Insert(int position, ISequenceItem item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        ///  Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <param name="position">
        /// The position within the data to remove the data item. Note that this
        /// position starts its counting from 0. Thus to remove the first item
        /// of the sequence, set this parameter to 0.
        /// </param>
        public void RemoveAt(int position)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }
        #endregion IList<ISequenceItem> Members

        #region ICollection<ISequenceItem> Members

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method always returns 0.
        /// </summary>
        public int Count
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <param name="item">The item to add to the end of the sequence.</param>
        public void Add(ISequenceItem item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method always returns false.
        /// </summary>
        /// <returns>Always returns false.</returns>
        public bool Contains(ISequenceItem item)
        {
            return false;
        }

        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        public void CopyTo(ISequenceItem[] array, int arrayIndex)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        ///  Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <param name="item">The items to search for and remove.</param>
        /// <returns>Always throws NotSupportedException.</returns>
        public bool Remove(ISequenceItem item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        #endregion ICollection<ISequenceItem> Members

        #region IEnumerable<ISequenceItem> Members
        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <returns>Always throws NotSupportedException.</returns>
        public IEnumerator<ISequenceItem> GetEnumerator()
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        #endregion IEnumerable<ISequenceItem> Members

        #region IEnumerable Members
        /// <summary>
        /// Virtual sequence will not contain sequence and thus this method will throw NotSupportedException.
        /// </summary>
        /// <returns>Always throws NotSupportedException.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        #endregion IEnumerable Members

        #region ICloneable Members
        /// <summary>
        /// Creates a new VirtualSequence that is a copy of the current VirtualSequence.
        /// </summary>
        /// <returns>A new object that is a copy of this VirtualSequence.</returns>
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
        protected VirtualSequence(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _seqInfo = (BasicSequenceInfo)info.GetValue("VirtualSequence:seqInfo", typeof(BasicSequenceInfo));
            Documentation = info.GetValue("VirtualSequence:Documentation", typeof(object));
            MoleculeType = (MoleculeType)info.GetValue("VirtualSequence:MoleculeType", typeof(MoleculeType));
        }

        /// <summary>
        /// Method for serializing the VirtualSequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("VirtualSequence:seqInfo", _seqInfo);

            if (Documentation != null && ((Documentation.GetType().Attributes &
                           System.Reflection.TypeAttributes.Serializable) == System.Reflection.TypeAttributes.Serializable))
            {
                info.AddValue("VirtualSequence:Documentation", Documentation);
            }
            else
            {
                info.AddValue("VirtualSequence:Documentation", null);
            }

            info.AddValue("VirtualSequence:MoleculeType", MoleculeType);
        }

        #endregion
    }
}
