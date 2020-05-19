// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SequenceParserFormatterBvtTestCases.cs
 * 
 *   This file contains the SequenceParserFormatter - Parsers & Formatter Bvt test cases.
 * 
***************************************************************************/

using System;

using Bio.IO;
using Bio.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bio.TestAutomation.IO
{
    /// <summary>
    /// SequenceParserFormatter Bvt parser Test case implementation.
    /// </summary>
    [TestClass]
    public class SequenceParserFormatterBvtTestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SequenceParserFormatterBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("bio.automation.log");
            }
        }

        #endregion Constructor

        #region Parser Test cases

        /// <summary>
        /// Validate FindParserByFileName() method
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSequenceParserFindParserByFileName()
        {
            ISequenceParser fastAObj = SequenceParsers.FindParserByFileName(@"TestUtils\Small_Size.fasta");
            Assert.AreEqual("FastA", fastAObj.Name);

            ISequenceParser gbkObj = SequenceParsers.FindParserByFileName(@"TestUtils\Small_Size.gbk");
            Assert.AreEqual("GenBank", gbkObj.Name);

            ISequenceParser fastQObj = SequenceParsers.FindParserByFileName(@"TestUtils\Simple_Fastq_Sequence.fastq");
            Assert.AreEqual("FastQ", fastQObj.Name);

            ISequenceParser gffObj = SequenceParsers.FindParserByFileName(@"TestUtils\Simple_Gff_Dna.gff");
            Assert.AreEqual("GFF", gffObj.Name);

            ApplicationLog.WriteLine("Sequence Formatter BVT : Successfully validated the FindParserByFileName() method");
            Console.WriteLine("Sequence Formatter BVT : Successfully validated the FindParserByFileName() method");
        }

        #endregion Test cases

        #region Formatter Test cases

        /// <summary>
        /// Validate FindFormatterByFileName() method
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSequenceFormatterFindFormatterByFileName()
        {
            IFormatter fastAObj = SequenceFormatters.FindFormatterByFileName("test.fasta");
            Assert.AreEqual("FastA", fastAObj.Name);

            IFormatter gbkObj = SequenceFormatters.FindFormatterByFileName("test.gbk");
            Assert.AreEqual("GenBank", gbkObj.Name);

            IFormatter fastQObj = SequenceFormatters.FindFormatterByFileName("test.fastq");
            Assert.AreEqual("FastQ", fastQObj.Name);

            IFormatter gffObj = SequenceFormatters.FindFormatterByFileName("test.gff");
            Assert.AreEqual("GFF", gffObj.Name);

            ApplicationLog.WriteLine("Sequence Formatter BVT : Successfully validated the FindFormatterByFileName() method");
            Console.WriteLine("Sequence Formatter BVT : Successfully validated the FindFormatterByFileName() method");
        }

        #endregion
    }
}