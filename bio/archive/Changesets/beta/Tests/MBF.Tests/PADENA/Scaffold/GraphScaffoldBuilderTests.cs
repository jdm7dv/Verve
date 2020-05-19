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
using MBF.Algorithms.Assembly.Graph;
using MBF.Algorithms.Assembly.PaDeNA;
using MBF.Algorithms.Assembly.PaDeNA.Scaffold;
using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.Tests
{
    /// <summary>
    /// Builds scaffold sequence. 
    /// </summary>
    [TestClass]
    public class GraphScaffoldBuilderTests : ParallelDeNovoAssembler
    {
        static GraphScaffoldBuilderTests()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.tests.log");
            }
        }

        /// <summary>
        /// Test Class Scaffold Builder using 9 contigs.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void BuildScaffold()
        {
            const int kmerLength = 6;
            const int dangleThreshold = 3;
            const int redundantThreshold = 7;
            List<ISequence> sequences = TestInputs.GetReadsForScaffolds();

            KmerLength = kmerLength;
            SequenceReads.Clear();
            this.AddSequenceReads(sequences);
            CreateGraph();
            DanglingLinksThreshold = dangleThreshold;
            DanglingLinksPurger = new DanglingLinksPurger(dangleThreshold);
            RedundantPathLengthThreshold = redundantThreshold;
            RedundantPathsPurger = new RedundantPathsPurger(redundantThreshold);
            UnDangleGraph();
            RemoveRedundancy();

            IList<ISequence> contigs = BuildContigs();
            CloneLibrary.Instance.AddLibrary("abc", (float)5, (float)20);
            GraphScaffoldBuilder scaffold = new GraphScaffoldBuilder();
            IList<ISequence> scaffoldSeq = scaffold.BuildScaffold(
                sequences, contigs, this.KmerLength, 3, 0);

            Assert.AreEqual(scaffoldSeq.Count, 8);
            Assert.IsTrue(scaffoldSeq[0].ToString().Equals(
                "ATGCCTCCTATCTTAGCGCGC"));
            scaffold.Dispose();
        }
    }
}
