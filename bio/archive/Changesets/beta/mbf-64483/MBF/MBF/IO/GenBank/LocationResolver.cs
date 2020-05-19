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
using System.Linq;
using System.Runtime.Serialization;
using MBF.Properties;

namespace MBF.IO.GenBank
{
    /// <summary>
    /// This is the default implementation of ILocationResolver.
    /// This class resolves the start and end positions of a location.
    /// 
    /// Please see the following table for how this class resolves the ambiguities in start and end data.
    /// 
    /// Start/End Data		Resolved Start		Resolved End
    /// 12.30               12      			30
    /// &gt;30	            30			        30
    /// &lt;30 	            30			        30
    /// 23^24		        23	                24
    /// 100^1		        1000    			1	
    /// </summary>
    [Serializable]
    public class LocationResolver : ILocationResolver
    {
        #region Constructors
        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocationResolver()
        {
            // No implementation.
        }
        #endregion Constructors

        #region Public Methods
        /// <summary>
        /// Returns the new LocationResolver instance that is a copy of this instance.
        /// </summary>
        public LocationResolver Clone()
        {
            return new LocationResolver();
        }
        #endregion Public Methods

        #region ILocationResolver Members

        /// <summary>
        /// Returns the start position by resolving the startdata present in the specified location.
        /// If unable to resolve startdata then an exception will occur.
        /// </summary>
        /// <param name="location">Location instance.</param>
        public int GetStart(ILocation location)
        {
            if (location == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameLocation);
            }

            // If sublocations are there, then get the minimum start position from the sublocations.
            if (location.SubLocations.Count > 0)
            {
                return location.SubLocations.OrderBy(L => L.Start).ToList()[0].Start;
            }

            if (string.IsNullOrEmpty(location.StartData))
            {
                throw new ArgumentException(Resource.StartDataCannotBeNull);
            }

            return ResolveStart(location.StartData);
        }

        /// <summary>
        /// Returns the end position by resolving the enddata present in the specified location.
        /// If unable to resolve enddata then an exception will occur.
        /// </summary>
        /// <param name="location">Location instance.</param>
        public int GetEnd(ILocation location)
        {
            if (location == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameLocation);
            }

            // If sublocations are there, then get the max end position from the sublocations.
            if (location.SubLocations.Count > 0)
            {
                return location.SubLocations.OrderByDescending(L => L.End).ToList()[0].End;
            }

            if (string.IsNullOrEmpty(location.EndData))
            {
                throw new InvalidOperationException(Resource.EndDataCannotBeNull);
            }

