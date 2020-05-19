// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.IO;

using MBF.IO;

using NUnit.Framework;

namespace MBF.Test
{
    /// <summary>
    /// Unit tests for MBFStreamReader.
    /// </summary>
    [TestFixture]
    class MBFStreamReaderTest
    {
        /// <summary>
        /// Test all of the constructors and the Filename property.
        /// </summary>
        [Test]
        public void TestMBFTextReaderConstructors()
        {
            string testFileFullName = @"testdata\Fasta\uniprot-dutpase.fasta";

            using (StreamReader stream = new StreamReader(testFileFullName))
            {
                using (MBFStreamReader mbfReader = new MBFStreamReader(testFileFullName))
                {
                    Assert.AreEqual(testFileFullName, mbfReader.FileName);
                    Assert.AreEqual(stream.ReadLine(), mbfReader.Line);
                }
            }

            // MBFStreamReader(string) should read first line and set the Filename property.
            using (MBFStreamReader mbfReader = new MBFStreamReader(testFileFullName))
            {
                Assert.AreEqual(testFileFullName, mbfReader.FileName);
            }

            using (Stream stream = new FileStream(testFileFullName, FileMode.Open, FileAccess.Read))
            {
                using (MBFStreamReader mbfReader = new MBFStreamReader(testFileFullName))
                {
                    Assert.AreEqual(testFileFullName, mbfReader.FileName);
                }
            }

            using (MBFStreamReader mbfReader = new MBFStreamReader(testFileFullName))
            {
                Assert.AreEqual(testFileFullName, mbfReader.FileName);
            }
        }

        /// <summary>
        /// Tests all the public methods 
        /// </summary>
        /// <remarks>
        /// These are all tested together, because they're functionality is somewhat sequential.
        /// </remarks>
        [Test]
        public void TestMBFTextReaderCoreFunctionality()
        {
            string testFileFullName = @"testdata\Fasta\5_sequences.fasta";

            using (StreamReader streamReader = new StreamReader(testFileFullName))
            {
                using (MBFStreamReader mbfReader = new MBFStreamReader(testFileFullName))
                {
                    //Test line access members.
                    Assert.IsTrue(mbfReader.HasLines);

                    // Test line reads
                    string streamLine = streamReader.ReadLine();
                    Assert.AreEqual(streamLine, mbfReader.Line);

                    // Test getting of line fields
                    Assert.AreEqual(streamLine.Substring(26, 10), mbfReader.GetLineField(27, 36));
                    Assert.AreEqual(streamLine.Substring(14), mbfReader.GetLineField(15));
                    
                    // Test moving to next line
                    mbfReader.GoToNextLine();
                    Assert.AreEqual(streamReader.ReadLine(), mbfReader.Line);

                    char[] streamBuffer = new char[10];
                    char[] bioBuffer;

                    // Test seeking to a position in the stream
                    streamReader.DiscardBufferedData();
                    streamReader.BaseStream.Seek(100, SeekOrigin.Begin);
                    mbfReader.Seek(100, SeekOrigin.Begin);
                    Assert.AreEqual(streamReader.BaseStream.Position, mbfReader.Position);

                    // Test character reading
                    streamReader.ReadBlock(streamBuffer, 0, 10);
                    bioBuffer = mbfReader.ReadChars(100, 10);
                    Assert.AreEqual(streamBuffer, bioBuffer);

                    // Test disposal
                    mbfReader.Dispose();
                    Assert.IsFalse(mbfReader.CanRead);
                }
            }
        }
    }
}
