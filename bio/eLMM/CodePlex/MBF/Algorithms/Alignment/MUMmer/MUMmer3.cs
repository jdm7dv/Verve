//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bio.Algorithms.SuffixTree;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// Implementats of MUMmer interface. Defines methods necessary to 
    /// complete the MUMmer 3.0 method for sequence alignment. Implements following
    /// 1. Instantiate appropriate interface and invoke method to build Suffix Tree
    ///     using Kurtz Algorithm
    /// 2. Instantiate appropriate interface and invoke method to stream the query
    ///     sequence through suffix tree and find the MUMs
    /// 3. Sort the MUMs found in the order of start index in reference sequence
    /// 4. Instantiate appropriate interface and invoke method to find Longest
    ///     Increasing Sequence (LIS)
    /// </summary>
    public class MUMmer3 : MUMmer
    {
        #region -- Member Variables --

        /// <summary>
        /// Reference sequence number
        /// </summary>
        private int referenceSequenceNumber;

        #endregion -- Member Variables --

        #region -- Properties --
        /// <summary>
        /// Gets the name of the current Alignment algorithm used.
        /// This is a overriden property from the abstract parent.
        /// This property returns the Name of our algorithm i.e 
        /// MUMmer v3.0 algorithm.
        /// </summary>
        public override string Name
        {
            get { return Properties.Resource.MUMMER3; }
        }

        /// <summary>
        /// Gets the Description of the current Alignment algorithm used.
        /// This is a overriden property from the abstract parent.
        /// This property returns a simple description of what 
        /// MUMmer3 class implements.
        /// </summary>
        public override string Description
        {
            get { return Properties.Resource.MUMMER3DESC; }
        }

        /// <summary>
        /// Gets or sets the id of reference sequence.
        /// The class uses to id to find the reference sequence in the list of
        /// sequences. And also the same is used to differentiate reference 
        /// sequence from query sequence during streaming.
        /// </summary>
        public override int ReferenceSequenceNumber
        {
            get { return referenceSequenceNumber; }
            set { referenceSequenceNumber = value; }
        }

        #endregion -- Properties --

        /// <summary>
        /// Generates list of MUMs for each query sequence.
        /// This returns the MUMs that are generated. 
        /// The MUMs are not sorted or processed using 
        /// Longest Increasing Subsequence (LIS).
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequenceList">List of query sequences</param>
        /// <returns>List of MUMs for each query sequence</returns>
        public override IDictionary<ISequence, IList<MaxUniqueMatch>> GetMUMs(
            ISequence referenceSequence,
            IList<ISequence> querySequenceList)
        {
            return GetMUMs(referenceSequence, querySequenceList, false);
        }

        /// <summary>
        /// Generates list of MUMs for each query sequence.
        /// This returns the MUMs that are generated.
        /// If 'performLIS' is true, MUMs are sorted and processed 
        /// using Longest Increasing Subsequence (LIS). If 'performLIS' 
        /// is false, MUMs are returned immediately after streaming.
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequenceList">List of query sequences</param>
        /// <param name="performLIS">Boolean indicating whether Longest Increasing 
        /// Subsequence (LIS) modules is run on MUMs before returning</param>
        /// <returns>List of MUMs for each query sequence</returns>
        public override IDictionary<ISequence, IList<MaxUniqueMatch>> GetMUMs(
            ISequence referenceSequence, 
            IList<ISequence> querySequenceList, 
            bool performLIS)
        {
            GetMUMsValidate(referenceSequence, querySequenceList);

            // Initializations
            IDictionary<ISequence, IList<MaxUniqueMatch>> queryMums = new Dictionary<ISequence, IList<MaxUniqueMatch>>();
                
            // Step1 : building suffix trees using reference sequence
            SequenceSuffixTree suffixTree = BuildSuffixTree(referenceSequence);

            // On each query sequence aligned with reference sequence
            //foreach (ISequence sequence in querySequenceList)
            Parallel.ForEach(querySequenceList, sequence =>
            {
                bool isQuerySequence = true;
                IList<MaxUniqueMatch> mumList;

                if (sequence.Equals(referenceSequence))
                {
                    isQuerySequence = false;
                }

                if (isQuerySequence)
                {
                    // Step2 : streaming process is performed with the query sequence
                    mumList = Streaming(suffixTree, sequence, LengthOfMUM);

                    if (performLIS)
                    {
                        // Step3(a) : sorted mum list based on reference sequence
                        mumList = SortMum(mumList);

                        if (mumList.Count > 0)
                        {
                            // Step3(b) : LIS using greedy cover algorithm
                            mumList = CollectLongestIncreasingSubsequence(mumList);
                        }
                        else
                        {
                            mumList = null;
                        }
                    }

                    lock (queryMums)
                    {
                        queryMums.Add(sequence, mumList);
                    }
                }
            });

            return queryMums;
        }

        /// <summary>
        /// Build Suffix Tree using reference sequence.
        /// This method using Kurtz algorithm to build the Suffix Tree
        /// </summary>
        /// <param name="referenceSequence">Reference sequence number</param>
        /// <returns>Suffix Tree</returns>
        protected override SequenceSuffixTree BuildSuffixTree(ISequence referenceSequence)
        {
            ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
            SequenceSuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(referenceSequence);
            return suffixTree;
        }

        /// <summary>
        /// Traverse the suffix tree using query sequence and return list of MUMs
        /// </summary>
        /// <param name="suffixTree">Suffix Tree</param>
        /// <param name="sequence">Query Sequence</param>
        /// <param name="lengthOfMUM">Minimum length of MUM</param>
        /// <returns>List of MUMs</returns>
        protected override IList<MaxUniqueMatch> Streaming(SequenceSuffixTree suffixTree, ISequence sequence, long lengthOfMUM)
        {
            ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
            return suffixTreeBuilder.FindMatches(suffixTree, sequence, lengthOfMUM);
        }

        /// <summary>
        /// Sort the MUM list.
        /// Sorts the MUM list by order of start index in reference sequence
        /// </summary>
        /// <param name="mumList">MUM list that has to be sorted</param>
        /// <returns>Sorted MUM list</returns>
        protected override IList<MaxUniqueMatch> SortMum(IList<MaxUniqueMatch> mumList)
        {
            IEnumerable<MaxUniqueMatch> sortedMums =
                mumList.OrderBy(Mums => Mums.FirstSequenceStart);
            mumList = sortedMums.ToList();

            for (int index = 0; index < mumList.Count; index++)
            {
                mumList[index].FirstSequenceMumOrder = index + 1;
            }

            return mumList;
        }

        /// <summary>
        /// Get the MUMs in the order of Longest Increasing Subsequence using position in query sequence
        /// </summary>
        /// <param name="sortedMumList">Sorted list of MUMs</param>
        /// <returns>MUMs in longest increasing subsequence order</returns>
        protected override IList<MaxUniqueMatch> CollectLongestIncreasingSubsequence(IList<MaxUniqueMatch> sortedMumList)
        {
            // Get and return Longest Increasing Subsequence
            ILongestIncreasingSubsequence lis = new LongestIncreasingSubsequence();
            return lis.GetLongestSequence(sortedMumList);
        }

        /// <summary>
        /// Validate the inputs
        /// </summary>
        /// <param name="referenceSequence">reference sequence</param>
        /// <param name="querySequenceList">list of input sequences</param>
        private void GetMUMsValidate(
                ISequence referenceSequence,
                IList<ISequence> querySequenceList)
        {
            if (null == referenceSequence)
            {
                string message = Properties.Resource.ReferenceSequenceCannotBeNull;
                throw new ArgumentNullException("referenceSequence");
            }

            if (null == querySequenceList)
            {
                string message = Properties.Resource.QueryListCannotBeNull;
                throw new ArgumentNullException("querySequenceList");
            }

            foreach (ISequence querySequence in querySequenceList)
            {
                if (null == querySequence)
                {
                    string message = Properties.Resource.QuerySequenceCannotBeNull;
                    throw new ArgumentNullException("querySequenceList", message);
                }

                if (referenceSequence.Alphabet != querySequence.Alphabet)
                {
                    string message = Properties.Resource.InputAlphabetsMismatch;
                    throw new ArgumentException(message);
                }
            }

            if (1 > LengthOfMUM)
            {
                string message = Properties.Resource.MUMLengthTooSmall;
                throw new ArgumentException(message);
            }
        }
    }
}
