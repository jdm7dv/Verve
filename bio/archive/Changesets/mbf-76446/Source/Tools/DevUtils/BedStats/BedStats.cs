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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bio;
using Bio.IO.Bed;
using CommandLine;
using Tools.VennDiagram;

namespace BedStats
{
    class BedStatsArguments
    {
        [Argument(ArgumentType.AtMostOnce, HelpText = "Display Verbose logging during processing")]
        public bool verbose;
        [Argument(ArgumentType.AtMostOnce, HelpText = "normalizeInput .BED files prior to processing")]
        public bool normalizeInputs;
        [Argument(ArgumentType.AtMostOnce, HelpText = "Output file for use with VennTool")]
        public string outputVennTool;
        [Argument(ArgumentType.AtMostOnce, HelpText = "Create an Excel file with BED stats")]
        public string xlFilename;
        [DefaultArgument(ArgumentType.MultipleUnique, HelpText = "List of 2 or 3 input .BED files to process")]
        public string[] inputFiles;

        public BedStatsArguments()
        {
            verbose = false;
            normalizeInputs = false;
            outputVennTool = null;
            xlFilename = null;
            inputFiles = null;
        }
    }


#if false
    // here
    enum FileSource
    {
        SetA = 1,
        SetB = 2,
        SetC = 4
    }
#endif
#if false
    enum OverlapState
    {   // we use flag values in a two bit field
        //  below = 1 mid = 2, above = 3
        //
        overlapEqual = 0x0000,
        overlapStartBelowEndBelow = 0x0101,
        overlapStartBelowEndMiddle = 0x0110,
        overlapStartBelowEndAbove = 0x0111,
        overlapStartMiddleEndMiddle = 0x1010,
        overlapStartMiddleEndAbove = 0x1011,
        overlapStartAboveEndAbove = 0x1111
    }
#endif
#if false
    class BedRangeAndStats
    {
        string  ID;
        long Start;
        long End;
    }
    // and here
#endif

    class BedStats
    {
        static BedStatsArguments parsedArgs;
        static bool fVerbose;
        static bool fCreateExcelWorkbook;
        static bool fCreateVennToolInputFile;
        static string ExcelWorkbookFullPath;
        static string VennToolInputFileFullPath;

        static void Splash()
        {
            // Display the program's splash screene
            Console.WriteLine("\nBedStats V0.00 - Copyright(c) 2009, 2010 Microsoft Corporation.  All Rights Reserved.\n");
        }

        public static BedStatsArguments ProcessCommandLineArguments( string[] args )
        {
            BedStatsArguments parsedArgs = new BedStatsArguments();
            try
            {
                Parser.ParseArgumentsWithUsage(args, parsedArgs);
            }
            catch( Exception e )
            {
                Console.Error.WriteLine(e.ToString());
                Environment.Exit(-1);
            }

            /*
             * Do any and all follow-up command line argument validation required
             */
            if ( (parsedArgs.inputFiles == null)
                || (parsedArgs.inputFiles.Length < 2) 
                || (parsedArgs.inputFiles.Length > 3)
                )
            {
                Console.Error.WriteLine("\nProcessCommandLineArguments Failed to find expected number of file arguments. [2 or 3 required]");
                Environment.Exit(-1);
            }
            
            fCreateExcelWorkbook = ((parsedArgs.xlFilename != null) && (parsedArgs.xlFilename.Count() != 0));
            if ( fCreateExcelWorkbook )
            {
                ExcelWorkbookFullPath = Path.GetFullPath( parsedArgs.xlFilename );
            }

            fCreateVennToolInputFile = ((parsedArgs.outputVennTool != null) && (parsedArgs.outputVennTool.Count() != 0));
            if (fCreateVennToolInputFile)
            {
                VennToolInputFileFullPath = Path.GetFullPath(parsedArgs.outputVennTool);
            }
            fVerbose = parsedArgs.verbose;
#if false
            if (fVerbose)
            {
                Console.Error.WriteLine(parsedArgs.verbose);
                Console.Error.WriteLine(parsedArgs.inputFiles.Length);
                foreach (string filename in parsedArgs.inputFiles)
                {
                    Console.Error.WriteLine(filename);
                }
            }
#endif
            return parsedArgs;
        }

