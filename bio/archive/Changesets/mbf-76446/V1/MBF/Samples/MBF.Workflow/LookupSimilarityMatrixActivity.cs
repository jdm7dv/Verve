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
using MBF.Algorithms;
using MBF.Algorithms.Alignment;
using MBF.SimilarityMatrices;

namespace MBF.Workflow
{

    [Name("Lookup Similarity Matrix")]
    [Description("Looks for a known similarity matrix.")]
    [WorkflowCategory("Bioinformatics")]
    public class LookupSimilarityMatrixActivity : Activity
    {
        #region Dependency Properties

        public static DependencyProperty MatrixNameProperty = 
            DependencyProperty.Register("MatrixName", typeof(string),
            typeof(LookupSimilarityMatrixActivity),
            new PropertyMetadata("Blosum50"));

        [RequiredInputParam]
        [Name("Matrix Name")]
        [Description(@"The name of a known similarity matrix (e.g. 'Blosum50').")]
        public string MatrixName
        {
            get { return ((string)(base.GetValue(LookupSimilarityMatrixActivity.MatrixNameProperty))); }
            set { base.SetValue(LookupSimilarityMatrixActivity.MatrixNameProperty, value); }
        }

        public static DependencyProperty MatrixProperty = 
            DependencyProperty.Register("Matrix", typeof(SimilarityMatrix),
            typeof(LookupSimilarityMatrixActivity));

        [OutputParam]
        [Name("Matrix")]
        [Description("The similarity matrix result.")]
        public SimilarityMatrix Matrix
        {
            get { return ((SimilarityMatrix)(base.GetValue(LookupSimilarityMatrixActivity.MatrixProperty))); }
            set { base.SetValue(LookupSimilarityMatrixActivity.MatrixProperty, value); }
        }

        #endregion

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            if (MatrixName.Equals("Blosum45", StringComparison.InvariantCultureIgnoreCase))
                Matrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum45);
            else if (MatrixName.Equals("Blosum50", StringComparison.InvariantCultureIgnoreCase))
                Matrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum50);
            else if (MatrixName.Equals("Blosum62", StringComparison.InvariantCultureIgnoreCase))
                Matrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum62);
            else if (MatrixName.Equals("Blosum80", StringComparison.InvariantCultureIgnoreCase))
                Matrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum80);
            else if (MatrixName.Equals("Blosum90", StringComparison.InvariantCultureIgnoreCase))
                Matrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Blosum90);
            else if (MatrixName.Equals("Pam250", StringComparison.InvariantCultureIgnoreCase))
                Matrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Pam250);
            else if (MatrixName.Equals("Pam30", StringComparison.InvariantCultureIgnoreCase))
                Matrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Pam30);
            else if (MatrixName.Equals("Pam70", StringComparison.InvariantCultureIgnoreCase))
                Matrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.Pam70);
            else if (MatrixName.Equals("AmbiguousDna", StringComparison.InvariantCultureIgnoreCase))
                Matrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);
            else if (MatrixName.Equals("AmbiguousRna", StringComparison.InvariantCultureIgnoreCase))
                Matrix = new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousRna);
            
            return ActivityExecutionStatus.Closed;
        }
    }
}
