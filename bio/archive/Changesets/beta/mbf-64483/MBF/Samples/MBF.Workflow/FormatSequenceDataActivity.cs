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
using System.Text;
using System.Workflow.ComponentModel;

using Microsoft.Research.ScientificWorkflow;

using MBF;

namespace MBF.Workflow
{

    [Name("Format Sequence Data")]
    [Description("Formats a sequence into a string that is human readable. The string contains the display ID of the sequence, count statistics, and formatted data symbols.")]
    [WorkflowCategory("Bioinformatics")]
    public class FormatSequenceDataActivity : Activity
    {
        #region Dependency Properties

        public static DependencyProperty SequenceProperty =
            DependencyProperty.Register("Sequence", typeof(ISequence),
            typeof(FormatSequenceDataActivity));

        [RequiredInputParam]
        [Name("Sequence")]
        [Description("The first sequence found in the input file.")]
        public ISequence Sequence
        {
            get { return ((ISequence)(base.GetValue(FormatSequenceDataActivity.SequenceProperty))); }
            set { base.SetValue(FormatSequenceDataActivity.SequenceProperty, value); }
        }

        public static DependencyProperty DataProperty = 
            DependencyProperty.Register("Data", typeof(string),
            typeof(FormatSequenceDataActivity));

        [OutputParam]
        [Name("Formatted Data")]
        [Description(@"The formatted represenation of the sequence.")]
        public string Data
        {
            get { return ((string)(base.GetValue(FormatSequenceDataActivity.DataProperty))); }
            set { base.SetValue(FormatSequenceDataActivity.DataProperty, value); }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            StringBuilder buff = new StringBuilder();

            buff.AppendLine(Sequence.DisplayID);
            buff.Append("Statistics: ");
            buff.Append(Sequence.Count);
            buff.Append(" Total");

            if (Sequence.Alphabet == Alphabets.DNA)
            {
                buff.Append(" - G: ");
                buff.Append(Sequence.Statistics.GetCount(Alphabets.DNA.G));
                buff.Append(" - A: ");
                buff.Append(Sequence.Statistics.GetCount(Alphabets.DNA.A));
                buff.Append(" - T: ");
                buff.Append(Sequence.Statistics.GetCount(Alphabets.DNA.T));
                buff.Append(" - C: ");
                buff.Append(Sequence.Statistics.GetCount(Alphabets.DNA.C));
            }
            else if (Sequence.Alphabet == Alphabets.RNA)
            {
                buff.Append(" - G: ");
                buff.Append(Sequence.Statistics.GetCount(Alphabets.RNA.G));
                buff.Append(" - A: ");
                buff.Append(Sequence.Statistics.GetCount(Alphabets.RNA.A));
                buff.Append(" - U: ");
                buff.Append(Sequence.Statistics.GetCount(Alphabets.RNA.U));
                buff.Append(" - C: ");
                buff.Append(Sequence.Statistics.GetCount(Alphabets.RNA.C));
            }

            buff.AppendLine();
            buff.AppendLine();

            for (int i = 0; i < Sequence.Count; i++)
            {
                if ((i % 50) == 0)
                {
                    string num = (i + 1).ToString();
                    int pad = 5 - num.Length;
                    StringBuilder buff2 = new StringBuilder();
                    for (int j = 0; j < pad; j++)
                        buff2.Append(' ');
                    buff2.Append(num);
                    buff.Append(buff2.ToString());
                }

                if ((i % 10) == 0)
                    buff.Append(' ');

                buff.Append(Sequence[i].Symbol);

                if ((i % 50) == 49)
                    buff.AppendLine();
            }
            buff.AppendLine();

            Data = buff.ToString();
                        
            return ActivityExecutionStatus.Closed;
        }
    }
}
