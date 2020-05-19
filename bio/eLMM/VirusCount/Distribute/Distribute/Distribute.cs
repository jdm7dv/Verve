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
using MBT.Escience.Parse;
using Bio.Util;

namespace MBT.Escience.Distribute
{
    public class Distribute : IRunnable
    {
        /// <summary>
        /// The object you wish to distribute.
        /// </summary>
        [Parse(ParseAction.Required)]
        public IDistributable Distributable;

        /// <summary>
        /// How to distribute the object.
        /// </summary>
        [Parse(ParseAction.Required)]
        public IDistributor Distributor;

        public void Run()
        {
            Distributor.Distribute(Distributable);
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine(args.StringJoin(" "));
                CommandArguments.ConstructAndRun<Distribute>(args);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                Environment.ExitCode = -532462766; // general failure.
            }
        }
    }
}
