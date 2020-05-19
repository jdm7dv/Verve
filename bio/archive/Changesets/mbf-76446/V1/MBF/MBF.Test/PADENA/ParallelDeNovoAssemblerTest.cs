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
using MBF.Util.Logging;
using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// Builds scaffold sequence. 
    /// </summary>
    [TestFixture]
    public class ParallelDeNovoAssemblerTest
    {
        static ParallelDeNovoAssemblerTest()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Test Assembler method in ParallelDeNovoAssembler
        /// </summary>
        [Test]
        public void AssemblerTest()
        {
            const int KmerLength = 11;
            const int DangleThreshold = 3;
            const int RedundantThreshold = 10;

            List<ISequence> readSeqs = TestInputs.GetDanglingReads();
            ParallelDeNovoAssembler assembler = new ParallelDeNovoAssembler();
            assembler.KmerLength = KmerLength;
            assembler.DanglingLinksThreshold = DangleThreshold;
            assembler.RedundantPathLengthThreshold = RedundantThreshold;
            IDeNovoAssembly result = assembler.Assemble(readSeqs);
            
            // Compare the two graphs
            Assert.AreEqual(1, result.AssembledSequences.Count);
            HashSet<string> expectedContigs = new HashSet<string>() 
            { 
                "ATCGCTAGCATCGAACGATCATT" 
            };

            foreach (ISequence contig in result.AssembledSequences)
            {
                Assert.IsTrue(expectedContigs.Contains(contig.ToString()));
            }
        }
    }
}
