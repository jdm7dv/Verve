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
using System.Text.RegularExpressions;
using Bio.Encoding;
using Bio.Properties;
using Bio.Util;
using Bio.Util.Logging;

namespace Bio.IO.GenBank
{
    /// <summary>
    /// A GenBankParser reads from a source of text that is formatted according to the GenBank flat
    /// file specification, and converts the data to in-memory ISequence objects.  For advanced
    /// users, the ability to select an encoding for the internal memory representation is
    /// provided. There is also a default encoding for each alphabet that may be encountered.
    /// Documentation for the latest GenBank file format can be found at
    /// ftp.ncbi.nih.gov/genbank/gbrel.txt
    /// </summary>
    public class GenBankParser : BasicSequenceParser
    {
        #region Fields

        // the standard indent for data is different from the indent for data in the features section
        private const int _dataIndent = 12;
        private const int _featureDataIndent = 21;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor chooses default encoding based on alphabet.
        /// </summary>
        public GenBankParser()
            : base()
        {
            LocationBuilder = new LocationBuilder();
        }

        /// <summary>
        /// Constructor for setting the encoding.
        /// </summary>
        /// <param name="encoding">The encoding to use for parsed ISequence objects.</param>
        public GenBankParser(IEncoding encoding)
            : base(encoding)
        {
            LocationBuilder = new LocationBuilder();
        }

        #endregion

        #region Properties
        /// <summary>
        /// Location builder is used to build location objects from the location string 
        /// present in the features.
        /// By default an instance of LocationBuilder class is used to build location objects.
        /// </summary>
        public ILocationBuilder LocationBuilder { get; set; }
        #endregion

        #region BasicSequenceParser Members

        /// <summary>
        /// Gets the type of Parser i.e GenBank.
        /// This is intended to give developers some information 
        /// of the parser class.
        /// </summary>
        public override string Name
        {
            get
            {
                return Resource.GENBANK_NAME;
            }
        }

        /// <summary>
        /// Gets the description of GenBank parser.
        /// This is intended to give developers some information 
        /// of the formatter class. This property returns a simple description of what the
        /// GenBankParser class acheives.
        /// </summary>
        public override string Description
        {
            get
            {
                return Resource.GENBANKPARSER_DESCRIPTION;
            }
        }

        /// <summary>
        /// Gets a comma seperated values of the possible
        /// file extensions for a GenBank file.
        /// </summary>
        public override string FileTypes
        {
            get
            {
                return Resource.GENBANK_FILEEXTENSION;
            }
        }

        /// <summary>
        /// Parses a single GenBank text from a reader into a sequence.
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
            Sequence sequence = null;

            if (Alphabet == null)
            {
                if (Encoding == null)
                {
                    sequence = new Sequence(Alphabets.DNA);
                }
                else
                {
                    sequence = new Sequence(Alphabets.DNA, Encoding, string.Empty);
                    sequence.IsReadOnly = false;
                }
            }
            else
            {
                if (Encoding == null)
                {
                    sequence = new Sequence(Alphabet);
                }
                else
                {
                    sequence = new Sequence(Alphabet, Encoding, string.Empty);
                    sequence.IsReadOnly = false;
                }
            }

            sequence.Metadata[Helper.GenBankMetadataKey] = new GenBankMetadata();
            sequence.MoleculeType = GetMoleculeType(sequence.Alphabet);
            // parse the file
            ParseHeaders(bioReader, ref sequence);
            ParseFeatures(bioReader, ref  sequence);
            ParseSequence(bioReader, ref sequence);

            sequence.IsReadOnly = isReadOnly;
            return sequence;
        }

        #endregion

        #region Parse Headers Methods

