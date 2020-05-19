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

namespace MBF.IO
{
    /// <summary>
    /// Implementations of this interface write an ISequence to a particular location, usually a
    /// file. The output is formatted according to the particular file format. A method is
    /// also provided for quickly accessing the content in string form for applications that do not
    /// need to first write to file.
    /// </summary>
    public interface ISequenceFormatter : IFormatter
    {
        /// <summary>
        /// Writes an ISequence to the location specified by the writer.
        /// </summary>
        /// <param name="sequence">The sequence to format.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence text.</param>
        void Format(ISequence sequence, TextWriter writer);

        /// <summary>
        /// Writes an ISequence to the specified file.
        /// </summary>
        /// <param name="sequence">The sequence to format.</param>
        /// <param name="filename">The name of the file to write the formatted sequence text.</param>
        void Format(ISequence sequence, string filename);

        /// <summary>
        /// Write a collection of ISequences to a writer.
        /// </summary>
        /// <param name="sequences">The sequences to write.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequences.</param>
        void Format(ICollection<ISequence> sequences, TextWriter writer);

        /// <summary>
        /// Write a collection of ISequences to a file.
        /// </summary>
        /// <param name="sequences">The sequences to write.</param>
        /// <param name="filename">The name of the file to write the formatted sequences.</param>
        void Format(ICollection<ISequence> sequences, string filename);

        /// <summary>
        /// Converts an ISequence to a formatted string.
        /// </summary>
        /// <param name="sequence">The sequence to format.</param>
        /// <returns>A string of the formatted text.</returns>
        string FormatString(ISequence sequence);
    }
}
