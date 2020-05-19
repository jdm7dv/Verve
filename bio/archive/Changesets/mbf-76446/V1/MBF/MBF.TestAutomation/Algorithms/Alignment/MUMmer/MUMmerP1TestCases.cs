// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * MUMmerP1TestCases.cs
 * 
 *   This file contains the MUMmer Priority one test cases
 * 
***************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

using MBF.IO.Fasta;
using MBF.IO.GenBank;
using MBF.IO.Gff;
using MBF.Algorithms;
using MBF.Algorithms.Alignment;
using MBF.Algorithms.SuffixTree;
using MBF.SimilarityMatrices;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.Algorithms.Alignment
{
    /// <summary>
    /// MUMmer Priority One Test case implementation.
    /// </summary>
    [TestFixture]
    public class MUMmerP1TestCases
    {
        #region Enums

        /// <summary>
        /// LIS Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum LISParameters
        {
            FindUniqueMatches,
            PerformLIS,
        };

        /// <summary>
        /// Parser Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum ParserParameters
        {
            FastA,
            GenBank,
            Gff
        };

        /// <summary>
        /// Additional Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AdditionalParameters
        {
            PerformSimilarityMatrixChange,
            PerformAlgorithmChange,
            Other
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static MUMmerP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\MUMmerTestsConfig.xml");
        }

        #endregion Constructor

        #region Suffix Tree Test cases

        /// <summary>
        /// Validate BuildSuffixTree() method with Dna sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : Dna sequence file
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [Test]
        public void SuffixTreeBuildSuffixTreeDnaSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.DnaSequenceNodeName, true);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with Rna sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : Rna sequence file
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [Test]
        public void SuffixTreeBuildSuffixTreeRnaSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.RnaSequenceNodeName, true);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with Protein sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : Protein sequence file
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        public void SuffixTreeBuildSuffixTreeProteinSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.ProteinSequenceNodeName, true);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with Medium size sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : Medium size sequence file
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [Test]
        public void SuffixTreeBuildSuffixTreeMediumSizeSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.MediumSizeSequenceNodeName, true);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with ContinousRepeating Characters 
        /// and validate the nodes, edges and the sequence
        /// Input : Continuous Repeating Characters
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [Test]
        public void SuffixTreeBuildSuffixTreeContinousRepeatingCharacters()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.OneLineRepeatingCharactersNodeName, false);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with Alternate Repeating Characters 
        /// and validate the nodes, edges and the sequence
        /// Input : Alternate Repeating Characters
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [Test]
        public void SuffixTreeBuildSuffixTreeAlternateRepeatingCharacters()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.OneLineAlternateRepeatingCharactersNodeName,
                false);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with Fasta File sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : Dna sequence Fasta file
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [Test]
        public void SuffixTreeBuildSuffixTreeFastaFileSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.SimpleDnaFastaNodeName, true,
                ParserParameters.FastA);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with GenBank File sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : Dna sequence GenBank file
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [Test]
        public void SuffixTreeBuildSuffixTreeGenBankFileSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.SimpleDnaGenBankNodeName, true,
                ParserParameters.GenBank);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with Gff File sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : Dna sequence Gff file
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [Test]
        public void SuffixTreeBuildSuffixTreeGffFileSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.SimpleDnaGffNodeName, true,
                ParserParameters.Gff);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with Only Repeating Characters 
        /// and validate the nodes, edges and the sequence
        /// Input : Only Repeating Characters
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [Test]
        public void SuffixTreeBuildSuffixTreeOnlyRepeatingCharacters()
        {
            ValidateBuildSuffixTreeGeneralTestCases(
                Constants.OneLineOnlyRepeatingCharactersNodeName, false);
        }

        /// <summary>
        /// Validate FindMatches() method with valid MUM length 
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : Valid mum length with both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesValidMumLengthSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineSequenceNodeName,
                false, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with valid Dna sequence 
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : Dna sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesDnaSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.DnaSequenceNodeName,
                true, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with valid Rna sequence 
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : Rna sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesRnaSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.RnaSequenceNodeName,
                true, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with valid Medium size sequence 
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : Medium Size sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesMediumSizeSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.MediumSizeSequenceNodeName,
                true, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with continuous repeating sequence 
        /// for reference and valid Sequence for query parameter and validate
        /// the unique matches
        /// Input : continuous repeating sequence for reference and valid sequence for query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesContinousRepeatingSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineRepeatingCharactersNodeName,
                false, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with same sequence 
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : same sequence for reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesSameSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineSameCharactersNodeName,
                false, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with over lap sequence 
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : over lap sequence for reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesOverlapSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineOverlapSequenceNodeName,
                false, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with no match 
        /// for reference and query parameter and validate
        /// there are no unique matches
        /// Input : over lap sequence for reference and query parameter
        /// Validation : Validate there are no unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesNoMatchSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineNoMatchSequenceNodeName,
                false, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with cross over lap sequence 
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : cross over lap sequence for reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesCrossoverlapSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.SmallSizeSequenceNodeName,
                true, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with Dna valid sequence 
        /// for reference and Ambiguity characters in sequence for query parameter
        /// and validate the unique matches
        /// Input : valid Dna sequence for reference and ambiguity characters for query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesDnaSearchAmbiguitySequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.DnaSearchAmbiguitySequenceNodeName,
                true, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with Rna valid sequence 
        /// for reference and Ambiguity characters in sequence for query parameter
        /// and validate the unique matches
        /// Input : valid Rna sequence for reference and ambiguity characters for query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesRnaSearchAmbiguitySequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.RnaSearchAmbiguitySequenceNodeName,
                true, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with Dna valid sequence 
        /// with Ambiguity characters for reference and valid sequence for query parameter
        /// and validate the unique matches
        /// Input : valid Dna sequence with ambiguity characters for reference 
        /// and valid sequence for query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesDnaQueryAmbiguitySequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.DnaQueryAmbiguitySequenceNodeName,
                true, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with Rna valid sequence 
        /// with Ambiguity characters for reference and valid sequence for query parameter
        /// and validate the unique matches
        /// Input : valid Rna sequence with ambiguity characters for reference 
        /// and valid sequence for query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesRnaQueryAmbiguitySequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.RnaQueryAmbiguitySequenceNodeName,
                true, LISParameters.FindUniqueMatches);
        }

        #endregion Suffix Tree Test cases

        #region Longest Increasing Sub Sequence

        /// <summary>
        /// Validate GetLongestSequence() method with more than two unique matches
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : One sequence for both reference and query parameter with more than 
        /// two unique matches
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void LISMoreThanTwoUniqueMatch()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineMoreThanTwoMatchSequenceNodeName,
                false, LISParameters.PerformLIS);
        }

        /// <summary>
        /// Validate GetLongestSequence() method with more than two unique matches
        /// and overlap in reference and query parameter and validate
        /// the unique matches
        /// Input : One sequence for both reference and query parameter with more than 
        /// two unique matches and cross overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void LISMoreThanTwoUniqueMatchCrossOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineMoreThanTwoMatchOverlapSequenceNodeName,
                false, LISParameters.PerformLIS);
        }

        /// <summary>
        /// Validate GetLongestSequence() method with more than two unique matches
        /// ,overlap and Longest sequence in reference and query parameter and validate
        /// the unique matches
        /// Input : One sequence for both reference and query parameter with more than 
        /// two unique matches, longest sequence and cross overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void LISMultipleLongUniqueMatchCrossOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineMultipleLongMatchOverlapSequenceNodeName,
                false, LISParameters.PerformLIS);
        }

        /// <summary>
        /// Validate GetLongestSequence() method with more than two unique matches
        /// ,overlap and same length sequence in reference and query parameter and validate
        /// the unique matches
        /// Input : One sequence for both reference and query parameter with more than 
        /// two unique matches, same length and cross overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void LISMultipleSameLengthUniqueMatchCrossOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineMultipleSameLengthMatchOverlapSequenceNodeName,
                false, LISParameters.PerformLIS);
        }

        /// <summary>
        /// Validate GetLongestSequence() method with more than two unique matches
        /// ,overlap and not cross over lap sequence in reference and query parameter and validate
        /// the unique matches
        /// Input : One sequence for both reference and query parameter with more than 
        /// two unique matches, same length and overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void LISMultipleUniqueMatchOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineMultipleMatchOverlapSequenceNodeName,
                false, LISParameters.PerformLIS);
        }

        #endregion Longest Increasing Sub Sequence

        #region MUMmer Align Test Cases

        /// <summary>
        /// Validate Align(reference, query) method with Dna sequence 
        /// and validate the aligned sequences
        /// Input : Dna sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignDnaSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.DnaSequenceNodeName, true, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with Rna sequence 
        /// and validate the aligned sequences
        /// Input : Rna sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignRnaSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.RnaSequenceNodeName, true, false);
        }

        /// <summary>
        /// Validate Align(list of sequences) method with valid sequence 
        /// and validate the aligned sequences
        /// Input : valid sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignListOneLineSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.OneLineSequenceNodeName, false, true);
        }

        /// <summary>
        /// Validate Align(list of sequences) method with valid 
        /// small size sequence and validate the aligned sequences
        /// Input : valid small size sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignListSmallSizeSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.SmallSizeSequenceNodeName, true, true);
        }

        /// <summary>
        /// Validate Align(list of sequences) method with valid 
        /// Dna sequence and validate the aligned sequences
        /// Input : valid Dna sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignListDnaSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.DnaSequenceNodeName, true, true);
        }

        /// <summary>
        /// Validate Align(list of sequences) method with valid 
        /// Rna sequence and validate the aligned sequences
        /// Input : valid Rna sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignListRnaSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.RnaSequenceNodeName, true, true);
        }

        /// <summary>
        /// Validate Align(reference, query) method with repeating character sequence 
        /// and validate the aligned sequences
        /// Input : repeating character sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignRepeatingCharactersSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.OneLineRepeatingCharactersNodeName, false, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with alternate repeating character sequence 
        /// and validate the aligned sequences
        /// Input : repeating character sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignAlternateRepeatingCharactersSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.OneLineAlternateRepeatingCharactersNodeName,
                false, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with fasta file sequence 
        /// and validate the aligned sequences
        /// Input : FastA file sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignFastAFileSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.SimpleDnaFastaNodeName,
                true, false, ParserParameters.FastA);
        }

        /// <summary>
        /// Validate Align(reference, query) method with GenBank file sequence 
        /// and validate the aligned sequences
        /// Input : GenBank file sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignGenBankFileSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.SimpleDnaGenBankNodeName,
                true, false, ParserParameters.GenBank);
        }

        /// <summary>
        /// Validate Align(reference, query) method with Gff file sequence 
        /// and validate the aligned sequences
        /// Input : Gff file sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignGffFileSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.SimpleDnaGffNodeName,
                true, false, ParserParameters.Gff);
        }

        /// <summary>
        /// Validate Align(reference, query) method with only repeating character sequence 
        /// and validate the aligned sequences
        /// Input : only Repeating character sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignOnlyRepeatingCharactersSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.OneLineOnlyRepeatingCharactersNodeName,
                false, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with two query sequences 
        /// and validate the aligned sequences
        /// Input : Two Query sequences
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignTwoQuerySequences()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.DnaQueryDnaRnaSequenceNodeName,
                true, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with Dna sequence 
        /// and validate the aligned sequences with valid MUM length
        /// Input : Dna sequence with valid Mum length
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignValidMumLengthSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.DnaSequenceNodeName, true, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with same sequence 
        /// for both and validate the aligned sequences
        /// Input : Same sequence for both Query and Reference
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignSameCharactersSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.OneLineSameCharactersNodeName,
                false, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with overlap sequence 
        /// and validate the aligned sequences
        /// Input : Over lap sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignOverlapSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.OneLineOverlapSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with no matching sequence 
        /// and validate the aligned sequences
        /// Input : No matching sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignNoMatchSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.OneLineNoMatchSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with Cross overlap sequence 
        /// and validate the aligned sequences
        /// Input : Cross Over lap sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignCrossOverlapSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.OneLineMoreThanTwoMatchOverlapSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with ambiguity
        /// in Dna Search sequence and validate the aligned sequences
        /// Input : Ambiguity characters in Dna Search sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignDnaSearchAmbiguitySequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.DnaSearchAmbiguitySequenceNodeName,
                true, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with ambiguity
        /// in Rna Search sequence and validate the aligned sequences
        /// Input : Ambiguity characters in Rna Search sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignRnaSearchAmbiguitySequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.RnaSearchAmbiguitySequenceNodeName,
                true, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with ambiguity
        /// in Dna Query sequence and validate the aligned sequences
        /// Input : Ambiguity characters in Dna Query sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignDnaQueryAmbiguitySequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.DnaQueryAmbiguitySequenceNodeName,
                true, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with ambiguity
        /// in Rna Query sequence and validate the aligned sequences
        /// Input : Ambiguity characters in Rna Query sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignRnaQueryAmbiguitySequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.RnaQueryAmbiguitySequenceNodeName,
                true, false);
        }

        /// <summary>
        /// Validate Align(reference, query) method with similarity matrix
        /// as Blosum 50 and validate the aligned sequences
        /// Input : Sequence with Similarity matrix Blosum 50.
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignSimilarityMatrixBlosum50Sequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.OneLineMultipleSameLengthMatchOverlapSequenceNodeName,
                false, false, ParserParameters.FastA, AdditionalParameters.PerformSimilarityMatrixChange);
        }

        /// <summary>
        /// Validate Align(reference, query) method with algorithm
        /// as Needleman-wunsch and validate the aligned sequences
        /// Input : Sequence with algorithm as NeedleManWunsch.
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignAlgorithmSmithWatermanSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.OneLineMultipleLongMatchOverlapSequenceNodeName,
                false, false, ParserParameters.FastA, AdditionalParameters.PerformAlgorithmChange);
        }

        #endregion MUMmer Align Test Cases

        #region MUMs valildation Test Cases

        /// <summary>
        /// Validate MUMs with One Line Multiple Match Overlap Sequences 
        /// and validate the MUMs up to LIS.
        /// Input : One Line Multiple Match Overlap Sequences
        /// Validation : Validate the MUMs Output.
        /// </summary>
        [Test]
        public void ValidateMUMsUpToLISOneLineMultipleOverlapSeq()
        {
            ValidateMUMsGeneralTestCases(Constants.OneLineMultipleMatchOverlapSequenceUpToLISNode, false, false, true);
        }

        /// <summary>
        /// Validate MUMs with One Line Multiple Match Overlap Sequences 
        /// and validate the MUMs after LIS.
        /// Input : One Line Multiple Match Overlap Sequences
        /// Validation : Validate the MUMs Output.
        /// </summary>
        [Test]
        public void ValidateMUMsAfterLISOneLineMultipleOverlapSeq()
        {
            ValidateMUMsGeneralTestCases(Constants.OneLineMultipleMatchOverlapSequenceAfterLISNode, false, true, true);
        }

        /// <summary>
        /// Validate MUMs with One Line Multiple Same Length MatchOverlap Sequence
        /// AfterLIS and validate the MUMs after LIS.
        /// Input : One Line MultipleMatchOverlap Sequences
        /// Validation : Validate the MUMs Output.
        /// </summary>
        [Test]
        public void ValidateMUMsAfterLISOneLineMultipleOverlapSameLengthSeq()
        {
            ValidateMUMsGeneralTestCases(Constants.OneLineMultipleSameLengthMatchOverlapSequenceNodeName, false, true, true);
        }

        /// <summary>
        /// Validate MUMs with One Line Multiple Same Length MatchOverlap Sequence
        /// AfterLIS and validate the MUMs Up to LIS.
        /// Input : One Line MultipleMatchOverlap Sequences
        /// Validation : Validate the MUMs Output.
        /// </summary>
        [Test]
        public void ValidateMUMsUpToLISOneLineMultipleOverlapSameLengthSeq()
        {
            ValidateMUMsGeneralTestCases(Constants.OneLineMultipleSameLengthMatchOverlapSequenceUptoLISNode, false, false, true);
        }

        /// <summary>
        /// Validate MUMs with Protein sequence 
        /// and validate the MUMs after LIS.
        /// Input : One Line Protein sequence
        /// Validation : Validate the MUMs Output.
        /// </summary>
        [Test]
        public void ValidateMUMsAfterLISProteinSeq()
        {
            ValidateMUMsGeneralTestCases(Constants.ProteinSequenceNodeName, true, true, true);
        }

        /// <summary>
        /// Validate MUMs with Protein sequence 
        /// and validate the MUMs Up to LIS.
        /// Input : One Line Protein sequence
        /// Validation : Validate the MUMs Output.
        /// </summary>
        [Test]
        public void ValidateMUMsUpToLISProteinSeq()
        {
            ValidateMUMsGeneralTestCases(Constants.ProteinSequenceNodeName,
                true, false, true);
        }

        /// <summary>
        /// Validate MUMs with One Line More Than Two Match Sequence
        /// and validate the MUMs after LIS.
        /// Input : One Line More Than Two Match Sequence
        /// Validation : Validate the MUMs Output.
        /// </summary>
        [Test]
        public void ValidateMUMsAfterLISWithOneLineMoreThanTwoMatchSequence()
        {
            ValidateMUMsGeneralTestCases(Constants.OneLineMoreThanTwoMatchSequenceAfterLISNode,
                false, true, true);
        }

        /// <summary>
        /// Validate MUMs with One Line More Than Two Match Sequence
        /// and validate the MUMs Upto LIS.
        /// Input : One Line More Than Two Match Sequence
        /// Validation : Validate the MUMs Output.
        /// </summary>
        [Test]
        public void ValidateMUMsUpToLISOneLineMoreThanTwoMatchSequence()
        {
            ValidateMUMsGeneralTestCases(Constants.OneLineMoreThanTwoMatchSequenceUpToLISNode,
                false, false, true);
        }

        #endregion MUMs valildation Test Cases

        #region Supported Methods

        /// <summary>
        /// Validates most of the build suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is file path?</param>
        static void ValidateBuildSuffixTreeGeneralTestCases(string nodeName, bool isFilePath)
        {
            ValidateBuildSuffixTreeGeneralTestCases(nodeName, isFilePath, ParserParameters.FastA);
        }

        /// <summary>
        /// Validates most of the find matches suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is File Path?</param>
        /// <param name="LISActionType">LIS action type enum</param>
        static void ValidateFindMatchSuffixGeneralTestCases(string nodeName, bool isFilePath,
            LISParameters LISActionType)
        {
            ValidateFindMatchSuffixGeneralTestCases(nodeName, isFilePath, LISActionType, false);
        }

        /// <summary>
        /// Validates most of the find matches suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is File Path?</param>
        /// <param name="LISActionType">LIS action type enum</param>
        /// <param name="isMultiSequenceFile">Is Multi Sequence Search File?</param>
        static void ValidateFindMatchSuffixGeneralTestCases(string nodeName, bool isFilePath,
            LISParameters LISActionType, bool isMultiSequenceSearchFile)
        {
            ISequence referenceSeq = null;
            ISequence querySeq = null;
            string referenceSequence = string.Empty;
            string querySequence = string.Empty;
            IList<ISequence> querySeqs = null;

            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format(null,
                    "MUMmer P1 : Successfully validated the File Path '{0}'.", filePath));

                FastaParser parser = new FastaParser();
                IList<ISequence> referenceSeqs = parser.Parse(filePath);
                referenceSeq = referenceSeqs[0];
                referenceSequence = referenceSeq.ToString();

                // Gets the query sequence from the configurtion file
                string queryFilePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceFilePathNode);

                Assert.IsNotNull(queryFilePath);
                ApplicationLog.WriteLine(string.Format(null,
                    "MUMmer P1 : Successfully validated the Search File Path '{0}'.", queryFilePath));

                FastaParser queryParser = new FastaParser();
                querySeqs = queryParser.Parse(queryFilePath);
                querySeq = querySeqs[0];
                querySequence = querySeq.ToString();
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                string referenceAlphabet = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(referenceAlphabet),
                    referenceSequence);

                querySequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceNode);

                referenceAlphabet = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceAlphabetNode);

                querySeq = new Sequence(Utility.GetAlphabet(referenceAlphabet),
                    querySequence);
            }

            string mumLength = Utility._xmlUtil.GetTextValue(nodeName, Constants.MUMLengthNode);

            // Builds the suffix for the reference sequence passed.
            ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
            SequenceSuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSeq);
            IList<MaxUniqueMatch> matches = null;

            // For multi sequence query file validate all the sequences with the reference sequence
            if (isMultiSequenceSearchFile)
            {
                matches = suffixTreeBuilder.FindMatches(suffixTree,
                    querySeqs[0], long.Parse(mumLength, null));
                ValidateUniqueMatchesForAllParameters(matches,
                    nodeName, LISActionType, referenceSequence, querySeqs[0].ToString());
                matches = suffixTreeBuilder.FindMatches(suffixTree,
                    querySeqs[1], long.Parse(mumLength, null));
                ValidateUniqueMatchesForAllParameters(matches,
                    nodeName, LISActionType, referenceSequence, querySeqs[1].ToString());
            }
            else
            {
                matches = suffixTreeBuilder.FindMatches(suffixTree,
                    querySeq, long.Parse(mumLength, null));
                ValidateUniqueMatchesForAllParameters(matches, nodeName,
                    LISActionType, referenceSequence, querySeq.ToString());
            }
        }

        /// <summary>
        /// Validates the Mummer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isFilePath">Is Sequence saved in File</param>
        /// <param name="isAlignList">Is align method to take list?</param>
        static void ValidateMUMmerAlignGeneralTestCases(string nodeName, bool isFilePath,
            bool isAlignList)
        {
            ValidateMUMmerAlignGeneralTestCases(nodeName, isFilePath, isAlignList,
                ParserParameters.FastA, AdditionalParameters.Other);
        }

        /// <summary>
        /// Validates the Mummer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isFilePath">Is Sequence saved in File</param>
        /// <param name="isAlignList">Is align method to take list?</param>
        /// <param name="parserParam">Parser Parameter</param>
        static void ValidateMUMmerAlignGeneralTestCases(string nodeName, bool isFilePath,
            bool isAlignList, ParserParameters parserParam)
        {
            ValidateMUMmerAlignGeneralTestCases(nodeName, isFilePath,
                       isAlignList, parserParam, AdditionalParameters.Other);
        }

        /// <summary>
        /// Validates the Mummer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isFilePath">Is Sequence saved in File</param>
        /// <param name="isAlignList">Is align method to take list?</param>
        /// <param name="parserParam">Parser Parameter</param>
        /// <param name="addParam">Additional parameter</param>
        static void ValidateMUMmerAlignGeneralTestCases(string nodeName, bool isFilePath,
            bool isAlignList, ParserParameters parserParam,
            AdditionalParameters addParam)
        {
            ISequence referenceSeq = null;
            ISequence querySeq = null;
            IList<ISequence> querySeqs = null;
            string referenceSequence = string.Empty;
            string querySequence = string.Empty;
            List<ISequence> alignList = null;
            IList<ISequence> referenceSeqs = null;

            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format(null,
                    "MUMmer P1 : Successfully validated the File Path '{0}'.",
                    filePath));

                switch (parserParam)
                {
                    case ParserParameters.GenBank:
                        GenBankParser genBankParserObj = new GenBankParser();
                        referenceSeqs = genBankParserObj.Parse(filePath);
                        break;
                    case ParserParameters.Gff:
                        GffParser gffParserObj = new GffParser();
                        referenceSeqs = gffParserObj.Parse(filePath);
                        break;
                    default:
                        FastaParser fastaParserObj = new FastaParser();
                        referenceSeqs = fastaParserObj.Parse(filePath);
                        break;
                }

                referenceSeq = referenceSeqs[0];
                referenceSequence = referenceSeq.ToString();

                // Gets the reference sequence from the configurtion file
                string queryFilePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceFilePathNode);

                Assert.IsNotNull(queryFilePath);
                ApplicationLog.WriteLine(string.Format(null,
                    "MUMmer P1 : Successfully validated the Search File Path '{0}'.",
                    queryFilePath));

                switch (parserParam)
                {
                    case ParserParameters.GenBank:
                        GenBankParser genBankParserObj = new GenBankParser();
                        querySeqs = genBankParserObj.Parse(queryFilePath);
                        break;
                    case ParserParameters.Gff:
                        GffParser gffParserObj = new GffParser();
                        querySeqs = gffParserObj.Parse(queryFilePath);
                        break;
                    default:
                        FastaParser fastaParserObj = new FastaParser();
                        querySeqs = fastaParserObj.Parse(queryFilePath);
                        break;
                }

                querySeq = querySeqs[0];
                querySequence = querySeq.ToString();

                if (isAlignList)
                {
                    alignList = new List<ISequence>();
                    alignList.Add(referenceSeq);
                    alignList.Add(querySeq);
                }
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                string referenceSeqAlphabet = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(referenceSeqAlphabet),
                    referenceSequence);

                querySequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceNode);

                referenceSeqAlphabet = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceAlphabetNode);

                querySeq = new Sequence(Utility.GetAlphabet(referenceSeqAlphabet),
                    querySequence);
                querySeqs = new List<ISequence>();

                if (isAlignList)
                {
                    alignList = new List<ISequence>();
                    alignList.Add(referenceSeq);
                    alignList.Add(querySeq);
                }
                else
                    querySeqs.Add(querySeq);
            }

            string mumLength = Utility._xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode);

            MUMmer mum = new MUMmer3();
            mum.LengthOfMUM = long.Parse(mumLength, null);
            mum.StoreMUMs = true;

            switch (addParam)
            {
                case AdditionalParameters.PerformAlgorithmChange:
                    mum.PairWiseAlgorithm = new SmithWatermanAligner();
                    mum.SimilarityMatrix =
                        new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
                    mum.GapOpenCost = -13;
                    break;
                case AdditionalParameters.PerformSimilarityMatrixChange:
                    mum.SimilarityMatrix =
                        new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum50);
                    mum.PairWiseAlgorithm = new NeedlemanWunschAligner();
                    mum.GapOpenCost =
                        int.Parse(Utility._xmlUtil.GetTextValue(nodeName, Constants.GapOpenCostNode),
                        (IFormatProvider)null);
                    break;
                default:
                    mum.PairWiseAlgorithm = new NeedlemanWunschAligner();
                    mum.GapOpenCost =
                        int.Parse(Utility._xmlUtil.GetTextValue(nodeName, Constants.GapOpenCostNode),
                        (IFormatProvider)null);
                    break;
            }

            IList<IPairwiseSequenceAlignment> align = null;

            if (isAlignList)
                align = mum.AlignSimple(alignList);
            else
                align = mum.AlignSimple(referenceSeq, querySeqs);

            // Validate FinalMUMs and MUMs Properties.
            Assert.IsNotNull(mum.FinalMUMs);
            Assert.IsNotNull(mum.MUMs);

            string expectedScore = Utility._xmlUtil.GetTextValue(nodeName, Constants.ScoreNodeName);

            string[] expectedSequences =
                Utility._xmlUtil.GetTextValues(nodeName, Constants.ExpectedSequencesNode);
            IList<IPairwiseSequenceAlignment> expectedOutput = new List<IPairwiseSequenceAlignment>();

            // Validate for two aligned sequences and single aligned sequences appropriately
            if (1 >= querySeqs.Count)
            {
                IPairwiseSequenceAlignment seqAlign = new PairwiseSequenceAlignment();
                PairwiseAlignedSequence alignedSeq = new PairwiseAlignedSequence();
                alignedSeq.FirstSequence = new Sequence(referenceSeq.Alphabet, expectedSequences[0]);
                alignedSeq.SecondSequence = new Sequence(referenceSeq.Alphabet, expectedSequences[1]);
                alignedSeq.Score = int.Parse(expectedScore);
                seqAlign.PairwiseAlignedSequences.Add(alignedSeq);
                expectedOutput.Add(seqAlign);
                Assert.IsTrue(CompareAlignment(align, expectedOutput));
            }
            else
            {
                string[] expectedScores = expectedScore.Split(',');
                IPairwiseSequenceAlignment seq1Align = new PairwiseSequenceAlignment();
                IPairwiseSequenceAlignment seq2Align = new PairwiseSequenceAlignment();

                // Get the first sequence for validation
                PairwiseAlignedSequence alignedSeq1 = new PairwiseAlignedSequence();
                alignedSeq1.FirstSequence = new Sequence(referenceSeq.Alphabet, expectedSequences[0]);
                alignedSeq1.SecondSequence = new Sequence(referenceSeq.Alphabet, expectedSequences[1]);
                alignedSeq1.Score = int.Parse(expectedScores[0]);
                seq1Align.PairwiseAlignedSequences.Add(alignedSeq1);
                expectedOutput.Add(seq1Align);

                // Get the second sequence for validation
                PairwiseAlignedSequence alignedSeq2 = new PairwiseAlignedSequence();
                alignedSeq2.FirstSequence = new Sequence(referenceSeq.Alphabet, expectedSequences[2]);
                alignedSeq2.SecondSequence = new Sequence(referenceSeq.Alphabet, expectedSequences[3]);
                alignedSeq2.Score = int.Parse(expectedScores[1]);
                seq2Align.PairwiseAlignedSequences.Add(alignedSeq2);
                expectedOutput.Add(seq2Align);
                Assert.IsTrue(CompareAlignment(align, expectedOutput));
            }
            Console.WriteLine("MUMmer P1 : Successfully validated the aligned sequences.");
            ApplicationLog.WriteLine("MUMmer P1 : Successfully validated the aligned sequences.");
        }

        /// <summary>
        /// Validates most of the build suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is file path?</param>
        /// <param name="parserParam">Parser parameter.</param>
        static void ValidateBuildSuffixTreeGeneralTestCases(string nodeName, bool isFilePath,
            ParserParameters parserParam)
        {
            ISequence referenceSeq = null;
            string referenceSequence = string.Empty;

            if (isFilePath)
            {
                IList<ISequence> referenceSeqs = null;
                // Gets the reference sequence from the configurtion file
                string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format(null,
                    "MUMmer P1 : Successfully validated the File Path '{0}'.", filePath));

                switch (parserParam)
                {
                    case ParserParameters.GenBank:
                        GenBankParser genBankParserObj = new GenBankParser();
                        referenceSeqs = genBankParserObj.Parse(filePath);
                        break;
                    case ParserParameters.Gff:
                        GffParser gffParserObj = new GffParser();
                        referenceSeqs = gffParserObj.Parse(filePath);
                        break;
                    default:
                        FastaParser fastaParserObj = new FastaParser();
                        referenceSeqs = fastaParserObj.Parse(filePath);
                        break;
                }
                referenceSeq = referenceSeqs[0];
                referenceSequence = referenceSeq.ToString();
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode)), referenceSequence);
            }

            // Builds the suffix for the reference sequence passed.
            ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
            SequenceSuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSeq);

            // Validates the edges for a given sequence.
            ApplicationLog.WriteLine("MUMmer P1 : Validating the Edges");
            Assert.IsTrue(ValidateEdges(suffixTree, nodeName));
            Console.WriteLine(string.Format(null,
                "MUMmer P1 : Successfully validated the all the Edges for the sequence '{0}'.",
                referenceSequence));
            ApplicationLog.WriteLine(string.Format(null,
                "MUMmer P1 : Successfully validated the all the Edges for the sequence '{0}'.",
                referenceSequence));

            Assert.AreEqual(suffixTree.Sequence.ToString(), referenceSequence);
            Console.WriteLine(string.Format(null,
                "MUMmer P1 : Successfully validated the Suffix Tree properties for the sequence '{0}'.",
                referenceSequence));
            ApplicationLog.WriteLine(string.Format(null,
                "MUMmer P1 : Successfully validated the Suffix Tree properties for the sequence '{0}'.",
                referenceSequence));
        }

        /// <summary>
        /// Validates the edges for the suffix tree and the node name specified.
        /// </summary>
        /// <param name="suffixTree">Suffix Tree.</param>
        /// <param name="nodeName">Node name which needs to be read for validation</param>
        /// <returns>True, if successfully validated.</returns>
        static bool ValidateEdges(SequenceSuffixTree suffixTree, string nodeName)
        {
            Dictionary<int, Edge> ed = suffixTree.Edges;

            string[] actualStrtIndexes = new string[ed.Count];
            string[] actualEndIndexes = new string[ed.Count];

            int j = 0;
            foreach (int col in ed.Keys)
            {
                Edge a = ed[col];
                actualStrtIndexes[j] = a.StartIndex.ToString();
                actualEndIndexes[j] = a.EndIndex.ToString();
                j++;
            }

            // Gets the sorted edge list for the actual Edge list
            List<Edge> actualEdgeList = GetSortedEdges(actualStrtIndexes, actualEndIndexes);

            // Gets all the edges to be validated as in xml.
            string[] startIndexes =
                Utility._xmlUtil.GetTextValue(nodeName, Constants.EdgeStartIndexesNode).Split(',');
            string[] endIndexes =
                Utility._xmlUtil.GetTextValue(nodeName, Constants.EdgeEndIndexesNode).Split(',');

            // Gets the sorted edge list for the expected Edge list
            List<Edge> expectedEdgeList = GetSortedEdges(startIndexes, endIndexes);

            Console.WriteLine(string.Format(null,
                "MUMmer P1 : Total Edges Found is : '{0}'", ed.Keys.Count.ToString((IFormatProvider)null)));
            ApplicationLog.WriteLine(string.Format(null,
                "MUMmer P1 : Total Edges Found is : '{0}'", ed.Keys.Count.ToString((IFormatProvider)null)));

            // Loops through all the edges and validates the same.
            for (int i = 0; i < expectedEdgeList.Count; i++)
            {
                if (!(actualEdgeList[i].StartIndex == expectedEdgeList[i].StartIndex)
                    && (actualEdgeList[i].EndIndex == expectedEdgeList[i].EndIndex))
                {
                    Console.WriteLine(string.Format(null,
                        "MUMmer P1 : Edges not matching at index '{0}'", i.ToString()));
                    ApplicationLog.WriteLine(string.Format(null,
                        "MUMmer P1 : Edges not matching at index '{0}'", i.ToString()));
                    return false;
                }

                i++;
            }

            return true;
        }

        /// <summary>
        /// Gets the Sorted Edge for the given start and end indexes
        /// </summary>
        /// <param name="startIndexes">Start Index</param>
        /// <param name="endIndexes">End Index</param>
        /// <returns>Sorted Edge list</returns>
        static List<Edge> GetSortedEdges(string[] startIndexes, string[] endIndexes)
        {
            List<Edge> edgList = new List<Edge>();

            // Loops through all the indexes and creates EdgeList.
            for (int i = 0; i < startIndexes.Length; i++)
            {
                Edge edg = new Edge();
                edg.StartIndex = int.Parse(startIndexes[i]);
                edg.EndIndex = int.Parse(endIndexes[i]);

                edgList.Add(edg);
            }

            List<Edge> sortedEdgeList =
                edgList.OrderBy(stEd => stEd.StartIndex).ThenBy(endEd => endEd.EndIndex).ToList();

            return sortedEdgeList;
        }

        /// <summary>
        /// Validates the Unique Matches for the input provided.
        /// </summary>
        /// <param name="matches">Max Unique Match list</param>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="LISActionType">Unique Match/Sub level LIS/LIS</param>
        /// <param name="referenceSequence">Reference sequence.</param>
        /// <param name="querySequence">Query sequence.</param>
        static void ValidateUniqueMatchesForAllParameters(IList<MaxUniqueMatch> matches, string nodeName,
            LISParameters LISActionType, string referenceSequence, string querySequence)
        {
            switch (LISActionType)
            {
                case LISParameters.FindUniqueMatches:
                    // Validates the Unique Matches.
                    ApplicationLog.WriteLine("MUMmer P1 : Validating the Unique Matches");
                    Assert.IsTrue(ValidateUniqueMatches(matches, nodeName, LISActionType));
                    break;
                case LISParameters.PerformLIS:
                    // Validates the Unique Matches.
                    ApplicationLog.WriteLine("MUMmer P1 : Validating the Unique Matches");
                    LongestIncreasingSubsequence lisObj = new LongestIncreasingSubsequence();
                    IList<MaxUniqueMatch> lisMatches = lisObj.GetLongestSequence(matches);
                    Assert.IsTrue(ValidateUniqueMatches(lisMatches, nodeName, LISActionType));
                    break;
                default:
                    break;
            }

            Console.WriteLine(string.Format(null,
                "MUMmer P1 : Successfully validated the all the unique matches for the sequence '{0}' and '{1}'.",
                referenceSequence, querySequence));
            ApplicationLog.WriteLine(string.Format(null,
                "MUMmer P1 : Successfully validated the all the unique matches for the sequence '{0}' and '{1}'.",
                referenceSequence, querySequence));
        }

        /// <summary>
        /// Validates the Unique Matches for the input provided.
        /// </summary>
        /// <param name="matches">Max Unique Match list</param>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="LISActionType">Unique Match/Sub level LIS/LIS</param>
        /// <returns>True, if successfully validated</returns>
        static bool ValidateUniqueMatches(IList<MaxUniqueMatch> matches, string nodeName,
            LISParameters LISActionType)
        {
            // Gets all the unique matches properties to be validated as in xml.
            string[] firstSeqOrder = null;
            string[] firstSeqStart = null;
            string[] length = null;
            string[] secondSeqOrder = null;
            string[] secondSeqStart = null;

            switch (LISActionType)
            {
                case LISParameters.PerformLIS:
                    firstSeqOrder = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.LisFirstSequenceMumOrderNode).Split(',');
                    firstSeqStart = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.LisFirstSequenceStartNode).Split(',');
                    length = Utility._xmlUtil.GetTextValue(nodeName, Constants.LisLengthNode).Split(',');
                    secondSeqOrder = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.LisSecondSequenceMumOrderNode).Split(',');
                    secondSeqStart = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.LisSecondSequenceStartNode).Split(',');
                    break;
                case LISParameters.FindUniqueMatches:
                    firstSeqOrder = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.FirstSequenceMumOrderNode).Split(',');
                    firstSeqStart = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.FirstSequenceStartNode).Split(',');
                    length = Utility._xmlUtil.GetTextValue(nodeName, Constants.LengthNode).Split(',');
                    secondSeqOrder = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.SecondSequenceMumOrderNode).Split(',');
                    secondSeqStart = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.SecondSequenceStartNode).Split(',');
                    break;
                default:
                    break;
            }

            Console.WriteLine(string.Format(null, "MUMmer P1 : Total Matches Found is : '{0}'",
                matches.Count.ToString((IFormatProvider)null)));
            ApplicationLog.WriteLine(string.Format(null, "MUMmer P1 : Total Matches Found is : '{0}'",
                matches.Count.ToString((IFormatProvider)null)));

            int i = 0;
            // Loops through all the matches and validates the same.
            foreach (MaxUniqueMatch match in matches)
            {
                if ((0 != string.Compare(firstSeqOrder[i],
                    match.FirstSequenceMumOrder.ToString((IFormatProvider)null),
                    true, CultureInfo.CurrentCulture))
                    || (0 != string.Compare(firstSeqStart[i],
                    match.FirstSequenceStart.ToString((IFormatProvider)null),
                    true, CultureInfo.CurrentCulture))
                    || (0 != string.Compare(length[i],
                    match.Length.ToString((IFormatProvider)null), true, CultureInfo.CurrentCulture))
                    || (0 != string.Compare(secondSeqOrder[i],
                    match.SecondSequenceMumOrder.ToString((IFormatProvider)null),
                    true, CultureInfo.CurrentCulture))
                    || (0 != string.Compare(secondSeqStart[i],
                    match.SecondSequenceStart.ToString((IFormatProvider)null),
                    true, CultureInfo.CurrentCulture)))
                {
                    Console.WriteLine(string.Format(null,
                        "MUMmer P1 : Unique match not matching at index '{0}'", i.ToString()));
                    ApplicationLog.WriteLine(string.Format(null,
                        "MUMmer P1 : Unique match not matching at index '{0}'", i.ToString()));
                    return false;
                }
                i++;
            }

            return true;
        }

        /// <summary>
        /// Validate the Mummer GetMUMs method for different test cases.
        /// </summary>
        /// <param name="nodeName">Name of the XML node to be read.</param>
        /// <param name="isFilePath">Is Sequence saved in File</param>
        /// <param name="isAfterLIS">Is Mummer execution after LIS</param>
        /// <param name="isLIS">Is Mummer execution with LIS option</param>
        static void ValidateMUMsGeneralTestCases(string nodeName, bool isFilePath, bool isAfterLIS, bool isLIS)
        {
            ISequence referenceSeq = null;
            ISequence querySeq = null;
            IList<ISequence> querySeqs = null;
            string referenceSequence = string.Empty;
            string querySequence = string.Empty;

            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format(null,
                    "MUMmer P1 : Successfully validated the File Path '{0}'.", filePath));

                FastaParser parser = new FastaParser();
                IList<ISequence> referenceSeqs = parser.Parse(filePath);
                referenceSeq = referenceSeqs[0];
                referenceSequence = referenceSeq.ToString();

                // Gets the reference sequence from the configurtion file
                string queryFilePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceFilePathNode);

                Assert.IsNotNull(queryFilePath);
                ApplicationLog.WriteLine(string.Format(null,
                    "MUMmer P1 : Successfully validated the Search File Path '{0}'.", queryFilePath));

                FastaParser queryParser = new FastaParser();
                querySeqs = queryParser.Parse(queryFilePath);
                querySeq = querySeqs[0];
                querySequence = querySeq.ToString();
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                string referenceSeqAlphabet = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(referenceSeqAlphabet),
                    referenceSequence);

                querySequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceNode);

                referenceSeqAlphabet = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceAlphabetNode);

                querySeq = new Sequence(Utility.GetAlphabet(referenceSeqAlphabet),
                    querySequence);
                querySeqs = new List<ISequence>();
                querySeqs.Add(querySeq);
            }

            string mumLength = Utility._xmlUtil.GetTextValue(nodeName, Constants.MUMLengthNode);

            // Validate MUMmer Attribute ctor(To hit Attribute ctor).
            MUMmerAttributes mumAttributeObj = new MUMmerAttributes();
            var attribute = mumAttributeObj.Attributes;
            Assert.IsNotNull(attribute);

            MUMmer mum = new MUMmer3();
            mum.LengthOfMUM = long.Parse(mumLength, null);
            IDictionary<ISequence, IList<MaxUniqueMatch>> actualResult = null;

            if (!isLIS)
            {
                actualResult = mum.GetMUMs(referenceSeq, querySeqs);
            }
            else
            {
                actualResult = mum.GetMUMs(referenceSeq, querySeqs, isAfterLIS);
            }

            // Validate MUMs output.
            Assert.IsTrue(ValidateMums(nodeName, actualResult, querySeq));

            Console.WriteLine("MUMmer P1 : Successfully validated the Mumms");
            ApplicationLog.WriteLine("MUMmer P1 : Successfully validated the Mumms.");
        }

        /// <summary>
        /// Compare the alignment of mummer and defined alignment
        /// </summary>
        /// <param name="actualAlignment">actual alignment</param>
        /// <param name="expectedAlignment">expected output</param>
        /// <returns>Compare result of alignments</returns>
        static bool CompareAlignment(IList<IPairwiseSequenceAlignment> actualAlignment,
            IList<IPairwiseSequenceAlignment> expectedAlignment)
        {
            bool output = true;

            if (actualAlignment.Count == expectedAlignment.Count)
            {
                for (int resultCount = 0; resultCount < actualAlignment.Count; resultCount++)
                {
                    if (actualAlignment[resultCount].PairwiseAlignedSequences.Count ==
                        expectedAlignment[resultCount].PairwiseAlignedSequences.Count)
                    {
                        for (int alignSeqCount = 0; alignSeqCount <
                            actualAlignment[resultCount].PairwiseAlignedSequences.Count; alignSeqCount++)
                        {
                            // Validates the First Sequence, Second Sequence and Score
                            if (actualAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].FirstSequence.ToString().Equals(
                                    expectedAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].FirstSequence.ToString())
                                && actualAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].SecondSequence.ToString().Equals(
                                    expectedAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].SecondSequence.ToString())
                                && actualAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].Score ==
                                    expectedAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].Score)
                            {
                                output = true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return output;
        }

        /// <summary>
        /// Validate the Mums output.
        /// </summary>
        /// <param name="result">Mumms Output</param>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="querySeq">Query Sequence</param>
        static bool ValidateMums(string nodeName,
            IDictionary<ISequence, IList<MaxUniqueMatch>> result, ISequence querySeq)
        {
            string[] firstSeqOrder = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.LisFirstSequenceMumOrderNode).Split(',');
            string[] firstSeqStart = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.LisFirstSequenceStartNode).Split(',');
            string[] length = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.LisLengthNode).Split(',');
            string[] secondSeqOrder = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.LisSecondSequenceMumOrderNode).Split(',');
            string[] secondSeqStart = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.LisSecondSequenceStartNode).Split(',');

            foreach (var mum in result)
            {
                var mums = result[mum.Key];
                for (int i = 0; i < mums.Count; i++)
                {
                    if (0 != string.Compare(querySeq.ToString(), mums[i].Query.ToString())
                       || (0 != string.Compare(firstSeqOrder[i], mums[i].FirstSequenceMumOrder.ToString()))
                       || (0 != string.Compare(firstSeqStart[i], mums[i].FirstSequenceStart.ToString()))
                       || (0 != string.Compare(length[i], mums[i].Length.ToString()))
                       || (0 != string.Compare(secondSeqOrder[i], mums[i].SecondSequenceMumOrder.ToString()))
                       || (0 != string.Compare(secondSeqStart[i], mums[i].SecondSequenceStart.ToString())))
                    {
                        Console.WriteLine(string.Format(null,
                            "MUMmer P1 : There is no match at '{0}'", i.ToString()));
                        ApplicationLog.WriteLine(string.Format(null,
                            "MUMmer P1 : There is no match at '{0}'", i.ToString()));
                        return false;
                    }

                }
            }
            return true;
        }
        #endregion Supported Methods
    }
}
