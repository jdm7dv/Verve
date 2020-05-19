// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

using MBF.IO.GenBank;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.Tests
{
    /// <summary>
    /// Class to test Feature Item.
    /// </summary>
    [TestClass]
    public class FeatureItemTests
    {

        /// <summary>
        /// Test Feature item.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestFeatureItem()
        {
            ISequence seq;
            ISequence featureSeq = null;
            GenBankParser parser = new GenBankParser();
            string _genBankDataPath = @"TestUtils\GenBank";

            seq = parser.ParseOne(_genBankDataPath + @"\BK000016-tpa.gbk");
            GenBankMetadata metadata = seq.Metadata["GenBank"] as GenBankMetadata;
            #region Test GetSubSequence Method

            featureSeq = metadata.Features.All[0].GetSubSequence(seq);
            int start = metadata.Features.All[0].Location.Start - 1;
            int end = metadata.Features.All[0].Location.End - start;
            Assert.AreEqual(featureSeq.ToString(), seq.Range(start, end).ToString());
            featureSeq = metadata.Features.All[1].GetSubSequence(seq);
            start = metadata.Features.All[1].Location.Start - 1;
            end = metadata.Features.All[1].Location.End - start;
            Assert.AreEqual(featureSeq.ToString(), seq.Range(start, end).ToString());


            seq = new Sequence(Alphabets.DNA, "ACGTAAAGGT");
            Sequence refSeq = new Sequence(Alphabets.DNA, "AAAAATTTT");
            LocationBuilder locbuilder = new LocationBuilder();
            ILocation loc = locbuilder.GetLocation("join(complement(4..8),Ref1:5..7)");
            Assert.AreEqual("join(complement(4..8),Ref1:5..7)", locbuilder.GetLocationString(loc));
            FeatureItem fi = new FeatureItem("Feature1", loc);
            Dictionary<string, ISequence> refSeqs = new Dictionary<string, ISequence>();
            refSeqs.Add("Ref1", refSeq);
            ISequence result = fi.GetSubSequence(seq, refSeqs);
            Assert.AreEqual("ATTTCATT", result.ToString());
            #endregion

            #region Test GetSubFeatures Method
            SequenceFeatures seqFeatures = new SequenceFeatures();
            FeatureItem source = new FeatureItem("Source", "1..1509");
            FeatureItem mRNA = new FeatureItem("mRNA", "join(10..567,789..1320)");
            FeatureItem cds = new FeatureItem("CDS", "join(54..567,789..1254)");
            FeatureItem exon1 = new FeatureItem("Exon", "10..567");
            FeatureItem intron = new FeatureItem("Intron", "568..788");
            FeatureItem exon2 = new FeatureItem("Exon", "789..1320");

            seqFeatures.All.Add(source);
            seqFeatures.All.Add(mRNA);
            seqFeatures.All.Add(cds);
            seqFeatures.All.Add(exon1);
            seqFeatures.All.Add(intron);
            seqFeatures.All.Add(exon2);
            List<FeatureItem> subFeatures = source.GetSubFeatures(seqFeatures);
            Assert.AreEqual(5, subFeatures.Count);
            subFeatures = mRNA.GetSubFeatures(seqFeatures);
            Assert.AreEqual(4, subFeatures.Count);
            subFeatures = cds.GetSubFeatures(seqFeatures);
            Assert.AreEqual(1, subFeatures.Count);
            subFeatures = exon1.GetSubFeatures(seqFeatures);
            Assert.AreEqual(0, subFeatures.Count);
            subFeatures = intron.GetSubFeatures(seqFeatures);
            Assert.AreEqual(0, subFeatures.Count);
            subFeatures = exon2.GetSubFeatures(seqFeatures);
            Assert.AreEqual(0, subFeatures.Count);

            #endregion
        }
    }
}
