// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommandLine;

using MBF;
using MBF.IO.Fasta;
using MBF.Algorithms.Alignment;

namespace MumUtil
{
    class MumUtil
    {
        class CommandLineOptions
        {
            //
            // Mummer commandline from summer interns
            //   mummer -maxmatch -b -c -s -n chromosome_1_1_1_1.fasta read_chrom1_2000.fasta > mummer2000-n.txt
            // translates to
            //   mumutil -maxmatch -b -c -s -n chromosome_1_1_1_1.fasta read_chrom1_2000.fasta > mummer2000-n.txt
            //
            [Argument(ArgumentType.AtMostOnce, HelpText = "Show the help information with program options and a description of program operation.")]
            public bool help;
            [Argument(ArgumentType.AtMostOnce, HelpText = "Display Verbose logging during processing")]
            public bool verbose;

            [Argument(ArgumentType.AtMostOnce, HelpText = "Compute all MUM candidates in reference [default]")]
            public bool mumreference;           //
            [Argument(ArgumentType.AtMostOnce, HelpText = "Compute Maximal Unique Matches, strings that are unique in both the reference and query sets")]
            public bool mum;                    // 
            [Argument(ArgumentType.AtMostOnce, HelpText = "Compute all maximal matches ignoring uniqueness")]
            public bool maxmatch;               // 

            [Argument(ArgumentType.AtMostOnce, HelpText = "Disallow ambiguity character matches and only match A, T, C, or G (case insensitive)")]
            public bool noAmbiguity;            // -n #
#if false
            [Argument(ArgumentType.AtMostOnce, HelpText = "Minimium match length [20]")]
            public int minMumLength;            // -l
#endif
            [Argument(ArgumentType.AtMostOnce, HelpText = "Compute forward and reverse complement matches")]
            public bool both;                   // -b
            [Argument(ArgumentType.AtMostOnce, HelpText = "Compute only reverse complement matches")]
            public bool reverseOnly;            // -r


            [Argument(ArgumentType.AtMostOnce, HelpText = "Show the length of the query sequence")]
            public bool displayQueryLength;     // -d

            [Argument(ArgumentType.AtMostOnce, HelpText = "Show the matching substring in the output")]
            public bool showMatchingString;     // -s

            [Argument(ArgumentType.AtMostOnce, HelpText = "Report the query position of a reverse complement match relative to the forward strand of the query sequence")]
            public bool c;             // -c
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
            [Argument(ArgumentType.AtMostOnce, HelpText = "Output file")]
            public string outputFile;

            [DefaultArgument(ArgumentType.MultipleUnique, HelpText = "Query file(s) containing the ")]
            public string[] fileList;

            public CommandLineOptions()
            {
                //  use assignments in the constructor to avoid the warning about unwritten variables
                help = false;
                verbose = false;
                mumreference = false;
                mum = false;
                maxmatch = false;
                noAmbiguity = false;
                both = false;
                reverseOnly = false;
                displayQueryLength = false;
                showMatchingString = false;
                c = false;
                outputFile = null;
                //referenceFilename = null;
                //queryFiles = null;
                fileList = null;
            }
        }
        static FileStream fsConsoleOut;
        static StreamWriter swConsoleOut;
        static TextWriter twConsoleOutSave;

        static CommandLineOptions ProcessCommandLine(string[] args)
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
            if ((myArgs.maxmatch && myArgs.mum)
                || (myArgs.maxmatch && myArgs.mumreference)
                || (myArgs.mum && myArgs.mumreference)
                )
            {
                Console.Error.WriteLine("\nError: only one of -maxmatch, -mum, -mumreference options can be specified.");
                Environment.Exit(-1);
            }
            if (!myArgs.mumreference && !myArgs.mum && !myArgs.maxmatch)
            {
                myArgs.mumreference = true;
            }
            if ((myArgs.fileList == null) || (myArgs.fileList.Length < 2))
            {
                Console.Error.WriteLine("\nError: A reference file and at least 1 query file are required.");
                Environment.Exit(-1);
            }
            if (myArgs.both && myArgs.reverseOnly)
            {
                Console.Error.WriteLine("\nError: only one of -both or -reverseOnly options can be specified.");
            }
            if (myArgs.c && (!myArgs.both && !myArgs.reverseOnly))
            {
                Console.Error.WriteLine("\nError: c requires one of either /b or /r options.");
                Environment.Exit(-1);
            }
            if (myArgs.outputFile != null)
            {   // redirect stdout
                twConsoleOutSave = Console.Out;
                fsConsoleOut = new FileStream(myArgs.outputFile, FileMode.Create);
                swConsoleOut = new StreamWriter(fsConsoleOut);
                Console.SetOut(swConsoleOut);
                swConsoleOut.AutoFlush = true;                
            }

