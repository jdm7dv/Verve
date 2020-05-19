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
using Bio.IO.FastA;
using CommandLine;
using Bio.Algorithms.Assembly.Comparative;
using System.Diagnostics;
using ComparativeUtil.Properties;

namespace ComparativeUtil
{
    /// <summary>
    /// Class for Assemble options.
    /// </summary>
    internal class AssembleArguments
    {
        #region Public Fields

        /// <summary>
        /// Paths of input and output file.
        /// </summary>
        [DefaultArgument(ArgumentType.Multiple, HelpText = "File path")]
        public string[] FilePath = null;

        /// <summary>
        /// Use anchor matches that are unique in both the reference and query.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "kmerlength", ShortName = "k",
            HelpText = "Set kmer length")]
        public int KmerLength = 10;

        /// <summary>
        /// Run scaffolding step after generating contigs
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "scaffold", ShortName = "s",
            HelpText = "Run scaffolding step after generating contigs.")]
        public bool EnableScaffolding = false;

        /// <summary>
        /// Minimum length of MUM.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "MumLength", ShortName = "m",
            HelpText = "Mum Length")]
        public int MumLength = 20;

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
        /// Clone Library Name.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "n", LongName = "clonelibraryname",
            HelpText = "Clone Library Name")]
        public string CloneLibraryName = string.Empty;

        /// <summary>
        /// Mean Length of clone library.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "i", LongName = "meanlengthofinsert",
            HelpText = "Mean Length of clone library.")]
        public double MeanLengthOfInsert = 0;

        /// <summary>
        /// Standard Deviation of Clone Library.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, ShortName = "sd", LongName = "standarddeviationofinsert",
            HelpText = "Standard Deviation of Clone Library.")]
        public double StandardDeviationOfInsert = 0;

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
            if (this.FilePath.Length != 2)
            {
                Console.Error.WriteLine("\nError: A reference file and 1 query file are required.");
                Environment.Exit(-1);
            }

            TimeSpan timeSpan = new TimeSpan();
            Stopwatch runAlgorithm = new Stopwatch();
            FileInfo inputFileinfo = new FileInfo(this.FilePath[0]);
            long inputFileLength = inputFileinfo.Length;
            inputFileinfo = null;

            if (!string.IsNullOrEmpty(this.CloneLibraryName))
            {
                CloneLibrary.Instance.AddLibrary(this.CloneLibraryName, (float)this.MeanLengthOfInsert, (float)this.StandardDeviationOfInsert);
            }
            
            runAlgorithm.Restart();
            // Parse input files
            IEnumerable<ISequence> referenceSequences = new FastAParser(this.FilePath[0]).Parse();
            runAlgorithm.Stop();

            if (this.Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("  Processed reference file: {0}", Path.GetFullPath(this.FilePath[0]));
                Console.WriteLine("            Read/Processing time: {0}", runAlgorithm.Elapsed);
                Console.WriteLine("            File Size           : {0}", inputFileLength);
            }

            inputFileinfo = new FileInfo(this.FilePath[1]);
            inputFileLength = inputFileinfo.Length; 
            runAlgorithm.Restart();
            IEnumerable<ISequence> reads = new FastAParser(this.FilePath[1]).Parse();
            runAlgorithm.Stop();

            if (this.Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("  Processed reads file: {0}", Path.GetFullPath(this.FilePath[1]));
                Console.WriteLine("            Read/Processing time: {0}", runAlgorithm.Elapsed);
                Console.WriteLine("            File Size           : {0}", inputFileLength);
            }

            runAlgorithm.Restart();
            ComparativeGenomeAssembler assembler = new ComparativeGenomeAssembler();
            assembler.ScaffoldingEnabled = this.EnableScaffolding;
            assembler.KmerLength = this.KmerLength;
            assembler.LengthOfMum = this.MumLength;
            IEnumerable<ISequence> assemblerResult = assembler.Assemble(referenceSequences, reads);
            runAlgorithm.Stop();
            timeSpan = timeSpan.Add(runAlgorithm.Elapsed);

            runAlgorithm.Restart();

            if (this.OutputFile == null)
            {
                // Write output to console.
                this.WriteContigs(assemblerResult, Console.Out);
            }
            else
            {
                // Write output to the specified file.
                this.WriteContigs(assemblerResult, null);
                Console.WriteLine(Resources.OutPutWrittenToFileSpecified);
            }
            runAlgorithm.Stop();

            if (this.Verbose)
            {
                Console.WriteLine("  Assemble time: {0}", timeSpan);
                Console.WriteLine("  Write() time: {0}", runAlgorithm.Elapsed);
            }
        }

        #endregion

        #region Protected Members

        /// <summary>
        /// It Writes the contigs to the file.
        /// </summary>
        /// <param name="assembly">IDeNovoAssembly parameter is the result of running De Novo Assembly on a set of two or more sequences. </param>
        /// <param name="outputWriter">A TextWriter to which the output will be written to.</param>
        protected void WriteContigs(IEnumerable<ISequence> assembly, TextWriter outputWriter)
        {
            if (!string.IsNullOrEmpty(this.OutputFile))
            {
                using (FastAFormatter formatter = new FastAFormatter(this.OutputFile))
                {
                    formatter.AutoFlush = true;

                    foreach (ISequence seq in assembly)
                    {
                        formatter.Write(seq);
                    }
                }
            }
            else
            {
                foreach (ISequence seq in assembly)
                {
                    outputWriter.WriteLine(seq.ID);
                    outputWriter.WriteLine(new string(seq.Select(a => (char)a).ToArray()));
                }
            }
        }

        #endregion
    }
}
