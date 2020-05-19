// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SegmentedSequenceBVTTestCases.cs
 * 
 * This file contains the Segmented sequence BVT test case validation.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Segmented Sequence BVT level validations
    /// </summary>
    [TestFixture]
    public class SegmentedSequenceBvtTestCases
    {

        #region Enums

        /// <summary>
        /// Segmented sequence method Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum segmentedSequenceParameters
        {
            SegmentedSequence,
            SegmentedsequenceList,
            Add,
            Clear,
            Contains,
            CopyTo,
            Remove,
            ReplaceChar,
            Clone,
            Default
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SegmentedSequenceBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region Segmented sequence BVT TestCases

        /// <summary>
        /// Validate creation of segmented sequence With DNA Sequence data.
        /// Input Data : Valid DNA Sequence
        /// Output Data : Validation of DNA segmented sequence.
        /// </summary>
        [Test]
        public void ValidateSegmentedSequenceWithDNAData()
        {
            ValidateSegmentedsequenceCreation(Constants.DnaSegSequenceNode,
                segmentedSequenceParameters.SegmentedSequence);
        }

        /// <summary>
        /// Validate creation of segmented sequence with RNA Sequence data.
        /// Input Data : Valid RNA Sequence
        /// Output Data : Validation of RNA segmented sequence.
        /// </summary>
        [Test]
        public void ValidateSegmentedSequenceWithRNAData()
        {
            ValidateSegmentedsequenceCreation(Constants.RnaSegSequenceNode,
                segmentedSequenceParameters.SegmentedSequence);
        }

        /// <summary>
        /// Validate creation of segmented sequence with PROTEIN Sequence data.
        /// Input Data : Valid PROTEIN Sequence
        /// Output Data : Validation of PROTEIN segmented sequence.
        /// </summary>
        [Test]
        public void ValidateSegmentedSequenceWithPROTEINData()
        {
            ValidateSegmentedsequenceCreation(Constants.ProteinSegSequenceNode,
                segmentedSequenceParameters.SegmentedSequence);
        }

        /// <summary>
        /// Validate creation of segmented sequence with DNA Sequence data using SegSequenceList.
        /// Input Data : Valid DNA Sequence List
        /// Output Data : Validation of DNA segmented sequence.
        /// </summary>
        [Test]
        public void ValidateSegmentedSequenceListWithDNAData()
        {
            ValidateSegmentedsequenceCreation(Constants.DnaSegSequenceListNode,
                segmentedSequenceParameters.SegmentedsequenceList);
        }

        /// <summary>
        /// Validate creation of segmented sequence with RNA Sequence data using SegSequenceList.
        /// Input Data : Valid RNA Sequence List
        /// Output Data : Validation of RNA segmented sequence.
        /// </summary>
        [Test]
        public void ValidateSegmentedSequenceListWithRNAData()
        {
            ValidateSegmentedsequenceCreation(Constants.RnaSegSequenceListNode,
                segmentedSequenceParameters.SegmentedsequenceList);
        }

        /// <summary>
        /// Validate creation of segmented sequence with PROTEIN Sequence data using SegSequenceList.
        /// Input Data : Valid PROTEIN Sequence List
        /// Output Data : Validation of PROTEIN segmented sequence.
        /// </summary>
        [Test]
        public void ValidateSegmentedSequenceListWithPROTEINData()
        {
            ValidateSegmentedsequenceCreation(
                Constants.ProteinSegSequenceListNode, segmentedSequenceParameters.SegmentedsequenceList);
        }


        /// <summary>
        /// Validate addition of sequence items to the end of segmented sequence 
        /// Input Data : Valid DNA sequence
        /// Output Data : Validation of segmented sequence after adding sequence items.
        /// </summary>
        [Test]
        public void ValidateAdditionOfSeqItemsToSegSeq()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, segmentedSequenceParameters.Add);
        }

        /// <summary>
        /// Validate segmentedSequence cloning..
        /// Input Data : Valid DNA Sequence
        /// Output Data : Validation of DNA segmented sequence.cloning.
        /// </summary>
        [Test]
        public void ValidateSegmentedSequenceCloning()
        {
            ValidateSegmentedsequenceCreation(Constants.DnaSegSequenceListNode,
                segmentedSequenceParameters.Clone);
        }


        /// <summary>
        /// Validate deletion of sequence items from the segmented sequence 
        /// Input Data : Valid DNA sequence
        /// Output Data : Validation of segmented sequence after removing sequence items.
        /// </summary>
        [Test]
        public void ValidateDeletionOfSeqItemsFromSegSeq()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, segmentedSequenceParameters.Clear);
        }

        /// <summary>
        /// Validate copy of sequence items to pre-allocated array.
        /// Input Data : Valid DNA sequence
        /// Output Data : Validation of copy of segmented sequence items to array.
        /// </summary>
        [Test]
        public void ValidateCopyOfSeqItemsToArray()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, segmentedSequenceParameters.CopyTo);
        }

        /// <summary>
        /// Validate Removing sequence items from sequence.
        /// Input Data : Valid DNA sequence
        /// Output Data : Validation of removing sequence items from sequence.
        /// </summary>
        [Test]
        public void ValidateRemoveSeqItemsFromSegSequence()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, segmentedSequenceParameters.Remove);
        }

        /// <summary>
        /// Validate Replacing  sequence items with characters.
        /// Input Data : Valid DNA sequence
        /// Output Data : Validation of replacing sequence items with characters.
        /// </summary>
        [Test]
        public void ValidateReplaceSeqItemsWithChars()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, segmentedSequenceParameters.ReplaceChar);
        }

        /// <summary>
        /// Create a segmented sequence 
        /// and validate various properties present in the segmented sequence class.
        /// </summary>
        [Test]
        public void ValidateSegmentedSequenceProperties()
        {
            // Gets the alphabet from the Xml
            string alphabet = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.AlphabetNameNode);
            string inputSequence1 = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.Sequence1);
            string inputSequence2 = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.Sequence2);
            string inputSequence3 = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.Sequence3);
            string inputSequence4 = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.Sequence4);
            string inputSequence5 = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.Sequence5);
            string inputSequence6 = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.Sequence6);
            string inputSequence7 = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.Sequence7);
            string expectedSegSeqCount = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.SegmetedSeqCount);
            string expectedSeguencesCount = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.SequencesCount);
            string expectedComplement = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.Complement);
            string expectedReverse = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.Reverse);
            string expectedReverseComplement = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.ReverseComplement);
            string expectedASymbolCount = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.SymbolACountNode);
            string seqId = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.SequenceIdNode);
            string displayId = Utility._xmlUtil.GetTextValue(
                Constants.DnaSegSequenceListNode, Constants.DisplayId);

            List<ISequence> seqList = new List<ISequence>();
            SegmentedSequence segmentedSeq = null;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Segmented Sequence BVT: Sequence {0} is expected.", alphabet));

            // create a Isequence.
            ISequence seq1 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence1);
            ISequence seq2 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence2);
            ISequence seq3 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence3);
            ISequence seq4 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence4);
            ISequence seq5 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence5);
            ISequence seq6 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence6);
            ISequence seq7 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence7);

            // Add all sequences to sequence list.
            seqList.Add(seq1);
            seqList.Add(seq2);
            seqList.Add(seq3);
            seqList.Add(seq4);
            seqList.Add(seq5);
            seqList.Add(seq6);
            seqList.Add(seq7);

            // create a Segmented sequence with sequence list.
            segmentedSeq = new SegmentedSequence(seqList);

            // Set Segmented sequence Id and display Id.
            segmentedSeq.ID = seqId;
            segmentedSeq.DisplayID = displayId;

            //Validate all properties
            Assert.AreEqual(segmentedSeq.Count.ToString((IFormatProvider)null),
                expectedSegSeqCount);
            Assert.AreEqual(segmentedSeq.Sequences.Count.ToString((IFormatProvider)null),
                expectedSeguencesCount);
            Assert.AreEqual(expectedReverse, segmentedSeq.Reverse.ToString());
            Assert.AreEqual(expectedComplement, segmentedSeq.Complement.ToString());
            Assert.AreEqual(expectedReverseComplement, segmentedSeq.ReverseComplement.ToString());
            Assert.IsNull(segmentedSeq.Documentation);
            Assert.IsTrue(segmentedSeq.IsReadOnly);
            Assert.IsEmpty(segmentedSeq.Metadata);
            Assert.AreEqual(expectedASymbolCount,
                segmentedSeq.Statistics.GetCount('A').ToString());
            Assert.AreEqual(displayId, segmentedSeq.DisplayID);
            Assert.AreEqual(seqId, segmentedSeq.ID);

            Console.WriteLine(
                "SegmentedSequenceBVT: Validation of all properties of sparse sequence instance is completed");
            ApplicationLog.WriteLine(
                "SegmentedSequenceBVT: Validation of all properties of sparse sequence instance is completed");
        }

        #endregion Segmented sequence BVT Test Cases

        #region Supporting method

        /// <summary>
        /// General method to validate creation of Segmented sequence.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">Method Name</param>
        /// </summary>
        static void ValidateSegmentedsequenceCreation(
            string nodeName, segmentedSequenceParameters methodName)
        {
            // Gets the alphabet from the Xml
            string alphabet = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string expectedSegmentedSeq = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string inputSequence1 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence1);
            string inputSequence2 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence2);
            string inputSequence3 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence3);
            string inputSequence4 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence4);
            string inputSequence5 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence5);
            string inputSequence6 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence6);
            string inputSequence7 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence7);
            string expectedSegSeqCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SegmetedSeqCount);
            string expectedSeguencesCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequencesCount);

            ISequence seq = null;
            ISequence seq1 = null;
            ISequence seq2 = null;
            ISequence seq3 = null;
            ISequence seq4 = null;
            ISequence seq5 = null;
            ISequence seq6 = null;
            ISequence seq7 = null;
            List<ISequence> seqList = new List<ISequence>();
            SegmentedSequence segmentedSeq = null;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Segmented Sequence BVT: Sequence {0} is expected.", alphabet));

            // create a Isequence.
            seq1 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence1);
            seq2 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence2);
            seq3 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence3);
            seq4 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence4);
            seq5 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence5);
            seq6 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence6);
            seq7 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence7);

            // Add all sequences to sequence list.
            seqList.Add(seq1);
            seqList.Add(seq2);
            seqList.Add(seq3);
            seqList.Add(seq4);
            seqList.Add(seq5);
            seqList.Add(seq6);
            seqList.Add(seq7);
            switch (methodName)
            {
                case segmentedSequenceParameters.SegmentedsequenceList:
                    // create a Segmented sequence with sequence list.
                    segmentedSeq = new SegmentedSequence(seqList);

                    // validate created segmented sequence list.
                    Assert.AreEqual(expectedSegmentedSeq, segmentedSeq.ToString());
                    Assert.AreSame(seq1, segmentedSeq.Sequences[0]);
                    Assert.AreSame(seq2, segmentedSeq.Sequences[1]);
                    Assert.AreSame(seq3, segmentedSeq.Sequences[2]);
                    Assert.AreSame(seq4, segmentedSeq.Sequences[3]);
                    Assert.AreSame(seq5, segmentedSeq.Sequences[4]);
                    Assert.AreSame(seq6, segmentedSeq.Sequences[5]);
                    Assert.AreSame(seq7, segmentedSeq.Sequences[6]);
                    Console.WriteLine(string.Format(null,
                        "Segmented Sequence BVT: Segmented Sequence{0}  is as expected.",
                        segmentedSeq.ToString()));
                    break;

                case segmentedSequenceParameters.SegmentedSequence:
                    // create a Isequence.
                    seq = new Sequence(Utility.GetAlphabet(alphabet), inputSequence1);

                    // create a Segmented sequence 
                    segmentedSeq = new SegmentedSequence(seq);

                    // validate expected segmented sequence.
                    Assert.AreEqual(seq.ToString(), segmentedSeq.ToString());
                    Console.WriteLine(string.Format(null,
                        "Segmented Sequence BVT: Segmented Sequence{0}  is as expected.",
                        segmentedSeq.ToString()));
                    break;
                case segmentedSequenceParameters.Clone:
                    // create a Segmented sequence with sequence list.
                    segmentedSeq = new SegmentedSequence(seqList);

                    // create a segmented sequences clone.
                    SegmentedSequence segSequenceClone = segmentedSeq.Clone();

                    // validate Clone sequence.
                    Assert.AreEqual(segSequenceClone.ToString(), segmentedSeq.ToString());
                    Assert.AreEqual(segSequenceClone.Count, segmentedSeq.Count);
                    Assert.AreEqual(segSequenceClone.Sequences.Count, 7);
                    Console.WriteLine(string.Format(null,
                    "Segmented Sequence BVT: Segmented Sequence{0}  is as expected.",
                    segmentedSeq.ToString()));
                    break;
                default:
                    break;
            }

            // Validate a created segmented Sequence
            Assert.AreEqual(segmentedSeq.Count.ToString((IFormatProvider)null),
                expectedSegSeqCount);
            Assert.AreEqual(segmentedSeq.Sequences.Count.ToString((IFormatProvider)null),
                expectedSeguencesCount);
            Assert.AreSame(Utility.GetAlphabet(alphabet), segmentedSeq.Alphabet);

            // Logs to the NUnit GUI (Console.Out) window
            Console.WriteLine(string.Format(null,
                "Segmented Sequence BVT: Segmented Sequence count{0} is as expected.",
                segmentedSeq.Sequences.Count.ToString((IFormatProvider)null)));
            Console.WriteLine(string.Format(null,
                 "Segmented Sequence BVT: Segmented Sequences count{0} is as expected.",
                 segmentedSeq.Count.ToString((IFormatProvider)null)));

            // Logs to the NUnit GUI (Console.Out) window
            ApplicationLog.WriteLine(
                "Segmented Sequence BVT: Segmented Sequence validation is completed successfully.");
        }

        /// <summary>
        /// Validate general segmented sequence methods such as
        /// Add,Clear,replace,copyTo,Remove.
        /// <param name="nodeName">xml node name.</param
        /// <param name="methodName">Method Name</param>
        /// </summary>
        static void ValidateGeneralMethodsOfSegmentedSequence(
            string nodeName, segmentedSequenceParameters methodName)
        {
            // Gets the alphabet from the Xml
            string alphabet = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string expectedSegSeqCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SegmetedSeqCount);
            string inputSequence1 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence1);
            string expectedSegmentedSeqAfterAddingNucleotide = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSeqAfterAdd);
            string expectedSegmentedSeqCountAfterAddingNucleotide = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceCountAfterSeqAdd);
            string inputSequence3 = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence3);
            int index = 0;
            int j = 1;

            Sequence seq = null;
            SegmentedSequence segmentedSeq = null;

            // create a Isequence.
            seq = new Sequence(Utility.GetAlphabet(alphabet), inputSequence1);
            seq.IsReadOnly = false;

            // create a Segmented seqeunce 
            segmentedSeq = new SegmentedSequence(seq);
            Assert.AreEqual(seq.ToString(), segmentedSeq.ToString());

            switch (methodName)
            {
                case segmentedSequenceParameters.Add:
                    // Add all sequence items to the end of the sequence.
                    foreach (ISequenceItem item in Utility.GetAlphabet(alphabet))
                    {
                        segmentedSeq.Add(item);
                    }

                    // validate segmented sequence after adding sequence item.
                    Assert.AreEqual(segmentedSeq.ToString(),
                        expectedSegmentedSeqAfterAddingNucleotide);
                    Assert.AreEqual(segmentedSeq.Count.ToString((IFormatProvider)null),
                        expectedSegmentedSeqCountAfterAddingNucleotide);
                    break;
                case segmentedSequenceParameters.Clear:
                    // Clear all sequence items in segmented sequence.
                    segmentedSeq.Clear();

                    // validate segmented sequence after adding sequence item.
                    Assert.IsEmpty(segmentedSeq.ToString());
                    break;
                case segmentedSequenceParameters.CopyTo:
                    // Copy sequence items to array.
                    ISequenceItem[] iseqItems = new ISequenceItem[140];
                    segmentedSeq.CopyTo(iseqItems, index);
                    Assert.IsNotNull(iseqItems);

                    // Validate array.
                    foreach (ISequenceItem item in Utility.GetAlphabet(alphabet))
                    {
                        Assert.AreEqual(iseqItems[index], segmentedSeq[index]);
                        Assert.AreEqual(iseqItems[index], segmentedSeq[index]);
                        Assert.AreEqual(iseqItems[index].Symbol, segmentedSeq[index].Symbol);
                        index++;
                    }
                    break;
                case segmentedSequenceParameters.Remove:
                    //Remove sequence items in segmented sequence.
                    foreach (ISequenceItem item in segmentedSeq)
                    {
                        segmentedSeq.Remove(segmentedSeq[index]);
                        Assert.AreEqual(segmentedSeq.Count, Convert.ToInt32(expectedSegSeqCount) - j);
                        j++;
                        index++;
                        Console.WriteLine(string.Format(null,
                            "Segmented Sequence BVT: Segmented Sequence{0}  is as expected.",
                            segmentedSeq.ToString()));
                    }
                    break;
                case segmentedSequenceParameters.ReplaceChar:
                    //Replace each sequence items with different.
                    foreach (ISequenceItem item in segmentedSeq)
                    {
                        segmentedSeq.Replace(index, inputSequence3[index]);
                        index++;
                    }
                    // Validate replaced character sequence.
                    Assert.AreEqual(segmentedSeq.ToString(), inputSequence3);
                    break;
                default:
                    break;
            }

            Console.WriteLine(string.Format(null,
                "Segmented Sequence BVT: Segmented Sequence{0}  is as expected.",
                segmentedSeq.ToString()));
            Console.WriteLine(string.Format(null,
                "Segmented Sequence BVT: Segmented Sequence count{0}  is as expected.",
                segmentedSeq.Count.ToString()));
        }

        #endregion supporting method
    }
}
