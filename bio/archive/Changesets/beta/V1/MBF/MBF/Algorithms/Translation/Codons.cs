// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using System.Text;

namespace MBF.Algorithms.Translation
{
    /// <summary>
    /// A class which stores a table of mappings from triplets of RNA nucleotides
    /// to Amino Acids. This mapping comes from the standard Axiom of Genetics
    /// triplet rule. This class provides the basic lookup functionality from the
    /// codons. The ProteinTranslation class provides methods for translating
    /// whole RNA sequences.
    /// 
    /// In order to perform mapping from DNA, it is suggested that you first
    /// use the Transcription class to create the RNA sequence
    /// </summary>
    public static class Codons
    {
        private static Dictionary<string, AminoAcid> codonMap = new Dictionary<string, AminoAcid>();

        /// <summary>
        /// Lookup an amino acid based on a triplet of nucleotides. U U U for instance
        /// will result in Phenylalanine.
        /// </summary>
        public static AminoAcid Lookup(Nucleotide n1, Nucleotide n2, Nucleotide n3)
        {
            StringBuilder source = new StringBuilder();
            source.Append(n1.Symbol);
            source.Append(n2.Symbol);
            source.Append(n3.Symbol);
            return codonMap[source.ToString()];
        }

        /// <summary>
        /// Lookup an amino acid within a sequence starting a certian offset.
        /// </summary>
        /// <param name="sequence">The source sequence to lookup from.</param>
        /// <param name="offset">
        /// The offset within the sequence from which to look at the next three
        /// nucleotides. Note that this offset begins its count at zero. Thus
        /// looking at a sequence described by "AUGGCG" and using a offset of 0
        /// would lookup the amino acid for codon "AUG" while using a offset of 1
        /// would lookup the amino acid for codon "UGG".
        /// </param>
        /// <returns>An amino acid from the protein alphabet</returns>
        public static AminoAcid Lookup(ISequence sequence, int offset)
        {
            return Lookup((Nucleotide)sequence[offset], (Nucleotide)sequence[offset + 1], (Nucleotide)sequence[offset + 2]);
        }

        static Codons()
        {
            // Initialize the basic codon map from mRNA codes to Amino Acids
            codonMap.Add("UUU", Alphabets.Protein.Phe);
            codonMap.Add("UUC", Alphabets.Protein.Phe);
            codonMap.Add("UUA", Alphabets.Protein.Leu);
            codonMap.Add("UUG", Alphabets.Protein.Leu);

            codonMap.Add("UCU", Alphabets.Protein.Ser);
            codonMap.Add("UCC", Alphabets.Protein.Ser);
            codonMap.Add("UCA", Alphabets.Protein.Ser);
            codonMap.Add("UCG", Alphabets.Protein.Ser);

            codonMap.Add("UAU", Alphabets.Protein.Tyr);
            codonMap.Add("UAC", Alphabets.Protein.Tyr);
            codonMap.Add("UAA", Alphabets.Protein.Term);
            codonMap.Add("UAG", Alphabets.Protein.Term);

            codonMap.Add("UGU", Alphabets.Protein.Cys);
            codonMap.Add("UGC", Alphabets.Protein.Cys);
            codonMap.Add("UGA", Alphabets.Protein.Term);
            codonMap.Add("UGG", Alphabets.Protein.Trp);

            codonMap.Add("CUU", Alphabets.Protein.Leu);
            codonMap.Add("CUC", Alphabets.Protein.Leu);
            codonMap.Add("CUA", Alphabets.Protein.Leu);
            codonMap.Add("CUG", Alphabets.Protein.Leu);

            codonMap.Add("CCU", Alphabets.Protein.Pro);
            codonMap.Add("CCC", Alphabets.Protein.Pro);
            codonMap.Add("CCA", Alphabets.Protein.Pro);
            codonMap.Add("CCG", Alphabets.Protein.Pro);

            codonMap.Add("CAU", Alphabets.Protein.His);
            codonMap.Add("CAC", Alphabets.Protein.His);
            codonMap.Add("CAA", Alphabets.Protein.Gln);
            codonMap.Add("CAG", Alphabets.Protein.Gln);

            codonMap.Add("CGU", Alphabets.Protein.Arg);
            codonMap.Add("CGC", Alphabets.Protein.Arg);
            codonMap.Add("CGA", Alphabets.Protein.Arg);
            codonMap.Add("CGG", Alphabets.Protein.Arg);

            codonMap.Add("AUU", Alphabets.Protein.Ile);
            codonMap.Add("AUC", Alphabets.Protein.Ile);
            codonMap.Add("AUA", Alphabets.Protein.Ile);
            codonMap.Add("AUG", Alphabets.Protein.Met);

            codonMap.Add("ACU", Alphabets.Protein.Thr);
            codonMap.Add("ACC", Alphabets.Protein.Thr);
            codonMap.Add("ACA", Alphabets.Protein.Thr);
            codonMap.Add("ACG", Alphabets.Protein.Thr);

            codonMap.Add("AAU", Alphabets.Protein.Asn);
            codonMap.Add("AAC", Alphabets.Protein.Asn);
            codonMap.Add("AAA", Alphabets.Protein.Lys);
            codonMap.Add("AAG", Alphabets.Protein.Lys);

            codonMap.Add("AGU", Alphabets.Protein.Ser);
            codonMap.Add("AGC", Alphabets.Protein.Ser);
            codonMap.Add("AGA", Alphabets.Protein.Arg);
            codonMap.Add("AGG", Alphabets.Protein.Arg);

            codonMap.Add("GUU", Alphabets.Protein.Val);
            codonMap.Add("GUC", Alphabets.Protein.Val);
            codonMap.Add("GUA", Alphabets.Protein.Val);
            codonMap.Add("GUG", Alphabets.Protein.Val);

            codonMap.Add("GCU", Alphabets.Protein.Ala);
            codonMap.Add("GCC", Alphabets.Protein.Ala);
            codonMap.Add("GCA", Alphabets.Protein.Ala);
            codonMap.Add("GCG", Alphabets.Protein.Ala);

            codonMap.Add("GAU", Alphabets.Protein.Asp);
            codonMap.Add("GAC", Alphabets.Protein.Asp);
            codonMap.Add("GAA", Alphabets.Protein.Glu);
            codonMap.Add("GAG", Alphabets.Protein.Glu);

            codonMap.Add("GGU", Alphabets.Protein.Gly);
            codonMap.Add("GGC", Alphabets.Protein.Gly);
            codonMap.Add("GGA", Alphabets.Protein.Gly);
            codonMap.Add("GGG", Alphabets.Protein.Gly);
        }
    }
}
