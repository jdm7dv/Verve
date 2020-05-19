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



using System.Collections.Generic;

namespace Bio
{
    /// <summary>
    /// An alphabet defines a set of characters common to a particular representation
    /// of a biological sequence. The most common alphabets are DNA and RNA. The items
    /// in these alphabets are those you would find as the individual sequence items in
    /// an ISequence variable.
    /// <para>
    /// The characters in an alphabet may represent a particular biological structure
    /// or they may represent information helpful in understanding a sequence. For instance
    /// gap characters, termination characters, and characters representing items whose
    /// definition remains ambigious are all allowed.
    /// </para>
    /// </summary>
    public interface IAlphabet : ICollection<ISequenceItem>
    {
        /// <summary>
        /// Gets a human readable name for the alphabet. 
        /// For example "DNA", "RNA", or "Amino Acid".
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a value indicating whether the encoding has one or more characters 
        /// that represent a gap.
        /// </summary>
        bool HasGaps { get; }

        /// <summary>
        /// Gets a value indicating whether the encoding has one or more characters 
        /// that represent an ambigous item (i.e. and item for which it is not 
        /// precisely known what it represents)
        /// </summary>
        bool HasAmbiguity { get; }

        /// <summary>
        /// Gets a value indicating whether the encoding has one or more characters 
        /// that represent terminal items.
        /// </summary>
        bool HasTerminations { get; }

        /// <summary>
        /// Gets the nucleotide / amino acid corresponding to default gap character in alphabet
        /// </summary>
        ISequenceItem DefaultGap { get; }

        /// <summary>
        /// Returns one of the items in the encoding based on the character
        /// symbol of that item.
        /// </summary>
        /// <param name="symbol">The character symbol to look up, such as 'G' for Guanine.</param>
        /// <returns>ISequenceItem corresponding to input symbol</returns>
        ISequenceItem LookupBySymbol(char symbol);

        /// <summary>
        /// Returns one of the items in the encoding based on the symbol of
        /// that item. If the string is only composed of one character then the
        /// result should be the same as calling the char overload of this method.
        /// For instance in an amino acid encoding "Ala" or "A" could return the
        /// item representing Alanine.
        /// </summary>
        /// <param name="symbol">The string symbol to look up, such as "G" or "Gua" for Guanine.</param>
        /// <returns>ISequenceItem corresponding to input symbol</returns>
        ISequenceItem LookupBySymbol(string symbol);

        /// <summary>
        /// Find the consensus symbol for a set of sequence items
        /// </summary>
        /// <param name="symbols">Set of sequence items</param>
        /// <returns>Consensus sequence item</returns>
        ISequenceItem GetConsensusSymbol(HashSet<ISequenceItem> symbols);

        /// <summary>
        /// Find the set of symbols that is represented by input symbol
        /// </summary>
        /// <param name="symbol">Symbol to look up</param>
        /// <returns>Set of symbols</returns>
        HashSet<ISequenceItem> GetBasicSymbols(ISequenceItem symbol);

        /// <summary>
        /// Returns a list of all of the stored symbols filtered by the specified parameters
        /// </summary>
        /// <param name="includeBasics">Include the basic items of the alphabet (e.g. in DNA: G, A, T, and C)</param>
        /// <param name="includeGaps">Include the gap items if any (e.g. -)</param>
        /// <param name="includeAmbiguities">Include the ambiguity items if any (e.g. in DNA: GA, GAT, GC, etc.)</param>
        /// <param name="includeTerminations">Include the termination item if any (e.g. *)</param>
        /// <returns>List of all stored items matching parameters</returns>
        List<ISequenceItem> LookupAll(bool includeBasics, bool includeGaps, bool includeAmbiguities, bool includeTerminations);
    }
}
