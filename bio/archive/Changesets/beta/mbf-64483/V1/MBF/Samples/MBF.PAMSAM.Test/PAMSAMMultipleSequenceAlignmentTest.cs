// -------------------------------------------------------------------------------------
// <copyright file="MuscleMultipleSequenceAlignmentTest.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Contains test cases for MuscleMultipleSequenceAlignment class.
// </summary>
// -------------------------------------------------------------------------------------

namespace MBF.Test
{
    using System;
    using System.Collections.Generic;
    using MBF.Algorithms;
    using MBF.Algorithms.Alignment;
    using MBF.Algorithms.Alignment.MultipleSequenceAlignment;
    using MBF.Util.Logging;
    using MBF.IO;
    using MBF.IO.Fasta;
    using NUnit.Framework;
    using MBF.SimilarityMatrices;

    /// <summary>
    /// Test for MuscleMultipleSequenceAlignment class
    /// </summary>
    [TestFixture]
    public class PAMSAMMultipleSequenceAlignmentTest
    {
        /// <summary>
        /// Test MuscleMultipleSequenceAlignment class
        /// </summary>
        [Test]
        public void TestMuscleMultipleSequenceAlignment()
        {

            SimilarityMatrix similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            int gapOpenPenalty = -4;
            int gapExtendPenalty = -1;
            int kmerLength = 3;

            ISequence seqA = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            ISequence seqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG");
            ISequence seqC = new Sequence(Alphabets.DNA, "GGGACAAAATCAG");
            List<ISequence> sequences = new List<ISequence>();
            sequences.Add(seqA);
            sequences.Add(seqB);
            sequences.Add(seqC);

            DistanceFunctionTypes distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            ProfileAlignerNames profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.WeightedInnerProduct;

            PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner
                (sequences, MoleculeType.DNA, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty, 
                Environment.ProcessorCount * 2, Environment.ProcessorCount);

            ISequence expectedSeqA = new Sequence(Alphabets.DNA, "GGGA---AAAATCAGATT");
            ISequence expectedSeqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG---");
            ISequence expectedSeqC = new Sequence(Alphabets.DNA, "GGGA--CAAAATCAG---");
            Console.WriteLine("Aligned sequences in stage 1: {0}", msa.AlignmentScoreA);
            for (int i = 0; i < msa.AlignedSequencesA.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequencesA[i].ToString());
            }
            //Console.WriteLine("Aligned sequences in stage 2: {0}", msa.AlignmentScoreB);
            //for (int i = 0; i < msa.AlignedSequencesB.Count; ++i)
            //{
            //    Console.WriteLine(msa.AlignedSequencesB[i].ToString());
            //}
            Console.WriteLine("Aligned sequences in stage 3: {0}", msa.AlignmentScoreC);
            for (int i = 0; i < msa.AlignedSequencesC.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequencesC[i].ToString());
            }
            Console.WriteLine("Aligned sequences final: {0}", msa.AlignmentScore);

