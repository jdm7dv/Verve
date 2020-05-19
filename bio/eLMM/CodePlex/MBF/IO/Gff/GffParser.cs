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
using System.Globalization;
using System.IO;
using System.Linq;
using Bio.Encoding;
using Bio.Properties;
using Bio.Util.Logging;

namespace Bio.IO.Gff
{
    /// <summary>
    /// A GffParser reads from a source of text that is formatted according to the GFF flat
    /// file specification, and converts the data to in-memory ISequence objects.  For advanced
    /// users, the ability to select an encoding for the internal memory representation is
    /// provided. There is also a default encoding for each alphabet that may be encountered.
    /// Documentation for the latest GFF file format can be found at
    /// http://www.sanger.ac.uk/Software/formats/GFF/GFF_Spec.shtml
    /// </summary>
    public class GffParser : BasicSequenceParser
    {
        #region Fields

        private const string _headerMark = "##";
        private const string _commentMark = "#";

        private const int _minFieldsPerFeature = 8;
        private const int _maxFieldsPerFeature = 9;

        private bool _isSingleSeqGff;

        private List<Sequence> _sequences;
        private Sequence _commonSeq;
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor chooses default encoding based on alphabet.
        /// </summary>
        public GffParser()
            : base()
        {
        }

        /// <summary>
        /// Constructor for setting the encoding.
        /// </summary>
        /// <param name="encoding">The encoding to use for parsed ISequence objects.</param>
        public GffParser(IEncoding encoding)
            : base(encoding)
        {
        }

        #endregion

        #region BasicSequenceParser Members

        /// <summary>
        /// Gets the type of Parser i.e GFF.
        /// This is intended to give developers some information 
        /// of the parser class.
        /// </summary>
        public override string Name
        {
            get
            {
                return Resource.GFF_NAME;
            }
        }

