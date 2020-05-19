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



using System.Collections.Generic;
using System.IO;

namespace Bio.IO
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
