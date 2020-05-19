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
using System.Runtime.Serialization;
using MBF.Algorithms.Alignment;
using MBF.Util;

namespace MBF.IO.SAM
{
    /// <summary>
    /// Class to hold sequence alignment map (SAM) structure.
    /// </summary>
    [Serializable]
    public class SequenceAlignmentMap : ISequenceAlignment
    {
        /// <summary>
        /// Holds SAM header.
        /// </summary>
        private SAMAlignmentHeader _header;

        /// <summary>
        /// holds the metadta.
        /// </summary>
        private Dictionary<string, object> _metadata;

        /// <summary>
        /// Holds list of query sequences present in this SAM object.
        /// </summary>
        private IList<SAMAlignedSequence> _querySequences;

        /// <summary>
        /// Flag to indicate whether DV is enabled or not.
        /// </summary>
        private bool _isDVEnabled;

        /// <summary>
        /// Default constructor.
        /// Creates SequenceAlignmentMap instance.
        /// </summary>
        public SequenceAlignmentMap() : this(new SAMAlignmentHeader()) { }

        /// <summary>
        /// Creates SequenceAlignmentMap instance.
        /// </summary>
        /// <param name="header">SAM header.</param>
        /// <param name="querySequences">A list of virtual sequences.</param>
        public SequenceAlignmentMap(SAMAlignmentHeader header, IVirtualAlignedSequenceList<SAMAlignedSequence> querySequences)
            : this(header)
        {
            _querySequences = querySequences;
            _isDVEnabled = true;
        }

