//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShoNS.Array;
//using ShoNS.MathFunc;
using System.Diagnostics;
using MBT.Escience.Sho;
//using Microsoft.TMSN;
using System.Collections;
using MBT.Escience;
using Bio.Util;
using MBT.Escience.Parse;
using System.IO;
using GenerateKernels;
using Eigenstrat;
using Bio.Matrix;
using System.Threading;
using System.Threading.Tasks;
using ShoNS;
//using ShoNS.Stats;
using MBT.Escience.HpcLib;
using FastLMM;

namespace eLMM
{

    /// This is a clean(ish) re-write of LmmKernelMStep, which does kernel learning for the PNAS paper, by calling LMM learning code, and itself as well (previously in LMM, now in FastLMM)

    class eLMM
    {

        public static int _seed = 123456;
        public static bool HaveSetSetShoDirEnvironmentVariable = ShoUtils.HaveSetShoDirEnvironmentVariable();
        public static string _runInfoStr = "machine: " + Environment.MachineName + "\n";
        public static bool _CLEANUP;
        public static string _specialString = "Q";
        public static List<string> _kernelFiles;
        public static int _numCids, _numSNP;
        public static string _dateStamp;
        public static bool _coerceFixedEffectsToBeFullRankIfNecessary;
        private static double _multiKernelFudgeToPassCholesky = 0.0;//1e-5;
        private static double _cushionThatFallsOutExactlyButMakesComputationStable = 0;//10.0; obsolete now...

        public static bool _COMPUTE_LOG_LIKELIHOODS;
        public static bool _QUIET = true;

        public static RangeCollection _skipList;
        public static bool _CONSOLIDATE_RESULTS_AUTOMATICALLY = true;

        public static string _mappingFile = null; //required for alternative splicing omni tests

