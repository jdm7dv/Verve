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
using MBF.Util.Logging;

namespace MBF
{
    /// <summary>
    /// Provides DNA and RNA nucleotide complements.
    /// </summary>
    /// <remarks>
    /// Nucleotide complements are bases that bond with, and correspond to one
    /// another. For the DNA case, the correspondence is generally at a common
    /// location on the two strands of a DNA double helix. For RNA, the single
    /// strand can overlap and bond with itself in various ways to create
    /// secondary structure, and complementation will determine where this can happen.
    /// Static methods in the class provide lookup for single nucleotide
    /// complements, as well as creating the complementary strand for a given
    /// DNA sequence.
    /// </remarks>
    public static class Complementation
    {
        private static Dictionary<Nucleotide, Nucleotide> dnaToComplement = new Dictionary<Nucleotide, Nucleotide>();
        private static Dictionary<Nucleotide, Nucleotide> rnaToComplement = new Dictionary<Nucleotide, Nucleotide>();

        /// <summary>
        /// Returns the complement of a DNA nucleotide. This also
        /// respects ambiguous characters in the DNA alphabet.
        /// </summary>
        public static Nucleotide GetDnaComplement(Nucleotide dnaSource)
        {
            return dnaToComplement[dnaSource];
        }

        /// <summary>
        /// Returns the complement of an RNA nucleotide. This also
        /// respects ambiguous characters in the RNA alphabet.
        /// </summary>
        public static Nucleotide GetRnaComplement(Nucleotide rnaSource)
        {
            return rnaToComplement[rnaSource];
        }

        /// <summary>
        /// Transcribes a DNA sequence into the corresponding sequence on
        /// the other strand (which will have the opposite orientation). The length
        /// of the resulting sequence will equal the length of the source
        /// sequence. Gap and ambiguous characters will also be transcribed.
        /// </summary>
        /// <remarks>
        /// For example:
        /// 
        /// Sequence dna = new Sequence(Alphabets.DNA, "TACCGC");
        /// Sequence otherStrand = Complementarity.Complement(dna);
        ///
        /// otherStrand.ToString() would produce "ATGGCG"
        /// </remarks>
        /// <param name="dnaSource">The input sequence.</param>
        /// <returns>A new complementary sequence.</returns>
        public static ISequence Complement(ISequence dnaSource)
        {
            if (dnaSource.Alphabet != Alphabets.DNA)
            {
                string message = "Complement is only supported for DNA sequence.";
                Trace.Report(message);
                throw new NotSupportedException(message);
            }
            Sequence result = new Sequence(Alphabets.DNA);
            result.IsReadOnly = false;
            // avoid "Complement", already used in Transcription class
            result.ID = "Strand Complement: " + dnaSource.ID;
            result.DisplayID = "Strand Complement: " + dnaSource.DisplayID;

            foreach (Nucleotide n in dnaSource)
            {
                result.Add(GetDnaComplement(n));
            }
            result.IsReadOnly = true;
            return result;
        }

        /// <summary>
        /// Transcribes a DNA sequence into the corresponding sequence on
        /// the other strand, then reverses it so that the new sequence has
        /// the same orientation as the original (in the 3'-to-5' sense). The length
        /// of the resulting sequence will equal the length of the source
        /// sequence. Gap and ambiguous characters will also be transcribed.
        /// 
        /// For example:
        /// 
        /// Sequence dna = new Sequence(Alphabets.DNA, "TACCGC");
        /// Sequence otherStrand = Complementarity.ReverseComplement(dna);
        ///
        /// otherStrand.ToString() would produce "GCGGTA"
        /// </summary>
        /// <param name="dnaSource">The input sequence.</param>
        /// <returns>A new complementary sequence.</returns>
        public static ISequence ReverseComplement(ISequence dnaSource)
        {
            if (dnaSource.Alphabet != Alphabets.DNA)
            {
                string message = "ReverseComplement is only supported for DNA sequence.";
                Trace.Report(message);
                throw new NotSupportedException(message);
            }
            Sequence result = new Sequence(Alphabets.DNA);
            result.IsReadOnly = false;
            result.ID = "Reverse Complement: " + dnaSource.ID;
            result.DisplayID = "Reverse Complement: " + dnaSource.DisplayID;

            for (int iSymbol = dnaSource.Count - 1; iSymbol >= 0; --iSymbol)
            {
                Nucleotide n = (Nucleotide)dnaSource[iSymbol];
                result.Add(GetDnaComplement(n));
            }
            result.IsReadOnly = true;
            return result;
        }

