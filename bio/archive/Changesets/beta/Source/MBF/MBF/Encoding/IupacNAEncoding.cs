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
    /// A standard encoding for nucleic acids of DNA and RNA sequences. This encoding
    /// allows for the common ambiguities found when sequencing DNA and RNA but does
    /// not allow for gap characters.
    /// 
    /// An encoding that contains all of these symbols plus a gap character is found
    /// in Ncbi4NaEncoding and will likely be a better choice for encoding unless
    /// working with code or formats that require this encoding.
    /// 
    /// The encoding comes from the IUPACna standard and is summarized by:
    /// 
    /// Value - Symbol - Name
    /// 
    /// 65 - A - Adenine
    /// 66 - B - G or T or C
    /// 67 - C - Cytosine
    /// 68 - D - G or A or T
    /// 71 - G - Guanine
    /// 72 - H - A or C or T
    /// 75 - K - G or T
    /// 77 - M - A or C
    /// 78 - N - A or G or C or T
    /// 82 - R - G or A
    /// 83 - S - G or C
    /// 84 - T - Thymine/Uracil
    /// 86 - V - G or C or A
    /// 87 - W - A or T
    /// 89 - Y - T or C
    /// 
    /// Note that the reason for the byte values found here is that they represent
    /// the ASCII value for the symbol.
    /// </summary>
    public class IupacNAEncoding : IEncoding
    {
        #region Nucleotide Definitions
        /// <summary>
        /// Adenine
        /// </summary>
        public readonly Nucleotide A = new Nucleotide(65, 'A', "Adenine");

        /// <summary>
        /// Gaunine, Thymine, or Cytosine
        /// </summary>
        public readonly Nucleotide B = new Nucleotide(66, 'B', "G or T or C", false, true);

        /// <summary>
        /// Cytosine
        /// </summary>
        public readonly Nucleotide C = new Nucleotide(67, 'C', "Cytosine");

        /// <summary>
        /// Gaunine, Adenine, or Thymine
        /// </summary>
        public readonly Nucleotide D = new Nucleotide(68, 'D', "G or A or T", false, true);

        /// <summary>
        /// Guanine
        /// </summary>
        public readonly Nucleotide G = new Nucleotide(71, 'G', "Guanine");

        /// <summary>
        /// Adenine, Cytosine, or Thymine
        /// </summary>
        public readonly Nucleotide H = new Nucleotide(72, 'H', "A or C or T", false, true);

        /// <summary>
        /// Gaunine or Thymine
        /// </summary>
        public readonly Nucleotide K = new Nucleotide(75, 'K', "G or T", false, true);

        /// <summary>
        /// Adenine or Cytosine
        /// </summary>
        public readonly Nucleotide M = new Nucleotide(77, 'M', "A or C", false, true);

        /// <summary>
        /// Adenine, Guanine, Cytosine, or Thymine
        /// </summary>
        public readonly Nucleotide N = new Nucleotide(78, 'N', "A or G or C or T", false, true);

        /// <summary>
        /// Guanine or Adenine
        /// </summary>
        public readonly Nucleotide R = new Nucleotide(82, 'R', "G or A", false, true);

        /// <summary>
        /// Guanine or Cytosine
        /// </summary>
        public readonly Nucleotide S = new Nucleotide(83, 'S', "G or C", false, true);

        /// <summary>
        /// Thymine
        /// </summary>
        public readonly Nucleotide T = new Nucleotide(84, 'T', "Thymine");

        /// <summary>
        /// Gaunine, Cytosine, or Adenine
        /// </summary>
        public readonly Nucleotide V = new Nucleotide(86, 'V', "G or C or A", false, true);

        /// <summary>
        /// Adenine or Thymine
        /// </summary>
        public readonly Nucleotide W = new Nucleotide(87, 'W', "A or T", false, true);

        /// <summary>
        /// Thymine or Cytosine
        /// </summary>
        public readonly Nucleotide Y = new Nucleotide(89, 'Y', "T or C", false, true);
        #endregion

        private static IupacNAEncoding instance;
        private static string name = "IUPACna";

        /// <summary>
        /// An instance of the DNA alphabet for nucleic acids. Since the
        /// data does not change, use this static member instead of constructing
        /// a new one.
        /// </summary>
        public static IupacNAEncoding Instance
        {
            get { return instance; }
        }

        private List<Nucleotide> values = new List<Nucleotide>();

        // set up the static instance
        static IupacNAEncoding()
        {
            instance = new IupacNAEncoding();
        }

        private IupacNAEncoding()
        {
            values.Add(A);
            values.Add(B);
            values.Add(C);
            values.Add(D);
            values.Add(G);
            values.Add(H);
            values.Add(K);
            values.Add(M);
            values.Add(N);
            values.Add(R);
            values.Add(S);
            values.Add(T);
            values.Add(V);
            values.Add(W);
            values.Add(Y);
        }

        #region IEncoding Members

        /// <summary>
        /// The name of this encoding is always 'IUPACna'
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
            get { return false; }
        }

        /// <summary>
        /// Retrieves the nucleotide associated with a particular byte value. See the comment for
        /// the class description to view the encoding table.
        /// </summary>
        public ISequenceItem LookupByValue(byte value)
        {
            // values[] is a 0 based list.  This table maps the value to the index in the values[] list.

            // 0 65 - A - Adenine
            // 1 66 - B - G or T or C
            // 2 67 - C - Cytosine
            // 3 68 - D - G or A or T

            // 4 71 - G - Guanine
            // 5 72 - H - A or C or T

            // 6 75 - K - G or T

            // 7 77 - M - A or C
            // 8 78 - N - A or G or C or T

            // 9 82 - R - G or A
            //10 83 - S - G or C
            //11 84 - T - Thymine/Uracil

            //12 86 - V - G or V or A
            //13 87 - W - A or T

            //14 89 - Y - T or C

            switch (value)
            {
                case 65:
                case 66:
                case 67:
                case 68:
                    return values[value - 65];
                case 71:
                case 72:
                    return values[value - 67];
                case 75:
                    return values[6]; // or value-69
                case 77:
                case 78:
                    return values[value - 70];
                case 82:
                case 83:
                case 84:
                    return values[value - 73];
                case 86:
                case 87:
                    return values[value - 74];
                case 89:
                    return values[14]; // or value-75
                default:
                    return null;
            }
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
                case 65:
                    return 84;
                case 66:
                    return 86;
                case 67:
                    return 71;
                case 68:
                    return 72;
                case 71:
                    return 67;
                case 72:
                    return 68;
                case 75:
                    return 77;
                case 77:
                    return 75;
                case 78:
                    return 78;
                case 82:
                    return 89;
                case 83:
                    return 83;
                case 84:
                    return 65;
                case 86:
                    return 66;
                case 87:
                    return 87;
                case 89:
                    return 82;
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
        /// always be 15.
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
