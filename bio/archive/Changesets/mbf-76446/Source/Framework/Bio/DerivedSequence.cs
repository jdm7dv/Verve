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

namespace Bio
{
    /// <summary>
    /// This is a temporary implementation of DerivedSequence to support reversing and complementing a sequence.
    /// </summary>
    public sealed class DerivedSequence : ISequence
    {
        // holds information about if this sequence is reversed or complemented or both
        private bool isReversed;
        private bool isComplemented;
        private string id;

        // holds the base sequence with which this sequence is derived
        private ISequence baseSequence;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DerivedSequence class.
        /// </summary>
        /// <param name="sequence">Base sequence to use.</param>
        /// <param name="reverseSequence">Flag to indicate if the derived sequence should be reversed.</param>
        /// <param name="complementSequence">Flag to indicate if the derived sequence should be complemented.</param>
        public DerivedSequence(ISequence sequence, bool reverseSequence, bool complementSequence)
        {
            this.baseSequence = sequence;
            this.isReversed = reverseSequence;
            this.isComplemented = complementSequence;
        }

        #endregion Constructors

        #region Properties

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
            get { return this.baseSequence.Metadata; }
        }

        /// <summary>
        /// Gets or sets an identifier for this instance of sequence class.
        /// </summary>
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    return this.baseSequence.ID;
                }

                return id;
            }

            set
            {
                id = value;
            }
        }

        /// <summary>
        /// Gets the number of bytes contained in the Sequence.
        /// </summary>
        public long Count
        {
            get
            {
                return this.baseSequence.Count;
            }
        }

        /// <summary>
        /// Gets the alphabet to which symbols in this sequence belongs to.
        /// </summary>
        public IAlphabet Alphabet
        {
            get
            {
                return this.baseSequence.Alphabet;
            }
        }
        #endregion Properties

        /// <summary>
        /// Returns the byte found at the specified index if within bounds. Note 
        /// that the index value starts at 0.
        /// </summary>
        /// <param name="index">Index at which the symbol is required.</param>
        /// <returns></returns>
        public byte this[long index]
        {
            get
            {
                byte returnValue;

                if (this.isReversed)
                {
                    returnValue = this.baseSequence[(this.baseSequence.Count - 1) - index];
                }
                else
                {
                    returnValue = this.baseSequence[index];
                }

                if (this.isComplemented && returnValue != 0 && !this.baseSequence.Alphabet.TryGetComplementSymbol(returnValue, out returnValue))
                {
                    throw new InvalidOperationException(Properties.Resource.ComplementNotFound);
                }

                return returnValue;
            }
        }

        #region Methods

        /// <summary>
        /// Return a new sequence representing this sequence with the orientation reversed.
        /// </summary>
        public ISequence GetReversedSequence()
        {
            return new DerivedSequence(this, true, false);
        }

        /// <summary>
        /// Return a new sequence representing the complement of this sequence.
        /// </summary>
        public ISequence GetComplementedSequence()
        {
            return new DerivedSequence(this, false, true);
        }

        /// <summary>
        /// Return a new sequence representing the reverse complement of this sequence.
        /// </summary>
        public ISequence GetReverseComplementedSequence()
        {
            return new DerivedSequence(this, true, true);
        }

        /// <summary>
        /// Return a new sequence representing a range (subsequence) of this sequence.
        /// </summary>
        /// <param name="start">The index of the first symbol in the range.</param>
        /// <param name="length">The number of symbols in the range.</param>
        /// <returns>The sub-sequence.</returns>
        public ISequence GetSubSequence(long start, long length)
        {
            byte[] subSequence = new byte[length];
            for (long index = 0; index < length; index++)
            {
                subSequence[index] = this[start + index];
            }

            return new Sequence(this.Alphabet, subSequence);
        }

        /// <summary>
        /// Gets the index of first non-gap symbol.
        /// </summary>
        /// <returns>If found returns a zero based index of the first non-gap symbol, otherwise returns -1.</returns>
        public long IndexOfNonGap()
        {
            // TODO: implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the position of the first symbol beyond startPos that does not 
        /// have a Gap symbol.
        /// </summary>
        /// <param name="startPos">Index value beyond which the non-gap symbol is searched for.</param>
        /// <returns>If found returns a zero based index of the first non-gap symbol, otherwise returns -1.</returns>
        public long IndexOfNonGap(long startPos)
        {
            // TODO: implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the index of last non-gap symbol.
        /// </summary>
        /// <returns>If found returns a zero based index of the last non-gap symbol, otherwise returns -1.</returns>
        public long LastIndexOfNonGap()
        {
            // TODO: implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the index of last non-gap symbol before the specified end position.
        /// </summary>
        /// <param name="endPos">Index value up to which the non-Gap symbol is searched for.</param>
        /// <returns>If found returns a zero based index of the last non-gap symbol, otherwise returns -1.</returns>
        public long LastIndexOfNonGap(long endPos)
        {
            // TODO: implement this method
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets an enumerator to the bytes present in this sequence.
        /// </summary>
        /// <returns>An IEnumerator of bytes.</returns>
        public IEnumerator<byte> GetEnumerator()
        {
            for (long i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }

        /// <summary>
        /// Gets an enumerator to the bytes present in this sequence.
        /// </summary>
        /// <returns>An IEnumerator of bytes.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }
}
