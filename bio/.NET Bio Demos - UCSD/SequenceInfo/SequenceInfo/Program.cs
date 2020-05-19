using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio.IO;
using Bio;

namespace SequenceInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string FastQFilename = @"s_6_sequence.fastq";
            QualitativeSequence oneSequence = null;

            var parser = SequenceParsers.FindParserByFileName(FastQFilename);
            if (parser != null)
            {
                oneSequence = (QualitativeSequence) parser.Parse().First();
                parser.Close();
            }

            if (oneSequence == null)
            {
                Console.WriteLine("Could not load sequence.");
                return;
            }

            // Make a copy of it on disk.
            var formatter = SequenceFormatters.FastQ;
            formatter.Open(@"s_6_sequence.copy.fastq");
            formatter.Write(oneSequence);
            formatter.Close();

            // Dump it out
            Console.WriteLine(oneSequence);
            Console.WriteLine(oneSequence.FormatType);

            // Dump out the quality scores
            foreach (byte qs in oneSequence.QualityScores)
                Console.Write("{0:X} ", qs);
        }

        static void UseSequenceTransforms()
        {
            const string Filename = "E.coli.refseq.fasta";
            ISequence oneSequence = null;

            var parser = SequenceParsers.FindParserByFileName(Filename);
            if (parser != null)
            {
                oneSequence = parser.Parse().First();
                parser.Close();
            }

            if (oneSequence == null)
            {
                Console.WriteLine("Could not load sequence.");
                return;
            }

            Console.WriteLine("Original:");
            Console.WriteLine(oneSequence);

            Console.WriteLine("End Of Original:");
            ISequence endOfSequence = 
                oneSequence.GetSubSequence(
                    oneSequence.Count - 20, 20);
            Console.WriteLine(endOfSequence);

            ISequence reversedSequence = oneSequence.GetReversedSequence();
            Console.WriteLine("Reversed:");
            Console.WriteLine(reversedSequence);

            ISequence complementedSequence = oneSequence.GetComplementedSequence();
            Console.WriteLine("Complemented:");
            Console.WriteLine(complementedSequence);

            ISequence reverseComplementSequence = oneSequence.GetReverseComplementedSequence();
            Console.WriteLine("Reverse + Complemented Sequence");
            Console.WriteLine(reverseComplementSequence);
        }
    }
}
