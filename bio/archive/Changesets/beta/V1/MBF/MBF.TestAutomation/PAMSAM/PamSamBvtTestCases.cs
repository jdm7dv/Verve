// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * PamSamBvtTestCases.cs
 * 
 *  This file contains the MuscleMultipleSequenceAlignment Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;

using MBF.Algorithms.Alignment.MultipleSequenceAlignment;
using MBF.IO;
using MBF.IO.Fasta;
using MBF.SimilarityMatrices;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.PAMSAM
{
    /// <summary>
    /// The class contains Bvt test cases to confirm Muscle MSA alignment.
    /// </summary>
    [TestFixture]
    public class PamSamBvtTestCases
    {

        #region Enums

        /// <summary>
        /// Different profile aligner method types
        /// </summary>
        enum AlignType
        {
            AlignSimpleAllParams,
            AlignSimpleOnlyProfiles,
            AlignAllParams
        }

        /// <summary>
        /// Collection of different score functions
        /// </summary>
        enum ScoreType
        {
            QScore,
            TCScore,
            Offset,
            MultipleAlignmentScoreFunction,
            PairWiseScoreFunction
        }

        /// <summary>
        /// Different mathematical functions present in MsaUtils
        /// </summary>
        enum FunctionType
        {
            Correlation,
            FindMaxIndex,
            JensenShanonDivergence,
            KullbackLeiblerDistance
        }

        #endregion Enums

        #region Global Variables

        /// <summary>
        /// Initialize input sequence list.
        /// </summary>
        List<ISequence> lstSequences;

        /// <summary>
        /// Initialize expected aligned sequence list.
        /// </summary>
        List<ISequence> expectedSequences;

        /// <summary>
        /// Initialize expected aligned sequence list for stage1.
        /// </summary>
        List<ISequence> stage1ExpectedSequences;

        /// <summary>
        /// Initialize expected aligned sequence list for stage2.
        /// </summary>
        List<ISequence> stage2ExpectedSequences;

        /// <summary>
        /// Initialize the expected score.
        /// </summary>
        string expectedScore = string.Empty;

        /// <summary>
        /// Initialize the expected score of Stage1.
        /// </summary>
        string stage1ExpectedScore = string.Empty;

        /// <summary>
        /// Initialize the expected score of Stage2.
        /// </summary>
        string stage2ExpectedScore = string.Empty;

        /// <summary>
        /// Similarity matrix object
        /// </summary>
        SimilarityMatrix similarityMatrix;

        /// <summary>
        /// Set it with NW/ SW profiler
        /// </summary>
        IProfileAligner profileAligner;

        /// <summary>
        /// kmer length to generate kmer distance matrix
        /// </summary>
        int kmerLength = 2;

        /// <summary>
        /// Initialize the gap open penalty
        /// </summary>
        int gapOpenPenalty = -8;

        /// <summary>
        /// Initialize gap extend penalty.
        /// </summary>
        int gapExtendPenalty = -3;

        #endregion

        #region Constructors

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static PamSamBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\MSAConfig.xml");
        }

        #endregion

        #region Test Cases

        #region PamSam TestCases

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequences()
        {
            ValidatePamsamAlign(Constants.MuscleDnaSequenceNode,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences 
        /// and score with distance matrix method name as ModifiedMuscle
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndModifiedMuscle()
        {
            ValidatePamsamAlignWithDistanceFunctionaTypes(
              Constants.MuscleDnaSequenceWithModifiedMuscleDistanceMethodNodeName,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              DistanceFunctionTypes.ModifiedMUSCLE,
              ProfileAlignerNames.NeedlemanWunschProfileAligner, false);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences 
        /// and score with distance matrix method name as EuclieanDistance
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndEuclieanDistance()
        {
            ValidatePamsamAlignWithDistanceFunctionaTypes(
              Constants.MuscleDnaSequenceNode,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              DistanceFunctionTypes.EuclideanDistance,
              ProfileAlignerNames.NeedlemanWunschProfileAligner, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences 
        /// and score with distance matrix method name as CoVariance
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndCOVariance()
        {
            ValidatePamsamAlignWithDistanceFunctionaTypes(
              Constants.MuscleDnaSequenceWithCoVarianceNodeName,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              DistanceFunctionTypes.CoVariance,
              ProfileAlignerNames.NeedlemanWunschProfileAligner, false);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences 
        /// and score with distance matrix method name as PearsonCorrelation
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndPearsonCorrelation()
        {
            ValidatePamsamAlignWithDistanceFunctionaTypes(
              Constants.MuscleDnaSequenceWithPearsonCorrelationDistanceMethodNodeName,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              DistanceFunctionTypes.PearsonCorrelation,
              ProfileAlignerNames.NeedlemanWunschProfileAligner, false);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences 
        /// and score with Hierarchical Clustering method name as Average
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndAverageMethod()
        {
            ValidatePamsamAlignWithUpdateDistanceMethodTypes(
              Constants.MuscleDnaSequenceNode,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              UpdateDistanceMethodsTypes.Average,
              ProfileAlignerNames.NeedlemanWunschProfileAligner, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score with 
        /// Hierarchical Clustering method name as Complete
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndCompleteMethod()
        {
            ValidatePamsamAlignWithUpdateDistanceMethodTypes(
              Constants.MuscleDnaSequenceNode,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              UpdateDistanceMethodsTypes.Complete,
              ProfileAlignerNames.NeedlemanWunschProfileAligner, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score with 
        /// Hierarchical Clustering method name as Single
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndSingleMethod()
        {
            ValidatePamsamAlignWithUpdateDistanceMethodTypes(
              Constants.MuscleDnaSequenceWithSingleDistanceMethodNodeName,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              UpdateDistanceMethodsTypes.Single,
              ProfileAlignerNames.NeedlemanWunschProfileAligner, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score with 
        /// Hierarchical Clustering method name as WeightedMAFFT
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndWeightedMafftMethod()
        {
            ValidatePamsamAlignWithUpdateDistanceMethodTypes(
              Constants.MuscleDnaSequenceWithWeightedMafftNode,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              UpdateDistanceMethodsTypes.WeightedMAFFT,
              ProfileAlignerNames.NeedlemanWunschProfileAligner, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as InnerProduct
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndInnerProduct()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MuscleDnaSequenceNode,
              MoleculeType.DNA,
              Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.InnerProduct, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as JensenShannonDivergence
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndJensenShannonDivergence()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MuscleDnaWithJensenShannonDivergence,
              MoleculeType.DNA,
              Constants.ExpectedScoreNode, ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.JensenShannonDivergence, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as LogExponentialInnerProduct
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndLogExponentialInnerProduct()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MuscleDnaWithLogExponentialInnerProduct,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.LogExponentialInnerProduct, false);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as LogExponentialInnerProductShifted
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndLogExponentialInnerProductShifted()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MuscleDnaSequenceWithLogExponentialInnerProductShiftedNodeName,
              MoleculeType.DNA,
              Constants.ExpectedScoreNode, ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.LogExponentialInnerProductShifted, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as PearsonCorrelation
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndPearsonCorrelationProfileScore()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MuscleDnaWithJensenShannonDivergence,
              MoleculeType.DNA,
              Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.PearsonCorrelation, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as SymmetrizedEntropy
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndSymmetrizedEntropy()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MuscleDnaWithSymmetrizedEntropy,
              MoleculeType.DNA,
              Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.SymmetrizedEntropy, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as WeightedEuclideanDistance
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndWeightedEuclideanDistance()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MuscleDnaSequenceWithWeightedEuclideanDistanceNodeName,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.WeightedEuclideanDistance, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as WeightedInnerProduct
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndWeightedInnerProduct()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MuscleDnaSequenceWithWeightedInnerProduct,
              MoleculeType.DNA,
              Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.WeightedInnerProduct, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as WeightedInnerProductShifted
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndWeightedInnerProductShifted()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MusclerDnaSequenceWithWeightedInnerProductShiftedNode,
              MoleculeType.DNA,
              Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.WeightedInnerProductShifted, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment Stage1 using its aligned sequences and score
        /// </summary>
        [Test]
        public void ValidatePamsamStage1WithDnaSequences()
        {
            ValidatePamsamAlignStage1(Constants.MuscleDnaSequenceNode,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              UpdateDistanceMethodsTypes.Average, DistanceFunctionTypes.EuclideanDistance,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.InnerProduct);
        }

        /// <summary>
        /// Validates Muscle sequence alignment Stage2 using its aligned sequences and score
        /// </summary>
        [Test]
        public void ValidatePamsamStage2WithDnaSequences()
        {
            ValidatePamsamAlignStage2(Constants.MuscleDnaSequenceNode,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              UpdateDistanceMethodsTypes.Average, DistanceFunctionTypes.EuclideanDistance,
              ProfileAlignerNames.NeedlemanWunschProfileAligner, ProfileScoreFunctionNames.InnerProduct);
        }

        /// <summary>
        /// Validates Muscle sequence alignment Stage3 using its aligned sequences and score
        /// </summary>
        [Test]
        public void ValidatePamsamStage3WithDnaSequences()
        {
            ValidatePamsamAlignStage3(Constants.MuscleDnaSequenceNode,
              MoleculeType.DNA, Constants.ExpectedScoreNode,
              UpdateDistanceMethodsTypes.Average, DistanceFunctionTypes.EuclideanDistance,
              ProfileAlignerNames.NeedlemanWunschProfileAligner, ProfileScoreFunctionNames.InnerProduct);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as InnerProductFast
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndInnerProductFast()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MultipleNWProfilerDnaSequenceWithInnerProductFastNode,
              MoleculeType.DNA,
              Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.InnerProductFast, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as LogExponentialInnerProductFast
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndLogExponentialInnerProductFast()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MuscleDnaWithLogExponentialInnerProductFastNode,
              MoleculeType.DNA,
              Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.LogExponentialInnerProductFast, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as LogExponentialInnerProductShiftedFast
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndLogExponentialInnerProductShiftedFast()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MuscleDnaWithLogExponentialInnerProductFastNode,
              MoleculeType.DNA,
              Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.LogExponentialInnerProductShiftedFast, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as WeightedEuclideanDistanceFast
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndWeightedEuclideanDistanceFast()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MuscleDnaSequenceWithWeightedEuclideanDistanceNodeName,
              MoleculeType.DNA,
              Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.WeightedEuclideanDistanceFast, true);
        }

        /// <summary>
        /// Validates Muscle sequence alignment using its aligned sequences and score.
        /// Profile score method name as WeightedInnerProductShiftedFast
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesAndWeightedInnerProductShiftedFast()
        {
            ValidatePamsamAlignWithProfileScoreFunctionName(
              Constants.MuscleDnaSequenceWithWeightedInnerProductShiftedFastNode,
              MoleculeType.DNA,
              Constants.ExpectedScoreNode,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.WeightedInnerProductShiftedFast, true);
        }

        /// <summary>
        /// Validate Muscle sequence alignment with sequence weights
        /// </summary>
        [Test]
        public void ValidateUseWeightsPamsamWithDnaSequences()
        {
            ValidatePamsamAlign(Constants.MuscleDnaSequenceWithWeightsNode, MoleculeType.DNA,
              Constants.ExpectedScoreNode, UpdateDistanceMethodsTypes.Average,
              DistanceFunctionTypes.EuclideanDistance, ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.InnerProductFast, true, false, false);
        }

        /// <summary>
        /// Validate the faster version of muscle multiple sequence alignment
        /// </summary>
        [Test]
        public void ValidatePamsamWithDnaSequencesWithFasterVersion()
        {
            ValidatePamsamAlign(
              Constants.MuscleDnaWithJensenShannonDivergence, MoleculeType.DNA,
              Constants.ExpectedScoreNode, UpdateDistanceMethodsTypes.Average,
              DistanceFunctionTypes.EuclideanDistance, ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.InnerProductFast, false, true, false);
        }

        #endregion

        #region KmerDistanceMatrix

        /// <summary>
        /// Validate kmerdistancematrix for stage1 with default distance function name
        /// </summary>
        [Test]
        public void ValidatePamsamKmerDistanceMatrixStage1()
        {
            ValidateKmerDistanceMatrixStage1(Constants.KmerDistanceMatrixNode, 3, MoleculeType.DNA);
        }

        /// <summary>
        /// Validate kmerdistancematrix for stage1 with EuclieanDistance distance function name
        /// </summary>
        [Test]
        public void ValidatePamsamKmerDistanceMatrixWithEuclieanDistance()
        {
            ValidateKmerDistanceMatrixStage1(Constants.KmerDistanceMatrixNode, 3,
              MoleculeType.DNA, DistanceFunctionTypes.EuclideanDistance);
        }

        /// <summary>
        /// Validate kmerdistancematrix for stage1 with PearsonCorrelation distance function name
        /// </summary>
        [Test]
        public void ValidatePamsamKmerDistanceMatrixWithPearsonCorrelation()
        {
            ValidateKmerDistanceMatrixStage1(Constants.KmerDistanceMatrixWithPearsonCorrelation,
              kmerLength, MoleculeType.DNA, DistanceFunctionTypes.PearsonCorrelation);
        }

        /// <summary>
        /// Validate kmerdistancematrix for stage1 with CoVariance distance function name
        /// </summary>
        [Test]
        public void ValidatePamsamKmerDistanceMatrixWithCOVariance()
        {
            ValidateKmerDistanceMatrixStage1(Constants.KmerDistanceMatrixWithCoVariance, kmerLength,
              MoleculeType.DNA, DistanceFunctionTypes.CoVariance);
        }

        /// <summary>
        /// Validate kmerdistancematrix for stage1 with ModifiedMUSCLE distance function name
        /// </summary>
        [Test]
        public void ValidatePamsamKmerDistanceMatrixWithModifiedMuscle()
        {
            ValidateKmerDistanceMatrixStage1(Constants.KmerDistanceMatrixWithModifiedMuscle, kmerLength,
              MoleculeType.DNA, DistanceFunctionTypes.ModifiedMUSCLE);
        }

        #endregion

        #region HierarchicalClusteringStage1 & Stage2

        /// <summary>
        /// Validate HierarchicalClustering for stage1 using kmer distance matrix
        /// </summary>
        [Test]
        public void ValidatePamsamHierarchicalClusteringStage1()
        {
            ValidateHierarchicalClusteringStage1(Constants.HierarchicalClusteringNode,
              kmerLength, MoleculeType.DNA);
        }

        /// <summary>
        /// Validate HierarchicalClustering for stage1 using kmer distance matrix 
        /// and hierarchical clustering method name as Average
        /// </summary>
        [Test]
        public void ValidatePamsamHierarchicalClusteringWithAverage()
        {
            ValidateHierarchicalClusteringStage1(Constants.HierarchicalClusteringNode,
              kmerLength, MoleculeType.DNA, UpdateDistanceMethodsTypes.Average);
        }

        /// <summary>
        /// Validate HierarchicalClustering for stage1 using kmer distance matrix 
        /// and hierarchical clustering method name as Single
        /// </summary>
        [Test]
        public void ValidatePamsamHierarchicalClusteringWithSingle()
        {
            ValidateHierarchicalClusteringStage1(Constants.HierarchicalClusteringWeightedMAFFT,
              kmerLength, MoleculeType.DNA, UpdateDistanceMethodsTypes.Single);
        }

        /// <summary>
        /// Validate HierarchicalClustering for stage1 using kmer distance matrix 
        /// and hierarchical clustering method name as Complete
        /// </summary>
        [Test]
        public void ValidatePamsamHierarchicalClusteringWithComplete()
        {
            ValidateHierarchicalClusteringStage1(Constants.HierarchicalClusteringNode,
              kmerLength, MoleculeType.DNA, UpdateDistanceMethodsTypes.Complete);
        }

        /// <summary>
        /// Validate HierarchicalClustering for stage1 using kmer distance matrix
        /// and hierarchical clustering method name as WeightedMAFFT
        /// </summary>
        [Test]
        public void ValidatePamsamHierarchicalClusteringWithWeightedMafft()
        {
            ValidateHierarchicalClusteringStage1(Constants.HierarchicalClusteringWeightedMAFFT, kmerLength,
              MoleculeType.DNA, UpdateDistanceMethodsTypes.WeightedMAFFT);
        }

        /// <summary>
        /// Validate HierarchicalClustering for stage1 using kimura distance matrix
        /// and stage 1 aligned sequences
        /// </summary>
        [Test]
        public void ValidatePamsamHierarchicalClusteringStage2()
        {
            ValidateHierarchicalClusteringStage2(Constants.HierarchicalClusteringStage2Node,
              MoleculeType.DNA);
        }

        /// <summary>
        /// Validate HierarchicalClustering for stage1 
        /// using kimura distance matrix with hierarchical method name as Average 
        /// </summary>
        [Test]
        public void ValidatePamsamHierarchicalClusteringStage2WithAverage()
        {
            ValidateHierarchicalClusteringStage2(Constants.HierarchicalClusteringStage2Node,
              MoleculeType.DNA,
              UpdateDistanceMethodsTypes.Average);
        }

        /// <summary>
        /// Validate HierarchicalClustering for stage1 
        /// using kimura distance matrix with hierarchical method name as Complete
        /// </summary>
        [Test]
        public void ValidatePamsamHierarchicalClusteringStage2WithComplete()
        {
            ValidateHierarchicalClusteringStage2(Constants.HierarchicalClusteringStage2WithCompleteNode,
              MoleculeType.DNA, UpdateDistanceMethodsTypes.Complete);
        }

        /// <summary>
        /// Validate HierarchicalClustering for stage1 
        /// using kimura distance matrix with hierarchical method name as Single
        /// </summary>
        [Test]
        public void ValidatePamsamHierarchicalClusteringStage2WithSingle()
        {
            ValidateHierarchicalClusteringStage2(Constants.HierarchicalClusteringStage2WithSingleNode,
              MoleculeType.DNA, UpdateDistanceMethodsTypes.Single);
        }

        /// <summary>
        /// Validate HierarchicalClustering for stage1 
        /// using kimura distance matrix with hierarchical method name as WeightedMAFFT
        /// </summary>
        [Test]
        public void ValidatePamsamHierarchicalClusteringStage2WithWeightedMafft()
        {
            ValidateHierarchicalClusteringStage2(Constants.HierarchicalClusteringStage2WithWeightedMAFFT,
              MoleculeType.DNA, UpdateDistanceMethodsTypes.WeightedMAFFT);
        }

        #endregion

        #region BinaryTreeStage1 & Stage2

        /// <summary>
        /// Validate HierarchicalClustering for stage1 using kmer distance matrix
        /// </summary>
        [Test]
        public void ValidatePamsamBinaryTreeStage1()
        {
            ValidateBinaryTreeStage1(Constants.BinaryTreeNode, kmerLength, MoleculeType.DNA);
        }

        /// <summary>
        /// Validate HierarchicalClustering for stage1 using kmer distance matrix
        /// </summary>
        [Test]
        public void ValidatePamsamBinaryTreeStage2()
        {
            ValidateBinaryTreeStage2(Constants.BinaryTreeStage2Node, MoleculeType.DNA);
        }

        /// <summary>
        /// Validate HierarchicalClustering for stage1 using kmer distance matrix
        /// </summary>
        [Test]
        public void ValidatePamsamBinaryTreeSeparateSequencesByCutTree()
        {
            ValidateBinaryTreeSeparateSequencesByCutTree(3, Constants.BinaryTreeNode, 4, MoleculeType.DNA);
        }

        #endregion

        #region ProfileAligner & ProgressiveAlignment

        /// <summary>
        /// Validate ProgressiveAligner using stage1 sequences.
        /// </summary>
        [Test]
        public void ValidatePamsamPrgressiveAlignerStage1()
        {
            ValidateProgressiveAlignmentStage1(Constants.MuscleDnaSequenceNode, MoleculeType.DNA);
        }

        /// <summary>
        /// Validate Profile Aligner GenerateEString() method using two profiles of sub trees.
        /// </summary>
        [Test]
        public void ValidatePamsamProfileAlignerGenerateEString()
        {
            ValidateProfileAlignerGenerateEString(Constants.ProfileAligner, MoleculeType.DNA, 4);
        }

        /// <summary>
        /// Validate Profile Aligner GenerateSequencesEString() method using two profiles of sub trees.
        /// </summary>
        [Test]
        public void ValidatePamsamProfileAlignerGenerateSequencesEString()
        {
            ValidateProfileAlignerGenerateSequenceString(Constants.ProfileAlignerWithAlignmentNode,
              MoleculeType.DNA, 4);
        }
        #endregion

        #region KimuraDistanceMatrix

        /// <summary>
        /// Validate kimura distance matrix using stage1 aligned sequences.
        /// </summary>
        [Test]
        public void ValidatePamsamKimuraDistanceMatrix()
        {
            ValidateKimuraDistanceMatrix(Constants.KimuraDistanceMatrix, MoleculeType.DNA);
        }

        #endregion

        #region NeedlemanProfileAlignerSerial

        /// <summary>
        /// Creates binarytree using stage2 sequences and 
        /// cut the binary tree at an random edge to get two profiles.
        /// Create NeedlemanWunschProfileAlignerSerial instance 
        /// and execute AlignSimple(Profile A,Profile B).
        /// Validate the IProfileAlignment properties.
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialAlignSimple()
        {
            ValidateProfileAlignerAlign(Constants.ProfileAligner, MoleculeType.DNA,
              Constants.SerialProcess, 3, AlignType.AlignSimpleOnlyProfiles);
        }

        /// <summary>
        /// Creates binarytree using stage2 sequences 
        /// and cut the binary tree at an random edge to get two profiles.
        /// Create NeedlemanWunschProfileAlignerSerial instance 
        /// and execute AlignSimple(sm, gapOpenPenalty, Profile A,Profile B).
        /// Validate the IProfileAlignment properties.
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialAlignSimpleWithAllParams()
        {
            ValidateProfileAlignerAlign(Constants.ProfileAligner, MoleculeType.DNA,
              Constants.SerialProcess, 3, AlignType.AlignSimpleAllParams);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with
        /// profilescorefunction name as "WeightedInnerProductCached".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithWeightedInnerProductCached()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithWeightedInnerProductCachedNode,
              Constants.SerialProcess, ProfileScoreFunctionNames.WeightedInnerProductCached,
              5, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with
        /// profilescorefunction name as "WeightedInnerProductFast".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithWeightedInnerProductFast()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithWeightedInnerProductCachedNode, Constants.SerialProcess,
              ProfileScoreFunctionNames.WeightedInnerProductFast, 5, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with
        /// profilescorefunction name as "WeightedInnerProduct".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithWeightedInnerProduct()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithWeightedInnerProductCachedNode, Constants.SerialProcess,
              ProfileScoreFunctionNames.WeightedInnerProduct, 5, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with
        /// profilescorefunction name as "WeightedInnerProductShiftedFast".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithWeightedInnerProductShiftedFast()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithWeightedInnerProductCachedNode, Constants.SerialProcess,
              ProfileScoreFunctionNames.WeightedInnerProductShiftedFast, 5, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with
        /// profilescorefunction name as "WeightedEuclideanDistanceFast".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithWeightedEuclideanDistanceFast()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithWeightedEuclideanDistanceFastNode, Constants.SerialProcess,
              ProfileScoreFunctionNames.WeightedEuclideanDistanceFast, 5, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with
        /// profilescorefunction name as "WeightedEuclideanDistance".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithWeightedEuclideanDistance()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithWeightedEuclideanDistanceFastNode, Constants.SerialProcess,
              ProfileScoreFunctionNames.WeightedEuclideanDistance, 5, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with
        /// profilescorefunction name as "PearsonCorrelation".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithPearsonCorrelation()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithWeightedInnerProductCachedNode, Constants.SerialProcess,
              ProfileScoreFunctionNames.PearsonCorrelation, 5, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with
        /// profilescorefunction name as "LogExponentialInnerProductShiftedFast".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithLogExponentialInnerProductShiftedFast()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithLogExponentialInnerProductShiftedFastNode,
              Constants.SerialProcess,
              ProfileScoreFunctionNames.LogExponentialInnerProductShiftedFast, 3, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with
        /// profilescorefunction name as "LogExponentialInnerProductShifted".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithLogExponentialInnerProductShifted()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithWeightedEuclideanDistanceFastNode, Constants.SerialProcess,
              ProfileScoreFunctionNames.LogExponentialInnerProductShifted, 5, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with
        /// profilescorefunction name as "LogExponentialInnerProduct".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithLogExponentialInnerProduct()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithWeightedInnerProductNode, Constants.SerialProcess,
              ProfileScoreFunctionNames.LogExponentialInnerProduct, 7, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with
        /// profilescorefunction name as "LogExponentialInnerProductFast".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithLogExponentialInnerProductFast()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithLogExponentialInnerProductFastNode, Constants.SerialProcess,
              ProfileScoreFunctionNames.LogExponentialInnerProductFast, 5, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with
        /// profilescorefunction name as "JensenShannonDivergence".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithJensenShannonDivergence()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAligner, Constants.SerialProcess,
              ProfileScoreFunctionNames.JensenShannonDivergence, 8, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with profilescorefunction 
        /// name as "InnerProductFast".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithInnerProductFast()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAligner, Constants.SerialProcess,
              ProfileScoreFunctionNames.InnerProductFast, 8, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with profilescorefunction 
        /// name as "InnerProduct".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialWithInnerProduct()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAligner, Constants.SerialProcess,
              ProfileScoreFunctionNames.InnerProduct, 8, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates binarytree using stage2 sequences and cut the binary tree at an 
        /// random edge to get two profiles.
        /// Create NeedlemanWunschProfileAlignerParallel instance and execute Align(Profile A,Profile B).
        /// Validate the IProfileAlignment properties.
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialAlign()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithWeightedInnerProductNode, Constants.SerialProcess,
              ProfileScoreFunctionNames.WeightedInnerProduct, 3, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates binarytree using stage2 sequences and cut the binary tree at an 
        /// random edge to get two profiles.
        /// Create NeedlemanWunschProfileAlignerParallel instance and execute 
        /// AlignSimple(Profile A,Profile B).
        /// Validate the IProfileAlignment properties.
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerSerialAlignWithAllParams()
        {
            ValidateProfileAlignerAlign(Constants.ProfileAligner, MoleculeType.DNA,
              Constants.SerialProcess, 2, AlignType.AlignAllParams);
        }

        #endregion

        #region NeedlemanProfileAlignerParallel

        /// <summary>
        /// Creates binarytree using stage1 sequences and cut the binary tree 
        /// at an random edge to get two profiles.
        /// Create NeedlemanWunschProfileAlignerParallel instance and execute 
        /// AlignSimple(Profile A,Profile B).
        /// Validate the IProfileAlignment properties.
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerParallelAlignSimple()
        {
            ValidateProfileAlignerAlign(Constants.ProfileAligner, MoleculeType.DNA,
              Constants.ParallelProcess, 3, AlignType.AlignSimpleOnlyProfiles);
        }

        /// <summary>
        /// Creates binarytree using stage1 sequences and cut the binary tree 
        /// at an random edge to get two profiles.
        /// Create NeedlemanWunschProfileAlignerParallel instance and execute
        /// AlignSimple(sm, gappenalty,Profile A,Profile B).
        /// Validate the IProfileAlignment properties.
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerParallelAlignSimpleAllParams()
        {
            ValidateProfileAlignerAlign(Constants.ProfileAligner, MoleculeType.DNA,
              Constants.ParallelProcess, 3, AlignType.AlignSimpleAllParams);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with 
        /// profilescorefunction name as "WeightedInnerProductCached".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerParallelWithWeightedInnerProductCached()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithWeightedInnerProductCachedNode,
              Constants.ParallelProcess,
              ProfileScoreFunctionNames.WeightedInnerProductCached,
              5, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates NeedlemanWunschProfileAlignerParallel instance with 
        /// profilescorefunction name as "WeightedInnerProductFast".
        /// Execute Align() method and Validate IProfileAlignment
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerParallelWithWeightedInnerProductFast()
        {
            ValidateProfileAlignerAlignWithProfileFunctionScore(
              Constants.ProfileAlignerWithWeightedInnerProductCachedNode,
              Constants.ParallelProcess,
              ProfileScoreFunctionNames.WeightedInnerProductFast,
              5, MoleculeType.DNA);
        }

        /// <summary>
        /// Creates binarytree using stage1 sequences and cut the binary tree 
        /// at an random edge to get two profiles.
        /// Create NeedlemanWunschProfileAlignerParallel instance and execute 
        /// Align(sm, gappenalty,Profile A,Profile B).
        /// Validate the IProfileAlignment properties.
        /// </summary>
        [Test]
        public void ValidateNWProfileAlignerParallelAlignAllParams()
        {
            ValidateProfileAlignerAlign(Constants.ProfileAligner, MoleculeType.DNA,
              Constants.ParallelProcess, 3, AlignType.AlignAllParams);
        }

        #endregion

        #region MsaUtils

        /// <summary>
        /// Validate the QScore of 12 Pamsam aligned dna sequences against benmark sequences
        /// </summary>
        [Test]
        public void ValidateAlignmentQualityScore()
        {
            ValidateAlignmentScore(Constants.DnaWith12SequencesNode, MoleculeType.DNA, ScoreType.QScore);
        }

        /// <summary>
        /// Validate the TCScore of 12 Pamsam aligned dna sequences against benmark sequences
        /// </summary>
        [Test]
        public void ValidateAlignmentTCScore()
        {
            ValidateAlignmentScore(Constants.DnaWith12SequencesNode, MoleculeType.DNA, ScoreType.TCScore);
        }

        /// <summary>
        /// Execute the CalculateOffset().
        /// Validate the number of residues whose position index will be negative 
        /// </summary>
        [Test]
        public void ValidateAlignmentOffset()
        {
            ValidateAlignmentScore(Constants.DnaWith12SequencesNode, MoleculeType.DNA, ScoreType.Offset);
        }

        /// <summary>
        /// Validate the Multiple alignment score of Pamsam aligned sequences
        /// </summary>
        [Test]
        public void ValidateMultipleAlignmentScore()
        {
            ValidateAlignmentScore(Constants.DnaWith12SequencesNode,
              MoleculeType.DNA, ScoreType.MultipleAlignmentScoreFunction);
        }

        /// <summary>
        /// Validate the pairwise score function of a pair of aligned sequences
        /// </summary>
        [Test]
        public void ValidatePairWiseScoreFunction()
        {
            ValidateAlignmentScore(Constants.DnaWith12SequencesNode,
              MoleculeType.DNA, ScoreType.PairWiseScoreFunction);
        }

        /// <summary>
        /// Get two profiles after cutting the edge of binary tree.
        /// Validate the correlation value of two profiles.
        /// </summary>
        [Test]
        public void ValidateCorrelation()
        {
            ValidateFunctionCalculations(Constants.DnaFunctionsNode,
              MoleculeType.DNA, 4, FunctionType.Correlation);
        }

        /// <summary>
        /// Get profiles after cutting the edge of binary tree.
        /// Validate the max index value of a profile.
        /// </summary>
        [Test]
        public void ValidateFindMaxIndex()
        {
            ValidateFunctionCalculations(Constants.DnaFunctionsNode,
              MoleculeType.DNA, 4, FunctionType.FindMaxIndex);
        }

        /// <summary>
        /// Get profiles after cutting the edge of binary tree.
        /// Validate the JensenShanonDivergence value of two profiles.
        /// </summary>
        [Test]
        public void ValidateJensenShanonDivergence()
        {
            ValidateFunctionCalculations(Constants.DnaFunctionsNode,
              MoleculeType.DNA, 4, FunctionType.JensenShanonDivergence);
        }

        /// <summary>
        /// Get profiles after cutting the edge of binary tree.
        /// Validate the KullbackLeiblerDistance of two profiles.
        /// </summary>
        [Test]
        public void ValidateKullbackLeiblerDistance()
        {
            ValidateFunctionCalculations(Constants.DnaFunctionsNode,
              MoleculeType.DNA, 4, FunctionType.KullbackLeiblerDistance);
        }

        /// <summary>
        /// Get pam sam aligned sequences. Execute UnAlign() method 
        /// and verify that it does not contains gap
        /// </summary>
        [Test]
        public void ValidateUNAlign()
        {
            ValidateUNAlign(Constants.DnaWith12SequencesNode, MoleculeType.DNA);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Read from xml config and initialize all member variables
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="expectedScoreNode">Expected score node</param>
        void Initialize(string nodeName, string expectedScoreNode)
        {
            // Read all the input sequences from xml config file
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(nodeName,
              Constants.AlphabetNameNode));
            string sequenceString1 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Sequence1);
            string sequenceString2 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Sequence2);
            string sequenceString3 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Sequence3);
            string sequenceString4 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Sequence4);
            string sequenceString5 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Sequence5);
            string sequenceString6 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Sequence6);
            string sequenceString7 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Sequence7);
            string sequenceString8 = null;
            string sequenceString9 = null;

            // Get all the input sequence object
            lstSequences = new List<ISequence>();
            ISequence seq1 = new Sequence(alphabet, sequenceString1);
            ISequence seq2 = new Sequence(alphabet, sequenceString2);
            ISequence seq3 = new Sequence(alphabet, sequenceString3);
            ISequence seq4 = new Sequence(alphabet, sequenceString4);
            ISequence seq5 = new Sequence(alphabet, sequenceString5);
            ISequence seq6 = new Sequence(alphabet, sequenceString6);
            ISequence seq7 = new Sequence(alphabet, sequenceString7);
            ISequence seq8 = null;
            ISequence seq9 = null;

            // Add all sequences to list.
            lstSequences.Add(seq1);
            lstSequences.Add(seq2);
            lstSequences.Add(seq3);
            lstSequences.Add(seq4);
            lstSequences.Add(seq5);
            lstSequences.Add(seq6);
            lstSequences.Add(seq7);

            similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            profileAligner = new NeedlemanWunschProfileAlignerParallel(similarityMatrix, ProfileScoreFunctionNames.InnerProduct, gapOpenPenalty, gapExtendPenalty, Environment.ProcessorCount);

            // Read all expected Sequences
            sequenceString1 = Utility._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode1);
            sequenceString2 = Utility._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode2);
            sequenceString3 = Utility._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode3);
            sequenceString4 = Utility._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode4);
            sequenceString5 = Utility._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode5);
            sequenceString6 = Utility._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode6);
            sequenceString7 = Utility._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode7);
            sequenceString8 = Utility._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode8);
            sequenceString9 = Utility._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequenceNode9);

            seq1 = new Sequence(alphabet, sequenceString1);
            seq2 = new Sequence(alphabet, sequenceString2);
            seq3 = new Sequence(alphabet, sequenceString3);
            seq4 = new Sequence(alphabet, sequenceString4);
            seq5 = new Sequence(alphabet, sequenceString5);
            seq6 = new Sequence(alphabet, sequenceString6);
            seq7 = new Sequence(alphabet, sequenceString7);
            seq8 = new Sequence(alphabet, sequenceString8);
            seq9 = new Sequence(alphabet, sequenceString9);

            // Add all sequences to list.
            expectedSequences = new List<ISequence>();
            expectedSequences.Add(seq1);
            expectedSequences.Add(seq2);
            expectedSequences.Add(seq3);
            expectedSequences.Add(seq4);
            expectedSequences.Add(seq5);
            expectedSequences.Add(seq6);
            expectedSequences.Add(seq7);
            expectedSequences.Add(seq8);
            expectedSequences.Add(seq9);

            expectedScore = Utility._xmlUtil.GetTextValue(nodeName, expectedScoreNode);

            // Parallel Option will only get set if the PAMSAMMultipleSequenceAligner is getting called
            // To test separately distance matrix, binary tree etc.. 
            // Set the parallel option using below ctor.
            PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner(lstSequences,
              MoleculeType.DNA, kmerLength, DistanceFunctionTypes.EuclideanDistance,
              UpdateDistanceMethodsTypes.Average,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.InnerProduct, similarityMatrix,
              gapOpenPenalty, gapExtendPenalty, 2, 2);

            if (null != msa)
            {
                Console.WriteLine(String.Format(
                  "Initialization of all variables successfully completed for xml node {0}", nodeName));
                ApplicationLog.WriteLine(String.Format(
                  "Initialization of all variables successfully completed for xml node {0}", nodeName));
            }
        }

        void InitializeStage1Variables(string nodeName)
        {
            // Read all the input sequences from xml config file
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(nodeName,
              Constants.AlphabetNameNode));
            // Read all expected Sequences for stage1 
            string sequenceString1 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Stage1ExpectedSequenceNode1);
            string sequenceString2 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Stage1ExpectedSequenceNode2);
            string sequenceString3 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Stage1ExpectedSequenceNode3);
            string sequenceString4 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Stage1ExpectedSequenceNode4);
            string sequenceString5 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Stage1ExpectedSequenceNode5);
            string sequenceString6 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Stage1ExpectedSequenceNode6);
            string sequenceString7 = Utility._xmlUtil.GetTextValue(nodeName, Constants.Stage1ExpectedSequenceNode7);

            // Get all expected stage1 sequences object
            ISequence seq1 = new Sequence(alphabet, sequenceString1);
            ISequence seq2 = new Sequence(alphabet, sequenceString2);
            ISequence seq3 = new Sequence(alphabet, sequenceString3);
            ISequence seq4 = new Sequence(alphabet, sequenceString4);
            ISequence seq5 = new Sequence(alphabet, sequenceString5);
            ISequence seq6 = new Sequence(alphabet, sequenceString6);
            ISequence seq7 = new Sequence(alphabet, sequenceString7);

            stage1ExpectedSequences = new List<ISequence>();
            stage1ExpectedSequences.Add(seq1);
            stage1ExpectedSequences.Add(seq2);
            stage1ExpectedSequences.Add(seq3);
            stage1ExpectedSequences.Add(seq4);
            stage1ExpectedSequences.Add(seq5);
            stage1ExpectedSequences.Add(seq6);
            stage1ExpectedSequences.Add(seq7);

            stage1ExpectedScore = Utility._xmlUtil.GetTextValue(nodeName, Constants.Stage1ExpectedScoreNode);

            Console.WriteLine(String.Format(
              "Initialization of stage1 variables successfully completed for xml node {0}", nodeName));
            ApplicationLog.WriteLine(String.Format(
              "Initialization of stage1 variables successfully completed for xml node {0}", nodeName));
        }

        void InitializeStage2Variables(string nodeName)
        {
            // Read all the input sequences from xml config file
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(nodeName,
              Constants.AlphabetNameNode));
            // Read all expected Sequences for stage1 
            string sequenceString1 = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.Stage2ExpectedSequenceNode1);
            string sequenceString2 = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.Stage2ExpectedSequenceNode2);
            string sequenceString3 = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.Stage2ExpectedSequenceNode3);
            string sequenceString4 = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.Stage2ExpectedSequenceNode4);
            string sequenceString5 = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.Stage2ExpectedSequenceNode5);
            string sequenceString6 = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.Stage2ExpectedSequenceNode6);
            string sequenceString7 = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.Stage2ExpectedSequenceNode7);

            // Get all expected stage1 sequences object
            ISequence seq1 = new Sequence(alphabet, sequenceString1);
            ISequence seq2 = new Sequence(alphabet, sequenceString2);
            ISequence seq3 = new Sequence(alphabet, sequenceString3);
            ISequence seq4 = new Sequence(alphabet, sequenceString4);
            ISequence seq5 = new Sequence(alphabet, sequenceString5);
            ISequence seq6 = new Sequence(alphabet, sequenceString6);
            ISequence seq7 = new Sequence(alphabet, sequenceString7);

            stage2ExpectedSequences = new List<ISequence>();
            stage2ExpectedSequences.Add(seq1);
            stage2ExpectedSequences.Add(seq2);
            stage2ExpectedSequences.Add(seq3);
            stage2ExpectedSequences.Add(seq4);
            stage2ExpectedSequences.Add(seq5);
            stage2ExpectedSequences.Add(seq6);
            stage2ExpectedSequences.Add(seq7);

            stage2ExpectedScore = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.Stage2ExpectedScoreNode);

            Console.WriteLine(String.Format(
              "PamsamBvtTest:: Initialization of stage2 variables successfully completed for xml node {0}",
              nodeName));
            ApplicationLog.WriteLine(String.Format(
              "PamsamBvtTest:: Initialization of stage2 variables successfully completed for xml node {0}",
              nodeName));
        }

        /// <summary>
        /// Validate Muscle multiple sequence alignment with different profiler and hierarchical clustering method name.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="moleculeType">molecule type of sequences</param>
        /// <param name="expectedScoreNode">Expected score node</param>
        /// <param name="hierarchicalClusteringMethodName">hierarchical clustering method name</param>
        /// <param name="profileName">SW/NW profiler</param>
        /// <param name="IsWeightedProduct">True if it of the WeightedProduct type else false.</param>
        void ValidatePamsamAlignWithUpdateDistanceMethodTypes(string nodeName,
          MoleculeType moleculeType, string expectedScoreNode,
          UpdateDistanceMethodsTypes hierarchicalClusteringMethodName,
          ProfileAlignerNames profileName, bool IsWeightedProduct)
        {
            ValidatePamsamAlign(nodeName, moleculeType, expectedScoreNode, hierarchicalClusteringMethodName,
              DistanceFunctionTypes.EuclideanDistance, profileName, ProfileScoreFunctionNames.InnerProduct,
               IsWeightedProduct);

            Console.WriteLine(String.Format(
              @"PamsamBvtTest:: Pamsam alignment validation completed successfully for {0} 
          moleculetype with different hierarchical clustering method name {1}",
              moleculeType.ToString(), hierarchicalClusteringMethodName.ToString()));
            ApplicationLog.WriteLine(String.Format(
              @"PamsamBvtTest:: Pamsam alignment validation completed successfully for 
          {0} moleculetype with different hierarchical clustering method name {1}",
              moleculeType.ToString(), hierarchicalClusteringMethodName.ToString()));
        }

        /// <summary>
        /// Validate Muscle multiple sequence alignment with different 
        /// profiler and distance matrix method name.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="moleculeType">molecule type of sequences</param>
        /// <param name="expectedScoreNode">Expected score node</param>
        /// <param name="distanceFunctionName">distance method name.</param>
        /// <param name="profileName">SW/NW profiler</param>
        /// <param name="IsWeightedProduct">True if it of the WeightedProduct type else false.</param>
        void ValidatePamsamAlignWithDistanceFunctionaTypes(string nodeName,
          MoleculeType moleculeType, string expectedScoreNode,
          DistanceFunctionTypes distanceFunctionName, ProfileAlignerNames profileName,
            bool IsWeightedProduct)
        {
            ValidatePamsamAlign(nodeName, moleculeType, expectedScoreNode,
              UpdateDistanceMethodsTypes.Average, distanceFunctionName,
              profileName, ProfileScoreFunctionNames.InnerProduct, IsWeightedProduct);

            Console.WriteLine(String.Format(
              @"PamsamBvtTest:: Pamsam alignment validation completed successfully for {0} 
          moleculetype with different kmer distance method name {1}",
              moleculeType.ToString(), distanceFunctionName.ToString()));
            ApplicationLog.WriteLine(String.Format(
              @"PamsamBvtTest:: Pamsam alignment validation completed successfully for {0} 
          moleculetype with different kmer distance method name {1}",
              moleculeType.ToString(), distanceFunctionName.ToString()));
        }
        /// <summary>
        /// Validate Muscle multiple sequence alignment with different profiler and 
        /// profile score function name.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="moleculeType">molecule type of sequences</param>
        /// <param name="expectedScoreNode">Expected score node</param>
        /// <param name="profileName">SW/NW profiler</param>
        /// <param name="profileScoreFunctionName">Profile score function name</param>
        /// <param name="IsWeightedProduct">True if it of the WeightedProduct type else false.</param>
        void ValidatePamsamAlignWithProfileScoreFunctionName(string nodeName,
          MoleculeType moleculeType, string expectedScoreNode,
          ProfileAlignerNames profileName, ProfileScoreFunctionNames profileScoreFunctionName,
            bool IsWeightedProduct)
        {
            ValidatePamsamAlign(nodeName, moleculeType, expectedScoreNode,
              UpdateDistanceMethodsTypes.Average,
              DistanceFunctionTypes.EuclideanDistance, profileName,
              profileScoreFunctionName, IsWeightedProduct);

            Console.WriteLine(String.Format(
              @"PamsamBvtTest:: Pamsam alignment validation completed successfully for {0} 
          moleculetype with different profile score function name {1}",
              moleculeType.ToString(), profileScoreFunctionName.ToString()));
            ApplicationLog.WriteLine(String.Format(
              @"PamsamBvtTest:: Pamsam alignment validation completed successfully for {0} 
          moleculetype with different profile score function name {1}",
              moleculeType.ToString(), profileScoreFunctionName.ToString()));
        }

        /// <summary>
        /// Validate Muscle multiple sequence alignment with default values.
        /// </summary>
        /// <param name="nodeName">Node Name</param>
        /// <param name="moleculeType">Molecule type</param>
        /// <param name="expectedScoreNode">Expected score node</param>
        /// <param name="profileName">Profile Name</param>
        /// <param name="IsWeightedProduct">True if it of the WeightedProduct type else false.</param>
        void ValidatePamsamAlign(string nodeName, MoleculeType moleculeType,
          string expectedScoreNode, ProfileAlignerNames profileName,
            bool IsWeightedProduct)
        {
            ValidatePamsamAlign(nodeName, moleculeType, expectedScoreNode,
              UpdateDistanceMethodsTypes.Average, DistanceFunctionTypes.EuclideanDistance,
              profileName, ProfileScoreFunctionNames.InnerProduct, IsWeightedProduct);

            Console.WriteLine(String.Format(
              "PamsamBvtTest:: Pamsam alignment validation completed successfully for {0} moleculetype with all default params",
              moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
              "PamsamBvtTest:: Pamsam alignment validation completed successfully for {0} moleculetype with all default params",
              moleculeType.ToString()));
        }

        /// <summary>
        /// Validate Muscle multiple sequence alignment.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">molecule type</param>
        /// <param name="expectedScoreNode">Expected score node</param>
        /// <param name="hierarchicalClusteringMethodName">hierarchical clustering method name</param>
        /// <param name="distanceFunctionName">kmerdistancematrix method name.</param>
        /// <param name="profileAlignerName">SW/NW profiler</param>
        /// <param name="profileScoreName">Profile score function name.</param>  
        /// <param name="IsWeightedProduct">True if it of the WeightedProduct type else false.</param>
        void ValidatePamsamAlign(string nodeName, MoleculeType moleculeType,
          string expectedScoreNode,
          UpdateDistanceMethodsTypes hierarchicalClusteringMethodName,
          DistanceFunctionTypes distanceFunctionName,
          ProfileAlignerNames profileAlignerName,
          ProfileScoreFunctionNames profileScoreName,
          bool IsWeightedProduct)
        {
            Initialize(nodeName, expectedScoreNode);

            // MSA aligned sequences.
            PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner(lstSequences,
              moleculeType, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
              profileAlignerName, profileScoreName,
              similarityMatrix, gapOpenPenalty, gapExtendPenalty, 2, 2);

            int index = 0;
            foreach (ISequence seq in msa.AlignedSequences)
            {
                if (IsWeightedProduct)
                {
                    Assert.AreEqual(seq.ToString(), expectedSequences[index].ToString());
                    index++;
                }
            }

            Assert.IsTrue(expectedScore.Contains(msa.AlignmentScore.ToString()));
        }

        /// <summary>
        /// Validate Stage 1 aligned sequences and score of Muscle multiple sequence alignment.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">molecule type</param>
        /// <param name="expectedScoreNode">Expected score node</param>
        /// <param name="hierarchicalClusteringMethodName">hierarchical clustering method name</param>
        /// <param name="distanceFunctionName">kmerdistancematrix method name.</param>
        /// <param name="profileAlignerName">SW/NW profiler</param>
        /// <param name="profileScoreName">Profile score function name.</param>  
        void ValidatePamsamAlignStage1(string nodeName, MoleculeType moleculeType,
          string expectedScoreNode, UpdateDistanceMethodsTypes hierarchicalClusteringMethodName,
          DistanceFunctionTypes distanceFunctionName,
          ProfileAlignerNames profileAlignerName,
          ProfileScoreFunctionNames profileScoreName)
        {
            Initialize(nodeName, expectedScoreNode);
            InitializeStage1Variables(nodeName);

            // MSA aligned sequences.
            PAMSAMMultipleSequenceAligner msa =
              new PAMSAMMultipleSequenceAligner(lstSequences, moleculeType, kmerLength,
                distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileScoreName, similarityMatrix, gapOpenPenalty,
                gapExtendPenalty, 2, 2);

            // Validate the aligned Sequence and score of stage1
            Assert.AreEqual(stage1ExpectedSequences.Count, msa.AlignedSequences.Count);
            int index = 0;
            foreach (ISequence seq in msa.AlignedSequencesA)
            {
                Assert.AreEqual(stage1ExpectedSequences[index].ToString(), seq.ToString());
                index++;
            }
            Assert.AreEqual(stage1ExpectedScore, msa.AlignmentScoreA.ToString());

            Console.WriteLine(String.Format(
               "PamsamBvtTest:: Pamsam stage1 alignment completed successfully for {0} moleculetype with all default params",
               moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
               "PamsamBvtTest:: Pamsam stage1 alignment completed successfully for {0} moleculetype with all default params",
               moleculeType.ToString()));
        }

        /// <summary>
        /// Validate Stage 2 aligned sequences and score of Muscle multiple sequence alignment.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">molecule type</param>
        /// <param name="expectedScoreNode">Expected score node</param>
        /// <param name="hierarchicalClusteringMethodName">hierarchical clustering method name</param>
        /// <param name="distanceFunctionName">kmerdistancematrix method name.</param>
        /// <param name="profileAlignerName">SW/NW profiler</param>
        /// <param name="profileScoreName">Profile score function name.</param>  
        void ValidatePamsamAlignStage2(string nodeName, MoleculeType moleculeType,
          string expectedScoreNode,
          UpdateDistanceMethodsTypes hierarchicalClusteringMethodName,
          DistanceFunctionTypes distanceFunctionName,
          ProfileAlignerNames profileAlignerName,
          ProfileScoreFunctionNames profileScoreName)
        {
            Initialize(nodeName, expectedScoreNode);
            InitializeStage2Variables(nodeName);

            // MSA aligned sequences.
            PAMSAMMultipleSequenceAligner msa =
              new PAMSAMMultipleSequenceAligner(lstSequences, moleculeType,
                kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileScoreName, similarityMatrix, gapOpenPenalty,
                gapExtendPenalty, 2, 2);

            // Validate the aligned Sequence and score of stage2
            if (null != msa.AlignedSequencesB)
            {
                Assert.AreEqual(stage2ExpectedSequences.Count, msa.AlignedSequencesB.Count);
                int index = 0;
                foreach (ISequence seq in msa.AlignedSequencesB)
                {
                    Assert.AreEqual(stage2ExpectedSequences[index].ToString(), seq.ToString());
                    index++;
                }
                Assert.AreEqual(stage2ExpectedScore, msa.AlignmentScoreB.ToString());
            }

            Console.WriteLine(String.Format(
               "PamsamBvtTest:: Pamsam stage2 alignment completed successfully for {0} moleculetype with all default params",
               moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
               "PamsamBvtTest:: Pamsam stage2 alignment completed successfully for {0} moleculetype with all default params",
               moleculeType.ToString()));
        }

        /// <summary>
        /// Validate Stage 3 aligned sequences and score of Muscle multiple sequence alignment.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">molecule type</param>
        /// <param name="expectedScoreNode">Expected score node</param>
        /// <param name="hierarchicalClusteringMethodName">hierarchical clustering method name</param>
        /// <param name="distanceFunctionName">kmerdistancematrix method name.</param>
        /// <param name="profileAlignerName">SW/NW profiler</param>
        /// <param name="profileScoreName">Profile score function name.</param>  
        void ValidatePamsamAlignStage3(string nodeName, MoleculeType moleculeType,
          string expectedScoreNode, UpdateDistanceMethodsTypes hierarchicalClusteringMethodName,
          DistanceFunctionTypes distanceFunctionName,
          ProfileAlignerNames profileAlignerName,
          ProfileScoreFunctionNames profileScoreName)
        {
            Initialize(nodeName, expectedScoreNode);

            // MSA aligned sequences.
            PAMSAMMultipleSequenceAligner msa =
              new PAMSAMMultipleSequenceAligner(lstSequences, moleculeType,
                kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileScoreName, similarityMatrix, gapOpenPenalty,
                gapExtendPenalty, 2, 2);

            string expectedSeqString = string.Empty;
            foreach (ISequence seq in expectedSequences)
            {
                expectedSeqString += seq.ToString() + ",";
            }

            foreach (ISequence seq in msa.AlignedSequencesC)
            {
                Assert.IsTrue(expectedSeqString.Contains(seq.ToString()));
            }

            Console.WriteLine("Stage3 score :{0}", msa.AlignmentScoreC.ToString());
            Assert.IsTrue(expectedScore.Contains(msa.AlignmentScoreC.ToString()));

            Console.WriteLine(String.Format(
              "PamsamBvtTest:: Pamsam stage3 alignment completed successfully for {0} moleculetype with all default params",
              moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
              "PamsamBvtTest:: Pamsam stage3 alignment completed successfully for {0} moleculetype with all default params",
              moleculeType.ToString()));
        }

        /// <summary>
        /// Validate DistanceMatrix at stage1 using different DistanceFunction names.
        /// </summary>
        /// <param name="nodeName">Xml node name</param>
        /// <param name="kmrlength">Kmer length</param>
        /// <param name="moleculeType">Molecule type</param>
        /// <param name="distanceFunctionName">Distance method name</param>
        void ValidateKmerDistanceMatrixStage1(string nodeName,
          int kmrlength, MoleculeType moleculeType,
          DistanceFunctionTypes distanceFunctionName)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            IDistanceMatrix matrix = GetKmerDistanceMatrix(kmrlength, moleculeType, distanceFunctionName);
            ValidateDistanceMatrix(nodeName, matrix);

            Console.WriteLine(String.Format(
              @"PamsamBvtTest:: kmer distance matrix generation and validation completed success for {0} 
          moleculetype with different distance method name {1}",
                                           moleculeType.ToString(),
                                           distanceFunctionName.ToString()));
            ApplicationLog.WriteLine(String.Format(
               @"PamsamBvtTest:: kmer distance matrix generation and validation completed success for {0} 
          moleculetype with different distance method name {1}",
                                           moleculeType.ToString(),
                                           distanceFunctionName.ToString()));
        }

        /// <summary>
        /// Validate Distance Matrix with default distancefunctionname
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="kmrlength">kmr length</param>
        /// <param name="moleculeType">molecule type.</param>
        void ValidateKmerDistanceMatrixStage1(string nodeName,
          int kmrlength, MoleculeType moleculeType)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            IDistanceMatrix matrix = GetKmerDistanceMatrix(kmrlength, moleculeType);
            ValidateDistanceMatrix(nodeName, matrix);

            Console.WriteLine(String.Format(
               @"PamsamBvtTest:: kmer distance matrix generation and validation completed success for {0} 
          moleculetype with default params",
                                 moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
              @"PamsamBvtTest:: kmer distance matrix generation and validation completed success for {0} 
          moleculetype with default params",
                                 moleculeType.ToString()));
        }

        /// <summary>
        /// Validate Distance Matrix
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="matrix">distance matrix</param>
        void ValidateDistanceMatrix(string nodeName, IDistanceMatrix matrix)
        {
            // Read expected values from config
            string expectedDimension = Utility._xmlUtil.GetTextValue(nodeName, Constants.Dimension);
            string expectedMinimumValue = Utility._xmlUtil.GetTextValue(nodeName, Constants.MinimumValue);
            string expectedNearestDistances = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.NearestDistances);

            // Validate values in distance matrix
            Assert.AreEqual(expectedDimension, matrix.Dimension.ToString());
            Assert.IsTrue(expectedMinimumValue.Contains(matrix.MinimumValue.ToString()));

            for (int idist = 0; idist < matrix.NearestDistances.Length; idist++)
            {
                Assert.IsTrue(expectedNearestDistances.Contains(
                    matrix.NearestDistances[idist].ToString()));
            }
        }

        /// <summary>
        /// Validate Hierarchical Clustering for stage1
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="kmrlength">kmer length for distance matrix</param>
        /// <param name="moleculeType">molecule type of input sequences</param>
        /// <param name="hierarchicalMethoName">hierarchical method name.</param>
        void ValidateHierarchicalClusteringStage1(string nodeName,
          int kmrlength, MoleculeType moleculeType,
          UpdateDistanceMethodsTypes hierarchicalMethoName)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            IDistanceMatrix matrix = GetKmerDistanceMatrix(kmrlength, moleculeType);
            IHierarchicalClustering hierarcicalClustering =
              GetHierarchicalClustering(matrix, hierarchicalMethoName);

            ValidateHierarchicalClustering(nodeName, hierarcicalClustering.Nodes,
              hierarcicalClustering.Edges);

            Console.WriteLine(String.Format(
              @"PamsamBvtTest:: herarchical clustering stage1 nodes and edges generation and 
          validation completed success for {0} moleculetype with different 
          hierarchical clustering method name {1}",
                                    moleculeType.ToString(),
                                    hierarchicalMethoName.ToString()));
            ApplicationLog.WriteLine(String.Format(
               @"PamsamBvtTest:: herarchical clustering stage1 nodes and edges generation and 
          validation completed success for {0} moleculetype with different 
          hierarchical clustering method name {1}",
                                    moleculeType.ToString(),
                                    hierarchicalMethoName.ToString()));
        }

        /// <summary>
        /// Validate Hierarchical Clustering for stage2 using kimura distance matrix and other default params.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">molecule type of sequences</param>
        void ValidateHierarchicalClusteringStage2(string nodeName, MoleculeType moleculeType)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            List<ISequence> stage1AlignedSequences = GetStage1AlignedSequence(moleculeType);
            IDistanceMatrix matrix = GetKimuraDistanceMatrix(stage1AlignedSequences);
            IHierarchicalClustering hierarcicalClustering = GetHierarchicalClustering(matrix);

            ValidateHierarchicalClustering(nodeName, hierarcicalClustering.Nodes,
              hierarcicalClustering.Edges);

            Console.WriteLine(String.Format(
              @"PamsamBvtTest:: herarchical clustering stage2 nodes and edges generation and 
          validation completed success for {0} moleculetype with default params",
                                                   moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
               @"PamsamBvtTest:: herarchical clustering stage2 nodes and edges generation and 
          validation completed success for {0} moleculetype with default params",
                                                   moleculeType.ToString()));
        }

        /// <summary>
        /// Validate Hierarchical Clustering for stage1 using kmer distance matrix
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">molecule type of sequences</param>
        /// <param name="kmrlength">kmer length to generate distance matrix</param>
        void ValidateHierarchicalClusteringStage1(string nodeName, int kmrlength, MoleculeType moleculeType)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            IDistanceMatrix matrix = GetKmerDistanceMatrix(kmrlength, moleculeType);
            IHierarchicalClustering hierarcicalClustering = GetHierarchicalClustering(matrix);
            ValidateHierarchicalClustering(nodeName, hierarcicalClustering.Nodes,
              hierarcicalClustering.Edges);

            Console.WriteLine(String.Format(
              @"PamsamBvtTest:: herarchical clustering stage1 nodes and edges generation and 
          validation completed success for {0} moleculetype with default params",
                moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
               @"PamsamBvtTest:: herarchical clustering stage1 nodes and edges generation and 
          validation completed success for {0} moleculetype with default params",
                moleculeType.ToString()));
        }

        /// <summary>
        /// Validate Hierarchical Clustering for stage2 using kimura distance matrix and hierarchical method name
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">molecule type of sequences</param>
        /// <param name="hierarchicalMethoName">hierarchical method name</param>
        void ValidateHierarchicalClusteringStage2(string nodeName, MoleculeType moleculeType,
          UpdateDistanceMethodsTypes hierarchicalMethodName)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            List<ISequence> stage1AlignedSequences = GetStage1AlignedSequence(moleculeType);
            IDistanceMatrix matrix = GetKimuraDistanceMatrix(stage1AlignedSequences);

            IHierarchicalClustering hierarcicalClustering = GetHierarchicalClustering(matrix,
              hierarchicalMethodName);

            ValidateHierarchicalClustering(nodeName, hierarcicalClustering.Nodes,
              hierarcicalClustering.Edges);

            Console.WriteLine(String.Format(
                @"PamsamBvtTest:: herarchical clustering stage2 nodes and edges generation and 
          validation completed success for {0} moleculetype with different 
          hierarchical clustering method name {1}",
                                    moleculeType.ToString(),
                                    hierarchicalMethodName.ToString()));
            ApplicationLog.WriteLine(String.Format(
               @"PamsamBvtTest:: herarchical clustering stage2 nodes and edges generation and 
          validation completed success for {0} moleculetype with different 
          hierarchical clustering method name {1}",
                                    moleculeType.ToString(),
                                    hierarchicalMethodName.ToString()));
        }

        /// <summary>
        /// Validate the nodes and edges of hierarchical clustering object.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="nodes">binary tree nodes</param>
        /// <param name="edges">binary tree edges</param>
        void ValidateHierarchicalClustering(string nodeName,
          List<BinaryGuideTreeNode> nodes, List<BinaryGuideTreeEdge> edges)
        {
            // Validate the nodes and edges.
            string expectedEdgeCount = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.EdgesCount);
            string expectedNodesLeftChild = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.NodesLeftChild);
            string expectedNodesRightChild = Utility._xmlUtil.GetTextValue(nodeName,
              Constants.NodesRightChild);
            string expectednode = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.Nodes);

            foreach (BinaryGuideTreeNode node in nodes)
            {
                Assert.IsTrue(expectednode.Contains(node.ID.ToString()));
                if (node.LeftChildren != null)
                {
                    Console.WriteLine("Left child:{0}\n", node.LeftChildren.ID.ToString());
                    Assert.IsTrue(expectedNodesLeftChild.Contains(
                        node.LeftChildren.ID.ToString()));
                }
                if (node.RightChildren != null)
                {
                    Console.WriteLine("Right child:{0}\n", node.RightChildren.ID.ToString());
                    Assert.IsTrue(expectedNodesRightChild.Contains(
                        node.RightChildren.ID.ToString()));
                }
            }
            Assert.AreEqual(expectedEdgeCount, edges.Count.ToString());
        }

        /// <summary>
        /// Validate the binary tree leaves, root using unaligned sequences.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="kmrlength">kmer length to generate distance matrix</param>
        /// <param name="moleculeType">molecule type of sequences</param>
        void ValidateBinaryTreeStage1(string nodeName, int kmrLength, MoleculeType moleculeType)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            IDistanceMatrix matrix = GetKmerDistanceMatrix(kmrLength, moleculeType);

            IHierarchicalClustering hierarcicalClustering = GetHierarchicalClustering(matrix);

            BinaryGuideTree binaryTree = GetBinaryTree(hierarcicalClustering);

            ValidateBinaryTree(binaryTree, nodeName);

            Console.WriteLine(String.Format(
              @"PamsamBvtTest:: Binary tree stage1 root and leaves generation and 
          validation completed success for {0} moleculetype with default params",
                                                                                moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
               @"PamsamBvtTest:: Binary Tree stage1 root and leaves generation and 
          validation completed success for {0} moleculetype with default params",
                                                                                moleculeType.ToString()));
        }

        /// <summary>
        /// Validate the binary tree leaves, root using stage1 aligned sequences.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">molecule type of sequences</param>
        void ValidateBinaryTreeStage2(string nodeName, MoleculeType moleculeType)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            List<ISequence> stage1AlignedSequences = GetStage1AlignedSequence(moleculeType);

            IDistanceMatrix matrix = GetKimuraDistanceMatrix(stage1AlignedSequences);

            IHierarchicalClustering hierarcicalClustering = GetHierarchicalClustering(matrix);

            BinaryGuideTree binaryTree = GetBinaryTree(hierarcicalClustering);

            ValidateBinaryTree(binaryTree, nodeName);

            Console.WriteLine(String.Format(
              @"PamsamBvtTest:: Binary tree stage2 root and leaves generation and 
          validation completed success for {0} moleculetype with default params",
                                                                                moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
                @"PamsamBvtTest:: Binary Tree stage2 root and leaves generation and 
          validation completed success for {0} moleculetype with default params",
                                                                                moleculeType.ToString()));
        }

        /// <summary>
        /// Validate SeparateSequencesByCutTree() method of Binary tree by cutting the tree at an edge.
        /// </summary>
        /// <param name="edgeIndex">edge index to cut the tree</param>
        /// <param name="nodeName">xml node name</param>
        /// <param name="kmrLength">kmerlength to get distance matrix.</param>
        /// <param name="moleculeType">molecule type of sequences</param>
        void ValidateBinaryTreeSeparateSequencesByCutTree(int edgeIndex, string nodeName,
            int kmrLength, MoleculeType moleculeType)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            IDistanceMatrix matrix = GetKmerDistanceMatrix(kmrLength, moleculeType);
            IHierarchicalClustering hierarcicalClustering = GetHierarchicalClustering(matrix);

            BinaryGuideTree binaryTree = GetBinaryTree(hierarcicalClustering);

            List<int>[] sequences = binaryTree.SeparateSequencesByCuttingTree(edgeIndex);
            string seqIndicesString = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SequenceIndicesWithCutTree);

            string[] seqIndices = seqIndicesString.Split(',');
            int counter = 0;
            for (int index = 0; index < sequences.Length; index++)
            {
                for (int seqIndex = 0; seqIndex < sequences[index].Count; seqIndex++)
                {
                    Assert.AreEqual(sequences[index][seqIndex].ToString(), seqIndices[counter]);
                    counter++;
                }
            }

            Console.WriteLine(String.Format(
                @"PamsamBvtTest::Validate binary tree by cutting tree at an edge index {0}
          and validation of nodes and edges completed successfully for {1} moleculetype",
                                                                                        edgeIndex,
                                                                                        moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
               @"PamsamBvtTest::Validate binary tree by cutting tree at an edge index {0}
          and validation of nodes and edges completed successfully for {1} moleculetype",
                                                                                        edgeIndex,
                                                                                        moleculeType.ToString()));
        }

        /// <summary>
        /// Validate the leaves and root of binary tree
        /// </summary>
        /// <param name="binaryTree">binary tree object.</param>
        /// <param name="nodeName">xml node name.</param>
        void ValidateBinaryTree(BinaryGuideTree binaryTree, string nodeName)
        {
            string rootId = Utility._xmlUtil.GetTextValue(nodeName, Constants.RootId);
            string leavesCount = Utility._xmlUtil.GetTextValue(nodeName, Constants.LeavesCount);
            string expectedNodesLeftChild = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.NodesLeftChild);
            string expectedNodesRightChild = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.NodesRightChild);
            string expectenode = Utility._xmlUtil.GetTextValue(nodeName, Constants.Nodes);
            string[] expectedNodes = expectenode.Split(',');

            Assert.IsTrue(rootId.Contains(binaryTree.Root.ID.ToString()));
            Assert.IsTrue(leavesCount.Contains(binaryTree.NumberOfLeaves.ToString()));
            int index = 0;
            foreach (BinaryGuideTreeNode node in binaryTree.Nodes)
            {
                Console.WriteLine("Node:{0}\n", node.ID.ToString());
                Assert.AreEqual(expectedNodes[index], node.ID.ToString());
                if (node.LeftChildren != null)
                {
                    Console.WriteLine("Lef child :{0}\n", node.LeftChildren.ID.ToString());
                    Assert.IsTrue(expectedNodesLeftChild.Contains(node.LeftChildren.ID.ToString()));

                }
                if (node.RightChildren != null)
                {
                    Console.WriteLine("Right child :{0}\n", node.RightChildren.ID.ToString());
                    Assert.IsTrue(expectedNodesRightChild.Contains(node.RightChildren.ID.ToString()));
                }
                index++;
            }
        }

        /// <summary>
        /// Validate Progressive Alignment of Stage 1
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="moleculeType">Molecule Type.</param>
        void ValidateProgressiveAlignmentStage1(string nodeName, MoleculeType moleculeType)
        {
            Initialize(nodeName, Constants.ExpectedScoreNode);
            InitializeStage1Variables(nodeName);

            IDistanceMatrix matrix = GetKmerDistanceMatrix(kmerLength, moleculeType);
            IHierarchicalClustering hierarcicalClustering = GetHierarchicalClustering(matrix);
            BinaryGuideTree binaryTree = GetBinaryTree(hierarcicalClustering);

            List<ISequence> alignedSequences = GetProgressiveAlignerAlignedSequences(lstSequences,
                binaryTree);

            // Validate the aligned Sequence of stage1
            Assert.AreEqual(stage1ExpectedSequences.Count, alignedSequences.Count);
            int index = 0;
            foreach (ISequence seq in alignedSequences)
            {
                Assert.AreEqual(stage1ExpectedSequences[index].ToString(), seq.ToString());
                index++;
            }

            Console.WriteLine(String.Format(
                @"PamsamBvtTest:: Validation and generation of stage1 aligned sequences
          using progressivealignment completed successfully for moleculetype {0}",
                                                                                 moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
               @"PamsamBvtTest:: Validation and generation of stage1 aligned sequences
          using progressivealignment completed successfully for moleculetype {0}",
                                                                                 moleculeType.ToString()));
        }

        /// <summary>
        /// Validate the Profile Aligner GenerateEString() method using profiles of sub trees.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">Molecule Type</param>
        /// <param name="edgeIndex">Edge index to cut tree.</param>
        void ValidateProfileAlignerGenerateEString(string nodeName,
            MoleculeType moleculeType, int edgeIndex)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            InitializeStage2Variables(Constants.MuscleDnaSequenceNode);

            // Get Stage2 Binary Tree
            List<ISequence> stage1AlignedSequences = GetStage1AlignedSequence(moleculeType);
            IDistanceMatrix matrix = GetKimuraDistanceMatrix(stage1AlignedSequences);
            IHierarchicalClustering hierarcicalClustering = GetHierarchicalClustering(matrix);
            BinaryGuideTree binaryTree = GetBinaryTree(hierarcicalClustering);

            // Get profiles
            GetProfiles(edgeIndex, binaryTree);

            // Get id's of edges and root using two profiles
            List<int> eStringSubtreeEdge = profileAligner.GenerateEString(profileAligner.AlignedA);
            List<int> eStringSubtreeRoot = profileAligner.GenerateEString(profileAligner.AlignedB);

            string expectedTreeEdges = Utility._xmlUtil.GetTextValue(nodeName, Constants.SubTreeEdges);
            string expectedTreeRoot = Utility._xmlUtil.GetTextValue(nodeName, Constants.SubTreeRoots);

            string[] expectededges = expectedTreeEdges.Split(',');
            for (int index = 0; index < eStringSubtreeEdge.Count; index++)
            {
                Assert.AreEqual(eStringSubtreeEdge[index].ToString(), expectededges[index]);
            }

            Assert.AreEqual(eStringSubtreeRoot[0].ToString(), expectedTreeRoot);

            Console.WriteLine(String.Format(
                @"PamsamBvtTest:: Validation and generation of subtrees roots and edges
          using profile aligner GenerateEString() completed successfully for moleculetype{0}",
                                                                                             moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
               @"PamsamBvtTest:: Validation and generation of subtrees roots and edges
          using profile aligner GenerateEString() completed successfully for moleculetype{0}",
                                                                                             moleculeType.ToString()));
        }

        /// <summary>
        /// Validate the Profile Aligner GenerateSequenceString() method using profiles of sub trees.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">Molecule Type</param>
        /// <param name="edgeIndex">Edge index to cut tree.</param>
        void ValidateProfileAlignerGenerateSequenceString(string nodeName,
            MoleculeType moleculeType, int edgeIndex)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            InitializeStage2Variables(Constants.MuscleDnaSequenceNode);

            // Get Stage2 Binary Tree
            List<ISequence> stage1AlignedSequences = GetStage1AlignedSequence(moleculeType);
            IDistanceMatrix matrix = GetKimuraDistanceMatrix(stage1AlignedSequences);
            IHierarchicalClustering hierarcicalClustering = GetHierarchicalClustering(matrix);
            BinaryGuideTree binaryTree = GetBinaryTree(hierarcicalClustering);

            GetProfiles(edgeIndex, binaryTree);

            // Get id's of edges and root using two profiles
            List<int> eStringSubtreeEdge = profileAligner.GenerateEString(profileAligner.AlignedA);

            string expectedSequence = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.GenerateSequenceString);

            ISequence sequence = profileAligner.GenerateSequenceFromEString(eStringSubtreeEdge,
                lstSequences[0]);
            Assert.AreEqual(sequence.ToString(), expectedSequence);

            Console.WriteLine(String.Format(
                @"PamsamBvtTest:: Validation and generation of subtrees sequences
          using profile aligner GenerateSequenceFromEString() completed successfully for moleculetype{0}",
               moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
               @"PamsamBvtTest:: Validation and generation of subtrees sequences
          using profile aligner GenerateSequenceFromEString() completed successfully for moleculetype{0}",
                moleculeType.ToString()));
        }

        /// <summary>
        /// Validate the kimura distance matrix using stage 1 aligned sequences.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="moleculeType">Molecule Type.</param>
        void ValidateKimuraDistanceMatrix(string nodeName, MoleculeType moleculeType)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            List<ISequence> stage1AlignedSequences = GetStage1AlignedSequence(moleculeType);

            IDistanceMatrix matrix = GetKimuraDistanceMatrix(stage1AlignedSequences);
            ValidateDistanceMatrix(nodeName, matrix);

            Console.WriteLine(String.Format(
                @"PamsamBvtTest:: kimura distance matrix generation and validation completed success for {0} 
         moleculetype with default params",
               moleculeType.ToString()));
            ApplicationLog.WriteLine(String.Format(
                @"PamsamBvtTest:: kimura distance matrix generation and validation completed success for {0} 
          moleculetype with default params",
                moleculeType.ToString()));
        }

        /// <summary>
        /// Creates binarytree using stage1 sequences and
        /// cut the binary tree at an random edge to get two profiles.
        /// Create NeedlemanWunschProfileAlignerSerial\Parallel instance 
        /// according to degree of parallelism
        /// and execute AlignSimple\Align() method using two profiles.
        /// Validate the IProfileAlignment properties.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">Molecule Type</param>
        /// <param name="degreeOfParallelism">if 1 it is serial Profiler else parallel profiler</param>
        /// <param name="edgeIndex">edge index to cut the tree</param>
        /// <param name="overloadType">Execute Align()\AlignSimple()</param>
        void ValidateProfileAlignerAlign(string nodeName, MoleculeType moleculeType,
          int degreeOfParallelism, int edgeIndex, AlignType overloadType)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            InitializeStage2Variables(Constants.MuscleDnaSequenceNode);

            // Get Stage2 Binary Tree
            List<ISequence> stage1AlignedSequences = GetStage1AlignedSequence(moleculeType);
            IDistanceMatrix matrix = GetKimuraDistanceMatrix(stage1AlignedSequences);
            IHierarchicalClustering hierarcicalClustering = GetHierarchicalClustering(matrix);
            BinaryGuideTree binaryTree = GetBinaryTree(hierarcicalClustering);

            // Cut Tree at an edge and get sequences.
            List<int>[] leafNodeIndices = binaryTree.SeparateSequencesByCuttingTree(edgeIndex);

            // Extract profiles 
            List<int>[] removedPositions = null;
            IProfileAlignment[] separatedProfileAlignments = ProfileAlignment.ProfileExtraction(
                stage2ExpectedSequences, leafNodeIndices[0], leafNodeIndices[1], out removedPositions);

            // Align it
            IProfileAlignment alignedProfile = null;
            if (1 == degreeOfParallelism)
            {
                NeedlemanWunschProfileAlignerSerial nwprofileAligner =
                    new NeedlemanWunschProfileAlignerSerial(similarityMatrix,
                        ProfileScoreFunctionNames.InnerProductFast,
                        gapOpenPenalty, gapExtendPenalty, 2);
                switch (overloadType)
                {
                    case AlignType.AlignSimpleAllParams:
                        alignedProfile = nwprofileAligner.AlignSimple(similarityMatrix,
                            gapOpenPenalty, separatedProfileAlignments[0], separatedProfileAlignments[1]);
                        break;
                    case AlignType.AlignSimpleOnlyProfiles:
                        alignedProfile = nwprofileAligner.AlignSimple(separatedProfileAlignments[0],
                            separatedProfileAlignments[1]);
                        break;
                    case AlignType.AlignAllParams:
                        alignedProfile = nwprofileAligner.Align(similarityMatrix, gapOpenPenalty,
                            gapExtendPenalty, separatedProfileAlignments[0], separatedProfileAlignments[1]);
                        break;
                }

            }
            else
            {
                if (Environment.ProcessorCount >= degreeOfParallelism)
                {
                    NeedlemanWunschProfileAlignerParallel nwprofileAlignerParallel =
                        new NeedlemanWunschProfileAlignerParallel(
                            similarityMatrix,
                            ProfileScoreFunctionNames.InnerProductFast,
                            gapOpenPenalty,
                            gapExtendPenalty, 2);

                    alignedProfile = nwprofileAlignerParallel.AlignSimple(
                        separatedProfileAlignments[0],
                        separatedProfileAlignments[1]);

                    switch (overloadType)
                    {
                        case AlignType.AlignSimpleAllParams:
                            alignedProfile = nwprofileAlignerParallel.AlignSimple(similarityMatrix,
                                gapOpenPenalty, separatedProfileAlignments[0], separatedProfileAlignments[1]);
                            break;
                        case AlignType.AlignSimpleOnlyProfiles:
                            alignedProfile = nwprofileAlignerParallel.AlignSimple(
                                separatedProfileAlignments[0],
                                separatedProfileAlignments[1]);
                            break;
                        case AlignType.AlignAllParams:
                            alignedProfile = nwprofileAlignerParallel.Align(
                                similarityMatrix, gapOpenPenalty,
                                gapExtendPenalty, separatedProfileAlignments[0],
                                separatedProfileAlignments[1]);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(String.Format(@"PamsamBvtTest: NeedlemanWunschProfileAlignerParallel 
                        could not be instantiated as number of processor is {0} and degree of parallelism {1}",
                                                                                                              Environment.ProcessorCount.ToString(), degreeOfParallelism));
                    ApplicationLog.WriteLine(String.Format(@"PamsamBvtTest: NeedlemanWunschProfileAlignerParallel could not be instantiated
                          as number of processor is {0} and degree of parallelism {1}",
                                                                                      Environment.ProcessorCount.ToString(), degreeOfParallelism));

                }
            }

            if (null != alignedProfile)
            {
                // Validate profile alignement 
                string expectedRowSize = Utility._xmlUtil.GetTextValue(nodeName, Constants.RowSize);
                string expectedColSize = Utility._xmlUtil.GetTextValue(nodeName, Constants.ColumnSize);
                Assert.AreEqual(expectedColSize, alignedProfile.ProfilesMatrix.ColumnSize.ToString());
                Assert.AreEqual(expectedRowSize, alignedProfile.ProfilesMatrix.RowSize.ToString());

                Console.WriteLine(String.Format(@"PamsamBvtTest: {0} {1} method validation completed successfully with
                        number of processor is {2} and degree of parallelism {3} for molecule type {4}",
                                     profileAligner.ToString(),
                                     overloadType,
                                     Environment.ProcessorCount.ToString(),
                                     degreeOfParallelism,
                                     moleculeType));
                ApplicationLog.WriteLine(String.Format(@"PamsamBvtTest: {0} {1} method validation completed successfully with
                        number of processor is {2} and degree of parallelism {3} for molecule type {4}",
                                     profileAligner.ToString(),
                                     overloadType,
                                     Environment.ProcessorCount.ToString(),
                                     degreeOfParallelism,
                                     moleculeType));
            }
            else
            {
                Assert.Fail("Profile Aligner is not instantiated");
            }
        }

        /// <summary>
        /// Creates binarytree using stage1 sequences and 
        /// cut the binary tree at an random edge to get two profiles.
        /// Create NeedlemanWunschProfileAlignerSerial\Parallel instance 
        /// according to degree of parallelism 
        /// and using profile function score . Execute Align() method.
        /// Validates the IProfileAlignment properties.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="degreeOfParallelism">if 1 it is serial Profiler else parallel profiler</param>
        /// <param name="edgeIndex">edge index to cut the tree</param>
        /// <param name="profileFunction">profile function score name</param>
        /// <param name="moleculeType">Molecule Type</param>
        void ValidateProfileAlignerAlignWithProfileFunctionScore(
            string nodeName, int degreeOfParallelism, ProfileScoreFunctionNames profileFunction,
            int edgeIndex,
            MoleculeType moleculeType)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            InitializeStage2Variables(Constants.MuscleDnaSequenceNode);

            // Get Stage2 Binary Tree
            List<ISequence> stage1AlignedSequences = GetStage1AlignedSequence(moleculeType);
            IDistanceMatrix matrix = GetKimuraDistanceMatrix(stage1AlignedSequences);
            IHierarchicalClustering hierarcicalClustering = GetHierarchicalClustering(matrix);
            BinaryGuideTree binaryTree = GetBinaryTree(hierarcicalClustering);

            // Cut Tree at an edge and get sequences.
            List<int>[] leafNodeIndices = binaryTree.SeparateSequencesByCuttingTree(edgeIndex);

            // Extract profiles 
            List<int>[] removedPositions = null;
            IProfileAlignment[] separatedProfileAlignments = ProfileAlignment.ProfileExtraction(
                stage2ExpectedSequences, leafNodeIndices[0], leafNodeIndices[1], out removedPositions);

            IProfileAligner aligner = null;
            if (1 == degreeOfParallelism)
            {
                aligner = new NeedlemanWunschProfileAlignerSerial(similarityMatrix,
                    profileFunction, gapOpenPenalty, gapExtendPenalty, 2);
            }
            else
            {
                if (Environment.ProcessorCount >= degreeOfParallelism)
                {
                    aligner = new NeedlemanWunschProfileAlignerParallel(similarityMatrix,
                        profileFunction, gapOpenPenalty, gapExtendPenalty, 2);
                }
                else
                {
                    Console.WriteLine(String.Format(@"PamsamBvtTest: NeedlemanWunschProfileAlignerParallel could not be instantiated
                          as number of processor is {0} and degree of parallelism {1}",
                                                                                      Environment.ProcessorCount.ToString(), degreeOfParallelism));
                    ApplicationLog.WriteLine(String.Format(@"PamsamBvtTest: NeedlemanWunschProfileAlignerParallel could not be instantiated
                          as number of processor is {0} and degree of parallelism {1}",
                                                                                      Environment.ProcessorCount.ToString(), degreeOfParallelism));

                }
            }

            if (null != aligner)
            {
                IProfileAlignment profileAlignment = aligner.Align(separatedProfileAlignments[0],
                  separatedProfileAlignments[0]);

                // Validate profile alignement 
                string expectedRowSize = Utility._xmlUtil.GetTextValue(nodeName, Constants.RowSize);
                string expectedColSize = Utility._xmlUtil.GetTextValue(nodeName, Constants.ColumnSize);
                Console.WriteLine("Row Size :{0}", profileAlignment.ProfilesMatrix.RowSize.ToString());
                Console.WriteLine("Column Size :{0}", profileAlignment.ProfilesMatrix.ColumnSize.ToString());
                Assert.IsTrue(expectedColSize.Contains(profileAlignment.ProfilesMatrix.ColumnSize.ToString()));
                Assert.IsTrue(expectedRowSize.Contains(profileAlignment.ProfilesMatrix.RowSize.ToString()));
                Console.WriteLine(String.Format(@"PamsamBvtTest: {0} Align() method validation completed successfully with
                        number of processor is {1} and degree of parallelism {2} for molecule type {3}",
                                      profileAligner.ToString(),
                                      Environment.ProcessorCount.ToString(),
                                      degreeOfParallelism,
                                      moleculeType));
                ApplicationLog.WriteLine(String.Format(@"PamsamBvtTest: {0} Align() method validation completed successfully with
                        number of processor is {1} and degree of parallelism {2} for molecule type {3}",
                                profileAligner.ToString(),
                                Environment.ProcessorCount.ToString(),
                                degreeOfParallelism,
                                moleculeType));
            }
            else
            {
                Assert.Fail("Profile Aligner is not instantiated");
            }
        }

        /// <summary>
        /// Get Hierarchical Clustering using kmerdistancematrix\kimura distance matrix and hierarchical method name.
        /// </summary>
        /// <param name="distanceMatrix">distance matrix.</param>
        /// <param name="hierarchicalClusteringMethodName">Hierarchical clustering method name.</param>
        /// <returns>Hierarchical clustering</returns>
        IHierarchicalClustering GetHierarchicalClustering(IDistanceMatrix distanceMatrix,
          UpdateDistanceMethodsTypes hierarchicalClusteringMethodName)
        {
            // Hierarchical clustering
            IHierarchicalClustering hierarcicalClustering =
                new HierarchicalClusteringParallel(distanceMatrix, hierarchicalClusteringMethodName);

            return hierarcicalClustering;
        }

        /// <summary>
        /// Get Hierarchical Clustering using kmerdistancematrix\kimura distance matrix.
        /// </summary>
        /// <param name="distanceMatrix"></param>
        /// <param name="hierarchicalClusteringMethodName"></param>
        /// <returns>Hierarchical clustering</returns>
        IHierarchicalClustering GetHierarchicalClustering(IDistanceMatrix distanceMatrix)
        {
            // Hierarchical clustering with default distance method name
            IHierarchicalClustering hierarcicalClustering =
                new HierarchicalClusteringParallel(distanceMatrix);

            return hierarcicalClustering;
        }

        /// <summary>
        /// Get distance matrix with distance function name
        /// </summary>
        /// <param name="kmrlength">kmr length</param>
        /// <param name="moleculeType">Molecule Type</param>
        /// <param name="distanceFunctionName">distance matrix function name.</param>
        /// <returns>Distance matrix</returns>
        IDistanceMatrix GetKmerDistanceMatrix(int kmrlength,
            MoleculeType moleculeType, DistanceFunctionTypes distanceFunctionName)
        {

            // Generate DistanceMatrix
            KmerDistanceMatrixGenerator kmerDistanceMatrixGenerator =
                new KmerDistanceMatrixGenerator(lstSequences, kmrlength,
                    moleculeType, distanceFunctionName);

            return kmerDistanceMatrixGenerator.DistanceMatrix;
        }

        /// <summary>
        /// Get distance matrix with default distance function name
        /// </summary>
        /// <param name="kmrlength">kmr length</param>
        /// <param name="moleculeType">Molecule Type</param>
        /// <returns>Distance matrix</returns>
        IDistanceMatrix GetKmerDistanceMatrix(int kmrlength, MoleculeType moleculeType)
        {

            // Generate DistanceMatrix
            KmerDistanceMatrixGenerator kmerDistanceMatrixGenerator =
              new KmerDistanceMatrixGenerator(lstSequences, kmrlength, moleculeType);


            return kmerDistanceMatrixGenerator.DistanceMatrix;
        }

        /// <summary>
        /// Get kimura distanc matrix using stage1 aligned sequences
        /// </summary>
        /// <param name="alignedSequences">aligned Sequences of stage 1</param>
        /// <returns>Distance matrix</returns>
        IDistanceMatrix GetKimuraDistanceMatrix(List<ISequence> alignedSequences)
        {
            // Generate DistanceMatrix from Multiple Sequence Alignment
            KimuraDistanceMatrixGenerator kimuraDistanceMatrixGenerator =
                new KimuraDistanceMatrixGenerator();
            kimuraDistanceMatrixGenerator.GenerateDistanceMatrix(alignedSequences);

            return kimuraDistanceMatrixGenerator.DistanceMatrix;
        }

        /// <summary>
        /// Get the binary tree object using hierarchical clustering object
        /// </summary>
        /// <param name="hierarchicalClustering">hierarchical Clustering</param>
        /// <returns>Binary guide tree</returns>
        BinaryGuideTree GetBinaryTree(IHierarchicalClustering hierarchicalClustering)
        {
            // Generate Guide Tree
            BinaryGuideTree binaryGuideTree =
              new BinaryGuideTree(hierarchicalClustering);

            return binaryGuideTree;
        }

        /// <summary>
        /// Get the aligned sequence for stage1
        /// </summary>
        /// <param name="moleculeType">Molecule Type</param>
        /// <returns>Sequence list</returns>
        List<ISequence> GetStage1AlignedSequence(MoleculeType moleculeType)
        {

            // MSA aligned sequences.
            PAMSAMMultipleSequenceAligner msa =
                new PAMSAMMultipleSequenceAligner(lstSequences,
                    moleculeType, kmerLength, DistanceFunctionTypes.EuclideanDistance,
                    UpdateDistanceMethodsTypes.Average,
                    ProfileAlignerNames.NeedlemanWunschProfileAligner,
                    ProfileScoreFunctionNames.InnerProduct, similarityMatrix, gapOpenPenalty,
                    gapExtendPenalty, 2, 2);
            return msa.AlignedSequencesA;
        }

        /// <summary>
        /// Gets progressive aligner aligned sequences
        /// </summary>
        /// <param name="sequences">list of sequences</param>
        /// <param name="binaryGuidTree">binary guide tree</param>
        /// <returns>list of aligned sequences</returns>
        List<ISequence> GetProgressiveAlignerAlignedSequences(List<ISequence> sequences,
            BinaryGuideTree binaryGuidTree)
        {
            // Progressive Alignment
            IProgressiveAligner progressiveAligner = new ProgressiveAligner(profileAligner);
            progressiveAligner.Align(sequences, binaryGuidTree);

            return progressiveAligner.AlignedSequences;
        }

        /// <summary>
        /// Gets profiles for the give edge index and binary tree
        /// </summary>
        /// <param name="edgeIndex">Edge index</param>
        /// <param name="binaryTree">Binary Guide tree</param>
        void GetProfiles(int edgeIndex, BinaryGuideTree binaryTree)
        {
            // Cut Tree at an edge and get sequences.
            List<int>[] leafNodeIndices = binaryTree.SeparateSequencesByCuttingTree(edgeIndex);

            // Extract profiles and align it.
            List<int>[] removedPositions = null;
            IProfileAlignment[] separatedProfileAlignments =
                ProfileAlignment.ProfileExtraction(stage2ExpectedSequences,
                leafNodeIndices[0], leafNodeIndices[1], out removedPositions);

            profileAligner.Align(separatedProfileAlignments[0], separatedProfileAlignments[1]);
        }

        /// <summary>
        /// Validate different alignment score functions 
        /// using input sequences and reference sequences
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="type">Molecule Type</param>
        /// <param name="scoretype">Score Function Type.</param>
        void ValidateAlignmentScore(string nodeName, MoleculeType type, ScoreType scoretype)
        {
            string inputFilePath = Utility._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);
            string refFilePath = Utility._xmlUtil.GetTextValue(nodeName, Constants.RefFilePathNode);


            ISequenceParser parser = new FastaParser();
            IList<ISequence> sequences = parser.Parse(inputFilePath);
            IList<ISequence> refSequences = parser.Parse(refFilePath);

            List<ISequence> alignedSequences = GetPAMSAMAlignedSequences(type, sequences);

            // Validate the score
            switch (scoretype)
            {
                case ScoreType.QScore:
                    string expectedQScore = Utility._xmlUtil.GetTextValue(nodeName, Constants.QScoreNode);
                    float qScore = MsaUtils.CalculateAlignmentScoreQ(alignedSequences, refSequences);
                    Assert.AreEqual(expectedQScore, qScore.ToString());
                    break;
                case ScoreType.TCScore:
                    string expectedTCScore = Utility._xmlUtil.GetTextValue(nodeName, Constants.TCScoreNode);
                    float tcScore = MsaUtils.CalculateAlignmentScoreQ(alignedSequences, refSequences);
                    Assert.AreEqual(expectedTCScore, tcScore.ToString());
                    break;
                case ScoreType.Offset:
                    string expectedResiduesCount = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ResiduesCountNode);
                    List<int> positions = MsaUtils.CalculateOffset(alignedSequences[0], refSequences[0]);
                    int residuesCount = 0;
                    for (int i = 0; i < positions.Count; i++)
                    {
                        if (positions[i] < 0)
                        {
                            residuesCount++;
                        }
                    }
                    Assert.AreEqual(expectedResiduesCount, residuesCount.ToString());
                    break;
                case ScoreType.MultipleAlignmentScoreFunction:
                    string expectedScore = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedScoreNode);
                    float score = MsaUtils.MultipleAlignmentScoreFunction(
                      alignedSequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

                    Assert.IsTrue(expectedScore.Contains(score.ToString()));
                    break;
                case ScoreType.PairWiseScoreFunction:
                    string expectedPairwiseScore = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.PairWiseScoreNode);
                    float pairwiseScore = MsaUtils.PairWiseScoreFunction(
                      alignedSequences[0], alignedSequences[1], similarityMatrix,
                      gapOpenPenalty, gapExtendPenalty);
                    Assert.AreEqual(expectedPairwiseScore, pairwiseScore.ToString());
                    break;
            }

            Console.WriteLine(
                String.Format(@"PamsamBvtTest:{0} validation completed successfully for molecule type {1}",
                scoretype.ToString(),
                type));
            ApplicationLog.WriteLine(
                String.Format(@"PamsamBvtTest:{0} validation completed successfully for molecule type {1}",
                scoretype.ToString(),
                type.ToString()));
        }

        /// <summary>
        /// Validate the UnAlign method is removing gaps from the sequence
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="type">Molecule Type</param>
        void ValidateUNAlign(string nodeName, MoleculeType type)
        {
            string inputFilePath = Utility._xmlUtil.GetTextValue(nodeName, Constants.FilePathNode);

            ISequenceParser parser = new FastaParser();
            IList<ISequence> sequences = parser.Parse(inputFilePath);

            List<ISequence> alignedSequences = GetPAMSAMAlignedSequences(type, sequences);
            ISequenceItem gapItem = new Nucleotide('-', "Gap");
            Assert.IsTrue(alignedSequences[0].Contains(gapItem));
            ISequence unalignedseq = MsaUtils.UnAlign(alignedSequences[0]);
            Assert.IsFalse(unalignedseq.Contains(gapItem));

            Console.WriteLine(
                String.Format(@"PamsamBvtTest:Validation of UnAlign() method of MsaUtils completed 
                    successfully for molecule type {0}",
                                                       type));
            ApplicationLog.WriteLine(
                String.Format(@"PamsamBvtTest:Validation of UnAlign() method of MsaUtils completed 
                    successfully for molecule type {0}",
                                                       type));
        }

        /// <summary>
        /// Get Pamsam aligned sequences
        /// </summary>
        /// <param name="moleculeType">Molecule Type.</param>
        /// <param name="sequences">sequences.</param>
        /// <returns>returns aligned sequences</returns>
        List<ISequence> GetPAMSAMAlignedSequences(MoleculeType moleculeType,
          IList<ISequence> sequences)
        {
            switch (moleculeType)
            {
                case MoleculeType.DNA:
                    similarityMatrix = new SimilarityMatrix(
                      SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
                    break;
                case MoleculeType.RNA:
                    similarityMatrix = new SimilarityMatrix(
                      SimilarityMatrix.StandardSimilarityMatrix.AmbiguousRna);
                    break;
                case MoleculeType.Protein:
                    similarityMatrix = new SimilarityMatrix(
                        SimilarityMatrix.StandardSimilarityMatrix.Blosum62);
                    break;
            }
            // MSA aligned sequences.
            PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner(sequences,
              moleculeType, kmerLength, DistanceFunctionTypes.EuclideanDistance,
              UpdateDistanceMethodsTypes.Average,
              ProfileAlignerNames.NeedlemanWunschProfileAligner,
              ProfileScoreFunctionNames.InnerProductFast, similarityMatrix,
              gapOpenPenalty, gapExtendPenalty, 2, 2);

            return msa.AlignedSequences;
        }

        /// <summary>
        /// Validate function calculations of MsaUtils class.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">Molecule Type</param>
        /// <param name="edgeIndex">Edge Index</param>
        /// <param name="functionType">Function Type.</param>
        void ValidateFunctionCalculations(string nodeName,
          MoleculeType moleculeType, int edgeIndex, FunctionType functionType)
        {
            // Get Two profiles
            IProfileAlignment[] separatedProfileAlignments = GetProfiles(moleculeType, edgeIndex);

            switch (functionType)
            {
                case FunctionType.Correlation:
                    float correlation = MsaUtils.Correlation(
                      separatedProfileAlignments[0].ProfilesMatrix.ProfilesMatrix[0],
                      separatedProfileAlignments[1].ProfilesMatrix.ProfilesMatrix[0]);
                    string expectedCorrelation = Utility._xmlUtil.GetTextValue(nodeName,
                      Constants.CorrelationNode);
                    Assert.AreEqual(expectedCorrelation, correlation.ToString());
                    break;
                case FunctionType.FindMaxIndex:
                    string expectedMaxIndex = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.MaxIndexNode);
                    int index = MsaUtils.FindMaxIndex(
                      separatedProfileAlignments[0].ProfilesMatrix.ProfilesMatrix[0]);
                    Assert.AreEqual(expectedMaxIndex, index.ToString());
                    break;
                case FunctionType.JensenShanonDivergence:
                    string expectedJsDivergence = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.JensenShanonDivergenceNode);
                    float jsdivergence = MsaUtils.JensenShannonDivergence(
                      separatedProfileAlignments[0].ProfilesMatrix.ProfilesMatrix[0],
                      separatedProfileAlignments[1].ProfilesMatrix.ProfilesMatrix[0]);
                    Assert.AreEqual(expectedJsDivergence, jsdivergence.ToString());
                    break;
                case FunctionType.KullbackLeiblerDistance:
                    string expectedKlDistance = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.KullbackLeiblerDistanceNode);
                    float kldistance = MsaUtils.KullbackLeiblerDistance(
                      separatedProfileAlignments[0].ProfilesMatrix.ProfilesMatrix[0],
                      separatedProfileAlignments[1].ProfilesMatrix.ProfilesMatrix[0]);
                    Assert.AreEqual(expectedKlDistance, kldistance.ToString());
                    break;
            }

            Console.WriteLine(
                String.Format(@"Validation of {0} function calculation of MsaUtils completed 
                    successfully for molecule type {1}",
                                                       functionType,
                                                       moleculeType));
            ApplicationLog.WriteLine(
                String.Format(@"Validation of {0} function calculation of MsaUtils completed 
                    successfully for molecule type {1}",
                                                       functionType,
                                                       moleculeType));
        }

        /// <summary>
        /// Validate Muscle multiple sequence alignment with static properties 
        /// of PamsamMultipleSequenceAligner.
        /// </summary>
        /// <param name="nodeName">xml node name</param>
        /// <param name="moleculeType">molecule type</param>
        /// <param name="hierarchicalMethodName">hierarchical clustering method name</param>
        /// <param name="distanceFunctionName">kmerdistancematrix method name.</param>
        /// <param name="profileAlignName">SW/NW profiler</param>
        /// <param name="profileScoreName">Profile score function name.</param>
        /// <param name="useweights">use sequence weights true\false</param>
        /// <param name="fasterVersion">fasterversion true\false</param>
        /// <param name="useStageB">stage2 computation true\false</param>
        void ValidatePamsamAlign(string nodeName, MoleculeType moleculeType,
            string expectedScoreNode,
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName,
            DistanceFunctionTypes distanceFunctionName,
            ProfileAlignerNames profileAlignerName,
            ProfileScoreFunctionNames profileScoreName,
            bool useweights,
            bool fasterVersion,
            bool useStageB)
        {
            Initialize(nodeName, expectedScoreNode);

            // get old properties
            bool prevVersion = PAMSAMMultipleSequenceAligner.FasterVersion;
            bool prevUseWeights = PAMSAMMultipleSequenceAligner.UseWeights;
            bool prevUseStageB = PAMSAMMultipleSequenceAligner.UseStageB;

            try
            {
                // Set static properties
                PAMSAMMultipleSequenceAligner.FasterVersion = fasterVersion;
                PAMSAMMultipleSequenceAligner.UseWeights = useweights;
                PAMSAMMultipleSequenceAligner.UseStageB = useStageB;

                // MSA aligned sequences.
                int numberOfDegrees = 2;
                int numberOfPartitions = 2;
                PAMSAMMultipleSequenceAligner msa =
                    new PAMSAMMultipleSequenceAligner(lstSequences,
                        moleculeType, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                        profileAlignerName, profileScoreName, similarityMatrix, gapOpenPenalty,
                        gapExtendPenalty, numberOfDegrees, numberOfPartitions);

                // Validate the aligned Sequence and score
                if (fasterVersion)
                {
                    InitializeStage1Variables(nodeName);
                    Assert.AreEqual(stage1ExpectedSequences.Count, msa.AlignedSequences.Count);
                    int index = 0;
                    foreach (ISequence seq in msa.AlignedSequences)
                    {
                        Assert.AreEqual(seq.ToString(), stage1ExpectedSequences[index].ToString());
                        index++;
                    }
                    Assert.IsTrue(stage1ExpectedScore.Contains(msa.AlignmentScore.ToString()));
                }
                else
                {
                    int index = 0;
                    foreach (ISequence seq in msa.AlignedSequences)
                    {
                        Assert.AreEqual(seq.ToString(), expectedSequences[index].ToString());
                        index++;
                    }
                    Assert.AreEqual(expectedScore, msa.AlignmentScore.ToString());
                }
            }
            finally
            {
                // Reset it back
                PAMSAMMultipleSequenceAligner.FasterVersion = prevVersion;
                PAMSAMMultipleSequenceAligner.UseWeights = prevUseWeights;
                PAMSAMMultipleSequenceAligner.UseStageB = prevUseStageB;
            }

            Console.WriteLine(
                String.Format(@"Validation of pamsam alignment completed 
                      successfully for molecule type {0} with 
                      static property fasterversion {1}, usestageb {2} and useweights {3}",
                            moleculeType, fasterVersion, useStageB, useweights));
            ApplicationLog.WriteLine(
                String.Format(@"Validation of pamsam alignment completed 
                      successfully for molecule type {0} with 
                      static property fasterversion {1}, usestageb {2} and useweights {3}",
                            moleculeType, fasterVersion, useStageB, useweights));
        }


        /// <summary>
        /// Creates binarytree using stage1 sequences.
        /// Cut the binary tree at an random edge to get two profiles.
        /// </summary>
        /// <param name="moleculeType">Molecule Type.</param>
        /// <param name="edgeIndex">Random edge index.</param>
        /// <returns>Returns profiles</returns>
        IProfileAlignment[] GetProfiles(MoleculeType moleculeType, int edgeIndex)
        {
            Initialize(Constants.MuscleDnaSequenceNode, Constants.ExpectedScoreNode);
            InitializeStage2Variables(Constants.MuscleDnaSequenceNode);

            // Get Stage2 Binary Tree
            List<ISequence> stage1AlignedSequences = GetStage1AlignedSequence(moleculeType);
            IDistanceMatrix matrix = GetKimuraDistanceMatrix(stage1AlignedSequences);
            IHierarchicalClustering hierarcicalClustering = GetHierarchicalClustering(matrix);
            BinaryGuideTree binaryTree = GetBinaryTree(hierarcicalClustering);

            // Cut Tree at an edge and get sequences.
            List<int>[] leafNodeIndices = binaryTree.SeparateSequencesByCuttingTree(edgeIndex);

            // Extract profiles 
            List<int>[] removedPositions = null;
            IProfileAlignment[] separatedProfileAlignments = ProfileAlignment.ProfileExtraction(
              stage2ExpectedSequences, leafNodeIndices[0], leafNodeIndices[1], out removedPositions);

            return separatedProfileAlignments;
        }

        #endregion

        #endregion
    }
}