        static Complementation()
        {
            // Fill the DNA complements
            dnaToComplement.Add(Alphabets.DNA.A, Alphabets.DNA.T);
            dnaToComplement.Add(Alphabets.DNA.T, Alphabets.DNA.A);
            dnaToComplement.Add(Alphabets.DNA.C, Alphabets.DNA.G);
            dnaToComplement.Add(Alphabets.DNA.G, Alphabets.DNA.C);
            dnaToComplement.Add(Alphabets.DNA.Gap, Alphabets.DNA.Gap);
            dnaToComplement.Add(Alphabets.DNA.Any, Alphabets.DNA.Any);
            dnaToComplement.Add(Alphabets.DNA.AC, Alphabets.DNA.GT);
            dnaToComplement.Add(Alphabets.DNA.AT, Alphabets.DNA.AT);
            dnaToComplement.Add(Alphabets.DNA.ACT, Alphabets.DNA.GAT);
            dnaToComplement.Add(Alphabets.DNA.GA, Alphabets.DNA.TC);
            dnaToComplement.Add(Alphabets.DNA.GC, Alphabets.DNA.GC);
            dnaToComplement.Add(Alphabets.DNA.GT, Alphabets.DNA.AC);
            dnaToComplement.Add(Alphabets.DNA.GAT, Alphabets.DNA.ACT);
            dnaToComplement.Add(Alphabets.DNA.GCA, Alphabets.DNA.GTC);
            dnaToComplement.Add(Alphabets.DNA.GTC, Alphabets.DNA.GCA);
            dnaToComplement.Add(Alphabets.DNA.TC, Alphabets.DNA.GA);

            // Fill the RNA complements
            rnaToComplement.Add(Alphabets.RNA.A, Alphabets.RNA.U);
            rnaToComplement.Add(Alphabets.RNA.U, Alphabets.RNA.A);
            rnaToComplement.Add(Alphabets.RNA.C, Alphabets.RNA.G);
            rnaToComplement.Add(Alphabets.RNA.G, Alphabets.RNA.C);
            rnaToComplement.Add(Alphabets.RNA.Gap, Alphabets.RNA.Gap);
            rnaToComplement.Add(Alphabets.RNA.Any, Alphabets.RNA.Any);
            rnaToComplement.Add(Alphabets.RNA.AC, Alphabets.RNA.GU);
            rnaToComplement.Add(Alphabets.RNA.AU, Alphabets.RNA.AU);
            rnaToComplement.Add(Alphabets.RNA.ACU, Alphabets.RNA.GAU);
            rnaToComplement.Add(Alphabets.RNA.GA, Alphabets.RNA.UC);
            rnaToComplement.Add(Alphabets.RNA.GC, Alphabets.RNA.GC);
            rnaToComplement.Add(Alphabets.RNA.GU, Alphabets.RNA.AC);
            rnaToComplement.Add(Alphabets.RNA.GAU, Alphabets.RNA.ACU);
            rnaToComplement.Add(Alphabets.RNA.GCA, Alphabets.RNA.GUC);
            rnaToComplement.Add(Alphabets.RNA.GUC, Alphabets.RNA.GCA);
            rnaToComplement.Add(Alphabets.RNA.UC, Alphabets.RNA.GA);
        }
    }
}
