// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MBF.Properties;
using MBF.Util;

namespace MBF.Algorithms.Kmer
{
    /// <summary>
    /// Constructs k-mers from given input sequence(s).
    /// For each input sequence, k-mers are constructed by sliding 
    /// a frame of size kmerLength along the input sequence, 
    /// and extracting sub-sequence inside the frame.
    /// </summary>
    public class SequenceToKmerBuilder : IKmerBuilder
    {
        /// <summary>
        /// Builds k-mers from a list of given input sequences.
        /// For each sequence in input list, constructs a KmersOfSequence 
        /// corresponding to the sequence and associated k-mers.
        /// </summary>
        /// <param name="sequences">List of input sequences</param>
        /// <param name="kmerLength">k-mer length</param>
        /// <returns>List of KmersOfSequence instances</returns>
        public IEnumerable<KmersOfSequence> Build(IList<ISequence> sequences, int kmerLength)
        {
            if (kmerLength <= 0)
            {
                throw new ArgumentException(Resource.KmerLengthShouldBePositive);
            }

            if (sequences == null)
            {
                throw new ArgumentNullException("sequences");
            }

            Task<KmersOfSequence>[] kmerTasks = new Task<KmersOfSequence>[sequences.Count];
            int ndx = 0;

            foreach (ISequence sequence in sequences)
            {
                ISequence localSequence = sequence;
                kmerTasks[ndx] = Task<KmersOfSequence>.Factory.StartNew(
                    o => Build(localSequence, kmerLength), TaskCreationOptions.None);
                ndx++;
            }

            return kmerTasks.Select(t => t.Result);
        }

        /// <summary>
        /// For input sequence, constructs k-mers by sliding 
        /// a frame of size kmerLength along the input sequence.
        /// Track positions of occurance for each kmer in sequence.
        /// Constructs KmersOfSequence for sequence and associated k-mers.
        /// </summary>
        /// <param name="sequence">Input sequence</param>
        /// <param name="kmerLength">k-mer length</param>
        /// <returns>KmersOfSequence constructed from sequence and associated k-mers</returns>
        public KmersOfSequence Build(ISequence sequence, int kmerLength)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(Resource.SequenceCannotBeNull);
            }

            if (kmerLength <= 0)
            {
                throw new ArgumentException(Resource.KmerLengthShouldBePositive);
            }

            if (kmerLength > sequence.Count)
            {
                throw new ArgumentException(Resource.KmerLengthIsTooLong);
            }

            // kmers maintains the map between k-mer strings to list of positions in sequence
            Dictionary<string, List<int>> kmers = new Dictionary<string, List<int>>();

            // Sequence 'kmer' stores the k-mer in each window.
            // Construct each k-mer using range from sequence
            for (int i = 0; i <= sequence.Count - kmerLength; ++i)
            {
                string kmerString = sequence.Range(i, kmerLength).ToString();
                if (kmers.ContainsKey(kmerString))
                {
                    kmers[kmerString].Add(i);
                }
                else
                {
                    kmers[kmerString] = new List<int>() { i };
                }
            }

