// *****************************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the Microsoft Public License.
//  THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//  ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//  IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//  PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MBF.Algorithms.Alignment;
using MBF.Algorithms.Alignment.MultipleSequenceAlignment;
using MBF.Algorithms.Assembly;
using MBF.Algorithms.Assembly.PaDeNA;
using MBF.IO;
using MBF.IO.Fasta;
using MBF.PerfTests.Util;
using MBF.SimilarityMatrices;
using NUnit.Framework;
using MBF.IO.SAM;
using MBF.IO.BAM;
using System.Reflection;
using System.Management;

namespace MBF.PerfTests
{
    /// <summary>
    /// Class for getting the Perf numbers for different algorithms.
    /// </summary>
    [TestFixture]
    public class PerfTests
    {
        #region Global Variables

        internal static PerformanceCounter _cpuCounterObj =
           new PerformanceCounter();
        internal static string _cpuCounterValue = "";
        internal static Stopwatch _watchObj = new Stopwatch();
        MUMmer3 mummerObj;
        SmithWatermanAligner swObj;
        NeedlemanWunschAligner nwObj;
        PAMSAMMultipleSequenceAligner msa;
        RunPaDeNA runPaDeNA;

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static PerfTests()
        {
            Utility._xmlUtil =
               new XmlUtility(@"TestUtils\PerfTestsConfig.xml");

            // Setting property value
            _cpuCounterObj.CategoryName = "Processor";
            _cpuCounterObj.CounterName = "% Processor Time";
            _cpuCounterObj.InstanceName = "_Total";
        }

        #endregion Constructor

        #region Perf Tests

        /// <summary>
        /// Gets Object Model perf and CPU utilization numbers.
        /// </summary>
        [Test]
        public void PerformObjectModelPerf()
        {
            string filePathObj =
              Utility._xmlUtil.GetTextValue(Constants.ObjectModelNodeName,
              Constants.FilePathNode);
            string seqCountToRead =
              Utility._xmlUtil.GetTextValue(Constants.ObjectModelNodeName,
              Constants.SequenceRangeToRead);

            Assert.IsNotNullOrEmpty(filePathObj);

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(filePathObj);

            FastaParser parserObj = new FastaParser();
            parserObj.EnforceDataVirtualization = true;

            IList<ISequence> seqListObj = parserObj.Parse(filePathObj, false);

            SequencePointer pointerObj = new SequencePointer();
            pointerObj.AlphabetName =
              Utility._xmlUtil.GetTextValue(Constants.ObjectModelNodeName,
              Constants.AlphabetNode);
            pointerObj.Id =
              Utility._xmlUtil.GetTextValue(Constants.ObjectModelNodeName,
              Constants.SequenceIDNode);
            pointerObj.IndexOffsets[0] = int.Parse(
              Utility._xmlUtil.GetTextValue(Constants.ObjectModelNodeName,
              Constants.StartIndexNode));
            pointerObj.IndexOffsets[1] = int.Parse(
              Utility._xmlUtil.GetTextValue(Constants.ObjectModelNodeName,
              Constants.EndIndexNode));
            pointerObj.StartingLine = int.Parse(
              Utility._xmlUtil.GetTextValue(Constants.ObjectModelNodeName,
              Constants.StartLineNode));

            GetBlockPerfNumber(parserObj, pointerObj, seqListObj[0],
                seqCountToRead);
            GetSequencePerfNumber(seqListObj[0]);
        }

