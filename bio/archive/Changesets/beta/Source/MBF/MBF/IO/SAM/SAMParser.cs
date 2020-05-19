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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using MBF.Algorithms.Alignment;
using MBF.Encoding;
using MBF.Properties;
using MBF.Util;
using MBF.Util.Logging;

namespace MBF.IO.SAM
{
    /// <summary>
    /// A SAMParser reads from a source of text that is formatted according to the SAM
    /// file specification, and converts the data to in-memory SequenceAlignmentMap object.
    /// For advanced users, the ability to select an encoding for the internal memory representation is
    /// provided. There is also a default encoding for each alphabet that may be encountered.
    /// Documentation for the latest SAM file format can be found at
    /// http://samtools.sourceforge.net/SAM1.pdf
    /// </summary>
    public class SAMParser : IDisposable, IVirtualSequenceAlignmentParser
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
        /// Holds optional field regular expression object.
        /// </summary>
        private static Regex OptionalFieldRegex = new Regex(OptionalFieldLinePattern);

        /// <summary>
        /// Constant for tab and space delimeter.
        /// </summary>
        private static char[] tabDelim = "\t".ToCharArray();

        /// <summary>
        ///  Constant for colon delimeter.
        /// </summary>
        private static char[] colonDelim = ":".ToCharArray();

        /// <summary>
        /// Indicates whether data virtualization has been explictly enabled.
        /// </summary>
        private bool _isDataVirtualizationEnforced;

        /// <summary>
        /// The number of lines that have been parsed.
        /// </summary>
        private int _lineCount;

        /// <summary>
        /// The name of the file being parsed.
        /// </summary>
        private string _fileName;

        /// <summary>
        /// Indicates whether the parsed sequence is read-only.
        /// </summary>
        private bool _isReadOnly;

        /// <summary>
        /// The length of the SAM header.
        /// </summary>
        private static long _headerLength;

        /// <summary>
        /// Holds a pointer to the sidecar file for the parsed sequence
        /// when data virtualization is enabled.
        /// </summary>
        private SidecarFileProvider _sidecarFileProvider;

        /// <summary>
        /// A stream reader to use data virtualization on biological sequence files.
        /// </summary>
        private MBFStreamReader _mbfStreamReader;

        /// <summary>
        /// File size in KBs to enable data virtualization
        /// </summary>
        private int _enforceDataVirtualizationByFileSize;
        #endregion

        #region Constructors
        /// <summary>
        /// The default constructor which chooses the default encoding based on the alphabet.
        /// </summary>
        public SAMParser()
        {
            RefSequences = new List<ISequence>();
        }

        /// <summary>
        /// A constructor to set the encoding used.
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
        public static SAMAlignmentHeader ParseSAMHeader(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            using (MBFTextReader mbfReader = new MBFTextReader(fileName))
            {
                return ParseSAMHeader(mbfReader);
            }
        }

        /// <summary>
        /// Parses SAM alignment header from specified text reader.
        /// </summary>
        /// <param name="reader">Text reader.</param>
        public static SAMAlignmentHeader ParseSAMHeader(TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            MBFTextReader mbfReader = new MBFTextReader(reader);
            return ParseSAMHeader(mbfReader);
        }

        /// <summary>
        /// Parses SAM alignment header from specified MBFTextReader.
        /// </summary>
        /// <param name="mbfReader">MBF text reader.</param>
        public static SAMAlignmentHeader ParseSAMHeader(MBFTextReader mbfReader)
        {
            if (mbfReader == null)
            {
                throw new ArgumentNullException("mbfReader");
            }

            _headerLength = 0;
            SAMAlignmentHeader samHeader = new SAMAlignmentHeader();
            if (mbfReader.HasLines && mbfReader.Line.StartsWith(@"@", StringComparison.OrdinalIgnoreCase))
            {
                while (mbfReader.HasLines && mbfReader.Line.StartsWith(@"@", StringComparison.OrdinalIgnoreCase))
                {
                    _headerLength += mbfReader.Line.Length;
                    string[] tokens = mbfReader.Line.Split(tabDelim, StringSplitOptions.RemoveEmptyEntries);
                    string recordTypecode = tokens[0].Substring(1);
                    // Validate the header format.
                    ValidateHeaderLineFormat(mbfReader.Line);

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
                        samHeader.Comments.Add(mbfReader.Line.Substring(4));
                    }

                    mbfReader.GoToNextLine();
                }

                string message = samHeader.IsValid();
                if (!string.IsNullOrEmpty(message))
                {
                    throw new FormatException(message);
                }
            }

