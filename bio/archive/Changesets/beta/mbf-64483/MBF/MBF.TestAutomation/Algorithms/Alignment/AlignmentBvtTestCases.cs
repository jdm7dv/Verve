// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * AlignmentBvtTestCases.cs
 * 
 *   This file contains the Alignment Bvt test cases i.e., NeedlemanWunschAligner, 
 *   SmithWatermanAlignmer, PairwiseOverlapAligner and SequenceAlignment
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

using MBF.Algorithms.Alignment;
using MBF.IO.Fasta;
using MBF.SimilarityMatrices;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.Algorithms.Alignment
{
    /// <summary>
    /// Alignment BVT test cases implementation.
    /// </summary>
    [TestClass]
    public class AlignmentBvtTestCases
    {
        #region Enums

        /// <summary>
        /// Alignment Type Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AlignmentType
        {
            SimpleAlign,
            Align
        };

        /// <summary>
        /// Alignment Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AlignmentParamType
        {
            AlignTwo,
            AlignList,
            AllParam
        };

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static AlignmentBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region NeedlemanWunschAligner BVT Test cases

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschSimpleAlignTwoSequencesFromTextFile()
        {
            ValidateNeedlemanWunschAlignment(true, AlignmentParamType.AlignTwo, false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschSimpleAlignTwoSequencesFromTextFileWithEARTHEnabled()
        {
            ValidateNeedlemanWunschAlignment(true, AlignmentParamType.AlignTwo,
                true);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is passed in code using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : sequence in xml
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschSimpleAlignTwoSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(false, AlignmentParamType.AlignTwo, false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschSimpleAlignListSequencesFromTextFile()
        {
            ValidateNeedlemanWunschAlignment(true, AlignmentParamType.AlignList,
                false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschSimpleAlignListSequencesFromTextFileWithEARTHEnabled()
        {
            ValidateNeedlemanWunschAlignment(true, AlignmentParamType.AlignList,
                true);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is passed in code using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : sequence in xml
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschSimpleAlignListSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(false, AlignmentParamType.AlignList,
                false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : Text File i.e., Fasta
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschSimpleAlignAllParamsFromTextFile()
        {
            ValidateNeedlemanWunschAlignment(true, AlignmentParamType.AllParam,
                false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : Text File i.e., Fasta
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschSimpleAlignAllParamsFromTextFileWithEARTHEnabled()
        {
            ValidateNeedlemanWunschAlignment(true, AlignmentParamType.AllParam,
                true);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is passed in code using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : sequence in xml
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschSimpleAlignAllParamsFromXml()
        {
            ValidateNeedlemanWunschAlignment(false, AlignmentParamType.AllParam,
                false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is passed in code using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : sequence in xml
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschSimpleAlignAllParamsFromXmlWithEARTHPropertyEnabled()
        {
            ValidateNeedlemanWunschAlignment(false, AlignmentParamType.AllParam,
                true);
        }

        #region Gap Extension Cost inclusion Test cases

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// which is in a text file using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschAlignTwoSequencesFromTextFile()
        {
            ValidateNeedlemanWunschAlignment(true, AlignmentParamType.AlignTwo,
                AlignmentType.Align, false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// which is in a text file using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschAlignListSequencesFromTextFile()
        {
            ValidateNeedlemanWunschAlignment(true, AlignmentParamType.AlignList,
                AlignmentType.Align, false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, 
        /// Similarity Matrix and enabled EARTH which is in a text file using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschAlignListSequencesFromTextFileWithEARTHModelEnabled()
        {
            ValidateNeedlemanWunschAlignment(true, AlignmentParamType.AlignList,
                AlignmentType.Align, true);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : Text File i.e., Fasta
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschAlignAllParamsFromTextFile()
        {
            ValidateNeedlemanWunschAlignment(true, AlignmentParamType.AllParam,
                AlignmentType.Align, false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// and Enable EARTH model which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : Text File i.e., Fasta
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void NeedlemanWunschAlignAllParamsFromTextFileWithEARTHModel()
        {
            ValidateNeedlemanWunschAlignment(true, AlignmentParamType.AllParam,
                AlignmentType.Align, true);
        }

        #endregion Gap Extension Cost inclusion Test cases

        #endregion NeedlemanWunschAligner BVT Test cases

        #region SmithWatermanAligner BVT Test cases

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanSimpleAlignTwoSequencesFromTextFile()
        {
            ValidateSmithWatermanAlignment(true, AlignmentParamType.AlignTwo, false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanSimpleAlignTwoSequencesFromTextFileWithEARTHEnabled()
        {
            ValidateSmithWatermanAlignment(true, AlignmentParamType.AlignTwo,
                true);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is passed in code using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : sequence in xml
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanSimpleAlignTwoSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(false, AlignmentParamType.AlignTwo, false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanSimpleAlignListSequencesFromTextFile()
        {
            ValidateSmithWatermanAlignment(true, AlignmentParamType.AlignList, false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanSimpleAlignListSequencesFromTextFileWithEARTHEnabled()
        {
            ValidateSmithWatermanAlignment(true, AlignmentParamType.AlignList,
                true);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is passed in code using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : sequence in xml
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanSimpleAlignListSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(false, AlignmentParamType.AlignList,
                false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : Text File i.e., Fasta
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanSimpleAlignAllParamsFromTextFile()
        {
            ValidateSmithWatermanAlignment(true, AlignmentParamType.AllParam,
                false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : Text File i.e., Fasta
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanSimpleAlignAllParamsFromTextFileWithEARTHEnabled()
        {
            ValidateSmithWatermanAlignment(true, AlignmentParamType.AllParam, true);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is passed in code using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : sequence in xml
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanSimpleAlignAllParamsFromXml()
        {
            ValidateSmithWatermanAlignment(false, AlignmentParamType.AllParam, false);
        }

        #region Gap Extension Cost inclusion Test cases

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// which is in a text file using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanAlignTwoSequencesFromTextFile()
        {
            ValidateSmithWatermanAlignment(true, AlignmentParamType.AlignTwo, AlignmentType.Align,
            false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// which is in a text file using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanAlignListSequencesFromTextFile()
        {
            ValidateSmithWatermanAlignment(true, AlignmentParamType.AlignList,
                AlignmentType.Align, false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// which is in a text file using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanAlignListSequencesFromTextFileWithEARTHEnabled()
        {
            ValidateSmithWatermanAlignment(true, AlignmentParamType.AlignList,
                AlignmentType.Align, true);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : Text File i.e., Fasta
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SmithWatermanAlignAllParamsFromTextFile()
        {
            ValidateSmithWatermanAlignment(true, AlignmentParamType.AllParam, AlignmentType.Align,
                false);
        }

        #endregion Gap Extension Cost inclusion Test cases

        #endregion SmithWatermanAligner BVT Test cases

        #region PairwiseOverlapAligner BVT Test cases

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapSimpleAlignTwoSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(true, AlignmentParamType.AlignTwo,
                false);
        }


        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapSimpleAlignTwoSequencesFromTextFileWithEARTHEnabled()
        {
            ValidatePairwiseOverlapAlignment(true, AlignmentParamType.AlignTwo,
                true);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is passed in code using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : sequence in xml
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapSimpleAlignTwoSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(false, AlignmentParamType.AlignTwo,
                false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapSimpleAlignListSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(true, AlignmentParamType.AlignList,
                false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is passed in code using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : sequence in xml
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapSimpleAlignListSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(false, AlignmentParamType.AlignList,
                false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is passed in code using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : sequence in xml
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapSimpleAlignListSequencesFromXmlWithEARTHEnabled()
        {
            ValidatePairwiseOverlapAlignment(false, AlignmentParamType.AlignList,
                true);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : Text File i.e., Fasta
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapSimpleAlignAllParamsFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(true, AlignmentParamType.AllParam,
                false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is passed in code using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : sequence in xml
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapSimpleAlignAllParamsFromXml()
        {
            ValidatePairwiseOverlapAlignment(false, AlignmentParamType.AllParam,
                false);
        }

        #region Gap Extension Cost inclusion Test cases

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// which is in a text file using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapAlignTwoSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(true, AlignmentParamType.AlignTwo,
                AlignmentType.Align, false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// which is in a text file using the method Align(sequence1, sequence2) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapAlignTwoSequencesFromTextFileWithEARTHEnabled()
        {
            ValidatePairwiseOverlapAlignment(true, AlignmentParamType.AlignTwo,
                AlignmentType.Align, true);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// which is in a text file using the method Align(List) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapAlignListSequencesFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(true, AlignmentParamType.AlignList,
                AlignmentType.Align, false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : Text File i.e., Fasta
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapAlignAllParamsFromTextFile()
        {
            ValidatePairwiseOverlapAlignment(true, AlignmentParamType.AllParam,
                AlignmentType.Align, false);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Open Cost, Gap Extension Cost, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : Text File i.e., Fasta
        /// Validation : Aligned sequence and score.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void PairwiseOverlapAlignAllParamsFromTextFileWithEARTHEnabled()
        {
            ValidatePairwiseOverlapAlignment(true, AlignmentParamType.AllParam,
                AlignmentType.Align, true);
        }

        #endregion Gap Extension Cost inclusion Test cases

        #endregion PairwiseOverlapAligner BVT Test cases

        #region Sequence Alignment BVT Test cases

        /// <summary>
        /// Pass a valid sequence to AddSequence() method and validate the same.
        /// Input : Sequence read from xml file.
        /// Validation : Added sequences are got back and validated.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void SequenceAlignmentAddSequence()
        {
            // Read the xml file for getting both the files for aligning.
            string origSequence1 = _utilityObj._xmlUtil.GetTextValue(
                Constants.AlignAlgorithmNodeName,
                Constants.SequenceNode1);
            string origSequence2 = _utilityObj._xmlUtil.GetTextValue(Constants.AlignAlgorithmNodeName,
                Constants.SequenceNode2);

            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.AlignAlgorithmNodeName,
                Constants.AlphabetNameNode));

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "SequenceAlignment BVT : First sequence used is '{0}'.",
                origSequence1));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "SequenceAlignment BVT : Second sequence used is '{0}'.",
                origSequence2));

            Console.WriteLine(string.Format((IFormatProvider)null,
                "SequenceAlignment BVT : First sequence used is '{0}'.",
                origSequence1));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SequenceAlignment BVT : Second sequence used is '{0}'.",
                origSequence2));

            // Create two sequences
            ISequence aInput = new Sequence(alphabet, origSequence1);
            ISequence bInput = new Sequence(alphabet, origSequence2);

            // Add the sequences to the Sequence alignment object using AddSequence() method.
            IList<IPairwiseSequenceAlignment> sequenceAlignmentObj =
                new List<IPairwiseSequenceAlignment>();

            PairwiseAlignedSequence alignSeq = new PairwiseAlignedSequence();
            alignSeq.FirstSequence = aInput;
            alignSeq.SecondSequence = bInput;
            IPairwiseSequenceAlignment seqAlignObj = new PairwiseSequenceAlignment();
            seqAlignObj.Add(alignSeq);
            sequenceAlignmentObj.Add(seqAlignObj);

            // Read the output back and validate the same.
            IList<PairwiseAlignedSequence> newAlignedSequences =
                sequenceAlignmentObj[0].PairwiseAlignedSequences;

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "SequenceAlignment BVT : First sequence read is '{0}'.",
                origSequence1));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "SequenceAlignment BVT : Second sequence read is '{0}'.",
                origSequence2));

            Console.WriteLine(string.Format((IFormatProvider)null,
                "SequenceAlignment BVT : First sequence read is '{0}'.",
                origSequence1));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SequenceAlignment BVT : Second sequence read is '{0}'.",
                origSequence2));

            Assert.AreEqual(newAlignedSequences[0].FirstSequence.ToString(), origSequence1);
            Assert.AreEqual(newAlignedSequences[0].SecondSequence.ToString(), origSequence2);
        }

        #endregion Sequence Alignment BVT Test cases

        #region Aligned Sequence BVT Test Cases

        /// <summary>
        /// Validate Aligned Sequence ctor by adding aligned sequnece and 
        /// metadata using smithwatermanaligner
        /// Input : dna aligned sequence
        /// Output : dna aligned sequence instance
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAlignedSequenceCtor()
        {
            ValidateAlignedSequenceCtor(Constants.SmithWatermanAlignAlgorithmNodeName,
                SequenceAligners.SmithWaterman);
        }

        /// <summary>
        /// Validate Aligned Sequence by passing IAligned sequence of dna sequence 
        /// using smithwatermanaligner
        /// Input : dna sequence
        /// Output : dna aligned sequence instance
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAlignedSequenceWithSmithWatermanAligner()
        {
            ValidateAlignedSequence(Constants.SmithWatermanAlignAlgorithmNodeName,
                SequenceAligners.SmithWaterman);
        }

        /// <summary>
        /// Validate Aligned Sequence by passing IAligned sequence of dna sequence 
        /// using needlemanwunschaligner
        /// Input : dna sequence
        /// Output : dna aligned sequence instance
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAlignedSequenceWithNeedlemanWunschAligner()
        {
            ValidateAlignedSequence(Constants.NeedlemanWunschAlignAlgorithmNodeName,
                SequenceAligners.NeedlemanWunsch);
        }

        /// <summary>
        /// Validate Aligned Sequence by passing IAligned sequence of dna sequence using pairwisealigner
        /// Input : dna sequence
        /// Output : dna aligned sequence instance
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAlignedSequenceWithPairWiseOverlapAligner()
        {
            ValidateAlignedSequence(Constants.PairwiseOverlapAlignAlgorithmNodeName,
                SequenceAligners.PairwiseOverlap);
        }

        /// <summary>
        /// Validate Aligned Sequence serialization and deserialization
        /// by passing IAligned sequence of dna sequence using pairwisealigner
        /// Input : dna sequence
        /// Output : dna aligned sequence instance
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateAlignedSequenceSerializationAndDeserialization()
        {
            ValidateAlignedSequenceSerializationAndDeserialization(
                Constants.PairwiseOverlapAlignAlgorithmNodeName);
        }

        #endregion

        #region Sequence Alignment BVT Test Cases

        /// <summary>
        /// Validate Sequence Alignment ctor by passing ISequenceAlignment of dna sequence 
        /// using smithwatermanaligner
        /// Input : dna sequence
        /// Output : dna sequence alignment instance
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSequenceAlignmentCtorWithSmithWatermanAligner()
        {
            ValidateSequenceAlignmentCtor(Constants.SmithWatermanAlignAlgorithmNodeName,
                SequenceAligners.SmithWaterman);
        }

        /// <summary>
        /// Validate Sequence Alignment ctor by passing ISequenceAlignment of dna sequence 
        /// using needlemanwunschaligner
        /// Input : dna sequence
        /// Output : dna sequence alignment instance
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSequenceAlignmentCtorWithNeedlemanWunschAligner()
        {
            ValidateSequenceAlignmentCtor(Constants.NeedlemanWunschAlignAlgorithmNodeName,
                SequenceAligners.NeedlemanWunsch);
        }

        /// <summary>
        /// Validate Sequence Alignment ctor by passing ISequenceAlignment of dna sequence 
        /// using pairwiseoverlapaligner
        /// Input : dna sequence
        /// Output : dna sequence alignment instance
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSequenceAlignmentCtorWithPairwiseOverlapAligner()
        {
            ValidateSequenceAlignmentCtor(Constants.PairwiseOverlapAlignAlgorithmNodeName,
                SequenceAligners.PairwiseOverlap);
        }

        /// <summary>
        /// Validate Sequence Alignment by passing ISequenceAlignment of dna sequence 
        /// using smithwatermanaligner
        /// Input : dna sequence
        /// Output : dna sequence alignment instance
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSequenceAlignmentWithSmithWatermanAligner()
        {
            ValidateSequenceAlignment(Constants.SmithWatermanAlignAlgorithmNodeName,
                SequenceAligners.SmithWaterman);
        }

        /// <summary>
        /// Validate Sequence Alignment by passing ISequenceAlignment of dna sequence 
        /// using needlemanwunschaligner
        /// Input : dna sequence
        /// Output : dna sequence alignment instance
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSequenceAlignmentWithNeedlemanWunschAligner()
        {
            ValidateSequenceAlignment(Constants.NeedlemanWunschAlignAlgorithmNodeName,
                SequenceAligners.NeedlemanWunsch);
        }

        /// <summary>
        /// Validate Sequence Alignment by passing ISequenceAlignment of 
        /// dna sequence using pairwisealigner
        /// Input : dna sequence
        /// Output : dna sequence alignment instance
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSequenceAlignmentWithPairWiseOverlapAligner()
        {
            ValidateSequenceAlignment(Constants.PairwiseOverlapAlignAlgorithmNodeName,
                SequenceAligners.PairwiseOverlap);
        }

        /// <summary>
        /// Validate Sequence Alignment serialization and deserialization
        /// by passing ISequenceAlignment of dna sequence using pairwisealigner
        /// Input : dna sequence
        /// Output : dna sequence alignment instance
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void ValidateSequenceAlignmentSerializationAndDeserialization()
        {
            ValidateSequenceAlignmentSerializationAndDeserialization(
                Constants.PairwiseOverlapAlignAlgorithmNodeName);
        }

        #endregion

        #region Supporting Methods

        /// <summary>
        /// Validates NeedlemanWunschAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="isTextFile">Is text file an input.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        void ValidateNeedlemanWunschAlignment(bool isTextFile, AlignmentParamType alignParam,
            bool IsUseEARTHToFillMatrix)
        {
            ValidateNeedlemanWunschAlignment(isTextFile, alignParam,
                AlignmentType.SimpleAlign, IsUseEARTHToFillMatrix);
        }

        /// <summary>
        /// Validates NeedlemanWunschAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="isTextFile">Is text file an input.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="alignType">Is the Align type Simple or Align with Gap Extension cost?</param>
        void ValidateNeedlemanWunschAlignment(bool isTextFile, AlignmentParamType alignParam,
            AlignmentType alignType, bool IsUseEARTHToFillMatrix)
        {
            ISequence originalSequence1 = null;
            ISequence originalSequence2 = null;
            Sequence aInput = null;
            Sequence bInput = null;

            IAlphabet alphabet = Utility.GetAlphabet(
                _utilityObj._xmlUtil.GetTextValue(Constants.NeedlemanWunschAlignAlgorithmNodeName,
                Constants.AlphabetNameNode));

            if (isTextFile)
            {
                // Read the xml file for getting both the files for aligning.
                string filePath1 = _utilityObj._xmlUtil.GetTextValue(
                    Constants.NeedlemanWunschAlignAlgorithmNodeName,
                    Constants.FilePathNode1);
                string filePath2 = _utilityObj._xmlUtil.GetTextValue(
                    Constants.NeedlemanWunschAlignAlgorithmNodeName,
                    Constants.FilePathNode2);

                // Parse the files and get the sequence.
                IList<ISequence> seqs1 = null;
                IList<ISequence> seqs2 = null;
                using (FastaParser parser = new FastaParser())
                {
                    seqs1 = parser.Parse(filePath1);
                    seqs2 = parser.Parse(filePath2);

                    originalSequence1 = seqs1[0];
                    originalSequence2 = seqs2[0];
                    aInput = new Sequence(alphabet, originalSequence1.ToString());
                    bInput = new Sequence(alphabet, originalSequence2.ToString());
                }
            }
            else
            {
                // Read the xml file for getting both the files for aligning.
                string origSequence1 = _utilityObj._xmlUtil.GetTextValue(
                    Constants.NeedlemanWunschAlignAlgorithmNodeName,
                    Constants.SequenceNode1);
                string origSequence2 = _utilityObj._xmlUtil.GetTextValue(
                    Constants.NeedlemanWunschAlignAlgorithmNodeName,
                    Constants.SequenceNode2);

                aInput = new Sequence(alphabet, origSequence1);
                bInput = new Sequence(alphabet, origSequence2);
            }

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "NeedlemanWunschAligner BVT : First sequence used is '{0}'.",
                aInput.ToString()));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "NeedlemanWunschAligner BVT : Second sequence used is '{0}'.",
                bInput.ToString()));

            Console.WriteLine(string.Format((IFormatProvider)null,
                "NeedlemanWunschAligner BVT : First sequence used is '{0}'.",
                aInput.ToString()));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "NeedlemanWunschAligner BVT : Second sequence used is '{0}'.",
                bInput.ToString()));

            string blosumFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.NeedlemanWunschAlignAlgorithmNodeName,
                Constants.BlosumFilePathNode);

            SimilarityMatrix sm = new SimilarityMatrix(blosumFilePath);
            int gapOpenCost = int.Parse(_utilityObj._xmlUtil.GetTextValue(
                Constants.NeedlemanWunschAlignAlgorithmNodeName,
                Constants.GapOpenCostNode), (IFormatProvider)null);

            int gapExtensionCost = int.Parse(_utilityObj._xmlUtil.GetTextValue(
                Constants.NeedlemanWunschAlignAlgorithmNodeName,
                Constants.GapExtensionCostNode), (IFormatProvider)null);

            NeedlemanWunschAligner needlemanWunschObj = new NeedlemanWunschAligner();
            if (IsUseEARTHToFillMatrix)
            {
                needlemanWunschObj.UseEARTHToFillMatrix = true;
            }
            if (AlignmentParamType.AllParam != alignParam)
            {
                needlemanWunschObj.SimilarityMatrix = sm;
                needlemanWunschObj.GapOpenCost = gapOpenCost;
                needlemanWunschObj.GapExtensionCost = gapExtensionCost;
            }

            IList<IPairwiseSequenceAlignment> result = null;

            switch (alignParam)
            {
                case AlignmentParamType.AlignList:
                    List<ISequence> sequences = new List<ISequence>();
                    sequences.Add(aInput);
                    sequences.Add(bInput);
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            result = needlemanWunschObj.Align(sequences);
                            break;
                        default:
                            result = needlemanWunschObj.AlignSimple(sequences);
                            break;
                    }
                    break;
                case AlignmentParamType.AlignTwo:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            result = needlemanWunschObj.Align(aInput, bInput);
                            break;
                        default:
                            result = needlemanWunschObj.AlignSimple(aInput, bInput);
                            break;
                    }
                    break;
                case AlignmentParamType.AllParam:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            result = needlemanWunschObj.Align(
                                sm, gapOpenCost, gapExtensionCost, aInput, bInput);
                            break;
                        default:
                            result = needlemanWunschObj.AlignSimple(sm, gapOpenCost, aInput, bInput);
                            break;
                    }
                    break;
                default:
                    break;
            }

            // Read the xml file for getting both the files for aligning.
            string expectedSequence1 = string.Empty;
            string expectedSequence2 = string.Empty;

            string expectedScore = string.Empty;

            switch (alignType)
            {
                case AlignmentType.Align:
                    expectedScore = _utilityObj._xmlUtil.GetTextValue(
                        Constants.NeedlemanWunschAlignAlgorithmNodeName,
                        Constants.ExpectedGapExtensionScoreNode);
                    expectedSequence1 = _utilityObj._xmlUtil.GetTextValue(
                        Constants.NeedlemanWunschAlignAlgorithmNodeName,
                        Constants.ExpectedGapExtensionSequence1Node);
                    expectedSequence2 = _utilityObj._xmlUtil.GetTextValue(
                        Constants.NeedlemanWunschAlignAlgorithmNodeName,
                        Constants.ExpectedGapExtensionSequence2Node);
                    break;
                default:
                    expectedScore = _utilityObj._xmlUtil.GetTextValue(
                        Constants.NeedlemanWunschAlignAlgorithmNodeName,
                        Constants.ExpectedScoreNode);
                    expectedSequence1 = _utilityObj._xmlUtil.GetTextValue(
                        Constants.NeedlemanWunschAlignAlgorithmNodeName,
                        Constants.ExpectedSequenceNode1);
                    expectedSequence2 = _utilityObj._xmlUtil.GetTextValue(
                        Constants.NeedlemanWunschAlignAlgorithmNodeName,
                        Constants.ExpectedSequenceNode2);
                    break;
            }

            IList<IPairwiseSequenceAlignment> expectedOutput = new List<IPairwiseSequenceAlignment>();

            IPairwiseSequenceAlignment align = new PairwiseSequenceAlignment();
            PairwiseAlignedSequence alignedSeq = new PairwiseAlignedSequence();
            alignedSeq.FirstSequence = new Sequence(alphabet, expectedSequence1);
            alignedSeq.SecondSequence = new Sequence(alphabet, expectedSequence2);
            alignedSeq.Score = Convert.ToInt32(expectedScore, (IFormatProvider)null);
            align.PairwiseAlignedSequences.Add(alignedSeq);
            expectedOutput.Add(align);
            Assert.IsTrue(CompareAlignment(result, expectedOutput));

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "NeedlemanWunschAligner BVT : Final Score '{0}'.", expectedScore));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "NeedlemanWunschAligner BVT : Aligned First Sequence is '{0}'.",
               expectedSequence1));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "NeedlemanWunschAligner BVT : Aligned Second Sequence is '{0}'.",
                expectedSequence2));

            Console.WriteLine(string.Format((IFormatProvider)null,
                "NeedlemanWunschAligner BVT : Final Score '{0}'.", expectedScore));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "NeedlemanWunschAligner BVT : Aligned First Sequence is '{0}'.",
               expectedSequence1));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "NeedlemanWunschAligner BVT : Aligned Second Sequence is '{0}'.",
                expectedSequence2));
        }

        /// <summary>
        /// Validates SmithWatermanAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="isTextFile">Is text file an input.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        void ValidateSmithWatermanAlignment(bool isTextFile, AlignmentParamType alignParam,
            bool IsUseEARTHToFillMatrix)
        {
            ValidateSmithWatermanAlignment(isTextFile, alignParam, AlignmentType.SimpleAlign,
                IsUseEARTHToFillMatrix);
        }

        /// <summary>
        /// Validates SmithWatermanAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="isTextFile">Is text file an input.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="alignType">Is the Align type Simple or Align with Gap Extension cost?</param>
        void ValidateSmithWatermanAlignment(bool isTextFile, AlignmentParamType alignParam,
            AlignmentType alignType, bool IsUseEARTHToFillMatrix)
        {
            ISequence originalSequence1 = null;
            ISequence originalSequence2 = null;
            Sequence aInput = null;
            Sequence bInput = null;

            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.SmithWatermanAlignAlgorithmNodeName,
                Constants.AlphabetNameNode));

            if (isTextFile)
            {
                // Read the xml file for getting both the files for aligning.
                string filePath1 = _utilityObj._xmlUtil.GetTextValue(
                    Constants.SmithWatermanAlignAlgorithmNodeName,
                    Constants.FilePathNode1);
                string filePath2 = _utilityObj._xmlUtil.GetTextValue(
                    Constants.SmithWatermanAlignAlgorithmNodeName,
                    Constants.FilePathNode2);

                // Parse the files and get the sequence.
                IList<ISequence> seqs1 = null;
                IList<ISequence> seqs2 = null;
                using (FastaParser parser = new FastaParser())
                {
                    seqs1 = parser.Parse(filePath1);
                    seqs2 = parser.Parse(filePath2);

                    originalSequence1 = seqs1[0];
                    originalSequence2 = seqs2[0];
                    aInput = new Sequence(alphabet, originalSequence1.ToString());
                    bInput = new Sequence(alphabet, originalSequence2.ToString());
                }
            }
            else
            {
                // Read the xml file for getting both the files for aligning.
                string origSequence1 = _utilityObj._xmlUtil.GetTextValue(
                    Constants.SmithWatermanAlignAlgorithmNodeName,
                    Constants.SequenceNode1);
                string origSequence2 = _utilityObj._xmlUtil.GetTextValue(
                    Constants.SmithWatermanAlignAlgorithmNodeName,
                    Constants.SequenceNode2);
                aInput = new Sequence(alphabet, origSequence1);
                bInput = new Sequence(alphabet, origSequence2);
            }

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "SmithWatermanAligner BVT : First sequence used is '{0}'.",
                aInput.ToString()));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "SmithWatermanAligner BVT : Second sequence used is '{0}'.",
                bInput.ToString()));

            Console.WriteLine(string.Format((IFormatProvider)null,
                "SmithWatermanAligner BVT : First sequence used is '{0}'.",
                aInput.ToString()));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SmithWatermanAligner BVT : Second sequence used is '{0}'.",
                bInput.ToString()));

            string blosumFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmithWatermanAlignAlgorithmNodeName,
                Constants.BlosumFilePathNode);

            SimilarityMatrix sm = new SimilarityMatrix(blosumFilePath);
            int gapOpenCost = int.Parse(_utilityObj._xmlUtil.GetTextValue(
                Constants.SmithWatermanAlignAlgorithmNodeName,
                Constants.GapOpenCostNode), (IFormatProvider)null);

            int gapExtensionCost = int.Parse(_utilityObj._xmlUtil.GetTextValue(
                Constants.SmithWatermanAlignAlgorithmNodeName,
                Constants.GapExtensionCostNode), (IFormatProvider)null);

            SmithWatermanAligner smithWatermanObj = new SmithWatermanAligner();

            if (AlignmentParamType.AllParam != alignParam)
            {
                smithWatermanObj.SimilarityMatrix = sm;
                smithWatermanObj.GapOpenCost = gapOpenCost;
            }

            if (IsUseEARTHToFillMatrix)
            {
                smithWatermanObj.UseEARTHToFillMatrix = true;
            }
            IList<IPairwiseSequenceAlignment> result = null;

            switch (alignParam)
            {
                case AlignmentParamType.AlignList:
                    List<ISequence> sequences = new List<ISequence>();
                    sequences.Add(aInput);
                    sequences.Add(bInput);
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            result = smithWatermanObj.Align(sequences);
                            break;
                        default:
                            result = smithWatermanObj.AlignSimple(sequences);
                            break;
                    }
                    break;
                case AlignmentParamType.AlignTwo:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            result = smithWatermanObj.Align(aInput, bInput);
                            break;
                        default:
                            result = smithWatermanObj.AlignSimple(aInput, bInput);
                            break;
                    }
                    break;
                case AlignmentParamType.AllParam:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            result = smithWatermanObj.Align(sm, gapOpenCost,
                                gapExtensionCost, aInput, bInput);
                            break;
                        default:
                            result = smithWatermanObj.AlignSimple(sm, gapOpenCost, aInput, bInput);
                            break;
                    }
                    break;
                default:
                    break;
            }

            // Read the xml file for getting both the files for aligning.
            string expectedSequence1 = string.Empty;
            string expectedSequence2 = string.Empty;

            string expectedScore = string.Empty;

            switch (alignType)
            {
                case AlignmentType.Align:
                    expectedScore = _utilityObj._xmlUtil.GetTextValue(
                        Constants.SmithWatermanAlignAlgorithmNodeName,
                        Constants.ExpectedGapExtensionScoreNode);
                    expectedSequence1 = _utilityObj._xmlUtil.GetTextValue(
                        Constants.SmithWatermanAlignAlgorithmNodeName,
                        Constants.ExpectedGapExtensionSequence1Node);
                    expectedSequence2 = _utilityObj._xmlUtil.GetTextValue(
                        Constants.SmithWatermanAlignAlgorithmNodeName,
                        Constants.ExpectedGapExtensionSequence2Node);
                    break;
                default:
                    expectedScore = _utilityObj._xmlUtil.GetTextValue(
                        Constants.SmithWatermanAlignAlgorithmNodeName,
                        Constants.ExpectedScoreNode);
                    expectedSequence1 = _utilityObj._xmlUtil.GetTextValue(
                        Constants.SmithWatermanAlignAlgorithmNodeName,
                        Constants.ExpectedSequenceNode1);
                    expectedSequence2 = _utilityObj._xmlUtil.GetTextValue(
                        Constants.SmithWatermanAlignAlgorithmNodeName,
                        Constants.ExpectedSequenceNode2);
                    break;
            }

            IList<IPairwiseSequenceAlignment> expectedOutput = new List<IPairwiseSequenceAlignment>();

            IPairwiseSequenceAlignment align = new PairwiseSequenceAlignment();
            PairwiseAlignedSequence alignedSeq = new PairwiseAlignedSequence();
            alignedSeq.FirstSequence = new Sequence(alphabet, expectedSequence1);
            alignedSeq.SecondSequence = new Sequence(alphabet, expectedSequence2);
            alignedSeq.Score = Convert.ToInt32(expectedScore, (IFormatProvider)null);
            align.PairwiseAlignedSequences.Add(alignedSeq);
            expectedOutput.Add(align);
            Assert.IsTrue(CompareAlignment(result, expectedOutput));

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "SmithWatermanAligner BVT : Final Score '{0}'.", expectedScore));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "SmithWatermanAligner BVT : Aligned First Sequence is '{0}'.",
                expectedSequence1));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "SmithWatermanAligner BVT : Aligned Second Sequence is '{0}'.",
                expectedSequence2));

            Console.WriteLine(string.Format((IFormatProvider)null,
                "SmithWatermanAligner BVT : Final Score '{0}'.", expectedScore));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SmithWatermanAligner BVT : Aligned First Sequence is '{0}'.",
                expectedSequence1));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "SmithWatermanAligner BVT : Aligned Second Sequence is '{0}'.",
                expectedSequence2));
        }

        /// <summary>
        /// Validates PairwiseOverlapAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="isTextFile">Is text file an input.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        void ValidatePairwiseOverlapAlignment(bool isTextFile, AlignmentParamType alignParam,
            bool IsUseEARTHToFillMatrix)
        {
            ValidatePairwiseOverlapAlignment(isTextFile, alignParam, AlignmentType.SimpleAlign,
               IsUseEARTHToFillMatrix);
        }

        /// <summary>
        /// Validates PairwiseOverlapAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="isTextFile">Is text file an input.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="alignType">Is the Align type Simple or Align with Gap Extension cost?</param>
        void ValidatePairwiseOverlapAlignment(bool isTextFile, AlignmentParamType alignParam,
            AlignmentType alignType, bool IsUseEARTHToFillMatrix)
        {
            ISequence originalSequence1 = null;
            ISequence originalSequence2 = null;
            Sequence aInput = null;
            Sequence bInput = null;

            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                Constants.AlphabetNameNode));

            if (isTextFile)
            {
                // Read the xml file for getting both the files for aligning.
                string filePath1 = _utilityObj._xmlUtil.GetTextValue(
                    Constants.PairwiseOverlapAlignAlgorithmNodeName,
                    Constants.FilePathNode1);
                string filePath2 = _utilityObj._xmlUtil.GetTextValue(
                    Constants.PairwiseOverlapAlignAlgorithmNodeName,
                    Constants.FilePathNode2);

                // Parse the files and get the sequence.
                IList<ISequence> seqs1 = null;
                IList<ISequence> seqs2 = null;
                using (FastaParser parser = new FastaParser())
                {
                    seqs1 = parser.Parse(filePath1);
                    seqs2 = parser.Parse(filePath2);

                    originalSequence1 = seqs1[0];
                    originalSequence2 = seqs2[0];
                    aInput = new Sequence(alphabet, originalSequence1.ToString());
                    bInput = new Sequence(alphabet, originalSequence2.ToString());
                }
            }
            else
            {
                // Read the xml file for getting both the files for aligning.
                string origSequence1 = _utilityObj._xmlUtil.GetTextValue(
                    Constants.PairwiseOverlapAlignAlgorithmNodeName,
                    Constants.SequenceNode1);
                string origSequence2 = _utilityObj._xmlUtil.GetTextValue(
                    Constants.PairwiseOverlapAlignAlgorithmNodeName,
                    Constants.SequenceNode2);
                aInput = new Sequence(alphabet, origSequence1);
                bInput = new Sequence(alphabet, origSequence2);
            }

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "PairwiseOverlapAligner BVT : First sequence used is '{0}'.",
                aInput.ToString()));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "PairwiseOverlapAligner BVT : Second sequence used is '{0}'.",
                bInput.ToString()));

            Console.WriteLine(string.Format((IFormatProvider)null,
                "PairwiseOverlapAligner BVT : First sequence used is '{0}'.",
                aInput.ToString()));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "PairwiseOverlapAligner BVT : Second sequence used is '{0}'.",
                bInput.ToString()));

            string blosumFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                Constants.BlosumFilePathNode);

            SimilarityMatrix sm = new SimilarityMatrix(blosumFilePath);
            int gapOpenCost = int.Parse(_utilityObj._xmlUtil.GetTextValue(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                Constants.GapOpenCostNode), (IFormatProvider)null);

            int gapExtensionCost = int.Parse(_utilityObj._xmlUtil.GetTextValue(
                Constants.PairwiseOverlapAlignAlgorithmNodeName,
                Constants.GapExtensionCostNode), (IFormatProvider)null);

            PairwiseOverlapAligner pairwiseOverlapObj = new PairwiseOverlapAligner();
            if (AlignmentParamType.AllParam != alignParam)
            {
                pairwiseOverlapObj.SimilarityMatrix = sm;
                pairwiseOverlapObj.GapOpenCost = gapOpenCost;
            }
            if (IsUseEARTHToFillMatrix)
            {
                pairwiseOverlapObj.UseEARTHToFillMatrix = true;
            }
            IList<IPairwiseSequenceAlignment> result = null;

            switch (alignParam)
            {
                case AlignmentParamType.AlignList:
                    List<ISequence> sequences = new List<ISequence>();
                    sequences.Add(aInput);
                    sequences.Add(bInput);
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            result = pairwiseOverlapObj.Align(sequences);
                            break;
                        default:
                            result = pairwiseOverlapObj.AlignSimple(sequences);
                            break;
                    }
                    break;
                case AlignmentParamType.AlignTwo:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            result = pairwiseOverlapObj.Align(aInput, bInput);
                            break;
                        default:
                            result = pairwiseOverlapObj.AlignSimple(aInput, bInput);
                            break;
                    }
                    break;
                case AlignmentParamType.AllParam:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            result = pairwiseOverlapObj.Align(sm, gapOpenCost,
                                gapExtensionCost, aInput, bInput);
                            break;
                        default:
                            result = pairwiseOverlapObj.AlignSimple(sm, gapOpenCost, aInput, bInput);
                            break;
                    }
                    break;
                default:
                    break;
            }

            // Read the xml file for getting both the files for aligning.
            string expectedSequence1 = string.Empty;
            string expectedSequence2 = string.Empty;

            string expectedScore = string.Empty;

            switch (alignType)
            {
                case AlignmentType.Align:
                    expectedScore = _utilityObj._xmlUtil.GetTextValue(
                        Constants.PairwiseOverlapAlignAlgorithmNodeName,
                        Constants.ExpectedGapExtensionScoreNode);
                    expectedSequence1 = _utilityObj._xmlUtil.GetTextValue(
                        Constants.PairwiseOverlapAlignAlgorithmNodeName,
                        Constants.ExpectedGapExtensionSequence1Node);
                    expectedSequence2 = _utilityObj._xmlUtil.GetTextValue(
                        Constants.PairwiseOverlapAlignAlgorithmNodeName,
                        Constants.ExpectedGapExtensionSequence2Node);
                    break;
                default:
                    expectedScore = _utilityObj._xmlUtil.GetTextValue(
                        Constants.PairwiseOverlapAlignAlgorithmNodeName,
                        Constants.ExpectedScoreNode);
                    expectedSequence1 = _utilityObj._xmlUtil.GetTextValue(
                        Constants.PairwiseOverlapAlignAlgorithmNodeName,
                        Constants.ExpectedSequenceNode1);
                    expectedSequence2 = _utilityObj._xmlUtil.GetTextValue(
                        Constants.PairwiseOverlapAlignAlgorithmNodeName,
                        Constants.ExpectedSequenceNode2);
                    break;
            }

            IList<IPairwiseSequenceAlignment> expectedOutput = new List<IPairwiseSequenceAlignment>();
            string[] expectedSequences1, expectedSequences2;
            char[] seperators = new char[1] { ';' };
            expectedSequences1 = expectedSequence1.Split(seperators);
            expectedSequences2 = expectedSequence2.Split(seperators);

            IPairwiseSequenceAlignment align = new PairwiseSequenceAlignment();
            PairwiseAlignedSequence alignedSeq;
            for (int i = 0; i < expectedSequences1.Length; i++)
            {
                alignedSeq = new PairwiseAlignedSequence();
                alignedSeq.FirstSequence = new Sequence(alphabet, expectedSequences1[i]);
                alignedSeq.SecondSequence = new Sequence(alphabet, expectedSequences2[i]);
                alignedSeq.Score = Convert.ToInt32(expectedScore, (IFormatProvider)null);
                align.PairwiseAlignedSequences.Add(alignedSeq);
            }

            expectedOutput.Add(align);
            Assert.IsTrue(CompareAlignment(result, expectedOutput));

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "PairwiseOverlapAligner BVT : Final Score '{0}'.", expectedScore));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "PairwiseOverlapAligner BVT : Aligned First Sequence is '{0}'.",
                expectedSequence1));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "PairwiseOverlapAligner BVT : Aligned Second Sequence is '{0}'.",
                expectedSequence2));

            Console.WriteLine(string.Format((IFormatProvider)null,
                "PairwiseOverlapAligner BVT : Final Score '{0}'.", expectedScore));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "PairwiseOverlapAligner BVT : Aligned First Sequence is '{0}'.",
                expectedSequence1));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "PairwiseOverlapAligner BVT : Aligned Second Sequence is '{0}'.",
                expectedSequence2));
        }

        /// <summary>
        /// Compare the alignment of mummer and defined alignment
        /// </summary>
        /// <param name="result">output of Aligners</param>
        /// <param name="expectedAlignment">expected output</param>
        /// <returns>Compare result of alignments</returns>
        private static bool CompareAlignment(IList<IPairwiseSequenceAlignment> actualAlignment,
             IList<IPairwiseSequenceAlignment> expectedAlignment)
        {
            bool output = true;

            if (actualAlignment.Count == expectedAlignment.Count)
            {
                for (int resultCount = 0; resultCount < actualAlignment.Count; resultCount++)
                {
                    if (actualAlignment[resultCount].PairwiseAlignedSequences.Count == expectedAlignment[resultCount].PairwiseAlignedSequences.Count)
                    {
                        for (int alignSeqCount = 0; alignSeqCount < actualAlignment[resultCount].PairwiseAlignedSequences.Count; alignSeqCount++)
                        {
                            // Validates the First Sequence, Second Sequence and Score
                            if (
                                actualAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].FirstSequence.ToString().Equals(
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
        /// Validate aligned sequence instance using different aligners
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="aligner">sw/nw/pw aligners</param>
        private void ValidateAlignedSequence(string nodeName, ISequenceAligner aligner)
        {
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));
            string origSequence1 = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode1);
            string origSequence2 = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode2);

            // Create input sequences
            List<ISequence> inputSequences = new List<ISequence>();
            inputSequences.Add(new Sequence(alphabet, origSequence1));
            inputSequences.Add(new Sequence(alphabet, origSequence2));

            // Get aligned sequences
            IList<ISequenceAlignment> alignment = aligner.Align(inputSequences);

            // Create AlignedSequence instance
            IAlignedSequence sequence = alignment[0].AlignedSequences[0];

            // Validate the alignedsequence properties
            Assert.AreEqual(alignment[0].AlignedSequences[0].Sequences, sequence.Sequences);
            Assert.AreEqual(alignment[0].AlignedSequences[0].Metadata, sequence.Metadata);

            Console.WriteLine(@"Alignment BVT : Validation of 
                               aligned sequence completed successfully");
            ApplicationLog.WriteLine(@"Alignment BVT : Validation of 
                                     aligned sequence completed successfully");
        }

        /// <summary>
        /// Validate aligned sequence serialization and deserialization using pairwise aligner
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        private void ValidateAlignedSequenceSerializationAndDeserialization(string nodeName)
        {
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));
            string origSequence1 = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode1);
            string origSequence2 = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode2);

            // Create input sequences
            List<ISequence> inputSequences = new List<ISequence>();
            inputSequences.Add(new Sequence(alphabet, origSequence1));
            inputSequences.Add(new Sequence(alphabet, origSequence2));

            // Get aligned sequences
            ISequenceAligner aligner = SequenceAligners.PairwiseOverlap;
            IList<ISequenceAlignment> alignment = aligner.Align(inputSequences);

            // Create AlignedSequence instance
            IAlignedSequence sequence = alignment[0].AlignedSequences[0];

            using (Stream stream = File.Open("AlignedSequence.data", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, sequence);

                stream.Seek(0, SeekOrigin.Begin);
                IAlignedSequence deserializedSeq = (IAlignedSequence)formatter.Deserialize(stream);

                // Validate the alignedsequence properties
                Assert.AreEqual(alignment[0].AlignedSequences[0].Sequences[0].ToString(),
                    deserializedSeq.Sequences[0].ToString());
                Assert.AreEqual(alignment[0].AlignedSequences[0].Metadata.First().Value.ToString(),
                    deserializedSeq.Metadata.First().Value.ToString());
            }

            Console.WriteLine(@"Alignment BVT : Validation of aligned sequence serialization and 
                              deserialization completed successfully");
            ApplicationLog.WriteLine(@"Alignment BVT : Validation of aligned sequence serialization and 
                              deserialization completed successfully");
        }

        /// <summary>
        /// Validate sequence alignment instance using different aligners
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="aligner">sw/nw/pw aligners</param>
        private void ValidateSequenceAlignment(string nodeName, ISequenceAligner aligner)
        {
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));
            string origSequence1 = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode1);
            string origSequence2 = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode2);

            // Create input sequences
            List<ISequence> inputSequences = new List<ISequence>();
            inputSequences.Add(new Sequence(alphabet, origSequence1));
            inputSequences.Add(new Sequence(alphabet, origSequence2));

            // Get aligned sequences
            IList<ISequenceAlignment> alignments = aligner.Align(inputSequences);
            ISequenceAlignment alignment = alignments[0];

            Assert.AreEqual(alignments[0].AlignedSequences.Count, alignment.AlignedSequences.Count);
            Assert.AreEqual(alignments[0].Metadata, alignment.Metadata);
            Assert.AreEqual(inputSequences[0].ToString(), alignment.Sequences[0].ToString());

            Console.WriteLine(@"Alignment BVT : Validation of 
                               sequence alignment completed successfully");
            ApplicationLog.WriteLine(@"Alignment BVT : Validation of 
                                     sequence alignment completed successfully");
        }

        /// <summary>
        /// Validate sequence alignment serialization and deserialization using pairwise aligner
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        private void ValidateSequenceAlignmentSerializationAndDeserialization(string nodeName)
        {
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));
            string origSequence1 = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode1);
            string origSequence2 = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode2);

            // Create input sequences
            List<ISequence> inputSequences = new List<ISequence>();
            inputSequences.Add(new Sequence(alphabet, origSequence1));
            inputSequences.Add(new Sequence(alphabet, origSequence2));

            // Get aligned sequences
            ISequenceAligner aligner = SequenceAligners.PairwiseOverlap;
            IList<ISequenceAlignment> alignments = aligner.Align(inputSequences);
            ISequenceAlignment alignment = alignments[0];

            // Serialize and deserialize
            using (Stream stream = File.Open("SequenceAlignment.data", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, alignment);

                stream.Seek(0, SeekOrigin.Begin);
                PairwiseSequenceAlignment deserializedAlignment = (PairwiseSequenceAlignment)formatter.
                    Deserialize(stream);

                // Validate the deserialized sequence alignment
                for (int index = 0; index < alignments[0].AlignedSequences[0].Sequences.Count; index++)
                {
                    Assert.AreEqual(alignments[0].AlignedSequences[0].Sequences[index].ToString(),
                        deserializedAlignment.AlignedSequences[0].Sequences[index].ToString());
                }

                foreach (string key in alignments[0].AlignedSequences[0].Metadata.Keys)
                {
                    Assert.AreEqual(alignments[0].AlignedSequences[0].Metadata[key].ToString(),
                        deserializedAlignment.AlignedSequences[0].Metadata[key].ToString());
                }
            }

            Console.WriteLine(@"Alignment BVT : Validation of serialization and 
                              deserialization completed successfully");
            ApplicationLog.WriteLine(@"Alignment BVT : Validation of serialization and 
                              deserialization completed successfully");
        }

        /// <summary>
        /// Validate aligned sequence instance using different aligners
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="aligner">sw/nw/pw aligners</param>
        private void ValidateAlignedSequenceCtor(string nodeName, ISequenceAligner aligner)
        {
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));
            string origSequence1 = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode1);
            string origSequence2 = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode2);

            // Create input sequences
            List<ISequence> inputSequences = new List<ISequence>();
            inputSequences.Add(new Sequence(alphabet, origSequence1));
            inputSequences.Add(new Sequence(alphabet, origSequence2));

            // Get aligned sequences
            IAlignedSequence alignedSequence = new AlignedSequence();
            IList<ISequenceAlignment> alignment = aligner.Align(inputSequences);

            // add aligned sequence and metadata information
            for (int iseq = 0; iseq < alignment[0].AlignedSequences[0].Sequences.Count; iseq++)
            {
                alignedSequence.Sequences.Add(alignment[0].AlignedSequences[0].Sequences[iseq]);
            }

            foreach (string key in alignment[0].AlignedSequences[0].Metadata.Keys)
            {
                alignedSequence.Metadata.Add(key, alignment[0].AlignedSequences[0].Metadata[key]);
            }

            // Validate the alignedsequence properties
            for (int index = 0; index < alignment[0].AlignedSequences[0].Sequences.Count; index++)
            {
                Assert.AreEqual(alignment[0].AlignedSequences[0].Sequences[index].ToString(),
                    alignedSequence.Sequences[index].ToString());
            }

            foreach (string key in alignment[0].AlignedSequences[0].Metadata.Keys)
            {
                Assert.AreEqual(alignment[0].AlignedSequences[0].Metadata[key],
                    alignedSequence.Metadata[key]);
            }

            Console.WriteLine(@"Alignment BVT : Validation of 
                               aligned sequence ctor completed successfully");
            ApplicationLog.WriteLine(@"Alignment BVT : Validation of 
                                     aligned sequence ctor completed successfully");
        }

        /// <summary>
        /// Validate sequence alignment instance using different aligners
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="aligner">sw/nw/pw aligners</param>
        private void ValidateSequenceAlignmentCtor(string nodeName, ISequenceAligner aligner)
        {
            IAlphabet alphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));
            string origSequence1 = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode1);
            string origSequence2 = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode2);

            // Create input sequences
            List<ISequence> inputSequences = new List<ISequence>();
            inputSequences.Add(new Sequence(alphabet, origSequence1));
            inputSequences.Add(new Sequence(alphabet, origSequence2));

            // Get aligned sequences
            IList<ISequenceAlignment> alignments = aligner.Align(inputSequences);
            ISequenceAlignment alignment = new SequenceAlignment();
            for (int ialigned = 0; ialigned < alignments[0].AlignedSequences.Count; ialigned++)
            {
                alignment.AlignedSequences.Add(alignments[0].AlignedSequences[ialigned]);
            }

            foreach (string key in alignments[0].Metadata.Keys)
            {
                alignment.Metadata.Add(key, alignments[0].Metadata[key]);
            }

            // Validate the properties
            for (int ialigned = 0; ialigned < alignments[0].AlignedSequences.Count; ialigned++)
            {
                Assert.AreEqual(alignments[0].AlignedSequences[ialigned].Sequences[0].ToString(),
                    alignment.AlignedSequences[ialigned].Sequences[0].ToString());
            }

            foreach (string key in alignments[0].Metadata.Keys)
            {
                Assert.AreEqual(alignments[0].Metadata[key], alignment.Metadata[key]);
            }

            Console.WriteLine(@"Alignment BVT : Validation of 
                               sequence alignment ctor completed successfully");
            ApplicationLog.WriteLine(@"Alignment BVT : Validation of 
                                     sequence alignment  ctor completed successfully");
        }

        #endregion Supporting Methods
    }
}
