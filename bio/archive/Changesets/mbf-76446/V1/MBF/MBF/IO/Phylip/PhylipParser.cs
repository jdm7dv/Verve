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
using MBF.Algorithms.Alignment;
using MBF.Encoding;
using MBF.Properties;

namespace MBF.IO.Phylip
{
    /// <summary>
    /// A PhylipParser reads from a source of text that is formatted according 
    /// to the PhylipParser flat file specification, and converts the data to 
    /// in-memory ISequenceAlignment objects. For advanced users, the ability 
    /// to select an encoding for the internal memory representation is provided. 
    /// There is also a default encoding for each alphabet that may be encountered.
    /// </summary>
    public class PhylipParser : ISequenceAlignmentParser
    {
        #region "Private Field(s)"
        /// <summary>
        /// Basic Sequence Alignment Parser that contains all the common methods required.
        /// </summary>
        private CommonSequenceParser _basicParser;
        #endregion

        #region "Constructor(s)"
        /// <summary>
        /// Initializes a new instance of the PhylipParser class.
        /// Default constructor chooses default encoding based on alphabet.
        /// </summary>
        public PhylipParser()
        {
            _basicParser = new CommonSequenceParser();
        }

        /// <summary>
        /// Initializes a new instance of the PhylipParser class.
        /// Constructor for setting the encoding.
        /// </summary>
        /// <param name="encoding">The encoding to use for parsed ISequence objects.</param>
        public PhylipParser(IEncoding encoding)
            : this()
        {
            Encoding = encoding;
        }
        #endregion

        #region "Public Property(ies)"
        /// <summary>
        /// Gets the name of the sequence alignment parser being
        /// implemented. This is intended to give the
        /// developer some information of the parser type.
        /// </summary>
        public string Name
        {
            get { return Resource.PHYLIP_NAME; }
        }

        /// <summary>
        /// Gets the description of the sequence alignment parser being
        /// implemented. This is intended to give the
        /// developer some information of the parser.
        /// </summary>
        public string Description
        {
            get { return Resource.PHYLIPPARSER_DESCRIPTION; }
        }

        /// <summary>
        /// Gets or sets alphabet to use for sequences in parsed ISequenceAlignment objects.
        /// </summary>
        public IAlphabet Alphabet { get; set; }

        /// <summary>
        /// Gets or sets encoding to use for sequences in parsed ISequenceAlignment objects.
        /// </summary>
        public IEncoding Encoding { get; set; }

        /// <summary>
        /// Gets the file extensions that the parser implementation
        /// will support.
        /// </summary>
        public string FileTypes
        {
            get { return Resource.PHYLIP_FILEEXTENSION; }
        }
        #endregion

        #region "Public Method(s)"
        /// <summary>
        /// Parses a list of biological sequence alignment texts from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence alignment text.</param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        public IList<ISequenceAlignment> Parse(TextReader reader)
        {
            return Parse(reader, true);
        }

        /// <summary>
        /// Parses a list of biological sequence alignment texts from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence alignment text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        public IList<ISequenceAlignment> Parse(TextReader reader, bool isReadOnly)
        {
            using (MBFTextReader mbfReader = new MBFTextReader(reader))
            {
                return Parse(mbfReader, isReadOnly);
            }
        }

        /// <summary>
        /// Parses a list of biological sequence alignment texts from a file.
        /// </summary>
        /// <param name="fileName">The name of a biological sequence alignment file.</param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        public IList<ISequenceAlignment> Parse(string fileName)
        {
            return Parse(fileName, true);
        }

        /// <summary>
        /// Parses a list of biological sequence alignment texts from a file.
        /// </summary>
        /// <param name="fileName">The name of a biological sequence alignment file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequenceAlignment objects.</returns>
        public IList<ISequenceAlignment> Parse(string fileName, bool isReadOnly)
        {
            using (MBFTextReader mbfReader = new MBFTextReader(fileName))
            {
                return Parse(mbfReader, isReadOnly);
            }
        }

