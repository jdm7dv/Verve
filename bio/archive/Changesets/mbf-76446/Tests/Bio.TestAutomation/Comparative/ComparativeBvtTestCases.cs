// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * ComparativeBvtTestCases.cs
 * 
 *  This file contains the Comparative Bvt test cases.
 * 
***************************************************************************/
using Bio;
using Bio.Algorithms.Alignment;
using Bio.Algorithms.Assembly.Padena.Scaffold;
using Bio.IO.FastA;
using Bio.Util.Logging;
using System;
using System.IO;
using Bio.Algorithms.Assembly.Comparative;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bio.TestAutomation.Util;
using System.Globalization;

namespace Bio.TestAutomation.Algorithms.Assembly.Comparative
{
    /// <summary>
    /// Comparative Bvt test cases
    /// </summary>
    [TestClass]
    public class ComparativeBvtTestCases
    {
        #region Global Variables

        Utility utilityObj = new Utility(@"TestUtils\ComparativeTestData\ComparativeTestsConfig.xml");
        ASCIIEncoding encodingObj = new ASCIIEncoding();

        #endregion Global Variables

        #region Constructor

        /// <summary>
        /// Static constructor to open log and make other settings needed for test
        /// </summary>
        static ComparativeBvtTestCases()
        {
            Trace.Set(Trace.SeqWarnings);
            if (!ApplicationLog.Ready)
            {
                ApplicationLog.Open("mbf.automation.log");
            }
        }

        #endregion Constructor

        # region ComparativeBvtTestCases

        # region Step2 test cases

        /// <summary>
        /// Validates ResolveAmbiguity().
        /// The input reference sequences and reads are present in fasta files.
        /// Input data:One line Dna sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRepeatResolutionWithDnaFile()
        {
            RepeatResolution(Constants.RepeatResolution, true);
        }

        /// <summary>
        ///  Validates ResolveAmbiguity().
        ///  The input reference sequences and reads are taken from Configuration file.
        ///  Input data:One line Dna sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRepeatResolutionWithDnaString()
        {
            RepeatResolution(Constants.RepeatResolution, false);
        }

        /// <summary>
        ///  Validates ResolveAmbiguity().
        ///  Input: Paired Reads with zero errors and 1X coverage.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRepeatResolutionWithZeroErrorsAndPairedReads1X()
        {
            RepeatResolution(Constants.RepeatResolutionWithPairedReadsNode, true);
        }

        /// <summary>
        ///  Validates ResolveAmbiguity().
        ///  Input: Paired Reads with Errors and 1X coverage.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRepeatResolutionWithErrorsAndPairedReads1X()
        {
            RepeatResolution(Constants.RepeatResolutionWithErrorsInPairedReadsNode, true);
        }


        # endregion Step2 test cases

        # region Step3 test cases

        /// <summary>
        /// Validates RefineLayout().
        /// The input reference sequences and reads are present in fasta files.
        /// Input data:One line Dna sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRefineLayoutWithDnaFile()
        {
            RefineLayout(Constants.RefineLayout, true);
        }

        /// <summary>
        ///  Validates RefineLayout().
        ///  The input reference sequences and reads are taken from Configuration file.
        ///  Input data:One line Dna sequence.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRefineLayoutWithDnaString()
        {
            RefineLayout(Constants.RefineLayout, false);
        }

        /// <summary>
        ///  Validates RefineLayout().
        ///  Input: Paired Reads with zero errors and 1X coverage.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateRefineLayoutWithZeroErrorsAndPairedReads1X()
        {
            RefineLayout(Constants.RefineLayoutWithPairedReadsNode, true);
        }

        # endregion Step3 test cases

        # region Step4 test cases

        /// <summary>
        /// Validate GenerateConsensus().
        /// Inputs(references and reads) having one line sequences present in a file.        
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateGenerateConsensusForDnaSequencesFromFile()
        {
            ValidateGenerateConsensus(Constants.SimpleFastaNodeName, true);
        }

        /// <summary>
        /// This method will validate GenerateConsensus().
        /// Inputs(references and reads) with one line sequences .        
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateGenerateConsensusForDnaSequencesFromString()
        {
            ValidateGenerateConsensus(Constants.SimpleFastaNodeName, false);
        }

        /// <summary>
        ///  Validates GenerateConsensus().
        ///  Input: Paired Reads with zero errors and 1X coverage.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateGenerateConsensusWithZeroErrorsAndPairedReads1X()
        {
            ValidateGenerateConsensus(Constants.EColi500WithZeroError1XPairedReadsNode, true);
        }

        # endregion Step4 test cases

