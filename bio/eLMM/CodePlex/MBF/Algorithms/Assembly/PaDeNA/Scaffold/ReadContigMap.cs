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

namespace Bio.Algorithms.Assembly.PaDeNA.Scaffold
{
    /// <summary>
    /// Class stores multiple mapping between reads and a contig.
    ///     -------------------         Read Sequence
    /// ------------------------------  Contig Sequence [Full Overlap]
    ///               ----------------  Contig Sequence [Partial Overlap]
    /// The Class stores 
    /// Key: Sequence Id of Read 
    /// Value
    ///     Key: Sequence of Contig
    ///     Value: List of position of Overlaps of contig with read.
    /// </summary>
    [Serializable]
    public class ReadContigMap : Dictionary<string, Dictionary<ISequence, IList<ReadMap>>>
    {
        #region constructors

        /// <summary>
        /// Initializes a new instance of the ReadContigMap class.
        /// </summary>
        public ReadContigMap()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ReadContigMap class. 
        /// Used for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected ReadContigMap(SerializationInfo info, StreamingContext context)
            : base (info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ReadContigMap class.
        /// </summary>
        /// <param name="reads">List of reads.</param>
        public ReadContigMap(IList<ISequence> reads)
        {
            if (reads == null)
            {
                throw new ArgumentNullException("reads");
            }

            foreach (ISequence read in reads)
            {
                this.Add(read.DisplayID, new Dictionary<ISequence, IList<ReadMap>>());
            }
        }

        #endregion
    }
}
