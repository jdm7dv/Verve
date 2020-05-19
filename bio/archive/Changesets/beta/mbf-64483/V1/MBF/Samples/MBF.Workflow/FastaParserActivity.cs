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

namespace MBF.Workflow
{
    [Name("FASTA Parser")]
    [Description("Parses a specified input file according to the FASTA file format.")]
    [WorkflowCategory("Bioinformatics")]

    public class FastaParserActivity : Activity
    {
        #region Dependency Properties
        public static DependencyProperty InputFileProperty = 
            DependencyProperty.Register("InputFile", typeof(string), 
            typeof(FastaParserActivity));

        [RequiredInputParam]
        [Name("Input File")]
        [Description(@"The path to the FASTA formatted file to read in.")]
        public string InputFile
        {
            get { return ((string)(base.GetValue(FastaParserActivity.InputFileProperty))); }
            set { base.SetValue(FastaParserActivity.InputFileProperty, value); }
        }

        public static DependencyProperty SequenceListProperty = 
            DependencyProperty.Register("SequenceList", typeof(IList<ISequence>), 
            typeof(FastaParserActivity));

        [OutputParam]
        [Name("Sequence List")]
        [Description("A list of sequences found within the input file.")]
        public IList<ISequence> SequenceList
        {
            get { return ((IList<ISequence>)(base.GetValue(FastaParserActivity.SequenceListProperty))); }
            set { base.SetValue(FastaParserActivity.SequenceListProperty, value); }
        }
        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            String inputFileName = InputFile;
            FastaParser parser = new FastaParser();
            SequenceList = parser.Parse(inputFileName);
            return ActivityExecutionStatus.Closed;
        }
    }
}
