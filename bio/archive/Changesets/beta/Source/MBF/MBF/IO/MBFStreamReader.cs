// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.IO;
using System.Text;

namespace MBF.IO
{
    /// <summary>
    /// A MBFStreamReader reads from a biological sequence file stream.
    /// </summary>
    public class MBFStreamReader : IDisposable
    {
        #region Fields

        /// <summary>
        /// 1 KB
        /// </summary>
        private const int KBytes = 1024;

        /// <summary>
        /// Buffer size (4KB) of reading block
        /// </summary>
        private const int bufferSize = 4 * KBytes;

        private StreamReader _reader;
        private int _bufferPosition;
        private long _filePosition;
        private char[] _buffer;
        private StringBuilder _lineBuilder;
        private bool isEndOfFile;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the MBFStreamReader class.
        /// Constructor from a file name using the default data indent; opens a StreamReader on
        /// the file, and reads in the first line of text, with SkipBlankLines set to true.
        /// </summary>
        /// <param name="fileName">The filename to open.</param>
        public MBFStreamReader(string fileName)
            : this(fileName, new StreamReader(fileName), true)
        { }

        /// <summary>
        /// Initializes a new instance of the MBFStreamReader class.
        /// Constructor from a file name using the default data indent; opens a StreamReader on
        /// the file, and reads in the first line of text, with SkipBlankLines set to the specified value.
        /// </summary>
        /// <param name="filename">The filename to open.</param>
        /// <param name="skipBlankLines">Indicates whether to skip blank lines or not.</param>
        public MBFStreamReader(string filename, bool skipBlankLines)
            : this(filename, new StreamReader(filename), skipBlankLines)
        { }

        /// <summary>
        /// Initializes a new instance of the MBFStreamReader class.
        /// Constructor from a Stream using the default data indent; opens a StreamReader on
        /// the Stream, and reads in the first line of text.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        public MBFStreamReader(Stream stream)
            : this(null, new StreamReader(stream), true)
        { }

        /// <summary>
        /// Initializes a new instance of the MBFStreamReader class.
        /// Constructor from a Stream using the default data indent; opens a StreamReader on
        /// the Stream, and reads in the first line of text, with SkipBlankLines set to the specified value
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <param name="skipBlankLines">Indicates whether to skip blank lines or not.</param>
        public MBFStreamReader(Stream stream, bool skipBlankLines)
            : this(null, new StreamReader(stream), skipBlankLines)
        { }

        /// <summary>
        /// Initializes a new instance of the MBFStreamReader class.
        /// </summary>
        /// <param name="filename">The filename to open.</param>
        /// <param name="reader">The stream to read.</param>
        /// <param name="skipBlankLines">Indicates whether to skip blank lines or not.</param>
        private MBFStreamReader(string filename, StreamReader reader, bool skipBlankLines)
        {
            FileName = filename;
            _reader = reader;
            _buffer = new char[bufferSize];
            _filePosition = 0;
            _bufferPosition = bufferSize;
            _lineBuilder = new StringBuilder();
            SkipBlankLines = skipBlankLines;
            NewLineCharacterCount = 0;
            GoToNextLine();
        }
        #endregion
        
        #region Properties
        /// <summary>
        /// The full path to the file.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets a value indicating whether current line is not past the end of the formatted text.
        /// </summary>
        public bool HasLines
        {
            get 
            {
                return (Line != null);
            }
        }

        /// <summary>
        /// Gets current line of text.
        /// </summary>
        public string Line { get; private set; }

        /// <summary>
        /// Gets the number of new line characters that
        /// terminate the current line stored in the Line property.
        /// </summary>
        public int NewLineCharacterCount { get; private set; }

        /// <summary>
        /// Gets the starting index of the current line
        /// held by MBFStreamReader.
        /// </summary>
        public long CurrentLineStartingIndex { get; private set; }

        /// <summary>
        /// Gets the number of new line characters for
        /// the current sequence being parsed.
        /// </summary>
        public long NumberOfNewlineCharacters { get; private set; }

