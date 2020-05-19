using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio.IO;
using System.Xml.Linq;
using Bio.Util.ArgumentParser;
using Bio.Registration;

namespace Rnaml.IO
{
    [Registrable(true)]
    public class RnamlFormatter : ISequenceFormatter
    {
        private string _filename;
        private XDocument _doc;

        public RnamlFormatter()
        {
        }

        public RnamlFormatter(string filename)
        {
            Open(filename);
        }

        public void Close()
        {
            if (_doc != null)
            {
                _doc.Save(_filename);
            }
        }

        public void Open(System.IO.StreamWriter outStream)
        {
            throw new NotSupportedException();
        }

        public void Open(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("filename");

            _filename = filename;
            _doc = new XDocument(new XElement("rnaml",
                new XAttribute("version", "1.1")));
        }

        public void Write(Bio.ISequence sequence)
        {
            _doc.Root.Add(new XElement("molecule",
                new XAttribute("id", sequence.ID),
                new XElement("sequence",
                    new XAttribute("length", sequence.Count),
                    new XElement("seq-data",
                        new string(sequence.Select(b => (char)b).ToArray()))
                        )));
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
