using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Bio.Registration;

namespace Bio.IO
{
    /// <summary>
    /// A simple parser to read the .BIO format.
    /// </summary>
    [Registrable(true)]
    public class BioParser : ISequenceParser
    {
        private string _filename;

        /// <summary>
        /// Default public constructor.
        /// </summary>
        public BioParser()
        {
        }

        /// <summary>
        /// Constructor that takes a filename
        /// </summary>
        /// <param name="filename">File to open</param>
        public BioParser(string filename)
        {
            Open(filename);
        }

        /// <summary>
        /// Description of the parser
        /// </summary>
        public string Description
        {
            get { return "BioParser Sample - From Lab4 of the .NET Bio course"; }
        }

        /// <summary>
        /// File types supported by this parser.
        /// </summary>
        public string SupportedFileTypes
        {
            get { return ".bio"; }
        }

        /// <summary>
        /// Name of the parser.
        /// </summary>
        public string Name
        {
            get { return "BioParser"; }
        }

        /// <summary>
        /// Alphabet to use
        /// </summary>
        public IAlphabet Alphabet { get; set; }

        /// <summary>
        /// Used to close the file
        /// </summary>
        public void Close()
        {
            _filename = null;
        }

        /// <summary>
        /// Used to open the file
        /// </summary>
        /// <param name="filename"></param>
        public void Open(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("Missing filename");

            if (_filename != null && _filename != filename)
                throw new InvalidOperationException("Parser already has a file open.");

            if (!File.Exists(filename))
                throw new FileNotFoundException("File does not exist.");

            _filename = filename;
        }

        /// <summary>
        /// Parses out the file.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ISequence> Parse()
        {
            return Parse(new StreamReader(_filename));
        }

        /// <summary>
        /// Parses out the file.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ISequence> Parse(StreamReader reader)
        {
            using (reader)
            {
                // Read the first non-blank line
                string line = ReadLine(reader);
                if (line == null || !line.StartsWith("="))
                    yield break;

                do
                {
                    // Get the name of the sequence.
                    string id = line.Substring(1);

                    // Look for Metadata
                    var metadata = new Dictionary<string, string>();
                    while ((line = ReadLine(reader)) != null)
                    {
                        if (line.StartsWith(":"))
                        {
                            string[] keyValue = line.Split(new[] {':'});
                            metadata.Add(keyValue[1], string.Join(":", keyValue.Skip(2)));
                        }
                        else break;
                    }

                    // Now read the data.
                    if (line == null)
                        yield break;

                    if (!line.StartsWith("|"))
                        throw new FormatException("Missing Sequence Data");

                    int count = 0;
                    byte[] data = new byte[line.Length-1];

                    while (line != null && line.StartsWith("|"))
                    {
                        int newDataSize = line.Length - 1;

                        // Not enough space - increase our array size.
                        if (newDataSize + count > data.Length)
                            Array.Resize(ref data, newDataSize + count);

                        // Add the bytes - skip the first byte
                        Array.Copy(Encoding.ASCII.GetBytes(line),
                                   1, data, count, newDataSize);

                        count += newDataSize;
                        line = ReadLine(reader);
                    }

                    // If we have not established the alphabet for this file, do so now.
                    if (Alphabet == null)
                    {
                        // Try DNA, RNA and then finally Protein.
                        Alphabet = DnaAlphabet.Instance;
                        if (!Alphabet.ValidateSequence(data, 0, count))
                        {
                            Alphabet = RnaAlphabet.Instance;
                            if (!Alphabet.ValidateSequence(data, 0, count))
                            {
                                Alphabet = ProteinAlphabet.Instance;
                                if (!Alphabet.ValidateSequence(data, 0, count))
                                    throw new FormatException("Failed to identify proper alphabet for symbols.");
                            }
                        }
                    }

                    // Create the sequence
                    Sequence sequence = new Sequence(Alphabet, data, false) {ID = id};

                    // Add the metadata to the sequence
                    foreach (var kvp in metadata)
                        sequence.Metadata.Add(kvp.Key, kvp.Value);

                    // Return it as part of our enumerable.
                    yield return sequence;
                } 
                while (line != null && line.StartsWith("="));
            }
        }

        /// <summary>
        /// Reads a single line from the data file
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static string ReadLine(StreamReader reader)
        {
            string line;

            // Read lines until we hit a sequence marker or EOF (null)
            do
            {
                line = reader.ReadLine();

            } while (line != null 
                && (string.IsNullOrWhiteSpace(line) || line.StartsWith("!")));

            return line;
        }

        /// <summary>
        /// Called to dispose the parser
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}