        // parses everything before the features section
        private void ParseHeaders(BioTextReader bioReader, ref Sequence sequence)
        {
            GenBankMetadata metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
            string data = string.Empty;
            string[] tokens = null;
            // set data indent for headers
            bioReader.DataIndent = _dataIndent;

            // only allow one locus line
            bool haveParsedLocus = false;

            // parse until we hit the features or sequence section
            bool haveFinishedHeaders = false;
            while (bioReader.HasLines && !haveFinishedHeaders)
            {
                switch (bioReader.LineHeader)
                {
                    case "LOCUS":
                        if (haveParsedLocus)
                        {
                            string message = String.Format(
                                    CultureInfo.CurrentCulture,
                                    Properties.Resource.ParserSecondLocus,
                                    bioReader.LocationString);
                            Trace.Report(message);
                            throw new InvalidDataException(message);
                        }
                        ParseLocus(bioReader, ref sequence);
                        metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
                        haveParsedLocus = true;
                        // don't go to next line; current line still needs to be processed
                        break;

                    case "VERSION":
                        tokens = bioReader.LineData.Split(new char[] { ' ' },
                            StringSplitOptions.RemoveEmptyEntries);
                        // first token contains accession and version
                        Match m = Regex.Match(tokens[0], @"^(?<accession>\w+)\.(?<version>\d+)$");
                        metadata.Version = new GenBankVersion();

                        if (m.Success)
                        {
                            metadata.Version.Version = m.Groups["version"].Value;
                            // The first token in the data from the accession line is referred to as
                            // the primary accession number, and should be the one used here in the
                            // version line.
                            string versionLineAccession = m.Groups["accession"].Value;
                            if (metadata.Accession == null)
                            {
                                ApplicationLog.WriteLine("WARN: VERSION processed before ACCESSION");
                            }
                            else
                            {
                                if (!versionLineAccession.Equals(metadata.Accession.Primary))
                                {
                                    ApplicationLog.WriteLine("WARN: VERSION tag doesn't match ACCESSION");
                                }
                                else
                                {
                                    metadata.Version.Accession = metadata.Accession.Primary;
                                }
                            }
                        }
                        // second token contains primary ID
                        m = Regex.Match(tokens[1], @"^GI:(?<primaryID>.*)");
                        if (m.Success)
                        {
                            metadata.Version.GINumber = m.Groups["primaryID"].Value;
                        }
                        bioReader.GoToNextLine();
                        break;

                    case "PROJECT":
                        tokens = bioReader.LineData.Split(':');
                        if (tokens.Length == 2)
                        {
                            metadata.Project = new ProjectIdentifier();
                            metadata.Project.Name = tokens[0];
                            tokens = tokens[1].Split(',');
                            for (int i = 0; i < tokens.Length; i++)
                            {
                                metadata.Project.Numbers.Add(tokens[i]);
                            }
                        }
                        else
                        {
                            ApplicationLog.WriteLine("WARN: unexpected PROJECT header: " + bioReader.Line);
                        }
                        bioReader.GoToNextLine();
                        break;

                    case "SOURCE":
                        ParseSource(bioReader, ref sequence);
                        metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
                        // don't go to next line; current line still needs to be processed
                        break;

                    case "REFERENCE":
                        ParseReferences(bioReader, ref sequence);   // can encounter more than one
                        metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
                        // don't go to next line; current line still needs to be processed
                        break;

                    case "COMMENT":
                        ParseComments(bioReader, ref sequence);   // can encounter more than one
                        metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
                        // don't go to next line; current line still needs to be processed
                        break;

                    case "PRIMARY":
                        // This header is followed by sequence info in a table format that could be
                        // stored in a custom object.  The first line contains column headers.
                        // For now, just validate the presence of the headers, and save the data
                        // as a string.
                        int[] locs = new int[4];
                        locs[0] = bioReader.LineData.IndexOf("TPA_SPAN", StringComparison.Ordinal);
                        locs[1] = bioReader.LineData.IndexOf("PRIMARY_IDENTIFIER", StringComparison.Ordinal);
                        locs[2] = bioReader.LineData.IndexOf("PRIMARY_SPAN", StringComparison.Ordinal);
                        locs[3] = bioReader.LineData.IndexOf("COMP", StringComparison.Ordinal);
                        if (locs[0] < 0 || locs[1] < 0 || locs[2] < 0 || locs[3] < 0)
                        {
                            string message = String.Format(
                                    CultureInfo.CurrentCulture,
                                    Properties.Resource.ParserPrimaryLineError,
                                    bioReader.Line);
                            Trace.Report(message);
                            throw new InvalidDataException(message);
                        }
                        string primaryData = ParseMultiLineData(bioReader, Environment.NewLine);
                        metadata.Primary = primaryData;
                        // don't go to next line; current line still needs to be processed
                        break;

                    // all the following are extracted the same way - possibly multiline
                    case "DEFINITION":
                        metadata.Definition = ParseMultiLineData(bioReader, " ");
                        break;
                    case "ACCESSION":
                        data = ParseMultiLineData(bioReader, " ");
                        metadata.Accession = new GenBankAccession();
                        string[] accessions = data.Split(' ');
                        metadata.Accession.Primary = accessions[0];

                        for (int i = 1; i < accessions.Length; i++)
                        {
                            metadata.Accession.Secondary.Add(accessions[i]);
                        }
                        break;

                    case "DBLINK":
                        tokens = bioReader.LineData.Split(':');
                        if (tokens.Length == 2)
                        {
                            metadata.DBLink = new CrossReferenceLink();
                            if (string.Compare(tokens[0],
                                CrossReferenceType.Project.ToString(),
                                StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                metadata.DBLink.Type = CrossReferenceType.Project;
                            }
                            else
                            {
                                metadata.DBLink.Type = CrossReferenceType.TraceAssemblyArchive;
                            }

                            tokens = tokens[1].Split(',');
                            for (int i = 0; i < tokens.Length; i++)
                            {
                                metadata.DBLink.Numbers.Add(tokens[i]);
                            }
                        }
                        else
                        {
                            ApplicationLog.WriteLine("WARN: unexpected DBLINK header: " + bioReader.Line);
                        }
                        bioReader.GoToNextLine();
                        break;

                    case "DBSOURCE":
                        metadata.DBSource = ParseMultiLineData(bioReader, " ");
                        break;

                    case "KEYWORDS":
                        metadata.Keywords = ParseMultiLineData(bioReader, " ");
                        break;

                    case "SEGMENT":
                        data = ParseMultiLineData(bioReader, " ");
                        string delimeter = "of";
                        tokens = data.Split(delimeter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        int outvalue;
                        if (tokens.Length == 2)
                        {
                            metadata.Segment = new SequenceSegment();
                            if (int.TryParse(tokens[0].Trim(), out outvalue))
                            {
                                metadata.Segment.Current = outvalue;
                            }
                            else
                            {
                                ApplicationLog.WriteLine("WARN: unexpected SEGMENT header: " + bioReader.Line);
                            }

                            if (int.TryParse(tokens[1].Trim(), out outvalue))
                            {
                                metadata.Segment.Count = outvalue;
                            }
                            else
                            {
                                ApplicationLog.WriteLine("WARN: unexpected SEGMENT header: " + bioReader.Line);
                            }
                        }
                        else
                        {
                            ApplicationLog.WriteLine("WARN: unexpected SEGMENT header: " + bioReader.Line);
                        }
                        break;

                    // all the following indicate sections beyond the headers parsed by this method
                    case "FEATURES":
                    case "BASE COUNT":
                    case "ORIGIN":
                    case "CONTIG":
                        haveFinishedHeaders = true;
                        break;

                    default:
                        ApplicationLog.WriteLine(ToString() + "WARN: unknown {0} -> {1}", bioReader.LineHeader, bioReader.LineData);
                        string errMessage = String.Format(
                                    CultureInfo.CurrentCulture,
                                    Properties.Resource.ParseHeaderError,
                                    bioReader.LineHeader);
                            Trace.Report(errMessage);
                            throw new InvalidDataException(errMessage);
                }
            }

            // check for required features
            if (!haveParsedLocus)
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.INVAILD_INPUT_FILE, this.Name);
                Trace.Report(message);
                throw new InvalidDataException(message);
            }
        }