        /// <summary>
        /// Creates SequenceAlignmentMap instance.
        /// </summary>
        /// <param name="header">SAM header.</param>
        public SequenceAlignmentMap(SAMAlignmentHeader header)
        {
            if (header == null)
            {
                throw new ArgumentNullException("header");
            }

            _isDVEnabled = false;
            _header = header;
            _metadata = new Dictionary<string, object>();
            _metadata.Add(Helper.SAMAlignmentHeaderKey, _header);
            _querySequences = new List<SAMAlignedSequence>();
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected SequenceAlignmentMap(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _header = (SAMAlignmentHeader)info.GetValue("header", typeof(SAMAlignmentHeader));
            _metadata = new Dictionary<string, object>();
            _metadata.Add(Helper.SAMAlignmentHeaderKey, _header);
            _querySequences = (IList<SAMAlignedSequence>)info.GetValue("sequences", typeof(IList<SAMAlignedSequence>));
            _isDVEnabled = false;

            if (_querySequences == null)
            {
                _querySequences = new List<SAMAlignedSequence>();
            }

            else if (_querySequences is IVirtualAlignedSequenceList<SAMAlignedSequence>)
            {
                _isDVEnabled = true;
            }
        }

        /// <summary>
        /// Gets the SAM header.
        /// </summary>
        public SAMAlignmentHeader Header
        {
            get
            {
                return _header;
            }
        }

        /// <summary>
        /// Gets the query sequences present in this alignment.
        /// </summary>
        public IList<SAMAlignedSequence> QuerySequences
        {
            get
            {
                return _querySequences;
            }
        }

        /// <summary>
        /// Gets list of aligned sequences present in this alignment.
        /// </summary>
        public IList<IAlignedSequence> AlignedSequences
        {
            get
            {
                ReadOnlyAlignedSequenceCollection collection = new ReadOnlyAlignedSequenceCollection(_querySequences);

                return collection;
            }
        }

        /// <summary>
        /// Gets list of source sequences present in this alignment.
        /// Note that this method always returns an empty readonly list.
        /// </summary>
        public IList<ISequence> Sequences
        {
            get { return new List<ISequence>().AsReadOnly(); }
        }

        /// <summary>
        /// Gets the metadata of this alignment.
        /// </summary>
        public Dictionary<string, object> Metadata
        {
            get { return _metadata; }
        }

        /// <summary>
        /// Gets documentation object.
        /// </summary>
        public object Documentation
        {
            get;
            set;
        }

        /// <summary>
        /// Returns list of reference sequences present in this header. 
        /// </summary>
        public IList<string> GetRefSequences()
        {
            return _header.GetReferenceSequences();
        }

        /// <summary>
        /// Returns list of SequenceRanges objects which represents reference sequences present in the header. 
        /// </summary>
        public IList<SequenceRange> GetReferenceSequenceRanges()
        {
            return _header.GetReferenceSequenceRanges();
        }

        /// <summary>
        /// Gets the paired reads.
        /// </summary>
        /// <returns>List of paired read.</returns>
        public IList<PairedRead> GetPairedReads()
        {
            if (_isDVEnabled)
            {
                return GetDVAwarePairedReads(0, 0, true);
            }
            else
            {
                return GetInMemoryPairedReads(0, 0, true);
            }
        }

        /// <summary>
        /// Gets the paired reads.
        /// </summary>
        /// <param name="libraryName">Name of the library present in CloneLibrary.</param>
        /// <returns>List of paired read.</returns>
        public IList<PairedRead> GetPairedReads(string libraryName)
        {
            if (string.IsNullOrEmpty(libraryName))
            {
                throw new ArgumentNullException("libraryName");
            }

            CloneLibraryInformation libraryInfo = CloneLibrary.Instance.GetLibraryInformation(libraryName);

            if (libraryInfo == null)
            {
                throw new ArgumentOutOfRangeException("libraryName");
            }

            return GetPairedReads(libraryInfo);
        }

        /// <summary>
        /// Gets the paired reads.
        /// </summary>
        /// <param name="libraryInfo">Library information.</param>
        /// <returns>List of paired read.</returns>
        public IList<PairedRead> GetPairedReads(CloneLibraryInformation libraryInfo)
        {
            if (libraryInfo == null)
            {
                throw new ArgumentNullException("libraryInfo");
            }

            return GetPairedReads(libraryInfo.MeanLengthOfInsert, libraryInfo.StandardDeviationOfInsert);
        }

        /// <summary>
        /// Gets the paired reads.
        /// </summary>
        /// <param name="meanLengthOfInsert">Mean of the insert length.</param>
        /// <param name="standardDeviationOfInsert">Standard deviation of insert length.</param>
        /// <returns>List of paired read.</returns>
        public IList<PairedRead> GetPairedReads(float meanLengthOfInsert, float standardDeviationOfInsert)
        {
            if (_isDVEnabled)
            {
                return GetDVAwarePairedReads(meanLengthOfInsert, standardDeviationOfInsert);
            }
            else
            {
                return GetInMemoryPairedReads(meanLengthOfInsert, standardDeviationOfInsert);
            }
        }

        /// <summary>
        /// Method for serializing the SAM object.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("header", _header);
            info.AddValue("sequences", _querySequences);
        }

        /// <summary>
        /// This Method calculates mean and standard deviation from the available reads
        /// and then using this information updates the type of reads.
        /// </summary>
        /// <param name="allreads">All reads.</param>
        /// <param name="sum">Pre calculated sum of insert length of reads 
        /// (invavled in calculation mean and std deviation) if available, else pass 0.</param>
        /// <param name="count">Pre calculated count of reads (invavled in calculation mean and std deviation)
        /// if available, else pass 0.</param>
        private static void UpdateType(IList<PairedRead> allreads, double sum, int count)
        {
            // Calculate the Mean of insert lengths. 
            // Note: In case MultipleHits, Orphan, Chimera, StructuralAnomaly we can't calculate insert length.
            IEnumerable<PairedRead> reads = allreads.Where(R => R.PairedType == PairedReadType.Normal || R.PairedType == PairedReadType.LengthAnomaly);
            if (reads.Count() == 0)
            {
                return;
            }

            if (sum == 0 || count == 0)
            {
                sum = reads.Sum(PE => PE.InsertLength);
                count = reads.Count();
            }

            float mean = (float)(sum / count);
            sum = 0;
            foreach (PairedRead pairedRead in reads)
            {
                sum += Math.Pow((pairedRead.InsertLength - mean), 2);
            }

            float stddeviation = (float)Math.Sqrt(sum / count);
            // µ + 3σ
            float upperLimit = mean + (3 * stddeviation);
            // µ - 3σ
            float lowerLimit = mean - (3 * stddeviation);
            foreach (PairedRead pairedRead in reads)
            {

                if (pairedRead.InsertLength > upperLimit || pairedRead.InsertLength < lowerLimit)
                {
                    pairedRead.PairedType = PairedReadType.LengthAnomaly;
                }
                else
                {
                    pairedRead.PairedType = PairedReadType.Normal;
                }
            }
        }

