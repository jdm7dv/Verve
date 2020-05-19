// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using Bio.Algorithms.Alignment;
using Bio.Algorithms.Assembly.Padena;
using Bio.Algorithms.Assembly.Padena.Scaffold;
using Bio.Algorithms.Assembly.Comparative.Properties;

namespace Bio.Algorithms.Assembly.Comparative
{
    /// <summary>
    /// Implements a comparative genome assembly for
    /// assembly of DNA sequences.
    /// </summary>
    public class ComparativeGenomeAssembler
    {
        /// <summary>
        /// kmer Length.
        /// </summary>
        private int kmerLength = 10;

        /// <summary>
        /// Length of MUM. 
        /// </summary>
        private int lengthOfMum = 20;

        /// <summary>
        /// Default depth for graph traversal in scaffold builder step.
        /// </summary>
        private int depth = 10;

        /// <summary>
        /// Break Length.
        /// </summary>
        private int breakLength = 1;

        #region Properties
        /// <summary>
        /// Gets or sets the kmer length.
        /// </summary>
        public int KmerLength
        {
            get
            {
                return this.kmerLength;
            }

            set
            {
                this.kmerLength = value;
                this.AllowKmerLengthEstimation = false;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to estimate kmer length.
        /// </summary>
        public bool AllowKmerLengthEstimation { get; set; }

        /// <summary>
        /// Gets or sets value of redundancy for building scaffolds.
        /// </summary>
        public int ScaffoldRedundancy { get; set; }

        /// <summary>
        /// Gets or sets whether to run scaffolding step or not.
        /// </summary>
        public bool ScaffoldingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the Depth for graph traversal in scaffold builder step.
        /// </summary>
        public int Depth
        {
            get
            {
                return depth;
            }
            set
            {
                depth = value;
            }
        }

        /// <summary>
        /// Gets the name of the sequence assembly algorithm being
        /// implemented. This is intended to give the
        /// developer some information of the current sequence assembly algorithm.
        /// </summary>
        public string Name
        {
            get { return Properties.Resources.PcaName; }
        }

        /// <summary>
        /// Gets the description of the sequence assembly algorithm being
        /// implemented. This is intended to give the
        /// developer some information of the current sequence assembly algorithm.
        /// </summary>
        public string Description
        {
            get { return Properties.Resources.PcaDescription; }
        }

        /// <summary>
        /// Gets or sets the length of MUM for using with NUCmer.
        /// </summary>
        public int LengthOfMum
        {
            get
            {
                return lengthOfMum;
            }

            set
            {
                lengthOfMum = value;
            }
        }

        /// <summary>
        /// Gets or sets number of bases to be extended before stopping alignment in NUCmer
        /// </summary>
        private int BreakLength
        {
            get
            {
                return breakLength;
            }

            set
            {
                breakLength = value;
            }
        }

        #endregion

        /// <summary>
        /// Assemble the input sequences into the largest possible contigs. 
        /// </summary>
        /// <param name="referenceSequence">The sequence used as backbone for assembly.</param>
        /// <param name="reads">The sequences to assemble.</param>
        /// <returns>IComparativeAssembly instance which contains list of assembled sequences.</returns>
        public IEnumerable<ISequence> Assemble(IEnumerable<ISequence> referenceSequence, IEnumerable<ISequence> reads)
        {
            // Converting to list to avoid multiple parse of the reference file if its a yeild return
            referenceSequence = referenceSequence.ToList();
            reads = reads.ToList();

            //Comparative Assembly Steps
            //1) Read Alignment (Calling NUCmer for aligning reads to reference sequence)
            IList<IEnumerable<DeltaAlignment>> alignmentBetweenReferenceAndReads = this.ReadAlignment(referenceSequence, reads.Where( a => a.Count >= LengthOfMum));

            // 2) Repeat Resolution
            IList<DeltaAlignment> repeatResolvedDeltas = this.RepeatResolution(alignmentBetweenReferenceAndReads);

            List<DeltaAlignment> orderedRepeatResolvedDeltas = repeatResolvedDeltas.OrderBy(a => a.FirstSequenceStart).ToList();
 
            // 3) Layout Refinement
            LayoutRefinment(orderedRepeatResolvedDeltas);
 
            // 4) Consensus Generation
            IEnumerable<ISequence> contigs = this.ConsensusGenerator(orderedRepeatResolvedDeltas);

            if (ScaffoldingEnabled)
            {
                // 5) Scaffold Generation
                IEnumerable<ISequence> scaffolds = ScaffoldsGenerator(contigs, reads);

                return scaffolds;
            }
            else
            {
                return contigs;
            }
        }

        #region Assembler Steps
        /// <summary>
        /// Build scaffolds from contigs and paired reads (uses Padena Step 6 for assembly).
        /// </summary>
        /// <param name="contigs">List of contigs.</param>
        /// <param name="reads">List of paired reads.</param>
        /// <returns>List of scaffold sequences.</returns>
        private IEnumerable<ISequence> ScaffoldsGenerator(IEnumerable<ISequence> contigs, IEnumerable<ISequence> reads)
        {
            using (GraphScaffoldBuilder scaffoldBuilder = new GraphScaffoldBuilder())
            {
                return scaffoldBuilder.BuildScaffold(reads, contigs.ToList(), this.KmerLength, this.Depth, this.ScaffoldRedundancy);
            }
        }

        /// <summary>
        /// Generates a consensus sequence for the genomic region covered by reads.
        /// </summary>
        /// <param name="alignmentBetweenReferenceAndReads">Alignment between reference genome and reads.</param>
        /// <returns>List of contigs.</returns>
        private IEnumerable<ISequence> ConsensusGenerator(IEnumerable<DeltaAlignment> alignmentBetweenReferenceAndReads)
        {
            return ConsensusGeneration.GenerateConsensus(alignmentBetweenReferenceAndReads);
        }

        /// <summary>
        /// Refines layout of alignment between reads and reference genome by taking care of indels and rearrangements.
        /// </summary>
        /// <param name="orderedRepeatResolvedDeltas">Ordered Repeat Resolved Deltas.</param>
        private void LayoutRefinment(IList<DeltaAlignment> orderedRepeatResolvedDeltas)
        {
            LayoutRefiner.RefineLayout(orderedRepeatResolvedDeltas);
        }

        /// <summary>
        /// Reads ambiguously placed due to genomic reads.
        /// This step requires mate pair information to resolve the ambiguity about placements of repeated sequences.
        /// </summary>
        /// <param name="alignmentBetweenReferenceAndReads">Alignment between reference genome and reads.</param>
        /// <returns>List of DeltaAlignments after resolving repeating reads.</returns>
        private List<DeltaAlignment> RepeatResolution(IList<IEnumerable<DeltaAlignment>> alignmentBetweenReferenceAndReads)
        {
            return RepeatResolver.ResolveAmbiguity(alignmentBetweenReferenceAndReads);
        }

        /// <summary>
        /// Aligns reads to reference genome using NUCmer.
        /// </summary>
        /// <param name="referenceSequence">Sequence of reference genome.</param>
        /// <param name="reads">List of sequence reads.</param>
        /// <returns>Delta alignments after read alignment.</returns>
        private IList<IEnumerable<DeltaAlignment>> ReadAlignment(IEnumerable<ISequence> referenceSequence, IEnumerable<ISequence> reads)
        {
              IList<List<IEnumerable<DeltaAlignment>>> delta;
              List<IEnumerable<DeltaAlignment>> deltaAlignments = new List<IEnumerable<DeltaAlignment>>();
                delta = referenceSequence.AsParallel().Select(sequence =>
                {
                    NUCmer nucmer = new NUCmer();

                    // TODO: Update these hardcoded values with the onces used in AMOSS while validating functionality with real data.
                    nucmer.FixedSeparation = 0;
                    nucmer.MinimumScore = 1;
                    nucmer.SeparationFactor = 0;
                    nucmer.MaximumSeparation = 0;
                    nucmer.BreakLength = BreakLength;
                    nucmer.LengthOfMUM = LengthOfMum;
                    return nucmer.GetDeltaAlignments(new List<ISequence> { sequence }, reads, false);
                }).ToList();

                foreach (var alignment in delta)
                {
                    foreach (var align in alignment)
                    {
                        deltaAlignments.Add(align);
                    }
                }
                return deltaAlignments;
        }
        #endregion
    }
}
