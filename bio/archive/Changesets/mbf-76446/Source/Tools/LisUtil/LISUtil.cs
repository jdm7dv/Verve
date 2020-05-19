﻿// *********************************************************
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
using System.Text;
using LISUtil.Utils;
using Bio;
using Bio.IO.FastA;
using Bio.Algorithms.MUMmer;
using Bio.Algorithms.MUMmer.LIS;
using Bio.Algorithms.SuffixTree;
using LISUtil.Properties;

namespace LisUtil
{
    class LISUtil
    {
        private static FileStream fileStreamConsoleOut;
        private static StreamWriter streamWriterConsoleOut;
        private static TextWriter textWriterConsoleOutSave;

        private static CommandLineOptions ProcessCommandLine(string[] args)
        {
            CommandLineOptions myArgs = new CommandLineOptions();
            try
            {
                Parser.ParseArgumentsWithUsage(args, myArgs);
            }

            catch (Exception e)
            {
                Console.Error.WriteLine("\nException while processing Command Line arguments [{0}]", e.ToString());
                Environment.Exit(-1);
            }

            /*
             * Process all the arguments for 'semantic' correctness
             */
            if (!myArgs.PerformLISOnly)
            {
                if ((myArgs.MaxMatch && myArgs.Mum)
                    || (myArgs.MaxMatch && myArgs.Mumreference)
                    || (myArgs.Mum && myArgs.Mumreference))
                {
                    Console.Error.WriteLine("\nError: only one of -maxmatch, -mum, -mumreference options can be specified.");
                    Environment.Exit(-1);
                }

                if (!myArgs.Mumreference && !myArgs.Mum && !myArgs.MaxMatch)
                {
                    myArgs.Mumreference = true;
                }
            }

            if (myArgs.FileList == null || myArgs.FileList.Length == 0)
            {
                Console.Error.WriteLine("\nError: Atleast one input file needed.");
                Environment.Exit(-1);
            }

            if (myArgs.FileList.Length < 2 && !myArgs.PerformLISOnly)
            {
                Console.Error.WriteLine("\nError: A reference file and at least 1 query file are required.");
                Environment.Exit(-1);
            }

            if (myArgs.FileList.Length < 1 && myArgs.PerformLISOnly)
            {
                Console.Error.WriteLine("\nError: A file containing list of MUMs required.");
                Environment.Exit(-1);
            }

            if ((myArgs.Length <= 2) || (myArgs.Length >= (8 * 1024)))
            {
                // TODO: What are real reasonable mum length limits?
                Console.Error.WriteLine("\nError: mum length must be between 10 and 1024.");
                Environment.Exit(-1);
            }

            if (myArgs.Both && myArgs.ReverseOnly)
            {
                Console.Error.WriteLine("\nError: only one of -both or -reverseOnly options can be specified.");
                Environment.Exit(-1);
            }

            if (myArgs.C && (!myArgs.Both && !myArgs.ReverseOnly))
            {
                Console.Error.WriteLine("\nError: c requires one of either /b or /r options.");
                Environment.Exit(-1);
            }

            if (myArgs.OutputFile != null)
            {   // redirect stdout
                textWriterConsoleOutSave = Console.Out;
                fileStreamConsoleOut = new FileStream(myArgs.OutputFile, FileMode.Create);
                streamWriterConsoleOut = new StreamWriter(fileStreamConsoleOut);
                Console.SetOut(streamWriterConsoleOut);
                streamWriterConsoleOut.AutoFlush = true;
            }

            return (myArgs);
        }

        private static string SplashString()
        {
            const string SplashString = "\nLisUtil v2.0 - Longest Increasing Subsequence Utility"
                                      + "\n  Copyright (c) Microsoft 2010, 2011";
            return (SplashString);
        }

        // Given a list of sequences, create a new list with only the Reverse Complements
        //   of the original sequences.
        private static IEnumerable<ISequence> ReverseComplementSequenceList(IEnumerable<ISequence> sequenceList)
        {
            foreach (ISequence seq in sequenceList)
            {
                ISequence seqReverseComplement = seq.GetReverseComplementedSequence();

                if (seqReverseComplement != null)
                {
                    seqReverseComplement.ID = seqReverseComplement.ID + " Reverse";

                    // seqReverseComplement.DisplayID = seqReverseComplement.DisplayID + " Reverse";
                }

                yield return seqReverseComplement;
            }
        }