        # region Step 1-5 test cases

        # region With Errors.

        /// <summary>
        /// Validate Assemble().
        /// Inputs(references and reads) having one line sequences with reads having few Nucleotides added.        
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateAssembleWithAddedNucloetides()
        {
            ValidateComparativeAssembleMethod(Constants.FastaOneLineSequenceWithAdditionNode, false);
        }

        /// <summary>
        /// Validate Assemble().
        /// Inputs(references and reads) having one line sequences with reads having few Nucleotides deleted.        
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateAssembleWithDeletedNucleotide()
        {
            ValidateComparativeAssembleMethod(Constants.FastaOneLineSequenceWithDeletionNode, false);
        }

        /// <summary>
        /// Validate Assemble().
        /// Input: One line dna sequence and reads with .001 % errors.
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateAssembleWithOneLineSequenceWithErrors()
        {
            ValidateComparativeAssembleMethod(Constants.FastaWithOneLineSequenceNodeWithErrors, true);
        }


        # endregion With Errors.

        # region Without errors.

        /// <summary>
        /// Validate Assemble().
        /// Inputs(references and reads) having one line sequences present in a file.        
        /// Output: The reads are generated manually on reference file and the output of Assemble
        /// is expected to be the sequences in the reference file.
        /// Ref file: ATGCAGTA
        /// Read's file: ATGC,AGTA
        /// Expected Output: ATGCAGTA
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateAssembleWithOneLineSequence()
        {
            ValidateComparativeAssembleMethod(Constants.FastaWithOneLineSequenceNode, true);
        }

        /// <summary>
        /// Validate Assemble().
        /// Input: Zero error's 10x coverage, without paired reads
        /// Output: The reads are generated using Read Simulator on reference file and the output of Assemble
        /// is expected to be the sequences in the reference file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateAssembleForZeroErrors10XCoverageWithoutPairedReads()
        {
            ValidateComparativeAssembleMethod(Constants.SequenceWithZeroErrorsAnd10XCoverageNode, true);
        }

        /// <summary>
        /// Validate Assemble().
        /// Input: Zero error's 1x coverage, without paired reads
        /// Output: The reads are generated using Read Simulator on reference file and the output of Assemble
        /// is expected to be the sequences in the reference file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateAssembleForZeroErrors1XCoverageWithoutPairedReads()
        {
            ValidateComparativeAssembleMethod(Constants.SequenceWithZeroErrorsAnd1XCoverageNode, true);
        }

        /// <summary>
        /// Validate Assemble().
        /// Input: Zero error's 10x coverage, with paired reads
        /// Output: The reads are generated using SeqGen.exe on reference file and the output of Assemble
        /// is expected to be the sequences in the reference file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateAssembleForZeroErrors10XCoverageWithPairedReads()
        {
            ValidateComparativeAssembleMethod(Constants.SequenceWithZeroErrorsAnd10XCoveragePairedNode, true);
        }

        /// <summary>
        /// Validate Assemble().
        /// Input: Zero error's 1x coverage, with paired reads
        /// Output: The reads are generated using SeqGen.exe on reference file and the output of Assemble
        /// is expected to be the sequences in the reference file.
        /// </summary>
        [TestMethod]
        [Priority(0)]
        [TestCategory("Priority0")]
        public void ValidateAssembleForZeroErrors1XCoverageWithPairedReads()
        {
            ValidateComparativeAssembleMethod(Constants.SequenceWithZeroErrorsAnd1XCoveragePairedNode, true);
        }

        # endregion Without errors.

        # endregion Step 1-5 test cases

        # endregion ComparativeBvtTestCases

        # region Supporting methods

