// -------------------------------------------------------------------------------------
// <copyright file="ProgressiveAlignerTest.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Contains test cases for ProgressiveAligner class.
// </summary>
// -------------------------------------------------------------------------------------

namespace Bio.Test
{
    using System;
    using System.Collections.Generic;
    using Bio.Algorithms;
    using Bio.Algorithms.Alignment;
    using Bio.Algorithms.Alignment.MultipleSequenceAlignment;
    using Bio.Util.Logging;
    using NUnit.Framework;

    /// <summary>
    /// Test for ProgressiveAligner class
    /// </summary>
    [TestFixture]
    public class ProgressiveAlignerTest
    {
        /// <summary>
        /// Test ProgressiveAligner class
        /// </summary>
        [Test]
        public void TestProgressiveAligner()
        {

            ISequence templateSequence = new Sequence(Alphabets.DNA, "ATGCSWRYKMBVHDN-");
            Dictionary<ISequenceItem, int> itemSet = new Dictionary<ISequenceItem, int>();
            for (int i = 0; i < templateSequence.Count; ++i)
            {
                itemSet.Add(templateSequence[i], i);
            }
            Profiles.ItemSet = itemSet;

            SimilarityMatrix similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrices.AmbiguousDna);
            int gapOpenPenalty = -8;
            int gapExtendPenalty = -1;
            int kmerLength = 3;

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

            IHierarchicalClustering hierarchicalClustering = new HierarchicalClusteringSerial(kmerDistanceMatrixGenerator.DistanceMatrix);

            BinaryGuideTree tree = new BinaryGuideTree(hierarchicalClustering);

            IProgressiveAligner progressiveAligner = new ProgressiveAligner(ProfileAlignerNames.NeedlemanWunschProfileAligner, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            progressiveAligner.Align(sequences, tree);

            ISequence expectedSeqA = new Sequence(Alphabets.DNA, "GGGA---AAAATCAGATT");
            ISequence expectedSeqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG---");
            ISequence expectedSeqC = new Sequence(Alphabets.DNA, "GGGA--CAAAATCAG---");

            Assert.AreEqual(expectedSeqA.ToString(), progressiveAligner.AlignedSequences[0].ToString());
            Assert.AreEqual(expectedSeqB.ToString(), progressiveAligner.AlignedSequences[1].ToString());
            Assert.AreEqual(expectedSeqC.ToString(), progressiveAligner.AlignedSequences[2].ToString());
        }
    }
}