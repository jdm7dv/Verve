// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SequenceP2TestCases.cs
 * 
 * This file contains the Sequence P2 test cases.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

using MBF.IO.Fasta;
using MBF.IO.GenBank;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Sequences and P2 validations.
    /// </summary>
    [TestFixture]
    public class SequenceP2TestCases
    {
        #region Enums

        /// <summary>
        /// Parser Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum ParserParameters
        {
            FastA,
            GenBank
        };

        /// <summary>
        /// Additional Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AdditionalParameters
        {
            Insert,
            Remove,
            Replace,
            ReplaceRange,
            Complement,
            Reverse,
            ReverseComplement,
            Count,
            Sequence,
            Cloneable,
            CloneModify,
            Dna,
            Rna,
            Protein,
            NullSequence,
            LastIndexNullSequence,
            NegativeIndex,
            NegativeLastIndex,
            GapCharSequence,
            LastIndexGapCharSequence,
            Range,
            RemoveRange,
            ReplaceSeqItem,
            InsertRange,
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SequenceP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region Sequence P2 TestCases

        /// <summary>
        /// Validate empty invalid sequence with empty string.
        /// Input data : Empty Sequence.
        /// Output Data : Validate the sequence.
        /// </summary>
        [Test]
        public void ValidateEmptySequence()
        {
            // Gets the expected sequence from the Xml
            string genBankFilePath = Utility._xmlUtil.GetTextValue(Constants.EmptyGenBankNodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(genBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null, "Sequence P2: The File exist in the Path {0}.",
                genBankFilePath));
            GenBankParser parser = new GenBankParser();

            // Parse a FastA file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(genBankFilePath);
            Sequence sequenceObj = (Sequence)sequence[0];
            Assert.AreEqual("", sequenceObj.ToString());
            ApplicationLog.WriteLine("Sequence P2: Empty Sequence validated successfully.");
            Console.WriteLine("Sequence P2: Empty Sequence validated successfully.");
        }

        /// <summary>
        /// Validate invalid sequence to Add().
        /// Input data : Invalid sequence to Add().
        /// Output Data : Validate error message.
        /// </summary>
        [Test]
        public void ValidateInvalidAddSequence()
        {
            // Gets the expected sequence from the Xml
            string genBankFilePath = Utility._xmlUtil.GetTextValue(Constants.SimpleGenBankDnaNodeName,
                Constants.FilePathNode);

            string invalidException = Utility._xmlUtil.GetTextValue(Constants.SimpleGenBankDnaNodeName,
                Constants.InvalidAddExceptionNode);

            Assert.IsTrue(File.Exists(genBankFilePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null, "Sequence P2: The File exist in the Path {0}.",
                genBankFilePath));
            GenBankParser parser = new GenBankParser();

            // Parse a FastA file Using Parse method and convert the same to sequence.
            IList<ISequence> sequence = parser.Parse(genBankFilePath);
            Sequence sequenceObj = (Sequence)sequence[0];
            sequenceObj.IsReadOnly = false;

            ISequence proteinSeq = new Sequence(Alphabets.Protein, "E");
            try
            {
                sequenceObj.Add(proteinSeq[0]);
                Assert.IsTrue(false);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.Contains(invalidException));
                ApplicationLog.WriteLine(
                    "Sequence P2: Successfully validated Add() with invalid characters.");
                Console.WriteLine(
                    "Sequence P2: Successfully validated Add() with invalid characters.");
            }
        }

        /// <summary>
        /// Validate MediumSize sequence to Add().
        /// Input data : MediumSize sequence to Add().
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateMediumSizeAddSequence()
        {
            ValidateSizeAddSequenceTestCases(Constants.MediumSizeP2FastaNodeName,
                ParserParameters.FastA);
        }

        /// <summary>
        /// Validate Large Size Fasta sequence to Add().
        /// Input data : Large Size Fasta sequence to Add().
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateLargeSizeFastaAddSequence()
        {
            ValidateSizeAddSequenceTestCases(Constants.LargeSizeFasta,
                ParserParameters.FastA);
        }

        /// <summary>
        /// Validate Small Size sequence to Add().
        /// Input data : Small Size sequence to Add().
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateSmallSizeAddSequence()
        {
            ValidateSizeAddSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank);
        }

        /// <summary>
        /// Validate Large Size GenBank sequence to Add().
        /// Input data : Large Size genbank sequence to Add().
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateLargeSizeGenBankAddSequence()
        {
            ValidateSizeAddSequenceTestCases(Constants.LargeSizeGenBank,
                ParserParameters.GenBank);
        }

        /// <summary>
        /// Validate GenBank sequence and Insert() characters.
        /// Input data : Genbank sequence and Insert() "GAAT" characters.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateGenBankInsertSequence()
        {
            ValidateInsertRemoveReplaceSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Insert);
        }

        /// <summary>
        /// Validate FastA sequence and Insert() characters.
        /// Input data : FastA sequence and Insert() "GAAT" characters.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateFastaInsertSequence()
        {
            ValidateInsertRemoveReplaceSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA, AdditionalParameters.Insert);
        }

        /// <summary>
        /// Validate GenBank sequence and Remove() characters.
        /// Input data : Genbank sequence and Remove() "GAAT" characters.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateGenBankRemoveSequence()
        {
            ValidateInsertRemoveReplaceSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Remove);
        }

        /// <summary>
        /// Validate FastA sequence and Remove() characters.
        /// Input data : FastA sequence and Remove() "GAAT" characters.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateFastaRemoveSequence()
        {
            ValidateInsertRemoveReplaceSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA, AdditionalParameters.Remove);
        }

        /// <summary>
        /// Validate FastA sequence and Remove() characters at the start.
        /// Input data : FastA sequence and Remove() "GAAT" characters at the start.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateFastaRemoveStartSequence()
        {
            ValidateInsertRemoveReplaceSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA, AdditionalParameters.Remove);
        }

        /// <summary>
        /// Validate FastA sequence and Replace() characters.
        /// Input data : FastA sequence and Replace() characters.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateGenBankReplaceSequence()
        {
            ValidateInsertRemoveReplaceSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Replace);
        }

        /// <summary>
        /// Validate FastA sequence and Replace() characters at the start.
        /// Input data : FastA sequence and Replace() characters.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateFastaReplaceSequence()
        {
            ValidateInsertRemoveReplaceSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA, AdditionalParameters.Replace);
        }

        /// <summary>
        /// Validate FastA sequence and ReplaceRange() characters at the start.
        /// Input data : FastA sequence and ReplaceRange() characters.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateFastaReplaceRangeSequence()
        {
            ValidateInsertRemoveReplaceSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA, AdditionalParameters.ReplaceRange);
        }

        /// <summary>
        /// Validate GenBank sequence and Reverse the DNA sequence.
        /// Input data : GenBank DNA sequence.
        /// Output Data : Validate the expected sequence after reverse.
        /// </summary>
        [Test]
        public void ValidateGenBankReverseDnaSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Reverse);
        }

        /// <summary>
        /// Validate GenBank sequence and Reverse the RNA sequence.
        /// Input data : GenBank RNA sequence.
        /// Output Data : Validate the expected sequence after reverse.
        /// </summary>
        [Test]
        public void ValidateGenBankReverseRnaSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleGenBankRnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Reverse);
        }

        /// <summary>
        /// Validate GenBank sequence and Reverse the Protein sequence.
        /// Input data : GenBank Protein sequence.
        /// Output Data : Validate the expected sequence after reverse.
        /// </summary>
        [Test]
        public void ValidateGenBankReverseProSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleGenBankProNodeName,
                ParserParameters.GenBank, AdditionalParameters.Reverse);
        }

        /// <summary>
        /// Validate GenBank sequence and Complement the DNA sequence.
        /// Input data : GenBank DNA sequence.
        /// Output Data : Validate the expected sequence after Complement.
        /// </summary>
        [Test]
        public void ValidateGenBankComplementDnaSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate GenBank sequence and Complement the RNA sequence.
        /// Input data : GenBank RNA sequence.
        /// Output Data : Validate the expected sequence after Complement.
        /// </summary>
        [Test]
        public void ValidateGenBankComplementRnaSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleGenBankRnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate GenBank sequence and ReverseComplement the DNA sequence.
        /// Input data : GenBank DNA sequence.
        /// Output Data : Validate the expected sequence after ReverseComplement.
        /// </summary>
        [Test]
        public void ValidateGenBankReverseComplementDnaSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.ReverseComplement);
        }

        /// <summary>
        /// Validate GenBank sequence and ReverseComplement the RNA sequence.
        /// Input data : GenBank RNA sequence.
        /// Output Data : Validate the expected sequence after ReverseComplement.
        /// </summary>
        [Test]
        public void ValidateGenBankReverseComplementRnaSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleGenBankRnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.ReverseComplement);
        }

        /// <summary>
        /// Validate Fasta sequence and Reverse the RNA sequence.
        /// Input data : Fasta RNA sequence.
        /// Output Data : Validate the expected sequence after reverse.
        /// </summary>
        [Test]
        public void ValidateFastaReverseRnaSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleFastaRnaNodeName,
                ParserParameters.FastA, AdditionalParameters.Reverse);
        }

        /// <summary>
        /// Validate Fasta sequence and Reverse the Protein sequence.
        /// Input data : Fasta Protein sequence.
        /// Output Data : Validate the expected sequence after reverse.
        /// </summary>
        [Test]
        public void ValidateFastaReverseProSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleFastaProteinNodeName,
                ParserParameters.FastA, AdditionalParameters.Reverse);
        }

        /// <summary>
        /// Validate Fasta sequence and Complement the DNA sequence.
        /// Input data : Fasta DNA sequence.
        /// Output Data : Validate the expected sequence after Complement.
        /// </summary>
        [Test]
        public void ValidateFastaComplementDnaSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA, AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate Fasta sequence and Complement the RNA sequence.
        /// Input data : Fasta RNA sequence.
        /// Output Data : Validate the expected sequence after Complement.
        /// </summary>
        [Test]
        public void ValidateFastaComplementRnaSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleFastaRnaNodeName,
                ParserParameters.FastA, AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate Fasta sequence and ReverseComplement the DNA sequence.
        /// Input data : Fasta DNA sequence.
        /// Output Data : Validate the expected sequence after ReverseComplement.
        /// </summary>
        [Test]
        public void ValidateFastaReverseComplementDnaSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA, AdditionalParameters.ReverseComplement);
        }

        /// <summary>
        /// Validate Fasta sequence and ReverseComplement the RNA sequence.
        /// Input data : Fasta RNA sequence.
        /// Output Data : Validate the expected sequence after ReverseComplement.
        /// </summary>
        [Test]
        public void ValidateFastaReverseComplementRnaSequence()
        {
            ValidateReverseComplementSequenceTestCases(Constants.SimpleFastaRnaNodeName,
                ParserParameters.FastA, AdditionalParameters.ReverseComplement);
        }

        /// <summary>
        /// Validate one character Dna sequence and Reverse the sequence.
        /// Input data : DNA sequence.
        /// Output Data : Validate the expected sequence after Reverse.
        /// </summary>
        [Test]
        public void ValidateReverseDnaSingleSequence()
        {
            ValidateSingleDnaAlphabetTestCases(AdditionalParameters.Reverse);
        }

        /// <summary>
        /// Validate one character Dna sequence and Complement the sequence.
        /// Input data : DNA sequence.
        /// Output Data : Validate the expected sequence after Complement.
        /// </summary>
        [Test]
        public void ValidateComplementDnaSingleSequence()
        {
            ValidateSingleDnaAlphabetTestCases(AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate one character Dna sequence and ReverseComplement the sequence.
        /// Input data : DNA sequence.
        /// Output Data : Validate the expected sequence after ReverseComplement.
        /// </summary>
        [Test]
        public void ValidateReverseComplementDnaSingleSequence()
        {
            ValidateSingleDnaAlphabetTestCases(AdditionalParameters.ReverseComplement);
        }

        /// <summary>
        /// Validate GenBank Dna sequence and validate the Contains() method.
        /// Input data : GenBank Dna sequence.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateGenBankDnaContainsSequence()
        {
            ValidateSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank);
        }

        /// <summary>
        /// Validate Fasta Dna sequence and validate the Contains() method.
        /// Input data : Fasta Dna sequence.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateFastaDnaContainsSequence()
        {
            ValidateSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA);
        }

        /// <summary>
        /// Validate GenBank Rna sequence and validate the Contains() method.
        /// Input data : GenBank Rna sequence.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateGenBankRnaContainsSequence()
        {
            ValidateSequenceTestCases(Constants.SimpleGenBankRnaNodeName,
                ParserParameters.GenBank);
        }

        /// <summary>
        /// Validate Fasta Rna sequence and validate the Contains() method.
        /// Input data : Fasta Dna sequence.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateFastaRnaContainsSequence()
        {
            ValidateSequenceTestCases(Constants.SimpleFastaRnaNodeName,
                ParserParameters.FastA);
        }

        /// <summary>
        /// Validate GenBank Protein sequence and validate the Contains() method.
        /// Input data : GenBank Protein sequence.
        /// Output Data : Validate the expected sequence.
        /// </summary>
        [Test]
        public void ValidateGenBankProteinContainsSequence()
        {
            ValidateSequenceTestCases(Constants.SimpleGenBankProNodeName,
                ParserParameters.GenBank);
        }

        /// <summary>
        /// Validate Fasta sequence, Clone() and the count.
        /// Input data : Fasta DNA sequence.
        /// Output Data : Validate the expected sequence after Cloning.
        /// </summary>
        [Test]
        public void ValidateCloneCountSequence()
        {
            ValidateCloningSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                AdditionalParameters.Count);
        }

        /// <summary>
        /// Validate Fasta sequence, Clone() and the Complement.
        /// Input data : Fasta DNA sequence.
        /// Output Data : Validate the expected sequence after Cloning.
        /// </summary>
        [Test]
        public void ValidateCloneComplementSequence()
        {
            ValidateCloningSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate Fasta sequence, Clone() and the ISequence.
        /// Input data : Fasta DNA sequence.
        /// Output Data : Validate the expected sequence after Cloning.
        /// </summary>
        [Test]
        public void ValidateCloneISequenceSequence()
        {
            ValidateCloningSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                AdditionalParameters.Sequence);
        }

        /// <summary>
        /// Validate Fasta sequence, Clone() and the IClonable.
        /// Input data : Fasta DNA sequence.
        /// Output Data : Validate the expected sequence after Cloning.
        /// </summary>
        [Test]
        public void ValidateCloneICloneableSequence()
        {
            ValidateCloningSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                AdditionalParameters.Cloneable);
        }

        /// <summary>
        /// Validate error message for IndexOfNonGap by passing null sequence.
        /// Input data : Sequence.
        /// Output Data : Validation of Error message
        /// </summary>
        [Test]
        public void InValidateSequenceIndexOfNonGapChars()
        {
            ValidateGeneralIndexOfNonGapChars(Constants.BasicSeqInfoNode,
                AdditionalParameters.NullSequence);
        }

        /// <summary>
        /// Validate error message for IndexOfNonGap by passing invalid start position.
        /// Input data : Sequence.
        /// Output Data : Validation of Error message
        /// </summary>
        [Test]
        public void InValidateSequenceIndexOfNonGapCharsByPassingInvalidStartPos()
        {
            ValidateGeneralIndexOfNonGapChars(Constants.BasicSeqInfoNode,
                AdditionalParameters.NegativeIndex);
        }

        /// <summary>
        /// Validate error message for LastIndexOfNonGap by passing null sequence.
        /// Input data : Sequence.
        /// Output Data : Validation of Error message
        /// </summary>
        [Test]
        public void InValidateSequenceLastIndexOfNonGapChars()
        {
            ValidateGeneralIndexOfNonGapChars(Constants.BasicSeqInfoNode,
                AdditionalParameters.LastIndexNullSequence);
        }

        /// <summary>
        /// Validate error message for LastIndexOfNonGap by passing invalid start position.
        /// Input data : Sequence.
        /// Output Data : Validation of Error message
        /// </summary>
        [Test]
        public void InValidateSequenceLastIndexOfNonGapCharsByPassingInvalidStartPos()
        {
            ValidateGeneralIndexOfNonGapChars(Constants.BasicSeqInfoNode,
                AdditionalParameters.NegativeLastIndex);
        }

        /// <summary>
        /// Validate Index of Non Gap characters by passing Sequence with only gap characters.
        /// Input data : Gap characters Sequence.
        /// Output Data : Validation of IndexOfNonGap(Seq) method.
        /// </summary>
        [Test]
        public void InValidateIndexOfNonGapByPassingSeqwithOnlyGapChars()
        {
            ValidateGeneralIndexOfNonGapChars(Constants.BasicSeqInfoNode,
                AdditionalParameters.GapCharSequence);
        }

        /// <summary>
        /// Validate LastIndex of Non Gap characters by passing Sequence with only gap characters.
        /// Input data : Gap characters Sequence.
        /// Output Data : Validation of LastIndexOfNonGap(Seq) method.
        /// </summary>
        [Test]
        public void InValidateLastIndexOfNonGapByPassingSeqwithOnlyGapChars()
        {
            ValidateGeneralIndexOfNonGapChars(Constants.BasicSeqInfoNode,
                AdditionalParameters.LastIndexGapCharSequence);
        }

        /// <summary>
        /// InValidate BasicSequenceInfo GetObjectData.
        /// Input data : Sequence and null value.
        /// Output Data : InValidation of GetObjectData method..
        /// </summary>
        [Test]
        public void InValidateBasicSequenceInfoGetObjectData()
        {
            string expectedErrorMsg = Utility._xmlUtil.GetTextValue(Constants.BasicSeqInfoNode,
                Constants.NullSeqInfoErrorMessageNode);
            string actualError = null;

            // Create a BasicSequenceInfo object.
            BasicSequenceInfo basicSeqInfoObj = new BasicSequenceInfo();
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            // Pass a null value to GetObjectData method.
            try
            {
                basicSeqInfoObj.GetObjectData(null, context);
            }
            catch (ArgumentNullException ex)
            {
                actualError = ex.Message;
            }

            // Validate Sequence.
            Assert.AreEqual(expectedErrorMsg, actualError.Replace("\r", "").Replace("\n", ""));
            ApplicationLog.WriteLine("Sequence P2: Successfully Invalidated GetObjectData() method");
            Console.WriteLine("Sequence P2: Successfully Invalidated Add() GetObjectData() method");
        }

        /// <summary>
        /// Invalidate Sequence Range operation cases.
        /// Input data : Invalid sequence range values.
        /// Output Data : InValidation of Range method.
        /// </summary>
        [Test]
        public void InvalidateSequenceRange()
        {
            InvalidateSequenceOperations(AdditionalParameters.Range);
        }

        /// <summary>
        /// Invalidate Sequence RemoveRange operation cases.
        /// Input data : Invalid sequence range values.
        /// Output Data : InValidation of RemoveRange method.
        /// </summary>
        [Test]
        public void InvalidateSequenceRemoveRange()
        {
            InvalidateSequenceOperations(AdditionalParameters.RemoveRange);
        }

        /// <summary>
        /// Invalidate Sequence Replace characters operation cases.
        /// Input data : Invalid sequence range values.
        /// Output Data : InValidation of Replace method.
        /// </summary>
        [Test]
        public void InvalidateSequenceReplaceChars()
        {
            InvalidateSequenceOperations(AdditionalParameters.Replace);
        }

        /// <summary>
        /// Invalidate Sequence Replace sequence item cases.
        /// Input data : Invalid sequence range values.
        /// Output Data : InValidation of Replace method.
        /// </summary>
        [Test]
        public void InvalidateSequenceReplaceSeqItem()
        {
            InvalidateSequenceOperations(AdditionalParameters.ReplaceSeqItem);
        }

        /// <summary>
        /// Invalidate Sequence ReplaceRange operation cases.
        /// Input data : Invalid sequence range values.
        /// Output Data : InValidation of Replace method.
        /// </summary>
        [Test]
        public void InvalidateSequenceReplaceRange()
        {
            InvalidateSequenceOperations(AdditionalParameters.ReplaceRange);
        }

        /// <summary>
        /// Invalidate Sequence Insert operation cases.
        /// Input data : Invalid sequence insert operations.
        /// Output Data : InValidation of Insert method.
        /// </summary>
        [Test]
        public void InvalidateSequenceInsert()
        {
            InvalidateSequenceOperations(AdditionalParameters.Insert);
        }

        /// <summary>
        /// Invalidate Sequence InsertRange operation cases.
        /// Input data : Invalid sequence insert operations.
        /// Output Data : InValidation of InsertRange method.
        /// </summary>
        [Test]
        public void InvalidateSequenceInsertRange()
        {
            InvalidateSequenceOperations(AdditionalParameters.InsertRange);
        }

        /// <summary>
        /// Invalidate Pattern Converter.
        /// Input data : Empty sequence.
        /// Output Data : InValidation of Convert method.
        /// </summary>
        [Test]
        public void InValidateEmptyPattern()
        {
            try
            {
                IPatternConverter patternConverterObj =
                    PatternConverter.GetInstanace(DnaAlphabet.Instance);
                patternConverterObj.Convert(string.Empty);
                Assert.Fail();
            }
            catch
            {
                Console.WriteLine("Successfully validated the Convert() method with Empty Pattern");
                ApplicationLog.WriteLine("Successfully validated the Convert() method with Empty Pattern");
            }
        }

        /// <summary>
        /// Left angled pattern convert() method validation.
        /// </summary>
        [Test]
        public void InValidateConvertLeftAngledPattern()
        {
            try
            {
                IPatternConverter patternConverterObj =
                    PatternConverter.GetInstanace(DnaAlphabet.Instance);
                patternConverterObj.Convert("<<");
                Assert.Fail();
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Successfully validated the Convert() method with Left Angled Pattern");
                ApplicationLog.WriteLine(
                    "Successfully validated the Convert() method with Left Angled Pattern");
            }
        }

        /// <summary>
        /// Right angled pattern convert() method validation.
        /// </summary>
        [Test]
        public void InValidateConvertRightAngledPattern()
        {
            try
            {
                IPatternConverter patternConverterObj =
                    PatternConverter.GetInstanace(DnaAlphabet.Instance);
                patternConverterObj.Convert(">>");
                Assert.Fail();
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Successfully validated the Convert() method with Right Angled Pattern");
                ApplicationLog.WriteLine(
                    "Successfully validated the Convert() method with Right Angled Pattern");
            }
        }

        /// <summary>
        /// Invalid Parenthesis pattern convert() method validation.
        /// </summary>
        [Test]
        public void InValidateConvertInvalidParanthesisPattern()
        {
            try
            {
                IPatternConverter patternConverterObj =
                    PatternConverter.GetInstanace(DnaAlphabet.Instance);
                patternConverterObj.Convert("()");
                Assert.Fail();
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Successfully validated the Convert() method with Invalid Paranthesis Pattern");
                ApplicationLog.WriteLine(
                    "Successfully validated the Convert() method with Invalid Paranthesis Pattern");
            }
        }

        /// <summary>
        /// Right Parenthesis pattern convert() method validation.
        /// </summary>
        [Test]
        public void InValidateConvertRightParanthesisPattern()
        {
            try
            {
                IPatternConverter patternConverterObj =
                    PatternConverter.GetInstanace(DnaAlphabet.Instance);
                patternConverterObj.Convert("))");
                Assert.Fail();
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Successfully validated the Convert() method with Right Paranthesis Pattern");
                ApplicationLog.WriteLine(
                    "Successfully validated the Convert() method with Right Paranthesis Pattern");
            }
        }

        /// <summary>
        /// Right SquareBracket pattern convert() method validation.
        /// </summary>
        [Test]
        public void InValidateConvertRightSquareBracketPattern()
        {
            try
            {
                IPatternConverter patternConverterObj =
                    PatternConverter.GetInstanace(DnaAlphabet.Instance);
                patternConverterObj.Convert("]]");
                Assert.Fail();
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Successfully validated the Convert() method with Right SquareBracket Pattern");
                ApplicationLog.WriteLine(
                    "Successfully validated the Convert() method with Right SquareBracket Pattern");
            }
        }

        /// <summary>
        /// First index Right CurlyBracket pattern convert() method validation.
        /// </summary>
        [Test]
        public void InValidateConvertFirstRightCurlyBracketPattern()
        {
            try
            {
                IPatternConverter patternConverterObj =
                    PatternConverter.GetInstanace(DnaAlphabet.Instance);
                patternConverterObj.Convert("}}");
                Assert.Fail();
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Successfully validated the Convert() method with first index Right CurlyBracket Pattern");
                ApplicationLog.WriteLine(
                    "Successfully validated the Convert() method with first index Right CurlyBracket Pattern");
            }
        }

        /// <summary>
        /// Right CurlyBracket pattern convert() method validation.
        /// </summary>
        [Test]
        public void InValidateConvertRightCurlyBracketPattern()
        {
            try
            {
                IPatternConverter patternConverterObj =
                    PatternConverter.GetInstanace(DnaAlphabet.Instance);
                patternConverterObj.Convert("A{}");
                Assert.Fail();
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Successfully validated the Convert() method with Right CurlyBracket Pattern");
                ApplicationLog.WriteLine(
                    "Successfully validated the Convert() method with Right CurlyBracket Pattern");
            }
        }

        /// <summary>
        /// Left Right CurlyBracket pattern convert() method validation.
        /// </summary>
        [Test]
        public void InValidateConvertLeftRightCurlyBracketPattern()
        {
            try
            {
                IPatternConverter patternConverterObj =
                    PatternConverter.GetInstanace(DnaAlphabet.Instance);
                patternConverterObj.Convert("A{{}");
                Assert.Fail();
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Successfully validated the Convert() method with Left Right CurlyBracket Pattern");
                ApplicationLog.WriteLine(
                    "Successfully validated the Convert() method with Left Right CurlyBracket Pattern");
            }
        }

        /// <summary>
        /// Left SquareBracket pattern convert() method validation.
        /// </summary>
        [Test]
        public void InValidateConvertLeftSquareBracketPattern()
        {
            try
            {
                IPatternConverter patternConverterObj =
                    PatternConverter.GetInstanace(DnaAlphabet.Instance);
                patternConverterObj.Convert("A[]");
                Assert.Fail();
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Successfully validated the Convert() method with Left SquareBracket Pattern");
                ApplicationLog.WriteLine(
                    "Successfully validated the Convert() method with Left SquareBracket Pattern");
            }
        }

        /// <summary>
        /// Left Right SquareBracket pattern convert() method validation.
        /// </summary>
        [Test]
        public void InValidateConvertLeftRightSquareBracketPattern()
        {
            try
            {
                IPatternConverter patternConverterObj =
                    PatternConverter.GetInstanace(DnaAlphabet.Instance);
                patternConverterObj.Convert("A[[]");
                Assert.Fail();
            }
            catch (FormatException)
            {
                Console.WriteLine(
                    "Successfully validated the Convert() method with Left Right SquareBracket Pattern");
                ApplicationLog.WriteLine(
                    "Successfully validated the Convert() method with Left Right SquareBracket Pattern");
            }
        }

        #endregion Sequence P2 TestCases

        #region BasicDerivedSequence P2 Test cases

        /// <summary>
        /// Validate a Basic DerivedSequence creation for a Dna with invalid Index.
        /// Input Data : Valid Dna Sequence.
        /// Output Data : Validatation for invalid index.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceDnaInvalidIndex()
        {
            ValidateInvalidIndexTestCases(AdditionalParameters.Dna);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence creation for a Rna with invalid Index.
        /// Input Data : Valid Rna Sequence.
        /// Output Data : Validatation for invalid index.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceRnaInvalidIndex()
        {
            ValidateInvalidIndexTestCases(AdditionalParameters.Rna);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence creation for a Protein with invalid Index.
        /// Input Data : Valid Protein Sequence.
        /// Output Data : Validatation for invalid index.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceProteinInvalidIndex()
        {
            ValidateInvalidIndexTestCases(AdditionalParameters.Protein);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence complement for a Dna.
        /// Input Data : Valid Dna Sequence.
        /// Output Data : Validatation for complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceDnaComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA, AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence complement for a Dna from FastA file.
        /// Input Data : Valid Fasta Dna Sequence.
        /// Output Data : Validatation for complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceDnaFastaComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA, AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence complement for a GenBank Dna.
        /// Input Data : Valid GenBank Dna Sequence.
        /// Output Data : Validatation for complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceDnaGenBankComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence Reverse for a Dna from FastA file.
        /// Input Data : Valid Fasta Dna Sequence.
        /// Output Data : Validatation for Reverse.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceDnaFastaReverse()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA, AdditionalParameters.Reverse);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence Reverse for a GenBank Dna.
        /// Input Data : Valid GenBank Dna Sequence.
        /// Output Data : Validatation for Reverse.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceDnaGenBankReverse()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Reverse);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence Reverse for a Rna from FastA file.
        /// Input Data : Valid Fasta Rna Sequence.
        /// Output Data : Validatation for Reverse.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceRnaFastaReverse()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleFastaRnaNodeName,
                ParserParameters.FastA, AdditionalParameters.Reverse);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence Reverse for a GenBank Rna.
        /// Input Data : Valid GenBank Rna Sequence.
        /// Output Data : Validatation for Reverse.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceRnaGenBankReverse()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleGenBankRnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Reverse);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence Reverse for a Protein from FastA file.
        /// Input Data : Valid Fasta Protein Sequence.
        /// Output Data : Validatation for Reverse.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceProteinFastaReverse()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleFastaProteinNodeName,
                ParserParameters.FastA, AdditionalParameters.Reverse);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence Reverse for a GenBank Protein.
        /// Input Data : Valid GenBank Protein Sequence.
        /// Output Data : Validatation for Reverse.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceProteinGenBankReverse()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleGenBankProNodeName,
                ParserParameters.GenBank, AdditionalParameters.Reverse);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence reverse complement for a Dna.
        /// Input Data : Valid Dna Sequence.
        /// Output Data : Validatation for Reverse complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceDnaReverseComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA, AdditionalParameters.ReverseComplement);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence Reverse complement for a Dna from FastA file.
        /// Input Data : Valid Fasta Dna Sequence.
        /// Output Data : Validatation for Reverse complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceDnaFastaReverseComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                ParserParameters.FastA, AdditionalParameters.ReverseComplement);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence Reverse complement for a GenBank Dna.
        /// Input Data : Valid GenBank Dna Sequence.
        /// Output Data : Validatation for Reverse complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceDnaGenBankReverseComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.ReverseComplement);
        }

        /// <summary>
        /// Validate Fasta sequence, Clone() for BasicDerivedSequence and the count.
        /// Input data : Fasta DNA sequence.
        /// Output Data : Validate the expected sequence after Cloning.
        /// </summary>
        [Test]
        public void ValidateCloneCountBasicDerivedSequence()
        {
            ValidateBasicDerivedCloningSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                AdditionalParameters.Count);
        }

        /// <summary>
        /// Validate Fasta sequence, Clone() BasicDerivedSequence and the Complement.
        /// Input data : Fasta DNA sequence.
        /// Output Data : Validate the expected sequence after Cloning.
        /// </summary>
        [Test]
        public void ValidateCloneComplementBasicDerivedSequence()
        {
            ValidateBasicDerivedCloningSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate Fasta sequence, Clone() BasicDerivedSequence and the ISequence.
        /// Input data : Fasta DNA sequence.
        /// Output Data : Validate the expected sequence after Cloning.
        /// </summary>
        [Test]
        public void ValidateCloneISequenceBasicDerivedSequence()
        {
            ValidateBasicDerivedCloningSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                AdditionalParameters.Sequence);
        }

        /// <summary>
        /// Validate Fasta sequence, Clone() BasicDerivedSequence and the IClonable.
        /// Input data : Fasta DNA sequence.
        /// Output Data : Validate the expected sequence after Cloning.
        /// </summary>
        [Test]
        public void ValidateCloneICloneableBasicDerivedSequence()
        {
            ValidateBasicDerivedCloningSequenceTestCases(Constants.SimpleFastaDnaNodeName,
                AdditionalParameters.Cloneable);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence complement for a Rna from FastA file.
        /// Input Data : Valid Fasta Rna Sequence.
        /// Output Data : Validatation for complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceRnaFastaComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleFastaRnaNodeName,
                ParserParameters.FastA, AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence complement for a GenBank Rna.
        /// Input Data : Valid GenBank Rna Sequence.
        /// Output Data : Validatation for complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceRnaGenBankComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleGenBankRnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence Reverse complement for a Rna from FastA file.
        /// Input Data : Valid Fasta Rna Sequence.
        /// Output Data : Validatation for Reverse complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceRnaFastaReverseComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleFastaRnaNodeName,
                ParserParameters.FastA, AdditionalParameters.ReverseComplement);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence Reverse complement for a GenBank Rna.
        /// Input Data : Valid GenBank Rna Sequence.
        /// Output Data : Validatation for Reverse complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceRnaGenBankReverseComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleGenBankRnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.ReverseComplement);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence complement for a Protein from FastA file.
        /// Input Data : Valid Fasta Rna Sequence.
        /// Output Data : Validatation for complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceProteinFastaComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleFastaProteinNodeName,
                ParserParameters.FastA, AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence complement for a GenBank Protein.
        /// Input Data : Valid GenBank Protein Sequence.
        /// Output Data : Validatation for complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceProteinGenBankComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleGenBankProNodeName,
                ParserParameters.GenBank, AdditionalParameters.Complement);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence Reverse complement for a Protein from FastA file.
        /// Input Data : Valid Fasta Protein Sequence.
        /// Output Data : Validatation for Reverse complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceProteinFastaReverseComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleFastaProteinNodeName,
                ParserParameters.FastA, AdditionalParameters.ReverseComplement);
        }

        /// <summary>
        /// Validate a Basic DerivedSequence Reverse complement for a GenBank Protein.
        /// Input Data : Valid GenBank Protein Sequence.
        /// Output Data : Validatation for Reverse complement.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceProteinGenBankReverseComplement()
        {
            ValidateReverseComplementDerivedSequenceTestCases(Constants.SimpleGenBankProNodeName,
                ParserParameters.GenBank, AdditionalParameters.ReverseComplement);
        }

        /// <summary>
        /// Validate GenBank BasicDerived sequence and Insert() characters.
        /// Input data : Genbank BasicDerived sequence and Insert() "G" characters.
        /// Output Data : Validate the expected error.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedInvalidDnaGenBankInsertSequence()
        {
            ValidateBasicDerivedInsertSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Insert);
        }

        /// <summary>
        /// Validate GenBank BasicDerived sequence and Insert() characters.
        /// Input data : Genbank BasicDerived sequence and Insert() "G" characters.
        /// Output Data : Validate the expected error.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedInvalidRnaGenBankInsertSequence()
        {
            ValidateBasicDerivedInsertSequenceTestCases(Constants.SimpleGenBankRnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Insert);
        }

        /// <summary>
        /// Validate GenBank BasicDerived sequence and Insert() characters.
        /// Input data : Genbank BasicDerived sequence and Insert() "G" characters.
        /// Output Data : Validate the expected error.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedInvalidProGenBankInsertSequence()
        {
            ValidateBasicDerivedInsertSequenceTestCases(Constants.SimpleGenBankProNodeName,
                ParserParameters.GenBank, AdditionalParameters.Insert);
        }

        /// <summary>
        /// Validate GenBank BasicDerived sequence and Remove() characters.
        /// Input data : Genbank BasicDerived sequence and Remove() "G" characters.
        /// Output Data : Validate the expected error.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedInvalidDnaGenBankRemoveSequence()
        {
            ValidateBasicDerivedInsertSequenceTestCases(Constants.SimpleGenBankDnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Remove);
        }

        /// <summary>
        /// Validate GenBank BasicDerived sequence and Remove() characters.
        /// Input data : Genbank BasicDerived sequence and Remove() "G" characters.
        /// Output Data : Validate the expected error.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedInvalidRnaGenBankRemoveSequence()
        {
            ValidateBasicDerivedInsertSequenceTestCases(Constants.SimpleGenBankRnaNodeName,
                ParserParameters.GenBank, AdditionalParameters.Remove);
        }

        /// <summary>
        /// Validate GenBank BasicDerived sequence and Remove() characters.
        /// Input data : Genbank BasicDerived sequence and Remove() "G" characters.
        /// Output Data : Validate the expected error.
        /// </summary>
        [Test]
        public void ValidateBasicDerivedInvalidProGenBankRemoveSequence()
        {
            ValidateBasicDerivedInsertSequenceTestCases(Constants.SimpleGenBankProNodeName,
                ParserParameters.GenBank, AdditionalParameters.Remove);
        }

        /// <summary>
        /// Validate BasicDerivedSequence CopyTo().
        /// Input data : Sequence.
        /// Output Data : Validation of CopyTo() method..
        /// </summary>
        [Test]
        public void ValidateBasicDerivedSequenceCopyTo()
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

            ISequenceItem[] sequenceItems = new
                ISequenceItem[basicDerSeqObj.Count];

            try
            {
                basicDerSeqObj.CopyTo(sequenceItems, -1);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine(string.Format("Successfully caught the exception '{0}'",
                    ex.Message));
                ApplicationLog.WriteLine(string.Format("Successfully caught the exception '{0}'",
                    ex.Message));
            }
        }

        /// <summary>
        /// Invalidate Basic derived sequence NotSupportedMethods.
        /// Input : Basic derived sequence
        /// Output : Exception validation.
        /// </summary>
        [Test]
        public void InValidateBasicDerivedSeqMethods()
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.AlphabetNameNode);
            string sequence = Utility._xmlUtil.GetTextValue(Constants.SimpleFastaNodeName,
                Constants.ExpectedSequence);
            string exceptionMessage = null;

            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequence);

            // Create a BasicDerivedSequence object.
            BasicDerivedSequence basicDerSeqObj = new BasicDerivedSequence(seq, false, false,
                0, seq.Count);

            // Invalidate a Basic derived sequence methods.
            try
            {
                basicDerSeqObj.RemoveRange(0, 2);
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                exceptionMessage = ex.Message;
                Console.WriteLine(string.Format("Sequence P2 : Successfully caught the exception '{0}'",
                    exceptionMessage));
                ApplicationLog.WriteLine(string.Format("Sequence P2 : Successfully caught the exception '{0}'",
                    exceptionMessage));
            }

            // Invalidate Replace methods.
            try
            {
                basicDerSeqObj.Replace(0, 'C');
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                exceptionMessage = ex.Message;
                Console.WriteLine(string.Format("Sequence P2 : Successfully caught the exception '{0}'",
                    exceptionMessage));
                ApplicationLog.WriteLine(string.Format("Sequence P2 : Successfully caught the exception '{0}'",
                    exceptionMessage));
            }

            try
            {
                basicDerSeqObj.Replace(0, basicDerSeqObj[0]);
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                exceptionMessage = ex.Message;
                Console.WriteLine(string.Format("Sequence P2 : Successfully caught the exception '{0}'",
                    exceptionMessage));
                ApplicationLog.WriteLine(string.Format("Sequence P2 : Successfully caught the exception '{0}'",
                    exceptionMessage));
            }

            // Invalidate ReplaceRange() method.
            try
            {
                basicDerSeqObj.ReplaceRange(0, "ACG");
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                exceptionMessage = ex.Message;
                Console.WriteLine(string.Format("Sequence P2 : Successfully caught the exception '{0}'",
                    exceptionMessage));
                ApplicationLog.WriteLine(string.Format("Sequence P2 : Successfully caught the exception '{0}'",
                    exceptionMessage));
            }

            // Invalidate Propeties.
            try
            {
                basicDerSeqObj.Documentation = "Basic Derived Sequence";
                Assert.Fail();
            }
            catch (NotSupportedException ex)
            {
                exceptionMessage = ex.Message;
                Console.WriteLine(string.Format("Sequence P2 : Successfully caught the exception '{0}'",
                    exceptionMessage));
                ApplicationLog.WriteLine(string.Format("Sequence P2 : Successfully caught the exception '{0}'",
                    exceptionMessage));
            }
        }

        #endregion BasicDerivedSequence P2 Test cases

        #region Supported Methods

        /// <summary>
        /// Validates Add() method for Add test cases
        /// </summary>
        /// <param name="nodeName">Xml Node name</param>
        /// <param name="parserParam">Fasta/GenBank</param>
        static void ValidateSizeAddSequenceTestCases(string nodeName,
            ParserParameters parserParam)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null, "Sequence P2: The File exist in the Path {0}.",
                filePath));

            IList<ISequence> seqList = null;

            // Parse the files based on GenBank/FastA
            switch (parserParam)
            {
                case ParserParameters.GenBank:
                    GenBankParser genBankParser = new GenBankParser();
                    seqList = genBankParser.Parse(filePath);
                    break;
                default:
                    FastaParser fastaParser = new FastaParser();
                    seqList = fastaParser.Parse(filePath);
                    break;
            }

            // Validation of Add() method
            Sequence sequenceObj = (Sequence)seqList[0];
            sequenceObj.IsReadOnly = false;
            string seqBeforeAdding = sequenceObj.ToString();
            ISequence dnaSeq = new Sequence(Alphabets.DNA, "T");
            sequenceObj.Add(dnaSeq[0]);
            Assert.AreEqual(string.Concat(seqBeforeAdding, "T"), sequenceObj.ToString());
            ApplicationLog.WriteLine("Sequence P2: Successfully validated Add() sequence.");
            Console.WriteLine("Sequence P2: Successfully validated Add() sequence.");
        }

        /// <summary>
        /// Validates Insert(), Remove(), Replace() and ReplaceRange() method for Add test cases
        /// </summary>
        /// <param name="nodeName">Xml Node name</param>
        /// <param name="parserParam">Fasta/GenBank</param>
        /// <param name="addParam">Additional Parameter to specify the method to execute</param>
        static void ValidateInsertRemoveReplaceSequenceTestCases(string nodeName,
            ParserParameters parserParam, AdditionalParameters addParam)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null, "Sequence P2: The File exist in the Path {0}.",
                filePath));

            IList<ISequence> seqList = null;

            // Parse the file based on the input file type
            switch (parserParam)
            {
                case ParserParameters.GenBank:
                    GenBankParser genBankParser = new GenBankParser();
                    seqList = genBankParser.Parse(filePath);
                    break;
                default:
                    FastaParser fastaParser = new FastaParser();
                    seqList = fastaParser.Parse(filePath);
                    break;
            }

            Sequence sequenceObj = (Sequence)seqList[0];
            sequenceObj.IsReadOnly = false;
            string seqBeforeAdding = sequenceObj.ToString();

            // Insert few objects to the sequences
            sequenceObj.Insert(0, 'G');
            sequenceObj.Insert(1, 'A');
            sequenceObj.Insert(2, 'A');
            sequenceObj.Insert(3, 'T');

            // Validate based on the additional parameter for Replace, Insert, Remove and ReplaceRange
            switch (addParam)
            {
                case AdditionalParameters.Insert:
                    Assert.AreEqual(string.Concat("GAAT", seqBeforeAdding), sequenceObj.ToString());
                    ApplicationLog.WriteLine("Sequence P2: Successfully validated Insert() sequence.");
                    Console.WriteLine("Sequence P2: Successfully validated Insert() sequence.");
                    break;
                case AdditionalParameters.Remove:
                    for (int i = 0; i < 4; i++)
                    {
                        ISequenceItem seqItemObj = sequenceObj[0];
                        sequenceObj.Remove(seqItemObj);
                        switch (i)
                        {
                            case 0:
                                Assert.AreEqual(string.Concat("AAT", seqBeforeAdding),
                                    sequenceObj.ToString());
                                break;
                            case 1:
                                Assert.AreEqual(string.Concat("AT", seqBeforeAdding),
                                    sequenceObj.ToString());
                                break;
                            case 2:
                                Assert.AreEqual(string.Concat("T", seqBeforeAdding),
                                    sequenceObj.ToString());
                                break;
                            default:
                                break;
                        }
                    }
                    Assert.AreEqual(seqBeforeAdding, sequenceObj.ToString());
                    ApplicationLog.WriteLine("Sequence P2: Successfully validated Remove() sequence.");
                    Console.WriteLine("Sequence P2: Successfully validated Remove() sequence.");
                    break;
                case AdditionalParameters.Replace:
                    sequenceObj.Replace(0, 'T');
                    Assert.AreEqual(string.Concat("TAAT", seqBeforeAdding), sequenceObj.ToString());
                    sequenceObj.Replace(1, 'G');
                    Assert.AreEqual(string.Concat("TGAT", seqBeforeAdding), sequenceObj.ToString());
                    sequenceObj.Replace(2, 'G');
                    Assert.AreEqual(string.Concat("TGGT", seqBeforeAdding), sequenceObj.ToString());
                    sequenceObj.Replace(3, 'A');
                    Assert.AreEqual(string.Concat("TGGA", seqBeforeAdding), sequenceObj.ToString());
                    ApplicationLog.WriteLine("Sequence P2: Successfully validated Replace() sequence.");
                    Console.WriteLine("Sequence P2: Successfully validated Replace() sequence.");
                    break;
                case AdditionalParameters.ReplaceRange:
                    sequenceObj.ReplaceRange(0, "TGGA");
                    Assert.AreEqual(string.Concat("TGGA", seqBeforeAdding), sequenceObj.ToString());
                    ApplicationLog.WriteLine("Sequence P2: Successfully validated ReplaceRange() sequence.");
                    Console.WriteLine("Sequence P2: Successfully validated ReplaceRange() sequence.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Validates Reverse(), Complement(), ReverseComplement() method for several test cases
        /// </summary>
        /// <param name="nodeName">Xml Node name</param>
        /// <param name="parserParam">Fasta/GenBank</param>
        /// <param name="addParam">Additional Parameter to specify the method to execute</param>
        static void ValidateReverseComplementSequenceTestCases(string nodeName,
            ParserParameters parserParam, AdditionalParameters addParam)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Sequence P2: The File exist in the Path {0}.",
                filePath));

            IList<ISequence> seqList = null;
            // Parse the file based on the input file type
            switch (parserParam)
            {
                case ParserParameters.GenBank:
                    GenBankParser genBankParser = new GenBankParser();
                    seqList = genBankParser.Parse(filePath);
                    break;
                default:
                    FastaParser fastaParser = new FastaParser();
                    seqList = fastaParser.Parse(filePath);
                    break;
            }

            Sequence sequenceObj = (Sequence)seqList[0];

            // Validation of Reverse(), Complement() and ReverseComplement() method
            switch (addParam)
            {
                case AdditionalParameters.Reverse:
                    string revSeq = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ReverseSequenceNode);
                    ISequence revSeqObj = sequenceObj.Reverse;
                    Assert.AreEqual(revSeq, revSeqObj.ToString());
                    ApplicationLog.WriteLine("Sequence P2: Successfully validated the reverse string.");
                    Console.WriteLine("Sequence P2: Successfully validated the reverse string.");
                    break;
                case AdditionalParameters.Complement:
                    string comSeq = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ComplementSequenceNode);
                    ISequence compSeqObj = sequenceObj.Complement;
                    Assert.AreEqual(comSeq, compSeqObj.ToString());
                    ApplicationLog.WriteLine("Sequence P2: Successfully validated the complement string.");
                    Console.WriteLine("Sequence P2: Successfully validated the complement string.");
                    break;
                case AdditionalParameters.ReverseComplement:
                    string revCompSeq = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ReverseComplementSequenceNode);
                    ISequence revCompSeqObj = sequenceObj.ReverseComplement;
                    Assert.AreEqual(revCompSeq, revCompSeqObj.ToString());
                    ApplicationLog.WriteLine(
                        "Sequence P2: Successfully validated the reverse complement string.");
                    Console.WriteLine(
                        "Sequence P2: Successfully validated the reverse complement string.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Validates Reverse(), Complement(), ReverseComplement() method for several test cases
        /// </summary>
        /// <param name="addParam">Additional Parameter to specify the method to execute</param>
        static void ValidateSingleDnaAlphabetTestCases(AdditionalParameters addParam)
        {
            // Validation of Reverse(), Complement() and ReverseComplement() method
            switch (addParam)
            {
                case AdditionalParameters.Reverse:
                    Sequence sequenceObj = new Sequence(Alphabets.DNA, "G");
                    ISequence revSeqObj = sequenceObj.Reverse;
                    Assert.AreEqual("G", revSeqObj.ToString());
                    ApplicationLog.WriteLine("Sequence P2: Successfully validated the reverse string.");
                    Console.WriteLine("Sequence P2: Successfully validated the reverse string.");
                    break;
                case AdditionalParameters.Complement:
                    Sequence seqObj = new Sequence(Alphabets.DNA, "G");
                    ISequence compSeqObj = seqObj.Complement;
                    Assert.AreEqual("C", compSeqObj.ToString());
                    ApplicationLog.WriteLine("Sequence P2: Successfully validated the complement string.");
                    Console.WriteLine("Sequence P2: Successfully validated the complement string.");
                    break;
                case AdditionalParameters.ReverseComplement:
                    Sequence revSeqsObj = new Sequence(Alphabets.DNA, "G");
                    ISequence revCompSeqObj = revSeqsObj.ReverseComplement;
                    Assert.AreEqual("C", revCompSeqObj.ToString());
                    ApplicationLog.WriteLine(
                        "Sequence P2: Successfully validated the reverse complement string.");
                    Console.WriteLine(
                        "Sequence P2: Successfully validated the reverse complement string.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Validates sequence test cases
        /// </summary>
        /// <param name="nodeName">Xml Node name</param>
        /// <param name="parserParam">Fasta/GenBank</param>
        static void ValidateSequenceTestCases(string nodeName,
            ParserParameters parserParam)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null, "Sequence P2: The File exist in the Path {0}.",
                filePath));

            IList<ISequence> seqList = null;

            // Parse the file based on the input file type
            switch (parserParam)
            {
                case ParserParameters.GenBank:
                    GenBankParser genBankParser = new GenBankParser();
                    seqList = genBankParser.Parse(filePath);
                    break;
                default:
                    FastaParser fastaParser = new FastaParser();
                    seqList = fastaParser.Parse(filePath);
                    break;
            }

            // Get the object for validation
            Sequence sequenceObj = (Sequence)seqList[0];
            ISequenceItem seqItem = sequenceObj[10];
            Assert.IsTrue(sequenceObj.Contains(seqItem));
            ApplicationLog.WriteLine(
                "Sequence P2 : Successfully validated the sequence item for Contains() method.");
            Console.WriteLine(
                "Sequence P2 : Successfully validated the sequence item for Contains() method.");
        }

        /// <summary>
        /// Validates Clone() method for several test cases
        /// </summary>
        /// <param name="nodeName">Xml Node name</param>
        /// <param name="addParam">Additional Parameter to specify the method to execute</param>
        static void ValidateCloningSequenceTestCases(string nodeName,
            AdditionalParameters addParam)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null, "Sequence P2: The File exist in the Path {0}.",
                filePath));

            // Parse a particular file for getting the sequence
            FastaParser fastaParser = new FastaParser();
            IList<ISequence> seqList = fastaParser.Parse(filePath);

            Sequence sequenceObj = (Sequence)seqList[0];

            // Validate based on the Parameters
            switch (addParam)
            {
                case AdditionalParameters.Count:
                    Sequence cloneSeqObj = sequenceObj.Clone();
                    Assert.AreEqual(cloneSeqObj.Count, sequenceObj.Count);
                    ApplicationLog.WriteLine(
                        "Sequence P2: Successfully validated the Count for Cloned sequence.");
                    Console.WriteLine(
                        "Sequence P2: Successfully validated the Count for Cloned sequence.");
                    break;
                case AdditionalParameters.Complement:
                    Sequence compCloneSeqObj = sequenceObj.Clone();
                    Assert.AreEqual(compCloneSeqObj.Complement.ToString(), sequenceObj.Complement.ToString());
                    ApplicationLog.WriteLine(
                        "Sequence P2: Successfully validated the Complement for Cloned sequence.");
                    Console.WriteLine(
                        "Sequence P2: Successfully validated the Complement for Cloned sequence.");
                    break;
                case AdditionalParameters.Sequence:
                    ISequence interSeqObj = sequenceObj.Clone();
                    Assert.AreEqual(interSeqObj.ToString(), sequenceObj.ToString());
                    ApplicationLog.WriteLine(
                        "Sequence P2: Successfully validated the ISequence for Cloned sequence.");
                    Console.WriteLine(
                        "Sequence P2: Successfully validated the ISequence for Cloned sequence.");
                    break;
                case AdditionalParameters.Cloneable:
                    ICloneable icloneSeqObj = sequenceObj.Clone();
                    Assert.AreEqual(icloneSeqObj.ToString(), sequenceObj.ToString());
                    ApplicationLog.WriteLine(
                        "Sequence P2: Successfully validated the IClonable for Cloned sequence.");
                    Console.WriteLine(
                        "Sequence P2: Successfully validated the IClonable for Cloned sequence.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Validate Invalid index test case
        /// </summary>
        /// <param name="addParam">Additional Parameter to specify the method to execute</param>
        static void ValidateInvalidIndexTestCases(AdditionalParameters addParam)
        {
            Sequence seqObj = null;
            ISequenceItem seqItemObj = null;
            BasicDerivedSequence basicDerivedSeq = null;
            switch (addParam)
            {
                case AdditionalParameters.Dna:
                    seqObj = new Sequence(Alphabets.DNA, "AGT");
                    Assert.IsNotNull(seqObj);

                    // Create a derived Sequences for a Genebank file sequence.
                    basicDerivedSeq = new BasicDerivedSequence(seqObj, false, false, 0, 3);
                    Sequence tempDnaObj = new Sequence(Alphabets.DNA, "C");
                    seqItemObj = tempDnaObj[0];
                    break;
                case AdditionalParameters.Rna:
                    seqObj = new Sequence(Alphabets.RNA, "AGU");
                    Assert.IsNotNull(seqObj);

                    // Create a derived Sequences for a Genebank file sequence.
                    basicDerivedSeq = new BasicDerivedSequence(seqObj, false, false, 0, 3);
                    Sequence tempRnaObj = new Sequence(Alphabets.RNA, "C");
                    seqItemObj = tempRnaObj[0];
                    break;
                case AdditionalParameters.Protein:
                    seqObj = new Sequence(Alphabets.Protein, "KIE");
                    Assert.IsNotNull(seqObj);

                    // Create a derived Sequences for a Genebank file sequence.
                    basicDerivedSeq = new BasicDerivedSequence(seqObj, false, false, 0, 3);
                    Sequence tempProObj = new Sequence(Alphabets.Protein, "J");
                    seqItemObj = tempProObj[0];
                    break;
                default:
                    break;
            }

            // Validate the DerivedSequence with originalSequence.
            Assert.IsNotNull(basicDerivedSeq);
            int indexValue = basicDerivedSeq.IndexOf(seqItemObj);

            Assert.AreEqual(-1, indexValue);
            ApplicationLog.WriteLine(
                "Sequence P2: The BasicDerived Sequence with invalid index is validated.");

            // Logs to Nunit GUI.
            Console.WriteLine(
                "Sequence P2: The BasicDerived Sequence with invalid index is validated.");
        }

        /// <summary>
        /// Validates Reverse(), Complement(), ReverseComplement() method for basic derived test cases
        /// </summary>
        /// <param name="nodeName">Xml Node name</param>
        /// <param name="parserParam">Fasta/GenBank</param>
        /// <param name="addParam">Additional Parameter to specify the method to execute</param>
        static void ValidateReverseComplementDerivedSequenceTestCases(string nodeName,
            ParserParameters parserParam, AdditionalParameters addParam)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null, "Sequence P2: The File exist in the Path {0}.",
                filePath));

            IList<ISequence> seqList = null;
            // Parse the file based on the input file type
            switch (parserParam)
            {
                case ParserParameters.GenBank:
                    GenBankParser genBankParser = new GenBankParser();
                    seqList = genBankParser.Parse(filePath);
                    break;
                default:
                    FastaParser fastaParser = new FastaParser();
                    seqList = fastaParser.Parse(filePath);
                    break;
            }

            Sequence sequenceObj = (Sequence)seqList[0];
            BasicDerivedSequence derivedSeqObj =
                new BasicDerivedSequence(sequenceObj, false, false, 0, -1);

            // Validation of Reverse(), Complement() and ReverseComplement() method
            switch (addParam)
            {
                case AdditionalParameters.Reverse:
                    string revSeq = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.ReverseSequenceNode);
                    ISequence revSeqObj = derivedSeqObj.Reverse;
                    Assert.AreEqual(revSeq, revSeqObj.ToString());
                    ApplicationLog.WriteLine("Sequence P2: Successfully validated the reverse string.");
                    Console.WriteLine("Sequence P2: Successfully validated the reverse string.");
                    break;
                case AdditionalParameters.Complement:
                    // Validate for Exception for protein complement
                    if (Alphabets.Protein == sequenceObj.Alphabet)
                    {
                        string comExcep = Utility._xmlUtil.GetTextValue(nodeName,
                                                   Constants.ComplementExceptionNode);
                        string actualExcep = string.Empty;

                        try
                        {
                            ISequence compSeqObj = sequenceObj.Complement;
                        }
                        catch (NotSupportedException ex)
                        {
                            actualExcep = ex.Message;
                        }
                        Assert.AreEqual(comExcep, actualExcep);
                    }
                    else
                    {
                        string comSeq = Utility._xmlUtil.GetTextValue(nodeName,
                            Constants.ComplementSequenceNode);
                        ISequence compSeqObj = derivedSeqObj.Complement;
                        Assert.AreEqual(comSeq, compSeqObj.ToString());
                    }
                    ApplicationLog.WriteLine("Sequence P2: Successfully validated the complement string.");
                    Console.WriteLine("Sequence P2: Successfully validated the complement string.");
                    break;
                case AdditionalParameters.ReverseComplement:
                    // Validate for Exception for protein complement
                    if (Alphabets.Protein == sequenceObj.Alphabet)
                    {
                        string comExcep = Utility._xmlUtil.GetTextValue(nodeName,
                                                   Constants.ComplementExceptionNode);
                        string actualExcep = string.Empty;

                        try
                        {
                            ISequence compSeqObj = sequenceObj.ReverseComplement;
                        }
                        catch (NotSupportedException ex)
                        {
                            actualExcep = ex.Message;
                        }
                        Assert.AreEqual(comExcep, actualExcep);
                    }
                    else
                    {
                        string revCompSeq = Utility._xmlUtil.GetTextValue(nodeName,
                            Constants.ReverseComplementSequenceNode);
                        ISequence revCompSeqObj = derivedSeqObj.ReverseComplement;
                        Assert.AreEqual(revCompSeq, revCompSeqObj.ToString());
                    }
                    ApplicationLog.WriteLine(
                        "Sequence P2: Successfully validated the reverse complement string.");
                    Console.WriteLine(
                        "Sequence P2: Successfully validated the reverse complement string.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Validates Clone() method for basic derived sequence test cases
        /// </summary>
        /// <param name="nodeName">Xml Node name</param>
        /// <param name="addParam">Additional Parameter to specify the method to execute</param>
        static void ValidateBasicDerivedCloningSequenceTestCases(string nodeName,
            AdditionalParameters addParam)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null, "Sequence P2: The File exist in the Path {0}.",
                filePath));

            // Parse a particular file for getting the sequence
            FastaParser fastaParser = new FastaParser();
            IList<ISequence> seqList = fastaParser.Parse(filePath);

            Sequence sequenceObj = (Sequence)seqList[0];
            BasicDerivedSequence derivedSeqObj = new BasicDerivedSequence(sequenceObj, false, false, 0, -1);

            // Validate based on the Parameters
            switch (addParam)
            {
                case AdditionalParameters.Count:
                    BasicDerivedSequence cloneSeqObj = derivedSeqObj.Clone();
                    Assert.AreEqual(cloneSeqObj.Count, sequenceObj.Count);
                    ApplicationLog.WriteLine(
                        "Sequence P2: Successfully validated the Count for Cloned sequence.");
                    Console.WriteLine(
                        "Sequence P2: Successfully validated the Count for Cloned sequence.");
                    break;
                case AdditionalParameters.Complement:
                    BasicDerivedSequence compCloneSeqObj = derivedSeqObj.Clone();
                    Assert.AreEqual(compCloneSeqObj.Complement.ToString(), sequenceObj.Complement.ToString());
                    ApplicationLog.WriteLine(
                        "Sequence P2: Successfully validated the Complement for Cloned sequence.");
                    Console.WriteLine(
                        "Sequence P2: Successfully validated the Complement for Cloned sequence.");
                    break;
                case AdditionalParameters.Sequence:
                    ISequence interSeqObj = derivedSeqObj.Clone();
                    Assert.AreEqual(interSeqObj.ToString(), sequenceObj.ToString());
                    ApplicationLog.WriteLine(
                        "Sequence P2: Successfully validated the ISequence for Cloned sequence.");
                    Console.WriteLine(
                        "Sequence P2: Successfully validated the ISequence for Cloned sequence.");
                    break;
                case AdditionalParameters.Cloneable:
                    ICloneable icloneSeqObj = derivedSeqObj.Clone();
                    Assert.AreEqual(icloneSeqObj.ToString(), sequenceObj.ToString());
                    ApplicationLog.WriteLine(
                        "Sequence P2: Successfully validated the IClonable for Cloned sequence.");
                    Console.WriteLine(
                        "Sequence P2: Successfully validated the IClonable for Cloned sequence.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Validates Insert(), Remove() method for Add test cases
        /// </summary>
        /// <param name="nodeName">Xml Node name</param>
        /// <param name="parserParam">Fasta/GenBank</param>
        /// <param name="addParam">Additional Parameter to specify the method to execute</param>
        static void ValidateBasicDerivedInsertSequenceTestCases(string nodeName,
            ParserParameters parserParam, AdditionalParameters addParam)
        {
            // Gets the expected sequence from the Xml
            string filePath = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            Assert.IsTrue(File.Exists(filePath));

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null, "Sequence P2: The File exist in the Path {0}.",
                filePath));

            IList<ISequence> seqList = null;

            // Parse the file based on the input file type
            switch (parserParam)
            {
                case ParserParameters.GenBank:
                    GenBankParser genBankParser = new GenBankParser();
                    seqList = genBankParser.Parse(filePath);
                    break;
                default:
                    FastaParser fastaParser = new FastaParser();
                    seqList = fastaParser.Parse(filePath);
                    break;
            }

            Sequence sequenceObj = (Sequence)seqList[0];
            sequenceObj.IsReadOnly = false;

            BasicDerivedSequence deriveSeqObj =
                new BasicDerivedSequence(sequenceObj, false, false, 0, -1);

            // Validate based on the additional parameter for Insert
            switch (addParam)
            {
                case AdditionalParameters.Insert:
                    string expMessage = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.InsertExceptionNode);
                    string actualMessage = string.Empty;

                    try
                    {
                        // Insert few objects to the sequences
                        deriveSeqObj.Insert(0, 'G');
                    }
                    catch (NotSupportedException ex)
                    {
                        actualMessage = ex.Message;
                    }

                    Assert.AreEqual(expMessage, actualMessage);
                    ApplicationLog.WriteLine("Sequence P2: Successfully validated Insert() sequence.");
                    Console.WriteLine("Sequence P2: Successfully validated Insert() sequence.");
                    break;
                case AdditionalParameters.Remove:
                    string expRemoveMessage = Utility._xmlUtil.GetTextValue(nodeName,
                        Constants.RemoveExceptionNode);
                    string actualRemoveMessage = string.Empty;
                    try
                    {
                        ISequenceItem iSeqItemObj = sequenceObj[0];
                        // Insert few objects to the sequences
                        deriveSeqObj.Remove(iSeqItemObj);
                    }
                    catch (NotSupportedException ex)
                    {
                        actualRemoveMessage = ex.Message;
                    }

                    Assert.AreEqual(expRemoveMessage, actualRemoveMessage);
                    ApplicationLog.WriteLine("Sequence P2: Successfully validated Remove() sequence.");
                    Console.WriteLine("Sequence P2: Successfully validated Remove() sequence.");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Validate Sequence Index with Non Gap Chanracters.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// </summary>
        static void ValidateGeneralIndexOfNonGapChars(string nodeName,
            AdditionalParameters methodName)
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode);
            string sequence1 = Utility._xmlUtil.GetTextValue(nodeName,
               Constants.Sequence1);
            string gapCharSequence = Utility._xmlUtil.GetTextValue(nodeName,
               Constants.GapCharSeqNode);
            string expectedNullSeqMessage = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.NullSeqErrorMessageNode);
            string expectedStartPosError = Utility._xmlUtil.GetTextValue(nodeName,
               Constants.StartPosErrorNode);
            string expectedEndPosError = Utility._xmlUtil.GetTextValue(nodeName,
               Constants.EndPosErrorNode);
            int pos;
            string actaulError = null;

            // Create a sequence.
            Sequence seqWithGapChar = new Sequence(Utility.GetAlphabet(alphabetName), sequence1);
            Sequence gapCharSeq = new Sequence(Utility.GetAlphabet(alphabetName), gapCharSequence);
            switch (methodName)
            {
                case AdditionalParameters.NullSequence:
                    try
                    {
                        pos = BasicSequenceInfo.IndexOfNonGap(null);

                    }
                    catch (ArgumentNullException ex)
                    {
                        actaulError = ex.Message;
                    }

                    // Validate error message
                    Assert.AreEqual(expectedNullSeqMessage,
                        actaulError.Replace("\r", "").Replace("\n", ""));
                    break;
                case AdditionalParameters.NegativeIndex:
                    try
                    {
                        pos = BasicSequenceInfo.IndexOfNonGap(seqWithGapChar, -10);

                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        actaulError = ex.Message;
                    }

                    // Validate error message
                    Assert.AreEqual(expectedStartPosError,
                        actaulError.Replace("\r", "").Replace("\n", ""));
                    break;
                case AdditionalParameters.LastIndexNullSequence:
                    try
                    {
                        pos = BasicSequenceInfo.LastIndexOfNonGap(null);

                    }
                    catch (ArgumentNullException ex)
                    {
                        actaulError = ex.Message;
                    }

                    // Validate error message
                    Assert.AreEqual(expectedNullSeqMessage,
                        actaulError.Replace("\r", "").Replace("\n", ""));
                    break;
                case AdditionalParameters.NegativeLastIndex:
                    try
                    {
                        pos = BasicSequenceInfo.LastIndexOfNonGap(seqWithGapChar, -10);

                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        actaulError = ex.Message;
                    }

                    // Validate error message
                    Assert.AreEqual(expectedEndPosError,
                        actaulError.Replace("\r", "").Replace("\n", ""));
                    break;
                case AdditionalParameters.GapCharSequence:

                    pos = BasicSequenceInfo.IndexOfNonGap(gapCharSeq);

                    // Validate error message
                    Assert.AreEqual(-1, pos);
                    break;
                case AdditionalParameters.LastIndexGapCharSequence:

                    pos = BasicSequenceInfo.LastIndexOfNonGap(gapCharSeq);

                    // Validate error message
                    Assert.AreEqual(-1, pos);
                    break;
                default:
                    break;
            }
            ApplicationLog.WriteLine("Sequence P2: Successfully validated BasicSequenceInformation");
        }

        /// <summary>
        /// Invalidate sequence operations.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// </summary>
        static void InvalidateSequenceOperations(AdditionalParameters param)
        {
            // Get a expected values from the Xml
            string alphabetName = Utility._xmlUtil.GetTextValue(Constants.BasicSeqInfoNode,
                Constants.AlphabetNameNode);
            string sequenceString = Utility._xmlUtil.GetTextValue(Constants.BasicSeqInfoNode,
               Constants.Sequence1);

            // Create a sequence.
            Sequence seq = new Sequence(Utility.GetAlphabet(alphabetName), sequenceString);
            seq.IsReadOnly = false;
            switch (param)
            {
                case AdditionalParameters.Range:
                    // Inalidate sequence Range with invalid start index.
                    try
                    {
                        seq.Range(-10, 2);
                        Assert.Fail();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }

                    // Invalidate Range operation with invalid sequence length.
                    try
                    {
                        seq.Range(0, -10);
                        Assert.Fail();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }

                    // Invalidate sequence range with invalid start(count-start)<length index.
                    try
                    {
                        seq.Range(3, 9);
                        Assert.Fail();
                    }
                    catch (ArgumentException)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }
                    break;
                case AdditionalParameters.RemoveRange:
                    // Invalidate sequence remove range with invalid start index.
                    try
                    {
                        seq.RemoveRange(-10, 2);
                        Assert.Fail();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }

                    // Invalidate Range operation with invalid sequence length.
                    try
                    {
                        seq.RemoveRange(0, -10);
                        Assert.Fail();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }

                    // Invalidate sequence remove range with invalid start index.
                    try
                    {
                        seq.RemoveRange(3, 9);
                        Assert.Fail();
                    }
                    catch (ArgumentException)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }
                    break;
                case AdditionalParameters.Replace:
                    // Invalidate sequence Replace operation with invalid sequence length.
                    try
                    {
                        seq.Replace(-2, 'A');
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }

                    // Invalidate readonly sequence.
                    try
                    {
                        seq.IsReadOnly = true;
                        seq.Replace(2, 'A');
                        Assert.Fail();
                    }
                    catch (InvalidOperationException)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }
                    break;
                case AdditionalParameters.ReplaceSeqItem:
                    // Invalidate Replace sequence item operation with invalid
                    // sequence length.
                    try
                    {
                        seq.Replace(-2, seq[0]);
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }

                    // Invalidate ReadOnly sequence with Replace method.
                    try
                    {
                        seq.IsReadOnly = true;
                        seq.Replace(2, seq[0]);
                        Assert.Fail();
                    }
                    catch (InvalidOperationException)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }
                    break;
                case AdditionalParameters.ReplaceRange:
                    // Invalidate null sequence
                    try
                    {
                        seq.ReplaceRange(2, null);
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }

                    // Invalidate sequence replace range with invalid start index.
                    try
                    {
                        seq.ReplaceRange(13, "AG");
                        Assert.Fail();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }

                    // Invalidate ReplaceRange with ReadOnly sequence.
                    try
                    {
                        seq.IsReadOnly = true;
                        seq.ReplaceRange(2, "A");
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }
                    break;
                case AdditionalParameters.Insert:
                    // Invalidate null sequence insertion.
                    try
                    {
                        seq.Insert(2, null);
                        Assert.Fail();
                    }
                    catch (ArgumentNullException)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }

                    // Invalidate Sequence insert with ReadOnly operation.
                    try
                    {
                        seq.IsReadOnly = true;
                        seq.Insert(2, 'A');
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }
                    break;
                case AdditionalParameters.InsertRange:
                    // Invalidate null sequence InsertRange operation.
                    try
                    {
                        seq.InsertRange(2, null);
                        Assert.Fail();
                    }
                    catch (ArgumentNullException)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }

                    // Invalidate InsertRange operation with invalid start index.
                    try
                    {
                        seq.InsertRange(-13, "AG");
                        Assert.Fail();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }

                    // Invalidate ReadOnly sequence.
                    try
                    {
                        seq.IsReadOnly = true;
                        seq.InsertRange(2, "A");
                        Assert.Fail();
                    }
                    catch (Exception)
                    {
                        ApplicationLog.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                        Console.WriteLine(
                            "Sequence P2 : Successfully validated the exception");
                    }
                    break;
                default:
                    break;
            }

        }

        #endregion Supported Methods
    }
}