        /// <summary>
        /// Gets MUMmer perf and CPU utilization numbers.
        /// </summary>
        [Test]
        public void PerformMUMmerPerf()
        {
            Stopwatch _watchObj = new Stopwatch();
            IList<IPairwiseSequenceAlignment> alignment = null;

            // Get Sequence file path.
            string refPath =
              Utility._xmlUtil.GetTextValue(Constants.MUMmerNodeName,
              Constants.RefFilePathNode);
            string queryPath =
              Utility._xmlUtil.GetTextValue(Constants.MUMmerNodeName,
              Constants.QueryFilePathNode);
            string smFilePath =
              Utility._xmlUtil.GetTextValue(Constants.MUMmerNodeName,
              Constants.SMFilePathNode);

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(refPath);
            lstInputFiles.Add(queryPath);

            FastaParser parserObj = new FastaParser();
            IList<ISequence> seqs1 = parserObj.Parse(refPath);

            parserObj = new FastaParser();
            IList<ISequence> seqs2 = parserObj.Parse(queryPath);

            IAlphabet alphabet = Alphabets.DNA;
            ISequence originalSequence1 = seqs1[0];
            ISequence originalSequence2 = seqs2[0];

            ISequence aInput = new Sequence(alphabet, originalSequence1.ToString());
            ISequence bInput = new Sequence(alphabet, originalSequence2.ToString());

            SimilarityMatrix sm = new SimilarityMatrix(smFilePath);
            mummerObj = new MUMmer3();
            mummerObj.GapOpenCost = -10;
            mummerObj.GapExtensionCost = -10;
            mummerObj.SimilarityMatrix = sm;
            mummerObj.LengthOfMUM = Int32.Parse(
              Utility._xmlUtil.GetTextValue(Constants.MUMmerNodeName,
              Constants.MUMLengthNode));

            _watchObj.Reset();
            _watchObj.Start();
            long memoryStart = GC.GetTotalMemory(false);
            // Align sequences using MUMmer.

            alignment = mummerObj.AlignSimple(aInput, seqs2);

            _watchObj.Stop();
            long memoryEnd = GC.GetTotalMemory(false);

            string memoryUsed = (memoryEnd - memoryStart).ToString();

            // Display MUMmer perf test case execution details.
            DisplayTestCaseHeader(lstInputFiles, _watchObj, memoryUsed,
                "MUMmer");

            Console.WriteLine(string.Format(
              "MUMmer AlignSimple() method, Alignment Score is : {0}",
              alignment[0].PairwiseAlignedSequences[0].Score.ToString()));

            // Dispose MUMmer object.
            mummerObj = null;
        }

        /// <summary>
        /// Gets PaDeNa perf and memory nos.
        /// </summary>
        [Test]
        public void PerformPaDeNAPerf()
        {
            // Get a query file path
            string filePathObj =
              Utility._xmlUtil.GetTextValue(Constants.PaDeNANodeName,
              Constants.FilePathNode);

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(filePathObj);

            runPaDeNA = new RunPaDeNA();
            int ScaffoldScore = runPaDeNA.RunPerf(filePathObj);

            // Display PaDeNA perf test case execution details.
            DisplayTestCaseHeader(lstInputFiles, _watchObj, runPaDeNA.MemoryUsed,
                "PaDeNA");

            Console.WriteLine(string.Format("Scaffold Score is : {0}",
              ScaffoldScore.ToString()));
        }

        /// <summary>
        /// Gets PAMSAM perf and memory nos.
        /// </summary>
        [Test]
        public void PerformPAMSAMPerf()
        {
            Stopwatch _watchObj = new Stopwatch();

            // Get input values from XML.
            string refPath =
                Utility._xmlUtil.GetTextValue(Constants.PamsamNode,
                Constants.RefFilePathNode);
            string queryPath =
                Utility._xmlUtil.GetTextValue(Constants.PamsamNode,
                Constants.QueryFilePathNode);

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(refPath);
            lstInputFiles.Add(queryPath);

            // Parse a Reference and query sequence file.
            ISequenceParser parser = new FastaParser();
            IList<ISequence> refsequences = parser.Parse(queryPath);
            IList<ISequence> orgSequences = parser.Parse(refPath);

            // Execute UnAlign method to verify that it does not contains gap
            List<ISequence> sequences = MsaUtils.UnAlign(orgSequences);

            // Set static properties
            PAMSAMMultipleSequenceAligner.FasterVersion = true;
            PAMSAMMultipleSequenceAligner.UseWeights = false;
            PAMSAMMultipleSequenceAligner.UseStageB = false;
            PAMSAMMultipleSequenceAligner.NumberOfCores = 2;

            // Set Alignment parameters.
            int gapOpenPenalty = -13;
            int gapExtendPenalty = -5;
            int kmerLength = 2;
            int numberOfDegrees = 2;
            int numberOfPartitions = 4;

            // Profile Distance function name
            DistanceFunctionTypes distanceFunctionName =
                DistanceFunctionTypes.EuclideanDistance;

            // Set Hierarchical clustering.
            UpdateDistanceMethodsTypes hierarchicalClusteringMethodName =
                UpdateDistanceMethodsTypes.Average;

            // Set NeedlemanWunschProfileAligner 
            ProfileAlignerNames profileAlignerName =
                ProfileAlignerNames.NeedlemanWunschProfileAligner;
            ProfileScoreFunctionNames profileProfileFunctionName =
                ProfileScoreFunctionNames.InnerProduct;

            // Create similarity matrix instance.
            SimilarityMatrix similarityMatrix =
                new SimilarityMatrix(SimilarityMatrix.StandardSimilarityMatrix.AmbiguousDna);

            // Reset stop watch and start timer.
            _watchObj.Reset();
            _watchObj.Start();
            long memoryStart = GC.GetTotalMemory(true);

            // Parallel Option will only get set if the PAMSAMMultipleSequenceAligner is getting called
            // To test separately distance matrix, binary tree etc.. 
            // Set the parallel option using below ctor.
            msa = new PAMSAMMultipleSequenceAligner
               (sequences, MoleculeType.DNA, kmerLength, distanceFunctionName,
               hierarchicalClusteringMethodName, profileAlignerName,
               profileProfileFunctionName, similarityMatrix, gapOpenPenalty,
               gapExtendPenalty, numberOfPartitions, numberOfDegrees);

            // Stop watchclock.
            _watchObj.Stop();
            long memoryEnd = GC.GetTotalMemory(true);

            string memoryUsed = (memoryEnd - memoryStart).ToString();

            // Display all aligned sequence, performance and memory optimization nos.
            DisplayTestCaseHeader(lstInputFiles, _watchObj,
                memoryUsed, "PAMSAM");

            Console.WriteLine(string.Format(
              "PAMSAM SequenceAligner method, Alignment Score is : {0}",
               msa.AlignmentScore.ToString()));
            int index = 0;
            foreach (ISequence seq in msa.AlignedSequences)
            {
                Console.WriteLine(string.Format(
                    "PAMSAM Aligned Seq {0}:{1}", index, seq.ToString()));
                index++;
            }
        }