        /// <summary>
        /// Parses a single biological sequence alignment text from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence alignment text.</param>
        /// <returns>The parsed ISequenceAlignment object.</returns>
        public ISequenceAlignment ParseOne(TextReader reader)
        {
            return ParseOne(reader, true);
        }

        /// <summary>
        /// Parses a single biological sequence alignment text from a reader.
        /// </summary>
        /// <param name="reader">A reader for a biological sequence alignment text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed ISequenceAlignment object.</returns>
        public ISequenceAlignment ParseOne(TextReader reader, bool isReadOnly)
        {
            using (MBFTextReader mbfReader = new MBFTextReader(reader))
            {
                return ParseOne(mbfReader, isReadOnly);
            }
        }

        /// <summary>
        /// Parses a single biological sequence alignment text from a file.
        /// </summary>
        /// <param name="fileName">The name of a biological sequence alignment file.</param>
        /// <returns>The parsed ISequenceAlignment object.</returns>
        public ISequenceAlignment ParseOne(string fileName)
        {
            return ParseOne(fileName, true);
        }

        /// <summary>
        /// Parses a single biological sequence alignment text from a file.
        /// </summary>
        /// <param name="fileName">The name of a biological sequence alignment file.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The parsed ISequenceAlignment object.</returns>
        public ISequenceAlignment ParseOne(string fileName, bool isReadOnly)
        {
            using (MBFTextReader mbfReader = new MBFTextReader(fileName))
            {
                return ParseOne(mbfReader, isReadOnly);
            }
        }
        #endregion

        #region "Protected Method(s)"
        /// <summary>
        /// Parses a list of sequences using a MBFTextReader.
        /// </summary>
        /// <remarks>
        /// This method should be overridden by any parsers that need to process file-scope
        /// metadata that applies to all of the sequences in the file.
        /// </remarks>
        /// <param name="mbfReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequence objects.</returns>
        protected virtual IList<ISequenceAlignment> Parse(MBFTextReader mbfReader, bool isReadOnly)
        {
            if (mbfReader == null)
                throw new ArgumentNullException("mbfReader");

            // no empty files allowed
            if (!mbfReader.HasLines)
            {
                string message = Properties.Resource.IONoTextToParse;
                throw new InvalidDataException(message);
            }

            List<ISequenceAlignment> alignments = new List<ISequenceAlignment>();

            while (mbfReader.HasLines)
            {
                if (string.IsNullOrEmpty(mbfReader.Line.Trim()))
                {
                    mbfReader.GoToNextLine();
                    continue;
                }

                alignments.Add(ParseOneWithSpecificFormat(mbfReader, isReadOnly));
            }

            return alignments;
        }

