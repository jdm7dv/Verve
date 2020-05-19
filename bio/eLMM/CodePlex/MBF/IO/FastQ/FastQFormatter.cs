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
using Bio.Properties;

namespace Bio.IO.FastQ
{
    /// <summary>
    /// Writes an IQualitativeSequence to a particular location, usually a file. The output is formatted
    /// according to the FastQ file format. A method is also provided for quickly accessing
    /// the content in string form for applications that do not need to first write to file.
    /// </summary>
    public class FastQFormatter : BasicSequenceFormatter
    {
        #region Properties
        /// <summary>
        /// Gets the name of Formatter i.e FastQ.
        /// This is intended to give developers some information 
        /// of the formatter class.
        /// </summary>
        public override string Name
        {
            get { return Resource.FASTQ_NAME; }
        }

        /// <summary>
        /// Gets the description of FastQ formatter.
        /// This is intended to give developers some information 
        /// of the formatter class. This property returns a simple description of what the
        /// FastQFormatter class acheives.
        /// </summary>
        public override string Description
        {
            get { return Resource.FASTQFORMATTER_DESCRIPTION; }
        }

        /// <summary>
        /// Gets a comma seperated values of the possible
        /// file extensions for a FASTQ file.
        /// </summary>
        public override string FileTypes
        {
            get { return Resource.FASTQ_FILEEXTENSION; }
        }
        #endregion Properties

        #region Methods
        /// <summary>
        /// Writes an IQualitativeSequence to a FASTQ file in the location specified by the writer.
        /// </summary>
        /// <param name="sequence">The QualitativeSequence to format.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence text.</param>
        public override void Format(ISequence sequence, TextWriter writer)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameSequence);
            }

            IQualitativeSequence qualSequence = sequence as IQualitativeSequence;
            if (qualSequence == null)
            {
                throw new ArgumentException(Resource.FastQ_InvalidSequence);
            }

            if (writer == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameWriter);
            }

            Format(qualSequence, writer);
        }

        /// <summary>
        /// Writes an IQualitativeSequence to the specified file.
        /// </summary>
        /// <param name="qualSequence">The QualitativeSequence to format.</param>
        /// <param name="fileName">The name of the file to write the formatted sequence text.</param>
        public void Format(IQualitativeSequence qualSequence, string fileName)
        {
            using (TextWriter writer = new StreamWriter(fileName))
            {
                Format(qualSequence, writer);
            }
        }

        /// <summary>
        /// Writes a list of IQualitativeSequence to the specified file.
        /// </summary>
        /// <param name="qualSequences">List of IQualitativeSequence to format.</param>
        /// <param name="fileName">The name of the file to write the formatted sequence text.</param>
        public void Format(ICollection<IQualitativeSequence> qualSequences, string fileName)
        {
            using (TextWriter writer = new StreamWriter(fileName))
            {
                Format(qualSequences, writer);
            }
        }

        /// <summary>
        /// Writes a list of IQualitativeSequence to the specified text writer.
        /// </summary>
        /// <param name="qualSequences">List of IQualitativeSequence to format.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence text.</param>
        public void Format(ICollection<IQualitativeSequence> qualSequences, TextWriter writer)
        {
            foreach (IQualitativeSequence qualSequence in qualSequences)
            {
                Format(qualSequence, writer);
            }
        }

        /// <summary>
        /// Writes an IQualitativeSequence to the specified text writer.
        /// </summary>
        /// <param name="qualSequence">The QualitativeSequence to format.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence text.</param>
        public void Format(IQualitativeSequence qualSequence, TextWriter writer)
        {
            if (qualSequence == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameQualSequence);
            }

            if (writer == null)
            {
                throw new ArgumentNullException(Resource.ParameterNameWriter);
            }

            string header = qualSequence.ID;
            string lengthStr = " length=";

            if (qualSequence.ID.Contains(lengthStr))
            {
                int startIndex = qualSequence.ID.LastIndexOf(lengthStr);
                header = header.Substring(0, startIndex + 8) + qualSequence.Count;
            }

            writer.WriteLine("@" + header);
            writer.WriteLine(qualSequence.ToString());
            writer.WriteLine("+" + header);
            writer.WriteLine(ASCIIEncoding.ASCII.GetString(qualSequence.Scores));
            writer.Flush();
        }

        /// <summary>
        /// Converts an IQualitativeSequence to a formatted text.
        /// </summary>
        /// <param name="qualSequence">The QualitativeSequence to format.</param> 
        /// <returns>A string of the formatted text.</returns>
        public string FormatString(IQualitativeSequence qualSequence)
        {
            using (TextWriter writer = new StringWriter())
            {
                Format(qualSequence, writer);
                return writer.ToString();
            }
        }
        #endregion Methods
    }
}
