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
using System.Text;
using Bio.Algorithms.Alignment;
using Bio.Encoding;
using Bio.Properties;
using Bio.Util;
using Bio.Util.Logging;

namespace Bio.IO.SAM
{
    /// <summary>
    /// A SAMParser reads from a source of text that is formatted according to the SAM
    /// file specification, and converts the data to in-memory SequenceAlignmentMap object.
    /// For advanced users, the ability to select an encoding for the internal memory representation is
    /// provided. There is also a default encoding for each alphabet that may be encountered.
    /// Documentation for the latest SAM file format can be found at
    /// http://samtools.sourceforge.net/SAM1.pdf
    /// </summary>
    public class SAMParser : ISequenceAlignmentParser
    {
        #region  Constants
        /// <summary>
        /// Constant to hold SAM alignment header line pattern.
        /// </summary>
        public const string HeaderLinePattern = "(@..){1}((\t..:[^\t]+)+)";

        /// <summary>
        /// Constant to hold SAM optional filed line pattern.
        /// </summary>
        public const string OptionalFieldLinePattern = "..:.:([^\t\n\r]+)";

        /// <summary>
        /// Holds the qualitative value type.
        /// </summary>
        private const FastQFormatType QualityFormatType = FastQFormatType.Sanger;

        #endregion

        #region Private Fields
        /// <summary>
        /// Constant for tab and space delimeter.
        /// </summary>
        private static char[] tabDelim = "\t".ToCharArray();

        /// <summary>
        ///  Constant for colon delimeter.
        /// </summary>
        private static char[] colonDelim = ":".ToCharArray();

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor chooses default encoding based on alphabet.
        /// </summary>
        public SAMParser()
        {
            RefSequences = new List<ISequence>();
        }

        /// <summary>
        /// Constructor for setting the encoding.
        /// </summary>
        /// <param name="encoding">The encoding to use for parsed ISequence objects.</param>
        public SAMParser(IEncoding encoding)
            : this()
        {
            Encoding = encoding;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the name of the sequence alignment parser being
        /// implemented. This is intended to give the
        /// developer some information of the parser type.
        /// </summary>
        public string Name
        {
            get { return Resource.SAM_NAME; }
        }

        /// <summary>
        /// Gets the description of the sequence alignment parser being
        /// implemented. This is intended to give the
        /// developer some information of the parser.
        /// </summary>
        public string Description
        {
            get { return Resource.SAMPARSER_DESCRIPTION; }
        }

        /// <summary>
        /// The alphabet to use for sequences in parsed SequenceAlignmentMap objects.
        /// Always returns DNA.
        /// </summary>
        public IAlphabet Alphabet
        {
            get
            {
                return Alphabets.DNA;
            }
            set
            {
                throw new NotSupportedException(Resource.SAMParserAlphabetCantBeSet);
            }
        }

        /// <summary>
        /// The encoding to use for sequences in parsed SequenceAlignmentMap objects.
        /// </summary>
        public IEncoding Encoding { get; set; }

        /// <summary>
        /// Gets the file extensions that the parser implementation
        /// will support.
        /// </summary>
        public string FileTypes
        {
            get { return Resource.SAM_FILEEXTENSION; }
        }

        /// <summary>
        /// Reference sequences, used to resolve "=" symbol in the sequence data.
        /// </summary>
        public IList<ISequence> RefSequences { get; private set; }
        #endregion

        #region Public Static Methods

        /// <summary>
        /// Parses SAM alignment header from specified file.
        /// </summary>
        /// <param name="fileName">file name.</param>
        public static SAMAlignmentHeader ParserSAMHeader(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            using (BioTextReader bioReader = new BioTextReader(fileName))
            {
                return ParserSAMHeader(bioReader);
            }
        }

        /// <summary>
        /// Parses SAM alignment header from specified text reader.
        /// </summary>
        /// <param name="reader">Text reader.</param>
        public static SAMAlignmentHeader ParserSAMHeader(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            using (BioTextReader bioReader = new BioTextReader(reader))
            {
                return ParserSAMHeader(bioReader);
            }
        }

        /// <summary>
        /// Parases sequence data and quality values and updates SAMAlignedSequence instance.
        /// </summary>
        /// <param name="alignedSeq">SAM aligned Sequence.</param>
        /// <param name="alphabet">Alphabet of the sequence to be created.</param>
        /// <param name="Encoding">Encoding to use while creating sequence.</param>
        /// <param name="sequencedata">Sequence data.</param>
        /// <param name="qualitydata">Quality values.</param>
        /// <param name="refSeq">Reference sequence if known.</param>
        /// <param name="isReadOnly">Flag to indicate whether the new sequence is required to in readonly or not.</param>
        public static void ParseQualityNSequence(SAMAlignedSequence alignedSeq, IAlphabet alphabet, IEncoding Encoding, string sequencedata, string qualitydata, ISequence refSeq, bool isReadOnly)
        {
            if (alignedSeq == null)
            {
                throw new ArgumentNullException("alignedSeq");
            }

            if (string.IsNullOrWhiteSpace(sequencedata))
            {
                throw new ArgumentNullException("sequencedata");
            }

            if (string.IsNullOrWhiteSpace(qualitydata))
            {
                throw new ArgumentNullException("qualitydata");
            }

            bool isQualitativeSequence = true;
            string message = string.Empty;
            byte[] qualScores = null;
            FastQFormatType fastQType = QualityFormatType;

            if (sequencedata.Equals("*"))
            {
                return;
            }

            if (qualitydata.Equals("*"))
            {
                isQualitativeSequence = false;
            }

            if (isQualitativeSequence)
            {
                // Get the quality scores from the fourth line.
                qualScores = ASCIIEncoding.ASCII.GetBytes(qualitydata);

                // Check for sequence length and quality score length.
                if (sequencedata.Length != qualitydata.Length)
                {
                    string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidQualityScoresLength, alignedSeq.QName);
                    message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Resource.SAM_NAME, message1);
                    Trace.Report(message);
                    throw new FileFormatException(message);
                }
            }

