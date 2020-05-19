// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * RegistrationBvtTestCases.cs
 * 
 *   This file contains the BVT test cases for validation the
 *   registration process in MBF
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using MBF.Algorithms.Alignment;
using MBF.Algorithms.Assembly;
using MBF.IO;
using MBF.Registration;
using MBF.SimilarityMatrices;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation.Registration
{

    /// <summary>
    /// Registration BVT Test case implementation.
    /// </summary>
    [TestClass]
    public class RegistrationBvtTestCases
    {

        #region Constants

        const string AddInsFolder = "\\Add-ins";
        const string MBFTestAutomationDll = "\\MBF.TestAutomation.dll";

        #endregion Constants

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static RegistrationBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Initilization/Cleanup

        [ClassInitialize()]
        public static void RegistrationBvtInitialize(TestContext testContext)
        {
            CreateAddinsFolder();
        }

        [ClassCleanup()]
        public static void RegistrationBvtCleanUp()
        {
            DeleteAddinsFolder();
        }

        #endregion Initilization/Cleanup

        #region Register Addins BVT Test cases

        /// <summary>
        /// Validates Registered Aligners.
        /// Input : Register Two Aligners.
        /// Validation : Validate the Aligners Registered.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void RegisterAddinsRegisterAligner()
        {
            IList<ISequenceAligner> finalValue = new List<ISequenceAligner>();
            finalValue.Add(new TestAutomationSequenceAligner());
            finalValue.Add(new TestAutomationPairwiseSequenceAligner());

            // Gets the registered Aligners
            IList<ISequenceAligner> registeredAligners = GetClasses<ISequenceAligner>(true);
            RegisterAlignGeneralTestCases(registeredAligners, finalValue);
        }

        /// <summary>
        /// Validates Registered Assemblies.
        /// Input : Register One Assembly.
        /// Validation : Validate the Assembly Registered.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void RegisterAddinsRegisterAssembly()
        {
            IList<IDeNovoAssembler> finalValue = new List<IDeNovoAssembler>();
            finalValue.Add(new TestAutomationSequenceAssembler());

            // Gets the registered Assemblers
            IList<IDeNovoAssembler> registeredAssemblers = GetClasses<IDeNovoAssembler>(true);
            if (null != registeredAssemblers && registeredAssemblers.Count > 0)
            {
                foreach (IDeNovoAssembler assembler in finalValue)
                {
                    string name = string.Empty;
                    string description = string.Empty;

                    registeredAssemblers.FirstOrDefault(IA => string.Compare(name = IA.Name,
                        assembler.Name, StringComparison.OrdinalIgnoreCase) == 0);
                    registeredAssemblers.FirstOrDefault(IA => string.Compare(description = IA.Description,
                        assembler.Description, StringComparison.OrdinalIgnoreCase) == 0);

                    // Validates the Name and Description
                    Assert.AreEqual(assembler.Name, name);
                    Assert.AreEqual(assembler.Description, description);
                    Console.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered components for Assembly '{0}'.",
                        name));
                    ApplicationLog.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered components for Assembly '{0}'.",
                        name));
                }
            }
            else
            {
                Console.WriteLine("No Components to Register.");
                ApplicationLog.WriteLine("No Components to Register.");
                Assert.Fail();
            }
        }

        /// <summary>
        /// Validates Registered Alphabets.
        /// Input : Register One Alphabet.
        /// Validation : Validate the Alphabet Registered.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void RegisterAddinsRegisterAlphabet()
        {
            IList<IAlphabet> finalValue = new List<IAlphabet>();
            finalValue.Add(new TestAutomationAlphabet());

            // Gets the registered Alphabets
            IList<IAlphabet> registeredAlphabets = RegisteredAddIn.GetAlphabets(true);
            if (null != registeredAlphabets && registeredAlphabets.Count > 0)
            {
                foreach (IAlphabet alphabet in finalValue)
                {
                    string name = string.Empty;

                    registeredAlphabets.FirstOrDefault(IA => string.Compare(name = IA.Name,
                        alphabet.Name, StringComparison.OrdinalIgnoreCase) == 0);

                    // Validates the Name
                    Assert.AreEqual(alphabet.Name, name);
                    Console.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered components for Alphabet '{0}'.",
                        name));
                    ApplicationLog.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered components for Alphabet '{0}'.",
                        name));
                }
            }
            else
            {
                Console.WriteLine("No Components to Register.");
                ApplicationLog.WriteLine("No Components to Register.");
                Assert.Fail();
            }
        }

        /// <summary>
        /// Validates Registered Formatter.
        /// Input : Register One Formatter.
        /// Validation : Validate the Formatter Registered.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void RegisterAddinsRegisterFormatter()
        {
            IList<ISequenceFormatter> finalValue = new List<ISequenceFormatter>();
            finalValue.Add(new TestAutomationSequenceFormatter());

            // Gets the registered formatters
            IList<ISequenceFormatter> registeredFormatters = GetClasses<ISequenceFormatter>(true);
            if (null != registeredFormatters && registeredFormatters.Count > 0)
            {
                foreach (ISequenceFormatter formatter in finalValue)
                {
                    string name = string.Empty;
                    string description = string.Empty;

                    registeredFormatters.FirstOrDefault(IA => string.Compare(name = IA.Name,
                        formatter.Name, StringComparison.OrdinalIgnoreCase) == 0);
                    registeredFormatters.FirstOrDefault(IA => string.Compare(description = IA.Description,
                        formatter.Description, StringComparison.OrdinalIgnoreCase) == 0);

                    // Validates the Name and Description
                    Assert.AreEqual(formatter.Name, name);
                    Assert.AreEqual(formatter.Description, description);
                    Console.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered components for Formatter '{0}'.",
                        name));
                    ApplicationLog.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered components for Formatter '{0}'.",
                        name));
                }
            }
            else
            {
                Console.WriteLine("No Components to Register.");
                ApplicationLog.WriteLine("No Components to Register.");
                Assert.Fail();
            }
        }

        /// <summary>
        /// Validates Registered Parsers.
        /// Input : Register One Parser.
        /// Validation : Validate the Parser Registered.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void RegisterAddinsRegisterParser()
        {
            IList<ISequenceParser> finalValue = new List<ISequenceParser>();
            finalValue.Add(new TestAutomationSequenceParser());

            IList<ISequenceParser> registeredParsers = GetClasses<ISequenceParser>(true);
            if (null != registeredParsers && registeredParsers.Count > 0)
            {
                foreach (ISequenceParser parser in finalValue)
                {
                    string name = string.Empty;
                    string description = string.Empty;

                    registeredParsers.FirstOrDefault(IA => string.Compare(name = IA.Name,
                        parser.Name, StringComparison.OrdinalIgnoreCase) == 0);
                    registeredParsers.FirstOrDefault(IA => string.Compare(description = IA.Description,
                        parser.Description, StringComparison.OrdinalIgnoreCase) == 0);

                    // Validates the Name and Description
                    Assert.AreEqual(parser.Name, name);
                    Assert.AreEqual(parser.Description, description);
                    Console.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered components for Parser '{0}'.",
                        name));
                    ApplicationLog.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered components for Parser '{0}'.",
                        name));
                }
            }
            else
            {
                Console.WriteLine("No Components to Register.");
                ApplicationLog.WriteLine("No Components to Register.");
                Assert.Fail();
            }
        }

        /// <summary>
        /// Validates Registered Alignment Formatter.
        /// Input : Register One Formatter.
        /// Validation : Validate the Formatter Registered.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void RegisterAddinsRegisterAlignmentFormatter()
        {
            IList<ISequenceAlignmentFormatter> finalValue = new List<ISequenceAlignmentFormatter>();
            finalValue.Add(new TestAutomationSequenceAlignmentFormatter());

            // Gets the registered formatters
            IList<ISequenceAlignmentFormatter> registeredFormatters =
                GetClasses<ISequenceAlignmentFormatter>(true);
            if (null != registeredFormatters && registeredFormatters.Count > 0)
            {
                foreach (ISequenceAlignmentFormatter formatter in finalValue)
                {
                    string name = string.Empty;
                    string description = string.Empty;

                    registeredFormatters.FirstOrDefault(IA => string.Compare(name = IA.Name,
                        formatter.Name, StringComparison.OrdinalIgnoreCase) == 0);
                    registeredFormatters.FirstOrDefault(IA => string.Compare(description = IA.Description,
                        formatter.Description, StringComparison.OrdinalIgnoreCase) == 0);

                    // Validates the Name and Description
                    Assert.AreEqual(formatter.Name, name);
                    Assert.AreEqual(formatter.Description, description);
                    Console.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered components for AlignmentFormatter '{0}'.",
                        name));
                    ApplicationLog.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered components for AlignmentFormatter '{0}'.",
                        name));
                }
            }
            else
            {
                Console.WriteLine("No Components to Register.");
                ApplicationLog.WriteLine("No Components to Register.");
                Assert.Fail();
            }
        }

        /// <summary>
        /// Validates Registered AlignmentParsers.
        /// Input : Register One Parser.
        /// Validation : Validate the Parser Registered.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void RegisterAddinsRegisterAlignmentParser()
        {
            IList<ISequenceAlignmentParser> finalValue = new List<ISequenceAlignmentParser>();
            finalValue.Add(new TestAutomationSequenceAlignmentParser());

            IList<ISequenceAlignmentParser> registeredParsers = GetClasses<ISequenceAlignmentParser>(true);
            if (null != registeredParsers && registeredParsers.Count > 0)
            {
                foreach (ISequenceAlignmentParser parser in finalValue)
                {
                    string name = string.Empty;
                    string description = string.Empty;

                    registeredParsers.FirstOrDefault(IA => string.Compare(name = IA.Name,
                        parser.Name, StringComparison.OrdinalIgnoreCase) == 0);
                    registeredParsers.FirstOrDefault(IA => string.Compare(description = IA.Description,
                        parser.Description, StringComparison.OrdinalIgnoreCase) == 0);

                    // Validates the Name and Description
                    Assert.AreEqual(parser.Name, name);
                    Assert.AreEqual(parser.Description, description);
                    Console.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered components for AlignmentParser '{0}'.",
                        name));
                    ApplicationLog.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered components for AlignmentParser '{0}'.",
                        name));
                }
            }
            else
            {
                Console.WriteLine("No Components to Register.");
                ApplicationLog.WriteLine("No Components to Register.");
                Assert.Fail();
            }
        }

        /// <summary>
        /// Validates Registered Instances.
        /// Input : Register Two Aligners.
        /// Validation : Validate the Instances Registered.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void RegisterAddinsGetInstances()
        {
            IList<ISequenceAligner> finalValue = new List<ISequenceAligner>();
            finalValue.Add(new TestAutomationSequenceAligner());
            finalValue.Add(new TestAutomationPairwiseSequenceAligner());

            // Gets the registered Instances for the path passed
            string assemblyPath = string.Concat(RegisteredAddIn.AddinFolderPath,
                MBFTestAutomationDll);
            IList<ISequenceAligner> registeredAligners =
                RegisteredAddIn.GetInstancesFromAssembly<ISequenceAligner>(assemblyPath);

            RegisterAlignGeneralTestCases(registeredAligners, finalValue);
        }

        /// <summary>
        /// Validates Registered Instances.
        /// Input : Register Two Aligners.
        /// Validation : Validate the Instances Registered.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void RegisterAddinsGetInstancesFilter()
        {
            IList<ISequenceAligner> finalValue = new List<ISequenceAligner>();
            finalValue.Add(new TestAutomationSequenceAligner());
            finalValue.Add(new TestAutomationPairwiseSequenceAligner());

            // Gets the registered Instances for the path passed and the filter
            IList<ISequenceAligner> registeredAligners =
                RegisteredAddIn.GetInstancesFromAssemblyPath<ISequenceAligner>(
                RegisteredAddIn.AddinFolderPath, "*.dll");

            RegisterAlignGeneralTestCases(registeredAligners, finalValue);
        }

        /// <summary>
        /// Validates Registered Instances.
        /// Input : Register Two Aligners.
        /// Validation : Validate the Instances Registered.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void RegisterAddinsGetInstancesExecutingAssembly()
        {
            IList<ISequenceAligner> finalValue = new List<ISequenceAligner>();
            finalValue.Add(new TestAutomationSequenceAligner());
            finalValue.Add(new TestAutomationPairwiseSequenceAligner());

            // Gets the registered Instances for the path passed
            IList<ISequenceAligner> registeredAligners =
                RegisteredAddIn.GetInstancesFromExecutingAssembly<ISequenceAligner>();

            if (0 == registeredAligners.Count)
            {
                Console.WriteLine("Referring from the MBF.dll, hence validation is not required.");
                ApplicationLog.WriteLine("Referring from the MBF.dll, hence validation is not required.");
            }
        }

        /// <summary>
        /// Validates the properties
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void RegisterAddinsAllProperties()
        {
            // Validate the property values if exists
            Assert.IsTrue(!string.IsNullOrEmpty(RegisteredAddIn.AddinFolderPath));
            Assert.IsTrue(string.IsNullOrEmpty(RegisteredAddIn.CoreFolderPath));
            Console.WriteLine(
                string.Format((IFormatProvider)null, "Successfully validate the property AddInFolderPath with value '{0}'",
                RegisteredAddIn.AddinFolderPath));
            Console.WriteLine("Successfully validate the property CoreFolderPath");
            ApplicationLog.WriteLine(
                string.Format((IFormatProvider)null, "Successfully validate the property AddInFolderPath with value '{0}'",
                RegisteredAddIn.AddinFolderPath));
            ApplicationLog.WriteLine("Successfully validate the property CoreFolderPath");
        }

        /// <summary>
        /// Validates GetAligners() method in SequenceAligners.cs which register aligner
        /// Input : Register Two Aligners.
        /// Validation : Validate the Instances Registered.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void RegisterAddinsSequenceAligners()
        {
            // Validate the property values if exists
            Assert.IsTrue(!string.IsNullOrEmpty(RegisteredAddIn.AddinFolderPath));
            Assert.IsTrue(string.IsNullOrEmpty(RegisteredAddIn.CoreFolderPath));

            bool addinAlignerCreated = false;
            foreach (ISequenceAligner seqAligner in SequenceAligners.All)
            {
                // Check if SequenceAligners are already created, 
                // if already created, then dont check for Addin description
                if (SequenceAligners.All.Count > 5)
                {
                    if (0 == string.Compare(seqAligner.Description,
                        "TestAutomation SequenceAligner Description", true,
                        System.Globalization.CultureInfo.CurrentCulture))
                    {
                        addinAlignerCreated = true;
                    }
                }
                else
                {
                    addinAlignerCreated = true;
                }
            }

            Assert.IsTrue(addinAlignerCreated);

            Console.WriteLine("Successfully registered Aligners");
            ApplicationLog.WriteLine("Successfully registered Aligners");
        }

        #endregion Register Addins BVT Test cases

        #region Registration Components

        /// <summary>
        /// Creating new aligner class which is extended from ISequenceAligner. 
        /// Also registered for auto-plugin by the registration attribute as true
        /// </summary>
        [RegistrableAttribute(true)]
        public sealed class TestAutomationSequenceAligner : ISequenceAligner
        {

            #region ISequenceAligner members

            string ISequenceAligner.Name
            {
                get { return "TestAutomation SequenceAligner"; }
            }

            string ISequenceAligner.Description
            {
                get { return "TestAutomation SequenceAligner Description"; }
            }

            IConsensusResolver ISequenceAligner.ConsensusResolver
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            SimilarityMatrix ISequenceAligner.SimilarityMatrix
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            int ISequenceAligner.GapOpenCost
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            int ISequenceAligner.GapExtensionCost
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            IList<ISequenceAlignment> ISequenceAligner.AlignSimple(IList<ISequence> inputSequences)
            {
                throw new NotImplementedException();
            }

            IList<ISequenceAlignment> ISequenceAligner.Align(IList<ISequence> inputSequences)
            {
                throw new NotImplementedException();
            }

            #endregion ISequenceAligner members
        }

        /// <summary>
        /// Creating new pairwise aligner class which is extended from IPairwiseSequenceAligner. 
        /// Also registered for auto-plugin by the registration attribute as true   
        /// </summary>
        [RegistrableAttribute(true)]
        public sealed class TestAutomationPairwiseSequenceAligner : IPairwiseSequenceAligner
        {

            #region IPairwiseSequenceAligner Members

            /// <summary>
            /// Similarity Matrix
            /// </summary>
            public SimilarityMatrix SimilarityMatrix
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// Gap open Cost
            /// </summary>
            public int GapOpenCost
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// Gap extension cost
            /// </summary>
            public int GapExtensionCost
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// Align Simple
            /// </summary>
            /// <param name="sequence1">Sequence 1</param>
            /// <param name="sequence2">Sequence 2</param>
            /// <returns>Not Implemented exception</returns>
            public IList<IPairwiseSequenceAlignment> AlignSimple(ISequence sequence1, ISequence sequence2)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// Align
            /// </summary>
            /// <param name="sequence1">Sequence 1</param>
            /// <param name="sequence2">Sequence 2</param>
            /// <returns>Not Implemented exception</returns>
            public IList<IPairwiseSequenceAlignment> Align(ISequence sequence1, ISequence sequence2)
            {
                throw new System.NotImplementedException();
            }

            #endregion

            #region ISequenceAligner Members

            /// <summary>
            /// Name of the aligner
            /// </summary>
            public string Name
            {
                get { return "TestAutomation Pairwise SequenceAligner"; }
            }

            /// <summary>
            /// Name of the description
            /// </summary>
            public string Description
            {
                get { return "TestAutomation Pairwise SequenceAligner Description"; }
            }

            /// <summary>
            /// Consensus Resolver
            /// </summary>
            public IConsensusResolver ConsensusResolver
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }


            /// <summary>
            /// Align Simple
            /// </summary>
            /// <param name="inputSequences">Input Sequences</param>
            /// <returns>Not Implemented exception</returns>
            IList<ISequenceAlignment> ISequenceAligner.AlignSimple(IList<ISequence> inputSequences)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// Align
            /// </summary>
            /// <param name="inputSequences">Input Sequences</param>
            /// <returns>Not Implemented exception</returns>
            IList<ISequenceAlignment> ISequenceAligner.Align(IList<ISequence> inputSequences)
            {
                throw new System.NotImplementedException();
            }
            #endregion
        }

        /// <summary>
        /// Creating new assembler class which is extended from IDeNovoAssembler. 
        /// Also registered for auto-plugin by the registration attribute as true
        /// </summary>
        [RegistrableAttribute(true)]
        public sealed class TestAutomationSequenceAssembler : IDeNovoAssembler
        {
            #region IDeNovoAssembler Members

            string IDeNovoAssembler.Name
            {
                get { return "TestAutomation SequenceAssembler"; }
            }

            string IDeNovoAssembler.Description
            {
                get { return "TestAutomation SequenceAssembler Description"; }
            }

            IDeNovoAssembly IDeNovoAssembler.Assemble(IList<ISequence> inputSequences)
            {
                throw new System.NotImplementedException();
            }

            #endregion
        }

        /// <summary>
        /// Creating new alphabet class which is extended from IAlphabet. 
        /// Also registered for auto-plugin by the registration attribute as true
        /// </summary>
        [RegistrableAttribute(true)]
        public sealed class TestAutomationAlphabet : IAlphabet
        {

            #region IAlphabet Members

            string IAlphabet.Name
            {
                get { return "TestAutomation Alphabet"; }
            }

            bool IAlphabet.HasGaps
            {
                get { throw new System.NotImplementedException(); }
            }

            bool IAlphabet.HasAmbiguity
            {
                get { throw new System.NotImplementedException(); }
            }

            bool IAlphabet.HasTerminations
            {
                get { throw new System.NotImplementedException(); }
            }

            ISequenceItem IAlphabet.DefaultGap
            {
                get { throw new System.NotImplementedException(); }
            }

            ISequenceItem IAlphabet.LookupBySymbol(char symbol)
            {
                throw new System.NotImplementedException();
            }

            ISequenceItem IAlphabet.LookupBySymbol(string symbol)
            {
                throw new System.NotImplementedException();
            }

            ISequenceItem IAlphabet.GetConsensusSymbol(HashSet<ISequenceItem> symbols)
            {
                throw new System.NotImplementedException();
            }

            HashSet<ISequenceItem> IAlphabet.GetBasicSymbols(ISequenceItem symbol)
            {
                throw new System.NotImplementedException();
            }

            List<ISequenceItem> IAlphabet.LookupAll(bool includeBasics, bool includeGaps, bool includeAmbiguities, bool includeTerminations)
            {
                throw new System.NotImplementedException();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
            ISequenceItem IAlphabet.LookupByValue(byte value)
            {
                throw new System.NotImplementedException();
            }

            #endregion

            #region ICollection<ISequenceItem> Members

            void ICollection<ISequenceItem>.Add(ISequenceItem item)
            {
                throw new System.NotImplementedException();
            }

            void ICollection<ISequenceItem>.Clear()
            {
                throw new System.NotImplementedException();
            }

            bool ICollection<ISequenceItem>.Contains(ISequenceItem item)
            {
                throw new System.NotImplementedException();
            }

            void ICollection<ISequenceItem>.CopyTo(ISequenceItem[] array, int arrayIndex)
            {
                throw new System.NotImplementedException();
            }

            int ICollection<ISequenceItem>.Count
            {
                get { throw new System.NotImplementedException(); }
            }

            bool ICollection<ISequenceItem>.IsReadOnly
            {
                get { throw new System.NotImplementedException(); }
            }

            bool ICollection<ISequenceItem>.Remove(ISequenceItem item)
            {
                throw new System.NotImplementedException();
            }

            #endregion

            #region IEnumerable<ISequenceItem> Members

            IEnumerator<ISequenceItem> IEnumerable<ISequenceItem>.GetEnumerator()
            {
                throw new System.NotImplementedException();
            }

            #endregion

            #region IEnumerable Members

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new System.NotImplementedException();
            }

            #endregion
        }

        /// <summary>
        /// Creating new formatter class which is extended from ISequenceFormatter. 
        /// Also registered for auto-plugin by the registration attribute as true
        /// </summary>
        [RegistrableAttribute(true)]
        public sealed class TestAutomationSequenceFormatter : ISequenceFormatter
        {

            #region ISequenceFormatter Members
            /// <summary>
            /// 
            /// </summary>
            public string Name
            {
                get { return "TestAutomation SequenceFormatter"; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string Description
            {
                get { return "TestAutomation SequenceFormatter Description"; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string FileTypes
            {
                get { throw new System.NotImplementedException(); }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sequence"></param>
            /// <param name="writer"></param>
            public void Format(ISequence sequence,
                TextWriter writer)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sequence"></param>
            /// <param name="filename"></param>
            public void Format(ISequence sequence,
                string filename)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sequences"></param>
            /// <param name="writer"></param>
            public void Format(ICollection<ISequence> sequences,
                TextWriter writer)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sequences"></param>
            /// <param name="filename"></param>
            public void Format(ICollection<ISequence> sequences,
                string filename)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="sequence"></param>
            /// <returns></returns>
            public string FormatString(ISequence sequence)
            {
                throw new System.NotImplementedException();
            }

            #endregion
        }

        /// <summary>
        /// Creating new parser class which is extended from ISequenceParser. 
        /// Also registered for auto-plugin by the registration attribute as true
        /// </summary>
        [RegistrableAttribute(true)]
        public sealed class TestAutomationSequenceParser : ISequenceParser
        {

            #region ISequenceParser Members

            string IParser.Name
            {
                get { return "TestAutomation SequenceParser"; }
            }

            string IParser.Description
            {
                get { return "TestAutomation SequenceParser Description"; }
            }

            string IParser.FileTypes
            {
                get { throw new System.NotImplementedException(); }
            }

            IAlphabet IParser.Alphabet
            {
                get
                {
                    throw new System.NotImplementedException();
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            MBF.Encoding.IEncoding IParser.Encoding
            {
                get
                {
                    throw new System.NotImplementedException();
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            IList<ISequence> ISequenceParser.Parse(TextReader reader)
            {
                throw new System.NotImplementedException();
            }

            IList<ISequence> ISequenceParser.Parse(string filename)
            {
                throw new System.NotImplementedException();
            }

            ISequence ISequenceParser.ParseOne(TextReader reader)
            {
                throw new System.NotImplementedException();
            }

            ISequence ISequenceParser.ParseOne(string filename)
            {
                throw new System.NotImplementedException();
            }

            IList<ISequence> ISequenceParser.Parse(TextReader reader,
                bool isReadOnly)
            {
                throw new NotImplementedException();
            }

            IList<ISequence> ISequenceParser.Parse(string filename,
                bool isReadOnly)
            {
                throw new NotImplementedException();
            }

            ISequence ISequenceParser.ParseOne(TextReader reader,
                bool isReadOnly)
            {
                throw new NotImplementedException();
            }

            ISequence ISequenceParser.ParseOne(string filename,
                bool isReadOnly)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        /// <summary>
        /// Creating new formatter class which is extended from ISequenceAlignmentFormatter. 
        /// Also registered for auto-plugin by the registration attribute as true
        /// </summary>
        [RegistrableAttribute(true)]
        public sealed class TestAutomationSequenceAlignmentFormatter : ISequenceAlignmentFormatter
        {

            #region ISequenceAlignmentFormatter Members

            /// <summary>
            /// 
            /// </summary>
            public string Name
            {
                get { return "TestAutomation SequenceAlignmentFormatter"; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string Description
            {
                get { return "TestAutomation SequenceAlignmentFormatter Description"; }
            }

            /// <summary>
            /// 
            /// </summary>
            public string FileTypes
            {
                get { throw new System.NotImplementedException(); }
            }

            /// <summary>
            /// Writes an ISequenceAlignment to the location specified by the writer.
            /// </summary>
            /// <param name="sequenceAlignment">The sequence alignment to format.</param>
            /// <param name="writer">The TextWriter used to write the formatted sequence alignment text.</param>
            public void Format(ISequenceAlignment sequenceAlignment, TextWriter writer)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// Writes an ISequenceAlignment to the specified file.
            /// </summary>
            /// <param name="sequenceAlignment">The sequence alignment to format.</param>
            /// <param name="filename">The name of the file to write the formatted sequence alignment text.</param>
            public void Format(ISequenceAlignment sequenceAlignment, string filename)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// Write a collection of ISequenceAlignments to a writer.
            /// </summary>
            /// <param name="sequenceAlignments">The sequence alignments to write.</param>
            /// <param name="writer">The TextWriter used to write the formatted sequence alignments.</param>
            public void Format(ICollection<ISequenceAlignment> sequenceAlignments, TextWriter writer)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// Write a collection of ISequenceAlignments to a file.
            /// </summary>
            /// <param name="sequenceAlignments">The sequenceAlignments to write.</param>
            /// <param name="filename">The name of the file to write the formatted sequence alignments.</param>
            public void Format(ICollection<ISequenceAlignment> sequenceAlignments, string filename)
            {
                throw new System.NotImplementedException();
            }

            /// <summary>
            /// Converts an ISequenceAlignment to a formatted string.
            /// </summary>
            /// <param name="sequenceAlignment">The sequence alignment to format.</param>
            /// <returns>A string of the formatted text.</returns>
            public string FormatString(ISequenceAlignment sequenceAlignment)
            {
                throw new System.NotImplementedException();
            }

            #endregion
        }

        /// <summary>
        /// Creating new parser class which is extended from ISequenceAlignmentParser. 
        /// Also registered for auto-plugin by the registration attribute as true
        /// </summary>
        [RegistrableAttribute(true)]
        public sealed class TestAutomationSequenceAlignmentParser : ISequenceAlignmentParser
        {

            #region ISequenceAlignmentParser Members

            string IParser.Name
            {
                get { return "TestAutomation SequenceAlignmentParser"; }
            }

            string IParser.Description
            {
                get { return "TestAutomation SequenceAlignmentParser Description"; }
            }

            string IParser.FileTypes
            {
                get { throw new System.NotImplementedException(); }
            }

            IAlphabet IParser.Alphabet
            {
                get
                {
                    throw new System.NotImplementedException();
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            MBF.Encoding.IEncoding IParser.Encoding
            {
                get
                {
                    throw new System.NotImplementedException();
                }
                set
                {
                    throw new System.NotImplementedException();
                }
            }

            /// <summary>
            /// Parses a list of biological sequence alignment texts from a reader.
            /// </summary>
            /// <param name="reader">A reader for a biological sequence alignment text.</param>
            /// <returns>The list of parsed ISequenceAlignment objects.</returns>
            public IList<ISequenceAlignment> Parse(TextReader reader)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Parses a list of biological sequence alignment texts from a reader.
            /// </summary>
            /// <param name="reader">A reader for a biological sequence alignment text.</param>
            /// <param name="isReadOnly">
            /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
            /// If this flag is set to true then the resulting sequences's isReadOnly property 
            /// will be set to true, otherwise it will be set to false.
            /// </param>
            /// <returns>The list of parsed ISequenceAlignment objects.</returns>
            public IList<ISequenceAlignment> Parse(TextReader reader, bool isReadOnly)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Parses a list of biological sequence alignment texts from a file.
            /// </summary>
            /// <param name="fileName">The name of a biological sequence alignment file.</param>
            /// <returns>The list of parsed ISequenceAlignment objects.</returns>
            public IList<ISequenceAlignment> Parse(string fileName)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Parses a list of biological sequence alignment texts from a file.
            /// </summary>
            /// <param name="fileName">The name of a biological sequence alignment file.</param>
            /// <param name="isReadOnly">
            /// Flag to indicate whether the resulting sequences should be in readonly mode or not.
            /// If this flag is set to true then the resulting sequences's isReadOnly property 
            /// will be set to true, otherwise it will be set to false.
            /// </param>
            /// <returns>The list of parsed ISequenceAlignment objects.</returns>
            public IList<ISequenceAlignment> Parse(string fileName, bool isReadOnly)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Parses a single biological sequence alignment text from a reader.
            /// </summary>
            /// <param name="reader">A reader for a biological sequence alignment text.</param>
            /// <returns>The parsed ISequenceAlignment object.</returns>
            public ISequenceAlignment ParseOne(TextReader reader)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Parses a single biological sequence alignment text from a reader.
            /// </summary>
            /// <param name="reader">A reader for a biological sequence alignment text.</param>
            /// <param name="isReadOnly">
            /// Flag to indicate whether the resulting sequence alignment should be in readonly mode or not.
            /// If this flag is set to true then the resulting sequence's isReadOnly property 
            /// will be set to true, otherwise it will be set to false.
            /// </param>
            /// <returns>The parsed ISequenceAlignment object.</returns>
            public ISequenceAlignment ParseOne(TextReader reader, bool isReadOnly)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Parses a single biological sequence alignment text from a file.
            /// </summary>
            /// <param name="fileName">The name of a biological sequence alignment file.</param>
            /// <returns>The parsed ISequenceAlignment object.</returns>
            public ISequenceAlignment ParseOne(string fileName)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Parses a single biological sequence alignment text from a file.
            /// </summary>
            /// <param name="fileName">The name of a biological sequence alignment file.</param>
            /// <param name="isReadOnly">
            /// Flag to indicate whether the resulting sequence alignment should be in readonly mode or not.
            /// If this flag is set to true then the resulting sequence's isReadOnly property 
            /// will be set to true, otherwise it will be set to false.
            /// </param>
            /// <returns>The parsed ISequenceAlignment object.</returns>
            public ISequenceAlignment ParseOne(string fileName, bool isReadOnly)
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        #endregion Registration Components

        #region Supported Methods

        /// <summary>
        /// General Register Aligner test case validation goes here.
        /// </summary>
        /// <param name="registeredAligners">Registered Aligners</param>
        /// <param name="finalValue">Expected Registered Aligners</param>
        static void RegisterAlignGeneralTestCases(IList<ISequenceAligner> registeredAligners,
            IList<ISequenceAligner> finalValue)
        {
            if (null != registeredAligners && registeredAligners.Count > 0)
            {
                foreach (ISequenceAligner aligner in finalValue)
                {
                    string name = string.Empty;
                    string description = string.Empty;

                    registeredAligners.FirstOrDefault(IA => string.Compare(name = IA.Name,
                        aligner.Name, StringComparison.OrdinalIgnoreCase) == 0);
                    registeredAligners.FirstOrDefault(IA => string.Compare(
                        description = IA.Description, aligner.Description,
                        StringComparison.OrdinalIgnoreCase) == 0);

                    // Validates the Name and Description
                    Assert.AreEqual(aligner.Name, name);
                    Assert.AreEqual(aligner.Description, description);
                    Console.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered Instances for '{0}'.",
                        name));
                    ApplicationLog.WriteLine(
                        string.Format((IFormatProvider)null, @"Successfully validated the Registered Instances for '{0}'.",
                        name));
                }
            }
            else
            {
                Console.WriteLine("No Components to Register.");
                ApplicationLog.WriteLine("No Components to Register.");
                Assert.Fail();
            }
        }

        /// <summary>
        /// Creates the Add-ins folder
        /// </summary>
        static void CreateAddinsFolder()
        {
            // Gets the Add-ins folder name
            Uri uri = new Uri(Assembly.GetCallingAssembly().CodeBase);
            string addInsFolderPath = Uri.UnescapeDataString(string.Concat(
                Path.GetDirectoryName(uri.AbsolutePath),
                AddInsFolder));

            if (!Directory.Exists(addInsFolderPath))
                // Creates the Add-ins folder
                Directory.CreateDirectory(addInsFolderPath);

            // If TestAutomation file already exists, don't replace
            if (!File.Exists(string.Concat(addInsFolderPath, MBFTestAutomationDll)))
            {
                // Copies the MBF.TestAutomation.dll to Add-ins folder
                File.Copy(Uri.UnescapeDataString(uri.AbsolutePath),
                    string.Concat(addInsFolderPath, MBFTestAutomationDll), true);
            }
        }

        /// <summary>
        /// Deletes the Add-ins folder if exists
        /// </summary>
        static void DeleteAddinsFolder()
        {
            Uri uri = new Uri(Assembly.GetCallingAssembly().CodeBase);
            string addInsFolderPath = Uri.UnescapeDataString(string.Concat(
                Path.GetDirectoryName(uri.AbsolutePath),
                AddInsFolder));

            // If the Add-ins folder exists delete the same
            if (Directory.Exists(addInsFolderPath))
                Directory.Delete(addInsFolderPath, true);
        }

        /// <summary>
        /// Gets all registered specified classes in core folder and addins (optional) folders
        /// </summary>
        /// <param name="includeAddinFolder">include add-ins folder or not</param>
        /// <returns>List of registered classes</returns>
        private static IList<T> GetClasses<T>(bool includeAddinFolder)
        {
            IList<T> registeredAligners = new List<T>();

            if (includeAddinFolder)
            {
                IList<T> addInAligners;
                if (null != RegisteredAddIn.AddinFolderPath)
                {
                    addInAligners =
                        RegisteredAddIn.GetInstancesFromAssemblyPath<T>(RegisteredAddIn.AddinFolderPath,
                        RegisteredAddIn.DLLFilter);
                    if (null != addInAligners && addInAligners.Count > 0)
                    {
                        foreach (T aligner in addInAligners)
                        {
                            if (aligner != null)
                            {
                                registeredAligners.Add(aligner);
                            }
                        }
                    }
                }
            }

            return registeredAligners;
        }

        #endregion Supported Methods
    }
}