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
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MBF.Encoding;
using MBF.Properties;
using MBF.Util;
using MBF.Util.Logging;

namespace MBF.IO.GenBank
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
        /// <param name="mbfReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>A new Sequence instance containing parsed data.</returns>
        protected override ISequence ParseOneWithSpecificFormat(MBFTextReader mbfReader, bool isReadOnly)
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
            ParseHeaders(mbfReader, ref sequence);
            ParseFeatures(mbfReader, ref  sequence);
            ParseSequence(mbfReader, ref sequence);

            sequence.IsReadOnly = isReadOnly;
            return sequence;
        }

        #endregion

        #region Parse Headers Methods

        // parses everything before the features section
        private void ParseHeaders(MBFTextReader mbfReader, ref Sequence sequence)
        {
            GenBankMetadata metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
            string data = string.Empty;
            string[] tokens = null;
            // set data indent for headers
            mbfReader.DataIndent = _dataIndent;

            // only allow one locus line
            bool haveParsedLocus = false;

            // parse until we hit the features or sequence section
            bool haveFinishedHeaders = false;
            while (mbfReader.HasLines && !haveFinishedHeaders)
            {
                switch (mbfReader.LineHeader)
                {
                    case "LOCUS":
                        if (haveParsedLocus)
                        {
                            string message = String.Format(
                                    CultureInfo.CurrentCulture,
                                    Properties.Resource.ParserSecondLocus,
                                    mbfReader.LocationString);
                            Trace.Report(message);
                            throw new InvalidDataException(message);
                        }
                        ParseLocusByTokens(mbfReader, ref sequence);
                        metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
                        haveParsedLocus = true;
                        // don't go to next line; current line still needs to be processed
                        break;

                    case "VERSION":
                        tokens = mbfReader.LineData.Split(new char[] { ' ' },
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
                        mbfReader.GoToNextLine();
                        break;

                    case "PROJECT":
                        tokens = mbfReader.LineData.Split(':');
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
                            ApplicationLog.WriteLine("WARN: unexpected PROJECT header: " + mbfReader.Line);
                        }
                        mbfReader.GoToNextLine();
                        break;

                    case "SOURCE":
                        ParseSource(mbfReader, ref sequence);
                        metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
                        // don't go to next line; current line still needs to be processed
                        break;

                    case "REFERENCE":
                        ParseReferences(mbfReader, ref sequence);   // can encounter more than one
                        metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
                        // don't go to next line; current line still needs to be processed
                        break;

                    case "COMMENT":
                        ParseComments(mbfReader, ref sequence);   // can encounter more than one
                        metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
                        // don't go to next line; current line still needs to be processed
                        break;

                    case "PRIMARY":
                        // This header is followed by sequence info in a table format that could be
                        // stored in a custom object.  The first line contains column headers.
                        // For now, just validate the presence of the headers, and save the data
                        // as a string.
                        tokens = mbfReader.LineData.Split("\t ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        // Validating for minimum two headers.
                        if (tokens.Length != 4)
                        {
                            string message = String.Format(
                                    CultureInfo.CurrentCulture,
                                    Properties.Resource.ParserPrimaryLineError,
                                    mbfReader.Line);
                            Trace.Report(message);
                            throw new InvalidDataException(message);
                        }

                        string primaryData = ParseMultiLineData(mbfReader, Environment.NewLine);
                        metadata.Primary = primaryData;
                        // don't go to next line; current line still needs to be processed
                        break;

                    // all the following are extracted the same way - possibly multiline
                    case "DEFINITION":
                        metadata.Definition = ParseMultiLineData(mbfReader, " ");
                        break;
                    case "ACCESSION":
                        data = ParseMultiLineData(mbfReader, " ");
                        metadata.Accession = new GenBankAccession();
                        string[] accessions = data.Split(' ');
                        metadata.Accession.Primary = accessions[0];

                        for (int i = 1; i < accessions.Length; i++)
                        {
                            metadata.Accession.Secondary.Add(accessions[i]);
                        }
                        break;

                    case "DBLINK":
                        tokens = mbfReader.LineData.Split(':');
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
                            ApplicationLog.WriteLine("WARN: unexpected DBLINK header: " + mbfReader.Line);
                        }
                        mbfReader.GoToNextLine();
                        break;

                    case "DBSOURCE":
                        metadata.DBSource = ParseMultiLineData(mbfReader, " ");
                        break;

                    case "KEYWORDS":
                        metadata.Keywords = ParseMultiLineData(mbfReader, " ");
                        break;

                    case "SEGMENT":
                        data = ParseMultiLineData(mbfReader, " ");
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
                                ApplicationLog.WriteLine("WARN: unexpected SEGMENT header: " + mbfReader.Line);
                            }

                            if (int.TryParse(tokens[1].Trim(), out outvalue))
                            {
                                metadata.Segment.Count = outvalue;
                            }
                            else
                            {
                                ApplicationLog.WriteLine("WARN: unexpected SEGMENT header: " + mbfReader.Line);
                            }
                        }
                        else
                        {
                            ApplicationLog.WriteLine("WARN: unexpected SEGMENT header: " + mbfReader.Line);
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
                        ApplicationLog.WriteLine(ToString() + "WARN: unknown {0} -> {1}", mbfReader.LineHeader, mbfReader.LineData);
                        string errMessage = String.Format(
                                    CultureInfo.CurrentCulture,
                                    Properties.Resource.ParseHeaderError,
                                    mbfReader.LineHeader);
                        Trace.Report(errMessage);
                        throw new InvalidDataException(errMessage);
                }
            }

            // check for required features
            if (!haveParsedLocus)
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.INVALID_INPUT_FILE, this.Name);
                Trace.Report(message);
                throw new InvalidDataException(message);
            }
        }

        /// <summary>
        /// Parses the GenBank LOCUS using a token based approach which provides more flexibility for 
        /// GenBank documents that do not follow the standard 100%.
        /// </summary>
        /// <param name="mbfReader"></param>
        /// <param name="sequence"></param>
        private void ParseLocusByTokens(MBFTextReader mbfReader, ref Sequence sequence)
        {
            var locusInfo = new GenBankLocusTokenParser().Parse(mbfReader.LineData);
           IAlphabet alphabet = GetAlphabet(locusInfo.MoleculeType);
           if (alphabet != sequence.Alphabet)
           {
              if (Alphabet != null && Alphabet != alphabet)
              {
                 Trace.Report(Resource.ParserIncorrectAlphabet);
                 throw new InvalidDataException(Resource.ParserIncorrectAlphabet);
              }
              sequence = new Sequence(alphabet, Encoding, sequence) { IsReadOnly = false };
           }

           sequence.ID = locusInfo.Name;
           sequence.MoleculeType = locusInfo.MoleculeType;
           var metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
           metadata.Locus = locusInfo;
           mbfReader.GoToNextLine();
        }

        private static void ParseReferences(MBFTextReader mbfReader, ref Sequence sequence)
        {
            GenBankMetadata metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
            IList<CitationReference> referenceList = metadata.References;
            CitationReference reference = null;
            //List<MetadataListItem<string>> referenceList = new List<MetadataListItem<string>>();
            //MetadataListItem<string> reference = null;

            while (mbfReader.HasLines)
            {
                if (mbfReader.LineHeader == "REFERENCE")
                {
                    // add previous reference
                    if (reference != null)
                    {
                        referenceList.Add(reference);
                    }

                    // check for start/end e.g. (bases 1 to 118), or prose notes
                    Match m = Regex.Match(mbfReader.LineData,
                        @"^(?<number>\d+)(\s+\((?<location>.*)\))?");
                    if (!m.Success)
                    {
                        string message = String.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.ParserReferenceError,
                                mbfReader.LineData);
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
                    mbfReader.GoToNextLine();
                }
                else if (mbfReader.Line.StartsWith(" ", StringComparison.Ordinal))
                {
                    switch (mbfReader.LineHeader)
                    {
                        // all the following are extracted the same way - possibly multiline
                        case "AUTHORS":
                            reference.Authors = ParseMultiLineData(mbfReader, " ");
                            break;
                        case "CONSRTM":
                            reference.Consortiums = ParseMultiLineData(mbfReader, " ");
                            break;
                        case "TITLE":
                            reference.Title = ParseMultiLineData(mbfReader, " ");
                            break;
                        case "JOURNAL":
                            reference.Journal = ParseMultiLineData(mbfReader, " ");
                            break;
                        case "REMARK":
                            reference.Remarks = ParseMultiLineData(mbfReader, " ");
                            break;
                        case "MEDLINE":
                            reference.Medline = ParseMultiLineData(mbfReader, " ");
                            break;
                        case "PUBMED":
                            reference.PubMed = ParseMultiLineData(mbfReader, " ");
                            break;

                        default:
                            string message = String.Format(
                                    CultureInfo.CurrentCulture,
                                    Properties.Resource.ParserInvalidReferenceField,
                                    mbfReader.LineHeader);
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

        private static void ParseComments(MBFTextReader mbfReader, ref Sequence sequence)
        {
            IList<string> commentList = ((GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey]).Comments;

            // don't skip blank lines in comments
            mbfReader.SkipBlankLines = false;

            while (mbfReader.HasLines && mbfReader.LineHeader == "COMMENT")
            {
                string data = ParseMultiLineData(mbfReader, Environment.NewLine);
                commentList.Add(data);
                // don't go to next line; current line still needs to be processed
            }

            // back to skipping blank lines when done with comments
            mbfReader.SkipBlankLines = true;
        }

        private static void ParseSource(MBFTextReader mbfReader, ref Sequence sequence)
        {
            string source = string.Empty;
            string organism = string.Empty;
            string classLevels = string.Empty;

            while (mbfReader.HasLines)
            {
                if (mbfReader.LineHeader == "SOURCE")
                {
                    // data can be multiline. spec says last line must end with period
                    // (note: this doesn't apply unless multiline)
                    bool lastDotted = true;
                    source = mbfReader.LineData;

                    mbfReader.GoToNextLine();
                    while (mbfReader.HasLines && !mbfReader.LineHasHeader)
                    {
                        source += " " + mbfReader.LineData;
                        lastDotted = (source.EndsWith(".", StringComparison.Ordinal));
                        mbfReader.GoToNextLine();
                    }

                    if (!lastDotted && Trace.Want(Trace.SeqWarnings))
                    {
                        Trace.Report("GenBank.ParseSource", Properties.Resource.OutOfSpec, source);
                    }

                    // don't go to next line; current line still needs to be processed
                }
                else if (mbfReader.Line[0] == ' ')
                {
                    if (mbfReader.LineHeader != "ORGANISM")
                    {
                        string message = String.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.ParserInvalidSourceField,
                                mbfReader.LineHeader);
                        Trace.Report(message);
                        throw new InvalidDataException(message);
                    }

                    // this also can be multiline
                    organism = mbfReader.LineData;

                    mbfReader.GoToNextLine();
                    while (mbfReader.HasLines && !mbfReader.LineHasHeader)
                    {
                        if (mbfReader.Line.EndsWith(";", StringComparison.Ordinal) || mbfReader.Line.EndsWith(".", StringComparison.Ordinal))
                        {
                            if (!String.IsNullOrEmpty(classLevels))
                            {
                                classLevels += " ";
                            }

                            classLevels += mbfReader.LineData;
                        }
                        else
                        {
                            organism += " " + mbfReader.LineData;
                        }
                        mbfReader.GoToNextLine();
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

        private void ParseFeatures(MBFTextReader mbfReader, ref Sequence sequence)
        {
            ILocationBuilder locBuilder = LocationBuilder;
            if (locBuilder == null)
            {
                throw new InvalidOperationException(Resource.NullLocationBuild);
            }

            // set data indent for features
            mbfReader.DataIndent = _featureDataIndent;

            // The sub-items of a feature are referred to as qualifiers.  These do not have unique
            // keys, so they are stored as lists in the SubItems dictionary.
            SequenceFeatures features = new SequenceFeatures();
            IList<FeatureItem> featureList = features.All;

            while (mbfReader.HasLines)
            {
                if (String.IsNullOrEmpty(mbfReader.Line) || mbfReader.LineHeader == "FEATURES")
                {
                    mbfReader.GoToNextLine();
                    continue;
                }

                if (mbfReader.Line[0] != ' ')
                {
                    // start of non-feature text
                    break;
                }

                if (!mbfReader.LineHasHeader)
                {
                    string message = Properties.Resource.GenbankEmptyFeature;
                    Trace.Report(message);
                    throw new InvalidDataException(message);
                }

                // check for multi-line location string
                string featureKey = mbfReader.LineHeader;
                string location = mbfReader.LineData;
                mbfReader.GoToNextLine();
                while (mbfReader.HasLines && !mbfReader.LineHasHeader &&
                    mbfReader.LineHasData && !mbfReader.LineData.StartsWith("/", StringComparison.Ordinal))
                {
                    location += mbfReader.LineData;
                    mbfReader.GoToNextLine();
                }

                // create features as MetadataListItems
                FeatureItem feature = new FeatureItem(featureKey, locBuilder.GetLocation(location));

                // process the list of qualifiers, which are each in the form of
                // /key="value"
                string qualifierKey = string.Empty;
                string qualifierValue = string.Empty;
                while (mbfReader.HasLines)
                {
                    if (!mbfReader.LineHasHeader && mbfReader.LineHasData)
                    {
                        // '/' denotes a continuation of the previous line
                        if (mbfReader.LineData.StartsWith("/", StringComparison.Ordinal))
                        {
                            // new qualifier; save previous if this isn't the first
                            if (!String.IsNullOrEmpty(qualifierKey))
                            {
                                AddQualifierToFeature(feature, qualifierKey, qualifierValue);
                            }

                            // set the key and value of this qualifier
                            int equalsIndex = mbfReader.LineData.IndexOf('=');
                            if (equalsIndex < 0)
                            {
                                // no value, just key (this is allowed, see NC_005213.gbk)
                                qualifierKey = mbfReader.LineData.Substring(1);
                                qualifierValue = string.Empty;
                            }
                            else if (equalsIndex > 0)
                            {
                                qualifierKey = mbfReader.LineData.Substring(1, equalsIndex - 1);
                                qualifierValue = mbfReader.LineData.Substring(equalsIndex + 1);
                            }
                            else
                            {
                                string message = String.Format(
                                        CultureInfo.CurrentCulture,
                                        Properties.Resource.GenbankInvalidFeature,
                                        mbfReader.Line);
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

                            qualifierValue += mbfReader.LineData;
                        }

                        mbfReader.GoToNextLine();
                    }
                    else if (mbfReader.Line.StartsWith("\t", StringComparison.Ordinal))
                    {
                        // this seems to be data corruption; but BioPerl test set includes
                        // (old, 2003) NT_021877.gbk which has this problem, so we
                        // handle it
                        ApplicationLog.WriteLine("WARN: nonstandard line format at line {0}: '{1}'",
                            mbfReader.LineNumber, mbfReader.Line);
                        qualifierValue += " " + mbfReader.Line.Trim();
                        mbfReader.GoToNextLine();
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
        private void ParseSequence(MBFTextReader mbfReader, ref Sequence sequence)
        {
            string message = string.Empty;

            GenBankMetadata metadata = (GenBankMetadata)sequence.Metadata[Helper.GenBankMetadataKey];
            // set data indent for sequence headers
            mbfReader.DataIndent = _dataIndent;

            while (mbfReader.HasLines)
            {
                if (mbfReader.Line.StartsWith("//", StringComparison.Ordinal))
                {
                    mbfReader.GoToNextLine();
                    break; // end of sequence record
                }

                switch (mbfReader.LineHeader)
                {
                    case "BASE COUNT":
                        // The BASE COUNT linetype is obsolete and was removed
                        // from the GenBank flatfile format in October 2003.  But if it is
                        // present, we will use it.  We get the untrimmed version since it
                        // starts with a right justified column.
                        metadata.BaseCount = mbfReader.Line.Substring(_dataIndent);
                        mbfReader.GoToNextLine();
                        break;

                    case "ORIGIN":
                        // Change Note: The original implementation would validate the alphabet every line
                        // which would greatly impact performance on large sequences.  This updates the method
                        // to improve performance by validating the alphabet after parsing the sequence.
                        ParseOrigin(mbfReader, metadata, ref sequence);
                        break;

                    case "CONTIG":
                        metadata.Contig = ParseMultiLineData(mbfReader, Environment.NewLine);
                        // don't go to next line; current line still needs to be processed
                        break;

                    default:
                        message = String.Format(
                                CultureInfo.CurrentCulture,
                                Properties.Resource.ParserUnexpectedLineInSequence,
                                mbfReader.Line);
                        Trace.Report(message);
                        throw new InvalidDataException(message);
                }
            }
        }

        private void ParseOrigin(MBFTextReader mbfReader, GenBankMetadata metadata, ref Sequence sequence)
        {
           // The origin line can contain optional data; don't put empty string into
           // metadata.
           if (!String.IsNullOrEmpty(mbfReader.LineData))
           {
              metadata.Origin = mbfReader.LineData;
           }
           mbfReader.GoToNextLine();
           IAlphabet alphabet = null;

           var sequenceBuilder = new StringBuilder();
           while (mbfReader.HasLines && mbfReader.Line[0] == ' ')
           {
              // Using a regex is too slow.
              int len = mbfReader.Line.Length;
              int k = 10;
              while (k < len)
              {
                 string seqData = mbfReader.Line.Substring(k, Math.Min(10, len - k));

                 sequenceBuilder.Append(seqData);
                 k += 11;
              }

              mbfReader.GoToNextLine();
           }

           var sequenceString = sequenceBuilder.ToString().Trim();
           if (!string.IsNullOrEmpty(sequenceString))
           {
               if (Alphabet == null)
               {
                   alphabet = IdentifyAlphabet(alphabet, sequenceString);

                   if (alphabet == null)
                   {
                       var message = String.Format(Resource.InvalidSymbolInString, mbfReader.Line);
                       Trace.Report(message);
                       throw new Exception(message);
                   }

                   if (sequence.Alphabet != alphabet)
                   {
                       Sequence seq = new Sequence(alphabet, Encoding, sequence)
                       {
                           MoleculeType = sequence.MoleculeType,
                           IsReadOnly = false
                       };
                       sequence.Clear();
                       sequence = seq;
                   }
               }

               sequence.InsertRange(sequence.Count, sequenceString);
           }
        }
        #endregion

        #region General Helper Methods

        // returns a string of the data for a header block that spans multiple lines
        private static string ParseMultiLineData(MBFTextReader mbfReader, string lineBreakSubstitution)
        {
            string data = mbfReader.LineData;
            mbfReader.GoToNextLine();

            // while succeeding lines start with no header, add to data
            while (mbfReader.HasLines && !mbfReader.LineHasHeader)
            {
                data += lineBreakSubstitution + mbfReader.LineData;
                mbfReader.GoToNextLine();
            }

            return data;
        }

        #endregion
    }
}
