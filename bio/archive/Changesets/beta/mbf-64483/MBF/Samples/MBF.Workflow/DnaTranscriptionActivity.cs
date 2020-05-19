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
    [Name("DNA Transcription")]
    [Description("Given a DNA sequence, this activity produces the transcripted RNA sequence.")]
    [WorkflowCategory("Bioinformatics")]
    public class DnaTranscriptionActivity : Activity
    {
        #region Dependency Properties

        public static DependencyProperty DnaInputProperty = 
            DependencyProperty.Register("DnaInput", typeof(ISequence), 
            typeof(DnaTranscriptionActivity));

        [RequiredInputParam]
        [Name("DNA Sequence")]
        [Description(@"The DNA sequence to transcribe.")]
        public ISequence DnaInput
        {
            get { return ((ISequence)(base.GetValue(DnaTranscriptionActivity.DnaInputProperty))); }
            set { base.SetValue(DnaTranscriptionActivity.DnaInputProperty, value); }
        }

        public static DependencyProperty RnaOutputProperty = 
            DependencyProperty.Register("RnaOutput", typeof(ISequence), 
            typeof(DnaTranscriptionActivity));

        [OutputParam]
        [Name("RNA Sequence")]
        [Description(@"The RNA sequence as an output.")]
        public ISequence RnaOutput
        {
            get { return ((ISequence)(base.GetValue(DnaTranscriptionActivity.RnaOutputProperty))); }
            set { base.SetValue(DnaTranscriptionActivity.RnaOutputProperty, value); }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            RnaOutput = Transcription.Transcribe(DnaInput);
            return ActivityExecutionStatus.Closed;
        }
	}
}
