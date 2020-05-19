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
using System.Text;

using MBF.Properties;
using MBF.Util;

namespace MBF.IO.GenBank
{
    /// <summary>
    /// Writes an ISequence to a particular location, usually a file. The output is formatted
    /// according to the GenBank file format. A method is also provided for quickly accessing
    /// the content in string form for applications that do not need to first write to file.
    /// </summary>
    public class GenBankFormatter : BasicSequenceFormatter
    {
        #region Fields

        // the standard indent for data is different from the indent for headers or data in the
        // features section
        private static readonly string _dataIndentString = Helper.StringMultiply(" ", 12);
        private static readonly string _featureHeaderIndentString = Helper.StringMultiply(" ", 5);
        private static readonly string _featureDataIndentString = Helper.StringMultiply(" ", 21);

        // the spec allows for up to 80 chars per line, but everyone else does 79
        private const int _maxLineLength = 79;

        // the sequence is output with each line containing 6 sets of 10 chars
        private const int _seqCharsPerChunk = 10;
        private const int _seqChunksPerLine = 6;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GenBankFormatter()
            : base()
        {
            LocationBuilder = new LocationBuilder();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Location builder is used to build location string from the location object persent in the feature items.
        /// By default an instance of LocationBuilder class is used to get the location string.
        /// </summary>
        public ILocationBuilder LocationBuilder { get; set; }
        #endregion

        #region BasicSequenceFormatter Members

        /// <summary>
        /// Gets the type of Formatter i.e GenBank.
        /// This is intended to give developers some information 
        /// of the formatter class.
        /// </summary>
        public override string Name
        {
            get
            {
                return Resource.GENBANK_NAME;
            }
        }

        /// <summary>
        /// Gets a comma seperated values of the possible
        /// file extensions for a GanBank file.
        /// </summary>
        public override string FileTypes
        {
            get
            {
                return Resource.GENBANK_FILEEXTENSION;
            }
        }

        /// <summary>
        /// Gets the description of GenBank formatter.
        /// This is intended to give developers some information 
        /// of the formatter class. This property returns a simple description of what the
        /// GenBankFormatter class acheives.
        /// </summary>
        public override string Description
        {
            get
            {
                return Resource.GENBANKFORMATTER_DESCRIPTION;
            }
        }

        /// <summary>
        /// Writes an ISequence to a GenBank file in the location specified by the writer.
        /// </summary>
        /// <param name="sequence">The sequence to format.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence text.</param>
        public override void Format(ISequence sequence, TextWriter writer)
        {
            WriteHeaders(sequence, writer);
            WriteFeatures(sequence, writer);
            WriteSequence(sequence, writer);

            writer.Flush();
        }

        #endregion

        #region Write Headers Members

        // Write all the header sections that come before the features section.
        private void WriteHeaders(ISequence sequence, TextWriter writer)
        {
            GenBankMetadata metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
            if (metadata != null)
            {
                WriteLocus(sequence, writer);
                WriteHeaderSection("DEFINITION", metadata.Definition, writer);

                if (metadata.Accession != null)
                {
                    WriteHeaderSection("ACCESSION", Helper.GetGenBankAccession(metadata.Accession), writer);

                    string version = "";
                    if (metadata.Version != null)
                    {
                        version = metadata.Accession.Primary + "." + metadata.Version.Version;

                        if (!string.IsNullOrEmpty(metadata.Version.GINumber))
                        {
                            version += "  GI:" + metadata.Version.GINumber;
                        }
                        if (version.Length > 0)
                        {
                            WriteHeaderSection("VERSION", version, writer);
                        }
                    }
                }

                if (metadata.Project != null)
                {
                    WriteHeaderSection("PROJECT", Helper.GetProjectIdentifier(metadata.Project), writer);
                }

                if (metadata.DBLink != null)
                {
                    WriteHeaderSection("DBLINK", Helper.GetCrossReferenceLink(metadata.DBLink), writer);
                }

                WriteHeaderSection("DBSOURCE", metadata.DBSource, writer);
                WriteHeaderSection("KEYWORDS", metadata.Keywords, writer);

                if (metadata.Segment != null)
                {
                    WriteHeaderSection("SEGMENT", Helper.GetSequenceSegment(metadata.Segment), writer);
                }

                WriteSource(metadata, writer);
                WriteReferences(metadata, writer);
                WriteComments(metadata, writer);
                WriteHeaderSection("PRIMARY", metadata.Primary, writer);

            }
        }

        private void WriteLocus(ISequence sequence, TextWriter writer)
        {
            // determine molecule and seqiemce type
            GenBankMetadata metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];

            GenBankLocusInfo locusInfo = null;
            if (metadata != null)
            {
                locusInfo = metadata.Locus;
            }

            string molType = sequence.MoleculeType.ToString();
            string seqType;
            if (sequence.MoleculeType != MoleculeType.Invalid)
            {
                if (molType == MoleculeType.Protein.ToString())
                {
                    seqType = "aa";
                    molType = string.Empty; // protein files don't use molecule type
                }
                else
                {
                    seqType = "bp";
                }
            }
            else
            {
                if (sequence.Alphabet == Alphabets.Protein)
                {
                    seqType = "aa";
                    molType = string.Empty; // protein files don't use molecule type
                }
                else
                {
                    seqType = "bp";

                    if (sequence.Alphabet == Alphabets.DNA)
                    {
                        molType = MoleculeType.DNA.ToString();
                    }
                    else
                    {
                        molType = MoleculeType.RNA.ToString();
                    }
                }
            }

            // retrieve metadata fields
            string strandType = string.Empty;
            string strandTopology = string.Empty;
            string division = string.Empty;
            DateTime date = DateTime.Now;

            if (locusInfo != null)
            {
                strandType = Helper.GetStrandType(locusInfo.Strand);

                strandTopology = Helper.GetStrandTopology(locusInfo.StrandTopology);
                if (locusInfo.DivisionCode != SequenceDivisionCode.None)
                {
                    division = locusInfo.DivisionCode.ToString();
                }

                date = locusInfo.Date;
            }

            writer.WriteLine("{0,-12}{1,-16} {2,11} {3} {4,3}{5,-6}  {6,-8} {7,3} {8}",
                "LOCUS",
                sequence.ID,
                sequence.Count,
                seqType,
                strandType,
                molType,
                strandTopology,
                division,
                date.ToString("dd-MMM-yyyy").ToUpper());
        }

