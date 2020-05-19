// -------------------------------------------------------------------------------------
// <copyright file="NeedlemanWunschProfileAlignerTest.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// Contains test cases for NeedlemanWunschProfileAligner class.
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
    /// Test for NeedlemanWunschProfileAligner class
    /// </summary>
    [TestFixture]
    public class NeedlemanWunschProfileAlignerTest
    {
        /// <summary>
        /// Test NeedlemanWunschProfileAligner class
        /// </summary>
        [Test]
        public void TestNeedlemanWunschProfileAligner()
        {

            ISequence templateSequence = new Sequence(Alphabets.DNA, "ATGCSWRYKMBVHDN-");
            Dictionary<ISequenceItem, int> itemSet = new Dictionary<ISequenceItem, int>();
            for (int i = 0; i < templateSequence.Count; ++i)
            {
                itemSet.Add(templateSequence[i], i);
            }
            Profiles.ItemSet = itemSet;


            IProfileAligner profileAligner = new NeedlemanWunschProfileAligner();
            SimilarityMatrix similarityMatrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrices.AmbiguousDna);
            int gapOpenPenalty = -8;
            int gapExtendPenalty = -1;

            profileAligner.SimilarityMatrix = similarityMatrix;
            profileAligner.GapOpenCost = gapOpenPenalty;
            profileAligner.GapExtensionCost = gapExtendPenalty;

            ISequence seqA = new Sequence(Alphabets.DNA, "GGGAAAAATCAGATT");
            ISequence seqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG");

            List<ISequence> sequences = new List<ISequence>();
            sequences.Add(seqA);
            sequences.Add(seqB);

            IProfileAlignment profileAlignmentA = ProfileAlignment.GenerateProfileAlignment(sequences[0]);
            IProfileAlignment profileAlignmentB = ProfileAlignment.GenerateProfileAlignment(sequences[1]);
            profileAligner.Align(profileAlignmentA, profileAlignmentB);

            
            List<int> eStringSubtree = profileAligner.GenerateEString(profileAligner.AlignedA);
            List<int> eStringSubtreeB = profileAligner.GenerateEString(profileAligner.AlignedB);

            List<ISequence> alignedSequences = new List<ISequence>();

            ISequence seq = profileAligner.GenerateSequenceFromEString(eStringSubtree, sequences[0]);
            alignedSequences.Add(seq);
            seq = profileAligner.GenerateSequenceFromEString(eStringSubtreeB, sequences[1]);
            alignedSequences.Add(seq);

            float profileScore = MsaUtils.MultipleAlignmentScoreFunction(alignedSequences, similarityMatrix, gapOpenPenalty, gapExtendPenalty);

            ISequence expectedSeqA = new Sequence(Alphabets.DNA, "GGGAA---AAATCAGATT");
            ISequence expectedSeqB = new Sequence(Alphabets.DNA, "GGGAATCAAAATCAG---");

            Assert.AreEqual(expectedSeqA.ToString(), alignedSequences[0].ToString());
            Assert.AreEqual(expectedSeqB.ToString(), alignedSequences[1].ToString());

            Assert.AreEqual(40, profileScore);

        }
    }
}