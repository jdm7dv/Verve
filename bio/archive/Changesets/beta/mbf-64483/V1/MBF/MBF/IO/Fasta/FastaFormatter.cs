// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.IO;

using MBF.Properties;

namespace MBF.IO.Fasta
{
    /// <summary>
    /// Writes an ISequence to a particular location, usually a file. The output is formatted
    /// according to the FASTA file format. A method is also provided for quickly accessing
    /// the content in string form for applications that do not need to first write to file.
    /// </summary>
    public class FastaFormatter : BasicSequenceFormatter
    {
        #region Fields

        // FASTA preferred number of characters per line, used when writing sequence data
        private const int _maxLineLength = 80;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public FastaFormatter()
            : base()
        {
        }

        #endregion

        #region BasicSequenceFormatter Members

        /// <summary>
        /// Gets the type of Formatter i.e FASTA.
        /// This is intended to give developers some information 
        /// of the formatter class.
        /// </summary>
        public override string Name
        {
            get
            {
                return Resource.FASTA_NAME;
            }
        }

        /// <summary>
        /// Gets the description of Fasta formatter.
        /// This is intended to give developers some information 
        /// of the formatter class. This property returns a simple description of what the
        /// FastaFormatter class acheives.
        /// </summary>
        public override string Description
        {
            get
            {
                return Resource.FASTAFORMATTER_DESCRIPTION;
            }
        }

        /// <summary>
        /// Gets a comma seperated values of the possible
        /// file extensions for a FASTA file.
        /// </summary>
        public override string FileTypes
        {
            get
            {
                return Resource.FASTA_FILEEXTENSION;
            }
        }

        /// <summary>
        /// Writes an ISequence to a FASTA file in the location specified by the writer.
        /// </summary>
        /// <param name="sequence">The sequence to format.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence text.</param>
        public override void Format(ISequence sequence, TextWriter writer)
        {
            // write header
            writer.WriteLine(">" + sequence.ID);

            // write sequence
            BasicDerivedSequence derivedSeq = new BasicDerivedSequence(sequence, false, false, 0, 0);
            for (int lineStart = 0; lineStart < sequence.Count; lineStart += _maxLineLength)
            {
                derivedSeq.RangeStart = lineStart;
                derivedSeq.RangeLength = Math.Min(_maxLineLength, sequence.Count - lineStart);
                writer.WriteLine(derivedSeq.ToString());
            }

            writer.Flush();
        }

        #endregion
    }
}
