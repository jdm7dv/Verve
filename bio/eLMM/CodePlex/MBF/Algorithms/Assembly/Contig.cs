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
using Bio.Util.Logging;

namespace Bio.Algorithms.Assembly
{
    /// <summary>
    /// Contig is a data storage object representing a set of sequences
    /// that have been assembled into a new, longer sequence.
    /// 
    /// This class is marked with Serializable attribute thus instances of this 
    /// class can be serialized and stored to files and the stored files 
    /// can be de-serialized to restore the instances.
    /// </summary>
    [Serializable]
    public class Contig
    {
        #region Member Variables
        private IList<AssembledSequence> _sequences = new List<AssembledSequence>();
        #endregion

        #region Nested Structs
        /// <summary>
        /// A sequence, as it has been located into the contig. This includes
        /// possible reversal, complementation, or both.
        /// 
        /// This class is marked with Serializable attribute thus instances of this 
        /// class can be serialized and stored to files and the stored files 
        /// can be de-serialized to restore the instances.
        /// </summary>
        [Serializable]
        public struct AssembledSequence
        {
            /// <summary>
            /// The sequence, as possibly modified (via gap insertion) by
            /// the overlap algorithm.
            /// </summary>
            public ISequence Sequence;
            /// <summary>
            /// The offset from the start of the contig where this sequence begins.
            /// </summary>
            public int Position;
            /// <summary>
            /// Whether the sequence was complemented in order to find sufficient overlap.
            /// </summary>
            public bool IsComplemented;
            /// <summary>
            /// Whether the orientation of the sequence was reversed in order to find
            /// sufficient overlap.
            /// <remarks>
            /// If the assembly algorithm used AssumeStandardOrientation=true, then IsReversed
            /// and IsComplemented will both be true (reverse complement) or both be false.
            /// </remarks>
            /// </summary>
            public bool IsReversed;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The set of sequences that have been assembled to form the contig.
        /// </summary>
        public IList<AssembledSequence> Sequences
        {
            get
            {
                return _sequences;
            }
            set
            {
                _sequences = value;
            }
        }

        /// <summary>
        /// A sequence derived from the input sequences as assembled, representing the
        /// contents of the whole range of the contig.
        /// <remarks>
        /// This is built by an IConsensusMethod.
        /// </remarks>
        /// </summary>
        public ISequence Consensus { get; set; }

        /// <summary>
        /// The length of the contig equals the length of its consensus.
        /// </summary>
        public int Length
        {
            get
            {
                if (Consensus == null)
                {
                    string message = "Contig.Length: Consensus has not been computed.";
                    Trace.Report(message);
                    throw new Exception(message);
                }
                return Consensus.Count;
            }
        }
        #endregion
    }
}
