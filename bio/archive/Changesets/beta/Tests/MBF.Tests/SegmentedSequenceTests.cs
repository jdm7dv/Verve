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

    using MBF.Util;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Class to test SegmentedSequence.
    /// </summary>
    [TestClass]
    public class SegmentedSequenceTests
    {
        /// <summary>
        /// Test constructors with invalid input values.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestConstructorsWithInvalidParameters()
        {
            SegmentedSequence segmntSeq = null;
            ISequence seq = null;

            // Create a sequence to use in constructor of segmented sequence.
            seq = new Sequence(Alphabets.DNA);
            seq.InsertRange(0, "GAATC");

            try
            {
                segmntSeq = new SegmentedSequence(null as ISequence);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                Assert.IsNull(segmntSeq);
            }

            try
            {
                segmntSeq = new SegmentedSequence(null as IList<ISequence>);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                Assert.IsNull(segmntSeq);
            }

            List<ISequence> seqList = new List<ISequence>();
            ISequence seq1 = null;
            ISequence seq2 = new Sequence(Alphabets.DNA, "CGTATGGC");
            ISequence seq3 = new Sequence(Alphabets.DNA, "GGGTAA");

            seqList.Add(seq1);
            seqList.Add(seq2);
            seqList.Add(seq3);

            try
            {
                segmntSeq = new SegmentedSequence(seqList);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.IsNull(segmntSeq);
            }

            seqList = new List<ISequence>();
            seq1 = new Sequence(Alphabets.DNA, "ATGC");
            seq2 = null;
            seq3 = new Sequence(Alphabets.RNA, "GGGCAA");

            seqList.Add(seq1);
            seqList.Add(seq2);
            seqList.Add(seq3);

            try
            {
                segmntSeq = new SegmentedSequence(seqList);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.IsNull(segmntSeq);
            }

            seqList = new List<ISequence>();
            seq1 = new Sequence(Alphabets.DNA, "ATGC");
            seq2 = new Sequence(Alphabets.RNA, "CGCACGGC");
            seq3 = new Sequence(Alphabets.DNA, "GGGTAA");

            seqList.Add(seq1);
            seqList.Add(seq2);
            seqList.Add(seq3);

            try
            {
                segmntSeq = new SegmentedSequence(seqList);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.IsNull(segmntSeq);
            }

        }

        /// <summary>
        /// Test constructors with valid input values.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestConstructorsWithValidParametetrs()
        {
            SegmentedSequence segmntSeq = null;
            ISequence seq = null;

            seq = new Sequence(Alphabets.DNA);
            seq.InsertRange(0, "GAATC");
            segmntSeq = new SegmentedSequence(seq);
            Assert.AreEqual(segmntSeq.Count, 5);
            Assert.AreEqual(segmntSeq.Sequences.Count, 1);
            Assert.AreSame(seq, segmntSeq.Sequences[0]);
            Assert.AreEqual(seq.ToString(), segmntSeq.ToString());

            List<ISequence> seqList = new List<ISequence>();
            ISequence seq1 = new Sequence(Alphabets.DNA, "ATGC");
            ISequence seq2 = new Sequence(Alphabets.DNA, "CGTATGGC");
            ISequence seq3 = new Sequence(Alphabets.DNA, "GGGTAA");
            seqList.Add(seq1);
            seqList.Add(seq2);
            seqList.Add(seq3);
            seqList.Add(seq1);

            segmntSeq = new SegmentedSequence(seqList);
            Assert.AreEqual(segmntSeq.Count, 22);
            Assert.AreEqual(segmntSeq.Sequences.Count, 4);
            Assert.AreSame(seq1, segmntSeq.Sequences[0]);
            Assert.AreSame(seq2, segmntSeq.Sequences[1]);
            Assert.AreSame(seq3, segmntSeq.Sequences[2]);
            Assert.AreSame(seq1, segmntSeq.Sequences[3]);

            Assert.AreEqual(segmntSeq.ToString(), "ATGCCGTATGGCGGGTAAATGC");
        }

        /// <summary>
        /// Test editing when IsReadOnly flag is true.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestEditingWhenIsReadOnlyIsTrue()
        {

            // IsReadOnly property of a segmented sequence returns true if IsReadOnly property of any one 
            // of the underlying sequence is true.

            List<ISequence> seqList = new List<ISequence>();
            ISequence seq1 = new Sequence(Alphabets.DNA); // By default IsReadOnly is false.
            seq1.InsertRange(0, "ATGC");
            ISequence seq2 = new Sequence(Alphabets.DNA); // By default IsReadOnly is false.
            seq2.InsertRange(0, "CGTATGGC");
            ISequence seq3 = new Sequence(Alphabets.DNA, "GGGTAA"); // By default IsReadOnly is true.
            seqList.Add(seq1);
            seqList.Add(seq2);
            seqList.Add(seq3);
            seqList.Add(seq1);

            SegmentedSequence segmntSeq = new SegmentedSequence(seqList);
            Assert.IsFalse(seq1.IsReadOnly);
            Assert.IsFalse(seq2.IsReadOnly);
            Assert.IsTrue(seq3.IsReadOnly);

            try
            {
                segmntSeq.Add(Alphabets.DNA.A);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(segmntSeq.Count, 22);
            }

            try
            {
                segmntSeq.Clear();
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(segmntSeq.Count, 22);
            }

            try
            {
                segmntSeq.Insert(0, 'C');
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(segmntSeq.Count, 22);
            }

            try
            {
                segmntSeq.Insert(0, Alphabets.DNA.A);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(segmntSeq.Count, 22);
            }

            try
            {
                segmntSeq.InsertRange(0, "CGA");
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(segmntSeq.Count, 22);
            }

            try
            {
                segmntSeq.Remove(Alphabets.DNA.A);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(segmntSeq.Count, 22);
            }

            try
            {
                segmntSeq.RemoveAt(0);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(segmntSeq.Count, 22);
            }

            try
            {
                segmntSeq.RemoveRange(0, 2);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(segmntSeq.Count, 22);
            }

            try
            {
                segmntSeq.Replace(0, 'C');
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(segmntSeq.Count, 22);
                Assert.AreSame(segmntSeq[0], Alphabets.DNA.A);
            }

            try
            {
                segmntSeq.Replace(0, Alphabets.DNA.C);
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(segmntSeq.Count, 22);
                Assert.AreSame(segmntSeq[0], Alphabets.DNA.A);
            }

            try
            {
                segmntSeq.ReplaceRange(0, "G");
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
                Assert.AreEqual(segmntSeq.Count, 22);
                Assert.AreSame(segmntSeq[0], Alphabets.DNA.A);
            }
        }

        /// <summary>
        /// Test editing when IsReadOnly flag is false.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestEditingwhenIsReadOnlyIsFalse()
        {
            Sequence seq = new Sequence(Alphabets.DNA);
            seq.Insert(0, Alphabets.DNA.A);
            SegmentedSequence segmntSeq = new SegmentedSequence(seq);

            // Test - void Add(ISequenceItem)
            ValidateSegSeqAdd(segmntSeq);

            // Test - void Clear()
            ValidateSegSeqClear(segmntSeq);

            // Test - void void Insert(int position, char character)
            ValidateSegSeqInsert(segmntSeq);

            // Test - void void Insert(int index, ISequenceItem item)
            ValidateSegSeqInsertSeqItem(segmntSeq);

            // Test - void void InsertRange(int position, string sequence)
            ValidateSegSeqInsertRange(segmntSeq);

            // Test - void void Remove(ISequenceItem item)
            ValidateSegSeqRemove(segmntSeq);

            // Test - void void RemoveAt(int index)
            ValidateSegSeqRemoveAt(segmntSeq);

            // Test - void void  RemoveRange(int position, int length)
            ValidateSegSeqRemoveRange(segmntSeq);

            // Test - void void  Replace(int position, char character)
            ValidateSegSeqReplace(segmntSeq);

            // Test - void void  Replace(int position, IsequenceItem item)
            ValidateSegSeqReplaceSeqItem(segmntSeq);

            // Test - void void ReplaceRange(int position, string sequence)
            ValidateSegSeqReplaceRange(segmntSeq);

            #region Test - ISequenceItem this[int index]

            Assert.AreEqual(segmntSeq.ToString(), "CGTGC");

            segmntSeq[0] = Alphabets.DNA.A;
            Assert.AreEqual(segmntSeq.Count, 5);
            Assert.AreSame(segmntSeq[0], Alphabets.DNA.A);
            Assert.AreSame(segmntSeq[1], Alphabets.DNA.G);
            Assert.AreSame(segmntSeq[2], Alphabets.DNA.T);
            Assert.AreSame(segmntSeq[3], Alphabets.DNA.G);
            Assert.AreSame(segmntSeq[4], Alphabets.DNA.C);

            try
            {
                segmntSeq[-1] = Alphabets.DNA.A;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            try
            {
                segmntSeq[segmntSeq.Count] = Alphabets.DNA.A;
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            Assert.AreEqual(segmntSeq.Count, 5);
            Assert.AreSame(segmntSeq[0], Alphabets.DNA.A);
            Assert.AreSame(segmntSeq[1], Alphabets.DNA.G);
            Assert.AreSame(segmntSeq[2], Alphabets.DNA.T);
            Assert.AreSame(segmntSeq[3], Alphabets.DNA.G);
            Assert.AreSame(segmntSeq[4], Alphabets.DNA.C);
            Assert.AreEqual(segmntSeq.ToString(), "AGTGC");

            #endregion Test - ISequenceItem this[int index]

            #region Test - IEnumerator<ISequenceItem> GetEnumerator()
            Assert.AreEqual(segmntSeq.ToString(), "AGTGC");
            int i = 0;
            foreach (ISequenceItem item in segmntSeq)
            {
                Assert.AreSame(segmntSeq[i++], item);
            }
            #endregion Test - IEnumerator<ISequenceItem> GetEnumerator()
        }

        /// <summary>
        /// Test creating segmented sequence by passing segmented sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestNestedSegmentedSequence()
        {
            List<ISequence> seqList = new List<ISequence>();
            ISequence seq1 = new Sequence(Alphabets.DNA, "ATGC");
            ISequence seq2 = new Sequence(Alphabets.DNA, "CGTATGGC");
            seqList.Add(seq1);
            seqList.Add(seq2);
            seqList.Add(seq1);

            SegmentedSequence innerSegmntSeq = new SegmentedSequence(seqList);
            Assert.AreEqual(innerSegmntSeq.ToString(), "ATGCCGTATGGCATGC");

            SegmentedSequence segmntSeq = new SegmentedSequence(innerSegmntSeq);
            ISequence seq3 = new Sequence(Alphabets.DNA, "GGGTAA");
            segmntSeq.Sequences.Add(seq3);
            Assert.AreEqual(segmntSeq.ToString(), "ATGCCGTATGGCATGCGGGTAA");
        }

        #region Heper Methods

        /// <summary>
        /// Validate Segmented seq Add().
        /// </summary>
        /// <param name="segmntSeq">Segmented sequence</param>
        private static void ValidateSegSeqAdd(SegmentedSequence segmntSeq)
        {
            segmntSeq.Add(Alphabets.DNA.G);
            Assert.AreEqual(segmntSeq.Count, 2);
            Assert.AreSame(segmntSeq[0], Alphabets.DNA.A);
            Assert.AreSame(segmntSeq[1], Alphabets.DNA.G);

            try
            {
                segmntSeq.Add(Alphabets.RNA.U);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.AreEqual(segmntSeq.Count, 2);
            }

        }

        /// <summary>
        /// Validate Segmented seq Clear().
        /// </summary>
        /// <param name="segmntSeq">Segmented sequence</param>
        private static void ValidateSegSeqClear(SegmentedSequence segmntSeq)
        {
            segmntSeq.Clear();
            Assert.AreEqual(segmntSeq.Count, 0);
        }

        /// <summary>
        /// Validate Segmented seq Insert().
        /// </summary>
        /// <param name="segmntSeq">Segmented sequence</param>
        private static void ValidateSegSeqInsert(SegmentedSequence segmntSeq)
        {
            segmntSeq.Insert(0, 'C');
            Assert.AreEqual(segmntSeq.Count, 1);

            segmntSeq.Insert(segmntSeq.Count, 'A');
            Assert.AreEqual(segmntSeq.Count, 2);

            try
            {
                segmntSeq.Insert(-1, 'A');
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.AreEqual(segmntSeq.Count, 2);
            }

            try
            {
                segmntSeq.Insert(segmntSeq.Count + 1, 'A');
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.AreEqual(segmntSeq.Count, 2);
            }

            try
            {
                segmntSeq.Insert(0, 'U');
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.AreEqual(segmntSeq.Count, 2);
            }
        }

        /// <summary>
        /// Validate Segmented seq Insert().
        /// </summary>
        /// <param name="segmntSeq">Segmented sequence</param>
        private static void ValidateSegSeqInsertSeqItem(SegmentedSequence segmntSeq)
        {
            segmntSeq.Insert(0, Alphabets.DNA.G);
            Assert.AreEqual(segmntSeq.Count, 3);

            segmntSeq.Insert(segmntSeq.Count, Alphabets.DNA.G);
            Assert.AreEqual(segmntSeq.Count, 4);

            Assert.AreEqual(segmntSeq.ToString(), "GCAG");

            try
            {
                segmntSeq.Insert(-1, Alphabets.DNA.A);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.AreEqual(segmntSeq.Count, 4);
            }

            try
            {
                segmntSeq.Insert(segmntSeq.Count + 1, Alphabets.DNA.A);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.AreEqual(segmntSeq.Count, 4);
            }

            try
            {
                segmntSeq.Insert(0, Alphabets.RNA.U);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.AreEqual(segmntSeq.Count, 4);
            }
        }

        /// <summary>
        /// Validate Segmented seq InsertRange().
        /// </summary>
        /// <param name="segmntSeq">Segmented sequence</param>
        private static void ValidateSegSeqInsertRange(SegmentedSequence segmntSeq)
        {
            Assert.AreEqual(segmntSeq.ToString(), "GCAG");

            segmntSeq.InsertRange(0, "CGA");
            Assert.AreEqual(segmntSeq.Count, 7);
            Assert.AreEqual(segmntSeq.ToString(), "CGAGCAG");
            try
            {
                segmntSeq.InsertRange(-1, "CGA");
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(segmntSeq.Count, 7);
            }

            try
            {
                segmntSeq.InsertRange(segmntSeq.Count + 1, "CGA");
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(segmntSeq.Count, 7);
            }

            try
            {
                segmntSeq.InsertRange(0, string.Empty);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                Assert.AreEqual(segmntSeq.Count, 7);
            }

            try
            {
                segmntSeq.InsertRange(0, null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                Assert.AreEqual(segmntSeq.Count, 7);
            }
        }

        /// <summary>
        /// Validate Segmented seq Remove().
        /// </summary>
        /// <param name="segmntSeq">Segmented sequence</param>
        private static void ValidateSegSeqRemove(SegmentedSequence segmntSeq)
        {
            Assert.AreEqual(segmntSeq.ToString(), "CGAGCAG");

            Assert.IsTrue(segmntSeq.Remove(Alphabets.DNA.A));
            Assert.AreEqual(segmntSeq.Count, 6);
            Assert.AreEqual(segmntSeq.ToString(), "CGGCAG");

            Assert.IsFalse(segmntSeq.Remove(Alphabets.DNA.T));

            Assert.IsFalse(segmntSeq.Remove(Alphabets.RNA.U));
            Assert.AreEqual(segmntSeq.Count, 6);
            Assert.AreEqual(segmntSeq.ToString(), "CGGCAG");
        }

        /// <summary>
        /// Validate Segmented seq RemoveAt().
        /// </summary>
        /// <param name="segmntSeq">Segmented sequence</param>
        private static void ValidateSegSeqRemoveAt(SegmentedSequence segmntSeq)
        {
            Assert.AreEqual(segmntSeq.ToString(), "CGGCAG");
            segmntSeq.RemoveAt(2);
            Assert.AreEqual(segmntSeq.Count, 5);
            Assert.AreEqual(segmntSeq.ToString(), "CGCAG");

            try
            {
                segmntSeq.RemoveAt(-1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(segmntSeq.Count, 5);
            }

            try
            {
                segmntSeq.RemoveAt(segmntSeq.Count);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(segmntSeq.Count, 5);
            }
        }

        /// <summary>
        /// Validate Segmented seq RemoveRange().
        /// </summary>
        /// <param name="segmntSeq">Segmented sequence</param>
        private static void ValidateSegSeqRemoveRange(SegmentedSequence segmntSeq)
        {
            segmntSeq.RemoveRange(1, 3);
            Assert.AreEqual(segmntSeq.Count, 2);
            Assert.AreEqual(segmntSeq.ToString(), "CG");

            try
            {
                segmntSeq.RemoveRange(-1, 1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(segmntSeq.Count, 2);
            }

            try
            {
                segmntSeq.RemoveRange(segmntSeq.Count, 1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(segmntSeq.Count, 2);
            }

            try
            {
                segmntSeq.RemoveRange(0, -1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(segmntSeq.Count, 2);
            }

            try
            {
                segmntSeq.RemoveRange(0, 4);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.AreEqual(segmntSeq.Count, 2);
            }

        }

        /// <summary>
        /// Validate Segmented seq Replace().
        /// </summary>
        /// <param name="segmntSeq">Segmented sequence</param>
        private static void ValidateSegSeqReplace(SegmentedSequence segmntSeq)
        {
            Assert.AreEqual(segmntSeq.ToString(), "CG");
            segmntSeq.InsertRange(segmntSeq.Count, "CGA");

            segmntSeq.Replace(3, 'A');
            Assert.AreEqual(segmntSeq.Count, 5);
            Assert.AreEqual(segmntSeq.ToString(), "CGCAA");
            Assert.AreSame(segmntSeq[0], Alphabets.DNA.C);
            Assert.AreSame(segmntSeq[1], Alphabets.DNA.G);
            Assert.AreSame(segmntSeq[2], Alphabets.DNA.C);
            Assert.AreSame(segmntSeq[3], Alphabets.DNA.A);
            Assert.AreSame(segmntSeq[4], Alphabets.DNA.A);

            segmntSeq.Replace(2, 'T');
            Assert.AreEqual(segmntSeq.Count, 5);
            Assert.AreSame(segmntSeq[0], Alphabets.DNA.C);
            Assert.AreSame(segmntSeq[1], Alphabets.DNA.G);
            Assert.AreSame(segmntSeq[2], Alphabets.DNA.T);
            Assert.AreSame(segmntSeq[3], Alphabets.DNA.A);
            Assert.AreSame(segmntSeq[4], Alphabets.DNA.A);
            Assert.AreEqual(segmntSeq.ToString(), "CGTAA");

            try
            {
                segmntSeq.Replace(-1, 'C');
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(segmntSeq.Count, 5);
            }

            try
            {
                segmntSeq.Replace(segmntSeq.Count, 'C');
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(segmntSeq.Count, 5);
            }

            try
            {
                segmntSeq.Replace(2, 'U');
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.AreEqual(segmntSeq.Count, 5);
            }
        }

        /// <summary>
        /// Validate Segmented seq ReplaceSeqItem().
        /// </summary>
        /// <param name="segmntSeq">Segmented sequence</param>
        private static void ValidateSegSeqReplaceSeqItem(SegmentedSequence segmntSeq)
        {
            segmntSeq.Replace(3, Alphabets.DNA.T);
            Assert.AreEqual(segmntSeq.Count, 5);
            Assert.AreEqual(segmntSeq.ToString(), "CGTTA");

            try
            {
                segmntSeq.Replace(-1, Alphabets.DNA.A);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {

                Assert.AreEqual(segmntSeq.ToString(), "CGTTA");
            }

            try
            {
                segmntSeq.Replace(segmntSeq.Count, Alphabets.DNA.A);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(segmntSeq.ToString(), "CGTTA");
            }

            try
            {
                segmntSeq.Replace(2, Alphabets.RNA.U);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                Assert.AreEqual(segmntSeq.ToString(), "CGTTA");
            }
        }

        /// <summary>
        /// Validate Segmented seq ReplaceRange().
        /// </summary>
        /// <param name="segmntSeq">Segmented sequence</param>
        private static void ValidateSegSeqReplaceRange(SegmentedSequence segmntSeq)
        {

            try
            {
                segmntSeq.ReplaceRange(-1, "GC");
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(segmntSeq.ToString(), "CGTTA");
            }

            try
            {
                segmntSeq.ReplaceRange(segmntSeq.Count, "GC");
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
                Assert.AreEqual(segmntSeq.ToString(), "CGTTA");
            }

            try
            {
                segmntSeq.ReplaceRange(0, string.Empty);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                Assert.AreEqual(segmntSeq.ToString(), "CGTTA");
            }

            try
            {
                segmntSeq.ReplaceRange(0, null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                Assert.AreEqual(segmntSeq.ToString(), "CGTTA");
            }

            segmntSeq.ReplaceRange(3, "GC");
            Assert.AreEqual(segmntSeq.Count, 5);
            Assert.AreEqual(segmntSeq.ToString(), "CGTGC");
        }
        #endregion Helper Methods
    }
}
