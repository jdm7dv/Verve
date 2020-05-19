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
using MBF.Encoding;
using MBF.Properties;
using MBF.Util.Logging;

namespace MBF.IO.Fasta
{
    /// <summary>
    /// A FastaParser reads from a source of text that is formatted according to the FASTA flat
    /// file specification and converts the data to in-memory ISequence objects.  For advanced
    /// users, the ability to select an encoding for the internal memory representation is
    /// provided. There is also a default encoding for each alphabet that may be encountered.
    /// Documentation for the latest GenBank file format can be found at
    /// http://www.ncbi.nlm.nih.gov/blast/fasta.shtml
    /// 
    /// FastaParser supports data virtualization by implementing the IVirtualSequenceParser interface.
    /// </summary>
    public class FastaParser : IVirtualSequenceParser, IDisposable
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
        /// File size in KBs to enable data virtualization
        /// </summary>
        private int _enforceDataVirtualizationByFileSize;
        /// <summary>
        /// Behavior common to all sequence parsers.
        /// </summary>
        private readonly CommonSequenceParser _commonSequenceParser;
        /// <summary>
        /// A stream reader to use data virtualization on biological sequence files.
        /// </summary>
        private MBFStreamReader _mbfStreamReader;
        /// <summary>
        /// Holds a pointer to the sidecar file for the parsed sequence
        /// when data virtualization is enabled.
        /// </summary>
        private SidecarFileProvider _sidecarFileProvider;
        #endregion Fields

        #region Constructors

        /// <summary>
        /// The default constructor which chooses the default encoding based on the alphabet.
        /// </summary>
        public FastaParser()
        {
            _commonSequenceParser = new CommonSequenceParser();
        }

        /// <summary>
        /// A constructor to set the encoding used.
        /// </summary>
        /// <param name="encoding">The encoding to use for parsed ISequence objects.</param>
        public FastaParser(IEncoding encoding)
        {
            _commonSequenceParser = new CommonSequenceParser();
            Encoding = encoding;
        }

        #endregion

        #region Properties

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
        /// File size to enable data virtualization. If the file size is
        /// larger, then data virtualization is loaded automatically.
        /// </summary>
        public int EnforceDataVirtualizationByFileSize {get; set;}

        /// <summary>
        /// The alphabet to use for parsed ISequence objects.  If this is not set, the alphabet will
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
        /// about parser class.
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
        #endregion