        /// <summary>
        /// Gets the description of GFF parser.
        /// This is intended to give developers some information 
        /// of the formatter class. This property returns a simple description of what the
        /// GffParser class acheives.
        /// </summary>
        public override string Description
        {
            get
            {
                return Resource.GFFPARSER_DESCRIPTION;
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
        /// Parses a list of GFF sequences using a BioTextReader.
        /// </summary>
        /// <remarks>
        /// This method is overridden to process file-scope metadata that applies to all
        /// of the sequences in the file.
        /// </remarks>
        /// <param name="bioReader">A reader for a GFF text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequence objects.</returns>
        protected override IList<ISequence> Parse(BioTextReader bioReader, bool isReadOnly)
        {
            if (bioReader == null)
            {
                throw new ArgumentNullException("bioReader");
            }

            _isSingleSeqGff = false;
            _sequences = new List<Sequence>();
            IAlphabet alphabet = Alphabet;

            if (alphabet == null)
            {
                alphabet = Alphabets.DNA;
            }

            if (Encoding == null)
            {
                _commonSeq = new Sequence(alphabet);
            }
            else
            {
                _commonSeq = new Sequence(alphabet, Encoding, string.Empty);
            }

            // The GFF spec says that all headers need to be at the top of the file.
            ParseHeaders(bioReader);

            // Use the multiSeqBuilder to parse all of the sequences from the file into a list.
            while (bioReader.HasLines)
            {
                ParseFeatures(bioReader);
            }

            CopyMetadata(isReadOnly);
            IEnumerable<ISequence> sequences = from seq in _sequences select seq as ISequence;
            return sequences.ToList();
        }

        /// <summary>
        /// Parses a single GFF text from a reader into a sequence.
        /// </summary>
        /// <param name="bioReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>A new Sequence instance containing parsed data.</returns>
        protected override ISequence ParseOneWithSpecificFormat(BioTextReader bioReader, bool isReadOnly)
        {
            _isSingleSeqGff = true;
            _sequences = new List<Sequence>();
            IAlphabet alphabet = Alphabet;

            if (alphabet == null)
            {
                alphabet = Alphabets.DNA;
            }

            if (Encoding == null)
            {
                _commonSeq = new Sequence(alphabet);
            }
            else
            {
                _commonSeq = new Sequence(alphabet, Encoding, string.Empty);
            }

            // The GFF spec says that all headers need to be at the top of the file.
            ParseHeaders(bioReader);
            ParseFeatures(bioReader);
            CopyMetadata(isReadOnly);
            return _sequences[0];
        }

        #endregion

        #region Private Methods

        // Processes headers, which are a type of comment.
        private void ParseHeaders(BioTextReader bioReader)
        {
            while (bioReader.HasLines && bioReader.Line.StartsWith(_commentMark, StringComparison.Ordinal))
            {
                Sequence specificSeq = null;

                // process headers, but ignore other comments
                if (bioReader.Line.StartsWith(_headerMark, StringComparison.Ordinal))
                {
                    string[] fields = bioReader.GetLineField(3).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    switch (fields[0].ToUpperInvariant())
                    {
                        case "GFF-VERSION":
                            if (fields.Length > 1 && fields[1] != "2")
                            {
                                string message = String.Format(
                                        CultureInfo.CurrentCulture,
                                        Properties.Resource.GffUnsupportedVersion,
                                        bioReader.LocationString);
                                Trace.Report(message);
                                throw new NotSupportedException(message);
                            }
                            // don't store this
                            break;
                        case "SOURCE-VERSION":
                            _commonSeq.Metadata["source"] = fields[1];
                            _commonSeq.Metadata["version"] = fields[2];
                            break;
                        case "DATE":
                            DateTime date;
                            if (!DateTime.TryParse(fields[1], out date))
                            {
                                string message = String.Format(
                                        CultureInfo.CurrentCulture,
                                        Properties.Resource.ParserInvalidDate,
                                        bioReader.LocationString);
                                Trace.Report(message);
                                throw new FormatException(message);
                            }

                            _commonSeq.Metadata["date"] = date;
                            break;
                        case "TYPE":
                            if (fields.Length == 2)
                            {
                                _commonSeq.MoleculeType = GetMoleculeType(fields[1]);
                                if (_commonSeq.MoleculeType == MoleculeType.Invalid)
                                {
                                    string message = String.Format(
                                            CultureInfo.CurrentCulture,
                                            Properties.Resource.InvalidType,
                                            bioReader.LocationString);
                                    Trace.Report(message);
                                    throw new FormatException(message);
                                }
                            }
                            else
                            {
                                specificSeq = GetSpecificSequence(fields[2], GetMoleculeType(fields[1]), bioReader);

                                if (specificSeq.MoleculeType == MoleculeType.Invalid)
                                {
                                    string message = String.Format(
                                            CultureInfo.CurrentCulture,
                                            Properties.Resource.InvalidType,
                                            bioReader.LocationString);
                                    Trace.Report(message);
                                    throw new FormatException(message);
                                }
                            }
                            break;
                        case "DNA":
                        case "RNA":
                        case "PROTEIN":
                            specificSeq = GetSpecificSequence(fields[1], GetMoleculeType(fields[0]), bioReader);
                            bioReader.GoToNextLine();

                            while (bioReader.HasLines && bioReader.Line != "##end-" + fields[0])
                            {
                                if (!bioReader.Line.StartsWith(_headerMark, StringComparison.Ordinal))
                                {
                                    string message = String.Format(
                                            CultureInfo.CurrentCulture,
                                            Properties.Resource.GffInvalidSequence,
                                            bioReader.LocationString);
                                    Trace.Report(message);
                                    throw new FormatException(message);
                                }

                                specificSeq.InsertRange(specificSeq.Count, bioReader.GetLineField(3));

                                bioReader.GoToNextLine();
                            }

                            break;
                        case "SEQUENCE-REGION":
                            specificSeq = GetSpecificSequence(fields[1], MoleculeType.Invalid, bioReader);
                            specificSeq.Metadata["start"] = fields[2];
                            specificSeq.Metadata["end"] = fields[3];
                            break;
                    }
                }

                bioReader.GoToNextLine();
            }
        }

        // Parses the consecutive feature lines for one sequence.
        private void ParseFeatures(BioTextReader bioReader)
        {
            // The non-comment lines contain features, which are each stored as MetadataListItems.
            // The fields of each feature are referred to as sub-items.  For GFF, these have
            // unique keys, but for compatability with our internal representation of features from
            // GenBank format, each sub-item is a list of strings, rather than a simple string.
            List<MetadataListItem<List<string>>> featureList = null;

            Sequence specificSeq = null;

            while (bioReader.HasLines)
            {
                if (bioReader.Line.StartsWith(_headerMark, StringComparison.Ordinal))
                {
                    // ignore comments
                    bioReader.GoToNextLine();
                }
                else
                {
                    // fields are tab-delimited
                    string[] featureFields = bioReader.Line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    if (featureFields.Length < _minFieldsPerFeature ||
                        featureFields.Length > _maxFieldsPerFeature)
                    {
                        string message = string.Format(CultureInfo.CurrentCulture, Resource.INVAILD_INPUT_FILE, this.Name); ;
                        throw new InvalidDataException(message);
                    }

                    // The featureFields array should now contain the following fields:
                    //      featureFields[0]: sequence name
                    //      featureFields[1]: source
                    //      featureFields[2]: feature name
                    //      featureFields[3]: start
                    //      featureFields[4]: end
                    //      featureFields[5]: score
                    //      featureFields[6]: strand
                    //      featureFields[7]: frame
                    //      featureFields[8]: attributes (optional)

                    // Process sequence name.
                    if (specificSeq == null)
                    {
                        specificSeq = GetSpecificSequence(featureFields[0], MoleculeType.Invalid, bioReader);

                        // Retrieve features list, or add empty features list to metadata if this
                        // is the first feature.
                        if (specificSeq.Metadata.ContainsKey("features"))
                        {
                            featureList = specificSeq.Metadata["features"] as
                                List<MetadataListItem<List<string>>>;
                        }
                        else
                        {
                            featureList = new List<MetadataListItem<List<string>>>();
                            specificSeq.Metadata["features"] = featureList;
                        }

                    }
                    else if (specificSeq.DisplayID != featureFields[0])
                    {
                        // don't go to next line; current line still needs to be processed
                        break;
                    }

                    // use feature name as key; attributes field is stored as free text
                    string attributes = (featureFields.Length == 9 ? featureFields[8] : string.Empty);
                    MetadataListItem<List<string>> feature = new MetadataListItem<List<string>>(featureFields[2], attributes);

                    // source
                    feature.SubItems.Add("source", new List<string> { featureFields[1] });

                    // start is an int
                    int ignoreMe;
                    if (!int.TryParse(featureFields[3], out ignoreMe))
                    {
                        string message = String.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.GffInvalidField,
                                "start",
                                featureFields[3]);
                        Trace.Report(message);
                        throw new InvalidDataException(message);
                    }
                    feature.SubItems.Add("start", new List<string> { featureFields[3] });

                    // end is an int
                    if (!int.TryParse(featureFields[4], out ignoreMe))
                    {
                        string message = String.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.GffInvalidField,
                                "end",
                                featureFields[4]);
                        Trace.Report(message);
                        throw new InvalidDataException(message);
                    }

                    feature.SubItems.Add("end", new List<string> { featureFields[4] });

                    // source is a double, or a dot as a space holder
                    if (featureFields[5] != ".")
                    {
                        double ignoreMeToo;
                        if (!double.TryParse(featureFields[5], out ignoreMeToo))
                        {
                            string message = String.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.GffInvalidField,
                                "score",
                                featureFields[5]);
                            Trace.Report(message);
                            throw new InvalidDataException(message);
                        }
                        feature.SubItems.Add("score", new List<string> { featureFields[5] });
                    }

                    // strand is + or -, or a dot as a space holder
                    if (featureFields[6] != ".")
                    {
                        if (featureFields[6] != "+" && featureFields[6] != "-")
                        {
                            string message = String.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.GffInvalidField,
                                "strand",
                                featureFields[6]);
                            Trace.Report(message);
                            throw new InvalidDataException(message);
                        }
                        feature.SubItems.Add("strand", new List<string> { featureFields[6] });
                    }

                    // frame is an int, or a dot as a space holder
                    if (featureFields[7] != ".")
                    {
                        if (!int.TryParse(featureFields[7], out ignoreMe))
                        {
                            string message = String.Format(
                            CultureInfo.CurrentCulture,
                                Properties.Resource.GffInvalidField,
                                "frame",
                                featureFields[7]);
                            Trace.Report(message);
                            throw new InvalidDataException(message);
                        }

                        feature.SubItems.Add("frame", new List<string> { featureFields[7] });
                    }

                    // done with that one
                    featureList.Add(feature);
                    bioReader.GoToNextLine();
                }
            }

