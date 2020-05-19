// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * VirtualSequenceBVTTestCases.cs
 * 
 * This file contains the Virtual Sequence BVT test case validation.
 * 
******************************************************************************/

using System;
using System.Runtime.Serialization;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Test Automation code for MBF Virtual Sequences and BVT level validations.
    /// </summary>
    [TestFixture]
    public class VirtualSequenceBvtTestCases
    {

        #region Enums

        /// <summary>
        /// Virtual seqeunce alphabets Parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum AlphabetNameParameter
        {
            DNA,
            RNA,
            PROTEIN,
            Default
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static VirtualSequenceBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil = new XmlUtility(@"TestUtils\TestsConfig.xml");
        }

        #endregion Constructor

        #region VirtualSequence BVT TestCases

        /// <summary>
        /// Validate creation of virtual sequence for DNA alphabet.
        /// Input Data : Valid Dna Alphabet
        /// Output Data : validation of virtual seqeunce.
        /// </summary>
        [Test]
        public void ValidateDnaVirtualSequence()
        {
            ValidateGeneralVirtualSequence(Constants.DnaVirtualSeqNode, AlphabetNameParameter.DNA);
        }

        /// <summary>
        /// Validate creation of virtual sequence for RNA alphabet.
        /// Input Data : Valid Rna Alphabet
        /// Output Data : validation of virtual seqeunce.
        /// </summary>
        [Test]
        public void ValidateRnaVirtualSequence()
        {
            ValidateGeneralVirtualSequence(Constants.RnaVirtualSeqNode, AlphabetNameParameter.RNA);
        }

        /// <summary>
        /// Validate creation of virtual sequence for PROTEIN alphabet.
        /// Input Data : Valid Protein Alphabet
        /// Output Data : validation of virtual seqeunce.
        /// </summary>
        [Test]
        public void ValidateProteinVirtualSequence()
        {
            ValidateGeneralVirtualSequence(Constants.ProteinVirtualSeqNode, AlphabetNameParameter.PROTEIN);
        }

        /// <summary>
        /// Validate GetObjectData() of VirtualSequence by passing valid
        /// SerializationInfo and StreamingContext
        /// Input Data : Valid SerializationInfo and StreamingContext.
        /// Output Data : Validate GetObjectData with ICloneable.
        /// </summary>
        [Test]
        public void ValidateVirtualSequenceGetObjectDataDocument()
        {
            ISequence virSeq =
                new VirtualSequence(Alphabets.DNA);
            virSeq.Documentation = Constants.Documentation;

            SerializationInfo info =
                new SerializationInfo(typeof(VirtualSequence),
                    new FormatterConverter());
            StreamingContext context =
                new StreamingContext(StreamingContextStates.All);

            // Serialize the target object
            virSeq.GetObjectData(info, context);

            Assert.IsNotNull(info.GetValue(
                string.Format("{0}:{1}",
                Constants.VirtualSequence, Constants.Documentation),
                typeof(object)));
            Assert.IsNotNull(info.GetValue(
                string.Format("{0}:{1}",
                Constants.VirtualSequence, Constants.SeqInfo),
                typeof(BasicSequenceInfo)));
            Assert.IsNotNull(info.GetValue(
                string.Format("{0}:{1}",
                Constants.VirtualSequence, Constants.MoleculeType),
                typeof(MoleculeType)));
            Assert.AreEqual
                (Constants.Documentation,
                info.GetValue(
                string.Format("{0}:{1}",
                Constants.VirtualSequence, Constants.Documentation),
                typeof(object)).ToString());
            Assert.AreEqual(
                Constants.MBFBasicSequenceInfo,
                info.GetValue(
                string.Format("{0}:{1}",
                Constants.VirtualSequence, Constants.SeqInfo),
                typeof(BasicSequenceInfo)).ToString());
            Assert.AreEqual(
                MoleculeType.Invalid,
                info.GetValue(
                string.Format("{0}:{1}",
                Constants.VirtualSequence, Constants.MoleculeType),
                typeof(MoleculeType)));

            ICloneable iClone = virSeq;
            object clone = null;

            // Create a copy of Virtual Sequence
            clone = iClone.Clone();

            Assert.IsNotNull(clone);
            Assert.AreEqual(
                ((MBF.VirtualSequence)(clone)).Alphabet, Alphabets.DNA);
            Assert.AreEqual(
                ((MBF.VirtualSequence)(clone)).Documentation,
                Constants.Documentation);
            Assert.AreEqual(
                ((MBF.VirtualSequence)(clone)).IsReadOnly, true);
            Assert.AreEqual(
                ((MBF.VirtualSequence)(clone)).MoleculeType,
                MoleculeType.Invalid);
            Assert.AreEqual(
                ((MBF.VirtualSequence)(clone)).UseEncoding, false);
        }

        /// <summary>
        /// Validate GetObjectData() of VirtualSequence by passing valid
        /// SerializationInfo and StreamingContext and null documentation.
        /// Input Data : Valid SerializationInfo and StreamingContext.
        /// Output Data : Validate GetObjectData with Isequence clone.
        /// </summary>
        [Test]
        public void ValidateVirtualSequenceGetObjectData()
        {
            ISequence virSeq =
                new VirtualSequence(Alphabets.DNA);

            SerializationInfo info =
                new SerializationInfo(typeof(Sequence),
                    new FormatterConverter());
            StreamingContext context =
                new StreamingContext(StreamingContextStates.All);

            // Serialize the target object
            virSeq.GetObjectData(info, context);

            Assert.IsNull(info.GetValue(
                string.Format("{0}:{1}",
                Constants.VirtualSequence, Constants.Documentation),
                typeof(object)));
            Assert.IsNotNull(info.GetValue(
                string.Format("{0}:{1}",
                Constants.VirtualSequence, Constants.SeqInfo),
                typeof(BasicSequenceInfo)));
            Assert.IsNotNull(info.GetValue(
                string.Format("{0}:{1}",
                Constants.VirtualSequence, Constants.MoleculeType),
                typeof(MoleculeType)));
            Assert.AreEqual(null, info.GetValue(
                string.Format("{0}:{1}",
                Constants.VirtualSequence, Constants.Documentation),
                typeof(object)));
            Assert.AreEqual(
                Constants.MBFBasicSequenceInfo,
                info.GetValue(
                string.Format("{0}:{1}",
                Constants.VirtualSequence, Constants.SeqInfo),
                typeof(BasicSequenceInfo)).ToString());
            Assert.AreEqual(MoleculeType.Invalid,
                info.GetValue(
                string.Format("{0}:{1}",
                Constants.VirtualSequence, Constants.MoleculeType),
                typeof(MoleculeType)));

            // Create a copy of Virtual Sequence
            object seqClone = virSeq.Clone();
            Assert.IsNotNull(seqClone);
            Assert.IsNotNull(seqClone);
            Assert.AreEqual(
                ((MBF.VirtualSequence)(seqClone)).Alphabet, Alphabets.DNA);
            Assert.AreEqual(
                ((MBF.VirtualSequence)(seqClone)).Documentation, null);
            Assert.AreEqual(
                ((MBF.VirtualSequence)(seqClone)).IsReadOnly, true);
            Assert.AreEqual(
                ((MBF.VirtualSequence)(seqClone)).MoleculeType, MoleculeType.Invalid);
            Assert.AreEqual(
                ((MBF.VirtualSequence)(seqClone)).UseEncoding, false);
        }

        #endregion VirtualSequence BVT TestCases

        #region Supporting method

        /// <summary>
        /// General method to validate creation of virtual seqeunce.
        /// <param name="nodeName">xml node name.</param>
        /// <param name="alphabetName">alphabet name.</param>
        /// </summary>
        static void ValidateGeneralVirtualSequence(
            string nodeName, AlphabetNameParameter alphabetName)
        {
            // Gets the alphabet from the Xml
            string alphabet = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string expectedSeqCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedVSeqCount);
            string id = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Id);
            string diplayId = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DisplayId);
            string expectedValue = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IndexValue);
            string expectedDocumentaion = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Documentaion);

            // Logs information to the log file
            ApplicationLog.WriteLine(string.Format(null,
                "Virtual Sequence BVT: Sequence {0} is expected.", alphabet));

            // create virtual seqeunce for an alphabet.
            VirtualSequence virtualSeq = new VirtualSequence(
                Utility.GetAlphabet(alphabet));

            // Set value to virtual sequences.
            virtualSeq.ID = id;
            virtualSeq.DisplayID = diplayId;
            virtualSeq.Documentation = expectedDocumentaion;

            // Validate the created virtual Sequence
            Assert.AreEqual(virtualSeq.DisplayID, diplayId);
            Assert.AreEqual(virtualSeq.ID, id);
            Assert.AreEqual(virtualSeq.IsReadOnly, true);
            Assert.AreEqual(virtualSeq.Count.ToString(
                (IFormatProvider)null), expectedSeqCount);
            Assert.AreEqual(virtualSeq.Statistics, null);

            switch (alphabetName)
            {
                case AlphabetNameParameter.DNA:
                    virtualSeq.MoleculeType = MoleculeType.DNA;
                    foreach (Nucleotide nucleo in Alphabets.DNA)
                    {
                        Assert.AreEqual(virtualSeq.IndexOf(nucleo).ToString(
                            (IFormatProvider)null), expectedValue);
                    }
                    Assert.AreEqual(virtualSeq.MoleculeType, MoleculeType.DNA);
                    break;
                case AlphabetNameParameter.RNA:
                    virtualSeq.MoleculeType = MoleculeType.RNA;
                    foreach (Nucleotide nucleo in Alphabets.RNA)
                    {
                        Assert.AreEqual(virtualSeq.IndexOf(nucleo).ToString(
                            (IFormatProvider)null), expectedValue);
                    }
                    Assert.AreEqual(virtualSeq.MoleculeType, MoleculeType.RNA);
                    break;
                case AlphabetNameParameter.PROTEIN:
                    virtualSeq.MoleculeType = MoleculeType.Protein;
                    foreach (AminoAcid aminoacid in Alphabets.Protein)
                    {
                        Assert.AreEqual(virtualSeq.IndexOf(aminoacid).ToString(
                            (IFormatProvider)null), expectedValue);
                    }
                    Assert.AreEqual(virtualSeq.MoleculeType, MoleculeType.Protein);
                    break;
                default:
                    break;
            }

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

        #endregion Supporting method
    }
}