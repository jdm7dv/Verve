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
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MBF.Util;
using MBF.Util.Logging;

namespace MBF.Tests
{
    /// <summary>
    /// Test the Bio.Util class.
    /// </summary>
    [TestClass]
    public class UtilTests
    {
        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static UtilTests()
        {
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("MBF.Tests.log");
            }
        }

        /// <summary>
        /// Test the Options class.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void Options()
        {
            Options options = new Options("goo=;goo;goo=12", "goo=3", "food;", "food=", "goo");
            Options compare = new Options();
            compare.Add("goo", null);
            compare.Add("goo", null);
            compare.Add("goo", "12");
            compare.Add("goo", "3");
            compare.Add("goo", null);
            compare.Add("food", null);
            compare.Add("food", null);
            string s1 = options.ToString();
            Assert.IsNotNull(s1);
            string s2 = compare.ToString();
            Assert.IsNotNull(s2);
            Assert.AreEqual(s1, s2);
        }

        /// <summary>
        /// Tests Helper.IsFasta() method which is capable
        /// of identifying if a file extension is a
        /// valid extension for FASTA formats.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void FastaCorrectExtension()
        {
            bool output = Helper.IsFasta("temp.fasta");
            Assert.AreEqual(output, true);

            output = Helper.IsFasta("temp.fa");
            Assert.AreEqual(output, true);

            output = Helper.IsFasta("temp.mpfa");
            Assert.AreEqual(output, true);

            output = Helper.IsFasta("temp.fna");
            Assert.AreEqual(output, true);

            output = Helper.IsFasta("temp.faa");
            Assert.AreEqual(output, true);

            output = Helper.IsFasta("temp.fsa");
            Assert.AreEqual(output, true);

            output = Helper.IsFasta("temp.fas");
            Assert.AreEqual(output, true);
        }

        /// <summary>
        /// Tests Helper.IsFasta() method with 
        /// a incorrent extension,
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void FastaInCorrenctExtension()
        {
            bool output = Helper.IsFasta("temp.fas1s");
            Assert.AreEqual(output, false);
        }

    }
}
