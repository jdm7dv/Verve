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
using MBF.IO.Fasta;
using MBF.IO.GenBank;

namespace MBF.Workflow
{

    [Name("Single Sequence Parser")]
    [Description("Parses any sequence file of a known format and file extension and returns the first sequence found in that file.")]
    [WorkflowCategory("Bioinformatics")]
    public class SingleSequenceParserActivity : Activity
    {
        #region Dependency Properties

        public static DependencyProperty InputFileProperty = 
            DependencyProperty.Register("InputFile", typeof(string),
            typeof(SingleSequenceParserActivity));

        [RequiredInputParam]
        [Name("Input File")]
        [Description(@"The path of the input sequence file.")]
        public string InputFile
        {
            get { return ((string)(base.GetValue(SingleSequenceParserActivity.InputFileProperty))); }
            set { base.SetValue(SingleSequenceParserActivity.InputFileProperty, value); }
        }

        public static DependencyProperty SequenceResultProperty = 
            DependencyProperty.Register("SequenceResult", typeof(ISequence),
            typeof(SingleSequenceParserActivity));

        [OutputParam]
        [Name("Sequence")]
        [Description("The first sequence found in the input file.")]
        public ISequence SequenceResult
        {
            get { return ((ISequence)(base.GetValue(SingleSequenceParserActivity.SequenceResultProperty))); }
            set { base.SetValue(SingleSequenceParserActivity.SequenceResultProperty, value); }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            String inputFileName = InputFile;
            ISequenceParser parser = SequenceParsers.FindParserByFile(inputFileName);
            SequenceResult = parser.ParseOne(inputFileName);
            
            return ActivityExecutionStatus.Closed;
        }

       
    }
   
}
