// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * FastaBvtTestCases.cs
 * 
 *   This file contains the Fasta - Parsers and Formatters Bvt test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Bio.TestAutomation.Util;
using Bio;
using Bio.IO;
using Bio.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bio.IO.FastA;

namespace Bio.TestAutomation.IO.Fasta
{
    /// <summary>
    /// FASTA Bvt parser and formatter Test case implementation.
    /// </summary>
    [TestClass]
    public class FastaBvtTestCases
    {

        #region Global Variables

        Utility utilityObj = new Utility(@"TestUtils\TestsConfig.xml");
        ASCIIEncoding encodingObj = new ASCIIEncoding();

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static FastaBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("bio.automation.log");
            }
        }

        #endregion Constructor

        #region Fasta Parser Bvt Test cases

        /// <summary>
        /// Parse a valid FastA file (Small size sequence less than 35 kb) and convert the 
        /// same to one sequence using Parse(file-name) method and validate with the 
        /// expected sequence.
        /// Input : FastA File
        /// Validation : Expected sequence, Sequence Length, Sequence Alphabet, Sequence ID.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)"), TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void FastaParserValidateParse()
        {
            // Gets the expected sequence from the Xml
            string filePath = utilityObj.xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file            
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "FastA Parser BVT: File Exists in the Path '{0}'.", filePath));

            IEnumerable<ISequence> seqsList = null;
            using (FastAParser parser = new FastAParser(filePath))
            {
                parser.Alphabet = Alphabets.Protein;
                seqsList = parser.Parse();

                Assert.IsNotNull(seqsList);
                Assert.AreEqual(1, seqsList.Count());

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastA Parser BVT: Number of Sequences found are '{0}'.",
                    seqsList.Count()));

                string expectedSequence = utilityObj.xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                   Constants.ExpectedSequenceNode);

                Sequence seq = (Sequence)seqsList.ElementAt(0);
                char[] seqString = seqsList.ElementAt(0).Select(a => (char)a).ToArray();
                string newSequence = new string(seqString);

                Assert.IsNotNull(seq);
                Assert.AreEqual(expectedSequence, newSequence);

                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastA Parser BVT: The FASTA sequence '{0}' validation after Parse() is found to be as expected.",
                    newSequence));

                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastA Parser BVT: The FASTA sequence '{0}' validation after Parse() is found to be as expected.",
                    newSequence));

                byte[] tmpEncodedSeq = new byte[seq.Count];
                (seq as IEnumerable<byte>).ToArray().CopyTo(tmpEncodedSeq, 0);

                Assert.AreEqual(expectedSequence.Length, tmpEncodedSeq.Length);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastA Parser BVT: The FASTA Length sequence '{0}' is as expected.",
                    expectedSequence.Length));

                Assert.IsNotNull(seq.Alphabet);
                Assert.AreEqual(seq.Alphabet.Name.ToLower(CultureInfo.CurrentCulture),
                    utilityObj.xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                    Constants.AlphabetNameNode).ToLower(CultureInfo.CurrentCulture));
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastA Parser BVT: The Sequence Alphabet is '{0}' and is as expected.",
                    seq.Alphabet.Name));

                Assert.AreEqual(utilityObj.xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                    Constants.SequenceIdNode), seq.ID);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastA Parser BVT: The Sequence ID is '{0}' and is as expected.", seq.ID));
                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "FastA Parser BVT: The Sequence ID is '{0}' and is as expected.", seq.ID));
            }
        }

        #endregion Fasta Parser BVT Test cases

        #region Fasta Formatter Bvt Test cases

        /// <summary>
        /// Format a valid Single Sequence (Small size sequence less than 35 kb) to a 
        /// FastA file Write() method with Sequence and Writer as parameter
        /// and validate the same.
        /// Input : FastA Sequence
        /// Validation : Read the FastA file to which the sequence was formatted and 
        /// validate Sequence, Sequence Count
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Console.WriteLine(System.String)"), TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void FastaFormatterValidateWrite()
        {

            using (FastAFormatter formatter = new FastAFormatter(Constants.FastaTempFileName))
            {

                // Gets the actual sequence and the alphabet from the Xml
                string actualSequence = utilityObj.xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                    Constants.ExpectedSequenceNode);
                string alpName = utilityObj.xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                    Constants.AlphabetNameNode);

                // Logs information to the log file
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "FastA Formatter BVT: Validating with Sequence '{0}' and Alphabet '{1}'.",
                    actualSequence, alpName));
                Sequence seqOriginal = new Sequence(Utility.GetAlphabet(alpName),
                    encodingObj.GetBytes(actualSequence));
                    seqOriginal.ID = "";
                    Assert.IsNotNull(seqOriginal);

                    // Use the formatter to write the original sequences to a temp file            
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "FastA Formatter BVT: Creating the Temp file '{0}'.", Constants.FastaTempFileName));

                    formatter.Write(seqOriginal);
                    formatter.Close();

                    IEnumerable<ISequence> seqsNew = null;

                    // Read the new file, then compare the sequences            
                    using (FastAParser parser = new FastAParser(Constants.FastaTempFileName))
                    {
                        parser.Alphabet = Alphabets.Protein;
                        seqsNew = parser.Parse();
                        char[] seqString = seqsNew.ElementAt(0).Select(a => (char)a).ToArray();
                        string newSequence = new string(seqString);
                        Assert.IsNotNull(seqsNew);

                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "FastA Formatter BVT: New Sequence is '{0}'.",
                            newSequence));

                        // Now compare the sequences.
                        int countNew = seqsNew.Count();
                        Assert.AreEqual(1, countNew);
                        ApplicationLog.WriteLine("The Number of sequences are matching.");
                        Assert.AreEqual(seqOriginal.ID, seqsNew.ElementAt(0).ID);
                        string orgSeq = new string(seqsNew.ElementAt(0).Select(a => (char)a).ToArray());

                        Assert.AreEqual(orgSeq, newSequence);
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "FastA Formatter BVT: The FASTA sequences '{0}' are matching with Format() method and is as expected.",
                            newSequence));

                        ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                            "FastA Formatter BVT: The FASTA sequences '{0}' are matching with Format() method.",
                            newSequence));
                    }

                // Passed all the tests, delete the tmp file. If we failed an Assert,
                // the tmp file will still be there in case we need it for debugging.
                File.Delete(Constants.FastaTempFileName);
                ApplicationLog.WriteLine("Deleted the temp file created.");
            }
        }
        #endregion Fasta Formatter Bvt Test cases
    }
}
