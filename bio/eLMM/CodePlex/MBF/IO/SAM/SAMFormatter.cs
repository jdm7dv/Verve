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



using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bio.Algorithms.Alignment;
using Bio.Properties;
using Bio.Util;

namespace Bio.IO.SAM
{
    /// <summary>
    /// Writes a SequenceAlignmentMap to a particular location, usually a file. 
    /// The output is formatted according to the SAM file format. 
    /// A method is also provided for quickly accessing the content in string 
    /// form for applications that do not need to first write to file.
    /// Documentation for the latest BAM file format can be found at
    /// http://samtools.sourceforge.net/SAM1.pdf
    /// </summary>
    public class SAMFormatter : ISequenceAlignmentFormatter
    {
        #region Properties
        /// <summary>
        /// Gets the name of the sequence alignment formatter being
        /// implemented. This is intended to give the developer some
        /// information of the formatter type.
        /// </summary>
        public string Name
        {
            get
            {
                return Resource.SAM_NAME;
            }
        }

        /// <summary>
        /// Gets the description of the sequence alignment formatter being
        /// implemented. This is intended to give the developer some 
        /// information of the formatter.
        /// </summary>
        public string Description
        {
            get
            {
                return Resource.SAMFORMATTER_DESCRIPTION;
            }
        }

        /// <summary>
        /// Gets the file extensions that the formatter implementation
        /// will support.
        /// </summary>
        public string FileTypes
        {
            get
            {
                return Resource.SAM_FILEEXTENSION;
            }
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Writes specified SAMAlignedHeader to specified text writer.
        /// </summary>
        /// <param name="header">Header to write.</param>
        /// <param name="writer">Text writer.</param>
        public static void WriteHeader(SAMAlignmentHeader header, TextWriter writer)
        {
            if (header == null)
            {
                return;
            }

            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            string message = header.IsValid();
            if (!string.IsNullOrEmpty(message))
            {
                throw new ArgumentException(message);
            }

            StringBuilder headerLine = null;
            for (int i = 0; i < header.RecordFields.Count; i++)
            {
                headerLine = new StringBuilder();
                headerLine.Append("@");
                headerLine.Append(header.RecordFields[i].Typecode);
                for (int j = 0; j < header.RecordFields[i].Tags.Count; j++)
                {
                    headerLine.Append("\t");
                    headerLine.Append(header.RecordFields[i].Tags[j].Tag);
                    headerLine.Append(":");
                    headerLine.Append(header.RecordFields[i].Tags[j].Value);
                }

                writer.WriteLine(headerLine.ToString());
            }

            writer.Flush();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Writes an ISequenceAlignment to the location specified by the writer.
        /// </summary>
        /// <param name="sequenceAlignment">The sequence alignment to format.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence alignment text.</param>
        public void Format(ISequenceAlignment sequenceAlignment, TextWriter writer)
        {
            string message = string.Empty;
            if (sequenceAlignment == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameSequenceAlignment);
            }

            if (writer == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameWriter);
            }

            #region Write alignment header
            SAMAlignmentHeader header = sequenceAlignment.Metadata[Helper.SAMAlignmentHeaderKey] as SAMAlignmentHeader;
            if (header != null)
            {
                WriteHeader(header, writer);
            }

            #endregion

            #region Write aligned sequences
            foreach (IAlignedSequence alignedSequence in sequenceAlignment.AlignedSequences)
            {
                SAMAlignedSequenceHeader alignedHeader = alignedSequence.Metadata[Helper.SAMAlignedSequenceHeaderKey] as SAMAlignedSequenceHeader;
                if (alignedHeader == null)
                {
                    throw new ArgumentException(Resource.SAM_AlignedSequenceHeaderMissing);
                }

                StringBuilder alignmentLine = new StringBuilder();
                message = alignedHeader.IsValid();
                if (!string.IsNullOrEmpty(message))
                {
                    throw new ArgumentException(message);
                }

                alignmentLine.Append(alignedHeader.QName);
                alignmentLine.Append("\t");
                alignmentLine.Append((int)alignedHeader.Flag);
                alignmentLine.Append("\t");
                alignmentLine.Append(alignedHeader.RName);
                alignmentLine.Append("\t");
                alignmentLine.Append(alignedHeader.Pos);
                alignmentLine.Append("\t");
                alignmentLine.Append(alignedHeader.MapQ);
                alignmentLine.Append("\t");
                alignmentLine.Append(alignedHeader.CIGAR);
                alignmentLine.Append("\t");

                if (string.Compare(alignedHeader.MRNM, alignedHeader.RName, StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    alignmentLine.Append("=");
                }
                else
                {
                    alignmentLine.Append(alignedHeader.MRNM);
                }

                alignmentLine.Append("\t");
                alignmentLine.Append(alignedHeader.MPos);
                alignmentLine.Append("\t");
                alignmentLine.Append(alignedHeader.ISize);
                alignmentLine.Append("\t");
                writer.Write(alignmentLine.ToString());
                List<int> dotSymbolIndices = new List<int>(alignedHeader.DotSymbolIndices);
                List<int> equalSymbolIndices = new List<int>(alignedHeader.EqualSymbolIndices);

                if (alignedSequence.Sequences.Count > 0 && alignedSequence.Sequences[0] != null)
                {
                    ISequence seq = alignedSequence.Sequences[0];

                    if (seq.Alphabet != Alphabets.DNA)
                    {
                        throw new ArgumentException(Resource.SAMFormatterSupportsDNAOnly);
                    }

                    for (int i = 0; i < seq.Count; i++)
                    {
                        char symbol = seq[i].Symbol;

                        if (dotSymbolIndices.Count > 0)
                        {
                            if (dotSymbolIndices.Contains(i))
                            {
                                symbol = '.';
                                dotSymbolIndices.Remove(i);
                            }
                        }

                        if (equalSymbolIndices.Count > 0)
                        {
                            if (equalSymbolIndices.Contains(i))
                            {
                                symbol = '=';
                                equalSymbolIndices.Remove(i);
                            }
                        }

                        writer.Write(symbol);
                    }

                    writer.Write("\t");

                    IQualitativeSequence qualSeq = seq as IQualitativeSequence;
                    if (qualSeq != null)
                    {
                        writer.Write(ASCIIEncoding.ASCII.GetString(qualSeq.Scores));
                    }
                    else
                    {
                        writer.Write("*");
                    }
                }
                else
                {
                    writer.Write("*");
                    writer.Write("\t");
                    writer.Write("*");
                }

                foreach (SAMOptionalField field in alignedHeader.OptionalFields)
                {
                    writer.Write("\t");
                    writer.Write(field.Tag);
                    writer.Write(":");
                    writer.Write(field.VType);
                    writer.Write(":");
                    writer.Write(field.Value);
                }

                writer.WriteLine();
            }
            #endregion

            writer.Flush();
        }

        /// <summary>
        /// Writes an ISequenceAlignment to the specified file.
        /// </summary>
        /// <param name="sequenceAlignment">The sequence alignment to format.</param>
        /// <param name="filename">The name of the file to write the formatted sequence alignment text.</param>
        public void Format(ISequenceAlignment sequenceAlignment, string filename)
        {
            if (sequenceAlignment == null)
            {
                throw new ArgumentNullException("sequenceAlignment");
            }

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }

            using (TextWriter writer = new StreamWriter(filename))
            {
                Format(sequenceAlignment, writer);
            }
        }

