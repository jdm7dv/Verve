// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * NUCmerP1TestCases.cs
 * 
 *   This file contains the NUCmer P1 test cases
 * 
***************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

using MBF.Algorithms;
using MBF.Algorithms.Alignment;
using MBF.Algorithms.SuffixTree;
using MBF.IO.Fasta;
using MBF.IO.GenBank;
using MBF.IO.Gff;
using MBF.SimilarityMatrices;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.Algorithms.Alignment
{
    /// <summary>
    /// NUCmer P1 Test case implementation.
    /// </summary>
    [TestClass]
    public class NUCmerP1TestCases
    {

        #region Enums

        /// <summary>
        /// Lis Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AdditionalParameters
        {
            FindUniqueMatches,
            PerformClusterBuilder,
            AlignSimilarityMatrix,
            Default
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
        /// Parameters which are used for different test cases 
        /// based on which the properties are updated.
        /// </summary>
        enum PropertyParameters
        {
            MaximumSeparation,
            MinimumScore,
            SeparationFactor,
            FixedSeparation,
            FixedSeparationAndSeparationFactor,
            MaximumFixedAndSeparationFactor,
            Default
        };

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\NUCmerTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static NUCmerP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Suffix Tree Test Cases

        /// <summary>
        /// Validate BuildSuffixTree() method with one line DNA sequence 
        /// and validate the nodes, edges and the sequences
        /// Input : One line sequences
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeBuildSuffixTreeDnaSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.DnaNucmerSequenceNodeName, false);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with one line RNA sequence 
        /// and validate the nodes, edges and the sequences
        /// Input : One line sequences
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeBuildSuffixTreeRnaSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.RnaNucmerSequenceNodeName, false);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with medium size sequence 
        /// and validate the nodes, edges and the sequences
        /// Input : Medium size sequences
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeBuildSuffixTreeMediumSizeSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.MediumSizeSequenceNodeName, true);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with one line repeating character sequence 
        /// and validate the nodes, edges and the sequences
        /// Input : One line sequences
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeBuildSuffixTreeRepeatingCharacterSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.OneLineRepeatingCharactersNodeName, false);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with one line alternate repeating character sequence 
        /// and validate the nodes, edges and the sequences
        /// Input : One line sequences
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeBuildSuffixTreeAlternateRepeatingCharacterSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(
                Constants.OneLineAlternateRepeatingCharactersNodeName, false);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with one line only repeating character sequence 
        /// and validate the nodes, edges and the sequences
        /// Input : One line sequences
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeBuildSuffixTreeOnlyRepeatingCharacterSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(
                Constants.OneLineOnlyRepeatingCharactersNodeName, false);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with fasta file sequence 
        /// and validate the nodes, edges and the sequences
        /// Input : FastA file sequences
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeBuildSuffixTreeFastAFileSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.SimpleDnaFastaNodeName,
                true, ParserParameters.FastA);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with GenBank file sequence 
        /// and validate the nodes, edges and the sequences
        /// Input : GenBank file sequences
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeBuildSuffixTreeGenBankFileSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.SimpleDnaGenBankNodeName,
                true, ParserParameters.GenBank);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with Gff file sequence 
        /// and validate the nodes, edges and the sequences
        /// Input : Gff file sequences
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeBuildSuffixTreeGffFileSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.SimpleDnaGffNodeName,
                true, ParserParameters.Gff);
        }

        /// <summary>
        /// Validate FindMatches() method with one line sequences
        /// and valid MUM length for both reference and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter with Valid MUM length
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesOneLineSequenceValidMUMLength()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with DNA sequences
        /// for both reference and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter with Valid MUM length
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesDnaSequences()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.DnaNucmerSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with RNA sequences
        /// for both reference and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter with Valid MUM length
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesRnaSequences()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.RnaNucmerSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with medium sequences
        /// for both reference and query parameter and validate
        /// the unique matches
        /// Input : Medium size reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesMediumSizeSequences()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.MediumSizeSequenceNodeName,
                true, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with continous repeating character sequences
        /// for both reference and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter with continous
        /// repeating characters
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesContinousRepeatingSequences()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineRepeatingCharactersNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with same sequences
        /// for both reference and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter with same characters
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesSameSequences()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.SameSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with overlap sequences
        /// for both reference and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter with overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesWithCrossOverlapSequences()
        {
            ValidateFindMatchSuffixGeneralTestCases(
                Constants.TwoUniqueMatchWithCrossOverlapSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with no match sequences
        /// for both reference and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter with no match
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesWithNoMatchSequences()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineNoMatchSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with overlap sequences
        /// for both reference and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter with overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesWithOverlapSequences()
        {
            ValidateFindMatchSuffixGeneralTestCases(
                Constants.TwoUniqueMatchWithoutCrossOverlapSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with ambiguity characters in
        /// reference Dna sequence and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter with ambiguity
        /// characters in reference Dna sequence
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesAmbiguityDnaReferenceSequences()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.DnaAmbiguityReferenceSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with ambiguity characters in
        /// search Dna sequence and reference parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter with ambiguity
        /// characters in search Dna sequence
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesAmbiguityDnaSearchSequences()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.DnaAmbiguitySearchSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with ambiguity characters in
        /// reference Rna sequence and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter with ambiguity
        /// characters in reference Rna sequence
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesAmbiguityRnaReferenceSequences()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.RnaAmbiguityReferenceSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with ambiguity characters in
        /// search Rna sequence and reference parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter with ambiguity
        /// characters in search Rna sequence
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void SuffixTreeFindMatchesAmbiguityRnaSearchSequences()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.RnaAmbiguitySearchSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate BuildCluster() method with two unique match
        /// and without cross over lap and validate the clusters
        /// Input : Two unique matches without cross overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ClusterBuilderTwoUniqueMatchesWithoutCrossOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(
                Constants.TwoUniqueMatchWithoutCrossOverlapSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder);
        }

        /// <summary>
        /// Validate BuildCluster() method with two unique match
        /// and with cross over lap and validate the clusters
        /// Input : Two unique matches with cross overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ClusterBuilderTwoUniqueMatchesWithCrossOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(
                Constants.TwoUniqueMatchWithCrossOverlapSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder);
        }

        /// <summary>
        /// Validate BuildCluster() method with two unique match
        /// and with overlap and no cross overlap and validate the clusters
        /// Input : Two unique matches with overlap and no cross overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ClusterBuilderWithOverlapNoCrossOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneUniqueMatchSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder);
        }

        /// <summary>
        /// Validate BuildCluster() method with Minimum Score set to 0 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with minimum score 0
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ClusterBuilderWithMinimumScoreZero()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneUniqueMatchSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder, PropertyParameters.MinimumScore);
        }

        /// <summary>
        /// Validate BuildCluster() method with MaximumSeparation set to 0 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with MaximumSeparation 0
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ClusterBuilderWithMaximumSeparationZero()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneUniqueMatchSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder, PropertyParameters.MaximumSeparation);
        }

        /// <summary>
        /// Validate BuildCluster() method with SeperationFactor set to 0 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with SeperationFactor 0
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ClusterBuilderWithSeperationFactoreZero()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneUniqueMatchSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder, PropertyParameters.SeparationFactor);
        }

        /// <summary>
        /// Validate BuildCluster() method with FixedSeparation set to 0 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with FixedSeparation 0
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ClusterBuilderWithFixedSeparationZero()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneUniqueMatchSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder, PropertyParameters.FixedSeparation);
        }

        /// <summary>
        /// Validate BuildCluster() method with 
        /// MinimumScore set to greater than MUMlength 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with 
        /// MinimumScore set to greater than MUMlength
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ClusterBuilderWithMinimumScoreGreater()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.MinimumScoreGreaterSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder, PropertyParameters.MinimumScore);
        }

        /// <summary>
        /// Validate BuildCluster() method with 
        /// FixedSeparation set to postive value and SeparationFactor=0 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with 
        /// FixedSeparation set to postive value and SeparationFactor=0
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ClusterBuilderWithFixedSeparationAndSeparationFactor()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.MinimumScoreGreaterSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder,
                PropertyParameters.FixedSeparationAndSeparationFactor);
        }

        /// <summary>
        /// Validate BuildCluster() method with 
        /// MaximumSeparation=6, FixedSeparation=7 and SeparationFactor=0 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with 
        /// MaximumSeparation=6, FixedSeparation=7 and SeparationFactor=0 
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ClusterBuilderWithMaximumFixedAndSeparationFactor()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.MinimumScoreGreaterSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder,
                PropertyParameters.MaximumFixedAndSeparationFactor);
        }

        #endregion Suffix Tree Test Cases

        #region NUCmer Align Test Cases

        /// <summary>
        /// Validate Align() method with one line Dna sequence 
        /// and validate the aligned sequences
        /// Input : One line Dna sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignDnaSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.DnaNucmerSequenceNodeName, false, false);
        }

        /// <summary>
        /// Validate Align() method with one line Rna sequence 
        /// and validate the aligned sequences
        /// Input : One line Rna sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignRnaSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.RnaNucmerSequenceNodeName, false, false);
        }

        /// <summary>
        /// Validate Align() method with one line list of sequence 
        /// and validate the aligned sequences
        /// Input : One line list of sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignOneLineListOfSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneLineOneReferenceQuerySequenceNodeName,
                false, true);
        }

        /// <summary>
        /// Validate Align() method with small size list of sequence 
        /// and validate the aligned sequences
        /// Input : small size list of sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSmallSizeListOfSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneLineOneReferenceQuerySequenceNodeName,
                false, true);
        }

        /// <summary>
        /// Validate Align() method with one line Dna list of sequence 
        /// and validate the aligned sequences
        /// Input : One line Dna list of sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignOneLineDnaListOfSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.SingleDnaNucmerSequenceNodeName, false, true);
        }

        /// <summary>
        /// Validate Align() method with one line Rna list of sequence 
        /// and validate the aligned sequences
        /// Input : One line Rna list of sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignOneLineRnaListOfSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.SingleRnaNucmerSequenceNodeName, false, true);
        }

        /// <summary>
        /// Validate Align() method with medium size sequence 
        /// and validate the aligned sequences
        /// Input : Medium size sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignMediumSizeSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.MediumSizeSequenceNodeName, true, false);
        }

        /// <summary>
        /// Validate Align() method with One Line Repeating Characters sequence 
        /// and validate the aligned sequences
        /// Input : One Line Repeating Characters sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignRepeatingCharactersSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneLineRepeatingCharactersNodeName, false, false);
        }

        /// <summary>
        /// Validate Align() method with One Line Alternate Repeating Characters sequence 
        /// and validate the aligned sequences
        /// Input : One Line Alternate Repeating Characters sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignAlternateRepeatingCharactersSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneLineAlternateRepeatingCharactersNodeName,
                false, false);
        }

        /// <summary>
        /// Validate Align() method with FastA file sequence 
        /// and validate the aligned sequences
        /// Input : FastA file sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignFastASequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.SimpleDnaFastaNodeName, true, false,
                ParserParameters.FastA);
        }

        /// <summary>
        /// Validate Align() method with Genbank file sequence 
        /// and validate the aligned sequences
        /// Input : Genbank file sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignGenbankSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.SimpleDnaGenBankNodeName, true, false,
                ParserParameters.GenBank);
        }

        /// <summary>
        /// Validate Align() method with Gff file sequence 
        /// and validate the aligned sequences
        /// Input : Gff file sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignGffSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.SimpleDnaGffNodeName, true, false,
                ParserParameters.Gff);
        }

        /// <summary>
        /// Validate Align() method with One Line only Repeating Characters sequence 
        /// and validate the aligned sequences
        /// Input : One Line only Repeating Characters sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignOnlyRepeatingCharactersSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneLineOnlyRepeatingCharactersNodeName,
                false, false);
        }

        /// <summary>
        /// Validate Align() method with one reference multi search sequence 
        /// and validate the aligned sequences
        /// Input : one reference multi search sequence file
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignOneRefMultiSearchSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.SmallSizeSequenceNodeName, true, false);
        }

        /// <summary>
        /// Validate Align() method with valid MUM length 
        /// and validate the aligned sequences
        /// Input : One line sequence with valid MUM length
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignOneLineSequenceValidMumLength()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneLineSequenceNodeName, false, false);
        }

        /// <summary>
        /// Validate Align() method with One Line same sequences
        /// and validate the aligned sequences
        /// Input : One Line same sequences
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSameSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.SameSequenceNodeName, false, false);
        }

        /// <summary>
        /// Validate Align() method with overlap sequences
        /// and validate the aligned sequences
        /// Input : One Line overlap sequences
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignOverlapMatchSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneOverlapMatchSequenceNodeName, false, false);
        }

        /// <summary>
        /// Validate Align() method with no match sequences
        /// and validate the aligned sequences
        /// Input : One Line no match sequences
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignNoMatchSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneLineNoMatchSequenceNodeName, false, false);
        }

        /// <summary>
        /// Validate Align() method with cross overlap sequences
        /// and validate the aligned sequences
        /// Input : One Line cross overlap sequences
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignCrossOverlapMatchSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.TwoUniqueMatchWithCrossOverlapSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate Align() method with one line reference Dna sequence with ambiguity
        /// and validate the aligned sequences
        /// Input : One line Dna sequence with ambiguity
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignDnaReferenceAmbiguitySequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.DnaAmbiguityReferenceSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate Align() method with one line reference Rna sequence with ambiguity
        /// and validate the aligned sequences
        /// Input : One line Rna sequence with ambiguity
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignRnaReferenceAmbiguitySequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.RnaAmbiguityReferenceSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate Align() method with one line search Dna sequence with ambiguity
        /// and validate the aligned sequences
        /// Input : One line Dna sequence with ambiguity
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignDnaSearchAmbiguitySequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.DnaAmbiguitySearchSequenceNodeName, false, false);
        }

        /// <summary>
        /// Validate Align() method with one line search Rna sequence with ambiguity
        /// and validate the aligned sequences
        /// Input : One line Rna sequence with ambiguity
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignRnaSearchAmbiguitySequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.RnaAmbiguitySearchSequenceNodeName, false, false);
        }

        /// <summary>
        /// Validate Align() method with one ref. and one query sequence 
        /// and validate the aligned sequences
        /// Input : one reference and one query sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignOneRefOneQuerySequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.SingleDnaNucmerSequenceNodeName, false, false);
        }

        /// <summary>
        /// Validate Align() method with multi reference one search sequence 
        /// and validate the aligned sequences
        /// Input : multiple reference one search sequence file
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignMultiRefOneSearchSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.MultiRefSingleQueryMatchSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate Align() method with multi reference multi search sequence 
        /// and validate the aligned sequences
        /// Input : multiple reference multi search sequence file
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignMultiRefMultiSearchSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneLineSequenceNodeName, false, false);
        }

        /// <summary>
        /// Validate BuildCluster() method with Minimum Score set to 0 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with minimum score 0
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignWithMinimumScoreZero()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneUniqueMatchSequenceNodeName,
                false, false, ParserParameters.FastA, AdditionalParameters.Default,
                PropertyParameters.MinimumScore);
        }

        /// <summary>
        /// Validate BuildCluster() method with MaximumSeparation set to 0 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with MaximumSeparation 0
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignWithMaximumSeparationZero()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneUniqueMatchSequenceNodeName,
                  false, false, ParserParameters.FastA, AdditionalParameters.Default,
                  PropertyParameters.MaximumSeparation);
        }

        /// <summary>
        /// Validate BuildCluster() method with SeperationFactor set to 0 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with SeperationFactor 0
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignWithSeperationFactoreZero()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneUniqueMatchSequenceNodeName,
                false, false, ParserParameters.FastA, AdditionalParameters.Default,
                PropertyParameters.SeparationFactor);
        }

        /// <summary>
        /// Validate BuildCluster() method with FixedSeparation set to 0 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with FixedSeparation 0
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignWithFixedSeparationZero()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneUniqueMatchSequenceNodeName,
                false, false, ParserParameters.FastA, AdditionalParameters.Default,
                PropertyParameters.FixedSeparation);
        }

        /// <summary>
        /// Validate BuildCluster() method with 
        /// MinimumScore set to greater than MUMlength 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with 
        /// MinimumScore set to greater than MUMlength
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignWithMinimumScoreGreater()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.MinimumScoreGreaterSequenceNodeName,
                false, false, ParserParameters.FastA, AdditionalParameters.Default,
                PropertyParameters.MinimumScore);
        }

        /// <summary>
        /// Validate BuildCluster() method with 
        /// FixedSeparation set to postive value and SeparationFactor=0 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with 
        /// FixedSeparation set to postive value and SeparationFactor=0
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignWithFixedSeparationAndSeparationFactor()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.MinimumScoreGreaterSequenceNodeName,
                false, false, ParserParameters.FastA, AdditionalParameters.Default,
                PropertyParameters.FixedSeparationAndSeparationFactor);
        }

        /// <summary>
        /// Validate BuildCluster() method with 
        /// MaximumSeparation=6, FixedSeparation=7 and SeparationFactor=0 
        /// and validate the clusters
        /// Input : Reference and Search Sequences with 
        /// MaximumSeparation=6, FixedSeparation=7 and SeparationFactor=0 
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignWithMaximumFixedAndSeparationFactor()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.MinimumScoreGreaterSequenceNodeName,
                false, false, ParserParameters.FastA, AdditionalParameters.Default,
                PropertyParameters.MaximumFixedAndSeparationFactor);
        }

        #endregion NUCmer Align Test Cases

        #region NUCmer Align Simple Test Cases

        /// <summary>
        /// Validate AlignSimple() method with Dna sequence 
        /// and validate the aligned sequences
        /// Input : Dna sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSimpleDnaSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SimpleDnaNucmerSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate AlignSimple() method with Rna sequence 
        /// and validate the aligned sequences
        /// Input : Rna sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSimpleRnaSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SimpleRnaNucmerSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate AlignSimple() method with one line Dna list of sequence 
        /// and validate the aligned sequences
        /// Input : Dna list of sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSimpleDnaListOfSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SingleDnaNucmerSequenceNodeName,
                false, true);
        }

        /// <summary>
        /// Validate AlignSimple() method with one line Rna list of sequence 
        /// and validate the aligned sequences
        /// Input : Rna list of sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSimpleRnaListOfSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SingleRnaNucmerSequenceNodeName,
                false, true);
        }

        /// <summary>
        /// Validate AlignSimple() method with medium size sequence 
        /// and validate the aligned sequences
        /// Input : Medium size sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSimpleMediumSizeSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SimpleAlignMediumSizeSequence,
                true, false);
        }

        /// <summary>
        /// Validate AlignSimple() method with one line Dna sequence 
        /// and validate the aligned sequences
        /// Input : Single Reference and Single Query Dna sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSimpleSingleRefSingleQueryDnaSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SingleDnaNucmerSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate AlignSimple() method with one line Rna sequence 
        /// and validate the aligned sequences
        /// Input : Single Reference and Single Query Rna sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSimpleSingleRefSingleQueryRnaSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SingleRnaNucmerSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate AlignSimple() method with one line Dna sequence 
        /// and validate the aligned sequences
        /// Input : Single Reference and Multi Query Dna sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSimpleSingleRefMultiQueryDnaSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SingleDnaNucmerSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate AlignSimple() method with one line Rna sequence 
        /// and validate the aligned sequences
        /// Input : Single Reference and Multi Query Rna sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSimpleSingleRefMultiQueryRnaSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SingleRnaNucmerSequenceNodeName,
                false, false);
        }

        /// <summary>
        /// Validate AlignSimple() method with one line Dna sequence 
        /// and validate the aligned sequences
        /// Input : Multi Reference and Multi Query Dna sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSimpleMultiRefMultiQueryDnaSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.MultiRefMultiQueryDnaMatchSequence,
                false, false);
        }

        /// <summary>
        /// Validate AlignSimple() method with one line Rna sequence 
        /// and validate the aligned sequences
        /// Input : Multi Reference and Multi Query Rna sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAlignSimpleMultiRefMultiQueryRnaSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.MultiRefMultiQueryRnaMatchSequence,
                false, false);
        }

        /// <summary>
        /// Validate GapOpenCost and GapExtensionCost properties in NUCmer class
        /// Input : Create a NUCmer object
        /// Validation : Validate the properties
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerProperties()
        {
            NUCmer nucmerObj = new NUCmer3();
            Assert.AreEqual(Constants.NUCGapOpenCost, nucmerObj.GapOpenCost.ToString((IFormatProvider)null));
            Assert.AreEqual(Constants.NUCGapExtensionCostNode, nucmerObj.GapExtensionCost.ToString((IFormatProvider)null));
            Console.WriteLine("Successfully validated all the properties of NUCmer class.");
            ApplicationLog.WriteLine("Successfully validated all the properties of NUCmer class.");
        }

        /// <summary>
        /// Validate NUCmerAttributes class constructor
        /// Input : Create a NUCmerAttributes object
        /// Validation : Validate the attributes
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void NUCmerAttributes()
        {
            NUCmerAttributes nucmerAttri = new NUCmerAttributes();
            Assert.IsNotNull(nucmerAttri.Attributes);
            Console.WriteLine("Successfully validated all the properties of NUCmer class.");
            ApplicationLog.WriteLine("Successfully validated all the properties of NUCmer class.");
        }

        #endregion NUCmer Align Simple Test Cases

        #region Supported Methods

        /// <summary>
        /// Validates most of the build suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is file path?</param>
        void ValidateBuildSuffixTreeGeneralTestCases(string nodeName, bool isFilePath)
        {
            ValidateBuildSuffixTreeGeneralTestCases(nodeName, isFilePath, ParserParameters.FastA);
        }

        /// <summary>
        /// Validates most of the build suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is file path?</param>
        /// <param name="parserParam">Parser parameters</param>
        void ValidateBuildSuffixTreeGeneralTestCases(string nodeName, bool isFilePath,
            ParserParameters parserParam)
        {
            ISequence referenceSeqs = null;
            string[] referenceSequences = null;

            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "NUCmer P1 : Successfully validated the File Path '{0}'.", filePath));

                IList<ISequence> referenceSeqList = null;
                switch (parserParam)
                {
                    case ParserParameters.GenBank:
                        GenBankParser genBankParserObj = new GenBankParser();

                        referenceSeqList = genBankParserObj.Parse(filePath);
                        isFilePath = false;

                        break;
                    case ParserParameters.Gff:
                        GffParser gffParserObj = new GffParser();

                        referenceSeqList = gffParserObj.Parse(filePath);
                        isFilePath = false;

                        break;
                    default:
                        using (FastaParser fastaParserObj = new FastaParser())
                        {
                            referenceSeqList = fastaParserObj.Parse(filePath);
                        }
                        break;
                }

                referenceSeqs = new SegmentedSequence(referenceSeqList);
            }
            else
            {
                // Gets the reference sequences from the configurtion file
                referenceSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                    Constants.ReferenceSequencesNode);

                IAlphabet seqAlphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                       Constants.AlphabetNameNode));

                List<ISequence> refSeqList = new List<ISequence>();

                for (int i = 0; i < referenceSequences.Length; i++)
                {
                    ISequence referSeq = new Sequence(seqAlphabet, referenceSequences[i]);
                    refSeqList.Add(referSeq);
                }

                referenceSeqs = new SegmentedSequence(refSeqList);
            }

            // Builds the suffix for the reference sequence passed.
            ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
            SequenceSuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSeqs);

            // Validates the edges for a given sequence.
            ApplicationLog.WriteLine("NUCmer P1 : Validating the Edges");
            Assert.IsTrue(ValidateEdges(suffixTree, nodeName, isFilePath));
            Console.WriteLine(
                "NUCmer P1 : Successfully validated the all the Edges for the sequence specified.");
            ApplicationLog.WriteLine(
                "NUCmer P1 : Successfully validated the all the Edges for the sequence specified.");
        }

        /// <summary>
        /// Validates most of the find matches suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is File Path?</param>
        /// <param name="additionalParam">LIS action type enum</param>
        void ValidateFindMatchSuffixGeneralTestCases(string nodeName, bool isFilePath,
            AdditionalParameters additionalParam)
        {
            ValidateFindMatchSuffixGeneralTestCases(nodeName, isFilePath, additionalParam,
                PropertyParameters.Default);
        }

        /// <summary>
        /// Validates most of the find matches suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is File Path?</param>
        /// <param name="additionalParam">LIS action type enum</param>
        /// <param name="propParam">Property parameters</param>
        void ValidateFindMatchSuffixGeneralTestCases(string nodeName, bool isFilePath,
            AdditionalParameters additionalParam, PropertyParameters propParam)
        {
            ISequence referenceSeqs = null;
            ISequence searchSeqs = null;
            string[] referenceSequences = null;
            string[] searchSequences = null;
            IList<ISequence> referenceSeqList = null;

            if (isFilePath)
            {
                // Gets the reference sequence from the FastA file
                string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "NUCmer P1 : Successfully validated the File Path '{0}'.", filePath));

                using (FastaParser parser = new FastaParser())
                {
                    referenceSeqList = parser.Parse(filePath);
                    referenceSeqs = new SegmentedSequence(referenceSeqList);

                    // Gets the query sequence from the FastA file
                    string queryFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.SearchSequenceFilePathNode);

                    Assert.IsNotNull(queryFilePath);
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "NUCmer P1 : Successfully validated the File Path '{0}'.", queryFilePath));

                    IList<ISequence> querySeqList = parser.Parse(queryFilePath);
                    searchSeqs = new SegmentedSequence(querySeqList);
                }
            }
            else
            {
                // Gets the reference & search sequences from the configurtion file
                referenceSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                    Constants.ReferenceSequencesNode);
                searchSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                  Constants.SearchSequencesNode);

                IAlphabet seqAlphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                       Constants.AlphabetNameNode));

                List<ISequence> refSeqList = new List<ISequence>();
                List<ISequence> searchSeqList = new List<ISequence>();
                for (int i = 0; i < referenceSequences.Length; i++)
                {
                    ISequence referSeq = new Sequence(seqAlphabet, referenceSequences[i]);
                    refSeqList.Add(referSeq);
                }

                referenceSeqs = new SegmentedSequence(refSeqList);
                for (int i = 0; i < searchSequences.Length; i++)
                {
                    ISequence searchSeq = new Sequence(seqAlphabet, searchSequences[i]);
                    searchSeqList.Add(searchSeq);
                }

                searchSeqs = new SegmentedSequence(searchSeqList);
            }

            string mumLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMLengthNode);

            // Builds the suffix for the reference sequence passed.
            ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
            SequenceSuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSeqs);

            IList<MaxUniqueMatch> matches = suffixTreeBuilder.FindMatches(suffixTree, searchSeqs,
                long.Parse(mumLength, null));

            switch (additionalParam)
            {
                case AdditionalParameters.FindUniqueMatches:
                    // Validates the Unique Matches.
                    ApplicationLog.WriteLine("NUCmer P1 : Validating the Unique Matches");
                    Assert.IsTrue(ValidateUniqueMatches(matches, nodeName, isFilePath));
                    Console.WriteLine(
                        "NUCmer P1 : Successfully validated the all the unique matches for the sequences.");
                    break;
                case AdditionalParameters.PerformClusterBuilder:
                    // Validates the Unique Matches.
                    ApplicationLog.WriteLine(
                        "NUCmer P1 : Validating the Unique Matches using Cluster Builder");
                    Assert.IsTrue(ValidateClusterBuilderMatches(matches, nodeName, propParam));
                    Console.WriteLine(
                        "NUCmer P1 : Successfully validated the all the cluster builder matches for the sequences.");
                    break;
                default:
                    break;
            }


            ApplicationLog.WriteLine(
                "NUCmer P1 : Successfully validated the all the unique matches for the sequences.");
        }

        /// <summary>
        /// Validates the NUCmer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isFilePath">Is Sequence saved in File</param>
        /// <param name="isAlignList">Is align method to take list?</param>
        void ValidateNUCmerAlignGeneralTestCases(string nodeName, bool isFilePath,
            bool isAlignList)
        {
            ValidateNUCmerAlignGeneralTestCases(nodeName, isFilePath, isAlignList, ParserParameters.FastA);
        }

        /// <summary>
        /// Validates the NUCmer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isFilePath">Is Sequence saved in File</param>
        /// <param name="isAlignList">Is align method to take list?</param>
        void ValidateNUCmerAlignSimpleGeneralTestCases(string nodeName, bool isFilePath,
            bool isAlignList)
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(nodeName, isFilePath, isAlignList,
                ParserParameters.FastA);
        }

        /// <summary>
        /// Validates the NUCmer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isFilePath">Is Sequence saved in File</param>
        /// <param name="isAlignList">Is align method to take list?</param>
        /// <param name="parserParam">Parser type</param>
        void ValidateNUCmerAlignGeneralTestCases(string nodeName, bool isFilePath, bool isAlignList,
            ParserParameters parserParam)
        {
            ValidateNUCmerAlignGeneralTestCases(nodeName, isFilePath, isAlignList, parserParam,
                AdditionalParameters.Default, PropertyParameters.Default);
        }

        /// <summary>
        /// Validates the NUCmer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isFilePath">Is Sequence saved in File</param>
        /// <param name="isAlignList">Is align method to take list?</param>
        /// <param name="parserParam">Parser type</param>
        /// <param name="addParam">Additional parameters</param>
        /// <param name="propParam">Property parameters</param>
        void ValidateNUCmerAlignGeneralTestCases(string nodeName, bool isFilePath, bool isAlignList,
            ParserParameters parserParam, AdditionalParameters addParam, PropertyParameters propParam)
        {
            string[] referenceSequences = null;
            string[] searchSequences = null;
            IList<ISequence> refSeqList = new List<ISequence>();
            IList<ISequence> searchSeqList = new List<ISequence>();

            if (isFilePath)
            {
                // Gets the reference sequence from the FastA file
                string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "NUCmer P1 : Successfully validated the File Path '{0}'.", filePath));

                switch (parserParam)
                {
                    case ParserParameters.GenBank:
                        GenBankParser genBankParserObj = new GenBankParser();
                        refSeqList = genBankParserObj.Parse(filePath);
                        isFilePath = false;

                        break;
                    case ParserParameters.Gff:
                        GffParser gffParserObj = new GffParser();

                        refSeqList = gffParserObj.Parse(filePath);
                        isFilePath = false;

                        break;
                    default:
                        using (FastaParser fastaParserObj = new FastaParser())
                        {
                            refSeqList = fastaParserObj.Parse(filePath);
                        }
                        break;
                }

                // Gets the query sequence from the FastA file
                string queryFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceFilePathNode);

                Assert.IsNotNull(queryFilePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "NUCmer P1 : Successfully validated the File Path '{0}'.", queryFilePath));

                switch (parserParam)
                {
                    case ParserParameters.GenBank:
                        GenBankParser genBankParserObj = new GenBankParser();
                        searchSeqList = genBankParserObj.Parse(queryFilePath);
                        break;
                    case ParserParameters.Gff:
                        GffParser gffParserObj = new GffParser();
                        searchSeqList = gffParserObj.Parse(queryFilePath);
                        break;
                    default:
                        using (FastaParser fastaParserObj = new FastaParser())
                        {
                            searchSeqList = fastaParserObj.Parse(queryFilePath);
                        }
                        break;
                }
            }
            else
            {
                // Gets the reference & search sequences from the configurtion file
                referenceSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                    Constants.ReferenceSequencesNode);
                searchSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                  Constants.SearchSequencesNode);

                IAlphabet seqAlphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                       Constants.AlphabetNameNode));

                for (int i = 0; i < referenceSequences.Length; i++)
                {
                    ISequence referSeq = new Sequence(seqAlphabet, referenceSequences[i]);
                    refSeqList.Add(referSeq);
                }

                for (int i = 0; i < searchSequences.Length; i++)
                {
                    ISequence searchSeq = new Sequence(seqAlphabet, searchSequences[i]);
                    searchSeqList.Add(searchSeq);
                }
            }
            // Gets the mum length from the xml
            string mumLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode);

            NUCmer nucmerObj = new NUCmer3();
            // Check for additional parameters and update the object accordingly
            switch (addParam)
            {
                case AdditionalParameters.AlignSimilarityMatrix:
                    nucmerObj.SimilarityMatrix = new SimilarityMatrix(
                        SimilarityMatrix.StandardSimilarityMatrix.Blosum50);
                    break;
                default:
                    break;
            }
            // Update other values for NUCmer object
            nucmerObj.MaximumSeparation = 0;
            nucmerObj.MinimumScore = 2;
            nucmerObj.SeparationFactor = 0.12f;
            nucmerObj.BreakLength = 2;
            nucmerObj.LengthOfMUM = long.Parse(mumLength, null);

            switch (propParam)
            {
                case PropertyParameters.MinimumScore:
                    nucmerObj.MinimumScore = int.Parse(
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MinimumScoreNode), (IFormatProvider)null);
                    break;
                case PropertyParameters.MaximumSeparation:
                    nucmerObj.MaximumSeparation = int.Parse(
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MaximumSeparationNode), (IFormatProvider)null);
                    break;
                case PropertyParameters.FixedSeparation:
                    nucmerObj.FixedSeparation = int.Parse(
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FixedSeparationNode), (IFormatProvider)null);
                    break;
                case PropertyParameters.SeparationFactor:
                    nucmerObj.SeparationFactor = int.Parse(
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SeparationFactorNode), (IFormatProvider)null);
                    break;
                case PropertyParameters.FixedSeparationAndSeparationFactor:
                    nucmerObj.SeparationFactor = int.Parse(
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SeparationFactorNode), (IFormatProvider)null);
                    nucmerObj.FixedSeparation = int.Parse(
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FixedSeparationNode), (IFormatProvider)null);
                    break;
                case PropertyParameters.MaximumFixedAndSeparationFactor:
                    nucmerObj.MaximumSeparation = int.Parse(
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MaximumSeparationNode), (IFormatProvider)null);
                    nucmerObj.SeparationFactor = int.Parse(
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SeparationFactorNode), (IFormatProvider)null);
                    nucmerObj.FixedSeparation = int.Parse(
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.FixedSeparationNode), (IFormatProvider)null);
                    break;
                default:
                    break;
            }

            IList<IPairwiseSequenceAlignment> align = null;

            if (isAlignList)
            {
                List<ISequence> listOfSeq = new List<ISequence>();
                listOfSeq.Add(refSeqList[0]);
                listOfSeq.Add(searchSeqList[0]);
                align = nucmerObj.Align(listOfSeq);
            }
            else
            {
                align = nucmerObj.Align(refSeqList, searchSeqList);
            }

            string expectedSequences = string.Empty;

            if (isFilePath)
                expectedSequences = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                    Constants.ExpectedSequencesNode);
            else
                expectedSequences = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.ExpectedSequencesNode);
            string[] expSeqArray = expectedSequences.Split(',');

            int j = 0;

            // Gets all the aligned sequences in comma seperated format
            foreach (IPairwiseSequenceAlignment seqAlignment in align)
            {
                foreach (PairwiseAlignedSequence alignedSeq in seqAlignment)
                {
                    Assert.AreEqual(expSeqArray[j], alignedSeq.FirstSequence.ToString());
                    ++j;
                    Assert.AreEqual(expSeqArray[j], alignedSeq.SecondSequence.ToString());
                    j++;
                }
            }

            Console.WriteLine("NUCmer P1 : Successfully validated all the aligned sequences.");
            ApplicationLog.WriteLine("NUCmer P1 : Successfully validated all the aligned sequences.");
        }

        /// <summary>
        /// Validates the NUCmer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isFilePath">Is Sequence saved in File</param>
        /// <param name="isAlignList">Is align method to take list?</param>
        /// <param name="parserParam">Parser type</param>
        void ValidateNUCmerAlignSimpleGeneralTestCases(string nodeName,
            bool isFilePath, bool isAlignList, ParserParameters parserParam)
        {
            string[] referenceSequences = null;
            string[] searchSequences = null;
            IList<ISequence> refSeqList = new List<ISequence>();
            IList<ISequence> searchSeqList = new List<ISequence>();

            if (isFilePath)
            {
                // Gets the reference sequence from the FastA file
                string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "NUCmer P1 : Successfully validated the File Path '{0}'.", filePath));

                switch (parserParam)
                {
                    case ParserParameters.GenBank:
                        GenBankParser genBankParserObj = new GenBankParser();
                        refSeqList = genBankParserObj.Parse(filePath);
                        isFilePath = false;
                        break;
                    case ParserParameters.Gff:
                        GffParser gffParserObj = new GffParser();
                        refSeqList = gffParserObj.Parse(filePath);
                        isFilePath = false;
                        break;
                    default:
                        using (FastaParser fastaParserObj = new FastaParser())
                        {
                            refSeqList = fastaParserObj.Parse(filePath);
                        }
                        break;
                }

                // Gets the query sequence from the FastA file
                string queryFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceFilePathNode);

                Assert.IsNotNull(queryFilePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "NUCmer P1 : Successfully validated the File Path '{0}'.", queryFilePath));

                switch (parserParam)
                {
                    case ParserParameters.GenBank:
                        GenBankParser genBankParserObj = new GenBankParser();
                        searchSeqList = genBankParserObj.Parse(queryFilePath);
                        break;
                    case ParserParameters.Gff:
                        GffParser gffParserObj = new GffParser();
                        searchSeqList = gffParserObj.Parse(queryFilePath);
                        break;
                    default:
                        using (FastaParser fastaParserObj = new FastaParser())
                        {
                            searchSeqList = fastaParserObj.Parse(queryFilePath);
                        }
                        break;
                }
            }
            else
            {
                // Gets the reference & search sequences from the configurtion file
                referenceSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                    Constants.ReferenceSequencesNode);
                searchSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                  Constants.SearchSequencesNode);

                IAlphabet seqAlphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                       Constants.AlphabetNameNode));

                for (int i = 0; i < referenceSequences.Length; i++)
                {
                    ISequence referSeq = new Sequence(seqAlphabet, referenceSequences[i]);
                    refSeqList.Add(referSeq);
                }

                for (int i = 0; i < searchSequences.Length; i++)
                {
                    ISequence searchSeq = new Sequence(seqAlphabet, searchSequences[i]);
                    searchSeqList.Add(searchSeq);
                }
            }
            // Gets the mum length from the xml
            string mumLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode);

            NUCmer nucmerObj = new NUCmer3();

            // Update other values for NUCmer object
            nucmerObj.MaximumSeparation = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.MUMAlignLengthNode), (IFormatProvider)null);
            nucmerObj.MinimumScore = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.MUMAlignLengthNode), (IFormatProvider)null);
            nucmerObj.SeparationFactor = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.MUMAlignLengthNode), (IFormatProvider)null);
            nucmerObj.BreakLength = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.MUMAlignLengthNode), (IFormatProvider)null);
            nucmerObj.LengthOfMUM = long.Parse(mumLength, (IFormatProvider)null);

            IList<IPairwiseSequenceAlignment> alignSimple = null;

            if (isAlignList)
            {
                List<ISequence> listOfSeq = new List<ISequence>();
                listOfSeq.Add(refSeqList[0]);
                listOfSeq.Add(searchSeqList[0]);
                alignSimple = nucmerObj.AlignSimple(listOfSeq);
            }
            else
            {
                alignSimple = nucmerObj.AlignSimple(refSeqList, searchSeqList);
            }

            string expectedSequences = string.Empty;

            if (isFilePath)
                expectedSequences = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                    Constants.ExpectedSequencesNode);
            else
                expectedSequences = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.ExpectedSequencesNode);

            string[] expSeqArray = expectedSequences.Split(',');

            int j = 0;

            // Gets all the aligned sequences in comma seperated format
            foreach (IPairwiseSequenceAlignment seqAlignment in alignSimple)
            {
                foreach (PairwiseAlignedSequence alignedSeq in seqAlignment)
                {
                    Assert.AreEqual(expSeqArray[j], alignedSeq.FirstSequence.ToString());
                    ++j;
                    Assert.AreEqual(expSeqArray[j], alignedSeq.SecondSequence.ToString());
                    j++;
                }
            }

            Console.WriteLine("NUCmer P1 : Successfully validated all the aligned sequences.");
            ApplicationLog.WriteLine("NUCmer P1 : Successfully validated all the aligned sequences.");
        }

        /// <summary>
        /// Validates the edges for the suffix tree and the node name specified.
        /// </summary>
        /// <param name="suffixTree">Suffix Tree.</param>
        /// <param name="nodeName">Node name which needs to be read for validation</param>
        /// <param name="isFilePath">Edges to be read from Text file?</param>
        /// <returns>True, if successfully validated.</returns>
        bool ValidateEdges(SequenceSuffixTree suffixTree, string nodeName, bool isFilePath)
        {
            Dictionary<int, Edge> ed = suffixTree.Edges;

            string[] actualStrtIndexes = new string[ed.Count];
            string[] actualEndIndexes = new string[ed.Count];
            // Gets all the edges to be validated as in xml.
            string[] startIndexes = null;
            string[] endIndexes = null;

            int j = 0;
            foreach (int col in ed.Keys)
            {
                Edge a = ed[col];
                actualStrtIndexes[j] = a.StartIndex.ToString((IFormatProvider)null);
                actualEndIndexes[j] = a.EndIndex.ToString((IFormatProvider)null);
                j++;
            }

            // Gets the sorted edge list for the actual Edge list
            List<Edge> actualEdgeList = GetSortedEdges(actualStrtIndexes, actualEndIndexes);

            // Gets all the edges to be validated as in xml.
            if (isFilePath)
            {
                // Gets all the edges to be validated from the text file.
                startIndexes = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                    Constants.EdgeStartIndexesNode).Split(',');
                endIndexes = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                    Constants.EdgeEndIndexesNode).Split(',');
            }
            else
            {
                // Gets all the edges to be validated as in xml.
                startIndexes = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.EdgeStartIndexesNode).Split(',');
                endIndexes = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.EdgeEndIndexesNode).Split(',');
            }

            // Gets the sorted edge list for the expected Edge list
            List<Edge> expectedEdgeList = GetSortedEdges(startIndexes, endIndexes);

            Console.WriteLine(string.Format((IFormatProvider)null,
                "NUCmer P1 : Total Edges Found is : '{0}'", ed.Keys.Count.ToString((IFormatProvider)null)));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "NUCmer P1 : Total Edges Found is : '{0}'", ed.Keys.Count.ToString((IFormatProvider)null)));

            // Loops through all the edges and validates the same.
            for (int i = 0; i < expectedEdgeList.Count; i++)
            {
                if (!(actualEdgeList[i].StartIndex == expectedEdgeList[i].StartIndex)
                    && (actualEdgeList[i].EndIndex == expectedEdgeList[i].EndIndex))
                {
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "NUCmer P1 : Edges not matching at index '{0}'", i.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "NUCmer P1 : Edges not matching at index '{0}'", i.ToString((IFormatProvider)null)));
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
        private static List<Edge> GetSortedEdges(string[] startIndexes, string[] endIndexes)
        {
            List<Edge> edgList = new List<Edge>();

            // Loops through all the indexes and creates EdgeList.
            for (int i = 0; i < startIndexes.Length; i++)
            {
                Edge edg = new Edge();
                edg.StartIndex = int.Parse(startIndexes[i], (IFormatProvider)null);
                edg.EndIndex = int.Parse(endIndexes[i], (IFormatProvider)null);

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
        /// <param name="isFilePath">Nodes to be read from Text file?</param>
        /// <returns>True, if successfully validated</returns>
        bool ValidateUniqueMatches(IList<MaxUniqueMatch> matches,
            string nodeName, bool isFilePath)
        {
            // Gets all the unique matches properties to be validated as in xml.
            string[] firstSeqOrder = null;
            string[] firstSeqStart = null;
            string[] length = null;
            string[] secondSeqOrder = null;
            string[] secondSeqStart = null;

            if (isFilePath)
            {
                firstSeqOrder = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                    Constants.FirstSequenceMumOrderNode).Split(',');
                firstSeqStart = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                    Constants.FirstSequenceStartNode).Split(',');
                length = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                    Constants.LengthNode).Split(',');
                secondSeqOrder = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                    Constants.SecondSequenceMumOrderNode).Split(',');
                secondSeqStart = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                    Constants.SecondSequenceStartNode).Split(',');
            }
            else
            {
                firstSeqOrder = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FirstSequenceMumOrderNode).Split(',');
                firstSeqStart = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FirstSequenceStartNode).Split(',');
                length = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.LengthNode).Split(',');
                secondSeqOrder = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SecondSequenceMumOrderNode).Split(',');
                secondSeqStart = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SecondSequenceStartNode).Split(',');
            }

            int i = 0;
            // Loops through all the matches and validates the same.
            foreach (MaxUniqueMatch match in matches)
            {
                if ((0 != string.Compare(firstSeqOrder[i],
                    match.FirstSequenceMumOrder.ToString((IFormatProvider)null), true,
                    CultureInfo.CurrentCulture))
                    || (0 != string.Compare(firstSeqStart[i],
                    match.FirstSequenceStart.ToString((IFormatProvider)null), true,
                    CultureInfo.CurrentCulture))
                    || (0 != string.Compare(length[i],
                    match.Length.ToString((IFormatProvider)null), true, CultureInfo.CurrentCulture))
                    || (0 != string.Compare(secondSeqOrder[i],
                    match.SecondSequenceMumOrder.ToString((IFormatProvider)null), true,
                    CultureInfo.CurrentCulture))
                    || (0 != string.Compare(secondSeqStart[i],
                    match.SecondSequenceStart.ToString((IFormatProvider)null), true,
                    CultureInfo.CurrentCulture)))
                {
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "NUCmer P1 : Unique match not matching at index '{0}'", i.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "NUCmer P1 : Unique match not matching at index '{0}'", i.ToString((IFormatProvider)null)));
                    return false;
                }
                i++;
            }

            return true;
        }

        /// <summary>
        /// Validates the Cluster Builder Matches for the input provided.
        /// </summary>
        /// <param name="matches">Max Unique Match list</param>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="propParam">Property parameters</param>
        /// <returns>True, if successfully validated</returns>
        bool ValidateClusterBuilderMatches(IList<MaxUniqueMatch> matches,
            string nodeName, PropertyParameters propParam)
        {
            // Validates the Cluster builder MUMs
            string firstSeqOrderExpected = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ClustFirstSequenceMumOrderNode);
            string firstSeqStartExpected = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ClustFirstSequenceStartNode);
            string lengthExpected = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ClustLengthNode);
            string secondSeqOrderExpected = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ClustSecondSequenceMumOrderNode);
            string secondSeqStartExpected = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ClustSecondSequenceStartNode);

            StringBuilder firstSeqOrderActual = new StringBuilder();
            StringBuilder firstSeqStartActual = new StringBuilder();
            StringBuilder lengthActual = new StringBuilder();
            StringBuilder secondSeqOrderActual = new StringBuilder();
            StringBuilder secondSeqStartActual = new StringBuilder();

            ClusterBuilder cbObj = new ClusterBuilder();
            cbObj.MinimumScore = 0;
            switch (propParam)
            {
                case PropertyParameters.MinimumScore:
                    cbObj.MinimumScore = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.MinimumScoreNode), (IFormatProvider)null);
                    break;
                case PropertyParameters.MaximumSeparation:
                    cbObj.MaximumSeparation = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.MaximumSeparationNode), (IFormatProvider)null);
                    break;
                case PropertyParameters.FixedSeparation:
                    cbObj.FixedSeparation = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.FixedSeparationNode), (IFormatProvider)null);
                    break;
                case PropertyParameters.SeparationFactor:
                    cbObj.SeparationFactor = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.SeparationFactorNode), (IFormatProvider)null);
                    break;
                case PropertyParameters.FixedSeparationAndSeparationFactor:
                    cbObj.SeparationFactor = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.SeparationFactorNode), (IFormatProvider)null);
                    cbObj.FixedSeparation = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.FixedSeparationNode), (IFormatProvider)null);
                    break;
                case PropertyParameters.MaximumFixedAndSeparationFactor:
                    cbObj.MaximumSeparation = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.MaximumSeparationNode), (IFormatProvider)null);
                    cbObj.SeparationFactor = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.SeparationFactorNode), (IFormatProvider)null);
                    cbObj.FixedSeparation = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.FixedSeparationNode), (IFormatProvider)null);
                    break;
                default:
                    break;
            }

            IList<Cluster> clusts = cbObj.BuildClusters(matches);

            foreach (Cluster clust in clusts)
            {
                foreach (MaxUniqueMatchExtension maxMatchExtension in clust.Matches)
                {
                    firstSeqOrderActual.Append(maxMatchExtension.FirstSequenceMumOrder);
                    secondSeqOrderActual.Append(maxMatchExtension.SecondSequenceMumOrder);
                    secondSeqStartActual.Append(maxMatchExtension.SecondSequenceStart);
                    firstSeqStartActual.Append(maxMatchExtension.FirstSequenceStart);
                    lengthActual.Append(maxMatchExtension.Length);
                }
            }

            if ((0 != string.Compare(firstSeqOrderExpected.Replace(",", ""),
                firstSeqOrderActual.ToString(), true, CultureInfo.CurrentCulture))
                || (0 != string.Compare(firstSeqStartExpected.Replace(",", ""),
                firstSeqStartActual.ToString(), true, CultureInfo.CurrentCulture))
                || (0 != string.Compare(lengthExpected.Replace(",", ""),
                lengthActual.ToString(), true, CultureInfo.CurrentCulture))
                || (0 != string.Compare(secondSeqOrderExpected.Replace(",", ""),
                secondSeqOrderActual.ToString(), true, CultureInfo.CurrentCulture))
                || (0 != string.Compare(secondSeqStartExpected.Replace(",", ""),
                secondSeqStartActual.ToString(), true, CultureInfo.CurrentCulture)))
            {
                Console.WriteLine("NUCmer P1 : Cluster builder match not matching");
                ApplicationLog.WriteLine("NUCmer P1 : Cluster builder match not matching");
                return false;
            }

            return true;
        }

        #endregion Supported Methods
    }
}
