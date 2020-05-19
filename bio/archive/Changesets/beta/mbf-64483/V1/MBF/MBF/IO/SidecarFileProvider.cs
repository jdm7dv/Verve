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
using System.IO;

namespace MBF.IO
{
    /// <summary>
    /// Factory class which provides indexing of large sequence files. An option to index large 
    /// files which contain sequences for faster access. This class is used by Data Virtualization, 
    /// to create a sidecar (.isc) and write pointer info into the file for further access of same file.
    /// </summary>
    public class SidecarFileProvider : IDisposable
    {
        #region Fields
        /// <summary>
        /// Extension of sidecar files which will be appended to the source filename to create a new sidecar file.
        /// </summary>
        private const string SidecarFileExtension = ".isc";

        /// <summary>
        /// Extension of a temporary block index file created when parsing multi-line sequences.
        /// </summary>
        private const string BlockIndexExtension = ".bix";

        /// <summary>
        /// Hard coded version of the file. Used for checking compatibility when loading sidecar files.
        /// </summary>
        private const int FileVersion = 2;

        /// <summary>
        /// Holds an open file stream to the sidecar file using which we can read sequences on demand
        /// </summary>
        private FileStream _streamReader;

        /// <summary>
        /// Holds an open file stream to the sidecar file to write sequence information to it.
        /// </summary>
        private FileStream _streamWriter;

        /// <summary>
        /// Holds an open file stream to a temporary file to which block indices are
        /// written when a multi-line sequence is being parsed.
        /// </summary>
        private FileStream _blockIndexStreamWriter;

        /// <summary>
        /// This binary reader is used to read the offset of a particular item in the payload
        /// </summary>
        private BinaryReader _offsetReader;

        /// <summary>
        /// Writes sequence index information to the sidecar file.
        /// </summary>
        private BinaryWriter _offsetWriter;

        /// <summary>
        /// Writes block index information to a temporary file when a multi-line
        /// sequence is being parsed.
        /// </summary>
        private BinaryWriter _blockIndexOffsetWriter;

        /// <summary>
        /// Header information of the sidecar file
        /// </summary>
        private SidecarFileHeader _header;

        /// <summary>
        /// Offset from the beginning of the outputStream to the starting of the sequence indexes.
        /// </summary>
        private long _contentsOffset;

        /// <summary>
        /// The total size of data written when writing one sequence pointer to
        /// the sidecar file. It is used to seek to an item at a given index.
        /// payload = StartingLine + (StartingIndex + EndingIndex) + Alphabet name character array (3 elements)
        /// </summary>
         private const int PayloadBlockSize = sizeof(int) + 2 * sizeof(long) + 3 * sizeof(byte);

        /// <summary>
        /// Tracks whether Dispose has been called.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Last written time of the sidecar file.
        /// </summary>
        private static long _sidecarLastWriteTime;

        /// <summary>
        /// Size of sidecar file.
        /// </summary>
        private static long _sidecarLength;

        /// <summary>
        /// The full path to the source file for which a sidecar is being created.
        /// </summary>
        private string _sourceFileName;

        /// <summary>
        /// The number of sequences in the source file.
        /// </summary>
        private int _sequenceCount = 0;

        /// <summary>
        /// The index of the previous sequence parsed.
        /// Used when multi-line sequences are being parsed.
        /// </summary>
        private int _prevSequenceCount = -1;

        /// <summary>
        /// The starting index of the current block.
        /// Used when multi-line sequences are being parsed.
        /// </summary>
        private long _currentBlockStartingIndex;

