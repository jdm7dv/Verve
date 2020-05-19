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
using System.Globalization;
using System.Linq;

namespace Bio
{
    /// <summary>
    /// The basic alphabet that describes symbols used in sequences of amino
    /// acids that come from codon encodings of RNA. This alphabet allows for
    /// the twenty amino acids as well as a termination and gap symbol.
    /// <para>
    /// The character representations come from the NCBIstdaa standard and
    /// are used in many sequence file formats. The NCBIstdaa standard has all
    /// the same characters as NCBIeaa and IUPACaa, but adds Selenocysteine,
    /// termination, and gap symbols to the latter.
    /// </para>
    /// <para>
    /// The entries in this dictionary are:
    /// Symbol - Extended Symbol - Name
    /// A - Ala - Alanine
    /// B - Asx - Aspartic Acid or Asparagine
    /// C - Cys - Cysteine
    /// D - Asp - Aspartic Acid
    /// E - Glu - Glutamic Acid
    /// F - Phe - Phenylalanine
    /// G - Gly - Glycine
    /// H - His - Histidine
    /// I - Ile - Isoleucine
    /// J - Xle - Leucine or Isoleucine
    /// K - Lys - Lysine
    /// L - Leu - Leucine
    /// M - Met - Methionine
    /// N - Asn - Asparagine
    /// O - Pyl - Pyrrolysine
    /// P - Pro - Proline
    /// Q - Gln - Glutamine
    /// R - Arg - Arginine
    /// S - Ser - Serine
    /// T - Thr - Threoine
    /// U - Sel - Selenocysteine
    /// V - Val - Valine
    /// W - Trp - Tryptophan
    /// X - Xxx - Undetermined or atypical
    /// Y - Tyr - Tyrosine
    /// Z - Glx - Glutamic Acid or Glutamine
    /// * - Ter - Termination
    /// - - --- - Gap
    /// </para>
    /// </summary>
    public class ProteinAlphabet : IAlphabet
    {
        #region Amino Acid Definitions
        /// <summary>
        /// Alanine Amino acid 
        /// </summary>
        public readonly AminoAcid Ala = new AminoAcid('A', "Ala", "Alanine");

        /// <summary>
        /// Aspartic Acid or Asparagine
        /// </summary>
        public readonly AminoAcid Asx = new AminoAcid('B', "Asx", "Aspartic Acid or Asparagine", false, true, false);

        /// <summary>
        /// Cysteine Amino acid 
        /// </summary>
        public readonly AminoAcid Cys = new AminoAcid('C', "Cys", "Cysteine");

        /// <summary>
        /// Aspartic Acid
        /// </summary>
        public readonly AminoAcid Asp = new AminoAcid('D', "Asp", "Aspartic Acid");

        /// <summary>
        /// Glutamic Acid
        /// </summary>
        public readonly AminoAcid Glu = new AminoAcid('E', "Glu", "Glutamic Acid");

        /// <summary>
        /// Phenylalanine Amino acid 
        /// </summary>
        public readonly AminoAcid Phe = new AminoAcid('F', "Phe", "Phenylalanine");

        /// <summary>
        /// Glycine Amino acid 
        /// </summary>
        public readonly AminoAcid Gly = new AminoAcid('G', "Gly", "Glycine");

        /// <summary>
        /// Histidine Amino acid
        /// </summary>
        public readonly AminoAcid His = new AminoAcid('H', "His", "Histidine");

        /// <summary>
        /// Isoleucine Amino acid
        /// </summary>
        public readonly AminoAcid Ile = new AminoAcid('I', "Ile", "Isoleucine");

        /// <summary>
        /// Leucine or Isoleucine
        /// </summary>
        public readonly AminoAcid Xle = new AminoAcid('J', "Xle", "Leucine or Isoleucine", false, true, false);

        /// <summary>
        /// Lysine Amino acid
        /// </summary>
        public readonly AminoAcid Lys = new AminoAcid('K', "Lys", "Lysine");

        /// <summary>
        /// Leucine Amino acid
        /// </summary>
        public readonly AminoAcid Leu = new AminoAcid('L', "Leu", "Leucine");

        /// <summary>
        /// Methionine Amino acid
        /// </summary>
        public readonly AminoAcid Met = new AminoAcid('M', "Met", "Methionine");

        /// <summary>
        /// Asparagine Amino acid
        /// </summary>
        public readonly AminoAcid Asn = new AminoAcid('N', "Asn", "Asparagine");

