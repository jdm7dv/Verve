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

using Bio.Properties;

namespace Bio.IO.Gff
{
    /// <summary>
    /// Writes an ISequence to a particular location, usually a file. The output is formatted
    /// according to the GFF file format. A method is also provided for quickly accessing
    /// the content in string form for applications that do not need to first write to file.
    /// </summary>
    public class GffFormatter : BasicSequenceFormatter
    {
        #region Fields

        private const string _headerMark = "##";

        // the spec 32k chars per line, but it shows only 37 sequence symbols per line
        // in the examples
        private const int _maxSequenceSymbolsPerLine = 37;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GffFormatter()
            : base()
        {
            ShouldWriteSequenceData = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Whether or not sequence data will be written as part of the GFF header information;
        /// defaults to false, since GFF files normally do not contain this data.
        /// </summary>
        public bool ShouldWriteSequenceData { get; set; }

        #endregion

        #region BasicSequenceFormatter Members

        /// <summary>
        /// Gets the type of Formatter i.e GFF.
        /// This is intended to give developers some information 
        /// of the formatter class.
        /// </summary>
        public override string Name
        {
            get
            {
                return Resource.GFF_NAME;
            }
        }

        /// <summary>
        /// Gets a comma seperated values of the possible
        /// file extensions for a GFF file.
        /// </summary>
        public override string FileTypes
        {
            get
            {
                return Resource.GFF_FILEEXTENSION;
            }
        }

        /// <summary>
        /// Gets the description of GFF formatter.
        /// This is intended to give developers some information 
        /// of the formatter class. This property returns a simple description of what the
        /// GffFormatter class acheives.
        /// </summary>
        public override string Description
        {
            get
            {
                return Resource.GFFFORMATTER_DESCRIPTION;
            }
        }

        /// <summary>
        /// Write a collection of ISequences to a file.
        /// </summary>
        /// <remarks>
        /// This method is overridden to format file-scope metadata that applies to all
        /// metadata that applies to all of the sequences in the file.
        /// </remarks>
        /// <param name="sequences">The sequences to write</param>
        /// <param name="writer">the TextWriter</param>
        public override void Format(ICollection<ISequence> sequences, TextWriter writer)
        {
            WriteHeaders(sequences, writer);

            foreach (ISequence sequence in sequences)
            {
                WriteFeatures(sequence, writer);
            }

            writer.Flush();
        }

        /// <summary>
        /// Writes an ISequence to a GenBank file in the location specified by the writer.
        /// </summary>
        /// <param name="sequence">The sequence to format.</param>
        /// <param name="writer">The TextWriter used to write the formatted sequence text.</param>
        public override void Format(ISequence sequence, TextWriter writer)
        {
            WriteHeaders(new List<ISequence> { sequence }, writer);
            WriteFeatures(sequence, writer);

            writer.Flush();
        }

        #endregion

        #region Private Methods

        // The headers for all sequences go at the top of the file before any features.
        private void WriteHeaders(ICollection<ISequence> sequenceList, TextWriter writer)
        {
            // look for file-scope data tha is common to all sequences; null signifies no match
            string source = null;
            string version = null;
            string type = null;
            bool firstSeq = true;
            foreach (ISequence sequence in sequenceList)
            {
                if (firstSeq)
                {
                    // source and version go together; can't output one without the other
                    if (sequence.Metadata.ContainsKey("source") && sequence.Metadata.ContainsKey("version"))
                    {
                        source = sequence.Metadata["source"] as string;
                        version = sequence.Metadata["version"] as string;
                    }

                    // map to generic string; e.g. mRNA, tRNA -> RNA
                    type = GetGenericTypeString(sequence.MoleculeType);

                    firstSeq = false;
                }
                else
                {
                    // source and version go together; can't output one without the other
                    if (source != null)
                    {
                        bool sourceAndVersionMatchOthers =
                            sequence.Metadata.ContainsKey("source") &&
                            sequence.Metadata.ContainsKey("version") &&
                            source == sequence.Metadata["source"] as string &&
                            version == sequence.Metadata["version"] as string;

                        // set both to null if this seq source and version don't match previous ones
                        if (!sourceAndVersionMatchOthers)
                        {
                            source = null;
                            version = null;
                        }
                    }

                    // set type to null if this seq type doesn't match previous types
                    if (type != null && type != GetGenericTypeString(sequence.MoleculeType))
                    {
                        type = null;
                    }
                }
            }

            // formatting using gff version 2
            WriteHeaderLine(writer, "gff-version", "2");

            // only output source if they all match
            if (source != null)
            {
                WriteHeaderLine(writer, "source-version", source, version);
            }

            // today's date
            WriteHeaderLine(writer, "date", DateTime.Today.ToString("yyyy-MM-dd"));

            // type header
            if (type == null)
            {
                foreach (ISequence sequence in sequenceList)
                {
                    type = GetGenericTypeString(sequence.MoleculeType);

                    // only ouput seq-specific type header if this seq won't have its type
                    // output as part of a sequence data header; don't need to output if DNA,
                    // as DNA is default
                    if (type != MoleculeType.DNA.ToString() &&
                        (!ShouldWriteSequenceData || sequence.Count == 0))
                    {
                        WriteHeaderLine(writer, "type", type, sequence.DisplayID);
                    }
                }
            }
            else
            {
                // output that the types all match; don't need to output if DNA, as DNA is default
                if (type != MoleculeType.DNA.ToString())
                {
                    WriteHeaderLine(writer, "type", type);
                }
            }

            // sequence data
            if (ShouldWriteSequenceData)
            {
                foreach (ISequence sequence in sequenceList)
                {
                    if (sequence.Count > 0)
                    {
                        type = GetGenericTypeString(sequence.MoleculeType);

                        WriteHeaderLine(writer, type, sequence.DisplayID);

                        BasicDerivedSequence derivedSeq = new BasicDerivedSequence(sequence, false, false, 0, 0);
                        for (int lineStart = 0; lineStart < sequence.Count; lineStart += _maxSequenceSymbolsPerLine)
                        {
                            derivedSeq.RangeStart = lineStart;
                            derivedSeq.RangeLength = Math.Min(_maxSequenceSymbolsPerLine, sequence.Count - lineStart);
                            WriteHeaderLine(writer, derivedSeq.ToString().ToLower());
                        }

                        WriteHeaderLine(writer, "end-" + type);
                    }
                }
            }

            // sequence-region header
            foreach (ISequence sequence in sequenceList)
            {
                if (sequence.Metadata.ContainsKey("start") && sequence.Metadata.ContainsKey("end"))
                {
                    WriteHeaderLine(writer, "sequence-region", sequence.DisplayID,
                        sequence.Metadata["start"] as string, sequence.Metadata["end"] as string);
                }
            }
        }

        // Returns "DNA", "RNA", "Protein", or null.
        private string GetGenericTypeString(MoleculeType type)
        {
            string typeString = null;

            switch (type)
            {
                case MoleculeType.DNA:
                    typeString = MoleculeType.DNA.ToString();
                    break;
                case MoleculeType.RNA:
                case MoleculeType.tRNA:
                case MoleculeType.rRNA:
                case MoleculeType.mRNA:
                case MoleculeType.uRNA:
                case MoleculeType.snRNA:
                case MoleculeType.snoRNA:
                    typeString = MoleculeType.RNA.ToString();
                    break;
                case MoleculeType.Protein:
                    typeString = MoleculeType.Protein.ToString();
                    break;
            }

            return typeString;
        }

        private void WriteHeaderLine(TextWriter writer, string key, params string[] dataFields)
        {
            string headerLine = _headerMark + key;

            foreach (string field in dataFields)
            {
                headerLine += " " + field;
            }

            writer.WriteLine(headerLine);
        }

        // Skips the sequence if it has no features, and skips any features that don't
        // have all the mandatory fields.
        private void WriteFeatures(ISequence sequence, TextWriter writer)
        {
            if (sequence.Metadata.ContainsKey("features"))
            {
                foreach (MetadataListItem<List<string>> feature in
                    sequence.Metadata["features"] as List<MetadataListItem<List<string>>>)
                {
                    // only write the line if we have all the mandatory fields
                    if (feature.SubItems.ContainsKey("source") &&
                        feature.SubItems.ContainsKey("start") &&
                        feature.SubItems.ContainsKey("end"))
                    {
                        string featureLine = sequence.DisplayID;

                        featureLine += GetSubItemString(feature, "source");
                        featureLine += "\t" + feature.Key;
                        featureLine += GetSubItemString(feature, "start");
                        featureLine += GetSubItemString(feature, "end");
                        featureLine += GetSubItemString(feature, "score");
                        featureLine += GetSubItemString(feature, "strand");
                        featureLine += GetSubItemString(feature, "frame");

                        // optional attributes field is stored as free text
                        if (feature.FreeText != string.Empty)
                        {
                            featureLine += "\t" + feature.FreeText;
                        }

                        writer.WriteLine(featureLine);
                    }
                }
            }
        }

        // Returns a tab plus the sub-item text or a "." if the sub-item is absent.
        private string GetSubItemString(MetadataListItem<List<string>> feature, string subItemName)
        {
            return "\t" + (feature.SubItems.ContainsKey(subItemName) ? feature.SubItems[subItemName][0] : ".");
        }

        #endregion
    }
}
