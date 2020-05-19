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

using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.Tests
{
    /// <summary>
    /// PatternConverter Test cases
    /// </summary>
    [TestClass]
    public class SequenceFindMatchTests
    {
        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SequenceFindMatchTests()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("MBF.Tests.log");
            }
        }

        /// <summary>
        /// Find pattern test.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void MultipleFindOneOutputPatternTest()
        {
            ISequence sequence = new Sequence(DnaAlphabet.Instance, "AGCT");
            IList<string> patterns = new List<string>();
            patterns.Add("AGCT");
            IDictionary<string, IList<int>> actual = sequence.FindMatches(patterns);

            IDictionary<string, HashSet<int>> expected = new Dictionary<string, HashSet<int>>();
            HashSet<int> indices = new HashSet<int>();
            indices.Add(0);
            expected.Add("AGCT", indices);

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Find at valid start index pattern test.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void MultipleFindValidStartIndexPatternTest()
        {
            ISequence sequence = new Sequence(DnaAlphabet.Instance, "CAAGCT");
            IList<string> patterns = new List<string>();
            patterns.Add("AGCT");
            IDictionary<string, IList<int>> actual = sequence.FindMatches(patterns, 1);

            IDictionary<string, HashSet<int>> expected = new Dictionary<string, HashSet<int>>();
            HashSet<int> indices = new HashSet<int>();
            indices.Add(2);
            expected.Add("AGCT", indices);

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Find at invalid start index pattern test.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void MultipleFindInvalidStartIndexPatternTest()
        {
            ISequence sequence = new Sequence(DnaAlphabet.Instance, "CAAGCT");
            IList<string> patterns = new List<string>();
            patterns.Add("AGCT");
            IDictionary<string, IList<int>> actual = sequence.FindMatches(patterns, 3);

            IDictionary<string, HashSet<int>> expected = new Dictionary<string, HashSet<int>>();
            HashSet<int> indices = new HashSet<int>();
            expected.Add("AGCT", indices);

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Find at valid start index pattern test.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void MultipleFindValidCasingPatternTest()
        {
            ISequence sequence = new Sequence(DnaAlphabet.Instance, "AGCT");
            IList<string> patterns = new List<string>();
            patterns.Add("AGCT");
            IDictionary<string, IList<int>> actual = sequence.FindMatches(patterns, 0, false);

            IDictionary<string, HashSet<int>> expected = new Dictionary<string, HashSet<int>>();
            HashSet<int> indices = new HashSet<int>();
            indices.Add(0);
            expected.Add("AGCT", indices);

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Find at valid start index pattern test.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void MultipleFindInvalidCasingPatternTest()
        {
            ISequence sequence = new Sequence(DnaAlphabet.Instance, "AGCT");
            IList<string> patterns = new List<string>();
            patterns.Add("agct");
            IDictionary<string, IList<int>> actual = sequence.FindMatches(patterns, 0, false);

            IDictionary<string, HashSet<int>> expected = new Dictionary<string, HashSet<int>>();
            HashSet<int> indices = new HashSet<int>();
            expected.Add("agct", indices);

            Assert.IsTrue(Compare(expected, actual));
        }

        /// <summary>
        /// Compares the expected and actual values and return true if they match
        /// otherwise return false.
        /// </summary>
        /// <param name="expected">Expected values</param>
        /// <param name="actual">Actual values</param>
        /// <returns>Is match</returns>
        private static bool Compare(IDictionary<string, HashSet<int>> expected, IDictionary<string, IList<int>> actual)
        {
            if (expected.Count != actual.Count)
            {
                return false;
            }

            HashSet<int> indices = null;
            foreach (KeyValuePair<string, IList<int>> result in actual)
            {
                if (expected.TryGetValue(result.Key, out indices))
                {
                    if (!Compare(indices, result.Value))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Compares the expected and actual values and return true if they match
        /// otherwise return false.
        /// </summary>
        /// <param name="expected">Expected values</param>
        /// <param name="actual">Actual values</param>
        /// <returns>Is match</returns>
        private static bool Compare(HashSet<int> expected, IList<int> actual)
        {
            if (expected.Count != actual.Count)
            {
                return false;
            }

            foreach (int result in actual)
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