            return (myArgs);
        }

        static string SplashString()
        {
            const string splashString = "\nMumUtil v1.0 - Maximal Unique Match Utility"
                                      + "\n  Copyright (c) Microsoft 2010 ";
            return (splashString);
        }


        //
        // Given a list of sequences, create a new list with only the Reverse Complements
        //   of the original sequences.
        static IList<ISequence> ReverseComplementSequenceList(IList<ISequence> sequenceList)
        {
            List<ISequence> updatedSequenceList = new List<ISequence>();
            foreach (ISequence seq in sequenceList)
            {

                ISequence seqReverseComplement = seq.ReverseComplement;
                //
                // DISCUSSION:
                //   Should there be an easily accessed indicator that this is a reversed sequence?
                //   And should we be able to get the 'base' version even if it is a sub-sequence?
                //
                BasicDerivedSequence derivedSeq = seqReverseComplement as BasicDerivedSequence;
                if (derivedSeq != null)
                {
                    derivedSeq.DisplayID = derivedSeq.DisplayID + " Reverse";
                    //                    seqReverseComplement.DisplayID = seqReverseComplement.DisplayID + " Reverse";
                }
                updatedSequenceList.Add(seqReverseComplement);
            }
            return (updatedSequenceList);

        }

        //
        // Given a list of sequences, create a new list with the orginal sequence followed
        // by the Reverse Complement of that sequence.
        static IList<ISequence> AddReverseComplementsToSequenceList(IList<ISequence> sequenceList)
        {
            List<ISequence> updatedSequenceList = new List<ISequence>();
            foreach (ISequence seq in sequenceList)
            {

                ISequence seqReverseComplement = seq.ReverseComplement;
                //
                // DISCUSSION:
                //   Should there be an easily accessed indicator that this is a reversed sequence?
                //
                BasicDerivedSequence derivedSeq = seqReverseComplement as BasicDerivedSequence;
                if (derivedSeq != null)
                {
                    derivedSeq.DisplayID = derivedSeq.DisplayID + " Reverse";
                    //                    seqReverseComplement.DisplayID = seqReverseComplement.DisplayID + " Reverse";
                }
                //seqReverseComplement.ID = seqReverseComplement.ID + " Reverse";
                updatedSequenceList.Add(seq);
                updatedSequenceList.Add(seqReverseComplement);
            }
            return (updatedSequenceList);
        }

