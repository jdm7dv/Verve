// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SequenceBVTTestCases.cs
 * 
 * This file contains the Sequence and BasicDerived Sequence BVT test cases.
 * 
******************************************************************************/

using System;
using System.IO;
using System.Collections.Generic;

using MBF.Algorithms.StringSearch;
using MBF.IO;
using MBF.IO.Fasta;
using MBF.IO.GenBank;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Sequences and BVT level validations.
    /// </summary>
    [TestClass]
    public class SequenceBvtTestCases
    {
        #region Enum

        /// <summary>
        /// SearchSequence method names
        /// </summary>
        enum PatternType
        {
            FileMatches,
            FileMatch,
        }

        #endregion

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SequenceBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Sequence Bvt TestCases

        /// <summary>
        /// Validate a creation of DNA Sequence by passing valid Single Character sequence.
        /// Input Data : Valid DNA Sequence with single character - "A".
        /// Output Data : Validation of created DNA Sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSingleCharDnaSequence()
        {

            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedSingleChar);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            Sequence createSequence = new Sequence(
                Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            Assert.AreEqual(Utility.GetAlphabet(alphabetName), createSequence.Alphabet);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence Alphabet is as expected."));

            // Logs to the NUnit GUI (Console.Out) window
            ApplicationLog.WriteLine(
                "Sequence BVT: The DNA with single character Sequence is completed successfully.");
        }

        /// <summary>
        /// Validate a creation of DNA Sequence by passing valid string.
        /// Input Data: Valid DNA sequence "ACGA".
        /// Output Data : Validation of created DNA Sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaSequence()
        {

            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedNormalString);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence ", actualSequence, " and Alphabet ", alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            Assert.AreEqual(Utility.GetAlphabet(alphabetName), createSequence.Alphabet);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence Alphabet is as expected."));

            // Logs to the NUnit GUI (Console.Out) window
            ApplicationLog.WriteLine(
                "Sequence BVT: The DNA Sequence with string is created successfully.");
        }

        /// <summary>
        /// Validate a creation of RNA Sequence by passing valid string sequence.
        /// Input Data : Valid RNA Sequence "GAUUCAAGGGCU".
        /// Output Data : Validation of created RNA sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRnaSequence()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.ExpectedNormalString);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence ", actualSequence, " and Alphabet ", alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            Assert.AreEqual(Utility.GetAlphabet(alphabetName), createSequence.Alphabet);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence Alphabet is as expected."));

            // Logs to the NUnit GUI (Console.Out) window
            ApplicationLog.WriteLine(
                "Sequence BVT: The RNA Sequence is created successfully.");
        }

        /// <summary>
        /// Validate a creation of Protein Sequence by passing valid string sequence.
        /// Input Data : Valid Protein sequece "AGTN".
        /// Output Data : Validation of created Protein sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateProteinSequence()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.ExpectedNormalString);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence ", actualSequence, " and Alphabet ", alphabetName));

            Sequence createSequence = new Sequence(
                Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            Assert.AreEqual(Utility.GetAlphabet(alphabetName), createSequence.Alphabet);

            // Logs to the NUnit GUI (Console.Out) window
            ApplicationLog.WriteLine("Sequence BVT: The Protein Sequence is created successfully.");
        }

        /// <summary>
        /// Validate a BasicDerivedSequence creation for the original sequence.
        /// Input Data : Valid DNA sequence.
        /// Output Data : Validation of Basic derived sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateBasicDerivedSequence()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedNormalString);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence ", actualSequence, " and Alphabet ", alphabetName));

            Sequence createSequence = new Sequence(
                Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            Assert.AreEqual(Utility.GetAlphabet(alphabetName), createSequence.Alphabet);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence Alphabet is as expected."));

            //Create a BasicDerived Sequence.
            BasicDerivedSequence derivedSequence = new BasicDerivedSequence(
                createSequence, false, false, -1, -1);

            //Validate the DerivedSequence
            Assert.AreEqual(createSequence.ToString(), derivedSequence.ToString());
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));
            Assert.IsNotNull(derivedSequence);


            Assert.AreEqual(derivedSequence.ToString(), actualSequence);
            Assert.AreEqual(Utility.GetAlphabet(alphabetName), derivedSequence.Alphabet);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence Alphabet is as expected."));

            // Logs to the NUnit GUI (Console.Out) window
            ApplicationLog.WriteLine(
                "Sequence BVT: The BasicDerived Sequence validation is completed successfully.");
        }

        /// <summary>
        /// Validate Reverse of the  BasicDerivedSequence.
        /// Input Data : Valid DNA Sequence "ACGA".
        /// Output Data : Revrse of the DNA Sequence "AGCA".
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateReverse()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedNormalString);
            string expectedRevSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedReverseSequence);
            string expectedDnaNormalSequenceCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.EncodedDnaNormalSequenceCount);
            string revDerSeq = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence ", actualSequence, " and Alphabet ", alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            Assert.AreEqual(Utility.GetAlphabet(alphabetName), createSequence.Alphabet);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence Alphabet is as expected."));

            // Create a BasicDerived Sequence.
            BasicDerivedSequence derivedSequence = new BasicDerivedSequence(
                createSequence, false, false, -1, -1);

            // Validate the Reverse of DerivedSequence.
            revDerSeq = derivedSequence.Reverse.ToString();
            Assert.AreEqual(revDerSeq.Length.ToString((IFormatProvider)null),
                expectedDnaNormalSequenceCount);
            Assert.AreEqual(revDerSeq, expectedRevSequence);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Reverse sequence {0} is expected", revDerSeq));

            // Logs to the NUnit GUI (Console.Out) window
            ApplicationLog.WriteLine(
                "Sequence BVT: The Reverse of the sequence is validated successfully.");
        }

        /// <summary>
        /// Validate a Sequence creation for a given FastaA file.
        /// Input Data : Valid FastaA file sequence.
        /// Output Data : Validation of FastaA file sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateFastaAFileSequence()
        {
            // Gets the expected sequence from the Xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.ExpectedSequenceNode);
            string fastAFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.FilePathNode);

            Assert.IsTrue(File.Exists(fastAFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The File exist in the Path ", fastAFilePath));

            IList<ISequence> sequence = null;
            using (FastaParser parser = new FastaParser())
            {
                // Parse a FastA file Using Parse method and convert the same to sequence.
                sequence = parser.Parse(fastAFilePath);
            }
            Assert.IsNotNull(sequence);
            Sequence fastASequence = (Sequence)sequence[0];
            Assert.IsNotNull(fastASequence);
            Assert.AreEqual(expectedSequence, fastASequence.ToString());
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The Sequence is as expected."));

            byte[] tmpEncodedSeq = new byte[fastASequence.Count];
            (fastASequence as IList<byte>).CopyTo(tmpEncodedSeq, 0);
            Assert.AreEqual(expectedSequence.Length, tmpEncodedSeq.Length);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence Length is as expected."));

            Assert.AreEqual(_utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.SequenceIdNode), fastASequence.ID);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: SequenceID is as expected."));


            Assert.AreEqual(fastASequence.Alphabet.Name,
                _utilityObj._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName, Constants.AlphabetNameNode));
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence Alphabet is as expected."));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence BVT: Validation of FastaA file Sequence is completed successfully.");

        }

        /// <summary>
        /// Validate a DerivedSequence creation for a given FastaA file.
        /// Input Data : Valid FastA file Sequence.
        /// Output Data : Validation of derived
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateBasicDerivedSequenceWithFastaFormat()
        {
            // Gets the expected sequence from the Xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.ExpectedSequenceNode);
            string fastAFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.FilePathNode);

            Assert.IsTrue(File.Exists(fastAFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The File exist in the Path ", fastAFilePath));
            IList<ISequence> sequence = null;
            using (FastaParser parser = new FastaParser())
            {

                //Parse a FastA file Using Parse method and convert the same to sequence.
                sequence = parser.Parse(fastAFilePath);
            }

            Assert.IsNotNull(sequence);
            Sequence fastASequence = (Sequence)sequence[0];
            Assert.IsNotNull(fastASequence);
            Assert.AreEqual(expectedSequence, fastASequence.ToString());
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The Sequence is as expected."));

            byte[] tmpEncodedSeq = new byte[fastASequence.Count];
            (fastASequence as IList<byte>).CopyTo(tmpEncodedSeq, 0);
            Assert.AreEqual(expectedSequence.Length, tmpEncodedSeq.Length);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The Sequence Length is as expected."));

            Assert.AreEqual(_utilityObj._xmlUtil.GetTextValue(Constants.SimpleProteinAlphabetNode, Constants.SequenceIdNode), fastASequence.ID);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: SequenceID is as expected."));


            Assert.AreEqual(fastASequence.Alphabet.Name, _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.AlphabetNameNode));
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The Sequence Alphabet is as expected."));

            // Create a derived Sequences for the fastA file sequence.
            BasicDerivedSequence fastADerivedSeq =
                new BasicDerivedSequence(fastASequence, false, false, -1, -1);

            // validate the DerivedSequence with originalSequence.
            Assert.IsNotNull(fastADerivedSeq);
            Assert.AreEqual(expectedSequence, fastADerivedSeq.ToString());
            Assert.AreEqual(fastASequence.ToString(), fastADerivedSeq.ToString());
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The BasicDerived Sequence is as expected."));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence BVT: Validation of FastaA file Sequence is completed successfully.");

        }

        /// <summary>
        /// Validate a DerivedSequence creation for a given GenBank file.
        /// Input Data : Valid GenBank file Sequence.
        /// Output Data : Validatation of Derived sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateBasicDerivedSequenceWithGenBankFormat()
        {
            // Gets the expected sequence from the Xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleGeneBankNodeName, Constants.ExpectedSequenceNode);
            string geneBankFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleGeneBankNodeName, Constants.FilePathNode);

            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence BVT: The File exist in the Path ", geneBankFilePath));

            // Parse a GenBank file Using Parse method and convert the same to sequence.
            ISequenceParser parser = new GenBankParser();

            IList<ISequence> sequence = parser.Parse(geneBankFilePath);

            Assert.IsNotNull(sequence);
            Sequence geneBankSeq = (Sequence)sequence[0];
            Assert.IsNotNull(geneBankSeq);
            Assert.AreEqual(expectedSequence, geneBankSeq.ToString());
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The GenBank Sequence is as expected."));

            byte[] tmpEncodedSeq = new byte[geneBankSeq.Count];
            (geneBankSeq as IList<byte>).CopyTo(tmpEncodedSeq, 0);
            Assert.AreEqual(expectedSequence.Length, tmpEncodedSeq.Length);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The GenBank Sequence Length is as expected."));

            // Create a derived Sequences for the fastA file sequence.
            BasicDerivedSequence genebankDerivedSeq =
                new BasicDerivedSequence(geneBankSeq, false, false, -1, -1);

            // validate the DerivedSequence with originalSequence.
            Assert.IsNotNull(genebankDerivedSeq);
            Assert.AreEqual(expectedSequence, genebankDerivedSeq.ToString());
            Assert.AreEqual(geneBankSeq.ToString(), genebankDerivedSeq.ToString());
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The BasicDerived Sequence is as expected."));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence BVT: Validation of GenBank file Sequence is completed successfully.");
        }

        /// <summary>
        /// Validate Add() method by passing SequenceItem as a parameter.
        /// Input data : Valid FastaA file sequence.
        /// Output Data : Validation of Add() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSequenceInsertWithFastaFormat()
        {

            // Gets the expected sequence from the Xml
            string expectedSequenceCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.SimpleFastaSequenceCount);
            string fastAFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.FilePathNode);
            string expectedSeqAfterAdd = _utilityObj._xmlUtil.GetTextValue
                (Constants.SimpleFastaNodeName, Constants.ExpectedSeqAfterAdd);
            string seqAfterAdd = string.Empty;

            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string seqBeforeAdding = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            Sequence seqItem = new Sequence(Utility.GetAlphabet(alphabetName), "TCGN");

            Assert.IsTrue(File.Exists(fastAFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The File exist in the Path ", fastAFilePath));

            IList<ISequence> sequence = null;
            using (FastaParser parser = new FastaParser())
            {
                // Parse a FastA file Using Parse method and convert the same to sequence.
                sequence = parser.Parse(fastAFilePath);
            }

            Sequence Seq = (Sequence)sequence[0];
            Seq.IsReadOnly = false;
            seqBeforeAdding = Seq.ToString();
            Seq.Add(seqItem[0]);

            // Validate sequence list after adding sequence item to the sequence list.
            seqAfterAdd = Seq.ToString();
            Assert.AreEqual(seqAfterAdd, expectedSeqAfterAdd);
            Assert.AreNotEqual(seqAfterAdd, seqBeforeAdding);
            Assert.AreEqual(Seq.Count.ToString((IFormatProvider)null), expectedSequenceCount);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));
        }

        /// <summary>
        /// Validate Add() method by passing SequenceItem.
        /// Input Data : Valid GeneBankFile sequence.
        /// Output Data : Validatation of Add() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSequenceInsertWithGenBankFormat()
        {
            // Gets the expected sequence from the Xml
            string expectedSequenceCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleGeneBankNodeName, Constants.SimpleFastaSequenceCount);
            string geneBankFilePath = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleGeneBankNodeName, Constants.FilePathNode);
            string expectedSeqAfterAdd = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleGeneBankNodeName, Constants.ExpectedSeqAfterAdd);
            string seqAfterAdd = string.Empty;

            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string seqBeforeAdding = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            Sequence seqItem = new Sequence(Utility.GetAlphabet(alphabetName), "TCGN");

            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: The File exist in the Path ", geneBankFilePath));

            ISequenceParser parser = new GenBankParser();

            // Parse a FastA file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(geneBankFilePath);
            Sequence Seq = (Sequence)sequence[0];
            Seq.IsReadOnly = false;
            seqBeforeAdding = Seq.ToString();
            Seq.Add(seqItem[0]);

            // Validate sequence list after adding sequence item to the sequence list.
            seqAfterAdd = Seq.ToString();
            Assert.AreEqual(seqAfterAdd, expectedSeqAfterAdd);
            Assert.AreNotEqual(seqAfterAdd, seqBeforeAdding);
            Assert.AreEqual(Seq.Count.ToString((IFormatProvider)null), expectedSequenceCount);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));
        }

        /// <summary>
        /// Validate Clear() method by deleting the Sequence data from sequence list and validate the same.
        /// Input Data : Sequence string - "ACGA" and DNA Alphabets.
        /// Output Data : Sequence list shouuld be Empty.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateSequenceDelete()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedNormalString);

            string seqAfterDelete = string.Empty;
            int seqCountBefore;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence ", actualSequence, " and Alphabet ", alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            seqCountBefore = createSequence.Count;
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));

            // Delete Sequence data.
            createSequence.IsReadOnly = false;
            createSequence.Clear();
            seqAfterDelete = createSequence.ToString();

            // Validate Sequence list after removing the sequence data.
            Assert.IsTrue(string.IsNullOrEmpty(seqAfterDelete));
            Assert.AreNotEqual(createSequence.Count, seqCountBefore);
            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT : The sequence count is as expected"));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Concat(
                "Sequence BVT: Sequence is as expected."));
        }

        /// <summary>
        /// Validate FindMatches method by passing valid Pattern
        /// </summary>
        /// Input Data :  Valid Pattern
        /// Output Data : Validate Expected Results
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateFindMatches()
        {
            ValidatePatternMatch(
                Constants.SimplePatternNode,
                Constants.PatternNode,
                PatternType.FileMatches);
        }

        /// <summary>
        /// Validate Convert(string) method by passing valid Pattern
        /// </summary>
        /// Input Data :  Valid Pattern
        /// Output Data : Validate Expected Results
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateConvert()
        {
            string node = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimplePatternNode, Constants.PatternNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimplePatternNode,
                Constants.ConvertExpectedPatternSequenceNode);
            string expectedCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimplePatternNode,
                Constants.ConvertExpectedPatternCountNode);

            string[] expectedCounts = expectedCount.Split(',');
            string[] expectedSequence = expectedSeq.Split(',');
            string[] patterns = node.Split(',');

            IList<string> lstStr = new List<string>();
            IDictionary<string, IList<string>> result = null;

            foreach (string pattern in patterns)
            {
                lstStr.Add(pattern);
            }

            result =
                PatternConverter.GetInstanace(Alphabets.DNA).Convert(lstStr);

            int i = 0;
            foreach (string pattern in patterns)
            {
                Assert.AreEqual(expectedCounts[i].ToString(),
                    result[pattern].Count.ToString((IFormatProvider)null));

                string[] strValues = expectedSequence[i].Split(':');
                for (int j = 0; j < result[pattern].Count; j++)
                {
                    Assert.AreEqual(strValues[j],
                        result[pattern][j].ToString());
                }
                ++i;
            }

            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT : Successfully validated the expected result"));
            Console.WriteLine(string.Concat(
                "Sequence BVT : Successfully validated the expected result"));
        }

        /// <summary>
        /// Validate Convert(list of string) method by passing valid Pattern
        /// </summary>
        /// Input Data :  Valid Pattern
        /// Output Data : Validate Expected Results
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateConvertList()
        {
            string node = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimplePatternNode, Constants.PatternNode);
            string expectedSeq = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimplePatternNode,
                Constants.ConvertExpectedPatternSequenceNode);
            string expectedCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimplePatternNode,
                Constants.ConvertExpectedPatternCountNode);

            string[] expectedCounts = expectedCount.Split(',');
            string[] expectedSequence = expectedSeq.Split(',');
            string[] patterns = node.Split(',');

            IList<IList<string>> lstStr = new List<IList<string>>();

            foreach (string pattern in patterns)
            {
                lstStr.Add(
                    PatternConverter.GetInstanace(Alphabets.DNA).Convert(pattern));
            }

            int i = 0;
            foreach (string pattern in patterns)
            {
                Assert.AreEqual(expectedCounts[i].ToString(),
                    lstStr[i].Count.ToString((IFormatProvider)null));

                string[] strValues = expectedSequence[i].Split(':');
                for (int j = 0; j < lstStr[i].Count; j++)
                {
                    Assert.AreEqual(strValues[j],
                        lstStr[i][j].ToString());
                }
                ++i;
            }

            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT : Successfully validated the expected result"));
            Console.WriteLine(string.Concat(
                "Sequence BVT : Successfully validated the expected result"));
        }

        /// <summary>
        /// Validate FindMatch(string) method by passing valid Pattern
        /// </summary>
        /// Input Data :  Valid Pattern
        /// Output Data : Validate Expected Results
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateFindMatch()
        {
            ValidatePatternMatch(
                Constants.SimplePatternNode,
                Constants.SeqFileMatchNode,
                PatternType.FileMatch);
        }

        /// <summary>
        /// Validate FindMatch(list of string) method by passing valid Pattern
        /// </summary>
        /// Input Data :  Valid Pattern
        /// Output Data : Validate Expected Results
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateFindMatchList()
        {
            string nodeStr = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimplePatternNode, Constants.SeqFileMatchNode);
            string sequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimplePatternNode, Constants.Sequence1);
            string count = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimplePatternNode,
                Constants.FileMatchesExpectedCountNode);
            string value = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimplePatternNode,
                Constants.FileMatchesExpectedValueNode);

            string[] counts = count.Split(',');
            string[] values = value.Split(',');
            string[] patterns = nodeStr.Split(',');

            Sequence seq = new Sequence(Alphabets.DNA, sequence);
            IList<int> result = null;

            IList<string> lstStr = new List<string>();

            foreach (string pattern in patterns)
            {
                lstStr.Add(pattern);
            }

            int i = 0;
            foreach (string pattern in patterns)
            {
                result = new BoyerMoore().FindMatch(seq, pattern);
                if (result.Count != 0)
                {
                    Assert.AreEqual(counts[i].ToString(),
                        result.Count.ToString((IFormatProvider)null));
                    string[] strValues = values[i].Split(':');
                    for (int j = 0; j < result.Count; j++)
                    {
                        Assert.AreEqual(strValues[j],
                            result[j].ToString((IFormatProvider)null));
                    }
                    ++i;
                }
            }

            ApplicationLog.WriteLine(string.Concat(
                "Sequence BVT : Successfully validated the expected result"));
            Console.WriteLine(string.Concat(
                "Sequence BVT : Successfully validated the expected result"));
        }

        /// <summary>
        /// Validate FindMatches method by passing valid DNA Pattern
        /// </summary>
        /// Input Data :  Valid Pattern
        /// Output Data : Validate Expected Results
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateShortStringSearchFindMatchesDna()
        {
            ValidatePatternMatch(
                Constants.SimpleDnaPatternNode,
                Constants.PatternNode,
                PatternType.FileMatches);
        }

        /// <summary>
        /// Validate FindMatches method by passing valid RNA Pattern
        /// </summary>
        /// Input Data :  Valid Pattern
        /// Output Data : Validate Expected Results
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateShortStringSearchFindMatchesRna()
        {
            ValidatePatternMatch(
                Constants.SimpleRnaPatternNode,
                Constants.PatternNode,
                PatternType.FileMatches);
        }

        /// <summary>
        /// Validate FindMatches method by passing valid Protein Pattern
        /// </summary>
        /// Input Data :  Valid Pattern
        /// Output Data : Validate Expected Results
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateShortStringSearchFindMatchesProtein()
        {
            ValidatePatternMatch(
                Constants.SimpleProteinPatternNode,
                Constants.PatternNode,
                PatternType.FileMatches);
        }
        #endregion Sequence Bvt TestCases

        #region Support Method

        /// <summary>
        /// General Method to validate Search pattern methods
        /// </summary>
        /// <param name="type">Type of Pattern</param>
        void ValidatePatternMatch(
            string node,
            string subNode,
            PatternType type)
        {
            string nodeStr = _utilityObj._xmlUtil.GetTextValue(
                node, subNode);
            string sequence = _utilityObj._xmlUtil.GetTextValue(
                node,
                Constants.Sequence1);
            string count = _utilityObj._xmlUtil.GetTextValue(
                node,
                Constants.FileMatchesExpectedCountNode);
            string key = _utilityObj._xmlUtil.GetTextValue(
                node,
                Constants.FileMatchesExpectedKeyNode);
            string value = _utilityObj._xmlUtil.GetTextValue(
                node,
                Constants.FileMatchesExpectedValueNode);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                node, Constants.AlphabetNameNode);

            string[] counts = count.Split(',');
            string[] keys = key.Split(',');
            string[] values = value.Split(',');
            string[] patterns = nodeStr.Split(',');

            Sequence seq = new Sequence(
                Utility.GetAlphabet(alphabetName), sequence);
            IDictionary<string, IList<int>> result = null;

            IList<string> lstStr = new List<string>();

            foreach (string pattern in patterns)
            {
                lstStr.Add(pattern);
            }

            switch (type)
            {
                case PatternType.FileMatches:
                    result = seq.FindMatches(lstStr, 0, true);
                    break;
                case PatternType.FileMatch:
                    result = new BoyerMoore().FindMatch(seq, lstStr);
                    break;
            }

            int i = 0;
            foreach (string pattern in keys)
            {
                Assert.AreEqual(counts[i].ToString(),
                    result[pattern].Count.ToString((IFormatProvider)null));
                string[] strValues = values[i].Split(':');

                for (int j = 0; j < result[pattern].Count; j++)
                {
                    Assert.AreEqual(strValues[j],
                        result[pattern][j].ToString((IFormatProvider)null));
                }
                ++i;
            }

            ApplicationLog.WriteLine(string.Concat(
               "Sequence BVT : Successfully validated the expected result"));
            Console.WriteLine(string.Concat(
                "Sequence BVT : Successfully validated the expected result"));
        }
        #endregion Support Method
    }
}
