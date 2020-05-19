using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Bio;
using System.Xml.Linq;
using Bio.IO;
//using Rnaml.IO;
using System.IO;

namespace SequenceViewer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Need source and destination filenames.");
                return;
            }

            string sourceFilename = args[0];
            string destFilename = args[1];

            ISequenceParser parser = SequenceParsers.FindParserByFileName(sourceFilename);
            if (parser == null)
            {
                parser = SequenceParsers.All.FirstOrDefault(
                    sp => sp.SupportedFileTypes.Contains(Path.GetExtension(sourceFilename)));
                if (parser == null)
                {
                    Console.WriteLine("Failed to locate parser for {0}", sourceFilename);
                    return;
                }
                parser.Open(sourceFilename);
            }

            ISequenceFormatter formatter = SequenceFormatters.FindFormatterByFileName(destFilename);
            if (formatter == null)
            {
                formatter = SequenceFormatters.All.FirstOrDefault(
                    sp => sp.SupportedFileTypes.Contains(Path.GetExtension(destFilename)));
                if (formatter == null)
                {
                    Console.WriteLine("Failed to locate formatter for {0}", destFilename);
                    return;
                }
                formatter.Open(destFilename);
            }

            foreach (var sequence in parser.Parse())
            {
                formatter.Write(sequence);
            }

            parser.Close();
            formatter.Close();
        }

#if FALSE
        static void Test()
        {
            using (ISequenceParser parser = new RnamlParser())
            {
                parser.Open(@"alignment.xml");
                
                foreach (var sequence in parser.Parse())
                {
                    Console.WriteLine(sequence.ID);
                    Console.WriteLine(sequence);
                }
            }

            IEnumerable<ISequence> otherSequences =
                SequenceParsers.FindParserByFileName(@"c:\users\mark\desktop\data\5s.a.fasta")
                    .Parse();

            using (ISequenceFormatter formatter = new RnamlFormatter(@"c:\users\mark\desktop\test.xml"))
            {
                otherSequences.ToList()
                    .ForEach(s => formatter.Write(s));
            }
        }

        //static IEnumerable<ISequence> LoadSequences(string filename)
        //{
        //    XDocument doc = XDocument.Load(filename);

        //    var sequences = from molecule in doc.Root.Elements("molecule")
        //                    let name = molecule.Element("identity").Element("name").Value
        //                    let sequence = molecule.Element("sequence")
        //                    let start =
        //                        (int) sequence.Element("numbering-system").Element("numbering-range").Element("start")
        //                    let data = sequence.Element("seq-data")
        //                    select new Sequence(Alphabets.AmbiguousRNA, new Regex(@"\s*").Replace(data.Value, ""))
        //                           {
        //                               ID = name,
        //                           };

        //    return sequences;
        //}
#endif
    }
}
