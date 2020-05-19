//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the Apache License, Version 2.0.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Threading;
using Bio.Algorithms.Alignment;
using Bio.Algorithms.Alignment.MultipleSequenceAlignment;
using Bio.Algorithms.MUMmer;
using Bio.IO;
using Bio.IO.BAM;
using Bio.IO.FastA;
using Bio.IO.SAM;
using Bio.PerfTests.Util;
using Bio.SimilarityMatrices;
using Bio.Util.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bio.Algorithms.Assembly.Padena;
using Bio.Algorithms.Assembly;

namespace Bio.PerfTests
{
    /// <summary>
    /// Class for getting the Perf numbers for different algorithms.
    /// </summary>
    [TestClass]
    public class PerfTests
    {
        #region Global Variables

        internal static PerformanceCounter cpuCounterObj =
           new PerformanceCounter();
        internal static string cpuCounterValue = "";
        internal static Stopwatch watchObj = new Stopwatch();
        MUMmer mummerObj;
        NUCmer nucmerObj;
        SmithWatermanAligner swObj;
        NeedlemanWunschAligner nwObj;
        PAMSAMMultipleSequenceAligner msa;
        RunPaDeNA runPaDeNA;
        bool mummerThreadCompleted = false;
        bool swThreadCompleted = false;
        bool nwThreadCompleted = false;
        bool pamsamThreadCompleted = false;
        bool samBamThreadCompleted = false;
        long memoryDifference = 0;
        List<long> sampledMemoryNumbers = null;

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static PerfTests()
        {
            Utility.xmlUtil =
               new XmlUtility(@"PerfTestsConfig.xml");

            // Setting property value
            cpuCounterObj.CategoryName = "Processor";
            cpuCounterObj.CounterName = "% Processor Time";
            cpuCounterObj.InstanceName = "_Total";
        }

        #endregion Constructor

        #region Perf Tests

        /// <summary>
        /// Gets MUMmer perf and CPU utilization numbers using small size Ecoli TestData.
        /// Input : Small size Ecoli test data with 8KB, MUMLength-10.
        /// Output : Validation of time and memory taken to execute 8KB file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void MUMmerPerfTestUsingSmallSizeEcoliData()
        {
            MUMmerPerf(Constants.MUMmerSmallSizeTestDataNodeName, 10);
        }

        /// <summary>
        /// Gets MUMmer perf and CPU utilization numbers using large size E-coli TestData.
        /// Input : Large size Ecoli test data with 1MB,MUMLength-10.
        /// Output : Validation of time and memory taken to execute 1MB file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void MUMmerPerfTestUsingLargeSizeEcoliData()
        {
            MUMmerPerf(Constants.MUMmerLargeSizeTestDataNodeName, 100);
        }

        /// <summary>
        /// Gets MUMmer perf and CPU utilization numbers using QUT test data.
        /// Input : QUT test data test data with 8KB, MUMLength-10.
        /// Output : Validation of time and memory taken to execute 8KB file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void MUMmerPerfTestUsingQUTData()
        {
            MUMmerPerf(Constants.MUMmerQUTTestDataNodeName, 10);
        }

        /// <summary>
        /// Gets PaDeNa perf and memory nos using large size Euler test data 
        /// with size 7MB.
        /// Input : Euler test data with size 7MB.
        /// Output : Validation of perf and memory using Euler test data.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void PaDeNAPerfUsingLargeSizeEulerTestData()
        {
            runPaDeNA = new RunPaDeNA();
            runPaDeNA.RunPaDeNAPerf(Constants.PaDeNAEulerTestDataName, 100);
        }

        /// <summary>
        /// Gets PaDeNa perf and memory nos using small size Euler test data 
        /// with size 2MB.
        /// Input : Euler test data with size 2MB.
        /// Output : Validation of perf nad memory using Euler test data.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void PaDeNAPerfUsingSmallSizeEulerTestData()
        {
            runPaDeNA = new RunPaDeNA();
            runPaDeNA.RunPaDeNAPerf(Constants.PaDeNASmallSizeEulerTestDataNode, 10);
        }

        /// <summary>
        /// Gets PAMSAM perf and memory nos using medium size 45 KB of file with 122 reads
        /// Input : Medium size "Phyllodonta indeterminata voucher 05-SRNP-3066 
        /// cytochrome oxidase subunit 1 (COI) gene" file with 122 sequence reads,
        /// Gap Penalty =-13,KmerLength=4.
        /// Output : Validation of time and memory taken to execute 45KB file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void PAMSAMPerfTestUsingMediumSizeTestData()
        {
            PAMSAMPerf(Constants.PamsamSmallSizeTestDataNode, 10);
        }

        /// <summary>
        /// Gets PAMSAM perf and memory nos using large size 375 KB of file with 854 reads
        /// Input : Large size "Phyllodonta indeterminata voucher 05-SRNP-3066 
        /// cytochrome oxidase subunit 1 (COI) gene" file with 854 sequence reads,
        /// Gap Penalty =-13,KmerLength=4.
        /// Output : Validation of time and memory taken to execute 375KB file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void PAMSAMPerfTestUsingLargeSizeTestData()
        {
            PAMSAMPerf(Constants.PamsamLargeSizeTestDataNode, 100);
        }

        /// <summary>
        /// Gets Smith Waterman perf and CPU utilization numbers using small size file test data.
        /// Input : Small size "Homo sapiens claspin homolog (Xenopus laevis) genome" test data
        /// with 8KB size,Gap Penalty =-10.
        /// Output : Validation of time and memory taken to execute 8KB file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void SmithWatermanPerfTestUsingSmallSizeTestData()
        {
            SmithWatermanPerf(Constants.AlignmentAlgorithmSmallSizeTestDataNode, 10);
        }

