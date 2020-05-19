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

namespace MBF.Encoding
{
    /// <summary>
    /// A standard encoding for nucleic acids that allows for compression of DNA and RNA
    /// sequences. This encoding allows for the common ambiguities found when sequencing
    /// DNA and RNA and also allows for gap characters
    /// 
    /// The encoding comes from the NCBIA4na standard and is summarized by:
    /// 
    /// Value - Symbol - Name
    /// 
    /// 0  - - - Gap 
    /// 1  - A - Adenine
    /// 2  - C - Cytosine
    /// 3  - M - A or C
    /// 4  - G - Guanine
    /// 5  - R - G or A
    /// 6  - S - G or C
    /// 7  - V - G or C or A
    /// 8  - T - Thymine/Uracil
    /// 9  - W - A or T
    /// 10 - Y - T or C
    /// 11 - H - A or C or T
    /// 12 - K - G or T
    /// 13 - D - G or A or T
    /// 14 - B - G or T or C
    /// 15 - N - A or G or C or T
    /// 
    /// Notice that this encoding allows for bitwise comparison on 4 bit numbers
    /// to determine the makeup of the value. The leftmost bit for instance when
    /// set to 1 indicates that Thymine/Uracil is a possible value. The rightmost
    /// bit does the same for Adenine.
    /// </summary>
    public class Ncbi4NAEncoding : IEncoding
    {
        #region Nucleotide Definitions
        /// <summary>
        /// A gap character
        /// </summary>
        public readonly Nucleotide Gap = new Nucleotide(0, '-', "Gap", true, false);

        /// <summary>
        /// Adenine
        /// </summary>
        public readonly Nucleotide A = new Nucleotide(1, 'A', "Adenine");

        /// <summary>
        /// Cytosine
        /// </summary>
        public readonly Nucleotide C = new Nucleotide(2, 'C', "Cytosine");

        /// <summary>
        /// Adenine or Cytosine
        /// </summary>
        public readonly Nucleotide M = new Nucleotide(3, 'M', "A or C", false, true);

        /// <summary>
        /// Guanine
        /// </summary>
        public readonly Nucleotide G = new Nucleotide(4, 'G', "Guanine");

        /// <summary>
        /// Guanine or Adenine
        /// </summary>
        public readonly Nucleotide R = new Nucleotide(5, 'R', "G or A", false, true);

        /// <summary>
        /// Guanine or Cytosine
        /// </summary>
        public readonly Nucleotide S = new Nucleotide(6, 'S', "G or C", false, true);

        /// <summary>
        /// Gaunine, Cytosine, or Adenine
        /// </summary>
        public readonly Nucleotide V = new Nucleotide(7, 'V', "G or C or A", false, true);

        /// <summary>
        /// Thymine
        /// </summary>
        public readonly Nucleotide T = new Nucleotide(8, 'T', "Thymine/Uracil");

        /// <summary>
        /// Adenine or Thymine
        /// </summary>
        public readonly Nucleotide W = new Nucleotide(9, 'W', "A or T", false, true);

        /// <summary>
        /// Thymine or Cytosine
        /// </summary>
        public readonly Nucleotide Y = new Nucleotide(10, 'Y', "T or C", false, true);

        /// <summary>
        /// Adenine, Cytosine, or Thymine
        /// </summary>
        public readonly Nucleotide H = new Nucleotide(11, 'H', "A or C or T", false, true);

        /// <summary>
        /// Gaunine or Thymine
        /// </summary>
        public readonly Nucleotide K = new Nucleotide(12, 'K', "G or T", false, true);

        /// <summary>
        /// Gaunine, Adenine, or Thymine
        /// </summary>
        public readonly Nucleotide D = new Nucleotide(13, 'D', "G or A or T", false, true);

        /// <summary>
        /// Gaunine, Thymine, or Cytosine
        /// </summary>
        public readonly Nucleotide B = new Nucleotide(14, 'B', "G or T or C", false, true);

        /// <summary>
        /// Adenine, Guanine, Cytosine, or Thymine
        /// </summary>
        public readonly Nucleotide N = new Nucleotide(15, 'N', "A or G or C or T", false, true);
        #endregion

        private static Ncbi4NAEncoding instance;
        private static string name = "NCBI4na";

