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



namespace Bio.IO
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
        /// Parses the sequence range based on the sequence
        /// </summary>
        /// <param name="startIndex">sequence start index</param>
        /// <param name="count">sequence length</param>
        /// <param name="seqPointer">sequence pointer to a sequence</param>
        /// <returns>ISequence</returns>
        ISequence ParseRange(int startIndex, int count, SequencePointer seqPointer);
    }
}
