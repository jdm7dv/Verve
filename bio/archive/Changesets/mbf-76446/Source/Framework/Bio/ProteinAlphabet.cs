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
    /// C - Cys - Cysteine
    /// D - Asp - Aspartic Acid
    /// E - Glu - Glutamic Acid
    /// F - Phe - Phenylalanine
    /// G - Gly - Glycine
    /// H - His - Histidine
    /// I - Ile - Isoleucine
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
    /// Y - Tyr - Tyrosine
    /// * - Ter - Termination
    /// - - --- - Gap.
    /// </para>
    /// </summary>
    public class ProteinAlphabet : IAlphabet
    {
        #region Private members

        /// <summary>
        /// Contains only basic symbols including Gap
        /// </summary>
        private HashSet<byte> basicSymbols = new HashSet<byte>();

        /// <summary>
        /// Amino acids map  -  Maps A to A  and a to A
        /// that is key will contain unique values.
        /// This will be used in the IsValidSymbol method to address Scenarios like a == A, G == g etc.
        /// </summary>
        private Dictionary<byte, byte> aminoAcidValueMap = new Dictionary<byte, byte>();

        /// <summary>
        /// Holds the amino acids present in this RnaAlphabet.
        /// </summary>
        private List<byte> aminoAcids = new List<byte>();

        /// <summary>
        /// Mapping from set of symbols to corresponding ambiguous symbol.
        /// </summary>
        private Dictionary<HashSet<byte>, byte> basicSymbolsToAmbiguousSymbolMap = new Dictionary<HashSet<byte>, byte>(HashSet<byte>.CreateSetComparer());

        /// <summary>
        /// Mapping from ambiguous symbol to set of basic symbols they represent.
        /// </summary>
        private Dictionary<byte, HashSet<byte>> ambiguousSyToBasicSymbolsMap = new Dictionary<byte, HashSet<byte>>();

        #endregion Private members

        /// <summary>
        /// Initializes static members of the ProteinAlphabet class
        /// Set up the static instance.
        /// </summary>
        static ProteinAlphabet()
        {
            Instance = new ProteinAlphabet();
        }

        /// <summary>
        /// Initializes a new instance of the ProteinAlphabet class.
        /// </summary>
        protected ProteinAlphabet()
        {
            this.Name = Properties.Resource.ProteinAlphabetName;
            this.HasGaps = true;
            this.HasAmbiguity = false;
            this.HasTerminations = true;
            this.IsComplementSupported = false;

            this.A = (byte)'A';
            this.C = (byte)'C';
            this.D = (byte)'D';
            this.E = (byte)'E';
            this.F = (byte)'F';
            this.G = (byte)'G';
            this.H = (byte)'H';
            this.I = (byte)'I';
            this.K = (byte)'K';
            this.L = (byte)'L';
            this.M = (byte)'M';
            this.N = (byte)'N';
            this.O = (byte)'O';
            this.P = (byte)'P';
            this.Q = (byte)'Q';
            this.R = (byte)'R';
            this.S = (byte)'S';
            this.T = (byte)'T';
            this.U = (byte)'U';
            this.V = (byte)'V';
            this.W = (byte)'W';
            this.Y = (byte)'Y';

            this.Gap = (byte)'-';
            this.Ter = (byte)'*';

            // Add to basic symbols
            basicSymbols.Add(A); basicSymbols.Add((byte)char.ToLower((char)A, CultureInfo.InvariantCulture));
            basicSymbols.Add(C); basicSymbols.Add((byte)char.ToLower((char)C, CultureInfo.InvariantCulture));
            basicSymbols.Add(D); basicSymbols.Add((byte)char.ToLower((char)D, CultureInfo.InvariantCulture));
            basicSymbols.Add(E); basicSymbols.Add((byte)char.ToLower((char)E, CultureInfo.InvariantCulture));
            basicSymbols.Add(F); basicSymbols.Add((byte)char.ToLower((char)F, CultureInfo.InvariantCulture));
            basicSymbols.Add(G); basicSymbols.Add((byte)char.ToLower((char)G, CultureInfo.InvariantCulture));
            basicSymbols.Add(H); basicSymbols.Add((byte)char.ToLower((char)H, CultureInfo.InvariantCulture));
            basicSymbols.Add(I); basicSymbols.Add((byte)char.ToLower((char)I, CultureInfo.InvariantCulture));
            basicSymbols.Add(K); basicSymbols.Add((byte)char.ToLower((char)K, CultureInfo.InvariantCulture));
            basicSymbols.Add(L); basicSymbols.Add((byte)char.ToLower((char)L, CultureInfo.InvariantCulture));
            basicSymbols.Add(M); basicSymbols.Add((byte)char.ToLower((char)M, CultureInfo.InvariantCulture));
            basicSymbols.Add(N); basicSymbols.Add((byte)char.ToLower((char)N, CultureInfo.InvariantCulture));
            basicSymbols.Add(O); basicSymbols.Add((byte)char.ToLower((char)O, CultureInfo.InvariantCulture));
            basicSymbols.Add(P); basicSymbols.Add((byte)char.ToLower((char)P, CultureInfo.InvariantCulture));
            basicSymbols.Add(Q); basicSymbols.Add((byte)char.ToLower((char)Q, CultureInfo.InvariantCulture));
            basicSymbols.Add(R); basicSymbols.Add((byte)char.ToLower((char)R, CultureInfo.InvariantCulture));
            basicSymbols.Add(S); basicSymbols.Add((byte)char.ToLower((char)S, CultureInfo.InvariantCulture));
            basicSymbols.Add(T); basicSymbols.Add((byte)char.ToLower((char)T, CultureInfo.InvariantCulture));
            basicSymbols.Add(U); basicSymbols.Add((byte)char.ToLower((char)U, CultureInfo.InvariantCulture));
            basicSymbols.Add(V); basicSymbols.Add((byte)char.ToLower((char)V, CultureInfo.InvariantCulture));
            basicSymbols.Add(W); basicSymbols.Add((byte)char.ToLower((char)W, CultureInfo.InvariantCulture));
            basicSymbols.Add(Y); basicSymbols.Add((byte)char.ToLower((char)Y, CultureInfo.InvariantCulture));
            basicSymbols.Add(this.Gap);

            this.AddAminoAcid(this.A, (byte)'a');
            this.AddAminoAcid(this.C, (byte)'c');
            this.AddAminoAcid(this.D, (byte)'d');
            this.AddAminoAcid(this.E, (byte)'e');
            this.AddAminoAcid(this.F, (byte)'f');
            this.AddAminoAcid(this.G, (byte)'g');
            this.AddAminoAcid(this.H, (byte)'h');
            this.AddAminoAcid(this.I, (byte)'i');
            this.AddAminoAcid(this.K, (byte)'k');
            this.AddAminoAcid(this.L, (byte)'l');
            this.AddAminoAcid(this.M, (byte)'m');
            this.AddAminoAcid(this.N, (byte)'n');
            this.AddAminoAcid(this.O, (byte)'o');
            this.AddAminoAcid(this.P, (byte)'p');
            this.AddAminoAcid(this.Q, (byte)'q');
            this.AddAminoAcid(this.R, (byte)'r');
            this.AddAminoAcid(this.S, (byte)'s');
            this.AddAminoAcid(this.T, (byte)'t');
            this.AddAminoAcid(this.U, (byte)'u');
            this.AddAminoAcid(this.V, (byte)'v');
            this.AddAminoAcid(this.W, (byte)'w');
            this.AddAminoAcid(this.Y, (byte)'y');

            this.AddAminoAcid(this.Gap);
            this.AddAminoAcid(this.Ter);
        }

        /// <summary>
        /// Gets the Alanine Amino acid. 
        /// </summary>
        public byte A { get; private set; }

        /// <summary>
        /// Gets the Cysteine Amino acid.
        /// </summary>
        public byte C { get; private set; }

        /// <summary>
        /// Gets the Aspartic Acid.
        /// </summary>
        public byte D { get; private set; }

        /// <summary>
        /// Gets the Glutamic Acid.
        /// </summary>
        public byte E { get; private set; }

        /// <summary>
        /// Gets the Phenylalanine Amino acid. 
        /// </summary>
        public byte F { get; private set; }

        /// <summary>
        /// Gets the Glycine Amino acid.
        /// </summary>
        public byte G { get; private set; }

        /// <summary>
        /// Gets the Histidine Amino acid.
        /// </summary>
        public byte H { get; private set; }

        /// <summary>
        /// Gets the Isoleucine Amino acid.
        /// </summary>
        public byte I { get; private set; }

        /// <summary>
        /// Gets the Lysine Amino acid.
        /// </summary>
        public byte K { get; private set; }

        /// <summary>
        /// Gets the Leucine Amino acid.
        /// </summary>
        public byte L { get; private set; }

        /// <summary>
        /// Gets the Methionine Amino acid.
        /// </summary>
        public byte M { get; private set; }

        /// <summary>
        /// Gets the Asparagine Amino acid.
        /// </summary>
        public byte N { get; private set; }

        /// <summary>
        /// Gets the Pyrrolysine Amino acid.
        /// </summary>
        public byte O { get; private set; }

        /// <summary>
        /// Gets the Proline Amino acid.
        /// </summary>
        public byte P { get; private set; }

        /// <summary>
        /// Gets the Glutamine Amino acid.
        /// </summary>
        public byte Q { get; private set; }

        /// <summary>
        /// Gets the Arginine Amino acid.
        /// </summary>
        public byte R { get; private set; }

        /// <summary>
        /// Gets the Serine Amino acid.
        /// </summary>
        public byte S { get; private set; }

        /// <summary>
        /// Gets the Threoine Amino acid.
        /// </summary>
        public byte T { get; private set; }

        /// <summary>
        /// Gets the Selenocysteine Amino acid.
        /// </summary>
        public byte U { get; private set; }

        /// <summary>
        /// Gets the Valine Amino acid.
        /// </summary>
        public byte V { get; private set; }

        /// <summary>
        /// Gets the Tryptophan Amino acid.
        /// </summary>
        public byte W { get; private set; }

        /// <summary>
        /// Gets the Tyrosine Amino acid.
        /// </summary>
        public byte Y { get; private set; }

        /// <summary>
        /// Gets the Gap character.
        /// </summary>
        public byte Gap { get; private set; }

        /// <summary>
        /// Gets the Termination character.
        /// </summary>
        public byte Ter { get; private set; }

        /// <summary>
        /// Gets or sets the name of this alphabet - this is always 'Protein'.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this alphabet has a gap character.
        /// This alphabet does have a gap character.
        /// </summary>
        public bool HasGaps { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this alphabet has ambiguous characters.
        /// This alphabet does have ambiguous characters.
        /// </summary>
        public bool HasAmbiguity { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this alphabet has termination characters.
        /// This alphabet does have termination characters.
        /// </summary>
        public bool HasTerminations { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether complement is supported or not.
        /// </summary>
        public bool IsComplementSupported { get; protected set; }

        /// <summary>
        /// Instance of this class.
        /// </summary>
        public static readonly ProteinAlphabet Instance;

        /// <summary>
        /// Gets count of nucleotides.
        /// </summary>
        public int Count
        {
            get
            {
                return this.aminoAcids.Count;
            }
        }

        /// <summary>
        /// Gets the byte value of item at the given index.
        /// </summary>
        /// <param name="index">Index of the item to retrieve.</param>
        /// <returns>Byte value at the given index.</returns>
        public byte this[int index]
        {
            get
            {
                return this.aminoAcids[index];
            }
        }

        /// <summary>
        /// Gets the complement of the symbol.
        /// </summary>
        /// <param name="symbol">The protein symbol.</param>
        /// <param name="complementSymbol">The complement symbol.</param>
        /// <returns>Returns true if it gets the complements symbol.</returns>
        public bool TryGetComplementSymbol(byte symbol, out byte complementSymbol)
        {
            // Complement is not possible.
            complementSymbol = default(byte);
            return false;
        }

        /// <summary>
        /// This method tries to get the complements for specified symbols.
        /// </summary>
        /// <param name="symbols">Symbols to look up.</param>
        /// <param name="complementSymbols">Complement  symbols (output).</param>
        /// <returns>Returns true if found else false.</returns>
        public bool TryGetComplementSymbol(byte[] symbols, out byte[] complementSymbols)
        {
            complementSymbols = null;
            return false;
        }
        /// <summary>
        /// Gets the default Gap symbol.
        /// </summary>
        /// <param name="defaultGapSymbol">The default symbol.</param>
        /// <returns>Returns true if it gets the Default Gap Symbol.</returns>
        public virtual bool TryGetDefaultGapSymbol(out byte defaultGapSymbol)
        {
            defaultGapSymbol = this.Gap;
            return true;
        }

        /// <summary>
        /// Gets the default Termination symbol.
        /// </summary>
        /// <param name="defaultTerminationSymbol">The default Termination symbol.</param>
        /// <returns>Returns true if it gets the  default Termination symbol.</returns>
        public virtual bool TryGetDefaultTerminationSymbol(out byte defaultTerminationSymbol)
        {
            defaultTerminationSymbol = this.Ter;
            return true;
        }

        /// <summary>
        /// Gets the Gap symbol.
        /// </summary>
        /// <param name="gapSymbols">The Gap Symbol.</param>
        /// <returns>Returns true if it gets the  Gap symbol.</returns>
        public virtual bool TryGetGapSymbols(out HashSet<byte> gapSymbols)
        {
            gapSymbols = new HashSet<byte>();
            gapSymbols.Add(this.Gap);
            return true;
        }

        /// <summary>
        /// Gets the Termination symbol.
        /// </summary>
        /// <param name="terminationSymbols">The Termination symbol.</param>
        /// <returns>Returns true if it gets the Termination symbol.</returns>
        public virtual bool TryGetTerminationSymbols(out HashSet<byte> terminationSymbols)
        {
            terminationSymbols = new HashSet<byte>();
            terminationSymbols.Add(this.Ter);
            return true;
        }

        /// <summary>
        /// Gets the valid symbol.
        /// </summary>
        /// <returns>Returns HashSet of valid symbols.</returns>
        public HashSet<byte> GetValidSymbols()
        {
            return new HashSet<byte>(this.aminoAcidValueMap.Keys);
        }

        /// <summary>
        /// Gets the ambigious characters present in alphabet.
        /// </summary>
        public HashSet<byte> GetAmbiguousSymbols()
        {
            return new HashSet<byte>(this.ambiguousSyToBasicSymbolsMap.Keys);
        }

        /// <summary>
        /// Maps A to A  and a to A
        /// that is key will contain unique values.
        /// This will be used in the IsValidSymbol method to address Scenarios like a == A, G == g etc.
        /// </summary>
        public byte[] GetSymbolValueMap()
        {
            byte[] symbolMap = new byte[256];

            foreach (KeyValuePair<byte, byte> mapping in this.aminoAcidValueMap)
            {
                symbolMap[mapping.Key] = mapping.Value;
            }

            return symbolMap;
        }

        /// <summary>
        /// Gets the Ambiguous symbol.
        /// </summary>
        /// <param name="symbols">The symbol.</param>
        /// <param name="ambiguousSymbol">The Ambiguous symbol.</param>
        /// <returns>Returns true if it gets the Ambiguous symbol.</returns>
        public bool TryGetAmbiguousSymbol(HashSet<byte> symbols, out byte ambiguousSymbol)
        {
            return this.basicSymbolsToAmbiguousSymbolMap.TryGetValue(symbols, out ambiguousSymbol);
        }

        /// <summary>
        /// Gets the Basic symbol.
        /// </summary>
        /// <param name="ambiguousSymbol">The Ambiguous symbol.</param>
        /// <param name="basicSymbols">The Basic symbol.</param>
        /// <returns>Returns true if it gets the Basic symbol.</returns>
        public bool TryGetBasicSymbols(byte ambiguousSymbol, out HashSet<byte> basicSymbols)
        {
            return this.ambiguousSyToBasicSymbolsMap.TryGetValue(ambiguousSymbol, out basicSymbols);
        }

        /// <summary>
        /// Compares two symbols.
        /// </summary>
        /// <param name="x">The first symbol to compare.</param>
        /// <param name="y">The second symbol to compare.</param>
        /// <returns>Returns true if x equals y else false.</returns>
        public virtual bool CompareSymbols(byte x, byte y)
        {
            byte nucleotideA, nucleotideB;

            if (this.aminoAcidValueMap.TryGetValue(x, out nucleotideA))
            {
                if (this.aminoAcidValueMap.TryGetValue(y, out nucleotideB))
                {
                    if (this.ambiguousSyToBasicSymbolsMap.ContainsKey(nucleotideA) || this.ambiguousSyToBasicSymbolsMap.ContainsKey(nucleotideB))
                    {
                        return false;
                    }

                    return nucleotideA == nucleotideB;
                }
                else
                {
                    throw new ArgumentException(Properties.Resource.InvalidParameter, "y");
                }
            }
            else
            {
                throw new ArgumentException(Properties.Resource.InvalidParameter, "x");
            }
        }

        /// <summary>
        /// Find the consensus nucleotide for a set of nucleotides.
        /// </summary>
        /// <param name="symbols">Set of sequence items.</param>
        /// <returns>Consensus nucleotide.</returns>
        public virtual byte GetConsensusSymbol(HashSet<byte> symbols)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Validates if all symbols provided are Protein symbols or not.
        /// </summary>
        /// <param name="symbols">Symbols to be validated.</param>
        /// <param name="offset">Offset from where validation should start.</param>
        /// <param name="length">Number of symbols to validate from the specified offset.</param>
        /// <returns>True if the validation succeeds, else false.</returns>
        public bool ValidateSequence(byte[] symbols, long offset, long length)
        {
            if (symbols == null)
            {
                throw new ArgumentNullException("symbols");
            }

            for (long i = offset; i < length; i++)
            {
                if (!this.aminoAcidValueMap.ContainsKey(symbols[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the provided item is a gap character or not
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>True if the specified item is a gap</returns>
        public virtual bool CheckIsGap(byte item)
        {
            return item == this.Gap;
        }

        /// <summary>
        /// Checks if the provided item is an ambiguous character or not
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>True if the specified item is a ambiguous</returns>
        public virtual bool CheckIsAmbiguous(byte item)
        {
            return !basicSymbols.Contains(item);
        }

        /// <summary>
        /// Byte array of nucleotides.
        /// </summary>
        /// <returns>Returns the Enumerator for nucleotides list.</returns>
        public IEnumerator<byte> GetEnumerator()
        {
            return this.aminoAcids.GetEnumerator();
        }

        /// <summary>
        /// Creates an IEnumerator of the nucleotides.
        /// </summary>
        /// <returns>Returns Enumerator over alphabet values.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Adds a Amino acid to the existing amino acids.
        /// </summary>
        /// <param name="aminoAcidValue">Amino acid to be added.</param>
        /// <param name="otherPosibleValues">Maps Capital and small Letters.</param>
        protected void AddAminoAcid(byte aminoAcidValue, params byte[] otherPosibleValues)
        {
            // Verify whether the aminoAcidValue or other possible values already exist or not.
            if (this.aminoAcidValueMap.ContainsKey(aminoAcidValue) || otherPosibleValues.Any(x => this.aminoAcidValueMap.Keys.Contains(x)))
            {
                throw new ArgumentException(Properties.Resource.SymbolExistsInAlphabet, "aminoAcidValue");
            }

            this.aminoAcidValueMap.Add(aminoAcidValue, aminoAcidValue);
            foreach (byte value in otherPosibleValues)
            {
                this.aminoAcidValueMap.Add(value, aminoAcidValue);
            }

            this.aminoAcids.Add(aminoAcidValue);
        }

        /// <summary>
        /// Maps the ambiguous amino acids to the amino acids it represents. 
        /// For example ambiguous amino acids M represents the basic nucleotides A or C.
        /// </summary>
        /// <param name="ambiguousAminoAcid">Ambiguous amino acids.</param>
        /// <param name="aminoAcidsToMap">Nucleotide represented by ambiguous amino acids.</param>
        protected void MapAmbiguousAminoAcid(byte ambiguousAminoAcid, params byte[] aminoAcidsToMap)
        {
            byte ambiguousSymbol;

            // Verify whether the nucleotides to map are valid nucleotides.
            if (!this.aminoAcidValueMap.TryGetValue(ambiguousAminoAcid, out ambiguousSymbol) || !aminoAcidsToMap.All(x => this.aminoAcidValueMap.Keys.Contains(x)))
            {
                throw new ArgumentException(Properties.Resource.CouldNotRecognizeSymbol, "ambiguousAminoAcid");
            }

            byte[] mappingValues = new byte[aminoAcidsToMap.Length];
            int i = 0;
            foreach (byte valueToMap in aminoAcidsToMap)
            {
                mappingValues[i++] = this.aminoAcidValueMap[valueToMap];
            }

            HashSet<byte> basicSymbols = new HashSet<byte>(mappingValues);
            this.ambiguousSyToBasicSymbolsMap.Add(ambiguousSymbol, basicSymbols);
            this.basicSymbolsToAmbiguousSymbolMap.Add(basicSymbols, ambiguousSymbol);
        }
    }
}