            return new KmersOfSequence(
                sequence,
                kmerLength,
                new HashSet<KmersOfSequence.KmerPositions>(kmers.Values.Select(l => new KmersOfSequence.KmerPositions(l))));
        }

        /// <summary>
        /// Builds k-mers from a list of given input sequences.
        /// For each sequence in input list, constructs a KmersOfSequence 
        /// corresponding to the sequence and associated k-mers.
        /// </summary>
        /// <param name="sequences">List of input sequences</param>
        /// <param name="kmerLength">k-mer length</param>
        /// <returns>List of KmersOfSequence instances</returns>
        public static KmerIndexerDictionary BuildKmerDictionary(IList<ISequence> sequences, int kmerLength)
        {
            if (sequences == null)
            {
                throw new ArgumentNullException("sequences");
            }

            if (kmerLength <= 0)
            {
                throw new ArgumentException(Properties.Resource.KmerLengthShouldBePositive);
            }

            Task<KmerPositionDictionary>[] kmerTasks = new Task<KmerPositionDictionary>[sequences.Count];
            int ndx = 0;

            for (int index = 0; index < sequences.Count; index++)
            {
                ISequence localSequence = sequences[index];
                kmerTasks[ndx] = Task<KmerPositionDictionary>.Factory.StartNew(
                    o => BuildKmerDictionary(localSequence, kmerLength), TaskCreationOptions.None);
                ndx++;
            }

            KmerIndexerDictionary maps = new KmerIndexerDictionary();
            IList<KmerIndexer> kmerIndex;
            char[] rcBuilder = new char[kmerLength];
            for (int index = 0; index < kmerTasks.Length; index++)
            {
                foreach (KeyValuePair<string, IList<int>> value in kmerTasks[index].Result)
                {
                    if (maps.TryGetValue(value.Key, out kmerIndex) ||
                        maps.TryGetValue(value.Key.GetReverseComplement(rcBuilder), out kmerIndex))
                    {
                        kmerIndex.Add(new KmerIndexer(index, value.Value));
                    }
                    else
                    {
                        maps.Add(value.Key, new List<KmerIndexer> { new KmerIndexer(index, value.Value) });
                    }
                }
            }

            return maps;
        }

        /// <summary>
        /// For input sequence, constructs k-mers by sliding 
        /// a frame of size kmerLength along the input sequence.
        /// Track positions of occurance for each kmer in sequence.
        /// Constructs KmersOfSequence for sequence and associated k-mers.
        /// </summary>
        /// <param name="sequence">Input sequence</param>
        /// <param name="kmerLength">k-mer length</param>
        /// <returns>KmersOfSequence constructed from sequence and associated k-mers</returns>
        public static KmerPositionDictionary BuildKmerDictionary(ISequence sequence, int kmerLength)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            if (kmerLength > sequence.Count)
            {
                throw new ArgumentException(Properties.Resource.KmerLengthIsTooLong);
            }

            // kmers maintains the map between k-mer strings to list of positions in sequence
            KmerPositionDictionary kmers = new KmerPositionDictionary();

            // Sequence 'kmer' stores the k-mer in each window.
            // Construct each k-mer using range from sequence
            for (int i = 0; i <= sequence.Count - kmerLength; ++i)
            {
                string kmerString = sequence.Range(i, kmerLength).ToString();
                if (kmers.ContainsKey(kmerString))
                {
                    kmers[kmerString].Add(i);
                }
                else
                {
                    kmers[kmerString] = new List<int>() { i };
                }
            }

            return kmers;
        }

        /// <summary>
        /// Gets the set of kmer strings that occur in given sequences
        /// </summary>
        /// <param name="sequence">Source Sequence</param>
        /// <param name="kmerLength">Kmer Length</param>
        /// <returns>Set of kmer strings</returns>
        public static IEnumerable<string> GetKmerStrings(ISequence sequence, int kmerLength)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            if (kmerLength <= 0)
            {
                throw new ArgumentException(Resource.KmerLengthShouldBePositive);
            }

            if (kmerLength > sequence.Count)
            {
                throw new ArgumentException(Resource.KmerLengthIsTooLong);
            }


            IList<string> kmers = new List<string>();
            for (int i = 0; i <= sequence.Count - kmerLength; ++i)
            {
                string kmerString = sequence.Range(i, kmerLength).ToString();
                kmers.Add(kmerString);
            }

            return kmers;
        }

        #region Parallelized Kmer builder for PaDeNA
        /// <summary>
        /// Builds k-mers from a list of given input sequences.
        /// </summary>
        /// <param name="sequences">List of input sequences</param>
        /// <param name="kmerLength">k-mer length</param>
        /// <returns>List of KmersOfSequence instances</returns>
        public static Dictionary<string, KmerDataGraphNodePair> BuildPaDeNAKmerDictionary(IList<ISequence> sequences, int kmerLength)
        {
            if (kmerLength <= 0)
                throw new ArgumentException(Resource.KmerLengthShouldBePositive);
            if (sequences == null)
                throw new ArgumentNullException("sequences");

           int rangeSize = (int)Math.Ceiling((float)sequences.Count / Environment.ProcessorCount);
            
           var kmerDictionaries = Partitioner.Create(0, sequences.Count, rangeSize).AsParallel().Select
                (p => BuildPaDeNAKmerDictionary(p.Item1, p.Item2, kmerLength, sequences)).ToList();

           return MergeKmerDictionaries(kmerDictionaries);
        }

        /// <summary>
        /// For input sequence, constructs k-mers by sliding 
        /// a frame of size kmerLength along the input sequence.
        /// Track positions of occurance for each kmer in sequence.
        /// Constructs KmersOfSequence for sequence and associated k-mers.
        /// </summary>
        /// <param name="start">Start index of sequence range.</param>
        /// <param name="end">End index of sequence range.</param>
        /// <param name="kmerLength">Kmer Length.</param>
        /// <param name="sequences">List of sequences.</param>
        /// <returns>Kmer Dictionary.</returns>
        public static Dictionary<string, KmerDataGraphNodePair> BuildPaDeNAKmerDictionary(
          int start,
          int end,
          int kmerLength,
          IList<ISequence> sequences)
        {
            if (kmerLength <= 0)
            {
                throw new ArgumentException(Resource.KmerLengthShouldBePositive);
            }

            if (sequences == null)
            {
                throw new ArgumentNullException("sequences");
            }
          
            char[] rcBuilder = new char[kmerLength];
            Dictionary<string, KmerDataGraphNodePair> kmerDict = new Dictionary<string, KmerDataGraphNodePair>();
            for (int index = start; index < end; index++)
            {

                // Sequence 'kmer' stores the k-mer in each window.
                // Construct each k-mer using range from sequence
                for (int i = 0; i <= sequences[index].Count - kmerLength; ++i)
                {
                    ISequence kmerSeq = sequences[index].Range(i, kmerLength);
                    if (!kmerSeq.Any<ISequenceItem>(a => a.IsAmbiguous || a.IsGap))
                    {
                        string kmerString = kmerSeq.ToString();
                        string kmerStringRC = kmerString.GetReverseComplement(rcBuilder);
                        bool isNormalOrientation = string.Compare(kmerString, kmerStringRC, StringComparison.OrdinalIgnoreCase) <= 0;
                        string kmer = isNormalOrientation ? kmerString : kmerStringRC; //take the alphabetically first one

                        KmerDataGraphNodePair kmerData;
                        if (!kmerDict.TryGetValue(kmer, out kmerData))
                        {
                            kmerDict.Add(kmer,
                            new KmerDataGraphNodePair(new KmerData(index, i, isNormalOrientation), isNormalOrientation));
                        }
                        else
                        {
                            kmerData.Kmer.IncrementCount(isNormalOrientation);
                        }
                    }
                }
            }

            return kmerDict;
        }

        /// <summary>
        /// Merges a list of kmer dictionaries into a single one.
        /// Divides the list into chunks based on the number of cores available.
        /// Merges the results from each core in a binary fashion.
        /// </summary>
        /// <param name="kmerDicts">List of kmer dictionaries to merge</param>
        /// <returns>Merged Dictionary</returns>
        private static Dictionary<string, KmerDataGraphNodePair> MergeKmerDictionaries(List<Dictionary<string, KmerDataGraphNodePair>> kmerDicts)
        {
            if (kmerDicts.Count == 0)
            {
                return null;
            }
            else if (kmerDicts.Count == 1)
            {
                return kmerDicts[0];
            }

            int rangeSize = (int)Math.Ceiling((float)kmerDicts.Count / Environment.ProcessorCount);
            var mergedDicts =
                Partitioner.Create(0, kmerDicts.Count, rangeSize).AsParallel().
                Select(p => MergeKmerDictionaries(kmerDicts, p.Item1, p.Item2)).ToList();

            while (mergedDicts.Count > 1)
            {
                mergedDicts =
                    Partitioner.Create(0, mergedDicts.Count, 2).AsParallel().
                    Select(p => MergeKmerDictionaries(mergedDicts, p.Item1, p.Item2)).ToList();
            }

            return mergedDicts[0];
        }

        /// <summary>
        /// Merges a given range of dicrtionaries in list into a single one.
        /// </summary>
        /// <param name="kmerDicts">List of kmer dictionaries to merge</param>
        /// <param name="start">Start index of range</param>
        /// <param name="end">End index of range</param>
        /// <returns>Merged Dictionary</returns>
        private static Dictionary<string, KmerDataGraphNodePair> MergeKmerDictionaries(
            List<Dictionary<string, KmerDataGraphNodePair>> kmerDicts, 
            int start, 
            int end)
        {
            var mergedDict = kmerDicts[start];
            KmerDataGraphNodePair kmerTuple;
            string kmerString;
            KmerData kmerData;
            for (int i = start + 1; i < end; i++)
            {
                foreach (var item in kmerDicts[i])
                {
                    kmerString = item.Key;
                    kmerData = item.Value.Kmer;
                    mergedDict.TryGetValue(kmerString, out kmerTuple);
                    if (kmerTuple != null)
                    {
                        kmerTuple.Kmer.Count += kmerData.Count;
                        kmerTuple.Kmer.CountRC += kmerData.CountRC;
                    }
                    else
                    {
                        mergedDict.Add(kmerString, item.Value);
                    }
                }
            }

            return mergedDict;
        }
 
        #endregion
    }
}