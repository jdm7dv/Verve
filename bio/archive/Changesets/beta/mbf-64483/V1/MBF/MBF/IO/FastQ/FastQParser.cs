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
using MBF.Encoding;
using MBF.Properties;
using MBF.Util.Logging;
using MBF.Util;

namespace MBF.IO.FastQ
{
    /// <summary>
    /// A FastQParser reads from a source of text that is formatted according to the FASTQ 
    /// file specification and converts the data to in-memory IQualitativeSequence objects.
    /// </summary>
    public class FastQParser : IVirtualSequenceParser, IDisposable
    {
        #region Fields
        /// <summary>
        /// The block size.
        /// </summary>
        private int _blockSize = -1;
        /// <summary>
        /// The maximum number of blocks per sequence.
        /// </summary>
        private int _maxNumberOfBlocks;
        /// <summary>
        /// The name of the file being parsed.
        /// </summary>
        private string _fileName = string.Empty;
        /// <summary>
        /// Indicates whether data virtualization has been explictly enabled.
        /// </summary>
        private bool _isDataVirtualizationEnforced;
        /// <summary>
        /// A list of sequence pointers.
        /// </summary>
        private List<SequencePointer> _sequencePointers = new List<SequencePointer>();
        /// <summary>
        /// A stream reader to use data virtualization on biological sequence files.
        /// </summary>
        private MBFStreamReader _mbfStreamReader;
        /// <summary>
        /// File size in KBs to enable data virtualization
        /// </summary>
        private int _enforceDataVirtualizationByFileSize;

        /// <summary>
        /// Behavior common to all sequence parsers.
        /// </summary>
        private readonly CommonSequenceParser _commonSequenceParser;
        #endregion

        #region Constructors
        /// <summary>
        /// The default constructor which chooses the default encoding based on the alphabet.
        /// </summary>
        public FastQParser()
        {
            AutoDetectFastQFormat = true;
            _commonSequenceParser = new CommonSequenceParser();
        }

