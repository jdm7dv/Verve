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
using Bio.Algorithms.Alignment;
using Bio.Encoding;
using Bio.Properties;

namespace Bio.IO.Phylip
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
            using (BioTextReader bioReader = new BioTextReader(reader))
            {
                return Parse(bioReader, isReadOnly);
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
            using (BioTextReader bioReader = new BioTextReader(fileName))
            {
                return Parse(bioReader, isReadOnly);
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
            using (BioTextReader bioReader = new BioTextReader(reader))
            {
                return ParseOne(bioReader, isReadOnly);
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
            using (BioTextReader bioReader = new BioTextReader(fileName))
            {
                return ParseOne(bioReader, isReadOnly);
            }
        }
        #endregion

        #region "Protected Method(s)"
        /// <summary>
        /// Parses a list of sequences using a BioTextReader.
        /// </summary>
        /// <remarks>
        /// This method should be overridden by any parsers that need to process file-scope
        /// metadata that applies to all of the sequences in the file.
        /// </remarks>
        /// <param name="bioReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequences's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.
        /// </param>
        /// <returns>The list of parsed ISequence objects.</returns>
        protected virtual IList<ISequenceAlignment> Parse(BioTextReader bioReader, bool isReadOnly)
        {
            if (bioReader == null)
                throw new ArgumentNullException("bioReader");

            // no empty files allowed
            if (!bioReader.HasLines)
            {
                string message = Properties.Resource.IONoTextToParse;
                throw new InvalidDataException(message);
            }

            List<ISequenceAlignment> alignments = new List<ISequenceAlignment>();

            while (bioReader.HasLines)
            {
                if (string.IsNullOrEmpty(bioReader.Line.Trim()))
                {
                    bioReader.GoToNextLine();
                    continue;
                }

                alignments.Add(ParseOneWithSpecificFormat(bioReader, isReadOnly));
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
        /// <param name="bioReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.</param>
        /// <returns>A new Sequence Alignment instance containing parsed data.</returns>
        protected ISequenceAlignment ParseOneWithSpecificFormat(BioTextReader bioReader, bool isReadOnly)
        {
            if (bioReader == null)
                throw new ArgumentNullException("bioReader");

            string message = string.Empty;

            // Parse first line
            IList<string> tokens = GetTokens(bioReader.Line);
            if (2 != tokens.Count)
            {
                message = string.Format(CultureInfo.CurrentCulture, Resource.INVAILD_INPUT_FILE, this.Name);
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

            bioReader.GoToNextLine();  // Skip blank lines until we get to the first block.

            // Now that we're at the first block, one or more blank lines are the block separators, which we'll need.
            bioReader.SkipBlankLines = false;

            while (bioReader.HasLines)
            {
                if (string.IsNullOrEmpty(bioReader.Line.Trim()))
                {
                    bioReader.GoToNextLine();
                    continue;
                }

                for (int index = 0; index < sequenceCount; index++)
                {
                    if (isFirstBlock)
                    {
                        tokens = GetTokens(bioReader.Line);

                        if (1 == tokens.Count)
                        {
                            id = tokens[0].Substring(0, 10);
                            sequenceString = tokens[0].Substring(10);
                        }
                        else
                        {
                            id = tokens[0];
                            sequenceString = tokens[1];
                        }

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
                        sequence.InsertRange(sequence.Count, bioReader.Line.Trim());
                    }

                    bioReader.GoToNextLine();
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
        /// Split the line and return the tokens in the line
        /// </summary>
        /// <param name="line">Line to be split</param>
        /// <returns>Tokens in line</returns>
        private static IList<string> GetTokens(string line)
        {
            return line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Parses a single sequences using a BioTextReader.
        /// </summary>
        /// <param name="bioReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.</param>
        /// <returns>A new Sequence Alignment instance containing parsed data.</returns>
        private ISequenceAlignment ParseOne(BioTextReader bioReader, bool isReadOnly)
        {
            // no empty files allowed
            if (!bioReader.HasLines)
            {
                string message = Properties.Resource.IONoTextToParse;
                throw new InvalidDataException(message);
            }

            // do the actual parsing
            return ParseOneWithSpecificFormat(bioReader, isReadOnly);
        }
        #endregion
    }
}
