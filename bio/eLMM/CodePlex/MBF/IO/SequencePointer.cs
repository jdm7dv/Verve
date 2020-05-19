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

namespace Bio.IO
{
    /// <summary>
    /// class holds file pointer info of sequence. This class is used by Data Virtualization.
    /// This class is loaded with pointer info and serialized into sidecar file for the further 
    /// quick load.
    /// </summary>
    [Serializable]
    public class SequencePointer
    {
        /// <summary>
        /// starting line number
        /// </summary>
        public int StartingLine { get; set; }

        /// <summary>
        /// sequence starting index
        /// </summary>
        public long StartingIndex { get; set; }

        /// <summary>
        /// sequence ending index
        /// </summary>
        public long EndingIndex { get; set; }

        /// <summary>
        /// ID of the sequence
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Alphabet type of the sequence
        /// </summary>
        public string AlphabetName { get; set; }
    }
}