        /// <summary>
        /// Gets Smith Waterman perf and CPU utilization numbers using medium size file test data.
        /// Input : Medium size "Homo sapiens claspin homolog (Xenopus laevis) genome" test data
        /// with 25KB size,Gap Penalty =-10.
        /// Output : Validation of time and memory taken to execute 25KB file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void SmithWatermanPerfTestUsingMediumSizeTestData()
        {
            SmithWatermanPerf(Constants.AlignmentAlgorithmMediumSizeTestDataNode, 100);
        }

        /// <summary>
        /// Gets NeedlemanWunsch perf and CPU utilization numbers using small size file test data.
        /// Input : Small size "Homo sapiens claspin homolog (Xenopus laevis) genome" test data
        /// with 8KB size,Gap Penalty =-10.
        /// Output : Validation of time and memory taken to execute 8KB file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void NeedlemanWunschPerfTestUsingSmallSizeTestData()
        {
            NeedlemanWunschPerf(Constants.AlignmentAlgorithmSmallSizeTestDataNode, 10);
        }

        /// <summary>
        /// Gets NeedlemanWunsch perf and CPU utilization numbers using medium size file test data.
        /// Input : Small size "Homo sapiens claspin homolog (Xenopus laevis) genome" test data
        /// with 25KB size,Gap Penalty =-10.
        /// Output : Validation of time and memory taken to execute 25KB file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void NeedlemanWunschPerfTestUsingMediumSizeTestData()
        {
            NeedlemanWunschPerf(Constants.AlignmentAlgorithmMediumSizeTestDataNode, 100);
        }

        /// <summary>
        /// Gets BAM Parser perf numbers using large size BAM file with size 1MB.
        /// Input : 1MB BAM file.
        /// Output : Validation of time and memory taken to parse 1MB BAM file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void BAMParserPerfTestUsingLargeSizeTestData()
        {
            SAMBAMParserPerf(Constants.BAMParserLargeSizeTestDataNode,
                "BAM", 10);
        }

        /// <summary>
        /// Gets BAM Parser perf numbers using very large size BAM file with size 100MB.
        /// Input : 100MB BAM file.
        /// Output : Validation of time and memory taken to parse 100MB BAM file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void BAMParserPerfTestUsingVeryLargeSizeTestData()
        {
            SAMBAMParserPerf(Constants.BAMParserVeryLargeSizeTestDataNode,
                "BAM", 100);
        }

        /// <summary>
        /// Gets SAM Parser perf numbers using large size SAM file with size 1MB.
        /// Input : 1MB SAM file.
        /// Output : Validation of time and memory taken to parse 1MB BAM file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void SAMParserPerfTestUsingLargeSizeTestData()
        {
            SAMBAMParserPerf(Constants.SAMParserLargeSizeTestDataNode,
                "SAM", 10);
        }

        /// <summary>
        /// Gets SAM Parser perf numbers using very large size SAM file with size 100MB.
        /// Input : 100MB SAM file.
        /// Output : Validation of time and memory taken to parse 100MB SAM file.
        /// </summary>
        [TestMethod]
        [Priority(3)]
        [TestCategory("Priority3")]
        public void SAMParserPerfTestUsingVeryLargeSizeTestData()
        {
            SAMBAMParserPerf(Constants.SAMParserVeryLargeSizeTestDataNode,
                "SAM", 100);
        }

        #endregion Perf Tests

        #region Helper Methods

        /// <summary>
        /// Gets MUMmer perf and CPU utilization numbers using E-coli small/Large size test data.
        /// <param name="nodeName">Different xml nodename used for different test case</param>
        /// <param name="samplingTime">Sampling time for getting the memory numbers</param>
        /// </summary>
        void MUMmerPerf(string nodeName, int samplingTime)
        {
            Stopwatch watchObj = new Stopwatch();

            ApplicationLog.Write("\r\nMUMMER Algorithm Performance Numbers");
            ApplicationLog.Write("\r\n------------------------------------\r\n");
            // Get Sequence file path.
            string refPath =
              Utility.xmlUtil.GetTextValue(nodeName,
              Constants.RefFilePathNode);
            string queryPath =
              Utility.xmlUtil.GetTextValue(nodeName,
              Constants.QueryFilePathNode);
            string smFilePath =
              Utility.xmlUtil.GetTextValue(nodeName,
              Constants.SMFilePathNode);

            // Dispose the resources allocated.
            Dispose();

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(refPath);
            lstInputFiles.Add(queryPath);

            FastAParser parserObj = new FastAParser(refPath);
            IEnumerable<ISequence> seqs1 = parserObj.Parse();

            parserObj = new FastAParser(queryPath);
            IEnumerable<ISequence> seqs2 = parserObj.Parse();

            IAlphabet alphabet = Alphabets.AmbiguousDNA;
            ISequence originalSequence1 = seqs1.ElementAt(0);

            Sequence aInput = new Sequence(alphabet, new string(originalSequence1.Select(a => (char)a).ToArray()));

            SimilarityMatrix sm = new SimilarityMatrix(smFilePath);
            mummerObj = new MUMmer(aInput);
            mummerObj.LengthOfMUM = Int32.Parse(
              Utility.xmlUtil.GetTextValue(nodeName,
              Constants.MUMLengthNode));

            watchObj.Reset();
            watchObj.Start();

            // Align sequences using MUMmer.
            var alignment = mummerObj.GetMatchesUniqueInReference(seqs2.ElementAt(0));

            watchObj.Stop();

            // Sampled Memory numbers are stored here
            sampledMemoryNumbers = new List<long>();
            PerformanceCounter counter =
            new PerformanceCounter("Memory", "Available MBytes");

            Tuple<IList<ISequence>> tupleObj =
                new Tuple<IList<ISequence>>(seqs2.ToList());

            // Thread where the GetMUMs method is executed
            Thread getMUMsThread = new Thread(GetMUMs);
            getMUMsThread.Start(tupleObj);

            while (true)
            {
                // Sampling time is set and the memory is read after the sampling time
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Thread.Sleep(samplingTime);
                long totalMemory = GC.GetTotalMemory(true);
                sampledMemoryNumbers.Add(totalMemory);

                // Condition to check if the available memory is less than 100 MB and
                // kill the thread if it exceeds
                if (counter.NextValue() < 100)
                {
                    ApplicationLog.Write("Limit Reached, available memory is only "
                        + counter.NextValue().ToString() + " MB");
                    ApplicationLog.WriteLine("\r\nGetMUMs() method aborted");
                    getMUMsThread.Abort();
                    break;
                }

                // If mummer run is completed, exit out of the loop.
                if (mummerThreadCompleted)
                    break;
            }

            if (mummerThreadCompleted)
            {
                double memAverage = sampledMemoryNumbers.Average();
                double memPeak = sampledMemoryNumbers.Max();

                ApplicationLog.WriteLine(string.Format(
                 "MUMmer GetMUMs() method, No Of MUMs : {0}",
                 alignment.Count().ToString()));

                //// Display MUMmer perf test case execution details.
                DisplayTestCaseHeader(lstInputFiles, watchObj, memAverage.ToString(), memPeak.ToString(),
                    "MUMmer");
            }

            // Dispose MUMmer object.
            mummerObj = null;
            counter = null;
        }

