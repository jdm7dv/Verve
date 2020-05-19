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
    /// A basic encoding that describes symbols used in sequences of amino
    /// acids that come from codon encodings of RNA. This encoding closely
    /// follows the ProteinAlphabet and is based on the standard NCBIeaa.
    /// This encoding also contains the three letter symbols defined in the
    /// IUPAC3aa standard.
    /// 
    /// This encoding assigns values based on the ASCII value of the one
    /// character symbol of the amino acid. For a more sequential byte value
    /// encodings see the NcbiStdAAEncoding.
    /// 
    /// The entries in this dictionary are:
    /// 
    /// Value - Symbol - Extended Symbol - Name
    /// 
    /// 42 - * - Ter - Termination
    /// 45  - - --- - Gap
    /// 65 - A - Ala - Alanine
    /// 66 - B - Asx - Aspartic Acid or Asparagine
    /// 67 - C - Cys - Cysteine
    /// 68 - D - Asp - Aspartic Acid
    /// 69 - E - Glu - Glutamic Acid
    /// 70 - F - Phe - Phenylalanine
    /// 71 - G - Gly - Glycine
    /// 72 - H - His - Histidine
    /// 73 - I - Ile - Isoleucine
    /// 74 - J - Xle - Leucine or Isoleucine
    /// 75 - K - Lys - Lysine
    /// 76 - L - Leu - Leucine
    /// 77 - M - Met - Methionine
    /// 78 - N - Asn - Asparagine
    /// 79 - O - Pyl - Pyrrolysine
    /// 80 - P - Pro - Proline
    /// 81 - Q - Gln - Glutamine
    /// 82 - R - Arg - Arginine
    /// 83 - S - Ser - Serine
    /// 84 - T - Thr - Threoine
    /// 85 - U - Sel - Selenocysteine
    /// 86 - V - Val - Valine
    /// 87 - W - Trp - Tryptophan
    /// 88 - X - Xxx - Undetermined or atypical
    /// 89 - Y - Tyr - Tyrosine
    /// 90 - Z - Glx - Glutamic Acid or Glutamine
    /// </summary>
    public class NcbiEAAEncoding : IEncoding
    {
        #region Amino Acid Definitions

        /// <summary>
        /// Termination character
        /// </summary>
        public readonly AminoAcid Term = new AminoAcid(42, '*', "Ter", "Termination");

        /// <summary>
        /// Gap character
        /// </summary>
        public readonly AminoAcid Gap = new AminoAcid(45, '-', "---", "Gap");

        /// <summary>
        /// Alanine
        /// </summary>
        public readonly AminoAcid Ala = new AminoAcid(65, 'A', "Ala", "Alanine");

        /// <summary>
        /// Aspartic Acid or Asparagine
        /// </summary>
        public readonly AminoAcid Asx = new AminoAcid(66, 'B', "Asx", "Aspartic Acid or Asparagine");

        /// <summary>
        /// Cysteine
        /// </summary>
        public readonly AminoAcid Cys = new AminoAcid(67, 'C', "Cys", "Cysteine");

        /// <summary>
        /// Aspartic Acid
        /// </summary>
        public readonly AminoAcid Asp = new AminoAcid(68, 'D', "Asp", "Aspartic Acid");

        /// <summary>
        /// Glutamic Acid
        /// </summary>
        public readonly AminoAcid Glu = new AminoAcid(69, 'E', "Glu", "Glutamic Acid");

        /// <summary>
        /// Phenylalanine
        /// </summary>
        public readonly AminoAcid Phe = new AminoAcid(70, 'F', "Phe", "Phenylalanine");

        /// <summary>
        /// Glycine
        /// </summary>
        public readonly AminoAcid Gly = new AminoAcid(71, 'G', "Gly", "Glycine");

        /// <summary>
        /// Histidine
        /// </summary>
        public readonly AminoAcid His = new AminoAcid(72, 'H', "His", "Histidine");

        /// <summary>
        /// Isoleucine
        /// </summary>
        public readonly AminoAcid Ile = new AminoAcid(73, 'I', "Ile", "Isoleucine");

        /// <summary>
        /// Leucine or Isoleucine
        /// </summary>
        public readonly AminoAcid Xle = new AminoAcid(74, 'J', "Xle", "Leucine or Isoleucine");

        /// <summary>
        /// Lysine
        /// </summary>
        public readonly AminoAcid Lys = new AminoAcid(75, 'K', "Lys", "Lysine");

        /// <summary>
        /// Leucine
        /// </summary>
        public readonly AminoAcid Leu = new AminoAcid(76, 'L', "Leu", "Leucine");

        /// <summary>
        /// Methionine
        /// </summary>
        public readonly AminoAcid Met = new AminoAcid(77, 'M', "Met", "Methionine");

        /// <summary>
        /// Asparagine
        /// </summary>
        public readonly AminoAcid Asn = new AminoAcid(78, 'N', "Asn", "Asparagine");

        /// <summary>
        /// Pyrrolysine
        /// </summary>
        public readonly AminoAcid Pyl = new AminoAcid(79, 'O', "Pyl", "Pyrrolysine");

        /// <summary>
        /// Proline
        /// </summary>
        public readonly AminoAcid Pro = new AminoAcid(80, 'P', "Pro", "Proline");

        /// <summary>
        /// Glutamine
        /// </summary>
        public readonly AminoAcid Gln = new AminoAcid(81, 'Q', "Gln", "Glutamine");

        /// <summary>
        /// Arginine
        /// </summary>
        public readonly AminoAcid Arg = new AminoAcid(82, 'R', "Arg", "Arginine");

        /// <summary>
        /// Serine
        /// </summary>
        public readonly AminoAcid Ser = new AminoAcid(83, 'S', "Ser", "Serine");

        /// <summary>
        /// Threoine
        /// </summary>
        public readonly AminoAcid Thr = new AminoAcid(84, 'T', "Thr", "Threoine");

        /// <summary>
        /// Selenocysteine
        /// </summary>
        public readonly AminoAcid Sel = new AminoAcid(85, 'U', "Sel", "Selenocysteine");

        /// <summary>
        /// Valine
        /// </summary>
        public readonly AminoAcid Val = new AminoAcid(86, 'V', "Val", "Valine");

        /// <summary>
        /// Tryptophan
        /// </summary>
        public readonly AminoAcid Trp = new AminoAcid(87, 'W', "Trp", "Tryptophan");

        /// <summary>
        /// Undetermined or atypical
        /// </summary>
        public readonly AminoAcid Xxx = new AminoAcid(88, 'X', "Xxx", "Undetermined or atypical");

        /// <summary>
        /// Tyrosine
        /// </summary>
        public readonly AminoAcid Tyr = new AminoAcid(89, 'Y', "Tyr", "Tyrosine");

        /// <summary>
        /// Glutamic Acid or Glutamine
        /// </summary>
        public readonly AminoAcid Glx = new AminoAcid(90, 'Z', "Glx", "Glutamic Acid or Glutamine");
        #endregion

        private static NcbiEAAEncoding instance;
        private static string name = "NCBIeaa";

        /// <summary>
        /// An instance of the NcbiStdAA encoding for amino acids. Since the
        /// data does not change, use this static member instead of constructing
        /// a new one.
        /// </summary>
        public static NcbiEAAEncoding Instance
        {
            get { return instance; }
        }

        private List<AminoAcid> values = new List<AminoAcid>();

        // set up the static instance
        static NcbiEAAEncoding()
        {
            instance = new NcbiEAAEncoding();
        }

        private NcbiEAAEncoding()
        {
            values.Add(Term);
            values.Add(Gap);
            values.Add(Ala);
            values.Add(Asx);
            values.Add(Cys);
            values.Add(Asp);
            values.Add(Glu);
            values.Add(Phe);
            values.Add(Gly);
            values.Add(His);
            values.Add(Ile);
            values.Add(Xle);
            values.Add(Lys);
            values.Add(Leu);
            values.Add(Met);
            values.Add(Asn);
            values.Add(Pyl);
            values.Add(Pro);
            values.Add(Gln);
            values.Add(Arg);
            values.Add(Ser);
            values.Add(Thr);
            values.Add(Sel);
            values.Add(Val);
            values.Add(Trp);
            values.Add(Xxx);
            values.Add(Tyr);
            values.Add(Glx);
        }

        #region IEncoding Members

        /// <summary>
        /// The name of this alphabet is always 'Protein'
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
            get { return true; }
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
        /// Retrieves an encoded AminoAcid instance based on the byte value
        /// representation of that acid.
        /// </summary>
        public ISequenceItem LookupByValue(byte value)
        {
            if (value == 42)
                return Term;
            if (value == 45)
                return Gap;
            if (value < 65 || value > 90)
                throw new Exception("Unrecognized value: " + value);

            return values[value - 63];
        }

        /// <summary>
        /// Retrieves the amino acid associated with a particular charcter symbol. See the comment for
        /// the class description to view the encoding table.
        /// </summary>
        public ISequenceItem LookupBySymbol(char symbol)
        {
            // Using switch statement for performance
            switch (symbol)
            {
                case 'A':
                case 'a':
                    return Ala;
                case 'B':
                case 'b':
                    return Asx;
                case 'C':
                case 'c':
                    return Cys;
                case 'D':
                case 'd':
                    return Asp;
                case 'E':
                case 'e':
                    return Glu;
                case 'F':
                case 'f':
                    return Phe;
                case 'G':
                case 'g':
                    return Gly;
                case 'H':
                case 'h':
                    return His;
                case 'I':
                case 'i':
                    return Ile;
                case 'J':
                case 'j':
                    return Xle;
                case 'K':
                case 'k':
                    return Lys;
                case 'L':
                case 'l':
                    return Leu;
                case 'M':
                case 'm':
                    return Met;
                case 'N':
                case 'n':
                    return Asn;
                case 'O':
                case 'o':
                    return Pyl;
                case 'P':
                case 'p':
                    return Pro;
                case 'Q':
                case 'q':
                    return Gln;
                case 'R':
                case 'r':
                    return Arg;
                case 'S':
                case 's':
                    return Ser;
                case 'T':
                case 't':
                    return Thr;
                case 'U':
                case 'u':
                    return Sel;
                case 'V':
                case 'v':
                    return Val;
                case 'W':
                case 'w':
                    return Trp;
                case 'X':
                case 'x':
                    return Xxx;
                case 'Y':
                case 'y':
                    return Tyr;
                case 'Z':
                case 'z':
                    return Glx;
                case '-':
                    return Gap;
                case '*':
                    return Term;
                default:
                    throw new ArgumentException("Could not recognize symbol: " + symbol);
            }
        }

        /// <summary>
        /// Retrieves the amino acid associated with a particular string symbol.
        /// This method will throw an exception for a string with more than one
        /// character in it. See the comment for the class description to view the
        /// encoding table.
        /// </summary>
        public ISequenceItem LookupBySymbol(string symbol)
        {
            string trimmed = symbol.Trim();
            if (trimmed.Length == 1)
            {
                return LookupBySymbol(trimmed[0]);
            }
            else if (trimmed.Length == 3)
            {
                foreach (AminoAcid aa in values)
                {
                    if (trimmed.Equals(aa.ExtendedSymbol, StringComparison.InvariantCultureIgnoreCase))
                        return aa;
                }
            }

            throw new ArgumentException("Could not recognize symbol: " + trimmed);
        }

        #endregion

        #region ICollection<ISequenceItem> Members
        /// <summary>
        /// The number of alphabet symbols. For this alphabet the result should
        /// always be 28.
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
        /// Indication of whether or not an ISequenceItem is in the alphabet. This is
        /// a simple lookup and will only match exactly with items of this alphabet. It
        /// will not compare items from other alphabets that match the same amino acid.
        /// </summary>
        public bool Contains(ISequenceItem item)
        {
            AminoAcid aa = item as AminoAcid;
            if (aa == null)
                return false;

            return values.Contains(aa);
        }

        /// <summary>
        /// Copies the nucleotides in this alphabet into an array
        /// </summary>
        public void CopyTo(ISequenceItem[] array, int arrayIndex)
        {
            foreach (AminoAcid aa in values)
            {
                array[arrayIndex++] = aa;
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
            foreach (AminoAcid aa in values)
                siList.Add(aa);
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
