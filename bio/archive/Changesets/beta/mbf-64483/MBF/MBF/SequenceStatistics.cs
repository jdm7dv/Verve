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

namespace MBF
{
    /// <summary>
    /// SequenceStatistics is used to keep track of the number of occurrances of each symbol within
    /// a sequence.
    /// </summary>
    [Serializable]
    public class SequenceStatistics : ISerializable
    {
        #region Fields

        private IAlphabet _alphabet;
        private Dictionary<char, int> _countHash;
        private double _totalCount; // double so we don't need to cast when dividing

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs sequence statistics with alphabet and 0 counts.
        /// </summary>
        /// <param name="alphabet">The alphabet for the sequence.</param>
        internal SequenceStatistics(IAlphabet alphabet)
        {
            _alphabet = alphabet;

            _countHash = new Dictionary<char, int>();
            _totalCount = 0;
        }

        /// <summary>
        /// Constructs sequence statistics from the alphabet and symbol counts for a sequence.
        /// </summary>
        /// <param name="alphabet">The alphabet for the sequence.</param>
        /// <param name="symbolCounts">An array of length 256 containing the counts of each symbol
        /// indexed by that symbol's char representation.</param>
        internal SequenceStatistics(IAlphabet alphabet, int[] symbolCounts)
        {
            _alphabet = alphabet;

            LoadFromIntArray(symbolCounts);
        }

        /// <summary>
        /// Constructs sequence statistics by iterating through a sequence.
        /// </summary>
        /// <param name="sequence">The sequence to construct statistics for.</param>
        internal SequenceStatistics(ISequence sequence)
        {
            _alphabet = sequence.Alphabet;

            // Counting with an array is way faster than using a dictionary.
            int[] symbolCounts = new int[256];
            foreach (ISequenceItem item in sequence)
            {
                if (item == null)
                {
                    continue;
                }

                symbolCounts[item.Symbol]++;
            }

            LoadFromIntArray(symbolCounts);
        }

