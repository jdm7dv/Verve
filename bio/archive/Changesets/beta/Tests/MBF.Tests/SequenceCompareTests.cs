// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using MBF.Algorithms.Kmer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.Tests
{
    /// <summary>
    /// Unit tests for the Sequence Compare.
    /// </summary>
    [TestClass]
    public class SequenceCompareTests
    {
        /// <summary>
        /// Compares the difference between 2 sequences.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void SequenceCompare()
        {
            ISequence seq1 = new Sequence(Alphabets.DNA, "AAAAAA");
            ISequence seq2 = new Sequence(Alphabets.DNA, "AAATAA");

            SequenceToKmerBuilder kmerBuilder = new SequenceToKmerBuilder();
            KmersOfSequence kmers = kmerBuilder.Build(seq1, 2);
            List<WordMatch> nodes = WordMatch.BuildMatchTable(kmers, seq1, seq2, 2);
            List<WordMatch> matchList = WordMatch.GetMinimalList(nodes, 2);
            List<DifferenceNode> diffNode = DifferenceNode.BuildDiffList(matchList, seq1, seq2);
            List<DifferenceNode.CompareFeature> features = DifferenceNode.OutputDiffList(diffNode, seq1, seq2);

            Assert.AreEqual(features.Count, 4);
            Assert.AreEqual(features[0].Feature, "Insertion of 1 bases in 2 ");
            Assert.AreEqual(features[1].FeatureType, "REPLACE");
            Assert.AreEqual(features[2].Feature, "Insertion of 1 bases in 1 ");
            Assert.AreEqual(features[3].FeatureType, "REPLACE");
        }
    }
}
