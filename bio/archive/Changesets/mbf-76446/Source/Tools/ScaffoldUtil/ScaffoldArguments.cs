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
using System.Diagnostics;
using System.IO;
using System.Linq;
using Bio;
using Bio.Algorithms.Alignment;
using Bio.Algorithms.SuffixTree;
using Bio.IO.FastA;
using CommandLine;
using Bio.Algorithms.Assembly.Comparative;
using Bio.Algorithms.Assembly.Padena.Scaffold;

namespace ScaffoldUtil
{
    /// <summary>
    /// Command line arguments for ScaffoldGeneration.
    /// </summary>
    internal class ScaffoldArguments
    {
        #region Public Fields
        /// <summary>
        /// Paths of input and output file.
        /// </summary>
        [DefaultArgument(ArgumentType.Multiple, HelpText = "File path")]
        public string[] FilePath = null;

        /// <summary>
        /// Print the help information.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "help", ShortName = "h",
            HelpText = "Print the help information.")]
        public bool Help = false;

        /// <summary>
        /// Output file.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "outputFile", ShortName = "o",
            HelpText = "Output file.")]
        public string OutputFile = null;

        /// <summary>
        /// Display verbose logging during processing.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "verbose", ShortName = "v",
            HelpText = "Display verbose logging during processing.")]
        public bool Verbose = false;

        /// <summary>
        /// Length of k-mer.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "k", HelpText = "Length of k-mer")]
        public int KmerLength = 10;

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

        #endregion

        #region Public Method

        /// <summary>
        /// Refine layout in the delta alignments.
        /// </summary>
        public void GenerateScaffolds()
        {
            TimeSpan timeSpan = new TimeSpan();
            Stopwatch runAlgorithm = new Stopwatch();
            FileInfo inputFileinfo = new FileInfo(this.FilePath[0]);
            long inputFileLength = inputFileinfo.Length;
            inputFileinfo = null;

            runAlgorithm.Restart();
            FastAParser parser = new FastAParser(this.FilePath[0]);
            IEnumerable<ISequence> contigs = parser.Parse();
            runAlgorithm.Stop();

            if (this.Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("  Processed Contig file: {0}", Path.GetFullPath(this.FilePath[0]));
                Console.WriteLine("            Read/Processing time: {0}", runAlgorithm.Elapsed);
                Console.WriteLine("            File Size           : {0}", inputFileLength);
            }

            inputFileinfo = new FileInfo(this.FilePath[1]);
            inputFileLength = inputFileinfo.Length;

            runAlgorithm.Restart();
            FastAParser readParser = new FastAParser(this.FilePath[1]);
            IEnumerable<ISequence> reads = readParser.Parse();
            runAlgorithm.Stop();

            if (this.Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("  Processed reads file: {0}", Path.GetFullPath(this.FilePath[1]));
                Console.WriteLine("            Read/Processing time: {0}", runAlgorithm.Elapsed);
                Console.WriteLine("            File Size           : {0}", inputFileLength);
            }

            runAlgorithm.Restart();
            IEnumerable<ISequence> scaffolds = null;
            using (GraphScaffoldBuilder scaffoldBuilder = new GraphScaffoldBuilder())
            {
                 scaffolds = scaffoldBuilder.BuildScaffold(reads, contigs.ToList(), this.KmerLength, this.Depth, this.Redundancy);
            } 
            runAlgorithm.Stop();
            timeSpan = timeSpan.Add(runAlgorithm.Elapsed);

            runAlgorithm.Restart();
            this.WriteSequences(scaffolds);
            runAlgorithm.Stop();


            if (this.Verbose)
            {
                Console.WriteLine("  Compute time: {0}", timeSpan);
                Console.WriteLine("  Write() time: {0}", runAlgorithm.Elapsed);
            }
        }

        #endregion

        #region Private Methods
        private void WriteSequences(IEnumerable<ISequence> sequences)
        {
            using (FastAFormatter ff = new FastAFormatter(this.OutputFile))
            {
                foreach (ISequence sequence in sequences)
                {
                    ff.Write(sequence);
                }
            }
        }
        #endregion
    }
}
