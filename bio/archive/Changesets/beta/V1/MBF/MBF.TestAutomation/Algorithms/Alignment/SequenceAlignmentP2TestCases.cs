// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * SequenceAlignmentP2TestCases.cs
 * 
 *   This file contains the SequenceAlignmentAlignment P2 test cases 
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MBF.Util.Logging;
using MBF.TestAutomation.Util;
using NUnit.Framework;
using MBF.Algorithms.Alignment;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization;

namespace MBF.TestAutomation.Algorithms.Alignment
{
    /// <summary>
    /// Sequence Alignment algorithm P2 test cases
    /// </summary>
    [TestFixture]
    public class SequenceAlignmentP2TestCases
    {

        #region Enums
        /// <summary>
        /// Input sequences to get aligned in different cases.
        /// </summary>
        enum SequenceCaseType
        {
            LowerCase,
            UpperCase,
            Default
        }

        /// <summary>
        /// Types of invalid sequence
        /// </summary>
        enum SequenceType
        {
            SequenceWithSpecialChars,
            EmptySequence,
            SequenceWithSpaces,
            SequenceWithGap,
            Default
        }

        /// <summary>
        /// Types of offset value to be passed as parameter.
        /// </summary>
        enum OffsetValidation
        {
            positiveOffset,
            negativeOffset,
            Default
        }

        /// <summary>
        /// SequenceAlignment methods name which are used for different cases.
        /// </summary>
        enum SeqAlignmentMethods
        {
            Add,
            Clear,
            Remove,
            AddSequence,
            GetObjectData,
            Default
        };

        #endregion

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static SequenceAlignmentP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion

        #region Test Cases

