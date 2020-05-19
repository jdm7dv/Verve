// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * VirtualSequenceP1TestCases.cs
 * 
 * This file contains the Virtual Sequence P1 test case validation.
 * 
******************************************************************************/

using System;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Virtual Sequences and P1 level validations.
    /// </summary>
    [TestClass]
    public class VirtualSequenceP1TestCases
    {

        #region Enums

        /// <summary>
        /// Virtual seqeunce method Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum VirtualSequenceParameters
        {
            Add,
            Clear,
            Contains,
            CopyTo,
            Remove,
            GetEnumerator,
            IndexOf,
            Insert,
            RemoveAt,
            InsertRange,
            Range,
            RemoveRange,
            ReplaceSeqItem,
            ReplaceChar,
            ReplaceRange,
            ToString,
            Complement,
            Default
        };

        #endregion Enums

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static VirtualSequenceP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region VirtualSequence P1 TestCases

        /// <summary>
        /// Validate an exception when try to add an iseq
        /// item to virtual sequence.
        /// Input Data : Sequence item.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateAddMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.Add);
        }

        /// <summary>
        /// Validate an exception when try to clear sequences from virtual sequence.
        /// Input Data : Sequence item.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateClearMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.Clear);
        }

        /// <summary>
        /// Validate if Contains() method returns flase.
        /// Input Data : Sequence item.
        /// Output Data : False.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateContainsMethodExpectedValue()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.Contains);
        }

        /// <summary>
        /// Validate an exception when trying to copy seqeunce items to pre-allocated array.
        /// Input Data : pre-allocated sequence items array..
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateCopyToMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.CopyTo);
        }

        /// <summary>
        /// Validate an exception when trying to get enumerator of the virtual sequence.
        /// Input Data : virtual sequence.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateGetEnumeratorMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.GetEnumerator);
        }

        /// <summary>
        /// Validate an exception when trying to remove sequence items from virtual sequence.
        /// Input Data : Sequence item.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRemoveMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.Remove);
        }

        /// <summary>
        /// Validate an exception when trying to get a index 
        /// value of sequence items in virtual sequence..
        /// Input Data : Sequence item.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateIndexOfMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.IndexOf);
        }

        /// <summary>
        /// Validate an exception when trying to insert a 
        /// seq item to virtual sequence.
        /// Input Data : Sequence item.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateInsertMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.Insert);
        }

        /// <summary>
        /// Validate an exception when trying to remove sequence item from  
        /// virtual sequence at specified position.
        /// Input Data : Sequence item.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRemoveAtMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.RemoveAt);
        }

        /// <summary>
        /// Validate an exception when trying to
        /// insert sequence to virtual sequence.
        /// Input Data : Sequence and position of teh sequence where to insert.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateInsertRangeMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.InsertRange);
        }

        /// <summary>
        /// Validate an exception when trying to
        /// get sequence range in virtual sequence.
        /// Input Data : Index of first and last symbol.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRangeMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.Range);
        }

        /// <summary>
        /// Validate an exception when trying to
        /// remove continuous sequence items from virtual sequence.
        /// Input Data : sequence range to be reoved.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRemoveRangeMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.RemoveRange);
        }

        /// <summary>
        /// Validate an exception when trying to
        /// replace sequence item with another sequence item in virtual sequence.
        /// Input Data : Sequence item.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateReplaceSeqItemMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.ReplaceSeqItem);
        }

        /// <summary>
        /// Validate an exception when trying to
        /// replace sequence item with char in virtual sequence.
        /// Input Data : Valid char..
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateReplaceCharMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.ReplaceChar);
        }

        /// <summary>
        /// Validate an exception when trying to
        /// replace sequence with another sequence in virtual sequence.
        /// Input Data : Valid sequence.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateReplaceRangeMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.ReplaceRange);
        }

        /// <summary>
        /// Validate an exception when trying to
        /// get complement,ReverseComplement, and reverse of an virtual sequence.
        /// Input Data : Sequence.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateComplementPropertiesException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.Complement);
        }

        /// <summary>
        /// Validate an exception when trying to
        /// get string of the virtual sequence.
        /// Input Data : sequence range to be reoved.
        /// Output Data : Exception.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateToStringMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.ToString);
        }

        /// <summary>
        /// Validate creation of cloneable virtual sequence for DNA alphabet.
        /// Input Data : Valid Dna Alphabet
        /// Output Data : validation of copy of virtual seqeunce.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDnaCloneVirtualSequence()
        {
            ValidateGeneralVirtualSequenceCloning(Constants.DnaVirtualSeqNode);
        }

        /// <summary>
        /// Validate creation of cloneable virtual sequence for RNA alphabet.
        /// Input Data : Valid Dna Alphabet
        /// Output Data : validation of copy of virtual seqeunce.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaCloneVirtualSequence()
        {
            ValidateGeneralVirtualSequenceCloning(Constants.RnaVirtualSeqNode);
        }

        /// <summary>
        /// Validate creation of cloneable virtual sequence for PROTEIN alphabet.
        /// Input Data : Valid Dna Alphabet
        /// Output Data : validation of copy of virtual seqeunce.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinCloneVirtualSequence()
        {
            ValidateGeneralVirtualSequenceCloning(Constants.ProteinVirtualSeqNode);
        }

        /// <summary>
        /// Validate all the properties in virtual sequence class.
        /// Input Data : Valid Dna Alphabet
        /// Output Data : Validation of all the properties.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateAllProprtiesOfVirtualSequenceClass()
        {
            // Gets the alphabet from the Xml
            string alphabet = _utilityObj._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.AlphabetNameNode);
            string expectedSeqCount = _utilityObj._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.ExpectedVSeqCount);
            string id = _utilityObj._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.Id);
            string diplayId = _utilityObj._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.DisplayId);
            string expectedValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.IndexValue);
            string expectedDocumentaion = _utilityObj._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.Documentaion);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Virtual Sequence P1: Sequence {0} is expected.", alphabet));

            // create virtual seqeunce for an alphabet.
            VirtualSequence virtualSeq = new VirtualSequence(
                Utility.GetAlphabet(alphabet));

            // Set value to virtual sequences.
            virtualSeq.ID = id;
            virtualSeq.DisplayID = diplayId;
            virtualSeq.Documentation = expectedDocumentaion;
            virtualSeq.Metadata.Add("metaDataValue", Alphabets.DNA);

            // Validate the created virtual Sequence
            Assert.AreEqual(virtualSeq.DisplayID, diplayId);
            Assert.AreEqual(virtualSeq.ID, id);
            Assert.AreEqual(virtualSeq.IsReadOnly, true);
            Assert.AreEqual(virtualSeq.Count.ToString((IFormatProvider)null), expectedSeqCount);
            Assert.AreEqual(virtualSeq.Statistics, null);
            Assert.AreEqual(virtualSeq.Alphabet, Alphabets.DNA);
            Assert.AreEqual(virtualSeq.Documentation, expectedDocumentaion);

            virtualSeq.MoleculeType = MoleculeType.DNA;
            foreach (Nucleotide nucleo in Alphabets.DNA)
            {
                Assert.AreEqual(virtualSeq.IndexOf(nucleo).ToString((IFormatProvider)null), expectedValue);
            }
            Assert.AreEqual(virtualSeq.MoleculeType, MoleculeType.DNA);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence BVT: Virtual Sequence ID {0} is as expected.",
                virtualSeq.ID.ToString((IFormatProvider)null)));
            Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence BVT: Virtual Sequence Display ID {0} is as expected.",
                virtualSeq.DisplayID.ToString((IFormatProvider)null)));
            Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence BVT: Virtual Sequence count {0} is as expected.",
                virtualSeq.Count.ToString((IFormatProvider)null)));

            // Logs to the NUnit GUI (Console.Out) window
            ApplicationLog.WriteLine(
                "Virtual Sequence BVT: Virtual Sequence validation is completed successfully.");
        }

        #endregion VirtualSequence P1 TestCases

        #region Supporting methods

        /// <summary>
        /// General method to validate creation of clone virtual seqeunce.
        /// <param name="nodeName">xml node name.</param>
        /// </summary>
        void ValidateGeneralVirtualSequenceCloning(
            string nodeName)
        {
            // Gets alphabet and properties from the Xml
            string alphabet = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string id = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Id);
            string diplayId = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DisplayId);
            string expectedDocumentaion = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Documentaion);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Virtual Sequence P1: Sequence {0} is expected.", alphabet));

            // Create virtual seqeunce for an alphabet.
            VirtualSequence virtualSeq = new VirtualSequence(
                Utility.GetAlphabet(alphabet));

            // Set the value to virtual sequences.
            virtualSeq.ID = id;
            virtualSeq.DisplayID = diplayId;
            virtualSeq.Documentation = expectedDocumentaion;

            // Create a copy of virtual seqeunce.
            VirtualSequence cloneVirtualSeq = virtualSeq.Clone();

            // Validate the created clone virtual Sequence
            Assert.AreEqual(virtualSeq.DisplayID, cloneVirtualSeq.DisplayID);
            Assert.AreEqual(virtualSeq.ID, cloneVirtualSeq.ID);
            Assert.AreEqual(virtualSeq.Documentation, cloneVirtualSeq.Documentation);
            Assert.AreNotSame(virtualSeq, cloneVirtualSeq);
            Assert.AreEqual(virtualSeq.Alphabet, cloneVirtualSeq.Alphabet);
            Assert.AreEqual(virtualSeq.Count, cloneVirtualSeq.Count);
            Assert.AreEqual(virtualSeq.Statistics, cloneVirtualSeq.Statistics);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Clone Virtual Sequence ID {0} is as expected.",
                cloneVirtualSeq.ID.ToString((IFormatProvider)null)));
            Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Clone Virtual Sequence Display ID {0} is as expected.",
                cloneVirtualSeq.DisplayID.ToString((IFormatProvider)null)));
            Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1:Clone Virtual Sequence count {0} is as expected.",
                cloneVirtualSeq.Count.ToString((IFormatProvider)null)));

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(
                "Virtual Sequence P1:Clone Virtual Sequence validation is completed successfully.");
        }

        /// <summary>
        /// Validate virtual seqeunce methods general exception.
        /// <param name="methodName">Virtual seqeunce method parameters</param>
        /// </summary>
        void ValidateGeneralVirtualSequenceException(
            VirtualSequenceParameters methodName)
        {
            // Gets the alphabet from the Xml
            string alphabet = _utilityObj._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.AlphabetNameNode);
            string expectedException = _utilityObj._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.ExceptionMessage);
            string expectedValue = _utilityObj._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.IndexValue);
            bool exThrown = false;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Virtual Sequence P1: Sequence {0} is expected.", alphabet));

            // create virtual seqeunce for an alphabet.
            VirtualSequence virtualSeq = new VirtualSequence(
                Utility.GetAlphabet(alphabet));

            switch (methodName)
            {
                case VirtualSequenceParameters.Add:
                    VirtualSeqAddValidation(virtualSeq,
                        exThrown, expectedException);
                    break;
                case VirtualSequenceParameters.Clear:
                    VirtualSeqClearValidation(virtualSeq,
                        exThrown, expectedException);
                    break;
                case VirtualSequenceParameters.CopyTo:
                    VirtualSeqCopyToValidation(virtualSeq,
                         exThrown, expectedException);
                    break;
                case VirtualSequenceParameters.Remove:
                    VirtualSeqRemoveValidation(virtualSeq,
                         exThrown, expectedException);
                    break;
                case VirtualSequenceParameters.GetEnumerator:
                    VirtualSeqGetEnumeratorValidation(virtualSeq, exThrown,
                        expectedException);
                    break;
                case VirtualSequenceParameters.Insert:
                    VirtualSeqInsertValidation(virtualSeq, exThrown,
                         expectedException);
                    break;
                case VirtualSequenceParameters.Range:
                    VirtualSeqRangeValidation(virtualSeq, exThrown,
                         expectedException);
                    break;
                case VirtualSequenceParameters.InsertRange:
                    VirtualSeqInsertRangeValidation(virtualSeq, exThrown,
                         expectedException);
                    break;
                case VirtualSequenceParameters.RemoveAt:
                    VirtualSeqRemoveAtValidation(virtualSeq, exThrown,
                         expectedException);
                    break;
                case VirtualSequenceParameters.RemoveRange:
                    VirtualSeqRemoveRangeValidation(virtualSeq, exThrown,
                        expectedException);
                    break;
                case VirtualSequenceParameters.ReplaceRange:
                    VirtualSeqReplaceRangeValidation(virtualSeq, exThrown,
                        expectedException);
                    break;
                case VirtualSequenceParameters.ReplaceChar:
                    VirtualSeqReplaceCharValidation(virtualSeq, exThrown,
                        expectedException);
                    break;
                case VirtualSequenceParameters.ReplaceSeqItem:
                    VirtualSeqSeqItemValidation(virtualSeq, exThrown,
                        expectedException);
                    break;
                case VirtualSequenceParameters.ToString:
                    VirtualSeqToStringValidation(virtualSeq, exThrown,
                        expectedException);
                    break;
                case VirtualSequenceParameters.Complement:
                    VirtualSeqComplementValidation(virtualSeq,
                        exThrown, expectedException);
                    break;
                case VirtualSequenceParameters.IndexOf:
                    VirtualSeqIndexOfValidation(virtualSeq, expectedValue);
                    break;
                case VirtualSequenceParameters.Contains:
                    VirtualSeqContainsValidation(virtualSeq);
                    break;
                default:
                    break;
            }
        }

        // <summary>
        /// Validate Range  method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqRangeValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.Range(0, 5);
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }


        // <summary>
        /// Validate Insert  method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqInsertValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.Insert(0, 'G');
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }

        // <summary>
        /// Validate InsertRange  method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqInsertRangeValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.InsertRange(0, "GCCAAAATTTAGGCAGAGA");
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }

        // <summary>
        /// Validate RemoveAt  method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqRemoveAtValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.RemoveAt(0);
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }
        // <summary>
        /// Validate GetEnumerator  method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqGetEnumeratorValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.GetEnumerator();
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }

        // <summary>
        /// Validate RemoveRange  method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqRemoveRangeValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.RemoveRange(0, 1);
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }


        // <summary>
        /// Validate ReplaceRange  method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqReplaceRangeValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.ReplaceRange(0, "AUGAUGAUGAG");
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }

        // <summary>
        /// Validate ReplaceChar  method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqReplaceCharValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.Replace(0, Alphabets.DNA.A.Symbol);
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }

        // <summary>
        /// Validate SeqItem  method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqSeqItemValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.Replace(0, virtualSeq[0]);
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }

        }

        /// <summary>
        /// Validate VirtualSeq ToString() method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqToStringValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.ToString();
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }

        /// <summary>
        /// Validate VirtualSeq Complement method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqComplementValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                ISequence complSeq = virtualSeq.Complement;
                // Below line is the dummy code 
                // for fixing FxCop error "RemoveUnusedLocals".
                Assert.AreEqual(complSeq, null);
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
            // Validate an exception.
            Assert.IsTrue(exThrown);
            try
            {
                ISequence rev = virtualSeq.Reverse;
                // Below line is the dummy code 
                // for fixing FxCop error "RemoveUnusedLocals".
                Assert.AreEqual(rev, null);
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
            // Validate an exception.
            Assert.IsTrue(exThrown);
            try
            {
                ISequence complSeq = virtualSeq.ReverseComplement;
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }

        /// <summary>
        /// Validate VirtualSeq IndexOf method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        static void VirtualSeqIndexOfValidation(VirtualSequence virtualSeq,
            string expectedValue)
        {
            foreach (Nucleotide nucleotide in Alphabets.RNA)
            {
                Assert.AreEqual(virtualSeq.IndexOf(nucleotide).ToString((IFormatProvider)null),
                    expectedValue);
            }
        }

        /// <summary>
        /// Validate VirtualSeq Contains method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        static void VirtualSeqContainsValidation(VirtualSequence virtualSeq)
        {
            foreach (Nucleotide nucleotide in Alphabets.DNA)
            {
                Assert.IsFalse(virtualSeq.Contains(nucleotide));
            }
        }

        /// <summary>
        /// Validate VirtualSeq Add method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqAddValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.Add(Alphabets.DNA.A);
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }

        /// <summary>
        /// Validate VirtualSeq Clear method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqClearValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.Clear();
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }

        /// <summary>
        /// Validate VirtualSeq CopyTo method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqCopyToValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                ISequenceItem[] iseqItems = new ISequenceItem[20];
                virtualSeq.CopyTo(iseqItems, 0);
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }

        /// <summary>
        /// Validate VirtualSeq Remove method exception
        /// </summary>
        /// <param name="virtualSeq">Virtual seq</param>
        /// <param name="exThrown">true if an exception</param>
        /// <param name="expectedException">exception message</param>
        static void VirtualSeqRemoveValidation(VirtualSequence virtualSeq,
            bool exThrown, string expectedException)
        {
            try
            {
                virtualSeq.Remove(virtualSeq[0]);
            }
            catch (NotSupportedException e)
            {
                exThrown = true;
                Assert.IsTrue(exThrown);
                Assert.AreEqual(expectedException, e.Message);
                Console.WriteLine(string.Format((IFormatProvider)null,
                " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
            }
        }
        #endregion Supporting methods
    }
}
