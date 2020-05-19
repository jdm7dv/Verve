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
using System.Globalization;
using System.Linq;
using Bio.Algorithms.MUMmer;
using Bio.SimilarityMatrices;
using Bio.Util.Logging;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// Class which uses nucmer to perform an alignment.
    /// </summary>
    public class NucmerPairwiseAligner : ISequenceAligner
    {
        // Nucmer algorithm.
        private NUCmer nucmerAlgo = new NUCmer();

        /// <summary>
        /// Gets or Sets the name of the algorithm.
        /// </summary>
        public string Name
        {
            get { return Bio.Properties.Resource.NUCMER; }
        }

        /// <summary>
        /// Gets or Sets the description of the algorithm.
        /// </summary>
        public string Description
        {
            get { return Bio.Properties.Resource.NUCMERDESC; }
        }

        /// <summary>
        /// Gets or sets maximum fixed diagonal difference.
        /// </summary>
        public int FixedSeparation
        {
            get
            {
                return this.nucmerAlgo.FixedSeparation;
            }

            set
            {
                this.nucmerAlgo.FixedSeparation = value;
            }
        }

        /// <summary>
        /// Gets or sets number of bases to be extended before stopping alignment.
        /// </summary>
        public int BreakLength
        {
            get
            {
                return this.nucmerAlgo.BreakLength;
            }

            set
            {
                this.nucmerAlgo.BreakLength = value;
            }
        }

        /// <summary>
        /// Gets or sets minimum output score.
        /// </summary>
        public int MinimumScore
        {
            get
            {
                return this.nucmerAlgo.MinimumScore;
            }

            set
            {
                this.nucmerAlgo.MinimumScore = value;
            }
        }

        /// <summary>
        /// Gets or sets separation factor. Fraction equal to 
        /// (diagonal difference / match separation) where higher values
        /// increase the insertion or deletion (indel) tolerance.
        /// </summary>
        public float SeparationFactor
        {
            get
            {
                return this.nucmerAlgo.SeparationFactor;
            }

            set
            {
                this.nucmerAlgo.SeparationFactor = value;
            }
        }

        /// <summary>
        /// Gets or sets minimum length of Match that can be considered as MUM.
        /// </summary>
        public long LengthOfMUM
        {
            get
            {
                return this.nucmerAlgo.LengthOfMUM;
            }

            set
            {
                this.nucmerAlgo.LengthOfMUM = value;
            }
        }

        /// <summary>
        /// Gets or sets maximum separation between the adjacent matches in clusters.
        /// </summary>
        public int MaximumSeparation
        {
            get
            {
                return this.nucmerAlgo.MaximumSeparation;
            }

            set
            {
                this.nucmerAlgo.MaximumSeparation = value;
            }
        }

        /// <summary>
        /// Gets or sets the consensus resolver attached with this aligner.
        /// </summary>
        public IConsensusResolver ConsensusResolver
        {
            get
            {
                return this.nucmerAlgo.ConsensusResolver;
            }

            set
            {
                this.nucmerAlgo.ConsensusResolver = value;
            }
        }

        /// <summary>
        /// Gets or sets the similarity matrix associated with this aligner.
        /// </summary>
        public SimilarityMatrix SimilarityMatrix
        {
            get
            {
                return this.nucmerAlgo.SimilarityMatrix;
            }

            set
            {
                this.nucmerAlgo.SimilarityMatrix = value;
            }
        }

        /// <summary>
        /// Gets or sets the Gap Open Cost.
        /// </summary>
        public int GapOpenCost
        {
            get
            {
                return this.nucmerAlgo.GapOpenCost;
            }

            set
            {
                this.nucmerAlgo.GapOpenCost = value;
            }
        }

        /// <summary>
        /// Gets or sets the Gap Extension Cost.
        /// </summary>
        public int GapExtensionCost
        {
            get
            {
                return this.nucmerAlgo.GapExtensionCost;
            }

            set
            {
                this.nucmerAlgo.GapExtensionCost = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to run Align or AlignSimple.
        /// </summary>
        protected bool IsAlign { get; set; }

        #region public methods
        /// <summary>
        /// AlignSimple aligns the set of input sequences using the linear gap model (one gap penalty), 
        /// and returns the best alignment found.
        /// </summary>
        /// <param name="inputSequences">The sequences to align.</param>
        /// <returns>List of sequence alignments.</returns>
        public IList<ISequenceAlignment> AlignSimple(IEnumerable<ISequence> inputSequences)
        {
            this.IsAlign = false;

            if (inputSequences.Count() < 2)
            {
                throw new ArgumentException(Properties.Resource.MinimumTwoSequences);
            }

            return this.Alignment(new List<ISequence> { inputSequences.ElementAt(0) }, inputSequences).ToList().ConvertAll(SA => SA as ISequenceAlignment);
        }

        /// <summary>
        /// Align aligns the set of input sequences using the affine gap model (gap open and gap extension penalties)
        /// and returns the best alignment found.
        /// </summary>
        /// <param name="inputSequences">The sequences to align.</param>
        public IList<ISequenceAlignment> Align(IEnumerable<ISequence> inputSequences)
        {
            this.IsAlign = true;

            if (inputSequences.Count() < 2)
            {
                throw new ArgumentException(Properties.Resource.MinimumTwoSequences);
            }

            return this.Alignment(new List<ISequence> { inputSequences.ElementAt(0) }, inputSequences).ToList().ConvertAll(SA => SA as ISequenceAlignment);
        }

        /// <summary>
        /// Align aligns the set of input sequences using the affine gap model (gap open and gap extension penalties)
        /// and returns the best alignment found.
        /// </summary>
        /// <param name="referenceList">Reference sequences.</param>
        /// <param name="queryList">Query sequences.</param>
        public IList<ISequenceAlignment> Align(IEnumerable<ISequence> referenceList, IEnumerable<ISequence> queryList)
        {
            this.IsAlign = true;

            if (referenceList.Count() + queryList.Count() < 2)
            {
                throw new ArgumentException(Properties.Resource.MinimumTwoSequences);
            }

            return this.Alignment(referenceList, queryList).ToList().ConvertAll(SA => SA as ISequenceAlignment);
        }

        /// <summary>
        /// Align aligns the set of input sequences using the affine gap model (gap open and gap extension penalties)
        /// and returns the best alignment found.
        /// </summary>
        /// <param name="reference">Reference sequence.</param>
        /// <param name="querySequences">Query sequences.</param>
        public IList<ISequenceAlignment> Align(ISequence reference, IEnumerable<ISequence> querySequences)
        {
            this.IsAlign = true;

            if (querySequences.Count() < 1)
            {
                throw new ArgumentException(Properties.Resource.MinimumTwoSequences);
            }

            return this.Alignment(new List<ISequence> { reference }, querySequences).ToList().ConvertAll(SA => SA as ISequenceAlignment);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Concat all the sequences into one sequence with special character.
        /// </summary>
        /// <param name="sequences">List of reference sequence.</param>
        /// <returns>Concatenated sequence.</returns>
        protected static ISequence ConcatSequence(IEnumerable<ISequence> sequences)
        {
            if (sequences == null)
            {
                throw new ArgumentNullException("sequences");
            }

            // Add the Concatenating symbol to every sequence in reference list
            // Note that, as of now protein sequence is being created out 
            // of input sequence add  concatenation character "X" to it to 
            // simplify implemenation.
            List<byte> referenceSequence = null;
            foreach (ISequence sequence in sequences)
            {
                if (null == referenceSequence)
                {
                    referenceSequence = new List<byte>(sequence);
                }
                else
                {
                    referenceSequence.AddRange(sequence);
                }

                // Add conctatinating symbol.
                referenceSequence.Add((byte)'+');
            }

            return new Sequence(AlphabetExtensions.GetMummerAlphabet(sequences.ElementAt(0).Alphabet), referenceSequence.ToArray(), false);
        }

        /// <summary>
        /// Analyze the given seqquences and store a consensus into its Consensus property.
        /// </summary>
        /// <param name="referenceSequence">Reference sequence.</param>
        /// <param name="querySequence">Query sequence.</param>
        /// <returns>Consensus of sequences.</returns>
        protected ISequence MakeConsensus(
                ISequence referenceSequence,
                ISequence querySequence)
        {
            if (referenceSequence == null)
            {
                throw new ArgumentNullException("referenceSequence");
            }

            if (querySequence == null)
            {
                throw new ArgumentNullException("querySequence");
            }

            // For each pair of symbols (characters) in reference and query sequence
            // get the consensus symbol and append it.
            byte[] consensus = new byte[referenceSequence.Count];
            for (int index = 0; index < referenceSequence.Count; index++)
            {
                consensus[index] = this.ConsensusResolver.GetConsensus(
                    new byte[] { referenceSequence[index], querySequence[index] });
            }

            IAlphabet alphabet = Alphabets.AutoDetectAlphabet(consensus, 0, consensus.LongLength, referenceSequence.Alphabet);
            return new Sequence(alphabet, consensus, false);
        }

        /// <summary>
        /// Calculate the score of alignment.
        /// </summary>
        /// <param name="referenceSequence">Reference sequence.</param>
        /// <param name="querySequence">Query sequence.</param>
        /// <returns>Score of the alignment.</returns>
        protected int CalculateScore(
                ISequence referenceSequence,
                ISequence querySequence)
        {
            if (referenceSequence == null)
            {
                throw new ArgumentNullException("referenceSequence");
            }

            if (querySequence == null)
            {
                throw new ArgumentNullException("querySequence");
            }

            int index, score, gapCount;
            byte referenceCharacter, queryCharacter;

            score = 0;

            // For each pair of symbols (characters) in reference and query sequence
            // 1. If the character are different and not alignment character "-", 
            //      then find the cost form Similarity Matrix
            // 2. If Gap Extension cost needs to be used
            //      a. Find how many gaps exists in appropriate sequence (reference / query)
            //          and calculate the score
            // 3. Add the gap open cost
            for (index = 0; index < referenceSequence.Count; index++)
            {
                referenceCharacter = referenceSequence[index];
                queryCharacter = querySequence[index];

                if (DnaAlphabet.Instance.Gap != referenceCharacter
                    && DnaAlphabet.Instance.Gap != queryCharacter)
                {
                    score += SimilarityMatrix[referenceCharacter, queryCharacter];
                }
                else
                {
                    if (this.IsAlign)
                    {
                        if (DnaAlphabet.Instance.Gap == referenceCharacter)
                        {
                            gapCount = FindExtensionLength(referenceSequence, index);
                        }
                        else
                        {
                            gapCount = FindExtensionLength(querySequence, index);
                        }

                        score += this.GapOpenCost + (gapCount * this.GapExtensionCost);

                        // move the index pointer to end of extension
                        index = index + gapCount - 1;
                    }
                    else
                    {
                        score += this.GapOpenCost;
                    }
                }
            }

            return score;
        }

        /// <summary>
        /// Find the index of extension.
        /// </summary>
        /// <param name="sequence">Sequence object.</param>
        /// <param name="index">Position at which extension starts.</param>
        /// <returns>Last index of extension.</returns>
        private static int FindExtensionLength(ISequence sequence, int index)
        {
            // Find the number of alignment characters "-" in the given sequence 
            // from positon index
            int gapCounter = index;

            while (gapCounter < sequence.Count
                    && DnaAlphabet.Instance.Gap == sequence[gapCounter])
            {
                gapCounter++;
            }

            return gapCounter - index;
        }

        /// <summary>
        /// This method is considered as main execute method which defines the
        /// step by step algorithm. Drived class flows the defined flow by this
        /// method.
        /// </summary>
        /// <param name="referenceSequenceList">Reference sequence.</param>
        /// <param name="querySequenceList">List of input sequences.</param>
        /// <returns>A list of sequence alignment.</returns>
        private IList<IPairwiseSequenceAlignment> Alignment(IEnumerable<ISequence> referenceSequenceList, IEnumerable<ISequence> querySequenceList)
        {
            this.ConsensusResolver = new SimpleConsensusResolver(referenceSequenceList.ElementAt(0).Alphabet);

            IList<IPairwiseSequenceAlignment> results = new List<IPairwiseSequenceAlignment>();
            IPairwiseSequenceAlignment sequenceAlignment = null;
            IList<IEnumerable<DeltaAlignment>> deltaAlignments = null;
            IList<PairwiseAlignedSequence> alignments = null;

            // Validate the input
            Validate(referenceSequenceList.ElementAt(0), querySequenceList);

            deltaAlignments = this.nucmerAlgo.GetDeltaAlignments(referenceSequenceList, querySequenceList);

            ISequence concatReference = ConcatSequence(referenceSequenceList);

            // On each query sequence aligned with reference sequence
            IEnumerator<ISequence> qryEnumerator = querySequenceList.GetEnumerator();
            foreach (var delta in deltaAlignments)
            {
                qryEnumerator.MoveNext();
                sequenceAlignment = new PairwiseSequenceAlignment(concatReference, qryEnumerator.Current);

                // Convert delta alignments to sequence alignments
                alignments = ConvertDeltaToAlignment(delta);

                if (alignments.Count > 0)
                {
                    foreach (PairwiseAlignedSequence align in alignments)
                    {
                        // Calculate the score of alignment
                        align.Score = this.CalculateScore(
                                align.FirstSequence,
                                align.SecondSequence);

                        // Make Consensus
                        align.Consensus = this.MakeConsensus(
                                align.FirstSequence,
                                align.SecondSequence);

                        sequenceAlignment.PairwiseAlignedSequences.Add(align);
                    }
                }

                results.Add(sequenceAlignment);
            }

            return results;
        }

        /// <summary>
        /// Validate the inputs.
        /// </summary>
        /// <param name="referenceSequence">Reference sequence.</param>
        /// <param name="querySequenceList">List of input sequences.</param>
        /// <returns>Are inputs valid.</returns>
        private static bool Validate(
                ISequence referenceSequence,
                IEnumerable<ISequence> querySequenceList)
        {
            IAlphabet alphabetSet;

            if (referenceSequence == null)
            {
                string message = Properties.Resource.ReferenceListCannotBeNull;
                Trace.Report(message);
                throw new ArgumentNullException("referenceSequence");
            }

            if (querySequenceList == null)
            {
                string message = Properties.Resource.QueryListCannotBeNull;
                Trace.Report(message);
                throw new ArgumentNullException("querySequenceList");
            }

            alphabetSet = referenceSequence.Alphabet;

            // only DNA or RNA will work
            if (!(alphabetSet is DnaAlphabet || alphabetSet is RnaAlphabet))
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resource.OnlyDNAOrRNAInput,
                    "NUCmer");
                Trace.Report(message);
                throw new ArgumentException(message);
            }

            return true;
        }

        /// <summary>
        /// Convert to delta alignments to sequence alignments.
        /// </summary>
        /// <param name="alignments">List of delta alignments.</param>
        /// <returns>List of Sequence alignment.</returns>
        private static IList<PairwiseAlignedSequence> ConvertDeltaToAlignment(
                IEnumerable<DeltaAlignment> alignments)
        {
            if (alignments == null)
            {
                throw new ArgumentNullException("alignments");
            }

            IList<PairwiseAlignedSequence> alignedSequences = null;
            PairwiseAlignedSequence alignedSequence;
            long referenceStart, queryStart, difference;

            alignedSequences = new List<PairwiseAlignedSequence>();
            foreach (DeltaAlignment deltaAlignment in alignments)
            {
                alignedSequence = deltaAlignment.ConvertDeltaToSequences();

                // Find the offsets
                referenceStart = deltaAlignment.FirstSequenceStart;
                queryStart = deltaAlignment.SecondSequenceStart;
                difference = referenceStart - queryStart;
                if (0 < difference)
                {
                    alignedSequence.FirstOffset = 0;
                    alignedSequence.SecondOffset = difference;
                }
                else
                {
                    alignedSequence.FirstOffset = -1 * difference;
                    alignedSequence.SecondOffset = 0;
                }

                alignedSequences.Add(alignedSequence);
            }

            return alignedSequences;
        }
        #endregion
    }
}
