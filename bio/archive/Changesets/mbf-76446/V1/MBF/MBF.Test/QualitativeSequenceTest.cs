// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MBF.Encoding;

namespace MBF.Test
{
    /// <summary>
    /// Class to test QualitativeSequence.
    /// </summary>
    [TestFixture]
    public class QualitativeSequenceTest
    {
        /// <summary>
        /// Test constructors with invalid input values.
        /// </summary>
        [Test]
        public void TestConstructorsWithInvalidParameters()
        {
            QualitativeSequence qualSequence;
            try
            {
                qualSequence = new QualitativeSequence(null);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            try
            {
                qualSequence = new QualitativeSequence(
                                                Alphabets.DNA,
                                                FastQFormatType.Sanger,
                                                null as IEncoding,
                                                string.Empty);
                Assert.Fail();
            }
            catch (Exception)
            {
            }

            try
            {
                qualSequence = new QualitativeSequence(Alphabets.RNA, FastQFormatType.Illumina, "ACTGGA");
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Test constructors with valid input values.
        /// </summary>
        [Test]
        public void TestConstructorsWithValidParametetrs()
        {
            QualitativeSequence qualSequence;
            qualSequence = new QualitativeSequence(Alphabets.DNA);
            Assert.AreEqual(qualSequence.Alphabet, Alphabets.DNA);
            Assert.IsFalse(qualSequence.IsReadOnly);
            Assert.AreEqual(qualSequence.Count, 0);
            Assert.AreEqual(qualSequence.Scores.Length, 0);
            Assert.AreEqual(qualSequence.ToString(), string.Empty);
            Assert.AreEqual(qualSequence.Type, FastQFormatType.Illumina);

            qualSequence = new QualitativeSequence(Alphabets.RNA, FastQFormatType.Illumina);
            Assert.AreEqual(qualSequence.Alphabet, Alphabets.RNA);
            Assert.IsFalse(qualSequence.IsReadOnly);
            Assert.AreEqual(qualSequence.Count, 0);
            Assert.AreEqual(qualSequence.Scores.Length, 0);
            Assert.AreEqual(qualSequence.ToString(), string.Empty);
            Assert.AreEqual(qualSequence.Type, FastQFormatType.Illumina);

            qualSequence = new QualitativeSequence(Alphabets.RNA, FastQFormatType.Illumina, "ACUGGA");
            Assert.AreEqual(qualSequence.Alphabet, Alphabets.RNA);
            Assert.IsTrue(qualSequence.IsReadOnly);
            Assert.AreEqual(qualSequence.Count, 6);
            Assert.AreEqual(qualSequence.Scores.Length, 6);
            foreach (byte qualScore in qualSequence.Scores)
            {
                Assert.AreEqual(qualScore, QualitativeSequence.GetDefaultQualScore(FastQFormatType.Illumina));
            }

            Assert.AreEqual(qualSequence.ToString(), "ACUGGA");
            Assert.AreEqual(qualSequence.Type, FastQFormatType.Illumina);

            qualSequence = new QualitativeSequence(Alphabets.RNA, FastQFormatType.Illumina, "ACUGGA", 65);
            Assert.AreEqual(qualSequence.Alphabet, Alphabets.RNA);
            Assert.IsTrue(qualSequence.IsReadOnly);
            Assert.AreEqual(qualSequence.Count, 6);
            Assert.AreEqual(qualSequence.Scores.Length, 6);

            foreach (byte qualScore in qualSequence.Scores)
            {
                Assert.AreEqual(qualScore, 65);
            }

            Assert.AreEqual(qualSequence.ToString(), "ACUGGA");
            Assert.AreEqual(qualSequence.Type, FastQFormatType.Illumina);
        }

        /// <summary>
        /// Test editing when IsReadOnly flag is true.
        /// </summary>
        [Test]
        public void TestEditingWhenIsReadOnlyIsTrue()
        {
            QualitativeSequence qualSequence = new QualitativeSequence(Alphabets.RNA, FastQFormatType.Illumina, "ACUGGA", 65);
            Assert.IsTrue(qualSequence.IsReadOnly);

            try
            {
                qualSequence.Clear();
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(qualSequence.Count, 6);
            }

            try
            {
                qualSequence.Add(Alphabets.RNA.AC);
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(qualSequence.Count, 6);
            }

            try
            {
                qualSequence.Remove(Alphabets.RNA.AC);
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(qualSequence.Count, 6);
            }

            try
            {
                qualSequence.Replace(0, Alphabets.RNA.AC.Symbol);
                Assert.Fail();
            }
            catch
            {
                Assert.AreEqual(qualSequence.Count, 6);
            }
        }

        /// <summary>
        /// Test editing when IsReadOnly flag is false.
        /// </summary>
        [Test]
        public void TestEditingwhenIsReadOnlyIsFalse()
        {
            QualitativeSequence qualSequence = new QualitativeSequence(Alphabets.RNA, FastQFormatType.Illumina, "ACUGGA", 65);
            Assert.IsTrue(qualSequence.IsReadOnly);
            qualSequence.IsReadOnly = false;

            Assert.AreEqual(qualSequence.Count, 6);
            Assert.AreEqual(qualSequence.ToString(), "ACUGGA");

            qualSequence.Clear();
            Assert.AreEqual(qualSequence.Count, 0);
            Assert.AreEqual(qualSequence.Scores.Length, 0);

            qualSequence.Add(Alphabets.RNA.A, 65);
            Assert.AreEqual(qualSequence.Count, 1);
            Assert.AreEqual(qualSequence.Scores.Length, 1);
            Assert.AreSame(qualSequence[0], Alphabets.RNA.A);
            Assert.AreEqual(qualSequence.Scores[0], 65);

            qualSequence.Insert(0, Alphabets.RNA.G, 70);
            Assert.AreEqual(qualSequence.Count, 2);
            Assert.AreEqual(qualSequence.Scores.Length, 2);
            Assert.AreSame(qualSequence[0], Alphabets.RNA.G);
            Assert.AreSame(qualSequence[1], Alphabets.RNA.A);
            Assert.AreEqual(qualSequence.Scores[0], 70);
            Assert.AreEqual(qualSequence.Scores[1], 65);

            qualSequence.Replace(0, Alphabets.RNA.U, 75);
            Assert.AreEqual(qualSequence.Count, 2);
            Assert.AreEqual(qualSequence.Scores.Length, 2);
            Assert.AreSame(qualSequence[0], Alphabets.RNA.U);
            Assert.AreSame(qualSequence[1], Alphabets.RNA.A);
            Assert.AreEqual(qualSequence.Scores[0], 75);
            Assert.AreEqual(qualSequence.Scores[1], 65);

            qualSequence.Remove(Alphabets.RNA.C);
            Assert.AreEqual(qualSequence.Count, 2);
            Assert.AreEqual(qualSequence.Scores.Length, 2);
            Assert.AreSame(qualSequence[0], Alphabets.RNA.U);
            Assert.AreSame(qualSequence[1], Alphabets.RNA.A);
            Assert.AreEqual(qualSequence.Scores[0], 75);
            Assert.AreEqual(qualSequence.Scores[1], 65);

            qualSequence.Remove(Alphabets.RNA.A);
            Assert.AreEqual(qualSequence.Count, 1);
            Assert.AreEqual(qualSequence.Scores.Length, 1);
            Assert.AreSame(qualSequence[0], Alphabets.RNA.U);
            Assert.AreEqual(qualSequence.Scores[0], 75);

            qualSequence.Add(Alphabets.RNA.A, 65);
            Assert.AreEqual(qualSequence.Count, 2);
            Assert.AreEqual(qualSequence.Scores.Length, 2);
            Assert.AreSame(qualSequence[0], Alphabets.RNA.U);
            Assert.AreSame(qualSequence[1], Alphabets.RNA.A);
            Assert.AreEqual(qualSequence.Scores[0], 75);
            Assert.AreEqual(qualSequence.Scores[1], 65);

            qualSequence.RemoveAt(1);
            Assert.AreEqual(qualSequence.Count, 1);
            Assert.AreEqual(qualSequence.Scores.Length, 1);
        }

        /// <summary>
        /// Test Converting from Sanger to Solexa format.
        /// </summary>
        [Test]
        public void TestConvertingFromSangerToSolexa()
        {
            string sequencestr = "ACGTAAAAAAAGGGGGGGGGGGGGGGGCCCCCCCCCCCCCCCCCGGGGGGGGTTTTTTTTTTTTTTTTTTTTTTTTTGGGGGGGGGGGGAAAAA";
            string sangerQaulScores = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            string solexaQualScores = ";;>@BCEFGHJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";
            QualitativeSequence sangerSeq = new QualitativeSequence(Alphabets.DNA,
              FastQFormatType.Sanger,
              sequencestr,
              ASCIIEncoding.ASCII.GetBytes(sangerQaulScores));
            QualitativeSequence solexaSeq = sangerSeq.ConvertTo(FastQFormatType.Solexa);
            string testSolexa = ASCIIEncoding.ASCII.GetString(solexaSeq.Scores);
            Assert.AreEqual(testSolexa, solexaQualScores);
        }

        /// <summary>
        /// Test Converting from Sanger to Illumina format.
        /// </summary>
        [Test]
        public void TestConvertingFromSangerToIllumina()
        {
            string sequencestr = "ACGTAAAAAAAGGGGGGGGGGGGGGGGCCCCCCCCCCCCCCCCCGGGGGGGGTTTTTTTTTTTTTTTTTTTTTTTTTGGGGGGGGGGGGAAAAA";
            string sangerQaulScores = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            string illuminaQualScores = "@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~";
            QualitativeSequence sangerSeq = new QualitativeSequence(Alphabets.DNA,
              FastQFormatType.Sanger,
              sequencestr,
              ASCIIEncoding.ASCII.GetBytes(sangerQaulScores));
            QualitativeSequence illuminaSeq = sangerSeq.ConvertTo(FastQFormatType.Illumina);
            string testIllumina = ASCIIEncoding.ASCII.GetString(illuminaSeq.Scores);
            Assert.AreEqual(testIllumina, illuminaQualScores);
        }

        /// <summary>
        /// Test Converting from Solexa to Sanger format.
        /// </summary>
        [Test]
        public void TestConvertingFromSolexaToSanger()
        {
            string sequencestr = "GCCCCCCCCCCCCCCCCCGGGGGGGGTTTTTTTTTTTTTTTTTTTTTTTTTGGGGGGGGGGGGAAAAA";
            string solexaQualScores = ";<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            string sangerQaulScores = "!\"##$$%%&&'()*++,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_";
            QualitativeSequence solexaSeq = new QualitativeSequence(Alphabets.DNA,
              FastQFormatType.Solexa,
              sequencestr,
              ASCIIEncoding.ASCII.GetBytes(solexaQualScores));
            QualitativeSequence sangerSeq = solexaSeq.ConvertTo(FastQFormatType.Sanger);
            string testSanger = ASCIIEncoding.ASCII.GetString(sangerSeq.Scores);
            Assert.AreEqual(testSanger, sangerQaulScores);
        }

        /// <summary>
        /// Test Converting from Solexa to Illumina format.
        /// </summary>
        [Test]
        public void TestConvertingFromSolexaToIllumina()
        {
            string sequencestr = "GCCCCCCCCCCCCCCCCCGGGGGGGGTTTTTTTTTTTTTTTTTTTTTTTTTGGGGGGGGGGGGAAAAA";
            string solexaQualScores = ";<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            string illuminaQualScores = "@ABBCCDDEEFGHIJJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            QualitativeSequence solexaSeq = new QualitativeSequence(Alphabets.DNA,
              FastQFormatType.Solexa,
              sequencestr,
              ASCIIEncoding.ASCII.GetBytes(solexaQualScores));
            QualitativeSequence illuminaSeq = solexaSeq.ConvertTo(FastQFormatType.Illumina);
            string testillumina = ASCIIEncoding.ASCII.GetString(illuminaSeq.Scores);
            Assert.AreEqual(testillumina, illuminaQualScores);
        }

        /// <summary>
        /// Test Converting from Illumina to Sanger format.
        /// </summary>
        [Test]
        public void TestConvertingFromIlluminaToSanger()
        {
            string sequencestr = "CCCCCCCCCCCCCGGGGGGGGTTTTTTTTTTTTTTTTTTTTTTTTTGGGGGGGGGGGGAAAAA";
            string illuminaQualScores = "@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            string sangerQaulScores = "!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_";

            QualitativeSequence illuminaSeq = new QualitativeSequence(Alphabets.DNA,
              FastQFormatType.Illumina,
              sequencestr,
              ASCIIEncoding.ASCII.GetBytes(illuminaQualScores));

            QualitativeSequence sangerSeq = illuminaSeq.ConvertTo(FastQFormatType.Sanger);
            string testSanger = ASCIIEncoding.ASCII.GetString(sangerSeq.Scores);
            Assert.AreEqual(testSanger, sangerQaulScores);
        }


        /// <summary>
        /// Test Converting from Illumina to Solexa format.
        /// </summary>
        [Test]
        public void TestConvertingFromIlluminaToSolexa()
        {
            string sequencestr = "CCCCCCCCCCCCCGGGGGGGGTTTTTTTTTTTTTTTTTTTTTTTTTGGGGGGGGGGGGAAAAA";
            string illuminaQualScores = "@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
            string solexaQaulScores = ";;>@BCEFGHJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";

            QualitativeSequence illuminaSeq = new QualitativeSequence(Alphabets.DNA,
              FastQFormatType.Illumina,
              sequencestr,
              ASCIIEncoding.ASCII.GetBytes(illuminaQualScores));

            QualitativeSequence solexaSeq = illuminaSeq.ConvertTo(FastQFormatType.Solexa);
            string testSolexa = ASCIIEncoding.ASCII.GetString(solexaSeq.Scores);
            Assert.AreEqual(testSolexa, solexaQaulScores);
        }
    }
}
