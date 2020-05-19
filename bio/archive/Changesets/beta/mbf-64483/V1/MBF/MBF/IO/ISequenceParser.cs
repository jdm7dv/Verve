// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using System.IO;

using MBF.Encoding;

namespace MBF.IO
{
    /// <summary>
    /// Implementations of this interface are designed to parse a file from a particular file
    /// format to produce an ISequence. For advanced users, the ability to select an encoding for
    /// the internal memory representation is provided. Implementations also have a default
    /// encoding for each alphabet that may be encountered.
    /// </summary>
    public interface ISequenceParser : IParser
    {
        /// <summary>
        /// Parses a list of biological sequence texts from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence text.</param>
        /// <returns>The list of parsed ISequence objects.</returns>
        IList<ISequence> Parse(TextReader reader);

        /// <summary>
        /// Parses a list of biological sequence texts from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequence objects.</returns>
        IList<ISequence> Parse(TextReader reader, bool isReadOnly);

        /// <summary>
        /// Parses a list of biological sequence texts from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <returns>The list of parsed ISequence objects.</returns>
        IList<ISequence> Parse(string filename);

        /// <summary>
        /// Parses a list of biological sequence texts from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequence objects.</returns>
        IList<ISequence> Parse(string filename, bool isReadOnly);

        /// <summary>
        /// Parses a single biological sequence text from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence text.</param>
        /// <returns>The parsed ISequence object.</returns>
        ISequence ParseOne(TextReader reader);

        /// <summary>
        /// Parses a single biological sequence text from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed ISequence object.</returns>
        ISequence ParseOne(TextReader reader, bool isReadOnly);

        /// <summary>
        /// Parses a single biological sequence text from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <returns>The parsed ISequence object.</returns>
        ISequence ParseOne(string filename);

        /// <summary>
        /// Parses a single biological sequence text from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed ISequence object.</returns>
        ISequence ParseOne(string filename, bool isReadOnly);
    }
}