        // LOCUS is the first line in a GenBank record
        private void ParseLocus(BioTextReader bioReader, ref Sequence sequence)
        {
            GenBankLocusInfo locusInfo = new GenBankLocusInfo();

            // GenBank spec recommends token rather than position-based parsing, but this
            // is only partially possible without making extra assumptions about the presence
            // of optional fields.
            string[] tokens = bioReader.LineData.Split(new char[] { ' ' },
                StringSplitOptions.RemoveEmptyEntries);
            sequence.ID = tokens[0];
            locusInfo.Name = tokens[0];

            int sequenceLength;
            if (!int.TryParse(tokens[1], out sequenceLength))
                throw new InvalidOperationException();
            locusInfo.SequenceLength = sequenceLength;

            string seqType = tokens[2];
            if (seqType != "bp" && seqType != "aa")
            {
                string message = String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.ParserInvalidLocus,
                        bioReader.Line);
                Trace.Report(message);
                throw new InvalidDataException(message);
            }

            // Determine format version and parse the remaining fields by position.
            string strandType;
            string strandTopology;
            string division;
            string rawDate;
            string molType = string.Empty;
            if (Helper.StringHasMatch(bioReader.GetLineField(31, 32), "bp", "aa"))
            {
                // older format
                strandType = bioReader.GetLineField(34, 36).Trim();
                strandTopology = bioReader.GetLineField(43, 52).Trim();
                division = bioReader.GetLineField(53, 56).Trim();
                rawDate = bioReader.GetLineField(63).Trim();

                // molecule type field is not used for amino acid chains
                if (seqType != "aa")
                {
                    molType = bioReader.GetLineField(37, 42).Trim();
                }
            }
            else
            {
                // newer format
                strandType = bioReader.GetLineField(45, 47).Trim();
                strandTopology = bioReader.GetLineField(56, 63).Trim();
                division = bioReader.GetLineField(65, 67).Trim();
                rawDate = bioReader.GetLineField(69).Trim();

                // molecule type field is not used for amino acid chains
                if (seqType != "aa")
                {
                    molType = bioReader.GetLineField(48, 53).Trim();
                }
            }

