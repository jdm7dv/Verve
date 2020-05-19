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
using System.Globalization;
using System.Linq;
using MBF.Encoding;

namespace MBF.SimilarityMatrices
{
    internal class BasicSmEncoding : IEncoding
    {
        // List<> works with value, hard to find value associated with a particular symbol.
        // Dictionary<> works with symbol, hard to find symbol associated with a particular value.
        // Do it twice, use a little extra space, avoid more expensive execution time searches.
        ISequenceItem[] _symbols;

        private Dictionary<char, ISequenceItem> _values = new Dictionary<char, ISequenceItem>();

        /// <summary>
        /// Sets up a basic encoding for use with similarity matrices.
        /// Because this encoding is only used to correlate the ordering of the similarity matrix
        /// with the sequence encoding, the HasGaps, HasAmbiguity and HasTerminations properties
        /// will not be used, and don't have to be specified.
        /// </summary>
        /// <param name="symbols">
        /// Symbols in the encoding, in order.  These will map to values using zero based indexing.
        /// The symbols string must contain only the symbols, no whitespace or other delimiters.
        /// The symbols string should be upper case -- if not, the symbols will be converted to
        /// upper case before creating the encoding.
        /// </param>
        /// <param name="name">Name of the encoding.</param>
        /// <param name="moleculeType">Type of molecule, must be DNA, RNA, NA or Protein</param>
        public BasicSmEncoding(string symbols, string name, MoleculeType moleculeType)
            : this(symbols, name, moleculeType, false, false, false)
        {
        }

        public BasicSmEncoding(string symbols, string name, MoleculeType moleculeType, bool hasGaps, bool hasAmbiguity, bool hasTerminations)
        {
            Name = name;
            HasGaps = hasGaps;
            HasAmbiguity = hasAmbiguity;
            HasTerminations = hasTerminations;
            
            // Load the symbols into items
            string trimmed = symbols.Trim().ToUpper(CultureInfo.InvariantCulture); // should be no leading or trailing whitespace, but why take chances?
            _symbols = new ISequenceItem[trimmed.Length];
            byte i = 0;  // index into mappings
            foreach (char c in trimmed)
            {
                if (moleculeType == MoleculeType.DNA || moleculeType == MoleculeType.RNA || moleculeType == MoleculeType.NA)
                {
                    Nucleotide item = new Nucleotide(i, c, c.ToString());
                    _symbols[i] = item;
                    _values.Add(c, item);
                }
                else if (moleculeType == MoleculeType.Protein)
                {
                    AminoAcid item = new AminoAcid(i, c, c.ToString());
                    _symbols[i] = item;
                    _values.Add(c, item);
                }
                i++;
            }
        }

        #region IEncoding Members

        public string Name { get; private set; }
        public bool HasGaps { get; private set; }
        public bool HasAmbiguity { get; private set; }
        public bool HasTerminations { get; private set; }

        public ISequenceItem LookupByValue(byte value)
        {
            if (value >= _values.Count)
                return null;

            return _symbols[value];
        }

        public ISequenceItem LookupBySymbol(char symbol)
        {
            return _values[symbol];
        }

        public ISequenceItem LookupBySymbol(string symbol)
        {
            return LookupBySymbol(symbol.Trim()[0]);
        }

        /// <summary>
        /// This method is not supported
        /// </summary>        
        public byte GetComplement(byte value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// This method is not supported
        /// </summary>
        public byte[] Encode(string source)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region ICollection<ISequenceItem> Members

        // Todo: Below methods which are implementations of ICollection is not having any body and throwing exceptions.
        //       This class inherits from IEncoding which implements ICollection
        //       We better implement IEnumerable in IEncoding and get rid of this dummy methods below.
        //       If we need to support any of these methods using the base class IEncoding, declare it there instead of implementing ICollection.
        public void Add(ISequenceItem item)
        {
            throw new InvalidOperationException("Read Only");
        }

        public void Clear()
        {
            throw new InvalidOperationException("Read Only");
        }

        /// <summary>
        /// Return true if item.symbol is in the encoding, otherwise false.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <remarks>
        /// Note that the Value or Name property, or other properties (even including object type)
        /// may be different.  This method is primarily useful to see if a symbol in a sequence
        /// is in the encoding in some form or other.
        /// </remarks>
        public bool Contains(ISequenceItem item)
        {
            if (_values.ContainsKey(item.Symbol))
            {
                return true;
            }
            return false;
        }

        public void CopyTo(ISequenceItem[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameArray);
            }

            if (arrayIndex < 0 || (arrayIndex + Count) > array.Length)
            {
                throw new ArgumentException(Properties.Resource.DestArrayNotLargeEnoughError);
            }

            foreach (ISequenceItem item in _symbols)
            {
                array[arrayIndex++] = item;
            }
        }

        public int Count
        {
            get { return _symbols.Count(); }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(ISequenceItem item)
        {
            throw new InvalidOperationException("Read Only");
        }

        #endregion

        #region IEnumerable<ISequenceItem> Members

        public IEnumerator<ISequenceItem> GetEnumerator()
        {
            List<ISequenceItem> list = new List<ISequenceItem>();
            foreach (ISequenceItem s in _symbols)
                list.Add(s);
            return list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _symbols.GetEnumerator();
        }

        #endregion
    }
}
