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
    [Name("Nucleotide Complementation")]
    [Description("Given a DNA or RNA, this activity produces the complement of that sequence.")]
    [WorkflowCategory("Bioinformatics")]

    public class NucleotideComplementationActivity : Activity
    {
        #region Dependency Properties

        public static DependencyProperty DnaOrRnaInputProperty = 
            DependencyProperty.Register("DnaOrRnaInput", typeof(ISequence), 
            typeof(NucleotideComplementationActivity));

        [RequiredInputParam]
        [Name("Input DNA or RNA")]
        [Description(@"The DNA or RNA sequence as an input.")]
        public ISequence DnaOrRnaInput
        {
            get { return ((ISequence)(base.GetValue(NucleotideComplementationActivity.DnaOrRnaInputProperty))); }
            set { base.SetValue(NucleotideComplementationActivity.DnaOrRnaInputProperty, value); }
        }

        public static DependencyProperty DnaOrRnaOutputProperty = 
            DependencyProperty.Register("DnaOrRnaOutput", typeof(ISequence), 
            typeof(NucleotideComplementationActivity));

        [OutputParam]
        [Name("Ouput DNA or RNA")]
        [Description(@"The DNA or RNA sequence as an output.")]
        public ISequence DnaOrRnaOutput
        {
            get { return ((ISequence)(base.GetValue(NucleotideComplementationActivity.DnaOrRnaOutputProperty))); }
            set { base.SetValue(NucleotideComplementationActivity.DnaOrRnaOutputProperty, value); }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            DnaOrRnaOutput = Complementation.Complement(DnaOrRnaInput);
            return ActivityExecutionStatus.Closed;
        }
    }
}
