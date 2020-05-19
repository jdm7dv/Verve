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
using Bio.Algorithms.Assembly;
using Bio.Algorithms.Assembly.Padena;
using Bio.IO.FastA;
using System.Diagnostics;
using System.IO;

namespace PadenaUtil
{
    /// <summary>
    /// Class for Assemble options.
    /// </summary>
    internal class AssembleArguments
    {
        #region Public Fields

        /// <summary>
        /// Length of k-mer.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, DefaultValue=10, ShortName = "k", HelpText = "Length of k-mer")]
        public int KmerLength = -1;

        /// <summary>
        /// Threshold for removing dangling ends in graph.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "d", HelpText = "Threshold for removing dangling ends in graph")]
        public int DangleThreshold = -1;

        /// <summary>
        /// Length Threshold for removing redundant paths in graph.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "r", HelpText = "Length Threshold for removing redundant paths in graph")]
        public int RedundantPathLengthThreshold = -1;

        /// <summary>
        /// Threshold for eroding low coverage ends.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "e", HelpText = "Threshold for eroding low coverage ends")]
        public int ErosionThreshold = -1;

        /// <summary>
        /// Bool to do erosion or not.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "i", HelpText = "Bool to do erosion or not.")]
        public bool AllowErosion = false;

        /// <summary>
        /// Whether to estimate kmer length.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "a", HelpText = "Whether to estimate kmer length.")]
        public bool AllowKmerLengthEstimation = false;

        /// <summary>
        /// Threshold used for removing low-coverage contigs.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "c", HelpText = "Threshold used for removing low-coverage contigs.")]
        public int ContigCoverageThreshold = -1;

        /// <summary>
        /// Enable removal of low coverage contigs.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "l", HelpText = " Enable removal of low coverage contigs.")]
        public bool LowCoverageContigRemovalEnabled = false;

        /// <summary>
        /// Help.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "h")]
        public bool Help = false;

        /// <summary>
        /// Input file of reads.
        /// </summary>
        [DefaultArgument(ArgumentType.AtMostOnce, HelpText = "Input file of reads")]
        public string Filename = string.Empty;

        /// <summary>
        /// Output file.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "o", HelpText = "Output file")]
        public string OutputFile = string.Empty;

        /// <summary>
        /// Display verbose logging during processing.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "verbose", ShortName = "v",
            HelpText = "Display verbose logging during processing.")]
        public bool Verbose = false;

        #endregion

        #region Public methods

        /// <summary>
        /// It assembles the sequences.
        /// </summary>
        public virtual void AssembleSequences()
        {
            TimeSpan algorithmSpan = new TimeSpan();
            Stopwatch runAlgorithm = new Stopwatch();
            FileInfo refFileinfo = new FileInfo(this.Filename);
            long refFileLength = refFileinfo.Length;

            runAlgorithm.Restart();
            IEnumerable<ISequence> reads = this.ParseFile();
            runAlgorithm.Stop();
            algorithmSpan = algorithmSpan.Add(runAlgorithm.Elapsed);

            if (this.Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("  Processed read file: {0}", Path.GetFullPath(this.Filename));
                Console.WriteLine("            Read/Processing time: {0}", runAlgorithm.Elapsed);
                Console.WriteLine("            File Size           : {0}", refFileLength);
                Console.WriteLine("            k-mer Length        : {0}", this.KmerLength);
            }

            using (ParallelDeNovoAssembler assembler = new ParallelDeNovoAssembler())
            {
                assembler.AllowErosion = this.AllowErosion;
                assembler.AllowKmerLengthEstimation = this.AllowKmerLengthEstimation;
                assembler.AllowLowCoverageContigRemoval = this.LowCoverageContigRemovalEnabled;
                assembler.ContigCoverageThreshold = this.ContigCoverageThreshold;
                assembler.DanglingLinksThreshold = this.DangleThreshold;
                assembler.ErosionThreshold = this.ErosionThreshold;
                if (!this.AllowKmerLengthEstimation)
                {
                    assembler.KmerLength = this.KmerLength;
                }

                assembler.RedundantPathLengthThreshold = this.RedundantPathLengthThreshold;
                runAlgorithm.Restart();
                IDeNovoAssembly assembly = assembler.Assemble(reads);
                runAlgorithm.Stop();
                algorithmSpan = algorithmSpan.Add(runAlgorithm.Elapsed);
                if (this.Verbose)
                {
                    Console.WriteLine();
                    Console.WriteLine("  Compute time: {0}", runAlgorithm.Elapsed);
                 }

                runAlgorithm.Restart();
                this.WriteContigs(assembly);
                runAlgorithm.Stop();
                algorithmSpan = algorithmSpan.Add(runAlgorithm.Elapsed);
                if (this.Verbose)
                {
                    Console.WriteLine();
                    Console.WriteLine("  Write contigs time: {0}", runAlgorithm.Elapsed);
                    Console.WriteLine("  Total runtime: {0}", algorithmSpan);
                }
            }
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// It Writes the contigs to the file.
        /// </summary>
        /// <param name="assembly">IDeNovoAssembly parameter is the result of running De Novo Assembly on a set of two or more sequences. </param>
        protected void WriteContigs(IDeNovoAssembly assembly)
        {
            if (!string.IsNullOrEmpty(this.OutputFile))
            {
                using (FastAFormatter formatter = new FastAFormatter(this.OutputFile))
                {
                    formatter.AutoFlush = true;

                    foreach (ISequence seq in assembly.AssembledSequences)
                    {
                        formatter.Write(seq);
                    }
                }
            }
            else
            {
                foreach (ISequence seq in assembly.AssembledSequences)
                {
                    Console.WriteLine(seq.ID);
                    Console.WriteLine(new string(seq.Select(a => (char)a).ToArray()));
                }
            }
        }

        /// <summary>
        /// Parses the File.
        /// </summary>
        protected IEnumerable<ISequence> ParseFile()
        {
            // TODO: Add other parsers.
            FastAParser parser = new FastAParser(this.Filename);
            return parser.Parse();
        }

        #endregion
    }
}
