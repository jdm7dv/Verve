// -------------------------------------------------------------------------------------
// <copyright file="MsaBenchMarkTest.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Contains test cases for MuscleMultipleSequenceAlignment class.
// </summary>
// -------------------------------------------------------------------------------------

namespace MBF.Tests
{
    using System;
    using System.Collections.Generic;
    using MBF.Algorithms.Alignment.MultipleSequenceAlignment;
    using System.IO;

    using MBF.IO;
    using MBF.IO.Fasta;

    using MBF.SimilarityMatrices;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// MsaBenchMarkTest
    /// </summary>
    [TestClass]
    public class MsaBenchMarkTests
    {

        /// <summary>
        /// Test MsaBenchMark
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestMsaBenchMark()
        {
            string fileDirectory = @"TestUtils\FASTA\Protein\Balibase\RV911\";
            DirectoryInfo iD = new DirectoryInfo(fileDirectory);

            PAMSAMMultipleSequenceAligner.FasterVersion = false;
            PAMSAMMultipleSequenceAligner.UseWeights = false;
            PAMSAMMultipleSequenceAligner.UseStageB = true;
            PAMSAMMultipleSequenceAligner.NumberOfCores = 2;
            MoleculeType mt = MoleculeType.Protein;
            SimilarityMatrix similarityMatrix;
            int gapOpenPenalty = -20;
            int gapExtendPenalty = -5;
            int kmerLength = 4;

            int numberOfDegrees = 2;//Environment.ProcessorCount;
            int numberOfPartitions = 16;// Environment.ProcessorCount * 2;

            DistanceFunctionTypes distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            ProfileAlignerNames profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.WeightedInnerProductCached;

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
                    throw new InvalidDataException("Invalid molecular type");
            }


            List<float> allQ = new List<float>();
            List<float> allTC = new List<float>();

