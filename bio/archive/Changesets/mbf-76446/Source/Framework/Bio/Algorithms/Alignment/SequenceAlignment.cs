// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Bio;
using Bio.Util.Logging;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// A simple implementation of ISequenceAlignment that stores the 
    /// result of an alignment. 
    /// </summary>
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
    }
}
