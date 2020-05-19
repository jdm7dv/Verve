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
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using MBF.Algorithms.Assembly.PaDeNA.Properties;
using MBF.Algorithms.Kmer;
using MBF.Util;

namespace MBF.Algorithms.Assembly.PaDeNA.Scaffold
{
    /// <summary>
    /// Maps reads to contigs.
    /// Class map reads to contigs using kmer method of alignment.
    /// Each Sequence is broken in kmers. And these kmer are aligned with 
    /// kmer of other sequences to generate ungapped alignments.
    /// </summary>
    public class ReadContigMapper : IReadContigMapper
    {
        #region IReadContigMapper Members

        /// <summary>
        /// Public method mapping Reads to Contigs.
        /// </summary>
        /// <param name="contigs">List of sequences of contigs.</param>
        /// <param name="reads">List of input reads.</param>
        /// <param name="kmerLength">Length of kmer.</param>
        /// <returns>Contig Read Map.</returns>
        public ReadContigMap Map(IList<ISequence> contigs, IList<ISequence> reads, int kmerLength)
        {
            KmerIndexerDictionary map = SequenceToKmerBuilder.BuildKmerDictionary(contigs, kmerLength);
            ReadContigMap maps = new ReadContigMap();
            using (ThreadLocal<char[]> rcBuilder = new ThreadLocal<char[]>(() => new char[kmerLength]))
            {
                Parallel.ForEach(reads, (ISequence readSequence) =>
                {
                    IEnumerable<string> kmers = SequenceToKmerBuilder.GetKmerStrings(readSequence, kmerLength);
                    ReadIndex read = new ReadIndex(readSequence);
                    IList<KmerIndexer> positions;
                    foreach (string kmer in kmers)
                    {
                        if (map.TryGetValue(kmer, out positions) ||
                            map.TryGetValue(kmer.GetReverseComplement(rcBuilder.Value), out positions))
                        {
                            read.ContigReadMatchIndexes.Add(positions);
                        }
                    }

                    IList<Task<IList<ReadMap>>> tasks =
                        new List<Task<IList<ReadMap>>>();

                    //Stores information about contigs for which tasks has been generated.
                    IList<int> visitedContigs = new List<int>();

                    //Creates Task for every read in nodes for a given contig.
                    for (int index = 0; index < read.ContigReadMatchIndexes.Count; index++)
                    {
                        int readPosition = index;
                        foreach (KmerIndexer kmer in read.ContigReadMatchIndexes[index])
                        {
                            int contigIndex = kmer.SequenceIndex;
                            if (!visitedContigs.Contains(contigIndex))
                            {
                                visitedContigs.Add(contigIndex);
                                tasks.Add(Task<IList<ReadMap>>.Factory.StartNew(t =>
                                    MapRead(readPosition, read.ContigReadMatchIndexes, contigIndex, read.ReadSequence.Count, kmerLength),
                                    TaskCreationOptions.AttachedToParent));
                            }
                        }
                    }

                    Dictionary<ISequence, IList<ReadMap>> overlapMaps =
                        new Dictionary<ISequence, IList<ReadMap>>();
                    for (int index = 0; index < visitedContigs.Count; index++)
                    {
                        overlapMaps.Add(contigs[visitedContigs[index]], tasks[index].Result);
                    }

                    lock (maps)
                    {
                        if (!maps.ContainsKey(read.ReadSequence.DisplayID))
                        {
                            maps.Add(read.ReadSequence.DisplayID, overlapMaps);
                        }
                        else
                        {
                            throw new ArgumentException(
                                string.Format(CultureInfo.CurrentCulture, Resource.DuplicatingReadIds, read.ReadSequence.DisplayID));
                        }
                    }
                });
            }

            return maps;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Traverse through list of contig-read match indexes for given read.
        /// </summary>
        /// <param name="position">Position from where list of 
        ///  indexes should be traversed.</param>
        /// <param name="contigReadMatch">List for contig-read match indexes</param>
        /// <param name="contigIndex">Index of contig sequence</param>
        /// <param name="readLength">Length of read</param>
        /// <param name="kmerLength">Length of kmer</param>
        /// <returns>List of read maps.</returns>
        private static IList<ReadMap> MapRead(
            int position,
            IList<IList<KmerIndexer>> contigReadMatch,
            int contigIndex,
            int readLength,
            int kmerLength)
        {
            IList<ReadMap> readMaps = new List<ReadMap>();
            for (int index = position; index < contigReadMatch.Count; index++)
            {
                foreach (KmerIndexer kmer in contigReadMatch[index])
                {
                    if (kmer.SequenceIndex == contigIndex)
                    {
                        FindContinuous(kmer, readMaps, index, kmerLength, readLength);
                    }
                }
            }

            return readMaps;
        }

        /// <summary>
        /// Merge continuous positions of a read in kmer indexes.
        /// </summary>
        /// <param name="kmer">Position of contig kmer.</param>
        /// <param name="readMaps">Alignment between read and contig.</param>
        /// <param name="position">Position of kmer in read.</param>
        /// <param name="kmerLength">Length of kmer.</param>
        /// <param name="readLength">Length of the read.</param>
        private static void FindContinuous(
            KmerIndexer kmer,
            IList<ReadMap> readMaps,
            int position,
            int kmerLength,
            int readLength)
        {
            //Create new object ReadInformation as read is encountered first time.
            if (readMaps.Count == 0)
            {
                foreach (int pos in kmer.Positions)
                {
                    ReadMap readMap = new ReadMap();
                    readMap.StartPositionOfContig = pos;
                    readMap.StartPositionOfRead = position;
                    readMap.Length = kmerLength;
                    SetReadContigOverlap(readLength, readMap);
                    readMaps.Add(readMap);
                }
            }
            else
            {
                //Merge current kmer node with previous kmer node of DeBruijn Graph, 
                //if they are continuous in either right or left traversal of graph.
                bool isMerged = false;
                foreach (int pos in kmer.Positions)
                {
                    foreach (ReadMap read in readMaps)
                    {
                        if (IsContinousRight(read, position, pos, kmerLength) ||
                            IsContinousLeft(read, position, pos, kmerLength))
                        {
                            read.Length++;
                            if (read.StartPositionOfContig > pos)
                            {
                                read.StartPositionOfContig = pos;
                            }

                            SetReadContigOverlap(readLength, read);
                            isMerged = true;
                            break;
                        }
                    }

                    //If not continuous a new object ReadMap is created to store new overlap.
                    if (isMerged == false)
                    {
                        ReadMap readmap = new ReadMap();
                        readmap.StartPositionOfContig = pos;
                        readmap.StartPositionOfRead = position;
                        readmap.Length = kmerLength;
                        SetReadContigOverlap(readLength, readmap);
                        readMaps.Add(readmap);
                    }
                }
            }
        }

        /// <summary>
        ///  Find if positions occur simultaneously of read in contig, 
        ///  if contig is traced from right direction
        /// </summary>
        /// <param name="map">map from previous position of read</param>
        /// <param name="readPosition">position of read</param>
        /// <param name="contigPosition">position of contig</param>
        /// <param name="length">length of kmer</param>
        /// <returns>True if continuous position of reads in contig.</returns>
        private static bool IsContinousRight(
            ReadMap map,
            int readPosition,
            int contigPosition,
            int length)
        {
            return (map.Length - length + map.StartPositionOfContig + 1) == contigPosition &&
                    (map.StartPositionOfRead + map.Length - length + 1) == readPosition;
        }

        /// <summary>
        ///  Find if positions occur simultaneously of read in contig, 
        ///  if contig is traced from left direction.
        /// </summary>
        /// <param name="map">map from previous position of read</param>
        /// <param name="readPosition">position of read</param>
        /// <param name="contigPosition">position of contig</param>
        /// <param name="length">length of kmer</param>
        /// <returns>true if continuous position of reads in contig.</returns>
        private static bool IsContinousLeft(
            ReadMap map,
            int readPosition,
            int contigPosition,
            int length)
        {
            return (map.Length - length + map.StartPositionOfRead + 1) == readPosition &&
                (map.StartPositionOfContig - 1) == contigPosition;
        }

        /// <summary>
        /// Determines whether read is full or partial overlap between read and contig.
        /// Overlap of read and contig
        /// FullOverlap
        /// ------------- Contig
        ///    ------     Read
        /// PartialOverlap
        /// -------------       Contig
        ///            ------   Read
        /// </summary>
        /// <param name="length">length of read</param>
        /// <param name="read">map of read to contig</param>
        private static void SetReadContigOverlap(int length, ReadMap read)
        {
            if (length == read.Length)
            {
                read.ReadOverlap = ContigReadOverlapType.FullOverlap;
            }
            else
            {
                read.ReadOverlap = ContigReadOverlapType.PartialOverlap;
            }
        }



        #endregion

        #region Internal Class

        /// <summary>
        /// Stores information of kmer reads map with contigs.
        /// </summary>
        internal class ReadIndex
        {
            #region Field Variables

            /// <summary>
            /// Contig stored in form of kmer maps of reads
            /// ------------------------------  Contig
            /// --------                        read1
            /// --------                        read5
            ///         --------                read2
            ///                 --------        read3
            ///                         ------  read4
            /// </summary>
            private IList<IList<KmerIndexer>> _contigReadMatchIndexes = new List<IList<KmerIndexer>>();

            /// <summary>
            /// Read Sequence
            /// </summary>
            private ISequence _readSequence;

            #endregion

            /// <summary>
            /// Initializes a new instance of the ReadIndex class.
            /// </summary>
            /// <param name="read">Read sequence</param>
            public ReadIndex(ISequence read)
            {
                _readSequence = read;
            }

            /// <summary>
            /// Gets the value of Read as indexes of contig overlap.
            /// </summary>
            public IList<IList<KmerIndexer>> ContigReadMatchIndexes
            {
                get { return _contigReadMatchIndexes; }
            }

            /// <summary>
            /// Gets the value of read sequence.
            /// </summary>
            public ISequence ReadSequence
            {
                get
                {
                    return _readSequence;
                }
            }
        }

        #endregion
    }
}