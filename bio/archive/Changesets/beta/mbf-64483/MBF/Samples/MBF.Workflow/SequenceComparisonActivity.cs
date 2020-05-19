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

namespace MBF.Workflow
{
    [Name("Sequence Comparison")]
    [Description("Performs a quick comparison to see if two formatted sequences are the same or not")]
    [WorkflowCategory("Bioinformatics")]
 
    public class SequenceComparisonActivity : Activity
    {
        #region Dependency Properties

        public static DependencyProperty Sequence1Property = 
            DependencyProperty.Register("Sequence1", typeof(ISequence), 
            typeof(SequenceComparisonActivity));

        [RequiredInputParam]
        [Name("First Sequence")]
        [Description(@"The first sequence to compare.")]
        public ISequence Sequence1
        {
            get { return ((ISequence)(base.GetValue(SequenceComparisonActivity.Sequence1Property))); }
            set { base.SetValue(SequenceComparisonActivity.Sequence1Property, value); }
        }

        public static DependencyProperty Sequence2Property = 
            DependencyProperty.Register("Sequence2", typeof(ISequence), 
            typeof(SequenceComparisonActivity));

        [RequiredInputParam]
        [Name("Second Sequence")]
        [Description(@"The second sequence to compare.")]
        public ISequence Sequence2
        {
            get { return ((ISequence)(base.GetValue(SequenceComparisonActivity.Sequence2Property))); }
            set { base.SetValue(SequenceComparisonActivity.Sequence2Property, value); }
        }

        public static DependencyProperty AreEqualProperty = 
            DependencyProperty.Register("AreEqual", typeof(bool),
            typeof(SequenceComparisonActivity));

        [OutputParam]
        [Name("Are Equal")]
        [Description(@"An indication as to whether or not the sequences are the same.")]
        public bool AreEqual
        {
            get { return ((bool)(base.GetValue(SequenceComparisonActivity.AreEqualProperty))); }
            set { base.SetValue(SequenceComparisonActivity.AreEqualProperty, value); }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            AreEqual = true;
            if (Sequence1.Alphabet != Sequence2.Alphabet)
            {
                AreEqual = false;
                return ActivityExecutionStatus.Closed;
            }

            if (Sequence1.Count != Sequence2.Count)
            {
                AreEqual = false;
                return ActivityExecutionStatus.Closed;
            }

            for (int i = 0; i < Sequence1.Count; i++ )
            {
                if (Sequence1[i] != Sequence2[i])
                {
                    AreEqual = false;
                    return ActivityExecutionStatus.Closed;
                }
            }

            AreEqual = true;
            return ActivityExecutionStatus.Closed;
        }
    }
}