        // Given a list of sequences, create a new list with the original sequence followed
        // by the Reverse Complement of that sequence.
        private static IEnumerable<ISequence> AddReverseComplementsToSequenceList(IEnumerable<ISequence> sequenceList)
        {
            foreach (ISequence seq in sequenceList)
            {
                ISequence seqReverseComplement = seq.GetReverseComplementedSequence();

                if (seqReverseComplement != null)
                {
                    seqReverseComplement.ID = seqReverseComplement.ID + " Reverse";
                }

                yield return seq;
                yield return seqReverseComplement;
            }
        }

        private static void WriteMums(IEnumerable<Match> mums, ISequence refSequence, ISequence querySequence, CommandLineOptions myArgs)
        {
            // write the QuerySequenceId
            string displayID = querySequence.ID;
            Console.Write("> {0}", displayID);
            if (myArgs.DisplayQueryLength)
            {
                Console.Write(" {1}", querySequence.Count);
            }

            Console.WriteLine();

            bool isReverseComplement = myArgs.C && displayID.EndsWith(" Reverse");

            // foreach (MaxUniqueMatch m in sortedMums)
            // {
            // Start is 1 based in literature but in programming (e.g. MaxUniqueMatch) they are 0 based.  
            // Add 1
            foreach (Match match in mums)
            {
                Console.WriteLine(
                        "{0,8}  {1,8}  {2,8}",
                        match.ReferenceSequenceOffset + 1,
                        !isReverseComplement ? match.QuerySequenceOffset + 1 : querySequence.Count - match.QuerySequenceOffset,
                        match.Length);
                if (myArgs.ShowMatchingString)
                {
                    for (int i = 0; i < match.Length; ++i)
                    {
                        // mummer uses all lowercase and MBF uses uppercase...convert on display
                        Console.Write(char.ToLowerInvariant((char)querySequence[match.QuerySequenceOffset + i]));
                    }

                    Console.WriteLine();
                }
            }
        }

        private static void WriteMums(IEnumerable<Match> mums)
        {
            foreach (Match match in mums)
            {
                Console.WriteLine(
                        "{0,8}  {1,8}  {2,8}",
                        match.ReferenceSequenceOffset + 1,
                        match.QuerySequenceOffset + 1,
                        match.Length);
            }
        }

        private static void ShowSequence(ISequence seq)
        {
            const long DefaultSequenceDisplayLength = 25;

            Console.Error.WriteLine("--- Sequence Dump ---");
            Console.Error.WriteLine("     Type: {0}", seq.GetType());
            Console.Error.WriteLine("       ID: {0}", seq.ID);
            Console.Error.WriteLine("    Count: {0}", seq.Count);
            Console.Error.WriteLine(" Alphabet: {0}", seq.Alphabet);
            long lengthToPrint = (seq.Count <= DefaultSequenceDisplayLength) ? seq.Count : DefaultSequenceDisplayLength;
            StringBuilder printString = new StringBuilder((int)lengthToPrint);
            for (int i = 0; i < lengthToPrint; ++i)
            {
                printString.Append((char)seq[i]);
            }

            Console.Error.WriteLine(" Sequence: {0}{1}", printString, lengthToPrint >= DefaultSequenceDisplayLength ? "..." : String.Empty);
            Console.Error.WriteLine();
        }

