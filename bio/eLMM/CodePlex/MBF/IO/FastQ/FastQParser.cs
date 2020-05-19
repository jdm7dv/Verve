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
using Bio.Encoding;
using Bio.Properties;
using Bio.Util.Logging;

namespace Bio.IO.FastQ
{
    /// <summary>
    /// A FastQParser reads from a source of text that is formatted according to the FASTQ 
    /// file specification, and converts the data to in-memory IQualitativeSequence objects.
    /// </summary>
    public class FastQParser : BasicSequenceParser, IVirtualSequenceParser
    {
        #region Fields
        /// <summary>
        /// Block size
        /// </summary>
        private int _blockSize = -1;
        /// <summary>
        /// Max number of block per sequence
        /// </summary>
        private int _maxNumberOfBlocks;
        /// <summary>
        /// Number of characters parsed inside a file.
        /// </summary>
        private long _numberOfCharactersParsed;
        /// <summary>
        /// file helper to determine the DV
        /// </summary>
        private FileLoadHelper _fileLoadHelper;
        /// <summary>
        /// parsing file name
        /// </summary>
        private string _fileName = string.Empty;
        /// <summary>
        /// either data virtualization forced or not
        /// </summary>
        private bool _isDataVirtualizationForced;
        /// <summary>
        /// list of sequence pointers
        /// </summary>
        private List<SequencePointer> _sequencePointers = new List<SequencePointer>();
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor chooses default encoding based on alphabet.
        /// </summary>
        public FastQParser()
            : base()
        {
            AutoDetectFastQFormat = true;
        }

        /// <summary>
        /// Constructor for setting the encoding.
        /// </summary>
        /// <param name="encoding">The encoding to use for parsed IQualitativeSequence objects.</param>
        public FastQParser(IEncoding encoding)
            : base(encoding)
        {
            AutoDetectFastQFormat = true;
        }

        #endregion Constructors

        #region Properties
        /// <summary>
        /// The FastQFormatType to be used for parsed IQualitativeSequence objects.
        /// Set AutoDetectFastQFormat property to false, else FastQ parser
        /// will ignore this property and try to identify the FastQFormatType for 
        /// each sequence data it parses.
        /// </summary>
        public FastQFormatType FastqType { get; set; }

        /// <summary>
        /// Gets the name of Parser i.e FastQ.
        /// This is intended to give developers some information 
        /// of the parser class.
        /// </summary>
        public override string Name
        {
            get { return Resource.FASTQ_NAME; }
        }

        /// <summary>
        /// Gets the description of FastQ parser.
        /// This is intended to give developers some information 
        /// of the parser class. This property returns a simple description of what the
        /// FastQParser class acheives.
        /// </summary>
        public override string Description
        {
            get { return Resource.FASTQPARSER_DESCRIPTION; }
        }

        /// <summary>
        /// Gets a comma seperated values of the possible
        /// file extensions for a FASTQ file.
        /// </summary>
        public override string FileTypes
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
        /// <returns>The list of parsed IQualitativeSequence objects.</returns>
        new public IList<IQualitativeSequence> Parse(TextReader reader)
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
        new public IList<IQualitativeSequence> Parse(TextReader reader, bool isReadOnly)
        {
            using (BioTextReader bioReader = new BioTextReader(reader))
            {
                return Parse(bioReader, isReadOnly);
            }
        }

        /// <summary>
        /// Parses a list of biological sequence data from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <returns>The list of parsed IQualitativeSequence objects.</returns>
        new public IList<IQualitativeSequence> Parse(string filename)
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
        new public IList<IQualitativeSequence> Parse(string filename, bool isReadOnly)
        {
            _fileName = filename;

             //check DV is requried
            if (filename != null)
            {
                _fileLoadHelper = new FileLoadHelper(filename);
                _blockSize = _fileLoadHelper.BlockSize;
                _maxNumberOfBlocks = _fileLoadHelper.MaxNumberOfBlocks;

                if (_isDataVirtualizationForced)
                {
                    _blockSize = FileLoadHelper.DefaultBlockSize;
                }
            }
            else
            {
                _blockSize = FileLoadHelper.DefaultFullLoadBlockSize;
                _maxNumberOfBlocks = 0;
            }

            SidecarFileProvider indexedProvider = null;

            // Check for sidecar
            if (IsDataVirtualizationEnabled)
            {
                try
                {
                    indexedProvider = SidecarFileProvider.GetProvider(filename);
                }
                catch (OperationCanceledException)
                {
                    indexedProvider = null;
                }
            }

            if (indexedProvider != null)
            {
                // Create virtual list and return
                return new VirtualQualitativeSequenceList(indexedProvider, this, indexedProvider.Count) { CreateSequenceAsReadOnly = isReadOnly };
            }
            else
            {
                using (BioTextReader bioReader = new BioTextReader(filename))
                {
                    return Parse(bioReader, isReadOnly);
                }
            }
        }