            // A feature file with no features?  May it never be.
            if (featureList == null)
            {
                string message = Properties.Resource.GFFNoFeatures;
                Trace.Report(message);
                throw new InvalidOperationException(message);
            }
        }

        // Returns a sequence corresponding to the given sequence name, setting its display
        // ID if it has not yet been set.  If parsing for single sequence and already a sequence is exist and it
        // has already been assigened a display ID that doesn't matach sequenceName, and exception
        // is thrown.
        private Sequence GetSpecificSequence(string sequenceName, MoleculeType moleculeType, BioTextReader bioReader)
        {
            Sequence seq = null;

            // The GFF spec says that DNA is the default molecule type.
            if (moleculeType == MoleculeType.Invalid)
            {
                moleculeType = MoleculeType.DNA;
            }

            IAlphabet alphabet = GetAlphabet(moleculeType);

            if (_sequences.Count == 0)
            {
                if (Encoding == null)
                {
                    seq = new Sequence(alphabet);
                }
                else
                {
                    seq = new Sequence(alphabet, Encoding, string.Empty);
                    seq.IsReadOnly = false;
                }

                seq.DisplayID = sequenceName;
                seq.ID = sequenceName;
                seq.MoleculeType = moleculeType;
                _sequences.Add(seq);
            }

            if (_isSingleSeqGff)
            {
                if (!_sequences[0].DisplayID.Equals(sequenceName))
                {
                    string message = String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.UnexpectedSecondSequenceName,
                        bioReader.LocationString);
                    Trace.Report(message);
                    throw new InvalidOperationException(message);
                }

                seq = _sequences[0];
            }
            else
            {
                seq = _sequences.FirstOrDefault(S => S.DisplayID.Equals(sequenceName));
                if (seq == null)
                {
                    if (Encoding == null)
                    {
                        seq = new Sequence(alphabet);
                    }
                    else
                    {
                        seq = new Sequence(alphabet, Encoding, string.Empty);
                        seq.IsReadOnly = false;
                    }

                    seq.DisplayID = sequenceName;
                    seq.ID = sequenceName;
                    seq.MoleculeType = moleculeType;
                    _sequences.Add(seq);
                }
            }

            return seq;
        }

        /// <summary>
        /// Copy file-scope metadata to all the sequences in the list.
        /// </summary>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        private void CopyMetadata(bool isReadOnly)
        {
            foreach (Sequence seq in _sequences)
            {
                foreach (KeyValuePair<string, object> pair in _commonSeq.Metadata)
                {
                    seq.Metadata[pair.Key] = pair.Value;
                }

                seq.IsReadOnly = isReadOnly;
            }
        }
        #endregion
    }
}
