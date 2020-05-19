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

namespace Bio.IO.GenBank
{
    /// <summary>
    /// A StrandTopology specifies whether the strand is linear or circular.
    /// </summary>
    [Serializable]
    public enum SequenceStrandTopology
    {
        /// <summary>
        /// None - StrandTopology is unspecified.
        /// </summary>
        None,

        /// <summary>
        /// Linear.
        /// </summary>
        Linear,

        /// <summary>
        /// Circular.
        /// </summary>
        Circular,
    }
}