        private void WriteSource(GenBankMetadata metadata, TextWriter writer)
        {
            if (metadata.Source != null)
            {
                string commonname = string.Empty;
                if (!string.IsNullOrEmpty(metadata.Source.CommonName))
                {
                    commonname = metadata.Source.CommonName;
                }

                WriteHeaderSection("SOURCE", commonname, writer);

                string organism = string.Empty;
                if (!string.IsNullOrEmpty(metadata.Source.Organism.Genus))
                {
                    organism += metadata.Source.Organism.Genus;
                }
                organism += " ";

                if (!string.IsNullOrEmpty(metadata.Source.Organism.Species))
                {
                    organism += metadata.Source.Organism.Species;
                }

                // Organism might be empty, trim the value to ensure that a string with one space is not written (writer fails on this)
                WriteHeaderSection("  ORGANISM", organism.Trim(), writer);
                WriteHeaderSection(string.Empty, metadata.Source.Organism.ClassLevels, writer);
            }
        }

        private void WriteReferences(GenBankMetadata metadata, TextWriter writer)
        {
            if (metadata.References != null)
            {
                foreach (CitationReference reference in metadata.References)
                {
                    // format the data for the first line
                    string data = reference.Number.ToString();
                    if (!string.IsNullOrEmpty(reference.Location))
                    {
                        data = data.PadRight(3) + "(" + reference.Location + ")";
                    }

                    WriteHeaderSection("REFERENCE", data, writer);
                    WriteHeaderSection("  AUTHORS", reference.Authors, writer);
                    WriteHeaderSection("  CONSRTM", reference.Consortiums, writer);
                    WriteHeaderSection("  TITLE", reference.Title, writer);
                    WriteHeaderSection("  JOURNAL", reference.Journal, writer);
                    WriteHeaderSection("  MEDLINE", reference.Medline, writer);
                    WriteHeaderSection("   PUBMED", reference.PubMed, writer);
                    WriteHeaderSection("  REMARK", reference.Remarks, writer);
                }
            }
        }

