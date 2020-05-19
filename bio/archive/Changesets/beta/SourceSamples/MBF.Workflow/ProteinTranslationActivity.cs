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
    [Name("Protein Translation")]
    [Description("Translates a RNA sequence into a corresponding amino acid sequence for encoding proteins.")]
    [WorkflowCategory("Bioinformatics")]
    public class ProteinTranslationActivity : Activity
    {
        #region Dependency Properties
        public static DependencyProperty RnaSequenceProperty =
            DependencyProperty.Register("RnaSequence", typeof(ISequence),
            typeof(ProteinTranslationActivity));

        [RequiredInputParam]
        [Name("RNA Sequence")]
        [Description(@"The RNA sequence to be translated.")]
        public ISequence RnaSequence
        {
            get { return ((ISequence)(base.GetValue(ProteinTranslationActivity.RnaSequenceProperty))); }
            set { base.SetValue(ProteinTranslationActivity.RnaSequenceProperty, value); }
        }

        public static DependencyProperty initialSequenceOffsetProperty =
            DependencyProperty.Register("initialSequenceOffset", typeof(int),
            typeof(ProteinTranslationActivity), new PropertyMetadata((int)0));

        [OptionalInputParam]
        [Name("Sequence Offset")]
        [Description(@"Translation will start by skipping the number of base pairs specifed by this parameter in the input sequence.")]
        public int initialSequenceOffset
        {
            get { return ((int)(base.GetValue(ProteinTranslationActivity.initialSequenceOffsetProperty))); }
            set { base.SetValue(ProteinTranslationActivity.initialSequenceOffsetProperty, value); }
        }

        public static DependencyProperty ProteinProperty =
            DependencyProperty.Register("Protein", typeof(ISequence),
            typeof(ProteinTranslationActivity));

        [OutputParam]
        [Name("Protein")]
        [Description(@"The tranlsated protein sequence.")]
        public ISequence Protein
        {
            get { return ((ISequence)(base.GetValue(ProteinTranslationActivity.ProteinProperty))); }
            set { base.SetValue(ProteinTranslationActivity.ProteinProperty, value); }
        }
        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            Protein = ProteinTranslation.Translate(RnaSequence, initialSequenceOffset);
            return ActivityExecutionStatus.Closed;
        }
    }
}
