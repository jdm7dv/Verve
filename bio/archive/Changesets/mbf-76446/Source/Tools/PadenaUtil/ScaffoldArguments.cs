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
using System.Linq;
using CommandLine;
using Bio;
using Bio.Algorithms.Assembly.Padena.Scaffold;
using Bio.IO.FastA;
using System.Diagnostics;
using System.IO;

namespace PadenaUtil
{
    /// <summary>
    /// This class defines Scaffolding Option.
    /// </summary>
    internal class ScaffoldArguments
    {
        #region Public Fields

        /// <summary>
        /// Length of k-mer.
        /// </summary>
        [Argument(ArgumentType.Required, ShortName = "k", HelpText = "Length of k-mer")]
        public int KmerLength = -1;

        /// <summary>
        /// Help.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "h")]
        public bool Help = false;

        /// <summary>
        /// Input file of reads and contigs.
        /// </summary>
        [DefaultArgument(ArgumentType.Multiple, HelpText = "Input file of reads and contigs.")]
        public string[] FileNames = null;

        /// <summary>
        /// Output file.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "o", HelpText = "Output file")]
        public string OutputFile = string.Empty;

        /// <summary>
        /// Clone Library Name
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "n", HelpText = "Clone Library Name")]
        public string CloneLibraryName = string.Empty;

        /// <summary>
        /// Mean Length of clone library.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "m", HelpText = "Mean Length of clone library.")]
        public double MeanLengthOfInsert = 0;

        /// <summary>
        /// Standard Deviation of Clone Library.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "s", HelpText = "Standard Deviation of Clone Library.")]
        public double StandardDeviationOfInsert = 0;

        /// <summary>
        /// Number of paired read required to connect two contigs.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "r", HelpText = "Number of paired read required to connect two contigs.")]
        public int Redundancy = 2;

        /// <summary>
        /// Depth for graph traversal.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "d", HelpText = "Depth for graph traversal.")]
        public int Depth = 10;

        /// <summary>
        /// Display verbose logging during processing.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "verbose", ShortName = "v",
            HelpText = "Display verbose logging during processing.")]
        public bool Verbose = false;

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates the Scaffold.
        /// </summary>
        public void GenerateScaffold()
        {
            if (!string.IsNullOrEmpty(this.CloneLibraryName))
            {
                CloneLibrary.Instance.AddLibrary(this.CloneLibraryName, (float)this.MeanLengthOfInsert, (float)this.StandardDeviationOfInsert);
            }

            if (this.FileNames.Length != 2)
            {
                Console.Error.WriteLine("\nError: A reference file and 1 query file are required.");
                Environment.Exit(-1);
            }
            TimeSpan algorithmSpan = new TimeSpan();
            Stopwatch runAlgorithm = new Stopwatch();
            FileInfo refFileinfo = null;

            using (GraphScaffoldBuilder scaffoldBuilder = new GraphScaffoldBuilder())
            {
                refFileinfo = new FileInfo(this.FileNames[0]);
                long refFileLength = refFileinfo.Length;

                runAlgorithm.Restart();
                IEnumerable<ISequence> contigs = this.ParseFile(this.FileNames[0]);
                runAlgorithm.Stop();
                algorithmSpan = algorithmSpan.Add(runAlgorithm.Elapsed);

                if (this.Verbose)
                {
                    Console.WriteLine();
                    Console.WriteLine("  Processed contigs file: {0}", Path.GetFullPath(this.FileNames[0]));
                    Console.WriteLine("            Read/Processing time: {0}", runAlgorithm.Elapsed);
                    Console.WriteLine("            File Size           : {0}", refFileLength);
                }

                refFileinfo = new FileInfo(this.FileNames[1]);
                refFileLength = refFileinfo.Length;

                runAlgorithm.Restart();
                IEnumerable<ISequence> reads = this.ParseFile(this.FileNames[1]);
                runAlgorithm.Stop();
                algorithmSpan = algorithmSpan.Add(runAlgorithm.Elapsed);

                if (this.Verbose)
                {
                    Console.WriteLine();
                    Console.WriteLine("  Processed reads file: {0}", Path.GetFullPath(this.FileNames[1]));
                    Console.WriteLine("            Read/Processing time: {0}", runAlgorithm.Elapsed);
                    Console.WriteLine("            File Size           : {0}", refFileLength);
                }

                runAlgorithm.Restart();
                IEnumerable<ISequence> scaffolds = scaffoldBuilder.BuildScaffold(reads, contigs.ToList(), this.KmerLength, this.Depth, this.Redundancy);
                runAlgorithm.Stop();
                algorithmSpan = algorithmSpan.Add(runAlgorithm.Elapsed);
                if (this.Verbose)
                {
                    Console.WriteLine();
                    Console.WriteLine("  Compute time: {0}", runAlgorithm.Elapsed);
                }

                runAlgorithm.Restart();
                WriteContigs(scaffolds);
                runAlgorithm.Stop();
                algorithmSpan = algorithmSpan.Add(runAlgorithm.Elapsed);
                if (this.Verbose)
                {
                    Console.WriteLine();
                    Console.WriteLine("  Write time: {0}", runAlgorithm.Elapsed);
                    Console.WriteLine("  Total runtime: {0}", algorithmSpan);
                }

            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// It writes Contigs to the file.
        /// </summary>
        /// <param name="scaffolds">The list of scaffolds sequence.</param>
        private void WriteContigs(IEnumerable<ISequence> scaffolds)
        {
            if (!string.IsNullOrEmpty(this.OutputFile))
            {
                using (FastAFormatter formatter = new FastAFormatter(this.OutputFile))
                {
                    formatter.AutoFlush = true;

                    foreach (ISequence seq in scaffolds)
                    {
                        formatter.Write(seq);
                    }
                }
            }
            else
            {
                foreach (ISequence seq in scaffolds)
                {
                    Console.WriteLine(seq.ID);
                    Console.WriteLine(new string(seq.Select(a => (char)a).ToArray()));
                }
            }
        }

        /// <summary>
        /// It parses the file.
        /// </summary>
        private IEnumerable<ISequence> ParseFile(string fileName)
        {
            // TODO: Add other parsers.
            FastAParser parser = new FastAParser(fileName);
            return parser.Parse();
        }

        #endregion
    }
}
