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
using System.Globalization;
using System.IO;
using System.Linq;

namespace Bio.IO
{
    /// <summary>
    /// A BioTextReader reads from a source of formatted text one line at a time, storing each line
    /// until the next is read.
    /// </summary>
    /// <remarks>
    /// The BioTextReader class implements some simple helper methods, such as separating
    /// line headers from data, and allowing access to data fields using the 1-based, inclusive index
    /// notation common to the specs of many file formats. When a BioTextReader is constructed,
    /// the first line of text is read immediately so that it is available without having to call
    /// GoToNextLine.  The initial call of GoToNextLine will therefore set Line equal to the
    /// second non-empty line of text.  If SkipBlankLines is true, lines containing only white space
    /// are passed over by GoToNextLine.
    /// </remarks>
    public class BioTextReader : IDisposable
    {
        #region Fields

        /// <summary>
        /// The default data indent used if none is supplied at construction = 12
        /// </summary>
        public const int DefaultDataIndent = 12;

        /// <summary>
        /// Reader to read the content of text file
        /// </summary>
        private TextReader _reader;

        /// <summary>
        /// The number of spaces that the data (i.e. non-header) portion of each line is indented.
        /// </summary>
        private int _dataIndent;

        /// <summary>
        /// Line header
        /// </summary>
        private string _lineHeader;

