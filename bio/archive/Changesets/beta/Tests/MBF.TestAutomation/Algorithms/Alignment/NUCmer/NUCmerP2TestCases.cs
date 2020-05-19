// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * NUCmerP2TestCases.cs
 * 
 *   This file contains NUCmer P2 test cases
 * 
***************************************************************************/

using System;
using System.Collections.Generic;

using MBF.Algorithms.Alignment;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.Algorithms.Alignment
{
    /// <summary>
    /// NUCmer P2 Test case implementation.
    /// </summary>
    [TestClass]
    public class NUCmerP2TestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\NUCmerTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static NUCmerP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region InValidate Align NUCmer Test Cases

        /// <summary>
        /// Validate Align() method with empty QueryList
        /// and validate the aligned sequences
        /// Input : Empty QueryList sequences
        /// Validation : Validate the Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void NUCmerAlignEmptyQueryList()
        {
            ValidateNUCmerEmptyAlignGeneralTestCases(null);
        }

        /// <summary>
        /// Validate Align() method with empty ReferenceSequence
        /// and validate the aligned sequences
        /// Input : Empty Reference Sequence
        /// Validation : Validate the Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void NUCmerAlignEmptyReferenceList()
        {
            ValidateNUCmerEmptyAlignGeneralTestCases(null);
        }

        /// <summary>
        /// Validate Align() method with Zero MUMLength
        /// and validate the aligned sequences
        /// Input : Zero MUMLength
        /// Validation : Validate the Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void NUCmerAlignInvalidMumLength()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.InvalidMumLengthSequence);
        }

        /// <summary>
        /// Validate Align() method with MUMLength
        /// Greater than ReferenceSequence
        /// and validate the aligned sequences
        /// Input : MUMLength Greater than Sequences
        /// Validation : Validate the Exception
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void NUCmerAlignGreaterMumLength()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.GreaterMumLengthSequence);
        }

        #endregion InValidate Align NUCmer Test Cases

        #region Supported Methods

        /// <summary>
        /// Validates the NUCmer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="sequenceList">List of Input Sequences</param>
        private static void ValidateNUCmerEmptyAlignGeneralTestCases(IList<ISequence> sequenceList)
        {
            NUCmer nucmerObj = new NUCmer3();
            IList<IPairwiseSequenceAlignment> result = nucmerObj.Align(sequenceList);
            Assert.AreEqual(result, null,
                "NUCmer P2: Successfully validate attributes with empty SequenceList");
        }

        /// <summary>
        /// Validates the NUCmer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        void ValidateNUCmerAlignGeneralTestCases(string nodeName)
        {
            string mumLength = String.Empty;
            bool exThrown = false;
            string[] referenceSequences = null;
            string[] searchSequences = null;
            List<ISequence> refSeqList = new List<ISequence>();
            List<ISequence> searchSeqList = new List<ISequence>();

            // Gets the reference & search sequences from the configurtion file
            referenceSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.ReferenceSequencesNode);
            searchSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.SearchSequencesNode);

            IAlphabet seqAlphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));

            for (int i = 0; i < referenceSequences.Length; i++)
            {
                ISequence referSeq = new Sequence(seqAlphabet, referenceSequences[i]);
                refSeqList.Add(referSeq);
            }

            for (int i = 0; i < searchSequences.Length; i++)
            {
                ISequence searchSeq = new Sequence(seqAlphabet, searchSequences[i]);
                searchSeqList.Add(searchSeq);
            }

            NUCmer nucmerObj = new NUCmer3();
            mumLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMLengthNode);

            // Update other values for NUCmer object
            nucmerObj.MaximumSeparation = 0;
            nucmerObj.MinimumScore = 2;
            nucmerObj.SeparationFactor = 0.12f;
            nucmerObj.BreakLength = 2;
            nucmerObj.LengthOfMUM = long.Parse(mumLength, null);
            nucmerObj.MinimumScore = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.MinimumScoreNode), (IFormatProvider)null);

            try
            {
                nucmerObj.Align(refSeqList, searchSeqList);
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                exThrown = true;
                Assert.IsTrue(exThrown, String.Format((IFormatProvider)null,
                    "NUCmer P2: {0}", ex.Message));
            }
        }

        #endregion Supported Methods
    }
}
