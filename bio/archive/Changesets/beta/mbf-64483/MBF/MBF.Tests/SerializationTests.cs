// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using MBF.Algorithms.Alignment;
    using MBF.Algorithms.Assembly;
    using MBF.IO;
    using MBF.Encoding;
    using MBF.IO.GenBank;

    using MBF.SimilarityMatrices;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test the ISerializable implementation.
    /// </summary>
    [TestClass]
    public class SerializationTests
    {

        /// <summary>
        /// Test serialization of Sequence with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestSequenceWithBinaryFormatter()
        {
            Stream stream = null;

            try
            {
                Sequence seq = new Sequence(Alphabets.DNA, "ACGTACGT");

                stream = File.Open("Sequence.data", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, seq);

                stream.Seek(0, SeekOrigin.Begin);
                Sequence deserializedSeq = (Sequence)formatter.Deserialize(stream);

                Assert.AreNotSame(seq, deserializedSeq);
                Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);
                Assert.AreSame(seq.Encoding, deserializedSeq.Encoding);

                Assert.AreNotSame(seq.Encoder, deserializedSeq.Encoder);
                Assert.AreNotSame(seq.Decoder, deserializedSeq.Decoder);

                Assert.AreEqual(seq.Complement.ToString(), deserializedSeq.Complement.ToString());
                Assert.AreEqual(seq.Count, deserializedSeq.Count);
                Assert.AreEqual(seq.DisplayID, deserializedSeq.DisplayID);
                Assert.AreEqual(seq.ID, deserializedSeq.ID);
                Assert.AreEqual(seq.IsReadOnly, deserializedSeq.IsReadOnly);
                Assert.AreEqual(seq.MoleculeType, deserializedSeq.MoleculeType);
                Assert.AreEqual(seq.Reverse.ToString(), deserializedSeq.Reverse.ToString());
                Assert.AreEqual(seq.ReverseComplement.ToString(), deserializedSeq.ReverseComplement.ToString());
                Assert.AreEqual(seq.ToString(), deserializedSeq.ToString());
                Assert.AreEqual(seq.Statistics.GetCount('A'), seq.Statistics.GetCount('A'));
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        /// <summary>
        /// Test serialization of SparseSequence with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestSparseSequenceWithBinaryFormatter()
        {
            Stream stream = null;

            try
            {
                SparseSequence seq = new SparseSequence(Alphabets.DNA);
                seq.Count = 50;
                seq.Insert(2, Alphabets.DNA.A);
                seq.Insert(8, Alphabets.DNA.T);
                seq.Insert(9, Alphabets.DNA.T);
                seq.Insert(10, Alphabets.DNA.A);
                seq.Insert(21, Alphabets.DNA.G);
                seq.Insert(45, Alphabets.DNA.C);
                CompoundNucleotide cn = new CompoundNucleotide('M', "Compound");
                cn.Add(new Nucleotide('A', "Item A"), 30);
                cn.Add(new Nucleotide('C', "Item C"), 20);
                seq[46] = cn;
                seq.Add(new Nucleotide('A', "Question"));
                stream = File.Open("SparseSequence.data", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, seq);

                stream.Seek(0, SeekOrigin.Begin);
                SparseSequence deserializedSeq = (SparseSequence)formatter.Deserialize(stream);

                Assert.AreNotSame(seq, deserializedSeq);
                Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);

                Assert.AreEqual(seq.Count, deserializedSeq.Count);
                Assert.AreEqual(seq.DisplayID, deserializedSeq.DisplayID);
                Assert.AreEqual(seq.ID, deserializedSeq.ID);
                Assert.AreEqual(seq.IsReadOnly, deserializedSeq.IsReadOnly);
                Assert.AreEqual(seq.MoleculeType, deserializedSeq.MoleculeType);

                Assert.AreSame(seq[0], deserializedSeq[0]);
                Assert.AreSame(seq[2], deserializedSeq[2]);
                Assert.AreSame(seq[8], deserializedSeq[8]);
                Assert.AreSame(seq[9], deserializedSeq[9]);
                Assert.AreSame(seq[10], deserializedSeq[10]);
                Assert.AreSame(seq[21], deserializedSeq[21]);
                Assert.AreSame(seq[45], deserializedSeq[45]);
                Assert.AreNotSame(seq[46], deserializedSeq[46]);
                cn = deserializedSeq[46] as CompoundNucleotide;
                IList<ISequenceItem> sequenceItems = cn.SequenceItems;
                Assert.AreEqual(sequenceItems.Count, 2);
                Assert.IsTrue(sequenceItems.First(I => I.Symbol == 'A') != null);
                Assert.IsTrue(sequenceItems.First(I => I.Symbol == 'C') != null);
                Assert.AreEqual(seq[46].Symbol, 'M');

                Assert.AreNotSame(seq[seq.Count - 1], deserializedSeq[deserializedSeq.Count - 1]);
                Assert.AreEqual(seq.Statistics.GetCount('A'), deserializedSeq.Statistics.GetCount('A'));
                stream.Close();
                stream = null;
                seq = new SparseSequence(Alphabets.DNA);
                stream = File.Open("SparseSequence.data", FileMode.Create);
                formatter = new BinaryFormatter();
                formatter.Serialize(stream, seq);

                stream.Seek(0, SeekOrigin.Begin);
                deserializedSeq = (SparseSequence)formatter.Deserialize(stream);

                Assert.AreNotSame(seq, deserializedSeq);
                Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);

                Assert.AreEqual(seq.Count, deserializedSeq.Count);
                Assert.AreEqual(seq.DisplayID, deserializedSeq.DisplayID);
                Assert.AreEqual(seq.ID, deserializedSeq.ID);
                Assert.AreEqual(seq.IsReadOnly, deserializedSeq.IsReadOnly);
                Assert.AreEqual(seq.MoleculeType, deserializedSeq.MoleculeType);
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        /// <summary>
        /// Test serialization of SegmentedSequence with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestSegmentedSequenceWithBinaryFormatter()
        {
            Stream stream = null;

            try
            {
                Sequence seq1 = new Sequence(Alphabets.DNA);
                seq1.InsertRange(0, "ACGACTGC");
                Sequence seq2 = new Sequence(Alphabets.DNA, "AGGTCA");
                Sequence seq3 = new Sequence(Alphabets.DNA, Encodings.Ncbi2NA, "GGCCA");
                SegmentedSequence seq = new SegmentedSequence(
                    new List<ISequence>()
                    {
                        seq1,
                        seq2,
                        seq3
                    });

                stream = File.Open("SegmentedSequence.data", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, seq);

                stream.Seek(0, SeekOrigin.Begin);
                SegmentedSequence deserializedSeq = (SegmentedSequence)formatter.Deserialize(stream);

                Assert.AreNotSame(seq, deserializedSeq);
                Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);

                Assert.AreEqual(seq.Count, deserializedSeq.Count);
                Assert.AreEqual(seq.DisplayID, deserializedSeq.DisplayID);
                Assert.AreEqual(seq.ID, deserializedSeq.ID);
                Assert.AreEqual(seq.IsReadOnly, deserializedSeq.IsReadOnly);
                Assert.AreEqual(seq.MoleculeType, deserializedSeq.MoleculeType);

                Assert.AreEqual(seq.ToString(), deserializedSeq.ToString());
                Assert.AreEqual(seq.Sequences.Count, deserializedSeq.Sequences.Count);

                // Verify Selection changed event.
                try
                {
                    deserializedSeq.Sequences.Add(new Sequence(Alphabets.RNA, "ACGAC"));
                    Assert.Fail();
                }
                catch (ArgumentException)
                {
                    Assert.AreEqual(seq.Sequences.Count, deserializedSeq.Sequences.Count);
                }
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        /// <summary>
        /// Test serialization of VirtualSequence with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestVirtualSequenceWithBinaryFormatter()
        {
            Stream stream = null;

            try
            {
                string id = Guid.NewGuid().ToString();

                VirtualSequence seq = new VirtualSequence(Alphabets.RNA);
                seq.Metadata.Add("string", "Value1");
                seq.Metadata.Add("int", 2);
                seq.Metadata.Add("float", 4.5);

                // Add non serializable object.
                seq.Metadata.Add("Nucleotide", new Nucleotide('A', "A", false, false));
                seq.Documentation = "document";
                seq.ID = id;
                seq.DisplayID = "displayid";
                seq.MoleculeType = MoleculeType.mRNA;
                stream = File.Open("VirtualSequence.data", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, seq);

                stream.Seek(0, SeekOrigin.Begin);
                VirtualSequence deserializedSeq = (VirtualSequence)formatter.Deserialize(stream);
                Assert.AreNotSame(seq, deserializedSeq);
                Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);
                Assert.AreEqual(seq.ID, deserializedSeq.ID);
                Assert.AreEqual(seq.IsReadOnly, deserializedSeq.IsReadOnly);
                Assert.AreEqual(seq.MoleculeType, deserializedSeq.MoleculeType);
                Assert.AreEqual(seq.Count, deserializedSeq.Count);
                Assert.AreEqual(seq.DisplayID, deserializedSeq.DisplayID);
                Assert.AreEqual(seq.Documentation, deserializedSeq.Documentation);

                if (deserializedSeq.Metadata != null && deserializedSeq.Metadata.Count > 0)
                {
                    foreach (string key in seq.Metadata.Keys)
                    {
                        if (deserializedSeq.Metadata.ContainsKey(key))
                        {
                            if (key.Equals("Nucleotide"))
                            {
                                Assert.IsNotNull(deserializedSeq.Metadata[key]);
                            }
                            else
                            {
                                Assert.AreEqual(seq.Metadata[key], deserializedSeq.Metadata[key]);
                            }
                        }
                        else
                        {
                            Assert.Fail();
                        }
                    }
                }
                else
                {
                    Assert.Fail();
                }
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        /// <summary>
        /// Test serialization of BasicDerivedSequence with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestBasicDerivedSequenceWithBinaryFormatter()
        {
            Stream stream = null;

            try
            {
                stream = File.Open("BasicDerivedSequence.data", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();

                string id = Guid.NewGuid().ToString();
                Sequence seq1 = new Sequence(Alphabets.RNA, "ACUGCA");
                seq1.ID = id;
                seq1.DisplayID = "displayid";
                seq1.Documentation = "document";
                BasicDerivedSequence seq = new BasicDerivedSequence(seq1, true, true, -1, -1);

                formatter.Serialize(stream, seq);
                stream.Seek(0, SeekOrigin.Begin);
                BasicDerivedSequence deserializedSeq = (BasicDerivedSequence)formatter.Deserialize(stream);

                Assert.AreNotSame(seq, deserializedSeq);
                Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);
                Assert.AreEqual(seq.Complement.ToString(), deserializedSeq.Complement.ToString());
                Assert.AreEqual(seq.Complemented, deserializedSeq.Complemented);
                Assert.AreEqual(seq.Count, deserializedSeq.Count);
                Assert.AreEqual(seq.DisplayID, deserializedSeq.DisplayID);
                Assert.AreEqual(seq.Documentation, deserializedSeq.Documentation);
                Assert.AreEqual(seq.ID, deserializedSeq.ID);
                Assert.AreEqual(seq.IsReadOnly, deserializedSeq.IsReadOnly);
                Assert.AreEqual(seq.MoleculeType, deserializedSeq.MoleculeType);
                Assert.AreEqual(seq.RangeLength, deserializedSeq.RangeLength);
                Assert.AreEqual(seq.RangeStart, deserializedSeq.RangeStart);
                Assert.AreEqual(seq.Reverse.ToString(), deserializedSeq.Reverse.ToString());
                Assert.AreEqual(seq.ReverseComplement.ToString(), deserializedSeq.ReverseComplement.ToString());
                Assert.AreEqual(seq.Reversed, deserializedSeq.Reversed);
                Assert.AreEqual(seq.Source.ToString(), deserializedSeq.Source.ToString());
                Assert.AreEqual(seq.ToString(), deserializedSeq.ToString());
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        /// <summary>
        /// Test serialization of SequenceAlignment with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestSequenceAlignmentWithBinaryFormatter()
        {
            Stream stream = null;

            try
            {
                stream = File.Open("SequenceAlignment.data", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                Sequence seq1 = new Sequence(Alphabets.DNA, "ACGACTTACG");
                Sequence seq2 = new Sequence(Alphabets.DNA, "TACGATCCGGAAA");
                Sequence seq3 = new Sequence(Alphabets.DNA, "ACGACTTACGATCCGGAAA");
                PairwiseAlignedSequence seqAlignment = new PairwiseAlignedSequence();
                seqAlignment.FirstSequence = seq1;
                seqAlignment.SecondSequence = seq2;
                seqAlignment.Score = 30;
                seqAlignment.Consensus = seq3;
                PairwiseSequenceAlignment alignment = new PairwiseSequenceAlignment();
                alignment.PairwiseAlignedSequences.Add(seqAlignment);
                alignment.Documentation = "Aligned seq1 and seq2";
                formatter.Serialize(stream, alignment);
                stream.Seek(0, SeekOrigin.Begin);
                PairwiseSequenceAlignment deserializedseqAlignment = (PairwiseSequenceAlignment)formatter.Deserialize(stream);

                Assert.AreNotSame(alignment, deserializedseqAlignment);
                Assert.AreEqual(alignment.PairwiseAlignedSequences[0].Consensus.ToString(), deserializedseqAlignment.PairwiseAlignedSequences[0].Consensus.ToString());
                Assert.AreEqual(alignment.Documentation, deserializedseqAlignment.Documentation);
                Assert.AreEqual(alignment.IsReadOnly, deserializedseqAlignment.IsReadOnly);
                Assert.AreEqual(alignment.PairwiseAlignedSequences[0].Score, deserializedseqAlignment.PairwiseAlignedSequences[0].Score);
                Assert.AreEqual(alignment.PairwiseAlignedSequences.Count, deserializedseqAlignment.PairwiseAlignedSequences.Count);
                Assert.AreEqual(alignment.PairwiseAlignedSequences[0].FirstSequence.ToString(), deserializedseqAlignment.PairwiseAlignedSequences[0].FirstSequence.ToString());
                Assert.AreEqual(alignment.PairwiseAlignedSequences[0].SecondSequence.ToString(), deserializedseqAlignment.PairwiseAlignedSequences[0].SecondSequence.ToString());
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        /// <summary>
        /// Test serialization of Contig with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestContigWithBinaryFormatter()
        {
            Stream stream = null;

            try
            {
                stream = File.Open("Contig.data", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                Sequence seq1 = new Sequence(Alphabets.DNA, "ACGACTTACG");
                Contig.AssembledSequence assembledSeq1 = new Contig.AssembledSequence();
                assembledSeq1.Sequence = seq1;
                assembledSeq1.Position = 0;
                assembledSeq1.IsReversed = false;
                assembledSeq1.IsComplemented = false;

                Sequence seq2 = new Sequence(Alphabets.DNA, "TACGATCCGGAAA");
                Contig.AssembledSequence assembledSeq2 = new Contig.AssembledSequence();
                assembledSeq2.Sequence = seq2;
                assembledSeq2.Position = 6;
                assembledSeq2.IsReversed = false;
                assembledSeq2.IsComplemented = false;

                Sequence consensus = new Sequence(Alphabets.DNA, "ACGACTTACGATCCGGAAA");
                Contig contig = new Contig();
                contig.Sequences.Add(assembledSeq1);
                contig.Sequences.Add(assembledSeq2);
                contig.Consensus = consensus;

                formatter.Serialize(stream, contig);
                stream.Seek(0, SeekOrigin.Begin);

                Contig deserializedContig = (Contig)formatter.Deserialize(stream);
                Assert.AreNotSame(contig, deserializedContig);
                Assert.AreEqual(contig.Consensus.ToString(), deserializedContig.Consensus.ToString());
                Assert.AreEqual(contig.Length, deserializedContig.Length);
                Assert.AreEqual(contig.Sequences.Count, deserializedContig.Sequences.Count);

                for (int i = 0; i < contig.Sequences.Count; i++)
                {
                    Assert.AreEqual(
                        contig.Sequences[i].Sequence.ToString(),
                        deserializedContig.Sequences[i].Sequence.ToString());

                    Assert.AreEqual(
                        contig.Sequences[i].IsComplemented,
                        deserializedContig.Sequences[i].IsComplemented);

                    Assert.AreEqual(contig.Sequences[i].IsReversed, deserializedContig.Sequences[i].IsReversed);
                    Assert.AreEqual(contig.Sequences[i].Position, deserializedContig.Sequences[i].Position);
                }
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        /// <summary>
        /// Test serialization of OverlapDeNovoAssembly with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestSequenceAssemblyWithBinaryFormatter()
        {
            Stream stream = null;

            try
            {
                stream = File.Open("SequenceAssembly.data", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                IOverlapDeNovoAssembly seqAssembly = null;

                #region Create OverlapDeNovoAssembly by calling OverlapDeNovoAssembler.Assembly()

                int matchScore = 1;
                int mismatchScore = -8;
                int gapCost = -8;
                double mergeThreshold = 3;
                double consensusThreshold = 99;
                const bool AssumeOrientedReads = true;

                OverlapDeNovoAssembler assembler = new OverlapDeNovoAssembler();
                assembler.MergeThreshold = mergeThreshold;
                assembler.OverlapAlgorithm = new PairwiseOverlapAligner();
                ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).SimilarityMatrix = new DiagonalSimilarityMatrix(
                                                                    matchScore,
                                                                    mismatchScore,
                                                                    MoleculeType.DNA);
                ((IPairwiseSequenceAligner)assembler.OverlapAlgorithm).GapOpenCost = gapCost;
                assembler.ConsensusResolver = new SimpleConsensusResolver(consensusThreshold);
                assembler.AssumeStandardOrientation = AssumeOrientedReads;

                Sequence seq1 = new Sequence(Alphabets.DNA, "ACGACACG");
                Sequence seq2 = new Sequence(Alphabets.DNA, "ACGACCGGAGG");
                Sequence seq3 = new Sequence(Alphabets.DNA, "TTTTTT");
                seqAssembly = (IOverlapDeNovoAssembly)assembler.Assemble(new List<ISequence>() { seq1, seq2, seq3 });
                #endregion  Create OverlapDeNovoAssembly by calling OverlapDeNovoAssembler.Assembly()

                formatter.Serialize(stream, seqAssembly);
                stream.Seek(0, SeekOrigin.Begin);

                IOverlapDeNovoAssembly deserializedseqAssembly =
                    (IOverlapDeNovoAssembly)formatter.Deserialize(stream);
                Assert.AreNotSame(seqAssembly, deserializedseqAssembly);
                Assert.AreEqual(seqAssembly.Contigs.Count, deserializedseqAssembly.Contigs.Count);

                for (int i = 0; i < seqAssembly.Contigs.Count; i++)
                {
                    Assert.AreEqual(
                        seqAssembly.Contigs[i].Consensus.ToString(),
                        deserializedseqAssembly.Contigs[i].Consensus.ToString());
                    Assert.AreEqual(
                        seqAssembly.Contigs[i].Sequences.Count,
                        deserializedseqAssembly.Contigs[i].Sequences.Count);

                    for (int j = 0; j < seqAssembly.Contigs[i].Sequences.Count; j++)
                    {
                        Assert.AreEqual(
                            seqAssembly.Contigs[i].Sequences[j].ToString(),
                            deserializedseqAssembly.Contigs[i].Sequences[j].ToString());
                    }
                }

                for (int i = 0; i < seqAssembly.UnmergedSequences.Count; i++)
                {
                    Assert.AreEqual(
                        seqAssembly.UnmergedSequences[i].ToString(),
                        deserializedseqAssembly.UnmergedSequences[i].ToString());
                }
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        /// <summary>
        /// Test serialization of derived sequence with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestDerivedSequenceWithBinaryFormatter()
        {
            Stream stream = null;

            try
            {
                stream = File.Open("DerivedSequence.data", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();

                Sequence seq = new Sequence(Alphabets.RNA, "ACUGA");
                DerivedSequence derSeq = new DerivedSequence(seq);
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

                Assert.AreEqual(deserializedDerSeq.ToString(), derSeq.ToString());
                Assert.AreEqual(deserializedDerSeq[2], derSeq[2]);
                Assert.AreEqual(deserializedDerSeq[2].Symbol, derSeq[2].Symbol);
                Assert.AreEqual(deserializedDerSeq[2].Name, derSeq[2].Name);
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        /// <summary>
        /// Test serialization of GenBankMetadata with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestGenBankMetadataWithBinaryFormatter()
        {
            
            try
            {
                LocationBuilder locBuilder = new LocationBuilder();
                using (Stream stream = File.Open("GenbankMetadata.data", FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
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
                    formatter.Serialize(stream, metadata);
                    stream.Seek(0, SeekOrigin.Begin);

                    GenBankMetadata deserializedMetadta = (GenBankMetadata)formatter.Deserialize(stream);

                    Assert.AreEqual(deserializedMetadta.Accession.Primary, metadata.Accession.Primary);

                    for (int i = 0; i < deserializedMetadta.Accession.Secondary.Count; i++)
                    {
                        Assert.AreEqual(deserializedMetadta.Accession.Secondary[i], metadata.Accession.Secondary[i]);
                    }

                    Assert.AreEqual(deserializedMetadta.BaseCount, metadata.BaseCount);

                    for (int i = 0; i < deserializedMetadta.Comments.Count; i++)
                    {
                        Assert.AreEqual(deserializedMetadta.Comments[i], metadata.Comments[i]);
                    }

                    Assert.AreEqual(deserializedMetadta.Contig, metadata.Contig);

                    Assert.AreEqual(deserializedMetadta.DBLink.Type, metadata.DBLink.Type);

                    for (int i = 0; i < deserializedMetadta.DBLink.Numbers.Count; i++)
                    {
                        Assert.AreEqual(deserializedMetadta.DBLink.Numbers[i], metadata.DBLink.Numbers[i]);
                    }

                    Assert.AreEqual(deserializedMetadta.DBSource, metadata.DBSource);
                    Assert.AreEqual(deserializedMetadta.Definition, metadata.Definition);
                    for (int i = 0; i < deserializedMetadta.Features.All.Count; i++)
                    {
                        Assert.AreEqual(deserializedMetadta.Features.All[i].Key, metadata.Features.All[i].Key);
                        Assert.AreEqual(locBuilder.GetLocationString(deserializedMetadta.Features.All[i].Location), locBuilder.GetLocationString(metadata.Features.All[i].Location));

                        foreach (KeyValuePair<string, List<string>> kvp in deserializedMetadta.Features.All[i].Qualifiers)
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

                    Assert.AreEqual(deserializedMetadta.Keywords, metadata.Keywords);
                    Assert.AreEqual(deserializedMetadta.Locus.Date, metadata.Locus.Date);
                    Assert.AreEqual(deserializedMetadta.Locus.DivisionCode, metadata.Locus.DivisionCode);
                    Assert.AreEqual(deserializedMetadta.Locus.MoleculeType, metadata.Locus.MoleculeType);
                    Assert.AreEqual(deserializedMetadta.Locus.Name, metadata.Locus.Name);
                    Assert.AreEqual(deserializedMetadta.Locus.SequenceLength, metadata.Locus.SequenceLength);
                    Assert.AreEqual(deserializedMetadta.Locus.SequenceType, metadata.Locus.SequenceType);
                    Assert.AreEqual(deserializedMetadta.Locus.Strand, metadata.Locus.Strand);
                    Assert.AreEqual(deserializedMetadta.Locus.StrandTopology, metadata.Locus.StrandTopology);
                    Assert.AreEqual(deserializedMetadta.Origin, deserializedMetadta.Origin);
                    Assert.AreEqual(deserializedMetadta.Primary, metadata.Primary);
                    Assert.AreEqual(deserializedMetadta.Project.Name, metadata.Project.Name);
                    for (int i = 0; i < deserializedMetadta.Project.Numbers.Count; i++)
                    {
                        Assert.AreEqual(deserializedMetadta.Project.Numbers[i], metadata.Project.Numbers[i]);
                    }

                    for (int i = 0; i < deserializedMetadta.References.Count; i++)
                    {
                        Assert.AreEqual(deserializedMetadta.References[i].Authors, metadata.References[i].Authors);
                        Assert.AreEqual(deserializedMetadta.References[i].Consortiums, metadata.References[i].Consortiums);
                        Assert.AreEqual(deserializedMetadta.References[i].Journal, metadata.References[i].Journal);
                        Assert.AreEqual(deserializedMetadta.References[i].Location, metadata.References[i].Location);
                        Assert.AreEqual(deserializedMetadta.References[i].Medline, metadata.References[i].Medline);
                        Assert.AreEqual(deserializedMetadta.References[i].Number, metadata.References[i].Number);
                        Assert.AreEqual(deserializedMetadta.References[i].PubMed, metadata.References[i].PubMed);
                        Assert.AreEqual(deserializedMetadta.References[i].Remarks, metadata.References[i].Remarks);
                        Assert.AreEqual(deserializedMetadta.References[i].Title, metadata.References[i].Title);
                    }

                    Assert.AreEqual(deserializedMetadta.Segment.Current, metadata.Segment.Current);
                    Assert.AreEqual(deserializedMetadta.Segment.Count, metadata.Segment.Count);
                    Assert.AreEqual(deserializedMetadta.Source.CommonName, metadata.Source.CommonName);
                    Assert.AreEqual(deserializedMetadta.Source.Organism.ClassLevels, metadata.Source.Organism.ClassLevels);
                    Assert.AreEqual(deserializedMetadta.Source.Organism.Genus, metadata.Source.Organism.Genus);
                    Assert.AreEqual(deserializedMetadta.Source.Organism.Species, metadata.Source.Organism.Species);
                    Assert.AreEqual(deserializedMetadta.Version.Accession, metadata.Version.Accession);
                    Assert.AreEqual(deserializedMetadta.Version.CompoundAccession, metadata.Version.CompoundAccession);
                    Assert.AreEqual(deserializedMetadta.Version.GINumber, metadata.Version.GINumber);
                    Assert.AreEqual(deserializedMetadta.Version.Version, metadata.Version.Version);
                }
            }
            catch
            {
                Assert.Fail();
            }
           
        }

        /// <summary>
        /// Test serialization of GenBankMetadata features with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestGenBankFeaturesWithBinaryFormatter()
        {
           // Stream stream = null;

            try
            {
                using (Stream stream = File.Open("GenbankMetadata.data", FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
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
                    formatter.Serialize(stream, metadata);
                    stream.Seek(0, SeekOrigin.Begin);
                    GenBankMetadata deserializedMetadata = (GenBankMetadata)formatter.Deserialize(stream);
                    Assert.AreNotSame(metadata, deserializedMetadata);
                    Assert.AreEqual(deserializedMetadata.Features.All.Count, 743);
                    Assert.AreEqual(deserializedMetadata.Features.CodingSequences.Count, 117);
                    Assert.AreEqual(deserializedMetadata.Features.Exons.Count, 32);
                    Assert.AreEqual(deserializedMetadata.Features.Introns.Count, 22);
                    Assert.AreEqual(deserializedMetadata.Features.Genes.Count, 60);
                    Assert.AreEqual(deserializedMetadata.Features.MiscFeatures.Count, 455);
                    Assert.AreEqual(deserializedMetadata.Features.Promoters.Count, 17);
                    Assert.AreEqual(deserializedMetadata.Features.TransferRNAs.Count, 21);
                    Assert.AreEqual(deserializedMetadata.Features.All.FindAll(F => F.Key.Equals(StandardFeatureKeys.CodingSequence)).Count, 117);
                    Assert.AreEqual(deserializedMetadata.Features.CodingSequences[0].Translation.Trim('"'), metadata.Features.CodingSequences[0].GetTranslation().ToString());
                    Assert.AreEqual(deserializedMetadata.GetFeatures(11918, 12241).Count, 2);
                }
            }
            catch
            {
                Assert.Fail();
            }            
        }

        /// <summary>
        /// Test serialization of Qualitative sequence with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestQualitativeSequenceWithBinaryFormatter()
        {
            Stream stream = null;

            try
            {
                QualitativeSequence seq = new QualitativeSequence(Alphabets.DNA, FastQFormatType.Sanger, "ACGTACGT", 65);
                stream = File.Open("QualitativeSequence.data", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, seq);

                stream.Seek(0, SeekOrigin.Begin);
                QualitativeSequence deserializedSeq = (QualitativeSequence)formatter.Deserialize(stream);
                Assert.AreNotSame(seq, deserializedSeq);
                Assert.AreSame(seq.Alphabet, deserializedSeq.Alphabet);
                Assert.AreSame(seq.Encoding, deserializedSeq.Encoding);

                Assert.AreNotSame(seq.Encoder, deserializedSeq.Encoder);
                Assert.AreNotSame(seq.Decoder, deserializedSeq.Decoder);

                Assert.AreEqual(seq.Complement.ToString(), deserializedSeq.Complement.ToString());
                Assert.AreEqual(seq.Count, deserializedSeq.Count);
                Assert.AreEqual(seq.DisplayID, deserializedSeq.DisplayID);
                Assert.AreEqual(seq.ID, deserializedSeq.ID);
                Assert.AreEqual(seq.IsReadOnly, deserializedSeq.IsReadOnly);
                Assert.AreEqual(seq.MoleculeType, deserializedSeq.MoleculeType);
                Assert.AreEqual(seq.Reverse.ToString(), deserializedSeq.Reverse.ToString());
                Assert.AreEqual(seq.ReverseComplement.ToString(), deserializedSeq.ReverseComplement.ToString());
                Assert.AreEqual(seq.ToString(), deserializedSeq.ToString());
                Assert.AreEqual(seq.Statistics.GetCount('A'), seq.Statistics.GetCount('A'));
                foreach (byte qualScore in deserializedSeq.Scores)
                {
                    Assert.AreEqual(qualScore, 65);
                }
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

        /// <summary>
        /// Test serialization of Location with binary formatter.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        public void TestLocationWithBinaryFormatter()
        {
            Stream stream = null;

            try
            {
                Location loc = new Location();
                loc.Operator = LocationOperator.Complement;
                Location subLocation = new Location();
                subLocation.StartData = "100";
                subLocation.EndData = "200";
                subLocation.Separator = "..";
                subLocation.Accession = "123";
                loc.SubLocations.Add(subLocation);
                stream = File.Open("Location.data", FileMode.Create);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, loc);

                stream.Seek(0, SeekOrigin.Begin);
                Location deserializedLoc = (Location)formatter.Deserialize(stream);

                Assert.AreEqual(loc.StartData, deserializedLoc.StartData);
                Assert.AreEqual(loc.EndData, deserializedLoc.EndData);
                Assert.AreEqual(loc.Start, deserializedLoc.Start);
                Assert.AreEqual(loc.End, deserializedLoc.End);
                Assert.AreEqual(loc.Operator, deserializedLoc.Operator);
                Assert.AreEqual(loc.SubLocations.Count, deserializedLoc.SubLocations.Count);
                Assert.AreEqual(loc.SubLocations[0].StartData, deserializedLoc.SubLocations[0].StartData);
                Assert.AreEqual(loc.SubLocations[0].EndData, deserializedLoc.SubLocations[0].EndData);
                Assert.AreEqual(loc.SubLocations[0].Separator, deserializedLoc.SubLocations[0].Separator);
                Assert.AreEqual(loc.SubLocations[0].Accession, deserializedLoc.SubLocations[0].Accession);
                Assert.AreEqual(loc.SubLocations[0].Start, deserializedLoc.SubLocations[0].Start);
                Assert.AreEqual(loc.SubLocations[0].End, deserializedLoc.SubLocations[0].End);
            }
            catch
            {
                Assert.Fail();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }

    }
}
