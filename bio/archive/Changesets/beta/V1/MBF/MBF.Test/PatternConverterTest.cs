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
using MBF.Util.Logging;
using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// PatternConverter Test cases
    /// </summary>
    [TestFixture]
    public class PatternConverterTest
    {
        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static PatternConverterTest()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Empty pattern test.
        /// </summary>
        [Test]
        public void EmptyPatternTest()
        {
            try
            {
                IPatternConverter patternConverter = PatternConverter.GetInstanace(DnaAlphabet.Instance);
                patternConverter.Convert(string.Empty);
            }
            catch (Exception ex)
            {
                Assert.AreEqual(ex.Message, "Value cannot be null.\r\nParameter name: pattern");
            }
        }

        /// <summary>
        /// Simple Dna Pattern test.
        /// </summary>
        [Test]
        public void UnAmbiguousDnaPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("AGCT");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGCT");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Simple Rna Pattern test.
        /// </summary>
        [Test]
        public void UnAmbiguousRnaPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(RnaAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("ACGU");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("ACGU");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Simple Protien Pattern test.
        /// </summary>
        [Test]
        public void UnAmbiguousProtienPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(ProteinAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("ACDEFGH");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("ACDEFGH");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Ambiguous Dna Pattern test.
        /// </summary>
        [Test]
        public void AmbiguousDnaPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("AGCTR");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGCTG");
            expected.Add("AGCTA");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Ambiguous Rna Pattern test.
        /// </summary>
        [Test]
        public void AmbiguousRnaPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(RnaAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("AGCUM");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGCUA");
            expected.Add("AGCUC");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Ambiguous Protein Pattern test.
        /// </summary>
        [Test]
        public void AmbiguousProteinPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(ProteinAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("ABCDEFGH");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("ADCDEFGH");
            expected.Add("ANCDEFGH");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Dna Pattern with square bracket test.
        /// </summary>
        [Test]
        public void SquareBracketDnaPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("A[GCT]R");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGG");
            expected.Add("ACG");
            expected.Add("ATG");
            expected.Add("AGA");
            expected.Add("ACA");
            expected.Add("ATA");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Dna Pattern with curly bracket test.
        /// </summary>
        [Test]
        public void CurlyBracketDnaPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("A{GCT}R");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AAG");
            expected.Add("AAA");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Dna Pattern with parenthesis test.
        /// </summary>
        [Test]
        public void ParenthesisDnaPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("AGC(5)TR");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGCCCCCTG");
            expected.Add("AGCCCCCTA");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Dna Pattern with parenthesis test.
        /// </summary>
        [Test]
        public void ParenthesisRangeDnaPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("AGC(2, 5)TR");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGCCTG");
            expected.Add("AGCCCTG");
            expected.Add("AGCCCCTG");
            expected.Add("AGCCCCCTG");
            expected.Add("AGCCTA");
            expected.Add("AGCCCTA");
            expected.Add("AGCCCCTA");
            expected.Add("AGCCCCCTA");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Dna Pattern with StartsWith test.
        /// </summary>
        [Test]
        public void StartsWithDnaPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("<AGCTR");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("<AGCTG");
            expected.Add("<AGCTA");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Dna Pattern with EndsWith test.
        /// </summary>
        [Test]
        public void EndsWithDnaPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("AGCTR>");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGCTG>");
            expected.Add("AGCTA>");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Dna Pattern with Repeat test.
        /// </summary>
        [Test]
        public void RepeatDnaPatternTest()
        {
            IPatternConverter patternConverter = PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> actual = patternConverter.Convert("AG*CTR");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AG*CTG");
            expected.Add("AG*CTA");

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Compares the expected and actual values and return true if they match
        /// otherwise return false.
        /// </summary>
        /// <param name="expected">Expected values</param>
        /// <param name="actual">Actual values</param>
        /// <returns>Is match</returns>
        private static bool Compare(HashSet<string> expected, IList<string> actual)
        {
            if (expected.Count != actual.Count)
            {
                return false;
            }

            foreach (string result in actual)
            {
                if (!expected.Contains(result))
                { 
                    return false;
                }
            }

            return true;
        }
    }
}