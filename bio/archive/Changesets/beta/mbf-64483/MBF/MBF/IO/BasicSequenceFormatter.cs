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
    /// This is an abstract class that provides some basic operations common to sequence
    /// formatters. It is meant to be used as the base class for formatter implementations
    /// if the implementer wants to make use of default behavior.
    /// </summary>
    public abstract class BasicSequenceFormatter : ISequenceFormatter
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BasicSequenceFormatter()
        {
        }

        #endregion

        #region ISequenceFormatter Members

        /// <summary>
        /// Gets the name of the formatter. Intended to be filled in 
        /// by classes deriving from BasicSequenceFormatter class
        /// with the exact name of the formatter type.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the description of the formatter. Intended to be filled in 
        /// by classes deriving from BasicSequenceFormatter class
        /// with the exact details of the formatter.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the filetypes supported by the formatter. Intended to be filled in 
        /// by classes deriving from BasicSequenceFormatter class
        /// with the exact details of the filetypes supported.
        /// </summary>
        public abstract string FileTypes { get; }

        /// <summary>
        /// Writes an ISequence to the location specified by the writer.
        /// </summary>
        /// <param name="sequence">The sequence to format.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence text.</param>
        public abstract void Format(ISequence sequence, TextWriter writer);

        /// <summary>
        /// Writes an ISequence to the specified file.
        /// </summary>
        /// <param name="sequence">The sequence to format.</param>
        /// <param name="filename">The name of the file to write the formatted sequence text.</param>
        public void Format(ISequence sequence, string filename)
        {
            TextWriter writer = null;
            try
            {
                writer = new StreamWriter(filename);
                Format(sequence, writer);
            }
            catch
            {
                // If format failed, remove the created file.
                if (File.Exists(filename))
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                    File.Delete(filename);
                }
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                }
            }
        }

        /// <summary>
        /// Write a collection of ISequences to a file.
        /// </summary>
        /// <remarks>
        /// This method should be overridden by any formatters that need to format file-scope
        /// metadata that applies to all of the sequences in the file.
        /// </remarks>
        /// <param name="sequences">The sequences to write</param>
        /// <param name="writer">the TextWriter</param>
        public virtual void Format(ICollection<ISequence> sequences, TextWriter writer)
        {
            foreach (ISequence sequence in sequences)
            {
                Format(sequence, writer);
            }
        }

        /// <summary>
        /// Write a collection of ISequences to a file.
        /// </summary>
        /// <param name="sequences">The sequences to write.</param>
        /// <param name="filename">The name of the file to write the formatted sequences to.</param>
        public void Format(ICollection<ISequence> sequences, string filename)
        {
            TextWriter writer = null;
            try
            {
                writer = new StreamWriter(filename);
                Format(sequences, writer);
            }
            catch
            {
                // If format failed, remove the created file.
                if (File.Exists(filename))
                {
                    if (writer != null)
                    {
                        writer.Close();
                    }
                    File.Delete(filename);
                }
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Dispose();
                }
            }
        }

        /// <summary>
        /// Converts an ISequence to a formatted text.
        /// </summary>
        /// <param name="sequence">The sequence to format.</param> 
        /// <returns>A string of the formatted text.</returns>
        public string FormatString(ISequence sequence)
        {
            using (TextWriter writer = new StringWriter())
            {
                Format(sequence, writer);
                return writer.ToString();
            }
        }

        #endregion
    }
}
