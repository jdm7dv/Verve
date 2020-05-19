// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * CompoundSequenceBVTTestCases.cs
 * 
 * This file contains the Compound Sequence BVT test case validation.
 * 
******************************************************************************/

using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;
using MBF.Util.Logging;
using MBF.TestAutomation.Util;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Bvt test cases to confirm the features of Compound Nucleotide and Compound Amino Acid
    /// </summary>
    [TestFixture]
    public class CompoundSequenceBvtTestCases
    {

        #region Enums

        /// <summary>
        /// Additional parameters to validate different scenarios.
        /// </summary>
        enum AdditionalParameters
        {
            Constructor_CSSLL,
            Constructor_CSBBSLL,
            Constructor_BCSLL,
            Constructor_BCSSLL,
            Constructor_BCSSBBLL,
            Constructor_CSSBBLL,
            Constructor
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static CompoundSequenceBvtTestCases()
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

        #region Compound Nucleotide Test cases

        /// <summary>
        /// Creates the Compound Nucleotide ctor(char,name) and add it sequence.
        /// Validates the sequence symbol and compound nucleotide.
        /// </summary>
        [Test]
        public void ValidateCompoundNucleotide()
        {
            ValidateCompoundSequenceItem(Alphabets.DNA, Constants.CompoundNucleotideNode);
        }

        /// <summary>
        /// Creates the Compound Nucleotide ctor(byte,char,name) and add it to sequence.
        /// Validates the sequence symbol and compound nucleotide.
        /// </summary>
        [Test]
        public void ValidateCompounNucleotideWithByte()
        {
            ValidateCompoundSequenceItemWithByteValue(Alphabets.DNA,
                Constants.CompoundNucleotideNode);
        }

        /// <summary>
        /// Creates compound nucleotide with ctor(byte, char,name,isgap,isambiguous).
        /// Adds ambiguous compound nucleotide and other compound nucleotide to sparse sequence.
        /// Validates ambiguous and other compound nucleotide
        /// </summary>
        [Test]
        public void ValidateAmbiguousCompoundNucleotide()
        {
            ValidateCompoundSequenceItemWithAmbiguous(Alphabets.DNA,
                Constants.CompoundNucleotideNode);
        }

        /// <summary>
        /// Creates gap compound nucleotide with ctor(byte, char,name,isgap,isambiguous).
        /// Adds gap compound nucleotide to sparse sequence.
        /// Validates the gap compound nucleotide
        /// </summary>
        [Test]
        public void ValidateGapCompoundNucleotide()
        {
            ValidateCompoundSequenceItemWithGap(Alphabets.DNA,
                Constants.CompoundNucleotideNode);
        }

        /// <summary>
        /// Creates compound nucleotide with ctor(char, name, nucleo list, weights).
        /// Add compound nucleotide to sequence.
        /// Validates the sequence symbol, sequence item and weights.
        /// </summary>
        [Test]
        public void ValidateCompoundNucleotideWithItemList()
        {
            ValidateCompoundSequenceItemWithItemList(Alphabets.DNA,
                Constants.CompoundNucleotideNode,
                AdditionalParameters.Constructor);
        }

        /// <summary>
        /// Creates compound  nucleotide with ctor(byte, char, name, nucleo list, weights)
        /// Add compound nucleotide to sequence.
        /// Validates the sequence symbol, sequence item, weights and byte.
        /// </summary>
        [Test]
        public void ValidateCompoundNucleotideWithItemListandByte()
        {
            ValidateCompoundSequenceItemWithItemListandByte(Alphabets.DNA,
                Constants.CompoundNucleotideNode);
        }

        /// <summary>
        /// Creates compound nucleotide with ctor(char, name, nucleo list, weights, isambgious, isgap).
        /// Adds ambiguous compound nucleotide and other compound nucleotide to sparse sequence.
        /// Validates ambiguous and other compound nucleotide
        /// </summary>
        [Test]
        public void ValidateAmbiguousCompoundNucleotideWithItemList()
        {
            ValidateCompoundSequenceItemListWithAmbiguousChar(Alphabets.DNA,
                Constants.CompoundNucleotideNode);
        }

        /// <summary>
        /// Creates compound nucleotide with ctor(char, name, nucleo list, weights, isambgious, isgap).
        /// Adds gap compound nucleotide and other compound nucleotide to sparse sequence.
        /// Validates gap and other compound nucleotide
        /// </summary>
        [Test]
        public void ValidateGapCompoundNucleotideWithItemList()
        {
            ValidateCompoundSequenceItemListWithGapChar(Alphabets.DNA,
                Constants.CompoundNucleotideNode);
        }

        /// <summary>
        /// Creates compound nucleotide with ctor(byte, char,name,nucleo list, weights, isambgious, isgap) .
        /// Adds ambiguous compound nucleotide and other compound nucleotide to sparse sequence.
        /// Validates ambiguous and other compound nucleotide
        /// </summary>
        [Test]
        public void ValidateAmbiguousCompoundNucleotideWithItemListAndByte()
        {
            ValidateAmbiguousCompoundSequenceItemListAndByte(Alphabets.DNA,
                Constants.CompoundNucleotideNode);
        }

        /// <summary>
        /// Creates the Compound Nucleotide ctor(char,name). Add nucleotides.
        /// Validate the compound nucleotide and sequence items
        /// </summary>
        [Test]
        public void ValidateCompoundNucleotideAdd()
        {
            ValidateCompoundSequenceItem(Alphabets.DNA,
                Constants.CompoundNucleotideNode);
        }

        /// <summary>
        /// Create Compound Nucleotide and add sequence item.
        /// Create the copy of Compound Nucleotide using Clone() method.
        /// Validate the cloned compound nucleotide.
        /// </summary>
        [Test]
        public void ValidateCompoundNucleotideClone()
        {
            ValidateCompoundSequenceItemClone(Alphabets.DNA,
                Constants.CompoundNucleotideNode);
        }

        /// <summary>
        /// Create compound nucleotide and add sequence item with weights
        /// GetWeights() and validate the added weights
        /// Set new weights using SetWeight()
        /// Validate the new set weights.
        /// </summary>
        [Test]
        public void ValidateCompoundNucleotideGetWeightsAndSetWeights()
        {
            ValidateCompoundSequenceItemGetWeightAndSetWeight(Alphabets.DNA,
                Constants.CompoundNucleotideNode);
        }

        /// <summary>
        /// Create Compound Nucleotide and add new sequence item using Add() method.
        /// Remove few sequence item() and validate the compound nucleotide item.
        /// </summary>
        [Test]
        public void ValidateCompoundNucleotideAddandRemove()
        {
            ValidateCompoundSequenceItemAddandRemove(Alphabets.DNA,
                Constants.CompoundNucleotideNode);
        }

        /// <summary>
        /// Creates Compound Nucleotide and searlize it into stream. Deserialize the stream 
        /// and validate the retrieved compound nucleotide.
        /// </summary>
        [Test]
        public void ValidateCompoundNucleotideSerializeAndDeserialize()
        {
            ValidateCompoundSequenceItemSerializeandDeserialize(Alphabets.DNA,
                Constants.CompoundNucleotideNode);
        }

        #endregion

        #region Compound AminoAcid Test Cases

        /// <summary>
        /// Validates the Compound Nucleotide ctor(char, name).
        /// </summary>
        [Test]
        public void ValidateCompoundAminoAcid()
        {
            ValidateCompoundSequenceItem(Alphabets.Protein,
                Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// Creates compound amino acid with ctor(byte, char, name, amino acid list, weights)
        /// Add compound amino acid  to sequence.
        /// Validates the sequence symbol, sequence item, weights and byte.
        /// </summary>
        [Test]
        public void ValidateCompoundAminoAcidWithItemListandByte()
        {
            ValidateCompoundSequenceItemWithItemList(Alphabets.Protein,
                Constants.CompoundAminoAcidNode,
                AdditionalParameters.Constructor);
        }

        /// <summary>
        /// Creates compound amino acid with ctor(byte, char, name, amino acid list, 
        /// weights, isambgious, isgap).
        /// Adds ambiguous compound amino acid and other compound amino acid to sparse sequence.
        /// Validates ambiguous and other compound amino acid
        /// </summary>
        [Test]
        public void ValidateAmbiguousAminoAcidWithItemListAndByte()
        {
            ValidateAmbiguousCompoundSequenceItemListAndByte(Alphabets.Protein,
                Constants.CompoundAminoAcidNode);
        }

        /// <summary>
        /// Validates the Amino acid with ctor(value, char, extSymbol, name).
        /// </summary>
        [Test]
        public void ValidateAminoAcidCtor()
        {
            SerializationInfo info =
                new SerializationInfo(typeof(Sequence),
                    new FormatterConverter());
            StreamingContext context =
                new StreamingContext(StreamingContextStates.All);
            AminoAcid acidObj = new AminoAcid(
                1,
                'A',
                "A",
                "Item");

            ICloneable item = acidObj;
            object objClone = null;
            Assert.AreEqual(false, acidObj.Equals(objClone));

            objClone = item.Clone();
            Assert.AreEqual("A", ((AminoAcid)(objClone)).ExtendedSymbol);
            Assert.AreEqual('A', ((AminoAcid)(objClone)).Symbol);
            Assert.AreEqual(1, ((AminoAcid)(objClone)).Value);
            Assert.AreEqual("Item", ((AminoAcid)(objClone)).Name);
            Assert.AreEqual(false, ((AminoAcid)(objClone)).IsAmbiguous);
            Assert.AreEqual(false, ((AminoAcid)(objClone)).IsGap);
            Assert.AreEqual(false, ((AminoAcid)(objClone)).IsTermination);

            acidObj.GetObjectData(info, context);
            Assert.AreEqual(7, info.MemberCount);

            Console.WriteLine(
                "AminoAcid Bvt: Completed validating Object, Clone and Serialization)");
            ApplicationLog.WriteLine(
                "AminoAcid Bvt: Completed validating Object, Clone and Serialization");
        }

        /// <summary>
        /// Validates Compund Amino acid object data.
        /// </summary>
        [Test]
        public void ValidateCompoundAminoAcidData()
        {
            SerializationInfo info =
                new SerializationInfo(typeof(Sequence),
                    new FormatterConverter());
            StreamingContext context =
                new StreamingContext(StreamingContextStates.All);
            CompoundAminoAcid acidObj = new CompoundAminoAcid(
                1,
                'A',
                "A",
                "Item");

            ICloneable item = acidObj;
            object objClone = null;
            Assert.AreEqual(false, acidObj.Equals(objClone));

            objClone = item.Clone();
            Assert.AreEqual("A", ((AminoAcid)(objClone)).ExtendedSymbol);
            Assert.AreEqual('A', ((AminoAcid)(objClone)).Symbol);
            Assert.AreEqual(1, ((AminoAcid)(objClone)).Value);
            Assert.AreEqual("Item", ((AminoAcid)(objClone)).Name);
            Assert.AreEqual(false, ((AminoAcid)(objClone)).IsAmbiguous);
            Assert.AreEqual(false, ((AminoAcid)(objClone)).IsGap);
            Assert.AreEqual(false, ((AminoAcid)(objClone)).IsTermination);

            ISequenceItem items = acidObj;
            items = items.Clone();
            Assert.AreEqual('A', items.Symbol);
            Assert.AreEqual(1, items.Value);
            Assert.AreEqual("Item", items.Name);
            Assert.AreEqual(false, items.IsAmbiguous);
            Assert.AreEqual(false, items.IsGap);
            Assert.AreEqual(false, items.IsTermination);

            acidObj.GetObjectData(info, context);
            Assert.AreEqual(8, info.MemberCount);

            Console.WriteLine(
                "CompoundAminoAcid Bvt: Completed validating Object, Clone and Serialization)");
            ApplicationLog.WriteLine(
                "CompoundAminoAcid Bvt: Completed validating Object, Clone and Serialization");
        }

        /// <summary>
        /// Validates the Compound Amino Acid ctor
        /// </summary>
        [Test]
        public void ValidateCompoundAminoAcidCtor()
        {
            ValidateCompoundSequenceItemWithItemList(
                Alphabets.Protein,
                Constants.CompoundNucleotideNode,
                AdditionalParameters.Constructor_BCSLL);

            ValidateCompoundSequenceItemWithItemList(
                Alphabets.Protein,
                Constants.CompoundNucleotideNode,
                AdditionalParameters.Constructor_BCSSBBLL);

            ValidateCompoundSequenceItemWithItemList(
                Alphabets.Protein,
                Constants.CompoundNucleotideNode,
                AdditionalParameters.Constructor_BCSSLL);

            ValidateCompoundSequenceItemWithItemList(
                Alphabets.Protein,
                Constants.CompoundNucleotideNode,
                AdditionalParameters.Constructor_CSBBSLL);

            ValidateCompoundSequenceItemWithItemList(
                Alphabets.Protein,
                Constants.CompoundNucleotideNode,
                AdditionalParameters.Constructor_CSSLL);
            ValidateCompoundSequenceItemWithItemList(
                Alphabets.Protein,
                Constants.CompoundNucleotideNode,
                AdditionalParameters.Constructor_CSSBBLL);
        }

        #endregion

        #endregion

        #region Helper Methods

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItem(IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sequence
            Sequence seq = new Sequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights);
            seq.Add(compoundItem);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(seq.ToString(), compoundItem.Symbol.ToString());
            for (int iseq = 0; iseq < compoundItem.SequenceItems.Count; iseq++)
            {
                Assert.AreEqual(sequenceItems[iseq], compoundItem.SequenceItems[iseq].Symbol.ToString());
                Assert.AreEqual(sequenceItemWeights[iseq],
                    compoundItem.GetWeight(compoundItem.SequenceItems[iseq]).ToString());
            }

            Console.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item ctor(byte,name)");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item ctor(byte,name)");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemAddandRemove(IAlphabet alphabet,
            string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
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
            Assert.AreEqual(baseSymbol.ToString(), item.Symbol.ToString());
            int iItem = 0;
            for (int iseq = 0; iseq < item.SequenceItems.Count; iseq++)
            {
                if (iseq >= 1)
                {
                    iItem = iseq + 1;
                }

                Assert.AreEqual(sequenceItems[iItem],
                    item.SequenceItems[iseq].Symbol.ToString());
                Assert.AreEqual(sequenceItemWeights[iItem],
                    item.GetWeight(item.SequenceItems[iseq]).ToString());
            }

            Console.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item Add() and Remove()");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item Add() and Remove()");
        }

        /// <summary>
        /// Creates Compound Sequence Item and searlize it into stream. Deserialize the stream 
        /// and validate the retrieved compound nucleotide.
        /// </summary>
        /// <param name="alphabet">Alphabet</param>
        /// <param name="nodeName">Xml Node based on which the values are read</param>
        private void ValidateCompoundSequenceItemSerializeandDeserialize(IAlphabet alphabet,
            string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sequence
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights);
            Stream stream = File.Open("CompoundItem.data", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, compoundItem);

            stream.Seek(0, SeekOrigin.Begin);
            ICompoundSequenceItem deserializeditem = (ICompoundSequenceItem)formatter.Deserialize(stream);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(baseSymbol, deserializeditem.Symbol.ToString());
            for (int iseq = 0; iseq < deserializeditem.SequenceItems.Count; iseq++)
            {
                Assert.AreEqual(sequenceItems[iseq],
                    deserializeditem.SequenceItems[iseq].Symbol.ToString());
                Assert.AreEqual(sequenceItemWeights[iseq],
                    deserializeditem.GetWeight(deserializeditem.SequenceItems[iseq]).ToString());
            }

            Console.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item Serialize and Deserialize");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item Serialize and Deserialize");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. Add the item to sequence.
        /// Create the cloned compound sequence item using Clone() method.
        /// Validate the compound sequence item.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemClone(IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sequence
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights);

            // Validate the cloned compoundsequenceitem.
            ICompoundSequenceItem item = compoundItem.Clone();
            for (int iseq = 0; iseq < item.SequenceItems.Count; iseq++)
            {
                Assert.AreEqual(sequenceItems[iseq], item.SequenceItems[iseq].Symbol.ToString());
                Assert.AreEqual(sequenceItemWeights[iseq],
                    item.GetWeight(item.SequenceItems[iseq]).ToString());
            }

            Console.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item Clone()");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item Clone()");
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
            string baseSymbolsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
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
            Assert.AreEqual(baseSymbol, spseq[10].Symbol.ToString());
            ICompoundSequenceItem item = spseq[10] as ICompoundSequenceItem;
            int iseqlast = item.SequenceItems.Count - 1;
            for (int iseq = 0; iseq < item.SequenceItems.Count; iseq++)
            {

                Assert.AreEqual(sequenceItems[iseq], item.SequenceItems[iseq].Symbol.ToString());
                Assert.AreEqual(sequenceItemWeights[iseq],
                    item.GetWeight(item.SequenceItems[iseq]).ToString());

                // Set new weight
                item.SetWeight(item.SequenceItems[iseq], byte.Parse(sequenceItemWeights[iseqlast]));
                Assert.AreEqual(sequenceItemWeights[iseqlast],
                   item.GetWeight(item.SequenceItems[iseq]).ToString());
                iseqlast--;
            }

            Console.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item GetWeight() and SetWeight()");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item GetWeight() and SetWeight()");
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
            string baseSymbolsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');

            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            string baseValueString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseValuesNode);
            string[] baseValues = baseValueString.Split(',');
            string baseValue = baseValues[0];

            // Add the item to sequence
            Sequence seq = new Sequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights, baseValue);
            seq.Add(compoundItem);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(seq.ToString(), compoundItem.Symbol.ToString());
            Assert.AreEqual(baseValue, compoundItem.Value.ToString());
            for (int iseq = 0; iseq < compoundItem.SequenceItems.Count; iseq++)
            {
                Assert.AreEqual(sequenceItems[iseq],
                    compoundItem.SequenceItems[iseq].Symbol.ToString());
                Assert.AreEqual(sequenceItemWeights[iseq],
                    compoundItem.GetWeight(compoundItem.SequenceItems[iseq]).ToString());
            }

            Console.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item with byte value");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item with byte value");
        }

        /// <summary>
        /// Create ambiguious compound sequence item using sequence items and weights. Add the item to sparse sequence.
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemWithAmbiguous(IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');

            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
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
                Assert.AreEqual(baseSymbols[iItem], item.Symbol.ToString());
                for (int iseq = 0; iseq < item.SequenceItems.Count; iseq++)
                {
                    Assert.AreEqual(sequenceItems[iseq],
                        item.SequenceItems[iseq].Symbol.ToString());
                    Assert.AreEqual(sequenceItemWeights[iseq],
                        item.GetWeight(item.SequenceItems[iseq]).ToString());
                }
            }

            Console.WriteLine(
                "CompoundSequence Bvt: Completed validating ambiguous compound sequence item");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt: Completed validating ambiguous compound sequence item");
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
            string baseSymbol = "-";

            // Read xml nodes
            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');

            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');

            // Add the item to sequence
            Sequence seq = new Sequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, sequenceItems, sequenceItemWeights, true, false);
            seq.Add(compoundItem);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(seq.ToString(), compoundItem.Symbol.ToString());
            for (int iseq = 0; iseq < compoundItem.SequenceItems.Count; iseq++)
            {
                Assert.AreEqual(sequenceItems[iseq],
                    compoundItem.SequenceItems[iseq].Symbol.ToString());
                Assert.AreEqual(sequenceItemWeights[iseq],
                    compoundItem.GetWeight(compoundItem.SequenceItems[iseq]).ToString());
            }

            Console.WriteLine(
                "CompoundSequence Bvt: Completed validating gap compound sequence item");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt: Completed validating gap compound sequence item");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. 
        /// Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemWithItemList(
            IAlphabet alphabet,
            string nodeName,
            AdditionalParameters method)
        {
            // Read xml nodes
            string baseSymbolsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            List<ISequenceItem> lstSequenceItems = new List<ISequenceItem>();
            for (int iseq = 0; iseq < sequenceItems.Length; iseq++)
            {
                Sequence seq = new Sequence(alphabet, sequenceItems[iseq]);
                lstSequenceItems.Add(seq[0]);
            }

            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');
            List<double> lstWeights = new List<double>();
            for (int iweight = 0; iweight < sequenceItemWeights.Length; iweight++)
            {
                lstWeights.Add(double.Parse(sequenceItemWeights[iweight]));
            }

            // Add the item to sequence
            Sequence sequence = new Sequence(alphabet);
            ICompoundSequenceItem compoundItem = null;

            switch (method)
            {
                case AdditionalParameters.Constructor:
                    compoundItem = CreateCompoundSequenceItem(
                        alphabet, baseSymbol, lstSequenceItems, lstWeights);
                    break;
                default:
                    string baseValueString = Utility._xmlUtil.GetTextValue(
                        nodeName, Constants.BaseValuesNode);
                    string[] baseValues = baseValueString.Split(',');
                    string baseValue = baseValues[0];

                    compoundItem = CreateCompoundSequenceItem(
                    byte.Parse(baseValue),
                    baseSymbol,
                    false,
                    false,
                    lstSequenceItems,
                    lstWeights,
                    method);
                    break;
            }
            sequence.Add(compoundItem);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(sequence.ToString(), compoundItem.Symbol.ToString());
            for (int iseq = 0; iseq < compoundItem.SequenceItems.Count; iseq++)
            {
                Assert.AreEqual(sequenceItems[iseq],
                    compoundItem.SequenceItems[iseq].Symbol.ToString());
                Assert.AreEqual(sequenceItemWeights[iseq],
                    compoundItem.GetWeight(compoundItem.SequenceItems[iseq]).ToString());
            }

            Console.WriteLine(
                "CompoundSequence Bvt : Completed validating compound sequence item with item list");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt : Completed validating compound sequence item with item list");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemWithItemListandByte(IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            List<ISequenceItem> lstSequenceItems = new List<ISequenceItem>();
            for (int iseq = 0; iseq < sequenceItems.Length; iseq++)
            {
                Sequence seq = new Sequence(alphabet, sequenceItems[iseq]);
                lstSequenceItems.Add(seq[0]);
            }

            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');
            List<double> lstWeights = new List<double>();
            for (int iweight = 0; iweight < sequenceItemWeights.Length; iweight++)
            {
                lstWeights.Add(double.Parse(sequenceItemWeights[iweight]));
            }

            string baseValueString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseValuesNode);
            string[] baseValues = baseValueString.Split(',');
            string baseValue = baseValues[0];

            // Add the item to sequence
            Sequence sequence = new Sequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, lstSequenceItems, lstWeights, byte.Parse(baseValue));
            sequence.Add(compoundItem);

            // Validate the compoundsequenceitem and sequence.
            Assert.AreEqual(sequence.ToString(), compoundItem.Symbol.ToString());
            for (int iseq = 0; iseq < compoundItem.SequenceItems.Count; iseq++)
            {
                Assert.AreEqual(sequenceItems[iseq],
                    compoundItem.SequenceItems[iseq].Symbol.ToString());
                Assert.AreEqual(sequenceItemWeights[iseq],
                    (compoundItem as CompoundNucleotide).GetWeight(
                    compoundItem.SequenceItems[iseq]).ToString());
            }

            Console.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item with item list");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt: Completed validating compound sequence item with item list");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateCompoundSequenceItemListWithAmbiguousChar(IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            List<ISequenceItem> lstSequenceItems = new List<ISequenceItem>();
            for (int iseq = 0; iseq < sequenceItems.Length; iseq++)
            {
                Sequence seq = new Sequence(alphabet, sequenceItems[iseq]);
                lstSequenceItems.Add(seq[0]);
            }

            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');
            List<double> lstWeights = new List<double>();
            for (int iweight = 0; iweight < sequenceItemWeights.Length; iweight++)
            {
                lstWeights.Add(double.Parse(sequenceItemWeights[iweight]));
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
                Assert.AreEqual(baseSymbols[iItem], item.Symbol.ToString());
                for (int iseq = 0; iseq < item.SequenceItems.Count; iseq++)
                {
                    Assert.AreEqual(sequenceItems[iseq],
                        item.SequenceItems[iseq].Symbol.ToString());
                    Assert.AreEqual(sequenceItemWeights[iseq],
                        item.GetWeight(item.SequenceItems[iseq]).ToString());
                }
            }

            Console.WriteLine(
                "CompoundSequence Bvt: Completed validating ambiguous compound sequence item with item list");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt: Completed validating ambiguous compound sequence item with item list");
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
            string baseSymbolsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = "-";

            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            List<ISequenceItem> lstSequenceItems = new List<ISequenceItem>();
            for (int iseq = 0; iseq < sequenceItems.Length; iseq++)
            {
                Sequence seq = new Sequence(alphabet, sequenceItems[iseq]);
                lstSequenceItems.Add(seq[0]);
            }

            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');
            List<double> lstWeights = new List<double>();
            for (int iweight = 0; iweight < sequenceItemWeights.Length; iweight++)
            {
                lstWeights.Add(double.Parse(sequenceItemWeights[iweight]));
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
                    Assert.AreEqual(baseSymbols[iItem], item.Symbol.ToString());
                }
                else
                {
                    Assert.AreEqual("-", item.Symbol.ToString());
                }
                for (int iseq = 0; iseq < item.SequenceItems.Count; iseq++)
                {
                    Assert.AreEqual(sequenceItems[iseq], item.SequenceItems[iseq].Symbol.ToString());
                    Assert.AreEqual(sequenceItemWeights[iseq],
                        item.GetWeight(item.SequenceItems[iseq]).ToString());
                }
            }

            Console.WriteLine(
                "CompoundSequence Bvt: Completed validating gap compound sequence item");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt: Completed validating gap compound sequence item");
        }

        /// <summary>
        /// Create a compound sequence item using sequence items and weights. 
        /// Add the item to sequence
        /// Validate the base symbol and sequence item symbol.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        private void ValidateAmbiguousCompoundSequenceItemListAndByte(
            IAlphabet alphabet, string nodeName)
        {
            // Read xml nodes
            string baseSymbolsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseSymbolsNode);
            string[] baseSymbols = baseSymbolsString.Split(',');
            string baseSymbol = baseSymbols[0];

            string sequenceItemsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemSymbolsNode);
            string[] sequenceItems = sequenceItemsString.Split(',');
            List<ISequenceItem> lstSequenceItems = new List<ISequenceItem>();
            for (int iseq = 0; iseq < sequenceItems.Length; iseq++)
            {
                Sequence seq = new Sequence(alphabet, sequenceItems[iseq]);
                lstSequenceItems.Add(seq[0]);
            }

            string sequenceItemWeightsString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SequenceItemWeightsNode);
            string[] sequenceItemWeights = sequenceItemWeightsString.Split(',');
            List<double> lstWeights = new List<double>();
            for (int iweight = 0; iweight < sequenceItemWeights.Length; iweight++)
            {
                lstWeights.Add(double.Parse(sequenceItemWeights[iweight]));
            }

            string baseValueString = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.BaseValuesNode);
            string[] baseValues = baseValueString.Split(',');
            string baseValue = baseValues[0];

            // Add the item to sparse sequence
            SparseSequence spsequence = new SparseSequence(alphabet);
            ICompoundSequenceItem compoundItem = CreateCompoundSequenceItem(
                alphabet, baseSymbol, lstSequenceItems, lstWeights,
                false, true, byte.Parse(baseValue));
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
                Assert.AreEqual(baseSymbols[iItem], item.Symbol.ToString());
                if (0 == iItem)
                {
                    Assert.AreEqual(baseValue, item.Value.ToString());
                }
                for (int iseq = 0; iseq < item.SequenceItems.Count; iseq++)
                {
                    Assert.AreEqual(sequenceItems[iseq],
                        item.SequenceItems[iseq].Symbol.ToString());
                    Assert.AreEqual(sequenceItemWeights[iseq],
                        item.GetWeight(item.SequenceItems[iseq]).ToString());
                }
            }

            Console.WriteLine(
                "CompoundSequence Bvt: Completed validating ambiguous compound sequence item");
            ApplicationLog.WriteLine(
                "CompoundSequence Bvt: Completed validating ambiguous compound sequence item");
        }

        /// <summary>
        /// Create compound nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="baseSymbol">base compound item symbo.l</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <returns>returns compound sequence item.</returns>
        private ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
            string baseSymbol, string[] sequenceItems, string[] weights)
        {
            ICompoundSequenceItem compoundItem = null;

            if (alphabet == Alphabets.DNA || alphabet == Alphabets.RNA)
            {
                compoundItem = new CompoundNucleotide(baseSymbol[0],
                    Constants.CompoundItemName);
                for (int iseq = 0; iseq < sequenceItems.Length; iseq++)
                {
                    compoundItem.Add(new Nucleotide(
                        sequenceItems[iseq][0], String.Format("Item",
                        sequenceItems[iseq].ToString())), double.Parse(weights[iseq]));
                }
            }
            else if (alphabet == Alphabets.Protein)
            {
                compoundItem = new CompoundAminoAcid(baseSymbol[0],
                    Constants.CompoundItemName);
                for (int iseq = 0; iseq < sequenceItems.Length; iseq++)
                {
                    compoundItem.Add(new AminoAcid(
                        sequenceItems[iseq][0], String.Format("Item",
                        sequenceItems[iseq].ToString())), double.Parse(weights[iseq]));
                }
            }

            return compoundItem;
        }

        /// <summary>
        /// Create compound nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="baseSymbol">base compound item symbo.l</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <param name="value">byte value of base symbol.</param>
        /// <returns>returns compound sequence item.</returns>
        private ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
            string baseSymbol, string[] sequenceItems, string[] weights, string value)
        {
            ICompoundSequenceItem compoundItem = null;

            if (alphabet == Alphabets.DNA || alphabet == Alphabets.RNA)
            {
                compoundItem = new CompoundNucleotide(byte.Parse(value),
                    baseSymbol[0], Constants.CompoundItemName);
                for (int iseq = 0; iseq < sequenceItems.Length; iseq++)
                {
                    compoundItem.Add(new Nucleotide(
                        sequenceItems[iseq][0], String.Format("Item",
                        sequenceItems[iseq].ToString())), double.Parse(weights[iseq]));
                }
            }
            else if (alphabet == Alphabets.Protein)
            {
                compoundItem = new CompoundAminoAcid(byte.Parse(value),
                    baseSymbol[0], Constants.CompoundItemName);
                for (int iseq = 0; iseq < sequenceItems.Length; iseq++)
                {
                    compoundItem.Add(new AminoAcid(
                        sequenceItems[iseq][0], String.Format("Item",
                        sequenceItems[iseq].ToString())), double.Parse(weights[iseq]));
                }
            }

            return compoundItem;
        }

        /// <summary>
        /// Create compound nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="baseSymbol">base compound item symbol</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <param name="isGap">true\false base symbol is gap or not</param>
        /// <param name="isAmbiguous">true\false base symbol is ambiguous or not.</param>
        /// <returns>returns compound sequence item.</returns>
        private ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
            string baseSymbol, string[] sequenceItems, string[] weights, bool isGap, bool isAmbiguous)
        {
            ICompoundSequenceItem compoundItem = null;

            if (alphabet == Alphabets.DNA || alphabet == Alphabets.RNA)
            {
                compoundItem = new CompoundNucleotide(baseSymbol[0],
                    Constants.CompoundItemName, isGap, isAmbiguous);
                for (int iseq = 0; iseq < sequenceItems.Length; iseq++)
                {
                    compoundItem.Add(new Nucleotide(
                        sequenceItems[iseq][0], String.Format("Item",
                        sequenceItems[iseq].ToString())), double.Parse(weights[iseq]));
                }
            }
            else if (alphabet == Alphabets.Protein)
            {
                compoundItem = new CompoundAminoAcid(baseSymbol[0],
                    Constants.CompoundItemName, isGap, isAmbiguous);
                for (int iseq = 0; iseq < sequenceItems.Length; iseq++)
                {
                    compoundItem.Add(new AminoAcid(
                        sequenceItems[iseq][0], String.Format("Item",
                        sequenceItems[iseq].ToString())), double.Parse(weights[iseq]));
                }
            }

            return compoundItem;
        }


        /// <summary>
        /// Create compound nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="baseSymbol">base compound item symbo.l</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <returns>returns compound sequence item.</returns>
        private ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
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
        /// Create compound nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="baseSymbol">base compound item symbol</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <param name="value">byte value of base symbol.</param>
        /// <returns>returns compound sequence item.</returns>
        private ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
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
        /// Create compound nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="alphabet">alphabet node.</param>
        /// <param name="baseSymbol">base compound item symbol</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <param name="isGap">true\false base symbol is gap or not</param>
        /// <param name="isAmbiguous">true\false base symbol is ambiguous or not.</param>
        /// <returns>returns compound sequence item.</returns>
        private ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
            string baseSymbol, List<ISequenceItem> sequenceItems, List<double> weights,
            bool isGap, bool isAmbiguous)
        {
            ICompoundSequenceItem compoundItem = null;

            if (alphabet == Alphabets.DNA || alphabet == Alphabets.RNA)
            {
                compoundItem = new CompoundNucleotide(
                    baseSymbol[0], Constants.CompoundItemName, isGap,
                    isAmbiguous, sequenceItems, weights);
            }
            else if (alphabet == Alphabets.Protein)
            {
                compoundItem = new CompoundAminoAcid(
                    baseSymbol[0], Constants.CompoundItemName, isGap,
                    isAmbiguous, sequenceItems, weights);
            }

            return compoundItem;
        }

        /// <summary>
        /// Create compound nucleotide or compound amino acid according to the alphabet.
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
        private ICompoundSequenceItem CreateCompoundSequenceItem(IAlphabet alphabet,
            string baseSymbol, List<ISequenceItem> sequenceItems, List<double> weights,
            bool isGap, bool isAmbiguous, byte value)
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

        /// <summary>
        /// Create compound nucleotide or compound amino acid according to the alphabet.
        /// Add sequence items and weights.
        /// </summary>
        /// <param name="baseSymbol">base compound item symbo.l</param>
        /// <param name="sequenceItems">sequence items.</param>
        /// <param name="weights">weights for each sequence item.</param>
        /// <param name="value">byte value of base symbol.</param>
        /// <param name="isGap">Indicates if this is a gap character.</param>
        /// <param name="isAmbiguous">Indicates if this is ambiguous.</param>
        /// <param name="ctor">Compound Amino Acid ctor parameter</param>
        /// <returns>returns compound sequence item.</returns>
        private ICompoundSequenceItem CreateCompoundSequenceItem(
            byte value,
            string baseSymbol,
            bool isGap,
            bool isAmbiguous,
            List<ISequenceItem> sequenceItems,
            List<double> weights,
            AdditionalParameters ctor)
        {
            ICompoundSequenceItem compoundItem = null;

            switch (ctor)
            {
                case AdditionalParameters.Constructor_BCSLL:
                    compoundItem = new CompoundAminoAcid(
                        value,
                        baseSymbol[0],
                        Constants.CompoundItemName,
                        sequenceItems,
                        weights);
                    break;
                case AdditionalParameters.Constructor_BCSSBBLL:
                    compoundItem = new CompoundAminoAcid(
                        value,
                        baseSymbol[0],
                        baseSymbol,
                        Constants.CompoundItemName,
                        isGap,
                        isAmbiguous,
                        sequenceItems, weights);
                    break;
                case AdditionalParameters.Constructor_BCSSLL:
                    compoundItem = new CompoundAminoAcid(
                        value,
                        baseSymbol[0],
                        baseSymbol,
                        Constants.CompoundItemName,
                        sequenceItems,
                        weights);
                    break;
                case AdditionalParameters.Constructor_CSBBSLL:
                    compoundItem = new CompoundAminoAcid(
                        baseSymbol[0],
                        Constants.CompoundItemName,
                        isGap,
                        isAmbiguous,
                        sequenceItems,
                        weights);
                    break;
                case AdditionalParameters.Constructor_CSSLL:
                    compoundItem = new CompoundAminoAcid(
                        baseSymbol[0],
                        baseSymbol,
                        Constants.CompoundItemName,
                        sequenceItems,
                        weights);
                    break;
                case AdditionalParameters.Constructor_CSSBBLL:
                    compoundItem = new CompoundAminoAcid(
                        baseSymbol[0],
                        baseSymbol,
                        Constants.CompoundItemName,
                        isGap,
                        isAmbiguous,
                        sequenceItems,
                        weights);
                    break;

            }

            return compoundItem;
        }

        #endregion;
    }
}
