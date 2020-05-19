// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MBF.Algorithms.Assembly;
using MBF.IO;


namespace MBF.IO 
{
  
    /// <summary>
    /// This class will write a sparse sequence to a character separated value file,
    /// with one line per sequence item. The sequence ID, the sequence count and 
    /// offset (if provided) will be written as a comment to a sequence start line.
    /// Multiple sparse sequences can be written with the sequence start line
    /// acting as delimiters.
    /// E.g. formatting with '#' as sequence prefix and ',' as character separator
    /// #0,100, A sparse sequence of length 100 with 2 items
    /// 12,A
    /// 29,T
    /// #3,10, A sparse sequence of length 10 at offset 3 with 1 item
    /// 2,G
    /// #0,10, A sparse sequence of length 15 with no items
    /// </summary>
    public class XsvSparseFormatter : ISequenceFormatter 
    {
        #region Public Properties

        /// <summary>
        /// The character to separate the position and sequence item symbol on each line
        /// </summary>
        public char Separator { get; protected set; }
        
        /// <summary>
        /// this prefix will be printed at the start of the line with 
        /// the offset, count and sequence ID. This is treated as the comment 
        /// character prefix in the underlying XsvTextReader.
        /// </summary>
        public char SequenceIDPrefix { get; set; }

        #endregion


        #region Constructor

        /// <summary>
        /// Creates an XsvSparseFormatter to format ISequences with one 
        /// line per sequence item. Useful for efficient serialization of sparse 
        /// sequences.
        /// </summary>
        /// <param name="separator_">Seprator character to be used between sequence item 
        /// position and its symbol.</param>
        /// <param name="sequenceIDPrefix_">The character to prefix the sequence start 
        /// line with.</param>
        public XsvSparseFormatter(char separator_, char sequenceIDPrefix_) 
        {
            Separator = separator_;
            SequenceIDPrefix = sequenceIDPrefix_;
        }

        #endregion


        #region Implementation of ISequenceFormatter

        /// <summary>
        ///             Writes an ISequence to the location specified by the writer.
        /// </summary>
        /// <param name="sequence">The sequence to format.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence text.</param>
        public void Format(ISequence sequence, TextWriter writer) 
        {
            Format(sequence, 0, writer);
        }

        /// <summary>
        /// Writes an ISequence to the location specified by the writer, 
        /// after adding an offset value to the position.
        /// 
        /// </summary>
        /// <param name="sequence">The sequence to format.</param>
        /// <param name="positionOffset">Adds this offset value to the item position within the sequence</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence text.</param>
        public void Format (ISequence sequence, int positionOffset, TextWriter writer) 
        {
            // Check input arguments
            if (sequence == null) 
            {
                throw new ArgumentNullException("sequence", "Sequence to be formatted cannot be null");
            }
            if (writer == null) 
            {
                throw new ArgumentNullException("writer", "Text writer to write formatted sequence into cannot be null");
            }

            // write the sequence start line
            writer.WriteLine(
                string.Format("{0}{1}{2}{3}{4}{5}", SequenceIDPrefix, positionOffset, Separator,
                              sequence.Count, Separator, sequence.ID).Replace('\n', ' '));

            // for sparse sequences, only write the non-null sequence items
            if (sequence is SparseSequence) 
            {
                foreach (IndexedItem<ISequenceItem> item in 
                    (sequence as SparseSequence).GetKnownSequenceItems()) 
                {
                    writer.WriteLine("{0}{1}{2}", item.Index, Separator, item.Item.Symbol);
                }
            }
            else // for non-sparse sequence, write all sequence items
            {
                for (int i = 0; i < sequence.Count; i++) 
                {
                    writer.WriteLine("{0}{1}{2}", i, Separator, sequence[i].Symbol);
                }
            }
        }


        /// <summary>
        ///             Writes an ISequence to the specified file.
        /// </summary>
        /// <param name="sequence">The sequence to format.</param>
        /// <param name="filename">The name of the file to write the formatted sequence text.</param>
        public void Format(ISequence sequence, string filename) 
        {
            // Check input arguments
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence", "Sequence to be formatted cannot be null");
            }
            if (filename == null) 
            {
                throw new ArgumentNullException("filename", "Filename of file to write formatted sequence into cannot be null");
            }

            using (StreamWriter sw = new StreamWriter(filename)) 
            {
                Format(sequence, sw);
            }
        }


        /// <summary>
        ///             Write a collection of ISequences to a writer.
        /// </summary>
        /// <param name="sequences">The sequences to write.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequences.</param>
        public void Format(ICollection<ISequence> sequences, TextWriter writer) 
        {
            foreach(ISequence sequence in sequences) 
            {
                Format(sequence, writer);
            }
        }


        /// <summary>
        ///             Write a collection of ISequences to a file.
        /// </summary>
        /// <param name="sequences">The sequences to write.</param>
        /// <param name="filename">The name of the file to write the formatted sequences.</param>
        public void Format(ICollection<ISequence> sequences, string filename) 
        {
            // Check input arguments
            if (filename == null) 
            {
                throw new ArgumentNullException("filename", "Filename of file to write formatted sequences into cannot be null");
            }

            using (StreamWriter sw = new StreamWriter(filename)) 
            {
                Format(sequences, sw);
            }
        }


        /// <summary>
        ///             Converts an ISequence to a formatted string.
        /// </summary>
        /// <param name="sequence">The sequence to format.</param>
        /// <returns>
        /// A string of the formatted text.
        /// </returns>
        public string FormatString(ISequence sequence) 
        {
            using(StringWriter sw = new StringWriter())
            {
                Format(sequence, sw);
                sw.Flush();
                return sw.ToString();
            }
        }


        /// <summary>
        ///             Gets the name of the sequence formatter being
        ///             implemented. This is intended to give the
        ///             developer some information of the formatter type.
        /// </summary>
        public string Name  { get { return "XsvSparseFormatter"; } }

        /// <summary>
        ///             Gets the description of the sequence formatter being
        ///             implemented. This is intended to give the
        ///             developer some information of the formatter.
        /// </summary>
        public string Description 
        {
            get { return "Sparse Sequence formatter to character separated value file"; }
        }

        /// <summary>
        ///             Gets the file extensions that the formatter implementation
        ///             will support.
        /// </summary>
        public string FileTypes 
        {
            get { return "csv,tsv"; }
        }

        #endregion
    }
}