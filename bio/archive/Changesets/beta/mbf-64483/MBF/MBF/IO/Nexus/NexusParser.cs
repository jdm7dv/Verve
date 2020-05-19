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

namespace MBF.IO.Nexus
{
    /// <summary>
    /// A NexusParser reads from a source of text that is formatted according 
    /// to the NexusParser flat file specification, and converts the data to 
    /// in-memory ISequenceAlignment objects.  For advanced users, the ability 
    /// to select an encoding for the internal memory representation is provided. 
    /// There is also a default encoding for each alphabet that may be encountered.
    /// </summary>
    public class NexusParser : ISequenceAlignmentParser
    {
        #region "Private Field(s)"
        /// <summary>
        /// Basic Sequence Alignment Parser that contains all the common methods required.
        /// </summary>
        private CommonSequenceParser _basicParser;
        #endregion

        #region "Constructor(s)"
        /// <summary>
        /// Initializes a new instance of the NexusParser class.
        /// Default constructor chooses default encoding based on alphabet.
        /// </summary>
        public NexusParser()
        {
            _basicParser = new CommonSequenceParser();
        }

        /// <summary>
        /// Initializes a new instance of the NexusParser class.
        /// Constructor for setting the encoding.
        /// </summary>
        /// <param name="encoding">The encoding to use for parsed ISequence objects.</param>
        public NexusParser(IEncoding encoding)
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
            get { return Resource.NEXUS_NAME; }
        }

