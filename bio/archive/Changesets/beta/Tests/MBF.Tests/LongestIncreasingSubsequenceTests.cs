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
    #region -- Using Directive --

    using System.Collections.Generic;
    using MBF.Algorithms.Alignment;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion -- Using Directive --

    /// <summary>
    /// Test Mumer implementation.
    /// </summary>
    [TestClass]
    public class LongestIncreasingSubsequenceTests
    {
        /// <summary>
        /// Test LongestIncreasingSubsequence with MUM set which has neither
        /// crosses nor overlaps
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestLISWithoutCrossAndOverlap()
        {
            // Create a list of Mum classes.
            List<MaxUniqueMatch> MUM = new List<MaxUniqueMatch>();
            MaxUniqueMatch mum = null;

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 0;
            mum.FirstSequenceMumOrder = 1;
            mum.Length = 3;
            mum.SecondSequenceStart = 0;
            mum.SecondSequenceMumOrder = 1;
            MUM.Add(mum);

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 4;
            mum.FirstSequenceMumOrder = 2;
            mum.Length = 3;
            mum.SecondSequenceStart = 3;
            mum.SecondSequenceMumOrder = 2;
            MUM.Add(mum);

            ILongestIncreasingSubsequence lis = new LongestIncreasingSubsequence();
            IList<MaxUniqueMatch> lisList = lis.GetLongestSequence(MUM);

            List<MaxUniqueMatch> expectedOutput = new List<MaxUniqueMatch>();
            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 0;
            mum.FirstSequenceMumOrder = 1;
            mum.Length = 3;
            mum.SecondSequenceStart = 0;
            mum.SecondSequenceMumOrder = 1;
            expectedOutput.Add(mum);

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 4;
            mum.FirstSequenceMumOrder = 2;
            mum.Length = 3;
            mum.SecondSequenceStart = 3;
            mum.SecondSequenceMumOrder = 2;
            expectedOutput.Add(mum);

            Assert.IsTrue(CompareMumList(lisList, expectedOutput));
        }

        /// <summary>
        /// Test LongestIncreasingSubsequence with MUM set which has crosses.
        /// First MUM is bigger
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestLISWithCross1()
        {
            // Create a list of Mum classes.
            List<MaxUniqueMatch> MUM = new List<MaxUniqueMatch>();
            MaxUniqueMatch mum = null;

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 0;
            mum.FirstSequenceMumOrder = 1;
            mum.Length = 4;
            mum.SecondSequenceStart = 4;
            mum.SecondSequenceMumOrder = 1;
            MUM.Add(mum);

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 4;
            mum.FirstSequenceMumOrder = 2;
            mum.Length = 3;
            mum.SecondSequenceStart = 0;
            mum.SecondSequenceMumOrder = 2;
            MUM.Add(mum);

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 10;
            mum.FirstSequenceMumOrder = 3;
            mum.Length = 3;
            mum.SecondSequenceStart = 10;
            mum.SecondSequenceMumOrder = 3;
            MUM.Add(mum);

            ILongestIncreasingSubsequence lis = new LongestIncreasingSubsequence();
            IList<MaxUniqueMatch> lisList = lis.GetLongestSequence(MUM);

            List<MaxUniqueMatch> expectedOutput = new List<MaxUniqueMatch>();
            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 0;
            mum.FirstSequenceMumOrder = 1;
            mum.Length = 4;
            mum.SecondSequenceStart = 4;
            mum.SecondSequenceMumOrder = 1;
            expectedOutput.Add(mum);

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 10;
            mum.FirstSequenceMumOrder = 3;
            mum.Length = 3;
            mum.SecondSequenceStart = 10;
            mum.SecondSequenceMumOrder = 3;
            expectedOutput.Add(mum);

            Assert.IsTrue(CompareMumList(lisList, expectedOutput));
        }

        /// <summary>
        /// Test LongestIncreasingSubsequence with MUM set which has crosses.
        /// Second MUM is bigger
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestLISWithCross2()
        {
            // Create a list of Mum classes.
            List<MaxUniqueMatch> MUM = new List<MaxUniqueMatch>();
            MaxUniqueMatch mum = null;

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 0;
            mum.FirstSequenceMumOrder = 1;
            mum.Length = 3;
            mum.SecondSequenceStart = 4;
            mum.SecondSequenceMumOrder = 1;
            MUM.Add(mum);

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 4;
            mum.FirstSequenceMumOrder = 2;
            mum.Length = 4;
            mum.SecondSequenceStart = 0;
            mum.SecondSequenceMumOrder = 2;
            MUM.Add(mum);

            ILongestIncreasingSubsequence lis = new LongestIncreasingSubsequence();
            IList<MaxUniqueMatch> lisList = lis.GetLongestSequence(MUM);

            List<MaxUniqueMatch> expectedOutput = new List<MaxUniqueMatch>();
            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 4;
            mum.FirstSequenceMumOrder = 2;
            mum.Length = 4;
            mum.SecondSequenceStart = 0;
            mum.SecondSequenceMumOrder = 2;
            expectedOutput.Add(mum);

            Assert.IsTrue(CompareMumList(lisList, expectedOutput));
        }

        /// <summary>
        /// Test LongestIncreasingSubsequence with MUM set which has overlap
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestLISWithCrossAndOverlap()
        {
            // Create a list of Mum classes.
            List<MaxUniqueMatch> MUM = new List<MaxUniqueMatch>();
            MaxUniqueMatch mum = null;

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 0;
            mum.FirstSequenceMumOrder = 1;
            mum.Length = 5;
            mum.SecondSequenceStart = 5;
            mum.SecondSequenceMumOrder = 1;
            MUM.Add(mum);

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 3;
            mum.FirstSequenceMumOrder = 2;
            mum.Length = 5;
            mum.SecondSequenceStart = 0;
            mum.SecondSequenceMumOrder = 2;
            MUM.Add(mum);

            ILongestIncreasingSubsequence lis = new LongestIncreasingSubsequence();
            IList<MaxUniqueMatch> lisList = lis.GetLongestSequence(MUM);

            List<MaxUniqueMatch> expectedOutput = new List<MaxUniqueMatch>();
            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 0;
            mum.FirstSequenceMumOrder = 1;
            mum.Length = 5;
            mum.SecondSequenceStart = 5;
            mum.SecondSequenceMumOrder = 1;
            expectedOutput.Add(mum);

            Assert.IsTrue(CompareMumList(lisList, expectedOutput));
        }

        /// <summary>
        /// Test LongestIncreasingSubsequence with MUM set which has overlap
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void TestLISWithOverlap()
        {
            // Create a list of Mum classes.
            List<MaxUniqueMatch> MUM = new List<MaxUniqueMatch>();
            MaxUniqueMatch mum = null;

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 0;
            mum.FirstSequenceMumOrder = 1;
            mum.Length = 4;
            mum.SecondSequenceStart = 0;
            mum.SecondSequenceMumOrder = 1;
            MUM.Add(mum);

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 2;
            mum.FirstSequenceMumOrder = 2;
            mum.Length = 5;
            mum.SecondSequenceStart = 4;
            mum.SecondSequenceMumOrder = 2;
            MUM.Add(mum);

            ILongestIncreasingSubsequence lis = new LongestIncreasingSubsequence();
            IList<MaxUniqueMatch> lisList = lis.GetLongestSequence(MUM);

            List<MaxUniqueMatch> expectedOutput = new List<MaxUniqueMatch>();
            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 0;
            mum.FirstSequenceMumOrder = 1;
            mum.Length = 4;
            mum.SecondSequenceStart = 0;
            mum.SecondSequenceMumOrder = 1;
            expectedOutput.Add(mum);

            mum = new MaxUniqueMatch();
            mum.FirstSequenceStart = 4;
            mum.FirstSequenceMumOrder = 2;
            mum.Length = 3;
            mum.SecondSequenceStart = 6;
            mum.SecondSequenceMumOrder = 2;
            expectedOutput.Add(mum);

            Assert.IsTrue(CompareMumList(lisList, expectedOutput));
        }

        /// <summary>
        /// Compares two list of Mum against their SecondSequeceMumOrder value.
        /// </summary>
        /// <param name="lisList">First list to be compared.</param>
        /// <param name="expectedOutput">Second list to be compared.</param>
        /// <returns>true if the order of their SecondSequeceMumOrder are same.</returns>
        private static bool CompareMumList(
                IList<MaxUniqueMatch> lisList,
                IList<MaxUniqueMatch> expectedOutput)
        {
            if (lisList.Count == expectedOutput.Count)
            {
                bool correctOutput = true;
                for (int index = 0; index < expectedOutput.Count; index++)
                {
                    if (lisList[index].FirstSequenceStart != expectedOutput[index].FirstSequenceStart)
                    {
                        correctOutput = false;
                        break;
                    }

                    if (lisList[index].FirstSequenceMumOrder != expectedOutput[index].FirstSequenceMumOrder)
                    {
                        correctOutput = false;
                        break;
                    }

                    if (lisList[index].Length != expectedOutput[index].Length)
                    {
                        correctOutput = false;
                        break;
                    }

                    if (lisList[index].SecondSequenceMumOrder != expectedOutput[index].SecondSequenceMumOrder)
                    {
                        correctOutput = false;
                        break;
                    }

                    if (lisList[index].SecondSequenceStart != expectedOutput[index].SecondSequenceStart)
                    {
                        correctOutput = false;
                        break;
                    }
                }

                return correctOutput;
            }

            return false;
        }
    }
}