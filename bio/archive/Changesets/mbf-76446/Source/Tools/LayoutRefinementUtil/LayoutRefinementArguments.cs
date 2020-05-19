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
using LayoutRefinementUtil.IO;
using Bio.Algorithms.Assembly.Comparative;

namespace LayoutRefinementUtil
{
    /// <summary>
    /// Command line arguments for LayoutRefinement.
    /// </summary>
    internal class LayoutRefinementArguments
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
        public void RefineLayout()
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
            LayoutRefiner.RefineLayout(deltas);
            runAlgorithm.Stop();
            timeSpan = timeSpan.Add(runAlgorithm.Elapsed);

            runAlgorithm.Restart();
            this.WriteDelta(deltas);
            runAlgorithm.Stop();


            if (this.Verbose)
            {
                Console.WriteLine("  Compute time: {0}", timeSpan);
                Console.WriteLine("  Write() time: {0}", runAlgorithm.Elapsed);
            }
        }

        #endregion

        #region Private Methods

         /// <summary>
        /// Writes delta for query sequences.
        /// </summary>
        /// <param name="delta">The Deltas.</param>
        private void WriteDelta(
            IList<DeltaAlignment> delta)
        {
            TextWriter textWriterConsoleOutSave = Console.Out;
            if (!string.IsNullOrEmpty(this.OutputFile))
            {
                FileStream fileStreamConsoleOut = new FileStream(this.OutputFile, FileMode.Create);
                StreamWriter streamWriterConsoleOut = new StreamWriter(fileStreamConsoleOut);
                Console.SetOut(streamWriterConsoleOut);
                streamWriterConsoleOut.AutoFlush = true;
            }

            foreach (DeltaAlignment deltaAlignment in delta)
            {
                Console.WriteLine(
                    ">" +
                    deltaAlignment.ReferenceSequenceId);
                if (deltaAlignment.QuerySequence != null)
                {
                    Console.WriteLine(FastAFormatter.FormatString(deltaAlignment.QuerySequence).Substring(1));
                }
                else
                {
                    //To provide empty lines in place of query sequence id and query sequence
                    Console.WriteLine();
                    Console.WriteLine();
                }
                Console.WriteLine(
                    deltaAlignment.FirstSequenceStart + " " +
                    deltaAlignment.FirstSequenceEnd + " " +
                    deltaAlignment.SecondSequenceStart + " " +
                    deltaAlignment.SecondSequenceEnd + " " +
                    deltaAlignment.Errors + " " +
                    deltaAlignment.SimilarityErrors + " " +
                    deltaAlignment.NonAlphas);

                foreach (long deltas in deltaAlignment.Deltas)
                {
                    Console.WriteLine(deltas);
                }

                Console.WriteLine("*");
            }

            Console.SetOut(textWriterConsoleOutSave);
        }

        #endregion
    }
}