        // Writes the comments, which are stored in a list of strings.
        private void WriteComments(GenBankMetadata metadata, TextWriter writer)
        {
            foreach (string comment in metadata.Comments)
            {
                WriteHeaderSection("COMMENT", comment, writer);
            }
        }

        #endregion

        #region Write Features Methods

        private void WriteFeatures(ISequence sequence, TextWriter writer)
        {
            ILocationBuilder locBuilder = LocationBuilder;
            if (locBuilder == null)
            {
                throw new InvalidOperationException(Resource.NullLocationBuild);
            }
            GenBankMetadata metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
            if (metadata != null && metadata.Features != null)
            {
                WriteFeatureSection("FEATURES", "Location/Qualifiers", writer);

                // write the features in the order they were put in the list
                foreach (FeatureItem feature in metadata.Features.All)
                {
                    WriteFeatureSection(_featureHeaderIndentString + feature.Key, locBuilder.GetLocationString(feature.Location), writer);

                    // The sub-items of a feature are referred to as qualifiers.  These do not have
                    // unique keys, so they are stored as lists in the SubItems dictionary.
                    foreach (KeyValuePair<string, List<string>> qualifierList in feature.Qualifiers)
                    {
                        foreach (string qualifierValue in qualifierList.Value)
                        {
                            string data = "/" + qualifierList.Key;

                            if (qualifierValue != string.Empty)
                            {
                                data += "=" + qualifierValue;
                            }

                            // use a blank header; the qualifier key is part of the data
                            WriteFeatureSection(string.Empty, data, writer);
                        }
                    }
                }
            }
        }

        // Writes a header and data string as a GenBank feature section, indenting the data of
        // each line to the standard feature indent.
        private void WriteFeatureSection(string header, string data, TextWriter writer)
        {
            WriteGenBankSection(header, _featureDataIndentString, data, writer);
        }

        #endregion

        #region Write Sequence Methods

        // Write the sequence and other post-features data.
        private void WriteSequence(ISequence sequence, TextWriter writer)
        {
            // "BASE COUNT" is stored as "baseCount", not "base count"
            GenBankMetadata metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
            if (metadata != null && !string.IsNullOrEmpty(metadata.BaseCount))
            {
                writer.WriteLine("BASE COUNT  " + metadata.BaseCount);
            }

            if (metadata != null && !string.IsNullOrEmpty(metadata.Contig))
            {
                WriteHeaderSection("CONTIG", metadata.Contig, writer);
            }

            if (sequence.Count > 0)
            {
                if (metadata != null && !string.IsNullOrEmpty(metadata.Origin))
                {
                    WriteHeaderSection("ORIGIN", metadata.Origin, writer);
                }
                else
                {
                    // always write at least a data-less origin line before the sequence, even
                    // if we don't have an origin stored in metadata
                    writer.WriteLine("ORIGIN");
                }

                WriteGenBankSequence(sequence, writer);
            }

            writer.WriteLine("//");
        }

