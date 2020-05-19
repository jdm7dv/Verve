// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;
using MBF.Util;
using MBF.Util.Logging;

namespace MBF.Test
{
    /// <summary>
    /// Test the DerivedSequence classes.
    /// </summary>
    [TestFixture]
    public class DerivedSequenceTest
    {
        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static DerivedSequenceTest()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.test.log");
            }
        }

        /// <summary>
        /// Test the BasicDerivedSequence class.
        /// </summary>
        [Test]
        public void TestBasicDerivedSequence()
        {
            ISequence seq = new Sequence(Alphabets.DNA, "GATACCCAAGGT");

            Assert.AreEqual("GATAC", seq.Range(0, 5).ToString());
            Assert.AreEqual("CCCAA", seq.Range(4, 5).ToString());
            Assert.AreEqual("TGGAACCCATAG", seq.Reverse.ToString());
            Assert.AreEqual("ACCTTGGGTATC", seq.ReverseComplement.ToString());
            Assert.AreEqual("TGGG", seq.ReverseComplement.Range(4, 4).ToString());
            // zero means ignore length
            Assert.AreEqual("", seq.Range(2, 0).ToString());

            seq = new Sequence(Alphabets.RNA, "GUACGGACCUUUGUG");

            Assert.AreEqual("GGACC", seq.Range(4, 5).ToString());
            Assert.AreEqual("GUGUUUCCAGGCAUG", seq.Reverse.ToString());

            seq = new Sequence(Alphabets.RNA, "GUACAAUCGGG");

            Assert.AreEqual("CCCGAUUGUAC", seq.ReverseComplement.ToString());
            Assert.AreEqual("CG", seq.Reverse.Complement.Range(2, 2).ToString());
            Assert.AreEqual("CAUGUUAGCCC", seq.Complement.ToString());

            seq = new Sequence(Alphabets.Protein, "ARNDCAA");

            Assert.AreEqual("CD", seq.Reverse.Range(2, 2).ToString());
            Assert.AreEqual(3, seq.Reverse.Range(2, -1).IndexOf(Alphabets.Protein.LookupBySymbol('R')));
            Assert.AreEqual(-1, seq.Reverse.IndexOf(Alphabets.Protein.LookupBySymbol('-')));

            string str = "";
            foreach (ISequenceItem item in seq.Reverse)
            {
                str += item.Symbol;
            }
            Assert.AreEqual("AACDNRA", str);

            bool exception = false;
            try
            {
                string s = seq.ReverseComplement.ToString();
            }
            catch (Exception ex)
            {
                exception = true;
                Assert.AreEqual(ex.Message, "BasicDerivedSequence: Complement is only allowed for nucleotide sequences.");
            }
            Assert.IsTrue(exception);
       }

        /// <summary>
        /// Test the DerivedSequence class.
        /// </summary>
        [Test]
        public void TestDerivedSequence()
        {
            Sequence seq = new Sequence(Alphabets.RNA, "ACUGUA");
            DerivedSequence derSeq = new DerivedSequence(seq);
            derSeq.Insert(3, Alphabets.DNA.A);
            derSeq.RemoveAt(2);
            Assert.AreEqual(derSeq.ToString(), "ACAGUA");

            derSeq.Insert(3, Alphabets.DNA.A);
            Assert.AreEqual(derSeq.ToString(), "ACAAGUA");
            derSeq.RemoveAt(2);
            derSeq.RemoveAt(1);
            Assert.AreEqual(derSeq.ToString(), "AAGUA");
            derSeq.Insert(1, 'C');
            Assert.AreEqual(derSeq.ToString(), "ACAGUA");
            derSeq.Replace(0, 'C');
            Assert.AreEqual(derSeq.ToString(), "CCAGUA");
            derSeq.ReplaceRange(1, "UA");
            Assert.AreEqual(derSeq.ToString(), "CUAGUA");
            derSeq.InsertRange(derSeq.Count, "UUAA");
            Assert.AreEqual(derSeq.ToString(), "CUAGUAUUAA");
            derSeq.InsertRange(0, "UUAA");
            Assert.AreEqual(derSeq.ToString(), "UUAACUAGUAUUAA");

            seq = new Sequence(Alphabets.DNA, "ACTGTA");
            derSeq = new DerivedSequence(seq);
            derSeq.RemoveAt(3);
            derSeq.Insert(3, Alphabets.DNA.A);
            Assert.AreEqual(derSeq.ToString(), "ACTATA");
            derSeq.InsertRange(derSeq.Count, "TTTT");
            derSeq.RemoveRange(6, 4);
            Assert.AreEqual(derSeq.ToString(), "ACTATA");
            derSeq.Insert(derSeq.Count, 'T');
            Assert.AreEqual(derSeq.ToString(), "ACTATAT");
            derSeq.Add(Alphabets.DNA.G);
            Assert.AreEqual(derSeq.ToString(), "ACTATATG");
            Assert.AreEqual(0, derSeq.IndexOf(Alphabets.DNA.A));
            Assert.IsTrue(derSeq.Contains(Alphabets.DNA.G));
            Assert.IsTrue(derSeq.Remove(Alphabets.DNA.C));
            Assert.AreEqual(derSeq.ToString(), "ATATATG");
            Assert.IsFalse(derSeq.Contains(Alphabets.DNA.C));
            Assert.IsFalse(derSeq.Remove(Alphabets.DNA.C));
            Assert.AreEqual(derSeq.Source.ToString(), "ACTGTA");
            derSeq.Clear();
            Assert.AreEqual(derSeq.Source.ToString(), derSeq.ToString());
            Assert.AreEqual(derSeq.Source.ToString(), "ACTGTA");

            seq = new Sequence(Alphabets.DNA, "ACTGTA");
            derSeq = new DerivedSequence(seq);
            derSeq.RemoveAt(3);
            derSeq.Insert(3, Alphabets.DNA.A);
            IList<IndexedItem<UpdatedSequenceItem>> updatedItems = derSeq.GetUpdatedItems();
            Assert.AreEqual(updatedItems.Count, 2);
            Assert.AreEqual(updatedItems[0].Index, 3);
            Assert.AreEqual(updatedItems[0].Item.Type, UpdateType.Removed);
            Assert.AreEqual(updatedItems[1].Index, 4);
            Assert.AreEqual(updatedItems[1].Item.Type, UpdateType.Inserted);

            seq = new Sequence(Alphabets.DNA, "ACGTATAT");
            derSeq = new DerivedSequence(seq);
            derSeq.RemoveAt(5);
            derSeq.RemoveAt(4);
            derSeq.RemoveAt(3);
            derSeq.RemoveAt(1);
            Assert.AreEqual(derSeq.ToString(), "AGAT");
            updatedItems = derSeq.GetUpdatedItems();
            Assert.AreEqual(updatedItems.Count, 4);
            Assert.AreEqual(updatedItems[0].Index, 1);
            Assert.AreEqual(updatedItems[0].Item.Type, UpdateType.Removed);
            Assert.AreSame(updatedItems[0].Item.SequenceItem,seq[1]);
            Assert.AreEqual(updatedItems[1].Index, 3);
            Assert.AreEqual(updatedItems[1].Item.Type, UpdateType.Removed);
            Assert.AreSame(updatedItems[1].Item.SequenceItem, seq[3]);
            Assert.AreEqual(updatedItems[2].Index, 4);
            Assert.AreEqual(updatedItems[2].Item.Type, UpdateType.Removed);
            Assert.AreSame(updatedItems[2].Item.SequenceItem, seq[4]);
            Assert.AreEqual(updatedItems[3].Index, 5);
            Assert.AreEqual(updatedItems[3].Item.Type, UpdateType.Removed);
            Assert.AreSame(updatedItems[3].Item.SequenceItem, seq[5]);

            seq = new Sequence(Alphabets.DNA, "ACGTATAT");
            derSeq = new DerivedSequence(seq);
            derSeq.RemoveRange(1, 4);
            Assert.AreEqual(derSeq.ToString(), "ATAT");

            seq = new Sequence(Alphabets.DNA, "ACGTATAT");
            derSeq = new DerivedSequence(seq);
            derSeq.RemoveAt(1);
            derSeq.RemoveAt(2);
            derSeq.RemoveAt(5);
            derSeq.Insert(3, 'A');
            updatedItems = derSeq.GetUpdatedItems();
            Assert.AreEqual(updatedItems.Count, 4);
            Assert.AreEqual(updatedItems.Where(R => R.Item.Type == UpdateType.Removed).Count(), 3);
            Assert.AreEqual(updatedItems.Where(R => R.Item.Type == UpdateType.Inserted).Count(), 1);

            #region Test - IndexOfNonGap and LastIndexOfNonGap
            seq = new Sequence(Alphabets.RNA, "--ACUGUA-");
            derSeq = new DerivedSequence(seq);
            Assert.AreEqual(seq.IndexOfNonGap(), 2);
            Assert.AreEqual(seq.IndexOfNonGap(1), 2);
            Assert.AreEqual(seq.IndexOfNonGap(4), 4);
            Assert.AreEqual(seq.IndexOfNonGap(8), -1);
            Assert.AreEqual(seq.LastIndexOfNonGap(), 7);
            Assert.AreEqual(seq.LastIndexOfNonGap(seq.Count - 1), 7);
            Assert.AreEqual(seq.LastIndexOfNonGap(7), 7);
            Assert.AreEqual(seq.LastIndexOfNonGap(5), 5);

            Assert.AreEqual(seq.IndexOfNonGap(), derSeq.IndexOfNonGap());
            Assert.AreEqual(seq.IndexOfNonGap(1), derSeq.IndexOfNonGap(1));
            Assert.AreEqual(seq.IndexOfNonGap(4), derSeq.IndexOfNonGap(4));
            Assert.AreEqual(seq.IndexOfNonGap(8), derSeq.IndexOfNonGap(8));
            Assert.AreEqual(seq.LastIndexOfNonGap(), derSeq.LastIndexOfNonGap());
            Assert.AreEqual(seq.LastIndexOfNonGap(seq.Count-1), derSeq.LastIndexOfNonGap(derSeq.Count-1));
            Assert.AreEqual(seq.LastIndexOfNonGap(7), derSeq.LastIndexOfNonGap(7));
            Assert.AreEqual(seq.LastIndexOfNonGap(5), derSeq.LastIndexOfNonGap(5));
            #endregion Test - IndexOfNonGap and LastIndexOfNonGap

            seq = new Sequence(Alphabets.RNA, "ACUGUA");
            derSeq = new DerivedSequence(seq);
            derSeq.Insert(3, new Nucleotide('A',"RNACHAR"));
            derSeq.RemoveAt(2);
            Assert.AreEqual(derSeq.ToString(), "ACAGUA");
            CompoundNucleotide cn = new CompoundNucleotide('M',"Compound");
            cn.Add(new Nucleotide('A',"Item A"),30);
            cn.Add(new Nucleotide('C',"Item C"),20);
            derSeq.Insert(3, cn);
            Assert.AreEqual(derSeq[2].Symbol, 'A');
            Assert.AreEqual(derSeq[2].Name, "RNACHAR");
            Assert.AreEqual(derSeq[3].Symbol, 'M');
        }
    }
}
