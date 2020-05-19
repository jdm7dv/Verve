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
    using System.Collections.Generic;
    using MBF;
    using MBF.Algorithms.Alignment;
    using NUnit.Framework;

    /// <summary>
    /// Test Mumer implementation for GetMUMs method.
    /// </summary>
    [TestFixture]
    public class MUMmer3Test
    {
        #region MUMmer Test Cases - GetMUMs before performing LIS

        /// <summary>
        /// Test MUMmer 3 GetMUM with performLIS set as false.
        /// </summary>
        [Test]
        public void TestMUMmer3GetMUMsSingleMum()
        {
            string reference = "TTAATTTTAG";
            string search = "AGTTTAGAG";

            Sequence referenceSeq = null;
            Sequence querySeq = null;
            List<ISequence> querySeqs = null;

            referenceSeq = new Sequence(Alphabets.DNA, reference);
            querySeq = new Sequence(Alphabets.DNA, search);

            querySeqs = new List<ISequence>();
            querySeqs.Add(querySeq);

            MUMmer mummer = new MUMmer3();
            mummer.LengthOfMUM = 3;

            var result = mummer.GetMUMs(referenceSeq, querySeqs);

            // Check if output is not null
            Assert.AreNotEqual(null, result);

            IDictionary<ISequence, IList<MaxUniqueMatch>> expectedOutput = new Dictionary<ISequence, IList<MaxUniqueMatch>>();
            MaxUniqueMatch mum = new MaxUniqueMatch();
            mum.FirstSequenceMumOrder = 1;
            mum.FirstSequenceStart = 5;
            mum.SecondSequenceMumOrder = 1;
            mum.SecondSequenceStart = 2;
            mum.Length = 5;
            mum.Query = querySeq;
            expectedOutput.Add(querySeq, new List<MaxUniqueMatch> { mum });

            Assert.IsTrue(CompareMUMs(result, expectedOutput));
        }

        /// <summary>
        /// MUMmer 3 test where we get multiple MUMs.
        /// </summary>
        [Test]
        public void TestMUMmer3GetMUMsMultipleMum()
        {
            string reference = "ATGCGCATCCCCTT";
            string search = "GCGCCCCCTA";

            Sequence referenceSeq = null;
            Sequence querySeq = null;

            referenceSeq = new Sequence(Alphabets.DNA, reference);
            querySeq = new Sequence(Alphabets.DNA, search);

            List<ISequence> querySeqs = new List<ISequence>();
            querySeqs.Add(querySeq);

            MUMmer mummer = new MUMmer3();
            mummer.LengthOfMUM = 4;

            var result = mummer.GetMUMs(referenceSeq, querySeqs);

            // Check if output is not null
            Assert.AreNotEqual(null, result);

            IDictionary<ISequence, IList<MaxUniqueMatch>> expectedOutput = new Dictionary<ISequence, IList<MaxUniqueMatch>>();
            MaxUniqueMatch mum1 = new MaxUniqueMatch();
            mum1.FirstSequenceMumOrder = 1;
            mum1.FirstSequenceStart = 2;
            mum1.SecondSequenceMumOrder = 1;
            mum1.SecondSequenceStart = 0;
            mum1.Length = 4;
            mum1.Query = querySeq;

            MaxUniqueMatch mum2 = new MaxUniqueMatch();
            mum2.FirstSequenceMumOrder = 2;
            mum2.FirstSequenceStart = 8;
            mum2.SecondSequenceMumOrder = 2;
            mum2.SecondSequenceStart = 3;
            mum2.Length = 4;
            mum2.Query = querySeq;

            MaxUniqueMatch mum3 = new MaxUniqueMatch();
            mum3.FirstSequenceMumOrder = 3;
            mum3.FirstSequenceStart = 8;
            mum3.SecondSequenceMumOrder = 3;
            mum3.SecondSequenceStart = 4;
            mum3.Length = 5;
            mum3.Query = querySeq;

            expectedOutput.Add(querySeq, new List<MaxUniqueMatch> { mum1, mum2, mum3 });

            Assert.IsTrue(CompareMUMs(result, expectedOutput));
        }

        /// <summary>
        /// Test MUMmer 3 GetMUM with performLIS set as false with RNA.
        /// </summary>
        [Test]
        public void TestMUMmer3GetMUMsWithRNASingleMum()
        {
            string reference = "AUGCSWRYKMBVHDN";
            string search = "UAUASWRYBB";

            Sequence referenceSeq = null;
            Sequence querySeq = null;
            List<ISequence> querySeqs = null;

            referenceSeq = new Sequence(Alphabets.RNA, reference);
            querySeq = new Sequence(Alphabets.RNA, search);

            querySeqs = new List<ISequence>();
            querySeqs.Add(querySeq);

            MUMmer3 mummer = new MUMmer3();
            mummer.LengthOfMUM = 3;

            var result = mummer.GetMUMs(referenceSeq, querySeqs);

            // Check if output is not null
            Assert.AreNotEqual(null, result);

            IDictionary<ISequence, IList<MaxUniqueMatch>> expectedOutput = new Dictionary<ISequence, IList<MaxUniqueMatch>>();
            MaxUniqueMatch mum1 = new MaxUniqueMatch();
            mum1.FirstSequenceMumOrder = 1;
            mum1.FirstSequenceStart = 4;
            mum1.SecondSequenceMumOrder = 1;
            mum1.SecondSequenceStart = 4;
            mum1.Length = 4;
            mum1.Query = querySeq;

            expectedOutput.Add(querySeq, new List<MaxUniqueMatch> { mum1 });

            Assert.IsTrue(CompareMUMs(result, expectedOutput));
        }
        
        #endregion MUMmer Test Cases - GetMUMs before performing LIS

        #region MUMmer Test Cases - GetMUMs after performing LIS

        /// <summary>
        /// Test MUMmer 3 GetMUM with performLIS set as true to get final MUMs.
        /// </summary>
        [Test]
        public void TestMUMmer3GetFinalMUMsSingleMum()
        {
            string reference = "TTAATTTTAG";
            string search = "AGTTTAGAG";

            Sequence referenceSeq = null;
            Sequence querySeq = null;
            List<ISequence> querySeqs = null;

            referenceSeq = new Sequence(Alphabets.DNA, reference);
            querySeq = new Sequence(Alphabets.DNA, search);

            querySeqs = new List<ISequence>();
            querySeqs.Add(querySeq);

            MUMmer mummer = new MUMmer3();
            mummer.LengthOfMUM = 3;

            var result = mummer.GetMUMs(referenceSeq, querySeqs, true);

            // Check if output is not null
            Assert.AreNotEqual(null, result);

            IDictionary<ISequence, IList<MaxUniqueMatch>> expectedOutput = new Dictionary<ISequence, IList<MaxUniqueMatch>>();
            MaxUniqueMatch mum = new MaxUniqueMatch();
            mum.FirstSequenceMumOrder = 1;
            mum.FirstSequenceStart = 5;
            mum.SecondSequenceMumOrder = 1;
            mum.SecondSequenceStart = 2;
            mum.Length = 5;
            mum.Query = querySeq;
            expectedOutput.Add(querySeq, new List<MaxUniqueMatch> { mum });

            Assert.IsTrue(CompareMUMs(result, expectedOutput));
        }

        /// <summary>
        /// MUMmer 3 test where we get multiple MUMs.
        /// </summary>
        [Test]
        public void TestMUMmer3GetFinalMUMsMultipleMum()
        {
            string reference = "ATGCGCATCCCCTT";
            string search = "GCGCCCCCTA";

            Sequence referenceSeq = null;
            Sequence querySeq = null;

            referenceSeq = new Sequence(Alphabets.DNA, reference);
            querySeq = new Sequence(Alphabets.DNA, search);

            List<ISequence> querySeqs = new List<ISequence>();
            querySeqs.Add(querySeq);

            MUMmer mummer = new MUMmer3();
            mummer.LengthOfMUM = 4;

            var result = mummer.GetMUMs(referenceSeq, querySeqs, true);

            // Check if output is not null
            Assert.AreNotEqual(null, result);

            IDictionary<ISequence, IList<MaxUniqueMatch>> expectedOutput = new Dictionary<ISequence, IList<MaxUniqueMatch>>();
            MaxUniqueMatch mum1 = new MaxUniqueMatch();
            mum1.FirstSequenceMumOrder = 1;
            mum1.FirstSequenceStart = 2;
            mum1.SecondSequenceMumOrder = 1;
            mum1.SecondSequenceStart = 0;
            mum1.Length = 4;
            mum1.Query = querySeq;

            MaxUniqueMatch mum2 = new MaxUniqueMatch();
            mum2.FirstSequenceMumOrder = 3;
            mum2.FirstSequenceStart = 8;
            mum2.SecondSequenceMumOrder = 3;
            mum2.SecondSequenceStart = 4;
            mum2.Length = 5;
            mum2.Query = querySeq;

            expectedOutput.Add(querySeq, new List<MaxUniqueMatch> { mum1, mum2 });

            Assert.IsTrue(CompareMUMs(result, expectedOutput));
        }

        /// <summary>
        /// Test MUMmer 3 GetMUM with performLIS set as true 
        /// with RNA to get final MUMs.
        /// </summary>
        [Test]
        public void TestMUMmer3GetFinalMUMsWithRNASingleMum()
        {
            string reference = "AUGCSWRYKMBVHDN";
            string search = "UAUASWRYBB";

            Sequence referenceSeq = null;
            Sequence querySeq = null;
            List<ISequence> querySeqs = null;

            referenceSeq = new Sequence(Alphabets.RNA, reference);
            querySeq = new Sequence(Alphabets.RNA, search);

            querySeqs = new List<ISequence>();
            querySeqs.Add(querySeq);

            MUMmer3 mummer = new MUMmer3();
            mummer.LengthOfMUM = 3;

            var result = mummer.GetMUMs(referenceSeq, querySeqs, true);

            // Check if output is not null
            Assert.AreNotEqual(null, result);

            IDictionary<ISequence, IList<MaxUniqueMatch>> expectedOutput = new Dictionary<ISequence, IList<MaxUniqueMatch>>();
            MaxUniqueMatch mum1 = new MaxUniqueMatch();
            mum1.FirstSequenceMumOrder = 1;
            mum1.FirstSequenceStart = 4;
            mum1.SecondSequenceMumOrder = 1;
            mum1.SecondSequenceStart = 4;
            mum1.Length = 4;
            mum1.Query = querySeq;

            expectedOutput.Add(querySeq, new List<MaxUniqueMatch> { mum1 });

            Assert.IsTrue(CompareMUMs(result, expectedOutput));
        }

        #endregion MUMmer Test Cases - GetMUMs after performing LIS

        /// <summary>
        /// Compare the MUM results of mummer and expected MUMs
        /// </summary>
        /// <param name="result">output of MUMs</param>
        /// <param name="expectedMUMs">expected output</param>
        /// <returns>Compare result of GetMUMs</returns>
        private bool CompareMUMs(
                IDictionary<ISequence, IList<MaxUniqueMatch>> result,
                IDictionary<ISequence, IList<MaxUniqueMatch>> expectedMUMs)
        {
            if (result.Count != expectedMUMs.Count)
            {
                return false;
            }

            foreach (var mumResult in result)
            {
                var mums = mumResult.Value;
                var expectedMums = expectedMUMs[mumResult.Key];
                if (mums.Count != expectedMums.Count)
                {
                    return false;
                }

                for (int count = 0; count < mums.Count; count++)
                {
                    if (!(
                        mums[count].FirstSequenceMumOrder == expectedMums[count].FirstSequenceMumOrder &&
                        mums[count].FirstSequenceStart == expectedMums[count].FirstSequenceStart &&
                        mums[count].SecondSequenceMumOrder == expectedMums[count].SecondSequenceMumOrder &&
                        mums[count].SecondSequenceStart == expectedMums[count].SecondSequenceStart &&
                        mums[count].Length == expectedMums[count].Length &&
                        mums[count].Query == expectedMums[count].Query))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
