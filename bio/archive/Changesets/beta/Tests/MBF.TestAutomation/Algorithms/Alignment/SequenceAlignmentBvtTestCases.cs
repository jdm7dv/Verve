// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Runtime.Serialization;
using MBF.Algorithms.Alignment;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.Algorithms.Alignment
{
    /// <summary>
    /// Sequence Alignment algorithm Bvt test cases
    /// </summary>
    [TestClass]
    public class SequenceAlignmentBvtTestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SequenceAlignmentBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion

        #region Test Cases

        /// <summary>
        /// Validate GetObjectData() method with valid values
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSequenceAlignmentGetObjectData()
        {
            SerializationInfo info =
               new SerializationInfo(typeof(Sequence),
                   new FormatterConverter());
            StreamingContext context =
                new StreamingContext(StreamingContextStates.All);

            SequenceAlignment seqAlignmentObj = new SequenceAlignment();
            seqAlignmentObj.GetObjectData(info, context);
            Assert.AreEqual(4, info.MemberCount);
        }

        #endregion
    }
}