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
using System.Runtime.Serialization;

namespace MBF.Web.Blast
{
    /// <summary>
    /// A single result from a sequence search, such as any of the various flavors of BLAST.
    /// This is referred to as an Iteration in the BLAST XML schema; some flavors (such as
    /// PSI-BLAST) can combine multiple interations into one XML document.
    /// </summary>
    [Serializable]
    public class BlastSearchRecord : ISerializable
    {
        #region Fields

        /// <summary>
        /// Lists of hits associated with this iteration
        /// </summary>
        private IList<Hit> _hits = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public BlastSearchRecord()
        {
            _hits = new List<Hit>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The set of hits associated with this iteration
        /// </summary>
        public IList<Hit> Hits
        {
            get { return _hits; }
        }

        /// <summary>
        /// The index for this iteration
        /// </summary>
        public int IterationNumber { get; set; }

        /// <summary>
        /// The ID of the query which generated this iteration
        /// </summary>
        public string IterationQueryId { get; set; }

        /// <summary>
        /// The definition of the query which generated this iteration
        /// </summary>
        public string IterationQueryDefinition { get; set; }

        /// <summary>
        /// The length of the query which generated this iteration
        /// </summary>
        public int IterationQueryLength { get; set; }

        /// <summary>
        /// A human-readable message associated with this iteration
        /// </summary>
        public string IterationMessage { get; set; }

        /// <summary>
        /// The statistics returned for this iteration
        /// </summary>
        public BlastStatistics Statistics { get; set; }

        #endregion

        #region ISerializable Members

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected BlastSearchRecord(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            _hits = (IList<Hit>)info.GetValue("BlastSearchRecord:Hits", typeof(IList<Hit>));
            IterationNumber = info.GetInt32("BlastSearchRecord:IterationNumber");
            IterationQueryId = info.GetString("BlastSearchRecord:IterationQueryId");
            IterationQueryDefinition = info.GetString("BlastSearchRecord:IterationQueryDefinition");
            IterationQueryLength = info.GetInt32("BlastSearchRecord:IterationQueryLength");
            IterationMessage = info.GetString("BlastSearchRecord:IterationMessage");
            Statistics = (BlastStatistics)info.GetValue("BlastSearchRecord:Statistics", typeof(BlastStatistics));
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

            info.AddValue("BlastSearchRecord:Hits", _hits);
            info.AddValue("BlastSearchRecord:IterationNumber", IterationNumber);
            info.AddValue("BlastSearchRecord:IterationQueryId", IterationQueryId);
            info.AddValue("BlastSearchRecord:IterationQueryDefinition", IterationQueryDefinition);
            info.AddValue("BlastSearchRecord:IterationQueryLength", IterationQueryLength);
            info.AddValue("BlastSearchRecord:IterationMessage", IterationMessage);
            info.AddValue("BlastSearchRecord:Statistics", Statistics);
        }

        #endregion
    }
}