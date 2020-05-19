// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * MUMmerBvtTestCases.cs
 * 
 *   This file contains the MUMmer Bvt test cases
 * 
***************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;

using MBF.Algorithms;
using MBF.Algorithms.Alignment;
using MBF.Algorithms.SuffixTree;
using MBF.IO.Fasta;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.Algorithms.Alignment
{
    /// <summary>
    /// MUMmer Bvt Test case implementation.
    /// </summary>
    [TestFixture]
    public class MUMmerBvtTestCases
    {
        #region Enums

        /// <summary>
        /// Lis Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum LISParameters
        {
            FindUniqueMatches,
            PerformLIS,
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static MUMmerBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\MUMmerTestsConfig.xml");
        }

        #endregion Constructor

        #region Suffix Tree Test Cases

        /// <summary>
        /// Validate BuildSuffixTree() method with one line sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : One line sequence
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [Test]
        public void SuffixTreeBuildSuffixTreeOneLineSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.OneLineSequenceNodeName, false);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with small size (less than 35kb) sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : small size sequence file
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [Test]
        public void SuffixTreeBuildSuffixTreeSmallSizeSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.SmallSizeSequenceNodeName, true);
        }

        /// <summary>
        /// Validate FindMatches() method with one line sequence 
        /// for both reference and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesOneLineSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineSequenceNodeName,
                false, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with small size (less than 35kb) sequence 
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : Small size sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void SuffixTreeFindMatchesSmallSizeSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.SmallSizeSequenceNodeName,
                true, LISParameters.FindUniqueMatches);
        }

        #endregion Suffix Tree Test Cases

        #region Longest Increasing Sub Sequence

        /// <summary>
        /// Validate GetLongestSequence() method with one line sequence 
        /// for both reference and query parameter and validate
        /// the unique matches
        /// Input : One sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void LongestIncreasingSubsequenceOneUniqueMatch()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineSequenceNodeName,
                false, LISParameters.PerformLIS);
        }

        /// <summary>
        /// Validate GetLongestSequence() method with two matches
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : One sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void LongestIncreasingSubsequenceTwoUniqueMatchNoOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineTwoMatchSequenceNodeName,
                false, LISParameters.PerformLIS);
        }

        /// <summary>
        /// Validate GetLongestSequence() method with cross overlap matches
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : One sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [Test]
        public void LongestIncreasingSubsequenceTwoUniqueMatchOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineTwoMatchOverlapSequenceNodeName,
                false, LISParameters.PerformLIS);
        }

        #endregion Longest Increasing Sub Sequence

        #region MUMmer Align Test Cases

        /// <summary>
        /// Validate Align() method with one line sequence 
        /// and validate the aligned sequences
        /// Input : One line sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignOneLineSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.OneLineSequenceNodeName, 
                false,false);
        }

        /// <summary>
        /// Validate Align() method with small size (less than 35kb) sequence 
        /// and validate the aligned sequences
        /// Input : small size sequence file
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignSmallSizeSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.SmallSizeSequenceNodeName, 
                true,false);
        }

        /// <summary>
        /// Validate Align(QuerySeqList) method with one line sequence 
        /// and validate the aligned sequences
        /// Input : One line multiple sequences
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [Test]
        public void MUMmerAlignQuerySeqList()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.QuerySeqeunceListNode, false, true);
        }

        /// <summary>
        /// Validate All properties in MUMmer class
        /// Input : One line sequence and update all properties
        /// Validation : Validate the properties
        /// </summary>
        [Test]
        public void MUMmerProperties()
        {
            MUMmer mum = new MUMmer3();
            Assert.AreEqual(Constants.MUMDescription, mum.Description);
            Assert.AreEqual(Constants.MUMLength, mum.LengthOfMUM.ToString((IFormatProvider)null));
            Assert.AreEqual(Constants.MUMName, mum.Name);
            Assert.AreEqual(Constants.MUMPairWiseAlgorithm, mum.PairWiseAlgorithm.ToString());
            Assert.AreEqual(Constants.MUMRefSeqNumber,
                mum.ReferenceSequenceNumber.ToString((IFormatProvider)null));
            Assert.AreEqual(Constants.MUMGapOpenCost, mum.GapOpenCost.ToString((IFormatProvider)null));

            Console.WriteLine("Successfully validated all the properties of MUMmer class.");
            ApplicationLog.WriteLine("Successfully validated all the properties of MUMmer class.");
        }

        #endregion MUMmer Align Test Cases

        #region MUMs validation Test Cases

        /// <summary>
        /// Validate GetMUMs() method with one line sequence 
        /// and validate the Mumms.
        /// Input : One line sequence
        /// Validation : Validate the MUMs Output.
        /// </summary>
        [Test]
        public void ValidateMUMsWithOneLineSequence()
        {
            ValidateMUMsGeneralTestCases(Constants.OneLineSequenceNodeName, 
                false, false, false);
        }

        /// <summary>
        /// Validate GetMUMs() method with one line sequence 
        /// and validate the MUMs up to LIS.
        /// Input : One line sequence
        /// Validation : Validate the MUMs Output.
        /// </summary>
        [Test]
        public void ValidateMUMsUpToLISWithOneLineSequence()
        {
            ValidateMUMsGeneralTestCases(Constants.OneLineSequenceNodeName,
                false, false, true);
        }

        /// <summary>
        /// Validate GetMUMs() method with one line sequence 
        /// and validate the MUMs after LIS.
        /// Input : One line sequence
        /// Validation : Validate the MUMs Output.
        /// </summary>
        [Test]
        public void ValidateMUMsAfterLISWithOneLineSequence()
        {
            ValidateMUMsGeneralTestCases(Constants.OneLineSequenceNodeName,
                false, true, true);
        }

        #endregion MUMs validation Test Cases
        
        #region Supported Methods

        /// <summary>
        /// Validates most of the build suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is file path?</param>
        static void ValidateBuildSuffixTreeGeneralTestCases(string nodeName, bool isFilePath)
        {
            ISequence referenceSeq = null;
            string referenceSequence = string.Empty;
            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format(null,
                    "MUMmer BVT : Successfully validated the File Path '{0}'.", filePath));

                FastaParser parser = new FastaParser();
                IList<ISequence> referenceSeqs = parser.Parse(filePath);
                referenceSeq = referenceSeqs[0];
                referenceSequence = referenceSeq.ToString();
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode)), referenceSequence);
            }

            // Builds the suffix for the reference sequence passed.
            ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
            SequenceSuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSeq);

            // Validates the edges for a given sequence.
            ApplicationLog.WriteLine("MUMmer BVT : Validating the Edges");
            Assert.IsTrue(ValidateEdges(suffixTree, nodeName));
            Console.WriteLine(string.Format(null,
                "MUMmer BVT : Successfully validated the all the Edges for the sequence '{0}'.",
                referenceSequence));
            ApplicationLog.WriteLine(string.Format(null,
                "MUMmer BVT : Successfully validated the all the Edges for the sequence '{0}'.",
                referenceSequence));

            Assert.AreEqual(suffixTree.Sequence.ToString(), referenceSequence);
            Console.WriteLine(string.Format(null,
                "MUMmer BVT : Successfully validated the Suffix Tree properties for the sequence '{0}'.",
                referenceSequence));
            ApplicationLog.WriteLine(string.Format(null,
                "MUMmer BVT : Successfully validated the Suffix Tree properties for the sequence '{0}'.",
                referenceSequence));
        }

        /// <summary>
        /// Validates most of the find matches suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is File Path?</param>
        /// <param name="LISActionType">LIS action type enum</param>
        static void ValidateFindMatchSuffixGeneralTestCases(string nodeName, bool isFilePath,
            LISParameters LISActionType)
        {
            ISequence referenceSeq = null;
            ISequence querySeq = null;
            string referenceSequence = string.Empty;
            string querySequence = string.Empty;

            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format(null,
                    "MUMmer BVT : Successfully validated the File Path '{0}'.", filePath));

                FastaParser parser = new FastaParser();
                IList<ISequence> referenceSeqs = parser.Parse(filePath);
                referenceSeq = referenceSeqs[0];
                referenceSequence = referenceSeq.ToString();

                // Gets the reference sequence from the configurtion file
                string queryFilePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceFilePathNode);

                Assert.IsNotNull(queryFilePath);
                ApplicationLog.WriteLine(string.Format(null,
                    "MUMmer BVT : Successfully validated the Search File Path '{0}'.", queryFilePath));

                FastaParser queryParser = new FastaParser();
                IList<ISequence> querySeqs = queryParser.Parse(queryFilePath);
                querySeq = querySeqs[0];
                querySequence = querySeq.ToString();
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                string seqAlp = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(seqAlp),
                    referenceSequence);

                querySequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceNode);

                seqAlp = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceAlphabetNode);

                querySeq = new Sequence(Utility.GetAlphabet(seqAlp),
                    querySequence);
            }

            string mumLength = Utility._xmlUtil.GetTextValue(nodeName, Constants.MUMLengthNode);

            // Builds the suffix for the reference sequence passed.
            ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
            SequenceSuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSeq);

            IList<MaxUniqueMatch> matches = suffixTreeBuilder.FindMatches(suffixTree, querySeq,
                long.Parse(mumLength, null));

            switch (LISActionType)
            {
                case LISParameters.FindUniqueMatches:
                    // Validates the Unique Matches.
                    ApplicationLog.WriteLine("MUMmer BVT : Validating the Unique Matches");
                    Assert.IsTrue(ValidateUniqueMatches(matches, nodeName, LISActionType));
                    break;
                case LISParameters.PerformLIS:
                    // Validates the Unique Matches.
                    ApplicationLog.WriteLine("MUMmer BVT : Validating the Unique Matches using LIS");
                    LongestIncreasingSubsequence lisObj = new LongestIncreasingSubsequence();
                    IList<MaxUniqueMatch> lisMatches = lisObj.GetLongestSequence(matches);
                    Assert.IsTrue(ValidateUniqueMatches(lisMatches, nodeName, LISActionType));
                    break;
                default:
                    break;
            }

            Console.WriteLine(string.Format(null,
                "MUMmer BVT : Successfully validated the all the unique matches for the sequence '{0}' and '{1}'.",
                referenceSequence, querySequence));
            ApplicationLog.WriteLine(string.Format(null,
                "MUMmer BVT : Successfully validated the all the unique matches for the sequence '{0}' and '{1}'.",
                referenceSequence, querySequence));
        }

        /// <summary>
        /// Validates the Mummer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isSeqList">Is MUMmer alignment with List of sequences</param>
        static void ValidateMUMmerAlignGeneralTestCases(string nodeName, bool isFilePath,
            bool isSeqList)
        {
            ISequence referenceSeq = null;
            ISequence querySeq = null;
            IList<ISequence> querySeqs = null;
            string referenceSequence = string.Empty;
            string querySequence = string.Empty;
            IList<IPairwiseSequenceAlignment> align = null;

            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format(null,
                    "MUMmer BVT : Successfully validated the File Path '{0}'.", filePath));

                FastaParser parser = new FastaParser();
                IList<ISequence> referenceSeqs = parser.Parse(filePath);
                referenceSeq = referenceSeqs[0];
                referenceSequence = referenceSeq.ToString();

                // Gets the reference sequence from the configurtion file
                string queryFilePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceFilePathNode);

                Assert.IsNotNull(queryFilePath);
                ApplicationLog.WriteLine(string.Format(null,
                    "MUMmer BVT : Successfully validated the Search File Path '{0}'.", queryFilePath));

                FastaParser queryParser = new FastaParser();
                querySeqs = queryParser.Parse(queryFilePath);
                querySeq = querySeqs[0];
                querySequence = querySeq.ToString();
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                string referenceSeqAlphabet = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(referenceSeqAlphabet),
                    referenceSequence);

                querySequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceNode);

                referenceSeqAlphabet = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceAlphabetNode);

                querySeq = new Sequence(Utility.GetAlphabet(referenceSeqAlphabet),
                    querySequence);
                querySeqs = new List<ISequence>();
                querySeqs.Add(querySeq);
            }

            string mumLength = Utility._xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode);

            MUMmer mum = new MUMmer3();
            mum.LengthOfMUM = long.Parse(mumLength, null);
            mum.PairWiseAlgorithm = new NeedlemanWunschAligner();
            mum.GapOpenCost = int.Parse(Utility._xmlUtil.GetTextValue(nodeName, Constants.GapOpenCostNode),
                (IFormatProvider)null);

            if (isSeqList)
            {
                querySeqs.Add(referenceSeq);
                align = mum.Align(querySeqs);
            }
            else
            {
                align = mum.AlignSimple(referenceSeq, querySeqs);
            }

            string expectedScore = Utility._xmlUtil.GetTextValue(nodeName, Constants.ScoreNodeName);
            Assert.AreEqual(expectedScore,
                align[0].PairwiseAlignedSequences[0].Score.ToString((IFormatProvider)null));
            Console.WriteLine(string.Format(null,
                "MUMmer BVT : Successfully validated the score for the sequence '{0}' and '{1}'.",
                referenceSequence, querySequence));
            ApplicationLog.WriteLine(string.Format(null,
                "MUMmer BVT : Successfully validated the score for the sequence '{0}' and '{1}'.",
                referenceSequence, querySequence));

            string[] expectedSequences = Utility._xmlUtil.GetTextValues(nodeName,
                Constants.ExpectedSequencesNode);

            IList<IPairwiseSequenceAlignment> expectedOutput = new List<IPairwiseSequenceAlignment>();

            IPairwiseSequenceAlignment seqAlign = new PairwiseSequenceAlignment();
            PairwiseAlignedSequence alignedSeq = new PairwiseAlignedSequence();
            alignedSeq.FirstSequence = new Sequence(referenceSeq.Alphabet, expectedSequences[0]);
            alignedSeq.SecondSequence = new Sequence(referenceSeq.Alphabet, expectedSequences[1]);
            alignedSeq.Score = int.Parse(expectedScore);
            seqAlign.PairwiseAlignedSequences.Add(alignedSeq);
            expectedOutput.Add(seqAlign);
            Assert.IsTrue(CompareAlignment(align, expectedOutput));
            Console.WriteLine("MUMmer BVT : Successfully validated the aligned sequences.");
            ApplicationLog.WriteLine("MUMmer BVT : Successfully validated the aligned sequences.");
        }

        /// <summary>
        /// Validates the edges for the suffix tree and the node name specified.
        /// </summary>
        /// <param name="suffixTree">Suffix Tree.</param>
        /// <param name="nodeName">Node name which needs to be read for validation</param>
        /// <returns>True, if successfully validated.</returns>
        static bool ValidateEdges(SequenceSuffixTree suffixTree, string nodeName)
        {
            Dictionary<int, Edge> ed = suffixTree.Edges;

            string[] actualStrtIndexes = new string[ed.Count];
            string[] actualEndIndexes = new string[ed.Count];

            int j = 0;
            foreach (int col in ed.Keys)
            {
                Edge a = ed[col];
                actualStrtIndexes[j] = a.StartIndex.ToString();
                actualEndIndexes[j] = a.EndIndex.ToString();
                j++;
            }

            // Gets the sorted edge list for the actual Edge list
            List<Edge> actualEdgeList = GetSortedEdges(actualStrtIndexes, actualEndIndexes);

            // Gets all the edges to be validated as in xml.
            string[] startIndexes =
                Utility._xmlUtil.GetTextValue(nodeName, Constants.EdgeStartIndexesNode).Split(',');
            string[] endIndexes =
                Utility._xmlUtil.GetTextValue(nodeName, Constants.EdgeEndIndexesNode).Split(',');

            // Gets the sorted edge list for the expected Edge list
            List<Edge> expectedEdgeList = GetSortedEdges(startIndexes, endIndexes);

            Console.WriteLine(string.Format(null,
                "MUMmer BVT : Total Edges Found is : '{0}'", ed.Keys.Count.ToString((IFormatProvider)null)));
            ApplicationLog.WriteLine(string.Format(null,
                "MUMmer BVT : Total Edges Found is : '{0}'", ed.Keys.Count.ToString((IFormatProvider)null)));

            // Loops through all the edges and validates the same.
            for (int i = 0; i < expectedEdgeList.Count; i++)
            {
                if (!(actualEdgeList[i].StartIndex == expectedEdgeList[i].StartIndex)
                    && (actualEdgeList[i].EndIndex == expectedEdgeList[i].EndIndex))
                {
                    Console.WriteLine(string.Format(null,
                        "MUMmer BVT : Edges not matching at index '{0}'", i.ToString()));
                    ApplicationLog.WriteLine(string.Format(null,
                        "MUMmer BVT : Edges not matching at index '{0}'", i.ToString()));
                    return false;
                }

                i++;
            }

            return true;
        }

        /// <summary>
        /// Gets the Sorted Edge for the given start and end indexes
        /// </summary>
        /// <param name="startIndexes">Start Index</param>
        /// <param name="endIndexes">End Index</param>
        /// <returns>Sorted Edge list</returns>
        static List<Edge> GetSortedEdges(string[] startIndexes, string[] endIndexes)
        {
            List<Edge> edgList = new List<Edge>();

            // Loops through all the indexes and creates EdgeList.
            for (int i = 0; i < startIndexes.Length; i++)
            {
                Edge edg = new Edge();
                edg.StartIndex = int.Parse(startIndexes[i]);
                edg.EndIndex = int.Parse(endIndexes[i]);

                edgList.Add(edg);
            }

            List<Edge> sortedEdgeList =
                edgList.OrderBy(stEd => stEd.StartIndex).ThenBy(endEd => endEd.EndIndex).ToList();

            return sortedEdgeList;
        }

        /// <summary>
        /// Validates the Unique Matches for the input provided.
        /// </summary>
        /// <param name="matches">Max Unique Match list</param>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="LISActionType">Unique Match/Sub level LIS/LIS</param>
        /// <returns>True, if successfully validated</returns>
        static bool ValidateUniqueMatches(IList<MaxUniqueMatch> matches,
            string nodeName, LISParameters LISActionType)
        {
            // Gets all the unique matches properties to be validated as in xml.
            string[] firstSeqOrder = null;
            string[] firstSeqStart = null;
            string[] length = null;
            string[] secondSeqOrder = null;
            string[] secondSeqStart = null;

            switch (LISActionType)
            {
                case LISParameters.PerformLIS:
                    firstSeqOrder = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.LisFirstSequenceMumOrderNode).Split(',');
                    firstSeqStart = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.LisFirstSequenceStartNode).Split(',');
                    length = Utility._xmlUtil.GetTextValue(nodeName, Constants.LisLengthNode).Split(',');
                    secondSeqOrder = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.LisSecondSequenceMumOrderNode).Split(',');
                    secondSeqStart = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.LisSecondSequenceStartNode).Split(',');
                    break;
                case LISParameters.FindUniqueMatches:
                    firstSeqOrder = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.FirstSequenceMumOrderNode).Split(',');
                    firstSeqStart = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.FirstSequenceStartNode).Split(',');
                    length = Utility._xmlUtil.GetTextValue(nodeName, Constants.LengthNode).Split(',');
                    secondSeqOrder = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.SecondSequenceMumOrderNode).Split(',');
                    secondSeqStart = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.SecondSequenceStartNode).Split(',');
                    break;
                default:
                    break;
            }

            int i = 0;
            // Loops through all the matches and validates the same.
            foreach (MaxUniqueMatch match in matches)
            {
                if ((0 != string.Compare(firstSeqOrder[i],
                    match.FirstSequenceMumOrder.ToString((IFormatProvider)null), true, CultureInfo.CurrentCulture))
                    || (0 != string.Compare(firstSeqStart[i],
                    match.FirstSequenceStart.ToString((IFormatProvider)null), true, CultureInfo.CurrentCulture))
                    || (0 != string.Compare(length[i],
                    match.Length.ToString((IFormatProvider)null), true, CultureInfo.CurrentCulture))
                    || (0 != string.Compare(secondSeqOrder[i],
                    match.SecondSequenceMumOrder.ToString((IFormatProvider)null), true, CultureInfo.CurrentCulture))
                    || (0 != string.Compare(secondSeqStart[i],
                    match.SecondSequenceStart.ToString((IFormatProvider)null), true, CultureInfo.CurrentCulture)))
                {
                    Console.WriteLine(string.Format(null,
                        "MUMmer BVT : Unique match not matching at index '{0}'", i.ToString()));
                    ApplicationLog.WriteLine(string.Format(null,
                        "MUMmer BVT : Unique match not matching at index '{0}'", i.ToString()));
                    return false;
                }
                i++;
            }

            return true;
        }

        /// <summary>
        /// Compare the alignment of mummer and defined alignment
        /// </summary>
        /// <param name="result">output of Aligners</param>
        /// <param name="expectedAlignment">expected output</param>
        /// <returns>Compare result of alignments</returns>
        static bool CompareAlignment(IList<IPairwiseSequenceAlignment> actualAlignment,
            IList<IPairwiseSequenceAlignment> expectedAlignment)
        {
            bool output = true;

            if (actualAlignment.Count == expectedAlignment.Count)
            {
                for (int resultCount = 0; resultCount < actualAlignment.Count; resultCount++)
                {
                    if (actualAlignment[resultCount].PairwiseAlignedSequences.Count == expectedAlignment[resultCount].PairwiseAlignedSequences.Count)
                    {
                        for (int alignSeqCount = 0; alignSeqCount < actualAlignment[resultCount].PairwiseAlignedSequences.Count; alignSeqCount++)
                        {
                            // Validates the First Sequence, Second Sequence and Score
                            if (actualAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].FirstSequence.ToString().Equals(
                                    expectedAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].FirstSequence.ToString())
                                && actualAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].SecondSequence.ToString().Equals(
                                    expectedAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].SecondSequence.ToString())
                                && actualAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].Score ==
                                    expectedAlignment[resultCount].PairwiseAlignedSequences[alignSeqCount].Score)
                            {
                                output = true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return output;
        }

        /// <summary>
        /// Validate the Mummer GetMUMs method for different test cases.
        /// </summary>
        /// <param name="nodeName">Name of the XML node to be read.</param>
        /// <param name="isFilePath">Is Sequence saved in File</param>
        /// <param name="isAfterLIS">Is Mummer execution after LIS</param>
        /// <param name="isLIS">Is Mummer execution with LIS option</param>
        static void ValidateMUMsGeneralTestCases(string nodeName, bool isFilePath,
            bool isAfterLIS, bool isLIS)
        {
            ISequence referenceSeq = null;
            ISequence querySeq = null;
            IList<ISequence> querySeqs = null;
            string referenceSequence = string.Empty;
            string querySequence = string.Empty;

            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format(null, 
                    "MUMmer BVT : Successfully validated the File Path '{0}'.", filePath));

                FastaParser parser = new FastaParser();
                IList<ISequence> referenceSeqs = parser.Parse(filePath);
                referenceSeq = referenceSeqs[0];
                referenceSequence = referenceSeq.ToString();

                // Gets the reference sequence from the configurtion file
                string queryFilePath = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceFilePathNode);

                Assert.IsNotNull(queryFilePath);
                ApplicationLog.WriteLine(string.Format(null, 
                    "MUMmer BVT : Successfully validated the Search File Path '{0}'.", queryFilePath));

                FastaParser queryParser = new FastaParser();
                querySeqs = queryParser.Parse(queryFilePath);
                querySeq = querySeqs[0];
                querySequence = querySeq.ToString();
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                string referenceSeqAlphabet = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(referenceSeqAlphabet),
                    referenceSequence);

                querySequence = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceNode);

                referenceSeqAlphabet = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceAlphabetNode);

                querySeq = new Sequence(Utility.GetAlphabet(referenceSeqAlphabet),
                    querySequence);
                querySeqs = new List<ISequence>();
                querySeqs.Add(querySeq);
            }

            string mumLength = Utility._xmlUtil.GetTextValue(nodeName, Constants.MUMLengthNode);

            MUMmer mum = new MUMmer3();
            mum.LengthOfMUM = long.Parse(mumLength, null);
            IDictionary<ISequence, IList<MaxUniqueMatch>> actualResult = null;

            if (!isLIS)
            {
                actualResult = mum.GetMUMs(referenceSeq, querySeqs);
            }
            else
            {
                actualResult = mum.GetMUMs(referenceSeq, querySeqs, isAfterLIS);
            }

            // Validate MUMs output.
            Assert.IsTrue(ValidateMums(nodeName, actualResult, querySeq));

            Console.WriteLine("MUMmer BVT : Successfully validated the Mumms");
            ApplicationLog.WriteLine("MUMmer BVT : Successfully validated the Mumms.");
        }

        /// <summary>
        /// Validate the Mums output.
        /// </summary>
        /// <param name="result">Mumms Output</param>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="querySeq">Query Sequence</param>
        static bool ValidateMums(string nodeName,
            IDictionary<ISequence, IList<MaxUniqueMatch>> result, ISequence querySeq)
        {
            string[] firstSeqOrder = Utility._xmlUtil.GetTextValue(nodeName, Constants.LisFirstSequenceMumOrderNode).Split(',');
            string[] firstSeqStart = Utility._xmlUtil.GetTextValue(nodeName, Constants.LisFirstSequenceStartNode).Split(',');
            string[] length = Utility._xmlUtil.GetTextValue(nodeName, Constants.LisLengthNode).Split(',');
            string[] secondSeqOrder = Utility._xmlUtil.GetTextValue(nodeName, Constants.LisSecondSequenceMumOrderNode).Split(',');
            string[] secondSeqStart = Utility._xmlUtil.GetTextValue(nodeName, Constants.LisSecondSequenceStartNode).Split(',');

            foreach (var mum in result)
            {
                var mums = result[mum.Key];
                for (int i = 0; i < mums.Count; i++)
                {
                    if (0 != string.Compare(querySeq.ToString(), mums[i].Query.ToString())
                       || (0 != string.Compare(firstSeqOrder[i], mums[i].FirstSequenceMumOrder.ToString()))
                       || (0 != string.Compare(firstSeqStart[i], mums[i].FirstSequenceStart.ToString()))
                       || (0 != string.Compare(length[i], mums[i].Length.ToString()))
                       || (0 != string.Compare(secondSeqOrder[i], mums[i].SecondSequenceMumOrder.ToString()))
                       || (0 != string.Compare(secondSeqStart[i], mums[i].SecondSequenceStart.ToString())))
                    {
                        Console.WriteLine(string.Format(null, "MUMmer BVT : There is no match at '{0}'", i.ToString()));
                        ApplicationLog.WriteLine(string.Format(null, "MUMmer BVT : There is no match at '{0}'", i.ToString()));
                        return false;
                    }

                }
            }
            return true;
        }
        #endregion Supported Methods
    }
}
