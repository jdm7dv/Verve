// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.IO
{
    /// <summary>
    /// Interface provides Data Virtualization on the parser. If the parser wants to support
    /// Data Virtualization, it needs to come from this interface.
    /// </summary>
    public interface IVirtualSequenceParser : ISequenceParser
    {
        /// <summary>
        /// Data virtualization is enabled or not with the parser
        /// </summary>
        bool IsDataVirtualizationEnabled { get; }

        /// <summary>
        /// Enforces data virtualization on the parser.
        /// </summary>
        bool EnforceDataVirtualization {get; set;}

        /// <summary>
        /// File size (in KBs) to enforce data virtualization. If the file size is
        /// larger, then data virtualization is loaded automatically.
        /// </summary>
        int EnforceDataVirtualizationByFileSize { get; set; } 

        /// <summary>
        /// Parses the sequence range based on the sequence
        /// </summary>
        /// <param name="startIndex">sequence start index</param>
        /// <param name="count">sequence length</param>
        /// <param name="seqPointer">sequence pointer to a sequence</param>
        /// <returns>ISequence</returns>
        byte[] ParseRange(int startIndex, int count, SequencePointer seqPointer);
    }
}
