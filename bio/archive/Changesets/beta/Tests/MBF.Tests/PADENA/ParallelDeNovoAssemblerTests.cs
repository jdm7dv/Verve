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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.Tests
{
    /// <summary>
    /// Builds scaffold sequence. 
    /// </summary>
    [TestClass]
    public class ParallelDeNovoAssemblerTests
    {
        static ParallelDeNovoAssemblerTests()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.tests.log");
            }
        }

        /// <summary>
        /// Test Assembler method in ParallelDeNovoAssembler
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void AssemblerTest()
        {
            const int KmerLength = 11;
            const int DangleThreshold = 3;
            const int RedundantThreshold = 10;

            List<ISequence> readSeqs = TestInputs.GetDanglingReads();
            using (ParallelDeNovoAssembler assembler = new ParallelDeNovoAssembler())
            {
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
}
