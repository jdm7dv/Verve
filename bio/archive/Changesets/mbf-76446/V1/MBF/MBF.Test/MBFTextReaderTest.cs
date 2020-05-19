// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.IO;
using System.Text;

using MBF.IO;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// Unit tests for MBFTextReader.
    /// </summary>
    [TestFixture]
    public class MBFTextReaderTest
    {
        // This file is sufficient for all of the tests.
        private const string testFileFullName = @"testdata\GenBank\U49845.gbk";

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static MBFTextReaderTest()
        {
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Test all of the constructors and the Filename property.
        /// </summary>
        [Test]
        public void TestMBFTextReaderConstructors()
        {
            string firstLineHeader = "LOCUS";

            // MBFTextReader(string) should read first line and set the Filename property.
            using (MBFTextReader mbfReader = new MBFTextReader(testFileFullName))
            {
                Assert.AreEqual(firstLineHeader, mbfReader.LineHeader);
                Assert.AreEqual(testFileFullName, mbfReader.FileName);
            }

            // MBFTextReader(Stream) should read first line and set the Filename property to null.
            using (FileStream stream = new FileStream(testFileFullName, FileMode.Open, FileAccess.Read))
            {
                using (MBFTextReader mbfReader = new MBFTextReader(stream))
                {
                    Assert.AreEqual(firstLineHeader, mbfReader.LineHeader);
                    Assert.IsNull(mbfReader.FileName);
                }
            }

            // MBFTextReader(TextReader) should read first line and set the Filename property to null.
            using (StreamReader reader = new StreamReader(testFileFullName))
            {
                using (MBFTextReader mbfReader = new MBFTextReader(reader))
                {
                    Assert.AreEqual(firstLineHeader, mbfReader.LineHeader);
                    Assert.IsNull(mbfReader.FileName);
                }
            }

            // Data indent specifies the number of chars that are considered the line header.
            int dataIndent = 2;
            firstLineHeader = firstLineHeader.Substring(0, 2);

            // MBFTextReader(string) should read first line, update the data indent, and set the
            // Filename property.
            using (MBFTextReader mbfReader = new MBFTextReader(testFileFullName, dataIndent))
            {
                Assert.AreEqual(firstLineHeader, mbfReader.LineHeader);
                Assert.AreEqual(testFileFullName, mbfReader.FileName);
            }

            // MBFTextReader(Stream, int) should read first line, update the data indent, and set
            // the Filename property to null.
            using (FileStream stream = new FileStream(testFileFullName, FileMode.Open, FileAccess.Read))
            {
                using (MBFTextReader mbfReader = new MBFTextReader(stream, dataIndent))
                {
                    Assert.AreEqual(firstLineHeader, mbfReader.LineHeader);
                    Assert.IsNull(mbfReader.FileName);
                }
            }

            // MBFTextReader(TextReader) should read first line, update the data indent, and set
            // the Filename property to null.
            using (StreamReader reader = new StreamReader(testFileFullName))
            {
                using (MBFTextReader mbfReader = new MBFTextReader(reader, dataIndent))
                {
                    Assert.AreEqual(firstLineHeader, mbfReader.LineHeader);
                    Assert.IsNull(mbfReader.FileName);
                }
            }
        }


        /// <summary>
        /// Test all of the methods and properties except for the Filename property.
        /// </summary>
        /// <remarks>
        /// These are all tested together, because they're functionality is somewhat sequential.
        /// </remarks>
        [Test]
        public void TestMBFTextReaderCoreFunctionality()
        {
            using (MBFTextReader mbfReader = new MBFTextReader(testFileFullName))
            {
                // Test line access members.
                Assert.IsTrue(mbfReader.HasLines);
                Assert.AreEqual("LOCUS       SCU49845     5028 bp    DNA             PLN       21-JUN-1999",
                    mbfReader.Line);
                Assert.IsTrue(mbfReader.LineHasHeader);
                Assert.AreEqual("LOCUS", mbfReader.LineHeader);
                Assert.IsTrue(mbfReader.LineHasData);
                Assert.AreEqual("SCU49845     5028 bp    DNA             PLN       21-JUN-1999",
                    mbfReader.LineData);
                Assert.AreEqual("NA  ", mbfReader.GetLineField(38, 41));

                // Test reading lines and line number tracking.
                for (int i = 1; i < 6; i++)
                {
                    mbfReader.GoToNextLine();
                }
                Assert.AreEqual(7, mbfReader.LineNumber);
                Assert.AreEqual("KEYWORDS", mbfReader.LineHeader);

                // Test switching line indent.
                mbfReader.DataIndent = 2;
                Assert.AreEqual("KE", mbfReader.LineHeader);
                Assert.AreEqual("YWORDS    .", mbfReader.LineData);

                // Test recognition of blank header and data.
                for (int i = 6; i < 8; i++)
                {
                    mbfReader.GoToNextLine();
                }
                Assert.IsFalse(mbfReader.LineHasHeader); // line starts with 2 spaces
                Assert.IsTrue(mbfReader.LineHasData);
                mbfReader.DataIndent = 37; // the line length
                Assert.IsTrue(mbfReader.LineHasHeader);
                Assert.IsFalse(mbfReader.LineHasData);
                mbfReader.DataIndent = 12; // back to standard line length

                // Test skipping sections and EOF recognition.
                mbfReader.SkipToNextSection(); // ref 1
                mbfReader.SkipToNextSection(); // ref 2
                mbfReader.SkipToNextSection(); // features
                mbfReader.SkipToNextSection(); // origin
                mbfReader.SkipToNextSection(); // "//"
                Assert.IsTrue(mbfReader.HasLines);
                mbfReader.GoToNextLine(); // EOF
                Assert.IsTrue(mbfReader.HasLines);
            }
        }
    }
}