            // get "." symbol indexes.
            int index = sequencedata.IndexOf('.', 0);
            while (index > -1)
            {
                alignedSeq.DotSymbolIndexes.Add(index);
                index = sequencedata.IndexOf('.', index);
            }

            // replace "." with N
            if (alignedSeq.DotSymbolIndexes.Count > 0)
            {
                sequencedata = sequencedata.Replace('.', 'N');
            }

            // get "=" symbol indexes.
            index = sequencedata.IndexOf('=', 0);
            while (index > -1)
            {
                alignedSeq.EqualSymbolIndexes.Add(index);
                index = sequencedata.IndexOf('=', index);
            }

            // replace "=" with corresponding symbol from refSeq.
            if (alignedSeq.EqualSymbolIndexes.Count > 0)
            {
                if (refSeq == null)
                {
                    throw new ArgumentException(Resource.RefSequenceNofFound);
                }

                for (int i = 0; i < alignedSeq.EqualSymbolIndexes.Count; i++)
                {
                    index = alignedSeq.EqualSymbolIndexes[i];
                    sequencedata = sequencedata.Remove(index, 1);
                    sequencedata = sequencedata.Insert(index, refSeq[index].Symbol.ToString());
                }
            }

            ISequence sequence = null;
            if (isQualitativeSequence)
            {
                QualitativeSequence qualSeq = null;
                if (Encoding == null)
                {
                    qualSeq = new QualitativeSequence(alphabet, fastQType, sequencedata, qualScores);
                }
                else
                {
                    qualSeq = new QualitativeSequence(alphabet, fastQType, Encoding, sequencedata, qualScores);
                }

                qualSeq.ID = alignedSeq.QName;
                qualSeq.IsReadOnly = isReadOnly;
                sequence = qualSeq;
            }
            else
            {
                Sequence seq = null;
                if (Encoding == null)
                {
                    seq = new Sequence(alphabet, sequencedata);
                }
                else
                {
                    seq = new Sequence(alphabet, Encoding, sequencedata);
                }

                seq.ID = alignedSeq.QName;
                seq.IsReadOnly = isReadOnly;
                sequence = seq;
            }

