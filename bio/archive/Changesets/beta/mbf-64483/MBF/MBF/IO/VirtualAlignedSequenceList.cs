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
using MBF.Algorithms.Alignment;

namespace MBF.IO
{
    /// <summary>
    /// A VirtualAlignedSequenceList is used in data virtualization scenarios to maintain a
    /// large list of aligned sequences. When all the available sequences cannot be accommodated
    /// at once in memory, this list fetches blocks of sequence alignments from the cache or
    /// from the file on request.
    /// For example, if a SAM file has more than one sequence and data virtualization returns this class, then
    /// each sequence is loaded from the SAM file when it is first accessed.
    /// </summary>
    public class VirtualAlignedSequenceList<T> : IVirtualAlignedSequenceList<T> where T:IAlignedSequence
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
        private readonly IVirtualSequenceAlignmentParser _sequenceParser;

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
        /// Initializes a new instance of the VirtualAlignedSequenceList class with a specified provider,
        /// a specified parser, and a specifed sequence count.
        /// </summary>
        /// <param name="provider">SequencePointer provider from sidecar file.</param>
        /// <param name="parser">Parser used to parse sequence data on request.</param>
        /// <param name="count">Number of items in the actual file.</param>
        public VirtualAlignedSequenceList(SidecarFileProvider provider, IVirtualSequenceAlignmentParser parser, int count)
        {
            _sequenceParser = parser;
            _sidecarProvider = provider;
            _count = count;
            _sequenceDictionary = new Dictionary<int, WeakReference>();
        }
        #endregion

        #region IList<IAlignedSequence> Members

        /// <summary>
        ///  Returns the zero-based index of the first occurrence of the specific item in the VirtualAlignedSequenceList.
        /// </summary>
        /// <param name="item">The object to locate in the list.</param>
        /// <returns>The index of the item if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item)
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
        /// This method is not supported since VirtualAlignedSequenceList is read-only.
        /// </summary>
        public void Insert(int index, T item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// This method is not supported since VirtualAlignedSequenceList is read-only.
        /// </summary>
        public void RemoveAt(int index)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// Throws a NotSupportedException when attempting to set the position
        /// since VirtualAlignedSequenceList is read-only.
        /// </summary>
        /// <param name="index">The zero-based index of the sequence in the list.</param>
        /// <returns>The aligned sequence at the specified index.</returns>
        public T this[int index]
        {
            get
            {
                IAlignedSequence virtualAlignedSequence;
                
                if(_sequenceDictionary.ContainsKey(index))
                {
                    virtualAlignedSequence = _sequenceDictionary[index].Target as IAlignedSequence;
                    if (virtualAlignedSequence != null)
                    {
                        return (T)virtualAlignedSequence;
                    }
                    _sequenceDictionary.Remove(index);
                }
                
                SequencePointer sequencePointer = _sidecarProvider[index];

                virtualAlignedSequence = _sequenceParser.ParseAlignedSequence(sequencePointer);

                // memory opt : cleanup the weak reference dictionary
                if (_sequenceDictionary.Count > 0 && (_sequenceDictionary.Count % MaximumDictionaryLength) == 0)
                {
                    foreach (int key in _sequenceDictionary.Keys.Where(K => _sequenceDictionary[K].IsAlive == false).ToList())
                    {
                        _sequenceDictionary.Remove(key);
                    }
                }
                
                _sequenceDictionary.Add(index, new WeakReference(virtualAlignedSequence, false));

                return (T)virtualAlignedSequence;
            }
            set
            {
                throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
            }
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// This method is not supported since VirtualAlignedSequenceList is read-only.
        /// </summary>
        public void Add(T item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// This method is not supported since VirtualAlignedSequenceList is read-only.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Determines whether a specific aligned sequence is in the virtual sequence list.
        /// </summary>
        /// <param name="item">The aligned sequence to locate in the list.</param>
        /// <returns>true if the aligned sequence is found in the list; otherwise, false</returns>
        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Copies the entire virtual aligned sequence list to a compatible one-dimensional array, 
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements
        /// copied from the current list. The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in the array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameArray);
            }

            int index = arrayIndex;
            foreach (T seq in this)
            {
                array[index++] = seq;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the VirtualAlignedSequenceList.
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Gets a value indicating whether the VirtualAlignedSequenceList is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// This method is not supported since VirtualAlignedSequenceList is read-only.
        /// </summary>
        public bool Remove(T item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        #endregion

        #region IEnumerable<IAlignedSequence> Members

        /// <summary>
        /// Get the enumerator to the aligned sequences in the list.
        /// </summary>
        /// <returns>The enumerator to the aligned sequences in the list.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new VirtualAlignedSequenceEnumerator<T>(this);
        }

        #endregion

        #region IEnumerable Members
        /// <summary>
        /// Get the enumerator to the aligned sequences in the list.
        /// </summary>
        /// <returns>The enumerator to the aligned sequences in the list.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new VirtualAlignedSequenceEnumerator<T>(this);
        }

        #endregion

        /// <summary>
        /// Implementation of the enumerator for the VirtualAlignedSequenceList.
        /// </summary>
        internal class VirtualAlignedSequenceEnumerator<U> : IEnumerator<U>
        {
            #region Fields
            /// <summary>
            /// A list of sequences.
            /// </summary>
            private readonly IList<U> _alignedSequences;

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
            /// Initializes an enumerator for the VirtualAlignedSequenceEnumerator.
            /// </summary>
            /// <param name="virtualAlignedSequenceList"></param>
            public VirtualAlignedSequenceEnumerator(IList<U> virtualAlignedSequenceList)
            {
                _alignedSequences = virtualAlignedSequenceList;
                Reset();
            }

            #endregion

            #region IEnumerator<T> Members

            /// <summary>
            /// The current item reference for the enumerator.
            /// </summary>
            public U Current
            {
                get
                {
                    if (_index < 0)
                    {
                        return default(U);
                    }

                    return _alignedSequences[_index];
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
            /// <param name="disposing">Indicates whether to dispose of all resources or only unmanaged ones.</param>
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
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return _alignedSequences[_index];
                }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; 
            /// false if the enumerator has passed the end of the collection.
            /// </returns>
            public bool MoveNext()
            {
                if (_index < (_alignedSequences.Count - 1))
                {
                    _index++;
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element
            /// in the collection.
            /// </summary>
            public void Reset()
            {
                _index = -1;
            }

            #endregion
        }
    }
}
