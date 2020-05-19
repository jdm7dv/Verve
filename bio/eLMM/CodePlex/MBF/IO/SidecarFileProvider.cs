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
using System.Runtime.Serialization.Formatters.Binary;

namespace Bio.IO
{
    /// <summary>
    /// Factory class which provides indexing of large sequence files. An option to index large 
    /// files which contain sequences for faster access. This class is used by Data Virtualization, 
    /// to create index (.isc) and write pointer info into the file for further access of same file.
    /// </summary>
    public class SidecarFileProvider : IDisposable
    {
        #region Fields
        /// <summary>
        /// Extension of index files which will be appended to the source filename to create a new index file.
        /// </summary>
        private const string IndexFileExtension = ".isc";

        /// <summary>
        /// Hard coded version of the file. Used for checking compatibility whcn loading index files.
        /// </summary>
        private const int FileVersion = 1;

        /// <summary>
        /// Holds an open outputStream to the index file using which we can read sequences on demand
        /// </summary>
        private readonly FileStream _inputStream;

        /// <summary>
        /// This binary reader is used to read the offset of a particular item in the payload
        /// </summary>
        private readonly BinaryReader _offsetReader;

        /// <summary>
        /// Header information of the index file
        /// </summary>
        private readonly IndexFileHeader _header;

        /// <summary>
        /// Offset from the begening of the outputStream to the starting of TOC.
        /// </summary>
        private readonly long _contentsOffset;

        /// <summary>
        /// Total size of data written when writing one sequence pointer.
        /// Used to seek to an item in a given index
        /// </summary>
        private const int PayloadBlockSize = 23;

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
        #endregion Fields

        #region constructors

        /// <summary>
        /// Initializes a new instance of IndexedSequenceProvider class.
        /// </summary>
        /// <param name="sourceFilename">Full path to the source sequence file. 
        /// This method will search for the index file by itself</param>
        private SidecarFileProvider(string sourceFilename)
        {
            try
            {
                _inputStream = new FileStream(sourceFilename + IndexFileExtension, FileMode.Open, FileAccess.Read);
                _offsetReader = new BinaryReader(_inputStream);

                _sidecarLength = _offsetReader.ReadInt64();
                _sidecarLastWriteTime = _offsetReader.ReadInt64();

                // read header info
                BinaryFormatter headerFormatter = new BinaryFormatter();
                _header = headerFormatter.Deserialize(_inputStream) as IndexFileHeader;

                _contentsOffset = _inputStream.Position; // TOC starts just after the header
            }
            catch
            {
                // Dispose of streams if opened and later crashed
                Dispose();
                throw;
            }
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
        /// <param name="index">Zero based index of the SequencePointer to fetch.</param>
        /// <returns>SequencePointer object at the specified index.</returns>
        public SequencePointer this[int index]
        {
            get
            {
                if (index >= _header.SequenceCount || index < 0)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                _inputStream.Seek(_contentsOffset, SeekOrigin.Begin);

                // find the entry for the item at the given index
                _inputStream.Seek(index * PayloadBlockSize, SeekOrigin.Current);

                // Read the required sequence pointer
                SequencePointer loadedSequence = ReadPointer(_inputStream) as SequencePointer;

                return loadedSequence;
            }
        }

        /// <summary>
        /// Creates an index file, to make sure the file creation will work fine.
        /// </summary>
        /// <param name="sourcefilename"></param>
        /// <returns></returns>
        public static bool IsIndexFileExists(string sourcefilename)
        {
            string fileName = sourcefilename + IndexFileExtension;
            try
            {
                File.Create(fileName).Close();
            }
            catch(IOException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }

            return File.Exists(fileName);
        }

        /// <summary>
        /// Method to create an index file if one does not exist.
        /// </summary>
        /// <param name="sourcefilename">Source filename.</param>
        /// <param name="sequencePointers">List of sequences in the source</param>
        /// <returns>IndexedSequenceProvider object</returns>
        public static SidecarFileProvider CreateIndexFile(string sourcefilename, IList<SequencePointer> sequencePointers)
        {
            if (sequencePointers == null)
            {
                throw new ArgumentNullException("sequencePointers");
            }

            using(FileStream outputStream = new FileStream(sourcefilename + IndexFileExtension, FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(outputStream))
                {
                    // create a new master index file
                    IndexFileHeader header = new IndexFileHeader(sourcefilename, sequencePointers.Count);

                    // Spare space for writing filesize and date later on
                    outputStream.Seek(sizeof(long) * 2, SeekOrigin.Begin);

                    // serialize header information
                    BinaryFormatter headerWriter = new BinaryFormatter();
                    headerWriter.Serialize( outputStream, header);
                    
                    // serialize sequence pointer objects
                    int index;
                    for (index = 0; index < sequencePointers.Count; index++)
                    {
                        WritePointer(outputStream, sequencePointers[index]);
                    }

                    outputStream.Seek(0, SeekOrigin.Begin);

                    // Write to the space left at top (File size and date)
                    writer.Write(outputStream.Length);
                    writer.Write(DateTime.Now.Date.ToFileTime());
                }
            }

            return new SidecarFileProvider(sourcefilename);
        }

        /// <summary>
        /// Create and return a IndexedSequenceProvider to the source file if exists
        /// </summary>
        /// <param name="sourceFilename">Source file name.</param>
        /// <returns>IndexedSequenceProvider object if an index file exists else null.</returns>
        public static SidecarFileProvider GetProvider(string sourceFilename)
        {
            SidecarFileProvider provider = null;
            string fileName = sourceFilename + IndexFileExtension;

            if (File.Exists(fileName))
            {
                try
                {
                    provider = new SidecarFileProvider(sourceFilename);
                }
                catch
                {
                    throw new OperationCanceledException();
                }
            }

            FileInfo fileInfo = new FileInfo(fileName);

            if ((provider != null && _sidecarLastWriteTime == fileInfo.LastWriteTime.Date.ToFileTime()) && (_sidecarLength == fileInfo.Length))
            {
                    // Check if the file properties match
                    if (
                        provider._header.FileVersion == FileVersion &&
                        provider._header.LastWriteTime == new FileInfo(sourceFilename).LastWriteTime)
                    {
                        return provider;
                    }
            }

            return null;
        }

        /// <summary>
        /// Implementation of IDispose to close the file streams as soon as possible
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Serialize an object to a binary outputStream.
        /// </summary>
        /// <param name="stream">The outputStream to serialize to.</param>
        /// <param name="pointer">Sequence pointer obejct to serialize</param>
        private static void WritePointer(FileStream stream, SequencePointer pointer)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            
            writer.Write(pointer.StartingLine);
            writer.Write(pointer.StartingIndex);
            writer.Write(pointer.EndingIndex);
            // Writing only first three chars of alphabet name
            writer.Write(pointer.AlphabetName.Substring(0, 3).ToCharArray());
        }

