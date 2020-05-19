// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using MBF.Algorithms.Alignment;
using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.Algorithms.Alignment
{
    /// <summary>
    /// Sequence Aligners BVT test cases
    /// </summary>
    [TestClass]
    public class SequenceAlignerBvtTestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SequenceAlignerBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion

        #region Test cases

        /// <summary>
        /// Validate if all the Aligners are creating respective objects
        /// Input : Sequence read from xml file.
        /// Validation : Added sequences are retrieved and validated.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSequenceAlignersAll()
        {
            MUMmer mumobj = new MUMmer3();
            Assert.AreEqual(mumobj.ToString(), SequenceAligners.MUMmer.ToString());
            NeedlemanWunschAligner nwAlignerobj = new NeedlemanWunschAligner();
            Assert.AreEqual(nwAlignerobj.ToString(), SequenceAligners.NeedlemanWunsch.ToString());
            NUCmer nucobj = new NUCmer3();
            Assert.AreEqual(nucobj.ToString(), SequenceAligners.NUCmer.ToString());
            PairwiseOverlapAligner poAlignerobj = new PairwiseOverlapAligner();
            Assert.AreEqual(poAlignerobj.ToString(), SequenceAligners.PairwiseOverlap.ToString());
            SmithWatermanAligner swAlignerobj = new SmithWatermanAligner();
            Assert.AreEqual(swAlignerobj.ToString(), SequenceAligners.SmithWaterman.ToString());
            Assert.IsNotNull(SequenceAligners.All);

            Console.Write("Successfully created all the objects in Sequence Aligners");
            ApplicationLog.Write("Successfully created all the objects in Sequence Aligners");
        }

        #endregion Test cases
    }
}