        /// <summary>
        /// Pyrrolysine Amino acid
        /// </summary>
        public readonly AminoAcid Pyl = new AminoAcid('O', "Pyl", "Pyrrolysine");

        /// <summary>
        /// Proline Amino acid
        /// </summary>
        public readonly AminoAcid Pro = new AminoAcid('P', "Pro", "Proline");

        /// <summary>
        /// Glutamine Amino acid
        /// </summary>
        public readonly AminoAcid Gln = new AminoAcid('Q', "Gln", "Glutamine");

        /// <summary>
        /// Arginine Amino acid
        /// </summary>
        public readonly AminoAcid Arg = new AminoAcid('R', "Arg", "Arginine");

        /// <summary>
        /// Serine Amino acid
        /// </summary>
        public readonly AminoAcid Ser = new AminoAcid('S', "Ser", "Serine");

        /// <summary>
        /// Threoine Amino acid
        /// </summary>
        public readonly AminoAcid Thr = new AminoAcid('T', "Thr", "Threoine");

        /// <summary>
        /// Selenocysteine Amino acid
        /// </summary>
        public readonly AminoAcid Sel = new AminoAcid('U', "Sel", "Selenocysteine");

        /// <summary>
        /// Valine Amino acid
        /// </summary>
        public readonly AminoAcid Val = new AminoAcid('V', "Val", "Valine");

        /// <summary>
        /// Tryptophan Amino acid
        /// </summary>
        public readonly AminoAcid Trp = new AminoAcid('W', "Trp", "Tryptophan");

        /// <summary>
        /// Tyrosine Amino acid
        /// </summary>
        public readonly AminoAcid Tyr = new AminoAcid('Y', "Tyr", "Tyrosine");

        /// <summary>
        /// Glutamic Acid or Glutamine
        /// </summary>
        public readonly AminoAcid Glx = new AminoAcid('Z', "Glx", "Glutamic Acid or Glutamine", false, true, false);

        /// <summary>
        /// Undetermined or atypical
        /// </summary>
        public readonly AminoAcid Xxx = new AminoAcid('X', "Xxx", "Undetermined or atypical", false, true, false);

        /// <summary>
        /// Termination character
        /// </summary>
        public readonly AminoAcid Term = new AminoAcid('*', "Ter", "Termination", false, false, true);

        /// <summary>
        /// Gap character
        /// </summary>
        public readonly AminoAcid Gap = new AminoAcid('-', "---", "Gap", true, false, false);

        #endregion

        /// <summary>
        /// Instance of this class.
        /// </summary>
        private static ProteinAlphabet instance;

        /// <summary>
        /// Friendly name for Alphabet type.
        /// </summary>
        private static string name = "Protein";

        /// <summary>
        /// Stores the list of amino acids for protein
        /// </summary>
        private List<AminoAcid> values = new List<AminoAcid>();

        /// <summary>
        /// Stores the set of character that represent 'gap' in Proteins
        /// </summary>
        private HashSet<ISequenceItem> gapItems = new HashSet<ISequenceItem>();

        /// <summary>
        /// Mapping from set of characters to corresponding ambiguous character
        /// </summary>
        private Dictionary<HashSet<ISequenceItem>, ISequenceItem> basicToAmbiguousSymbolMap = new Dictionary<HashSet<ISequenceItem>, ISequenceItem>(HashSet<ISequenceItem>.CreateSetComparer());

        /// <summary>
        /// Mapping from ambiguous character to set of characters they represent
        /// </summary>
        private Dictionary<ISequenceItem, HashSet<ISequenceItem>> ambiguousToBasicSymbolMap = new Dictionary<ISequenceItem, HashSet<ISequenceItem>>();

        /// <summary>
        /// Initializes static members of the ProteinAlphabet class
        /// Set up the static instance
        /// </summary>
        static ProteinAlphabet()
        {
            instance = new ProteinAlphabet();
        }

        /// <summary>
        /// Prevents a default instance of the ProteinAlphabet class from being created.
        /// Populates amino acid values.
        /// </summary>
        private ProteinAlphabet()
        {
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
            values.Add(Term);
            values.Add(Gap);

            PopulateMaps();
        }

        /// <summary>
        /// Gets an instance of the Protein alphabet for amino acids. Since the
        /// data does not change, use this static member instead of constructing
        /// a new one.
        /// </summary>
        public static ProteinAlphabet Instance
        {
            get { return instance; }
        }

        #region IAlphabet Members

