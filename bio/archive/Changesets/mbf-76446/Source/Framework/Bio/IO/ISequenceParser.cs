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

namespace Bio.IO
{
    /// <summary>
    /// Implementations of this interface are designed to parse a file from a particular file
    /// format to produce an ISequence or collection of ISequence.
    /// </summary>
    public interface ISequenceParser : IParser, IDisposable
    {
        /// <summary>
        /// Opens the specified file to parse.
        /// </summary>
        /// <param name="filename">Filename to open.</param>
        void Open(string filename);

        /// <summary>
        /// Parses a list of biological sequence texts.
        /// </summary>
        /// <returns>The collection of parsed ISequence objects.</returns>
        IEnumerable<ISequence> Parse();

        /// <summary>
        /// Closes underlying stream if any.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets or sets the alphabet to use for parsed ISequence objects.  If this is not set, the alphabet will
        /// be determined based on the file being parsed.
        /// </summary>
        IAlphabet Alphabet { get; set; }
    }
}