        /// <summary>
        /// A constructor to set the encoding used.
        /// </summary>
        /// <param name="encoding">The encoding to use for the parsed IQualitativeSequence objects.</param>
        public FastQParser(IEncoding encoding)
        {
            AutoDetectFastQFormat = true;
            Encoding = encoding;
            _commonSequenceParser = new CommonSequenceParser();
        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// The alphabet to use for parsed ISequence objects.  If this is not set, an alphabet will
        /// be determined based on the file being parsed.
        /// </summary>
        public IAlphabet Alphabet { get; set; }

        /// <summary>
        /// The encoding to use for parsed ISequence objects.  If this is not set, the default
        /// for the given alphabet will be used.
        /// </summary>
        public IEncoding Encoding { get; set; }

        /// <summary>
        /// The FastQFormatType to be used for parsed IQualitativeSequence objects.
        /// Set AutoDetectFastQFormat property to false, otherwise the FastQ parser
        /// will ignore this property and try to identify the FastQFormatType for 
        /// each sequence data it parses.
        /// </summary>
        public FastQFormatType FastqType { get; set; }

        /// <summary>
        /// Gets the name of the parser, in this case, FastQ.
        /// </summary>
        public string Name
        {
            get { return Resource.FASTQ_NAME; }
        }

        /// <summary>
        /// Gets a description of the FastQ parser.
        /// This is intended to give developers information about the parser
        /// class. This property returns a simple description of what the
        /// FastQParser class acheives.
        /// </summary>
        public string Description
        {
            get { return Resource.FASTQPARSER_DESCRIPTION; }
        }

        /// <summary>
        /// Gets a string of comma-separated values of the
        /// possible file extensions for a FASTQ file.
        /// </summary>
        public string FileTypes
        {
            get { return Resource.FASTQ_FILEEXTENSION; }
        }

        /// <summary>
        /// If this flag is true then FastQParser will ignore the Type property 
        /// and try to identify the FastQFormatType for each sequence data it parses.
        /// By default this property is set to true.
        /// 
        /// If this flag is false then FastQParser will parse the sequence data 
        /// according to the FastQFormatType specified in Type property.
        /// </summary>
        public bool AutoDetectFastQFormat { get; set; }
        #endregion Properties

        #region ISequenceParser Members
        /// <summary>
        /// Parses a list of biological sequence data from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence text.</param>
        /// <returns>The list of parsed ISequence objects.</returns>
        IList<ISequence> ISequenceParser.Parse(TextReader reader)
        {
            using (MBFTextReader mbfReader = new MBFTextReader(reader))
            {
                return Parse(mbfReader, true);
            }
        }

        /// <summary>
        /// Parses a list of biological sequence data from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequence objects.</returns>
        IList<ISequence> ISequenceParser.Parse(TextReader reader, bool isReadOnly)
        {
            using (MBFTextReader mbfReader = new MBFTextReader(reader))
            {
                return Parse(mbfReader, isReadOnly);
            }
        }

        /// <summary>
        /// Parses a list of biological sequence data from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <returns>The list of parsed ISequence objects.</returns>
        IList<ISequence> ISequenceParser.Parse(string filename)
        {
            return ((ISequenceParser)this).Parse(filename, true);
        }

        /// <summary>
        /// Parses a list of biological sequence data from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequence objects.</returns>
        IList<ISequence> ISequenceParser.Parse(string filename, bool isReadOnly)
        {
            VerifyForDV(filename);

            VirtualQualitativeSequenceList virList = null;
            // Check for sidecar
            if (IsDataVirtualizationEnabled)
            {
                virList = ParseWithDV(isReadOnly);
                if (virList != null)
                {
                    return virList;
                }
            }

            // non-DV parsing
            using (MBFTextReader mbfReader = new MBFTextReader(filename))
            {
                return Parse(mbfReader, isReadOnly);
            }

        }

        /// <summary>
        /// Parses a single biological sequence data from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence data.</param>
        /// <returns>The parsed ISequence object.</returns>
        ISequence ISequenceParser.ParseOne(TextReader reader)
        {
            return ParseOne(reader);
        }

        /// <summary>
        /// Parses a single biological sequence data from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence data.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed ISequence object.</returns>
        ISequence ISequenceParser.ParseOne(TextReader reader, bool isReadOnly)
        {
            return ParseOne(reader, isReadOnly);
        }

        /// <summary>
        /// Parses a single biological sequence data from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <returns>The parsed ISequence object.</returns>
        ISequence ISequenceParser.ParseOne(string filename)
        {
            return ParseOne(filename);
        }

        /// <summary>
        /// Parses a single biological sequence from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in read-only mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's IsReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed ISequence object.</returns>
        ISequence ISequenceParser.ParseOne(string filename, bool isReadOnly)
        {
            return ParseOne(filename, isReadOnly);
        }

        /// <summary>
        /// Parses a list of biological sequence data from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence text.</param>
        /// <returns>The list of parsed IQualitativeSequence objects.</returns>
        public IList<IQualitativeSequence> Parse(TextReader reader)
        {
            return Parse(reader, true);
        }

        /// <summary>
        /// Parses a list of biological sequence data from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed IQualitativeSequence objects.</returns>
        public IList<IQualitativeSequence> Parse(TextReader reader, bool isReadOnly)
        {
            using (MBFTextReader mbfReader = new MBFTextReader(reader))
            {
                return ParseQualSeqs(mbfReader, isReadOnly);
            }
        }

        /// <summary>
        /// Parses a list of biological sequence data from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <returns>The list of parsed IQualitativeSequence objects.</returns>
        public IList<IQualitativeSequence> Parse(string filename)
        {
            return Parse(filename, true);
        }

        /// <summary>
        /// Parses a list of biological sequence data from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed IQualitativeSequence objects.</returns>
        public IList<IQualitativeSequence> Parse(string filename, bool isReadOnly)
        {
            VerifyForDV(filename);
           
            VirtualQualitativeSequenceList virList=null;
            // Check for sidecar
            if (IsDataVirtualizationEnabled)
            {
                virList = ParseWithDV(isReadOnly);
                if (virList != null)
                {
                    return virList;
                }
            }

            // non-DV parsing
            using (MBFTextReader mbfReader = new MBFTextReader(filename))
            {
                return ParseQualSeqs(mbfReader, isReadOnly);
            }

        }

        /// <summary>
        /// Parses a single biological sequence data from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence data.</param>
        /// <returns>The parsed IQualitativeSequence object.</returns>
        public IQualitativeSequence ParseOne(TextReader reader)
        {
            return ParseOne(reader, true);
        }

        /// <summary>
        /// Parses a single biological sequence data from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence data.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed IQualitativeSequence object.</returns>
        public IQualitativeSequence ParseOne(TextReader reader, bool isReadOnly)
        {
            using (MBFTextReader mbfReader = new MBFTextReader(reader))
            {
                return ParseOne(mbfReader, isReadOnly);
            }
        }

        /// <summary>
        /// Parses a single biological sequence data from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <returns>The parsed IQualitativeSequence object.</returns>
        public IQualitativeSequence ParseOne(string filename)
        {
            return ParseOne(filename, true);
        }

        /// <summary>
        /// Parses a single biological sequence from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in read-only mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's IsReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed IQualitativeSequence object.</returns>
        public IQualitativeSequence ParseOne(string filename, bool isReadOnly)
        {
            if (IsDataVirtualizationEnabled)
            {
                using (MBFStreamReader mbfStreamReader = new MBFStreamReader(filename))
                {
                    return ParseOne(mbfStreamReader, isReadOnly);
                }
            }
            else
            {
                using (MBFTextReader mbfReader = new MBFTextReader(filename))
                {
                    return ParseOne(mbfReader, isReadOnly);
                }
            }
        }
        #endregion

        #region IVirtualSequenceParser Members
        /// <summary>
        /// Indicates whether data virtualization is enabled or not.
        /// </summary>
        public bool IsDataVirtualizationEnabled
        {
            get
            {
                if (_blockSize == FileLoadHelper.DefaultFullLoadBlockSize)
                    return false;
                return true;
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
                if (value)
                {
                    _blockSize = FileLoadHelper.DefaultBlockSize;
                    _maxNumberOfBlocks = 5;
                }
                else
                {
                    _blockSize = FileLoadHelper.DefaultFullLoadBlockSize;
                    _maxNumberOfBlocks = 0;
                }
                _isDataVirtualizationEnforced = value;
            }
        }

        /// <summary>
        /// Parses a range of sequence items starting from the specified index in the sequence.
        /// </summary>
        /// <param name="startIndex">The zero-based index at which to begin parsing.</param>
        /// <param name="count">The number of symbols to parse.</param>
        /// <param name="seqPointer">The sequence pointer of that sequence.</param>
        /// <returns>The parsed sequence.</returns>
        public ISequence ParseRange(int startIndex, int count, SequencePointer seqPointer)
        {
            if (0 > startIndex)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            if (0 >= count)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            if (seqPointer == null)
            {
                throw new ArgumentNullException("seqPointer");
            }

            // if the start index exceeds the sequence boundary
            if ((long)startIndex + seqPointer.IndexOffsets[0] >= seqPointer.IndexOffsets[1])
            {
                return null;
            }

            IAlphabet alphabet = Alphabets.All.Single(A => A.Name.Equals(seqPointer.AlphabetName));
            Sequence sequence = new Sequence(alphabet) { IsReadOnly = false };

            if (_mbfStreamReader == null || !_mbfStreamReader.CanRead)
            {
                _mbfStreamReader = new MBFStreamReader(_fileName);
            }

            long filePosition = startIndex + seqPointer.IndexOffsets[0];

            int sequenceLength = (int)(seqPointer.IndexOffsets[1] - seqPointer.IndexOffsets[0]);

            if (count + startIndex >= sequenceLength)
            {
                count = (int)(sequenceLength - startIndex);
            }

            char[] buffer = _mbfStreamReader.ReadChars(filePosition, count);
            sequence.InsertRange(0, new string(buffer));

            // default for partial load
            sequence.IsReadOnly = true;

            return sequence;
        }

        /// <summary>
        /// Gets the quality scores of a particular sequence.
        /// </summary>
        /// <param name="qualScoresStartingIndex">
        /// The starting index of the quality 
        /// scores within the source file.
        /// </param>
        /// <returns>The quality scores.</returns>
        public byte[] GetQualityScores(long qualScoresStartingIndex)
        {
            byte[] qualScores;

            using (StreamReader reader = new StreamReader(_fileName))
            {
                reader.BaseStream.Seek(qualScoresStartingIndex, SeekOrigin.Begin);

                string qualScoresString = reader.ReadLine();

                ASCIIEncoding encoding = new ASCIIEncoding();
                qualScores = encoding.GetBytes(qualScoresString);
            }

            return qualScores;
        }
        #endregion

        #region BasicSequenceParser Members

        /// <summary>
        /// Get sequence ID corresponding to a given sequence pointer
        /// </summary>
        /// <param name="pointer">Sequence pointer</param>
        /// <returns>Sequence ID</returns>
        public string GetSequenceID(SequencePointer pointer)
        {
            if (pointer == null)
            {
                throw new ArgumentNullException("pointer");
            }

            using (StreamReader sourceReader = new StreamReader(_fileName))
            {
                int includesNewline = pointer.StartingLine * Environment.NewLine.Length;

                // Read Sequence ID by looking back from the sequence starting index
                sourceReader.BaseStream.Seek(pointer.IndexOffsets[0] + includesNewline, SeekOrigin.Begin);
                sourceReader.BaseStream.Seek(-2, SeekOrigin.Current);

                while (sourceReader.BaseStream.ReadByte() != '@')
                {
                    sourceReader.BaseStream.Seek(-2, SeekOrigin.Current);
                }

                pointer.Id = sourceReader.ReadLine();
                return pointer.Id;
            }
        }

        /// <summary>
        /// Parses a list of biological sequence data from a MBFTextReader.
        /// </summary>
        /// <param name="mbfReader">MBFTextReader instance for a biological sequence data.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed IQualitativeSequence objects.</returns>
        protected IList<IQualitativeSequence> ParseQualSeqs(MBFTextReader mbfReader, bool isReadOnly)
        {
            if (mbfReader == null)
            {
                throw new ArgumentNullException("mbfReader");
            }

            // no empty files allowed
            if (!mbfReader.HasLines)
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, Resource.IONoTextToParse);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            List<IQualitativeSequence> qualSequences = new List<IQualitativeSequence>();

            while (mbfReader.HasLines)
            {
                qualSequences.Add(ParseOne(mbfReader, isReadOnly));
            }

            return qualSequences;
        }

