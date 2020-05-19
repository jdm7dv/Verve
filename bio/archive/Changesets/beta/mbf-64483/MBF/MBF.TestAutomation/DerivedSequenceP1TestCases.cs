// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * DerivedSequenceP1TestCases.cs
 * 
 * This file contains the Derived Sequence P1 test case validation.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MBF.TestAutomation
{
    /// <summary>
    /// P1 test cases to confirm the features of Derived Sequence
    /// </summary>
    [TestClass]
    public class DerivedSequenceP1TestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static DerivedSequenceP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        #region Test Cases

        /// <summary>
        /// Creates a Rna derived sequence after adding and removing few items from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequence()
        {
            ValidateDerivedSequence(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after adding and removing few items from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequence()
        {
            ValidateDerivedSequence(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after inserting and removing few items from original sequence.
        /// Validates its updated items against expected items. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceGetUpdatedItems()
        {
            ValidateDerivedSequenceGetUpdatedItems(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after inserting and removing few items from original sequence.
        /// Validates its updated items against expected updated items. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceGetUpdatedItems()
        {
            ValidateDerivedSequenceGetUpdatedItems(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after removing few items using RemoveAt() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceRemoveAt()
        {
            ValidateDerivedSequenceRemoveAt(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Dna derived sequence after removing few items using Remove() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDnaDerivedSequenceRemove()
        {
            ValidateDerivedSequenceRemove(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after removing few items using RemoveAt() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceRemoveAt()
        {
            ValidateDerivedSequenceRemoveAt(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after removing few items using RemoveRange() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceRemoveRange()
        {
            ValidateDerivedSequenceRemoveRange(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after removing few items using RemoveRange() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceRemoveRange()
        {
            ValidateDerivedSequenceRemoveRange(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after adding few items using Add() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceAdd()
        {
            ValidateDerivedSequenceAdd(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after adding few items using Add() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceAdd()
        {
            ValidateDerivedSequenceAdd(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after inserting few items using Insert(pos,char) from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceInsertWithChar()
        {
            ValidateDerivedSequenceInsertWithChar(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after inserting few items using Insert(pos,char) 
        /// from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceInsertWithChar()
        {
            ValidateDerivedSequenceInsertWithChar(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after inserting few items using Insert(pos,item) 
        /// from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceInsertWithSequenceItem()
        {
            ValidateDerivedSequenceInsertWithSequenceItem(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after inserting few items using Insert(pos,item) 
        /// from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceInsertWithSequenceItem()
        {
            ValidateDerivedSequenceInsertWithSequenceItem(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after inserting a sequence using InsertRange(pos,sequence) 
        /// from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceInsertRange()
        {
            ValidateDerivedSequenceInsertRange(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after inserting a sequence using InsertRange(pos,sequence) 
        /// from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceInsertRange()
        {
            ValidateDerivedSequenceInsertRange(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after adding and removing few items from original sequence.
        /// Create a copy of derived sequence and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceClone()
        {
            ValidateDerivedSequenceClone(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after adding and removing few items from original sequence.
        /// Create a empty array and copy all sequence items derived sequence 
        /// and validates it against expected sequence items. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceCopyTo()
        {
            ValidateDerivedSequenceCopyTo(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after adding and removing few items from original sequence.
        /// Create a empty array and copy all sequence items derived sequence 
        /// and validates it against expected sequence items. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceCopyTo()
        {
            ValidateDerivedSequenceCopyTo(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after adding and removing few items from original sequence.
        /// Get a sub sequence using Range() and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceRange()
        {
            ValidateDerivedSequenceRange(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after adding and removing few items from original sequence.
        /// Get a sub sequence using Range() and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceRange()
        {
            ValidateDerivedSequenceRange(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after adding and removing few items from original sequence.
        /// Replace few items using ReplaceRange() and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceReplaceRange()
        {
            ValidateDerivedSequenceReplaceRange(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after adding and removing few items from original sequence.
        /// Replace few items using ReplaceRange() and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceReplaceRange()
        {
            ValidateDerivedSequenceReplaceRange(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after adding and removing few items from original sequence.
        /// Replace few items using Replace() and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceReplace()
        {
            ValidateDerivedSequenceReplace(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after adding and removing few items from original sequence.
        /// Replace few items using Replace() and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceReplace()
        {
            ValidateDerivedSequenceReplace(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after adding and removing few items from original sequence.
        /// Replace few items by passing char using Replace() and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceReplaceWithChar()
        {
            ValidateDerivedSequenceReplaceWithChar(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after adding and removing few items from original sequence.
        /// Replace few items by passing char using Replace() and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceReplaceWithChar()
        {
            ValidateDerivedSequenceReplaceWithChar(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after adding and removing few items from original sequence.
        /// Clear the derived sequence changes and validated that it matches against orginal sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceClear()
        {
            ValidateDerivedSequenceClear(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after adding and removing few items from original sequence.
        /// Clear the derived sequence changes and validated that it matches against orginal sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceClear()
        {
            ValidateDerivedSequenceClear(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Dna derived sequence after adding and removing few items from original sequence.
        /// Clear the derived sequence changes and validated that it matches against orginal sequence. 
        /// Now perform insert and remove operation and validate derived sequence.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDnaDerivedSequenceClearWithInsertRemove()
        {
            ValidateDerivedSequenceClearWithInsertRemove(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after adding and removing few items from original sequence.
        /// Validates derived sequence index matches expected index of items using IndexOf().
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceIndexOf()
        {
            ValidateDerivedSequenceIndexOf(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after adding and removing few items from original sequence.
        /// Validates derived sequence index matches expected index of items using IndexOf().
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceIndexOf()
        {
            ValidateDerivedSequenceIndexOf(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after adding and removing few items from original sequence.
        /// Validates expected items are present in derived sequence using Contains() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceContains()
        {
            ValidateDerivedSequenceContains(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Protein derived sequence after adding and removing few items from original sequence.
        /// Validates expected items are present in derived sequence using Contains() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateProteinDerivedSequenceContains()
        {
            ValidateDerivedSequenceContains(Constants.ProteinDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a Rna derived sequence after adding and removing few items from original sequence.
        /// Validates items of derived sequence using GeEnumerator()
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceGetEnumerator()
        {
            ValidateDerivedSequenceGetEnumerator(Constants.RnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a derived sequence and validates GetObjectData() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDerivedSequenceGetObjectData()
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                Constants.AlphabetNameNode);

            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence =
                new DerivedSequence(new Sequence(alphabet, expectedSequence));

            SerializationInfo info = new SerializationInfo(typeof(Sequence),
               new FormatterConverter());
            StreamingContext context = new StreamingContext(StreamingContextStates.All);

            // Validate GetObjectData
            derSequence.GetObjectData(info, context);

            // Validate Sequence.
            Assert.AreEqual(expectedSequence, derSequence.ToString());

            Console.WriteLine(@"DerivedSequence P1 TestCases: 
            Validation of GetObjectData() method of derived sequence completed successfully");
            ApplicationLog.WriteLine(@"DerivedSequence P1 TestCases: 
            Validation of GetObjectData() method of derived sequence completed successfully");
        }

        /// <summary>
        /// Validate DerivedSequence IndexOfNonGap().
        /// Input data : Sequence.
        /// Output Data : Validation of IndexOfNonGap() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDerivedSequenceIndexOfNonGap()
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                Constants.AlphabetNameNode);

            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create a DerivedSequence object.
            DerivedSequence derSeqObj = new DerivedSequence(new Sequence(alphabet, expectedSequence));

            int index = derSeqObj.IndexOfNonGap();

            Assert.AreEqual(0, index);
        }

        /// <summary>
        /// Validate DerivedSequence LastIndexOfNonGap().
        /// Input data : Sequence.
        /// Output Data : Validation of LastIndexOfNonGap() method.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDerivedSequenceLastIndexOfNonGap()
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                Constants.AlphabetNameNode);

            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create a BasicDerivedSequence object.
            DerivedSequence derSeqObj =
                new DerivedSequence(new Sequence(alphabet, expectedSequence));

            int index = derSeqObj.LastIndexOfNonGap();

            Assert.AreEqual(expectedSequence.Length - 1, index);
        }

        /// <summary>
        /// Validate DerivedSequence Properties.
        /// Input data : Sequence.
        /// Output Data : Validation of all properties.
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateDerivedSequenceAllProperties()
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(Constants.RnaDerivedSequenceNode,
                Constants.AlphabetNameNode);

            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            Sequence seq = new Sequence(alphabet, expectedSequence);
            seq.MoleculeType = MoleculeType.RNA;

            // Create a BasicDerivedSequence object.
            DerivedSequence derSeqObj = new DerivedSequence(seq);

            // Validate Complement Property
            Assert.IsNotNull(derSeqObj.Complement);

            // Validate Display ID
            Assert.IsNotNull(derSeqObj.DisplayID);

            // Validate Documentation by setting a value
            derSeqObj.Documentation = "Documentation";
            Assert.AreEqual("Documentation", derSeqObj.Documentation);

            // Validate ID property for not null
            Assert.IsNotNull(derSeqObj.ID);

            // Validate Readonly property
            Assert.IsTrue(derSeqObj.IsReadOnly);

            // Validate MoleculeType property
            Assert.AreEqual(MoleculeType.RNA.ToString(), derSeqObj.MoleculeType.ToString());

            // Validate MetaData property
            Dictionary<string, object> metaObj = derSeqObj.Metadata;
            Assert.AreEqual(0, metaObj.Count);

            // Validate Statistics property
            SequenceStatistics stats = derSeqObj.Statistics;
            Assert.IsNotNull(derSeqObj.Statistics);
            Assert.IsTrue(0 != stats.GetCount(seq[0]));
            Assert.IsTrue(0 != stats.GetFraction(seq[0]));

            Console.WriteLine(
                "DerivedSequence P1 TestCases: Validation of all the properties completed successfully.");
            ApplicationLog.WriteLine(
                "DerivedSequence P1 TestCases: Validation of all the properties completed successfully.");

            // Validate serialization.
            using (Stream stream = File.Open("DerivedSequence.data", FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                Sequence seqObj = new Sequence(Alphabets.RNA, "ACUGA");
                DerivedSequence derSeq = new DerivedSequence(seqObj);
                derSeq.RemoveAt(0);
                Assert.AreEqual(derSeq.ToString(), "CUGA");
                derSeq.RemoveAt(2);
                Assert.AreEqual(derSeq.ToString(), "CUA");
                derSeq.Insert(2, Alphabets.RNA.C);
                Assert.AreEqual(derSeq.ToString(), "CUCA");
                derSeq.Insert(2, new Nucleotide('C', "Rna"));
                Assert.AreEqual(derSeq.ToString(), "CUCCA");
                formatter.Serialize(stream, derSeq);
                stream.Seek(0, SeekOrigin.Begin);

                DerivedSequence deserializedDerSeq = (DerivedSequence)formatter.Deserialize(stream);
                Assert.IsNotNull(deserializedDerSeq);
            }
        }

        /// <summary>
        /// Create a Rna derived sequence after adding and removing few items from original sequence.
        /// Create a copy of derived sequence and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(1)]
        public void ValidateRnaDerivedSequenceCloneWithISequenceInterface()
        {
            ValidateDerivedSequenceCloneWithISequence(
                Constants.RnaDerivedSequenceNode);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Creates alphabet derived sequence after adding and removing few items from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabetNode</param>
        private void ValidateDerivedSequence(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RemoveRange);
            string addSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.DerivedSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequenceWithAddRemove(
                alphabet, expectedSequence, addSequence, removeRange);

            // Validate derived Sequence.
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Validates its updated items against expected updated items. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceGetUpdatedItems(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removePosition = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.UpdatedItemRemovePosition);
            string insertParams = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.UpdatedItemInsertParams);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RemoveInsertDerivedSequence);
            string updatedItems = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.UpdatedItemList);
            string updatedIndices = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.UpdatedItemsIndex);
            string updatedTypes = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.UpdatedTypeList);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);
            CreateDerivedSequenceWithRemoveInsert(ref derSequence, insertParams, removePosition);
            string[] updatedIndexList = updatedIndices.Split(',');
            string[] updatedTypesList = updatedTypes.Split(',');
            Assert.AreEqual(updatedTypesList.Length, updatedIndexList.Length);

            // Validate derived Sequence.
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            // Validate GetUpdatedItems
            IList<IndexedItem<UpdatedSequenceItem>> actualUpdatedItemList = derSequence.GetUpdatedItems();
            Assert.AreEqual(updatedIndexList.Length, actualUpdatedItemList.Count);
            for (int index = 0; index < actualUpdatedItemList.Count; index++)
            {
                Assert.AreEqual(updatedIndexList[index],
                    actualUpdatedItemList[index].Index.ToString((IFormatProvider)null));
                Assert.AreEqual(updatedTypesList[index],
                    actualUpdatedItemList[index].Item.Type.ToString());
                Assert.AreEqual(updatedItems[index].ToString(),
                    actualUpdatedItemList[index].Item.SequenceItem.Symbol.ToString((IFormatProvider)null));
            }

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of GetUpdatedItems() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of GetUpdatedItems() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Clear the derived sequence changes and validated that it matches against oroginal sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceClear(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RemoveRange);
            string addSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.DerivedSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequenceWithAddRemove(
                alphabet, expectedSequence, addSequence, removeRange);

            // Validate the changes are present in derived sequence.
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            // clear the changes.
            derSequence.Clear();

            // Validate derived Sequence changes are cleared
            // It matches now with source sequence.
            Assert.AreEqual(expectedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Clear() method of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Clear() method of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Clear the derived sequence changes and validated that it matches against oroginal sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceClearWithInsertRemove(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RemoveRange);
            string addSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.DerivedSequence);
            string removePosition = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.UpdatedItemRemovePosition);
            string insertParams = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.UpdatedItemInsertParams);
            string removeInsertDerivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RemoveInsertDerivedSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequenceWithAddRemove(
                alphabet, expectedSequence, addSequence, removeRange);

            // Validate the changes are present in derived sequence.
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            // clear the changes.
            derSequence.Clear();

            // Validate derived Sequence changes are cleared
            // It matches now with source sequence.
            Assert.AreEqual(expectedSequence, derSequence.ToString());

            CreateDerivedSequenceWithRemoveInsert(ref derSequence, insertParams, removePosition);

            // Validate derived Sequence changes are cleared
            // It matches now with source sequence.
            Assert.AreEqual(removeInsertDerivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Clear() method along with Insert() and Remove() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Clear() method along with Insert() and Remove() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Validates derived sequence index matches expected index of items using IndexOf().
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceIndexOf(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RemoveRange);
            string addSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.DerivedSequence);
            string indexOfSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.IndexOfSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequenceWithAddRemove(
                alphabet, expectedSequence, addSequence, removeRange);

            // Validate IndexOf() derived Sequence.
            Assert.AreEqual(derivedSequence, derSequence.ToString());
            Sequence sequence = new Sequence(alphabet, addSequence);
            string[] indices = indexOfSequence.Split(',');
            int index = 0;
            foreach (ISequenceItem item in sequence)
            {
                int position = derSequence.IndexOf(item);
                Assert.AreEqual(position.ToString((IFormatProvider)null), indices[index]);
                index++;
            }

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of IndexOf() method of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of IndexOf() method of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Validates expected items are present in derived sequence using Contains() method.
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceContains(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RemoveRange);
            string addSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.DerivedSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequenceWithAddRemove(
                alphabet, expectedSequence, addSequence, removeRange);

            // Validate Contains() derived Sequence.
            Assert.AreEqual(derivedSequence, derSequence.ToString());
            Sequence sequence = new Sequence(alphabet, addSequence);
            foreach (ISequenceItem item in sequence)
            {
                Assert.IsTrue(derSequence.Contains(item));
            }

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Contains() method of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Contains() method of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Validates items of derived sequence using GeEnumerator()
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceGetEnumerator(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RemoveRange);
            string addSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AddSequence);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.DerivedSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequenceWithAddRemove(
                alphabet, expectedSequence, addSequence, removeRange);

            // Validate GetEnumerator() derived Sequence.
            Assert.AreEqual(derivedSequence, derSequence.ToString());
            IEnumerator<ISequenceItem> list = derSequence.GetEnumerator();
            int index = 0;
            while (list.MoveNext())
            {
                Assert.AreEqual(list.Current.Symbol, derivedSequence[index]);
                index++;
            }

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of GetEnumerator() method of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of GetEnumerator() method of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after removing few items using RemoveAt() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceRemoveAt(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RemoveRange1);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RemoveDerivedSequence1);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);
            string[] removals = removeRange.Split(',');
            int position = int.Parse(removals[0], null);
            int length = int.Parse(removals[1], null);

            // Remove items
            for (int index = position; index <= length; index++)
            {
                derSequence.RemoveAt(index);
            }

            // Validate Derived Sequence
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of RemoveAt() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of RemoveAt() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after removing few items using Remove() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceRemove(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removeSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RangeSequence);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RemoveDerivedSequence1);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);

            // Remove sequence items
            Sequence removeSeq = new Sequence(alphabet, removeSequence);
            foreach (ISequenceItem seqItem in removeSeq)
            {
                derSequence.Remove(seqItem);
            }

            // Validate Derived Sequence
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Remove() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Remove() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after removing few items using RemoveRange() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceRemoveRange(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RemoveRange1);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RemoveDerivedSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);
            string[] removals = removeRange.Split(',');
            int position = int.Parse(removals[0], null);
            int length = int.Parse(removals[1], null);

            // Remove items
            derSequence.RemoveRange(position, length);

            // Validate Derived Sequence
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            // Code added to hit all the blocks in the Remove Range.
            derSequence.Insert(0, 'a');
            derSequence.Insert(1, 'a');
            derSequence.Insert(2, 'a');
            derSequence.Insert(3, 'a');
            derSequence.RemoveRange(2, 1);

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of RemoveRange() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of RemoveRange() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding few items using Add() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceAdd(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddDerivedSequence);
            string addSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);

            // Add sequence item
            Sequence addSeq = new Sequence(alphabet, addSequence);
            foreach (ISequenceItem item in addSeq)
            {
                derSequence.Add(item);
            }

            // Validate Derived Sequence
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Add() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Add() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after inserting few items using Insert(pos,char) from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceInsertWithChar(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InsertDerivedSequence);
            string insertSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);
            int position = 1;

            // Insert sequence item using symbol
            Sequence insertSeq = new Sequence(alphabet, insertSequence);
            foreach (ISequenceItem item in insertSeq)
            {
                derSequence.Insert(position, item.Symbol);
            }

            // Validate Derived Sequence
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Insert() by passing char of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Insert() by passing char of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after inserting few items using Insert(pos,item) from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceInsertWithSequenceItem(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InsertDerivedSequence);
            string insertSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);
            int position = 1;

            // Insert sequence item
            Sequence insertSeq = new Sequence(alphabet, insertSequence);
            foreach (ISequenceItem item in insertSeq)
            {
                derSequence.Insert(position, item);
            }

            // Validate Derived Sequence
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Insert() by passing item of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Insert() by passing item of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after inserting a sequence using InsertRange(pos,sequence) from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceInsertRange(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.InsertSequence);
            string insertSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);
            int position = 1;

            // Insert sequence item
            derSequence.InsertRange(position, insertSequence);

            // Validate Derived Sequence
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of InsertRange() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of InsertRange() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Create a copy of derived sequence and validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceClone(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RemoveRange);
            string addSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.DerivedSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequenceWithAddRemove(
                alphabet, expectedSequence, addSequence, removeRange);
            DerivedSequence derSequenceCopy = derSequence.Clone();

            // Validate copy of derived Sequence.
            Assert.AreEqual(derSequence.ToString(), derSequenceCopy.ToString());
            Assert.AreEqual(derivedSequence, derSequenceCopy.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Clone() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Clone() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Create a empty array and copy all sequence items derived sequence 
        /// and validates it against expected sequence items. 
        /// </summary>
        /// <param name="nodeName"></param>
        private void ValidateDerivedSequenceCopyTo(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RemoveRange);
            string addSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.DerivedSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequenceWithAddRemove(
                alphabet, expectedSequence, addSequence, removeRange);
            ISequenceItem[] sequenceItems = new ISequenceItem[derSequence.Count];
            derSequence.CopyTo(sequenceItems, 0);

            // Validate copy of derived Sequence.
            int index = 0;
            foreach (ISequenceItem item in sequenceItems)
            {
                Assert.AreEqual(derivedSequence[index], item.Symbol);
                index++;
            }

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Clone() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Clone() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Get a sub sequence using Range() and validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName"></param>
        private void ValidateDerivedSequenceRange(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RemoveRange);
            string addSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);
            string rangeSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RangeSequence);
            string range = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.Range);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequenceWithAddRemove(
                alphabet, expectedSequence, addSequence, removeRange);
            string[] ranges = range.Split(',');
            int position = int.Parse(ranges[0], null);
            int length = int.Parse(ranges[1], null);

            ISequence sequence = derSequence.Range(position, length);

            // Validate range Sequence.
            Assert.AreEqual(rangeSequence, sequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Range() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Range() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Replace few items using ReplaceRange() and validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceReplaceRange(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ReplaceSequence);
            string replaceSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ReplaceRangeSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);
            int position = 1;

            // Replace range of sequence.
            derSequence.ReplaceRange(position, replaceSequence);

            // Validate Derived Sequence
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of ReplaceRange() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of ReplaceRange() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Replace few items using Replace() and validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceReplace(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ReplaceSequence);
            string replaceSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ReplaceRangeSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);
            int position = 1;

            // Replace range of sequence.
            Sequence replaceSeq = new Sequence(alphabet, replaceSequence);
            foreach (ISequenceItem item in replaceSeq)
            {
                derSequence.Replace(position, item);
                position++;
            }

            // Validate Derived Sequence
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Replace() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Replace() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Replace few items by passing char using Replace() and validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceReplaceWithChar(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ReplaceSequence);
            string replaceSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ReplaceRangeSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);
            int position = 1;

            // Replace sequence with char.
            Sequence replaceSeq = new Sequence(alphabet, replaceSequence);
            foreach (ISequenceItem item in replaceSeq)
            {
                derSequence.Replace(position, item.Symbol);
                position++;
            }

            // Validate Derived Sequence
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Replace() by passing char of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Replace() by passing char of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a derived sequence after removing few items and inserting items from original sequence.
        /// </summary>
        /// <param name="derSequence">Derived Sequence</param>
        /// <param name="insertParams">Insert params</param>
        /// <param name="removePosition">Remove items positions</param>
        private static void CreateDerivedSequenceWithRemoveInsert(ref DerivedSequence derSequence,
            string insertParams, string removePosition)
        {
            string[] removals = removePosition.Split(',');
            string[] insertVals = insertParams.Split(',');

            // Remove few elements
            for (int index = 0; index < removals.Length; index++)
            {
                derSequence.RemoveAt(int.Parse(removals[index], null));
            }

            // Insert sequence item by passing char.
            derSequence.Insert(int.Parse(insertVals[0], null), insertVals[1][0]);
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// </summary>
        /// <param name="alphabet">alphabet object</param>
        /// <param name="source">original sequence.</param>
        /// <param name="addSeq">Sequnce to add</param>
        /// <param name="removeString">Remove positions and length</param>
        /// <returns>Derived Sequence</returns>
        private static DerivedSequence CreateDerivedSequenceWithAddRemove(IAlphabet alphabet,
            string source, string addSeq, string removeString)
        {
            Sequence seq = new Sequence(alphabet, source);
            DerivedSequence derSequence = new DerivedSequence(seq);
            string[] removals = removeString.Split(',');

            // Add sequence item
            Sequence addSequence = new Sequence(alphabet, addSeq);
            foreach (ISequenceItem item in addSequence)
            {
                derSequence.Add(item);
            }

            // Remove few elements
            derSequence.RemoveRange(int.Parse(removals[0], null), int.Parse(removals[1], null));

            return derSequence;
        }

        /// <summary>
        /// Creates a derived sequence after adding and removing few items from original sequence.
        /// Create a copy of derived sequence and validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceCloneWithISequence(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.RemoveRange);
            string addSequence = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.AddSequence);

            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            ISequence derSequence = CreateDerivedSequenceWithAddRemove(
                alphabet, expectedSequence, addSequence, removeRange);
            ICloneable derSequenceCloneable = CreateDerivedSequenceWithAddRemove(
                alphabet, expectedSequence, addSequence, removeRange);
            ISequence derSequenceCopy = derSequence.Clone();
            object derSequenceCloneableCopy = derSequenceCloneable.Clone();

            // Validate copy of derived Sequence.
            Assert.AreEqual(derSequenceCopy.ToString(), derSequence.ToString());
            Assert.AreEqual(derSequenceCloneableCopy.ToString(),
                derSequenceCloneable.ToString());

            Console.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Clone() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceP1TestCases:Validation of Clone() of derived sequence completed successfully");
        }

        #endregion
    }
}
