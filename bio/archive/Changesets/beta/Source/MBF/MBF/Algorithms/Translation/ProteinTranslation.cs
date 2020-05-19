// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Algorithms.Translation
{
    /// <summary>
    /// Provides the means of translating an RNA sequence into an Protein
    /// sequence of amino acids.
    /// </summary>
    public static class ProteinTranslation
    {
        /// <summary>
        /// Translates the RNA sequence passed in as source into a Protein
        /// sequence of amino acids. Works on the entire source sequence
        /// starting from the first triplet of nucleotides.
        /// </summary>
        public static ISequence Translate(ISequence source)
        {
            return Translate(source, 0);
        }

        /// <summary>
        /// Translates the RNA sequence passed in as a source into a Protein
        /// sequence of amino acids. Allows the setting of a particular index
        /// into the source sequence for the start of translation.
        /// 
        /// For instance if you wanted to translate all the phases of an RNA
        /// sequence you could perform the following:
        /// 
        /// Sequence rnaSeq = new Sequence(Alphabets.RNA), "AUGCGCCCG");
        /// Sequence phase1 = ProteinTranslation.Translate(rnaSeq, 0);
        /// Sequence phase2 = ProteinTranslation.Translate(rnaSeq, 1);
        /// Sequence phase3 = ProteinTranslation.Translate(rnaSeq, 2);
        /// </summary>
        /// <param name="source">The source RNA sequence to translate from</param>
        /// <param name="nucleotideOffset">
        /// An offset into the source sequence from which to begin translation.
        /// Note that this offset begins counting from 0. Set this parameter to
        /// 0 to translate the entire source sequence. Set it to 1 to ignore the
        /// first nucleotide in the source sequence, etc.
        /// </param>
        /// <returns></returns>
        public static ISequence Translate(ISequence source, int nucleotideOffset)
        {
            Sequence result = new Sequence(Alphabets.Protein);
            result.IsReadOnly = false;
            result.ID = "AA: " + source.ID;
            result.DisplayID = "Amino Acids translated from: " + source.DisplayID;

            for (int i = nucleotideOffset; i < source.Count - 2; i += 3)
            {
                result.Add(Codons.Lookup(source, i));
            }

            return result;
        }
    }
}
