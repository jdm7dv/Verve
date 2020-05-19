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
using MBF.IO.Fasta;
using MBF.IO.GenBank;

namespace MBF.Workflow
{
    [Name("FASTA File Formatter")]
    [Description("Writes a sequence and/or list of sequences out to a specified file using the FASTA file format.")]
    [WorkflowCategory("Bioinformatics")]

    public class FastaFormatterActivity : Activity
    {
        #region Dependency Properties
        public static DependencyProperty OutputFileProperty =
            DependencyProperty.Register("OutputFile", typeof(string),
            typeof(FastaFormatterActivity));
        
        [RequiredInputParam]
        [Name("Ouput file")]
        [Description(@"A file path specifying the location of the output file.")]
        public string OutputFile
        {
            get { return ((string)(base.GetValue(FastaFormatterActivity.OutputFileProperty))); }
            set { base.SetValue(FastaFormatterActivity.OutputFileProperty, value); }
        }

        public static DependencyProperty SequenceProperty =
            DependencyProperty.Register("Sequence", typeof(ISequence),
            typeof(FastaFormatterActivity));
        
        [InputParam]
        [Name("Sequence")]
        [Description(@"An individual sequence to write.")]
        public ISequence Sequence
        {
            get { return ((ISequence)(base.GetValue(FastaFormatterActivity.SequenceProperty))); }
            set { base.SetValue(FastaFormatterActivity.SequenceProperty, value); }
        }

        public static DependencyProperty SequenceListProperty =
            DependencyProperty.Register("SequenceList", typeof(IList<ISequence>),
            typeof(FastaFormatterActivity));

        [InputParam]
        [Name("Sequence List")]
        [Description("A list of sequences to write")]
        public IList<ISequence> SequenceList
        {
            get { return ((IList<ISequence>)(base.GetValue(FastaFormatterActivity.SequenceListProperty))); }
            set { base.SetValue(FastaFormatterActivity.SequenceListProperty, value); }
        }
        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            if ((Sequence == null) && (SequenceList != null))
            {
                FastaFormatter formatter = new FastaFormatter();
                formatter.Format(SequenceList, OutputFile);
            }
            else if ((Sequence != null) && (SequenceList == null))
            {
                FastaFormatter formatter = new FastaFormatter();
                formatter.Format(Sequence, OutputFile);
            }
            else if ((Sequence != null) && (SequenceList != null))
            {
                SequenceList.Add(Sequence);
                FastaFormatter formatter = new FastaFormatter();
                formatter.Format(SequenceList, OutputFile);
                SequenceList.Remove(Sequence);
            }
            return ActivityExecutionStatus.Closed;
        }
	}
}