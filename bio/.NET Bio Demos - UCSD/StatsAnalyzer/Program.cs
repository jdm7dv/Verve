using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bio;
using Bio.IO;

namespace StatsAnalyzer
{
    class Program
    {
        private const string Filename = @"c:\users\mark\desktop\data\5s.a.fasta";

        static void Main(string[] args)
        {
            var parser = SequenceParsers.FindParserByFileName(Filename);
            if (parser == null)
            {
                Console.WriteLine("No parser found.");
                return;
            }

            List<ISequence> allSequences = parser
                .Parse()
                .ToList();

            IAlphabet alphabet = allSequences[0].Alphabet;

            long totalColums = allSequences.Min(s => s.Count);
            //byte[] data = new byte[allSequences.Count];
            for (int column = 0; column < totalColums; column++)
            {
                //    for (int row = 0; row < allSequences.Count; row++)
                //        data[row] = allSequences[row][column];
                //ISequence newSequence = new Sequence(alphabet, data);

                ISequence newSequence = new Sequence(AmbiguousRnaAlphabet.Instance,
                        allSequences
                            .Select(s => s[column]).ToArray());
                SequenceStatistics stats =
                    new SequenceStatistics(newSequence);
                var TopCount = 
                    alphabet
                    .Where(symbol => !alphabet.CheckIsGap(symbol))
                    .Select(symbol =>
                    new
                    {
                        Symbol = (char)symbol,
                        Count = stats.GetCount(symbol),
                        Frequency = stats.GetFraction(symbol)
                    })
                    .Where(tc => tc.Count > 0)
                    .OrderByDescending(c => c.Count)
                    .FirstOrDefault();

                if (TopCount != null)
                    Console.WriteLine("{0}: {1} = {2} of {3}", 
                        column, TopCount.Symbol, TopCount.Count, totalColums);
            }
        }

        static void DumpStatsForOneSequence()
        {
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

            SequenceStatistics stats = 
                new SequenceStatistics(oneSequence);

            foreach (var symbol in oneSequence.Alphabet)
            {
                Console.WriteLine("{0} = Count={1}, Fraction={2}", 
                    (char)symbol, stats.GetCount(symbol),
                    stats.GetFraction(symbol));
            }

        }
    }
}
