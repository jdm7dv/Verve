// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * ComparativeP2TestCases.cs
 * 
 *  This file contains the Comparative P2 test cases.
 * 
***************************************************************************/

using Bio;
using Bio.Algorithms.Assembly.Comparative;
using Bio.Algorithms.Alignment;
using Bio.Algorithms.Assembly.Padena.Scaffold;
using Bio.IO.FastA;
using Bio.Util.Logging;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bio.TestAutomation.Util;

namespace Bio.TestAutomation.Algorithms.Assembly.Comparative
{
    /// <summary>
    /// Comparative P2 test cases
    /// </summary>
    [TestClass]
    public class ComparativeP2TestCases
    {
        #region Global Variables

        Utility utilityObj = new Utility(@"TestUtils\ComparativeTestData\ComparativeTestsConfig.xml");
        ASCIIEncoding encodingObj = new ASCIIEncoding();

        #endregion Global Variables

        # region enum

        public enum ExceptionTypes
        {
            RefineLayoutNullDeltas,
            ResolveAmbiguityNullDeltas,            
            ConsensusWithNullDelta,
        };


        # endregion enum

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static ComparativeP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        # region Comparative P2 test cases

        /// <summary>
        /// Validate Generate Consensus() with null value as Delta Alignment.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateGenerateConsensusMethodWithNullDeltas()
        {
            string actualException = null;
            
            try
            {
                ConsensusGeneration.GenerateConsensus(null);
                Assert.Fail();
                ApplicationLog.WriteLine("GenerateConsensus P2 : Exception not thrown");
            }
            catch (ArgumentNullException ex)
            {
                actualException = ex.Message;
                ApplicationLog.WriteLine(
                    "GenerateConsensus P2 : Successfully validated the exception");
                Console.WriteLine(
                    "GenerateConsensus P2 : Successfully validated the exception");
            }
            
            string expectedMessage = GetExpectedErrorMessageWithInvalidSequenceType(Constants.SimpleFastaNodeName, ExceptionTypes.ConsensusWithNullDelta);
            Assert.AreEqual(expectedMessage.Trim(), actualException.Trim());            
        }


        /// <summary>
        /// Validate RefineLayout() with null as Delta Alignment.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateRefineLayoutMethodWithNullDeltas()
        {
            ValidateRefineLayoutMethod(Constants.RefineLayout, ExceptionTypes.RefineLayoutNullDeltas);
        }

        /// <summary>
        /// Validate ResolveAmbiguity() with null value as Delta Alignment.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void ValidateResolveAmbiguityMethodWithNullDeltas()
        {           
            string actualException = null;

            try
            {
                RepeatResolver.ResolveAmbiguity(null);
                Assert.Fail();
                ApplicationLog.WriteLine("Resolve Ambiguity P2 : Exception not thrown");
            }
            catch (ArgumentNullException ex)
            {
                actualException = ex.Message;
                ApplicationLog.WriteLine(
                    "Resolve Ambiguity P2 : Successfully validated the exception");
                Console.WriteLine(
                    "Resolve Ambiguity P2 : Successfully validated the exception");
            }

            string expectedMessage = GetExpectedErrorMessageWithInvalidSequenceType(Constants.RepeatResolution, ExceptionTypes.ResolveAmbiguityNullDeltas);
            Assert.AreEqual(expectedMessage, actualException);
        }
      
        # endregion Comparative P2 test cases

        #region Supporting methods

        /// <summary>
        /// Aligns reads to reference genome using NUCmer.
        /// </summary>
        /// <param name="nodeName">Name of parent Node which contains the data in xml.</param>
        /// <param name="isFilePath">Represents sequence is in a file or not.</param>
        /// <returns>Delta i.e. output from NUCmer</returns>      
        IList<DeltaAlignment> GetDeltaAlignment(string nodeName, bool isFilePath)
        {
            string[] referenceSequences = null;
            string[] searchSequences = null;

            List<ISequence> referenceSeqList = new List<ISequence>();
            List<ISequence> searchSeqList = new List<ISequence>();

            if (isFilePath)
            {
                // Gets the reference sequence from the FastA file
                string filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode1);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Comparative P2 : Successfully validated the File Path '{0}'.", filePath));

                using (FastAParser parser = new FastAParser(filePath))
                {
                    IEnumerable<ISequence> referenceList = parser.Parse();

                    foreach (ISequence seq in referenceList)
                    {
                        referenceSeqList.Add(seq);
                    }

                    // Gets the query sequence from the FastA file
                    string queryFilePath = utilityObj.xmlUtil.GetTextValue(nodeName,
                        Constants.FilePathNode2);

                    Assert.IsNotNull(queryFilePath);
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "Comparative P2 : Successfully validated the File Path '{0}'.", queryFilePath));