        /// <summary>
        /// Parses a single Phylip text from a reader into a sequence.
        /// 1. First link has Count of Taxa and length of each sequence
        /// 2. Sequences
        ///     a. First ten character are ID
        ///     b. Sequence itself
        /// </summary>
        /// <param name="mbfReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.</param>
        /// <returns>A new Sequence Alignment instance containing parsed data.</returns>
        protected ISequenceAlignment ParseOneWithSpecificFormat(MBFTextReader mbfReader, bool isReadOnly)
        {
            if (mbfReader == null)
                throw new ArgumentNullException("mbfReader");

            string message = string.Empty;

            // Parse first line
            IList<string> tokens = mbfReader.Line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (2 != tokens.Count)
            {
                message = string.Format(CultureInfo.CurrentCulture, Resource.INVALID_INPUT_FILE, this.Name);
                throw new InvalidDataException(message);
            }

            bool isFirstBlock = true;
            int sequenceCount = 0;
            int sequenceLength = 0;
            IList<Sequence> data = new List<Sequence>();
            string id = string.Empty;
            string sequenceString = string.Empty;
            Sequence sequence = null;
            IAlphabet alignmentAlphabet = null;

            sequenceCount = Int32.Parse(tokens[0], CultureInfo.InvariantCulture);
            sequenceLength = Int32.Parse(tokens[1], CultureInfo.InvariantCulture);

            mbfReader.GoToNextLine();  // Skip blank lines until we get to the first block.

            // Now that we're at the first block, one or more blank lines are the block separators, which we'll need.
            mbfReader.SkipBlankLines = false;

            while (mbfReader.HasLines)
            {
                if (string.IsNullOrEmpty(mbfReader.Line.Trim()))
                {
                    mbfReader.GoToNextLine();
                    continue;
                }

                for (int index = 0; index < sequenceCount; index++)
                {
                    if (isFirstBlock)
                    {
                        // First 10 characters are sequence ID, remaining is the first block of sequence
                        // Note that both may contain whitespace, and there may be no whitespace between them.
                        if (mbfReader.Line.Length <= 10)
                        {
                            message = string.Format(CultureInfo.CurrentCulture, Resource.INVALID_INPUT_FILE, this.Name);
                            throw new Exception(message);
                        }
                        id = mbfReader.Line.Substring(0, 10).Trim();
                        sequenceString = Util.Helper.StringRemoveWhitespace(mbfReader.Line.Substring(10));

                        IAlphabet alphabet = Alphabet;
                        if (null == alphabet)
                        {
                            alphabet = _basicParser.IdentifyAlphabet(alphabet, sequenceString);

                            if (null == alphabet)
                            {
                                message = string.Format(
                                        CultureInfo.InvariantCulture,
                                        Resource.InvalidSymbolInString,
                                        sequenceString);
                                throw new InvalidDataException(message);
                            }
                            else
                            {
                                if (null == alignmentAlphabet)
                                {
                                    alignmentAlphabet = alphabet;
                                }
                                else
                                {
                                    if (alignmentAlphabet != alphabet)
                                    {
                                        message = Properties.Resource.SequenceAlphabetMismatch;
                                        throw new InvalidDataException(message);
                                    }
                                }
                            }
                        }

                        if (Encoding == null)
                        {
                            sequence = new Sequence(alphabet, sequenceString);
                        }
                        else
                        {
                            sequence = new Sequence(alphabet, Encoding, sequenceString);
                        }

                        sequence.ID = id;
                        sequence.IsReadOnly = false;
                        data.Add(sequence);
                    }
                    else
                    {
                        sequence = data[index];
                        sequence.InsertRange(sequence.Count, Util.Helper.StringRemoveWhitespace(mbfReader.Line));
                    }

                    mbfReader.GoToNextLine();
                }

                // Reset the first block flag
                isFirstBlock = false;
            }

            // Validate for the count of sequence
            if (sequenceCount != data.Count)
            {
                throw new InvalidDataException(Properties.Resource.SequenceCountMismatch);
            }

            SequenceAlignment sequenceAlignment = new SequenceAlignment();
            sequenceAlignment.AlignedSequences.Add(new AlignedSequence());

            foreach (Sequence dataSequence in data)
            {
                dataSequence.IsReadOnly = isReadOnly;

                // Validate for the count of sequence
                if (sequenceLength != dataSequence.Count)
                {
                    throw new InvalidDataException(Properties.Resource.SequenceLengthMismatch);
                }

                sequenceAlignment.AlignedSequences[0].Sequences.Add(dataSequence);
            }

            return sequenceAlignment;
        }
        #endregion

        #region "Private Method(s)"
        /// <summary>
        /// Parses a single sequences using a MBFTextReader.
        /// </summary>
        /// <param name="mbfReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.</param>
        /// <returns>A new Sequence Alignment instance containing parsed data.</returns>
        private ISequenceAlignment ParseOne(MBFTextReader mbfReader, bool isReadOnly)
        {
            // no empty files allowed
            if (!mbfReader.HasLines)
            {
                string message = Properties.Resource.IONoTextToParse;
                throw new InvalidDataException(message);
            }

            // do the actual parsing
            return ParseOneWithSpecificFormat(mbfReader, isReadOnly);
        }
        #endregion
    }
}