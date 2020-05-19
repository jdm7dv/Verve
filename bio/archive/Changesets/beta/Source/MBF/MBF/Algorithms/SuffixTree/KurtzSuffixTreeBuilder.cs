// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBF.Algorithms.Alignment;

namespace MBF.Algorithms.SuffixTree
{
    /// <summary>
    /// This class implements Kurtz Algorithm (using Hash Table) to build the Suffix Tree.
    /// Steps to build the suffix tree are:
    ///     For every prefix in suffix of sequence string
    ///     Add prefix to the tree, prefix can be added in to ways
    ///         1. New edge is created
    ///         2. Previous edge is updated (split the tree and and new edges)
    /// <remarks>
    /// Kurtz Algorithm adds a terminating symbol if the last character
    /// repeats anywhere in the string. This implementation has a method that
    /// (GetSequenceSymbol) takes care of terminating symbol without actually 
    /// adding the symbol to the sequence. Note that the Suffix Tree returned
    /// by this implementation will have index reference to symbol which actually
    /// does not exist in the Sequence.
    /// </remarks>
    /// </summary>
    public class KurtzSuffixTreeBuilder : ISuffixTreeBuilder
    {
        #region -- Constants --

        /// <summary>
        /// Character used as terminating symbol for Suffix Tree
        /// </summary>
        private const byte TERMINATING_SYMBOL = 36;

        /// <summary>
        /// Character used as Concatenating symbol for Suffix Tree
        /// Note: This is specific to NUCmer implementation
        /// </summary>
        private const byte CONCATENATING_SYMBOL = 43;

        #endregion

        #region -- Member Variables --

        /// <summary>
        /// Hash table size for storing edges
        /// </summary>
        private int _maxHashKey;

        /// <summary>
        /// Minimum Required length of MUM
        /// </summary>
        private long _minimumLengthOfMUM;

        /// <summary>
        /// Last Match found
        /// </summary>
        private MaxUniqueMatch _lastMatch;

        /// <summary>
        /// Last edge in the previous match
        /// </summary>
        private Edge _lastEdge;

        /// <summary>
        /// Current suffix tree
        /// </summary>
        private SequenceSuffixTree _suffixTree;

        /// <summary>
        /// Reference sequence
        /// </summary>
        private ISequence _referenceSequences = null;

        /// <summary>
        /// Query sequence
        /// </summary>
        private ISequence _querySequence = null;

        /// <summary>
        /// If reference sequence is segmented sequence then store sequence's encoded values in 
        /// segemented sequence in list of byte array.
        /// </summary>
        private IList<byte[]> _referenceSymbolsList = null;

        /// <summary>
        /// Reference sequence's encoded values are in the byte array.
        /// </summary>
        private byte[] _referenceSymbols = null;

        /// <summary>
        /// Query sequence's encoded values in the byte array.
        /// </summary>
        private byte[] _querySymbols = null;

        /// <summary>
        /// If set, find all the matches irrespective of the uniqueness in 
        /// reference or query sequence.
        /// </summary>
        private bool _findMaximumMatch = false;

        #endregion

        #region -- Constructors --

        /// <summary>
        /// Initializes a new instance of the KurtzSuffixTreeBuilder class
        /// </summary>
        public KurtzSuffixTreeBuilder()
        {
            // Set the hash key value
            _maxHashKey = int.MaxValue;
        }

        #endregion

        #region -- Properties --

        /// <summary>
        /// Gets the length of reference sequence
        /// </summary>
        private int ReferenceLength
        {
            get
            {
                SegmentedSequence segments = _referenceSequences as SegmentedSequence;
                if (segments == null)
                {
                    return _referenceSequences.Count + 1;
                }

                return segments.Count + segments.Sequences.Count;
            }
        }

        #endregion

        #region -- Public Methods --