        /// <summary>
        /// Validates Assemble method .Step 1-5.        
        /// </summary>
        /// <param name="nodeName">Parent Node name in Xml</param>
        /// <param name="isFilePath">Sequence location.</param>
        public void ValidateComparativeAssembleMethod(string nodeName, bool isFilePath)
        {
            ComparativeGenomeAssembler assemble = new ComparativeGenomeAssembler();
            List<ISequence> referenceSeqList = new List<ISequence>();
            List<ISequence> readSeqList = new List<ISequence>();
            StringBuilder expectedSequence = new StringBuilder(utilityObj.xmlUtil.GetTextValue(nodeName,
                                    Constants.ExpectedSequenceNode));
            string alpName = utilityObj.xmlUtil.GetTextValue(nodeName,
                Constants.AlphabetNameNode);
            string LengthOfMUM = utilityObj.xmlUtil.GetTextValue(nodeName,
                     Constants.MUMLengthNode);
            string kmerLength = utilityObj.xmlUtil.GetTextValue(nodeName,
                     Constants.KmerLengthNode);

            string[] referenceSequences = null;
            IAlphabet seqAlphabet = Utility.GetAlphabet(alpName);

            if (isFilePath)
            {
                // Gets the reference sequence from the FastA file
                string filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode1);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Comparative BVT : Successfully validated the File Path '{0}'.", filePath));

                using (FastAParser parser = new FastAParser(filePath))
                {
                    IEnumerable<ISequence> referenceList = parser.Parse();

                    foreach (ISequence seq in referenceList)
                    {
                        referenceSeqList.Add(seq);
                    }
                }

                //Get the reads from configurtion file .
                readSeqList = GetReads(nodeName, true);
            }
            else
            {
                referenceSequences = utilityObj.xmlUtil.GetTextValues(nodeName,
                   Constants.ReferenceSequencesNode);

                for (int i = 0; i < referenceSequences.Length; i++)
                {
                    ISequence referSeq = new Sequence(seqAlphabet, encodingObj.GetBytes(referenceSequences[i]));
                    referenceSeqList.Add(referSeq);
                }

                //Get the reads from configurtion file .
                readSeqList = GetReads(nodeName, false);

            }

            assemble.LengthOfMum = int.Parse(LengthOfMUM);
            assemble.KmerLength = int.Parse(kmerLength);
            assemble.ScaffoldingEnabled = true;
            IEnumerable<ISequence> output = assemble.Assemble(referenceSeqList, readSeqList);
            StringBuilder longOutput = new StringBuilder();
            IEnumerable<string> outputStrings = output.Select(a => new string(a.Select(b => (char)b).ToArray())).OrderBy(c => c);
            foreach (string x in outputStrings)
                longOutput.Append(x);

            Assert.AreEqual(expectedSequence.ToString().ToUpperInvariant(), longOutput.ToString().ToUpperInvariant());
        }

        /// <summary>
        /// This method is Step 4 in Comparative Assembly.
        /// It validates GenerateConsensus method.
        /// </summary>
        /// <param name="nodeName">Parent Node name in Xml</param>
        /// <param name="isFilePath">Sequence location.</param>
        public void ValidateGenerateConsensus(string nodeName, bool isFilePath)
        {
            IList<IEnumerable<DeltaAlignment>> deltaValues = GetDeltaAlignment(nodeName, isFilePath);
            List<DeltaAlignment> result = new List<DeltaAlignment>();
            IList<IEnumerable<DeltaAlignment>> readDeltas = deltaValues.ToList();

            while (readDeltas.Count > 0)
            {
                IEnumerable<DeltaAlignment> curReadDeltas = readDeltas[0];
                readDeltas.RemoveAt(0); // remove currently processing item from the list

                foreach (DeltaAlignment curDelta in curReadDeltas)
                {
                    result.Add(curDelta);
                }
            }

            IEnumerable<ISequence> assembledOutput = ConsensusGeneration.GenerateConsensus(result.OrderBy(a => a.FirstSequenceStart));

            int index = 0;

            //Validate consensus sequences. 
            foreach (ISequence seq in assembledOutput)
            {
                string[] expectedValue = utilityObj.xmlUtil.GetTextValues(nodeName,
                                      Constants.ExpectedSequenceNode);
                string actualOutput = new string(seq.Select(a => (char)a).ToArray());
                Assert.AreEqual(expectedValue[index].ToUpperInvariant(), actualOutput.ToUpperInvariant());
                index++;
            }
        }