            return samHeader;
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
                alignedSeq.DotSymbolIndexes.Add(index++);
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
                alignedSeq.EqualSymbolIndexes.Add(index++);
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
        /// Parses SAM alignment header from specified MBFStreamReader.
        /// </summary>
        /// <param name="mbfReader">MBF text reader.</param>
        private static SAMAlignmentHeader ParseSAMHeader(MBFStreamReader mbfReader)
        {
            _headerLength = 0;
            SAMAlignmentHeader samHeader = new SAMAlignmentHeader();
            if (mbfReader.HasLines && mbfReader.Line.StartsWith(@"@", StringComparison.OrdinalIgnoreCase))
            {
                while (mbfReader.HasLines && mbfReader.Line.StartsWith(@"@", StringComparison.OrdinalIgnoreCase))
                {
                    _headerLength += mbfReader.Line.Length;
                    string[] tokens = mbfReader.Line.Split(tabDelim, StringSplitOptions.RemoveEmptyEntries);
                    string recordTypecode = tokens[0].Substring(1);
                    // Validate the header format.
                    ValidateHeaderLineFormat(mbfReader.Line);

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
                        samHeader.Comments.Add(mbfReader.Line.Substring(4));
                    }

                    mbfReader.GoToNextLine();
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
            using (MBFTextReader mbfReader = new MBFTextReader(reader))
            {
                alignments.Add(Parse(mbfReader, true));
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
            using (MBFTextReader mbfReader = new MBFTextReader(reader))
            {
                alignments.Add(Parse(mbfReader, isReadOnly));
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
            ISequenceAlignment alignment = Parse(fileName, true);
            return new List<ISequenceAlignment>() { alignment };
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
            ISequenceAlignment alignment = Parse(fileName, isReadOnly);
            return new List<ISequenceAlignment>() { alignment };
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

            _fileName = fileName;

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

            _fileName = fileName;

            // check if DV is required

            FileInfo fileInfo = new FileInfo(_fileName);

            _enforceDataVirtualizationByFileSize = EnforceDataVirtualizationByFileSize * FileLoadHelper.KBytes;
            if ((_enforceDataVirtualizationByFileSize != 0 && fileInfo.Length >= _enforceDataVirtualizationByFileSize)
                || _isDataVirtualizationEnforced)
            {
                EnforceDataVirtualization = true;
            }

            SequenceAlignmentMap sequenceAlignmentMap = null;
            SAMAlignmentHeader header = null;

            if (IsDataVirtualizationEnabled)
            {
                VirtualAlignedSequenceList<SAMAlignedSequence> queries = null;

                using (MBFStreamReader mbfReader = new MBFStreamReader(fileName))
                {
                    header = ParseSAMHeader(mbfReader);

                    if (header.Comments.Count == 0 && header.RecordFields.Count == 0)
                    {
                        try
                        {
                            // verify whether this is a valid SAM file by parsing a single sequence
                            ParseSequence(mbfReader.Line, true, Alphabet, Encoding, RefSequences);
                        }
                        catch (IndexOutOfRangeException)
                        {
                            throw new FileFormatException(Resource.SAM_InvalidInputFile);
                        }
                    }

                    _sidecarFileProvider = new SidecarFileProvider(fileName);

                    // if a valid sidecar does not exist then recreate it
                    if (_sidecarFileProvider.SidecarFileExists && _sidecarFileProvider.IsSidecarValid == false)
                    {
                        ParseSequences(mbfReader);
                    }

                    if (_sidecarFileProvider.IsSidecarValid)
                    {
                        queries = new VirtualAlignedSequenceList<SAMAlignedSequence>(_sidecarFileProvider, this, _sidecarFileProvider.Count);
                        sequenceAlignmentMap = new SequenceAlignmentMap(header, queries);
                        return sequenceAlignmentMap;
                    }
                }
            }

            using (MBFTextReader mbfReader = new MBFTextReader(fileName))
            {
                return Parse(mbfReader, isReadOnly);
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

            using (MBFTextReader mbfReader = new MBFTextReader(reader))
            {
                return Parse(mbfReader, isReadOnly);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Parses alignments in SAM format from a reader into a SequenceAlignmentMap object.
        /// </summary>
        /// <param name="mbfReader">A reader for a biological sequence alignment text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether sequences in the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.</param>
        /// <returns>A new SequenceAlignmentMap instance containing parsed data.</returns>
        protected SequenceAlignmentMap ParseOneWithSpecificFormat(MBFTextReader mbfReader, bool isReadOnly)
        {
            _isReadOnly = isReadOnly;

            if (mbfReader == null)
            {
                throw new ArgumentNullException("mbfReader");
            }

            // no empty files allowed
            if (!mbfReader.HasLines)
            {
                throw new FormatException(Resource.Parser_NoTextErrorMessage);
            }

            // Parse the alignment header.
            SAMAlignmentHeader header = ParseSAMHeader(mbfReader);

            SequenceAlignmentMap sequenceAlignmentMap = null;

            sequenceAlignmentMap = new SequenceAlignmentMap(header);
            // Parse aligned sequences 
            ParseSequences(sequenceAlignmentMap, mbfReader, isReadOnly);

            return sequenceAlignmentMap;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Parses SequenceAlignmentMap using a MBFTextReader.
        /// </summary>
        /// <param name="mbfReader">A reader for a sequence alignment text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether sequences in the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        private SequenceAlignmentMap Parse(MBFTextReader mbfReader, bool isReadOnly)
        {
            _fileName = mbfReader.FileName;

            // Parse Header, Loop through the blocks and parse
            while (mbfReader.HasLines)
            {
                if (string.IsNullOrEmpty(mbfReader.Line.Trim()))
                {
                    mbfReader.GoToNextLine();
                    continue;
                }

                return ParseOneWithSpecificFormat(mbfReader, isReadOnly);
            }

            return null;
        }

        /// <summary>
        /// Parse a single sequence using a MBFTextReader.
        /// </summary>
        /// <param name="bioText">sequence alignment text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the sequences in the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <param name="alphabet">Alphabet of the sequences.</param>
        /// <param name="encoding">Required encoding.</param>
        /// <param name="referenceSequences">Reference sequences.</param>
        private static SAMAlignedSequence ParseSequence(string bioText, bool isReadOnly, IAlphabet alphabet, IEncoding encoding, IList<ISequence> referenceSequences)
        {
            const int optionalTokenStartingIndex = 11;
            string[] tokens = bioText.Split(tabDelim, StringSplitOptions.RemoveEmptyEntries);

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

            ISequence refSeq = null;

            if (referenceSequences != null && referenceSequences.Count > 0)
            {
                refSeq = referenceSequences.FirstOrDefault(R => string.Compare(R.ID, alignedSeq.RName, StringComparison.OrdinalIgnoreCase) == 0);
            }

            ParseQualityNSequence(alignedSeq, alphabet, encoding, tokens[9], tokens[10], refSeq, isReadOnly);
            SAMOptionalField optField = null;
            string message;
            for (int i = optionalTokenStartingIndex; i < tokens.Length; i++)
            {
                optField = new SAMOptionalField();
                if (!Helper.IsValidRegexValue(OptionalFieldRegex, tokens[i]))
                {
                    message = string.Format(CultureInfo.CurrentCulture, Resource.InvalidOptionalField, tokens[i]);
                    throw new FormatException(message);
                }

                string[] opttokens = tokens[i].Split(colonDelim, StringSplitOptions.RemoveEmptyEntries);
                optField.Tag = opttokens[0];
                optField.VType = opttokens[1];
                optField.Value = opttokens[2];

                alignedSeq.OptionalFields.Add(optField);
            }

            return alignedSeq;
        }

        /// <summary>
        /// Parse a single sequence using a MBFTextReader.
        /// </summary>
        /// <param name="mbfReader">A reader for a sequence alignment text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the sequences in the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        public static SAMAlignedSequence ParseSequence(MBFTextReader mbfReader, bool isReadOnly)
        {
            return ParseSequence(mbfReader, isReadOnly, Alphabets.DNA, null, null);
        }

        /// <summary>
        /// Parse a single sequence using a MBFTextReader.
        /// </summary>
        /// <param name="mbfReader">A reader for a sequence alignment text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the sequences in the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <param name="alphabet">Alphbatet to use while creating sequence instance.</param>
        /// <param name="encoding">Encoding to use. Pass Null to consider default value.</param>
        /// <param name="referenceSeqeunces">Reference sequences if known, else pass null.</param>
        public static SAMAlignedSequence ParseSequence(MBFTextReader mbfReader, bool isReadOnly, IAlphabet alphabet, IEncoding encoding, IList<ISequence> referenceSeqeunces)
        {
            if (mbfReader == null)
            {
                throw new ArgumentNullException("mbfReader");
            }

            return ParseSequence(mbfReader.Line, isReadOnly, alphabet, encoding, referenceSeqeunces);
        }

        /// <summary>
        /// Parses all the sequences in a SAM file.
        /// </summary>
        /// <param name="seqAlignment">SequenceAlignmentMap object</param>
        /// <param name="mbfReader">A reader for the sequence alignment text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the sequences in the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        private void ParseSequences(SequenceAlignmentMap seqAlignment, MBFTextReader mbfReader, bool isReadOnly)
        {
            while (mbfReader.HasLines && !mbfReader.Line.StartsWith(@"@", StringComparison.OrdinalIgnoreCase))
            {
                SAMAlignedSequence alignedSeq = ParseSequence(mbfReader, isReadOnly, Alphabet, Encoding, RefSequences);
                seqAlignment.QuerySequences.Add(alignedSeq);
                mbfReader.GoToNextLine();
            }
        }

        /// <summary>
        /// Parses all the sequences in a SAM file.
        /// This method is used only in data virtualization scenarios.
        /// </summary>
        /// <param name="mbfReader">A reader for the sequence alignment text.</param>
        private void ParseSequences(MBFStreamReader mbfReader)
        {
            // if DV enabled
            if (IsDataVirtualizationEnabled && _sidecarFileProvider.SidecarFileExists)
            {
                try
                {
                    while (mbfReader.HasLines && !mbfReader.Line.StartsWith(@"@", StringComparison.OrdinalIgnoreCase))
                    {
                        SequencePointer sequencePointer = new SequencePointer { AlphabetName = Alphabets.DNA.Name };

                        // sequence starting index
                        sequencePointer.IndexOffsets[0] = mbfReader.CurrentLineStartingIndex;
                        // sequence ending index
                        sequencePointer.IndexOffsets[1] = mbfReader.CurrentLineStartingIndex + mbfReader.Line.Length;

                        // Write each sequence pointer to the sidecar file immediately
                        _sidecarFileProvider.WritePointer(sequencePointer);

                        mbfReader.GoToNextLine();
                        _lineCount++;
                    }

                    _sidecarFileProvider.Close();
                }
                catch (Exception)
                {
                    _sidecarFileProvider.Cleanup();
                }
            }
        }


        #endregion

        #region IVirtualSequenceAlignmentParser Members

        /// <summary>
        /// Indicates whether data virtualization is enabled or not.
        /// </summary>
        public bool IsDataVirtualizationEnabled
        {
            get
            {
                if (_isDataVirtualizationEnforced)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// File size in KBs to enable data virtualization. If the file size is
        /// larger, then data virtualization is loaded automatically.
        /// </summary>
        public int EnforceDataVirtualizationByFileSize { get; set; }

        /// <summary>
        /// Enforces data virtualization on the parser.
        /// </summary>
        public bool EnforceDataVirtualization
        {
            get
            {
                return _isDataVirtualizationEnforced;
            }
            set
            {
                _isDataVirtualizationEnforced = value;
            }
        }

        /// <summary>
        /// Parses the sequence represented by the specified sequence pointer.
        /// </summary>
        /// <param name="pointer">
        /// A sequence pointer which holds information about the sequence to be retrieved.
        /// </param>
        /// <returns>IAlignedSequence object.</returns>
        public IAlignedSequence ParseAlignedSequence(SequencePointer pointer)
        {
            if (pointer == null)
            {
                throw new ArgumentNullException("pointer");
            }

            if (string.IsNullOrEmpty(_fileName))
            {
                throw new NotSupportedException(Resource.DataVirtualizationNeedsInputFile);
            }

            if (pointer.IndexOffsets[0] >= pointer.IndexOffsets[1])
                return null;

            if (_mbfStreamReader == null || !_mbfStreamReader.CanRead)
            {
                _mbfStreamReader = new MBFStreamReader(_fileName);
            }

            string buffer;

            _mbfStreamReader.Seek(pointer.IndexOffsets[0], SeekOrigin.Begin);

            buffer = _mbfStreamReader.ReadLine();

            return ParseSequence(buffer, _isReadOnly, Alphabet, Encoding, RefSequences);
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// If the TextReader was opened by this object, dispose it.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Disposes this object.
        /// </summary>
        /// <param name="disposing">If true disposes resourses held by this instance.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_sidecarFileProvider != null)
                {
                    _sidecarFileProvider.Close();
                }

                if (_mbfStreamReader != null)
                {
                    _mbfStreamReader.Dispose();
                }
            }
        }
        #endregion


    }
}