        /// <summary>
        /// Writes an SequenceAlignmentMap to the specified file.
        /// </summary>
        /// <param name="sequenceAlignmentMap">SequenceAlignmentMap object to format.</param>
        /// <param name="filename">The name of the file to write the formatted sequence alignment text.</param>
        public void Format(SequenceAlignmentMap sequenceAlignmentMap, string filename)
        {
            if (sequenceAlignmentMap == null)
            {
                throw new ArgumentNullException("sequenceAlignmentMap");
            }

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException("filename");
            }

            Format(sequenceAlignmentMap as ISequenceAlignment, filename);
        }

        /// <summary>
        /// Writes an sequenceAlignmentMap to the location specified by the writer.
        /// </summary>
        /// <param name="sequenceAlignmentMap">SequenceAlignmentMap object to format.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence alignment text.</param>
        public void Format(SequenceAlignmentMap sequenceAlignmentMap, TextWriter writer)
        {
            if (sequenceAlignmentMap == null)
            {
                throw new ArgumentNullException("sequenceAlignmentMap");
            }

            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            Format(sequenceAlignmentMap as ISequenceAlignment, writer);
        }

        /// <summary>
        /// Write a collection of ISequenceAlignments to a writer.
        /// Note that SAM format supports only one ISequenceAlignment object per file.
        /// Thus first ISequenceAlignment in the collection will be written to the file.
        /// </summary>
        /// <param name="sequenceAlignments">The sequence alignments to write.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence alignments.</param>
        public void Format(ICollection<ISequenceAlignment> sequenceAlignments, TextWriter writer)
        {
            throw new NotSupportedException(Resource.SAM_FormatMultipleAlignmentsNotSupported);
        }

        /// <summary>
        /// Write a collection of ISequenceAlignments to a file.
        /// </summary>
        /// <param name="sequenceAlignments">The sequenceAlignments to write.</param>
        /// <param name="filename">The name of the file to write the formatted sequence alignments.</param>
        public void Format(ICollection<ISequenceAlignment> sequenceAlignments, string filename)
        {
            throw new NotSupportedException(Resource.SAM_FormatMultipleAlignmentsNotSupported);
        }

        /// <summary>
        /// Converts an ISequenceAlignment to a formatted string.
        /// </summary>
        /// <param name="sequenceAlignment">The sequence alignment to format.</param>
        /// <returns>A string of the formatted text.</returns>
        public string FormatString(ISequenceAlignment sequenceAlignment)
        {
            if (sequenceAlignment == null)
            {
                throw new ArgumentNullException("sequenceAlignment");
            }

            using (TextWriter writer = new StringWriter())
            {
                Format(sequenceAlignment, writer);
                return writer.ToString();
            }
        }
        #endregion
    }
}
