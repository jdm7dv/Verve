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
using MBF.IO.Newick;
using MBF.Phylogenetics;
using MBF.Util.Logging;
using NUnit.Framework;
using MBF.Util;

namespace MBF.Test
{
    /// <summary>
    /// Newick format parser and formatter.
    /// </summary>
    [TestFixture]
    public class NewickTest
    {
        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static NewickTest()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Simple string test.
        /// </summary>
        [Test]
        public void NewickStringTest()
        {
            StringBuilder input = new StringBuilder("(A:0.1,B:0.2,(C:0.3,D:0.4):0.5);");
            // parse
            Perf.Start("Parsing...");
            NewickParser parser = new NewickParser();

            Tree phylogeneticTree = parser.Parse(input);
            Perf.End();

            Perf.Start("Formatting...");
            NewickFormatter formatter = new NewickFormatter();

            string output = formatter.FormatString(phylogeneticTree);
            Perf.End();

            Assert.AreEqual(input.ToString(), output);
        }



        /// <summary>
        /// Tree file test
        /// IMPORTANT NOTE: This test case may fail when the the length comes with
        /// zeros (0) at the end of decimal place. Example, 45.56000
        /// </summary>
        [Test]
        public void NewickFileTest()
        {
            // parse
            Perf.Start("Parsing...");
            string filepath = @"TestData\PhylogeneticTree\tree.txt";

            NewickParser parser = new NewickParser();
            Tree phylogeneticTree = parser.Parse(filepath);
            Perf.End();

            string outpath = @"TestData\PhylogeneticTree\out.txt";
            Perf.Start("Formatting...");
            NewickFormatter formatter = new NewickFormatter();

            formatter.Format(phylogeneticTree, outpath);
            Perf.End();
            Assert.AreEqual(true, FileCompare(filepath, outpath));
        }


        // This method accepts two strings the represent two files to 
        // compare. A return value of 0 indicates that the contents of the files
        // are the same. A return value of any other value indicates that the 
        // files are not the same.
        private bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            // Open the two files.
            fs1 = new FileStream(file1, FileMode.Open);
            fs2 = new FileStream(file2, FileMode.Open);

            // Check the file sizes. If they are not the same, the files 
            // are not the same.
            if (fs1.Length != fs2.Length)
            {
                // Close the file
                fs1.Close();
                fs2.Close();

                // Return false to indicate files are different
                return false;
            }

            // Read and compare a byte from each file until either a
            // non-matching set of bytes is found or until the end of
            // file1 is reached.
            do
            {
                // Read one byte from each file.
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1));

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is 
            // equal to "file2byte" at this point only if the files are 
            // the same.
            return ((file1byte - file2byte) == 0);
        }


    }
}