                    using (FastAParser queryParser = new FastAParser(queryFilePath))
                    {
                        IEnumerable<ISequence> querySeqList = queryParser.Parse();

                        foreach (ISequence seq in querySeqList)
                        {
                            searchSeqList.Add(seq);
                        }
                    }
                }
            }
            else
            {
                // Gets the reference & search sequences from the configurtion file
                referenceSequences = utilityObj.xmlUtil.GetTextValues(nodeName,
                    Constants.ReferenceSequencesNode);
                searchSequences = utilityObj.xmlUtil.GetTextValues(nodeName,
                  Constants.SearchSequencesNode);

                IAlphabet seqAlphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName,
                       Constants.AlphabetNameNode));

                for (int i = 0; i < referenceSequences.Length; i++)
                {
                    ISequence referSeq = new Sequence(seqAlphabet, encodingObj.GetBytes(referenceSequences[i]));
                    referenceSeqList.Add(referSeq);
                }

                for (int i = 0; i < searchSequences.Length; i++)
                {
                    ISequence searchSeq = new Sequence(seqAlphabet, encodingObj.GetBytes(searchSequences[i]));
                    searchSeqList.Add(searchSeq);
                }
            }

            NUCmer nucmerAligner = new NUCmer();

            string fixedSeparation = utilityObj.xmlUtil.GetTextValue(nodeName,
                     Constants.FixedSeparationNode);
            string minimumScore = utilityObj.xmlUtil.GetTextValue(nodeName,
                     Constants.MinimumScoreNode);
            string separationFactor = utilityObj.xmlUtil.GetTextValue(nodeName,
                     Constants.SeparationFactorNode);
            string LengthOfMUM = utilityObj.xmlUtil.GetTextValue(nodeName,
                     Constants.MUMLengthNode);

            nucmerAligner.FixedSeparation = int.Parse(fixedSeparation); ;
            nucmerAligner.MinimumScore = int.Parse(minimumScore);
            nucmerAligner.SeparationFactor = int.Parse(separationFactor);
            nucmerAligner.LengthOfMUM = int.Parse(LengthOfMUM);

            var subResult = nucmerAligner.GetDeltaAlignments(referenceSeqList, searchSeqList);
            List<DeltaAlignment> result = new List<DeltaAlignment>();

            foreach (IEnumerable<DeltaAlignment> deltaList in subResult)
            {
                foreach (DeltaAlignment deltaalign in deltaList)
                {
                    result.Add(deltaalign);
                }
            }

            return result;
        }

        /// <summary>
        /// Validate exceptions from RefineLayout() with null reads/null delta's.
        /// </summary>
        /// <param name="exceptionType">Layout refinement different parameters</param>
        /// <param name="deltaValues">Deltas for layout Refinemenr</param>
        public void ValidateRefineLayoutMethod(string nodeName, ExceptionTypes exceptionType)
        {
            string actualException = null;
                    
            try
            {
                LayoutRefiner.RefineLayout(null);
                Assert.Fail();
                ApplicationLog.WriteLine("RefineLayout P2 : Exception not thrown");
            }
            catch (ArgumentNullException ex)
            {
                actualException = ex.Message;
                ApplicationLog.WriteLine(
                    "Refine Layout P2 : Successfully validated the exception");
                Console.WriteLine(
                    "Refine Layout P2 : Successfully validated the exception");
            }

            string expectedMessage = GetExpectedErrorMessageWithInvalidSequenceType(nodeName, exceptionType);
            Assert.AreEqual(expectedMessage, actualException);
        }
        

        /// <summary>
        /// Gets the expected error message for invalid sequence type.
        /// </summary>
        /// <param name="nodeName">xml node</param>
        /// <param name="invalidType">Invalid sequence type.</param>
        /// <returns>Returns expected error message</returns>
        string GetExpectedErrorMessageWithInvalidSequenceType(string nodeName,
            ExceptionTypes sequenceType)
        {
            string expectedErrorMessage = string.Empty;
            switch (sequenceType)
            {
                case ExceptionTypes.RefineLayoutNullDeltas:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                        Constants.RefineLayoutWithNullDeltasNode);
                    break;                
                case ExceptionTypes.ResolveAmbiguityNullDeltas:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                        Constants.ResolveAmbiguityWithNullDeltasNode);
                    break;
                case ExceptionTypes.ConsensusWithNullDelta:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                        Constants.ConsensusWithNullDeltaNode);
                    break;                                    
                default:
                    expectedErrorMessage = utilityObj.xmlUtil.GetTextValue(nodeName,
                        Constants.ExpectedErrorMessage);
                    break;
            }

            return expectedErrorMessage;
        }

        #endregion Supporting methods
    }
}