        /// <summary>
        /// Constructs sequence statistics by iterating through a list of sequences.
        /// </summary>
        /// <param name="sequences">The list of sequences to construct statistics for.</param>
        internal SequenceStatistics(IList<ISequence> sequences)
        {
            _alphabet = sequences[0].Alphabet;

            _countHash = new Dictionary<char, int>();
            _totalCount = 0;

            foreach (ISequence seq in sequences)
            {
                SequenceStatistics seqStats = seq.Statistics;

                if (seqStats._alphabet != _alphabet)
                {
                    throw new Exception("Cannot create statistics for list of sequences with different alphabets.");
                }

                foreach (char symbol in seqStats._countHash.Keys)
                {
                    if (_countHash.ContainsKey(symbol))
                    {
                        _countHash[symbol] += seqStats._countHash[symbol];
                    }
                    else
                    {
                        _countHash.Add(symbol, seqStats._countHash[symbol]);
                    }
                }

                _totalCount += seqStats._totalCount;
            }
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="that">The sequence statistics to copy from.</param>
        internal SequenceStatistics(SequenceStatistics that)
        {
            _alphabet = that._alphabet;

            _countHash = new Dictionary<char, int>();
            foreach (char symbol in that._countHash.Keys)
            {
                _countHash.Add(symbol, that._countHash[symbol]);
            }

            _totalCount = that._totalCount;
        }

        private void LoadFromIntArray(int[] symbolCounts)
        {
            if (symbolCounts.Length != 256)
            {
                throw new Exception("Array of symbol counts should have length of 256.");
            }

            _countHash = new Dictionary<char, int>();
            _totalCount = 0;

            for (int i = 0; i < symbolCounts.Length; i++)
            {
                if (symbolCounts[i] > 0)
                {
                    _countHash.Add((char)i, symbolCounts[i]);

                    _totalCount += symbolCounts[i];
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the number of occurences of a specific symbol.  This method does not perform
        /// any calculations to group counts of ambiguous symbols with corresponding unambiguous
        /// symbols.  So the minimum G-C content of a DNA sequence would be calculated as
        /// 
        ///     GetCount('G') + GetCount('C') + GetCount('S')
        /// </summary>
        /// <param name="symbol">The char representation of a symbol.</param>
        /// <returns>The number of occurences of the given symbol.</returns>
        public int GetCount(char symbol)
        {
            symbol = char.ToUpper(symbol);

            if (_countHash.ContainsKey(symbol))
            {
                return _countHash[symbol];
            }

            return 0;
        }

        /// <summary>
        /// Gets the number of occurences of the specific sequence item.  This method does not perform
        /// any calculations to group counts of ambiguous symbols with corresponding unambiguous
        /// symbols.  So the minimum G-C content of a DNA sequence would be calculated as
        /// 
        ///     GetCount('G') + GetCount('C') + GetCount('S')
        /// </summary>
        /// <param name="item">A sequence item.</param>
        /// <returns>The number of occurences of the given sequence item.</returns>
        public int GetCount(ISequenceItem item)
        {
            return GetCount(item.Symbol);
        }

        /// <summary>
        /// Gets the fraction of occurences of a specific symbol.  This method does not perform
        /// any calculations to group counts of ambiguous symbols with corresponding unambiguous
        /// symbols.  So the minimum G-C content of a DNA sequence would be calculated as
        /// 
        ///     GetFraction('G') + GetFraction('C') + GetFraction('S')
        /// </summary>
        /// <param name="symbol">The char representation of a symbol.</param>
        /// <returns>The fraction of occurences of the given symbol.</returns>
        public double GetFraction(char symbol)
        {
            return GetCount(symbol) / _totalCount;
        }

        /// <summary>
        /// Gets the fraction of occurences of a specific sequence item.  This method does not perform
        /// any calculations to group counts of ambiguous symbols with corresponding unambiguous
        /// symbols.  So the minimum G-C content of a DNA sequence would be calculated as
        /// 
        ///     GetFraction('G') + GetFraction('C') + GetFraction('S')
        /// </summary>
        /// <param name="item">A sequence item.</param>
        /// <returns>The fraction of occurences of the given sequence item.</returns>
        public double GetFraction(ISequenceItem item)
        {
            return GetCount(item) / _totalCount;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Adds the counts of the symbols in the given string to the existing counts.
        /// </summary>
        /// <param name="sequence">The symbols to add.</param>
        internal void Add(string sequence)
        {
            foreach (char symbol in sequence)
            {
                Add(symbol);
            }
        }

        /// <summary>
        /// Increments the count of the given symbol.
        /// </summary>
        /// <param name="symbol">The symbol to add.</param>
        internal void Add(char symbol)
        {
            symbol = char.ToUpper(symbol);

            if (_countHash.ContainsKey(symbol))
            {
                _countHash[symbol]++;
            }
            else
            {
                _countHash.Add(symbol, 1);
            }

            _totalCount++;
        }

        /// <summary>
        /// Increments the count of the given item.
        /// </summary>
        /// <param name="item">The item to add.</param>
        internal void Add(ISequenceItem item)
        {
            Add(item.Symbol);
        }

        /// <summary>
        /// Decrements the count of the given item.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        internal void Remove(ISequenceItem item)
        {
            _countHash[item.Symbol]--;
            _totalCount--;
        }

        /// <summary>
        /// Removes all counts from the SequenceStatistics.
        /// </summary>
        internal void Clear()
        {
            _countHash.Clear();
            _totalCount = 0;
        }

        /// <summary>
        /// The SequenceStatistics for the complement of the sequence that this was created from.
        /// </summary>
        internal SequenceStatistics Complement
        {
            get
            {
                SequenceStatistics complementStats = new SequenceStatistics(_alphabet);

                foreach (char symbol in _countHash.Keys)
                {
                    Nucleotide item = _alphabet.LookupBySymbol(symbol) as Nucleotide;

                    Nucleotide itemComplement;
                    if (_alphabet == Alphabets.DNA)
                    {
                        itemComplement = Complementation.GetDnaComplement(item);
                    }
                    else
                    {
                        itemComplement = Complementation.GetRnaComplement(item);
                    }

                    complementStats._countHash[itemComplement.Symbol] = _countHash[symbol];
                }

                complementStats._totalCount = _totalCount;

                return complementStats;
            }
        }

        /// <summary>
        /// Creates a new SequenceStatistics that is a copy of the current SequenceStatistics.
        /// </summary>
        /// <returns>A new SequenceStatistics that is a copy of this.</returns>
        internal SequenceStatistics Clone()
        {
            return new SequenceStatistics(this);
        }

        #endregion

        #region ISerializable Members
        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected SequenceStatistics(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _alphabet = Alphabets.All.Single(A => A.Name.Equals(info.GetString("AN")));
            if (info.GetBoolean("CHP"))
            {
                _countHash = (Dictionary<char, int>)info.GetValue("CH", typeof(Dictionary<char, int>));
            }
            else
            {
                _countHash = new Dictionary<char, int>();
            }

            _totalCount = info.GetDouble("TC");
        }

        /// <summary>
        /// Method for serializing the SequenceStatistics.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("AN", _alphabet.Name);
            if (_countHash.Count > 0)
            {
                info.AddValue("CHP",true);
                info.AddValue("CH", _countHash);
            }
            else
            {
                info.AddValue("CHP", false);
            }
            
            info.AddValue("TC", _totalCount);
        }

        #endregion
    }
}
