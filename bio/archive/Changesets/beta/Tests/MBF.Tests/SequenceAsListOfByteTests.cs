// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.Tests
{
    [TestClass]
    public class SequenceAsListOfByteTests
    {
        /// <summary>
        /// Test implementation of IList of bytes on ISequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestListOfByteOnISequence()
        {
            // Create each sequence type and send to the test method which will call all methods of IList<byte>

            Sequence sequence = new Sequence(DnaAlphabet.Instance, "ACT");
            sequence.IsReadOnly = false;
            TestListOfByteMethods(sequence);

            sequence = new Sequence(DnaAlphabet.Instance, "ACT");
            sequence.UseEncoding = true;
            TestListOfByteMethods(sequence);

            BasicDerivedSequence basicDerived = new BasicDerivedSequence(new Sequence(DnaAlphabet.Instance, "ACT"), false, false, 0, sequence.Count);
            TestListOfByteMethods(basicDerived);

            basicDerived = new BasicDerivedSequence(new Sequence(DnaAlphabet.Instance, "ACT"), false, false, 0, sequence.Count);
            basicDerived.UseEncoding = true;
            TestListOfByteMethods(basicDerived);

            DerivedSequence derived = new DerivedSequence(new Sequence(DnaAlphabet.Instance, "ACT"));
            TestListOfByteMethods(derived);

            derived = new DerivedSequence(new Sequence(DnaAlphabet.Instance, "ACT"));
            derived.UseEncoding = true;
            TestListOfByteMethods(derived);

            QualitativeSequence qualitative = new QualitativeSequence(DnaAlphabet.Instance,FastQFormatType.Illumina,"ACT");
            qualitative.IsReadOnly = false;
            TestListOfByteMethods(qualitative);

            qualitative = new QualitativeSequence(DnaAlphabet.Instance, FastQFormatType.Illumina, "ACT");
            qualitative.UseEncoding = true;
            TestListOfByteMethods(qualitative);

            SegmentedSequence segmented = new SegmentedSequence(new List<ISequence> { 
                new Sequence(DnaAlphabet.Instance,"A") { IsReadOnly = false}, 
                new Sequence(DnaAlphabet.Instance,"CT") { IsReadOnly = false}
            });
            TestListOfByteMethods(segmented);

            SparseSequence sparse = new SparseSequence(DnaAlphabet.Instance, 0,
                new List<ISequenceItem>
                {
                    DnaAlphabet.Instance.A, DnaAlphabet.Instance.C, DnaAlphabet.Instance.T
                });
            sparse.IsReadOnly = false;
            TestListOfByteMethods(sparse);

            // No test code for VirtualSequence
        }

        /// <summary>
        /// Test each method in IList implementation
        /// </summary>
        /// <param name="sequence">Sequence to test. Expects 'ACT'</param>
        private static void TestListOfByteMethods(ISequence sequence)
        {
            // setup byte values for alphabets
            byte A, C, G, T; // byte values
            ISequenceItem oC; // sequence items

            if (sequence.UseEncoding)
            {
                A = sequence.Encoding.LookupBySymbol('A').Value;
                C = sequence.Encoding.LookupBySymbol('C').Value;
                G = sequence.Encoding.LookupBySymbol('G').Value;
                T = sequence.Encoding.LookupBySymbol('T').Value;

                oC = sequence.Encoding.LookupBySymbol('C');
            }
            else
            {
                A = (byte)'A';
                C = (byte)'C';
                G = (byte)'G';
                T = (byte)'T';

                oC = sequence.Alphabet.LookupBySymbol('C');
            }

            // Cast to byte list
            IList<byte> byteSequence = sequence as IList<byte>;

            // IndexOf
            Assert.AreEqual(sequence.IndexOf(oC), byteSequence.IndexOf(C));

            // indexer get
            Assert.AreEqual(C, byteSequence[1]);

            // Contains
            Assert.IsFalse(!byteSequence.Contains(C));
            Assert.IsFalse(byteSequence.Contains(G));

            // Copy to
            byte[] bytes = new byte[sequence.Count];
            byteSequence.CopyTo(bytes, 0);

            // Enumerator
            int curIndex = 0;
            foreach (byte value in byteSequence)
            {
                Assert.AreEqual(bytes[curIndex++], value);
            }

            // tests for writable sequences
            if (!sequence.IsReadOnly)
            {
                // Insert
                byteSequence.Insert(2, G); // ACGT
                // IndexOf
                Assert.AreEqual(2, byteSequence.IndexOf(G));

                // indexer set
                byteSequence[2] = A; // ACAT
                Assert.IsFalse(byteSequence.Contains(G));

                // Add
                byteSequence.Add(G); // ACATG
                Assert.IsFalse(!byteSequence.Contains(G));

                // Remove
                byteSequence.Remove(T); // ACA
                Assert.IsFalse(byteSequence.Contains(T));

                byteSequence.RemoveAt(2);
            }
        }
    }
}