        // Output 6 groups of 10 symbols per line.
        private void WriteGenBankSequence(ISequence sequence, TextWriter writer)
        {
            bool done = false;
            int symbolIndex = 0;
            while (!done)
            {
                // start each line with the symbol number
                StringBuilder line = new StringBuilder(string.Format("{0,9}", symbolIndex + 1));

                // next add 6 groups of 10, with groups separated by spaces
                for (int chunkIndex = 0; chunkIndex < _seqChunksPerLine && !done; chunkIndex++)
                {
                    // set done = true if this is the last chunk
                    done = _seqCharsPerChunk >= sequence.Count - symbolIndex;
                    int chunkSize = done ? sequence.Count - symbolIndex : _seqCharsPerChunk;

                    // append the chunk
                    line.Append(" ");
                    for (int start = symbolIndex; symbolIndex < start + chunkSize; symbolIndex++)
                    {
                        line.Append(sequence[symbolIndex].Symbol);
                    }
                }

                writer.WriteLine(line.ToString().ToLower());
            }
        }

        #endregion

        #region General Helper Methods

        // Writes a header and data string as a GenBank header section, indenting the data of
        // each line to the standard header indent.
        private void WriteHeaderSection(string header, string data, TextWriter writer)
        {
            if (data != null)
            {
                WriteGenBankSection(header, _dataIndentString, data, writer);
            }
        }

        /// Writes a header and data string as a GenBank header section, indenting the data of
        /// each line to the length of the given indent string.
        private void WriteGenBankSection(string header, string indentString, string data, TextWriter writer)
        {
            int maxLineDataLength = _maxLineLength - indentString.Length;
            bool firstLine = true;

            // process the data by chunks using any line breaks it already contains
            foreach (string dataChunk in data.Split('\r', '\n'))
            {
                int lineDataLength = 0;
                for (int lineStart = 0; lineStart < dataChunk.Length; lineStart += lineDataLength)
                {
                    // skip spaces at start of this line of data
                    while (dataChunk[lineStart] == ' ')
                    {
                        lineStart++;
                    }

                    // use the header for the first line, and the indent string for subsequent
                    // lines, appending the data
                    string beforeData;
                    if (firstLine)
                    {
                        beforeData = header.PadRight(indentString.Length);
                        firstLine = false;
                    }
                    else
                    {
                        beforeData = indentString;
                    }

                    // check if the rest of this chunk will fit on one line
                    if (lineStart + maxLineDataLength >= dataChunk.Length)
                    {
                        // the rest of the chunk will be written to this line
                        lineDataLength = dataChunk.Length - lineStart;
                    }
                    else
                    {
                        // use the last space in the first maxLineDataLength characters as the line
                        // break; the startIndex for LastIndexOf actually needs to equal the end of
                        // the substring being examined - not intuitive
                        int startIndex = lineStart + maxLineDataLength;
                        int lineBreak = dataChunk.LastIndexOf(' ', startIndex, maxLineDataLength);

                        // if we didn't find a space, look for assorted other punctuation
                        if (lineBreak == -1)
                        {
                            // move the start index back 1; we'll include any non-space break
                            // char on the same line
                            startIndex--;

                            // try commas and semi-colons first
                            lineBreak = dataChunk.LastIndexOfAny(
                                new char[] { ',', ';' }, startIndex, maxLineDataLength);

                            // next try periods and dashes
                            if (lineBreak == -1)
                            {
                                lineBreak = dataChunk.LastIndexOfAny(
                                    new char[] { '.', '-' }, startIndex, maxLineDataLength);
                            }

                            // include the break char if we found one
                            if (lineBreak != -1)
                            {
                                lineBreak++;
                            }
                        }

                        // use the line break to determine the length of the data to be written into
                        // this line; if no good place to break was found in the first
                        // maxLineDataLength characters, use maxLineDataLength
                        lineDataLength = (lineBreak == -1 ? maxLineDataLength : lineBreak - lineStart);
                    }

                    writer.WriteLine(beforeData + dataChunk.Substring(lineStart, lineDataLength));
                }
            }
        }

        #endregion
    }
}
