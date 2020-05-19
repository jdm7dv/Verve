// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MBF.IO;
using MBF.IO.FastQ;
using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// FASTA format parser and formatter.
    /// </summary>
    [TestFixture]
    public class FastQTest
    {
        /// <summary>
        /// Verifies that the parser doesn't throw an exception when calling ParseOne on a file
        /// containing more than one sequence.
        /// </summary>
        [Test]
        public void TestFastQWhenParsingOneOfMany()
        {
            // parse
            ISequenceParser parser = new FastQParser();
            string filepath = @"testdata\FASTQ\SRR002012_5.fastq";
            ISequence seq = parser.ParseOne(filepath);

            FastQParser fqParser = new FastQParser();
            fqParser.AutoDetectFastQFormat = false;
            fqParser.FastqType = FastQFormatType.Sanger;
            IQualitativeSequence qualSeq = fqParser.ParseOne(filepath);
        }

        /// <summary>
        /// Verifies that the parser parses string in FastQ format.
        /// </summary>
        [Test]
        public void TestFastQWhenParsingString()
        {
            string str =
                @"@SRR002012.1 Oct4:5:1:871:340 length=26
GGCGCACTTACACCCTACATCCATTG
+SRR002012.1 Oct4:5:1:871:340 length=26
IIIIG1?II;IIIII1IIII1%.I7I
";

            StringReader sr = new StringReader(str);
            ISequenceParser parser = new FastQParser();
            ISequence seq = parser.ParseOne(sr);
            sr = new StringReader(str);
            FastQParser fqParser = new FastQParser();
            fqParser.AutoDetectFastQFormat = false;
            fqParser.FastqType = FastQFormatType.Sanger;
            IQualitativeSequence qualSeq = fqParser.ParseOne(sr);
        }

        /// <summary>
        /// Test formatter by reading the multisequence FASTQ file SRR002012_5.fastq,
        /// writing it back to disk using the formatter, then reading the new file
        /// and confirming that the data has been written correctly.
        /// </summary>
        [Test]
        public void FastQFormatter()
        {
            string filepathOriginal = @"testdata\FASTQ\SRR002012_5.fastq";
            Assert.IsTrue(File.Exists(filepathOriginal));

            FastQParser parser = new FastQParser();
            FastQFormatter formatter = new FastQFormatter();

            // Read the original file
            IList<IQualitativeSequence> seqsOriginal = null;
            parser = new FastQParser();
            seqsOriginal = parser.Parse(filepathOriginal);
            Assert.IsNotNull(seqsOriginal);

            // Use the formatter to write the original sequences to a temp file
            string filepathTmp = Path.GetTempFileName();
            using (TextWriter writer = new StreamWriter(filepathTmp))
            {
                foreach (IQualitativeSequence s in seqsOriginal)
                {
                    formatter.Format(s, writer);
                }
            }

            // Read the new file, then compare the sequences
            IList<IQualitativeSequence> seqsNew = null;
            parser = new FastQParser();
            seqsNew = parser.Parse(filepathTmp);
            Assert.IsNotNull(seqsNew);

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
                string orgscores = ASCIIEncoding.ASCII.GetString(seqsOriginal[i].Scores);
                string newscores = ASCIIEncoding.ASCII.GetString(seqsNew[i].Scores);
                Assert.AreEqual(orgSeq, newSeq);
                Assert.AreEqual(orgscores, newscores);
            }

            // Passed all the tests, delete the tmp file. If we failed an Assert,
            // the tmp file will still be there in case we need it for debugging.
            File.Delete(filepathTmp);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestFormatingString()
        {
              string str =
                @"@SRR002012.1 Oct4:5:1:871:340 length=26
GGCGCACTTACACCCTACATCCATTG
+SRR002012.1 Oct4:5:1:871:340 length=26
IIIIG1?II;IIIII1IIII1%.I7I
";

            StringReader sr = new StringReader(str);

            FastQParser parser = new FastQParser();
            IQualitativeSequence seq = parser.ParseOne(sr);
            FastQFormatter formatter = new FastQFormatter();
            string formatterStr =  formatter.FormatString(seq);
            Assert.AreEqual(str,formatterStr);
        }

        /// <summary>
        /// Verify that the parser can read many files without exceptions.
        /// </summary>
        [Test]
        public void FastQParserForManyFiles()
        {
            string path = @"testdata\FASTQ";
            Assert.IsTrue(Directory.Exists(path));
            int count = 0;
            FastQParser parser = new FastQParser();
            FastQFormatter formatter = new FastQFormatter();
            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo fi in di.GetFiles("*.fastq"))
            {
                using (StreamReader reader = File.OpenText(fi.FullName))
                {
                    foreach (IQualitativeSequence seq in parser.Parse(reader))
                    {
                        count++;
                    }
                }
            }

            Assert.IsTrue(count >= 3);
        }

        /// <summary>
        /// Tests the name,description and file extension property of 
        /// Fasta formatter and parser.
        /// </summary>
        [Test]
        public void FastQProperties()
        {
            ISequenceParser parser = new FastQParser();

            Assert.AreEqual(parser.Name, Properties.Resource.FASTQ_NAME);
            Assert.AreEqual(parser.Description, Properties.Resource.FASTQPARSER_DESCRIPTION);
            Assert.AreEqual(parser.FileTypes, Properties.Resource.FASTQ_FILEEXTENSION);

            ISequenceFormatter formatter = new FastQFormatter();

            Assert.AreEqual(formatter.Name, Properties.Resource.FASTQ_NAME);
            Assert.AreEqual(formatter.Description, Properties.Resource.FASTQFORMATTER_DESCRIPTION);
            Assert.AreEqual(formatter.FileTypes, Properties.Resource.FASTQ_FILEEXTENSION);
        }

        /// <summary>
        /// Tests the default FastQ format type.
        /// </summary>
        [Test]
        public void TestDefaultFastQFormatType()
        {
            QualitativeSequence qualSeq = new QualitativeSequence(Alphabets.DNA);
            Assert.AreEqual(qualSeq.Type, FastQFormatType.Illumina);

            string str =
                @"@Seq1
GGCGCACTTACACCCTACATCCATTG
+
ABCDEFGHIJKLMNOPQRSTUVWZYZ
";

            StringReader sr = new StringReader(str);

            FastQParser parser = new FastQParser();
            IQualitativeSequence seq = parser.ParseOne(sr);
            Assert.AreEqual(seq.Type, FastQFormatType.Illumina);
        }

        /// <summary>
        /// Tests all editable scenarios with y-axis and x-axis DV enabled.
        /// </summary>
        [Test]
        public void AllEditableScenarios()
        {
            string filepathOriginal = @"TestData\FastQ\SRR002012_5.fastq";
            FastQParser fastqParser = new FastQParser();
            IList<IQualitativeSequence> qualitativeSequences;
            string[] expectedSequences = new string[] { 
                "GGCGCACTTACACCCTACATCCATTG",
                "GTCTGCATTATCTACCAGCACTTCCC",
                "GCTGTCTTCCCGCTGTTTTATCCCCC",
                "GTAGTTTACCTGTTCATATGTTTCTG",
                "GGAAGGAAGAGGCTAGCCCAGCCTTT"
            };

            fastqParser.EnforceDataVirtualization = true;
            fastqParser.AutoDetectFastQFormat = false;
            fastqParser.FastqType = FastQFormatType.Sanger;

            qualitativeSequences = fastqParser.Parse(filepathOriginal, true);
            int sequenceCount = qualitativeSequences.Count;

            for(int i=0; i<sequenceCount; i++)
            {
                QualitativeSequence actualSequence = qualitativeSequences[i] as QualitativeSequence;
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

                actualSequence.ReplaceRange(5, "GG");
                expectedSequences[i] = expectedSequences[i].Remove(5, 2);
                expectedSequences[i] = expectedSequences[i].Insert(5, "GG");
                Assert.AreEqual(expectedSequences[i], actualSequence.ToString());

                actualSequence.Insert(10, item);
                expectedSequences[i] = expectedSequences[i].Insert(10, item.Symbol.ToString());
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
