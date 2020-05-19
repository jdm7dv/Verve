using System;
using System.IO;
using System.Linq;

namespace DisplaySequences
{
    class Program
    {
        static void Main(string[] args)
        {
            // Test the Open/Parse/Close style
            using (var parser = new BioParser("sample.bio"))
            {
                var sequences = parser.Parse();
                foreach (var seq in sequences)
                    Console.WriteLine("ID={0}, Alphabet={1}, Data={2}",
                                      seq.ID, seq.Alphabet.Name,
                                      new string(seq.Select(b => (char) b).ToArray()));
            }

            // Test the new/Parse style
            foreach (var seq in new BioParser().Parse(new StreamReader("sample.bio")))
            {
                Console.WriteLine("ID={0}, Alphabet={1}, Data={2}",
                                    seq.ID, seq.Alphabet.Name,
                                    new string(seq.Select(b => (char)b).ToArray()));
            }
        }
    }
}
