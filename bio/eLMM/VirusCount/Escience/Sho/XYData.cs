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

using ShoNS.Array;

namespace MBT.Escience.Sho
{
    /// <summary>
    /// Simple class to hold X-Y data to plot
    /// </summary>
    public class XYData
    {
        public DoubleArray _xDat;
        public DoubleArray _yDat;

        public XYData(DoubleArray xDat, DoubleArray yDat)
        {
            _xDat = xDat;
            _yDat = yDat;
        }
    }
}
