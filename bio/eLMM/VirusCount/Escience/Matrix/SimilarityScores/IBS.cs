//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoNS.Array;
using Bio.Util;
using MBT.Escience.Parse;
using MBT.Escience.Matrix.RowNormalizers;


//Should only meanImpuate, doesn't make sense to do Eigenstrat, or MeanCenter, etc,

namespace MBT.Escience.Matrix.SimilarityScores
{
    public class IBS : SimilarityScore, IParsable
    {
        protected override double  JustKernel(IList<double> p1, IList<double> p2)
        {
            double score = 0;
            int S = p1.Count;
            Helper.CheckCondition(S == p2.Count, "vectors are not of the same length");
            for (int s = 0; s < S; s++)
            {
                score = score + 2 - Math.Abs(p1[s] - p2[s]);
            }
            score = score / (2 * S);
            Helper.CheckCondition(!double.IsNaN(score) && 0 <= score && score <= 1, "IBS score should between 0 and 1");

            return score;
        }

        #region IParsable Members

        public void FinalizeParse()
        {
            if (null == RowNormalizer)
            {
                RowNormalizer = new ImputeWithRowMean();
            }
        }

        #endregion
    }
}