        /// <summary>
        /// Parses a single biological sequence data from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence data.</param>
        /// <returns>The parsed IQualitativeSequence object.</returns>
        new public IQualitativeSequence ParseOne(TextReader reader)
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
        new public IQualitativeSequence ParseOne(TextReader reader, bool isReadOnly)
        {
            using (BioTextReader bioReader = new BioTextReader(reader))
            {
                return ParseOne(bioReader, isReadOnly);
            }
        }

        /// <summary>
        /// Parses a single biological sequence data from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <returns>The parsed IQualitativeSequence object.</returns>
        new public IQualitativeSequence ParseOne(string filename)
        {
            return ParseOne(filename, true);
        }

        /// <summary>
        /// Parses a single biological sequence data from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed IQualitativeSequence object.</returns>
        new public IQualitativeSequence ParseOne(string filename, bool isReadOnly)
        {
            using (BioTextReader bioReader = new BioTextReader(filename))
            {
                return ParseOne(bioReader, isReadOnly);
            }
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
                sourceReader.BaseStream.Seek(pointer.StartingIndex + includesNewline, SeekOrigin.Begin);
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
        /// Parses a list of biological sequence data from a BioTextReader.
        /// </summary>
        /// <param name="bioReader">BioTextReader instance for a biological sequence data.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed IQualitativeSequence objects.</returns>
        new protected IList<IQualitativeSequence> Parse(BioTextReader bioReader, bool isReadOnly)
        {
            if (bioReader == null)
            {
                throw new ArgumentNullException("bioReader");
            }


            // no empty files allowed
            if (!bioReader.HasLines)
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, Resource.IONoTextToParse);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            if (!string.IsNullOrEmpty(bioReader.FileName) &&
                IsDataVirtualizationEnabled && SidecarFileProvider.IsIndexFileExists(bioReader.FileName))
            {
                while (bioReader.HasLines)
                {
                    ParseOne(bioReader, isReadOnly);
                }

                // Create sidecar          
                SidecarFileProvider provider = SidecarFileProvider.CreateIndexFile(bioReader.FileName, _sequencePointers);

                VirtualQualitativeSequenceList virtualSequences =
                         new VirtualQualitativeSequenceList(provider, this, _sequencePointers.Count) { CreateSequenceAsReadOnly = isReadOnly };

                _sequencePointers.Clear();

                return virtualSequences;
            }
            else
            {
                List<IQualitativeSequence> qualSequences = new List<IQualitativeSequence>();

                while (bioReader.HasLines)
                {
                    qualSequences.Add(ParseOne(bioReader, isReadOnly));
                }

                return qualSequences;
            }
        }

        /// <summary>
        /// Parses a single FASTQ text from a reader into a QualitativeSequence.
        /// </summary>
        /// <param name="bioReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>A new QualitativeSequence instance containing parsed data.</returns>
        protected override ISequence ParseOneWithSpecificFormat(BioTextReader bioReader, bool isReadOnly)
        {
            return ParseOneWithFastQFormat(bioReader, isReadOnly);
        }
        #endregion

