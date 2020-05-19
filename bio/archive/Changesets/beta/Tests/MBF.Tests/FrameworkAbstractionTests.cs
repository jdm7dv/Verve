// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Tests
{
    using MBF.IO;
    using MBF.IO.Fasta;
    using MBF.IO.GenBank;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test the Framework Abstraction code like SequenceParsers
    /// SequenceFormatter etc.
    /// </summary>
    [TestClass]
    public class FrameworkAbstractionTests
    {
        /// <summary>
        /// Tests SequenceParsers.FindParserByFile by giving valid FASTA
        /// extensions as input.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void FindFastaParser()
        {
            string dummyFileName = "dummy.fasta";
            ISequenceParser parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOfType(parser, typeof(FastaParser));

            dummyFileName = "dummy.fa";
            parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOfType(parser, typeof(FastaParser));

            dummyFileName = "dummy.mpfa";
            parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOfType(parser, typeof(FastaParser));

            dummyFileName = "dummy.fna";
            parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOfType(parser, typeof(FastaParser));

            dummyFileName = "dummy.faa";
            parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOfType(parser, typeof(FastaParser));

            dummyFileName = "dummy.fsa";
            parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOfType(parser, typeof(FastaParser));

            dummyFileName = "dummy.fas";
            parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOfType(parser, typeof(FastaParser));
        }

        /// <summary>
        /// Tests SequenceParsers.FindParserByFile by giving valid GenBank
        /// extensions as input.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void FindGenBankParser()
        {
            string dummyFileName = "dummy.gb";
            ISequenceParser parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOfType(parser, typeof(GenBankParser));
        }

        /// <summary>
        /// Tests SequenceParsers.FindParserByFile by giving invalid
        /// file extension.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ReturnNoParser()
        {
            string dummyFileName = "dummy.abc";
            ISequenceParser parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.AreEqual(parser, null);
        }

        /// <summary>
        /// Tests SequenceFormatters.FindFormatterByFile by giving valid FASTA
        /// extensions as input.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void FindFastaFromater()
        {
            string dummyFileName = "dummy.fasta";
            ISequenceFormatter formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOfType(formatter, typeof(FastaFormatter));

            dummyFileName = "dummy.fa";
            formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOfType(formatter, typeof(FastaFormatter));

            dummyFileName = "dummy.mpfa";
            formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOfType(formatter, typeof(FastaFormatter));

            dummyFileName = "dummy.fna";
            formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOfType(formatter, typeof(FastaFormatter));

            dummyFileName = "dummy.faa";
            formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOfType(formatter, typeof(FastaFormatter));

            dummyFileName = "dummy.fsa";
            formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOfType(formatter, typeof(FastaFormatter));

            dummyFileName = "dummy.fas";
            formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOfType(formatter, typeof(FastaFormatter));
        }

        /// <summary>
        /// Tests SequenceFormatters.FindFormatterByFile by giving valid GenBank
        /// extensions as input.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void FindGenBankFromatter()
        {
            string dummyFileName = "dummy.gb";
            ISequenceFormatter formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOfType(formatter, typeof(GenBankFormatter));
        }

        /// <summary>
        /// Tests SequenceFormatters.FindFormatterByFile by giving invalid
        /// file extension.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ReturnNoFormatter()
        {
            string dummyFileName = "dummy.abc";
            ISequenceFormatter formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.AreEqual(formatter, null);
        }
    }
}
