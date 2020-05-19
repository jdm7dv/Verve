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
using System.Runtime.Serialization;
using Bio.Properties;
using Bio.Util.Logging;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// A simple implementation of IPairwiseSequenceAlignment that stores the 
    /// results as list of Aligned Sequences 
    /// </summary>
    [Serializable]
    public class PairwiseSequenceAlignment : IPairwiseSequenceAlignment
    {
        #region Fields
        /// <summary>
        /// sequence alignment instance.
        /// </summary>
        private SequenceAlignment _seqAlignment;

        /// <summary>
        /// List of alignments
        /// </summary>
        private List<PairwiseAlignedSequence> _alignedSequences
                = new List<PairwiseAlignedSequence>();

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the PairwiseSequenceAlignment class
        /// Constructs an empty PairwiseSequenceAlignment
        /// </summary>
        public PairwiseSequenceAlignment()
        {
            _seqAlignment = new SequenceAlignment();
            IsReadOnly = false;  // initializes to false by default, but make it explicit for good style
        }

        /// <summary>
        /// Initializes a new instance of the PairwiseSequenceAlignment class
        /// Constructs PairwiseSequenceAlignment with input sequences
        /// </summary>
        /// <param name="firstSequence">First input sequence</param>
        /// <param name="secondSequence">Second input sequence</param>
        public PairwiseSequenceAlignment(ISequence firstSequence, ISequence secondSequence)
        {
            _seqAlignment = new SequenceAlignment();
            _seqAlignment.Sequences.Add(firstSequence);
            _seqAlignment.Sequences.Add(secondSequence);
            _alignedSequences = new List<PairwiseAlignedSequence>();
        }

        /// <summary>
        /// Initializes a new instance of the PairwiseSequenceAlignment class.
        /// Internal constructor to create new instance of PairwiseSequenceAlignment 
        /// from ISequenceAlignment.
        /// </summary>
        /// <param name="seqAlignment">ISequenceAlignment instance.</param>
        internal PairwiseSequenceAlignment(ISequenceAlignment seqAlignment)
        {
            _seqAlignment = new SequenceAlignment(seqAlignment);
            _alignedSequences = new List<PairwiseAlignedSequence>();
            foreach (AlignedSequence alignedSeq in seqAlignment.AlignedSequences)
            {
                _alignedSequences.Add(new PairwiseAlignedSequence(alignedSeq));
            }

            // Clear the AlignedSequences in the _seqAlignment as this no longer needed.
            if (!_seqAlignment.AlignedSequences.IsReadOnly)
            {
                _seqAlignment.AlignedSequences.Clear();
            }
        }
        #endregion

        #region ISequenceAlignment members
        /// <summary>
        /// Gets any additional information about the Alignment.
        /// </summary>
        public Dictionary<string, object> Metadata
        {
            get
            {
                return _seqAlignment.Metadata;
            }
        }

        /// <summary>
        /// Gets list of the (output) aligned sequences.
        /// Note that it always returns readonly list. To add aligned sequence use AddSequence method.
        /// </summary>
        public IList<IAlignedSequence> AlignedSequences
        {
            get
            {
                // get all IPairwiseAlignedSequence as IAlignedSequence.
                // Return as readonly collection to avoid any modification to the collection by user.
                return _alignedSequences.ConvertAll(PAS => PAS as IAlignedSequence).AsReadOnly();
            }
        }

        /// <summary>
        /// Gets list of sequences involved in this alignment.
        /// </summary>
        public IList<ISequence> Sequences
        {
            get
            {
                return _seqAlignment.Sequences;
            }
        }

        #endregion

        #region IPairwiseSequenceAlignment members

        /// <summary>
        /// Gets the list of alignments.
        /// </summary>
        public IList<PairwiseAlignedSequence> PairwiseAlignedSequences
        {
            get { return _alignedSequences; }
        }

        /// <summary>
        /// Gets accessor for the first sequence
        /// </summary>
        public ISequence FirstSequence
        {
            get
            {
                if (_seqAlignment.Sequences.Count == 0)
                {
                    return null;
                }

                return _seqAlignment.Sequences[0];
            }
        }

        /// <summary>
        /// Gets accessor for the second sequence
        /// </summary>
        public ISequence SecondSequence
        {
            get
            {
                if (_seqAlignment.Sequences.Count <= 1)
                {
                    return null;
                }

                return _seqAlignment.Sequences[1];
            }
        }

        /// <summary>
        /// Returns the ith aligned sequence in the alignment.
        /// </summary>
        /// <param name="i">The index.</param>
        /// <returns>The aligned sequence.</returns>
        public PairwiseAlignedSequence this[int i]
        {
            get
            {
                return _alignedSequences[i];
            }
        }

        /// <summary>
        /// Gets or sets Documentation object is intended for tracking the history, provenance,
        /// and experimental context of a PairwiseSequenceAlignment. The user can adopt any desired
        /// convention for use of this object.
        /// </summary>
        public object Documentation { get; set; }

        /// <summary>
        /// Add a new Aligned Sequence Object to the end of the list.
        /// </summary>
        /// <param name="pairwiseAlignedSequence">The sequence to add.</param>
        public void AddSequence(PairwiseAlignedSequence pairwiseAlignedSequence)
        {
            if (IsReadOnly)
            {
                Trace.Report(Resource.READ_ONLY_COLLECTION_MESSAGE);
                throw new NotSupportedException(Resource.READ_ONLY_COLLECTION_MESSAGE);
            }

            _alignedSequences.Add(pairwiseAlignedSequence);
        }

        #endregion

        #region ICollection<ISequence> Members

        /// <summary>
        /// Gets number of aligned sequence objects in the PairwiseSequenceAlignment.
        /// </summary>
        public int Count
        {
            get
            {
                return _alignedSequences.Count;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether PairwiseSequenceAlignment is read-only or not.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Adds an aligned sequence to the list of aligned sequences in the PairwiseSequenceAlignment.
        /// Throws exception if sequence alignment is read only.
        /// </summary>
        /// <param name="item">PairwiseAlignedSequence to add.</param>
        public void Add(PairwiseAlignedSequence item)
        {
            if (IsReadOnly)
            {
                Trace.Report(Resource.READ_ONLY_COLLECTION_MESSAGE);
                throw new NotSupportedException(Resource.READ_ONLY_COLLECTION_MESSAGE);
            }

            _alignedSequences.Add(item);
        }

        /// <summary>
        /// Clears the PairwiseSequenceAlignment
        /// Throws exception if PairwiseSequenceAlignment is read only.
        /// </summary>
        public void Clear()
        {
            if (IsReadOnly)
            {
                Trace.Report(Resource.READ_ONLY_COLLECTION_MESSAGE);
                throw new NotSupportedException(Resource.READ_ONLY_COLLECTION_MESSAGE);
            }

            _alignedSequences.Clear();
        }

        /// <summary>
        /// Returns true if the PairwiseSequenceAlignment contains the aligned sequence in the
        /// list of aligned sequences.
        /// </summary>
        /// <param name="item">PairwiseAlignedSequence object</param>
        /// <returns>True if contains item, otherwise returns false.</returns>
        public bool Contains(PairwiseAlignedSequence item)
        {
            return _alignedSequences.Contains(item);
        }

        /// <summary>
        /// Copies the aligned sequences from the PairwiseSequenceAlignment into an existing aligned sequence array.
        /// </summary>
        /// <param name="array">Array into which to copy the sequences.</param>
        /// <param name="arrayIndex">Starting index in array at which to begin the copy.</param>
        public void CopyTo(PairwiseAlignedSequence[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameArray);
            }

            foreach (PairwiseAlignedSequence seq in _alignedSequences)
            {
                array[arrayIndex++] = seq;
            }
        }

        /// <summary>
        /// Removes item from the list of aligned sequences in the PairwiseSequenceAlignment.
        /// Throws exception if PairwiseSequenceAlignment is read only.
        /// </summary>
        /// <param name="item">Aligned sequence object</param>
        /// <returns>True if item was removed, false if item was not found.</returns>
        public bool Remove(PairwiseAlignedSequence item)
        {
            if (IsReadOnly)
            {
                Trace.Report(Resource.READ_ONLY_COLLECTION_MESSAGE);
                throw new NotSupportedException(Resource.READ_ONLY_COLLECTION_MESSAGE);
            }

            return _alignedSequences.Remove(item);
        }

        #endregion

        #region ISerializable Members
        /// <summary>
        /// Initializes a new instance of the PairwiseSequenceAlignment class
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected PairwiseSequenceAlignment(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _alignedSequences = (List<PairwiseAlignedSequence>)info.GetValue("AlignedSeqs", typeof(List<PairwiseAlignedSequence>));
            _seqAlignment = (SequenceAlignment)info.GetValue("base", typeof(SequenceAlignment));
            Documentation = info.GetValue("Doc", typeof(object));
            IsReadOnly = info.GetBoolean("IsReadOnly");
        }
        
        /// <summary>
        /// Method for serializing the PairwiseSequenceAlignment.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("AlignedSeqs", _alignedSequences);
            info.AddValue("base", _seqAlignment);
            info.AddValue("IsReadOnly", IsReadOnly);

            if (Documentation != null && ((Documentation.GetType().Attributes &
                System.Reflection.TypeAttributes.Serializable) == System.Reflection.TypeAttributes.Serializable))
            {
                info.AddValue("Doc", Documentation);
            }
            else
            {
                info.AddValue("Doc", null);
            }
        }

        #endregion

        #region IEnumerable<ISequence> Members

        /// <summary>
        /// Returns an enumerator for the aligned sequences in the PairwiseSequenceAlignment.
        /// </summary>
        /// <returns>Returns the enumerator for PairwiseAlignedSequence</returns>
        public IEnumerator<PairwiseAlignedSequence> GetEnumerator()
        {
            return _alignedSequences.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator for the aligned sequences in the PairwiseSequenceAlignment.
        /// </summary>
        /// <returns>Returns the enumerator for PairwiseAlignedSequence</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _alignedSequences.GetEnumerator();
        }

        #endregion
    }
}