        /// <summary>
        /// GetMUMs mummer method is executed here
        /// </summary>
        /// <param name="obj">Object</param>
        void GetMUMs(object obj)
        {
            Tuple<IList<ISequence>> tupleObj =
                obj as Tuple<IList<ISequence>>;
            GetMUMs(tupleObj.Item1);
        }

        /// <summary>
        /// GetMUMs mummer method is executed here
        /// </summary>
        /// <param name="aInput">Input Reference Sequence</param>
        /// <param name="seqs">Query Sequence</param>
        void GetMUMs(IList<ISequence> seqs)
        {
            memoryDifference = 0;

            nucmerObj = new NUCmer();

            GC.Collect();
            long memoryStart = GC.GetTotalMemory(true);

            // Align sequences using NUCmer.
            var alignment = mummerObj.GetMatchesUniqueInReference(seqs.ElementAt(0));

            long memoryEnd = GC.GetTotalMemory(true);

            memoryDifference = memoryEnd - memoryStart;
            mummerThreadCompleted = true;
        }

        /// <summary>
        /// Gets PAMSAM perf and memory nos.
        /// <param name="nodeName">Different xml nodename used for different test case</param>
        /// </summary>
        void PAMSAMPerf(string nodeName, int samplingTime)
        {
            ApplicationLog.Write("\r\nPAMSAM Algorithm Performance numbers:");
            ApplicationLog.Write("\r\n-------------------------------------\r\n");
            Stopwatch watchObj = new Stopwatch();

            // Get input values from XML.
            string refPath =
                Utility.xmlUtil.GetTextValue(nodeName,
                Constants.RefFilePathNode);
            string queryPath =
                Utility.xmlUtil.GetTextValue(nodeName,
                Constants.QueryFilePathNode);

            // Dispose Garbage collection.
            Dispose();

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(refPath);
            lstInputFiles.Add(queryPath);

            // Parse a Reference and query sequence file.
            ISequenceParser parser = new FastAParser(refPath);
            IEnumerable<ISequence> orgSequences = parser.Parse();

            // Execute UnAlign method to verify that it does not contains gap
            List<ISequence> sequences = MsaUtils.UnAlign(orgSequences.ToList());

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
            watchObj.Reset();
            watchObj.Start();

            // Parallel Option will only get set if the PAMSAMMultipleSequenceAligner is getting called
            // To test separately distance matrix, binary tree etc.. 
            // Set the parallel option using below ctor.
            msa = new PAMSAMMultipleSequenceAligner
               (sequences, kmerLength, distanceFunctionName,
               hierarchicalClusteringMethodName, profileAlignerName,
               profileProfileFunctionName, similarityMatrix, gapOpenPenalty,
               gapExtendPenalty, numberOfPartitions, numberOfDegrees);

            // Stop watchclock.
            watchObj.Stop();

            // Sampled Memory numbers are stored here
            sampledMemoryNumbers = new List<long>();
            PerformanceCounter counter =
            new PerformanceCounter("Memory", "Available MBytes");

            Tuple<IList<ISequence>, DistanceFunctionTypes, UpdateDistanceMethodsTypes,
                    ProfileAlignerNames, ProfileScoreFunctionNames, SimilarityMatrix> tupleObj =
                new Tuple<IList<ISequence>, DistanceFunctionTypes, UpdateDistanceMethodsTypes,
                    ProfileAlignerNames, ProfileScoreFunctionNames, SimilarityMatrix>(sequences,
                    distanceFunctionName, hierarchicalClusteringMethodName, profileAlignerName,
               profileProfileFunctionName, similarityMatrix);

            // Thread where the Align method is executed
            Thread pamsamAlignThread = new Thread(pamsamAlign);
            pamsamAlignThread.Start(tupleObj);

            while (true)
            {
                // Sampling time is set and the memory is read after the sampling time
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Thread.Sleep(samplingTime);
                long totalMemory = GC.GetTotalMemory(true);
                sampledMemoryNumbers.Add(totalMemory);

                // Condition to check if the available memory is less than 100 MB and
                // kill the thread if it exceeds
                if (counter.NextValue() < 100)
                {
                    ApplicationLog.WriteLine("Limit Reached, available memory is only "
                        + counter.NextValue().ToString() + " MB");
                    ApplicationLog.WriteLine("Align() method aborted");
                    pamsamAlignThread.Abort();
                    break;
                }

                // If mummer run is completed, exit out of the loop.
                if (pamsamThreadCompleted)
                    break;
            }

            if (pamsamThreadCompleted)
            {
                double memAverage = sampledMemoryNumbers.Average();
                double memPeak = sampledMemoryNumbers.Max();

                ApplicationLog.WriteLine(string.Format(
                 "PAMSAM SequenceAligner method, Alignment Score is : {0}",
                  msa.AlignmentScore.ToString()));

                // Display all aligned sequence, performance and memory optimization nos.
                DisplayTestCaseHeader(lstInputFiles, watchObj, memAverage.ToString(),
                    memPeak.ToString(), "PAMSAM");
            }

            counter = null;
        }

