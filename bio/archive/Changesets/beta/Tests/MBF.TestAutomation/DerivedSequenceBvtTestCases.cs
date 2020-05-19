// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * DerivedSequenceBvtTestCases.cs
 * 
 * This file contains the Derived Sequence BVT test case validation.
 * 
******************************************************************************/

using System;
using System.Collections.Generic;

using MBF.TestAutomation.Util;
using MBF.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.TestAutomation
{
    /// <summary>
    /// Bvt test cases to confirm the features of Derived Sequence
    /// </summary>
    [TestClass]
    public class DerivedSequenceBvtTestCases
    {

        #region Global Variables

        Utility _utilityObj = new Utility(@"TestUtils\TestsConfig.xml");

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static DerivedSequenceBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion

        #region Test Cases

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequence()
        {
            ValidateDerivedSequence(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Validates its updated items against expected updated items. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaGetUpdatedItems()
        {
            ValidateDerivedSequenceGetUpdatedItems(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after removing few items using 
        /// RemoveAt() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceRemoveAt()
        {
            ValidateDerivedSequenceRemoveAt(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after removing few items using 
        /// RemoveRange() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceRemoveRange()
        {
            ValidateDerivedSequenceRemoveRange(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding few items using 
        /// Add() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceAdd()
        {
            ValidateDerivedSequenceAdd(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after inserting few items using 
        /// Insert(pos,char) from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceInsertWithChar()
        {
            ValidateDerivedSequenceInsertWithChar(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after inserting few items using 
        /// Insert(pos,item) from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceInsertWithSequenceItem()
        {
            ValidateDerivedSequenceInsertWithSequenceItem(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after inserting a sequence using 
        /// InsertRange(pos,sequence) from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceInsertRange()
        {
            ValidateDerivedSequenceInsertRange(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Create a copy of derived sequence and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceClone()
        {
            ValidateDerivedSequenceClone(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Create a empty array and copy all sequence items derived sequence 
        /// and validates it against expected sequence items. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceCopyTo()
        {
            ValidateDerivedSequenceCopyTo(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Get a sub sequence using Range() and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceRange()
        {
            ValidateDerivedSequenceRange(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Replace few items using ReplaceRange() and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceReplaceRange()
        {
            ValidateDerivedSequenceReplaceRange(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Replace few items using Replace() and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceReplace()
        {
            ValidateDerivedSequenceReplace(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Replace few items by passing char using Replace() and validates it against expected sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceReplaceWithChar()
        {
            ValidateDerivedSequenceReplaceWithChar(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Clear the derived sequence changes and validated that it matches against oroginal sequence. 
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceClear()
        {
            ValidateDerivedSequenceClear(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Validates derived sequence index matches expected index of items using IndexOf().
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceIndexOf()
        {
            ValidateDerivedSequenceIndexOf(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Validates expected items are present in derived sequence using Contains() method.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceContains()
        {
            ValidateDerivedSequenceContains(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Validates items of derived sequence using GeEnumerator()
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceGetEnumerator()
        {
            ValidateDerivedSequenceGetEnumerator(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Validates properties of derived sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDnaDerivedSequenceProperties()
        {
            ValidateDerivedSequenceProperties(Constants.DnaDerivedSequenceNode);
        }

        /// <summary>
        /// Validates FindMatches() method with valid values.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateDerivedSequenceFindMatches()
        {
            ISequence seq = new Sequence(Alphabets.DNA, "ACCGGTT");
            BasicDerivedSequence bdsObj =
                new BasicDerivedSequence(seq, false, false, 0, seq.Count - 1);
            IList<string> patterns = new List<string>();
            patterns.Add("ACCG");
            IDictionary<string, IList<int>> actual = bdsObj.FindMatches(patterns, 0, false);
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(0, actual["ACCG"][0]);

            Console.WriteLine("Derived Sequence : Successfully validated FindMatches() method.");
            ApplicationLog.WriteLine("Derived Sequence : Successfully validated FindMatches() method.");
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
            DerivedSequence derSequence = CreateDerivedSequence(
                alphabet, expectedSequence, addSequence, removeRange);

            // Validate derived Sequence.
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Clear the derived sequence changes and validated that it matches against oroginal sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceClear(string nodeName)
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
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequence(
                alphabet, expectedSequence, addSequence, removeRange);

            // clear the changes.
            derSequence.Clear();

            // Validate derived Sequence changes are cleared
            // It matches now with source sequence.
            Assert.AreEqual(expectedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Clear() method of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Clear() method of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Validates derived sequence index matches expected index of items using IndexOf().
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceIndexOf(string nodeName)
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
            string indexOfSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.IndexOfSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequence(
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
                "DerivedSequenceBvtTestCases:Validation of IndexOf() method of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of IndexOf() method of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Validates expected items are present in derived sequence using Contains() method.
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceContains(string nodeName)
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
            DerivedSequence derSequence = CreateDerivedSequence(
                alphabet, expectedSequence, addSequence, removeRange);

            // Validate Contains() derived Sequence.
            Assert.AreEqual(derivedSequence, derSequence.ToString());
            Sequence sequence = new Sequence(alphabet, addSequence);
            foreach (ISequenceItem item in sequence)
            {
                Assert.IsTrue(derSequence.Contains(item));
            }

            Console.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Contains() method of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Contains() method of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
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
            DerivedSequence derSequence = CreateDerivedSequence(
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
                "DerivedSequenceBvtTestCases:Validation of GetEnumerator() method of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of GetEnumerator() method of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Validates properties of derived sequence.
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceProperties(string nodeName)
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
            string complement = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Complement);
            string reverse = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.Reverse);
            string reverseComplement = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ReverseComplement);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequence(
                alphabet, expectedSequence, addSequence, removeRange);

            // Validate properties of derived Sequence.
            Assert.AreEqual(derivedSequence, derSequence.ToString());
            Assert.AreEqual(alphabet, derSequence.Alphabet);
            Assert.AreEqual(complement, derSequence.Complement.ToString());
            Assert.AreEqual(reverse, derSequence.Reverse.ToString());
            Assert.AreEqual(reverseComplement, derSequence.ReverseComplement.ToString());
            Assert.IsFalse(derSequence.IsReadOnly);
            Assert.AreEqual(expectedSequence, derSequence.Source.ToString());
            Assert.AreEqual(MoleculeType.Invalid, derSequence.MoleculeType);

            Console.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of properties of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of properties of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Validates its updated items against expected updated items. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceGetUpdatedItems(string nodeName)
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
            string updatedItems = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.UpdatedItemList);
            string updatedIndices = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.UpdatedItemsIndex);
            string updatedTypes = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.UpdatedTypeList);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequence(
                alphabet, expectedSequence, addSequence, removeRange);
            string[] updatedIndexList = updatedIndices.Split(',');
            string[] updatedTypesList = updatedTypes.Split(',');
            Assert.AreEqual(updatedTypesList.Length, updatedIndexList.Length);

            // Validate derived Sequence.
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            // Validate GetUpdatedItems
            IList<IndexedItem<UpdatedSequenceItem>> actualUpdatedItemList =
                derSequence.GetUpdatedItems();
            Assert.AreEqual(updatedIndexList.Length, actualUpdatedItemList.Count);
            for (int index = 0; index < actualUpdatedItemList.Count; index++)
            {
                Assert.AreEqual(updatedIndexList[index],
                    actualUpdatedItemList[index].Index.ToString((IFormatProvider)null));
                Assert.AreEqual(updatedItems[index].ToString(),
                    actualUpdatedItemList[index].Item.SequenceItem.Symbol.ToString((IFormatProvider)null));
                Assert.AreEqual(updatedTypesList[index],
                    actualUpdatedItemList[index].Item.Type.ToString());
            }

            Console.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of GetUpdatedItems() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of GetUpdatedItems() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after removing few items using RemoveAt() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceRemoveAt(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RemoveRange1);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RemoveDerivedSequence2);
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
                "DerivedSequenceBvtTestCases:Validation of RemoveAt() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of RemoveAt() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after removing few items using 
        /// RemoveRange() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceRemoveRange(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string removeRange = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RemoveRange1);
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

            Console.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of RemoveRange() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of RemoveRange() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding few items using Add() from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceAdd(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AddDerivedSequence);
            string addSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AddSequence);
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
                "DerivedSequenceBvtTestCases:Validation of Add() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Add() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after inserting few items using Insert(pos,char) from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceInsertWithChar(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InsertDerivedSequence);
            string insertSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AddSequence);
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
                "DerivedSequenceBvtTestCases:Validation of Insert() by passing char of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Insert() by passing char of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after inserting few items using Insert(pos,item) from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceInsertWithSequenceItem(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InsertDerivedSequence);
            string insertSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AddSequence);
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
                "DerivedSequenceBvtTestCases:Validation of Insert() by passing item of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Insert() by passing item of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after inserting a sequence using InsertRange(pos,sequence) from original sequence.
        /// Validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceInsertRange(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.InsertSequence);
            string insertSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AddSequence);
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
                "DerivedSequenceBvtTestCases:Validation of InsertRange() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of InsertRange() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Create a copy of derived sequence and validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceClone(string nodeName)
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
            DerivedSequence derSequence = CreateDerivedSequence(
                alphabet, expectedSequence, addSequence, removeRange);
            DerivedSequence derSequenceCopy = derSequence.Clone();


            // Validate copy of derived Sequence.
            Assert.AreEqual(derSequence.ToString(), derSequenceCopy.ToString());
            Assert.AreEqual(derivedSequence, derSequenceCopy.ToString());

            Console.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Clone() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Clone() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Create a empty array and copy all sequence items derived sequence 
        /// and validates it against expected sequence items. 
        /// </summary>
        /// <param name="nodeName"></param>
        private void ValidateDerivedSequenceCopyTo(string nodeName)
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
            DerivedSequence derSequence = CreateDerivedSequence(
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
                "DerivedSequenceBvtTestCases:Validation of Clone() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Clone() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Get a sub sequence using Range() and validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName"></param>
        private void ValidateDerivedSequenceRange(string nodeName)
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
            string rangeSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.RangeSequence);
            string range = _utilityObj._xmlUtil.GetTextValue(nodeName, Constants.Range);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence
            DerivedSequence derSequence = CreateDerivedSequence(
                alphabet, expectedSequence, addSequence, removeRange);
            string[] ranges = range.Split(',');
            int position = int.Parse(ranges[0], null);
            int length = int.Parse(ranges[1], null);

            ISequence sequence = derSequence.Range(position, length);

            // Validate range Sequence.
            Assert.AreEqual(rangeSequence, sequence.ToString());

            Console.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Range() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Range() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Replace few items using ReplaceRange() and validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceReplaceRange(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ReplaceSequence);
            string replaceSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ReplaceRangeSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);
            int position = 0;

            // Replace range of sequence.
            derSequence.ReplaceRange(position, replaceSequence);

            // Validate Derived Sequence
            Assert.AreEqual(derivedSequence, derSequence.ToString());

            Console.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of ReplaceRange() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of ReplaceRange() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Replace few items using Replace() and validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceReplace(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ReplaceSequence);
            string replaceSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ReplaceRangeSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);
            int position = 0;

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
                "DerivedSequenceBvtTestCases:Validation of Replace() of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Replace() of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// Replace few items by passing char using Replace() and validates it against expected sequence. 
        /// </summary>
        /// <param name="nodeName">alphabet xml node.</param>
        private void ValidateDerivedSequenceReplaceWithChar(string nodeName)
        {
            // Get input and expected values from xml
            string expectedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string alphabetName = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string derivedSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ReplaceSequence);
            string replaceSequence = _utilityObj._xmlUtil.GetTextValue(
                nodeName, Constants.ReplaceRangeSequence);
            IAlphabet alphabet = Utility.GetAlphabet(alphabetName);

            // Create derived Sequence.
            Sequence seq = new Sequence(alphabet, expectedSequence);
            DerivedSequence derSequence = new DerivedSequence(seq);
            int position = 0;

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
                "DerivedSequenceBvtTestCases:Validation of Replace() by passing char of derived sequence completed successfully");
            ApplicationLog.WriteLine(
                "DerivedSequenceBvtTestCases:Validation of Replace() by passing char of derived sequence completed successfully");
        }

        /// <summary>
        /// Creates a dna derived sequence after adding and removing few items from original sequence.
        /// </summary>
        /// <param name="source"></param>
        private static DerivedSequence CreateDerivedSequence(
            IAlphabet alphabet, string source, string addSeq, string removeString)
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
            derSequence.RemoveRange(int.Parse(removals[0], null),
                int.Parse(removals[1], null));

            return derSequence;
        }

        #endregion
    }
}
