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
using MBT.Escience.Matrix;
using System.IO;
using Bio.Util;
using Bio.Matrix;

namespace MBT.Escience.Parse
{
    public class LeaveOnDiskIfPossibleString : FileMatrix<string>, IParsable
    {

        #region IParsable Members

        void IParsable.FinalizeParse()
        {
            MergeType = MergeType.Both;
            LeaveOnDiskIfPossible = true;
            CharToTValConverter = Bio.Util.ValueConverter.CharToString;
            MissingValueForParse = "?";
            base.FinalizeParse();
        }

        #endregion
    }

    public class LeaveOnDiskIfPossible : FileMatrix<double>, IParsable
    {

        #region IParsable Members

        void IParsable.FinalizeParse()
        {
            MergeType = MergeType.Both;
            LeaveOnDiskIfPossible = true;
            CharToTValConverter = Bio.Util.ValueConverter.CharToDouble;
            MissingValueForParse = double.NaN;
            base.FinalizeParse();
        }

        #endregion
    }
}