        /// <summary>
        /// Gets the paired reads when SAMAligned sequences are in memory.
        /// </summary>
        /// <param name="meanLengthOfInsert">Mean of the insert length.</param>
        /// <param name="standardDeviationOfInsert">Standard deviation of insert length.</param>
        /// <param name="calculate">If this flag is set then mean and standard deviation will
        /// be calculated from the paired reads instead of specified.</param>
        /// <returns>List of paired read.</returns>
        private IList<PairedRead> GetInMemoryPairedReads(float meanLengthOfInsert, float standardDeviationOfInsert, bool calculate = false)
        {
            // Dictionary helps to get the information at one pass of alinged sequence list.
            Dictionary<string, PairedRead> pairedReads = new Dictionary<string, PairedRead>();
            double sum = 0;
            int count = 0;

            for (int i = 0; i < QuerySequences.Count; i++)
            {
                PairedRead pairedRead;
                SAMAlignedSequence read = QuerySequences[i];
                if ((read.Flag & SAMFlags.PairedRead) == SAMFlags.PairedRead)
                {
                    if (pairedReads.TryGetValue(read.QName, out pairedRead))
                    {
                        if (pairedRead.Read2 == null || pairedRead.Read1 == null)
                        {
                            if (pairedRead.Read2 == null)
                            {
                                pairedRead.Read2 = read;
                            }
                            else
                            {
                                pairedRead.Read1 = read;
                            }

                            pairedRead.PairedType = PairedRead.GetPairedReadType(pairedRead.Read1, pairedRead.Read2, meanLengthOfInsert, standardDeviationOfInsert);
                            if (pairedRead.PairedType == PairedReadType.Normal || pairedRead.PairedType == PairedReadType.LengthAnomaly)
                            {
                                pairedRead.InsertLength = PairedRead.GetInsertLength(pairedRead.Read1, pairedRead.Read2);
                                if (calculate)
                                {
                                    sum += pairedRead.InsertLength;
                                    count++;
                                }
                            }
                        }
                        else
                        {
                            pairedRead.InsertLength = 0;
                            if (calculate)
                            {
                                sum -= pairedRead.InsertLength;
                                count--;
                            }

                            pairedRead.Reads.Add(read);
                            pairedRead.PairedType = PairedReadType.MultipleHits;
                        }
                    }
                    else
                    {
                        pairedRead = new PairedRead();
                        if (!string.IsNullOrEmpty(read.RName) && !read.RName.Equals("*"))
                        {
                            pairedRead.Read1 = read;
                        }
                        else
                        {
                            pairedRead.Read2 = read;
                        }

                        pairedRead.PairedType = PairedReadType.Orphan;
                        pairedRead.InsertLength = 0;
                        pairedReads.Add(read.QName, pairedRead);
                    }
                }
            }

            List<PairedRead> allreads = pairedReads.Values.ToList();
            pairedReads = null;
            if (calculate && count > 0)
            {
                UpdateType(allreads, sum, count);
            }


            return allreads;
        }