        /// <summary>
        /// Builds the Suffix Tree using Kurtz Algorithm(using Hash Table)
        /// </summary>
        /// <example>
        /// --------------------------------------------------
        /// Create the Sequence from string (let say DNA sequence "CACCAS")
        /// --------------------------------------------------
        /// string aOriginalStr = "CACCAS";
        /// Sequence aInput = new Sequence(Alphabets.DNA, aOriginalStr);
        /// --------------------------------------------------
        /// Instantiate and run the suffix tree builder
        /// --------------------------------------------------
        /// ISuffixTreeBuilder suffixTreeBuilder = new KurtzSuffixTreeBuilder();
        /// SuffixTree suffixTree = suffixTreeBuilder.BuildSuffixTree(aInput);
        /// </example>
        /// <param name="sequence">Input Sequence</param>
        /// <returns>Suffix Tree</returns>
        public ISuffixTree BuildSuffixTree(ISequence sequence)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            ValidateReferenceSequence(sequence);

            // Initialize
            Edge.NodeCount = 1;
            InitializeReferenceSequence(sequence);

            // Create Tasks
            Dictionary<byte, IList<int>> treeTasks = new Dictionary<byte, IList<int>>();
            // Loop through subset of sequence string and build the suffix tree
            // this will loop through the sequence once and collect all the indexes needed.
            for (int index = 0; index < ReferenceLength; index++)
            {
                IList<int> startIndices = null;

                if (!treeTasks.TryGetValue(GetReferenceSymbol(index), out startIndices))
                {
                    startIndices = new List<int>();
                    treeTasks.Add(GetReferenceSymbol(index), startIndices);
                }

                startIndices.Add(index);
            }

            IList<Task<SequenceSuffixTree>> tasks = treeTasks.Values.Select(
                    indices => Task<SequenceSuffixTree>.Factory.StartNew(
                            t => AppendSuffix(indices, sequence), TaskCreationOptions.None)).ToList();

            // Wait for all the task
            Task.WaitAll(tasks.ToArray());

            // Merge the branches of tree
            _suffixTree = new SequenceSuffixTree(sequence, tasks.Sum(task => task.Result.Edges.Count));
            Edge edgeFound = null;
            foreach (Task<SequenceSuffixTree> task in tasks)
            {
                foreach (KeyValuePair<int, Edge> edge in task.Result.Edges)
                {
                    if (_suffixTree.Edges.TryGetValue(edge.Key, out edgeFound))
                    {
                        Insert(edgeFound, _suffixTree);
                    }
                    else
                    {
                        _suffixTree.Edges.Add(edge.Key, edge.Value);
                    }
                }
            }

            // return the suffix tree
            return _suffixTree;
        }

        /// <summary>
        /// Find the matches of sequence in suffix tree
        /// </summary>
        /// <param name="suffixTree">Suffix Tree</param>
        /// <param name="searchSequence">Query searchSequence</param>
        /// <param name="lengthOfMUM">Mininum length of MUM</param>
        /// <returns>Matches found</returns>
        public IList<MaxUniqueMatch> FindMatches(
            ISuffixTree suffixTree,
            ISequence searchSequence,
            long lengthOfMUM)
        {
            _findMaximumMatch = false;
            return FindMatchWithOption(suffixTree, searchSequence, lengthOfMUM);
        }

        /// <summary>
        /// Finds all the matches of given sequence in suffix tree irrespective of the uniqueness in
        /// reference or query sequence
        /// </summary>
        /// <param name="suffixTree">Suffix Tree</param>
        /// <param name="searchSequence">Query searchSequence</param>
        /// <param name="lengthOfMUM">Mininum length of MUM</param>
        /// <returns>Matches found</returns>
        public IList<MaxUniqueMatch> FindMaximumMatches(
            ISuffixTree suffixTree,
            ISequence searchSequence,
            long lengthOfMUM)
        {
            _findMaximumMatch = true;
            return FindMatchWithOption(suffixTree, searchSequence, lengthOfMUM);
        }

        #endregion

        #region -- Private Methods --