            return ResolveEnd(location.EndData);
        }

        /// <summary>
        /// Returns a sequence which contains bases from the specified sequence as specified by the location.
        /// If the location of a feature and sequence in which the feature is present is 
        /// specified then this method returns a sequence which contains the bases of the specified feature.
        /// 
        /// Please note that,
        /// 1. If Accession of the location is not null or empty then an exception will occur.
        /// 2. If the location contains "order" operator then this method uses SegmentedSequence class to construct the sequence.
        ///    For example, order(100..200,300..450) will result in a SegmentedSequence which internally contains two sequences, 
        ///    first one created from 100 to 200 bases, and second one created from 300 to 450 bases.
        /// </summary>
        /// <param name="location">Location instance.</param>
        /// <param name="sequence">Sequence from which the sub sequence has to be returned.</param>
        public ISequence GetSubSequence(ILocation location, ISequence sequence)
        {
            if (location == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameLocation);
            }

            if (sequence == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameSequence);
            }

            return GetSubSequence(location, sequence, null);
        }

        /// <summary>
        /// Returns a sequence which contains bases from the specified sequence as specified by the location.
        /// If the location contains accession then the sequence from the referredSequences which matches the 
        /// accession of the location will be considered.
        /// 
        /// For example,
        /// if location is "join(100..200, J00089.1:10..50, J00090.2:30..40)"
        /// then bases from 100 to 200 will be considered from the sequence parameter and referredSequences will
        /// be searched for the J00089.1 and J00090.2 accession if found then those sequences will be considered 
        /// for constructing the output sequence.
        /// If the referred sequence is not found in the referredSequences then an exception will occur.
        /// </summary>
        /// <param name="location">Location instance.</param>
        /// <param name="sequence">Sequence instance from which the sub sequence has to be returned.</param>
        /// <param name="referredSequences">A dictionary containing Accession numbers as keys and Sequences as values, this will be used when
        /// the location or sublocations contains accession.</param>
        public ISequence GetSubSequence(ILocation location, ISequence sequence, Dictionary<string, ISequence> referredSequences)
        {
            if (location == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameLocation);
            }

            if (sequence == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameSequence);
            }

            BasicDerivedSequence basicDerSeq = new BasicDerivedSequence(sequence, false, false, -1, -1);

            if (location.Operator == LocationOperator.Complement)
            {
                if (location.SubLocations.Count > 1)
                {
                    throw new ArgumentException(Resource.ComplementWithMorethanOneSubLocs);
                }

                if (location.SubLocations.Count > 0)
                {
                    basicDerSeq.Source = location.SubLocations[0].GetSubSequence(sequence, referredSequences);
                }
                else
                {
                    basicDerSeq.Source = GetSubSequence(location.Start, location.End, location.Accession, location.Separator, sequence, referredSequences);
                }

                basicDerSeq.Complemented = true;
                return new Sequence(sequence.Alphabet, basicDerSeq.ToString());
            }

            if (location.Operator == LocationOperator.Order)
            {
                List<ISequence> subSequences = new List<ISequence>();
                if (location.SubLocations.Count > 0)
                {
                    foreach (ILocation loc in location.SubLocations)
                    {
                        subSequences.Add(loc.GetSubSequence(sequence, referredSequences));
                    }
                }
                else
                {
                    basicDerSeq.Source = GetSubSequence(location.Start, location.End, location.Accession, location.Separator, sequence, referredSequences);
                    subSequences.Add(new Sequence(sequence.Alphabet, basicDerSeq.ToString()));
                }

                return new SegmentedSequence(subSequences);
            }

            if (location.Operator == LocationOperator.Join || location.Operator == LocationOperator.Bond)
            {
                if (location.SubLocations.Count > 0)
                {
                    List<ISequence> subSequences = new List<ISequence>();
                    foreach (ILocation loc in location.SubLocations)
                    {
                        subSequences.Add(loc.GetSubSequence(sequence, referredSequences));
                    }

                    Sequence seq = new Sequence(sequence.Alphabet);
                    foreach (ISequence subSeq in subSequences)
                    {
                        seq.InsertRange(seq.Count, subSeq.ToString());
                    }

                    return seq;
                }
                else
                {
                    return GetSubSequence(location.Start, location.End, location.Accession, location.Separator, sequence, referredSequences);
                }
            }

            if (location.SubLocations.Count > 0)
            {
                throw new ArgumentException(Resource.NoneWithSubLocs);
            }

            return GetSubSequence(location.Start, location.End, location.Accession, location.Separator, sequence, referredSequences);
        }

        /// <summary>
        /// Return true if the specified position is within the start position.
        /// For example,
        /// if the startdata of a location is "23.40", this method will 
        /// return true for the position values ranging from 23 to 40.
        /// </summary>
        /// <param name="location">Location instance.</param>
        /// <param name="Position">Position to be verified.</param>
        /// <returns>Returns true if the specified position is with in the start position else returns false.</returns>
        public bool IsInStart(ILocation location, int Position)
        {
            if (location == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameLocation);
            }

            List<ILocation> leafLocations = location.GetLeafLocations();
            int minStart;
            int maxStart;
            foreach (ILocation loc in leafLocations)
            {
                minStart = ResolveStart(loc.StartData);
                maxStart = ResolveEnd(loc.StartData);
                if (Position >= minStart && Position <= maxStart)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Return true if the specified position is within the end position.
        /// For example,
        /// if the enddata of a location is "23.40", this method will 
        /// return true for the position values ranging from 23 to 40.
        /// </summary>
        /// <param name="location">Location instance.</param>
        /// <param name="Position">Position to be verified.</param>
        /// <returns>Returns true if the specified P\position is with in the end position else returns false.</returns>
        public bool IsInEnd(ILocation location, int Position)
        {
            if (location == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameLocation);
            }

            List<ILocation> leafLocations = location.GetLeafLocations();
            int minStart;
            int maxStart;
            foreach (ILocation loc in leafLocations)
            {
                maxStart = ResolveEnd(loc.EndData);
                minStart = ResolveStart(loc.EndData);

                if (Position >= minStart && Position <= maxStart)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the specified position is with in the start and end positions.
        /// </summary>
        /// <param name="location">Location instance.</param>
        /// <param name="Position">Position to be verified.</param>
        /// <returns>Returns true if the specified position is with in the start and end positions else returns false.</returns>
        public bool IsInRange(ILocation location, int Position)
        {
            if (location == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameLocation);
            }

            List<ILocation> leafLocations = location.GetLeafLocations();
            foreach (ILocation loc in leafLocations)
            {
                if (Position >= loc.Start && Position <= loc.End)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a new ILocationResolver that is a copy of the current ILocationResolver.
        /// </summary>
        /// <returns>A new ILocationResolver that is a copy of this ILocationResolver.</returns>
        ILocationResolver ILocationResolver.Clone()
        {
            return Clone();
        }
        #endregion

        #region ISerializable Members
        /// <summary>
        /// Method for serializing the LocationResolver.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // No implementation.
        }

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        internal LocationResolver(SerializationInfo info, StreamingContext context)
        {
            // No implementation.
        }
        #endregion ISerializable Members

        #region ICloneable Members

        /// <summary>
        /// Creates a new LocationResolver that is a copy of the current LocationResolver.
        /// </summary>
        /// <returns>A new object that is a copy of this LocationResolver.</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        #endregion ICloneable Members

        #region Private Methods
        /// <summary>
        /// Resolves and returns the start position.
        /// </summary>
        /// <param name="str">Start data.</param>
        private int ResolveStart(string str)
        {
            int value;
            if (int.TryParse(str, out value))
            {
                return value;
            }
            else
            {
                if (str.StartsWith(">"))
                {
                    int firstIndex = str.IndexOf(">");
                    if (firstIndex != str.LastIndexOf(">"))
                    {
                        string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidStartData, str);
                        throw new FormatException(msgStr);
                    }

                    return ResolveStart(str.Substring(1));
                }
                else if (str.StartsWith("<"))
                {
                    int firstIndex = str.IndexOf("<");
                    if (firstIndex != str.LastIndexOf("<"))
                    {
                        string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidStartData, str);
                        throw new FormatException(msgStr);
                    }

                    return ResolveStart(str.Substring(1));
                }
                else if (str.Contains("^"))
                {
                    int firstIndex = str.IndexOf("^");
                    if (firstIndex != str.LastIndexOf("^"))
                    {
                        string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidStartData, str);
                        throw new FormatException(msgStr);
                    }

                    string[] values = str.Split("^".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length != 2)
                    {
                        string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidStartData, str);
                        throw new FormatException(msgStr);
                    }

                    return ResolveStart(values[0]);
                }
                else if (str.Contains("."))
                {
                    int firstIndex = str.IndexOf(".");
                    if (firstIndex != str.LastIndexOf("."))
                    {
                        string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidStartData, str);
                        throw new FormatException(msgStr);
                    }

                    string[] values = str.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length != 2)
                    {
                        string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidStartData, str);
                        throw new FormatException(msgStr);
                    }

                    return ResolveStart(values[0]);
                }
                else
                {
                    string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidStartData, str);
                    throw new FormatException(msgStr);
                }
            }
        }

        /// <summary>
        /// Resolves and returns the end position.
        /// </summary>
        /// <param name="str">End data.</param>
        private int ResolveEnd(string str)
        {
            int value;
            if (int.TryParse(str, out value))
            {
                return value;
            }
            else
            {
                if (str.StartsWith(">"))
                {
                    int firstIndex = str.IndexOf(">");
                    if (firstIndex != str.LastIndexOf(">"))
                    {
                        string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidEndData, str);
                        throw new FormatException(msgStr);
                    }

                    return ResolveStart(str.Substring(1));
                }
                else if (str.StartsWith("<"))
                {
                    int firstIndex = str.IndexOf("<");
                    if (firstIndex != str.LastIndexOf("<"))
                    {
                        string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidEndData, str);
                        throw new FormatException(msgStr);
                    }

                    return ResolveStart(str.Substring(1));
                }
                else if (str.Contains("^"))
                {
                    int firstIndex = str.IndexOf("^");
                    if (firstIndex != str.LastIndexOf("^"))
                    {
                        string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidEndData, str);
                        throw new FormatException(msgStr);
                    }

                    string[] values = str.Split("^".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length > 2)
                    {
                        string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidEndData, str);
                        throw new FormatException(msgStr);
                    }

                    return ResolveStart(values[values.Length - 1]);
                }
                else if (str.Contains("."))
                {
                    int firstIndex = str.IndexOf(".");
                    if (firstIndex != str.LastIndexOf("."))
                    {
                        string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidEndData, str);
                        throw new FormatException(msgStr);
                    }

                    string[] values = str.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length > 2)
                    {
                        string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidEndData, str);
                        throw new FormatException(msgStr);
                    }

                    return ResolveStart(values[values.Length - 1]);
                }
                else
                {
                    string msgStr = string.Format(CultureInfo.CurrentCulture, Resource.InvalidEndData, str);
                    throw new FormatException(msgStr);
                }
            }
        }

        /// <summary>
        /// Returns the sequence for the specified start and end positions.
        /// If the accession is null or empty then the source sequence is used to construct the output sequence,
        /// otherwise appropriate sequence from the referred sequence is used to construct output sequence.
        /// </summary>
        /// <param name="start">Start position.</param>
        /// <param name="end">End position.</param>
        /// <param name="accession">Accession number.</param>
        /// <param name="sepataror">Start and End separator.</param>
        /// <param name="source">Source sequence.</param>
        /// <param name="referredSequences">Referred Sequences.</param>
        private ISequence GetSubSequence(int start, int end, string accession, string sepataror, ISequence source, Dictionary<string, ISequence> referredSequences)
        {
            if (string.Compare(sepataror, "^", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                return new Sequence(source.Alphabet);
            }

            if (string.Compare(sepataror, "..", StringComparison.InvariantCultureIgnoreCase) != 0 &&
                string.Compare(sepataror, ".", StringComparison.InvariantCultureIgnoreCase) != 0 &&
                !string.IsNullOrEmpty(sepataror))
            {
                string str = string.Format(CultureInfo.CurrentCulture, Resource.InvalidSeparator, sepataror);
                throw new ArgumentException(str);
            }

            if (!string.IsNullOrEmpty(accession) && (referredSequences == null || !referredSequences.ContainsKey(accession)))
            {
                string str = string.Format(CultureInfo.CurrentCulture, Resource.AccessionSequenceNotFound, accession);
                throw new ArgumentException(str);
            }

            if (!string.IsNullOrEmpty(accession))
            {
                if (source.Alphabet != referredSequences[accession].Alphabet)
                {
                    string str = string.Format(CultureInfo.CurrentCulture, Resource.InvalidReferredAlphabet, accession);
                    throw new ArgumentException(str);
                }

                source = referredSequences[accession];
            }

            // as location.start is one based where as Range accepts zero based index.
            start = start - 1;
            int length = end - start;

            if (string.IsNullOrEmpty(sepataror) || string.Compare(sepataror, ".", StringComparison.InvariantCultureIgnoreCase) == 0)
            {
                length = 1;
            }

            return new Sequence(source.Alphabet, source.Range(start, length).ToString());
        }
        #endregion Private Methods
    }
}
