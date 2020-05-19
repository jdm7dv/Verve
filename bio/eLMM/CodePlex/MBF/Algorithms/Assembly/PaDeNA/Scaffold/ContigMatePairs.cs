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
    /// Stores information about Contig - Contig mate pair map.
    /// Forward Contig     Reverse Contig
    /// ---------------) (---------------
    ///    -------)           (------
    ///    Forward              Reverse
    ///    read                 read
    ///    
    /// Key: Sequence of Forward Contig
    /// Value:
    ///     Key: Sequence of reverse contig
    ///     Value: List of mate pair between two contigs.
    /// </summary>
    [Serializable]
    public class ContigMatePairs : Dictionary<ISequence, Dictionary<ISequence, IList<ValidMatePair>>>
    {
        #region constructors

        /// <summary>
        /// Initializes a new instance of the ContigMatePairs class.
        /// </summary>
        public ContigMatePairs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ContigMatePairs class for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected ContigMatePairs(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ContigMatePairs class.
        /// </summary>
        /// <param name="contigs">List of contigs.</param>
        public ContigMatePairs(IList<ISequence> contigs)
        {
            if (contigs == null)
            {
                throw new ArgumentNullException("contigs");
            }

            foreach (ISequence contig in contigs)
            {
                Add(contig, new Dictionary<ISequence, IList<ValidMatePair>>());
            }
        }

        #endregion
    }
}
