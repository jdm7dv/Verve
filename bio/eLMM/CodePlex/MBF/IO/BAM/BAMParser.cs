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
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Bio.Algorithms.Alignment;
using Bio.Encoding;
using Bio.IO.SAM;
using Bio.Properties;
using Bio.Util;
using System.Globalization;

namespace Bio.IO.BAM
{
    /// <summary>
    /// A BAMParser reads from a source of binary data that is formatted according to the BAM
    /// file specification, and converts the data to in-memory SequenceAlignmentMap object.
    /// Documentation for the latest BAM file format can be found at
    /// http://samtools.sourceforge.net/SAM1.pdf
    /// </summary>
    public class BAMParser : ISequenceAlignmentParser, IDisposable
    {
        #region Private Fields
        /// <summary>
        /// Holds bam files stream.
        /// </summary>
        private Stream _readStream;

        /// <summary>
        /// Flag to indicate whether the BAM file is compressed or not.
        /// </summary>
        private bool _isCompressed;

        /// <summary>
        /// holds reference sequence names.
        /// </summary>
        private List<string> refSeqNames;

        /// <summary>
        /// Holds references sequence's length.
        /// </summary>
        private List<int> refSeqLengths;

        /// <summary>
        /// Temp Stream to hold uncompressed blocks. 
        /// </summary>
        private Stream _deCompressedStream;

        /// <summary>
        /// Holds current position of the compressed BAM file.
        /// Used while creating BAMIndex objects from BAM file and while parsing BAM file using BAM index file.
        /// </summary>
        private long currentCompressedBlockStartPos;

        /// <summary>
        /// Holds BAMIndex while creating the BAMIndex object from BAMfile.
        /// </summary>
        private BAMIndex _bamIndex;

        /// <summary>
        /// Flag to indicate to create BAMIndex while parsing BAM file or not.
        /// </summary>
        private bool _createBamIndex = false;
        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor chooses default encoding based on alphabet.
        /// </summary>
        public BAMParser()
        {
            RefSequences = new List<ISequence>();
            refSeqNames = new List<string>();
            refSeqLengths = new List<int>();
        }