        public static void Main(string[] args)
        {
            System.Console.WriteLine("machine: " + Environment.MachineName);

            /*Four ways that this program can get called:
            (1) [obsolete] normally, to just run locally (no "-cluster", and no "computeMstepOnCluster")
            (2) as a slave task to compute the M-step on the cluster ("-computeMstepOnCluster" and NO -"cluster")
            (3) as a local job which uses the cluster to do the computations for the M-step (therefore spawns jobs of type 2 above)
            (4) -cleanup to clean up tasks of type (2)
            */

            try
            {
                Console.WriteLine(Helper.CreateDelimitedString(" ", args));

                ArgumentCollection argumentCollection = new CommandArguments(args);
                ArgumentCollection argCollectionOrig = new CommandArguments(args);

                if (argumentCollection.ExtractOptionalFlag("help"))
                {
                    Console.WriteLine("");
                    Console.WriteLine(UsageMessage);
                    return;
                }

                #region preliminaries

                bool USE_CLUSTER = false;
                //get rid of cluster stuff so that we can pass the NoMoreArguments, etc. We keep a copy of the original one, so this is fine
                if (argumentCollection.PeekOptionalFlag("cluster"))
                {
                    USE_CLUSTER = true;
                    ClusterSubmitterArgs garbage = new ClusterSubmitterArgs(argumentCollection);
                }

                _runInfoStr += "Start date: " + DateTime.Now + "\n";
                string codeVersion = SpecialFunctions.GetEntryOrCallingAssembly().GetName().Version.ToString();
                _runInfoStr += "Code version " + codeVersion + " compiled on " + SpecialFunctions.DateProgramWasCompiled().ToString() + "\n";
                Console.WriteLine("Start date: " + DateTime.Now);
                Console.WriteLine("Code version " + codeVersion + " compiled on " + SpecialFunctions.DateProgramWasCompiled().ToString());

                string firstKernelFile = argumentCollection.ExtractOptional<string>("ehKernel", null);
                string phenDataFile = argumentCollection.ExtractOptional<string>("expressionFile", null);
                //string phenDataFile = argumentCollection.ExtractOptional<string>("phenDataFile", null);


                //for internal use: a user should never call it with this falg
                bool COMPUTE_MSTEP_ON_CLUSTER = argumentCollection.ExtractOptionalFlag("computeMstepOnCluster");
                bool DO_FULL_KERNEL_LEARNING = argumentCollection.ExtractOptionalFlag("doFullKernelLearning");

                _COMPUTE_LOG_LIKELIHOODS = argumentCollection.ExtractOptionalFlag("computeLogLikelihoood");
                bool allowNonPosDefKernel = true;//ArgumentCollection.ExtractOptionalFlag("allowNonPosDefKernel");

                _CLEANUP = argumentCollection.ExtractOptional<bool>("cleanup", true);
                bool QUIET = argumentCollection.ExtractOptionalFlag("QUIET");

                string outputFileOrNull = argumentCollection.ExtractOptional<string>("outputFile", null);

                //cluster stuff
                RangeCollection tasks = argumentCollection.ExtractOptional<RangeCollection>("Tasks", new RangeCollection(0));
                int taskCount = argumentCollection.ExtractOptional<int>("TaskCount", 1);
                int lmmTaskCount = argumentCollection.ExtractOptional<int>("lmmTaskCount", 100);
                Helper.CheckCondition(_CLEANUP || (0 <= tasks.FirstElement && tasks.LastElement < taskCount), "The tasks must be more than 0 (inclusive) and less than taskCount (exclusive).");

                string nameNotUsedButNeededInArgsToPassToCluster = argumentCollection.ExtractOptional<string>("name", null);

                string impType = argumentCollection.ExtractOptional<string>("impType", "eigImp");

                //in only want to do a subset of SNPs/phenotypes
                List<string> snpIdListToDo = argumentCollection.ExtractOptional<List<string>>("snpIdListToDo", null);
                List<string> phenIdListToDo = argumentCollection.ExtractOptional<List<string>>("phenIdListToDo", null);

                //bool tryToLeaveDataOnDisk = ArgumentCollection.ExtractOptionalFlag("tryToLeaveDataOnDisk");
                LeaveDataOnDisk leaveDataOnDisk = argumentCollection.ExtractOptional<LeaveDataOnDisk>("leaveDataOnDisk", LeaveDataOnDisk.No);

                GWAS_MODEL_TYPE MODEL = argumentCollection.ExtractOptional<GWAS_MODEL_TYPE>("model", GWAS_MODEL_TYPE.NULL);
                _runInfoStr += "MODEL=" + MODEL.ToString() + "\n";

                int numPC = argumentCollection.ExtractOptional<int>("numPC", -1);
                //for backward compability, keep this one here too
                int maxNumEMsteps = argumentCollection.ExtractOptional<int>("maxNumMsteps", -1);
                if (maxNumEMsteps == -1)
                {
                    maxNumEMsteps = argumentCollection.ExtractOptional<int>("numEMit", 3);
                }

                //for backward compatibilty
                int maxFullLearnIt = argumentCollection.ExtractOptional<int>("maxFullLearnIt", -1);
                if (maxFullLearnIt == -1)
                {
                    maxFullLearnIt = argumentCollection.ExtractOptional<int>("numIt", 10);
                }
                int indOfKernelInit = argumentCollection.ExtractOptional<int>("indOfKernelInit", 0);//i.e. the first kernel... does *not* match up with -k1 -k2, where 1 is the first kernel due to STUPIDITY
                int batchSize = argumentCollection.ExtractOptional<int>("batchSize", -1);

                firstKernelFile = argumentCollection.ExtractOptional<string>("k1", null); //if (firstKernelFile == null) throw new Exception("must use at least one kernel");

                string fixedEffectsFile = argumentCollection.ExtractOptional<string>("f", null);
                //_coerceFixedEffectsToBeFullRankIfNecessary = argumentCollection.ExtractOptionalFlag("coerceF");

                _dateStamp = argumentCollection.ExtractOptional<string>("dateStamp", null);

                _skipList = argumentCollection.ExtractOptional<RangeCollection>("skipList", new RangeCollection());
                string skipListFileNameOrNull = argumentCollection.ExtractOptional<string>("skipListFile", null);
                if (null != skipListFileNameOrNull)
                {
                    Helper.CheckCondition(_skipList.Count() == 0, "Expect at most one of -skipList and -skipListFile to be given");
                    _skipList = RangeCollection.Parse(Bio.Util.FileUtils.ReadLine(skipListFileNameOrNull));
                }

                ParallelOptions parallelOptions;
                //parallelOptions = new ParallelOptions { DegreeOfParallelism = ArgumentCollection.ExtractOptional("MaxDegreeOfParallelism", Environment.ProcessorCount) };
                parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = argumentCollection.ExtractOptional("MaxDegreeOfParallelism", Environment.ProcessorCount) };
                if (USE_CLUSTER)
                {
                    argumentCollection.ForceOptional<int>("MaxDegreeOfParallelism", 1);
                    argCollectionOrig.ForceOptional<int>("MaxDegreeOfParallelism", 1);
                }
                ShoUtils.HaveSetShoDirEnvironmentVariable(parallelOptions.MaxDegreeOfParallelism);

                int maxKernelsAllowed = 2;
                _kernelFiles = new List<string>();
                if (null != firstKernelFile) _kernelFiles.Add(firstKernelFile);
                for (int k = 2; k < maxKernelsAllowed + 1; k++)
                {
                    string tmpFile = argumentCollection.ExtractOptional<string>("k" + k, null);
                    if (tmpFile == null) break;
                    _kernelFiles.Add(tmpFile);
                }

                bool doEigImpNorm = (impType == "eigImp") ? true : false;

                string inputTabFile = argumentCollection.ExtractOptional<string>("inputTabFile", null);

                phenDataFile = argumentCollection.ExtractOptional<string>("phenDataFile", null);

                /* don't use this so can leave extra stuff unparsed that is only for other exes*/
                //argumentCollection.CheckNoMoreOptions(1);

                string SNP_ENCODING_STR = argumentCollection.ExtractNext<string>("_SNP_ENCODING"); ;//max val that SNPs take on (1 for binary, 2 for trinary), assumes 0 up to max val
                string snpDataFile = null;//ArgumentCollection.ExtractNext<string>("snpDataFile");

                //Helper.CheckCondition(SNP_ENCODING_STR == "01" || SNP_ENCODING_STR == "012");
                int SNP_ENCODING = (SNP_ENCODING_STR == "01") ? 1 : 2;

                /* don't use this so can leave extra stuff unparsed that is only for other exes*/
                //argumentCollection.CheckThatEmpty();

                int numKernels = _kernelFiles.Count;
                Helper.CheckCondition(numKernels <= 2, "cannot currently use more than 2 kernels");
                Helper.CheckCondition(MODEL == (GWAS_MODEL_TYPE.FAST_LMM | GWAS_MODEL_TYPE.LMM), "can only use with GWAS_MODEL_TYPE==FAST_LMM");

                Console.WriteLine("Running eLMM kernel learning");

                //if (DO_FULL_KERNEL_LEARNING && !argCollectionOrig.PeekOptionalFlag("cluster")) throw new Exception("must use -cluster when using doFullKernelLearning");

                if (null == _dateStamp) _dateStamp = SpecialFunctions.GetDateStamp();
                string fileNamePostfix = "_" + _dateStamp;// +"Q"; //need the Q to differentiate this from the file that other code uses to find out when the jobs are done

                string strainIdFile = null;
                Matrix<string, string, double> snpMatrix, phenMatrix;
                Matrix<string, string, string> strainIds;
                //GenerateKernels.LoadData.LoadSnpAndPhenData(snpDataFile, phenDataFile, strainIdFile, parallelOptions.DegreeOfParallelism, out snpMatrix, out phenMatrix, out strainIds, tryToLeaveDataOnDisk)
                GenerateKernels.LoadData.LoadSnpAndPhenData(snpDataFile, phenDataFile, strainIdFile, leaveDataOnDisk, parallelOptions, out snpMatrix, out phenMatrix, out strainIds);


                ShoMatrix snpMatrixOrig = null;
                if (null != snpIdListToDo) snpMatrix = snpMatrix.SelectRowsView(snpIdListToDo);
                if (null != phenIdListToDo) phenMatrix = phenMatrix.SelectRowsView(phenIdListToDo);

                if (doEigImpNorm && snpMatrix != null)
                {
                    //snpMatrix = snpMatrix.AsShoMatrix().Standardize(SNP_ENCODING, parallelOptions.DegreeOfParallelism);//.AsShoMatrix();
                    snpMatrix = snpMatrix.AsShoMatrix().Standardize(SNP_ENCODING, parallelOptions);//.AsShoMatrix();
                }

                //fixed effects that we always want to learn
                Matrix<string, string, double> fixedEffectsMatrix = null;
                if (fixedEffectsFile != null)
                {
                    //fixedEffectsMatrix = ShoMatrix.CreateShoDoubleMatrixFromMatrixFile(fixedEffectsFile, parallelOptions.DegreeOfParallelism);
                    fixedEffectsMatrix = ShoMatrix.CreateShoMatrixFromMatrixFile(fixedEffectsFile, parallelOptions);
                    int numOrig = fixedEffectsMatrix.RowCount;
                    throw new NotImplementedException("is this correct? it needs to match what is done in LMM.exe, and just noticed that it looks like it might not be");
                    fixedEffectsMatrix = fixedEffectsMatrix.GetRowsThatVary();//don't want features that can't contribute information (i.e., are constant)
                    int numRemoved = numOrig - fixedEffectsMatrix.RowCount;
                    if (numRemoved > 0) _runInfoStr += "removed " + numRemoved + " fixed effects that had no variation\n";
                }

                List<Matrix<string, string, double>> kernelMatrixes = null;
                if (_kernelFiles.Count > 1)
                {
                    allowNonPosDefKernel = true;//false;//will mess things up because we need Cholesky then---I don't think this is true anymore, check
                }
                else
                {
                    indOfKernelInit = 0;
                }
                //numKernels = LoadData.ReadInKernelFiles(parallelOptions.DegreeOfParallelism, numKernels, null, out kernelMatrixes, allowNonPosDefKernel, _kernelFiles);
                numKernels = LoadData.ReadInKernelFiles(parallelOptions, numKernels, null, out kernelMatrixes, allowNonPosDefKernel, _kernelFiles, 0);
                string fileNameOfKcurrent = _kernelFiles[indOfKernelInit];

                List<string> snpIds = new List<string> { };
                if (null != snpMatrix) snpIds = snpMatrix.RowKeys.ToList();

                LoadData.CheckCidOrderMatchesOnAllData(snpMatrix, phenMatrix, fixedEffectsMatrix, kernelMatrixes);

                _numCids = (snpMatrix != null) ? snpMatrix.ColCount : phenMatrix.ColCount;
                _numSNP = (snpMatrix != null) ? snpMatrix.RowCount : 0;
                List<string> cidList = (snpMatrix != null) ? snpMatrix.ColKeys.ToList() : phenMatrix.ColKeys.ToList();

                DoubleArray fixedEffects = (fixedEffectsMatrix == null) ? null : fixedEffectsMatrix.AsShoMatrix().DoubleArray;

                string summaryOutputFile = "";
                TextWriter textWriterSummary = null;

                #endregion

                if (!COMPUTE_MSTEP_ON_CLUSTER)
                {
                    summaryOutputFile = ShoUtils.AddPostfixToFileName(outputFileOrNull, fileNamePostfix.TrimEnd(new char[] { 'Q' }) + "INFO");
                    Bio.Util.FileUtils.CreateDirectoryForFileIfNeeded(summaryOutputFile);
                    textWriterSummary = File.CreateText(summaryOutputFile);
                    textWriterSummary.WriteLine("num cids=" + _numCids + ", numSnp=" + _numSNP);
                    textWriterSummary.WriteLine(Helper.CreateDelimitedString(" ", args));
                    textWriterSummary.WriteLine(_dateStamp);
                }

                if (COMPUTE_MSTEP_ON_CLUSTER)
                {
                    ComputeNewKernelFromCurrentGuessUsingCluster(args, ref outputFileOrNull, indOfKernelInit, parallelOptions, inputTabFile, fileNamePostfix, ref phenMatrix, ref kernelMatrixes,
                        fixedEffects, tasks, taskCount);
                }
                else if (DO_FULL_KERNEL_LEARNING)
                {
                    DoFullKernelLearning(argCollectionOrig, textWriterSummary, ref outputFileOrNull, maxNumEMsteps, indOfKernelInit, parallelOptions, fileNamePostfix,
                        ref phenMatrix, ref kernelMatrixes, fixedEffects, tasks, taskCount, fileNameOfKcurrent, lmmTaskCount, maxFullLearnIt, batchSize, phenDataFile);
                }
                else //USE EM to DO KERNEL LEARNING, either locally, or setting up a cluster job
                {
                    throw new Exception("This code path is now obsolete");
                    //DoEmToLearnKernel(textWriterSummary, argCollectionOrig, ref outputFileOrNull, maxNumEMsteps, indOfKernelInit, parallelOptions, inputTabFile, fileNamePostfix, phenMatrix, kernelMatrixes, fixedEffects, tasks, taskCount, fileNameOfKcurrent);
                }
                Console.WriteLine("done eLMM");

                //textWriter.WriteLine(TaskCommandLine);


            }

