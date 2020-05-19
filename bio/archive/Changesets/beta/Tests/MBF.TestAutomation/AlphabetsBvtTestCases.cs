// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * AlphabetsBvtTestCases.cs
 * 
 * This file contains the Alphabets BVT test cases.
 * 
******************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using MBF.Util.Logging;
using MBF.TestAutomation.Util;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.Encoding;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Alphabets and BVT level validations.
    /// </summary>
    [TestClass]
    public class AlphabetsBvtTestCases
    {
        #region Constants

        const string AddInsFolder = "\\Add-ins";
        const string MBFTestAutomationDll = "\\MBF.TestAutomation.dll";

        #endregion Constants

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static AlphabetsBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region DNA Alphabets Bvt TestCases

        /// <summary>
        /// Validate of Add() method for the Dna Alphabets.
        /// Input Data : Valid Dna Alphabet.
        /// Output Data : Validate if Read-only is enabled.
        /// </summary>
        /// Suppressing the Error "DoNotCatchGeneralExceptionTypes" because the exception is being thrown by DEV code
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaAlphabetAdd()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.AlphabetNameNode);
            string actualSequence = _utilityObj._xmlUtil.GetTextValue(Constants.SimpleDnaAlphabetNode,
                Constants.ExpectedSingleChar);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            DnaAlphabet alp = DnaAlphabet.Instance;

            try
            {
                alp.Add(seq[0]);
                Assert.Fail();
            }
            catch (Exception)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine("Alphabets BVT: Validation of Add() method completed successfully.");
                Console.WriteLine("Alphabets BVT: Validation of Add() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of Clear() method for the Dna Alphabets.
        /// Input Data : Valid Dna Alphabet.
        /// Output Data : Validate if Read-only is enabled.
        /// </summary>
        /// Suppressing the Error "DoNotCatchGeneralExceptionTypes" because the exception is being thrown by DEV code
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaAlphabetClear()
        {
            DnaAlphabet alp = DnaAlphabet.Instance;

            try
            {
                alp.Clear();
                Assert.Fail();
            }
            catch (Exception)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine("Alphabets BVT: Validation of Clear() method completed successfully.");
                Console.WriteLine("Alphabets BVT: Validation of Clear() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of CopyTo() method for the Dna Alphabets.
        /// Input Data : Valid Dna Alphabet.
        /// Output Data : Validate if CopyTo is is validated as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaAlphabetCopyTo()
        {
            ISequence seq = new Sequence(Alphabets.DNA, "AAAAAAAAAAAAAAAA");
            DnaAlphabet alp = DnaAlphabet.Instance;
            ISequenceItem[] item = seq.ToArray<ISequenceItem>();
            alp.CopyTo(item, 0);
            Assert.AreEqual('A', item[0].Symbol);
            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of CopyTo() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of CopyTo() method completed successfully.");
        }

        /// <summary>
        /// Validate of GetBasicSymbols() method for the Dna Alphabets.
        /// Input Data : Valid Dna Alphabet.
        /// Output Data : Validate if GetBasicSymbols() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaAlphabetGetBasicSymbols()
        {
            ISequence seq = new Sequence(Alphabets.DNA, "AGCTB");
            DnaAlphabet alp = DnaAlphabet.Instance;

            ISequenceItem item = seq[4];
            Assert.IsNotNull(alp.GetBasicSymbols(item));

            item = seq[3];
            Assert.IsNotNull(alp.GetBasicSymbols(item));

            item = seq[3];
            Assert.IsNotNull(alp.GetBasicSymbols(item));

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of GetBasicSymbols() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of GetBasicSymbols() method completed successfully.");
        }

        /// <summary>
        /// Validate of GetConsensusSymbol() method for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if GetConsensusSymbol() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaAlphabetGetConsensusSymbol()
        {
            ISequence seq = new Sequence(Alphabets.DNA, "ATGCA");
            DnaAlphabet alp = DnaAlphabet.Instance;

            HashSet<ISequenceItem> hashSet = new HashSet<ISequenceItem>();

            foreach (ISequenceItem item in seq)
            {
                hashSet.Add(item);
            }

            Assert.IsNotNull(alp.GetConsensusSymbol(hashSet));

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of GetBasicSymbols() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of GetBasicSymbols() method completed successfully.");
        }

        /// <summary>
        /// Validate of LookupBySymbol() method for the Dna Alphabets.
        /// Input Data : Valid Dna Alphabet.
        /// Output Data : Validate if LookupBySymbol() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaAlphabetLookupBySymbol()
        {
            DnaAlphabet alp = DnaAlphabet.Instance;
            ISequenceItem itm = alp.LookupBySymbol("A");

            Assert.AreEqual('A', itm.Symbol);

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of LookupBySymbol() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of LookupBySymbol() method completed successfully.");
        }

        /// <summary>
        /// Validate of LookupAll() method for the Dna Alphabets.
        /// Input Data : Valid Dna Alphabet.
        /// Output Data : Validate if LookupAll() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaAlphabetLookupAll()
        {
            DnaAlphabet alp = DnaAlphabet.Instance;
            List<ISequenceItem> itm = alp.LookupAll(true, true, true, true);

            Assert.AreEqual(16, itm.Count);

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of LookupAll() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of LookupAll() method completed successfully.");
        }

        /// <summary>
        /// Validate of all properties for the Dna Alphabets.
        /// Input Data : Valid Dna Alphabet.
        /// Output Data : Validate if all properties is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaAlphabetAllProperties()
        {
            DnaAlphabet alp = DnaAlphabet.Instance;
            Assert.AreEqual(16, alp.Count);
            Assert.AreEqual('-', alp.DefaultGap.Symbol);
            Assert.IsTrue(alp.HasAmbiguity);
            Assert.IsTrue(alp.HasGaps);
            Assert.IsFalse(alp.HasTerminations);
            Assert.IsTrue(alp.IsReadOnly);
            Assert.AreEqual("DNA", alp.Name);

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of All properties completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of All properties method completed successfully.");
        }

        /// <summary>
        /// Validate of Alphabet() static constructor.
        /// Input Data : Valid Dna Alphabet.
        /// Output Data : Validate Sequences.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void AlphabetStaticCtorValidate()
        {
            CreateAddinsFolder();

            Sequence seq =
                new Sequence(Alphabets.DNA, Encodings.Ncbi2NA, "ATAGC");
            Assert.AreEqual(seq.Count, 5);
            Assert.AreEqual(seq.ToString(), "ATAGC");

            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of Static Constructor completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of Static Constructor method completed successfully.");

            DeleteAddinsFolder();
        }

        #endregion DNA Alphabets Bvt TestCases

        #region Protein Alphabets Bvt TestCases

        /// <summary>
        /// Validate of Add() method for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if Read-only is enabled.
        /// </summary>
        /// Suppressing the Error "DoNotCatchGeneralExceptionTypes" because the exception is being thrown by DEV code
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateProteinAlphabetAdd()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleProteinAlphabetNode, Constants.ExpectedSingleChar);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            ProteinAlphabet alp = ProteinAlphabet.Instance;

            try
            {
                alp.Add(seq[0]);
                Assert.Fail();
            }
            catch (Exception)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets BVT: Validation of Add() method completed successfully.");
                Console.WriteLine(
                    "Alphabets BVT: Validation of Add() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of Clear() method for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if Read-only is enabled.
        /// </summary>
        /// Suppressing the Error "DoNotCatchGeneralExceptionTypes" because the exception is being thrown by DEV code
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateProteinAlphabetClear()
        {
            ProteinAlphabet alp = ProteinAlphabet.Instance;

            try
            {
                alp.Clear();
                Assert.Fail();
            }
            catch (Exception)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets BVT: Validation of Clear() method completed successfully.");
                Console.WriteLine(
                    "Alphabets BVT: Validation of Clear() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of CopyTo() method for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if CopyTo is is validated as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateProteinAlphabetCopyTo()
        {
            ISequence seq = new Sequence(Alphabets.Protein, "AAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            ProteinAlphabet alp = ProteinAlphabet.Instance;
            ISequenceItem[] item = seq.ToArray<ISequenceItem>();
            alp.CopyTo(item, 0);
            Assert.AreEqual('A', item[0].Symbol);
            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of CopyTo() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of CopyTo() method completed successfully.");
        }

        /// <summary>
        /// Validate of GetBasicSymbols() method for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if GetBasicSymbols() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateProteinAlphabetGetBasicSymbols()
        {
            ISequence seq = new Sequence(Alphabets.Protein, "AGCTX");
            ProteinAlphabet alp = ProteinAlphabet.Instance;

            ISequenceItem item = seq[4];
            Assert.IsNotNull(alp.GetBasicSymbols(item));

            item = seq[3];
            Assert.IsNotNull(alp.GetBasicSymbols(item));

            item = seq[3];
            Assert.IsNotNull(alp.GetBasicSymbols(item));

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of GetBasicSymbols() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of GetBasicSymbols() method completed successfully.");
        }

        /// <summary>
        /// Validate of GetConsensusSymbol() method for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if GetConsensusSymbol() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateProteinAlphabetGetConsensusSymbol()
        {
            ISequence seq = new Sequence(Alphabets.Protein, "AGCTA");
            ProteinAlphabet alp = ProteinAlphabet.Instance;

            HashSet<ISequenceItem> hashSet = new HashSet<ISequenceItem>();

            foreach (ISequenceItem item in seq)
            {
                hashSet.Add(item);
            }

            Assert.IsNotNull(alp.GetConsensusSymbol(hashSet));

            seq = new Sequence(Alphabets.Protein, "AGCTX");
            hashSet = new HashSet<ISequenceItem>();

            foreach (ISequenceItem item in seq)
            {
                hashSet.Add(item);
            }

            Assert.IsNotNull(alp.GetConsensusSymbol(hashSet));

            seq = new Sequence(Alphabets.Protein, "-");
            hashSet = new HashSet<ISequenceItem>();

            foreach (ISequenceItem item in seq)
            {
                hashSet.Add(item);
            }

            Assert.IsNotNull(alp.GetConsensusSymbol(hashSet));

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of GetBasicSymbols() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of GetBasicSymbols() method completed successfully.");
        }

        /// <summary>
        /// Validate of LookupBySymbol() method for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if LookupBySymbol() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateProteinAlphabetLookupBySymbol()
        {
            ProteinAlphabet alp = ProteinAlphabet.Instance;
            ISequenceItem itm = alp.LookupBySymbol("A");

            Assert.AreEqual('A', itm.Symbol);

            itm = alp.LookupBySymbol("Ala");
            Assert.AreEqual('A', itm.Symbol);

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of LookupBySymbol() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of LookupBySymbol() method completed successfully.");
        }

        /// <summary>
        /// Validate of LookupAll() method for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if LookupAll() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateProteinAlphabetLookupAll()
        {
            ProteinAlphabet alp = ProteinAlphabet.Instance;
            List<ISequenceItem> itm = alp.LookupAll(true, true, true, true);

            Assert.AreEqual(28, itm.Count);

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of LookupAll() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of LookupAll() method completed successfully.");
        }

        /// <summary>
        /// Validate of all properties for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if all properties is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateProteinAlphabetAllProperties()
        {
            ProteinAlphabet alp = ProteinAlphabet.Instance;
            Assert.AreEqual(28, alp.Count);
            Assert.AreEqual('-', alp.DefaultGap.Symbol);
            Assert.IsTrue(alp.HasAmbiguity);
            Assert.IsTrue(alp.HasGaps);
            Assert.IsTrue(alp.HasTerminations);
            Assert.IsTrue(alp.IsReadOnly);
            Assert.AreEqual("Protein", alp.Name);

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of All properties completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of All properties method completed successfully.");
        }

        #endregion Protein Alphabets Bvt TestCases

        #region Rna Alphabets Bvt TestCases

        /// <summary>
        /// Validate of Add() method for the Rna Alphabets.
        /// Input Data : Valid Rna Alphabet.
        /// Output Data : Validate if Read-only is enabled.
        /// </summary>
        /// Suppressing the Error "DoNotCatchGeneralExceptionTypes" because the exception is being thrown by DEV code
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRnaAlphabetAdd()
        {
            // Gets the actual sequence and the alphabet from the Xml
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.AlphabetNameNode);
            string actualSequence = _utilityObj._xmlUtil.GetTextValue(
                Constants.SimpleRnaAlphabetNode, Constants.ExpectedSingleChar);

            ISequence seq = new Sequence(Utility.GetAlphabet(alphabetName), actualSequence);
            RnaAlphabet alp = RnaAlphabet.Instance;

            try
            {
                alp.Add(seq[0]);
                Assert.Fail();
            }
            catch (Exception)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets BVT: Validation of Add() method completed successfully.");
                Console.WriteLine(
                    "Alphabets BVT: Validation of Add() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of Clear() method for the Rna Alphabets.
        /// Input Data : Valid Rna Alphabet.
        /// Output Data : Validate if Read-only is enabled.
        /// </summary>
        /// Suppressing the Error "DoNotCatchGeneralExceptionTypes" because the exception is being thrown by DEV code
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRnaAlphabetClear()
        {
            RnaAlphabet alp = RnaAlphabet.Instance;

            try
            {
                alp.Clear();
                Assert.Fail();
            }
            catch (Exception)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets BVT: Validation of Clear() method completed successfully.");
                Console.WriteLine(
                    "Alphabets BVT: Validation of Clear() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of CopyTo() method for the Rna Alphabets.
        /// Input Data : Valid Rna Alphabet.
        /// Output Data : Validate if CopyTo is is validated as expected.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRnaAlphabetCopyTo()
        {
            ISequence seq = new Sequence(Alphabets.RNA, "AAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            RnaAlphabet alp = RnaAlphabet.Instance;
            ISequenceItem[] item = seq.ToArray<ISequenceItem>();
            alp.CopyTo(item, 0);
            Assert.AreEqual('A', item[0].Symbol);
            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of CopyTo() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of CopyTo() method completed successfully.");
        }

        /// <summary>
        /// Validate of GetBasicSymbols() method for the Rna Alphabets.
        /// Input Data : Valid Rna Alphabet.
        /// Output Data : Validate if GetBasicSymbols() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRnaAlphabetGetBasicSymbols()
        {
            ISequence seq = new Sequence(Alphabets.RNA, "AGCUB");
            RnaAlphabet alp = RnaAlphabet.Instance;

            ISequenceItem item = seq[4];
            Assert.IsNotNull(alp.GetBasicSymbols(item));

            item = seq[3];
            Assert.IsNotNull(alp.GetBasicSymbols(item));

            item = seq[3];
            Assert.IsNotNull(alp.GetBasicSymbols(item));

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of GetBasicSymbols() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of GetBasicSymbols() method completed successfully.");
        }

        /// <summary>
        /// Validate of LookupBySymbol() method for the Rna Alphabets.
        /// Input Data : Valid Rna Alphabet.
        /// Output Data : Validate if LookupBySymbol() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRnaAlphabetLookupBySymbol()
        {
            RnaAlphabet alp = RnaAlphabet.Instance;
            ISequenceItem itm = alp.LookupBySymbol("A");

            Assert.AreEqual('A', itm.Symbol);

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of LookupBySymbol() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of LookupBySymbol() method completed successfully.");
        }

        /// <summary>
        /// Validate of LookupAll() method for the Rna Alphabets.
        /// Input Data : Valid Rna Alphabet.
        /// Output Data : Validate if LookupAll() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRnaAlphabetLookupAll()
        {
            RnaAlphabet alp = RnaAlphabet.Instance;
            List<ISequenceItem> itm = alp.LookupAll(true, true, true, true);

            Assert.AreEqual(16, itm.Count);

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of LookupAll() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of LookupAll() method completed successfully.");
        }

        /// <summary>
        /// Validate of all properties for the Rna Alphabets.
        /// Input Data : Valid Rna Alphabet.
        /// Output Data : Validate if all properties is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRnaAlphabetAllProperties()
        {
            RnaAlphabet alp = RnaAlphabet.Instance;
            Assert.AreEqual(16, alp.Count);
            Assert.AreEqual('-', alp.DefaultGap.Symbol);
            Assert.IsTrue(alp.HasAmbiguity);
            Assert.IsTrue(alp.HasGaps);
            Assert.IsFalse(alp.HasTerminations);
            Assert.IsTrue(alp.IsReadOnly);
            Assert.AreEqual("RNA", alp.Name);

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of All properties completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of All properties method completed successfully.");
        }

        /// <summary>
        /// Validate of GetConsensusSymbol() method for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if GetConsensusSymbol() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRnaAlphabetGetConsensusSymbol()
        {
            ISequence seq = new Sequence(Alphabets.RNA, "CGUGA");
            RnaAlphabet alp = RnaAlphabet.Instance;

            HashSet<ISequenceItem> hashSet = new HashSet<ISequenceItem>();

            foreach (ISequenceItem item in seq)
            {
                hashSet.Add(item);
            }

            Assert.IsNotNull(alp.GetConsensusSymbol(hashSet));

            // Logs to the NUnit GUI window
            ApplicationLog.WriteLine(
                "Alphabets BVT: Validation of GetBasicSymbols() method completed successfully.");
            Console.WriteLine(
                "Alphabets BVT: Validation of GetBasicSymbols() method completed successfully.");
        }

        #endregion Rna Alphabets Bvt TestCases

        #region Helper Method

        /// <summary>
        /// Creates the Add-ins folder
        /// </summary>
        private static void CreateAddinsFolder()
        {
            // Gets the Add-ins folder name
            Uri uri = new Uri(Assembly.GetCallingAssembly().CodeBase);
            string addInsFolderPath = Uri.UnescapeDataString(string.Concat(
                Path.GetDirectoryName(uri.AbsolutePath),
                AddInsFolder));

            if (!Directory.Exists(addInsFolderPath))
                // Creates the Add-ins folder
                Directory.CreateDirectory(addInsFolderPath);

            // Copies the MBF.TestAutomation.dll to Add-ins folder
            File.Copy(Uri.UnescapeDataString(uri.AbsolutePath),
                string.Concat(addInsFolderPath, MBFTestAutomationDll), false);
        }

        /// <summary>
        /// Deletes the Add-ins folder if exists
        /// </summary>
        private static void DeleteAddinsFolder()
        {
            Uri uri = new Uri(Assembly.GetCallingAssembly().CodeBase);
            string addInsFolderPath = string.Concat(
                Path.GetDirectoryName(uri.AbsolutePath),
                AddInsFolder);

            // If the Add-ins folder exists delete the same
            if (Directory.Exists(addInsFolderPath))
                Directory.Delete(addInsFolderPath, true);
        }

        #endregion Helper Method
    }
}