// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * CompoundSequenceP1TestCases.cs
 * 
 * This file contains the Compound Sequence P1 test case validation.
 * 
******************************************************************************/

using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBF.Util.Logging;
using MBF.TestAutomation.Util;

namespace MBF.TestAutomation
{
    /// <summary>
    /// P1 test cases to confirm the features of Compound Nucleotide and Compound Amino Acid
    /// </summary>
    [TestClass]
    public class CompoundSequenceP1TestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static CompoundSequenceP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion

        #region Test Cases

        #region Compound Nucleotide Test cases

        /// <summary>
        /// Creates the Compound Nucleotide ctor(char,name) and add it sequence.
        /// Validates the sequence symbol and compound nucleotide.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundNucleotide()
        {
            ValidateCompoundSequenceItem(Alphabets.RNA, Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Creates the Compound Nucleotide ctor(byte,char,name) and add it to sequence.
        /// Validates the sequence symbol and compound nucleotide.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompounNucleotideWithByte()
        {
            ValidateCompoundSequenceItemWithByteValue(Alphabets.RNA,
                Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Creates compound nucleotide with ctor(byte, char,name,isgap,isambiguous).
        /// Adds ambiguous compound nucleotide and other compound nucleotide to sparse sequence.
        /// Validates ambiguous and other compound nucleotide
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateAmbiguousCompoundNucleotide()
        {
            ValidateCompoundSequenceItemWithAmbiguous(Alphabets.RNA,
                Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Creates gap compound nucleotide with ctor(byte, char,name,isgap,isambiguous).
        /// Adds gap compound nucleotide to sparse sequence.
        /// Validates the gap compound nucleotide
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateGapCompoundNucleotide()
        {
            ValidateCompoundSequenceItemWithGap(Alphabets.RNA,
                Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Creates compound  nucleotide with ctor(char,name,nucleo list, weights).
        /// Add compound nucleotide to sequence.
        /// Validates the sequence symbol, sequence item and weights.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundNucleotideWithItemList()
        {
            ValidateCompoundSequenceItemWithItemList(Alphabets.RNA,
                Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Creates compound  nucleotide with ctor(byte,char,name,nucleo list, weights)
        /// Add compound nucleotide to sequence.
        /// Validates the sequence symbol, sequence item, weights and byte.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundNucleotideWithItemListandByte()
        {
            ValidateCompoundSequenceItemWithItemListandByte(Alphabets.RNA,
                Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Creates compound nucleotide with ctor(char,name,nucleo list, weights, isambgious, isgap).
        /// Adds ambiguous compound nucleotide and other compound nucleotide to sparse sequence.
        /// Validates ambiguous and other compound nucleotide
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateAmbiguousCompoundNucleotideWithItemList()
        {
            ValidateCompoundSequenceItemListWithAmbiguousChar(Alphabets.RNA,
                Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Creates compound nucleotide with ctor(char,name,nucleo list, weights, isambgious, isgap).
        /// Adds gap compound nucleotide and other compound nucleotide to sparse sequence.
        /// Validates gap and other compound nucleotide
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateGapCompoundNucleotideWithItemList()
        {
            ValidateCompoundSequenceItemListWithGapChar(Alphabets.RNA,
                Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Creates compound nucleotide with ctor(byte, char,name,nucleo list, weights, isambgious, isgap) .
        /// Adds ambiguous compound nucleotide and other compound nucleotide to sparse sequence.
        /// Validates ambiguous and other compound nucleotide
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateAmbiguousCompoundNucleotideWithItemListAndByte()
        {
            ValidateAmbiguousCompoundSequenceItemListAndByte(Alphabets.RNA,
                Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Creates the Compound Nucleotide ctor(char,name). Add nucleotides.
        /// Validate the compound nucleotide and sequence items
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundNucleotideAdd()
        {
            ValidateCompoundSequenceItem(Alphabets.RNA, Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Create Compound Nucleotide and add sequence item.
        /// Create the copy of Compound Nucleotide using Clone() method.
        /// Validate the cloned compound nucleotide.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundNucleotideClone()
        {
            ValidateCompoundSequenceItemClone(Alphabets.RNA, Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Create compound nucleotide and add sequence item with weights
        /// GetWeights() and validate the added weights
        /// Set new weights using SetWeight()
        /// Validate the new set weights.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundNucleotideGetWeightsAndSetWeights()
        {
            ValidateCompoundSequenceItemGetWeightAndSetWeight(Alphabets.RNA,
                Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Create Compound Nucleotide and add new sequence item using Add() method.
        /// Remove few sequence item() and validate the compound nucleotide item.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundNucleotideAddandRemove()
        {
            ValidateCompoundSequenceItemAddandRemove(Alphabets.RNA,
                Constants.CompoundNucleotideRnaNode);
        }

        /// <summary>
        /// Creates Compound Nucleotide and searlize it into stream. Deserialize the stream 
        /// and validate the retrieved compound nucleotide.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundNucleotideSerializeAndDeserialize()
        {
            ValidateCompoundSequenceItemSerializeandDeserialize(Alphabets.RNA,
                Constants.CompoundNucleotideRnaNode);
        }

        #endregion

        #region Compound AminoAcid Test cases

        /// <summary>
        /// Creates the Compound AminoAcid ctor(char,name) and add it sequence.
        /// Validates the sequence symbol and compound AminoAcid.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundAminoAcid()
        {
            ValidateCompoundSequenceItem(Alphabets.Protein, Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// Creates compound AminoAcid with ctor(byte, char,name,isgap,isambiguous).
        /// Adds ambiguous compound AminoAcid and other compound AminoAcid to sparse sequence.
        /// Validates ambiguous and other compound AminoAcid
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateAmbiguousCompoundAminoAcid()
        {
            ValidateCompoundSequenceItemWithAmbiguous(Alphabets.Protein,
                Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// Creates gap compound AminoAcid with ctor(byte, char,name,isgap,isambiguous).
        /// Adds gap compound AminoAcid to sparse sequence.
        /// Validates the gap compound AminoAcid
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateGapCompoundAminoAcid()
        {
            ValidateCompoundSequenceItemWithGap(Alphabets.Protein,
                Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// Creates compound  AminoAcid with ctor(char,name,nucleo list, weights).
        /// Add compound AminoAcid to sequence.
        /// Validates the sequence symbol, sequence item and weights.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundAminoAcidWithItemList()
        {
            ValidateCompoundSequenceItemWithItemList(Alphabets.Protein,
                Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// Creates compound AminoAcid with ctor(char,name,nucleo list, weights, isambgious, isgap).
        /// Adds ambiguous compound AminoAcid and other compound AminoAcid to sparse sequence.
        /// Validates ambiguous and other compound AminoAcid
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateAmbiguousCompoundAminoacidWithItemList()
        {
            ValidateCompoundSequenceItemListWithAmbiguousChar(Alphabets.Protein,
                Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// Creates compound AminoAcid with ctor(byte, char,name,nucleo list, weights, isambgious, isgap) .
        /// Adds ambiguous compound AminoAcid and other compound AminoAcid to sparse sequence.
        /// Validates ambiguous and other compound AminoAcid
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundAminoAcidWithItemListAndByte()
        {
            ValidateAmbiguousCompoundSequenceItemListAndByte(Alphabets.Protein,
                Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// Creates the Compound AminoAcid ctor(char,name). Add AminoAcids.
        /// Validate the compound AminoAcid and sequence items
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundAminoAcidAdd()
        {
            ValidateCompoundSequenceItem(Alphabets.Protein, Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// Create Compound AminoAcid and add sequence item.
        /// Create the copy of Compound AminoAcid using Clone() method.
        /// Validate the cloned compound AminoAcid.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundAminoAcidClone()
        {
            ValidateCompoundSequenceItemClone(Alphabets.Protein, Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// Create compound AminoAcid and add sequence item with weights
        /// GetWeights() and validate the added weights
        /// Set new weights using SetWeight()
        /// Validate the new set weights.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundAminoAcidGetWeightsAndSetWeights()
        {
            ValidateCompoundSequenceItemGetWeightAndSetWeight(Alphabets.Protein,
                Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// Create Compound AminoAcid and add new sequence item using Add() method.
        /// Remove few sequence item() and validate the compound AminoAcid item.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateCompoundAminoAcidAddandRemove()
        {
            ValidateCompoundSequenceItemAddandRemove(Alphabets.Protein,
                Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidetCompoundAminoAcidProperties()
        {
            ValidateCompoundSequenceItemProperties(Alphabets.Protein,
                Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// Creates multiple gap compound AminoAcid with ctor(byte, char,name,isgap,isambiguous).
        /// Adds multiple gap compound AminoAcid to sparse sequence.
        /// Validates all gap compound AminoAcids
        /// </summary>
        [TestMethod]
        [Priority(1)]
        [TestCategory("Priority1")]
        public void ValidateMultipleGapCompoundAminoAcid()
        {
            ValidateMultipleCompoundSequenceItemWithGap(Alphabets.Protein,
                Constants.CompoundAminoAcidNode, 4);
        }

        #endregion

        #endregion

        #region Helper Methods

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. 
        /// Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItem(IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sequence
            Sequence seq = new Sequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights);
            seq.Add(compoundItem);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(seq.ToString(), compoundItem.Symbol.ToString((IFormatProvider)null));
            for (int iSeq = 0; iSeq < compoundItem.SequenceItems.Count; iSeq++)
            {
                Assert.AreEqual(sequenceItems[iSeq],
                    compoundItem.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                Assert.AreEqual(sequenceItemWeights[iSeq],
                    compoundItem.GetWeight(compoundItem.SequenceItems[iSeq]).ToString((IFormatProvider)null));
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item ctor(byte,name)");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item ctor(byte,name)");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. 
        /// Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemProperties(IAlphabet alphabet,
            string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sequence
            Sequence seq = new Sequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights);
            seq.Add(compoundItem);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(seq.ToString(), compoundItem.Symbol.ToString((IFormatProvider)null));
            Assert.IsFalse(compoundItem.IsAmbiguous);
            Assert.IsFalse(compoundItem.IsGap);
            Assert.IsFalse(compoundItem.IsTermination);
            Assert.AreEqual((byte)compoundItem.Symbol, compoundItem.Value);
            for (int iSeq = 0; iSeq < compoundItem.SequenceItems.Count; iSeq++)
            {
                Assert.AreEqual(sequenceItems[iSeq],
                    compoundItem.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                Assert.AreEqual(sequenceItemWeights[iSeq],
                    compoundItem.GetWeight(compoundItem.SequenceItems[iSeq]).ToString((IFormatProvider)null));
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item properties");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item properties");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. 
        /// Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemAddandRemove(IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sequence
            SparseSequence spSequence = new SparseSequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights);
            spSequence.Count = 10;
            spSequence[5] = compoundItem;

            // Remove second Item
            ICompoundSequenceItem item = spSequence[5] as ICompoundSequenceItem;
            item.Remove(item.SequenceItems[1]);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(baseSymbol.ToString((IFormatProvider)null), item.Symbol.ToString((IFormatProvider)null));
            int iItem = 0;
            for (int iSeq = 0; iSeq < item.SequenceItems.Count; iSeq++)
            {
                if (iSeq >= 1)
                {
                    iItem = iSeq + 1;
                }

                Assert.AreEqual(sequenceItems[iItem], item.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                Assert.AreEqual(sequenceItemWeights[iItem],
                    item.GetWeight(item.SequenceItems[iSeq]).ToString((IFormatProvider)null));
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item Add() and Remove()");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item Add() and Remove()");
        }

        /// <summary>
        /// Creates Compound Sequence Item and searlize it into stream. Deserialize the stream 
        /// and validate the retrieved compound nucleotide.
        /// </summary>
        private void ValidateCompoundSequenceItemSerializeandDeserialize(
            IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sequence
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights);
            using (Stream stream = File.Open("CompoundItem.data", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, compoundItem);

                stream.Seek(0, SeekOrigin.Begin);
                ICompoundSequenceItem deserializeditem =
                    (ICompoundSequenceItem)formatter.Deserialize(stream);

                // Validate the compoundsequenceitem and sequence.
                Assert.AreEqual(baseSymbol, deserializeditem.Symbol.ToString((IFormatProvider)null));
                for (int iSeq = 0; iSeq < deserializeditem.SequenceItems.Count; iSeq++)
                {
                    Assert.AreEqual(sequenceItems[iSeq],
                        deserializeditem.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                    Assert.AreEqual(sequenceItemWeights[iSeq],
                        deserializeditem.GetWeight(deserializeditem.SequenceItems[iSeq]).ToString((IFormatProvider)null));
                }
            }
            Console.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item Serialize and Deserialize");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item Serialize and Deserialize");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. 
        /// Add the item to sequence.
        /// Create the cloned compound sequence item using Clone() method.
        /// Validate the compound sequence item.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemClone(IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sequence
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights);

            // Validate the cloned compoundsequenceitem.
            ICompoundSequenceItem item = compoundItem.Clone();
            for (int iSeq = 0; iSeq < item.SequenceItems.Count; iSeq++)
            {
                Assert.AreEqual(sequenceItems[iSeq], item.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                Assert.AreEqual(sequenceItemWeights[iSeq],
                    item.GetWeight(item.SequenceItems[iSeq]).ToString((IFormatProvider)null));
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item Clone()");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item Clone()");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. Add the item to sequence.
        /// Get old weights and validate it. Set new weights using SetWeights() and validate it.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemGetWeightAndSetWeight(
            IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sequence
            SparseSequence spseq = new SparseSequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights);
            spseq.Count = 50;
            spseq[10] = compoundItem;

            // Validate the compoundsequenceitem and sequence.
            // Get old weights and validate it
            // set new weights and validate it
            Assert.AreEqual(baseSymbol, spseq[10].Symbol.ToString((IFormatProvider)null));
            ICompoundSequenceItem item = spseq[10] as ICompoundSequenceItem;
            int iseqlast = item.SequenceItems.Count - 1;
            for (int iSeq = 0; iSeq < item.SequenceItems.Count; iSeq++)
            {

                Assert.AreEqual(sequenceItems[iSeq],
                    item.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                Assert.AreEqual(sequenceItemWeights[iSeq],
                    item.GetWeight(item.SequenceItems[iSeq]).ToString((IFormatProvider)null));

                // Set new weight
                item.SetWeight(item.SequenceItems[iSeq],
                    byte.Parse(sequenceItemWeights[iseqlast], (IFormatProvider)null));
                Assert.AreEqual(sequenceItemWeights[iseqlast],
                   item.GetWeight(item.SequenceItems[iSeq]).ToString((IFormatProvider)null));
                iseqlast--;
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item GetWeight() and SetWeight()");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item GetWeight() and SetWeight()");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. Add the item to sequence.
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemWithByteValue(IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');

            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            string baseValueString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseValuesNode);
            string[] baseValues = baseValueString.Split(',');
            string baseValue = baseValues[0];

            // Add the item to sequence
            Sequence seq = new Sequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights, baseValue);
            seq.Add(compoundItem);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(seq.ToString(), compoundItem.Symbol.ToString((IFormatProvider)null));
            Assert.AreEqual(baseValue, compoundItem.Value.ToString((IFormatProvider)null));
            for (int iSeq = 0; iSeq < compoundItem.SequenceItems.Count; iSeq++)
            {
                Assert.AreEqual(sequenceItems[iSeq],
                    compoundItem.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                Assert.AreEqual(sequenceItemWeights[iSeq],
                    compoundItem.GetWeight(compoundItem.SequenceItems[iSeq]).ToString((IFormatProvider)null));
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item with byte value");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item with byte value");
        }

        /// <summary>
        /// Create ambiguious compound sequence item using sequence items and weights. Add the item to sparse sequence.
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemWithAmbiguous(
            IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');

            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sparse sequence
            SparseSequence seq = new SparseSequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights, false, true);
            seq.Count = baseSymbols.Length + 10;
            seq[0] = compoundItem;

            for (int ibase = 1; ibase < baseSymbols.Length; ibase++)
            {
                compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbols[ibase], sequenceItems, sequenceItemWeights);
                seq[ibase] = compoundItem;
            }

            // Validate the compoundsequenceitem and sequence.
            for (int iItem = 0; iItem < baseSymbols.Length; iItem++)
            {
                ICompoundSequenceItem item = seq[iItem] as ICompoundSequenceItem;
                Assert.AreEqual(baseSymbols[iItem], item.Symbol.ToString((IFormatProvider)null));
                for (int iSeq = 0; iSeq < item.SequenceItems.Count; iSeq++)
                {
                    Assert.AreEqual(sequenceItems[iSeq],
                        item.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                    Assert.AreEqual(sequenceItemWeights[iSeq],
                        item.GetWeight(item.SequenceItems[iSeq]).ToString((IFormatProvider)null));
                }
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating ambiguous compound sequence item");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating ambiguous compound sequence item");
        }

        /// <summary>
        /// Create gap compound sequence item using sequence items and weights. 
        /// Add the item to sparse sequence.
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemWithGap(IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbol = "-";

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');

            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sequence
            Sequence seq = new Sequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights, true, false);
            seq.Add(compoundItem);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(seq.ToString(), compoundItem.Symbol.ToString((IFormatProvider)null));
            for (int iSeq = 0; iSeq < compoundItem.SequenceItems.Count; iSeq++)
            {
                Assert.AreEqual(sequenceItems[iSeq],
                    compoundItem.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                Assert.AreEqual(sequenceItemWeights[iSeq],
                    compoundItem.GetWeight(compoundItem.SequenceItems[iSeq]).ToString((IFormatProvider)null));
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating gap compound sequence item");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating gap compound sequence item");
        }

        /// <summary>
        /// Create gap compound sequence item using sequence items and weights. 
        /// Add the item to sparse sequence.
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="baseSymbolCount">Number of base symbol</param>
        private void ValidateMultipleCompoundSequenceItemWithGap(
            IAlphabet alphabet, string nodeName, int baseSymbolCount)
        {
            // Read xml nodes
            string baseSymbol = "-";

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');

            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sequence
            SparseSequence spSequence = new SparseSequence(alphabet);
            spSequence.Count = baseSymbolCount + 10;
            for (int iItem = 0; iItem < baseSymbolCount; iItem++)
            {
                ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                    alphabet, baseSymbol, sequenceItems, sequenceItemWeights, true, false);
                spSequence[iItem] = compoundItem;
            }

            // Validate the compoundsequenceitem and sequence.
            for (int icmpItem = 0; icmpItem < baseSymbolCount; icmpItem++)
            {
                ICompoundSequenceItem cmpItem = spSequence[icmpItem] as ICompoundSequenceItem;
                Assert.AreEqual(baseSymbol, cmpItem.Symbol.ToString((IFormatProvider)null));
                for (int iSeq = 0; iSeq < cmpItem.SequenceItems.Count; iSeq++)
                {
                    Assert.AreEqual(sequenceItems[iSeq],
                        cmpItem.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                    Assert.AreEqual(sequenceItemWeights[iSeq],
                        cmpItem.GetWeight(cmpItem.SequenceItems[iSeq]).ToString((IFormatProvider)null));
                }
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating multiple gap compound sequence item");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating multiple gap compound sequence item");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemWithItemList(
            IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            List<ISequenceItem> lstSequenceItems = new List<ISequenceItem>();
            for (int iSeq = 0; iSeq < sequenceItems.Length; iSeq++)
            {
                Sequence seq = new Sequence(alphabet, sequenceItems[iSeq]);
                lstSequenceItems.Add(seq[0]);
            }

            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');
            List<double> lstWeights = new List<double>();
            for (int iweight = 0; iweight < sequenceItemWeights.Length; iweight++)
            {
                lstWeights.Add(double.Parse(sequenceItemWeights[iweight], (IFormatProvider)null));
            }

            // Add the item to sequence
            Sequence sequence = new Sequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, lstSequenceItems, lstWeights);
            sequence.Add(compoundItem);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(sequence.ToString(), compoundItem.Symbol.ToString((IFormatProvider)null));
            for (int iSeq = 0; iSeq < compoundItem.SequenceItems.Count; iSeq++)
            {
                Assert.AreEqual(sequenceItems[iSeq],
                    compoundItem.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                Assert.AreEqual(sequenceItemWeights[iSeq],
                    compoundItem.GetWeight(compoundItem.SequenceItems[iSeq]).ToString((IFormatProvider)null));
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item with item list");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item with item list");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemWithItemListandByte(
            IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            List<ISequenceItem> lstSequenceItems = new List<ISequenceItem>();
            for (int iSeq = 0; iSeq < sequenceItems.Length; iSeq++)
            {
                Sequence seq = new Sequence(alphabet, sequenceItems[iSeq]);
                lstSequenceItems.Add(seq[0]);
            }

            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');
            List<double> lstWeights = new List<double>();
            for (int iweight = 0; iweight < sequenceItemWeights.Length; iweight++)
            {
                lstWeights.Add(double.Parse(sequenceItemWeights[iweight], (IFormatProvider)null));
            }

            string baseValueString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseValuesNode);
            string[] baseValues = baseValueString.Split(',');
            string baseValue = baseValues[0];

            // Add the item to sequence
            Sequence sequence = new Sequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, lstSequenceItems, lstWeights, byte.Parse(baseValue, (IFormatProvider)null));
            sequence.Add(compoundItem);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(sequence.ToString(), compoundItem.Symbol.ToString((IFormatProvider)null));
            for (int iSeq = 0; iSeq < compoundItem.SequenceItems.Count; iSeq++)
            {
                Assert.AreEqual(sequenceItems[iSeq],
                    compoundItem.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                Assert.AreEqual(sequenceItemWeights[iSeq],
                    (compoundItem as CompoundNucleotide).GetWeight(compoundItem.SequenceItems[iSeq]).ToString((IFormatProvider)null));
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item with item list");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating compound sequence item with item list");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemListWithAmbiguousChar(
            IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            List<ISequenceItem> lstSequenceItems = new List<ISequenceItem>();
            for (int iSeq = 0; iSeq < sequenceItems.Length; iSeq++)
            {
                Sequence seq = new Sequence(alphabet, sequenceItems[iSeq]);
                lstSequenceItems.Add(seq[0]);
            }

            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');
            List<double> lstWeights = new List<double>();
            for (int iweight = 0; iweight < sequenceItemWeights.Length; iweight++)
            {
                lstWeights.Add(double.Parse(sequenceItemWeights[iweight], (IFormatProvider)null));
            }

            // Add the item to sparse sequence
            SparseSequence spsequence = new SparseSequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, lstSequenceItems, lstWeights, false, true);
            spsequence.Count = baseSymbols.Length + 10;
            spsequence[0] = compoundItem;

            for (int ibase = 1; ibase < baseSymbols.Length; ibase++)
            {
                compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbols[ibase], sequenceItems, sequenceItemWeights);
                spsequence[ibase] = compoundItem;
            }

            // Validate the compoundsequenceitem and sequence.
            for (int iItem = 0; iItem < baseSymbols.Length; iItem++)
            {
                ICompoundSequenceItem item = spsequence[iItem] as ICompoundSequenceItem;
                Assert.AreEqual(baseSymbols[iItem], item.Symbol.ToString((IFormatProvider)null));
                for (int iSeq = 0; iSeq < item.SequenceItems.Count; iSeq++)
                {
                    Assert.AreEqual(sequenceItems[iSeq],
                        item.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                    Assert.AreEqual(sequenceItemWeights[iSeq],
                        item.GetWeight(item.SequenceItems[iSeq]).ToString((IFormatProvider)null));
                }
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating ambiguous compound sequence item with item list");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating ambiguous compound sequence item with item list");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemListWithGapChar(IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = "-";

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            List<ISequenceItem> lstSequenceItems = new List<ISequenceItem>();
            for (int iSeq = 0; iSeq < sequenceItems.Length; iSeq++)
            {
                Sequence seq = new Sequence(alphabet, sequenceItems[iSeq]);
                lstSequenceItems.Add(seq[0]);
            }

            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');
            List<double> lstWeights = new List<double>();
            for (int iweight = 0; iweight < sequenceItemWeights.Length; iweight++)
            {
                lstWeights.Add(double.Parse(sequenceItemWeights[iweight], (IFormatProvider)null));
            }

            // Add the item to sparse sequence
            SparseSequence spsequence = new SparseSequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, lstSequenceItems, lstWeights, false, true);
            spsequence.Count = baseSymbols.Length + 10;
            spsequence[0] = compoundItem;

            for (int ibase = 1; ibase < baseSymbols.Length; ibase++)
            {
                compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbols[ibase], sequenceItems, sequenceItemWeights);
                spsequence[ibase] = compoundItem;
            }

            // Validate the compoundsequenceitem and sequence.
            for (int iItem = 0; iItem < baseSymbols.Length; iItem++)
            {
                ICompoundSequenceItem item = spsequence[iItem] as ICompoundSequenceItem;
                if (iItem > 0)
                {
                    Assert.AreEqual(baseSymbols[iItem], item.Symbol.ToString((IFormatProvider)null));
                }
                else
                {
                    Assert.AreEqual("-", item.Symbol.ToString((IFormatProvider)null));
                }
                for (int iSeq = 0; iSeq < item.SequenceItems.Count; iSeq++)
                {
                    Assert.AreEqual(sequenceItems[iSeq],
                        item.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                    Assert.AreEqual(sequenceItemWeights[iSeq],
                        item.GetWeight(item.SequenceItems[iSeq]).ToString((IFormatProvider)null));
                }
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating gap compound sequence item");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating gap compound sequence item");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateAmbiguousCompoundSequenceItemListAndByte(
            IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            List<ISequenceItem> lstSequenceItems = new List<ISequenceItem>();
            for (int iSeq = 0; iSeq < sequenceItems.Length; iSeq++)
            {
                Sequence seq = new Sequence(alphabet, sequenceItems[iSeq]);
                lstSequenceItems.Add(seq[0]);
            }

            string sequenceItemWeightsString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');
            List<double> lstWeights = new List<double>();
            for (int iweight = 0; iweight < sequenceItemWeights.Length; iweight++)
            {
                lstWeights.Add(double.Parse(sequenceItemWeights[iweight], (IFormatProvider)null));
            }

            string baseValueString = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.BaseValuesNode);
            string[] baseValues = baseValueString.Split(',');
            string baseValue = baseValues[0];

            // Add the item to sparse sequence
            SparseSequence spsequence = new SparseSequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, lstSequenceItems, lstWeights,
                false, true, byte.Parse(baseValue, (IFormatProvider)null));
            spsequence.Count = baseSymbols.Length + 10;
            spsequence[0] = compoundItem;

            for (int ibase = 1; ibase < baseSymbols.Length; ibase++)
            {
                compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbols[ibase], sequenceItems, sequenceItemWeights);
                spsequence[ibase] = compoundItem;
            }

            // Validate the compoundsequenceitem and sequence.
            for (int iItem = 0; iItem < baseSymbols.Length; iItem++)
            {
                ICompoundSequenceItem item = spsequence[iItem] as ICompoundSequenceItem;
                Assert.AreEqual(baseSymbols[iItem], item.Symbol.ToString((IFormatProvider)null));
                if (0 == iItem)
                {
                    Assert.AreEqual(baseValue, item.Value.ToString((IFormatProvider)null));
                }
                for (int iSeq = 0; iSeq < item.SequenceItems.Count; iSeq++)
                {
                    Assert.AreEqual(sequenceItems[iSeq], item.SequenceItems[iSeq].Symbol.ToString((IFormatProvider)null));
                    Assert.AreEqual(sequenceItemWeights[iSeq],
                        item.GetWeight(item.SequenceItems[iSeq]).ToString((IFormatProvider)null));
                }
            }

            Console.WriteLine(
                "CompoundSequence P1: Completed validating ambiguous compound sequence item");
            ApplicationLog.WriteLine(
                "CompoundSequence P1: Completed validating ambiguous compound sequence item");
        }

        /// <summary>
        /// Create compund nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="baseSymbol">base compound item symbo.l</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <returns>returns compound sequence item.</returns>
        private static ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
            string baseSymbol, string[] sequenceItems, string[] weights)
        {
            ICompoundSequenceItem compoundItem = null;

            if (alphabet == Alphabets.DNA || alphabet == Alphabets.RNA)
            {
                compoundItem = new CompoundNucleotide(baseSymbol[0], Constants.CompoundItemName);
                for (int iSeq = 0; iSeq < sequenceItems.Length; iSeq++)
                {
                    compoundItem.Add(new Nucleotide(
                        sequenceItems[iSeq][0], String.Concat("Item",
                        sequenceItems[iSeq].ToString((IFormatProvider)null))), double.Parse(weights[iSeq], (IFormatProvider)null));
                }
            }
            else if (alphabet == Alphabets.Protein)
            {
                compoundItem = new CompoundAminoAcid(baseSymbol[0], Constants.CompoundItemName);
                for (int iSeq = 0; iSeq < sequenceItems.Length; iSeq++)
                {
                    compoundItem.Add(new AminoAcid(
                        sequenceItems[iSeq][0], String.Concat("Item",
                        sequenceItems[iSeq].ToString((IFormatProvider)null))), double.Parse(weights[iSeq], (IFormatProvider)null));
                }
            }

            return compoundItem;
        }

        /// <summary>
        /// Create compund nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="baseSymbol">base compound item symbol</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <param name="value">byte value of base symbol.</param>
        /// <returns>returns compound sequence item.</returns>
        private static ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
            string baseSymbol, string[] sequenceItems, string[] weights, string value)
        {
            ICompoundSequenceItem compoundItem = null;

            if (alphabet == Alphabets.DNA || alphabet == Alphabets.RNA)
            {
                compoundItem = new CompoundNucleotide(byte.Parse(value, (IFormatProvider)null),
                    baseSymbol[0], Constants.CompoundItemName);
                for (int iSeq = 0; iSeq < sequenceItems.Length; iSeq++)
                {
                    compoundItem.Add(new Nucleotide(
                        sequenceItems[iSeq][0], String.Concat("Item",
                        sequenceItems[iSeq].ToString((IFormatProvider)null))), double.Parse(weights[iSeq], (IFormatProvider)null));
                }
            }
            else if (alphabet == Alphabets.Protein)
            {
                compoundItem = new CompoundAminoAcid(byte.Parse(value, (IFormatProvider)null),
                    baseSymbol[0], Constants.CompoundItemName);
                for (int iSeq = 0; iSeq < sequenceItems.Length; iSeq++)
                {
                    compoundItem.Add(new AminoAcid(
                        sequenceItems[iSeq][0], String.Concat("Item",
                        sequenceItems[iSeq].ToString((IFormatProvider)null))), double.Parse(weights[iSeq], (IFormatProvider)null));
                }
            }

            return compoundItem;
        }

        /// <summary>
        /// Create compund nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="baseSymbol">base compound item symbol</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <param name="isGap">true\false base symbol is gap or not</param>
        /// <param name="isAmbiguous">true\false base symbol is ambiguous or not.</param>
        /// <returns>returns compound sequence item.</returns>
        private static ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
            string baseSymbol, string[] sequenceItems, string[] weights, bool isGap, bool isAmbiguous)
        {
            ICompoundSequenceItem compoundItem = null;

            if (alphabet == Alphabets.DNA || alphabet == Alphabets.RNA)
            {
                compoundItem = new CompoundNucleotide(baseSymbol[0],
                    Constants.CompoundItemName, isGap, isAmbiguous);
                for (int iSeq = 0; iSeq < sequenceItems.Length; iSeq++)
                {
                    compoundItem.Add(new Nucleotide(
                        sequenceItems[iSeq][0], String.Concat("Item",
                        sequenceItems[iSeq].ToString((IFormatProvider)null))), double.Parse(weights[iSeq], (IFormatProvider)null));
                }
            }
            else if (alphabet == Alphabets.Protein)
            {
                compoundItem = new CompoundAminoAcid(baseSymbol[0],
                    Constants.CompoundItemName, isGap, isAmbiguous);
                for (int iSeq = 0; iSeq < sequenceItems.Length; iSeq++)
                {
                    compoundItem.Add(new AminoAcid(
                        sequenceItems[iSeq][0], String.Concat("Item",
                        sequenceItems[iSeq].ToString((IFormatProvider)null))), double.Parse(weights[iSeq], (IFormatProvider)null));
                }
            }

            return compoundItem;
        }

        /// <summary>
        /// Create compund nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="baseSymbol">base compound item symbol</param>
        /// <param name="sequenceItems">sequence items list.</param>
        /// <param name="weights">weights list.</param>
        /// <returns>returns compound sequence item.</returns>
        private static ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
            string baseSymbol, List<ISequenceItem> sequenceItems, List<double> weights)
        {
            ICompoundSequenceItem compoundItem = null;

            if (alphabet == Alphabets.DNA || alphabet == Alphabets.RNA)
            {
                compoundItem = new CompoundNucleotide(
                    baseSymbol[0], Constants.CompoundItemName, sequenceItems, weights);
            }
            else if (alphabet == Alphabets.Protein)
            {
                compoundItem = new CompoundAminoAcid(
                    baseSymbol[0], Constants.CompoundItemName, sequenceItems, weights);
            }

            return compoundItem;
        }

        /// <summary>
        /// Create compund nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="baseSymbol">base compound item symbol</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <param name="value">byte value of base symbol.</param>
        /// <returns>returns compound sequence item.</returns>
        private static ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
            string baseSymbol, List<ISequenceItem> sequenceItems, List<double> weights, byte value)
        {
            ICompoundSequenceItem compoundItem = null;

            if (alphabet == Alphabets.DNA || alphabet == Alphabets.RNA)
            {
                compoundItem = new CompoundNucleotide(
                    value, baseSymbol[0], Constants.CompoundItemName, sequenceItems, weights);
            }
            else if (alphabet == Alphabets.Protein)
            {
                compoundItem = new CompoundAminoAcid(
                    value, baseSymbol[0], Constants.CompoundItemName, sequenceItems, weights);
            }

            return compoundItem;
        }

        /// <summary>
        /// Create compund nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="baseSymbol">base compound item symbol</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <param name="isGap">true\false base symbol is gap or not</param>
        /// <param name="isAmbiguous">true\false base symbol is ambiguous or not.</param>
        /// <returns>returns compound sequence item.</returns>
        private static ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
            string baseSymbol, List<ISequenceItem> sequenceItems,
            List<double> weights, bool isGap, bool isAmbiguous)
        {
            ICompoundSequenceItem compoundItem = null;

            if (alphabet == Alphabets.DNA || alphabet == Alphabets.RNA)
            {
                compoundItem = new CompoundNucleotide(
                    baseSymbol[0], Constants.CompoundItemName,
                    isGap, isAmbiguous, sequenceItems, weights);
            }
            else if (alphabet == Alphabets.Protein)
            {
                compoundItem = new CompoundAminoAcid(
                    baseSymbol[0], Constants.CompoundItemName,
                    isGap, isAmbiguous, sequenceItems, weights);
            }

            return compoundItem;
        }

        /// <summary>
        /// Create compund nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="baseSymbol">base compound item symbol</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <param name="isGap">true\false base symbol is gap or not</param>
        /// <param name="isAmbiguous">true\false base symbol is ambiguous or not.</param>
        /// <param name="value">byte value</param>
        /// <returns>returns compound sequence item.</returns>
        private static ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
            string baseSymbol, List<ISequenceItem> sequenceItems,
            List<double> weights, bool isGap, bool isAmbiguous, byte value)
        {
            ICompoundSequenceItem compoundItem = null;

            if (alphabet == Alphabets.DNA || alphabet == Alphabets.RNA)
            {
                compoundItem = new CompoundNucleotide(
                    value, baseSymbol[0], Constants.CompoundItemName,
                    isGap, isAmbiguous, sequenceItems, weights);
            }
            else if (alphabet == Alphabets.Protein)
            {
                compoundItem = new CompoundAminoAcid(
                    value, baseSymbol[0], Constants.CompoundItemName,
                    isGap, isAmbiguous, sequenceItems, weights);
            }

            return compoundItem;
        }

        #endregion;
    }
}
