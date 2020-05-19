// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SequenceP1TestCases.cs
 * 
 * This file contains the Sequence and BasicDerived Sequence P1 test cases.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

using MBF.Encoding;
using MBF.IO;
using MBF.IO.Fasta;
using MBF.IO.GenBank;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;
using System.Runtime.Serialization.Formatters.Binary;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Sequences and P1 level validations.
    /// </summary>
    [TestFixture]
    public class SequenceP1TestCases
    {
        #region Enums

        /// <summary>
        /// BasicSequenceInformation method names.
        /// </summary>
        enum BasicSeqInformationMethodParameters
        {
            Index,
            LastIndex,
            Default
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SequenceP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region Sequence P1 TestCases

        /// <summary>
        /// Validate Add() method by passing FastA file RNA SequenceItem as a parameter.
        /// Input data : FastA file Sequence.
        /// Output Data : Validate Add() by passing sequence item.
        /// </summary>
        [Test]
        public void ValidateAddRnaSequenceItem()
        {
            // Gets the expected sequence from the Xml
            string fastAFilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaRnaNodeName, Constants.FilePathNode);
            string expectedSeqAfterAdd = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaRnaNodeName, Constants.ExpectedSeqAfterAdd);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.ExpectedNormalString);
            string seqAfterAdd = string.Empty;

            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string seqBeforeAdding = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", actualSequence, alphabetName));

            Sequence seqItem = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsTrue(File.Exists(fastAFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", fastAFilePath));
            FastaParser parser = new FastaParser();

            // Parse a FastA file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(fastAFilePath);
            Sequence seq = (Sequence)sequence[0];
            seq.IsReadOnly = false;
            seqBeforeAdding = seq.ToString();
            seq.Add(seqItem[0]);

            // Validate sequence list after adding sequence item to the sequence list.
            seqAfterAdd = seq.ToString();
            Assert.AreEqual(seqAfterAdd, expectedSeqAfterAdd);
            Assert.AreNotEqual(seqAfterAdd, seqBeforeAdding);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", seqAfterAdd));
        }

        /// <summary>
        /// Validate Add() method by passing RNA SequenceItem to GeneBank File.
        /// Input Data : Valid GeneBankFile sequence.
        /// Output Data : Validatation of Add() method.
        /// </summary>
        [Test]
        public void ValidateAddGeneBankFileRnaSequenceItem()
        {
            // Gets the expected sequence from the Xml
            string geneBankFilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankRnaNodeName, Constants.FilePathNode);
            string expectedSeqAfterAdd = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankRnaNodeName, Constants.ExpectedSeqAfterAdd);
            string seqAfterAdd = string.Empty;

            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.ExpectedNormalString);
            string seqBeforeAdding = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", actualSequence, alphabetName));

            Sequence seqItem = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", geneBankFilePath));

            ISequenceParser parser = new GenBankParser();

            // Parse a Genebank file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(geneBankFilePath);
            Sequence seq = (Sequence)sequence[0];
            seq.IsReadOnly = false;
            seqBeforeAdding = seq.ToString();
            seq.Add(seqItem[0]);

            // Validate sequence list after adding sequence item to the sequence list.
            seqAfterAdd = seq.ToString();
            Assert.AreEqual(seqAfterAdd, expectedSeqAfterAdd);
            Assert.AreNotEqual(seqAfterAdd, seqBeforeAdding);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", seqAfterAdd));
        }

        /// <summary>
        /// Validate Add() method by passing Protein SequenceItem to FastA File.
        /// Input Data : Valid Fasta sequence.
        /// Output Data : Validatation of Add() method.
        /// </summary>
        [Test]
        public void ValidateAddFastAFileProteinSequenceItem()
        {
            // Gets the expected sequence from the Xml
            string fastaFilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.FilePathNode);
            string expectedSeqAfterAdd = Utility._xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.ExpectedSeqAfterAdd);
            string seqAfterAdd = string.Empty;
            string proteinAlphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.AlphabetNameNode);
            string proteinActualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.ExpectedNormalString);
            string seqBeforeAdding = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", proteinActualSequence, proteinAlphabetName));

            Sequence seqItem = new Sequence(Utility.GetAlphabet(proteinAlphabetName), proteinActualSequence);
            Assert.IsTrue(File.Exists(fastaFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", fastaFilePath));

            FastaParser parser = new FastaParser();
            // Parse a FastA file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(fastaFilePath);
            Sequence seq = (Sequence)sequence[0];
            seq.IsReadOnly = false;
            seqBeforeAdding = seq.ToString();
            seq.Add(seqItem[0]);

            // Validate sequence list after adding sequence item to the sequence list.
            seqAfterAdd = seq.ToString();
            Assert.AreEqual(seqAfterAdd, expectedSeqAfterAdd);
            Assert.AreNotEqual(seqAfterAdd, seqBeforeAdding);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", seqAfterAdd));
        }

        /// <summary>
        /// Validate Add() method for less than 35KB FastA file by passing DNA Alphabet
        /// Input data : Valid FastaA file sequence.
        /// Output Data : Validation of Add() method.
        /// </summary>
        [Test]
        public void ValidateLessThan35KBFastAFileSequence()
        {

            // Gets the expected sequence from the Xml
            string expectedSequenceCount = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.SimpleFastaSequenceCount);
            string fastAFilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.FilePathNode);
            string expectedSeqAfterAdd = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.ExpectedSeqAfterAdd);
            string seqAfterAdd = string.Empty;
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedSingleChar);
            string seqBeforeAdding = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence BVT: Sequence {0} is expected.", actualSequence, alphabetName));

            Sequence seqItem = new Sequence(Utility.GetAlphabet(alphabetName), "TGN");
            Assert.IsTrue(File.Exists(fastAFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence BVT: The File exist in the Path {0}.", fastAFilePath));

            FastaParser parser = new FastaParser();

            // Parse a FastA file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(fastAFilePath);
            Sequence seq = (Sequence)sequence[0];
            seq.IsReadOnly = false;
            seqBeforeAdding = seq.ToString();
            seq.Add(seqItem[0]);

            // Validate sequence list after adding sequence item to the sequence list.
            seqAfterAdd = seq.ToString();
            Assert.AreEqual(seqAfterAdd, expectedSeqAfterAdd);
            Assert.AreNotEqual(seqAfterAdd, seqBeforeAdding);
            Assert.AreEqual(seq.Count.ToString((IFormatProvider)null), expectedSequenceCount);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence BVT: Sequence {0} is expected.", seqAfterAdd));
        }

        /// <summary>
        /// Validate Add() method by passing RNA SequenceItem to Medium size GeneBank File. 
        /// with less than 100KB.
        /// Input Data : Valid GeneBankFile sequence.
        /// Output Data : Validatation of Add() method.
        /// </summary>
        [Test]
        public void ValidateMediumSizeGeneBankFileRnaSequence()
        {
            // Gets the expected sequence from the Xml
            string geneBankFilePath = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeGenBankNodeName, Constants.FilePathNode);
            string expectedSeqAfterAdd = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeGenBankNodeName, Constants.ExpectedSeqAfterAdd);
            string seqAfterAdd = string.Empty;
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedNormalString);
            string seqBeforeAdding = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", actualSequence, alphabetName));

            Sequence seqItem = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", geneBankFilePath));

            ISequenceParser parser = new GenBankParser();

            // Parse a GeneBank file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(geneBankFilePath);
            Sequence seq = (Sequence)sequence[0];
            seq.IsReadOnly = false;
            seqBeforeAdding = seq.ToString();
            seq.Add(seqItem[0]);

            // Validate sequence list after adding sequence item to the sequence list.
            seqAfterAdd = seq.ToString();
            Assert.AreEqual(seqAfterAdd, expectedSeqAfterAdd);
            Assert.AreNotEqual(seqAfterAdd, seqBeforeAdding);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", seqAfterAdd));
        }

        /// <summary>
        /// Validate Clear() method by deleting the Sequence data from sequence list and validate the same.
        ///  Input Data : Valid GeneBankFile sequence.
        /// Output Data : Validatation of Clear() method.
        /// </summary>
        /// 
        [Test]
        public void ValidateGeneBankFileSequenceDelete()
        {
            // Gets the expected sequence from the Xml
            string geneBankFilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankRnaNodeName, Constants.FilePathNode);
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.ExpectedNormalString);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", actualSequence, alphabetName));
            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", geneBankFilePath));

            ISequenceParser parser = new GenBankParser();

            // Parse a GeneBank file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(geneBankFilePath);
            Sequence seq = (Sequence)sequence[0];
            seq.IsReadOnly = false;
            seq.Clear();

            // Validate Sequence list after removing the sequence data.
            Assert.IsEmpty(seq.ToString());

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", seq.ToString()));
        }

        /// <summary>
        /// Validate Insert() method by inserting Sequence item in the sequence list and validate the same.
        ///  Input Data : Valid Sequence."ACGA"
        /// Output Data : Valid Sequence. "AACGA".
        /// </summary>
        [Test]
        public void ValidateInsertSequenceItem()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedNormalString);
            string expectedSequenceAfterInsert = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedSequenceAfterInsert);
            string seqAfterReplace = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence '{0}' and Alphabet '{1}'.", actualSequence, alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", createSequence.ToString()));

            // Insert Sequence item.at the 0th position in the sequence list.
            createSequence.IsReadOnly = false;
            createSequence.Insert(0, 'A');
            seqAfterReplace = createSequence.ToString();

            // Validate Sequence list after inserting sequence item.
            Assert.AreEqual(seqAfterReplace, expectedSequenceAfterInsert);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1 : The sequence count {0} is expected", createSequence.Count));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", createSequence.ToString()));
        }

        /// <summary>
        /// Validate Insert() method by inserting Sequence item 
        /// in the sequence list and validate the same with DV enabled
        /// Input Data : Valid FastA file
        /// Output Data : Valid Inserted Sequence
        /// </summary>
        [Test]
        public void ValidateInsertSequenceItemWithDV()
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.MultiSequenceFileNodeName,
                Constants.FilePathNode1);

            FastaParser parserObj = new FastaParser();
            parserObj.EnforceDataVirtualization = true;
            IList<ISequence> seqList = parserObj.Parse(filePath);
            Sequence newSeqObj = (Sequence)seqList[0];
            newSeqObj.IsReadOnly = false;
            newSeqObj.Insert(0, 'A');

            Assert.AreEqual('A', newSeqObj[0].Symbol);

            ApplicationLog.WriteLine(
                "Sequence P1 : Successfully validated the Insert() method with DV");
            Console.WriteLine(
                "Sequence P1 : Successfully validated the Insert() method with DV");
        }

        /// <summary>
        /// Validate InsertRange() method by inserting range 
        /// in the sequence list and validate the same with DV enabled
        /// Input Data : Valid FastA file
        /// Output Data : Valid Inserted Range
        /// </summary>
        [Test]
        public void ValidateInsertRangeWithDV()
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.MultiSequenceFileNodeName,
                Constants.FilePathNode1);

            FastaParser parserObj = new FastaParser();
            parserObj.EnforceDataVirtualization = true;
            IList<ISequence> seqList = parserObj.Parse(filePath);
            Sequence newSeqObj = (Sequence)seqList[0];
            newSeqObj.IsReadOnly = false;
            newSeqObj.InsertRange(0, "AA");

            Assert.AreEqual('A', newSeqObj[0].Symbol);

            ApplicationLog.WriteLine(
                "Sequence P1 : Successfully validated the InsertRange() method with DV");
            Console.WriteLine(
                "Sequence P1 : Successfully validated the InsertRange() method with DV");

            // Set UseEncoding value true
            newSeqObj.UseEncoding = true;
            newSeqObj.InsertRange(0, "AA");

            Assert.AreEqual('A', newSeqObj[0].Symbol);
            Assert.AreEqual(886, newSeqObj.EncodedValues.Length);

            ApplicationLog.WriteLine(
                "Sequence P1 : Successfully validated the InsertRange() method with DV and Encoding");
            Console.WriteLine(
                "Sequence P1 : Successfully validated the InsertRange() method with DV and Encoding");
        }

        /// <summary>
        /// Validate RemoveRange() method by inserting range 
        /// in the sequence list and validate the same with DV enabled
        /// Input Data : Valid FastA file
        /// Output Data : Valid Remove Range
        /// </summary>
        [Test]
        public void ValidateRemoveRangeWithDV()
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.MultiSequenceFileNodeName,
                Constants.FilePathNode1);

            FastaParser parserObj = new FastaParser();
            parserObj.EnforceDataVirtualization = true;
            IList<ISequence> seqList = parserObj.Parse(filePath);
            Sequence newSeqObj = (Sequence)seqList[0];
            int oldCount = newSeqObj.Count;
            newSeqObj.IsReadOnly = false;
            newSeqObj.RemoveRange(0, 2);

            Assert.AreEqual(oldCount - 2, newSeqObj.Count);

            ApplicationLog.WriteLine(
                "Sequence P1 : Successfully validated the RemoveRange() method with DV");
            Console.WriteLine(
                "Sequence P1 : Successfully validated the RemoveRange() method with DV");
        }

        /// <summary>
        /// Validate ReplaceRange() method by inserting range 
        /// in the sequence list and validate the same with DV enabled
        /// Input Data : Valid FastA file
        /// Output Data : Valid Replace Range
        /// </summary>
        [Test]
        public void ValidateReplaceRangeWithDV()
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.MultiSequenceFileNodeName,
                Constants.FilePathNode1);

            FastaParser parserObj = new FastaParser();
            parserObj.EnforceDataVirtualization = true;
            IList<ISequence> seqList = parserObj.Parse(filePath);
            Sequence newSeqObj = (Sequence)seqList[0];
            newSeqObj.IsReadOnly = false;
            newSeqObj.ReplaceRange(0, "AA");

            Assert.AreEqual('A', newSeqObj[0].Symbol);

            ApplicationLog.WriteLine(
                "Sequence P1 : Successfully validated the ReplaceRange() method with DV");
            Console.WriteLine(
                "Sequence P1 : Successfully validated the ReplaceRange() method with DV");
        }

        /// <summary>
        /// Validate Insert() method by inserting sequence item at the end of the sequence.
        /// Input Data : Valid GeneBankFile sequence.
        /// Output Data : Validatation of Insert() method.
        /// </summary>
        [Test]
        public void ValidateMediumSizeGeneBankFileRnaSequenceInsertItem()
        {
            // Gets the expected sequence from the Xml
            string geneBankFilePath = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeGenBankNodeName, Constants.FilePathNode);
            string expectedSeqAfterInsert = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeGenBankNodeName, Constants.ExpectedSequenceAfterInsert);
            string seqAfterAdd = string.Empty;
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedNormalString);
            string seqBeforeInserting = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", actualSequence, alphabetName));

            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", geneBankFilePath));

            ISequenceParser parser = new GenBankParser();

            // Parse a Genebank file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(geneBankFilePath);
            Sequence seq = (Sequence)sequence[0];
            seq.IsReadOnly = false;
            seqBeforeInserting = seq.ToString();
            seq.Insert(50040, 'G');

            // Validate sequence list after inserting sequence item at the end of sequence list.
            seqAfterAdd = seq.ToString();
            Assert.AreEqual(seqAfterAdd, expectedSeqAfterInsert);
            Assert.AreNotEqual(seqAfterAdd, seqBeforeInserting);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", seqAfterAdd));
        }

        /// <summary>
        /// Validate Insert() method by inserting sequence item at the very begining of the sequence.
        /// Input Data : Valid GeneBankFile sequence.
        /// Output Data : Validatation of Insert() method.
        /// </summary>
        [Test]
        public void ValidateInsertMethodAtFirstPosition()
        {
            // Gets the expected sequence from the Xml
            string geneBankFilePath = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeGenBankNodeName, Constants.FilePathNode);
            string expectedSeqAfterInsertAtfirstPlace = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeGenBankNodeName, Constants.ExpectedSequenceAfterInsertAtFirstPosition);
            string seqAfterAdd = string.Empty;
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedNormalString);
            string seqBeforeInserting = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", actualSequence, alphabetName));

            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", geneBankFilePath));

            ISequenceParser parser = new GenBankParser();

            // Parse a Genebank file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(geneBankFilePath);
            Sequence seq = (Sequence)sequence[0];
            seq.IsReadOnly = false;
            seqBeforeInserting = seq.ToString();
            seq.Insert(0, 'G');

            // Validate sequence list after inserting sequence item at the end of sequence list.
            seqAfterAdd = seq.ToString();
            Assert.AreEqual(seqAfterAdd, expectedSeqAfterInsertAtfirstPlace);
            Assert.AreNotEqual(seqAfterAdd, seqBeforeInserting);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", seqAfterAdd));
        }

        /// <summary>
        /// Validate Remove() method by removing the Sequence item from sequence 
        /// list and validate the same.
        /// Input Data : Sequence string - "ACGA" and DNA Alphabets.
        /// Output Data : "CGA".
        /// </summary>
        [Test]
        public void ValidateRemoveSequenceItem()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedNormalString);
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedSequenceAfterRemoveSequenceData);
            string seqAfterRemove = string.Empty;
            int seqCountBefore;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence '{0}' and Alphabet '{1}'.", actualSequence, alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            seqCountBefore = createSequence.Count;
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", createSequence.ToString()));

            // Remove Sequence data. from the sequence list.
            createSequence.IsReadOnly = false;
            createSequence.Remove(createSequence[0]);
            seqAfterRemove = createSequence.ToString();

            // Validate Sequence list after removing the sequence data.
            Assert.AreEqual(seqAfterRemove, expectedSequence);
            Assert.AreNotEqual(createSequence.Count, seqCountBefore);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1 : The sequence count {0} is expected", createSequence.Count));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", createSequence.ToString()));
        }

        /// <summary>
        /// Validate RemoveAt() method by removing sequence item from sequence list by passing sequence index.
        /// Input Data : Valid GeneBankFile sequence.
        /// Output Data : Validatation of RemoveAt() method.
        /// </summary>
        [Test]
        public void ValidateRemoveAt()
        {
            // Gets the expected sequence from the Xml
            string geneBankFilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankRnaNodeName, Constants.FilePathNode);
            string expectedSeqAfterRemove = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankRnaNodeName, Constants.ExpectedSequenceAfterRemoveSequenceData);
            string seqAfterRemove = string.Empty;
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.ExpectedNormalString);
            string seqBeforeRemoving = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", actualSequence, alphabetName));

            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", geneBankFilePath));

            ISequenceParser parser = new GenBankParser();

            // Parse a FastA file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(geneBankFilePath);
            Sequence seq = (Sequence)sequence[0];
            seq.IsReadOnly = false;
            seqBeforeRemoving = seq.ToString();
            seq.RemoveAt(1474);

            // Validate sequence list after adding sequence item to the sequence list.
            seqAfterRemove = seq.ToString();
            Assert.AreEqual(seqAfterRemove, expectedSeqAfterRemove);
            Assert.AreNotEqual(seqAfterRemove, seqBeforeRemoving);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", seqAfterRemove));
        }

        /// <summary>
        /// Validate Replace() method by replacing the Sequence item in sequence list and validate the same.
        /// Input Data : Sequence string - "ACGA" and DNA Alphabets.
        /// Output Data : "CCGA".
        /// </summary>
        [Test]
        public void ValidateReplace()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedNormalString);
            string expectedSequenceAfterReplace = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedSequenceAfterReplace);
            string seqAfterReplace = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence '{0}' and Alphabet '{1}'.", actualSequence, alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", createSequence.ToString()));

            // Replace Sequence item. in the sequence list.
            createSequence.IsReadOnly = false;
            createSequence.Replace(0, 'C');
            seqAfterReplace = createSequence.ToString();

            // Validate Sequence list after replacing sequence item.
            Assert.AreEqual(seqAfterReplace, expectedSequenceAfterReplace);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1 : The sequence count {0} is expected", createSequence.Count));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", createSequence.ToString()));
        }

        /// <summary>
        /// Validate IndexOf() method by passing a Sequence item and get a index of sequence item.
        /// Input Data : Sequence string - "ACGA" and DNA Alphabets.
        /// Output Data : Index of sequence item 2.
        /// </summary>
        [Test]
        public void ValidateIndexOfSquenceItem()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode,
                Constants.ExpectedNormalString);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence '{0}' and Alphabet '{1}'.",
                actualSequence,
                alphabetName));

            Sequence createSequence =
                new Sequence(
                    Utility.GetAlphabet(alphabetName),
                    actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.",
                createSequence.ToString()));

            // Remove Sequence data. from the sequence list.
            createSequence.IsReadOnly = false;
            int seqIndex = createSequence.IndexOf(createSequence[2]);

            // Validate index of sequence item.
            Assert.AreEqual(seqIndex, 2);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1 : The sequence count {0} is expected",
                createSequence.Count));

            // Set UseEncoding value true
            createSequence =
                new Sequence(Utility.GetAlphabet(alphabetName),
                    actualSequence,
                    true);

            // Remove Sequence data. from the sequence list.
            createSequence.IsReadOnly = false;
            seqIndex = createSequence.IndexOf(createSequence[2]);
            Assert.IsNotNull(createSequence);

            // Validate index of sequence item.
            Assert.AreEqual(seqIndex, 2);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1 : The sequence count {0} is expected for Encoding",
                createSequence.Count));

            // Set UseEncoding value true
            createSequence =
                new Sequence(Utility.GetAlphabet(alphabetName),
                    actualSequence,
                    true);
            // Validate the createdSequence
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.",
                createSequence.ToString()));

            // Remove Sequence data. from the sequence list.
            createSequence.IsReadOnly = false;
            seqIndex = createSequence.IndexOf(createSequence[2]);

            // Validate index of sequence item.
            Assert.AreEqual(seqIndex, 2);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1 : The sequence count {0} is expected",
                createSequence.Count));
        }

        /// <summary>
        /// Validate a Basic derived sequence creation by passing RNA sequence.
        /// Input Data : Valid RNA Sequence "GAUUCAAGGGCU".
        /// Output Data : Validation of a Basic derived RNA sequence.
        /// </summary>
        [Test]
        public void ValidateRnaBasicDerivedSequence()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.ExpectedNormalString);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence '{0}' and Alphabet '{1}'.", actualSequence, alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is as expected.", createSequence.ToString()));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is as expected.", createSequence.ToString()));

            // Create Basic derived sequence.
            BasicDerivedSequence rnaDerivedSeq =
                new BasicDerivedSequence(createSequence, false, false, -1, -1);

            // Validate derived sequence.
            Assert.AreEqual(createSequence.ToString(), rnaDerivedSeq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Basic derived Sequence is '{0}' expected.", rnaDerivedSeq));
        }

        /// <summary>
        /// Validate a Basic DerivedSequence creation for a small size  GenBank file.
        /// Input Data : Valid small size GenBank file Sequence.
        /// Output Data : Validatation of basic Derived sequence.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceWithSmallSizeGenBankFormat()
        {
            // Gets the expected sequence from the Xml
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGeneBankNodeName, Constants.ExpectedSequenceNode);
            string geneBankFilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGeneBankNodeName, Constants.FilePathNode);

            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence P1: The File exist in the Path {0}.", geneBankFilePath));

            // Parse a GenBank file Using Parse method and convert the same to sequence.
            ISequenceParser parser = new GenBankParser();

            IList<ISequence> sequence = parser.Parse(geneBankFilePath);

            Assert.IsNotNull(sequence);
            Sequence geneBankSeq = (Sequence)sequence[0];
            Assert.IsNotNull(geneBankSeq);
            Assert.AreEqual(expectedSequence, geneBankSeq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The GenBank Sequence {0} is as expected.", geneBankSeq.ToString()));

            Assert.AreEqual(expectedSequence.Length, geneBankSeq.EncodedValues.Length);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The GenBank Sequence Length {0} is as expected.", expectedSequence.Length));

            // Create a derived Sequences for a Genebank file sequence.
            BasicDerivedSequence basicDerivedSeq =
                new BasicDerivedSequence(geneBankSeq, false, false, -1, -1);

            // Validate the DerivedSequence with originalSequence.
            Assert.IsNotNull(basicDerivedSeq);
            Assert.AreEqual(expectedSequence, basicDerivedSeq.ToString());
            Assert.AreEqual(geneBankSeq.ToString(), basicDerivedSeq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The BasicDerived Sequence {0} is expected.", geneBankSeq.ToString()));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of GenBank file Sequence is completed successfully.");
        }

        /// <summary>
        /// Validate a Basic DerivedSequence creation for a Medium size(Less than 100KB) GenBank file.
        /// Input Data : Valid GeneBankFile sequence.
        /// Output Data : Validatation of Add() method.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceWithMediumSizeGenBankFormat()
        {
            // Gets the expected sequence from the Xml
            string geneBankFilePath = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeGenBankNodeName, Constants.FilePathNode);
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeGenBankNodeName, Constants.ExpectedDerivedSequence);

            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", geneBankFilePath));

            ISequenceParser parser = new GenBankParser();

            // Parse a FastA file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(geneBankFilePath);
            Sequence geneBankSeq = (Sequence)sequence[0];
            geneBankSeq.IsReadOnly = false;

            // Create a derived Sequences for a GeneBank file sequence.
            BasicDerivedSequence basicDerivedSeq =
                new BasicDerivedSequence(geneBankSeq, false, false, -1, -1);

            // Validate the DerivedSequence with originalSequence.
            Assert.IsNotNull(basicDerivedSeq);
            Assert.AreEqual(expectedSequence, basicDerivedSeq.ToString());
            Assert.AreEqual(geneBankSeq.ToString(), basicDerivedSeq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The BasicDerived Sequence {0} is expected.", geneBankSeq.ToString()));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of GenBank file Derived Sequence is completed successfully.");
        }

        /// <summary>
        /// Validate a Basic DerivedSequence creation for a Large size(Laess than 350KB) GenBank file.
        /// Input Data : Valid GeneBankFile sequence.
        /// Output Data : Validatation of Add() method.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceWithLargeSizeGenBankFormat()
        {
            // Gets the expected sequence from the Xml
            string geneBankFilePath = Utility._xmlUtil.GetTextValue(
                Constants.LargeSizeGenBank, Constants.FilePathNode);
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.LargeSizeGenBank, Constants.ExpectedDerivedSequence);

            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", geneBankFilePath));

            ISequenceParser parser = new GenBankParser();

            // Parse a FastA file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(geneBankFilePath);
            Sequence geneBankSeq = (Sequence)sequence[0];
            geneBankSeq.IsReadOnly = false;

            // Create a derived Sequences for a Genebank file sequence.
            BasicDerivedSequence basicDerivedSeq =
                new BasicDerivedSequence(geneBankSeq, false, false, -1, -1);

            // Validate the DerivedSequence with originalSequence.
            Assert.IsNotNull(basicDerivedSeq);
            Assert.AreEqual(expectedSequence, basicDerivedSeq.ToString());
            Assert.AreEqual(geneBankSeq.ToString(), basicDerivedSeq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The BasicDerived Sequence {0} is expected.", geneBankSeq.ToString()));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of GenBank file Derived Sequence is completed successfully.");
        }

        /// <summary>
        /// Validate a Basic DerivedSequence creation for a small size  FastaA file.
        /// Input Data : Valid small size FastaA file Sequence.
        /// Output Data : Validatation of basic Derived sequence.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceWithSmallSizeFastAFormat()
        {
            // Gets the expected sequence from the Xml
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.ExpectedSequenceNode);
            string fastAFilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleFastaNodeName, Constants.FilePathNode);

            Assert.IsTrue(File.Exists(fastAFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence P1: The File exist in the Path {0}.", fastAFilePath));

            // Parse a GenBank file Using Parse method and convert the same to sequence.
            FastaParser parser = new FastaParser();

            IList<ISequence> sequence = parser.Parse(fastAFilePath);

            Assert.IsNotNull(sequence);
            Sequence fastASeq = (Sequence)sequence[0];
            Assert.IsNotNull(fastASeq);
            Assert.AreEqual(expectedSequence, fastASeq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The fastA Sequence {0} is as expected.", fastASeq.ToString()));

            Assert.AreEqual(expectedSequence.Length, fastASeq.EncodedValues.Length);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The fastA Sequence Length {0} is as expected.", expectedSequence.Length));

            // Create a derived Sequences for the fastA file sequence.
            BasicDerivedSequence basicDerivedSeq =
                new BasicDerivedSequence(fastASeq, false, false, -1, -1);

            // Validate the DerivedSequence with originalSequence.
            Assert.IsNotNull(basicDerivedSeq);
            Assert.AreEqual(expectedSequence, basicDerivedSeq.ToString());
            Assert.AreEqual(fastASeq.ToString(), basicDerivedSeq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The BasicDerived Sequence {0} is expected.", fastASeq.ToString()));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of fastA file Sequence is completed successfully.");
        }

        /// <summary>
        /// Validate a Basic DerivedSequence creation for a Medium size  FastaA file.
        /// Input Data : Valid medium size FastaA file Sequence.
        /// Output Data : Validatation of basic Derived sequence.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceWithMediumSizeFastAFormat()
        {
            // Gets the expected sequence from the Xml
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeFastaNodeName, Constants.ExpectedDerivedSequence);
            string fastAFilePath = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeFastaNodeName, Constants.FilePathNode);

            Assert.IsTrue(File.Exists(fastAFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence P1: The File exist in the Path {0}.", fastAFilePath));

            // Parse a GenBank file Using Parse method and convert the same to sequence.
            FastaParser parser = new FastaParser();

            IList<ISequence> sequence = parser.Parse(fastAFilePath);

            Assert.IsNotNull(sequence);
            Sequence fastASeq = (Sequence)sequence[0];
            Assert.IsNotNull(fastASeq);
            Assert.AreEqual(expectedSequence, fastASeq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The fastA Sequence {0} is as expected.", fastASeq.ToString()));

            Assert.AreEqual(expectedSequence.Length, fastASeq.EncodedValues.Length);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The fastA Sequence Length {0} is as expected.", expectedSequence.Length));

            // Create a derived Sequences for the fastA file sequence.
            BasicDerivedSequence basicDerivedSeq =
                new BasicDerivedSequence(fastASeq, false, false, -1, -1);

            // Validate the DerivedSequence with originalSequence.
            Assert.IsNotNull(basicDerivedSeq);
            Assert.AreEqual(expectedSequence, basicDerivedSeq.ToString());
            Assert.AreEqual(fastASeq.ToString(), basicDerivedSeq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The BasicDerived Sequence {0} is expected.", fastASeq.ToString()));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of fastA file Sequence is completed successfully.");
        }

        /// <summary>
        /// Validate a Basic DerivedSequence creation for a Large size >100KB  FastaA file.
        /// Input Data : Valid medium size FastaA file Sequence.
        /// Output Data : Validatation of basic Derived sequence.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceWithLargeSizeFastAFormat()
        {
            // Gets the expected sequence from the Xml
            string expectedSequence = Utility._xmlUtil.GetTextValue(
                Constants.LargeSizeFasta, Constants.ExpectedSequence);
            string fastAFilePath = Utility._xmlUtil.GetTextValue(
                Constants.LargeSizeFasta, Constants.FilePathNode);

            Assert.IsTrue(File.Exists(fastAFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence P1: The File exist in the Path {0}.", fastAFilePath));

            // Parse a GenBank file Using Parse method and convert the same to sequence.
            FastaParser parser = new FastaParser();

            IList<ISequence> sequence = parser.Parse(fastAFilePath);

            Assert.IsNotNull(sequence);
            Sequence fastASeq = (Sequence)sequence[0];
            Assert.IsNotNull(fastASeq);
            Assert.AreEqual(expectedSequence, fastASeq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The fastA Sequence {0} is as expected.", fastASeq.ToString()));

            Assert.AreEqual(expectedSequence.Length, fastASeq.EncodedValues.Length);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The fastA Sequence Length {0} is as expected.", expectedSequence.Length));

            // Create a derived Sequences for the fastA file sequence.
            BasicDerivedSequence basicDerivedSeq =
                new BasicDerivedSequence(fastASeq, false, false, -1, -1);

            // Validate the DerivedSequence with originalSequence.
            Assert.IsNotNull(basicDerivedSeq);
            Assert.AreEqual(expectedSequence, basicDerivedSeq.ToString());
            Assert.AreEqual(fastASeq.ToString(), basicDerivedSeq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The BasicDerived Sequence {0} is expected.", fastASeq.ToString()));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of fastA file Sequence is completed successfully.");
        }

        /// <summary>
        /// Validate a Index of BasicDerivedSequence item 
        /// Input Data : Valid DNA sequence.
        /// Output Data : Index of derived sequence item.
        /// </summary>
        [Test]
        public void ValidateIndexOfDerivedSequence()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(
            Constants.SimpleDnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleDnaAlphabetNode, Constants.ExpectedNormalString);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence BVT: Sequence '{0}' and Alphabet '{1}'.", actualSequence, alphabetName));

            Sequence createSequence = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Validate the createdSequence
            Assert.AreEqual(createSequence.ToString(), actualSequence);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence BVT: Sequence {0} is as expected.", createSequence.ToString()));

            // Create a Basic derived sequence for original sequence.
            BasicDerivedSequence derivedSeq =
                new BasicDerivedSequence(createSequence, false, false, -1, -1);

            // Get a index of derived sequence item 2.
            int seqIndesx = derivedSeq.IndexOf(createSequence[3]);

            // Validate index
            Assert.AreEqual(seqIndesx, 0);

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of derived sequence index was completed successfully.");
        }

        /// <summary>
        /// Validate a Index of BasicDerivedSequence item for FastA file protein Sequence.
        /// Input Data : Valid Protein  sequence.
        /// Output Data : Validation of index.
        /// </summary>
        [Test]
        public void ValidateIndexOfDerivedProteinSequence()
        {
            // Gets the expected sequence from the Xml
            string fastaFilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.FilePathNode);
            string proteinAlphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.AlphabetNameNode);
            string proteinActualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.ExpectedNormalString);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", proteinActualSequence, proteinAlphabetName));

            Assert.IsTrue(File.Exists(fastaFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", fastaFilePath));

            FastaParser parser = new FastaParser();

            // Parse a FastA file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(fastaFilePath);
            Sequence seq = (Sequence)sequence[0];
            seq.IsReadOnly = false;

            // Create a Basic derived sequence for original sequence.
            BasicDerivedSequence derivedSeq = new BasicDerivedSequence(seq, false, false, -1, -1);

            // Get a index of derived sequence item 2.
            int seqIndesx = derivedSeq.IndexOf(seq[10]);

            // Validate index
            Assert.AreEqual(seqIndesx, 2);

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of derived sequence index was completed successfully.");

        }

        /// <summary>
        /// Validate a Index of BasicDerivedSequence item for GeneBank file sequence.
        /// Input Data : Valid GeneBankFile sequence.
        /// Output Data : Validation of index.
        /// </summary>
        [Test]
        public void ValidateIndexOfDerivedGeneBankFileSequence()
        {
            // Gets the expected sequence from the Xml
            string geneBankFilePath = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeGenBankNodeName, Constants.FilePathNode);

            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", geneBankFilePath));

            ISequenceParser parser = new GenBankParser();

            // Parse a FastA file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(geneBankFilePath);
            Sequence geneBankSeq = (Sequence)sequence[0];
            geneBankSeq.IsReadOnly = false;

            // Create a derived Sequences for the fastA file sequence.
            BasicDerivedSequence basicDerivedSeq =
                new BasicDerivedSequence(geneBankSeq, false, false, -1, -1);

            // Get the index.
            int geneBankSeqIndex = basicDerivedSeq.IndexOf(geneBankSeq[30]);

            // Validate index
            Assert.AreEqual(geneBankSeqIndex, 1);

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of GenBank file Derived Sequence index was completed successfully.");
        }

        /// <summary>
        /// Validate Add() method by passing DNA SequenceItem to GeneBank File.
        /// Input Data : Valid GeneBankFile sequence.
        /// Output Data : Validatation of Add() method.
        /// </summary>
        [Test]
        public void ValidateAddGeneBankFileDnaSequenceItem()
        {
            // Gets the expected sequence from the Xml
            string geneBankFilePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankDnaNodeName, Constants.FilePathNode);
            string expectedSeqAfterAdd = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankDnaNodeName, Constants.ExpectedSeqAfterAdd);
            string seqAfterAdd = string.Empty;

            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.ExpectedNormalString);
            string seqBeforeAdding = string.Empty;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", actualSequence, alphabetName));

            Sequence seqItem = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsTrue(File.Exists(geneBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: The File exist in the Path {0}.", geneBankFilePath));

            ISequenceParser parser = new GenBankParser();

            // Parse a Genebank file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(geneBankFilePath);
            Sequence seq = (Sequence)sequence[0];
            seq.IsReadOnly = false;
            seqBeforeAdding = seq.ToString();
            seq.Add(seqItem[0]);

            // Validate sequence list after adding sequence item to the sequence list.
            seqAfterAdd = seq.ToString();
            Assert.AreEqual(seqAfterAdd, expectedSeqAfterAdd);
            Assert.AreNotEqual(seqAfterAdd, seqBeforeAdding);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Sequence {0} is expected.", seqAfterAdd));
        }

        /// <summary>
        /// Validate Dna, Rna and Protein Alphabets.
        /// Input data : Alphabets.
        /// Output Data : Validation of alphabets class properties.
        /// </summary>
        [Test]
        public void ValidateAlphabets()
        {
            // Get a expected values from the Xml
            string alphabetsCount = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaRnaNodeName,
                Constants.AlphabetsCountNode);
            string rnaAlphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleRnaAlphabetNode,
                Constants.RnaAlphabetNode);
            string dnaAlphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleRnaAlphabetNode,
                Constants.DnaAlphabetNode);
            string proteinAlphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleRnaAlphabetNode,
                Constants.ProteinAlphabetNode);

            // Get alphabet names and count.
            IList<IAlphabet> alphabetList = Alphabets.All;

            // Validate Alphabet class properties.
            Assert.AreEqual(alphabetsCount, alphabetList.Count.ToString());
            Assert.AreEqual(rnaAlphabetName, alphabetList[1].Name);
            Assert.AreEqual(dnaAlphabetName, alphabetList[0].Name);
            Assert.AreEqual(proteinAlphabetName, alphabetList[2].Name);
            ApplicationLog.WriteLine(string.Format(null,
                "Alphabets :Alphabets validation is successful {0} ",
                alphabetList.Count));
        }

        /// <summary>
        /// Validate Sequence index of non gap characters
        /// Input data : Sequence.
        /// Output Data : Validation of Sequence index.
        /// </summary>
        [Test]
        public void ValidateSequenceIndexOfNonGapChars()
        {
            ValidateGeneralIndexOfNonGapChars(Constants.BasicSeqInfoNode,
                BasicSeqInformationMethodParameters.Index, false);
        }

        /// <summary>
        /// Validate Sequence index of non gap characters by passing index value.
        /// Input data : Sequence.
        /// Output Data : Validation of Sequence index.
        /// </summary>
        [Test]
        public void ValidateSequenceIndexOfNonGapCharsUsingIndex()
        {
            ValidateGeneralIndexOfNonGapChars(Constants.BasicSeqInfoNode,
                BasicSeqInformationMethodParameters.Index, true);
        }

        /// <summary>
        /// Validate Sequence last index of non gap characters
        /// Input data : Sequence.
        /// Output Data : Validation of Sequence last index.
        /// </summary>
        [Test]
        public void ValidateSequenceLastIndexOfNonGapChars()
        {
            ValidateGeneralIndexOfNonGapChars(Constants.BasicSeqInfoLastIndexGapCharNode,
                BasicSeqInformationMethodParameters.LastIndex, false);
        }

        /// <summary>
        /// Validate Sequence last index of non gap characters by passing index value.
        /// Input data : Sequence.
        /// Output Data : Validation of Sequence last index.
        /// </summary>
        [Test]
        public void ValidateSequenceLastIndexOfNonGapCharsUsingIndex()
        {
            ValidateGeneralIndexOfNonGapChars(Constants.BasicSeqInfoLastIndexGapCharNode,
                BasicSeqInformationMethodParameters.LastIndex, true);
        }

        /// <summary>
        /// Validate BasicSequenceInfo GetObjectData.
        /// Input data : Sequence.
        /// Output Data : Validation of GetObjectData method.
        /// </summary>
        [Test]
        public void ValidateBasicSequenceInfoGetObjectData()
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.BasicSeqInfoLastIndexGapCharNode,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(Constants.BasicSeqInfoLastIndexGapCharNode,
                Constants.ExpectedNormalString);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);

            // Create a BasicSequenceInfo object.
            BasicSequenceInfo basicSeqInfoObj = new BasicSequenceInfo();
            SerializationInfo info = new SerializationInfo(typeof(Sequence),
                new FormatterConverter());
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            // Validate GetObjectData
            basicSeqInfoObj.GetObjectData(info, context);

            // Validate Sequence.
            Assert.AreEqual(sequence, seq.ToString());
        }

        /// <summary>
        /// Validate BasicDerivedSequence GetObjectData.
        /// Input data : Sequence.
        /// Output Data : Validation of GetObjectData method.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceGetObjectData()
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequence);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);

            // Create a BasicDerivedSequence object.
            BasicDerivedSequence basicDerSeqObj = new BasicDerivedSequence(seq, false, false,
                0, seq.Count);

            SerializationInfo info = new SerializationInfo(typeof(Sequence),
                new FormatterConverter());
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            // Validate GetObjectData
            basicDerSeqObj.GetObjectData(info, context);

            // Validate Sequence.
            Assert.AreEqual(sequence, seq.ToString());
        }

        /// <summary>
        /// Validate BasicDerivedSequence Add().
        /// Input data : Sequence.
        /// Output Data : Validation of Add() method.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceAdd()
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequence);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);
            seq.IsReadOnly = false;

            // Create a BasicDerivedSequence object.
            BasicDerivedSequence basicDerSeqObj = new BasicDerivedSequence(seq, false, false,
                0, seq.Count);

            ISequenceItem addItem = seq[0];

            try
            {
                basicDerSeqObj.Add(addItem);
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine(string.Format("Successfully caught the exception '{0}'",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format("Successfully caught the exception '{0}'",
                    ex.Message));
            }
        }

        /// <summary>
        /// Validate BasicDerivedSequence RemoveAt().
        /// Input data : Sequence.
        /// Output Data : Validation of RemoveAt() method.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceRemoveAt()
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequence);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);
            seq.IsReadOnly = false;

            // Create a BasicDerivedSequence object.
            BasicDerivedSequence basicDerSeqObj = new BasicDerivedSequence(seq, false, false,
                0, seq.Count);

            try
            {
                basicDerSeqObj.RemoveAt(0);
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine(string.Format("Successfully caught the exception '{0}'",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format("Successfully caught the exception '{0}'",
                    ex.Message));
            }
        }

        /// <summary>
        /// Validate BasicDerivedSequence Clear().
        /// Input data : Sequence.
        /// Output Data : Validation of Clear() method.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceClear()
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequence);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);
            // Create a BasicDerivedSequence object.
            BasicDerivedSequence basicDerSeqObj = new BasicDerivedSequence(seq, false, false,
                0, seq.Count);


            try
            {
                basicDerSeqObj.Clear();
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine(string.Format("Successfully caught the exception '{0}'",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format("Successfully caught the exception '{0}'",
                    ex.Message));
            }
        }

        /// <summary>
        /// Validate BasicDerivedSequence Insert().
        /// Input data : Sequence.
        /// Output Data : Validation of Insert() method.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceInsert()
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequence);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);

            // Create a BasicDerivedSequence object.
            BasicDerivedSequence basicDerSeqObj = new BasicDerivedSequence(seq, false, false,
                0, seq.Count);

            ISequenceItem addItem = seq[0];

            try
            {
                basicDerSeqObj.Insert(0, addItem);
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine(string.Format("Successfully caught the exception '{0}'",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format("Successfully caught the exception '{0}'",
                    ex.Message));
            }
        }

        /// <summary>
        /// Validate BasicDerivedSequence InsertRange().
        /// Input data : Sequence.
        /// Output Data : Validation of InsertRange() method.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceInsertRange()
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequence);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);

            // Create a BasicDerivedSequence object.
            BasicDerivedSequence basicDerSeqObj = new BasicDerivedSequence(seq, false, false,
                0, seq.Count);

            ISequenceItem addItem = seq[0];

            try
            {
                basicDerSeqObj.InsertRange(0, addItem.ToString());
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine(string.Format("Successfully caught the exception '{0}'",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format("Successfully caught the exception '{0}'",
                    ex.Message));
            }
        }

        /// <summary>
        /// Validate BasicDerivedSequence IndexOfNonGap().
        /// Input data : Sequence.
        /// Output Data : Validation of IndexOfNonGap() method.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceIndexOfNonGap()
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequence);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);

            // Create a BasicDerivedSequence object.
            BasicDerivedSequence basicDerSeqObj = new BasicDerivedSequence(seq, false, false,
                0, seq.Count);

            int index = basicDerSeqObj.IndexOfNonGap();

            Assert.AreEqual(0, index);
        }

        /// <summary>
        /// Validate BasicDerivedSequence LastIndexOfNonGap().
        /// Input data : Sequence.
        /// Output Data : Validation of LastIndexOfNonGap() method.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceLastIndexOfNonGap()
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequence);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);

            // Create a BasicDerivedSequence object.
            BasicDerivedSequence basicDerSeqObj = new BasicDerivedSequence(seq, false, false,
                0, seq.Count);

            int index = basicDerSeqObj.LastIndexOfNonGap();

            Assert.AreEqual(seq.Count - 1, index);
        }

        /// <summary>
        /// Validate BasicDerivedSequence Properties.
        /// Input data : Sequence.
        /// Output Data : Validation of all properties.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceAllProperties()
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequence);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);
            seq.MoleculeType = MoleculeType.Protein;

            // Create a BasicDerivedSequence object.
            BasicDerivedSequence basicDerSeqObj = new BasicDerivedSequence(seq.Clone(), false, false,
                0, seq.Count);

            // Validate Readonly property
            Assert.IsTrue(basicDerSeqObj.IsReadOnly);

            // Validate MoleculeType property
            Assert.AreEqual(MoleculeType.Protein.ToString(),
                basicDerSeqObj.MoleculeType.ToString());
            Dictionary<string, object> metaObj = basicDerSeqObj.Metadata;
            // Validate MetaData property
            Assert.AreEqual(0, metaObj.Count);
            // Validate Documentation property
            Assert.IsNull(basicDerSeqObj.Documentation);
            // Validate Statistics property
            SequenceStatistics stats = basicDerSeqObj.Statistics;
            Assert.IsNotNull(basicDerSeqObj.Statistics);
            Assert.IsTrue(0 != stats.GetCount(seq[0]));
            Assert.IsTrue(0 != stats.GetFraction(seq[0]));
        }

        /// <summary>
        /// Validate BasicDerivedSequence Contains().
        /// Input data : Sequence.
        /// Output Data : Validation of Contains() method.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceContains()
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequence);

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);

            // Create a BasicDerivedSequence object.
            BasicDerivedSequence basicDerSeqObj = new BasicDerivedSequence(seq, false, false,
                0, seq.Count);

            // Validate Contains method
            ISequenceItem item = seq[0];
            Assert.IsTrue(basicDerSeqObj.Contains(item));
        }

        /// <summary>
        /// Validate SequenceStatistics GetObjectData().
        /// Input data : Sequence.
        /// Output Data : Validation of GetObjectData() method.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceStatisticsGetObjectData()
        {
            ISequence seq = new Sequence(Alphabets.DNA, "GGCCTT");

            // Create a BasicDerivedSequence object.
            BasicDerivedSequence basicDerSeqObj = new BasicDerivedSequence(seq, false, false,
                0, seq.Count);
            basicDerSeqObj.Complemented = true;


            SerializationInfo info = new SerializationInfo(typeof(Sequence),
                new FormatterConverter());
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            SequenceStatistics stats = basicDerSeqObj.Statistics;

            // Validate GetObjectData
            stats.GetObjectData(info, context);

            // Validate Sequence.
            Assert.AreEqual("GGCCTT", seq.ToString());

            // Validate Complement Property
            basicDerSeqObj.RangeLength = -1;

            stats = basicDerSeqObj.Statistics;

            // Validate Serialization.
            Sequence seqObj = new Sequence(Alphabets.DNA, "ACGTACGT");

            Stream stream = File.Open("Sequence.data", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, seqObj);

            stream.Seek(0, SeekOrigin.Begin);
            Sequence deserializedSeq = (Sequence)formatter.Deserialize(stream);

            Assert.AreNotSame(seq, deserializedSeq);
            Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);

        }

        /// <summary>
        /// Validate Sequence GetObjectData().
        /// Input data : Sequence.
        /// Output Data : Validation of GetObjectData() method.
        /// </summary>
        [Test]
        public void ValidateSequenceGetObjectData()
        {
            Sequence seq = new Sequence(Alphabets.DNA, "GGCCTT");

            SerializationInfo info = new SerializationInfo(typeof(Sequence),
                new FormatterConverter());
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            // Validate GetObjectData
            seq.GetObjectData(info, context);

            // Validate Sequence.
            Assert.AreEqual("GGCCTT", seq.ToString());

            // Logs to Nunit GUI.
            Console.WriteLine("Sequence P1: Validation of GetObjectData() method successful.");
            ApplicationLog.WriteLine("Sequence P1: Validation of GetObjectData() method successful.");
        }

        /// <summary>
        /// Validate Sequence All properties.
        /// Input data : Sequence.
        /// Output Data : Validation of all properties in Sequence class.
        /// </summary>
        [Test]
        public void ValidateSequenceAllProperties()
        {
            Sequence seq = new Sequence(Alphabets.DNA, Encodings.IupacNA,
                new SequenceEncoder(Encodings.IupacNA),
                new SequenceDecoder(Encodings.IupacNA),
                "GGCCTT");

            // Set all properties
            seq.IsReadOnly = false;
            seq.DisplayID = "Display ID";
            seq.Documentation = "Documentation";
            seq.ID = "Sequence ID";
            seq.MoleculeType = MoleculeType.DNA;
            seq.UseEncoding = true;

            // Validate all properties
            Assert.AreEqual("CCGGAA", seq.Complement.ToString());
            Assert.AreEqual("TTCCGG", seq.Reverse.ToString());
            Assert.AreEqual("AAGGCC", seq.ReverseComplement.ToString());
            Assert.AreEqual(6, seq.Count);
            Assert.AreEqual(new SequenceDecoder(Encodings.IupacNA).ToString(),
                seq.Decoder.ToString());
            Assert.AreEqual(new SequenceEncoder(Encodings.IupacNA).ToString(),
                seq.Encoder.ToString());
            Assert.AreEqual("Display ID", seq.DisplayID);
            Assert.AreEqual("Documentation", seq.Documentation);
            Assert.IsNotNull(seq.Metadata);
            Assert.AreEqual("Sequence ID", seq.ID);
            Assert.IsNotNull(seq.EncodedValues);
            Assert.AreEqual(Encodings.IupacNA, seq.Encoding);
            Assert.IsFalse(seq.IsReadOnly);
            Assert.IsNull(seq.VirtualSequenceProvider);
            Assert.AreEqual(-1, seq.BlockSize);
            Assert.AreEqual(0, seq.MaxNumberOfBlocks);
            Assert.AreEqual("GGCCTT", seq.ToString());
            Assert.IsNotNull(seq.Statistics);
            Assert.AreEqual(true, seq.UseEncoding);

            // Logs to Nunit GUI.
            Console.WriteLine("Sequence P1: Validation of all the properties successful.");
            ApplicationLog.WriteLine("Sequence P1: Validation of all the properties successful.");
        }

        /// <summary>
        /// Validate Sequence Add() method.
        /// Input data : Sequence.
        /// Output Data : Validation of Add() method in Sequence class.
        /// </summary>
        [Test]
        public void ValidateSequenceAdd()
        {
            Sequence seq = new Sequence(Alphabets.DNA, "GGCCTT");
            seq.IsReadOnly = false;
            ISequenceItem itm = seq[0];

            // Clear is called before Add to make sure Statistics property is set
            seq.Clear();
            seq.Add(itm);

            Assert.AreEqual("G", seq.ToString());

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of Add() method successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of Add() method successful.");

            // Set UseEncoding value false
            seq = new Sequence(Alphabets.DNA, "GGCCTT", false);
            seq.IsReadOnly = false;
            itm = seq[0];

            // Clear is called before Add to make sure Statistics property is set
            seq.Clear();
            seq.Add(itm);

            Assert.AreEqual("G", seq.ToString());

            // Set UseEncoding value true
            seq = new Sequence(Alphabets.DNA, "GGCCTT", true);
            seq.IsReadOnly = false;
            itm = seq[0];

            // Clear is called before Add to make sure Statistics property is set
            seq.Clear();
            seq.Add(itm);

            Assert.AreEqual("G", seq.ToString());

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of Add() method with Encoding successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of Add() method with Encoding successful.");
        }

        /// <summary>
        /// Validate Sequence InsertRange() method.
        /// Input data : Sequence.
        /// Output Data : Validation of InsertRange() method in Sequence class.
        /// </summary>
        [Test]
        public void ValidateSequenceInsertRange()
        {
            Sequence seq = new Sequence(Alphabets.DNA, "GGCCTT");
            seq.IsReadOnly = false;

            seq.InsertRange(1, "G");
            // Clear is called before Add to make sure Statistics property is set
            seq.Clear();
            seq.InsertRange(0, "G");

            Assert.AreEqual("G", seq.ToString());

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of InsertRange() method successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of InsertRange() method successful.");

            // Set UseEncoding value true
            seq = new Sequence(Alphabets.DNA, "GGCCTT", true);
            seq.IsReadOnly = false;

            seq.InsertRange(0, "G");
            Assert.AreEqual("GGGCCTT", seq.ToString());

            // Set UseEncoding value false
            seq = new Sequence(Alphabets.DNA, "GGCCTT", false);
            seq.IsReadOnly = false;
            seq.InsertRange(1, "A");

            Assert.AreEqual("GAGCCTT", seq.ToString());

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of InsertRange() method with Encoding successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of InsertRange() method with Encoding successful.");
        }

        /// <summary>
        /// Validate Sequence Range() method.
        /// Input data : Sequence.
        /// Output Data : Validation of Range() method in Sequence class.
        /// </summary>
        [Test]
        public void ValidateSequenceRange()
        {
            Sequence seq = new Sequence(Alphabets.DNA, "GGCCTT");
            seq.IsReadOnly = false;

            // Clear is called before Add to make sure Statistics property is set
            seq.Clear();
            seq.InsertRange(0, "G");
            Assert.AreEqual("G", seq.Range(0, 1).ToString());

            // Logs to Nunit GUI.
            Console.WriteLine("Sequence P1: Validation of Range() method successful.");
            ApplicationLog.WriteLine("Sequence P1: Validation of Range() method successful.");
        }

        /// <summary>
        /// Validate Sequence Remove() method.
        /// Input data : Sequence.
        /// Output Data : Validation of Remove() method in Sequence class.
        /// </summary>
        [Test]
        public void ValidateSequenceRemove()
        {
            Sequence seq = new Sequence(Alphabets.DNA, "GGCCTT");
            seq.IsReadOnly = false;
            ISequenceItem itm = seq[0];

            seq.Remove(itm);
            Assert.AreEqual("GCCTT", seq.ToString());

            // Logs to Nunit GUI.
            Console.WriteLine("Sequence P1: Validation of Remove() method successful.");
            ApplicationLog.WriteLine("Sequence P1: Validation of Remove() method successful.");
        }

        /// <summary>
        /// Validate Sequence RemoveAt() method.
        /// Input data : Sequence.
        /// Output Data : Validation of RemoveAt() method in Sequence class.
        /// </summary>
        [Test]
        public void ValidateSequenceRemoveAt()
        {
            Sequence seq = new Sequence(Alphabets.DNA, "GGCCTT");
            seq.IsReadOnly = false;

            seq.RemoveAt(0);
            Assert.AreEqual("GCCTT", seq.ToString());

            // Logs to Nunit GUI.
            Console.WriteLine("Sequence P1: Validation of RemoveAt() method successful.");
            ApplicationLog.WriteLine("Sequence P1: Validation of RemoveAt() method successful.");
        }

        /// <summary>
        /// Validate Sequence RemoveRange() method.
        /// Input data : Sequence.
        /// Output Data : Validation of RemoveRange() method in Sequence class.
        /// </summary>
        [Test]
        public void ValidateSequenceRemoveRange()
        {
            Sequence seq = new Sequence(Alphabets.DNA, "GGCCTT");
            seq.IsReadOnly = false;

            seq.RemoveRange(0, 1);
            Assert.AreEqual("GCCTT", seq.ToString());

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of RemoveRange() method successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of RemoveRange() method successful.");

            // Set UseEncoding value false
            seq = new Sequence(Alphabets.DNA, "GGCCTT", false);
            seq.IsReadOnly = false;
            SequenceStatistics seqStatic = seq.Statistics;
            seq.RemoveRange(1, 3);

            Assert.AreEqual("GTT", seq.ToString());
            Assert.AreEqual(1, seqStatic.GetCount(seq[0].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[1].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[2].Symbol));

            // Set UseEncoding value false
            seq = new Sequence(Alphabets.DNA, "GGCCTT", true);
            seq.IsReadOnly = false;
            seqStatic = seq.Statistics;
            seq.RemoveRange(1, 3);

            Assert.AreEqual("GTT", seq.ToString());
            Assert.AreEqual(1, seqStatic.GetCount(seq[0].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[1].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[2].Symbol));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of RemoveRange() method with Encoding successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of RemoveRange() method with Encoding successful.");
        }

        /// <summary>
        /// Validate Sequence Replace() method.
        /// Input data : Sequence.
        /// Output Data : Validation of Replace() method in Sequence class.
        /// </summary>
        [Test]
        public void ValidateSequenceReplace()
        {
            Sequence seq = new Sequence(Alphabets.DNA, "GGCCTT");
            seq.IsReadOnly = false;
            ISequenceItem itm = seq[0];

            seq.Replace(0, itm);
            Assert.AreEqual("GGCCTT", seq.ToString());

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of Replace() method successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of Replace() method successful.");

            // Set UseEncoding value false
            seq = new Sequence(Alphabets.DNA, "GGCCTT", true);
            seq.IsReadOnly = false;
            SequenceStatistics seqStatic = seq.Statistics;
            itm = seq[0];

            seq.Replace(0, itm);
            Assert.AreEqual("GGCCTT", seq.ToString());
            Assert.AreEqual(2, seqStatic.GetCount(seq[0].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[1].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[2].Symbol));

            // Set UseEncoding value true
            seq = new Sequence(Alphabets.DNA, "GGCCTT", false);
            seq.IsReadOnly = false;
            seqStatic = seq.Statistics;
            itm = seq[0];

            seq.Replace(0, itm);
            Assert.AreEqual("GGCCTT", seq.ToString());
            Assert.AreEqual(2, seqStatic.GetCount(seq[0].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[1].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[2].Symbol));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of Replace() method with Encoding successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of Replace() method with Encoding successful.");
        }

        /// <summary>
        /// Validate Sequence Replace(int, Char) method.
        /// Input data : Sequence.
        /// Output Data : Validation of Replace(int, Char) method in Sequence class.
        /// </summary>
        [Test]
        public void ValidateSequenceReplaceChar()
        {
            Sequence seq = new Sequence(Alphabets.DNA, "GGCCTT");
            seq.IsReadOnly = false;

            seq.Replace(0, 'G');
            Assert.AreEqual("GGCCTT", seq.ToString());

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of Replace() method successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of Replace() method successful.");

            // Set UseEncoding value false
            seq = new Sequence(Alphabets.DNA, "GGCCTT", false);
            seq.IsReadOnly = false;
            SequenceStatistics seqStatic = seq.Statistics;

            seq.Replace(0, 'G');
            Assert.AreEqual("GGCCTT", seq.ToString());
            Assert.AreEqual(2, seqStatic.GetCount(seq[0].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[1].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[2].Symbol));

            // Set UseEncoding value true
            seq = new Sequence(Alphabets.DNA, "GGCCTT", true);
            seq.IsReadOnly = false;
            seqStatic = seq.Statistics;

            seq.Replace(0, 'G');
            Assert.AreEqual("GGCCTT", seq.ToString());
            Assert.AreEqual(2, seqStatic.GetCount(seq[0].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[1].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[2].Symbol));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of Replace() method with Encoding successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of Replace() method with Encoding successful.");
        }

        /// <summary>
        /// Validate Sequence ReplaceRange() method.
        /// Input data : Sequence.
        /// Output Data : Validation of ReplaceRange() method in Sequence class.
        /// </summary>
        [Test]
        public void ValidateSequenceReplaceRange()
        {
            Sequence seq = new Sequence(Alphabets.DNA, "GGCCTT");
            seq.IsReadOnly = false;

            seq.ReplaceRange(0, "GG");
            Assert.AreEqual("GGCCTT", seq.ToString());

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of ReplaceRange() method successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of ReplaceRange() method successful.");

            //Set Encoding value false
            seq = new Sequence(Alphabets.DNA, "GGCCTT", false);
            seq.IsReadOnly = false;
            SequenceStatistics seqStatic = seq.Statistics;
            seq.ReplaceRange(0, "GG");
            Assert.AreEqual("GGCCTT", seq.ToString());
            Assert.AreEqual(2, seqStatic.GetCount(seq[0].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[1].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[2].Symbol));

            //Set Encoding value true
            seq = new Sequence(Alphabets.DNA, "GGCCTT", true);
            seq.IsReadOnly = false;
            seqStatic = seq.Statistics;
            seq.ReplaceRange(0, "GG");

            Assert.AreEqual("GGCCTT", seq.ToString());
            Assert.AreEqual(2, seqStatic.GetCount(seq[0].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[1].Symbol));
            Assert.AreEqual(2, seqStatic.GetCount(seq[2].Symbol));

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of ReplaceRange() method with Encoding successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of ReplaceRange() method with Encoding successful.");
        }

        /// <summary>
        /// Validate a Basic derived sequence cloning.     
        /// Input Data : Valid RNA Sequence "GAUUCAAGGGCU".
        /// Output Data : Validation of a Basic derived clone sequence.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedCloneSequence()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.ExpectedNormalString);

            Sequence createSequence = new Sequence(
                Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Create Basic derived sequence.
            ISequence rnaDerivedSeq =
                new BasicDerivedSequence(createSequence, false, false,
                    0, createSequence.Count);

            // create a copy of basic derived sequence.
            ISequence basicDerivedCloneSeq = rnaDerivedSeq.Clone();

            // Validate cloned  sequence.
            Assert.AreEqual(basicDerivedCloneSeq, rnaDerivedSeq);
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Basic derived Sequence is '{0}' expected.", rnaDerivedSeq));

            // Validate Serialization
            Stream stream = File.Open("BasicDerivedSequence.data", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();

            string id = Guid.NewGuid().ToString();
            Sequence seq1 = new Sequence(Alphabets.RNA, "ACUGCA");
            seq1.ID = id;
            seq1.DisplayID = "displayid";
            seq1.Documentation = "document";
            BasicDerivedSequence seq = new BasicDerivedSequence(seq1, true, true, -1, -1);

            formatter.Serialize(stream, seq);
            stream.Seek(0, SeekOrigin.Begin);
            BasicDerivedSequence deserializedSeq = (BasicDerivedSequence)formatter.Deserialize(stream);

            Assert.AreNotSame(seq, deserializedSeq);
            Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);
        }

        /// <summary>
        /// Validate a Basic derived sequence CopyTo method.     
        /// Input Data : Valid RNA Sequence "GAUUCAAGGGCU".
        /// Output Data : Validation of copied sequence items
        /// </summary>
        [Test]
        public void ValidateCopiedBasicDerivedSequenceItems()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.ExpectedNormalString);
            ISequenceItem[] iSeqItems = new ISequenceItem[50];
            int index = 0;

            Sequence createSequence = new Sequence(
                Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Create Basic derived sequence.
            BasicDerivedSequence rnaDerivedSeq =
                new BasicDerivedSequence(createSequence, false, false,
                    0, createSequence.Count);

            // copy sequence items to array.
            rnaDerivedSeq.CopyTo(iSeqItems, index);

            // Validate copied ISequence items.
            for (int i = 0; i < rnaDerivedSeq.Count; i++)
            {
                Assert.AreEqual(iSeqItems[i].ToString(), rnaDerivedSeq[i].ToString());
                Assert.AreEqual(iSeqItems[i].Symbol, rnaDerivedSeq[i].Symbol);

                Console.WriteLine(string.Format(null,
                    "Qualitative Sequence P1:Qualitative Sequence {0} is as expected.",
                    rnaDerivedSeq[i].Symbol));
            }
        }

        /// <summary>
        /// Validate a Basic derived sequence clonning using IClonable interface.     
        /// Input Data : Valid RNA Sequence "GAUUCAAGGGCU".
        /// Output Data : Validation of a Basic derived clone sequence.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedCloneSequenceWithClonableObject()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = Utility._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.ExpectedNormalString);

            Sequence createSequence = new Sequence(
                Utility.GetAlphabet(alphabetName), actualSequence);
            Assert.IsNotNull(createSequence);

            // Create Basic derived sequence.
            ICloneable rnaDerivedSeq =
                new BasicDerivedSequence(createSequence, false, false,
                    0, createSequence.Count);

            // create a copy of basic derived sequence.
            object basicDerivedCloneSeq = rnaDerivedSeq.Clone();

            // Validate cloned  sequence.
            Assert.AreEqual(basicDerivedCloneSeq.ToString(),
                rnaDerivedSeq.ToString());
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P1: Basic derived Sequence is '{0}' expected.", rnaDerivedSeq));
        }

        /// <summary>
        /// Validate Sequence ctor.
        /// Input data : Sequence.
        /// Output Data : Validation of UseEncoding in Sequence class.
        /// </summary>
        [Test]
        public void ValidateSequenceCtor()
        {
            Sequence seq = new Sequence(Alphabets.DNA, "GGCCTT");
            // Set UseEncoding value true
            seq.UseEncoding = true;
            seq.IsReadOnly = false;

            seq[0] = seq[1];

            Assert.AreEqual("GGCCTT", seq.ToString());

            seq.IsReadOnly = true;
            Assert.AreEqual(true, seq.IsReadOnly);

            // Set UseEncoding value
            seq = new Sequence(Alphabets.DNA, Encodings.IupacNA, "GGCCTT");
            seq.IsReadOnly = false;
            seq[0] = seq[2];

            Assert.AreEqual("CGCCTT", seq.ToString());

            // Set Encoding value and UseEncoding as false
            seq = new Sequence(
                Alphabets.DNA,
                Encodings.IupacNA,
                "GGCCTT",
                false);
            seq.IsReadOnly = false;
            seq[0] = seq[2];

            Assert.AreEqual("CGCCTT", seq.ToString());

            // Set Encoding, Encoder, Decoder value 
            seq = new Sequence(Alphabets.DNA,
                Encodings.IupacNA,
                new SequenceEncoder(Encodings.IupacNA),
                new SequenceDecoder(Encodings.IupacNA),
                "GGCCTT");
            seq.IsReadOnly = false;
            seq[0] = seq[2];

            Assert.AreEqual("CGCCTT", seq.ToString());

            // Set Encoding, Encoder, Decoder value and UseEncoding as false
            seq = new Sequence(Alphabets.DNA,
                Encodings.IupacNA,
                new SequenceEncoder(Encodings.IupacNA),
                new SequenceDecoder(Encodings.IupacNA),
                "GGCCTT",
                false);
            seq.IsReadOnly = false;
            seq[0] = seq[2];

            Assert.AreEqual("CGCCTT", seq.ToString());

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P1: Validation of UseEncoding successful.");
            ApplicationLog.WriteLine(
                "Sequence P1: Validation of UseEncoding successful.");
        }

        /// <summary>
        /// Simple Dna Pattern convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertUnAmbiguousDnaPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("AGCT");

            Assert.AreEqual("AGCT", convertObj[0]);
        }

        /// <summary>
        /// Simple Rna Pattern convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertUnAmbiguousRnaPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(RnaAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("ACGU");

            Assert.AreEqual("ACGU", convertObj[0]);
        }

        /// <summary>
        /// Simple Protien Pattern convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertUnAmbiguousProtienPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(ProteinAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("ACDEFGH");

            Assert.AreEqual("ACDEFGH", convertObj[0]);
        }

        /// <summary>
        /// Ambiguous Dna Pattern convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertAmbiguousDnaPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("AGCTR");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGCTG");
            expected.Add("AGCTA");

            Assert.IsTrue(ValidatePattern(expected, convertObj));
        }

        /// <summary>
        /// Ambiguous Rna Pattern convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertAmbiguousRnaPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(RnaAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("AGCUM");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGCUA");
            expected.Add("AGCUC");

            Assert.IsTrue(ValidatePattern(expected, convertObj));
        }

        /// <summary>
        /// Ambiguous Protein Pattern convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertAmbiguousProteinPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(ProteinAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("ABCDEFGH");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("ADCDEFGH");
            expected.Add("ANCDEFGH");

            Assert.IsTrue(ValidatePattern(expected, convertObj));
        }

        /// <summary>
        /// Dna Pattern with square bracket convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertSquareBracketDnaPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("A[GCT]R");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGG");
            expected.Add("ACG");
            expected.Add("ATG");
            expected.Add("AGA");
            expected.Add("ACA");
            expected.Add("ATA");

            Assert.IsTrue(ValidatePattern(expected, convertObj));
        }

        /// <summary>
        /// Dna Pattern with curly bracket convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertCurlyBracketDnaPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("A{GCT}R");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AAG");
            expected.Add("AAA");

            Assert.IsTrue(ValidatePattern(expected, convertObj));
        }

        /// <summary>
        /// Dna Pattern with parenthesis convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertParenthesisDnaPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("AGC(5)TR");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGCCCCCTG");
            expected.Add("AGCCCCCTA");

            Assert.IsTrue(ValidatePattern(expected, convertObj));
        }

        /// <summary>
        /// Dna Pattern with parenthesis convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertParenthesisRangeDnaPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("AGC(2, 5)TR");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGCCTG");
            expected.Add("AGCCCTG");
            expected.Add("AGCCCCTG");
            expected.Add("AGCCCCCTG");
            expected.Add("AGCCTA");
            expected.Add("AGCCCTA");
            expected.Add("AGCCCCTA");
            expected.Add("AGCCCCCTA");

            Assert.IsTrue(ValidatePattern(expected, convertObj));
        }

        /// <summary>
        /// Dna Pattern with StartsWith convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertStartsWithDnaPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("<AGCTR");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("<AGCTG");
            expected.Add("<AGCTA");

            Assert.IsTrue(ValidatePattern(expected, convertObj));
        }

        /// <summary>
        /// Dna Pattern with EndsWith convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertEndsWithDnaPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("AGCTR>");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AGCTG>");
            expected.Add("AGCTA>");

            Assert.IsTrue(ValidatePattern(expected, convertObj));
        }

        /// <summary>
        /// Dna Pattern with Repeat convert() method validation.
        /// </summary>
        [Test]
        public void ValidateConvertRepeatDnaPattern()
        {
            IPatternConverter patternConverterObj =
                PatternConverter.GetInstanace(DnaAlphabet.Instance);
            IList<string> convertObj = patternConverterObj.Convert("AG*CTR");

            HashSet<string> expected = new HashSet<string>();
            expected.Add("AG*CTG");
            expected.Add("AG*CTA");

            Assert.IsTrue(ValidatePattern(expected, convertObj));
        }

        #endregion Sequence P1 TestCases

        #region Supporting Methods

        /// <summary>
        /// Validate Sequence Index with Non Gap Characters.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="IsPos">Value of the position to be returned</param>
        /// </summary>
        static void ValidateGeneralIndexOfNonGapChars(string nodeName,
            BasicSeqInformationMethodParameters methodName, bool IsPos)
        {

            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.ExpectedNormalString);
            string sequence1 = Utility._xmlUtil.GetTextValue(nodeName,
               Constants.Sequence1);
            string expectedSeqPosWithNonGapChar = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SeqPosWithNonGapCharNode);
            string expectedSeqPosWithGapChar = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SeqPosWithGapCharNode);
            int pos;
            int posIndex;
            int posWithGap;

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);
            Sequence seqWithGapChar = new Sequence(Utility.GetAlphabet(alphabetName), sequence1);

            switch (methodName)
            {
                case BasicSeqInformationMethodParameters.Index:

                    // Get a index of first non gap character in sequence with no gap characters.
                    pos = BasicSequenceInfo.IndexOfNonGap(seqWithGapChar);

                    // Get a index of first non gap character in sequence with Sequence starts from Gap char.
                    posWithGap = BasicSequenceInfo.IndexOfNonGap(seq);

                    // Validate Index of non gap characters.
                    Assert.AreEqual(expectedSeqPosWithNonGapChar, pos.ToString());
                    Assert.AreEqual(expectedSeqPosWithGapChar, posWithGap.ToString());

                    if (IsPos)
                    {
                        posIndex = BasicSequenceInfo.IndexOfNonGap(seqWithGapChar,
                            int.Parse(expectedSeqPosWithGapChar));
                        Assert.AreEqual(expectedSeqPosWithGapChar, posIndex.ToString());
                    }
                    ApplicationLog.WriteLine(string.Format(null,
                        "BasicSequenceInfo :Validated Sequence first Index successfully {0} ", pos));
                    Console.WriteLine(string.Format(null,
                       "BasicSequenceInfo :Validated Sequence first Index successfully {0} ", pos));
                    break;
                case BasicSeqInformationMethodParameters.LastIndex:

                    // Get a index of last non gap character in sequence with no gap characters.
                    pos = BasicSequenceInfo.LastIndexOfNonGap(seqWithGapChar);

                    // Get a index of last non gap character in sequence with Sequence starts from Gap char.
                    posWithGap = BasicSequenceInfo.LastIndexOfNonGap(seq);

                    // Validate Index of non gap characters.
                    Assert.AreEqual(expectedSeqPosWithNonGapChar, pos.ToString());
                    Assert.AreEqual(expectedSeqPosWithGapChar, posWithGap.ToString());

                    if (IsPos)
                    {
                        posIndex = BasicSequenceInfo.LastIndexOfNonGap(seqWithGapChar,
                            int.Parse(expectedSeqPosWithGapChar));
                        Assert.AreEqual(expectedSeqPosWithGapChar, posIndex.ToString());
                    }
                    ApplicationLog.WriteLine(string.Format(null,
                        "BasicSequenceInfo :Validated Sequence Last Index successfully {0} ", pos));
                    Console.WriteLine(string.Format(null,
                        "BasicSequenceInfo :Validated Sequence Last Index successfully {0} ", pos));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Compares the expected and actual values and return true if they match
        /// otherwise return false.
        /// </summary>
        /// <param name="expected">Expected values</param>
        /// <param name="actual">Actual values</param>
        /// <returns>Is match</returns>
        static bool ValidatePattern(HashSet<string> expected, IList<string> actual)
        {
            if (expected.Count != actual.Count)
            {
                return false;
            }

            foreach (string result in actual)
            {
                if (!expected.Contains(result))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion Supporting Methods
    }
}