        /// <summary>
        /// Pass a valid sequences(Lower Case) to AddSequence() method and 
        /// validate with expected sequences.
        /// Input : Sequence read from xml file.
        /// Validation : Added sequences are retrieved and validated.
        /// </summary>
        [Test]
        public void ValidateSequenceAlignmentAddSequenceWithLowerCase()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignAlgorithmNodeName,
                OffsetValidation.Default, SequenceCaseType.LowerCase, SequenceType.Default);
        }

        /// <summary>
        /// Pass a valid sequences(Upper Case) to AddSequence() method and 
        /// validate with expected sequences.
        /// Input : Sequence read from xml file.
        /// Validation : Added sequences are retrieved and validated.
        /// </summary>
        [Test]
        public void ValidateSequenceAlignmentAddSequenceWithUpperCase()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignAlgorithmNodeName,
                OffsetValidation.Default, SequenceCaseType.UpperCase, SequenceType.Default);
        }

        /// <summary>
        /// Pass a sequences with gaps to AddSequence() method and 
        /// validate that exception is thrown
        /// Input : Sequence read from xml file.
        /// Validation : Expected exception is thrown
        /// </summary>
        [Test]
        public void InValidateSequenceAlignmentWithGapSequences()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignAlgorithmNodeName,
                OffsetValidation.Default, SequenceCaseType.Default, SequenceType.SequenceWithGap);
        }

        /// <summary>
        /// Pass a invalid sequence to AddSequence() method and validate that exception is thrown
        /// Input : Sequence read from xml file.
        /// Validation : Expected exception is thrown
        /// </summary>
        [Test]
        public void InValidateSequenceAlignmentWithInvalidSequence()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignAlgorithmNodeName,
                OffsetValidation.Default, SequenceCaseType.Default, SequenceType.SequenceWithSpecialChars);
        }

        /// <summary>
        /// Pass a invalid sequence to AddSequence() method and validate that exception is thrown
        /// Input : Sequence read from xml file.
        /// Validation : Expected exception is thrown
        /// </summary>
        [Test]
        public void InValidateSequenceAlignmentWithEmptySequence()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignAlgorithmNodeName,
                OffsetValidation.Default, SequenceCaseType.Default, SequenceType.EmptySequence);
        }

        /// <summary>
        /// Pass a sequence with spaces to AddSequence() method and validate that exception is thrown
        /// Input : Sequence read from xml file.
        /// Validation : Expected exception is thrown
        /// </summary>
        [Test]
        public void InValidateSequenceAlignmentWithSpacesSequence()
        {
            ValidateGeneralSequenceAlignment(Constants.AlignAlgorithmNodeName,
                OffsetValidation.Default, SequenceCaseType.Default, SequenceType.SequenceWithSpaces);
        }

        /// <summary>
        /// Validate Add() method exception by setting ReadOnly SequenceAlignment 
        /// Input : Dna Sequence read from xml file.
        /// Validation : Validation of Add() method exception
        /// </summary>
        [Test]
        public void InValidateAddSequenceToSequenceAlignment()
        {
            InValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.Add);
        }

        /// <summary>
        /// Validate Clear() method exception by setting ReadOnly SequenceAlignment 
        /// Input : Dna Sequence read from xml file.
        /// Validation : Validation of Clear() method exception
        /// </summary>
        [Test]
        public void InValidateClearSequenceAlignment()
        {
            InValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.Clear);
        }

        /// <summary>
        /// Validate Remove() method exception by setting ReadOnly SequenceAlignment 
        /// Input : Dna Sequence read from xml file.
        /// Validation : Validation of Remove() method exception
        /// </summary>
        [Test]
        public void InValidateRemoveSequenceAlignment()
        {
            InValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.Remove);
        }

        /// <summary>
        /// Validate AddSequence() method exception by setting ReadOnly SequenceAlignment 
        /// Input : Dna Sequence read from xml file.
        /// Validation : Validation of AddSequence() method exception
        /// </summary>
        [Test]
        public void InValidateAddSequenceAlignment()
        {
            InValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.AddSequence);
        }

        /// <summary>
        /// InValidate GetObjectData() method exception.
        /// Input : Dna Sequence read from xml file.
        /// Validation : Validation of GetObjectData() method exception
        /// </summary>
        [Test]
        public void InValidateGetObjectData()
        {
            InValidateSequenceAlignmentGeneralMethods(Constants.AlignAlgorithmNodeName,
                SeqAlignmentMethods.GetObjectData);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Validates Sequence Alignment test cases for the parameters passed.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="offset">Offset</param>
        /// <param name="caseType">Case type</param>
        /// <param name="type">Sequence type</param>
        static void ValidateGeneralSequenceAlignment(string nodeName,
            OffsetValidation offset, SequenceCaseType caseType, SequenceType type)
        {
            // Read the xml file for getting both the files for aligning.
            string firstInputSequence = string.Empty;
            string secondInputSequence = string.Empty;

            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));
            GetInputSequencesWithSequenceType(nodeName, type, out firstInputSequence,
                out secondInputSequence);

            ApplicationLog.WriteLine(string.Format(null,
                "SequenceAlignment P2 : First sequence used is '{0}'.", firstInputSequence));
            ApplicationLog.WriteLine(string.Format(null,
                "SequenceAlignment P2 : Second sequence used is '{0}'.", secondInputSequence));

            Console.WriteLine(string.Format(null,
                "SequenceAlignment P2 : First sequence used is '{0}'.", firstInputSequence));
            Console.WriteLine(string.Format(null,
                "SequenceAlignment P2 : Second sequence used is '{0}'.", secondInputSequence));

            // Create two sequences
            ISequence aInput = null;
            ISequence bInput = null;
            Exception actualException = null;

            switch (caseType)
            {
                case SequenceCaseType.LowerCase:
                    aInput = new Sequence(alphabet, firstInputSequence.ToLower(CultureInfo.CurrentCulture));
                    bInput = new Sequence(alphabet, secondInputSequence.ToLower(CultureInfo.CurrentCulture));

                    break;
                case SequenceCaseType.UpperCase:
                    aInput = new Sequence(alphabet, firstInputSequence.ToUpper(CultureInfo.CurrentCulture));
                    bInput = new Sequence(alphabet, secondInputSequence.ToUpper(CultureInfo.CurrentCulture));
                    break;
                case SequenceCaseType.Default:
                    try
                    {
                        aInput = new Sequence(alphabet, firstInputSequence);
                        bInput = new Sequence(alphabet, secondInputSequence);
                    }
                    catch (Exception ex)
                    {
                        actualException = ex;
                    }
                    break;
            }

            // Add the sequences to the Sequence alignment object using AddSequence() method.
            IList<IPairwiseSequenceAlignment> sequenceAlignmentObj = new List<IPairwiseSequenceAlignment>();

            try
            {
                PairwiseAlignedSequence alignSeq = new PairwiseAlignedSequence();
                alignSeq.FirstSequence = aInput;
                IPairwiseSequenceAlignment seqAlignObj = new PairwiseSequenceAlignment();
                seqAlignObj.Add(alignSeq);
                sequenceAlignmentObj.Add(seqAlignObj);
            }
            catch (Exception ex)
            {
                actualException = ex;
            }

            if (actualException == null)
            {
                if (offset == OffsetValidation.Default)
                {
                    sequenceAlignmentObj[0].PairwiseAlignedSequences[0].SecondSequence = bInput;

                    // Read the output back and validate the same.
                    IList<PairwiseAlignedSequence> newAlignedSequences = sequenceAlignmentObj[0].PairwiseAlignedSequences;

                    ApplicationLog.WriteLine(string.Format(null,
                        "SequenceAlignment P2 : First sequence read is '{0}'.", firstInputSequence));
                    ApplicationLog.WriteLine(string.Format(null,
                        "SequenceAlignment P2 : Second sequence read is '{0}'.", secondInputSequence));

                    Console.WriteLine(string.Format(null,
                        "SequenceAlignment P2 : First sequence read is '{0}'.", firstInputSequence));
                    Console.WriteLine(string.Format(null,
                        "SequenceAlignment P2 : Second sequence read is '{0}'.", secondInputSequence));

                    Assert.AreEqual(newAlignedSequences[0].FirstSequence.ToString(), firstInputSequence);
                    Assert.AreEqual(newAlignedSequences[0].SecondSequence.ToString(), secondInputSequence);
                }
            }
            else
            {
                // Validate that expected exception is thrown using error message.
                string expectedErrorMessage = Utility._xmlUtil.GetTextValue(nodeName,
                    Constants.ExpectedErrorMessage);
                Assert.AreEqual(expectedErrorMessage, actualException.Message);

                ApplicationLog.WriteLine(string.Format(null,
                    "SequenceAlignment P2 : Expected Error message is thrown ", expectedErrorMessage));

                Console.WriteLine(string.Format(null,
                    "SequenceAlignment P2 : Expected Error message is thrown ", expectedErrorMessage));
            }
        }

        /// <summary>
        /// Get Input sequences to add in the sequence alignment object.
        /// </summary>
        /// <param name="nodeName">xml align algo root name</param>
        /// <param name="sequenceType">invalid/valid sequence type</param>
        /// <param name="firstInputSequence">returns first input sequence.</param>
        /// <param name="secondInputSequence">returns second input sequence.</param>
        static void GetInputSequencesWithSequenceType(string nodeName,
            SequenceType sequenceType, out string firstInputSequence, out string secondInputSequence)
        {
            firstInputSequence = null;
            secondInputSequence = null;

            switch (sequenceType)
            {
                case SequenceType.SequenceWithSpecialChars:
                    firstInputSequence = Utility._xmlUtil.GetTextValue(nodeName, Constants.InvalidSequence1);
                    secondInputSequence = firstInputSequence;
                    break;
                case SequenceType.EmptySequence:
                    firstInputSequence = string.Empty;
                    secondInputSequence = firstInputSequence;
                    break;
                case SequenceType.SequenceWithSpaces:
                    firstInputSequence = Utility._xmlUtil.GetTextValue(nodeName, Constants.SpacesSequence);
                    secondInputSequence = firstInputSequence;
                    break;
                case SequenceType.SequenceWithGap:
                    firstInputSequence = Utility._xmlUtil.GetTextValue(nodeName, Constants.GapSequenceNode);
                    secondInputSequence = firstInputSequence;
                    break;
                case SequenceType.Default:
                    firstInputSequence = Utility._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode1);
                    secondInputSequence = Utility._xmlUtil.GetTextValue(nodeName, Constants.SequenceNode2);
                    break;
            }
        }

        /// <summary>
        /// Validate Sequence Alignment Class General methods exception.
        /// </summary>
        /// <param name="nodeName">Node Name in the xml.</param>
        /// <param name="methodName">Name of the SequenceAlignment method to be validated</param>
        /// </summary>
        static void InValidateSequenceAlignmentGeneralMethods(string nodeName,
            SeqAlignmentMethods methodName)
        {
            // Read the xml file for getting both the files for aligning.
            string origSequence1 = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SequenceNode1);
            string origSequence2 = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.SequenceNode2);
            IAlphabet alphabet = Utility.GetAlphabet(Utility._xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode));
            string readOnlyException = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.ReadOnlyExceptionNode);
            string expectedGetObjectDataException = Utility._xmlUtil.GetTextValue(nodeName,
                Constants.GetObjectDataNullErrorMessageNode);
            string actualError = null;
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            // Create two sequences
            ISequence aInput = new Sequence(alphabet, origSequence1);
            ISequence bInput = new Sequence(alphabet, origSequence2);

            // Add the sequences to the Sequence alignment object using AddSequence() method.
            IList<IPairwiseSequenceAlignment> sequenceAlignmentObj =
                new List<IPairwiseSequenceAlignment>();

            PairwiseAlignedSequence alignSeq = new PairwiseAlignedSequence();
            alignSeq.FirstSequence = aInput;
            alignSeq.SecondSequence = bInput;
            PairwiseSequenceAlignment seqAlignObj = new PairwiseSequenceAlignment(aInput, bInput); ;

            seqAlignObj.Add(alignSeq);
            sequenceAlignmentObj.Add(seqAlignObj);

            // Set SequenceAlignment IsReadOnly prpoerty to true.
            seqAlignObj.IsReadOnly = true;
            IList<PairwiseAlignedSequence> newAlignedSequences =
                sequenceAlignmentObj[0].PairwiseAlignedSequences;

            switch (methodName)
            {
                case SeqAlignmentMethods.Add:
                    try
                    {
                        seqAlignObj.Add(newAlignedSequences[0]);
                    }
                    catch (NotSupportedException ex)
                    {
                        actualError = ex.Message;
                    }
                    // Validate Error message
                    Assert.AreEqual(readOnlyException, actualError);
                    break;
                case SeqAlignmentMethods.Clear:
                    try
                    {
                        seqAlignObj.Clear();
                    }
                    catch (NotSupportedException ex)
                    {
                        actualError = ex.Message;
                    }
                    // Validate Error message
                    Assert.AreEqual(readOnlyException, actualError);
                    break;
                case SeqAlignmentMethods.Remove:
                    try
                    {
                        seqAlignObj.Remove(newAlignedSequences[0]);
                    }
                    catch (NotSupportedException ex)
                    {
                        actualError = ex.Message;
                    }

                    // Validate Error message
                    Assert.AreEqual(readOnlyException, actualError);
                    break;
                case SeqAlignmentMethods.AddSequence:
                    try
                    {
                        seqAlignObj.AddSequence(newAlignedSequences[0]);
                    }
                    catch (NotSupportedException ex)
                    {
                        actualError = ex.Message;
                    }
                    // Validate Error message
                    Assert.AreEqual(readOnlyException, actualError);
                    break;
                case SeqAlignmentMethods.GetObjectData:
                    try
                    {
                        seqAlignObj.GetObjectData(null, context);
                    }
                    catch (ArgumentNullException ex)
                    {
                        actualError = ex.Message;
                    }
                    // Validate Error message
                    Assert.AreEqual(expectedGetObjectDataException,
                        actualError.Replace("\r", "").Replace("\n", ""));
                    break;
                default:
                    break;
            }

            ApplicationLog.WriteLine("SequenceAlignment P2 : Successfully validated the IsRead Property");
            Console.WriteLine("SequenceAlignment P2 : Successfully validated the IsRead Property");

        }

        #endregion
    }
}