            // process strand type
            if (!Helper.StringHasMatch(strandType, string.Empty, "ss-", "ds-", "ms-"))
            {
                string message = String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.ParserInvalidLocus,
                        bioReader.Line);
                Trace.Report(message);
                throw new InvalidDataException(message);
            }
            locusInfo.Strand = Helper.GetStrandType(strandType);

            // process strand topology
            if (!Helper.StringHasMatch(strandTopology, string.Empty, "linear", "circular"))
            {
                string message = String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.ParserInvalidStrand,
                        strandTopology);
                Trace.Report(message);
                throw new InvalidDataException(message);
            }

            locusInfo.StrandTopology = Helper.GetStrandTopology(strandTopology);

            // process division
            try
            {
                locusInfo.DivisionCode = (SequenceDivisionCode)Enum.Parse(typeof(SequenceDivisionCode), division);
            }
            catch (ArgumentException)
            {
                locusInfo.DivisionCode = SequenceDivisionCode.None;
            }

            // process date
            DateTime date;
            if (!DateTime.TryParse(rawDate, out date))
            {
                string message = String.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.ParserInvalidDate,
                        rawDate);
                Trace.Report(message);
                throw new FormatException(message);
            }

            locusInfo.Date = date;
            locusInfo.SequenceType = seqType;

            // process sequence type and molecule type
            MoleculeType moleculeType;
            if (seqType == "aa")
            {
                moleculeType = MoleculeType.Protein;
            }
            else
            {
                moleculeType = GetMoleculeType(molType);

                if (moleculeType == MoleculeType.Invalid)
                {
                    string message = String.Format(
                            CultureInfo.CurrentCulture,
                            Properties.Resource.ParserInvalidLocus,
                            bioReader.Line);
                    Trace.Report(message);
                    throw new FormatException(message);
                }
            }

            IAlphabet alphabet = GetAlphabet(moleculeType);
            if (alphabet != sequence.Alphabet)
            {
                if (Alphabet != null && Alphabet != alphabet)
                {
                    string message = Properties.Resource.ParserIncorrectAlphabet;
                    Trace.Report(message);
                    throw new InvalidDataException(message);
                }
                sequence = new Sequence(alphabet, Encoding, sequence);
                sequence.IsReadOnly = false;
            }

            sequence.MoleculeType = moleculeType;
            locusInfo.MoleculeType = moleculeType;
            GenBankMetadata metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
            metadata.Locus = locusInfo;
            bioReader.GoToNextLine();
        }

        private static void ParseReferences(BioTextReader bioReader, ref Sequence sequence)
        {
            GenBankMetadata metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
            IList<CitationReference> referenceList = metadata.References;
            CitationReference reference = null;
            //List<MetadataListItem<string>> referenceList = new List<MetadataListItem<string>>();
            //MetadataListItem<string> reference = null;

            while (bioReader.HasLines)
            {
                if (bioReader.LineHeader == "REFERENCE")
                {
                    // add previous reference
                    if (reference != null)
                    {
                        referenceList.Add(reference);
                    }

                    // check for start/end e.g. (bases 1 to 118), or prose notes
                    Match m = Regex.Match(bioReader.LineData,
                        @"^(?<number>\d+)(\s+\((?<location>.*)\))?");
                    if (!m.Success)
                    {
                        string message = String.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.ParserReferenceError,
                                bioReader.LineData);
                        Trace.Report(message);
                        throw new InvalidDataException(message);
                    }

                    // create new reference
                    string number = m.Groups["number"].Value;
                    string location = m.Groups["location"].Value;
                    reference = new CitationReference();
                    int outValue;
                    if (!int.TryParse(number, out outValue))
                        throw new InvalidOperationException();
                    reference.Number = outValue;
                    reference.Location = location;
                    bioReader.GoToNextLine();
                }
                else if (bioReader.Line.StartsWith(" ", StringComparison.Ordinal))
                {
                    switch (bioReader.LineHeader)
                    {
                        // all the following are extracted the same way - possibly multiline
                        case "AUTHORS":
                            reference.Authors = ParseMultiLineData(bioReader, " ");
                            break;
                        case "CONSRTM":
                            reference.Consortiums = ParseMultiLineData(bioReader, " ");
                            break;
                        case "TITLE":
                            reference.Title = ParseMultiLineData(bioReader, " ");
                            break;
                        case "JOURNAL":
                            reference.Journal = ParseMultiLineData(bioReader, " ");
                            break;
                        case "REMARK":
                            reference.Remarks = ParseMultiLineData(bioReader, " ");
                            break;
                        case "MEDLINE":
                            reference.Medline = ParseMultiLineData(bioReader, " ");
                            break;
                        case "PUBMED":
                            reference.PubMed = ParseMultiLineData(bioReader, " ");
                            break;

                        default:
                            string message = String.Format(
                                    CultureInfo.CurrentCulture,
                                    Properties.Resource.ParserInvalidReferenceField,
                                    bioReader.LineHeader);
                            Trace.Report(message);
                            throw new InvalidDataException(message);
                    }
                }
                else
                {
                    // add last reference
                    if (reference != null)
                    {
                        referenceList.Add(reference);
                    }

                    // don't go to next line; current line still needs to be processed
                    break;
                }
            }
        }

        private static void ParseComments(BioTextReader bioReader, ref Sequence sequence)
        {
            IList<string> commentList = ((GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey]).Comments;

            // don't skip blank lines in comments
            bioReader.SkipBlankLines = false;

            while (bioReader.HasLines && bioReader.LineHeader == "COMMENT")
            {
                string data = ParseMultiLineData(bioReader, Environment.NewLine);
                commentList.Add(data);
                // don't go to next line; current line still needs to be processed
            }

            // back to skipping blank lines when done with comments
            bioReader.SkipBlankLines = true;
        }

        private static void ParseSource(BioTextReader bioReader, ref Sequence sequence)
        {
            string source = string.Empty;
            string organism = string.Empty;
            string classLevels = string.Empty;

            while (bioReader.HasLines)
            {
                if (bioReader.LineHeader == "SOURCE")
                {
                    // data can be multiline. spec says last line must end with period
                    // (note: this doesn't apply unless multiline)
                    bool lastDotted = true;
                    source = bioReader.LineData;

                    bioReader.GoToNextLine();
                    while (bioReader.HasLines && !bioReader.LineHasHeader)
                    {
                        source += " " + bioReader.LineData;
                        lastDotted = (source.EndsWith(".", StringComparison.Ordinal));
                        bioReader.GoToNextLine();
                    }

                    if (!lastDotted && Trace.Want(Trace.SeqWarnings))
                    {
                        Trace.Report("GenBank.ParseSource", Properties.Resource.OutOfSpec, source);
                    }

                    // don't go to next line; current line still needs to be processed
                }
                else if (bioReader.Line[0] == ' ')
                {
                    if (bioReader.LineHeader != "ORGANISM")
                    {
                        string message = String.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.ParserInvalidSourceField,
                                bioReader.LineHeader);
                        Trace.Report(message);
                        throw new InvalidDataException(message);
                    }

                    // this also can be multiline
                    organism = bioReader.LineData;

                    bioReader.GoToNextLine();
                    while (bioReader.HasLines && !bioReader.LineHasHeader)
                    {
                        if (bioReader.Line.EndsWith(";", StringComparison.Ordinal) || bioReader.Line.EndsWith(".", StringComparison.Ordinal))
                        {
                            if (!String.IsNullOrEmpty(classLevels))
                            {
                                classLevels += " ";
                            }

                            classLevels += bioReader.LineData;
                        }
                        else
                        {
                            organism += " " + bioReader.LineData;
                        }
                        bioReader.GoToNextLine();
                    }

                    // don't go to next line; current line still needs to be processed
                }
                else
                {
                    // don't go to next line; current line still needs to be processed
                    break;
                }
            }

            GenBankMetadata metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
            metadata.Source = new SequenceSource();
            metadata.Source.CommonName = source;
            if (!string.IsNullOrEmpty(organism))
            {
                int index = organism.IndexOf(" ", StringComparison.Ordinal);
                if (index > 0)
                {
                    metadata.Source.Organism.Genus = organism.Substring(0, index);
                    if (organism.Length > index)
                    {
                        index++;
                        metadata.Source.Organism.Species = organism.Substring(index, organism.Length - index);
                    }
                }
                else
                {
                    metadata.Source.Organism.Genus = organism;
                }
            }

            metadata.Source.Organism.ClassLevels = classLevels;
        }

        #endregion

        #region Parse Features Methods

        private void ParseFeatures(BioTextReader bioReader, ref Sequence sequence)
        {
            ILocationBuilder locBuilder = LocationBuilder;
            if (locBuilder == null)
            {
                throw new InvalidOperationException(Resource.NullLocationBuild);
            }

            // set data indent for features
            bioReader.DataIndent = _featureDataIndent;

            // The sub-items of a feature are referred to as qualifiers.  These do not have unique
            // keys, so they are stored as lists in the SubItems dictionary.
            SequenceFeatures features = new SequenceFeatures();
            IList<FeatureItem> featureList = features.All;

            while (bioReader.HasLines)
            {
                if (String.IsNullOrEmpty(bioReader.Line) || bioReader.LineHeader == "FEATURES")
                {
                    bioReader.GoToNextLine();
                    continue;
                }

                if (bioReader.Line[0] != ' ')
                {
                    // start of non-feature text
                    break;
                }

                if (!bioReader.LineHasHeader)
                {
                    string message = Properties.Resource.GenbankEmptyFeature;
                    Trace.Report(message);
                    throw new InvalidDataException(message);
                }

                // check for multi-line location string
                string featureKey = bioReader.LineHeader;
                string location = bioReader.LineData;
                bioReader.GoToNextLine();
                while (bioReader.HasLines && !bioReader.LineHasHeader &&
                    bioReader.LineHasData && !bioReader.LineData.StartsWith("/", StringComparison.Ordinal))
                {
                    location += bioReader.LineData;
                    bioReader.GoToNextLine();
                }

                // create features as MetadataListItems
                FeatureItem feature = new FeatureItem(featureKey, locBuilder.GetLocation(location));

                // process the list of qualifiers, which are each in the form of
                // /key="value"
                string qualifierKey = string.Empty;
                string qualifierValue = string.Empty;
                while (bioReader.HasLines)
                {
                    if (!bioReader.LineHasHeader && bioReader.LineHasData)
                    {
                        // '/' denotes a continuation of the previous line
                        if (bioReader.LineData.StartsWith("/", StringComparison.Ordinal))
                        {
                            // new qualifier; save previous if this isn't the first
                            if (!String.IsNullOrEmpty(qualifierKey))
                            {
                                AddQualifierToFeature(feature, qualifierKey, qualifierValue);
                            }

                            // set the key and value of this qualifier
                            int equalsIndex = bioReader.LineData.IndexOf('=');
                            if (equalsIndex < 0)
                            {
                                // no value, just key (this is allowed, see NC_005213.gbk)
                                qualifierKey = bioReader.LineData.Substring(1);
                                qualifierValue = string.Empty;
                            }
                            else if (equalsIndex > 0)
                            {
                                qualifierKey = bioReader.LineData.Substring(1, equalsIndex - 1);
                                qualifierValue = bioReader.LineData.Substring(equalsIndex + 1);
                            }
                            else
                            {
                                string message = String.Format(
                                        CultureInfo.CurrentCulture,
                                        Properties.Resource.GenbankInvalidFeature,
                                        bioReader.Line);
                                Trace.Report(message);
                                throw new InvalidDataException(message);
                            }
                        }
                        else
                        {
                            // Continuation of previous line; "note" gets a line break, and
                            // everything else except "translation" and "transl_except" gets a
                            // space to separate words.
                            if (qualifierKey == "note")
                            {
                                qualifierValue += Environment.NewLine;
                            }
                            else if (qualifierKey != "translation" && qualifierKey != "transl_except")
                            {
                                qualifierValue += " ";
                            }

                            qualifierValue += bioReader.LineData;
                        }

                        bioReader.GoToNextLine();
                    }
                    else if (bioReader.Line.StartsWith("\t", StringComparison.Ordinal))
                    {
                        // this seems to be data corruption; but BioPerl test set includes
                        // (old, 2003) NT_021877.gbk which has this problem, so we
                        // handle it
                        ApplicationLog.WriteLine("WARN: nonstandard line format at line {0}: '{1}'",
                            bioReader.LineNumber, bioReader.Line);
                        qualifierValue += " " + bioReader.Line.Trim();
                        bioReader.GoToNextLine();
                    }
                    else
                    {
                        break;
                    }
                }

                // add last qualifier
                if (!String.IsNullOrEmpty(qualifierKey))
                {
                    AddQualifierToFeature(feature, qualifierKey, qualifierValue);
                }

                // still add feature, even if it has no qualifiers
                featureList.Add(StandardFeatureMap.GetStandardFeatureItem(feature));
            }

            if (featureList.Count > 0)
            {
                ((GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey]).Features = features;
            }
        }

        // The sub-items of a feature are referred to as qualifiers.  These do not have unique
        // keys, so they are stored as lists in the SubItems dictionary.
        private static void AddQualifierToFeature(FeatureItem feature, string qualifierKey, string qualifierValue)
        {
            if (!feature.Qualifiers.ContainsKey(qualifierKey))
            {
                feature.Qualifiers[qualifierKey] = new List<string>();
            }

            feature.Qualifiers[qualifierKey].Add(qualifierValue);
        }

        #endregion

        #region Parse Sequence Methods

        // Handle optional BASE COUNT, then ORIGIN and sequence data.
        private void ParseSequence(BioTextReader bioReader, ref Sequence sequence)
        {
            string message = string.Empty;

            GenBankMetadata metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
            // set data indent for sequence headers
            bioReader.DataIndent = _dataIndent;

            while (bioReader.HasLines)
            {
                if (bioReader.Line.StartsWith("//", StringComparison.Ordinal))
                {
                    bioReader.GoToNextLine();
                    break; // end of sequence record
                }

                switch (bioReader.LineHeader)
                {
                    case "BASE COUNT":
                        // The BASE COUNT linetype is obsolete and was removed
                        // from the GenBank flatfile format in October 2003.  But if it is
                        // present, we will use it.  We get the untrimmed version since it
                        // starts with a right justified column.
                        metadata.BaseCount = bioReader.Line.Substring(_dataIndent);
                        bioReader.GoToNextLine();
                        break;

                    case "ORIGIN":
                        // The origin line can contain optional data; don't put empty string into
                        // metadata.
                        if (!String.IsNullOrEmpty(bioReader.LineData))
                        {
                            metadata.Origin = bioReader.LineData;
                        }
                        bioReader.GoToNextLine();
                        IAlphabet alphabet = null;
                        while (bioReader.HasLines && bioReader.Line[0] == ' ')
                        {
                            // Using a regex is too slow.
                            int len = bioReader.Line.Length;
                            int k = 10;
                            while (k < len)
                            {
                                string seqData = bioReader.Line.Substring(k, Math.Min(10, len - k));
                                if (Alphabet == null)
                                {
                                    alphabet = IdentifyAlphabet(alphabet, seqData);

                                    if (alphabet == null)
                                    {
                                        message = String.Format(CultureInfo.CurrentCulture, Resource.InvalidSymbolInString, bioReader.Line);
                                        Trace.Report(message);
                                        throw new InvalidDataException(message);
                                    }

                                    if (sequence.Alphabet != alphabet)
                                    {
                                        Sequence seq = new Sequence(alphabet, Encoding, sequence);
                                        seq.MoleculeType = sequence.MoleculeType;
                                        seq.IsReadOnly = false;
                                        sequence.Clear();
                                        sequence = seq;
                                    }
                                }

                                sequence.InsertRange(sequence.Count, seqData);
                                k += 11;
                            }

                            bioReader.GoToNextLine();
                        }
                        break;

                    case "CONTIG":
                        metadata.Contig = ParseMultiLineData(bioReader, Environment.NewLine);
                        // don't go to next line; current line still needs to be processed
                        break;

                    default:
                        message = String.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.ParserUnexpectedLineInSequence,
                                bioReader.Line);
                        Trace.Report(message);
                        throw new InvalidDataException(message);
                }
            }
        }

        #endregion

        #region General Helper Methods

        // returns a string of the data for a header block that spans multiple lines
        private static string ParseMultiLineData(BioTextReader bioReader, string lineBreakSubstitution)
        {
            string data = bioReader.LineData;
            bioReader.GoToNextLine();

            // while succeeding lines start with no header, add to data
            while (bioReader.HasLines && !bioReader.LineHasHeader)
            {
                data += lineBreakSubstitution + bioReader.LineData;
                bioReader.GoToNextLine();
            }

            return data;
        }

        #endregion
    }
}