            alignedSeq.QuerySequence = sequence;
        }
        #endregion

        #region Private Static Methods
        /// <summary>
        /// Parses SAM alignment header from specified BioTextReader.
        /// </summary>
        /// <param name="bioReader">Bio text reader.</param>
        private static SAMAlignmentHeader ParserSAMHeader(BioTextReader bioReader)
        {
            SAMAlignmentHeader samHeader = new SAMAlignmentHeader();
            if (bioReader.HasLines && bioReader.Line.StartsWith(@"@", StringComparison.OrdinalIgnoreCase))
            {
                while (bioReader.HasLines && bioReader.Line.StartsWith(@"@", StringComparison.OrdinalIgnoreCase))
                {
                    string[] tokens = bioReader.Line.Split(tabDelim, StringSplitOptions.RemoveEmptyEntries);
                    string recordTypecode = tokens[0].Substring(1);
                    // Validate the header format.
                    ValidateHeaderLineFormat(bioReader.Line);

                    SAMRecordField headerLine = null;
                    if (string.Compare(recordTypecode, "CO", StringComparison.OrdinalIgnoreCase) != 0)
                    {
                        List<string> tags = new List<string>();
                        headerLine = new SAMRecordField(recordTypecode);
                        for (int i = 1; i < tokens.Length; i++)
                        {
                            string tagToken = tokens[i];
                            string tagName = tagToken.Substring(0, 2);
                            tags.Add(tagName);
                            headerLine.Tags.Add(new SAMRecordFieldTag(tagName, tagToken.Substring(3)));
                        }

                        samHeader.RecordFields.Add(headerLine);
                    }
                    else
                    {
                        samHeader.Comments.Add(bioReader.Line.Substring(4));
                    }

                    bioReader.GoToNextLine();
                }

                string message = samHeader.IsValid();
                if (!string.IsNullOrEmpty(message))
                {
                    throw new FormatException(message);
                }
            }

            return samHeader;
        }

        // validates header.
        private static bool ValidateHeaderLineFormat(string headerline)
        {
            if (headerline.Length >= 3)
            {
                if (!headerline.StartsWith("@CO", StringComparison.OrdinalIgnoreCase))
                {
                    string headerPattern = HeaderLinePattern;
                    return Helper.IsValidRegexValue(headerPattern, headerline);
                }
            }

            return false;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Parses a list of sequence alignment texts from a reader.
        /// </summary>
        /// <param name="reader">A reader for a sequence alignment text.</param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        IList<ISequenceAlignment> ISequenceAlignmentParser.Parse(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            List<ISequenceAlignment> alignments = new List<ISequenceAlignment>();
            using (BioTextReader bioReader = new BioTextReader(reader))
            {
                alignments.Add(Parse(bioReader, true));
            }

            return alignments;
        }

        /// <summary>
        /// Parses a list of sequence alignment texts from a reader.
        /// </summary>
        /// <param name="reader">A reader for a sequence alignment text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the sequence alignment should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        IList<ISequenceAlignment> ISequenceAlignmentParser.Parse(TextReader reader, bool isReadOnly)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            List<ISequenceAlignment> alignments = new List<ISequenceAlignment>();
            using (BioTextReader bioReader = new BioTextReader(reader))
            {
                alignments.Add(Parse(bioReader, isReadOnly));
            }

