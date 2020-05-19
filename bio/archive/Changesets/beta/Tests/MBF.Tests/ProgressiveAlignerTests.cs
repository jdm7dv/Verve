// -------------------------------------------------------------------------------------
// <copyright file="ProgressiveAlignerTest.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Contains test cases for ProgressiveAligner class.
// </summary>
// -------------------------------------------------------------------------------------

namespace MBF.Tests
{
    using System;
    using System.Collections.Generic;
    using MBF.Algorithms.Alignment.MultipleSequenceAlignment;
    using MBF.IO;
    using MBF.IO.Fasta;

    using MBF.SimilarityMatrices;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test for ProgressiveAligner class
    /// </summary>
    [TestClass]
    public class ProgressiveAlignerTests
    {

        /// <summary>
        /// Test ProgressiveAligner class
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestProgressiveAligner()
        {

            MsaUtils.SetProfileItemSets(MoleculeType.DNA);

            SimilarityMatrix similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            int gapOpenPenalty = -8;
            int gapExtendPenalty = -1;
            int kmerLength = 4;

            PAMSAMMultipleSequenceAligner.parallelOption = new ParallelOptions { MaxDegreeOfParallelism = 2 };
            ISequence seqA = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            ISequence seqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG");
            ISequence seqC = new Sequence(Alphabets.DNA, "GGGACAAAATCAG");
            List<ISequence> sequences = new List<ISequence>();
            sequences.Add(seqA);
            sequences.Add(seqB);
            sequences.Add(seqC);

            KmerDistanceMatrixGenerator kmerDistanceMatrixGenerator =
                new KmerDistanceMatrixGenerator(sequences, kmerLength, MoleculeType.DNA);

            kmerDistanceMatrixGenerator.GenerateDistanceMatrix(sequences);

            IHierarchicalClustering hierarchicalClustering = new HierarchicalClusteringParallel(kmerDistanceMatrixGenerator.DistanceMatrix);

            BinaryGuideTree tree = new BinaryGuideTree(hierarchicalClustering);

            IProgressiveAligner progressiveAligner = new ProgressiveAligner(ProfileAlignerNames.NeedlemanWunschProfileAligner, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            progressiveAligner.Align(sequences, tree);

            ISequence expectedSeqA = new Sequence(Alphabets.DNA, "GGGA---AAAATCAGATT");
            ISequence expectedSeqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG---");
            ISequence expectedSeqC = new Sequence(Alphabets.DNA, "GGGA--CAAAATCAG---");

            Assert.AreEqual(expectedSeqA.ToString(), progressiveAligner.AlignedSequences[0].ToString());
            Assert.AreEqual(expectedSeqB.ToString(), progressiveAligner.AlignedSequences[1].ToString());
            Assert.AreEqual(expectedSeqC.ToString(), progressiveAligner.AlignedSequences[2].ToString());



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

            kmerDistanceMatrixGenerator.GenerateDistanceMatrix(sequences);

            hierarchicalClustering = new HierarchicalClusteringParallel(kmerDistanceMatrixGenerator.DistanceMatrix);

            tree = new BinaryGuideTree(hierarchicalClustering);

            for (int i = 0; i < tree.NumberOfNodes; ++i)
            {
                Console.WriteLine("Node {0} ID: {1}", i, tree.Nodes[i].ID);
            }
            for (int i = 0; i < tree.NumberOfEdges; ++i)
            {
                Console.WriteLine("Edge {0} ID: {1}, length: {2}", i, tree.Edges[i].ID, tree.Edges[i].Length);
            }

            SequenceWeighting sw = new SequenceWeighting(tree);

            for (int i = 0; i < sw.Weights.Length; ++i)
            {
                Console.WriteLine("weights {0} is {1}", i, sw.Weights[i]);
            }

            progressiveAligner = new ProgressiveAligner(ProfileAlignerNames.NeedlemanWunschProfileAligner, similarityMatrix, gapOpenPenalty, gapExtendPenalty);
            progressiveAligner.Align(sequences, tree);
            for (int i = 0; i < progressiveAligner.AlignedSequences.Count; ++i)
            {
                Console.WriteLine(progressiveAligner.AlignedSequences[i].ToString());
            }


            MsaUtils.SetProfileItemSets(MoleculeType.Protein);
            ISequenceParser parser = new FastaParser();
            string filepath = @"TestUtils\FASTA\Protein\BB11001.tfa";
            IList<ISequence> orgSequences = parser.Parse(filepath);


            sequences = MsaUtils.UnAlign(orgSequences);

            similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum62);
            kmerLength = 4;

            gapOpenPenalty = -13;
            gapExtendPenalty = -5;

            kmerDistanceMatrixGenerator =
                new KmerDistanceMatrixGenerator(sequences, kmerLength, MoleculeType.DNA);

            kmerDistanceMatrixGenerator.GenerateDistanceMatrix(sequences);

            hierarchicalClustering = new HierarchicalClusteringParallel(kmerDistanceMatrixGenerator.DistanceMatrix);

            tree = new BinaryGuideTree(hierarchicalClustering);

            for (int i = tree.NumberOfLeaves; i < tree.Nodes.Count; ++i)
            {
                Console.WriteLine("Node {0}: leftchildren-{1}, rightChildren-{2}", i, tree.Nodes[i].LeftChildren.ID, tree.Nodes[i].RightChildren.ID);
            }
            progressiveAligner = new ProgressiveAligner(ProfileAlignerNames.NeedlemanWunschProfileAligner, similarityMatrix, gapOpenPenalty, gapExtendPenalty);
            progressiveAligner.Align(sequences, tree);
            for (int i = 0; i < progressiveAligner.AlignedSequences.Count; ++i)
            {
                Console.WriteLine(progressiveAligner.AlignedSequences[i].ToString());
            }

            ((FastaParser)parser).Dispose();
        }
    }
}