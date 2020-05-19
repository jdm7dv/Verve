// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Test
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// Class to test the VirtualSequence.
    /// </summary>
    [TestFixture]
    public class VirtualSequenceTest
    {
        /// <summary>
        /// Test virtual sequence class.
        /// </summary>
        [Test]
        public void TestVirtualSequence()
        {
            VirtualSequence virtualSeq = new VirtualSequence(Alphabets.DNA);
            string id = Guid.NewGuid().ToString(string.Empty);

            virtualSeq.ID = id;
            Assert.AreEqual(virtualSeq.Alphabet, Alphabets.DNA);
            Assert.AreEqual(virtualSeq.Count, 0);
            Assert.AreEqual(virtualSeq.ID, id);

            // By default Displayid should show value of id.
            Assert.AreEqual(virtualSeq.DisplayID, virtualSeq.ID);

            virtualSeq.DisplayID = "VirtualSeq1";
            Assert.AreEqual(virtualSeq.DisplayID, "VirtualSeq1");
            Assert.AreNotEqual(virtualSeq.DisplayID, virtualSeq.ID);
            Assert.AreEqual(virtualSeq.IsReadOnly, true);
            Assert.AreEqual(virtualSeq.IndexOf(Alphabets.DNA.A), -1);

            foreach (Nucleotide nucleotide in Alphabets.DNA)
            {
                Assert.AreEqual(virtualSeq.IndexOf(nucleotide), -1);
                Assert.IsFalse(virtualSeq.Contains(nucleotide));
            }

            // Verify the above test using ISequence interface.
            ISequence seq = virtualSeq as ISequence;
            Assert.AreEqual(seq.Alphabet, Alphabets.DNA);
            Assert.AreEqual(seq.Count, 0);
            Assert.AreEqual(seq.ID, id);
            Assert.AreNotEqual(seq.DisplayID, virtualSeq.ID);
            Assert.AreEqual(seq.DisplayID, "VirtualSeq1");
            Assert.AreEqual(seq.IsReadOnly, true);

            foreach (Nucleotide nucleotide in Alphabets.DNA)
            {
                Assert.AreEqual(seq.IndexOf(nucleotide), -1);
                Assert.IsFalse(seq.Contains(nucleotide));
            }

            #region Test not supported members

            try
            {
                seq.Add(Alphabets.DNA.A);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.AreEqual(seq.Count, 0);
            }
            try
            {
                seq.Clear();
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.AreEqual(seq.Count, 0);
            }

            ISequence tmpSeq = null;

            try
            {
                tmpSeq = seq.Complement;
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.IsNull(tmpSeq);
            }

            try
            {
                ISequenceItem[] seqItems = new ISequenceItem[10];
                seq.CopyTo(seqItems, 0);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
            }

            try
            {
                seq.Insert(0, 'A');
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.AreEqual(seq.Count, 0);
            }

            try
            {
                seq.InsertRange(0, "ACG");
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.AreEqual(seq.Count, 0);
            }

            try
            {
                tmpSeq = seq.Range(0, 1);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.IsNull(tmpSeq);
            }

            try
            {
                seq.Remove(Alphabets.DNA.A);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.IsNull(tmpSeq);
            }

            try
            {
                seq.RemoveAt(0);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.AreEqual(seq.Count, 0);
            }

            try
            {
                seq.RemoveRange(0, 1);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.AreEqual(seq.Count, 0);
            }

            try
            {
                seq.Replace(0, Alphabets.DNA.A.Symbol);
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.AreEqual(seq.IndexOf(Alphabets.DNA.A), -1);
            }

            try
            {
                seq.ReplaceRange(0, "GA");
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.AreEqual(seq.Count, 0);
            }

            try
            {
                tmpSeq = seq.Reverse;
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.IsNull(tmpSeq);
            }

            try
            {
                tmpSeq = seq.ReverseComplement;
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.IsNull(tmpSeq);
            }

            string sequence = string.Empty;
            try
            {
                sequence = seq.ToString();
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.IsEmpty(sequence);
            }

            ISequenceItem seqItem = null;
            try
            {
                seqItem = seq[0];
                Assert.Fail();
            }
            catch (NotSupportedException)
            {
                Assert.IsNull(seqItem);
            }
            #endregion Test not supported members
        }
    }
}
