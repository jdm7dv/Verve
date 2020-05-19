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
using System.Collections.Generic;
using CommandLine;
using Bio;
using Bio.Algorithms.Assembly;
using Bio.Algorithms.Assembly.Padena;
using System.Diagnostics;
using System.IO;
using System;

namespace PadenaUtil
{
    /// <summary>
    /// This class defines AssembleWithScaffold options.
    /// </summary>
    internal class AssembleWithScaffoldArguments : AssembleArguments
    {
        #region Public Fields

        /// <summary>
        /// Clone Library Name.
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
        [Argument(ArgumentType.AtMostOnce, ShortName = "b", HelpText = "Number of paired read required to connect two contigs.")]
        public int Redundancy = 2;

        /// <summary>
        /// Depth for graph traversal.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "f", HelpText = "Depth for graph traversal.")]
        public int Depth = 10;

        #endregion

        #region Public methods

        /// <summary>
        /// It assembles the sequences.
        /// </summary>
        public override void AssembleSequences()
        {
            TimeSpan algorithmSpan = new TimeSpan();
            Stopwatch runAlgorithm = new Stopwatch();
            FileInfo refFileinfo = new FileInfo(this.Filename);
            long refFileLength = refFileinfo.Length;

            if (!string.IsNullOrEmpty(this.CloneLibraryName))
            {
                CloneLibrary.Instance.AddLibrary(this.CloneLibraryName, (float)this.MeanLengthOfInsert, (float)this.StandardDeviationOfInsert);
            }

            runAlgorithm.Restart();
            IEnumerable<ISequence> reads = ParseFile();
            runAlgorithm.Stop();
            algorithmSpan = algorithmSpan.Add(runAlgorithm.Elapsed);

            if (this.Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("  Processed read file: {0}", Path.GetFullPath(this.Filename));
                Console.WriteLine("            Read/Processing time: {0}", runAlgorithm.Elapsed);
                Console.WriteLine("            File Size           : {0}", refFileLength);
            }

            ParallelDeNovoAssembler assembler = new ParallelDeNovoAssembler();
            assembler.AllowErosion = AllowErosion;
            assembler.AllowKmerLengthEstimation = AllowKmerLengthEstimation;
            assembler.AllowLowCoverageContigRemoval = LowCoverageContigRemovalEnabled;
            assembler.ContigCoverageThreshold = ContigCoverageThreshold;
            assembler.DanglingLinksThreshold = DangleThreshold;
            assembler.ErosionThreshold = ErosionThreshold;
            if (!this.AllowKmerLengthEstimation)
            {
                assembler.KmerLength = this.KmerLength;
            }

            assembler.RedundantPathLengthThreshold = RedundantPathLengthThreshold;
            runAlgorithm.Restart();
            IDeNovoAssembly assembly = assembler.Assemble(reads, true);
            runAlgorithm.Stop();
            algorithmSpan = algorithmSpan.Add(runAlgorithm.Elapsed);
            if (this.Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("  Compute time: {0}", runAlgorithm.Elapsed);
            }

            runAlgorithm.Restart();
            WriteContigs(assembly);
            runAlgorithm.Stop();
            algorithmSpan = algorithmSpan.Add(runAlgorithm.Elapsed);
            if (this.Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("  Write time: {0}", runAlgorithm.Elapsed);
                Console.WriteLine("  Total runtime: {0}", algorithmSpan);
            }
        }

        #endregion
    }
}
