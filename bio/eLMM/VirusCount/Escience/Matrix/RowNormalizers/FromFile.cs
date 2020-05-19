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
using MBT.Escience.Sho;
using Bio.Matrix;
using Bio.Util;
using System.Threading.Tasks;
using MBT.Escience.Parse;
using MBT.Escience;

namespace MBT.Escience.Matrix.RowNormalizers
{
    public class FromFile : RowNormalizer
    {
        [Parse(ParseAction.Required, typeof(FileMatrix<LinearTransform>), DefaultParameters = "MissingValueForParse:null")]
        Matrix<string, string, LinearTransform> LinearTransformationMatrixFromFile;


        public override Matrix<string, string, LinearTransform> LinearTransformMatrix(Matrix<string, string, double> inputMatrix)
        {
            Helper.CheckCondition(inputMatrix.RowKeys.SequenceEqual(LinearTransformationMatrixFromFile.RowKeys), "Expect the rowsKeys of the inputMatrix to be the same (and in the same order) as the rowKeys of the linearTransformMatrix from the file");
            return LinearTransformationMatrixFromFile;
        }

        public override LinearTransform CreateLinearTransform(IList<double> unnormalizedList, string rowKeyOrNull)
        {
            return LinearTransformationMatrixFromFile[rowKeyOrNull, SpecialFunctions.FirstAndOnly(LinearTransformationMatrixFromFile.ColKeys)];
        }
    }
}
