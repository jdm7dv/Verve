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

namespace MBF
{
    /// <summary>
    /// The basic alphabet that describes symbols used in RNA sequences.
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
    /// U - Uracil
    /// W - A or U
    /// Y - U or C
    /// H - A or C or U
    /// K - G or U
    /// D - G or A or U
    /// B - G or U or C
    /// - - Gap
    /// N - A or G or U or C
    /// </para>
    /// </summary>
    public class RnaAlphabet : IAlphabet
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
        /// Nucleotide Uracil
        /// </summary>
        public readonly Nucleotide U = new Nucleotide('U', "Uracil");

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
        /// Adenine or Uracil
        /// </summary>
        public readonly Nucleotide AU = new Nucleotide('W', "A or U", false, true);

        /// <summary>
        /// Uracil or Cytosine
        /// </summary>
        public readonly Nucleotide UC = new Nucleotide('Y', "T or C", false, true);

        /// <summary>
        /// Gaunine or Uracil
        /// </summary>
        public readonly Nucleotide GU = new Nucleotide('K', "G or U", false, true);

        /// <summary>
        /// Gaunine, Cytosine, or Adenine
        /// </summary>
        public readonly Nucleotide GCA = new Nucleotide('V', "G or C or A", false, true);

        /// <summary>
        /// Adenine, Cytosine, or Uracil
        /// </summary>
        public readonly Nucleotide ACU = new Nucleotide('H', "A or C or U", false, true);

        /// <summary>
        /// Gaunine, Adenine, or Uracil
        /// </summary>
        public readonly Nucleotide GAU = new Nucleotide('D', "G or A or U", false, true);

        /// <summary>
        /// Gaunine, Uracil, or Cytosine
        /// </summary>
        public readonly Nucleotide GUC = new Nucleotide('B', "G or U or C", false, true);

        /// <summary>
        /// Adenine, Guanine, Cytosine, or Uracil
        /// </summary>
        public readonly Nucleotide Any = new Nucleotide('N', "A or G or C or U", false, true);

        /// <summary>
        /// A gap character
        /// </summary>
        public readonly Nucleotide Gap = new Nucleotide('-', "Gap", true, false);
        #endregion

        /// <summary>
        /// Static instance of this class.
        /// </summary>
        private static RnaAlphabet instance;

        /// <summary>
        /// Friendly name for Alphabet type.
        /// </summary>
        private static string name = "RNA";

        /// <summary>
        /// Stores the list of Nucleotides for RNA
        /// </summary>
        private List<Nucleotide> values = new List<Nucleotide>();

        /// <summary>
        /// Stores the set of character that represent 'gap' in RNA
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
        /// Initializes static members of the RnaAlphabet class
        /// Set up the static instance
        /// </summary>
        static RnaAlphabet()
        {
            instance = new RnaAlphabet();
        }

        /// <summary>
        /// Prevents a default instance of the RnaAlphabet class from being created.
        /// Populates nucleotide values, and ambiguous maps.
        /// </summary>
        private RnaAlphabet()
        {
            values.Add(A);
            values.Add(C);
            values.Add(G);
            values.Add(U);
            values.Add(Gap);
            values.Add(Any);
            values.Add(AC);
            values.Add(GA);
            values.Add(GC);
            values.Add(AU);
            values.Add(UC);
            values.Add(GU);
            values.Add(GCA);
            values.Add(ACU);
            values.Add(GAU);
            values.Add(GUC);

            PopulateMaps();
        }

        /// <summary>
        /// Gets an instance of the RNA alphabet for nucleic acids. Since the
        /// data does not change, use this static member instead of constructing
        /// a new one.
        /// </summary>
        public static RnaAlphabet Instance
        {
            get { return instance; }
        }

        #region IAlphabet Members