        /// <summary>
        /// Constructor for setting the encoding.
        /// </summary>
        /// <param name="encoding">The encoding to use for parsed ISequence objects.</param>
        public BAMParser(IEncoding encoding)
            : this()
        {
            Encoding = encoding;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the name of the sequence alignment parser being
        /// implemented. This is intended to give the
        /// developer some information of the parser type.
        /// </summary>
        public string Name
        {
            get { return Resource.BAM_NAME; }
        }

        /// <summary>
        /// Gets the description of the sequence alignment parser being
        /// implemented. This is intended to give the
        /// developer some information of the parser.
        /// </summary>
        public string Description
        {
            get { return Resource.BAMPARSER_DESCRIPTION; }
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
                throw new NotSupportedException(Resource.BAMParserAlphabetCantBeSet);
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
            get { return Resource.BAM_FILEEXTENSION; }
        }

        /// <summary>
        /// Reference sequences, used to resolve "=" symbol in the sequence data.
        /// </summary>
        public IList<ISequence> RefSequences { get; private set; }
        #endregion

        #region Private Static Methods

        /// <summary>
        /// Returns a boolean value indicating whether a BAM file is compressed or uncompressed.
        /// </summary>
        /// <param name="array">Byte array containing first 4 bytes of a BAM file</param>
        /// <returns>Returns true if the specified byte array indicates that the BAM file is compressed else returns false.</returns>
        private static bool IsCompressedBAMFile(byte[] array)
        {
            bool result = false;
            if (array[0] == 31 && array[1] == 139 && array[2] == 8) //  && array[3] == 4
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Returns a boolean value indicating whether a BAM file is valid uncompressed BAM file or not.
        /// </summary>
        /// <param name="array">Byte array containing first 4 bytes of a BAM file</param>
        /// <returns>Returns true if the specified byte array indicates a valid uncompressed BAM file else returns false.</returns>
        private static bool IsUnCompressedBAMFile(byte[] array)
        {
            bool result = false;
            if (array[0] == 66 && array[1] == 65 && array[2] == 77 && array[3] == 1)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Gets optional value depending on the valuetype.
        /// </summary>
        /// <param name="valueType">Value Type.</param>
        /// <param name="array">Byte array to read from.</param>
        /// <param name="startIndex">Start index of value in the array.</param>
        private static object GetOptionalValue(char valueType, byte[] array, ref int startIndex)
        {
            object obj;
            int intValue;
            int len;
            switch (valueType)
            {
                case 'A':  //  Printable character
                    obj = (char)array[startIndex];
                    startIndex++;
                    break;
                case 'c': //signed 8-bit integer
                    intValue = (array[startIndex] & 0x7F);
                    if ((array[startIndex] & 0x80) == 0x80)
                    {

                        intValue = intValue + sbyte.MinValue;
                    }

                    obj = intValue;
                    startIndex++;
                    break;
                case 'C':
                    obj = (uint)array[startIndex];
                    startIndex++;
                    break;
                case 's':
                    obj = Helper.GetInt16(array, startIndex);
                    startIndex += 2;
                    break;
                case 'S':
                    obj = Helper.GetUInt16(array, startIndex);
                    startIndex += 2;
                    break;
                case 'i':
                    obj = Helper.GetInt32(array, startIndex);
                    startIndex += 4;
                    break;
                case 'I':
                    obj = Helper.GetUInt32(array, startIndex);
                    startIndex += 4;
                    break;
                case 'f':
                    obj = Helper.GetSingle(array, startIndex);
                    startIndex += 4;
                    break;
                case 'Z':
                    len = GetStringLength(array, startIndex);
                    obj = System.Text.ASCIIEncoding.ASCII.GetString(array, startIndex, len - 1);
                    startIndex += len;
                    break;
                case 'H':
                    len = GetStringLength(array, startIndex);
                    obj = Helper.GetHexString(array, startIndex, len - 1);
                    startIndex += len;
                    break;
                default:
                    throw new FileFormatException(Resource.BAM_InvalidOptValType);
            }

            return obj;
        }

        /// <summary>
        /// Gets the length of the string in byte array.
        /// </summary>
        /// <param name="array">Byte array which contains string.</param>
        /// <param name="startIndex">Start index of array from which string is stored.</param>
        private static int GetStringLength(byte[] array, int startIndex)
        {
            int i = startIndex;
            while (i < array.Length && array[i] != '\x0')
            {
                i++;
            }

            return i + 1 - startIndex;
        }

        /// <summary>
        /// Gets equivalent sequence char for the specified encoded value.
        /// </summary>
        /// <param name="encodedValue">Encoded value.</param>
        private static char GetSeqChar(int encodedValue)
        {
            switch (encodedValue)
            {
                case 0:
                    return '=';
                case 1:
                    return 'A';
                case 2:
                    return 'C';
                case 4:
                    return 'G';
                case 8:
                    return 'T';
                case 15:
                    return 'N';

                default:
                    throw new FileFormatException(Resource.BAM_InvalidEncodedSequenceValue);
            }
        }

        /// <summary>
        /// Decompresses specified compressed stream to out stream.
        /// </summary>
        /// <param name="compressedStream">Compressed stream to decompress.</param>
        /// <param name="outStream">Out stream.</param>
        private static void Decompress(Stream compressedStream, Stream outStream)
        {
            using (GZipStream Decompress = new GZipStream(compressedStream, CompressionMode.Decompress, true))
            {
                Decompress.CopyTo(outStream);
            }
        }

        // Gets list of possible bins for a given start and end reference sequence co-ordinates.
        private static IList<uint> Reg2Bins(uint start, uint end)
        {
            List<uint> bins = new List<uint>();
            uint k;
            --end;
            bins.Add(0);
            for (k = 1 + (start >> 26); k <= 1 + (end >> 26); ++k) bins.Add(k);
            for (k = 9 + (start >> 23); k <= 9 + (end >> 23); ++k) bins.Add(k);
            for (k = 73 + (start >> 20); k <= 73 + (end >> 20); ++k) bins.Add(k);
            for (k = 585 + (start >> 17); k <= 585 + (end >> 17); ++k) bins.Add(k);
            for (k = 4681 + (start >> 14); k <= 4681 + (end >> 14); ++k) bins.Add(k);
            return bins;
        }

        // Gets all chunks for the specified ref sequence index.
        private static IList<Chunk> GetChunks(BAMReferenceIndexes refIndex)
        {
            List<Chunk> chunks = new List<Chunk>();
            foreach (Bin bin in refIndex.Bins)
            {
                chunks.InsertRange(chunks.Count, bin.Chunks);
            }

            return chunks;
        }

        // Gets chunks for specified ref seq index, start and end co-ordinate this method considers linear index also.
        private static IList<Chunk> GetChunks(BAMReferenceIndexes refIndex, int start, int end)
        {
            List<Chunk> chunks = new List<Chunk>();
            IList<uint> binnumbers = Reg2Bins((uint)start, (uint)end);
            List<Bin> bins = refIndex.Bins.Where(B => binnumbers.Contains(B.BinNumber)).ToList();

            // consider linear indexing only for the bins less than 4681.
            foreach (Bin bin in bins.Where(B => B.BinNumber < 4681))
            {
                chunks.InsertRange(chunks.Count, bin.Chunks);
            }

            int index = start / (16 * 1024);  // Linear indexing window size is 16K

            if (refIndex.LinearOffsets.Count > index)
            {
                FileOffset offset = refIndex.LinearOffsets[index];
                chunks = chunks.Where(C => C.ChunkEnd.CompressedBlockOffset > offset.CompressedBlockOffset || (C.ChunkEnd.CompressedBlockOffset == offset.CompressedBlockOffset && C.ChunkEnd.UncompressedBlockOffset > offset.UncompressedBlockOffset)).ToList();
            }

            // add chunks for the bin numbers greater than 4681.
            foreach (Bin bin in bins.Where(B => B.BinNumber >= 4681))
            {
                chunks.InsertRange(chunks.Count, bin.Chunks);
            }

            return chunks;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a SequenceAlignmentMap object by parsing a BAM file.
        /// </summary>
        /// <param name="reader">Stream to read.</param>
        /// <returns>SequenceAlignmentMap object.</returns>
        public SequenceAlignmentMap Parse(Stream reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return Parse(reader, true);
        }

        /// <summary>
        /// Returns a SequenceAlignmentMap object by parsing a BAM file.
        /// </summary>
        /// <param name="reader">Stream to read.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the SequenceAlignmentMap should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>SequenceAlignmentMap object.</returns>
        public SequenceAlignmentMap Parse(Stream reader, bool isReadOnly)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            return GetAlignment(reader, isReadOnly);
        }

        /// <summary>
        /// Returns BAMIndex by parsing specified BAM stream.
        /// </summary>
        /// <param name="bamfilename">BAM file to read.</param>
        public BAMIndex GetIndexFromBAMFile(string bamfilename)
        {
            if (string.IsNullOrWhiteSpace(bamfilename))
            {
                throw new ArgumentNullException("bamfilename");
            }

            using (Stream bamStream = new FileStream(bamfilename, FileMode.Open, FileAccess.Read))
            {
                return GetIndexFromBAMFile(bamStream);
            }
        }

        /// <summary>
        /// Returns BAMIndex by parsing specified BAM stream.
        /// </summary>
        /// <param name="bamStream">Stream to read.</param>
        public BAMIndex GetIndexFromBAMFile(Stream bamStream)
        {
            if (bamStream == null)
            {
                throw new ArgumentNullException("bamStream");
            }

            try
            {
                _createBamIndex = true;
                GetAlignment(bamStream, true);
                ReduceChunks();
                return _bamIndex;
            }
            finally
            {
                _createBamIndex = false;
            }
        }

        /// <summary>
        /// Returns a SequenceAlignmentMap object by parsing a BAM file.
        /// </summary>
        /// <param name="fileName">BAM filename to parse.</param>
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
        /// Returns a SequenceAlignmentMap object by parsing a BAM file.
        /// </summary>
        /// <param name="fileName">BAM filename to parse.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the SequenceAlignmentMap should be in 
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

            using (_readStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Parse(_readStream, isReadOnly);
            }
        }

        /// <summary>
        /// Parses specified BAM file using index file.
        /// Index file is assumed to be in the same location as that of the specified bam file with the name "filename".bai
        /// For example, if the specified bam file name is D:\BAMdata\sample.bam then index file name will be taken as D:\BAMdata\sample.bam.bai
        /// If index file is not available then this method throw an exception.
        /// </summary>
        /// <param name="fileName">BAM filename to parse.</param>
        /// <param name="refSeqIndex">Index of reference sequence.</param>
        /// <returns>SequenceAlignmentMap object which contains alignments for specified reference sequence.</returns>
        public SequenceAlignmentMap ParseRange(string fileName, int refSeqIndex)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            return ParseRange(fileName, refSeqIndex, true);
        }

        /// <summary>
        /// Parses specified BAM file using index file.
        /// Index file is assumed to be in the same location as that of the specified bam file with the name "filename".bai
        /// For example, if the specified bam file name is D:\BAMdata\sample.bam then index file name will be taken as D:\BAMdata\sample.bam.bai
        /// If index file is not available then this method throw an exception.
        /// </summary>
        /// <param name="fileName">BAM file name.</param>
        /// <param name="refSeqIndex">Index of reference sequence.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the SequenceAlignmentMap should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>SequenceAlignmentMap object which contains alignments for specified reference sequence.</returns>
        public SequenceAlignmentMap ParseRange(string fileName, int refSeqIndex, bool isReadOnly)
        {
            using (FileStream bamStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (BAMIndexFile bamIndexFile = new BAMIndexFile(fileName + Resource.BAM_INDEXFILEEXTENSION, FileMode.Open, FileAccess.Read))
                {
                    return GetAlignment(bamStream, bamIndexFile, refSeqIndex, isReadOnly);
                }
            }
        }

        /// <summary>
        /// Parses specified BAM file using index file.
        /// Index file is assumed to be in the same location as that of the specified bam file with the name "filename".bai
        /// For example, if the specified bam file name is D:\BAMdata\sample.bam then index file name will be taken as D:\BAMdata\sample.bam.bai
        /// If index file is not available then this method throw an exception.
        /// </summary>
        /// <param name="fileName">BAM file name.</param>
        /// <param name="refSeqIndex">Index of reference sequence.</param>
        /// <param name="start">Start index.</param>
        /// <param name="end">End index.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the SequenceAlignmentMap should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>SequenceAlignmentMap object which contains alignments overlaps with the specified start 
        /// and end co-ordinate of the specified reference sequence.</returns>
        public SequenceAlignmentMap ParseRange(string fileName, int refSeqIndex, int start, int end, bool isReadOnly)
        {
            using (FileStream bamStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (BAMIndexFile bamIndexFile = new BAMIndexFile(fileName + Resource.BAM_INDEXFILEEXTENSION, FileMode.Open, FileAccess.Read))
                {
                    return GetAlignment(bamStream, bamIndexFile, refSeqIndex, start, end, isReadOnly);
                }
            }
        }

        /// <summary>
        /// Parses specified BAM file using index file.
        /// </summary>
        /// <param name="fileName">BAM file name.</param>
        /// <param name="range">SequenceRange object which contains reference sequence name and start and end co-ordinates.</param>
        /// <returns>SequenceAlignmentMap object which contains alignments for specified reference sequence and for specified range.</returns>
        public SequenceAlignmentMap ParseRange(string fileName, SequenceRange range)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if (range == null)
            {
                throw new ArgumentNullException("range");
            }

            return ParseRange(fileName, range, true);
        }