        /// <summary>
        /// PAMSAM Align method is executed here
        /// </summary>
        /// <param name="obj">Object</param>
        void pamsamAlign(object obj)
        {
            Tuple<IList<ISequence>, DistanceFunctionTypes, UpdateDistanceMethodsTypes,
                   ProfileAlignerNames, ProfileScoreFunctionNames, SimilarityMatrix> tupleObj =
                obj as Tuple<IList<ISequence>, DistanceFunctionTypes, UpdateDistanceMethodsTypes,
                   ProfileAlignerNames, ProfileScoreFunctionNames, SimilarityMatrix>;
            pamsamAlign(tupleObj.Item1, tupleObj.Item2, tupleObj.Item3,
                tupleObj.Item4, tupleObj.Item5, tupleObj.Item6);
        }

        /// <summary>
        /// PAMSAM Align method is executed here.
        /// </summary>
        /// <param name="seqs">Query sequences</param>
        /// <param name="disFunc">Profile Distance function</param>
        /// <param name="updatedMetdType">Distance Method type</param>
        /// <param name="profileAlignrName">Profiler name</param>
        /// <param name="scoreFunc">Score function used</param>
        /// <param name="sm">Similarity Matrix used</param>
        void pamsamAlign(IList<ISequence> seqs, DistanceFunctionTypes disFunc,
            UpdateDistanceMethodsTypes updatedMetdType, ProfileAlignerNames profileAlignrName,
            ProfileScoreFunctionNames scoreFunc, SimilarityMatrix sm)
        {
            memoryDifference = 0;

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

            // Parallel Option will only get set if the PAMSAMMultipleSequenceAligner is getting called
            // To test separately distance matrix, binary tree etc.. 
            // Set the parallel option using below ctor.

            GC.Collect();
            long memoryStart = GC.GetTotalMemory(true);
            msa = new PAMSAMMultipleSequenceAligner
               (seqs, kmerLength, disFunc,
               updatedMetdType, profileAlignrName,
               scoreFunc, sm, gapOpenPenalty,
               gapExtendPenalty, numberOfPartitions, numberOfDegrees);
            long memoryEnd = GC.GetTotalMemory(true);

            memoryDifference = memoryEnd - memoryStart;
            pamsamThreadCompleted = true;
        }

        /// <summary>
        /// Gets Needleman Wunsch perf and CPU utilization numbers.
        /// <param name="nodeName">Different xml nodename used for different test case</param>
        /// <param name="samplingTime">sampling time</param>
        /// </summary>
        void NeedlemanWunschPerf(string nodeName, int samplingTime)
        {
            ApplicationLog.Write("\r\nNeedlemanWunsch Algorithm Performance numbers:");
            ApplicationLog.Write("\r\n----------------------------------------------\r\n");
            // Get Sequence file path.
            string refPath =
              Utility.xmlUtil.GetTextValue(nodeName,
              Constants.RefFilePathNode);
            string queryPath =
              Utility.xmlUtil.GetTextValue(nodeName,
              Constants.QueryFilePathNode);
            string smFilePath =
              Utility.xmlUtil.GetTextValue(nodeName,
              Constants.SMFilePathNode);

            // Dispose the resources allocated 
            Dispose();

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(refPath);
            lstInputFiles.Add(queryPath);

            FastAParser parserObj = new FastAParser(refPath);
            IEnumerable<ISequence> seqs1 = parserObj.Parse();

            parserObj = new FastAParser(queryPath);
            IEnumerable<ISequence> seqs2 = parserObj.Parse();

            IAlphabet alphabet = Alphabets.DNA;
            ISequence originalSequence1 = seqs1.ElementAt(0);
            ISequence originalSequence2 = seqs2.ElementAt(0);

            ISequence aInput = new Sequence(alphabet,
                new string(originalSequence1.Select(a => (char)a).ToArray()));
            ISequence bInput = new Sequence(alphabet,
                new string(originalSequence2.Select(a => (char)a).ToArray()));

            SimilarityMatrix sm = new SimilarityMatrix(smFilePath);
            nwObj = new NeedlemanWunschAligner();
            nwObj.GapOpenCost = -10;
            nwObj.GapExtensionCost = -10;
            nwObj.SimilarityMatrix = sm;

            watchObj = new Stopwatch();
            watchObj.Reset();

            watchObj.Start();

            // Align sequences using smith water man algorithm.
            IList<IPairwiseSequenceAlignment> alignment = nwObj.AlignSimple(aInput, bInput);

            watchObj.Stop();

            // Sampled Memory numbers are stored here
            sampledMemoryNumbers = new List<long>();
            PerformanceCounter counter =
            new PerformanceCounter("Memory", "Available MBytes");

            Tuple<ISequence, ISequence, string> tupleObj =
               new Tuple<ISequence, ISequence, string>(aInput, bInput, smFilePath);

            // Thread where the GetMUMs method is executed
            Thread nwAlignThread = new Thread(nwAlignSimple);
            nwAlignThread.Start(tupleObj);

            while (true)
            {
                // Sampling time is set and the memory is read after the sampling time
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Thread.Sleep(samplingTime);
                long totalMemory = GC.GetTotalMemory(true);
                sampledMemoryNumbers.Add(totalMemory);

                // Condition to check if the available memory is less than 100 MB and
                // kill the thread if it exceeds
                if (counter.NextValue() < 100)
                {
                    ApplicationLog.WriteLine("Limit Reached, available memory is only "
                        + counter.NextValue().ToString() + " MB");
                    ApplicationLog.WriteLine("NW SimpleAlign() method aborted");
                    nwAlignThread.Abort();
                    break;
                }

                // If mummer run is completed, exit out of the loop.
                if (nwThreadCompleted)
                    break;
            }

            if (nwThreadCompleted)
            {
                double memAverage = sampledMemoryNumbers.Average();
                double memPeak = sampledMemoryNumbers.Max();

                ApplicationLog.WriteLine(string.Format(
                "Needleman Wunsch AlignSimple() method, Alignment Score is : {0}",
                alignment[0].PairwiseAlignedSequences[0].Score.ToString()));

                // Display Needlemanwunsch perf test case execution details.
                DisplayTestCaseHeader(lstInputFiles, watchObj, memAverage.ToString(),
                   memPeak.ToString(), "NeedlemanWunsch");

            }

            // Dispose NeedlemanWunsch object
            nwObj = null;
            counter = null;
        }