            catch (Exception exception)
            {
                Console.WriteLine("");
                Console.WriteLine(exception.Message);
                if (exception.InnerException != null)
                {
                    Console.WriteLine(exception.InnerException.Message);
                }

                Console.WriteLine("");
                Console.WriteLine(UsageMessage);

                Console.WriteLine(exception.StackTrace);
                throw new Exception("", exception);
            }
            finally
            {
                Console.WriteLine("done LmmKernelMstep.exe");
            }
        }

        private static void DoFullKernelLearning(ArgumentCollection argCollectionOrig, TextWriter textWriterSummary, ref string outputFileOrNull,
                                                 int maxNumEMsteps, int indOfKernelInit, ParallelOptions parallelOptions,
                                                 string fileNamePostfix, ref Matrix<string, string, double> phenMatrix,
                                                 ref List<Matrix<string, string, double>> kernelMatrixes, DoubleArray fixedEffects,
                                                 RangeCollection tasks, int taskCount, string fileNameOfKcurrent, int lmmTaskCount, int maxFullAlgoIter, int batchSize, string phenFileOrig)
        {
            //int maxFullAlgoIter = 200;

            bool USE_CLUSTER = argCollectionOrig.PeekOptionalFlag("cluster") ? true : false;

            string rootClusterDir = "";
            if (USE_CLUSTER) rootClusterDir = new ClusterSubmitterArgs((ArgumentCollection)argCollectionOrig.Clone()).ExternalRemoteDirectoryName;

            bool USE_BATCHES = (batchSize == -1) ? false : true;
            Helper.CheckCondition(!USE_BATCHES || USE_CLUSTER, "not sure if batches are supported for local only---check");
            List<List<string>> genesForEachBatch = null;
            List<string> geneBatchFiles = new List<string>();
            int numBatches = 0;
            if (USE_BATCHES)
            {
                List<string> fullGeneList = phenMatrix.RowKeys.ToList();
                int M = fullGeneList.Count;
                numBatches = (int)Math.Floor((double)M) / batchSize;
                throw new NotImplementedException("need to fix this up as it disappeared");
                // genesForEachBatch = SpecialFunctions.DivideListIntoEqualChunksFromNumChunks(fullGeneList, numBatches);

                //write out the files once at the start, to the cluster
                for (int batchNum = 0; batchNum < numBatches; batchNum++)
                {
                    string batchPhenFileTmp = ShoUtils.AddPostfixToFileName(phenFileOrig, "_BATCH" + batchNum + "_" + numBatches + "_" + _dateStamp);
                    batchPhenFileTmp = rootClusterDir + @"\tmp\" + new FileInfo(batchPhenFileTmp).Name;
                    geneBatchFiles.Add(batchPhenFileTmp);
                    phenMatrix.SelectRowsView(genesForEachBatch[batchNum]).WriteDense(batchPhenFileTmp);
                }
            }
            int currentBatch = (USE_BATCHES) ? 0 : -1;

            //we never want these again, so take them off
            argCollectionOrig.ExtractOptionalFlag("doFullKernelLearning");
            argCollectionOrig.ExtractOptional<int>("batchSize", -1);
            argCollectionOrig.ExtractOptional<int>("maxFullLearnIt", 200);

            string currentKernelFile = ExtractNameOfInitKernelFromArgCollection(indOfKernelInit, argCollectionOrig);
            string currentTabFile = "";

            string name1 = "FastLMM.from.eLMM";
            string name2 = "Mstep.from.eLMM";

            int numK = kernelMatrixes.Count;
            List<SVD> svdList = new List<SVD>();
            for (int j = 0; j < numK; j++) svdList.Add(null);

            int numFinalFullIter = 5;
            for (int itFull = 0; itFull < maxFullAlgoIter; itFull++)
            {
                Console.WriteLine("----iteration=" + itFull + " of " + maxFullAlgoIter + "----------------------------");
                if (itFull == maxFullAlgoIter - numFinalFullIter) USE_BATCHES = false;//do last one with all genes
                //--------------------------------------------
                //(1)submit a LMM.exe job to the cluster, using current kernel estimate, and wait until done
                ArgumentCollection argCollectionLearner = (ArgumentCollection)argCollectionOrig.Clone();

                //strip off stuff we don't want for FastLMM.exe                
                argCollectionLearner.ExtractOptional<string>("tasks", "");
                argCollectionLearner.ExtractOptional<string>("inputTabFile", "");
                argCollectionLearner.ExtractOptional<string>("maxNumMsteps", "");
                argCollectionLearner.ExtractOptional<string>("numEMit", "");
                argCollectionLearner.ExtractOptional<string>("numIt", "");
                argCollectionLearner.ExtractOptional<string>("indOfKernelInit", "");
                argCollectionLearner.ExtractOptional<string>("lmmTaskCount", "");
                argCollectionLearner.ExtractOptional<string>("lmmTaskCount", "");

                string iterPostfix = ".ITF" + itFull;
                string tmpDateStamp = _dateStamp + iterPostfix;
                argCollectionLearner.ForceOptional<string>("dateStamp", tmpDateStamp);
                argCollectionLearner.ForceOptional<string>("predictorTargetPairFilter", "NullOnly");
                argCollectionLearner.ForceOptional<COV_LEARN_TYPE>("covLearnType", COV_LEARN_TYPE.ONCE);

                if (USE_CLUSTER)
                {
                    argCollectionLearner.ForceOptional<string>("taskCount", lmmTaskCount.ToString());
                    argCollectionLearner.ForceOptional("cleanup", true);
                    argCollectionLearner.ForceOptional<int>("MaxSubmitTries", 10);
                    argCollectionLearner.ForceOptional<string>("ExeName", "FastLMM.exe");
                    if (!USE_BATCHES) argCollectionLearner.ForceOptional<string>("name", name1 + "." + tmpDateStamp);
                    else argCollectionLearner.ForceOptional<string>("name", name1 + "." + tmpDateStamp + "S" + currentBatch);
                }

                ReplaceKernelInitFileWithNewKernelInitFile(indOfKernelInit, currentKernelFile, argCollectionLearner);

                string outFile, outInfoFile;

                argCollectionLearner.Add("null");//NO_SNP_DATA_FILE");

                if (USE_BATCHES)
                {//change the phenotpye file that is being called
                    Console.WriteLine("working on batch " + currentBatch);
                    argCollectionLearner.ForceOptional<string>("phenDataFile", geneBatchFiles[currentBatch]);
                }
                outFile = ShoUtils.AddPostfixToFileName(outputFileOrNull.Replace("Results", ""), "_" + tmpDateStamp + "Tab");
                outInfoFile = ShoUtils.AddPostfixToFileName(outputFileOrNull.Replace("Results", ""), "_" + tmpDateStamp + "INFO");


                Console.WriteLine(argCollectionLearner);

                //=====================================
                string localDirectory = Environment.CurrentDirectory;
                string resultDirectory = rootClusterDir + @"\Results\";
                string clusterInputDir = rootClusterDir + @"\Input\";
                string copyToDirectory = localDirectory + @"\Input\";

                //CALL FastLMM.exe
                if (USE_CLUSTER)
                {//run on cluster                    
                    ClusterSubmitterArgs clusterSubmitterArgsLmm = ClusterSubmitter.SubmitAndWait(argCollectionLearner);
                    //also want to copy it to cluster so that don't rely on wildcards in clusterCopy file commands
                    //currentTabFile = MoveOrCopyFiles(resultDirectory, copyToDirectory, new List<string> { outFile, outInfoFile }, true)[0];
                    //currentTabFile = MoveOrCopyFiles(resultDirectory, clusterInputDir, new List<string> { outFile, outInfoFile }, false/*doCopy*/)[0];
                    currentTabFile = resultDirectory + outFile;//MoveOrCopyFiles(resultDirectory, clusterInputDir, new List<string> { outFile, outInfoFile }, false/*doCopy*/)[0];
                }
                else
                {//run locally                           
                    /*ideally we would not call things in this way, because it requires re-loading up all of the data, etc.*/
                    FastLMMArgs myArgs = FastLMMArgs.Parse(argCollectionLearner);
                    FastLMM.FastLMM.LmmGwas(myArgs);

                    currentTabFile = localDirectory + @"\" + myArgs.finalTabulatedFile;
                }
                //=====================================


                if (itFull == maxFullAlgoIter - 1) break;//don't want to do step (2) last as we won't have the log likelihood
                //--------------------------------------------
                //(2)run this EM learning program using the output from (1)
                Helper.CheckCondition(File.Exists(currentTabFile), "file: " + currentTabFile + " does not exist, so cannot do eLMM.exe");

                //don't want the possiblity of clobbering stuff in my Input directory
                //currentTabFile = @"Input\" + new FileInfo(currentTabFile).Name;

                ArgumentCollection argCollectionEM = (ArgumentCollection)argCollectionOrig.Clone();
                argCollectionEM.ForceOptional<string>("inputTabFile", currentTabFile);
                argCollectionEM.ForceOptional<string>("dateStamp", tmpDateStamp);
                if (!USE_BATCHES) argCollectionEM.ForceOptional<string>("name", name2 + "." + tmpDateStamp);
                else argCollectionEM.ForceOptional<string>("name", name2 + "." + tmpDateStamp + "S" + currentBatch);

                ReplaceKernelInitFileWithNewKernelInitFile(indOfKernelInit, currentKernelFile, argCollectionEM);

                string outputFileEM = ShoUtils.AddPostfixToFileName(outputFileOrNull, ".cov");
                kernelMatrixes[indOfKernelInit] = LoadData.CreateMatrixFromMatrixFile(currentKernelFile, parallelOptions);

                if (USE_BATCHES)
                {
                    argCollectionEM.ForceOptional<string>("phenDataFile", geneBatchFiles[currentBatch]);
                }
                Console.WriteLine("calling DoEmToLearnKernel for fullKernelLearning, itFull=" + itFull);

                string currentKernelFileTmp = @"Input\" + new FileInfo(currentKernelFile).Name;
                //=====================================
                DoEmToLearnKernel(textWriterSummary, argCollectionEM, ref outputFileEM, maxNumEMsteps, indOfKernelInit, parallelOptions, currentTabFile,
                    "_" + tmpDateStamp, (USE_BATCHES) ? phenMatrix.SelectRowsView(genesForEachBatch[currentBatch]) : phenMatrix,
                   kernelMatrixes, fixedEffects, tasks, taskCount, currentKernelFileTmp, ref svdList);
                //=====================================

                if (USE_CLUSTER)
                {
                    string copyFromDirectory = localDirectory + @"\Results\";
                    string localInputDirectory = localDirectory + @"\Input\";
                    MoveOrCopyFile(copyFromDirectory, clusterInputDir, new FileInfo(outputFileEM).Name, true);
                    MoveOrCopyFile(copyFromDirectory, localInputDirectory, new FileInfo(outputFileEM).Name, false);

                    currentKernelFile = rootClusterDir + @"\Input\" + new FileInfo(outputFileEM).Name;
                }
                else
                {
                    currentKernelFile = localDirectory + @"\Results\" + new FileInfo(outputFileEM).Name;
                }

                if (USE_BATCHES)
                {
                    currentBatch++;
                    if (currentBatch == numBatches) currentBatch = 0;
                }
            }
        }

