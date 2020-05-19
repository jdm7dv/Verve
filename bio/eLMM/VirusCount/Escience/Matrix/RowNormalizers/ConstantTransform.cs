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
using Bio.Matrix;
using Bio.Util;
using System.Threading.Tasks;
using MBT.Escience;
using MBT.Escience.Parse;
using MBT.Escience.Matrix;

namespace MBT.Escience.Matrix.RowNormalizers
{
    [Serializable]
    /// <summary>
    /// (For testing) Always uses a Factor of 1 and an offset of 0
    /// </summary>
    public class ConstantTransform : RowNormalizer
    {

        [Parse(ParseAction.Required)]
        LinearTransform LinearTransform;



        public override LinearTransform CreateLinearTransform(IList<double> rowArrayWithNaN, string rowKeyOrNull)
        {
            return LinearTransform;
        }




    }
}
