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

namespace Bio.IO
{
    /// <summary>
    /// Implementations of this interface write an ISequence to a particular location, usually a
    /// file. The output is formatted according to the particular file format.
    /// </summary>
    public interface ISequenceFormatter : IFormatter, IDisposable
    {
        /// <summary>
        /// Opens specified file to write.
        /// </summary>
        /// <param name="filename">Filename to open.</param>
        void Open(string filename);

        /// <summary>
        /// Writes an ISequence.
        /// </summary>
        /// <param name="sequence">The sequence to write.</param>
        void Write(ISequence sequence);

        /// <summary>
        /// Closes underlying stream if any.
        /// </summary>
        void Close();
    }
}