        /// <summary>
        /// Aligns reads to reference genome using NUCmer.
        /// </summary>
        /// <param name="nodeName">Name of parent Node which contains the data in xml.</param>
        /// <param name="isFilePath">Represents sequence is in a file or not.</param>
        /// <returns>Delta i.e. output from NUCmer</returns>      
        IList<IEnumerable<DeltaAlignment>> GetDeltaAlignment(string nodeName, bool isFilePath)
        {
            string[] referenceSequences = null;
            string[] searchSequences = null;

            List<ISequence> referenceSeqList = new List<ISequence>();
            List<ISequence> searchSeqList = new List<ISequence>();

            if (isFilePath)
            {
                // Gets the reference sequence from the FastA file
                string filePath = utilityObj.xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode1);

                Assert.IsNotNull(filePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Comparative BVT : Successfully validated the File Path '{0}'.", filePath));

                using (FastAParser parser = new FastAParser(filePath))
                {
                    IEnumerable<ISequence> referenceList = parser.Parse();

                    foreach (ISequence seq in referenceList)
                    {
                        referenceSeqList.Add(seq);
                    }

                    // Gets the query sequence from the FastA file
                    string queryFilePath = utilityObj.xmlUtil.GetTextValue(nodeName,
                        Constants.FilePathNode2);

                    Assert.IsNotNull(queryFilePath);
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                        "Comparative BVT : Successfully validated the File Path '{0}'.", queryFilePath));

                    using (FastAParser queryParser = new FastAParser(queryFilePath))
                    {
                        IEnumerable<ISequence> querySeqList = queryParser.Parse();

                        foreach (ISequence seq in querySeqList)
                        {
                            searchSeqList.Add(seq);
                        }
                    }
                }
            }
            else
            {
                // Gets the reference & search sequences from the configurtion file
                referenceSequences = utilityObj.xmlUtil.GetTextValues(nodeName,
                    Constants.ReferenceSequencesNode);
                searchSequences = utilityObj.xmlUtil.GetTextValues(nodeName,
                  Constants.SearchSequencesNode);

                IAlphabet seqAlphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName,
                       Constants.AlphabetNameNode));

                for (int i = 0; i < referenceSequences.Length; i++)
                {
                    ISequence referSeq = new Sequence(seqAlphabet, encodingObj.GetBytes(referenceSequences[i]));
                    referenceSeqList.Add(referSeq);
                }

                string[] seqArray = searchSequences.ElementAt(0).Split(',');

                for (int i = 0; i < seqArray.Length; i++)
                {
                    ISequence searchSeq = new Sequence(seqAlphabet, encodingObj.GetBytes(seqArray[i]));
                    searchSeqList.Add(searchSeq);
                }
            }

            NUCmer nucmerAligner = new NUCmer();

            string fixedSeparation = utilityObj.xmlUtil.GetTextValue(nodeName,
                     Constants.FixedSeparationNode);
            string minimumScore = utilityObj.xmlUtil.GetTextValue(nodeName,
                     Constants.MinimumScoreNode);
            string separationFactor = utilityObj.xmlUtil.GetTextValue(nodeName,
                     Constants.SeparationFactorNode);
            string LengthOfMUM = utilityObj.xmlUtil.GetTextValue(nodeName,
                     Constants.MUMLengthNode);

            nucmerAligner.FixedSeparation = int.Parse(fixedSeparation); ;
            nucmerAligner.MinimumScore = int.Parse(minimumScore);
            nucmerAligner.SeparationFactor = int.Parse(separationFactor);
            nucmerAligner.LengthOfMUM = int.Parse(LengthOfMUM);

            return nucmerAligner.GetDeltaAlignments(referenceSeqList, searchSeqList);

        }

        /// <summary>
        /// Validate Step 2 of Comparative assembly i.e. ResolveAmbiguity() method.
        /// Input sequence does not contain repeats.
        /// </summary>
        /// <param name="nodeName">Parent node in Xml</param>
        /// <param name="isFilePath">Represents sequence is in a file or not.</param>
        public void RepeatResolution(string nodeName, bool isFilePath)
        {
            IList<IEnumerable<DeltaAlignment>> deltaValues = GetDeltaAlignment(nodeName, isFilePath);
            List<DeltaAlignment> repeatDeltaValues = null;

            //Validate Resolve Ambiguity method.            
            repeatDeltaValues = RepeatResolver.ResolveAmbiguity(deltaValues);

            //Compare the delta's with the expected .
            Assert.IsTrue(CompareDeltaValues(nodeName, repeatDeltaValues));
        }

        /// <summary>
        /// Method to get the reads from file/xml.
        /// </summary>
        /// <param name="nodeName">Parent node in Xml</param>
        /// <param name="isFilePath">Represents sequence is in a file or not</param>
        /// <returns></returns>
        public List<ISequence> GetReads(string nodeName, bool isFilePath)
        {
            string[] reads = null;
            List<ISequence> readSeqList = new List<ISequence>();

            if (isFilePath)
            {
                // Gets the reads from the FastA file
                string readFilePath = utilityObj.xmlUtil.GetTextValue(nodeName,
                    Constants.FilePathNode2);

                Assert.IsNotNull(readFilePath);
                ApplicationLog.WriteLine(string.Format((IFormatProvider)null,
                    "Comparative BVT : Successfully validated the File Path '{0}'.", readFilePath));

                using (FastAParser queryParser = new FastAParser(readFilePath))
                {
                    IEnumerable<ISequence> querySeqList = queryParser.Parse();

                    foreach (ISequence seq in querySeqList)
                    {
                        readSeqList.Add(seq);
                    }
                }
            }
            else
            {
                // Gets the reads from the configurtion file                
                reads = utilityObj.xmlUtil.GetTextValues(nodeName,
                  Constants.SearchSequencesNode);

                IAlphabet seqAlphabet = Utility.GetAlphabet(utilityObj.xmlUtil.GetTextValue(nodeName,
                       Constants.AlphabetNameNode));

                string[] seqArray = reads.ElementAt(0).Split(',');

                for (int i = 0; i < seqArray.Length; i++)
                {
                    ISequence searchSeq = new Sequence(seqAlphabet, encodingObj.GetBytes(seqArray[i]));
                    readSeqList.Add(searchSeq);
                    readSeqList[i].ID = i.ToString();
                }
            }

            return readSeqList;
        }

        /// </summary>
        /// Validate Step 3 of Comparative assembly i.e. RefineLayout() method.
        /// <param name="nodeName">Parent node in Xml</param>
        /// <param name="isFilePath">Represents sequence is in a file or not.</param>
        public void RefineLayout(string nodeName, bool isFilePath)
        {
            List<DeltaAlignment> repeatDeltaValues = null;
            IList<IEnumerable<DeltaAlignment>> deltaValues = GetDeltaAlignment(nodeName, isFilePath);
            List<DeltaAlignment> result = new List<DeltaAlignment>();
            IList<IEnumerable<DeltaAlignment>> readDeltas = deltaValues.ToList();

            while (readDeltas.Count > 0)
            {
                IEnumerable<DeltaAlignment> curReadDeltas = readDeltas[0];
                readDeltas.RemoveAt(0); // remove currently processing item from the list

                foreach (DeltaAlignment curDelta in curReadDeltas)
                {
                    result.Add(curDelta);
                }
            }
            List<DeltaAlignment> orderedRepeatResolvedDeltas = result.OrderBy(a => a.FirstSequenceStart).ToList();

            //Validate RefineLayout().
            LayoutRefiner.RefineLayout(orderedRepeatResolvedDeltas);
            repeatDeltaValues = orderedRepeatResolvedDeltas;

            //Compare the delta's with the expected .
            Assert.IsTrue(CompareDeltaValues(nodeName, repeatDeltaValues));
        }

        /// <summary>
        /// Compares the values of Delta Alignments.
        /// </summary>
        /// <param name="nodeName">Parent node in Xml.</param>
        /// <param name="deltaValues">List of Delta Alignments which are to be compared.</param>
        /// <returns>True is the deltas are same,otherwise false.</returns>
        public bool CompareDeltaValues(string nodeName, IList<DeltaAlignment> deltaValues)
        {
            //Get the expected values from configurtion file .
            string[] firstSeqEnd = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FirstSequenceEndNode).Split(',');
            string[] firstSeqStart = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.FirstSequenceStartNode).Split(',');
            string[] deltaReferencePosition = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.DeltasNode).Split(',');
            string[] secondSeqStart = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SecondSequenceStart).Split(',');
            string[] secondSeqEnd = utilityObj.xmlUtil.GetTextValue(nodeName, Constants.SecondSequenceEndNode).Split(',');

            var deltas = deltaValues;
            for (int index = 0; index < deltas.Count(); index++)
            {
                if ((0 != string.Compare(firstSeqStart[index], deltas.ElementAt(index).FirstSequenceStart.ToString((IFormatProvider)null), StringComparison.CurrentCulture))
                   || (0 != string.Compare(firstSeqEnd[index], deltas.ElementAt(index).FirstSequenceEnd.ToString((IFormatProvider)null), StringComparison.CurrentCulture))
                   || (0 != string.Compare(deltaReferencePosition[index], deltas.ElementAt(index).DeltaReferencePosition.ToString((IFormatProvider)null), StringComparison.CurrentCulture))
                   || (0 != string.Compare(secondSeqStart[index], deltas.ElementAt(index).SecondSequenceStart.ToString((IFormatProvider)null), StringComparison.CurrentCulture))
                   || (0 != string.Compare(secondSeqEnd[index], deltas.ElementAt(index).SecondSequenceEnd.ToString((IFormatProvider)null), StringComparison.CurrentCulture)))
                {
                    Console.WriteLine(string.Format((IFormatProvider)null, "Delta Alignment : There is no match at '{0}'", index.ToString((IFormatProvider)null)));
                    ApplicationLog.WriteLine(string.Format((IFormatProvider)null, "Delta Alignment : There is no match at '{0}'", index.ToString((IFormatProvider)null)));
                    return false;
                }
            }

            return true;
        }

        # endregion Supporting methods
    }
}


