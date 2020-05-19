using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using Bio;
using Bio.IO;
using Bio.IO.FastA;

namespace AlphabetSoup
{
    class Program
    {
        static void Main(string[] args)
        {
            string filename = @"c:\users\mark\desktop\data\Simple_GenBank_DNA.genbank";

            //ISequenceParser parser =
            //    SequenceParsers.FindParserByFileName(filename);

            using (ISequenceParser parser = 
                new FastAParser(filename))
            {
                if (parser != null)
                {
                    IEnumerable<ISequence> sequences = parser.Parse();
                    foreach (ISequence sequence in sequences)
                    {
                        Console.WriteLine("{0}: {1}", sequence.ID, sequence);
                    }
                }
            }
        }

        static void UseOurOwnSequences()
        {
            ISequenceParser parser;
            Sequence newSequence = 
                new Sequence(Alphabets.RNA, 
                    "ACGU---GG--UU--CC--AA");
            Console.WriteLine(
                newSequence.ConvertToString(0, newSequence.Count));

            foreach (byte symbol in newSequence)
            {
                Console.WriteLine(symbol);
            }
        }

        static void TestAlphabet()
        {
            //IAlphabet dnaAlphabet = Alphabets.DNA;

            foreach (var alphabet in Alphabets.All)
            {
                Console.WriteLine("{0} - {1}, HasGaps={2}, HasTerminators={3}, HasAmbiguities={4}",
                    alphabet.Name, alphabet, alphabet.HasGaps, alphabet.HasTerminations, alphabet.HasAmbiguity);

                foreach (byte symbol in alphabet)
                    Console.WriteLine("\t{0} - {1}",
                        (char) symbol, symbol);
            }

        }
    }
}