        #region ISequenceParser Members
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
            using (MBFTextReader mbfReader = new MBFTextReader(reader))
            {
                return Parse(mbfReader, isReadOnly);
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

            // Check for sidecar
            if (IsDataVirtualizationEnabled)
            {
                _sidecarFileProvider = new SidecarFileProvider(_fileName);
                _sidecarFileProvider.Close();

                // if valid sidecar file exists
                if (_sidecarFileProvider.IsSidecarValid)
                {
                    // Create virtual list and return
                    return new VirtualSequenceList(_sidecarFileProvider, this, _sidecarFileProvider.Count) { CreateSequenceAsReadOnly = isReadOnly };
                }

                // else create new sidecar
                _sidecarFileProvider = new SidecarFileProvider(_fileName, true);

                if (_sidecarFileProvider.SidecarFileExists)
                {
                    using (_mbfStreamReader = new MBFStreamReader(_fileName))
                    {
                        try
                        {
                            while (_mbfStreamReader.HasLines)
                            {
                                // Parse and forget as the list is now maintained by DV using sequence pointers
                                ParseOne(_mbfStreamReader, isReadOnly);
                            }

                            _sidecarFileProvider.Close();

                            VirtualSequenceList virtualSequences =
                                new VirtualSequenceList(_sidecarFileProvider, this, _sidecarFileProvider.Count) { CreateSequenceAsReadOnly = isReadOnly };

                            return virtualSequences;
                        }
                        catch (Exception)
                        {
                            _sidecarFileProvider.Cleanup();
                        }
                    }
                }
            }

            // non-DV parsing
            using (MBFTextReader mbfReader = new MBFTextReader(filename))
            {
                return Parse(mbfReader, isReadOnly);
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
            using (MBFTextReader mbfReader = new MBFTextReader(reader))
            {
                return ParseOne(mbfReader, isReadOnly);
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
        /// Parses a range of sequence items starting from the specified index in the sequence.
        /// </summary>
        /// <param name="startIndex">The zero-based index at which to begin parsing.</param>
        /// <param name="count">The number of symbols to parse.</param>
        /// <param name="seqPointer">The sequence pointer of the specified sequence.</param>
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

            if (_mbfStreamReader == null || !_mbfStreamReader.CanRead)
            {
                _mbfStreamReader = new MBFStreamReader(_fileName);
            }

            long fileIndex = startIndex + seqPointer.IndexOffsets[0];
            char[] buffer = _mbfStreamReader.ReadChars(fileIndex, count);
            sequence.InsertRange(0, new string(buffer));

            // default for partial load
            sequence.IsReadOnly = true;

            return sequence;
        }
        #endregion

        #region Private Members

        /// <summary>
        /// Gets the sequence ID corresponding to the specified sequence pointer.
        /// </summary>
        /// <param name="pointer">
        /// A sequence pointer representing the sequence whose ID is to be retrieved.
        /// </param>
        /// <returns>The sequence ID of the specified sequence.</returns>
        public string GetSequenceID(SequencePointer pointer)
        {
            if (pointer == null)
            {
                throw new ArgumentNullException("pointer");
            }

            if (_mbfStreamReader == null || !_mbfStreamReader.CanRead)
            {
                _mbfStreamReader = new MBFStreamReader(_fileName);
            }

            _mbfStreamReader.Seek(pointer.IndexOffsets[0] - pointer.StartingLine, SeekOrigin.Begin);
            _mbfStreamReader.ReadLine();

             // Read Sequence ID by looking back from the sequence starting index
            pointer.Id = _mbfStreamReader.GetLineField(2);
                return pointer.Id;
        }

        /// <summary>
        /// Parses a list of sequences using a MBFTextReader.
        /// </summary>
        /// <remarks>
        /// This method should be overridden by any parsers that need to process the file-scope
        /// metadata that applies to all of the sequences in the file.
        /// </remarks>
        /// <param name="mbfReader">MBF text reader</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequence objects.</returns>
        protected IList<ISequence> Parse(MBFTextReader mbfReader, bool isReadOnly)
        {
            if (mbfReader == null)
            {
                throw new ArgumentNullException("mbfReader");
            }

            // no empty files allowed
            if (!mbfReader.HasLines)
            {
                string message = Resource.Parser_NoTextErrorMessage;
                Trace.Report(message);
                throw new InvalidOperationException(message);
            }

            List<ISequence> sequences = new List<ISequence>();

            while (mbfReader.HasLines)
            {
                sequences.Add(ParseOne(mbfReader, isReadOnly));
            }

            return sequences;
        }

        /// <summary>
        /// Parses a single FASTA sequence from a file using MBFTextReader.
        /// This method is used in non-data virtualization scenarios.
        /// </summary>
        /// <param name="mbfReader">The MBFTextReader of the file to be parsed.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in read-only mode.
        /// If this flag is set to true then the resulting sequence's IsReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed sequence.</returns>
        protected ISequence ParseOneWithSpecificFormat(MBFTextReader mbfReader, bool isReadOnly)
        {
            if (mbfReader == null)
            {
                throw new ArgumentNullException("mbfReader");
            }

            string message;
            if (!mbfReader.Line.StartsWith(">", StringComparison.OrdinalIgnoreCase))
            {
                message = string.Format(CultureInfo.InvariantCulture,
                        Resource.INVALID_INPUT_FILE,
                        Resource.FASTA_NAME);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Process header line.
            Sequence sequence;
            string id = mbfReader.GetLineField(2).Trim();

            mbfReader.GoToNextLine();

            IAlphabet alphabet = Alphabet;
            if (alphabet == null)
            {
                alphabet = _commonSequenceParser.IdentifyAlphabet(alphabet, mbfReader.Line);

                if (alphabet == null)
                {
                    message = string.Format(CultureInfo.InvariantCulture,
                            Resource.InvalidSymbolInString,
                            mbfReader.Line);
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

            sequence.ID = id;
            while (mbfReader.HasLines && !mbfReader.Line.StartsWith(">", StringComparison.OrdinalIgnoreCase))
            {
                if (Alphabet == null)
                {
                    alphabet = _commonSequenceParser.IdentifyAlphabet(sequence.Alphabet, mbfReader.Line);

                    if (alphabet == null)
                    {
                        message = string.Format(CultureInfo.InvariantCulture,
                                Resource.InvalidSymbolInString,
                                mbfReader.Line);
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

                sequence.InsertRange(sequence.Count, mbfReader.Line);
                mbfReader.GoToNextLine();
            }

            if (sequence.MoleculeType == MoleculeType.Invalid)
            {
                sequence.MoleculeType = CommonSequenceParser.GetMoleculeType(sequence.Alphabet);
            }

            sequence.IsReadOnly = isReadOnly;
            return sequence;
        }

        /// <summary>
        /// Parses a single FASTA sequence from a file using MBFStreamReader.
        /// This method is only used in data virtualization scenarios.
        /// </summary>
        /// <param name="mbfReader">The MBFStreamReader of the file to be parsed.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in read-only mode.
        /// If this flag is set to true then the resulting sequence's IsReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed sequence.</returns>
        protected ISequence ParseOneWithSpecificFormat(MBFStreamReader mbfReader, bool isReadOnly)
        {
            SequencePointer sequencePointer = new SequencePointer();

            if (mbfReader == null)
            {
                throw new ArgumentNullException("mbfReader");
            }

            string message;
            if (!mbfReader.Line.StartsWith(">", StringComparison.OrdinalIgnoreCase))
            {
                message = string.Format(CultureInfo.InvariantCulture,
                        Resource.INVALID_INPUT_FILE,
                        Resource.FASTA_NAME);
                Trace.Report(message);
                throw new FileFormatException(message);
            }

            // Process header line.
            Sequence sequence;
            string id = mbfReader.GetLineField(2).Trim();

            // save initial start and end indices
            sequencePointer.StartingLine = (int)(mbfReader.Position - mbfReader.CurrentLineStartingIndex);
            sequencePointer.IndexOffsets[0] = mbfReader.Position;
            sequencePointer.IndexOffsets[1] = mbfReader.Position;

            mbfReader.GoToNextLine();

            IAlphabet alphabet = Alphabet;
            if (alphabet == null)
            {
                alphabet = _commonSequenceParser.IdentifyAlphabet(alphabet, mbfReader.Line);

                if (alphabet == null)
                {
                    message = string.Format(CultureInfo.InvariantCulture,
                            Resource.InvalidSymbolInString,
                            mbfReader.Line);
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

            int currentBlockSize = 0;
            int symbolCount = -1;
            int newLineCharacterCount = mbfReader.NewLineCharacterCount;
            int prenewLineCharacterCount = 0;
            int lineLength = mbfReader.Line.Length;

            sequence.ID = id;

            while (mbfReader.HasLines && !mbfReader.Line.StartsWith(">", StringComparison.OrdinalIgnoreCase))
            {
                sequencePointer.IndexOffsets[1] += mbfReader.Line.Length;
                if (Alphabet == null)
                {
                    alphabet = _commonSequenceParser.IdentifyAlphabet(sequence.Alphabet, mbfReader.Line);

                    if (alphabet == null)
                    {
                        message = string.Format(CultureInfo.InvariantCulture,
                                Resource.InvalidSymbolInString,
                                mbfReader.Line);
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

                newLineCharacterCount = mbfReader.NewLineCharacterCount;
                lineLength = mbfReader.Line.Length;

                while (lineLength != 0 && _sidecarFileProvider != null)
                {
                    if (lineLength + currentBlockSize + newLineCharacterCount <= _blockSize)
                    {
                        symbolCount += lineLength;
                        currentBlockSize += lineLength + newLineCharacterCount;
                        lineLength = 0;
                    }
                    else
                    {
                        symbolCount += _blockSize - currentBlockSize;
                        lineLength = lineLength - (_blockSize - currentBlockSize);
                        if (lineLength <= 0)
                        {
                            symbolCount += lineLength;
                            prenewLineCharacterCount = newLineCharacterCount + lineLength;
                            lineLength = 0;
                        }

                        currentBlockSize = _blockSize;
                    }

                    if (currentBlockSize == _blockSize)
                    {
                        // write to file.
                        _sidecarFileProvider.WriteBlockIndex(symbolCount);
                        currentBlockSize = prenewLineCharacterCount;
                        prenewLineCharacterCount = 0;
                    }
                }

                mbfReader.GoToNextLine();
            }

            if (_sidecarFileProvider != null)
            {
                if (sequencePointer.IndexOffsets[1] - sequencePointer.IndexOffsets[0] > _blockSize
                    && currentBlockSize - newLineCharacterCount > 0)
                {
                    _sidecarFileProvider.WriteBlockIndex(symbolCount);
                }
                else
                {
                    _sidecarFileProvider.WriteBlockIndex(0);
                }
            }

            if (sequence.MoleculeType == MoleculeType.Invalid)
            {
                sequence.MoleculeType = CommonSequenceParser.GetMoleculeType(sequence.Alphabet);
            }

            sequence.IsReadOnly = isReadOnly;

            sequencePointer.AlphabetName = sequence.Alphabet.Name;
            sequencePointer.Id = sequence.ID;

            if (_sidecarFileProvider != null)
            {
                // Write each sequence pointer to the sidecar file immediately
                _sidecarFileProvider.WritePointer(sequencePointer);
            }

            FileVirtualSequenceProvider dataprovider = new FileVirtualSequenceProvider(this, sequencePointer)
            {
                BlockSize = _blockSize,
                MaxNumberOfBlocks = _maxNumberOfBlocks
            };

            sequence.VirtualSequenceProvider = dataprovider;
            return sequence;
        }

        /// <summary>
        /// Parses a single sequence using a MBFTextReader.
        /// </summary>
        /// <param name="mbfReader">The MBFTextReader of the file to be parsed.</param>
        /// <param name="isReadOnly">Indicates whether the parsed sequence is read-only.</param>
        /// <returns>The parsed sequence.</returns>
        private ISequence ParseOne(MBFTextReader mbfReader, bool isReadOnly)
        {
            _fileName = mbfReader.FileName;

            // no empty files allowed
            if (!mbfReader.HasLines)
            {
                string message = Resource.Parser_NoTextErrorMessage;
                Trace.Report(message);
                throw new InvalidOperationException(message);
            }

            // do the actual parsing
            ISequence sequence = ParseOneWithSpecificFormat(mbfReader, isReadOnly);

            return sequence;
        }

        /// <summary>
        /// Parses a single sequence using a MBFStreamReader.
        /// This method is only used in data virtualization scenarios.
        /// </summary>
        /// <param name="mbfReader">The MBFStreamReader of the file to be parsed.</param>
        /// <param name="isReadOnly">Indicates whether the parsed sequence is read-only.</param>
        /// <returns>The parsed sequence.</returns>
        private ISequence ParseOne(MBFStreamReader mbfReader, bool isReadOnly)
        {
            _fileName = mbfReader.FileName;

            // no empty files allowed
            if (!mbfReader.HasLines)
            {
                string message = Resource.Parser_NoTextErrorMessage;
                Trace.Report(message);
                throw new InvalidOperationException(message);
            }

            // do the actual parsing
            ISequence sequence = ParseOneWithSpecificFormat(mbfReader, isReadOnly);

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
                    _sidecarFileProvider.Dispose();
                }
            }
        }

        #endregion
    }
}
