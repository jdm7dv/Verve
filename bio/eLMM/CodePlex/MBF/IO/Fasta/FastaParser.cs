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

namespace Bio.IO.Fasta
{
    /// <summary>
    /// A FastaParser reads from a source of text that is formatted according to the FASTA flat
    /// file specification, and converts the data to in-memory ISequence objects.  For advanced
    /// users, the ability to select an encoding for the internal memory representation is
    /// provided. There is also a default encoding for each alphabet that may be encountered.
    /// Documentation for the latest GenBank file format can be found at
    /// http://www.ncbi.nlm.nih.gov/blast/fasta.shtml
    /// 
    /// FastaParser wants to support Data Virtualization, so its coming from IVirtualSequenceParser
    /// interface.
    /// </summary>
    public class FastaParser : IVirtualSequenceParser, IDisposable
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
        /// Total number of lines
        /// </summary>
        private int _lineCount;
        /// <summary>
        /// Total number of sequences
        /// </summary>
        private int _sequenceCount;
        /// <summary>
        /// Single line length
        /// </summary>
        private long _lineLength;
        /// <summary>
        /// Begining line of sequence
        /// </summary>
        private long _sequenceBeginsAt = 1;
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
        /// <summary>
        /// common behavior of sequence parser
        /// </summary>
        private readonly CommonSequenceParser _commonSequenceParser;
        // /// <summary>
        // /// common behavior of data virtualization on sequence parser
        // /// </summary>
        //private CommonVirtualSequenceParser _commonVirtualSequenceParser = null;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Default constructor chooses default encoding based on alphabet.
        /// </summary>
        public FastaParser()
        {
            _commonSequenceParser = new CommonSequenceParser();
        }

        /// <summary>
        /// Constructor for setting the encoding.
        /// </summary>
        /// <param name="encoding">The encoding to use for parsed ISequence objects.</param>
        public FastaParser(IEncoding encoding)
        {
            _commonSequenceParser = new CommonSequenceParser();
            Encoding = encoding;
        }

        #endregion

        #region ISequenceParser Members

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
        /// Data virtualization is enabled or not
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
        /// Gets the type of Parser i.e FASTA.
        /// This is intended to give developers some information 
        /// of the parser class.
        /// </summary>
        public string Name
        {
            get
            {
                return Resource.FASTA_NAME;
            }
        }

        /// <summary>
        /// Gets the description of Fasta parser.
        /// This is intended to give developers some information 
        /// of the parser class. This property returns a simple description of what the
        /// FastaParser class acheives.
        /// </summary>
        public string Description
        {
            get
            {
                return Resource.FASTAPARSER_DESCRIPTION;
            }
        }

        /// <summary>
        /// Gets a comma seperated values of the possible
        /// file extensions for a FASTA file.
        /// </summary>
        public string FileTypes
        {
            get
            {
                return Resource.FASTA_FILEEXTENSION;
            }
        }

        /// <summary>
        /// Parses a list of biological sequence texts from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence text.</param>
        /// <returns>The list of parsed ISequence objects.</returns>
        public IList<ISequence> Parse(TextReader reader)
        {
            return Parse(reader, true);
        }

        /// <summary>
        /// Parses a list of biological sequence texts from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequence objects.</returns>
        public IList<ISequence> Parse(TextReader reader, bool isReadOnly)
        {
            using (BioTextReader bioReader = new BioTextReader(reader))
            {
                return Parse(bioReader, isReadOnly);
            }
        }

        /// <summary>
        /// Parses a list of biological sequence texts from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <returns>The list of parsed ISequence objects.</returns>
        public IList<ISequence> Parse(string filename)
        {
            return Parse(filename, true);
        }

