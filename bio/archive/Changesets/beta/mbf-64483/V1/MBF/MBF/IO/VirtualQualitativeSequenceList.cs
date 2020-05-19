// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MBF.IO.FastQ;

namespace MBF.IO
{
    /// <summary>
    /// A VirtualQualitativeSequenceList is used in data virtualization scenarios to maintain a
    /// large list of sequences. When all the available sequences cannot be accommodated at once
    /// in memory, this list fetches blocks of sequences from the cache or from the file on request.
    /// For example, if a FastA file has more than one sequence and data virtualization returns this class, then
    /// each sequence is loaded from the FastA file when it is first accessed.
    /// </summary>
    public class VirtualQualitativeSequenceList : IList<ISequence>, IList<IQualitativeSequence>
    {
        #region Fields
        /// <summary>
        /// Number of items in the actual file
        /// </summary>
        private readonly int _count;

        /// <summary>
        /// SequencePointer provider from sidecar file
        /// </summary>
        private readonly SidecarFileProvider _sidecarProvider;

        /// <summary>
        /// Parser used to parse sequence data on request
        /// </summary>
        private readonly IVirtualSequenceParser _sequenceParser;

        /// <summary>
        /// Contains the index of the sequence in the actual file, and a weak reference to that sequence.
        /// </summary>
        private readonly Dictionary<int, WeakReference> _sequenceDictionary;

        /// <summary>
        /// 1 KB
        /// </summary>
        private const int KBytes = 1024;

        /// <summary>
        /// The maximum allowable number of items in the dictionary before the weak
        /// references are forcibly removed to facilitate optimal use of available memory.
        /// </summary>
        private const int MaximumDictionaryLength = 100 * KBytes;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the VirtualQualitativeSequenceList class with a specified provider,
        /// a specified parser, and a specifed sequence count.
        /// </summary>
        /// <param name="provider">SequencePointer provider from sidecar file.</param>
        /// <param name="parser">Parser used to parse sequence data on request.</param>
        /// <param name="count">Number of items in the actual file.</param>
        public VirtualQualitativeSequenceList(SidecarFileProvider provider, IVirtualSequenceParser parser, int count)
        {
            _sequenceParser = parser;
            _sidecarProvider = provider;
            _count = count;
            _sequenceDictionary = new Dictionary<int, WeakReference>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value indicating whether to set the sequence to read-only
        /// when reading from the sidecar file.
        /// </summary>
        public bool CreateSequenceAsReadOnly { get; set; }
        #endregion

        #region IList<ISequence> Members

        int IList<ISequence>.IndexOf(ISequence item)
        {
            return IndexOf(item as IQualitativeSequence);
        }

        void IList<ISequence>.Insert(int index, ISequence item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        void IList<ISequence>.RemoveAt(int index)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        ISequence IList<ISequence>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (IQualitativeSequence)value;
            }
        }

        #endregion

        #region ICollection<ISequence> Members
        /// <summary>
        /// This method is not supported since VirtualSequenceList is read-only.
        /// </summary>
        void ICollection<ISequence>.Add(ISequence item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// This method is not supported since VirtualSequenceList is read-only.
        /// </summary>
        void ICollection<ISequence>.Clear()
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Determines whether a specific sequence is in the virtual sequence list.
        /// </summary>
        /// <param name="item">The sequence to locate in the list.</param>
        /// <returns>true if the sequence is found in the list; otherwise, false</returns>
        bool ICollection<ISequence>.Contains(ISequence item)
        {
            return Contains(item as IQualitativeSequence);
        }

        /// <summary>
        /// Copies the entire virtual sequence list to a compatible one-dimensional array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements
        /// copied from the current list. The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        void ICollection<ISequence>.CopyTo(ISequence[] array, int arrayIndex)
        {
            CopyTo(array as IQualitativeSequence[], arrayIndex);
        }

        /// <summary>
        /// Gets the number of sequences in the list.
        /// </summary>
        int ICollection<ISequence>.Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Gets the read-only status of the list.
        /// </summary>
        bool ICollection<ISequence>.IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// This method is not supported since VirtualSequenceList is read-only.
        /// </summary>
        bool ICollection<ISequence>.Remove(ISequence item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }
        #endregion

        #region IEnumerable<ISequence> Members
        /// <summary>
        /// Get the enumerator to the sequences in the list.
        /// </summary>
        /// <returns>The enumerator to the sequences in the list.</returns>
        IEnumerator<ISequence> IEnumerable<ISequence>.GetEnumerator()
        {
            return new VirtualQualitativeSequenceEnumerator(this);
        }
        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Get the enumerator to the qualitative sequences in the list.
        /// </summary>
        /// <returns>The enumerator to the qualitative sequences in the list.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new VirtualQualitativeSequenceEnumerator(this);
        }
        #endregion

