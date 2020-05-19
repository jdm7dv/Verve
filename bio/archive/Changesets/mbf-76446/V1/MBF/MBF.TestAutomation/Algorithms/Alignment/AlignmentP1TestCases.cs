// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * AlignmentP1TestCases.cs
 * 
 *   This file contains the Alignment P1 test cases i.e., NeedlemanWunschAligner, 
 *   SmithWatermanAlignmer, PairwiseOverlapAligner and SequenceAlignment
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;

using MBF.Algorithms.Alignment;
using MBF.IO.Fasta;
using MBF.SimilarityMatrices;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using NUnit.Framework;

namespace MBF.TestAutomation.Algorithms.Alignment
{
    /// <summary>
    /// Alignment P1 Test case implementation.
    /// </summary>
    [TestFixture]
    public class AlignmentP1TestCases
    {

        #region Enums

        /// <summary>
        /// Alignment Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AlignParameters
        {
            AlignList,
            AlignListCode,
            AllParam,
            AllParamCode,
            AlignTwo,
            AlignTwoCode
        };

        /// <summary>
        /// Similarity Matrix Parameters which are used for different test cases 
        /// based on which the test cases are executed with different Similarity Matrixes.
        /// </summary>
        enum SimilarityMatrixParameters
        {
            TextReader,
            DiagonalMatrix,
            Default
        };

        /// <summary>
        /// Alignment Type Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AlignmentType
        {
            SimpleAlign,
            Align,
        };

