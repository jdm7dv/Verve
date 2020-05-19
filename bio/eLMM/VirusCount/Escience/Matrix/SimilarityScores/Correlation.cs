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

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Bio.Util;
//using ShoNS.Array;
//using System.Threading.Tasks;
//using MBT.Escience.Sho;
//using MBT.Escience.Parse;
//using MBT.Escience.Matrix.RowNormalizers;

//namespace MBT.Escience.Matrix.SimilarityScores
//{
//    public class Correlation : SimilarityScore, IParsable
//    {
//        public override DoubleArray ComputeKernel(ref DoubleArray snpArray, bool checkForNaN)
//        {
//            DoubleArray K = Covariance.StaticComputeKernel(ref snpArray, checkForNaN: false);
//             K = ShoUtils.CorrelationMatrixFromCovarianceMatrix(K);
//            Helper.CheckCondition(!checkForNaN || !K.HasNaN(), "should not contain NaNs in data");
//            return K;
//        }

//        public override double ScoreNoNaN(IList<double> case1, IList<double> case2, bool checkForNaN)
//        {
//            throw new NotImplementedException();
//        }

//        public void FinalizeParse()
//        {
//        }

//    }

//}
