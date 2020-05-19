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
    /// The basic alphabet that describes symbols used in DNA sequences.
    /// This alphabet allows not only for the four base nucleotide symbols,
    /// but also for various ambiguities, termination, and gap symbols.
    /// <para>
    /// The character representations come from the NCBI4na standard and
    /// are used in many sequence file formats. The NCBI4na standard is the
    /// same as the IUPACna standard with only the addition of the gap
    /// character.
    /// </para>
    /// <para>
    /// The entries in this dictionary are:
    /// Symbol - Name
    /// A - Adenine
    /// C - Cytosine
    /// M - A or C
    /// G - Guanine
    /// R - G or A
    /// S - G or C
    /// V - G or V or A
    /// T - Thymine
    /// W - A or T
    /// Y - T or C
    /// H - A or C or T
    /// K - G or T
    /// D - G or A or T
    /// B - G or T or C
    /// - - Gap
    /// N - A or G or T or C
    /// </para>
    /// </summary>
    public class DnaAlphabet : IAlphabet
    {
        #region Nucleotide Definitions
        /// <summary>
        /// Nucleotide Adenine
        /// </summary>
        public readonly Nucleotide A = new Nucleotide('A', "Adenine");

        /// <summary>
        /// Nucleotide Cytosine
        /// </summary>
        public readonly Nucleotide C = new Nucleotide('C', "Cytosine");

        /// <summary>
        /// Nucleotide Guanine
        /// </summary>
        public readonly Nucleotide G = new Nucleotide('G', "Guanine");

        /// <summary>
        /// Nucleotide Thymine
        /// </summary>
        public readonly Nucleotide T = new Nucleotide('T', "Thymine");

        /// <summary>
        /// Adenine or Cytosine
        /// </summary>
        public readonly Nucleotide AC = new Nucleotide('M', "A or C", false, true);

        /// <summary>
        /// Guanine or Adenine
        /// </summary>
        public readonly Nucleotide GA = new Nucleotide('R', "G or A", false, true);

        /// <summary>
        /// Guanine or Cytosine
        /// </summary>
        public readonly Nucleotide GC = new Nucleotide('S', "G or C", false, true);

        /// <summary>
        /// Adenine or Thymine
        /// </summary>
        public readonly Nucleotide AT = new Nucleotide('W', "A or T", false, true);

        /// <summary>
        /// Thymine or Cytosine
        /// </summary>
        public readonly Nucleotide TC = new Nucleotide('Y', "T or C", false, true);

        /// <summary>
        /// Gaunine or Thymine
        /// </summary>
        public readonly Nucleotide GT = new Nucleotide('K', "G or T", false, true);

        /// <summary>
        /// Gaunine, Cytosine, or Adenine
        /// </summary>
        public readonly Nucleotide GCA = new Nucleotide('V', "G or C or A", false, true);

        /// <summary>
        /// Adenine, Cytosine, or Thymine
        /// </summary>
        public readonly Nucleotide ACT = new Nucleotide('H', "A or C or T", false, true);

        /// <summary>
        /// Gaunine, Adenine, or Thymine
        /// </summary>
        public readonly Nucleotide GAT = new Nucleotide('D', "G or A or T", false, true);

        /// <summary>
        /// Gaunine, Thymine, or Cytosine
        /// </summary>
        public readonly Nucleotide GTC = new Nucleotide('B', "G or T or C", false, true);

        /// <summary>
        /// Adenine, Guanine, Cytosine, or Thymine
        /// </summary>
        public readonly Nucleotide Any = new Nucleotide('N', "A or G or C or T", false, true);

        /// <summary>
        /// A gap character
        /// </summary>
        public readonly Nucleotide Gap = new Nucleotide('-', "Gap", true, false);
        #endregion

        /// <summary>
        /// Static instance of this class.
        /// </summary>
        private static DnaAlphabet instance;

        /// <summary>
        /// Friendly name for Alphabet type.
        /// </summary>
        private static string name = "DNA";

        /// <summary>
        /// Stores the list of Nucleotides for DNA
        /// </summary>
        private List<Nucleotide> values = new List<Nucleotide>();

        /// <summary>
        /// Stores the set of character that represent 'gap' in DNA
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
        /// Initializes static members of the DnaAlphabet class
        /// Set up the static instance
        /// </summary>
        static DnaAlphabet()
        {
            instance = new DnaAlphabet();
        }

        /// <summary>
        /// Prevents a default instance of the DnaAlphabet class from being created.
        /// Populates nucleotide values, and ambiguous maps.
        /// </summary>
        private DnaAlphabet()
        {
            values.Add(A);
            values.Add(C);
            values.Add(G);
            values.Add(T);
            values.Add(Gap);
            values.Add(Any);
            values.Add(AC);
            values.Add(GA);
            values.Add(GC);
            values.Add(AT);
            values.Add(TC);
            values.Add(GT);
            values.Add(GCA);
            values.Add(ACT);
            values.Add(GAT);
            values.Add(GTC);

            PopulateMaps();
        }

        /// <summary>
        /// Gets an instance of the DNA alphabet for nucleic acids. Since the
        /// data does not change, use this static member instead of constructing
        /// a new one.
        /// </summary>
        public static DnaAlphabet Instance
        {
            get { return instance; }
        }

        #region IAlphabet Members

        /// <summary>
        /// Gets the name of this alphabet - this is always 'DNA'
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Gets a value indicating whether this alphabet has termination characters.
        /// This alphabet does not have termination characters.
        /// </summary>
        public bool HasTerminations
        {
            get { return false; }
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
        /// Gets the nucleotide that denotes default gap character in DNA
        /// </summary>
        public ISequenceItem DefaultGap 
        {
            get
            {
                return Gap;
            }
        }

        /// <summary>
        /// Retrieves the nucleotide associated with a particular charcter symbol. See the comment for
        /// the class description to view the encoding table.
        /// </summary>
        /// <param name="symbol">Symbol to look up</param>
        /// <returns>ISequenceItem for nucleotide corresponding to input symbol</returns>
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
                    return T;
                case '-':
                    return Gap;
                case 'N':
                case 'n':
                    return Any;
                case 'R':
                case 'r':
                    return GA;
                case 'M':
                case 'm':
                    return AC;
                case 'S':
                case 's':
                    return GC;
                case 'V':
                case 'v':
                    return GCA;
                case 'W':
                case 'w':
                    return AT;
                case 'Y':
                case 'y':
                    return TC;
                case 'H':
                case 'h':
                    return ACT;
                case 'K':
                case 'k':
                    return GT;
                case 'D':
                case 'd':
                    return GAT;
                case 'B':
                case 'b':
                    return GTC;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Retrieves the nucleotide associated with a particular string symbol.
        /// This method will throw an exception for a string with more than one
        /// character in it. See the comment for the class description to view the
        /// encoding table.
        /// </summary>
        /// <param name="symbol">Symbol as a string</param>
        /// <returns>ISequenceItem for nucleotide corresponding to input symbol</returns>
        public ISequenceItem LookupBySymbol(string symbol)
        {
            string trimmed = symbol.Trim();
            if (trimmed.Length != 1)
            {
                throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture, Properties.Resource.INVALID_SYMBOL, trimmed, Name));
            }

            return LookupBySymbol(trimmed[0]);
        }

        /// <summary>
        /// Find the consensus nucleotide for a set of nucleotides
        /// </summary>
        /// <param name="symbols">Set of sequence items</param>
        /// <returns>Consensus nucleotide</returns>
        public ISequenceItem GetConsensusSymbol(HashSet<ISequenceItem> symbols)
        {
            // Validate that all are valid DNA symbols
            foreach (ISequenceItem s in symbols)
            {
                Nucleotide n = s as Nucleotide;
                if (n == null || !values.Contains(n))
                {
                    throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture, Properties.Resource.INVALID_SYMBOL, s.Symbol, Name));
                }
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
                foreach (Nucleotide n in symbols)
                {
                    if (ambiguousToBasicSymbolMap.ContainsKey(n))
                    {
                        baseSet.UnionWith(ambiguousToBasicSymbolMap[n]);
                    }
                    else
                    {
                        // If not found in ambiguous map, it has to be base / unambiguous character
                        baseSet.Add(n);
                    }
                }

                return basicToAmbiguousSymbolMap[baseSet];
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
            Nucleotide n = symbol as Nucleotide;
            if (n == null || !values.Contains(n))
            {
                throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture, Properties.Resource.INVALID_SYMBOL, symbol.Symbol, Name));
            }

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

        /// <summary>
        /// Returns a list of all of the stored items filtered by the specified parameters
        /// </summary>
        /// <param name="includeBasics">Include the basic items of the alphabet (G, A, T, and C)</param>
        /// <param name="includeGaps">Include the gap item (-)</param>
        /// <param name="includeAmbiguities">Include the ambiguity items (GA, GAT, GC, etc.)</param>
        /// <param name="includeTerminations">Has no effect in this alphabet</param>
        /// <returns>List of all stored items matching parameters</returns>
        public List<ISequenceItem> LookupAll(bool includeBasics, bool includeGaps, bool includeAmbiguities, bool includeTerminations)
        {
            List<ISequenceItem> result = new List<ISequenceItem>();

            if (includeBasics)
            {
                result.Add(G);
                result.Add(A);
                result.Add(T);
                result.Add(C);
            }

            if (includeGaps)
            {
                result.Add(Gap);
            }

            if (includeAmbiguities)
            {
                result.Add(GA);
                result.Add(AC);
                result.Add(GC);
                result.Add(GCA);
                result.Add(AT);
                result.Add(TC);
                result.Add(ACT);
                result.Add(GT);
                result.Add(GAT);
                result.Add(GTC);
            }

            return result;
        }
        #endregion

        #region ICollection<ISequenceItem> Members
        /// <summary>
        /// Gets the number of alphabet symbols. 
        /// For this alphabet the result should always be 16.
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
        /// will not compare items from other alphabets that match the same nucleotide.
        /// </summary>
        /// <param name="item">Item whose presence is to be checked</param>
        /// <returns>True if this contains input item</returns>
        public bool Contains(ISequenceItem item)
        {
            Nucleotide nucleo = item as Nucleotide;
            if (nucleo == null)
            {
                return false;
            }

            return values.Contains(nucleo);
        }

        /// <summary>
        /// Copies the nucleotides in this alphabet into an array
        /// </summary>
        /// <param name="array">Destination array</param>
        /// <param name="arrayIndex">Start index in array for copying</param>
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
            foreach (Nucleotide n in values)
            {
                seqItemList.Add(n);
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
            gapItems = new HashSet<ISequenceItem>(values.Where(N => N.IsGap).Select(N => (ISequenceItem)N));

            HashSet<ISequenceItem> symbols;

            // A, C, AC
            symbols = new HashSet<ISequenceItem>() { A, C };
            basicToAmbiguousSymbolMap.Add(symbols, AC);
            ambiguousToBasicSymbolMap.Add(AC, symbols);

            // G, A, GA
            symbols = new HashSet<ISequenceItem>() { G, A };
            basicToAmbiguousSymbolMap.Add(symbols, GA);
            ambiguousToBasicSymbolMap.Add(GA, symbols);

            // G, C, GC
            symbols = new HashSet<ISequenceItem>() { G, C };
            basicToAmbiguousSymbolMap.Add(symbols, GC);
            ambiguousToBasicSymbolMap.Add(GC, symbols);

            // A, T, AT
            symbols = new HashSet<ISequenceItem>() { A, T };
            basicToAmbiguousSymbolMap.Add(symbols, AT);
            ambiguousToBasicSymbolMap.Add(AT, symbols);

            // T, C, TC
            symbols = new HashSet<ISequenceItem>() { T, C };
            basicToAmbiguousSymbolMap.Add(symbols, TC);
            ambiguousToBasicSymbolMap.Add(TC, symbols);

            // G, T, GT
            symbols = new HashSet<ISequenceItem>() { G, T };
            basicToAmbiguousSymbolMap.Add(symbols, GT);
            ambiguousToBasicSymbolMap.Add(GT, symbols);

            // G, C, A, GCA
            symbols = new HashSet<ISequenceItem>() { G, C, A };
            basicToAmbiguousSymbolMap.Add(symbols, GCA);
            ambiguousToBasicSymbolMap.Add(GCA, symbols);

            // A, C, T, ACT
            symbols = new HashSet<ISequenceItem>() { A, C, T };
            basicToAmbiguousSymbolMap.Add(symbols, ACT);
            ambiguousToBasicSymbolMap.Add(ACT, symbols);

            // G, A, T, GAT
            symbols = new HashSet<ISequenceItem>() { G, A, T };
            basicToAmbiguousSymbolMap.Add(symbols, GAT);
            ambiguousToBasicSymbolMap.Add(GAT, symbols);

            // G, T, C, GTC
            symbols = new HashSet<ISequenceItem>() { G, T, C };
            basicToAmbiguousSymbolMap.Add(symbols, GTC);
            ambiguousToBasicSymbolMap.Add(GTC, symbols);

            // A, T, G, C, Any
            symbols = new HashSet<ISequenceItem>() { A, T, G, C };
            basicToAmbiguousSymbolMap.Add(symbols, Any);
            ambiguousToBasicSymbolMap.Add(Any, symbols);
        }
        #endregion
    }
}