            foreach (FileInfo fi in iD.GetFiles())
            {
                String filePath = fi.FullName;
                Console.WriteLine(filePath);
                ISequenceParser parser = new FastaParser();

                IList<ISequence> orgSequences = parser.Parse(filePath);

                List<ISequence> sequences = MsaUtils.UnAlign(orgSequences);

                int numberOfSequences = orgSequences.Count;


                Console.WriteLine("The number of sequences is: {0}", numberOfSequences);
                Console.WriteLine("Original unaligned sequences are:");
                for (int i = 0; i < numberOfSequences; ++i)
                {
                    //Console.WriteLine(sequences[i].ToString());
                }
                Console.WriteLine("Original aligned sequences are:");
                for (int i = 0; i < numberOfSequences; ++i)
                {
                    //Console.WriteLine(orgSequences[i].ToString());
                }

                PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner
                    (sequences, mt, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                    profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
                    numberOfPartitions, numberOfDegrees);


                Console.WriteLine("Aligned sequences in stage 1: {0}", msa.AlignmentScoreA);
                for (int i = 0; i < msa.AlignedSequencesA.Count; ++i)
                {
                    Console.WriteLine(msa.AlignedSequencesA[i].ToString());
                }
                Console.WriteLine("Aligned sequences in stage 2: {0}", msa.AlignmentScoreB);
                for (int i = 0; i < msa.AlignedSequencesB.Count; ++i)
                {
                    Console.WriteLine(msa.AlignedSequencesB[i].ToString());
                }
                Console.WriteLine("Aligned sequences in stage 3: {0}", msa.AlignmentScoreC);
                for (int i = 0; i < msa.AlignedSequencesC.Count; ++i)
                {
                    Console.WriteLine(msa.AlignedSequencesC[i].ToString());
                }

                Console.WriteLine("Aligned sequences final: {0}", msa.AlignmentScore);
                for (int i = 0; i < msa.AlignedSequences.Count; ++i)
                {
                    //Console.WriteLine(msa.AlignedSequences[i].ToString());
                }
                float scoreQ = MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequences, orgSequences);
                float scoreTC = MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequences, orgSequences);
                allQ.Add(scoreQ);
                allTC.Add(scoreTC);
                Console.WriteLine("Alignment score Q is: {0}", scoreQ);
                Console.WriteLine("Alignment score TC is: {0}", scoreTC);
                ((FastaParser)parser).Dispose();
            }
            Console.WriteLine("Number of datasets is: {0}", allQ.Count);
            Console.WriteLine("average Q score is: {0}", MsaUtils.Mean(allQ.ToArray()));
            Console.WriteLine("average TC score is: {0}", MsaUtils.Mean(allTC.ToArray()));

        }
        /// <summary>
        /// Test MsaBenchMark on very large dataset
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestMsaBenchMarkLargeDataset()
        {
            // Test on DNA benchmark dataset
            ISequenceParser parser = new FastaParser();
            string filepath = @"TestUtils\FASTA\Protein\Balibase\RV913\BOX032.xml.afa";
            IList<ISequence> orgSequences = parser.Parse(filepath);

            IList<ISequence> sequences = MsaUtils.UnAlign(orgSequences);
            int numberOfSequences = orgSequences.Count;

            String outputFilePath = @"tempBOX032.xml.afa";

            using (StreamWriter writer = new StreamWriter(outputFilePath, true))
            {

                foreach (ISequence sequence in sequences)
                {
                    writer.WriteLine(">" + sequence.ID);
                    // write sequence
                    BasicDerivedSequence derivedSeq = new BasicDerivedSequence(sequence, false, false, 0, 0);
                    for (int lineStart = 0; lineStart < sequence.Count; lineStart += 60)
                    {
                        derivedSeq.RangeStart = lineStart;
                        derivedSeq.RangeLength = Math.Min(60, sequence.Count - lineStart);
                        writer.WriteLine(derivedSeq.ToString());
                    }
                    writer.Flush();
                }
            }

            sequences.Clear();
            sequences = parser.Parse(outputFilePath);

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

            PAMSAMMultipleSequenceAligner.FasterVersion = false;
            PAMSAMMultipleSequenceAligner.UseWeights = false;
            PAMSAMMultipleSequenceAligner.UseStageB = true;
            PAMSAMMultipleSequenceAligner.NumberOfCores = 2;
            int gapOpenPenalty = -13;
            int gapExtendPenalty = -5;
            int kmerLength = 3;

            int numberOfDegrees = 2;//Environment.ProcessorCount;
            int numberOfPartitions = 16;// Environment.ProcessorCount * 2;

            SimilarityMatrix similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum62);

            DistanceFunctionTypes distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            ProfileAlignerNames profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.WeightedInnerProduct;

            PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner
               (sequences, MoleculeType.Protein, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
               profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
               numberOfPartitions, numberOfDegrees);

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

            if (File.Exists(outputFilePath))
            {
                File.Delete(outputFilePath);
            }
            ((FastaParser)parser).Dispose();
        }

        /// <summary>
        /// Test on SABmark
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestMsaBenchMarkOnSABmark()
        {

            List<float> allQ = new List<float>();
            List<float> allTC = new List<float>();

            string fileDirectory = @"TestUtils\FASTA\Protein\SABmark";
            DirectoryInfo iD = new DirectoryInfo(fileDirectory);

            PAMSAMMultipleSequenceAligner.FasterVersion = false;
            PAMSAMMultipleSequenceAligner.UseWeights = false;
            PAMSAMMultipleSequenceAligner.UseStageB = true;
            PAMSAMMultipleSequenceAligner.NumberOfCores = 2;

            MoleculeType mt = MoleculeType.Protein;
            SimilarityMatrix similarityMatrix;
            int gapOpenPenalty = -13;
            int gapExtendPenalty = -5;
            int kmerLength = 3;

            int numberOfDegrees = 2;//Environment.ProcessorCount;
            int numberOfPartitions = 16;// Environment.ProcessorCount * 2;

            DistanceFunctionTypes distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            ProfileAlignerNames profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.WeightedInnerProduct;

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
                    throw new InvalidDataException("Invalid molecular type");
            }


            foreach (DirectoryInfo fi in iD.GetDirectories())
            {
                foreach (DirectoryInfo fii in fi.GetDirectories())
                {
                    foreach (FileInfo fiii in fii.GetFiles())
                    {
                        String filePath = fiii.FullName;
                        Console.WriteLine(filePath);
                        ISequenceParser parser = new FastaParser();

                        IList<ISequence> orgSequences = parser.Parse(filePath);

                        List<ISequence> sequences = MsaUtils.UnAlign(orgSequences);

                        int numberOfSequences = orgSequences.Count;

                        Console.WriteLine("The number of sequences is: {0}", numberOfSequences);
                        Console.WriteLine("Original unaligned sequences are:");

                        PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner
                            (sequences, mt, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                            profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
                            numberOfPartitions, numberOfDegrees);

                        Console.WriteLine("Aligned sequences final: {0}", msa.AlignmentScore);
                        for (int i = 0; i < msa.AlignedSequences.Count; ++i)
                        {
                            //Console.WriteLine(msa.AlignedSequences[i].ToString());
                        }
                        float scoreQ = MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequences, orgSequences);
                        float scoreTC = MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequences, orgSequences);
                        allQ.Add(scoreQ);
                        allTC.Add(scoreTC);
                        Console.WriteLine("Alignment score Q is: {0}", scoreQ);
                        Console.WriteLine("Alignment score TC is: {0}", scoreTC);

                        if (allQ.Count % 1000 == 0)
                        {
                            Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                            Console.WriteLine("average Q score is: {0}", MsaUtils.Mean(allQ.ToArray()));
                            Console.WriteLine("average TC score is: {0}", MsaUtils.Mean(allTC.ToArray()));
                        }
                        ((FastaParser)parser).Dispose();
                    }
                }
            }

            Console.WriteLine("average Q score is: {0}", MsaUtils.Mean(allQ.ToArray()));
            Console.WriteLine("average TC score is: {0}", MsaUtils.Mean(allTC.ToArray()));

        }

        /// <summary>
        /// Test on SABmark
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestMsaBenchMarkOnBralibase()
        {

            List<float> allQ = new List<float>();
            List<float> allTC = new List<float>();

            string fileDirectory = @"TestUtils\FASTA\RNA\k10";
            DirectoryInfo iD = new DirectoryInfo(fileDirectory);

            PAMSAMMultipleSequenceAligner.FasterVersion = false;
            PAMSAMMultipleSequenceAligner.UseWeights = false;
            PAMSAMMultipleSequenceAligner.UseStageB = false;
            PAMSAMMultipleSequenceAligner.NumberOfCores = 2;

            MoleculeType mt = MoleculeType.RNA;
            SimilarityMatrix similarityMatrix;
            int gapOpenPenalty = -20;
            int gapExtendPenalty = -5;
            int kmerLength = 4;

            int numberOfDegrees = 2;//Environment.ProcessorCount;
            int numberOfPartitions = 16;// Environment.ProcessorCount * 2;

            DistanceFunctionTypes distanceFunctionName = DistanceFunctionTypes.EuclideanDistance;
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Average;
            ProfileAlignerNames profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.WeightedInnerProductCached;

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
                    throw new InvalidDataException("Invalid molecular type");
            }


            foreach (DirectoryInfo fi in iD.GetDirectories())
            {
                foreach (FileInfo fiii in fi.GetFiles())
                {
                    String filePath = fiii.FullName;
                    Console.WriteLine(filePath);
                    ISequenceParser parser = new FastaParser();

                    IList<ISequence> orgSequences = parser.Parse(filePath);

                    List<ISequence> sequences = MsaUtils.UnAlign(orgSequences);

                    int numberOfSequences = orgSequences.Count;

                    Console.WriteLine("The number of sequences is: {0}", numberOfSequences);
                    Console.WriteLine("Original unaligned sequences are:");

                    PAMSAMMultipleSequenceAligner msa = new PAMSAMMultipleSequenceAligner
                        (sequences, mt, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                        profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty,
                        numberOfPartitions, numberOfDegrees);

                    Console.WriteLine("Aligned sequences final: {0}", msa.AlignmentScore);
                    for (int i = 0; i < msa.AlignedSequences.Count; ++i)
                    {
                        //Console.WriteLine(msa.AlignedSequences[i].ToString());
                    }
                    float scoreQ = MsaUtils.CalculateAlignmentScoreQ(msa.AlignedSequences, orgSequences);
                    float scoreTC = MsaUtils.CalculateAlignmentScoreTC(msa.AlignedSequences, orgSequences);
                    allQ.Add(scoreQ);
                    allTC.Add(scoreTC);
                    Console.WriteLine("Alignment score Q is: {0}", scoreQ);
                    Console.WriteLine("Alignment score TC is: {0}", scoreTC);

                    if (allQ.Count % 1000 == 0)
                    {
                        Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                        Console.WriteLine("average Q score is: {0}", MsaUtils.Mean(allQ.ToArray()));
                        Console.WriteLine("average TC score is: {0}", MsaUtils.Mean(allTC.ToArray()));
                    }
                    ((FastaParser)parser).Dispose();
                }
            }
            Console.WriteLine("number of datasets is: {0}", allQ.Count);
            Console.WriteLine("average Q score is: {0}", MsaUtils.Mean(allQ.ToArray()));
            Console.WriteLine("average TC score is: {0}", MsaUtils.Mean(allTC.ToArray()));

        }
    }
}
