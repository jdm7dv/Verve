// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using MBF.Util.Logging;

using System.Collections.Generic;
using System.Linq;
using MBF.IO.GenBank;
using MBF.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MBF.Tests
{
    /// <summary>
    /// Test the ICloneable implementation.
    /// </summary>
    [TestClass]
    public class CloneTests
    {

        /// <summary>
        /// Static constructor to open log and make other settings needed for test.
        /// </summary>
        static CloneTests()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("MBF.Tests.log");
            }
        }

        /// <summary>
        /// Test Sequence cloning.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void SequenceClone()
        {
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "SequenceClone test started"));

            string seqData = "ACGAACCGGAAACCCGGG";

            Sequence orgSeq = new Sequence(Alphabets.DNA, seqData);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Original Sequence: " + orgSeq.ToString()));
            Sequence cloneSeq = orgSeq.Clone();

            // Test the clone copy with original.
            Assert.AreEqual(cloneSeq.ToString(), orgSeq.ToString());
            Assert.AreEqual(cloneSeq.Alphabet, orgSeq.Alphabet);
            Assert.AreEqual(cloneSeq.Complement.ToString(), orgSeq.Complement.ToString());
            Assert.AreEqual(cloneSeq.Count, orgSeq.Count);
            Assert.AreEqual(cloneSeq.DisplayID, orgSeq.DisplayID);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Cloned Sequence: " + cloneSeq));

            // Check whether modifying the clone sequence modifies the original sequence or not.
            cloneSeq.IsReadOnly = false;
            cloneSeq[1] = Alphabets.DNA.G;
            cloneSeq[2] = Alphabets.DNA.G;
            cloneSeq[3] = Alphabets.DNA.G;

            Assert.AreEqual(orgSeq.ToString(), seqData);
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Original Sequence After modifying the cloned sequence: " + orgSeq.ToString()));

            // Test using ISequence interface.
            ISequence seq1 = orgSeq;
            ISequence seq2 = seq1.Clone();

            Assert.AreEqual(seq1.ToString(), seq2.ToString());

            // Test using ICloneable interface.
            ICloneable cloneseq1 = orgSeq;
            Sequence cloneseq2 = cloneseq1.Clone() as Sequence;

            Assert.AreEqual(cloneseq1.ToString(), cloneseq2.ToString());

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "SequenceClone test completed"));
        }

        /// <summary>
        /// Test BasicDerivedSequence cloning.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void BasicDerivedSequenceClone()
        {
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "BasicDerivedSequenceClone test started"));

            string seqData = "GCCAACGAACCGGAAACCCGGGACCG";

            Sequence orgSeq = new Sequence(Alphabets.DNA, seqData);
            BasicDerivedSequence basicDerivedSeq = new BasicDerivedSequence(orgSeq, false, false, 0, 0);

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Original Sequence: " + basicDerivedSeq.ToString()));
            BasicDerivedSequence basicDerivedSeqClone = basicDerivedSeq.Clone();
            Assert.AreEqual(basicDerivedSeq.ToString(), basicDerivedSeqClone.ToString());
            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "Cloned Sequence: " + basicDerivedSeqClone.ToString()));

            ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                "BasicDerivedSequenceClone test completed"));
        }

        /// <summary>
        /// Test VirtualSequence cloning.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void VirtualSequenceClone()
        {
            VirtualSequence virtualSeq = new VirtualSequence(Alphabets.DNA);
            string id = Guid.NewGuid().ToString(String.Empty);
            virtualSeq.ID = id;
            virtualSeq.DisplayID = "Id to display";
            virtualSeq.MoleculeType = MoleculeType.DNA;

            // Test cloning of data in Metadata property.
            virtualSeq.Metadata.Add("NotCloneable", Alphabets.DNA);
            Sequence seqCloneable = new Sequence(Alphabets.RNA);
            virtualSeq.Metadata.Add("CloneableValue", seqCloneable);

            VirtualSequence cloneCopy1 = virtualSeq.Clone();
            Assert.AreNotSame(cloneCopy1, virtualSeq);
            Assert.AreEqual(virtualSeq.ID, cloneCopy1.ID);
            Assert.AreEqual(virtualSeq.DisplayID, cloneCopy1.DisplayID);
            Assert.AreEqual(virtualSeq.MoleculeType, cloneCopy1.MoleculeType);
            Assert.AreNotSame(virtualSeq.Metadata, cloneCopy1.Metadata);
            Assert.AreSame(virtualSeq.Metadata["NotCloneable"], cloneCopy1.Metadata["NotCloneable"]);
            Assert.AreNotSame(virtualSeq.Metadata["CloneableValue"], cloneCopy1.Metadata["CloneableValue"]);
        }

        /// <summary>
        /// Test Sparse sequence cloning.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void SparseSequenceClone()
        {
            SparseSequence sparseSeq = new SparseSequence(Alphabets.DNA);
            sparseSeq.Count = 50;
            sparseSeq[0] = Alphabets.DNA.C;
            sparseSeq[5] = Alphabets.DNA.G;
            sparseSeq[30] = Alphabets.DNA.C;
            sparseSeq[44] = Alphabets.DNA.A;
            sparseSeq[45] = new Nucleotide('A', "Nucleotide A");
            CompoundNucleotide cn = new CompoundNucleotide('M', "Compound");
            cn.Add(new Nucleotide('A', "Item A"), 30);
            cn.Add(new Nucleotide('C', "Item C"), 20);
            sparseSeq[46] = cn;

            Assert.AreEqual(sparseSeq.Count, 50);
            Assert.AreEqual(sparseSeq.Alphabet, Alphabets.DNA);
            Assert.AreSame(sparseSeq[0], Alphabets.DNA.C);
            Assert.AreSame(sparseSeq[5], Alphabets.DNA.G);
            Assert.AreSame(sparseSeq[30], Alphabets.DNA.C);
            Assert.AreSame(sparseSeq[44], Alphabets.DNA.A);
            Assert.AreEqual(sparseSeq[45].Symbol, 'A');
            Assert.AreSame(sparseSeq[46], cn);
            Assert.AreNotSame(sparseSeq[46], Alphabets.DNA.AC);
            for (int i = 0; i < sparseSeq.Count; i++)
            {
                if (i != 0 && i != 5 && i != 30 && i != 44 && i != 45 && i != 46)
                {
                    Assert.IsNull(sparseSeq[i]);
                }
            }

            SparseSequence sparseSeqClone = sparseSeq.Clone();

            Assert.AreEqual(sparseSeqClone.Count, 50);
            Assert.AreEqual(sparseSeqClone.Alphabet, Alphabets.DNA);
            Assert.AreSame(sparseSeqClone[0], Alphabets.DNA.C);
            Assert.AreSame(sparseSeqClone[5], Alphabets.DNA.G);
            Assert.AreSame(sparseSeqClone[30], Alphabets.DNA.C);
            Assert.AreSame(sparseSeqClone[44], Alphabets.DNA.A);
            Assert.AreNotSame(sparseSeqClone[46], cn);
            Assert.AreNotSame(sparseSeqClone[46], Alphabets.DNA.AC);
            Assert.AreEqual(sparseSeqClone[46].Symbol, 'M');
            cn = sparseSeqClone[46] as CompoundNucleotide;
            IList<ISequenceItem> sequenceItems = cn.SequenceItems;
            Assert.AreEqual(sequenceItems.Count, 2);
            Assert.IsTrue(sequenceItems.First(I => I.Symbol == 'A') != null);
            Assert.IsTrue(sequenceItems.First(I => I.Symbol == 'C') != null);

            for (int i = 0; i < sparseSeqClone.Count; i++)
            {
                if (i != 0 && i != 5 && i != 30 && i != 44 && i != 45 && i != 46)
                {
                    Assert.IsNull(sparseSeqClone[i]);
                }
            }
        }

        /// <summary>
        /// Test segmented sequence cloning.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void SegmentedSequenceClone()
        {
            SegmentedSequence segmntSeq;
            SegmentedSequence segmntSeqClone;
            List<ISequence> seqList = new List<ISequence>();
            ISequence seq1 = new Sequence(Alphabets.DNA, "ATGC");
            ISequence seq2 = new Sequence(Alphabets.DNA, "CGTATGGC");
            ISequence seq3 = new Sequence(Alphabets.DNA, "GGGTAA");
            seqList.Add(seq1);
            seqList.Add(seq2);
            seqList.Add(seq3);
            seqList.Add(seq1);

            segmntSeq = new SegmentedSequence(seqList);
            Assert.AreEqual(segmntSeq.Count, 22);
            Assert.AreEqual(segmntSeq.Sequences.Count, 4);
            Assert.AreSame(seq1, segmntSeq.Sequences[0]);
            Assert.AreSame(seq2, segmntSeq.Sequences[1]);
            Assert.AreSame(seq3, segmntSeq.Sequences[2]);
            Assert.AreSame(seq1, segmntSeq.Sequences[3]);

            Assert.AreEqual(segmntSeq.ToString(), "ATGCCGTATGGCGGGTAAATGC");

            segmntSeqClone = segmntSeq.Clone();

            Assert.AreNotSame(segmntSeq, segmntSeqClone);
            Assert.AreEqual(segmntSeqClone.Count, 22);
            Assert.AreEqual(segmntSeqClone.Sequences.Count, 4);
            Assert.AreNotSame(seq1, segmntSeqClone.Sequences[0]);
            Assert.AreNotSame(seq2, segmntSeqClone.Sequences[1]);
            Assert.AreNotSame(seq3, segmntSeqClone.Sequences[2]);
            Assert.AreNotSame(seq1, segmntSeqClone.Sequences[3]);

            Assert.AreEqual(segmntSeqClone.ToString(), "ATGCCGTATGGCGGGTAAATGC");
        }

        /// <summary>
        /// Test derived sequence cloning.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void DerivedSequenceClone()
        {
            Sequence seq = new Sequence(Alphabets.RNA, "ACUGA");
            DerivedSequence derSeq = new DerivedSequence(seq);
            derSeq.RemoveAt(0);
            Assert.AreEqual(derSeq.ToString(), "CUGA");
            derSeq.RemoveAt(2);
            Assert.AreEqual(derSeq.ToString(), "CUA");
            derSeq.Insert(2, Alphabets.RNA.C);
            Assert.AreEqual(derSeq.ToString(), "CUCA");
            DerivedSequence cloneCopy = derSeq.Clone();
            Assert.AreNotSame(derSeq, cloneCopy);
            Assert.AreNotSame(derSeq.Source, cloneCopy.Source);
            Assert.AreEqual(derSeq.ToString(), cloneCopy.ToString());
        }

        /// <summary>
        /// Test Genbank metadata cloning.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void GenBankMetadataClone()
        {
            LocationBuilder locBuilder = new LocationBuilder();
            GenBankMetadata metadata = new GenBankMetadata();
            metadata.Accession = new GenBankAccession();
            metadata.Accession.Primary = "PAccession";
            metadata.Accession.Secondary.Add("SAccession1");
            metadata.Accession.Secondary.Add("SAccession2");
            metadata.BaseCount = "a 1 c 2";
            metadata.Comments.Add("Comment1");
            metadata.Comments.Add("Comment2");
            metadata.Contig = "Contig Info";
            metadata.DBLink = new CrossReferenceLink();
            metadata.DBLink.Type = CrossReferenceType.Project;
            metadata.DBLink.Numbers.Add("100");
            metadata.DBLink.Numbers.Add("200");
            metadata.DBSource = "DbSourceInfo";
            metadata.Definition = "Defination info";
            metadata.Features = new SequenceFeatures();
            FeatureItem feature = new FeatureItem("feature1", "1");
            List<string> qualifierValues = new List<string>();
            qualifierValues.Add("qualifier1value1");
            qualifierValues.Add("qualifier1value2");
            feature.Qualifiers.Add("qualifier1", qualifierValues);
            metadata.Features.All.Add(feature);

            feature = new FeatureItem("feature2", "2");
            qualifierValues = new List<string>();
            qualifierValues.Add("qualifier2value1");
            qualifierValues.Add("qualifier2value2");
            feature.Qualifiers.Add("qualifier2", qualifierValues);
            metadata.Features.All.Add(feature);
            feature = new FeatureItem("feature2", "2");
            qualifierValues = new List<string>();
            qualifierValues.Add("qualifier2value1");
            qualifierValues.Add("qualifier2value2");
            feature.Qualifiers.Add("qualifier2", qualifierValues);
            metadata.Features.All.Add(feature);

            metadata.Keywords = "keywords data";
            metadata.Locus = new GenBankLocusInfo();
            metadata.Locus.Date = DateTime.Now;
            metadata.Locus.DivisionCode = SequenceDivisionCode.CON;
            metadata.Locus.MoleculeType = MoleculeType.DNA;
            metadata.Locus.Name = "LocusName";
            metadata.Locus.SequenceLength = 100;
            metadata.Locus.SequenceType = "bp";
            metadata.Locus.Strand = SequenceStrandType.Double;
            metadata.Locus.StrandTopology = SequenceStrandTopology.Linear;
            metadata.Origin = "origin info";
            metadata.Primary = "Primary info";
            metadata.Project = new ProjectIdentifier();
            metadata.Project.Name = "Project1";
            metadata.Project.Numbers.Add("101");
            metadata.Project.Numbers.Add("201");
            CitationReference reference = new CitationReference();
            reference.Authors = "Authors";
            reference.Consortiums = "Consortiums";
            reference.Journal = "Journal";
            reference.Location = "3";
            reference.Medline = "Medline info";
            reference.Number = 1;
            reference.PubMed = "pubmid";
            reference.Remarks = "remarks";
            reference.Title = "Title of the book";
            metadata.References.Add(reference);
            reference = new CitationReference();
            reference.Authors = "Authors";
            reference.Consortiums = "Consortiums";
            reference.Journal = "Journal";
            reference.Location = "4";
            reference.Medline = "Medline info";
            reference.Number = 2;
            reference.PubMed = "pubmid";
            reference.Remarks = "remarks";
            reference.Title = "Title of the book";
            metadata.References.Add(reference);
            metadata.Segment = new SequenceSegment();
            metadata.Segment.Count = 2;
            metadata.Segment.Current = 1;
            metadata.Source = new SequenceSource();
            metadata.Source.CommonName = "ABC Xyz";
            metadata.Source.Organism.Genus = "ABC";
            metadata.Source.Organism.Species = "Xyz";
            metadata.Source.Organism.ClassLevels = "123 123";
            metadata.Version = new GenBankVersion();
            metadata.Version.Accession = "PAccession";
            metadata.Version.Version = "1";
            metadata.Version.GINumber = "12345";

            GenBankMetadata clonemetadta = metadata.Clone();
            Assert.AreEqual(clonemetadta.Accession.Primary, metadata.Accession.Primary);

            for (int i = 0; i < clonemetadta.Accession.Secondary.Count; i++)
            {
                Assert.AreEqual(clonemetadta.Accession.Secondary[i], metadata.Accession.Secondary[i]);
            }

            Assert.AreEqual(clonemetadta.BaseCount, metadata.BaseCount);

            for (int i = 0; i < clonemetadta.Comments.Count; i++)
            {
                Assert.AreEqual(clonemetadta.Comments[i], metadata.Comments[i]);
            }

            Assert.AreEqual(clonemetadta.Contig, metadata.Contig);

            Assert.AreEqual(clonemetadta.DBLink.Type, metadata.DBLink.Type);

            for (int i = 0; i < clonemetadta.DBLink.Numbers.Count; i++)
            {
                Assert.AreEqual(clonemetadta.DBLink.Numbers[i], metadata.DBLink.Numbers[i]);
            }

            Assert.AreEqual(clonemetadta.DBSource, metadata.DBSource);
            Assert.AreEqual(clonemetadta.Definition, metadata.Definition);

            for (int i = 0; i < clonemetadta.Features.All.Count; i++)
            {
                Assert.AreEqual(clonemetadta.Features.All[i].Key, metadata.Features.All[i].Key);
                Assert.AreEqual(locBuilder.GetLocationString(clonemetadta.Features.All[i].Location), locBuilder.GetLocationString(metadata.Features.All[i].Location));

                foreach (KeyValuePair<string, List<string>> kvp in clonemetadta.Features.All[i].Qualifiers)
                {
                    if (metadata.Features.All[i].Qualifiers.ContainsKey(kvp.Key))
                    {
                        if (kvp.Value == null)
                        {
                            Assert.IsNull(metadata.Features.All[i].Qualifiers[kvp.Key]);
                        }
                        else
                        {
                            for (int j = 0; j < kvp.Value.Count; j++)
                            {
                                Assert.AreEqual(kvp.Value[j], metadata.Features.All[i].Qualifiers[kvp.Key][j]);
                            }
                        }
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
            }

            Assert.AreEqual(clonemetadta.Keywords, metadata.Keywords);
            Assert.AreEqual(clonemetadta.Locus.Date, metadata.Locus.Date);
            Assert.AreEqual(clonemetadta.Locus.DivisionCode, metadata.Locus.DivisionCode);
            Assert.AreEqual(clonemetadta.Locus.MoleculeType, metadata.Locus.MoleculeType);
            Assert.AreEqual(clonemetadta.Locus.Name, metadata.Locus.Name);
            Assert.AreEqual(clonemetadta.Locus.SequenceLength, metadata.Locus.SequenceLength);
            Assert.AreEqual(clonemetadta.Locus.SequenceType, metadata.Locus.SequenceType);
            Assert.AreEqual(clonemetadta.Locus.Strand, metadata.Locus.Strand);
            Assert.AreEqual(clonemetadta.Locus.StrandTopology, metadata.Locus.StrandTopology);
            Assert.AreEqual(clonemetadta.Origin, clonemetadta.Origin);
            Assert.AreEqual(clonemetadta.Primary, metadata.Primary);
            Assert.AreEqual(clonemetadta.Project.Name, metadata.Project.Name);
            for (int i = 0; i < clonemetadta.Project.Numbers.Count; i++)
            {
                Assert.AreEqual(clonemetadta.Project.Numbers[i], metadata.Project.Numbers[i]);
            }

            for (int i = 0; i < clonemetadta.References.Count; i++)
            {
                Assert.AreEqual(clonemetadta.References[i].Authors, metadata.References[i].Authors);
                Assert.AreEqual(clonemetadta.References[i].Consortiums, metadata.References[i].Consortiums);
                Assert.AreEqual(clonemetadta.References[i].Journal, metadata.References[i].Journal);
                Assert.AreEqual(clonemetadta.References[i].Location, metadata.References[i].Location);
                Assert.AreEqual(clonemetadta.References[i].Medline, metadata.References[i].Medline);
                Assert.AreEqual(clonemetadta.References[i].Number, metadata.References[i].Number);
                Assert.AreEqual(clonemetadta.References[i].PubMed, metadata.References[i].PubMed);
                Assert.AreEqual(clonemetadta.References[i].Remarks, metadata.References[i].Remarks);
                Assert.AreEqual(clonemetadta.References[i].Title, metadata.References[i].Title);
            }

            Assert.AreEqual(clonemetadta.Segment.Current, metadata.Segment.Current);
            Assert.AreEqual(clonemetadta.Segment.Count, metadata.Segment.Count);
            Assert.AreEqual(clonemetadta.Source.CommonName, metadata.Source.CommonName);
            Assert.AreEqual(clonemetadta.Source.Organism.ClassLevels, metadata.Source.Organism.ClassLevels);
            Assert.AreEqual(clonemetadta.Source.Organism.Genus, metadata.Source.Organism.Genus);
            Assert.AreEqual(clonemetadta.Source.Organism.Species, metadata.Source.Organism.Species);
            Assert.AreEqual(clonemetadta.Version.Accession, metadata.Version.Accession);
            Assert.AreEqual(clonemetadta.Version.CompoundAccession, metadata.Version.CompoundAccession);
            Assert.AreEqual(clonemetadta.Version.GINumber, metadata.Version.GINumber);
            Assert.AreEqual(clonemetadta.Version.Version, metadata.Version.Version);

        }

        /// <summary>
        /// Test genbank features cloning.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void GenBankFeatureClone()
        {
            ISequenceParser parser = new GenBankParser();
            ISequence seq = parser.ParseOne(@"TestUtils\GenBank\NC_001284.gbk");
            GenBankMetadata metadata = seq.Metadata["GenBank"] as GenBankMetadata;
            Assert.AreEqual(metadata.Features.All.Count, 743);
            Assert.AreEqual(metadata.Features.CodingSequences.Count, 117);
            Assert.AreEqual(metadata.Features.Exons.Count, 32);
            Assert.AreEqual(metadata.Features.Introns.Count, 22);
            Assert.AreEqual(metadata.Features.Genes.Count, 60);
            Assert.AreEqual(metadata.Features.MiscFeatures.Count, 455);
            Assert.AreEqual(metadata.Features.Promoters.Count, 17);
            Assert.AreEqual(metadata.Features.TransferRNAs.Count, 21);
            Assert.AreEqual(metadata.Features.All.FindAll(F => F.Key.Equals(StandardFeatureKeys.CodingSequence)).Count, 117);
            Assert.AreEqual(metadata.Features.CodingSequences[0].Translation.Trim('"'), metadata.Features.CodingSequences[0].GetTranslation().ToString());
            Assert.AreEqual(metadata.GetFeatures(11918, 12241).Count, 2);
            GenBankMetadata clonedMetadata = metadata.Clone();
            Assert.AreEqual(clonedMetadata.Features.All.Count, 743);
            Assert.AreEqual(clonedMetadata.Features.CodingSequences.Count, 117);
            Assert.AreEqual(clonedMetadata.Features.Exons.Count, 32);
            Assert.AreEqual(clonedMetadata.Features.Introns.Count, 22);
            Assert.AreEqual(clonedMetadata.Features.Genes.Count, 60);
            Assert.AreEqual(clonedMetadata.Features.MiscFeatures.Count, 455);
            Assert.AreEqual(clonedMetadata.Features.Promoters.Count, 17);
            Assert.AreEqual(clonedMetadata.Features.TransferRNAs.Count, 21);
            Assert.AreEqual(clonedMetadata.Features.All.FindAll(F => F.Key.Equals(StandardFeatureKeys.CodingSequence)).Count, 117);
            Assert.AreEqual(clonedMetadata.Features.CodingSequences[0].Translation.Trim('"'), clonedMetadata.Features.CodingSequences[0].GetTranslation().ToString());
            Assert.AreEqual(clonedMetadata.GetFeatures(11918, 12241).Count, 2);
        }

        /// <summary>
        /// Test Qualitative Sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void QualitativeSequenceClone()
        {
            QualitativeSequence qualSequence = new QualitativeSequence(Alphabets.RNA, FastQFormatType.Illumina, "ACUGGA", 65);
            Assert.AreEqual(qualSequence.Alphabet, Alphabets.RNA);
            Assert.IsTrue(qualSequence.IsReadOnly);
            Assert.AreEqual(qualSequence.Count, 6);
            Assert.AreEqual(qualSequence.Scores.Length, 6);
            foreach (byte qualScore in qualSequence.Scores)
            {
                Assert.AreEqual(qualScore, 65);
            }
            Assert.AreEqual(qualSequence.ToString(), "ACUGGA");
            Assert.AreEqual(qualSequence.Type, FastQFormatType.Illumina);

            QualitativeSequence cloneCopy = qualSequence.Clone();

            Assert.AreEqual(cloneCopy.Alphabet, Alphabets.RNA);
            Assert.IsTrue(cloneCopy.IsReadOnly);
            Assert.AreEqual(cloneCopy.Count, 6);
            Assert.AreEqual(cloneCopy.Scores.Length, 6);
            foreach (byte qualScore in cloneCopy.Scores)
            {
                Assert.AreEqual(qualScore, 65);
            }
            Assert.AreEqual(cloneCopy.ToString(), "ACUGGA");
            Assert.AreEqual(cloneCopy.Type, FastQFormatType.Illumina);
        }

        /// <summary>
        /// Test Location.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void LocationClone()
        {
            Location loc = new Location();
            loc.Operator = LocationOperator.Complement;
            Location subLocation = new Location();
            subLocation.StartData = "100";
            subLocation.EndData = "200";
            subLocation.Separator = "..";
            subLocation.Accession = "123";
            loc.SubLocations.Add(subLocation);

            Location cloned = loc.Clone();
            Assert.AreEqual(loc.StartData, cloned.StartData);
            Assert.AreEqual(loc.EndData, cloned.EndData);
            Assert.AreEqual(loc.Start, cloned.Start);
            Assert.AreEqual(loc.End, cloned.End);
            Assert.AreEqual(loc.Operator, cloned.Operator);
            Assert.AreEqual(loc.SubLocations.Count, cloned.SubLocations.Count);
            Assert.AreEqual(loc.SubLocations[0].StartData, cloned.SubLocations[0].StartData);
            Assert.AreEqual(loc.SubLocations[0].EndData, cloned.SubLocations[0].EndData);
            Assert.AreEqual(loc.SubLocations[0].Separator, cloned.SubLocations[0].Separator);
            Assert.AreEqual(loc.SubLocations[0].Accession, cloned.SubLocations[0].Accession);
            Assert.AreEqual(loc.SubLocations[0].Start, cloned.SubLocations[0].Start);
            Assert.AreEqual(loc.SubLocations[0].End, cloned.SubLocations[0].End);
        }
    }
}