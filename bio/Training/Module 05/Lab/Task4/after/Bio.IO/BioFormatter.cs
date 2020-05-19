using System;
using System.Linq;
using System.IO;
using Bio.Registration;

namespace Bio.IO
{
    /// <summary>
    /// Sample .bio Formatter
    /// </summary>
    [Registrable(true)]
    public class BioFormatter : ISequenceFormatter
    {
        private bool _writtenHeader;
        private StreamWriter _writer;

        /// <summary>
        /// Default constructor
        /// </summary>
        public BioFormatter()
        {
        }

        /// <summary>
        /// Open the file
        /// </summary>
        public BioFormatter(string filename)
        {
            Open(filename);
        }

        /// <summary>
        /// Name of the formatter
        /// </summary>
        public string Name
        {
            get { return "BioFormatter"; }
        }

        /// <summary>
        /// Description of the formatter
        /// </summary>
        public string Description
        {
            get { return "BioFormatter Test Formatter"; }
        }

        /// <summary>
        /// File types supported
        /// </summary>
        public string SupportedFileTypes
        {
            get { return ".bio"; }
        }

        /// <summary>
        /// Open the file
        /// </summary>
        /// <param name="filename"></param>
        public void Open(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("Missing filename");

            Open(new StreamWriter(filename, false));
        }

        /// <summary>
        /// Assign the open stream
        /// </summary>
        /// <param name="outStream"></param>
        public void Open(StreamWriter outStream)
        {
            if (_writer != null)
                throw new InvalidOperationException("Formatter already has a file open.");

            _writer = outStream;
            _writtenHeader = false;
        }

        /// <summary>
        /// Close the writer
        /// </summary>
        public void Close()
        {
            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }
        }

        /// <summary>
        /// This is called to format a single sequence to our output file.
        /// </summary>
        /// <param name="sequence">Sequence to write out</param>
        public void Write(ISequence sequence)
        {
            if (_writer == null)
                throw new InvalidOperationException("Formatter has not been opened.");

            // Write the header (comment)
            if (!_writtenHeader)
            {
               _writer.WriteLine("! Created At {0}", DateTime.Now);
                _writtenHeader = true;
            }

            // Write the name block
            _writer.WriteLine("={0}", sequence.ID);
            
            // Write out all the additional metadata
            foreach (var kvp in sequence.Metadata)
                _writer.WriteLine(":{0}:{1}", kvp.Key, kvp.Value);

            // Write the data - in 80 character blocks.
            string data = new string(sequence.Select(b => (char)b).ToArray());
            int pos = 0;
            while (pos < data.Length)
            {
                int len = Math.Min(79, data.Length-pos);
                string line = data.Substring(pos, len);

                _writer.Write('|');
                _writer.WriteLine(line);
                
                pos += len;
            }

            // Add a line separator and flush the writer.
            _writer.WriteLine();
            _writer.Flush();
        }

        /// <summary>
        /// Disposes the formatter - closes the file.
        /// </summary>
        public void Dispose()
        {
            Close();
        }
    }
}
