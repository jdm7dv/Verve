// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;

namespace MBF.IO
{
    /// <summary>
    /// This class holds data needed to index each sequence in a sequence file 
    /// when data virtualization is enabled. The data is serialized into a
    /// sidecar file to enable faster parsing and sequence access.
    /// </summary>
    [Serializable]
    public class SequencePointer
    {
        /// <summary>
        /// Initializes a new instance of the SequencePointer class.
        /// </summary>
        public SequencePointer()
        {
            IndexOffsets = new long[2];
        }

        /// <summary>
        /// Zero-based starting line number of the specified sequence.
        /// </summary>
        public int StartingLine { get; set; }

        /// <summary>
        /// Gets the zero-based indexes of the offsets of the specified sequence.
        /// </summary>
        public long[] IndexOffsets { get; private set; }

        /// <summary>
        /// ID of the specified sequence.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Alphabet type of the specified sequence.
        /// </summary>
        public string AlphabetName { get; set; }
    }
}