        /// <summary>
        /// Reads a sequencePointer from a binary stream.
        /// </summary>
        /// <param name="stream">The stream to deserialize from.</param>
        /// <returns>The SequencePointer object.</returns>
        private static SequencePointer ReadPointer(FileStream stream)
        {
            SequencePointer pointer = new SequencePointer();
            BinaryReader reader = new BinaryReader(stream);

            pointer.StartingLine = reader.ReadInt32();
            pointer.StartingIndex = reader.ReadInt64();
            pointer.EndingIndex = reader.ReadInt64();
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
        /// Private nested class which has the implementation of the header structure
        /// </summary>
        [Serializable]
        private class IndexFileHeader
        {
            /// <summary>
            /// File last written time.
            /// </summary>
            public DateTime LastWriteTime { get; private set; }

            /// <summary>
            /// Number of sequences in file.
            /// </summary>
            public int SequenceCount { get; private set; }

            /// <summary>
            /// Framework version info.
            /// </summary>
            public int FileVersion { get; private set; }

            /// <summary>
            /// Creates an instance of the master index.
            /// </summary>
            /// <param name="sourceFilename">Name of the source file.</param>
            /// <param name="sequenceCount">Number of sequences in the parsed file.</param>
            public IndexFileHeader(string sourceFilename, int sequenceCount)
            {
                FileInfo fileInfo = new FileInfo(sourceFilename);
                LastWriteTime = fileInfo.LastWriteTime;
                SequenceCount = sequenceCount;
                FileVersion = SidecarFileProvider.FileVersion;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    if (_inputStream != null)
                    {
                        if (_offsetReader != null)
                        {
                            _offsetReader.Close();
                        }

                        _inputStream.Close();
                    }
                }

                _disposed = true;
            }
        }
        #endregion Private Methods
    }
}
