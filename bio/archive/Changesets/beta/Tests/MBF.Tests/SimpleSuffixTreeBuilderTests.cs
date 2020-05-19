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
    using System.Collections.Generic;
    using MBF.Algorithms.Alignment;
    using MBF.Algorithms.SuffixTree;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test for Simple Suffix Tree Algorithm
    /// </summary>
    [TestClass]
    public class SimpleSuffixTreeBuilderTests
    {
        /// <summary>
        /// Test Simple Suffix Tree builder algorithm for simple sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestInMemorySimpleSequence()
        {
            string sequenceString = "BANANA";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);
            using (SimpleSuffixTreeBuilder simpleSuffixTreeBuilder = new SimpleSuffixTreeBuilder())
            {
                IMultiWaySuffixTree inMemorySuffixTree = simpleSuffixTreeBuilder.BuildSuffixTree(sequence) as IMultiWaySuffixTree;

                // Verify the edges in Suffix Tree
                Assert.AreEqual(7, inMemorySuffixTree.Count);

                // Verify the sequence in Suffix Tree
                Assert.AreEqual(inMemorySuffixTree.Sequence.ToString(), sequenceString);
            }
        }

        /// <summary>
        /// Test Simple Suffix Tree builder algorithm for segmented sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestInMemorySegmentedSequence()
        {
            string sequenceString = "ATATTGCCG";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);
            SegmentedSequence segmentedSequece = new SegmentedSequence(sequence);

            sequenceString = "BANANA";
            sequence = new Sequence(Alphabets.Protein, sequenceString);
            segmentedSequece.Sequences.Add(sequence);

            using (SimpleSuffixTreeBuilder simpleSuffixTreeBuilder = new SimpleSuffixTreeBuilder())
            {
                IMultiWaySuffixTree inMemorySuffixTree = simpleSuffixTreeBuilder.BuildSuffixTree(segmentedSequece) as IMultiWaySuffixTree;

                // Verify the edges in Suffix Tree
                Assert.AreEqual(21, inMemorySuffixTree.Count);

                // Verify the sequence in Suffix Tree
                Assert.AreEqual(inMemorySuffixTree.Sequence.ToString(), segmentedSequece.ToString());
            }
        }

        /// <summary>
        /// Test streaming for simple sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestStreamingInMemorySimpleSequence()
        {
            string sequenceString = "AGTATGCCCCCCCCCCTGCCG";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);

            using (SimpleSuffixTreeBuilder simpleSuffixTreeBuilder = new SimpleSuffixTreeBuilder())
            {
                ISuffixTree inMemorySuffixTree = simpleSuffixTreeBuilder.BuildSuffixTree(sequence);

                string queryString = "CCCCCCCCTATG";
                Sequence querySequence = new Sequence(Alphabets.Protein, queryString);

                IList<MaxUniqueMatch> MUMs = simpleSuffixTreeBuilder.FindMatches(inMemorySuffixTree, querySequence, 3);

                // Verify the count of MUMs found
                Assert.AreEqual(2, MUMs.Count);
            }
        }

        /// <summary>
        /// Test streaming for simple sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestStreamingInMemorySegmentedSequence()
        {
            string sequenceString = "AAATTGGC";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);
            SegmentedSequence segmentedSequece = new SegmentedSequence(sequence);

            sequenceString = "ANANA";
            sequence = new Sequence(Alphabets.Protein, sequenceString);
            segmentedSequece.Sequences.Add(sequence);

            using (SimpleSuffixTreeBuilder simpleSuffixTreeBuilder = new SimpleSuffixTreeBuilder())
            {
                ISuffixTree inMemorySuffixTree = simpleSuffixTreeBuilder.BuildSuffixTree(segmentedSequece);

                string queryString = "AATTNANAGGC";
                Sequence querySequence = new Sequence(Alphabets.Protein, queryString);

                IList<MaxUniqueMatch> MUMs = simpleSuffixTreeBuilder.FindMatches(inMemorySuffixTree, querySequence, 3);

                // Verify the count of MUMs found
                Assert.AreEqual(3, MUMs.Count);
            }
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

            using (SimpleSuffixTreeBuilder simpleSuffixTreebldr = new SimpleSuffixTreeBuilder())
            {
                ISuffixTreeBuilder simpleSuffixTreeBuilder = simpleSuffixTreebldr;
                ISuffixTree simpleSuffixTree = simpleSuffixTreeBuilder.BuildSuffixTree(sequence);

                string queryString = "ANA";
                Sequence querySequence = new Sequence(Alphabets.Protein, queryString);

                IList<MaxUniqueMatch> MUMs = simpleSuffixTreeBuilder.FindMaximumMatches(simpleSuffixTree, querySequence, 3);

                // Verify the count of MUMs found
                Assert.AreEqual(1, MUMs.Count);
                simpleSuffixTreeBuilder = null;
            }
        }

        /// <summary>
        /// Test Simple Suffix Tree builder algorithm for simple sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestPersistentSimpleSequence()
        {
            string sequenceString = "BANANA";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);
            using (SimpleSuffixTreeBuilder simpleSuffixTreeBuilder = new SimpleSuffixTreeBuilder())
            {
                simpleSuffixTreeBuilder.PersistenceThreshold = 0;

                IMultiWaySuffixTree persistentSuffixTree = simpleSuffixTreeBuilder.BuildSuffixTree(sequence) as IMultiWaySuffixTree;

                // Verify the edges in Suffix Tree
                Assert.AreEqual(7, persistentSuffixTree.Count);

                // Verify the sequence in Suffix Tree
                Assert.AreEqual(persistentSuffixTree.Sequence.ToString(), sequenceString);
            }
        }

        /// <summary>
        /// Test Simple Suffix Tree builder algorithm for segmented sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestPersistentSegmentedSequence()
        {
            string sequenceString = "ATATTGCCG";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);
            SegmentedSequence segmentedSequece = new SegmentedSequence(sequence);

            sequenceString = "BANANA";
            sequence = new Sequence(Alphabets.Protein, sequenceString);
            segmentedSequece.Sequences.Add(sequence);

            using (SimpleSuffixTreeBuilder simpleSuffixTreeBuilder = new SimpleSuffixTreeBuilder())
            {
                simpleSuffixTreeBuilder.PersistenceThreshold = 0;

                IMultiWaySuffixTree persistentSuffixTree = simpleSuffixTreeBuilder.BuildSuffixTree(segmentedSequece) as IMultiWaySuffixTree;

                // Verify the edges in Suffix Tree
                Assert.AreEqual(21, persistentSuffixTree.Count);

                // Verify the sequence in Suffix Tree
                Assert.AreEqual(persistentSuffixTree.Sequence.ToString(), segmentedSequece.ToString());
            }
        }

        /// <summary>
        /// Test streaming for simple sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestStreamingPersistentSimpleSequence()
        {
            string sequenceString = "AGTATGCCCCCCCCCCTGCCG";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);

            using (SimpleSuffixTreeBuilder simpleSuffixTreeBuilder = new SimpleSuffixTreeBuilder())
            {
                simpleSuffixTreeBuilder.PersistenceThreshold = 0;

                ISuffixTree persistentSuffixTree = simpleSuffixTreeBuilder.BuildSuffixTree(sequence);

                string queryString = "CCCCCCCCTATG";
                Sequence querySequence = new Sequence(Alphabets.Protein, queryString);

                IList<MaxUniqueMatch> MUMs = simpleSuffixTreeBuilder.FindMatches(persistentSuffixTree, querySequence, 3);

                // Verify the count of MUMs found
                Assert.AreEqual(2, MUMs.Count);
            }
        }

        /// <summary>
        /// Test streaming for simple sequence
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestStreamingPersistentSegmentedSequence()
        {
            string sequenceString = "AAATTGGC";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);
            SegmentedSequence segmentedSequece = new SegmentedSequence(sequence);

            sequenceString = "ANANA";
            sequence = new Sequence(Alphabets.Protein, sequenceString);
            segmentedSequece.Sequences.Add(sequence);

            using (SimpleSuffixTreeBuilder simpleSuffixTreeBuilder = new SimpleSuffixTreeBuilder())
            {
                simpleSuffixTreeBuilder.PersistenceThreshold = 0;

                ISuffixTree persistentSuffixTree = simpleSuffixTreeBuilder.BuildSuffixTree(segmentedSequece);

                string queryString = "AATTNANAGGC";
                Sequence querySequence = new Sequence(Alphabets.Protein, queryString);

                IList<MaxUniqueMatch> MUMs = simpleSuffixTreeBuilder.FindMatches(persistentSuffixTree, querySequence, 3);

                // Verify the count of MUMs found
                Assert.AreEqual(3, MUMs.Count);
            }
        }

        /// <summary>
        /// Test streaming for simple sequence with findmaximummatch option
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestFindMaximumMatchPersistentInSequence()
        {
            string sequenceString = "BANANA";
            Sequence sequence = new Sequence(Alphabets.Protein, sequenceString);

            using (SimpleSuffixTreeBuilder simpleSuffixTreeBuilder = new SimpleSuffixTreeBuilder())
            {
                simpleSuffixTreeBuilder.PersistenceThreshold = 0;
                ISuffixTree simpleSuffixTree = simpleSuffixTreeBuilder.BuildSuffixTree(sequence);

                string queryString = "ANA";
                Sequence querySequence = new Sequence(Alphabets.Protein, queryString);

                IList<MaxUniqueMatch> MUMs = simpleSuffixTreeBuilder.FindMaximumMatches(simpleSuffixTree, querySequence, 3);

                // Verify the count of MUMs found
                Assert.AreEqual(1, MUMs.Count);
            }
        }
    }
}