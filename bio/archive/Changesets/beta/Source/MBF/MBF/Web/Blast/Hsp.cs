// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Runtime.Serialization;

namespace MBF.Web.Blast
{
    /// <summary>
    /// a High-scoring Segment Pair.
    /// </summary>
    /// <remarks>
    /// Represents an aligned section of the query and hit sequences with high similarity.
    /// </remarks>
    [Serializable]
    public class Hsp : ISerializable
    {
        #region Constructors

        /// <summary>
        /// Default Constructor: Initializes an instance of class BlastStatistics
        /// </summary>
        public Hsp()
        { }

        #endregion

        /// <summary>
        /// The score for the pair
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Normalized form of the score
        /// </summary>
        public double BitScore { get; set; }

        /// <summary>
        /// Expectation value
        /// </summary>
        public double EValue { get; set; }

        /// <summary>
        /// The start location of the match in the query sequence
        /// </summary>
        public long QueryStart { get; set; }

        /// <summary>
        /// The end location of the match in the query sequence
        /// </summary>
        public long QueryEnd { get; set; }

        /// <summary>
        /// The start location of the match in the hit sequence
        /// </summary>
        public long HitStart { get; set; }

        /// <summary>
        /// The end location of the match in the hit sequence
        /// </summary>
        public long HitEnd { get; set; }

        /// <summary>
        /// The frame for the query sequence
        /// </summary>
        public long QueryFrame { get; set; }

        /// <summary>
        /// The frame for the hit sequence
        /// </summary>
        public long HitFrame { get; set; }

        /// <summary>
        /// Number of residues that matched exactly
        /// </summary>
        public long IdentitiesCount { get; set; }

        /// <summary>
        /// Number of residues that matched conservatively (for proteins)
        /// </summary>
        public long PositivesCount { get; set; }

        /// <summary>
        /// The length of the local match
        /// </summary>
        public long AlignmentLength { get; set; }

        /// <summary>
        /// The score density
        /// </summary>
        public int Density { get; set; }

        /// <summary>
        /// The local match in the query sequence, as a string
        /// </summary>
        public string QuerySequence { get; set; }

        /// <summary>
        /// The local match in the hit sequence, as a string
        /// </summary>
        public string HitSequence { get; set; }

        /// <summary>
        /// Gets or sets the formating middle line
        /// </summary>
        public string Midline { get; set; }

        /// <summary>
        /// Gets or sets start of PHI-BLAST pattern
        /// </summary>
        public int PatternFrom { get; set; }

        /// <summary>
        /// Gets or sets end of PHI-BLAST pattern
        /// </summary>
        public int PatternTo { get; set; }

        /// <summary>
        /// Gets are sets number of gaps in HSP
        /// </summary>
        public int Gaps { get; set; }

        #region ISerializable Members

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected Hsp(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            Score = info.GetDouble("Hsp:Score");
            BitScore = info.GetDouble("Hsp:BitScore");
            EValue = info.GetDouble("Hsp:EValue");
            QueryStart = info.GetInt64("Hsp:QueryStart");
            QueryEnd = info.GetInt64("Hsp:QueryEnd");
            HitStart = info.GetInt64("Hsp:HitStart");
            HitEnd = info.GetInt64("Hsp:HitEnd");
            QueryFrame = info.GetInt64("Hsp:QueryFrame");
            HitFrame = info.GetInt64("Hsp:HitFrame");
            IdentitiesCount = info.GetInt64("Hsp:IdentitiesCount");
            PositivesCount = info.GetInt64("Hsp:PositivesCount");
            AlignmentLength = info.GetInt64("Hsp:AlignmentLength");
            Density = info.GetInt32("Hsp:Density");
            QuerySequence = info.GetString("Hsp:QuerySequence");
            HitSequence = info.GetString("Hsp:HitSequence");
            Midline = info.GetString("Hsp:Midline");
            PatternFrom = info.GetInt32("Hsp:PatternFrom");
            PatternTo = info.GetInt32("Hsp:PatternTo");
            Gaps = info.GetInt32("Hsp:Gaps");
        }

        /// <summary>
        /// Method for serializing the sequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("Hsp:Score", Score);
            info.AddValue("Hsp:BitScore", BitScore);
            info.AddValue("Hsp:EValue", EValue);
            info.AddValue("Hsp:QueryStart", QueryStart);
            info.AddValue("Hsp:QueryEnd", QueryEnd);
            info.AddValue("Hsp:HitStart", HitStart);
            info.AddValue("Hsp:HitEnd", HitEnd);
            info.AddValue("Hsp:QueryFrame", QueryFrame);
            info.AddValue("Hsp:HitFrame", HitFrame);
            info.AddValue("Hsp:IdentitiesCount", IdentitiesCount);
            info.AddValue("Hsp:PositivesCount", PositivesCount);
            info.AddValue("Hsp:AlignmentLength", AlignmentLength);
            info.AddValue("Hsp:Density", Density);
            info.AddValue("Hsp:QuerySequence", QuerySequence);
            info.AddValue("Hsp:HitSequence", HitSequence);
            info.AddValue("Hsp:Midline", Midline);
            info.AddValue("Hsp:PatternFrom", PatternFrom);
            info.AddValue("Hsp:PatternTo", PatternTo);
            info.AddValue("Hsp:Gaps", Gaps);
        }

        #endregion
    }
}