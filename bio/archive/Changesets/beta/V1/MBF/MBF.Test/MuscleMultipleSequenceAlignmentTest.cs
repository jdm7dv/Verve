// -------------------------------------------------------------------------------------
// <copyright file="MuscleMultipleSequenceAlignmentTest.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Contains test cases for MuscleMultipleSequenceAlignment class.
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
    /// Test for MuscleMultipleSequenceAlignment class
    /// </summary>
    [TestFixture]
    public class MuscleMultipleSequenceAlignmentTest
    {
        /// <summary>
        /// Test MuscleMultipleSequenceAlignment class
        /// </summary>
        [Test]
        public void TestMuscleMultipleSequenceAlignment()
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

            DistanceFunctionTypes distanceFunctionName = DistanceFunctionTypes.EuclieanDistance;
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName = UpdateDistanceMethodsTypes.Aaverage;
            ProfileAlignerNames profileAlignerName = ProfileAlignerNames.NeedlemanWunschProfileAligner;
            ProfileScoreFunctionNames profileProfileFunctionName = ProfileScoreFunctionNames.WeightedInnerProduct;

            MuscleMultipleSequenceAlignment msa = new MuscleMultipleSequenceAlignment
                (sequences, MoleculeType.DNA, kmerLength, distanceFunctionName, hierarchicalClusteringMethodName,
                profileAlignerName, profileProfileFunctionName, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            ISequence expectedSeqA = new Sequence(Alphabets.DNA, "GGGA---AAAATCAGATT");
            ISequence expectedSeqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG---");
            ISequence expectedSeqC = new Sequence(Alphabets.DNA, "GGGA--CAAAATCAG---");

            Assert.AreEqual(expectedSeqA.ToString(), msa.AlignedSequences[0].ToString());
            Assert.AreEqual(expectedSeqB.ToString(), msa.AlignedSequences[1].ToString());
            Assert.AreEqual(expectedSeqC.ToString(), msa.AlignedSequences[2].ToString());

            Assert.AreEqual(46, msa.AlignmentScore);
        }
    }
}