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

namespace Bio
{
    /// <summary>
    /// A SequenceRange holds the data necessary to represent a region within
    /// a sequence defined by its start and end index without necessarily holding
    /// any of the sequence item data. At a minimum and ID, start index, and end
    /// index are required. Additional metadata can be stored as well using a
    /// generic key value pair.
    /// </summary>
    public class SequenceRange : ISequenceRange
    {
        private long _start = 0;
        private long _end = long.MaxValue;
        private string _id = String.Empty;
        private Dictionary<string, object> _metadata = null;

        private List<ISequenceRange> _parentSeqRanges = null;
        /// <summary>
        /// Default constructor that does not set any fields.
        /// </summary>
        public SequenceRange() { }

        /// <summary>
        /// Data constructor that sets the most commonly used fields.
        /// Note that if the end value is less than start value then the end values is assigned to the start value.
        /// </summary>
        /// <param name="ID">An ID for the range. This does not need to be unique, and often represents the chromosome of the range.</param>
        /// <param name="start">A starting index for the range. In the BED format this index starts counting from 0.</param>
        /// <param name="end">An ending index for the range. In the BED format this index is exclusive.</param>
        public SequenceRange(string ID, long start, long end)
        {
            _id = ID;
            Start = start;
            End = end;
        }

        /// <summary>
        /// The beginning index of the range. This index must be non-negative and
        /// it will be enforced to always be less than or equal to the End index.
        /// </summary>
        public long Start
        {
            get { return _start; }
            set
            {
                if (value < 0)
                    throw new IndexOutOfRangeException(Properties.Resource.SequenceRangeNonNegative);

                if (value > _end)
                    throw new IndexOutOfRangeException(Properties.Resource.SequenceRangeStartError);
                _start = value;
            }
        }

        /// <summary>
        /// The end index of the range. This index must be non-negative and
        /// it will be enforced to always be greater than or equal to the Start index.
        /// </summary>
        public long End
        {
            get { return _end; }
            set
            {
                if (value < 0)
                    throw new IndexOutOfRangeException(Properties.Resource.SequenceRangeNonNegative);

                if (value < _start)
                    throw new IndexOutOfRangeException(Properties.Resource.SequenceRangeEndError);
                _end = value;
            }
        }

        /// <summary>
        /// A string identifier of the sequence range.
        /// </summary>
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// The length of the range, which can be zero. This result is the
        /// difference of the End and Start index.
        /// </summary>
        public long Length
        {
            get { return End - Start; }
        }

        /// <summary>
        /// Optional additional data to store along with the ID and indices of
        /// the range. Metadata must be stored with a string key name.
        /// </summary>
        public Dictionary<string, object> Metadata
        {
            get
            {
                if (_metadata == null)
                    _metadata = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);

                return _metadata;
            }
        }

        /// <summary>
        /// Gets the sequence ranges from which this sequence range is obtained.
        /// This property will be filled by the operations like Merge, Intersect etc.
        /// </summary>
        public List<ISequenceRange> ParentSeqRanges 
        {
            get
            {
                if (_parentSeqRanges == null)
                    _parentSeqRanges = new List<ISequenceRange>();

                return _parentSeqRanges;
            }
        } 

        #region IComparable Members
        /// <summary>
        /// Compares two sequence ranges.
        /// </summary>
        /// <param name="obj">SequenceRange instance to compare.</param>
        /// <returns>
        /// If the Start values of the two ranges are identical then the
        /// result of this comparison is the result from calling CompareTo() on
        /// the two End values. If the Start values are not equal then the result
        /// of this comparison is the result of calling CompareTo() on the two
        /// Start values.
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 0;
            if (!(obj is SequenceRange))
                return 0;
            return CompareTo(obj as SequenceRange);
        }

        #endregion

        #region IComparable<ISequenceRange> Members
        /// <summary>
        /// Compares two sequence ranges.
        /// </summary>
        /// <param name="other">SequenceRange instance to compare.</param>
        /// <returns>
        /// If the Start values of the two ranges are identical then the
        /// result of this comparison is the result from calling CompareTo() on
        /// the two End values. If the Start values are not equal then the result
        /// of this comparison is the result of calling CompareTo() on the two
        /// Start values.
        /// </returns>
        public int CompareTo(ISequenceRange other)
        {
            int compare = Start.CompareTo(other.Start);

            if (compare == 0)
                compare = End.CompareTo(other.End);

            if (compare == 0)
                compare = string.Compare(ID, other.ID, StringComparison.OrdinalIgnoreCase);

            if (compare == 0)
            {
                compare = ParentSeqRanges.Count.CompareTo(other.ParentSeqRanges.Count);

                if (compare == 0)
                {
                    for (int index = 0; index < ParentSeqRanges.Count; index++)
                    {
                        compare = ParentSeqRanges[index].CompareTo(other.ParentSeqRanges[index]);
                        if (compare != 0)
                            break;
                    }
                }
            }

            return compare;
        }

        #endregion

        /// <summary>
        /// Overrides hash function for a particular type.
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Overrides the equal method
        /// </summary>
        /// <param name="obj">Object to be checked</param>
        /// <returns>Is equals</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Override equal operator
        /// </summary>
        /// <param name="leftHandSideObject">LHS object</param>
        /// <param name="rightHandSideObject">RHS object</param>
        /// <returns>Is LHS == RHS</returns>
        public static bool operator ==(SequenceRange leftHandSideObject, SequenceRange rightHandSideObject)
        {
            if (System.Object.ReferenceEquals(leftHandSideObject, rightHandSideObject))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Override not equal operator
        /// </summary>
        /// <param name="leftHandSideObject">LHS object</param>
        /// <param name="rightHandSideObject">RHS object</param>
        /// <returns>Is LHS == RHS</returns>
        public static bool operator !=(SequenceRange leftHandSideObject, SequenceRange rightHandSideObject)
        {
            return !(leftHandSideObject == rightHandSideObject);
        }

        /// <summary>
        /// Override less than operator
        /// </summary>
        /// <param name="leftHandSideObject">LHS object</param>
        /// <param name="rightHandSideObject">RHS object</param>
        /// <returns>Is LHS == RHS</returns>
        public static bool operator <(SequenceRange leftHandSideObject, SequenceRange rightHandSideObject)
        {
            if (object.ReferenceEquals(leftHandSideObject, null) || object.ReferenceEquals(rightHandSideObject, null))
            {
                return false;
            }

            return (leftHandSideObject.CompareTo(rightHandSideObject) < 0);
        }

        /// <summary>
        /// Override greater than operator
        /// </summary>
        /// <param name="leftHandSideObject">LHS object</param>
        /// <param name="rightHandSideObject">RHS object</param>
        /// <returns>Is LHS == RHS</returns>
        public static bool operator >(SequenceRange leftHandSideObject, SequenceRange rightHandSideObject)
        {
            if (object.ReferenceEquals(leftHandSideObject, null) || object.ReferenceEquals(rightHandSideObject, null))
            {
                return false;
            }

            return (leftHandSideObject.CompareTo(rightHandSideObject) > 0);
        }
    }
}
