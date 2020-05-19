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
using MBF.Encoding;


namespace MBF.IO
{
    /// <summary>
    /// 
    /// Implements common methods for parsing one or more sparse sequences from 
    /// an XsvSparseReader. This reads sequence items from the reader and 
    /// returns a sparse sequence created for the items. Multiple sparse sequences
    /// are separated by a "comment" line that starts with the sequence prefix 
    /// character.
    /// 
    /// This also returns the optional offset position of the sequence, if 
    /// present, to support aligned sequences such as in a Contig.
    /// 
    /// This is an abstract class and extending classes will have to implement
    /// the GetSparseReader(TextReader reader) method.
    /// 
    /// This class is based on the MBF.IO.BasicSequenceParser.
    /// 
    /// </summary>
    public abstract class XsvSparseParser : ISequenceParser
    {
        #region Properties
        /// <summary>
        /// The alphabet to use for parsed ISequence objects.
        /// </summary>
        public IAlphabet Alphabet { get; set; }

        /// <summary>
        /// The encoding to use for parsed ISequence objects. 
        /// </summary>
        public IEncoding Encoding { get; set; }

        /// <summary>
        /// Gets the name of the parser. 
        /// </summary>
        public string Name
        {
            get { return "XsvSparseParser"; }
        }

        /// <summary>
        /// Gets the description of the parser. 
        /// </summary>
        public string Description
        {
            get { return "Parses sparse sequences from character separated value reader"; }
        }

        /// <summary>
        /// Gets the filetypes supported by the sparse parser.
        /// </summary>
        public string FileTypes
        {
            get { return "csv,tsv"; }
        }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Creates a Sparse parser with the given encoding and alphabet
        /// </summary>
        /// <param name="encoding">Encoding for the sequence items</param>
        /// <param name="alphabet">Alphabet for the sequence items</param>
        protected XsvSparseParser(IEncoding encoding, IAlphabet alphabet)
        {
            this.Encoding = encoding;
            this.Alphabet = alphabet;
        }

        #endregion Constructor

        #region Methods to implement ISequenceParser

        /// <summary>
        /// Creates a text reader from the file name and calls Parse(TextReader reader).
        /// </summary>
        /// <param name="filename">name of file containing the Xsv formatted sparse sequences</param>
        /// <returns>A list of sparse sequences that were present in the file.</returns>
        public IList<ISequence> Parse(string filename)
        {
            return Parse(filename, true);
        }

        /// <summary>
        /// Creates a text reader from the file name and calls Parse(TextReader reader).
        /// </summary>
        /// <param name="filename">name of file containing the Xsv formatted sparse sequences</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>A list of sparse sequences that were present in the file.</returns>
        public IList<ISequence> Parse(string filename, bool isReadOnly)
        {
            // Check input arguments
            if (filename == null) 
            {
                throw new ArgumentNullException("filename", "Filename of file to parse sequence from cannot be null");
            }

            using (StreamReader reader = new StreamReader(filename))
            {
                return Parse(reader, isReadOnly);
            }
        }


        /// <summary>
        /// Creates a XsvSparseReader for the given text text reader by calling
        /// GetSparseReader() and parses a list of sparse sequences from the reader.
        /// </summary>
        /// <param name="reader">The text reader that has zero or more sparse sequences
        /// formatted using the XsvSparseFormatter.</param>
        /// <returns>A list of sparse sequences that were present in the reader.</returns>
        public IList<ISequence> Parse(TextReader reader)
        {
            return Parse(reader, true);
        }

        /// <summary>
        /// Creates a XsvSparseReader for the given text text reader by calling
        /// GetSparseReader() and parses a list of sparse sequences from the reader.
        /// </summary>
        /// <param name="reader">The text reader that has zero or more sparse sequences
        /// formatted using the XsvSparseFormatter.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>A list of sparse sequences that were present in the reader.</returns>
        public IList<ISequence> Parse(TextReader reader, bool isReadOnly)
        {
            // Check input arguments
            if (reader == null) 
            {
                throw new ArgumentNullException("reader", "Text reader to read sequences from cannot be null");
            }

            XsvSparseReader sparseReader = GetSparseReader(reader);
            List<ISequence> sequenceList = new List<ISequence>();
            while (sparseReader.HasLines)
                sequenceList.Add(ParseOne(sparseReader, isReadOnly));
            return sequenceList;
        }