        //
        // Given a set of MUMs, write the output to stdout with the appropriate options
        //   DISCUSSION:  
        //      We should consider a MumList class which relates two regions in 
        //      two sequences together rather than a IDictionary<ISeqeuence, IList<MaxUniqueMatch>>
        //
        private static void WriteMums(IDictionary<ISequence, IList<MaxUniqueMatch>> mums, CommandLineOptions myArgs)
        {
            foreach (KeyValuePair<ISequence, IList<MaxUniqueMatch>> kvp in mums)
            {
                // DISCUSSION:
                //  why deal with a KeyValuePair rather than a real MUMs object with appropriate methods?
                //  I have to know the Key.ID and what the KeyValuePair represents in programmer terms
                //  rather than biological terms.  Can creating a "MUMS" class can hide that complexity?
                //
                // write the QuerySequenceId
                string DisplayID = kvp.Key.DisplayID;
                Console.Write("> {0}", DisplayID);
                if (myArgs.displayQueryLength)
                {
                    Console.Write(" {1}", kvp.Key.Count);
                }
                Console.WriteLine();

                // DISCUSSION:
                //   If a ReverseComplement sequence, MUMmer has the option to print the index start point relative 
                //   to the original sequence we tagged the reversed DisplayID with " Reverse" so _if_ we find a 
                //   " Reverse" on the end of the ID, assume it is a ReverseComplement and reverse the index
                bool isReverseComplement = myArgs.c && DisplayID.EndsWith(" Reverse");

                // DISCUSSION:
                //   Why do I have to sort here?  Should MUMS come back sorted?
                IEnumerable<MaxUniqueMatch> sortedMums = kvp.Value.OrderBy(Mums => Mums.SecondSequenceStart);
                foreach (MaxUniqueMatch m in sortedMums)
                {
                    // Start is 1 based in literature but in programming (e.g. MaxUniqueMatch) they are 0 based.  
                    // Add 1
                    Console.WriteLine("{0,8}  {1,8}  {2,8}",
                            m.FirstSequenceStart + 1,
                            !isReverseComplement ? m.SecondSequenceStart + 1 : kvp.Key.Count - m.SecondSequenceStart,
                            m.Length);
                    if (myArgs.showMatchingString)
                    {
                        //Console.WriteLine(m.ToString());
                        //Console.Write("{0}", kvp.Key ):       // prints the whole seqeuence
                        for (int i = 0; i < m.Length; ++i)
                        {
                            // mummer uses all lowercase and MBF uses uppercase...convert on display
                            //Console.Write(kvp.Key[m.SecondSequenceStart + i].Symbol);
                            Console.Write(char.ToLowerInvariant(kvp.Key[m.SecondSequenceStart + i].Symbol));
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        static void ShowSequence(ISequence seq)
        {
            const int DefaultSequenceDisplayLength = 25;

            Console.Error.WriteLine("--- Sequence Dump ---");
            Console.Error.WriteLine("     Type: {0}", seq.GetType());
            Console.Error.WriteLine("       ID: {0}", seq.ID);
            Console.Error.WriteLine("    Count: {0}", seq.Count);
            Console.Error.WriteLine(" Alphabet: {0}", seq.Alphabet);
            int lengthToPrint = (seq.Count <= DefaultSequenceDisplayLength) ? seq.Count : DefaultSequenceDisplayLength;
            StringBuilder printString = new StringBuilder(lengthToPrint);
            for (int i = 0; i < lengthToPrint; ++i)
            {
                printString.Append(seq[i].Symbol);
            }
            Console.Error.WriteLine(" Sequence: {0}{1}", printString, lengthToPrint >= DefaultSequenceDisplayLength ? "..." : "");
            Console.Error.WriteLine();
        }

        static void Main(string[] args)
        {
//            DateTime dStart = DateTime.Now;
            Stopwatch swMumUtil = Stopwatch.StartNew();
            Stopwatch swInterval = new Stopwatch();

            Console.Error.WriteLine(SplashString());
            CommandLineOptions myArgs = ProcessCommandLine(args);
            myArgs.verbose = true;

            swInterval.Restart();
            IList<ISequence> referenceSequences = ParseFastA(myArgs.fileList[0]);
            swInterval.Stop();
            if (myArgs.verbose)
            {
                Console.Error.WriteLine();
                Console.Error.WriteLine("  Processed Reference FastA file: {0}", Path.GetFullPath(myArgs.fileList[0]));
                Console.Error.WriteLine("             Number of Sequences: {0}", referenceSequences.Count);
                Console.Error.WriteLine("        Length of first Sequence: {0:#,000}", referenceSequences[0].Count);
                Console.Error.WriteLine("            Read/Processing time: {0}", swInterval.Elapsed);
                // ShowSequence(referenceSequences[0]);
            }

            swInterval.Restart();
            IList<ISequence> querySequences = ParseFastA(myArgs.fileList[1]);
            swInterval.Stop();
            if (myArgs.verbose)
            {
                Console.Error.WriteLine();
                Console.Error.WriteLine("      Processed Query FastA file: {0}", Path.GetFullPath(myArgs.fileList[1]));
                Console.Error.WriteLine("             Number of Sequences: {0}", querySequences.Count);
                Console.Error.WriteLine("        Length of first Sequence: {0:#,000}", querySequences[0].Count);
                Console.Error.WriteLine("            Read/Processing time: {0}", swInterval.Elapsed);
                // ShowSequence(querySequences[0]);
            }

            if (myArgs.reverseOnly)
            {   // convert list to reverse complement sequences
                swInterval.Restart();
                querySequences = ReverseComplementSequenceList(querySequences);
                swInterval.Stop();
                if (myArgs.verbose) { Console.Error.WriteLine("         Reverse Complement time: {0}", swInterval.Elapsed); }
            }
            else if (myArgs.both)
            {   // add the reverse complement sequences to the query list too
                swInterval.Restart();
                querySequences = AddReverseComplementsToSequenceList(querySequences);
                swInterval.Stop();
                if (myArgs.verbose) { Console.Error.WriteLine("     Add Reverse Complement time: {0}", swInterval.Elapsed); }
            }

            // DISCUSSION:
            // why empty constructor here?  
            // Why not pass the reference / query info on construction?
            // ANSWER:
            //     That would break the "Constructors should not do a 'lot' of work" philosophy

            // DISCUSSION:
            // Why an IDictionary return?  Why not encapsulate MUMs into a class of its own?  
            //   Or perhaps a MumList
            //
            // DISCUSSION:
            // Three possible outputs desired.  Globally unique 'mum' (v1), unique in reference sequence (v2), 
            //   or get the maximum matches of length or greater.  
            //
            IDictionary<ISequence, IList<MaxUniqueMatch>> mums;
            MUMmer3 mum3 = new MUMmer3();
            if (myArgs.maxmatch)
            {
                // DISCUSSION:
                //   If there are a small number of configuration parameters
                //   it is frequently better to create a funtion to do the
                //   work e.g.
                //   mums = mum3.GetMumsMaxMatch( referenceSequences[0], querySequences );
                //
                //   If we have a large number of configuration parameters there are
                //   several styles to pass the information.  Do not pass a 'true' or
                //   'false' as a parameter.  It is frequently uncommented as to what
                //   the 'true' or 'false' means in that context of the call and leads
                //   to confusion.
                //   If many arguments are necessary to configure the call, seriously 
                //   consider a re-design.  It if MUST be, there are two prefered 
                //   ways to pass the configuration information in.   
                //     1.  If the same parameter values will be frequently re-used,
                //         then use a parameter structure and save it for use between
                //         calls.  
                //     2.  If the parameter values are local to this invocation and
                //         may change between calls, set the parameter values on
                //         the object you will be invoking.  Good 'defaults' during
                //         object construction and allow properties to update them.
                // Mummer3 mum3 = new Mummer3( ProcessWithMaxMum=true, ProcessWithAmbiguityDisallowed=true );
                //  or
                // mum3.ProcessWithMaxMum = true;
                // mum3.ProcessWithAmbiguityDisallowed = true;
                // mums = mum3.GetMums(referenceSequences[0], querySequences);

                // This is a placeholder stub for now!!!!!
                mum3.MaximumMatchEnabled = true;
                swInterval.Restart();
                mums = mum3.GetMUMs(referenceSequences[0], querySequences);
                swInterval.Stop();
                if (myArgs.verbose) { Console.Error.WriteLine("  Compute GetMumsMaxMatch() time: {0}", swInterval.Elapsed); }
            }
            else if (myArgs.mum)
            {
                // 
                // mums = mum3.GetMumsMum( referenceSequences[0], querySequences);
                swInterval.Restart();
                mums = mum3.GetMUMs(referenceSequences[0], querySequences);                 // 
                swInterval.Stop();
                if (myArgs.verbose) { Console.Error.WriteLine("       Compute GetMumsMum() time: {0}", swInterval.Elapsed); }
            }
            else if (myArgs.mumreference)
            {
                // NOTE:
                //     mum3.GetMUMs() this really implements the GetMumReference() functionality
                // mums = mum3.GetMumsReference( referenceSequences[0], querySequences);     // should be
                swInterval.Restart();
                mums = mum3.GetMUMs(referenceSequences[0], querySequences);
                swInterval.Stop();
                if (myArgs.verbose) { Console.Error.WriteLine(" Compute GetMumsReference() time: {0}", swInterval.Elapsed); }
            }
            else
            {
                // cannot happen as argument processing already asserted one of the three options must be specified
                Console.Error.WriteLine("\nError: one of /maxmatch, /mum, /mumreference options must be specified.");
                Environment.Exit(-1);
                // kill the error about unitialized use of 'mums' in the next block...the compiler does not recognize 
                //   Environment.Exit() as a no-return function
                throw new Exception("Never hit this");
            }

            swInterval.Restart();
            WriteMums(mums, myArgs);
            swInterval.Stop();
            if (myArgs.verbose) { Console.Error.WriteLine("                WriteMums() time: {0}", swInterval.Elapsed); }
            swMumUtil.Stop();
            if ( myArgs.verbose)
            {
                Console.Error.WriteLine("           Total MumUtil Runtime: {0}", swMumUtil.Elapsed);
            }
        }


#if true
        /// <summary>
        /// Parses a FastA file which has one or more sequences.
        /// </summary>
        /// <param name="filename">Path to the file to be parsed.</param>
        /// <returns>List of ISequence objects</returns>
        static IList<ISequence> ParseFastA(string filename)
        {
            //
            //  DISCUSSION:  [bd]
            //    Should this just be in the FASTA parser space?  If the model
            //    is to just load the file, it seems useful to not require 
            //    the user to type two lines when one will do...
            //

            // A new parser to import a file
            FastaParser parser = new FastaParser();
            return parser.Parse(filename);
        }
#endif
    }
}
