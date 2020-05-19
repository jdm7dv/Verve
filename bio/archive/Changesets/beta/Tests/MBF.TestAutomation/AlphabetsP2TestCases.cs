// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * AlphabetsP2TestCases.cs
 * 
 * This file contains the Alphabets P2 test cases.
 * 
******************************************************************************/

using System;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Alphabets and P2 level validations.
    /// </summary>
    [TestClass]
    public class AlphabetsP2TestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static AlphabetsP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Dna Alphabets P2 TestCases

        /// <summary>
        /// Validate of GetBasicSymbols() property for the Dna Alphabets.
        /// Input Data : Valid Dna Alphabet.
        /// Output Data : Validate if GetBasicSymbols() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateDnaAlphabetGetBasicSymbols()
        {
            DnaAlphabet alp = DnaAlphabet.Instance;

            try
            {
                alp.GetBasicSymbols(null);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets P2: Validation of GetBasicSymbols() method completed successfully.");
                Console.WriteLine(
                    "Alphabets P2: Validation of GetBasicSymbols() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of LookupBySymbol() property for the Dna Alphabets.
        /// Input Data : Valid Dna Alphabet.
        /// Output Data : Validate if LookupBySymbol() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateDnaAlphabetLookupBySymbol()
        {
            DnaAlphabet alp = DnaAlphabet.Instance;
            try
            {
                ISequenceItem itm = alp.LookupBySymbol("");
                Assert.IsNotNull(itm);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets P2: Validation of LookupBySymbol() method completed successfully.");
                Console.WriteLine(
                    "Alphabets P2: Validation of LookupBySymbol() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of Remove() method for the ISequenceItem.
        /// Input Data : null as Item.
        /// Output Data : Validatation of Exception.
        /// </summary>
        /// Suppressing the Error "DoNotCatchGeneralExceptionTypes" because the exception is being thrown by DEV code
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateDnaAlphabetRemove()
        {
            try
            {
                DnaAlphabet alp = DnaAlphabet.Instance;
                alp.Remove(null as ISequenceItem);
                Assert.Fail();
            }
            catch (Exception)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets P2: Validation of Remove() method completed successfully.");
                Console.WriteLine(
                    "Alphabets P2: Validation of Remove() method completed successfully.");
            }
        }

        #endregion Dna Alphabets P2 TestCases

        #region Protein Alphabets P2 TestCases

        /// <summary>
        /// Validate of GetBasicSymbols() property for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if GetBasicSymbols() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateProteinAlphabetGetBasicSymbols()
        {
            ProteinAlphabet alp = ProteinAlphabet.Instance;

            try
            {
                alp.GetBasicSymbols(null);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets P2: Validation of GetBasicSymbols() method completed successfully.");
                Console.WriteLine(
                    "Alphabets P2: Validation of GetBasicSymbols() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of LookupBySymbol() property for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if LookupBySymbol() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateProteinAlphabetLookupBySymbol()
        {
            ProteinAlphabet alp = ProteinAlphabet.Instance;
            try
            {
                ISequenceItem itm = alp.LookupBySymbol("");
                Assert.IsNotNull(itm);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets P2: Validation of LookupBySymbol() method completed successfully.");
                Console.WriteLine(
                    "Alphabets P2: Validation of LookupBySymbol() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of Remove() method for the ISequenceItem.
        /// Input Data : null as Item.
        /// Output Data : Validatation of Exception.
        /// </summary>
        /// Suppressing the Error "DoNotCatchGeneralExceptionTypes" because the exception is being thrown by DEV code
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateProteinAlphabetRemove()
        {
            try
            {
                ProteinAlphabet alp = ProteinAlphabet.Instance;
                alp.Remove(null as ISequenceItem);
                Assert.Fail();
            }
            catch (Exception)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets P2: Validation of Remove() method completed successfully.");
                Console.WriteLine(
                    "Alphabets P2: Validation of Remove() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of GetConsensusSymbol() property for the Protein Alphabets.
        /// Input Data : Valid Protein Alphabet.
        /// Output Data : Validate if GetConsensusSymbol() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateProteinAlphabetGetConsensusSymbol()
        {
            ProteinAlphabet alp = ProteinAlphabet.Instance;

            try
            {
                HashSet<ISequenceItem> hashSet = new HashSet<ISequenceItem>();
                hashSet.Add(null);
                alp.GetConsensusSymbol(hashSet);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets P2: Validation of GetConsensusSymbol() method completed successfully.");
                Console.WriteLine(
                    "Alphabets P2: Validation of GetConsensusSymbol() method completed successfully.");
            }
        }

        #endregion Protein Alphabets P2 TestCases

        #region Rna Alphabets P2 TestCases

        /// <summary>
        /// Validate of GetBasicSymbols() property for the Rna Alphabets.
        /// Input Data : Valid Rna Alphabet.
        /// Output Data : Validate if GetBasicSymbols() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateRnaAlphabetGetBasicSymbols()
        {
            RnaAlphabet alp = RnaAlphabet.Instance;

            try
            {
                alp.GetBasicSymbols(null);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets P2: Validation of GetBasicSymbols() method completed successfully.");
                Console.WriteLine(
                    "Alphabets P2: Validation of GetBasicSymbols() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of LookupBySymbol() property for the Rna Alphabets.
        /// Input Data : Valid Rna Alphabet.
        /// Output Data : Validate if LookupBySymbol() method is returning valid value.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateRnaAlphabetLookupBySymbol()
        {
            RnaAlphabet alp = RnaAlphabet.Instance;

            try
            {
                ISequenceItem itm = alp.LookupBySymbol("");
                Assert.IsNotNull(itm);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets P2: Validation of LookupBySymbol() method completed successfully.");
                Console.WriteLine(
                    "Alphabets P2: Validation of LookupBySymbol() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of Remove() method for the ISequenceItem.
        /// Input Data : null as Item.
        /// Output Data : Validatation of Exception.
        /// </summary>
        /// Suppressing the Error "DoNotCatchGeneralExceptionTypes" because the exception is being thrown by DEV code
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes"), TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateRnaAlphabetRemove()
        {
            try
            {
                RnaAlphabet alp = RnaAlphabet.Instance;
                alp.Remove(null as ISequenceItem);
                Assert.Fail();
            }
            catch (Exception)
            {
                // Logs to the NUnit GUI window
                ApplicationLog.WriteLine(
                    "Alphabets P2: Validation of Remove() method completed successfully.");
                Console.WriteLine(
                    "Alphabets P2: Validation of Remove() method completed successfully.");
            }
        }

        /// <summary>
        /// Validate of Contains() method for the ISequenceItem.
        /// Input Data : null as Item.
        /// Output Data : Validatation of Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        [TestCategory("Priority2")]
        public void InValidateRnaAlphabetContains()
        {
            RnaAlphabet alp = RnaAlphabet.Instance;
            Assert.AreEqual(false, alp.Contains(null as ISequenceItem));
            ISequence seq = new Sequence(Alphabets.RNA, "AGCUB");
            Assert.AreEqual(true, alp.Contains(seq[4]));

            ApplicationLog.WriteLine(
                "Alphabets P2: Validation of Contains() method completed successfully.");
            Console.WriteLine(
                "Alphabets P2: Validation of Contains() method completed successfully.");
        }

        #endregion Rna Alphabets P2 TestCases
    }
}