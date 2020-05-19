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
    /// A single BLAST search result. This is represented by a single XML
    /// document in BLAST XML format. It consist of some introductory information
    /// such as BLAST version, a structure listing the search parameters, and
    /// a list of Iterations (represented in the BlastSearchRecord class).
    /// </summary>
    [Serializable]
    public class BlastResult : ISerializable
    {
        #region Fields

        /// <summary>
        /// List of BlastSearchRecords in the document.
        /// </summary>
        private IList<BlastSearchRecord> _records = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public BlastResult()
        {
            _records = new List<BlastSearchRecord>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The summary data for the search
        /// </summary>
        public BlastXmlMetadata Metadata { get; set; }

        /// <summary>
        /// The list of BlastSearchRecords in the document.
        /// </summary>
        public IList<BlastSearchRecord> Records
        {
            get { return _records; }
        }

        #endregion

        #region Methods
        #endregion

        #region ISerializable Members

        /// <summary>
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected BlastResult(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            Metadata = (BlastXmlMetadata)info.GetValue("BlastResult:Metadata", typeof(BlastXmlMetadata));
            _records = (IList<BlastSearchRecord>)info.GetValue("BlastResult:Records", typeof(IList<BlastSearchRecord>));
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

            info.AddValue("BlastResult:Metadata", Metadata);
            info.AddValue("BlastResult:Records", _records);
        }

        #endregion
    }
}