        /// <summary>
        /// The block size for the sequence file being parsed.
        /// </summary>
        private const int BLOCKSIZE = 4096;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of SidecarFileProvider class, either by
        /// creating a stream reader to a sidecar file that is valid for the 
        /// specified source sequence file, or by creating a stream writer to
        /// a newly created blank sidecar file.
        /// </summary>
        /// <param name="sourceFileName">Full path to the source sequence file. 
        /// This method will search for the sidecar file by itself</param>
        public SidecarFileProvider(string sourceFileName) 
            : this(sourceFileName, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of SidecarFileProvider, forcing
        /// a new sidecar file to be created.
        /// </summary>
        /// <param name="sourceFileName">Full path to the source sequence file.</param>
        /// <param name="forceCreation">
        /// A value indicating whether creation of a new sidecar file should be forced.
        /// </param>
        public SidecarFileProvider(string sourceFileName, bool forceCreation)
        {
            _sourceFileName = sourceFileName;
            SidecarFileExists = false;
            IsSidecarValid = false;
            HasBlockInfo = true;

            string sidecarFileName = sourceFileName + SidecarFileExtension;

            if (File.Exists(sidecarFileName) && forceCreation == false)
            {
                SidecarFileExists = true;
                OpenSidecar(sidecarFileName);
            }

            if (IsSidecarValid == false || forceCreation)
            {
                CreateBlankSidecarFile(sidecarFileName);
            }
        }
        #endregion

        #region Nested Classes
        /// <summary>
        /// Private nested class which has the implementation of the header structure
        /// </summary>
        [Serializable]
        private class SidecarFileHeader
        {
            /// <summary>
            /// File last written time.
            /// </summary>
            public DateTime SourceFileLastWriteTime { get; set; }

            /// <summary>
            /// Number of sequences in file.
            /// </summary>
            public int SequenceCount { get; set; }

            /// <summary>
            /// Framework version info.
            /// </summary>
            public int FileVersion { get; set; }

            /// <summary>
            /// Creates a new instance of an empty sidecar file header.
            /// </summary>
            public SidecarFileHeader()
            { }

            /// <summary>
            /// Creates a new instance of the sidecar file header.
            /// </summary>
            /// <param name="sourceFilename">Name of the source file.</param>
            /// <param name="sequenceCount">Number of sequences in the parsed file.</param>
            public SidecarFileHeader(string sourceFilename, int sequenceCount)
            {
                FileInfo fileInfo = new FileInfo(sourceFilename);
                SourceFileLastWriteTime = fileInfo.LastWriteTime;
                SequenceCount = sequenceCount;
                FileVersion = SidecarFileProvider.FileVersion;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Indicates whether the given sidecar file contains
        /// valid indexes for the specified source file.
        /// </summary>
        public bool IsSidecarValid
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether a sidecar file exists on disk.
        /// </summary>
        public bool SidecarFileExists
        {
            get;
            private set;
        }

        /// <summary>
        /// Indicates whether the given sidecar file stores
        /// x-axis block information for each sequence.
        /// </summary>
        public bool HasBlockInfo 
        { 
            get; 
            private set; 
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Return number of seqences in the original file
        /// </summary>
        public int Count
        {
            get { return _header.SequenceCount; }
        }

        /// <summary>
        /// Indexer with which one can access the sequences in the indexed file.
        /// </summary>
        /// <param name="index">The zero-based index of the SequencePointer to fetch.</param>
        /// <returns>SequencePointer object at the specified index.</returns>
        public SequencePointer this[int index]
        {
            get
            {
                if (_streamReader == null || _streamReader.CanRead == false)
                {
                    string sidecarFileName = _sourceFileName + SidecarFileExtension;
                    OpenSidecar(sidecarFileName);
                }

                if (IsSidecarValid)
                {
                    if (index >= _header.SequenceCount || index < 0)
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }

                    _streamReader.Seek(_contentsOffset, SeekOrigin.Begin);

                    // find the entry for the item at the given index
                    _streamReader.Seek(index * PayloadBlockSize, SeekOrigin.Current);

                    // Read the required sequence pointer
                    SequencePointer loadedSequence = ReadPointer(_streamReader);

                    return loadedSequence;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Method to create a sidecar file if one does not exist.
        /// </summary>
        /// <param name="sourceFileName">The name of the source file.</param>
        /// <param name="sequencePointers">
        /// A list of sequence pointers representing each sequence in the source.</param>
        /// <returns>A sidecar file object.</returns>
        public void CreateSidecarFile(string sourceFileName, IList<SequencePointer> sequencePointers)
        {
            if (string.IsNullOrEmpty(sourceFileName))
            {
                throw new ArgumentNullException("sourceFileName");
            }

            if (sequencePointers == null)
            {
                throw new ArgumentNullException("sequencePointers");
            }

            SeekToSequenceIndexesStart();

            // serialize sequence pointer objects
            int index;
            for (index = 0; index < sequencePointers.Count; index++)
            {
                WritePointer(_streamWriter, sequencePointers[index]);
            }

            _sourceFileName = sourceFileName;
         
            Close();
        }

        /// <summary>
        /// Seeks to the end of the header region in the sidecar file,
        /// which is the starting of the sequence indices.
        /// </summary>
        public void SeekToSequenceIndexesStart()
        {
            // Spare space for writing filesize and date later on
            _streamWriter.Seek(sizeof(long) * 2, SeekOrigin.Begin);

            // header =  SequenceCount (int) + FileVersion (int) + DateTime (8 bytes)
            const long headerSize = (2 * sizeof(int)) + 8;

            _streamWriter.Seek(headerSize, SeekOrigin.Current);
        }

        /// <summary>
        /// Implementation of IDispose to close the file streams as soon as possible
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Reads the header information from an sidecar file
        /// and reconstructs a header object.
        /// </summary>
        /// <param name="reader">A binary reader to the sidecar file.</param>
        /// <returns>The reconstructed header object.</returns>
        private static SidecarFileHeader ReadHeader(BinaryReader reader)
        {
            SidecarFileHeader header = new SidecarFileHeader
                                           {
                                               SourceFileLastWriteTime = DateTime.FromBinary(reader.ReadInt64()),
                                               SequenceCount = reader.ReadInt32(),
                                               FileVersion = reader.ReadInt32()
                                           };
            return header;
        }

        /// <summary>
        /// Reads the complete set of block indices for each sequence from
        /// a temporary block index file and writes it to the sidecar file,
        /// then deletes the temporary file. This method is used when parsing
        /// files containing sequences that span multiple lines.
        /// </summary>
        private void WriteBlockIndexesToSidecar()
        {
            string blockIndexFileName = _sourceFileName + BlockIndexExtension;

            using (FileStream blockIndexStreamReader = new FileStream(blockIndexFileName, FileMode.Open))
            {
                long lastWrittenPosition = 0;
                BinaryReader blockIndexOffsetReader = new BinaryReader(blockIndexStreamReader);
                long sizeOfIndexBlock = sizeof(long) * (_sequenceCount + 1);

                long sequenceBlockIndexerEnd = _streamWriter.Position + sizeOfIndexBlock;

                // starting point of the first block
                _offsetWriter.Write(sequenceBlockIndexerEnd);

                long sequenceBlockIndexerCurrentPos = _streamWriter.Position;

                // leave space to write sequence block index offsets
                _streamWriter.Seek(sequenceBlockIndexerEnd, SeekOrigin.Begin);

                while (blockIndexStreamReader.Position != blockIndexStreamReader.Length)
                {
                    long blockEndingIndex = blockIndexOffsetReader.ReadInt64();

                    if (blockIndexStreamReader.Position == blockEndingIndex)
                    {
                        lastWrittenPosition = _streamWriter.Position;
                        _streamWriter.Seek(sequenceBlockIndexerCurrentPos - sizeof(long), SeekOrigin.Begin);
                        _offsetWriter.Write((long)0);
                        _offsetWriter.Write(lastWrittenPosition);
                    }

                    else
                    {
                        while (blockIndexStreamReader.Position != blockEndingIndex)
                        {
                            // read each block index and write to sidecar file
                            _offsetWriter.Write(blockIndexOffsetReader.ReadInt32());
                        }

                        lastWrittenPosition = _streamWriter.Position;

                        _streamWriter.Seek(sequenceBlockIndexerCurrentPos, SeekOrigin.Begin);

                        _offsetWriter.Write(lastWrittenPosition);
                    }

                    sequenceBlockIndexerCurrentPos = _streamWriter.Position;

                    _streamWriter.Seek(lastWrittenPosition, SeekOrigin.Begin);
                }
            }

            if (File.Exists(blockIndexFileName))
            {
                File.Delete(blockIndexFileName);
            }
        }

        /// <summary>
        /// Writes a single block index to a temporary block index file.
        /// Used when parsing multi-line sequences.
        /// </summary>
        /// <param name="index">
        /// The block index to be written to the temporary file.
        /// </param>
        public void WriteBlockIndex(int index)
        {
            if (_blockIndexStreamWriter == null || !_blockIndexStreamWriter.CanWrite)
            {
                _blockIndexStreamWriter = new FileStream(_sourceFileName + BlockIndexExtension, FileMode.Create, FileAccess.Write);
            }
            if (_blockIndexOffsetWriter == null)
            {
                _blockIndexOffsetWriter = new BinaryWriter(_blockIndexStreamWriter);
            }

            if (_prevSequenceCount != _sequenceCount)
            {
                // if new sequence index block
                if (_prevSequenceCount != -1)
                {
                    // change the block.
                    long lastWrittenPosition = _blockIndexStreamWriter.Position;

                    //seek to beginning of the block and write the block ending index
                    _blockIndexStreamWriter.Seek(_currentBlockStartingIndex, SeekOrigin.Begin);
                    _blockIndexOffsetWriter.Write(lastWrittenPosition);

                     // seek to start of new block
                    _blockIndexStreamWriter.Seek(lastWrittenPosition, SeekOrigin.Begin);
                }

                // store the position.
                _currentBlockStartingIndex = _blockIndexStreamWriter.Position;

                // leave space to write block size after all indices have been written
                _blockIndexStreamWriter.Seek(sizeof(long), SeekOrigin.Current);
            }

            if (index != 0)
            {
                // write current index
                _blockIndexOffsetWriter.Write(index);
            }

            _prevSequenceCount = _sequenceCount;
        }

        /// <summary>
        /// Writes the block size for the last sequence block.
        /// </summary>
        public void WriteLastBlockSize()
        {
            long lastWrittenPosition = _blockIndexStreamWriter.Position;
            _blockIndexStreamWriter.Seek(_currentBlockStartingIndex, SeekOrigin.Begin);
            _blockIndexOffsetWriter.Write(lastWrittenPosition);
        }

        /// <summary>
        /// Writes the sequence pointer to a binary stream.
        /// </summary>
        /// <param name="streamWriter">The outputStream to serialize to.</param>
        /// <param name="sequencePointer">Sequence pointer obejct to serialize</param>
        public void WritePointer(Stream streamWriter, SequencePointer sequencePointer)
        {
            if (streamWriter == null)
            {
                throw new ArgumentNullException("streamWriter");
            }

            if (sequencePointer == null)
            {
                throw new ArgumentNullException("sequencePointer");
            }

            _sequenceCount++;

            BinaryWriter writer = new BinaryWriter(streamWriter);

            writer.Write(sequencePointer.StartingLine);
            writer.Write(sequencePointer.IndexOffsets[0]);
            writer.Write(sequencePointer.IndexOffsets[1]);
            // Writing only first three chars of alphabet name
            writer.Write(sequencePointer.AlphabetName.Substring(0, 3).ToCharArray());
        }

        /// <summary>
        /// Writes a specified sequence pointer to the current position in the 
        /// file stream of the sidecar file associated with this object. 
        /// </summary>
        /// <param name="sequencePointer">
        /// The sequence pointer object to be written.
        /// </param>
        public void WritePointer(SequencePointer sequencePointer)
        {
            WritePointer(_streamWriter, sequencePointer);
        }

        /// <summary>
        /// Closes all streams open for the sidecar file that
        /// this object represents.
        /// </summary>
        public void Close()
        {
            if (_streamWriter != null && _streamWriter.CanWrite)
            {
                // if this is the final step in creating a complete sidecar
                if (_sequenceCount > 0)
                {
                    // if multi-line sequence block data exists
                    if (_blockIndexStreamWriter != null && _blockIndexStreamWriter.CanWrite)
                    {
                        WriteLastBlockSize();
                        CloseBlockIndexOutputStream();
                        WriteBlockIndexesToSidecar();
                    }

                    WriteHeaderData(_sequenceCount);
                    IsSidecarValid = true;
                }
            }

            CloseInputStream();
            CloseOutputStream();
            
        }

        /// <summary>
        /// Closes any streams open for the sidecar file associated with this 
        /// object and deletes the sidecar file.
        /// </summary>
        public void Cleanup()
        {
            string sidecarFileName = _sourceFileName + SidecarFileExtension;

            Dispose();
            File.Delete(sidecarFileName);

            IsSidecarValid = false;
            SidecarFileExists = false;
        }

        /// <summary>
        /// Gets the file position of the block of sequence items to load for
        /// the specified sequence and the specified sequence item index.
        /// </summary>
        /// <param name="sequenceIndex">
        /// The starting file position of the specified sequence.
        /// </param>
        /// <param name="sequenceItemIndex">
        /// The zero-based index of the specified sequence item in the sequence.
        /// </param>
        /// <param name="firstSymbolIndex">
        /// The zero-based index of the first sequence item within the block to be loaded.
        /// </param>
        /// <param name="symbolCount">
        /// The number of sequence items in the block to be loaded.
        /// </param>
        /// <returns></returns>
        public int GetBlockToLoad(int sequenceIndex, int sequenceItemIndex, ref int firstSymbolIndex, ref int symbolCount)
        {
            firstSymbolIndex = -1;

            // seek to end of sequence pointers region 
            // (start of sequence block offset indexers, if exists)
            _streamReader.Seek(_contentsOffset + (Count * PayloadBlockSize), SeekOrigin.Begin);

            // get index of block indices for the specified sequence
            _streamReader.Seek((long)sequenceIndex * sizeof(long), SeekOrigin.Current);
            
            long sequenceBlockOffset = _offsetReader.ReadInt64();

            if (sequenceBlockOffset == 0)
            {
                return 0;
            }

            // seek to start of block indices for the specified sequence
            _streamReader.Seek(sequenceBlockOffset, SeekOrigin.Begin);

            int blockToAccess = sequenceItemIndex / BLOCKSIZE;

            _streamReader.Seek(blockToAccess * sizeof(int), SeekOrigin.Current);

            int lastSymbolIndex = _offsetReader.ReadInt32();

            while (sequenceItemIndex > lastSymbolIndex)
            {
                blockToAccess++;
                firstSymbolIndex = lastSymbolIndex + 1;
                lastSymbolIndex = _offsetReader.ReadInt32();
            }

            if (blockToAccess == 0)
            {
                firstSymbolIndex = 0;
            }

            if (firstSymbolIndex == -1)
            {
                _streamReader.Seek(-(sizeof(int) * 2), SeekOrigin.Current);
                firstSymbolIndex = 1 + _offsetReader.ReadInt32();
            }

            symbolCount = lastSymbolIndex - firstSymbolIndex + 1;

            return blockToAccess * BLOCKSIZE;
        }
        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Reads a sequence pointer from a binary stream.
        /// </summary>
        /// <param name="stream">The stream to deserialize from.</param>
        /// <returns>The SequencePointer object.</returns>
        private static SequencePointer ReadPointer(FileStream stream)
        {
            SequencePointer pointer = new SequencePointer();
            BinaryReader reader = new BinaryReader(stream);

            pointer.StartingLine = reader.ReadInt32();
            pointer.IndexOffsets[0] = reader.ReadInt64();
            pointer.IndexOffsets[1] = reader.ReadInt64();
            string alphabetName = new string(reader.ReadChars(3));

            foreach (IAlphabet alphabet in Alphabets.All)
            {
                if (alphabet.Name.StartsWith(alphabetName, StringComparison.Ordinal))
                {
                    pointer.AlphabetName = alphabet.Name;
                    break;
                }
            }

            if (string.IsNullOrEmpty(pointer.AlphabetName))
            {
                throw new FileFormatException("alphabet");
            }

            return pointer;
        }

        /// <summary>
        /// Opens an existing sidecar file and checks its validity.
        /// </summary>
        /// <param name="sidecarFileName">The path to the sidecar file.</param>
        private void OpenSidecar(string sidecarFileName)
        {
            // reset flag before proceeding
            if (IsSidecarValid)
            {
                IsSidecarValid = false;
            }

            try
            {
                _streamReader = new FileStream(sidecarFileName, FileMode.Open, FileAccess.Read);
                _offsetReader = new BinaryReader(_streamReader);

                _sidecarLength = _offsetReader.ReadInt64();
                _sidecarLastWriteTime = _offsetReader.ReadInt64();

                FileInfo sidecarFileInfo = new FileInfo(sidecarFileName);

                // validate the sidecar file properties
                if ((_sidecarLastWriteTime == sidecarFileInfo.LastWriteTime.Date.ToFileTime()) && (_sidecarLength == sidecarFileInfo.Length))
                {
                    _header = ReadHeader(_offsetReader);

                    // validate the source file properties
                    if (_header.FileVersion == FileVersion && _header.SourceFileLastWriteTime == new FileInfo(_sourceFileName).LastWriteTime)
                    {
                        IsSidecarValid = true;

                        // sequence indexes start just after header
                        _contentsOffset = _streamReader.Position;
                    }
                    else
                    {
                        CloseInputStream();
                    }

                }
                else // close streams if sidecar is invalid
                {
                    CloseInputStream();
                }
            }
            catch (Exception)
            {
                CloseInputStream();
            }
        }

        /// <summary>
        /// Closes all input streams open for the sidecar file that
        /// this object represents.
        /// </summary>
        public void CloseInputStream()
        {
            if (_streamReader != null)
            {
                if (_offsetReader != null)
                {
                    _offsetReader.Close();
                }

                _streamReader.Close();
            }
        }

        /// <summary>
        /// Closes all output streams open for the sidecar file that
        /// this object represents.
        /// </summary>
        private void CloseOutputStream()
        {
            if (_streamWriter != null)
            {
                if (_offsetWriter != null)
                {
                    _offsetWriter.Close();
                }

                _streamWriter.Close();
            }
        }

        /// <summary>
        /// Closes the output stream created to store block
        /// information for multi-line sequences.
        /// </summary>
        private void CloseBlockIndexOutputStream()
        {
            if (_blockIndexStreamWriter != null)
            {
                if (_blockIndexOffsetWriter != null)
                {
                    _blockIndexOffsetWriter.Close();
                }

                _blockIndexStreamWriter.Close();
            }
        }

        /// <summary>
        /// Create a blank sidecar file.
        /// </summary>
        /// <param name="sidecarFileName">The path to the sidecar file.</param>
        private void CreateBlankSidecarFile(string sidecarFileName)
        {
            if (string.IsNullOrEmpty(sidecarFileName))
            {
                throw new ArgumentNullException("sidecarFileName");
            }

            // reset flag before proceeding
            if (SidecarFileExists)
            {
                SidecarFileExists = false;
            }

            if (IsSidecarValid)
            {
                IsSidecarValid = false;
            }

            try
            {
                if (File.Exists(sidecarFileName))
                {
                    File.Delete(sidecarFileName);
                }

                _streamWriter = new FileStream(sidecarFileName, FileMode.Create, FileAccess.Write);
                _offsetWriter = new BinaryWriter(_streamWriter);

                SeekToSequenceIndexesStart();

                SidecarFileExists = true;
            }
            catch (Exception)
            {
                CloseOutputStream();
            }
        }

        /// <summary>
        /// Writes the header to the sidecar file, and also writes information
        /// about the sidecar file itself.
        /// </summary>
        /// <param name="sequenceCount">The number of sequences in the source file.</param>
        /// <returns>true if the header was successfully created; otherwise, false</returns>
        private void WriteHeaderData(int sequenceCount)
        {
            _header = new SidecarFileHeader(_sourceFileName, sequenceCount);
            _streamWriter.Seek(0, SeekOrigin.Begin);

            // Write sidecar file size and last written date to the space left at top
            _offsetWriter.Write(_streamWriter.Length);
            _offsetWriter.Write(DateTime.Now.Date.ToFileTime());

            _offsetWriter.Write(_header.SourceFileLastWriteTime.ToBinary());
            _offsetWriter.Write(_header.SequenceCount);
            _offsetWriter.Write(_header.FileVersion);
        }

        /// <summary>
        /// Implementation of IDispose to close the file streams as soon as possible
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    CloseInputStream();
                    CloseOutputStream();
                    CloseBlockIndexOutputStream();
                }

                _disposed = true;
            }
        }
        #endregion Private Methods
    }
}