        private static ShoMatrix DoEmToLearnKernel(TextWriter textWriterSummary, ArgumentCollection argCollectionOrig, ref string outputFileOrNull,
                                                   int maxNumEMiterations, int indOfKernelToLearn, ParallelOptions parallelOptions, string inputTabFile,
                                                   string fileNamePostfix, Matrix<string, string, double> phenMatrix,
                                                   List<Matrix<string, string, double>> kernelMatrixes, DoubleArray fixedEffects,
                                                   RangeCollection tasks, int taskCount, string fileNameOfKcurrent, ref List<SVD> svd)
        {
            int REPORT_FREQ;
            Stopwatch st2;
            ShoMatrix finalKernel, tmpPhenMatrix;
            List<string> phenIdAll, snpIdAll, colKeys;
            DoubleArray sigmaE2All, sigmaG2All, biasAll, Kcurrent, logLikes;
            List<DoubleArray> fixedEffectParams, kernelWeights, allKernels;

            logLikes = new DoubleArray(1, maxNumEMiterations);

            string outputFileOrNullOrig = outputFileOrNull;
            outputFileOrNull = ShoUtils.AddPostfixToFileName(outputFileOrNull, fileNamePostfix);
            SetUpEm(indOfKernelToLearn, inputTabFile, fileNamePostfix, phenMatrix, ref kernelMatrixes, out REPORT_FREQ, out st2, out finalKernel,
                out allKernels, out Kcurrent, out colKeys, out tmpPhenMatrix, out phenIdAll, out snpIdAll, out sigmaE2All, out sigmaG2All, out biasAll, out fixedEffectParams, out kernelWeights);

            //string fileNameOfKcurrent = ArgumentCollection.PeekOptional<string>("k" + indOfKernelInit, "");

            int N = phenMatrix.ColCount;
            int M = phenMatrix.RowCount;                       

            _runInfoStr += "Log likes:\n";
            //************************EM iterations**********************************//
            for (int it = 0; it < maxNumEMiterations; it++)
            {
                ArgumentCollection argumentCollection = (ArgumentCollection)argCollectionOrig.Clone();

                if (allKernels != null)
                {
                    //not sure if I need this or not, but I think not, so removed it... 4/27/2010
                    //Helper.CheckCondition(Kcurrent.TryCholesky()==null, "cholesky of kernel failed");
                    allKernels[indOfKernelToLearn] = Kcurrent;
                }

                if (_COMPUTE_LOG_LIKELIHOODS)
                {
                    Console.Write("working on it=" + it + " of EM for learning kernel, loglikelihood=...");
                    logLikes[it] = ComputeLogLikelihoodOfLmm(phenMatrix, phenIdAll, snpIdAll, sigmaE2All, sigmaG2All, biasAll,
                                    Kcurrent, fixedEffects, fixedEffectParams, allKernels, kernelWeights, parallelOptions);
                    Console.WriteLine(logLikes[it]);
                    _runInfoStr += logLikes[it] + "\n";
                }
                else
                {
                    Console.Write("working on it=" + it + " of EM for learning kernel");
                    logLikes[it] = double.NaN;
                }
                Console.WriteLine("");

                if (it > 0 && _COMPUTE_LOG_LIKELIHOODS)
                {
                    double diff = logLikes[it] - logLikes[it - 1];
                    Helper.CheckCondition(diff >= 0, "log likelihood went down from: " + logLikes[it - 1] + " to " + logLikes[it]);
                }

                DoubleArray tmpNewK = ShoUtils.DoubleArrayZeros(N, N);
                               

                //===============================================                
                bool USE_CLUSTER = argumentCollection.PeekOptionalFlag("cluster") ? true : false;
                if (USE_CLUSTER)
                {
                    ArgumentCollection tmpArgCollection = (ArgumentCollection)argumentCollection.Clone();
                    string extraBit = "it" + it;
                    tmpArgCollection.ForceOptional("name", "eLMM." + fileNamePostfix + extraBit);
                    string tmpPostfix = (fileNamePostfix + extraBit).Substring(1);//Replace("_", "");
                    tmpArgCollection.ForceOptional<string>("dateStamp", tmpPostfix);

                    tmpArgCollection.ForceOptionalFlag("QUIET");
                    tmpArgCollection.ForceOptional("cleanup", true);

                    bool isExclusive = tmpArgCollection.PeekOptionalFlag("isExclusive");
                    if (!isExclusive) tmpArgCollection.ForceOptional<int>("MaxDegreeOfParallelism", 1);
                    tmpArgCollection.ForceOptionalFlag("computeMStepOnCluster");
                    tmpArgCollection.ForceOptional<string>("outputFile", outputFileOrNullOrig);
                    tmpArgCollection.ForceOptional<string>("inputTabFile", inputTabFile);

                    //substitute in the current guess of the kernel, which we have written to file
                    //hack fix because of bad naming conventions:
                    ReplaceKernelInitFileWithNewKernelInitFile(indOfKernelToLearn, fileNameOfKcurrent, tmpArgCollection);

                    //submit and wait--------------------------------------
                    tmpArgCollection.ForceOptional<int>("MaxSubmitTries", 100);
                    ClusterSubmitterArgs clusterSubmitterArgs = ClusterSubmitter.SubmitAndWait(tmpArgCollection);
                    //-----------------------------------------------------

                    string rootClusterDir = clusterSubmitterArgs.ExternalRemoteDirectoryName;
                    string resultDirectory = rootClusterDir + @"\Results\";
                    string copyToDirectory = rootClusterDir + @"\Input\";//Environment.CurrentDirectory + @"\Input";
                    string outFile = ShoUtils.AddPostfixToFileName(outputFileOrNullOrig.Replace("Results", ""), "_" + tmpPostfix);
                    outFile = ShoUtils.AddPostfixToFileName(outFile, "Tab");
                    fileNameOfKcurrent = MoveOrCopyFile(resultDirectory, copyToDirectory, outFile, false);
                    Console.WriteLine("next kernel file: " + fileNameOfKcurrent);

                    Kcurrent = LoadData.CreateMatrixFromMatrixFile(fileNameOfKcurrent, parallelOptions).AsShoMatrix().DoubleArray;
                    //Helper.CheckCondition(Kcurrent.TryCholesky().Succeeded, "cholesky of kernel failed");//this is done elsewhere and it's too expensive to do it here too
                    //now delete it as we no longer need it
                    //MBT.Escience.FileUtils.DeleteFilesPatiently(new List<FileInfo> { new FileInfo(fileNameOfKcurrent) }, 5, new TimeSpan(0, 1, 0));
                }
                else
                {//run locally     
                    GetSvdOfKernelsIfNeeded(allKernels, N, ref svd, indOfKernelToLearn);
                    //==========================================
                    //when doing it not on the cluster
                    DoubleArray KcurrentOld = Kcurrent;
                    Kcurrent = ComputeNewKernel(indOfKernelToLearn, parallelOptions, phenMatrix, fixedEffects, allKernels,
                        N, M, phenIdAll, snpIdAll, sigmaE2All, sigmaG2All, biasAll, fixedEffectParams, kernelWeights, svd);
                    double maxVal = (Kcurrent - KcurrentOld).ToVector().Abs().Max();
                    Console.WriteLine("maxVal=" + maxVal);
                    //==============================================================
                }

                if (REPORT_FREQ != 0 && (it % REPORT_FREQ) == 0)
                {
                    st2.Stop();
                    Console.WriteLine("***Finished M-step iteration " + it + ", Elapsed = {0}", st2.Elapsed.ToString());
                    System.Console.Out.Flush();
                    st2.Start();
                }

                textWriterSummary.WriteLine(_runInfoStr); _runInfoStr = "";
                textWriterSummary.Flush();

                finalKernel = new ShoMatrix(Kcurrent, phenMatrix.ColKeys, phenMatrix.ColKeys, double.NaN);
                //if ((it % 5) == 0)
                //{
                //    finalKernel.WriteDense(ShoUtils.AddPostfixToFileName(outputFileOrNull, "iter" + it));
                //}
                //finalKernel.WriteDense(outputFileOrNull);
            }
            finalKernel.WriteDense(outputFileOrNull);
            if (_COMPUTE_LOG_LIKELIHOODS) Console.WriteLine(logLikes.ToString());//.ToString2());
            return finalKernel;
        }