        /// <summary>
        /// Gets Smith Waterman perf and CPU utilization numbers.
        /// </summary>
        [Test]
        public void PerformSmithWatermanPerf()
        {
            // Get Sequence file path.
            string refPath =
              Utility._xmlUtil.GetTextValue(Constants.AlignmentAlgorithmNodeName,
              Constants.RefFilePathNode);
            string queryPath =
              Utility._xmlUtil.GetTextValue(Constants.AlignmentAlgorithmNodeName,
              Constants.QueryFilePathNode);
            string smFilePath =
              Utility._xmlUtil.GetTextValue(Constants.AlignmentAlgorithmNodeName,
              Constants.SMFilePathNode);

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(refPath);
            lstInputFiles.Add(queryPath);

            FastaParser parserObj = new FastaParser();
            IList<ISequence> seqs1 = parserObj.Parse(refPath);

            parserObj = new FastaParser();
            IList<ISequence> seqs2 = parserObj.Parse(queryPath);

            IAlphabet alphabet = Alphabets.DNA;
            ISequence originalSequence1 = seqs1[0];
            ISequence originalSequence2 = seqs2[0];

            ISequence aInput = new Sequence(alphabet, originalSequence1.ToString());
            ISequence bInput = new Sequence(alphabet, originalSequence2.ToString());

            SimilarityMatrix sm = new SimilarityMatrix(smFilePath);
            swObj = new SmithWatermanAligner();
            swObj.GapOpenCost = -10;
            swObj.GapExtensionCost = -10;
            swObj.SimilarityMatrix = sm;
            _watchObj = new Stopwatch();

            _watchObj.Reset();
            _watchObj.Start();
            long memoryStart = GC.GetTotalMemory(false);

            // Align sequences using smith water man algorithm.
            IList<IPairwiseSequenceAlignment> alignment = swObj.AlignSimple(aInput, bInput);

            _watchObj.Stop();
            long memoryEnd = GC.GetTotalMemory(false);

            string memoryUsed = (memoryEnd - memoryStart).ToString();

            // Display SmithWaterman perf test case execution details.
            DisplayTestCaseHeader(lstInputFiles, _watchObj, memoryUsed,
                "SmithWaterman");

            Console.WriteLine(string.Format(
              "Smith Waterman AlignSimple() method, Alignment Score is : {0}",
              alignment[0].PairwiseAlignedSequences[0].Score.ToString()));

            // Dispose SmithWaterman object
            swObj = null;

        }

