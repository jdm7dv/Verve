// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using MBF.Algorithms.Alignment;
using MBF.IO;
using MBF.IO.SAM;
using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// Test SAM format parser and formatter.
    /// </summary>
    [TestFixture]
    public class SAMTest
    {
        /// <summary>
        /// Test the SAM Parser.
        /// </summary>
        [Test]
        public void TestParser()
        {
            string filePath = @"TestData\SAM\SeqAlignment1.sam";
            ISequenceAlignmentParser parser = new SAMParser();
            IList<ISequenceAlignment> alignments= parser.Parse(filePath);
            Assert.IsTrue(alignments != null);
            Assert.AreEqual(alignments.Count, 1);
            Assert.AreEqual(alignments[0].AlignedSequences.Count, 2);
        }

        /// <summary>
        /// Test the SAM Formatter.
        /// </summary>
        [Test]
        public void TestFormatter()
        {
            string filePath = @"TestData\SAM\SeqAlignment1.sam";
            string outputfilePath = "samtest.sam";
            ISequenceAlignmentParser parser = new SAMParser();
            SAMFormatter formatter = new SAMFormatter();
            IList<ISequenceAlignment> alignments =  parser.Parse(filePath) ;

            Assert.IsTrue(alignments != null);
            Assert.AreEqual(alignments.Count, 1);
            Assert.AreEqual(alignments[0].AlignedSequences.Count, 2);

            formatter.Format(alignments[0],outputfilePath);

            alignments = parser.Parse(outputfilePath) ;

            Assert.IsTrue(alignments != null);
            Assert.AreEqual(alignments.Count, 1);
            Assert.AreEqual(alignments[0].AlignedSequences.Count, 2);
        }

        /// <summary>
        /// Tests the name,description and file extension property of 
        /// SAM formatter and parser.
        /// </summary>
        [Test]
        public void SAMProperties()
        {
            ISequenceAlignmentParser parser = new SAMParser();

            Assert.AreEqual(parser.Name, Properties.Resource.SAM_NAME);
            Assert.AreEqual(parser.Description, Properties.Resource.SAMPARSER_DESCRIPTION);
            Assert.AreEqual(parser.FileTypes, Properties.Resource.SAM_FILEEXTENSION);

            ISequenceAlignmentFormatter formatter = new SAMFormatter();

            Assert.AreEqual(formatter.Name, Properties.Resource.SAM_NAME);
            Assert.AreEqual(formatter.Description, Properties.Resource.SAMFORMATTER_DESCRIPTION);
            Assert.AreEqual(formatter.FileTypes, Properties.Resource.SAM_FILEEXTENSION);
        }
    }
}
