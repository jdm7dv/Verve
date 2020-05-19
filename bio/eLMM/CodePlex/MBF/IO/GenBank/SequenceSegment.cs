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

namespace Bio.IO.GenBank
{
    /// <summary>
    /// Segment provides the information on the order in which this entry appears in a
    /// series of discontinuous sequences from the same molecule.
    /// </summary>
    [Serializable]
    public class SequenceSegment : ICloneable
    {
        #region Properties
        /// <summary>
        /// Current segment number.
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// Total number of segments.
        /// </summary>
        public int Count { get; set; }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Creates a new SequenceSegment that is a copy of the current SequenceSegment.
        /// </summary>
        /// <returns>A new SequenceSegment that is a copy of this SequenceSegment.</returns>
        public SequenceSegment Clone()
        {
            return (SequenceSegment)this.MemberwiseClone();
        }
        #endregion Methods

        #region ICloneable Members
        /// <summary>
        /// Creates a new SequenceSegment that is a copy of the current SequenceSegment.
        /// </summary>
        /// <returns>A new object that is a copy of this SequenceSegment.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion ICloneable Members
    }
}