        private static void ReplaceKernelInitFileWithNewKernelInitFile(int indOfKernelInit, string fileNameOfKcurrent, ArgumentCollection argumentCollection)
        {

            int kernelNum = indOfKernelInit + 1;
            string oldInitFile = ExtractNameOfInitKernelFromArgCollection(indOfKernelInit, argumentCollection);
            argumentCollection.AddOptional("k" + kernelNum, fileNameOfKcurrent);
            Console.WriteLine("Replacing " + "-k" + kernelNum + "  " + oldInitFile + " with -k" + kernelNum + " " + fileNameOfKcurrent);
        }

        private static string ExtractNameOfInitKernelFromArgCollection(int indOfKernelInit, ArgumentCollection argumentCollection)
        {

            int kernelNum = indOfKernelInit + 1;
            string initKernelFile = argumentCollection.ExtractOptional<string>("k" + kernelNum, "");
            return initKernelFile;
        }

        private static string ExtractNameOfInitKernelFromArgCollectionAlternativeSplicing(ArgumentCollection argumentCollection)
        {
            string initKernelFile = argumentCollection.ExtractOptional<string>("ehKernel", "");
            return initKernelFile;
        }

        private static List<string> MoveOrCopyFiles(string resultDirectory, string copyToDirectory, List<string> outputFiles, bool doCopy)
        {
            List<string> newNames = new List<string>();
            foreach (string tmpFile in outputFiles) newNames.Add(MoveOrCopyFile(resultDirectory, copyToDirectory, tmpFile, doCopy));
            return newNames;
        }

