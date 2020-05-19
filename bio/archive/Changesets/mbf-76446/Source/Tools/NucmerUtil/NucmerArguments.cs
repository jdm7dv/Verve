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

namespace NucmerUtil
{
    /// <summary>
    /// Command line arguments for NUCmer.
    /// </summary>
    internal class NucmerArguments
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
        [Argument(ArgumentType.AtMostOnce, LongName = "mum", ShortName = "m",
            HelpText = "Use anchor matches that are unique in both the reference and query.")]
        public bool Mum = false;

        /// <summary>
        /// Use anchor matches that are unique in the reference but not necessarily unique in the query (default behavior).
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "mumreference", ShortName = "r",
            HelpText = "Use anchor matches that are unique in the reference but not necessarily unique in the query"
             + "(default behavior).")]
        public bool MumReference = true;

        /// <summary>
        /// Use all anchor matches regardless of their uniqueness.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "maxmatch", ShortName = "x",
            HelpText = "Use all anchor matches regardless of their uniqueness.")]
        public bool MaxMatch = false;

        /// <summary>
        /// Distance an alignment extension will attempt to extend poor scoring regions before giving up (default 200).
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "breaklength", ShortName = "b",
            HelpText = "Distance an alignment extension will attempt to extend poor scoring regions" +
            "before giving up (default 200).")]
        public int BreakLength = 200;

        /// <summary>
        /// Minimum cluster length (default 65).
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "mincluster", ShortName = "c",
            HelpText = "Minimum cluster length (default 65).")]
        public int MinCluster = 65;

        /// <summary>
        /// Maximum diagonal difference factor for clustering, i.e. diagonal difference / match separation (default 0.12).
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "diagfactor", ShortName = "d",
            HelpText = "Maximum diagonal difference factor for clustering," +
            "i.e. diagonal difference / match separation (default 0.12).")]
        public double DiagFactor = 0.12;

        /// <summary>
        /// Align only the forward strands of each sequence.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "forwardAndReverse", ShortName = "f",
            HelpText = "Align only the forward and reverse strands of each sequence.")]
        public bool ForwardAndReverse = false;

        /// <summary>
        /// Maximum gap between two adjacent matches in a cluster (default 90).
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "maxgap", ShortName = "g",
            HelpText = "Maximum gap between two adjacent matches in a cluster (default 90).")]
        public int MaxGap = 90;

        /// <summary>
        /// Print the help information.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "help", ShortName = "h",
            HelpText = "Print the help information.")]
        public bool Help = false;

        /// <summary>
        /// Minimum length of an maximal exact match (default 20).
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "minmatch", ShortName = "l",
            HelpText = "Minimum length of an maximal exact match (default 20).")]
        public int MinMatch = 20;

        /// <summary>
        /// Output file.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "outputFile", ShortName = "o",
            HelpText = "Output file.")]
        public string OutputFile = null;

        /// <summary>
        /// Align only the reverse strand of the query sequence to the forward strand of the reference.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "reverse", ShortName = "e",
            HelpText = "Align only the reverse strand of the query sequence to the forward strand of the reference.")]
        public bool Reverse = false;

        /// <summary>
        /// Toggle the outward extension of alignments from their anchoring clusters. Setting --noextend will 
        /// prevent alignment extensions but still align the DNA between clustered matches and create the .delta file 
        /// (default --extend).
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "extend", ShortName = "n",
            HelpText = "Toggle the outward extension of alignments from their anchoring clusters." +
            "Setting --noextend will prevent alignment extensions but still align the DNA between clustered" +
            "matches and create the .delta file (default extend)")]
        public bool NotExtend = false;

        /// <summary>
        /// Display verbose logging during processing.
        /// </summary>
        [Argument(ArgumentType.AtMostOnce, LongName = "verbose", ShortName = "v",
            HelpText = "Display verbose logging during processing.")]
        public bool Verbose = false;
        
        #endregion

        #region Private Fields

        /// <summary>
        /// Time taken to get reverse compliment.
        /// </summary>
        private static TimeSpan timeTakenToGetReverseComplement = new TimeSpan();

        /// <summary>
        /// Time taken to parse query sequences.
        /// </summary>
        private static TimeSpan timeTakenToParseQuerySequences = new TimeSpan();

        #endregion

        #region Public Method

        /// <summary>
        /// Align the sequences.
        /// </summary>
        public void Align()
        {
            TimeSpan nucmerSpan = new TimeSpan();
            Stopwatch runNucmer = new Stopwatch();
            FileInfo refFileinfo = new FileInfo(this.FilePath[0]);
            long refFileLength = refFileinfo.Length;
            refFileinfo = null;

            runNucmer.Restart();
            IEnumerable<ISequence> referenceSequence = this.Parse(this.FilePath[0]);
            runNucmer.Stop();

            if (this.Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("  Processed Reference FastA file: {0}", Path.GetFullPath(this.FilePath[0]));
                Console.WriteLine("            Read/Processing time: {0}", runNucmer.Elapsed);
                Console.WriteLine("            File Size           : {0}", refFileLength);
            }

            refFileinfo = new FileInfo(this.FilePath[1]);
            refFileLength = refFileinfo.Length;
            
            runNucmer.Restart();
            IEnumerable<ISequence> querySequence = this.Parse(this.FilePath[1]);
            runNucmer.Stop();

            if (this.Verbose)
            {
                Console.WriteLine();
                Console.WriteLine("  Processed Query FastA file: {0}", Path.GetFullPath(this.FilePath[1]));
                Console.WriteLine("            Read/Processing time: {0}", runNucmer.Elapsed);
                Console.WriteLine("            File Size           : {0}", refFileLength);
            }

            if (!this.NotExtend)
            {
                runNucmer.Restart();
                IList<List<IEnumerable<DeltaAlignment>>> delta = this.GetDelta(referenceSequence, querySequence);
                runNucmer.Stop();
                nucmerSpan = nucmerSpan.Add(runNucmer.Elapsed);

                runNucmer.Restart();
                this.WriteDelta(delta);
                runNucmer.Stop();
            }
            else
            {
                runNucmer.Restart();
                IList<List<IList<Cluster>>> clusters = this.GetCluster(referenceSequence, querySequence);
                runNucmer.Stop();
                nucmerSpan = nucmerSpan.Add(runNucmer.Elapsed);

                runNucmer.Restart();
                this.WriteCluster(clusters, referenceSequence, querySequence);
                runNucmer.Stop();
            }

            if (this.Verbose)
            {
                if (this.Reverse || this.ForwardAndReverse)
                {
                    Console.WriteLine("            Read/Processing time: {0}", timeTakenToParseQuerySequences);
                    Console.WriteLine("         Reverse Complement time: {0}", timeTakenToGetReverseComplement);
                }
                
                Console.WriteLine("  Compute time: {0}", nucmerSpan);
                Console.WriteLine("  Write() time: {0}", runNucmer.Elapsed);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Given a list of sequences, create a new list with only the Reverse Complements
        /// of the original sequences.
        /// </summary>
        /// <param name="sequenceList">List of sequence.</param>
        /// <returns>Returns the list of sequence.</returns>
        private static IEnumerable<ISequence> ReverseComplementSequenceList(IEnumerable<ISequence> sequenceList)
        {
            Stopwatch stopWatchInterval = new Stopwatch();
            stopWatchInterval.Restart();
            foreach (ISequence seq in sequenceList)
            {
                stopWatchInterval.Stop();

                // Add the query sequence parse time.
                timeTakenToParseQuerySequences = timeTakenToParseQuerySequences.Add(stopWatchInterval.Elapsed);

                stopWatchInterval.Restart();
                ISequence seqReverseComplement = seq.GetReverseComplementedSequence();
                stopWatchInterval.Stop();

                // Add the reverse complement time.
                timeTakenToGetReverseComplement = timeTakenToGetReverseComplement.Add(stopWatchInterval.Elapsed);

                if (seqReverseComplement != null)
                {
                    seqReverseComplement.ID = seqReverseComplement.ID + " Reverse";
                }

                yield return seqReverseComplement;
            }

            // Stop watch if there are not query sequences left.
            stopWatchInterval.Stop();
        }

        /// <summary>
        /// Given a list of sequences, create a new list with the orginal sequence followed
        /// by the Reverse Complement of that sequence.
        /// </summary>
        /// <param name="sequenceList">List of sequence.</param>
        /// <returns>Returns the List of sequence.</returns>
        private static IEnumerable<ISequence> AddReverseComplementsToSequenceList(IEnumerable<ISequence> sequenceList)
        {
            Stopwatch stopWatchInterval = new Stopwatch();
            stopWatchInterval.Restart();
            foreach (ISequence seq in sequenceList)
            {
                stopWatchInterval.Stop();

                // Add the query sequence parse time.
                timeTakenToParseQuerySequences = timeTakenToParseQuerySequences.Add(stopWatchInterval.Elapsed);

                stopWatchInterval.Restart();
                ISequence seqReverseComplement = seq.GetReverseComplementedSequence();

                stopWatchInterval.Stop();

                // Add the reverse complement time.
                timeTakenToGetReverseComplement = timeTakenToGetReverseComplement.Add(stopWatchInterval.Elapsed);
                if (seqReverseComplement != null)
                {
                    seqReverseComplement.ID = seqReverseComplement.ID + " Reverse";
                }

                yield return seq;
                yield return seqReverseComplement;
            }

            // Stop watch if there are not query sequences left.
            stopWatchInterval.Stop();
        }

        /// <summary>
        /// Returns the cluster.
        /// </summary>
        /// <param name="referenceSequence">The Reference sequence.</param>
        /// <param name="querySequence">The Query sequence.</param>
        /// <returns>Returns list of clusters.</returns>
        private IList<List<IList<Cluster>>> GetCluster(IEnumerable<ISequence> referenceSequence, IEnumerable<ISequence> querySequence)
        {
            IList<List<IList<Cluster>>> clusters;
            if (this.MaxMatch)
            {
                clusters = referenceSequence.AsParallel().Select(sequence =>
                    {
                        NUCmer nucmer = new NUCmer();
                        nucmer.BreakLength = BreakLength;
                        nucmer.LengthOfMUM = MinMatch;
                        nucmer.MaximumSeparation = MaxGap;
                        nucmer.MinimumScore = MinCluster;
                        nucmer.SeparationFactor = (float)DiagFactor;
                        return nucmer.GetClusters(
                                new List<ISequence> { sequence }, GetSequence(querySequence), false);
                    }).ToList();
            }
            else
            {
                clusters = referenceSequence.AsParallel().Select(sequence =>
                {
                    NUCmer nucmer = new NUCmer();
                    nucmer.BreakLength = BreakLength;
                    nucmer.LengthOfMUM = MinMatch;
                    nucmer.MaximumSeparation = MaxGap;
                    nucmer.MinimumScore = MinCluster;
                    nucmer.SeparationFactor = (float)DiagFactor;
                    return nucmer.GetClusters(new List<ISequence> { sequence }, GetSequence(querySequence));
                }).ToList();
            }

            return clusters;
        }

        /// <summary>
        /// Gets the modified sequence.
        /// </summary>
        /// <param name="querySequence">The query sequence.</param>
        /// <returns>Returns a list of sequence.</returns>
        private IEnumerable<ISequence> GetSequence(IEnumerable<ISequence> querySequence)
        {
            if (this.Reverse)
            {
                return ReverseComplementSequenceList(querySequence);
            }
            else if (this.ForwardAndReverse)
            {
                return AddReverseComplementsToSequenceList(querySequence);
            }
            else
            {
                return querySequence;
            }
        }

        /// <summary>
        /// Gets the Delta for list of query sequences.
        /// </summary>
        /// <param name="referenceSequence">The reference sequence.</param>
        /// <param name="querySequence">The query sequence.</param>
        /// <returns>Returns list of IEnumerable Delta Alignment.</returns>
        private IList<List<IEnumerable<DeltaAlignment>>> GetDelta(IEnumerable<ISequence> referenceSequence, IEnumerable<ISequence> querySequence)
        {
            IList<List<IEnumerable<DeltaAlignment>>> delta;
            if (this.MaxMatch)
            {
                delta = referenceSequence.AsParallel().Select(sequence =>
                {
                    NUCmer nucmer = new NUCmer();
                    nucmer.BreakLength = BreakLength;
                    nucmer.LengthOfMUM = MinMatch;
                    nucmer.MaximumSeparation = MaxGap;
                    nucmer.MinimumScore = MinCluster;
                    nucmer.SeparationFactor = (float)DiagFactor;
                    return nucmer.GetDeltaAlignments(new List<ISequence> { sequence }, GetSequence(querySequence), false);
                }).ToList();
            }
            else
            {
                delta = referenceSequence.AsParallel().Select(sequence =>
                {
                    NUCmer nucmer = new NUCmer();
                    nucmer.BreakLength = BreakLength;
                    nucmer.LengthOfMUM = MinMatch;
                    nucmer.MaximumSeparation = MaxGap;
                    nucmer.MinimumScore = MinCluster;
                    nucmer.SeparationFactor = (float)DiagFactor;
                    return nucmer.GetDeltaAlignments(new List<ISequence> { sequence }, GetSequence(querySequence));
                }).ToList();
            }

            return delta;
        }

        /// <summary>
        /// Parses the file.
        /// </summary>
        /// <param name="fileName">The FileName.</param>
        /// <returns>List of sequence.</returns>
        private IEnumerable<ISequence> Parse(string fileName)
        {
            FastAParser parser = new FastAParser(fileName);
            return parser.Parse();
        }

        /// <summary>
        /// Writes cluster for query sequences.
        /// </summary>
        /// <param name="clusters">The Clusters.</param>
        /// <param name="referenceSequence">The reference sequence.</param>
        /// <param name="querySequence">The query sequence.</param>
        private void WriteCluster(
            IList<List<IList<Cluster>>> clusters,
            IEnumerable<ISequence> referenceSequence,
            IEnumerable<ISequence> querySequence)
        {
            TextWriter textWriterConsoleOutSave = Console.Out;
            if (!string.IsNullOrEmpty(this.OutputFile))
            {
                FileStream fileStreamConsoleOut = new FileStream(this.OutputFile, FileMode.Create);
                StreamWriter streamWriterConsoleOut = new StreamWriter(fileStreamConsoleOut);
                Console.SetOut(streamWriterConsoleOut);
                streamWriterConsoleOut.AutoFlush = true;
            }

            long refSequenceCount = referenceSequence.Count();
            long querySequenceCount = querySequence.Count();
            for (int i = 0; i < refSequenceCount; i++)
            {
                List<IList<Cluster>> queryCluster = clusters[i];
                for (int j = 0; j < querySequenceCount; j++)
                {
                    IEnumerable<Cluster> cluster = queryCluster[j];
                    if (cluster != null)
                    {
                        Console.WriteLine(referenceSequence.ElementAt(i).ID + " " + querySequence.ElementAt(j).ID);
                        foreach (Cluster c in cluster)
                        {
                            foreach (MatchExtension ext in c.Matches)
                            {
                                Console.WriteLine(ext.ReferenceSequenceOffset + " " + ext.QuerySequenceOffset + " " + ext.Length);
                            }
                        }
                    }
                }
            }

            Console.SetOut(textWriterConsoleOutSave);
        }

        /// <summary>
        /// Writes delta for query sequences.
        /// </summary>
        /// <param name="delta">The Deltas.</param>
        private void WriteDelta(
            IList<List<IEnumerable<DeltaAlignment>>> delta)
        {
            TextWriter textWriterConsoleOutSave = Console.Out;
            if (!string.IsNullOrEmpty(this.OutputFile))
            {
                FileStream fileStreamConsoleOut = new FileStream(this.OutputFile, FileMode.Create);
                StreamWriter streamWriterConsoleOut = new StreamWriter(fileStreamConsoleOut);
                Console.SetOut(streamWriterConsoleOut);
                streamWriterConsoleOut.AutoFlush = true;
            }

            foreach (var alignment in delta)
            {
                foreach (IEnumerable<DeltaAlignment> align in alignment)
                {
                    foreach (DeltaAlignment deltaAlignment in align)
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
                    Console.WriteLine("--");
                }
            }

            Console.SetOut(textWriterConsoleOutSave);
        }

        #endregion
    }
}
