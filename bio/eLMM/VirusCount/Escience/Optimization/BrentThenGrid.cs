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

namespace MBT.Escience.Optimization
{
    public class BrentThenGrid : OneDOptimization
    {
        Brent Brent;
        OneDGrid Grid;

        public override double Tolerance
        {
            get { return base.Tolerance; }
            set
            {
                base.Tolerance = value;
                Brent.Tolerance = value;
                Grid.Tolerance = value;
            }
        }

        public int GridLineCount
        {
            get { return Grid.GridLineCount; }
            set { Grid.GridLineCount = value; }
        }

        public BrentThenGrid()
        {
            Brent = new Brent();
            Grid = new OneDGrid();
        }
        override public double Run(Converter<OptimizationParameter, double> oneDRealFunction,
            /*ref*/ OptimizationParameter param)
        {
            try
            {
                return Brent.Run(oneDRealFunction, param);
                //return Brent.Run(oneDRealFunction, param, gridLineCount, out bestInput);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error calling Brent");
                Console.WriteLine(exception.Message);
                if (exception.InnerException != null)
                {
                    Console.WriteLine(exception.InnerException.Message);
                }
            }

            return Grid.Run(oneDRealFunction, param);
        }
    }
}