        /// <summary>
        /// Gets Needleman Wunsch perf and CPU utilization numbers.
        /// </summary>
        [Test]
        public void PerformNeedlemanWunschPerf()
        {
            // Get Sequence file path.
            string refPath =
              Utility._xmlUtil.GetTextValue(Constants.AlignmentAlgorithmNodeName,
              Constants.RefFilePathNode);
            string queryPath =
              Utility._xmlUtil.GetTextValue(Constants.AlignmentAlgorithmNodeName,
              Constants.QueryFilePathNode);
            string smFilePath =
              Utility._xmlUtil.GetTextValue(Constants.AlignmentAlgorithmNodeName,
              Constants.SMFilePathNode);

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(refPath);
            lstInputFiles.Add(queryPath);

            FastaParser parserObj = new FastaParser();
            IList<ISequence> seqs1 = parserObj.Parse(refPath);

            parserObj = new FastaParser();
            IList<ISequence> seqs2 = parserObj.Parse(queryPath);

            IAlphabet alphabet = Alphabets.DNA;
            ISequence originalSequence1 = seqs1[0];
            ISequence originalSequence2 = seqs2[0];

            ISequence aInput = new Sequence(alphabet, originalSequence1.ToString());
            ISequence bInput = new Sequence(alphabet, originalSequence2.ToString());

            SimilarityMatrix sm = new SimilarityMatrix(smFilePath);
            nwObj = new NeedlemanWunschAligner();
            nwObj.GapOpenCost = -10;
            nwObj.GapExtensionCost = -10;
            nwObj.SimilarityMatrix = sm;

            _watchObj = new Stopwatch();
            _watchObj.Reset();
            _watchObj.Start();
            long memoryStart = GC.GetTotalMemory(false);

            // Align sequences using smith water man algorithm.
            IList<IPairwiseSequenceAlignment> alignment = nwObj.AlignSimple(aInput, bInput);

            _watchObj.Stop();
            long memoryEnd = GC.GetTotalMemory(false);

            string memoryUsed = (memoryEnd - memoryStart).ToString();

            // Display Needlemanwunsch perf test case execution details.
            DisplayTestCaseHeader(lstInputFiles, _watchObj, memoryUsed,
                "NeedlemanWunsch");

            Console.WriteLine(string.Format(
                "Needleman Wunsch AlignSimple() method, Alignment Score is : {0}",
                alignment[0].PairwiseAlignedSequences[0].Score.ToString()));

            // Dispose NeedlemanWunsch object
            nwObj = null;
        }

        /// <summary>
        /// Gets BAM Parser perf numbers.
        /// </summary>
        [Test]
        public void PerformBAMParserPerf()
        {
            // Get BAM file path.
            string bamFilePath =
                Utility._xmlUtil.GetTextValue(Constants.BAMParserNode,
                Constants.FilePathNode);

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(bamFilePath);

            // Create a BAM Parser object
            BAMParser parseBam = new BAMParser();

            // Enable DV.
            parseBam.EnforceDataVirtualization = true;

            _watchObj = new Stopwatch();
            _watchObj.Reset();
            _watchObj.Start();
            long memoryStart = GC.GetTotalMemory(false);
            SequenceAlignmentMap align = parseBam.Parse(bamFilePath);

            _watchObj.Stop();
            long memoryEnd = GC.GetTotalMemory(false);

            string memoryUsed = (memoryEnd - memoryStart).ToString();

            // Display BAMParser perf test case execution details.
            DisplayTestCaseHeader(lstInputFiles, _watchObj, memoryUsed,
               null);
        }

        /// <summary>
        /// Gets BAM Parse and display sequence alignments perf numbers.
        /// </summary>
        [Test]
        public void PerformBAMParseAndDisplayAlignmentsPerf()
        {
            // Get BAM file path.
            string bamFilePath =
                Utility._xmlUtil.GetTextValue(Constants.BAMParserNode,
                Constants.FilePathNode);

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(bamFilePath);

            // Create a BAM Parser object
            BAMParser parseBam = new BAMParser();

            // Enable DV.
            parseBam.EnforceDataVirtualization = true;

            _watchObj = new Stopwatch();
            _watchObj.Reset();
            _watchObj.Start();
            long memoryStart = GC.GetTotalMemory(false);
            SequenceAlignmentMap align = parseBam.Parse(bamFilePath);

            // Display the header part 
            DisplayHeader(align);

            // Display Sequence alignments
            DisplaySeqAlignments(align);

            _watchObj.Stop();
            long memoryEnd = GC.GetTotalMemory(false);

            string memoryUsed = (memoryEnd - memoryStart).ToString();

            // Display BAMParser perf test case execution details.
            DisplayTestCaseHeader(lstInputFiles, _watchObj, memoryUsed,
               null);
        }

        #endregion Perf Tests

        #region Helper Methods

