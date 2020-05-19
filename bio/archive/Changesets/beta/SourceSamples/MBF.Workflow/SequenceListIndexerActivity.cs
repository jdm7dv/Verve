// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using System.ComponentModel;
using System.Workflow.ComponentModel;

using Microsoft.Research.ScientificWorkflow;

namespace MBF.Workflow
{
    [Name("Sequence List Indexer")]
    [Description("Fetches and returns sequence at the specified index from a list of sequences.")]
    [WorkflowCategory("Bioinformatics")]

    public class SequenceListIndexerActivity : Activity
    {
        #region Dependency Properties

        public static DependencyProperty SequenceListProperty =
            DependencyProperty.Register("SequenceList", typeof(IList<ISequence>),
            typeof(SequenceListIndexerActivity));

        [RequiredInputParam]
        [Name("Sequence List")]
        [Description("A list of sequences.")]
        public IList<ISequence> SequenceList
        {
            get { return ((IList<ISequence>)(base.GetValue(SequenceListIndexerActivity.SequenceListProperty))); }
            set { base.SetValue(SequenceListIndexerActivity.SequenceListProperty, value); }
        }

        public static DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int),
            typeof(SequenceListIndexerActivity));

        [RequiredInputParam]
        [Name("Index")]
        [Description("Index of the item in the sequence list.")]
        public int Index
        {
            get { return ((int)(base.GetValue(SequenceListIndexerActivity.IndexProperty))); }
            set { base.SetValue(SequenceListIndexerActivity.IndexProperty, value); }
        }

        public static DependencyProperty SequenceProperty =
            DependencyProperty.Register("Sequence", typeof(ISequence),
            typeof(SequenceListIndexerActivity));

        [OutputParam]
        [Name("Sequence")]
        [Description("Sequence at the specified index.")]
        public ISequence Sequence
        {
            get { return ((ISequence)(base.GetValue(SequenceListIndexerActivity.SequenceProperty))); }
            set { base.SetValue(SequenceListIndexerActivity.SequenceProperty, value); }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            // Fetch nth element and store it in local output variable.
            Sequence = SequenceList[Index];
            return ActivityExecutionStatus.Closed;
        }
    }
}
