// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * GenBankFeaturesP1TestCases.cs
 * 
 *   This file contains the GenBank Features P1 test cases.
 * 
***************************************************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using MBF.IO;
using MBF.IO.GenBank;
using MBF.TestAutomation.Util;
using MBF.Util.Logging;

using NUnit.Framework;

namespace MBF.TestAutomation.IO.GenBank
{
    /// <summary>
    /// GenBank Features P1 test case implementation.
    /// </summary>
    [TestFixture]
    public class GenBankFeaturesP1TestCases
    {

        #region Enums

        /// <summary>
        /// GenBank Feature parameters which are used for different test cases 
        /// based on which the test cases are executed.
        /// </summary>
        enum FeatureGroup
        {
            CDS,
            Exon,
            Intron,
            mRNA,
            Enhancer,
            Promoter,
            miscDifference,
            variation,
            MiscStructure,
            TrnsitPeptide,
            StemLoop,
            ModifiedBase,
            PrecursorRNA,
            PolySite,
            MiscBinding,
            GCSignal,
            LTR,
            Operon,
            UnsureSequenceRegion,
            NonCodingRNA,
            Default
        };

        /// <summary>
        /// GenBank Feature location operators used for different test cases.
        /// </summary>
        enum FeatureOperator
        {
            Join,
            Complement,
            Order,
            Default
        };

        #endregion Enums

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static GenBankFeaturesP1TestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }

