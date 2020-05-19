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
    [Name("Sequence Creator")]
    [Description("Allows the creation of a sequence by providing the sequence data directly.")]
    [WorkflowCategory("Bioinformatics")]
    public class CreateSequenceActivity : Activity
    {
        #region Dependency Properties

        public static DependencyProperty AlphabetNameProperty = 
            DependencyProperty.Register("AlphabetName", typeof(string), 
            typeof(CreateSequenceActivity));

        [RequiredInputParam]
        [Name("Alphabet")]
        [Description(@"The alphabet of the sequence. Must be one of: DNA, RNA, or Protein.")]
        public string AlphabetName
        {
            get { return ((string)(base.GetValue(CreateSequenceActivity.AlphabetNameProperty))); }
            set { base.SetValue(CreateSequenceActivity.AlphabetNameProperty, value); }
        }

        public static DependencyProperty SequenceDataProperty =
            DependencyProperty.Register("SequenceData", typeof(string),
            typeof(CreateSequenceActivity));

        [RequiredInputParam]
        [Name("Sequence Data")]
        [Description(@"The characters making up the sequence (e.g. for DNA, 'GATTCCA').")]
        public string SequenceData
        {
            get { return ((string)(base.GetValue(CreateSequenceActivity.SequenceDataProperty))); }
            set { base.SetValue(CreateSequenceActivity.SequenceDataProperty, value); }
        }

        public static DependencyProperty IDProperty =
            DependencyProperty.Register("ID", typeof(string),
            typeof(CreateSequenceActivity));

        [OptionalInputParam]
        [Name("ID")]
        [Description(@"An internal identification of the sequence.")]
        public string ID
        {
            get { return ((string)(base.GetValue(CreateSequenceActivity.IDProperty))); }
            set { base.SetValue(CreateSequenceActivity.IDProperty, value); }
        }

        public static DependencyProperty DisplayIDProperty =
            DependencyProperty.Register("DisplayID", typeof(string),
            typeof(CreateSequenceActivity));

        [OptionalInputParam]
        [Name("Display ID")]
        [Description(@"A human readable identification of the sequence.")]
        public string DisplayID
        {
            get { return ((string)(base.GetValue(CreateSequenceActivity.DisplayIDProperty))); }
            set { base.SetValue(CreateSequenceActivity.DisplayIDProperty, value); }
        }

        public static DependencyProperty SequenceResultProperty = 
            DependencyProperty.Register("SequenceResult", typeof(Sequence), 
            typeof(CreateSequenceActivity));

        [OutputParam]
        [Name("Sequence")]
        [Description("The first sequence found in the input file.")]
        public Sequence SequenceResult
        {
            get { return ((Sequence)(base.GetValue(CreateSequenceActivity.SequenceResultProperty))); }
            set { base.SetValue(CreateSequenceActivity.SequenceResultProperty, value); }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            IAlphabet alphabet = Alphabets.DNA;
            if (AlphabetName.Equals("DNA", StringComparison.InvariantCultureIgnoreCase))
                alphabet = Alphabets.DNA;
            else if (AlphabetName.Equals("RNA", StringComparison.InvariantCultureIgnoreCase))
                alphabet = Alphabets.RNA;
            else if (AlphabetName.Equals("Protein", StringComparison.InvariantCultureIgnoreCase))
                alphabet = Alphabets.Protein;
            else
                throw new ArgumentException("Unknown alphabet name");

            SequenceResult = new Sequence(alphabet, SequenceData);
            SequenceResult.IsReadOnly = false;
            SequenceResult.ID = ID;
            SequenceResult.DisplayID = DisplayID;
            
            return ActivityExecutionStatus.Closed;
        }
    }
}
