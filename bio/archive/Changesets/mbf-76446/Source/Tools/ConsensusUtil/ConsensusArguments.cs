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
using ConsensusUtil.IO;
using Bio.Algorithms.Assembly.Comparative;

namespace ConsensusUtil
{
    /// <summary>
    /// Command line arguments for ConsensusGeneration.
    /// </summary>
    internal class ConsensusArguments
    {
        #region Public Fields

        /// <summary>
        /// Paths of input and output file.
        /// </summary>
        [DefaultArgument(ArgumentType.Required, HelpText = "File path")]
        public string FilePath = null;

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
        
        #endregion

        #region Public Method

        /// <summary>
        /// Refine layout in the delta alignments.
        /// </summary>
        public void GenerateConsensus()
        {
            TimeSpan timeSpan = new TimeSpan();
            Stopwatch runAlgorithm = new Stopwatch();
            FileInfo inputFileinfo = new FileInfo(this.FilePath);
            long inputFileLength = inputFileinfo.Length;
            inputFileinfo = null;

            runAlgorithm.Restart();
            DeltaAlignmentParser parser = new DeltaAlignmentParser(this.FilePath);
            IList<DeltaAlignment> deltas = parser.Parse();
            runAlgorithm.Stop();

            if (this.Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("  Processed DeltaAlignment file: {0}", Path.GetFullPath(this.FilePath));
                Console.WriteLine("            Read/Processing time: {0}", runAlgorithm.Elapsed);
                Console.WriteLine("            File Size           : {0}", inputFileLength);
            }

            runAlgorithm.Restart();
            IEnumerable<ISequence> consensus = ConsensusGeneration.GenerateConsensus(deltas);
            runAlgorithm.Stop();
            timeSpan = timeSpan.Add(runAlgorithm.Elapsed);

            runAlgorithm.Restart();
            this.WriteSequences(consensus);
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
