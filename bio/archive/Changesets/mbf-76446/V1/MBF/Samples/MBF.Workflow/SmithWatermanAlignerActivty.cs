// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Workflow.ComponentModel;

using Microsoft.Research.ScientificWorkflow;

using MBF;
using MBF.IO;
using MBF.IO.GenBank;
using MBF.IO.Fasta;
using MBF.Algorithms.Alignment;
using MBF.SimilarityMatrices;

namespace MBF.Workflow
{
    [Name("Smith-Waterman Aligner")]
    [Description("Given two input sequences, this activity creates an alignment of the two sequences.")]
    [WorkflowCategory("Bioinformatics")]

    public class SmithWatermanAlignerActivty : Activity
    {
        #region Dependency Properties

        public static DependencyProperty Sequence1Property =
            DependencyProperty.Register("Sequence1", typeof(ISequence),
            typeof(SmithWatermanAlignerActivty));

        [RequiredInputParam]
        [Name("First Sequence")]
        [Description(@"The reference sequence to align against.")]
        public ISequence Sequence1
        {
            get { return ((ISequence)(base.GetValue(SmithWatermanAlignerActivty.Sequence1Property))); }
            set { base.SetValue(SmithWatermanAlignerActivty.Sequence1Property, value); }
        }

        public static DependencyProperty Sequence2Property =
            DependencyProperty.Register("Sequence2", typeof(ISequence),
            typeof(SmithWatermanAlignerActivty));

        [RequiredInputParam]
        [Name("Second Sequence")]
        [Description(@"The sequence to align.")]
        public ISequence Sequence2
        {
            get { return ((ISequence)(base.GetValue(SmithWatermanAlignerActivty.Sequence2Property))); }
            set { base.SetValue(SmithWatermanAlignerActivty.Sequence2Property, value); }
        }

        public static DependencyProperty SimilarityMatrixProperty =
            DependencyProperty.Register("SimilarityMatrix", typeof(SimilarityMatrix),
            typeof(SmithWatermanAlignerActivty));

        [InputParam]
        [Name("Similarity Matrix")]
        [Description(@"The similarity matrix used to manage comparison costs.")]
        public SimilarityMatrix SimilarityMatrix
        {
            get { return ((SimilarityMatrix)(base.GetValue(SmithWatermanAlignerActivty.SimilarityMatrixProperty))); }
            set { base.SetValue(SmithWatermanAlignerActivty.SimilarityMatrixProperty, value); }
        }

        public static DependencyProperty GapPenaltyProperty =
            DependencyProperty.Register("GapPenalty", typeof(int),
            typeof(SmithWatermanAlignerActivty),
            new PropertyMetadata(-2));

        [InputParam]
        [Name("Gap Penalty")]
        [Description(@"The penalty for adding gaps during alignment.")]
        public int GapPenalty
        {
            get { return ((int)(base.GetValue(SmithWatermanAlignerActivty.GapPenaltyProperty))); }
            set { base.SetValue(SmithWatermanAlignerActivty.GapPenaltyProperty, value); }
        }

        public static DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(IList<IPairwiseSequenceAlignment>),
            typeof(SmithWatermanAlignerActivty));

        [OutputParam]
        [Name("Alignment")]
        [Description(@"The sequence alignment produced by running the algorithm.")]
        public IList<IPairwiseSequenceAlignment> Result
        {
            get
            {
                return ((IList<IPairwiseSequenceAlignment>)(base.GetValue(SmithWatermanAlignerActivty.ResultProperty)));
            }
            set
            {
                base.SetValue(SmithWatermanAlignerActivty.ResultProperty, value);
            }
        }

        public static DependencyProperty ConsensusProperty =
            DependencyProperty.Register("Consensus", typeof(ISequence),
            typeof(SmithWatermanAlignerActivty));

        [OutputParam]
        [Name("Consensus")]
        [Description(@"The consensus sequence from the alignment result.")]
        public ISequence Consensus
        {
            get { return ((ISequence)(base.GetValue(SmithWatermanAlignerActivty.ConsensusProperty))); }
            set { base.SetValue(SmithWatermanAlignerActivty.ConsensusProperty, value); }
        }

        public static DependencyProperty Result1Property =
            DependencyProperty.Register("Result1", typeof(ISequence),
            typeof(SmithWatermanAlignerActivty));

        [OutputParam]
        [Name("First Result")]
        [Description(@"The first modified result sequence.")]
        public ISequence Result1
        {
            get { return ((ISequence)(base.GetValue(SmithWatermanAlignerActivty.Result1Property))); }
            set { base.SetValue(SmithWatermanAlignerActivty.Result1Property, value); }
        }

        public static DependencyProperty Result2Property =
            DependencyProperty.Register("Result2", typeof(ISequence),
            typeof(SmithWatermanAlignerActivty));

        [OutputParam]
        [Name("Second Result")]
        [Description(@"The second modified result sequence.")]
        public ISequence Result2
        {
            get { return ((ISequence)(base.GetValue(SmithWatermanAlignerActivty.Result2Property))); }
            set { base.SetValue(SmithWatermanAlignerActivty.Result2Property, value); }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            SmithWatermanAligner SWA = new SmithWatermanAligner();
            Result = SWA.AlignSimple(SimilarityMatrix, GapPenalty, Sequence1, Sequence2);
            if (Result.Count >= 1 && Result[0].PairwiseAlignedSequences.Count >= 1)
            {
                Result1 = Result[0].PairwiseAlignedSequences[0].FirstSequence;
                Result2 = Result[0].PairwiseAlignedSequences[0].SecondSequence;
                Consensus = Result[0].PairwiseAlignedSequences[0].Consensus;
            }

            return ActivityExecutionStatus.Closed;
        }
    }
}