        /// <summary>
        /// NW AlignSimple method is executed here
        /// </summary>
        /// <param name="obj">Object</param>
        void nwAlignSimple(object obj)
        {
            Tuple<ISequence, ISequence, string> tupleObj =
                obj as Tuple<ISequence, ISequence, string>;
            nwAlignSimple(tupleObj.Item1, tupleObj.Item2, tupleObj.Item3);
        }

        /// <summary>
        /// NW AlignSimple method is executed here
        /// </summary>
        /// <param name="aInput">Input Reference Sequence</param>
        /// <param name="bInput">Input Query Sequence</param>
        /// <param name="smFilePath">Similarity matrix file path</param>
        void nwAlignSimple(ISequence aInput, ISequence bInput, string smFilePath)
        {
            memoryDifference = 0;

            SimilarityMatrix sm = new SimilarityMatrix(smFilePath);
            nwObj = new NeedlemanWunschAligner();
            nwObj.GapOpenCost = -10;
            nwObj.GapExtensionCost = -10;
            nwObj.SimilarityMatrix = sm;

            GC.Collect();
            long memoryStart = GC.GetTotalMemory(true);

            // Align sequences using smith water man algorithm.
            IList<IPairwiseSequenceAlignment> alignment = nwObj.AlignSimple(aInput, bInput);

            long memoryEnd = GC.GetTotalMemory(true);

            memoryDifference = memoryEnd - memoryStart;
            nwThreadCompleted = true;
        }

        /// <summary>
        /// Gets Smith Waterman perf and CPU utilization numbers.
        /// <param name="nodeName">Different xml nodename used for different test case</param>
        /// <param name="samplingTime">Sampling Time</param>
        /// </summary>
        void SmithWatermanPerf(string nodeName, int samplingTime)
        {
            ApplicationLog.Write("\r\nSmithWaterman Algorithm Performance numbers:");
            ApplicationLog.Write("\r\n--------------------------------------------\r\n");
            // Get Sequence file path.
            string refPath =
              Utility.xmlUtil.GetTextValue(nodeName,
              Constants.RefFilePathNode);
            string queryPath =
              Utility.xmlUtil.GetTextValue(nodeName,
              Constants.QueryFilePathNode);
            string smFilePath =
              Utility.xmlUtil.GetTextValue(nodeName,
              Constants.SMFilePathNode);

            // Dispose resources allocated.
            Dispose();

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(refPath);
            lstInputFiles.Add(queryPath);

            FastAParser parserObj = new FastAParser(refPath);
            IEnumerable<ISequence> seqs1 = parserObj.Parse();

            parserObj = new FastAParser(queryPath);
            IEnumerable<ISequence> seqs2 = parserObj.Parse();

            IAlphabet alphabet = Alphabets.DNA;
            ISequence originalSequence1 = seqs1.ElementAt(0);
            ISequence originalSequence2 = seqs2.ElementAt(0);

            ISequence aInput = new Sequence(alphabet, new string(originalSequence1.Select(a => (char)a).ToArray()));
            ISequence bInput = new Sequence(alphabet, new string(originalSequence2.Select(a => (char)a).ToArray()));

            SimilarityMatrix sm = new SimilarityMatrix(smFilePath);
            swObj = new SmithWatermanAligner();
            swObj.GapOpenCost = -10;
            swObj.GapExtensionCost = -10;
            swObj.SimilarityMatrix = sm;
            watchObj = new Stopwatch();

            watchObj.Reset();
            watchObj.Start();

            // Align sequences using smith water man algorithm.
            IList<IPairwiseSequenceAlignment> alignment = swObj.AlignSimple(aInput, bInput);

            watchObj.Stop();

            // Sampled Memory numbers are stored here
            sampledMemoryNumbers = new List<long>();
            PerformanceCounter counter =
            new PerformanceCounter("Memory", "Available MBytes");

            Tuple<ISequence, ISequence, string> tupleObj =
                new Tuple<ISequence, ISequence, string>(aInput, bInput, smFilePath);

            // Thread where the AlignSimple method is executed
            Thread swAlignThread = new Thread(swAlignSimple);
            swAlignThread.Start(tupleObj);

            while (true)
            {
                // Sampling time is set and the memory is read after the sampling time
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Thread.Sleep(samplingTime);
                long totalMemory = GC.GetTotalMemory(true);
                sampledMemoryNumbers.Add(totalMemory);

                // Condition to check if the available memory is less than 100 MB and
                // kill the thread if it exceeds
                if (counter.NextValue() < 100)
                {
                    ApplicationLog.WriteLine("Limit Reached, available memory is only "
                        + counter.NextValue().ToString() + " MB");
                    ApplicationLog.WriteLine("SW Align() method aborted");
                    swAlignThread.Abort();
                    break;
                }

                // If mummer run is completed, exit out of the loop.
                if (swThreadCompleted)
                    break;
            }

            if (swThreadCompleted)
            {
                double memAverage = sampledMemoryNumbers.Average();
                double memPeak = sampledMemoryNumbers.Max();


                ApplicationLog.WriteLine(string.Format(
                  "Smith Waterman AlignSimple() method, Alignment Score is : {0}",
                  alignment[0].PairwiseAlignedSequences[0].Score.ToString()));

                // Display SmithWaterman perf test case execution details.
                DisplayTestCaseHeader(lstInputFiles, watchObj, memAverage.ToString(),
                    memPeak.ToString(), "SmithWaterman");
            }

            // Dispose SmithWaterman object
            swObj = null;
            counter = null;

        }