        /// <summary>
        /// Parses a list of biological sequence texts from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequence objects.</returns>
        public IList<ISequence> Parse(string filename, bool isReadOnly)
        {
            _fileName = filename;

            // check DV is requried
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
            
            // check for sidecar file 
            SidecarFileProvider indexedProvider = null;

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
                return new VirtualSequenceList(indexedProvider, this, indexedProvider.Count) { CreateSequenceAsReadOnly = isReadOnly };
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
        /// Parses a single biological sequence text from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence text.</param>
        /// <returns>The parsed ISequence object.</returns>
        public ISequence ParseOne(TextReader reader)
        {
            return ParseOne(reader, true);
        }

        /// <summary>
        /// Parses a single biological sequence text from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed ISequence object.</returns>
        public ISequence ParseOne(TextReader reader, bool isReadOnly)
        {
            _lineCount = 0;
            _sequenceCount = 0;
            _lineLength = 0;
            _sequenceBeginsAt = 1;
            using (BioTextReader bioReader = new BioTextReader(reader))
            {
                return ParseOne(bioReader, isReadOnly);
            }
        }

        /// <summary>
        /// Parses a single biological sequence text from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <returns>The parsed ISequence object.</returns>
        public ISequence ParseOne(string filename)
        {
            return ParseOne(filename, true);
        }

        /// <summary>
        /// Parses a single biological sequence text from a file.
        /// </summary>
        /// <param name="filename">The name of a biological sequence file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed ISequence object.</returns>
        public ISequence ParseOne(string filename, bool isReadOnly)
        {
            _lineCount = 0;
            _sequenceCount = 0;
            _lineLength = 0;
            _sequenceBeginsAt = 1;

            using (BioTextReader bioReader = new BioTextReader(filename))
            {
                return ParseOne(bioReader, isReadOnly);
            }
        }

        #endregion

        #region IVirtualSequenceParser Members

        /// <summary>
        /// Parses a range of sequence items starting from the specified index in the sequence.
        /// </summary>
        /// <param name="startIndex">The zero-based index at which to begin parsing.</param>
        /// <param name="count">The number of symbols to parse.</param>
        /// <param name="seqPointer">The sequence pointer of that sequence.</param>
        /// <returns>The parsed sequence.</returns>
        public ISequence ParseRange(int startIndex, int count, SequencePointer seqPointer)
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                throw new NotSupportedException(Resource.DataVirtualizationNeedsInputFile);
            }

            if (startIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            IAlphabet alphabet = Alphabets.All.Single(A => A.Name.Equals(seqPointer.AlphabetName));
            Sequence sequence = new Sequence(alphabet) { IsReadOnly = false };

            int start = (int)seqPointer.StartingIndex + startIndex;

            if (start >= seqPointer.EndingIndex)
                return null;

            int includesNewline = seqPointer.StartingLine * Environment.NewLine.Length;
            int len = (int)(seqPointer.EndingIndex - seqPointer.StartingIndex);

            using (BioTextReader bioReader = new BioTextReader(_fileName))
            {
                string str = bioReader.ReadBlock(startIndex, seqPointer.StartingIndex + includesNewline, count, len);
                sequence.InsertRange(0, str);
            }

            // default for partial load
            sequence.IsReadOnly = true;

            return sequence;
        }
        #endregion

        #region Private Members

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

                while (sourceReader.BaseStream.ReadByte() != '>')
                {
                    sourceReader.BaseStream.Seek(-2, SeekOrigin.Current);
                }
                
