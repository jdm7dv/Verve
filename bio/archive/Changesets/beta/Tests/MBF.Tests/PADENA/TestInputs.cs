// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using System.Linq;

namespace MBF.Tests
{
    /// <summary>
    /// Contains test inputs for steps in PaDeNA algorithm. 
    /// </summary>
    public static class TestInputs
    {
        /// <summary>
        /// Sequence Reads for unit tests
        /// </summary>
        /// <returns>List of sequences</returns>
        public static List<ISequence> GetTinyReads()
        {
            List<string> reads = new List<string>();
            reads.Add("ATGCC");
            reads.Add("TCCTA");
            reads.Add("CCTATC");
            reads.Add("TGCCTCC");
            reads.Add("CCTCCT");
            return new List<ISequence>(reads.Select(r => new Sequence(Alphabets.DNA, r)));
        }

        /// <summary>
        /// Sequence Reads for unit tests
        /// </summary>
        /// <returns>List of sequences</returns>
        public static List<ISequence> GetSmallReads()
        {
            // Sequence to assemble: GATGCCTCCTATCGATCGTCGATGC
            List<string> reads = new List<string>();
            reads.Add("GATGCCTCCTAT");
            reads.Add("CCTCCTATCGA");
            reads.Add("TCCTATCGATCGT");
            reads.Add("ATCGTCGATGC");
            reads.Add("TCCTATCGATCGTC");
            reads.Add("TGCCTCCTATCGA");
            reads.Add("TATCGATCGTCGA");
            reads.Add("TCGATCGTCGATGC");
            reads.Add("GCCTCCTATCGA");
            return new List<ISequence>(reads.Select(r => new Sequence(Alphabets.DNA, r)));
        }

        /// <summary>
        /// Sequence reads for testing dangling links purger
        /// </summary>
        /// <returns>List of sequences</returns>
        public static List<ISequence> GetDanglingReads()
        {
            // Sequence to assemble: ATCGCTAGCATCGAACGATCATTA
            List<string> reads = new List<string>();
            reads.Add("ATCGCTAGCATCG");
            reads.Add("CTAGCATCGAAC");
            reads.Add("CATCGAACGATCATT");
            reads.Add("GCTAGCATCGAAC");
            reads.Add("CGCTAGCATCGAA");
            reads.Add("ATCGAACGATGA"); // ATCGAACGATCA: SNP introduced to create dangling link
            reads.Add("CTAGCATCGAACGATC");
            reads.Add("ATCGCTAGCATCGAA");
            reads.Add("GCTAGCATCGAACGAT");
            reads.Add("AGCATCGAACGATCAT");
            return new List<ISequence>(reads.Select(r => (ISequence)new Sequence(Alphabets.DNA, r)));
        }

        /// <summary>
        /// Sequence reads for testing redundant paths purger
        /// </summary>
        /// <returns>List of sequence</returns>
        public static List<ISequence> GetRedundantPathReads()
        {
            // Sequence to assemble: ATGCCTCCTATCTTAGCGATGCGGTGT
            List<string> reads = new List<string>();
            reads.Add("ATGCCTCCTAT");
            reads.Add("CCTCCTATCTT");
            reads.Add("TCCTATCTT");
            reads.Add("TGCCTCCTATC");
            reads.Add("GCCTCCTATCTT");
            reads.Add("CTTAGCGATG");
            reads.Add("CTATCTTAGCGAT");
            reads.Add("CTATCTTAGC");
            reads.Add("GCCTCGTATCT"); // GCCTCCTATCT: SNP introduced to create bubble
            reads.Add("AGCGATGCGGTGT");
            reads.Add("TATCTTAGCGATGC");
            reads.Add("ATCTTAGCGATGC");
            reads.Add("TTAGCGATGCGG");
            return new List<ISequence>(reads.Select(r => (ISequence)new Sequence(Alphabets.DNA, r)));
        }

        /// <summary>
        /// Gets reads required for scaffolds.
        /// </summary>
        public static List<ISequence> GetReadsForScaffolds()
        {
            List<ISequence> sequences = new List<ISequence>();
            Sequence seq = new Sequence(Alphabets.DNA, "ATGCCTC");
            seq.DisplayID = ">10.x1:abc";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "CCTCCTAT");
            seq.DisplayID = "1";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TCCTATC");
            seq.DisplayID = "2";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TGCCTCCT");
            seq.DisplayID = "3";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "ATCTTAGC");
            seq.DisplayID = "4";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "CTATCTTAG");
            seq.DisplayID = "5";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "CTTAGCG");
            seq.DisplayID = "6";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "GCCTCCTAT");
            seq.DisplayID = ">8.x1:abc";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TAGCGCGCTA");
            seq.DisplayID = ">8.y1:abc";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "AGCGCGC");
            seq.DisplayID = ">9.x1:abc";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTTT");
            seq.DisplayID = "7";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTTAAA");
            seq.DisplayID = "8";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TAAAAA");
            seq.DisplayID = "9";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTTAG");
            seq.DisplayID = "10";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "TTTAGC");
            seq.DisplayID = "11";
            sequences.Add(seq);
            seq = new Sequence(Alphabets.DNA, "GCGCGCCGCGCG");
            seq.DisplayID = "12";
            sequences.Add(seq);
            return sequences;
        }
    }
}