        /// <summary>
        /// Get perf nos of each block
        /// </summary>
        /// <param name="parserObj">Fasta Parser object</param>
        /// <param name="pointerObj">Seq pointer</param>
        /// <param name="seq">Isequence</param>
        private void GetBlockPerfNumber(FastaParser parserObj,
           SequencePointer pointerObj,
           ISequence seq, string seqCountToRead)
        {
            // Calculating First Block Time and CPU Utilization
            _watchObj.Reset();
            _watchObj.Start();

            ISequence firstBlock = parserObj.ParseRange(0, Int32.Parse(seqCountToRead)
                , pointerObj);
            _watchObj.Stop();

            Console.WriteLine(string.Format("FirstBlock Perf Time : {0} Secs",
              TimeSpan.FromMilliseconds(
              _watchObj.ElapsedMilliseconds).TotalSeconds.ToString()));
            Console.WriteLine(string.Format("FirstBlock CPU Utilization : {0}",
              _cpuCounterObj.NextValue().ToString()));

            // Calculating Middle Block Time and CPU Utilization      
            _watchObj.Reset();
            _watchObj.Start();

            ISequence middleBlock =
              parserObj.ParseRange((seq.Count / 2),
             Int32.Parse(seqCountToRead), pointerObj);
            _watchObj.Stop();
            Console.WriteLine(string.Format("MiddleBlock Perf Time : {0} Secs",
              TimeSpan.FromMilliseconds(
              _watchObj.ElapsedMilliseconds).TotalSeconds.ToString()));
            Console.WriteLine(string.Format("MiddleBlock CPU Utilization : {0}",
              _cpuCounterObj.NextValue().ToString()));

            // Calculating Last Block Time and CPU Utilization
            _watchObj.Reset();
            _watchObj.Start();
            ISequence lastBlock =
              parserObj.ParseRange(seq.Count - Int32.Parse(seqCountToRead),
              Int32.Parse(seqCountToRead), pointerObj);
            _watchObj.Stop();

            Console.WriteLine(string.Format("LastBlock Perf Time : {0} Secs",
              TimeSpan.FromMilliseconds(
              _watchObj.ElapsedMilliseconds).TotalSeconds.ToString()));
            Console.WriteLine(string.Format("LastBlock CPU Utilization : {0}",
              _cpuCounterObj.NextValue().ToString()));
        }

        /// <summary>
        /// Get perf, memory and cpu utilization nos for sequence operations
        /// Insert,Seq ctor,IndexOf,Add,Clone,Remove.
        /// </summary>
        /// <param name="sequence">Sequence string</param>
        private void GetSequencePerfNumber(ISequence sequence)
        {
            // Calculating Constructor Time and CPU utilization
            Stopwatch _watchObj = new Stopwatch();
            FastaParser parserObj = new FastaParser();

            _watchObj.Reset();
            _watchObj.Start();
            Sequence seq = new Sequence(
              Alphabets.DNA,
              sequence.ToString());
            _watchObj.Stop();
            Console.WriteLine(string.Format("Constructor() method Perf Time : {0} Secs",
              TimeSpan.FromMilliseconds(
              _watchObj.ElapsedMilliseconds).TotalSeconds.ToString()));
            Console.WriteLine(string.Format("Constructor() method CPU Utilization : {0}",
              _cpuCounterObj.NextValue().ToString()));

            seq.IsReadOnly = false;

            // Calculating Insert method Time and CPU utilization
            _watchObj.Reset();
            _watchObj.Start();
            foreach (ISequenceItem item in sequence)
            {
                seq.Insert(1, item);
            }
            _watchObj.Stop();
            Console.WriteLine(string.Format("Insert() method Perf Time : {0} Secs",
              TimeSpan.FromMilliseconds(
              _watchObj.ElapsedMilliseconds).TotalSeconds.ToString()));
            Console.WriteLine(string.Format("Insert() method CPU Utilization : {0}",
              _cpuCounterObj.NextValue().ToString()));

            // Calculating IndexOf method Time and CPU utilization
            _watchObj.Reset();
            _watchObj.Start();
            foreach (ISequenceItem item in sequence)
            {
                seq.IndexOf(item);
            }
            _watchObj.Stop();
            Console.WriteLine(string.Format("IndexOf() method Perf Time : {0} Secs",
              TimeSpan.FromMilliseconds(
              _watchObj.ElapsedMilliseconds).TotalSeconds.ToString()));
            Console.WriteLine(string.Format("IndexOf() method CPU Utilization : {0}",
              _cpuCounterObj.NextValue().ToString()));

            // Calculating Add method Time and CPU utilization
            _watchObj.Reset();
            _watchObj.Start();
            foreach (ISequenceItem item in sequence)
            {
                seq.Add(item);
            }
            _watchObj.Stop();
            Console.WriteLine(string.Format("Add() method Perf Time : {0} Secs",
               TimeSpan.FromMilliseconds(
               _watchObj.ElapsedMilliseconds).TotalSeconds.ToString()));
            Console.WriteLine(string.Format("Add() method CPU Utilization : {0}",
              _cpuCounterObj.NextValue().ToString()));

            // Calculating Clone method Time and CPU utilization
            _watchObj.Reset();
            _watchObj.Start();
            Sequence seqClone = seq.Clone();
            _watchObj.Stop();
            Console.WriteLine(string.Format("Clone() method Perf Time : {0} Secs",
              TimeSpan.FromMilliseconds(
              _watchObj.ElapsedMilliseconds).TotalSeconds.ToString()));
            Console.WriteLine(string.Format("Clone() method CPU Utilization : {0}",
              _cpuCounterObj.NextValue().ToString()));

            // Calculating Remove method Time and CPU utilization
            _watchObj.Reset();
            _watchObj.Start();
            foreach (ISequenceItem item in sequence)
            {
                seq.Remove(item);
            }
            _watchObj.Stop();
            Console.WriteLine(string.Format("Remove() method Perf Time : {0} Secs",
               TimeSpan.FromMilliseconds(
               _watchObj.ElapsedMilliseconds).TotalSeconds.ToString()));
            Console.WriteLine(string.Format("Remove() method CPU Utilization : {0}",
              _cpuCounterObj.NextValue().ToString()));

        }