                pointer.Id = sourceReader.ReadLine();
                return pointer.Id;
            }
        }

        /// <summary>
        /// Parses a list of sequences using a BioTextReader.
        /// </summary>
        /// <remarks>
        /// This method should be overridden by any parsers that need to process file-scope
        /// metadata that applies to all of the sequences in the file.
        /// </remarks>
        /// <param name="bioReader">bio text reader</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequence objects.</returns>
        protected IList<ISequence> Parse(BioTextReader bioReader, bool isReadOnly)
        {
            _lineCount = 0;
            _sequenceCount = 0;
            _lineLength = 0;
            _sequenceBeginsAt = 1;

            if (bioReader == null)
            {
                throw new ArgumentNullException("bioReader");
            }

            // no empty files allowed
            if (!bioReader.HasLines)
            {
                string message = Resource.Parser_NoTextErrorMessage;
                Trace.Report(message);
                throw new InvalidOperationException(message);
            }

            // Check if DV is enabled and sidecar creation is possible
            if (!string.IsNullOrEmpty(bioReader.FileName) && IsDataVirtualizationEnabled && SidecarFileProvider.IsIndexFileExists(bioReader.FileName))
            {
                while (bioReader.HasLines)
                {
                    // Parse and forget as the list is now maintained by DV using sequence pointers
                    ParseOne(bioReader, isReadOnly);
                }

                // Create sidecar          
                SidecarFileProvider provider = SidecarFileProvider.CreateIndexFile(bioReader.FileName, _sequencePointers);

                VirtualSequenceList virtualSequences = 
                    new VirtualSequenceList(provider, this, _sequencePointers.Count) { CreateSequenceAsReadOnly = isReadOnly };

                _sequencePointers.Clear();

                return virtualSequences;
            }
            else
            {
                List<ISequence> sequences = new List<ISequence>();

                while (bioReader.HasLines)
                {
                    sequences.Add(ParseOne(bioReader, isReadOnly));
                }

                return sequences;
            }
        }

        /// <summary>
        /// Parses a single FASTA text from a reader into a sequence.
        /// </summary>
        /// <param name="bioReader">bio text reader</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>A new Sequence instance containing parsed data.</returns>
        protected ISequence ParseOneWithSpecificFormat(BioTextReader bioReader, bool isReadOnly)
        {
            SequencePointer sequencePointer = null;

            if (bioReader == null)
            {
                throw new ArgumentNullException("bioReader");
            }

            string message;
            if (!bioReader.Line.StartsWith(">", StringComparison.OrdinalIgnoreCase))
            {
                message = string.Format(CultureInfo.InvariantCulture,
                        Resource.INVAILD_INPUT_FILE,
                        Resource.FASTA_NAME);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Process header line.
            Sequence sequence;
            string id = bioReader.GetLineField(2).Trim();

            if (_blockSize > FileLoadHelper.DefaultFullLoadBlockSize)
            {
                _lineCount++;
                _lineLength += bioReader.Line.Length;
                sequencePointer = new SequencePointer { StartingLine = _lineCount };
            }

            bioReader.GoToNextLine();

            IAlphabet alphabet = Alphabet;
            if (alphabet == null)
            {
                alphabet = _commonSequenceParser.IdentifyAlphabet(alphabet, bioReader.Line);

                if (alphabet == null)
                {
                    message = string.Format(CultureInfo.InvariantCulture,
                            Resource.InvalidSymbolInString,
                            bioReader.Line);
                    Trace.Report(message);
                    throw new FileFormatException(message);
                }
            }

            if (Encoding == null)
            {
                sequence = new Sequence(alphabet);
            }
            else
            {
                sequence = new Sequence(alphabet, Encoding, string.Empty) { IsReadOnly = false };
            }

            bool sameSequence = false;
            sequence.ID = id;
            while (bioReader.HasLines && !bioReader.Line.StartsWith(">", StringComparison.OrdinalIgnoreCase))
            {
                if (Alphabet == null)
                {
                    alphabet = _commonSequenceParser.IdentifyAlphabet(sequence.Alphabet, bioReader.Line);

                    if (alphabet == null)
                    {
                        message = string.Format(CultureInfo.InvariantCulture,
                                Resource.InvalidSymbolInString,
                                bioReader.Line);
                        Trace.Report(message);
                        throw new FileFormatException(message);
                    }

                    if (sequence.Alphabet != alphabet)
                    {
                        Sequence seq = new Sequence(alphabet, Encoding, sequence) { IsReadOnly = false };
                        sequence.Clear();
                        sequence = seq;
                    }
                }


                // full load
                if (_blockSize <= 0)
                {
                    sequence.InsertRange(sequence.Count, bioReader.Line);
                }
                else
                {
                    if (sameSequence == false)
                    {
                        _sequenceBeginsAt = _lineLength;
                        sameSequence = true;
                    }

                    _lineLength += bioReader.Line.Length;
                    _lineCount++;
                }

                bioReader.GoToNextLine();

            }

            if (sequence.MoleculeType == MoleculeType.Invalid)
            {
                sequence.MoleculeType = CommonSequenceParser.GetMoleculeType(sequence.Alphabet);
            }
            sequence.IsReadOnly = isReadOnly;

            // full load
            if (_blockSize == FileLoadHelper.DefaultFullLoadBlockSize)
            {
                return sequence;
            }

            if (sequencePointer != null)
            {
                sequencePointer.AlphabetName = sequence.Alphabet.Name;
                sequencePointer.Id = sequence.ID;

                sequencePointer.StartingIndex = _sequenceBeginsAt;
                sequencePointer.EndingIndex = _lineLength;
                _sequencePointers.Add(sequencePointer);
            }
            _sequenceCount++;
            FileVirtualSequenceProvider dataprovider = new FileVirtualSequenceProvider(this, sequencePointer)
            {
                BlockSize = _blockSize,
                MaxNumberOfBlocks = _maxNumberOfBlocks
            };
            sequence.VirtualSequenceProvider = dataprovider;
            return sequence;
        }

        /// <summary>
        /// Parses a single sequences using a BioTextReader
        /// </summary>
        /// <param name="bioReader">bio text reader</param>
        /// <param name="isReadOnly">sequence property</param>
        /// <returns>a new Sequence</returns>
        private ISequence ParseOne(BioTextReader bioReader, bool isReadOnly)
        {
            _fileName = bioReader.FileName;

            // no empty files allowed
            if (!bioReader.HasLines)
            {
                string message = Resource.Parser_NoTextErrorMessage;
                Trace.Report(message);
                throw new InvalidOperationException(message);
            }

            // do the actual parsing
            ISequence sequence = ParseOneWithSpecificFormat(bioReader, isReadOnly);

            return sequence;
        }
        #endregion Private Members

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
        /// Dispose the file reader instances
        /// </summary>
        /// <param name="disposing">If disposing equals true, dispose all resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_sequencePointers != null)
                {
                    _sequencePointers = null;
                }
            }
        }

        #endregion
    }
}
