// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Test
{
    using MBF.IO;
    using MBF.IO.Fasta;
    using MBF.IO.GenBank;
    using NUnit.Framework;

    /// <summary>
    /// Test the Framework Abstraction code like SequenceParsers
    /// SequenceFormatter etc.
    /// </summary>
    [TestFixture]
    public class FrameworkAbstractionTest
    {
        /// <summary>
        /// Tests SequenceParsers.FindParserByFile by giving valid FASTA
        /// extensions as input.
        /// </summary>
        [Test]
        public void FindFastaParser()
        {
            string dummyFileName = "dummy.fasta";
            ISequenceParser parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaParser), parser);

            dummyFileName = "dummy.fa";
            parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaParser), parser);

            dummyFileName = "dummy.mpfa";
            parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaParser), parser);

            dummyFileName = "dummy.fna";
            parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaParser), parser);

            dummyFileName = "dummy.faa";
            parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaParser), parser);

            dummyFileName = "dummy.fsa";
            parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaParser), parser);

            dummyFileName = "dummy.fas";
            parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaParser), parser);
        }

        /// <summary>
        /// Tests SequenceParsers.FindParserByFile by giving valid GenBank
        /// extensions as input.
        /// </summary>
        [Test]
        public void FindGenBankParser()
        {
            string dummyFileName = "dummy.gb";
            ISequenceParser parser = SequenceParsers.FindParserByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(GenBankParser), parser);
        }

        /// <summary>
        /// Tests SequenceParsers.FindParserByFile by giving invalid
        /// file extension.
        /// </summary>
        [Test]
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
        [Test]
        public void FindFastaFromater()
        {
            string dummyFileName = "dummy.fasta";
            ISequenceFormatter formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaFormatter), formatter);

            dummyFileName = "dummy.fa";
            formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaFormatter), formatter);

            dummyFileName = "dummy.mpfa";
            formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaFormatter), formatter);

            dummyFileName = "dummy.fna";
            formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaFormatter), formatter);

            dummyFileName = "dummy.faa";
            formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaFormatter), formatter);

            dummyFileName = "dummy.fsa";
            formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaFormatter), formatter);

            dummyFileName = "dummy.fas";
            formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(FastaFormatter), formatter);
        }

        /// <summary>
        /// Tests SequenceFormatters.FindFormatterByFile by giving valid GenBank
        /// extensions as input.
        /// </summary>
        [Test]
        public void FindGenBankFromatter()
        {
            string dummyFileName = "dummy.gb";
            ISequenceFormatter formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.IsInstanceOf(typeof(GenBankFormatter), formatter);
        }

        /// <summary>
        /// Tests SequenceFormatters.FindFormatterByFile by giving invalid
        /// file extension.
        /// </summary>
        [Test]
        public void ReturnNoFormatter()
        {
            string dummyFileName = "dummy.abc";
            ISequenceFormatter formatter = SequenceFormatters.FindFormatterByFile(dummyFileName);
            Assert.AreEqual(formatter, null);
        }
    }
}