        #region IList<IQualitativeSequence> Members
        /// <summary>
        /// Returns the index of the first sequence matching the sequence
        /// passed in to the parameter. This does not do a value-based
        /// comparison. The match must be the exact same IQualitativeSequence.
        /// </summary>
        /// <returns>the zero-based index of the sequence if found; otherwise, -1</returns>
        public int IndexOf(IQualitativeSequence item)
        {
            if (item != null)
            {
                foreach (var entry in _sequenceDictionary)
                {
                    if (ReferenceEquals(item, entry.Value.Target))
                    {
                        return entry.Key;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// This method is not supported since VirtualSequenceList is read-only.
        /// </summary>
        public void Insert(int index, IQualitativeSequence item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// This method is not supported since VirtualSequenceList is read-only.
        /// </summary>
        void IList<IQualitativeSequence>.RemoveAt(int index)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Gets the index of a qualitative sequence within the list.
        /// Throws a NotSupportedException when attempting to set the position
        /// since VirtualSequenceList is read-only.
        /// </summary>
        /// <param name="index">The zero-based index of the qualitative sequence in the list.</param>
        /// <returns>The qualitative sequence found at the specified index.</returns>
        public IQualitativeSequence this[int index]
        {
            get
            {
                QualitativeSequence virtualQualitativeSequence;

                if (_sequenceDictionary.ContainsKey(index))
                {
                    virtualQualitativeSequence = _sequenceDictionary[index].Target as QualitativeSequence;
                    if (virtualQualitativeSequence != null)
                    {
                        return virtualQualitativeSequence;
                    }
                    _sequenceDictionary.Remove(index);
                }

                SequencePointer pointer = _sidecarProvider[index];

                // Get the alphabet from alphabet name.
                IAlphabet alphabet = Alphabets.All.Single(A => A.Name.Equals(pointer.AlphabetName));

                virtualQualitativeSequence = new QualitativeSequence(alphabet)
                {
                    ID = ((FastQParser) _sequenceParser).GetSequenceID(pointer),
                    VirtualQualitativeSequenceProvider =
                        new FileVirtualQualitativeSequenceProvider(_sequenceParser, pointer)
                };
                if (pointer.IndexOffsets[1] - pointer.IndexOffsets[0] < virtualQualitativeSequence.VirtualQualitativeSequenceProvider.BlockSize)
                {
                    virtualQualitativeSequence.VirtualQualitativeSequenceProvider.BlockSize = (int)(pointer.IndexOffsets[1] - pointer.IndexOffsets[0]);
                }

                virtualQualitativeSequence.IsReadOnly = CreateSequenceAsReadOnly;

                // memory opt : cleanup the weak reference dictionary
                if (_sequenceDictionary.Count > 0 && (_sequenceDictionary.Count % MaximumDictionaryLength) == 0)
                {
                    foreach (int key in _sequenceDictionary.Keys.Where(K => _sequenceDictionary[K].IsAlive == false).ToList())
                    {
                        _sequenceDictionary.Remove(key);
                    }
                }

                _sequenceDictionary.Add(index, new WeakReference(virtualQualitativeSequence, false));
                
                return virtualQualitativeSequence;
            }
            set
            {
                throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
            }
        }        
        #endregion

        #region ICollection<IQualitativeSequence> Members
        /// <summary>
        /// This method is not supported since VirtualQualitativeSequenceList is read-only.
        /// </summary>
        public void Add(IQualitativeSequence item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// This method is not supported since VirtualQualitativeSequenceList is read-only.
        /// </summary>
        void ICollection<IQualitativeSequence>.Clear()
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Determines whether a specific qualitative sequence is in the virtual qualitative sequence list.
        /// </summary>
        /// <param name="item">The qualitative sequence to locate in the list.</param>
        /// <returns>true if the qualitative sequence is found in the list; otherwise, false</returns>
        public bool Contains(IQualitativeSequence item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Copies the entire virtual qualitative sequence list to a compatible one-dimensional array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements
        /// copied from the current list. The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(IQualitativeSequence[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameArray);
            }

            int index = arrayIndex;
            foreach (IQualitativeSequence seq in this)
            {
                array[index++] = seq;
            }
        }

        /// <summary>
        /// Gets the number of qualitative sequences in the list.
        /// </summary>
        int ICollection<IQualitativeSequence>.Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Gets the read-only status of the list.
        /// </summary>
        bool ICollection<IQualitativeSequence>.IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// This method is not supported since VirtualQualitativeSequenceList is read-only.
        /// </summary>
        public bool Remove(IQualitativeSequence item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        #endregion

        #region IEnumerable<IQualitativeSequence> Members

        /// <summary>
        /// Get the enumerator to the qualitative sequences in the list.
        /// </summary>
        /// <returns>The enumerator to the qualitative sequences in the list.</returns>
        public IEnumerator<IQualitativeSequence> GetEnumerator()
        {
            return new VirtualQualitativeSequenceEnumerator(this);
        }
        #endregion
    }

    /// <summary>
    /// Implementation of the enumerator for the VirtualQualitativeSequenceList.
    /// </summary>
    internal class VirtualQualitativeSequenceEnumerator : IEnumerator<IQualitativeSequence>
    {
        #region Fields
        /// <summary>
        /// A list of qualitative sequences.
        /// </summary>
        private readonly IList<IQualitativeSequence> _sequences;

        /// <summary>
        /// The zero-based index of the sequence in the list.
        /// </summary>
        private int _index;

        /// <summary>
        /// Track whether disposed has been called.
        /// </summary>
        private bool _disposed;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes an enumerator for the VirtualQualitativeSequenceList.
        /// </summary>
        /// <param name="virtualSequenceList"></param>
        public VirtualQualitativeSequenceEnumerator(IList<IQualitativeSequence> virtualSequenceList)
        {
            _sequences = virtualSequenceList;
            Reset();
        }
        #endregion

        #region IEnumerator<ISequence> Members

        /// <summary>
        /// The current item reference for the enumerator.
        /// </summary>
        public IQualitativeSequence Current
        {
            get
            {
                if (_index < 0)
                {
                    return null;
                }

                return _sequences[_index];
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes of any allocated memory.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of any allocated memory.
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // No op
                }

                _disposed = true;
            }
        }



        #endregion

        #region IEnumerator Members
        /// <summary>
        /// The current item reference for the enumerator.
        /// </summary>
        object IEnumerator.Current
        {
            get
            {
                return _sequences[_index];
            }
        }

        /// <summary>
        /// Advances the enumerator to the next item.
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            if (_index < (_sequences.Count - 1))
            {
                _index++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets the enumerator to the start of the qualitative sequence.
        /// </summary>
        public void Reset()
        {
            _index = -1;
        }

        #endregion
    }
}