        private static string MoveOrCopyFile(string resultDirectory, string copyToDirectory, string outputFileOrNull, bool doCopy)
        {
            string fromFile = resultDirectory + outputFileOrNull;
            string toFile = copyToDirectory + outputFileOrNull;
            Bio.Util.FileUtils.CreateDirectoryForFileIfNeeded(toFile);
            try
            {
                if (File.Exists(toFile)) File.Delete(toFile);
                if (doCopy)
                {
                    File.Copy(fromFile, toFile);
                }
                else
                {
                    File.Move(fromFile, toFile);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("could not copy/move file from: " + fromFile + " to " + toFile);
            }
            return toFile;
        }

        //get required SVDs---ideally, would cache the SVD for the "context" kernel that is not changing... but too lazy to change that now
        private static void GetSvdOfKernelsIfNeeded(List<DoubleArray> allK, int N, ref List<SVD> svd, int indOfKernelToLearn)
        {
            Helper.CheckCondition(_cushionThatFallsOutExactlyButMakesComputationStable == 0, "this part can no longer acccept this param");
            int numK = allK.Count;
            //use emma trick to efficiently compute posterior covariance, C, for each gene
            for (int k = 0; k < numK; k++)
            {
                SVD tmpsvd;
                if (k == indOfKernelToLearn || svd[k] == null)
                {                    
                    svd[k] = new SVD(allK[k]); //+ _cushionThatFallsOutExactlyButMakesComputationStable * ShoUtils.Identity(N, N));
                }
            }
        }

        private static void SetUpEm(int indOfKernelInit, string inputTabFile, string fileNamePostfix, Matrix<string, string, double> phenMatrix,
                                    ref List<Matrix<string, string, double>> kernelMatrixes, out int REPORT_FREQ, out Stopwatch st2, out ShoMatrix finalKernel,
                                    out List<DoubleArray> allKernels, out DoubleArray Kcurrent, out List<string> colKeys,
                                    out ShoMatrix tmpPhenMatrix, out List<string> phenIdAll, out List<string> snpIdAll, out DoubleArray sigmaE2All,
                                    out DoubleArray sigmaG2All, out DoubleArray biasAll, out List<DoubleArray> fixedEffectParams, out List<DoubleArray> kernelWeights)
        {
            //Right now, assumes a NullOnly run
            REPORT_FREQ = 1;
            int REPORT_FREQ2 = 1000;

            st2 = new Stopwatch();
            st2.Start();

            finalKernel = null;

            Dictionary<string, int> numTimesPhenSeen = new Dictionary<string, int>();

            DoubleArray Kinit = kernelMatrixes[indOfKernelInit].AsShoMatrix().DoubleArray;

            allKernels = new List<DoubleArray>();
            for (int k = 0; k < kernelMatrixes.Count; k++)
            {
                allKernels.Add(kernelMatrixes[k].AsShoMatrix().DoubleArray);
            }
            kernelMatrixes = null;//so can't use it anymore
            //if (allKernels.Count == 1) { allKernels = null; }//don't want this if have no fixed kernels during learning

            Kcurrent = Kinit;

            int N = Kinit.size0;
            int M = phenMatrix.RowCount;

            colKeys = phenMatrix.ColKeys.ToList();

            tmpPhenMatrix = phenMatrix.AsShoMatrix();

            string header = ExtractParamsFromTabulatedFile(inputTabFile, numTimesPhenSeen, M, N, out phenIdAll, out snpIdAll, out sigmaE2All,
                                                           out sigmaG2All, out biasAll, out fixedEffectParams, out kernelWeights);
        }

        private static void ComputeNewKernelFromCurrentGuessUsingCluster(string[] args, ref string outputFileOrNull, int indOfKernelToLearn, ParallelOptions parallelOptions,
            string inputTabFile, string fileNamePostfix, ref Matrix<string, string, double> phenMatrix, ref List<Matrix<string, string, double>> kernelMatrixes,
            DoubleArray fixedEffects, RangeCollection tasks, int taskCount)
        {
            int REPORT_FREQ;
            Stopwatch st2;
            ShoMatrix finalKernel, tmpPhenMatrix;
            List<string> phenIdAll, snpIdAll, colKeys;
            DoubleArray sigmaE2All, sigmaG2All, biasAll, Kcurrent;
            List<DoubleArray> fixedEffectParams, kernelWeights, allKernels;
            TextWriter textWriterSummary;

            SetUpEm(indOfKernelToLearn, inputTabFile, fileNamePostfix, phenMatrix, ref kernelMatrixes, out REPORT_FREQ, out st2, out finalKernel, out allKernels, out Kcurrent, out colKeys, out tmpPhenMatrix, out phenIdAll, out snpIdAll, out sigmaE2All, out sigmaG2All, out biasAll, out fixedEffectParams, out kernelWeights);

            int N = phenMatrix.ColCount;
            int M = phenMatrix.RowCount;

            string completedRowsFileName, finalOutputFileName, uniqueId;
            outputFileOrNull = InitializeForClusterFileMerge(outputFileOrNull, tasks, taskCount, fileNamePostfix, out completedRowsFileName, out finalOutputFileName, out uniqueId);

            List<SVD> svd = new List<SVD>();
            for (int j = 0; j < allKernels.Count; j++) svd.Add(null);
            GetSvdOfKernelsIfNeeded(allKernels, N, ref svd, indOfKernelToLearn);

            var workUnitsToDo = SpecialFunctions.DivideWorkIntoContiguousItems(Enumerable.Range(0, M), tasks, taskCount, M, _skipList);
            using (SynchronizedRangeCollectionWrapper completedRows =
                               SynchronizedRangeCollectionWrapper.GetInstance(completedRowsFileName, taskCount))
            {
                var kernelUpdatesFromAllGenes =
                from g in workUnitsToDo.Select(x => x.Key)//.AsParallel()//.WithDegreeOfParallelism(parallelOptions.MaxDegreeOfParallelism)
                select ComputeKernelUpdateForOneGene(tmpPhenMatrix, N, phenIdAll, snpIdAll, sigmaE2All, sigmaG2All, biasAll, fixedEffects, fixedEffectParams, svd,
                                             kernelWeights, allKernels, indOfKernelToLearn, g);

                Kcurrent = new DoubleArray(Kcurrent.size0, Kcurrent.size1);
                foreach (DoubleArray kUpdate in kernelUpdatesFromAllGenes)
                {
                    Kcurrent += kUpdate;
                    Helper.CheckCondition(!Kcurrent.HasNaN(), "Kcurrent has NaN");
                }
                //average the matrixes that this task was responsible for
                //Console.WriteLine("dividing by " + kernelUpdatesFromAllGenes.Count() + " to normalize accumulated matrixes");
                //Kcurrent = Kcurrent;//do the division during merging//workUnitsToDo.Count();//M;

                if (!tasks.IsEmpty)
                {
                    ShoMatrix tmpMatrix = new ShoMatrix(Kcurrent, colKeys, colKeys, double.NaN);
                    Helper.CheckCondition(!tmpMatrix.DoubleArray.HasNaN(), "matrix has NaNs: " + outputFileOrNull);
                    ShoUtils.Pickle(tmpMatrix.DoubleArray, ShoUtils.AddPostfixToFileName(outputFileOrNull, _specialString));
                    completedRows.Add(tasks);//unlike when using Tabulated files, we just add all these pieces at once, because they have already been summarized into one Kcurrent matrix
                }

                if (_CONSOLIDATE_RESULTS_AUTOMATICALLY)
                {
                    string filePatternToMerge = uniqueId.Replace(".txt", "");//_dateStamp;
                    bool averageAccumulatedMatrixes = true;
                    double numToDivideBy = (double)M;
                    double diagFudge = 1.0e-5;
                    RunCONDITIONALTabulate(_CLEANUP, finalOutputFileName, fileNamePostfix, completedRowsFileName, completedRows, colKeys, filePatternToMerge, numToDivideBy, diagFudge);
                }
            }
        }

        private static DoubleArray ComputeNewKernel(int indOfKernelInit, ParallelOptions parallelOptions, Matrix<string, string, double> phenMatrix, DoubleArray fixedEffects,
                                                    List<DoubleArray> allKernels, int N, int M, List<string> phenIdAll, List<string> snpIdAll, DoubleArray sigmaE2All,
                                                    DoubleArray sigmaG2All, DoubleArray biasAll, List<DoubleArray> fixedEffectParams, List<DoubleArray> kernelWeights,
                                                    List<SVD> svd)
        {
            //Carl's trick for doing accumulation in a thread-safe manner-------
            var tmpPhenMatrix = phenMatrix;//so compiler doesn't complain
            var kernelUpdatesFromAllGenes =
                from g in Enumerable.Range(0, M).AsParallel().WithDegreeOfParallelism(parallelOptions.MaxDegreeOfParallelism)
                select ComputeKernelUpdateForOneGene(tmpPhenMatrix, N, phenIdAll, snpIdAll, sigmaE2All, sigmaG2All, biasAll, fixedEffects, fixedEffectParams, svd,
                                                     kernelWeights, allKernels, indOfKernelInit, g);

            //var kernelUpdatesFromAllGenes =
            //                      from g in Enumerable.Range(0, M).AsParallel().WithDegreeOfParallelism(parallelOptions.DegreeOfParallelism)
            //                      select ComputeKernelUpdateForOneGene(phenMatrix, N, phenIdAll, snpIdAll, sigmaE2All, sigmaG2All, biasAll, fixedEffects, fixedEffectParams, svd,
            //                                                           diagVals, kernelWeights, allKernels, indOfKernelInit,invCurrentK, g);

            DoubleArray Kcurrent = new DoubleArray(N, N);
            foreach (DoubleArray kUpdate in kernelUpdatesFromAllGenes)
            {
                Kcurrent += kUpdate;
            }
            Kcurrent = Kcurrent / M;
            return Kcurrent;
        }


        private static DoubleArray ComputeKernelUpdateForOneGene(Matrix<string, string, double> phenMatrix, int N, List<string> phenIdAll,
                                               List<string> snpIdAll, DoubleArray sigmaE2All, DoubleArray sigmaG2All, DoubleArray biasAll,
                                               DoubleArray fixedEffects, List<DoubleArray> fixedEffectWeights, List<SVD> allSvd,
                                               List<DoubleArray> kernelWeights, List<DoubleArray> allKernels, int indOfKernelInit, int g)
        {
            //Console.WriteLine("ComputeKernelUpdateForOneGene, g=" + g);

            string phenId = phenIdAll[g];
            string SNPId = snpIdAll[g];
            double sigmaE2 = sigmaE2All[g];
            double sigmaG2 = sigmaG2All[g];
            double bias = biasAll[g];

            DoubleArray dat = phenMatrix.SelectRowsView(phenId).AsShoMatrix().DoubleArray;
            DoubleArray XB = GetXBterm(N, fixedEffects, fixedEffectWeights[g], bias);
            DoubleArray yMinusXbeta = dat.T - XB.T;

            DoubleArray tmpInv = sigmaG2 / sigmaE2 * ShoUtils.Identity(N, N);

            DoubleArray C, c;
            if (allKernels.Count == 1)//there was only one kernel
            {//use Emma trick for efficiency
                //DoubleArray diagTermsInv = ShoUtils.MakeDiag(1.0 / (allDiagVals[0] + sigmaG2 / sigmaE2));
                DoubleArray diagTermsInv = ShoUtils.MakeDiag(1.0 / (allSvd[0].D.Diagonal + sigmaG2 / sigmaE2));
                //C = allSvd[0].U.Multiply(diagTermsInv).Multiply(allSvd[0].U.T);//this used to be the svd of the inverse
                C = allSvd[0].U.T.Multiply(diagTermsInv).Multiply(allSvd[0].U);//effectively taking the inverse, plus modifications to the diagonal
                c = C.Multiply(tmpInv).Multiply(yMinusXbeta) / Math.Sqrt(sigmaG2);
            }
            else
            {
                if (allKernels.Count > 2) throw new NotImplementedException("this solution is hard coded for a total of two kernels, not more, but I think the math extends naturally");

                //follow the notation in my hand-written notes for the case of 2 kernels, where K1 is the kernel we're learning, and K2 is the one that is fixed
                int indOfFixedKernel = 1 - indOfKernelInit;
                double kernelWeight1 = kernelWeights[g][indOfKernelInit];
                double kernelWeight2 = kernelWeights[g][indOfFixedKernel];
                DoubleArray K1 = allKernels[indOfKernelInit];
                DoubleArray K2 = allKernels[indOfFixedKernel];

                DoubleArray k1Inv = ShoUtils.InvertFromSvd(allSvd[indOfKernelInit]);//allkInv[indOfKernelInit];                
                //DoubleArray k2Inv = allkInv[indOfFixedKernel];//don't need this

                DoubleArray a = yMinusXbeta / Math.Sqrt(sigmaG2 * kernelWeight1);

                //use Emma trick to compute inv(A) instead
                //DoubleArray A = (ShoUtils.Identity(N, N) * sigmaE2 + kernelWeight2 * sigmaG2 * K2) / (sigmaG2 * kernelWeight1);
                //is this stable even when A would fail on Cholesky?
                //DoubleArray Ainv = A.Inv();
                //should be able to use Emma trick here too
                //DoubleArray Ainv = ShoUtils.InvFromChol(new Cholesky(A));
                double aa = sigmaE2;
                double bb = kernelWeight2 * sigmaG2;
                double cc = sigmaG2 * kernelWeight1;
                double extraInvDiag = aa / bb;
                //DoubleArray diagTermsInv = ShoUtils.MakeDiag(1.0 / (allDiagVals[indOfFixedKernel] + extraInvDiag));
                DoubleArray diagTermsInv = ShoUtils.MakeDiag(1.0 / (allSvd[indOfFixedKernel].D.Diagonal + extraInvDiag));
                DoubleArray tmpU = allSvd[indOfFixedKernel].U;
                DoubleArray Ainv = cc / bb * tmpU.Multiply(diagTermsInv).Multiply(tmpU.T);
                //DoubleArray AinvOld = A.Inv();
                //Console.WriteLine(AinvOld);

                //is this stable even when A would fail on Cholesky?
                //C=(kInv+Ainv).Inv();
                //C = ShoUtils.InvFromChol(new Cholesky(kInv + Ainv));
                C = (k1Inv + Ainv).InverseFromCholesky();//ShoUtils.InvFromChol(new Cholesky(k1Inv + Ainv));
                Helper.CheckCondition(!C.HasNaN(), "C has Nan initially");
                c = C.Multiply(Ainv).Multiply(a);
            }

            for (int n1 = 0; n1 < N; n1++)
            {
                for (int n2 = n1; n2 < N; n2++)
                {
                    C[n1, n2] += c[n1] * c[n2];
                    C[n2, n1] = C[n1, n2];
                }
            }
            bool ChasNan = C.HasNaN();
            if (ChasNan)
            {
                string errorFile = Environment.CurrentDirectory + @"\" + "cWithNaN" + g + ".csv";
                throw new NotImplementedException("This doesn't compile since the move to .NET 4. PRobably change to WriteToCSVBaseDirector");
                //C.WriteToCsv(errorFile);
                Console.WriteLine("wrote C with NaN's to " + errorFile);
            }
            Helper.CheckCondition(!C.HasNaN(), "C has Nan for g=" + g);
            return C;
        }

        private static DoubleArray GetXBterm(int N, DoubleArray fixedEffects, DoubleArray fixedEffectWeights, double bias)
        {
            DoubleArray XB = null;
            if (fixedEffects != null)
            {
                XB = fixedEffectWeights.Multiply(fixedEffects) + bias;
            }
            else
            {
                XB = ShoUtils.DoubleArrayOnes(1, N) * bias;
            }
            return XB;
        }

        private static double ComputeLogLikelihoodOfLmm(Matrix<string, string, double> phenMatrix, List<string> phenIdAll, List<string> snpIdAll, DoubleArray sigmaE2All, DoubleArray sigmaG2All, DoubleArray biasAll, DoubleArray K, DoubleArray fixedEffects,
                                List<DoubleArray> fixedEffectsParams, List<DoubleArray> allKernels, List<DoubleArray> kernelWeights, ParallelOptions parallelOptions)
        {
            //Use the Emma version of log likelihood so that can do it efficiently. This means writing it in terms of H, and using SVD
            int N = phenMatrix.ColCount;
            int M = phenMatrix.RowCount;

            double fudge = 10.0;
            SVD svdK = null;
            DoubleArray diagTerms = null;
            if (allKernels.Count == 1)
            {
                svdK = new SVD(K + fudge * ShoUtils.Identity(N, N));
                diagTerms = svdK.D.Diagonal - fudge;
            }

            DoubleArray priorCushion = null;//ShoUtils.Identity(N, N) * _multiKernelFudgeToPassCholesky;

            DoubleArray allLL = new DoubleArray(1, M);
            //Console.WriteLine("starting");
            Parallel.For(0, M, parallelOptions, m =>
            {
                //Console.WriteLine(m);

                DoubleArray XB = GetXBterm(N, fixedEffects, fixedEffectsParams[m], biasAll[m]);
                DoubleArray y = phenMatrix.SelectRowsView(phenIdAll[m]).AsShoMatrix().DoubleArray;
                DoubleArray yMinusXBeta = y - XB;

                double delta = sigmaE2All[m] / sigmaG2All[m];

                DoubleArray Hinv;
                double logDetH;
                if (allKernels.Count == 1)
                {
                    DoubleArray invDiagVector = (diagTerms + delta).DotInv();
                    DoubleArray invDiagMatrix = ShoUtils.MakeDiag(invDiagVector);
                    Hinv = svdK.U.Multiply(invDiagMatrix).Multiply(svdK.U.T);
                    logDetH = (diagTerms + delta).Log().Sum();
                }
                else
                {
                    DoubleArray thisGeneK = ShoUtils.GetLinearComboOfDoubleArrays(allKernels, kernelWeights[m]);
                    //thisGeneK += priorCushion;
                    DoubleArray H = ShoUtils.Identity(N, N) * delta + thisGeneK;
                    Cholesky cholH = H.TryCholesky();
                    if (cholH == null)
                    {
                        Console.WriteLine("Cholesky failed");
                        throw new Exception("Cholesky failed");
                    }
                    Hinv = ShoUtils.InvFromChol(cholH);
                    logDetH = ShoUtils.LogDetFromChol(cholH);
                }

                double R = yMinusXBeta.Multiply(Hinv).Multiply(yMinusXBeta.T).ToScalar();
                double newTerm = -0.5 * (N * Math.Log(2 * Math.PI * sigmaG2All[m]) + logDetH + R / sigmaG2All[m]);
                allLL[m] = newTerm;
            }
            );
            //Console.WriteLine("done parallel loop");
            return allLL.Sum();
        }

        private static string ExtractParamsFromTabulatedFile(string inputTabFile, Dictionary<string, int> numTimesPhenSeen, int M, int N,
                               out List<string> phenIdAll, out List<string> snpIdAll, out DoubleArray sigmaE2All,
                               out DoubleArray sigmaG2All, out DoubleArray biasAll, out List<DoubleArray> fixedEffectParams, out List<DoubleArray> kernelWeights)
        {
            Console.WriteLine("extracting info from tab file");

            phenIdAll = new List<string>(M);
            snpIdAll = new List<string>(M);
            sigmaE2All = new DoubleArray(1, M);
            sigmaG2All = new DoubleArray(1, M);
            biasAll = new DoubleArray(1, M);
            fixedEffectParams = new List<DoubleArray>(M);
            kernelWeights = new List<DoubleArray>(M);

            string header;
            bool haveCheckedForOptionalParams = false;
            int numFixedEffects = 0;
            int numKernelWeights = 0;
            int rowCount = -1;
            int m = 0;//line number being parsed
            foreach (Dictionary<string, string> row in SpecialFunctions.TabFileTable(inputTabFile, true, out header))
            {
                //AlternativeSplicing produces this header, different from LMM.exe which contains "nullBias" (and other stuff)
                //rowIndex	rowCount	nullIndex	probeId	transcriptClusterId	logLikeAlt	logLikeNull	pValue	effect	nullResidualVar	SNPId	#Markers	 nullGeneticVar	qValue

                rowCount = int.Parse(row["rowCount"]);

                snpIdAll.Add(row["SNPId"]);
                sigmaE2All[m] = double.Parse(row["nullResidualVar"]);
                try { sigmaG2All[m] = double.Parse(row["nullGeneticVar"]); }
                catch { sigmaG2All[m] = double.Parse(row["nullGeneticVarSq"]); }

                string garb;
                bool isLmmTabulatedFile = row.TryGetValue("nullBias", out garb);
                double nullBias;

                if (isLmmTabulatedFile)
                {
                    nullBias = double.Parse(row["nullBias"]);
                    phenIdAll.Add(row["phenId"]);

                    biasAll[m] = nullBias;//double.Parse(row["nullBias"]);

                    if (!haveCheckedForOptionalParams)
                    {
                        numFixedEffects = GetNumberOfFixedEffectsInTabFile(row);
                        numKernelWeights = GetNumberOfKernelWeightsInTabFile(row);
                        haveCheckedForOptionalParams = true;
                    }

                    DoubleArray fixedEffectParamsTmp = new DoubleArray(numFixedEffects, 1);
                    for (int f = 0; f < numFixedEffects; f++)
                    {
                        fixedEffectParamsTmp[f] = double.Parse(row["nullFixEffWght" + f]);
                    }
                    fixedEffectParams.Add(fixedEffectParamsTmp);

                }

                else
                {
                    phenIdAll.Add(row["transcriptClusterId"]);
                    List<double> fixedEffectParamsTmp = row["fixedEffectParams"].Split(',').Select(elt => double.Parse(elt)).ToList();
                    fixedEffectParams.Add(DoubleArray.From(fixedEffectParamsTmp));
                }

                DoubleArray kernelWeightsTmp = new DoubleArray(numKernelWeights, 1);
                for (int k = 0; k < numKernelWeights; k++)
                {
                    kernelWeightsTmp[k] = double.Parse(row["k" + k + "weight"]);
                }
                kernelWeights.Add(kernelWeightsTmp);

                SpecialFunctions.IncrementOrIntiDictionaryForCountingKeyValues<string>(numTimesPhenSeen, phenIdAll[m]);
                Helper.CheckCondition(snpIdAll[m] == "nullModelOnly", "only works for NullOnly model file right now");

                if (numTimesPhenSeen[phenIdAll[m]] > 1) throw new Exception("can only handle one of each phenotype in input file right now");
                m++;
            }

            //Helper.CheckCondition(m == M, "wrong number of phens observed: should be " + M + "but was" + m + 1);//doesn't work for alternative splicing            
            Helper.CheckCondition(m == rowCount, "wrong number of phens observed: should be " + rowCount + "but was" + m);

            return header;
        }

        public static string InitializeForClusterFileMerge(string outputFileOrNull, RangeCollection pieceIndexRange, int pieceCount, string fileNamePostfix, out string completedRowsFileName, out string finalOutputFileName, out string uniqueId)
        {
            uniqueId = ShoUtils.AddPostfixToFileName(new FileInfo(outputFileOrNull).Name, "*" + _dateStamp);
            completedRowsFileName = ShoUtils.AddPostfixToFileName(outputFileOrNull.Replace("{0}", ""), fileNamePostfix + "Sync");
            //add the piece index info to output file here
            finalOutputFileName = ShoUtils.AddPostfixToFileName(outputFileOrNull, fileNamePostfix + "Tab");
            outputFileOrNull = ShoUtils.AddPostfixToFileName(outputFileOrNull, fileNamePostfix + _specialString + "." + string.Format("{0}_{1}", pieceIndexRange, pieceCount));
            Bio.Util.FileUtils.CreateDirectoryForFileIfNeeded(outputFileOrNull);
            return outputFileOrNull;
        }

        private static int GetNumberOfKernelWeightsInTabFile(Dictionary<string, string> row)
        {
            int numKernelWeights = 0;
            bool doneCheckingNumberKernelWeights = false;
            int kernelNum = 0;
            while (!doneCheckingNumberKernelWeights)
            {
                string tmpVal;
                bool success = row.TryGetValue("k" + kernelNum + "weight", out tmpVal);
                if (!success)
                {
                    doneCheckingNumberKernelWeights = true;
                }
                else
                {
                    double garb = double.Parse(tmpVal);
                    numKernelWeights++;
                    kernelNum++;
                }
            }
            return numKernelWeights;
        }

        private static int GetNumberOfFixedEffectsInTabFile(Dictionary<string, string> row)
        {
            int numFixedEffects = 0;
            bool doneCheckingNumberFixedEffects = false;
            int fixEffectNum = 0;
            while (!doneCheckingNumberFixedEffects)
            {
                string tmpVal;
                bool success = row.TryGetValue("nullFixEffWght" + fixEffectNum, out tmpVal);
                if (!success)
                {
                    doneCheckingNumberFixedEffects = true;
                }
                else
                {
                    double garb = double.Parse(tmpVal);
                    numFixedEffects++;
                    fixEffectNum++;
                }
            }
            return numFixedEffects;
        }

        /// <summary>
        /// what to do when the last file is hit: merge them together
        /// </summary>
        /// <param name="outputFileOrNull"></param>
        /// <param name="fileNamePostfix"></param>
        /// <param name="completedRowsFileName"></param>
        /// <param name="completedRows"></param>
        public static void RunCONDITIONALTabulate(bool CLEANUP, string finalOutputFileName, string fileNamePostfix,
                                                  string completedRowsFileName, SynchronizedRangeCollectionWrapper completedRows, List<string> rowColKeys,
                                                  string filePatternToMerge, double numToDivideFinalMatrixBy, double diagFudge)
        {
            bool deleteUponSucceess = true;
            if (CLEANUP)
            //if ((completedRows.RangeCompletedByThisProcess()))  //"RangeCompletedByThisProcess" called first to cause a blocking flush
            {
                System.Console.WriteLine("all jobs are completed and I am sewing and cleaning");
                DirectoryInfo dirinfo = new FileInfo(finalOutputFileName).Directory;
                //string outputFilePattern = "*" + _uniqueStringForThisExp + "*" + ".txt";
                string outputFilePattern = "*" + filePatternToMerge + "*Q.txt";
                string mergeFile = ShoUtils.AddPostfixToFileName(completedRowsFileName, "TabM");
                Helper.CheckCondition(outputFilePattern != null);
                //_tabulateResultFileName = finalOutputFileName;
                //string finishedDir = new DirectoryInfo(finalOutputFileName).Parent.Parent.FullName + @"\Tabulated\";
                string finishedDir = new DirectoryInfo(finalOutputFileName).Parent.Parent.FullName + @"\Results\";
                Directory.CreateDirectory(finishedDir);
                string tabulateResultFileName = finishedDir + new FileInfo(finalOutputFileName).Name;

                System.Console.WriteLine("outputFilePattern=" + outputFilePattern);
                KeepTest<Dictionary<string, string>> keepTest = AlwaysKeep<Dictionary<string, string>>.GetInstance();//keep everything, using an instance of KeepTest

                if (ShoUtils.MergeShoMatrixFilesByAddition(
                                       dirinfo,
                                       outputFilePattern,
                                       tabulateResultFileName,
                                       keepTest,
                                       new List<KeepTest<Dictionary<string, string>>>(),
                                       true /* audit */, rowColKeys, rowColKeys, deleteUponSucceess, numToDivideFinalMatrixBy, diagFudge, false))
                {
                    System.Console.WriteLine("Done Tabulating ShoMatrixAdd, final results are in: " + dirinfo.Name + @"\" + tabulateResultFileName);

                    if (deleteUponSucceess) File.Delete(completedRowsFileName); ;// at this point we know we're the last one, so delete it.
                    //    /*                    if (File.Exists(skipFileName))
                    //                            File.Delete(skipFileName);
                    //    */
                }
                else
                {
                    Console.WriteLine("MergeShoMatrixFilesByAddition failed. Completed rows placed in {0} file.", completedRowsFileName);
                    SpecialFunctions.MoveAndReplace(tabulateResultFileName, ShoUtils.AddPostfixToFileName(completedRowsFileName, "_FAILED_MERGE_SHO_MATRIXES"));
                    tabulateResultFileName = ShoUtils.AddPostfixToFileName(completedRowsFileName, "_FAILED_TABULATE");
                    throw new Exception("MergeShomatrixFilesByAddition tabulation failed");
                }
            }
        }


        static string UsageMessage = "";

        /* OLD USEAGE STRING WHICH AT SOME POINT INCLUDED INTERNAL CALLS MADE BY THE GENERAL CALL ABOVE FOR FULL KERNEL LEARNING
          static string UsageMessage = @"Usage: snpEncoding(01 or 012) -phenDataFile -k1(kernelInitFile) -k2(otherKernelsToFixInPlace) -k3, etc."+
         " -outputFile -inputTabFile -impType -maxNumMsteps[2] -indOfKernelInit -computeLogLikelihood" +
         "-cluster (all the usual cluster stuff) -TaskCount -name -batchSize(number of genes)" +
         "-computeMstepOnCluster [for internal use only, not to be called by user]" +
         "-doFullKernelLearning(false) -maxFullLearnIt(200) -lmmTaskCount ---this means iterate between LMM.exe and eLMM.exe using SubmitAndWait (and means you don't need an inputTabFile to start" +
         "/*if doing multi-kernel, then kernels should be in same order as in Tab File, with indOfKernelInit specifying" +
         "which one is the initializing kernel (all others are fixed during learning). So -indOfKernelInit 0 means" +
         "k1, the first kernel, is the initializing one*/
    }
}