        /// <summary>
        /// Gets the name of this alphabet - this is always 'RNA'
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
        /// Gets the nucleotide that denotes default gap character in RNA
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
                case 'U':
                case 'u':
                    return U;
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
                    return AU;
                case 'Y':
                case 'y':
                    return UC;
                case 'H':
                case 'h':
                    return ACU;
                case 'K':
                case 'k':
                    return GU;
                case 'D':
                case 'd':
                    return GAU;
                case 'B':
                case 'b':
                    return GUC;
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
        /// <param name="symbol">Symbol to look up</param>
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
        /// Retrieves the nucleotide associated with a particular byte value.
        /// See the comment for the class description to view the
        /// encoding table.
        /// </summary>
        /// <param name="value">Byte value of the symbol</param>
        /// <returns>ISequenceItem for nucleotide corresponding to input value</returns>
        public ISequenceItem LookupByValue(byte value)
        {
            // Using switch statement for performance
            switch (value)
            {
                case 65:
                    return A;
                case 67:
                    return C;
                case 71:
                    return G;
                case 85:
                    return U;
                case 45:
                    return Gap;
                case 78:
                    return Any;
                case 82:
                    return GA;
                case 77:
                    return AC;
                case 83:
                    return GC;
                case 86:
                    return GCA;
                case 87:
                    return AU;
                case 89:
                    return UC;
                case 72:
                    return ACU;
                case 75:
                    return GU;
                case 68:
                    return GAU;
                case 66:
                    return GUC;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Find the consensus nucleotide for a set of nucleotides
        /// </summary>
        /// <param name="symbols">Set of sequence items</param>
        /// <returns>Consensus nucleotide</returns>
        public ISequenceItem GetConsensusSymbol(HashSet<ISequenceItem> symbols)
        {
            // Validate that all are valid RNA symbols
            foreach (ISequenceItem sequenceItem in symbols)
            {
                Nucleotide nucleotide = sequenceItem as Nucleotide;

                if (sequenceItem == null)
                {
                    throw new ArgumentException(Properties.Resource.ParameterContainsNullValue, "symbols");
                }

                if (nucleotide == null)
                {
                    throw new ArgumentException(Properties.Resource.AllItemMustBeNucleotide, "symbols");
                }

                if (!values.Contains(nucleotide))
                {
                    throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture, Properties.Resource.INVALID_SYMBOL, sequenceItem.Symbol, Name));
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
        /// <param name="includeBasics">Include the basic items of the alphabet (G, A, U, and C)</param>
        /// <param name="includeGaps">Include the gap item (-)</param>
        /// <param name="includeAmbiguities">Include the ambiguity items (GA, GAU, GC, etc.)</param>
        /// <param name="includeTerminations">Has no effect in this alphabet</param>
        /// <returns>List of all stored items matching parameters</returns>
        public List<ISequenceItem> LookupAll(bool includeBasics, bool includeGaps, bool includeAmbiguities, bool includeTerminations)
        {
            List<ISequenceItem> result = new List<ISequenceItem>();

            if (includeBasics)
            {
                result.Add(G);
                result.Add(A);
                result.Add(U);
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
                result.Add(AU);
                result.Add(UC);
                result.Add(ACU);
                result.Add(GU);
                result.Add(GAU);
                result.Add(GUC);
                result.Add(Any);
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
            gapItems = new HashSet<ISequenceItem>(values.Where(N => N.IsGap).Select(N => ((ISequenceItem)N)));

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

            // A, U, AU
            symbols = new HashSet<ISequenceItem>() { A, U };
            basicToAmbiguousSymbolMap.Add(symbols, AU);
            ambiguousToBasicSymbolMap.Add(AU, symbols);

            // U, C, UC
            symbols = new HashSet<ISequenceItem>() { U, C };
            basicToAmbiguousSymbolMap.Add(symbols, UC);
            ambiguousToBasicSymbolMap.Add(UC, symbols);

            // G, U, GU
            symbols = new HashSet<ISequenceItem>() { G, U };
            basicToAmbiguousSymbolMap.Add(symbols, GU);
            ambiguousToBasicSymbolMap.Add(GU, symbols);

            // G, C, A, GCA
            symbols = new HashSet<ISequenceItem>() { G, C, A };
            basicToAmbiguousSymbolMap.Add(symbols, GCA);
            ambiguousToBasicSymbolMap.Add(GCA, symbols);

            // A, C, U, ACU
            symbols = new HashSet<ISequenceItem>() { A, C, U };
            basicToAmbiguousSymbolMap.Add(symbols, ACU);
            ambiguousToBasicSymbolMap.Add(ACU, symbols);

            // G, A, U, GAU
            symbols = new HashSet<ISequenceItem>() { G, A, U };
            basicToAmbiguousSymbolMap.Add(symbols, GAU);
            ambiguousToBasicSymbolMap.Add(GAU, symbols);

            // G, U, C, GUC
            symbols = new HashSet<ISequenceItem>() { G, U, C };
            basicToAmbiguousSymbolMap.Add(symbols, GUC);
            ambiguousToBasicSymbolMap.Add(GUC, symbols);

            // A, U, G, C, Any
            symbols = new HashSet<ISequenceItem>() { A, U, G, C };
            basicToAmbiguousSymbolMap.Add(symbols, Any);
            ambiguousToBasicSymbolMap.Add(Any, symbols);
        }
        #endregion
    }
}
