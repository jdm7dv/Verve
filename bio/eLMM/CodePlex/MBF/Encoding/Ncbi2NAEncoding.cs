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

namespace Bio.Encoding
{
    /// <summary>
    /// A very simple encoding that allows for great compression of DNA and RNA
    /// sequences. This encoding only allows for 4 nucleotides and thus has no
    /// room for gap, termination, or ambiguous characters. This is done so that
    /// the encodings can be done using only two bits of information. It is
    /// appropriate only where this extra information is not necessary.
    /// 
    /// The encoding comes from the NCBIA2na standard and is summarized by:
    /// 
    /// Value - Symbol - Name
    /// 
    /// 0 - A - Adenine
    /// 1 - C - Cytosine
    /// 2 - G - Guanine
    /// 3 - T - Thymine / Uracil
    /// </summary>
    public class Ncbi2NAEncoding : IEncoding
    {
        /// <summary>
        /// Adenine
        /// </summary>
        public readonly Nucleotide A = new Nucleotide(0, 'A', "Adenine");

        /// <summary>
        /// Cytosine
        /// </summary>
        public readonly Nucleotide C = new Nucleotide(1, 'C', "Cytosine");

        /// <summary>
        /// Guanine
        /// </summary>
        public readonly Nucleotide G = new Nucleotide(2, 'G', "Guanine");

        /// <summary>
        /// For DNA this symbol represents Thymine. For RNA it represents Uracil
        /// </summary>
        public readonly Nucleotide T = new Nucleotide(3, 'T', "Thymine/Uracil");

        private static Ncbi2NAEncoding instance;
        private static string name = "NCBI2na";

        /// <summary>
        /// An instance of the Ncbi2Na encoding for nucleic acids. Since the
        /// data does not change, use this static member instead of constructing
        /// a new one.
        /// </summary>
        public static Ncbi2NAEncoding Instance
        {
            get { return instance; }
        }

        private List<Nucleotide> values = new List<Nucleotide>();

        // set up the static instance
        static Ncbi2NAEncoding()
        {
            instance = new Ncbi2NAEncoding();
        }

        private Ncbi2NAEncoding()
        {
            values.Add(A);
            values.Add(C);
            values.Add(G);
            values.Add(T);
        }

        #region IEncoding Members

        /// <summary>
        /// The name of this encoding is always 'NCBI2na'
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// This encodings does not have termination characters.
        /// </summary>
        public bool HasTerminations
        {
            get { return false; }
        }

        /// <summary>
        /// This encodings does not have ambiguous characters.
        /// </summary>
        public bool HasAmbiguity
        {
            get { return false; }
        }

        /// <summary>
        /// This encodings does not have gap characters.
        /// </summary>
        public bool HasGaps
        {
            get { return false; }
        }

        /// <summary>
        /// Retrieves the nucleotide associated with a particular byte value. See the comment for
        /// the class description to view the encoding table.
        /// </summary>
        public ISequenceItem LookupByValue(byte value)
        {
            if (value > 3)
                throw new IndexOutOfRangeException("Value recieved was outside of the encoding range");

            return values[value];
        }

        /// <summary>
        /// Retrieves the nucleotide associated with a particular charcter symbol. See the comment for
        /// the class description to view the encoding table.
        /// </summary>
        public ISequenceItem LookupBySymbol(char symbol)
        {
            switch (symbol)
            {
                case 'A':
                case 'a':
                    return A;
                case 'C':
                case 'c':
                    return C;
                case 'G':
                case 'g':
                    return G;
                case 'T':
                case 't':
                case 'U':
                case 'u':
                    return T;
                default:
                    throw new ArgumentException("Could not recognize symbol: " + symbol);
            }
        }

        /// <summary>
        /// Retrieves the nucleotide associated with a particular string symbol.
        /// This method will throw an exception for a string with more than one
        /// character in it. See the comment for the class description to view the
        /// encoding table.
        /// </summary>
        public ISequenceItem LookupBySymbol(string symbol)
        {
            string trimmed = symbol.Trim();
            if (trimmed.Length != 1)
                throw new ArgumentException("Could not recognize symbol: " + trimmed);

            return LookupBySymbol(trimmed[0]);
        }

        #endregion

        #region ICollection<ISequenceItem> Members
        /// <summary>
        /// The number of encoded values. For this encoding the result should
        /// always be 4.
        /// </summary>
        public int Count
        {
            get { return values.Count; }
        }

        /// <summary>
        /// Always returns true.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// This is a read only collection and thus this method will throw an exception
        /// </summary>
        public void Add(ISequenceItem item)
        {
            throw new Exception("Read Only");
        }

        /// <summary>
        /// This is a read only collection and thus this method will throw an exception
        /// </summary>
        public void Clear()
        {
            throw new Exception("Read Only");
        }

        /// <summary>
        /// Indication of whether or not an ISequenceItem is in the encoding. This is
        /// a simple lookup and will only match exactly with items of this encoding. It
        /// will not compare items from other encodings that match the same nucleotide.
        /// </summary>
        public bool Contains(ISequenceItem item)
        {
            Nucleotide nucleo = item as Nucleotide;
            if (nucleo == null)
                return false;

            return values.Contains(nucleo);
        }

        /// <summary>
        /// Copies the nucleotides in this encoding into an array
        /// </summary>
        public void CopyTo(ISequenceItem[] array, int arrayIndex)
        {
            foreach (Nucleotide nucleo in values)
            {
                array[arrayIndex++] = nucleo;
            }
        }

        /// <summary>
        /// This is a read only collection and thus this method will throw an exception
        /// </summary>
        public bool Remove(ISequenceItem item)
        {
            throw new Exception("Read Only");
        }

        #endregion

        #region IEnumerable<ISequenceItem> Members

        /// <summary>
        /// Creates an IEnumerator of the nucleotides
        /// </summary>
        public IEnumerator<ISequenceItem> GetEnumerator()
        {
            List<ISequenceItem> siList = new List<ISequenceItem>();
            foreach (Nucleotide n in values)
                siList.Add(n);
            return siList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Creates an IEnumerator of the nucleotides
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }

        #endregion
    }
}