        /// <summary>
        /// Gets the name of this alphabet - this is always 'Protein'
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets a value indicating whether this alphabet has termination characters.
        /// This alphabet does have termination characters.
        /// </summary>
        public bool HasTerminations
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this alphabet has ambiguous characters.
        /// This alphabet does have ambiguous characters.
        /// </summary>
        public bool HasAmbiguity
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this alphabet has a gap character.
        /// This alphabet does have a gap character.
        /// </summary>
        public bool HasGaps
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the amino acid that denotes default gap character in Protein
        /// </summary>
        public ISequenceItem DefaultGap
        {
            get
            {
                return Gap;
            }
        }

        /// <summary>
        /// Retrieves the amino acid associated with a particular charcter symbol. See the comment for
        /// the class description to view the encoding table.
        /// </summary>
        /// <param name="symbol">Symbol to look up</param>
        /// <returns>ISequenceItem for amino acid corresponding to input symbol</returns>
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
                    return null;
            }
        }

        /// <summary>
        /// Retrieves the amino acid associated with a particular string symbol.
        /// This method will throw an exception for a string with more than one
        /// character in it. See the comment for the class description to view the
        /// encoding table.
        /// </summary>
        /// <param name="symbol">Symbol to look up</param>
        /// <returns>ISequenceItem for amino acid corresponding to input symbol</returns>
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
                    if (trimmed.Equals(aa.ExtendedSymbol, StringComparison.OrdinalIgnoreCase))
                    {
                        return aa;
                    }
                }
            }

            throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture, Properties.Resource.INVALID_SYMBOL, trimmed, Name));
        }

        /// <summary>
        /// Find the consensus symbol for a set of amino acids
        /// </summary>
        /// <param name="symbols">Set of sequence items</param>
        /// <returns>Consensus amino acid</returns>
        public ISequenceItem GetConsensusSymbol(HashSet<ISequenceItem> symbols)
        {
            // Validate that all are valid protein symbols
            foreach (ISequenceItem s in symbols)
            {
                AminoAcid aa = s as AminoAcid;
                if (aa == null || !values.Contains(aa))
                {
                    throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture, Properties.Resource.INVALID_SYMBOL, s.Symbol, Name));
                }
            }

            if (symbols.Contains(Xxx))
            {
                return Xxx;
            }

            // Remove all gap symbols
            symbols.ExceptWith(gapItems);

            if (symbols.Count == 0)
            {
                // All are gap characters, return default 'Gap'
                return DefaultGap;
            }
            else if (symbols.Count == 1)
            {
                return symbols.First();
            }
            else
            {
                HashSet<ISequenceItem> baseSet = new HashSet<ISequenceItem>();
                foreach (ISequenceItem aa in symbols)
                {
                    if (ambiguousToBasicSymbolMap.ContainsKey(aa))
                    {
                        baseSet.UnionWith(ambiguousToBasicSymbolMap[aa]);
                    }
                    else
                    {
                        // If not found in ambiguous map, it has to be base / unambiguous character
                        baseSet.Add(aa);
                    }
                }

                if (basicToAmbiguousSymbolMap.ContainsKey(baseSet))
                {
                    return basicToAmbiguousSymbolMap[baseSet];
                }
                else
                {
                    return Xxx;
                }
            }
        }

        /// <summary>
        /// Find the set of symbols that is represented by input symbol
        /// </summary>
        /// <param name="symbol">Symbol to look up</param>
        /// <returns>Set of symbols</returns>
        public HashSet<ISequenceItem> GetBasicSymbols(ISequenceItem symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException("symbol");
            }

            // Validate input symbol
            AminoAcid aa = symbol as AminoAcid;
            if (aa == null || !values.Contains(aa))
            {
                throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture, Properties.Resource.INVALID_SYMBOL, symbol.Symbol, Name));
            }

            if (symbol == Xxx)
            {
                // Return all unambiguous symbols
                return new HashSet<ISequenceItem>(values.Where(AA => !AA.IsAmbiguous).Select(AA => (ISequenceItem)AA));
            }
            else
            {
                if (ambiguousToBasicSymbolMap.ContainsKey(symbol))
                {
                    return ambiguousToBasicSymbolMap[symbol];
                }
                else
                {
                    // It is base / unambiguous character
                    return new HashSet<ISequenceItem>() { symbol };
                }
            }
        }

        /// <summary>
        /// Returns a list of all of the stored items filtered by the specified parameters
        /// </summary>
        /// <param name="includeBasics">Include the basic items of the alphabet (Ala, Cys, Asp, Gle, etc.)</param>
        /// <param name="includeGaps">Include the gap item (-)</param>
        /// <param name="includeAmbiguities">Include the ambiguity items (Asx, Xle, Xxx, and Glx)</param>
        /// <param name="includeTerminations">Include the termination item (*)</param>
        /// <returns>List of all stored items matching parameters</returns>
        public List<ISequenceItem> LookupAll(bool includeBasics, bool includeGaps, bool includeAmbiguities, bool includeTerminations)
        {
            List<ISequenceItem> result = new List<ISequenceItem>();

            if (includeBasics)
            {
                result.Add(Ala);
                result.Add(Cys);
                result.Add(Asp);
                result.Add(Glu);
                result.Add(Phe);
                result.Add(Gly);
                result.Add(His);
                result.Add(Ile);
                result.Add(Lys);
                result.Add(Leu);
                result.Add(Met);
                result.Add(Asn);
                result.Add(Pyl);
                result.Add(Pro);
                result.Add(Gln);
                result.Add(Arg);
                result.Add(Ser);
                result.Add(Thr);
                result.Add(Sel);
                result.Add(Val);
                result.Add(Trp);
                result.Add(Tyr);
            }

            if (includeGaps)
            {
                result.Add(Gap);
            }

            if (includeAmbiguities)
            {
                result.Add(Asx);
                result.Add(Xle);
                result.Add(Xxx);
                result.Add(Glx);
            }

            if (includeTerminations)
            {
                result.Add(Term);
            }

            return result;
        }
        #endregion

        #region ICollection<ISequenceItem> Members
        /// <summary>
        /// Gets the number of alphabet symbols. 
        /// For this alphabet the result should always be 28.
        /// </summary>
        public int Count
        {
            get { return values.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the alphabet is read only.
        /// Always returns true.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// This is a read only collection and thus this method will throw an exception
        /// </summary>
        /// <param name="item">Item to be added</param>
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
        /// <param name="item">Item whose presence is to be checked</param>
        /// <returns>True if this contains input item</returns>
        public bool Contains(ISequenceItem item)
        {
            AminoAcid aa = item as AminoAcid;
            if (aa == null)
            {
                return false;
            }

            return values.Contains(aa);
        }

        /// <summary>
        /// Copies the nucleotides in this alphabet into an array
        /// </summary>
        /// <param name="array">Destination array</param>
        /// <param name="arrayIndex">Start index in array for copying</param>
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
        /// <param name="item">Item to be removed</param>
        /// <returns>Value indicating whether value was removed</returns>
        public bool Remove(ISequenceItem item)
        {
            throw new Exception("Read Only");
        }
        #endregion

        #region IEnumerable<ISequenceItem> Members
        /// <summary>
        /// Creates an IEnumerator of the nucleotides
        /// </summary>
        /// <returns>Enumerator of ISequenceItem over alphabet values</returns>
        public IEnumerator<ISequenceItem> GetEnumerator()
        {
            List<ISequenceItem> seqItemList = new List<ISequenceItem>();
            foreach (AminoAcid aa in values)
            {
                seqItemList.Add(aa);
            }

            return seqItemList.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Creates an IEnumerator of the nucleotides
        /// </summary>
        /// <returns>Enumerator over alphabet values</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
        #endregion

        #region Ambiguous Maps
        /// <summary>
        /// Populates basic Set to ambiguous symbol (and vice versa) maps
        /// Sets gap symbols
        /// </summary>
        private void PopulateMaps()
        {
            // Store Gap characters
            gapItems = new HashSet<ISequenceItem>(values.Where(AA => AA.IsGap).Select(AA => (ISequenceItem)AA));

            // Populate Ambiguous Maps
            HashSet<ISequenceItem> symbols;

            // Asp, Asn, Asx
            symbols = new HashSet<ISequenceItem>() { Asp, Asn };
            basicToAmbiguousSymbolMap.Add(symbols, Asx);
            ambiguousToBasicSymbolMap.Add(Asx, symbols);

            // Leu, Ile, Xle
            symbols = new HashSet<ISequenceItem>() { Leu, Ile };
            basicToAmbiguousSymbolMap.Add(symbols, Xle);
            ambiguousToBasicSymbolMap.Add(Xle, symbols);

            // Glu, Gln, Glx
            symbols = new HashSet<ISequenceItem>() { Glu, Gln };
            basicToAmbiguousSymbolMap.Add(symbols, Glx);
            ambiguousToBasicSymbolMap.Add(Glx, symbols);
        }
        #endregion
    }
}