        /// <summary>
        /// Line of data
        /// </summary>
        private string _lineData;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the BioTextReader class.
        /// Constructs from a TextReader using the default data indent, and reads in the first
        /// line of text, with SkipBlankLines set to true.
        /// </summary>
        /// <param name="reader">The TextReader to read from.</param>
        public BioTextReader(TextReader reader)
            : this(null, reader, DefaultDataIndent, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BioTextReader class.
        /// Constructs from a TextReader using the given data indent, and reads in the first
        /// line of text, with SkipBlankLines set to true.
        /// </summary>
        /// <param name="reader">The TextReader to read from.</param>
        /// <param name="dataIndent">The number of spaces that data is indented within the 
        /// formatted text.</param>
        public BioTextReader(TextReader reader, int dataIndent)
            : this(null, reader, dataIndent, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BioTextReader class.
        /// Constructs from a TextReader using the given data indent, and reads in the first
        /// line of text.
        /// </summary>
        /// <param name="reader">The TextReader to read from.</param>
        /// <param name="dataIndent">The number of spaces that data is indented within the 
        /// formatted text.</param>
        /// <param name="skipBlandLines">Whether to skip blank lines when reading the
        /// next line.</param>
        public BioTextReader(TextReader reader, int dataIndent, bool skipBlandLines)
            : this(null, reader, dataIndent, skipBlandLines)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BioTextReader class.
        /// Constructor from a file name using the default data indent; opens a StreamReader on
        /// the file, and reads in the first line of text, with SkipBlankLines set to true.
        /// </summary>
        /// <param name="fileName">The filename to open.</param>
        public BioTextReader(string fileName)
            : this(fileName, new StreamReader(fileName), DefaultDataIndent, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BioTextReader class.
        /// Constructor from a file name using the given data indent; opens a StreamReader on
        /// the file, and reads in the first line of text, with SkipBlankLines set to true.
        /// </summary>
        /// <param name="fileName">The filename to open.</param>
        /// <param name="dataIndent">The number of spaces that data is indented within the 
        /// formatted text.</param>
        public BioTextReader(string fileName, int dataIndent)
            : this(fileName, new StreamReader(fileName), dataIndent, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BioTextReader class.
        /// Constructor from a file name using the given data indent; opens a StreamReader on
        /// the file, and reads in the first line of text.
        /// </summary>
        /// <param name="fileName">The filename to open.</param>
        /// <param name="dataIndent">The number of spaces that data is indented within the 
        /// formatted text.</param>
        /// <param name="skipBlandLines">Whether to skip blank lines when reading the
        /// next line.</param>
        public BioTextReader(string fileName, int dataIndent, bool skipBlandLines)
            : this(fileName, new StreamReader(fileName), dataIndent, skipBlandLines)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BioTextReader class.
        /// Constructor from a Stream using the default data indent; opens a StreamReader on
        /// the Stream, and reads in the first line of text, with SkipBlankLines set to true.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        public BioTextReader(Stream stream)
            : this(null, new StreamReader(stream), DefaultDataIndent, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BioTextReader class.
        /// Constructor from a Stream using the given data indent; opens a StreamReader on
        /// the Stream, and reads in the first line of text, with SkipBlankLines set to true.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <param name="dataIndent">The number of spaces that data is indented within the 
        /// formatted text.</param>
        public BioTextReader(Stream stream, int dataIndent)
            : this(null, new StreamReader(stream), dataIndent, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BioTextReader class.
        /// Constructor from a Stream using the given data indent; opens a StreamReader on
        /// the Stream, and reads in the first line of text.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <param name="dataIndent">The number of spaces that data is indented within the 
        /// formatted text.</param>
        /// <param name="skipBlandLines">Whether to skip blank lines when reading the
        /// next line.</param>
        public BioTextReader(Stream stream, int dataIndent, bool skipBlandLines)
            : this(null, new StreamReader(stream), dataIndent, skipBlandLines)
        {
        }

        /// <summary>
        /// Initializes a new instance of the BioTextReader class.
        /// </summary>
        /// <param name="filename">The filename to open.</param>
        /// <param name="reader">The stream to read.</param>
        /// <param name="dataIndent">The number of spaces that data is indented within the 
        /// formatted text.</param>
        /// <param name="skipBlankLines">Whether to skip blank lines when reading the
        /// next line.</param>
        private BioTextReader(string filename, TextReader reader, int dataIndent, bool skipBlankLines)
        {
            FileName = filename;
            _reader = reader;
            SkipBlankLines = skipBlankLines;
            DataIndent = dataIndent;

            LineNumber = 0;
            GoToNextLine();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets file to read, or null if the file name is not known.
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets current line of text.
        /// </summary>
        public string Line { get; private set; }

        /// <summary>
        /// Gets number of the current line of text.  (Line numbering starts at 1.)
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not blank lines should be skipped when GoToNextLine is called.
        /// </summary>
        public bool SkipBlankLines { get; set; }

        /// <summary>
        /// Gets or sets number of spaces that the data (i.e. non-header) portion of each line is indented.
        /// </summary>
        public virtual int DataIndent
        {
            get
            { 
                return _dataIndent; 
            }

            set
            {
                if (_dataIndent != value)
                {
                    _dataIndent = value;
                    _lineHeader = null;
                    _lineData = null;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether current line is not past the end of the formatted text.
        /// </summary>
        public bool HasLines
        {
            get { return Line != null; }
        }

        /// <summary>
        /// Gets a value indicating whether current line contains anything other than white space to the
        /// left of the indent.
        /// </summary>
        public bool LineHasHeader
        {
            get { return LineHeader.Length > 0; }
        }

        /// <summary>
        /// Gets a value indicating whether current line contains anything other than white space to the
        /// right of the indent.
        /// </summary>
        public bool LineHasData
        {
            get { return LineData.Length > 0; }
        }

        /// <summary>
        /// Gets trimmed portion of the current line to the left of the data indent.
        /// Populated on demand.
        /// </summary>
        public virtual string LineHeader
        {
            get
            {
                if (_lineHeader == null)
                {
                    if (Line.Length >= DataIndent)
                    {
                        _lineHeader = Line.Substring(0, DataIndent).Trim();
                    }
                    else
                    {
                        _lineHeader = Line.Trim();
                    }
                }

                return _lineHeader;
            }
        }

        /// <summary>
        /// Gets trimmed portion of the current line to the right of the data indent.
        /// Populated on demand.
        /// </summary>
        public virtual string LineData
        {
            get
            {
                if (_lineData == null)
                {
                    if (Line.Length >= DataIndent)
                    {
                        _lineData = Line.Substring(DataIndent).Trim();
                    }
                    else
                    {
                        _lineData = string.Empty;
                    }
                }

                return _lineData;
            }
        }

        /// <summary>
        /// Gets human readable string giving the filename and current line number.
        /// </summary>
        public string LocationString
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture,
                        "{0}line {1}",
                        FileName == null ? string.Empty : "file '" + FileName + "' ",
                        LineNumber);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reads the next line of text, storing it in the Line property.  If SkipBlankLines is
        /// true, any lines containing only white space are skipped.
        /// </summary>
        public virtual void GoToNextLine()
        {
            do
            {
                // StreamReader streamReader = _reader as StreamReader;
                // streamReader.DiscardBufferedData();
                Line = _reader.ReadLine();
                LineNumber++;
            } 
            while (SkipBlankLines && Line != null && string.IsNullOrEmpty(Line.Trim()));

            // don't set header and data till the user asks for them
            _lineHeader = null;
            _lineData = null;
        }

        /// <summary>
        /// Reads until the next line starting with non-white space is reached, storing it
        /// in the Line property.
        /// </summary>
        public void SkipToNextSection()
        {
            GoToNextLine();
            while (HasLines && Line.StartsWith(" ", StringComparison.OrdinalIgnoreCase))
            {
                GoToNextLine();
            }
        }

        /// <summary>
        /// Set the file offset to begin of the file
        /// </summary>
        public void StartFromBegin()
        {
            StreamReader sr = _reader as StreamReader;
            if (sr != null && sr.BaseStream != null)
            {
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
            }
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
            return Line.Substring(start - 1);
        }

        /// <summary>
        /// Read a specific block from the starting position
        /// </summary>
        /// <param name="startingIndex">starting index of sequence</param>
        /// <param name="sequenceStartPosition">file position of sequence</param>
        /// <param name="blockLength">block size</param>
        /// <param name="sequenceLength">sequence length</param>
        /// <returns>block of string</returns>
        public string ReadBlock(long startingIndex, long sequenceStartPosition, int blockLength, int sequenceLength)
        {
            const int maximumBufferSize = 4096;

            StreamReader streamReader = _reader as StreamReader;

            string data = string.Empty;

            // Discard any buffered data
            if (streamReader != null)
            {
                streamReader.DiscardBufferedData();

                // Go to the start of sequence
                streamReader.BaseStream.Seek(sequenceStartPosition, SeekOrigin.Begin);
            }

            // Number of bytes read
            int totalBytesRead = 0;
            int bytesRead = 0;
            int bytesToRead = 0;
            char[] buffer;

            if (blockLength + startingIndex >= sequenceLength)
            {
                blockLength = (int) (sequenceLength - startingIndex);
            }
            
            while (totalBytesRead < startingIndex)
            {
                bytesToRead = (int)((maximumBufferSize < startingIndex - totalBytesRead)
                                ? maximumBufferSize
                                : startingIndex - totalBytesRead);
                //bytesToRead = blockLength;

                buffer = new char[bytesToRead];
                bytesRead = _reader.Read(buffer, 0, bytesToRead);

                if (0 == bytesRead)
                {
                    // We have reached EOF.
                    break;
                }

                buffer = buffer.Where(c => ((c != '\n') && (c != '\r'))).ToArray();

                totalBytesRead += buffer.Length;
            }

            totalBytesRead = 0;
            bytesRead = 0;
            bytesToRead = 0;
            // now read
            while (totalBytesRead < blockLength)
            {
                bytesToRead = (maximumBufferSize < blockLength - totalBytesRead)
                                  ? maximumBufferSize
                                  : blockLength - totalBytesRead;
                //bytesToRead = blockLength;

                buffer = new char[bytesToRead];
                bytesRead = _reader.Read(buffer, 0, bytesToRead);

                if (0 == bytesRead)
                {
                    // We have reached EOF.
                    break;
                }

                buffer = buffer.Where(c => ((c != '\n') && (c != '\r'))).ToArray();

                totalBytesRead += buffer.Length;
                data = String.Format("{0}{1}", data, new string(buffer));
            }

            return data;
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
        /// Dispose the file reader instances
        /// </summary>
        /// <param name="disposing">If disposing equals true, dispose all resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != FileName && null != _reader)
                {
                    // we opened the reader, so dispose it
                    _reader.Dispose();
                    _reader = null;
                    FileName = null;
                }
            }
        }

        #endregion
    }
}
