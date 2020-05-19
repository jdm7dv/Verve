// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using MBF.Algorithms.Assembly;
using MBF.Algorithms.Assembly.PaDeNA;
using MBF.Util;
using MBF.Util.Logging;
using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// Builds scaffold sequence. 
    /// </summary>
    [TestFixture]
    public class ParallelDeNovoAssemblerWithScaffoldBuilder
    {
        static ParallelDeNovoAssemblerWithScaffoldBuilder()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Test Class Scaffold Builder using 9 contigs.
        /// </summary>
        [Test]
        public void AssemblerTestWithScaffoldBuilder()
        {
            const int kmerLength = 6;
            const int dangleThreshold = 3;
            const int redundantThreshold = 7;

            ParallelDeNovoAssembler assembler = new ParallelDeNovoAssembler();
            assembler.KmerLength = kmerLength;
            assembler.DanglingLinksThreshold = dangleThreshold;
            assembler.RedundantPathLengthThreshold = redundantThreshold;
            
            assembler.ScaffoldRedundancy = 0;
            assembler.Depth = 3;
            CloneLibrary.Instance.AddLibrary("abc", (float)5, (float)20);

            PaDeNAAssembly result = (PaDeNAAssembly) assembler.Assemble(TestInputs.GetReadsForScaffolds(), true);

            Assert.AreEqual(10, result.ContigSequences.Count);
            HashSet<string> expectedContigs = new HashSet<string>
            {
               "GCGCGC",
               "TTTTTT",
               "TTTTTA",
               "TTTTAA",
               "TTTAAA",
               "ATGCCTCCTATCTTAGC",
               "TTTTAGC",
               "TTAGCGCG",
               "CGCGCCGCGC",
               "CGCGCG"
            };

            foreach (ISequence contig in result.ContigSequences)
            {
                string contigSeq = contig.ToString();
                Assert.IsTrue(
                    expectedContigs.Contains(contigSeq) ||
                    expectedContigs.Contains(contigSeq.GetReverseComplement(new char[contigSeq.Length])));
            }

            Assert.AreEqual(8, result.Scaffolds.Count);
            HashSet<string> expectedScaffolds = new HashSet<string>
            {
                "ATGCCTCCTATCTTAGCGCGC",
                "TTTTTT",
                "TTTTTA",
                "TTTTAA",
                "TTTAAA",
                "CGCGCCGCGC",
                "TTTTAGC",
                "CGCGCG"
            };

            foreach (ISequence scaffold in result.Scaffolds)
            {
                string scaffoldSeq = scaffold.ToString();
                Assert.IsTrue(
                    expectedScaffolds.Contains(scaffoldSeq) ||
                    expectedScaffolds.Contains(scaffoldSeq.GetReverseComplement(new char[scaffoldSeq.Length])));
            }
        }
    }
}
