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

namespace MBF.Algorithms.Assembly.PaDeNA.Scaffold
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
        /// Initializes a new instance of the ContigMatePairs class.
        /// This constructor is used for deserialization.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming context.</param>
        protected ContigMatePairs(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ContigMatePairs class with specified contigs.
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
