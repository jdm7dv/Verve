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
using MBF.Algorithms.Translation;

namespace MBF.Workflow
{
    [Name("RNA Reverse Transcription")]
    [Description("Given an RNA sequence, this activity produces the reverse transcription DNA sequence.")]
    [WorkflowCategory("Bioinformatics")]

    public class RnaReverseTranscriptionActivty : Activity
    {
        #region Dependency Properties
        public static DependencyProperty RnaProperty =
            DependencyProperty.Register("Rna", typeof(ISequence),
            typeof(RnaReverseTranscriptionActivty));

        [RequiredInputParam]
        [Name("RNA")]
        [Description(@"The RNA sequence to which reverse transcription will be applied.")]
        public ISequence Rna
        {
            get { return ((ISequence)(base.GetValue(RnaReverseTranscriptionActivty.RnaProperty))); }
            set { base.SetValue(RnaReverseTranscriptionActivty.RnaProperty, value); }
        }

        public static DependencyProperty DnaProperty =
            DependencyProperty.Register("Dna", typeof(ISequence),
            typeof(RnaReverseTranscriptionActivty));

        [OutputParam]
        [Name("DnaOutput")]
        [Description(@"The DNA sequence as an output.")]
        public ISequence Dna
        {
            get { return ((ISequence)(base.GetValue(RnaReverseTranscriptionActivty.DnaProperty))); }
            set { base.SetValue(RnaReverseTranscriptionActivty.DnaProperty, value); }
        }
        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Dna = Transcription.ReverseTranscribe(Rna);
            return ActivityExecutionStatus.Closed;
        }
    }
}
