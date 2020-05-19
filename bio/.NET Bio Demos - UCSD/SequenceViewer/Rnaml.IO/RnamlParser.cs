using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Bio;
using Bio.IO;
using Bio.Registration;

namespace Rnaml.IO
{
    [Registrable(true)]
    public class RnamlParser : ISequenceParser
    {
        private string _filename;
        public Bio.IAlphabet Alphabet { get; set; }

        public void Close()
        {
            _filename = null;
        }

        public void Open(string dataSource)
        {
            _filename = dataSource;
        }

        public IEnumerable<Bio.ISequence> Parse(System.IO.StreamReader reader)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<Bio.ISequence> Parse()
        {
            if (string.IsNullOrEmpty(_filename))
                throw new InvalidOperationException("Missing filename to open.");

            XDocument doc = XDocument.Load(_filename);

            var sequences = from molecule in doc.Root.Elements("molecule")
                            let name = molecule.Element("identity").Element("name").Value
                            let sequence = molecule.Element("sequence")
                            let start =
                                (int)sequence.Element("numbering-system").Element("numbering-range").Element("start")
                            let data = sequence.Element("seq-data")
                            select new Sequence(Alphabets.AmbiguousRNA, new Regex(@"\s*").Replace(data.Value, ""))
                            {
                                ID = name,
                            };

            return sequences;
        }

        public string Description
        {
            get { return "Rnaml is an XML data format for RNA stuff."; }
        }

        public string Name
        {
            get { return "Rnaml"; }
        }

        public string SupportedFileTypes
        {
            get { return ".xml,.rnaml,.rna"; }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
