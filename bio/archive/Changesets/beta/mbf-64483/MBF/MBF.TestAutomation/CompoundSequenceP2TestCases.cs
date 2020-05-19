// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * CompoundSequenceP2TestCases.cs
 * 
 * This file contains the Compound Sequence P2 test case validation.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation
{
    /// <summary>
    /// P2 test cases to confirm the features of Compound Nucleotide and Compound Amino Acid
    /// </summary>
    [TestClass]
    public class CompoundSequenceP2TestCases
    {

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static CompoundSequenceP2TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion

        #region Test Cases

        #region Nucleotide Test cases

        /// <summary>
        /// Validate Compound sequence with invalid symbol for Nucleotide.
        /// Input data : Nucleotide invalid Symbol.
        /// Output Data : Validate the exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideInValidSymbol()
        {
            try
            {
                ICompoundSequenceItem seqItemObj = new CompoundNucleotide('Ñ', "CompoundSeq");
                ISequence seq = new Sequence(Alphabets.DNA);
                seq.Add(seqItemObj);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the invalid sequence Symbol");
                Console.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the invalid sequence Symbol");
            }
        }

        /// <summary>
        /// Validate Compound sequence with non ambiguous for Nucleotide.
        /// Input data : Nucleotide symbol with name.
        /// Output Data : Validate the property.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideNonAmbiguous()
        {
            ICompoundSequenceItem seqItemObj =
                new CompoundNucleotide('X', "CompoundSeq", false, false);
            Assert.IsFalse(seqItemObj.IsAmbiguous);

            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the Ambiguous property");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the Ambiguous property");
        }

        /// <summary>
        /// Validate Compound sequence with non gap for Nucleotide.
        /// Input data : Nucleotide symbol with name.
        /// Output Data : Validate the property.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideNonGap()
        {
            ICompoundSequenceItem seqItemObj =
                new CompoundNucleotide('X', "CompoundSeq", false, false);
            Assert.IsFalse(seqItemObj.IsGap);

            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the Gap property");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the Gap property");
        }

        /// <summary>
        /// Validate Compound sequence with negative weight Nucleotide.
        /// Input data : Nucleotide Character with negative weight.
        /// Output Data : Validate the sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideNegWeight()
        {
            ICompoundSequenceItem seqItem = new CompoundNucleotide('B', "NegativeWeight");
            seqItem.Add(new Sequence(Alphabets.DNA, "AGCT")[0], -35);
            Assert.AreEqual("Adenine", seqItem.SequenceItems[0].Name);
            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the negative weight expression");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the negative weight expression");
        }

        /// <summary>
        /// Validate Compound sequence with invalid sequence item for Nucleotide.
        /// Input data : Nucleotide invalid sequence item.
        /// Output Data : Validate the exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideInValidSeqItem()
        {
            ISequenceItem seqItem1 = new Sequence(Alphabets.DNA, "AGCT")[0];
            ISequenceItem seqItem2 = new Sequence(Alphabets.Protein, "KIE")[0];
            List<ISequenceItem> seqItemList = new List<ISequenceItem>();
            seqItemList.Add(seqItem1);
            seqItemList.Add(seqItem2);
            double weightItem1 = 35;
            double weightItem2 = 33;
            List<double> weightList = new List<double>();
            weightList.Add(weightItem1);
            weightList.Add(weightItem2);

            try
            {
                ICompoundSequenceItem seqItemObj =
                    new CompoundNucleotide('B', "CompoundSeq", seqItemList, weightList);
                Assert.IsTrue(null != seqItemObj);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the invalid sequence list");
                Console.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the invalid sequence list");
            }
        }

        /// <summary>
        /// Validate Compound sequence and remove invalid sequence item for Nucleotide.
        /// Input data : Nucleotide valid sequence item.
        /// Output Data : Validate there is no exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideInValidRemove()
        {
            List<ISequenceItem> seqItemList = null;
            List<double> weightList = null;
            GetSequenceItemWithWeight(out seqItemList, out weightList, true);

            ICompoundSequenceItem seqItem =
                new CompoundNucleotide('B', "CompoundSeq", seqItemList, weightList);
            seqItem.Remove(new Sequence(Alphabets.DNA, "T")[0]);
            Assert.AreEqual(2, seqItem.SequenceItems.Count);

            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the invalid remove");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the invalid remove");
        }

        /// <summary>
        /// Validate Compound sequence and duplicate sequence items for Nucleotide.
        /// Input data : Nucleotide valid sequence items.
        /// Output Data : Validate the sequence items are matching.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideDuplicateSequenceItem()
        {
            List<ISequenceItem> seqItemList = null;
            List<double> weightList = null;
            GetSequenceItemWithWeight(out seqItemList, out weightList, true);

            ICompoundSequenceItem seqItemObj1 =
                new CompoundNucleotide('B', "CompoundSeq", seqItemList, weightList);
            ICompoundSequenceItem seqItemObj2 =
                new CompoundNucleotide('C', "CompoundSeq", seqItemList, weightList);

            Sequence originalSeq = new Sequence(Alphabets.DNA);
            originalSeq.Add(seqItemObj1);
            originalSeq.Add(seqItemObj2);

            Assert.AreEqual("BC", originalSeq.ToString());

            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the duplicate item");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the duplicate item");
        }

        /// <summary>
        /// Validate Compound sequence and duplicate sequence items for Nucleotide & Symbol.
        /// Input data : Nucleotide valid sequence items & Symbol.
        /// Output Data : Validate the sequence items are matching.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideDuplicateSequenceItemSymbol()
        {
            List<ISequenceItem> seqItemList = null;
            List<double> weightList = null;
            GetSequenceItemWithWeight(out seqItemList, out weightList, true);

            ICompoundSequenceItem seqItemObj1 =
                new CompoundNucleotide('B', "CompoundSeq", seqItemList, weightList);
            ICompoundSequenceItem seqItemObj2 =
                new CompoundNucleotide('B', "CompoundSeq", seqItemList, weightList);

            Sequence originalSeq = new Sequence(Alphabets.DNA);
            originalSeq.Add(seqItemObj1);
            originalSeq.Add(seqItemObj2);

            Assert.AreEqual("BB", originalSeq.ToString());

            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the duplicate item & symbol");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the duplicate item & symbol");
        }

        /// <summary>
        /// Validate Compound sequence and duplicate sequence items for Nucleotide.
        /// Input data : Nucleotide valid sequence items.
        /// Output Data : Validate the sequence items are matching.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideDuplicateSequence()
        {
            List<ISequenceItem> seqItemList = null;
            List<double> weightList = null;
            GetSequenceItemWithWeight(out seqItemList, out weightList, true);

            ICompoundSequenceItem seqItemObj1 =
                new CompoundNucleotide('B', "CompoundSeq", seqItemList, weightList);
            ICompoundSequenceItem seqItemObj2 =
                new CompoundNucleotide('B', "CompoundSeq", seqItemList, weightList);

            Assert.AreEqual(seqItemObj1, seqItemObj2);

            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the duplicate item");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the duplicate item");
        }

        /// <summary>
        /// Validate Compound sequence and sequence items for Nucleotide with weight as null.
        /// Input data : Nucleotide valid sequence items.
        /// Output Data : Validate the exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideWeightListNull()
        {
            List<ISequenceItem> seqItemList = null;
            List<double> weightList = null;
            GetSequenceItemWithWeight(out seqItemList, out weightList, true);

            try
            {
                ICompoundSequenceItem seqItemObj =
                    new CompoundNucleotide('B', "CompoundSeq", seqItemList, null);
                Assert.IsTrue(null != seqItemObj);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the weight null exception");
                Console.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the weight null exception");
            }
        }

        /// <summary>
        /// Validate Compound sequence and sequence items for Nucleotide null.
        /// Input data : Nucleotide valid sequence items.
        /// Output Data : Validate the exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideSequenceListNull()
        {
            List<ISequenceItem> seqItemList = null;
            List<double> weightList = null;
            GetSequenceItemWithWeight(out seqItemList, out weightList, true);

            try
            {
                ICompoundSequenceItem seqItemObj =
                    new CompoundNucleotide('B', "CompoundSeq", null, weightList);
                Assert.IsTrue(null != seqItemObj);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the sequence list null exception");
                Console.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the sequence list null exception");
            }
        }

        /// <summary>
        /// Validate Compound sequence and duplicate sequence items for Nucleotide.
        /// Input data : Nucleotide valid sequence items.
        /// Output Data : Validate the sequence items are matching.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideAminoAcid()
        {
            ISequenceItem seqItem1 = new Sequence(Alphabets.DNA, "AGCT")[0];
            ISequenceItem seqItem2 = new Sequence(Alphabets.Protein, "KEI")[0];
            List<ISequenceItem> seqItemList = new List<ISequenceItem>();
            seqItemList.Add(seqItem1);
            seqItemList.Add(seqItem2);
            double weightItem1 = 35;
            double weightItem2 = 33;
            List<double> weightList = new List<double>();
            weightList.Add(weightItem1);
            weightList.Add(weightItem2);

            try
            {
                ICompoundSequenceItem seqItemObj =
                    new CompoundNucleotide('B', "CompoundSeq'", seqItemList, weightList);
                Assert.IsTrue(null != seqItemObj);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the Amino Acid for Nucleotide exception");
                Console.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the Amino Acid for Nucleotide exception");
            }
        }

        #endregion Nucleotide Test cases

        #region Amino Acid Test cases

        /// <summary>
        /// Validate Compound sequence with invalid symbol for Amino Acid.
        /// Input data : Amino Acid invalid Symbol.
        /// Output Data : Validate the exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundAminoAcidInValidSymbol()
        {
            try
            {
                ICompoundSequenceItem seqItemObj = new CompoundAminoAcid('Ñ', "CompoundSeq");
                ISequence seq = new Sequence(Alphabets.Protein);
                seq.Add(seqItemObj);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the invalid sequence Symbol");
                Console.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the invalid sequence Symbol");
            }
        }

        /// <summary>
        /// Validate Compound sequence with non ambiguous for AminoAcid.
        /// Input data : AminoAcid symbol with name.
        /// Output Data : Validate the property.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundAminoAcidNonAmbiguous()
        {
            ICompoundSequenceItem seqItemObj =
                new CompoundAminoAcid('X', "CompoundSeq", false, false);
            Assert.IsFalse(seqItemObj.IsAmbiguous);

            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the Ambiguous property");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the Ambiguous property");
        }

        /// <summary>
        /// Validate Compound sequence with non gap for AminoAcid.
        /// Input data : AminoAcid symbol with name.
        /// Output Data : Validate the property.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundAminoAcidNonGap()
        {
            ICompoundSequenceItem seqItemObj =
                new CompoundAminoAcid('X', "CompoundSeq", false, false);
            Assert.IsFalse(seqItemObj.IsGap);

            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the Gap property");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the Gap property");
        }

        /// <summary>
        /// Validate Compound sequence with negative weight AminoAcid.
        /// Input data : AminoAcid Character with negative weight.
        /// Output Data : Validate the sequence.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundAminoAcidNegWeight()
        {
            ICompoundSequenceItem seqItem =
                new CompoundAminoAcid('B', "NegativeWeight");
            seqItem.Add(new Sequence(Alphabets.Protein, "KIET")[0], -35);
            Assert.AreEqual("Lysine", seqItem.SequenceItems[0].Name);
            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the negative weight expression");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the negative weight expression");
        }

        /// <summary>
        /// Validate Compound sequence with invalid sequence item for AminoAcid.
        /// Input data : AminoAcid invalid sequence item.
        /// Output Data : Validate the exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundAminoAcidInValidSeqItem()
        {
            ISequenceItem seqItem1 = new Sequence(Alphabets.Protein, "KIET")[0];
            ISequenceItem seqItem2 = new Sequence(Alphabets.DNA, "ACG")[0];
            List<ISequenceItem> seqItemList = new List<ISequenceItem>();
            seqItemList.Add(seqItem1);
            seqItemList.Add(seqItem2);
            double weightItem1 = 35;
            double weightItem2 = 33;
            List<double> weightList = new List<double>();
            weightList.Add(weightItem1);
            weightList.Add(weightItem2);

            try
            {
                ICompoundSequenceItem seqItemObj =
                    new CompoundAminoAcid('B', "CompoundSeq", seqItemList, weightList);
                Assert.IsTrue(null != seqItemObj);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the invalid sequence list");
                Console.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the invalid sequence list");
            }
        }

        /// <summary>
        /// Validate Compound sequence and remove invalid sequence item for AminoAcid.
        /// Input data : AminoAcid valid sequence item.
        /// Output Data : Validate there is no exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundAminoAcidInValidRemove()
        {
            List<ISequenceItem> seqItemList = null;
            List<double> weightList = null;
            GetSequenceItemWithWeight(out seqItemList, out weightList, false);

            ICompoundSequenceItem seqItem =
                new CompoundAminoAcid('B', "CompoundSeq", seqItemList, weightList);
            seqItem.Remove(new Sequence(Alphabets.DNA, "T")[0]);
            Assert.AreEqual(2, seqItem.SequenceItems.Count);

            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the invalid remove");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the invalid remove");
        }

        /// <summary>
        /// Validate Compound sequence and duplicate sequence items for AminoAcid.
        /// Input data : AminoAcid valid sequence items.
        /// Output Data : Validate the sequence items are matching.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundAminoAcidDuplicateSequenceItem()
        {
            List<ISequenceItem> seqItemList = null;
            List<double> weightList = null;
            GetSequenceItemWithWeight(out seqItemList, out weightList, false);

            ICompoundSequenceItem seqItemObj1 =
                new CompoundAminoAcid('B', "CompoundSeq", seqItemList, weightList);
            ICompoundSequenceItem seqItemObj2 =
                new CompoundAminoAcid('C', "CompoundSeq", seqItemList, weightList);

            Sequence originalSeq = new Sequence(Alphabets.DNA);
            originalSeq.Add(seqItemObj1);
            originalSeq.Add(seqItemObj2);

            Assert.AreEqual("BC", originalSeq.ToString());

            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the duplicate item");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the duplicate item");
        }

        /// <summary>
        /// Validate Compound sequence and duplicate sequence items for AminoAcid & Symbol.
        /// Input data : AminoAcid valid sequence items & Symbol.
        /// Output Data : Validate the sequence items are matching.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundAminoAcidDuplicateSequenceItemSymbol()
        {
            List<ISequenceItem> seqItemList = null;
            List<double> weightList = null;
            GetSequenceItemWithWeight(out seqItemList, out weightList, false);

            ICompoundSequenceItem seqItemObj1 =
                new CompoundAminoAcid('B', "CompoundSeq", seqItemList, weightList);
            ICompoundSequenceItem seqItemObj2 =
                new CompoundAminoAcid('B', "CompoundSeq", seqItemList, weightList);

            Sequence originalSeq = new Sequence(Alphabets.DNA);
            originalSeq.Add(seqItemObj1);
            originalSeq.Add(seqItemObj2);

            Assert.AreEqual("BB", originalSeq.ToString());

            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the duplicate item & symbol");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the duplicate item & symbol");
        }

        /// <summary>
        /// Validate Compound sequence and duplicate sequence items for AminoAcid.
        /// Input data : AminoAcid valid sequence items.
        /// Output Data : Validate the sequence items are matching.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundAminoAcidDuplicateSequence()
        {
            List<ISequenceItem> seqItemList = null;
            List<double> weightList = null;
            GetSequenceItemWithWeight(out seqItemList, out weightList, false);

            ICompoundSequenceItem seqItemObj1 =
                new CompoundAminoAcid('B', "CompoundSeq", seqItemList, weightList);
            ICompoundSequenceItem seqItemObj2 =
                new CompoundAminoAcid('B', "CompoundSeq", seqItemList, weightList);

            Assert.AreEqual(seqItemObj1, seqItemObj2);

            ApplicationLog.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the duplicate item");
            Console.WriteLine(
                "CompoundSequenceP2TestCases : Successfully validated the duplicate item");
        }

        /// <summary>
        /// Validate Compound sequence and sequence items for AminoAcid with weight as null.
        /// Input data : AminoAcid valid sequence items.
        /// Output Data : Validate the exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundAminoAcidWeightListNull()
        {
            List<ISequenceItem> seqItemList = null;
            List<double> weightList = null;
            GetSequenceItemWithWeight(out seqItemList, out weightList, false);

            try
            {
                ICompoundSequenceItem seqItemObj =
                    new CompoundAminoAcid('B', "CompoundSeq", seqItemList, null);
                Assert.IsTrue(null != seqItemObj);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the weight null exception");
                Console.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the weight null exception");
            }
        }

        /// <summary>
        /// Validate Compound sequence and sequence items for AminoAcid null.
        /// Input data : AminoAcid valid sequence items.
        /// Output Data : Validate the exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundAminoAcidSequenceListNull()
        {
            List<ISequenceItem> seqItemList = null;
            List<double> weightList = null;
            GetSequenceItemWithWeight(out seqItemList, out weightList, false);

            try
            {
                ICompoundSequenceItem seqItemObj =
                    new CompoundAminoAcid('B', "CompoundSeq", null, weightList);
                Assert.IsTrue(null != seqItemObj);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the sequence list null exception");
                Console.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the sequence list null exception");
            }
        }

        /// <summary>
        /// Validate Compound sequence and duplicate sequence items for AminoAcid.
        /// Input data : AminoAcid valid sequence items.
        /// Output Data : Validate the sequence items are matching.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundAminoAcidNucleotide()
        {
            ISequenceItem seqItem1 = new Sequence(Alphabets.Protein, "KIET")[0];
            ISequenceItem seqItem2 = new Sequence(Alphabets.DNA, "AGC")[0];
            List<ISequenceItem> seqItemList = new List<ISequenceItem>();
            seqItemList.Add(seqItem1);
            seqItemList.Add(seqItem2);
            double weightItem1 = 35;
            double weightItem2 = 33;
            List<double> weightList = new List<double>();
            weightList.Add(weightItem1);
            weightList.Add(weightItem2);

            try
            {
                ICompoundSequenceItem seqItemObj =
                    new CompoundAminoAcid('B', "CompoundSeq'", seqItemList, weightList);
                Assert.IsTrue(null != seqItemObj);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine("CompoundSequenceP2TestCases : Successfully validated the Nucleotide for AminoAcid exception");
                Console.WriteLine("CompoundSequenceP2TestCases : Successfully validated the Nucleotide for AminoAcid exception");
            }
        }

        /// <summary>
        /// Validate Compound sequence and duplicate sequence items for AminoAcid.
        /// Input data : AminoAcid valid sequence items.
        /// Output Data : Validate the sequence items are matching.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void ValidateCompoundNucleotideToAminoAcid()
        {
            ISequenceItem seqItem1 = new Sequence(Alphabets.Protein, "KIET")[0];
            ISequenceItem seqItem2 = new Sequence(Alphabets.DNA, "AGC")[0];
            List<ISequenceItem> seqItemList = new List<ISequenceItem>();
            seqItemList.Add(seqItem1);
            seqItemList.Add(seqItem2);
            double weightItem1 = 35;
            double weightItem2 = 33;
            List<double> weightList = new List<double>();
            weightList.Add(weightItem1);
            weightList.Add(weightItem2);

            try
            {
                ICompoundSequenceItem seqItemObj =
                    new CompoundAminoAcid('B', "CompoundSeq'", seqItemList, weightList);
                Assert.IsTrue(null != seqItemObj);
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the Nucleotide for AminoAcid exception");
                Console.WriteLine(
                    "CompoundSequenceP2TestCases : Successfully validated the Nucleotide for AminoAcid exception");
            }
        }

        /// <summary>
        /// Invalidate Amino Acid and Compound Amino Acid Object Data
        /// method for Serialization.
        /// Input : null object.
        /// Output : Validation of Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidateAminoAcidObjectData()
        {
            try
            {
                AminoAcid acidObj = new CompoundAminoAcid('A', "Item");
                acidObj.GetObjectData(null as SerializationInfo, new StreamingContext());

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "AminoAcid : Successfully validated the exception");
                Console.WriteLine(
                    "AminoAcid : Successfully validated the exception");
            }

            try
            {
                CompoundAminoAcid compAcidObj = new CompoundAminoAcid('A', "Item");
                compAcidObj.GetObjectData(null as SerializationInfo, new StreamingContext());

                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                ApplicationLog.WriteLine(
                    "CompoundAminoAcid : Successfully validated the exception");
                Console.WriteLine(
                    "CompoundAminoAcid : Successfully validated the exception");
            }
        }

        /// <summary>
        /// Invalidate Compound Amino Acid Add(Item, Weights) method by passing
        /// invalid ISequenceItem or weight
        /// Input : Invalid value.
        /// Output : Validation of Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidateCompoundAminoAcidAdd()
        {
            CompoundAminoAcid aminoAcid = null;
            try
            {
                aminoAcid = new CompoundAminoAcid('A', "Item");
                aminoAcid.Add(null as ISequenceItem, 35);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundAminoAcid : Successfully validated the exception");
                Console.WriteLine(
                    "CompoundAminoAcid : Successfully validated the exception");
            }

            finally
            {
                if (aminoAcid != null)
                    ((IDisposable)aminoAcid).Dispose();
            }
            try
            {
                List<ISequenceItem> seq = new List<ISequenceItem>();
                seq.Add(new Sequence(Alphabets.Protein, "a")[0]);
                seq.Add(new Sequence(Alphabets.Protein, "a")[0]);

                List<double> weight = new List<double>();
                weight.Add(80);
                weight.Add(78);

                aminoAcid = new CompoundAminoAcid(
                    1, 'B', Constants.CompoundItemName,
                    false, false, seq, weight);

                Assert.Fail();
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundAminoAcid : Successfully validated the exception");
                Console.WriteLine(
                    "CompoundAminoAcid : Successfully validated the exception");
            }
            finally
            {
                if (aminoAcid != null)
                    ((IDisposable)aminoAcid).Dispose();
            }
        }

        /// <summary>
        /// Invalidate Compound Amino Acid Add(listItem, listWeights) method by passing
        /// invalid ISequenceItem or weight
        /// Input : Invalid value.
        /// Output : Validation of Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidateCompoundAminoAcidListAdd()
        {
            CompoundAminoAcid cAminoAcid = null;
            try
            {
                List<ISequenceItem> seq = new List<ISequenceItem>();
                seq.Add(null);
                List<double> weight = new List<double>();

                cAminoAcid = new CompoundAminoAcid(
                     1, 'B', Constants.CompoundItemName,
                     false, false, seq, weight); ;

                Assert.Fail();
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundAminoAcid : Successfully validated the exception");
                Console.WriteLine(
                    "CompoundAminoAcid : Successfully validated the exception");
            }         

            try
            {
                List<ISequenceItem> seq = new List<ISequenceItem>();
                seq.Add(new Sequence(Alphabets.DNA, "a")[0]);

                List<double> weight = new List<double>();

                cAminoAcid =  new CompoundAminoAcid(
                   1, 'B', Constants.CompoundItemName,
                   false, false, seq, weight); ;

                Assert.Fail();
            }
            catch (ArgumentException)
            {
                ApplicationLog.WriteLine(
                    "CompoundAminoAcid : Successfully validated the exception");
                Console.WriteLine(
                    "CompoundAminoAcid : Successfully validated the exception");
            }
            finally
            {
                if (cAminoAcid != null)
                    ((IDisposable)cAminoAcid).Dispose();
            }

        }

        /// <summary>
        /// Invalidate Compound Amino Acid genaral method by passing
        /// invalid ISequenceItem
        /// Input : Invalid value.
        /// Output : Validation of Exception.
        /// </summary>
        [TestMethod]
        [Priority(2)]
        public void InvalidateCompoundAminoAcidGeneral()
        {
            Assert.AreEqual(
                false,
                new CompoundAminoAcid(
                    'A',
                    Constants.CompoundItemName).Remove(null));
            ApplicationLog.WriteLine(
                "CompoundAminoAcid : Successfully validated the Remove method");
            Console.WriteLine(
                "CompoundAminoAcid : Successfully validated the Remove method");

            Assert.AreEqual(
                double.NaN,
                new CompoundAminoAcid(
                    'A',
                    Constants.CompoundItemName).GetWeight(null));
            ApplicationLog.WriteLine(
                "CompoundAminoAcid : Successfully validated the GetWeight method for null item");
            Console.WriteLine(
                "CompoundAminoAcid : Successfully validated the GetWeight method for null item");

            ISequenceItem seq = new Sequence(Alphabets.DNA, "a")[0];
            seq = new Sequence(Alphabets.DNA, "a")[0];
            List<double> weight = new List<double>();
            weight.Add(80);
            weight.Add(78);
            Assert.AreEqual(
                double.NaN,
                new CompoundAminoAcid(
                    'A',
                    Constants.CompoundItemName).GetWeight(seq));
            ApplicationLog.WriteLine(
                "CompoundAminoAcid : Successfully validated the GetWeight for dublicate item method");
            Console.WriteLine(
                "CompoundAminoAcid : Successfully validated the GetWeight for dublicate item method");
        }
        #endregion Amino Acid Test cases

        #endregion

        #region Supported Methods

        static void GetSequenceItemWithWeight(out List<ISequenceItem> seqItemList,
            out List<double> weightList, bool isNucleotide)
        {
            ISequenceItem seqItem1 = null;
            ISequenceItem seqItem2 = null;

            if (isNucleotide)
            {
                seqItem1 = new Sequence(Alphabets.DNA, "AGCT")[0];
                seqItem2 = new Sequence(Alphabets.DNA, "GCT")[0];
            }
            else
            {
                seqItem1 = new Sequence(Alphabets.Protein, "KIET")[0];
                seqItem2 = new Sequence(Alphabets.Protein, "IET")[0];
            }
            seqItemList = new List<ISequenceItem>();
            seqItemList.Add(seqItem1);
            seqItemList.Add(seqItem2);
            double weightItem1 = 35;
            double weightItem2 = 33;
            weightList = new List<double>();
            weightList.Add(weightItem1);
            weightList.Add(weightItem2);
        }

        #endregion Supported Methods
    }
}
