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
using System.Workflow.Activities;

using Microsoft.Research.ScientificWorkflow;

using MBF;
using MBF.IO;
using MBF.IO.GenBank;
using MBF.IO.Fasta;
using MBF.Algorithms.Translation;

namespace MBF.Workflow
{
    [Name("Sequence Data Breakdown")]
    [Description("Given an input sequence, this activity produces a string version of the data of the sequence as well as access to the metadata of the sequence.")]
    [WorkflowCategory("Bioinformatics")]

    public class SequenceDataStringActivity : Activity
    {
        #region Dependency Properties

        public static DependencyProperty SequenceProperty = 
            DependencyProperty.Register("Sequence", typeof(ISequence), 
            typeof(SequenceDataStringActivity));

        [RequiredInputParam]
        [Name("Sequence")]
        [Description(@"The input sequence from which to extract the data and metadata.")]
        public ISequence Sequence
        {
            get { return ((ISequence)(base.GetValue(SequenceDataStringActivity.SequenceProperty)));}
            set { base.SetValue(SequenceDataStringActivity.SequenceProperty, value);}
        }

        public static DependencyProperty SequenceDataProperty = 
            DependencyProperty.Register("SequenceData", typeof(string), 
            typeof(SequenceDataStringActivity));

        [OutputParam]
        [Name("Sequence Data")]
        [Description(@"An unformatted string representation of the data of the sequence.")]
        public string SequenceData
        {
            get { return ((string)(base.GetValue(SequenceDataStringActivity.SequenceDataProperty))); }
            set { base.SetValue(SequenceDataStringActivity.SequenceDataProperty, value); }
        }

        public static DependencyProperty IDProperty = 
            DependencyProperty.Register("ID", typeof(string), 
            typeof(SequenceDataStringActivity));

        [OutputParam]
        [Name("ID")]
        [Description(@"The ID of the sequence.")]
        public string ID
        {
            get { return ((string)(base.GetValue(SequenceDataStringActivity.IDProperty))); }
            set { base.SetValue(SequenceDataStringActivity.IDProperty, value); }
        }

        public static DependencyProperty DisplayIDProperty = 
            DependencyProperty.Register("DisplayID", typeof(string), 
            typeof(SequenceDataStringActivity));

        [OutputParam]
        [Name("DisplayID")]
        [Description(@"The display ID of the sequence.")]
        public string DisplayID
        {
            get { return ((string)(base.GetValue(SequenceDataStringActivity.DisplayIDProperty))); }
            set { base.SetValue(SequenceDataStringActivity.DisplayIDProperty, value); }
        }

        public static DependencyProperty StatisticsProperty =
            DependencyProperty.Register("Statistics", typeof(SequenceStatistics),
            typeof(SequenceDataStringActivity));

        [OutputParam]
        [Name("Statistics")]
        [Description(@"The count statistics of the sequence data.")]
        public SequenceStatistics Statistics
        {
            get { return ((SequenceStatistics)(base.GetValue(SequenceDataStringActivity.StatisticsProperty))); }
            set { base.SetValue(SequenceDataStringActivity.StatisticsProperty, value); }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            SequenceData = Sequence.ToString();
            ID = Sequence.ID;
            DisplayID = Sequence.DisplayID;
            Statistics = Sequence.Statistics;

            return ActivityExecutionStatus.Closed;
        }
    }
}