        // 
        // default printing of List<ISequenceRange>
        //
        public static void ListSequenceRangeToString( IList<ISequenceRange> l )
        {
            // Display the first 10 rows if data (if they are there.)
            int cLinesToDisplay = Math.Min(l.Count, 10);
            
            for (int i = 0; i < cLinesToDisplay; ++i)
                {
                    Console.WriteLine("{0}, {1}, {2}", l[i].ID, l[i].Start, l[i].End);
                }
                if (cLinesToDisplay < l.Count)
                {
                    Console.Error.WriteLine("...");
                }
            Console.Error.WriteLine();
        }


        //
        // print 
        public static long SequenceRangeGroupingCBases(SequenceRangeGrouping srg)
        {
            SequenceRangeGroupingMetrics srgm = new SequenceRangeGroupingMetrics(srg);
            return (srgm.bases);
        }

        // default printing of SequenceRangeGrouping
        //
        public static void SequenceRangeGroupingToString(SequenceRangeGrouping srg, string name)
        {
            Console.Error.Write("[{0}] : SeqeuenceRangeGrouping: ", name);
            SequenceRangeGroupingMetrics srgm = new SequenceRangeGroupingMetrics(srg);
            Console.Error.WriteLine("{0}, {1}, {2}", srgm.groups, srgm.ranges, srgm.bases);

            foreach (string id in srg.GroupIDs)
            {
                Console.Error.WriteLine("--GroupID: {0}, {1}", id, srg.GetGroup(id).Count());
                ListSequenceRangeToString(srg.GetGroup(id));
            }
            Console.Error.WriteLine();
        }

        //
        // Read a Bed file into memory
        //
        public static SequenceRangeGrouping ReadBedFile(string filename)
        {
            BedParser parser = new BedParser();
            IList<ISequenceRange> listSequenceRange = parser.ParseRange(filename);
            if (fVerbose)
            {
                //listSequenceRange.ToString();
                Console.Error.WriteLine("Processed File: {0}", filename);
                ListSequenceRangeToString( listSequenceRange );
            }

            SequenceRangeGrouping srg = new SequenceRangeGrouping(listSequenceRange);
            if (parsedArgs.normalizeInputs)
            {
                srg.MergeOverlaps();        // could be called Normalize() or Cannonicalize()
            }
            return srg;
        }

