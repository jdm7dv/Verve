// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace Bio
{
    /// <summary>
    /// This is the standard implementation of the ISequence interface. It contains
    /// the raw data that defines the contents of a sequence. Since Sequence uses 
    /// enumerable of bytes that can be accessed as follows:
    /// Sequence mySequence = new Sequence(Alphabets.DNA, "GATTC");
    /// foreach (Nucleotide nucleotide in mySequence) { ... }
    /// The results will be based on the Alphabet associated with the
    /// sequence. Common alphabets include those for DNA, RNA, and Amino Acids.
    /// For users who wish to get at the underlying data directly, Sequence provides
    /// a means to do this as well. This may be useful for those writing algorithms
    /// against the sequence where performance is especially important. For these
    /// advanced users access is provided to the encoding classes associated with the
    /// sequence.
    /// </summary>
    public sealed class Sequence : ISequence
    {
        #region Member variables

        /// <summary>
        /// Holds the sequence data.
        /// Used when the data size is small.
        /// </summary>
        private byte[] sequenceData = null;

        /// <summary>
        /// Metadata is features or references or related things of a sequence.
        /// </summary>
        private Dictionary<string, object> metadata;

        #endregion Member variables

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the Sequence class with specified alphabet and string sequence.
        /// Symbols in the sequence are validated with the specified alphabet.
        /// </summary>
        /// <param name="alphabet">Alphabet to which this class should conform.</param>
        /// <param name="sequence">The sequence in string form.</param>
        public Sequence(IAlphabet alphabet, string sequence)
            : this(alphabet, sequence, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Sequence class with specified alphabet and string sequence.
        /// </summary>
        /// <param name="alphabet">Alphabet to which this class should conform.</param>
        /// <param name="sequence">The sequence in string form.</param>
        /// <param name="validate">If this flag is true then validation will be done to see whether the data is valid or not,
        /// else validation will be skipped.</param>
        public Sequence(IAlphabet alphabet, string sequence, bool validate)
        {
            // validate the inputs
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            if (alphabet == null)
            {
                throw new ArgumentNullException("alphabet");
            }

            this.Alphabet = alphabet;
            this.ID = string.Empty;
            byte[] values = ASCIIEncoding.ASCII.GetBytes(sequence);

            if (validate)
            {
                // Validate sequence data
                if (!alphabet.ValidateSequence(values, 0, values.LongLength))
                {
                    throw new ArgumentOutOfRangeException("sequence");
                }
            }

            this.sequenceData = values;
            this.Count = this.sequenceData.LongLength;
        }

        /// <summary>
        /// Initializes a new instance of the Sequence class with specified alphabet and bytes.
        /// Bytes representing Symbols in the values are validated with the specified alphabet.
        /// </summary>
        /// <param name="alphabet">Alphabet to which this instance should conform.</param>
        /// <param name="values">An array of bytes representing the symbols.</param>
        public Sequence(IAlphabet alphabet, byte[] values)
            : this(alphabet, values, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Sequence class with specified alphabet and bytes.
        /// </summary>
        /// <param name="alphabet">Alphabet to which this instance should conform.</param>
        /// <param name="values">An array of bytes representing the symbols.</param>
        /// <param name="validate">If this flag is true then validation will be done to see whether the data is valid or not,
        /// else validation will be skipped.</param>
        public Sequence(IAlphabet alphabet, byte[] values, bool validate)
        {
            // validate the inputs
            if (alphabet == null)
            {
                throw new ArgumentNullException("alphabet");
            }

            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (validate)
            {
                // Validate sequence data
                if (!alphabet.ValidateSequence(values, 0, values.LongLength))
                {
                    throw new ArgumentOutOfRangeException("values");
                }
            }

            this.sequenceData = new byte[values.LongLength];
            this.ID = string.Empty;
            Array.Copy(values, this.sequenceData, values.LongLength);

            this.Alphabet = alphabet;
            this.Count = this.sequenceData.LongLength;
        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// Gets or sets an identifier for this instance of sequence class.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets the number of bytes contained in the Sequence.
        /// </summary>
        public long Count { get; private set; }

        /// <summary>
        /// Gets or sets the alphabet to which symbols in this sequence belongs to.
        /// </summary>
        public IAlphabet Alphabet { get; set; }

        /// <summary>
        /// Gets or sets the Metadata of this instance.
        /// Many sequence representations when saved to file also contain
        /// information about that sequence. Unfortunately there is no standard
        /// around what that data may be from format to format. This property
        /// allows a place to put structured metadata that can be accessed by
        /// a particular key.
        /// <para>
        /// For example, if species information is stored in a particular Species
        /// class, you could add it to the dictionary by:
        /// </para>
        /// <para>
        /// mySequence.Metadata["SpeciesInfo"] = mySpeciesInfo;
        /// </para>
        /// <para>
        /// To fetch the data you would use:
        /// </para>
        /// <para>
        /// Species mySpeciesInfo = mySequence.Metadata["SpeciesInfo"];
        /// </para>
        /// <para>
        /// Particular formats may create their own data model class for information
        /// unique to their format as well. Such as:
        /// </para>
        /// <para>
        /// GenBankMetadata genBankData = new GenBankMetadata();
        /// // ... add population code
        /// mySequence.MetaData["GenBank"] = genBankData;.
        /// </para>
        /// </summary>
        public Dictionary<string, object> Metadata
        {
            get
            {
                if (this.metadata == null)
                {
                    this.metadata = new Dictionary<string, object>();
                }

                return this.metadata;
            }

            set
            {
                this.metadata = value;
            }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Returns the byte found at the specified index if within bounds. Note 
        /// that the index value starts at 0.
        /// </summary>
        /// <param name="index">Index at which the symbol is required.</param>
        /// <returns>Byte value at the given index.</returns>
        public byte this[long index]
        {
            get
            {
                return this.sequenceData[index];
            }
        }

        /// <summary>
        /// Return a new sequence representing this sequence with the orientation reversed.
        /// </summary>
        public ISequence GetReversedSequence()
        {
            byte[] values = new byte[this.Count];
            Array.Copy(this.sequenceData, values, this.sequenceData.LongLength);
            Array.Reverse(values);
            Sequence seq = new Sequence(this.Alphabet, values, false);
            seq.ID = this.ID;
            seq.metadata = this.metadata;

            return seq;
        }

        /// <summary>
        /// Return a new sequence representing the complement of this sequence.
        /// </summary>
        public ISequence GetComplementedSequence()
        {
            if (!this.Alphabet.IsComplementSupported)
            {
                throw new InvalidOperationException(Properties.Resource.ComplementNotFound);
            }

            byte[] complemented = new byte[this.Count];
            this.Alphabet.TryGetComplementSymbol(this.sequenceData, out complemented);
            Sequence seq = new Sequence(this.Alphabet, complemented, false);
            seq.ID = this.ID;
            seq.metadata = this.metadata;

            return seq;
        }

        /// <summary>
        /// Return a new sequence representing the reverse complement of this sequence.
        /// </summary>
        public ISequence GetReverseComplementedSequence()
        {
            byte[] reverseComplemented = new byte[this.Count];
            this.Alphabet.TryGetComplementSymbol(this.sequenceData, out reverseComplemented);
            Array.Reverse(reverseComplemented);
            Sequence seq = new Sequence(this.Alphabet, reverseComplemented, false);
            seq.ID = this.ID;
            seq.metadata = this.metadata;

            return seq;
        }

        /// <summary>
        /// Return a new sequence representing a range (subsequence) of this sequence.
        /// </summary>
        /// <param name="start">The index of the first symbol in the range.</param>
        /// <param name="length">The number of symbols in the range.</param>
        /// <returns>The sub-sequence.</returns>
        public ISequence GetSubSequence(long start, long length)
        {
            if (start >= this.Count)
            {
                throw new ArgumentOutOfRangeException("start");
            }

            if (start + length > this.Count)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            byte[] subSequence = new byte[length];
            for (long index = 0; index < length; index++)
            {
                subSequence[index] = this.sequenceData[start + index];
            }

            Sequence seq = new Sequence(this.Alphabet, subSequence, false);
            seq.ID = this.ID;
            seq.metadata = this.metadata;

            return seq;
        }

        /// <summary>
        /// Gets the index of first non-gap symbol.
        /// </summary>
        /// <returns>If found returns a zero based index of the first non-gap symbol, otherwise returns -1.</returns>
        public long IndexOfNonGap()
        {
            return this.IndexOfNonGap(0);
        }

        /// <summary>
        /// Returns the position of the first symbol beyond startPos that does not 
        /// have a Gap symbol.
        /// </summary>
        /// <param name="startPos">Index value beyond which the non-gap symbol is searched for.</param>
        /// <returns>If found returns a zero based index of the first non-gap symbol, otherwise returns -1.</returns>
        public long IndexOfNonGap(long startPos)
        {
            if (startPos >= this.sequenceData.LongLength)
            {
                throw new ArgumentOutOfRangeException("startPos");
            }

            HashSet<byte> gapSymbols;
            if (!this.Alphabet.TryGetGapSymbols(out gapSymbols))
            {
                return startPos;
            }

            byte[] aliasSymbolsMap = this.Alphabet.GetSymbolValueMap();

            for (long index = startPos; index < this.Count; index++)
            {
                byte symbol = aliasSymbolsMap[this.sequenceData[index]];
                if (!gapSymbols.Contains(symbol))
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the index of last non-gap symbol.
        /// </summary>
        /// <returns>If found returns a zero based index of the last non-gap symbol, otherwise returns -1.</returns>
        public long LastIndexOfNonGap()
        {
            return this.LastIndexOfNonGap(this.Count - 1);
        }

        /// <summary>
        /// Returns the index of last non-gap symbol before the specified end position.
        /// </summary>
        /// <param name="endPos">Index value up to which the non-Gap symbol is searched for.</param>
        /// <returns>If found returns a zero based index of the last non-gap symbol, otherwise returns -1.</returns>
        public long LastIndexOfNonGap(long endPos)
        {
            HashSet<byte> gapSymbols;

            if (!this.Alphabet.TryGetGapSymbols(out gapSymbols))
            {
                return endPos;
            }

            byte[] aliasSymbolsMap = this.Alphabet.GetSymbolValueMap();
            for (long index = endPos; index >= 0; index--)
            {
                byte symbol = aliasSymbolsMap[this.sequenceData[index]];
                if (!gapSymbols.Contains(symbol))
                {
                    return index;
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets an enumerator to the bytes present in this sequence.
        /// </summary>
        /// <returns>An IEnumerator of bytes.</returns>
        public IEnumerator<byte> GetEnumerator()
        {
            for (long index = 0; index < this.Count; index++)
            {
                yield return this.sequenceData[index];
            }
        }

        /// <summary>
        /// Gets an enumerator to the bytes present in this sequence.
        /// </summary>
        /// <returns>An IEnumerator of bytes.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.sequenceData.GetEnumerator();
        }
        #endregion
    }
}