        /// <summary>
        /// SequenceAlignment methods name which are used for different cases.
        /// </summary>
        enum SeqAlignmentMethods
        {
            Add,
            Clear,
            Contains,
            CopyTo,
            Remove,
            AddSequence,
            GetEnumerator,
            GetObjectData,
            Default
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static AlignmentP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region NeedlemanWunschAligner P1 Test cases

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignListSequencesDna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamDna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignListSequencesPro()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamPro()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Rna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignListSequencesRna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Rna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamRna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignListSequencesGapCostMax()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamGapCostMax()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignListSequencesGapCostMin()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamGapCostMin()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignListSequencesBlosum()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschBlosumAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamBlosum()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschBlosumAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignListSequencesPam()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschPamAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamPam()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschPamAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignListSequencesSimMatTextRead()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
                AlignParameters.AlignList,
                SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text Reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamSimMatTextRead()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
                AlignParameters.AllParam,
                SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignListSequencesDiagonalSimMat()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AlignList,
                SimilarityMatrixParameters.DiagonalMatrix);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamDiagonalSimMat()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AllParam,
                SimilarityMatrixParameters.DiagonalMatrix);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoDnaSequences()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoDnaSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignListDnaSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamDnaFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA RNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoRnaSequences()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA RNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoRnaSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA RNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignListRnaSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA RNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamRnaFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoProSequences()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoProSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignListProSequencesFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamProFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoSequencesGapCostMax()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoSequencesGapCostMaxFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignSequenceListGapCostMaxFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamGapCostMaxFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoSequencesGapCostMin()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoSequencesGapCostMinFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignSequenceListGapCostMinFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignAllParamGapCostMinFromXml()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(Two Sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoSequencesBlosum()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschBlosumAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two Sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoSequencesPam()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschPamAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoSequencesSimMatTextRead()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
                AlignParameters.AlignTwo,
                SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two Sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschSimpleAlignTwoSequencesDiagonalSimMat()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AlignTwo,
                SimilarityMatrixParameters.DiagonalMatrix);
        }

        #region Gap Extension Cost inclusion Test cases

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignListSequencesDna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignAllParamDna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDnaAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignListSequencesPro()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignAllParamPro()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Rna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignListSequencesRna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Rna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignAllParamRna()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschRnaAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignListSequencesGapCostMax()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignAllParamGapCostMax()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignListSequencesGapCostMin()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignAllParamGapCostMin()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignListSequencesBlosum()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschBlosumAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignAllParamBlosum()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschBlosumAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignListSequencesPam()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschPamAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignAllParamPam()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschPamAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignListSequencesSimMatTextRead()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName, AlignParameters.AlignList,
                SimilarityMatrixParameters.TextReader, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text Reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignAllParamSimMatTextRead()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschProAlignAlgorithmNodeName, AlignParameters.AllParam,
                SimilarityMatrixParameters.TextReader, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignListSequencesDiagonalSimMat()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDiagonalSimMatAlignAlgorithmNodeName, AlignParameters.AlignList,
                SimilarityMatrixParameters.DiagonalMatrix, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignAllParamDiagonalSimMat()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschDiagonalSimMatAlignAlgorithmNodeName, AlignParameters.AllParam,
                SimilarityMatrixParameters.DiagonalMatrix, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost = Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignListSequencesGapCostGapExtensionEqual()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschEqualAlignAlgorithmNodeName, AlignParameters.AlignList,
                SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost = Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void NeedlemanWunschAlignAllParamGapCostGapExtensionEqual()
        {
            ValidateNeedlemanWunschAlignment(Constants.NeedlemanWunschEqualAlignAlgorithmNodeName, AlignParameters.AllParam,
                SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        #endregion Gap Extension Cost inclusion Test cases

        #endregion NeedlemanWunschAligner P1 Test cases

        #region SmithWatermanAligner P1 Test cases

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignListSequencesDna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamDna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignListSequencesPro()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamPro()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Rna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignListSequencesRna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Rna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamRna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignListSequencesGapCostMax()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamGapCostMax()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignListSequencesGapCostMin()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamGapCostMin()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignListSequencesBlosum()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanBlosumAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamBlosum()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanBlosumAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignListSequencesPam()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanPamAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamPam()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanPamAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignListSequencesSimMatTextRead()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AlignList,
                SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text Reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamSimMatTextRead()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AllParam,
                SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignListSequencesDiagonalSimMat()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AlignList,
                SimilarityMatrixParameters.DiagonalMatrix);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamDiagonalSimMat()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AllParam,
                SimilarityMatrixParameters.DiagonalMatrix);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoDnaSequences()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoDnaSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignListDnaSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamDnaFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA RNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoRnaSequences()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA RNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoRnaSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA RNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignListRnaSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA RNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamRnaFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoProSequences()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoProSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignListProSequencesFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamProFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoSequencesGapCostMax()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoSequencesGapCostMaxFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignSequenceListGapCostMaxFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamGapCostMaxFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoSequencesGapCostMin()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoSequencesGapCostMinFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignSequenceListGapCostMinFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignAllParamGapCostMinFromXml()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(Two Sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoSequencesBlosum()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanBlosumAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two Sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoSequencesPam()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanPamAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoSequencesSimMatTextRead()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AlignTwo,
                SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two Sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanSimpleAlignTwoSequencesDiagonalSimMat()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AlignTwo,
                SimilarityMatrixParameters.DiagonalMatrix);
        }

        #region Gap Extension Cost inclusion Test cases

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignListSequencesDna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignAllParamDna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDnaAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignListSequencesPro()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignAllParamPro()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Rna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignListSequencesRna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Rna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignAllParamRna()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanRnaAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignListSequencesGapCostMax()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignAllParamGapCostMax()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignListSequencesGapCostMin()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignAllParamGapCostMin()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignListSequencesBlosum()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanBlosumAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignAllParamBlosum()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanBlosumAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignListSequencesPam()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanPamAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignAllParamPam()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanPamAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignListSequencesSimMatTextRead()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AlignList,
                SimilarityMatrixParameters.TextReader, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text Reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignAllParamSimMatTextRead()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanProAlignAlgorithmNodeName,
                AlignParameters.AllParam,
                SimilarityMatrixParameters.TextReader, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignListSequencesDiagonalSimMat()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AlignList,
                SimilarityMatrixParameters.DiagonalMatrix, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignAllParamDiagonalSimMat()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AllParam,
                SimilarityMatrixParameters.DiagonalMatrix, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost = Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignListSequencesGapCostGapExtensionEqual()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanEqualAlignAlgorithmNodeName,
                AlignParameters.AlignList,
                SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost = Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void SmithWatermanAlignAllParamGapCostGapExtensionEqual()
        {
            ValidateSmithWatermanAlignment(Constants.SmithWatermanEqualAlignAlgorithmNodeName,
                AlignParameters.AllParam,
                SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        #endregion Gap Extension Cost inclusion Test cases

        #endregion SmithWatermanAligner P1 Test cases

        #region PairwiseOverlapAligner P1 Test cases

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignListSequencesDna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamDna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignListSequencesPro()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamPro()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Rna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignListSequencesRna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Rna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamRna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignListSequencesGapCostMax()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamGapCostMax()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignListSequencesGapCostMin()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamGapCostMin()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignListSequencesBlosum()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapBlosumAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamBlosum()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapBlosumAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignListSequencesPam()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapPamAlignAlgorithmNodeName,
                AlignParameters.AlignList);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamPam()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapPamAlignAlgorithmNodeName,
                AlignParameters.AllParam);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignListSequencesSimMatTextRead()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AlignList,
                SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text Reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamSimMatTextRead()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AllParam,
                SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 6
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignListSequencesDiagonalSimMat()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AlignList,
                SimilarityMatrixParameters.DiagonalMatrix);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamDiagonalSimMat()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AllParam,
                SimilarityMatrixParameters.DiagonalMatrix);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoDnaSequences()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoDnaSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignListDnaSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamDnaFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA RNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoRnaSequences()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA RNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoRnaSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA RNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignListRnaSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA RNA sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamRnaFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoProSequences()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoProSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignListProSequencesFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein sequence
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamProFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoSequencesGapCostMax()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoSequencesGapCostMaxFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(list of sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignSequenceListGapCostMaxFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignListCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamGapCostMaxFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoSequencesGapCostMin()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoSequencesGapCostMinFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignTwoCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a xml file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein Sequence with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignAllParamGapCostMinFromXml()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AllParamCode);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(Two Sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoSequencesBlosum()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapBlosumAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two Sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoSequencesPam()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapPamAlignAlgorithmNodeName,
                AlignParameters.AlignTwo);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoSequencesSimMatTextRead()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AlignTwo,
                SimilarityMatrixParameters.TextReader);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid GapPenalty, Similarity Matrix 
        /// which is in a text file using the method Align(two Sequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapSimpleAlignTwoSequencesDiagonalSimMat()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AlignTwo,
                SimilarityMatrixParameters.DiagonalMatrix);
        }

        #region Gap Extension Cost inclusion Test cases

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignListSequencesDna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignAllParamDna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDnaAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignListSequencesPro()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignAllParamPro()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Rna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignListSequencesRna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Rna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignAllParamRna()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapRnaAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignListSequencesGapCostMax()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Max Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignAllParamGapCostMax()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMaxAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignListSequencesGapCostMin()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Min Gap Cost
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignAllParamGapCostMin()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapGapCostMinAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignListSequencesBlosum()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapBlosumAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with blosum SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignAllParamBlosum()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapBlosumAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignListSequencesPam()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapPamAlignAlgorithmNodeName,
                AlignParameters.AlignList, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Pam SM
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignAllParamPam()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapPamAlignAlgorithmNodeName,
                AlignParameters.AllParam, SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignListSequencesSimMatTextRead()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AlignList,
                SimilarityMatrixParameters.TextReader, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Protein File with Similarity Matrix passed as Text Reader
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignAllParamSimMatTextRead()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapProAlignAlgorithmNodeName,
                AlignParameters.AllParam,
                SimilarityMatrixParameters.TextReader, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignListSequencesDiagonalSimMat()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AlignList,
                SimilarityMatrixParameters.DiagonalMatrix, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost, Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File Diagonal Matrix
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignAllParamDiagonalSimMat()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapDiagonalSimMatAlignAlgorithmNodeName,
                AlignParameters.AllParam,
                SimilarityMatrixParameters.DiagonalMatrix, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost = Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(ListofSequences) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignListSequencesGapCostGapExtensionEqual()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapEqualAlignAlgorithmNodeName,
                AlignParameters.AlignList,
                SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        /// <summary>
        /// Pass a Valid Sequence with valid Gap Cost = Gap Extension, Similarity Matrix 
        /// which is in a text file using the method Align(all parameters) 
        /// and validate if the aligned sequence is as expected and 
        /// also validate the score for the same
        /// Input : FastA Dna File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void PairwiseOverlapAlignAllParamGapCostGapExtensionEqual()
        {
            ValidatePairwiseOverlapAlignment(Constants.PairwiseOverlapEqualAlignAlgorithmNodeName,
                AlignParameters.AllParam,
                SimilarityMatrixParameters.Default, AlignmentType.Align);
        }

        #endregion Gap Extension Cost inclusion Test cases

        #endregion PairwiseOverlapAligner P1 Test cases

        #region Sequence Alignment P1 Test cases

        /// <summary>
        /// Pass a valid Dna sequences to AddSequence() method and validate the same.
        /// Input : Dna Sequence read from xml file.
        /// Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        public void SequenceAlignmentAddDnaSequence()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignDnaAlgorithmNodeName, false);
        }

        /// <summary>
        /// Pass a valid Rna sequences to AddSequence() method and validate the same.
        /// Input : Rna Sequence read from xml file.
        /// Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        public void SequenceAlignmentAddRnaSequence()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignRnaAlgorithmNodeName, false);
        }

        /// <summary>
        /// Pass a valid Protein sequences to AddSequence() method and validate the same.
        /// Input : Dna Sequence read from xml file.
        /// Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        public void SequenceAlignmentAddProteinSequence()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignProteinAlgorithmNodeName, false);
        }

        /// <summary>
        /// Pass a valid sequences to AddSequence() method and validate the properties.
        /// Input : Dna Sequence read from xml file.
        /// Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        public void SequenceAlignmentValidateProperties()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignAlgorithmNodeName, true);
        }

        /// <summary>
        /// Validate all SequenceAlignment public properties
        /// Input : FastA DNA File
        /// Validation : Aligned sequence and score.
        /// </summary>
        [Test]
        public void ValidateSequenceAlignmentProperties()
        {
            // Read the xml file for getting both the files for aligning.
            string origSequence1 = Utility._xmlUtil.GetTextValue(Constants.AlignDnaAlgorithmNodeName,
                Constants.SequenceNode1);
            string origSequence2 = Utility._xmlUtil.GetTextValue(Constants.AlignDnaAlgorithmNodeName,
                Constants.SequenceNode2);
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(
                Constants.AlignDnaAlgorithmNodeName,
                Constants.AlphabetNameNode));
            string seqCount = Utility._xmlUtil.GetTextValue(
                Constants.AlignDnaAlgorithmNodeName,
                Constants.SequenceCountNode);

            // Create two sequences
            ISequence aInput = new Sequence(alphabet, origSequence1);
            ISequence bInput = new Sequence(alphabet, origSequence2);

            // Add the sequences to the Sequence alignment object using AddSequence() method.
            IList<IPairwiseSequenceAlignment> sequenceAlignmentObj = new List<IPairwiseSequenceAlignment>();

            PairwiseAlignedSequence alignSeq = new PairwiseAlignedSequence();
            alignSeq.FirstSequence = aInput;
            alignSeq.SecondSequence = bInput;
            IPairwiseSequenceAlignment seqAlignObj = new PairwiseSequenceAlignment(aInput, bInput);
            seqAlignObj.Add(alignSeq);
            sequenceAlignmentObj.Add(seqAlignObj);

            // Validate all properties of sequence alignment class. 
            Assert.AreEqual(seqCount, seqAlignObj.Count.ToString());
            Assert.AreEqual(origSequence1, seqAlignObj.FirstSequence.ToString());
            Assert.AreEqual(origSequence2, seqAlignObj.SecondSequence.ToString());
            Assert.IsFalse(seqAlignObj.IsReadOnly);
            Assert.IsNull(seqAlignObj.Documentation);
            Assert.AreEqual(seqCount, seqAlignObj.PairwiseAlignedSequences.Count.ToString());

            ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the IsRead Property");
            ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the Count Property");
            ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the Sequences Property");

            Console.WriteLine("SequenceAlignment P1 : Successfully validated the IsRead Property");
            Console.WriteLine("SequenceAlignment P1 : Successfully validated the Count Property");
            Console.WriteLine("SequenceAlignment P1 : Successfully validated the Sequences Property");
        }

        /// <summary>
        /// Validate SequenceAlignment Add() method.
        /// Input : Dna Sequence read from xml file.
        /// Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        public void ValidateAddSequenceToSequenceAlignment()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.Add, false);
        }

        /// <summary>
        /// Validate SequenceAlignment Clear() method.
        /// Input : Dna Sequence read from xml file.
        /// Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        public void ValidateDeletingSequenceAlignment()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.Clear, false);
        }

        /// <summary>
        /// Validate SequenceAlignment Contains() method.
        /// Input : Dna Sequence read from xml file.
        /// Validation : Validate whether SequenceAlignment contains Aligned sequence or not.
        /// </summary>
        [Test]
        public void ValidateSequenceAlignmentContainsMethod()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.Contains, false);
        }

        /// <summary>
        /// Validate copying SequenceAlignment values to array.
        /// Input : Dna Sequence read from xml file.
        /// Validation : Validate copying SequenceAlignment values to array.
        /// </summary>
        [Test]
        public void ValidateCopiedSeqAlinmentItems()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.CopyTo, false);
        }

        /// <summary>
        /// Validate Remove Aligned Sequence from Sequence Alignment
        /// Input : Dna Sequence read from xml file.
        /// Validation : Validate Sequence Alignment.
        /// </summary>
        [Test]
        public void ValidateRemoveAlignedSeqItem()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.Remove, false);
        }

        /// <summary>
        /// Validate Sequence Alignment default constructor
        /// Input : Dna Sequence read from xml file.
        /// Validation : Validate Sequence Alignment default constructor
        /// </summary>
        [Test]
        public void ValidateSeqAlignmentDefaultCtr()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.Remove, true);
        }

        /// <summary>
        /// Validate SequenceAlignment AddSequence() method.
        /// Input : Dna Sequence read from xml file.
        /// Validation : Added sequences are got back and validated.
        /// </summary>
        [Test]
        public void ValidateAddSequenceToAlignedSeqList()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.AddSequence, false);
        }

        /// <summary>
        /// Validate GetEnumerator() method.
        /// Input : Dna Sequence read from xml file.
        /// Validation : Validate GetEnumerator() method
        /// </summary>
        [Test]
        public void ValidateAlignedSeqGetEnumerator()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.GetEnumerator, false);
        }

        /// <summary>
        /// Validate GetObjectData() method.
        /// Input : Dna Sequence read from xml file.
        /// Validation : Validate GetObjectData() method
        /// </summary>
        [Test]
        public void ValidateAlignedSeqGetObjectData()
        {
            ValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.GetObjectData, false);
        }

        #endregion Sequence Alignment P1 Test cases

        #region Supporting Methods

        /// <summary>
        /// Validates NeedlemanWunschAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        static void ValidateNeedlemanWunschAlignment(string nodeName, AlignParameters alignParam)
        {
            ValidateNeedlemanWunschAlignment(nodeName, alignParam, SimilarityMatrixParameters.Default);
        }

        /// <summary>
        /// Validates NeedlemanWunschAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="similarityMatrixParam">Similarity Matrix Parameter.</param>
        static void ValidateNeedlemanWunschAlignment(string nodeName, AlignParameters alignParam,
            SimilarityMatrixParameters similarityMatrixParam)
        {
            ValidateNeedlemanWunschAlignment(nodeName, alignParam, similarityMatrixParam,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        /// Validates NeedlemanWunschAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="similarityMatrixParam">Similarity Matrix Parameter.</param>
        /// <param name="alignType">Is the Align type Simple or Align with Gap Extension cost?</param>
        static void ValidateNeedlemanWunschAlignment(string nodeName, AlignParameters alignParam,
            SimilarityMatrixParameters similarityMatrixParam, AlignmentType alignType)
        {
            ISequence originalSequence1 = null;
            ISequence originalSequence2 = null;
            Sequence aInput = null;
            Sequence bInput = null;

            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));

            // Parse the files and get the sequence.
            IList<ISequence> seqs1 = null;
            IList<ISequence> seqs2 = null;

            if (alignParam.ToString().Contains("Code"))
            {
                string sequence1 = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode1);
                string sequence2 = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode2);

                aInput = new Sequence(alphabet, sequence1);
                bInput = new Sequence(alphabet, sequence2);
            }
            else
            {
                // Read the xml file for getting both the files for aligning.
                string filePath1 = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode1);
                string filePath2 = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode2);
                FastaParser parser = new FastaParser();
                seqs1 = parser.Parse(filePath1);
                parser = new FastaParser();
                seqs2 = parser.Parse(filePath2);

                originalSequence1 = seqs1[0];
                originalSequence2 = seqs2[0];
                aInput = new Sequence(alphabet, originalSequence1.ToString());
                bInput = new Sequence(alphabet, originalSequence2.ToString());
            }

            ApplicationLog.WriteLine(string.Format(null,
                "NeedlemanWunschAligner P1 : First sequence used is '{0}'.",
                aInput.ToString()));
            ApplicationLog.WriteLine(string.Format(null,
                "NeedlemanWunschAligner P1 : Second sequence used is '{0}'.",
                bInput.ToString()));

            Console.WriteLine(string.Format(null,
                "NeedlemanWunschAligner P1 : First sequence used is '{0}'.",
                aInput.ToString()));
            Console.WriteLine(string.Format(null,
                "NeedlemanWunschAligner P1 : Second sequence used is '{0}'.",
                bInput.ToString()));

            string blosumFilePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.BlosumFilePathNode);

            SimilarityMatrix sm = null;

            switch (similarityMatrixParam)
            {
                case SimilarityMatrixParameters.TextReader:
                    using (TextReader reader = new StreamReader(blosumFilePath))
                        sm = new SimilarityMatrix(reader);
                    break;
                case SimilarityMatrixParameters.DiagonalMatrix:
                    string matchValue = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.MatchScoreNode);
                    string misMatchValue = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.MisMatchScoreNode);
                    MoleculeType molType = Utility.GetMoleculeType(Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.MoleculeTypeNode));
                    sm = new DiagonalSimilarityMatrix(int.Parse(matchValue, null),
                        int.Parse(misMatchValue, null), molType);
                    break;
                default:
                    sm = new SimilarityMatrix(blosumFilePath);
                    break;
            }

            int gapOpenCost = int.Parse(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.GapOpenCostNode), (IFormatProvider)null);

            int gapExtensionCost = int.Parse(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.GapExtensionCostNode), (IFormatProvider)null);

            NeedlemanWunschAligner needlemanWunschObj = new NeedlemanWunschAligner();
            if (AlignParameters.AllParam != alignParam)
            {
                needlemanWunschObj.SimilarityMatrix = sm;
                needlemanWunschObj.GapOpenCost = gapOpenCost;
            }

            IList<IPairwiseSequenceAlignment> result = null;

            switch (alignParam)
            {
                case AlignParameters.AlignList:
                case AlignParameters.AlignListCode:
                    List<ISequence> sequences = new List<ISequence>();
                    sequences.Add(aInput);
                    sequences.Add(bInput);
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            needlemanWunschObj.GapExtensionCost = gapExtensionCost;
                            result = needlemanWunschObj.Align(sequences);
                            break;
                        default:
                            result = needlemanWunschObj.AlignSimple(sequences);
                            break;
                    }
                    break;
                case AlignParameters.AllParam:
                case AlignParameters.AllParamCode:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            needlemanWunschObj.GapExtensionCost = gapExtensionCost;
                            result = needlemanWunschObj.Align(sm,
                                gapOpenCost, gapExtensionCost, aInput, bInput);
                            break;
                        default:
                            result = needlemanWunschObj.AlignSimple(sm, gapOpenCost, aInput, bInput);
                            break;
                    }
                    break;
                case AlignParameters.AlignTwo:
                case AlignParameters.AlignTwoCode:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            needlemanWunschObj.GapExtensionCost = gapExtensionCost;
                            result = needlemanWunschObj.Align(aInput, bInput);
                            break;
                        default:
                            result = needlemanWunschObj.AlignSimple(aInput, bInput);
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
                    expectedScore = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedGapExtensionScoreNode);
                    expectedSequence1 = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedGapExtensionSequence1Node);
                    expectedSequence2 = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedGapExtensionSequence2Node);
                    break;
                default:
                    expectedScore = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedScoreNode);
                    expectedSequence1 = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedSequenceNode1);
                    expectedSequence2 = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedSequenceNode2);

                    break;
            }

            IList<IPairwiseSequenceAlignment> expectedOutput = new List<IPairwiseSequenceAlignment>();

            IPairwiseSequenceAlignment align = new PairwiseSequenceAlignment(aInput, bInput);
            PairwiseAlignedSequence alignedSeq = new PairwiseAlignedSequence();
            alignedSeq.FirstSequence = new Sequence(alphabet, expectedSequence1);
            alignedSeq.SecondSequence = new Sequence(alphabet, expectedSequence2);
            alignedSeq.Score = int.Parse(expectedScore);
            align.PairwiseAlignedSequences.Add(alignedSeq);
            expectedOutput.Add(align);
            Assert.IsTrue(CompareAlignment(result, expectedOutput));

            ApplicationLog.WriteLine(string.Format(null,
                "NeedlemanWunschAligner P1 : Final Score '{0}'.", expectedScore));
            ApplicationLog.WriteLine(string.Format(null,
                "NeedlemanWunschAligner P1 : Aligned First Sequence is '{0}'.",
                expectedSequence1));
            ApplicationLog.WriteLine(string.Format(null,
                "NeedlemanWunschAligner P1 : Aligned Second Sequence is '{0}'.",
                expectedSequence2));

            Console.WriteLine(string.Format(null,
                "NeedlemanWunschAligner P1 : Final Score '{0}'.", expectedScore));
            Console.WriteLine(string.Format(null,
                "NeedlemanWunschAligner P1 : Aligned First Sequence is '{0}'.",
                expectedSequence1));
            Console.WriteLine(string.Format(null,
                "NeedlemanWunschAligner P1 : Aligned Second Sequence is '{0}'.",
                expectedSequence2));
        }

        /// <summary>
        /// Validates SmithWatermanAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        static void ValidateSmithWatermanAlignment(string nodeName, AlignParameters alignParam)
        {
            ValidateSmithWatermanAlignment(nodeName, alignParam, SimilarityMatrixParameters.Default);
        }

        /// <summary>
        /// Validates SmithWatermanAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="similarityMatrixParam">Similarity Matrix Parameter.</param>
        static void ValidateSmithWatermanAlignment(string nodeName, AlignParameters alignParam,
            SimilarityMatrixParameters similarityMatrixParam)
        {
            ValidateSmithWatermanAlignment(nodeName, alignParam, similarityMatrixParam,
                AlignmentType.SimpleAlign);
        }

        /// <summary>
        /// Validates SmithWatermanAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="similarityMatrixParam">Similarity Matrix Parameter.</param>
        /// <param name="alignType">Is the Align type Simple or Align with Gap Extension cost?</param>
        static void ValidateSmithWatermanAlignment(string nodeName, AlignParameters alignParam,
            SimilarityMatrixParameters similarityMatrixParam, AlignmentType alignType)
        {
            ISequence originalSequence1 = null;
            ISequence originalSequence2 = null;
            Sequence aInput = null;
            Sequence bInput = null;

            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));

            // Parse the files and get the sequence.
            IList<ISequence> seqs1 = null;
            IList<ISequence> seqs2 = null;

            if (alignParam.ToString().Contains("Code"))
            {
                string sequence1 = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode1);
                string sequence2 = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode2);

                aInput = new Sequence(alphabet, sequence1);
                bInput = new Sequence(alphabet, sequence2);
            }
            else
            {
                // Read the xml file for getting both the files for aligning.
                string filePath1 = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode1);
                string filePath2 = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode2);
                FastaParser parser = new FastaParser();
                seqs1 = parser.Parse(filePath1);
                parser = new FastaParser();
                seqs2 = parser.Parse(filePath2);

                originalSequence1 = seqs1[0];
                originalSequence2 = seqs2[0];
                aInput = new Sequence(alphabet, originalSequence1.ToString());
                bInput = new Sequence(alphabet, originalSequence2.ToString());
            }

            string blosumFilePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.BlosumFilePathNode);

            SimilarityMatrix sm = null;

            switch (similarityMatrixParam)
            {
                case SimilarityMatrixParameters.TextReader:
                    using (TextReader reader = new StreamReader(blosumFilePath))
                        sm = new SimilarityMatrix(reader);
                    break;
                case SimilarityMatrixParameters.DiagonalMatrix:
                    string matchValue = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.MatchScoreNode);
                    string misMatchValue = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.MisMatchScoreNode);
                    MoleculeType molType = Utility.GetMoleculeType(
                        Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.MoleculeTypeNode));
                    sm = new DiagonalSimilarityMatrix(int.Parse(matchValue, null),
                        int.Parse(misMatchValue, null), molType);
                    break;
                default:
                    sm = new SimilarityMatrix(blosumFilePath);
                    break;
            }

            int gapOpenCost = int.Parse(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.GapOpenCostNode), (IFormatProvider)null);

            int gapExtensionCost = int.Parse(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.GapExtensionCostNode), (IFormatProvider)null);

            SmithWatermanAligner smithWatermanObj = new SmithWatermanAligner();

            if (AlignParameters.AllParam != alignParam)
            {
                smithWatermanObj.SimilarityMatrix = sm;
                smithWatermanObj.GapOpenCost = gapOpenCost;
            }

            IList<IPairwiseSequenceAlignment> result = null;

            switch (alignParam)
            {
                case AlignParameters.AlignList:
                case AlignParameters.AlignListCode:
                    List<ISequence> sequences = new List<ISequence>();
                    sequences.Add(aInput);
                    sequences.Add(bInput);
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            smithWatermanObj.GapExtensionCost = gapExtensionCost;
                            result = smithWatermanObj.Align(sequences);
                            break;
                        default:
                            result = smithWatermanObj.AlignSimple(sequences);
                            break;
                    }
                    break;
                case AlignParameters.AllParam:
                case AlignParameters.AllParamCode:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            smithWatermanObj.GapExtensionCost = gapExtensionCost;
                            result = smithWatermanObj.Align(sm, gapOpenCost,
                                gapExtensionCost, aInput, bInput);
                            break;
                        default:
                            result = smithWatermanObj.AlignSimple(sm, gapOpenCost, aInput, bInput);
                            break;
                    }
                    break;
                case AlignParameters.AlignTwo:
                case AlignParameters.AlignTwoCode:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            smithWatermanObj.GapExtensionCost = gapExtensionCost;
                            result = smithWatermanObj.Align(aInput, bInput);
                            break;
                        default:
                            result = smithWatermanObj.AlignSimple(aInput, bInput);
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
                    expectedScore = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedGapExtensionScoreNode);
                    expectedSequence1 = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedGapExtensionSequence1Node);
                    expectedSequence2 = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedGapExtensionSequence2Node);
                    break;
                default:
                    expectedScore = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedScoreNode);
                    expectedSequence1 = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedSequenceNode1);
                    expectedSequence2 = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedSequenceNode2);

                    break;
            }

            IList<IPairwiseSequenceAlignment> expectedOutput = new List<IPairwiseSequenceAlignment>();

            IPairwiseSequenceAlignment align = new PairwiseSequenceAlignment();
            PairwiseAlignedSequence alignedSeq = new PairwiseAlignedSequence();
            alignedSeq.FirstSequence = new Sequence(alphabet, expectedSequence1);
            alignedSeq.SecondSequence = new Sequence(alphabet, expectedSequence2);
            alignedSeq.Score = int.Parse(expectedScore);
            align.PairwiseAlignedSequences.Add(alignedSeq);
            expectedOutput.Add(align);
            Assert.IsTrue(CompareAlignment(result, expectedOutput));

            ApplicationLog.WriteLine(string.Format(null,
                "SmithWatermanAligner P1 : Final Score '{0}'.", expectedScore));
            ApplicationLog.WriteLine(string.Format(null,
                "SmithWatermanAligner P1 : Aligned First Sequence is '{0}'.",
               expectedSequence1));
            ApplicationLog.WriteLine(string.Format(null,
                "SmithWatermanAligner P1 : Aligned Second Sequence is '{0}'.",
                expectedSequence2));

            Console.WriteLine(string.Format(null,
                "SmithWatermanAligner P1 : Final Score '{0}'.", expectedScore));
            Console.WriteLine(string.Format(null,
                "SmithWatermanAligner P1 : Aligned First Sequence is '{0}'.",
                expectedSequence1));
            Console.WriteLine(string.Format(null,
                "SmithWatermanAligner P1 : Aligned Second Sequence is '{0}'.",
                expectedSequence2));
        }

        /// <summary>
        /// Validates PairwiseOverlapAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        static void ValidatePairwiseOverlapAlignment(string nodeName, AlignParameters alignParam)
        {
            ValidatePairwiseOverlapAlignment(nodeName, alignParam, SimilarityMatrixParameters.Default);
        }

        /// <summary>
        /// Validates PairwiseOverlapAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="similarityMatrixParam">Similarity Matrix Parameter.</param>
        static void ValidatePairwiseOverlapAlignment(string nodeName, AlignParameters alignParam,
            SimilarityMatrixParameters similarityMatrixParam)
        {
            ValidatePairwiseOverlapAlignment(nodeName, alignParam,
                similarityMatrixParam, AlignmentType.SimpleAlign);
        }

        /// <summary>
        /// Validates PairwiseOverlapAlignment algorithm for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="alignParam">parameter based on which certain validations are done.</param>
        /// <param name="similarityMatrixParam">Similarity Matrix Parameter.</param>
        /// <param name="alignType">Alignment Type</param>
        static void ValidatePairwiseOverlapAlignment(string nodeName, AlignParameters alignParam,
            SimilarityMatrixParameters similarityMatrixParam, AlignmentType alignType)
        {
            ISequence originalSequence1 = null;
            ISequence originalSequence2 = null;
            Sequence aInput = null;
            Sequence bInput = null;

            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));

            // Parse the files and get the sequence.
            IList<ISequence> seqs1 = null;
            IList<ISequence> seqs2 = null;

            if (alignParam.ToString().Contains("Code"))
            {
                string sequence1 = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode1);
                string sequence2 = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode2);

                aInput = new Sequence(alphabet, sequence1);
                bInput = new Sequence(alphabet, sequence2);
            }
            else
            {
                // Read the xml file for getting both the files for aligning.
                string filePath1 = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode1);
                string filePath2 = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode2);
                FastaParser parser = new FastaParser();
                seqs1 = parser.Parse(filePath1);
                parser = new FastaParser();
                seqs2 = parser.Parse(filePath2);

                originalSequence1 = seqs1[0];
                originalSequence2 = seqs2[0];
                aInput = new Sequence(alphabet, originalSequence1.ToString());
                bInput = new Sequence(alphabet, originalSequence2.ToString());
            }

            string blosumFilePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.BlosumFilePathNode);

            SimilarityMatrix sm = null;

            switch (similarityMatrixParam)
            {
                case SimilarityMatrixParameters.TextReader:
                    using (TextReader reader = new StreamReader(blosumFilePath))
                        sm = new SimilarityMatrix(reader);
                    break;
                case SimilarityMatrixParameters.DiagonalMatrix:
                    string matchValue = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.MatchScoreNode);
                    string misMatchValue = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.MisMatchScoreNode);
                    MoleculeType molType = Utility.GetMoleculeType(
                        Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.MoleculeTypeNode));
                    sm = new DiagonalSimilarityMatrix(int.Parse(matchValue, null),
                        int.Parse(misMatchValue, null), molType);
                    break;
                default:
                    sm = new SimilarityMatrix(blosumFilePath);
                    break;
            }

            int gapOpenCost = int.Parse(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.GapOpenCostNode), (IFormatProvider)null);

            int gapExtensionCost = int.Parse(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.GapExtensionCostNode), (IFormatProvider)null);

            PairwiseOverlapAligner pairwiseOverlapObj = new PairwiseOverlapAligner();
            if (AlignParameters.AllParam != alignParam)
            {
                pairwiseOverlapObj.SimilarityMatrix = sm;
                pairwiseOverlapObj.GapOpenCost = gapOpenCost;
            }

            IList<IPairwiseSequenceAlignment> result = null;

            switch (alignParam)
            {
                case AlignParameters.AlignList:
                case AlignParameters.AlignListCode:
                    List<ISequence> sequences = new List<ISequence>();
                    sequences.Add(aInput);
                    sequences.Add(bInput);
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            pairwiseOverlapObj.GapExtensionCost = gapExtensionCost;
                            result = pairwiseOverlapObj.Align(sequences);
                            break;
                        default:
                            result = pairwiseOverlapObj.AlignSimple(sequences);
                            break;
                    }
                    break;
                case AlignParameters.AllParam:
                case AlignParameters.AllParamCode:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            pairwiseOverlapObj.GapExtensionCost = gapExtensionCost;
                            result = pairwiseOverlapObj.Align(sm, gapOpenCost,
                                gapExtensionCost, aInput, bInput);
                            break;
                        default:
                            result = pairwiseOverlapObj.AlignSimple(sm, gapOpenCost, aInput, bInput);
                            break;
                    }
                    break;
                case AlignParameters.AlignTwo:
                case AlignParameters.AlignTwoCode:
                    switch (alignType)
                    {
                        case AlignmentType.Align:
                            pairwiseOverlapObj.GapExtensionCost = gapExtensionCost;
                            result = pairwiseOverlapObj.Align(aInput, bInput);
                            break;
                        default:
                            result = pairwiseOverlapObj.AlignSimple(aInput, bInput);
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
                    expectedScore = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedGapExtensionScoreNode);
                    expectedSequence1 = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedGapExtensionSequence1Node);
                    expectedSequence2 = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedGapExtensionSequence2Node);
                    break;
                default:
                    expectedScore = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedScoreNode);
                    expectedSequence1 = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedSequenceNode1);
                    expectedSequence2 = Utility._xmlUtil.GetTextValue(nodeName,
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
                alignedSeq.Score = int.Parse(expectedScore);
                align.PairwiseAlignedSequences.Add(alignedSeq);
            }
            expectedOutput.Add(align);

            Assert.IsTrue(CompareAlignment(result, expectedOutput));

            ApplicationLog.WriteLine(string.Format(null,
                "PairwiseOverlapAligner P1 : Final Score '{0}'.", expectedScore));
            ApplicationLog.WriteLine(string.Format(null,
                "PairwiseOverlapAligner P1 : Aligned First Sequence is '{0}'.",
                expectedSequence1));
            ApplicationLog.WriteLine(string.Format(null,
                "PairwiseOverlapAligner P1 : Aligned Second Sequence is '{0}'.",
                expectedSequence2));

            Console.WriteLine(string.Format(null,
                "PairwiseOverlapAligner P1 : Final Score '{0}'.", expectedScore));
            Console.WriteLine(string.Format(null,
                "PairwiseOverlapAligner P1 : Aligned First Sequence is '{0}'.",
                expectedSequence1));
            Console.WriteLine(string.Format(null,
                "PairwiseOverlapAligner P1 : Aligned Second Sequence is '{0}'.",
                expectedSequence2));
        }

        /// <summary>
        /// Validates Sequence Alignment test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="validateProperty">Is validation of properties required?</param>
        static void ValidateGeneralSequenceAlignment(string nodeName, bool validateProperty)
        {
            // Read the xml file for getting both the files for aligning.
            string origSequence1 = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SequenceNode1);
            string origSequence2 = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SequenceNode2);
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));

            ApplicationLog.WriteLine(string.Format(null,
                "SequenceAlignment P1 : First sequence used is '{0}'.",
             origSequence1));
            ApplicationLog.WriteLine(string.Format(null,
                "SequenceAlignment P1 : Second sequence used is '{0}'.",
                origSequence2));

            Console.WriteLine(string.Format(null,
                "SequenceAlignment P1 : First sequence used is '{0}'.",
                origSequence1));
            Console.WriteLine(string.Format(null,
                "SequenceAlignment P1 : Second sequence used is '{0}'.",
                origSequence2));

            // Create two sequences
            ISequence aInput = new Sequence(alphabet, origSequence1);
            ISequence bInput = new Sequence(alphabet, origSequence2);

            // Add the sequences to the Sequence alignment object using AddSequence() method.
            IList<IPairwiseSequenceAlignment> sequenceAlignmentObj = new List<IPairwiseSequenceAlignment>();

            PairwiseAlignedSequence alignSeq = new PairwiseAlignedSequence();
            alignSeq.FirstSequence = aInput;
            alignSeq.SecondSequence = bInput;
            IPairwiseSequenceAlignment seqAlignObj = new PairwiseSequenceAlignment();
            seqAlignObj.Add(alignSeq);
            sequenceAlignmentObj.Add(seqAlignObj);

            // Read the output back and validate the same.
            IList<PairwiseAlignedSequence> newAlignedSequences =
                sequenceAlignmentObj[0].PairwiseAlignedSequences;

            ApplicationLog.WriteLine(string.Format(null,
                "SequenceAlignment P1 : First sequence read is '{0}'.",
                origSequence1));
            ApplicationLog.WriteLine(string.Format(null,
                "SequenceAlignment P1 : Second sequence read is '{0}'.",
                origSequence2));

            Console.WriteLine(string.Format(null,
                "SequenceAlignment P1 : First sequence read is '{0}'.",
                origSequence1));
            Console.WriteLine(string.Format(null,
                "SequenceAlignment P1 : Second sequence read is '{0}'.",
                origSequence2));

            if (validateProperty)
            {
                string score = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.MatchScoreNode);
                string seqCount = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceCountNode);

                Assert.IsFalse(sequenceAlignmentObj.IsReadOnly);
                Assert.AreEqual(sequenceAlignmentObj.Count.ToString((IFormatProvider)null), seqCount);
                Assert.AreEqual(
                    sequenceAlignmentObj[0].PairwiseAlignedSequences[0].Score.ToString((IFormatProvider)null), score);

                Assert.AreEqual(sequenceAlignmentObj.Count.ToString((IFormatProvider)null), seqCount);

                ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the IsRead Property");
                ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the Count Property");
                ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the Sequences Property");

                Console.WriteLine("SequenceAlignment P1 : Successfully validated the IsRead Property");
                Console.WriteLine("SequenceAlignment P1 : Successfully validated the Count Property");
                Console.WriteLine("SequenceAlignment P1 : Successfully validated the Sequences Property");
            }
            else
            {
                Assert.AreEqual(newAlignedSequences[0].FirstSequence.ToString(), origSequence1);
                Assert.AreEqual(newAlignedSequences[0].SecondSequence.ToString(), origSequence2);
            }
        }

        /// <summary>
        /// Compare the alignment of mummer and defined alignment
        /// </summary>
        /// <param name="result">output of Aligners</param>
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
                    if (actualAlignment[resultCount].PairwiseAlignedSequences.Count == expectedAlignment[resultCount].PairwiseAlignedSequences.Count)
                    {
                        for (int alignSeqCount = 0; alignSeqCount < actualAlignment[resultCount].PairwiseAlignedSequences.Count; alignSeqCount++)
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
        /// Validates Sequence Alignment Class General methods.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="methodName">Name of the SequenceAlignment method to be validated</param>
        /// <param name="IsSeqAlignDefCtr">Is sequence alignment Def Constructor</param>
        /// </summary>
        static void ValidateSequenceAlignmentGeneralMethods(string nodeName, SeqAlignmentMethods methodName,
            bool IsSeqAlignDefCtr)
        {
            // Read the xml file for getting both the files for aligning.
            string origSequence1 = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SequenceNode1);
            string origSequence2 = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SequenceNode2);
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));
            string seqCount = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SeqCountNode);
            string AlignedSeqCountAfterAddSeq = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlignedSeqCountAfterAddAlignedSeqNode);
            string arrayLength = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.ArraySizeNode);

            PairwiseAlignedSequence[] alignedSeqItems = new PairwiseAlignedSequence[int.Parse(arrayLength)];
            int index = 0;
            SerializationInfo info = new SerializationInfo(typeof(PairwiseAlignedSequence),
                new FormatterConverter());
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            // Create two sequences
            ISequence aInput = new Sequence(alphabet, origSequence1);
            ISequence bInput = new Sequence(alphabet, origSequence2);

            // Add the sequences to the Sequence alignment object using AddSequence() method.
            IList<IPairwiseSequenceAlignment> sequenceAlignmentObj = new List<IPairwiseSequenceAlignment>();

            PairwiseAlignedSequence alignSeq = new PairwiseAlignedSequence();
            alignSeq.FirstSequence = aInput;
            alignSeq.SecondSequence = bInput;
            IPairwiseSequenceAlignment seqAlignObj;

            if (IsSeqAlignDefCtr)
            {
                seqAlignObj = new PairwiseSequenceAlignment();
            }
            else
            {
                seqAlignObj = new PairwiseSequenceAlignment(aInput, bInput);
            }

            seqAlignObj.Add(alignSeq);
            sequenceAlignmentObj.Add(seqAlignObj);

            IList<PairwiseAlignedSequence> newAlignedSequences =
                sequenceAlignmentObj[0].PairwiseAlignedSequences;

            switch (methodName)
            {
                case SeqAlignmentMethods.Add:
                    seqAlignObj.Add(alignSeq);
                    Assert.AreEqual(seqCount, seqAlignObj.PairwiseAlignedSequences.Count.ToString());
                    break;
                case SeqAlignmentMethods.Clear:
                    seqAlignObj.Clear();
                    Assert.AreEqual(0, seqAlignObj.PairwiseAlignedSequences.Count);
                    break;
                case SeqAlignmentMethods.Contains:
                    Assert.IsTrue(seqAlignObj.Contains(newAlignedSequences[0]));
                    break;
                case SeqAlignmentMethods.CopyTo:
                    seqAlignObj.CopyTo(alignedSeqItems, index);

                    // Validate Copied array.
                    Assert.AreEqual(alignedSeqItems[index].FirstSequence, seqAlignObj.FirstSequence);
                    Assert.AreEqual(alignedSeqItems[index].SecondSequence, seqAlignObj.SecondSequence);
                    break;
                case SeqAlignmentMethods.Remove:
                    seqAlignObj.Remove(newAlignedSequences[0]);

                    // Validate whether removed item is deleted from SequenceAlignment.
                    Assert.AreEqual(0, newAlignedSequences.Count);
                    break;
                case SeqAlignmentMethods.AddSequence:
                    seqAlignObj.AddSequence(newAlignedSequences[0]);

                    // Validate SeqAlignObj after adding aligned sequence.
                    Assert.AreEqual(AlignedSeqCountAfterAddSeq, seqAlignObj.Count.ToString());
                    break;
                case SeqAlignmentMethods.GetEnumerator:
                    IEnumerator<PairwiseAlignedSequence> alignedSeqList = seqAlignObj.GetEnumerator();

                    // Aligned Sequence list after iterating through ailgnedSeq collection.
                    Assert.IsNotNull(alignedSeqList);
                    break;
                case SeqAlignmentMethods.GetObjectData:
                    seqAlignObj.GetObjectData(info, context);

                    Assert.IsNotNull(seqAlignObj);
                    break;
                default:
                    break;
            }

            ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the IsRead Property");
            ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the Count Property");
            ApplicationLog.WriteLine("SequenceAlignment P1 : Successfully validated the Sequences Property");

            Console.WriteLine("SequenceAlignment P1 : Successfully validated the IsRead Property");
            Console.WriteLine("SequenceAlignment P1 : Successfully validated the Count Property");
            Console.WriteLine("SequenceAlignment P1 : Successfully validated the Sequences Property");
        }

        #endregion Supporting Methods
    }
}