        private static void Main(string[] args)
        {
            try
            {
                Stopwatch stopWatchMumUtil = Stopwatch.StartNew();
                Stopwatch stopWatchInterval = new Stopwatch();
                Console.Error.WriteLine(SplashString());
                CommandLineOptions options = new CommandLineOptions();
                if (args.Length > 0 && Parser.ParseArguments(args, options))
                {
                    if (options.Help)
                    {
                        Console.WriteLine(Resources.LisUtilHelp);
                    }
                    else
                    {
                        CommandLineOptions myArgs = ProcessCommandLine(args);
                        TimeSpan writetime = new TimeSpan();
                        LongestIncreasingSubsequence lis = new LongestIncreasingSubsequence();
                        IEnumerable<Match> mums;
                        if (myArgs.PerformLISOnly)
                        {
                            stopWatchInterval.Restart();
                            IList<Match> parsedMUMs = ParseMums(myArgs.FileList[0]);
                            stopWatchInterval.Stop();

                            if (myArgs.Verbose)
                            {
                                Console.Error.WriteLine();
                                Console.Error.WriteLine("  Processed MUM file: {0}", Path.GetFullPath(myArgs.FileList[0]));
                                Console.Error.WriteLine("        Total MUMs: {0:#,000}", parsedMUMs.Count);
                                Console.Error.WriteLine("            Read/Processing time: {0}", stopWatchInterval.Elapsed);
                            }

                            stopWatchInterval.Restart();
                            IList<Match> sortedMUMs = lis.SortMum(parsedMUMs);
                            stopWatchInterval.Stop();

                            if (myArgs.Verbose)
                            {
                                Console.Error.WriteLine();
                                Console.Error.WriteLine("  Sort MUM time: {0}", stopWatchInterval.Elapsed);
                            }

                            stopWatchInterval.Restart();
                            mums = lis.GetLongestSequence(sortedMUMs);
                            stopWatchInterval.Stop();
                            if (myArgs.Verbose)
                            {
                                Console.Error.WriteLine();
                                Console.Error.WriteLine("  Perform LIS time: {0}", stopWatchInterval.Elapsed);
                            }

                            stopWatchInterval.Restart();
                            WriteMums(mums);
                            stopWatchInterval.Stop();
                            if (myArgs.Verbose)
                            {
                                Console.Error.WriteLine();
                                Console.Error.WriteLine("  Write MUM time: {0}", stopWatchInterval.Elapsed);
                            }

                            stopWatchMumUtil.Stop();
                        }
                        else
                        {
                            FileInfo refFileinfo = new FileInfo(myArgs.FileList[0]);
                            long refFileLength = refFileinfo.Length;
                            refFileinfo = null;

                            stopWatchInterval.Restart();
                            IEnumerable<ISequence> referenceSequences = ParseFastA(myArgs.FileList[0]);
                            ISequence referenceSequence = referenceSequences.First();
                            stopWatchInterval.Stop();
                            if (myArgs.Verbose)
                            {
                                Console.Error.WriteLine();
                                Console.Error.WriteLine("  Processed Reference FastA file: {0}", Path.GetFullPath(myArgs.FileList[0]));
                                Console.Error.WriteLine("        Length of first Sequence: {0:#,000}", referenceSequence.Count);
                                Console.Error.WriteLine("            Read/Processing time: {0}", stopWatchInterval.Elapsed);
                                Console.Error.WriteLine("            File Size           : {0}", refFileLength);
                            }

                            FileInfo queryFileinfo = new FileInfo(myArgs.FileList[1]);
                            long queryFileLength = queryFileinfo.Length;
                            refFileinfo = null;

                            stopWatchInterval.Restart();
                            IEnumerable<ISequence> querySequences = ParseFastA(myArgs.FileList[1]);
                            stopWatchInterval.Stop();
                            if (myArgs.Verbose)
                            {
                                Console.Error.WriteLine();
                                Console.Error.WriteLine("      Processed Query FastA file: {0}", Path.GetFullPath(myArgs.FileList[1]));
                                Console.Error.WriteLine("            Read/Processing time: {0}", stopWatchInterval.Elapsed);
                                Console.Error.WriteLine("            File Size           : {0}", queryFileLength);
                            }

                            if (myArgs.ReverseOnly)
                            {
                                stopWatchInterval.Restart();
                                querySequences = ReverseComplementSequenceList(querySequences);
                                stopWatchInterval.Stop();
                                if (myArgs.Verbose)
                                {
                                    Console.Error.WriteLine("         Reverse Complement time: {0}", stopWatchInterval.Elapsed);
                                }
                            }
                            else if (myArgs.Both)
                            {   // add the reverse complement sequences to the query list too
                                stopWatchInterval.Restart();
                                querySequences = AddReverseComplementsToSequenceList(querySequences);
                                stopWatchInterval.Stop();
                                if (myArgs.Verbose)
                                {
                                    Console.Error.WriteLine("     Add Reverse Complement time: {0}", stopWatchInterval.Elapsed);
                                }
                            }

                            TimeSpan mummerTime = new TimeSpan(0, 0, 0);
                            stopWatchInterval.Restart();
                            Sequence seq = referenceSequence as Sequence;
                            if (seq == null)
                            {
                                throw new ArgumentException("MUMmer supports only Sequence class");
                            }

                            MUMmer mummer = new MUMmer(seq);
                            stopWatchInterval.Stop();
                            if (myArgs.Verbose)
                            {
                                Console.Error.WriteLine("Suffix tree construction time: {0}", stopWatchInterval.Elapsed);
                            }

                            mummer.LengthOfMUM = myArgs.Length;
                            mummer.NoAmbiguity = myArgs.NoAmbiguity;
                            long querySeqCount = 0;
                            double sumofSeqLength = 0;
                            if (myArgs.MaxMatch)
                            {
                                foreach (ISequence querySeq in querySequences)
                                {
                                    stopWatchInterval.Restart();
                                    mums = lis.GetLongestSequence(lis.SortMum(GetMumsForLIS(mummer.GetMatches(querySeq))));
                                    stopWatchInterval.Stop();
                                    mummerTime = mummerTime.Add(stopWatchInterval.Elapsed);
                                    stopWatchInterval.Restart();
                                    WriteMums(mums, referenceSequence, querySeq, myArgs);
                                    stopWatchInterval.Stop();
                                    writetime = writetime.Add(stopWatchInterval.Elapsed);
                                    querySeqCount++;
                                    sumofSeqLength += querySeq.Count;
                                }

                                if (myArgs.Verbose)
                                {
                                    Console.Error.WriteLine("             Number of query Sequences: {0}", querySeqCount);
                                    Console.Error.WriteLine("             Average length of query Sequences: {0}", sumofSeqLength / querySeqCount);
                                    Console.Error.WriteLine("  Compute GetMumsMaxMatch() with LIS time: {0}", mummerTime);
                                }
                            }
                            else if (myArgs.Mum)
                            {
                                foreach (ISequence querySeq in querySequences)
                                {
                                    stopWatchInterval.Restart();
                                    mums = lis.GetLongestSequence(lis.SortMum(GetMumsForLIS(mummer.GetMatchesUniqueInReference(querySeq))));
                                    stopWatchInterval.Stop();
                                    mummerTime = mummerTime.Add(stopWatchInterval.Elapsed);
                                    stopWatchInterval.Restart();
                                    WriteMums(mums, referenceSequence, querySeq, myArgs);
                                    stopWatchInterval.Stop();
                                    writetime = writetime.Add(stopWatchInterval.Elapsed);
                                    querySeqCount++;
                                    sumofSeqLength += querySeq.Count;
                                }

                                if (myArgs.Verbose)
                                {
                                    Console.Error.WriteLine("             Number of query Sequences: {0}", querySeqCount);
                                    Console.Error.WriteLine("             Average length of query Sequences: {0}", sumofSeqLength / querySeqCount);
                                    Console.Error.WriteLine("       Compute GetMumsMum() with LIS time: {0}", mummerTime);
                                }
                            }
                            else if (myArgs.Mumreference)
                            {
                                // NOTE:
                                //     mum3.GetMUMs() this really implements the GetMumReference() functionality
                                // mums = mum3.GetMumsReference( referenceSequences[0], querySequences);     // should be
                                foreach (ISequence querySeq in querySequences)
                                {
                                    stopWatchInterval.Restart();
                                    mums = lis.GetLongestSequence(lis.SortMum(GetMumsForLIS(mummer.GetMatchesUniqueInReference(querySeq))));
                                    stopWatchInterval.Stop();
                                    mummerTime = mummerTime.Add(stopWatchInterval.Elapsed);
                                    stopWatchInterval.Restart();

                                    // do sort
                                    // WriteLongestIncreasingSubsequences
                                    WriteMums(mums, referenceSequence, querySeq, myArgs);
                                    stopWatchInterval.Stop();
                                    writetime = writetime.Add(stopWatchInterval.Elapsed);
                                    querySeqCount++;
                                    sumofSeqLength += querySeq.Count;
                                }

                                if (myArgs.Verbose)
                                {
                                    Console.Error.WriteLine("             Number of query Sequences: {0}", querySeqCount);
                                    Console.Error.WriteLine("             Average length of query Sequences: {0}", sumofSeqLength / querySeqCount);
                                    Console.Error.WriteLine(" Compute GetMumsReference() time: {0}", mummerTime);
                                }
                            }
                            else
                            {
                                // cannot happen as argument processing already asserted one of the three options must be specified
                                Console.Error.WriteLine("\nError: one of /maxmatch, /mum, /mumreference options must be specified.");
                                Environment.Exit(-1);

                                // kill the error about unitialized use of 'mums' in the next block...the compiler does not recognize 
                                // Environment.Exit() as a no-return function
                                throw new Exception("Never hit this");
                            }
                        }

                        if (myArgs.Verbose)
                        {
                            Console.Error.WriteLine("                WriteMums() time: {0}", writetime);
                        }

                        stopWatchMumUtil.Stop();
                        if (myArgs.Verbose)
                        {
                            Console.Error.WriteLine("           Total LisUtil Runtime: {0}", stopWatchMumUtil.Elapsed);
                        }
                    }
                }
                else
                {
                    Console.WriteLine(Resources.LisUtilHelp);
                }
             }
            catch (Exception ex)
            {
                DisplayException(ex);
            }
        }

