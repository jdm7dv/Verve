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
using System.Runtime.Serialization;
using MBF.Properties;
using MBF.Util.Logging;

namespace MBF.Algorithms.Alignment
{
    /// <summary>
    /// A simple implementation of ISequenceAlignment that stores the 
    /// result of an alignment. 
    /// </summary>
    [Serializable]
    public class SequenceAlignment : ISequenceAlignment
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the SequenceAlignment class
        /// Default Constructor.
        /// </summary>
        public SequenceAlignment()
        {
            Metadata = new Dictionary<string, object>();
            AlignedSequences = new List<IAlignedSequence>();
            Sequences = new List<ISequence>();
        }

        /// <summary>
        /// Initializes a new instance of the SequenceAlignment class
        /// Internal constructor to create SequenceAlignemnt from ISequenceAlignment.
        /// </summary>
        /// <param name="seqAlignment">Sequence Alignment</param>
        internal SequenceAlignment(ISequenceAlignment seqAlignment)
        {
            Metadata = seqAlignment.Metadata;
            AlignedSequences = new List<IAlignedSequence>(seqAlignment.AlignedSequences);
            Documentation = seqAlignment.Documentation;
            Sequences = new List<ISequence>(seqAlignment.Sequences);
        }
        #endregion

        /// <summary>
        /// Gets any additional information about the Alignment.
        /// </summary>
        public Dictionary<string, object> Metadata { get; private set; }

        /// <summary>
        /// Gets list of aligned sequences.
        /// </summary>
        public IList<IAlignedSequence> AlignedSequences { get; private set; }

        /// <summary>
        /// Gets list of source sequences involved in the alignment.
        /// </summary>
        public IList<ISequence> Sequences { get; private set; }

        /// <summary>
        /// Gets or sets documentation for this alignment.
        /// </summary>
        public object Documentation { get; set; }

        #region ISerializable Members
        /// <summary>
        /// Initializes a new instance of the SequenceAlignment class
        /// Constructor for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected SequenceAlignment(SerializationInfo info, StreamingContext context)
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
            AlignedSequences = (IList<IAlignedSequence>)info.GetValue("AlignedSeqs", typeof(IList<IAlignedSequence>));
            Documentation = info.GetValue("Doc", typeof(object));
        }
        
        /// <summary>
        /// Method for serializing the SequenceAlignment.
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
            info.AddValue("AlignedSeqs", AlignedSequences);

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
    }
}