        /// <summary>
        /// Gets the current position within the stream.
        /// </summary>
        public long Position
        {
            get
            { 
                return _filePosition; 
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not blank lines should be skipped when GoToNextLine is called.
        /// </summary>
        public bool SkipBlankLines { get; set; }

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        public bool CanRead
        {
            get
            {
                if (_reader != null)
                {
                    return _reader.BaseStream.CanRead;
                }
                else
                {
                    return false;
                }
            }
        }
        #endregion

        /// <summary>
        /// Reads the next line of text, storing it in the Line property.  If SkipBlankLines is
        /// true, any lines containing only white space are skipped.
        /// </summary>
        public virtual void GoToNextLine()
        {
            do
            {
                Line = this.ReadLine();
            }
            while (SkipBlankLines && !isEndOfFile && string.IsNullOrEmpty(Line.Trim()));
        }

        /// <summary>
        /// Access substrings from the current line in the form used by the specs for many
        /// file formats: start and end positions, inclusive, one-based.  Allows the use
        /// of numbers directly from specs.
        /// </summary>
        /// <param name="start">1-based index of first character of field.</param>
        /// <param name="end">1-based index of last character of field.</param>
        /// <returns>A string of the requested field.</returns>
        public string GetLineField(int start, int end)
        {
            if (start < 1)
            {
                throw new ArgumentException(Properties.Resource.ParameterMustNonNegative, "start");
            }

            if (end < 1)
            {
                throw new ArgumentException(Properties.Resource.ParameterMustNonNegative, "start");
            }

            if (start > end)
            {
                throw new ArgumentException(Properties.Resource.InvalidStartNEndPositions);
            }

            return Line.Substring(start - 1, end - start + 1);
        }

        /// <summary>
        /// Access substrings from the current line in the form used by the specs for many
        /// file formats: start and end positions, inclusive, one-based.  Allows the use
        /// of numbers directly from specs.
        /// </summary>
        /// <param name="start">1-based index of first character of field.</param>
        /// <returns>A string of the requested field.</returns>
        public string GetLineField(int start)
        {
            if (start < 1)
            {
                throw new ArgumentException(Properties.Resource.ParameterMustNonNegative, "start");
            }

            return Line.Substring(start - 1);
        }

        private void ReadBlock()
        {
            // buffer position is at the end
            int numberOfBytesRead = _reader.ReadBlock(_buffer, 0, bufferSize);
            if (numberOfBytesRead < bufferSize)
                _buffer[numberOfBytesRead] = '\0';
            _bufferPosition = 0;
        }

        /// <summary>
        /// Reads the next block of characters from the sequence file into
        /// the specified character array.
        /// </summary>
        /// <param name="index">
        /// Zero-based starting index of the range of sequence characters to retrieve.
        /// This index must be the exact position within the actual file.
        /// </param>
        /// <param name="count">
        /// The number of characters to be read.
        /// </param>
        /// <returns>The number of characters read.</returns>
        public char[] ReadChars(long index, int count)
        {
            if (count < 0)
            {
                throw new ArgumentException(Properties.Resource.ParameterMustNonNegative, "count");
            }

            int j = 0;
            bool endOfFile = false;
            char[] destArray = new char[count];

            Seek(index, SeekOrigin.Begin);

            ReadBlock();

            for (; _bufferPosition < bufferSize && !endOfFile && j < count; _bufferPosition++)
            {
                if (_buffer[_bufferPosition] == '\0')
                {
                    endOfFile = true;
                    break;
                }

                if (_buffer[_bufferPosition] != '\r' && _buffer[_bufferPosition] != '\n')
                {
                    destArray[j++] = _buffer[_bufferPosition];
                }
                else
                {
                    NumberOfNewlineCharacters++;
                }

                _filePosition++;
            }

            return destArray;
        }

        /// <summary>
        /// Reads the next block of characters from the sequence file into
        /// the specified byte array.
        /// </summary>
        /// <param name="index">
        /// Zero-based starting index of the range of sequence characters to retrieve.
        /// This index must be the exact position within the actual file.
        /// </param>
        /// <param name="count">
        /// The number of characters to be read.
        /// </param>
        /// <returns>Returns byte array.</returns>
        public byte[] ReadBytes(long index, int count)
        {
            if (count < 0)
            {
                throw new ArgumentException(Properties.Resource.ParameterMustNonNegative, "count");
            }

            int j = 0;
            bool endOfFile = false;
            byte[] destArray = new byte[count];

            Seek(index, SeekOrigin.Begin);

            ReadBlock();

            for (; _bufferPosition < bufferSize && !endOfFile && j < count; _bufferPosition++)
            {
                if (_buffer[_bufferPosition] == '\0')
                {
                    endOfFile = true;
                    break;
                }

                if (_buffer[_bufferPosition] != '\r' && _buffer[_bufferPosition] != '\n')
                {
                    int value=(int)_buffer[_bufferPosition];
                    
                    if(value >= 97 && value <=122)
                    {
                        value-=32;
                    }

                    destArray[j++] = (byte)value;
                }
                else
                {
                    NumberOfNewlineCharacters++;
                }

                _filePosition++;
            }

            return destArray;
        }

        /// <summary>
        /// Reads a line of characters from the current stream and returns the data as a string.
        /// </summary>
        /// <returns>
        /// The next line from the input stream, or null if the end 
        /// of the input stream is reached.
        /// </returns>
        public string ReadLine()
        {
            _lineBuilder.Clear();
            bool reachedNewLine;
            int validCharacterCount = 0;
            NewLineCharacterCount = 0;
            CurrentLineStartingIndex = _filePosition;
            int internalStartingIndex = _bufferPosition;
            do
            {
                if (_bufferPosition >= bufferSize)
                {
                    ReadBlock();
                    internalStartingIndex = _bufferPosition;
                }

                reachedNewLine = false;

                for (; _bufferPosition < bufferSize; _bufferPosition++, _filePosition++)
                {
                    if (_buffer[_bufferPosition] == '\0')
                    {
                        isEndOfFile = true;
                        break;
                    }
                    if (_buffer[_bufferPosition] == '\r')
                    {
                        NewLineCharacterCount++;
                        continue;
                    }
                    else if (_buffer[_bufferPosition] == '\n')
                    {
                        NewLineCharacterCount++;
                        reachedNewLine = true;
                        continue;
                    }
                    else
                    {
                        if (isEndOfFile || reachedNewLine)
                        {
                            break;
                        }
                        validCharacterCount++;
                    }
                }

                _lineBuilder.Append(_buffer, internalStartingIndex, validCharacterCount);
                validCharacterCount = 0;

            } while (!reachedNewLine && !isEndOfFile);

            if (_lineBuilder.Length > 0)
            {
                return Line = _lineBuilder.ToString();
            }
            else if (isEndOfFile)
            {
                return null;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="position">The position to seek to.</param>
        /// <param name="seekOrigin">The reference position in the stream.</param>
        public void Seek(long position, SeekOrigin seekOrigin)
        {
            if (_reader != null)
            {
                _reader.DiscardBufferedData();
                _filePosition = _reader.BaseStream.Seek(position, seekOrigin);
                if (isEndOfFile && _reader.BaseStream.Position < _reader.BaseStream.Length)
                {
                    isEndOfFile = false;
                }

                _bufferPosition = bufferSize;
            }
        }

        /// <summary>
        /// Closes the stream reader.
        /// </summary>
        public void Close()
        {
            if (_reader != null)
            {
                _reader.Close();
            }
        }

        /// <summary>
        /// Implements dispose to supress GC finalize
        /// This is done as one of the methods uses ReadWriterLockSlim
        /// which extends IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose field instances
        /// </summary>
        /// <param name="disposeManaged">If disposeManaged equals true, clean all resources</param>
        protected virtual void Dispose(bool disposeManaged)
        {
            if (_reader != null && disposeManaged)
            {
                _reader.Close();
                _reader.Dispose();
                _reader = null;
            }
        }
    }
}