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

using MBF.Properties;
using System.Globalization;

namespace MBF.IO.Gff
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
        private const string _sourceVersionKey = "SOURCE-VERSION";
        private const string _sourceVersionLowercaseKey = "source-version";
        private const string _sourceKey = "source";
        private const string _versionKey = "version";
        private const string _typeKey = "TYPE";
        private const string _typeLowercaseKey = "type";
        private const string _multiTypeKey = "TYPE_";
        private const string _multiSeqDataKey = "SEQDATA_";
        private const string _multiSeqRegKey = "SEQUENCE-REGION_";
        private const string _commentSectionKey = "COMMENTSECTION_";
        private const string _gffVersionLowercaseKey = "gff-version";
        private const string _gffVersionKey = "GFF-VERSION";

        private const string _dateKey = "DATE";
        private const string _dateLowercaseKey = "date";
        private const string _seqRegKey = "sequence-region";
        private const string _startKey = "start";
        private const string _endKey = "end";
        private const string _scoreKey = "score";
        private const string _strandKey = "strand";
        private const string _frameKey = "frame";
        private const string _featuresKey = "features";

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
            ShouldWriteSequenceData = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Whether or not sequence data will be written as part of the GFF header information;
        /// This property is required as GFF files normally do not contain sequence data.
        /// Defaults value is true.
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
            MetadataListItem<string> sourceVersion = null;
            string source = null;
            string version = null;
            string type = null;
            bool firstSeq = true;
            ISequence commonSeq = null;
            List<string> typeExceptionList = new List<string>();
            List<string> seqDataExceptionList = new List<string>();
            List<string> seqRegExceptionList = new List<string>();

            foreach (ISequence sequence in sequenceList)
            {
                if (firstSeq)
                {
                    // consider first seq for common metadata.
                    commonSeq = sequence;

                    object tmpobj;
                    // source and version go together; can't output one without the other
                    if (sequence.Metadata.TryGetValue(_sourceVersionKey, out tmpobj))
                    {
                        sourceVersion = tmpobj as MetadataListItem<string>;
                        if (sourceVersion != null && sourceVersion.SubItems.Count > 1)
                        {
                            source = sourceVersion.SubItems[_sourceKey];
                            version = sourceVersion.SubItems[_versionKey];
                        }
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
                        bool sourceAndVersionMatchOthers = false;

                        object tmpobj;
                        // source and version go together; can't output one without the other
                        if (sequence.Metadata.TryGetValue(_sourceVersionKey, out tmpobj))
                        {
                            sourceVersion = tmpobj as MetadataListItem<string>;
                            if (sourceVersion != null && sourceVersion.SubItems.Count > 1)
                            {
                                sourceAndVersionMatchOthers = source == sourceVersion.SubItems[_sourceKey] &&
                                version == sourceVersion.SubItems[_versionKey];
                            }
                        }

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

            if (commonSeq == null)
            {
                commonSeq = new Sequence(Alphabets.DNA);
            }

            WriteCommonMetadata(commonSeq, sequenceList, writer, source, version, type, 1);

            int totalTypeCount = commonSeq.Metadata.Keys.Count(K => K.ToUpperInvariant().Contains(_multiTypeKey));
            int currentTypeCount = 0;
            int totalSeqData = commonSeq.Metadata.Keys.Count(K => K.ToUpperInvariant().Contains(_multiSeqDataKey));
            int totalSeqRegs = commonSeq.Metadata.Keys.Count(K => K.ToUpperInvariant().Contains(_multiSeqRegKey));

            ISequence seq = null;
            foreach (string key in commonSeq.Metadata.Keys)
            {
                string keyToCompare = key.ToUpperInvariant();
                string value = string.Empty;

                if (keyToCompare.Contains(_commentSectionKey))
                {
                    keyToCompare = _commentSectionKey;
                    value = commonSeq.Metadata[key] as string;
                }

                if (keyToCompare.Contains(_multiTypeKey))
                {
                    keyToCompare = _multiTypeKey;
                    value = commonSeq.Metadata[key] as string;
                }

                if (keyToCompare.Contains(_multiSeqDataKey))
                {
                    keyToCompare = _multiSeqDataKey;
                    value = commonSeq.Metadata[key] as string;
                }

                if (keyToCompare.Contains(_multiSeqRegKey))
                {
                    keyToCompare = _multiSeqRegKey;
                    value = commonSeq.Metadata[key] as string;
                }

                switch (keyToCompare)
                {
                    case _commentSectionKey:
                        writer.WriteLine(value);
                        break;

                    case _gffVersionKey:
                        // formatting using gff version 2
                        WriteHeaderLine(writer, _gffVersionLowercaseKey, "2");
                        WriteCommonMetadata(commonSeq, sequenceList, writer, source, version, type, 2);
                        break;

                    case _sourceVersionKey:

                        // only output source if they all match
                        if (source != null)
                        {
                            WriteHeaderLine(writer, _sourceVersionLowercaseKey, source, version);
                        }

                        WriteCommonMetadata(commonSeq, sequenceList, writer, source, version, type, 3);
                        break;

                    case _dateKey:
                        // today's date
                        WriteHeaderLine(writer, _dateLowercaseKey, DateTime.Today.ToString("yyyy-MM-dd"));
                        WriteCommonMetadata(commonSeq, sequenceList, writer, source, version, type, 4);
                        break;
                    case _typeKey:
                        // type header
                        if (type != null)
                        {
                            // output that the types all match; don't need to output if DNA, as DNA is default
                            if (type != MoleculeType.DNA.ToString())
                            {
                                WriteHeaderLine(writer, _typeLowercaseKey, type);
                            }

                        }
                        else if (totalTypeCount == 0)
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
                                    WriteHeaderLine(writer, _typeLowercaseKey, type, sequence.DisplayID);
                                }
                            }
                        }
                        break;

                    case _multiTypeKey:

                        if (totalTypeCount > 0)
                        {
                            if (type == null)
                            {
                                seq = sequenceList.FirstOrDefault(S => S.DisplayID.Equals(value));
                                if (seq != null)
                                {
                                    WriteHeaderLine(writer, _typeLowercaseKey, seq.MoleculeType.ToString(), seq.DisplayID);
                                    typeExceptionList.Add(seq.DisplayID);
                                }

                                currentTypeCount++;

                                if (currentTypeCount == totalTypeCount)
                                {
                                    foreach (ISequence sequence in sequenceList)
                                    {
                                        if (typeExceptionList.Contains(sequence.DisplayID))
                                        {
                                            continue;
                                        }

                                        type = GetGenericTypeString(sequence.MoleculeType);

                                        // only ouput seq-specific type header if this seq won't have its type
                                        // output as part of a sequence data header; don't need to output if DNA,
                                        // as DNA is default
                                        if (type != MoleculeType.DNA.ToString() &&
                                            (!ShouldWriteSequenceData || sequence.Count == 0))
                                        {
                                            WriteHeaderLine(writer, _typeLowercaseKey, type, sequence.DisplayID);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // output that the types all match; don't need to output if DNA, as DNA is default
                                if (type != MoleculeType.DNA.ToString())
                                {
                                    WriteHeaderLine(writer, _typeLowercaseKey, type);
                                }

                                totalTypeCount = 0;
                            }
                        }
                        break;

                    case _multiSeqDataKey:
                        // sequence data
                        if (ShouldWriteSequenceData)
                        {

                            seq = sequenceList.FirstOrDefault(S => S.DisplayID.Equals(value));
                            if (seq != null)
                            {
                                WriteSeqData(seq, type, writer);
                                seqDataExceptionList.Add(seq.DisplayID);
                            }

                            totalSeqData--;

                            if (totalSeqData == 0)
                            {
                                foreach (ISequence sequence in sequenceList)
                                {
                                    if (seqDataExceptionList.Contains(sequence.DisplayID))
                                    {
                                        continue;
                                    }

                                    WriteSeqData(sequence, type, writer);
                                }
                            }
                        }

                        break;

                    case _multiSeqRegKey:
                        seq = sequenceList.FirstOrDefault(S => S.DisplayID.Equals(value));
                        if (seq != null)
                        {
                            if (seq.Metadata.ContainsKey(_startKey) && seq.Metadata.ContainsKey(_endKey))
                            {
                                WriteHeaderLine(writer, _seqRegKey, seq.DisplayID,
                                    seq.Metadata[_startKey] as string, seq.Metadata[_endKey] as string);

                            }

                            seqRegExceptionList.Add(value);
                        }


                        totalSeqRegs--;
                        if (totalSeqRegs == 0)
                        {
                            // sequence-region header
                            foreach (ISequence sequence in sequenceList)
                            {
                                if (seqRegExceptionList.Contains(sequence.DisplayID))
                                {
                                    continue;
                                }

                                if (sequence.Metadata.ContainsKey(_startKey) && sequence.Metadata.ContainsKey(_endKey))
                                {
                                    WriteHeaderLine(writer, _seqRegKey, sequence.DisplayID,
                                        sequence.Metadata[_startKey] as string, sequence.Metadata[_endKey] as string);

                                }
                            }
                        }
                        break;
                }
            }
        }

        // writes the sequence to the sepecified writer.
        private void WriteSeqData(ISequence sequence, string type, TextWriter writer)
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
                    WriteHeaderLine(writer, derivedSeq.ToString().ToLower(CultureInfo.InvariantCulture));
                }

                WriteHeaderLine(writer, "end-" + type);
            }
        }

        // writes common metadata.
        private void WriteCommonMetadata(ISequence commonSeq, ICollection<ISequence> sequenceList, TextWriter writer, string source, string version, string type, int startFrom)
        {
            int totalTypeCount = commonSeq.Metadata.Keys.Count(K => K.ToUpperInvariant().Contains(_multiTypeKey));

            if (startFrom == 1)
            {
                if (commonSeq.Metadata.Keys.Count(K => K.ToUpperInvariant().Contains(_gffVersionKey)) == 0)
                {
                    // formatting using gff version 2
                    WriteHeaderLine(writer, _gffVersionLowercaseKey, "2");

                    WriteCommonMetadata(commonSeq, sequenceList, writer, source, version, type, 2);
                }
            }

            if (startFrom == 2)
            {

                if (source != null && commonSeq.Metadata.Keys.Count(K => K.ToUpperInvariant().Contains(_sourceVersionKey)) == 0)
                {
                    // only output source if they all match
                    WriteHeaderLine(writer, _sourceVersionLowercaseKey, source, version);
                }

                WriteCommonMetadata(commonSeq, sequenceList, writer, source, version, type, 3);
            }

            if (startFrom == 3)
            {
                if (commonSeq.Metadata.Keys.Count(K => K.ToUpperInvariant().Contains(_dateKey)) == 0)
                {
                    // today's date
                    WriteHeaderLine(writer, _dateLowercaseKey, DateTime.Today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

                    WriteCommonMetadata(commonSeq, sequenceList, writer, source, version, type, 4);
                }
            }

            if (startFrom == 4)
            {
                if (totalTypeCount == 0 && commonSeq.Metadata.Keys.Count(K => K.ToUpperInvariant().Contains(_typeKey)) == 0)
                {
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
                                WriteHeaderLine(writer, _typeLowercaseKey, type, sequence.DisplayID);
                            }
                        }
                    }
                    else
                    {
                        // output that the types all match; don't need to output if DNA, as DNA is default
                        if (type != MoleculeType.DNA.ToString())
                        {
                            WriteHeaderLine(writer, _typeLowercaseKey, type);
                        }
                    }
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
            if (sequence.Metadata.ContainsKey(_featuresKey))
            {
                foreach (MetadataListItem<List<string>> feature in
                    sequence.Metadata[_featuresKey] as List<MetadataListItem<List<string>>>)
                {
                    // only write the line if we have all the mandatory fields
                    if (feature.SubItems.ContainsKey(_sourceKey) &&
                        feature.SubItems.ContainsKey(_startKey) &&
                        feature.SubItems.ContainsKey(_endKey))
                    {
                        StringBuilder featureLine = new StringBuilder();
                        featureLine.Append(sequence.DisplayID);
                        featureLine.Append("\t");
                        featureLine.Append(GetSubItemString(feature, _sourceKey));
                        featureLine.Append("\t");
                        featureLine.Append(feature.Key);
                        featureLine.Append("\t");
                        featureLine.Append(GetSubItemString(feature, _startKey));
                        featureLine.Append("\t");
                        featureLine.Append(GetSubItemString(feature, _endKey));
                        featureLine.Append("\t");
                        featureLine.Append(GetSubItemString(feature, _scoreKey));
                        featureLine.Append("\t");
                        featureLine.Append(GetSubItemString(feature, _strandKey));
                        featureLine.Append("\t");
                        featureLine.Append(GetSubItemString(feature, _frameKey));

                        // optional attributes field is stored as free text
                        if (feature.FreeText != string.Empty)
                        {
                            featureLine.Append("\t");
                            featureLine.Append(feature.FreeText);
                        }

                        writer.WriteLine(featureLine.ToString());
                    }
                }
            }
        }

        // Returns a tab plus the sub-item text or a "." if the sub-item is absent.
        private string GetSubItemString(MetadataListItem<List<string>> feature, string subItemName)
        {
            List<string> list = null;

            if (feature.SubItems.TryGetValue(subItemName, out list))
            {
                return list[0];
            }

            return ".";
        }

        #endregion
    }
}