        /// <summary>
        /// Gets the description of the sequence alignment parser being
        /// implemented. This is intended to give the
        /// developer some information of the parser.
        /// </summary>
        public string Description
        {
            get { return Resource.NEXUSPARSER_DESCRIPTION; }
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
            get { return Resource.NEXUS_FILEEXTENSION; }
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
            {
                throw new ArgumentNullException("mbfReader");
            }

            // no empty files allowed
            if (!mbfReader.HasLines)
            {
                string message = Properties.Resource.IONoTextToParse;
                throw new InvalidDataException(message);
            }

            List<ISequenceAlignment> alignments = new List<ISequenceAlignment>();

            // Parse Header, Loop through the blocks and parse
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
        /// Parses a single Nexus text from a reader into a sequence.
        /// </summary>
        /// <param name="mbfReader">A reader for a biological sequence text.</param>
        /// <param name="isReadOnly">
        /// Flag to indicate whether the resulting sequence alignment should be in readonly mode or not.
        /// If this flag is set to true then the resulting sequence's isReadOnly property 
        /// will be set to true, otherwise it will be set to false.</param>
        /// <returns>A new Sequence instance containing parsed data.</returns>
        protected ISequenceAlignment ParseOneWithSpecificFormat(MBFTextReader mbfReader, bool isReadOnly)
        {
            if (mbfReader == null)
            {
                throw new ArgumentNullException("mbfReader");
            }

            ParseHeader(mbfReader);

            string message = string.Empty;
            ISequenceAlignment sequenceAlignment = new SequenceAlignment();
            sequenceAlignment.AlignedSequences.Add(new AlignedSequence());
            IList<string> ids = null;
            bool isInBlock = true;

            if (mbfReader.Line.StartsWith("begin", StringComparison.OrdinalIgnoreCase))
            {
                while (mbfReader.HasLines && isInBlock)
                {
                    if (string.IsNullOrEmpty(mbfReader.Line.Trim()))
                    {
                        mbfReader.GoToNextLine();
                        continue;
                    }

                    string blockName = GetTokens(mbfReader.Line)[1];

                    switch (blockName.ToUpper(CultureInfo.InvariantCulture))
                    {
                        case "TAXA":
                        case "TAXA;":
                            // This block contains the count of sequence & title of each sequence
                            ids = (IList<string>)ParseTaxaBlock(mbfReader);

                            break;

                        case "CHARACTERS":
                        case "CHARACTERS;":
                            // Block contains sequences
                            Dictionary<string, string> dataSet = ParseCharacterBlock(mbfReader, ids);

                            IAlphabet alignmentAlphabet = null;
                            string data = string.Empty;

                            foreach (string ID in ids)
                            {
                                IAlphabet alphabet = Alphabet;
                                Sequence sequence = null;
                                data = dataSet[ID];

                                if (null == alphabet)
                                {
                                    alphabet = _basicParser.IdentifyAlphabet(alphabet, data);

                                    if (null == alphabet)
                                    {
                                        message = string.Format(
                                                CultureInfo.InvariantCulture,
                                                Resource.InvalidSymbolInString,
                                                data);
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
                                                message = string.Format(
                                                        CultureInfo.InvariantCulture,
                                                        Properties.Resource.SequenceAlphabetMismatch);
                                                throw new InvalidDataException(message);
                                            }
                                        }
                                    }
                                }

                                if (Encoding == null)
                                {
                                    sequence = new Sequence(alphabet, data);
                                }
                                else
                                {
                                    sequence = new Sequence(alphabet, Encoding, data);
                                }

                                sequence.IsReadOnly = isReadOnly;
                                sequence.ID = ID;
                                sequenceAlignment.AlignedSequences[0].Sequences.Add(sequence);
                            }

                            break;

                        case "END":
                        case "END;":
                            // Have reached the end of block
                            isInBlock = false;

                            break;

                        default:
                            // skip this block
                            while (mbfReader.HasLines)
                            {
                                mbfReader.GoToNextLine();
                                if (0 == string.Compare(mbfReader.Line, "end;", StringComparison.OrdinalIgnoreCase))
                                {
                                    break;
                                }
                            }

                            break;
                    }

                    mbfReader.GoToNextLine();
                }
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
        /// Parse Nexus Header
        /// </summary>
        /// <param name="mbfReader">A reader for a biological sequence text.</param>
        private void ParseHeader(MBFTextReader mbfReader)
        {
            string message = string.Empty;

            if (!mbfReader.Line.StartsWith("#NEXUS", StringComparison.OrdinalIgnoreCase))
            {
                message = string.Format(CultureInfo.CurrentCulture, Resource.INVALID_INPUT_FILE, this.Name);
                throw new InvalidDataException(message);
            }

            mbfReader.GoToNextLine();  // Skip blank lines until we get to the first block.

            // Title of Alignment
            if (mbfReader.Line.Trim().StartsWith("[", StringComparison.OrdinalIgnoreCase))
            {
                while (mbfReader.HasLines)
                {
                    mbfReader.GoToNextLine();
                    if (mbfReader.Line.Trim().EndsWith("]", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }
            }

            mbfReader.GoToNextLine();

            // Now that we're at the first block, one or more blank lines are the block separators, which we'll need.
            mbfReader.SkipBlankLines = false;
        }

        /// <summary>
        /// Gets the list of sequence titles
        /// </summary>
        /// <param name="mbfReader">A reader for a biological sequence text.</param>
        /// <returns>List of sequence IDs</returns>
        private static IList<string> ParseTaxaBlock(MBFTextReader mbfReader)
        {
            bool isInTaxaBlock = true;
            string data = string.Empty;
            int sequenceCount = 0;
            IList<string> IDs = new List<string>();

            while (mbfReader.HasLines && isInTaxaBlock)
            {
                mbfReader.GoToNextLine();
                IList<string> tokens = GetTokens(mbfReader.Line);
                switch (tokens[0].ToUpper(CultureInfo.InvariantCulture))
                {
                    case "DIMENSIONS":
                        tokens[0] = string.Empty;

                        // Parse dimensions
                        // 1. Read count of sequence
                        do
                        {
                            foreach (string token in tokens)
                            {
                                data = token.Trim(new char[] { ';' });

                                if (string.IsNullOrEmpty(data))
                                {
                                    continue;
                                }

                                if (data.StartsWith("ntax=", StringComparison.OrdinalIgnoreCase))
                                {
                                    sequenceCount = Int32.Parse(data.Substring(5), CultureInfo.InvariantCulture);
                                }
                            }

                            if (mbfReader.Line.Trim().EndsWith(";", StringComparison.OrdinalIgnoreCase))
                            {
                                break;
                            }
                            else
                            {
                                mbfReader.GoToNextLine();
                                tokens = GetTokens(mbfReader.Line);
                            }
                        }
                        while (mbfReader.HasLines);

                        break;

                    case "TAXLABELS":
                    case "TAXLABELS;":
                        tokens[0] = string.Empty;

                        // Parse taxlabels
                        // 1. Read IDs of sequence
                        do
                        {
                            foreach (string token in tokens)
                            {
                                data = token.Trim(new char[] { ';' });

                                if (string.IsNullOrEmpty(data))
                                {
                                    continue;
                                }

                                IDs.Add(data);
                            }

                            if (mbfReader.Line.Trim().EndsWith(";", StringComparison.OrdinalIgnoreCase))
                            {
                                break;
                            }
                            else
                            {
                                mbfReader.GoToNextLine();
                                tokens = GetTokens(mbfReader.Line);
                            }
                        }
                        while (mbfReader.HasLines);

                        break;

                    case "END":
                    case "END;":
                        // Have reached the end of taxa block
                        isInTaxaBlock = false;
                        break;

                    default:
                        break;
                }
            }

            // Read the end line "end;"
            mbfReader.GoToNextLine();

            // Validate the count
            if (sequenceCount != IDs.Count)
            {
                throw new InvalidDataException(Properties.Resource.NtaxMismatch);
            }

            return IDs;
        }

        /// <summary>
        /// Parse the Sequence data in the block
        /// </summary>
        /// <param name="mbfReader">A reader for a biological sequence text.</param>
        /// <param name="IDs">List of sequence IDs</param>
        /// <returns>parse sequence in alignment</returns>
        private static Dictionary<string, string> ParseCharacterBlock(MBFTextReader mbfReader, IList<string> IDs)
        {
            bool isInCharactersBlock = true;
            string data = string.Empty;
            int sequenceLength = 0;
            Dictionary<string, string> dataSet = new Dictionary<string, string>();

            while (mbfReader.HasLines && isInCharactersBlock)
            {
                mbfReader.GoToNextLine();
                IList<string> tokens = GetTokens(mbfReader.Line);

                if (0 == string.Compare("DIMENSIONS", tokens[0], StringComparison.OrdinalIgnoreCase))
                {
                    tokens[0] = string.Empty;

                    // Parse dimensions
                    // 1. Length of sequence
                    do
                    {
                        foreach (string token in tokens)
                        {
                            data = token.Trim(new char[] { ';' });

                            if (string.IsNullOrEmpty(data))
                            {
                                continue;
                            }

                            if (data.StartsWith("nchar=", StringComparison.OrdinalIgnoreCase))
                            {
                                sequenceLength = Int32.Parse(data.Substring(6), CultureInfo.InvariantCulture);
                            }
                        }

                        if (mbfReader.Line.Trim().EndsWith(";", StringComparison.OrdinalIgnoreCase))
                        {
                            break;
                        }
                        else
                        {
                            mbfReader.GoToNextLine();
                            tokens = GetTokens(mbfReader.Line);
                        }
                    }
                    while (mbfReader.HasLines);
                }
                else if (0 == string.Compare("FORMAT", tokens[0], StringComparison.OrdinalIgnoreCase))
                {
                    tokens[0] = string.Empty;

                    // Parse format
                    // 1. Notation for "missing"
                    // 2. Notation for "gap"
                    // 3. Notation for "matchchar"
                    // 4. data type
                    do
                    {
                        if (mbfReader.Line.Trim().EndsWith(";", StringComparison.OrdinalIgnoreCase))
                        {
                            break;
                        }
                        else
                        {
                            mbfReader.GoToNextLine();
                            tokens = GetTokens(mbfReader.Line);
                        }
                    }
                    while (mbfReader.HasLines);
                }
                if (0 == string.Compare("MATRIX", tokens[0], StringComparison.OrdinalIgnoreCase))
                {
                    tokens[0] = string.Empty;

                    // "If available" ignore the data in square brackets []
                    while (mbfReader.HasLines)
                    {
                        if (mbfReader.Line.StartsWith("[", StringComparison.OrdinalIgnoreCase))
                        {
                            mbfReader.GoToNextLine();
                        }
                        else
                        {
                            break;
                        }
                    }

                    // Here are the alignment sequences
                    while (mbfReader.HasLines)
                    {
                        mbfReader.GoToNextLine();

                        if (string.IsNullOrEmpty(mbfReader.Line.Trim()))
                        {
                            continue;
                        }

                        tokens = GetTokens(mbfReader.Line);
                        if (tokens[0].StartsWith(";", StringComparison.OrdinalIgnoreCase))
                        {
                            isInCharactersBlock = false;
                            break;
                        }

                        if (IDs.Contains(tokens[0]))
                        {
                            data = tokens[1];

                            if (dataSet.ContainsKey(tokens[0]))
                            {
                                data = string.Concat(dataSet[tokens[0]], data);
                            }

                            dataSet[tokens[0]] = data;
                        }
                    }
                }
                else if (tokens[0].StartsWith(";", StringComparison.OrdinalIgnoreCase))
                {
                    isInCharactersBlock = false;
                }
            }

            // Read the end line "end;"
            mbfReader.GoToNextLine();

            // Validate the length of sequence
            foreach (string dataSequence in dataSet.Values)
            {
                if (dataSequence.Length != sequenceLength)
                {
                    throw new FormatException(Properties.Resource.SequenceLengthMismatch);
                }
            }

            return dataSet;
        }

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