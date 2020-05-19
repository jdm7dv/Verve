// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SegmentedSequenceP1TestCases.cs
 * 
 * This file contains the Segmented Sequence P1 test case validation.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MBF.Encoding;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Segmented Sequence P1 level validations
    /// </summary>
    [TestClass]
    public class SegmentedSequenceP1TestCases
    {

        #region Enums

        /// <summary>
        /// Segmented seqeunce method Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum SegmentedSequenceParameters
        {
            SegmentedSeqList,
            SegmentedSeq,
            Add,
            Clear,
            Contains,
            CopyTo,
            Remove,
            GetEnumerator,
            IndexOf,
            InsertRange,
            InsertChar,
            RemoveAt,
            Range,
            RemoveRange,
            ReplaceSeqItem,
            ReplaceChar,
            ReplaceRange,
            ToString,
            Clone,
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
        static SegmentedSequenceP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Segmented P1 Test cases

        /// <summary>
        /// Validate creation of segmented sequence
        /// for four small size DNA Sequences with size less than 100 KB
        /// Input Data : Valid four DNA Sequences
        /// Output Data : Validation of DNA segmented sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceListsWithSmallSizeDNAData()
        {
            ValidateSegmentedSequenceCreation(Constants.SmallSizeDnaSegSequenceListNode,
                SegmentedSequenceParameters.SegmentedSeqList, false);
        }

        /// <summary>
        /// Validate creation of segmented sequence
        /// for four small Size RNA Sequences with size less than 100 KB
        /// Input Data : Valid four RNA Sequences
        /// Output Data : Validation of RNA segmented sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceListsWithSmallSizeRNAData()
        {
            ValidateSegmentedSequenceCreation(Constants.SmallSizeRnaSegSequenceListNode,
                SegmentedSequenceParameters.SegmentedSeqList, false);
        }

        /// <summary>
        /// Validate creation of segmented sequence
        /// for four small Size PROTEIN Sequences with size less than 100 KB
        /// Input Data : Valid four PROTEIN Sequences
        /// Output Data : Validation of PROTEIN segmented sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceListsWithSmallSizePROTEINData()
        {
            ValidateSegmentedSequenceCreation(Constants.SmallSizeProteinSegSequenceListNode,
                SegmentedSequenceParameters.SegmentedSeqList, false);
        }

        /// <summary>
        /// Validate creation of segmented sequence
        /// for a small Size DNA Sequence with size less than 100 KB
        /// Input Data : Valid DNA Sequence
        /// Output Data : Validation of DNA segmented sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceWithSmallSizeDNAData()
        {
            ValidateSegmentedSequenceCreation(Constants.SmallSizeDnaSegSequenceNode,
                SegmentedSequenceParameters.SegmentedSeq, false);
        }

        /// <summary>
        /// Validate creation of segmented sequence
        /// for a small Size RNA Sequence with size less than 100 KB
        /// Input Data : Valid RNA Sequence
        /// Output Data : Validation of RNA segmented sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceWithSmallSizeRNAData()
        {
            ValidateSegmentedSequenceCreation(Constants.SmallSizeRnaSegSequencetNode,
                SegmentedSequenceParameters.SegmentedSeq, false);
        }

        /// <summary>
        /// Validate creation of segmented sequence
        /// for a small Size PROTEIN Sequence with size less than 100 KB
        /// Input Data : Valid PROTEIN Sequence
        /// Output Data : Validation of PROTEIN segmented sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceWithSmallSizePROTEINData()
        {
            ValidateSegmentedSequenceCreation(Constants.SmallSizeProteinSegSequenceNode,
                SegmentedSequenceParameters.SegmentedSeq, false);
        }

        /// <summary>
        /// Validate creation of segmented sequence
        /// for four medium Size DNA Sequences with size greater than 100 KB
        /// Input Data : Valid four DNA Sequences
        /// Output Data : Validation of DNA segmented sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceListsWithMediumSizeDNAData()
        {
            ValidateSegmentedSequenceCreation(Constants.MediumSizeDnaSegSequenceListNode,
                SegmentedSequenceParameters.SegmentedSeqList, false);
        }

        /// <summary>
        /// Validate creation of segmented sequence
        /// for four medium Size RNA Sequences with size greater than 100 KB
        /// Input Data : Valid four RNA Sequences
        /// Output Data : Validation of RNA segmented sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceListsWithMediumSizeRNAData()
        {
            ValidateSegmentedSequenceCreation(Constants.MediumSizeRnaSegSequenceListNode,
                SegmentedSequenceParameters.SegmentedSeqList, false);
        }

        /// <summary>
        /// Validate creation of segmented sequence
        /// for four medium Size PROTEIN Sequences with size greater than 100 KB
        /// Input Data : Valid four PROTEIN Sequences
        /// Output Data : Validation of PROTEIN segmented sequences.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceListsWithMediumSizePROTEINData()
        {
            ValidateSegmentedSequenceCreation(Constants.MediumSizeProteinSegSequenceListNode,
                SegmentedSequenceParameters.SegmentedSeqList, false);
        }

        /// <summary>
        /// Validate creation of segmented sequence
        /// for a medium Size DNA Sequence with size greater than 100 KB
        /// Input Data : Valid DNA Sequence
        /// Output Data : Validation of DNA segmented sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceWithMediumSizeDNAData()
        {
            ValidateSegmentedSequenceCreation(Constants.MediumSizeDnaSegSequenceNode,
                SegmentedSequenceParameters.SegmentedSeq, false);
        }

        /// <summary>
        /// Validate creation of segmented sequence
        /// for a medium size RNA Sequence with size greater than 100 KB
        /// Input Data : Valid RNA Sequence
        /// Output Data : Validation of RNA segmented sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceWithMediumSizeRNAData()
        {
            ValidateSegmentedSequenceCreation(Constants.MediumSizeRnaSegSequencetNode,
                SegmentedSequenceParameters.SegmentedSeq, false);
        }

        /// <summary>
        /// Validate creation of segmented sequence
        /// for a medium Size PROTEIN Sequence with size greater than 100 KB
        /// Input Data : Valid PROTEIN Sequence
        /// Output Data : Validation of PROTEIN segmented sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceWithMediumSizePROTEINData()
        {
            ValidateSegmentedSequenceCreation(Constants.MediumSizeProteinSegSequenceNode,
                SegmentedSequenceParameters.SegmentedSeq, false);
        }

        /// <summary>
        /// Validate addition of sequence items to the end of segmented sequence 
        /// Input Data : Valid RNA sequence
        /// Output Data : Validation of segmented sequence after adding sequence items.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateAdditionOfSeqItemsToRNASegSeq()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.RnaSegSequenceNode, SegmentedSequenceParameters.Add);
        }

        /// <summary>
        /// Validate addition of sequence items to the end of segmented sequence 
        /// Input Data : Valid PROTEIN sequence
        /// Output Data : Validation of segmented sequence after adding sequence items.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateAdditionOfSeqItemsToPROTEINSegSeq()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.ProteinSegSequenceNode, SegmentedSequenceParameters.Add);
        }

        /// <summary>
        /// Validate deletion of sequence items from the segmented sequence 
        /// Input Data : Valid RNA sequence
        /// Output Data : Validation of segmented sequence after removing sequence items.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDeletionOfSeqItemsFromSegSeqWithRNAData()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.RnaSegSequenceNode, SegmentedSequenceParameters.Clear);
        }

        /// <summary>
        /// Validate deletion of sequence items from the segmented sequence 
        /// Input Data : Valid PROTEIN sequence
        /// Output Data : Validation of segmented sequence after removing sequence items.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDeletionOfSeqItemsFromSegSeqWithProteinData()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                    Constants.ProteinSegSequenceNode, SegmentedSequenceParameters.Clear);
        }
        /// <summary>
        /// Validate wheather seq items present in the segmented sequence or not.
        /// Input Data : Valid DNA sequence
        /// Output Data : Validation of seq items present in the segmented sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateIfSeqItemsPresentInSegSeq()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, SegmentedSequenceParameters.Contains);
        }

        /// <summary>
        /// Validate copy of sequence items  to pre-allocated 
        /// array.with medium size Protein sequence
        /// Input Data : Valid PROTEIN sequence with size >100KB.
        /// Output Data : Validation of copy of segmented sequence items to array.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateCopyOfSeqItemsToArrayWithMediumSizeProteinSeq()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.MediumSizeProteinSegSequenceListNode, SegmentedSequenceParameters.CopyTo);
        }

        /// <summary>
        /// Validate enumerator of the segmented sequence.
        /// Input Data : Valid DNA sequence.
        /// Output Data : Validation of segmented sequence enumerator.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegSeqEnumerator()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, SegmentedSequenceParameters.GetEnumerator);
        }

        /// <summary>
        /// Validate Index of the segmented sequence items.
        /// Input Data : Valid RNA sequence.
        /// Output Data : Validation of segmented sequence items index.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateIndexOfRNASegSeqItems()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.RnaSegSequenceNode, SegmentedSequenceParameters.IndexOf);
        }

        /// <summary>
        /// Validate insertion of sequence items in the segmented sequence
        /// Input Data : Valid DNA sequence.
        /// Output Data : Validation of inserting sequence item in the segmented sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateInsertSeqItem()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.RnaSegSequenceNode, SegmentedSequenceParameters.InsertChar);
        }

        /// <summary>
        /// Validate insert range.
        /// Input Data : Valid DNA sequence.
        /// Output Data : Validation of inserting sequence item in the segmented sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateInsertSequenceRange()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, SegmentedSequenceParameters.InsertRange);
        }

        /// <summary>
        /// Validate range of segmented sequence.
        /// Input Data : Valid DNA sequence.
        /// Output Data : Validation of range of segmented sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegSeqRange()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, SegmentedSequenceParameters.Range);
        }

        /// <summary>
        /// Validate removal of seq item at specified position.
        /// Input Data : Valid DNA sequence.
        /// Output Data : Validation of removal of seq items.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRemoveSeqItemAtPosition()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, SegmentedSequenceParameters.RemoveAt);
        }

        /// <summary>
        /// Validate removal of range of seq from segmented sequence
        /// Input Data : Valid DNA sequence.
        /// Output Data : Validation of removal of seq range.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRemoveSeqWithGivenRange()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, SegmentedSequenceParameters.RemoveRange);
        }

        /// <summary>
        /// Validate Repalce of sequence items with chars
        /// Input Data : Valid DNA sequence.
        /// Output Data : Validation of replacing seq item with chars.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateReplaceSeqItems()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, SegmentedSequenceParameters.ReplaceChar);
        }

        /// <summary>
        /// Validate replacement of sequence with given range.
        /// Input Data : Valid DNA sequence.
        /// Output Data : Validation of replacing seq sequence with given range.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateReplaceSeqWithRange()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, SegmentedSequenceParameters.ReplaceRange);
        }

        /// <summary>
        /// Validate Segmented Sequences ToString((IFormatProvider)null)
        /// Input Data : Valid DNA sequence.
        /// Output Data : Validation of ToString((IFormatProvider)null).
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegSeqToStringMethod()
        {
            ValidateGeneralMethodsOfSegmentedSequence(
                Constants.DnaSegSequenceNode, SegmentedSequenceParameters.ToString);
        }

        /// <summary>
        /// Validate segmented sequence clonning.
        /// Input : Segmented sequence
        /// Output : Segmented sequence clone
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateSegmentedSequenceClone()
        {
            ValidateSegmentedSequenceCreation(Constants.SmallSizeRnaSegSequencetNode,
                SegmentedSequenceParameters.Clone, true);
        }

        /// <summary>
        /// Validate segmented sequence index of Non gap characters.
        /// Input : Segmented sequence
        /// Output : Index of Non gap char
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateIndexOfNonGapCharacters()
        {
            string alphabet = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSizeRnaSegSequencetNode, Constants.AlphabetNameNode);
            string sequenceString = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSizeRnaSegSequencetNode, Constants.ExpectedSequence);
            SerializationInfo info = new SerializationInfo(typeof(QualitativeSequence),
                new FormatterConverter());
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            // Create a sequence.
            ISequence seqObj = new Sequence(Utility.GetAlphabet(alphabet),
                sequenceString);

            // Create a segmented sequence
            SegmentedSequence segSeq = new SegmentedSequence(seqObj);

            segSeq.GetObjectData(info, context);

            // Get index of non gap characters.
            int index = segSeq.IndexOfNonGap();
            int indexWithPos = segSeq.IndexOfNonGap(0);

            Assert.AreEqual(0, index);
            Assert.AreEqual(0, indexWithPos);

            // Log to Nunit GUI
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Segmented Sequence P1: Segmented sequence index is {0}",
                index));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Segmented Sequence P1: Segmented sequence index is {0}",
                index));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Segmented Sequence P1: Segmented sequence index is {0}",
                indexWithPos));
        }

        /// <summary>
        /// Validate segmented sequence Last index of Non gap characters.
        /// Input : Segmented sequence
        /// Output : Last Index of Non gap char
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateLastIndexOfNonGapCharacters()
        {
            string alphabet = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSizeRnaSegSequencetNode,
                Constants.AlphabetNameNode);
            string sequenceString = _utilityObj._xmlUtil.GetTextValue(
                Constants.SmallSizeRnaSegSequencetNode,
                Constants.ExpectedSequence);

            // Create a sequence.
            ISequence seqObj = new Sequence(Utility.GetAlphabet(alphabet),
                sequenceString);

            // Create a segmented sequence
            SegmentedSequence segSeq = new SegmentedSequence(seqObj);

            // Get Last index of non gap characters.
            int lastIndex = segSeq.LastIndexOfNonGap();
            int lastIndexWithPos = segSeq.LastIndexOfNonGap(segSeq.Count - 1);

            Assert.AreEqual(segSeq.Count - 1, lastIndex);
            Assert.AreEqual(segSeq.Count - 1, lastIndexWithPos);

            // Log to Nunit GUI
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Segmented Sequence P1: Segmented sequence index is {0}",
                lastIndex));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Segmented Sequence P1: Segmented sequence index is {0}",
                lastIndexWithPos));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Segmented Sequence P1: Segmented sequence index is {0}",
                lastIndexWithPos));

            // Validate Serialization
            Sequence seq1 = new Sequence(Alphabets.DNA);
            seq1.InsertRange(0, "ACGACTGC");
            Sequence seq2 = new Sequence(Alphabets.DNA, "AGGTCA");
            Sequence seq3 = new Sequence(Alphabets.DNA, Encodings.Ncbi2NA, "GGCCA");
            SegmentedSequence seq = new SegmentedSequence(
                new List<ISequence>()
                    {
                        seq1,
                        seq2,
                        seq3
                    });

            using (Stream stream = File.Open("SegmentedSequence.data", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, seq);

                stream.Seek(0, SeekOrigin.Begin);
                SegmentedSequence deserializedSeq = (SegmentedSequence)formatter.Deserialize(stream);

                Assert.AreNotSame(seq, deserializedSeq);
                Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);
            }
        }

        #endregion Segmented P1 Test cases

        #region Supporting methods

        /// <summary>
        /// General method to validate creation of Segmented seqeunce.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">Method Name</param>
        /// </summary>
        void ValidateSegmentedSequenceCreation(
            string nodeName, SegmentedSequenceParameters methodName,
            bool IsCloneMethod)
        {
            // Gets the alphabet from the Xml
            string alphabet = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string expectedSegmentedSeq = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string inputSequence1 = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence1);
            string inputSequence2 = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence2);
            string inputSequence3 = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence3);
            string inputSequence4 = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence4);
            string expectedSegSeqCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SegmetedSeqCount);
            string expectedSeguencesCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequencesCount);

            ISequence seq = null;
            List<ISequence> seqList = new List<ISequence>();
            SegmentedSequence segmentedSeq = null;
            ISequence segSeq = null;

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Segmented Sequence P1: Sequence {0} is expected.", alphabet));

            switch (methodName)
            {
                case SegmentedSequenceParameters.SegmentedSeqList:
                    // create a Isequence.
                    ISequence seq1 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence1);
                    ISequence seq2 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence2);
                    ISequence seq3 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence3);
                    ISequence seq4 = new Sequence(Utility.GetAlphabet(alphabet), inputSequence4);

                    // Add all sequences to sequence list.
                    seqList.Add(seq1);
                    seqList.Add(seq2);
                    seqList.Add(seq3);
                    seqList.Add(seq4);

                    // create a Segmented seqeunce with sequence list.
                    segmentedSeq = new SegmentedSequence(seqList);

                    // validate created segmented sequence list.
                    Assert.AreEqual(expectedSegmentedSeq, segmentedSeq.ToString());
                    Assert.AreSame(seq1, segmentedSeq.Sequences[0]);
                    Assert.AreSame(seq2, segmentedSeq.Sequences[1]);
                    Assert.AreSame(seq3, segmentedSeq.Sequences[2]);
                    Assert.AreSame(seq4, segmentedSeq.Sequences[3]);
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "Segmented Sequence P1: Segmented Sequence{0}  is as expected.",
                        segmentedSeq.ToString()));
                    break;
                case SegmentedSequenceParameters.SegmentedSeq:
                    // create a Isequence.
                    seq = new Sequence(Utility.GetAlphabet(alphabet), inputSequence1);

                    // create a Segmented seqeunce 
                    segmentedSeq = new SegmentedSequence(seq);

                    // validate expected segmented sequence.
                    Assert.AreEqual(seq.ToString(), segmentedSeq.ToString());
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "Segmented Sequence P1: Segmented Sequence{0}  is as expected.",
                        segmentedSeq.ToString()));
                    break;
                case SegmentedSequenceParameters.Clone:
                    // create a Isequence.
                    seq = new Sequence(Utility.GetAlphabet(alphabet), inputSequence1);
                    segSeq = new SegmentedSequence(seq);
                    ICloneable cloneableSegSeq = new SegmentedSequence(seq);

                    // Create a copy of segmented sequence and validate.
                    ISequence cloneSeq = segSeq.Clone();
                    object iCloneSegSeq = cloneableSegSeq.Clone();

                    Assert.AreEqual(cloneSeq.ToString(), segSeq.ToString());
                    Assert.AreEqual(iCloneSegSeq.ToString(), segSeq.ToString());
                    Console.WriteLine(string.Format((IFormatProvider)null,
                        "Segmented Sequence P1: Segmented Sequence{0}  is as expected.",
                        cloneSeq.ToString()));
                    break;
                default:
                    break;
            }

            if (!IsCloneMethod)
            {
                // Validate the created segmented Sequence
                Assert.AreEqual(segmentedSeq.Count.ToString((IFormatProvider)null),
                    expectedSegSeqCount);
                Assert.AreEqual(segmentedSeq.Sequences.Count.ToString((IFormatProvider)null),
                    expectedSeguencesCount);
                Assert.AreSame(Utility.GetAlphabet(alphabet), segmentedSeq.Alphabet);

                // Logs to the NUnit GUI (Console.Out) window
                Console.WriteLine(string.Format((IFormatProvider)null,
                    "Segmented Sequence P1: Segmented Sequence count{0} is as expected.",
                    segmentedSeq.Sequences.Count.ToString((IFormatProvider)null)));
                Console.WriteLine(string.Format((IFormatProvider)null,
                     "Segmented Sequence P1: Segmented Sequences count{0} is as expected.",
                     segmentedSeq.Count.ToString((IFormatProvider)null)));
                // Logs to the NUnit GUI (Console.Out) window
                ApplicationLog.WriteLine(
                    "Segmented Sequence P1: Segmented Sequence validation is completed successfully.");
            }
        }

        /// <summary>
        /// Validate general segmented sequences methods 
        /// Add,Clear,replace,copyTo,Remove,Contains,Range,
        /// Insert,IndexOf,Replace,RemoveAt,InsertRange,ReplaceRange.
        /// <param name="nodeName">xml node name.</param
        /// <param name="methodName">Method Name</param>
        /// </summary>
        void ValidateGeneralMethodsOfSegmentedSequence(
            string nodeName, SegmentedSequenceParameters methodName)
        {
            // Gets the alphabet from the Xml
            string alphabet = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string expectedSegSeqCount = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SegmetedSeqCount);
            string inputSequence1 = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Sequence1);
            string expectedSegmentedSeqAfterAddingNucleotide = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSeqAfterAdd);
            string expectedSegmentedSeqCountAfterAddingNucleotide = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequenceCountAfterSeqAdd);
            string expectedIndexValue = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedIndexValue);
            string insertSeq = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InsertSeq);
            string expectedInsertSeq = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedInsertSeq);
            string expectedRangeSeq = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RangeSeq);
            string expectedSeqAfterRemoveSeqItem = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceAfterRemove);
            string expectedSeqAfterRemoveSeq = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SeqAfterRemoveSeqRange);
            string expectedSeqAfterReplace = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceAfterReplace);
            string expectedSeqAfterReplaceSeqRange = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceAfterReplaceRange);
            int index = 0;
            int j = 1;
            string expectedSeqAfterInsert = inputSequence1[0] + inputSequence1;
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
                case SegmentedSequenceParameters.Add:
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
                case SegmentedSequenceParameters.Clear:
                    // Clear all sequence items in segmented sequence.
                    segmentedSeq.Clear();

                    // validate segmented sequence after adding sequence item.
                    Assert.IsTrue(string.IsNullOrEmpty(segmentedSeq.ToString()));
                    break;
                case SegmentedSequenceParameters.CopyTo:
                    // Copy sequence items to array.
                    ISequenceItem[] iseqItems = new ISequenceItem[895990];
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
                case SegmentedSequenceParameters.Remove:
                    //Remove sequence items in segmented sequence.
                    foreach (ISequenceItem item in segmentedSeq)
                    {
                        segmentedSeq.Remove(segmentedSeq[index]);
                        Assert.AreEqual(segmentedSeq.Count, Convert.ToInt32(expectedSegSeqCount, (IFormatProvider)null) - j);
                        j++;
                        index++;
                        Console.WriteLine(string.Format((IFormatProvider)null,
                            "Segmented Sequence P1: Segmented Sequence{0}  is as expected.",
                            segmentedSeq.ToString()));
                    }
                    break;
                case SegmentedSequenceParameters.Contains:
                    //Validate wheather sequence item present in the segmented sequence
                    foreach (ISequenceItem item in seq)
                    {
                        bool seqItemPresent = segmentedSeq.Contains(seq[index]);
                        Assert.IsTrue(seqItemPresent);
                        index++;
                    }
                    break;
                case SegmentedSequenceParameters.GetEnumerator:
                    //Get enumerator of the segmented sequence.
                    IEnumerator<ISequenceItem> segSeqEnumerator = segmentedSeq.GetEnumerator();
                    Assert.IsNotNull(segSeqEnumerator);
                    break;
                case SegmentedSequenceParameters.IndexOf:
                    // Validate index of seg sequence items.
                    int indexValue = segmentedSeq.IndexOf(seq[6]);
                    Assert.AreEqual(indexValue, Convert.ToInt32(expectedIndexValue, (IFormatProvider)null));
                    break;
                case SegmentedSequenceParameters.InsertChar:
                    // Validate insertion of sequence items in segmeted sequence.
                    segmentedSeq.Insert(0, inputSequence1[0]);
                    Assert.AreEqual(segmentedSeq.ToString(), expectedSeqAfterInsert);
                    Assert.AreEqual(segmentedSeq.Count, Convert.ToInt32(expectedSegSeqCount, (IFormatProvider)null) + 1);
                    break;
                case SegmentedSequenceParameters.InsertRange:
                    // Validate insertion of sequence items in segmeted sequence.
                    segmentedSeq.InsertRange(10, insertSeq);
                    Assert.AreEqual(segmentedSeq.ToString(), expectedInsertSeq);
                    Assert.AreEqual(segmentedSeq.Count,
                        Convert.ToInt32(expectedSegSeqCount, (IFormatProvider)null) + insertSeq.Length);
                    break;
                case SegmentedSequenceParameters.Range:
                    // Validate range of segmented sequence.
                    ISequence rangeSeq = segmentedSeq.Range(20, 20);
                    Assert.AreEqual(rangeSeq.ToString(), expectedRangeSeq);
                    break;
                case SegmentedSequenceParameters.RemoveAt:
                    // Validate range of segmented sequence.
                    segmentedSeq.RemoveAt(70);
                    Assert.AreEqual(segmentedSeq.ToString(), expectedSeqAfterRemoveSeqItem);
                    break;
                case SegmentedSequenceParameters.RemoveRange:
                    // Validate range of segmented sequence.
                    segmentedSeq.RemoveRange(0, 10);
                    Assert.AreEqual(segmentedSeq.ToString(), expectedSeqAfterRemoveSeq);
                    break;
                case SegmentedSequenceParameters.ToString:
                    // Validate Tostring.
                    Assert.AreEqual(segmentedSeq.ToString(), inputSequence1);
                    break;
                case SegmentedSequenceParameters.ReplaceChar:
                    // Validate replacement of sequence items in segmeted sequence.
                    segmentedSeq.Replace(15, inputSequence1[0]);
                    Assert.AreEqual(segmentedSeq.ToString(), expectedSeqAfterReplace);
                    break;
                case SegmentedSequenceParameters.ReplaceRange:
                    // Validate replacement of sequence items in segmeted sequence.
                    segmentedSeq.ReplaceRange(0, insertSeq);
                    Assert.AreEqual(segmentedSeq.ToString(), expectedSeqAfterReplaceSeqRange);
                    break;
                default:
                    break;
            }
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Segmented Sequence P1: Segmented Sequence{0}  is as expected.",
                segmentedSeq.ToString()));
            Console.WriteLine(string.Format((IFormatProvider)null,
                "Segmented Sequence P1: Segmented Sequence count{0}  is as expected.",
                segmentedSeq.Count.ToString((IFormatProvider)null)));
        }

        #endregion Supporting methods
    }
}
