// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Tests
{
    using System;
    using System.Collections.Generic;
    using MBF.Algorithms.Alignment;
    using MBF.Algorithms.SuffixTree;
    using MBF.Util.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test for Suffix Tree Algorithm
    /// </summary>
    [TestClass]
    public class SuffixTreeTests
    {
        /// <summary>
        /// Test Kurtz Suffix Tree builder algorithm for simple sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestSequence()
        {
            string sequenceString = "BANANA";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);
            ISuffixTreeBuilder kurtzSuffixTreeBuilder = new KurtzSuffixTreeBuilder();

            ApplicationLog.WriteLine("Begin SuffixTree Test for string '{0}'", sequenceString);
            ApplicationLog.WriteTime("Start Time.", DateTime.Now.ToString());
            SequenceSuffixTree kurtzSuffixTree = kurtzSuffixTreeBuilder.BuildSuffixTree(sequence) as SequenceSuffixTree;
            ApplicationLog.WriteTime("End Time.", DateTime.Now.ToString());

            // Verify the edges in Suffix Tree
            Assert.AreEqual(10, kurtzSuffixTree.Edges.Count);

            // Verify the sequence in Suffix Tree
            Assert.AreEqual(kurtzSuffixTree.Sequence.ToString(), sequenceString);
        }

        /// <summary>
        /// Test Kurtz Suffix Tree builder algorithm for segmented sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestSegmentedSequence()
        {
            string sequenceString = "ATATTGCCG";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);
            SegmentedSequence segmentedSequece = new SegmentedSequence(sequence);

            sequenceString = "BANANA";
            sequence = new Sequence(Alphabets.Protein, sequenceString);
            segmentedSequece.Sequences.Add(sequence);

            ISuffixTreeBuilder kurtzSuffixTreeBuilder = new KurtzSuffixTreeBuilder();

            ApplicationLog.WriteLine("Begin SuffixTree Test for string '{0}'", segmentedSequece.ToString());
            ApplicationLog.WriteTime("Start Time.", DateTime.Now.ToString());
            SequenceSuffixTree kurtzSuffixTree = kurtzSuffixTreeBuilder.BuildSuffixTree(segmentedSequece) as SequenceSuffixTree;
            ApplicationLog.WriteTime("End Time.", DateTime.Now.ToString());

            // Verify the edges in Suffix Tree
            Assert.AreEqual(24, kurtzSuffixTree.Edges.Count);

            // Verify the sequence in Suffix Tree
            Assert.AreEqual(kurtzSuffixTree.Sequence.ToString(), segmentedSequece.ToString());
        }

        /// <summary>
        /// Test streaming for simple sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestStreamingInSequence()
        {
            string sequenceString = "AGTATGCCCCCCCCCCTGCCG";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);

            ApplicationLog.WriteLine("Begin SuffixTree Test for string '{0}'", sequenceString);
            ISuffixTreeBuilder kurtzSuffixTreeBuilder = new KurtzSuffixTreeBuilder();
            SequenceSuffixTree kurtzSuffixTree = kurtzSuffixTreeBuilder.BuildSuffixTree(sequence) as SequenceSuffixTree;

            string queryString = "CCCCCCCCTATG";
            Sequence querySequence = new Sequence(Alphabets.Protein, queryString);

            ApplicationLog.WriteLine("Query string : {0}. Minimum Length of MUM : 3.", queryString);
            ApplicationLog.WriteTime("Start Time.", DateTime.Now.ToString());
            IList<MaxUniqueMatch> MUMs = kurtzSuffixTreeBuilder.FindMatches(kurtzSuffixTree, querySequence, 3);
            ApplicationLog.WriteTime("End Time.", DateTime.Now.ToString());

            // Verify the count of MUMs found
            Assert.AreEqual(2, MUMs.Count);
        }

        /// <summary>
        /// Test streaming for simple sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestStreamingInSegmentedSequence()
        {
            string sequenceString = "AAATTGGC";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);
            SegmentedSequence segmentedSequece = new SegmentedSequence(sequence);

            sequenceString = "ANANA";
            sequence = new Sequence(Alphabets.Protein, sequenceString);
            segmentedSequece.Sequences.Add(sequence);

            ISuffixTreeBuilder kurtzSuffixTreeBuilder = new KurtzSuffixTreeBuilder();

            ApplicationLog.WriteLine("Begin SuffixTree Test for string '{0}'", segmentedSequece.ToString());
            SequenceSuffixTree kurtzSuffixTree = kurtzSuffixTreeBuilder.BuildSuffixTree(segmentedSequece) as SequenceSuffixTree;

            string queryString = "AATTNANAGGC";
            Sequence querySequence = new Sequence(Alphabets.Protein, queryString);

            ApplicationLog.WriteLine("Query string : {0}. Minimum Length of MUM : 3.", queryString);
            ApplicationLog.WriteTime("Start Time.", DateTime.Now.ToString());
            IList<MaxUniqueMatch> MUMs = kurtzSuffixTreeBuilder.FindMatches(kurtzSuffixTree, querySequence, 3);
            ApplicationLog.WriteTime("End Time.", DateTime.Now.ToString());

            // Verify the count of MUMs found
            Assert.AreEqual(3, MUMs.Count);
        }

        /// <summary>
        /// Test streaming for simple sequence with findmaximummatch option
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestFindMaximumMatchInSequence()
        {
            string sequenceString = "BANANA";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);

            ApplicationLog.WriteLine("Begin SuffixTree Test for string '{0}'", sequenceString);
            ISuffixTreeBuilder kurtzSuffixTreeBuilder = new KurtzSuffixTreeBuilder();
            ISuffixTree kurtzSuffixTree = kurtzSuffixTreeBuilder.BuildSuffixTree(sequence);

            string queryString = "ANA";
            Sequence querySequence = new Sequence(Alphabets.Protein, queryString);

            ApplicationLog.WriteLine("Query string : {0}. Minimum Length of MUM : 3.", queryString);
            ApplicationLog.WriteTime("Start Time.", DateTime.Now.ToString());
            IList<MaxUniqueMatch> MUMs = kurtzSuffixTreeBuilder.FindMaximumMatches(kurtzSuffixTree, querySequence, 3);
            ApplicationLog.WriteTime("End Time.", DateTime.Now.ToString());

            // Verify the count of MUMs found
            Assert.AreEqual(1, MUMs.Count);
        }
    }
}