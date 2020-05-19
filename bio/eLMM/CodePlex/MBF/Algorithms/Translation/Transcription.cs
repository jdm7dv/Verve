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

namespace Bio.Algorithms.Translation
{
    /// <summary>
    /// Provides basic nucleotide transcription across DNA and RNA sequences.
    /// Static methods in the class provide lookup for single nucleotide
    /// complements as well as creating RNA (transcription) or DNA (reverse
    /// transcription) from a DNA or RNA sequence, respectively.
    /// </summary>
    public class Transcription
    {
        private static Dictionary<Nucleotide, Nucleotide> dnaToRna = new Dictionary<Nucleotide, Nucleotide>();
        private static Dictionary<Nucleotide, Nucleotide> rnaToDna = new Dictionary<Nucleotide, Nucleotide>();

        /// <summary>
        /// Returns the complement nucleotide from DNA to RNA. This also
        /// respects ambiguous characters in the DNA and RNA alphabet.
        /// </summary>
        public static Nucleotide GetRnaComplement(Nucleotide dnaSource)
        {
            return dnaToRna[dnaSource];
        }

        /// <summary>
        /// Returns the complement nucleotide from RNA to DNA. This also
        /// respects ambiguous characters in the DNA and RNA alphabet.
        /// </summary>
        public static Nucleotide GetDnaComplement(Nucleotide rnaSource)
        {
            return rnaToDna[rnaSource];
        }

        /// <summary>
        /// Transcribes a DNA sequence into an RNA sequence. The length
        /// of the resulting sequence will equal the length of the source
        /// sequence. Gap and ambiguous characters will also be transcribed.
        /// 
        /// For example:
        /// 
        /// Sequence dna = new Sequence(Alphabets.DNA, "TACCGC");
        /// Sequence rna = Transcription.Transcribe(dna);
        ///
        /// rna.ToString() would produce "AUGGCG"
        /// </summary>
        public static ISequence Transcribe(ISequence dnaSource)
        {
            Sequence result = new Sequence(Alphabets.RNA);
            result.ID = "Complement: " + dnaSource.ID;
            result.DisplayID = "Complement: " + dnaSource.DisplayID;
            result.IsReadOnly = false;
            foreach (Nucleotide n in dnaSource)
            {
                result.Add(GetRnaComplement(n));
            }
            result.IsReadOnly = true;
            return result;
        }

        /// <summary>
        /// Does reverse transcription from an RNA sequence into an DNA sequence.
        /// The length of the resulting sequence will equal the length of the source
        /// sequence. Gap and ambiguous characters will also be transcribed.
        /// 
        /// For example:
        /// 
        /// Sequence rna = new Sequence(Alphabets.RNA, "UACCGC");
        /// Sequence dna = Transcription.ReverseTranscribe(rna);
        ///
        /// dna.ToString() would produce "ATGGCG"
        /// </summary>
        public static ISequence ReverseTranscribe(ISequence rnaSource)
        {
            Sequence result = new Sequence(Alphabets.DNA);
            result.ID = "Complement: " + rnaSource.ID;
            result.DisplayID = "Complement: " + rnaSource.DisplayID;
            result.IsReadOnly = false;
            foreach (Nucleotide n in rnaSource)
            {
                result.Add(GetDnaComplement(n));
            }
            result.IsReadOnly = true;
            return result;
        }

        static Transcription()
        {
            // Fill the DNA to RNA components
            dnaToRna.Add(Alphabets.DNA.A, Alphabets.RNA.U);
            dnaToRna.Add(Alphabets.DNA.T, Alphabets.RNA.A);
            dnaToRna.Add(Alphabets.DNA.C, Alphabets.RNA.G);
            dnaToRna.Add(Alphabets.DNA.G, Alphabets.RNA.C);
            dnaToRna.Add(Alphabets.DNA.Gap, Alphabets.RNA.Gap);
            dnaToRna.Add(Alphabets.DNA.Any, Alphabets.RNA.Any);
            dnaToRna.Add(Alphabets.DNA.AC, Alphabets.RNA.GU);
            dnaToRna.Add(Alphabets.DNA.AT, Alphabets.RNA.AU);
            dnaToRna.Add(Alphabets.DNA.ACT, Alphabets.RNA.GAU);
            dnaToRna.Add(Alphabets.DNA.GA, Alphabets.RNA.UC);
            dnaToRna.Add(Alphabets.DNA.GC, Alphabets.RNA.GC);
            dnaToRna.Add(Alphabets.DNA.GT, Alphabets.RNA.AC);
            dnaToRna.Add(Alphabets.DNA.GAT, Alphabets.RNA.ACU);
            dnaToRna.Add(Alphabets.DNA.GCA, Alphabets.RNA.GUC);
            dnaToRna.Add(Alphabets.DNA.GTC, Alphabets.RNA.GCA);
            dnaToRna.Add(Alphabets.DNA.TC, Alphabets.RNA.GA);

            // Fill the RNA to DNA components
            rnaToDna.Add(Alphabets.RNA.A, Alphabets.DNA.T);
            rnaToDna.Add(Alphabets.RNA.U, Alphabets.DNA.A);
            rnaToDna.Add(Alphabets.RNA.C, Alphabets.DNA.G);
            rnaToDna.Add(Alphabets.RNA.G, Alphabets.DNA.C);
            rnaToDna.Add(Alphabets.RNA.Gap, Alphabets.DNA.Gap);
            rnaToDna.Add(Alphabets.RNA.Any, Alphabets.DNA.Any);
            rnaToDna.Add(Alphabets.RNA.AC, Alphabets.DNA.GT);
            rnaToDna.Add(Alphabets.RNA.AU, Alphabets.DNA.AT);
            rnaToDna.Add(Alphabets.RNA.ACU, Alphabets.DNA.GAT);
            rnaToDna.Add(Alphabets.RNA.GA, Alphabets.DNA.TC);
            rnaToDna.Add(Alphabets.RNA.GC, Alphabets.DNA.GC);
            rnaToDna.Add(Alphabets.RNA.GU, Alphabets.DNA.AC);
            rnaToDna.Add(Alphabets.RNA.GAU, Alphabets.DNA.ACT);
            rnaToDna.Add(Alphabets.RNA.GCA, Alphabets.DNA.GTC);
            rnaToDna.Add(Alphabets.RNA.GUC, Alphabets.DNA.GCA);
            rnaToDna.Add(Alphabets.RNA.UC, Alphabets.DNA.GA);
        }
    }
}