        /// <summary>
        /// Parses specified BAM file using index file.
        /// </summary>
        /// <param name="fileName">BAM file name.</param>
        /// <param name="range">SequenceRange object which contains reference sequence name and start and end co-ordinates.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the SequenceAlignmentMap should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>SequenceAlignmentMap object which contains alignments for specified reference sequence and for specified range.</returns>
        public SequenceAlignmentMap ParseRange(string fileName, SequenceRange range, bool isReadOnly)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException("fileName");
            }

            if (range == null)
            {
                throw new ArgumentNullException("range");
            }

            if (string.IsNullOrEmpty(range.ID))
            {
                throw new ArgumentException("Reference sequence name (range.ID) can't empty or null.");
            }

            int start = range.Start >= int.MaxValue ? int.MaxValue : (int)range.Start;
            int end = range.End >= int.MaxValue ? int.MaxValue : (int)range.End;

            if (start == 0 && end == int.MaxValue)
            {
                using (FileStream bamStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BAMIndexFile bamIndexFile = new BAMIndexFile(fileName + Resource.BAM_INDEXFILEEXTENSION, FileMode.Open, FileAccess.Read))
                    {
                        return GetAlignment(bamStream, bamIndexFile, range.ID, isReadOnly);
                    }
                }
            }
            else
            {
                using (FileStream bamStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BAMIndexFile bamIndexFile = new BAMIndexFile(fileName + Resource.BAM_INDEXFILEEXTENSION, FileMode.Open, FileAccess.Read))
                    {
                        return GetAlignment(bamStream, bamIndexFile, range.ID, start, end, isReadOnly);
                    }
                }
            }

        }

        #region ISequenceAlignmentParser Method
        /// <summary>
        /// Always throws NotSupportedException as BAM parser does not supports reading from a text reader.
        /// </summary>
        /// <param name="reader">Text reader.</param>
        IList<ISequenceAlignment> ISequenceAlignmentParser.Parse(TextReader reader)
        {
            throw new NotSupportedException(Resource.BAM_TextreaderNotSupportedMessage);
        }

        /// <summary>
        /// Always throws NotSupportedException as BAM parser does not supports reading from a text reader.
        /// </summary>
        /// <param name="reader">Text reader.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the sequence alignment should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        IList<ISequenceAlignment> ISequenceAlignmentParser.Parse(TextReader reader, bool isReadOnly)
        {
            throw new NotSupportedException(Resource.BAM_TextreaderNotSupportedMessage);
        }

        /// <summary>
        /// Parses a list of sequence alignment from a BAM file.
        /// </summary>
        /// <param name="fileName">The name of a BAM file to parse.</param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        IList<ISequenceAlignment> ISequenceAlignmentParser.Parse(string fileName)
        {
            ISequenceAlignment alignment = Parse(fileName, true);
            return new List<ISequenceAlignment>() { alignment };
        }

        /// <summary>
        /// Parses a list of sequence alignment from a BAM file.
        /// </summary>
        /// <param name="fileName">The name of a BAM file to parse.</param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
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
        /// Always throws NotSupportedException as BAM parser does not supports reading from a text reader.
        /// </summary>
        /// <param name="reader">Text reader.</param>
        public ISequenceAlignment ParseOne(TextReader reader)
        {
            throw new NotSupportedException(Resource.BAM_TextreaderNotSupportedMessage);
        }

        /// <summary>
        /// Always throws NotSupportedException as BAM parser does not supports reading from a text reader.
        /// </summary>
        /// <param name="reader">Text reader.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the sequence alignment should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        public ISequenceAlignment ParseOne(TextReader reader, bool isReadOnly)
        {
            throw new NotSupportedException(Resource.BAM_TextreaderNotSupportedMessage);
        }

        /// <summary>
        /// Parses a SequenceAlignmentMap from a BAM file.
        /// </summary>
        /// <param name="fileName">The name of a BAM file.</param>
        /// <returns>ISequenceAlignment object.</returns>
        public ISequenceAlignment ParseOne(string fileName)
        {
            return Parse(fileName, true);
        }

        /// <summary>
        /// Parses a SequenceAlignmentMap from a BAM file.
        /// </summary>
        /// <param name="fileName">The name of a BAM file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences in the sequence alignment should be in 
        /// readonly mode or not. If this flag is set to true then the resulting sequences's 
        /// isReadOnly property will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>ISequenceAlignment object.</returns>
        public ISequenceAlignment ParseOne(string fileName, bool isReadOnly)
        {
            return Parse(fileName, isReadOnly);
        }
        #endregion

        /// <summary>
        /// Disposes resources if any.
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
                if (_readStream != null)
                {
                    _readStream = null;
                }

                if (_deCompressedStream != null)
                {
                    _deCompressedStream.Dispose();
                    _deCompressedStream = null;
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Validates the BAM stream.
        /// </summary>
        private void ValidateReader()
        {
            _isCompressed = true;
            byte[] array = new byte[4];

            if (_readStream.Read(array, 0, 4) != 4)
            {
                // cannot read file properly.
                throw new FileFormatException(Resource.BAM_InvalidBAMFile);
            }

            _isCompressed = IsCompressedBAMFile(array);

            if (!_isCompressed)
            {
                if (!IsUnCompressedBAMFile(array))
                {
                    // Neither compressed BAM file nor uncompressed BAM file header.
                    throw new FileFormatException(Resource.BAM_InvalidBAMFile);
                }
            }

            _readStream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Parses the BAM file and returns the Header.
        /// </summary>
        private SAMAlignmentHeader GetHeader()
        {
            SAMAlignmentHeader header = new SAMAlignmentHeader();
            refSeqNames = new List<string>();
            refSeqLengths = new List<int>();

            _readStream.Seek(0, SeekOrigin.Begin);
            byte[] array = new byte[8];
            ReadUnCompressedData(array, 0, 8);
            int l_text = Helper.GetInt32(array, 4);
            byte[] samHeaderData = new byte[l_text];
            if (l_text != 0)
            {
                ReadUnCompressedData(samHeaderData, 0, l_text);
            }

            ReadUnCompressedData(array, 0, 4);
            int noofRefSeqs = Helper.GetInt32(array, 0);

            for (int i = 0; i < noofRefSeqs; i++)
            {
                ReadUnCompressedData(array, 0, 4);
                int len = Helper.GetInt32(array, 0);
                byte[] refName = new byte[len];
                ReadUnCompressedData(refName, 0, len);
                ReadUnCompressedData(array, 0, 4);
                int refLen = Helper.GetInt32(array, 0);
                refSeqNames.Add(System.Text.ASCIIEncoding.ASCII.GetString(refName, 0, refName.Length - 1));
                refSeqLengths.Add(refLen);
            }

            if (samHeaderData.Length != 0)
            {
                string str = System.Text.ASCIIEncoding.ASCII.GetString(samHeaderData);
                using (StringReader reader = new StringReader(str))
                {
                    header = SAMParser.ParserSAMHeader(reader);
                }
            }

            return header;
        }
        /// <summary>
        /// Merges small chunks belongs to a bin which are in the same compressed block.
        /// This will reduce number of seek calls required.
        /// </summary>
        private void ReduceChunks()
        {
            if (_bamIndex == null)
                return;

            for (int i = 0; i < _bamIndex.RefIndexes.Count; i++)
            {
                BAMReferenceIndexes bamRefIndex = _bamIndex.RefIndexes[i];
                for (int j = 0; j < bamRefIndex.Bins.Count; j++)
                {
                    Bin bin = bamRefIndex.Bins[j];
                    int lastIndex = 0;
                    int noofchunksToRemove = 0;

                    for (int k = 1; k < bin.Chunks.Count; k++)
                    {
                        // check for the chunks which are in the same compressed blocks.
                        if (bin.Chunks[lastIndex].ChunkEnd.CompressedBlockOffset == bin.Chunks[k].ChunkStart.CompressedBlockOffset)
                        {
                            bin.Chunks[lastIndex].ChunkEnd.CompressedBlockOffset = bin.Chunks[k].ChunkEnd.CompressedBlockOffset;
                            bin.Chunks[lastIndex].ChunkEnd.UncompressedBlockOffset = bin.Chunks[k].ChunkEnd.UncompressedBlockOffset;
                            noofchunksToRemove++;
                        }
                        else
                        {
                            bin.Chunks[++lastIndex] = bin.Chunks[k];
                        }
                    }

                    if (noofchunksToRemove > 0)
                    {
                        for (int index = 0; index < noofchunksToRemove; index++)
                        {
                            bin.Chunks.RemoveAt(bin.Chunks.Count - 1);
                        }
                    }
                }
            }

        }

        // Returns SequenceAlignmentMap object by parsing specified BAM stream.
        private SequenceAlignmentMap GetAlignment(Stream reader, bool isReadOnly)
        {
            if (reader == null || reader.Length == 0)
            {
                throw new FileFormatException(Resource.BAM_InvalidBAMFile);
            }

            _readStream = reader;
            ValidateReader();
            SAMAlignmentHeader header = GetHeader();
            SequenceAlignmentMap seqMap = new SequenceAlignmentMap(header);


            int lastBin = int.MaxValue;
            Chunk lastChunk = null;
            Bin bin;
            Chunk chunk;
            int lastRefSeqIndex = 0;
            int curRefSeqIndex;
            ulong lastcOffset = 0;
            ushort lastuOffset = 0;
            BAMReferenceIndexes refIndices = null;

            if (_createBamIndex)
            {
                _bamIndex = new BAMIndex();

                for (int i = 0; i < refSeqNames.Count; i++)
                {
                    _bamIndex.RefIndexes.Add(new BAMReferenceIndexes());
                }

                refIndices = _bamIndex.RefIndexes[0];
            }

            while (!IsEOF())
            {
                if (_createBamIndex)
                {
                    lastcOffset = (ulong)currentCompressedBlockStartPos;
                    lastuOffset = (ushort)_deCompressedStream.Position;
                }

                SAMAlignedSequence alignedSeq = GetAlignedSequence(0, int.MaxValue, isReadOnly);

                if (_createBamIndex)
                {
                    curRefSeqIndex = refSeqNames.IndexOf(alignedSeq.RName);

                    if (lastRefSeqIndex != curRefSeqIndex)
                    {
                        refIndices = _bamIndex.RefIndexes[curRefSeqIndex];
                        lastBin = int.MaxValue;
                        lastRefSeqIndex = curRefSeqIndex;
                    }

                    if (lastBin != alignedSeq.Bin)
                    {
                        bin = refIndices.Bins.FirstOrDefault(B => B.BinNumber == alignedSeq.Bin);
                        if (bin == null)
                        {
                            bin = new Bin();
                            bin.BinNumber = (uint)alignedSeq.Bin;
                            refIndices.Bins.Add(bin);
                        }

                        if (lastChunk != null)
                        {
                            lastChunk.ChunkEnd.CompressedBlockOffset = lastcOffset;
                            lastChunk.ChunkEnd.UncompressedBlockOffset = lastuOffset;
                        }

                        chunk = new Chunk();
                        chunk.ChunkStart = new FileOffset();
                        chunk.ChunkEnd = new FileOffset();
                        chunk.ChunkStart.CompressedBlockOffset = lastcOffset;
                        chunk.ChunkStart.UncompressedBlockOffset = lastuOffset;
                        bin.Chunks.Add(chunk);

                        lastChunk = chunk;
                        lastBin = alignedSeq.Bin;
                    }

                    // store linear index other than 16k bins, that is bin number less than 4681.
                    if (alignedSeq.Bin < 4681)
                    {
                        int pos = alignedSeq.Pos > 0 ? alignedSeq.Pos - 1 : 0;
                        int end = pos + alignedSeq.QueryLength - 1;
                        pos = pos >> 14;
                        end = end >> 14;
                        if (refIndices.LinearOffsets.Count == 0)
                        {
                            refIndices.LinearOffsets.Add(new FileOffset());
                        }

                        if (refIndices.LinearOffsets.Count <= end)
                        {
                            for (int i = refIndices.LinearOffsets.Count; i <= end; i++)
                            {
                                refIndices.LinearOffsets.Add(new FileOffset());
                            }
                        }

                        for (int i = pos + 1; i <= end; i++)
                        {
                            FileOffset offset = refIndices.LinearOffsets[i];
                            if (offset.CompressedBlockOffset == 0 && offset.UncompressedBlockOffset == 0)
                            {
                                offset.CompressedBlockOffset = lastcOffset;
                                offset.UncompressedBlockOffset = lastuOffset;
                            }
                        }
                    }
                }

                if (alignedSeq != null)
                {
                    seqMap.QuerySequences.Add(alignedSeq);
                }
            }

            if (_createBamIndex)
            {
                lastChunk.ChunkEnd.CompressedBlockOffset = (ulong)_readStream.Position;

                if (_deCompressedStream != null)
                {
                    lastChunk.ChunkEnd.UncompressedBlockOffset = (ushort)_deCompressedStream.Position;
                }
                else
                {
                    lastChunk.ChunkEnd.UncompressedBlockOffset = 0;
                }
            }

            return seqMap;
        }

        /// <summary>
        /// Returns an aligned sequence by parses the BAM file.
        /// </summary>
        private SAMAlignedSequence GetAlignedSequence(int start, int end, bool isReadOnly)
        {
            byte[] array = new byte[4];

            ReadUnCompressedData(array, 0, 4);
            int blockLen = Helper.GetInt32(array, 0);
            byte[] alignmentBlock = new byte[blockLen];
            ReadUnCompressedData(alignmentBlock, 0, blockLen);
            SAMAlignedSequence alignedSeq = new SAMAlignedSequence();
            int value;
            UInt32 UnsignedValue;
            // 0-4 bytes
            int refSeqIndex = Helper.GetInt32(alignmentBlock, 0);

            alignedSeq.RName = refSeqNames[refSeqIndex];
            // 4-8 bytes
            alignedSeq.Pos = Helper.GetInt32(alignmentBlock, 4) + 1;

            // if there is no overlap no need to parse further.
            if (alignedSeq.Pos > end)
            {
                return null;
            }

            // 8 - 12 bytes "bin<<16|mapQual<<8|read_name_len"
            UnsignedValue = Helper.GetUInt32(alignmentBlock, 8);

            // 10 -12 bytes
            alignedSeq.Bin = (int)(UnsignedValue & 0xFFFF0000) >> 16;
            // 9th bytes
            alignedSeq.MapQ = (int)(UnsignedValue & 0x0000FF00) >> 8;
            // 8th bytes
            int queryNameLen = (int)(UnsignedValue & 0x000000FF);

            // 12 - 16 bytes
            UnsignedValue = Helper.GetUInt32(alignmentBlock, 12);
            // 14-16 bytes
            int flagValue = (int)(UnsignedValue & 0xFFFF0000) >> 16;
            alignedSeq.Flag = (SAMFlags)flagValue;
            // 12-14 bytes
            int cigarLen = (int)(UnsignedValue & 0x0000FFFF);

            // 16-20 bytes
            int readLen = Helper.GetInt32(alignmentBlock, 16);
            //alignedSeq.QueryLength = GetInt32(alignmentBlock, 16);

            // 20-24 bytes
            int mateRefSeqIndex = Helper.GetInt32(alignmentBlock, 20);
            if (mateRefSeqIndex != -1)
            {
                alignedSeq.MRNM = refSeqNames[mateRefSeqIndex];
            }
            else
            {
                alignedSeq.MRNM = "*";
            }

            // 24-28 bytes
            alignedSeq.MPos = Helper.GetInt32(alignmentBlock, 24) + 1;

            // 28-32 bytes
            alignedSeq.ISize = Helper.GetInt32(alignmentBlock, 28);

            // 32-(32+readLen) bytes
            alignedSeq.QName = System.Text.ASCIIEncoding.ASCII.GetString(alignmentBlock, 32, queryNameLen - 1);
            StringBuilder strbuilder = new StringBuilder();
            int startIndex = 32 + queryNameLen;

            for (int i = startIndex; i < (startIndex + cigarLen * 4); i += 4)
            {
                // Get the CIGAR operation length stored in first 28 bits.
                UInt32 cigarValue = Helper.GetUInt32(alignmentBlock, i);
                strbuilder.Append(((cigarValue & 0xFFFFFFF0) >> 4).ToString(CultureInfo.InvariantCulture));

                // Get the CIGAR operation stored in last 4 bits.
                value = (int)cigarValue & 0x0000000F;

                // MIDNSHP=>0123456
                switch (value)
                {
                    case 0:
                        strbuilder.Append("M");
                        break;
                    case 1:
                        strbuilder.Append("I");
                        break;
                    case 2:
                        strbuilder.Append("D");
                        break;
                    case 3:
                        strbuilder.Append("N");
                        break;
                    case 4:
                        strbuilder.Append("S");
                        break;
                    case 5:
                        strbuilder.Append("H");
                        break;
                    case 6:
                        strbuilder.Append("P");
                        break;
                    default:
                        throw new FileFormatException(Resource.BAM_InvalidCIGAR);
                }
            }

            alignedSeq.CIGAR = strbuilder.ToString();

            // if there is no overlap no need to parse further.
            if ((alignedSeq.Pos + alignedSeq.QueryLength) < start)
            {
                return null;
            }

            startIndex += cigarLen * 4;
            strbuilder = new StringBuilder();
            int index = startIndex;
            for (; index < (startIndex + (readLen + 1) / 2) - 1; index++)
            {
                // Get first 4 bit value
                value = (alignmentBlock[index] & 0xF0) >> 4;
                strbuilder.Append(GetSeqChar(value));
                // Get last 4 bit value
                value = alignmentBlock[index] & 0x0F;
                strbuilder.Append(GetSeqChar(value));
            }

            value = (alignmentBlock[index] & 0xF0) >> 4;
            strbuilder.Append(GetSeqChar(value));
            if (readLen % 2 == 0)
            {
                value = alignmentBlock[index] & 0x0F;
                strbuilder.Append(GetSeqChar(value));
            }

            ISequence refSeq = null;
            if (RefSequences != null && RefSequences.Count > 0)
            {
                refSeq = RefSequences.FirstOrDefault(R => string.Compare(R.ID, alignedSeq.RName, StringComparison.OrdinalIgnoreCase) == 0);
            }

            startIndex = index + 1;
            string strSequence = strbuilder.ToString();
            byte[] qualValues = new byte[readLen];
            string strQualValues = "*";

            if (alignmentBlock[startIndex] != 0xFF)
            {
                for (int i = startIndex; i < (startIndex + readLen); i++)
                {
                    qualValues[i - startIndex] = (byte)(alignmentBlock[i] + 33);
                }

                strQualValues = System.Text.ASCIIEncoding.ASCII.GetString(qualValues);
            }

            SAMParser.ParseQualityNSequence(alignedSeq, Alphabet, Encoding, strSequence, strQualValues, refSeq, isReadOnly);

            startIndex += readLen;
            if (alignmentBlock.Length > startIndex + 4 && alignmentBlock[startIndex] != 0x0 && alignmentBlock[startIndex + 1] != 0x0)
            {
                for (index = startIndex; index < alignmentBlock.Length; )
                {
                    SAMOptionalField optionalField = new SAMOptionalField();
                    optionalField.Tag = System.Text.ASCIIEncoding.ASCII.GetString(alignmentBlock, index, 2);
                    index += 2;
                    char vType = (char)alignmentBlock[index++];
                    optionalField.VType = vType.ToString();

                    // SAM format supports [AifZH] for value type.
                    // In BAM, an integer may be stored as a signed 8-bit integer (c), unsigned 8-bit integer (C), signed short (s), unsigned
                    // short (S), signed 32-bit (i) or unsigned 32-bit integer (I), depending on the signed magnitude of the integer. However,
                    // in SAM, all types of integers are presented as type ʻiʼ. 
                    string message = Helper.IsValidPatternValue("VType", optionalField.VType, "[AifZHcCsSI]");
                    if (!string.IsNullOrEmpty(message))
                    {
                        throw new FormatException(message);
                    }

                    optionalField.Value = GetOptionalValue(vType, alignmentBlock, ref index).ToString();

                    // Convert to SAM format.
                    if ("cCsSI".IndexOf(vType) >= 0)
                    {
                        optionalField.VType = "i";
                    }

                    alignedSeq.OptionalFields.Add(optionalField);
                }
            }

            return alignedSeq;
        }

        /// <summary>
        /// Reads specified number of uncompressed bytes from BAM file to byte array
        /// </summary>
        /// <param name="array">Byte array to copy.</param>
        /// <param name="offset">Offset of Byte array from which the data has to be copied.</param>
        /// <param name="count">Number of bytes to copy.</param>
        private void ReadUnCompressedData(byte[] array, int offset, int count)
        {
            if (!_isCompressed)
            {
                _readStream.Read(array, offset, count);
                return;
            }

            if (_deCompressedStream == null || _deCompressedStream.Length - _deCompressedStream.Position == 0)
            {
                GetNextBlock();
            }

            long remainingBlockSize = _deCompressedStream.Length - _deCompressedStream.Position;
            if (remainingBlockSize == 0)
            {
                return;
            }

            int bytesToRead = remainingBlockSize >= (long)count ? count : (int)remainingBlockSize;
            _deCompressedStream.Read(array, offset, bytesToRead);

            if (bytesToRead < count)
            {
                GetNextBlock();
                ReadUnCompressedData(array, bytesToRead, count - bytesToRead);
            }
        }

        /// <summary>
        /// Gets next block by reading and decompressing the compressed block from compressed BAM file.
        /// </summary>
        private void GetNextBlock()
        {
            int ELEN = 0;
            int BSIZE = 0;
            int size = 0;
            byte[] arrays = new byte[18];
            _deCompressedStream = null;
            if (_readStream.Length <= _readStream.Position)
            {
                return;
            }

            currentCompressedBlockStartPos = _readStream.Position;

            _readStream.Read(arrays, 0, 18);
            ELEN = Helper.GetUInt16(arrays, 10);

            if (ELEN != 0)
            {
                BSIZE = Helper.GetUInt16(arrays, 12 + ELEN - 2);
            }

            size = BSIZE + 1;

            byte[] block = new byte[size];
            using (MemoryStream memStream = new MemoryStream(size))
            {
                arrays.CopyTo(block, 0);

                if (_readStream.Read(block, 18, size - 18) != size - 18)
                {
                    throw new FileFormatException(Resource.BAM_UnableToReadCompressedBlock);
                }

                uint unCompressedBlockSize = Helper.GetUInt32(block, size - 4);

                _deCompressedStream = GetTempStream(unCompressedBlockSize);

                memStream.Write(block, 0, size);
                memStream.Seek(0, SeekOrigin.Begin);
                Decompress(memStream, _deCompressedStream);
            }

            _deCompressedStream.Seek(0, SeekOrigin.Begin);
        }

        /// <summary>
        /// Gets the temp stream to store Decompressed blocks.
        /// If the specified capacity is with in the Maximum integer (32 bit int) limit then 
        /// a MemoryStream is created else a temp file is created to store Decompressed data.
        /// </summary>
        /// <param name="capacity">Required capacity.</param>
        private Stream GetTempStream(uint capacity)
        {
            if (_deCompressedStream != null)
            {
                _deCompressedStream.Close();
                _deCompressedStream = null;
            }

            if (capacity <= int.MaxValue)
            {
                _deCompressedStream = new MemoryStream((int)capacity);
            }
            else
            {
                string fileName = Path.GetTempFileName();
                _deCompressedStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }

            return _deCompressedStream;
        }

        /// <summary>
        /// Returns a boolean indicating whether the reader is reached end of file or not.
        /// </summary>
        /// <returns>Returns true if the </returns>
        public bool IsEOF()
        {
            // if the BAM file is uncompressed then check the EOF by verifying the BAM file EOF.
            if (!_isCompressed || _deCompressedStream == null)
            {
                return _readStream.Length <= _readStream.Position;
            }

            // if the BAM file is compressed then verify uncompressed block.
            if (_deCompressedStream.Length <= _deCompressedStream.Position)
            {
                GetNextBlock();
            }

            return _deCompressedStream == null || _deCompressedStream.Length == 0;
        }

        // Returns SequenceAlignmentMap by prasing specified BAM stream and BAMIndexFile for the specified reference sequence index.
        private SequenceAlignmentMap GetAlignment(Stream bamStream, BAMIndexFile bamIndexFile, int refSeqIndex, bool isReadOnly)
        {
            _readStream = bamStream;
            if (_readStream == null || _readStream.Length == 0)
            {
                throw new FileFormatException(Resource.BAM_InvalidBAMFile);
            }

            ValidateReader();
            SAMAlignmentHeader header = GetHeader();
            SequenceAlignmentMap seqMap = new SequenceAlignmentMap(header);

            BAMIndex bamIndex = bamIndexFile.Read();

            if (bamIndex.RefIndexes.Count < refSeqIndex)
            {
                throw new ArgumentOutOfRangeException("refSeqIndex");
            }

            BAMReferenceIndexes refIndex = bamIndex.RefIndexes[refSeqIndex];
            IList<Chunk> chunks = GetChunks(refIndex);

            IList<SAMAlignedSequence> alignedSeqs = GetAlignedSequences(chunks, 0, int.MaxValue, isReadOnly);
            foreach (SAMAlignedSequence alignedSeq in alignedSeqs)
            {
                seqMap.QuerySequences.Add(alignedSeq);
            }

            _readStream = null;
            return seqMap;
        }

        // Returns SequenceAlignmentMap by prasing specified BAM stream and BAMIndexFile for the specified reference sequence name.
        private SequenceAlignmentMap GetAlignment(Stream bamStream, BAMIndexFile bamIndexFile, string refSeqName, bool isReadOnly)
        {
            _readStream = bamStream;
            if (_readStream == null || _readStream.Length == 0)
            {
                throw new FileFormatException(Resource.BAM_InvalidBAMFile);
            }

            ValidateReader();
            SAMAlignmentHeader header = GetHeader();
            IList<string> refSeqs = header.GetReferenceSequences();
            int refSeqIndex = refSeqs.IndexOf(refSeqName);
            if (refSeqIndex < 0)
            {
                string message = string.Format(CultureInfo.InvariantCulture, Resource.BAM_RefSeqNotFound, refSeqName);
                throw new ArgumentException(message, "refSeqName");
            }
            refSeqs = null;

            SequenceAlignmentMap seqMap = new SequenceAlignmentMap(header);

            BAMIndex bamIndex = bamIndexFile.Read();



            BAMReferenceIndexes refIndex = bamIndex.RefIndexes[refSeqIndex];
            IList<Chunk> chunks = GetChunks(refIndex);

            IList<SAMAlignedSequence> alignedSeqs = GetAlignedSequences(chunks, 0, int.MaxValue, isReadOnly);
            foreach (SAMAlignedSequence alignedSeq in alignedSeqs)
            {
                seqMap.QuerySequences.Add(alignedSeq);
            }

            _readStream = null;
            return seqMap;
        }

        // Returns SequenceAlignmentMap by prasing specified BAM stream and BAMIndexFile for the specified reference sequence index.
        // this method uses linear index information also.
        private SequenceAlignmentMap GetAlignment(Stream bamStream, BAMIndexFile bamIndexFile, string refSeqName, int start, int end, bool isReadOnly)
        {
            _readStream = bamStream;
            if (_readStream == null || _readStream.Length == 0)
            {
                throw new FileFormatException(Resource.BAM_InvalidBAMFile);
            }

            ValidateReader();
            SAMAlignmentHeader header = GetHeader();

            IList<string> refSeqs = header.GetReferenceSequences();
            int refSeqIndex = refSeqs.IndexOf(refSeqName);
            if (refSeqIndex < 0)
            {
                string message = string.Format(CultureInfo.InvariantCulture, Resource.BAM_RefSeqNotFound, refSeqName);
                throw new ArgumentException(message, "refSeqName");
            }

            refSeqs = null;

            SequenceAlignmentMap seqMap = new SequenceAlignmentMap(header);
            BAMIndex bamIndex = bamIndexFile.Read();


            BAMReferenceIndexes refIndex = bamIndex.RefIndexes[refSeqIndex];
            IList<Chunk> chunks = GetChunks(refIndex, start, end);

            IList<SAMAlignedSequence> alignedSeqs = GetAlignedSequences(chunks, start, end, isReadOnly);
            foreach (SAMAlignedSequence alignedSeq in alignedSeqs)
            {
                seqMap.QuerySequences.Add(alignedSeq);
            }

            _readStream = null;
            return seqMap;
        }

        // Returns SequenceAlignmentMap by prasing specified BAM stream and BAMIndexFile for the specified reference sequence index.
        // this method uses linear index information also.
        private SequenceAlignmentMap GetAlignment(Stream bamStream, BAMIndexFile bamIndexFile, int refSeqIndex, int start, int end, bool isReadOnly)
        {
            _readStream = bamStream;
            if (_readStream == null || _readStream.Length == 0)
            {
                throw new FileFormatException(Resource.BAM_InvalidBAMFile);
            }

            ValidateReader();
            SAMAlignmentHeader header = GetHeader();
            SequenceAlignmentMap seqMap = new SequenceAlignmentMap(header);
            BAMIndex bamIndex = bamIndexFile.Read();

            if (bamIndex.RefIndexes.Count < refSeqIndex || refSeqIndex < 0)
            {
                throw new ArgumentOutOfRangeException("refSeqIndex");
            }

            BAMReferenceIndexes refIndex = bamIndex.RefIndexes[refSeqIndex];
            IList<Chunk> chunks = GetChunks(refIndex, start, end);

            IList<SAMAlignedSequence> alignedSeqs = GetAlignedSequences(chunks, start, end, isReadOnly);
            foreach (SAMAlignedSequence alignedSeq in alignedSeqs)
            {
                seqMap.QuerySequences.Add(alignedSeq);
            }

            _readStream = null;
            return seqMap;
        }

        // Gets aligned sequence from the specified chunks of the BAM file which overlaps with the specified start and end co-ordinates.
        private IList<SAMAlignedSequence> GetAlignedSequences(IList<Chunk> chunks, int start, int end, bool isReadOnly)
        {
            List<SAMAlignedSequence> alignedSeqs = new List<SAMAlignedSequence>();
            foreach (Chunk chunk in chunks)
            {
                _readStream.Seek((long)chunk.ChunkStart.CompressedBlockOffset, SeekOrigin.Begin);
                GetNextBlock();
                if (_deCompressedStream != null)
                {
                    _deCompressedStream.Seek(chunk.ChunkStart.UncompressedBlockOffset, SeekOrigin.Begin);

                    // read until eof or end of the chunck is reached.
                    while (!IsEOF() && (currentCompressedBlockStartPos < (long)chunk.ChunkEnd.CompressedBlockOffset || _deCompressedStream.Position < chunk.ChunkEnd.UncompressedBlockOffset))
                    {
                        SAMAlignedSequence alignedSeq = GetAlignedSequence(start, end, isReadOnly);
                        if (alignedSeq != null)
                        {
                            alignedSeqs.Add(alignedSeq);
                        }
                    }
                }
            }

            return alignedSeqs;
        }

        #endregion
    }
}
