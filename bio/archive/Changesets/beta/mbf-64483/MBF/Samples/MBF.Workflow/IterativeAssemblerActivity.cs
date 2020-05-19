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
using MBF.Algorithms.Assembly;

namespace MBF.Workflow
{
    [Name("Iterative Assembler")]
    [Description("Given a list of input sequences, this activity creates an assembly object of the contigs and a consensus from running an assembly algorithm.")]
    [WorkflowCategory("Bioinformatics")]
    
    public class IterativeAssemblerActivity : Activity
    {
        #region Dependency Properties

        public static DependencyProperty SequencesProperty = 
            DependencyProperty.Register("Sequences", typeof(IList<ISequence>), 
            typeof(IterativeAssemblerActivity));

        [RequiredInputParam]
        [Name("Sequences")]
        [Description("A list of sequences to assemble.")]
        public IList<ISequence> Sequences
        {
            get { return ((IList<ISequence>)(base.GetValue(IterativeAssemblerActivity.SequencesProperty))); }
            set { base.SetValue(IterativeAssemblerActivity.SequencesProperty, value); }
        }

        public static DependencyProperty ContigsProperty = 
            DependencyProperty.Register("Contigs", typeof(List<Contig>), 
            typeof(IterativeAssemblerActivity));

        [OutputParam]
        [Name("Contigs")]
        [Description(@"A list of contigs produced by the assembly algorithm.")]
        public IList<Contig> Contigs
        {
            get { return ((IList<Contig>)(base.GetValue(IterativeAssemblerActivity.ContigsProperty))); }
            set { base.SetValue(IterativeAssemblerActivity.ContigsProperty, value); }
        }

        public static DependencyProperty ConsensusProperty = 
            DependencyProperty.Register("Consensus", typeof(IDeNovoAssembly), 
            typeof(IterativeAssemblerActivity));

        [OutputParam]
        [Name("Consensus")]
        [Description(@"The result of ISequence or Consensus.")]
        public IDeNovoAssembly Consensus
        {
            get { return ((IDeNovoAssembly)(base.GetValue(IterativeAssemblerActivity.ConsensusProperty))); }
            set { base.SetValue(IterativeAssemblerActivity.ConsensusProperty, value); }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            OverlapDeNovoAssembler SSA = new OverlapDeNovoAssembler();
            Consensus= SSA.Assemble(Sequences);
            Contigs = ((IOverlapDeNovoAssembly)Consensus).Contigs;

            return ActivityExecutionStatus.Closed;
        }
    }
}