            return alignments;
        }

        /// <summary>
        /// Parses a list of sequence alignment texts from a file.
        /// </summary>
        /// <param name="fileName">The name of a sequence alignment file.</param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        IList<ISequenceAlignment> ISequenceAlignmentParser.Parse(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            List<ISequenceAlignment> alignments = new List<ISequenceAlignment>();
            using (BioTextReader bioReader = new BioTextReader(fileName))
            {
                alignments.Add(Parse(bioReader, true));
            }

            return alignments;
        }

        /// <summary>
        /// Parses a list of sequence alignment texts from a file.
        /// </summary>
        /// <param name="fileName">The name of a sequence alignment file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the sequence alignment should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        IList<ISequenceAlignment> ISequenceAlignmentParser.Parse(string fileName, bool isReadOnly)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            List<ISequenceAlignment> alignments = new List<ISequenceAlignment>();
            using (BioTextReader bioReader = new BioTextReader(fileName))
            {
                alignments.Add(Parse(bioReader, isReadOnly));
            }

            return alignments;
        }

        /// <summary>
        /// Parses a sequence alignment texts from a reader.
        /// </summary>
        /// <param name="reader">A reader for a sequence alignment text.</param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        public ISequenceAlignment ParseOne(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return Parse(reader, true);
        }

        /// <summary>
        /// Parses a sequence alignment texts from a reader.
        /// </summary>
        /// <param name="reader">A reader for a sequence alignment text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the sequence alignment should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        public ISequenceAlignment ParseOne(TextReader reader, bool isReadOnly)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return Parse(reader, isReadOnly);
        }

        /// <summary>
        /// Parses a sequence alignment texts from a file.
        /// </summary>
        /// <param name="fileName">The name of a sequence alignment file.</param>
        /// <returns>ISequenceAlignment object.</returns>
        public ISequenceAlignment ParseOne(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            return Parse(fileName, true);
        }

        /// <summary>
        /// Parses a sequence alignment texts from a file.
        /// </summary>
        /// <param name="fileName">The name of a sequence alignment file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the sequence alignment should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        public ISequenceAlignment ParseOne(string fileName, bool isReadOnly)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            return Parse(fileName, isReadOnly);
        }

        /// <summary>
        /// Parses a sequence alignment texts from a file.
        /// </summary>
        /// <param name="fileName">file name.</param>
        /// <returns>SequenceAlignmentMap object.</returns>
        public SequenceAlignmentMap Parse(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            return Parse(fileName, true);
        }

        /// <summary>
        /// Parses a sequence alignment texts from a file.
        /// </summary>
        /// <param name="fileName">file name.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the sequence alignment should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>SequenceAlignmentMap object.</returns>
        public SequenceAlignmentMap Parse(string fileName, bool isReadOnly)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            using (BioTextReader bioReader = new BioTextReader(fileName))
            {
                return Parse(bioReader, isReadOnly);
            }
        }

        /// <summary>
        /// Parses a sequence alignment texts from a text reader.
        /// </summary>
        /// <param name="reader">Text reader.</param>
        /// <returns>SequenceAlignmentMap object.</returns>
        public SequenceAlignmentMap Parse(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return Parse(reader, true);
        }

        /// <summary>
        /// Parses a sequence alignment texts from a file.
        /// </summary>
        /// <param name="reader">Text reader.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the sequence alignment should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>SequenceAlignmentMap object.</returns>
        public SequenceAlignmentMap Parse(TextReader reader, bool isReadOnly)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            using (BioTextReader bioReader = new BioTextReader(reader))
            {
                return Parse(bioReader, isReadOnly);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Parses alignments in SAM format from a reader into a SequenceAlignmentMap object.
        /// </summary>
        /// <param name="bioReader">A reader for a biological sequence alignment text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether sequencs in the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.</param>
        /// <returns>A new SequenceAlignmentMap instance containing parsed data.</returns>
        protected SequenceAlignmentMap ParseOneWithSpecificFormat(BioTextReader bioReader, bool isReadOnly)
        {
            if (bioReader == null)
            {
                throw new ArgumentNullException("bioReader");
            }

            // no empty files allowed
            if (!bioReader.HasLines)
            {
                throw new FormatException(Resource.Parser_NoTextErrorMessage);
            }

            // Parse the alignment header.
            SAMAlignmentHeader header = ParserSAMHeader(bioReader);

            SequenceAlignmentMap seqAlignt = new SequenceAlignmentMap(header);

            // Parse aligned sequences 
            ParseSequences(seqAlignt, bioReader, isReadOnly);
            return seqAlignt;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Parses SequenceAlignmentMap using a BioTextReader.
        /// </summary>
        /// <param name="bioReader">A reader for a sequence alignment text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether sequences in the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        private SequenceAlignmentMap Parse(BioTextReader bioReader, bool isReadOnly)
        {
            // Parse Header, Loop through the blocks and parse
            while (bioReader.HasLines)
            {
                if (string.IsNullOrEmpty(bioReader.Line.Trim()))
                {
                    bioReader.GoToNextLine();
                    continue;
                }

                return ParseOneWithSpecificFormat(bioReader, isReadOnly);
            }

            return null;
        }

        // parses sequence.
        private void ParseSequences(SequenceAlignmentMap seqAlignment, BioTextReader bioReader, bool isReadOnly)
        {
            while (bioReader.HasLines && !bioReader.Line.StartsWith(@"@", StringComparison.OrdinalIgnoreCase))
            {
                string[] tokens = bioReader.Line.Split(tabDelim, StringSplitOptions.RemoveEmptyEntries);
                SAMAlignedSequence alignedSeq = new SAMAlignedSequence();

                alignedSeq.QName = tokens[0];
                alignedSeq.Flag = SAMAlignedSequenceHeader.GetFlag(tokens[1]);
                alignedSeq.RName = tokens[2];
                alignedSeq.Pos = int.Parse(tokens[3], CultureInfo.InvariantCulture);
                alignedSeq.MapQ = int.Parse(tokens[4], CultureInfo.InvariantCulture);
                alignedSeq.CIGAR = tokens[5];
                alignedSeq.MRNM = tokens[6].Equals("=") ? alignedSeq.RName : tokens[6];
                alignedSeq.MPos = int.Parse(tokens[7], CultureInfo.InvariantCulture);
                alignedSeq.ISize = int.Parse(tokens[8], CultureInfo.InvariantCulture);
                string message = alignedSeq.IsValidHeader();

                if (!string.IsNullOrEmpty(message))
                {
                    throw new FormatException(message);
                }

                ISequence refSeq = null;

                if (RefSequences != null && RefSequences.Count > 0)
                {
                    refSeq = RefSequences.FirstOrDefault(R => string.Compare(R.ID, alignedSeq.RName, StringComparison.OrdinalIgnoreCase) == 0);
                }

                ParseQualityNSequence(alignedSeq, Alphabet, Encoding, tokens[9], tokens[10], refSeq, isReadOnly);
                SAMOptionalField optField = null;
                for (int i = 11; i < tokens.Length; i++)
                {
                    optField = new SAMOptionalField();
                    string optionalFieldRegExpn = OptionalFieldLinePattern;
                    if (!Helper.IsValidRegexValue(optionalFieldRegExpn, tokens[i]))
                    {
                        message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidOptionalField, tokens[i]);
                        throw new FormatException(message);
                    }

                    string[] opttokens = tokens[i].Split(colonDelim, StringSplitOptions.RemoveEmptyEntries);
                    optField.Tag = opttokens[0];
                    optField.VType = opttokens[1];
                    optField.Value = opttokens[2];
                    message = optField.IsValid();
                    if (!string.IsNullOrEmpty(message))
                    {
                        throw new FormatException(message);
                    }

                    alignedSeq.OptionalFields.Add(optField);
                }

                seqAlignment.QuerySequences.Add(alignedSeq);
                bioReader.GoToNextLine();
            }
        }

        #endregion
    }
}
