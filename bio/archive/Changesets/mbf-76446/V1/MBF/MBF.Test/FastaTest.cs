// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using MBF.IO;
using MBF.IO.Fasta;
using MBF.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// FASTA format parser and formatter.
    /// </summary>
    [TestFixture]
    public class FastaTest
    {
        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static FastaTest()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Verifies that the parser doesn't throw an exception when calling ParseOne on a file
        /// containing more than one sequence.
        /// </summary>
        [Test]
        public void TestFastaWhenParsingOneOfMany()
        {
            // parse
            ISequenceParser parser = new FastaParser();
            string filepath = @"testdata\FASTA\uniprot-dutpase.fasta";
            ISequence seq = parser.ParseOne(filepath);
        }

        /// <summary>
        /// Parse sample FASTA file 186972391.fasta and verify that it is read correctly.
        /// </summary>
        [Test]
        public void FastaFor186972391()
        {
            string expectedSequence =
                "IFYEPVEILGYDNKSSLVLVKRLITRMYQQKSLISSLNDSNQNEFWGHKNSFSSHFSSQMVSEGFGVILE" +
                "IPFSSRLVSSLEEKRIPKSQNLRSIHSIFPFLEDKLSHLNYVSDLLIPHPIHLEILVQILQCWIKDVPSL" +
                "HLLRLFFHEYHNLNSLITLNKSIYVFSKRKKRFFGFLHNSYVYECEYLFLFIRKKSSYLRSISSGVFLER" +
                "THFYGKIKYLLVVCCNSFQRILWFLKDTFIHYVRYQGKAIMASKGTLILMKKWKFHLVNFWQSYFHFWFQ" +
                "PYRINIKQLPNYSFSFLGYFSSVRKNPLVVRNQMLENSFLINTLTQKLDTIVPAISLIGSLSKAQFCTVL" +
                "GHPISKPIWTDLSDSDILDRFCRICRNLCRYHSGSSKKQVLYRIKYIFRLSCARTLARKHKSTVRTFMRR" +
                "LGSGFLEEFFLEEE";

            string filepath = @"testdata\FASTA\186972391.fasta";
            Assert.IsTrue(File.Exists(filepath));

            IList<ISequence> seqs = null;
            FastaParser parser = new FastaParser();
            using (StreamReader reader = File.OpenText(filepath))
            {
                seqs = parser.Parse(reader);
            }
            Assert.IsNotNull(seqs);
            Assert.AreEqual(1, seqs.Count);
            Sequence seq = (Sequence)seqs[0];
            Assert.IsNotNull(seq);
            Assert.AreEqual(expectedSequence, seq.ToString());
            Assert.AreEqual(expectedSequence.Length, seq.EncodedValues.Length);
            Assert.IsNotNull(seq.Alphabet);
            Assert.AreEqual(seq.Alphabet.Name, "Protein");

            Assert.AreEqual("gi|186972391|gb|ACC99454.1| maturase K [Scaphosepalum rapax]", seq.ID);

            // Try it again with ParseOne, from reader and from filename
            using (StreamReader reader = File.OpenText(filepath))
            {
                seq = (Sequence)parser.ParseOne(reader);
            }
            Assert.IsNotNull(seq);
            Assert.AreEqual(expectedSequence, seq.ToString());
            Assert.AreEqual(expectedSequence.Length, seq.EncodedValues.Length);
            Assert.IsNotNull(seq.Alphabet);
            Assert.AreEqual(seq.Alphabet.Name, "Protein");

            Assert.AreEqual("gi|186972391|gb|ACC99454.1| maturase K [Scaphosepalum rapax]", seq.ID);

            seq = (Sequence)parser.ParseOne(filepath);

            Assert.IsNotNull(seq);
            Assert.AreEqual(expectedSequence, seq.ToString());
            Assert.AreEqual(expectedSequence.Length, seq.EncodedValues.Length);
            Assert.IsNotNull(seq.Alphabet);
            Assert.AreEqual(seq.Alphabet.Name, "Protein");

            Assert.AreEqual("gi|186972391|gb|ACC99454.1| maturase K [Scaphosepalum rapax]", seq.ID);
        }

        /// <summary>
        /// Parse multisequence FASTA file uniprot-dutpase.fasta and verify that it is read correctly.
        /// </summary>
        [Test]
        public void FastaForUniprotDutpase()
        {
            int expectedSequenceCount = 2015;
            string filepath = @"testdata\FASTA\uniprot-dutpase.fasta";
            Assert.IsTrue(File.Exists(filepath));

            List<string> headers = new List<string>();
            List<string> sequences = new List<string>();
            using (StreamReader reader = File.OpenText(filepath))
            {
                string line = null;
                StringBuilder s = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(">"))
                    {
                        if (s != null)
                        {
                            sequences.Add(s.ToString());
                            s = null;
                        }
                        headers.Add(line);
                    }
                    else
                    {
                        if (s == null)
                        {
                            s = new StringBuilder();
                        }
                        s.Append(line);
                    }
                }
                if (s != null)
                {
                    sequences.Add(s.ToString());
                    s = null;
                }
            }
            Assert.AreEqual(expectedSequenceCount, headers.Count);
            Assert.AreEqual(expectedSequenceCount, sequences.Count);

            IList<ISequence> seqs = null;
            FastaParser parser = new FastaParser();
            using (StreamReader reader = File.OpenText(filepath))
            {
                seqs = parser.Parse(reader);
            }
            Assert.IsNotNull(seqs);
            Assert.AreEqual(expectedSequenceCount, seqs.Count);

            for (int i = 0; i < expectedSequenceCount; i++)
            {
                Sequence seq = (Sequence)seqs[i];
                Assert.IsNotNull(seq);
                Assert.AreEqual(sequences[i], seq.ToString());
                Assert.AreEqual(sequences[i].Length, seq.EncodedValues.Length);
                Assert.AreEqual(headers[i].Substring(1), seq.ID);
            }
        }

        /// <summary>
        /// Verify that the parser can read many files without exceptions.
        /// </summary>
        [Test]
        public void FastaParserForManyFiles()
        {
            string path = @"testdata\FASTA";
            Assert.IsTrue(Directory.Exists(path));
            int count = 0;
            FastaParser parser = new FastaParser();
            FastaFormatter formatter = new FastaFormatter();
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo fi in di.GetFiles("*.fasta"))
            {
                using (StreamReader reader = File.OpenText(fi.FullName))
                {
                    ApplicationLog.WriteLine("parsing file {0}...", fi.FullName);
                    foreach (Sequence seq in parser.Parse(reader))
                    {
                        ApplicationLog.Write(formatter.FormatString(seq));
                        count++;
                    }
                }
                ApplicationLog.WriteLine("parsing file {0}...", fi.FullName);
            }
            Assert.IsTrue(count >= 3);
        }

        /// <summary>
        /// Test formatter by reading the multisequence FASTA file NC_005213.ffn,
        /// writing it back to disk using the formatter, then reading the new file
        /// and confirming that the data has been written correctly.
        /// </summary>
        [Test]
        public void FastaFormatter()
        {
            // Test with FASTA file from Simon

            string filepathOriginal = @"testdata\FASTA\NC_005213.ffn";
            Assert.IsTrue(File.Exists(filepathOriginal));

            FastaParser parser = new FastaParser();
            FastaFormatter formatter = new FastaFormatter();

            // Read the original file
            IList<ISequence> seqsOriginal = null;
            parser = new FastaParser();
            seqsOriginal = parser.Parse(filepathOriginal);
            Assert.IsNotNull(seqsOriginal);

            // Use the formatter to write the original sequences to a temp file
            string filepathTmp = Path.GetTempFileName();
            using (TextWriter writer = new StreamWriter(filepathTmp))
            {
                foreach (Sequence s in seqsOriginal)
                {
                    formatter.Format(s, writer);
                }
            }

            // Read the new file, then compare the sequences
            IList<ISequence> seqsNew = null;
            parser = new FastaParser();
            seqsNew = parser.Parse(filepathTmp);
            Assert.IsNotNull(seqsOriginal);

            // Now compare the sequences.
            int countOriginal = seqsOriginal.Count();
            int countNew = seqsNew.Count();
            Assert.AreEqual(countOriginal, countNew);

            int i;
            for (i = 0; i < countOriginal; i++)
            {
                Assert.AreEqual(seqsOriginal[i].ID, seqsNew[i].ID);
                string orgSeq = seqsOriginal[i].ToString();
                string newSeq = seqsNew[i].ToString();
                Assert.AreEqual(orgSeq, newSeq);
            }
            // Passed all the tests, delete the tmp file. If we failed an Assert,
            // the tmp file will still be there in case we need it for debugging.
            File.Delete(filepathTmp);
        }

        /// <summary>
        /// Try to read text that does not contain an entry (no line beginning with '>')
        /// </summary>
        [Test]
        public void BadContent()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("AAAA");
            FastaParser parser = new FastaParser();
            bool exception = false;
            IList<ISequence> sequences = null;
            try
            {
                sequences = parser.Parse(new StringReader(s.ToString()));
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.AreEqual(ex.Message, "Bad input in file [Fasta]");
            }
            Assert.IsTrue(exception);
        }

        /// <summary>
        /// Tests the name,description and file extension property of 
        /// Fasta formatter and parser.
        /// </summary>
        [Test]
        public void FastaProperties()
        {
            ISequenceParser parser = new FastaParser();

            Assert.AreEqual(parser.Name, Properties.Resource.FASTA_NAME);
            Assert.AreEqual(parser.Description, Properties.Resource.FASTAPARSER_DESCRIPTION);
            Assert.AreEqual(parser.FileTypes, Properties.Resource.FASTA_FILEEXTENSION);

            ISequenceFormatter formatter = new FastaFormatter();

            Assert.AreEqual(formatter.Name, Properties.Resource.FASTA_NAME);
            Assert.AreEqual(formatter.Description, Properties.Resource.FASTAFORMATTER_DESCRIPTION);
            Assert.AreEqual(formatter.FileTypes, Properties.Resource.FASTA_FILEEXTENSION);

        }

        /// <summary>
        /// Tests all editable scenarios with y-axis and x-axis DV enabled.
        /// </summary>
        [Test]
        public void AllEditableScenarios()
        {
            string filepathOriginal = @"TestData\Fasta\5_sequences.fasta";
            Assert.IsTrue(File.Exists(filepathOriginal));

            FastaParser fastaParser = new FastaParser();
            IList<ISequence> sequences;
            string[] expectedSequences = new string[] { 
                "KRIPKSQNLRSIHSIFPFLEDKLSHLN",
                "LNIPSLITLNKSIYVFSKRKKRLSGFLHN",
                "HEAGAWGHEEHEAGAWGHEEHEAGAWGHEE",
                "PAWHEAEPAWHEAEPAWHEAEPAWHEAEPAWHEAE",
                "CGGUCCCGCGGUCCCGCGGUCCCGCGGUCCCG"
            };

            fastaParser.EnforceDataVirtualization = true;

            sequences = fastaParser.Parse(filepathOriginal, true);
            int sequenceCount = sequences.Count;

            for (int i = 0; i < sequenceCount; i++)
            {
                Sequence actualSequence = sequences[i] as Sequence;
                actualSequence.IsReadOnly = false;
                ISequenceItem item = actualSequence[1];

                actualSequence.Add(item);
                expectedSequences[i] += item.Symbol;
                Assert.AreEqual(expectedSequences[i], actualSequence.ToString());

                actualSequence.Remove(item);
                int indexOfItem = expectedSequences[i].IndexOf(item.Symbol);
                expectedSequences[i] = expectedSequences[i].Remove(indexOfItem, 1);
                Assert.AreEqual(expectedSequences[i], actualSequence.ToString());

                actualSequence.RemoveAt(0);
                expectedSequences[i] = expectedSequences[i].Remove(0, 1);
                Assert.AreEqual(expectedSequences[i], actualSequence.ToString());

                actualSequence.RemoveRange(2, 5);
                expectedSequences[i] = expectedSequences[i].Remove(2, 5);
                Assert.AreEqual(expectedSequences[i], actualSequence.ToString());

                actualSequence.Replace(0, 'C');
                expectedSequences[i] = expectedSequences[i].Remove(0, 1);
                expectedSequences[i] = expectedSequences[i].Insert(0, "C");
                Assert.AreEqual(expectedSequences[i], actualSequence.ToString());

                actualSequence.ReplaceRange(3, "GG");
                expectedSequences[i] = expectedSequences[i].Remove(3, 2);
                expectedSequences[i] = expectedSequences[i].Insert(3, "GG");
                Assert.AreEqual(expectedSequences[i], actualSequence.ToString());

                actualSequence.Insert(3, item);
                expectedSequences[i] = expectedSequences[i].Insert(3, item.Symbol.ToString());
                Assert.AreEqual(expectedSequences[i], actualSequence.ToString());

                actualSequence.InsertRange(2, "CC");
                expectedSequences[i] = expectedSequences[i].Insert(2, "CC");
                Assert.AreEqual(expectedSequences[i], actualSequence.ToString());

                bool actualContainsValue = actualSequence.Contains(actualSequence[3]);
                bool expectedContainsValue = expectedSequences[i].Contains(actualSequence[3].Symbol.ToString());
                Assert.AreEqual(actualContainsValue, expectedContainsValue);
            }
        }
    }
}