        public static void Main(string[] args)
        {
            Splash();

            parsedArgs = ProcessCommandLineArguments(args);

            if (parsedArgs.inputFiles.Count() == 2)
            {
                // now read the 2 BED files and do the operation to isolate each set
                SequenceRangeGrouping srg1 = ReadBedFile(parsedArgs.inputFiles[0]);
                SequenceRangeGrouping srg2 = ReadBedFile(parsedArgs.inputFiles[1]);

                SequenceRangeGrouping srgOnly_A, srgOnly_B, srgOnly_AB;

                VennToNodeXL.CreateSequenceRangeGroupingsForVennDiagram(srg1, srg2, out srgOnly_A, out srgOnly_B, out srgOnly_AB);

                if ( fCreateVennToolInputFile || fCreateExcelWorkbook )
                {
                    SequenceRangeGroupingMetrics srgmOnly_A = new SequenceRangeGroupingMetrics(srgOnly_A);
                    SequenceRangeGroupingMetrics srgmOnly_B = new SequenceRangeGroupingMetrics(srgOnly_B);
                    SequenceRangeGroupingMetrics srgmOnly_AB = new SequenceRangeGroupingMetrics(srgOnly_AB);

                    if (fCreateVennToolInputFile)
                    {
                        //SequenceRangeGroupingMetrics srgm = new SequenceRangeGroupingMetrics(srgOnly_A);
                        StreamWriter VennOutput = new StreamWriter(VennToolInputFileFullPath);
                        VennOutput.WriteLine("{0} {1} {2}"
                            , srgmOnly_A.bases
                            , srgmOnly_B.bases
                            , srgmOnly_AB.bases);
                        VennOutput.Close();
                    }

                    if (fCreateExcelWorkbook) 
                    {
                        // create the Excel workbook with a NodeXL Venn diagram
                        VennDiagramData vdd = new VennDiagramData(srgmOnly_A.bases
                            , srgmOnly_B.bases
                            , srgmOnly_AB.bases);
                        VennToNodeXL.CreateVennDiagramNodeXLFile(ExcelWorkbookFullPath, vdd );
                    }
                    if (fVerbose)
                    {
                        SequenceRangeGroupingToString(srgOnly_A, "srgOnly_A");
                        SequenceRangeGroupingToString(srgOnly_B, "srgOnly_B");
                        SequenceRangeGroupingToString(srgOnly_AB, "srgOnly_AB");
                        Console.WriteLine("end two file dump");
                    }
                }
            }
            if (parsedArgs.inputFiles.Count() == 3)
            {
                // TODO:  Reduce memory usage by re-using the SRGs after debugging is complete
                //
                // now read the 3 BED files and do the operation to isolate each set
                SequenceRangeGrouping srg1 = ReadBedFile(parsedArgs.inputFiles[0]);
                SequenceRangeGrouping srg2 = ReadBedFile(parsedArgs.inputFiles[1]);
                SequenceRangeGrouping srg3 = ReadBedFile(parsedArgs.inputFiles[2]);

                SequenceRangeGrouping srgOnly_A, srgOnly_B, srgOnly_C, srgOnly_AB, srgOnly_AC, srgOnly_BC, srgOnly_ABC;

                VennToNodeXL.CreateSequenceRangeGroupingsForVennDiagram(srg1, srg2, srg3, 
                    out srgOnly_A, 
                    out srgOnly_B, 
                    out srgOnly_C, 
                    out srgOnly_AB, 
                    out srgOnly_AC, 
                    out srgOnly_BC, 
                    out srgOnly_ABC);

                /*
                 * We have the set information data for the three files.  
                 * Now what?
                 */
                if (fCreateVennToolInputFile || fCreateExcelWorkbook)
                {
                    // generate the intersection Venn metrics
                    SequenceRangeGroupingMetrics srgmOnly_A = new SequenceRangeGroupingMetrics(srgOnly_A);
                    SequenceRangeGroupingMetrics srgmOnly_B = new SequenceRangeGroupingMetrics(srgOnly_B);
                    SequenceRangeGroupingMetrics srgmOnly_C = new SequenceRangeGroupingMetrics(srgOnly_C);
                    SequenceRangeGroupingMetrics srgmOnly_AB = new SequenceRangeGroupingMetrics(srgOnly_AB);
                    SequenceRangeGroupingMetrics srgmOnly_AC = new SequenceRangeGroupingMetrics(srgOnly_AC);
                    SequenceRangeGroupingMetrics srgmOnly_BC = new SequenceRangeGroupingMetrics(srgOnly_BC);
                    SequenceRangeGroupingMetrics srgmOnly_ABC = new SequenceRangeGroupingMetrics(srgOnly_ABC);

                    if (fCreateVennToolInputFile) 
                    {
                        StreamWriter VennOutput = new StreamWriter(VennToolInputFileFullPath);
                        VennOutput.WriteLine("{0} {1} {2} {3} {4} {5} {6}"
                            , srgmOnly_A.bases
                            , srgmOnly_B.bases
                            , srgmOnly_C.bases
                            , srgmOnly_AB.bases
                            , srgmOnly_AC.bases
                            , srgmOnly_BC.bases
                            , srgmOnly_ABC.bases);
                        VennOutput.Close();
                    }

                    if (fCreateExcelWorkbook)
                    {
                        // create the NodeXL Venn diagram filefile
                        VennDiagramData vdd = new VennDiagramData(srgmOnly_A.bases
                            , srgmOnly_B.bases
                            , srgmOnly_C.bases
                            , srgmOnly_AB.bases
                            , srgmOnly_AC.bases
                            , srgmOnly_BC.bases
                            , srgmOnly_ABC.bases);
                        VennToNodeXL.CreateVennDiagramNodeXLFile(ExcelWorkbookFullPath, vdd);
                    }
                    if (fVerbose)
                    {
                        SequenceRangeGroupingToString(srgOnly_A, "srgOnly_A");
                        SequenceRangeGroupingToString(srgOnly_B, "srgOnly_B");
                        SequenceRangeGroupingToString(srgOnly_C, "srgOnly_C");
                        SequenceRangeGroupingToString(srgOnly_AB, "srgOnly_AB");
                        SequenceRangeGroupingToString(srgOnly_AC, "srgOnly_AC");
                        SequenceRangeGroupingToString(srgOnly_BC, "srgOnly_BC");
                        SequenceRangeGroupingToString(srgOnly_ABC, "srgOnly_ABC");
                        Console.Error.WriteLine("end three file dump");
                    }
                }
            }
        }
    }
}
