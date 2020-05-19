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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using MBF.IO.BAM;
using MBF.IO.SAM;
using SAMUtils.Properties;

namespace SAMUtils
{
    /// <summary>
    /// Class implementing merge command of SAMUtility.
    /// </summary>
    public class Merge
    {
        #region Public Fields

        /// <summary>
        /// Paths of output file and input files.
        /// </summary>
        [DefaultArgument(ArgumentType.Multiple, HelpText = "File Paths")]
        public string[] FilePaths;

        /// <summary>
        /// Sort input files by read names.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Sort by read name", ShortName = "n")]
        public bool SortByReadName;

        /// <summary>
        /// Header for merged file.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, HelpText = "Copy the Header from ths file", ShortName = "h")]
        public string HeaderFile;

        /// <summary>
        /// Usage.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce)]
        public bool Usage;

        #endregion

        #region Private Fields

        /// <summary>
        /// Header to be written in merged file.
        /// </summary>
        private SAMAlignmentHeader _header = null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Merge multiple sorted alignments.
        /// SAMUtil.exe out.bam in1.bam in2.bam
        /// </summary>
        public void DoMerge()
        {
            if (FilePaths == null)
            {
                throw new InvalidOperationException("FilePath");
            }

            if (FilePaths.Length < 3)
            {
                throw new InvalidOperationException(Resources.MergeHelp);
            }

            IList<IList<BAMSortedIndex>> sortedIndexes = new List<IList<BAMSortedIndex>>();
            IList<SequenceAlignmentMap> sequenceAlignmentMaps = new List<SequenceAlignmentMap>();
            IList<int> help = new List<int>();
            Parallel.For(1, FilePaths.Length, (int index) =>
            {
                IList<BAMSortedIndex> sortedIndex;
                BAMParser parser = new BAMParser(); ;
                SequenceAlignmentMap map;
                if (index == 1)
                {
                    try
                    {
                        map = parser.Parse(FilePaths[1]);
                    }
                    catch
                    {
                        throw new InvalidOperationException(Resources.InvalidBAMFile);
                    }

                    if (map == null)
                    {
                        throw new InvalidOperationException(Resources.EmptyFile);
                    }

                    if (string.IsNullOrEmpty(HeaderFile) && map.Header.RecordFields.Count == 0)
                    {
                        throw new InvalidOperationException(Resources.HeaderMissing);
                    }

                    if (!string.IsNullOrEmpty(HeaderFile))
                    {
                        SAMParser parse = new SAMParser();
                        SequenceAlignmentMap head;
                        try
                        {
                            head = parse.Parse(HeaderFile);
                        }
                        catch
                        {
                            throw new InvalidOperationException(Resources.IncorrectHeaderFile);   
                        }

                        if (head == null)
                        {
                            throw new InvalidOperationException(Resources.EmptyFile);
                        }

                        _header = head.Header;                       
                    }
                    else
                    {
                        _header = map.Header;
                    }

                    sortedIndex = Sort(map, SortByReadName ? BAMSortByFields.ReadNames : BAMSortByFields.ChromosomeCoordinates);
                }
                else
                {
                    try
                    {
                        map = parser.Parse(FilePaths[index]);
                    }
                    catch
                    {
                        throw new InvalidOperationException(Resources.InvalidBAMFile);
                    }
                    
                    if (map == null)
                    {
                        throw new InvalidOperationException(Resources.EmptyFile);
                    }

                    sortedIndex = Sort(map, SortByReadName ? BAMSortByFields.ReadNames : BAMSortByFields.ChromosomeCoordinates);
                }

                lock (sortedIndexes)
                {
                    sortedIndexes.Add(sortedIndex);
                    sequenceAlignmentMaps.Add(map);
                }
            });

            string filePath = Path.GetTempFileName();
            using (FileStream fstemp = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
            {
                BAMFormatter formatter = new BAMFormatter();
                formatter.WriteHeader(_header, fstemp);
                int[] indexes = new int[sortedIndexes.Count];

                if (SortByReadName)
                {
                    IList<BAMSortedIndex> sortedIndex = sortedIndexes.Select(a => a.First()).ToList();
                    WriteMergeFileSortedByReadName(sortedIndex, fstemp, formatter, sequenceAlignmentMaps);
                }
                else
                {
                    WriteMergeFile(sortedIndexes, fstemp, formatter, sequenceAlignmentMaps);
                }

                using (FileStream fsoutput = new FileStream(FilePaths[0], FileMode.Create, FileAccess.Write))
                {
                    fstemp.Seek(0, SeekOrigin.Begin);
                    formatter.CompressBAMFile(fstemp, fsoutput);
                }
            }

            File.Delete(filePath);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sort Sequence Alignments.
        /// </summary>
        /// <param name="map">SAM object to be sorted.</param>
        /// <param name="bAMSortType">Sort based on genomic coordinates or read name.</param>
        /// <returns></returns>
        private IList<BAMSortedIndex> Sort(SequenceAlignmentMap map, BAMSortByFields bAMSortType)
        {
            BAMSort sort = new BAMSort(map, bAMSortType);
            return sort.Sort();
        }

        /// <summary>
        /// Sort and merge multiple SAM objects
        /// </summary>
        /// <param name="sortedIndexes">Sorted Indexes of SAM object.</param>
        /// <param name="fstemp">Temporary tream to write alignments.</param>
        /// <param name="formatter">Format aligned sequences in BAM format.</param>
        /// <param name="sequenceAlignmentMaps">List of SAM objects to be merged.</param>
        private void WriteMergeFileSortedByReadName(
           IList<BAMSortedIndex> sortedIndexes,
           FileStream fstemp,
           BAMFormatter formatter,
           IList<SequenceAlignmentMap> sequenceAlignmentMaps)
        {
            List<SAMAlignedSequence> alignedSeqs = new List<SAMAlignedSequence>();

            for (int i = 0; i < sortedIndexes.Count; i++)
            {
                BAMSortedIndex bamSortedIndex = sortedIndexes[i];
                if (bamSortedIndex != null)
                {
                    if (bamSortedIndex.MoveNext())
                    {
                        alignedSeqs.Add(sequenceAlignmentMaps[i].QuerySequences[bamSortedIndex.Current]);
                    }
                    else
                    {
                        alignedSeqs.Add(null);
                    }
                }
                else
                {
                    alignedSeqs.Add(null);
                }
            }

            int smallestIndex = -1;

            do
            {
                for (int index = 0; index < alignedSeqs.Count; index++)
                {
                    if (alignedSeqs[index] != null)
                    {
                        if (smallestIndex == -1)
                        {
                            smallestIndex = index;
                        }
                        else
                        {
                            if (0 < string.Compare(alignedSeqs[smallestIndex].QName, alignedSeqs[index].QName, StringComparison.OrdinalIgnoreCase))
                            {
                                smallestIndex = index;
                            }
                        }
                    }
                }

                if (smallestIndex > -1)
                {
                    SAMAlignedSequence alignSeqTowrite = alignedSeqs[smallestIndex];

                    if (sortedIndexes[smallestIndex].MoveNext())
                    {
                        int nextIndex = sortedIndexes[smallestIndex].Current;
                        alignedSeqs[smallestIndex] = sequenceAlignmentMaps[smallestIndex].QuerySequences[nextIndex];
                    }
                    else
                    {
                        alignedSeqs[smallestIndex] = null;
                        smallestIndex = -1;
                    }

                    formatter.WriteAlignedSequence(_header, alignSeqTowrite, fstemp);
                }

            } while (!alignedSeqs.All(a => a == null));
        }

        /// <summary>
        /// Sort and merge multiple SAM objects
        /// </summary>
        /// <param name="sortedIndexes">Sorted Indexes of SAM object.</param>
        /// <param name="fstemp">Temporary tream to write alignments.</param>
        /// <param name="formatter">Format aligned sequences in BAM format.</param>
        /// <param name="sequenceAlignmentMaps">List of SAM objects to be merged.</param>
        private void WriteMergeFile(IList<IList<BAMSortedIndex>> sortedIndexes, FileStream fstemp, BAMFormatter formatter, IList<SequenceAlignmentMap> sequenceAlignmentMaps)
        {
            List<SAMAlignedSequence> alignedSeqs = new List<SAMAlignedSequence>();
            int[] sortedIndex = new int[sequenceAlignmentMaps.Count];

            for (int i = 0; i < sortedIndexes.Count; i++)
            {
                BAMSortedIndex bamSortedIndex = sortedIndexes[i].ElementAt(sortedIndex[i]);
                if (bamSortedIndex != null)
                {
                    if (bamSortedIndex.MoveNext())
                    {
                        alignedSeqs.Add(sequenceAlignmentMaps[i].QuerySequences[bamSortedIndex.Current]);
                    }
                    else
                    {
                        alignedSeqs.Add(null);
                    }
                }
                else
                {
                    alignedSeqs.Add(null);
                }
            }

            int smallestIndex = -1;

            do
            {
                for (int index = 0; index < alignedSeqs.Count; index++)
                {
                    if (alignedSeqs[index] != null)
                    {
                        if (smallestIndex == -1)
                        {
                            smallestIndex = index;
                        }
                        else
                        {
                            if (0 < string.Compare(alignedSeqs[smallestIndex].RName, alignedSeqs[index].RName, StringComparison.OrdinalIgnoreCase))
                            {
                                smallestIndex = index;
                            }
                            else if (alignedSeqs[smallestIndex].RName.Equals(alignedSeqs[index].RName))
                            {
                                if (alignedSeqs[smallestIndex].Pos > alignedSeqs[index].Pos)
                                {
                                    smallestIndex = index;
                                }
                            }
                        }
                    }
                }

                if (smallestIndex > -1)
                {
                    SAMAlignedSequence alignSeqTowrite = alignedSeqs[smallestIndex];

                    if (sortedIndexes[smallestIndex].ElementAt(sortedIndex[smallestIndex]).MoveNext())
                    {
                        int nextIndex = sortedIndexes[smallestIndex].ElementAt(sortedIndex[smallestIndex]).Current;
                        alignedSeqs[smallestIndex] = sequenceAlignmentMaps[smallestIndex].QuerySequences[nextIndex];
                    }
                    else
                    {
                        sortedIndex[smallestIndex]++;
                        if (sortedIndex[smallestIndex] < sortedIndexes[smallestIndex].Count &&
                            sortedIndexes[smallestIndex].ElementAt(sortedIndex[smallestIndex]).MoveNext())
                        {
                            int nextIndex = sortedIndexes[smallestIndex].ElementAt(sortedIndex[smallestIndex]).Current;
                            alignedSeqs[smallestIndex] = sequenceAlignmentMaps[smallestIndex].QuerySequences[nextIndex];
                        }
                        else
                        {
                            alignedSeqs[smallestIndex] = null;
                            smallestIndex = -1;
                        }
                    }

                    formatter.WriteAlignedSequence(_header, alignSeqTowrite, fstemp);
                }

            } while (!alignedSeqs.All(a => a == null));

        }

        #endregion
    }
}