        /// <summary>
        /// Gets the paired reads when DV is enabled.
        /// </summary>
        /// <param name="meanLengthOfInsert">Mean of the insert length.</param>
        /// <param name="standardDeviationOfInsert">Standard deviation of insert length.</param>
        /// <param name="calculate">If this flag is set then mean and standard deviation will
        /// be calculated from the paired reads instead of specified.</param>
        /// <returns>List of paired read.</returns>
        private IList<PairedRead> GetDVAwarePairedReads(float meanLengthOfInsert, float standardDeviationOfInsert, bool calculate = false)
        {
            // Dictionary helps to get the information at one pass of alinged sequence list.
            Dictionary<string, DVEnabledPairedRead> pairedReads = new Dictionary<string, DVEnabledPairedRead>();
            double sum = 0;
            int count = 0;

            for (int i = 0; i < QuerySequences.Count; i++)
            {
                DVEnabledPairedRead pairedRead;
                SAMAlignedSequence read = QuerySequences[i];
                if ((read.Flag & SAMFlags.PairedRead) == SAMFlags.PairedRead)
                {
                    if (pairedReads.TryGetValue(read.QName, out pairedRead))
                    {
                        if (pairedRead.Index2 == -1 || pairedRead.Index1 == -1)
                        {
                            if (pairedRead.Index2 == -1)
                            {
                                pairedRead.Index2 = i;
                            }
                            else
                            {
                                pairedRead.Index1 = i;
                            }

                            // For best performace,
                            // 1. BAM/SAM file should be sorted by reads name.
                            // 2. If sorted on mapping position then give unmapped read a coordinate (generally the coordinate of the mapped mate)
                            //    for sorting/indexing purposes only.


                            pairedRead.PairedType = PairedRead.GetPairedReadType(pairedRead.Read1, pairedRead.Read2, meanLengthOfInsert, standardDeviationOfInsert);

                            if (pairedRead.PairedType == PairedReadType.Normal || pairedRead.PairedType == PairedReadType.LengthAnomaly)
                            {
                                pairedRead.InsertLength = PairedRead.GetInsertLength(pairedRead.Read1, pairedRead.Read2);

                                if (calculate)
                                {
                                    sum += pairedRead.InsertLength;
                                    count++;
                                }
                            }
                        }
                        else
                        {
                            pairedRead.InsertLength = 0;
                            if (calculate)
                            {
                                sum -= pairedRead.InsertLength;
                                count--;
                            }

                            pairedRead.ReadIndexes.Add(i);
                            pairedRead.PairedType = PairedReadType.MultipleHits;
                        }

                    }
                    else
                    {
                        pairedRead = new DVEnabledPairedRead(QuerySequences);
                        if (!string.IsNullOrEmpty(read.RName) && !read.RName.Equals("*"))
                        {
                            pairedRead.Index1 = i;
                        }
                        else
                        {
                            pairedRead.Index2 = i;
                        }

                        pairedRead.PairedType = PairedReadType.Orphan;
                        pairedRead.InsertLength = 0;
                        pairedReads.Add(read.QName, pairedRead);
                    }
                }
            }

            List<PairedRead> allreads = pairedReads.Values.ToList<PairedRead>();
            pairedReads = null;

            if (calculate && count > 0)
            {
                UpdateType(allreads, sum, count);
            }

            return allreads;
        }
    }

    /// <summary>
    /// DV aware paired read
    /// </summary>
    internal class DVEnabledPairedRead : PairedRead
    {
        /// <summary>
        /// Reference to the list.
        /// </summary>
        private IList<SAMAlignedSequence> _list;

        /// <summary>
        /// Read only list, holds reads list which are paired.
        /// </summary>
        private IList<SAMAlignedSequence> _mappedAlignedSequences;

        /// <summary>
        /// Holds list of read indexes.
        /// </summary>
        private List<int> _readIndexes;

        /// <summary>
        /// Constructor - Creates an instance of DVEnabledPairedRead class.
        /// </summary>
        /// <param name="list">List containing alinged sequences.</param>
        public DVEnabledPairedRead(IList<SAMAlignedSequence> list)
        {
            _list = list;
        }

        /// <summary>
        /// List of read indexes.
        /// </summary>
        public IList<int> ReadIndexes
        {
            get
            {
                if (_readIndexes == null)
                {
                    _readIndexes = new List<int>();
                }

                return _readIndexes;
            }
        }