        /// <summary>
        /// Find the matches of sequence in suffix tree
        /// </summary>
        /// <param name="suffixTree">Suffix tree to searh on</param>
        /// <param name="searchSequence">query sequence to find matches</param>
        /// <param name="lengthOfMUM">Minimum length of the match</param>
        /// <returns>Matches found</returns>
        private IList<MaxUniqueMatch> FindMatchWithOption(
                ISuffixTree suffixTree,
                ISequence searchSequence,
                long lengthOfMUM)
        {
            if (suffixTree == null)
            {
                throw new ArgumentNullException("suffixTree");
            }

            if (searchSequence == null)
            {
                throw new ArgumentNullException("searchSequence");
            }
            ValidateSequence(suffixTree.Sequence, searchSequence);

            // Initialize
            _minimumLengthOfMUM = lengthOfMUM;
            _suffixTree = suffixTree as SequenceSuffixTree;
            InitializeReferenceSequence(suffixTree.Sequence);
            InitializeQuerySequence(searchSequence);

            int interval = (int)(_querySequence.Count - (_minimumLengthOfMUM - 1)) / Environment.ProcessorCount;
            if (interval < 1)
            {
                interval = 1;
            }

            IList<Task<List<MaxUniqueMatch>>> result = new List<Task<List<MaxUniqueMatch>>>();
            for (int index = 0; index < _querySequence.Count - (_minimumLengthOfMUM - 1); index += interval)
            {
                int taskIndex = index;
                result.Add(
                    Task.Factory.StartNew<List<MaxUniqueMatch>>(
                        o => FindMUMs(taskIndex, interval),
                        TaskCreationOptions.None));
            }

            List<MaxUniqueMatch> mergedList = new List<MaxUniqueMatch>();
            foreach (List<MaxUniqueMatch> local in result.Select(l => l.Result))
            {
                // Check if there is overlap, last MUM of mergedList overlaps with first MUM of local
                if (0 == mergedList.Count)
                {
                    mergedList.AddRange(local.Select(m => m));
                }
                else
                {
                    if (0 < local.Count)
                    {
                        MaxUniqueMatch previous = mergedList.Last();
                        MaxUniqueMatch current = local.First();

                        if ((current.SecondSequenceStart >= previous.SecondSequenceStart
                            && current.SecondSequenceStart <= previous.SecondSequenceStart + previous.Length)
                            && (current.SecondSequenceStart + current.Length >= previous.SecondSequenceStart
                            && current.SecondSequenceStart + current.Length <= previous.SecondSequenceStart + previous.Length))
                        {
                            local.RemoveAt(0);
                        }

                        if (0 < local.Count)
                        {
                            mergedList.AddRange(local.Select(m => m));
                        }
                    }
                }
            }
            // Order the mum list with query sequence order
            for (int index = 0; index < mergedList.Count; index++)
            {
                mergedList[index].FirstSequenceMumOrder = index + 1;
                mergedList[index].SecondSequenceMumOrder = index + 1;
            }

            return mergedList;
        }

        /// <summary>
        /// Validate input sequences are of same encoding
        /// </summary>
        /// <param name="sequence">Reference sequence</param>
        private static void ValidateReferenceSequence(ISequence sequence)
        {
            ISequence baseSequence = null;
            SegmentedSequence segments = sequence as SegmentedSequence;

            if (segments != null)
            {
                IEnumerator<ISequence> sequences = segments.Sequences.GetEnumerator();
                if (sequences.MoveNext())
                {
                    baseSequence = sequences.Current;
                }

                while (sequences.MoveNext())
                {
                    if ((baseSequence.UseEncoding != sequences.Current.UseEncoding)
                        || (baseSequence.Encoding != sequences.Current.Encoding))
                    {
                        throw new InvalidOperationException(Properties.Resource.InvalidSuffixTreeEncoding);
                    }
                }
            }
        }

