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

using NUnit.Framework;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Virtual Sequences and P1 level validations.
    /// </summary>
    [TestFixture]
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

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region VirtualSequence P1 TestCases

        /// <summary>
        /// Validate an exception when try to add an iseq
        /// item to virtual sequence.
        /// Input Data : Sequence item.
        /// Output Data : Exception.
        /// </summary>
        [Test]
        public void ValidateAddMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.Add);
        }

        /// <summary>
        /// Validate an exception when try to clear sequences from virtual sequence.
        /// Input Data : Sequence item.
        /// Output Data : Exception.
        /// </summary>
        [Test]
        public void ValidateClearMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.Clear);
        }

        /// <summary>
        /// Validate if Contains() method returns flase.
        /// Input Data : Sequence item.
        /// Output Data : False.
        /// </summary>
        [Test]
        public void ValidateContainsMethodExpectedValue()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.Contains);
        }

        /// <summary>
        /// Validate an exception when trying to copy seqeunce items to pre-allocated array.
        /// Input Data : pre-allocated sequence items array..
        /// Output Data : Exception.
        /// </summary>
        [Test]
        public void ValidateCopyToMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.CopyTo);
        }

        /// <summary>
        /// Validate an exception when trying to get enumerator of the virtual sequence.
        /// Input Data : virtual sequence.
        /// Output Data : Exception.
        /// </summary>
        [Test]
        public void ValidateGetEnumeratorMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.GetEnumerator);
        }

        /// <summary>
        /// Validate an exception when trying to remove sequence items from virtual sequence.
        /// Input Data : Sequence item.
        /// Output Data : Exception.
        /// </summary>
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
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
        [Test]
        public void ValidateToStringMethodException()
        {
            ValidateGeneralVirtualSequenceException(VirtualSequenceParameters.ToString);
        }

        /// <summary>
        /// Validate creation of cloneable virtual sequence for DNA alphabet.
        /// Input Data : Valid Dna Alphabet
        /// Output Data : validation of copy of virtual seqeunce.
        /// </summary>
        [Test]
        public void ValidateDnaCloneVirtualSequence()
        {
            ValidateGeneralVirtualSequenceCloning(Constants.DnaVirtualSeqNode);
        }

        /// <summary>
        /// Validate creation of cloneable virtual sequence for RNA alphabet.
        /// Input Data : Valid Dna Alphabet
        /// Output Data : validation of copy of virtual seqeunce.
        /// </summary>
        [Test]
        public void ValidateRnaCloneVirtualSequence()
        {
            ValidateGeneralVirtualSequenceCloning(Constants.RnaVirtualSeqNode);
        }

        /// <summary>
        /// Validate creation of cloneable virtual sequence for PROTEIN alphabet.
        /// Input Data : Valid Dna Alphabet
        /// Output Data : validation of copy of virtual seqeunce.
        /// </summary>
        [Test]
        public void ValidateProteinCloneVirtualSequence()
        {
            ValidateGeneralVirtualSequenceCloning(Constants.ProteinVirtualSeqNode);
        }

        /// <summary>
        /// Validate all the properties in virtual sequence class.
        /// Input Data : Valid Dna Alphabet
        /// Output Data : Validation of all the properties.
        /// </summary>
        [Test]
        public void ValidateAllProprtiesOfVirtualSequenceClass()
        {
            // Gets the alphabet from the Xml
            string alphabet = Utility._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.AlphabetNameNode);
            string expectedSeqCount = Utility._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.ExpectedVSeqCount);
            string id = Utility._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.Id);
            string diplayId = Utility._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.DisplayId);
            string expectedValue = Utility._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.IndexValue);
            string expectedDocumentaion = Utility._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.Documentaion);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
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
            Console.WriteLine(string.Format(null,
                " VirtualSequence BVT: Virtual Sequence ID {0} is as expected.",
                virtualSeq.ID.ToString((IFormatProvider)null)));
            Console.WriteLine(string.Format(null,
                " VirtualSequence BVT: Virtual Sequence Display ID {0} is as expected.",
                virtualSeq.DisplayID.ToString((IFormatProvider)null)));
            Console.WriteLine(string.Format(null,
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
        static void ValidateGeneralVirtualSequenceCloning(
            string nodeName)
        {
            // Gets alphabet and properties from the Xml
            string alphabet = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string id = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Id);
            string diplayId = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DisplayId);
            string expectedDocumentaion = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Documentaion);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
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
            Console.WriteLine(string.Format(null,
                " VirtualSequence P1: Clone Virtual Sequence ID {0} is as expected.",
                cloneVirtualSeq.ID.ToString((IFormatProvider)null)));
            Console.WriteLine(string.Format(null,
                " VirtualSequence P1: Clone Virtual Sequence Display ID {0} is as expected.",
                cloneVirtualSeq.DisplayID.ToString((IFormatProvider)null)));
            Console.WriteLine(string.Format(null,
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
        static void ValidateGeneralVirtualSequenceException(
            VirtualSequenceParameters methodName)
        {
            // Gets the alphabet from the Xml
            string alphabet = Utility._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.AlphabetNameNode);
            string expectedException = Utility._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.ExceptionMessage);
            string expectedValue = Utility._xmlUtil.GetTextValue(
                Constants.DnaVirtualSeqNode, Constants.IndexValue);
            bool exThrown = false;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Virtual Sequence P1: Sequence {0} is expected.", alphabet));

            // create virtual seqeunce for an alphabet.
            VirtualSequence virtualSeq = new VirtualSequence(
                Utility.GetAlphabet(alphabet));

            switch (methodName)
            {
                case VirtualSequenceParameters.Add:
                    try
                    {
                        virtualSeq.Add(Alphabets.DNA.A);
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }

                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.Clear:
                    try
                    {
                        virtualSeq.Clear();
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.CopyTo:
                    try
                    {
                        ISequenceItem[] iseqItems = new ISequenceItem[20];
                        virtualSeq.CopyTo(iseqItems, 0);
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.Remove:
                    try
                    {
                        virtualSeq.Remove(virtualSeq[0]);
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.GetEnumerator:
                    try
                    {
                        virtualSeq.GetEnumerator();
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.Insert:
                    try
                    {
                        virtualSeq.Insert(0, 'G');
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.Range:
                    try
                    {
                        virtualSeq.Range(0, 5);
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.InsertRange:
                    try
                    {
                        virtualSeq.InsertRange(0, "GCCAAAATTTAGGCAGAGA");
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.RemoveAt:
                    try
                    {
                        virtualSeq.RemoveAt(0);
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.RemoveRange:
                    try
                    {
                        virtualSeq.RemoveRange(0, 1);
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.ReplaceRange:
                    try
                    {
                        virtualSeq.ReplaceRange(0, "AUGAUGAUGAG");
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.ReplaceChar:
                    try
                    {
                        virtualSeq.Replace(0, Alphabets.DNA.A.Symbol);
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    break;
                case VirtualSequenceParameters.ReplaceSeqItem:
                    try
                    {
                        virtualSeq.Replace(0, virtualSeq[0]);
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.ToString:
                    try
                    {
                        virtualSeq.ToString();
                    }
                    catch (NotSupportedException e)
                    {
                        exThrown = true;
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    break;
                case VirtualSequenceParameters.Complement:
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
                        Console.WriteLine(string.Format(null,
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
                        Console.WriteLine(string.Format(null,
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
                        Assert.AreEqual(expectedException, e.Message);
                        Console.WriteLine(string.Format(null,
                        " VirtualSequence P1: Virtual seqeunce item to virtual seqeunce was {0}", e.Message));
                    }
                    // Validate an exception.
                    Assert.IsTrue(exThrown);
                    break;
                case VirtualSequenceParameters.IndexOf:
                    foreach (Nucleotide nucleotide in Alphabets.RNA)
                    {
                        Assert.AreEqual(virtualSeq.IndexOf(nucleotide).ToString((IFormatProvider)null),
                            expectedValue);
                    }
                    break;
                case VirtualSequenceParameters.Contains:
                    foreach (Nucleotide nucleotide in Alphabets.DNA)
                    {
                        Assert.IsFalse(virtualSeq.Contains(nucleotide));
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion Supporting methods
    }
}
