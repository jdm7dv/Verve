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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;

namespace MBF.TestAutomation.Algorithms.Alignment
{
    /// <summary>
    /// MUMmer Bvt Test case implementation.
    /// </summary>
    [TestClass]
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

        /// <summary>
        /// SuffixTree Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum SuffxTreeParameters
        {
            KurtzSuffixTree,
            SimpleSuffixTree,
            MultiWaySuffixTree,
            PersistentMultiWaySuffixTree,
        };

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\MUMmerTestsConfig.xml");

        #endregion Global Variables

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
        }

        #endregion Constructor

        #region Suffix Tree Test Cases

        /// <summary>
        /// Validate BuildSuffixTree() method with one line sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : One line sequence
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void SuffixTreeBuildSuffixTreeOneLineSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.OneLineSequenceNodeName, false,
                SuffxTreeParameters.KurtzSuffixTree);
        }

        /// <summary>
        /// Validate BuildSuffixTree() method with small size (less than 35kb) sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : small size sequence file
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void SuffixTreeBuildSuffixTreeSmallSizeSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.SmallSizeSequenceNodeName, true,
                SuffxTreeParameters.KurtzSuffixTree);
        }

        /// <summary>
        /// Validate FindMatches() method with one line sequence 
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
                false, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate FindMatches() method with small size (less than 35kb) sequence 
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
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
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
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
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
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
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
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void MUMmerAlignOneLineSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.OneLineSequenceNodeName,
                false, false);
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
        public void MUMmerAlignSmallSizeSequence()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.SmallSizeSequenceNodeName,
                true, false);
        }

        /// <summary>
        /// Validate Align(QuerySeqList) method with one line sequence 
        /// and validate the aligned sequences
        /// Input : One line multiple sequences
        /// Validation : Validate the aligned sequences.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void MUMmerAlignQuerySeqList()
        {
            ValidateMUMmerAlignGeneralTestCases(Constants.QuerySeqeunceListNode, false, true);
        }

        /// <summary>
        /// Validate All properties in MUMmer class
        /// Input : One line sequence and update all properties
        /// Validation : Validate the properties
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
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
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
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
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
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
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateMUMsAfterLISWithOneLineSequence()
        {
            ValidateMUMsGeneralTestCases(Constants.OneLineSequenceNodeName,
                false, true, true);
        }

        #endregion MUMs validation Test Cases

        #region Simple Suffix Tree Test Cases

        /// <summary>
        /// Validate SimpleSuffixTree Find Match() for one line sequence
        /// Input : One line sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void SimpleSuffixTreeFindMatchesOneLineSequence()
        {
            ValidateFindMatchSimpleSuffixGeneralTestCases(Constants.OneLineSequenceNodeName,
                false, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate SimpleSuffixTree Find Match() for small size sequences
        /// Input : Small size sequence for both reference and query parameter
        /// Validation : Validate the unique matches
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void SimpleSuffixTreeFindMatchesSmallSizeSequence()
        {
            ValidateFindMatchSimpleSuffixGeneralTestCases(Constants.SmallSizeSequenceNodeName,
                true, LISParameters.FindUniqueMatches);
        }

        /// <summary>
        /// Validate SuffixEdgeStorage operations(Read/Write).
        /// Input : Start and End Index
        /// Validation : Validate the Edge Read/Write operations of SuffixEdgeStorage.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSuffixTreeEdgeStorage()
        {
            // Get Start and End Index values from XML file.
            string startIndex = _utilityObj._xmlUtil.GetTextValue(Constants.FileStorageNode,
                Constants.EdgeStartIndexesNode);
            string endIndex = _utilityObj._xmlUtil.GetTextValue(Constants.FileStorageNode,
                Constants.EdgeEndIndexesNode);
            string childCount = _utilityObj._xmlUtil.GetTextValue(Constants.FileStorageNode,
               Constants.ChildrenCountNode);

            // Create a Persistent suffixTreeEdge
            PersistentMultiWaySuffixEdge edge = new PersistentMultiWaySuffixEdge(
                Int32.Parse(startIndex, (IFormatProvider)null), Int32.Parse(endIndex, (IFormatProvider)null),
                Int32.Parse(childCount, (IFormatProvider)null));

            using (FileSuffixEdgeStorage fileStorage = new FileSuffixEdgeStorage())
            {
                // Write an edge to the storage
                fileStorage.Write(edge);                

                // Read edge written to storage in previous step and validate the same
                IPersistentEdge readEdge = fileStorage.Read(0);
                Assert.AreEqual(edge.StartIndex, readEdge.StartIndex);
                Assert.AreEqual(edge.EndIndex, readEdge.EndIndex);
                Assert.AreEqual(edge.Key, readEdge.Key);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                       "MUMmer BVT : Successfully validated the SuffixTree Storage operations"));
            }
        }

       

        #endregion  Simple Suffix Tree Test Cases

        #region MultiWaySuffixTree

        /// <summary>
        /// Validate MultiWaySuffixTree.BuildSuffixTree() method with one line sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : One line sequence
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateBuildMultiWaySuffixTreeOneLineSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.OneLineSequenceNodeName, false,
                SuffxTreeParameters.MultiWaySuffixTree);
        }

        /// <summary>
        /// Validate MultiWaySuffixTree.BuildSuffixTree() method with small size (less than 35kb) sequence 
        /// and validate the nodes, edges and the sequence
        /// Input : small size sequence file
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateBuildMultiWaySuffixTreeSmallSizeSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.SmallSizeSequenceNodeName, true,
                SuffxTreeParameters.MultiWaySuffixTree);
        }

        /// <summary>
        /// Validate MultiWaySuffixTreeEdge operations
        /// Input : Start and End Index
        /// Validation : Validate the MultiWaySuffixTreeEdge operations.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateMultiWaySuffixTreeEdge()
        {
            // Get Start and End Index values from XML file.
            string referenceSequence = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                Constants.SequenceNode);

            // Create a MultiWaySuffixTreeEdge
            ISequence referenceSeq = new Sequence(Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.OneLineSequenceNodeName, Constants.AlphabetNameNode)),
                referenceSequence);

            using (SimpleSuffixTreeBuilder simpleSuffixTree = new SimpleSuffixTreeBuilder())
            {
                // Build MultiWaySuffixTree
                IMultiWaySuffixTree multiWaySuffixTree = simpleSuffixTree.BuildSuffixTree(referenceSeq)
                    as MultiWaySuffixTree;

                // Get the root Node
                MultiWaySuffixEdge rootEdge = multiWaySuffixTree.Root as MultiWaySuffixEdge;
                IEdge[] childrenOfRoot = rootEdge.GetChildren();

                // Create a MultiWaySuffixTreeEdge            
                MultiWaySuffixEdge multiWayEdges = new MultiWaySuffixEdge(childrenOfRoot[0].StartIndex,
                    childrenOfRoot[0].EndIndex);
                Assert.IsTrue(multiWayEdges.IsLeaf);

                // Add a Child Node to the MultiWayEdge created in previous step
                multiWayEdges.AddChild(childrenOfRoot[0]);

                Assert.IsFalse(multiWayEdges.IsLeaf);

                // Replace children with new edges
                multiWayEdges.ReplaceChildren(childrenOfRoot);

                IEdge[] childerenOfMultiWayEdge = multiWayEdges.GetChildren();

                // Validate all the children recursively
                ValidateRecursiveEdges(childerenOfMultiWayEdge, Constants.OneLineSequenceNodeName);

                // Clear all children
                multiWayEdges.ClearChildren();

                Assert.IsTrue(multiWayEdges.IsLeaf);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                       "MUMmer BVT : Successfully validated the MultiWaySuffixEdge operations"));
            }
        }

        /// <summary>
        /// Validate MultiWaySuffixTreeEdge.Merge()
        /// Input : MultiWaySuffixEdge branch.
        /// Validation : Validate the merge of MultiWaySuffixEdge.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateMergeMultiWaySuffixTree()
        {
            // Get Start and End Index values from XML file.
            string firstSeq = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                   Constants.SequenceNode);
            string secondSeq = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                   Constants.SearchSequenceNode);

            ISequence firstSequence = new Sequence(Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.OneLineSequenceNodeName, Constants.AlphabetNameNode)),
                firstSeq);
            ISequence secondSequence = new Sequence(Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.OneLineSequenceNodeName, Constants.AlphabetNameNode)),
                secondSeq);

            using (SimpleSuffixTreeBuilder simpleSuffixTree = new SimpleSuffixTreeBuilder())
            {
                // Build MultiWaySuffixTree
                IMultiWaySuffixTree multiWaySuffixTree = simpleSuffixTree.BuildSuffixTree(firstSequence)
                    as MultiWaySuffixTree;

                IMultiWaySuffixTree secondMultiWaySuffixTree = simpleSuffixTree.BuildSuffixTree(secondSequence)
                    as MultiWaySuffixTree;

                // Merge Suffix Tree
                multiWaySuffixTree.Merge(secondMultiWaySuffixTree);

                MultiWaySuffixEdge rootNode = multiWaySuffixTree.Root as MultiWaySuffixEdge;
                IEdge[] childOfRoot = rootNode.GetChildren();

                // Validate edges after merging.
                ValidateRecursiveEdges(childOfRoot, Constants.OneLineSequenceNodeName);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "MUMmer BVT : Successfully validated the Merge MultiWaySuffixEdge"));
            }
        }

        /// <summary>
        /// Validate MultiWaySuffixTreeEdge.Insert(), Find() and Remove().
        /// Input : MultiWaySiffxEdge.
        /// Validation : Validate MultiWaySuffixEdge operations.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateMultiWaySuffixEdgeOperations()
        {
            // Get Start and End Index values from XML file.
            string refSeq = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                   Constants.SequenceNode);
            string startIndex = _utilityObj._xmlUtil.GetTextValue(Constants.FileStorageNode,
             Constants.EdgeStartIndexesNode);
            string endIndex = _utilityObj._xmlUtil.GetTextValue(Constants.FileStorageNode,
                Constants.EdgeEndIndexesNode);
            string byteCharacterToSearch = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                   Constants.ByteCharacterNode);
            string searchStartIndex = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                Constants.ExpectedStartIndexNode);
            string searchEndIndex = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                Constants.ExpectedEndIndexNode);

            ISequence referenceSequence = new Sequence(Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.OneLineSequenceNodeName, Constants.AlphabetNameNode)),
                refSeq);

            using (SimpleSuffixTreeBuilder simpleSuffixTree = new SimpleSuffixTreeBuilder())
            {
                // Build MultiWaySuffixTree
                IMultiWaySuffixTree multiWaySuffixTree = simpleSuffixTree.BuildSuffixTree(referenceSequence)
                    as MultiWaySuffixTree;

                MultiWaySuffixEdge rootNode = multiWaySuffixTree.Root as MultiWaySuffixEdge;
                IEdge[] childOfRoot = rootNode.GetChildren();

                // Insert an edge 
                multiWaySuffixTree.Insert(childOfRoot[0], Int32.Parse(startIndex, (IFormatProvider)null),
                    Int32.Parse(endIndex, (IFormatProvider)null));

                // Validate whether inserted edge is present in the tree
                IEdge searchEdge = multiWaySuffixTree.Find(childOfRoot[0], byte.Parse(byteCharacterToSearch,
                    (IFormatProvider)null));

                Assert.AreEqual(searchEdge.StartIndex.ToString((IFormatProvider)null), searchStartIndex);
                Assert.AreEqual(searchEdge.EndIndex.ToString((IFormatProvider)null), searchEndIndex);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "MUMmer BVT : Successfully validated the Insert and Find edge MultiWaySuffixEdge"));

                // Get the total number of edges in the tree
                int totalEdges = multiWaySuffixTree.Count;

                rootNode = multiWaySuffixTree.Root as MultiWaySuffixEdge;
                childOfRoot = rootNode.GetChildren();

                // Get Children of first child node
                MultiWaySuffixEdge firstRootNode = childOfRoot[0] as MultiWaySuffixEdge;
                IEdge[] childOfChildrenNode = firstRootNode.GetChildren();

                // Remove edge
                multiWaySuffixTree.Remove(childOfRoot[0], childOfChildrenNode[0]);

                // Validate tree count after child removal
                Assert.IsTrue(totalEdges > multiWaySuffixTree.Count);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "MUMmer BVT : Successfully validated the Remove Edge MultiWaySuffixEdge"));
            }
        }

        /// <summary>
        /// Validate MultiWaySuffixTreeEdge.Update()
        /// Input : SuffixTree new node name
        /// Validation : Validate MultiWaySuffixEdge.update() operation.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateUpdateMultiWaySuffixEdge()
        {
            // Get Start and End Index values from XML file.
            string refSeq = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                   Constants.SequenceNode);

            ISequence referenceSequence = new Sequence(Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.OneLineSequenceNodeName, Constants.AlphabetNameNode)),
                refSeq);

            using (SimpleSuffixTreeBuilder simpleSuffixTree = new SimpleSuffixTreeBuilder())
            {
                // Build MultiWaySuffixTree
                IMultiWaySuffixTree multiWaySuffixTree = simpleSuffixTree.BuildSuffixTree(referenceSequence)
                    as MultiWaySuffixTree;

                MultiWaySuffixEdge rootNode = multiWaySuffixTree.Root as MultiWaySuffixEdge;
                IEdge[] childOfRoot = rootNode.GetChildren();

                // Get first child of ChildRoot
                MultiWaySuffixEdge firstRootNode = childOfRoot[0] as MultiWaySuffixEdge;
                IEdge[] childOfChildrenNode = firstRootNode.GetChildren();

                // Update first child with child of root
                bool result = multiWaySuffixTree.Update(childOfRoot[0],
                    childOfChildrenNode[0], childOfChildrenNode[1]);
                Assert.IsTrue(result);

                // Get updated child and validate.
                MultiWaySuffixEdge updatedfirstRootNode = childOfRoot[0] as MultiWaySuffixEdge;
                IEdge[] updatedChildOfChildrenNode = updatedfirstRootNode.GetChildren();

                Assert.AreEqual(updatedChildOfChildrenNode[0].StartIndex, childOfChildrenNode[1].StartIndex);
                Assert.AreEqual(updatedChildOfChildrenNode[0].EndIndex, childOfChildrenNode[1].EndIndex);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "MUMmer BVT : Successfully validated the Update Edge MultiWaySuffixEdge"));
            }
        }

        /// <summary>
        /// Validate MultiWaySuffixTreeEdge.Split()
        /// Input : MultiWaySuffixTree.
        /// Validation : Validate MultiWaySuffixEdge.Split() operation.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSplitMultiWaySuffixEdge()
        {
            // Get Start and End Index values from XML file.
            string refSeq = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                   Constants.SequenceNode);

            ISequence referenceSequence = new Sequence(Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.OneLineSequenceNodeName, Constants.AlphabetNameNode)),
                refSeq);

            using (SimpleSuffixTreeBuilder simpleSuffixTree = new SimpleSuffixTreeBuilder())
            {
                // Build MultiWaySuffixTree
                IMultiWaySuffixTree multiWaySuffixTree = simpleSuffixTree.BuildSuffixTree(referenceSequence)
                    as MultiWaySuffixTree;

                MultiWaySuffixEdge rootNode = multiWaySuffixTree.Root as MultiWaySuffixEdge;
                IEdge[] childOfRoot = rootNode.GetChildren();

                IEdge splitEdges = multiWaySuffixTree.Split(childOfRoot[0], 0);

                // Get child of Split()
                MultiWaySuffixEdge firstRootNode = splitEdges as MultiWaySuffixEdge;
                IEdge[] childOfSplitNode = firstRootNode.GetChildren();

                // Validate results 
                ValidateRecursiveEdges(childOfSplitNode, Constants.OneLineSequenceNodeName);

                // Get update child and validate.
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "MUMmer BVT : Successfully validated the Update Edge MultiWaySuffixEdge"));
            }
        }

        #endregion MultiWaySuffixTree

        #region PersistentMultiWaySuffixTree

        /// <summary>
        /// Validate MultiWaySuffixTree.BuildSuffixTree() method with one line sequence with
        /// PersistentThreshold and validate the nodes, edges and the sequence
        /// Input : One line sequence
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateBuildPersistentMultiWaySuffixTreeOneLineSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.OneLineSequenceNodeName, false,
                SuffxTreeParameters.PersistentMultiWaySuffixTree);
        }

        /// <summary>
        /// Validate MultiWaySuffixTree.BuildSuffixTree() method with small size (less than 35kb) sequence 
        /// with PersistentThreshold and validate the nodes, edges and the sequence
        /// Input : small size sequence file
        /// Validation : Validate the nodes, edges and the sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateBuildPersistentMultiWaySuffixTreeSmallSizeSequence()
        {
            ValidateBuildSuffixTreeGeneralTestCases(Constants.SmallSizeSequenceNodeName, true,
                SuffxTreeParameters.PersistentMultiWaySuffixTree);
        }

        /// <summary>
        /// Validate PersistentSuffixTreeEdge operations
        /// Input : PersistentSuffixTreeEdge
        /// Validation : Validate the creation of PersistentSuffixTreeEdge.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidatePersistentSuffixTreeEdge()
        {
            // Get Start and End Index values from XML file.
            string childCount = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
               Constants.ChildrenCountNode);
            string referenceSequence = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                   Constants.SequenceNode);
            string childrenToReplace = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                   Constants.ChildrenToReplaceNode);
            long[] children = { long.Parse(childrenToReplace, (IFormatProvider)null) };

            // Create a MultiWaySuffixTreeEdge
            ISequence referenceSeq = new Sequence(Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.OneLineSequenceNodeName, Constants.AlphabetNameNode)),
                referenceSequence);

            using (SimpleSuffixTreeBuilder simpleSuffixTree = new SimpleSuffixTreeBuilder())
            {
                // Build MultiWaySuffixTree
                IMultiWaySuffixTree multiWaySuffixTree = simpleSuffixTree.BuildSuffixTree(referenceSeq)
                    as MultiWaySuffixTree;

                // Get the root Node
                MultiWaySuffixEdge rootEdge = multiWaySuffixTree.Root as MultiWaySuffixEdge;
                IEdge[] childrenOfRoot = rootEdge.GetChildren();

                // Create a MultiWaySuffixTreeEdge            
                PersistentMultiWaySuffixEdge persistentMultiWayEdges = new PersistentMultiWaySuffixEdge(childrenOfRoot[0].StartIndex,
                    childrenOfRoot[0].EndIndex, Int32.Parse(childCount, (IFormatProvider)null));

                Assert.IsTrue(persistentMultiWayEdges.IsLeaf);

                // Add a Child Node to the MultiWaySuffixEdge created in previous step
                persistentMultiWayEdges.AddChild(1);

                Assert.IsFalse(persistentMultiWayEdges.IsLeaf);

                // Replace children with new edges
                persistentMultiWayEdges.ReplaceChildren(children);

                // Get children
                long[] childerenOfMultiWayEdge = persistentMultiWayEdges.GetChildren();

                Assert.AreEqual(children, childerenOfMultiWayEdge);

                // Clear all children
                persistentMultiWayEdges.ClearChildren();

                Assert.IsTrue(persistentMultiWayEdges.IsLeaf);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                       "MUMmer BVT : Successfully validated the PersistentMultiWaySuffixEdge operations"));
            }
        }

        /// <summary>
        /// Validate PersistentMultiWaySuffixTreeEdge.Split()
        /// Input : SuffixTree new node name
        /// Validation : Validate PersistentMultiWaySuffixEdge.Split() operation.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSplitPersistentMultiWaySuffixEdge()
        {
            // Get Start and End Index values from XML file.
            string refSeq = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                   Constants.SequenceNode);
            string[] startIndex = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
             Constants.EdgeStartIndexesNode).Split(',');
            string[] endIndex = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                Constants.EdgeEndIndexesNode).Split(',');
            string childCount = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
              Constants.ChildrenCountNode);
            string persitentThreshold = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
               Constants.PersistentThresholdNode);
            string treeCount = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
              Constants.PersistentTreeCountNode);
            string byteCharacterToSearch = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                  Constants.ByteCharacterNode);

            ISequence referenceSequence = new Sequence(Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.OneLineSequenceNodeName, Constants.AlphabetNameNode)),
                refSeq);

            // Build PersistentMultipleSuffixTree
            using (PersistentMultiWaySuffixTree persistentTree = new PersistentMultiWaySuffixTree(referenceSequence,
                 Int32.Parse(childCount, (IFormatProvider)null), Int32.Parse(persitentThreshold, (IFormatProvider)null)))
            {
                Assert.AreEqual(persistentTree.PersisntThreshold.ToString((IFormatProvider)null),
                    persitentThreshold);
                Assert.AreEqual(persistentTree.Count.ToString((IFormatProvider)null),
                    treeCount);
                Assert.AreEqual(persistentTree.Sequence.ToString(), referenceSequence.ToString());

                // Insert edge at root level.
                int treeCountBeforeInsert = persistentTree.Count;
                persistentTree.Insert(persistentTree.Root, Int32.Parse(startIndex[0], (IFormatProvider)null),
                    Int32.Parse(endIndex[0], (IFormatProvider)null));

                int treeCountAftrerInsert = persistentTree.Count;
                Assert.IsTrue(treeCountAftrerInsert > treeCountBeforeInsert);

                // Search inserted edge
                IEdge searchEdge = persistentTree.Find(persistentTree.Root,
                    byte.Parse(byteCharacterToSearch, (IFormatProvider)null));

                Assert.AreEqual(searchEdge.StartIndex.ToString((IFormatProvider)null), startIndex[0]);
                Assert.AreEqual(searchEdge.EndIndex.ToString((IFormatProvider)null), endIndex[0]);


                IEdge splitEdges = persistentTree.Split(persistentTree.Root, 0);

                // Get child of Split()
                MultiWaySuffixEdge firstRootNode = splitEdges as MultiWaySuffixEdge;
                IEdge[] childOfSplitNode = firstRootNode.GetChildren();

                // Vaidate split edges
                ValidateRecursiveEdges(childOfSplitNode, Constants.OneLineSequenceNodeName);

                // Remove an edge
                int countBeforeRemove = persistentTree.Count;
                bool result = persistentTree.Remove(persistentTree.Root, childOfSplitNode[0]);
                Assert.IsTrue(result);

                // Validate count 
                Assert.IsTrue(countBeforeRemove > persistentTree.Count);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "MUMmer BVT : Successfully validated the Insert,Remove,Find and Split an PersistentMultiWaySuffixEdge"));
            }
        }

        /// <summary>
        /// Validate PersistentMultiWaySuffixTreeEdge.Merge()
        /// Input : Start and End Index
        /// Validation : Validate the merge of MultiWaySuffixTreeEdge.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateMergePersistentMultiWaySuffixTree()
        {
            // Get Start and End Index values from XML file.
            string firstSeq = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                   Constants.SequenceNode);
            string secondSeq = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
                   Constants.SearchSequenceNode);
            string childCount = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
             Constants.ChildrenCountNode);
            string persitentThreshold = _utilityObj._xmlUtil.GetTextValue(Constants.OneLineSequenceNodeName,
               Constants.PersistentThresholdNode);

            ISequence firstSequence = new Sequence(Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.OneLineSequenceNodeName, Constants.AlphabetNameNode)),
                firstSeq);
            ISequence secondSequence = new Sequence(Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(
                Constants.OneLineSequenceNodeName, Constants.AlphabetNameNode)),
                secondSeq);

            // Build PersistentMultipleSuffixTree
            using (SimpleSuffixTreeBuilder simpleSuffixTree = new SimpleSuffixTreeBuilder())
            {
                IMultiWaySuffixTree multiWaySuffixTree = simpleSuffixTree.BuildSuffixTree(firstSequence)
                    as MultiWaySuffixTree;

                PersistentMultiWaySuffixTree persistentTree = new PersistentMultiWaySuffixTree(secondSequence,
                    Int32.Parse(childCount, (IFormatProvider)null),
                    Int32.Parse(persitentThreshold, (IFormatProvider)null));

                // Merge PersistentMultiWaySuffix Tree
                persistentTree.Merge(multiWaySuffixTree);

                MultiWaySuffixEdge rootNode = persistentTree.Root as MultiWaySuffixEdge;
                IEdge[] childOfRoot = rootNode.GetChildren();

                // Validate edges after merging.
                ValidateRecursiveEdges(childOfRoot, Constants.OneLineSequenceNodeName);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "MUMmer BVT : Successfully validated the Merge PersistentMultiWaySuffixEdge"));

                // Get first child of ChildRoot
                MultiWaySuffixEdge firstRootNode = childOfRoot[0] as MultiWaySuffixEdge;
                IEdge[] childOfChildrenNode = firstRootNode.GetChildren();

                // Update first child with child of root
                bool result = persistentTree.Update(childOfRoot[0],
                    childOfChildrenNode[0], childOfChildrenNode[1]);
                Assert.IsTrue(result);

                // Get updated child and validate.
                MultiWaySuffixEdge updatedfirstRootNode = childOfRoot[0] as MultiWaySuffixEdge;
                IEdge[] updatedChildOfChildrenNode = updatedfirstRootNode.GetChildren();

                Assert.AreEqual(updatedChildOfChildrenNode[0].StartIndex, childOfChildrenNode[1].StartIndex);
                Assert.AreEqual(updatedChildOfChildrenNode[0].EndIndex, childOfChildrenNode[1].EndIndex);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                   "MUMmer BVT : Successfully validated the Update Edge PersistentMultiWaySuffixEdge"));
            }
        }


        /// <summary>
        /// Validate GetObjectData().
        /// Input : PersistentMultiwaySuffixTree getobject data.
        /// Validation : Validate GetObjectData().
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidatePersistentMultiWayTreeGetObjectData()
        {
            // Get Start and End Index values from XML file.
            string startIndex = _utilityObj._xmlUtil.GetTextValue(Constants.FileStorageNode,
                Constants.EdgeStartIndexesNode);
            string endIndex = _utilityObj._xmlUtil.GetTextValue(Constants.FileStorageNode,
                Constants.EdgeEndIndexesNode);
            string childCount = _utilityObj._xmlUtil.GetTextValue(Constants.FileStorageNode,
               Constants.ChildrenCountNode);

            SerializationInfo info =
               new SerializationInfo(typeof(VirtualSequence),
                   new FormatterConverter());
            StreamingContext context =
                new StreamingContext(StreamingContextStates.All);

            // Create a Persistent suffixTreeEdge
            PersistentMultiWaySuffixEdge edge = new PersistentMultiWaySuffixEdge(
                Int32.Parse(startIndex, (IFormatProvider)null), Int32.Parse(endIndex, (IFormatProvider)null),
                Int32.Parse(childCount, (IFormatProvider)null));

            // Validate GetObjectData()
            try
            {
                edge.GetObjectData(null, context);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                edge.GetObjectData(info, context);
                Assert.IsNotNull(edge);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                       "MUMmer BVT : Successfully validated the GetObjectData "));
            }

        }

        #endregion PersistentMultiWaySuffixTree

        #region Supported Methods

        /// <summary>
        /// Validates most of the build suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is file path?</param>
        void ValidateBuildSuffixTreeGeneralTestCases(string nodeName, bool isFilePath,
            SuffxTreeParameters treePams)
        {
            ISequence referenceSeq = null;
            string referenceSequence = string.Empty;
            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "MUMmer BVT : Successfully validated the File Path '{0}'.", filePath));

                using (FastaParser parser = new FastaParser())
                {
                    IList<ISequence> referenceSeqs = parser.Parse(filePath);
                    referenceSeq = referenceSeqs[0];
                    referenceSequence = referenceSeq.ToString();
                }
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(_utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode)), referenceSequence);
            }


            switch (treePams)
            {
                case SuffxTreeParameters.KurtzSuffixTree:

                    // Builds the suffixTree for the reference sequence passed.
                    ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
                    SequenceSuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSeq) as SequenceSuffixTree;

                    // Validates the edges for a given sequence.
                    ApplicationLog.WriteLine("MUMmer BVT : Validating the Edges");
                    Assert.IsTrue(ValidateEdges(suffixTree, nodeName));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "MUMmer BVT : Successfully validated the all the Edges for the sequence '{0}'.",
                        referenceSequence));

                    Assert.AreEqual(suffixTree.Sequence.ToString(), referenceSequence);
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                     "MUMmer BVT : Successfully validated the Suffix Tree properties for the sequence '{0}'.",
                     referenceSequence));
                    break;

                case SuffxTreeParameters.SimpleSuffixTree:

                    // Build SimpleSuffixTree
                    using (SimpleSuffixTreeBuilder simpleSuffixTreeBuilder = new SimpleSuffixTreeBuilder())
                    {
                        SequenceSuffixTree simpleSuffixtree = simpleSuffixTreeBuilder.BuildSuffixTree(referenceSeq) as SequenceSuffixTree;

                        // Validates the edges for a given sequence.
                        ApplicationLog.WriteLine("MUMmer BVT : Validating the Edges");
                        Assert.IsTrue(ValidateEdges(simpleSuffixtree, nodeName));
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "MUMmer BVT : Successfully validated the all the Edges for the sequence '{0}'.",
                            referenceSequence));

                        Assert.AreEqual(simpleSuffixtree.Sequence.ToString(), referenceSequence);
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "MUMmer BVT : Successfully validated the Suffix Tree properties for the sequence '{0}'.",
                            referenceSequence));
                    }
                    break;

                case SuffxTreeParameters.MultiWaySuffixTree:

                    // Build MultiWaySuffixTree
                    using (SimpleSuffixTreeBuilder simpleSuffixTree = new SimpleSuffixTreeBuilder())
                    {
                        IMultiWaySuffixTree multiWaySuffixTree = simpleSuffixTree.BuildSuffixTree(referenceSeq) as MultiWaySuffixTree;

                        // Get the root Node and its children
                        MultiWaySuffixEdge rootEdge = multiWaySuffixTree.Root as MultiWaySuffixEdge;
                        IEdge[] childrenOfRoot = rootEdge.GetChildren();

                        // Validate the edges recursively
                        ValidateRecursiveEdges(childrenOfRoot, nodeName);
                        Assert.AreEqual(multiWaySuffixTree.Sequence.ToString(), referenceSequence);
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "MUMmer BVT : Successfully validated the MultiWaySuffix Tree properties for the sequence '{0}'.",
                            referenceSequence));
                    }
                    break;

                case SuffxTreeParameters.PersistentMultiWaySuffixTree:

                    // Build PersistentMultiWaySuffixTree with PersistentThreshold
                    using (SimpleSuffixTreeBuilder builder = new SimpleSuffixTreeBuilder())
                    {
                        builder.PersistenceThreshold = Int32.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.PersistentThresholdNode), (IFormatProvider)null);

                        IMultiWaySuffixTree multiWayTree = builder.BuildSuffixTree(referenceSeq) as MultiWaySuffixTree;

                        // Get the root Node
                        MultiWaySuffixEdge rootNode = multiWayTree.Root as MultiWaySuffixEdge;
                        IEdge[] childOfRoot = rootNode.GetChildren();

                        // Validate the edges recursively
                        ValidateRecursiveEdges(childOfRoot, nodeName);
                        Assert.AreEqual(multiWayTree.Sequence.ToString(), referenceSequence);
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "MUMmer BVT : Successfully validated the MultiWaySuffix Tree properties for the sequence '{0}'.",
                            referenceSequence));
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Validates most of the find matches suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is File Path?</param>
        /// <param name="LISActionType">LIS action type enum</param>
        void ValidateFindMatchSuffixGeneralTestCases(string nodeName, bool isFilePath,
            LISParameters LISActionType)
        {
            ISequence referenceSeq = null;
            ISequence querySeq = null;
            string referenceSequence = string.Empty;
            string querySequence = string.Empty;
            IList<ISequence> referenceSeqs = null;

            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "MUMmer BVT : Successfully validated the File Path '{0}'.", filePath));

                using (FastaParser parser = new FastaParser())
                {
                    referenceSeqs = parser.Parse(filePath);
                    referenceSeq = referenceSeqs[0];
                    referenceSequence = referenceSeq.ToString();
                }

                // Gets the reference sequence from the configurtion file
                string queryFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceFilePathNode);

                Assert.IsNotNull(queryFilePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "MUMmer BVT : Successfully validated the Search File Path '{0}'.", queryFilePath));

                IList<ISequence> querySeqs = null;
                using (FastaParser queryParser = new FastaParser())
                {
                    querySeqs = queryParser.Parse(queryFilePath);
                    querySeq = querySeqs[0];
                    querySequence = querySeq.ToString();
                }
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                string seqAlp = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(seqAlp),
                    referenceSequence);

                querySequence = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceNode);

                seqAlp = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceAlphabetNode);

                querySeq = new Sequence(Utility.GetAlphabet(seqAlp),
                    querySequence);
            }

            string mumLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMLengthNode);

            // Builds the suffix for the reference sequence passed.
            ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
            SequenceSuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSeq) as SequenceSuffixTree;

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

            Console.WriteLine(string.Format((IFormatProvider)null,
                "MUMmer BVT : Successfully validated the all the unique matches for the sequence '{0}' and '{1}'.",
                referenceSequence, querySequence));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "MUMmer BVT : Successfully validated the all the unique matches for the sequence '{0}' and '{1}'.",
                referenceSequence, querySequence));
        }

        /// <summary>
        /// Validates the Mummer align method for several test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node name to be read from xml</param>
        /// <param name="isSeqList">Is MUMmer alignment with List of sequences</param>
        void ValidateMUMmerAlignGeneralTestCases(string nodeName, bool isFilePath,
            bool isSeqList)
        {
            ISequence referenceSeq = null;
            ISequence querySeq = null;
            IList<ISequence> querySeqs = null;
            string referenceSequence = string.Empty;
            string querySequence = string.Empty;
            IList<IPairwiseSequenceAlignment> align = null;
            IList<ISequence> referenceSeqs = null;

            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "MUMmer BVT : Successfully validated the File Path '{0}'.", filePath));

                using (FastaParser parser = new FastaParser())
                {
                    referenceSeqs = parser.Parse(filePath);
                    referenceSeq = referenceSeqs[0];
                    referenceSequence = referenceSeq.ToString();

                    // Gets the reference sequence from the configurtion file
                    string queryFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.SearchSequenceFilePathNode);

                    Assert.IsNotNull(queryFilePath);
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "MUMmer BVT : Successfully validated the Search File Path '{0}'.", queryFilePath));

                    querySeqs = parser.Parse(queryFilePath);
                    querySeq = querySeqs[0];
                    querySequence = querySeq.ToString();
                }
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                string referenceSeqAlphabet = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(referenceSeqAlphabet),
                    referenceSequence);

                querySequence = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceNode);

                referenceSeqAlphabet = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceAlphabetNode);

                querySeq = new Sequence(Utility.GetAlphabet(referenceSeqAlphabet),
                    querySequence);
                querySeqs = new List<ISequence>();
                querySeqs.Add(querySeq);
            }

            string mumLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMAlignLengthNode);

            MUMmer mum = new MUMmer3();
            mum.LengthOfMUM = long.Parse(mumLength, null);
            mum.PairWiseAlgorithm = new NeedlemanWunschAligner();
            mum.GapOpenCost = int.Parse(_utilityObj._xmlUtil.GetTextValue(nodeName, Constants.GapOpenCostNode),
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

            string expectedScore = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ScoreNodeName);
            Assert.AreEqual(expectedScore,
                align[0].PairwiseAlignedSequences[0].Score.ToString((IFormatProvider)null));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "MUMmer BVT : Successfully validated the score for the sequence '{0}' and '{1}'.",
                referenceSequence, querySequence));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "MUMmer BVT : Successfully validated the score for the sequence '{0}' and '{1}'.",
                referenceSequence, querySequence));

            string[] expectedSequences = _utilityObj._xmlUtil.GetTextValues(nodeName,
                Constants.ExpectedSequencesNode);

            IList<IPairwiseSequenceAlignment> expectedOutput = new List<IPairwiseSequenceAlignment>();

            IPairwiseSequenceAlignment seqAlign = new PairwiseSequenceAlignment();
            PairwiseAlignedSequence alignedSeq = new PairwiseAlignedSequence();
            alignedSeq.FirstSequence = new Sequence(referenceSeq.Alphabet, expectedSequences[0]);
            alignedSeq.SecondSequence = new Sequence(referenceSeq.Alphabet, expectedSequences[1]);
            alignedSeq.Score = Convert.ToInt32(expectedScore, (IFormatProvider)null);
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
        bool ValidateEdges(SequenceSuffixTree suffixTree, string nodeName)
        {
            Dictionary<int, Edge> ed = suffixTree.Edges;

            string[] actualStrtIndexes = new string[ed.Count];
            string[] actualEndIndexes = new string[ed.Count];

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
            string[] startIndexes =
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.EdgeStartIndexesNode).Split(',');
            string[] endIndexes =
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.EdgeEndIndexesNode).Split(',');

            // Gets the sorted edge list for the expected Edge list
            List<Edge> expectedEdgeList = GetSortedEdges(startIndexes, endIndexes);

            Console.WriteLine(string.Format((IFormatProvider)null,
                "MUMmer BVT : Total Edges Found is : '{0}'", ed.Keys.Count.ToString((IFormatProvider)null)));
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "MUMmer BVT : Total Edges Found is : '{0}'", ed.Keys.Count.ToString((IFormatProvider)null)));

            // Loops through all the edges and validates the same.
            for (int i = 0; i < expectedEdgeList.Count; i++)
            {
                if (!(actualEdgeList[i].StartIndex == expectedEdgeList[i].StartIndex)
                    && (actualEdgeList[i].EndIndex == expectedEdgeList[i].EndIndex))
                {
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "MUMmer BVT : Edges not matching at index '{0}'", i.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "MUMmer BVT : Edges not matching at index '{0}'", i.ToString((IFormatProvider)null)));
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
        /// <param name="LISActionType">Unique Match/Sub level LIS/LIS</param>
        /// <returns>True, if successfully validated</returns>
        bool ValidateUniqueMatches(IList<MaxUniqueMatch> matches,
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
                    firstSeqOrder = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.LisFirstSequenceMumOrderNode).Split(',');
                    firstSeqStart = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.LisFirstSequenceStartNode).Split(',');
                    length = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.LisLengthNode).Split(',');
                    secondSeqOrder = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.LisSecondSequenceMumOrderNode).Split(',');
                    secondSeqStart = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.LisSecondSequenceStartNode).Split(',');
                    break;
                case LISParameters.FindUniqueMatches:
                    firstSeqOrder = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.FirstSequenceMumOrderNode).Split(',');
                    firstSeqStart = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.FirstSequenceStartNode).Split(',');
                    length = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.LengthNode).Split(',');
                    secondSeqOrder = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.SecondSequenceMumOrderNode).Split(',');
                    secondSeqStart = _utilityObj._xmlUtil.GetTextValue(nodeName,
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
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "MUMmer BVT : Unique match not matching at index '{0}'", i.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "MUMmer BVT : Unique match not matching at index '{0}'", i.ToString((IFormatProvider)null)));
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
        private static bool CompareAlignment(IList<IPairwiseSequenceAlignment> actualAlignment,
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
        void ValidateMUMsGeneralTestCases(string nodeName, bool isFilePath,
            bool isAfterLIS, bool isLIS)
        {
            ISequence referenceSeq = null;
            ISequence querySeq = null;
            IList<ISequence> querySeqs = null;
            string referenceSequence = string.Empty;
            string querySequence = string.Empty;
            IList<ISequence> referenceSeqs = null;

            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "MUMmer BVT : Successfully validated the File Path '{0}'.", filePath));

                using (FastaParser parser = new FastaParser())
                {
                    referenceSeqs = parser.Parse(filePath);
                    referenceSeq = referenceSeqs[0];
                    referenceSequence = referenceSeq.ToString();

                    // Gets the reference sequence from the configurtion file
                    string queryFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                        Constants.SearchSequenceFilePathNode);

                    Assert.IsNotNull(queryFilePath);
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "MUMmer BVT : Successfully validated the Search File Path '{0}'.", queryFilePath));

                    querySeqs = parser.Parse(queryFilePath);
                    querySeq = querySeqs[0];
                    querySequence = querySeq.ToString();
                }
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                string referenceSeqAlphabet = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(referenceSeqAlphabet),
                    referenceSequence);

                querySequence = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceNode);

                referenceSeqAlphabet = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceAlphabetNode);

                querySeq = new Sequence(Utility.GetAlphabet(referenceSeqAlphabet),
                    querySequence);
                querySeqs = new List<ISequence>();
                querySeqs.Add(querySeq);
            }

            string mumLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMLengthNode);

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
        bool ValidateMums(string nodeName,
            IDictionary<ISequence, IList<MaxUniqueMatch>> result, ISequence querySeq)
        {
            string[] firstSeqOrder = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.LisFirstSequenceMumOrderNode).Split(',');
            string[] firstSeqStart = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.LisFirstSequenceStartNode).Split(',');
            string[] length = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.LisLengthNode).Split(',');
            string[] secondSeqOrder = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.LisSecondSequenceMumOrderNode).Split(',');
            string[] secondSeqStart = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.LisSecondSequenceStartNode).Split(',');

            foreach (var mum in result)
            {
                var mums = result[mum.Key];
                for (int i = 0; i < mums.Count; i++)
                {
                    if (0 != string.Compare(querySeq.ToString(), mums[i].Query.ToString(), StringComparison.CurrentCulture)
                       || (0 != string.Compare(firstSeqOrder[i], mums[i].FirstSequenceMumOrder.ToString((IFormatProvider)null), StringComparison.CurrentCulture))
                       || (0 != string.Compare(firstSeqStart[i], mums[i].FirstSequenceStart.ToString((IFormatProvider)null), StringComparison.CurrentCulture))
                       || (0 != string.Compare(length[i], mums[i].Length.ToString((IFormatProvider)null), StringComparison.CurrentCulture))
                       || (0 != string.Compare(secondSeqOrder[i], mums[i].SecondSequenceMumOrder.ToString((IFormatProvider)null), StringComparison.CurrentCulture))
                       || (0 != string.Compare(secondSeqStart[i], mums[i].SecondSequenceStart.ToString((IFormatProvider)null), StringComparison.CurrentCulture)))
                    {
                        Console.WriteLine(string.Format((IFormatProvider)null, "MUMmer BVT : There is no match at '{0}'", i.ToString((IFormatProvider)null)));
                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null, "MUMmer BVT : There is no match at '{0}'", i.ToString((IFormatProvider)null)));
                        return false;
                    }

                }
            }
            return true;
        }

        /// <summary>
        /// Validates most of the find matches suffix tree test cases with varying parameters.
        /// </summary>
        /// <param name="nodeName">Node name which needs to be read for execution.</param>
        /// <param name="isFilePath">Is File Path?</param>
        /// <param name="LISActionType">LIS action type enum</param>
        void ValidateFindMatchSimpleSuffixGeneralTestCases(string nodeName, bool isFilePath,
            LISParameters LISActionType)
        {
            ISequence referenceSeq = null;
            ISequence querySeq = null;
            string referenceSequence = string.Empty;
            string querySequence = string.Empty;
            IList<ISequence> referenceSeqs = null;

            if (isFilePath)
            {
                // Gets the reference sequence from the configurtion file
                string filePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "MUMmer BVT : Successfully validated the File Path '{0}'.", filePath));

                using (FastaParser parser = new FastaParser())
                {
                    referenceSeqs = parser.Parse(filePath);
                    referenceSeq = referenceSeqs[0];
                    referenceSequence = referenceSeq.ToString();
                }

                // Gets the reference sequence from the configurtion file
                string queryFilePath = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceFilePathNode);

                Assert.IsNotNull(queryFilePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "MUMmer BVT : Successfully validated the Search File Path '{0}'.", queryFilePath));

                IList<ISequence> querySeqs = null;
                using (FastaParser queryParser = new FastaParser())
                {
                    querySeqs = queryParser.Parse(queryFilePath);
                    querySeq = querySeqs[0];
                    querySequence = querySeq.ToString();
                }
            }
            else
            {
                // Gets the reference sequence from the configurtion file
                referenceSequence = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SequenceNode);

                string seqAlp = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.AlphabetNameNode);

                referenceSeq = new Sequence(Utility.GetAlphabet(seqAlp),
                    referenceSequence);

                querySequence = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceNode);

                seqAlp = _utilityObj._xmlUtil.GetTextValue(nodeName,
                    Constants.SearchSequenceAlphabetNode);

                querySeq = new Sequence(Utility.GetAlphabet(seqAlp),
                    querySequence);
            }

            string mumLength = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MUMLengthNode);

            // Builds the suffix for the reference sequence passed.
            using (SimpleSuffixTreeBuilder suffixTreeBuilder = new SimpleSuffixTreeBuilder())
            {
                ISuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSeq);

                IList<MaxUniqueMatch> matches = suffixTreeBuilder.FindMatches(suffixTree, querySeq,
                    long.Parse(mumLength, null));


                // Validates the Unique Matches.
                ApplicationLog.WriteLine("MUMmer BVT : Validating the Unique Matches");
                Assert.IsTrue(ValidateUniqueMatches(matches, nodeName, LISActionType));

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "MUMmer BVT : Successfully validated the all the unique matches for the sequence '{0}' and '{1}'.",
                    referenceSequence, querySequence));
            }
        }

        /// <summary>
        /// Validate Tree edges recursively till leaf node
        /// </summary>
        /// <param name="rootEdge">Root edge</param>
        /// <param name="nodeName">Edge start and end index values nodename</param>
        void ValidateRecursiveEdges(IEdge[] rootEdge, string nodeName)
        {
            // Gets all the edges to be validated as in xml.
            string[] startIndexes =
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MultiWaytreeEdgeStartIndexesNode).Split(',');
            string[] endIndexes =
                _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.MultiWaytreeEdgeEndIndexesNode).Split(',');

            for (int i = 0; i < rootEdge.Count(); i++)
            {
                if (rootEdge[i].IsLeaf)
                {
                    Assert.IsTrue(startIndexes.Contains(rootEdge[i].StartIndex.ToString((IFormatProvider)null)));
                    Assert.IsTrue(endIndexes.Contains(rootEdge[i].EndIndex.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                       "MUMmer BVT : Edges are matching at index '{0}'", i.ToString((IFormatProvider)null)));
                }
                else
                {
                    MultiWaySuffixEdge subEdge = rootEdge[i] as MultiWaySuffixEdge;
                    IEdge[] childOfSubNode = subEdge.GetChildren();
                    ValidateRecursiveEdges(childOfSubNode, nodeName);
                }
            }

        }
        #endregion Supported Methods
    }
}