        /// <summary>
        /// Parses a list of biological sequence data from a MBFTextReader.
        /// </summary>
        /// <param name="mbfReader">MBFTextReader instance for a biological sequence data.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed IQualitativeSequence objects.</returns>
        protected IList<ISequence> Parse(MBFTextReader mbfReader, bool isReadOnly)
        {
            if (mbfReader == null)
            {
                throw new ArgumentNullException("mbfReader");
            }

            // no empty files allowed
            if (!mbfReader.HasLines)
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, Resource.IONoTextToParse);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            List<ISequence> qualSequences = new List<ISequence>();

            while (mbfReader.HasLines)
            {
                qualSequences.Add(ParseOne(mbfReader, isReadOnly));
            }

            return qualSequences;
        }

        /// <summary>
        /// Parses a single FASTQ text from a reader into a QualitativeSequence.
        /// </summary>
        /// <param name="mbfReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>A new QualitativeSequence instance containing parsed data.</returns>
        protected ISequence ParseOneWithSpecificFormat(MBFTextReader mbfReader, bool isReadOnly)
        {
            return ParseOneWithFastQFormat(mbfReader, isReadOnly);
        }

        /// <summary>
        /// Parses a single FASTQ text from a reader into a QualitativeSequence.
        /// </summary>
        /// <param name="mbfReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>A new QualitativeSequence instance containing parsed data.</returns>
        protected ISequence ParseOneWithSpecificFormat(MBFStreamReader mbfReader, bool isReadOnly)
        {
            return ParseOneWithFastQFormat(mbfReader, isReadOnly);
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Parses a single FASTQ text from a reader into a QualitativeSequence.
        /// </summary>
        /// <param name="mbfReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>A new QualitativeSequence instance containing parsed data.</returns>
        private IQualitativeSequence ParseOneWithFastQFormat(MBFTextReader mbfReader, bool isReadOnly)
        {
            string message;

            // Check for '@' symbol at the first line.
            if (!mbfReader.HasLines || !mbfReader.Line.StartsWith("@", StringComparison.Ordinal))
            {
                message = string.Format(CultureInfo.CurrentCulture, Resource.INVALID_INPUT_FILE, Name);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Process header line.
            string id = mbfReader.GetLineField(2).Trim();

            // Go to second line.
            mbfReader.GoToNextLine();
            if (!mbfReader.HasLines || string.IsNullOrEmpty(mbfReader.Line))
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidSequenceLine, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Get sequence from second line.
            string sequenceLine = mbfReader.Line;

            // Goto third line.
            mbfReader.GoToNextLine();

            // Check for '+' symbol in the third line.
            if (!mbfReader.HasLines || !mbfReader.Line.StartsWith("+", StringComparison.Ordinal))
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidQualityScoreHeaderLine, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            string qualScoreId = mbfReader.GetLineField(2).Trim();

            if (!string.IsNullOrEmpty(qualScoreId) && !id.Equals(qualScoreId))
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidQualityScoreHeaderData, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Goto fourth line.
            mbfReader.GoToNextLine();
            if (!mbfReader.HasLines || string.IsNullOrEmpty(mbfReader.Line))
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_EmptyQualityScoreLine, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Get the quality scores from the fourth line.
            byte[] qualScores = ASCIIEncoding.ASCII.GetBytes(mbfReader.Line);

            // Check for sequence length and quality score length.
            if (sequenceLine.Length != mbfReader.Line.Length)
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidQualityScoresLength, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            mbfReader.GoToNextLine();

            IAlphabet alphabet = Alphabet;

            // Identify alphabet if it is not specified.
            if (alphabet == null)
            {
                alphabet = _commonSequenceParser.IdentifyAlphabet(alphabet, sequenceLine);

                if (alphabet == null)
                {
                    string message1 = string.Format(CultureInfo.CurrentCulture, Resource.InvalidSymbolInString, sequenceLine);
                    message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                    Trace.Report(message);
                    throw new FileFormatException(message);
                }
            }

            FastQFormatType fastQType = FastqType;

            // Identify fastq format type if AutoDetectFastQFormat property is set to true.
            if (AutoDetectFastQFormat)
            {
                fastQType = IdentifyFastQFormatType(qualScores);
            }

            QualitativeSequence sequence = null;

            if (Encoding == null)
            {
                sequence = new QualitativeSequence(alphabet, fastQType, sequenceLine, qualScores);
            }
            else
            {
                sequence = new QualitativeSequence(alphabet, fastQType, Encoding, sequenceLine, qualScores);
            }

            sequence.ID = id;
            sequence.IsReadOnly = isReadOnly;

            return sequence;
        }

        /// <summary>
        /// Parses a single FASTQ text from a reader into a QualitativeSequence.
        /// </summary>
        /// <param name="mbfReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>A new QualitativeSequence instance containing parsed data.</returns>
        private IQualitativeSequence ParseOneWithFastQFormat(MBFStreamReader mbfReader, bool isReadOnly)
        {
            SequencePointer sequencePointer = new SequencePointer();

            string message;

            // Check for '@' symbol at the first line.
            if (!mbfReader.HasLines || !mbfReader.Line.StartsWith("@", StringComparison.Ordinal))
            {
                message = string.Format(CultureInfo.CurrentCulture, Resource.INVALID_INPUT_FILE, Name);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Process header line.
            string id = mbfReader.GetLineField(2).Trim();

            // save sequence starting index
            sequencePointer.IndexOffsets[0] = mbfReader.Position;

            // Go to second line.
            mbfReader.GoToNextLine();
            if (!mbfReader.HasLines || string.IsNullOrEmpty(mbfReader.Line))
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidSequenceLine, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Get sequence from second line.
            string sequenceLine = mbfReader.Line;

            //save sequence ending index
            sequencePointer.IndexOffsets[1] = sequencePointer.IndexOffsets[0] + mbfReader.Line.Length;

            // Goto third line.
            mbfReader.GoToNextLine();

            // Check for '+' symbol in the third line.
            if (!mbfReader.HasLines || !mbfReader.Line.StartsWith("+", StringComparison.Ordinal))
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidQualityScoreHeaderLine, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            string qualScoreId = mbfReader.GetLineField(2).Trim();

            if (!string.IsNullOrEmpty(qualScoreId) && !id.Equals(qualScoreId))
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidQualityScoreHeaderData, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Goto fourth line.
            mbfReader.GoToNextLine();
            if (!mbfReader.HasLines || string.IsNullOrEmpty(mbfReader.Line))
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_EmptyQualityScoreLine, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Get the quality scores from the fourth line.
            byte[] qualScores = ASCIIEncoding.ASCII.GetBytes(mbfReader.Line);

            // Check for sequence length and quality score length.
            if (sequenceLine.Length != mbfReader.Line.Length)
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidQualityScoresLength, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            mbfReader.GoToNextLine();

            IAlphabet alphabet = Alphabet;

            // Identify alphabet if it is not specified.
            if (alphabet == null)
            {
                alphabet = _commonSequenceParser.IdentifyAlphabet(alphabet, sequenceLine);

                if (alphabet == null)
                {
                    string message1 = string.Format(CultureInfo.CurrentCulture, Resource.InvalidSymbolInString, sequenceLine);
                    message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                    Trace.Report(message);
                    throw new FileFormatException(message);
                }
            }

            FastQFormatType fastQType = FastqType;

            // Identify fastq format type if AutoDetectFastQFormat property is set to true.
            if (AutoDetectFastQFormat)
            {
                fastQType = IdentifyFastQFormatType(qualScores);
            }

            QualitativeSequence sequence = null;

            if (Encoding == null)
            {
                sequence = new QualitativeSequence(alphabet, fastQType, sequenceLine, qualScores);
            }
            else
            {
                sequence = new QualitativeSequence(alphabet, fastQType, Encoding, sequenceLine, qualScores);
            }

            sequence.ID = id;
            sequence.IsReadOnly = isReadOnly;

            sequencePointer.AlphabetName = sequence.Alphabet.Name;
            sequencePointer.Id = sequence.ID;
            _sequencePointers.Add(sequencePointer);

            FileVirtualQualitativeSequenceProvider dataProvider = new FileVirtualQualitativeSequenceProvider(this, sequencePointer)
            {
                BlockSize = _blockSize,
                MaxNumberOfBlocks = _maxNumberOfBlocks
            };

            sequence.VirtualQualitativeSequenceProvider = dataProvider;
            return sequence;
        }

        /// <summary>
        /// Identifies Alphabet for the sepecified quality scores.
        /// This method returns,
        ///  Illumina - if the quality scores are in the range of 64 to 104
        ///  Solexa   - if the quality scores are in the range of 59 to 104
        ///  Sanger   - if the quality scores are in the range of 33 to 126.
        /// </summary>
        /// <param name="qualScores">Quality scores.</param>
        /// <returns>Returns appropriate FastQFormatType for the specified quality scores.</returns>
        private FastQFormatType IdentifyFastQFormatType(byte[] qualScores)
        {
            List<byte> uniqueScores = qualScores.Distinct().ToList();
            FastQFormatType formatType = FastQFormatType.Illumina;
            foreach (byte qualScore in uniqueScores)
            {
                if (qualScore >= QualitativeSequence.SangerMinQualScore && qualScore <= QualitativeSequence.SangerMaxQualScore)
                {
                    if (formatType == FastQFormatType.Illumina)
                    {
                        if (qualScore >= QualitativeSequence.IlluminaMinQualScore && qualScore <= QualitativeSequence.IlluminaMaxQualScore)
                        {
                            continue;
                        }

                        if (qualScore >= QualitativeSequence.SolexaMinQualScore && qualScore <= QualitativeSequence.SolexaMaxQualScore)
                        {
                            formatType = FastQFormatType.Solexa;
                            continue;
                        }

                        if (qualScore >= QualitativeSequence.SangerMinQualScore && qualScore <= QualitativeSequence.SangerMaxQualScore)
                        {
                            formatType = FastQFormatType.Sanger;
                            continue;
                        }
                    }

                    if (formatType == FastQFormatType.Solexa)
                    {
                        if (qualScore >= QualitativeSequence.SolexaMinQualScore && qualScore <= QualitativeSequence.SolexaMaxQualScore)
                        {
                            continue;
                        }

                        if (qualScore >= QualitativeSequence.SangerMinQualScore && qualScore <= QualitativeSequence.SangerMaxQualScore)
                        {
                            formatType = FastQFormatType.Sanger;
                            continue;
                        }
                    }
                }
                else
                {
                    string message1 = string.Format(CultureInfo.CurrentCulture, Resource.InvalidQualityScore, qualScore);
                    string message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                    Trace.Report(message);
                    throw new FileFormatException(message);
                }
            }

            return formatType;
        }

        /// <summary>
        /// Parses a single FastQ text from a MBFTextReader.
        /// </summary>
        /// <param name="mbfReader">MBFTextReader instance for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed IQualitativeSequence objects.</returns>
        private IQualitativeSequence ParseOne(MBFTextReader mbfReader, bool isReadOnly)
        {
            // no empty files allowed
            if (!mbfReader.HasLines)
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, Resource.IONoTextToParse);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // do the actual parsing
            return ParseOneWithFastQFormat(mbfReader, isReadOnly);
        }

        /// <summary>
        /// Parses a single FastQ text from a MBFStreamReader.
        /// </summary>
        /// <param name="mbfReader">MBFStreamReader instance for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed IQualitativeSequence objects.</returns>
        private IQualitativeSequence ParseOne(MBFStreamReader mbfReader, bool isReadOnly)
        {
            // no empty files allowed
            if (!mbfReader.HasLines)
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, Resource.IONoTextToParse);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // do the actual parsing
            return ParseOneWithFastQFormat(mbfReader, isReadOnly);
        }

        /// <summary>
        /// Validates if DV is reaquired or not if required then sets the required fields.
        /// </summary>
        /// <param name="filename">Filename to verify.</param>
        private void VerifyForDV(string filename)
        {
            // default to full load
            _blockSize = FileLoadHelper.DefaultFullLoadBlockSize;
            _maxNumberOfBlocks = 0;

            // check if DV is required
            if (filename != null)
            {
                _fileName = filename;

                FileInfo fileInfo = new FileInfo(_fileName);
                _enforceDataVirtualizationByFileSize = EnforceDataVirtualizationByFileSize * FileLoadHelper.KBytes;
                if ((_enforceDataVirtualizationByFileSize != 0 && fileInfo.Length >= _enforceDataVirtualizationByFileSize)
                    || _isDataVirtualizationEnforced)
                {
                    _blockSize = FileLoadHelper.DefaultBlockSize;
                    _maxNumberOfBlocks = FileLoadHelper.DefaultMaxNumberOfBlocks;
                }
            }
        }

        /// <summary>
        /// Parses file with DV and returns Virtual Qualitative Sequences list.
        /// </summary>
        /// <param name="isReadOnly">Flag to indicate whether the sequences returned should be set to readonly or not.</param>
        private VirtualQualitativeSequenceList ParseWithDV(bool isReadOnly)
        {
            SidecarFileProvider sidecarFileProvider = null;

            sidecarFileProvider = new SidecarFileProvider(_fileName);
            sidecarFileProvider.Close();

            // if valid sidecar file exists
            if (sidecarFileProvider.IsSidecarValid)
            {
                // Create virtual list and return
                return new VirtualQualitativeSequenceList(sidecarFileProvider, this, sidecarFileProvider.Count) { CreateSequenceAsReadOnly = isReadOnly };
            }

            // else create new sidecar
            using (sidecarFileProvider = new SidecarFileProvider(_fileName, true))
            {
                using (_mbfStreamReader = new MBFStreamReader(_fileName))
                {
                    if (sidecarFileProvider.SidecarFileExists)
                    {
                        try
                        {
                            while (_mbfStreamReader.HasLines)
                            {
                                ParseOne(_mbfStreamReader, isReadOnly);
                            }

                            // Create sidecar          
                            sidecarFileProvider.CreateSidecarFile(_mbfStreamReader.FileName, _sequencePointers);

                            VirtualQualitativeSequenceList virtualSequences =
                                     new VirtualQualitativeSequenceList(sidecarFileProvider, this, _sequencePointers.Count) { CreateSequenceAsReadOnly = isReadOnly };

                            _sequencePointers.Clear();
                            return virtualSequences;
                        }
                        catch (Exception)
                        {
                            sidecarFileProvider.Cleanup();
                        }
                    }
                }
            }

            return null;
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

        /// <summary>
        /// Disposes of all resources held by this object.
        /// </summary>
        /// <param name="disposing">If disposing equals true, dispose all resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_mbfStreamReader != null)
                {
                    _mbfStreamReader.Dispose();
                }
            }
        }

        #endregion
    }
}