        /// <summary>
        /// Dispalys the headers present in the BAM file
        /// </summary>
        /// <param name="seqAlignmentMap">SeqAlignment map</param>
        private void DisplayHeader(SequenceAlignmentMap seqAlignmentMap)
        {
            // Get Header
            SAMAlignmentHeader header = seqAlignmentMap.Header;
            IList<SAMRecordField> recordField = header.RecordFields;
            IList<string> commenstList = header.Comments;

            if (recordField.Count > 0)
            {
                Console.WriteLine("MetaData:");

                // Read Header Lines
                for (int i = 0; i < recordField.Count; i++)
                {
                    Console.Write("\n@{0}", recordField[i].Typecode);
                    for (int tags = 0; tags < recordField[i].Tags.Count; tags++)
                    {
                        Console.Write("\t{0}:{1}", recordField[i].Tags[tags].Tag,
                            recordField[i].Tags[tags].Value);
                    }
                }
            }

            // Displays the comments if any
            if (commenstList.Count > 0)
            {
                for (int i = 0; i < commenstList.Count; i++)
                {
                    Console.Write("\n@CO\t{0}\n", commenstList[i].ToString());
                }
            }
        }

        /// <summary>
        /// Displays the Aligned sequence
        /// </summary>
        /// <param name="seqAlignment">SeqAlignmentMap object</param>
        private void DisplaySeqAlignments(SequenceAlignmentMap seqAlignment)
        {
            // Get Aligned sequences
            IList<SAMAlignedSequence> alignedSeqs = seqAlignment.QuerySequences;
            try
            {
                for (int i = 0; i < alignedSeqs.Count; i++)
                {
                    //Console.WriteLine();
                    string seq = "*";
                    if (alignedSeqs[i].QuerySequence.Count > 0)
                    {
                        seq = alignedSeqs[i].QuerySequence.ToString();
                    }

                    string qualValues = "*";

                    QualitativeSequence qualSeq = alignedSeqs[i].QuerySequence as QualitativeSequence;
                    if (qualSeq != null)
                    {
                        byte[] bytes = qualSeq.Scores;
                        qualValues = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
                    }

                    Console.Write("\n{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}",
                        alignedSeqs[i].QName, (int)alignedSeqs[i].Flag, alignedSeqs[i].RName,
                        alignedSeqs[i].Pos, alignedSeqs[i].MapQ, alignedSeqs[i].CIGAR,
                        alignedSeqs[i].MRNM.Equals(alignedSeqs[i].RName) ? "=" : alignedSeqs[i].MRNM,
                        alignedSeqs[i].MPos, alignedSeqs[i].ISize, seq, qualValues);

                    for (int j = 0; j < alignedSeqs[i].OptionalFields.Count; j++)
                    {
                        Console.Write("\t{0}:{1}:{2}", alignedSeqs[i].OptionalFields[j].Tag,
                            alignedSeqs[i].OptionalFields[j].VType, alignedSeqs[i].OptionalFields[j].Value);
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Display header information of the test case being executed.
        /// </summary>
        /// <param name="ipnutFiles">List of Input files</param>
        /// <param name="stopWatchObj">Stop watch object</param>
        /// <param name="memoryUsage">memory used to execute algorithm</param>
        /// <param name="algorithm">name of the algorithm being executed</param>
        private void DisplayTestCaseHeader(List<string> ipnutFiles,
            Stopwatch stopWatchObj, string memoryUsage, string algorithm)
        {
            // Get operating system.
            string osName = GetOSName();

            // Get Number of processors.
            string processorNo = GetProcessors();

            // Get Total memory of the System
            double memoryInBytes = GetPhysicalMemory();

            // Get CPU Speed.
            object cpuSpeed = GetProcessorSpeed();

            // Get the test method name being executed.
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            MethodBase methodBase = stackFrame.GetMethod();

            Console.WriteLine("Test Method: {0}", methodBase.Name);

            // Display Input sequence file information.
            int i = 1;
            Console.WriteLine("Input Files:");
            foreach (string file in ipnutFiles)
            {
                FileInfo ipnutFileInfo = new FileInfo(file);
                Console.WriteLine("IpnutFile{0} -\tFile-name : {1}, File-size :{2} bytes",
                    i, ipnutFileInfo.Name, ipnutFileInfo.Length.ToString());
                i++;
            }

            // Display Test input parameters used for different features.
            switch (algorithm)
            {
                case "MUMmer":
                    Console.WriteLine("Input Parameters Used : \n\tGapOpenCost:{0},GapExtensionCost:{1},MUMLength:{2},SimilarityMatrix:{3}",
                     mummerObj.GapOpenCost, mummerObj.GapExtensionCost, mummerObj.LengthOfMUM.ToString(), mummerObj.SimilarityMatrix.Name);
                    break;
                case "PAMSAM":
                    Console.WriteLine("Input Parameters Used : \n\tGapOpenPenalty:{0},GapExtendPenalty:{1}",
                    msa.GapOpenCost, msa.GapExtensionCost);
                    break;
                case "SmithWaterman":
                    Console.WriteLine("Input Parameters Used : \n\tGapOpenCost:{0},GapExtensionCost:{1},SimilarityMatrix:{2}",
                    swObj.GapOpenCost, swObj.GapExtensionCost, swObj.SimilarityMatrix.Name);
                    break;
                case "NeedlemanWunsch":
                    Console.WriteLine("Input Parameters Used : \n\tGapOpenCost:{0},GapExtensionCost:{1},SimilarityMatrix:{2}",
                        nwObj.GapOpenCost, nwObj.GapExtensionCost, nwObj.SimilarityMatrix.Name);
                    break;
                case "PaDeNA":
                    Console.WriteLine("Input Parameters Used : \n\tKmerLength:{0},Dangling threshold:{1},Redundant threshold:{2}",
                       runPaDeNA.kLength, runPaDeNA.dThreshold, runPaDeNA.rThreshold);
                    break;
            }

            Console.WriteLine("Platform Used : ");
            Console.WriteLine("\tOperating System :{0}\n\tProcessor cores:{1}\n\tPhysical memory :{2}\n\tCPU Speed:{3}",
                osName, processorNo, memoryInBytes, cpuSpeed);
            Console.WriteLine("Time taken to execute {0} in secs: {1}", methodBase.Name,
                TimeSpan.FromMilliseconds(
                stopWatchObj.ElapsedMilliseconds).TotalSeconds.ToString());
            Console.WriteLine("Memory used in bytes : {0}", memoryUsage);
        }

        /// <summary>
        /// Get Name of the Operating system being used.
        /// </summary>
        /// <returns>Name of the OS</returns>
        private string GetOSName()
        {
            OperatingSystem os = System.Environment.OSVersion;
            string osName = "UnKnown";
            if (Constants.WinMajorVersion == os.Version.Major)
            {
                if (Constants.WinXPMinorVersion == os.Version.Minor)
                    osName = "Windows XP";
                else
                    if (Constants.Win2K3MinorVersion == os.Version.Minor)
                        osName = "Windows 2003";
                    else
                        if (Constants.Win2KMinorVersion == os.Version.Minor)
                            osName = "Windows 2000";
            }
            else if (Constants.WinNTMajorVersion == os.Version.Major)
            {
                if (Constants.VistaMinorVersion == os.Version.Minor)
                    osName = "Windows Vista";
                else
                    if (Constants.Win7MinorVersion == os.Version.Minor)
                        osName = "Windows 7";
                    else
                        if (Constants.Win2K8MinorVersion == os.Version.Minor)
                            osName = "Windows 2008";
            }

            return osName;
        }

        /// <summary>
        /// Get no of proceesors being used.
        /// </summary>
        /// <returns>No of processors</returns>
        private string GetProcessors()
        {
            string systemCoreNos = string.Empty;
            ObjectQuery objectQuery = new
                 ObjectQuery("select * from Win32_Processor");
            ManagementObjectSearcher managementObj =
                new ManagementObjectSearcher(objectQuery);
            ManagementObjectCollection managemnetObjCollection =
                managementObj.Get();

            foreach (ManagementObject item in managemnetObjCollection)
            {
                systemCoreNos +=
                    System.Convert.ToDouble(item.GetPropertyValue("NumberOfCores"));
            }

            return systemCoreNos;
        }

        /// <summary>
        /// Gets physical memory of the machine.
        /// </summary>
        /// <returns>Physical memory in bytes</returns>
        private double GetPhysicalMemory()
        {
            double totalMemoryInBytes = 0;
            ObjectQuery objectQuery = new
                ObjectQuery("select * from Win32_PhysicalMemory");
            ManagementObjectSearcher managementObj =
                new ManagementObjectSearcher(objectQuery);
            ManagementObjectCollection managemnetObjCollection =
                managementObj.Get();

            foreach (ManagementObject item in managemnetObjCollection)
            {
                totalMemoryInBytes +=
                    System.Convert.ToDouble(item.GetPropertyValue("Capacity"));
            }

            return totalMemoryInBytes;
        }

        /// <summary>
        /// Gets Processor speed of the machine.
        /// </summary>
        /// <returns>Processor speed</returns>
        private object GetProcessorSpeed()
        {
            ManagementObject managementObject = new
                ManagementObject("Win32_Processor.DeviceID='CPU0'");
            object cpuSpeed = managementObject["CurrentClockSpeed"];
            return cpuSpeed;
        }

        #endregion Helper Methods
    }

    /// <summary>
    /// PaDeNA class 
    /// </summary>
    class RunPaDeNA : ParallelDeNovoAssembler
    {
        #region Properties

        /// <summary>
        /// Memory utilization
        /// </summary>
        public string MemoryUsed
        {
            get;
            set;
        }

        /// <summary>
        /// Kmer Length
        /// </summary>
        public int kLength
        {
            get;
            set;
        }

        /// <summary>
        /// Dangling Threshold
        /// </summary>
        public int dThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Redundant Threshold
        /// </summary>
        public int rThreshold
        {
            get;
            set;
        }

        #endregion Properties

        /// <summary>
        /// Runs PaDeNA and get the performance numbers
        /// </summary>
        /// <param name="QueryFilePath">Query file path</param>
        /// <returns>Scaffold count</returns>
        internal int RunPerf(string QueryFilePath)
        {
            ParallelDeNovoAssembler parallel = new ParallelDeNovoAssembler();
            kLength = 20;
            rThreshold = 2 * (kLength + 1);
            dThreshold = kLength;
            parallel.KmerLength = kLength;
            parallel.DanglingLinksThreshold = dThreshold;
            parallel.RedundantPathLengthThreshold = rThreshold;

            List<ISequence> sequences = new List<ISequence>();
            using (StreamReader read = new StreamReader(QueryFilePath))
            {
                string Id = read.ReadLine();
                string seq = read.ReadLine();
                while (!string.IsNullOrEmpty(seq))
                {
                    Sequence sequence = new Sequence(Alphabets.DNA, seq);
                    sequence.DisplayID = Id;
                    sequences.Add(sequence);
                    Id = read.ReadLine();
                    seq = read.ReadLine();
                }
            }

            CloneLibrary.Instance.AddLibrary("abc", (float)1000, (float)500);
            long memoryStart = GC.GetTotalMemory(false);
            PerfTests._watchObj.Reset();
            PerfTests._watchObj.Start();


            IDeNovoAssembly assembly = parallel.Assemble(sequences, true);
            long memoryEnd = GC.GetTotalMemory(false);
            PerfTests._watchObj.Stop();

            MemoryUsed = (memoryEnd - memoryStart).ToString();
            return assembly.AssembledSequences.Count;
        }
    }
}