        /// <summary>
        /// SW AlignSimple method is executed here
        /// </summary>
        /// <param name="obj">Object</param>
        void swAlignSimple(object obj)
        {
            Tuple<ISequence, ISequence, string> tupleObj =
                obj as Tuple<ISequence, ISequence, string>;
            swAlignSimple(tupleObj.Item1, tupleObj.Item2, tupleObj.Item3);
        }

        /// <summary>
        /// SW AlignSimple method is executed here
        /// </summary>
        /// <param name="aInput">Input Reference Sequence</param>
        /// <param name="bInput">Input Query Sequence</param>
        /// <param name="smFilePath">Similarity matrix file path</param>
        void swAlignSimple(ISequence aInput, ISequence bInput, string smFilePath)
        {
            memoryDifference = 0;

            SimilarityMatrix sm = new SimilarityMatrix(smFilePath);
            swObj = new SmithWatermanAligner();
            swObj.GapOpenCost = -10;
            swObj.GapExtensionCost = -10;
            swObj.SimilarityMatrix = sm;

            GC.Collect();
            long memoryStart = GC.GetTotalMemory(true);

            // Align sequences using smith water man algorithm.
            IList<IPairwiseSequenceAlignment> alignment = swObj.AlignSimple(aInput, bInput);

            long memoryEnd = GC.GetTotalMemory(true);

            memoryDifference = memoryEnd - memoryStart;
            swThreadCompleted = true;
        }

        /// <summary>
        /// Gets BAM Parser perf numbers.
        /// <param name="nodeName">XML nodename used for different test case</param>
        /// </summary>
        void SAMBAMParserPerf(string nodeName,
            string parserName, int samplingTime)
        {
            ApplicationLog.Write("\r\nSAM/BAM Parser Performance numbers:");
            ApplicationLog.Write("\r\n-----------------------------------\r\n");

            // Get SAM/BAM file path.
            string filePath =
                Utility.xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);

            // Dispose resources allocated before executing SAM/BAM Parser.
            Dispose();

            // Create a List for input files.
            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(filePath);
            SequenceAlignmentMap align = null;

            // Create a BAM Parser object
            BAMParser parseBam = new BAMParser();

            SAMParser parseSam = new SAMParser();

            watchObj = new Stopwatch();
            watchObj.Reset();
            watchObj.Start();

            if ("BAM" == parserName)
            {
                align = parseBam.Parse(filePath);
            }
            else
            {
                align = parseSam.Parse(filePath);
            }

            watchObj.Stop();


            Assert.IsNotNull(align);

            // Sampled Memory numbers are stored here
            sampledMemoryNumbers = new List<long>();
            PerformanceCounter counter =
            new PerformanceCounter("Memory", "Available MBytes");

            Tuple<string, string> tupleObj =
                new Tuple<string, string>(filePath, parserName);

            // Thread where the BAM/SAM Parser method is executed
            Thread samParserThread = new Thread(sambamParser);
            samParserThread.Start(tupleObj);

            while (true)
            {
                // Sampling time is set and the memory is read after the sampling time
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Thread.Sleep(samplingTime);
                long totalMemory = GC.GetTotalMemory(true);
                sampledMemoryNumbers.Add(totalMemory);

                // Condition to check if the available memory is less than 100 MB and
                // kill the thread if it exceeds
                if (counter.NextValue() < 100)
                {
                    ApplicationLog.WriteLine("Limit Reached, available memory is only "
                        + counter.NextValue().ToString() + " MB");
                    ApplicationLog.WriteLine("SAM/BAM Parser() method aborted");
                    samParserThread.Abort();
                    break;
                }

                // If mummer run is completed, exit out of the loop.
                if (samBamThreadCompleted)
                    break;
            }

            if (samBamThreadCompleted)
            {
                double memAverage = sampledMemoryNumbers.Average();
                double memPeak = sampledMemoryNumbers.Max();


                // Display BAMParser perf test case execution details.
                DisplayTestCaseHeader(lstInputFiles, watchObj, memAverage.ToString(), memPeak.ToString(),
                    null);
            }

            // Delete sidecar files.
            parseBam = null;
            parseSam = null;
            string sidecarFileName = Path.GetFileName(filePath) + ".isc";
            File.Delete(sidecarFileName);
            counter = null;
        }

        /// <summary>
        /// SAM/BAM Parser method is executed here
        /// </summary>
        /// <param name="obj">Object</param>
        void sambamParser(object obj)
        {
            Tuple<string, string> tupleObj =
                obj as Tuple<string, string>;
            sambamParser(tupleObj.Item1, tupleObj.Item2);
        }

        /// <summary>
        /// SAM/BAm Parser method is executed here.
        /// </summary>
        /// <param name="filePath">Path of the file</param>
        /// <param name="parserName">Parser being executed</param>
        void sambamParser(string filePath, string parserName)
        {
            memoryDifference = 0;
            SequenceAlignmentMap align = null;

            // Create a BAM Parser object
            BAMParser parseBam = new BAMParser();
            SAMParser parseSam = new SAMParser();


            GC.Collect();
            long memoryStart = GC.GetTotalMemory(true);

            if ("BAM" == parserName)
            {
                align = parseBam.Parse(filePath);
            }
            else
            {
                align = parseSam.Parse(filePath);
            }

            long memoryEnd = GC.GetTotalMemory(true);

            memoryDifference = memoryEnd - memoryStart;
            samBamThreadCompleted = true;
        }