            for (int i = 0; i < msa.AlignedSequences.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequences[i].ToString());
            }
            
            // Test case 2
            Console.WriteLine("Example 2");
            sequences = new List<ISequence>();
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAAATCG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCAATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCTTATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGACAAAATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGACAAAATCAG"));


            msa = new PAMSAMMultipleSequenceAligner
                (sequences, MoleculeType.DNA, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
                Environment.ProcessorCount * 2, Environment.ProcessorCount);

            Console.WriteLine("Aligned sequences in stage 1: {0}", msa.AlignmentScoreA);
            for (int i = 0; i < msa.AlignedSequencesA.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequencesA[i].ToString());
            }
            //Console.WriteLine("Aligned sequences in stage 2: {0}", msa.AlignmentScoreB);
            //for (int i = 0; i < msa.AlignedSequencesB.Count; ++i)
            //{
            //    Console.WriteLine(msa.AlignedSequencesB[i].ToString());
            //}
            Console.WriteLine("Aligned sequences in stage 3: {0}", msa.AlignmentScoreC);
            for (int i = 0; i < msa.AlignedSequencesC.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequencesC[i].ToString());
            }
            Console.WriteLine("Aligned sequences final: {0}", msa.AlignmentScore);
            for (int i = 0; i < msa.AlignedSequences.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequences[i].ToString());
            }
            
            // Test case e
            Console.WriteLine("Example 2");
            sequences = new List<ISequence>();
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAAATCG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCAATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGACAAAATCAG"));
            sequences.Add(new Sequence(Alphabets.DNA, "GGGAATCTTATCAG"));


            msa = new PAMSAMMultipleSequenceAligner
                (sequences, MoleculeType.DNA, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
                Environment.ProcessorCount * 2, Environment.ProcessorCount);

            Console.WriteLine("Aligned sequences in stage 1: {0}", msa.AlignmentScoreA);
            for (int i = 0; i < msa.AlignedSequencesA.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequencesA[i].ToString());
            }
            //Console.WriteLine("Aligned sequences in stage 2: {0}", msa.AlignmentScoreB);
            //for (int i = 0; i < msa.AlignedSequencesB.Count; ++i)
            //{
            //    Console.WriteLine(msa.AlignedSequencesB[i].ToString());
            //}
            Console.WriteLine("Aligned sequences in stage 3: {0}", msa.AlignmentScoreC);
            for (int i = 0; i < msa.AlignedSequencesC.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequencesC[i].ToString());
            }
            Console.WriteLine("Aligned sequences final: {0}", msa.AlignmentScore);
            for (int i = 0; i < msa.AlignedSequences.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequences[i].ToString());
            }

        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestMuscleMultipleSequenceAlignmentRunningTime()
        {

            // Test on DNA benchmark dataset
            ISequenceParser parser = new FastaParser();
            //string filepath = @"testdata\FASTA\RunningTime\122.afa";
            string filepath = @"testdata\FASTA\RunningTime\BOX246.xml.afa";

            MoleculeType mt = MoleculeType.Protein;

            IList<ISequence> orgSequences = parser.Parse(filepath);

            List<ISequence> sequences = MsaUtils.UnAlign(orgSequences);

            //filepath = @"testdata\FASTA\RunningTime\12_raw.afa";
            //List<ISequence> sequences = parser.Parse(filepath);

            int numberOfSequences = orgSequences.Count;
            
            Console.WriteLine("Original sequences are:");
            for (int i = 0; i < numberOfSequences; ++i)
            {
                Console.WriteLine(sequences[i].ToString());
            }

            Console.WriteLine("Benchmark sequences are:");
            for (int i = 0; i < numberOfSequences; ++i)
            {
                Console.WriteLine(orgSequences[i].ToString());
            }
            
            PAMSAMMultipleSequenceAligner.FasterVersion = true;
            PAMSAMMultipleSequenceAligner.UseWeights = false;
            PAMSAMMultipleSequenceAligner.UseStageB = false;
            PAMSAMMultipleSequenceAligner.NumberOfCores = 2;
            
            int gapOpenPenalty = -13;
            int gapExtendPenalty = -5;
            int kmerLength = 2;

            int numberOfDegrees = 2;//Environment.ProcessorCount;
            int numberOfPartitions = 16;// Environment.ProcessorCount * 2;


            DistanceFunctionTypes distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            ProfileAlignerNames profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.InnerProductFast;

            SimilarityMatrix similarityMatrix = null;

            switch (mt)
            {
                case (MoleculeType.DNA):
                    similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
                    break;
                case (MoleculeType.RNA):
                    similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousRna);
                    break;
                case (MoleculeType.Protein):
                    similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum62);
                    break;
                default:
                    throw new Exception("Invalid molecular type");
            }

            PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner
               (sequences, mt, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
               profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
               numberOfPartitions, numberOfDegrees);
            Console.WriteLine("The number of partitions is: {0}", numberOfPartitions);
            Console.WriteLine("The number of degrees is: {0}", numberOfDegrees);
            Console.WriteLine("Alignment score Q is: {0}", MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequences, orgSequences));
            Console.WriteLine("Alignment score TC is: {0}", MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequences, orgSequences));
            

            
            Console.WriteLine("Benchmark SPS score is: {0}", MsaUtils.MultipleAlignmentScoreFunction(orgSequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty));
            Console.WriteLine("Aligned sequences in stage 1: {0}", msa.AlignmentScoreA);
            for (int i = 0; i < msa.AlignedSequencesA.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequencesA[i].ToString());
            }
            Console.WriteLine("Alignment score Q is: {0}", MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequencesA, orgSequences));
            Console.WriteLine("Alignment score TC is: {0}", MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequencesA, orgSequences));
            Console.WriteLine("Aligned sequences in stage 2: {0}", msa.AlignmentScoreB);
            for (int i = 0; i < msa.AlignedSequencesB.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequencesB[i].ToString());
            }
            Console.WriteLine("Alignment score Q is: {0}", MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequencesB, orgSequences));
            Console.WriteLine("Alignment score TC is: {0}", MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequencesB, orgSequences));
            Console.WriteLine("Aligned sequences in stage 3: {0}", msa.AlignmentScoreC);
            for (int i = 0; i < msa.AlignedSequencesC.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequencesC[i].ToString());
            }
            Console.WriteLine("Alignment score Q is: {0}", MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequencesC, orgSequences));
            Console.WriteLine("Alignment score TC is: {0}", MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequencesC, orgSequences));
            Console.WriteLine("Aligned sequences final: {0}", msa.AlignmentScore);
            for (int i = 0; i < msa.AlignedSequences.Count; ++i)
            {
                Console.WriteLine(msa.AlignedSequences[i].ToString());
            }
            Console.WriteLine("Alignment score Q is: {0}", MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequences, orgSequences));
            Console.WriteLine("Alignment score TC is: {0}", MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequences, orgSequences));
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void testBug()
        {
            List<ISequence> sequences = new List<ISequence>();
            ISequence seq1 = new Sequence(Alphabets.Protein, "MQEPQSELNIDPPLSQETFSELWNLLPENNVLSSELCPAVDELLLPESVVNWLDEDSDDAPRMPATSAP");

            ISequence seq2 = new Sequence(Alphabets.Protein, "PLSQETFSDLWNLLPENNLLSSELSAPVDDLLPYTDVATWLDECPNEAPQMPEPSAPAAPPPATPAPATSWPLSSFVPSQKTYPGNYGFRLGF");

            ISequence seq3 = new Sequence(Alphabets.Protein, "MEPSSETGMDPPLSQETFEDLWSLLPDPLQTVTCRLDNLSEFPDYPLAADMSVLQEGLMGNAVPTVTSCAPSTDDYAGKYGLQLDFQQNGTAKS");

            ISequence seq4 = new Sequence(Alphabets.Protein, "MEEPQSDPSVEPPLSQETFSDLWKLLPENNVLSPLPSQAMDDLMLSPDDIEQWFTEDPGPDEAPRMPEAAPRVAPAPAAPTPAAPAPAPSWPLS");

            ISequence seq5 = new Sequence(Alphabets.Protein, "MEESQAELGVEPPLSQETFSDLWKLLPENNLLSSELSPAVDDLLLSPEDVANWLDERPDEAPQMPEPPAPAAPTPAAPAPATSWPLSSFVPSQK");

            ISequence seq6 = new Sequence(Alphabets.Protein, "MTAMEESQSDISLELPLSQETFSGLWKLLPPEDILPSPHCMDDLLLPQDVEEFFEGPSEALRVSGAPAAQDPVTETPGPVAPAPATPWPLSSFVPSQKTYQGNYGFHLGFLQ");

            ISequence seq7 = new Sequence(Alphabets.Protein, "FRLGFLHSGTAKSVTWTYSPLLNKLFCQLAKTCPVQLWVSSPPPPNTCVRAMAIYKKSEFVTEVVRRCPHHERCSDSSDGLAPPQHLIRVEGNLRAKYLDDRNTFRHSVV");
            sequences.Add(seq1);
            sequences.Add(seq2);
            sequences.Add(seq3);
            sequences.Add(seq4);
            sequences.Add(seq5);
            sequences.Add(seq6);
            sequences.Add(seq7);

            SimilarityMatrix sm = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum50);

            PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner(sequences, MoleculeType.Protein,
                2, DistanceFunctionTypes.EuclideanDistance, UpdateDistanceMethodsTypes.Average, ProfileAlignerNames.NeedlemanWunschProfileAligner,
                ProfileScoreFunctionNames.WeightedEuclideanDistance, sm, -8, -1, 2, 16);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void testBug2()
        {
            //Test on DNA benchmark dataset
            ISequenceParser parser = new FastaParser();
            string filepath = @"testdata\122_raw.afa";

            MoleculeType mt = MoleculeType.DNA;

            IList<ISequence> orgSequences = parser.Parse(filepath);

            List<ISequence> sequences = MsaUtils.UnAlign(orgSequences);
            int numberOfSequences = orgSequences.Count;
            PAMSAMMultipleSequenceAligner.FasterVersion = false;
            PAMSAMMultipleSequenceAligner.UseWeights = false;
            PAMSAMMultipleSequenceAligner.UseStageB = false;
            PAMSAMMultipleSequenceAligner.NumberOfCores = 2;

            int gapOpenPenalty = -13;
            int gapExtendPenalty = -5;
            int kmerLength = 2;

            int numberOfDegrees = 2;//Environment.ProcessorCount;
            int numberOfPartitions = 16;// Environment.ProcessorCount * 2;


            DistanceFunctionTypes distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            ProfileAlignerNames profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.InnerProductFast;

            SimilarityMatrix similarityMatrix = null;

            switch (mt)
            {
                case (MoleculeType.DNA):
                    similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
                    break;
                case (MoleculeType.RNA):
                    similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousRna);
                    break;
                case (MoleculeType.Protein):
                    similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum62);
                    break;
                default:
                    throw new Exception("Invalid molecular type");
            }

            //DateTime startTime = DateTime.Now;
            PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner
               (sequences, mt, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
               profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
               numberOfPartitions, numberOfDegrees);

        }
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void testBug3()
        {
            //Test on DNA benchmark dataset
            ISequenceParser parser = new FastaParser();
            string filepath = @"testdata\122_raw.afa";

            MoleculeType mt = MoleculeType.DNA;

            IList<ISequence> orgSequences = parser.Parse(filepath);

            List<ISequence> sequences = MsaUtils.UnAlign(orgSequences);
            int numberOfSequences = orgSequences.Count;
            PAMSAMMultipleSequenceAligner.FasterVersion = false;
            PAMSAMMultipleSequenceAligner.UseWeights = false;
            PAMSAMMultipleSequenceAligner.UseStageB = false;
            PAMSAMMultipleSequenceAligner.NumberOfCores = 2;

            int gapOpenPenalty = -13;
            int gapExtendPenalty = -5;
            int kmerLength = 2;

            int numberOfDegrees = 2;//Environment.ProcessorCount;
            int numberOfPartitions = 16;// Environment.ProcessorCount * 2;


            DistanceFunctionTypes distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            ProfileAlignerNames profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.InnerProductFast;

            SimilarityMatrix similarityMatrix = null;

            switch (mt)
            {
                case (MoleculeType.DNA):
                    similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
                    break;
                case (MoleculeType.RNA):
                    similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousRna);
                    break;
                case (MoleculeType.Protein):
                    similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum62);
                    break;
                default:
                    throw new Exception("Invalid molecular type");
            }

            //DateTime startTime = DateTime.Now;
            PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner
               (sequences, mt, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
               profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
               numberOfPartitions, numberOfDegrees);

        }
    }
}