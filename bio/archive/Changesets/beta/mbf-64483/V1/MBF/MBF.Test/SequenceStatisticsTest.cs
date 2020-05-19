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
using System.IO;

using MBF.IO;
using MBF.IO.GenBank;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// Unit tests for SequenceStatistics.
    /// </summary>
    [TestFixture]
    public class SequenceStatisticsTest
    {
        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SequenceStatisticsTest()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Verifies that the Statistics
        /// </summary>
        [Test]
        public void TestSequenceStatistics()
        {
            // Build a sequence.
            Sequence seq = new Sequence(Alphabets.DNA, "gattacayyy");
            seq.IsReadOnly = false;
            // Test initial stats.
            Assert.AreEqual(2, seq.Statistics.GetCount('T'));
            Assert.AreEqual(1, seq.Statistics.GetCount('c'));
            Assert.AreEqual(1, seq.Statistics.GetCount('C'));
            Assert.AreEqual(3, seq.Statistics.GetCount('Y'));
            Assert.AreEqual(0, seq.Statistics.GetCount('W'));
            Assert.AreEqual(1, seq.Statistics.GetCount(Alphabets.DNA.LookupBySymbol('G')));
            Assert.AreEqual(.2, seq.Statistics.GetFraction('T'));
            Assert.AreEqual(.3, seq.Statistics.GetFraction(Alphabets.DNA.LookupBySymbol('y')));

            // Test adding.
            seq.IsReadOnly = false;
            seq.Insert(1, 'a');                                 // "gaattacayyy"
            seq.Insert(1, Alphabets.DNA.LookupBySymbol('a'));   // "gaaattacayyy"
            seq.Add(Alphabets.DNA.LookupBySymbol('a'));         // "gaaattacayyya"
            Assert.AreEqual(6, seq.Statistics.GetCount('A'));
            seq.InsertRange(1, "ccc");                          // "gcccaaattacayyya"
            Assert.AreEqual(4, seq.Statistics.GetCount('c'));

            // Test removing.
            seq.RemoveRange(1, 3);                              // "gaaattacayyya"
            Assert.AreEqual(1, seq.Statistics.GetCount('c'));
            seq.RemoveAt(1);                                    // "gaattacayyya"
            seq.Remove(Alphabets.DNA.LookupBySymbol('a'));      // "gattacayyya"
            Assert.AreEqual(4, seq.Statistics.GetCount('A'));

            // Test derived sequences.
            ISequence range = seq.Range(2, 4);                  // "ttac"
            Assert.AreEqual(2, range.Statistics.GetCount('T'));
            Assert.AreEqual(1, range.Statistics.GetCount('A'));
            Assert.AreEqual(1, range.Statistics.GetCount('C'));
            ISequence rangeComplement = range.Complement;       // "aatg"
            Assert.AreEqual(2, rangeComplement.Statistics.GetCount('A'));
            Assert.AreEqual(1, rangeComplement.Statistics.GetCount('t'));
            Assert.AreEqual(1, rangeComplement.Statistics.GetCount('g'));

            // Test replacing.
            seq.Replace(2, 'a');                                // "gaatacayyya"
            Assert.AreEqual(5, seq.Statistics.GetCount('A'));
            seq.ReplaceRange(2, "ccc");                         // "gaccccayyya"
            Assert.AreEqual(3, seq.Statistics.GetCount('A'));
            Assert.AreEqual(0, seq.Statistics.GetCount('t'));
            Assert.AreEqual(4, seq.Statistics.GetCount('c'));

            // Test that derived sequences have been updated.
            // range == "cccc"
            // rangeComplement == "gggg"
            Assert.AreEqual(4, rangeComplement.Statistics.GetCount('g'));
            Assert.AreEqual(4, range.Statistics.GetCount('C'));
        }
    }
}
