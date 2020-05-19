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
using System.IO;

namespace MBT.Escience.Matrix.RowNormalizers
{
    public class FromBinaryFile : RowNormalizer, IParsable
    {
        [Parse(ParseAction.Required,typeof(InputFile))]
        FileInfo BinaryInputFile;

        [Parse(ParseAction.Ignore)]
        Lazy<Matrix<string, string, LinearTransform>> LinearTransformationMatrixFromFile;


        public override Matrix<string, string, LinearTransform> LinearTransformMatrix(Matrix<string, string, double> inputMatrix)
        {
            Helper.CheckCondition(inputMatrix.RowKeys.SequenceEqual(LinearTransformationMatrixFromFile.Value.RowKeys), "Expect the rowsKeys of the inputMatrix to be the same (and in the same order) as the rowKeys of the linearTransformMatrix from the file");
            return LinearTransformationMatrixFromFile.Value;
        }

        public override LinearTransform CreateLinearTransform(IList<double> unnormalizedList, string rowKeyOrNull)
        {
            return LinearTransformationMatrixFromFile.Value[rowKeyOrNull, SpecialFunctions.FirstAndOnly(LinearTransformationMatrixFromFile.Value.ColKeys)];
        }

        #region IParsable Members

        public void FinalizeParse()
        {
            LinearTransformationMatrixFromFile = new Lazy<Matrix<string, string, LinearTransform>>(() =>
            {
                Console.WriteLine("loading rownormalization from binary file");
                var output = (Matrix<string, string, LinearTransform>) SpecialFunctions.Deserialize(BinaryInputFile.FullName);
                Console.WriteLine("done");
                return output;
            });
        }

        #endregion
    }
}