        /// <summary>
        /// An instance of the DNA alphabet for nucleic acids. Since the
        /// data does not change, use this static member instead of constructing
        /// a new one.
        /// </summary>
        public static Ncbi4NAEncoding Instance
        {
            get { return instance; }
        }

        private List<Nucleotide> values = new List<Nucleotide>();

        // set up the static instance
        static Ncbi4NAEncoding()
        {
            instance = new Ncbi4NAEncoding();
        }

        private Ncbi4NAEncoding()
        {
            values.Add(Gap);
            values.Add(A);
            values.Add(C);
            values.Add(M);
            values.Add(G);
            values.Add(R);
            values.Add(S);
            values.Add(V);
            values.Add(T);
            values.Add(W);
            values.Add(Y);
            values.Add(H);
            values.Add(K);
            values.Add(D);
            values.Add(B);
            values.Add(N);
        }

        #region IEncoding Members

        /// <summary>
        /// The name of this encoding is always 'NCBI4na'
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// This alphabet does not have termination characters.
        /// </summary>
        public bool HasTerminations
        {
            get { return false; }
        }

        /// <summary>
        /// This alphabet does have ambiguous characters.
        /// </summary>
        public bool HasAmbiguity
        {
            get { return true; }
        }

        /// <summary>
        /// This alphabet does have a gap character.
        /// </summary>
        public bool HasGaps
        {
            get { return true; }
        }

        /// <summary>
        /// Retrieves the nucleotide associated with a particular byte value. See the comment for
        /// the class description to view the encoding table.
        /// </summary>
        public ISequenceItem LookupByValue(byte value)
        {
            if (value > 15)
                return null;

            return values[value];
        }

        /// <summary>
        /// Retrieves the nucleotide associated with a particular charcter symbol. See the comment for
        /// the class description to view the encoding table.
        /// </summary>
        public ISequenceItem LookupBySymbol(char symbol)
        {
            // Using switch statement for performance
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
                case '-':
                    return Gap;
                case 'N':
                case 'n':
                    return N;
                case 'R':
                case 'r':
                    return R;
                case 'M':
                case 'm':
                    return M;
                case 'S':
                case 's':
                    return S;
                case 'V':
                case 'v':
                    return V;
                case 'W':
                case 'w':
                    return W;
                case 'Y':
                case 'y':
                    return Y;
                case 'H':
                case 'h':
                    return H;
                case 'K':
                case 'k':
                    return K;
                case 'D':
                case 'd':
                    return D;
                case 'B':
                case 'b':
                    return B;
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

        /// <summary>
        /// Gets the complement of a given byte value
        /// </summary>
        /// <param name="value">Value of which complement has to be found</param>
        /// <returns>Complemented byte value</returns>
        public byte GetComplement(byte value)
        {
            switch (value)
            {
                case 0:
                    return 0;
                case 1:
                    return 8;
                case 2:
                    return 4;
                case 3:
                    return 12;
                case 4:
                    return 2;
                case 5:
                    return 10;
                case 6:
                    return 6;
                case 7:
                    return 14;
                case 8:
                    return 1;
                case 9:
                    return 9;
                case 10:
                    return 5;
                case 11:
                    return 13;
                case 12:
                    return 3;
                case 13:
                    return 11;
                case 14:
                    return 7;
                case 15:
                    return 15;
            }

            throw new ArgumentException(Properties.Resource.InvalidByteValue);
        }

        /// <summary>
        /// Encodes the source sequence onto a byte array. The array will be the
        /// size of the source when returned.
        /// </summary>
        /// <param name="source">The data to be encoded (eg. "TAGGC")</param>
        /// <returns>The array into which the encoded values will be placed</returns>
        public byte[] Encode(string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            
            byte[] result = new byte[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                result[i] = this.LookupBySymbol(source[i]).Value;
            }

            return result;
        }
        #endregion

        #region ICollection<ISequenceItem> Members
        /// <summary>
        /// The number of encoding symbols. For this encoding the result should
        /// always be 16.
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
            if (array == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameArray);
            }

            if (arrayIndex < 0 || (arrayIndex + Count) > array.Length)
            {
                throw new ArgumentException(Properties.Resource.DestArrayNotLargeEnoughError);
            }

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
