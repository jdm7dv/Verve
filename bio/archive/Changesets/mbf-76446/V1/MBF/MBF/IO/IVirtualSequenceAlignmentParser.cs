// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using MBF.Algorithms.Alignment;

namespace MBF.IO
{
    /// <summary>
    /// Interface provides Data Virtualization on the parser. If the parser wants to support
    /// Data Virtualization, it needs to come from this interface.
    /// </summary>
    public interface IVirtualSequenceAlignmentParser : ISequenceAlignmentParser
    {
        /// <summary>
        /// Explicitly enable data virtualization with this parser.
        /// </summary>
        bool EnforceDataVirtualization { get; set; }

        /// <summary>
        /// Data virtualization is enabled or not with the parser
        /// </summary>
        bool IsDataVirtualizationEnabled { get; }

        /// <summary>
        /// File size (in KBs) to enforce data virtualization. If the file size is
        /// larger, then data virtualization is loaded automatically.
        /// </summary>
        int EnforceDataVirtualizationByFileSize { get; set; } 

        /// <summary>
        /// Parses the sequence range based on the sequence
        /// </summary>
        /// <param name="pointer">Sequence pointer to the sequence.</param>
        /// <returns>IAlignedSequence object.</returns>
        IAlignedSequence ParseAlignedSequence(SequencePointer pointer);
    }
}
