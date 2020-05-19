using System;
using System.Linq;

namespace DisplaySequences
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read them in
            using (var parser = new BioParser("sample.bio"))
            {
                var sequences = parser.Parse();
                foreach (var seq in sequences)
                    Console.WriteLine("ID={0}, Alphabet={1}, Data={2}",
                                      seq.ID, seq.Alphabet.Name,
                                      new string(seq.Select(b => (char) b).ToArray()));

                // Write them out.
                using (BioFormatter formatter = new BioFormatter("testout.bio"))
                {
                    foreach (var seq in sequences)
                        formatter.Write(seq);
                }
            }
        }
    }
}