        #region Private Members
        /// <summary>
        /// Parses a single FASTQ text from a reader into a QualitativeSequence.
        /// </summary>
        /// <param name="bioReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>A new QualitativeSequence instance containing parsed data.</returns>
        private IQualitativeSequence ParseOneWithFastQFormat(BioTextReader bioReader, bool isReadOnly)
        {
            SequencePointer sequencePointer = new SequencePointer();
            string message = string.Empty;

            // Check for '@' symbol at the first line.
            if (!bioReader.HasLines || !bioReader.Line.StartsWith("@", StringComparison.Ordinal))
            {
                message = string.Format(CultureInfo.CurrentCulture, Resource.INVAILD_INPUT_FILE, this.Name);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Process header line.
            string id = bioReader.GetLineField(2).Trim();

            _numberOfCharactersParsed += bioReader.Line.Length;
            sequencePointer.StartingIndex = _numberOfCharactersParsed;
            sequencePointer.StartingLine = bioReader.LineNumber;

            // Go to second line.
            bioReader.GoToNextLine();
            if (!bioReader.HasLines || string.IsNullOrEmpty(bioReader.Line))
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidSequenceLine, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Get sequence from second line.
            string sequenceLine = bioReader.Line;

            _numberOfCharactersParsed += bioReader.Line.Length;
            sequencePointer.EndingIndex = _numberOfCharactersParsed;

            // Goto third line.
            bioReader.GoToNextLine();

            // Check for '+' symbol in the third line.
            if (!bioReader.HasLines || !bioReader.Line.StartsWith("+", StringComparison.Ordinal))
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidQualityScoreHeaderLine, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            _numberOfCharactersParsed += bioReader.Line.Length;

            string qualScoreId = bioReader.GetLineField(2).Trim();

            if (!string.IsNullOrEmpty(qualScoreId) && !id.Equals(qualScoreId))
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidQualityScoreHeaderData, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Goto fourth line.
            bioReader.GoToNextLine();
            if (!bioReader.HasLines || string.IsNullOrEmpty(bioReader.Line))
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_EmptyQualityScoreLine, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            _numberOfCharactersParsed += bioReader.Line.Length;

            // Get the quality scores from the fourth line.
            byte[] qualScores = ASCIIEncoding.ASCII.GetBytes(bioReader.Line);

            // Check for sequence length and quality score length.
            if (sequenceLine.Length != bioReader.Line.Length)
            {
                string message1 = string.Format(CultureInfo.CurrentCulture, Resource.FastQ_InvalidQualityScoresLength, id);
                message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, message1);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            bioReader.GoToNextLine();

            IAlphabet alphabet = Alphabet;

            // Identify alphabet if it is not specified.
            if (alphabet == null)
            {
                alphabet = IdentifyAlphabet(alphabet, sequenceLine);

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

            // full load
            if (_blockSize == FileLoadHelper.DefaultFullLoadBlockSize)
            {
                return sequence;
            }

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
        /// Parses a single FastQ text from a BioTextReader.
        /// </summary>
        /// <param name="bioReader">BioTextReader instance for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting QualitativeSequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting QualitativeSequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed IQualitativeSequence objects.</returns>
        private IQualitativeSequence ParseOne(BioTextReader bioReader, bool isReadOnly)
        {
            // no empty files allowed
            if (!bioReader.HasLines)
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.IOFormatErrorMessage, Name, Resource.IONoTextToParse);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // do the actual parsing
            return ParseOneWithFastQFormat(bioReader, isReadOnly);
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
        /// Force the data virtualization from parser
        /// </summary>
        public bool ForceDataVirtualization
        {
            get
            {
                return _isDataVirtualizationForced;
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
                _isDataVirtualizationForced = value;
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

            IAlphabet alphabet = Alphabets.All.Single(A => A.Name.Equals(seqPointer.AlphabetName));
            Sequence sequence = new Sequence(alphabet);

            sequence.IsReadOnly = false;

            int start = (int)seqPointer.StartingIndex + startIndex;

            if (start >= seqPointer.EndingIndex)
                return null;

            int includesNewline = seqPointer.StartingLine * Environment.NewLine.Length;
            int len = (int)(seqPointer.EndingIndex - seqPointer.StartingIndex);

            using (BioTextReader bioReader = new BioTextReader(_fileName))
            {
                string sequenceString = bioReader.ReadBlock(startIndex, seqPointer.StartingIndex + includesNewline, count, len);
                sequence.InsertRange(0, sequenceString);
            }

            // default for partial load
            sequence.IsReadOnly = true;

            return sequence;
        }

        /// <summary>
        /// Get the quality scores of a particular sequence.
        /// </summary>
        /// <param name="qualScoresStartingIndex">The starting index of the quality 
        /// scores within the source file.</param>
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
    }
}
