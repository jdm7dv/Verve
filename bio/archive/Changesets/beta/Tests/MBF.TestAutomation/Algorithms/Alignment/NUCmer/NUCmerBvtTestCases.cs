// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * NUCmerBvtTestCases.cs
 * 
 *   This file contains the NUCmer Bvt test cases
 * 
***************************************************************************/

using System;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Collections.Generic;

using MBF.IO.Fasta;
using MBF.Algorithms;
using MBF.Algorithms.Alignment;
using MBF.Algorithms.SuffixTree;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.Algorithms.Alignment
{
    /// <summary>
    /// NUCmer Bvt Test case implementation.
    /// </summary>
    [TestClass]
    public class NUCmerBvtTestCases
    {

        #region Enums

        /// <summary>
        /// Lis Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AdditionalParameters
        {
            FindUniqueMatches,
            PerformClusterBuilder
        };

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\NUCmerTestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static NUCmerBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Suffix Tree Test Cases

        /// <summary>
        /// Validate BuildSuffixTree() method with one line sequence 
        /// and validate the nodes, edges and the sequences
        /// Input : One line sequences
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void SuffixTreeBuildSuffixTreeOneLineSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.OneLineSequenceNodeName, false);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with small size (less than 35kb) sequence 
        /// and validate the nodes, edges and the sequences
        /// Input : small size sequence files
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void SuffixTreeBuildSuffixTreeSmallSizeSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.SmallSizeSequenceNodeName, true);
        }

        /// <summary>
        /// Validate FindMatches() method with one line sequences
        /// for both reference and query parameter and validate
        /// the unique matches
        /// Input : One line sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void SuffixTreeFindMatchesOneLineSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneLineSequenceNodeName,
                false, AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with small size (less than 35kb) sequences 
        /// for reference and query parameter and validate
        /// the unique matches
        /// Input : Small size sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void SuffixTreeFindMatchesSmallSizeSequence()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.SmallSizeSequenceNodeName, true,
                AdditionalParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate BuildCluster() method with one unique match
        /// and validate the clusters
        /// Input : one unique matches
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ClusterBuilderOneUniqueMatches()
        {
            ValidateFindMatchSuffixGeneralTestCases(Constants.OneUniqueMatchSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder);
        }

        /// <summary>
        /// Validate BuildCluster() method with two unique match
        /// without cross overlap and validate the clusters
        /// Input : two unique matches with out cross overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ClusterBuilderTwoUniqueMatchesWithoutCrossOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(
                Constants.TwoUniqueMatchWithoutCrossOverlapSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder);
        }

        /// <summary>
        /// Validate BuildCluster() method with two unique match
        /// with cross overlap and validate the clusters
        /// Input : two unique matches with cross overlap
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ClusterBuilderTwoUniqueMatchesWithCrossOverlap()
        {
            ValidateFindMatchSuffixGeneralTestCases(
                Constants.TwoUniqueMatchWithCrossOverlapSequenceNodeName,
                false, AdditionalParameters.PerformClusterBuilder);
        }

        #endregion Suffix Tree Test Cases

        #region NUCmer Align Test Cases

        /// <summary>
        /// Validate Align() method with one line sequence 
        /// and validate the aligned sequences
        /// Input : One line sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void NUCmerAlignOneLineSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.OneLineSequenceNodeName, false);
        }

        /// <summary>
        /// Validate Align() method with small size (less than 35kb) sequence 
        /// and validate the aligned sequences
        /// Input : small size sequence file
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void NUCmerAlignSmallSizeSequence()
        {
            ValidateNUCmerAlignGeneralTestCases(Constants.SmallSizeSequenceNodeName, true);
        }

        /// <summary>
        /// Validate Align() method with one line sequence 
        /// with cross over lap and validate the aligned sequences
        /// Input : One line sequence
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void NUCmerAlignSequenceWithCrossOverlap()
        {
            ValidateNUCmerAlignGeneralTestCases(
                Constants.TwoUniqueMatchWithCrossOverlapSequenceNodeName, false);
        }

        /// <summary>
        /// Validate All properties in NUCmer class
        /// Input : Create a NUCmer object.
        /// Validation : Validate the properties
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void NUCmerProperties()
        {
            NUCmer nucmerObj = new NUCmer3();
            Assert.AreEqual(Constants.NUCDescription, nucmerObj.Description);
            Assert.AreEqual(Constants.NUCLength,
                nucmerObj.LengthOfMUM.ToString((IFormatProvider)null));
            Assert.AreEqual(Constants.NUCName, nucmerObj.Name);
            Assert.AreEqual(Constants.NUCRefSeqNumber,
                nucmerObj.ReferenceSequenceNumber.ToString((IFormatProvider)null));
            Assert.AreEqual(Constants.NUCFixedSeperation,
                nucmerObj.FixedSeparation.ToString((IFormatProvider)null));
            Assert.AreEqual(Constants.NUCMaximumSeparation,
                nucmerObj.MaximumSeparation.ToString((IFormatProvider)null));
            Assert.AreEqual(Constants.NUCMinimumScore,
                nucmerObj.MinimumScore.ToString((IFormatProvider)null));
            Assert.AreEqual(Constants.NUCSeparationFactor,
                nucmerObj.SeparationFactor.ToString((IFormatProvider)null));
            Console.WriteLine("Successfully validated all the properties of NUCmer class.");
            ApplicationLog.WriteLine("Successfully validated all the properties of NUCmer class.");
        }

        #endregion NUCmer Align Test Cases

        #region NUCmer Simple Align Test Cases

        /// <summary>
        /// Validate AlignSimple() method with one line Dna sequence 
        /// and validate the aligned sequences
        /// Input : One line Dna sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void NUCmerAlignSimpleeOneLineDnaSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SingleDnaNucmerSequenceNodeName, false);
        }

        /// <summary>
        /// Validate AlignSimple() method with one line Rna sequence 
        /// and validate the aligned sequences
        /// Input : One line Rna sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void NUCmerAlignSimpleeOneLineRnaSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SingleRnaNucmerSequenceNodeName, false);
        }

        /// <summary>
        /// Validate SimpleAlign() method with one line Dna list of sequence 
        /// and validate the aligned sequences
        /// Input : One line Dna list of sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void NUCmerAlignSimpleOneLineDnaListOfSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SingleDnaNucmerSequenceNodeName, true);
        }

        /// <summary>
        /// Validate SimpleAlign() method with one line Rna list of sequence 
        /// and validate the aligned sequences
        /// Input : One line Rna list of sequence
        /// Validation : Validate the aligned sequences
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void NUCmerAlignSimpleOneLineRnaListOfSequence()
        {
            ValidateNUCmerAlignSimpleGeneralTestCases(Constants.SingleRnaNucmerSequenceNodeName, true);
        }

        #endregion NUCmer Simple Align Test Cases

        #region Supported Methods

        /// <summary>
        /// Validates most of the build suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is file path?</param>
        void ValidateBuildSuffixTreeGeneralTestCases(string nodeName, bool isFilePath)
        {
            ISequence referenceSeqs = null;
            string[] referenceSequences = null;

            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "NUCmer BVT : Successfully validated the File Path '{0}'.", filePath));

                using (FastaParser parser = new FastaParser())
                {
                    IList<ISequence> referenceSeqList = parser.Parse(filePath);
                    referenceSeqs = new SegmentedSequence(referenceSeqList);
                }
            }
            else
            {
                // Gets the reference & search sequences from the configurtion file
                referenceSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                    Constants.ReferenceSequencesNode);

                IAlphabet seqAlphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                       Constants.AlphabetNameNode));

                List<ISequence> refSeqList = new List<ISequence>();

                for (int i = 0; i < referenceSequences.Length; i++)
                {
                    ISequence referSeq = new Sequence(seqAlphabet, referenceSequences[i]);
                    refSeqList.Add(referSeq);
                }

                referenceSeqs = new SegmentedSequence(refSeqList);
            }

            // Builds the suffix for the reference sequence passed.
            ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
            SequenceSuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSeqs) as SequenceSuffixTree;

            // Validates the edges for a given sequence.
            ApplicationLog.WriteLine("NUCmer BVT : Validating the Edges");
            Assert.IsTrue(ValidateEdges(suffixTree, nodeName, isFilePath));
            Console.WriteLine(
                "NUCmer BVT : Successfully validated the all the Edges for the sequence specified.");
            ApplicationLog.WriteLine(
                "NUCmer BVT : Successfully validated the all the Edges for the sequence specified.");
        }

        /// <summary>
        /// Validates most of the find matches suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is File Path?</param>
        /// <param name="additionalParam">LIS action type enum</param>
        void ValidateFindMatchSuffixGeneralTestCases(string nodeName, bool isFilePath,
            AdditionalParameters additionalParam)
        {
            ISequence referenceSeqs = null;
            ISequence searchSeqs = null;
            string[] referenceSequences = null;
            string[] searchSequences = null;

            if (isFilePath)
            {
                // Gets the reference sequence from the FastA file
                string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "NUCmer BVT : Successfully validated the File Path '{0}'.", filePath));

                using (FastaParser parser = new FastaParser())
                {
                    IList<ISequence> referenceSeqList = parser.Parse(filePath);
                    referenceSeqs = new SegmentedSequence(referenceSeqList);


                    // Gets the query sequence from the FastA file
                    string queryFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.SearchSequenceFilePathNode);

                    Assert.IsNotNull(queryFilePath);
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "NUCmer BVT : Successfully validated the File Path '{0}'.", queryFilePath));

                    IList<ISequence> querySeqList = parser.Parse(queryFilePath);
                    searchSeqs = new SegmentedSequence(querySeqList);
                }
            }
            else
            {
                // Gets the reference & search sequences from the configurtion file
                referenceSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                    Constants.ReferenceSequencesNode);
                searchSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                  Constants.SearchSequencesNode);

                IAlphabet seqAlphabet = Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                       Constants.AlphabetNameNode));

                List<ISequence> refSeqList = new List<ISequence>();
                List<ISequence> searchSeqList = new List<ISequence>();
                for (int i = 0; i < referenceSequences.Length; i++)
                {
                    ISequence referSeq = new Sequence(seqAlphabet, referenceSequences[i]);
                    refSeqList.Add(referSeq);
                }

                referenceSeqs = new SegmentedSequence(refSeqList);
                for (int i = 0; i < searchSequences.Length; i++)
                {
                    ISequence searchSeq = new Sequence(seqAlphabet, searchSequences[i]);
                    searchSeqList.Add(searchSeq);
                }

                searchSeqs = new SegmentedSequence(searchSeqList);
            }

            string mumLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMLengthNode);

            // Builds the suffix for the reference sequence passed.
            ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
            SequenceSuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSeqs) as SequenceSuffixTree;

            IList<MaxUniqueMatch> matches = suffixTreeBuilder.FindMatches(suffixTree, searchSeqs,
                long.Parse(mumLength, null));

            switch (additionalParam)
            {
                case AdditionalParameters.FindUniqueMatches:
                    // Validates the Unique Matches.
                    ApplicationLog.WriteLine("NUCmer BVT : Validating the Unique Matches");
                    Assert.IsTrue(ValidateUniqueMatches(matches, nodeName, additionalParam, isFilePath));
                    Console.WriteLine(
                        "NUCmer BVT : Successfully validated the all the unique matches for the sequences.");
                    break;
                case AdditionalParameters.PerformClusterBuilder:
                    // Validates the Unique Matches.
                    ApplicationLog.WriteLine(
                        "NUCmer BVT : Validating the Unique Matches using Cluster Builder");
                    Assert.IsTrue(ValidateUniqueMatches(matches, nodeName, additionalParam, isFilePath));
                    Console.WriteLine(
                        "NUCmer BVT : Successfully validated the all the cluster builder matches for the sequences.");
                    break;
                default:
                    break;
            }


            ApplicationLog.WriteLine(
                "NUCmer BVT : Successfully validated the all the unique matches for the sequences.");
        }

        /// <summary>
        /// Validates the NUCmer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isFilePath">Is Sequence saved in File</param>
        void ValidateNUCmerAlignGeneralTestCases(string nodeName, bool isFilePath)
        {
            string[] referenceSequences = null;
            string[] searchSequences = null;
            IList<ISequence> refSeqList = new List<ISequence>();
            IList<ISequence> searchSeqList = new List<ISequence>();

            if (isFilePath)
            {
                // Gets the reference sequence from the FastA file
                string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "NUCmer BVT : Successfully validated the File Path '{0}'.", filePath));

                using (FastaParser parser = new FastaParser())
                {
                    refSeqList = parser.Parse(filePath);

                    // Gets the query sequence from the FastA file
                    string queryFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.SearchSequenceFilePathNode);

                    Assert.IsNotNull(queryFilePath);
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "NUCmer BVT : Successfully validated the File Path '{0}'.", queryFilePath));

                    searchSeqList = parser.Parse(queryFilePath);
                }
            }
            else
            {
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
            }

            string mumLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode);

            NUCmer nucmerObj = new NUCmer3();
            nucmerObj.MaximumSeparation = 0;
            nucmerObj.MinimumScore = 2;
            nucmerObj.SeparationFactor = 0.12f;
            nucmerObj.BreakLength = 2;
            nucmerObj.LengthOfMUM = long.Parse(mumLength, null);

            IList<IPairwiseSequenceAlignment> align = nucmerObj.Align(refSeqList, searchSeqList);

            string expectedSequences = string.Empty;

            if (isFilePath)
                expectedSequences = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                    Constants.ExpectedSequencesNode);
            else
                expectedSequences = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.ExpectedSequencesNode);

            string[] expSeqArray = expectedSequences.Split(',');

            int j = 0;

            // Gets all the aligned sequences in comma seperated format
            foreach (IPairwiseSequenceAlignment seqAlignment in align)
            {
                foreach (PairwiseAlignedSequence alignedSeq in seqAlignment)
                {
                    Assert.AreEqual(expSeqArray[j], alignedSeq.FirstSequence.ToString());
                    ++j;
                    Assert.AreEqual(expSeqArray[j], alignedSeq.SecondSequence.ToString());
                    j++;
                }
            }

            Console.WriteLine("NUCmer BVT : Successfully validated all the aligned sequences.");
            ApplicationLog.WriteLine("NUCmer BVT : Successfully validated all the aligned sequences.");
        }

        /// <summary>
        /// Validates the edges for the suffix tree and the node name specified.
        /// </summary>
        /// <param name="suffixTree">Suffix Tree.</param>
        /// <param name="nodeName">Node name which needs to be read for validation</param>
        /// <param name="isFilePath">Edges to be read from Text file?</param>
        /// <returns>True, if successfully validated.</returns>
        bool ValidateEdges(SequenceSuffixTree suffixTree, string nodeName, bool isFilePath)
        {
            Dictionary<int, Edge> ed = suffixTree.Edges;

            string[] actualStrtIndexes = new string[ed.Count];
            string[] actualEndIndexes = new string[ed.Count];
            // Gets all the edges to be validated as in xml.
            string[] startIndexes = null;
            string[] endIndexes = null;

            int j = 0;
            foreach (int col in ed.Keys)
            {
                Edge a = ed[col];
                actualStrtIndexes[j] = a.StartIndex.ToString((IFormatProvider)null);
                actualEndIndexes[j] = a.EndIndex.ToString((IFormatProvider)null);
                j++;
            }

            // Gets the sorted edge list for the actual Edge list
            List<Edge> actualEdgeList = GetSortedEdges(actualStrtIndexes, actualEndIndexes);

            // Gets all the edges to be validated as in xml.
            if (isFilePath)
            {
                // Gets all the edges to be validated from the text file.
                startIndexes = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                    Constants.EdgeStartIndexesNode).Split(',');
                endIndexes = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                    Constants.EdgeEndIndexesNode).Split(',');
            }
            else
            {
                // Gets all the edges to be validated as in xml.
                startIndexes = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.EdgeStartIndexesNode).Split(',');
                endIndexes = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.EdgeEndIndexesNode).Split(',');
            }

            // Gets the sorted edge list for the expected Edge list
            List<Edge> expectedEdgeList = GetSortedEdges(startIndexes, endIndexes);

            Console.WriteLine(string.Format((IFormatProvider)null,
                "NUCmer BVT : Total Edges Found is : '{0}'", ed.Keys.Count.ToString((IFormatProvider)null)));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "NUCmer BVT : Total Edges Found is : '{0}'", ed.Keys.Count.ToString((IFormatProvider)null)));

            // Loops through all the edges and validates the same.
            for (int i = 0; i < expectedEdgeList.Count; i++)
            {
                if (!(actualEdgeList[i].StartIndex == expectedEdgeList[i].StartIndex)
                    && (actualEdgeList[i].EndIndex == expectedEdgeList[i].EndIndex))
                {
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "NUCmer BVT : Edges not matching at index '{0}'", i.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "NUCmer BVT : Edges not matching at index '{0}'", i.ToString((IFormatProvider)null)));
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
        private static List<Edge> GetSortedEdges(string[] startIndexes, string[] endIndexes)
        {
            List<Edge> edgList = new List<Edge>();

            // Loops through all the indexes and creates EdgeList.
            for (int i = 0; i < startIndexes.Length; i++)
            {
                Edge edg = new Edge();
                edg.StartIndex = int.Parse(startIndexes[i], (IFormatProvider)null);
                edg.EndIndex = int.Parse(endIndexes[i], (IFormatProvider)null);

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
        /// <param name="additionalParam">Unique Match/Sub level LIS/LIS</param>
        /// <param name="isFilePath">Nodes to be read from Text file?</param>
        /// <returns>True, if successfully validated</returns>
        bool ValidateUniqueMatches(IList<MaxUniqueMatch> matches,
            string nodeName, AdditionalParameters additionalParam, bool isFilePath)
        {
            switch (additionalParam)
            {
                case AdditionalParameters.PerformClusterBuilder:
                    // Validates the Cluster builder MUMs
                    string firstSeqOrderExpected =
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ClustFirstSequenceMumOrderNode);
                    string firstSeqStartExpected =
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ClustFirstSequenceStartNode);
                    string lengthExpected =
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ClustLengthNode);
                    string secondSeqOrderExpected =
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ClustSecondSequenceMumOrderNode);
                    string secondSeqStartExpected =
                        _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ClustSecondSequenceStartNode);

                    StringBuilder firstSeqOrderActual = new StringBuilder();
                    StringBuilder firstSeqStartActual = new StringBuilder();
                    StringBuilder lengthActual = new StringBuilder();
                    StringBuilder secondSeqOrderActual = new StringBuilder();
                    StringBuilder secondSeqStartActual = new StringBuilder();

                    ClusterBuilder cb = new ClusterBuilder();
                    cb.MinimumScore = 0;
                    IList<Cluster> clusts = cb.BuildClusters(matches);

                    foreach (Cluster clust in clusts)
                    {
                        foreach (MaxUniqueMatchExtension maxMatchExtension in clust.Matches)
                        {
                            firstSeqOrderActual.Append(maxMatchExtension.FirstSequenceMumOrder);
                            secondSeqOrderActual.Append(maxMatchExtension.SecondSequenceMumOrder);
                            secondSeqStartActual.Append(maxMatchExtension.SecondSequenceStart);
                            firstSeqStartActual.Append(maxMatchExtension.FirstSequenceStart);
                            lengthActual.Append(maxMatchExtension.Length);
                        }
                    }

                    if ((0 != string.Compare(firstSeqOrderExpected.Replace(",", ""),
                        firstSeqOrderActual.ToString(), true, CultureInfo.CurrentCulture))
                        || (0 != string.Compare(firstSeqStartExpected.Replace(",", ""),
                        firstSeqStartActual.ToString(), true, CultureInfo.CurrentCulture))
                        || (0 != string.Compare(lengthExpected.Replace(",", ""),
                        lengthActual.ToString(), true, CultureInfo.CurrentCulture))
                        || (0 != string.Compare(secondSeqOrderExpected.Replace(",", ""),
                        secondSeqOrderActual.ToString(), true, CultureInfo.CurrentCulture))
                        || (0 != string.Compare(secondSeqStartExpected.Replace(",", ""),
                        secondSeqStartActual.ToString(), true, CultureInfo.CurrentCulture)))
                    {
                        Console.WriteLine("NUCmer BVT : Unique match not matching");
                        ApplicationLog.WriteLine("NUCmer BVT : Unique match not matching");
                        return false;
                    }
                    break;
                case AdditionalParameters.FindUniqueMatches:
                    // Gets all the unique matches properties to be validated as in xml.
                    string[] firstSeqOrder = null;
                    string[] firstSeqStart = null;
                    string[] length = null;
                    string[] secondSeqOrder = null;
                    string[] secondSeqStart = null;

                    if (isFilePath)
                    {
                        firstSeqOrder = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                            Constants.FirstSequenceMumOrderNode).Split(',');
                        firstSeqStart = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                            Constants.FirstSequenceStartNode).Split(',');
                        length = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                            Constants.LengthNode).Split(',');
                        secondSeqOrder = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                            Constants.SecondSequenceMumOrderNode).Split(',');
                        secondSeqStart = _utilityObj._xmlUtil.GetFileTextValue(nodeName,
                            Constants.SecondSequenceStartNode).Split(',');
                    }
                    else
                    {
                        firstSeqOrder = _utilityObj._xmlUtil.GetTextValue(nodeName,
                            Constants.FirstSequenceMumOrderNode).Split(',');
                        firstSeqStart = _utilityObj._xmlUtil.GetTextValue(nodeName,
                            Constants.FirstSequenceStartNode).Split(',');
                        length = _utilityObj._xmlUtil.GetTextValue(nodeName,
                            Constants.LengthNode).Split(',');
                        secondSeqOrder = _utilityObj._xmlUtil.GetTextValue(nodeName,
                            Constants.SecondSequenceMumOrderNode).Split(',');
                        secondSeqStart = _utilityObj._xmlUtil.GetTextValue(nodeName,
                            Constants.SecondSequenceStartNode).Split(',');
                    }

                    int i = 0;
                    // Loops through all the matches and validates the same.
                    foreach (MaxUniqueMatch match in matches)
                    {
                        if ((0 != string.Compare(firstSeqOrder[i],
                            match.FirstSequenceMumOrder.ToString((IFormatProvider)null), true,
                            CultureInfo.CurrentCulture))
                            || (0 != string.Compare(firstSeqStart[i],
                            match.FirstSequenceStart.ToString((IFormatProvider)null), true,
                            CultureInfo.CurrentCulture))
                            || (0 != string.Compare(length[i],
                            match.Length.ToString((IFormatProvider)null), true,
                            CultureInfo.CurrentCulture))
                            || (0 != string.Compare(secondSeqOrder[i],
                            match.SecondSequenceMumOrder.ToString((IFormatProvider)null), true,
                            CultureInfo.CurrentCulture))
                            || (0 != string.Compare(secondSeqStart[i],
                            match.SecondSequenceStart.ToString((IFormatProvider)null), true,
                            CultureInfo.CurrentCulture)))
                        {
                            Console.WriteLine(string.Format((IFormatProvider)null,
                                "NUCmer BVT : Unique match not matching at index '{0}'", i.ToString((IFormatProvider)null)));
                            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                                "NUCmer BVT : Unique match not matching at index '{0}'", i.ToString((IFormatProvider)null)));
                            return false;
                        }
                        i++;
                    }
                    break;
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// Validates the NUCmer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isAlignList">Is align method to take list?</param>
        void ValidateNUCmerAlignSimpleGeneralTestCases(string nodeName,
            bool isAlignList)
        {
            string[] referenceSequences = null;
            string[] searchSequences = null;
            List<ISequence> refSeqList = new List<ISequence>();
            List<ISequence> searchSeqList = new List<ISequence>();

            // Gets the reference & search sequences from the configurtion file
            referenceSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.ReferenceSequencesNode);
            searchSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
              Constants.SearchSequencesNode);

            IAlphabet seqAlphabet = Utility.GetAlphabet(
                _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));

            for (int i = 0; i < referenceSequences.Length; i++)
            {
                ISequence referSeq = new Sequence(seqAlphabet,
                    referenceSequences[i]);
                refSeqList.Add(referSeq);
            }

            for (int i = 0; i < searchSequences.Length; i++)
            {
                ISequence searchSeq = new Sequence(seqAlphabet,
                    searchSequences[i]);
                searchSeqList.Add(searchSeq);
            }

            // Gets the mum length from the xml
            string mumLength = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.MUMAlignLengthNode);

            NUCmer nucmerObj = new NUCmer3();

            // Update other values for NUCmer object
            nucmerObj.MaximumSeparation = int.Parse
                (_utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode), (IFormatProvider)null);
            nucmerObj.MinimumScore = int.Parse(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode), (IFormatProvider)null);
            nucmerObj.SeparationFactor = int.Parse(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode), (IFormatProvider)null);
            nucmerObj.BreakLength = int.Parse(
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode), (IFormatProvider)null);
            nucmerObj.LengthOfMUM = long.Parse(mumLength, null);

            IList<IPairwiseSequenceAlignment> alignSimple = null;

            if (isAlignList)
            {
                List<ISequence> listOfSeq = new List<ISequence>();
                listOfSeq.Add(refSeqList[0]);
                listOfSeq.Add(searchSeqList[0]);
                alignSimple = nucmerObj.AlignSimple(listOfSeq);
            }
            else
            {
                alignSimple = nucmerObj.AlignSimple(refSeqList, searchSeqList);
            }

            string expectedSequences = string.Empty;
            expectedSequences = _utilityObj._xmlUtil.GetTextValue(nodeName,
                Constants.ExpectedSequencesNode);

            string[] expSeqArray = expectedSequences.Split(',');

            int j = 0;

            // Gets all the aligned sequences in comma seperated format
            foreach (IPairwiseSequenceAlignment seqAlignment in alignSimple)
            {
                foreach (PairwiseAlignedSequence alignedSeq in seqAlignment)
                {
                    Assert.AreEqual(expSeqArray[j], alignedSeq.FirstSequence.ToString());
                    ++j;
                    Assert.AreEqual(expSeqArray[j], alignedSeq.SecondSequence.ToString());
                    j++;
                }
            }

            Console.WriteLine(
                "NUCmer BVT : Successfully validated all the aligned sequences.");
            ApplicationLog.WriteLine(
                "NUCmer BVT : Successfully validated all the aligned sequences.");
        }

        #endregion Supported Methods
    }
}
