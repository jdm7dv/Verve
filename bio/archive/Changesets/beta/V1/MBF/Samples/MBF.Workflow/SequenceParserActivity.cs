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
using MBF.IO.GenBank;
using MBF.IO.Fasta;

namespace MBF.Workflow
{
    [Name("Sequence Parser")]
    [Description("Parses any sequence file of a known format and file extension and returns all sequences found in that file.")]
    [WorkflowCategory("Bioinformatics")]
    public class SequenceParserActivity : Activity
    {
        #region Dependency Properties

        public static DependencyProperty InputFileProperty = 
            DependencyProperty.Register("InputFile", typeof(string), 
            typeof(SequenceParserActivity));

        [RequiredInputParam]
        [Name("Input File")]
        [Description(@"The path of the input sequence file.")]
        public string InputFile
        {
            get { return ((string)(base.GetValue(SequenceParserActivity.InputFileProperty))); }
            set { base.SetValue(SequenceParserActivity.InputFileProperty, value); }
        }

        public static DependencyProperty ListSequenceResultProperty = 
            DependencyProperty.Register("ListSequenceResult", typeof(IList<ISequence>), 
            typeof(SequenceParserActivity));

        [OutputParam]
        [Name("Sequence List")]
        [Description("List of sequence found in the input file.")]
        public IList<ISequence> ListSequenceResult
        {
            get { return ((IList<ISequence>)(base.GetValue(SequenceParserActivity.ListSequenceResultProperty))); }
            set { base.SetValue(SequenceParserActivity.ListSequenceResultProperty, value); }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            String inputFileName = InputFile;
            ISequenceParser parser = SequenceParsers.FindParserByFile(inputFileName);
            ListSequenceResult = parser.Parse(inputFileName);
  
            return ActivityExecutionStatus.Closed;
        }
    }
}