        /// <summary>
        /// Dispalys the headers present in the BAM file
        /// </summary>
        /// <param name="seqAlignmentMap">SeqAlignment map</param>
        void DisplayHeader(SequenceAlignmentMap seqAlignmentMap)
        {
            // Get Header
            SAMAlignmentHeader header = seqAlignmentMap.Header;
            IList<SAMRecordField> recordField = header.RecordFields;
            IList<string> commenstList = header.Comments;

            if (recordField.Count > 0)
            {
                ApplicationLog.WriteLine("MetaData:");

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
        void DisplaySeqAlignments(SequenceAlignmentMap seqAlignment)
        {
            // Get Aligned sequences
            IList<SAMAlignedSequence> alignedSeqs = seqAlignment.QuerySequences;
            try
            {
                for (int i = 0; i < alignedSeqs.Count; i++)
                {
                    string seq = "*";
                    if (alignedSeqs[i].QuerySequence.Count > 0)
                    {
                        seq = alignedSeqs[i].QuerySequence.ToString();
                    }

                    string qualValues = "*";

                    QualitativeSequence qualSeq = alignedSeqs[i].QuerySequence as QualitativeSequence;
                    if (qualSeq != null)
                    {
                        IEnumerable<byte> bytes = qualSeq.QualityScores;
                        qualValues = new string(bytes.Select(a => (char)a).ToArray());
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
                ApplicationLog.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Display header information of the test case being executed.
        /// </summary>
        /// <param name="ipnutFiles">List of Input files</param>
        /// <param name="stopWatchObj">Stop watch object</param>
        /// <param name="memoryUsage">memory used to execute algorithm</param>
        /// <param name="algorithm">name of the algorithm being executed</param>
        public void DisplayTestCaseHeader(List<string> ipnutFiles,
            Stopwatch stopWatchObj, string memAvg, string memoryPeak, string algorithm)
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

            ApplicationLog.Write("\r\nTest Method: {0}", methodBase.Name);

            // Display Input sequence file information.
            int i = 1;
            ApplicationLog.Write("\r\nInput Files:");
            foreach (string file in ipnutFiles)
            {
                FileInfo ipnutFileInfo = new FileInfo(file);
                ApplicationLog.Write("\r\nInputFile{0} -\tFile-name : {1}, File-size :{2} bytes",
                    i, ipnutFileInfo.Name, ipnutFileInfo.Length.ToString());
                i++;
            }

            // Display Test input parameters used for different features.
            switch (algorithm)
            {
                case "MUMmer":
                    ApplicationLog.Write("\r\nInput Parameters Used : MUMLength:{0}",
                     mummerObj.LengthOfMUM.ToString());
                    break;
                case "PAMSAM":
                    ApplicationLog.Write("\r\nInput Parameters Used : GapOpenPenalty:{0},GapExtendPenalty:{1}",
                    msa.GapOpenCost, msa.GapExtensionCost);
                    break;
                case "SmithWaterman":
                    ApplicationLog.Write("\r\nInput Parameters Used : GapOpenCost:{0},GapExtensionCost:{1},SimilarityMatrix:{2}",
                    swObj.GapOpenCost, swObj.GapExtensionCost, swObj.SimilarityMatrix.Name);
                    break;
                case "NeedlemanWunsch":
                    ApplicationLog.Write("\r\nInput Parameters Used : GapOpenCost:{0},GapExtensionCost:{1},SimilarityMatrix:{2}",
                        nwObj.GapOpenCost, nwObj.GapExtensionCost, nwObj.SimilarityMatrix.Name);
                    break;
            }

            ApplicationLog.Write("\r\nPlatform Used : ");
            ApplicationLog.Write("\r\nOperating System :{0}Processor cores:{1}Physical memory :{2}CPU Speed:{3}",
                osName, processorNo, memoryInBytes, cpuSpeed);
            ApplicationLog.Write("\r\nTime taken to execute {0} in secs: {1}", methodBase.Name,
                TimeSpan.FromMilliseconds(
                stopWatchObj.ElapsedMilliseconds).TotalSeconds.ToString());
            ApplicationLog.Write("\r\nAverage Memory used in bytes : {0}", memAvg);
            ApplicationLog.Write("\r\nMemory Difference in bytes : {0}", memoryDifference);
            ApplicationLog.Write("\r\nMemory Peak in bytes : {0}", memoryPeak);

            if (algorithm != "PaDeNA" && null != sampledMemoryNumbers && 0 != sampledMemoryNumbers.Count)
            {
                ApplicationLog.Write("\r\nSampled memory numbers in bytes are : \r\n");
                foreach (long mem in sampledMemoryNumbers)
                {
                    ApplicationLog.Write("{0},", mem.ToString());
                }
                ApplicationLog.Write("\r\n");
            }
        }

        /// <summary>
        /// Get Name of the Operating system being used.
        /// </summary>
        /// <returns>Name of the OS</returns>
        string GetOSName()
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
        string GetProcessors()
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
        double GetPhysicalMemory()
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
        object GetProcessorSpeed()
        {
            ManagementObject managementObject = new
                ManagementObject("Win32_Processor.DeviceID='CPU0'");
            object cpuSpeed = managementObject["CurrentClockSpeed"];
            return cpuSpeed;
        }

        /// <summary>
        /// Free the resources allocated.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion Helper Methods
    }

    /// <summary>
    /// PaDeNA Class
    /// </summary>
    class RunPaDeNA : ParallelDeNovoAssembler
    {

        #region Global variables

        long memoryDifference = 0;
        List<long> sampledMemoryNumbers = null;
        PerformanceCounter counter =
            new PerformanceCounter("Memory", "Available MBytes");
        bool PadenaThreadCompleted = false;

        #endregion Global variables

        /// <summary>
        /// Run PaDeNA Assemble method
        /// </summary>
        /// <param name="nodeName">XML nodename</param>
        /// <param name="samplingTime">Sampling time</param>
        public void RunPaDeNAPerf(string nodeName, int samplingTime)
        {
            ApplicationLog.Write("\r\nPaDeNA Algorithm Performance numbers:");
            ApplicationLog.Write("\r\n-------------------------------------\r\n");

            // Get input values from xml file.
            string queryFilePath = Utility.xmlUtil.GetTextValue(nodeName,
                Constants.FilePathNode);
            string kmerLength = Utility.xmlUtil.GetTextValue(nodeName,
                Constants.KmerLengthNode);
            string depth = Utility.xmlUtil.GetTextValue(nodeName,
                Constants.DepthNode);
            string stdDeviation = Utility.xmlUtil.GetTextValue(nodeName,
                Constants.StdDeviationNode);
            string meanValue = Utility.xmlUtil.GetTextValue(nodeName,
                Constants.MeanNode);

            List<string> lstInputFiles = new List<string>();
            lstInputFiles.Add(queryFilePath);

            ParallelDeNovoAssembler parallel = new ParallelDeNovoAssembler();
            parallel.KmerLength = Int32.Parse(kmerLength);
            parallel.DanglingLinksThreshold = Int32.Parse(kmerLength);
            parallel.RedundantPathLengthThreshold = 2 * (Int32.Parse(kmerLength) + 1);

            // Release Garbage collection.
            PerfTests perfTestObj = new PerfTests();
            perfTestObj.Dispose();

            List<ISequence> sequences = new List<ISequence>();
            using (StreamReader read = new StreamReader(queryFilePath))
            {
                string Id = read.ReadLine();
                string seq = read.ReadLine();
                while (!string.IsNullOrEmpty(seq))
                {
                    Sequence sequence = new Sequence(Alphabets.DNA, seq);
                    sequence.ID = Id;
                    sequences.Add(sequence);
                    Id = read.ReadLine();
                    seq = read.ReadLine();
                }
            }

            CloneLibrary.Instance.AddLibrary("abc", float.Parse(meanValue),
                float.Parse(stdDeviation));

            parallel.Depth = Int32.Parse(depth);
            parallel.AllowErosion = true;
            parallel.AllowLowCoverageContigRemoval = true;

            Stopwatch watch = new Stopwatch();
            watch.Reset();
            watch.Start();
            IDeNovoAssembly assembly = parallel.Assemble(sequences, true);

            watch.Stop();

            // Sampled Memory numbers are stored here
            sampledMemoryNumbers = new List<long>();

            Tuple<IList<ISequence>, int, float, float, int> tupleObj =
                new Tuple<IList<ISequence>, int, float, float, int>(sequences,
                    Int32.Parse(kmerLength), float.Parse(meanValue),
                    float.Parse(stdDeviation), Int32.Parse(depth));

            // Thread where the GetMUMs method is executed
            Thread getMUMsThread = new Thread(PadenaAssemble);
            getMUMsThread.Start(tupleObj);

            while (true)
            {
                // Sampling time is set and the memory is read after the sampling time
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                Thread.Sleep(samplingTime);
                long totalMemory = GC.GetTotalMemory(true);
                sampledMemoryNumbers.Add(totalMemory);

                // Condition to check if the available memory is less than 100 MB and
                // kill the thread if it exceeds
                if (counter.NextValue() < 100)
                {
                    ApplicationLog.WriteLine("Limit Reached, available memory is only "
                        + counter.NextValue().ToString() + " MB");
                    ApplicationLog.WriteLine("PaDeNA Assembler method aborted");
                    getMUMsThread.Abort();
                    break;
                }

                // If mummer run is completed, exit out of the loop.
                if (PadenaThreadCompleted)
                    break;
            }

            if (PadenaThreadCompleted)
            {
                double memAverage = sampledMemoryNumbers.Average();
                double memPeak = sampledMemoryNumbers.Max();

                // Display PaDeNA perf test case execution details.
                ApplicationLog.Write("\r\nInput Parameters Used :\tKmerLength:{0},Dangling threshold:{1},Redundant threshold:{2}",
                          kmerLength, 2 * (Int32.Parse(kmerLength) + 1), kmerLength);

                ApplicationLog.Write(string.Format("\r\nScaffold Score is : {0}",
                   assembly.AssembledSequences.Count.ToString()));

                PerfTests test = new PerfTests();
                test.DisplayTestCaseHeader(lstInputFiles, watch, memAverage.ToString(),
                    memPeak.ToString(), "PaDeNA");

                ApplicationLog.Write("\r\nSampled memory numbers in bytes are : \r\n");
                foreach (long mem in sampledMemoryNumbers)
                {
                    ApplicationLog.Write("{0},", mem.ToString());
                }
                ApplicationLog.Write("\r\n");

            }
        }

        /// <summary>
        /// PaDeNA Assemble method is executed here
        /// </summary>
        /// <param name="obj">Object</param>
        void PadenaAssemble(object obj)
        {
            Tuple<IList<ISequence>, int, float, float, int> tupleObj =
                obj as Tuple<IList<ISequence>, int, float, float, int>;
            PadenaAssemble(tupleObj.Item1, tupleObj.Item2, tupleObj.Item3,
                tupleObj.Item4, tupleObj.Item5);
        }

        /// <summary>
        /// Execute PaDeNA Assemble method here.
        /// </summary>
        /// <param name="seqs">List of query sequences</param>
        /// <param name="kLength">KmerLength</param>
        /// <param name="stdD">Standard deviation</param>
        /// <param name="mean">Mean</param>
        void PadenaAssemble(IList<ISequence> seqs, int kLength,
            float mean, float stdD, int depth)
        {
            memoryDifference = 0;

            ParallelDeNovoAssembler parallel = new ParallelDeNovoAssembler();
            parallel.KmerLength = kLength;
            parallel.DanglingLinksThreshold = kLength;
            parallel.RedundantPathLengthThreshold = 2 * (kLength + 1);

            CloneLibrary.Instance.AddLibrary("abc", mean, stdD);

            parallel.Depth = depth;
            parallel.AllowErosion = true;
            parallel.AllowLowCoverageContigRemoval = true;

            GC.Collect();

            long memoryStart = GC.GetTotalMemory(true);

            // Align sequences using PaDeNA algorithm.
            IDeNovoAssembly assembly = parallel.Assemble(seqs, true);
            long memoryEnd = GC.GetTotalMemory(true);

            memoryDifference = memoryEnd - memoryStart;
            PadenaThreadCompleted = true;
        }
    }
}