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

namespace Bio
{
    /// <summary>
    /// Implementations of ISequence make up the one of the core sets
    /// of data structures in MBF. It is these sequences that store
    /// data relevant to DNA, RNA, and Amino Acid structures. Several
    /// algorithms for alignment, assembly, and analysis take these items
    /// as their basic data inputs and outputs.
    /// 
    /// At its core, ISequence stores an ordered list of ISequenceItem,
    /// which in term can represent things like Nucleotides or Amino Acids.
    /// These can be accessed using any method made available through IList.
    /// You can also work with the sequence as a string by using the ToString()
    /// method which will decode the items in the list into a standard,
    /// readable format.
    /// 
    /// The basic implementation of this interface is Sequence, which stores
    /// byte codes for the sequence items. Additional implementations include
    /// VirtualSequence, SegmentedSequence, and SparseSequence.
    /// 
    /// ISequence extends from ICloneable interface this will enable 
    /// ISequence instances to create copies of them. 
    /// Calling Clone() method will create a copy of the ISequence.
    /// </summary>
    public interface ISequence : IList<ISequenceItem>, ICloneable, ISerializable
    {
        /// <summary>
        /// An identification provided to distinguish the sequence to others
        /// being worked with.
        /// </summary>
        string ID { get; }

        /// <summary>
        /// An identification of the sequence that is meant to be understood
        /// by human users when displayed in an application or file format.
        /// </summary>
        string DisplayID { get; }

        /// <summary>
        /// The alphabet to which string representations of the sequence should
        /// conform.
        /// </summary>
        IAlphabet Alphabet { get; }

        /// <summary>
        /// The molecule type (DNA, protein, or various kinds of RNA) the sequence encodes.
        /// </summary>
        MoleculeType MoleculeType { get; }

        /// <summary>
        /// Keeps track of the number of occurrences of each symbol within a sequence.
        /// </summary>
        SequenceStatistics Statistics { get; }

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
        Dictionary<string, object> Metadata { get; }

        /// <summary>
        /// The Documentation object is intended for tracking the history, provenance,
        /// and experimental context of a sequence. The user can adopt any desired
        /// convention for use of this object.
        /// </summary>
        Object Documentation { set; get; }

        /// <summary>
        /// Converts the list of sequence items into a more readable representation.
        /// This form may be particularly helpful for those users who are more
        /// familiar with string operations on sequences than iterating over
        /// individual entries.
        /// </summary>
        /// <returns>
        /// A string representation of the sequence data. For DNA a result
        /// may look like "GATTCAAG" for instance.
        /// </returns>
        string ToString();

        /// <summary>
        /// Return a virtual sequence representing this sequence with the orientation reversed.
        /// </summary>
        ISequence Reverse { get; }

        /// <summary>
        /// Return a virtual sequence representing the complement of this sequence.
        /// </summary>
        ISequence Complement { get; }

        /// <summary>
        /// Return a virtual sequence representing the reverse complement of this sequence.
        /// </summary>
        ISequence ReverseComplement { get; }

        /// <summary>
        /// Return a virtual sequence representing a range (substring) of this sequence.
        /// </summary>
        /// <param name="start">The index of the first symbol in the range.</param>
        /// <param name="length">The number of symbols in the range.</param>
        /// <returns>The virtual sequence.</returns>
        ISequence Range(int start, int length);

        /// <summary>
        /// Insert a single sequence item represented as a decodable character.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="character">The item to insert. Examples for DNA include: 'G' or 'C'</param>
        void Insert(int position, char character);

        /// <summary>
        /// Insert sequence item(s) represented as a string of decodable characters.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="sequence">The items to insert. Examples for DNA include: "G" or "GAAT"</param>
        void InsertRange(int position, string sequence);

        /// <summary>
        /// Remove a series of items from the sequence.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="length">The number of continuous items to remove starting at the position</param>
        void RemoveRange(int position, int length);

        /// <summary>
        /// Replaces the item at the indicated position with a single item from a decodable character.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="character">The item to insert. Examples from DNA include: 'G' or 'C'</param>
        void Replace(int position, char character);

        /// <summary>
        /// Replaces the item at the indicated position with a new item.
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="item">The item to place into the sequence</param>
        void Replace(int position, ISequenceItem item);

        /// <summary>
        /// Replaces the item at the indicated position with a string of decodable item(s).
        /// </summary>
        /// <param name="position">A zero-based index of the placement</param>
        /// <param name="sequence">The items to insert. Examples for DNA include: "G" or "GAAT"</param>
        void ReplaceRange(int position, string sequence);

        /// <summary>
        /// Gets the index of first non gap character.
        /// </summary>
        /// <returns>If found returns an zero based index of the first non gap character, otherwise returns -1.</returns>
        int IndexOfNonGap();

        /// <summary>
        /// Returns the position of the first item beyond startPos that does not 
        /// have a Gap character.
        /// </summary>
        /// <param name="startPos">Index value above which to search for non-Gap character.</param>
        /// <returns>If found returns an zero based index of the first non gap character, otherwise returns -1.</returns>
        int IndexOfNonGap(int startPos);

        /// <summary>
        /// Gets the index of last non gap character.
        /// </summary>
        /// <returns>If found returns an zero based index of the last non gap character, otherwise returns -1.</returns>
        int LastIndexOfNonGap();

        /// <summary>
        /// Gets the index of last non gap character before the specified end position.
        /// </summary>
        /// <param name="endPos">Index value below which to search for non-Gap character.</param>
        /// <returns>If found returns an zero based index of the last non gap character, otherwise returns -1.</returns>
        int LastIndexOfNonGap(int endPos);

        /// <summary>
        /// Creates a new ISequence that is a copy of the current ISequence.
        /// </summary>
        /// <returns>A new ISequence that is a copy of this ISequence.</returns>
        new ISequence Clone();
    }
}