        /// <summary>
        /// Initialize reference sequence.
        /// If the sequence is DV-enabled pre-fetch the sequence in local byte array
        /// </summary>
        /// <param name="sequence"></param>
        private void InitializeReferenceSequence(ISequence sequence)
        {
            // If the sequence is DV enabled
            SegmentedSequence segments = sequence as SegmentedSequence;

            if (segments == null)
            {
                _referenceSymbols = new byte[sequence.Count];
                (sequence as IList<byte>).CopyTo(_referenceSymbols, 0);
            }
            else
            {
                _referenceSymbolsList = new List<byte[]>();
                foreach (Sequence subSequence in segments.Sequences)
                {
                    byte[] tmpSeqBytes = new byte[subSequence.Count];
                    (subSequence as IList<byte>).CopyTo(tmpSeqBytes, 0);
                    _referenceSymbolsList.Add(tmpSeqBytes);
                }
            }

            _referenceSequences = sequence;
        }

        /// <summary>
        /// Validate input sequences are of same encoding
        /// </summary>
        /// <param name="referenceSequence">Reference sequence</param>
        /// <param name="querySequence">Query sequence</param>
        private static void ValidateSequence(ISequence referenceSequence, ISequence querySequence)
        {
            ISequence baseSequence = null;
            SegmentedSequence segments = referenceSequence as SegmentedSequence;

            if (segments == null)
            {
                baseSequence = referenceSequence;
            }
            else
            {
                baseSequence = segments.Sequences[0];
            }

            if ((baseSequence.UseEncoding != querySequence.UseEncoding)
                || (baseSequence.Encoding != querySequence.Encoding))
            {
                throw new InvalidOperationException(Properties.Resource.InvalidStreamEncoding);
            }
        }

        /// <summary>
        /// Initialize query sequence.
        /// If the sequence is DV-enabled pre-fetch the sequence in local byte array
        /// </summary>
        /// <param name="sequence"></param>
        private void InitializeQuerySequence(ISequence sequence)
        {
            _querySymbols = new byte[sequence.Count];
            (sequence as IList<byte>).CopyTo(_querySymbols,0);
            _querySequence = sequence;
        }

