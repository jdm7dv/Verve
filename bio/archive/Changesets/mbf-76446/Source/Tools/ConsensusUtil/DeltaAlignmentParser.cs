// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bio.Algorithms.Alignment;
using System.Globalization;
using Bio;
using ConsensusUtil.Properties;

namespace ConsensusUtil.IO
{
     /// <summary>
    /// This parser reads from a source of text that contains DeltaAlignments
    /// and converts the data to in-memory DeltaAlignment objects.  
    /// </summary>
    public sealed class DeltaAlignmentParser : IDisposable
    {
        #region Member variables
        /// <summary>
        /// The Size is 1 KB.
        /// </summary>
        public const int KBytes = 1024;

        /// <summary>
        /// The Size is 1 MB.
        /// </summary>
        public const int MBytes = 1024 * KBytes;

        /// <summary>
        /// The Size is 1 GB.
        /// </summary>
        public const int GBytes = 1024 * MBytes;

        /// <summary>
        /// Buffer size.
        /// </summary>
        private const int BufferSize = 256 * MBytes;

        /// <summary>
        /// Maximum sequence length.
        /// </summary>
        private const long MaximumSequenceLength = (long)2 * GBytes;

        /// <summary>
        /// String to hold the current line in file
        /// </summary>
        private string line = string.Empty;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the DeltaAlignmentParser class by 
        /// loading the specified filename.
        /// </summary>
        /// <param name="filename">Name of the File.</param>
        public DeltaAlignmentParser(string filename)
        {
            this.Open(filename);
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the filename.
        /// </summary>
        public string Filename { get; private set; }

        #endregion

        #region Methods
        /// <summary>
        /// Opens the stream for the specified file.
        /// </summary>
        /// <param name="filename">Name of the file to open.</param>
        public void Open(string filename)
        {
            // if the file is alread open throw invalid 
            if (!string.IsNullOrEmpty(this.Filename))
            {
                throw new InvalidOperationException();
            }

            // Validate the file - by try to open.
            using (new StreamReader(filename))
            {
            }

            this.Filename = filename;
        }

        /// <summary>
        /// Returns an IEnumerable of DeltaAlignment in the file being parsed.
        /// </summary>
        /// <returns>Returns DeltaAlignment collection.</returns>
        public IList<DeltaAlignment> Parse()
        {
            bool skipBlankLine = true;
            int currentBufferSize = BufferSize;
            byte[] buffer = new byte[currentBufferSize];
            IAlphabet alphabet = null;
            IList<DeltaAlignment> deltaAlignments = new List<DeltaAlignment>();
            string message = string.Empty;

            using (StreamReader streamReader = new StreamReader(this.Filename))
            {
                if (streamReader.EndOfStream)
                {
                    message = string.Format(
                                CultureInfo.InvariantCulture,
                                Resources.INVALID_INPUT_FILE,
                                Resources.Parser_Name);

                    throw new FileFormatException(message);
                }

                ReadNextLine(streamReader);

                do
                {
                    if (line == null || !line.StartsWith(">", StringComparison.OrdinalIgnoreCase))
                    {
                        message = string.Format(
                                CultureInfo.InvariantCulture,
                                Resources.INVALID_INPUT_FILE,
                                Resources.Parser_Name);

                        throw new FileFormatException(message);
                    }

                    //First line - reference id
                    string referenceId = line.Substring(1);
                    int bufferPosition = 0;

                    // Read next line.
                    ReadNextLine(streamReader);

                    //Second line - Query sequence id
                    string queryId = line;

                    //third line - query sequence
                    // Read next line.
                    ReadNextLine(streamReader);

                    // For large files copy the data in memory mapped file.
                    if ((((long)bufferPosition + line.Length) >= MaximumSequenceLength))
                    {
                        throw new ArgumentOutOfRangeException(
                            string.Format(CultureInfo.CurrentUICulture, Resources.SequenceDataGreaterthan2GB, queryId));
                    }

                    if (((bufferPosition + line.Length) >= currentBufferSize))
                    {
                        Array.Resize<byte>(ref buffer, buffer.Length + BufferSize);
                        currentBufferSize += BufferSize;
                    }

                    byte[] symbols = ASCIIEncoding.ASCII.GetBytes(line);

                    // Array.Copy -- for performance improvement.
                    Array.Copy(symbols, 0, buffer, bufferPosition, symbols.Length);

                    alphabet = Alphabets.AutoDetectAlphabet(buffer, bufferPosition, bufferPosition + line.Length, alphabet);
                    if (alphabet == null)
                    {
                        throw new FileFormatException(string.Format(Resources.InvalidSymbolInString, line));
                    }

                    bufferPosition += line.Length;

                    // Truncate buffer to remove trailing 0's

                    byte[] tmpBuffer = new byte[bufferPosition];
                    Array.Copy(buffer, tmpBuffer, bufferPosition);

                    Sequence sequence = null;

                    // In memory sequence
                    sequence = new Sequence(alphabet, tmpBuffer, false);
                    sequence.ID = queryId;

                    Sequence refEmpty = new Sequence(sequence.Alphabet, "A", false);
                    refEmpty.ID = referenceId;

                    DeltaAlignment deltaAlignment = new DeltaAlignment(refEmpty, sequence);

                    //Fourth line - properties of deltaalignment
                    // Read next line.
                    ReadNextLine(streamReader);

                    string[] deltaAlignmentProperties = line.Split(' ');
                    if (deltaAlignmentProperties != null && deltaAlignmentProperties.Length == 7)
                    {
                        long temp;
                        deltaAlignment.FirstSequenceStart = long.TryParse(deltaAlignmentProperties[0], out temp) ? temp : 0;
                        deltaAlignment.FirstSequenceEnd = long.TryParse(deltaAlignmentProperties[1], out temp) ? temp : 0;
                        deltaAlignment.SecondSequenceStart = long.TryParse(deltaAlignmentProperties[2], out temp) ? temp : 0;
                        deltaAlignment.SecondSequenceEnd = long.TryParse(deltaAlignmentProperties[3], out temp) ? temp : 0;
                        int error;
                        deltaAlignment.Errors = int.TryParse(deltaAlignmentProperties[4], out error) ? error : 0;
                        deltaAlignment.SimilarityErrors = int.TryParse(deltaAlignmentProperties[5], out error) ? error : 0;
                        deltaAlignment.NonAlphas = int.TryParse(deltaAlignmentProperties[6], out error) ? error : 0;
                    }

                    //Fifth line - either a 0 - marks the end of the delta alignment or they are deltas
                    while (line != null && !line.StartsWith("*", StringComparison.OrdinalIgnoreCase))
                    {
                        long temp;
                        if (long.TryParse(line, out temp))
                        {
                            deltaAlignment.Deltas.Add(temp);
                        }
                        // Read next line.
                        line = streamReader.ReadLine();

                        // Continue reading if blank line found.
                        while (skipBlankLine && line != null && string.IsNullOrEmpty(line))
                        {
                            line = streamReader.ReadLine();
                        }
                    }

                    deltaAlignments.Add(deltaAlignment);
                    //Read the next line
                    line = streamReader.ReadLine();
                }
                while (line != null);
            }
            return deltaAlignments;
        }

        /// <summary>
        /// Closes streams used.
        /// </summary>
        public void Close()
        {
            this.Filename = null;
        }

        /// <summary>
        /// Disposes the underlying stream.
        /// </summary>
        public void Dispose()
        {
            this.Close();
            GC.SuppressFinalize(this);
        }

        #region Private Methods

        /// <summary>
        /// Reads the next line skipping the blank lines
        /// </summary>
        /// <param name="streamReader"></param>
        private void ReadNextLine(StreamReader streamReader)
        {
            // Read next line.
            line = streamReader.ReadLine();
            string message = string.Empty;

            // Continue reading if blank line found.
            while (line != null && string.IsNullOrEmpty(line))
            {
                line = streamReader.ReadLine();
            }

            if (line == null)
            {
                message = string.Format(
                    CultureInfo.InvariantCulture,
                    Resources.InvalidSymbolInString,
                    string.Empty);
                throw new FileFormatException(message);
            }
        }
        #endregion
        #endregion
    }
}
