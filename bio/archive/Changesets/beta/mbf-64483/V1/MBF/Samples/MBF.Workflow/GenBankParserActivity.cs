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

namespace MBF.Workflow
{
    [Name("GenBank Parser")]
    [Description("This activity should take in an input file and produce as a result a set of sequence objects. It should be left generic as to the file format, choosing the appropriate format based on the file extension.")]
    [WorkflowCategory("Bioinformatics")]

    public class GenBankParserActivity : Activity
    {
        #region Dependency Properties
        public static DependencyProperty InputFileProperty =
            DependencyProperty.Register("InputFile", typeof(string),
            typeof(GenBankParserActivity));

        [RequiredInputParam]
        [Name("Input File")]
        [Description(@"The path to the GenBank formatted file to read in.")]
        public string InputFile
        {
            get
            {
                return ((string)(base.GetValue(GenBankParserActivity.InputFileProperty)));
            }
            set
            {
                base.SetValue(GenBankParserActivity.InputFileProperty, value);
            }
        }

        public static DependencyProperty SequenceListProperty =
            DependencyProperty.Register("SequenceList", typeof(IList<ISequence>),
            typeof(GenBankParserActivity));

        [OutputParam]
        [Name("Sequence List")]
        [Description("A list of sequences found within the input file.")]
        public IList<ISequence> SequenceList
        {
            get
            {
                return ((IList<ISequence>)(base.GetValue(GenBankParserActivity.SequenceListProperty)));
            }
            set
            {
                base.SetValue(GenBankParserActivity.SequenceListProperty, value);
            }
        }
        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            String inputFileName = InputFile;
            GenBankParser parser = new GenBankParser();
            SequenceList = parser.Parse(inputFileName);
            return ActivityExecutionStatus.Closed;
        }
    }
}
