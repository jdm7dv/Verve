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
using System.Runtime.Serialization;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// AlignedSequence is a class containing the single aligned unit of alignment.
    /// </summary>
    [Serializable]
    public class AlignedSequence : IAlignedSequence
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the AlignedSequence class
        /// Default Constructor.
        /// </summary>
        public AlignedSequence()
        {
            Metadata = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            Sequences = new List<ISequence>();
        }

        /// <summary>
        /// Initializes a new instance of the AlignedSequence class
        /// Internal constructor to create AlignedSequence instance from IAlignedSequence.
        /// </summary>
        /// <param name="alignedSequence">IAlignedSequence instance.</param>
        internal AlignedSequence(IAlignedSequence alignedSequence)
        {
            Metadata = alignedSequence.Metadata;
            Sequences = new List<ISequence>(alignedSequence.Sequences);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets information about the AlignedSequence, like score, offsets, consensus, etc..
        /// </summary>
        public Dictionary<string, object> Metadata { get; private set; }

        /// <summary>
        /// Gets list of sequences involved in the alignment.
        /// </summary>
        public IList<ISequence> Sequences { get; private set; }
        #endregion

        #region ISerializable Members
        /// <summary>
        /// Initializes a new instance of the AlignedSequence class
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected AlignedSequence(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            if (info.GetBoolean("M"))
            {
                Metadata = (Dictionary<string, object>)info.GetValue("MD", typeof(Dictionary<string, object>));
            }
            else
            {
                Metadata = new Dictionary<string, object>();
            }

            Sequences = (IList<ISequence>)info.GetValue("Seqs", typeof(IList<ISequence>));
        }
        
        /// <summary>
        /// Method for serializing the AlignedSequence.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            Dictionary<string, object> tempMetadata = new Dictionary<string, object>();

            // Ignore non serializable objects in the metadata.
            foreach (KeyValuePair<string, object> kvp in Metadata)
            {
                if ((kvp.Value.GetType().Attributes & System.Reflection.TypeAttributes.Serializable)
                    == System.Reflection.TypeAttributes.Serializable)
                {
                    tempMetadata.Add(kvp.Key, kvp.Value);
                }
                else
                {
                    tempMetadata.Add(kvp.Key, null);
                }
            }

            if (tempMetadata.Count > 0)
            {
                info.AddValue("M", true);
                info.AddValue("MD", tempMetadata);
            }
            else
            {
                info.AddValue("M", false);
            }

            info.AddValue("Seqs", Sequences);
        }
        #endregion
    }
}