        /// <summary>
        /// Generates a list of MUMs for computing LIS.
        /// </summary>
        /// <param name="mums">MUMs generated by the MUMmer.</param>
        /// <returns>List of MUMs.</returns>
        private static IList<Match> GetMumsForLIS(IEnumerable<Match> mums)
        {
            int index = 0;
            IList<Match> querySortedMum2 = new List<Match>();

            foreach (Match sortedMum in mums)
            {
                Match mum2 = new Match();
                mum2.ReferenceSequenceOffset = sortedMum.ReferenceSequenceOffset;
                mum2.QuerySequenceOffset = sortedMum.QuerySequenceOffset;
                mum2.Length = sortedMum.Length;
                querySortedMum2.Add(mum2);
                index++;
            }

            return querySortedMum2;
        }

        /// <summary>
        /// Parses MUMs from the input file.
        /// </summary>
        /// <param name="filename">MUM file name.</param>
        /// <returns>List of MUMs.</returns>
        private static IList<Match> ParseMums(string filename)
        {
            // TODO: Parse files with multiple query sequences
            IList<Match> mumList = new List<Match>();
            int index = 0;
            using (TextReader tr = File.OpenText(filename))
            {
                string line;
                while ((line = tr.ReadLine()) != null)
                {
                    if (!line.StartsWith(">"))
                    {
                        string[] items = line.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (items[0] != ">")
                        {
                            Match mum2 = new Match();
                            mum2.ReferenceSequenceOffset = Convert.ToInt32(items[0]);
                            mum2.QuerySequenceOffset = Convert.ToInt32(items[1]);
                            mum2.Length = Convert.ToInt32(items[2]);
                            mumList.Add(mum2);
                        }

                        index++;
                    }
                }
            }

            return mumList;
        }

#if true
        /// <summary>
        /// Parses a FastA file which has one or more sequences.
        /// </summary>
        /// <param name="filename">Path to the file to be parsed.</param>
        /// <returns>List of ISequence objects.</returns>
        private static IEnumerable<ISequence> ParseFastA(string filename)
        {
            // A new parser to import a file
            FastAParser parser = new FastAParser(filename);
            return parser.Parse();
        }
#endif

