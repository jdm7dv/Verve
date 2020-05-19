// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
using System;
using System.IO;
using CommandLine;
using Tools.VennDiagram;

namespace VennTool
{
    class VennToolArguments
    {
        [Argument(ArgumentType.AtMostOnce, HelpText = "Display Verbose output during processing")]
        public bool verbose;
        [Argument(ArgumentType.AtMostOnce, HelpText = "PreSort .BED files prior to processing")]
        public bool preSort;
        [Argument(ArgumentType.AtMostOnce, HelpText = "Write result using polar coordinates")]
        public bool polar;
        [Argument(ArgumentType.AtMostOnce, HelpText = "XL OutputFile")]
        public string xl;
        [DefaultArgument(ArgumentType.Multiple, HelpText = "Values 3 or 7 values for regions in chart, [A B AB] or [A B C AB AC BC ABC]")]
        public double[] regionArray;

        public VennToolArguments()
        {
            verbose = false;
            preSort = false;
            polar = false;
            regionArray = new double[0];
            xl = "";
        }
    }


    class VennTool
    {
        static VennToolArguments parsedArgs;

        static void Splash()
        {
            // Display the program's splash screene
            Console.WriteLine("VennTool V0.00 - Copyright(c) 2009 Microsoft");
        }

        public static VennToolArguments ProcessCommandLineArguments(string[] args)
        {
            VennToolArguments parsedArgs = new VennToolArguments();
            if (!Parser.ParseArgumentsWithUsage(args, parsedArgs))
            {
                Console.Error.WriteLine("\nProcessCommandLineArguments Failed to parse properly.");
                Environment.Exit(-1);
            }

            /*
             * Do any and all follow-up command line argument validation required
             */

            if ((parsedArgs.regionArray == null)
                || ((parsedArgs.regionArray.Length != 3) && (parsedArgs.regionArray.Length != 7)))
            {
                Console.Error.WriteLine("\nProcessCommandLineArguments failed to find the expected number of arguments. [3 or 7]");
                Environment.Exit(-1);
            }

            if (parsedArgs.verbose)
            {
                Console.WriteLine(parsedArgs.verbose);
                Console.Write("RegionArray Size: {0}\n   [", parsedArgs.regionArray.Length);
                for (int i = 0; i < parsedArgs.regionArray.Length; ++i)
                {
                    if (i == 0)
                        Console.Write(parsedArgs.regionArray[i]);
                    else
                        Console.Write(", {0}", parsedArgs.regionArray[i]);
                }
                Console.WriteLine("]");
            }

            return parsedArgs;
        }

        public static void Main(string[] args)
        {
            Splash();
            parsedArgs = ProcessCommandLineArguments(args);
            VennDiagramData vdd = new VennDiagramData(parsedArgs.regionArray);

            if (parsedArgs.polar)
            {
                vdd.WritePolarVennDiagramData();
            }
            else
            {
                vdd.WriteVennDiagramData();
            }
            if (parsedArgs.xl.Length > 0)
            {
                // produce an XL file with the 'right stuff'
                // Make sure we pass a complete filename path too.
                string filename = Path.GetFullPath(parsedArgs.xl);
                Console.WriteLine("Produce Excel VennDiagram file: {0}", filename);
                VennToNodeXL.CreateVennDiagramNodeXLFile(filename, vdd);

            }
        }
    }
}