            Utility._xmlUtil =
                new XmlUtility(@"TestUtils\GenBankFeaturesTestConfig.xml");
        }

        #endregion Constructor

        #region GenBank P1 TestCases

        /// <summary>
        /// Parse a valid medium size DNA GenBank file.
        /// and validate GenBank features.
        /// Input : DNA medium size Sequence
        /// Output : Validate GenBank features.
        /// </summary>
        [Test]
        public void ValidateGenBankFeaturesForMediumSizeDnaSequence()
        {
            ValidateGenBankFeatures(Constants.MediumSizeDNAGenBankFeaturesNode,
                "DNA");
        }

        /// <summary>
        /// Parse a valid medium size Protein GenBank file.
        /// and validate GenBank features.
        /// Input : Protein medium size Sequence
        /// Output : Validate GenBank features.
        /// </summary>
        [Test]
        public void ValidateGenBankFeaturesForMediumSizeProteinSequence()
        {
            ValidateGenBankFeatures(Constants.MediumSizePROTEINGenBankFeaturesNode,
                "Protein");
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank features.
        /// Input : RNA medium size Sequence
        /// Output : Validate GenBank features.
        /// </summary>
        [Test]
        public void ValidateGenBankFeaturesForMediumSizeRnaSequence()
        {
            ValidateGenBankFeatures(Constants.MediumSizeRNAGenBankFeaturesNode,
                "RNA");
        }

        /// <summary>
        /// Parse a valid medium size DNA GenBank file.
        /// and validate cloned GenBank features.
        /// Input : DNA medium size Sequence
        /// Output : alidate cloned GenBank features.
        /// </summary>
        [Test]
        public void ValidateClonedGenBankFeaturesForMediumSizeDnaSequence()
        {
            ValidateCloneGenBankFeatures(Constants.MediumSizeDNAGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a valid medium size Protein GenBank file.
        /// and validate cloned GenBank features.
        /// Input : Protein medium size Sequence
        /// Output : validate cloned GenBank features.
        /// </summary>
        [Test]
        public void ValidateClonedGenBankFeaturesForMediumSizeProteinSequence()
        {
            ValidateCloneGenBankFeatures(Constants.MediumSizePROTEINGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank features.
        /// Input : RNA medium size Sequence
        /// Output : Validate GenBank features.
        /// </summary>
        [Test]
        public void ValidateClonedGenBankFeaturesForMediumSizeRnaSequence()
        {
            ValidateCloneGenBankFeatures(Constants.MediumSizeRNAGenBankFeaturesNode);

        }

        /// <summary>
        /// Parse a valid medium size DNA GenBank file.
        /// and validate GenBank DNA sequence standard features.
        /// Input : Valid DNA sequence.
        /// Output : Validation of GenBank standard Features 
        /// </summary>
        [Test]
        public void ValidateMediumSizeDnaSequenceStandardFeatures()
        {
            ValidateGenBankStandardFeatures(Constants.MediumSizeDNAGenBankFeaturesNode,
               "DNA");
        }

        /// <summary>
        /// Parse a valid medium size Protein GenBank file.
        /// and validate GenBank Protein seq standard features.
        /// Input : Valid Protein sequence.
        /// Output : Validation of GenBank standard Features 
        /// </summary>
        [Test]
        public void ValidateMediumSizeProteinSequenceStandardFeatures()
        {
            ValidateGenBankStandardFeatures(Constants.MediumSizePROTEINGenBankFeaturesNode,
                "Protein");
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank RNA seq standard features.
        /// Input : Valid RNA sequence.
        /// Output : Validation of GenBank standard Features 
        /// </summary>
        [Test]
        public void ValidateMediumSizeRNaSequenceStandardFeatures()
        {
            ValidateGenBankStandardFeatures(Constants.MediumSizeRNAGenBankFeaturesNode,
                "RNA");
        }

        /// <summary>
        /// Parse a valid multiSequence Protein GenBank file.
        /// validate GenBank Features.
        /// Input : MultiSequence GenBank Protein file.
        /// Validation : Validate GenBank Features.
        /// </summary>
        [Test]
        public void ValidateGenBankFeaturesForMultipleProteinSequence()
        {
            ValidateGenBankFeatures(Constants.MultiSeqGenBankProteinNode,
                null);
        }

        /// <summary>
        /// Parse a valid multiSequence RNA GenBank file.
        /// validate GenBank Features.
        /// Input : MultiSequence GenBank RNA file.
        /// Validation : Validate GenBank Features.
        /// </summary>
        [Test]
        public void ValidateGenBankFeaturesForMultipleRnaSequence()
        {
            ValidateGenBankFeatures(Constants.MulitSequenceGenBankRNANode,
                "RNA");
        }

        /// <summary>
        /// Parse a valid medium size Protein GenBank file.
        /// and validate GenBank features.
        /// Input : Protein medium size Sequence
        /// Output : Validate of GenBank features with Binary formatter.
        /// </summary>
        [Test]
        public void ValidateProteinGenBankFeaturesWithBinaryFormatter()
        {
            ValidateGenBankFeaturesWithBinaryFormatter(
                Constants.MediumSizePROTEINGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank features.
        /// Input : RNA medium size Sequence
        /// Output : Validate of GenBank features with Binary formatter.
        /// </summary>
        [Test]
        public void ValidateRnaGenBankFeaturesWithBinaryFormatter()
        {
            ValidateGenBankFeaturesWithBinaryFormatter(
                Constants.MediumSizeRNAGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank Gene feature qualifiers
        /// Input : RNA medium size Sequence
        /// Output : Validate of GenBank Gene feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankGeneFeatureQualifiers()
        {
            ValidateGenBankGeneFeatureQualifiers(
                Constants.MediumSizeRNAGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a valid DNA GenBank file.
        /// and validate GenBank Gene feature qualifiers
        /// Input : DNA medium size Sequence
        /// Output : Validate of GenBank tRNA feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBanktRNAFeatureQualifiers()
        {
            ValidateGenBanktRNAFeatureQualifiers(
                Constants.DNAStandardFeaturesKeyNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank Gene feature qualifiers
        /// Input : RNA medium size Sequence
        /// Output : Validate of GenBank tRNA feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBanktRNAFeatureQualifiers()
        {
            ValidateGenBanktRNAFeatureQualifiers(
                Constants.MediumSizeRNAGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a valid DNA GenBank file.
        /// and validate GenBank Gene feature qualifiers
        /// Input : DNA medium size Sequence
        /// Output : Validate of GenBank Gene feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankGeneFeatureQualifiers()
        {
            ValidateGenBankGeneFeatureQualifiers(
                Constants.DNAStandardFeaturesKeyNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank Gene feature qualifiers
        /// Input : RNA medium size Sequence
        /// Output : Validate of GenBank mRNA feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankmRNAFeatureQualifiers()
        {
            ValidateGenBankmRNAFeatureQualifiers(
                Constants.MediumSizeRNAGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a valid DNA GenBank file.
        /// and validate GenBank Gene feature qualifiers
        /// Input : DNA medium size Sequence
        /// Output : Validate of GenBank mRNA feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankmRNAFeatureQualifiers()
        {
            ValidateGenBankmRNAFeatureQualifiers(
                Constants.DNAStandardFeaturesKeyNode);
        }

        /// <summary>
        /// Parse a valid Protein GenBank file.
        /// and validate GenBank Gene feature qualifiers
        /// Input : Protein medium size Sequence
        /// Output : Validate of GenBank mRNA feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateProteinSequenceGenBankmRNAFeatureQualifiers()
        {
            ValidateGenBankmRNAFeatureQualifiers(
                Constants.ProteinGenBankVariationNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate addition of GenBank features.
        /// Input : RNA medium size Sequence
        /// Output : validate addition of GenBank features.
        /// </summary>
        [Test]
        public void ValidateAdditionGenBankFeatures()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FilePathNode);
            string addFirstKey = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FirstKey);
            string addSecondKey = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.SecondKey);
            string addFirstLocation = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FirstLocation);
            string addSecondLocation = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.SecondLocation);

            // Parse a GenBank file.
            LocationBuilder locBuilder = new LocationBuilder();
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);

            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];

            // Add a new features to Genbank features list.
            metadata.Features = new SequenceFeatures();
            FeatureItem feature = new FeatureItem(addFirstKey, addFirstLocation);
            metadata.Features.All.Add(feature);
            feature = new FeatureItem(addSecondKey, addSecondLocation);
            metadata.Features.All.Add(feature);

            // Validate added GenBank features.
            Assert.AreEqual(metadata.Features.All[0].Key.ToString(), addFirstKey);
            Assert.AreEqual(locBuilder.GetLocationString(metadata.Features.All[0].Location),
                addFirstLocation);
            Assert.AreEqual(metadata.Features.All[1].Key.ToString(), addSecondKey);
            Assert.AreEqual(locBuilder.GetLocationString(metadata.Features.All[1].Location),
                addSecondLocation);

            //Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the new feature '{0}'",
                metadata.Features.All[0].Key.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the new location '{0}'",
                metadata.Features.All[0].Location.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the new feature '{0}'",
                metadata.Features.All[1].Key.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the new location '{0}'",
                metadata.Features.All[1].Location.ToString()));
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate addition of GenBank qualifiers.
        /// Input : RNA medium size Sequence
        /// Output : validate addition of GenBank qualifiers.
        /// </summary>
        [Test]
        public void ValidateAdditionGenBankQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FilePathNode);
            string addFirstKey = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FirstKey);
            string addSecondKey = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.SecondKey);
            string addFirstLocation = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FirstLocation);
            string addSecondLocation = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.SecondLocation);
            string addFirstQualifier = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FirstQualifier);
            string addSecondQualifier = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.SecondQualifier);

            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];

            // Add a new features to Genbank features list.
            metadata.Features = new SequenceFeatures();
            FeatureItem feature = new FeatureItem(addFirstKey,
                addFirstLocation);
            List<string> qualifierValues = new List<string>();
            qualifierValues.Add(addFirstQualifier);
            qualifierValues.Add(addFirstQualifier);
            feature.Qualifiers.Add(addFirstQualifier, qualifierValues);
            metadata.Features.All.Add(feature);

            feature = new FeatureItem(addSecondKey, addSecondLocation);
            qualifierValues = new List<string>();
            qualifierValues.Add(addSecondQualifier);
            qualifierValues.Add(addSecondQualifier);
            feature.Qualifiers.Add(addSecondQualifier, qualifierValues);
            metadata.Features.All.Add(feature);

            // Validate added GenBank features.
            Assert.AreEqual(metadata.Features.All[0].Key.ToString(),
                addFirstKey);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.All[0].Location),
                addFirstLocation);
            Assert.AreEqual(metadata.Features.All[1].Key.ToString(),
                addSecondKey);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.All[1].Location),
                addSecondLocation);

            //Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the new feature '{0}'",
                metadata.Features.All[0].Key.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the new location '{0}'",
                locBuilder.GetLocationString(metadata.Features.All[0].Location)));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the new feature '{0}'",
                metadata.Features.All[1].Key.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the new location '{0}'",
                locBuilder.GetLocationString(metadata.Features.All[1].Location)));
        }

        /// <summary>
        /// Parse a valid medium size Protein GenBank file.
        /// and Validate CDS Qualifiers
        /// Input : Protein medium size Sequence
        /// Output : validate CDS Qualifiers.
        /// </summary>
        [Test]
        public void ValidateMediumSizePROTEINSequenceCDSQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankProNodeName, Constants.FilePathNode);
            string expectedCDSException = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankProNodeName, Constants.CDSException);
            string expectedCDSLabel = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankProNodeName, Constants.CDSLabel);
            string expectedCDSDBReference = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankProNodeName, Constants.CDSDBReference);
            string expectedGeneSymbol = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankProNodeName, Constants.GeneSymbol);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            ISequence sequence = parserObj.ParseOne(filePath);

            GenBankMetadata metadata =
                sequence.Metadata[Constants.GenBank] as GenBankMetadata;

            // Get CDS qaulifier.value.
            List<CodingSequence> cdsQualifiers = metadata.Features.CodingSequences;
            List<string> dbReferenceValue = cdsQualifiers[0].DatabaseCrossReference;
            Assert.AreEqual(cdsQualifiers[0].Label, expectedCDSLabel);
            Assert.AreEqual(cdsQualifiers[0].Exception.ToString(), expectedCDSException);
            Assert.IsEmpty(cdsQualifiers[0].Allele);
            Assert.IsEmpty(cdsQualifiers[0].Citation);
            Assert.AreEqual(dbReferenceValue[0].ToString(), expectedCDSDBReference);
            Assert.AreEqual(cdsQualifiers[0].GeneSymbol, expectedGeneSymbol);
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the CDS Qualifiers '{0}'",
                cdsQualifiers[0].Label));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the CDS Qualifiers '{0}'",
                dbReferenceValue[0].ToString()));
        }

        /// <summary>
        /// Parse a valid GenBank file.
        /// and Validate Clearing feature list
        /// Input : Dna medium size Sequence
        /// Output : validate clear() featre list.
        /// </summary>
        [Test]
        public void ValidateRemoveFeatureItem()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankDnaNodeName, Constants.FilePathNode);
            string allFeaturesCount = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankDnaNodeName, Constants.GenBankFeaturesCount);

            // Parse a file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> sequenceList = parserObj.Parse(filePath);

            // GenBank metadata.
            GenBankMetadata metadata =
                sequenceList[0].Metadata[Constants.GenBank] as GenBankMetadata;

            // Validate GenBank features before removing feature item.
            Assert.AreEqual(metadata.Features.All.Count,
                Convert.ToInt32(allFeaturesCount));
            IList<FeatureItem> featureList = metadata.Features.All;

            // Remove feature items from feature list.
            featureList.Clear();

            // Validate feature list after clearing featureList.
            Assert.AreEqual(featureList.Count, 0);

            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validatedremoving GenBank features '{0}'",
                featureList.Count));
        }

        /// <summary>
        /// Parse a Protein valid GenBank file.
        /// and Validate Clearing feature list
        /// Input : Protein medium size Sequence
        /// Output : validate clear() featre list.
        /// </summary>
        [Test]
        public void ValidateRemoveFeatureItemForProteinSequence()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankProNodeName, Constants.FilePathNode);
            string allFeaturesCount = Utility._xmlUtil.GetTextValue(
                Constants.SimpleGenBankProNodeName, Constants.GenBankFeaturesCount);

            // Parse a file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> sequenceList = parserObj.Parse(filePath);

            // GenBank metadata.
            GenBankMetadata metadata =
                sequenceList[0].Metadata[Constants.GenBank] as GenBankMetadata;

            // Validate GenBank features before removing feature item.
            Assert.AreEqual(metadata.Features.All.Count, Convert.ToInt32(allFeaturesCount));
            IList<FeatureItem> featureList = metadata.Features.All;

            // Remove feature items from feature list.
            featureList.Clear();

            // Validate feature list after clearing featureList.
            Assert.AreEqual(featureList.Count, 0);

            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validatedremoving GenBank features '{0}'",
                featureList.Count));
        }

        /// <summary>
        /// Parse a Valid medium size Dna Sequence and Validate Features 
        /// within specified range.
        /// Input : Valid medium size Dna Sequence and specified range.
        /// Ouput : Validate features within specified range.
        /// </summary>
        [Test]
        public void ValidateFeaturesWithinRangeForMediumSizeDnaSequence()
        {
            ValidateGetFeatures(Constants.DNAStandardFeaturesKeyNode);
        }

        /// <summary>
        /// Parse a Valid medium size Rna Sequence and Validate Features 
        /// within specified range.
        /// Input : Valid medium size Rna Sequence and specified range.
        /// Ouput : Validate features within specified range.
        /// </summary>
        [Test]
        public void ValidateFeaturesWithinRangeForMediumSizeRnaSequence()
        {
            ValidateGetFeatures(Constants.MediumSizeRNAGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a Valid medium size Protein Sequence and Validate Features 
        /// within specified range.
        /// Input : Valid medium size Protein Sequence and specified range.
        /// Ouput : Validate features within specified range.
        /// </summary>
        [Test]
        public void ValidateFeaturesWithinRangeForMediumSizeProteinSequence()
        {
            ValidateGetFeatures(Constants.MediumSizeRNAGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a Valid DNA Sequence and validate citation referenced
        /// present in CDS GenBank Feature.
        /// Input : Valid DNA Sequence 
        /// Ouput : Validation of citation referneced for CDS feature.
        /// </summary>
        [Test]
        public void ValidateCitationReferencedForCDSFeature()
        {
            ValidateCitationReferenced(
                Constants.DNAStandardFeaturesKeyNode, FeatureGroup.CDS);
        }

        /// <summary>
        /// Parse a Valid DNA Sequence and validate citation referenced
        /// present in mRNA GenBank Feature.
        /// Input : Valid DNA Sequence 
        /// Ouput : Validation of citation referneced for mRNA feature.
        /// </summary>
        [Test]
        public void ValidateCitationReferencedFormRNAFeature()
        {
            ValidateCitationReferenced(
                Constants.DNAStandardFeaturesKeyNode, FeatureGroup.mRNA);
        }

        /// <summary>
        /// Parse a Valid DNA Sequence and validate citation referenced
        /// present in Exon GenBank Feature.
        /// Input : Valid DNA Sequence 
        /// Ouput : Validation of citation referneced for Exon feature.
        /// </summary>
        [Test]
        public void ValidateCitationReferencedForhExonFeature()
        {
            ValidateCitationReferenced(
                Constants.DNAStandardFeaturesKeyNode, FeatureGroup.Exon);
        }

        /// <summary>
        /// Parse a Valid DNA Sequence and validate citation referenced
        /// present in Intron GenBank Feature.
        /// Input : Valid DNA Sequence 
        /// Ouput : Validation of citation referneced for Intron feature.
        /// </summary>
        [Test]
        public void ValidateCitationReferencedForIntronFeature()
        {
            ValidateCitationReferenced(
                Constants.DNAStandardFeaturesKeyNode, FeatureGroup.Intron);
        }

        /// <summary>
        /// Parse a Valid DNA Sequence and validate citation referenced
        /// present in Promoter GenBank Feature.
        /// Input : Valid DNA Sequence 
        /// Ouput : Validation of citation referneced for Promoter feature.
        /// </summary>
        [Test]
        public void ValidateCitationReferencedForEnhancerFeature()
        {
            ValidateCitationReferenced(
                Constants.DNAStandardFeaturesKeyNode, FeatureGroup.Promoter);
        }

        /// <summary>
        /// Parse a valid medium size multiSequence RNA GenBank file.
        /// validate GenBank Features.
        /// Input : Medium size MultiSequence GenBank RNA file.
        /// Validation : Validate GenBank Features.
        /// </summary>
        [Test]
        public void ValidateGenBankFeaturesForMediumSizeMultipleRnaSequence()
        {
            ValidateGenBankFeatures(Constants.MediumSizeMulitSequenceGenBankRNANode,
                "RNA");
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate addition of single GenBank feature.
        /// Input : RNA medium size Sequence
        /// Output : validate addition of single GenBank  features.
        /// </summary>
        [Test]
        public void ValidateAdditionSingleGenBankFeature()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FilePathNode);
            string addFirstKey = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FirstKey);
            string addFirstLocation = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FirstLocation);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];

            // Add a new features to Genbank features list.
            metadata.Features = new SequenceFeatures();
            FeatureItem feature = new FeatureItem(addFirstKey, addFirstLocation);
            metadata.Features.All.Add(feature);

            // Validate added GenBank features.
            Assert.AreEqual(metadata.Features.All[0].Key.ToString(),
                addFirstKey);
            Assert.AreEqual(
                locBuilder.GetLocationString(metadata.Features.All[0].Location),
                addFirstLocation);

            //Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully added the new feature '{0}'",
                metadata.Features.All[0].Key.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully added the new location '{0}'",
                locBuilder.GetLocationString(metadata.Features.All[0].Location)));
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate addition of single GenBank qualifier.
        /// Input : RNA medium size Sequence
        /// Output : validate addition of single GenBank qualifiers.
        /// </summary>
        [Test]
        public void ValidateAdditionSingleGenBankQualifier()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FilePathNode);
            string addFirstKey = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FirstKey);
            string addFirstLocation = Utility._xmlUtil.GetTextValue(
                Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FirstLocation);
            string addFirstQualifier = Utility._xmlUtil.GetTextValue(
            Constants.MediumSizeRNAGenBankFeaturesNode, Constants.FirstQualifier);
            string addSecondQualifier = Utility._xmlUtil.GetTextValue(
            Constants.MediumSizeRNAGenBankFeaturesNode, Constants.SecondQualifier);

            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];

            // Add a new features to Genbank features list.
            metadata.Features = new SequenceFeatures();
            FeatureItem feature = new FeatureItem(addFirstKey, addFirstLocation);
            List<string> qualifierValues = new List<string>();
            qualifierValues.Add(addFirstQualifier);
            qualifierValues.Add(addFirstQualifier);
            feature.Qualifiers.Add(addFirstQualifier, qualifierValues);
            metadata.Features.All.Add(feature);

            qualifierValues = new List<string>();
            qualifierValues.Add(addSecondQualifier);
            qualifierValues.Add(addSecondQualifier);
            feature.Qualifiers.Add(addSecondQualifier, qualifierValues);
            metadata.Features.All.Add(feature);

            // Validate added GenBank features.
            Assert.AreEqual(
                metadata.Features.All[0].Key.ToString(), addFirstKey);
            Assert.AreEqual(
                locBuilder.GetLocationString(metadata.Features.All[0].Location),
                addFirstLocation);

            //Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully added the new feature '{0}'",
                metadata.Features.All[0].Key.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully added the new location '{0}'",
                locBuilder.GetLocationString(metadata.Features.All[0].Location)));
        }

        /// <summary>
        /// Parse a valid DNA GenBank file.
        /// and validate GenBank Misc feature qualifiers
        /// Input : DNA medium size Sequence
        /// Output : Validation of GenBank Misc feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankMiscFeatureQualifiers()
        {
            ValidateGenBankMiscFeatureQualifiers(
                Constants.DNAStandardFeaturesKeyNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank Misc feature qualifiers
        /// Input : RNA medium size Sequence
        /// Output : Validation of GenBank Misc feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankMiscFeatureQualifiers()
        {
            ValidateGenBankMiscFeatureQualifiers(
                Constants.MediumSizeRNAGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a valid DNA GenBank file.
        /// and validate GenBank Exon feature qualifiers
        /// Input : DNA medium size Sequence
        /// Output : Validation of GenBank Exon feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankExonFeatureQualifiers()
        {
            ValidateGenBankExonFeatureQualifiers(
                Constants.DNAStandardFeaturesKeyNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank Exon feature qualifiers
        /// Input : RNA medium size Sequence
        /// Output : Validation of GenBank Exon feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankExonFeatureQualifiers()
        {
            ValidateGenBankExonFeatureQualifiers(
                Constants.MediumSizeRNAGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a valid DNA GenBank file.
        /// and validate GenBank Intron feature qualifiers
        /// Input : DNA medium size Sequence
        /// Output : Validation of GenBank Intron feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankIntronFeatureQualifiers()
        {
            ValidateGenBankIntronFeatureQualifiers(
                Constants.DNAStandardFeaturesKeyNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank Intron feature qualifiers
        /// Input : RNA medium size Sequence
        /// Output : Validation of GenBank Intron feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankIntronFeatureQualifiers()
        {
            ValidateGenBankIntronFeatureQualifiers(
                Constants.MediumSizeRNAGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a valid DNA GenBank file.
        /// and validate GenBank Promoter feature qualifiers
        /// Input : DNA medium size Sequence
        /// Output : Validation of GenBank Promoter feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankPromoterFeatureQualifiers()
        {
            ValidateGenBankPromoterFeatureQualifiers(
                Constants.DNAStandardFeaturesKeyNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank Promoter feature qualifiers
        /// Input : RNA medium size Sequence
        /// Output : Validation of GenBank Promoter feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankPromoterFeatureQualifiers()
        {
            ValidateGenBankPromoterFeatureQualifiers(
                Constants.MediumSizeRNAGenBankFeaturesNode);
        }

        /// <summary>
        /// Parse a valid DNA GenBank file.
        /// and validate GenBank Variation feature qualifiers
        /// Input : DNA medium size Sequence
        /// Output : Validation of GenBank Variation feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankVariationFeatureQualifiers()
        {
            ValidateGenBankVariationFeatureQualifiers(
                Constants.DNAGenBankVariationNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank Variation feature qualifiers
        /// Input : RNA medium size Sequence
        /// Output : Validation of GenBank Variation feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankVariationFeatureQualifiers()
        {
            ValidateGenBankVariationFeatureQualifiers(
                Constants.RNAGenBankVariationNode);
        }

        /// <summary>
        /// Parse a valid medium size Protein GenBank file.
        /// and validate GenBank Variation feature qualifiers
        /// Input : Protein medium size Sequence
        /// Output : Validation of GenBank Variation feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateProteinSequenceGenBankVariationFeatureQualifiers()
        {
            ValidateGenBankVariationFeatureQualifiers(
                Constants.ProteinGenBankVariationNode);
        }

        /// <summary>
        /// Parse a valid DNA GenBank file.
        /// and validate GenBank Misc Difference feature qualifiers
        /// Input : DNA medium size Sequence
        /// Output : Validation of GenBank Misc Difference feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankMiscDiffFeatureQualifiers()
        {
            ValidateGenBankMiscDiffFeatureQualifiers(
                Constants.DNAGenBankVariationNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank Misc Difference feature qualifiers
        /// Input : RNA medium size Sequence
        /// Output : Validation of GenBank Misc Difference feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankMiscDiffFeatureQualifiers()
        {
            ValidateGenBankMiscDiffFeatureQualifiers(
                Constants.RNAGenBankVariationNode);
        }

        /// <summary>
        /// Parse a valid medium size Protein GenBank file.
        /// and validate GenBank Misc Difference feature qualifiers
        /// Input : Protein medium size Sequence
        /// Output : Validation of GenBank Misc Difference feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateProteinSequenceGenBankMiscDiffFeatureQualifiers()
        {
            ValidateGenBankMiscDiffFeatureQualifiers(
                Constants.ProteinGenBankVariationNode);
        }

        /// <summary>
        /// Parse a valid DNA GenBank file.
        /// and validate GenBank Protein Binding feature qualifiers
        /// Input : DNA medium size Sequence
        /// Output : Validation of GenBank Protein Binding feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankProteinBindingFeatureQualifiers()
        {
            ValidateGenBankProteinBindingFeatureQualifiers(
                Constants.DNAGenBankVariationNode);
        }

        /// <summary>
        /// Parse a valid medium size RNA GenBank file.
        /// and validate GenBank Protein Binding feature qualifiers
        /// Input : RNA medium size Sequence
        /// Output : Validation of GenBank Protein Binding feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankProteinBindingFeatureQualifiers()
        {
            ValidateGenBankProteinBindingFeatureQualifiers(
                Constants.RNAGenBankVariationNode);
        }

        /// <summary>
        /// Parse a valid medium size Protein GenBank file.
        /// and validate GenBank Protein Binding feature qualifiers
        /// Input : Protein medium size Sequence
        /// Output : Validation of GenBank Protein Binding feature qualifiers.
        /// </summary>
        [Test]
        public void ValidateProteinSequenceGenBankProteinBindingFeatureQualifiers()
        {
            ValidateGenBankProteinBindingFeatureQualifiers(
                Constants.ProteinGenBankVariationNode);
        }

        /// <summary>
        /// Parse a valid Dna GenBank file.
        /// and validate GenBank Exon feature clonning.
        /// Input : Dna medium size Sequence
        /// Output : validate GenBank Exon feature clonning.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankExonFeatureClonning()
        {
            ValidateGenBankFeaturesClonning(Constants.DNAStandardFeaturesKeyNode,
                FeatureGroup.Exon);
        }

        /// <summary>
        /// Parse a valid Rna GenBank file.
        /// and validate GenBank Exon feature clonning.
        /// Input : Rna medium size Sequence
        /// Output : validate GenBank Exon feature clonning.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankExonFeatureClonning()
        {
            ValidateGenBankFeaturesClonning(Constants.MediumSizeRNAGenBankFeaturesNode,
               FeatureGroup.Exon);
        }

        /// <summary>
        /// Parse a valid Protein GenBank file.
        /// and validate GenBank Exon feature clonning.
        /// Input : Protein medium size Sequence
        /// Output : validate GenBank Exon feature clonning.
        /// </summary>
        [Test]
        public void ValidateProteinSequenceGenBankExonFeatureClonning()
        {
            ValidateGenBankFeaturesClonning(Constants.ProteinGenBankVariationNode,
               FeatureGroup.Exon);
        }

        /// <summary>
        /// Parse a valid Dna GenBank file.
        /// and validate GenBank Misc Difference feature clonning.
        /// Input : Dna Sequence
        /// Output : validate GenBank Misc Difference feature clonning.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankMiscDiffFeatureClonning()
        {
            ValidateGenBankFeaturesClonning(Constants.DNAGenBankVariationNode,
                FeatureGroup.miscDifference);
        }

        /// <summary>
        /// Parse a valid Rna GenBank file.
        /// and validate GenBank Misc Difference feature clonning.
        /// Input : Rna Sequence
        /// Output : validate GenBank Misc Difference feature clonning.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankMiscDiffFeatureClonning()
        {
            ValidateGenBankFeaturesClonning(Constants.RNAGenBankVariationNode,
                FeatureGroup.miscDifference);
        }

        /// <summary>
        /// Parse a valid Protein GenBank file.
        /// and validate GenBank Misc Difference feature clonning.
        /// Input : Protein Sequence
        /// Output : Validate GenBank Misc Difference feature clonning.
        /// </summary>
        [Test]
        public void ValidateProteinSequenceGenBankMiscDiffFeatureClonning()
        {
            ValidateGenBankFeaturesClonning(Constants.ProteinGenBankVariationNode,
                FeatureGroup.miscDifference);
        }

        /// <summary>
        /// Parse a valid Dna GenBank file.
        /// and validate GenBank Intron feature clonning.
        /// Input : Dna Sequence
        /// Output : validate GenBank Intron feature clonning.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankIntronFeatureClonning()
        {
            ValidateGenBankFeaturesClonning(Constants.DNAStandardFeaturesKeyNode,
                FeatureGroup.Intron);
        }

        /// <summary>
        /// Parse a valid Rna GenBank file.
        /// and validate GenBank Intron feature clonning.
        /// Input : Rna Sequence
        /// Output : validate GenBank Intron feature clonning.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankIntronFeatureClonning()
        {
            ValidateGenBankFeaturesClonning(Constants.MediumSizeRNAGenBankFeaturesNode,
                FeatureGroup.Intron);
        }

        /// <summary>
        /// Parse a valid Protein GenBank file.
        /// and validate GenBank Intron feature clonning.
        /// Input : Protein Sequence
        /// Output : Validate GenBank Intron feature clonning.
        /// </summary>
        [Test]
        public void ValidateProteinSequenceGenBankIntronFeatureClonning()
        {
            ValidateGenBankFeaturesClonning(Constants.ProteinGenBankVariationNode,
                FeatureGroup.Intron);
        }

        /// <summary>
        /// Parse a valid Dna GenBank file.
        /// and validate GenBank Variation feature clonning.
        /// Input : Dna Sequence
        /// Output : validate GenBank Variation feature clonning.
        /// </summary>
        [Test]
        public void ValidateDnaSequenceGenBankVariationFeatureClonning()
        {
            ValidateGenBankFeaturesClonning(Constants.DNAGenBankVariationNode,
                FeatureGroup.variation);
        }

        /// <summary>
        /// Parse a valid Rna GenBank file.
        /// and validate GenBank Variation feature clonning.
        /// Input : Rna Sequence
        /// Output : validate GenBank Variation feature clonning.
        /// </summary>
        [Test]
        public void ValidateRnaSequenceGenBankVariationFeatureClonning()
        {
            ValidateGenBankFeaturesClonning(Constants.RNAGenBankVariationNode,
                FeatureGroup.variation);
        }

        /// <summary>
        /// Parse a valid Protein GenBank file.
        /// and validate GenBank Variation feature clonning.
        /// Input : Protein Sequence
        /// Output : Validate GenBank Variation feature clonning.
        /// </summary>
        [Test]
        public void ValidateProteinSequenceGenBankVariationFeatureClonning()
        {
            ValidateGenBankFeaturesClonning(Constants.ProteinGenBankVariationNode,
                FeatureGroup.variation);
        }

        /// <summary>
        /// Validate GenBank MaturePeptide feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank MaturePeptide feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankMPeptideFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.ProteinGenBankMPeptideQualifiersNode,
                Constants.FilePathNode);
            string mPeptideCount = Utility._xmlUtil.GetTextValue(
                Constants.ProteinGenBankMPeptideQualifiersNode,
                Constants.MiscPeptideCount);
            string geneSymbol = Utility._xmlUtil.GetTextValue(
                Constants.ProteinGenBankMPeptideQualifiersNode,
                Constants.GeneSymbol);
            string mPeptideLocation = Utility._xmlUtil.GetTextValue(
                Constants.ProteinGenBankMPeptideQualifiersNode,
                Constants.Location);


            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate MaturePeptide feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<MaturePeptide> mPeptideList = metadata.Features.MaturePeptides;

            // Create a clone and validate all qualifiers.
            MaturePeptide clonemPeptide = mPeptideList[0].Clone();
            Assert.AreEqual(mPeptideList.Count.ToString(), mPeptideCount);
            Assert.AreEqual(clonemPeptide.GeneSymbol.ToString(), geneSymbol);
            Assert.IsEmpty(clonemPeptide.DatabaseCrossReference);
            Assert.IsEmpty(clonemPeptide.Allele);
            Assert.IsEmpty(clonemPeptide.Citation);
            Assert.IsEmpty(clonemPeptide.Experiment);
            Assert.IsEmpty(clonemPeptide.Function);
            Assert.IsEmpty(clonemPeptide.GeneSynonym);
            Assert.IsEmpty(clonemPeptide.GenomicMapPosition);
            Assert.IsEmpty(clonemPeptide.Inference);
            Assert.IsEmpty(clonemPeptide.Label);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.MaturePeptides[0].Location), mPeptideLocation);
            Assert.IsEmpty(clonemPeptide.Note);
            Assert.IsFalse(mPeptideList[0].Pseudo);
            Assert.IsEmpty(mPeptideList[0].OldLocusTag);
            Assert.IsEmpty(mPeptideList[0].EnzymeCommissionNumber);
            Assert.IsFalse(mPeptideList[0].Pseudo);
            Assert.IsEmpty(mPeptideList[0].StandardName);
            Assert.IsEmpty(mPeptideList[0].LocusTag);
            Assert.IsEmpty(mPeptideList[0].OldLocusTag);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the MaturePeptide feature '{0}'",
                mPeptideList[0].GeneSymbol.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the MaturePeptide feature '{0}'",
                mPeptideList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Attenuator feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank Attenuator feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankAttenuatorFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankAttenuatorQualifiers, Constants.FilePathNode);
            string attenuatorLocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankAttenuatorQualifiers, Constants.Location);
            string featureCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankAttenuatorQualifiers, Constants.QualifierCount);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate Attenuator feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<Attenuator> attenuatorList = metadata.Features.Attenuators;

            // Create a clone of attenuator feature.
            Attenuator attenuatorClone = attenuatorList[0].Clone();
            Assert.AreEqual(attenuatorList.Count.ToString(), featureCount);
            Assert.IsEmpty(attenuatorList[0].GeneSymbol);
            Assert.IsEmpty(attenuatorClone.DatabaseCrossReference);
            Assert.IsEmpty(attenuatorClone.Allele);
            Assert.IsEmpty(attenuatorList[0].Citation);
            Assert.IsEmpty(attenuatorClone.Experiment);
            Assert.IsEmpty(attenuatorList[0].GenomicMapPosition);
            Assert.IsEmpty(attenuatorList[0].GeneSynonym);
            Assert.IsEmpty(attenuatorList[0].Inference);
            Assert.IsEmpty(attenuatorList[0].Label);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.Attenuators[0].Location), attenuatorLocation);
            Assert.IsEmpty(attenuatorList[0].Note);
            Assert.IsEmpty(attenuatorList[0].Operon);
            Assert.IsEmpty(attenuatorList[0].OldLocusTag);
            Assert.IsEmpty(attenuatorList[0].Phenotype);
            Assert.IsEmpty(attenuatorList[0].LocusTag);
            Assert.IsEmpty(attenuatorList[0].OldLocusTag);

            // Create a new Attenuator and validate the same.
            Attenuator attenuator = new Attenuator(attenuatorLocation);
            Attenuator attenuatorWithILoc = new Attenuator(
                metadata.Features.Attenuators[0].Location);

            // Set qualifiers and validate them.
            attenuator.Allele = attenuatorLocation;
            attenuator.GeneSymbol = string.Empty;
            attenuatorWithILoc.GenomicMapPosition = string.Empty;
            Assert.IsEmpty(attenuator.GeneSymbol);
            Assert.AreEqual(attenuator.Allele, attenuatorLocation);
            Assert.IsEmpty(attenuatorWithILoc.GenomicMapPosition);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the attenuator feature '{0}'",
                metadata.Features.Attenuators[0].Location));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the attenuator feature '{0}'",
                attenuatorList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Minus35Signal feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank Minus35Signal feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankMinus35SignalFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMinus35SignalNode, Constants.FilePathNode);
            string minus35Location = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMinus35SignalNode, Constants.Location);
            string featureCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMinus35SignalNode, Constants.QualifierCount);
            string geneSymbol = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMinus35SignalNode, Constants.GeneSymbol);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate Minus35Signal feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<Minus35Signal> minus35Signal = metadata.Features.Minus35Signals;

            // Create a clone of Minus35Signal feature feature.
            Minus35Signal cloneMinus35Signal = minus35Signal[0].Clone();
            Assert.AreEqual(minus35Signal.Count.ToString(), featureCount);
            Assert.AreEqual(cloneMinus35Signal.GeneSymbol, geneSymbol);
            Assert.IsEmpty(cloneMinus35Signal.DatabaseCrossReference);
            Assert.IsEmpty(minus35Signal[0].Allele);
            Assert.IsEmpty(minus35Signal[0].Citation);
            Assert.IsEmpty(minus35Signal[0].Experiment);
            Assert.IsEmpty(minus35Signal[0].GenomicMapPosition);
            Assert.IsEmpty(minus35Signal[0].GeneSynonym);
            Assert.IsEmpty(minus35Signal[0].Inference);
            Assert.IsEmpty(minus35Signal[0].Label);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.Minus35Signals[0].Location), minus35Location);
            Assert.IsEmpty(minus35Signal[0].Note);
            Assert.IsEmpty(minus35Signal[0].Operon);
            Assert.IsEmpty(minus35Signal[0].OldLocusTag);
            Assert.IsEmpty(minus35Signal[0].StandardName);
            Assert.IsEmpty(minus35Signal[0].LocusTag);
            Assert.IsEmpty(minus35Signal[0].OldLocusTag);
            Assert.IsEmpty(minus35Signal[0].StandardName);

            // Create a new Minus35Signal and validate the same.
            Minus10Signal minus35 = new Minus10Signal(minus35Location);
            Minus10Signal minus35WithILoc = new Minus10Signal(
                metadata.Features.Minus35Signals[0].Location);

            // Set qualifiers and validate them.
            minus35.GeneSymbol = geneSymbol;
            minus35WithILoc.GeneSymbol = geneSymbol;
            Assert.AreEqual(minus35.GeneSymbol, geneSymbol);
            Assert.AreEqual(minus35WithILoc.GeneSymbol, geneSymbol);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Minus35Signalfeature feature '{0}'",
                metadata.Features.Minus35Signals[0].Location));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Minus35Signalfeature feature '{0}'",
                minus35Signal.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Minus10Signal feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank Minus10Signal feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankMinus10SignalFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMInus10SignalNode, Constants.FilePathNode);
            string minus10Location = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMInus10SignalNode, Constants.Location);
            string featureCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMInus10SignalNode, Constants.QualifierCount);
            string geneSymbol = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMInus10SignalNode, Constants.GeneSymbol);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate Minus10Signal feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<Minus10Signal> minus10Signal = metadata.Features.Minus10Signals;

            // Create a clone of Minus10Signalfeature feature.
            Minus10Signal cloneMinus10Signal = minus10Signal[0].Clone();
            Assert.AreEqual(minus10Signal.Count.ToString(), featureCount);
            Assert.AreEqual(cloneMinus10Signal.GeneSymbol, geneSymbol);
            Assert.IsEmpty(cloneMinus10Signal.DatabaseCrossReference);
            Assert.IsEmpty(minus10Signal[0].Allele);
            Assert.IsEmpty(minus10Signal[0].Citation);
            Assert.IsEmpty(minus10Signal[0].Experiment);
            Assert.IsEmpty(minus10Signal[0].GenomicMapPosition);
            Assert.IsEmpty(minus10Signal[0].GeneSynonym);
            Assert.IsEmpty(minus10Signal[0].Inference);
            Assert.IsEmpty(minus10Signal[0].Label);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.Minus10Signals[0].Location), minus10Location);
            Assert.IsEmpty(minus10Signal[0].Note);
            Assert.IsEmpty(minus10Signal[0].Operon);
            Assert.IsEmpty(minus10Signal[0].OldLocusTag);
            Assert.IsEmpty(minus10Signal[0].StandardName);
            Assert.IsEmpty(minus10Signal[0].LocusTag);
            Assert.IsEmpty(minus10Signal[0].OldLocusTag);
            Assert.IsEmpty(minus10Signal[0].StandardName);

            // Create a new Minus10Signal and validate the same.
            Minus10Signal minus10 = new Minus10Signal(minus10Location);
            Minus10Signal minus10WithILoc = new Minus10Signal(
                metadata.Features.Minus10Signals[0].Location);

            // Set qualifiers and validate them.
            minus10.GeneSymbol = geneSymbol;
            minus10WithILoc.GeneSymbol = geneSymbol;
            Assert.AreEqual(minus10.GeneSymbol, geneSymbol);
            Assert.AreEqual(minus10WithILoc.GeneSymbol, geneSymbol);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the minus10Signal feature '{0}'",
                metadata.Features.Minus10Signals[0].Location));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the minus10Signal feature '{0}'",
                minus10Signal.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank PolyASignal feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank PolyASignal feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankPolyASignalFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankPolyASignalNode, Constants.FilePathNode);
            string polyALocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankPolyASignalNode, Constants.Location);
            string featureCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankPolyASignalNode, Constants.QualifierCount);
            string geneSymbol = Utility._xmlUtil.GetTextValue(
                Constants.GenBankPolyASignalNode, Constants.GeneSymbol);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate PolyASignal feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<PolyASignal> polyASignalList = metadata.Features.PolyASignals;

            // Create a clone of PolyASignal feature feature.
            PolyASignal cloneMinus10Signal = polyASignalList[0].Clone();
            Assert.AreEqual(polyASignalList.Count.ToString(), featureCount);
            Assert.AreEqual(cloneMinus10Signal.GeneSymbol, geneSymbol);
            Assert.IsEmpty(cloneMinus10Signal.DatabaseCrossReference);
            Assert.IsEmpty(polyASignalList[0].Allele);
            Assert.IsEmpty(polyASignalList[0].Citation);
            Assert.IsEmpty(polyASignalList[0].Experiment);
            Assert.IsEmpty(polyASignalList[0].GenomicMapPosition);
            Assert.IsEmpty(polyASignalList[0].GeneSynonym);
            Assert.IsEmpty(polyASignalList[0].Inference);
            Assert.IsEmpty(polyASignalList[0].Label);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.PolyASignals[0].Location), polyALocation);
            Assert.IsEmpty(polyASignalList[0].Note);
            Assert.IsEmpty(polyASignalList[0].OldLocusTag);
            Assert.IsEmpty(polyASignalList[0].LocusTag);
            Assert.IsEmpty(polyASignalList[0].OldLocusTag);

            // Create a new PolyA signal and validate the same.
            PolyASignal polyASignal = new PolyASignal(polyALocation);
            PolyASignal polyASignalWithILoc = new PolyASignal(
                metadata.Features.Minus10Signals[0].Location);

            // Set qualifiers and validate them.
            polyASignal.GeneSymbol = geneSymbol;
            polyASignalWithILoc.GeneSymbol = geneSymbol;
            Assert.AreEqual(polyASignal.GeneSymbol, geneSymbol);
            Assert.AreEqual(polyASignalWithILoc.GeneSymbol, geneSymbol);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the PolyASignal feature '{0}'",
                metadata.Features.PolyASignals[0].Location));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the PolyASignal feature '{0}'",
                polyASignalList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Terminator feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank Terminator feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankTerminatorFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankTerminatorNode, Constants.FilePathNode);
            string terminatorLocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankTerminatorNode, Constants.Location);
            string featureCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankTerminatorNode, Constants.QualifierCount);
            string geneSymbol = Utility._xmlUtil.GetTextValue(
                Constants.GenBankTerminatorNode, Constants.GeneSymbol);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate Terminator feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<Terminator> terminatorList = metadata.Features.Terminators;

            // Create a clone of Terminator feature feature.
            Terminator cloneTerminator = terminatorList[0].Clone();
            Assert.AreEqual(terminatorList.Count.ToString(), featureCount);
            Assert.AreEqual(cloneTerminator.GeneSymbol, geneSymbol);
            Assert.IsEmpty(cloneTerminator.DatabaseCrossReference);
            Assert.IsEmpty(terminatorList[0].Allele);
            Assert.IsEmpty(terminatorList[0].Citation);
            Assert.IsEmpty(terminatorList[0].Experiment);
            Assert.IsEmpty(terminatorList[0].GenomicMapPosition);
            Assert.IsEmpty(terminatorList[0].GeneSynonym);
            Assert.IsEmpty(terminatorList[0].Inference);
            Assert.IsEmpty(terminatorList[0].Label);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.Terminators[0].Location), terminatorLocation);
            Assert.IsEmpty(terminatorList[0].Note);
            Assert.IsEmpty(terminatorList[0].OldLocusTag);
            Assert.IsEmpty(terminatorList[0].LocusTag);
            Assert.IsEmpty(terminatorList[0].OldLocusTag);
            Assert.IsEmpty(terminatorList[0].StandardName);

            // Create a new Terminator signal and validate the same.
            Terminator terminator = new Terminator(terminatorLocation);
            Terminator terminatorWithILoc = new Terminator(
                metadata.Features.Terminators[0].Location);

            // Set qualifiers and validate them.
            terminator.GeneSymbol = geneSymbol;
            terminatorWithILoc.GeneSymbol = geneSymbol;
            Assert.AreEqual(terminator.GeneSymbol, geneSymbol);
            Assert.AreEqual(terminatorWithILoc.GeneSymbol, geneSymbol);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Terminator feature '{0}'",
                metadata.Features.PolyASignals[0].Location));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Terminator feature '{0}'",
                terminatorList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Misc Signal feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank Misc Signal feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankMiscSignalFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMiscSignalNode, Constants.FilePathNode);
            string miscSignalLocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMiscSignalNode, Constants.Location);
            string featureCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMiscSignalNode, Constants.QualifierCount);
            string geneSymbol = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMiscSignalNode, Constants.GeneSymbol);
            string function = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMiscSignalNode, Constants.FunctionNode);
            string dbReferenceNode = Utility._xmlUtil.GetTextValue(
                Constants.GenBankMiscSignalNode, Constants.DbReferenceNode);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate MiscSignal feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<MiscSignal> miscSignalList = metadata.Features.MiscSignals;

            // Create a clone of MiscSignal feature feature.
            MiscSignal cloneMiscSignal = miscSignalList[0].Clone();
            Assert.AreEqual(miscSignalList.Count.ToString(), featureCount);
            Assert.AreEqual(cloneMiscSignal.GeneSymbol, geneSymbol);
            Assert.AreEqual(cloneMiscSignal.DatabaseCrossReference[0], dbReferenceNode);
            Assert.IsEmpty(miscSignalList[0].Allele);
            Assert.IsEmpty(miscSignalList[0].Citation);
            Assert.IsEmpty(miscSignalList[0].Experiment);
            Assert.IsEmpty(miscSignalList[0].GenomicMapPosition);
            Assert.IsEmpty(miscSignalList[0].GeneSynonym);
            Assert.IsEmpty(miscSignalList[0].Inference);
            Assert.IsEmpty(miscSignalList[0].Label);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.MiscSignals[0].Location), miscSignalLocation);
            Assert.IsEmpty(miscSignalList[0].Note);
            Assert.IsEmpty(miscSignalList[0].OldLocusTag);
            Assert.IsEmpty(miscSignalList[0].LocusTag);
            Assert.IsEmpty(miscSignalList[0].OldLocusTag);
            Assert.AreEqual(miscSignalList[0].Function[0], function);
            Assert.IsEmpty(miscSignalList[0].Operon);

            // Create a new MiscSignal signal and validate the same.
            MiscSignal miscSignal = new MiscSignal(miscSignalLocation);
            MiscSignal miscSignalWithIloc = new MiscSignal(
                metadata.Features.MiscSignals[0].Location);

            // Set qualifiers and validate them.
            miscSignal.GeneSymbol = geneSymbol;
            miscSignalWithIloc.GeneSymbol = geneSymbol;
            Assert.AreEqual(miscSignal.GeneSymbol, geneSymbol);
            Assert.AreEqual(miscSignalWithIloc.GeneSymbol, geneSymbol);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Terminator feature '{0}'",
                metadata.Features.MiscSignals[0].Location));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Terminator feature '{0}'",
                miscSignalList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank DisplacementLoop feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank DisplacementLoop feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankDLoopFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankDLoopNode, Constants.FilePathNode);
            string dLoopLocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankDLoopNode, Constants.Location);
            string featureCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankDLoopNode, Constants.QualifierCount);
            string geneSymbol = Utility._xmlUtil.GetTextValue(
                Constants.GenBankDLoopNode, Constants.GeneSymbol);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate DLoop feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<DisplacementLoop> dLoopList = metadata.Features.DisplacementLoops;

            // Create a clone of DLoop feature feature.
            DisplacementLoop cloneDLoop = dLoopList[0].Clone();
            Assert.AreEqual(dLoopList.Count.ToString(), featureCount);
            Assert.AreEqual(cloneDLoop.GeneSymbol, geneSymbol);
            Assert.IsEmpty(cloneDLoop.DatabaseCrossReference);
            Assert.IsEmpty(dLoopList[0].Allele);
            Assert.IsEmpty(dLoopList[0].Citation);
            Assert.IsEmpty(dLoopList[0].Experiment);
            Assert.IsEmpty(dLoopList[0].GenomicMapPosition);
            Assert.IsEmpty(dLoopList[0].GeneSynonym);
            Assert.IsEmpty(dLoopList[0].Inference);
            Assert.IsEmpty(dLoopList[0].Label);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.DisplacementLoops[0].Location), dLoopLocation);
            Assert.IsEmpty(dLoopList[0].Note);
            Assert.IsEmpty(dLoopList[0].OldLocusTag);
            Assert.IsEmpty(dLoopList[0].LocusTag);
            Assert.IsEmpty(dLoopList[0].OldLocusTag);

            // Create a new DLoop signal and validate the same.
            DisplacementLoop dLoop = new DisplacementLoop(dLoopLocation);
            DisplacementLoop dLoopWithIloc = new DisplacementLoop(
                metadata.Features.DisplacementLoops[0].Location);

            // Set qualifiers and validate them.
            dLoop.GeneSymbol = geneSymbol;
            dLoopWithIloc.GeneSymbol = geneSymbol;
            Assert.AreEqual(dLoop.GeneSymbol, geneSymbol);
            Assert.AreEqual(dLoopWithIloc.GeneSymbol, geneSymbol);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the DisplacementLoop feature '{0}'",
                metadata.Features.DisplacementLoops[0].Location));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the DisplacementLoop feature '{0}'",
                dLoopList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Intervening DNA feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank Intervening feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankInterveningDNAFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankInterveningDNA, Constants.FilePathNode);
            string iDNALocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankInterveningDNA, Constants.Location);
            string featureCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankInterveningDNA, Constants.QualifierCount);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate iDNA feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<InterveningDNA> iDNAList = metadata.Features.InterveningDNAs;

            // Create a clone copy and validate.
            InterveningDNA iDNAClone = iDNAList[0].Clone();
            Assert.AreEqual(iDNAList.Count.ToString(), featureCount);
            Assert.IsEmpty(iDNAClone.GeneSymbol);
            Assert.IsEmpty(iDNAClone.DatabaseCrossReference);
            Assert.IsEmpty(iDNAClone.Allele);
            Assert.IsEmpty(iDNAList[0].Citation);
            Assert.IsEmpty(iDNAList[0].Experiment);
            Assert.IsEmpty(iDNAList[0].GenomicMapPosition);
            Assert.IsEmpty(iDNAList[0].GeneSynonym);
            Assert.IsEmpty(iDNAList[0].Inference);
            Assert.IsEmpty(iDNAList[0].Label);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.InterveningDNAs[0].Location), iDNALocation);
            Assert.IsEmpty(iDNAList[0].Note);
            Assert.IsEmpty(iDNAList[0].OldLocusTag);
            Assert.IsEmpty(iDNAList[0].LocusTag);
            Assert.IsEmpty(iDNAList[0].OldLocusTag);
            Assert.IsEmpty(iDNAList[0].Function);
            Assert.IsEmpty(iDNAList[0].Number);
            Assert.IsEmpty(iDNAList[0].StandardName);

            // Create a new Intervening DNA signal and validate the same.
            InterveningDNA iDNA = new InterveningDNA(iDNALocation);
            InterveningDNA iDNAWithIloc = new InterveningDNA(
                metadata.Features.DisplacementLoops[0].Location);

            // Set qualifiers and validate them.
            iDNA.GeneSymbol = iDNALocation;
            iDNAWithIloc.GeneSymbol = iDNALocation;
            Assert.AreEqual(iDNA.GeneSymbol, iDNALocation);
            Assert.AreEqual(iDNAWithIloc.GeneSymbol, iDNALocation);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the iDNA feature '{0}'",
                metadata.Features.InterveningDNAs[0].Location));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the iDNA feature '{0}'",
                iDNAList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Misc Recombination feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank Misc Recombination feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankMiscRecombinationFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.ProteinGenBankMiscRecombinationQualifiersNode,
                Constants.FilePathNode);
            string miscCombinationCount = Utility._xmlUtil.GetTextValue(
                Constants.ProteinGenBankMiscRecombinationQualifiersNode,
                Constants.FeaturesCount);
            string miscCombinationLocation = Utility._xmlUtil.GetTextValue(
                Constants.ProteinGenBankMiscRecombinationQualifiersNode,
                Constants.Location);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate MiscRecombination feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<MiscRecombination> miscRecombinationList =
                metadata.Features.MiscRecombinations;

            Assert.AreEqual(miscRecombinationList.Count.ToString(),
                miscCombinationCount);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.MiscRecombinations[0].Location),
                miscCombinationLocation);
            Assert.IsEmpty(miscRecombinationList[0].GeneSymbol);
            Assert.IsEmpty(miscRecombinationList[0].DatabaseCrossReference);
            Assert.IsEmpty(miscRecombinationList[0].Allele);
            Assert.IsEmpty(miscRecombinationList[0].Citation);
            Assert.IsEmpty(miscRecombinationList[0].Experiment);
            Assert.IsEmpty(miscRecombinationList[0].GeneSynonym);
            Assert.IsEmpty(miscRecombinationList[0].GenomicMapPosition);
            Assert.IsEmpty(miscRecombinationList[0].Inference);
            Assert.IsEmpty(miscRecombinationList[0].Label);
            Assert.IsEmpty(miscRecombinationList[0].OldLocusTag);
            Assert.IsEmpty(miscRecombinationList[0].StandardName);
            Assert.IsEmpty(miscRecombinationList[0].LocusTag);
            Assert.IsEmpty(miscRecombinationList[0].OldLocusTag);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the MiscRecombination feature '{0}'",
                miscRecombinationList[0].Note));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the MiscRecombination feature '{0}'",
                miscRecombinationList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Misc RNA feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank Misc RNA feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankMiscRNAFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.ProteinGenBankMiscRNAQualifiersNode,
                Constants.FilePathNode);
            string miscRnaCount = Utility._xmlUtil.GetTextValue(
                Constants.ProteinGenBankMiscRNAQualifiersNode,
                Constants.FeaturesCount);
            string miscRnaLocation = Utility._xmlUtil.GetTextValue(
                Constants.ProteinGenBankMiscRNAQualifiersNode,
                Constants.Location);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate MiscRNA feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<MiscRNA> miscRnaList = metadata.Features.MiscRNAs;

            // Create a clone of MiscRNA feature and validate
            MiscRNA cloneMiscRna = miscRnaList[0].Clone();
            Assert.AreEqual(miscRnaList.Count.ToString(), miscRnaCount);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.MiscRNAs[0].Location), miscRnaLocation);
            Assert.IsEmpty(cloneMiscRna.GeneSymbol);
            Assert.IsNotEmpty(cloneMiscRna.DatabaseCrossReference);
            Assert.IsEmpty(miscRnaList[0].Allele);
            Assert.IsEmpty(miscRnaList[0].Citation);
            Assert.IsEmpty(miscRnaList[0].Experiment);
            Assert.IsEmpty(miscRnaList[0].GeneSynonym);
            Assert.IsEmpty(miscRnaList[0].GenomicMapPosition);
            Assert.IsEmpty(miscRnaList[0].Label);
            Assert.IsEmpty(miscRnaList[0].OldLocusTag);
            Assert.IsEmpty(miscRnaList[0].StandardName);
            Assert.IsEmpty(miscRnaList[0].LocusTag);
            Assert.IsEmpty(miscRnaList[0].OldLocusTag);
            Assert.IsFalse(miscRnaList[0].Pseudo);
            Assert.IsEmpty(miscRnaList[0].Function);
            Assert.IsEmpty(miscRnaList[0].Operon);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the MiscRNA feature '{0}'",
                miscRnaList[0].Note));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the MiscRNA feature '{0}'",
                miscRnaList.Count.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the MiscRNA feature '{0}'",
                miscRnaList[0].DatabaseCrossReference));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the MiscRNA feature '{0}'",
                miscRnaList[0].Inference));
        }

        /// <summary>
        /// Validate GenBank Ribosomal RNA feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank Ribosomal RNA feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankRibosomalRNAFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.FilePathNode);
            string rRnaCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.FeaturesCount);
            string rRnaLocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.Location);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate RibosomalRNA feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<RibosomalRNA> ribosomalRnaList =
                metadata.Features.RibosomalRNAs;

            Assert.AreEqual(ribosomalRnaList.Count.ToString(), rRnaCount);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.RibosomalRNAs[0].Location), rRnaLocation);
            Assert.IsEmpty(ribosomalRnaList[0].GeneSymbol);
            Assert.IsNotEmpty(ribosomalRnaList[0].DatabaseCrossReference);
            Assert.IsEmpty(ribosomalRnaList[0].Allele);
            Assert.IsEmpty(ribosomalRnaList[0].Citation);
            Assert.IsEmpty(ribosomalRnaList[0].Experiment);
            Assert.IsEmpty(ribosomalRnaList[0].GeneSynonym);
            Assert.IsEmpty(ribosomalRnaList[0].GenomicMapPosition);
            Assert.IsEmpty(ribosomalRnaList[0].Label);
            Assert.IsEmpty(ribosomalRnaList[0].OldLocusTag);
            Assert.IsEmpty(ribosomalRnaList[0].StandardName);
            Assert.IsEmpty(ribosomalRnaList[0].LocusTag);
            Assert.IsEmpty(ribosomalRnaList[0].OldLocusTag);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the rRNA feature '{0}'",
                ribosomalRnaList[0].Note));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the rRNA feature '{0}'",
                ribosomalRnaList.Count.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the rRNA feature '{0}'",
                ribosomalRnaList[0].DatabaseCrossReference));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the rRNA feature '{0}'",
                ribosomalRnaList[0].Inference));
        }

        /// <summary>
        /// Validate GenBank Repeat Origin feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank Repeat Origin feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankRepeatOriginFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.FilePathNode);
            string rOriginCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.ROriginCount);
            string rOriginLocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.ROriginLocation);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate Repeat Origin feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<ReplicationOrigin> repeatOriginList =
                metadata.Features.ReplicationOrigins;

            Assert.AreEqual(repeatOriginList.Count.ToString(), rOriginCount);
            Assert.AreEqual(locBuilder.GetLocationString(
               metadata.Features.ReplicationOrigins[0].Location), rOriginLocation);
            Assert.IsEmpty(repeatOriginList[0].GeneSymbol);
            Assert.IsEmpty(repeatOriginList[0].DatabaseCrossReference);
            Assert.IsEmpty(repeatOriginList[0].Allele);
            Assert.IsEmpty(repeatOriginList[0].Citation);
            Assert.IsEmpty(repeatOriginList[0].Experiment);
            Assert.IsEmpty(repeatOriginList[0].GeneSynonym);
            Assert.IsEmpty(repeatOriginList[0].GenomicMapPosition);
            Assert.IsEmpty(repeatOriginList[0].Label);
            Assert.IsEmpty(repeatOriginList[0].OldLocusTag);
            Assert.IsEmpty(repeatOriginList[0].StandardName);
            Assert.IsEmpty(repeatOriginList[0].LocusTag);
            Assert.IsEmpty(repeatOriginList[0].OldLocusTag);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Repeat Origin feature '{0}'",
                repeatOriginList[0].Note));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Repeat Origin '{0}'",
                repeatOriginList.Count.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Repeat Origin '{0}'",
                repeatOriginList[0].DatabaseCrossReference));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Repeat Origin '{0}'",
                repeatOriginList[0].Inference));
        }

        /// <summary>
        /// Validate GenBank CaatSignal feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank CaatSignal feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankCaatSignalFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.FilePathNode);
            string caatSignalCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.CaatSignalCount);
            string expectedGeneSymbol = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.CaatSignalGene);
            string expectedLocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.CaatSignalLocation);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate CaatSignal feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<CaatSignal> caatSignalList = metadata.Features.CAATSignals;
            Assert.AreEqual(caatSignalList.Count.ToString(), caatSignalCount);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.CAATSignals[0].Location), expectedLocation);
            Assert.AreEqual(caatSignalList[0].GeneSymbol, expectedGeneSymbol);
            Assert.IsEmpty(caatSignalList[0].DatabaseCrossReference);
            Assert.IsEmpty(caatSignalList[0].Allele);
            Assert.IsEmpty(caatSignalList[0].Citation);
            Assert.IsEmpty(caatSignalList[0].Experiment);
            Assert.IsEmpty(caatSignalList[0].GeneSynonym);
            Assert.IsEmpty(caatSignalList[0].GenomicMapPosition);
            Assert.IsEmpty(caatSignalList[0].Label);
            Assert.IsEmpty(caatSignalList[0].OldLocusTag);
            Assert.IsEmpty(caatSignalList[0].LocusTag);
            Assert.IsEmpty(caatSignalList[0].OldLocusTag);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the CaatSignal feature '{0}'",
                caatSignalList[0].Note));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the CaatSignal '{0}'",
             caatSignalList.Count.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the CaatSignal '{0}'",
                caatSignalList[0].GeneSymbol));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the CaatSignal '{0}'",
                caatSignalList[0].Location));
        }

        /// <summary>
        /// Validate GenBank TataSignal feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank TataSignal feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankTataSignalFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.FilePathNode);
            string tataSignalCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.TataSignalCount);
            string expectedGeneSymbol = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.TataSignalGene);
            string expectedLocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.TataSignalLocation);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate TataSignal feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<TataSignal> tataSignalList = metadata.Features.TATASignals;
            Assert.AreEqual(tataSignalList.Count.ToString(), tataSignalCount);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.TATASignals[0].Location), expectedLocation);
            Assert.AreEqual(tataSignalList[0].GeneSymbol, expectedGeneSymbol);
            Assert.IsEmpty(tataSignalList[0].DatabaseCrossReference);
            Assert.IsEmpty(tataSignalList[0].Allele);
            Assert.IsEmpty(tataSignalList[0].Citation);
            Assert.IsEmpty(tataSignalList[0].Experiment);
            Assert.IsEmpty(tataSignalList[0].GeneSynonym);
            Assert.IsEmpty(tataSignalList[0].GenomicMapPosition);
            Assert.IsEmpty(tataSignalList[0].Label);
            Assert.IsEmpty(tataSignalList[0].OldLocusTag);
            Assert.IsEmpty(tataSignalList[0].LocusTag);
            Assert.IsEmpty(tataSignalList[0].OldLocusTag);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the TataSignal feature '{0}'",
                tataSignalList[0].Note));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the TataSignal '{0}'",
                tataSignalList.Count.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the TataSignal '{0}'",
                tataSignalList[0].GeneSymbol));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the TataSignal '{0}'",
                tataSignalList[0].Location));
        }

        /// <summary>
        /// Validate GenBank 3'UTRs  feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank 3'UTRs feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankThreePrimeUTRsFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.FilePathNode);
            string threePrimeUTRCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.ThreePrimeUTRCount);
            string expectedGeneSymbol = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.ThreePrimeUTRGene);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);

            // Validate 3'UTRs feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<ThreePrimeUtr> threeprimeUTRsList =
                metadata.Features.ThreePrimeUTRs;
            Assert.AreEqual(threeprimeUTRsList.Count.ToString(), threePrimeUTRCount);
            Assert.AreEqual(threeprimeUTRsList[0].GeneSymbol, expectedGeneSymbol);
            Assert.IsEmpty(threeprimeUTRsList[0].DatabaseCrossReference);
            Assert.IsEmpty(threeprimeUTRsList[0].Allele);
            Assert.IsEmpty(threeprimeUTRsList[0].Citation);
            Assert.IsEmpty(threeprimeUTRsList[0].Experiment);
            Assert.IsEmpty(threeprimeUTRsList[0].GeneSynonym);
            Assert.IsEmpty(threeprimeUTRsList[0].GenomicMapPosition);
            Assert.IsEmpty(threeprimeUTRsList[0].Label);
            Assert.IsEmpty(threeprimeUTRsList[0].OldLocusTag);
            Assert.IsEmpty(threeprimeUTRsList[0].LocusTag);
            Assert.IsEmpty(threeprimeUTRsList[0].OldLocusTag);


            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the 3'UTRs '{0}'",
                threeprimeUTRsList.Count.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the 3'UTRs '{0}'",
                threeprimeUTRsList[0].GeneSymbol));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the 3'UTRs '{0}'",
                threeprimeUTRsList[0].Location));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the 3'UTRs '{0}'",
                threeprimeUTRsList[0].Note));
        }

        /// <summary>
        /// Validate GenBank 5'UTRs  feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank 5'UTRs feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankFivePrimeUTRsFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.FilePathNode);
            string fivePrimeUTRCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.ThreePrimeUTRCount);
            string expectedGeneSymbol = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.FivePrimeUTRGene);
            string expectedLocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode, Constants.FivePrimeUTRLocation);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate 5'UTRs feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<FivePrimeUtr> fivePrimeUTRsList =
                metadata.Features.FivePrimeUTRs;
            Assert.AreEqual(fivePrimeUTRsList.Count.ToString(), fivePrimeUTRCount);
            Assert.AreEqual(fivePrimeUTRsList[0].GeneSymbol, expectedGeneSymbol);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.FivePrimeUTRs[0].Location), expectedLocation);
            Assert.IsEmpty(fivePrimeUTRsList[0].DatabaseCrossReference);
            Assert.IsEmpty(fivePrimeUTRsList[0].Allele);
            Assert.IsEmpty(fivePrimeUTRsList[0].Citation);
            Assert.IsEmpty(fivePrimeUTRsList[0].Experiment);
            Assert.IsEmpty(fivePrimeUTRsList[0].GeneSynonym);
            Assert.IsEmpty(fivePrimeUTRsList[0].GenomicMapPosition);
            Assert.IsEmpty(fivePrimeUTRsList[0].Label);
            Assert.IsEmpty(fivePrimeUTRsList[0].OldLocusTag);
            Assert.IsEmpty(fivePrimeUTRsList[0].LocusTag);
            Assert.IsEmpty(fivePrimeUTRsList[0].OldLocusTag);


            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the 5'UTRs '{0}'",
                fivePrimeUTRsList.Count.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the 5'UTRs '{0}'",
                fivePrimeUTRsList[0].GeneSymbol));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the 5'UTRs '{0}'",
                fivePrimeUTRsList[0].Location));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the 5'UTRs '{0}'",
                fivePrimeUTRsList[0].Note));
        }

        /// <summary>
        /// Validate GenBank SignalPeptide feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank SignalPeptide feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankSignalPeptideFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode,
                Constants.FilePathNode);
            string signalPeptideCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode,
                Constants.ThreePrimeUTRCount);
            string expectedLocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankrRNAQualifiersNode,
                Constants.SignalPeptideLocation);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate SignalPeptide feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<SignalPeptide> signalPeptideQualifiersList =
                metadata.Features.SignalPeptides;
            Assert.AreEqual(signalPeptideQualifiersList.Count.ToString(),
                signalPeptideCount);
            Assert.IsEmpty(signalPeptideQualifiersList[0].GeneSymbol);
            Assert.AreEqual(locBuilder.GetLocationString(
                 metadata.Features.SignalPeptides[0].Location),
                 expectedLocation);
            Assert.IsEmpty(signalPeptideQualifiersList[0].DatabaseCrossReference);
            Assert.IsEmpty(signalPeptideQualifiersList[0].Allele);
            Assert.IsEmpty(signalPeptideQualifiersList[0].Citation);
            Assert.IsEmpty(signalPeptideQualifiersList[0].Experiment);
            Assert.IsEmpty(signalPeptideQualifiersList[0].GeneSynonym);
            Assert.IsEmpty(signalPeptideQualifiersList[0].GenomicMapPosition);
            Assert.IsEmpty(signalPeptideQualifiersList[0].Label);
            Assert.IsEmpty(signalPeptideQualifiersList[0].OldLocusTag);
            Assert.IsEmpty(signalPeptideQualifiersList[0].LocusTag);
            Assert.IsEmpty(signalPeptideQualifiersList[0].OldLocusTag);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the SignalPeptide '{0}'",
                signalPeptideQualifiersList.Count.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the SignalPeptide '{0}'",
                signalPeptideQualifiersList[0].GeneSymbol));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the SignalPeptide '{0}'",
                signalPeptideQualifiersList[0].Location));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the SignalPeptide '{0}'",
                signalPeptideQualifiersList[0].Note));
        }

        /// <summary>
        /// Validate GenBank RepeatRegion feature Qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GenBank RepeatRegion feature Qualifiers.
        /// </summary>
        [Test]
        public void ValidateGenBankRepeatRegionFeatureQualifiers()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankRepeatRegionQualifiersNode,
                Constants.FilePathNode);
            string repeatRegionCount = Utility._xmlUtil.GetTextValue(
                Constants.GenBankRepeatRegionQualifiersNode,
                Constants.ThreePrimeUTRCount);
            string expectedLocation = Utility._xmlUtil.GetTextValue(
                Constants.GenBankRepeatRegionQualifiersNode,
                Constants.RepeatRegionLocation);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate RepeatRegion feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<RepeatRegion> repeatRegionsList =
                metadata.Features.RepeatRegions;
            Assert.AreEqual(repeatRegionsList.Count.ToString(),
                repeatRegionCount);
            Assert.IsEmpty(repeatRegionsList[0].GeneSymbol);
            Assert.AreEqual(locBuilder.GetLocationString(
                  metadata.Features.RepeatRegions[0].Location),
                  expectedLocation);
            Assert.IsEmpty(repeatRegionsList[0].DatabaseCrossReference);
            Assert.IsEmpty(repeatRegionsList[0].Note);
            Assert.IsEmpty(repeatRegionsList[0].Allele);
            Assert.IsEmpty(repeatRegionsList[0].Citation);
            Assert.IsEmpty(repeatRegionsList[0].Experiment);
            Assert.IsEmpty(repeatRegionsList[0].GeneSynonym);
            Assert.IsEmpty(repeatRegionsList[0].GenomicMapPosition);
            Assert.IsEmpty(repeatRegionsList[0].Label);
            Assert.IsEmpty(repeatRegionsList[0].OldLocusTag);
            Assert.IsEmpty(repeatRegionsList[0].LocusTag);
            Assert.IsEmpty(repeatRegionsList[0].OldLocusTag);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the RepeatRegion '{0}'",
                repeatRegionsList.Count.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the RepeatRegion '{0}'",
                repeatRegionsList[0].GeneSymbol));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the RepeatRegion '{0}'",
                repeatRegionsList[0].Location));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the RepeatRegion '{0}'",
                repeatRegionsList[0].Note));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the RepeatRegion '{0}'",
                repeatRegionsList[0].MobileElement));
        }

        /// <summary>
        /// Validate GenBank Location resolver subsequence of Repeat region feature.
        /// Input : GenBank file.
        /// Output : Validation of GenBank feature sub sequence.
        /// </summary>
        [Test]
        public void ValidateGenBankSubSequence()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankRepeatRegionQualifiersNode,
                Constants.FilePathNode);
            string expectedSubSequence = Utility._xmlUtil.GetTextValue(
                Constants.GenBankRepeatRegionQualifiersNode,
                Constants.ExpectedFeatureSubSequence);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            ISequence sequence = parserObj.ParseOne(filePath);
            ILocationResolver locResolver = new LocationResolver();

            // Get repeatregion subsequence.
            GenBankMetadata metadata =
                (GenBankMetadata)sequence.Metadata[Constants.GenBank];
            ISequence subSeq = locResolver.GetSubSequence(
                metadata.Features.RepeatRegions[0].Location, sequence);

            // Validate repeat region subsequence.
            Assert.AreEqual(subSeq.ToString(), expectedSubSequence);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank subSequence");
            Console.WriteLine(string.Format(null,
                "Successfully validated the GenBank subSequence ",
                subSeq.ToString()));
        }

        /// <summary>
        /// Validate GenBank IsInStart,IsInEnd and IsInRange 
        /// methods of location resolver.
        /// Input : GenBank file.
        /// Output : Validation of GenBank feature sub sequence.
        /// </summary>
        [Test]
        public void ValidateGenBankLocationStartAndEndRange()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.GenBankRepeatRegionQualifiersNode,
                Constants.FilePathNode);
            bool startLocResult = false;
            bool endLocResult = false;
            bool rangeLocResult = false;

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            ISequence sequence = parserObj.ParseOne(filePath);
            ILocationResolver locResolver = new LocationResolver();

            // Validate Start,End and Range of Gene feature location.
            GenBankMetadata metadata =
                (GenBankMetadata)sequence.Metadata[Constants.GenBank];
            startLocResult =
                locResolver.IsInStart(metadata.Features.Genes[0].Location, 289);
            endLocResult =
                locResolver.IsInEnd(metadata.Features.Genes[0].Location, 1647);
            rangeLocResult =
                locResolver.IsInRange(metadata.Features.Genes[0].Location, 300);

            Assert.IsTrue(startLocResult);
            Assert.IsTrue(endLocResult);
            Assert.IsTrue(rangeLocResult);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the location resolver Gene feature start End and IsInRange methods");
            Console.WriteLine(string.Format(null,
                "Successfully validated the location resolver Gene feature start",
                startLocResult));
        }

        /// <summary>
        /// Validate LocationRange creation.
        /// Input : GenBank file.
        /// Output : Validation of created location range.
        /// </summary>
        [Test]
        public void ValidateLocationRangeWithAccession()
        {
            // Get Values from XML node.
            string acessionNumber = Utility._xmlUtil.GetTextValue(
                Constants.LocationRangesNode, Constants.Accession);
            string startLoc = Utility._xmlUtil.GetTextValue(
                Constants.LocationRangesNode, Constants.LoocationStartNode);
            string endLoc = Utility._xmlUtil.GetTextValue(
                Constants.LocationRangesNode, Constants.LoocationEndNode);

            // Create a Location Range.
            LocationRange locRangeObj = new LocationRange(acessionNumber,
                Convert.ToInt32(startLoc), Convert.ToInt32(endLoc));

            // Validate created location Range.
            Assert.AreEqual(acessionNumber, locRangeObj.Accession.ToString());
            Assert.AreEqual(startLoc, locRangeObj.StartPosition.ToString());
            Assert.AreEqual(endLoc, locRangeObj.EndPosition.ToString());

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the creation of location range");
            Console.WriteLine(string.Format(null,
                "Successfully validated the start location",
                startLoc));
        }

        /// <summary>
        /// Validate LocationRange creation with empty accession ID.
        /// Input : GenBank file.
        /// Output : Validation of created location range.
        /// </summary>
        [Test]
        public void ValidateLocationRanges()
        {
            // Get Values from XML node.
            string startLoc = Utility._xmlUtil.GetTextValue(
                Constants.LocationRangesNode, Constants.LoocationStartNode);
            string endLoc = Utility._xmlUtil.GetTextValue(
                Constants.LocationRangesNode, Constants.LoocationEndNode);

            // Create a Location Range.
            LocationRange locRangeObj = new LocationRange(Convert.ToInt32(startLoc),
                Convert.ToInt32(endLoc));

            // Validate created location Range.
            Assert.AreEqual(startLoc, locRangeObj.StartPosition.ToString());
            Assert.AreEqual(endLoc, locRangeObj.EndPosition.ToString());

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the creation of location range");
            Console.WriteLine(string.Format(null,
                "Successfully validated the start location",
                startLoc));
        }

        /// <summary>
        /// Validate Misc Structure qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of Misc Structure qualifiers.
        /// </summary>
        [Test]
        public void ValidateMiscStructureQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankMiscStructureNode, FeatureGroup.MiscStructure);
        }

        /// <summary>
        /// Validate TransitPeptide feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of TransitPeptide qualifiers.
        /// </summary>
        [Test]
        public void ValidateTransitPeptideQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankTransitPeptideNode, FeatureGroup.TrnsitPeptide);
        }

        /// <summary>
        /// Validate Stem Loop feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of Stem Loop qualifiers.
        /// </summary>
        [Test]
        public void ValidateStemLoopQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankStemLoopNode, FeatureGroup.StemLoop);
        }

        /// <summary>
        /// Validate Modified base feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of Modified base qualifiers.
        /// </summary>
        [Test]
        public void ValidateModifiedBaseQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankModifiedBaseNode, FeatureGroup.ModifiedBase);
        }

        /// <summary>
        /// Validate Precursor RNA feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of Precursor RNA qualifiers.
        /// </summary>
        [Test]
        public void ValidatePrecursorRNAQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankPrecursorRNANode, FeatureGroup.PrecursorRNA);
        }

        /// <summary>
        /// Validate Poly Site feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of Poly Site qualifiers.
        /// </summary>
        [Test]
        public void ValidatePolySiteQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankPolySiteNode, FeatureGroup.PolySite);
        }

        /// <summary>
        /// Validate Misc Binding feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of Misc Binding qualifiers.
        /// </summary>
        [Test]
        public void ValidateMiscBindingQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankMiscBindingNode,
                FeatureGroup.MiscBinding);
        }

        /// <summary>
        /// Validate Enhancer feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of Enhancer qualifiers.
        /// </summary>
        [Test]
        public void ValidateEnhancerQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankEnhancerNode, FeatureGroup.Enhancer);
        }

        /// <summary>
        /// Validate GC_Signal feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of GC_Signal qualifiers.
        /// </summary>
        [Test]
        public void ValidateGCSignalQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankGCSignalNode, FeatureGroup.GCSignal);
        }

        /// <summary>
        /// Validate Long Terminal Repeat feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of Long Terminal Repeat qualifiers.
        /// </summary>
        [Test]
        public void ValidateLTRFeatureQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankLongTerminalRepeatNode, FeatureGroup.LTR);
        }

        /// <summary>
        /// Validate Operon region feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of Operon region qualifiers.
        /// </summary>
        [Test]
        public void ValidateOperonFeatureQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankOperonFeatureNode, FeatureGroup.Operon);
        }

        /// <summary>
        /// Validate Unsure Sequence region feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of Unsure Sequence region qualifiers.
        /// </summary>
        [Test]
        public void ValidateUnsureSeqRegionFeatureQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankUnsureSequenceRegionNode,
                FeatureGroup.UnsureSequenceRegion);
        }

        /// <summary>
        /// Validate NonCoding RNA feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of NonCoding RNA qualifiers.
        /// </summary>
        [Test]
        public void ValidateNonCodingRNAFeatureQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankNonCodingRNANode,
                FeatureGroup.NonCodingRNA);
        }

        /// <summary>
        /// Validate CDS feature qualifiers.
        /// Input : GenBank file.
        /// Output : Validation of CDS qualifiers.
        /// </summary>
        [Test]
        public void ValidateCDSFeatureQualifiers()
        {
            ValidateGeneralGenBankFeatureQualifiers(
                Constants.GenBankCDSNode, FeatureGroup.CDS);
        }

        /// <summary>
        /// Validate Standard Feature qualifier names.
        /// Input : GenBank file.
        /// Output : Validation of Standard Qaulifier names.
        /// </summary>
        [Test]
        public void ValidateStandardQualifierNames()
        {
            // Get Values from XML node.
            string expectedAlleleQualifier = Utility._xmlUtil.GetTextValue(
                Constants.StandardQualifierNamesNode, Constants.AlleleQualifier);
            string expectedGeneSymbolQualifier = Utility._xmlUtil.GetTextValue(
                Constants.StandardQualifierNamesNode, Constants.GeneQualifier);
            string expectedDbReferenceQualifier = Utility._xmlUtil.GetTextValue(
                Constants.StandardQualifierNamesNode, Constants.DBReferenceQualifier);
            string allQualifiersCount = Utility._xmlUtil.GetTextValue(
                Constants.StandardQualifierNamesNode, Constants.QualifierCount);

            // Validate GenBank feature standard qualifier names.
            Assert.AreEqual(StandardQualifierNames.Allele,
                expectedAlleleQualifier);
            Assert.AreEqual(StandardQualifierNames.GeneSymbol,
                expectedGeneSymbolQualifier);
            Assert.AreEqual(StandardQualifierNames.DatabaseCrossReference,
                expectedDbReferenceQualifier);
            Assert.AreEqual(StandardQualifierNames.All.Count.ToString(),
                allQualifiersCount);

            //Log to Nunit GUI.
            Console.WriteLine(string.Format(null,
                "validated the standard qualifier name '{0}'",
                StandardQualifierNames.Allele.ToString()));
            Console.WriteLine(string.Format(null,
                "validated the standard qualifier name '{0}'",
                StandardQualifierNames.All.Count));
        }

        /// <summary>
        /// Validate subsequence from GenBank sequence with location using 
        /// compliment operator.
        /// Input : GenBank sequence,location.
        /// Output : Validation of expected sub sequence.
        /// </summary>
        [Test]
        public void ValidateSubSequenceWithComplimentOperator()
        {
            ValidateGenBankLocationResolver(
                Constants.LocationWithComplementOperator,
                FeatureOperator.Complement, true);
        }

        /// <summary>
        /// Validate subsequence from GenBank sequence with location using 
        /// join operator.
        /// Input : GenBank sequence,location.
        /// Output : Validation of expected sub sequence.
        /// </summary>
        [Test]
        public void ValidateSubSequenceWithJoinOperator()
        {
            ValidateGenBankLocationResolver(
                Constants.LocationWithJoinOperatorNode,
                FeatureOperator.Join, true);
        }

        /// <summary>
        /// Validate subsequence from GenBank sequence with location using 
        /// order operator.
        /// Input : GenBank sequence,location.
        /// Output : Validation of expected sub sequence.
        /// </summary>
        [Test]
        public void ValidateSubSequenceWithOrderOperator()
        {
            ValidateGenBankLocationResolver(
                Constants.LocationWithJoinOperatorNode,
                FeatureOperator.Order, true);
        }

        /// <summary>
        /// Validate subsequence from GenBank sequence with location using 
        /// dot operator.
        /// Input : GenBank sequence,location.
        /// Output : Validation of expected sub sequence.
        /// </summary>
        [Test]
        public void ValidateSubSequenceWithDotOperator()
        {
            ValidateGenBankLocationResolver(
                Constants.LocationWithDotOperatorNode,
                FeatureOperator.Complement, false);
        }

        /// <summary>
        /// Validate subsequence from GenBank sequence with location using 
        /// compliment operator with sub location.
        /// Input : GenBank sequence,location.
        /// Output : Validation of expected sub sequence.
        /// </summary>
        [Test]
        public void ValidateSubSequenceWithSubLocationComplimentOperator()
        {
            ValidateGenBankLocationResolver(
                Constants.LocationWithOutComplementOperatorNode,
                FeatureOperator.Complement, false);
        }

        /// <summary>
        /// Validate subsequence from GenBank sequence with location using 
        /// join operator with sub location.
        /// Input : GenBank sequence,location.
        /// Output : Validation of expected sub sequence.
        /// </summary>
        [Test]
        public void ValidateSubSequenceWithSubLocationJoinOperator()
        {
            ValidateGenBankLocationResolver(
                Constants.LocationWithOutJoinOperatorNode,
                FeatureOperator.Join, false);
        }

        /// <summary>
        /// Validate subsequence from GenBank sequence with location using 
        /// order operator with sub location.
        /// Input : GenBank sequence,location.
        /// Output : Validation of expected sub sequence.
        /// </summary>
        [Test]
        public void ValidateSubSequenceWithSubLocationOrderOperator()
        {
            ValidateGenBankLocationResolver(
                Constants.LocationWithOutOrderOperatorNode,
                FeatureOperator.Order, false);
        }

        /// <summary>
        /// Validate GenBank location EndData.
        /// Input : GenBank sequence,location.
        /// Output : Validation of location end data.
        /// </summary>
        [Test]
        public void ValidateGenBankLocationEndData()
        {
            ValidateLocationEndData(Constants.LocationWithEndDataNode);
        }

        /// <summary>
        /// Validate GenBank location EndData with "^" operator.
        /// Input : GenBank sequence,location.
        /// Output : Validation of location end data.
        /// </summary>
        [Test]
        public void ValidateGenBankLocationEndDataWithOperator()
        {
            ValidateLocationEndData(Constants.LocationWithEndDataUsingOperatorNode);
        }

        /// <summary>
        /// Validate compare GenBank locations.
        /// Input : Two instances of GenBank locations.
        /// Output : Validate compare two instance of the locations object.
        /// </summary>
        [Test]
        public void ValidateCompareGenBankLocationsObject()
        {
            // Get Values from XML node.
            string locationFirstInput = Utility._xmlUtil.GetTextValue(
                Constants.CompareLocationsNode, Constants.Location1Node);
            string locationSecondInput = Utility._xmlUtil.GetTextValue(
                Constants.CompareLocationsNode, Constants.Location2Node);
            string locationThirdInput = Utility._xmlUtil.GetTextValue(
                Constants.CompareLocationsNode, Constants.Location3Node);

            // Create two location instance.
            ILocationBuilder locBuilder = new LocationBuilder();
            ILocation loc1 = locBuilder.GetLocation(locationFirstInput);
            object loc2 = locBuilder.GetLocation(locationSecondInput);
            object loc3 = locBuilder.GetLocation(locationThirdInput);

            // Compare first and second location instances.
            Assert.AreEqual(0, loc1.CompareTo(loc2));

            // Compare first and third location which are not identical.
            Assert.AreEqual(-1, loc1.CompareTo(loc3));

            // Compare first and null location.
            Assert.AreEqual(1, loc1.CompareTo(null));

            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Gene feature"));
        }

        /// <summary>
        /// Validate leaf location of GenBank locations.
        /// Input : GenBank File
        /// Output : Validation of GenBank leaf locations.
        /// </summary>
        [Test]
        public void ValidateGenBankLeafLocations()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.RNAGenBankFeaturesNode, Constants.FilePathNode);
            string expectedLeafLocation = Utility._xmlUtil.GetTextValue(
                Constants.RNAGenBankFeaturesNode,
                Constants.LeafLocationCountNode);

            GenBankParser parser = new GenBankParser();
            IList<ISequence> seqsList = parser.Parse(filePath);
            GenBankMetadata metadata =
              (GenBankMetadata)seqsList[0].Metadata[Constants.GenBank];

            List<CodingSequence> cdsList = metadata.Features.CodingSequences;

            ILocation newLoc = cdsList[0].Location;
            List<ILocation> leafsList = newLoc.GetLeafLocations();

            Assert.AreEqual(expectedLeafLocation, leafsList.Count.ToString());
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Gene feature"));
        }

        /// <summary>
        /// Validate GenBank locations positions.
        /// Input : GenBank File
        /// Output : Validation of GenBank location positions.
        /// </summary>
        [Test]
        public void ValidateGenBankLocationPositions()
        {
            // Get Values from XML node.
            string location = Utility._xmlUtil.GetTextValue(
                Constants.LocationWithEndDataUsingOperatorNode,
                Constants.Location);
            string expectedEndData = Utility._xmlUtil.GetTextValue(
                Constants.LocationWithEndDataUsingOperatorNode,
                Constants.EndData);
            string expectedStartData = Utility._xmlUtil.GetTextValue(
                Constants.LocationWithEndDataUsingOperatorNode,
                Constants.StartData);
            string position = Utility._xmlUtil.GetTextValue(
                Constants.LocationWithEndDataUsingOperatorNode,
                Constants.Position);

            bool result = false;

            // Build a location.
            LocationResolver locResolver = new LocationResolver();
            ILocationBuilder locBuilder = new LocationBuilder();
            Location loc = (Location)locBuilder.GetLocation(location);
            loc.EndData = expectedEndData;
            loc.StartData = expectedStartData;

            // Validate whether mentioned end data is present in the location
            // or not.
            result = locResolver.IsInEnd(loc, Int32.Parse(position));
            Assert.IsTrue(result);


            // Validate whether mentioned start data is present in the location
            // or not.
            result = locResolver.IsInStart(loc, Int32.Parse(position));
            Assert.IsTrue(result);

            // Validate whether mentioned data is present in the location
            // or not.
            result = locResolver.IsInRange(loc, Int32.Parse(position));
            Assert.IsTrue(result);

            // Log to Nunit GUI.
            ApplicationLog.WriteLine(string.Format(null,
                "GenBankFeatures P1 : Expected sequence is verified"));
            Console.WriteLine(string.Format(null,
                "GenBankFeatures P1 : Expected sequence is verified"));
        }

        /// <summary>
        /// Validate GenBank location clonning.
        /// Input : GenBank File
        /// Output : Validation of GenBank location clonning.
        /// </summary>
        [Test]
        public void ValidateGenBankLocationClonning()
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                Constants.RNAGenBankFeaturesNode, Constants.FilePathNode);

            GenBankParser parser = new GenBankParser();
            IList<ISequence> seqsList = parser.Parse(filePath);
            GenBankMetadata metadata =
              (GenBankMetadata)seqsList[0].Metadata[Constants.GenBank];

            List<CodingSequence> cdsList = metadata.Features.CodingSequences;

            ICloneable newLoc = cdsList[0].Location;

            // clone location.
            object cloneLoc = newLoc.Clone();

            Assert.AreEqual(cloneLoc.ToString(), newLoc.ToString());

            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Gene feature"));
        }

        #endregion GenBank P1 TestCases

        #region Supporting Methods

        /// <summary>
        /// Validate GenBank Gene features.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateGenBankGeneFeatureQualifiers(string nodeName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string GenesCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.GeneCount);
            string GenesDBCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CDSCodonStart);
            string geneSymbol = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.GeneFeatureGeneSymbol);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);

            // Validate Gene feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<Gene> genesList = metadata.Features.Genes;
            Assert.AreEqual(genesList.Count.ToString(), GenesCount);
            Assert.AreEqual(genesList[0].GeneSymbol.ToString(),
                geneSymbol);
            Assert.AreEqual(genesList[0].DatabaseCrossReference.Count,
                Convert.ToInt32(GenesDBCount));
            Assert.IsEmpty(genesList[0].Allele);
            Assert.IsEmpty(genesList[0].Citation);
            Assert.IsEmpty(genesList[0].Experiment);
            Assert.IsEmpty(genesList[0].Function);
            Assert.IsEmpty(genesList[0].GeneSynonym);
            Assert.IsEmpty(genesList[0].GenomicMapPosition);
            Assert.IsEmpty(genesList[0].Inference);
            Assert.IsEmpty(genesList[0].Label);
            Assert.IsEmpty(genesList[0].Note);
            Assert.IsEmpty(genesList[0].Operon);
            Assert.IsEmpty(genesList[0].OldLocusTag);
            Assert.IsEmpty(genesList[0].Phenotype);
            Assert.IsEmpty(genesList[0].Product);
            Assert.IsFalse(genesList[0].Pseudo);
            Assert.IsEmpty(genesList[0].StandardName);
            Assert.IsFalse(genesList[0].TransSplicing);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Gene feature '{0}'",
                genesList[0].GeneSymbol.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Gene feature '{0}'",
                genesList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank tRNA features.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateGenBanktRNAFeatureQualifiers(string nodeName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string tRNAsCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.tRNACount);
            string tRNAGeneSymbol = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.TRNAGeneSymbol);
            string tRNAComplement = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.TRNAComplement);
            string tRNADBCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CDSCodonStart);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate tRNA feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<TransferRNA> tRANsList =
                metadata.Features.TransferRNAs;
            Assert.AreEqual(tRANsList.Count.ToString(),
                tRNAsCount);
            Assert.AreEqual(tRANsList[0].GeneSymbol.ToString(),
                tRNAGeneSymbol);
            Assert.AreEqual(tRANsList[0].DatabaseCrossReference.Count,
                Convert.ToInt32(tRNADBCount));
            Assert.IsEmpty(tRANsList[0].Allele);
            Assert.IsEmpty(tRANsList[0].Citation);
            Assert.IsEmpty(tRANsList[0].Experiment);
            Assert.IsEmpty(tRANsList[0].Function);
            Assert.IsEmpty(tRANsList[0].GeneSynonym);
            Assert.IsEmpty(tRANsList[0].GenomicMapPosition);
            Assert.IsEmpty(tRANsList[0].Inference);
            Assert.IsEmpty(tRANsList[0].Label);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.TransferRNAs[0].Location),
                tRNAComplement);
            Assert.IsEmpty(tRANsList[0].Note);
            Assert.IsEmpty(tRANsList[0].OldLocusTag);
            Assert.IsFalse(tRANsList[0].Pseudo);
            Assert.IsEmpty(tRANsList[0].StandardName);
            Assert.IsFalse(tRANsList[0].TransSplicing);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the tRNA feature '{0}'",
                tRANsList[0].GeneSymbol.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the tRNA feature '{0}'",
                tRANsList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank mRNA features.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateGenBankmRNAFeatureQualifiers(string nodeName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string mRNAGeneSymbol = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.MRNAGeneSymbol);
            string mRNAComplement = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.MRNAComplement);
            string mRNADBCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CDSCodonStart);
            string mRNAStart = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.MRNAComplementStart);
            string mRNAStdName = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.StandardNameNode);
            string mRNAAllele = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CDSCodonStart);
            string mRNAOperon = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlleleNode);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);

            // Validate tRNA feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<MessengerRNA> mRANsList =
                metadata.Features.MessengerRNAs;

            // Create a copy of mRNA list
            MessengerRNA mRNAClone = mRANsList[0].Clone();
            Assert.AreEqual(mRANsList[0].GeneSymbol.ToString(),
                mRNAGeneSymbol);
            Assert.AreEqual(mRANsList[0].DatabaseCrossReference.Count,
                Convert.ToInt32(mRNADBCount));
            Assert.IsEmpty(mRNAClone.Allele);
            Assert.IsEmpty(mRANsList[0].Citation);
            Assert.IsEmpty(mRANsList[0].Experiment);
            Assert.IsEmpty(mRANsList[0].Function);
            Assert.IsEmpty(mRANsList[0].GeneSynonym);
            Assert.IsEmpty(mRANsList[0].GenomicMapPosition);
            Assert.IsEmpty(mRANsList[0].Inference);
            Assert.IsEmpty(mRANsList[0].Label);
            Assert.IsEmpty(mRANsList[0].LocusTag);
            Assert.IsEmpty(mRANsList[0].Operon);
            Assert.IsEmpty(mRANsList[0].Product);
            Assert.AreEqual(mRANsList[0].Location.Operator.ToString(),
                mRNAComplement);
            Assert.IsNull(mRANsList[0].Location.Separator);
            Assert.AreEqual(mRANsList[0].Location.Start,
                Convert.ToInt32(mRNAStart));
            Assert.IsNull(mRANsList[0].Location.StartData);
            Assert.IsNull(mRANsList[0].Location.EndData);
            Assert.IsEmpty(mRANsList[0].OldLocusTag);
            Assert.IsFalse(mRANsList[0].Pseudo);
            Assert.IsEmpty(mRANsList[0].StandardName);
            Assert.IsFalse(mRANsList[0].TransSplicing);
            Assert.IsEmpty(mRANsList[0].LocusTag);
            Assert.IsEmpty(mRANsList[0].Operon);
            Assert.IsEmpty(mRANsList[0].Product);

            // Create a new mRNA feature using constructor.
            MessengerRNA mRNA = new MessengerRNA(
                metadata.Features.MessengerRNAs[0].Location);

            // Set and validate qualifiers.
            mRNA.GeneSymbol = mRNAGeneSymbol;
            mRNA.Allele = mRNAAllele;
            mRNA.Operon = mRNAOperon;
            mRNA.StandardName = mRNAStdName;

            // Validate properties.
            Assert.AreEqual(mRNA.GeneSymbol, mRNAGeneSymbol);
            Assert.AreEqual(mRNA.Allele, mRNAAllele);
            Assert.AreEqual(mRNA.Operon, mRNAOperon);
            Assert.AreEqual(mRNA.StandardName, mRNAStdName);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
            "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated GeneSymbol'{0}'",
                mRANsList[0].GeneSymbol.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated mRNA count'{0}'",
                mRANsList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank features for medium size sequences.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">DNA,RNA or Protein method</param>
        static void ValidateGenBankFeatures(string nodeName, string methodName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string mRNAFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.mRNACount);
            string exonFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExonCount);
            string intronFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IntronCount);
            string cdsFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CDSCount);
            string allFeaturesCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.GenBankFeaturesCount);
            string expectedCDSKey = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CDSKey);
            string expectedIntronKey = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IntronKey);
            string expectedExonKey = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExonKey);
            string mRNAKey = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.mRNAKey);
            string sourceKeyName = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SourceKey);

            // Parse a file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> sequenceList = parserObj.Parse(filePath);

            // GenBank metadata.
            GenBankMetadata metadata = new GenBankMetadata();
            if (1 == sequenceList.Count)
            {
                metadata =
                    sequenceList[0].Metadata[Constants.GenBank] as GenBankMetadata;
            }
            else
            {
                metadata =
                    sequenceList[1].Metadata[Constants.GenBank] as GenBankMetadata;
            }

            // Validate GenBank Features.
            Assert.AreEqual(metadata.Features.All.Count,
                Convert.ToInt32(allFeaturesCount));
            Assert.AreEqual(metadata.Features.CodingSequences.Count,
                Convert.ToInt32(cdsFeatureCount));
            Assert.AreEqual(metadata.Features.Exons.Count,
                Convert.ToInt32(exonFeatureCount));
            Assert.AreEqual(metadata.Features.Introns.Count,
                Convert.ToInt32(intronFeatureCount));
            Assert.AreEqual(metadata.Features.MessengerRNAs.Count,
                Convert.ToInt32(mRNAFeatureCount));
            Assert.AreEqual(metadata.Features.Attenuators.Count, 0);
            Assert.AreEqual(metadata.Features.CAATSignals.Count, 0);
            Assert.AreEqual(metadata.Features.DisplacementLoops.Count, 0);
            Assert.AreEqual(metadata.Features.Enhancers.Count, 0);

            // Validate GenBank feature list.
            if ((0 == string.Compare(methodName, "DNA", true,
                CultureInfo.CurrentCulture))
                || (0 == string.Compare(methodName, "RNA", true,
                CultureInfo.CurrentCulture)))
            {
                IList<FeatureItem> featureList = metadata.Features.All;
                Assert.AreEqual(featureList[0].Key.ToString(), sourceKeyName);
                Assert.AreEqual(featureList[1].Key.ToString(), expectedCDSKey);
                Assert.AreEqual(featureList[2].Key.ToString(), expectedCDSKey);
                Assert.AreEqual(featureList[10].Key.ToString(), mRNAKey);
                Assert.AreEqual(featureList[12].Key.ToString(), expectedExonKey);
                Assert.AreEqual(featureList[18].Key.ToString(), expectedIntronKey);
                ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
                Console.WriteLine(string.Format(null,
                    "GenBank Features P1: Successfully validated the CDS feature '{0}'",
                    featureList[2].Key.ToString()));
                Console.WriteLine(string.Format(null,
                    "GenBank Features P1: Successfully validated the Intron feature '{0}'",
                    featureList[10].Key.ToString()));
            }
            else
                if ((0 == string.Compare(methodName, "Protein", true, CultureInfo.CurrentCulture)))
                {
                    IList<FeatureItem> featureList = metadata.Features.All;
                    Assert.AreEqual(featureList[10].Key.ToString(), expectedIntronKey);
                    Assert.AreEqual(featureList[18].Key.ToString(), expectedExonKey);
                    ApplicationLog.WriteLine(
                    "GenBank Features P1: Successfully validated the GenBank Features");
                    Console.WriteLine(string.Format(null,
                        "GenBank Features P1: Successfully validated the Intron feature '{0}'",
                        featureList[10].Key.ToString()));
                    Console.WriteLine(string.Format(null,
                        "GenBank Features P1: Successfully validated the Exon feature '{0}'",
                        featureList[1].Key.ToString()));
                }
        }

        /// <summary>
        /// Validate GenBank features for medium size sequences.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateCloneGenBankFeatures(string nodeName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string mRNAFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.mRNACount);
            string exonFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExonCount);
            string intronFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IntronCount);
            string cdsFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CDSCount);
            string allFeaturesCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.GenBankFeaturesCount);

            // Parse a file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> sequenceList = parserObj.Parse(filePath);

            GenBankMetadata metadata =
                sequenceList[0].Metadata[Constants.GenBank] as GenBankMetadata;

            // Validate GenBank Features before Cloning.
            Assert.AreEqual(metadata.Features.All.Count,
                Convert.ToInt32(allFeaturesCount));
            Assert.AreEqual(metadata.Features.CodingSequences.Count,
                Convert.ToInt32(cdsFeatureCount));
            Assert.AreEqual(metadata.Features.Exons.Count,
                Convert.ToInt32(exonFeatureCount));
            Assert.AreEqual(metadata.Features.Introns.Count,
                Convert.ToInt32(intronFeatureCount));
            Assert.AreEqual(metadata.Features.MessengerRNAs.Count,
                Convert.ToInt32(mRNAFeatureCount));
            Assert.AreEqual(metadata.Features.Attenuators.Count, 0);
            Assert.AreEqual(metadata.Features.CAATSignals.Count, 0);
            Assert.AreEqual(metadata.Features.DisplacementLoops.Count, 0);
            Assert.AreEqual(metadata.Features.Enhancers.Count, 0);

            // Clone GenBank Features.
            GenBankMetadata CloneGenBankMetadat = metadata.Clone();

            // Validate cloned GenBank Metadata.
            Assert.AreEqual(CloneGenBankMetadat.Features.All.Count,
                Convert.ToInt32(allFeaturesCount));
            Assert.AreEqual(CloneGenBankMetadat.Features.CodingSequences.Count,
                Convert.ToInt32(cdsFeatureCount));
            Assert.AreEqual(CloneGenBankMetadat.Features.Exons.Count,
                Convert.ToInt32(exonFeatureCount));
            Assert.AreEqual(CloneGenBankMetadat.Features.Introns.Count,
                Convert.ToInt32(intronFeatureCount));
            Assert.AreEqual(CloneGenBankMetadat.Features.MessengerRNAs.Count,
                Convert.ToInt32(mRNAFeatureCount));
            Assert.AreEqual(CloneGenBankMetadat.Features.Attenuators.Count, 0);
            Assert.AreEqual(CloneGenBankMetadat.Features.CAATSignals.Count, 0);
            Assert.AreEqual(CloneGenBankMetadat.Features.DisplacementLoops.Count, 0);
            Assert.AreEqual(CloneGenBankMetadat.Features.Enhancers.Count, 0);

            // Log to GUI NUnit.
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Intron feature '{0}'",
                CloneGenBankMetadat.Features.Introns.Count));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Exons feature '{0}'",
                CloneGenBankMetadat.Features.Exons.Count));
        }

        /// <summary>
        /// Validate GenBank standard features key for medium size sequences..
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="methodName">DNA,RNA or Protein method</param>
        static void ValidateGenBankStandardFeatures(string nodeName,
            string methodName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedCondingSeqCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CDSCount);
            string expectedCDSKey = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CDSKey);
            string expectedIntronKey = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IntronKey);
            string mRNAKey = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.mRNAKey);
            string allFeaturesCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.StandardFeaturesCount);

            // Parse a file.
            ISequenceParser parserObj = new GenBankParser();
            ISequence seq = parserObj.ParseOne(filePath);

            GenBankMetadata metadata =
                seq.Metadata[Constants.GenBank] as GenBankMetadata;

            if ((0 == string.Compare(methodName, "DNA",
                true, CultureInfo.CurrentCulture))
              || (0 == string.Compare(methodName, "RNA",
              true, CultureInfo.CurrentCulture)))
            {
                Assert.AreEqual(StandardFeatureKeys.CodingSequence.ToString(),
                    expectedCDSKey);
                Assert.AreEqual(StandardFeatureKeys.Intron.ToString(),
                    expectedIntronKey);
                Assert.AreEqual(StandardFeatureKeys.MessengerRNA.ToString(),
                    mRNAKey);
                Assert.AreEqual(StandardFeatureKeys.All.Count.ToString(),
                    allFeaturesCount);

                //Log to Nunit GUI.
                Console.WriteLine(string.Format(null,
                    "GenBank Features P1: Successfully validated the standard feature key '{0}'",
                    StandardFeatureKeys.Intron.ToString()));
                Console.WriteLine(string.Format(null,
                    "GenBank Features P1: Successfully validated the standard feature key '{0}'",
                    StandardFeatureKeys.MessengerRNA.ToString()));
            }
            else
            {
                Assert.AreEqual(metadata.Features.CodingSequences.Count.ToString(),
                    expectedCondingSeqCount);
                Assert.AreEqual(StandardFeatureKeys.CodingSequence.ToString(),
                    expectedCDSKey);
                Assert.AreEqual(StandardFeatureKeys.All.Count.ToString(),
                    allFeaturesCount);

                //Log to Nunit GUI.
                Console.WriteLine(string.Format(null,
                    "GenBank Features P1: Successfully validated the standard feature key '{0}'",
                    StandardFeatureKeys.CodingSequence.ToString()));
            }
        }

        /// <summary>
        /// Validate GenBank features with Binary Formatter.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateGenBankFeaturesWithBinaryFormatter(string nodeName)
        {
            Stream fileStream = null;
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string mRNAFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.mRNACount);
            string exonFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExonCount);
            string intronFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IntronCount);
            string cdsFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CDSCount);
            string allFeaturesCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.GenBankFeaturesCount);

            // Opne file in edit mode.
            fileStream = File.Open("GenBankData", FileMode.Create);

            // Create Binary Formatter object. 
            BinaryFormatter formatter = new BinaryFormatter();

            // Parse a file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> sequenceList = parserObj.Parse(filePath);

            GenBankMetadata metadata = new GenBankMetadata();

            // Validate GenBank feature list.
            metadata =
                sequenceList[0].Metadata[Constants.GenBank] as GenBankMetadata;

            // Validate GenBank Features.
            Assert.AreEqual(metadata.Features.All.Count,
                Convert.ToInt32(allFeaturesCount));
            Assert.AreEqual(metadata.Features.CodingSequences.Count,
                Convert.ToInt32(cdsFeatureCount));
            Assert.AreEqual(metadata.Features.Exons.Count,
                Convert.ToInt32(exonFeatureCount));
            Assert.AreEqual(metadata.Features.Introns.Count,
                Convert.ToInt32(intronFeatureCount));
            Assert.AreEqual(metadata.Features.MessengerRNAs.Count,
                Convert.ToInt32(mRNAFeatureCount));
            Assert.AreEqual(metadata.Features.Attenuators.Count, 0);
            Assert.AreEqual(metadata.Features.CAATSignals.Count, 0);
            Assert.AreEqual(metadata.Features.DisplacementLoops.Count, 0);
            Assert.AreEqual(metadata.Features.Enhancers.Count, 0);

            formatter.Serialize(fileStream, metadata);
            fileStream.Seek(0, SeekOrigin.Begin);

            // Deserialize data.
            GenBankMetadata deserializedMetadata =
                (GenBankMetadata)formatter.Deserialize(fileStream);

            // Validate GenBank features after deserialization.
            Assert.AreEqual(deserializedMetadata.Features.All.Count,
                Convert.ToInt32(allFeaturesCount));
            Assert.AreEqual(deserializedMetadata.Features.CodingSequences.Count,
                Convert.ToInt32(cdsFeatureCount));
            Assert.AreEqual(deserializedMetadata.Features.Exons.Count,
                Convert.ToInt32(exonFeatureCount));
            Assert.AreEqual(deserializedMetadata.Features.Introns.Count,
                Convert.ToInt32(intronFeatureCount));
            Assert.AreEqual(deserializedMetadata.Features.MessengerRNAs.Count,
                Convert.ToInt32(mRNAFeatureCount));
            Assert.AreEqual(deserializedMetadata.Features.Attenuators.Count, 0);
            Assert.AreEqual(deserializedMetadata.Features.CAATSignals.Count, 0);
            Assert.AreEqual(deserializedMetadata.Features.DisplacementLoops.Count, 0);
            Assert.AreEqual(deserializedMetadata.Features.Enhancers.Count, 0);

            // Log NUnit GUI.
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the mRNA GenBank features '{0}'",
                deserializedMetadata.Features.MessengerRNAs.Count.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the tRNA GenBank features '{0}'",
                deserializedMetadata.Features.TransferRNAs.Count.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the misc GenBank features '{0}'",
                deserializedMetadata.Features.MiscFeatures.Count.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Promoters GenBank features '{0}'",
                deserializedMetadata.Features.Promoters.Count.ToString()));
            fileStream.Close();
        }

        /// <summary>
        /// Validate GenBank features with specified range.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateGetFeatures(string nodeName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedFirstRangeStartPoint = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FirstRangeStartPoint);
            string expectedSecondRangeStartPoint = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SecondRangeStartPoint);
            string expectedFirstRangeEndPoint = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FirstRangeEndPoint);
            string expectedSecondRangeEndPoint = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SecondRangeEndPoint);
            string expectedCountWithinSecondRange = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.SecondRangeCount);
            string expectedCountWithinFirstRange = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FirstRangeCount);
            string expectedQualifierName = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CDSKey);
            string expectedQualifiers = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.QualifiersCount);
            string firstFeaturesCount = string.Empty;
            string secodFeaturesCount = string.Empty;

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            ISequence seq = parserObj.ParseOne(filePath);

            GenBankMetadata metadata =
                seq.Metadata[Constants.GenBank] as GenBankMetadata;

            // Validate GetFeature within specified range.
            List<FeatureItem> features =
                metadata.GetFeatures(Convert.ToInt32(
                expectedFirstRangeStartPoint), Convert.ToInt32(
                expectedFirstRangeEndPoint));

            firstFeaturesCount = metadata.GetFeatures(Convert.ToInt32(
            expectedFirstRangeStartPoint), Convert.ToInt32(
            expectedFirstRangeEndPoint)).Count.ToString();
            secodFeaturesCount = metadata.GetFeatures(Convert.ToInt32(
            expectedSecondRangeStartPoint), Convert.ToInt32(
            expectedSecondRangeEndPoint)).Count.ToString();

            // Validate GenBank features count within specified range.
            Assert.AreEqual(firstFeaturesCount, expectedCountWithinFirstRange);
            Assert.AreEqual(secodFeaturesCount, expectedCountWithinSecondRange);
            Assert.AreEqual(features.Count.ToString(), firstFeaturesCount);
            Assert.AreEqual(features[1].Qualifiers.Count.ToString(),
                expectedQualifiers);
            Assert.AreEqual(features[1].Key.ToString(), expectedQualifierName);

            // Log NUnit GUI.
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Get GenBank features '{0}'",
                firstFeaturesCount));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Get GenBank features '{0}'",
                secodFeaturesCount));
        }

        /// <summary>
        /// Validate GenBank Citation referenced present in GenBank Metadata.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="featureName">Feature Name</param>
        static void ValidateCitationReferenced(string nodeName,
            FeatureGroup featureName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedCitationReferenced = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.citationReferencedCount);
            string expectedmRNACitationReferenced = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.citationReferencedCount);
            string expectedExonACitationReferenced = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExonCitationReferencedCount);
            string expectedIntronCitationReferenced = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IntronCitationReferencedCount);
            string expectedpromoterCitationReferenced = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.PromotersCitationReferencedCount);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            ISequence seq = parserObj.ParseOne(filePath);

            GenBankMetadata metadata =
                seq.Metadata[Constants.GenBank] as GenBankMetadata;

            List<CitationReference> citationReferenceList;

            // Get a list citationReferenced present in GenBank file.
            switch (featureName)
            {
                case FeatureGroup.CDS:
                    FeatureItem cds =
                        metadata.Features.CodingSequences[0];
                    citationReferenceList =
                        metadata.GetCitationsReferredInFeature(cds);

                    // Validate citation referenced present in CDS features.
                    Assert.AreEqual(citationReferenceList.Count.ToString(),
                        expectedCitationReferenced);

                    //Log Nunit GUI.
                    Console.WriteLine(string.Format(null,
                        "GenBank Features P1: Successfully validated citation referenced '{0}'",
                        citationReferenceList.Count.ToString()));
                    break;
                case FeatureGroup.mRNA:
                    FeatureItem mRNA = metadata.Features.MessengerRNAs[0];
                    citationReferenceList = metadata.GetCitationsReferredInFeature(mRNA);

                    // Validate citation referenced present in mRNA features.
                    Assert.AreEqual(citationReferenceList.Count.ToString(), expectedmRNACitationReferenced);

                    //Log Nunit GUI.
                    Console.WriteLine(string.Format(null,
                        "GenBank Features P1: Successfully validated citation referenced '{0}'",
                        citationReferenceList.Count.ToString()));
                    break;
                case FeatureGroup.Exon:
                    FeatureItem exon = metadata.Features.Exons[0];
                    citationReferenceList =
                        metadata.GetCitationsReferredInFeature(exon);

                    // Validate citation referenced present in Exons features.
                    Assert.AreEqual(citationReferenceList.Count.ToString(),
                        expectedExonACitationReferenced);

                    //Log Nunit GUI.
                    Console.WriteLine(string.Format(null,
                        "GenBank Features P1: Successfully validated citation referenced '{0}'",
                        citationReferenceList.Count.ToString()));
                    break;
                case FeatureGroup.Intron:
                    FeatureItem introns = metadata.Features.Introns[0];
                    citationReferenceList =
                        metadata.GetCitationsReferredInFeature(introns);

                    // Validate citation referenced present in Introns features.
                    Assert.AreEqual(citationReferenceList.Count.ToString(),
                        expectedIntronCitationReferenced);

                    //Log Nunit GUI.
                    Console.WriteLine(string.Format(null,
                        "GenBank Features P1: Successfully validated citation referenced '{0}'",
                        citationReferenceList.Count.ToString()));
                    break;
                case FeatureGroup.Promoter:
                    FeatureItem promoter = metadata.Features.Promoters[0];
                    citationReferenceList =
                        metadata.GetCitationsReferredInFeature(promoter);

                    // Validate citation referenced present in Promoters features.
                    Assert.AreEqual(citationReferenceList.Count.ToString(),
                        expectedpromoterCitationReferenced);

                    //Log Nunit GUI.
                    Console.WriteLine(string.Format(null,
                        "GenBank Features P1: Successfully validated citation referenced '{0}'",
                        citationReferenceList.Count.ToString()));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Validate GenBank miscFeatures features.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateGenBankMiscFeatureQualifiers(string nodeName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string miscFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.MiscFeatureCount);
            string location = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Location);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);

            // Validate Misc feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<MiscFeature> miscFeatureList = metadata.Features.MiscFeatures;
            LocationBuilder locBuilder = new LocationBuilder();

            // Create copy of misc feature and validate all qualifiers
            MiscFeature cloneMiscFeatureList =
                miscFeatureList[0].Clone();
            Assert.AreEqual(miscFeatureList.Count.ToString(),
                miscFeatureCount);
            Assert.IsEmpty(cloneMiscFeatureList.Allele);
            Assert.IsEmpty(cloneMiscFeatureList.Citation);
            Assert.IsEmpty(cloneMiscFeatureList.Experiment);
            Assert.IsEmpty(cloneMiscFeatureList.Function);
            Assert.IsEmpty(miscFeatureList[0].GeneSynonym);
            Assert.IsEmpty(miscFeatureList[0].GenomicMapPosition);
            Assert.IsEmpty(miscFeatureList[0].Inference);
            Assert.IsEmpty(miscFeatureList[0].Label);
            Assert.IsEmpty(miscFeatureList[0].OldLocusTag);
            Assert.IsFalse(miscFeatureList[0].Pseudo);
            Assert.IsEmpty(miscFeatureList[0].StandardName);
            Assert.IsEmpty(miscFeatureList[0].Product);
            Assert.IsEmpty(miscFeatureList[0].Number);
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.MiscFeatures[0].Location), location);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the misc feature '{0}'",
                miscFeatureList[0].GeneSymbol.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the misc feature '{0}'",
                miscFeatureList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Exon feature qualifiers.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateGenBankExonFeatureQualifiers(string nodeName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedExonFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExonCount);
            string expectedExonGeneSymbol = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExonGeneSymbol);
            string expectedExonNumber = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExonNumber);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);

            // Validate Misc feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<Exon> exonFeatureList = metadata.Features.Exons;
            Assert.AreEqual(exonFeatureList.Count.ToString(),
                expectedExonFeatureCount);
            Assert.AreEqual(exonFeatureList[0].GeneSymbol,
                expectedExonGeneSymbol);
            Assert.AreEqual(exonFeatureList[0].Number,
                expectedExonNumber);
            Assert.IsEmpty(exonFeatureList[0].Allele);
            Assert.IsEmpty(exonFeatureList[0].Citation);
            Assert.IsEmpty(exonFeatureList[0].Experiment);
            Assert.IsEmpty(exonFeatureList[0].Function);
            Assert.IsEmpty(exonFeatureList[0].GeneSynonym);
            Assert.IsEmpty(exonFeatureList[0].GenomicMapPosition);
            Assert.IsEmpty(exonFeatureList[0].Inference);
            Assert.IsEmpty(exonFeatureList[0].Label);
            Assert.IsEmpty(exonFeatureList[0].OldLocusTag);
            Assert.IsFalse(exonFeatureList[0].Pseudo);
            Assert.IsEmpty(exonFeatureList[0].StandardName);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Exon feature '{0}'",
                exonFeatureList[0].GeneSymbol.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Exon feature '{0}'",
                exonFeatureList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Intron feature qualifiers.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateGenBankIntronFeatureQualifiers(string nodeName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedIntronGeneSymbol = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IntronGeneSymbol);
            string expectedIntronComplement = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IntronComplement);
            string expectedIntronNumber = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IntronNumber);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);

            // Validate Misc feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<Intron> intronFeatureList =
                metadata.Features.Introns;
            Assert.AreEqual(intronFeatureList[0].GeneSymbol,
                expectedIntronGeneSymbol);
            Assert.AreEqual(intronFeatureList[0].Location.Operator.ToString(),
                expectedIntronComplement);
            Assert.AreEqual(intronFeatureList[0].Number,
                expectedIntronNumber);
            Assert.IsEmpty(intronFeatureList[0].Allele);
            Assert.IsEmpty(intronFeatureList[0].Citation);
            Assert.IsEmpty(intronFeatureList[0].Experiment);
            Assert.IsEmpty(intronFeatureList[0].Function);
            Assert.IsEmpty(intronFeatureList[0].GeneSynonym);
            Assert.IsEmpty(intronFeatureList[0].GenomicMapPosition);
            Assert.IsEmpty(intronFeatureList[0].Inference);
            Assert.IsEmpty(intronFeatureList[0].Label);
            Assert.IsEmpty(intronFeatureList[0].OldLocusTag);
            Assert.IsFalse(intronFeatureList[0].Pseudo);
            Assert.IsEmpty(intronFeatureList[0].StandardName);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Intron feature '{0}'",
                intronFeatureList[0].GeneSymbol.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Intron feature '{0}'",
                intronFeatureList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Promoter feature qualifiers.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateGenBankPromoterFeatureQualifiers(string nodeName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedPromoterComplement = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.PromoterComplement);
            string expectedPromoterCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.PromoterCount);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate Misc feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<Promoter> promotersFeatureList =
                metadata.Features.Promoters;
            Assert.AreEqual(locBuilder.GetLocationString(
                metadata.Features.Promoters[0].Location),
                expectedPromoterComplement);
            Assert.AreEqual(promotersFeatureList.Count.ToString(),
                expectedPromoterCount);
            Assert.IsEmpty(promotersFeatureList[0].GeneSymbol);
            Assert.IsEmpty(promotersFeatureList[0].Allele);
            Assert.IsEmpty(promotersFeatureList[0].Citation);
            Assert.IsEmpty(promotersFeatureList[0].Experiment);
            Assert.IsEmpty(promotersFeatureList[0].Function);
            Assert.IsEmpty(promotersFeatureList[0].GeneSynonym);
            Assert.IsEmpty(promotersFeatureList[0].GenomicMapPosition);
            Assert.IsEmpty(promotersFeatureList[0].Inference);
            Assert.IsEmpty(promotersFeatureList[0].Label);
            Assert.IsEmpty(promotersFeatureList[0].OldLocusTag);
            Assert.IsFalse(promotersFeatureList[0].Pseudo);
            Assert.IsEmpty(promotersFeatureList[0].StandardName);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Promoter feature '{0}'",
                promotersFeatureList[0].GeneSymbol.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Promoter feature '{0}'",
                promotersFeatureList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Variation feature qualifiers.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateGenBankVariationFeatureQualifiers(string nodeName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedVariationCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.VarationCount);
            string expectedVariationReplace = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.VariationReplace);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);

            // Validate Misc feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            List<Variation> variationFeatureList =
                metadata.Features.Variations;
            Assert.AreEqual(variationFeatureList.Count.ToString(),
                expectedVariationCount);
            Assert.AreEqual(variationFeatureList[0].Replace,
                expectedVariationReplace);
            Assert.IsEmpty(variationFeatureList[0].GeneSymbol);
            Assert.IsEmpty(variationFeatureList[0].Allele);
            Assert.IsEmpty(variationFeatureList[0].Citation);
            Assert.IsEmpty(variationFeatureList[0].Experiment);
            Assert.IsEmpty(variationFeatureList[0].GeneSynonym);
            Assert.IsEmpty(variationFeatureList[0].GenomicMapPosition);
            Assert.IsEmpty(variationFeatureList[0].Inference);
            Assert.IsEmpty(variationFeatureList[0].Label);
            Assert.IsEmpty(variationFeatureList[0].OldLocusTag);
            Assert.IsEmpty(variationFeatureList[0].StandardName);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Variation feature '{0}'",
                variationFeatureList[0].Replace.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Variation feature '{0}'",
                variationFeatureList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Misc difference feature qualifiers.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateGenBankMiscDiffFeatureQualifiers(string nodeName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedMiscDiffCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.MiscDiffCount);
            string expectedGeneSymbol = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.GeneSymbol);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);

            // Validate Protein feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[1].Metadata[Constants.GenBank];
            List<MiscDifference> miscDifferenceFeatureList =
                metadata.Features.MiscDifferences;
            Assert.AreEqual(miscDifferenceFeatureList.Count.ToString(),
                expectedMiscDiffCount);
            Assert.AreEqual(miscDifferenceFeatureList[0].GeneSymbol,
                expectedGeneSymbol);
            Assert.IsEmpty(miscDifferenceFeatureList[0].Allele);
            Assert.IsEmpty(miscDifferenceFeatureList[0].Citation);
            Assert.IsEmpty(miscDifferenceFeatureList[0].Experiment);
            Assert.IsEmpty(miscDifferenceFeatureList[0].GeneSynonym);
            Assert.IsEmpty(miscDifferenceFeatureList[0].GenomicMapPosition);
            Assert.IsEmpty(miscDifferenceFeatureList[0].Inference);
            Assert.IsEmpty(miscDifferenceFeatureList[0].Label);
            Assert.IsEmpty(miscDifferenceFeatureList[0].OldLocusTag);
            Assert.IsEmpty(miscDifferenceFeatureList[0].StandardName);
            Assert.IsEmpty(miscDifferenceFeatureList[0].Replace);
            Assert.IsEmpty(miscDifferenceFeatureList[0].Phenotype);
            Assert.IsEmpty(miscDifferenceFeatureList[0].OldLocusTag);
            Assert.IsEmpty(miscDifferenceFeatureList[0].LocusTag);
            Assert.IsEmpty(miscDifferenceFeatureList[0].Compare);
            Assert.IsEmpty(miscDifferenceFeatureList[0].DatabaseCrossReference);
            Assert.IsEmpty(miscDifferenceFeatureList[0].ClonedFrom);


            // Create a new MiscDiff feature using constructor.
            MiscDifference miscDiffWithLoc = new MiscDifference(
                metadata.Features.MiscDifferences[0].Location);

            // Set and validate qualifiers.
            miscDiffWithLoc.GeneSymbol = expectedGeneSymbol;
            Assert.AreEqual(miscDiffWithLoc.GeneSymbol,
                expectedGeneSymbol);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Misc Difference feature '{0}'",
                miscDifferenceFeatureList[0].GeneSymbol.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Misc Difference feature '{0}'",
                miscDifferenceFeatureList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Protein binding feature qualifiers.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateGenBankProteinBindingFeatureQualifiers(string nodeName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
            nodeName, Constants.FilePathNode);
            string expectedProteinBindingCount =
                Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ProteinBindingCount);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);

            // Validate ProteinBinding feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[1].Metadata[Constants.GenBank];
            List<ProteinBindingSite> proteinBindingFeatureList =
                metadata.Features.ProteinBindingSites;
            Assert.AreEqual(proteinBindingFeatureList.Count.ToString(),
                expectedProteinBindingCount);
            Assert.IsEmpty(proteinBindingFeatureList[0].GeneSymbol);
            Assert.IsEmpty(proteinBindingFeatureList[0].Allele);
            Assert.IsEmpty(proteinBindingFeatureList[0].Citation);
            Assert.IsEmpty(proteinBindingFeatureList[0].Experiment);
            Assert.IsEmpty(proteinBindingFeatureList[0].GeneSynonym);
            Assert.IsEmpty(proteinBindingFeatureList[0].GenomicMapPosition);
            Assert.IsEmpty(proteinBindingFeatureList[0].Inference);
            Assert.IsEmpty(proteinBindingFeatureList[0].Label);
            Assert.IsEmpty(proteinBindingFeatureList[0].OldLocusTag);
            Assert.IsEmpty(proteinBindingFeatureList[0].StandardName);

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Protein Binding feature '{0}'",
                proteinBindingFeatureList[0].BoundMoiety.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBank Features P1: Successfully validated the Protein Binding feature '{0}'",
                proteinBindingFeatureList.Count.ToString()));
        }

        /// <summary>
        /// Validate GenBank Features clonning.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="featureName">Name of the GenBank feature</param>
        static void ValidateGenBankFeaturesClonning(string nodeName, FeatureGroup featureName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedExonFeatureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExonCount);
            string expectedExonGeneSymbol = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExonGeneSymbol);
            string expectedExonNumber = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExonNumber);
            string expectedMiscDiffCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.MiscQualifiersCount);
            string expectedGeneSymbol = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.GeneSymbol);
            string expectedIntronGeneSymbol = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IntronGeneSymbol);
            string expectedIntronNumber = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.IntronNumber);
            string expectedVariationReplace = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.VariationReplace);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            ISequence seq = parserObj.ParseOne(filePath);

            GenBankMetadata metadata =
                seq.Metadata[Constants.GenBank] as GenBankMetadata;

            // Validate cloned GenBank feature.
            switch (featureName)
            {
                case FeatureGroup.Exon:
                    List<Exon> exonFeatureList = metadata.Features.Exons;

                    // Validate Exon feature before clonning.
                    Assert.AreEqual(exonFeatureList.Count.ToString(),
                        expectedExonFeatureCount);
                    Assert.AreEqual(exonFeatureList[0].GeneSymbol,
                        expectedExonGeneSymbol);
                    Assert.AreEqual(exonFeatureList[0].Number,
                        expectedExonNumber);

                    // Clone Exon feature.
                    Exon clonedExons = exonFeatureList[0].Clone();

                    // Validate Exon feature after clonning.
                    Assert.AreEqual(clonedExons.GeneSymbol,
                        expectedExonGeneSymbol);
                    Assert.AreEqual(clonedExons.Number,
                        expectedExonNumber);
                    Console.WriteLine(string.Format(null,
                        "GenBank Features P1: Successfully validated the Exons Qualifiers '{0}'",
                        clonedExons.Location.ToString()));
                    break;
                case FeatureGroup.miscDifference:
                    // Validate Misc Difference feature before clonning.
                    List<MiscDifference> miscDifferenceFeatureList =
                        metadata.Features.MiscDifferences;
                    Assert.AreEqual(miscDifferenceFeatureList.Count.ToString(),
                        expectedMiscDiffCount);
                    Assert.AreEqual(miscDifferenceFeatureList[0].GeneSymbol,
                        expectedGeneSymbol);

                    // Clone Misc Difference feature 
                    MiscDifference clonedMiscDifferences =
                        miscDifferenceFeatureList[0].Clone();

                    // Validate Misc Difference feature  after clonning.
                    Assert.AreEqual(clonedMiscDifferences.GeneSymbol,
                        expectedGeneSymbol);
                    Console.WriteLine(string.Format(null,
                        "GenBank Features P1: Successfully validated the Misc Difference Qualifiers '{0}'",
                        clonedMiscDifferences.GeneSymbol));
                    break;
                case FeatureGroup.Intron:
                    // Validate Intron feature before clonning.
                    List<Intron> intronFeatureList = metadata.Features.Introns;
                    Assert.AreEqual(intronFeatureList[0].GeneSymbol,
                        expectedIntronGeneSymbol);
                    Assert.AreEqual(intronFeatureList[0].Number,
                        expectedIntronNumber);

                    // Clone Intron feature.
                    Intron clonedIntrons = intronFeatureList[0].Clone();

                    // Validate Intron feature after clonning.
                    Assert.AreEqual(clonedIntrons.GeneSymbol,
                        expectedIntronGeneSymbol);
                    Assert.AreEqual(clonedIntrons.Number,
                        expectedIntronNumber);
                    Console.WriteLine(string.Format(null,
                        "GenBank Features P1: Successfully validated the Introns '{0}'",
                        clonedIntrons.Location.ToString()));
                    break;
                case FeatureGroup.variation:
                    // Validate Variation feature before clonning.
                    List<Variation> variationFeatureList =
                        metadata.Features.Variations;
                    Assert.AreEqual(variationFeatureList[0].Replace,
                        expectedVariationReplace);

                    // Clone Variation feature.
                    Variation clonedVariations =
                        variationFeatureList[0].Clone();

                    // Validate Intron feature after clonning.
                    Assert.AreEqual(clonedVariations.Replace,
                        expectedVariationReplace);
                    Console.WriteLine(string.Format(null,
                        "GenBank Features P1: Successfully validated the Variations '{0}'",
                        clonedVariations.Replace));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Validate General GenBank Features 
        /// </summary>
        /// <param name="nodeName">xml node name for different feature.</param>
        /// <param name="featureName">Name of the GenBank feature</param>
        static void ValidateGeneralGenBankFeatureQualifiers(string nodeName, FeatureGroup featureName)
        {
            // Get Values from XML node.
            string filePath = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FilePathNode);
            string expectedLocation = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Location);
            string expectedAllele = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlleleNode);
            string featureCount = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.QualifierCount);
            string expectedDbReference = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.DbReferenceNode);
            string geneSymbol = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.GeneSymbol);
            string expectedCitation = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CitationNode);
            string expectedExperiment = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExperimentNode);
            string expectedFunction = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.FunctionNode);
            string expectedGeneSynonym = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.GeneSynonymNode);
            string expectedInference = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.InferenceNode);
            string expectedLabel = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.LabelNode);
            string expectedLocusTag = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.LocusTagNode);
            string expectedNote = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Note);
            string expectedOldLocusTag = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.OldLocusTagNode);
            string expectedMap = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.GenbankMapNode);
            string expectedNonCodingRnaClass = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.NonCodingRnaClassNode);
            string expectedTranslation = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.GenbankTranslationNode);
            string expectedCodonStart = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.CodonStartNode);

            // Parse a GenBank file.
            ISequenceParser parserObj = new GenBankParser();
            IList<ISequence> seqList = parserObj.Parse(filePath);
            LocationBuilder locBuilder = new LocationBuilder();

            // Validate GenBank feature all qualifiers.
            GenBankMetadata metadata =
                (GenBankMetadata)seqList[0].Metadata[Constants.GenBank];
            switch (featureName)
            {
                case FeatureGroup.MiscStructure:
                    List<MiscStructure> miscStrFeatureList =
                        metadata.Features.MiscStructures;

                    // Create a copy of Misc structure.
                    MiscStructure cloneMiscStr = miscStrFeatureList[0].Clone();

                    // Validate MiscStructure qualifiers.
                    Assert.AreEqual(miscStrFeatureList.Count.ToString(), featureCount);
                    Assert.AreEqual(cloneMiscStr.GeneSymbol, geneSymbol);
                    Assert.AreEqual(cloneMiscStr.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(miscStrFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(miscStrFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(miscStrFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(miscStrFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(miscStrFeatureList[0].GeneSynonym[0],
                        expectedGeneSynonym);
                    Assert.AreEqual(miscStrFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(miscStrFeatureList[0].Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.MiscStructures[0].Location),
                        expectedLocation);
                    Assert.AreEqual(miscStrFeatureList[0].Note[0],
                        expectedNote);
                    Assert.AreEqual(miscStrFeatureList[0].OldLocusTag[0],
                        expectedOldLocusTag);
                    Assert.AreEqual(miscStrFeatureList[0].LocusTag[0],
                        expectedLocusTag);
                    Assert.AreEqual(miscStrFeatureList[0].Function[0],
                        expectedFunction);
                    Assert.IsEmpty(miscStrFeatureList[0].StandardName);

                    // Create a new MiscStructure and validate the same.
                    MiscStructure miscStructure = new MiscStructure(expectedLocation);
                    MiscStructure miscStructureWithILoc = new MiscStructure(
                        metadata.Features.TransitPeptides[0].Location);

                    // Set qualifiers and validate them.
                    miscStructure.Allele = expectedAllele;
                    miscStructure.GeneSymbol = geneSymbol;
                    miscStructureWithILoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(miscStructure.GeneSymbol, geneSymbol);
                    Assert.AreEqual(miscStructure.Allele, expectedAllele);
                    Assert.AreEqual(miscStructureWithILoc.GenomicMapPosition,
                        expectedMap);
                    break;
                case FeatureGroup.TrnsitPeptide:
                    List<TransitPeptide> tansitPeptideFeatureList =
                        metadata.Features.TransitPeptides;

                    // Create a copy of transit peptide features.
                    TransitPeptide cloneTransit = tansitPeptideFeatureList[0].Clone();

                    // Validate transit peptide qualifiers.
                    Assert.AreEqual(tansitPeptideFeatureList.Count.ToString(), featureCount);
                    Assert.AreEqual(cloneTransit.GeneSymbol, geneSymbol);
                    Assert.AreEqual(cloneTransit.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(tansitPeptideFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(tansitPeptideFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(tansitPeptideFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(tansitPeptideFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(tansitPeptideFeatureList[0].GeneSynonym[0],
                        expectedGeneSynonym);
                    Assert.AreEqual(tansitPeptideFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(tansitPeptideFeatureList[0].Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.TransitPeptides[0].Location),
                        expectedLocation);
                    Assert.AreEqual(tansitPeptideFeatureList[0].Note[0],
                        expectedNote);
                    Assert.AreEqual(tansitPeptideFeatureList[0].OldLocusTag[0],
                        expectedOldLocusTag);
                    Assert.AreEqual(tansitPeptideFeatureList[0].LocusTag[0],
                        expectedLocusTag);
                    Assert.AreEqual(tansitPeptideFeatureList[0].Function[0],
                        expectedFunction);

                    // Create a new TransitPeptide and validate the same.
                    TransitPeptide tPeptide = new TransitPeptide(expectedLocation);
                    TransitPeptide tPeptideWithILoc = new TransitPeptide(
                        metadata.Features.TransitPeptides[0].Location);

                    // Set qualifiers and validate them.
                    tPeptide.Allele = expectedAllele;
                    tPeptide.GeneSymbol = geneSymbol;
                    tPeptideWithILoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(tPeptide.GeneSymbol, geneSymbol);
                    Assert.AreEqual(tPeptide.Allele, expectedAllele);
                    Assert.AreEqual(tPeptideWithILoc.GenomicMapPosition,
                        expectedMap);

                    break;
                case FeatureGroup.StemLoop:
                    List<StemLoop> sLoopFeatureList = metadata.Features.StemLoops;

                    // Create a copy of StemLoop feature.
                    StemLoop cloneSLoop = sLoopFeatureList[0].Clone();

                    // Validate transit peptide qualifiers.
                    Assert.AreEqual(sLoopFeatureList.Count.ToString(),
                        featureCount);
                    Assert.AreEqual(cloneSLoop.GeneSymbol, geneSymbol);
                    Assert.AreEqual(cloneSLoop.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(sLoopFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(sLoopFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(sLoopFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(sLoopFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(sLoopFeatureList[0].GeneSynonym[0],
                        expectedGeneSynonym);
                    Assert.AreEqual(sLoopFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(sLoopFeatureList[0].Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.StemLoops[0].Location),
                        expectedLocation);
                    Assert.AreEqual(sLoopFeatureList[0].Note[0],
                        expectedNote);
                    Assert.AreEqual(sLoopFeatureList[0].OldLocusTag[0],
                        expectedOldLocusTag);
                    Assert.AreEqual(sLoopFeatureList[0].LocusTag[0],
                        expectedLocusTag);
                    Assert.AreEqual(sLoopFeatureList[0].Function[0],
                        expectedFunction);
                    Assert.IsEmpty(sLoopFeatureList[0].Operon);
                    Assert.IsEmpty(sLoopFeatureList[0].StandardName);

                    // Create a new StemLoop and validate the same.
                    StemLoop stemLoop = new StemLoop(expectedLocation);
                    StemLoop stemLoopWithILoc = new StemLoop(
                        metadata.Features.StemLoops[0].Location);

                    // Set qualifiers and validate them.
                    stemLoop.Allele = expectedAllele;
                    stemLoop.GeneSymbol = geneSymbol;
                    stemLoopWithILoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(stemLoop.GeneSymbol, geneSymbol);
                    Assert.AreEqual(stemLoop.Allele, expectedAllele);
                    Assert.AreEqual(stemLoopWithILoc.GenomicMapPosition,
                        expectedMap);
                    break;
                case FeatureGroup.ModifiedBase:
                    List<ModifiedBase> modifiedBaseFeatureList =
                        metadata.Features.ModifiedBases;

                    // Create a copy of Modified base feature.
                    ModifiedBase cloneModifiedBase = modifiedBaseFeatureList[0].Clone();

                    // Validate Modified Base qualifiers.
                    Assert.AreEqual(modifiedBaseFeatureList.Count.ToString(),
                        featureCount);
                    Assert.AreEqual(cloneModifiedBase.GeneSymbol,
                        geneSymbol);
                    Assert.AreEqual(cloneModifiedBase.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(modifiedBaseFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(modifiedBaseFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(modifiedBaseFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(modifiedBaseFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(modifiedBaseFeatureList[0].GeneSynonym[0],
                        expectedGeneSynonym);
                    Assert.AreEqual(modifiedBaseFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(modifiedBaseFeatureList[0].Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.ModifiedBases[0].Location),
                        expectedLocation);
                    Assert.AreEqual(modifiedBaseFeatureList[0].Note[0],
                        expectedNote);
                    Assert.AreEqual(modifiedBaseFeatureList[0].OldLocusTag[0],
                        expectedOldLocusTag);
                    Assert.AreEqual(modifiedBaseFeatureList[0].LocusTag[0],
                        expectedLocusTag);
                    Assert.IsEmpty(modifiedBaseFeatureList[0].ModifiedNucleotideBase);

                    // Create a new ModifiedBase and validate the same.
                    ModifiedBase modifiedBase = new ModifiedBase(expectedLocation);
                    ModifiedBase modifiedBaseWithILoc = new ModifiedBase(
                        metadata.Features.ModifiedBases[0].Location);

                    // Set qualifiers and validate them.
                    modifiedBase.Allele = expectedAllele;
                    modifiedBase.GeneSymbol = geneSymbol;
                    modifiedBaseWithILoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(modifiedBase.GeneSymbol, geneSymbol);
                    Assert.AreEqual(modifiedBase.Allele, expectedAllele);
                    Assert.AreEqual(modifiedBaseWithILoc.GenomicMapPosition,
                        expectedMap);
                    break;
                case FeatureGroup.PrecursorRNA:
                    List<PrecursorRNA> precursorRNAFeatureList =
                        metadata.Features.PrecursorRNAs;

                    // Create a copy of Precursor RNA feature.
                    PrecursorRNA clonePrecursorRNA =
                        precursorRNAFeatureList[0].Clone();

                    // Validate Precursor RNA qualifiers.
                    Assert.AreEqual(precursorRNAFeatureList.Count.ToString(),
                        featureCount);
                    Assert.AreEqual(clonePrecursorRNA.GeneSymbol,
                        geneSymbol);
                    Assert.AreEqual(clonePrecursorRNA.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(precursorRNAFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(precursorRNAFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(precursorRNAFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(precursorRNAFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(precursorRNAFeatureList[0].GeneSynonym[0],
                        expectedGeneSynonym);
                    Assert.AreEqual(precursorRNAFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(precursorRNAFeatureList[0].Label, expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.PrecursorRNAs[0].Location),
                        expectedLocation);
                    Assert.AreEqual(precursorRNAFeatureList[0].Note[0],
                        expectedNote);
                    Assert.AreEqual(precursorRNAFeatureList[0].OldLocusTag[0],
                        expectedOldLocusTag);
                    Assert.AreEqual(precursorRNAFeatureList[0].LocusTag[0],
                        expectedLocusTag);
                    Assert.AreEqual(precursorRNAFeatureList[0].Function[0],
                        expectedFunction);
                    Assert.IsEmpty(precursorRNAFeatureList[0].StandardName);
                    Assert.IsEmpty(precursorRNAFeatureList[0].Product);
                    Assert.IsEmpty(precursorRNAFeatureList[0].Operon);
                    Assert.IsFalse(precursorRNAFeatureList[0].TransSplicing);

                    // Create a new Precursor RNA and validate the same.
                    PrecursorRNA precursorRNA = new PrecursorRNA(expectedLocation);
                    PrecursorRNA precursorRNAWithILoc = new PrecursorRNA(
                        metadata.Features.PrecursorRNAs[0].Location);

                    // Set qualifiers and validate them.
                    precursorRNA.Allele = expectedAllele;
                    precursorRNA.GeneSymbol = geneSymbol;
                    precursorRNAWithILoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(precursorRNA.GeneSymbol, geneSymbol);
                    Assert.AreEqual(precursorRNA.Allele, expectedAllele);
                    Assert.AreEqual(precursorRNAWithILoc.GenomicMapPosition, expectedMap);
                    break;
                case FeatureGroup.PolySite:
                    List<PolyASite> polySiteFeatureList = metadata.Features.PolyASites;

                    // Create a copy of Poly site feature.
                    PolyASite clonePolySite = polySiteFeatureList[0].Clone();

                    // Validate Poly site qualifiers.
                    Assert.AreEqual(polySiteFeatureList.Count.ToString(),
                        featureCount);
                    Assert.AreEqual(clonePolySite.GeneSymbol, geneSymbol);
                    Assert.AreEqual(clonePolySite.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(polySiteFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(polySiteFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(polySiteFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(polySiteFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(polySiteFeatureList[0].GeneSynonym[0],
                        expectedGeneSynonym);
                    Assert.AreEqual(polySiteFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(polySiteFeatureList[0].Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.PolyASites[0].Location),
                        expectedLocation);
                    Assert.AreEqual(polySiteFeatureList[0].Note[0],
                        expectedNote);
                    Assert.AreEqual(polySiteFeatureList[0].OldLocusTag[0],
                        expectedOldLocusTag);
                    Assert.AreEqual(polySiteFeatureList[0].LocusTag[0],
                        expectedLocusTag);

                    // Create a new PolySite and validate the same.
                    PolyASite polySite = new PolyASite(expectedLocation);
                    PolyASite polySiteWithILoc = new PolyASite(
                        metadata.Features.PolyASites[0].Location);

                    // Set qualifiers and validate them.
                    polySite.Allele = expectedAllele;
                    polySite.GeneSymbol = geneSymbol;
                    polySiteWithILoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(polySite.GeneSymbol, geneSymbol);
                    Assert.AreEqual(polySite.Allele, expectedAllele);
                    Assert.AreEqual(polySiteWithILoc.GenomicMapPosition, expectedMap);
                    break;
                case FeatureGroup.MiscBinding:
                    List<MiscBinding> miscBindingFeatureList = metadata.Features.MiscBindings;

                    // Create a copy of Misc Binding feature.
                    MiscBinding cloneMiscBinding = miscBindingFeatureList[0].Clone();

                    // Validate Misc Binding qualifiers.
                    Assert.AreEqual(miscBindingFeatureList.Count.ToString(),
                        featureCount);
                    Assert.AreEqual(cloneMiscBinding.GeneSymbol,
                        geneSymbol);
                    Assert.AreEqual(cloneMiscBinding.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(miscBindingFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(miscBindingFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(miscBindingFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(miscBindingFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(miscBindingFeatureList[0].GeneSynonym[0],
                        expectedGeneSynonym);
                    Assert.AreEqual(miscBindingFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(miscBindingFeatureList[0].Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.MiscBindings[0].Location),
                        expectedLocation);
                    Assert.AreEqual(miscBindingFeatureList[0].Note[0],
                        expectedNote);
                    Assert.AreEqual(miscBindingFeatureList[0].OldLocusTag[0],
                        expectedOldLocusTag);
                    Assert.AreEqual(miscBindingFeatureList[0].LocusTag[0],
                        expectedLocusTag);

                    // Create a new MiscBinding and validate the same.
                    MiscBinding miscBinding = new MiscBinding(expectedLocation);
                    MiscBinding miscBindingWithILoc = new MiscBinding(
                        metadata.Features.MiscBindings[0].Location);

                    // Set qualifiers and validate them.
                    miscBinding.Allele = expectedAllele;
                    miscBinding.GeneSymbol = geneSymbol;
                    miscBindingWithILoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(miscBinding.GeneSymbol, geneSymbol);
                    Assert.AreEqual(miscBinding.Allele, expectedAllele);
                    Assert.AreEqual(miscBindingWithILoc.GenomicMapPosition,
                        expectedMap);
                    break;
                case FeatureGroup.Enhancer:
                    List<Enhancer> enhancerFeatureList = metadata.Features.Enhancers;

                    // Create a copy of Enhancer feature.
                    Enhancer cloneEnhancer = enhancerFeatureList[0].Clone();

                    // Validate Enhancer qualifiers.
                    Assert.AreEqual(enhancerFeatureList.Count.ToString(),
                        featureCount);
                    Assert.AreEqual(cloneEnhancer.GeneSymbol,
                        geneSymbol);
                    Assert.AreEqual(cloneEnhancer.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(enhancerFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(enhancerFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(enhancerFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(enhancerFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(enhancerFeatureList[0].GeneSynonym[0],
                        expectedGeneSynonym);
                    Assert.AreEqual(enhancerFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(enhancerFeatureList[0].Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.Enhancers[0].Location),
                        expectedLocation);
                    Assert.AreEqual(enhancerFeatureList[0].Note[0],
                        expectedNote);
                    Assert.AreEqual(enhancerFeatureList[0].OldLocusTag[0],
                        expectedOldLocusTag);
                    Assert.AreEqual(enhancerFeatureList[0].LocusTag[0],
                        expectedLocusTag);
                    Assert.IsEmpty(enhancerFeatureList[0].StandardName);

                    // Create a new Enhancer and validate the same.
                    Enhancer enhancer = new Enhancer(expectedLocation);
                    GCSingal enhancerWithILoc = new GCSingal(
                        metadata.Features.Enhancers[0].Location);

                    // Set qualifiers and validate them.
                    enhancer.Allele = expectedAllele;
                    enhancer.GeneSymbol = geneSymbol;
                    enhancerWithILoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(enhancer.GeneSymbol, geneSymbol);
                    Assert.AreEqual(enhancer.Allele, expectedAllele);
                    Assert.AreEqual(enhancerWithILoc.GenomicMapPosition, expectedMap);
                    break;
                case FeatureGroup.GCSignal:
                    List<GCSingal> gcSignalFeatureList = metadata.Features.GCSignals;

                    // Create a copy of GC_Signal feature.
                    GCSingal cloneGCSignal = gcSignalFeatureList[0].Clone();

                    // Validate GC_Signal qualifiers.
                    Assert.AreEqual(gcSignalFeatureList.Count.ToString(),
                        featureCount);
                    Assert.AreEqual(cloneGCSignal.GeneSymbol,
                        geneSymbol);
                    Assert.AreEqual(cloneGCSignal.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(gcSignalFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(gcSignalFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(gcSignalFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(gcSignalFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(gcSignalFeatureList[0].GeneSynonym[0],
                        expectedGeneSynonym);
                    Assert.AreEqual(gcSignalFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(gcSignalFeatureList[0].Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.GCSignals[0].Location),
                        expectedLocation);
                    Assert.AreEqual(gcSignalFeatureList[0].Note[0],
                        expectedNote);
                    Assert.AreEqual(gcSignalFeatureList[0].OldLocusTag[0],
                        expectedOldLocusTag);
                    Assert.AreEqual(gcSignalFeatureList[0].LocusTag[0],
                        expectedLocusTag);

                    // Create a new GCSignal and validate the same.
                    GCSingal gcSignal = new GCSingal(expectedLocation);
                    GCSingal gcSignalWithILoc = new GCSingal(
                        metadata.Features.GCSignals[0].Location);

                    // Set qualifiers and validate them.
                    gcSignal.Allele = expectedAllele;
                    gcSignal.GeneSymbol = geneSymbol;
                    gcSignalWithILoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(gcSignal.GeneSymbol, geneSymbol);
                    Assert.AreEqual(gcSignal.Allele, expectedAllele);
                    Assert.AreEqual(gcSignalWithILoc.GenomicMapPosition,
                        expectedMap);
                    break;
                case FeatureGroup.LTR:
                    List<LongTerminalRepeat> LTRFeatureList =
                        metadata.Features.LongTerminalRepeats;

                    // Create a copy of Long Terminal Repeat feature.
                    LongTerminalRepeat cloneLTR = LTRFeatureList[0].Clone();

                    // Validate Long Terminal Repeat qualifiers.
                    Assert.AreEqual(LTRFeatureList.Count.ToString(),
                        featureCount);
                    Assert.AreEqual(cloneLTR.GeneSymbol, geneSymbol);
                    Assert.AreEqual(cloneLTR.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(LTRFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(LTRFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(LTRFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(LTRFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(LTRFeatureList[0].GeneSynonym[0],
                        expectedGeneSynonym);
                    Assert.AreEqual(LTRFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(LTRFeatureList[0].Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.LongTerminalRepeats[0].Location),
                        expectedLocation);
                    Assert.AreEqual(LTRFeatureList[0].Note[0],
                        expectedNote);
                    Assert.AreEqual(LTRFeatureList[0].OldLocusTag[0],
                        expectedOldLocusTag);
                    Assert.AreEqual(LTRFeatureList[0].LocusTag[0],
                        expectedLocusTag);
                    Assert.AreEqual(LTRFeatureList[0].Function[0],
                        expectedFunction);
                    Assert.IsEmpty(LTRFeatureList[0].StandardName);

                    // Create a new LTR and validate.
                    LongTerminalRepeat ltr =
                        new LongTerminalRepeat(expectedLocation);
                    LongTerminalRepeat ltrWithILoc = new LongTerminalRepeat(
                        metadata.Features.LongTerminalRepeats[0].Location);

                    // Set qualifiers and validate them.
                    ltr.Allele = expectedAllele;
                    ltr.GeneSymbol = geneSymbol;
                    ltrWithILoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(ltr.GeneSymbol, geneSymbol);
                    Assert.AreEqual(ltr.Allele, expectedAllele);
                    Assert.AreEqual(ltrWithILoc.GenomicMapPosition,
                        expectedMap);
                    break;
                case FeatureGroup.Operon:
                    List<OperonRegion> operonFeatureList =
                        metadata.Features.OperonRegions;

                    // Create a copy of Long Terminal Repeat feature.
                    OperonRegion cloneOperon = operonFeatureList[0].Clone();

                    // Validate Operon region qualifiers.
                    Assert.AreEqual(operonFeatureList.Count.ToString(),
                        featureCount);
                    Assert.AreEqual(cloneOperon.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(operonFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(operonFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(operonFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(operonFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(operonFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(operonFeatureList[0].Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.OperonRegions[0].Location),
                        expectedLocation);
                    Assert.AreEqual(operonFeatureList[0].Note[0],
                        expectedNote);
                    Assert.IsEmpty(operonFeatureList[0].Function);
                    Assert.AreEqual(operonFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.IsEmpty(operonFeatureList[0].Operon);
                    Assert.IsEmpty(operonFeatureList[0].Phenotype);
                    Assert.IsEmpty(operonFeatureList[0].StandardName);
                    Assert.IsFalse(operonFeatureList[0].Pseudo);

                    // Create a new Operon feature using constructor.
                    OperonRegion operonRegion =
                        new OperonRegion(expectedLocation);
                    OperonRegion operonRegionWithLoc = new OperonRegion(
                        metadata.Features.OperonRegions[0].Location);

                    // Set and validate qualifiers.
                    operonRegion.Allele = expectedAllele;
                    operonRegionWithLoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(operonRegionWithLoc.GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(operonRegion.Allele, expectedAllele);
                    break;
                case FeatureGroup.UnsureSequenceRegion:
                    List<UnsureSequenceRegion> unsureSeqRegionFeatureList =
                        metadata.Features.UnsureSequenceRegions;

                    // Create a copy of Unsure Seq Region feature.
                    UnsureSequenceRegion cloneUnSureSeqRegion =
                        unsureSeqRegionFeatureList[0].Clone();

                    // Validate Unsure Seq Region qualifiers.
                    Assert.AreEqual(unsureSeqRegionFeatureList.Count.ToString()
                        , featureCount);
                    Assert.AreEqual(cloneUnSureSeqRegion.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(cloneUnSureSeqRegion.GeneSymbol,
                        geneSymbol);
                    Assert.AreEqual(unsureSeqRegionFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(unsureSeqRegionFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(unsureSeqRegionFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(unsureSeqRegionFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(unsureSeqRegionFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(unsureSeqRegionFeatureList[0].Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.UnsureSequenceRegions[0].Location),
                        expectedLocation);
                    Assert.AreEqual(unsureSeqRegionFeatureList[0].Note[0],
                        expectedNote);
                    Assert.AreEqual(unsureSeqRegionFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.IsEmpty(unsureSeqRegionFeatureList[0].Compare);
                    Assert.IsEmpty(unsureSeqRegionFeatureList[0].Replace);

                    // Create a new Unsure feature using constructor.
                    UnsureSequenceRegion unsureRegion =
                        new UnsureSequenceRegion(expectedLocation);
                    UnsureSequenceRegion unsureRegionWithLoc =
                        new UnsureSequenceRegion(
                        metadata.Features.UnsureSequenceRegions[0].Location);

                    // Set and validate qualifiers.
                    unsureRegion.Allele = expectedAllele;
                    unsureRegionWithLoc.GeneSymbol = geneSymbol;
                    unsureRegionWithLoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(unsureRegionWithLoc.GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(unsureRegion.Allele, expectedAllele);
                    Assert.AreEqual(unsureRegionWithLoc.GeneSymbol,
                        geneSymbol);
                    break;
                case FeatureGroup.NonCodingRNA:
                    List<NonCodingRNA> nonCodingRNAFeatureList =
                        metadata.Features.NonCodingRNAs;

                    // Create a copy of Non coding RNA feature.
                    NonCodingRNA cloneNonCodingRNA =
                        nonCodingRNAFeatureList[0].Clone();

                    // Validate Non Coding RNA Region qualifiers.
                    Assert.AreEqual(nonCodingRNAFeatureList.Count.ToString(),
                        featureCount);
                    Assert.AreEqual(nonCodingRNAFeatureList[0].NonCodingRNAClass,
                        expectedNonCodingRnaClass);
                    Assert.AreEqual(cloneNonCodingRNA.Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.NonCodingRNAs[0].Location),
                        expectedLocation);

                    // Create a non Coding RNA and validate the same.
                    NonCodingRNA nRNA =
                        new NonCodingRNA(metadata.Features.NonCodingRNAs[0].Location);
                    NonCodingRNA nRNAWithLocation =
                        new NonCodingRNA(expectedLocation);

                    // Set properties 
                    nRNA.NonCodingRNAClass = expectedNonCodingRnaClass;
                    nRNAWithLocation.NonCodingRNAClass = expectedNonCodingRnaClass;

                    // Validate created nRNA.
                    Assert.AreEqual(nRNA.NonCodingRNAClass,
                        expectedNonCodingRnaClass);
                    Assert.AreEqual(nRNAWithLocation.NonCodingRNAClass,
                        expectedNonCodingRnaClass);
                    break;
                case FeatureGroup.CDS:
                    List<CodingSequence> codingSequenceFeatureList =
                        metadata.Features.CodingSequences;

                    // Create a copy of Coding Seq Region feature.
                    CodingSequence cloneCDS = codingSequenceFeatureList[0].Clone();

                    // Validate Unsure Seq Region qualifiers.
                    Assert.AreEqual(codingSequenceFeatureList.Count.ToString(),
                        featureCount);
                    Assert.AreEqual(cloneCDS.DatabaseCrossReference[0],
                        expectedDbReference);
                    Assert.AreEqual(cloneCDS.GeneSymbol, geneSymbol);
                    Assert.AreEqual(codingSequenceFeatureList[0].Allele,
                        expectedAllele);
                    Assert.AreEqual(codingSequenceFeatureList[0].Citation[0],
                        expectedCitation);
                    Assert.AreEqual(codingSequenceFeatureList[0].Experiment[0],
                        expectedExperiment);
                    Assert.AreEqual(codingSequenceFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(codingSequenceFeatureList[0].Inference[0],
                        expectedInference);
                    Assert.AreEqual(codingSequenceFeatureList[0].Label,
                        expectedLabel);
                    Assert.AreEqual(locBuilder.GetLocationString(
                        metadata.Features.CodingSequences[0].Location),
                        expectedLocation);
                    Assert.AreEqual(codingSequenceFeatureList[0].Note[0],
                        expectedNote);
                    Assert.AreEqual(codingSequenceFeatureList[0].GenomicMapPosition,
                        expectedMap);
                    Assert.AreEqual(codingSequenceFeatureList[0].CodonStart[0],
                        expectedCodonStart);
                    Assert.AreEqual(codingSequenceFeatureList[0].Translation,
                        expectedTranslation);
                    Assert.IsEmpty(codingSequenceFeatureList[0].Codon);
                    Assert.IsEmpty(codingSequenceFeatureList[0].EnzymeCommissionNumber);
                    Assert.IsEmpty(codingSequenceFeatureList[0].Number);
                    Assert.IsEmpty(codingSequenceFeatureList[0].Operon);
                    Assert.IsFalse(codingSequenceFeatureList[0].Pseudo);
                    Assert.IsFalse(codingSequenceFeatureList[0].RibosomalSlippage);
                    Assert.IsEmpty(codingSequenceFeatureList[0].StandardName);
                    Assert.IsEmpty(codingSequenceFeatureList[0].TranslationalExcept);
                    Assert.IsEmpty(codingSequenceFeatureList[0].TranslationTable);
                    Assert.IsFalse(codingSequenceFeatureList[0].TransSplicing);
                    Assert.IsEmpty(codingSequenceFeatureList[0].Exception);

                    // Create a new CDS feature using constructor.
                    CodingSequence cds = new CodingSequence(expectedLocation);
                    CodingSequence cdsWithLoc = new CodingSequence(
                        metadata.Features.CodingSequences[0].Location);
                    Sequence seq = cds.GetTranslation();
                    Assert.IsNotNull(seq);

                    // Set and validate qualifiers.
                    cds.Allele = expectedAllele;
                    cdsWithLoc.GeneSymbol = geneSymbol;
                    cdsWithLoc.GenomicMapPosition = expectedMap;
                    Assert.AreEqual(cdsWithLoc.GenomicMapPosition, expectedMap);
                    Assert.AreEqual(cds.Allele, expectedAllele);
                    Assert.AreEqual(cdsWithLoc.GeneSymbol, geneSymbol);
                    break;
                default:
                    break;
            }

            // Log Nunit GUI.
            ApplicationLog.WriteLine(
                "GenBank Features P1: Successfully validated the GenBank Features");
            Console.WriteLine(string.Format(null,
                "GenBank Features P1'{0}'",
                metadata.Features.GCSignals.Count));
        }

        /// <summary>
        /// Validate Location builder and location resolver.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        /// <param name="FeatureOperator">Name of the operator used in a location</param>
        /// <param name="isOperator">True if location resolver validation with 
        /// operator</param>
        static void ValidateGenBankLocationResolver(string nodeName,
            FeatureOperator operatorName, bool isOperator)
        {
            // Get Values from XML node.
            string sequence = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpectedSequence);
            string location = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Location);
            string alphabet = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.AlphabetNameNode);
            string expectedSeq = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.ExpSequenceWithOperator);

            // Create a sequence object.
            ISequence seqObj = new Sequence(Utility.GetAlphabet(alphabet),
                sequence);
            ISequence expectedSeqWithLoc;

            // Build a location.
            ILocationBuilder locBuilder = new LocationBuilder();
            ILocation loc = locBuilder.GetLocation(location);

            if (isOperator)
            {
                switch (operatorName)
                {
                    case FeatureOperator.Complement:
                        loc.Operator = LocationOperator.Complement;
                        break;
                    case FeatureOperator.Join:
                        loc.Operator = LocationOperator.Join;
                        break;
                    case FeatureOperator.Order:
                        loc.Operator = LocationOperator.Order;
                        break;
                    default:
                        break;
                }
            }

            // Get sequence using location of the sequence with operator.
            expectedSeqWithLoc = loc.GetSubSequence(seqObj);

            Assert.AreEqual(expectedSeq, expectedSeqWithLoc.ToString());

            // Log to Nunit GUI.
            ApplicationLog.WriteLine(string.Format(null,
                "GenBankFeatures P1 : Expected sequence is verfied '{0}'.",
               expectedSeqWithLoc.ToString()));
            Console.WriteLine(string.Format(null,
                "GenBankFeatures P1 : Expected sequence is verfied '{0}'.",
               expectedSeqWithLoc.ToString()));
        }

        /// <summary>
        /// Validate location resolver end data.
        /// </summary>
        /// <param name="nodeName">xml node name.</param>
        static void ValidateLocationEndData(string nodeName)
        {
            // Get Values from XML node.
            string location = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Location);
            string expectedEndData = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.EndData);
            string position = Utility._xmlUtil.GetTextValue(
                nodeName, Constants.Position);

            bool result = false;

            // Build a location.
            LocationResolver locResolver = new LocationResolver();
            ILocationBuilder locBuilder = new LocationBuilder();
            ILocation loc = locBuilder.GetLocation(location);
            loc.EndData = expectedEndData;

            // Validate whether mentioned end data is present in the location
            // or not.
            result = locResolver.IsInEnd(loc, Int32.Parse(position));
            Assert.IsTrue(result);

            // Log to Nunit GUI.
            ApplicationLog.WriteLine(string.Format(null,
                "GenBankFeatures P1 : Expected sequence is verified"));
            Console.WriteLine(string.Format(null,
                "GenBankFeatures P1 : Expected sequence is verified"));
        }

        #endregion Supporting Methods
    }
}