        /// <summary>
        /// Display Exception Messages, if inner exception found then displays the inner exception.
        /// </summary>
        /// <param name="ex">The Exception.</param>
        private static void DisplayException(Exception ex)
        {
            if (ex.InnerException == null || string.IsNullOrEmpty(ex.InnerException.Message))
            {
                Console.Error.WriteLine(ex.Message);
            }
            else
            {
                DisplayException(ex.InnerException);
            }
        }

        private class CommandLineOptions
        {
            // Mummer commandline from summer interns
            //   mummer -maxmatch -b -c -s -n chromosome_1_1_1_1.fasta read_chrom1_2000.fasta > mummer2000-n.txt
            // translates to
            //   mumutil -maxmatch -b -c -s -n chromosome_1_1_1_1.fasta read_chrom1_2000.fasta > mummer2000-n.txt
            [Argument(ArgumentType.AtMostOnce, ShortName = "h", HelpText = "Show the help information with program options and a description of program operation.")]
            public bool Help;
            [Argument(ArgumentType.AtMostOnce, ShortName = "v", HelpText = "Display Verbose logging during processing")]
            public bool Verbose;

            [Argument(ArgumentType.AtMostOnce, HelpText = "Compute all MUM candidates in reference [default]")]
            public bool Mumreference;
            [Argument(ArgumentType.AtMostOnce, HelpText = "Compute Maximal Unique Matches, strings that are unique in both the reference and query sets")]
            public bool Mum;
            [Argument(ArgumentType.AtMostOnce, HelpText = "Compute all maximal matches ignoring uniqueness")]
            public bool MaxMatch;