        /// <summary>
        /// Get the symbol from refrence sequence at given index
        /// </summary>
        /// <param name="index">Index of symbol</param>
        /// <returns>Symbol at index</returns>
        private byte GetReferenceSymbol(int index)
        {
            if (_referenceSymbols != null)
            {
                if (index == _referenceSymbols.Length)
                {
                    return TERMINATING_SYMBOL;
                }
                else
                {
                    return _referenceSymbols[index];
                }
            }
            else
            {
                int length = 0;
                for (int sequenceIndex = 0; sequenceIndex < _referenceSymbolsList.Count; sequenceIndex++)
                {
                    index -= length;

                    if (index < _referenceSymbolsList[sequenceIndex].Length)
                    {
                        return _referenceSymbolsList[sequenceIndex][index];
                    }
                    else if (index == _referenceSymbolsList[sequenceIndex].Length)
                    {
                        if (sequenceIndex < _referenceSymbolsList.Count - 1)
                        {
                            return CONCATENATING_SYMBOL;
                        }
                        else
                        {
                            return TERMINATING_SYMBOL;
                        }
                    }

                    length = _referenceSymbolsList[sequenceIndex].Length + 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// Get the symbol from query sequence at given index
        /// </summary>
        /// <param name="index">Index of symbol</param>
        /// <returns>Symbol at index</returns>
        private byte GetQuerySymbol(int index)
        {
            if (_querySymbols == null)
            {
                return (_querySequence as IList<byte>)[index];
            }

            return _querySymbols[index];
        }

        /// <summary>
        /// Finds the MUMs for suffix in given interval of query sequence
        /// </summary>
        /// <param name="startIndex">startindex of interval</param>
        /// <param name="interval">length of interval</param>
        /// <returns></returns>
        private List<MaxUniqueMatch> FindMUMs(int startIndex, int interval)
        {
            int secondSequenceStart = 0;
            int secondSequenceEnd = 0;

            List<MaxUniqueMatch> mumList = new List<MaxUniqueMatch>();
            MaxUniqueMatch match = null;

            for (int index = startIndex; index < startIndex + interval && index < _querySequence.Count; index++)
            {
                // loop through each suffix of search sequence and find the MUM in suffixTree
                match = Search(index);

                if (null != match)
                {
                    // Make sure the mum found does not already exists in query sequence
                    if ((match.SecondSequenceStart >= secondSequenceStart
                            && match.SecondSequenceStart <= secondSequenceEnd)
                            && (match.SecondSequenceStart + match.Length >= secondSequenceStart
                            && match.SecondSequenceStart + match.Length <= secondSequenceEnd))
                    {
                        continue;
                    }

                    mumList.Add(match);

                    secondSequenceStart = match.SecondSequenceStart;
                    secondSequenceEnd = match.SecondSequenceStart + match.Length;

                    if (_lastEdge.IsLeaf
                            && match.SecondSequenceStart + match.Length == _querySequence.Count)
                    {
                        // At index, we have found a MUM, such that there cannot be
                        // another MUM (till index + Current MUM Length) who length is
                        // greater then Current MUM
                        index += _lastMatch.Length - 1;
                    }
                }
            }

            // Order the mum list with query sequence order
            for (int index = 0; index < mumList.Count; index++)
            {
                mumList[index].FirstSequenceMumOrder = index + 1;
                mumList[index].SecondSequenceMumOrder = index + 1;
            }

            return mumList;
        }

        /// <summary>
        /// Find the position (hash) of the edge in the hash table
        /// </summary>
        /// <param name="node">Start node of the edge</param>
        /// <param name="character">Starting character of the edge</param>
        /// <returns>Hash code of node</returns>
        private int Hash(int node, byte character)
        {
            return ((node << 8) + character) % _maxHashKey;
        }

        /// <summary>
        /// Add suffix to the tree, the loop inside will break under two conditions
        ///     1. If you have reached the leaf node
        ///     2. If you have reached end of suffix
        /// </summary>
        /// <param name="startIndices">List of index of the first character of suffix</param>
        /// <param name="sequence">Reference sequence</param>
        /// <returns>Suffix tree</returns>
        private SequenceSuffixTree AppendSuffix(IList<int> startIndices, ISequence sequence)
        {
            SequenceSuffixTree tree = new SequenceSuffixTree(sequence, startIndices.Count);

            foreach (int index in startIndices)
            {
                int startIndex = index;
                int parentNode = 0;
                Edge edge = null;
                bool continueInsert = true;

                do
                {
                    edge = Find(parentNode, GetReferenceSymbol(startIndex), tree);

                    if (null == edge)
                    {
                        edge = new Edge(startIndex,
                                ReferenceLength - 1,
                                parentNode);

                        Insert(edge, tree);

                        continueInsert = false;
                        break;
                    }
                    else
                    {
                        startIndex++;

                        if (edge.StartIndex < edge.EndIndex)
                        {
                            for (int counter = edge.StartIndex + 1; counter <= edge.EndIndex; counter++)
                            {
                                if (GetReferenceSymbol(startIndex) != GetReferenceSymbol(counter))
                                {
                                    parentNode = SplitEdge(edge, counter - 1, parentNode, tree);

                                    // Add the leaf edge
                                    Edge newEdge = new Edge(startIndex,
                                            ReferenceLength - 1,
                                            parentNode);

                                    Insert(newEdge, tree);
                                    continueInsert = false;
                                    break;
                                }

                                startIndex++;
                            }
                        }

                        parentNode = edge.EndNode;
                    }
                } while (startIndex < ReferenceLength && continueInsert);
            }

            return tree;
        }

        /// <summary>
        /// Find the edge leading out of the node
        /// </summary>
        /// <param name="node">Start node</param>
        /// <param name="character">Starting character of the edge</param>
        /// <param name="tree">Suffix Tree</param>
        /// <returns>Edge leading out from the node</returns>
        private Edge Find(int node, byte character, SequenceSuffixTree tree)
        {
            Edge edge = null;
            Edge edgeFound = null;

            // Find the position of required node
            int position = Hash(node, character);

            // If the node exists in Edge list
            // Find the edge and return it
            while (tree.Edges.TryGetValue(position, out edgeFound))
            {
                if (((edgeFound.StartNode == node)
                        && (character == GetReferenceSymbol(edgeFound.StartIndex)))
                    || (-1 == edgeFound.StartNode))
                {
                    edge = edgeFound;
                    break;
                }

                position = ++position % _maxHashKey;
            }

            return edge;
        }

        /// <summary>
        /// Insert an Edge into Hash Table, if not already in the list
        /// </summary>
        /// <param name="edge">Edge that has to be inserted</param>
        /// <param name="tree">Suffix Tree</param>
        private void Insert(Edge edge, SequenceSuffixTree tree)
        {
            Edge edgeFound = null;

            int position = Hash(
                    edge.StartNode,
                    GetReferenceSymbol(edge.StartIndex));

            if (tree.Edges.TryGetValue(position, out edgeFound))
            {
                // If the hash already exists in tree, increment the edge till a position which
                // does not exist in tree is reached
                while (-1 != edgeFound.StartNode)
                {
                    position = ++position % _maxHashKey;
                    if (!tree.Edges.TryGetValue(position, out edgeFound))
                    {
                        break;
                    }
                }
            }

            tree.Edges[position] = edge;
        }

        /// <summary>
        /// Splitting the edge for creation of new node
        ///     Remove the edge at given position
        ///     Add new edge to the parent of removed edge
        ///     Add the removed edge to the new edge (requires start character to be recalculated)
        /// </summary>
        /// <param name="edge">Edge of tree which has to be split</param>
        /// <param name="splitAt">Split edge at index</param>
        /// <param name="parentNode">Parent node of new edge</param>
        /// <param name="tree">Suffix Tree</param>
        /// <returns>Value of new edge End node</returns>
        private int SplitEdge(Edge edge, int splitAt, int parentNode, SequenceSuffixTree tree)
        {
            Remove(edge, tree);
            Edge newEdge = new Edge(
                edge.StartIndex,
                splitAt,
                parentNode);
            newEdge.IsLeaf = false;
            Insert(newEdge, tree);

            edge.StartIndex = splitAt + 1;
            edge.StartNode = newEdge.EndNode;
            Insert(edge, tree);

            return newEdge.EndNode;
        }

        /// <summary>
        /// Remove the given edge from the tree and fill the gap
        /// </summary>
        /// <param name="edge">Edge that has to be removed</param>
        /// <param name="tree">Suffix Tree</param>
        private void Remove(Edge edge, SequenceSuffixTree tree)
        {
            Edge edgeFound = null;

            int position = Hash(
                    edge.StartNode,
                    GetReferenceSymbol(edge.StartIndex));

            while (tree.Edges[position].StartNode != edge.StartNode
                    || tree.Edges[position].StartIndex != edge.StartIndex)
            {
                position = ++position % _maxHashKey;
            }

            // loop through the branch is broken and move them to fill the gap
            while (tree.Edges.TryGetValue(position, out edgeFound))
            {
                edgeFound.StartNode = -1;
                int tempPosition = position;
                while (tree.Edges.TryGetValue(position, out edgeFound))
                {
                    position = ++position % _maxHashKey;
                    if (!tree.Edges.TryGetValue(position, out edgeFound))
                    {
                        return;
                    }

                    if (-1 == edgeFound.StartNode)
                    {
                        return;
                    }

                    byte symbol = GetReferenceSymbol(edgeFound.StartIndex);

                    int nextPosition = Hash(edgeFound.StartNode, symbol);

                    if (position >= nextPosition && nextPosition > tempPosition)
                    {
                        continue;
                    }

                    if (nextPosition > tempPosition && tempPosition > position)
                    {
                        continue;
                    }

                    if (tempPosition > position && position >= nextPosition)
                    {
                        continue;
                    }

                    break;
                }

                tree.Edges[tempPosition] = new Edge(edgeFound);
            }
        }

        /// <summary>
        /// Validate following conditions and create MUM only if valid
        /// 1. Make sure there is no split edge in reference sequence (this 
        ///     represent duplicate in reference sequence)
        /// 2. Validate required length of MUM
        /// </summary>
        /// <param name="referenceEndIndex">End index of string found in reference sequence</param>
        /// <param name="queryStartIndex">Start index of string found in query sequence</param>
        /// <param name="length">Length of match</param>
        /// <returns>Maximum Unique Match</returns>
        private MaxUniqueMatch CreateMUM(
                int referenceEndIndex,
                int queryStartIndex,
                int length)
        {
            MaxUniqueMatch newMUM = null;

            if (length >= _minimumLengthOfMUM)
            {
                newMUM = new MaxUniqueMatch();
                newMUM.Query = _querySequence;
                newMUM.FirstSequenceStart = referenceEndIndex - (length - 1);
                newMUM.SecondSequenceStart = queryStartIndex;
                newMUM.Length = length;
            }

            return newMUM;
        }

        /// <summary>
        /// Search for a query sequence in give Suffix Tree for existence
        /// </summary>
        /// <param name="startIndex">Index of first suffix character in search sequence</param>
        /// <returns>Does query sequence exists</returns>
        private MaxUniqueMatch Search(int startIndex)
        {
            // if the input sequence is empty
            if (0 == _querySequence.Count)
            {
                return null;
            }

            int hash;
            Edge edge = null;

            hash = Hash(0, GetQuerySymbol(startIndex));

            // if edge that starts with start character does not exits
            if (!_suffixTree.Edges.TryGetValue(hash, out edge))
            {
                return null;
            }

            if (-1 == edge.StartNode)
            {
                return null;
            }

            int queryIndex = startIndex;
            int referenceIndex = 0;
            MaxUniqueMatch match = null;
            bool matchFound = false;
            Edge nextEdge = null;

            while (!matchFound)
            {
                for (referenceIndex = edge.StartIndex; referenceIndex <= edge.EndIndex; referenceIndex++)
                {
                    if (queryIndex == _querySequence.Count || referenceIndex == ReferenceLength)
                    {
                        match = CreateMUM(
                                referenceIndex - 1,
                                startIndex,
                                queryIndex - startIndex);
                        matchFound = true;
                        break;
                    }

                    if (GetReferenceSymbol(referenceIndex) != GetQuerySymbol(queryIndex++))
                    {
                        match = CreateMUM(
                                referenceIndex - 1,
                                startIndex,
                                queryIndex - 1 - startIndex);
                        matchFound = true;
                        break;
                    }
                }

                if (!matchFound)
                {
                    if (queryIndex < _querySequence.Count)
                    {
                        hash = Hash(edge.EndNode, GetQuerySymbol(queryIndex));

                        // If the node exists in Edge list
                        // Find the edge and return it
                        while (_suffixTree.Edges.TryGetValue(hash, out nextEdge))
                        {
                            if (GetQuerySymbol(queryIndex) ==
                                    GetReferenceSymbol(nextEdge.StartIndex))
                            {
                                break;
                            }

                            hash = ++hash % _maxHashKey;
                        }

                        if (null == nextEdge)
                        {
                            match = CreateMUM(
                                    edge.EndIndex,
                                    startIndex,
                                    queryIndex - startIndex);

                            matchFound = true;
                        }
                        else
                        {
                            edge = nextEdge;
                        }
                    }
                    else
                    {
                        match = CreateMUM(
                                edge.EndIndex,
                                startIndex,
                                queryIndex - startIndex);

                        matchFound = true;
                    }
                }
            }

            if (null == match)
            {
                return null;
            }

            _lastEdge = edge;
            _lastMatch = match;

            // Make sure there is not split, if there is split, then this is a duplicate
            // and should be ignored.
            // And the length of match is greater then minimum required length
            if (!edge.IsLeaf && !_findMaximumMatch)
            {
                match = null;
            }

            return match;
        }

        #endregion
    }
}