        /// <summary>
        /// Index of first read in this paired read instance.
        /// </summary>
        public int Index1
        {
            get
            {
                if (_readIndexes == null || _readIndexes.Count == 0)
                {
                    return -1;
                }

                return _readIndexes[0];
            }
            set
            {
                if (_readIndexes != null && _readIndexes.Count > 0)
                {
                    _readIndexes[0] = value;
                }
                else
                {
                    if (value != -1)
                    {
                        ReadIndexes.Add(value);
                    }
                }
            }
        }

        /// <summary>
        /// Index of second read in this paired read instance.
        /// </summary>
        public int Index2
        {
            get
            {
                if (_readIndexes == null || _readIndexes.Count <= 1)
                {
                    return -1;
                }

                return _readIndexes[1];
            }
            set
            {
                if (_readIndexes != null && _readIndexes.Count > 1)
                {
                    _readIndexes[1] = value;
                }
                else
                {
                    if (value != -1)
                    {
                        int count = Reads.Count;
                        if (count == 0)
                        {
                            ReadIndexes.Add(-1);
                        }

                        ReadIndexes.Add(value);
                    }
                }
            }
        }

        /// <summary>
        /// First aligned sequence or read.
        /// </summary>
        public override SAMAlignedSequence Read1
        {
            get
            {
                return _list[Index1];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Second aligned sequence or read.
        /// </summary>
        public override SAMAlignedSequence Read2
        {
            get
            {
                return _list[Index2];
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Gets the list of paired reads.
        /// </summary>
        public override IList<SAMAlignedSequence> Reads
        {
            get
            {
                if (_mappedAlignedSequences == null)
                {
                    _mappedAlignedSequences = new VirtualList<SAMAlignedSequence>(_list, ReadIndexes);
                }

                return _mappedAlignedSequences;
            }
        }
    }

    /// <summary>
    /// Virtual list to navigate through the specified positions
    /// in the source list.
    /// Note that the edit scenarios are not supported.
    /// </summary>
    /// <typeparam name="T">Type of source list item.</typeparam>
    internal class VirtualList<T> : IList<T>
    {
        #region Fields
        /// <summary>
        /// Source list.
        /// </summary>
        private IList<T> _source;

        /// <summary>
        /// Source list indexes.
        /// </summary>
        private IList<int> _sourceIndexes;
        #endregion

        /// <summary>
        /// Creates an instance of Virtual list.
        /// </summary>
        /// <param name="source">Source list.</param>
        /// <param name="indexes">Source indexes to navigate.</param>
        public VirtualList(IList<T> source, IList<int> indexes)
        {
            _source = source;
            _sourceIndexes = indexes;
        }

        /// <summary>
        /// Determines the index of specified item.
        /// </summary>
        /// <param name="item">The instance to locate.</param>
        public int IndexOf(T item)
        {
            int index = _source.IndexOf(item);
            if (_sourceIndexes == null)
            {
                return index;
            }

            return _sourceIndexes.IndexOf(index);
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="index">index.</param>
        /// <param name="item">Item to insert</param>
        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="index">Index</param>
        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets the element at specified index.
        /// </summary>
        /// <param name="index">Index at whi</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                if (_sourceIndexes == null)
                {
                    return _source[index];
                }
                else
                {
                    return _source[_sourceIndexes[index]];
                }
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Determines whether this instance contains specified item or not.
        /// </summary>
        /// <param name="item">Item to be verified.</param>
        /// <returns>Returns true if found, else false.</returns>
        public bool Contains(T item)
        {
            if (this.IndexOf(item) > -1)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Copies the items in this instance to specified array.
        /// </summary>
        /// <param name="array">Array to copy.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            for (int i = 0; i < Count; i++)
            {
                array[i + arrayIndex] = this[i];
            }
        }

        /// <summary>
        /// Gets the count of elements presents in this instance.
        /// </summary>
        public int Count
        {
            get
            {
                if (_sourceIndexes != null)
                {
                    return _sourceIndexes.Count;
                }
                else
                {
                    return _source.Count;
                }
            }
        }

        /// <summary>
        /// Specifies whether this list is readonly or not.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets an enumerator for this instance.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        /// <summary>
        /// Gets an enumerator for this instance.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
