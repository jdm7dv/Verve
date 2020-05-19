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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using MBF.Encoding;

    /// <summary>
    /// Class to test ByteArray.
    /// </summary>
    [TestFixture]
    public class ByteArrayTest
    {
        /// <summary>
        /// Test ByteArray class.
        /// </summary>
        [Test]
        public void TestByteArray()
        {
            Sequence seq = new Sequence(Alphabets.DNA, Encodings.Ncbi2NA, "ACGTA");
            Assert.AreEqual(seq.ToString(), "ACGTA");

            seq = new Sequence(Alphabets.DNA);
            seq.Add(Alphabets.DNA.C);
            seq.Add(Alphabets.DNA.T);
            seq.Add(Alphabets.DNA.C);
            seq.IsReadOnly = true;
            Assert.AreEqual(seq.ToString(), "CTC");

            foreach (IEncoding encoding in Encodings.All)
            {
                StringBuilder str = new StringBuilder();
                foreach (ISequenceItem seqItem in encoding)
                {
                    str.Append(seqItem.Symbol);
                }

                string seqString = str.ToString();

                switch (encoding.Name)
                {
                    case "IUPACna":
                    case "NCBI2na":
                    case "NCBI4na":
                        Sequence dnaseq = new Sequence(Alphabets.DNA, encoding, seqString);
                        Assert.AreEqual(dnaseq.ToString(), seqString);
                        Sequence rnaseq = new Sequence(Alphabets.RNA, encoding, seqString.Replace('T', 'U'));
                        Assert.AreEqual(rnaseq.ToString(), seqString.Replace('T', 'U'));
                        break;

                    case "NcbiEAAEncoding":
                    case "NcbiStdAAEncoding":
                        Sequence proSeq = new Sequence(Alphabets.Protein, encoding, seqString);
                        Assert.AreEqual(proSeq.ToString(), seqString);
                        break;
                }
            }

            seq = new Sequence(Alphabets.DNA);
            seq.IsReadOnly = true;
            seq.IsReadOnly = false;
            seq.Add(Alphabets.DNA.C);
            seq.Add(Alphabets.DNA.C);
            seq.Add(Alphabets.DNA.G);
            seq.Add(Alphabets.DNA.C);
            seq.Add(Alphabets.DNA.G);
            seq.Add(Alphabets.DNA.A);
            seq.IsReadOnly = true;
            Assert.AreEqual(seq.ToString(), "CCGCGA");

            seq = new Sequence(Alphabets.DNA, Encodings.Ncbi2NA, "CCGCGA");
            seq.IsReadOnly = true;
            seq.IsReadOnly = false;
            seq.Add(Alphabets.DNA.C);
            seq.Add(Alphabets.DNA.C);
            seq.Add(Alphabets.DNA.G);
            seq.Add(Alphabets.DNA.C);
            seq.Add(Alphabets.DNA.G);
            seq.Add(Alphabets.DNA.A);
            seq.Add(Alphabets.DNA.T);
            seq.IsReadOnly = true;

            Assert.AreEqual(seq.ToString(), "CCGCGACCGCGAT");
            byte[] encodingValues = seq.EncodedValues;
            Assert.AreEqual(encodingValues[0], 1);
            Assert.AreEqual(encodingValues[1], 1);
            Assert.AreEqual(encodingValues[2], 2);
            Assert.AreEqual(encodingValues[3], 1);
            Assert.AreEqual(encodingValues[4], 2);
            Assert.AreEqual(encodingValues[5], 0);
            Assert.AreEqual(encodingValues[6], 1);
            Assert.AreEqual(encodingValues[7], 1);
            Assert.AreEqual(encodingValues[8], 2);
            Assert.AreEqual(encodingValues[9], 1);
            Assert.AreEqual(encodingValues[10], 2);
            Assert.AreEqual(encodingValues[11], 0);
            Assert.AreEqual(encodingValues[12], 3);

            seq = new Sequence(Alphabets.DNA, Encodings.IupacNA, "AAAAAAADGY");
            encodingValues = seq.EncodedValues;
            Assert.AreEqual(encodingValues.Length, 10);
            Assert.AreEqual(encodingValues[0], 65);
            Assert.AreEqual(encodingValues[1], 65);
            Assert.AreEqual(encodingValues[2], 65);
            Assert.AreEqual(encodingValues[3], 65);
            Assert.AreEqual(encodingValues[4], 65);
            Assert.AreEqual(encodingValues[5], 65);
            Assert.AreEqual(encodingValues[6], 65);
            Assert.AreEqual(encodingValues[7], 68);
            Assert.AreEqual(encodingValues[8], 71);
            Assert.AreEqual(encodingValues[9], 89);

            seq = new Sequence(Alphabets.DNA);
            seq.IsReadOnly = true;
            encodingValues = seq.EncodedValues;
            Assert.AreEqual(encodingValues.Length, 0);
        }
    }
}