        /// <summary>
        /// Creates a text reader from the file name and calls ParseOne(TextReader reader).
        /// </summary>
        /// <param name="filename">name of file containing the Xsv formatted sparse sequences</param>
        /// <returns>A list of sparse sequences that were present in the file.</returns>
        public ISequence ParseOne(string filename)
        {
            return ParseOne(filename, true);
        }

        /// <summary>
        /// Creates a text reader from the file name and calls ParseOne(TextReader reader).
        /// </summary>
        /// <param name="filename">name of file containing the Xsv formatted sparse sequences</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>A list of sparse sequences that were present in the file.</returns>
        public ISequence ParseOne(string filename, bool isReadOnly)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                return ParseOne(reader, isReadOnly);
            }
        }

        /// <summary>
        /// Creates a XsvSparseReader for the given text text reader by calling
        /// GetSparseReader() and parses the first sparse sequence from the reader.
        /// </summary>
        /// <param name="reader">The text reader that has zero or more sparse sequences
        /// formatted using the XsvSparseFormatter.</param>
        /// <returns>The first sparse sequence that was present in the reader.</returns>
        public ISequence ParseOne(TextReader reader)
        {
            return ParseOne(reader, true);
        }

        /// <summary>
        /// Creates a XsvSparseReader for the given text text reader by calling
        /// GetSparseReader() and parses the first sparse sequence from the reader.
        /// </summary>
        /// <param name="reader">The text reader that has zero or more sparse sequences
        /// formatted using the XsvSparseFormatter.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The first sparse sequence that was present in the reader.</returns>
        public ISequence ParseOne(TextReader reader, bool isReadOnly)
        {
            // Check input arguments
            if (reader == null) 
            {
                throw new ArgumentNullException("reader", "Text reader to read sequence from cannot be null");
            }

            XsvSparseReader sparseReader = GetSparseReader(reader);
            return ParseOne(sparseReader, isReadOnly);
        }

        #endregion

        #region Methods introduced by XsvSparseParser
        /// <summary>
        /// The common ParseOne method called for parsing sequences from Xsv files. 
        /// This assumes that that the first line has been read into the XsvSparseReader 
        /// (i.e. GoToNextLine() has been called). This adds the offset position present in 
        /// the sequence start line to each position value in the sequence item.
        /// e.g. the following returns a sparse sequence with ID 'Test sequence' of length 100 
        /// with A at position 32 (25+7) and G at position 57 (50+7).
        /// # 7, 100, Test sequence
        /// 25,A
        /// 50,G
        /// 
        /// </summary>
        /// <param name="sparseReader">The Xsv sparse reader that can read the sparse sequences.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The first sequence present starting from the 
        /// current position in the reader as a SparseSequence. The sparse sequence has the ID present in the 
        /// sequence start line, and its length equals the count present in that line. 
        /// Null if EOF has been reached. Throws an exception if the current position did 
        /// not have the sequence start line with the sequence prefix ID character.
        /// </returns>
        protected ISequence ParseOne(XsvSparseReader sparseReader, bool isReadOnly)
        {
            // Check input arguments
            if (sparseReader == null) 
            {
                throw new ArgumentNullException("sparseReader", "Sparse reader to read sequence from cannot be null");
            }

            if (!sparseReader.HasLines) return null;

            if (sparseReader.SkipCommentLines || !sparseReader.HasCommentLine)
                throw new InvalidDataException(Properties.Resource.XsvOffsetNotFound);

            // create a new sparse sequence
            SparseSequence sequence = new SparseSequence(Alphabet);

            // read the sequence ID, count and offset
            sequence.ID = sparseReader.GetSequenceId();
            sequence.Count = sparseReader.GetSequenceCount();
            int offset = sparseReader.GetSequenceOffset();

            // go to first sequence item
            sparseReader.GoToNextLine();

            while (sparseReader.HasLines && !sparseReader.HasCommentLine)
            {
                // add offset to position
                int position = int.Parse(sparseReader.Fields[0], CultureInfo.InvariantCulture) + offset;
                char symbol = sparseReader.Fields[1][0];
                if (sequence.Count <= position)
                    sequence.Count = position + 1;
                sequence[position] = Alphabet.LookupBySymbol(symbol);
                sparseReader.GoToNextLine();
            }

            sequence.IsReadOnly = isReadOnly;
            return sequence;
        }

        /// <summary>
        /// The ParseOne method called for parsing sequences from Xsv files,
        /// and returning the offset separately. This can be used by aligned/assembled 
        /// sparse sequences.
        /// This assumes that that the first line has been read into the XsvSparseReader 
        /// (i.e. GoToNextLine() has been called). This does NOT add the offset position 
        /// present in the sequence start line to each position value in the sequence item. 
        /// Instead, it returns the offset in the out parameter.
        /// e.g. the following returns a sparse sequence with ID 'Test sequence' of length 100 
        /// with A at position 25 and G at position 50, 
        /// and the out offset value as 7.
        /// # 7, 100, Test sequence
        /// 25,A
        /// 50,G
        /// 
        /// </summary>
        /// <param name="sparseReader">The Xsv sparse reader that can read the sparse sequences.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The first sequence present starting from the 
        /// current position in the reader, as a SparseSequence. The sparse sequence has the ID present in the 
        /// sequence start line, and its length equals the count present in that line. 
        /// Throws an exception if EOF has been reached or the current position did 
        /// not have the sequence start line with the sequence prefix ID character.
        /// Also returns the offset value present in the sequence start line.
        /// </returns>
        protected Tuple<ISequence, int> ParseOneWithOffset(XsvSparseReader sparseReader, bool isReadOnly)
        {
            // Check input arguments
            if (sparseReader == null) 
            {
                throw new ArgumentNullException("sparseReader", "Sparse reader to read sequence from cannot be null");
            }

            int offset = -1;
            if (!sparseReader.HasLines) return null;

            if (sparseReader.SkipCommentLines || !sparseReader.HasCommentLine)
                throw new InvalidDataException(Properties.Resource.XsvOffsetNotFound);

            // create a new sparse sequence
            SparseSequence sequence = new SparseSequence(Alphabet);

            // read the sequence ID and offset
            sequence.ID = sparseReader.GetSequenceId();
            sequence.Count = sparseReader.GetSequenceCount();
            offset = sparseReader.GetSequenceOffset();
            sparseReader.GoToNextLine();

            while (sparseReader.HasLines && !sparseReader.HasCommentLine)
            {
                int position = int.Parse(sparseReader.Fields[0], CultureInfo.InvariantCulture);
                char symbol = sparseReader.Fields[1][0];
                if (sequence.Count <= position)
                    sequence.Count = position + 1;
                sequence[position] = Alphabet.LookupBySymbol(symbol);
                sparseReader.GoToNextLine();
            }

            sequence.IsReadOnly = isReadOnly;
            return Tuple.Create((ISequence)sequence, offset);
        }


        /// <summary>
        /// Abstract method that returns a XSV sparse reader for the given text reader.
        /// The Parse*() methods use the returned sparse reader to parse sparse seqeunces.
        /// Classes extending from this base class must implement this method to match
        /// the XSV format of the passed text reader.
        /// </summary>
        /// <param name="reader">Text reader to create a sparse reader for</param>
        /// <returns>Sparse parser that can parse the text reader 
        /// as a sequence of items.</returns>
        protected abstract XsvSparseReader GetSparseReader(TextReader reader);

        #endregion Methods
    }
}