            [Argument(ArgumentType.AtMostOnce, ShortName = "n", HelpText = "Disallow ambiguity character matches and only match A, T, C, or G (case insensitive)")]
            public bool NoAmbiguity;            // -n 

            [Argument(ArgumentType.AtMostOnce, ShortName = "l", HelpText = "Minimum match length [20]")]
            public int Length;                  // -l #

            [Argument(ArgumentType.AtMostOnce, ShortName = "b", HelpText = "Compute forward and reverse complement matches")]
            public bool Both;                   // -b
            [Argument(ArgumentType.AtMostOnce, ShortName = "r", HelpText = "Compute only reverse complement matches")]
            public bool ReverseOnly;            // -r

            [Argument(ArgumentType.AtMostOnce, ShortName = "d", HelpText = "Show the length of the query sequence")]
            public bool DisplayQueryLength;     // -d

            [Argument(ArgumentType.AtMostOnce, ShortName = "s", HelpText = "Show the matching substring in the output")]
            public bool ShowMatchingString;     // -s

            [Argument(ArgumentType.AtMostOnce, ShortName = "c", HelpText = "Report the query position of a reverse complement match relative to the forward strand of the query sequence")]
            public bool C;                      // -c
#if false
            [Argument(ArgumentType.AtMostOnce, HelpText = "Force 4 column output format that prepends every match line with the reference sequence identifer")]
            public bool F;             // -F
#endif
#if false
            [Argument(ArgumentType.Required, HelpText = "Reference file containing the reference FASTA information")]
            public string referenceFilename;
            [DefaultArgument(ArgumentType.MultipleUnique, HelpText = "Query file(s) containing the query FASTA sequence information")]
            public string[] queryFiles;
#endif
            [Argument(ArgumentType.AtMostOnce, ShortName = "o", HelpText = "Output file")]
            public string OutputFile;

            [DefaultArgument(ArgumentType.Multiple, HelpText = "Query file(s) containing the query strings")]
            public string[] FileList;

            [Argument(ArgumentType.AtMostOnce, ShortName = "p", HelpText = "Perform LIS only on input list of MUMs")]
            public bool PerformLISOnly = false;

            public CommandLineOptions()
            {
                // use assignments in the constructor to avoid the warning about unwritten variables
                this.Help = false;
                this.Verbose = false;
                this.Mumreference = false;
                this.Mum = false;
                this.MaxMatch = false;
                this.NoAmbiguity = false;
                this.Length = 20;
                this.Both = false;
                this.ReverseOnly = false;
                this.DisplayQueryLength = false;
                this.ShowMatchingString = false;
                this.C = false;
                this.OutputFile = null;
                this.FileList = null;
            }
        }
    }
}