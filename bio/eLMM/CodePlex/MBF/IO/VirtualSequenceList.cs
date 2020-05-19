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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Bio.IO
{
    /// <summary>
    /// A VirtualSequenceList is used to store huge list of sequences where all items in the
    /// list might not be held in memory always. This list will get the item from cache or
    /// a virtual data provider when requested. Example, FastA file has more than one 
    /// sequence and Data Virtualization returns this class and then, on demand 
    /// each sequences are loaded from the FastA file using parser.
    /// </summary>
    public class VirtualSequenceList : IVirtualSequenceList
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

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the VirtualSequenceList class with a specified provider,
        /// a specified parser, and a specifed sequence count.
        /// </summary>
        /// <param name="provider">SequencePointer provider from sidecar file.</param>
        /// <param name="parser">Parser used to parse sequence data on request.</param>
        /// <param name="count">Number of items in the actual file.</param>
        public VirtualSequenceList(SidecarFileProvider provider, IVirtualSequenceParser parser, int count)
        {
            _sequenceParser = parser;
            _sidecarProvider = provider;
            _count = count;
            _sequenceDictionary = new Dictionary<int, WeakReference>(_count);
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

        /// <summary>
        /// Returns the index of the first sequence matching the sequence
        /// passed in to the parameter. This does not do a value-based
        /// comparison. The match must be the exact same ISequence.
        /// </summary>
        /// <returns>the zero-based index of the sequence if found; otherwise, -1</returns>
        public int IndexOf(ISequence item)
        {
            foreach (var entry in _sequenceDictionary)
            {
                if (ReferenceEquals(item, entry.Value.Target))
                {
                    return entry.Key;
                }
            }

            return -1;
        }

        /// <summary>
        /// This method is not supported since VirtualSequenceList is read-only.
        /// </summary>
        public void Insert(int index, ISequence item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// This method is not supported since VirtualSequenceList is read-only.
        /// </summary>
        public void RemoveAt(int index)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Gets the index of a sequence within the list.
        /// Throws a NotSupportedException when attempting to set the position
        /// since VirtualSequenceList is read-only.
        /// </summary>
        /// <param name="index">The zero-based index of the sequence in the list.</param>
        /// <returns>The sequence found at the specified index.</returns>
        public ISequence this[int index]
        {
            get
            {
                Sequence virtualSequence;

                if (_sequenceDictionary.ContainsKey(index))
                {
                    virtualSequence = _sequenceDictionary[index].Target as Sequence;
                    if (virtualSequence != null)
                    {
                        return virtualSequence;
                    }
                    _sequenceDictionary.Remove(index);
                }

                SequencePointer pointer = _sidecarProvider[index];

                // Get the alphabet from alphabet name.
                IAlphabet alphabet = Alphabets.All.Single(A => A.Name.Equals(pointer.AlphabetName));

                virtualSequence = new Sequence(alphabet)
                                      {
                                          ID = (_sequenceParser as Fasta.FastaParser).GetSequenceID(pointer),
                                          VirtualSequenceProvider =
                                              new FileVirtualSequenceProvider(_sequenceParser, pointer)
                                      };
                if (pointer.EndingIndex - pointer.StartingIndex < virtualSequence.VirtualSequenceProvider.BlockSize)
                {
                    virtualSequence.VirtualSequenceProvider.BlockSize = (int)(pointer.EndingIndex - pointer.StartingIndex);
                }

                _sequenceDictionary.Add(index, new WeakReference(virtualSequence, false));

                return virtualSequence;
            }
            set
            {
                throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
            }
        }

        #endregion

        #region ICollection<ISequence> Members

        /// <summary>
        /// This method is not supported since VirtualSequenceList is read-only.
        /// </summary>
        public void Add(ISequence item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// This method is not supported since VirtualSequenceList is read-only.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        /// <summary>
        /// Determines whether a specific sequence is in the virtual sequence list.
        /// </summary>
        /// <param name="item">The sequence to locate in the list.</param>
        /// <returns>true if the sequence is found in the list; otherwise, false</returns>
        public bool Contains(ISequence item)
        {
            return IndexOf(item) >= 0;
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
        public void CopyTo(ISequence[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameArray);
            }

            int index = arrayIndex;
            foreach (ISequence seq in this)
            {
                array[index++] = seq;
            }
        }

        /// <summary>
        /// Gets the number of sequences in the list.
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Gets the read-only status of the list.
        /// </summary>
        public bool IsReadOnly
        {
            get { return true; }
        }

        /// <summary>
        /// This method is not supported since VirtualSequenceList is read-only.
        /// </summary>
        public bool Remove(ISequence item)
        {
            throw new NotSupportedException(Properties.Resource.NotSupportedInVirtualSequence);
        }

        #endregion

        #region IEnumerable<ISequence> Members
        /// <summary>
        /// Get the enumerator to the sequences in the list.
        /// </summary>
        /// <returns>The enumerator to the sequences in the list.</returns>
        public IEnumerator<ISequence> GetEnumerator()
        {
            return new VirtualSequenceEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Get the enumerator to the sequences in the list.
        /// </summary>
        /// <returns>The enumerator to the sequences in the list.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new VirtualSequenceEnumerator(this);
        }

        #endregion
    }

    /// <summary>
    /// Implementation of the enumerator for the VirtualSequenceList.
    /// </summary>
    internal class VirtualSequenceEnumerator : IEnumerator<ISequence>
    {
        #region Fields
        /// <summary>
        /// A list of sequences.
        /// </summary>
        private readonly IList<ISequence> _sequences;

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
        /// Initializes an enumerator for the VirtualSequenceList.
        /// </summary>
        /// <param name="virtualSequenceList"></param>
        public VirtualSequenceEnumerator(IList<ISequence> virtualSequenceList)
        {
            _sequences = virtualSequenceList;
            Reset();
        }
        #endregion

        #region IEnumerator<ISequence> Members

        /// <summary>
        /// The current item reference for the enumerator.
        /// </summary>
        public ISequence Current
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
        /// Resets the enumerator to the start of the sequence.
        /// </summary>
        public void Reset()
        {
            _index = -1;
        }

        #endregion
    }
}
