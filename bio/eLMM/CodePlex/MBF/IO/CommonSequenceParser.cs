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
using System.Linq;

namespace Bio.IO
{
    /// <summary>
    /// This is a class that provides some basic operations common to sequence
    /// parsers. It is meant to be used as private memeber inside the parser implementations
    /// if the implementer wants to make use of some common behavior across parsers.
    /// </summary>
    public class CommonSequenceParser
    {
        #region Fields
        /// <summary>
        /// Holds distinct symbols while parsing the sequence, used to 
        /// identify alphabet for the sequence.
        /// </summary>
        private IEnumerable<char> _distinctSymbols;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor chooses default encoding based on alphabet.
        /// </summary>
        public CommonSequenceParser()
        {
        }
        #endregion
        
        #region Public Methods
        /// <summary>
        /// Identifies Alphabet for the sepecified sequence.
        /// </summary>
        /// <param name="currentAlphabet">Currently known alphabet of the sequence, null if alphabet is unknown.</param>
        /// <param name="sequence">Sequence data.</param>
        /// <returns>Returns appropriate alphabet for the specified sequence and considering the specified current alphabet. 
        /// Returns null if any character in the sequence is unrecognized by DNA, RNA and Protien Alphabets.</returns>
        public IAlphabet IdentifyAlphabet(IAlphabet currentAlphabet, string sequence)
        {
            if (string.IsNullOrEmpty(sequence))
            {
                return null;
            }

            bool canClearDistinctSymbol = false;
            if (_distinctSymbols != null)
            {
                _distinctSymbols = _distinctSymbols.Union(sequence.ToCharArray().Distinct()).ToList();
            }
            else
            {
                canClearDistinctSymbol = true;
                _distinctSymbols = sequence.ToCharArray().Distinct().ToList();
            }

            IAlphabet alphabet = null;

            if (currentAlphabet == Alphabets.Protein)
            {
                alphabet = StartCheckFromProtein();
            }
            else if (currentAlphabet == Alphabets.RNA)
            {
                alphabet = StartCheckFromRna();
            }
            else
            {
                alphabet = StartCheckFromDna();
            }

            if (canClearDistinctSymbol)
            {
                _distinctSymbols = null;
            }

            return alphabet;
        }

        /// <summary>
        /// Maps the string to a particular Molecule type and returns
        /// the instance of mapped molecule type.
        /// </summary>
        /// <param name="type">The molecule type.</param>
        /// <returns>Returns the appropriate molecule type for the specified string.</returns>
        public static MoleculeType GetMoleculeType(string type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            type = type.ToUpperInvariant();
            switch (type)
            {
                case "DNA":
                    return MoleculeType.DNA;
                case "NA":
                    return MoleculeType.NA;
                case "RNA":
                    return MoleculeType.RNA;
                case "TRNA":
                    return MoleculeType.tRNA;
                case "RRNA":
                    return MoleculeType.rRNA;
                case "MRNA":
                    return MoleculeType.mRNA;
                case "URNA":
                    return MoleculeType.uRNA;
                case "SNRNA":
                    return MoleculeType.snRNA;
                case "SNORNA":
                    return MoleculeType.snoRNA;
                case "PROTEIN":
                    return MoleculeType.Protein;
                default:
                    return MoleculeType.Invalid;
            }
        }

        /// <summary>
        /// Returns Molecule type depending on the specified alphabet.
        /// </summary>
        /// <param name="alphabet">Alphabet.</param>
        /// <returns>Returns molecule type.</returns>
        public static MoleculeType GetMoleculeType(IAlphabet alphabet)
        {
            if (alphabet == Alphabets.DNA)
            {
                return MoleculeType.DNA;
            }
            else if (alphabet == Alphabets.RNA)
            {
                return MoleculeType.RNA;
            }
            else if (alphabet == Alphabets.Protein)
            {
                return MoleculeType.Protein;
            }
            else
            {
                return MoleculeType.Invalid;
            }
        }

        /// <summary>
        /// Returns the alphabet depending on the specified molecule type.
        /// </summary>
        /// <param name="moleculeType">Molecule type.</param>
        /// <returns>IAlphabet instance.</returns>
        public static IAlphabet GetAlphabet(MoleculeType moleculeType)
        {
            switch (moleculeType)
            {
                case MoleculeType.DNA:
                case MoleculeType.NA:
                    return Alphabets.DNA;
                case MoleculeType.RNA:
                case MoleculeType.tRNA:
                case MoleculeType.rRNA:
                case MoleculeType.mRNA:
                case MoleculeType.uRNA:
                case MoleculeType.snRNA:
                case MoleculeType.snoRNA:
                    return Alphabets.RNA;
                case MoleculeType.Protein:
                    return Alphabets.Protein;
                default:
                    return null;
            }
        }
        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Returns Dna alphabet if all the symbols in distinctSymbols are 
        /// known by Dna alphabet else it continue to verify with Rna alpabet by 
        /// calling StartCheckFromRna method.
        /// </summary>
        /// <returns>If success then returns an instance of IAlphabet else returns null.</returns>
        private IAlphabet StartCheckFromDna()
        {
            if (!IsDnaAlphabet(_distinctSymbols))
            {
                return StartCheckFromRna();
            }

            return Alphabets.DNA;
        }

        /// <summary>
        /// Returns Rna alphabet if all the symbols in distinctSymbols are 
        /// known by Rna alphabet else it continue to verify with Protein alpabet by 
        /// calling StartCheckFromProtein method.
        /// </summary>
        /// <returns>If success then returns an instance of IAlphabet else returns null.</returns>
        private IAlphabet StartCheckFromRna()
        {
            if (!IsRnaAlphabet(_distinctSymbols))
            {
                return StartCheckFromProtein();
            }

            return Alphabets.RNA;
        }

        /// <summary>
        /// Returns Protein alphabet if all the symbols in distinctSymbols are 
        /// known by protein alphabet else returns null.
        /// </summary>
        /// <returns>If all symbols in distinctSymbols are known by protein alphabet 
        /// then returns protein Alphabet else returns null.</returns>
        private IAlphabet StartCheckFromProtein()
        {
            if (!IsProteinAlphabet(_distinctSymbols))
            {
                return null;
            }

            return Alphabets.Protein;
        }

        /// <summary>
        /// Returns true if all symbols in the specified list are known by Dna.
        /// </summary>
        /// <param name="characters">List of symbols.</param>
        /// <returns>True if all symbols are known else returns false.</returns>
        private static bool IsDnaAlphabet(IEnumerable<char> characters)
        {
            foreach (char ch in characters)
            {
                if (Alphabets.DNA.LookupBySymbol(ch) == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if all symbols in the specified list are known by Rna.
        /// </summary>
        /// <param name="characters">List of symbols.</param>
        /// <returns>True if all symbols are known else returns false.</returns>
        private static bool IsRnaAlphabet(IEnumerable<char> characters)
        {
            foreach (char ch in characters)
            {
                if (Alphabets.RNA.LookupBySymbol(ch) == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if all symbols in the specified list are known by Protein.
        /// </summary>
        /// <param name="characters">List of symbols.</param>
        /// <returns>True if all symbols are known else returns false.</returns>
        private static bool IsProteinAlphabet(IEnumerable<char> characters)
        {
            foreach (char ch in characters)
            {
                if (Alphabets.Protein.LookupBySymbol(ch) == null)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion Private Methods
    }
}
